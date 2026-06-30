using InventoryManagement.App_Start;
using InventoryManagement.Business;
using InventoryManagement.Common;
using InventoryManagement.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace InventoryManagement.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        TransactionManager objTransactManager = new TransactionManager();
        ProductManager objProductManager = new ProductManager();
        ReportManager objReportManager = new ReportManager();
        // GET: Report
        [SessionExpire]
        public ActionResult StockReport()
        {
            StockReportModel objModel = new StockReportModel();
            objModel.ProductDetailsList = new List<ProductDetails>();

            //objModel.ProductDetailsList = objReportManager.GetAllProducts(0);
            //var jsonSerialiser = new JavaScriptSerializer();
            //if (objModel.ProductDetailsList.Count > 0)
            //{
            //    //var jsonProduct = jsonSerialiser.Serialize(objModel.ProductDetailsList);
            //    var jsonProduct=Json(objModel.ProductDetailsList, JsonRequestBehavior.AllowGet);
            //    jsonProduct.MaxJsonLength = int.MaxValue;
            //    ViewBag.ProductJsonList = jsonProduct;
            //}
            //else
            //{
            //    ViewBag.ProductJsonList = "";
            //}
            if (Session["LoginUserType"] as string == "mobileshoppe" || new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "StockReport"))
                return View(objModel);
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult GetStockReport(string CategoryCode, string ProductCode, string PartyCode, bool IsBatchWise, string StockType)
        {
            //**Added on 21Nov18
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;

            if (PartyCode == "" || CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                PartyCode = CurrentPartyCode;
            //**            

            List<StockReportModel> objStockReportModel = new List<StockReportModel>();
            objStockReportModel = objReportManager.GetStockReport(CategoryCode, ProductCode, PartyCode, IsBatchWise, StockType);
            return Json(objStockReportModel, JsonRequestBehavior.AllowGet);
        }

        //BillWise Sales Report
        [SessionExpire]
        public ActionResult SalesReport()
        {
            List<SelectListItem> objListInvoiceType = new List<SelectListItem>();
            objListInvoiceType.Add(new SelectListItem { Text = "All", Value = "" });        
            objListInvoiceType.Add(new SelectListItem { Text = System.Web.Configuration.WebConfigurationManager.AppSettings["RepurchaseCaption"], Value = "RI" });
            objListInvoiceType.Add(new SelectListItem { Text = System.Web.Configuration.WebConfigurationManager.AppSettings["JoiningCaption"], Value = "JI" });
            objListInvoiceType.Add(new SelectListItem { Text = "FPV Invoice", Value = "FPV" });
            objListInvoiceType.Add(new SelectListItem { Text = "GV Invoice", Value = "G" });
            objListInvoiceType.Add(new SelectListItem { Text = "Stock Transfer", Value = "S" });
            objListInvoiceType.Add(new SelectListItem { Text = "Customer Invoice", Value = "GC" });
            objListInvoiceType.Add(new SelectListItem { Text = "MRI Coupon Invoice", Value = "C" });
            objListInvoiceType.Add(new SelectListItem { Text = "Shopping Jackpot Invoice", Value = "J" });
            objListInvoiceType.Add(new SelectListItem { Text = "SJP Scratch Card Invoice", Value = "X" });

            objListInvoiceType.Add(new SelectListItem { Text = "CPV Invoice", Value = "P" });
            ViewBag.InvoiceTypes = objListInvoiceType;
            if (Session["LoginUserType"] as string == "mobileshoppe" || new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "SalesReport"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }
        [SessionExpire]
        public ActionResult DateSalesReport()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "DateSalesReport"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }
        [SessionExpire]
        public ActionResult ProductSalesReport()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "ProductSalesReport"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [SessionExpire]
        public ActionResult StockJvReport()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "StockJvReport"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult GetStockJvReport(string FromDate, string ToDate, string PartyCode,string ViewType)
        {
            List<StockJv> objStockJv = new List<StockJv>();

            //**Added on 21Nov18
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;

            if (PartyCode == "" || CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                PartyCode = CurrentPartyCode;
            //**
            objStockJv = objReportManager.GetStockJvReport(FromDate, ToDate, PartyCode, ViewType);
            var jsonStock = Json(objStockJv, JsonRequestBehavior.AllowGet);
            jsonStock.MaxJsonLength = int.MaxValue;
            return jsonStock;

        }

        [HttpPost]
        public ActionResult GetSalesReport(string FromDate, string ToDate, string CustomerId, string ProductCode, string CategoryCode, string PartyCode, string BType, string SalesType, string InvoiceType,string BillNo, string FType,string OfferType,string ReportType)
        {
            //**Added on 21Nov18
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
            
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
                

            if (PartyCode == "" || CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                PartyCode = CurrentPartyCode;
            //**
          
            List<SalesReport> objSalesList = new List<SalesReport>();
            objSalesList = objReportManager.GetSalesReport(FromDate, ToDate, CustomerId, ProductCode, CategoryCode, PartyCode, BType, SalesType, InvoiceType, BillNo,FType, OfferType, ReportType);
            if (Session["LoginUserType"] as string == "mobileshoppe")
            {
                var username = (Session["LoginUser"] as User).UserName;
                objSalesList = objSalesList.Where(r => r.UserName == username).ToList();
            }

                var jsonProduct = Json(objSalesList, JsonRequestBehavior.AllowGet);
                jsonProduct.MaxJsonLength = int.MaxValue;
                return jsonProduct;
            //string returnresult = "";
            //if (string.IsNullOrEmpty(FromDate))
            //{
            //    returnresult = ToDate;
            //}
            //else if(string.IsNullOrEmpty(ToDate))
            //{
            //    returnresult = FromDate;
            //}
            //else
            //{
            //    returnresult = "All right";
            //}
            //return Json(returnresult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPurchaseSummary(string FromDate, string ToDate, string PartyCode, string SupplierCode, string ReportType,string InvoiceNo)
        {
            List<PurchaseReport> objPurchaseList = new List<PurchaseReport>();
            objPurchaseList = objReportManager.GetPurchaseSummary(FromDate, ToDate, PartyCode, SupplierCode, ReportType, InvoiceNo);
            var jsonProduct = Json(objPurchaseList, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;
            
        }

        [SessionExpire]
        public ActionResult PurchaseInvoicePrint(string Pm)
        {
            List<PurchaseReport> objInvoice = new List<PurchaseReport>();
            if (Session["LoginUser"] != null)
            {
                var base64DecodedBytes = System.Convert.FromBase64String(Pm);
                string InvoiceNoValue = System.Text.Encoding.UTF8.GetString(base64DecodedBytes);
                objInvoice = objTransactManager.GetPurchaseInvoice(InvoiceNoValue);
            }            
            return View(objInvoice);
        }
        public ActionResult GetAllCategory()
        {
            List<CategoryDetails> objCategory = new List<CategoryDetails>();
            objCategory = objProductManager.GetCategoryList("Y");

            return Json(objCategory, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public ActionResult GetAllProduct(decimal CategoryCode)
        {
            List<ProductDetails> objProduct = new List<ProductDetails>();
            objProduct = objReportManager.GetAllProducts(CategoryCode);
            var jsonProduct = Json(objProduct, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;

        }
        //public ActionResult GetAllParty()
        //{
        //    List<PartyModel> objparty = new List<PartyModel>();
        //    objparty = objReportManager.GetAllParty();
        //    return Json(objparty, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetAllParty()
        {
            List<PartyModel> objparty = new List<PartyModel>();
            string LoginPartyCode = "";
            decimal LoginStateCode = 0;
            if (Session["LoginUser"] != null)
            {
                LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
                LoginStateCode = (Session["LoginUser"] as User).StateCode;
            }
            objparty = objTransactManager.GetAllParty(LoginPartyCode, LoginStateCode);
            return Json(objparty, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllPartyListForReports()
        {
            List<PartyModel> objparty = new List<PartyModel>();
            
            objparty = objReportManager.GetAllParty(false);
            return Json(objparty, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetAllPartyListForReportswithMS()
        {
            List<PartyModel> objparty = new List<PartyModel>();

            objparty = objReportManager.GetAllParty(true);
            return Json(objparty, JsonRequestBehavior.AllowGet);
        }
        [SessionExpire]
        public ActionResult PurchaseSummary()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "PurchaseSummary"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }
       
        public ActionResult GetSupplier()
        {
            List<PartyModel> objListSupplier = new List<PartyModel>();
            decimal LoginStateCode = (Session["LoginUser"] as User).StateCode;
            string LoginPartyCode= (Session["LoginUser"] as User).PartyCode;
            objListSupplier = objTransactManager.GetAllSupplierList(LoginPartyCode,LoginStateCode);
            return Json(objListSupplier, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult PurchaseDetailSummary()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "PurchaseDetailSummary"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult GetMonthWisePurchaseSummary(string Year, bool IsQuantity, bool IsAmount, string PartyCode, string SupplierCode)
        {
            List<PurchaseReport> objPurchaseList = new List<PurchaseReport>();
            objPurchaseList = objReportManager.GetMonthWisePurchaseSummary(Year,IsQuantity, IsAmount, PartyCode, SupplierCode);
            var jsonProduct = Json(objPurchaseList, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;

        }

        [HttpPost]
        public ActionResult GetMonthWiseSalesSummary(string Year, bool IsQuantity, bool IsAmount, string PartyCode)
        {
            //**Added on 21Nov18
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;

            if (PartyCode == "" || CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                PartyCode = CurrentPartyCode;
            //**
            List<SalesReport> objSalesList = new List<SalesReport>();
            objSalesList = objReportManager.GetMonthWiseSalesSummary(Year, IsQuantity, IsAmount, PartyCode);
            var jsonProduct = Json(objSalesList, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;

        }

        [HttpPost]
        public ActionResult GetPurchaseDetailSummary(string FromDate, string ToDate, string PartyCode, string SupplierCode, string ProductCode)
        {
            List<PurchaseReport> objPurchaseList = new List<PurchaseReport>();
            objPurchaseList = objReportManager.GetPurchaseDetailSummary(FromDate, ToDate, PartyCode, SupplierCode, ProductCode);
            var jsonProduct = Json(objPurchaseList, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;

        }
        [SessionExpire]
        public ActionResult OrderHistory()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "OrderHistory"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }
        [SessionExpire]
        public ActionResult MonthlySummary()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "MonthlySummary"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult GetMonthlyReport(string PartyCode, string BillType)
        {
            List<MonthlySumm> objSumm = new List<MonthlySumm>();
            objSumm = objReportManager.GetMonthlyReport(PartyCode, BillType);
            var jsonProduct = Json(objSumm, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;
        }

        [SessionExpire]
        public ActionResult MonthWiseSalesSummary()
        {
            List<string> objYear = new List<string>();
            objYear = objReportManager.GetSalesYearList();
            List<SelectListItem> objList = new List<SelectListItem>();
            if (objYear.Count() > 0)
            {
                objList.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "0"
                });
            }
            else
            {
                objList.Add(new SelectListItem
                {
                    Text = "--No Record--",
                    Value = "0"
                });
            }
            foreach (var obj in objYear)
            {
                objList.Add(new SelectListItem
                {
                    Text = obj,
                    Value = obj
                });
            }
            ViewBag.ListYear = objList;
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "MonthWiseSalesSummary"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }
        [SessionExpire]
        public ActionResult MonthWisePurchaseSummary()
        {
            List<string> objYear = new List<string>();
            objYear = objReportManager.GetYearList();
            List<SelectListItem> objList = new List<SelectListItem>();
            if (objYear.Count() > 0)
            {
                objList.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "0"
                });
            }
            else
            {
                objList.Add(new SelectListItem
                {
                    Text= "--No Record--",
                    Value = "0"
                });
            }
            foreach (var obj in objYear)
            {
                objList.Add(new SelectListItem
                {
                    Text = obj,
                    Value = obj
                });
            }
            ViewBag.ListYear = objList;
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "MonthWisePurchaseSummary"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [SessionExpire]
        public ActionResult StockReceiptReport()
        {
            StockReportModel objModel = new StockReportModel();
            objModel.ProductDetailsList = new List<ProductDetails>();
            objModel.StateList = objReportManager.GetStateList();
            string LoginPartyName = (Session["LoginUser"] as User).PartyName;
            string LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
            int LoginGroupId = (Session["LoginUser"] as User).GroupId;
            objModel.PartyName = LoginPartyName;
            objModel.PartyCode = LoginPartyCode;
            objModel.GroupId = LoginGroupId;
            objModel.CategoryList = objProductManager.GetCategoryList("Y");
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "StockReceiptReport"))
                return View(objModel);
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult GetStockReceiptReport(string CategoryCode, string ProductCode, string PartyCode, string StateCode, string FromDate, string ToDate, string isSummary)
        {
            List<StockReportModel> objStockReportModel = new List<StockReportModel>();
            string LoginPartyCode = "";
            if (Session["LoginUser"] != null)
            {
                LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
            }
            objStockReportModel = objReportManager.GetStockReceiptReport(CategoryCode, ProductCode, PartyCode, StateCode, FromDate, ToDate, LoginPartyCode, isSummary);
            var jsonProduct = Json(objStockReportModel, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;
            //return Json(objStockReportModel, JsonRequestBehavior.AllowGet);
        }
        [SessionExpire]
        public ActionResult PartyWiseBalanceReport()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "PartyWiseBalanceReport"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }
        [HttpPost]
        public ActionResult GetPartyWiseWalletReport(string FromDate, string ToDate, string PartyCode, string ViewType)
        {
            //**Added on 21Nov18
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;

            if (PartyCode == "" || CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                PartyCode = CurrentPartyCode;
            //**
            List<PartyWiseWalletDetails> objWalletDetails = new List<PartyWiseWalletDetails>();
            objWalletDetails = objReportManager.GetPartyWiseWalletReport(FromDate, ToDate,PartyCode,ViewType);
            var jsonProduct = Json(objWalletDetails, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;
           
        }
        [SessionExpire]
        public ActionResult PaymentSummary()
        {
            PaymentSummary objPaymentSummary = new PaymentSummary();
            try
            {
                if (Session["LoginUser"] != null)
                {
                    objPaymentSummary.PaymentMode = objReportManager.GetPaymodeList();
                    string LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
                    string LoginPartyName = (Session["LoginUser"] as User).PartyName;
                    int LoginGroupId = (Session["LoginUser"] as User).GroupId;
                    objPaymentSummary.PartyName = LoginPartyName;
                    objPaymentSummary.PartyCode = LoginPartyCode;
                    objPaymentSummary.GroupId = LoginGroupId;
                }
            }
            catch (Exception ex)
            {

            }
            return View(objPaymentSummary);
        }

        [HttpPost]
        public ActionResult GetPaymentSummaryReport(string FromDate, string ToDate, string PartyCode, string Type)
        {
            //**Added on 21Nov18
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;

            if (PartyCode == "" || CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                PartyCode = CurrentPartyCode;
            //**
            List<PaymentSummaryReport> objWalletDetails = new List<PaymentSummaryReport>();
            objWalletDetails = objReportManager.GetPaymentSummaryReport(FromDate, ToDate, PartyCode, Type);
            var jsonProduct = Json(objWalletDetails, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;
        }
        [SessionExpire]
        public ActionResult SaleRegister()
        {
            PaymentSummary objPaymentSummary = new PaymentSummary();
            try
            {
                if (Session["LoginUser"] != null)
                {
                    string LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
                }                
            }
            catch (Exception ex)
            {

            }
            return View(objPaymentSummary);
        }

        [HttpPost]
        public ActionResult GetSaleRegisterReport(string FromDate, string ToDate, string PartyCode)
        {
            List<SaleRegister> objWalletDetails = new List<SaleRegister>();
            //**Added on 21Nov18
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;

            if (PartyCode == "" || CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                PartyCode = CurrentPartyCode;
            //**
            objWalletDetails = objReportManager.GetSaleRegisterReport(FromDate, ToDate, PartyCode);
            var jsonProduct = Json(objWalletDetails, JsonRequestBehavior.AllowGet);            
            return jsonProduct;
        }

        [HttpPost]
        public ActionResult GetProductSaleRegisterReport(string FromDate, string ToDate, string PartyCode)
        {
            List<SaleRegister> objWalletDetails = new List<SaleRegister>();
            //**Added on 21Nov18
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;

            if (PartyCode == "" || CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                PartyCode = CurrentPartyCode;
            //**
            objWalletDetails = objReportManager.GetProductSaleRegisterReport(FromDate, ToDate, PartyCode);
            var jsonProduct = Json(objWalletDetails, JsonRequestBehavior.AllowGet);
            return jsonProduct;
        }

        [SessionExpire]
        public ActionResult SalesReturnReport()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "SalesReturnReport"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult GetSalesReturnReport(string FromDate, string ToDate, string ProductCode, string CategoryCode, string PartyCode, string PartyType, string Type)
        {
            List<SalesReturnReport> objWalletDetails = new List<SalesReturnReport>();
            objWalletDetails = objReportManager.GetSalesReturnReport(FromDate, ToDate, ProductCode, CategoryCode, PartyCode, PartyType, Type);
            var jsonProduct = Json(objWalletDetails, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;
        }

        [SessionExpire]
        public ActionResult PurchaseReturnReport()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "PurchaseReturnReport"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult GetPurchaseReturnReport(string FromDate, string ToDate, string ProductCode, string CategoryCode, string SupplierCode, string Type)
        {
            List<SalesReturnReport> objWalletDetails = new List<SalesReturnReport>();
            objWalletDetails = objReportManager.GetPurchaseReturnReport(FromDate, ToDate, ProductCode, CategoryCode, SupplierCode, Type);
            var jsonProduct = Json(objWalletDetails, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;
        }

        [SessionExpire]
        public ActionResult PartyTargetReport()
        {
            try
            {
                if (Session["LoginUser"] != null)
                {
                    string LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
                }

                List<SelectListItem> objCategoryList = new List<SelectListItem>();
                var result = objProductManager.GetCategoryList("Y");
                SubCategoryDetails model = new SubCategoryDetails();
                bool f = true;
                foreach (var item in result)
                {
                    SelectListItem tempobj = new SelectListItem();
                    //SelectListItem tempobj1 = new SelectListItem();
                    tempobj.Text = item.CategoryName;
                    tempobj.Value = item.CategoryId.ToString();
                    if (f == true)
                    {
                        f = false;
                        model.CategoryId = int.Parse(item.CategoryId.ToString());
                        //model.SubCatId = int.Parse(item.ToString());
                    }

                    objCategoryList.Add(tempobj);
                }

                ViewBag.ListCategory = objCategoryList;
            }
            catch (Exception ex)
            {

            }
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "PartyTargetReport"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [SessionExpire]
        public ActionResult OfferReport()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "OfferReport"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        public ActionResult GetOfferReport(string PartyCode)
        {
            List<OfferReport> ObjResult = new List<Entity.Common.OfferReport>();
            string LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
            if (PartyCode == null || PartyCode == "" || PartyCode == "0")
            {
                if (LoginPartyCode == System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                    PartyCode = "All";
                else
                    PartyCode = LoginPartyCode;
            }
            ObjResult = objReportManager.GetOfferReport(PartyCode);
            var jsonProduct = Json(ObjResult, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;
        }
        [SessionExpire]
        public ActionResult InvWiseOfferReport(string Pm)

        {
            var base64DecodedBytes = System.Convert.FromBase64String(Pm);
            string req = System.Text.Encoding.UTF8.GetString(base64DecodedBytes);
            decimal OfferID = Convert.ToDecimal( req.Split('|')[0].ToString());
            string Partycode =  req.Split('|')[1].ToString();
           
            OfferReport Obj = new OfferReport();
            Obj.OfferUID = OfferID;
            Obj.FCode = Partycode;
            return View(Obj);

        }

        public ActionResult GetBillWiseOfferReport(string Pm, string SoldBy)
        {
            var base64DecodedBytes = System.Convert.FromBase64String(Pm);
            decimal OfferID = Convert.ToDecimal( System.Text.Encoding.UTF8.GetString(base64DecodedBytes));
            List<OfferReport> ObjResult = new List<Entity.Common.OfferReport>();
            string LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
            if (SoldBy == null || SoldBy==""  || SoldBy == "0"|| SoldBy== "OVER ALL")
            {
                if (LoginPartyCode == System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                    SoldBy = "All";
                else
                    SoldBy = LoginPartyCode;
            }
            ObjResult = objReportManager.GetBillWiseOfferReport(OfferID, SoldBy);
            var jsonProduct = Json(ObjResult, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;
        }

        [SessionExpire]
        public ActionResult ProdWiseOfferReport(string Pm)
        {
            var base64DecodedBytes = System.Convert.FromBase64String(Pm);
            string req =System.Text.Encoding.UTF8.GetString(base64DecodedBytes);
            decimal OfferID = Convert.ToDecimal(req.Split('|')[0].ToString());
            string Partycode = req.Split('|')[1].ToString();
            OfferReport Obj = new OfferReport();
            Obj.OfferUID = OfferID;
            Obj.FCode = Partycode;
            return View(Obj);
        }

        public ActionResult GetProdWiseOfferReport(string Pm,string PartyCode)
        {
            var base64DecodedBytes = System.Convert.FromBase64String(Pm);
            decimal OfferID = Convert.ToDecimal(System.Text.Encoding.UTF8.GetString(base64DecodedBytes));
            List<OfferReport> ObjResult = new List<Entity.Common.OfferReport>();
            string LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
            if (PartyCode == null || PartyCode == "" || PartyCode == "0" || PartyCode == "OVER ALL")
            {
                if (LoginPartyCode == System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                    PartyCode = "All";
                else
                    PartyCode = LoginPartyCode;
            }

            ObjResult = objReportManager.GetProdWiseOfferReport(OfferID, PartyCode);            
            var jsonProduct = Json(ObjResult, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;
        }
        [SessionExpire]
        public ActionResult DateWiseStockReport()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "DateWiseStockReport"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult GetDateWiseStockReport(string CategoryCode, string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            //**Added on 24Nov18
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;

            if (PartyCode == "" || CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                PartyCode = CurrentPartyCode;
            //**
            List<StockReportModel> objStockReportModel = new List<StockReportModel>();
            objStockReportModel = objReportManager.GetDateWiseStockReport(CategoryCode, ProductCode, PartyCode, FromDate, ToDate);
            return Json(objStockReportModel, JsonRequestBehavior.AllowGet);
        }
        [SessionExpire]
        public ActionResult DailyStockReport()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "DailyStockReport"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult GetDailyStockReport(string CategoryCode, string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            //**Added on 24Nov18
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;

            if (PartyCode == "" || CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                PartyCode = CurrentPartyCode;
            //**
            List<StockReportModel> objStockReportModel = new List<StockReportModel>();
            objStockReportModel = objReportManager.GetDailyStockReport(CategoryCode, ProductCode, PartyCode, FromDate, ToDate);
            return Json(objStockReportModel, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult FrWiseStock()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "FrWiseStock"))
                return View("DailyFrStockReport");
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult GetFrDailyStockReport(string CategoryCode, string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            //**Added on 24Nov18
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;

            if (PartyCode == "" || CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                PartyCode = CurrentPartyCode;
            //**
            List<StockReportModel> objStockReportModel = new List<StockReportModel>();
            objStockReportModel = objReportManager.GetDailyFrStockReport(CategoryCode, ProductCode, PartyCode, FromDate, ToDate);
            return Json(objStockReportModel, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult DailyStockValue()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "DailyStockReport"))
                return View("DailyFrStockReport");
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult GetDailyFrStockReport(string CategoryCode, string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            //**Added on 24Nov18
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;

            if (PartyCode == "" || CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                PartyCode = CurrentPartyCode;
            //**
            List<StockReportModel> objStockReportModel = new List<StockReportModel>();
            objStockReportModel = objReportManager.GetDailyFrStockReport(CategoryCode, ProductCode, PartyCode, FromDate, ToDate);
            return Json(objStockReportModel, JsonRequestBehavior.AllowGet);
        }


        [SessionExpire]
        public ActionResult SampleProductReport()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "SampleProductReport"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult GetSampleProductReport(string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            //**Added on 24Nov18
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;

            if (PartyCode == "" || CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                PartyCode = CurrentPartyCode;
            //**
            List<IssueSampleProduct> objStockReportModel = new List<IssueSampleProduct>();
            objStockReportModel = objReportManager.GetSampleProductReport(ProductCode, PartyCode, FromDate, ToDate);
            return Json(objStockReportModel, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult ShoppeReport()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "ShoppeReport"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult GetShoppeReport(string PartyCode)
        {
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;

            if (PartyCode == "" && CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                PartyCode = CurrentPartyCode;

            DashboardSummary objresult = new DashboardSummary();
            objresult.columns = objReportManager.getDasboardColumns();
            objresult.DashboardData = objReportManager.GetShoppeReport(PartyCode);
            return Json(objresult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSRProductList(string STRNo)
        {
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;            
            string list =  objReportManager.GetSRProductList(STRNo);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSampleProductList(string TransNo)
        {
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
            string list = objReportManager.GetSampleProductList(TransNo);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult StockSummary()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "StockSummary"))
            {
                string CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
                if (CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                    return RedirectToAction("Dashboard", "Home");

                return View();
            }
            else
                return RedirectToAction("Dashboard", "Home");
        }

        public string GetStockSumm(string StockDate, int DateDif)
        {
            try
            {

                DateTime startDate = Convert.ToDateTime(StockDate).AddDays(DateDif);
                // DateTime endDate = Convert.ToDateTime(ToDate);
                ///string PartyCond = "";
                //  if (PartyCode != "" && PartyCode.ToUpper() != "ALL" && PartyCode != "0") PartyCond = " AND PartyCode='" + PartyCode + "'";
                string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(AppConnectionString);
                string query = "StockSummary '" + startDate.ToString("dd-MMM-yyyy") + "'";
                SqlCommand cmd = new SqlCommand();
                SC.Open();
                cmd.CommandText = query;
                cmd.Connection = SC;
                SqlDataAdapter Da = new SqlDataAdapter(cmd);
                DataTable Dt = new DataTable();
                Da.Fill(Dt);
                SC.Close();

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in Dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in Dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }

                return serializer.Serialize(rows);// Json(serializer.Serialize(rows), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return "";// Json("", JsonRequestBehavior.AllowGet); ;
            }

        }

        [SessionExpire]
        public ActionResult GVCreditNote()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "GVCreditNote"))
            {
                string CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
                //if (CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                //    return RedirectToAction("Dashboard", "Home");

                return View();
            }
            else
                return RedirectToAction("Dashboard", "Home");
        }

        public ActionResult GetGVCreditNote(string PartyCode, string FromDate, string ToDate)
        {
            try
            {
                string CurrentPartyCode = "";
                if (Session["LoginUser"] != null)
                    CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
                var list = objReportManager.GetGVCreditNote(PartyCode, FromDate, ToDate);
                return Json(list, JsonRequestBehavior.AllowGet);
                

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [SessionExpire]
        public ActionResult ConsistentFPVOfferReport()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "ConsistentFPVOfferReport"))
            {
                string CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
            if (CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                return RedirectToAction("Dashboard", "Home");

            return View();
            }
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [SessionExpire]
        public ActionResult ConsistentFPVOfferReport2021()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "ConsistentFPVOfferReport"))
            {
                string CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
                if (CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                    return RedirectToAction("Dashboard", "Home");

                return View();
            }
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [SessionExpire]
        public ActionResult WalletRequestReport()
        {
            if (Session["LoginUserType"] as string == "mobileshoppe" || new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "WalletRequestReport"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        public ActionResult GetConsistentFPVOffer(string IdNo, bool TeamWise)
        {
            try
            {
                string CurrentPartyCode = "";
                if (Session["LoginUser"] != null)
                    CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
                var list = objReportManager.GetConsistentFPVOffer(IdNo,TeamWise);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public ActionResult GetMonthlyConsistentFPVOffer(string IdNo,bool teamWise)
        {
            try
            {
                string CurrentPartyCode = "";
                if (Session["LoginUser"] != null)
                    CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
                var list = objReportManager.GetMonthlyConsistentFPVOffer(IdNo, teamWise);
               
                var jsonProduct = Json(list, JsonRequestBehavior.AllowGet);
                jsonProduct.MaxJsonLength = int.MaxValue;
                return jsonProduct;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
       
        

        [SessionExpire]
        public ActionResult WalletReport()
        {
            if (Session["LoginUserType"] as string == "mobileshoppe" || new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "WalletReport"))                
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult GetWalletHistory(string FromDate, string ToDate, string PartyCode, string vtype)
        {
            string CurrentPartyCode = "";
            //string vtype = string.Empty;
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;

            if (CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"] || PartyCode == "" || PartyCode.ToUpper() == "ALL")
            {
                if (Session["LoginUserType"] as string == "mobileshoppe")
                {
                    PartyCode = (Session["LoginUser"] as User).UserName;
                }
                else
                {
                    PartyCode = (Session["LoginUser"] as User).PartyCode;
                }
            }
            //if (Session["LoginUserType"] as string == "mobileshoppe")
            //{                
            //    vtype = "M";
            //}
            //else
            //{
            //    vtype = "B";
            //}
            List<SalesReport> objSalesReport = new List<SalesReport>();
            objSalesReport = objReportManager.GetWalletHistory(FromDate, ToDate, PartyCode,vtype);
                        
            return Json(objSalesReport, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult ShoppeStockReport()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "WalletReport"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult GetShoppeStockReport(string PartyCode, string ProductCode, int Month, int year )
        {            

            var objSalesReport = objReportManager.GetShoppeStockReport(PartyCode,ProductCode, Month,year);
            return Json(objSalesReport, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult MRICouponReport()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "WalletReport"))
                return View("MRICouponCreditNote");
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult GetMRICouponReport(string PartyCode, string FromDate, string ToDate)
        {

            var objSalesReport = objReportManager.GetMRICouponReport(PartyCode, FromDate, ToDate);
            return Json(objSalesReport, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult SJPConsistentReport()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "WalletReport"))
                return View("SJPConsistentReport");
            else
                return RedirectToAction("Dashboard", "Home");
        }

        public ActionResult GetSJPConsistentReport(string IdNo)
        {
            try
            {
                string CurrentPartyCode = "";
                if (Session["LoginUser"] != null)
                    CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
                var list = objReportManager.GetSJPConsistentReport(IdNo);

                var jsonProduct = Json(list, JsonRequestBehavior.AllowGet);
                jsonProduct.MaxJsonLength = int.MaxValue;
                return jsonProduct;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [SessionExpire]
        public ActionResult FPVConsitencyReport2021()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "WalletReport"))
                return View("FPVConsitencyReport2021");
            else
                return RedirectToAction("Dashboard", "Home");
        }

        public ActionResult GetFPVConsitencyReport(string IdNo)
        {
            try
            {
                string CurrentPartyCode = "";
                if (Session["LoginUser"] != null)
                    CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
                var list = objReportManager.GetFPVConsitencyReport(IdNo);

                var jsonProduct = Json(list, JsonRequestBehavior.AllowGet);
                jsonProduct.MaxJsonLength = int.MaxValue;
                return jsonProduct;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [SessionExpire]
        public ActionResult FranchiseeCommission() 
        {
            FranchiseeCommission objfCommission = new Entity.Common.FranchiseeCommission();
            return View(objfCommission);
        }

        [HttpPost]
        public ActionResult GetFranchiseeBVCommission(string FromDate, string ToDate, string code, string Billtype)
        {
            List<FranchiseeCommission> lstFCommission = null;
            lstFCommission = objReportManager.GetFranchiseeBVCommission(FromDate, ToDate, code, Billtype);
            return Json(lstFCommission, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult PayoutSummary()
        {
            M_PayoutSummary obj = new M_PayoutSummary();
            obj.MSessids = objReportManager.GetSessids();
            return View(obj);
        }

        public ActionResult GetMonthWiseIncome(string Sessid,string PartyCode)
        {   
            List<MonthWiseIncome> objMonthWiseIncome = new List<MonthWiseIncome>();
            objMonthWiseIncome = objReportManager.GetMonthWiseIncome(Sessid, PartyCode);
            return Json(objMonthWiseIncome);
        }

        public ActionResult GetPerformanceInc(string Action,string PartyCode, string SessID)
        { 

            List<MPerformanceInc> obj = objReportManager.GetPerformanceInc(PartyCode, Action,Convert.ToInt32(SessID));

            return Json(obj);
        }

        public ActionResult PayoutStatement(string Prm)
        {
            var base64DecodedBytes = System.Convert.FromBase64String(Prm);
            string StatementPeriod = System.Text.Encoding.UTF8.GetString(base64DecodedBytes);
            var splitstr = StatementPeriod.Split(';');
            string Partycode = splitstr[0];
            string Sessid = splitstr[1];
            M_IncentiveStatement obj = new M_IncentiveStatement();
            obj = objReportManager.GetIncentiveStatement(Partycode, Sessid);
            return View(obj);
        }
    }
}