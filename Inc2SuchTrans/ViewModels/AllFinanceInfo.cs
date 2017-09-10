using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inc2SuchTrans.Models;

namespace Inc2SuchTrans.ViewModels
{
    public class AllFinanceInfo
    {
        public List<SimpleExpenseVM> exps { get; set; }
        public List<SimpleIncomeVM> incs { get; set; }

        public AllFinanceInfo()
        {

        }
        
       
    }
}