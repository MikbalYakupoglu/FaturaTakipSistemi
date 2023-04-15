using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Autofac.Core;
using Castle.DynamicProxy;
using FaturaTakip.Utils.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FaturaTakip.Core.Interceptors
{
    public abstract class MethodInterceptionBaseAttribute : Attribute, IInterceptor
    {
        public MethodInterceptionBaseAttribute() 
        {
            
        }
        public int Priority { get; set; }

        public virtual async void Intercept(IInvocation invocation)
        {
            
        }

        //public virtual void InterceptAsynchronous(IInvocation invocation)
        //{
            
        //}

        //public virtual void InterceptAsynchronous<TResult>(IInvocation invocation)
        //{
            
        //}

        //public virtual void InterceptSynchronous(IInvocation invocation)
        //{
            
        //}


    }
}