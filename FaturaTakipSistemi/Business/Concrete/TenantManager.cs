using AutoMapper;
using FaturaTakip.Business.Interface;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.Utils.Results;
using FaturaTakip.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.Business.Concrete
{
    public class TenantManager : ITenantService
    {
        private readonly ITenantDal _tenantDal;
        private readonly IRentedApartmentService _rentedApartmentService;
        private readonly IMapper _mapper;
        public TenantManager(ITenantDal tenantDal,
            IRentedApartmentService rentedApartmentService,
            IMapper mapper)
        {
            _tenantDal = tenantDal;
            _rentedApartmentService = rentedApartmentService;
            _mapper = mapper;
        }

        public async Task<DataResult<IEnumerable<Tenant>>> GetAllTenantsAsync()
        {
            var tenants = await _tenantDal.GetAllAsync();

            if (!tenants.Any())
                return new ErrorDataResult<IEnumerable<Tenant>>(Enumerable.Empty<Tenant>(),"Herhangi Kiracı Bulunamadı.");

            return new SuccessDataResult<IEnumerable<Tenant>>(tenants);
        }

        public async Task<DataResult<Tenant>> GetTenantByIdAsync(int? tenantId)
        {
            var tenant = await _tenantDal.GetAsync(t=> t.Id == tenantId);
            if (tenant == null)
                return new ErrorDataResult<Tenant>("Kiracı Bulunamadı.");

            return new SuccessDataResult<Tenant>(tenant);
        }
    

        public async Task<DataResult<Tenant>> GetTenantByIdAsync(string userId)
        {
            var tenant = await _tenantDal.GetAsync(t=> t.FK_UserId == userId);
            if (tenant == null)
                return new ErrorDataResult<Tenant>("Kiracı Bulunamadı");

            return new SuccessDataResult<Tenant>(tenant);
        }

        public bool IsAnyTenantExist()
        {
            //return _tenantDal.GetAllAsync().Result.Any();
            return _tenantDal.IsAnyExist();
        }

        public async Task<bool> IsTenantExistAsync(int tenantId)
        {
            var tenant = await _tenantDal.GetAsync(t=> t.Id ==tenantId);
            return tenant != null ? true : false;
        }

        public async Task<bool> IsTenantExistAsync(string userId)
        {
            var tenant = await _tenantDal.GetAsync(t => t.FK_UserId == userId);
            return tenant != null ? true : false;
        }

        public async Task<bool> IsTenantRegisteredInHouseAsync(int tenantId)
        {
            var rentedApartment = await _rentedApartmentService.GetRentedApartmentByTenantIdAsync(tenantId);
            return rentedApartment.Success;
        }
        public async Task<bool> IsTenantRegisteredInHouseAsync(string userId)
        {
            var tenant = await GetTenantByIdAsync(userId);
            return await IsTenantRegisteredInHouseAsync(tenant.Data.Id);

        }
        public async Task<Result> AddTenantAsync(Tenant tenantToAdd)
        {
            if (await IsTenantExistAsync(tenantToAdd.Id))
                return new ErrorResult("Kiracı Bulunuyor.");

            await _tenantDal.AddAsync(tenantToAdd);
            return new SuccessResult();
        }


        public async Task<Result> DeleteTenantAsync(int tenantId)
        {
            var tenantToDelete = await _tenantDal.GetAsync(t => t.Id == tenantId);

            if (!IsTenantExistAsync(tenantToDelete.Id).Result)
                return new ErrorResult("Kiracı Bulunamadı.");

            if (IsTenantRegisteredInHouseAsync(tenantToDelete.Id).Result)
                return new ErrorResult("Kiracı Evde Oturuyor.");

            await _tenantDal.RemoveAsync(tenantToDelete);
            return new SuccessResult();

        }

        public async Task<Result> UpdateTenantAsync(Tenant tenant)
        {
            var tenantToUpdate = await _tenantDal.GetAsync(t => t.Id == tenant.Id);

            if(tenantToUpdate == null)
                return new ErrorResult("Kiracı Bulunamadı.");
                
            if (tenantToUpdate.Name != tenant.Name && !string.IsNullOrEmpty(tenant.Name))
            {
                tenantToUpdate.Name = tenant.Name;
            }
            if (tenantToUpdate.LastName != tenant.LastName && !string.IsNullOrEmpty(tenant.LastName))
            {
                tenantToUpdate.LastName = tenant.LastName;
            }
            if (tenantToUpdate.GovermentId != tenant.GovermentId && !string.IsNullOrEmpty(tenant.GovermentId))
            {
                tenantToUpdate.GovermentId = tenant.GovermentId;
            }
            if (tenantToUpdate.YearOfBirth != tenant.YearOfBirth && !string.IsNullOrEmpty(tenant.YearOfBirth.ToString()))
            {
                tenantToUpdate.YearOfBirth = tenant.YearOfBirth;
            }
            if (tenantToUpdate.Phone != tenant.Phone && !string.IsNullOrEmpty(tenant.Phone))
            {
                tenantToUpdate.Phone = tenant.Phone;
            }
            if (tenantToUpdate.LisencePlate != tenant.LisencePlate && !string.IsNullOrEmpty(tenant.LisencePlate))
            {
                tenantToUpdate.LisencePlate = tenant.LisencePlate;
            }

            await _tenantDal.UpdateAsync(tenantToUpdate);
            return new SuccessResult();
        }

        public async Task<DataResult<Tenant>> GetTenantByGovermentId(string govermentId)
        {
            var tenant = await _tenantDal.GetAsync(t=> t.GovermentId == govermentId);
            if (tenant == null)
                return new ErrorDataResult<Tenant>("Kiracı Bulunamadı.");

            return new SuccessDataResult<Tenant>(tenant);
        }

        public async Task<DataResult<IEnumerable<TenantSelectVM>>> GetTenantsViewDataAsync()
        {
            var tenantsResult = await GetAllTenantsAsync();
            if (!tenantsResult.Success)
                return new ErrorDataResult<IEnumerable<TenantSelectVM>>(tenantsResult.Message);

            return new SuccessDataResult<IEnumerable<TenantSelectVM>>(_mapper.Map<IEnumerable<TenantSelectVM>>(tenantsResult.Data));
        }

        public async Task<DataResult<IEnumerable<Tenant>>> GetTenantsByLandlordIdAsync(int landlordId)
        {
            var tenants = await _tenantDal.GetTenantsByLandlordIdAsync(landlordId);

            if (!tenants.Any())
                return new ErrorDataResult<IEnumerable<Tenant>>(Enumerable.Empty<Tenant>(),"Kiracı Bulunamadı.");

            return new SuccessDataResult<IEnumerable<Tenant>>(tenants);
        }
    }
}
