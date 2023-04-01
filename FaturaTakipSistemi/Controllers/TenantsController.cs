using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.Data.Models.Abstract;
using FaturaTakip.Models;
using FaturaTakip.Utils;
using GovermentIdVerification;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using FaturaTakip.Business.Interface;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace FaturaTakip.Controllers
{
    public class TenantsController : Controller
    {
        private readonly ITenantService _tenantService;
        private readonly INotyfService _notyf;

        private readonly UserManager<InvoiceTrackUser> _userManager;

        public TenantsController(ITenantService tenantService,
            INotyfService notyf,
            UserManager<InvoiceTrackUser> userManager)
        {
            _tenantService = tenantService;
            _notyf = notyf;
            _userManager = userManager;
        }


        #region Tenants
        // GET: Tenants
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Index()
        {
            var tenants = await _tenantService.GetAllTenantsAsync();
            return View(tenants.Data);
        }

        // GET: Tenants/Details/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || !_tenantService.IsAnyTenantExist())
            {
                return NotFound();
            }

            var tenant = await _tenantService.GetTenantByIdAsync(id);

            if (!tenant.Success)
            {
                return NotFound();
            }

            return View(tenant.Data);
        }

        // GET: Tenants/Create
        //[Authorize(Roles = "admin,moderator")]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Tenants/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "admin,moderator")]
        //public async Task<IActionResult> Create([Bind($"{nameof(Tenant.Id)},{nameof(Tenant.Name)},{nameof(Tenant)},{nameof(Tenant.GovermentId)}," +
        //                                              $"{nameof(Tenant.YearOfBirth)},{nameof(Tenant.Email)},{nameof(Tenant.Phone)},{nameof(Tenant.LisencePlate)}")] Tenant tenant)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (!MernisUtils.VerifyGovermentId(tenant.GovermentId, tenant.Name, tenant.LastName, tenant.YearOfBirth).Result)
        //        {
        //            ViewData["VerificationError"] = "Girdiğiniz Bilgiler Yanlış, Lütfen kontrol ediniz.";
        //            return View(tenant);
        //        }


        //        _context.Add(tenant);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(tenant);
        //}

        // GET: Tenants/Edit/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || !_tenantService.IsAnyTenantExist())
            {
                return NotFound();
            }

            var tenant = await _tenantService.GetTenantByIdAsync(id);
            if (!tenant.Success)
            {
                return NotFound();
            }
            return View(tenant.Data);
        }

        // POST: Tenants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(int id, [Bind($"{nameof(Tenant.Name)},{nameof(Tenant.LastName)},{nameof(Tenant.GovermentId)}," +
                                                            $"{nameof(Tenant.YearOfBirth)},{nameof(Tenant.Email)},{nameof(Tenant.Phone)},{nameof(Tenant.LisencePlate)}")] Tenant tenant)
        {
            var tenantToEdit = await _tenantService.GetTenantByIdAsync(id);

            if (!tenantToEdit.Success)
            {
                return NotFound();
            }

            tenant.Id = tenantToEdit.Data.Id;
            tenant.FK_UserId = tenantToEdit.Data.FK_UserId;


            if (ModelState.IsValid)
            {
                try
                {
                    await _tenantService.UpdateTenantAsync(tenant);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _tenantService.IsTenantExistAsync(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tenant);
        }

        // GET: Tenants/Delete/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || ! _tenantService.IsAnyTenantExist())
            {
                return NotFound();
            }

            var tenant = await _tenantService.GetTenantByIdAsync(id);
            if (!tenant.Success)
            {
                return NotFound();
            }

            return View(tenant.Data);
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!_tenantService.IsAnyTenantExist())
            {
                return Problem("Entity set 'InvoiceTrackContext.Tenants'  is null.");
            }

            var tenant = await _tenantService.GetTenantByIdAsync(id);
            if (tenant.Success)
            {
                var result = await _tenantService.DeleteTenantAsync(tenant.Data);
                if(!result.Success)
                {
                    _notyf.Error(result.Message);
                }
                else
                {
                    _notyf.Success("Başarıyla Silindi.");
                    await _userManager.DeleteAsync(await _userManager.FindByIdAsync(tenant.Data.FK_UserId));
                }
            }
            
            return RedirectToAction(nameof(Index));
        }

        #endregion


        #region Manage


        #endregion



        #region Helpers


        #endregion
    }
}
