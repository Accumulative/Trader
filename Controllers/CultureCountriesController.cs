//using System.Linq;
//using System.Net;

//namespace Trader.Controllers
//{
//    public class CultureCountriesController : Controller
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();

//        // GET: CultureCountries
//        public ActionResult Index()
//        {
//            return View(db.CultureCountries.ToList());
//        }

//        // GET: CultureCountries/Details/5
//        public ActionResult Details(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CultureCountry cultureCountry = db.CultureCountries.Find(id);
//            if (cultureCountry == null)
//            {
//                return HttpNotFound();
//            }
//            return View(cultureCountry);
//        }

//        // GET: CultureCountries/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: CultureCountries/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "CultureCountryId,country,culture")] CultureCountry cultureCountry)
//        {
//            if (ModelState.IsValid)
//            {
//                if (db.CultureCountries.Where(x => x.country == cultureCountry.country && x.culture == cultureCountry.culture).Count() == 0)
//                {
//                    db.CultureCountries.Add(cultureCountry);
//                    db.SaveChanges();
//                    return RedirectToAction("Index");
//                }
//                ViewBag.Message = "There already exists a setup like this!";
//            }

//            return View(cultureCountry);
//        }

//        // GET: CultureCountries/Edit/5
//        public ActionResult Edit(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CultureCountry cultureCountry = db.CultureCountries.Find(id);
//            if (cultureCountry == null)
//            {
//                return HttpNotFound();
//            }
//            return View(cultureCountry);
//        }

//        // POST: CultureCountries/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "CultureCountryId,country,culture")] CultureCountry cultureCountry)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(cultureCountry).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(cultureCountry);
//        }

//        // GET: CultureCountries/Delete/5
//        public ActionResult Delete(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CultureCountry cultureCountry = db.CultureCountries.Find(id);
//            if (cultureCountry == null)
//            {
//                return HttpNotFound();
//            }
//            return View(cultureCountry);
//        }

//        // POST: CultureCountries/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(string id)
//        {
//            CultureCountry cultureCountry = db.CultureCountries.Find(id);
//            db.CultureCountries.Remove(cultureCountry);
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
