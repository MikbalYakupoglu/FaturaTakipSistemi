using FaturaTakip.Business.Interface;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.Utils.Results;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.Business.Concrete
{
    public class RentedApartmentManager : IRentedApartmentService
    {
        private readonly IRentedApartmentDal _rentedApartmentDal;
        private readonly InvoiceTrackContext _context;

        public RentedApartmentManager(InvoiceTrackContext context,
            IRentedApartmentDal rentedApartmentDal)
        {
            _context = context;
            _rentedApartmentDal = rentedApartmentDal;
        }

        public async Task<DataResult<RentedApartment>> GetRentedApartmentByTenantIdAsync(int? tenantId)
        {
            var rentedApartment = await _rentedApartmentDal.GetAsync(ra => ra.FKTenantId == tenantId);
            if (rentedApartment == null)
                return new ErrorDataResult<RentedApartment>("Kiralanmış Ev Bulunamadı.");

            return new SuccessDataResult<RentedApartment>(rentedApartment);
        }

        //public DataResult<IEnumerable<RentedApartment>> GetRentedApartmentsLandlords(int? id)
        //{
        //    var rentedApartmentsLandlords = _context.RentedApartments
        //    .Include(ra => ra.Apartment)
        //    .Where(ra => ra.Id == id);

        //    if(!rentedApartmentsLandlords.Any())
        //        return new ErrorDataResult<IEnumerable<RentedApartment>>("Kiralanmış ev bulunamadı.");

        //    return new SuccessDataResult<IEnumerable<RentedApartment>>(rentedApartmentsLandlords.ToList());
        //}

        public bool IsRentedApartmentExist(int tenantId)
        {
            return _rentedApartmentDal.GetAllAsync(ra=> ra.FKTenantId == tenantId).Result.Any();
        }
    }
}
