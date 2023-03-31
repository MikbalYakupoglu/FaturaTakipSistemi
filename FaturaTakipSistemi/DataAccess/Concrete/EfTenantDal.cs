using FaturaTakip.Controllers;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.DataAccess.Core;

namespace FaturaTakip.DataAccess.Concrete
{
    public class EfTenantDal : EfEntityRepositoryBase<Tenant, InvoiceTrackContext>, ITenantDal
    {
        public async Task<bool> IsAnyTenantExistAsync()
        {
            var tenants = await GetAllAsync();
            return tenants.Any();
        }
    }
}
