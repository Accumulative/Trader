//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;

//namespace Trader.Controllers
//{

//    public class SaleModelsController : BaseController
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();

//        [Authorize(Roles = "Administrator")]
//        public ActionResult Index()
//        {
//            List<ApplicationUser> users = db.Users.ToList();
//            List<SaleModel> saleModels = db.SaleModels.ToList();
//            List<Address> address = db.Addresses.ToList();
//            List<SaleViewModel> saleViewModels = saleModels.Select(x => new SaleViewModel
//            {
//                Amount = x.Amount,
//                User = users.Single(y => y.Id == x.CustomerId),
//                SaleId = x.SaleId,
//                status = x.status,
//                TimeStamp = x.ts
//            }).ToList();

//            ViewBag.SearchTerm = "";
//            ViewBag.SearchDateFrom = DateTime.MinValue;
//            ViewBag.SearchDateTo = DateTime.MaxValue;

//            return View(saleViewModels.ToPagedList(1,15));
//        }
//        [HttpPost]
//        [Authorize(Roles = "Administrator")]
//        public ActionResult Index(string SearchTerm, DateTime? SearchDateFrom, DateTime? SearchDateTo, int? page)
//        {
//            List<SaleModel> saleModels = db.SaleModels.ToList();
//            List<ApplicationUser> users = db.Users.ToList();
//            if (!String.IsNullOrEmpty(SearchTerm))
//            {
//                users = users.Where(x => x.Email.ToLower().Contains(SearchTerm.ToLower()) || x.FullName.ToLower().Contains(SearchTerm.ToLower())).ToList();
//                saleModels = saleModels.Where(x => users.Select(z => z.Id).Contains(x.CustomerId)).ToList();
//            }
//            if (SearchDateFrom != null || SearchDateTo != null)
//            {
//                if (SearchDateFrom == null) SearchDateFrom = DateTime.MinValue;
//                if (SearchDateTo == null) SearchDateTo = DateTime.MaxValue;
//                saleModels = saleModels.Where(x => x.ts < SearchDateTo && x.ts > SearchDateFrom).ToList();
//            }

//            List<SaleViewModel> saleViewModels = saleModels.Select(x => new SaleViewModel
//            {
//                Amount = x.Amount,
//                User = users.Single(y => y.Id == x.CustomerId),
//                SaleId = x.SaleId,
//                TimeStamp = x.ts
//            }).ToList();

//            ViewBag.SearchTerm = SearchTerm;
//            ViewBag.SearchDateFrom = SearchDateFrom;
//            ViewBag.SearchDateTo = SearchDateTo;
//            int pageNumber = (page ?? 1);
//            return View(saleViewModels.ToPagedList(pageNumber, 15));
//        }
//        [Authorize(Roles ="User")]
//        public ActionResult Orders()
//        {
//            string userId = User.Identity.GetUserId();
//            List<SaleModel> saleModels = db.SaleModels.Where(x => x.CustomerId == userId).ToList();
//            List<SaleViewModel> saleViewModels = saleModels.Select(x => new SaleViewModel
//            {
//                Amount = x.Amount,
//                SaleId = x.SaleId,
//                status = x.status,
//                TimeStamp = x.ts
//            }).ToList();

//            ViewBag.SearchTerm = "";
//            ViewBag.SearchDateFrom = DateTime.MinValue;
//            ViewBag.SearchDateTo = DateTime.MaxValue;

//            return View(saleViewModels.ToPagedList(1, 15));
//        }
//        [HttpPost]
//        [Authorize(Roles = "User")]
//        public ActionResult Orders(DateTime? SearchDateFrom, DateTime? SearchDateTo, int? page)
//        {
//            string userId = User.Identity.GetUserId();
//            List<SaleModel> saleModels = db.SaleModels.Where(x => x.CustomerId == userId).ToList();
//            if (SearchDateFrom != null || SearchDateTo != null)
//            {
//                if (SearchDateFrom == null) SearchDateFrom = DateTime.MinValue;
//                if (SearchDateTo == null) SearchDateTo = DateTime.MaxValue;
//                saleModels = saleModels.Where(x => x.ts < SearchDateTo && x.ts > SearchDateFrom).ToList();
//            }

//            List<SaleViewModel> saleViewModels = saleModels.Select(x => new SaleViewModel
//            {
//                Amount = x.Amount,
//                SaleId = x.SaleId,
//                TimeStamp = x.ts
//            }).ToList();

//            ViewBag.SearchDateFrom = SearchDateFrom;
//            ViewBag.SearchDateTo = SearchDateTo;
//            int pageNumber = (page ?? 1);
//            return View(saleViewModels.ToPagedList(pageNumber, 15));
//        }

