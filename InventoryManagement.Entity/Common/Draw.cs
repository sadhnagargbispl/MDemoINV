using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class Draw
    {
        public DateTime? FDate { get; set; }
        public string FdateStr { get; set; }
        public decimal ProdCode { get; set; }
        public string ProductName { get; set; }
        public decimal BillSeq { get; set; }
        public decimal AID { get; set; }
        public int UserID { get; set; }
    }

    public class DrawProds
    {
        public string prodString { get; set; }
        public decimal ProdCode { get; set; }
        public string ProductName { get; set; }
        public string ActiveStatus { get; set; }
        public bool IsActive { get; set; }
        public string RowStatus { get; set; }        
    }
}