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

        public async Task<Landlord> GetLoginedLandlord(HttpContext httpContext)
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
