using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trader.Data;
using Trader.Models;
using Trader.Models.FileImportModels;
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
            var tradesAsync = await _context.TradeImport.Include(s => s.Instrument).ToListAsync();

            /*var trades = tradesAsync.Select(x => new TradeImportIndexModel()
            {
                TradeImportID = x.TradeImportID,
                ExternalReference = x.ExternalReference,
                Instrument = x.Instrument.Name,
                Value = x.Value,
                Quantity = x.Quantity,
                TransactionDate = x.TransactionDate,
                ImportDate = x.ImportDate
            });*/
            return View(tradesAsync);
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
        public async Task<IActionResult> Create()
        {
            IEnumerable<Instrument> instrument = await _context.InstrumentCache();
            var instruments = instrument.OrderBy(c => c.Name).Select(x => new { Id = x.InstrumentID, Value = x.Name });
			var model = new TradeImportViewModel();
			model.InstrumentList = new SelectList(instruments, "Id", "Value");

			return View(model);
        }

        // POST: TradeImport/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string ExternalReference, int InstrumentId, decimal Value, decimal Quantity,DateTime TransactionDate)
        {
            if (ModelState.IsValid)
            {
                TradeImport trade = new TradeImport()
                {
                    ExternalReference = ExternalReference,
                    InstrumentId = InstrumentId,
                    Value = Value,
                    Quantity = Quantity,
                    TransactionDate = TransactionDate,
                    ImportDate = DateTime.Now
                };
                _context.Add(trade);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
			IEnumerable<Instrument> instrument = await _context.InstrumentCache();
            var instruments = instrument.OrderBy(c => c.Name).Select(x => new { Id = x.InstrumentID, Value = x.Name });
			var model = new TradeImportViewModel(); //tradeImport;
            model.ExternalReference = ExternalReference;
            model.Quantity = Quantity;
            model.Value = Value;
            model.TransactionDate = TransactionDate;
			model.InstrumentList = new SelectList(instruments, "Id", "Value", InstrumentId);
            return View(model);
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

        public async Task<IActionResult> Import()
        {
			IEnumerable<Instrument> instrument = await _context.InstrumentCache();
            IEnumerable<Exchange> exchange = await _context.ExchangeCache();
			var instruments = instrument.OrderBy(c => c.Name).Select(x => new { Id = x.InstrumentID, Value = x.Name });
            var exchanges = exchange.OrderBy(c => c.Name).Select(x => new { Id = x.ExchangeId, Value = x.Name });
            var model = new ImportModel();
            model.ExchangeList = new SelectList(exchanges, "Id", "Value");
			model.InstrumentList = new SelectList(instruments, "Id", "Value");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile file, int selInstrument, int selExchange)
        {
            int count = 0;
            FileImport fileImport;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                //var fileContent = reader.ReadToEnd();
                var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                fileImport = new FileImport
                {
                    Filename = parsedContentDisposition.FileName,
                    ExchangeId = selExchange,
                    ImportDate = DateTime.Now,

                };
                _context.FileImport.Add(fileImport);
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
                    count += 1;
                    if (count >= 5)
                    {
                        var data = line.Split(new[] { ',' });
                        var trade = new TradeImport()
                        {
                            ExternalReference = data[9],
                            Value = decimal.Parse(data[7]),
                            Quantity = decimal.Parse(data[2]),
                            InstrumentId = selInstrument, //to get from a dropdown
                            FileImport = fileImport,
                            ImportDate = DateTime.Now,
                            TransactionType = data[1] == "Buy" ? TransactionType.Buy : TransactionType.Sell,
                            TransactionDate = DateTime.Parse(data[0]),
                            Currency = (Currency)Enum.Parse(typeof(Currency),data[6]),
                            TransactionFee = decimal.Parse(data[4])

                        };
                        _context.TradeImport.Add(trade);
                    }
				}

				_context.SaveChanges();
            }
            return RedirectToAction("Result");
        }
        public async Task<IActionResult> Result()
        {
			ViewData["Message"] = "Submission successful";
            return View();
        }
    }
}
