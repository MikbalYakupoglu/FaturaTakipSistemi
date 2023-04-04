using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Core;
using FaturaTakip.Utils.Results;

namespace FaturaTakip.DataAccess.Abstract
{
    public interface ILandlordDal : IEntityRepository<Landlord>
    {
        Task<Landlord> GetLoginedLandlord(HttpContext httpContext);
    }
}
