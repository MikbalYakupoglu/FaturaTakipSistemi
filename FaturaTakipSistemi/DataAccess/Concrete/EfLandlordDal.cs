using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.DataAccess.Core;
using FaturaTakip.Utils.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.DataAccess.Concrete
{
    public class EfLandlordDal : EfEntityRepositoryBase<Landlord, InvoiceTrackContext>, ILandlordDal
    {
        private readonly UserManager<InvoiceTrackUser> _userManager;

        public EfLandlordDal(UserManager<InvoiceTrackUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<Landlord>> GetLandlordsByTenantIdAsync(int tenantId)
        {
            using (var context = new InvoiceTrackContext())
            {
                var landlords = await context.Messages
                    .Include(m => m.Landlord)
                    .Where(m => m.FKTenantId == tenantId)
                    .Select(m => m.Landlord)
                    .ToListAsync();

                return landlords;
            }
        }

        public async Task<Landlord> GetLoginedLandlordAsync(HttpContext httpContext)
        {
            using(var context = new InvoiceTrackContext())
            {
                var loginedUserId = _userManager.GetUserId(httpContext.User);
                var landlord = await context.Landlords.SingleOrDefaultAsync(l => l.FK_UserId == loginedUserId);
                return landlord;
            }
        }
    }
}
