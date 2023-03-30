using FaturaTakip.Data.Models;
using FaturaTakip.Utils.Results;

namespace FaturaTakip.Business.Interface
{
    public interface ILandlordService
    {
        Task<DataResult<Landlord>> GetLandlordByUserIdAsync(string userId);
        Task<bool> IsLandlordExistAsync(int landlordId);
        Task<bool> IsLandlordExistAsync(string userId);
        Task<bool> IsLandlordRegisteredInHouseAsync(int landlordId);
        Task<bool> IsLandlordRegisteredInHouseAsync(string userId);
        Task<Result> DeleteLandlordAsync(int landlordId);
    }
}
