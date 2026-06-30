using InventoryManagement.API.Controllers;
using InventoryManagement.DataAccess.Contract;
using InventoryManagement.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static InventoryManagement.Entity.Common.StockReportModel;

namespace InventoryManagement.DataAccess
{
    public class ReportRepository: IReportRepository
    {
        ReportAPIController objReportAPI = new ReportAPIController();
        public List<ProductDetails> GetAllProducts(decimal CategoryCode)
        {
            return (objReportAPI.GetAllProducts(CategoryCode));
        }
        public List<PartyModel> GetAllParty(bool WithMobileShoppee)
        {
            return (objReportAPI.GetAllParty(WithMobileShoppee));
        }
        public List<StockReportModel> GetStockReport(string CategoryCode, string ProductCode, string PartyCode, bool IsBatchWise, string StockType)
        {
            return (objReportAPI.GetStockReport(CategoryCode,ProductCode,PartyCode,IsBatchWise,StockType));
        }
        public List<SalesReport> GetSalesReport(string FromDate, string ToDate, string CustomerId, string ProductCode, string CategoryCode, string PartyCode, string BType, string SalesType, string InvoiceType , string BillNo, string FType,string OfferType,string ReportType)
        {
            return (objReportAPI.GetSalesReport(FromDate, ToDate, CustomerId, ProductCode, CategoryCode, PartyCode, BType, SalesType,InvoiceType, BillNo, FType, OfferType,ReportType));
        }
        public List<StockJv> GetStockJvReport(string FromDate, string ToDate, string PartyCode,string ViewType)
        {
            return (objReportAPI.GetStockJvReport(FromDate, ToDate, PartyCode, ViewType));
        }
        public List<PurchaseReport> GetPurchaseSummary(string FromDate, string ToDate, string PartyCode, string SupplierCode, string ReportType,string InvoiceNo)
        {
            return (objReportAPI.GetPurchaseSummary(FromDate,ToDate,PartyCode,SupplierCode,ReportType, InvoiceNo));
        }
       
        public List<PurchaseReport> GetPurchaseDetailSummary(string FromDate, string ToDate, string PartyCode, string SupplierCode, string ProductCode)
        {
            return (objReportAPI.GetPurchaseDetailSummary(FromDate,ToDate,PartyCode,SupplierCode,ProductCode));
        }
        public List<string> GetYearList()
        {
            return (objReportAPI.GetYearList());
        }
       public  List<PurchaseReport> GetMonthWisePurchaseSummary(string Year, bool IsQuantity, bool IsAmount, string PartyCode, string SupplierCode)
        {
            return (objReportAPI.GetMonthWisePurchaseSummary(Year,IsQuantity,IsAmount,PartyCode,SupplierCode));
        }
       public List<string> GetSalesYearList()
       {
            return (objReportAPI.GetSalesYearList());
        }
       public List<SalesReport> GetMonthWiseSalesSummary(string Year, bool IsQuantity, bool IsAmount, string PartyCode)
        {
            return (objReportAPI.GetMonthWiseSalesSummary(Year, IsQuantity, IsAmount, PartyCode));
        }
        public List<StockReportModel> GetStockReceiptReport(string CategoryCode, string ProductCode, string PartyCode, string StateCode, string FromDate, string ToDate, string LoginPartyCode, string isSummary)
        {
            return (objReportAPI.GetStockReceiptReport(CategoryCode,ProductCode,PartyCode,StateCode,FromDate,ToDate,LoginPartyCode,isSummary));
        }
        public List<SelectListItem> GetStateList()
        {
            return (objReportAPI.GetStateList());
        }
        public List<PartyWiseWalletDetails> GetPartyWiseWalletReport(string FromDate, string ToDate, string PartyCode, string ViewType)
        {
            return (objReportAPI.GetPartyWiseWalletReport(FromDate, ToDate, PartyCode, ViewType));
        }
        public List<PaymentSummaryReport> GetPaymentSummaryReport(string FromDate, string ToDate, string PartyCode, string Type)
        {
            return (objReportAPI.GetPaymentSummaryReport(FromDate, ToDate, PartyCode, Type));
        }

        public List<SaleRegister> GetSaleRegisterReport(string FromDate, string ToDate, string PartyCode)
        {
            return (objReportAPI.GetSaleRegisterReport(FromDate, ToDate, PartyCode));
        }

        public List<SaleRegister> GetProductSaleRegisterReport(string FromDate, string ToDate, string PartyCode)
        {
            return (objReportAPI.GetProductSaleRegisterReport(FromDate, ToDate, PartyCode));
        }
        
        public List<PaymentMode> GetPaymodeList()
        {
            return (objReportAPI.GetPaymodeList());
        }

