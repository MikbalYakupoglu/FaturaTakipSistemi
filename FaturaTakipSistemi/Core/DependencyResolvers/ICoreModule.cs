using System.Security.Cryptography;

namespace FaturaTakip.Core.DependencyResolvers
{
    public interface ICoreModule
    {
        void Load(IServiceCollection collection);
    }
}
