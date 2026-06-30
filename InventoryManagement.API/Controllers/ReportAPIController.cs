using InventoryManagement.API.Models;
using InventoryManagement.Entity.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using static InventoryManagement.Entity.Common.StockReportModel;

namespace InventoryManagement.API.Controllers
{
    public class ReportAPIController : ApiController
    {
        public List<ProductDetails> GetAllProducts(decimal CategoryCode)
        {
            List<ProductDetails> objProduct = new List<ProductDetails>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    if (CategoryCode == 0)
                    {
                        objProduct = (from product in entity.M_ProductMaster
                                      where product.ActiveStatus == "Y"
                                      select new ProductDetails
                                      {
                                          CategoryId = (int)product.CatId,
                                          ProductCodeStr = product.ProdId,
                                          ProductName = product.ProductName
                                      }

                                    ).ToList();
                    }
                    else
                    {
                        objProduct = (from product in entity.M_ProductMaster
                                      where product.ActiveStatus == "Y" && product.CatId == CategoryCode
                                      select new ProductDetails
                                      {
                                          CategoryId = (int)product.CatId,
                                          ProductCodeStr = product.ProdId,
                                          ProductName = product.ProductName
                                      }

                                    ).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objProduct;
        }

        public List<PartyModel> GetAllParty(bool WithMobileShoppee)
        {
            List<PartyModel> objpartyList = new List<PartyModel>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objpartyList = (from party in entity.M_LedgerMaster
                                    where party.ActiveStatus == "Y" && party.GroupId != 5 && party.ISApprove == "Y"
                                    orderby party.PartyName
                                    select new PartyModel
                                    {
                                        PartyCode = party.PartyCode,
                                        PartyName = party.PartyName,
                                        GroupId = party.GroupId
                                    }
                                 ).ToList();
                    if (WithMobileShoppee)
                    {
                        List<PartyModel> objMobileShoppe = (from party in entity.ShoppeAssignments
                                                            select new PartyModel
                                                            {
                                                                PartyCode = party.PCId,
                                                                PartyName = party.PCName
                                                            }
                                     ).ToList();

                        objpartyList = objpartyList.Concat(objMobileShoppe).ToList();

                    }
                }
            }
            catch (Exception ex)
            {

            }

            return objpartyList;
        }


