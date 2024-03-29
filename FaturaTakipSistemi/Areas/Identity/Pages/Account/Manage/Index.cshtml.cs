﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FaturaTakip.Business.Concrete;
using FaturaTakip.Business.Interface;
using FaturaTakip.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FaturaTakip.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<InvoiceTrackUser> _userManager;
        private readonly SignInManager<InvoiceTrackUser> _signInManager;
        private readonly ITenantService _tenantService;
        private readonly ILandlordService _landlordService;

        public IndexModel(
            UserManager<InvoiceTrackUser> userManager,
            SignInManager<InvoiceTrackUser> signInManager,
            ITenantService tenantService,
            ILandlordService landlordService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tenantService = tenantService;
            _landlordService = landlordService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

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
            [Required]
            [Display(Name = "Name")]
            [StringLength(50)]
            public string Name { get; set; }
            
            [Required]
            [Display(Name = "LastName")]
            [StringLength(50)]
            public string LastName { get; set; }


            [Required]
            [Display(Name = "GovermentId")]
            [StringLength(11)]
            public string GovermentId { get; set; }

            [Required]
            [Display(Name = "YearOfBirth")]
            [Range(1900, 2022)]
            public int YearOfBirth { get; set; }

            [Required]
            [Display(Name = "Phone")]
            [StringLength(10)]
            [Phone]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(InvoiceTrackUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Name = user.Name,
                LastName = user.LastName,
                GovermentId = user.GovermentId,
                PhoneNumber = user.PhoneNumber,
                YearOfBirth = user.YearOfBirth
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
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

            if(Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }

            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
            }

            if (Input.GovermentId != user.GovermentId)
            {
                user.GovermentId= Input.GovermentId;
            }

            if (Input.YearOfBirth != user.YearOfBirth)
            {
                user.YearOfBirth= Input.YearOfBirth;
            }

            var customUser = await _userManager.GetCustomUserWithUserIdAsync(user.Id);
            if(customUser.GetType() == typeof(Landlord))
            {
                var newLandlord = new Landlord()
                {
                    Id = customUser.Id,
                    Name = Input.Name,
                    LastName = Input.LastName,
                    GovermentId = Input.GovermentId,
                    YearOfBirth = Input.YearOfBirth,
                    Phone = Input.PhoneNumber
                };
                await _landlordService.UpdateLandlordAsync(newLandlord);
            }
            else if(customUser.GetType() == typeof(Tenant))
            {
                var newTenant = new Tenant()
                {
                    Id = customUser.Id,
                    Name = Input.Name,
                    LastName = Input.LastName,
                    GovermentId = Input.GovermentId,
                    YearOfBirth = Input.YearOfBirth,
                    Phone = Input.PhoneNumber
                };
                await _tenantService.UpdateTenantAsync(newTenant);
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
