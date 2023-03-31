using FaturaTakip.Business.Interface;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.Utils.Results;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.Business.Concrete
{
    public class RentedApartmentManager : IRentedApartmentService
    {
        private readonly InvoiceTrackContext _context;

        public RentedApartmentManager(InvoiceTrackContext context
            )
        {
            _context = context;
        }

        public DataResult<IEnumerable<RentedApartment>> GetRentedApartmentsLandlords(int? id)
        {
            var rentedApartmentsLandlords = _context.RentedApartments
            .Include(ra => ra.Apartment)
            .Where(ra => ra.Id == id);

            if(!rentedApartmentsLandlords.Any())
                return new ErrorDataResult<IEnumerable<RentedApartment>>("Kiralanmış ev bulunamadı.");

            return new SuccessDataResult<IEnumerable<RentedApartment>>(rentedApartmentsLandlords.ToList());
        }
    }
}
