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
    
    public partial class AccountTable
    {
        public int AccountID { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string BankName { get; set; }
        public decimal AmountInAccount { get; set; }
        public string CapturedBy { get; set; }
        public System.DateTime CapturedDate { get; set; }
        public string Comments { get; set; }
    }
}
