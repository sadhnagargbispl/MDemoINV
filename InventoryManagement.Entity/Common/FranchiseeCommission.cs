using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class FranchiseeCommission
    {
        public int TransNo { get; set; }
        public string IsApproved { get; set; }
        public string CommissionType { get; set; }
        public string MonthName { get; set; }
        public string Code { get; set; }
        public string Commission { get; set; }
        public string TotalBV { get; set; }
        public string BVCommission { get; set; }
        public string TotalSale { get; set; }
        public string TDS { get; set; }
        public string NetPayable { get; set; }
        public string BankName { get; set; }
        public string BankAcNo { get; set; }
        public string BranchName { get; set; }
        public string IfscCode { get; set; }
        public string PANNo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string MobileNo { get; set; }
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public string Year { get; set; }
        public string EmailID { get; set; }
        public string GroupName { get; set; }
        public string Address { get; set; }
        public string Date { get; set; }
        public string CommissionSlab { get; set; }
        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public string BillType { get; set; }
        public string PVCommission { get; set; }
        public string PVCommissionVal { get; set; }
        public string PVSlab { get; set; }
    }
}