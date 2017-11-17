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

                // set new filter before, others you get filtered-filters
                DashboardFilterModel filters = new DashboardFilterModel
                {
                    InstrumentList = new SelectList(trades.Select(x => x.Instrument).Distinct().Select(x => new { Id = x.InstrumentID, Value = x.Name }), "Id", "Value"), //sometimes null)
                    Month = new SelectList(trades.Select(x => x.TransactionDate.Month + "/" + x.TransactionDate.Year).Distinct().Select(x => new { Id = x, Value = x }), "Id", "Value"),
                    ExchangeList = new SelectList(trades.Select(x => x.FileImport.Exchange).Distinct().Select(x => new { Id = x.Name, Value = x.Name }), "Id", "Value")

                };
                return View(filters);
            }
            return NotFound();
        }

        public IActionResult DashboardGraphViewComponent(DashboardFilterModel filterModel)
        {
            return ViewComponent("DashboardGraphsViewComponent", filterModel);
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
