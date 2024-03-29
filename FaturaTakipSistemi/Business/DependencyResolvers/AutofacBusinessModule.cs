﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using FaturaTakip.Business.Aspects;
using FaturaTakip.Business.Concrete;
using FaturaTakip.Business.Interface;
using FaturaTakip.Core.Interceptors;

namespace FaturaTakip.Business.DependencyResolvers
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.Services.AddScoped<ITenantService, TenantManager>();
            //builder.Services.AddScoped<ILandlordService, LandlordManager>();
            //builder.Services.AddScoped<IApartmentService, ApartmentManager>();
            //builder.Services.AddScoped<IRentedApartmentService, RentedApartmentManager>();
            //builder.Services.AddScoped<IMessageService, MessageManager>();s


            builder.RegisterType<TenantManager>().As<ITenantService>().InstancePerLifetimeScope();
            builder.RegisterType<LandlordManager>().As<ILandlordService>().InstancePerLifetimeScope();
            builder.RegisterType<ApartmentManager>().As<IApartmentService>().InstancePerLifetimeScope();
            builder.RegisterType<RentedApartmentManager>().As<IRentedApartmentService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageManager>().As<IMessageService>().InstancePerLifetimeScope();

            //builder.RegisterType<NotificationAspectAttribute>().OnActivating(e =>
            //{
            //    var notyfService = e.Context.Resolve<INotyfService>();
            //    e.Instance.NotyfService = notyfService;
            //});

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
            .EnableInterfaceInterceptors(new ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector()
            }).SingleInstance();


        }
    }
}
