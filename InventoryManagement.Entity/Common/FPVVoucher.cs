using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class FPVVoucher
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string IdNo { get; set; }
        public Nullable<bool> Isuse { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> AdjustAmount { get; set; }
        public string BillNo { get; set; }
        public Nullable<decimal> credit { get; set; }
        public Nullable<decimal> debit { get; set; }
        public Nullable<decimal> balance { get; set; }
        public Nullable<decimal> TotalBalance { get; set; }
    }

    public class FPVoucherEligibilityResult
    {
        public string EligibilityStatus { get; set; }
        public string Reason { get; set; }
    }
}