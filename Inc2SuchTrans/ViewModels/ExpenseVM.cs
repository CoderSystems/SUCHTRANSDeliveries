using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inc2SuchTrans.ViewModels
{
    public class ExpenseVM
    {
        public int T_ID { get; set; }
        public DateTime? T_Date { get; set; }
        public string E_Code { get; set; }
        public string E_Name { get; set; }
        public decimal? Amount { get; set; }
        public ExpenseVM()
        {

        }
    }
}