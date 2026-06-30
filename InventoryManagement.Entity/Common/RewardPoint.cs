using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class RewardPoint
    {
        public Int32 ID { get; set; }
        public Int32 SlabID { get; set; }
        public DateTime FromDate { get; set; }
        public string FromDateStr { get; set; }
        public string IsActionName { get; set; }
        public DateTime ToDate { get; set; }
        public string ToDateStr { get; set; }
        public decimal RPoint { get; set; }
        public bool IsActive { get; set; }
        public string SlabRange { get; set; }
    }
}