        public List<StockReportModel> GetStockReport(string CategoryCode, string ProductCode, string PartyCode, bool IsBatchWise, string StockType)
        {
            List<StockReportModel> objStockModel = new List<StockReportModel>();
            decimal CatId = 0;
            decimal ProdCode = 0;
            try
            {
                if (!string.IsNullOrEmpty(CategoryCode))
                {
                    CatId = decimal.Parse(CategoryCode);
                }
                if (!string.IsNullOrEmpty(ProductCode))
                {
                    ProdCode = decimal.Parse(ProductCode);
                }
                using (var entity = new InventoryEntities())
                {
                    if (IsBatchWise)
                    {
                        objStockModel = (from r in entity.V_CurrentStockDetailNotForStockistNew
                                         orderby r.CatName, r.ProductName, r.BatchCode
                                         select new StockReportModel
                                         {
                                             PartyCode = r.PartyCode,
                                             PartyName = r.PartyName,
                                             MRP = r.MRP.ToString(),
                                             RateOrDP = r.DP.ToString(),
                                             Qty = r.Qty.ToString(),
                                             Category = r.CatName,
                                             DPStockValue = r.DPStockValue.ToString(),
                                             StockValue = r.StockValue.ToString(),
                                             ProductCode = r.ProdId,
                                             ProductName = r.ProductName,
                                             MRPSTockValue = r.MRPStockValue.ToString(),
                                             // Expired = r.IsExpired == "Y" ? "Yes" : "No",
                                             MfgDate = r.MfgDate.ToString(),
                                             ExpDate = r.ExpDate.ToString(),
                                             ExpDateD = r.ExpDate,
                                             MfgDateD = r.MfgDate,
                                             BatchNo = r.BatchCode,
                                             InStock = r.StockIn ?? 0,
                                             OutStock = r.StockOut,
                                             Barcode = r.Barcode,
                                             //openingStock = r.StockIn,
                                             Quantity = r.Qty,//?? 0,
                                             CatCode = r.CatId,
                                             BarcodeStatus = r.status,
                                             PurchaseRate = r.PurchaseRate,
                                             PV = r.PV,
                                             BV = r.BV

                                         }

                                               ).ToList();

                    }
                    else
                    {
                        objStockModel = (from r in entity.V_CurrentStockDetailNotForStockistNewWithoutBatch
                                         orderby r.CatName, r.ProductName
                                         select new StockReportModel
                                         {
                                             PartyCode = r.PartyCode,
                                             PartyName = r.PartyName,
                                             MRP = r.MRP.ToString(),
                                             RateOrDP = r.DP.ToString(),
                                             Qty = r.Qty.ToString(),
                                             Category = r.CatName,
                                             DPStockValue = r.DPStockValue.ToString(),
                                             StockValue = r.StockValue.ToString(),
                                             ProductCode = r.ProdId,
                                             ProductName = r.ProductName,
                                             MRPSTockValue = r.MRPStockValue.ToString(),
                                             Expired = "",
                                             MfgDate = "",
                                             ExpDate = "",
                                             BatchNo = "",
                                             Barcode = r.Barcode,
                                             //openingStock = r.StockIn,
                                             InStock = r.StockIn ?? 0,
                                             OutStock = r.StockOut,
                                             Quantity = r.Qty,// ?? 0,
                                             CatCode = r.CatId,
                                             BarcodeStatus = r.status,
                                             PurchaseRate = r.PurchaseRate,
                                             PV = r.PV,
                                             BV = r.BV
                                         }).ToList();
                        objStockModel = objStockModel.Where(r => r.BarcodeStatus == "Y").ToList();
                    }
                    if (StockType == "FinishStock")
                    {
                        objStockModel = objStockModel.Where(r => r.Quantity <= 0).ToList();
                    }
                    else
                    {
                        objStockModel = objStockModel.Where(r => r.Quantity > 0).ToList();
                    }

                    if (CatId != 0)
                    {
                        objStockModel = objStockModel.Where(r => r.CatCode == CatId).ToList();
                    }
                    if (ProdCode != 0)
                    {
                        objStockModel = objStockModel.Where(r => r.ProductCode == ProductCode).ToList();
                    }
                    if (PartyCode != "0" && PartyCode != "All")
                    {
                        objStockModel = objStockModel.Where(r => r.PartyCode == PartyCode).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objStockModel;
        }

        public List<SalesReport> GetSalesReport(string FromDate, string ToDate, string CustomerId, string ProductCode, string CategoryCode, string PartyCode, string BType, string SalesType, string InvoiceType, string BillNo, string FType, string OfferType, string ReportType)
        {
            List<SalesReport> objListSales = new List<SalesReport>();
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;
            string forDelete = SalesType;
            if (forDelete == "DelBillWise")
                SalesType = "BillWise";
            else
                forDelete = "";

            try
            {
                using (var entity = new InventoryEntities())
                {
                    // string sqlFromdate = "", sqlToDate = "";
                    if (!string.IsNullOrEmpty(FromDate) && (!string.IsNullOrEmpty(ToDate)))
                    {
                        if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                        {
                            var SplitDate = FromDate.Split('-');
                            string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];
                            StartDate = Convert.ToDateTime(NewDate);
                            StartDate = StartDate.Date;
                        }
                        if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                        {
                            var SplitDate = ToDate.Split('-');
                            string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];
                            EndDate = Convert.ToDateTime(NewDate);
                            EndDate = EndDate.Date;
                        }



                    }
                    if (FromDate.ToLower() == "all" || string.IsNullOrEmpty(FromDate))
                        StartDate = DateTime.Now.AddYears(-5);
                    if (ToDate.ToLower() == "all" || string.IsNullOrEmpty(ToDate))
                        EndDate = DateTime.Now;
                    switch (SalesType)
                    {
                        case "":
                            break;
                        case "BillWise":
                            List<SalesReport> objSales = new List<SalesReport>();
                            objSales = (from result in entity.V_BillWiseSaleSummary
                                            //where result.BType != BType
                                        where result.BillDate >= StartDate.Date && result.BillDate <= EndDate.Date
                                        group result by new
                                        {
                                            result.RefIDNo,
                                            result.BType,
                                            result.BVValue,
                                            result.Username,
                                            result.UserBillNo,
                                            result.CityName,
                                            result.TotalQty,
                                            result.SBillNo,
                                            result.BillNo,
                                            result.BillDate,
                                            result.BillDateStr,
                                            result.FType,
                                            result.PartyCode,
                                            result.PartyName,
                                            result.MobileNo,
                                            result.FCode,
                                            result.Name,
                                            result.OrderNo,
                                            result.FSessId,
                                            result.OrderDate,
                                            result.BillType,
                                            result.SGSTAmt,
                                            result.CGSTAmt,
                                            result.IGSTAmt,
                                            result.CanDelete,
                                            result.OfferUID,
                                            result.Discount,
                                            result.OrderMethod,
                                            result.PVValue
                                        }
                                                into BillResult
                                        orderby BillResult.Key.BillDate descending, BillResult.Key.SBillNo descending, BillResult.Key.PartyCode, BillResult.Key.FType
                                        select new SalesReport
                                        {
                                            RefIDNo = BillResult.Key.RefIDNo,
                                            BillType = BillResult.Key.BType,
                                            BillNo = BillResult.Key.UserBillNo,
                                            MobileNO = BillResult.Key.MobileNo.ToString(),
                                            InternalBillNo = BillResult.Key.BillNo,
                                            InternalsBillNo = BillResult.Key.SBillNo,
                                            BillDate = BillResult.Key.BillDate,
                                            StrBillDate = BillResult.Key.BillDateStr,
                                            PartyName = BillResult.Key.PartyName,
                                            PartyCode = BillResult.Key.PartyCode,
                                            CustCode = BillResult.Key.FCode,
                                            CustName = BillResult.Key.Name,
                                            Amount = BillResult.Sum(m => m.Amount).ToString(),
                                            NetAmount = BillResult.Sum(m => m.NetPayable).ToString(),
                                            CGSTAmount = BillResult.Sum(m => m.CGSTAmt).ToString(),
                                            SGSTAmount = BillResult.Sum(m => m.SGSTAmt).ToString(),
                                            IGSTAmount = BillResult.Sum(m => m.IGSTAmt).ToString(),
                                            TotalQty = BillResult.Key.TotalQty.ToString(),
                                            City = BillResult.Key.CityName,
                                            UserName = BillResult.Key.Username,
                                            TotalFreeQty = BillResult.Sum(m => m.FreeQty),
                                            TotalBV = BillResult.Key.BVValue.ToString(),
                                            TotalPV = BillResult.Key.PVValue.ToString(),
                                            //TaxAmount = BillResult.Sum(m => m.TaxAmount).ToString()
                                            FsessId = BillResult.Key.FSessId,
                                            IsDelete = false,
                                            Reason = "",
                                            UserId = 0,
                                            InvoiceType = BillResult.Key.BillType,
                                            FType = BillResult.Key.FType,
                                            CanDelete = BillResult.Key.CanDelete,
                                            OfferUID = BillResult.Key.OfferUID,
                                            DiscAmt = BillResult.Key.Discount.ToString(),
                                            GrossAmt = BillResult.Sum(m => m.NetAmt).ToString(),
                                            OrderNo = BillResult.Key.OrderMethod
                                        }).ToList();

                            if (FType != null && FType != "" && FType != "A")
                            {
                                if (FType != "M" && FType != "W")
                                {
                                    objSales = objSales.Where(m => m.FType != "M").ToList();
                                    objSales = objSales.Where(m => m.FType != "W").ToList();
                                }
                                else
                                    objSales = objSales.Where(m => m.FType.ToUpper() == FType.ToUpper()).ToList();
                            }

                            if (forDelete != "") //Added on 09Feb19
                                objSales = objSales.Where(m => m.CanDelete.ToUpper() == "Y").ToList();

                            if (OfferType == "F")
                                objSales = objSales.Where(m => m.OfferUID > 0).ToList();
                            else if (OfferType == "W")
                                objSales = objSales.Where(m => m.OfferUID == 0).ToList();

                            if (CustomerId != "All")
                            {
                                objSales = objSales.Where(m => m.CustCode.ToUpper() == CustomerId.ToUpper()).ToList();
                            }
                            if (PartyCode != "All" && PartyCode != "0")
                            {
                                objSales = objSales.Where(m => m.PartyCode.ToUpper() == PartyCode.ToUpper()).ToList();
                            }
                            if (InvoiceType != "")
                            {
                                if (InvoiceType == "FPV")
                                {
                                    objSales = objSales.Where(m => m.BillType == "F").ToList();
                                }
                                else if (InvoiceType == "P")
                                {
                                    objSales = objSales.Where(m => m.BillType == "P").ToList();
                                }
                                else if (InvoiceType == "G")
                                {
                                    objSales = objSales.Where(m => m.BillType == "G").ToList();
                                }
                                else if (InvoiceType == "GC")
                                {
                                    objSales = objSales.Where(m => m.BillType == "GC").ToList();
                                }
                                else if (InvoiceType == "RI")
                                {
                                    objSales = objSales.Where(m => m.BillType != "S"
                                    && m.BillType != "B"
                                    && m.BillType != "F"
                                    && m.BillType != "G"
                                    && m.BillType != "GC"
                                    && m.BillType != "C"
                                    && m.BillType != "J"
                                    && m.BillType != "X" && m.BillType != "P").ToList();
                                }
                                else if (InvoiceType == "JI")
                                {
                                    objSales = objSales.Where(m => m.BillType == "B").ToList();
                                }
                                else if (InvoiceType == "S")
                                {
                                    objSales = objSales.Where(m => m.BillType == "S").ToList();
                                }
                                else if (InvoiceType == "C")
                                {
                                    objSales = objSales.Where(m => m.BillType == "C").ToList();
                                }
                                else if (InvoiceType == "J")
                                {
                                    objSales = objSales.Where(m => m.BillType == "J").ToList();
                                }
                                else if (InvoiceType == "X")
                                {
                                    objSales = objSales.Where(m => m.BillType == "X").ToList();
                                }
                            }
                            //if (FromDate != "All" && ToDate != "All")
                            //{
                            //    foreach (var obj in objSales)
                            //    {
                            //        if (obj.BillDate >= StartDate.Date && obj.BillDate <= EndDate.Date)
                            //        {
                            //            objListSales.Add(obj);
                            //        }
                            //    }
                            //}
                            //else if (FromDate == "All" && ToDate != "All")
                            //{
                            //    foreach (var obj in objSales)
                            //    {
                            //        if (obj.BillDate <= EndDate.Date)
                            //        {
                            //            objListSales.Add(obj);
                            //        }
                            //    }

                            //}
                            //else if (FromDate != "All" && ToDate == "All")
                            //{
                            //    foreach (var obj in objSales)
                            //    {
                            //        if (obj.BillDate >= StartDate.Date)
                            //        {
                            //            objListSales.Add(obj);
                            //        }
                            //    }

                            //}
                            //else 
                            if (!string.IsNullOrEmpty(BillNo) && BillNo != "0" && BillNo != "All")
                            {

                                objListSales = objSales.Where(m => m.BillNo.ToUpper() == BillNo.ToUpper()).ToList();

                            }
                            else
                            {
                                foreach (var obj in objSales)
                                {
                                    objListSales.Add(obj);
                                }
                            }

                            break;
                        case "DateWise":
                            objSales = new List<SalesReport>();
                            objSales = (from m in (from r in entity.TrnBillMains
                                                   where r.BillDate >= StartDate.Date && r.BillDate <= EndDate.Date
                                                   //where r.SoldBy== PartyCode
                                                   from t in
                    (from p in entity.TrnPayModeDetails
                     where p.PayPrefix == "W" && p.BillNo == r.BillNo
                     //group p by new { p.BillNo, r.UserBillNo } into g
                     select new
                     {
                         WalletpaidAmount = p.Amount,
                         BillNo = r.UserBillNo
                     }
                    ).DefaultIfEmpty()

                                                   select new { r, t })

                                        group m by EntityFunctions.TruncateTime(m.r.BillDate)
                                               into grouped
                                        orderby new { grouped.Key.Value }
                                        select new SalesReport
                                        {
                                            TotalBV = grouped.Sum(m => m.r.BvValue).ToString(),
                                            TotalQty = grouped.Sum(m => m.r.TotalQty).ToString(),
                                            TotalBillAmt = grouped.Sum(m => m.r.NetPayable).ToString(),
                                            RecordDate = grouped.Key.Value,
                                            NoOfBills = grouped.Count().ToString(),
                                            TotalAmount = grouped.Sum(m => m.r.Amount).ToString(),
                                            TotalTaxAmount = grouped.Sum(m => (m.r.TaxAmount + m.r.CGSTAmt + m.r.STaxAmount)).ToString(),
                                            TotalPaidByWallet = grouped.Sum(m => m.t.WalletpaidAmount).ToString()
                                        }
                                               ).ToList();
                            if (PartyCode != "All" && PartyCode != "0")
                            {
                                objSales = objSales.Where(m => m.SoldBy.ToUpper() == PartyCode.ToUpper()).ToList();
                            }

                            foreach (var obj in objSales)
                            {
                                objListSales.Add(obj);
                            }

                            break;
                        case "ProductWise":
                            decimal CatCode = 0;
                            objSales = new List<SalesReport>();
                            if (!string.IsNullOrEmpty(CategoryCode))
                            {
                                CatCode = decimal.Parse(CategoryCode);
                            }
                            if (ReportType.ToLower() == "summary")
                            {
                                objListSales = (from r in entity.TrnBillDetails
                                                join t in entity.TrnBillMains on r.BillNo equals t.BillNo
                                                join p in entity.M_ProductMaster on r.ProductId equals p.ProdId
                                                where r.BillDate >= StartDate.Date && r.BillDate <= EndDate.Date
                                                && ((CatCode != 0 && p.CatId == CatCode) || CatCode == 0)
                                                && ((ProductCode != "0" && p.ProdId == ProductCode) || ProductCode == "0")
                                                && ((PartyCode != "All" && PartyCode != "0" && r.SoldBy == PartyCode) || PartyCode == "All" || PartyCode == "0")
                                                group r by new { r.FCode, t.PartyName, r.FType, p.CatId, r.ProductId, r.ProductName, r.SoldBy, r.BillDate, r.Rate, r.DiscountPer, r.Discount, r.NetAmount, r.Tax, r.CGST, r.SGST, r.CGSTAmt, r.SGSTAmt, r.TaxAmount }
                                                    into g
                                                orderby g.Key.BillDate, g.Key.CatId, g.Key.ProductId
                                                select new SalesReport
                                                {
                                                    IdNo = g.Key.FCode,
                                                    Name = g.Key.PartyName,
                                                    ProdCode = g.Key.ProductId,
                                                    ProductName = g.Key.ProductName,
                                                    TaxPer = g.Key.Tax.ToString(),
                                                    TaxAmount = g.Key.TaxAmount.ToString(),
                                                    CGSTAmount = g.Key.CGSTAmt.ToString(),
                                                    SGSTAmount = g.Key.SGSTAmt.ToString(),
                                                    SGST = g.Key.SGST.ToString(),
                                                    CGST = g.Key.CGST.ToString(),
                                                    DiscPer = g.Key.DiscountPer.ToString(),
                                                    DiscAmt = g.Key.Discount.ToString(),
                                                    Rate = g.Key.Rate.ToString(),
                                                    BasicAmt = g.Key.NetAmount.ToString(),
                                                    Qty = g.Sum(m => m.Qty).ToString(),
                                                    TotalAmt = (g.Key.NetAmount + g.Key.TaxAmount + g.Key.CGSTAmt + g.Key.SGSTAmt).ToString(),
                                                    BillDate = g.Key.BillDate,
                                                    CatCode = g.Key.CatId,
                                                    PartyCode = g.Key.SoldBy,
                                                    FType = g.Key.FType,
                                                    UserName = g.Key.PartyName,//g.Key.UserName
                                                }).ToList();

                                objListSales = (from r in objListSales
                                                join user in entity.Inv_M_UserMaster on r.IdNo equals user.FCode
                                                into temp
                                                from j in temp.DefaultIfEmpty()
                                                select new SalesReport
                                                {
                                                    IdNo = r.IdNo,
                                                    Name = r.Name,
                                                    ProdCode = r.ProdCode,
                                                    BillNo = r.BillNo,
                                                    ProductName = r.ProductName,
                                                    TaxPer = r.TaxPer.ToString(),
                                                    TaxAmount = r.TaxAmount.ToString(),
                                                    CGSTAmount = r.CGSTAmount.ToString(),
                                                    SGSTAmount = r.SGSTAmount.ToString(),
                                                    SGST = r.SGST.ToString(),
                                                    CGST = r.CGST.ToString(),
                                                    DiscPer = r.DiscPer.ToString(),
                                                    DiscAmt = r.DiscAmt.ToString(),
                                                    Rate = r.Rate,
                                                    BasicAmt = r.NetAmount,
                                                    Qty = r.Qty,
                                                    TotalAmt = r.TotalAmt,
                                                    BillDate = r.BillDate,
                                                    CatCode = r.CatCode,
                                                    PartyCode = r.SoldBy,
                                                    FType = r.FType,
                                                    UserName = j.UserName
                                                }).ToList();

                            }
                            else
                            {
                                objListSales = (from r in entity.TrnBillDetails
                                                join t in entity.TrnBillMains on r.BillNo equals t.BillNo
                                                join p in entity.M_ProductMaster on r.ProductId equals p.ProdId
                                                where r.BillDate >= StartDate.Date && r.BillDate <= EndDate.Date
                                                && ((CatCode != 0 && p.CatId == CatCode) || CatCode == 0)
                                                && ((ProductCode != "0" && p.ProdId == ProductCode) || ProductCode == "0")
                                                && ((PartyCode != "All" && PartyCode != "0" && r.SoldBy == PartyCode) || PartyCode == "All" || PartyCode == "0")
                                                group r by new { t.UserBillNo, r.BillDate, r.FCode, t.PartyName, r.FType, p.CatId, r.ProductId, r.ProductName, r.SoldBy, r.Rate, r.DiscountPer, r.Discount, r.NetAmount, r.Tax, r.CGST, r.SGST, r.CGSTAmt, r.SGSTAmt, r.TaxAmount, t.BillType }
                                                into g
                                                orderby g.Key.BillDate, g.Key.UserBillNo, g.Key.CatId, g.Key.ProductId
                                                select new SalesReport
                                                {
                                                    IdNo = g.Key.FCode,
                                                    Name = g.Key.PartyName,
                                                    ProdCode = g.Key.ProductId,
                                                    ProductName = g.Key.ProductName,
                                                    TaxPer = g.Key.Tax.ToString(),
                                                    TaxAmount = g.Key.TaxAmount.ToString(),
                                                    CGSTAmount = g.Key.CGSTAmt.ToString(),
                                                    SGSTAmount = g.Key.SGSTAmt.ToString(),
                                                    SGST = g.Key.SGST.ToString(),
                                                    CGST = g.Key.CGST.ToString(),
                                                    DiscPer = g.Key.DiscountPer.ToString(),
                                                    DiscAmt = g.Key.Discount.ToString(),
                                                    Rate = g.Key.Rate.ToString(),
                                                    BasicAmt = g.Key.NetAmount.ToString(),
                                                    Qty = g.Sum(m => m.Qty).ToString(),
                                                    TotalAmt = (g.Key.NetAmount + g.Key.TaxAmount + g.Key.CGSTAmt + g.Key.SGSTAmt).ToString(),
                                                    BillDate = g.Key.BillDate,
                                                    BillNo = g.Key.UserBillNo,
                                                    CatCode = g.Key.CatId,
                                                    PartyCode = g.Key.SoldBy,
                                                    FType = g.Key.FType,
                                                    UserName = g.Key.PartyName,//g.Key.UserName
                                                    BillType = g.Key.BillType
                                                }).ToList();


                                objListSales = (from r in objListSales
                                                join billtype in entity.V_BillWiseSaleSummary on r.BillNo equals billtype.UserBillNo
                                                select new SalesReport
                                                {
                                                    IdNo = r.IdNo,
                                                    Name = r.Name,
                                                    ProdCode = r.ProdCode,
                                                    BillNo = r.BillNo,
                                                    ProductName = r.ProductName,
                                                    TaxPer = r.TaxPer.ToString(),
                                                    TaxAmount = r.TaxAmount.ToString(),
                                                    CGSTAmount = r.CGSTAmount.ToString(),
                                                    SGSTAmount = r.SGSTAmount.ToString(),
                                                    SGST = r.SGST.ToString(),
                                                    CGST = r.CGST.ToString(),
                                                    DiscPer = r.DiscPer.ToString(),
                                                    DiscAmt = r.DiscAmt.ToString(),
                                                    Rate = r.Rate,
                                                    BasicAmt = r.NetAmount.ToString(),
                                                    Qty = r.Qty,
                                                    TotalAmt = r.TotalAmt,
                                                    BillDate = r.BillDate,
                                                    CatCode = r.CatCode,
                                                    PartyCode = r.SoldBy,
                                                    FType = r.FType,
                                                    UserName = billtype.Username,
                                                    BillType = billtype.BillType
                                                }).ToList();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                //objListSales = new List<SalesReport>();
                //objListSales[0].ErrorMsg = ex.Message;
            }
            return objListSales;
        }

        public List<StockJv> GetStockJvReport(string FromDate, string ToDate, string PartyCode, string ViewType)
        {
            List<StockJv> objStockJv = new List<StockJv>();
            try
            {
                DateTime StartDate = DateTime.Now;
                DateTime EndDate = DateTime.Now;
                if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                {
                    var SplitDate = FromDate.Split('-');
                    string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];
                    StartDate = Convert.ToDateTime(NewDate);
                    StartDate = StartDate.Date;
                }
                if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                {
                    var SplitDate = ToDate.Split('-');
                    string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];
                    EndDate = Convert.ToDateTime(NewDate);
                    EndDate = EndDate.Date;
                }
                using (var entity = new InventoryEntities())
                {
                    if (ViewType == "Both")
                    {
                        if (PartyCode != "0")
                        {
                            if (FromDate != "All" && ToDate != "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.FCode == PartyCode
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  JvDate = r.StockDate.ToString(),
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) >= EntityFunctions.TruncateTime(StartDate) && EntityFunctions.TruncateTime(m.StockDate) <= EntityFunctions.TruncateTime(EndDate)).ToList();
                            }
                            else if (FromDate != "All" && ToDate == "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.FCode == PartyCode
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) >= EntityFunctions.TruncateTime(StartDate)).ToList();
                            }
                            else if (FromDate == "All" && ToDate != "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.FCode == PartyCode
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) <= EntityFunctions.TruncateTime(EndDate)).ToList();
                            }
                            else
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.FCode == PartyCode
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                           ).ToList();
                            }
                        }
                        else
                        {
                            if (FromDate != "All" && ToDate != "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) >= EntityFunctions.TruncateTime(StartDate) && EntityFunctions.TruncateTime(m.StockDate) <= EntityFunctions.TruncateTime(EndDate)).ToList();
                            }
                            else if (FromDate != "All" && ToDate == "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) >= EntityFunctions.TruncateTime(StartDate)).ToList();
                            }
                            else if (FromDate == "All" && ToDate != "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) <= EntityFunctions.TruncateTime(EndDate)).ToList();
                            }
                            else
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                           ).ToList();
                            }
                        }
                    }
                    else if (ViewType == "Add")
                    {
                        if (PartyCode != "0")
                        {
                            if (FromDate != "All" && ToDate != "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.FCode == PartyCode && r.JType == "A"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) >= EntityFunctions.TruncateTime(StartDate) && EntityFunctions.TruncateTime(m.StockDate) <= EntityFunctions.TruncateTime(EndDate)).ToList();
                            }
                            else if (FromDate != "All" && ToDate == "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.FCode == PartyCode && r.JType == "A"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) >= EntityFunctions.TruncateTime(StartDate)).ToList();
                            }
                            else if (FromDate == "All" && ToDate != "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.FCode == PartyCode && r.JType == "A"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) <= EntityFunctions.TruncateTime(EndDate)).ToList();
                            }
                            else
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.FCode == PartyCode && r.JType == "A"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                           ).ToList();
                            }
                        }
                        else
                        {
                            if (FromDate != "All" && ToDate != "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.JType == "A"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) >= EntityFunctions.TruncateTime(StartDate) && EntityFunctions.TruncateTime(m.StockDate) <= EntityFunctions.TruncateTime(EndDate)).ToList();
                            }
                            else if (FromDate != "All" && ToDate == "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.JType == "A"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) >= EntityFunctions.TruncateTime(StartDate)).ToList();
                            }
                            else if (FromDate == "All" && ToDate != "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.JType == "A"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) <= EntityFunctions.TruncateTime(EndDate)).ToList();
                            }
                            else
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.JType == "A"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                           ).ToList();
                            }
                        }
                    }
                    else
                    {
                        //ViewType==Less
                        if (PartyCode != "0")
                        {
                            if (FromDate != "All" && ToDate != "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.FCode == PartyCode && r.JType == "L"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) >= EntityFunctions.TruncateTime(StartDate) && EntityFunctions.TruncateTime(m.StockDate) <= EntityFunctions.TruncateTime(EndDate)).ToList();
                            }
                            else if (FromDate != "All" && ToDate == "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.FCode == PartyCode && r.JType == "L"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) >= EntityFunctions.TruncateTime(StartDate)).ToList();
                            }
                            else if (FromDate == "All" && ToDate != "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.FCode == PartyCode && r.JType == "L"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) <= EntityFunctions.TruncateTime(EndDate)).ToList();
                            }
                            else
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.FCode == PartyCode && r.JType == "L"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                           ).ToList();
                            }
                        }
                        else
                        {
                            if (FromDate != "All" && ToDate != "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.JType == "L"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) >= EntityFunctions.TruncateTime(StartDate) && EntityFunctions.TruncateTime(m.StockDate) <= EntityFunctions.TruncateTime(EndDate)).ToList();
                            }
                            else if (FromDate != "All" && ToDate == "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.JType == "L"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) >= EntityFunctions.TruncateTime(StartDate)).ToList();
                            }
                            else if (FromDate == "All" && ToDate != "All")
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.JType == "L"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                            ).Where(m => EntityFunctions.TruncateTime(m.StockDate) <= EntityFunctions.TruncateTime(EndDate)).ToList();
                            }
                            else
                            {
                                objStockJv = (from r in entity.TrnStockJvs
                                              where r.ActiveStatus == "Y" && r.JType == "L"
                                              select new StockJv
                                              {
                                                  StockDate = r.StockDate,
                                                  //JvDate=r.StockDate.Date.ToString(),
                                                  RefNo = r.RefNo,
                                                  JvNo = r.JvNo,
                                                  FCode = r.FCode,
                                                  PartyName = r.PartyName,
                                                  objProduct = new ProductModel()
                                                  {
                                                      ProductCodeStr = r.ProdId,
                                                      ProductName = r.ProductName,
                                                      BatchNo = r.BatchNo,
                                                      Quantity = r.Qty,

                                                  },
                                                  Remarks = r.Remarks,
                                                  SoldBy = r.SoldBy
                                              }

                                           ).ToList();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return objStockJv;


        }



        public List<PurchaseReport> GetPurchaseSummary(string FromDate, string ToDate, string PartyCode, string SupplierCode, string ReportType, string InvoiceNo)
        {
            List<PurchaseReport> objListPurchaseSummary = new List<PurchaseReport>();
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                    {
                        var SplitDate = FromDate.Split('-');
                        string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];//SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                        StartDate = Convert.ToDateTime(NewDate);
                        StartDate = StartDate.Date;
                    }
                    if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                    {
                        var SplitDate = ToDate.Split('-');
                        string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];
                        EndDate = Convert.ToDateTime(NewDate);
                        EndDate = EndDate.Date;
                    }
                    switch (ReportType)
                    {
                        case "Supplier":
                            if (PartyCode == "0" && SupplierCode == "0")
                            {
                                objListPurchaseSummary = (from r in entity.M_InwardMain
                                                          where r.ActiveStatus == "Y"
                                                          group r by new { r.SupplierName, r.SupplierCode }
                                                          into g
                                                          select new PurchaseReport
                                                          {
                                                              SupplierCode = g.Key.SupplierCode,
                                                              SupplierName = g.Key.SupplierName,
                                                              TotalQty = g.Sum(m => m.TotalQty).ToString(),
                                                              Amount = g.Sum(m => m.TotalAmt).ToString(),
                                                              NetAmount = g.Sum(m => m.NetPayable).ToString(),
                                                              TaxAmount = g.Sum(m => m.TotalTaxAmt + m.CGSTAmt + m.SGSTAmt).ToString(),
                                                              TradeDisc = g.Sum(m => m.TotalTradeDiscount).ToString(),
                                                              CashDisc = g.Sum(m => m.TotalCashDiscount).ToString()
                                                          }).ToList();
                            }
                            else if (PartyCode == "0" && SupplierCode != "0")
                            {
                                objListPurchaseSummary = (from r in entity.M_InwardMain
                                                          where r.ActiveStatus == "Y" && r.SupplierCode == SupplierCode
                                                          group r by new { r.SupplierName, r.SupplierCode }
                                                          into g
                                                          select new PurchaseReport
                                                          {
                                                              SupplierCode = g.Key.SupplierCode,
                                                              SupplierName = g.Key.SupplierName,
                                                              TotalQty = g.Sum(m => m.TotalQty).ToString(),
                                                              Amount = g.Sum(m => m.TotalAmt).ToString(),
                                                              NetAmount = g.Sum(m => m.NetPayable).ToString(),
                                                              TaxAmount = g.Sum(m => m.TotalTaxAmt + m.CGSTAmt + m.SGSTAmt).ToString(),
                                                              TradeDisc = g.Sum(m => m.TotalTradeDiscount).ToString(),
                                                              CashDisc = g.Sum(m => m.TotalCashDiscount).ToString()
                                                          }).ToList();
                            }
                            else if (PartyCode != "0" && SupplierCode == "0")
                            {
                                objListPurchaseSummary = (from r in entity.M_InwardMain
                                                          where r.ActiveStatus == "Y" && r.InwardBy == PartyCode
                                                          group r by new { r.SupplierName, r.SupplierCode }
                                                          into g
                                                          select new PurchaseReport
                                                          {
                                                              SupplierCode = g.Key.SupplierCode,
                                                              SupplierName = g.Key.SupplierName,
                                                              TotalQty = g.Sum(m => m.TotalQty).ToString(),
                                                              Amount = g.Sum(m => m.TotalAmt).ToString(),
                                                              NetAmount = g.Sum(m => m.NetPayable).ToString(),
                                                              TaxAmount = g.Sum(m => m.TotalTaxAmt + m.CGSTAmt + m.SGSTAmt).ToString(),
                                                              TradeDisc = g.Sum(m => m.TotalTradeDiscount).ToString(),
                                                              CashDisc = g.Sum(m => m.TotalCashDiscount).ToString()
                                                          }).ToList();
                            }
                            else
                            {
                                objListPurchaseSummary = (from r in entity.M_InwardMain
                                                          where r.ActiveStatus == "Y" && r.InwardBy == PartyCode && r.SupplierCode == SupplierCode
                                                          group r by new { r.SupplierName, r.SupplierCode }
                                                          into g
                                                          select new PurchaseReport
                                                          {
                                                              SupplierCode = g.Key.SupplierCode,
                                                              SupplierName = g.Key.SupplierName,
                                                              TotalQty = g.Sum(m => m.TotalQty).ToString(),
                                                              Amount = g.Sum(m => m.TotalAmt).ToString(),
                                                              NetAmount = g.Sum(m => m.NetPayable).ToString(),
                                                              TaxAmount = g.Sum(m => m.TotalTaxAmt + m.CGSTAmt + m.SGSTAmt).ToString(),
                                                              TradeDisc = g.Sum(m => m.TotalTradeDiscount).ToString(),
                                                              CashDisc = g.Sum(m => m.TotalCashDiscount).ToString()
                                                          }).ToList();
                            }
                            break;
                        case "Invoice":
                            if (PartyCode == "0" && SupplierCode == "0")
                            {
                                objListPurchaseSummary = (from r in entity.M_InwardMain
                                                          where r.ActiveStatus == "Y"

                                                          group r by new { r.InwardNo, r.InwardFor, r.RefNo, r.RecvDate, r.SupplierCode, r.SupplierName }
                                                             into g
                                                          orderby g.Key.RecvDate, g.Key.SupplierCode
                                                          select new PurchaseReport
                                                          {
                                                              InvoiceFor = g.Key.InwardFor,
                                                              InvoiceNo = g.Key.InwardNo,
                                                              RefNo = g.Key.RefNo,
                                                              InvoiceDateStr = g.Key.RecvDate,
                                                              SupplierCode = g.Key.SupplierCode,
                                                              SupplierName = g.Key.SupplierName,
                                                              TotalQty = g.Sum(m => m.TotalQty).ToString(),
                                                              Amount = g.Sum(m => m.TotalAmt).ToString(),
                                                              NetAmount = g.Sum(m => m.NetPayable).ToString(),
                                                              TaxAmount = g.Sum(m => m.TotalTaxAmt + m.CGSTAmt + m.SGSTAmt).ToString(),
                                                              TradeDisc = g.Sum(m => m.TotalTradeDiscount).ToString(),
                                                              CashDisc = g.Sum(m => m.TotalCashDiscount).ToString()
                                                          }).ToList();
                            }
                            else if (PartyCode == "0" && SupplierCode != "0")
                            {
                                objListPurchaseSummary = (from r in entity.M_InwardMain
                                                          where r.ActiveStatus == "Y" && r.SupplierCode == SupplierCode
                                                          group r by new { r.InwardNo, r.InwardFor, r.RefNo, r.RecvDate, r.SupplierCode, r.SupplierName }
                                                             into g
                                                          orderby g.Key.RecvDate, g.Key.SupplierCode
                                                          select new PurchaseReport
                                                          {
                                                              InvoiceFor = g.Key.InwardFor,
                                                              InvoiceNo = g.Key.InwardNo,
                                                              RefNo = g.Key.RefNo,
                                                              InvoiceDateStr = g.Key.RecvDate,
                                                              SupplierCode = g.Key.SupplierCode,
                                                              SupplierName = g.Key.SupplierName,
                                                              TotalQty = g.Sum(m => m.TotalQty).ToString(),
                                                              Amount = g.Sum(m => m.TotalAmt).ToString(),
                                                              NetAmount = g.Sum(m => m.NetPayable).ToString(),
                                                              TaxAmount = g.Sum(m => m.TotalTaxAmt + m.CGSTAmt + m.SGSTAmt).ToString(),
                                                              TradeDisc = g.Sum(m => m.TotalTradeDiscount).ToString(),
                                                              CashDisc = g.Sum(m => m.TotalCashDiscount).ToString()
                                                          }).ToList();
                            }
                            else if (PartyCode != "0" && SupplierCode == "0")
                            {
                                objListPurchaseSummary = (from r in entity.M_InwardMain
                                                          where r.ActiveStatus == "Y" && r.InwardBy == PartyCode
                                                          group r by new { r.InwardNo, r.InwardFor, r.RefNo, r.RecvDate, r.SupplierCode, r.SupplierName, r.FSessId }
                                                             into g
                                                          orderby g.Key.RecvDate, g.Key.SupplierCode
                                                          select new PurchaseReport
                                                          {
                                                              InvoiceFor = g.Key.InwardFor,
                                                              FSessId = g.Key.FSessId,
                                                              InvoiceNo = g.Key.InwardNo,
                                                              RefNo = g.Key.RefNo,
                                                              InvoiceDateStr = g.Key.RecvDate,
                                                              SupplierCode = g.Key.SupplierCode,
                                                              SupplierName = g.Key.SupplierName,
                                                              TotalQty = g.Sum(m => m.TotalQty).ToString(),
                                                              Amount = g.Sum(m => m.TotalAmt).ToString(),
                                                              NetAmount = g.Sum(m => m.NetPayable).ToString(),
                                                              TaxAmount = g.Sum(m => m.TotalTaxAmt + m.CGSTAmt + m.SGSTAmt).ToString(),
                                                              TradeDisc = g.Sum(m => m.TotalTradeDiscount).ToString(),
                                                              CashDisc = g.Sum(m => m.TotalCashDiscount).ToString()
                                                          }).ToList();
                            }
                            else
                            {
                                objListPurchaseSummary = (from r in entity.M_InwardMain
                                                          where r.ActiveStatus == "Y" && r.InwardBy == PartyCode && r.SupplierCode == SupplierCode
                                                          group r by new { r.InwardNo, r.InwardFor, r.RefNo, r.RecvDate, r.SupplierCode, r.SupplierName }
                                                             into g
                                                          orderby g.Key.RecvDate, g.Key.SupplierCode
                                                          select new PurchaseReport
                                                          {
                                                              InvoiceFor = g.Key.InwardFor,
                                                              InvoiceNo = g.Key.InwardNo,
                                                              RefNo = g.Key.RefNo,
                                                              InvoiceDateStr = g.Key.RecvDate,
                                                              SupplierCode = g.Key.SupplierCode,
                                                              SupplierName = g.Key.SupplierName,
                                                              TotalQty = g.Sum(m => m.TotalQty).ToString(),
                                                              Amount = g.Sum(m => m.TotalAmt).ToString(),
                                                              NetAmount = g.Sum(m => m.NetPayable).ToString(),
                                                              TaxAmount = g.Sum(m => m.TotalTaxAmt + m.CGSTAmt + m.SGSTAmt).ToString(),
                                                              TradeDisc = g.Sum(m => m.TotalTradeDiscount).ToString(),
                                                              CashDisc = g.Sum(m => m.TotalCashDiscount).ToString()
                                                          }).ToList();
                            }
                            if (!string.IsNullOrEmpty(InvoiceNo) && InvoiceNo != "0" && InvoiceNo.ToLower() != "all")
                            {
                                objListPurchaseSummary = (from r in objListPurchaseSummary where r.InvoiceNo == InvoiceNo select r).ToList();
                            }
                            break;
                        case "":

                            break;
                    }
                    if (FromDate == "All" && ToDate != "All")
                    {
                        objListPurchaseSummary = objListPurchaseSummary.Where(m => m.InvoiceDateStr.Date <= EndDate.Date).ToList();
                    }
                    else if (FromDate != "All" && ToDate == "All")
                    {
                        objListPurchaseSummary = objListPurchaseSummary.Where(m => m.InvoiceDateStr.Date >= StartDate.Date).ToList();
                    }
                    else if (FromDate != "All" && ToDate != "All")
                    {
                        objListPurchaseSummary = objListPurchaseSummary.Where(m => m.InvoiceDateStr.Date >= StartDate.Date && m.InvoiceDateStr.Date <= EndDate.Date).ToList();
                    }


                }
            }
            catch (Exception ex)
            {

            }

            return objListPurchaseSummary;
        }

        public List<PurchaseReport> GetPurchaseDetailSummary(string FromDate, string ToDate, string PartyCode, string SupplierCode, string ProductCode)
        {
            List<PurchaseReport> objListPurchaseSummary = new List<PurchaseReport>();
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                    {
                        var SplitDate = FromDate.Split('-');
                        string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];
                        StartDate = Convert.ToDateTime(NewDate);
                        StartDate = StartDate.Date;
                    }
                    if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                    {
                        var SplitDate = ToDate.Split('-');
                        string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];
                        EndDate = Convert.ToDateTime(NewDate);
                        EndDate = EndDate.Date;
                    }


                    if (PartyCode == "0" && SupplierCode == "0")
                    {
                        if (ProductCode == "0")
                        {
                            objListPurchaseSummary = (from r in entity.M_InwardMain
                                                      where r.ActiveStatus == "Y"
                                                      join p in entity.M_InwardData on r.InwardNo equals p.InwardNo
                                                      group r by new { r.InwardNo, r.InwardFor, r.RefNo, r.RecvDate, r.SupplierCode, r.SupplierName, p.ProdCode, p.ProdName, p.BatchNo, p.Barcode, p.Qty, p.MRP, p.PRate, p.Amount, p.TradeDiscount, p.TradeAmount, p.Tax, p.TaxAmt, p.CGST, p.CGSTAmt, p.SGST, p.SGSTAmt, p.TotalAmount }
                                                         into g
                                                      orderby g.Key.RecvDate, g.Key.SupplierCode
                                                      select new PurchaseReport
                                                      {
                                                          InvoiceFor = g.Key.InwardFor,
                                                          InvoiceNo = g.Key.InwardNo,
                                                          RefNo = g.Key.RefNo,
                                                          InvoiceDateStr = g.Key.RecvDate,
                                                          SupplierCode = g.Key.SupplierCode,
                                                          SupplierName = g.Key.SupplierName,
                                                          objproduct = new ProductModel
                                                          {
                                                              ProductCodeStr = g.Key.ProdCode,
                                                              ProductName = g.Key.ProdName,
                                                              Quantity = g.Key.Qty,
                                                              Barcode = g.Key.Barcode,
                                                              BatchNo = g.Key.BatchNo,
                                                              DiscPer = g.Key.TradeDiscount,
                                                              DiscAmt = g.Key.TradeAmount,
                                                              TaxPer = g.Key.Tax,
                                                              TaxAmt = g.Key.TaxAmt,
                                                              CGST = g.Key.CGST,
                                                              CGSTAmount = g.Key.CGSTAmt,
                                                              SGST = g.Key.SGST,
                                                              SGSTAmount = g.Key.SGSTAmt,
                                                              MRP = g.Key.MRP,
                                                              Rate = g.Key.PRate,
                                                              Amount = g.Key.Amount,
                                                              TotalAmount = g.Key.TotalAmount
                                                          },
                                                          TotalQty = g.Sum(m => m.TotalQty).ToString(),
                                                          Amount = g.Sum(m => m.TotalAmt).ToString(),
                                                          NetAmount = g.Sum(m => m.NetPayable).ToString(),
                                                          TaxAmount = g.Sum(m => m.TotalTaxAmt).ToString(),
                                                          TradeDisc = g.Sum(m => m.TotalTradeDiscount).ToString(),
                                                          CashDisc = g.Sum(m => m.TotalCashDiscount).ToString()
                                                      }).ToList();
                        }
                        else
                        {
                            objListPurchaseSummary = (from r in entity.M_InwardMain
                                                      where r.ActiveStatus == "Y"
                                                      join p in entity.M_InwardData on r.InwardNo equals p.InwardNo
                                                      where p.ProdCode == ProductCode
                                                      group r by new { r.InwardNo, r.InwardFor, r.RefNo, r.RecvDate, r.SupplierCode, r.SupplierName, p.ProdCode, p.ProdName, p.BatchNo, p.Barcode, p.Qty, p.MRP, p.PRate, p.Amount, p.TradeDiscount, p.TradeAmount, p.Tax, p.TaxAmt, p.CGST, p.CGSTAmt, p.SGST, p.SGSTAmt, p.TotalAmount }
                                                         into g
                                                      orderby g.Key.RecvDate, g.Key.SupplierCode
                                                      select new PurchaseReport
                                                      {
                                                          InvoiceFor = g.Key.InwardFor,
                                                          InvoiceNo = g.Key.InwardNo,
                                                          RefNo = g.Key.RefNo,
                                                          InvoiceDateStr = g.Key.RecvDate,
                                                          SupplierCode = g.Key.SupplierCode,
                                                          SupplierName = g.Key.SupplierName,
                                                          objproduct = new ProductModel
                                                          {
                                                              ProductCodeStr = g.Key.ProdCode,
                                                              ProductName = g.Key.ProdName,
                                                              Quantity = g.Key.Qty,
                                                              Barcode = g.Key.Barcode,
                                                              BatchNo = g.Key.BatchNo,
                                                              DiscPer = g.Key.TradeDiscount,
                                                              DiscAmt = g.Key.TradeAmount,
                                                              TaxPer = g.Key.Tax,
                                                              TaxAmt = g.Key.TaxAmt,
                                                              CGST = g.Key.CGST,
                                                              CGSTAmount = g.Key.CGSTAmt,
                                                              SGST = g.Key.SGST,
                                                              SGSTAmount = g.Key.SGSTAmt,
                                                              MRP = g.Key.MRP,
                                                              Rate = g.Key.PRate,
                                                              Amount = g.Key.Amount,
                                                              TotalAmount = g.Key.TotalAmount
                                                          },
                                                          TotalQty = g.Sum(m => m.TotalQty).ToString(),
                                                          Amount = g.Sum(m => m.TotalAmt).ToString(),
                                                          NetAmount = g.Sum(m => m.NetPayable).ToString(),
                                                          TaxAmount = g.Sum(m => m.TotalTaxAmt).ToString(),
                                                          TradeDisc = g.Sum(m => m.TotalTradeDiscount).ToString(),
                                                          CashDisc = g.Sum(m => m.TotalCashDiscount).ToString()
                                                      }).ToList();
                        }
                    }
                    else if (PartyCode == "0" && SupplierCode != "0")
                    {
                        if (ProductCode == "0")
                        {
                            objListPurchaseSummary = (from r in entity.M_InwardMain
                                                      where r.ActiveStatus == "Y" && r.SupplierCode == SupplierCode
                                                      join p in entity.M_InwardData on r.InwardNo equals p.InwardNo
                                                      group r by new { r.InwardNo, r.InwardFor, r.RefNo, r.RecvDate, r.SupplierCode, r.SupplierName, p.ProdCode, p.ProdName, p.BatchNo, p.Barcode, p.Qty, p.MRP, p.PRate, p.Amount, p.TradeDiscount, p.TradeAmount, p.Tax, p.TaxAmt, p.CGST, p.CGSTAmt, p.SGST, p.SGSTAmt, p.TotalAmount }
                                                         into g
                                                      orderby g.Key.RecvDate, g.Key.SupplierCode
                                                      select new PurchaseReport
                                                      {
                                                          InvoiceFor = g.Key.InwardFor,
                                                          InvoiceNo = g.Key.InwardNo,
                                                          RefNo = g.Key.RefNo,
                                                          InvoiceDateStr = g.Key.RecvDate,
                                                          SupplierCode = g.Key.SupplierCode,
                                                          SupplierName = g.Key.SupplierName,
                                                          objproduct = new ProductModel
                                                          {
                                                              ProductCodeStr = g.Key.ProdCode,
                                                              ProductName = g.Key.ProdName,
                                                              Quantity = g.Key.Qty,
                                                              Barcode = g.Key.Barcode,
                                                              BatchNo = g.Key.BatchNo,
                                                              DiscPer = g.Key.TradeDiscount,
                                                              DiscAmt = g.Key.TradeAmount,
                                                              TaxPer = g.Key.Tax,
                                                              TaxAmt = g.Key.TaxAmt,
                                                              CGST = g.Key.CGST,
                                                              CGSTAmount = g.Key.CGSTAmt,
                                                              SGST = g.Key.SGST,
                                                              SGSTAmount = g.Key.SGSTAmt,
                                                              MRP = g.Key.MRP,
                                                              Rate = g.Key.PRate,
                                                              Amount = g.Key.Amount,
                                                              TotalAmount = g.Key.TotalAmount
                                                          },
                                                          TotalQty = g.Sum(m => m.TotalQty).ToString(),
                                                          Amount = g.Sum(m => m.TotalAmt).ToString(),
                                                          NetAmount = g.Sum(m => m.NetPayable).ToString(),
                                                          TaxAmount = g.Sum(m => m.TotalTaxAmt).ToString(),
                                                          TradeDisc = g.Sum(m => m.TotalTradeDiscount).ToString(),
                                                          CashDisc = g.Sum(m => m.TotalCashDiscount).ToString()
                                                      }).ToList();
                        }
                        else
                        {
                            objListPurchaseSummary = (from r in entity.M_InwardMain
                                                      where r.ActiveStatus == "Y" && r.SupplierCode == SupplierCode
                                                      join p in entity.M_InwardData on r.InwardNo equals p.InwardNo
                                                      where p.ProdCode == ProductCode
                                                      group r by new { r.InwardNo, r.InwardFor, r.RefNo, r.RecvDate, r.SupplierCode, r.SupplierName, p.ProdCode, p.ProdName, p.BatchNo, p.Barcode, p.Qty, p.MRP, p.PRate, p.Amount, p.TradeDiscount, p.TradeAmount, p.Tax, p.TaxAmt, p.CGST, p.CGSTAmt, p.SGST, p.SGSTAmt, p.TotalAmount }
                                                             into g
                                                      orderby g.Key.RecvDate, g.Key.SupplierCode
                                                      select new PurchaseReport
                                                      {
                                                          InvoiceFor = g.Key.InwardFor,
                                                          InvoiceNo = g.Key.InwardNo,
                                                          RefNo = g.Key.RefNo,
                                                          InvoiceDateStr = g.Key.RecvDate,
                                                          SupplierCode = g.Key.SupplierCode,
                                                          SupplierName = g.Key.SupplierName,
                                                          objproduct = new ProductModel
                                                          {
                                                              ProductCodeStr = g.Key.ProdCode,
                                                              ProductName = g.Key.ProdName,
                                                              Quantity = g.Key.Qty,
                                                              Barcode = g.Key.Barcode,
                                                              BatchNo = g.Key.BatchNo,
                                                              DiscPer = g.Key.TradeDiscount,
                                                              DiscAmt = g.Key.TradeAmount,
                                                              TaxPer = g.Key.Tax,
                                                              TaxAmt = g.Key.TaxAmt,
                                                              CGST = g.Key.CGST,
                                                              CGSTAmount = g.Key.CGSTAmt,
                                                              SGST = g.Key.SGST,
                                                              SGSTAmount = g.Key.SGSTAmt,
                                                              MRP = g.Key.MRP,
                                                              Rate = g.Key.PRate,
                                                              Amount = g.Key.Amount,
                                                              TotalAmount = g.Key.TotalAmount
                                                          },
                                                          TotalQty = g.Sum(m => m.TotalQty).ToString(),
                                                          Amount = g.Sum(m => m.TotalAmt).ToString(),
                                                          NetAmount = g.Sum(m => m.NetPayable).ToString(),
                                                          TaxAmount = g.Sum(m => m.TotalTaxAmt).ToString(),
                                                          TradeDisc = g.Sum(m => m.TotalTradeDiscount).ToString(),
                                                          CashDisc = g.Sum(m => m.TotalCashDiscount).ToString()
                                                      }).ToList();
                        }
                    }
                    else if (PartyCode != "0" && SupplierCode == "0")
                    {
                        if (ProductCode == "0")
                        {
                            objListPurchaseSummary = (from r in entity.M_InwardMain
                                                      where r.ActiveStatus == "Y" && r.InwardBy == PartyCode
                                                      join p in entity.M_InwardData on r.InwardNo equals p.InwardNo
                                                      group r by new { r.InwardNo, r.InwardFor, r.RefNo, r.RecvDate, r.SupplierCode, r.SupplierName, p.ProdCode, p.ProdName, p.BatchNo, p.Barcode, p.Qty, p.MRP, p.PRate, p.Amount, p.TradeDiscount, p.TradeAmount, p.Tax, p.TaxAmt, p.CGST, p.CGSTAmt, p.SGST, p.SGSTAmt, p.TotalAmount }
                                                         into g
                                                      orderby g.Key.RecvDate, g.Key.SupplierCode
                                                      select new PurchaseReport
                                                      {
                                                          InvoiceFor = g.Key.InwardFor,
                                                          InvoiceNo = g.Key.InwardNo,
                                                          RefNo = g.Key.RefNo,
                                                          InvoiceDateStr = g.Key.RecvDate,
                                                          SupplierCode = g.Key.SupplierCode,
                                                          SupplierName = g.Key.SupplierName,
                                                          objproduct = new ProductModel
                                                          {
                                                              ProductCodeStr = g.Key.ProdCode,
                                                              ProductName = g.Key.ProdName,
                                                              Quantity = g.Key.Qty,
                                                              Barcode = g.Key.Barcode,
                                                              BatchNo = g.Key.BatchNo,
                                                              DiscPer = g.Key.TradeDiscount,
                                                              DiscAmt = g.Key.TradeAmount,
                                                              TaxPer = g.Key.Tax,
                                                              TaxAmt = g.Key.TaxAmt,
                                                              CGST = g.Key.CGST,
                                                              CGSTAmount = g.Key.CGSTAmt,
                                                              SGST = g.Key.SGST,
                                                              SGSTAmount = g.Key.SGSTAmt,
                                                              MRP = g.Key.MRP,
                                                              Rate = g.Key.PRate,
                                                              Amount = g.Key.Amount,
                                                              TotalAmount = g.Key.TotalAmount
                                                          },
                                                          TotalQty = g.Sum(m => m.TotalQty).ToString(),
                                                          Amount = g.Sum(m => m.TotalAmt).ToString(),
                                                          NetAmount = g.Sum(m => m.NetPayable).ToString(),
                                                          TaxAmount = g.Sum(m => m.TotalTaxAmt).ToString(),
                                                          TradeDisc = g.Sum(m => m.TotalTradeDiscount).ToString(),
                                                          CashDisc = g.Sum(m => m.TotalCashDiscount).ToString()
                                                      }).ToList();
                        }
                        else
                        {
                            objListPurchaseSummary = (from r in entity.M_InwardMain
                                                      where r.ActiveStatus == "Y" && r.InwardBy == PartyCode
                                                      join p in entity.M_InwardData on r.InwardNo equals p.InwardNo
                                                      where p.ProdCode == ProductCode
                                                      group r by new { r.InwardNo, r.InwardFor, r.RefNo, r.RecvDate, r.SupplierCode, r.SupplierName, p.ProdCode, p.ProdName, p.BatchNo, p.Barcode, p.Qty, p.MRP, p.PRate, p.Amount, p.TradeDiscount, p.TradeAmount, p.Tax, p.TaxAmt, p.CGST, p.CGSTAmt, p.SGST, p.SGSTAmt, p.TotalAmount }
                                                             into g
                                                      orderby g.Key.RecvDate, g.Key.SupplierCode
                                                      select new PurchaseReport
                                                      {
                                                          InvoiceFor = g.Key.InwardFor,
                                                          InvoiceNo = g.Key.InwardNo,
                                                          RefNo = g.Key.RefNo,
                                                          InvoiceDateStr = g.Key.RecvDate,
                                                          SupplierCode = g.Key.SupplierCode,
                                                          SupplierName = g.Key.SupplierName,
                                                          objproduct = new ProductModel
                                                          {
                                                              ProductCodeStr = g.Key.ProdCode,
                                                              ProductName = g.Key.ProdName,
                                                              Quantity = g.Key.Qty,
                                                              Barcode = g.Key.Barcode,
                                                              BatchNo = g.Key.BatchNo,
                                                              DiscPer = g.Key.TradeDiscount,
                                                              DiscAmt = g.Key.TradeAmount,
                                                              TaxPer = g.Key.Tax,
                                                              TaxAmt = g.Key.TaxAmt,
                                                              CGST = g.Key.CGST,
                                                              CGSTAmount = g.Key.CGSTAmt,
                                                              SGST = g.Key.SGST,
                                                              SGSTAmount = g.Key.SGSTAmt,
                                                              MRP = g.Key.MRP,
                                                              Rate = g.Key.PRate,
                                                              Amount = g.Key.Amount,
                                                              TotalAmount = g.Key.TotalAmount
                                                          },
                                                          TotalQty = g.Sum(m => m.TotalQty).ToString(),
                                                          Amount = g.Sum(m => m.TotalAmt).ToString(),
                                                          NetAmount = g.Sum(m => m.NetPayable).ToString(),
                                                          TaxAmount = g.Sum(m => m.TotalTaxAmt).ToString(),
                                                          TradeDisc = g.Sum(m => m.TotalTradeDiscount).ToString(),
                                                          CashDisc = g.Sum(m => m.TotalCashDiscount).ToString()
                                                      }).ToList();
                        }
                    }
                    else
                    {
                        if (ProductCode == "0")
                        {
                            objListPurchaseSummary = (from r in entity.M_InwardMain
                                                      where r.ActiveStatus == "Y" && r.InwardBy == PartyCode && r.SupplierCode == SupplierCode
                                                      join p in entity.M_InwardData on r.InwardNo equals p.InwardNo
                                                      group r by new { r.InwardNo, r.InwardFor, r.RefNo, r.RecvDate, r.SupplierCode, r.SupplierName, p.ProdCode, p.ProdName, p.BatchNo, p.Barcode, p.Qty, p.MRP, p.PRate, p.Amount, p.TradeDiscount, p.TradeAmount, p.Tax, p.TaxAmt, p.CGST, p.CGSTAmt, p.SGST, p.SGSTAmt, p.TotalAmount }
                                                         into g
                                                      orderby g.Key.RecvDate, g.Key.SupplierCode
                                                      select new PurchaseReport
                                                      {
                                                          InvoiceFor = g.Key.InwardFor,
                                                          InvoiceNo = g.Key.InwardNo,
                                                          RefNo = g.Key.RefNo,
                                                          InvoiceDateStr = g.Key.RecvDate,
                                                          SupplierCode = g.Key.SupplierCode,
                                                          SupplierName = g.Key.SupplierName,
                                                          objproduct = new ProductModel
                                                          {
                                                              ProductCodeStr = g.Key.ProdCode,
                                                              ProductName = g.Key.ProdName,
                                                              Quantity = g.Key.Qty,
                                                              Barcode = g.Key.Barcode,
                                                              BatchNo = g.Key.BatchNo,
                                                              DiscPer = g.Key.TradeDiscount,
                                                              DiscAmt = g.Key.TradeAmount,
                                                              TaxPer = g.Key.Tax,
                                                              TaxAmt = g.Key.TaxAmt,
                                                              CGST = g.Key.CGST,
                                                              CGSTAmount = g.Key.CGSTAmt,
                                                              SGST = g.Key.SGST,
                                                              SGSTAmount = g.Key.SGSTAmt,
                                                              MRP = g.Key.MRP,
                                                              Rate = g.Key.PRate,
                                                              Amount = g.Key.Amount,
                                                              TotalAmount = g.Key.TotalAmount
                                                          },
                                                          TotalQty = g.Sum(m => m.TotalQty).ToString(),
                                                          Amount = g.Sum(m => m.TotalAmt).ToString(),
                                                          NetAmount = g.Sum(m => m.NetPayable).ToString(),
                                                          TaxAmount = g.Sum(m => m.TotalTaxAmt).ToString(),
                                                          TradeDisc = g.Sum(m => m.TotalTradeDiscount).ToString(),
                                                          CashDisc = g.Sum(m => m.TotalCashDiscount).ToString()
                                                      }).ToList();
                        }
                        else
                        {
                            objListPurchaseSummary = (from r in entity.M_InwardMain
                                                      where r.ActiveStatus == "Y" && r.InwardBy == PartyCode && r.SupplierCode == SupplierCode
                                                      join p in entity.M_InwardData on r.InwardNo equals p.InwardNo
                                                      where p.ProdCode == ProductCode
                                                      group r by new { r.InwardNo, r.InwardFor, r.RefNo, r.RecvDate, r.SupplierCode, r.SupplierName, p.ProdCode, p.ProdName, p.BatchNo, p.Barcode, p.Qty, p.MRP, p.PRate, p.Amount, p.TradeDiscount, p.TradeAmount, p.Tax, p.TaxAmt, p.CGST, p.CGSTAmt, p.SGST, p.SGSTAmt, p.TotalAmount }
                                                             into g
                                                      orderby g.Key.RecvDate, g.Key.SupplierCode
                                                      select new PurchaseReport
                                                      {
                                                          InvoiceFor = g.Key.InwardFor,
                                                          InvoiceNo = g.Key.InwardNo,
                                                          RefNo = g.Key.RefNo,
                                                          InvoiceDateStr = g.Key.RecvDate,
                                                          SupplierCode = g.Key.SupplierCode,
                                                          SupplierName = g.Key.SupplierName,
                                                          objproduct = new ProductModel
                                                          {
                                                              ProductCodeStr = g.Key.ProdCode,
                                                              ProductName = g.Key.ProdName,
                                                              Quantity = g.Key.Qty,
                                                              Barcode = g.Key.Barcode,
                                                              BatchNo = g.Key.BatchNo,
                                                              DiscPer = g.Key.TradeDiscount,
                                                              DiscAmt = g.Key.TradeAmount,
                                                              TaxPer = g.Key.Tax,
                                                              TaxAmt = g.Key.TaxAmt,
                                                              CGST = g.Key.CGST,
                                                              CGSTAmount = g.Key.CGSTAmt,
                                                              SGST = g.Key.SGST,
                                                              SGSTAmount = g.Key.SGSTAmt,
                                                              MRP = g.Key.MRP,
                                                              Rate = g.Key.PRate,
                                                              Amount = g.Key.Amount,
                                                              TotalAmount = g.Key.TotalAmount
                                                          },
                                                          TotalQty = g.Sum(m => m.TotalQty).ToString(),
                                                          Amount = g.Sum(m => m.TotalAmt).ToString(),
                                                          NetAmount = g.Sum(m => m.NetPayable).ToString(),
                                                          TaxAmount = g.Sum(m => m.TotalTaxAmt).ToString(),
                                                          TradeDisc = g.Sum(m => m.TotalTradeDiscount).ToString(),
                                                          CashDisc = g.Sum(m => m.TotalCashDiscount).ToString()
                                                      }).ToList();
                        }
                    }


                    if (FromDate == "All" && ToDate != "All")
                    {
                        objListPurchaseSummary = objListPurchaseSummary.Where(m => m.InvoiceDateStr.Date <= EndDate.Date).ToList();
                    }
                    else if (FromDate != "All" && ToDate == "All")
                    {
                        objListPurchaseSummary = objListPurchaseSummary.Where(m => m.InvoiceDateStr.Date >= StartDate.Date).ToList();
                    }
                    else if (FromDate != "All" && ToDate != "All")
                    {
                        objListPurchaseSummary = objListPurchaseSummary.Where(m => m.InvoiceDateStr.Date >= StartDate.Date && m.InvoiceDateStr.Date <= EndDate.Date).ToList();
                    }


                }
            }
            catch (Exception ex)
            {

            }

            return objListPurchaseSummary;
        }

        public List<PurchaseReport> GetMonthWisePurchaseSummary(string Year, bool IsQuantity, bool IsAmount, string PartyCode, string SupplierCode)
        {
            List<PurchaseReport> objListPurchaseReport = new List<PurchaseReport>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    int YearInt = 0;
                    if (!string.IsNullOrEmpty(Year))
                    {
                        YearInt = int.Parse(Year);
                    }
                    if (Year == "0")
                    {
                        if (PartyCode == "0" && SupplierCode == "0")
                        {
                            objListPurchaseReport = (from r in entity.V_MonthWiseSupplierPurchaseSummary
                                                     select new PurchaseReport
                                                     {
                                                         SupplierCode = r.SupplierCode,
                                                         SupplierName = r.SupplierName,
                                                         PartyCode = r.PartyCode,
                                                         PartyName = r.PartyName,
                                                         Jan_Qty = r.JanQty.ToString(),
                                                         Jan_Pur = r.JanSale.ToString(),
                                                         Feb_Qty = r.FebQty.ToString(),
                                                         Feb_Pur = r.FebSale.ToString(),
                                                         March_Qty = r.MarQty.ToString(),
                                                         March_Pur = r.MarSale.ToString(),
                                                         April_Qty = r.AprQty.ToString(),
                                                         April_Pur = r.AprSale.ToString(),
                                                         May_Qty = r.MayQty.ToString(),
                                                         May_Pur = r.MaySale.ToString(),
                                                         June_Qty = r.JunQty.ToString(),
                                                         June_Pur = r.JunSale.ToString(),
                                                         July_Qty = r.JulQty.ToString(),
                                                         July_Pur = r.JulSale.ToString(),
                                                         August_Qty = r.AugQty.ToString(),
                                                         August_Pur = r.AugSale.ToString(),
                                                         Sep_Qty = r.SepQty.ToString(),
                                                         Sep_Pur = r.SepSale.ToString(),
                                                         Oct_Qty = r.OctQty.ToString(),
                                                         Oct_Pur = r.OctSale.ToString(),
                                                         Nov_Qty = r.NovQty.ToString(),
                                                         Nov_Pur = r.NovSale.ToString(),
                                                         Dec_Qty = r.DecQty.ToString(),
                                                         Dec_Pur = r.DecSale.ToString(),
                                                         TotalQty = r.TotalQty.ToString(),
                                                         TotalPurchase = r.TotalSale.ToString()
                                                     }

                                          ).ToList();
                        }
                        else if (PartyCode == "0" && SupplierCode != "0")
                        {
                            objListPurchaseReport = (from r in entity.V_MonthWiseSupplierPurchaseSummary
                                                     where r.SupplierCode == SupplierCode
                                                     select new PurchaseReport
                                                     {
                                                         SupplierCode = r.SupplierCode,
                                                         SupplierName = r.SupplierName,
                                                         PartyCode = r.PartyCode,
                                                         PartyName = r.PartyName,
                                                         Jan_Qty = r.JanQty.ToString(),
                                                         Jan_Pur = r.JanSale.ToString(),
                                                         Feb_Qty = r.FebQty.ToString(),
                                                         Feb_Pur = r.FebSale.ToString(),
                                                         March_Qty = r.MarQty.ToString(),
                                                         March_Pur = r.MarSale.ToString(),
                                                         April_Qty = r.AprQty.ToString(),
                                                         April_Pur = r.AprSale.ToString(),
                                                         May_Qty = r.MayQty.ToString(),
                                                         May_Pur = r.MaySale.ToString(),
                                                         June_Qty = r.JunQty.ToString(),
                                                         June_Pur = r.JunSale.ToString(),
                                                         July_Qty = r.JulQty.ToString(),
                                                         July_Pur = r.JulSale.ToString(),
                                                         August_Qty = r.AugQty.ToString(),
                                                         August_Pur = r.AugSale.ToString(),
                                                         Sep_Qty = r.SepQty.ToString(),
                                                         Sep_Pur = r.SepSale.ToString(),
                                                         Oct_Qty = r.OctQty.ToString(),
                                                         Oct_Pur = r.OctSale.ToString(),
                                                         Nov_Qty = r.NovQty.ToString(),
                                                         Nov_Pur = r.NovSale.ToString(),
                                                         Dec_Qty = r.DecQty.ToString(),
                                                         Dec_Pur = r.DecSale.ToString(),
                                                         TotalQty = r.TotalQty.ToString(),
                                                         TotalPurchase = r.TotalSale.ToString()
                                                     }

                                         ).ToList();
                        }
                        else if (PartyCode != "0" && SupplierCode == "0")
                        {
                            objListPurchaseReport = (from r in entity.V_MonthWiseSupplierPurchaseSummary
                                                     where r.PartyCode == PartyCode
                                                     select new PurchaseReport
                                                     {
                                                         SupplierCode = r.SupplierCode,
                                                         SupplierName = r.SupplierName,
                                                         PartyCode = r.PartyCode,
                                                         PartyName = r.PartyName,
                                                         Jan_Qty = r.JanQty.ToString(),
                                                         Jan_Pur = r.JanSale.ToString(),
                                                         Feb_Qty = r.FebQty.ToString(),
                                                         Feb_Pur = r.FebSale.ToString(),
                                                         March_Qty = r.MarQty.ToString(),
                                                         March_Pur = r.MarSale.ToString(),
                                                         April_Qty = r.AprQty.ToString(),
                                                         April_Pur = r.AprSale.ToString(),
                                                         May_Qty = r.MayQty.ToString(),
                                                         May_Pur = r.MaySale.ToString(),
                                                         June_Qty = r.JunQty.ToString(),
                                                         June_Pur = r.JunSale.ToString(),
                                                         July_Qty = r.JulQty.ToString(),
                                                         July_Pur = r.JulSale.ToString(),
                                                         August_Qty = r.AugQty.ToString(),
                                                         August_Pur = r.AugSale.ToString(),
                                                         Sep_Qty = r.SepQty.ToString(),
                                                         Sep_Pur = r.SepSale.ToString(),
                                                         Oct_Qty = r.OctQty.ToString(),
                                                         Oct_Pur = r.OctSale.ToString(),
                                                         Nov_Qty = r.NovQty.ToString(),
                                                         Nov_Pur = r.NovSale.ToString(),
                                                         Dec_Qty = r.DecQty.ToString(),
                                                         Dec_Pur = r.DecSale.ToString(),
                                                         TotalQty = r.TotalQty.ToString(),
                                                         TotalPurchase = r.TotalSale.ToString()
                                                     }

                                         ).ToList();
                        }
                        else
                        {
                            objListPurchaseReport = (from r in entity.V_MonthWiseSupplierPurchaseSummary
                                                     where r.PartyCode == PartyCode && r.SupplierCode == SupplierCode
                                                     select new PurchaseReport
                                                     {
                                                         SupplierCode = r.SupplierCode,
                                                         SupplierName = r.SupplierName,
                                                         PartyCode = r.PartyCode,
                                                         PartyName = r.PartyName,
                                                         Jan_Qty = r.JanQty.ToString(),
                                                         Jan_Pur = r.JanSale.ToString(),
                                                         Feb_Qty = r.FebQty.ToString(),
                                                         Feb_Pur = r.FebSale.ToString(),
                                                         March_Qty = r.MarQty.ToString(),
                                                         March_Pur = r.MarSale.ToString(),
                                                         April_Qty = r.AprQty.ToString(),
                                                         April_Pur = r.AprSale.ToString(),
                                                         May_Qty = r.MayQty.ToString(),
                                                         May_Pur = r.MaySale.ToString(),
                                                         June_Qty = r.JunQty.ToString(),
                                                         June_Pur = r.JunSale.ToString(),
                                                         July_Qty = r.JulQty.ToString(),
                                                         July_Pur = r.JulSale.ToString(),
                                                         August_Qty = r.AugQty.ToString(),
                                                         August_Pur = r.AugSale.ToString(),
                                                         Sep_Qty = r.SepQty.ToString(),
                                                         Sep_Pur = r.SepSale.ToString(),
                                                         Oct_Qty = r.OctQty.ToString(),
                                                         Oct_Pur = r.OctSale.ToString(),
                                                         Nov_Qty = r.NovQty.ToString(),
                                                         Nov_Pur = r.NovSale.ToString(),
                                                         Dec_Qty = r.DecQty.ToString(),
                                                         Dec_Pur = r.DecSale.ToString(),
                                                         TotalQty = r.TotalQty.ToString(),
                                                         TotalPurchase = r.TotalSale.ToString()
                                                     }

                                         ).ToList();
                        }



                    }
                    else
                    {
                        if (PartyCode == "0" && SupplierCode == "0")
                        {
                            objListPurchaseReport = (from r in entity.V_MonthWiseSupplierPurchaseSummary
                                                     where r.BillYear == YearInt
                                                     select new PurchaseReport
                                                     {
                                                         SupplierCode = r.SupplierCode,
                                                         SupplierName = r.SupplierName,
                                                         PartyCode = r.PartyCode,
                                                         PartyName = r.PartyName,
                                                         Jan_Qty = r.JanQty.ToString(),
                                                         Jan_Pur = r.JanSale.ToString(),
                                                         Feb_Qty = r.FebQty.ToString(),
                                                         Feb_Pur = r.FebSale.ToString(),
                                                         March_Qty = r.MarQty.ToString(),
                                                         March_Pur = r.MarSale.ToString(),
                                                         April_Qty = r.AprQty.ToString(),
                                                         April_Pur = r.AprSale.ToString(),
                                                         May_Qty = r.MayQty.ToString(),
                                                         May_Pur = r.MaySale.ToString(),
                                                         June_Qty = r.JunQty.ToString(),
                                                         June_Pur = r.JunSale.ToString(),
                                                         July_Qty = r.JulQty.ToString(),
                                                         July_Pur = r.JulSale.ToString(),
                                                         August_Qty = r.AugQty.ToString(),
                                                         August_Pur = r.AugSale.ToString(),
                                                         Sep_Qty = r.SepQty.ToString(),
                                                         Sep_Pur = r.SepSale.ToString(),
                                                         Oct_Qty = r.OctQty.ToString(),
                                                         Oct_Pur = r.OctSale.ToString(),
                                                         Nov_Qty = r.NovQty.ToString(),
                                                         Nov_Pur = r.NovSale.ToString(),
                                                         Dec_Qty = r.DecQty.ToString(),
                                                         Dec_Pur = r.DecSale.ToString(),
                                                         TotalQty = r.TotalQty.ToString(),
                                                         TotalPurchase = r.TotalSale.ToString()
                                                     }

                                          ).ToList();
                        }
                        else if (PartyCode == "0" && SupplierCode != "0")
                        {
                            objListPurchaseReport = (from r in entity.V_MonthWiseSupplierPurchaseSummary
                                                     where r.BillYear == YearInt && r.SupplierCode == SupplierCode
                                                     select new PurchaseReport
                                                     {
                                                         SupplierCode = r.SupplierCode,
                                                         SupplierName = r.SupplierName,
                                                         PartyCode = r.PartyCode,
                                                         PartyName = r.PartyName,
                                                         Jan_Qty = r.JanQty.ToString(),
                                                         Jan_Pur = r.JanSale.ToString(),
                                                         Feb_Qty = r.FebQty.ToString(),
                                                         Feb_Pur = r.FebSale.ToString(),
                                                         March_Qty = r.MarQty.ToString(),
                                                         March_Pur = r.MarSale.ToString(),
                                                         April_Qty = r.AprQty.ToString(),
                                                         April_Pur = r.AprSale.ToString(),
                                                         May_Qty = r.MayQty.ToString(),
                                                         May_Pur = r.MaySale.ToString(),
                                                         June_Qty = r.JunQty.ToString(),
                                                         June_Pur = r.JunSale.ToString(),
                                                         July_Qty = r.JulQty.ToString(),
                                                         July_Pur = r.JulSale.ToString(),
                                                         August_Qty = r.AugQty.ToString(),
                                                         August_Pur = r.AugSale.ToString(),
                                                         Sep_Qty = r.SepQty.ToString(),
                                                         Sep_Pur = r.SepSale.ToString(),
                                                         Oct_Qty = r.OctQty.ToString(),
                                                         Oct_Pur = r.OctSale.ToString(),
                                                         Nov_Qty = r.NovQty.ToString(),
                                                         Nov_Pur = r.NovSale.ToString(),
                                                         Dec_Qty = r.DecQty.ToString(),
                                                         Dec_Pur = r.DecSale.ToString(),
                                                         TotalQty = r.TotalQty.ToString(),
                                                         TotalPurchase = r.TotalSale.ToString()
                                                     }

                                         ).ToList();
                        }
                        else if (PartyCode != "0" && SupplierCode == "0")
                        {
                            objListPurchaseReport = (from r in entity.V_MonthWiseSupplierPurchaseSummary
                                                     where r.BillYear == YearInt && r.PartyCode == PartyCode
                                                     select new PurchaseReport
                                                     {
                                                         SupplierCode = r.SupplierCode,
                                                         SupplierName = r.SupplierName,
                                                         PartyCode = r.PartyCode,
                                                         PartyName = r.PartyName,
                                                         Jan_Qty = r.JanQty.ToString(),
                                                         Jan_Pur = r.JanSale.ToString(),
                                                         Feb_Qty = r.FebQty.ToString(),
                                                         Feb_Pur = r.FebSale.ToString(),
                                                         March_Qty = r.MarQty.ToString(),
                                                         March_Pur = r.MarSale.ToString(),
                                                         April_Qty = r.AprQty.ToString(),
                                                         April_Pur = r.AprSale.ToString(),
                                                         May_Qty = r.MayQty.ToString(),
                                                         May_Pur = r.MaySale.ToString(),
                                                         June_Qty = r.JunQty.ToString(),
                                                         June_Pur = r.JunSale.ToString(),
                                                         July_Qty = r.JulQty.ToString(),
                                                         July_Pur = r.JulSale.ToString(),
                                                         August_Qty = r.AugQty.ToString(),
                                                         August_Pur = r.AugSale.ToString(),
                                                         Sep_Qty = r.SepQty.ToString(),
                                                         Sep_Pur = r.SepSale.ToString(),
                                                         Oct_Qty = r.OctQty.ToString(),
                                                         Oct_Pur = r.OctSale.ToString(),
                                                         Nov_Qty = r.NovQty.ToString(),
                                                         Nov_Pur = r.NovSale.ToString(),
                                                         Dec_Qty = r.DecQty.ToString(),
                                                         Dec_Pur = r.DecSale.ToString(),
                                                         TotalQty = r.TotalQty.ToString(),
                                                         TotalPurchase = r.TotalSale.ToString()
                                                     }

                                         ).ToList();
                        }
                        else
                        {
                            objListPurchaseReport = (from r in entity.V_MonthWiseSupplierPurchaseSummary
                                                     where r.BillYear == YearInt && r.PartyCode == PartyCode && r.SupplierCode == SupplierCode
                                                     select new PurchaseReport
                                                     {
                                                         SupplierCode = r.SupplierCode,
                                                         SupplierName = r.SupplierName,
                                                         PartyCode = r.PartyCode,
                                                         PartyName = r.PartyName,
                                                         Jan_Qty = r.JanQty.ToString(),
                                                         Jan_Pur = r.JanSale.ToString(),
                                                         Feb_Qty = r.FebQty.ToString(),
                                                         Feb_Pur = r.FebSale.ToString(),
                                                         March_Qty = r.MarQty.ToString(),
                                                         March_Pur = r.MarSale.ToString(),
                                                         April_Qty = r.AprQty.ToString(),
                                                         April_Pur = r.AprSale.ToString(),
                                                         May_Qty = r.MayQty.ToString(),
                                                         May_Pur = r.MaySale.ToString(),
                                                         June_Qty = r.JunQty.ToString(),
                                                         June_Pur = r.JunSale.ToString(),
                                                         July_Qty = r.JulQty.ToString(),
                                                         July_Pur = r.JulSale.ToString(),
                                                         August_Qty = r.AugQty.ToString(),
                                                         August_Pur = r.AugSale.ToString(),
                                                         Sep_Qty = r.SepQty.ToString(),
                                                         Sep_Pur = r.SepSale.ToString(),
                                                         Oct_Qty = r.OctQty.ToString(),
                                                         Oct_Pur = r.OctSale.ToString(),
                                                         Nov_Qty = r.NovQty.ToString(),
                                                         Nov_Pur = r.NovSale.ToString(),
                                                         Dec_Qty = r.DecQty.ToString(),
                                                         Dec_Pur = r.DecSale.ToString(),
                                                         TotalQty = r.TotalQty.ToString(),
                                                         TotalPurchase = r.TotalSale.ToString()
                                                     }

                                         ).ToList();
                        }
                    }



                }
            }
            catch (Exception ex)
            {

            }
            return objListPurchaseReport;
        }

        public List<string> GetYearList()
        {
            List<string> objYearLists = new List<string>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    var Result = (from r in entity.V_MonthWiseSupplierPurchaseSummary
                                  select r.BillYear

                                ).OrderBy(m => m.Value).ToList();
                    if (Result.Count() > 0)
                    {
                        var FirstYear = Result.First();
                        var LastYear = Result.Last();

                        var RemainingYears = LastYear - FirstYear;
                        if (RemainingYears > 0)
                        {
                            objYearLists.Add(FirstYear.ToString());
                            for (int i = 1; i <= RemainingYears; i++)
                            {
                                objYearLists.Add((FirstYear + i).ToString());
                            }
                        }
                        else
                        {
                            objYearLists.Add(FirstYear.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objYearLists;
        }
        public List<string> GetSalesYearList()
        {
            List<string> objYearLists = new List<string>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    var Result = (from r in entity.V_MonthWiseSaleSummary
                                  select r.BillYear

                                ).OrderBy(m => m.Value).ToList();
                    if (Result.Count() > 0)
                    {
                        var FirstYear = Result.First();
                        var LastYear = Result.Last();

                        var RemainingYears = LastYear - FirstYear;
                        if (RemainingYears > 0)
                        {
                            objYearLists.Add(FirstYear.ToString());
                            for (int i = 1; i <= RemainingYears; i++)
                            {
                                objYearLists.Add((FirstYear + i).ToString());
                            }
                        }
                        else
                        {
                            objYearLists.Add(FirstYear.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objYearLists;
        }

        public List<SalesReport> GetMonthWiseSalesSummary(string Year, bool IsQuantity, bool IsAmount, string PartyCode)
        {
            List<SalesReport> objSalesReport = new List<SalesReport>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    int YearInt = 0;
                    if (!string.IsNullOrEmpty(Year))
                    {
                        YearInt = int.Parse(Year);
                    }
                    objSalesReport = (from r in entity.V_MonthWiseSaleSummary
                                      select new SalesReport
                                      {

                                          PartyCode = r.PartyCode,
                                          PartyName = r.PartyName,
                                          Jan_Qty = r.JanQty.ToString(),
                                          Jan_Sales = r.JanSale.ToString(),
                                          Feb_Qty = r.FebQty.ToString(),
                                          Feb_Sales = r.FebSale.ToString(),
                                          March_Qty = r.MarQty.ToString(),
                                          March_Sales = r.MarSale.ToString(),
                                          April_Qty = r.AprQty.ToString(),
                                          April_Sales = r.AprSale.ToString(),
                                          May_Qty = r.MayQty.ToString(),
                                          May_Sales = r.MaySale.ToString(),
                                          June_Qty = r.JunQty.ToString(),
                                          June_Sales = r.JunSale.ToString(),
                                          July_Qty = r.JulQty.ToString(),
                                          July_Sales = r.JulSale.ToString(),
                                          August_Qty = r.AugQty.ToString(),
                                          August_Sales = r.AugSale.ToString(),
                                          Sep_Qty = r.SepQty.ToString(),
                                          Sep_Sales = r.SepSale.ToString(),
                                          Oct_Qty = r.OctQty.ToString(),
                                          Oct_Sales = r.OctSale.ToString(),
                                          Nov_Qty = r.NovQty.ToString(),
                                          Nov_Sales = r.NovSale.ToString(),
                                          Dec_Qty = r.DecQty.ToString(),
                                          Dec_Sales = r.DecSale.ToString(),
                                          TotalQty = r.TotalQty.ToString(),
                                          TotalSales = r.TotalSale.ToString(),
                                          BillYear = r.BillYear ?? 0,

                                      }

                                          ).ToList();
                    if (Year != "0")
                    {
                        objSalesReport = objSalesReport.Where(m => m.BillYear == YearInt).ToList();
                    }
                    if (PartyCode != "0" && PartyCode != "All")
                    {

                        objSalesReport = objSalesReport.Where(m => m.PartyCode == PartyCode).ToList();
                    }

                }


            }
            catch (Exception ex)
            {

            }
            return objSalesReport;
        }
        public List<SelectListItem> GetStateList()
        {
            List<SelectListItem> objStateList = new List<SelectListItem>();
            try
            {
                string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(AppConnectionString);

                string query = "Select * from M_StateDivMaster where RowStatus='Y' AND ActiveStatus='Y'";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();
                List<StateModel> M_StateDivMasterList = new List<StateModel>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        M_StateDivMasterList.Add(new StateModel
                        {
                            StateCode = decimal.Parse(reader["StateCode"].ToString()),
                            StateName = reader["StateName"].ToString()
                        });
                    }
                }
                using (var entity = new InventoryEntities())
                {
                    objStateList = (from r in M_StateDivMasterList

                                    select new SelectListItem
                                    {
                                        Value = r.StateCode.ToString(),
                                        Text = r.StateName
                                    }

                                  ).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return objStateList;
        }
        public List<StockReportModel> GetStockReceiptReport(string CategoryCode, string ProductCode, string PartyCode, string StateCode, string FromDate, string ToDate, string LoginPartyCode, string isSummary)
        {
            List<StockReportModel> objStockModel = new List<StockReportModel>();
            decimal CatId = 0;
            string ProdCode = "0";
            decimal StCode = 0;
            string PCode = "All";
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;
            try
            {

                string db = System.Configuration.ConfigurationManager.AppSettings["Database"];
                string dbInv = System.Configuration.ConfigurationManager.AppSettings["INVDatabase"];


                if (!string.IsNullOrEmpty(CategoryCode))
                {
                    CatId = decimal.Parse(CategoryCode);
                }
                if (!string.IsNullOrEmpty(ProductCode))
                {
                    ProdCode = ProductCode;
                    if (ProductCode == "0")
                    {
                        ProdCode = "All";
                    }
                }
                if (!string.IsNullOrEmpty(StateCode))
                {
                    StCode = decimal.Parse(StateCode);
                }
                if (!string.IsNullOrEmpty(PartyCode))
                {
                    PCode = PartyCode;
                    if (PartyCode == "0")
                    {
                        PCode = "All";
                    }

                }

                string Sql = "";
                string WhereCondition = "";
                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConnectionString);
                SqlCommand cmd = new SqlCommand();

                using (var entity = new InventoryEntities())
                {
                    bool IsAdmin = false;
                    if (!string.IsNullOrEmpty(LoginPartyCode))
                    {
                        IsAdmin = (from r in entity.Inv_M_UserMaster where r.BranchCode == LoginPartyCode select r.IsAdmin).FirstOrDefault() == "Y" ? true : false;
                    }
                    if (!string.IsNullOrEmpty(FromDate) && (!string.IsNullOrEmpty(ToDate)))
                    {
                        if (FromDate != "All")
                        {
                            //var sqlFromdate = FromDate.Split('-');
                            //StartDate = new DateTime(Convert.ToInt16(sqlFromdate[2]), Convert.ToInt16(sqlFromdate[1]), Convert.ToInt16(sqlFromdate[0]));
                            var SplitFromDate = FromDate.Split('-');
                            FromDate = SplitFromDate[1] + "-" + SplitFromDate[0] + "-" + SplitFromDate[2];
                            CultureInfo provider = CultureInfo.InvariantCulture;
                            var NewDate1 = DateTime.ParseExact(FromDate, new string[] { "MM.dd.yyyy", "MM-dd-yyyy", "MM/dd/yyyy" }, provider, DateTimeStyles.None);
                            // string NewDate1 = Convert.ToDateTime(DateTime.ParseExact(FromDate, "MM/dd/yyyy", new CultureInfo("en-US", true)));
                            // StartDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                            StartDate = Convert.ToDateTime(NewDate1);
                        }
                        if (ToDate != "All")
                        {
                            //var sqlFromdate = ToDate.Split('-');
                            //EndDate = new DateTime(Convert.ToInt16(sqlFromdate[2]), Convert.ToInt16(sqlFromdate[1]), Convert.ToInt16(sqlFromdate[0]));
                            var SplitToDate = ToDate.Split('-');
                            ToDate = SplitToDate[1] + "-" + SplitToDate[0] + "-" + SplitToDate[2];
                            CultureInfo provider = CultureInfo.InvariantCulture;
                            var NewDate1 = DateTime.ParseExact(ToDate, new string[] { "MM.dd.yyyy", "MM-dd-yyyy", "MM/dd/yyyy" }, provider, DateTimeStyles.None);

                            // var NewDate1 = Convert.ToDateTime(DateTime.ParseExact(ToDate, "MM/dd/yyyy", System.Globalization.CultureInfo.GetCultureInfo("en-US")));
                            EndDate = Convert.ToDateTime(NewDate1);
                        }

                        //if (PCode == "All")
                        //{
                        //    if (IsAdmin)
                        //    {

                        //    }
                        //    else
                        //    {
                        //        WhereCondition = WhereCondition + " AND c.FCode='" + LoginPartyCode + "'";
                        //    }
                        //}
                        //else
                        //{
                        //   WhereCondition = " AND a.FCode='" + PCode + "'";
                        //}
                        if (PCode != "0" && PCode != "All")
                        {
                            WhereCondition = " AND a.FCode='" + PCode + "'";
                        }

                        if (ProdCode == "All")
                        {
                            WhereCondition = WhereCondition + "";
                        }
                        else
                        {
                            WhereCondition = WhereCondition + " And b.ProdID='" + ProdCode + "'";
                        }

                        if (CatId == 0)
                        {
                            WhereCondition = WhereCondition + "";
                        }
                        else
                        {
                            WhereCondition = WhereCondition + " And b.CatID='" + CatId + "'";
                        }

                        //to add date range in last after result - pending;
                        string NewFromDate = StartDate.Date.ToString("dd-MMM-yyyy");
                        string NewToDate = EndDate.Date.ToString("dd-MMM-yyyy");
                        if (FromDate != "All" && ToDate != "All")
                        {
                            WhereCondition = WhereCondition + " and a.BillDate>='" + NewFromDate + "' and a.BillDate<='" + NewToDate + "'";
                        }
                        else if (FromDate != "All" && ToDate == "All")
                        {
                            WhereCondition = WhereCondition + " and a.BillDate>='" + NewFromDate + "'";
                        }
                        else if (FromDate == "All" && ToDate != "All")
                        {
                            WhereCondition = WhereCondition + "and a.BillDate<='" + NewToDate + "'";
                        }
                        else
                        {

                        }


                        if (StCode > 0)
                        {
                            WhereCondition = WhereCondition + " And d.StateCode=" + StCode;
                        }

                        if (isSummary.ToLower() == "s")
                        {
                            Sql = "Select '' as BillNo,'' as BillDate, a.FCode, '' As PartyName, a.ProductId,us.UserName, a.ProductName,Sum(a.Qty) as Qty,b.Dp as Rate,0 As TaxPer,Sum(a.TaxAmount+a.CGSTAmt+a.SGSTAmt) as TaxAmount,Sum(A.NetAmount) As Amount, Sum(a.NetAmount + A.TaxAmount + a.CGSTAmt + a.SGSTAmt) as NetPayable, '' as STNNo, '' as BillNo, '' As PartyType, d.PartyCode + ' - ' + d.PartyName as SoldPartyName, St.StateName";
                            Sql = Sql + " From (Select UserID, BranchCode, Username FROM(Select ROW_NUMBER() OVER(PARTITION BY BranchCode ORDER BY UserID) rwno, * FROM Inv_M_UserMaster  WHERE ActiveStatus = 'Y') a WHERE rwno = 1) as us, TrnBillDetails as a, M_ProductMaster as b, TrnBillMain as c, M_LedgerMaster as d, " + db + "..M_StateDivMaster as St";
                            Sql = Sql + " Where c.SoldBy = d.PartyCode And a.BillNo = c.BillNo And a.ProductId = b.ProdId AND d.StateCode = St.StateCode AND us.BranchCode = c.FCode ";
                            Sql = Sql + WhereCondition;
                            Sql = Sql + "Group By us.UserName, a.FCode, a.ProductId, a.ProductName, b.Dp, d.PartyCode + ' - ' + d.PartyName, St.StateName Order By a.ProductId";
                        }
                        else if (isSummary.ToLower() == "d")
                        {
                            Sql = "Select c.GRNo,Replace(Convert(varchar,c.StkRecvDate,106),' ','-') as GRDate,c.FCode,c.PartyName,a.ProductId ,a.ProductName,Sum(a.Qty) as Qty,a.Rate as Rate,a.Tax+a.CGST+a.SGST as TaxPer,Sum(a.TaxAmount +a.CGSTAmt+a.SGSTAmt) as TaxAmount,Sum(a.NetAmount) as Amount,Sum(a.NetAmount)+Sum(a.TaxAmount) as NetPayable,c.UserBillNo as STNo,c.BillNo,Case When c.Ftype='M' then 'Distributor' else Case When c.FType='GC' then 'Customer' else Case When c.Ftype Not in('M','GC') then 'Party' end end end as PartyType,d.PartyCode + ' - ' + d.PartyName as SoldPartyName,St.StateName,us.UserName ";
                            Sql = Sql + " From (Select UserID, BranchCode, Username FROM(Select ROW_NUMBER() OVER(PARTITION BY BranchCode ORDER BY UserID) rwno, * FROM Inv_M_UserMaster  WHERE ActiveStatus = 'Y') a WHERE rwno = 1) as us, TrnBillDetails as a,TrnBillMain as c,M_ProductMaster as b ,M_LedgerMaster as d," + db + "..M_StateDivMaster as St where  c.SoldBy=d.PartyCode And a.BillNo=c.BillNo AND d.StateCode=St.StateCode And a.ProductId=b.ProdId AND us.BranchCode=c.FCode  ";
                            Sql = Sql + WhereCondition;
                            Sql = Sql + " Group By c.GRNo,c.StkRecvDate,a.ProductId,a.ProductName,a.Tax+a.CGST+a.SGST,a.Rate,us.UserName,c.FCode,c.PartyName,c.UserBillNo,c.FType,d.PartyCode + ' - ' + d.PartyName,c.BillNo,St.StateName Order By c.StkRecvDate desc, c.GRNo desc,c.FType,a.ProductId,c.Fcode";
                        }
                        else
                        {
                            Sql = "Select c.BillNo,c.GRNo,Replace(Convert(varchar,c.StkRecvDate,106),' ','-') as GRDate,c.BillDate, c.FCode, c.PartyName, '' ProductId,us.UserName, '' ProductName,Sum(a.Qty) as Qty,0 Rate,0 As TaxPer,Sum(a.TaxAmount+a.CGSTAmt+a.SGSTAmt) as TaxAmount,Sum(A.NetAmount) As Amount, c.NetPayable, c.UserBillNo as STNo, '' as BillNo, '' As PartyType, d.PartyCode + ' - ' + d.PartyName as SoldPartyName, St.StateName " +
" From(Select UserID, BranchCode, Username FROM(Select ROW_NUMBER() OVER(PARTITION BY BranchCode ORDER BY UserID) rwno, *FROM Inv_M_UserMaster  WHERE ActiveStatus = 'Y') a WHERE rwno = 1)" +
" as us, trnBillDetails a, M_ProductMaster as b, TrnBillMain as c, M_LedgerMaster as d, " + db + "..M_StateDivMaster as St" +
" Where c.SoldBy = d.PartyCode And a.BillNo = c.BillNo And a.ProductId = b.ProdId AND d.StateCode = St.StateCode AND us.BranchCode = c.FCode" + WhereCondition +
" Group By c.BillNo,c.BillDate, c.FCode, c.PartyName,c.NetPayable, us.UserName,d.PartyCode + ' - ' + d.PartyName, St.StateName,Replace(Convert(varchar,c.StkRecvDate,106),' ','-'),c.UserBillNo,c.GRNo" +
" Order By c.BillNo,c.BillDate";
                        }

                        cmd.CommandText = Sql;
                        cmd.Connection = SC;
                        SC.Close();
                        SC.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StockReportModel tempobj = new StockReportModel();
                                tempobj.ProductCode = reader["ProductId"] != null ? reader["ProductId"].ToString() : "";
                                tempobj.ProductName = reader["ProductName"] != null ? reader["ProductName"].ToString() : "";
                                tempobj.PartyCode = reader["FCode"] != null ? reader["FCode"].ToString() : "";
                                tempobj.PartyName = reader["PartyName"] != null ? reader["PartyName"].ToString() : "";
                                tempobj.RateOrDP = reader["Rate"] != null ? reader["Rate"].ToString() : "";
                                tempobj.Qty = reader["Qty"] != null ? reader["Qty"].ToString() : "";
                                tempobj.TaxPer = reader["TaxPer"] != null ? reader["TaxPer"].ToString() : "";
                                tempobj.TaxAmt = reader["TaxAmount"] != null ? reader["TaxAmount"].ToString() : "";
                                tempobj.BasicAmt = reader["Amount"] != null ? reader["Amount"].ToString() : "";
                                tempobj.TotalAmt = reader["NetPayable"] != null ? reader["NetPayable"].ToString() : "";
                                tempobj.SoldBy = reader["SoldPartyName"] != null ? reader["SoldPartyName"].ToString() : "";
                                tempobj.StateName = reader["StateName"] != null ? reader["StateName"].ToString() : "";
                                tempobj.UserName = reader["UserName"] != null ? reader["UserName"].ToString() : "";
                                if (isSummary.ToLower() != "s")
                                {
                                    tempobj.StnNo = reader["STNo"] != null ? reader["STNo"].ToString() : "";
                                    tempobj.StrNo = reader["GRNo"] != null ? reader["GRNo"].ToString() : "";
                                    tempobj.StrDate = reader["GRDate"] != null ? reader["GRDate"].ToString() : "";
                                    if (tempobj.StrDate != "")
                                    {
                                        DateTime StrDate = Convert.ToDateTime(tempobj.StrDate);
                                        tempobj.StrDate = StrDate.ToString("dd/MM/yyyy");
                                    }
                                }
                                else
                                {
                                    tempobj.StnNo = "";
                                    tempobj.StrNo = "";
                                    tempobj.StrDate = "";
                                }
                                objStockModel.Add(tempobj);


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objStockModel;
        }

        public List<PartyWiseWalletDetails> GetPartyWiseWalletReport(string FromDate, string ToDate, string PartyCode, string ViewType)
        {
            List<PartyWiseWalletDetails> objPartyWalletDetails = new List<PartyWiseWalletDetails>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    var AllPartyDetails = new List<PartyWiseWalletDetails>();

                    AllPartyDetails = (from r in entity.TrnVouchers
                                       where r.ActiveStatus == "Y" && r.VType == "R"
                                       from p in entity.M_LedgerMaster
                                       where p.PartyCode == r.CrTo || p.PartyCode == r.DrTo
                                       select new PartyWiseWalletDetails
                                       {
                                           PartyCode = p.PartyCode,
                                           PartyName = p.PartyName,
                                           CrTo = r.CrTo,
                                           DrTo = r.DrTo,
                                           CrAmt = r.Amount.ToString(),
                                           DrAmt = r.Amount.ToString(),
                                           Narration = r.Narration,
                                           Balance = "0",
                                           TransDate = r.VoucherDate
                                       }
                                               ).Distinct().ToList();


                    if (PartyCode != "All" && PartyCode != "0")
                    {
                        AllPartyDetails = AllPartyDetails.Where(m => m.PartyCode.ToUpper() == PartyCode.ToUpper()).ToList();
                    }
                    DateTime StartDate = DateTime.Now.Date;
                    DateTime EndDate = DateTime.Now.Date;
                    if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                    {
                        var SplitDate = FromDate.Split('-');
                        string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];
                        StartDate = Convert.ToDateTime(NewDate);
                        StartDate = StartDate.Date;
                    }
                    if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                    {
                        var SplitDate = ToDate.Split('-');
                        string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];
                        EndDate = Convert.ToDateTime(NewDate);
                        EndDate = EndDate.Date;
                    }
                    if (FromDate != "All" && ToDate != "All")
                    {
                        AllPartyDetails = AllPartyDetails.Where(m => m.TransDate.Date >= StartDate.Date && m.TransDate.Date <= EndDate.Date).ToList();
                    }
                    else if (FromDate == "All" && ToDate != "All")
                    {
                        AllPartyDetails = AllPartyDetails.Where(m => m.TransDate.Date <= EndDate.Date).ToList();
                    }
                    else if (FromDate != "All" && ToDate == "All")
                    {
                        AllPartyDetails = AllPartyDetails.Where(m => m.TransDate.Date >= StartDate.Date).ToList();
                    }
                    decimal previousBalance = 0;
                    foreach (var obj in AllPartyDetails)
                    {
                        PartyWiseWalletDetails tempObj = new PartyWiseWalletDetails();
                        tempObj = obj;
                        if (obj.CrTo != "0")
                        {
                            tempObj.DrAmt = "0";
                            tempObj.DrAmtD = 0;
                            tempObj.CrAmtD = decimal.Parse(obj.CrAmt);
                            tempObj.Balance = (previousBalance + decimal.Parse(obj.CrAmt)).ToString();
                        }
                        else
                        {
                            tempObj.CrAmt = "0";
                            tempObj.CrAmtD = 0;
                            tempObj.DrAmtD = decimal.Parse(obj.DrAmt);
                            tempObj.Balance = (previousBalance - decimal.Parse(obj.DrAmt)).ToString();
                        }

                        previousBalance = decimal.Parse(tempObj.Balance);


                        objPartyWalletDetails.Add(tempObj);
                    }
                    if (ViewType == "Balance")
                    {
                        objPartyWalletDetails = (from obj in objPartyWalletDetails
                                                 group obj by new { obj.PartyCode, obj.PartyName } into grouped
                                                 select new PartyWiseWalletDetails
                                                 {
                                                     PartyCode = grouped.Key.PartyCode,
                                                     PartyName = grouped.Key.PartyName,
                                                     CrAmt = grouped.Sum(m => m.CrAmtD).ToString(),
                                                     DrAmt = grouped.Sum(m => m.DrAmtD).ToString(),
                                                     Balance = (grouped.Sum(m => m.CrAmtD) - grouped.Sum(m => m.DrAmtD)).ToString()
                                                 }).ToList();
                    }
                    //DashboardAPIController objDashboardApi = new DashboardAPIController();




                }
            }
            catch (Exception ex)
            {

            }
            return objPartyWalletDetails;
        }


        public List<SaleRegister> GetSaleRegisterReport(string FromDate, string ToDate, string PartyCode)
        {
            List<SaleRegister> objReport = new List<SaleRegister>();
            string WhereCondition = string.Empty;
            string Fld = string.Empty;
            var dataTable = new DataTable();

            try
            {
                DateTime StartDate = new DateTime();
                DateTime EndDate = new DateTime();

                if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                {
                    var SplitDate = FromDate.Split('-');
                    string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                    StartDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    StartDate = StartDate.Date;
                }
                if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                {
                    var SplitDate = ToDate.Split('-');
                    string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                    EndDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    EndDate = EndDate.Date;
                }

                string NewFromDate = StartDate.Date.ToString("dd-MMM-yyyy");
                string NewToDate = EndDate.Date.ToString("dd-MMM-yyyy");

                string wherea = string.Empty;
                string whereb = string.Empty;
                string wheree = string.Empty;

                if (!string.IsNullOrEmpty(FromDate) && FromDate.ToUpper() != "ALL")
                {
                    wherea += " and a.BillDate>='" + NewFromDate + "'";
                    whereb += " and b.BillDate>='" + NewFromDate + "'";
                    wheree += " and e.BillDate>='" + NewFromDate + "'";

                }

                if (!string.IsNullOrEmpty(ToDate) && ToDate.ToUpper() != "ALL")
                {
                    wherea = wherea + " and a.BillDate <='" + NewToDate + "'";
                    whereb = whereb + " and b.BillDate <='" + NewToDate + "'";
                    wheree = wheree + " and e.BillDate <='" + NewToDate + "'";

                }

                if (!string.IsNullOrEmpty(PartyCode) && PartyCode.ToUpper() != "ALL" && PartyCode.ToUpper() != "0")
                {
                    wherea = wherea + " and a.SoldBy='" + PartyCode.Trim() + "' ";
                    whereb = whereb + " and b.SoldBy='" + PartyCode.Trim() + "' ";
                    wheree = wheree + " and e.SoldBy='" + PartyCode.Trim() + "' ";

                }



                string TaxPerCondi0_ = " and (a.Tax=0 AND a.CGST=0 AND a.SGST=0) ";
                string TaxPerCondi1_ = " and (a.Tax=5 OR a.CGST=2.5 OR a.SGST=2.5) ";
                string TaxPerCondi2_ = " and (b.Tax=12 OR b.CGST=6 OR b.SGST=6) ";
                string TaxPerCondi3_ = " and (b.Tax=18 OR b.CGST=9 OR b.SGST=9) ";
                string TaxPerCondi4_ = " and (e.Tax=28 OR e.CGST=14 OR e.SGST=14) ";
                string TaxPerCondi5_ = " and (b.Tax=3 OR b.CGST=1.5 OR b.SGST=1.5) ";
                string db = System.Configuration.ConfigurationManager.AppSettings["Database"];

                string sql = " Select M.BillNo,M.Billdate,M.PartyCode as Code,M.PartyName,ISNULL(L.CSTNo,'') as GSTIN,M.[ExemptSale],M.Discount,st.StateName,NetAmount_3 as [Basic 3%] ,IGST_3 as [IGST@3%],CGST_3 as [CGST@1.5%],SGST_3 as [SGST@1.5%],NetAmount_5 as [Basic 5%] ,IGST_5 as [IGST@5%],CGST_5 as [CGST@2.5%],SGST_5 as [CGST @2.5%],NetAmount_12 as [Basic 12%],IGST_12 [IGST @12%],CGST_12 [CGST @6%],SGST_12 as [SGST @6%],NetAmount_18 as [Basic for 18%],IGST_18 [IGST @18%] ,CGST_18 [CGST @9%],SGST_18 [SGST @9%],NetAmount_28 [Basic 28%],IGST_28 [IGST @28%],CGST_28 [CGST @14%],SGST_28 [SGST @14%],TotalAmount as [Total Amt.],TotalIGSTAmt as [Total IGST],TotalCGSTAmt  [Total CGST],TotalSGSTAmt  [Total SGST],Rndoff as [Rnd.off],InvoiceAmt as [Bill Amount] FROM (";
                sql += " Select d.UserBillNo as BillNo,Replace(Convert(varchar, d.Billdate, 106), ' ', '-') as Billdate,d.Rndoff,d.PartyCode,d.PartyName,Sum(ExemptSale) as ExemptSale,Sum(d.Discount) as Discount,Sum(NetAmount_3) as NetAmount_3 ,Sum(IGST_3) as IGST_3,SUM(CGST_3) as CGST_3,SUM(SGST_3) as SGST_3,Sum(NetAmount_5) as NetAmount_5 ,Sum(IGST_5) as IGST_5,SUM(CGST_5) as CGST_5,SUM(SGST_5) as SGST_5,SUM(NetAmount_12) as 'NetAmount_12',SUM(IGST_12) as 'IGST_12',SUM(CGST_12) as CGST_12,SUM(SGST_12) as SGST_12,SUM(NetAmount_18) as 'NetAmount_18',SUM(IGST_18) as 'IGST_18',SUM(CGST_18) as CGST_18,SUM(SGST_18) as SGST_18,SUM(NetAmount_28) as 'NetAmount_28',SUM(IGST_28) as 'IGST_28',SUM(CGST_28) as CGST_28,SUM(SGST_28) as SGST_28,Sum(ExemptSale) + Sum(NetAmount_3) + Sum(NetAmount_5) + Sum(NetAmount_12) + Sum(NetAmount_18) + SUM(NetAmount_28) as TotalAmount,Sum(IGST_3) + Sum(IGST_5) + Sum(IGST_12) + Sum(IGST_18) + Sum(IGST_28) as TotalIGSTAmt,Sum(CGST_3) +Sum(CGST_5) + Sum(CGST_12) + Sum(CGST_18) + Sum(CGST_28) as TotalCGSTAmt,+ Sum(SGST_3) +Sum(SGST_5) + Sum(SGST_12) + Sum(SGST_18) + Sum(SGST_28) as TotalSGSTAmt,NetPayable as InvoiceAmt";
                sql += " from((";
                sql += " Select a.UserSBillNo,a.UserBillNo,a.Rndoff,a.NetPayable,a.BillDate,a.PartyCode,a.PartyName,a.Discount,a.NetAmount as ExemptSale ,0 as 'NetAmount_3',0 as 'IGST_3',0 as CGST_3,0 as SGST_3,0 as NetAmount_5,0 as IGST_5,0 as CGST_5,0 as SGST_5,0 as NetAmount_12,0 as IGST_12,0 as CGST_12,0 as SGST_12,0 as NetAmount_18,0 as IGST_18,0 as CGST_18,0 as SGST_18,0 as 'NetAmount_28',0 as 'IGST_28',0 as CGST_28,0 as SGST_28";
                sql += " from( Select b.UserSBillNo,b.UserBillNo,b.Rndoff,b.NetPayable,a.BillDate,b.FCode as PartyCode,b.PartyName+' - '+b.FCode as PartyName,a.Tax,Sum(a.Discount) as Discount,Sum(a.NetAmount) as NetAmount,0 as CGSTAmt,0 as SGSTAmt,0 as TaxAmount from TrnBillDetails as a,TrnBillMain as b where 1=1 " + TaxPerCondi0_ + wherea + " and a.BillNo=b.BillNo Group By b.UserSBillNo,b.UserBillNo,b.Rndoff,b.NetPayable,a.BillDate,a.Tax,b.PartyName,b.FCode ) as a ";

                sql += " Union Select b.UserSBillNo,b.UserBillNo,b.Rndoff,b.NetPayable,b.BillDate,b.PartyCode,b.PartyName,b.Discount,0 ,NetAmount as 'NetAmount_3',TaxAmount as 'IGST_3',CGSTAmt as CGST_3,SGSTAmt as SGST_3,0 as 'NetAmount_5',0 as 'IGST_5',0 as CGST_5,0 as SGST_5,0 as 'NetAmount_12',0 as 'IGST_12',0 as CGST_12,0 as SGST_12,0 as NetAmount_18,0 as 'IGST_18',0 as CGST_18,0 as SGST_18,0 as 'NetAmount_28',0 as 'IGST_28',0 as CGST_28,0 as SGST_28";
                sql += " from(Select c.UserSBillNo,c.UserBillNo,c.Rndoff,c.NetPayable,b.BillDate,c.FCode as PartyCode,c.PartyName+' - '+c.FCode as PartyName,b.Tax,Sum(b.Discount) as Discount,Sum(b.NetAmount) as NetAmount,Sum(b.CGSTAmt) as CGSTAmt,Sum(b.SGSTAmt) as SGSTAmt,Sum(b.TaxAmount) as TaxAmount  from TrnBillDetails as b,TrnBillMain as c where 1=1 " + TaxPerCondi5_ + whereb + " and b.BillNo=c.BillNo Group By c.UserSBillNo,c.UserBillNo,c.NetPayable,c.Rndoff,b.BillDate,c.PartyName,b.Tax,b.CGST,b.SGST,c.FCode) as b";

                sql += " UNION Select a.UserSBillNo,a.UserBillNo,a.Rndoff,a.NetPayable,a.BillDate,a.PartyCode,a.PartyName,a.Discount,0,0 as 'NetAmount_3',0 as 'IGST_3',0 as CGST_3,0 as SGST_3,a.NetAmount as 'NetAmount_5',a.TaxAmount as 'IGST_5',a.CGSTAmt as CGST_5,a.SGSTAmt as SGST_5,0 as 'NetAmount_12',0 as 'IGST_12',0 as CGST_12,0 as SGST_12,0 as NetAmount_18,0 as 'IGST_18',0 as CGST_18,0 as SGST_18,0 as 'NetAmount_28',0 as 'IGST_28',0 as CGST_28,0 as SGST_28";
                sql += " from(Select b.UserSBillNo,b.UserBillNo,b.Rndoff,b.NetPayable,a.BillDate,b.FCode as PartyCode,b.PartyName+' - '+b.FCode as PartyName,a.Tax,Sum(a.Discount) as Discount,Sum(a.NetAmount) as NetAmount,Sum(a.CGSTAmt) as CGSTAmt,Sum(a.SGSTAmt) as SGSTAmt,Sum(a.TaxAmount) as TaxAmount  from TrnBillDetails as a,TrnBillMain as b where 1=1 " + TaxPerCondi1_ + wherea + " and a.BillNo=b.BillNo Group By b.UserSBillNo,b.UserBillNo,b.NetPayable,b.Rndoff,a.BillDate,b.PartyName,a.Tax,a.CGST,a.SGST,b.FCode ) as a ";
                sql += " Union Select b.UserSBillNo,b.UserBillNo,b.Rndoff,b.NetPayable,b.BillDate,b.PartyCode,b.PartyName,b.Discount,0,0 as 'NetAmount_3',0 as 'IGST_3',0 as CGST_3,0 as SGST_3,0 as 'NetAmount_5',0 as 'IGST_5',0 as CGST_5,0 as SGST_5,NetAmount as 'NetAmount_12',TaxAmount as 'IGST_12',CGSTAmt as CGST_12,SGSTAmt as SGST_12,0 as NetAmount_18,0 as 'IGST_18',0 as CGST_18,0 as SGST_18,0 as 'NetAmount_28',0 as 'IGST_28',0 as CGST_28,0 as SGST_28";
                sql += " from(Select c.UserSBillNo,c.UserBillNo,c.Rndoff,c.NetPayable,b.BillDate,c.FCode as PartyCode,c.PartyName+' - '+c.FCode as PartyName,b.Tax,Sum(b.Discount) as Discount,Sum(b.NetAmount) as NetAmount,Sum(b.CGSTAmt) as CGSTAmt,Sum(b.SGSTAmt) as SGSTAmt,Sum(b.TaxAmount) as TaxAmount  from TrnBillDetails as b,TrnBillMain as c where 1=1 " + TaxPerCondi2_ + whereb + " and b.BillNo=c.BillNo Group By c.UserSBillNo,c.UserBillNo,c.NetPayable,c.Rndoff,b.BillDate,c.PartyName,b.Tax,b.CGST,b.SGST,c.FCode) as b";
                sql += " Union Select c.UserSBillNo,c.UserBillNo,c.Rndoff,c.NetPayable,c.BillDate,c.PartyCode,c.PartyName,c.Discount,0,0 as 'NetAmount_3',0 as 'IGST_3',0 as CGST_3,0 as SGST_3,0 as 'NetAmount_5',0 as 'IGST_5',0 as CGST_5,0 as SGST_5,0 as 'NetAmount_12',0 as 'IGST_12',0 as CGST_12,0 as SGST_12,NetAmount as NetAmount_18,TaxAmount as IGST_18,CGSTAmt as CGST_18,SGSTAmt as SGST_18,0 as 'NetAmount_28',0 as 'IGST_28',0 as CGST_28,0 as SGST_28";
                sql += " from (Select c.UserSBillNo,c.UserBillNo,c.Rndoff,c.NetPayable,b.BillDate,c.FCode as PartyCode,c.PartyName+' - '+c.FCode as PartyName,b.Tax,Sum(b.Discount) as Discount,Sum(b.NetAmount) as NetAmount,Sum(b.CGSTAmt) as CGSTAmt,Sum(b.SGSTAmt) as SGSTAmt,Sum(b.TaxAmount) as TaxAmount from TrnBillDetails as b,TrnBillMain as c where 1=1 " + TaxPerCondi3_ + whereb + " and b.BillNo=c.BillNo Group By c.UserSBillNo,c.UserBillNo,c.NetPayable,c.Rndoff,b.BillDate,c.PartyName,b.Tax,b.CGST,b.SGST,c.FCode) as c";
                sql += " Union Select e.UserSBillNo,e.UserBillNo,e.Rndoff,e.NetPayable,e.BillDate,e.PartyCode,e.PartyName,e.Discount,0,0 as 'NetAmount_3',0 as 'IGST_3',0 as CGST_3,0 as SGST_3,0 as 'NetAmount_5',0 as 'IGST_5',0 as CGST_5,0 as SGST_5,0 as 'NetAmount_12',0 as 'IGST_12',0 as CGST_12,0 as SGST_12,0 as 'NetAmount_18',0 as 'IGST_18',0 as CGST_18,0 as SGST_18,e.NetAmount as 'NetAmount_28',e.TaxAmount as 'IGST_28',e.CGSTAmt as 'CGST_28',e.SGSTAmt as 'SGST_28' ";
                sql += " from (Select f.UserSBillNo,f.UserBillNo,f.Rndoff,f.NetPayable,e.BillDate,e.FCode as PartyCode,f.PartyName+' - '+e.FCode as PartyName,e.Tax,Sum(e.Discount) as Discount,Sum(e.NetAmount) as NetAmount,Sum(e.CGSTAmt) as CGSTAmt,Sum(e.SGSTAmt) as SGSTAmt,Sum(e.TaxAmount) as TaxAmount from TrnBillDetails as e,TrnBillMain as f where 1=1 " + TaxPerCondi4_ + wheree + " and e.BillNo=f.BillNo Group By f.UserSBillNo,f.UserBillNo,f.NetPayable,f.Rndoff,e.BillDate,f.PartyName,e.Tax,e.CGST,e.SGST,e.FCode)as e)) as d  WHERE Cast(d.BillDate as Datetime)>='1-Jul-2017' Group By UserSBillNo,UserBillNo,Rndoff,BillDate,PartyCode,PartyName,NetPayable) as M LEFT JOIN M_LedgerMaster as L On M.PartyCode=L.PartyCode  LEFT JOIN " + db + "..M_StateDivMaster st on (L.StateCode=st.StateCode )  Order By M.BillNo,M.BillDate";

                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConnectionString);
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = sql;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SaleRegister obj = new SaleRegister();
                        obj.Basic_12 = reader["Basic 12%"] != null ? Convert.ToDecimal(reader["Basic 12%"]) : 0;
                        obj.Basic_28 = reader["Basic 28%"] != null ? Convert.ToDecimal(reader["Basic 28%"]) : 0;
                        obj.Basic_5 = reader["Basic 5%"] != null ? Convert.ToDecimal(reader["Basic 5%"]) : 0;
                        obj.Basic_for_18 = reader["Basic for 18%"] != null ? Convert.ToDecimal(reader["Basic for 18%"]) : 0;
                        obj.BillAmount = reader["Bill Amount"] != null ? Convert.ToDecimal(reader["Bill Amount"]) : 0;
                        obj.Billdate = reader["Billdate"] != null ? Convert.ToString(reader["Billdate"]) : "";
                        obj.BillNo = reader["BillNo"] != null ? Convert.ToString(reader["BillNo"]) : "";

                        obj.Basic_3 = reader["Basic 3%"] != null ? Convert.ToDecimal(reader["Basic 3%"]) : 0;
                        obj.CGST_15 = reader["CGST@1.5%"] != null ? Convert.ToDecimal(reader["CGST@1.5%"]) : 0;
                        obj.SGST_15 = reader["SGST@1.5%"] != null ? Convert.ToDecimal(reader["SGST@1.5%"]) : 0;
                        obj.IGST_3 = reader["IGST@3%"] != null ? Convert.ToDecimal(reader["IGST@3%"]) : 0;

                        obj.CGST1_25 = reader["CGST@2.5%"] != null ? Convert.ToDecimal(reader["CGST@2.5%"]) : 0;
                        obj.CGST2_25 = reader["CGST @2.5%"] != null ? Convert.ToDecimal(reader["CGST @2.5%"]) : 0;
                        obj.CGST_14 = reader["CGST @14%"] != null ? Convert.ToDecimal(reader["CGST @14%"]) : 0;
                        obj.CGST_6 = reader["CGST @6%"] != null ? Convert.ToDecimal(reader["CGST @6%"]) : 0;
                        obj.CGST_9 = reader["CGST @9%"] != null ? Convert.ToDecimal(reader["CGST @9%"]) : 0;
                        obj.Code = reader["Code"] != null ? Convert.ToString(reader["Code"]) : "";
                        obj.Discount = reader["Discount"] != null ? Convert.ToDecimal(reader["Discount"]) : 0;
                        obj.ExemptSale = reader["ExemptSale"] != null ? Convert.ToDecimal(reader["ExemptSale"]) : 0;
                        obj.GSTIN = reader["GSTIN"] != null ? Convert.ToString(reader["GSTIN"]) : "";
                        obj.IGST_12 = reader["IGST @12%"] != null ? Convert.ToDecimal(reader["IGST @12%"]) : 0;
                        obj.IGST_18 = reader["IGST @18%"] != null ? Convert.ToDecimal(reader["IGST @18%"]) : 0;
                        obj.IGST_28 = reader["IGST @28%"] != null ? Convert.ToDecimal(reader["IGST @28%"]) : 0;
                        obj.IGST_5 = reader["IGST@5%"] != null ? Convert.ToDecimal(reader["IGST@5%"]) : 0;
                        obj.PartyName = reader["PartyName"] != null ? Convert.ToString(reader["PartyName"]) : "";
                        obj.StateName = reader["StateName"] != null ? Convert.ToString(reader["StateName"]) : "";
                        obj.RndOff = reader["Rnd.off"] != null ? Convert.ToDecimal(reader["Rnd.off"]) : 0;
                        obj.SGST_14 = reader["SGST @14%"] != null ? Convert.ToDecimal(reader["SGST @14%"]) : 0;
                        obj.SGST_6 = reader["SGST @6%"] != null ? Convert.ToDecimal(reader["SGST @6%"]) : 0;
                        obj.SGST_9 = reader["SGST @9%"] != null ? Convert.ToDecimal(reader["SGST @9%"]) : 0;
                        obj.TotalAmt = reader["Total Amt."] != null ? Convert.ToDecimal(reader["Total Amt."]) : 0;
                        obj.TotalCGST = reader["Total CGST"] != null ? Convert.ToDecimal(reader["Total CGST"]) : 0;
                        obj.TotalIGST = reader["Total IGST"] != null ? Convert.ToDecimal(reader["Total IGST"]) : 0;
                        obj.TotalSGST = reader["Total SGST"] != null ? Convert.ToDecimal(reader["Total SGST"]) : 0;
                        objReport.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objReport;
        }

        public List<PaymentSummaryReport> GetPaymentSummaryReport(string FromDate, string ToDate, string PartyCode, string Type)
        {
            List<PaymentSummaryReport> objReport = new List<PaymentSummaryReport>();
            string WhereCondition = string.Empty;
            string Fld = string.Empty;
            string Fldorder = string.Empty;
            try
            {
                DateTime StartDate = new DateTime();
                DateTime EndDate = new DateTime();
                StartDate = DateTime.Now.AddMonths(-1);
                EndDate = DateTime.Now;
                if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                {
                    var SplitDate = FromDate.Split('-');
                    string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                    StartDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    StartDate = StartDate.Date;
                }
                if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                {
                    var SplitDate = ToDate.Split('-');
                    string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                    EndDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    EndDate = EndDate.Date;
                }
                string NewFromDate = StartDate.Date.ToString("dd-MMM-yyyy");
                string NewToDate = EndDate.Date.ToString("dd-MMM-yyyy");

                if (!string.IsNullOrEmpty(PartyCode) && PartyCode.ToUpper() != "0")
                {
                    WhereCondition = WhereCondition + " and SoldBy ='" + PartyCode + "'";
                }

                if (!string.IsNullOrEmpty(FromDate) && FromDate.ToUpper() != "ALL")
                {
                    WhereCondition = WhereCondition + " and BillDate >='" + NewFromDate + "'";
                }

                if (!string.IsNullOrEmpty(ToDate) && ToDate.ToUpper() != "ALL")
                {
                    WhereCondition = WhereCondition + " and BillDate <='" + NewToDate + "'";
                }

                if (Type == "B")
                {
                    Fld = "BillNo,BillDate, SoldBy,SoldByName,OrderNo, FCode,PartyName,VoucherAmt";
                    Fldorder = "BillDate";
                }
                else if (Type == "D")
                {
                    Fld = "BillDate,SoldBy,SoldByName,VoucherAmt";
                    Fldorder = "BillDate";
                }
                else if (Type == "P")
                {
                    Fld = "SoldBy,SoldByName,VoucherAmt";
                    Fldorder = "SoldByName";
                }
                string sql = "";
                if (Type == "M")
                {
                    if (!string.IsNullOrEmpty(PartyCode) && PartyCode.ToUpper() != "0")
                        WhereCondition = " and SoldBy ='" + PartyCode + "'";

                    sql = "Select SoldBy,SoldByName,MnthName,BillAmt,VDiscountAmt FPVAmt,NetAmt,ExcessAmt,NetPayable,CashAmt as C,ChqAmt as Q,DDAmt as D,CreditCardAmt as CC,BankDeposit as BD,WalletAmt as W,DebitCardAmt as DB,NetBanking as NB,Credit as T, PVPurchaseWallet, BVPurchaseWallet,VoucherAmt FROM V#MonthWisePaymodeSummary WHERE 1=1" + WhereCondition + " ORDER BY PARSE('01-' + MnthName AS DATE USING 'en-US') DESC";

                }
                else
                    //sql = "Select " + Fld + ",Sum(BillAmt) as BillAmt,SUM(IIF(BillType='F',BillAmt,0)) as FPVAmt,SUM(IIF(BillType='F',0,BillAmt)) as NetAmt,SUM(IIF(BillType='F',NetPayable,0)) as ExcessAmt,SUM(NetPayable) NetPayable,Sum(CashAmt) as C,Sum(ChqAmt) as Q,Sum(DDAmt) as D,Sum(CreditCardAmt) as CC,Sum(BankDeposit) as BD,Sum(WalletAmt) as W,Sum(DebitCardAmt) as DB,Sum(NetBanking) as NB,Sum(Credit) as T FROM V#PaymentModeWiseDetail WHERE 1=1" + WhereCondition + " GROUP BY " + Fld  order by billdate desc;
                    sql = "SELECT " + Fld + ",SUM(BillAmt) AS BillAmt,SUM(IIF(BillType='F',BillAmt,0)) AS FPVAmt,SUM(IIF(BillType='F',0,BillAmt)) AS NetAmt,SUM(IIF(BillType='F',NetPayable,0)) AS ExcessAmt,SUM(NetPayable) AS NetPayable,SUM(CashAmt) AS C,SUM(ChqAmt) AS Q,SUM(DDAmt) AS D,SUM(CreditCardAmt) AS CC,SUM(BankDeposit) AS BD,SUM(WalletAmt) AS W,SUM(DebitCardAmt) AS DB,SUM(NetBanking) AS NB,SUM(Credit) AS T,sum(PVPurchaseWallet) as PVPurchaseWallet,sum(BVPurchaseWallet) as BVPurchaseWallet,VoucherAmt FROM V#PaymentModeWiseDetail WHERE 1=1 " + WhereCondition + " GROUP BY " + Fld + " ORDER BY " + Fldorder + " DESC;";
                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PaymentSummaryReport tempobj = new PaymentSummaryReport();

                        tempobj.BillNo = "";
                        tempobj.BillDate = "";
                        tempobj.IDNo = "";
                        tempobj.IdName = "";
                        tempobj.Order = "";

                        if (Type == "B")
                        {
                            tempobj.BillNo = reader["BillNo"] != null ? reader["BillNo"].ToString() : "";
                            tempobj.BillDate = reader["BillDate"] != null ? Convert.ToDateTime(reader["BillDate"]).ToString("dd/MM/yyyy") : "";
                            tempobj.IDNo = reader["FCode"] != null ? reader["FCode"].ToString() : "";
                            tempobj.IdName = reader["PartyName"] != null ? reader["PartyName"].ToString() : "";
                            tempobj.Order = reader["OrderNo"] != null ? reader["OrderNo"].ToString() : "";

                        }
                        else if (Type == "D")
                        {
                            tempobj.BillDate = reader["BillDate"] != null ? Convert.ToDateTime(reader["BillDate"]).ToString("dd/MM/yyyy") : "";
                        }
                        else if (Type == "M")
                            tempobj.BillDate = reader["MnthName"] != null ? reader["MnthName"].ToString() : "";

                        tempobj.Name = reader["SoldByName"] != null ? reader["SoldByName"].ToString() : "";
                        tempobj.BillBy = reader["SoldBy"] != null ? reader["SoldBy"].ToString() : "";

                        tempobj.FPVAmt = reader["FPVAmt"] != null ? reader["FPVAmt"].ToString() : "";
                        tempobj.NetAmt = reader["NetAmt"] != null ? reader["NetAmt"].ToString() : "";
                        tempobj.ExcessAmt = reader["ExcessAmt"] != null ? reader["ExcessAmt"].ToString() : "";
                        tempobj.NetPayable = reader["NetPayable"] != null ? reader["NetPayable"].ToString() : "";

                        tempobj.Amount = reader["BillAmt"] != null ? reader["BillAmt"].ToString() : "";
                        tempobj.Cash = reader["C"] != null ? reader["C"].ToString() : "";
                        tempobj.Cheque = reader["Q"] != null ? reader["Q"].ToString() : "";
                        tempobj.dd = reader["D"] != null ? reader["D"].ToString() : "";
                        tempobj.CreditCard = reader["CC"] != null ? reader["CC"].ToString() : "";
                        tempobj.BankDeposit = reader["BD"] != null ? reader["BD"].ToString() : "";
                        tempobj.DeditCard = reader["DB"] != null ? reader["DB"].ToString() : "";
                        tempobj.NetBanking = reader["NB"] != null ? reader["NB"].ToString() : "";
                        tempobj.Credit = reader["T"] != null ? reader["T"].ToString() : "";
                        tempobj.Wallet = reader["W"] != null ? reader["W"].ToString() : "";


                        tempobj.PVPurchaseWallet = Convert.ToString(reader["PVPurchaseWallet"]);
                        tempobj.BVPurchaseWallet = Convert.ToString(reader["BVPurchaseWallet"]);
                        tempobj.VoucherAmt = Convert.ToString(reader["VoucherAmt"]);
                        objReport.Add(tempobj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objReport;
        }

        public List<PaymentMode> GetPaymodeList()
        {
            List<PaymentMode> paymode = new List<PaymentMode>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    paymode = (from r in entity.M_PayModeMaster
                               where r.IsShow == "Y"
                               select new PaymentMode
                               {
                                   payMode = r.PayMode,
                                   prefix = r.Prefix
                               }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return (paymode);
        }

        public List<MonthlySumm> GetMonthlyReport(string PartyCode, string BillType)
        {
            List<MonthlySumm> objSumm = new List<MonthlySumm>();
            try
            {
                using (var db = new InventoryEntities())
                {
                    objSumm = (from r in db.MonthWiseSummary(PartyCode, BillType)
                               select new MonthlySumm
                               {
                                   Mnth = r.Mnth,
                                   NetPayable = r.NetPayable,
                                   PvValue = r.PVValue
                               }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return objSumm;
        }

        public List<SalesReturnReport> GetPurchaseReturnReport(string FromDate, string ToDate, string ProductCode, string CategoryCode, string SupplierCode, string Type)
        {
            var report = new List<SalesReturnReport>();
            try
            {
                DateTime StartDate = DateTime.Now.Date;
                DateTime EndDate = DateTime.Now.Date;
                if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                {
                    var SplitDate = FromDate.Split('-');
                    string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];
                    StartDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    StartDate = StartDate.Date;
                }
                if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                {
                    var SplitDate = ToDate.Split('-');
                    string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];
                    EndDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    EndDate = EndDate.Date;
                }

                string NewFromDate = StartDate.Date.ToString("dd-MMM-yyyy");
                string NewToDate = EndDate.Date.ToString("dd-MMM-yyyy");

                string WhereCondition = string.Empty;

                if (!string.IsNullOrEmpty(FromDate) && FromDate.ToUpper() != "ALL")
                {
                    WhereCondition = WhereCondition + " and c.ReturnDate>='" + NewFromDate + "'";
                }
                if (!string.IsNullOrEmpty(ToDate) && ToDate.ToUpper() != "ALL")
                {
                    WhereCondition = WhereCondition + " and c.ReturnDate<='" + NewToDate + "'";
                }
                if (!string.IsNullOrEmpty(ProductCode) && ProductCode.ToUpper() != "0")
                {
                    WhereCondition = WhereCondition + " and a.ProdId ='" + ProductCode + "'";
                }
                if (!string.IsNullOrEmpty(CategoryCode) && CategoryCode.ToUpper() != "0")
                {
                    WhereCondition = WhereCondition + " and b.CatId = '" + CategoryCode + "'";
                }
                if (!string.IsNullOrEmpty(SupplierCode) && SupplierCode.ToUpper() != "0")
                {
                    WhereCondition = WhereCondition + " and a.ReturnTo ='" + SupplierCode + "'";
                }


                string Sql = string.Empty;


                if (Type.ToLower() == "detail")
                {
                    Sql = "Select c.ReturnNo,Replace(Convert(varchar,c.ReturnDate,106),' ','-') as GRDate,c.ReturnBy,c.ReturnByName,a.ProdId ,a.ProductName,Sum(a.ReturnQty) as Qty,a.Rate as Rate,a.Tax as TaxPer,Sum(a.TaxAmount) as TaxAmount,Sum(a.Amount) as Amount,";
                    Sql = Sql + "Sum(a.Amount)+Sum(a.TaxAmount) as NetPayable,c.OrderNo as STNo,c.ReturnNo,'' as PartyType,ReturnToName,'' StateName";
                    Sql = Sql + " From TrnPurchaseReturnDetail as a,TrnPurchaseReturnMain as c,M_ProductMaster as b where  a.ReturnNo=c.ReturnNo And a.ProdId=b.ProdId";
                    Sql = Sql + WhereCondition;
                    Sql = Sql + " Group By c.ReturnNo,c.ReturnDate,a.ProdId,a.ProductName,a.Tax,a.Rate,c.ReturnBy,c.ReturnByName,ReturnToName,c.OrderNo Order By a.ProdId,c.ReturnBy";
                }

                else
                {
                    Sql = "Select '' as ReturnNo,'' as GRDate, a.ReturnBy, '' As PartyName,'' As ReturnByName,'' As STNo, a.ProdId,c.ReturnToName,";
                    Sql = Sql + " a.ProductName,Sum(a.ReturnQty) as Qty,b.Dp as Rate,0 As TaxPer,Sum(a.TaxAmount) as TaxAmount, Sum(A.Amount) As Amount,";
                    Sql = Sql + " Sum(a.Amount+A.TaxAmount) as NetPayable,'' as STNNo,'' as BillNo, '' As PartyType,d.PartyCode + ' - ' + d.PartyName as SoldPartyName,'' StateName ";
                    Sql = Sql + " From TrnPurchaseReturnDetail as a,M_ProductMaster as b,TrnPurchaseReturnMain as c ,M_LedgerMaster as d ";
                    Sql = Sql + " Where c.ReturnTo=d.PartyCode And a.ReturnNo=c.ReturnNo And a.ProdId=b.ProdId ";
                    Sql = Sql + WhereCondition;
                    Sql = Sql + " Group By a.ReturnBy,c.ReturnDate,c.ReturnToName,a.ProdId,a.ProductName,b.Dp,d.PartyCode + ' - ' + d.PartyName Order By a.ProdId";
                }

                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = Sql;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SalesReturnReport tempobj = new SalesReturnReport();

                        tempobj.ProductCode = reader["ProdId"] != null ? reader["ProdId"].ToString() : "";
                        tempobj.ProductName = reader["ProductName"] != null ? reader["ProductName"].ToString() : "";
                        tempobj.STRNo = reader["ReturnNo"] != null ? reader["ReturnNo"].ToString() : "";
                        tempobj.STRDate = reader["GRDate"] != null ? reader["GRDate"].ToString() : "";
                        tempobj.ReturnBy = reader["ReturnBy"] != null ? reader["ReturnBy"].ToString() : "";
                        tempobj.ReturnByName = reader["ReturnByName"] != null ? reader["ReturnByName"].ToString() : "";
                        tempobj.Quantity = reader["Qty"] != null ? reader["Qty"].ToString() : "0";
                        tempobj.BasicAmt = reader["Amount"] != null ? reader["Amount"].ToString() : "0";
                        tempobj.TaxAmt = reader["TaxAmount"] != null ? reader["TaxAmount"].ToString() : "0";
                        tempobj.Tax = reader["TaxPer"] != null ? reader["TaxPer"].ToString() : "0";
                        tempobj.TotalAmt = reader["NetPayable"] != null ? reader["NetPayable"].ToString() : "";
                        tempobj.Rate = reader["Rate"] != null ? reader["Rate"].ToString() : "";
                        tempobj.ReturnTo = reader["ReturnToName"] != null ? reader["ReturnToName"].ToString() : "";
                        tempobj.BillNo = reader["STNo"] != null ? reader["STNo"].ToString() : "";
                        report.Add(tempobj);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return report;
        }

        public List<SalesReport> GetWalletHistory(string FromDate, string ToDate, string PartyCode, string vtype)
        {
            DateTime StartDate = DateTime.Now.AddYears(-5);
            DateTime EndDate = DateTime.Now;
            if (!string.IsNullOrEmpty(FromDate) && (!string.IsNullOrEmpty(ToDate)))
            {
                if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                {
                    var SplitDate = FromDate.Split('-');
                    var NewDate = (SplitDate[1].Length == 1 ? "0" + SplitDate[1] : SplitDate[1]) + "/" + (SplitDate[0].Length == 1 ? "0" + SplitDate[0] : SplitDate[0]) + "/" + SplitDate[2];
                    StartDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    StartDate = StartDate.Date;
                }
                if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                {
                    var SplitDate = ToDate.Split('-');
                    var NewDate = (SplitDate[1].Length == 1 ? "0" + SplitDate[1] : SplitDate[1]) + "/" + (SplitDate[0].Length == 1 ? "0" + SplitDate[0] : SplitDate[0]) + "/" + SplitDate[2];
                    EndDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    EndDate = EndDate.Date;
                }
            }



            List<SalesReport> objWalletHistory = new List<SalesReport>();
            try
            {
                using (var entity = new InventoryEntities())
                {

                    var MobileShoppe = (from r in entity.ShoppeAssignments where r.PCId == PartyCode select r).FirstOrDefault();
                    if (MobileShoppe != null)
                    {
                        vtype = "M";
                    }

                    if (vtype != "M")
                    {
                        objWalletHistory = (from r in entity.WalletReport(PartyCode, vtype)
                                            where r.VoucherDateOnly >= StartDate && r.VoucherDateOnly <= EndDate
                                            orderby r.VoucherDate descending
                                            select new SalesReport
                                            {
                                                RecordDate = r.VoucherDate,
                                                Reason = r.Narration,
                                                TotalAmount = r.DepositedAmt,
                                                TaxAmount = r.UsedAmt,
                                                PartyCode = PartyCode,
                                                NetPayable = r.Balance.ToString()
                                            }).ToList();
                    }
                    else
                    {
                        objWalletHistory = (from r in entity.MobileShoppeWalletReport(PartyCode)
                                            where r.VoucherDateOnly >= StartDate && r.VoucherDateOnly <= EndDate
                                            orderby r.VoucherDate descending
                                            select new SalesReport
                                            {
                                                RecordDate = r.VoucherDate,
                                                Reason = r.Narration,
                                                TotalAmount = r.DepositedAmt,
                                                TaxAmount = r.UsedAmt,
                                                PartyCode = r.otherParty,
                                                NetPayable = r.Balance.ToString()
                                            }).ToList();
                    }
                    objWalletHistory = (from r in objWalletHistory
                                        select new SalesReport
                                        {
                                            RecordDate = r.RecordDate,
                                            StrBillDate = r.RecordDate.ToString("dd-MMM-yyyy"),
                                            Reason = r.Reason,
                                            TotalAmount = r.TotalAmount,
                                            TaxAmount = r.TaxAmount,
                                            PartyCode = PartyCode,
                                            NetPayable = r.NetPayable
                                        }).ToList();

                }
            }
            catch (Exception ex)
            {

            }
            return (objWalletHistory);
        }
        public List<SalesReturnReport> GetSalesReturnReport(string FromDate, string ToDate, string ProductCode, string CategoryCode, string PartyCode, string PartyType, string Type)
        {
            var report = new List<SalesReturnReport>();
            try
            {
                DateTime StartDate = DateTime.Now.Date;
                DateTime EndDate = DateTime.Now.Date;
                if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                {
                    var SplitDate = FromDate.Split('-');
                    string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                    StartDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    StartDate = StartDate.Date;
                }
                if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                {
                    var SplitDate = ToDate.Split('-');
                    string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                    EndDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    EndDate = EndDate.Date;
                }

                string NewFromDate = StartDate.Date.ToString("dd-MMM-yyyy");
                string NewToDate = EndDate.Date.ToString("dd-MMM-yyyy");

                string WhereCondition = string.Empty;

                if (!string.IsNullOrEmpty(FromDate) && FromDate.ToUpper() != "ALL")
                {
                    WhereCondition = WhereCondition + " and c.ReturnDate>='" + NewFromDate + "'";
                }
                if (!string.IsNullOrEmpty(ToDate) && ToDate.ToUpper() != "ALL")
                {
                    WhereCondition = WhereCondition + " and c.ReturnDate<='" + NewToDate + "'";
                }
                if (!string.IsNullOrEmpty(ProductCode) && ProductCode.ToUpper() != "0")
                {
                    WhereCondition = WhereCondition + " and a.ProdId ='" + ProductCode + "'";
                }
                if (!string.IsNullOrEmpty(CategoryCode) && CategoryCode.ToUpper() != "0")
                {
                    WhereCondition = WhereCondition + " and b.CatId = '" + CategoryCode + "'";
                }
                if (!string.IsNullOrEmpty(PartyCode) && PartyCode.ToUpper() != "0")
                {
                    WhereCondition = WhereCondition + " and a.ReturnBy ='" + PartyCode + "'";
                }
                if (!string.IsNullOrEmpty(PartyType) && PartyType.ToUpper() != "ALL")
                {
                    if (PartyType.ToLower() == "customer")
                    {
                        WhereCondition = WhereCondition + " and c.Ftype ='GC'";
                    }
                    else if (PartyType.ToLower() == "distributor")
                    {
                        WhereCondition = WhereCondition + " and c.Ftype ='M'";
                    }
                    else
                    {
                        WhereCondition = WhereCondition + " and c.Ftype Not in('M','GC')";
                    }
                }

                string Sql = string.Empty;
                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConnectionString);

                SqlCommand cmd = new SqlCommand();

                //if (Type.ToLower() == "detail")
                //{
                //    Sql = "Select c.SReturnNo,Replace(Convert(varchar,c.ReturnDate,106),' ','-') as GRDate,c.ReturnBy,c.ReturnByName,a.ProdId ,a.ProductName,Sum(a.ReturnQty) as Qty,a.Rate as Rate,a.Tax as TaxPer,Sum(a.TaxAmount) as TaxAmount,Sum(a.Amount) as Amount,";
                //    Sql = Sql + "Sum(a.Amount)+Sum(a.TaxAmount) as NetPayable,c.OrderNo as STNo,c.SReturnNo,Case When c.Ftype='M' then 'Distributor' else Case When c.FType='GC' then 'Customer' else Case When c.Ftype Not in('M','GC') then 'Party' end end end as PartyType,ReturnToName,'' StateName";
                //    Sql = Sql + " From TrnSalesReturnDetail as a,TrnSalesReturnMain as c,M_ProductMaster as b where  a.SReturnNo=c.SReturnNo And a.ProdId=b.ProdId";
                //    Sql = Sql + WhereCondition;
                //    Sql = Sql + " Group By c.SReturnNo,c.ReturnDate,a.ProdId,a.ProductName,a.Tax,a.Rate,c.ReturnBy,c.ReturnByName,c.FType,ReturnToName,c.OrderNo Order By c.FType,a.ProdId,c.ReturnBy";
                //}

                //else
                //{
                //    Sql = "Select '' as SReturnNo,'' as GRDate, a.ReturnBy, '' As PartyName,'' As ReturnByName,'' As STNo, a.ProdId,c.ReturnToName,";
                //    Sql = Sql + " a.ProductName,Sum(a.ReturnQty) as Qty,b.Dp as Rate,0 As TaxPer,Sum(a.TaxAmount) as TaxAmount, Sum(A.Amount) As Amount,";
                //    Sql = Sql + " Sum(a.Amount+A.TaxAmount) as NetPayable,'' as STNNo,'' as BillNo, '' As PartyType,d.PartyCode + ' - ' + d.PartyName as SoldPartyName,'' StateName ";
                //    Sql = Sql + " From TrnSalesReturnDetail as a,M_ProductMaster as b,TrnSalesReturnMain as c ,M_LedgerMaster as d ";
                //    Sql = Sql + " Where c.ReturnTo=d.PartyCode And a.SReturnNo=c.SReturnNo And a.ProdId=b.ProdId ";
                //    Sql = Sql + WhereCondition;
                //    Sql = Sql + " Group By a.ReturnBy,c.ReturnToName,a.ProdId,a.ProductName,b.Dp,d.PartyCode + ' - ' + d.PartyName Order By a.ProdId";
                //}

                Sql = "Select c.SReturnNo,Replace(Convert(varchar,c.ReturnDate,106),' ','-') as GRDate,c.ReturnBy,c.ReturnByName,Sum(a.ReturnQty) as Qty,Sum(a.ReturnQty *a.DP) as NetPayable,";
                Sql = Sql + "c.SReturnNo,ReturnToName,'' StateName,c.Remarks";
                Sql = Sql + " From TrnSalesReturnDetail as a,TrnSalesReturnMain as c,M_ProductMaster as b where  a.SReturnNo=c.SReturnNo And a.ProdId=b.ProdId";
                Sql = Sql + WhereCondition;
                Sql = Sql + " Group By c.SReturnNo,c.ReturnDate,c.ReturnBy,c.ReturnByName,ReturnToName,c.Remarks Order By c.ReturnDate desc";

                cmd.CommandText = Sql;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SalesReturnReport tempobj = new SalesReturnReport();

                        //tempobj.ProductCode = reader["ProdId"] != null ? reader["ProdId"].ToString() : "";
                        //tempobj.ProductName = reader["ProductName"] != null ? reader["ProductName"].ToString() : "";
                        tempobj.STRNo = reader["SReturnNo"] != null ? reader["SReturnNo"].ToString() : "";
                        tempobj.STRDate = reader["GRDate"] != null ? reader["GRDate"].ToString() : "";
                        tempobj.ReturnBy = reader["ReturnBy"] != null ? reader["ReturnBy"].ToString() : "";
                        tempobj.ReturnByName = reader["ReturnByName"] != null ? reader["ReturnByName"].ToString() : "";
                        tempobj.Quantity = reader["Qty"] != null ? reader["Qty"].ToString() : "0";
                        //tempobj.BasicAmt = reader["Amount"] != null ? reader["Amount"].ToString() : "0";
                        //tempobj.TaxAmt = reader["TaxAmount"] != null ? reader["TaxAmount"].ToString() : "0";
                        //tempobj.Tax = reader["TaxPer"] != null ? reader["TaxPer"].ToString() : "0";
                        tempobj.TotalAmt = reader["NetPayable"] != null ? string.Format("{0:0.00}", Convert.ToDouble(reader["NetPayable"])) : "0";
                        //tempobj.Rate = reader["Rate"] != null ? reader["Rate"].ToString() : "";
                        tempobj.ReturnTo = reader["ReturnToName"] != null ? reader["ReturnToName"].ToString() : "";
                        tempobj.BillNo = reader["Remarks"] != null ? reader["Remarks"].ToString() : "";
                        report.Add(tempobj);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return report;
        }

        public List<OfferReport> GetOfferReport(string PartyCode)
        {
            List<OfferReport> objOffers = new List<OfferReport>();
            try
            {
                using (var db = new InventoryEntities())
                {
                    objOffers = (from r in db.sp_OfferReport(PartyCode)
                                     // orderby r.OfferUID 'Cmnted on 23Mar19 (coz Order By added in procedure)
                                 select new OfferReport
                                 {
                                     OfferUID = r.OfferUID,
                                     OfferName = r.OfferName,
                                     FromDate = r.FromDate,
                                     FCode = r.SoldBy,
                                     PartyName = r.SoldByUserName,
                                     ToDate = r.ToDate,
                                     TotalSale = r.TotalSale,
                                     FreeQty = r.FreeQty,
                                     FreeProdValue = r.FreeProdValue,
                                     AmtCollected = r.AmtCollected,
                                     OfferValue = r.OfferValue,
                                     OfferType = r.OfferType
                                 }).ToList();
                }

                if (PartyCode.ToUpper() != "ALL")
                    objOffers = objOffers.Where(m => m.SoldBy.ToUpper() == PartyCode.ToUpper()).ToList();
            }
            catch (Exception)
            {
            }
            return objOffers;
        }

        public List<OfferReport> GetBillWiseOfferReport(decimal OfferID, string SoldBy)
        {
            List<OfferReport> objOffers = new List<OfferReport>();
            try
            {
                using (var db = new InventoryEntities())
                {
                    objOffers = (from r in db.V_BillWiseOfferSummary
                                 where r.OfferUID == OfferID
                                 select new OfferReport
                                 {
                                     OfferName = r.OfferName,
                                     OfferType = r.OfferType,
                                     FromDate = r.FromDate,
                                     ToDate = r.ToDate,
                                     UserBillNo = r.UserBillNo,
                                     BillDateStr = r.BillDateStr,
                                     FCode = r.FCode,
                                     PartyName = r.PartyName,
                                     TotalSale = r.TotalSale,
                                     SoldBy = r.SoldBy,
                                     FreeQty = r.FreeQty,
                                     FreeProdValue = r.FreeProdValue,
                                     AmtCollected = r.AmtCollected,
                                     OfferValue = r.OfferValue
                                 }).ToList();
                    if (SoldBy.ToUpper() != "ALL")
                        objOffers = objOffers.Where(m => m.SoldBy.ToUpper() == SoldBy.ToUpper()).ToList();
                }
            }
            catch (Exception)
            {
            }
            return objOffers;
        }

        public List<OfferReport> GetProdWiseOfferReport(decimal OfferID, string PartyCode)
        {
            List<OfferReport> objOffers = new List<OfferReport>();
            try
            {
                using (var db = new InventoryEntities())
                {
                    objOffers = (from r in db.sp_ProdWiseOfferReport(PartyCode, (int)OfferID)

                                 select new OfferReport
                                 {
                                     OfferName = r.OfferName,
                                     OfferType = r.OfferType,
                                     OfferRange = r.OfferRange,
                                     SoldBy = r.SoldBy,
                                     PartyName = r.SoldByUserName,
                                     ProdID = r.ProdID,
                                     ProductName = r.ProductName,
                                     UserProdID = r.UserProdID,
                                     FreeQty = r.FreeQty,
                                     FreeProdValue = r.FreeProdValue,
                                     AmtCollected = r.AmtCollected,
                                     OfferValue = r.OfferValue
                                 }).ToList();
                }
                if (PartyCode.ToUpper() != "ALL")
                    objOffers = objOffers.Where(m => m.SoldBy.ToUpper() == PartyCode.ToUpper()).ToList();
            }
            catch (Exception)
            {
            }
            return objOffers;
        }

        public List<StockReportModel> GetDateWiseStockReport(string CategoryCode, string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            List<StockReportModel> objStockModel = new List<StockReportModel>();
            decimal CatId = 0;
            decimal ProdCode = 0;
            try
            {

                DateTime startDate = DateTime.Now;
                try
                {
                    startDate = Convert.ToDateTime(FromDate);
                }
                catch (Exception)
                { }

                DateTime endDate = DateTime.Now;
                try
                {
                    endDate = Convert.ToDateTime(ToDate);
                }
                catch (Exception)
                { }
                if (!string.IsNullOrEmpty(ProductCode))
                {
                    ProdCode = decimal.Parse(ProductCode);
                }
                using (var entity = new InventoryEntities())
                {


                    objStockModel = (from r in entity.StockDetail(PartyCode, startDate, endDate)
                                         // where ((PartyCode != "0" && PartyCode != "All" && r.FCode == PartyCode) || PartyCode == "0" || PartyCode == "" || PartyCode == "All")
                                     orderby r.ProductName
                                     select new StockReportModel
                                     {
                                         PartyCode = r.FCode,
                                         PartyName = r.Username,
                                         ProductCode = r.ProdID,
                                         ProductName = r.ProductName,
                                         OpStock = r.OpStock,
                                         InStock = r.InStock,
                                         StockOut = r.StockOut,
                                         ClsStock = r.ClsStock,
                                         OpStockValue = r.OpStockValue ?? 0,
                                         InStockValue = r.InStockValue ?? 0,
                                         StockOutValue = r.StockOutValue ?? 0,
                                         ClsStockValue = r.ClsStockValue ?? 0
                                     }

                                           ).ToList();


                    if (ProdCode != 0)
                    {
                        objStockModel = objStockModel.Where(r => r.ProductCode == ProductCode).ToList();
                    }
                    //if (PartyCode != "0" && PartyCode != "All")
                    //{
                    //    objStockModel = objStockModel.Where(r => r.PartyCode == PartyCode).ToList();
                    //}
                }
            }
            catch (Exception ex)
            {

            }
            return objStockModel;
        }

        public List<StockReportModel> GetDailyStockReport(string CategoryCode, string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            List<StockReportModel> objStockModel = new List<StockReportModel>();
            decimal CatId = 0;
            decimal ProdCode = 0;
            try
            {
                DateTime startDate = DateTime.Now;
                try
                {
                    startDate = Convert.ToDateTime(FromDate);
                }
                catch (Exception)
                { }

                DateTime endDate = DateTime.Now;
                try
                {
                    endDate = Convert.ToDateTime(ToDate);
                }
                catch (Exception)
                { }
                string PartyCond = "";
                //  if (PartyCode != "" && PartyCode.ToUpper() != "ALL" && PartyCode != "0") PartyCond = " AND PartyCode='" + PartyCode + "'";
                string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(AppConnectionString);
                string query = "Select * FROM DailyStockDetail ('" + startDate.ToString("dd-MMM-yyyy") + "','" + endDate.ToString("dd-MMM-yyyy") + "','" + ProductCode + "','" + PartyCode + "')  WHERE (StockDateStr='" + startDate.ToString("dd-MMM-yyyy") + "' OR  StockDateStr='" + endDate.ToString("dd-MMM-yyyy") + "' OR InStock>0 OR StockOut>0  )" + PartyCond + " ORDER BY PartyCode,ProductName,StockDate";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        objStockModel.Add(new StockReportModel
                        {
                            PartyCode = reader["PartyCode"].ToString(),
                            PartyName = reader["UserName"].ToString(),
                            ProductCode = reader["ProdID"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            StockDateStr = reader["StockDateStr"].ToString(),
                            OpStock = Convert.ToDecimal(reader["OpStock"].ToString()),
                            InStock = Convert.ToDecimal(reader["InStock"].ToString()),
                            StockOut = Convert.ToDecimal(reader["StockOut"].ToString()),
                            ClsStock = Convert.ToDecimal(reader["ClsStock"].ToString()),
                            OpStockValue = Convert.ToDecimal(reader["OpStockValue"].ToString()),
                            InStockValue = Convert.ToDecimal(reader["InStockValue"].ToString()),
                            StockOutValue = Convert.ToDecimal(reader["StockOutValue"].ToString()),
                            ClsStockValue = Convert.ToDecimal(reader["ClsStockValue"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objStockModel;
        }

        public List<StockReportModel> GetDailyFrStockReport(string CategoryCode, string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            List<StockReportModel> objStockModel = new List<StockReportModel>();
            decimal CatId = 0;
            decimal ProdCode = 0;
            try
            {
                DateTime startDate = DateTime.Now;
                try
                {
                    startDate = Convert.ToDateTime(FromDate);
                }
                catch (Exception)
                { }

                DateTime endDate = DateTime.Now;
                try
                {
                    endDate = Convert.ToDateTime(ToDate);
                }
                catch (Exception)
                { }
                string PartyCond = "";
                //  if (PartyCode != "" && PartyCode.ToUpper() != "ALL" && PartyCode != "0") PartyCond = " AND PartyCode='" + PartyCode + "'";
                string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(AppConnectionString);
                string query = "Select * FROM FrWiseStockSumm ('" + PartyCode + "','" + startDate.ToString("dd-MMM-yyyy") + "','" + endDate.ToString("dd-MMM-yyyy") + "')  " + PartyCond + " ORDER BY PartyCode,StockDate";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        objStockModel.Add(new StockReportModel
                        {
                            PartyCode = reader["PartyCode"].ToString(),
                            PartyName = reader["UserName"].ToString(),
                            StockDateStr = reader["StockDateStr"].ToString(),
                            OpStock = Convert.ToDecimal(reader["StockQty"].ToString()),
                            InStock = Convert.ToDecimal(reader["StockValue"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objStockModel;
        }


        public List<IssueSampleProduct> GetSampleProductReport(string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            List<IssueSampleProduct> objSampleModel = new List<IssueSampleProduct>();
            List<IssueSampleProduct> objListSales = new List<IssueSampleProduct>();

            decimal CatId = 0;
            decimal ProdCode = 0;
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;
            try
            {

                if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                {
                    var SplitDate = FromDate.Split('-');
                    string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];
                    StartDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MMM/dd/yyyy", CultureInfo.InvariantCulture));
                    StartDate = StartDate.Date;
                }
                if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                {
                    var SplitDate = ToDate.Split('-');
                    string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];
                    EndDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MMM/dd/yyyy", CultureInfo.InvariantCulture));
                    EndDate = EndDate.Date;
                }
                string where = " where 1=1 ";
                if ((!string.IsNullOrEmpty(ProductCode)) && ProductCode != "All" && ProductCode != "0")
                {
                    where += " and ProdID = '" + ProductCode + "' ";
                }

                if ((!string.IsNullOrEmpty(PartyCode)) && PartyCode != "All" && PartyCode != "0")
                {
                    where += " and SoldBy = '" + PartyCode + "' ";
                }

                string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(AppConnectionString);
                string query = "Select TransNo,PartyName,RefNo,CAST(RecTimeStamp AS DATE) as RecTimeStamp,TransDate,Remarks,sum(Qty) as Qty FROM TrnSampleProducts " + where + " group by TransNo,PartyName,RefNo,CAST(RecTimeStamp AS DATE), TransDate,Remarks";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        objSampleModel.Add(new IssueSampleProduct
                        {
                            TransNo = reader["TransNo"].ToString(),
                            partyName = reader["PartyName"].ToString(),
                            RefNo = reader["RefNo"].ToString(),
                            IssueDate = Convert.ToDateTime(reader["RecTimeStamp"].ToString()),
                            IssueDateStr = Convert.ToDateTime(reader["RecTimeStamp"].ToString()).ToString("dd-MMM-yyyy"),
                            TransDate = Convert.ToDateTime(reader["TransDate"].ToString()),
                            TransDateStr = Convert.ToDateTime(reader["TransDate"].ToString()).ToString("dd-MMM-yyyy"),
                            Qty = Convert.ToInt16(reader["Qty"].ToString()),
                            Remark = reader["Remarks"].ToString(),
                            RecTimeStamp = Convert.ToDateTime(reader["RecTimeStamp"].ToString())
                        });
                    }
                }


                if (FromDate != "All" && ToDate != "All")
                {
                    foreach (var obj in objSampleModel)
                    {
                        if (obj.IssueDate.Date >= StartDate.Date && obj.IssueDate.Date <= EndDate.Date)
                        {
                            objListSales.Add(obj);
                        }
                    }
                }
                else
                {
                    objListSales = objSampleModel;
                }
                objListSales = objListSales.OrderByDescending(r => r.RecTimeStamp).ToList();
            }
            catch (Exception ex)
            {

            }
            return objListSales;
        }

        public List<Dashboard> GetShoppeReport(string PartyCode)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseMessage = "Something went wrong!";

            objResponse.ResponseStatus = "FAILED";
            List<Dashboard> objDashboard = new List<Dashboard>();
            try
            {

                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConnectionString);
                String whereCondition = "";
                if (string.IsNullOrEmpty(PartyCode) || PartyCode == "0")
                {
                    PartyCode = string.Empty;
                    whereCondition = " WHERE PartyCode<>'" + System.Configuration.ConfigurationManager.AppSettings["WRPartyCode"] + "'";
                }
                string sqlQry = string.Empty;
                using (var entity = new InventoryEntities())
                {
                    var MobileShoppe = (from r in entity.ShoppeAssignments where r.PCId == PartyCode select r).FirstOrDefault();
                    if (MobileShoppe != null)
                    {
                        sqlQry = "Select * FROM MobileShoppeDashboardSummary('" + PartyCode + "') " + whereCondition;
                    }
                    else
                    {
                        sqlQry = "Select * FROM DashboardSummary('" + PartyCode + "') " + whereCondition;
                    }
                }
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlQry;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();
                Dashboard ds = new Dashboard();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        objDashboard.Add(new Dashboard
                        {
                            WalletBalance = Convert.ToDecimal(reader["WalletBalance"].ToString()),
                            PartyCode = reader["PartyCode"].ToString(),
                            PartyName = reader["PartyName"].ToString(),
                            UserName = reader["UserName"].ToString(),
                            TotalSale = Convert.ToDecimal(reader["TotalSale"].ToString()),
                            TotalFV = Convert.ToDecimal(reader["TotalFV"].ToString()),
                            TotalRV = Convert.ToDecimal(reader["TotalRV"].ToString()),
                            TotalFVAmt = Convert.ToDecimal(reader["TotalFVAmt"].ToString()),
                            TotalRVAmt = Convert.ToDecimal(reader["TotalRVAmt"].ToString()),
                            LastMnthSale = Convert.ToDecimal(reader["LastMnthSale"].ToString()),
                            LastMnthFV = Convert.ToDecimal(reader["LastMnthFV"].ToString()),
                            LastMnthRV = Convert.ToDecimal(reader["LastMnthRV"].ToString()),
                            LastMnthFVAmt = Convert.ToDecimal(reader["LastMnthFVAmt"].ToString()),
                            LastMnthRVAmt = Convert.ToDecimal(reader["LastMnthRVAmt"].ToString()),
                            LastMnthTDAmt = Convert.ToDecimal(reader["LastMnthTDAmt"].ToString()),
                            LastMnthTDFV = Convert.ToDecimal(reader["LastMnthTDFV"].ToString()),
                            LastMnthTDRV = Convert.ToDecimal(reader["LastMnthTDRV"].ToString()),
                            LastMnthTDFVAmt = Convert.ToDecimal(reader["LastMnthTDFVAmt"].ToString()),
                            LastMnthTDRVAmt = Convert.ToDecimal(reader["LastMnthTDRVAmt"].ToString()),
                            MnthSale = Convert.ToDecimal(reader["MnthSale"].ToString()),
                            MnthFV = Convert.ToDecimal(reader["MnthFV"].ToString()),
                            MnthRV = Convert.ToDecimal(reader["MnthRV"].ToString()),
                            MnthFVAmt = Convert.ToDecimal(reader["MnthFVAmt"].ToString()),
                            MnthRVAmt = Convert.ToDecimal(reader["MnthRVAmt"].ToString()),
                            TodaySale = Convert.ToDecimal(reader["TodaySale"].ToString()),
                            TodayFV = Convert.ToDecimal(reader["TodayFV"].ToString()),
                            TodayRV = Convert.ToDecimal(reader["TodayRV"].ToString()),
                            TodayFVAmt = Convert.ToDecimal(reader["TodayFVAmt"].ToString()),
                            TodayRVAmt = Convert.ToDecimal(reader["TodayRVAmt"].ToString()),
                            TodayBillCnt = Convert.ToDecimal(reader["TodayBillCnt"].ToString()),
                            TodayFVBillCnt = Convert.ToDecimal(reader["TodayFVBillCnt"].ToString()),
                            TodayRVBillCnt = Convert.ToDecimal(reader["TodayRVBillCnt"].ToString()),
                            StockVal = Convert.ToDecimal(reader["StockVal"].ToString()),
                            StockQty = Convert.ToDecimal(reader["StockQty"].ToString()),
                            WRVal = Convert.ToDecimal(reader["WRVal"].ToString()),
                            WRQty = Convert.ToDecimal(reader["WRQty"].ToString()),
                            FRVal = Convert.ToDecimal(reader["FRVal"].ToString()),
                            FRQty = Convert.ToDecimal(reader["FRQty"].ToString()),
                            LastMnthFVBillCnt = Convert.ToDecimal(reader["LastMnthFVBillCnt"].ToString()),
                            LastMnthRVBillCnt = Convert.ToDecimal(reader["LastMnthRVBillCnt"].ToString()),
                            LastMnthTDFVCnt = Convert.ToDecimal(reader["LastMnthTDFVCnt"].ToString()),
                            LastMnthTDRVCnt = Convert.ToDecimal(reader["LastMnthTDRVCnt"].ToString()),
                            MnthFVCnt = Convert.ToDecimal(reader["MnthFVCnt"].ToString()),
                            MnthRVCnt = Convert.ToDecimal(reader["MnthRVCnt"].ToString()),
                            TotalFVCnt = Convert.ToDecimal(reader["TotalFVCnt"].ToString()),
                            TotalRVCnt = Convert.ToDecimal(reader["TotalRVCnt"].ToString())
                        });
                        ds.PartyCode = "Total"; ds.UserName = ""; ds.PartyName = "";
                        ds.WalletBalance += Convert.ToDecimal(reader["WalletBalance"].ToString());
                        ds.StockVal += Convert.ToDecimal(reader["StockVal"].ToString());
                        ds.StockQty += Convert.ToDecimal(reader["StockQty"].ToString());
                        ds.WRVal += Convert.ToDecimal(reader["WRVal"].ToString());
                        ds.WRQty += Convert.ToDecimal(reader["WRQty"].ToString());
                        ds.FRVal += Convert.ToDecimal(reader["FRVal"].ToString());
                        ds.FRQty += Convert.ToDecimal(reader["FRQty"].ToString());

                        ds.TotalSale += Convert.ToDecimal(reader["TotalSale"].ToString());
                        ds.TodayBillCnt += Convert.ToDecimal(reader["TodayBillCnt"].ToString());
                        ds.TodaySale += Convert.ToDecimal(reader["TodaySale"].ToString());
                        ds.LastMnthSale += Convert.ToDecimal(reader["LastMnthSale"].ToString());
                        ds.LastMnthTDAmt += Convert.ToDecimal(reader["LastMnthTDAmt"].ToString());
                        ds.MnthSale += Convert.ToDecimal(reader["MnthSale"].ToString());

                        ds.TotalFV += Convert.ToDecimal(reader["TotalFV"].ToString());
                        ds.TodayFV += Convert.ToDecimal(reader["TodayFV"].ToString());
                        ds.LastMnthFV += Convert.ToDecimal(reader["LastMnthFV"].ToString());
                        ds.LastMnthTDFV += Convert.ToDecimal(reader["LastMnthTDFV"].ToString());
                        ds.MnthFV += Convert.ToDecimal(reader["MnthFV"].ToString());

                        ds.TotalFVAmt += Convert.ToDecimal(reader["TotalFVAmt"].ToString());
                        ds.TodayFVAmt += Convert.ToDecimal(reader["TodayFVAmt"].ToString());
                        ds.LastMnthFVAmt += Convert.ToDecimal(reader["LastMnthFVAmt"].ToString());
                        ds.LastMnthTDFVAmt += Convert.ToDecimal(reader["LastMnthTDFVAmt"].ToString());
                        ds.MnthFVAmt += Convert.ToDecimal(reader["MnthFVAmt"].ToString());

                        ds.TotalFVCnt += Convert.ToDecimal(reader["TotalFVCnt"].ToString());
                        ds.TodayFVBillCnt += Convert.ToDecimal(reader["TodayFVBillCnt"].ToString());
                        ds.LastMnthFVBillCnt += Convert.ToDecimal(reader["LastMnthFVBillCnt"].ToString());
                        ds.LastMnthTDFVCnt += Convert.ToDecimal(reader["LastMnthTDFVCnt"].ToString());
                        ds.MnthFVCnt += Convert.ToDecimal(reader["MnthFVCnt"].ToString());

                        ds.TotalRV += Convert.ToDecimal(reader["TotalRV"].ToString());
                        ds.TodayRV += Convert.ToDecimal(reader["TodayRV"].ToString());
                        ds.LastMnthRV += Convert.ToDecimal(reader["LastMnthRV"].ToString());
                        ds.LastMnthTDRV += Convert.ToDecimal(reader["LastMnthTDRV"].ToString());
                        ds.MnthRV += Convert.ToDecimal(reader["MnthRV"].ToString());

                        ds.TotalRVAmt += Convert.ToDecimal(reader["TotalRVAmt"].ToString());
                        ds.TodayRVAmt += Convert.ToDecimal(reader["TodayRVAmt"].ToString());
                        ds.LastMnthRVAmt += Convert.ToDecimal(reader["LastMnthRVAmt"].ToString());
                        ds.LastMnthTDRVAmt += Convert.ToDecimal(reader["LastMnthTDRVAmt"].ToString());
                        ds.MnthRVAmt += Convert.ToDecimal(reader["MnthRVAmt"].ToString());

                        ds.TotalRVCnt += Convert.ToDecimal(reader["TotalRVCnt"].ToString());
                        ds.TodayRVBillCnt += Convert.ToDecimal(reader["TodayRVBillCnt"].ToString());
                        ds.LastMnthRVBillCnt += Convert.ToDecimal(reader["LastMnthRVBillCnt"].ToString());
                        ds.LastMnthTDRVCnt += Convert.ToDecimal(reader["LastMnthTDRVCnt"].ToString());
                        ds.MnthRVCnt += Convert.ToDecimal(reader["MnthRVCnt"].ToString());


                    }
                }

                if (!(string.IsNullOrEmpty(PartyCode)))
                {
                    objDashboard = objDashboard.Where(r => r.PartyCode == PartyCode || r.UserName == PartyCode).ToList();
                }
                objDashboard.Add(ds);
            }
            catch (Exception ex)
            {

            }
            return objDashboard;
        }

        public List<DashboardColumn> getDasboardColumns()
        {
            List<DashboardColumn> columns = new List<DashboardColumn>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    columns = (from r in entity.DashboardSummaryColumns
                               where r.ActiveStatus == "Y"
                               select new DashboardColumn
                               {
                                   Name = r.FldName,
                                   column = r.FldValue
                               }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return columns;
        }

        public List<SaleRegister> GetProductSaleRegisterReport(string FromDate, string ToDate, string PartyCode)
        {
            List<SaleRegister> objReport = new List<SaleRegister>();
            string WhereCondition = string.Empty;
            string Fld = string.Empty;
            var dataTable = new DataTable();

            try
            {
                DateTime StartDate = new DateTime();
                DateTime EndDate = new DateTime();
                string NewFromDate = "";
                string NewToDate = "";

                if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                {
                    var SplitDate = FromDate.Split('-');
                    string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                    StartDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    StartDate = StartDate.Date;
                    NewFromDate = StartDate.Date.ToString("MM/dd/yyyy");
                }
                else
                {
                    NewFromDate = DateTime.Now.AddYears(-10).Date.ToString("MM/dd/yyyy");

                }
                if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                {
                    var SplitDate = ToDate.Split('-');
                    string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                    EndDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    EndDate = EndDate.Date;
                    NewToDate = EndDate.Date.ToString("MM/dd/yyyy");
                }
                else
                {
                    NewToDate = DateTime.Now.Date.ToString("MM/dd/yyyy");

                }
                string db = System.Configuration.ConfigurationManager.AppSettings["Database"];

                string sql = "Exec SaleRegister2 '" + NewFromDate + "','" + NewToDate + "','" + PartyCode + "'";
                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConnectionString);
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = sql;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SaleRegister obj = new SaleRegister();
                        obj.ProductId = Convert.ToString(reader["ProdID"].ToString());
                        obj.Qty = Convert.ToString(reader["Qty"].ToString());
                        obj.ProductName = Convert.ToString(reader["ProductName"].ToString());
                        obj.HSN = Convert.ToString(reader["HSNCode"].ToString());
                        obj.Tax = Convert.ToDecimal(reader["Tax"].ToString());
                        obj.BasicAmt = Convert.ToDecimal(reader["BasicAmount"].ToString());
                        obj.ExemtTax = Convert.ToDecimal(reader["ExemtTax"].ToString());
                        obj.TotalCGST = Convert.ToDecimal(reader["CGSTAmt"].ToString());
                        obj.TotalIGST = Convert.ToDecimal(reader["IGSTAmt"].ToString());
                        obj.TotalSGST = Convert.ToDecimal(reader["SGSTAmt"].ToString());
                        obj.TotalAmt = Convert.ToDecimal(reader["TotalAmount"].ToString());
                        objReport.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objReport;
        }

        public string GetSRProductList(string STRNo)
        {
            string productList = string.Empty;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    var list = (from r in entity.TrnSalesReturnDetails where r.SReturnNo == STRNo select r).ToList();
                    long amount = 0;
                    long netamount = 0;
                    long qty = 0;
                    productList = "<table border=1 style='width:100%'><tr><th>Product Code</th><th>Product Name</th><th>Batch No.</th><th>Return Qty</th><th>Amount</th><th>Net Amount</th></tr>";
                    foreach (var product in list)
                    {
                        productList += "<tr><td>" + product.ProdId + "</td><td>" + product.ProductName + "</td><td>" + product.BatchNo + "</td><td>" + (long)product.ReturnQty + "</td><td>" + (long)product.DP + "</td><td>" + (long)(product.DP * product.ReturnQty) + "</td></tr>";
                        amount += (long)product.DP;
                        netamount += (long)(product.DP * product.ReturnQty);
                        qty += (long)product.ReturnQty;
                    }
                    productList += "<tr><td></td><td><b>Total:-</b></td><td></td><td><b>" + qty + "</b></td><td></td><td><b>" + netamount + "</b></td></tr>";
                    productList += "</table>"; ;
                }
            }
            catch (Exception Ex)
            {

            }
            return productList;
        }

        public string GetSampleProductList(string STRNo)
        {
            string productList = string.Empty;
            long TQty = 0;
            long Tamt = 0;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    var list = (from r in entity.TrnSampleProducts
                                where r.TransNo == STRNo
                                join p in entity.M_ProductMaster on r.ProdID equals p.ProdId
                                select new
                                {
                                    ProdID = r.ProdID,
                                    ProductName = r.ProductName,
                                    BatchNo = r.BatchNo,
                                    Qty = r.Qty,
                                    dp = p.DP
                                }).ToList();
                    productList = "<table border=1 style='width:100%'><tr><th>Product Code</th><th>Product Name</th><th>Batch</th><th>Sample Qty</th><th>Amount</th><th>Net Amount</th></tr>";
                    foreach (var product in list)
                    {
                        productList += "<tr><td>" + product.ProdID + "</td><td>" + product.ProductName + "</td><td>" + product.BatchNo + "</td><td>" + (long)product.Qty + "</td><td>" + (long)product.dp + "</td><td>" + (long)(product.dp * product.Qty) + "</td></tr>";
                        TQty += (long)product.Qty;
                        Tamt += (long)(product.dp * product.Qty);
                    }
                    productList += "<tr><td></td><td></td><td><b>Total:-</b></td><td><b>" + TQty + "</b></td><td></td><td><b>" + Tamt + "</b></td></tr>";
                    productList += "</table>"; ;
                }
            }
            catch (Exception Ex)
            {

            }
            return productList;
        }

        public List<GVCreditNote> GetGVCreditNote(string PartyCode, string FromDate, string ToDate)
        {
            List<GVCreditNote> GvInvoiceList = new List<GVCreditNote>();
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;
            try
            {

                if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                {
                    var SplitDate = FromDate.Split('-');
                    string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];
                    StartDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MMM/dd/yyyy", CultureInfo.InvariantCulture));
                    StartDate = StartDate.Date;
                }
                if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                {
                    var SplitDate = ToDate.Split('-');
                    string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];
                    EndDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MMM/dd/yyyy", CultureInfo.InvariantCulture));
                    EndDate = EndDate.Date;
                }
                using (var entity = new InventoryEntities())
                {
                    List<TrnBillMain> gvInvoices = new List<TrnBillMain>();
                    if (!string.IsNullOrEmpty(PartyCode) && PartyCode != "0" && PartyCode.ToLower() != "all")
                    {
                        gvInvoices = (from r in entity.TrnBillMains where r.SoldBy == PartyCode && r.BillType == "G" select r).ToList();
                    }
                    else
                    {
                        gvInvoices = (from r in entity.TrnBillMains where r.BillType == "G" select r).ToList();
                    }

                    var gvInvoicesList = new List<TrnBillMain>();

                    if (FromDate != "All" && ToDate != "All")
                    {
                        foreach (var obj in gvInvoices)
                        {
                            if (obj.BillDate.Date >= StartDate.Date && obj.BillDate.Date <= EndDate.Date)
                            {
                                gvInvoicesList.Add(obj);
                            }
                        }
                    }
                    else
                    {
                        gvInvoicesList = gvInvoices;
                    }
                    DateTime checkdate = new DateTime(2019, 10, 22);
                    var GVInvoiceproductList = (from r in gvInvoicesList

                                                join t in entity.TrnBillDetails on r.BillNo equals t.BillNo
                                                join M in entity.M_ProductMaster on t.ProductId equals M.ProdId
                                                select new ProductModel
                                                {
                                                    IdNo = r.FCode,
                                                    UID = r.UserBillNo,
                                                    PartyName = r.PartyName,
                                                    ProductCodeStr = t.ProductId,
                                                    ProductName = t.ProductName,
                                                    Barcode = t.Barcode,
                                                    DP = t.DP1,
                                                    Quantity = t.Qty,
                                                    TotalNetPayable = r.NetPayable,
                                                    BillDate = r.BillDate
                                                }
                                                          ).ToList();



                    foreach (var invoice in gvInvoicesList)
                    {
                        if (invoice.BillDate.Date >= checkdate.Date)
                        {
                            GVCreditNote obj = new GVCreditNote();
                            obj.BillNo = invoice.UserBillNo;
                            obj.BillDate = invoice.BillDate.ToString("dd-MM-yyyy");
                            obj.BillAmount = invoice.NetPayable;
                            obj.AmountRecieved = invoice.NetPayable;
                            obj.RCPAmount = GVInvoiceproductList.Where(r => r.UID == invoice.UserBillNo).Select(t => t.DP * t.Quantity).Sum() ?? 0;
                            obj.CreditNoteValue = obj.RCPAmount - obj.AmountRecieved;
                            GvInvoiceList.Add(obj);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {

            }
            return GvInvoiceList;
        }


        public List<ConsistentOffers> GetConsistentFPVOffer(string IdNo, bool TeamWise)
        {
            string WhereCondition = string.Empty;
            List<ConsistentOffers> offers = new List<ConsistentOffers>();
            string db = System.Configuration.ConfigurationManager.AppSettings["Database"];
            string dbInv = System.Configuration.ConfigurationManager.AppSettings["INVDatabase"];
            if (!string.IsNullOrEmpty(IdNo) && IdNo.ToLower() != "all" && IdNo.ToLower() != "0")
            {

                WhereCondition = " and b.IDNo ='" + IdNo + "'";
            }
            string sql = string.Empty;
            if (TeamWise == false)
            {
                sql = "Select * FROM V#ConsistentOfferDetail as b WHERE 1=1" + WhereCondition;
            }
            else
            {
                sql = "Select * FROM V#ConsistentOfferDetail WHERE Formno in (Select FormnoDwn FRom " + db + "..R_MemTreeRelation a," + db + "..M_MemberMaster b WHERE a.Formno=b.Formno " + WhereCondition + ")";
            }

            string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
            SqlConnection SC = new SqlConnection(InvConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Connection = SC;
            SC.Close();
            SC.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ConsistentOffers tempobj = new ConsistentOffers();

                    tempobj.IDNo = Convert.ToString(reader["IDNo"].ToString());
                    tempobj.MemName = Convert.ToString(reader["MemName"].ToString());
                    tempobj.Mobl = Convert.ToString(reader["Mobl"].ToString());
                    tempobj.Formno = Convert.ToString(reader["Formno"].ToString());
                    tempobj.City = Convert.ToString(reader["City"].ToString());
                    tempobj.Amt200 = Convert.ToString(reader["Amt200"].ToString());
                    tempobj.Amt300 = Convert.ToString(reader["Amt300"].ToString());
                    tempobj.Amt400 = Convert.ToString(reader["Amt400"].ToString());
                    tempobj.Amt500 = Convert.ToString(reader["Amt500"].ToString());
                    tempobj.Amt600 = Convert.ToString(reader["Amt600"].ToString());
                    tempobj.Amt1000 = Convert.ToString(reader["Amt1000"].ToString());
                    offers.Add(tempobj);
                }
            }
            return offers;
        }

        public string GetMonthlyConsistentFPVOffer(string IdNo, bool TeamWise)
        {
            string tableString = string.Empty;

            int totalmonths = ((DateTime.Now.Year - new DateTime(2019, 02, 01).Year) * 12) + DateTime.Now.Month - new DateTime(2019, 02, 01).Month;
            int year = 2019;

            tableString = "<table border='1' class='montable'>";
            tableString += "<thead class='monthlyTable'><tr><th>Sno</th><th>PC ID</th><th>PC Name</th><th>Mobile No.</th><th>Form No.</th><th>City</th>";
            var columns = "<tbody class='monthlyTable'><tr style='font-weight: bold;'><td></td><td></td><td></td><td></td><td></td><td></td>";
            string[] monthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

            for (var i = 2019; i <= DateTime.Now.Year; i++)
            {
                var j = (i == 2019) ? 2 : 1;
                var k = (i == DateTime.Now.Year) ? DateTime.Now.Month : 12;
                for (; j <= k; j++)
                {
                    tableString += "<th colspan = '3'>" + monthNames[j - 1] + " " + i + " </th>";
                    columns += "<td><div style='width:120px; text-align:center;'>Bill Dt</div></td><td><div style='width:120px; text-align:center;'>Bill No</div></td><td><div style='width:120px; text-align:center;'>FPV</div></td>";
                }
            }

            tableString += "</tr></thead>";
            columns += "</tr>";
            tableString += columns;
            var formDetails = string.Empty;
            using (var entities = new InventoryEntities())
            {
                var offertable = (from r in entities.V_MonthWiseConsistentOffer1

                                  select new
                                  {
                                      Formno = r.Formno,
                                      IDNo = r.IDNo,
                                      MemName = r.MemName,
                                      Mobile = r.Mobl,
                                      City = r.City,
                                      RectimeStamp = r.RecTimeStamp,
                                      couponamt = r.CouponAmt,
                                      userbilno = r.UserBillNo
                                  }).OrderBy(r => r.Formno).ToList();

                if (TeamWise == true)
                {
                    decimal[] TeamList = (from r in entities.V_DownlineMember
                                          where (!string.IsNullOrEmpty(IdNo) && IdNo.ToLower() != "all" && IdNo.ToLower() != "0") ? r.IDNo == IdNo : 1 == 1
                                          select r.FormnoDwn).ToArray();
                    offertable = offertable.Where(r => TeamList.Contains(r.Formno)).OrderBy(r => r.Formno).ToList();
                }
                else
                {
                    offertable = offertable.Where(r => (!string.IsNullOrEmpty(IdNo) && IdNo.ToLower() != "all" && IdNo.ToLower() != "0") ? r.IDNo == IdNo : 1 == 1).OrderBy(r => r.Formno).ToList();
                }

                var formList = offertable.OrderBy(r => r.Formno).Select(r => r.Formno).Distinct().ToList();
                int count = 1;

                foreach (var record in formList)
                {
                    var FormRecords = offertable.Where(r => r.Formno == record).OrderBy(r => r.RectimeStamp).ToList();
                    formDetails += "<tr>";
                    formDetails += "<td><div style='width:50px'>" + count++ + "</div></td>";
                    formDetails += "<td><div style='width:120px'>" + FormRecords.FirstOrDefault().IDNo + "</div></td>";
                    formDetails += "<td><div style='width:200px'>" + FormRecords.FirstOrDefault().MemName + "</div></td>";
                    formDetails += "<td><div style='width:120px'>" + FormRecords.FirstOrDefault().Mobile + "</div></td>";
                    formDetails += "<td><div style='width:120px'>" + FormRecords.FirstOrDefault().Formno + "</div></td>";
                    formDetails += "<td>" + FormRecords.FirstOrDefault().City + "</td>";

                    for (var i = 2019; i <= DateTime.Now.Year; i++)
                    {
                        var j = (i == 2019) ? 2 : 1;
                        var k = (i == DateTime.Now.Year) ? DateTime.Now.Month : 12;
                        for (; j <= k; j++)
                        {
                            var monthRecord = FormRecords.Where(r => r.RectimeStamp.Month == j && r.RectimeStamp.Year == i).FirstOrDefault();
                            if (monthRecord != null)
                            {
                                formDetails += "<td>" + monthRecord.RectimeStamp.ToString("yyyy-MM-dd") + "</td>";
                                formDetails += "<td>" + monthRecord.userbilno + "</td>";
                                formDetails += "<td>" + monthRecord.couponamt + "</td>";
                            }
                            else
                            {
                                formDetails += "<td>&nbsp;</td>";
                                formDetails += "<td>&nbsp;</td>";
                                formDetails += "<td>&nbsp;</td>";
                            }

                        }

                    }
                    formDetails += "</tr>";
                }
            }

            tableString += formDetails;
            tableString += "</tbody></table>";


            return tableString;
        }

        public string GetMonthlyConsistentFPVOffe2021r(string IdNo, bool TeamWise)
        {
            string tableString = string.Empty;

            int totalmonths = ((DateTime.Now.Year - new DateTime(2019, 02, 01).Year) * 12) + DateTime.Now.Month - new DateTime(2019, 02, 01).Month;
            int year = 2019;

            tableString = "<table border='1' class='montable'>";
            tableString += "<thead class='monthlyTable'><tr><th>Sno</th><th>PC ID</th><th>PC Name</th><th>Mobile No.</th><th>Form No.</th><th>City</th>";
            var columns = "<tbody class='monthlyTable'><tr style='font-weight: bold;'><td></td><td></td><td></td><td></td><td></td><td></td>";
            string[] monthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

            for (var i = 2019; i <= DateTime.Now.Year; i++)
            {
                var j = (i == 2019) ? 2 : 1;
                var k = (i == DateTime.Now.Year) ? DateTime.Now.Month : 12;
                for (; j <= k; j++)
                {
                    tableString += "<th colspan = '3'>" + monthNames[j - 1] + " " + i + " </th>";
                    columns += "<td><div style='width:120px; text-align:center;'>Bill Dt</div></td><td><div style='width:120px; text-align:center;'>Bill No</div></td><td><div style='width:120px; text-align:center;'>FPV</div></td>";
                }
            }

            tableString += "</tr></thead>";
            columns += "</tr>";
            tableString += columns;
            var formDetails = string.Empty;
            using (var entities = new InventoryEntities())
            {
                var offertable = (from r in entities.V_MonthWiseConsistentOffer1

                                  select new
                                  {
                                      Formno = r.Formno,
                                      IDNo = r.IDNo,
                                      MemName = r.MemName,
                                      Mobile = r.Mobl,
                                      City = r.City,
                                      RectimeStamp = r.RecTimeStamp,
                                      couponamt = r.CouponAmt,
                                      userbilno = r.UserBillNo
                                  }).OrderBy(r => r.Formno).ToList();

                if (TeamWise == true)
                {
                    decimal[] TeamList = (from r in entities.V_DownlineMember
                                          where (!string.IsNullOrEmpty(IdNo) && IdNo.ToLower() != "all" && IdNo.ToLower() != "0") ? r.IDNo == IdNo : 1 == 1
                                          select r.FormnoDwn).ToArray();
                    offertable = offertable.Where(r => TeamList.Contains(r.Formno)).OrderBy(r => r.Formno).ToList();
                }
                else
                {
                    offertable = offertable.Where(r => (!string.IsNullOrEmpty(IdNo) && IdNo.ToLower() != "all" && IdNo.ToLower() != "0") ? r.IDNo == IdNo : 1 == 1).OrderBy(r => r.Formno).ToList();
                }

                var formList = offertable.OrderBy(r => r.Formno).Select(r => r.Formno).Distinct().ToList();
                int count = 1;

                foreach (var record in formList)
                {
                    var FormRecords = offertable.Where(r => r.Formno == record).OrderBy(r => r.RectimeStamp).ToList();
                    formDetails += "<tr>";
                    formDetails += "<td><div style='width:50px'>" + count++ + "</div></td>";
                    formDetails += "<td><div style='width:120px'>" + FormRecords.FirstOrDefault().IDNo + "</div></td>";
                    formDetails += "<td><div style='width:200px'>" + FormRecords.FirstOrDefault().MemName + "</div></td>";
                    formDetails += "<td><div style='width:120px'>" + FormRecords.FirstOrDefault().Mobile + "</div></td>";
                    formDetails += "<td><div style='width:120px'>" + FormRecords.FirstOrDefault().Formno + "</div></td>";
                    formDetails += "<td>" + FormRecords.FirstOrDefault().City + "</td>";

                    for (var i = 2019; i <= DateTime.Now.Year; i++)
                    {
                        var j = (i == 2019) ? 2 : 1;
                        var k = (i == DateTime.Now.Year) ? DateTime.Now.Month : 12;
                        for (; j <= k; j++)
                        {
                            var monthRecord = FormRecords.Where(r => r.RectimeStamp.Month == j && r.RectimeStamp.Year == i).FirstOrDefault();
                            if (monthRecord != null)
                            {
                                formDetails += "<td>" + monthRecord.RectimeStamp.ToString("yyyy-MM-dd") + "</td>";
                                formDetails += "<td>" + monthRecord.userbilno + "</td>";
                                formDetails += "<td>" + monthRecord.couponamt + "</td>";
                            }
                            else
                            {
                                formDetails += "<td>&nbsp;</td>";
                                formDetails += "<td>&nbsp;</td>";
                                formDetails += "<td>&nbsp;</td>";
                            }

                        }

                    }
                    formDetails += "</tr>";
                }
            }

            tableString += formDetails;
            tableString += "</tbody></table>";


            return tableString;
        }



        public string GetShoppeStockReport(string PartyCode, string ProductCode, int Month, int year)
        {
            string objStock = string.Empty;
            try
            {
                using (var db = new InventoryEntities())
                {

                    var Stocks = (from r in db.FranchiseStockSummaryNew(PartyCode, Month, year)
                                  where (!String.IsNullOrEmpty(ProductCode) && ProductCode != "0") ? r.ProdId == ProductCode : 1 == 1
                                  select r).OrderBy(o => o.ProductName).ToList();
                    int days = DateTime.DaysInMonth(year, Month);
                    var productList = (from r in db.M_ProductMaster
                                       select new
                                       {
                                           ProdId = r.ProdId,
                                           prodName = r.ProductName
                                       }).ToList();

                    var count = 1;
                    objStock = "<tr><td>SR NO</td><td style='white-space:nowrap;'>Product Name</td><td>Opening Stock</td>";
                    for (int i = 1; i <= days; i++)
                    {
                        objStock += "<td colspan='2'>" + i + "</td>";
                    }
                    objStock += "<td>Closing Stock</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>";

                    for (int i = 1; i <= days; i++)
                    {
                        objStock += "<td>In</td><td>Out</td>";
                    }
                    objStock += "<td>&nbsp;</td></tr>";
                    var TotalOpening = 0;
                    var TotalClosing = 0;

                    foreach (var obj in Stocks)
                    {
                        TotalOpening += Convert.ToInt16(obj.OpeningStock);
                        TotalClosing += Convert.ToInt16(obj.ClosingStock);

                        objStock += "<tr><td>" + count + "</td><td style='white-space:nowrap;'>" + productList.FirstOrDefault(o => o.ProdId == obj.ProdId).prodName + "</td><td>" + Convert.ToInt16(obj.OpeningStock) + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_1) > 0 ? "<b>" + Convert.ToInt16(obj.In_1) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_1) > 0 ? "<b>" + Convert.ToInt16(obj.Out_1) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_2) > 0 ? "<b>" + Convert.ToInt16(obj.In_2) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_2) > 0 ? "<b>" + Convert.ToInt16(obj.Out_2) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_3) > 0 ? "<b>" + Convert.ToInt16(obj.In_3) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_3) > 0 ? "<b>" + Convert.ToInt16(obj.Out_3) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_4) > 0 ? "<b>" + Convert.ToInt16(obj.In_4) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_4) > 0 ? "<b>" + Convert.ToInt16(obj.Out_4) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_5) > 0 ? "<b>" + Convert.ToInt16(obj.In_5) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_5) > 0 ? "<b>" + Convert.ToInt16(obj.Out_5) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_6) > 0 ? "<b>" + Convert.ToInt16(obj.In_6) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_6) > 0 ? "<b>" + Convert.ToInt16(obj.Out_6) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_7) > 0 ? "<b>" + Convert.ToInt16(obj.In_7) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_7) > 0 ? "<b>" + Convert.ToInt16(obj.Out_7) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_8) > 0 ? "<b>" + Convert.ToInt16(obj.In_8) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_8) > 0 ? "<b>" + Convert.ToInt16(obj.Out_8) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_9) > 0 ? "<b>" + Convert.ToInt16(obj.In_9) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_9) > 0 ? "<b>" + Convert.ToInt16(obj.Out_9) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_10) > 0 ? "<b>" + Convert.ToInt16(obj.In_10) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_10) > 0 ? "<b>" + Convert.ToInt16(obj.Out_10) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_11) > 0 ? "<b>" + Convert.ToInt16(obj.In_11) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_11) > 0 ? "<b>" + Convert.ToInt16(obj.Out_11) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_12) > 0 ? "<b>" + Convert.ToInt16(obj.In_12) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_12) > 0 ? "<b>" + Convert.ToInt16(obj.Out_12) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_13) > 0 ? "<b>" + Convert.ToInt16(obj.In_13) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_13) > 0 ? "<b>" + Convert.ToInt16(obj.Out_13) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_14) > 0 ? "<b>" + Convert.ToInt16(obj.In_14) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_14) > 0 ? "<b>" + Convert.ToInt16(obj.Out_14) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_15) > 0 ? "<b>" + Convert.ToInt16(obj.In_15) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_15) > 0 ? "<b>" + Convert.ToInt16(obj.Out_15) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_16) > 0 ? "<b>" + Convert.ToInt16(obj.In_16) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_16) > 0 ? "<b>" + Convert.ToInt16(obj.Out_16) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_17) > 0 ? "<b>" + Convert.ToInt16(obj.In_17) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_17) > 0 ? "<b>" + Convert.ToInt16(obj.Out_17) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_18) > 0 ? "<b>" + Convert.ToInt16(obj.In_18) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_18) > 0 ? "<b>" + Convert.ToInt16(obj.Out_18) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_19) > 0 ? "<b>" + Convert.ToInt16(obj.In_19) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_19) > 0 ? "<b>" + Convert.ToInt16(obj.Out_19) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_20) > 0 ? "<b>" + Convert.ToInt16(obj.In_20) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_20) > 0 ? "<b>" + Convert.ToInt16(obj.Out_20) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_21) > 0 ? "<b>" + Convert.ToInt16(obj.In_21) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_21) > 0 ? "<b>" + Convert.ToInt16(obj.Out_21) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_22) > 0 ? "<b>" + Convert.ToInt16(obj.In_22) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_22) > 0 ? "<b>" + Convert.ToInt16(obj.Out_22) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_23) > 0 ? "<b>" + Convert.ToInt16(obj.In_23) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_23) > 0 ? "<b>" + Convert.ToInt16(obj.Out_23) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_24) > 0 ? "<b>" + Convert.ToInt16(obj.In_24) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_24) > 0 ? "<b>" + Convert.ToInt16(obj.Out_24) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_25) > 0 ? "<b>" + Convert.ToInt16(obj.In_25) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_25) > 0 ? "<b>" + Convert.ToInt16(obj.Out_25) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_26) > 0 ? "<b>" + Convert.ToInt16(obj.In_26) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_26) > 0 ? "<b>" + Convert.ToInt16(obj.Out_26) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_27) > 0 ? "<b>" + Convert.ToInt16(obj.In_27) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_27) > 0 ? "<b>" + Convert.ToInt16(obj.Out_27) + "</b>" : "0") + "</td>";
                        objStock += "<td>" + (Convert.ToInt16(obj.In_28) > 0 ? "<b>" + Convert.ToInt16(obj.In_28) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_28) > 0 ? "<b>" + Convert.ToInt16(obj.Out_28) + "</b>" : "0") + "</td>";
                        if (days > 28)
                        {
                            objStock += "<td>" + (Convert.ToInt16(obj.In_29) > 0 ? "<b>" + Convert.ToInt16(obj.In_29) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_29) > 0 ? "<b>" + Convert.ToInt16(obj.Out_29) + "</b>" : "0") + "</td>";
                        }
                        if (days > 29)
                        {
                            objStock += "<td>" + (Convert.ToInt16(obj.In_30) > 0 ? "<b>" + Convert.ToInt16(obj.In_30) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_30) > 0 ? "<b>" + Convert.ToInt16(obj.Out_30) + "</b>" : "0") + "</td>";
                        }
                        if (days > 30)
                        {
                            objStock += "<td>" + (Convert.ToInt16(obj.In_31) > 0 ? "<b>" + Convert.ToInt16(obj.In_31) + "</b>" : "0") + "</td><td>" + (Convert.ToInt16(obj.Out_31) > 0 ? "<b>" + Convert.ToInt16(obj.Out_31) + "</b>" : "0") + "</td>";
                        }
                        objStock += "<td>" + Convert.ToInt16(obj.ClosingStock) + "</td></tr>";
                        count++;
                    }

                    objStock += "<tr><td>&nbsp;</td><td>&nbsp;</td><td>" + TotalOpening + "</td>";
                    for (int i = 1; i <= days; i++)
                    {
                        objStock += "<td>&nbsp;</td><td>&nbsp;</td>";
                    }
                    objStock += "<td>" + TotalClosing + "</td></tr>";


                }
            }
            catch (Exception)
            {

            }
            return objStock;
        }


        public List<GVCreditNote> GetMRICreditNote(string PartyCode, string FromDate, string ToDate)
        {
            List<GVCreditNote> GvInvoiceList = new List<GVCreditNote>();
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;
            try
            {

                if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                {
                    var SplitDate = FromDate.Split('-');
                    string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];
                    StartDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MMM/dd/yyyy", CultureInfo.InvariantCulture));
                    StartDate = StartDate.Date;
                }
                if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                {
                    var SplitDate = ToDate.Split('-');
                    string NewDate = SplitDate[0] + "/" + SplitDate[1] + "/" + SplitDate[2];
                    EndDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MMM/dd/yyyy", CultureInfo.InvariantCulture));
                    EndDate = EndDate.Date;
                }
                using (var entity = new InventoryEntities())
                {
                    List<TrnBillMain> gvInvoices = new List<TrnBillMain>();
                    if (!string.IsNullOrEmpty(PartyCode) && PartyCode != "0" && PartyCode.ToLower() != "all")
                    {
                        gvInvoices = (from r in entity.TrnBillMains where r.SoldBy == PartyCode && r.BillType == "C" select r).ToList();
                    }
                    else
                    {
                        gvInvoices = (from r in entity.TrnBillMains where r.BillType == "C" select r).ToList();
                    }

                    var gvInvoicesList = new List<TrnBillMain>();

                    if (FromDate != "All" && ToDate != "All")
                    {
                        foreach (var obj in gvInvoices)
                        {
                            if (obj.BillDate.Date >= StartDate.Date && obj.BillDate.Date <= EndDate.Date)
                            {
                                gvInvoicesList.Add(obj);
                            }
                        }
                    }
                    else
                    {
                        gvInvoicesList = gvInvoices;
                    }
                    DateTime checkdate = new DateTime(2019, 10, 22);
                    var GVInvoiceproductList = (from r in gvInvoicesList

                                                join t in entity.TrnBillDetails on r.BillNo equals t.BillNo
                                                join M in entity.M_ProductMaster on t.ProductId equals M.ProdId
                                                select new ProductModel
                                                {
                                                    IdNo = r.FCode,
                                                    UID = r.UserBillNo,
                                                    PartyName = r.PartyName,
                                                    ProductCodeStr = t.ProductId,
                                                    ProductName = t.ProductName,
                                                    Barcode = t.Barcode,
                                                    DP = t.DP1,
                                                    Quantity = t.Qty,
                                                    TotalNetPayable = r.NetPayable,
                                                    BillDate = r.BillDate
                                                }
                                                          ).ToList();



                    foreach (var invoice in gvInvoicesList)
                    {

                        GVCreditNote obj = new GVCreditNote();
                        obj.BillNo = invoice.UserBillNo;
                        obj.BillDate = invoice.BillDate.ToString("dd-MM-yyyy");
                        obj.BillAmount = invoice.NetPayable;
                        obj.AmountRecieved = invoice.NetPayable;
                        obj.RCPAmount = GVInvoiceproductList.Where(r => r.UID == invoice.UserBillNo).Select(t => t.DP * t.Quantity).Sum() ?? 0;
                        obj.CreditNoteValue = obj.RCPAmount - obj.AmountRecieved;
                        GvInvoiceList.Add(obj);
                    }
                }
            }
            catch (Exception Ex)
            {

            }
            return GvInvoiceList;
        }


        public string GetSJPConsistentReport(string IdNo)
        {
            string tableString = string.Empty;
            int totalmonths = ((DateTime.Now.Year - new DateTime(2021, 02, 01).Year) * 12) + DateTime.Now.Month - new DateTime(2021, 02, 01).Month;

            tableString = "<table border='1' class='montable'>";
            tableString += "<thead class='monthlyTable'><tr><th>Sno</th><th> ID</th><th> Name</th><th>Mobile No.</th><th>Form No.</th>";
            var columns = "<tbody class='monthlyTable'><tr style='font-weight: bold;'><td></td><td></td><td></td><td></td><td></td>";
            string[] monthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

            for (var i = 2021; i <= DateTime.Now.Year; i++)
            {
                var j = (i == 2021) ? 2 : 1;
                var k = (i == DateTime.Now.Year) ? DateTime.Now.Month : 12;
                for (; j <= k; j++)
                {
                    tableString += "<th colspan = '3'>" + monthNames[j - 1] + " " + i + " </th>";
                    columns += "<td><div style='width:120px; text-align:center;'>Bill Dt</div></td><td><div style='width:120px; text-align:center;'>Bill No</div></td><td><div style='width:120px; text-align:center;'>Bill Amount</div></td>";
                }
            }

            tableString += "</tr></thead>";
            columns += "</tr>";
            tableString += columns;
            var formDetails = string.Empty;
            using (var entities = new InventoryEntities())
            {
                var offertable = (from r in entities.V_MonthWiseSJPConsistent
                                  select new
                                  {
                                      Formno = r.Formno,
                                      IDNo = r.IDNo,
                                      MemName = r.MemName,
                                      Mobile = r.Mobl,
                                      RectimeStamp = r.billdate,
                                      couponamt = r.NetPayable,
                                      userbilno = r.UserBillNo
                                  }).OrderBy(r => r.Formno).ToList();


                offertable = offertable.Where(r => (!string.IsNullOrEmpty(IdNo) && IdNo.ToLower() != "all" && IdNo.ToLower() != "0") ? r.IDNo == IdNo : 1 == 1).OrderBy(r => r.Formno).ToList();

                var formList = offertable.OrderBy(r => r.Formno).Select(r => r.Formno).Distinct().ToList();
                int count = 1;

                foreach (var record in formList)
                {
                    var FormRecords = offertable.Where(r => r.Formno == record).OrderBy(r => r.RectimeStamp).ToList();
                    formDetails += "<tr>";
                    formDetails += "<td><div style='width:50px'>" + count++ + "</div></td>";
                    formDetails += "<td><div style='width:120px'>" + FormRecords.FirstOrDefault().IDNo + "</div></td>";
                    formDetails += "<td><div style='width:200px'>" + FormRecords.FirstOrDefault().MemName + "</div></td>";
                    formDetails += "<td><div style='width:120px'>" + FormRecords.FirstOrDefault().Mobile + "</div></td>";
                    formDetails += "<td><div style='width:120px'>" + FormRecords.FirstOrDefault().Formno + "</div></td>";

                    for (var i = 2021; i <= DateTime.Now.Year; i++)
                    {
                        var j = (i == 2021) ? 2 : 1;
                        var k = (i == DateTime.Now.Year) ? DateTime.Now.Month : 12;
                        for (; j <= k; j++)
                        {
                            var monthRecord = FormRecords.Where(r => r.RectimeStamp.Month == j && r.RectimeStamp.Year == i).FirstOrDefault();
                            if (monthRecord != null)
                            {
                                formDetails += "<td>" + monthRecord.RectimeStamp.ToString("yyyy-MM-dd") + "</td>";
                                formDetails += "<td>" + monthRecord.userbilno + "</td>";
                                formDetails += "<td>" + monthRecord.couponamt + "</td>";
                            }
                            else
                            {
                                formDetails += "<td>&nbsp;</td>";
                                formDetails += "<td>&nbsp;</td>";
                                formDetails += "<td>&nbsp;</td>";
                            }

                        }

                    }
                    formDetails += "</tr>";
                }
            }

            tableString += formDetails;
            tableString += "</tbody></table>";


            return tableString;
        }


        public string GetFPVConsitencyReport(string IdNo)
        {
            string tableString = string.Empty;

            tableString = "<table border='1' class='montable'>";
            tableString += "<thead class='monthlyTable'><tr><th>Sno</th><th>PC ID</th><th>PC Name</th><th>Mobile No.</th><th>City</th><th>Int. PC ID</th> <th>Int. PC Name</th><th>Int. PC Mobile</th><th>Month 1</th><th>Month 2</th><th>Month 3</th><th>Month 4</th></tr><tbody class='monthlyTable'>";
            var formDetails = string.Empty;
            using (var entities = new InventoryEntities())
            {
                var offertable = (from r in entities.V_FPVConsistancyNew2021
                                  select new
                                  {
                                      IDNo = r.FCode,
                                      MemName = r.MemName,
                                      Mobile = r.Mobl,
                                      City = r.City,
                                      RectimeStamp = r.billdate,
                                      userbilno = r.userBillno,
                                      BillNo = r.billNo,
                                      IntPCId = r.IdNo,
                                      intname = r.INTName,
                                      INTMobile = r.INTMobie
                                  }).OrderBy(r => r.IDNo).ToList();


                offertable = offertable.Where(r => (!string.IsNullOrEmpty(IdNo) && IdNo.ToLower() != "all" && IdNo.ToLower() != "0") ? r.IDNo.ToString().ToUpper() == IdNo.ToUpper() : 1 == 1).OrderBy(r => r.RectimeStamp).ToList();
                var formList = offertable.OrderBy(r => r.RectimeStamp).Select(r => r.IDNo).Distinct().ToList();
                int count = 1;


                foreach (var record in formList)
                {
                    var MinDate = offertable.Where(r => r.IDNo == record).Min(O => O.RectimeStamp);
                    var start = new DateTime(MinDate.Year, MinDate.Month, 01);
                    var end = new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month)).AddMonths(3);

                    while (end <= new DateTime(2023, 04, 01))
                    {
                        var FormRecords = offertable.Where(r => r.IDNo == record && r.RectimeStamp.Date >= start.Date && r.RectimeStamp.Date <= end.Date).OrderBy(r => r.RectimeStamp).ToList();
                        if (FormRecords.Count > 0 && !String.IsNullOrEmpty(FormRecords.Where(r => r.RectimeStamp.Month == start.Month && r.RectimeStamp.Year == start.Year).Select(r => r.userbilno).FirstOrDefault()))
                        {
                            formDetails += "<tr>";
                            formDetails += "<td><div style='width:50px'>" + count++ + "</div></td>";
                            formDetails += "<td><div style='width:120px'>" + FormRecords.FirstOrDefault().IDNo + "</div></td>";
                            formDetails += "<td><div style='width:200px'>" + FormRecords.FirstOrDefault().MemName + "</div></td>";
                            formDetails += "<td><div style='width:120px'>" + FormRecords.FirstOrDefault().Mobile + "</div></td>";
                            formDetails += "<td><div style='width:120px'>" + FormRecords.FirstOrDefault().City + "</div></td>";
                            formDetails += "<td><div style='width:200px'>" + FormRecords.FirstOrDefault().IntPCId + "</div></td>";
                            formDetails += "<td><div style='width:120px'>" + FormRecords.FirstOrDefault().intname + "</div></td>";
                            formDetails += "<td><div style='width:120px'>" + FormRecords.FirstOrDefault().INTMobile + "</div></td>";

                            var Month1 = FormRecords.Where(r => r.RectimeStamp.Month == start.Month && r.RectimeStamp.Year == start.Year).FirstOrDefault();
                            var Month2 = FormRecords.Where(r => r.RectimeStamp.Month == start.AddMonths(1).Month && r.RectimeStamp.Year == start.AddMonths(1).Year).FirstOrDefault();
                            var Month3 = FormRecords.Where(r => r.RectimeStamp.Month == start.AddMonths(2).Month && r.RectimeStamp.Year == start.AddMonths(2).Year).FirstOrDefault();
                            var Month4 = FormRecords.Where(r => r.RectimeStamp.Month == start.AddMonths(3).Month && r.RectimeStamp.Year == start.AddMonths(3).Year).FirstOrDefault();

                            var Month1Link = Month1 != null && !string.IsNullOrEmpty(Month1.BillNo) ? "<a target='_blank' style='text-decoration:underline; color:blue' href=/Transaction/InvoicePrint?Pm=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(Month1.userbilno)) + ">" + Month1.RectimeStamp.ToString("dd-MM-yyyy") + "</a>" : "";
                            var Month2Link = Month2 != null && !string.IsNullOrEmpty(Month2.BillNo) ? "<a target='_blank' style='text-decoration:underline; color:blue' href=/Transaction/InvoicePrint?Pm=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(Month2.userbilno)) + ">" + Month2.RectimeStamp.ToString("dd-MM-yyyy") + "</a>" : "";
                            var Month3Link = Month3 != null && !string.IsNullOrEmpty(Month3.BillNo) ? "<a target='_blank' style='text-decoration:underline; color:blue' href=/Transaction/InvoicePrint?Pm=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(Month3.userbilno)) + ">" + Month3.RectimeStamp.ToString("dd-MM-yyyy") + "</a>" : "";
                            var Month4Link = Month4 != null && !string.IsNullOrEmpty(Month4.BillNo) ? "<a target='_blank' style='text-decoration:underline; color:blue' href=/Transaction/InvoicePrint?Pm=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(Month4.userbilno)) + ">" + Month4.RectimeStamp.ToString("dd-MM-yyyy") + "</a>" : "";


                            formDetails += "<td><div style='width:120px'>" + Month1Link + "</div></td>";
                            formDetails += "<td><div style='width:120px'>" + Month2Link + "</div></td>";
                            formDetails += "<td><div style='width:120px'>" + Month3Link + "</div></td>";
                            formDetails += "<td><div style='width:120px'>" + Month4Link + "</div></td>";

                            formDetails += "</tr>";
                        }
                        start = end.AddMonths(1);
                        end = start.AddMonths(3);
                    }
                }
            }

            tableString += formDetails;
            tableString += "</tbody></table>";


            return tableString;
        }

        public List<FranchiseeCommission> GetFranchiseeBVCommission(string FromDate, string ToDate, string code, string Billtype)
        {
            List<FranchiseeCommission> lstFCommission = new List<FranchiseeCommission>();
            //ConnModel conobj = (System.Web.HttpContext.Current.Session["ConModel"] as ConnModel);
            try
            {
                DateTime StartDate = DateTime.Now;
                DateTime EndDate = DateTime.Now;
                if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                {
                    StartDate = Convert.ToDateTime(FromDate);
                    StartDate = StartDate.Date;
                }
                if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                {
                    EndDate = Convert.ToDateTime(ToDate);
                    EndDate = EndDate.Date;
                }

                if (FromDate.ToLower() == "all" || string.IsNullOrEmpty(FromDate))
                    StartDate = DateTime.Now.AddYears(-5);
                if (ToDate.ToLower() == "all" || string.IsNullOrEmpty(ToDate))
                    EndDate = DateTime.Now;

                string Procedurename = "exec sp_GetFranchiseBVCommission '" + code + "','" + StartDate.Date.ToString("yyyy/MM/dd") + "','" + EndDate.Date.ToString("yyyy/MM/dd") + "','" + Billtype + "'";
                string InvConstr = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConstr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = Procedurename;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();
                lstFCommission = new List<FranchiseeCommission>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        FranchiseeCommission FCommission = new FranchiseeCommission();
                        FCommission.Commission = Convert.ToString(reader["Commission"]);
                        FCommission.PartyName = Convert.ToString(reader["PartyName"]);
                        FCommission.PartyCode = Convert.ToString(reader["PartyCode"]);
                        FCommission.GroupName = Convert.ToString(reader["GroupName"]);
                        FCommission.BVCommission = Convert.ToString(reader["BVCommission"]);
                        FCommission.CommissionSlab = Convert.ToString(reader["CommissionSlab"]);
                        FCommission.BillNo = Convert.ToString(reader["BillNo"]);
                        FCommission.BillDate = Convert.ToString(reader["BillDate"]);
                        FCommission.BillType = Convert.ToString(reader["BillType"]);
                        FCommission.PVCommission = Convert.ToString(reader["PVCommission"]);
                        FCommission.PVCommissionVal = Convert.ToString(reader["PVCommissionVal"]);
                        FCommission.PVSlab = Convert.ToString(reader["PVSlab"]);
                        lstFCommission.Add(FCommission);
                    }
                }

            }
            catch (Exception Ex)
            {

            }
            return lstFCommission;
        }

        public List<MSessids> GetSessids()
        {
            List<MSessids> reuslt = new List<MSessids>();
            try
            {
                string Procedurename = "exec Sp_GetSessid";
                string InvConstr = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConstr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = Procedurename;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MSessids res = new MSessids();
                        res.SessID = Convert.ToDecimal(reader["SessID"]);
                        res.SessMonth = Convert.ToString(reader["SessMonth"]);
                        reuslt.Add(res);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return reuslt;
        }

        public List<MonthWiseIncome> GetMonthWiseIncome(string Sessid, string PartyCode)
        {
            List<MonthWiseIncome> reuslt = new List<MonthWiseIncome>();
            try
            {
                string Procedurename = "exec GEtMonthWiseIncome " + Sessid + ",'" + PartyCode + "'";
                string InvConstr = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConstr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = Procedurename;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MonthWiseIncome res = new MonthWiseIncome();
                        res.SessID = Convert.ToDecimal(reader["SessID"]);
                        res.FormNo = Convert.ToDecimal(reader["FormNo"]);
                        res.PartyCode = Convert.ToString(reader["PartyCode"]);
                        res.Self_Comm_BV = Convert.ToDecimal(reader["Self_Comm_BV"]);
                        res.Diff_Comm_BV = Convert.ToDecimal(reader["Diff_Comm_BV"]);
                        res.BV_Slab = Convert.ToDecimal(reader["BV_Slab"]);
                        res.SelfIncomePv = Convert.ToDecimal(reader["SelfIncomePv"]);
                        res.Diff_Comm_Pv = Convert.ToDecimal(reader["Diff_Comm_Pv"]);
                        res.PvSlab = Convert.ToDecimal(reader["PvSlab"]);
                        res.Gross_Amount = Convert.ToDecimal(reader["Gross_Amount"]);
                        res.Tds_Deduction = Convert.ToDecimal(reader["Tds_Deduction"]);
                        res.RndOff = Convert.ToDecimal(reader["RndOff"]);
                        res.NetIncome = Convert.ToDecimal(reader["NetIncome"]);
                        res.AdminCharge = Convert.ToDecimal(reader["AdminCharge"]);
                        reuslt.Add(res);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return reuslt;
        }

        public List<MPerformanceInc> GetPerformanceInc(string Partycode, string Action,int SessID)
        {
            List<MPerformanceInc> reuslt = new List<MPerformanceInc>();
            try
            {
                string Procedurename = "exec Sp_GetPerformanceInc '" + Partycode + "','" + Action + "',"+ SessID + "";
                string InvConstr = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConstr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = Procedurename;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MPerformanceInc res = new MPerformanceInc();
                        if (Action == "BV")
                        {
                            res.SessID = Convert.ToDecimal(reader["SessID"]);
                            res.PartyCode = Convert.ToString(reader["PartyCode"]);
                            res.From_ID = Convert.ToDecimal(reader["From_ID"]);
                            res.MLevel = Convert.ToDecimal(reader["MLevel"]);
                            res.Diff_Comm_BV = Convert.ToDecimal(reader["Diff_Comm_BV"]);
                            res.SelfSlab = Convert.ToDecimal(reader["SelfSlab"]);
                            res.DownSlab = Convert.ToDecimal(reader["DownSlab"]);
                            res.Diff_Comm_Pv = Convert.ToDecimal(reader["Diff_Comm_Pv"]);
                            res.Comm = Convert.ToDecimal(reader["Comm"]);
                            res.FromPartyCode = Convert.ToString(reader["FromPartyCode"]);
                        }

                        if (Action == "PV")
                        {
                            res.SessID = Convert.ToDecimal(reader["SessID"]);
                            res.PartyCode = Convert.ToString(reader["PartyCode"]);
                            //res.From_ID = Convert.ToDecimal(reader["From_ID"]);
                            res.MLevel = Convert.ToDecimal(reader["MLevel"]);
                            res.Pv = Convert.ToDecimal(reader["Pv"]);
                            res.SelfSlab = Convert.ToDecimal(reader["SelfSlab"]);
                            res.DownSlab = Convert.ToDecimal(reader["DownSlab"]);
                            res.Slab = Convert.ToDecimal(reader["Slab"]);
                            res.Comm = Convert.ToDecimal(reader["Comm"]);
                            res.FromPartyCode = Convert.ToString(reader["FromPartyCode"]);
                        }
                        reuslt.Add(res);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return reuslt;
        }

        public M_IncentiveStatement GetIncentiveStatement(string Partycode, string StatementPeriod)
        {
            M_IncentiveStatement obj = new M_IncentiveStatement();
            try
            {
                obj.StatementPeriodId =Convert.ToInt32(StatementPeriod);
                User objResponse = new User();
                using (var entity = new InventoryEntities())
                {
                   
                    objResponse = (from result in entity.Inv_M_UserMaster
                                   where result.ActiveStatus == "Y" && result.UserName == Partycode
                                   join ledger in entity.M_LedgerMaster on result.BranchCode equals ledger.PartyCode
                                   join groupid in entity.M_GroupMaster on ledger.GroupId equals groupid.GroupId
                                   select new User
                                   {
                                       PartyCode = ledger.PartyCode,
                                       PartyName = ledger.PartyName,
                                       IsApprove = ledger.ISApprove,
                                       GroupPrefix=groupid.Prefix,
                                       Address1=ledger.Address1
                                   }).FirstOrDefault();
                }

                obj.partycode = objResponse.PartyCode;
                obj.Address = objResponse.Address1;
                obj.GroupCode = objResponse.GroupPrefix;
                obj.PartyName = objResponse.PartyName;

                string InvConstr = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;

                string query = @"select CONVERT(varchar(100),FrmDate,106)+' To '+CONVERT(varchar(100),ToDate,106) as SessMonth 
                         from M_MonthSessnMaster   
                         where SessID=@Sessid";
                List<MonthWiseIncome> incomeresult = new List<MonthWiseIncome>();
                List<MPerformanceInc> mPerformanceIncs = new List<MPerformanceInc>();
                using (SqlConnection con = new SqlConnection(InvConstr))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Add parameter
                        cmd.Parameters.Add("@Sessid", SqlDbType.Decimal).Value = Convert.ToDecimal(StatementPeriod);
                        con.Open();
                        // Execute and read
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            obj.StatementPeriod = result.ToString();
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("sp_GetTotalPVBVValBySessID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        // Add parameters
                        cmd.Parameters.AddWithValue("@SessID", StatementPeriod);
                        cmd.Parameters.AddWithValue("@Partycode", Partycode);
                        

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                obj.TotalBVVal = reader["BvValue"] != DBNull.Value ? Convert.ToDecimal(reader["BvValue"]) : 0;
                                obj.TotalPVVal = reader["PVValue"] != DBNull.Value ? Convert.ToDecimal(reader["PVValue"]) : 0;
                                obj.CommOnPv = reader["CommOnPv"] != DBNull.Value ? Convert.ToDecimal(reader["CommOnPv"]) : 0;
                            }
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("GEtMonthWiseIncome", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        // Parameters
                        cmd.Parameters.Add("@Sessid", SqlDbType.Decimal).Value = StatementPeriod;
                        cmd.Parameters.Add("@PartyCode", SqlDbType.VarChar).Value = Partycode;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                incomeresult.Add(new MonthWiseIncome
                                {
                                    NetIncome = reader["NetIncome"] != DBNull.Value ? Convert.ToDecimal(reader["NetIncome"]) : 0,
                                    Self_Comm_BV = reader["Self_Comm_BV"] != DBNull.Value ? Convert.ToDecimal(reader["Self_Comm_BV"]) : 0,
                                    SelfIncomePv = reader["SelfIncomePv"] != DBNull.Value ? Convert.ToDecimal(reader["SelfIncomePv"]) : 0,
                                    BV_Slab = reader["BV_Slab"] != DBNull.Value ? Convert.ToDecimal(reader["BV_Slab"]) : 0,
                                    PvSlab = reader["PvSlab"] != DBNull.Value ? Convert.ToDecimal(reader["PvSlab"]) : 0,
                                    AdminCharge = reader["AdminCharge"] != DBNull.Value ? Convert.ToDecimal(reader["AdminCharge"]) : 0,
                                    Diff_Comm_BV = reader["Diff_Comm_BV"] != DBNull.Value ? Convert.ToDecimal(reader["Diff_Comm_BV"]) : 0,
                                    Diff_Comm_Pv = reader["Diff_Comm_Pv"] != DBNull.Value ? Convert.ToDecimal(reader["Diff_Comm_Pv"]) : 0
                                });
                            }
                        }

                        obj.ChequeAmount = incomeresult.Sum(x => x.NetIncome);
                        obj.self_Comm_BV = incomeresult.Sum(x => x.Self_Comm_BV);
                        obj.selfIncomePv = incomeresult.Sum(x => x.SelfIncomePv);
                        obj.bV_Slab = incomeresult.Sum(x => x.BV_Slab);
                        obj.pvSlab = incomeresult.Sum(x => x.PvSlab);
                        obj.AdminCharge = incomeresult.Sum(x => x.AdminCharge);
                        obj.Self_Comm_BV = incomeresult.Sum(x => x.Self_Comm_BV);
                        obj.Diff_Comm_BV = incomeresult.Sum(x => x.Diff_Comm_BV);
                        obj.Diff_Comm_Pv = incomeresult.Sum(x => x.Diff_Comm_Pv);
                    }

                    for (int i = 1; i <= 2; i++)
                    {
                        string Action = (i == 1) ? "BV" : "PV";

                        using (SqlCommand cmd = new SqlCommand("Sp_GetPerformanceInc", con))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;

                            // Parameters
                            cmd.Parameters.Add("@Partycode", SqlDbType.VarChar).Value = Partycode;
                            cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = Action;
                            cmd.Parameters.Add("@SessID", SqlDbType.Int).Value = StatementPeriod;

                            if (Action=="BV")
                            {
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        mPerformanceIncs.Add(new MPerformanceInc
                                        {
                                            FromPartyCode = reader["FromPartyCode"]?.ToString(),
                                            FranchiseeName = reader["FranchiseeName"]?.ToString(),
                                            Diff_Comm_BV = reader["Diff_Comm_BV"] != DBNull.Value ? Convert.ToDecimal(reader["Diff_Comm_BV"]) : 0,
                                            Diff_Comm_Pv = reader["Diff_Comm_Pv"] != DBNull.Value ? Convert.ToDecimal(reader["Diff_Comm_Pv"]) : 0,
                                            BvComm = reader["BvComm"] != DBNull.Value ? Convert.ToDecimal(reader["BvComm"]) : 0,
                                        });
                                    }
                                }
                            }

                            if (Action == "PV")
                            {
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        mPerformanceIncs.Add(new MPerformanceInc
                                        {
                                            FromPartyCode = reader["FromPartyCode"]?.ToString(),
                                            FranchiseeName = reader["FranchiseeName"]?.ToString(),
                                            PvComm = reader["PvComm"] != DBNull.Value ? Convert.ToDecimal(reader["PvComm"]) : 0,
                                            Slab = reader["Slab"] != DBNull.Value ? Convert.ToDecimal(reader["Slab"]) : 0,
                                            Pv = reader["Pv"] != DBNull.Value ? Convert.ToDecimal(reader["Pv"]) : 0,
                                        });
                                    }
                                }
                            }

                        }
                    }
                    var mPerformanceIncsgroup = mPerformanceIncs
                                             .GroupBy(x => x.FromPartyCode)
                .Select(g => new MPerformanceInc
                {
                    FromPartyCode = g.Key,
                    FranchiseeName = g.Select(x => x.FranchiseeName).FirstOrDefault(),
                    Diff_Comm_BV = g.Sum(x => x.Diff_Comm_BV),
                    Diff_Comm_Pv = g.Sum(x => x.Diff_Comm_Pv),
                    BvComm = g.Sum(x => x.BvComm),
                    Pv = g.Sum(x => x.Pv),
                    PvComm = g.Sum(x => x.PvComm),
                    Slab = g.Sum(x => x.Slab)
                })
                .ToList();

                    obj.DownlineFranchiseePerformance = mPerformanceIncsgroup;
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }

    }
}

