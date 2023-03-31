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
        public async Task<Result> DeleteTenantAsync(int tenantId)
        {
            using (InvoiceTrackContext _context = new InvoiceTrackContext())
            {
                var tenantToDelete = await _context.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId);
                if (!IsTenantExistAsync(tenantToDelete.Id).Result)
                    return new ErrorResult("Kiracı Bulunamadı.");

                if (IsTenantRegisteredInHouseAsync(tenantToDelete.Id).Result)
                    return new ErrorResult("Kiracı Evde Oturuyor.");

                _context.Tenants.Remove(tenantToDelete);

                return new SuccessResult("Kiracı Silindi.");
            }
        }

        public async Task<DataResult<IEnumerable<Tenant>>> GetAllTenants()
        {
            using (InvoiceTrackContext _context = new InvoiceTrackContext())
            {
                var tenants = await _context.Tenants.ToListAsync();

                if(!tenants.Any())
                    return new ErrorDataResult<IEnumerable<Tenant>>("Herhangi Kiracı Bulunamadı.");
                
                return new SuccessDataResult<IEnumerable<Tenant>>(tenants);
            }

        }

        public async Task<DataResult<Tenant>> GetTenantByIdAsync(int? tenantId)
        {
            using (InvoiceTrackContext _context = new InvoiceTrackContext())
            {
                var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId);
                if (tenant == null)
                    return new ErrorDataResult<Tenant>("Kiracı Bulunamadı.");

                return new SuccessDataResult<Tenant>(tenant);
            }

        }

        public async Task<DataResult<Tenant>> GetTenantByIdAsync(string userId)
        {
            using (InvoiceTrackContext _context = new InvoiceTrackContext())
            {
                var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.FK_UserId == userId);
                if (tenant == null)
                    return new ErrorDataResult<Tenant>("Kiracı Bulunamadı");

                return new SuccessDataResult<Tenant>(tenant);
            }

        }

        public async Task<bool> IsAnyTenantExistAsync()
        {
            using (InvoiceTrackContext _context = new InvoiceTrackContext())
            {
                return await _context.Tenants.AnyAsync();
            }
        }

        public async Task<bool> IsTenantExistAsync(int tenantId)
        {
            using (InvoiceTrackContext _context = new InvoiceTrackContext())
            {
                return await _context.Tenants.AnyAsync(t => t.Id == tenantId);

            }
        }

        public async Task<bool> IsTenantExistAsync(string userId)
        {
            using (InvoiceTrackContext _context = new InvoiceTrackContext())
            {
                return await _context.Tenants.AnyAsync(t => t.FK_UserId == userId);
            }
        }

        public async Task<bool> IsTenantRegisteredInHouseAsync(int tenantId)
        {
            using (InvoiceTrackContext _context = new InvoiceTrackContext())
            {
                return await _context.RentedApartments.AnyAsync(ra => ra.FKTenantId == tenantId);

            }
        }
        public async Task<bool> IsTenantRegisteredInHouseAsync(string userId)
        {
            using (InvoiceTrackContext _context = new InvoiceTrackContext())
            {
                var tenant = await GetTenantByIdAsync(userId);
                return await _context.RentedApartments.AnyAsync(ra => ra.FKTenantId == tenant.Data.Id);
            }

        }

        public async Task<Result> RemoveTenantAsync(Tenant tenant)
        {
            using (InvoiceTrackContext _context = new InvoiceTrackContext())
            {
                var tenantToDelete = await GetTenantByIdAsync(tenant.Id);

                if (!tenantToDelete.Success)
                    return new ErrorResult("Kiracı Bulunamadı.");

                _context.Tenants.Remove(tenant);
                await _context.SaveChangesAsync();
                return new SuccessResult();
            }

        }

        public async Task<Result> UpdateTenantAsync(Tenant tenant)
        {
            using (InvoiceTrackContext _context = new InvoiceTrackContext())
            {
                var tenantToUpdate = await _context.Tenants.FirstOrDefaultAsync(t => t.Id == tenant.Id);

                if (tenantToUpdate == null)
                    return new ErrorResult("Kiracı Bulunamadı.");

                tenantToUpdate.Name = tenant.Name;
                tenantToUpdate.LastName = tenant.LastName;
                tenantToUpdate.GovermentId = tenant.GovermentId;
                tenantToUpdate.YearOfBirth = tenant.YearOfBirth;
                tenantToUpdate.Email = tenant.Email;
                tenantToUpdate.Phone = tenant.Phone;
                tenantToUpdate.LisencePlate = tenant.LisencePlate;

                _context.Tenants.Update(tenantToUpdate);
                await _context.SaveChangesAsync();
                return new SuccessResult("Kiracı Başarıyla Güncellendi.");

            }

        }
    }
}
