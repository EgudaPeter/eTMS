//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eTMS.BusinessObjectLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Annual
    {
        public int AID { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public decimal TotalTransactionAmount { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal Profit { get; set; }
        public decimal Loss { get; set; }
    }
}
