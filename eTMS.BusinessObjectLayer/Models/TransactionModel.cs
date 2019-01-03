using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTMS.BusinessObjectLayer.Models
{
    public class TransactionModel
    {
        public int TransactionID { get; set; }
        public string TID { get; set; }
        public int FID { get; set; }
        public decimal AmountPaid { get; set; }
        public Nullable<decimal> AmountOutstanding { get; set; }
        public int AccountID { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public string EffectiveDateString { get; set; }
        public int DealerID { get; set; }
        public Nullable<System.DateTime> CapturedDate { get; set; }
        public string CapturedBy { get; set; }
        public string PaymentMode { get; set; }
        public string Narration { get; set; }
        public string Comments { get; set; }
        public string Bank { get; set; }
    }
}
