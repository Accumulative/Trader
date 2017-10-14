using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Trader.Data;
using ChartJSCore.Models;

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


        public IActionResult Dashboard()
        {
            /*var trades = _context.TradeImport.Select(x => x.Currency).ToList();
            List<int> count = new List<int>();
            var currencies = trades.Distinct();

            foreach (var item in trades)
            {
                count.Add(trades.Count(x => item == x));
            }
            ViewBag.Count = count;
            ViewBag.Currencies = currencies;
            return View();*/
            Chart chart = new Chart();

            ChartJSCore.Models.Options options = new Options()
            {
                MaintainAspectRatio = true,
                Responsive = false

            };
            chart.Options = options;


            chart.Type = "line";

            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();
            data.Labels = new List<string>() { "January", "February", "March", "April", "May", "June", "July" };

            LineDataset dataset = new LineDataset()
            {
                Label = "My First dataset",
                Data = new List<double>() { 65, 59, 80, 81, 56, 55, 40 },
                Fill = false,
                LineTension = 0.1,
                BackgroundColor = "rgba(75, 192, 192, 0.4)",
                BorderColor = "rgba(75,192,192,1)",
                BorderCapStyle = "butt",
                BorderDash = new List<int> { },
                BorderDashOffset = 0.0,
                BorderJoinStyle = "miter",
                PointBorderColor = new List<string>() { "rgba(75,192,192,1)" },
                PointBackgroundColor = new List<string>() { "#fff" },
                PointBorderWidth = new List<int> { 1 },
                PointHoverRadius = new List<int> { 5 },
                PointHoverBackgroundColor = new List<string>() { "rgba(75,192,192,1)" },
                PointHoverBorderColor = new List<string>() { "rgba(220,220,220,1)" },
                PointHoverBorderWidth = new List<int> { 2 },
                PointRadius = new List<int> { 1 },
                PointHitRadius = new List<int> { 10 },
                SpanGaps = false
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;

			ViewData["chart"] = chart;

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
