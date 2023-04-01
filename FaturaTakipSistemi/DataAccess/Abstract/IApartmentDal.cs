using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Core;
using FaturaTakip.Utils.Results;
using System.Collections;

namespace FaturaTakip.DataAccess.Abstract
{
    public interface IApartmentDal : IEntityRepository<Apartment>
    {
        Task<DataResult<IEnumerable<Apartment>>> GetAllApartmentsWithLandlordsAsync();
    }
}
