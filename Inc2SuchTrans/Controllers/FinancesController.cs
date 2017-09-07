using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inc2SuchTrans.Models;
using Inc2SuchTrans.ViewModels;

namespace Inc2SuchTrans.Controllers
{
    public class FinancesController : Controller
    {
        STFinancesEntities FDB = new STFinancesEntities();
        // GET: Finance
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IncomeStat()
        {
            var incs = FDB.Incomes;
            return View(incs);
        }

        public ActionResult ExpReport()
        {
            var exps = FDB.Expenses;
            return View(exps);
        }

        public ActionResult FinanSumm()
        {
            IEnumerable<Income> IncomeList = FDB.Incomes;
            IEnumerable<Expense> ExpenseList = FDB.Expenses;
            var VM = new AllFinanceInfo { Incs = IncomeList, Exps = ExpenseList };
            return View(VM);
        }
    }
}