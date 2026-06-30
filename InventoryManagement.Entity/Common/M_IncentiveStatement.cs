using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class M_IncentiveStatement
    {
        public string StatementPeriod { get; set; }
        public int StatementPeriodId { get; set; }
        public string PartyName { get; set; }
        public string GroupCode { get; set; }
        public string partycode { get; set; }
        public string Address { get; set; }
        public string Mobileno { get; set; }
        public string Email { get; set; }
        public decimal ProductSale { get; set; }
        public decimal PVSale { get; set; }
        public decimal TotalIncentivebySelf { get; set; }
        public decimal ChequeAmount { get; set; }
        public decimal self_Comm_BV { get; set; }
        public decimal selfIncomePv { get; set; }
        public decimal bV_Slab { get; set; }
        public decimal pvSlab { get; set; }
        public decimal AdminCharge { get; set; }
        public decimal Self_Comm_BV { get; set; }
        public decimal Diff_Comm_BV { get; set; }
        public decimal Diff_Comm_Pv { get; set; }
        public decimal TotalBVVal { get; set; }
        public decimal TotalPVVal { get; set; }
        public decimal CommOnPv { get; set; }
        public List<MPerformanceInc> DownlineFranchiseePerformance { get; set; }
    }
}