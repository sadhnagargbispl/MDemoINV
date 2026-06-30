using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class GVCreditNote
    {
        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public decimal BillAmount { get; set; }
        public decimal RCPAmount { get; set; }
        public decimal AmountRecieved { get; set; }
        public decimal CreditNoteValue { get; set; }
    }
}