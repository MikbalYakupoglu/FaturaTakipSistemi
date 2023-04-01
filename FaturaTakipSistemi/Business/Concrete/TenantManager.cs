﻿using FaturaTakip.Business.Interface;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.Utils.Results;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.Business.Concrete
{
    public class TenantManager : ITenantService
    {
        private readonly ITenantDal _tenantDal;
        private readonly IRentedApartmentService _rentedApartmentService;
        public TenantManager(ITenantDal tenantDal,
            IRentedApartmentService rentedApartmentService)
        {
            _tenantDal = tenantDal;
            _rentedApartmentService = rentedApartmentService;

        }

        public async Task<DataResult<IEnumerable<Tenant>>> GetAllTenantsAsync()
        {
            var tenants = await _tenantDal.GetAllAsync();

            if (!tenants.Any())
                return new ErrorDataResult<IEnumerable<Tenant>>("Herhangi Kiracı Bulunamadı.");

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
            return _tenantDal.GetAllAsync().Result.Any();
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
        public async Task<Result> AddTenantAsync(Tenant tenant)
        {
            if (await IsTenantExistAsync(tenant.Id))
                return new ErrorResult("Kiracı Bulunuyor.");

            await _tenantDal.AddAsync(tenant);
            return new SuccessResult();
        }


        public async Task<Result> DeleteTenantAsync(Tenant tenant)
        {
            var tenantToDelete = await _tenantDal.GetAsync(t => t.Id == tenant.Id);

            if (!IsTenantExistAsync(tenantToDelete.Id).Result)
                return new ErrorResult("Kiracı Bulunamadı.");

            if (IsTenantRegisteredInHouseAsync(tenantToDelete.Id).Result)
                return new ErrorResult("Kiracı Evde Oturuyor.");

            await _tenantDal.RemoveAsync(tenantToDelete);
            return new SuccessResult("Kiracı Silindi.");

        }

        public async Task<Result> UpdateTenantAsync(Tenant tenant)
        {
            var tenantToUpdate = await _tenantDal.GetAsync(t => t.Id == tenant.Id);

            if(tenantToUpdate == null)
                return new ErrorResult("Kiracı Bulunamadı.");


            tenantToUpdate.Name = tenant.Name;
            tenantToUpdate.LastName = tenant.LastName;
            tenantToUpdate.GovermentId = tenant.GovermentId;
            tenantToUpdate.YearOfBirth = tenant.YearOfBirth;
            tenantToUpdate.Email = tenant.Email;
            tenantToUpdate.Phone = tenant.Phone;
            tenantToUpdate.LisencePlate = tenant.LisencePlate;

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
    }
}
