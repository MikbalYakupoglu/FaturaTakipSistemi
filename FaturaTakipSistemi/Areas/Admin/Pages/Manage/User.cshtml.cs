using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.Data.Models.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NuGet.Packaging;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace FaturaTakip.Areas.Admin.Pages.Manage
{
[Authorize(Roles = "admin,moderator")]
    public class UserModel : PageModel
    {
        private readonly UserManager<InvoiceTrackUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserModel> _logger;
        private readonly InvoiceTrackContext _context;
        public UserModel(
            UserManager<InvoiceTrackUser> userManager,
            ILogger<UserModel> logger,
            RoleManager<IdentityRole> roleManager,
            InvoiceTrackContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }

        [BindProperty]
        public Guid Id { get; set; }
        [BindProperty]
        public List<string> AllRoleNames { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            [StringLength(50)]
            public string Name { get; set; }

            [Required]
            [Display(Name = "LastName")]
            [StringLength(50)]
            public string LastName { get; set; }


            [Required]
            [Display(Name = "Phone")]
            [StringLength(10)]
            [Phone]
            public string PhoneNumber { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            //[Required]
            //public Dictionary<string,bool>? Roles { get; set; }
            public List<string>? Roles { get; set; }

        }

        private async Task LoadAsync(InvoiceTrackUser user)
        {
            AllRoleNames = _roleManager.Roles.ToList().Select(x => x.Name).ToList();

            //AllRoleNames = new List<string>();
            //var allRoles = await _roleManager.Roles.ToListAsync();
            //List<string> roleNames = allRoles.Select(r => r.Name).ToList();
            //AllRoleNames.AddRange(roleNames);

            Input = new InputModel
            {
                Name = user.Name,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                //Roles = AllRoleNames.ToDictionary(r => r, b => false)
                Roles = new List<string>()
            };

            await GetSelectedRoles(user);
        }

        private async Task GetSelectedRoles(InvoiceTrackUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var allRole in AllRoleNames)
            {
                foreach (var userRole in userRoles)
                {
                    if (allRole == userRole)
                        Input.Roles.Add(allRole);
                }
                //Input.Roles[item] = await _userManager.IsInRoleAsync(user, item) ? true : false;
            }
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Id = id;
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            AllRoleNames = _roleManager.Roles.ToList().Select(x => x.Name).ToList();

            foreach (string roleName in AllRoleNames)
            {
                _logger.LogCritical(roleName);
            }

            var user = await _userManager.FindByIdAsync(Id.ToString());

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                StatusMessage = "Error : Model Error.";
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set email.";
                    return RedirectToPage();
                }
            }

            var userToUpdate = GetSelectedUserFromTable(user.GovermentId);

            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
                userToUpdate.Name = Input.Name;
            }

            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
                userToUpdate.LastName = Input.LastName;
            }

            var updateResult = await UpdateUserRoles(user);
            if (!updateResult)
            {
                return RedirectToPage();
            }

            await _userManager.UpdateAsync(user);

            if(!string.IsNullOrEmpty(StatusMessage))
            {
                if (!StatusMessage.StartsWith("Warning"))
                    StatusMessage = "Your profile has been updated";
            }
            else
                StatusMessage = "Your profile has been updated";



            return RedirectToPage();
        }

        private User GetSelectedUserFromTable(string govermentId)
        {
            var landlord = _context.Landlords.FirstOrDefault(l => l.GovermentId == govermentId);
            if (landlord != null)
                return landlord;

            var tenant = _context.Tenants.FirstOrDefault(t => t.GovermentId == govermentId);
            if (tenant != null)
                return tenant;

            return null;
        }

        private async Task<bool> UpdateUserRoles(InvoiceTrackUser user)
        {
            List<string> rolesToRemove = new List<string>();

            var userRoles = await _userManager.GetRolesAsync(user);


            if (Input.Roles?.Count > 0)
            {
                rolesToRemove = userRoles.Except(Input.Roles, StringComparer.Ordinal).ToList();
            }
            else
            {
                rolesToRemove = userRoles.ToList();
            }


            if (rolesToRemove.Count > 0)
            {
                if (rolesToRemove.Contains("tenant"))
                {
                    var tenantToDelete = await _context.Tenants.FirstAsync(t => t.GovermentId == user.GovermentId);
                    if (_context.RentedApartments.Any(ra => ra.Tenant.Id == tenantToDelete.Id))
                    {
                        StatusMessage = "Warning : Cannot be deleted while the tenant is actively in the apartment.";
                        rolesToRemove.Remove("tenant");
                    }
                    else
                        _context.Remove(tenantToDelete);
                }

                if (rolesToRemove.Contains("landlord"))
                {
                    var landLordToDelete = await _context.Landlords.FirstAsync(l => l.GovermentId == user.GovermentId);
                    if(_context.Apartments.Any(a=> a.Landlord.Id == landLordToDelete.Id))
                    {
                        StatusMessage = "Warning : You can't delete a host while the owner's home is found.";
                        rolesToRemove.Remove("landlord");
                    }
                    else
                        _context.Remove(landLordToDelete);
                }

                var deleteResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!deleteResult.Succeeded)
                {
                    StatusMessage = "Error : Role Delete Error.";
                    return false;

                }
            }

            if (Input.Roles?.Count > 0)
            {
                var rolesToAdd = Input.Roles.Except(userRoles, StringComparer.Ordinal).ToList();
                var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);

                if (!addResult.Succeeded)
                {
                    StatusMessage = "Error : Role Add Error.";
                    return false;
                }

                if (rolesToAdd.Contains("tenant"))
                {
                    var tenant = SetUserData<Tenant>(user);
                    await _context.AddAsync(tenant);
                }
                if (rolesToAdd.Contains("landlord"))
                {
                    var landLord = SetUserData<Landlord>(user);
                    await _context.AddAsync(landLord);
                }
            }


            userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Count == 0)
            {
                var addDefaultUnknownRoleResult = await _userManager.AddToRoleAsync(user, "unknown");

                if (!addDefaultUnknownRoleResult.Succeeded)
                {
                    StatusMessage = "Error : Add Default Unknown Role Error.";
                    return false;
                }

                user.Status = false;
                await _context.SaveChangesAsync();
            }
            else
            {
                if(userRoles.Contains("unknown") && userRoles.Count != 1)
                {
                    var removeUnknownResult = await _userManager.RemoveFromRoleAsync(user, "unknown");
                    if (!removeUnknownResult.Succeeded)
                    {
                        StatusMessage = "Error : Remove Unknown Role Error.";
                        return false;
                    }
                }

                user.Status = true;
                await _context.SaveChangesAsync();
            }

            return true;
        }

        private T SetUserData<T>(InvoiceTrackUser user) where T : User
        {
            T userToAdd = Activator.CreateInstance(typeof(T)) as T;

            userToAdd.Name = user.Name;
            userToAdd.LastName = user.LastName;
            userToAdd.GovermentId = user.GovermentId;
            userToAdd.Email = user.Email;
            userToAdd.Phone = user.PhoneNumber;
            userToAdd.YearOfBirth = user.YearOfBirth;
            userToAdd.FK_UserId = user.Id;

            return userToAdd;
        }
    }


}
