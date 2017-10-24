using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TraderData;
using ChartJSCore.Models;
using Microsoft.AspNetCore.Authorization;
using Trader.Models.Charts;
using Microsoft.AspNetCore.Identity;
using Trader.Models;
using TraderData.Models;
using System.Globalization;

namespace Trader.Controllers
{
    public class HomeController : Controller
    {
		private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

		public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
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
				var trades = _context.TradeImport.Where(x => x.UserID == userId);

				var curList = _context.TradeImport.Select(x => x.Currency.ToString()).ToList();
				List<double> count = new List<double>();
				var currencies = curList.Distinct();

				foreach (var item in currencies)
				{
					count.Add((double)curList.Count(x => item == x));
				}

				CryptoLineChart lineChart = new CryptoLineChart(currencies.ToList(), count);

				ViewData["chart"] = lineChart.getChart;


				var insList = _context.TradeImport.Select(x => x.Instrument.Name.ToString()).ToList();
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


				var exchangeList = _context.TradeImport.Select(x => x.FileImport.Exchange.Name.ToString()).ToList();
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

                    .Select(g => new { Month = g.Key.Month, Sum = g.Sum(x => x.Quantity * x.Value) });
                CryptoLineChart lineChart2 = new CryptoLineChart(months.Select(x => x.Month.ToString()).ToList(), months.Select(x => (double)x.Sum).ToList());
                ViewData["chart4"] = lineChart2.getChart;
                DashboardViewModel model = new DashboardViewModel()
                {
                    TotalSellAmount = trades.Where(x => x.TransactionType == TraderData.Models.TradeImportModels.TransactionType.Sell).Sum(x => x.Quantity * x.Value),
                    TotalBuyAmount = trades.Where(x => x.TransactionType == TraderData.Models.TradeImportModels.TransactionType.Buy).Sum(x => x.Quantity * x.Value),
                    TotalFeeAmount = trades.Sum(x => x.TransactionFee)
                };

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
