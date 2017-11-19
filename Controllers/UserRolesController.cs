using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trader.Models.AccountViewModels;
using TraderData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Trader.Controllers
{
    public class UserRolesController : BaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: UserRoles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserRoles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Name")] UserRoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                var exists = await _roleManager.RoleExistsAsync(role.Name);
                if (!exists)
                {
                    var res = await _roleManager.CreateAsync(new IdentityRole { Name = role.Name });
                    return RedirectToAction("Index", "Users");
                }
            }
            return View(role);
        }

        // GET: UserRoles/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserRoles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserRoles/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserRoles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}