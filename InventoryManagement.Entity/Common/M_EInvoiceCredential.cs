using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class M_EInvoiceCredential
    {
        public int id { get; set; }
        public string Aspid { get; set; }
        public string ApiPassword { get; set; }
        public string Gstin { get; set; }
        public string Username { get; set; }
        public string EInvPwd { get; set; }
        public Nullable<System.DateTime> RectimeStamp { get; set; }
        public Nullable<System.DateTime> Modifydate { get; set; }
        public string ActiveStatus { get; set; }
    }
}