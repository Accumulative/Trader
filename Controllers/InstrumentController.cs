using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TraderData;
using TraderData.Models.TradeImportModels;
using Trader.Models.InstrumentViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System;

namespace Trader.Controllers
{

    public class InstrumentController : Controller
    {
        private readonly IReference _reference;
		

        public InstrumentController(IReference reference)
        {
            _reference = reference;

        }
		
        // GET: Instrument
        public async Task<IActionResult> Index()
        {
            return View(await _reference.getInstruments());
        }

        // GET: Instrument/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Instrument instrument = await _reference.GetInstrumentById((int)id);
            if (instrument == null)
            {
                return NotFound();
            }

            return View(instrument);
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
        public async Task<IActionResult> Create([Bind("InstrumentID,Name,Ticker")] Instrument instrument)
        {
            if (ModelState.IsValid)
            {
                await _reference.AddInstrument(instrument);
                
                return RedirectToAction("Index");
            }
            return View(instrument);
        }

        public async Task<IActionResult> Graph()
        {
            var ins = await _reference.getInstruments();
            var start = DateTime.Now.AddYears(-1);
            var end = DateTime.Now;
            var dates = Enumerable.Range(0, 1 + end.Subtract(start).Days)
                          .Select(offset => start.AddDays(offset))
                          .ToList();

            InstrumentGraphFilter filters = new InstrumentGraphFilter
            {
                InstrumentList = new SelectList(ins.Select(x => new { Id = x.InstrumentID, Value = x.Name }), "Id", "Value"), //sometimes null)
                Month = new SelectList(dates.Select(x => x.Month + "/" + x.Year).Distinct().Select(x => new { Id = x, Value = x }), "Id", "Value")

            };
            return View(filters);
        }

        public IActionResult InstrumentGraphViewComponent(InstrumentGraphFilter filterModel)
        {
            return ViewComponent("InstrumentGraphViewComponent", filterModel);
        }

        // GET: Instrument/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Instrument instrument = await _reference.GetInstrumentById((int)id);
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
        public async Task<IActionResult> Edit(int id, [Bind("InstrumentID,Name,Ticker")] Instrument instrument)
        {
            if (id != instrument.InstrumentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var res = await _reference.EditInstrument(instrument);
                if (res)
                    return RedirectToAction("Index");
                else
                    return NotFound();
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

            Instrument instrument = await _reference.GetInstrumentById((int)id);
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
            await _reference.DeleteInstrument(id);
            return RedirectToAction("Index");
        }

        
    }
}
