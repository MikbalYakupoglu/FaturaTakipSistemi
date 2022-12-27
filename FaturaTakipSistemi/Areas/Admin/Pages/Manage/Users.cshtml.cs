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

        public IActionResult OnGet()
        {     
            return Page();
        }
    }
}
