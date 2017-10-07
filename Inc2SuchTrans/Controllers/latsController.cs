using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Inc2SuchTrans.Models;

namespace mapslatlong1.Controllers
{
    public class latsController : Controller
    {
        private STLogisticsEntities db = new STLogisticsEntities();

        // GET: lats
        public ActionResult Index()
        {
            return View(db.TrackDelivery.ToList());
        }


        // GET: lats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrackDelivery lat = db.TrackDelivery.Find(id);
            if (lat == null)
            {
                return HttpNotFound();
            }
            return View(lat);
        }

        // GET: lats/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: lats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,latituide,longitude")] TrackDelivery lat)
        {
            if (ModelState.IsValid)
            {
                db.TrackDelivery.Add(lat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lat);
        }

        // GET: lats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrackDelivery lat = db.TrackDelivery.Find(id);
            if (lat == null)
            {
                return HttpNotFound();
            }
            return View(lat);
        }

        // POST: lats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,latituide,longitude")] TrackDelivery lat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lat);
        }

        // GET: lats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrackDelivery lat = db.TrackDelivery.Find(id);
            if (lat == null)
            {
                return HttpNotFound();
            }
            return View(lat);
        }

        // POST: lats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrackDelivery lat = db.TrackDelivery.Find(id);
            db.TrackDelivery.Remove(lat);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
