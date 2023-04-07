using FaturaTakip.Controllers;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.DataAccess.Core;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.DataAccess.Concrete
{
    public class EfTenantDal : EfEntityRepositoryBase<Tenant, InvoiceTrackContext>, ITenantDal
    {
        public async Task<IEnumerable<Tenant>> GetTenantsByLandlordIdAsync(int landlordId)
        {
            using(var context = new InvoiceTrackContext())
            {
                //var tenants = await context.RentedApartments
                //    .Include(ra => ra.Apartment)
                //    .Include(ra => ra.Tenant)
                //    .Where(ra => ra.Apartment.FKLandlordId == landlordId)
                //    .Select(ra => ra.Tenant)
                //    .ToListAsync();
                var tenants = await context.Messages
                    .Include(m => m.Tenant)
                    .Where(m => m.FKLandlordId == landlordId)
                    .Select(m => m.Tenant)
                    .ToListAsync();

                return tenants;
            }
        }
    }
}
