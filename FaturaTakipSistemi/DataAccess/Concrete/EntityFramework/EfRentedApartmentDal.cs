using FaturaTakip.Core.DataAccess;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;

namespace FaturaTakip.DataAccess.Concrete.EntityFramework;

public class EfRentedApartmentDal : EfEntityRepositoryBase<RentedApartment>, IRentedApartmentDal
{
    public EfRentedApartmentDal(InvoiceTrackContext context) : base(context)
    {
    }
}