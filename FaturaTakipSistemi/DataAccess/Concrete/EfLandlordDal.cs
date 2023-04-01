using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.DataAccess.Core;

namespace FaturaTakip.DataAccess.Concrete
{
    public class EfLandlordDal : EfEntityRepositoryBase<Landlord, InvoiceTrackContext>, ILandlordDal
    {

    }
}
