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
using FaturaTakip.Business.Abstract;

namespace FaturaTakip.Controllers
{
    public class ApartmentsController : Controller
    {
        private readonly IApartmentService _apartmentService;
        private readonly InvoiceTrackContext _context;

        public ApartmentsController(IApartmentService apartmentService, InvoiceTrackContext context)
        {
            _apartmentService = apartmentService;
            _context = context;
        }

        // GET: Apartments
        public async Task<IActionResult> Index()
        {
            return View(_apartmentService.GetAllAsync().Result.Data); // Result.Data
        }

        // GET: Apartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || !_apartmentService.GetAllAsync().Result.IsSuccess)
            {
                return NotFound();
            }

            var apartment = await _apartmentService.GetByIdAsync(id);
            if (apartment.Data == null)
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
        public async Task<IActionResult> Create([Bind("Id,Floor,DoorNumber,Type,Block,FKLandlordId")] Apartment apartment)
        {
            SetBlockAndTypeData();
            var landlord = apartment.FKLandlordId;


            if (ModelState.IsValid && apartment.Type != Type.None)
            {
                 var result = await _apartmentService.AddAsync(apartment);
                 if (!result.IsSuccess)
                 {
                     ViewData["Hata"] = result.Message;
                     return View(apartment);
                 }
                 return RedirectToAction(nameof(Index));
            }
            else
                ViewData["Hata"] = "Apartman Bilgileri Boş Bırakılamaz.";


            return View(apartment);
        }

        // GET: Apartments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || !_apartmentService.GetAllAsync().Result.IsSuccess)
            {
                return NotFound();
            }

            var apartment = await _apartmentService.GetByIdAsync(id);
            if (apartment.Data == null)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Floor,DoorNumber,Type,Block")] Apartment apartment)
        {
            SetBlockAndTypeData();

            if (id != apartment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _apartmentService.UpdateAsync(apartment);
                    if (!result.IsSuccess)
                    {
                        ViewData["Hata"] = result.Message;
                        return View(result.Data);

                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartmentExists(apartment.Id))
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
                ViewData["Hata"] = "Apartman Bilgileri Boş Bırakılamaz.";

            return View(apartment);
        }

        // GET: Apartments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || !_apartmentService.GetAllAsync().Result.IsSuccess)
            {
                return NotFound();
            }

            var apartment = await _apartmentService.GetByIdAsync(id);
            if (apartment.Data == null)
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
            if (!_apartmentService.GetAllAsync().Result.IsSuccess)
            {
                return Problem("Entity set 'InvoiceTrackContext.Apartments'  is null.");
            }

            var apartment = await _apartmentService.GetByIdAsync(id);
            if (apartment.Data != null)
            {
                var result = await _apartmentService.DeleteAsync(apartment.Data);
                if (!result.IsSuccess)
                {
                    ViewData["Hata"] = result.Message;
                    return View(apartment.Data);
                }
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ApartmentExists(int id)
        {
            return _apartmentService.GetByIdAsync(id).Result.IsSuccess;
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


            var landlords = _context.Landlords.ToListAsync().Result;
            Dictionary<int, string> landlordInfo = new Dictionary<int, string>();

            foreach (var landlord in landlords)
            {
                landlordInfo.Add(landlord.Id,String.Join("",landlord.GovermentId, " - ", landlord.Name, " ", landlord.LastName));
            }

            ViewData["Landlords"] = new SelectList(landlordInfo.OrderBy(x=> x.Key), "Key", "Value");
        }
    }
}
