using FaturaTakip.Business.Interface;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.Data.Models.Abstract;
using FaturaTakip.Utils.Results;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.Business.Concrete
{
    public class TenantManager : ITenantService
    {
        private readonly InvoiceTrackContext _context;
        public TenantManager(InvoiceTrackContext context)
        {
            _context = context;
        }

        public async Task<Result> DeleteTenantAsync(int tenantId)
        {
            var tenantToDelete = await _context.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId);
            if (!IsTenantExistAsync(tenantToDelete.Id).Result)
                return new ErrorResult("Kiracı Bulunamadı.");

            if (IsTenantRegisteredInHouseAsync(tenantToDelete.Id).Result)
                return new ErrorResult("Kiracı Evde Oturuyor.");

            _context.Tenants.Remove(tenantToDelete);

            return new SuccessResult("Kiracı Silindi.");
        }

        public async Task<DataResult<Tenant>> GetTenantByUserIdAsync(string userId)
        {
            var tenant = await _context.Tenants.FirstOrDefaultAsync(t=> t.FK_UserId == userId);
            if(tenant == null)
                return new ErrorDataResult<Tenant>("Kiracı Bulunamadı");

            return new SuccessDataResult<Tenant>(tenant);
        }

        public async Task<bool> IsTenantExistAsync(int tenantId)
        {
            return await _context.Tenants.AnyAsync(t => t.Id == tenantId);
        }

        public async Task<bool> IsTenantExistAsync(string userId)
        {
            return await _context.Tenants.AnyAsync(t => t.FK_UserId == userId);
        }

        public async Task<bool> IsTenantRegisteredInHouseAsync(int tenantId)
        {
            return await _context.RentedApartments.AnyAsync(ra => ra.FKTenantId == tenantId);
        }
        public async Task<bool> IsTenantRegisteredInHouseAsync(string userId)
        {
            var tenant = await GetTenantByUserIdAsync(userId);
            return await _context.RentedApartments.AnyAsync(ra => ra.FKTenantId == tenant.Data.Id);
        }



    }
}
