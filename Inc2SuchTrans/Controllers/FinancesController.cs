using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inc2SuchTrans.Models;
using Inc2SuchTrans.ViewModels;
using System.Web.Helpers;
using Inc2SuchTrans.BLL;
using Inc2SuchTrans.CustomFilters;

namespace Inc2SuchTrans.Controllers
{
    public class FinancesController : Controller
    {
        STLogisticsEntities FDB = new STLogisticsEntities();
        // GET: Finance
        [AuthLog(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [AuthLog(Roles = "Admin")]
        public ActionResult IncomeStat()
        {
            List<string> DateList = TransactionLogic.ListOfDates();
            ViewBag.DatesList = DateList;

            var incDeff = FDB.Income;
            var TList = FDB.TransactionTable;
            ViewBag.ChartDate = null;
            var incs = new List<IncomeVM>();
            foreach (TransactionTable t in TList)
            {
                if (t.I_Code != null)
                {
                    IncomeVM newinc = new IncomeVM();
                    newinc.T_ID = t.T_ID;
                    newinc.T_Date = t.T_Date.Value.Date;
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
        [AuthLog(Roles = "Admin")]
        [HttpPost]
        public ActionResult IncomeStat(DateTime StartMonth)
        {
            List<string> DateList = TransactionLogic.ListOfDates();
            ViewBag.DatesList = DateList;
            var incDeff = FDB.Income;
            var TList = FDB.TransactionTable;
            var incs = new List<IncomeVM>();
            foreach (TransactionTable t in TList)
            {
                if (t.I_Code != null)
                {
                    IncomeVM newinc = new IncomeVM();
                    newinc.T_ID = t.T_ID;
                    newinc.T_Date = t.T_Date.Value.Date;
                    newinc.Amount = t.Amount;
                    newinc.I_Code = t.I_Code;
                    foreach (Income i in incDeff)
                    {
                        if (i.Code == t.I_Code)
                            newinc.I_Name = i.I_Desc;
                    }
                    if(newinc.T_Date >= StartMonth)
                    {
                        incs.Add(newinc);
                    }
                }

            }
            ViewBag.ChartDate = StartMonth;
            return View(incs);
        }

        [AuthLog(Roles = "Admin")]
        public ActionResult ExpReport()
        {
            ViewBag.ChartDate = null;

            List<string> DateList = TransactionLogic.ListOfDates();
            ViewBag.DatesList = DateList;

            var expDeff = FDB.Expense;
            var TList = FDB.TransactionTable;
            var exps = new List<ExpenseVM>();
            foreach (TransactionTable t in TList)
            {
                if (t.E_Code != null)
                {
                    ExpenseVM newexp = new ExpenseVM();
                    newexp.T_ID = t.T_ID;
                    newexp.T_Date = t.T_Date.Value.Date;
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

        [AuthLog(Roles = "Admin")]
        [HttpPost]
        public ActionResult ExpReport(DateTime StartMonth)
        {
            List<string> DateList = TransactionLogic.ListOfDates();
            ViewBag.DatesList = DateList;

            var expDeff = FDB.Expense;
            var TList = FDB.TransactionTable;
            var exps = new List<ExpenseVM>();

            ViewBag.ChartDate = StartMonth;
            
            foreach (TransactionTable t in TList)
            {
                
                    if (t.E_Code != null)
                    {
                        ExpenseVM newexp = new ExpenseVM();
                        newexp.T_ID = t.T_ID;
                        newexp.T_Date = t.T_Date.Value.Date;
                        newexp.Amount = t.Amount;
                        newexp.E_Code = t.E_Code;
                        foreach (Expense e in expDeff)
                        {
                            if (e.Code == t.E_Code)
                                newexp.E_Name = e.E_Desc;
                        }
                        if (newexp.T_Date >= StartMonth)
                        {
                            exps.Add(newexp);
                        }
                    }
                
            }
            return View(exps);
        }

        [AuthLog(Roles = "Admin")]
        public ActionResult FinanSumm()
        {
            List<string> DateList = TransactionLogic.ListOfDates();
            ViewBag.DatesList = DateList;

            ViewBag.ChartDate = null;
            IEnumerable<TransactionTable> TransactionList = FDB.TransactionTable;
            var expDeff = FDB.Expense;
            var incDeff = FDB.Income;
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
                    e.T_Date = t.T_Date.Value.Date;
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
                    i.T_Date = t.T_Date.Value.Date;
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

        [AuthLog(Roles = "Admin")]
        [HttpPost]
        public ActionResult FinanSumm(DateTime StartMonth)
        {
            List<string> DateList = TransactionLogic.ListOfDates();
            ViewBag.DatesList = DateList;
            IEnumerable<TransactionTable> TransactionList = FDB.TransactionTable;
            var expDeff = FDB.Expense;
            var incDeff = FDB.Income;
            List<ExpenseVM> el = new List<ExpenseVM>();
            List<IncomeVM> il = new List<IncomeVM>();
            decimal? TotalInc = 0;
            decimal? TotalExp = 0;
            decimal? Final = 0;
            string crdr = "";

            foreach (TransactionTable t in TransactionList)
            {
                if (t.E_Code != null)
                {
                    ExpenseVM e = new ExpenseVM();
                    e.T_ID = t.T_ID;
                    e.T_Date = t.T_Date.Value.Date;
                    e.Amount = t.Amount;
                    e.E_Code = t.E_Code;
                    foreach (Expense oe in expDeff)
                    {
                        if (oe.Code == t.E_Code)
                        {
                            e.E_Name = oe.E_Desc;
                        }
                    }
                    if (e.T_Date >= StartMonth)
                    {
                        TotalExp += e.Amount;
                        el.Add(e);
                    }
                }
                else if (t.I_Code != null)
                {
                    IncomeVM i = new IncomeVM();
                    i.T_ID = t.T_ID;
                    i.T_Date = t.T_Date.Value.Date;
                    i.Amount = t.Amount;
                    i.I_Code = t.I_Code;
                    foreach (Income oi in incDeff)
                    {
                        if (oi.Code == t.I_Code)
                        {
                            i.I_Name = oi.I_Desc;
                        }
                    }
                    if (i.T_Date >= StartMonth)
                    {
                        TotalInc += i.Amount;
                        il.Add(i);
                    }
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
            ViewBag.ChartDate = StartMonth;
            return View(VM);
        }

        public ActionResult IncomeBarGraph(DateTime? ChartDate)
        {
            IEnumerable<TransactionTable> AllT = FDB.TransactionTable;
            IEnumerable<Income> incDeff = FDB.Income;
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
                        newinc.T_Date = t.T_Date.Value.Date;
                        newinc.Amount = t.Amount;
                        newinc.I_Code = t.I_Code;
                        foreach (Income i in incDeff)
                        {
                            if (i.Code == t.I_Code)
                                newinc.I_Name = i.I_Desc;
                        }
                        if (ChartDate != null)
                        {
                            if (newinc.T_Date >= ChartDate)
                            {
                                IncList.Add(newinc);
                            }
                        }
                        else
                        {
                            IncList.Add(newinc);
                        }
                    }
                    else
                    {
                        IncomeVM currinc = IncList.Find(f => f.I_Code == t.I_Code);
                        if(ChartDate != null)
                        {
                            if(t.T_Date >= ChartDate)
                            {
                                currinc.Amount += t.Amount;
                            }
                        }
                        else
                        {
                            currinc.Amount += t.Amount;
                        }                        
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
            if (ChartDate != null)
            {
                var MyChart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
                    .AddTitle("Income Information including and after" + TransactionLogic.Month(ChartDate.Value.Month.ToString()) + " " + ChartDate.Value.Year)
                    .AddSeries(
                        name: "Data",
                        xValue: xList,
                        yValues: yList)
                        .Write("png");
            }
            else
            {
                var MyChart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
                    .AddTitle("Default Income Information")
                    .AddSeries(
                        name: "Data",
                        xValue: xList,
                        yValues: yList)
                        .Write("png");
            }
            return null;
        }

        

        public ActionResult ExpenseBarGraph(DateTime? ChartDate)
        {
            
            
            IEnumerable<TransactionTable> AllT = FDB.TransactionTable;
            IEnumerable<Expense> incDeff = FDB.Expense;
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
                        newinc.T_Date = t.T_Date.Value.Date;
                        newinc.Amount = t.Amount;
                        newinc.E_Code = t.E_Code;
                        foreach (Expense i in incDeff)
                        {
                            if (i.Code == t.E_Code)
                                newinc.E_Name = i.E_Desc;
                        }
                        if (ChartDate != null)
                        {
                            if (newinc.T_Date >= ChartDate)
                            {
                                IncList.Add(newinc);
                            }
                        }
                        else
                        {
                            IncList.Add(newinc);
                        }
                    }
                    else
                    {
                        ExpenseVM currinc = IncList.Find(f => f.E_Code == t.E_Code);
                        if(ChartDate != null)
                        {
                            if(t.T_Date >= ChartDate)
                            {
                                currinc.Amount += t.Amount;
                            }
                        }
                        else
                        {
                            currinc.Amount += t.Amount;
                        }                        
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
            if (ChartDate != null)
            {
                var MyChart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
                    .AddTitle("Expense Information including and after " + TransactionLogic.Month(ChartDate.Value.Month.ToString()) + " " + ChartDate.Value.Year)
                    .AddSeries(
                        name: "Data",
                        xValue: xList,
                        yValues: yList)
                        .Write("png");
            }
            else
            {
                var MyChart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
                    .AddTitle("Default Expense Information")
                    .AddSeries(
                        name: "Data",
                        xValue: xList,
                        yValues: yList)
                        .Write("png");
            }
            return null;
        }

        public ActionResult ExpensePieChart(DateTime? ChartDate)
        {

            IEnumerable<TransactionTable> AllT = FDB.TransactionTable;
            IEnumerable<Expense> incDeff = FDB.Expense;
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
                        newinc.T_Date = t.T_Date.Value.Date;
                        newinc.Amount = t.Amount;
                        newinc.E_Code = t.E_Code;
                        foreach (Expense i in incDeff)
                        {
                            if (i.Code == t.E_Code)
                                newinc.E_Name = i.E_Desc;
                        }
                        if (ChartDate != null)
                        {
                            if (newinc.T_Date >= ChartDate)
                            {
                                IncList.Add(newinc);
                            }
                        }
                        else
                        {
                            IncList.Add(newinc);
                        }
                    }
                    else
                    {
                        ExpenseVM currinc = IncList.Find(f => f.E_Code == t.E_Code);
                        if (ChartDate != null)
                        {
                            if (t.T_Date >= ChartDate)
                            {
                                currinc.Amount += t.Amount;
                            }

                        }
                        else
                        {
                            currinc.Amount += t.Amount;
                        }
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

            if(ChartDate != null)
            {
                var MyChart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
                .AddTitle("Expense Information including and after " + TransactionLogic.Month(ChartDate.Value.Month.ToString()) + " " + ChartDate.Value.Year)
                .AddSeries(
                    name: "Data",
                    chartType: "Pie",
                    xValue: xList,
                    yValues: yList)
                    .Write("png");
            }
            else
            {
                var MyChart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
                .AddTitle("Expense Information")
                .AddSeries(
                    name: "Data",
                    chartType: "Pie",
                    xValue: xList,
                    yValues: yList)
                    .Write("png");
            }
            
            return null;
        }

        public ActionResult IncomePieChart(DateTime? ChartDate)
        {
            IEnumerable<TransactionTable> AllT = FDB.TransactionTable;
            IEnumerable<Income> incDeff = FDB.Income;
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
                        newinc.T_Date = t.T_Date.Value.Date;
                        newinc.Amount = t.Amount;
                        newinc.I_Code = t.I_Code;
                        foreach (Income i in incDeff)
                        {
                            if (i.Code == t.I_Code)
                                newinc.I_Name = i.I_Desc;
                        }
                        if (ChartDate != null)
                        {
                            if (newinc.T_Date >= ChartDate)
                            {
                                IncList.Add(newinc);
                            }
                        }
                        else
                        {
                            IncList.Add(newinc);
                        }
                    }
                    else
                    {
                        IncomeVM currinc = IncList.Find(f => f.I_Code == t.I_Code);
                        if(ChartDate != null)
                        {
                            if(t.T_Date >= ChartDate)
                            {
                                currinc.Amount += t.Amount;
                            }
                        }
                        else
                        {
                        currinc.Amount += t.Amount;

                        }
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

            if (ChartDate != null)
            {
                var MyChart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
                    .AddTitle("Income Information including and after " + TransactionLogic.Month(ChartDate.Value.Month.ToString()) + " " + ChartDate.Value.Year)
                    .AddSeries(
                        name: "Data",
                        chartType: "Pie",
                        xValue: xList,
                        yValues: yList)
                        .Write("png");
            }
            else
            {
                var MyChart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
                    .AddTitle("Default Income Information")
                    .AddSeries(
                        name: "Data",
                        chartType: "Pie",
                        xValue: xList,
                        yValues: yList)
                        .Write("png");
            }
            return null;
        }

        public ActionResult NetDoughnutChart(DateTime? ChartDate)
        {
                IEnumerable<TransactionTable> TList = FDB.TransactionTable;
                decimal? Tinc = 0;
                decimal? Texp = 0;
                foreach (TransactionTable t in TList)
                {
                if (ChartDate != null)
                {
                    if (t.T_Date >= ChartDate)
                    {
                        if (t.E_Code != null)
                            Texp += t.Amount;
                        else if (t.I_Code != null)
                            Tinc += t.Amount;
                    }
                }
                else
                {


                    if (t.E_Code != null)
                        Texp += t.Amount;
                    else if (t.I_Code != null)
                        Tinc += t.Amount;
                }
                    
            }
            List<string> xList = new List<string> { "Income", "Expense" };
            List<decimal?> yList = new List<decimal?> { Tinc, Texp };
            
            if (ChartDate != null)
            {
                var MyChart = new Chart(width: 600, height: 400, theme:ChartTheme.Blue)
                .AddTitle("Net Summary Including and After " + TransactionLogic.Month(ChartDate.Value.Month.ToString()) + " " + ChartDate.Value.Year)
                .AddSeries(
                    name: "Data",
                    chartType: "Doughnut",
                    xValue: xList,
                    yValues: yList)

                    .Write("png");
            }
            else
            {
                var MyChart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
                    .AddTitle("Net Summary")
                    .AddSeries(
                        name: "Data",
                        chartType: "Doughnut",
                        xValue: xList,
                        yValues: yList)

                        .Write("png");
            }
            return null;
        }
        
    }
}


