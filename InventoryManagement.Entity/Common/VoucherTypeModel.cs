using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class VoucherTypeModel
    {
        public int id { get; set; }
        public string Vtype { get; set; }
        public string Voucher_Discrption { get; set; }
        public Nullable<bool> IsWr { get; set; }
        public Nullable<bool> IsShoppe { get; set; }
    }
}