using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.Utils;
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
        private readonly InvoiceTrackContext _context;
        private readonly UserManager<InvoiceTrackUser> _userManager;


        public LandlordsController(InvoiceTrackContext context,
            UserManager<InvoiceTrackUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #region Landlord
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Landlords.ToListAsync());
        }

        // GET: Landlords/Details/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Landlords == null)
            {
                return NotFound();
            }

            var landlord = await _context.Landlords
                .FirstOrDefaultAsync(m => m.Id == id);
            if (landlord == null)
            {
                return NotFound();
            }

            return View(landlord);
        }

        // GET: Landlords/Create
        [Authorize(Roles = "admin,moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Landlords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Create([Bind($"{nameof(Landlord.Id)},{nameof(Landlord.Name)},{nameof(Landlord.LastName)},{nameof(Landlord.GovermentId)}," +
                                                      $"{nameof(Landlord.YearOfBirth)},{nameof(Landlord.Email)},{nameof(Landlord.Phone)}")] Landlord landlord)
        {
            if (ModelState.IsValid)
            {
                if (!MernisUtils.VerifyGovermentId(landlord.GovermentId, landlord.Name, landlord.LastName, landlord.YearOfBirth).Result)
                {
                    ViewData["VerificationError"] = "Girdiğiniz Bilgiler Yanlış, Lütfen kontrol ediniz.";
                    return View(landlord);
                }
                _context.Add(landlord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(landlord);
        }

        // GET: Landlords/Edit/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Landlords == null)
            {
                return NotFound();
            }

            var landlord = await _context.Landlords.FindAsync(id);
            if (landlord == null)
            {
                return NotFound();
            }
            return View(landlord);
        }

        // POST: Landlords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(int id, [Bind(include: $"{nameof(Landlord.Id)},{nameof(Landlord.Name)},{nameof(Landlord.LastName)},{nameof(Landlord.GovermentId)}," +
                                                            $"{nameof(Landlord.YearOfBirth)},{nameof(Landlord.Email)},{nameof(Landlord.Phone)}")] Landlord landlord)
        {
            if (id != landlord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //if (!MernisUtils.VerifyGovermentId(landlord.GovermentId, landlord.Name, landlord.LastName, landlord.YearOfBirth).Result)
                //{
                //    ViewData["VerificationError"] = "Girdiğiniz Bilgiler Yanlış, Lütfen kontrol ediniz.";
                //    return View(landlord);
                //}
                try
                {
                    _context.Update(landlord);
                    await _context.SaveChangesAsync();
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
            if (id == null || _context.Landlords == null)
            {
                return NotFound();
            }

            var landlord = await _context.Landlords
                .FirstOrDefaultAsync(m => m.Id == id);
            if (landlord == null)
            {
                return NotFound();
            }

            return View(landlord);
        }

        // POST: Landlords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Landlords == null)
            {
                return Problem("Entity set 'InvoiceTrackContext.Landlords'  is null.");
            }
            var landlord = await _context.Landlords.FindAsync(id);
            if (landlord != null)
            {
                _context.Landlords.Remove(landlord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        #endregion


        #region Manage

        #region Apartment

        [Authorize(Roles = "landlord,admin,moderator")]
        [Route("landlords/manage/apartments")]
        public async Task<IActionResult> ListApartments()
        {
            var landlordId = GetLoginedLandlordId();


            var landlordsApartments = await _context.Apartments.Where(a => a.Landlord.Id == landlordId)
                .Include(a => a.Landlord)
                .ToListAsync(); 

            ViewData["Apartments"] = landlordsApartments;
            
            return View(landlordsApartments);
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
            var landlordId = GetLoginedLandlordId();

            var landlordsRentedApartments =
                await _context.RentedApartments.Where(ra => ra.Apartment.FKLandlordId == landlordId)
                    .Include(ra => ra.Apartment)
                    .Include(ra => ra.Tenant)
                    .ToListAsync();

            return View(landlordsRentedApartments);
        }

        [Authorize(Roles = "landlord,admin,moderator")]
        [Route("landlords/manage/tenants/{tenantId}")]
        public async Task<IActionResult> GetSelectedTenant(int? tenantId)
        {
            if (tenantId == null || _context.Tenants == null)
            {
                return NotFound();
            }

            var rentedApartment = await _context.RentedApartments
                .Include(ra => ra.Apartment)
                .Include(ra => ra.Tenant)
                .FirstOrDefaultAsync(ra => ra.FKTenantId == tenantId);
            if (rentedApartment == null)
            {
                return NotFound();
            }

            return View(rentedApartment);
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
            var tenantToAdd = await _context.Tenants.Where(t => t.GovermentId == tenant.GovermentId).FirstOrDefaultAsync();

            if (tenantToAdd == null)
            {
                ViewData["Hata"] = "TCNO ile Kiracı Bulunamadı";
                SetViewBags();

                RentedApartment model = new()
                {
                    Apartment = apartment
                };
                return View(model);
            }

            RentedApartment rentedApartment = new()
            {
                FKApartmentId = apartment.Id,
                FKTenantId = tenantToAdd.Id,
                Status = true
            };
            _context.Apartments.First(a => a.Id == rentedApartment.FKApartmentId).Rented = true;

            _context.Add(rentedApartment);
            await _context.SaveChangesAsync();
            return RedirectToAction("ListTenants");
        }

        // GET: Landlords/Manage/Tenants/Delete/5
        [Authorize(Roles = "landlord,admin,moderator")]
        [Route("landlords/manage/tenants/delete/{tenantId}")]
        public async Task<IActionResult> DeleteTenant(int? tenantId)
        {
            if (tenantId == null || _context.Tenants == null)            
                return NotFound();
            
            var tenant = await _context.Tenants.FirstOrDefaultAsync(t=> t.Id == tenantId);

            if(tenant == null)
                return NotFound();

            return View(tenant);
        }

        #endregion

        #endregion




        #region Helpers

        private bool LandlordExists(int id)
        {
            return _context.Landlords.Any(e => e.Id == id);
        }

        private void SetViewBags()
        {
            var loginedLandlordId = GetLoginedLandlordId();
            var landlordsApartments = _context.Apartments.Where(a => a.FKLandlordId == loginedLandlordId);
            var landlordsUnrentedApartments = landlordsApartments.Except(_context.RentedApartments.Select(ra => ra.Apartment));

            Dictionary<int, string> apartmentDetails = new();
            foreach (var apartment in landlordsUnrentedApartments)
            {
                apartmentDetails[apartment.Id] = "Block : " + apartment.Block + " - Floor : " + apartment.Floor + " - Door Number : " + apartment.DoorNumber;
            }

            ViewData["ApartmentDetails"] = new SelectList((IEnumerable)apartmentDetails, "Key", "Value");
        }

        public int GetLoginedLandlordId()
        {
            var loginedUser = _userManager.GetUserId(HttpContext.User);
            var landlordId = (from u in _context.Users
                    join l in _context.Landlords
                        on u.GovermentId equals l.GovermentId
                    where u.Id == loginedUser
                    select l.Id
                ).FirstOrDefault();

            return landlordId;
        }
        #endregion
    }
}
