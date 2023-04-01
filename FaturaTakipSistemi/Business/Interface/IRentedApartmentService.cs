using FaturaTakip.Data.Models;
using FaturaTakip.Utils.Results;

namespace FaturaTakip.Business.Interface
{
    public interface IRentedApartmentService
    {
        //DataResult<IEnumerable<RentedApartment>> GetRentedApartmentsLandlords(int? id);
        Task<DataResult<RentedApartment>> GetRentedApartmentByTenantIdAsync(int? tenantId);

    }
}
