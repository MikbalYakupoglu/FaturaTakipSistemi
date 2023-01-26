using FaturaTakip.Data.Models;
using FaturaTakip.Utils.Results;
using IResult = FaturaTakip.Utils.Results.IResult;

namespace FaturaTakip.Business.Abstract;

public interface IApartmentService
{
    Task<IDataResult<Apartment>> GetByIdAsync(int? id);
    Task<IDataResult<List<Apartment>>> GetAllAsync();

    Task<IDataResult<Apartment>> AddAsync(Apartment apartment);
    Task<IDataResult<Apartment>> UpdateAsync(Apartment apartment);
    Task<IDataResult<Apartment>> DeleteAsync(Apartment apartment);

}