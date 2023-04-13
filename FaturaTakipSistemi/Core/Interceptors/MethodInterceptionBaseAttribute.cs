using Castle.DynamicProxy;

namespace FaturaTakip.Core.Interceptors
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class MethodInterceptionBaseAttribute : Attribute, IInterceptor
    {
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
