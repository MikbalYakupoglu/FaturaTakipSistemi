using System.Globalization;
using System.Reflection;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using FaturaTakip.Business.Aspects;
using FaturaTakip.Business.Concrete;
using FaturaTakip.Business.Interface;
using FaturaTakip.Core.DependencyResolvers;
using FaturaTakip.Core.Extensions;
using FaturaTakip.Data;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.DataAccess.Concrete;
using FaturaTakip.Resources;
using FaturaTakip.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Core;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Castle.DynamicProxy;
using FaturaTakip.Business.DependencyResolvers;

var builder = WebApplication.CreateBuilder(args);

//Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
    builder.RegisterModule(new AutofacBusinessModule()));

// Add services to the container.

builder.Services.AddTransient<NotificationAspectAttribute>();

builder.Services.AddSingleton<CommonLocalizationService>();

builder.Services.AddScoped<ITenantDal, EfTenantDal>();
builder.Services.AddScoped<ILandlordDal, EfLandlordDal>();
builder.Services.AddScoped<IApartmentDal, EfApartmentDal>();
builder.Services.AddScoped<IRentedApartmentDal, EfRentedApartmentDal>();
builder.Services.AddScoped<IDebtDal, EfDebtDal>();
builder.Services.AddScoped<IMessageDal, EfMessageDal>();
builder.Services.AddScoped<IPaymentDal, EfPaymentDal>();


builder.Services.AddDbContext<InvoiceTrackContext>();

builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
    config.HasRippleEffect = true;
});

builder.Services.AddDependencyResolvers(new ICoreModule[]
{
    new CoreModule()
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());




var container = new WindsorContainer();
var containerBuilder = new ContainerBuilder();

// Register all classes in the current AppDomain that have methods decorated with the NotificationAspectAttribute
var assemblies = AppDomain.CurrentDomain.GetAssemblies();
foreach (var assembly in assemblies)
{
    container.Register(
        Classes.FromAssembly(assembly)
            .BasedOn<object>()
            .If(type => type.GetMethods().Any(m => m.GetCustomAttributes(typeof(NotificationAspectAttribute), true).Length > 0))
            .Configure(configurer =>
                configurer
                    .Interceptors(InterceptorReference.ForType<NotificationAspectAttribute>())
                    
    ).LifestyleTransient());
}

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<InvoiceTrackContext>(options =>
//    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<InvoiceTrackUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;

    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequiredLength = 3;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<InvoiceTrackContext>();



// Globalization : https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-6.0
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddMvc()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
{
    options.DataAnnotationLocalizerProvider = (type, factory) =>
    {
        var assemblyName = new AssemblyName(typeof(CommonResources).GetTypeInfo().Assembly.FullName);
        return factory.Create(nameof(CommonResources), assemblyName.Name);
    };
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("tr")
    };
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new CookieRequestCultureProvider(),
        new QueryStringRequestCultureProvider()
    };

});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseAuthentication();
app.UseAuthorization();

app.UseNotyf();

app.MapRazorPages();

app.Run();