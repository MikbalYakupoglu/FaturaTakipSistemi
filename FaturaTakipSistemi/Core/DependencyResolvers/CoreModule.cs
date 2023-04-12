using System.Diagnostics;

namespace FaturaTakip.Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection collection)
        {
            collection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
