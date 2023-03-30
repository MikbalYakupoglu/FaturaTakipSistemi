using FaturaTakip.Utils.Results;
using FaturaTakip.Data.Models;

namespace FaturaTakip.Business.Interface
{
    public interface ITenantService
    {
        Task<DataResult<Tenant>> GetTenantByUserIdAsync(string  userId);
        Task<bool> IsTenantExistAsync(int tenantId);
        Task<bool> IsTenantExistAsync(string userId);
        Task<bool> IsTenantRegisteredInHouseAsync(int tenantId);
        Task<bool> IsTenantRegisteredInHouseAsync(string userId);
        Task<Result> DeleteTenantAsync(int tenantId);
    }
}
