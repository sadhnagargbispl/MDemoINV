using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class M_INSERTPVBV
    {
        public string act { get; set; }
        public string uid { get; set; }
        public string logkey { get; set; }
        public string billno { get; set; }
        public string billdate { get; set; }
        public string bv { get; set; }
        public string partycode { get; set; }
        public string PV { get; set; }
        public string billamount { get; set; }
    }


    public class INSERTPVBVRes 
    {
        public string Success { get; set; }
        public string ApiMessage { get; set; }
        public Result Result { get; set; }
    }

    public class Result
    {
        public string billno { get; set; }
        public string status { get; set; }
    }

}