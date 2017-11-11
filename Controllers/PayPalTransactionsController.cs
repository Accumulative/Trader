//using System;
//using System.Linq;
//using System.Net;

//namespace Trader.Controllers
//{
//    [Authorize(Roles ="Administrator")]
//    public class PayPalTransactionsController : BaseController
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();

//        // GET: PayPalTransactions
//        public ActionResult Index()
//        {
//            return View(db.PayPalTransactions.ToList());
//        }

//        // GET: PayPalTransactions/Details/5
//        public ActionResult Details(Guid? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PayPalTransaction payPalTransaction = db.PayPalTransactions.Find(id);
//            if (payPalTransaction == null)
//            {
//                return HttpNotFound();
//            }
//            return View(payPalTransaction);
//        }

//        // GET: PayPalTransactions/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: PayPalTransactions/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "TransactionId,RequestId,TrackingReference,RequestTime,RequestStatus,TimeStamp,RequestError,Token,PayerId,RequestData,PaymentTransactionId,PaymentError")] PayPalTransaction payPalTransaction)
//        {
//            if (ModelState.IsValid)
//            {
//                payPalTransaction.TransactionId = Guid.NewGuid();
//                db.PayPalTransactions.Add(payPalTransaction);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(payPalTransaction);
//        }

//        // GET: PayPalTransactions/Edit/5
//        public ActionResult Edit(Guid? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PayPalTransaction payPalTransaction = db.PayPalTransactions.Find(id);
//            if (payPalTransaction == null)
//            {
//                return HttpNotFound();
//            }
//            return View(payPalTransaction);
//        }

//        // POST: PayPalTransactions/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "TransactionId,RequestId,TrackingReference,RequestTime,RequestStatus,TimeStamp,RequestError,Token,PayerId,RequestData,PaymentTransactionId,PaymentError")] PayPalTransaction payPalTransaction)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(payPalTransaction).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(payPalTransaction);
//        }

//        // GET: PayPalTransactions/Delete/5
//        public ActionResult Delete(Guid? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PayPalTransaction payPalTransaction = db.PayPalTransactions.Find(id);
//            if (payPalTransaction == null)
//            {
//                return HttpNotFound();
//            }
//            return View(payPalTransaction);
//        }

//        // POST: PayPalTransactions/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(Guid id)
//        {
//            PayPalTransaction payPalTransaction = db.PayPalTransactions.Find(id);
//            db.PayPalTransactions.Remove(payPalTransaction);
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
