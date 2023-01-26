using FaturaTakip.Core.DataAccess;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;

namespace FaturaTakip.DataAccess.Concrete.EntityFramework;

public class EfDebtDal : EfEntityRepositoryBase<Debt>, IDebtDal
{
    public EfDebtDal(InvoiceTrackContext context) : base(context)
    {
    }
}