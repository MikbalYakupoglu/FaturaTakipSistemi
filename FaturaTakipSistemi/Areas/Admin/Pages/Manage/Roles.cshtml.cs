using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FaturaTakip.Areas.Admin.Pages.Manage;

[Authorize(Roles = "admin,moderator")]
public class RoleModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<RoleModel> _logger;

    public RoleModel(RoleManager<IdentityRole> roleManager, ILogger<RoleModel> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }


    [BindProperty]
    public InputModel Input { get; set; }
    

    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        public string RoleName { get; set; }

    }

    public  void OnGetAsync(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ModelState.Remove("returnUrl"); // (????)
        if (ModelState.IsValid)
        {
            IdentityRole role = new() { Name = Input.RoleName };
            IdentityResult result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                Input.RoleName = "";
                _logger.LogInformation("Role Successfully Added.");
                return Page();
            }
            else
            {
                if (result.Errors.First().Code == "DuplicateRoleName")
                {
                    ModelState.AddModelError(string.Empty, "Role Already Exist.");
                    return Page();
                }


                ModelState.AddModelError(string.Empty, "Invalid Role Adding Attemp.");
                return Page();
            }
        }

        return Page();
    }
}
