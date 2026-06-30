using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class CourierchargeByWeight
   {
        public decimal Id { get; set; }
        public decimal Weight { get; set; }
        public decimal Amount { get; set; }
        public string isAdd { get; set; }
    }
}