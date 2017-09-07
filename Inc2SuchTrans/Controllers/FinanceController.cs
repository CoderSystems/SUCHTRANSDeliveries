using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inc2SuchTrans.Models;

namespace Inc2SuchTrans.Controllers
{
    public class FinanceController : Controller
    {
        STFinancesEntities db = new STFinancesEntities();
        // GET: Finance
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IncomeStat()
        {
            var incs = db.Incomes;
            return View(incs);
        }
    }
}