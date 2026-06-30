using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class DashboardSummary
    {
        public List<DashboardColumn> columns { get; set; }
        public List<Dashboard>DashboardData { get; set; }
    }

    public class DashboardColumn
    {
        public string column { get; set; }
        public string Name { get; set; }
    }

    public class Dashboard
    {
        public decimal WalletBalance { get; set; }
        public string PartyCode { get; set; }   
        public string PartyName { get; set; }
        public string UserName { get; set; }
        public decimal TotalSale  { get; set; }
        public decimal TotalFV  { get; set; }
        public decimal TotalRV { get; set; }
        public decimal TotalFVAmt { get; set; }   
        public decimal TotalRVAmt { get; set; }
        public decimal LastMnthSale { get; set; }
        public decimal LastMnthFV { get; set; }
        public decimal LastMnthRV { get; set; }
        public decimal LastMnthFVAmt { get; set; }
        public decimal LastMnthRVAmt { get; set; }
        public decimal LastMnthTDAmt { get; set; }
        public decimal LastMnthTDFV { get; set; }
        public decimal LastMnthTDRV { get; set; }
        public decimal LastMnthTDFVAmt { get; set; }
        public decimal LastMnthTDRVAmt { get; set; }
        public decimal MnthSale { get; set; }
        public decimal MnthFV { get; set; }
        public decimal MnthRV { get; set; }
        public decimal MnthFVAmt { get; set; }
        public decimal MnthRVAmt { get; set; }
        public decimal TodaySale { get; set; }
        public decimal TodayFV { get; set; }
        public decimal TodayRV { get; set; }
        public decimal TodayFVAmt { get; set; }
        public decimal TodayRVAmt { get; set; }
        public decimal TodayBillCnt { get; set; }
        public decimal TodayFVBillCnt { get; set; }
        public decimal TodayRVBillCnt { get; set; }
        public decimal StockVal { get; set; }
        public decimal StockQty { get; set; }
        public decimal WRVal { get; set; }
        public decimal WRQty { get; set; }
        public decimal FRVal { get; set; }
        public decimal FRQty { get; set; }
        public decimal LastMnthFVBillCnt { get; set; }
        public decimal LastMnthRVBillCnt { get; set; }
        public decimal LastMnthTDFVCnt { get; set; }
        public decimal LastMnthTDRVCnt { get; set; }
        public decimal MnthFVCnt { get; set; }
        public decimal MnthRVCnt { get; set; }
        public decimal TotalFVCnt { get; set; }
        public decimal TotalRVCnt { get; set; }

    }
}