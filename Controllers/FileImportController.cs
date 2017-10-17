using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TraderData;
using Trader.Models.FileImportModels;
using TraderData.Models.FileImportModels;

namespace Trader.Controllers
{
    [Authorize]
    public class FileImportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FileImportController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: FileImport
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FileImport.Include(f => f.Exchange);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FileImport/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileImport = await _context.FileImport
                .Include(f => f.Exchange)
                .SingleOrDefaultAsync(m => m.FileImportId == id);
            if (fileImport == null)
            {
                return NotFound();
            }

            return View(fileImport);
        }

        // GET: FileImport/Create
        public IActionResult Create()
        {
            ViewData["ExchangeId"] = new SelectList(_context.Exchange, "ExchangeId", "Name");
            return View();
        }

        // POST: FileImport/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FileImportId,Filename,ImportDate,ExchangeId")] FileImport fileImport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fileImport);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ExchangeId"] = new SelectList(_context.Exchange, "ExchangeId", "Name", fileImport.ExchangeId);
            return View(fileImport);
        }

        // GET: FileImport/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileImport = await _context.FileImport.SingleOrDefaultAsync(m => m.FileImportId == id);
            if (fileImport == null)
            {
                return NotFound();
            }
            ViewData["ExchangeId"] = new SelectList(_context.Exchange, "ExchangeId", "Name", fileImport.ExchangeId);
            return View(fileImport);
        }

        // POST: FileImport/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FileImportId,Filename,ImportDate,ExchangeId")] FileImport fileImport)
        {
            if (id != fileImport.FileImportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fileImport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileImportExists(fileImport.FileImportId))
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
            ViewData["ExchangeId"] = new SelectList(_context.Exchange, "ExchangeId", "Name", fileImport.ExchangeId);
            return View(fileImport);
        }

        // GET: FileImport/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileImport = await _context.FileImport
                .Include(f => f.Exchange)
                .SingleOrDefaultAsync(m => m.FileImportId == id);
            if (fileImport == null)
            {
                return NotFound();
            }

            return View(fileImport);
        }

        // POST: FileImport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fileImport = await _context.FileImport.SingleOrDefaultAsync(m => m.FileImportId == id);
            _context.FileImport.Remove(fileImport);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool FileImportExists(int id)
        {
            return _context.FileImport.Any(e => e.FileImportId == id);
        }
    }
}
