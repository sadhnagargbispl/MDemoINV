using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class GSTMasterLIST
    {
        public int id { get; set; }
        public Nullable<decimal> GSTTax { get; set; }
        public string objGSTStr { get; set; } 
    }
}