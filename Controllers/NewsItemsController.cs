using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TraderData;
using TraderData.Models.NewsModels;

namespace Trader.Controllers
{
    public class NewsItemsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public NewsItemsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: NewsItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.NewsItem.ToListAsync());
        }

        // GET: NewsItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsItem = await _context.NewsItem
                .SingleOrDefaultAsync(m => m.NewsItemID == id);
            if (newsItem == null)
            {
                return NotFound();
            }

            return View(newsItem);
        }

        // GET: NewsItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NewsItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsItemID,Title,Description,Ts")] NewsItem newsItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newsItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(newsItem);
        }

        // GET: NewsItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsItem = await _context.NewsItem.SingleOrDefaultAsync(m => m.NewsItemID == id);
            if (newsItem == null)
            {
                return NotFound();
            }
            return View(newsItem);
        }

        // POST: NewsItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewsItemID,Title,Description,Ts")] NewsItem newsItem)
        {
            if (id != newsItem.NewsItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsItemExists(newsItem.NewsItemID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(newsItem);
        }

        // GET: NewsItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsItem = await _context.NewsItem
                .SingleOrDefaultAsync(m => m.NewsItemID == id);
            if (newsItem == null)
            {
                return NotFound();
            }

            return View(newsItem);
        }

        // POST: NewsItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newsItem = await _context.NewsItem.SingleOrDefaultAsync(m => m.NewsItemID == id);
            _context.NewsItem.Remove(newsItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool NewsItemExists(int id)
        {
            return _context.NewsItem.Any(e => e.NewsItemID == id);
        }
    }
}
