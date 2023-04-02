using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FaturaTakip.Data;
using Type = FaturaTakip.Data.Models.Type;
using FaturaTakip.Utils;
using FaturaTakip.Data.Models;
using FaturaTakip.Models;
using System.Linq;
using System.Diagnostics;
using FaturaTakip.Business.Interface;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace FaturaTakip.Controllers
{
    public class ApartmentsController : Controller
    {
        private readonly IApartmentService _apartmentService;
        private readonly ILandlordService _landlordService;
        private readonly INotyfService _notyf;

        public ApartmentsController(InvoiceTrackContext context,
            IApartmentService apartmentService,
            ILandlordService landlordService,
            INotyfService notyf)
        { 
            _apartmentService = apartmentService;
            _landlordService = landlordService;
            _notyf = notyf;
        }

        // GET: Apartments
        public async Task<IActionResult> Index()
        {
            var apartments = await _apartmentService.GetAllApartmentsWithLandlordsAsync();
            return View(apartments.Data);
        }

        // GET: Apartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null ||! _apartmentService.IsAnyApartmentExist())
            {
                return NotFound();
            }

            var apartment = await _apartmentService.GetApartmentByIdWithLandlordAsync(id);
            if (!apartment.Success)
            {
                return NotFound();
            }

            return View(apartment.Data);
        }

        // GET: Apartments/Create
        public IActionResult Create()
        {
            SetBlockAndTypeData();
            return View();
        }

        // POST: Apartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FKLandlordId,Block,Floor,DoorNumber,Type,RentPrice,Rented")] Apartment apartment)
        {
            SetBlockAndTypeData();

            if (ModelState.IsValid && apartment.Type != Type.None)
            {
                var result = await _apartmentService.AddApartmentAsync(apartment);
                if (!result.Success)
                {
                    _notyf.Error(result.Message);
                    return View(apartment);
                }
                else
                {
                    _notyf.Success(result.Message);
                    return RedirectToAction(nameof(Index));
                }
            }
            else
                _notyf.Error(Messages.InputsCannotBeNull);


            return View(apartment);
        }

        // GET: Apartments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null ||! _apartmentService.IsAnyApartmentExist())
            {
                return NotFound();
            }

            var apartment = await _apartmentService.GetApartmentByIdAsync(id);
            if (!apartment.Success)
            {
                return NotFound();
            }
            SetBlockAndTypeData();
            return View(apartment.Data);
        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FKLandlordId,Block,Floor,DoorNumber,Type,RentPrice")] Apartment apartment)
        {
            SetBlockAndTypeData();

            var apartmentToEdit = await _apartmentService.GetApartmentByIdAsync(id);

            if (!apartmentToEdit.Success)
            {
                return NotFound();
            }

            apartment.Rented = apartmentToEdit.Data.Rented;
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _apartmentService.UpdateApartmentAsync(apartment);
                    if (result.Success)
                        _notyf.Success(result.Message);
                    else
                    {
                        _notyf.Error(result.Message);
                        return View(apartment);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _apartmentService.IsApartmentExistAsync(id))
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
            else
                _notyf.Error(Messages.InputsCannotBeNull);

            return View(apartment);
        }

        // GET: Apartments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null ||! _apartmentService.IsAnyApartmentExist())
            {
                return NotFound();
            }

            var apartment = await _apartmentService.GetApartmentByIdWithLandlordAsync(id);

            if (!apartment.Success)
            {
                return NotFound();
            }

            return View(apartment.Data);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (! _apartmentService.IsAnyApartmentExist())
            {
                return Problem("Entity set 'InvoiceTrackContext.Apartments'  is null.");
            }
            var apartment = await _apartmentService.GetApartmentByIdAsync(id);

            if (apartment.Success)
            {
                var result = await _apartmentService.RemoveApartmentAsync(id);
                if (result.Success)
                    _notyf.Success(result.Message);
                else
                    _notyf.Error(result.Message);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private void SetBlockAndTypeData()
        {
            var enumDataTypes = from Type e in Enum.GetValues(typeof(Type))
                                select new
                                {
                                    ID = e.ToString(),
                                    Name = EnumExtensions.GetDescriptionOfEnum((Type)e)
                                };
            ViewData["Types"] = new SelectList(enumDataTypes, "ID", "Name");

            //List<Type> typeList = new List<Type>();
            //foreach (var type in (Type[])Enum.GetValues(typeof(Type)))
            //{
            //    typeList.Add(type);
            //}

            List<Block> blockList = new List<Block>();
            foreach (var block in (Block[])Enum.GetValues(typeof(Block)))
            {
                blockList.Add(block);
            }


            //ViewData["Types"] = new SelectList(typeList);
            ViewData["Blocks"] = new SelectList(blockList);


            //var landlords = _context.Landlords.ToListAsync().Result;
            var landlords = _landlordService.GetAllLandlordsAsync().Result.Data;
            Dictionary<int, string> landlordInfo = new Dictionary<int, string>();

            foreach (var landlord in landlords)
            {
                landlordInfo.Add(landlord.Id,String.Join("",landlord.GovermentId, " - ", landlord.Name, " ", landlord.LastName));
            }

            ViewData["Landlords"] = new SelectList(landlordInfo.OrderBy(x=> x.Key), "Key", "Value");
        }
    }
}
