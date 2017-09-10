using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inc2SuchTrans.ViewModels
{
    public class SimpleExpenseVM
    {

        public string ExpName { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Total { get; set; }

        public SimpleExpenseVM()
        {

        }
    }
}