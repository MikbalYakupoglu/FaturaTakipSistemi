using FaturaTakip.Controllers;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.DataAccess.Core;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.DataAccess.Concrete
{
    public class EfTenantDal : EfEntityRepositoryBase<Tenant, InvoiceTrackContext>, ITenantDal
    {

    }
}
