//using System.Linq;
//using System.Net;

//namespace Trader.Controllers
//{
//    [Authorize(Roles ="Administrator")]
//    public class AddressController : BaseController
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();

//        // GET: Address
//        public ActionResult Index()
//        {
//            return View(db.Addresses.ToList());
//        }

//        // GET: Address/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Address address = db.Addresses.Find(id);
//            if (address == null)
//            {
//                return HttpNotFound();
//            }
//            return View(address);
//        }

//        // GET: Address/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: Address/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(Address address)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Addresses.Add(address);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(address);
//        }

//        // GET: Address/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Address address = db.Addresses.Find(id);
//            if (address == null)
//            {
//                return HttpNotFound();
//            }
//            return View(address);
//        }

//        // POST: Address/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit(Address address)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(address).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(address);
//        }

//        // GET: Address/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Address address = db.Addresses.Find(id);
//            if (address == null)
//            {
//                return HttpNotFound();
//            }
//            return View(address);
//        }

//        // POST: Address/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Address address = db.Addresses.Find(id);
//            db.Addresses.Remove(address);
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
