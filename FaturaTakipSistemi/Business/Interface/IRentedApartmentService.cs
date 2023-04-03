using FaturaTakip.Data.Models;
using FaturaTakip.Utils.Results;
using FaturaTakip.ViewModels;

namespace FaturaTakip.Business.Interface
{
    public interface IRentedApartmentService
    {
        //DataResult<IEnumerable<RentedApartment>> GetRentedApartmentsLandlords(int? id);
        Task<DataResult<RentedApartment>> GetRentedApartmentByTenantIdAsync(int? tenantId);
        Task<DataResult<IEnumerable<RentedApartmentVM>>> GetAllRentedApartmentsWithApartmentsAndTenantsAsync();
        Task<DataResult<RentedApartmentVM>> GetRentedApartmentByIdWithApartmentAndTenantAsync(int? id);
        bool IsAnyRentedApartmentExist();
        Task<Result> AddRentedApartmentAsync(RentedApartment rentedApartmentToAdd);
        Task<Result> DeleteRentedApartmentAsync(int rentedApartmentId);
        Task<Result> UpdateRentedApartmentAsync(RentedApartment rentedApartment);


    }
}
