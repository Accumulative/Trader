using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trader.Models.ContactViewModels;
using TraderData;
using TraderData.Models;
using TraderData.Models.ContactModels;

namespace Trader.Controllers
{
    public class EnquiriesController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EnquiriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Enquiries
        public async Task<IActionResult> Index()
        {
            var curUser = await GetCurrentUserAsync();
            return View(await _context.Enquiry.Where(x => x.UserID == curUser.Id).ToListAsync());
        }

		public async Task<IActionResult> AdminIndex()
		{
			return View(await _context.Enquiry.Include(x => x.ApplicationUser).ToListAsync());
		}

        // GET: Enquiries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enquiry = await _context.Enquiry
                .SingleOrDefaultAsync(m => m.EnquiryID == id);

			if (enquiry == null)
			{
				return NotFound();
			}

            var messages = await _context.Message
                                         .Where(x => x.EnquiryID == enquiry.EnquiryID).ToListAsync();

            ContactView model = new ContactView()
            {
                enquiry = enquiry,
                messages = messages
            };

            return View(model);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Details(int? id, ContactView viewModel)
		{
            if (id != null)
            {
                var newMessage = new Message
                {
                    Description = viewModel.newMessage,
                    EnquiryID = (int)id,
                    FromCustomer = true,
                    Ts = DateTime.Now
                };

                _context.Add(newMessage);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", (int)id);
            }
            return NotFound();
		}


		// GET: Enquiries/Create
		public IActionResult Create()
        {
            
            return View();
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // POST: Enquiries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("queryType,description")] InitialContactView contactView)
        {
            if (ModelState.IsValid)
            {
                var curUser = await GetCurrentUserAsync();

                var enquiry = new Enquiry{
                    QueryType = contactView.queryType,
                    Ts = DateTime.Now,
                    UserID = curUser.Id
                };
                var message = new Message
                {
                    Description = contactView.description,
                    Enquiry = enquiry,
                    Ts = DateTime.Now,
                    FromCustomer = true
                };


                _context.Add(enquiry);
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contactView);
        }

		


		// GET: Enquiries/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enquiry = await _context.Enquiry.SingleOrDefaultAsync(m => m.EnquiryID == id);
            if (enquiry == null)
            {
                return NotFound();
            }
            return View(enquiry);
        }

        // POST: Enquiries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnquiryID,CustomerId,QueryType,Ts")] Enquiry enquiry)
        {
            if (id != enquiry.EnquiryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enquiry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnquiryExists(enquiry.EnquiryID))
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
            return View(enquiry);
        }

        // GET: Enquiries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enquiry = await _context.Enquiry
                .SingleOrDefaultAsync(m => m.EnquiryID == id);
            if (enquiry == null)
            {
                return NotFound();
            }

            return View(enquiry);
        }

        // POST: Enquiries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enquiry = await _context.Enquiry.SingleOrDefaultAsync(m => m.EnquiryID == id);
            _context.Enquiry.Remove(enquiry);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool EnquiryExists(int id)
        {
            return _context.Enquiry.Any(e => e.EnquiryID == id);
        }

		// GET: Enquiries/Details/5
		public async Task<IActionResult> AdminDetails(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var enquiry = await _context.Enquiry
				.SingleOrDefaultAsync(m => m.EnquiryID == id);

			if (enquiry == null)
			{
				return NotFound();
			}

			var messages = await _context.Message
										 .Where(x => x.EnquiryID == enquiry.EnquiryID).ToListAsync();

			ContactView model = new ContactView()
			{
				enquiry = enquiry,
				messages = messages
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AdminDetails(int? id, ContactView viewModel)
		{
			if (id != null)
			{
				var newMessage = new Message
                {
                    Description = viewModel.newMessage,
                    EnquiryID = (int)id,
                    FromCustomer = false,
					Ts = DateTime.Now
				};

				_context.Add(newMessage);
				await _context.SaveChangesAsync();

				return RedirectToAction("AdminDetails", (int)id);
			}
			return NotFound();
		}

	}
}
