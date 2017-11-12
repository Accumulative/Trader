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
        public IActionResult Dashboard()
        { 
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
