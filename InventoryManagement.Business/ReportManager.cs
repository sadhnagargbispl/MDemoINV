using InventoryManagement.API.Models;
using InventoryManagement.Business.Contract;
using InventoryManagement.DataAccess;
using InventoryManagement.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static InventoryManagement.Entity.Common.StockReportModel;

namespace InventoryManagement.Business
{
    public class ReportManager : IReportManager
    {
        ReportRepository objReportRepo = new ReportRepository();
        public List<ProductDetails> GetAllProducts(decimal CategoryCode)
        {
            return (objReportRepo.GetAllProducts(CategoryCode));
        }
        public List<PartyModel> GetAllParty(bool WithMobileShoppee)
        {
            return (objReportRepo.GetAllParty(WithMobileShoppee));
        }
        public List<StockReportModel> GetStockReport(string CategoryCode, string ProductCode, string PartyCode, bool IsBatchWise, string StockType)
        {
            return (objReportRepo.GetStockReport(CategoryCode, ProductCode, PartyCode, IsBatchWise, StockType));
        }
        public List<SalesReport> GetSalesReport(string FromDate, string ToDate, string CustomerId, string ProductCode, string CategoryCode, string PartyCode, string BType, string SalesType, string InvoiceType, string BillNo, string FType, string OfferType, string ReportType)
        {
            return (objReportRepo.GetSalesReport(FromDate, ToDate, CustomerId, ProductCode, CategoryCode, PartyCode, BType, SalesType, InvoiceType, BillNo, FType, OfferType, ReportType));
        }
        public List<StockJv> GetStockJvReport(string FromDate, string ToDate, string PartyCode, string ViewType)
        {
            return (objReportRepo.GetStockJvReport(FromDate, ToDate, PartyCode, ViewType));
        }
        public List<PurchaseReport> GetPurchaseSummary(string FromDate, string ToDate, string PartyCode, string SupplierCode, string ReportType, string InvoiceNo)
        {
            return (objReportRepo.GetPurchaseSummary(FromDate, ToDate, PartyCode, SupplierCode, ReportType, InvoiceNo));
        }

        public List<PurchaseReport> GetPurchaseDetailSummary(string FromDate, string ToDate, string PartyCode, string SupplierCode, string ProductCode)
        {
            return (objReportRepo.GetPurchaseDetailSummary(FromDate, ToDate, PartyCode, SupplierCode, ProductCode));
        }
        public List<string> GetYearList()
        {
            return (objReportRepo.GetYearList());
        }
        public List<PurchaseReport> GetMonthWisePurchaseSummary(string Year, bool IsQuantity, bool IsAmount, string PartyCode, string SupplierCode)
        {
            return (objReportRepo.GetMonthWisePurchaseSummary(Year, IsQuantity, IsAmount, PartyCode, SupplierCode));
        }
        public List<string> GetSalesYearList()
        {
            return (objReportRepo.GetSalesYearList());
        }
        public List<SalesReport> GetMonthWiseSalesSummary(string Year, bool IsQuantity, bool IsAmount, string PartyCode)
        {
            return (objReportRepo.GetMonthWiseSalesSummary(Year, IsQuantity, IsAmount, PartyCode));
        }
        public List<StockReportModel> GetStockReceiptReport(string CategoryCode, string ProductCode, string PartyCode, string StateCode, string FromDate, string ToDate, string LoginPartyCode, string isSummary)
        {
            return (objReportRepo.GetStockReceiptReport(CategoryCode, ProductCode, PartyCode, StateCode, FromDate, ToDate, LoginPartyCode, isSummary));
        }
        public List<SelectListItem> GetStateList()
        {
            return (objReportRepo.GetStateList());
        }
        public List<PartyWiseWalletDetails> GetPartyWiseWalletReport(string FromDate, string ToDate, string PartyCode, string ViewType)
        {
            return (objReportRepo.GetPartyWiseWalletReport(FromDate, ToDate, PartyCode, ViewType));
        }
        public List<PaymentSummaryReport> GetPaymentSummaryReport(string FromDate, string ToDate, string PartyCode, string Type)
        {
            return (objReportRepo.GetPaymentSummaryReport(FromDate, ToDate, PartyCode, Type));
        }

        public List<SaleRegister> GetSaleRegisterReport(string FromDate, string ToDate, string PartyCode)
        {
            return (objReportRepo.GetSaleRegisterReport(FromDate, ToDate, PartyCode));
        }
        public List<SaleRegister> GetProductSaleRegisterReport(string FromDate, string ToDate, string PartyCode)
        {
            return (objReportRepo.GetProductSaleRegisterReport(FromDate, ToDate, PartyCode));
        }

        public List<PaymentMode> GetPaymodeList()
        {
            return (objReportRepo.GetPaymodeList());


        }

        public List<SalesReturnReport> GetSalesReturnReport(string FromDate, string ToDate, string ProductCode, string CategoryCode, string PartyCode, string PartyType, string Type)
        {
            return (objReportRepo.GetSalesReturnReport(FromDate, ToDate, ProductCode, CategoryCode, PartyCode, PartyType, Type));
        }

