using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TraderData;
using Microsoft.AspNetCore.Authorization;
using Trader.Models.Charts;
using Microsoft.AspNetCore.Identity;
using Trader.Models;
using TraderData.Models;
using TraderData.Models.TradeImportModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Trader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITrades _trades;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IInstrumentData _instrumentData;
        private readonly IReference _reference;

        public HomeController(ITrades trades, IInstrumentData instrumentData, UserManager<ApplicationUser> userManager, IReference reference)
        {
            _trades = trades;
            _userManager = userManager;
            _reference = reference;
            _instrumentData = instrumentData;
        }
        public IActionResult Index()
        {
            
            return View();
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var user = await GetCurrentUserAsync();

            var userId = user?.Id;

            if (userId != null)
            {
                var trades = await _trades.getAllByUser(userId);

                var curList = trades.Select(x => x.Currency.ToString()).ToList();
                List<double> count = new List<double>();
                var currencies = curList.Distinct();

                foreach (var item in currencies)
                {
                    count.Add((double)curList.Count(x => item == x));
                }

                CryptoLineChart lineChart = new CryptoLineChart(currencies.ToList(), new List<List<double>>() { count });

                ViewData["chart"] = lineChart.getChart;


                var insList = trades.Select(x => x.Instrument.Name.ToString()).ToList();
                count = new List<double>();
                var instruments = insList.Distinct();

                foreach (var item in instruments)
                {
                    count.Add((double)insList.Count(x => item == x));
                }


                CryptoBarChart barChart = new CryptoBarChart(instruments.ToList(), count);

                ViewData["chart2"] = barChart.getChart;

                CryptoPieChart pieChart = new CryptoPieChart(instruments.ToList(), count);
                ViewData["chart3"] = pieChart.getChart;


                var exchangeList = trades.Select(x => x.FileImport.Exchange.Name.ToString()).ToList();
                count = new List<double>();
                var exchanges = exchangeList.Distinct();

                foreach (var item in exchanges)
                {
                    count.Add((double)exchangeList.Count(x => item == x));
                }
                CryptoPieChart pieChart2 = new CryptoPieChart(exchanges.ToList(), count);
                ViewData["chart5"] = pieChart2.getChart;

                var months = trades
                    .GroupBy(dt => new { Month = dt.TransactionDate.Month, dt.TransactionDate.Year })
                    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                    .Select(g => new { Month = g.Key.Month, Sum = g.Sum(x => x.Quantity * x.Value * (x.TransactionType == TraderData.Models.TradeImportModels.TransactionType.Buy ? -1 : 1)),
                    Buys = g.Where(x => x.TransactionType == TraderData.Models.TradeImportModels.TransactionType.Buy).Sum(x => x.Quantity * x.Value),
                    Sells = g.Where(x => x.TransactionType == TraderData.Models.TradeImportModels.TransactionType.Sell).Sum(x => x.Quantity * x.Value)
                    });

                CryptoLineChart lineChart2 = new CryptoLineChart(months.Select(x => x.Month.ToString()).ToList(), new List<List<double>>() {
                    months.Select(x => (double)x.Sum).ToList(),
                    months.Select(x => (double)x.Buys).ToList(),
                    months.Select(x => (double)x.Sells).ToList()
                });
                ViewData["chart4"] = lineChart2.getChart;

                var months2 = trades
                    .GroupBy(dt => new { Month = dt.TransactionDate.Month, dt.TransactionDate.Year })
                    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                    .Select(g => new {
                        Month = g.Key.Month,
                        RawProfit = g.Sum(x => x.Quantity * x.Value),
                        Fees = g.Sum(x => x.TransactionFee),
                        
                    });

                CryptoLineChart lineChart4 = new CryptoLineChart(months.Select(x => x.Month.ToString()).ToList(), new List<List<double>>() {
                    months.Select(x => (double)x.Sum).ToList(),
                    months.Select(x => (double)x.Buys).ToList(),
                    months.Select(x => (double)x.Sells).ToList()
                });
                ViewData["chart6"] = lineChart4.getChart;

                var aTrades = await _trades.getActive(trades);
                var bTrades = aTrades
                    .GroupBy(x => x.Instrument)
                    .Select(dt => new ActiveHoldingsModel
                    {
                        Instrument = dt.Key,
                        Quantity = dt.Sum(x => x.Quantity),
                        Value = _instrumentData.GetCurrentValue(dt.Key),
                        Percentage = 0.5M

                    }).ToList();


                DashboardFilterModel filters = new DashboardFilterModel
                {
                    InstrumentList = new SelectList(trades.Select(x => x.Instrument).Distinct().Select(x => new { Id = x.InstrumentID, Value = x.Name }), "Id", "Value")
                };

                DashboardViewModel model = new DashboardViewModel()
                {
                    TotalSellAmount = trades.Where(x => x.TransactionType == TraderData.Models.TradeImportModels.TransactionType.Sell).Sum(x => x.Quantity * x.Value),
                    TotalBuyAmount = trades.Where(x => x.TransactionType == TraderData.Models.TradeImportModels.TransactionType.Buy).Sum(x => x.Quantity * x.Value),
                    TotalFeeAmount = trades.Sum(x => x.TransactionFee),
                    TotalHoldings = bTrades.Sum(x => x.Quantity * x.Value)
                };
                model.ActiveTrades = bTrades;
                model.filter = filters;


                return View(model);
            }
            return NotFound();

        }

        

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
