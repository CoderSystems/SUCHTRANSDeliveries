//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inc2SuchTrans.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    public partial class TransactionTable
    {
        [Key]
        public int T_ID { get; set; }
        public DateTime T_Date { get; set; }
        public string E_Code { get; set; }
        public string I_Code { get; set; }
        public Nullable<decimal> Amount { get; set; }
    
        public virtual Expense Expense { get; set; }
        public virtual Income Income { get; set; }
    }
}