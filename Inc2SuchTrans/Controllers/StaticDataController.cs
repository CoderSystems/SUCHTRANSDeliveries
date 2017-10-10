﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inc2SuchTrans.Models;
using System.Net;
using System.Data.Entity;

namespace Inc2SuchTrans.Controllers
{
    public class StaticDataController : Controller
    {
        STLogisticsEntities db = new STLogisticsEntities();
        // GET: StaticData
        public ActionResult Index()
        {
            try
            {
                return View(db.Contact.ToList());
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,Address,Tel,Fax,Cell,Email")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contact.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contact);
        }


        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contact.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContactID,CompanyAddress,Tel,Fax,Cell,Email")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                var ctact = db.Contact.Find(contact.ContactID);
                ctact.CompanyAddress = contact.CompanyAddress;
                ctact.Tel = contact.Tel;
                ctact.Fax = contact.Fax;
                ctact.Cell = contact.Cell;
                ctact.Email = contact.Email;
                db.Entry(ctact).State = EntityState.Modified;
                //db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Contact", "Home", null);
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contact.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contact.Find(id);
            db.Contact.Remove(contact);
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