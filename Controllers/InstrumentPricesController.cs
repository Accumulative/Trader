using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TraderData;
using TraderData.Models.InstrumentModels;

namespace Trader.Controllers
{
    public class InstrumentPricesController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public InstrumentPricesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: InstrumentPrices
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.InstrumentPrice.Include(i => i.Exchange).Include(i => i.Instrument);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: InstrumentPrices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrumentPrice = await _context.InstrumentPrice
                .Include(i => i.Exchange)
                .Include(i => i.Instrument)
                .SingleOrDefaultAsync(m => m.InstrumentPriceID == id);
            if (instrumentPrice == null)
            {
                return NotFound();
            }

            return View(instrumentPrice);
        }

        // GET: InstrumentPrices/Create
        public IActionResult Create()
        {
            ViewData["ExchangeID"] = new SelectList(_context.Exchange, "ExchangeId", "Name");
            ViewData["InstrumentID"] = new SelectList(_context.Instrument, "InstrumentID", "InstrumentID");
            return View();
        }

        // POST: InstrumentPrices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstrumentPriceID,ExchangeID,InstrumentID,DateTime,Price")] InstrumentPrice instrumentPrice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instrumentPrice);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ExchangeID"] = new SelectList(_context.Exchange, "ExchangeId", "Name", instrumentPrice.ExchangeID);
            ViewData["InstrumentID"] = new SelectList(_context.Instrument, "InstrumentID", "InstrumentID", instrumentPrice.InstrumentID);
            return View(instrumentPrice);
        }

        // GET: InstrumentPrices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrumentPrice = await _context.InstrumentPrice.SingleOrDefaultAsync(m => m.InstrumentPriceID == id);
            if (instrumentPrice == null)
            {
                return NotFound();
            }
            ViewData["ExchangeID"] = new SelectList(_context.Exchange, "ExchangeId", "Name", instrumentPrice.ExchangeID);
            ViewData["InstrumentID"] = new SelectList(_context.Instrument, "InstrumentID", "InstrumentID", instrumentPrice.InstrumentID);
            return View(instrumentPrice);
        }

        // POST: InstrumentPrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstrumentPriceID,ExchangeID,InstrumentID,DateTime,Price")] InstrumentPrice instrumentPrice)
        {
            if (id != instrumentPrice.InstrumentPriceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instrumentPrice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstrumentPriceExists(instrumentPrice.InstrumentPriceID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ExchangeID"] = new SelectList(_context.Exchange, "ExchangeId", "Name", instrumentPrice.ExchangeID);
            ViewData["InstrumentID"] = new SelectList(_context.Instrument, "InstrumentID", "InstrumentID", instrumentPrice.InstrumentID);
            return View(instrumentPrice);
        }

        // GET: InstrumentPrices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrumentPrice = await _context.InstrumentPrice
                .Include(i => i.Exchange)
                .Include(i => i.Instrument)
                .SingleOrDefaultAsync(m => m.InstrumentPriceID == id);
            if (instrumentPrice == null)
            {
                return NotFound();
            }

            return View(instrumentPrice);
        }

        // POST: InstrumentPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instrumentPrice = await _context.InstrumentPrice.SingleOrDefaultAsync(m => m.InstrumentPriceID == id);
            _context.InstrumentPrice.Remove(instrumentPrice);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool InstrumentPriceExists(int id)
        {
            return _context.InstrumentPrice.Any(e => e.InstrumentPriceID == id);
        }
    }
}
