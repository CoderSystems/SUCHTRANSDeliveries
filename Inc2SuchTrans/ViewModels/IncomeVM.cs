using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inc2SuchTrans.ViewModels
{
    public class IncomeVM
    {
        [Key]
        public int T_ID { get; set; }
        public DateTime? T_Date { get; set; }
        public string I_Code { get; set; }
        public string I_Name { get; set; }
        public decimal? Amount { get; set; }
        public IncomeVM()
        {

        }
    }
}