        public List<SalesReturnReport> GetPurchaseReturnReport(string FromDate, string ToDate, string ProductCode, string CategoryCode, string SupplierCode, string Type)
        {
            return (objReportRepo.GetPurchaseReturnReport(FromDate, ToDate, ProductCode, CategoryCode, SupplierCode, Type));
        }
        public List<MonthlySumm> GetMonthlyReport(string PartyCode, string BillType)
        {
            return (objReportRepo.GetMonthlyReport(PartyCode, BillType));
        }

        public List<OfferReport> GetOfferReport(string PartyCode)
        {
            return (objReportRepo.GetOfferReport(PartyCode));
        }
        public List<OfferReport> GetBillWiseOfferReport(decimal OfferID, string SoldBy)
        {
            return (objReportRepo.GetBillWiseOfferReport(OfferID, SoldBy));
        }
        public List<OfferReport> GetProdWiseOfferReport(decimal OfferID, string PartyCode)
        {
            return (objReportRepo.GetProdWiseOfferReport(OfferID, PartyCode));
        }
        public List<StockReportModel> GetDateWiseStockReport(string CategoryCode, string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            return (objReportRepo.GetDateWiseStockReport(CategoryCode, ProductCode, PartyCode, FromDate, ToDate));
        }

        public List<StockReportModel> GetDailyStockReport(string CategoryCode, string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            return (objReportRepo.GetDailyStockReport(CategoryCode, ProductCode, PartyCode, FromDate, ToDate));
        }

        public List<StockReportModel> GetDailyFrStockReport(string CategoryCode, string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            return (objReportRepo.GetDailyFrStockReport(CategoryCode, ProductCode, PartyCode, FromDate, ToDate));
        }
        public List<IssueSampleProduct> GetSampleProductReport(string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            return (objReportRepo.GetSampleProductReport(ProductCode, PartyCode, FromDate, ToDate));
        }

        public List<Dashboard> GetShoppeReport(string PartyCode)
        {
            return (objReportRepo.GetShoppeReport(PartyCode));
        }

        public List<DashboardColumn> getDasboardColumns()
        {
            return (objReportRepo.getDasboardColumns());
        }

        public string GetSRProductList(string STRNo)
        {
            return (objReportRepo.GetSRProductList(STRNo));
        }
        public string GetSampleProductList(string STRNo)
        {
            return (objReportRepo.GetSampleProductList(STRNo));
        }

        public List<GVCreditNote> GetGVCreditNote(string PartyCode, string FromDate, string ToDate)
        {
            return (objReportRepo.GetGVCreditNote(PartyCode, FromDate, ToDate));
        }

        public List<ConsistentOffers> GetConsistentFPVOffer(string IdNo, bool TeamWise)
        {
            return (objReportRepo.GetConsistentFPVOffer(IdNo, TeamWise));
        }

        public string GetMonthlyConsistentFPVOffer(string IdNo, bool TeamWise)
        {
            return (objReportRepo.GetMonthlyConsistentFPVOffer(IdNo, TeamWise));
        }

        public List<SalesReport> GetWalletHistory(string FromDate, string ToDate, string PartyCode, string vtype)
        {
            return (objReportRepo.GetWalletHistory(FromDate, ToDate, PartyCode, vtype));
        }

        public string GetShoppeStockReport(string PartyCode, string ProductCode, int Month, int year)
        { return (objReportRepo.GetShoppeStockReport(PartyCode, ProductCode, Month, year)); }

        public List<GVCreditNote> GetMRICouponReport(string PartyCode, string StartDate, string EndDate)
        {
            return objReportRepo.GetMRICouponReport(PartyCode, StartDate, EndDate);
        }

        public string GetSJPConsistentReport(string IdNo)
        {
            return (objReportRepo.GetSJPConsistentReport(IdNo));
        }

        public string GetFPVConsitencyReport(string IdNo)
        {
            return (objReportRepo.GetFPVConsitencyReport(IdNo));
        }
        public List<FranchiseeCommission> GetFranchiseeBVCommission(string FromDate, string ToDate, string code, string Billtype)
        {
            return (objReportRepo.GetFranchiseeBVCommission(FromDate, ToDate, code, Billtype));
        }

        public List<MSessids> GetSessids()
        {
            return objReportRepo.GetSessids();
        }
        public List<MonthWiseIncome> GetMonthWiseIncome(string Sessid, string PartyCode)
        {
            return objReportRepo.GetMonthWiseIncome(Sessid, PartyCode);
        }

        public List<MPerformanceInc> GetPerformanceInc(string Partycode, string Action,int SessID)
        {
            return objReportRepo.GetPerformanceInc(Partycode, Action, SessID);
        }
        public M_IncentiveStatement GetIncentiveStatement(string Partycode, string StatementPeriod)
        {
            return objReportRepo.GetIncentiveStatement(Partycode, StatementPeriod);
        }
    }
}