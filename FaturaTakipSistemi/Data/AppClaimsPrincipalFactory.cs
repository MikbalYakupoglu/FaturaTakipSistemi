using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace FaturaTakip.Data
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<InvoiceTrackUser, IdentityRole>
    {
        public AppClaimsPrincipalFactory
            (UserManager<InvoiceTrackUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> options) :
            base(userManager, roleManager, options)
        {
        }


        public async override Task<ClaimsPrincipal> CreateAsync(InvoiceTrackUser user)
        {
            var principal = await base.CreateAsync(user);

            if (!string.IsNullOrWhiteSpace(user.Name))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
        new Claim(ClaimTypes.GivenName, user.Name)
    });
            }

            if (!string.IsNullOrWhiteSpace(user.LastName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
         new Claim(ClaimTypes.Surname, user.LastName),
    });
            }

            return principal;
        }
    }
}
