using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.DataAccess.Core;
using FaturaTakip.Utils;
using FaturaTakip.Utils.Results;
using FaturaTakip.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.DataAccess.Concrete
{
    public class EfRentedApartmentDal : EfEntityRepositoryBase<RentedApartment, InvoiceTrackContext>, IRentedApartmentDal
    {
        public async Task<DataResult<IEnumerable<RentedApartment>>> GetAllRentedApartmentsWithApartmentsAndTenantsAsync()
        {
            using (var context = new InvoiceTrackContext())
            {
                var rentedApartments = await context.RentedApartments
                    .Include(ra => ra.Apartment)
                    .Include(ra => ra.Tenant)
                    .ToListAsync();

                if (!rentedApartments.Any())
                    return new ErrorDataResult<IEnumerable<RentedApartment>>(Messages.RentedApartmentNotFound);

                return new SuccessDataResult<IEnumerable<RentedApartment>>(rentedApartments);
            }
        }

        public async Task<DataResult<RentedApartment>> GetRentedApartmentByIdWithApartmentAndTenantAsync(int? id)
        {
            using (var context = new InvoiceTrackContext())
            {
                var rentedApartment = await context.RentedApartments
                    .Include(r => r.Apartment)
                    .Include(r => r.Tenant)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (rentedApartment == null)
                    return new ErrorDataResult<RentedApartment>(Messages.RentedApartmentNotFound);

                return new SuccessDataResult<RentedApartment>(rentedApartment);
            }
        }
    }
}
