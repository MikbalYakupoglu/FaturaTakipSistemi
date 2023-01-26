using System.Globalization;
using System.Reflection;
using FaturaTakip.Data;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.DataAccess.Concrete.EntityFramework;
using FaturaTakip.Resources;
using FaturaTakip.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region DependencyResolvers
builder.Services.AddSingleton<CommonLocalizationService>();
builder.Services.AddSingleton<IApartmentDal, EfApartmentDal>();
builder.Services.AddSingleton<IDebtDal, EfDebtDal>();
builder.Services.AddSingleton<ILandlordDal, EfLandlordDal>();
builder.Services.AddSingleton<IMessageDal, EfMessageDal>();
builder.Services.AddSingleton<IPaymentDal, EfPaymentDal>();
builder.Services.AddSingleton<IRentedApartmentDal, EfRentedApartmentDal>();
builder.Services.AddSingleton<ITenantDal, EfTenantDal>();


#endregion

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<InvoiceTrackContext>(options =>
    options.UseSqlServer(connectionString));

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
//builder.Services.AddMvc()
//    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
//    .AddDataAnnotationsLocalization();

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

//var cultures = new List<CultureInfo> {
//    new ("en"),
//    new ("tr")
//};


app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseRequestLocalization(options => {
//    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
//    options.SupportedCultures = cultures;
//    options.SupportedUICultures = cultures;
//});

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
app.MapRazorPages();

app.Run();