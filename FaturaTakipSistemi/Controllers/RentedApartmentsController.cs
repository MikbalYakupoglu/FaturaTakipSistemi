﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.Models;

namespace FaturaTakip.Controllers
{
    public class RentedApartmentsController : Controller
    {
        private readonly InvoiceTrackContext _context;

        public RentedApartmentsController(InvoiceTrackContext context)
        {
            _context = context;
        }

        // GET: RentedApartments
        public async Task<IActionResult> Index()
        {
            var invoiceTrackContext = _context.RentedApartments.Include(r => r.Apartment).Include(r => r.Landlord).Include(r => r.Tenant);
            return View(await invoiceTrackContext.ToListAsync());
        }

        // GET: RentedApartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RentedApartments == null)
            {
                return NotFound();
            }

            var rentedApartment = await _context.RentedApartments
                .Include(r => r.Apartment)
                .Include(r => r.Landlord)
                .Include(r => r.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentedApartment == null)
            {
                return NotFound();
            }

            return View(rentedApartment);
        }

        // GET: RentedApartments/Create
        public IActionResult Create()
        {
            ViewData["FKApartmentId"] = new SelectList(_context.Apartments, "Id", "Id");
            ViewData["FKLandlordId"] = new SelectList(_context.Landlords, "Id", "Id");
            ViewData["FKTenantId"] = new SelectList(_context.Tenants, "Id", "Id");
            return View();
        }

        // POST: RentedApartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,FKTenantId,FKApartmentId,FKLandlordId")] RentedApartment rentedApartment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rentedApartment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FKApartmentId"] = new SelectList(_context.Apartments, "Id", "Id", rentedApartment.FKApartmentId);
            ViewData["FKLandlordId"] = new SelectList(_context.Landlords, "Id", "Id", rentedApartment.FKLandlordId);
            ViewData["FKTenantId"] = new SelectList(_context.Tenants, "Id", "Id", rentedApartment.FKTenantId);
            return View(rentedApartment);
        }

        // GET: RentedApartments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RentedApartments == null)
            {
                return NotFound();
            }

            var rentedApartment = await _context.RentedApartments.FindAsync(id);
            if (rentedApartment == null)
            {
                return NotFound();
            }
            ViewData["FKApartmentId"] = new SelectList(_context.Apartments, "Id", "Id", rentedApartment.FKApartmentId);
            ViewData["FKLandlordId"] = new SelectList(_context.Landlords, "Id", "Id", rentedApartment.FKLandlordId);
            ViewData["FKTenantId"] = new SelectList(_context.Tenants, "Id", "Id", rentedApartment.FKTenantId);
            return View(rentedApartment);
        }

        // POST: RentedApartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,FKTenantId,FKApartmentId,FKLandlordId")] RentedApartment rentedApartment)
        {
            if (id != rentedApartment.Id)
            {
                return NotFound();
            }

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
            ViewData["FKApartmentId"] = new SelectList(_context.Apartments, "Id", "Id", rentedApartment.FKApartmentId);
            ViewData["FKLandlordId"] = new SelectList(_context.Landlords, "Id", "Id", rentedApartment.FKLandlordId);
            ViewData["FKTenantId"] = new SelectList(_context.Tenants, "Id", "Id", rentedApartment.FKTenantId);
            return View(rentedApartment);
        }

        // GET: RentedApartments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RentedApartments == null)
            {
                return NotFound();
            }

            var rentedApartment = await _context.RentedApartments
                .Include(r => r.Apartment)
                .Include(r => r.Landlord)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RentedApartments == null)
            {
                return Problem("Entity set 'InvoiceTrackContext.RentedApartments'  is null.");
            }
            var rentedApartment = await _context.RentedApartments.FindAsync(id);
            if (rentedApartment != null)
            {
                _context.RentedApartments.Remove(rentedApartment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentedApartmentExists(int id)
        {
          return _context.RentedApartments.Any(e => e.Id == id);
        }
    }
}