using Castle.DynamicProxy;
using FaturaTakip.Business.Aspects;
using System.Reflection;
using static FaturaTakip.Business.Aspects.NotificationAspectAttribute;

namespace FaturaTakip.Core.Interceptors
{
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes<NotyfMessageFilter>(true).ToList();
            var methodAttributes = type.GetMethod(method.Name).GetCustomAttributes<NotyfMessageFilter>(true);
            classAttributes.AddRange(methodAttributes);

            return classAttributes.OrderBy(x => x.Priority).ToArray();

        }
    }
}
