using AspNetCoreHero.ToastNotification.Abstractions;
using FaturaTakip.Business.Interface;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.Utils;
using FaturaTakip.Utils.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Data;

namespace FaturaTakip.Controllers
{
    public class LandlordsController : Controller
    {
        private readonly UserManager<InvoiceTrackUser> _userManager;
        private readonly ILandlordService _landlordService;
        private readonly ITenantService _tenantService;
        private readonly IApartmentService _apartmentService;
        private readonly IRentedApartmentService _rentedApartmentService;
        private readonly INotyfService _notyf;


        public LandlordsController(UserManager<InvoiceTrackUser> userManager,
            ILandlordService landlordService,
            ITenantService tenantService,
            IApartmentService apartmentService,
            IRentedApartmentService rentedApartmentService,
            INotyfService notyf)
        {
            _userManager = userManager;
            _landlordService = landlordService;
            _tenantService = tenantService;
            _apartmentService = apartmentService;
            _rentedApartmentService = rentedApartmentService;
            _notyf = notyf;
        }

        #region Landlord
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Index()
        {
            var landlords = await _landlordService.GetAllLandlordsAsync();
            return View(landlords.Data);
        }

        // GET: Landlords/Details/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || !_landlordService.IsAnyLandlordExist())
            {
                return NotFound();
            }

            //var landlord = await _context.Landlords
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var landlord = await _landlordService.GetLandlordByIdAsync(id);
            if (!landlord.Success)
            {
                return NotFound();
            }

            return View(landlord.Data);
        }

        // GET: Landlords/Create

        //[Authorize(Roles = "admin,moderator")]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: Landlords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "admin,moderator")]
        //public async Task<IActionResult> Create([Bind($"{nameof(Landlord.Id)},{nameof(Landlord.Name)},{nameof(Landlord.LastName)},{nameof(Landlord.GovermentId)}," +
        //                                              $"{nameof(Landlord.YearOfBirth)},{nameof(Landlord.Email)},{nameof(Landlord.Phone)}")] Landlord landlord)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (!MernisUtils.VerifyGovermentId(landlord.GovermentId, landlord.Name, landlord.LastName, landlord.YearOfBirth).Result)
        //        {
        //            ViewData["VerificationError"] = "Girdiğiniz Bilgiler Yanlış, Lütfen kontrol ediniz.";
        //            return View(landlord);
        //        }
        //        _context.Add(landlord);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(landlord);
        //}

        // GET: Landlords/Edit/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || !_landlordService.IsAnyLandlordExist())
            {
                return NotFound();
            }

