using FaturaTakip.Data.Models;
using FaturaTakip.Utils.Results;
using FaturaTakip.ViewModels;
using System.Collections;

namespace FaturaTakip.Business.Interface
{
    public interface IApartmentService
    {
        Task<DataResult<ApartmentVM>> GetApartmentByIdWithLandlordAsync(int? id);
        Task<DataResult<IEnumerable<ApartmentVM>>> GetAllApartmentsWithLandlordsAsync();
        Task<DataResult<IEnumerable<Apartment>>> GetApartmentsByLandlordIdAsync(int? landlordId);
        bool IsAnyApartmentExist();
        Task<bool> IsApartmentExistAsync(int? id);
        Task<DataResult<Apartment>> GetApartmentWithInformationsAsync(Block block, short floor, int doorNumber);
        Task<Result> AddApartmentAsync(Apartment apartmentToAdd);
        Task<Result> RemoveApartmentAsync(int apartmentId);
        Task<Result> UpdateApartmentAsync(Apartment apartment);
        Task<DataResult<Apartment>> GetApartmentByIdAsync(int? id);
        Task<DataResult<IEnumerable<Apartment>>> GetLandlordsUntenantedApartmentsAsync(int? landlordId);

    }
}
