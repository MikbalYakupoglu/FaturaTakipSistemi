using FaturaTakip.Data.Models.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FaturaTakip.Areas.Admin.Pages.Manage
{
    public class UsersModel : PageModel
    {
        private readonly UserManager<InvoiceTrackUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UsersModel> _logger;

        public UsersModel(
            UserManager<InvoiceTrackUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<UsersModel> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }


        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "GovermentId")]
            [StringLength(11)]
            public string GovermentId { get; set; }

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
            public string Phone { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            public List<string> Roles { get; set; }

        }

        public async Task<IActionResult> OnGetAsync()
        {
     
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //var user = await _userManager.GetUserAsync(User);
            //if (user == null)
            //{
            //    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            //}

            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            //if (Input.Phone != phoneNumber)
            //{
            //    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.Phone);
            //    if (!setPhoneResult.Succeeded)
            //    {
            //        StatusMessage = "Unexpected error when trying to set phone number.";
            //        return RedirectToPage();
            //    }
            //}

            //if (Input.Name != user.Name)
            //{
            //    user.Name = Input.Name;
            //}

            //if (Input.LastName != user.LastName)
            //{
            //    user.LastName = Input.LastName;
            //}

            //if (Input.GovermentId != user.GovermentId)
            //{
            //    user.GovermentId = Input.GovermentId;
            //}

            //if (Input.Phone != user.Phone)
            //{
            //    user.Phone = Input.Phone;
            //}

            //if (Input.Email != user.Email)
            //{
            //    user.Email = Input.Email;
            //}
            
            //var userRole = await _userManager.GetRolesAsync(user);
            //if (Input.Roles != userRole)
            //{
            //    var itemsToAdd = Input.Roles.Except(userRole);
            //    userRole = userRole.Union(itemsToAdd).ToList();
            //}

            //await _userManager.UpdateAsync(user);
            //await _userManager.AddToRolesAsync(user, userRole);
            //StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
