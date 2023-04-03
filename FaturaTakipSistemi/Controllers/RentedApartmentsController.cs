using FaturaTakip.Business.Interface;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.Controllers
{
    public class RentedApartmentsController : Controller
    {
        private readonly InvoiceTrackContext _context;
        private readonly IRentedApartmentService _rentedApartmentService;
        private readonly IApartmentService _apartmentService;
        private readonly ILandlordService _landlordService;
        private readonly ITenantService _tenantService;

        public RentedApartmentsController(InvoiceTrackContext context,
            IRentedApartmentService rentedApartmentService,
            IApartmentService apartmentService,
            ILandlordService landlordService,
            ITenantService tenantService)
        {
            _context = context;
            _rentedApartmentService = rentedApartmentService;
            _apartmentService = apartmentService;
            _landlordService = landlordService;
            _tenantService = tenantService;
        }

        // GET: RentedApartments
        [Authorize(Roles = "admin,moderator,landlord")]
        public async Task<IActionResult> Index()
        {
            var rentedApartments = await _rentedApartmentService.GetAllRentedApartmentsWithApartmentsAndTenantsAsync();

            if (!rentedApartments.Success)
                return View();

            return View(rentedApartments.Data);
        }

        // GET: RentedApartments/Details/5
        [Authorize(Roles = "admin,moderator,landlord")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || ! _rentedApartmentService.IsAnyRentedApartmentExist())
            {
                return NotFound();
            }

            var rentedApartment = await _rentedApartmentService.GetRentedApartmentByIdWithApartmentAndTenantAsync(id);
            if (!rentedApartment.Success)
            {
                return NotFound();
            }

            return View(rentedApartment.Data);
        }

        // GET: RentedApartments/Create
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Create()
        {
            await SetApartmentAndLandlordAndTenantDataAsync();
            return View();
        }

        // POST: RentedApartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Create([Bind("Id,Status,FKTenantId,FKApartmentId")] RentedApartment rentedApartment)
        {
            if (ModelState.IsValid)
            {
                rentedApartment.RentTime = DateTime.Now;
                _context.Add(rentedApartment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await SetApartmentAndLandlordAndTenantDataAsync();
            return View(rentedApartment);
        }

        // GET: RentedApartments/Edit/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _rentedApartmentService.IsAnyRentedApartmentExist())
            {
                return NotFound();
            }

            var rentedApartment = await _context.RentedApartments
                .Include(ra => ra.Apartment)
                .FirstOrDefaultAsync(ra => ra.Id == id);

            if (rentedApartment == null)
            {
                return NotFound();
            }


            await SetApartmentAndLandlordAndTenantDataAsync();
            return View(rentedApartment);
        }

        // POST: RentedApartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,FKTenantId,FKApartmentId,FKLandlordId")] RentedApartment rentedApartment)
        {
            var rentedApartmentToEdit = await _context.RentedApartments.Include(ra=> ra.Apartment).FirstOrDefaultAsync(ra => ra.Id == id);

            if (id != rentedApartment.Id)
            {
                return NotFound();
            }

            rentedApartment.Apartment = rentedApartmentToEdit.Apartment;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentedApartment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentedApartmentExists(rentedApartment.Id))
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

            await SetApartmentAndLandlordAndTenantDataAsync();
            return View(rentedApartment);
        }

        // GET: RentedApartments/Delete/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _rentedApartmentService.IsAnyRentedApartmentExist())
            {
                return NotFound();
            }

            var rentedApartment = await _context.RentedApartments
                .Include(r => r.Apartment)
                .Include(r => r.Apartment.Landlord)
                .Include(r => r.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentedApartment == null)
            {
                return NotFound();
            }

            return View(rentedApartment);
        }

        // POST: RentedApartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_rentedApartmentService.IsAnyRentedApartmentExist())
            {
                return Problem("Entity set 'InvoiceTrackContext.RentedApartments'  is null.");
            }
            var rentedApartment = await _context.RentedApartments.FindAsync(id);
            if (rentedApartment != null)
            {
                _context.RentedApartments.Include(ra => ra.Apartment)
                    .FirstOrDefault(ra => ra.Id == id).Apartment.Rented = false;
                _context.RentedApartments.Remove(rentedApartment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentedApartmentExists(int id)
        {
          return _context.RentedApartments.Any(e => e.Id == id);
        }

        #region Helpers
        private async Task SetApartmentAndLandlordAndTenantDataAsync()
        {
            var apartments = await _apartmentService.GetAllApartmentsAsync();
            Dictionary<int, string> apartmentsInfo = new Dictionary<int, string>();
            foreach (var apartment in apartments.Data)
            {
                apartmentsInfo.Add(apartment.Id, String.Join(" - ", $"Block : {apartment.Block}", $"Floor : {apartment.Floor}", $"Door : {apartment.DoorNumber}"));
            }
            ViewData["FKApartmentId"] = new SelectList(apartmentsInfo.OrderBy(x => x.Key), "Key", "Value");

            var tenants = await _tenantService.GetTenantsViewDataAsync();
            Dictionary<int, string> tenantsInfo = new Dictionary<int, string>();
            foreach (var tenant in tenants.Data)
            {
                tenantsInfo.Add(tenant.Id, tenant.GovermentIdAndName);
            }
            ViewData["FKTenantId"] = new SelectList(tenantsInfo.OrderBy(x=> x.Key), "Key", "Value");

            var landlords = await _landlordService.GetLandlordsViewDataAsync();
            Dictionary<int, string> landlordInfo = new Dictionary<int, string>();
            foreach (var landlord in landlords.Data)
            {
                landlordInfo.Add(landlord.Id,landlord.GovermentIdAndName);
            }

            ViewData["FKLandlordId"] = new SelectList(landlordInfo.OrderBy(x=>x.Key), "Key", "Value");
        }
        #endregion
    }
}
