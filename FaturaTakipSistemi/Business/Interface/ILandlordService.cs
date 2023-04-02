using FaturaTakip.Data.Models;
using FaturaTakip.Utils.Results;

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
        Task<Result> AddLandlordAsync(Landlord landlord);
        Task<Result> RemoveLandlordAsync(int landlordId);
        Task<Result> UpdateLandlordAsync(Landlord landlord);
        Task<DataResult<IEnumerable<Landlord>>> GetAllLandlordsAsync();
        bool IsAnyLandlordExist();

    }
}
