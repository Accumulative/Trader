using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraderData;
using TraderData.Models.TradeImportModels;

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
            return View(await _context.InstrumentCache());
        }

        // GET: Instrument/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IEnumerable<Instrument> instrument = await _context.InstrumentCache();
            var list = instrument
                .SingleOrDefault(m => m.InstrumentID == id);
            if (instrument == null)
            {
                return NotFound();
            }

            return View(list);
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
        public async Task<IActionResult> Create([Bind("InstrumentID,Name")] Instrument instrument)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instrument);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(instrument);
        }

        // GET: Instrument/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IEnumerable<Instrument> list = await _context.InstrumentCache();
            var instrument = list.SingleOrDefault(m => m.InstrumentID == id);
            if (instrument == null)
            {
                return NotFound();
            }
            return View(instrument);
        }

        // POST: Instrument/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstrumentID,Name")] Instrument instrument)
        {
            if (id != instrument.InstrumentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instrument);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstrumentExists(instrument.InstrumentID))
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
            return View(instrument);
        }

        // GET: Instrument/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			IEnumerable<Instrument> list = await _context.InstrumentCache();
			var instrument = list.SingleOrDefault(m => m.InstrumentID == id);
            if (instrument == null)
            {
                return NotFound();
            }

            return View(instrument);
        }

        // POST: Instrument/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instrument = await _context.Instrument.SingleOrDefaultAsync(m => m.InstrumentID == id);
            _context.Instrument.Remove(instrument);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool InstrumentExists(int id)
        {
            return _context.Instrument.Any(e => e.InstrumentID == id);
        }
    }
}
