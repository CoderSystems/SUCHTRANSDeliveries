using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inc2SuchTrans.ViewModels
{
    public class ExpenseVM
    {
        [Key]
        public int T_ID { get; set; }
        public Nullable<DateTime> T_Date { get; set; }
        public string E_Code { get; set; }
        public string E_Name { get; set; }
        public decimal? Amount { get; set; }
        public ExpenseVM()
        {
            
        }
    }
}