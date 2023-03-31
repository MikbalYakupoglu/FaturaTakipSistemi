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

namespace FaturaTakip.Controllers
{
    public class TenantsController : Controller
    {
        private readonly InvoiceTrackContext _context;

        public TenantsController(InvoiceTrackContext context)
        {
            _context = context;
        }


        #region Tenants
        // GET: Tenants
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Index()
        {
              return View(await _context.Tenants.ToListAsync());
        }

        // GET: Tenants/Details/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tenants == null)
            {
                return NotFound();
            }

            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tenant == null)
            {
                return NotFound();
            }

            return View(tenant);
        }

        // GET: Tenants/Create
        [Authorize(Roles = "admin,moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tenants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Create([Bind($"{nameof(Tenant.Id)},{nameof(Tenant.Name)},{nameof(Tenant)},{nameof(Tenant.GovermentId)}," +
                                                      $"{nameof(Tenant.YearOfBirth)},{nameof(Tenant.Email)},{nameof(Tenant.Phone)},{nameof(Tenant.LisencePlate)}")] Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                if (!MernisUtils.VerifyGovermentId(tenant.GovermentId, tenant.Name, tenant.LastName, tenant.YearOfBirth).Result)
                {
                    ViewData["VerificationError"] = "Girdiğiniz Bilgiler Yanlış, Lütfen kontrol ediniz.";
                    return View(tenant);
                }

                //CreatePasswordHash(tenant, "123456");

                _context.Add(tenant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tenant);
        }

        // GET: Tenants/Edit/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tenants == null)
            {
                return NotFound();
            }

            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant == null)
            {
                return NotFound();
            }
            return View(tenant);
        }

        // POST: Tenants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(int id, [Bind($"{nameof(Tenant.Id)},{nameof(Tenant.Name)},{nameof(Tenant.LastName)},{nameof(Tenant.GovermentId)}," +
                                                            $"{nameof(Tenant.YearOfBirth)},{nameof(Tenant.Email)},{nameof(Tenant.Phone)},{nameof(Tenant.LisencePlate)}")] Tenant tenant)
        {
            if (id != tenant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tenant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IsTenantExists(tenant.Id))
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
            if (id == null || _context.Tenants == null)
            {
                return NotFound();
            }

            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tenant == null)
            {
                return NotFound();
            }

            return View(tenant);
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tenants == null)
            {
                return Problem("Entity set 'InvoiceTrackContext.Tenants'  is null.");
            }
            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant != null)
            {
                _context.Tenants.Remove(tenant);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion


        #region Manage


        #endregion



        #region Helpers
        private bool IsTenantExists(int id)
        {
            return _context.Tenants.Any(e => e.Id == id);
        }

        #endregion
    }
}
