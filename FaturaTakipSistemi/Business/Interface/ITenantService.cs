using FaturaTakip.Utils.Results;
using FaturaTakip.Data.Models;

namespace FaturaTakip.Business.Interface
{
    public interface ITenantService
    {
        Task<DataResult<Tenant>> GetTenantByIdAsync(string  userId);
        Task<DataResult<Tenant>> GetTenantByIdAsync(int?  tenantId);
        Task<DataResult<IEnumerable<Tenant>>> GetAllTenants();
        Task<bool> IsTenantExistAsync(int tenantId);
        Task<bool> IsTenantExistAsync(string userId);
        Task<bool> IsTenantRegisteredInHouseAsync(int tenantId);
        Task<bool> IsTenantRegisteredInHouseAsync(string userId);
        Task<Result> DeleteTenantAsync(int tenantId);
        Task<Result> UpdateTenantAsync(Tenant tenant);
        Task<bool> IsAnyTenantExistAsync();
        Task<Result> RemoveTenantAsync(Tenant tenant);  
    }
}
