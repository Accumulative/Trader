using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TraderData;
using TraderData.Models.TradeImportModels;

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
        public async Task<IActionResult> Create([Bind("InstrumentID,Name")] Instrument instrument)
        {
            if (ModelState.IsValid)
            {
                await _reference.AddInstrument(instrument);
                
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
        public async Task<IActionResult> Edit(int id, [Bind("InstrumentID,Name")] Instrument instrument)
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
