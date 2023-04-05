using AspNetCoreHero.ToastNotification.Abstractions;
using FaturaTakip.Business.Interface;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.Utils;
using FaturaTakip.Utils.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FaturaTakip.Controllers
{
    public class RentedApartmentsController : Controller
    {
        private readonly IRentedApartmentService _rentedApartmentService;
        private readonly IApartmentService _apartmentService;
        private readonly ILandlordService _landlordService;
        private readonly ITenantService _tenantService;
        private readonly INotyfService _notyf;

        public RentedApartmentsController(IRentedApartmentService rentedApartmentService,
            IApartmentService apartmentService,
            ILandlordService landlordService,
            ITenantService tenantService,
            INotyfService notyf
            )
        {
            _rentedApartmentService = rentedApartmentService;
            _apartmentService = apartmentService;
            _landlordService = landlordService;
            _tenantService = tenantService;
            _notyf = notyf;
        }

        // GET: RentedApartments
        [Authorize(Roles = "admin,moderator,landlord")]
        public async Task<IActionResult> Index()
        {
            var rentedApartments = await _rentedApartmentService.GetAllRentedApartmentsAsync();

            //if (!rentedApartments.Success)
            //    return View();

            return View(rentedApartments.Data);
        }

        // GET: RentedApartments/Details/5
        [Authorize(Roles = "admin,moderator,landlord")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || !_rentedApartmentService.IsAnyRentedApartmentExist())
            {
                return NotFound();
            }

            var rentedApartment = await _rentedApartmentService.GetRentedApartmentVMByIdAsync(id);
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
            await SetUntenantedApartmentAndTenantDataAsync();
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
            var result = await _rentedApartmentService.AddRentedApartmentAsync(rentedApartment);
            var actionResult = await ReturnPageActionResult(result, rentedApartment);
            return actionResult;
        }

        // GET: RentedApartments/Edit/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || !_rentedApartmentService.IsAnyRentedApartmentExist())
            {
                return NotFound();
            }

            var rentedApartment = await _rentedApartmentService.GetRentedApartmentByIdAsync(id);
            if (!rentedApartment.Success)
            {
                return NotFound();
            }


            await SetAllApartmentAndTenantDataAsync();
            return View(rentedApartment.Data);
        }

        // POST: RentedApartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,FKTenantId")] RentedApartment rentedApartment)
        {
            var rentedApartmentToEdit = await _rentedApartmentService.GetRentedApartmentByIdAsync(id);

            if (id != rentedApartment.Id)
            {
                return NotFound();
            }
            //rentedApartment.Apartment = rentedApartmentToEdit.Data.Apartment;
            rentedApartment.FKApartmentId = rentedApartmentToEdit.Data.FKApartmentId;

            var result = await _rentedApartmentService.UpdateRentedApartmentAsync(rentedApartment);
            var actionResult = await ReturnPageActionResult(result, rentedApartment);
            return actionResult;
        }

        // GET: RentedApartments/Delete/5
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || !_rentedApartmentService.IsAnyRentedApartmentExist())
            {
                return NotFound();
            }

            var rentedApartment = await _rentedApartmentService.GetRentedApartmentByIdAsync(id);
            if (!rentedApartment.Success)
            {
                return NotFound();
            }

            return View(rentedApartment.Data);
        }

        // POST: RentedApartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!_rentedApartmentService.IsAnyRentedApartmentExist())
            {
                return Problem("Entity set 'InvoiceTrackContext.RentedApartments'  is null.");
            }

            var result = await _rentedApartmentService.DeleteRentedApartmentAsync(id);
            var actionResult = ReturnPageActionResult(result);
            return actionResult;
        }

        #region Helpers
        private async Task SetAllApartmentAndTenantDataAsync()
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
            ViewData["FKTenantId"] = new SelectList(tenantsInfo.OrderBy(x => x.Key), "Key", "Value");

            //var landlords = await _landlordService.GetLandlordsViewDataAsync();
            //Dictionary<int, string> landlordInfo = new Dictionary<int, string>();
            //foreach (var landlord in landlords.Data)
            //{
            //    landlordInfo.Add(landlord.Id, landlord.GovermentIdAndName);
            //}

            //ViewData["FKLandlordId"] = new SelectList(landlordInfo.OrderBy(x => x.Key), "Key", "Value");
        }
        private async Task SetUntenantedApartmentAndTenantDataAsync()
        {
            var allApartments = await _apartmentService.GetAllApartmentsAsync();
            var untenantedApartments = allApartments.Data.Where(a => a.Rented == false);
            Dictionary<int, string> apartmentsInfo = new Dictionary<int, string>();
            foreach (var apartment in untenantedApartments)
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
            ViewData["FKTenantId"] = new SelectList(tenantsInfo.OrderBy(x => x.Key), "Key", "Value");
        }



        private IActionResult ReturnPageActionResult(Result result)
        {
            if (ModelState.IsValid)
            {
                if (result.Success)
                {
                    _notyf.Success(result.Message);
                }
                else
                {
                    _notyf.Error(result.Message);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IActionResult> ReturnPageActionResult<T>(Result result, T model) where T : new()
        {
            if (ModelState.IsValid)
            {
                if (result.Success)
                {
                    _notyf.Success(result.Message);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _notyf.Error(result.Message);
                    await SetAllApartmentAndTenantDataAsync();
                    return View(model);
                }
            }
            _notyf.Error(Messages.InputsCannotBeNull);
            await SetAllApartmentAndTenantDataAsync();
            return View(model);
        }
        #endregion
    }
}
