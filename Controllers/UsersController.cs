using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TraderData.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Trader.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Trader.Controllers
{
    [Authorize(Roles = "Admin")]//RolesData.SeedRoles(app.ApplicationServices).Wait();
    public class UsersController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> Edit(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.ToListAsync();
            var model = new EditUserViewModel
            {
                RoleList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(roles.Select(x => x.Name)),
                UserID = Id
            };
            return View(model);
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string UserID, string selRole)
        {
            var user = await _userManager.FindByIdAsync(UserID);
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var res = await _userManager.AddToRoleAsync(user, selRole);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction("Index", "Users");
            }
            return View(user);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var model = new UserDetailsViewModel
            {
                user = user,
                roles = roles.ToList()
            };
            if (user == null)
            {
                return NotFound();
            }

            return View(model);
        }
    }
}
