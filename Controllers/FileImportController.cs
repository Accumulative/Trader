using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TraderData;
using TraderData.Models.FileImportModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using TraderData.Models;

namespace Trader.Controllers
{
    [Authorize]
    public class FileImportController : Controller
    {
        private readonly IFileImport _fileImport;
        private readonly IReference _reference;
        private readonly UserManager<ApplicationUser> _userManager;

        public FileImportController(IFileImport fileImport, UserManager<ApplicationUser> userManager, IReference reference)
        {
            _fileImport = fileImport;
            _userManager = userManager;
            _reference = reference;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: FileImport
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();

            var userId = user?.Id;

            if (userId != null)
            {                
                return View(await _fileImport.getAllByUser(userId));
            }
            return NotFound();
            
        }

        // GET: FileImport/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileImport = await _fileImport.getByIdAsync((int)id);

            if (fileImport == null)
            {
                return NotFound();
            }

            return View(fileImport);
        }

        // GET: FileImport/Create
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["ExchangeId"] = new SelectList(await _reference.getExchanges(), "ExchangeId", "Name");
            return View();
        }

        // POST: FileImport/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FileImportId,Filename,ImportDate,ExchangeId")] FileImport fileImport)
        {
            if (ModelState.IsValid)
            {
                await _fileImport.Add(fileImport);
                
                return RedirectToAction("Index");
            }
            ViewData["ExchangeId"] = new SelectList(await _reference.getExchanges(), "ExchangeId", "Name", fileImport.ExchangeId);
            return View(fileImport);
        }

        // GET: FileImport/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileImport = await _fileImport.getByIdAsync((int)id);
            if (fileImport == null)
            {
                return NotFound();
            }
            ViewData["ExchangeId"] = new SelectList(await _reference.getExchanges(), "ExchangeId", "Name", fileImport.ExchangeId);
            return View(fileImport);
        }

        // POST: FileImport/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FileImportId,Filename,ImportDate,ExchangeId")] FileImport fileImport)
        {
            if (id != fileImport.FileImportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var res = await _fileImport.Edit(fileImport);
                if (res)
                    return RedirectToAction("Index");
                else
                    return NotFound();
            }
            ViewData["ExchangeId"] = new SelectList(await _reference.getExchanges(), "ExchangeId", "Name", fileImport.ExchangeId);
            return View(fileImport);
        }

        // GET: FileImport/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileImport = await _fileImport.getByIdAsync((int)id);
            if (fileImport == null)
            {
                return NotFound();
            }

            return View(fileImport);
        }

        // POST: FileImport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _fileImport.Delete(id);
            return RedirectToAction("Index");
        }

        
    }
}
