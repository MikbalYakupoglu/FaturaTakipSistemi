using AutoMapper;
using FaturaTakip.Business.Interface;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.Utils;
using FaturaTakip.Utils.Results;
using FaturaTakip.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace FaturaTakip.Business.Concrete
{
    public class LandlordManager : ILandlordService
    {
        private readonly ILandlordDal _landlordDal;
        private readonly IApartmentService _apartmentService;
        private readonly IMapper _mapper;
        public LandlordManager(ILandlordDal landlordDal,
            IApartmentService apartmentService,
            IMapper mapper)
        {
            _landlordDal = landlordDal;
            _apartmentService = apartmentService;
            _mapper = mapper;
        }

        public async Task<Result> AddLandlordAsync(Landlord landlordToAdd)
        {
            if (await IsLandlordExistAsync(landlordToAdd.Id))
                return new ErrorResult("Ev Sahibi Zaten Kayıtlı.");

            await _landlordDal.AddAsync(landlordToAdd);
            return new SuccessResult();
        }

        public async Task<Result> DeleteLandlordAsync(int landlordId)
        {
            var landlordToDelete = await _landlordDal.GetAsync(l=> l.Id == landlordId);

            if (!await IsLandlordExistAsync(landlordToDelete.Id))
                return new ErrorResult("Ev Sahibi Bulunamadı.");

            if (await IsLandlordRegisteredInHouseAsync(landlordToDelete.Id))
                return new ErrorResult("Ev Sahibi Eve Kayıtlı.");

            await _landlordDal.RemoveAsync(landlordToDelete);

            return new SuccessResult();
        }

        public async Task<Result> UpdateLandlordAsync(Landlord landlord)
        {
            var landlordToUpdate = await _landlordDal.GetAsync(l=> l.Id == landlord.Id);

            if (landlordToUpdate == null)
                return new ErrorResult("Ev Sahibi Bulunamadı.");

            if(landlordToUpdate.Name != landlord.Name && !string.IsNullOrEmpty(landlord.Name))
            {
                landlordToUpdate.Name = landlord.Name;
            }
            if (landlordToUpdate.LastName != landlord.LastName && !string.IsNullOrEmpty(landlord.LastName))
            {
                landlordToUpdate.LastName = landlord.LastName;
            }
            if (landlordToUpdate.GovermentId != landlord.GovermentId && !string.IsNullOrEmpty(landlord.GovermentId))
            {
                landlordToUpdate.GovermentId = landlord.GovermentId;
            }
            if (landlordToUpdate.YearOfBirth != landlord.YearOfBirth && !string.IsNullOrEmpty(landlord.YearOfBirth.ToString()))
            {
                landlordToUpdate.YearOfBirth = landlord.YearOfBirth;
            }
            if (landlordToUpdate.Phone != landlord.Phone && !string.IsNullOrEmpty(landlord.Phone))
            {
                landlordToUpdate.Phone = landlord.Phone;
            }

            await _landlordDal.UpdateAsync(landlordToUpdate);

            return new SuccessResult();
        }

        public async Task<DataResult<IEnumerable<Landlord>>> GetAllLandlordsAsync()
        {
            var landlords = await _landlordDal.GetAllAsync();
            if(landlords == null)
                return new ErrorDataResult<IEnumerable<Landlord>>(Enumerable.Empty<Landlord>(),"Ev Sahibi Bulunamadı.");

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
            var apartment = await _apartmentService.GetApartmentsByLandlordIdAsync(landlordId);
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

        public async Task<DataResult<IEnumerable<LandlordSelectVM>>> GetLandlordsViewDataAsync()
        {
            var landlordsResult = await GetAllLandlordsAsync();

            if (!landlordsResult.Success)
                return new ErrorDataResult<IEnumerable<LandlordSelectVM>>(landlordsResult.Message);

            return new SuccessDataResult<IEnumerable<LandlordSelectVM>>(_mapper.Map<IEnumerable<LandlordSelectVM>>(landlordsResult.Data));
        }

        public async Task<DataResult<Landlord>> GetLoginedLandlord(HttpContext httpContext)
        {
            var loginedLandlord = await _landlordDal.GetLoginedLandlordAsync(httpContext);

            if (loginedLandlord == null)
                return new ErrorDataResult<Landlord>("Giriş Yapmış Kullanıcı Bulunamadı.");

            return new SuccessDataResult<Landlord>(loginedLandlord);
        }

        public async Task<DataResult<IEnumerable<Landlord>>> GetLandlordByTenantIdAsync(int tenantId)
        {
            var landlords = await _landlordDal.GetLandlordsByTenantIdAsync(tenantId);

            if (!landlords.Any())
                return new ErrorDataResult<IEnumerable<Landlord>>(Enumerable.Empty<Landlord>(),"Ev Sahibi Bulunamadı.");

            return new SuccessDataResult<IEnumerable<Landlord>>(landlords);
        }
    }
}
