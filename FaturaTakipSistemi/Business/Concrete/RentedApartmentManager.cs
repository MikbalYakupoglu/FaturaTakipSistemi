using AutoMapper;
using FaturaTakip.Business.Interface;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
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

        public RentedApartmentManager(IMapper mapper,
            IRentedApartmentDal rentedApartmentDal)
        {
            _mapper = mapper;
            _rentedApartmentDal = rentedApartmentDal;
        }



        public async Task<DataResult<IEnumerable<RentedApartmentVM>>> GetAllRentedApartmentsWithApartmentsAndTenantsAsync()
        {
            var rentedApartments = await _rentedApartmentDal.GetAllRentedApartmentsWithApartmentsAndTenantsAsync();
            if(!rentedApartments.Success)
                return new ErrorDataResult<IEnumerable<RentedApartmentVM>>(rentedApartments.Message);

            return new SuccessDataResult<IEnumerable<RentedApartmentVM>>(_mapper.Map<IEnumerable<RentedApartmentVM>>(rentedApartments.Data));
        }

        public async Task<DataResult<RentedApartmentVM>> GetRentedApartmentByIdWithApartmentAndTenantAsync(int? id)
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

        public bool IsRentedApartmentExist(int tenantId)
        {
            return _rentedApartmentDal.GetAllAsync(ra=> ra.FKTenantId == tenantId).Result.Any();
        }

        public async Task<Result> AddRentedApartmentAsync(RentedApartment rentedApartmentToAdd)
        {
            var apartment = await _rentedApartmentDal.GetAsync(ra => ra.Id == rentedApartmentToAdd.Id);

            if (apartment != null)
                return new ErrorResult("Ev Zaten Kiralanmış Durumda.");

            await _rentedApartmentDal.AddAsync(apartment);
            return new SuccessResult(Messages.AddSuccess);
        }
        public Task<Result> DeleteRentedApartmentAsync(int rentedApartmentId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateRentedApartmentAsync(RentedApartment rentedApartment)
        {
            throw new NotImplementedException();
        }
    }
}
