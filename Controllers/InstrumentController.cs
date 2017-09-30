using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trader.Data;
using Trader.Models.TradeImportModels;

namespace Trader.Controllers
{
    public class InstrumentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstrumentController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Instrument
        public async Task<IActionResult> Index()
        {
            return View(await _context.InstrumentModel.ToListAsync());
        }

        // GET: Instrument/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrumentModel = await _context.InstrumentModel
                .SingleOrDefaultAsync(m => m.InstrumentModelID == id);
            if (instrumentModel == null)
            {
                return NotFound();
            }

            return View(instrumentModel);
        }

        // GET: Instrument/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instrument/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstrumentModelID,Name")] InstrumentModel instrumentModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instrumentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(instrumentModel);
        }

        // GET: Instrument/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrumentModel = await _context.InstrumentModel.SingleOrDefaultAsync(m => m.InstrumentModelID == id);
            if (instrumentModel == null)
            {
                return NotFound();
            }
            return View(instrumentModel);
        }

        // POST: Instrument/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstrumentModelID,Name")] InstrumentModel instrumentModel)
        {
            if (id != instrumentModel.InstrumentModelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instrumentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstrumentModelExists(instrumentModel.InstrumentModelID))
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
            return View(instrumentModel);
        }

        // GET: Instrument/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrumentModel = await _context.InstrumentModel
                .SingleOrDefaultAsync(m => m.InstrumentModelID == id);
            if (instrumentModel == null)
            {
                return NotFound();
            }

            return View(instrumentModel);
        }

        // POST: Instrument/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instrumentModel = await _context.InstrumentModel.SingleOrDefaultAsync(m => m.InstrumentModelID == id);
            _context.InstrumentModel.Remove(instrumentModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool InstrumentModelExists(int id)
        {
            return _context.InstrumentModel.Any(e => e.InstrumentModelID == id);
        }
    }
}
