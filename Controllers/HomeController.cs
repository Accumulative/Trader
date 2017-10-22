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

				return View();
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
