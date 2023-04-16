using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using FaturaTakip.Business.Aspects;
using FaturaTakip.Business.Interface;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.Utils;
using FaturaTakip.Utils.Results;
using FaturaTakip.ViewModels;
using System.Collections;

namespace FaturaTakip.Business.Concrete
{
    public class ApartmentManager : IApartmentService
    {
        private readonly IApartmentDal _apartmentDal;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;
        public ApartmentManager(IApartmentDal apartmentDal,
            IMapper mapper,
            INotyfService notyf
            )
        {
            _apartmentDal = apartmentDal;
            _mapper = mapper;
            _notyf = notyf;
        }

        public async Task<DataResult<IEnumerable<ApartmentVM>>> GetAllApartmentsWithLandlordsAsync()
        {
            var apartmentsResult = await _apartmentDal.GetAllApartmentsWithLandlordsAsync();
            if(!apartmentsResult.Success)
                return new ErrorDataResult<IEnumerable<ApartmentVM>>(Enumerable.Empty<ApartmentVM>(),apartmentsResult.Message);

            return new SuccessDataResult<IEnumerable<ApartmentVM>>(_mapper.Map<IEnumerable<ApartmentVM>>(apartmentsResult.Data));
        }

        public async Task<DataResult<IEnumerable<Apartment>>> GetApartmentsByLandlordIdAsync(int? landlordId)
        {
            var apartments = await _apartmentDal.GetAllAsync(a=> a.FKLandlordId == landlordId);
            if(apartments == null)
                return new ErrorDataResult<IEnumerable<Apartment>>("Ev Bulunamadı.");

            
            return new SuccessDataResult<IEnumerable<Apartment>>(apartments);
        }

        public async Task<DataResult<ApartmentVM>> GetApartmentByIdWithLandlordAsync(int? id)
        {
            var apartment = await _apartmentDal.GetApartmentByIdWithLandlordAsync(id);

            if (!apartment.Success)
                return new ErrorDataResult<ApartmentVM>("Ev Bulunamadı.");

            return new SuccessDataResult<ApartmentVM>(_mapper.Map<ApartmentVM>(apartment.Data));
        }

        public bool IsAnyApartmentExist()
        {
            return _apartmentDal.IsAnyExist();
        }

        public async Task<DataResult<Apartment>> GetApartmentWithInformationsAsync(Block block, short floor, int doorNumber)
        {
            var apartment = await _apartmentDal.GetAsync(a => a.Block == block && a.Floor == floor && a.DoorNumber == doorNumber);

                if (apartment == null)
                    return new ErrorDataResult<Apartment>("Ev Bulunamadı.");

                return new SuccessDataResult<Apartment>(apartment);
            
        }

        
        public async Task<Result> AddApartmentAsync(Apartment apartmentToAdd)
        {
            var apartment = await _apartmentDal.GetAsync(a => a.Id == apartmentToAdd.Id);

            if (apartment != null)
                return new ErrorResult("Ev Zaten Kayıtlı.");

            if (GetApartmentWithInformationsAsync(apartmentToAdd.Block, apartmentToAdd.Floor, apartmentToAdd.DoorNumber).Result.Success)
                return new ErrorResult("Ev Zaten Kayıtlı");

            apartmentToAdd.Rented = false;

            await _apartmentDal.AddAsync(apartmentToAdd);
            return new SuccessResult(Messages.AddSuccess);

        }

        public async Task<Result> DeleteApartmentAsync(int apartmentId)
        {
            var apartmentToDelete = await _apartmentDal.GetAsync(a => a.Id == apartmentId);
            if (apartmentToDelete == null)
                return new ErrorResult("Ev Bulunamadı.");

            if (apartmentToDelete.Rented)
                return new ErrorResult("Kiralanmış Ev Silinemez.");

            await _apartmentDal.RemoveAsync(apartmentToDelete);
            return new SuccessResult(Messages.RemoveSuccess);
        }

        //[NotificationAspect]
        public async Task<Result> UpdateApartmentAsync(Apartment apartment)
        {
            var apartmentToUpdate = await _apartmentDal.GetAsync(a => a.Id == apartment.Id);

            if (apartment == null)
                return new ErrorResult("Ev Bulunamadı.");

            if(apartment.Type == Data.Models.Type.None)
                return new ErrorResult(Messages.TypeCannotBeNone);

            apartmentToUpdate.FKLandlordId = apartment.FKLandlordId;
            apartmentToUpdate.Block = apartment.Block;
            apartmentToUpdate.Floor = apartment.Floor;
            apartmentToUpdate.DoorNumber = apartment.DoorNumber;
            apartmentToUpdate.Type = apartment.Type;
            apartmentToUpdate.RentPrice = apartment.RentPrice;

            await _apartmentDal.UpdateAsync(apartmentToUpdate);
            return new SuccessResult(Messages.UpdateSuccess);
        }

        //public Result UpdateApartment(Apartment apartment)
        //{
        //    var apartmentToUpdate = _apartmentDal.GetAsync(a => a.Id == apartment.Id).Result;

        //    if (apartment == null)
        //        return new ErrorResult("Ev Bulunamadı.");

        //    if (apartment.Type == Data.Models.Type.None)
        //        return new ErrorResult(Messages.TypeCannotBeNone);

        //    apartmentToUpdate.FKLandlordId = apartment.FKLandlordId;
        //    apartmentToUpdate.Block = apartment.Block;
        //    apartmentToUpdate.Floor = apartment.Floor;
        //    apartmentToUpdate.DoorNumber = apartment.DoorNumber;
        //    apartmentToUpdate.Type = apartment.Type;
        //    apartmentToUpdate.RentPrice = apartment.RentPrice;

        //    _apartmentDal.UpdateAsync(apartmentToUpdate);
        //    return new SuccessResult(Messages.UpdateSuccess);
        //}

        public async Task<DataResult<Apartment>> GetApartmentByIdAsync(int? id) // Apartment -> Edit Custom Metotdan dolayı ViewModele convert edilmedi.
        {
            var apartment = await _apartmentDal.GetAsync(a=> a.Id == id);
            if (apartment == null)
                return new ErrorDataResult<Apartment>("Ev Bulunamadı.");

            return new SuccessDataResult<Apartment>(apartment);
        }

        public async Task<bool> IsApartmentExistAsync(int? id)
        {
            var apartment = await _apartmentDal.GetAsync(a=> a.Id == id);
            return apartment != null ? true : false;
        }

        public async Task<DataResult<IEnumerable<Apartment>>> GetLandlordsUntenantedApartmentsAsync(int? landlordId)
        {
            var apartments = await _apartmentDal.GetAllAsync(a => a.FKLandlordId == landlordId);
            var untenantedApartments = apartments.Where(a=> a.Rented == false).ToList();



            if (!untenantedApartments.Any())
                return new ErrorDataResult<IEnumerable<Apartment>>("Kiralanmamış Ev Bulunamadı.");

            return new SuccessDataResult<IEnumerable<Apartment>>(untenantedApartments);

        }

        public async Task<DataResult<IEnumerable<ApartmentVM>>> GetAllApartmentsAsync()
        {
            var apartments = await _apartmentDal.GetAllAsync();

            if (!apartments.Any())
                return new ErrorDataResult<IEnumerable<ApartmentVM>>(Enumerable.Empty<ApartmentVM>(),"Ev Bulunamadı.");

            return new SuccessDataResult<IEnumerable<ApartmentVM>>(_mapper.Map<IEnumerable<ApartmentVM>>(apartments));
        }
    }
}
