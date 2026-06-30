using InventoryManagement.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class IssueSampleProduct
    {
        public string TransNo { get; set; }
        public string RefNo { get; set; }
        public string partyCode { get; set; }
        public string partyName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public DateTime TransDate { get; set; }
        public string TransDateStr { get; set; }
        public DateTime IssueDate { get; set; }
        public string IssueDateStr { get; set; }
        public string Barcode { get; set; }
        public int Qty { get; set; }
        public string Remark { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime RecTimeStamp { get; set; }
    }
}