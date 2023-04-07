using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Core;
using FaturaTakip.Utils.Results;

namespace FaturaTakip.DataAccess.Abstract
{
    public interface ILandlordDal : IEntityRepository<Landlord>
    {
        Task<Landlord> GetLoginedLandlordAsync(HttpContext httpContext);
        Task<IEnumerable<Landlord>> GetLandlordsByTenantIdAsync(int tenantId);
    }
}
