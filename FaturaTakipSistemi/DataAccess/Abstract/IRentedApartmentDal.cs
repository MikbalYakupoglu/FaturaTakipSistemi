using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Core;
using FaturaTakip.Utils.Results;
using FaturaTakip.ViewModels;

namespace FaturaTakip.DataAccess.Abstract
{
    public interface IRentedApartmentDal : IEntityRepository<RentedApartment>
    {
        Task<DataResult<IEnumerable<RentedApartment>>> GetAllRentedApartmentsWithApartmentsAndTenantsAsync();
        Task<DataResult<RentedApartment>> GetRentedApartmentByIdWithApartmentAndTenantAsync(int? rentedApartmentId);
        Task<IEnumerable<RentedApartment>> GetRentedApartmentsByLandlordId(int? landlordId);
        Task<IEnumerable<RentedApartment>> GetTenantsRentedApartmentsByTenantId(int? tenantId);
        Task<RentedApartment> GetRentedApartmentByApartmentId(int? apartmentId);
    }
}
