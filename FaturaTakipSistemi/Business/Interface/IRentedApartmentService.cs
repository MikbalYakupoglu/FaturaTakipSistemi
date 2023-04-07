using FaturaTakip.Data.Models;
using FaturaTakip.Utils.Results;
using FaturaTakip.ViewModels;

namespace FaturaTakip.Business.Interface
{
    public interface IRentedApartmentService
    {
        //DataResult<IEnumerable<RentedApartment>> GetRentedApartmentsLandlords(int? id);
        Task<DataResult<IEnumerable<RentedApartment>>> GetTenantsRentedApartmentsByTenantIdAsync(int? tenantId);
        Task<DataResult<RentedApartment>> GetRentedApartmentByTenantIdAsync(int? tenantId);
        Task<DataResult<IEnumerable<RentedApartmentVM>>> GetAllRentedApartmentsAsync();
        Task<DataResult<RentedApartmentVM>> GetRentedApartmentVMByIdAsync(int? id);
        Task<DataResult<RentedApartment>> GetRentedApartmentByIdAsync(int? id);
        bool IsAnyRentedApartmentExist();
        Task<Result> AddRentedApartmentAsync(RentedApartment rentedApartmentToAdd);
        Task<Result> DeleteRentedApartmentAsync(int rentedApartmentId);
        Task<Result> UpdateRentedApartmentAsync(RentedApartment rentedApartment);
        Task<DataResult<IEnumerable<RentedApartment>>> GetRentedApartmentsByLandlordId(int? landlordId);
        Task<DataResult<RentedApartment>> GetRentedApartmentByApartmentIdAsync(int? apartmentId);

    }
}
