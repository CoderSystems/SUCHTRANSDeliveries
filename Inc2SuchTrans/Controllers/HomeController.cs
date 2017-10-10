using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inc2SuchTrans.Models;

namespace Inc2SuchTrans.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Start()
        {
            return View();
        }
        public ActionResult Index()
        {
            if (User.IsInRole("Super Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else
                if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else
                if (User.IsInRole("Operations Manager"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else
                if (User.IsInRole("Driver"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            STLogisticsEntities db = new STLogisticsEntities();
            var ctact = db.Contact.First();
            ViewBag.Message = "Your contact page.";

            return View(ctact);
        }
    }
}