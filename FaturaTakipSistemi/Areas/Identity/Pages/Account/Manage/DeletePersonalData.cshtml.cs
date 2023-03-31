// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FaturaTakip.Business.Interface;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.Data.Models.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FaturaTakip.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly ITenantService _tenantService;
        private readonly ILandlordService _landlordService;


        private readonly UserManager<InvoiceTrackUser> _userManager;
        private readonly SignInManager<InvoiceTrackUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;

        public DeletePersonalDataModel(
            UserManager<InvoiceTrackUser> userManager,
            SignInManager<InvoiceTrackUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,

            ITenantService tenantService,
            ILandlordService landlordService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;

            _tenantService = tenantService;
            _landlordService = landlordService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            var isTenant = await _tenantService.IsTenantExistAsync(user.Id);
            if (isTenant)
            {
                var isRegistered = await _tenantService.IsTenantRegisteredInHouseAsync(user.Id);
                if (isRegistered)
                {
                    ModelState.AddModelError(string.Empty, "Tenant is Registered with House.");
                    return Page();
                }
            }

            var isLandlord = await _landlordService.IsLandlordExistAsync(user.Id);
            if (isLandlord)
            {
                var isRegistered = await _landlordService.IsLandlordRegisteredInHouseAsync(user.Id);
                if(isRegistered)
                {
                    ModelState.AddModelError(string.Empty, "Landlord is Registered with House.");
                    return Page();
                }
            }

            if(user.NormalizedEmail == "ADMIN@ADMIN.COM")
            {
                ModelState.AddModelError(string.Empty, "Admin cannot be deleted.");
                return Page();
            }


            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}
