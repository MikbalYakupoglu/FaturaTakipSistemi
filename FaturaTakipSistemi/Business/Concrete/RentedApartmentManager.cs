using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using FaturaTakip.Business.Interface;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.DataAccess.Concrete;
using FaturaTakip.Utils;
using FaturaTakip.Utils.Results;
using FaturaTakip.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.Business.Concrete
{
    public class RentedApartmentManager : IRentedApartmentService
    {
        private readonly IMapper _mapper;
        private readonly IRentedApartmentDal _rentedApartmentDal;
        private readonly IApartmentDal _apartmentDal;


        public RentedApartmentManager(IMapper mapper,
            IRentedApartmentDal rentedApartmentDal,
            IApartmentDal apartmentDal)
        {
            _mapper = mapper;
            _rentedApartmentDal = rentedApartmentDal;
            _apartmentDal = apartmentDal;
        }



        public async Task<DataResult<IEnumerable<RentedApartmentVM>>> GetAllRentedApartmentsAsync()
        {
            var rentedApartments = await _rentedApartmentDal.GetAllRentedApartmentsWithApartmentsAndTenantsAsync();
            if(!rentedApartments.Success)
                return new ErrorDataResult<IEnumerable<RentedApartmentVM>>(Enumerable.Empty<RentedApartmentVM>() ,rentedApartments.Message);

            return new SuccessDataResult<IEnumerable<RentedApartmentVM>>(_mapper.Map<IEnumerable<RentedApartmentVM>>(rentedApartments.Data));
        }

        public async Task<DataResult<RentedApartmentVM>> GetRentedApartmentVMByIdAsync(int? id)
        {
            var rentedApartment = await _rentedApartmentDal.GetRentedApartmentByIdWithApartmentAndTenantAsync(id);

            if (!rentedApartment.Success)
                return new ErrorDataResult<RentedApartmentVM>(Messages.RentedApartmentNotFound);

            return new SuccessDataResult<RentedApartmentVM>(_mapper.Map<RentedApartmentVM>(rentedApartment.Data));
        }

        public async Task<DataResult<RentedApartment>> GetRentedApartmentByTenantIdAsync(int? tenantId)
        {
            var rentedApartment = await _rentedApartmentDal.GetAsync(ra => ra.FKTenantId == tenantId);
            if (rentedApartment == null)
                return new ErrorDataResult<RentedApartment>("Kiralanmış Ev Bulunamadı.");

            return new SuccessDataResult<RentedApartment>(rentedApartment);
        }

        public bool IsAnyRentedApartmentExist()
        {
            return _rentedApartmentDal.IsAnyExist();
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

        public async Task<Result> AddRentedApartmentAsync(RentedApartment rentedApartmentToAdd)
        {
            var rentedApartment = await _rentedApartmentDal.GetAsync(ra => ra.Id == rentedApartmentToAdd.Id);
            if (rentedApartment != null)
                return new ErrorResult("Ev Zaten Kiralanmış Durumda.");

            if(rentedApartmentToAdd.Status)
            {
                var apartmentToRent = await _apartmentDal.GetAsync(a => a.Id == rentedApartmentToAdd.FKApartmentId);
                apartmentToRent.Rented = true;
                await _apartmentDal.UpdateAsync(apartmentToRent);
            }

            rentedApartmentToAdd.RentTime = DateTime.Now;
            await _rentedApartmentDal.AddAsync(rentedApartmentToAdd);
            return new SuccessResult(Messages.AddSuccess);
        }
        public async Task<Result> DeleteRentedApartmentAsync(int rentedApartmentId)
        {
            var rentedApartmentToDelete = await GetRentedApartmentByIdAsync(rentedApartmentId);

            if (!rentedApartmentToDelete.Success)
                return new ErrorResult(Messages.RentedApartmentNotFound);


            if (rentedApartmentToDelete.Data.Status) // Şuan kiralanmış ev siliniyorsa, evin kiralanma durumu false olacak
            {
                var apartmentToRent = await _apartmentDal.GetAsync(a => a.Id == rentedApartmentToDelete.Data.FKApartmentId);
                apartmentToRent.Rented = false;
                await _apartmentDal.UpdateAsync(apartmentToRent);
            }

            await _rentedApartmentDal.RemoveAsync(rentedApartmentToDelete.Data);
            return new SuccessResult(Messages.RemoveSuccess);
        }

        public async Task<Result> UpdateRentedApartmentAsync(RentedApartment rentedApartment)
        {
            var rentedApartmentToUpdate = await _rentedApartmentDal.GetAsync(ra => ra.Id == rentedApartment.Id);

            if (rentedApartmentToUpdate == null)
                return new ErrorResult(Messages.RentedApartmentNotFound);

            rentedApartmentToUpdate.FKTenantId = rentedApartment.FKTenantId;
            rentedApartmentToUpdate.Status = rentedApartment.Status;

            if(rentedApartment.Status)
            {
                var apartmentToRent = await _apartmentDal.GetAsync(a => a.Id == rentedApartment.FKApartmentId);
                apartmentToRent.Rented = true;
                await _apartmentDal.UpdateAsync(apartmentToRent);
            }
            else
            {
                var apartmentToRent = await _apartmentDal.GetAsync(a => a.Id == rentedApartment.FKApartmentId);
                apartmentToRent.Rented = false;
                await _apartmentDal.UpdateAsync(apartmentToRent);
            }

            await _rentedApartmentDal.UpdateAsync(rentedApartmentToUpdate);
            return new SuccessResult(Messages.UpdateSuccess);
        }

        public async Task<DataResult<RentedApartment>> GetRentedApartmentByIdAsync(int? id)
        {
            var rentedApartment = await _rentedApartmentDal.GetRentedApartmentByIdWithApartmentAndTenantAsync(id);

            if (!rentedApartment.Success)
                return new ErrorDataResult<RentedApartment>(Messages.RentedApartmentNotFound);

            return new SuccessDataResult<RentedApartment>((rentedApartment.Data));
        }

        public async Task<DataResult<IEnumerable<RentedApartment>>> GetRentedApartmentsByLandlordIdAsync(int? landlordId)
        {
                var rentedApartments = await _rentedApartmentDal.GetRentedApartmentsByLandlordId(landlordId);

                if (!rentedApartments.Any())
                    return new ErrorDataResult<IEnumerable<RentedApartment>>(Enumerable.Empty<RentedApartment>(),"Kiralanmış Ev Bulunamadı.");

                return new SuccessDataResult<IEnumerable<RentedApartment>>(rentedApartments);
            
        }

        public async Task<DataResult<IEnumerable<RentedApartment>>> GetTenantsRentedApartmentsByTenantIdAsync(int? tenantId)
        {
            var rentedApartments = await _rentedApartmentDal.GetTenantsRentedApartmentsByTenantId(tenantId);

            if (!rentedApartments.Any())
                return new ErrorDataResult<IEnumerable<RentedApartment>>(Enumerable.Empty<RentedApartment>(),"Kiralanmış Ev Bulunamadı.");

            return new SuccessDataResult<IEnumerable<RentedApartment>>(rentedApartments);
        }

        public async Task<DataResult<RentedApartment>> GetRentedApartmentByApartmentIdAsync(int? apartmentId)
        {
            var rentedApartment = await _rentedApartmentDal.GetRentedApartmentByApartmentId(apartmentId);

            if (rentedApartment == null)
                return new ErrorDataResult<RentedApartment>("Kiralanmış Ev Bulunamadı.");

            return new SuccessDataResult<RentedApartment>(rentedApartment);
        }
    }
}
