using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Core;

namespace FaturaTakip.DataAccess.Abstract
{
    public interface ITenantDal : IEntityRepository<Tenant>
    {
        Task<IEnumerable<Tenant>> GetTenantsByLandlordIdAsync(int landlordId);
    }
}
