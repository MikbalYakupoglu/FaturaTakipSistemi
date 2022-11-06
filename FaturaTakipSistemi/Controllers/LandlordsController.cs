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
using FaturaTakip.Utils;

namespace FaturaTakip.Controllers
{
    public class LandlordsController : Controller
    {
        private readonly InvoiceTrackContext _context;

        public LandlordsController(InvoiceTrackContext context)
        {
            _context = context;
        }

        // GET: Landlords
        public async Task<IActionResult> Index()
        {
              return View(await _context.Landlords.ToListAsync());
        }

        // GET: Landlords/Details/5
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Landlords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> Edit(int id, [Bind(include: $"{nameof(Landlord.Id)},{nameof(Landlord.Name)},{nameof(Landlord.LastName)},{nameof(Landlord.GovermentId)}," +
                                                            $"{nameof(Landlord.YearOfBirth)},{nameof(Landlord.Email)},{nameof(Landlord.Phone)}")] Landlord landlord)
        {
            if (id != landlord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!MernisUtils.VerifyGovermentId(landlord.GovermentId, landlord.Name, landlord.LastName, landlord.YearOfBirth).Result)
                {
                    ViewData["VerificationError"] = "Girdiğiniz Bilgiler Yanlış, Lütfen kontrol ediniz.";
                    return View(landlord);
                }
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

        private bool LandlordExists(int id)
        {
          return _context.Landlords.Any(e => e.Id == id);
        }
    }
}
