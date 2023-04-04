using FaturaTakip.Data.Models;
using FaturaTakip.Utils.Results;
using FaturaTakip.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FaturaTakip.Business.Interface
{
    public interface ILandlordService
    {
        Task<DataResult<Landlord>> GetLandlordByIdAsync(string userId);
        Task<DataResult<Landlord>> GetLandlordByIdAsync(int? landlordId);
        Task<DataResult<Landlord>> GetLandlordByGovermentId(string govermentId);
        Task<bool> IsLandlordExistAsync(int landlordId);
        Task<bool> IsLandlordExistAsync(string userId);
        Task<bool> IsLandlordRegisteredInHouseAsync(int landlordId);
        Task<bool> IsLandlordRegisteredInHouseAsync(string userId);
        Task<Result> AddLandlordAsync(Landlord landlordToAdd);
        Task<Result> DeleteLandlordAsync(int landlordId);
        Task<Result> UpdateLandlordAsync(Landlord landlord);
        Task<DataResult<IEnumerable<Landlord>>> GetAllLandlordsAsync();
        bool IsAnyLandlordExist();
        Task<DataResult<IEnumerable<LandlordSelectVM>>> GetLandlordsViewDataAsync();
        Task<DataResult<Landlord>> GetLoginedLandlord(HttpContext httpContext);

    }
}
