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
        STTransactionEntities FDB = new STTransactionEntities();
        // GET: Finance
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IncomeStat()
        {
            var incDeff = FDB.Incomes;
            var TList = FDB.TransactionTables;
            var incs = new List<IncomeVM>();
            foreach (TransactionTable t in TList)
            {
                if (t.I_Code != null)
                {
                    IncomeVM newinc = new IncomeVM();
                    newinc.T_ID = t.T_ID;
                    newinc.T_Date = t.T_Date;
                    newinc.Amount = t.Amount;
                    newinc.I_Code = t.I_Code;
                    foreach (Income i in incDeff)
                    {
                        if (i.Code == t.I_Code)
                            newinc.I_Name = i.I_Desc;
                    }
                    incs.Add(newinc);
                }

            }

            return View(incs);
        }

        public ActionResult ExpReport()
        {
            var expDeff = FDB.Expenses;
            var TList = FDB.TransactionTables;
            var exps = new List<ExpenseVM>();
            foreach (TransactionTable t in TList)
            {
                if (t.E_Code != null)
                {
                    ExpenseVM newexp = new ExpenseVM();
                    newexp.T_ID = t.T_ID;
                    newexp.T_Date = t.T_Date;
                    newexp.Amount = t.Amount;
                    newexp.E_Code = t.E_Code;
                    foreach (Expense e in expDeff)
                    {
                        if (e.Code == t.E_Code)
                            newexp.E_Name = e.E_Desc;
                    }
                    exps.Add(newexp);
                }

            }
            return View(exps);
        }

        public ActionResult FinanSumm()
        {
            IEnumerable<TransactionTable> TransactionList = FDB.TransactionTables;
            var expDeff = FDB.Expenses;
            var incDeff = FDB.Incomes;
            List<ExpenseVM> el = new List<ExpenseVM>();
            List<IncomeVM> il = new List<IncomeVM>();
            decimal? TotalInc = 0;
            decimal? TotalExp = 0;
            decimal? Final = 0;
            string crdr = "";

            foreach(TransactionTable t in TransactionList)
            {
                if(t.E_Code != null)
                {
                    ExpenseVM e = new ExpenseVM();
                    e.T_ID = t.T_ID;
                    e.T_Date = t.T_Date;
                    e.Amount = t.Amount;
                    e.E_Code = t.E_Code;
                    foreach(Expense oe in expDeff)
                    {
                        if(oe.Code == t.E_Code)
                        {
                            e.E_Name = oe.E_Desc;
                        }
                    }
                    TotalExp += e.Amount;
                    el.Add(e);
                }
                else if (t.I_Code != null)
                {
                    IncomeVM i = new IncomeVM();
                    i.T_ID = t.T_ID;
                    i.T_Date = t.T_Date;
                    i.Amount = t.Amount;
                    i.I_Code = t.I_Code;
                    foreach (Income oi in incDeff)
                    {
                        if (oi.Code == t.I_Code)
                        {
                            i.I_Name = oi.I_Desc;
                        }
                    }
                    TotalInc += i.Amount;
                    il.Add(i);
                }
            }
            Final = TotalInc - TotalExp;
            if (Final < 0)
            {
                crdr = "Loss";
                Final = Final * (-1);
            }
            else if (Final > 0)
            {
                crdr = "Profit";
            }
            else
            {
                crdr = "Break Even";
            }
            ViewBag.TotalInc = "R" + TotalInc.ToString();
            ViewBag.TotalExp = "R" + TotalExp.ToString();
            ViewBag.Summary = "R" + Final.ToString() + " ---- " + crdr;

            var VM = new AllFinanceInfo { incs = il, exps = el };
            return View(VM);
        }

        public ActionResult IncomeBarGraph()
        {
            IEnumerable<TransactionTable> AllT = FDB.TransactionTables;
            IEnumerable<Income> incDeff = FDB.Incomes;
            List<IncomeVM> IncList = new List<IncomeVM>();

            foreach (TransactionTable t in AllT)
            {
                if (t.I_Code != null)
                {
                    int index = IncList.FindIndex(p => p.I_Code == t.I_Code);
                    if(index < 0)
                    {
                        IncomeVM newinc = new IncomeVM();
                        newinc.T_ID = t.T_ID;
                        newinc.T_Date = t.T_Date;
                        newinc.Amount = t.Amount;
                        newinc.I_Code = t.I_Code;
                        foreach (Income i in incDeff)
                        {
                            if (i.Code == t.I_Code)
                                newinc.I_Name = i.I_Desc;
                        }
                        IncList.Add(newinc);
                    }
                    else
                    {
                        IncomeVM currinc = IncList.Find(f => f.I_Code == t.I_Code);
                        currinc.Amount += t.Amount;
                    }
                }
            }

            List<string> xList = new List<string>();
            List<decimal?> yList = new List<decimal?>();

            foreach (IncomeVM si in IncList)
            {
                xList.Add(si.I_Name);
                yList.Add(si.Amount);
            }

            var MyChart = new Chart(width: 600, height: 400)
                .AddTitle("Income Information")
                .AddSeries(
                    name: "Data",
                    xValue: xList,
                    yValues: yList)
                    .Write("png");
            return null;
        }

        public ActionResult ExpenseBarGraph()
        {
            IEnumerable<TransactionTable> AllT = FDB.TransactionTables;
            IEnumerable<Expense> incDeff = FDB.Expenses;
            List<ExpenseVM> IncList = new List<ExpenseVM>();

            foreach (TransactionTable t in AllT)
            {
                if (t.E_Code != null)
                {
                    int index = IncList.FindIndex(p => p.E_Code == t.E_Code);
                    if (index < 0)
                    {
                        ExpenseVM newinc = new ExpenseVM();
                        newinc.T_ID = t.T_ID;
                        newinc.T_Date = t.T_Date;
                        newinc.Amount = t.Amount;
                        newinc.E_Code = t.E_Code;
                        foreach (Expense i in incDeff)
                        {
                            if (i.Code == t.E_Code)
                                newinc.E_Name = i.E_Desc;
                        }
                        IncList.Add(newinc);
                    }
                    else
                    {
                        ExpenseVM currinc = IncList.Find(f => f.E_Code == t.E_Code);
                        currinc.Amount += t.Amount;
                    }
                }
            }

            List<string> xList = new List<string>();
            List<decimal?> yList = new List<decimal?>();

            foreach (ExpenseVM si in IncList)
            {
                xList.Add(si.E_Name);
                yList.Add(si.Amount);
            }

            var MyChart = new Chart(width: 600, height: 400)
                .AddTitle("Expense Information")
                .AddSeries(
                    name: "Data",
                    xValue: xList,
                    yValues: yList)
                    .Write("png");
            return null;
        }

        public ActionResult ExpensePieChart()
        {

            IEnumerable<TransactionTable> AllT = FDB.TransactionTables;
            IEnumerable<Expense> incDeff = FDB.Expenses;
            List<ExpenseVM> IncList = new List<ExpenseVM>();

            foreach (TransactionTable t in AllT)
            {
                if (t.E_Code != null)
                {
                    int index = IncList.FindIndex(p => p.E_Code == t.E_Code);
                    if (index < 0)
                    {
                        ExpenseVM newinc = new ExpenseVM();
                        newinc.T_ID = t.T_ID;
                        newinc.T_Date = t.T_Date;
                        newinc.Amount = t.Amount;
                        newinc.E_Code = t.E_Code;
                        foreach (Expense i in incDeff)
                        {
                            if (i.Code == t.E_Code)
                                newinc.E_Name = i.E_Desc;
                        }
                        IncList.Add(newinc);
                    }
                    else
                    {
                        ExpenseVM currinc = IncList.Find(f => f.E_Code == t.E_Code);
                        currinc.Amount += t.Amount;
                    }
                }
            }

            List<string> xList = new List<string>();
            List<decimal?> yList = new List<decimal?>();

            foreach (ExpenseVM si in IncList)
            {
                xList.Add(si.E_Name);
                yList.Add(si.Amount);
            }


            var MyChart = new Chart(width: 600, height: 400)
                .AddTitle("Expense Information")
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
            IEnumerable<TransactionTable> AllT = FDB.TransactionTables;
            IEnumerable<Income> incDeff = FDB.Incomes;
            List<IncomeVM> IncList = new List<IncomeVM>();

            foreach (TransactionTable t in AllT)
            {
                if (t.I_Code != null)
                {
                    int index = IncList.FindIndex(p => p.I_Code == t.I_Code);
                    if (index < 0)
                    {
                        IncomeVM newinc = new IncomeVM();
                        newinc.T_ID = t.T_ID;
                        newinc.T_Date = t.T_Date;
                        newinc.Amount = t.Amount;
                        newinc.I_Code = t.I_Code;
                        foreach (Income i in incDeff)
                        {
                            if (i.Code == t.I_Code)
                                newinc.I_Name = i.I_Desc;
                        }
                        IncList.Add(newinc);
                    }
                    else
                    {
                        IncomeVM currinc = IncList.Find(f => f.I_Code == t.I_Code);
                        currinc.Amount += t.Amount;
                    }
                }
            }

            List<string> xList = new List<string>();
            List<decimal?> yList = new List<decimal?>();

            foreach (IncomeVM si in IncList)
            {
                xList.Add(si.I_Name);
                yList.Add(si.Amount);
            }

            var MyChart = new Chart(width: 600, height: 400)
                .AddTitle("Income Information")
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
            IEnumerable<TransactionTable> TList = FDB.TransactionTables;
            decimal? Tinc = 0;
            decimal? Texp = 0;

            foreach(TransactionTable t in TList)
            {
                if (t.E_Code != null)
                    Texp += t.Amount;
                else if (t.I_Code != null)
                    Tinc += t.Amount;
            }

            List<string> xList = new List<string> { "Income", "Expense" };
            List<decimal?> yList = new List<decimal?> { Tinc, Texp };

            string myTheme =
                    @"<Chart BackColor=""Transparent"" >
        	                                    <ChartAreas>
                                                <ChartArea Name=""Default"" BackColor=""Transparent""></ChartArea>
        	                                    </ChartAreas>
        	                                </Chart>";

            var MyChart = new Chart(width: 600, height: 400, theme: myTheme)
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


