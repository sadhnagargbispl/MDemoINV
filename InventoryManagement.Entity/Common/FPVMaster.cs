using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class FPVMaster
    {
        public int AID { get; set; }
        public decimal BVValue { get; set; }
        public string OnBillType { get; set; }
        public decimal FPV { get; set; }
        public string IsMultiple { get; set; }
        public string ActiveStatus { get; set; }
        public string RowStatus { get; set; }
        public DateTime RecTimeStamp { get; set; }
        public int ExpiredinDays { get; set; }
        public string productString { get; set; }
        public int BunchID { get; set; }
        public List<OfferProducts> products { get; set; }
    }
}