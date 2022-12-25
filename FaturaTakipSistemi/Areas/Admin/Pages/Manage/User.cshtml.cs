using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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

        [TempData]
        public string StatusMessage { get; set; }
        public List<IdentityRole> AllRoles { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

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
            public List<string>? Roles { get; set; }
        }

        private async Task LoadAsync(InvoiceTrackUser user)
        {
            Input = new InputModel
            {
                Name = user.Name,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Roles = (List<string>)await _userManager.GetRolesAsync(user)
            };
            AllRoles = await _roleManager.Roles.ToListAsync();
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
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
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

            //var userRole = await _userManager.GetRolesAsync(user);
            //if (Input.Roles != userRole)
            //{
            //    var itemsToAdd = Input.Roles.Except(userRole);
            //    userRole = userRole.Union(itemsToAdd).ToList();
            //}

            await _userManager.UpdateAsync(user);
            //await _userManager.AddToRolesAsync(user, userRole);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
