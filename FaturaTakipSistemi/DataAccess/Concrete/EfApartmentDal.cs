using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.DataAccess.Core;
using FaturaTakip.Utils.Results;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.DataAccess.Concrete
{
    public class EfApartmentDal : EfEntityRepositoryBase<Apartment, InvoiceTrackContext>, IApartmentDal
    {
        public async Task<DataResult<IEnumerable<Apartment>>> GetAllApartmentsWithLandlordsAsync()
        {
            using(var context = new InvoiceTrackContext())
            {
                var apartments = await context.Apartments
                    .Include(a => a.Landlord)
                    .ToListAsync();

                if(!apartments.Any())
                    return new ErrorDataResult<IEnumerable<Apartment>>("Ev Bulunamadı.");

                return new SuccessDataResult<IEnumerable<Apartment>>(apartments);
            }
        }

        public async Task<DataResult<Apartment>> GetApartmentByIdWithLandlordAsync(int? id)
        {
            using(var context = new InvoiceTrackContext())
            {
                var apartment = await context.Apartments
                    .Include(a => a.Landlord)
                    .SingleOrDefaultAsync(a => a.Id == id);

                if (apartment == null)
                    return new ErrorDataResult<Apartment>("Ev Bulunamadı.");

                return new SuccessDataResult<Apartment>(apartment);
            }
        }
    }
}
