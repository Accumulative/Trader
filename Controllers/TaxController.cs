using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TraderData;
using TraderData.Models.TaxModels;
using TraderData.Models;
using TraderData.Models.TradeImportModels;

namespace Trader.Controllers
{
    [Authorize]
    public class TaxController : Controller
    {
        private readonly ITrades _trades;
        private readonly IReference _reference;
        private readonly UserManager<ApplicationUser> _userManager;


        public TaxController(ITrades trades, UserManager<ApplicationUser> userManager, IReference reference)
        {
            _reference = reference;
            _trades = trades;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
			var user = await GetCurrentUserAsync();

			var userId = user?.Id;

			if (userId != null)
			{
                var tradesAsync = await _trades.getAllByUser(userId);
				return View(await CalculateTaxEvents(tradesAsync));
				
			}
			return NotFound();

        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public async Task<IEnumerable<TaxEventModel>> CalculateTaxEvents(IEnumerable<TradeImport> trades)
        {
            var sorted = trades.OrderBy(c => c.TransactionDate);

            var instruments = await _reference.getInstruments();
            List<TradeImport> holder = new List<TradeImport>();

            List<TaxEventModel> taxEvents = new List<TaxEventModel>();

            int id = 1;
            foreach (var item in sorted)
            {

                if (item.TransactionType == TransactionType.Buy)
                {
                    holder.Add(item);
                }
                else
                {
                    decimal remaining = item.Quantity;

                    foreach (var item2 in holder.Where(x => x.InstrumentId == item.InstrumentId).ToArray())
                        //Iterate through a copy so we dont get an error about changing array mid loop (ToArray)
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
									TaxableValue = 0,
                                    Fee = item.TransactionFee + item2.TransactionFee * remaining / item2.Quantity

								});
                                item2.Quantity -= remaining;
                                remaining = 0;
                            }
                            else
                            {
								taxEvents.Add(new TaxEventModel()
								{
									TaxEventID = id,
									Quantity = item2.Quantity,
									StartValue = item2.Value,
                                    EndValue = item.Value,
                                    Instrument = item.Instrument,
									StartDate = item2.TransactionDate,
									EndDate = item.TransactionDate,
									TaxableValue = 0,
                                    Fee = item.TransactionFee + item2.TransactionFee * item2.Quantity / remaining
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
