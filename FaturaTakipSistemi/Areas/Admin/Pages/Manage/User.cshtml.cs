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

namespace FaturaTakip.Areas.Admin.Pages.Manage
{
    public class UserModel : PageModel
    {
        private readonly UserManager<InvoiceTrackUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserModel> _logger;
        public UserModel(
            UserManager<InvoiceTrackUser> userManager,
            ILogger<UserModel> logger,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
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


            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }

            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
            }

            var updateResult = await UpdateUserRoles(user);
            if (!updateResult)
            {
                return RedirectToPage();
            }

            await _userManager.UpdateAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
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
            }


            return true;
        }
    }
}
