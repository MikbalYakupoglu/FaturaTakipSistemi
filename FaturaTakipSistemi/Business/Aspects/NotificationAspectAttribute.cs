using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using FaturaTakip.Core.Interceptors;
using FaturaTakip.Utils;
using FaturaTakip.Utils.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Threading.Tasks;
using Castle.MicroKernel;
using System.Reflection;

namespace FaturaTakip.Business.Aspects;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class NotificationAspectAttribute : TypeFilterAttribute
{
    public NotificationAspectAttribute() : base(typeof(NotyfMessageFilter))
    {
        
    }
    public class NotyfMessageFilter : MethodInterceptions
    {
        private readonly INotyfService _notyf;

        public NotyfMessageFilter(INotyfService notyf)
        {
            _notyf = notyf;
        }

        protected override async Task OnAfterAsync(IInvocation invocation)
        {
            try
            {
                if (invocation.ReturnValue is SuccessResult successResult) // async olduğundan çalışmıyor
                {
                    _notyf.Success(successResult.Message);
                }
                else if (invocation.ReturnValue is ErrorResult errorResult)
                {
                    _notyf.Error(errorResult.Message);
                }
            }
            catch (Exception)
            {
                _notyf.Error("An unexpected error occurred.");
                throw;
            }
        }
    }

    //public NotificationAspectAttribute() // TODO : Parametresiz olarak instance alma yolu bakılacak
    //{
    //    //_notyf = ServiceTool.ServiceProvider.GetService<INotyfService>();
    //}

    //protected override async Task OnAfterAsync(IInvocation invocation)
    //{
    //    try
    //    {
    //        if (invocation.ReturnValue is SuccessResult successResult) // async olduğundan çalışmıyor
    //        {
    //            _notyf.Success(successResult.Message);
    //        }
    //        else if (invocation.ReturnValue is ErrorResult errorResult)
    //        {
    //            _notyf.Error(errorResult.Message);
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        _notyf.Error("An unexpected error occurred.");
    //        throw;
    //    }
    //}
}