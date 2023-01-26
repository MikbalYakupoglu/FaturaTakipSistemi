using FaturaTakip.Core.DataAccess;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;

namespace FaturaTakip.DataAccess.Concrete.EntityFramework;

public class EfPaymentDal : EfEntityRepositoryBase<Payment>, IPaymentDal
{
    public EfPaymentDal(InvoiceTrackContext context) : base(context)
    {
    }
}