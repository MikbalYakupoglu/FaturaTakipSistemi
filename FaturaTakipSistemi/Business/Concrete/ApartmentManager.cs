using FaturaTakip.Business.Abstract;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.Utils;
using FaturaTakip.Utils.Results;
using IResult = FaturaTakip.Utils.Results.IResult;

namespace FaturaTakip.Business.Concrete;

public class ApartmentManager : IApartmentService
{
    private readonly IApartmentDal _apartmentDal;
    public ApartmentManager(IApartmentDal apartmentDal)
    {
        _apartmentDal = apartmentDal;
    }

    public async Task<IDataResult<Apartment>> GetByIdAsync(int? id)
    {
        var result = BusinessRules.Run(
            await IsApartmentExist(id)
        );

        if (!result.IsSuccess)
            return new ErrorDataResult<Apartment>(result.Message);

        return new SuccessDataResult<Apartment>(await _apartmentDal.GetAsync(a => a.Id == id));
    }

    public async Task<IDataResult<List<Apartment>>> GetAllAsync()
    {
        var result = BusinessRules.Run(
            await IsAnyApartmentExist()
        );

        if (!result.IsSuccess)
            return new ErrorDataResult<List<Apartment>>(result.Message);

        return new SuccessDataResult<List<Apartment>>(await _apartmentDal.GetAllAsync());
    }

    public async Task<IDataResult<Apartment>> AddAsync(Apartment apartment)
    {
        var result = BusinessRules.Run(
            await IsApartmentToBeAddedAlreadyExist(apartment)
            );

        if (!result.IsSuccess)
            return new ErrorDataResult<Apartment>(result.Message);

        await _apartmentDal.AddAsync(apartment);
        return new SuccessDataResult<Apartment>(apartment,"Apartment Added.");
    }

    public async Task<IDataResult<Apartment>> UpdateAsync(Apartment apartment)
    {
        var result = BusinessRules.Run(
            await IsApartmentExist(apartment.Id)
        );

        if (!result.IsSuccess)
            return new ErrorDataResult<Apartment>(result.Message);

        await _apartmentDal.UpdateAsync(apartment);
        return new SuccessDataResult<Apartment>(apartment, "Apartment Updated.");
    }

    public async Task<IDataResult<Apartment>> DeleteAsync(Apartment apartment)
    {
        var result = BusinessRules.Run(
            await IsApartmentExist(apartment.Id)
        );

        if (!result.IsSuccess)
            return new ErrorDataResult<Apartment>(result.Message);

        await _apartmentDal.DeleteAsync(apartment);
        return new SuccessDataResult<Apartment>(apartment, "Apartment Deleted.");
    }



    private async Task<IResult> IsApartmentExist(int? id)
    {
        var result = await _apartmentDal.GetAsync(a => a.Id == id);
        if (result == null)
            return new ErrorResult("Apartment Not Found");

        return new SuccessResult();
    }

    private async Task<IResult> IsAnyApartmentExist()
    {
        var result = await _apartmentDal.GetAllAsync();

        if (!result.Any())
            return new ErrorResult("Any Apartment Not Found");

        return new SuccessResult();
    }

    private async Task<IResult> IsApartmentToBeAddedAlreadyExist(Apartment apartment)
    {
        var result = await _apartmentDal.GetAsync(a => a.Block == apartment.Block && a.Floor == apartment.Floor && a.DoorNumber == apartment.DoorNumber);

        if (result != null)
            return new ErrorResult("Apartment Already Exist");

        return new SuccessResult();
    }
}