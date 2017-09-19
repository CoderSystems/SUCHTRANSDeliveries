using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inc2SuchTrans.Models;

namespace Inc2SuchTrans.ViewModels
{
    public class AllFinanceInfo
    {
        public List<ExpenseVM> exps { get; set; }
        public List<IncomeVM> incs { get; set; }

        public AllFinanceInfo()
        {

        }
        
       
    }
}