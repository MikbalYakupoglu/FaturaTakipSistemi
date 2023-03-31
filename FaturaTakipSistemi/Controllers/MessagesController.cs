using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.Models;
using Microsoft.AspNetCore.Identity;
using FaturaTakip.Data.Models.Abstract;
using Microsoft.AspNetCore.Authorization;
using FaturaTakip.Business.Interface;

namespace FaturaTakip.Controllers
{
    public class MessagesController : Controller
    {
        private readonly InvoiceTrackContext _context;
        private readonly UserManager<InvoiceTrackUser> _userManager;
        private readonly ITenantService _tenantService;
        private readonly ILandlordService _landlordService;

        public MessagesController(InvoiceTrackContext context,
            UserManager<InvoiceTrackUser> userManager,
            ITenantService tenantService,
            ILandlordService landlordService)
        {
            _context = context;
            _userManager = userManager;
            _tenantService = tenantService;
            _landlordService = landlordService;
        }

        // GET: Messages
        [Authorize(Roles = "admin,moderator,landlord,tenant")]
        public async Task<IActionResult> Index()
        {
            var loginedUser = GetLoginedUserWithType().Result;
            var landlord = new Landlord();
            var tenant = new Tenant();
            if (loginedUser != null)
            {
                if (loginedUser.GetType() == landlord.GetType())
                {
                    var messages = _context.Messages
                        .Include(m => m.Tenant)
                        .Where(m => m.FKLandlordId == loginedUser.Id);
                    return View(messages);
                }


                if (loginedUser.GetType() == tenant.GetType())
                {
                    var messages = _context.Messages
                        .Include(m => m.Landlord)
                        .Where(m => m.FKTenantId == loginedUser.Id);
                    return View(messages);
                }
            }
            else if (await _userManager.IsInRoleAsync(GetLoginedUser(), "admin") || (await _userManager.IsInRoleAsync(GetLoginedUser(), "moderator")))
            {
                var messages = _context.Messages
                    .Include(m => m.Landlord)
                    .Include(m => m.Tenant);

                return View(await messages.ToListAsync());
            }

            return Unauthorized();
        }

        [Authorize(Roles = "admin,moderator,landlord,tenant")]
        public async Task<IActionResult> View(int? id)
        {
            if (id == null || _context.Messages == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.Apartment)
                .Include(m => m.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }


        // GET: Messages/Details/5
        [Authorize(Roles = "admin,moderator,landlord,tenant")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Messages == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.Landlord)
                .Include(m => m.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
        [Authorize(Roles = "admin,moderator,landlord,tenant")]
        public IActionResult Create()
        {
            ViewData["FKLandlordId"] = new SelectList(_context.Landlords, "Id", "Id");
            ViewData["FKTenantId"] = new SelectList(_context.Tenants, "Id", "Id");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Body,FKTenantId,FKLandlordId,FkApartmentId")] Message message)
        {
            message.IsVisible = true;

            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FKLandlordId"] = new SelectList(_context.Landlords, "Id", "Id", message.FKLandlordId);
            ViewData["FKTenantId"] = new SelectList(_context.Tenants, "Id", "Id", message.FKTenantId);
            return View(message);
        }

        // GET: Messages/Edit/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Messages == null)
            {
                return NotFound();
            }

            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            ViewData["FKLandlordId"] = new SelectList(_context.Landlords, "Id", "Id", message.FKLandlordId);
            ViewData["FKTenantId"] = new SelectList(_context.Tenants, "Id", "Id", message.FKTenantId);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body,FKTenantId,FKLandlordId,FkApartmentId")] Message message)
        {
            if (id != message.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.Id))
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
            ViewData["FKLandlordId"] = new SelectList(_context.Landlords, "Id", "Id", message.FKLandlordId);
            ViewData["FKTenantId"] = new SelectList(_context.Tenants, "Id", "Id", message.FKTenantId);
            return View(message);
        }

        // GET: Messages/Delete/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Messages == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.Landlord)
                .Include(m => m.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Messages == null)
            {
                return Problem("Entity set 'InvoiceTrackContext.Messages'  is null.");
            }
            var message = await _context.Messages.FindAsync(id);
            if (message != null)
            {
                _context.Messages.Remove(message);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }

        #region Helpers
        private InvoiceTrackUser GetLoginedUser()
        {
            var loginedUserId = _userManager.GetUserId(HttpContext.User);
            if (loginedUserId == null)
                return null;

            var loginedUser = _context.Users.First(u => u.Id == loginedUserId);

            return loginedUser;
        }
        private async Task<User> GetLoginedUserWithType()
        {
            var loginedUser = GetLoginedUser();

            var landlord = await _landlordService.GetLandlordByUserIdAsync(loginedUser.Id);
            if (landlord.Success)
                return landlord.Data;

            var tenant = await _tenantService.GetTenantByIdAsync(loginedUser.Id);
            if (tenant.Success)
                return tenant.Data;

            return null;

        }
        #endregion
    }
}
