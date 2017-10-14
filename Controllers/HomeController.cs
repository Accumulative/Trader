using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Trader.Data;
using ChartJSCore.Models;
using Microsoft.AspNetCore.Authorization;
using Trader.Models.Charts;

namespace Trader.Controllers
{
    public class HomeController : Controller
    {
		private readonly ApplicationDbContext _context;


		public HomeController(ApplicationDbContext context)
		{
			_context = context;
		}
        public IActionResult Index()
        {
            
            return View();
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            var trades = _context.TradeImport;
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

			return View();
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