        public List<SalesReturnReport> GetSalesReturnReport(string FromDate, string ToDate, string ProductCode, string CategoryCode, string PartyCode, string PartyType, string Type)
        {
            return (objReportAPI.GetSalesReturnReport(FromDate, ToDate, ProductCode, CategoryCode, PartyCode, PartyType, Type));
        }
        public List<SalesReturnReport> GetPurchaseReturnReport(string FromDate, string ToDate, string ProductCode, string CategoryCode, string SupplierCode, string Type)
        {
            return (objReportAPI.GetPurchaseReturnReport(FromDate, ToDate, ProductCode, CategoryCode, SupplierCode, Type));
        }
        public List<MonthlySumm> GetMonthlyReport(string PartyCode, string BillType)
        {
            return (objReportAPI.GetMonthlyReport(PartyCode,  BillType));
        }
        public List<OfferReport> GetOfferReport(string PartyCode)
        {
            return (objReportAPI.GetOfferReport(PartyCode));
        }
        public List<OfferReport> GetBillWiseOfferReport(decimal OfferID, string SoldBy)
        {
            return (objReportAPI.GetBillWiseOfferReport(OfferID, SoldBy));
        }
        public List<OfferReport> GetProdWiseOfferReport(decimal OfferID, string PartyCode)
        {
            return (objReportAPI.GetProdWiseOfferReport(OfferID,PartyCode));
        }
        public List<StockReportModel> GetDateWiseStockReport(string CategoryCode, string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            return (objReportAPI.GetDateWiseStockReport(CategoryCode, ProductCode, PartyCode, FromDate, ToDate));
        }

        public List<StockReportModel> GetDailyStockReport(string CategoryCode, string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            return (objReportAPI.GetDailyStockReport(CategoryCode, ProductCode, PartyCode, FromDate, ToDate));
        }
        public List<StockReportModel> GetDailyFrStockReport(string CategoryCode, string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            return (objReportAPI.GetDailyFrStockReport(CategoryCode, ProductCode, PartyCode, FromDate, ToDate));
        }
        public List<IssueSampleProduct> GetSampleProductReport(string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            return (objReportAPI.GetSampleProductReport(ProductCode, PartyCode, FromDate, ToDate));
        }

        public List<Dashboard> GetShoppeReport(string PartyCode)
        {
            return (objReportAPI.GetShoppeReport(PartyCode));
        }

        public List<DashboardColumn> getDasboardColumns()
        {
            return (objReportAPI.getDasboardColumns());
        }

        public string GetSRProductList(string STRNo)
        {
            return (objReportAPI.GetSRProductList(STRNo));
        }
        public string GetSampleProductList(string STRNo)
        {
            return (objReportAPI.GetSampleProductList(STRNo));
        }

        public List<GVCreditNote> GetGVCreditNote(string PartyCode, string FromDate, string ToDate)
        {
            return (objReportAPI.GetGVCreditNote(PartyCode, FromDate, ToDate));
        }

        public List<ConsistentOffers> GetConsistentFPVOffer(string IdNo, bool TeamWise)
        {
            return (objReportAPI.GetConsistentFPVOffer(IdNo,TeamWise));
        }

        public string GetMonthlyConsistentFPVOffer(string IdNo,bool TeamWise)
        {
            return (objReportAPI.GetMonthlyConsistentFPVOffer(IdNo, TeamWise));
        }

        public List<SalesReport> GetWalletHistory(string FromDate, string ToDate, string PartyCode,string vtype)
        {
            return (objReportAPI.GetWalletHistory(FromDate, ToDate, PartyCode, vtype));
        }

        public string GetShoppeStockReport(string PartyCode,string ProductCode, int Month, int year)
        {
            return (objReportAPI.GetShoppeStockReport(PartyCode, ProductCode, Month, year));
        }
        public List<GVCreditNote> GetMRICouponReport(string PartyCode, string StartDate, string EndDate)
        {
            return objReportAPI.GetMRICreditNote(PartyCode, StartDate, EndDate);
        }
        public string GetSJPConsistentReport(string IdNo)
        {
            return objReportAPI.GetSJPConsistentReport(IdNo);
        }
        public string GetFPVConsitencyReport(string IdNo)
        {
            return (objReportAPI.GetFPVConsitencyReport(IdNo));
        }
        public List<FranchiseeCommission> GetFranchiseeBVCommission(string FromDate, string ToDate, string code, string Billtype)
        {
            return (objReportAPI.GetFranchiseeBVCommission(FromDate, ToDate, code, Billtype));
        }
        public List<MSessids> GetSessids()
        {
            return objReportAPI.GetSessids();
        }
        public List<MonthWiseIncome> GetMonthWiseIncome(string Sessid, string PartyCode)
        {
            return objReportAPI.GetMonthWiseIncome(Sessid, PartyCode);
        }
        public List<MPerformanceInc> GetPerformanceInc(string Partycode, string Action, int SessID)
        {
            return objReportAPI.GetPerformanceInc(Partycode, Action, SessID);
        }
        public M_IncentiveStatement GetIncentiveStatement(string Partycode, string StatementPeriod)
        {
            return objReportAPI.GetIncentiveStatement(Partycode, StatementPeriod);
        }
    }
}