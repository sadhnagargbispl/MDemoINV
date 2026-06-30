using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class Coupons
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string IdNo { get; set; }
        public Nullable<bool> Isuse { get; set; }
        public Nullable<System.DateTime> Rectimestamp { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string BillNo { get; set; }
        public Nullable<decimal> AdjustAmount { get; set; }
    }
}