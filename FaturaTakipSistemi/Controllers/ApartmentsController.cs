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

namespace FaturaTakip.Controllers
{
    public class ApartmentsController : Controller
    {
        private readonly InvoiceTrackContext _context;
        private readonly IApartmentService _apartmentService;

        public ApartmentsController(InvoiceTrackContext context,
            IApartmentService apartmentService)
        {
            _context = context;  
            _apartmentService = apartmentService;
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
            if (id == null || _context.Apartments == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
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
        public async Task<IActionResult> Create([Bind("Id,Floor,DoorNumber,Type,Block,FKLandlordId,RentPrice,Rented")] Apartment apartment)
        {
            SetBlockAndTypeData();
            var landlord = apartment.FKLandlordId;


            var isExist = await _context.Apartments.AnyAsync(a => a.Block == apartment.Block && a.Floor == apartment.Floor && a.DoorNumber == apartment.DoorNumber);

            if (isExist)
            {
                ViewData["Hata"] = "Apartman Zaten Bulunuyor.";
                return View(apartment);
            }

            if (ModelState.IsValid && apartment.Type != Type.None)
            {
                _context.Add(apartment);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
            }
            else
                ViewData["Hata"] = "Apartman Bilgileri Boş Bırakılamaz.";


            return View(apartment);
        }

        // GET: Apartments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Apartments == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment == null)
            {
                return NotFound();
            }
            SetBlockAndTypeData();
            return View(apartment);
        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Floor,DoorNumber,Type,Block,FKLandlordId,RentPrice,Rented")] Apartment apartment)
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
                    _context.Update(apartment);
                    await _context.SaveChangesAsync();
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
            if (id == null || _context.Apartments == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Apartments == null)
            {
                return Problem("Entity set 'InvoiceTrackContext.Apartments'  is null.");
            }
            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment != null)
            {
                _context.Apartments.Remove(apartment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartmentExists(int id)
        {
          return _context.Apartments.Any(e => e.Id == id);
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
