using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class BatchCode
    {
        public int ProductId { get; set; }
        public decimal  Batchno { get; set; }

        public string Batchcode { get; set; }
        public string GeneratedBy { get; set; }
        public DateTime GenerateDate { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal MRP { get; set; }
        public decimal DP { get; set; }
        public decimal Bv { get; set; }

        public string IsExpireStatus { get; set; }
        public string IsExpirable { get; set; }
        public string ExpDateStr { get; set; }
        public string MfgDateStr { get; set; }
        public DateTime? ExpDate { get; set; }
        public DateTime? MfgDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public string IsAdd { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string objProductListStr { get; set; }
        public string ExisitingBatchcode { get; set; }
        public string ActiveStatus { get; set; }
        public decimal BatchcodeId { get; set; }
        public string purchasefrom { get; set; }
        public decimal PV { get; set; }
    }
}