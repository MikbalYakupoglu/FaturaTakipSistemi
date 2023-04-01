using FaturaTakip.Business.Interface;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.Utils.Results;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.Business.Concrete
{
    public class LandlordManager : ILandlordService
    {
        private readonly ILandlordDal _landlordDal;
        private readonly IApartmentService _apartmentService;
        private readonly InvoiceTrackContext _context;
        public LandlordManager(InvoiceTrackContext context,
            ILandlordDal landlordDal,
            IApartmentService apartmentService)
        {
            _context = context;
            _landlordDal = landlordDal;
            _apartmentService = apartmentService;
        }

        public async Task<Result> AddLandlordAsync(Landlord landlord)
        {
            if (await IsLandlordExistAsync(landlord.Id))
                return new ErrorResult("Ev Sahibi Zaten Kayıtlı.");

            await _landlordDal.AddAsync(landlord);
            return new SuccessResult();
        }

        public async Task<Result> RemoveLandlordAsync(int landlordId)
        {
            var landlordToDelete = await _landlordDal.GetAsync(l=> l.Id == landlordId);

            if (!IsLandlordExistAsync(landlordToDelete.Id).Result)
                return new ErrorResult("Ev Sahibi Bulunamadı.");

            if (IsLandlordRegisteredInHouseAsync(landlordToDelete.Id).Result)
                return new ErrorResult("Ev Sahibi Evde Oturuyor.");

            await _landlordDal.RemoveAsync(landlordToDelete);

            return new SuccessResult("Ev Sahibi Silindi.");
        }

        public async Task<Result> DeleteLandlordAsync(Landlord landlord)
        {
            var landlordToUpdate = await _landlordDal.GetAsync(l=> l.Id == landlord.Id);

            if (landlordToUpdate == null)
                return new ErrorResult("Ev Sahibi Bulunamadı.");

            landlordToUpdate.Name = landlord.Name;
            landlordToUpdate.LastName = landlord.LastName;
            landlordToUpdate.GovermentId = landlord.GovermentId;
            landlordToUpdate.YearOfBirth = landlord.YearOfBirth;
            landlordToUpdate.Phone = landlord.Phone;
            await _landlordDal.UpdateAsync(landlordToUpdate);

            return new SuccessResult();
        }

        public async Task<DataResult<IEnumerable<Landlord>>> GetAllLandlordsAsync()
        {
            var landlords = await _landlordDal.GetAllAsync();
            if(landlords == null)
                return new ErrorDataResult<IEnumerable<Landlord>>("Ev Sahibi Bulunamadı.");

            return new SuccessDataResult<IEnumerable<Landlord>>(landlords);
        }

        public async Task<DataResult<Landlord>> GetLandlordByIdAsync(string userId)
        {
            var landlord = await _landlordDal.GetAsync(l=> l.FK_UserId == userId);
            if (landlord == null)
                return new ErrorDataResult<Landlord>("Ev Sahibi Bulunamadı");

            return new SuccessDataResult<Landlord>(landlord);
        }

        public async Task<DataResult<Landlord>> GetLandlordByIdAsync(int? landlordId)
        {
            var landlord = await _landlordDal.GetAsync(l => l.Id == landlordId);
            if (landlord == null)
                return new ErrorDataResult<Landlord>("Ev Sahibi Bulunamadı");

            return new SuccessDataResult<Landlord>(landlord);
        }

        public bool IsAnyLandlordExist()
        {
            return _landlordDal.GetAllAsync().Result.Any();
        }

        public async Task<bool> IsLandlordExistAsync(int landlordId)
        {
            var landlord = await _landlordDal.GetAsync(l=> l.Id ==landlordId);
            return landlord != null ? true : false;
        }

        public async Task<bool> IsLandlordExistAsync(string userId)
        {
            var landlord = await _landlordDal.GetAsync(l => l.FK_UserId == userId);
            return landlord != null ? true : false;
        }

        public async Task<bool> IsLandlordRegisteredInHouseAsync(int landlordId)
        {
            var apartment = await _apartmentService.GetApartmentByLandlordIdAsync(landlordId);
            return apartment.Success;
        }

        public async Task<bool> IsLandlordRegisteredInHouseAsync(string userId)
        {
            var landlord = await GetLandlordByIdAsync(userId);
            return await IsLandlordRegisteredInHouseAsync(landlord.Data.Id);
        }

        public async Task<DataResult<Landlord>> GetLandlordByGovermentId(string govermentId)
        {
            var landlord = await _landlordDal.GetAsync(l => l.GovermentId == govermentId);
            if(landlord == null)
                return new ErrorDataResult<Landlord>("Ev Sahibi Bulunamadı.");

            return new SuccessDataResult<Landlord>(landlord);
        }
    }
}