            //var landlord = await _context.Landlords.FindAsync(id);
            var landlord = await _landlordService.GetLandlordByIdAsync(id);
            if (!landlord.Success)
            {
                return NotFound();
            }
            return View(landlord.Data);
        }

        // POST: Landlords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(int id, [Bind(include: $"{nameof(Landlord.Name)},{nameof(Landlord.LastName)},{nameof(Landlord.GovermentId)}," +
                                                            $"{nameof(Landlord.YearOfBirth)},{nameof(Landlord.Email)},{nameof(Landlord.Phone)}")] Landlord landlord)
        {
            var landlordToEdit = await _landlordService.GetLandlordByIdAsync(id);
            if (!landlordToEdit.Success)
            {
                return NotFound();
            }

            landlord.Id = landlordToEdit.Data.Id;
            landlord.FK_UserId = landlordToEdit.Data.FK_UserId;

            if (ModelState.IsValid)
            {
                //if (!MernisUtils.VerifyGovermentId(landlord.GovermentId, landlord.Name, landlord.LastName, landlord.YearOfBirth).Result)
                //{
                //    ViewData["VerificationError"] = "Girdiğiniz Bilgiler Yanlış, Lütfen kontrol ediniz.";
                //    return View(landlord);
                //}
                try
                {
                    //_context.Update(landlord);
                    //await _context.SaveChangesAsync();
                    await _landlordService.UpdateLandlordAsync(landlord);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LandlordExists(landlord.Id))
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
            return View(landlord);
        }

        // GET: Landlords/Delete/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || !_landlordService.IsAnyLandlordExist())
            {
                return NotFound();
            }

            //var landlord = await _context.Landlords
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var landlord = await _landlordService.GetLandlordByIdAsync(id);
            if (!landlord.Success)
            {
                return NotFound();
            }

            return View(landlord.Data);
        }

        // POST: Landlords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!_landlordService.IsAnyLandlordExist())
            {
                return Problem("Entity set 'InvoiceTrackContext.Landlords'  is null.");
            }
            //var landlord = await _context.Landlords.FindAsync(id);
            var landlord = await _landlordService.GetLandlordByIdAsync(id);
            if (landlord.Success)
            {
                var result = await _landlordService.DeleteLandlordAsync(id);
                if(!result.Success)
                {
                    _notyf.Error(result.Message);
                }
                else
                {
                    _notyf.Success("Başarıyla Silindi.");
                    await _userManager.DeleteAsync(await _userManager.FindByIdAsync(landlord.Data.FK_UserId));
                }
            }

            return RedirectToAction(nameof(Index));
        }


        #endregion


        #region Manage

        #region Apartment

        [Authorize(Roles = "landlord,admin,moderator")]
        [Route("landlords/manage/apartments")]
        public async Task<IActionResult> ListApartments()
        {
            var landlordId = await GetLoginedLandlordId();
            var landlordsApartments = await _apartmentService.GetApartmentsByLandlordIdAsync(landlordId);

            ViewData["Apartments"] = landlordsApartments.Data;            
            return View(landlordsApartments.Data);
        }

        #endregion

        #region Message

        //[Authorize(Roles = "landlord,admin,moderator")]
        //[Route("landlords/manage/messages")]
        //public async Task<IActionResult> ListMessages()
        //{
        //    var landlordId = GetLoginedLandlordId();

        //    var landlordMessages = await _context.Messages.Where(m => m.Landlord.Id == landlordId)
        //        .Include(m => m.Tenant)
        //        .Include(m=> m.Apartment)
        //        .ToListAsync();

        //    ViewData["Messages"] = landlordMessages;

        //    return View(landlordMessages);
        //}



        #endregion

        #region Tenant

        [Authorize(Roles = "landlord,admin,moderator")]
        [Route("landlords/manage/tenants")]
        public async Task<IActionResult> ListTenants()
        {
            var landlordId = await GetLoginedLandlordId();

            //var landlordsRentedApartments =
            //    await _context.RentedApartments.Where(ra => ra.Apartment.FKLandlordId == landlordId)
            //        .Include(ra => ra.Apartment)
            //        .Include(ra => ra.Tenant)
            //        .ToListAsync();

            var landlordsRentedApartments = await _rentedApartmentService.GetRentedApartmentsByLandlordIdAsync(landlordId);

            return View(landlordsRentedApartments.Data);
        }

        [Authorize(Roles = "landlord,admin,moderator")]
        [Route("landlords/manage/apartments/{rentedApartmentId}")]
        public async Task<IActionResult> GetSelectedRentedApartment(int? rentedApartmentId)
        {
            if (rentedApartmentId == null || !_rentedApartmentService.IsAnyRentedApartmentExist())
            {
                return NotFound();
            }

            //var rentedApartment = await _context.RentedApartments
            //    .Include(ra => ra.Apartment)
            //    .Include(ra => ra.Tenant)
            //    .FirstOrDefaultAsync(ra => ra.FKTenantId == tenantId);
            var rentedApartment = await _rentedApartmentService.GetRentedApartmentByIdAsync(rentedApartmentId);

            if (!rentedApartment.Success)
            {
                return NotFound();
            }

            return View(rentedApartment.Data);
        }

        [Authorize(Roles = "landlord,admin,moderator")]
        [Route("landlords/manage/tenants/add")]
        public IActionResult AddTenantIntoApartment()
        {
            SetViewBags();
            return View();
        }

        [Authorize(Roles = "landlord,admin,moderator")]
        [Route("landlords/manage/tenants/add")]
        [HttpPost]
        public async Task<IActionResult> AddTenantIntoApartment([Bind("GovermentId")] Tenant tenant, [Bind("Id")] Apartment apartment)
        {
            //var tenantToAdd = await _context.Tenants.Where(t => t.GovermentId == tenant.GovermentId).FirstOrDefaultAsync();
            var tenantToAdd = await _tenantService.GetTenantByGovermentId(tenant.GovermentId);

            RentedApartment model = new()
            {
                Apartment = apartment
            };

            if (!tenantToAdd.Success)
            {
                ViewData["Hata"] = "TCNO ile Kiracı Bulunamadı";
                SetViewBags();
                return View(model);
            }

            RentedApartment rentedApartment = new()
            {
                FKApartmentId = apartment.Id,
                FKTenantId = tenantToAdd.Data.Id,
                Status = true
            };

            var result = await _rentedApartmentService.AddRentedApartmentAsync(rentedApartment);
            if(result.Success)            
                _notyf.Success(result.Message);
            
            else
            {
                _notyf.Error(result.Message);
                return View(model);
            }

            return RedirectToAction("ListTenants");
        }

        // GET: Landlords/Manage/Tenants/Delete/5
        [Authorize(Roles = "landlord,admin,moderator")]
        [Route("landlords/manage/tenants/delete/{tenantId}")]
        public async Task<IActionResult> DeleteTenant(int? tenantId)
        {
            if (tenantId == null || !_tenantService.IsAnyTenantExist())            
                return NotFound();

            //var tenant = await _context.Tenants.FirstOrDefaultAsync(t=> t.Id == tenantId);
            var tenant = await _tenantService.GetTenantByIdAsync(tenantId);

            if(!tenant.Success)
                return NotFound();

            return View(tenant.Data);
        }

        #endregion

        #endregion




        #region Helpers

        private bool LandlordExists(int id)
        {
            //return _context.Landlords.Any(e => e.Id == id);
            return _landlordService.IsAnyLandlordExist();
        }

        private void SetViewBags()
        {
            var loginedLandlordId = GetLoginedLandlordId().Result;
            var landlordsUntenantedApartments = _apartmentService.GetLandlordsUntenantedApartmentsAsync(loginedLandlordId).Result;
            if(landlordsUntenantedApartments.Success)
            {
                Dictionary<int, string> apartmentDetails = new();
                foreach (var apartment in landlordsUntenantedApartments.Data)
                {
                    apartmentDetails[apartment.Id] = "Block : " + apartment.Block + " - Floor : " + apartment.Floor + " - Door Number : " + apartment.DoorNumber;
                }

                ViewData["ApartmentDetails"] = new SelectList(apartmentDetails, "Key", "Value");
            }
        }

        public async Task<int> GetLoginedLandlordId()
        {
            var loginedLandlord = await _landlordService.GetLoginedLandlord(HttpContext);
            return loginedLandlord.Data.Id;
        }
        #endregion
    }
}
