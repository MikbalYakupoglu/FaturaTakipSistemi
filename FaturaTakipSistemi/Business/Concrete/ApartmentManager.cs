using FaturaTakip.Business.Interface;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.Utils.Results;

namespace FaturaTakip.Business.Concrete
{
    public class ApartmentManager : IApartmentService
    {
        private readonly IApartmentDal _apartmentDal;
        public ApartmentManager(IApartmentDal apartmentDal)
        {
            _apartmentDal = apartmentDal;
        }

        public async Task<DataResult<IEnumerable<Apartment>>> GetAllApartmentsWithLandlordsAsync()
        {
            return await _apartmentDal.GetAllApartmentsWithLandlordsAsync();
        }

        public async Task<DataResult<Apartment>> GetApartmentByLandlordIdAsync(int? landlordId)
        {
            var apartment = await _apartmentDal.GetAsync(a=> a.FKLandlordId == landlordId);
            if(apartment == null)
                return new ErrorDataResult<Apartment>("Ev Bulunamadı.");

            return new SuccessDataResult<Apartment>(apartment);
        }
    }
}
