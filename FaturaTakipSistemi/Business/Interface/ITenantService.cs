using FaturaTakip.Utils.Results;
using FaturaTakip.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using FaturaTakip.ViewModels;

namespace FaturaTakip.Business.Interface
{
    public interface ITenantService
    {
        Task<DataResult<Tenant>> GetTenantByIdAsync(string  userId);
        Task<DataResult<Tenant>> GetTenantByIdAsync(int?  tenantId);
        Task<DataResult<Tenant>> GetTenantByGovermentId(string govermentId);
        Task<DataResult<IEnumerable<Tenant>>> GetAllTenantsAsync();
        Task<bool> IsTenantExistAsync(int tenantId);
        Task<bool> IsTenantExistAsync(string userId);
        Task<bool> IsTenantRegisteredInHouseAsync(int tenantId);
        Task<bool> IsTenantRegisteredInHouseAsync(string userId);
        Task<Result> AddTenantAsync(Tenant tenantToAdd);
        Task<Result> DeleteTenantAsync(int tenantId);  
        Task<Result> UpdateTenantAsync(Tenant tenant);
        bool IsAnyTenantExist();
        Task<DataResult<IEnumerable<TenantSelectVM>>> GetTenantsViewDataAsync();
    }
}
