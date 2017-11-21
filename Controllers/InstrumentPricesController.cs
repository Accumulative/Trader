using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TraderData;
using TraderData.Models.InstrumentModels;

namespace Trader.Controllers
{
    public class InstrumentPricesController : BaseController
    {
        private readonly IInstrumentData _instrumentData;
        private readonly IReference _reference;

        public InstrumentPricesController(IInstrumentData instrumentData, IReference reference)
        {
            _instrumentData = instrumentData;
            _reference = reference;
        }

		// GET: InstrumentPrices
		public async Task<IActionResult> Index(
        	string sortOrder,
        	string currentFilter,
        	string searchString,
        	int? page)
		{
			ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "Price_desc" : "Price";

			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewData["CurrentFilter"] = searchString;
            var insPriceAll = await _instrumentData.GetStoredPricesAsync();
            var insPrices = insPriceAll.Select(x => x);

			if (!String.IsNullOrEmpty(searchString))
			{
                if(IsNumber(searchString))
                {
					insPrices = insPrices.Where(s => s.Price > int.Parse(searchString));
                }
                else
                {
					insPrices = insPrices.Where(s => s.Instrument.Name.Contains(searchString));   
                }
			}
			switch (sortOrder)
			{
				case "Price":
					insPrices = insPrices.OrderBy(s => s.Price);
					break;
				case "Price_desc":
					insPrices = insPrices.OrderByDescending(s => s.Price);
					break;
				case "Date":
					insPrices = insPrices.OrderBy(s => s.DateTime);
					break;
				case "Date_desc":
					insPrices = insPrices.OrderByDescending(s => s.DateTime);
					break;
				default:
					insPrices = insPrices.OrderBy(s => s.DateTime);
					break;
			}
			int pageSize = 50;
			return View( PaginatedList<InstrumentPrice>.Create(insPrices.AsQueryable(), page ?? 1, pageSize));
        }



        internal bool IsNumber(string s)
		{
			int i;
			return int.TryParse(s, out i);
		}
        // GET: InstrumentPrices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrumentPrice = await _instrumentData.GetPriceByID((int)id);
            if (instrumentPrice == null)
            {
                return NotFound();
            }

            return View(instrumentPrice);
        }

        // GET: InstrumentPrices/Create
        public async Task<IActionResult> Create()
        {
            
            ViewData["ExchangeID"] = new SelectList(await _reference.getExchanges(), "ExchangeId", "Name");
            ViewData["InstrumentID"] = new SelectList(await _reference.getInstruments(), "InstrumentID", "InstrumentID");
            return View();
        }

        // POST: InstrumentPrices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstrumentPriceID,ExchangeID,InstrumentID,DateTime,Price")] InstrumentPrice instrumentPrice)
        {
            if (ModelState.IsValid)
            {
                await _instrumentData.AddInstrumentPrice(instrumentPrice);
                return RedirectToAction("Index");
            }
            ViewData["ExchangeID"] = new SelectList(await _reference.getExchanges(), "ExchangeId", "Name", instrumentPrice.ExchangeID);
            ViewData["InstrumentID"] = new SelectList(await _reference.getInstruments(), "InstrumentID", "InstrumentID", instrumentPrice.InstrumentID);
            return View(instrumentPrice);
        }

        // GET: InstrumentPrices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrumentPrice = await _instrumentData.GetPriceByID((int)id);
            if (instrumentPrice == null)
            {
                return NotFound();
            }
            ViewData["ExchangeID"] = new SelectList(await _reference.getExchanges(), "ExchangeId", "Name", instrumentPrice.ExchangeID);
            ViewData["InstrumentID"] = new SelectList(await _reference.getInstruments(), "InstrumentID", "InstrumentID", instrumentPrice.InstrumentID);
            return View(instrumentPrice);
        }

        // POST: InstrumentPrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstrumentPriceID,ExchangeID,InstrumentID,DateTime,Price")] InstrumentPrice instrumentPrice)
        {
            if (id != instrumentPrice.InstrumentPriceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var res = await _instrumentData.EditInstrumentPrice(instrumentPrice);
                if (res)
                {
                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            ViewData["ExchangeID"] = new SelectList(await _reference.getExchanges(), "ExchangeId", "Name", instrumentPrice.ExchangeID);
            ViewData["InstrumentID"] = new SelectList(await _reference.getInstruments(), "InstrumentID", "InstrumentID", instrumentPrice.InstrumentID);
            return View(instrumentPrice);
        }

        // GET: InstrumentPrices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrumentPrice = await _instrumentData.GetPriceByID((int)id);
            if (instrumentPrice == null)
            {
                return NotFound();
            }

            return View(instrumentPrice);
        }

        // POST: InstrumentPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _instrumentData.DeleteInstrumentPrice(id);
            return RedirectToAction("Index");
        }


    }
}
