using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inc2SuchTrans.Models;
using Inc2SuchTrans.ViewModels;
using System.Web.Helpers;

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
            List<SimpleExpenseVM> el = new List<SimpleExpenseVM>();
            List<SimpleIncomeVM> il = new List<SimpleIncomeVM>();
            var VM = new AllFinanceInfo { incs = il, exps = el};
            decimal? TotalInc = 0;
            decimal? TotalExp = 0;
            decimal? Final = 0;
            string crdr = "";
            foreach(Income i in IncomeList)
            {
                VM.incs.Add(new SimpleIncomeVM { IncName = i.IncName, Amount = i.Amount, Total = 0 });
                TotalInc += i.Amount;
            }

            foreach(Expense e in ExpenseList)
            {
                VM.exps.Add(new SimpleExpenseVM { ExpName = e.ExpName, Amount=e.Amount,Total=0 });
                TotalExp += e.Amount;
            }
            Final = TotalInc - TotalExp;
            if(Final < 0)
            {
                crdr = "Loss";
                Final = Final * (-1);
            }
            else if(Final > 0)
            {
                crdr = "Profit";
            }
            else
            {
                crdr = "Break Even";
            }
            ViewBag.TotalInc = "R"+TotalInc.ToString();
            ViewBag.TotalExp = "R"+TotalExp.ToString();
            ViewBag.Summary = "R" + Final.ToString() + " ---- " + crdr;
            return View(VM);
        }

        public ActionResult Chart()
        {
            return View();
        }

        public ActionResult IncomeBarGraph()
        {
            IEnumerable<Income> IncomeList = FDB.Incomes;
            List<SimpleIncomeVM> SimIncList = new List<SimpleIncomeVM>();
            int count = IncomeList.Count();

            foreach (Income i in IncomeList)
            {
                int index = SimIncList.FindIndex(p => p.IncName == i.IncName);
                if ( index < 0)
                {
                    SimpleIncomeVM newinc = new SimpleIncomeVM();
                    newinc.IncName = i.IncName;
                    newinc.Amount = i.Amount;
                    newinc.Total = i.Amount;
                    SimIncList.Add(newinc);                    
                }
                else
                {
                    SimpleIncomeVM currinc = SimIncList.Find(f => f.IncName == i.IncName);
                    currinc.Total += i.Amount;
                }
            }

            List<string> xList = new List<string>();
            List<decimal?> yList = new List<decimal?>();

            foreach(SimpleIncomeVM si in SimIncList)
            {
                xList.Add(si.IncName);
                yList.Add(si.Total);
            }            

            var MyChart = new Chart(width: 600, height: 400)
                .AddTitle("Expense Summary")
                .AddSeries(
                    name: "Data",
                    xValue: xList,
                    yValues: yList)
                    .Write("png");
            return null;
        }

        public ActionResult ExpenseBarGraph()
        {
            IEnumerable<Expense> ExpenseList = FDB.Expenses;
            List<SimpleExpenseVM> SimExpList = new List<SimpleExpenseVM>();
            int count = ExpenseList.Count();

            foreach (Expense i in ExpenseList)
            {
                int index = SimExpList.FindIndex(p => p.ExpName == i.ExpName);
                if (index < 0)
                {
                    SimpleExpenseVM newexp = new SimpleExpenseVM();
                    newexp.ExpName = i.ExpName;
                    newexp.Amount = i.Amount;
                    newexp.Total = i.Amount;
                    SimExpList.Add(newexp);
                }
                else
                {
                    SimpleExpenseVM currexp = SimExpList.Find(f => f.ExpName == i.ExpName);
                    currexp.Total += i.Amount;
                }
            }

            List<string> xList = new List<string>();
            List<decimal?> yList = new List<decimal?>();

            foreach (SimpleExpenseVM si in SimExpList)
            {
                xList.Add(si.ExpName);
                yList.Add(si.Total);
            }

            var MyChart = new Chart(width: 600, height: 400)
                .AddTitle("Income Summary")
                .AddSeries(
                    name: "Data",
                    xValue: xList,
                    yValues: yList)
                    .Write("png");
            return null;
        }

        public ActionResult ExpensePieChart()
        {
            IEnumerable<Expense> ExpenseList = FDB.Expenses;
            List<SimpleExpenseVM> SimExpList = new List<SimpleExpenseVM>();
            int count = ExpenseList.Count();

            foreach (Expense i in ExpenseList)
            {
                int index = SimExpList.FindIndex(p => p.ExpName == i.ExpName);
                if (index < 0)
                {
                    SimpleExpenseVM newexp = new SimpleExpenseVM();
                    newexp.ExpName = i.ExpName;
                    newexp.Amount = i.Amount;
                    newexp.Total = i.Amount;
                    SimExpList.Add(newexp);
                }
                else
                {
                    SimpleExpenseVM currexp = SimExpList.Find(f => f.ExpName == i.ExpName);
                    currexp.Total += i.Amount;
                }
            }

            List<string> xList = new List<string>();
            List<decimal?> yList = new List<decimal?>();

            foreach (SimpleExpenseVM si in SimExpList)
            {
                xList.Add(si.ExpName);
                yList.Add(si.Total);
            }

            var MyChart = new Chart(width: 600, height: 400)
                .AddTitle("Income Summary")
                .AddSeries(
                    name: "Data",
                    chartType: "Pie",
                    xValue: xList,
                    yValues: yList)
                    .Write("png");
            return null;
        }

        public ActionResult IncomePieChart()
        {
            IEnumerable<Income> IncomeList = FDB.Incomes;
            List<SimpleIncomeVM> SimIncList = new List<SimpleIncomeVM>();
            int count = IncomeList.Count();

            foreach (Income i in IncomeList)
            {
                int index = SimIncList.FindIndex(p => p.IncName == i.IncName);
                if (index < 0)
                {
                    SimpleIncomeVM newinc = new SimpleIncomeVM();
                    newinc.IncName = i.IncName;
                    newinc.Amount = i.Amount;
                    newinc.Total = i.Amount;
                    SimIncList.Add(newinc);
                }
                else
                {
                    SimpleIncomeVM currinc = SimIncList.Find(f => f.IncName == i.IncName);
                    currinc.Total += i.Amount;
                }
            }

            List<string> xList = new List<string>();
            List<decimal?> yList = new List<decimal?>();

            foreach (SimpleIncomeVM si in SimIncList)
            {
                xList.Add(si.IncName);
                yList.Add(si.Total);
            }

            var MyChart = new Chart(width: 600, height: 400)
                .AddTitle("Expense Summary")
                .AddSeries(
                    name: "Data",
                    chartType: "Pie",
                    xValue: xList,
                    yValues: yList)
                    .Write("png");
            return null;
        }

        public ActionResult NetDoughnutChart()
        {
            IEnumerable<Income> IncomeList = FDB.Incomes;
            decimal? Tinc = 0;
            decimal? Texp = 0;

            foreach (Income i in IncomeList)
            {
                Tinc += i.Amount;
            }

            IEnumerable<Expense> ExpenseList = FDB.Expenses;

            foreach (Expense i in ExpenseList)
            {
                Texp += i.Amount;
            }

            List<string> xList = new List<string> { "Income","Expense"};
            List<decimal?> yList = new List<decimal?> { Tinc, Texp};

            string myTheme =
                    @"<Chart BackColor=""Transparent"" >
	                                    <ChartAreas>
                                        <ChartArea Name=""Default"" BackColor=""Transparent""></ChartArea>
	                                    </ChartAreas>
	                                </Chart>";
            
            var MyChart = new Chart(width: 600, height: 400, theme:myTheme)
                .AddTitle("Net Summary")
                .AddSeries(
                    name: "Data",
                    chartType: "Doughnut",
                    xValue: xList,
                    yValues: yList)
                    
                    .Write("png");

            return null;
        }
    }
}