//using Castle.DynamicProxy;
//using FaturaTakip.Core.Interceptors;

//namespace Core.Utilities.Interceptors
//{
//    public abstract class MethodInterceptions : MethodInterceptionBaseAttribute
//    {
//        protected virtual void OnBefore(IInvocation invocation) { }
//        protected virtual void OnAfter(IInvocation invocation) { }
//        protected virtual void OnException(IInvocation invocation, System.Exception e) { }
//        protected virtual void OnSuccess(IInvocation invocation) { }
//        public override void Intercept(IInvocation invocation)
//        {
//            var isSuccess = true;

//            OnBefore(invocation);
//            try
//            {
//                invocation.Proceed();
//            }
//            catch (Exception e)
//            {
//                isSuccess = false;
//                OnException(invocation, e);
//                throw;
//            }
//            finally
//            {
//                if (isSuccess)
//                {
//                    OnSuccess(invocation);
//                }
//            }
//            OnAfter(invocation);
//        }
//    }
//}

using Castle.DynamicProxy;
using FaturaTakip.Core.Interceptors;
using System.Threading.Tasks;

namespace Core.Utilities.Interceptors
{
    public abstract class MethodInterceptions : MethodInterceptionBaseAttribute
    {
        protected virtual async Task OnBeforeAsync(IInvocation invocation) { }
        protected virtual async Task OnAfterAsync(IInvocation invocation) { }
        protected virtual async Task OnExceptionAsync(IInvocation invocation, System.Exception e) { }
        protected virtual async Task OnSuccessAsync(IInvocation invocation) { }

        public override async void Intercept(IInvocation invocation)
        {
            var isSuccess = true;

            await OnBeforeAsync(invocation);
            try
            {
                invocation.Proceed();
                if (invocation.Method.ReturnType == typeof(Task))
                {
                    await ((Task)invocation.ReturnValue);
                }
                else if (invocation.Method.ReturnType.IsGenericType &&
                    invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    await (Task)invocation.ReturnValue;
                }
                await OnSuccessAsync(invocation);
            }
            catch (Exception e)
            {
                isSuccess = false;
                await OnExceptionAsync(invocation, e);
                throw;
            }
            finally
            {
                if (isSuccess)
                {
                    await OnAfterAsync(invocation);
                }
            }
        }
    }
}
