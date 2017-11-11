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
using ChartJSCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Trader.Controllers
{
    public class DashboardViewComponent : ViewComponent
    {
		private readonly ITrades _trades;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IInstrumentData _instrumentData;
		private readonly IReference _reference;

		public DashboardViewComponent(ITrades trades, IInstrumentData instrumentData, UserManager<ApplicationUser> userManager, IReference reference)
		{
			_trades = trades;
			_userManager = userManager;
			_reference = reference;
			_instrumentData = instrumentData;
		}
		private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

		public async Task<IViewComponentResult> InvokeAsync(DashboardViewModel viewModel)
		{
			var user = await GetCurrentUserAsync();

			var userId = user?.Id;

			if (userId != null)
			{
				var trades = await _trades.getAllByUser(userId);

				// set new filter before, others you get filtered-filters
				DashboardFilterModel filters = new DashboardFilterModel
				{
					InstrumentList = new SelectList(trades.Select(x => x.Instrument).Distinct().Select(x => new { Id = x.InstrumentID, Value = x.Name }), "Id", "Value", viewModel == null ? null : viewModel.filter.instrumentId), //sometimes null)
					Month = new SelectList(trades.Select(x => x.TransactionDate.Month + "/" + x.TransactionDate.Year).Distinct().Select(x => new { Id = x, Value = x }), "Id", "Value", viewModel == null ? null : viewModel.filter.monthNo),
					ExchangeList = new SelectList(trades.Select(x => x.FileImport.Exchange).Distinct().Select(x => new { Id = x.Name, Value = x.Name }), "Id", "Value", viewModel == null ? null : viewModel.filter.exchangeId)

				};

				// Apply filters
				if (viewModel != null)
				{
					if (viewModel.filter.exchangeId != null)
						trades = trades.Where(x => x.FileImport.ExchangeId == viewModel.filter.exchangeId).ToList();
					if (viewModel.filter.instrumentId != null)
						trades = trades.Where(x => x.InstrumentId == viewModel.filter.instrumentId).ToList();
					if (viewModel.filter.monthNo != null)
						trades = trades.Where(x => x.TransactionDate.Month + "/" + x.TransactionDate.Year == viewModel.filter.monthNo).ToList();
				}

				var curList = trades.Select(x => x.Currency.ToString()).ToList();
				List<double> count = new List<double>();
				var currencies = curList.Distinct();

				foreach (var item in currencies)
				{
					count.Add((double)curList.Count(x => item == x));
				}

				CryptoLineChart lineChart = new CryptoLineChart(currencies.ToList(), new List<List<double>>() { count });

				//ViewData["chart"] = lineChart.getChart;


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
					.Select(g => new {
						Month = g.Key.Month,
						Sum = g.Sum(x => x.Quantity * x.Value * (x.TransactionType == TraderData.Models.TradeImportModels.TransactionType.Buy ? -1 : 1)),
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
				var insPrices = await _instrumentData.InstrumentPriceCache();
				var bTrades = aTrades
					.GroupBy(x => x.Instrument)
					.Select(dt => new ActiveHoldingsModel
					{
						Instrument = dt.Key,
						Quantity = dt.Sum(x => x.Quantity),
						Value = insPrices.SingleOrDefault(y => y.instrument.InstrumentID == dt.Key.InstrumentID).price,
						Percentage = 0.5M

					}).ToList();




				DashboardViewModel model = new DashboardViewModel()
				{
					TotalSellAmount = trades.Where(x => x.TransactionType == TraderData.Models.TradeImportModels.TransactionType.Sell).Sum(x => x.Quantity * x.Value),
					TotalBuyAmount = trades.Where(x => x.TransactionType == TraderData.Models.TradeImportModels.TransactionType.Buy).Sum(x => x.Quantity * x.Value),
					TotalFeeAmount = trades.Sum(x => x.TransactionFee),
					TotalHoldings = bTrades.Sum(x => x.Quantity * x.Value)
				};
				model.ActiveTrades = bTrades;
				model.filter = filters;
                model.chartOne = lineChart.getChart;
                //ViewData["Test"] = "Hello world";

				return View(model);
			}
            return View(new DashboardViewModel());
		}
    }
}
