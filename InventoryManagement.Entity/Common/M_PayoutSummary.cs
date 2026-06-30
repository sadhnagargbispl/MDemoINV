using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class M_PayoutSummary
    {
        public List<MSessids> MSessids { get; set; }
        public string PartyCode { get; set; }
        public string PartyName { get; set; }
    }

    public class MSessids
    {
        public decimal SessID { get; set; }
        public string SessMonth { get; set; }
    }

    public class MonthWiseIncome
    {
        public decimal SessID { get; set; }
        public string PartyCode { get; set; }
        public decimal FormNo { get; set; }
        public decimal Self_Comm_BV { get; set; }
        public decimal Diff_Comm_BV { get; set; }
        public decimal BV_Slab { get; set; }
        public decimal SelfIncomePv { get; set; }
        public decimal Diff_Comm_Pv { get; set; }
        public decimal PvSlab { get; set; }
        public decimal Gross_Amount { get; set; }
        public decimal Tds_Deduction { get; set; }
        public decimal RndOff { get; set; }
        public decimal NetIncome { get; set; }
        public decimal AdminCharge { get; set; }

    }

    public class MPerformanceInc
    {
        public string FranchiseeName { get; set; }
        public decimal SessID { get; set; }
        public decimal Pv { get; set; }
        public string PartyCode { get; set; }
        public decimal From_ID { get; set; }
        public decimal MLevel { get; set; }
        public decimal Diff_Comm_BV { get; set; }
        public decimal SelfSlab { get; set; }
        public decimal DownSlab { get; set; }
        public decimal Diff_Comm_Pv { get; set; }
        public decimal Comm { get; set; }
        public decimal Slab { get; set; }
        public string FromPartyCode { get; set; }
        public decimal BvComm { get; set; }
        public decimal PvComm { get; set; }
    }
}