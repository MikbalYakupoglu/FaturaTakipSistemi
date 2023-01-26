using FaturaTakip.Core.DataAccess;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;

namespace FaturaTakip.DataAccess.Concrete.EntityFramework;

public class EfLandlordDal : EfEntityRepositoryBase<Landlord>, ILandlordDal
{
    public EfLandlordDal(InvoiceTrackContext context) : base(context)
    {
    }
}