//        // GET: SaleModels/Details/5
//        [Authorize(Roles = "Administrator")]
//        public ActionResult Details(Guid id)
//        {
//            if (id == Guid.Empty)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
            
//            SaleModel saleModel = db.SaleModels.Single(x => x.SaleId == id);
//            ApplicationUser user = db.Users.Find(saleModel.CustomerId);
//            Address address = db.Addresses.Find(saleModel.AddressId);
//            SaleViewModel saleViewModel = new SaleViewModel
//            {
//                Amount = saleModel.Amount,
//                SaleId = id,
//                TimeStamp = saleModel.ts,
//                User = user
//            };
//            List<ProductPrice> pp = db.ProductPrices.ToList();
//            List<ProductModel> pm = db.ProductModels.ToList();
//            List<SaleProductModel> sm = db.SaleProductModels.ToList();
//            List<ApplicationCartItem> saleProducts = sm.Where(x => x.SaleId == id).Select(x => new ApplicationCartItem
//            {
//                ProductId = x.ProductId,
//                Price = pp.FirstOrDefault(y => y.ID == x.PriceId).price,
//                Name = pm.FirstOrDefault(y => y.ID == x.ProductId).name,
//                Quantity = x.Quantity

//            }).ToList();
//            SaleDetailsModel model = new SaleDetailsModel
//            {
//                saleView = saleViewModel,
//                address = address,
//                products = saleProducts
//            };
//            if (saleModel == null)
//            {
//                return HttpNotFound();
//            }
//            return View(model);
//        }
//        [Authorize(Roles = "User")]
//        public ActionResult OrderDetails(Guid id)
//        {
//            if (id == Guid.Empty)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }

//            SaleModel saleModel = db.SaleModels.Single(x => x.SaleId == id);
//            Address address = db.Addresses.Find(saleModel.AddressId);
//            SaleViewModel saleViewModel = new SaleViewModel
//            {
//                Amount = saleModel.Amount,
//                SaleId = saleModel.SaleId,
//                TimeStamp = saleModel.ts
//            };
//            List<ProductPrice> pp = db.ProductPrices.ToList();
//            List<ProductModel> pm = db.ProductModels.ToList();
//            List<SaleProductModel> sm = db.SaleProductModels.ToList();
//            List<ApplicationCartItem> saleProducts = sm.Where(x => x.SaleId == id).Select(x => new ApplicationCartItem
//            {
//                ProductId = x.ProductId,
//                Price = pp.FirstOrDefault(y => y.ID == x.PriceId).price,
//                Name = pm.FirstOrDefault(y => y.ID == x.ProductId).name,
//                Quantity = x.Quantity

//            }).ToList();
//            SaleDetailsModel model = new SaleDetailsModel
//            {
//                saleView = saleViewModel,
//                address = address,
//                products = saleProducts
//            };
//            if (saleModel == null)
//            {
//                return HttpNotFound();
//            }
//            return View(model);
//        }

//        // GET: SaleModels/Create
//        [Authorize(Roles = "Administrator")]
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: SaleModels/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "SaleId,CustomerId,Amount,ts")] SaleModel saleModel)
//        {
//            if (ModelState.IsValid)
//            {
//                db.SaleModels.Add(saleModel);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(saleModel);
//        }

//        // GET: SaleModels/Edit/5
//        [Authorize(Roles ="Administrator")]
//        public ActionResult Edit(Guid id)
//        {
//            if (id == Guid.Empty)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            SaleModel saleModel = db.SaleModels.Single(x => x.SaleId == id);
//            if (saleModel == null)
//            {
//                return HttpNotFound();
//            }
//            return View(saleModel);
//        }

//        // POST: SaleModels/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "Administrator")]
//        public ActionResult Edit([Bind(Include = "SaleId,CustomerId,Amount,ts")] SaleModel saleModel)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(saleModel).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(saleModel);
//        }

//        // GET: SaleModels/Delete/5
//        [Authorize(Roles = "Administrator")]
//        public ActionResult Delete(Guid id)
//        {
//            if (id == Guid.Empty)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            SaleModel saleModel = db.SaleModels.Find(id);
//            if (saleModel == null)
//            {
//                return HttpNotFound();
//            }
//            return View(saleModel);
//        }

//        // POST: SaleModels/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "Administrator")]
//        public ActionResult DeleteConfirmed(Guid id)
//        {
//            SaleModel saleModel = db.SaleModels.Single(x => x.SaleId == id);
//            db.SaleModels.Remove(saleModel);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
