using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraderData;
using TraderData.Models.FileImportModels;

namespace Trader.Controllers
{
    public class ExchangeController : Controller
    {
        private readonly IReference _reference;

        public ExchangeController(IReference reference)
        {
            _reference = reference;    
        }

        // GET: Exchange
        public async Task<IActionResult> Index()
        {
            return View(await _reference.getExchanges());
        }

        // GET: Exchange/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exchange = await _reference.GetExchangeById((int)id);
            if (exchange == null)
            {
                return NotFound();
            }

            return View(exchange);
        }

        // GET: Exchange/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exchange/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExchangeId,Name,URL")] Exchange exchange)
        {
            exchange.DateCreated = DateTime.Now;
            if (ModelState.IsValid)
            {
                await _reference.AddExchange(exchange);
                return RedirectToAction("Index");
            }
            return View(exchange);
        }

        // GET: Exchange/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exchange = await _reference.GetExchangeById((int)id);
            if (exchange == null)
            {
                return NotFound();
            }
            return View(exchange);
        }

        // POST: Exchange/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExchangeId,Name,URL")] Exchange exchange)
        {
            if (id != exchange.ExchangeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var res = await _reference.EditExchange(exchange);
                if (res)
                    return RedirectToAction("Index");
                else
                    return NotFound();
            }
            return View(exchange);
        }

        // GET: Exchange/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exchange = await _reference.GetExchangeById((int)id);
            if (exchange == null)
            {
                return NotFound();
            }

            return View(exchange);
        }

        // POST: Exchange/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _reference.DeleteExchange(id);
            return RedirectToAction("Index");
        }

       
    }
}
