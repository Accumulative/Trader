using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TraderData;
using Trader.Models.FileImportModels;
using TraderData.Models.FileImportModels;

namespace Trader.Controllers
{
    public class ExchangeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExchangeController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Exchange
        public async Task<IActionResult> Index()
        {
            return View(await _context.Exchange.ToListAsync());
        }

        // GET: Exchange/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exchange = await _context.Exchange
                .SingleOrDefaultAsync(m => m.ExchangeId == id);
            if (exchange == null)
            {
                return NotFound();
            }

            return View(exchange);
        }

        // GET: Exchange/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exchange/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExchangeId,Name,URL")] Exchange exchange)
        {
            exchange.DateCreated = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(exchange);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(exchange);
        }

        // GET: Exchange/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exchange = await _context.Exchange.SingleOrDefaultAsync(m => m.ExchangeId == id);
            if (exchange == null)
            {
                return NotFound();
            }
            return View(exchange);
        }

        // POST: Exchange/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExchangeId,Name,DateCreated,URL")] Exchange exchange)
        {
            if (id != exchange.ExchangeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exchange);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExchangeExists(exchange.ExchangeId))
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
            return View(exchange);
        }

        // GET: Exchange/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exchange = await _context.Exchange
                .SingleOrDefaultAsync(m => m.ExchangeId == id);
            if (exchange == null)
            {
                return NotFound();
            }

            return View(exchange);
        }

        // POST: Exchange/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exchange = await _context.Exchange.SingleOrDefaultAsync(m => m.ExchangeId == id);
            _context.Exchange.Remove(exchange);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ExchangeExists(int id)
        {
            return _context.Exchange.Any(e => e.ExchangeId == id);
        }
    }
}
