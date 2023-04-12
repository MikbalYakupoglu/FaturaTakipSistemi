using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using FaturaTakip.Core.Interceptors;
using FaturaTakip.Utils.Results;
using System.Threading.Tasks;

namespace FaturaTakip.Business.Aspects;

public class NotificationAspectAttribute : MethodInterceptions
{
    private readonly INotyfService _notyf;

    public NotificationAspectAttribute() => _notyf = ServiceTool.ServiceProvider.GetService<INotyfService>();

    protected override async Task OnAfterAsync(IInvocation invocation)
    {
        try
        {
            if (invocation.ReturnValue is SuccessResult result) // async olduğundan çalışmıyor
            {
                _notyf.Success(result.Message);
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
