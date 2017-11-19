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
using Trader.Models.TradeAnalysisModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Trader.Controllers
{
    [Authorize]
    public class TradeAnalysisController : BaseController
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ITrades _trades;
        private readonly IInstrumentData _instrumentData;

        public TradeAnalysisController(ITrades trades, UserManager<ApplicationUser> userManager, IInstrumentData instrumentData)
        {
            _trades = trades;
            _userManager = userManager;
            _instrumentData = instrumentData;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
			var user = await GetCurrentUserAsync();

			var userId = user?.Id;

			if (userId != null)
			{
                var trades = await _trades.getAllByUser(userId);
                var finishedTrades = await _trades.getTaxableEvents(trades);
                //var convertedFinishedTrades = finishedTrades
                //.Select(x => new ActiveHoldingsModel
                //{
                //    Instrument = x.Instrument,

                //});
                var activeTrades = await _trades.getActive(trades);
                var insPrices = await _instrumentData.InstrumentPriceCache();
				TradeAnalysisDashboardViewModel model = new TradeAnalysisDashboardViewModel
				{
					activeTrades = activeTrades
                        .GroupBy(x => x.Instrument)
    					.Select(dt => new ActiveHoldingsModel
    					{
                            TradeImportId = 2, //needs thinking
    						Instrument = dt.Key,
    						Quantity = dt.Sum(x => x.Quantity),
    						Value = insPrices.SingleOrDefault(y => y.instrument.InstrumentID == dt.Key.InstrumentID).price,
    						Percentage = 0.5M

    					}).ToList(),
                    bestTrades = finishedTrades.OrderBy(x => x.ProfitLoss).Take(5).ToList(),
                    worstTrades = finishedTrades.OrderBy(x => -x.ProfitLoss).Take(5).ToList()

    			};
    			return View(model);
			}
            return NotFound();
        }
        public async Task<IActionResult> Analysis(int? StartTradeID, int? EndTradeID)
        {
            if(StartTradeID != null)
            {
                var startTradeVar = await _trades.getById((int)StartTradeID);
                TradeAnalysisModel model = new TradeAnalysisModel()
                {
                    startTrade = startTradeVar,
                    endTrade = EndTradeID == null ? null : await _trades.getById((int)EndTradeID),
                    CurrentValue = await _instrumentData.GetCurrentValue(startTradeVar.InstrumentId)
                };
                return View(model);
            }
            return NotFound();
        }
    }
}
