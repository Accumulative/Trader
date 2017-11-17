using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TraderData;
using Trader.Models.FileImportModels;
using Trader.Models.TradeImportModels;
using TraderData.Models;
using TraderData.Models.TradeImportModels;
using TraderData.Models.FileImportModels;


namespace Trader.Controllers
{
    [Authorize]
    public class TradeImportController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITrades _trades;
        private readonly IReference _reference;

		public TradeImportController(ITrades trades, IReference reference, UserManager<ApplicationUser> userManager)
		{
			_trades = trades;
            _userManager = userManager;
            _reference = reference;
        }
       

        // GET: TradeImport
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();

			var userId = user?.Id;

            if (userId != null)
            {
                var tradesAsync = await _trades.getAllByUser(userId);
                tradesAsync = tradesAsync.OrderBy(x => x.TransactionDate).ToList();
                return View(tradesAsync);
            }
            return NotFound();
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: TradeImport/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeImport = await _trades.getById((int)id);
            if (tradeImport == null)
            {
                return NotFound();
            }

            return View(tradeImport);
        }

        // GET: TradeImport/Create
        public async Task<IActionResult> Create()
        {
            IEnumerable<Instrument> instrument = await _reference.getInstruments();
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
                    TransactionDate = TransactionDate
                };

                return RedirectToAction("Index");
            }
            IEnumerable<Instrument> instrument = await _reference.getInstruments();
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

            var tradeImport = await _trades.getById((int)id);
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
        public async Task<IActionResult> Edit(int id, [Bind("TradeImportID,ExternalReference,InstrumentId,Value,Quantity,TransactionDate,UserID,FileImportId")] TradeImport tradeImport)
        {
            if (id != tradeImport.TradeImportID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
				try
				{
					var res = await _trades.Edit(tradeImport);
				}
				catch (DbUpdateConcurrencyException)
				{
                    var exists = await _trades.TradeImportExists(tradeImport.TradeImportID);

                    if (!exists)
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

            var tradeImport = await _trades.getById((int)id);
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
            _trades.Delete(id);
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Import()
        {
            IEnumerable<Instrument> instrument = await _reference.getInstruments();
            IEnumerable<Exchange> exchange = await _reference.getExchanges();
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
            var user = await GetCurrentUserAsync();

            var userId = user?.Id;

            if (userId != null)
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
                        UserID = userId

                    };
                    List<TradeImport> trades = new List<TradeImport>();
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
                                Currency = (Currency)Enum.Parse(typeof(Currency), data[6]),
                                TransactionFee = decimal.Parse(data[4]),
                                UserID = userId

                            };
                            trades.Add(trade);
                        }
                    }

                    _trades.Import(trades, fileImport);
                }
                return RedirectToAction("Result");
            }
            return NotFound();
        }
        public async Task<IActionResult> Result()
        {
			ViewData["Message"] = "Submission successful";
            return View();
        }
    }
}
