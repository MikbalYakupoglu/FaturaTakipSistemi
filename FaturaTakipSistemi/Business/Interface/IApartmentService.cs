using FaturaTakip.Data.Models;
using FaturaTakip.Utils.Results;
using System.Collections;

namespace FaturaTakip.Business.Interface
{
    public interface IApartmentService
    {
        Task<DataResult<Apartment>> GetApartmentByLandlordIdAsync(int? landlordId);
        Task<DataResult<IEnumerable<Apartment>>> GetAllApartmentsWithLandlordsAsync();
    }
}
