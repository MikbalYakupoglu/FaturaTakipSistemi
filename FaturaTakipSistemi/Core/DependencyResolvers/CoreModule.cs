using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Containers;
using AspNetCoreHero.ToastNotification.Middlewares;
using AspNetCoreHero.ToastNotification.Notyf;
using AspNetCoreHero.ToastNotification.Notyf.Models;
using AspNetCoreHero.ToastNotification.Services;
using AspNetCoreHero.ToastNotification.Toastify.Models;
using FaturaTakip.Business.Aspects;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using Microsoft.Extensions.Options;

namespace FaturaTakip.Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection collection)
        {

        }
    }
}