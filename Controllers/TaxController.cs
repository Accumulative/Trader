using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trader.Data;
using Trader.Models.TaxModels;
using Trader.Models.TradeImportModels;

namespace Trader.Controllers
{
    
    public class TaxController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TaxController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var trades = await _context.TradeImport.ToListAsync();
            return View(await CalculateTaxEvents(trades));
        }
        public async Task<IEnumerable<TaxEventModel>> CalculateTaxEvents(IEnumerable<TradeImport> trades)
        {
            var sorted = trades.OrderBy(c => c.TransactionDate);

            var instruments = await _context.InstrumentCache();
            List<TradeImport> holder = new List<TradeImport>();

            List<TaxEventModel> taxEvents = new List<TaxEventModel>();

            int id = 1;
            foreach (var item in sorted)
            {

                if (item.type == TransactionType.Buy)
                {
                    holder.Add(item);
                }
                else
                {
                    decimal remaining = item.Quantity;

                    foreach (var item2 in holder.Where(x => x.InstrumentId == item.InstrumentId))
                    {
                        if (remaining > 0)
                        {
                            if(item2.Quantity > remaining)
                            {
								taxEvents.Add(new TaxEventModel()
								{
									TaxEventID = id,
									Quantity = remaining,
									StartValue = item2.Value,
                                    EndValue = item.Value,
                                    Instrument = item.Instrument,
									StartDate = item2.TransactionDate,
									EndDate = item.TransactionDate,
									TaxableValue = 0
								});
                                item2.Quantity -= remaining;
                                remaining = 0;
                            }
                            else
                            {
								taxEvents.Add(new TaxEventModel()
								{
									TaxEventID = id,
									Quantity = remaining - item2.Quantity,
									StartValue = item2.Value,
                                    EndValue = item.Value,
                                    Instrument = item.Instrument,
									StartDate = item2.TransactionDate,
									EndDate = item.TransactionDate,
									TaxableValue = 0
								});
                                remaining -= item2.Quantity;
                                holder.Remove(item2);
                            }

                            id++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                }
            }

            return taxEvents;
        }
    }
}
