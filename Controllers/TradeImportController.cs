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
    public class TradeImportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TradeImportController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: TradeImport
        public async Task<IActionResult> Index()
        {
            return View(await _context.TradeImport.ToListAsync());
        }

        // GET: TradeImport/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeImport = await _context.TradeImport
                .SingleOrDefaultAsync(m => m.TradeImportID == id);
            if (tradeImport == null)
            {
                return NotFound();
            }

            return View(tradeImport);
        }

        // GET: TradeImport/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TradeImport/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TradeImportID,ExternalReference,Value,Quantity,TransactionDate,ImportDate")] TradeImport tradeImport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tradeImport);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tradeImport);
        }

        // GET: TradeImport/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeImport = await _context.TradeImport.SingleOrDefaultAsync(m => m.TradeImportID == id);
            if (tradeImport == null)
            {
                return NotFound();
            }
            return View(tradeImport);
        }

        // POST: TradeImport/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TradeImportID,ExternalReference,Value,Quantity,TransactionDate,ImportDate")] TradeImport tradeImport)
        {
            if (id != tradeImport.TradeImportID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tradeImport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TradeImportExists(tradeImport.TradeImportID))
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
            return View(tradeImport);
        }

        // GET: TradeImport/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeImport = await _context.TradeImport
                .SingleOrDefaultAsync(m => m.TradeImportID == id);
            if (tradeImport == null)
            {
                return NotFound();
            }

            return View(tradeImport);
        }

        // POST: TradeImport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tradeImport = await _context.TradeImport.SingleOrDefaultAsync(m => m.TradeImportID == id);
            _context.TradeImport.Remove(tradeImport);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool TradeImportExists(int id)
        {
            return _context.TradeImport.Any(e => e.TradeImportID == id);
        }
    }
}
