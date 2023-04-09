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
using FaturaTakip.Business.Concrete;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace FaturaTakip.Controllers
{
    public class MessagesController : Controller
    {
        private readonly UserManager<InvoiceTrackUser> _userManager;
        private readonly ITenantService _tenantService;
        private readonly ILandlordService _landlordService;
        private readonly IMessageService _messageService;
        private readonly IRentedApartmentService _rentedApartmentService;
        private readonly INotyfService _notyf;

        public MessagesController(UserManager<InvoiceTrackUser> userManager,
            ITenantService tenantService,
            ILandlordService landlordService,
            IMessageService messageService,
            IRentedApartmentService rentedApartmentService,
            INotyfService notyf)
        {
            _userManager = userManager;
            _tenantService = tenantService;
            _landlordService = landlordService;
            _messageService = messageService;
            _rentedApartmentService = rentedApartmentService;
            _notyf = notyf;
        }

        // GET: Messages
        [Authorize(Roles = "admin,moderator,landlord,tenant")]
        public async Task<IActionResult> Index()
        {
            var loginedUser = await _userManager.GetLoginedUserAsync(HttpContext);
            var loginedCustomUser = await GetLoginedUserWithType();

            if (loginedCustomUser != null) // Landlord veya Tenant mı
            {
                if (await _userManager.IsInRoleAsync(loginedUser, nameof(Landlord).ToLower()))
                {
                    var messages = await _messageService.GetMessagesByLandlordIdAsync(loginedCustomUser.Id);
                    return View(messages.Data);
                }
                if (await _userManager.IsInRoleAsync(loginedUser, nameof(Tenant).ToLower()))
                {
                    var messages = await _messageService.GetMessagesByTenantIdAsync(loginedCustomUser.Id);
                    return View(messages.Data);
                }
            }
            else if (await _userManager.IsInRoleAsync(loginedUser, "admin") || (await _userManager.IsInRoleAsync(loginedUser, "moderator")))
            {
                var messages = await _messageService.GetAllMessagesAsync();
                return View(messages.Data);
            }

            return Unauthorized();
        }

        [Authorize(Roles = "admin,moderator,landlord,tenant")]
        public async Task<IActionResult> View(int? id)
        {
            if (id == null || !_messageService.IsAnyMessageExist())
            {
                return NotFound();
            }
            var message = await _messageService.GetMessageByIdAsync(id);
            if (!message.Success)
            {
                return NotFound();
            }

            return View(message.Data);
        }


        // GET: Messages/Details/5
        [Authorize(Roles = "admin,moderator,landlord,tenant")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || !_messageService.IsAnyMessageExist())
            {
                return NotFound();
            }
            var message = await _messageService.GetMessageByIdAsync(id);
            if (!message.Success)
            {
                return NotFound();
            }

            return View(message.Data);
        }

        //GET: Messages/Create
        [Authorize(Roles = "admin,moderator,landlord,tenant")]
        public async Task<IActionResult> Create() // LoginedUserin Tenantları Listelenecek
        {
            var loginedUser = await _userManager.GetLoginedUserAsync(HttpContext);
            var loginedCustomUser = await GetLoginedUserWithType();

            if (loginedCustomUser != null) // Landlord veya Tenant mı
            {
                if (await _userManager.IsInRoleAsync(loginedUser, nameof(Landlord).ToLower()))
                {
                    var tenants = await _tenantService.GetTenantsByLandlordIdAsync(loginedCustomUser.Id);
                    var rentedApartments = await _rentedApartmentService.GetRentedApartmentsByLandlordIdAsync(loginedCustomUser.Id);
                    ViewData["FKRentedApartmentId"] = new SelectList(rentedApartments.Data, "Id", "Id");
                    //ViewData["FKTenantId"] = new SelectList(tenants.Data, "Id", "Id");
                }
                if (await _userManager.IsInRoleAsync(loginedUser, nameof(Tenant).ToLower()))
                {
                    var landlords = await _landlordService.GetLandlordByTenantIdAsync(loginedCustomUser.Id);
                    var rentedApartments = await _rentedApartmentService.GetTenantsRentedApartmentsByTenantIdAsync(loginedCustomUser.Id);
                    ViewData["FKRentedApartmentId"] = new SelectList(rentedApartments.Data, "Id", "Id");
                    //ViewData["FKLandlordId"] = new SelectList(landlords.Data, "Id", "Id");
                }
            }
            else if (await _userManager.IsInRoleAsync(loginedUser, "admin") || (await _userManager.IsInRoleAsync(loginedUser, "moderator")))
            {
                var users = await _userManager.GetAllUsersAsync();
                ViewData["Users"] = new SelectList(users, "Id", "Id");
            }
            return View();
        }

        //POST: Messages/Create
        //To protect from overposting attacks, enable the specific properties you want to bind to.
        //For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize(Roles = "admin,moderator,landlord,tenant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Body,FKTenantId,FKLandlordId,FKRentedApartmentId,FKUserId")] Message message)
        {
            var loginedUser = await _userManager.GetLoginedUserAsync(HttpContext);
            var loginedCustomUser = await GetLoginedUserWithType();

            message.IsVisible = true;

            if (loginedCustomUser != null) // Landlord veya Tenant mı
            {
                message.FKLandlordId = loginedCustomUser.Id;
                message.FkSenderId = loginedCustomUser.FK_UserId;
                var usersRentedApartment = await _rentedApartmentService.GetRentedApartmentByIdAsync(message.FKRentedApartmentId);
                message.FKRentedApartmentId = usersRentedApartment.Data.Id;

                if (await _userManager.IsInRoleAsync(loginedUser, nameof(Landlord).ToLower()))
                {
                    var tenants = await _tenantService.GetTenantsByLandlordIdAsync(loginedCustomUser.Id);
                    message.FKTenantId = usersRentedApartment.Data.FKTenantId;

                    var rentedApartments = await _rentedApartmentService.GetRentedApartmentsByLandlordIdAsync(loginedCustomUser.Id);
                    ViewData["FKRentedApartmentId"] = new SelectList(rentedApartments.Data, "Id", "Id");
                }
                if (await _userManager.IsInRoleAsync(loginedUser, nameof(Tenant).ToLower()))
                {
                    var landlords = await _landlordService.GetLandlordByTenantIdAsync(loginedCustomUser.Id);
                    message.FKLandlordId = usersRentedApartment.Data.Apartment.FKLandlordId;

                    var rentedApartments = await _rentedApartmentService.GetTenantsRentedApartmentsByTenantIdAsync(loginedCustomUser.Id);
                    ViewData["FKRentedApartmentId"] = new SelectList(rentedApartments.Data, "Id", "Id");
                }
            }
            else if (await _userManager.IsInRoleAsync(loginedUser, "admin") || (await _userManager.IsInRoleAsync(loginedUser, "moderator")))
            {
                var users = await _userManager.GetAllUsersAsync();
                ViewData["Users"] = new SelectList(users, "Id", "Id");

                var selectedUser = await GetCustomUserByUserId(message.FKUserId);

                if(selectedUser != null)
                {
                    message.FKLandlordId = selectedUser.GetType() == typeof(Landlord) ? selectedUser.Id : message.FKLandlordId;
                    message.FKTenantId = selectedUser.GetType() == typeof(Tenant) ? selectedUser.Id : message.FKTenantId;
                }

                message.FkSenderId = loginedUser.Id;
            }
            var result = await _messageService.AddAsync(message);
            if (result.Success)
            {
                _notyf.Success(result.Message);
                return RedirectToAction(nameof(Index));
            }
            else
                _notyf.Error(result.Message);



            return View(message);


            //if (ModelState.IsValid)
            //{
            //    _context.Add(message);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["FKLandlordId"] = new SelectList(_context.Landlords, "Id", "Id", message.FKLandlordId);
            //ViewData["FKTenantId"] = new SelectList(_context.Tenants, "Id", "Id", message.FKTenantId);
        }

        // GET: Messages/Edit/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || !_messageService.IsAnyMessageExist())
            {
                return NotFound();
            }

            //var message = await _context.Messages.FindAsync(id);
            var message = await _messageService.GetMessageByIdAsync(id);
            if (!message.Success)
            {
                return NotFound();
            }
            var landlords = await _landlordService.GetAllLandlordsAsync();
            var tenants = await _tenantService.GetAllTenantsAsync();
            ViewData["FKLandlordId"] = new SelectList(landlords.Data, "Id", "Id", message.Data.FKLandlordId);
            ViewData["FKTenantId"] = new SelectList(tenants.Data, "Id", "Id", message.Data.FKTenantId);
            return View(message.Data);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body")] Message message)
        {
            var messageToEdit = await _messageService.GetMessageByIdAsync(message.Id);

            if (!messageToEdit.Success)
            {
                return NotFound();
            }

            message.FKLandlordId = messageToEdit.Data.FKLandlordId;
            message.FKTenantId = messageToEdit.Data.FKTenantId;
            message.FKRentedApartmentId = messageToEdit.Data.FKRentedApartmentId;

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(message);
                    //await _context.SaveChangesAsync();
                    var result = await _messageService.UpdateAsync(message);
                    if (!result.Success)
                    {
                        _notyf.Error(result.Message);
                        return View(message);
                    }
                    else
                        _notyf.Success(result.Message);

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

            var landlords = await _landlordService.GetAllLandlordsAsync();
            var tenants = await _tenantService.GetAllTenantsAsync();
            ViewData["FKLandlordId"] = new SelectList(landlords.Data, "Id", "Id", messageToEdit.Data.FKLandlordId);
            ViewData["FKTenantId"] = new SelectList(tenants.Data, "Id", "Id", messageToEdit.Data.FKTenantId);
            return View(message);
        }

        // GET: Messages/Delete/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || !_messageService.IsAnyMessageExist())
            {
                return NotFound();
            }

            //var message = await _context.Messages
            //    .Include(m => m.Landlord)
            //    .Include(m => m.Tenant)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var message = await _messageService.GetMessageByIdAsync(id);
            if (!message.Success)
            {
                return NotFound();
            }

            return View(message.Data);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!_messageService.IsAnyMessageExist())
            {
                return Problem("Entity set 'InvoiceTrackContext.Messages'  is null.");
            }

            var result = await _messageService.DeleteAsync(id);

            if (result.Success)
                _notyf.Success(result.Message);
            else
                _notyf.Error(result.Message);

            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            var message = _messageService.GetMessageByIdAsync(id).Result;
            return message.Success;
        }

        #region Helpers
        private async Task<User> GetLoginedUserWithType()
        {
            var loginedUser = await _userManager.GetLoginedUserAsync(HttpContext);

            var landlord = await _landlordService.GetLandlordByIdAsync(loginedUser.Id);
            if (landlord.Success)
                return landlord.Data;

            var tenant = await _tenantService.GetTenantByIdAsync(loginedUser.Id);
            if (tenant.Success)
                return tenant.Data;

            return null;

        }

        private async Task<User> GetCustomUserByUserId(string userId)
        {
            var landlord = await _landlordService.GetLandlordByIdAsync(userId);
            if (landlord.Success)
                return landlord.Data;

            var tenant = await _tenantService.GetTenantByIdAsync(userId);
            if (tenant.Success)
                return tenant.Data;

            return null;

        }
        #endregion
    }
}
