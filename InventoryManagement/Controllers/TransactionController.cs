using InventoryManagement.App_Start;
using InventoryManagement.Business;
using InventoryManagement.Common;
using InventoryManagement.Entity.Common;

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using NPOI.SS.UserModel;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using InventoryManagement.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace InventoryManagement.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        // GET: Transaction

        TransactionManager objTransacManager = new TransactionManager();
        ProductManager objProductManager = new ProductManager();
        RegistrationManager objRegistrationManager = new RegistrationManager();
        [SessionExpire]
        public ActionResult DistributorBill()
        {

            DistributorBillModel objDistributorModel = new DistributorBillModel();
            try
            {

                if (Session["LoginUserType"] as string == "mobileshoppe" || CanUserAccessMenu((Session["LoginUser"] as User).UserId, "DistributorBill"))
                {
                    objDistributorModel.objCustomer = new CustomerDetail();
                    objDistributorModel.objProduct = new ProductModel();
                    List<SelectListItem> objBankList = new List<SelectListItem>();
                    var result = objTransacManager.GetBankList();
                    objDistributorModel.objProduct.PayDetails = new PayDetails();
                    foreach (var obj in result)
                    {
                        if (obj.BankCode == 0)
                        {
                            objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString(), Selected = true });
                            objDistributorModel.objProduct.PayDetails.BDBankName = obj.BankCode.ToString();
                        }
                        else
                        {
                            objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString() });
                        }
                    }
                    ViewBag.BankNames = objBankList;
                    List<SelectListItem> CardTypes = new List<SelectListItem>();
                    CardTypes.Add(new SelectListItem { Text = "Credit Card", Value = "CC" });
                    CardTypes.Add(new SelectListItem { Text = "Debit Card", Value = "DB" });
                    ViewBag.CardTypes = CardTypes;

                    objDistributorModel.objProduct.PayDetails.CardType = "CC";

                    //List<SelectListItem> objListCustomerTypes = new List<SelectListItem>();
                    //objListCustomerTypes.Add(new SelectListItem { Text = "New", Value = "New" });
                    //objListCustomerTypes.Add(new SelectListItem { Text = "Existing", Value = "Existing" });
                    //ViewBag.CustomerType = objListCustomerTypes;

                    //ViewBag.ConfigDetails = objTransacManager.GetConfigDetails();
                    //objDistributorModel.objConfigDetails = objTransacManager.GetConfigDetails();

                    objDistributorModel.objCustomer.CustomerType = "New";
                    //var OfferList = objTransacManager.GetOfferList();
                    List<SelectListItem> OfferSelectList = new List<SelectListItem>();
                    OfferSelectList.Add(new SelectListItem { Text = "--Choose Offer--", Value = "0" });
                    //foreach (var obj in OfferList)
                    //{ OfferSelectList.Add(new SelectListItem { Value = obj.ProdCode.ToString(), Text = obj.ProductName }); }
                    ViewBag.OfferList = OfferSelectList;

                    var KitIdlist = objTransacManager.GetKitIdList();
                    List<SelectListItem> KidIsListObj = new List<SelectListItem>();
                    KidIsListObj.Add(new SelectListItem { Text = "--Select Kit--", Value = "0" });
                    foreach (var obj in KitIdlist)
                    {
                        KidIsListObj.Add(new SelectListItem { Text = obj.KitName, Value = obj.KitId.ToString() });
                    }
                    ViewBag.objKitList = KidIsListObj;
                    objDistributorModel.objCustomer.KitId = 0;
                    //objDistributorModel.objCustomer.IsRegisteredCustomer = true;
                    //objDistributorModel.objCustomer.ReferenceIdNo = InventorySession.LoginUser.PartyCode;
                    //objDistributorModel.objCustomer.ReferenceName = InventorySession.LoginUser.PartyName;
                    InventorySession.StoredDistributorValues = null;
                    return View(objDistributorModel);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                Console.Write("Exception:", ex.Message);
            }
            return RedirectToAction("Dashboard", "Home");
        }

        public ActionResult GetOfferList(string Doj, string UpgradeDate, bool IsFirstBill, bool ActiveStatus)
        {
            string CurrentParty = (Session["LoginUser"] as User).PartyCode;
            return Json(objTransacManager.GetOfferList(Doj, UpgradeDate, IsFirstBill, ActiveStatus, CurrentParty), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetKitDescription(decimal KitId)
        {
            return Json(objTransacManager.GetKitDescription(KitId), JsonRequestBehavior.AllowGet);
        }
        public void LogError(Exception ex)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", "DistributorBill");
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += Environment.NewLine;
            message += string.Format("Source: {0}", ex.Source);
            message += Environment.NewLine;
            message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            string path = Server.MapPath("~/ErrorLog/ErrorLog.txt");
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }
        [SessionExpire]
        public ActionResult PartyBill()
        {
            if (Session["LoginUser"] != null)
                if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "PartyBill"))
                {
                    DistributorBillModel objDistributorModel = new DistributorBillModel();
                    objDistributorModel.objCustomer = new CustomerDetail();
                    objDistributorModel.objProduct = new ProductModel();
                    List<SelectListItem> objBankList = new List<SelectListItem>();
                    var result = objTransacManager.GetBankList();
                    objDistributorModel.objProduct.PayDetails = new PayDetails();
                    foreach (var obj in result)
                    {
                        if (obj.BankCode == 0)
                        {
                            objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString(), Selected = true });
                            objDistributorModel.objProduct.PayDetails.BDBankName = obj.BankCode.ToString();
                        }
                        else
                        {
                            objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString() });
                        }
                    }
                    ViewBag.BankNames = objBankList;
                    List<SelectListItem> CardTypes = new List<SelectListItem>();
                    CardTypes.Add(new SelectListItem { Text = "Credit Card", Value = "CC" });
                    CardTypes.Add(new SelectListItem { Text = "Debit Card", Value = "DB" });
                    ViewBag.CardTypes = CardTypes;

                    objDistributorModel.objProduct.PayDetails.CardType = "CC";

                    //List<SelectListItem> objListCustomerTypes = new List<SelectListItem>();
                    //objListCustomerTypes.Add(new SelectListItem { Text = "New", Value = "New" });
                    //objListCustomerTypes.Add(new SelectListItem { Text = "Existing", Value = "Existing" });
                    //ViewBag.CustomerType = objListCustomerTypes;

                    //ViewBag.ConfigDetails = objTransacManager.GetConfigDetails();
                    //objDistributorModel.objConfigDetails = objTransacManager.GetConfigDetails();

                    objDistributorModel.objCustomer.CustomerType = "New";
                    //objDistributorModel.objCustomer.IsRegisteredCustomer = true;
                    //objDistributorModel.objCustomer.ReferenceIdNo = InventorySession.LoginUser.PartyCode;
                    //objDistributorModel.objCustomer.ReferenceName = InventorySession.LoginUser.PartyName;

                    return View(objDistributorModel);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            else return View("../Login/Login");
        }

        [SessionExpire]
        public ActionResult CustomerBill()
        {
            if (Session["LoginUser"] != null)
                if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "CustomerBill"))
                {
                    DistributorBillModel objDistributorModel = new DistributorBillModel();
                    objDistributorModel.objCustomer = new CustomerDetail();
                    objDistributorModel.objProduct = new ProductModel();
                    List<SelectListItem> objBankList = new List<SelectListItem>();
                    var result = objTransacManager.GetBankList();
                    objDistributorModel.objProduct.PayDetails = new PayDetails();
                    foreach (var obj in result)
                    {
                        if (obj.BankCode == 0)
                        {
                            objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString(), Selected = true });
                            objDistributorModel.objProduct.PayDetails.BDBankName = obj.BankCode.ToString();
                        }
                        else
                        {
                            objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString() });
                        }
                    }
                    ViewBag.BankNames = objBankList;
                    List<SelectListItem> CardTypes = new List<SelectListItem>();
                    CardTypes.Add(new SelectListItem { Text = "Credit Card", Value = "CC" });
                    CardTypes.Add(new SelectListItem { Text = "Debit Card", Value = "DB" });
                    ViewBag.CardTypes = CardTypes;

                    objDistributorModel.objProduct.PayDetails.CardType = "CC";



                    objDistributorModel.objCustomer.CustomerType = "New";

                    var objStateList = objRegistrationManager.GetStateList();
                    List<SelectListItem> StateList = new List<SelectListItem>();
                    foreach (var obj in objStateList)
                    {
                        if (obj.StateCode != 0)
                        {
                            StateList.Add(new SelectListItem
                            {
                                Text = obj.StateName,
                                Value = obj.StateCode.ToString()
                            });
                        }
                    }
                    ViewBag.StateList = StateList;
                    // objModel.StateCode = objDistributorModel.objCustomer.StateList.Where(r => r.IsCompanyState == true).Select(m => m.StateCode).FirstOrDefault();

                    return View(objDistributorModel);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            else return View("../Login/Login");
        }

        [HttpPost]
        public async Task<ActionResult> SaveDistributorBill(DistributorBillModel objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            if (objModel != null)
            {
                objModel.objListProduct = new List<ProductModel>();
                if (!string.IsNullOrEmpty(objModel.objProductListStr))
                {
                    var objects = JArray.Parse(objModel.objProductListStr); // parse as array  
                    foreach (JObject root in objects)
                    {
                        ProductModel objTemp = new ProductModel();
                        foreach (KeyValuePair<String, JToken> app in root)
                        {
                            // var appName = app.Key;
                            //    var ProductGrid = [{"AvailStock":"", "SNo": "", "Code": "", "ProductName": "", "MRP": "", "DP": "", "Rate": "","BatchNo":"", "Barcode": "", "RP": "", "BV": "", "CV": "", "PV": "", "Qty": "", "RPValue": "", "BVValue": "", "CVValue": "", "PVValue": "", "CommsnPer": "", "CommsnAmt": "", "DiscPer": "", "DiscAmt": "", "Amount": "", "TaxType": "", "TaxPer": "", "TaxAmt": "", "TotalAmount": ""}];
                            if (app.Key == "Code")
                            {
                                objTemp.ProdCode = (int)app.Value;
                            }
                            else if (app.Key == "ProductName")
                            {
                                objTemp.ProductName = (string)app.Value;
                            }
                            else if (app.Key == "Rate")
                            {
                                objTemp.Rate = (decimal)app.Value;
                            }
                            else if (app.Key == "Barcode")
                            {
                                objTemp.Barcode = app.Value.ToString();
                            }
                            else if (app.Key == "BatchNo")
                            {
                                objTemp.BatchNo = app.Value.ToString();
                            }
                            else if (app.Key == "MRP")
                            {
                                objTemp.MRP = (decimal?)app.Value;
                            }
                            else if (app.Key == "Qty")
                            {
                                objTemp.Quantity = (decimal)app.Value;
                            }
                            else if (app.Key == "FreeQty")
                            {
                                objTemp.FreeQty = (decimal)app.Value;
                            }
                            else if (app.Key == "BuyFreeQty")
                            {
                                objTemp.TFreeQty = (int)app.Value;
                            }
                            else if (app.Key == "PV")
                            {
                                objTemp.PV = (decimal)app.Value;
                            }
                            else if (app.Key == "CV")
                            {
                                objTemp.CV = (decimal)app.Value;
                            }
                            else if (app.Key == "BV")
                            {
                                objTemp.BV = (decimal)app.Value;
                            }
                            else if (app.Key == "RP")
                            {
                                objTemp.RP = (decimal)app.Value;
                            }
                            else if (app.Key == "DP")
                            {
                                objTemp.DP = (decimal)app.Value;
                            }
                            else if (app.Key == "CVValue")
                            {
                                objTemp.CVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "PVValue")
                            {
                                objTemp.PVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "BVValue")
                            {
                                objTemp.BVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "DiscPer")
                            {
                                objTemp.DiscPer = (decimal)app.Value;
                            }
                            else if (app.Key == "DiscAmt")
                            {
                                objTemp.DiscAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxAmt")
                            {
                                objTemp.TaxAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxPer")
                            {
                                objTemp.TaxPer = (decimal)app.Value;
                            }
                            else if (app.Key == "Amount")
                            {
                                objTemp.Amount = (decimal)app.Value;
                            }
                            else if (app.Key == "TotalAmount")
                            {
                                objTemp.TotalAmount = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxType")
                            {
                                objTemp.TaxType = (string)app.Value;
                            }
                            else if (app.Key == "PVValue")
                            {
                                objTemp.DiscAmt = (decimal)app.Value;

                            }
                            else if (app.Key == "AvailStock")
                            {
                                objTemp.StockAvailable = (decimal)app.Value;
                            }
                            else if (app.Key == "CommsnPer")
                            {
                                objTemp.CommissionPer = (decimal)app.Value;
                            }
                            else if (app.Key == "CommsnAmt")
                            {
                                objTemp.CommissionAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "RPValue")
                            {
                                objTemp.RPValue = (decimal)app.Value;
                            }
                            else if (app.Key == "ProductTye")
                            {
                                objTemp.ProductTye = (string)app.Value;
                            }
                            else if (app.Key == "TotalWeight")
                            {
                                objTemp.TotalWeight = (decimal)app.Value;
                            }
                        }
                        objModel.objListProduct.Add(objTemp);
                    }
                    objModel.objCustomer.UserDetails = Session["LoginUser"] as User;
                    // Retrive the Name of HOST
                    string hostName = Dns.GetHostName();
                    // Get the IP  
                    decimal WBalance = 0;
                    if (Session["LoginUserType"] as string == "shoppe")
                    {
                        objModel.UserType = "shoppe";
                        WBalance = objTransacManager.GetPartyWalletBalance(objModel.objCustomer.UserDetails.BranchCode, "R");
                        //WBalance = objTransacManager.GetPartyWalletBalance(objModel.objCustomer.PartyCode, objModel.PartyInvoice);
                    }
                    else
                    {
                        objModel.UserType = "mobileshoppe";
                        WBalance = objTransacManager.GetPartyWalletBalance(objModel.objCustomer.UserDetails.UserName, "M");
                    }

                    if (objModel.BillType != "party" && WBalance < Math.Round(objModel.objProduct.TotalNetPayable) && objModel.objCustomer.UserDetails.BranchCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                    {
                        objResponse.ResponseStatus = "FAILED";
                        objResponse.ResponseMessage = "Insufficient Balance in your wallet";
                    }
                    else
                    {
                        string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                        string currentDate = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        objModel.objProduct.UID = myIP + currentDate;
                        objResponse = await objTransacManager.SaveDistributorBill(objModel);
                        if (Session["LoginUserType"] as string == "shoppe")
                            WBalance = objTransacManager.GetPartyWalletBalance(objModel.objCustomer.UserDetails.BranchCode, "R");
                        else
                            WBalance = objTransacManager.GetPartyWalletBalance(objModel.objCustomer.UserDetails.UserName, "M");
                        (Session["LoginUser"] as User).WBalance = WBalance.ToString();
                        if (objResponse.ResponseStatus == "OK")
                        {
                            InventorySession.StoredDistributorValues = objResponse.ResponseDetailsToPrint;
                        }
                    }
                }
            }
            else
            {
                objResponse.ResponseMessage = "Something went wrong!";
                objResponse.ResponseStatus = "FAILED";
            }

            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult AddLessStockJV(string JvType)
        {
            if (Session["LoginUser"] != null)
                if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "AddLessStockJV?JvType=" + JvType))
                {
                    StockJv objModel = new StockJv();
                    objModel.objListGroup = new List<GroupModel>();
                    objModel.objListGroup = objTransacManager.GetGroupList();
                    objModel.GroupId = objModel.objListGroup[0].GroupId;
                    //objModel.objPartyList = new List<PartyModel>();
                    //string LoginPartyCode = InventorySession.LoginUser.PartyCode;
                    //decimal StateCode = InventorySession.LoginUser.StateCode;
                    //objModel.objPartyList = objTransacManager.GetAllParty(LoginPartyCode, StateCode);
                    if (!string.IsNullOrEmpty(JvType))
                    {
                        if (JvType == "Add")
                        {
                            objModel.isAdd = true;
                        }
                        else
                        {
                            objModel.isAdd = false;
                        }
                    }
                    return View(objModel);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            else return View("../Login/Login");
        }

        [HttpPost]
        public ActionResult SaveStockJv(StockJv objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            if (objModel != null)
            {
                objModel.objListProduct = new List<ProductModel>();
                if (!string.IsNullOrEmpty(objModel.objProductListStr))
                {
                    var objects = JArray.Parse(objModel.objProductListStr); // parse as array  
                    foreach (JObject root in objects)
                    {
                        ProductModel objTemp = new ProductModel();
                        foreach (KeyValuePair<String, JToken> app in root)
                        {
                            // var appName = app.Key;
                            //    var ProductGrid = [{"AvailStock":"", "SNo": "", "Code": "", "ProductName": "", "MRP": "", "DP": "", "Rate": "","BatchNo":"", "Barcode": "", "RP": "", "BV": "", "CV": "", "PV": "", "Qty": "", "RPValue": "", "BVValue": "", "CVValue": "", "PVValue": "", "CommsnPer": "", "CommsnAmt": "", "DiscPer": "", "DiscAmt": "", "Amount": "", "TaxType": "", "TaxPer": "", "TaxAmt": "", "TotalAmount": ""}];
                            if (app.Key == "Code")
                            {
                                objTemp.ProdCode = (int)app.Value;
                            }
                            else if (app.Key == "ProductName")
                            {
                                objTemp.ProductName = (string)app.Value;
                            }

                            else if (app.Key == "Barcode")
                            {
                                objTemp.Barcode = app.Value.ToString();
                            }
                            else if (app.Key == "BatchNo")
                            {
                                objTemp.BatchNo = app.Value.ToString();
                            }

                            else if (app.Key == "Qty")
                            {
                                objTemp.Quantity = (decimal)app.Value;
                            }

                        }
                        objModel.objListProduct.Add(objTemp);
                    }
                    objModel.LoginUser = Session["LoginUser"] as User;


                    objResponse = objTransacManager.SaveStockJv(objModel);

                }

            }
            else
            {
                objResponse.ResponseMessage = "Something went wrong!";
                objResponse.ResponseStatus = "FAILED";
            }

            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCustInfo(string IdNo)
        {
            CustomerDetail model = new CustomerDetail();
            model = objTransacManager.GetCustInfo(IdNo);
            return Json(model, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult GetPasswordInfo(string IdNo, string Password)
        {
            CustomerDetail model = new CustomerDetail();
            model = objTransacManager.GetCustInfo(IdNo);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ValidateCustomerbyAPI(string IdNo, string Password)
        {
            MemberAPIRoot model = new MemberAPIRoot();
            string Message = "", Code = "101";
            string Voucherbal = "";
            string Fpv_Balance = "";

            try
            {
                model = objTransacManager.ValidateCustomerbyAPI(IdNo, Password);
                if (model.Success == "true")
                {
                    Message = "Password Varify Successfully";
                    Code = "200";
                    Voucherbal = model.Result.VOUCHERBAL_Balance;
                    Fpv_Balance = model.Result.Fpv_Balance;
                }
                else
                {
                    Message = model.ApiMessage;
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { Message, Code, Voucherbal, Fpv_Balance }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CheckCoupon(string code, string IdNo)
        {
            //Coupons model = new Coupons();
            //model = objTransacManager.CheckCoupon(code, IdNo);
            //return Json(model, JsonRequestBehavior.AllowGet);
            Coupons model = new Coupons();
            string Msg = string.Empty;
            string stscode = string.Empty;
            decimal amount = 0;
            try
            {
                var obj = objTransacManager.CheckCoupon(code, IdNo);

                if (obj != null && obj.Code != null)
                {
                    if (obj.Isuse == true)
                    {
                        stscode = "100";
                        Msg = "Coupon code already used";
                        amount = 0;
                    }
                    else
                    {
                        stscode = "200";
                        Msg = "Coupon valid";
                        amount = (decimal)obj.Amount;
                    }
                }
                else
                {
                    stscode = "100";
                    Msg = "Coupon code not found";
                    amount = 0;
                }
            }
            catch (Exception)
            {
                stscode = "100";
                Msg = "Something went wrong";
                amount = 0;
            }
            return Json(new { Msg, stscode, amount });
            //return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CheckFpVoucher(string code, string IdNo)
        {
            FPVVoucher model = new FPVVoucher();
            model = objTransacManager.CheckFpVoucher(code, IdNo);
            return Json(model, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetProductInfo(string SearchType, string data, bool isCForm, string BillType, bool IsBillOnMrp, string OfferID, string StockforParty, bool allhalf, string Invoice, string IsSpclOffer)
        {
            List<ProductModel> model = new List<ProductModel>();

            if (string.IsNullOrEmpty(StockforParty))
                StockforParty = (Session["LoginUser"] as User).PartyCode;

            model = objTransacManager.GetproductInfo(SearchType, data, isCForm, BillType, (Session["LoginUser"] as User).StateCode, StockforParty, IsBillOnMrp, OfferID, allhalf, Invoice, IsSpclOffer);
            return Json(model, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult GetproductInfoBatchWise(string SearchType, string data, bool isCForm, string BillType, bool IsBillOnMrp, string OfferID, string StockforParty, bool allhalf, string Invoice, string IsSpclOffer)
        {
            List<ProductModel> model = new List<ProductModel>();

            if (string.IsNullOrEmpty(StockforParty))
                StockforParty = (Session["LoginUser"] as User).PartyCode;

            model = objTransacManager.GetproductInfoBatchWise(SearchType, data, isCForm, BillType, (Session["LoginUser"] as User).StateCode, StockforParty, IsBillOnMrp, OfferID, allhalf, Invoice, IsSpclOffer);
            return Json(model, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetAllproductInfoBatchWise(string SearchType, string data, bool isCForm, string BillType, bool IsBillOnMrp, string OfferID, string StockforParty, bool allhalf, string Invoice, string IsSpclOffer)
        {
            List<ProductModel> model = new List<ProductModel>();

            if (string.IsNullOrEmpty(StockforParty))
                StockforParty = (Session["LoginUser"] as User).PartyCode;

            model = objTransacManager.GetAllproductInfoBatchWise(SearchType, data, isCForm, BillType, (Session["LoginUser"] as User).StateCode, StockforParty, IsBillOnMrp, OfferID, allhalf, Invoice, IsSpclOffer);
            return Json(model, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetPendingOrderProductList(string OrderNo, string OrderBy)
        {
            List<ProductModel> model = new List<ProductModel>();
            string StockforParty = "";
            if (string.IsNullOrEmpty(StockforParty))
                StockforParty = (Session["LoginUser"] as User).PartyCode;
            List<ProductModel> objOrderList = objTransacManager.GetPendingOrderProductList(OrderNo, OrderBy, StockforParty);
            return Json(objOrderList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PendingGetProductInfo(string SearchType, string data, bool isCForm, string BillType, bool IsBillOnMrp, string OfferID, string StockforParty, bool allhalf, string Invoice, string IsSpclOffer)
        {
            List<ProductModel> model = new List<ProductModel>();

            if (string.IsNullOrEmpty(StockforParty))
                StockforParty = (Session["LoginUser"] as User).PartyCode;

            model = objTransacManager.GetproductInfo(SearchType, data, isCForm, BillType, (Session["LoginUser"] as User).StateCode, StockforParty, IsBillOnMrp, OfferID, allhalf, Invoice, IsSpclOffer);
            return Json(model, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult GetAddStockGVproductInfo(string SearchType, string data, bool isCForm, string BillType, bool IsBillOnMrp, string OfferID, string StockforParty, bool allhalf, string Invoice, string IsSpclOffer)
        {
            List<ProductModel> model = new List<ProductModel>();

            if (string.IsNullOrEmpty(StockforParty))
                StockforParty = (Session["LoginUser"] as User).PartyCode;

            model = objTransacManager.GetAddStockGVproductInfo(SearchType, data, isCForm, BillType, (Session["LoginUser"] as User).StateCode, StockforParty, IsBillOnMrp, OfferID, allhalf, Invoice, IsSpclOffer);
            return Json(model, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult GetProductBatchInfo(string SearchType, string data, bool isCForm, string BillType, bool IsBillOnMrp, string OfferID, string StockforParty, bool allhalf, string Invoice, string IsSpclOffer)
        {
            List<ProductModel> model = new List<ProductModel>();

            if (string.IsNullOrEmpty(StockforParty))
                StockforParty = (Session["LoginUser"] as User).PartyCode;

            model = objTransacManager.GetproductBatchInfo(SearchType, data, isCForm, BillType, (Session["LoginUser"] as User).StateCode, StockforParty, IsBillOnMrp, OfferID, allhalf, Invoice, IsSpclOffer);
            return Json(model, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetAllProductNames(string InvType)
        {
            List<string> model = objTransacManager.GetAutocompleteProductNames(InvType);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAvailStockProductNamesOnly(string StockforParty)
        {
            if (string.IsNullOrEmpty(StockforParty))
                StockforParty = (Session["LoginUser"] as User).PartyCode;

            List<string> model = objTransacManager.GetAvailStockProductNamesOnly(StockforParty);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllBarcode()
        {
            List<string> model = objTransacManager.GetAllBarcode();
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetProductNamesOnly()
        {
            string StockforParty = (Session["LoginUser"] as User).PartyCode;
            List<string> model = objTransacManager.GetAutocompProductsOnly(StockforParty);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        

        public ActionResult GetOfferProductNamesOnly(decimal OfferID, int type)
        {
            List<OfferProducts> model = objTransacManager.GetOfferProductNamesOnly(OfferID, type);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetOfferBuyProducts(decimal OfferID)
        {
            List<OfferProducts> model = objTransacManager.GetOfferBuyProducts(OfferID);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

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

            objparty = objTransacManager.GetAllParty(LoginPartyCode, LoginStateCode);
            return Json(objparty, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllPartywithBal()
        {
            List<PartyModel> objparty = new List<PartyModel>();
            string LoginPartyCode = "";
            decimal LoginStateCode = 0;
            if (Session["LoginUser"] != null)
            {
                LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
                LoginStateCode = (Session["LoginUser"] as User).StateCode;
            }
            objparty = objTransacManager.GetAllPartyNew(LoginPartyCode, LoginStateCode, true);
            return Json(objparty, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllSupplier()
        {
            List<PartyModel> objparty = new List<PartyModel>();
            string LoginPartyCode = "";
            decimal LoginStateCode = 0;
            if (Session["LoginUser"] != null)
            {
                LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
                LoginStateCode = (Session["LoginUser"] as User).StateCode;
            }
            objparty = objTransacManager.GetAllSupplierList(LoginPartyCode, LoginStateCode);
            return Json(objparty, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult InvoicePrint(string Pm)
        {

            DistributorBillModel model = new DistributorBillModel();
            if (Session["LoginUser"] != null)
            {
                var base64DecodedBytes = System.Convert.FromBase64String(Pm);
                string BillNoValue = System.Text.Encoding.UTF8.GetString(base64DecodedBytes);
                string CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
                model = objTransacManager.getInvoice(BillNoValue, CurrentPartyCode, "F");
                if (model == null)
                {
                    model = new DistributorBillModel();
                }
            }

            string Viewname = string.Empty;
            if (model.BillType == "S")
            {
                Viewname = "StockInvoicePrint";
            }
            else if (model.BillType == "J" || model.BillType == "X")
            {
                Viewname = "JackpotInvoicePrint";
            }

            else
            {
                Viewname = "InvoicePrint";
            
            }
           

            return View(Viewname, model);
        }

        [SessionExpire]
        public ActionResult InvoicePrintNew(string Pm)
        {

            DistributorBillModel model = new DistributorBillModel();
            if (Session["LoginUser"] != null)
            {
                var base64DecodedBytes = System.Convert.FromBase64String(Pm);
                string BillNoValue = System.Text.Encoding.UTF8.GetString(base64DecodedBytes);
                string CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
                model = objTransacManager.getInvoiceNew(BillNoValue, CurrentPartyCode, "F");
                if (model == null)
                {
                    model = new DistributorBillModel();
                }
            }

            string Viewname = string.Empty;
            if (model.BillType == "S")
            {
                Viewname = "StockInvoicePrint";
            }
            else if (model.BillType == "J" || model.BillType == "X")
            {
                Viewname = "JackpotInvoicePrint";
            }

            else
            {
          
                Viewname = "InvoicePrintNew";
            }


            return View(Viewname, model);
        }

        [SessionExpire]
        public ActionResult OrderPrint(string Pm)
        {
            DistributorBillModel model = new DistributorBillModel();
            PartyOrderModel objOrderList = new PartyOrderModel();
            var base64DecodedBytes = System.Convert.FromBase64String(Pm);
            string Orderno = System.Text.Encoding.UTF8.GetString(base64DecodedBytes);
            if (Session["LoginUser"] != null)
            {
                objOrderList = objTransacManager.GetOrderPrintdata(Orderno);
            }
            return View(objOrderList);
        }

        [HttpPost]
        public async Task<ActionResult> SendOTP(string MobileNo, string TotalBillAmount, string idNo)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = await Task.Run(() => (objTransacManager.SendOTP(MobileNo, TotalBillAmount, idNo)));
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult PurchaseInvoice()
        {

            if (Session["LoginUser"] != null)
                if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "PurchaseInvoice"))
                {
                    return View();

                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            else return View("../Login/Login");
        }

        [HttpPost]
        public ActionResult SavePurchaseInvoice(DistributorBillModel objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            if (objModel != null)
            {
                objModel.objListProduct = new List<ProductModel>();
                try
                {
                    if (!string.IsNullOrEmpty(objModel.objProductListStr))
                    {
                        var objects = JArray.Parse(objModel.objProductListStr); // parse as array  
                        foreach (JObject root in objects)
                        {
                            ProductModel objTemp = new ProductModel();
                            foreach (KeyValuePair<String, JToken> app in root)
                            {
                                // var appName = app.Key;
                                //    var ProductGrid = [{"AvailStock":"", "SNo": "", "Code": "", "ProductName": "", "MRP": "", "DP": "", "Rate": "","BatchNo":"", "Barcode": "", "RP": "", "BV": "", "CV": "", "PV": "", "Qty": "", "RPValue": "", "BVValue": "", "CVValue": "", "PVValue": "", "CommsnPer": "", "CommsnAmt": "", "DiscPer": "", "DiscAmt": "", "Amount": "", "TaxType": "", "TaxPer": "", "TaxAmt": "", "TotalAmount": ""}];
                                if (app.Key == "Code")
                                {
                                    objTemp.ProdCode = (int)app.Value;
                                }
                                else if (app.Key == "ProductName")
                                {
                                    objTemp.ProductName = (string)app.Value;
                                }
                                else if (app.Key == "Rate")
                                {
                                    objTemp.Rate = (decimal)app.Value;
                                }
                                else if (app.Key == "Barcode")
                                {
                                    objTemp.Barcode = app.Value.ToString();
                                }
                                else if (app.Key == "BatchNo")
                                {
                                    objTemp.BatchNo = app.Value.ToString();
                                }
                                else if (app.Key == "MRP")
                                {
                                    objTemp.MRP = (decimal?)app.Value;
                                }
                                else if (app.Key == "Qty")
                                {
                                    objTemp.Quantity = (decimal)app.Value;
                                }
                                else if (app.Key == "PV")
                                {
                                    objTemp.PV = (decimal)app.Value;
                                }
                                else if (app.Key == "CV")
                                {
                                    objTemp.CV = (decimal)app.Value;
                                }
                                else if (app.Key == "BV")
                                {
                                    objTemp.BV = (decimal)app.Value;
                                }
                                else if (app.Key == "RP")
                                {
                                    objTemp.RP = (decimal)app.Value;
                                }
                                else if (app.Key == "DP")
                                {
                                    objTemp.DP = (decimal)app.Value;
                                }
                                else if (app.Key == "CVValue")
                                {
                                    objTemp.CVValue = (decimal)app.Value;
                                }
                                else if (app.Key == "PVValue")
                                {
                                    objTemp.PVValue = (decimal)app.Value;
                                }
                                else if (app.Key == "BVValue")
                                {
                                    objTemp.BVValue = (decimal)app.Value;
                                }
                                else if (app.Key == "DiscPer")
                                {
                                    objTemp.DiscPer = (decimal)app.Value;
                                }
                                else if (app.Key == "DiscAmt")
                                {
                                    objTemp.DiscAmt = (decimal)app.Value;
                                }
                                else if (app.Key == "TaxAmt")
                                {
                                    objTemp.TaxAmt = (decimal)app.Value;
                                }
                                else if (app.Key == "TaxPer")
                                {
                                    objTemp.TaxPer = (decimal)app.Value;
                                }
                                else if (app.Key == "Amount")
                                {
                                    objTemp.Amount = (decimal)app.Value;
                                }
                                else if (app.Key == "TotalAmount")
                                {
                                    objTemp.TotalAmount = (decimal)app.Value;
                                }
                                else if (app.Key == "TaxType")
                                {
                                    objTemp.TaxType = (string)app.Value;
                                }
                                else if (app.Key == "PVValue")
                                {
                                    objTemp.DiscAmt = (decimal)app.Value;

                                }
                                else if (app.Key == "AvailStock")
                                {
                                    objTemp.StockAvailable = (decimal)app.Value;
                                }
                                else if (app.Key == "CommsnPer")
                                {
                                    objTemp.CommissionPer = (decimal)app.Value;
                                }
                                else if (app.Key == "CommsnAmt")
                                {
                                    objTemp.CommissionAmt = (decimal)app.Value;
                                }
                                else if (app.Key == "RPValue")
                                {
                                    objTemp.RPValue = (decimal)app.Value;
                                }

                            }
                            objModel.objListProduct.Add(objTemp);
                        }
                        objModel.objCustomer.UserDetails = Session["LoginUser"] as User;
                        // Retrive the Name of HOST
                        string hostName = Dns.GetHostName();
                        // Get the IP  
                        string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                        string currentDate = DateTime.Now.ToString("yyyyMMddHHmmssfff"); ;
                        objModel.objProduct.UID = myIP + currentDate;
                        objResponse = objTransacManager.SavePurchaseInvoice(objModel);
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult OrderCreation()
        {
            if (Session["LoginUser"] != null)
                if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "OrderCreation"))
                {
                    PartyOrderModel objPartyModel = new PartyOrderModel();

                    objPartyModel.objProduct = new ProductModel();
                    List<SelectListItem> objBankList = new List<SelectListItem>();
                    var result = objTransacManager.GetBankList();
                    objPartyModel.objProduct.PayDetails = new PayDetails();
                    foreach (var obj in result)
                    {
                        if (obj.BankCode == 0)
                        {
                            objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString(), Selected = true });
                            objPartyModel.objProduct.PayDetails.BDBankName = obj.BankCode.ToString();
                        }
                        else
                        {
                            objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString() });
                        }
                    }
                    ViewBag.BankNames = objBankList;
                    List<SelectListItem> CardTypes = new List<SelectListItem>();
                    CardTypes.Add(new SelectListItem { Text = "Credit Card", Value = "Credit Card" });
                    CardTypes.Add(new SelectListItem { Text = "Debit Card", Value = "Debit Card" });
                    ViewBag.CardTypes = CardTypes;

                    objPartyModel.objProduct.PayDetails.CardType = "Credit Card";
                    objPartyModel.objProduct.PayDetails.PayMode = "BD";
                    string LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
                    objPartyModel.OrderNo = objTransacManager.GetOrderNo(LoginPartyCode);
                    objPartyModel.PartyWalletBalance = objTransacManager.GetPartyWalletBalance(LoginPartyCode, "R");
                    objPartyModel.OrderBy = LoginPartyCode;
                    objPartyModel.OrderTo = (Session["LoginUser"] as User).ParentPartyCode;
                    return View(objPartyModel);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            else return View("../Login/Login");
        }

        [HttpPost]
        public ActionResult SavePartyOrderDetails(PartyOrderModel objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            if (objModel != null)
            {
                objModel.objListProduct = new List<ProductModel>();
                if (!string.IsNullOrEmpty(objModel.objProductListStr))
                {
                    var objects = JArray.Parse(objModel.objProductListStr); // parse as array  
                    foreach (JObject root in objects)
                    {
                        ProductModel objTemp = new ProductModel();
                        foreach (KeyValuePair<String, JToken> app in root)
                        {
                            // var appName = app.Key;
                            //    var ProductGrid = [{"AvailStock":"", "SNo": "", "Code": "", "ProductName": "", "MRP": "", "DP": "", "Rate": "","BatchNo":"", "Barcode": "", "RP": "", "BV": "", "CV": "", "PV": "", "Qty": "", "RPValue": "", "BVValue": "", "CVValue": "", "PVValue": "", "CommsnPer": "", "CommsnAmt": "", "DiscPer": "", "DiscAmt": "", "Amount": "", "TaxType": "", "TaxPer": "", "TaxAmt": "", "TotalAmount": ""}];
                            if (app.Key == "Code")
                            {
                                objTemp.ProdCode = (int)app.Value;
                            }
                            else if (app.Key == "ProductName")
                            {
                                objTemp.ProductName = (string)app.Value;
                            }
                            else if (app.Key == "Rate")
                            {
                                objTemp.Rate = (decimal)app.Value;
                            }
                            else if (app.Key == "Barcode")
                            {
                                objTemp.Barcode = app.Value.ToString();
                            }
                            else if (app.Key == "BatchNo")
                            {
                                objTemp.BatchNo = app.Value.ToString();
                            }
                            else if (app.Key == "MRP")
                            {
                                objTemp.MRP = (decimal?)app.Value;
                            }
                            else if (app.Key == "Qty")
                            {
                                objTemp.Quantity = (decimal)app.Value;
                            }
                            else if (app.Key == "PV")
                            {
                                objTemp.PV = (decimal)app.Value;
                            }
                            else if (app.Key == "CV")
                            {
                                objTemp.CV = (decimal)app.Value;
                            }
                            else if (app.Key == "BV")
                            {
                                objTemp.BV = (decimal)app.Value;
                            }
                            else if (app.Key == "RP")
                            {
                                objTemp.RP = (decimal)app.Value;
                            }
                            else if (app.Key == "DP")
                            {
                                objTemp.DP = (decimal)app.Value;
                            }
                            else if (app.Key == "CVValue")
                            {
                                objTemp.CVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "PVValue")
                            {
                                objTemp.PVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "BVValue")
                            {
                                objTemp.BVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "DiscPer")
                            {
                                objTemp.DiscPer = (decimal)app.Value;
                            }
                            else if (app.Key == "DiscAmt")
                            {
                                objTemp.DiscAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxAmt")
                            {
                                objTemp.TaxAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxPer")
                            {
                                objTemp.TaxPer = (decimal)app.Value;
                            }
                            else if (app.Key == "Amount")
                            {
                                objTemp.Amount = (decimal)app.Value;
                            }
                            else if (app.Key == "TotalAmount")
                            {
                                objTemp.TotalAmount = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxType")
                            {
                                objTemp.TaxType = (string)app.Value;
                            }
                            else if (app.Key == "PVValue")
                            {
                                objTemp.DiscAmt = (decimal)app.Value;

                            }
                            else if (app.Key == "AvailStock")
                            {
                                objTemp.StockAvailable = (decimal)app.Value;
                            }
                            else if (app.Key == "CommsnPer")
                            {
                                objTemp.CommissionPer = (decimal)app.Value;
                            }
                            else if (app.Key == "CommsnAmt")
                            {
                                objTemp.CommissionAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "RPValue")
                            {
                                objTemp.RPValue = (decimal)app.Value;
                            }

                        }
                        objModel.objListProduct.Add(objTemp);
                    }
                    objModel.LoginUser = Session["LoginUser"] as User;
                    // Retrive the Name of HOST
                    string hostName = Dns.GetHostName();
                    // Get the IP  
                    string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                    string currentDate = DateTime.Now.ToString("yyyyMMddHHmm");
                    objModel.objProduct.UID = "";
                    objResponse = objTransacManager.SavePartyOrderDetails(objModel);

                }
            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult PendingOrder()
        {
            if (Session["LoginUser"] != null)
                if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "PendingOrder"))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            else return View("../Login/Login");
        }

        [SessionExpire]
        public ActionResult DispatchOrder()
        {
            if (Session["LoginUser"] != null)
                if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "DispatchOrder"))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            else return View("../Login/Login");
        }
        public ActionResult GetOrderList(string FromDate, string ToDate, string PartyCode, string ViewType, string IdNo, string OrderNo, string DispMode)
        {
            string CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
            string IsAdmin = (Session["LoginUser"] as User).IsAdmin;
            //if (IsAdmin != "Y")
            if (OrderNo != "" && OrderNo != "0")
            {
                FromDate = "All"; ToDate = "All"; PartyCode = "0"; IdNo = "0";
            }
            else
            {
                if (CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                    PartyCode = CurrentPartyCode;


            }
            List<DisptachOrderModel> objDispatchOrderList = new List<DisptachOrderModel>();
            objDispatchOrderList = objTransacManager.GetDispatchOrderList(FromDate, ToDate, PartyCode, ViewType, IdNo, OrderNo, DispMode);
            return Json(objDispatchOrderList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RejectOrder(string OrderNo, string RejectReason)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objTransacManager.RejectOrder(OrderNo, RejectReason, (Session["LoginUser"] as User).UserId);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrderProduct(string OrderNo)
        {
            List<ProductModel> objModel = new List<ProductModel>();
            objModel = objTransacManager.GetOrderProduct(OrderNo, (Session["LoginUser"] as User).PartyCode);
            return Json(objModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveDispatchOrderDetails(DisptachOrderModel objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            List<DisptachOrderModel> objDispatchList = new List<DisptachOrderModel>();
            try
            {
                if (objModel != null)
                {

                    if (!string.IsNullOrEmpty(objModel.OrderList))
                    {
                        var objects = JArray.Parse(objModel.OrderList); // parse as array  
                        foreach (JObject root in objects)
                        {
                            DisptachOrderModel objTemp = new DisptachOrderModel();
                            foreach (KeyValuePair<String, JToken> app in root)
                            {
                                // var appName = app.Key;
                                //    var ProductGrid = [{"AvailStock":"", "SNo": "", "Code": "", "ProductName": "", "MRP": "", "DP": "", "Rate": "","BatchNo":"", "Barcode": "", "RP": "", "BV": "", "CV": "", "PV": "", "Qty": "", "RPValue": "", "BVValue": "", "CVValue": "", "PVValue": "", "CommsnPer": "", "CommsnAmt": "", "DiscPer": "", "DiscAmt": "", "Amount": "", "TaxType": "", "TaxPer": "", "TaxAmt": "", "TotalAmount": ""}];
                                if (app.Key == "OrderNo")
                                {
                                    objTemp.OrderNo = (int)app.Value;
                                }
                                else if (app.Key == "SoldBy")
                                {
                                    objTemp.SoldBy = (Session["LoginUser"] as User).PartyCode;
                                }
                                else if (app.Key == "IsDispatched")
                                {
                                    objTemp.IsDispatched = (bool)app.Value;
                                }


                            }
                            objDispatchList.Add(objTemp);
                        }

                        objResponse = objTransacManager.SaveDispatchOrderdetails(objDispatchList);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RejectFranchiseOrder(string OrderNo, string RejectReason)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objTransacManager.RejectFranchiseOrder(OrderNo, RejectReason, (Session["LoginUser"] as User).UserId);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOrderDetails(string OrderBy, string OrderTo, string Status)
        {
            string LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
            List<PartyOrderModel> objOrderList = objTransacManager.GetOrderList(OrderBy, OrderTo, Status);
            return Json(objOrderList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOrderProductDetails(string OrderNo, string OrderBy)
        {
            List<ProductModel> objOrderList = objTransacManager.GetOrderProductList(OrderNo, OrderBy);
            return Json(objOrderList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveDispatchOrder(PartyOrderModel objPartyOrderModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            if (objPartyOrderModel != null)
            {
                objPartyOrderModel.objListProduct = new List<ProductModel>();
                if (!string.IsNullOrEmpty(objPartyOrderModel.objProductListStr))
                {
                    var objects = JArray.Parse(objPartyOrderModel.objProductListStr); // parse as array  
                    foreach (JObject root in objects)
                    {
                        ProductModel objTemp = new ProductModel();
                        foreach (KeyValuePair<String, JToken> app in root)
                        {
                            // var appName = app.Key;
                            //    var ProductGrid = [{"AvailStock":"", "SNo": "", "Code": "", "ProductName": "", "MRP": "", "DP": "", "Rate": "","BatchNo":"", "Barcode": "", "RP": "", "BV": "", "CV": "", "PV": "", "Qty": "", "RPValue": "", "BVValue": "", "CVValue": "", "PVValue": "", "CommsnPer": "", "CommsnAmt": "", "DiscPer": "", "DiscAmt": "", "Amount": "", "TaxType": "", "TaxPer": "", "TaxAmt": "", "TotalAmount": ""}];
                            if (app.Key == "Code")
                            {
                                objTemp.ProdCode = (int)app.Value;
                            }
                            else if (app.Key == "ProductName")
                            {
                                objTemp.ProductName = (string)app.Value;
                            }
                            else if (app.Key == "Rate")
                            {
                                objTemp.Rate = (decimal)app.Value;
                            }
                            else if (app.Key == "Barcode")
                            {
                                objTemp.Barcode = app.Value.ToString();
                            }
                            else if (app.Key == "BatchNo")
                            {
                                objTemp.BatchNo = app.Value.ToString();
                            }
                            else if (app.Key == "MRP")
                            {
                                objTemp.MRP = (decimal?)app.Value;
                            }
                            else if (app.Key == "Qty")
                            {
                                objTemp.Quantity = (decimal)app.Value;
                            }
                            else if (app.Key == "PV")
                            {
                                objTemp.PV = (decimal)app.Value;
                            }
                            else if (app.Key == "CV")
                            {
                                objTemp.CV = (decimal)app.Value;
                            }
                            else if (app.Key == "BV")
                            {
                                objTemp.BV = (decimal)app.Value;
                            }
                            else if (app.Key == "RP")
                            {
                                objTemp.RP = (decimal)app.Value;
                            }
                            else if (app.Key == "DP")
                            {
                                objTemp.DP = (decimal)app.Value;
                            }
                            else if (app.Key == "CVValue")
                            {
                                objTemp.CVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "PVValue")
                            {
                                objTemp.PVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "BVValue")
                            {
                                objTemp.BVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "DiscPer")
                            {
                                objTemp.DiscPer = (decimal)app.Value;
                            }
                            else if (app.Key == "DiscAmt")
                            {
                                objTemp.DiscAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxAmt")
                            {
                                objTemp.TaxAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxPer")
                            {
                                objTemp.TaxPer = (decimal)app.Value;
                            }
                            else if (app.Key == "Amount")
                            {
                                objTemp.Amount = (decimal)app.Value;
                            }
                            else if (app.Key == "TotalAmount")
                            {
                                objTemp.TotalAmount = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxType")
                            {
                                objTemp.TaxType = (string)app.Value;
                            }
                            else if (app.Key == "PVValue")
                            {
                                objTemp.DiscAmt = (decimal)app.Value;

                            }
                            else if (app.Key == "AvailStock")
                            {
                                objTemp.StockAvailable = (decimal)app.Value;
                            }
                            else if (app.Key == "CommsnPer")
                            {
                                objTemp.CommissionPer = (decimal)app.Value;
                            }
                            else if (app.Key == "CommsnAmt")
                            {
                                objTemp.CommissionAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "RPValue")
                            {
                                objTemp.RPValue = (decimal)app.Value;
                            }
                            else if (app.Key == "OfferUID")
                            {
                                objTemp.OfferUID = (decimal)app.Value;
                            }
                            else if (app.Key == "ProductType")
                            {
                                objTemp.ProductType = app.Value.ToString();
                            }
                        }
                        if (objTemp.Quantity > 0)
                        {
                            objPartyOrderModel.objListProduct.Add(objTemp);
                        }
                    }
                    objPartyOrderModel.LoginUser = Session["LoginUser"] as User;
                    // Retrive the Name of HOST
                    string hostName = Dns.GetHostName();
                    // Get the IP  
                    string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                    string currentDate = DateTime.Now.ToString("yyyyMMddHHmmssfff"); ;
                    objPartyOrderModel.objProduct.UID = myIP + currentDate;
                    objResponse = objTransacManager.SaveDispatchOrder(objPartyOrderModel);
                    //if (objResponse.ResponseStatus == "OK")
                    //{
                    //    InventorySession.StoredDistributorValues = objResponse.ResponseDetailsToPrint;
                    //}
                }

            }
            else
            {
                objResponse.ResponseMessage = "Something went wrong!";
                objResponse.ResponseStatus = "FAILED";
            }

            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult DeleteBills()
        {
            if (Session["LoginUser"] != null)
                if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "DeleteBills"))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            else return View("../Login/Login");
        }


        public ActionResult DeleteBill(string BillNo, string FsessId, string Reason)
        {
            ResponseDetail objResponse = new ResponseDetail();
            List<SalesReport> objBillList = new List<SalesReport>();
            try
            {
                decimal UserId = (Session["LoginUser"] as User).UserId;
                objResponse = objTransacManager.DeleteBills(BillNo, FsessId, UserId, Reason);
            }
            catch (Exception ex)
            {

            }

            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult SalesReturn()
        {
            try
            {
                if (Session["LoginUser"] != null)
                {
                    string LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
                    string returnNo = objTransacManager.GetSalesReturnNumber(LoginPartyCode);
                    ViewBag.returnNo = returnNo;
                    if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "SalesReturn"))
                        return View("SalesReturnNew");
                    else
                        return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (Exception ex)
            {

            }
            return View("../Login/Login");
        }

        public ActionResult GetListOfPartyBills(string party, string partyType)
        {
            List<PartyBill> objResponse = new List<PartyBill>();
            try
            {
                objResponse = objTransacManager.GetBillList(partyType, party);
            }
            catch (Exception ex)
            {

            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListOfSupplierBills(string supplier)
        {
            List<PartyBill> objResponse = new List<PartyBill>();
            try
            {
                objResponse = objTransacManager.GetListOfSupplierBills(supplier);
            }
            catch (Exception ex)
            {

            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetListOfBillProducts(string BillNo)
        {
            DistributorBillModel objResponse = new DistributorBillModel();
            try
            {
                objResponse = objTransacManager.getInvoice(BillNo, "", "F");
            }
            catch (Exception ex)
            {

            }
            return Json(objResponse.objListProduct, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetBillDetail(string BillNo)
        {
            PartyBill objResponse = new PartyBill();
            try
            {
                objResponse = objTransacManager.GetBillDetail(BillNo);
            }
            catch (Exception ex)
            {

            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBillDetailNew(string BillNo)
        {
            PartyBill objResponse = new PartyBill();
            try
            {
                objResponse = objTransacManager.GetBillDetailNew(BillNo, (Session["LoginUser"] as User).PartyCode);
            }
            catch (Exception ex)
            {

            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetPurchaseDetail(string BillNo)
        //{
        //    PurchaseBill objResponse = new PurchaseBill();
        //    try
        //    {
        //        objResponse = objTransacManager.GetPurchaseDetail(BillNo);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return Json(objResponse, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public ActionResult SaveReturnOrder(SalesReturnModel objPartyOrderModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            if (objPartyOrderModel != null)
            {
                objPartyOrderModel.ProductList = new List<ProductModel>();
                if (!string.IsNullOrEmpty(objPartyOrderModel.objProductListStr))
                {
                    var objects = JArray.Parse(objPartyOrderModel.objProductListStr); // parse as array  
                    foreach (JObject root in objects)
                    {
                        ProductModel objTemp = new ProductModel();
                        foreach (KeyValuePair<String, JToken> app in root)
                        {
                            if (app.Key == "ProductCode")
                            {
                                objTemp.ProductCodeStr = (string)app.Value;
                            }
                            else if (app.Key == "ProductName")
                            {
                                objTemp.ProductName = (string)app.Value;
                            }
                            else if (app.Key == "Rate")
                            {
                                objTemp.Rate = (decimal)app.Value;
                            }
                            else if (app.Key == "Barcode")
                            {
                                objTemp.Barcode = app.Value.ToString();
                            }
                            else if (app.Key == "BatchNo")
                            {
                                objTemp.BatchNo = app.Value.ToString();
                            }
                            else if (app.Key == "MRP")
                            {
                                objTemp.MRP = (decimal?)app.Value;
                            }
                            else if (app.Key == "Qty")
                            {
                                objTemp.Quantity = (decimal)app.Value;
                            }
                            else if (app.Key == "PV")
                            {
                                objTemp.PV = (decimal)app.Value;
                            }
                            else if (app.Key == "CV")
                            {
                                objTemp.CV = (decimal)app.Value;
                            }
                            else if (app.Key == "BV")
                            {
                                objTemp.BV = (decimal)app.Value;
                            }
                            else if (app.Key == "RP")
                            {
                                objTemp.RP = (decimal)app.Value;
                            }
                            else if (app.Key == "DP")
                            {
                                objTemp.DP = (decimal)app.Value;
                            }
                            else if (app.Key == "CVValue")
                            {
                                objTemp.CVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "PVValue")
                            {
                                objTemp.PVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "BVValue")
                            {
                                objTemp.BVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "DiscPer")
                            {
                                objTemp.DiscPer = (decimal)app.Value;
                            }
                            else if (app.Key == "DiscAmt")
                            {
                                objTemp.DiscAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxAmount")
                            {
                                objTemp.TaxAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxPer")
                            {
                                objTemp.GSTPer = (decimal)app.Value;
                            }
                            else if (app.Key == "Amount")
                            {
                                objTemp.Amount = (decimal)app.Value;
                            }
                            else if (app.Key == "TotalAmount")
                            {
                                objTemp.TotalAmount = (decimal)app.Value;
                            }
                            else if (app.Key == "PVValue")
                            {
                                objTemp.DiscAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "CommissionPer")
                            {
                                objTemp.CommissionPer = 0;
                            }
                            else if (app.Key == "CommissionAmt")
                            {
                                objTemp.CommissionAmt = 0;
                            }
                            else if (app.Key == "RPValue")
                            {
                                objTemp.RPValue = (decimal)app.Value;
                            }
                            else if (app.Key == "ReturnQty")
                            {
                                objTemp.ReturnQty = (decimal)app.Value;
                            }

                        }
                        objPartyOrderModel.ProductList.Add(objTemp);
                    }
                    objPartyOrderModel.LoggedInUserId = (Session["LoginUser"] as User).UserId;
                    objPartyOrderModel.returnto = (Session["LoginUser"] as User).PartyCode;
                    objPartyOrderModel.EntryBy = (Session["LoginUser"] as User).PartyCode;
                    // Retrive the Name of HOST
                    string hostName = Dns.GetHostName();
                    // Get the IP  
                    string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                    string currentDate = DateTime.Now.ToString("yyyyMMddHHmmssfff"); ;
                    objPartyOrderModel.LoggedInUserIP = myIP;
                    objResponse = objTransacManager.SaveOrderReturn(objPartyOrderModel);

                }

            }
            else
            {
                objResponse.ResponseMessage = "Something went wrong!";
                objResponse.ResponseStatus = "FAILED";
            }

            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult SavePurchaseReturnOrder(PurchaseReturnModel objPartyOrderModel)
        //{
        //    ResponseDetail objResponse = new ResponseDetail();
        //    if (objPartyOrderModel != null)
        //    {
        //        objPartyOrderModel.ProductList = new List<ProductModel>();
        //        if (!string.IsNullOrEmpty(objPartyOrderModel.objProductListStr))
        //        {
        //            var objects = JArray.Parse(objPartyOrderModel.objProductListStr); // parse as array  
        //            foreach (JObject root in objects)
        //            {
        //                ProductModel objTemp = new ProductModel();
        //                foreach (KeyValuePair<String, JToken> app in root)
        //                {
        //                    if (app.Key == "ProductCode")
        //                    {
        //                        objTemp.ProductCodeStr = (string)app.Value;
        //                    }
        //                    else if (app.Key == "ProductName")
        //                    {
        //                        objTemp.ProductName = (string)app.Value;
        //                    }
        //                    else if (app.Key == "Rate")
        //                    {
        //                        objTemp.Rate = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "Barcode")
        //                    {
        //                        objTemp.Barcode = app.Value.ToString();
        //                    }
        //                    else if (app.Key == "BatchNo")
        //                    {
        //                        objTemp.BatchNo = app.Value.ToString();
        //                    }
        //                    else if (app.Key == "MRP")
        //                    {
        //                        objTemp.MRP = (decimal?)app.Value;
        //                    }
        //                    else if (app.Key == "Qty")
        //                    {
        //                        objTemp.Quantity = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "PV")
        //                    {
        //                        objTemp.PV = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "CV")
        //                    {
        //                        objTemp.CV = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "BV")
        //                    {
        //                        objTemp.BV = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "RP")
        //                    {
        //                        objTemp.RP = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "DP")
        //                    {
        //                        objTemp.DP = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "CVValue")
        //                    {
        //                        objTemp.CVValue = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "PVValue")
        //                    {
        //                        objTemp.PVValue = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "BVValue")
        //                    {
        //                        objTemp.BVValue = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "DiscPer")
        //                    {
        //                        objTemp.DiscPer = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "DiscAmt")
        //                    {
        //                        objTemp.DiscAmt = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "TaxAmount")
        //                    {
        //                        objTemp.TaxAmt = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "GST")
        //                    {
        //                        objTemp.GSTPer = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "Amount")
        //                    {
        //                        objTemp.Amount = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "TotalAmount")
        //                    {
        //                        objTemp.TotalAmount = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "PVValue")
        //                    {
        //                        objTemp.DiscAmt = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "CommissionPer")
        //                    {
        //                        objTemp.CommissionPer = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "CommissionAmt")
        //                    {
        //                        objTemp.CommissionAmt = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "RPValue")
        //                    {
        //                        objTemp.RPValue = (decimal)app.Value;
        //                    }
        //                    else if (app.Key == "ReturnQty")
        //                    {
        //                        objTemp.ReturnQty = (decimal)app.Value;
        //                    }

        //                }
        //                objPartyOrderModel.ProductList.Add(objTemp);
        //            }
        //            objPartyOrderModel.LoggedInUserId = (Session["LoginUser"] as User).UserId;
        //            objPartyOrderModel.EntryBy = (Session["LoginUser"] as User).PartyCode;
        //            // Retrive the Name of HOST
        //            string hostName = Dns.GetHostName();
        //            // Get the IP  
        //            string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
        //            string currentDate = DateTime.Now.ToString("yyyyMMddHHmmssfff"); ;
        //            objPartyOrderModel.LoggedInUserIP = myIP;
        //            objResponse = objTransacManager.SavePurchaseReturnOrder(objPartyOrderModel);
        //        }
        //    }
        //    else
        //    {
        //        objResponse.ResponseMessage = "Something went wrong!";
        //        objResponse.ResponseStatus = "FAILED";
        //    }

        //    return Json(objResponse, JsonRequestBehavior.AllowGet);
        //}
        [SessionExpire]
        public ActionResult DeletePurchaseInvoice()
        {
            try
            {
                if (Session["LoginUser"] != null)
                {
                    string LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
                    if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "DeletePurchaseInvoice"))
                        return View();
                    else
                        return RedirectToAction("Dashboard", "Home");
                }
                else return View("../Login/Login");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }

        public ActionResult DeletePurchaseInvoices(string InwardNo, decimal FsessId, string Reason)
        {
            ResponseDetail objResponse = new ResponseDetail();
            List<SalesReport> objBillList = new List<SalesReport>();
            try
            {
                decimal UserId = (Session["LoginUser"] as User).UserId;
                objResponse = objTransacManager.DeletePurchaseInvoice(InwardNo, FsessId, UserId, Reason);
            }
            catch (Exception ex)
            {

            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult PartyTargetMaster()
        {
            try
            {
                if (Session["LoginUser"] != null)
                {
                    string LoginPartyCode = (Session["LoginUser"] as User).PartyCode;


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
                    if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "PartyTargetMaster"))
                        return View();
                    else
                        return RedirectToAction("Dashboard", "Home");

                }
            }
            catch (Exception ex)
            {

            }
            return View("../Login/Login");
        }

        [HttpPost]
        public ActionResult SavePartyTargetDetails(PartyTargetMaster objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                if (Session["LoginUser"] != null)
                {
                    int userid = (Session["LoginUser"] as User).UserId;
                    objModel.UserID = userid;
                    objResponse = objTransacManager.SavePartyTargetDetails(objModel);
                }
            }
            catch (Exception ex)
            {

            }

            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult UpdateDeliveryDetail()
        {
            PaymentSummary objPaymentSummary = new PaymentSummary();
            try
            {
                string LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
                if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "UpdateDeliveryDetail"))
                    return View(objPaymentSummary);
                else
                    return RedirectToAction("Dashboard", "Home");

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Dashboard", "Home");
        }
        [SessionExpire]
        public ActionResult OldBills()
        {
            if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "OldBills"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        public ActionResult GetOldBills(string FromDate, string ToDate, string IdNo, string OrderNo)
        {
            string CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
            string IsAdmin = (Session["LoginUser"] as User).IsAdmin;
            string PartyCode = "ALL";
            if (CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                PartyCode = CurrentPartyCode;
            List<OldBills> objDispatchOrderList = new List<OldBills>();
            objDispatchOrderList = objTransacManager.GetOldBills(FromDate, ToDate, IdNo, OrderNo, PartyCode);
            return Json(objDispatchOrderList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBillProducts(string billNo)
        {

            List<ProductModel> objDispatchOrderList = new List<ProductModel>();
            objDispatchOrderList = objTransacManager.GetBillProducts(billNo);
            return Json(objDispatchOrderList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRecordToUpdateDelDetails(string FromDate, string ToDate, string PartyCode, string Fcode, string status)
        {
            List<SalesReport> objResponse = objTransacManager.GetRecordToUpdateDelDetails(FromDate, ToDate, PartyCode, Fcode, status);
            var jsonProduct = Json(objResponse, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;
        }
        public ActionResult UpdateDeliveryDetails(UpdateDeliveryDetails obj)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                if (Session["LoginUser"] != null)
                {
                    obj.LoggedInUser = (Session["LoginUser"] as User).UserId;
                }
                obj.DeliverDetailList = new List<SalesReport>();
                if (!string.IsNullOrEmpty(obj.ListObjHidden))
                {
                    var objects = JArray.Parse(obj.ListObjHidden); // parse as array  
                    foreach (JObject root in objects)
                    {
                        SalesReport objTemp = new SalesReport();
                        foreach (KeyValuePair<String, JToken> app in root)
                        {

                            if (app.Key == "BillNo")
                            {
                                objTemp.BillNo = (string)app.Value;
                            }
                            else if (app.Key == "BillDate")
                            {
                                objTemp.BillDate = (app.Value.ToString() != "" && app.Value != null && app.Value.Type != JTokenType.Null) ? Convert.ToDateTime(DateTime.ParseExact(app.Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null;
                            }
                            else if (app.Key == "SoldBy")
                            {
                                objTemp.SoldBy = (string)app.Value;
                            }
                            else if (app.Key == "PartyName")
                            {
                                objTemp.PartyName = app.Value.ToString();
                            }
                            else if (app.Key == "PartyCode")
                            {
                                objTemp.PartyCode = app.Value.ToString();
                            }
                            else if (app.Key == "Name")
                            {
                                objTemp.Name = (string)app.Value;
                            }
                            else if (app.Key == "CourierName")
                            {
                                objTemp.CourierName = (string)app.Value;
                            }
                            else if (app.Key == "DocWeight")
                            {
                                objTemp.DocWeight = (string)app.Value;
                            }
                            else if (app.Key == "DocketNo")
                            {
                                objTemp.DocketNo = (string)app.Value;
                            }
                            else if (app.Key == "DocketDate")
                            {
                                //string dt = app.Value == null ? "" : app.Value.ToString();

                                //throw new Exception(dt);
                                DateTime tempDate;

                                objTemp.DocketDate =
                                    DateTime.TryParse(app.Value == null ? "" : app.Value.ToString(), out tempDate)
                                    ? tempDate
                                    : (DateTime?)null;
                                //objTemp.DocketDate = (app.Value.ToString() != "" && app.Value != null && app.Value.Type != JTokenType.Null) ? Convert.ToDateTime(DateTime.ParseExact(app.Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null;
                            }
                            else if (app.Key == "DOD")
                            {
                                DateTime tempDate;

                                objTemp.DOD =
                                    DateTime.TryParse(app.Value == null ? "" : app.Value.ToString(), out tempDate)
                                    ? tempDate
                                    : (DateTime?)null;
                                //objTemp.DOD = (app.Value.ToString() != "" && app.Value != null && app.Value.Type != JTokenType.Null) ? Convert.ToDateTime(DateTime.ParseExact(app.Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null;
                            }
                            else if (app.Key == "DelvAddress")
                            {
                                objTemp.DelvAddress = (string)app.Value;
                            }
                            else if (app.Key == "CID")
                            {
                                objTemp.CID = (string)app.Value;
                            }
                            else if (app.Key == "DispDate")
                            {
                                DateTime tempDate;

                                objTemp.DispDate =
                                    DateTime.TryParse(app.Value == null ? "" : app.Value.ToString(), out tempDate)
                                    ? tempDate
                                    : (DateTime?)null;
                                // objTemp.DispDate = (app.Value.ToString() != "" && app.Value != null && app.Value.Type != JTokenType.Null) ? Convert.ToDateTime(DateTime.ParseExact(app.Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null;
                            }

                            else if (app.Key == "NetPayable")
                            {
                                objTemp.NetPayable = (string)app.Value;
                            }
                            else if (app.Key == "MobileNO")
                            {
                                objTemp.MobileNO = (string)app.Value;
                            }
                            else if (app.Key == "OrderNo")
                            {
                                objTemp.OrderNo = (string)app.Value;
                            }
                        }
                        obj.DeliverDetailList.Add(objTemp);
                    }
                }

                objResponse = objTransacManager.UpdateDeliveryDetails(obj);
            }
            catch (Exception ex)
            {

            }

            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Upload()
        {
            UpdateDeliveryDetails objResponse = new UpdateDeliveryDetails();
            objResponse.ErrorMessage = "";
            try
            {
                HttpPostedFileBase upload = null;
                string filename = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        filename = Path.GetFileName(Request.Files[i].FileName);
                        upload = files[i];
                    }
                    if (upload != null && upload.ContentLength > 0)
                    {
                        try
                        {
                            DataTable dt = GetDataTableFromSpreadsheet(upload, filename, false);
                            objResponse.DeliverDetailList = SaveExcelReportData(dt);
                            objResponse.ErrorMessage = "OK";
                        }
                        catch (Exception e)
                        {
                            objResponse.ErrorMessage = "error--" + e.Message + "--Inner Exception--" + e.InnerException;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                objResponse.ErrorMessage = "error--" + e.Message + "--Inner Exception--" + e.InnerException;
            }

            var jsonProduct = Json(objResponse, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;
        }

        private static ISheet GetFileStream(HttpPostedFileBase MyExcelStream, string filename1, bool ReadOnly)
        {

            HttpPostedFileBase files = MyExcelStream; //Read the Posted Excel File  
            ISheet sheet; //Create the ISheet object to read the sheet cell values  
            string filename = Path.GetFileName(files.FileName); //get the uploaded file name  
            var fileExt = Path.GetExtension(filename); //get the extension of uploaded excel file  
            if (fileExt == ".xls")
            {
                HSSFWorkbook hssfwb = new HSSFWorkbook(files.InputStream); //HSSWorkBook object will read the Excel 97-2000 formats  
                sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook  
            }
            else
            {
                XSSFWorkbook hssfwb = new XSSFWorkbook(files.InputStream); //XSSFWorkBook will read 2007 Excel format  
                sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook   
            }

            return sheet;
        }


        public static DataTable GetDataTableFromSpreadsheet(HttpPostedFileBase MyExcelStream, string filename, bool ReadOnly)
        {

            try
            {
                var sh = GetFileStream(MyExcelStream, filename, ReadOnly);
                var dtExcelTable = new DataTable();
                dtExcelTable.Rows.Clear();
                dtExcelTable.Columns.Clear();
                var headerRow = sh.GetRow(0);
                int colCount = headerRow.LastCellNum;
                for (var c = 0; c < colCount; c++)
                    dtExcelTable.Columns.Add(headerRow.GetCell(c).ToString());
                var i = 1;
                var currentRow = sh.GetRow(i);
                while (currentRow != null)
                {
                    var dr = dtExcelTable.NewRow();
                    for (var j = 0; j < currentRow.Cells.Count; j++)
                    {
                        var cell = currentRow.GetCell(j);

                        if (cell != null)
                            switch (cell.CellType)
                            {
                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(cell))
                                    {
                                        DateTime date = cell.DateCellValue;
                                        dr[j] = date.ToString("dd/MM/yyyy");
                                    }
                                    else
                                    {
                                        dr[j] = cell.NumericCellValue.ToString();
                                    }
                                    break;
                                case CellType.String:
                                    dr[j] = cell.StringCellValue;
                                    break;
                                case CellType.Blank:
                                    dr[j] = string.Empty;
                                    break;
                            }
                    }
                    dtExcelTable.Rows.Add(dr);
                    i++;
                    currentRow = sh.GetRow(i);
                }
                return dtExcelTable;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        /// <summary>
        /// Save qci report data
        /// </summary>
        /// <param name="dt"></param>
        public List<SalesReport> SaveExcelReportData(DataTable dt)
        {
            var SalesReportList = new List<SalesReport>();
            try
            {
                if (dt == null)
                {
                    return null;
                }

                var userDetail = new SalesReport();
                DataColumnCollection columns = dt.Columns;

                foreach (DataRow row in dt.Rows)
                {

                    userDetail = new SalesReport();
                    if (columns.Contains("Bill No"))
                    {
                        userDetail.BillNo = Convert.ToString(row["Bill No"]);
                    }
                    if (columns.Contains("Party Name"))
                    {
                        userDetail.PartyName = Convert.ToString(row["Party Name"]);
                    }
                    if (columns.Contains("ID No"))
                    {
                        userDetail.PartyCode = Convert.ToString(row["ID No"]);
                    }
                    if (columns.Contains("Bill By"))
                    {
                        userDetail.SoldBy = Convert.ToString(row["Bill By"]);
                    }
                    if (columns.Contains("Name"))
                    {
                        userDetail.Name = Convert.ToString(row["Name"]);
                    }
                    if (columns.Contains("Courier"))
                    {
                        userDetail.CourierName = Convert.ToString(row["Courier"]);
                    }
                    if (columns.Contains("Weight"))
                    {
                        userDetail.DocWeight = Convert.ToString(row["Weight"]);
                    }
                    if (columns.Contains("Docket No"))
                    {
                        userDetail.DocketNo = Convert.ToString(row["Docket No"]);
                    }
                    if (columns.Contains("Delv Address"))
                    {
                        userDetail.DelvAddress = Convert.ToString(row["Delv Address"]);
                    }
                    if (columns.Contains("CID"))
                    {
                        userDetail.CID = Convert.ToString(row["CID"]);
                    }
                    if (columns.Contains("Net Pay"))
                    {
                        userDetail.NetPayable = Convert.ToString(row["Net Pay"]);
                    }
                    if (columns.Contains("Mobile No"))
                    {
                        userDetail.MobileNO = Convert.ToString(row["Mobile No"]);
                    }
                    if (columns.Contains("Order No"))
                    {
                        userDetail.OrderNo = Convert.ToString(row["Order No"]);
                    }

                    try
                    {
                        if (columns.Contains("Disp Date"))
                        {
                            var date = Convert.ToString(row["Disp Date"]);
                            if (!String.IsNullOrEmpty(date) && !date.StartsWith("#"))
                            {
                                userDetail.DispDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    try
                    {
                        if (columns.Contains("Bill Date"))
                        {
                            var date = Convert.ToString(row["Bill Date"]);
                            if (!String.IsNullOrEmpty(date) && !date.StartsWith("#"))
                            {
                                userDetail.BillDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    try
                    {
                        if (columns.Contains("Docket Date"))
                        {
                            var date = Convert.ToString(row["Docket Date"]);
                            if (!String.IsNullOrEmpty(date) && !date.StartsWith("#"))
                            {
                                userDetail.DocketDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    try
                    {
                        if (columns.Contains("DOD"))
                        {
                            var date = Convert.ToString(row["DOD"]);
                            if (!String.IsNullOrEmpty(date) && !date.StartsWith("#"))
                            {
                                userDetail.DOD = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    SalesReportList.Add(userDetail);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return SalesReportList;
        }

        [HttpPost]
        public ActionResult CheckForOffer(DistributorBillModel objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            if (objModel != null)
            {
                objModel.objListProduct = new List<ProductModel>();
                if (!string.IsNullOrEmpty(objModel.objProductListStr))
                {
                    var objects = JArray.Parse(objModel.objProductListStr); // parse as array  
                    foreach (JObject root in objects)
                    {
                        ProductModel objTemp = new ProductModel();
                        foreach (KeyValuePair<String, JToken> app in root)
                        {
                            if (app.Key == "Code")
                            {
                                objTemp.ProdCode = (int)app.Value;
                            }
                            else if (app.Key == "ProductName")
                            {
                                objTemp.ProductName = (string)app.Value;
                            }
                            else if (app.Key == "Rate")
                            {
                                objTemp.Rate = (decimal)app.Value;
                            }
                            else if (app.Key == "Barcode")
                            {
                                objTemp.Barcode = app.Value.ToString();
                            }
                            else if (app.Key == "BatchNo")
                            {
                                objTemp.BatchNo = app.Value.ToString();
                            }
                            else if (app.Key == "MRP")
                            {
                                objTemp.MRP = (decimal?)app.Value;
                            }
                            else if (app.Key == "Qty")
                            {
                                objTemp.Quantity = (decimal)app.Value;
                            }
                            else if (app.Key == "PV")
                            {
                                objTemp.PV = (decimal)app.Value;
                            }
                            else if (app.Key == "CV")
                            {
                                objTemp.CV = (decimal)app.Value;
                            }
                            else if (app.Key == "BV")
                            {
                                objTemp.BV = (decimal)app.Value;
                            }
                            else if (app.Key == "RP")
                            {
                                objTemp.RP = (decimal)app.Value;
                            }
                            else if (app.Key == "DP")
                            {
                                objTemp.DP = (decimal)app.Value;
                            }
                            else if (app.Key == "CVValue")
                            {
                                objTemp.CVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "PVValue")
                            {
                                objTemp.PVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "BVValue")
                            {
                                objTemp.BVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "DiscPer")
                            {
                                objTemp.DiscPer = (decimal)app.Value;
                            }
                            else if (app.Key == "DiscAmt")
                            {
                                objTemp.DiscAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxAmt")
                            {
                                objTemp.TaxAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxPer")
                            {
                                objTemp.TaxPer = (decimal)app.Value;
                            }
                            else if (app.Key == "Amount")
                            {
                                objTemp.Amount = (decimal)app.Value;
                            }
                            else if (app.Key == "TotalAmount")
                            {
                                objTemp.TotalAmount = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxType")
                            {
                                objTemp.TaxType = (string)app.Value;
                            }
                            else if (app.Key == "PVValue")
                            {
                                objTemp.DiscAmt = (decimal)app.Value;

                            }
                            else if (app.Key == "AvailStock")
                            {
                                objTemp.StockAvailable = (decimal)app.Value;
                            }
                            else if (app.Key == "CommsnPer")
                            {
                                objTemp.CommissionPer = (decimal)app.Value;
                            }
                            else if (app.Key == "CommsnAmt")
                            {
                                objTemp.CommissionAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "RPValue")
                            {
                                objTemp.RPValue = (decimal)app.Value;
                            }
                            else if (app.Key == "SubCatId")
                            {
                                objTemp.SubCatId = (decimal)app.Value;
                            }
                        }
                        objModel.objListProduct.Add(objTemp);
                    }
                    objModel.objCustomer.UserDetails = Session["LoginUser"] as User;
                    // Retrive the Name of HOST
                    string hostName = Dns.GetHostName();
                    // Get the IP  
                    string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                    string currentDate = DateTime.Now.ToString("yyyyMMddHHmmssfff"); ;
                    objModel.objProduct.UID = myIP + currentDate;
                    objResponse = objTransacManager.CheckForOffer(objModel);
                }

            }
            else
            {
                objResponse.ResponseMessage = "Something went wrong!";
                objResponse.ResponseStatus = "FAILED";
            }

            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult PackUnpack()
        {
            PackUnpack objPackUnpack = new PackUnpack();
            try
            {
                int LoginPartyId = (Session["LoginUser"] as User).UserId;
                string LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
                objPackUnpack.kitlist = objTransacManager.GetKitList();
                objPackUnpack.UserId = LoginPartyId;
                objPackUnpack.UserCode = LoginPartyCode;
                if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "PackUnpack"))
                    return View(objPackUnpack);
                else
                    return RedirectToAction("Dashboard", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Dashboard", "Home");
            }

        }

        public ActionResult GetPackUnpackProducts(string PackUnpack, decimal KitId, string prodID)
        {
            string LoginPartyCode = string.Empty;
            if (Session["LoginUser"] != null)
            {
                LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
            }
            List<PackUnpackProduct> objResponse = objTransacManager.GetPackUnpackProducts(PackUnpack, KitId, prodID, LoginPartyCode);
            var jsonProduct = Json(objResponse, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;

        }

        [HttpPost]
        public ActionResult SavePackUnpack(PackUnpack obj)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                obj.productList = new List<PackUnpackProduct>();
                if (!string.IsNullOrEmpty(obj.productstring))
                {
                    var objects = JArray.Parse(obj.productstring); // parse as array  
                    foreach (JObject root in objects)
                    {
                        PackUnpackProduct objTemp = new PackUnpackProduct();
                        foreach (KeyValuePair<String, JToken> app in root)
                        {
                            if (app.Key == "ProductId")
                            {
                                objTemp.ProductId = (string)app.Value;
                            }
                            if (app.Key == "ProductName")
                            {
                                objTemp.ProductName = (string)app.Value;
                            }
                            if (app.Key == "Qunatity")
                            {
                                objTemp.Qunatity = (string)app.Value;
                            }
                            if (app.Key == "AvailStock")
                            {
                                objTemp.AvailStock = (string)app.Value;
                            }
                        }
                        obj.productList.Add(objTemp);
                    }
                }
                objResponse = objTransacManager.SavePackUnpack(obj);
            }
            catch (Exception ex)
            {

            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        public bool CanUserAccessMenu(int UserID, string MenuFile)
        {
            return objTransacManager.CanUserAccessMenu(UserID, MenuFile);
        }

        [SessionExpire]
        public ActionResult OfferMaster()
        {
            if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "OfferMaster"))
            {
                ViewBag.OfferType = 1;
                return View();
            }
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet]
        public ActionResult GetAllOfferList(decimal OfferType)
        {
            return Json(objTransacManager.GetAllOfferList(OfferType), JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult CreateOffer(string ActionName, string OfferCode)
        {
            Offer OfferDetail = new Offer();
            try
            {
                List<SelectListItem> Activeoption = new List<SelectListItem>();
                Activeoption.Add(new SelectListItem() { Text = "Active", Value = "Y" });
                Activeoption.Add(new SelectListItem() { Text = "Deactive", Value = "N" });
                ViewBag.ActiveDropDown = Activeoption;

                List<SelectListItem> option = new List<SelectListItem>();
                option.Add(new SelectListItem() { Text = "All", Value = "A" });
                option.Add(new SelectListItem() { Text = "Yes", Value = "Y" });
                //option.Add(new SelectListItem() { Text = "No", Value = "N" });
                ViewBag.DropDownOptions = option;

                List<SelectListItem> idStatus = new List<SelectListItem>();
                idStatus.Add(new SelectListItem() { Text = "All", Value = "A" });
                idStatus.Add(new SelectListItem() { Text = "Active", Value = "Y" });
                idStatus.Add(new SelectListItem() { Text = "Deactive", Value = "N" });
                ViewBag.idStatusDropDown = idStatus;

                List<SelectListItem> ForBillType = new List<SelectListItem>();
                ForBillType.Add(new SelectListItem() { Text = "All", Value = "All" });
                ForBillType.Add(new SelectListItem() { Text = "Repurchase", Value = "Repurchase" });
                ForBillType.Add(new SelectListItem() { Text = "FirstBill", Value = "FirstBill" });
                ViewBag.ForBillTypeDropDown = ForBillType;

                OfferDetail.OfferFromDt = DateTime.Now;
                OfferDetail.OfferToDt = DateTime.Now;
                string LoginPartyCode = "";
                decimal LoginStateCode = 0;

                if (Session["LoginUser"] != null)
                {
                    LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
                    LoginStateCode = (Session["LoginUser"] as User).StateCode;
                }

                List<PartyModel> objparty = objRegistrationManager.GetAllPartyList(false);
                objparty = objparty.Where(r => r.GroupId != 5).ToList();
                if (ActionName.ToLower() == "edit")
                {
                    if (!string.IsNullOrEmpty(OfferCode))
                    {
                        decimal OfferId = decimal.Parse(OfferCode);
                        OfferDetail = objTransacManager.GetSelectedOfferDetails(OfferId);
                    }
                }

                OfferDetail.OfferParty = objparty;
                OfferDetail.Action = ActionName;
                OfferDetail.offerType = 1;

            }
            catch (Exception ex)
            {

            }
            return View(OfferDetail);

        }

        [HttpGet]
        public ActionResult GetSelectedOfferProducts(decimal OfferId, int? type)
        {
            return Json(objTransacManager.GetSelectedOfferProductList(OfferId, type ?? 0), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveOffer(Offer obj)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                obj.OfferProds = new List<OfferProducts>();
                if (!string.IsNullOrEmpty(obj.PrductString))
                {
                    var objects = JArray.Parse(obj.PrductString); // parse as array  
                    foreach (JObject root in objects)
                    {
                        OfferProducts objTemp = new OfferProducts();
                        foreach (KeyValuePair<String, JToken> app in root)
                        {
                            if (app.Key == "ProdCode")
                            {
                                objTemp.ProdID = (string)app.Value;
                            }
                            if (app.Key == "Qty")
                            {
                                objTemp.Qty = (decimal)app.Value;
                            }
                            if (app.Key == "FreeQty")
                            {
                                objTemp.FreeQty = (decimal)app.Value;
                            }
                            if (app.Key == "Flexible")
                            {
                                objTemp.IsFlexible = (string)app.Value;
                            }
                            if (app.Key == "OfferMrp")
                            {
                                objTemp.OfferMrp = (decimal?)app.Value;
                            }
                            objTemp.BuyProduct = "N";
                        }
                        obj.OfferProds.Add(objTemp);
                    }
                }

                if (!string.IsNullOrEmpty(obj.BuyPrductString))
                {
                    var objects = JArray.Parse(obj.BuyPrductString); // parse as array  
                    foreach (JObject root in objects)
                    {
                        OfferProducts objTemp = new OfferProducts();
                        foreach (KeyValuePair<String, JToken> app in root)
                        {
                            if (app.Key == "ProdCode")
                            {
                                objTemp.ProdID = (string)app.Value;
                            }
                            if (app.Key == "Qty")
                            {
                                objTemp.Qty = (decimal)app.Value;
                            }
                            objTemp.BuyProduct = "Y";
                        }
                        obj.OfferProds.Add(objTemp);
                    }
                }
                obj.CreatedBy = (Session["LoginUser"] as User).UserId;
                objResponse = objTransacManager.SaveOffer(obj);
            }
            catch (Exception ex)
            {

            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetproductInfobyCode(string data)
        {
            List<ProductModel> model = new List<ProductModel>();
            model = objTransacManager.GetproductInfobyCode(data);
            return Json(model, JsonRequestBehavior.AllowGet);

        }

        [SessionExpire]
        public ActionResult OtherOfferMaster()
        {
            if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "OtherOfferMaster"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet]
        public ActionResult GetAllOtherOfferList()
        {
            return Json(objTransacManager.GetAllOtherOfferList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSelectedOtherOfferDetails(decimal OfferId)
        {
            return Json(objTransacManager.GetSelectedOtherOfferDetails(OfferId), JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult OtherOffer(string ActionName, string OfferCode)
        {
            Offer OfferDetail = new Offer();
            try
            {
                List<SelectListItem> Activeoption = new List<SelectListItem>();
                Activeoption.Add(new SelectListItem() { Text = "Active", Value = "Y" });
                Activeoption.Add(new SelectListItem() { Text = "Deactive", Value = "N" });
                ViewBag.ActiveOptionDropDown = Activeoption;

                List<SelectListItem> ForBillType = new List<SelectListItem>();
                ForBillType.Add(new SelectListItem() { Text = "All", Value = "All" });
                ForBillType.Add(new SelectListItem() { Text = "Repurchase", Value = "RV" });
                ForBillType.Add(new SelectListItem() { Text = "FirstBill", Value = "FV" });
                ViewBag.ForBillTypeDropDown = ForBillType;

                if (ActionName.ToLower() == "edit")
                {
                    if (!string.IsNullOrEmpty(OfferCode))
                    {
                        decimal OfferId = decimal.Parse(OfferCode);
                        OfferDetail = objTransacManager.GetSelectedOtherOfferDetails(OfferId);
                    }
                }
                List<PartyModel> objparty = objRegistrationManager.GetAllPartyList(false);
                objparty = objparty.Where(r => r.GroupId != 5).ToList();
                OfferDetail.OfferParty = objparty;
                OfferDetail.Action = ActionName;
                OfferDetail.offerType = 9999;
            }
            catch (Exception ex)
            {

            }
            return View(OfferDetail);

        }

        [HttpPost]
        public ActionResult OtherOfferSave(Offer obj)
        {


            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                obj.OfferProds = new List<OfferProducts>();
                if (!string.IsNullOrEmpty(obj.PrductString))
                {
                    var objects = JArray.Parse(obj.PrductString); // parse as array  
                    foreach (JObject root in objects)
                    {
                        OfferProducts objTemp = new OfferProducts();
                        foreach (KeyValuePair<String, JToken> app in root)
                        {
                            if (app.Key == "ProdCode")
                            {
                                objTemp.ProdID = (string)app.Value;
                            }
                            if (app.Key == "Qty")
                            {
                                objTemp.Qty = (decimal)app.Value;
                            }
                            if (app.Key == "FreeQty")
                            {
                                objTemp.FreeQty = (decimal)app.Value;
                            }
                            if (app.Key == "Flexible")
                            {
                                objTemp.IsFlexible = (string)app.Value;
                            }
                            if (app.Key == "OfferMrp")
                            {
                                objTemp.OfferMrp = (decimal?)app.Value;
                            }
                            objTemp.BuyProduct = "N";
                        }
                        obj.OfferProds.Add(objTemp);
                    }
                }
                objResponse = objTransacManager.SaveOtherOffer(obj);
            }
            catch (Exception ex)
            {

            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveDraw(Draw obj)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                objResponse = objTransacManager.SaveDraw(obj);
            }
            catch (Exception ex)
            {

            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteDraw(decimal id)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                Draw d = new Draw();
                d.AID = id;
                d.UserID = (Session["LoginUser"] as User).UserId;
                objResponse = objTransacManager.DeleteDraw(d);
            }
            catch (Exception ex)
            {

            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult LuckyDrawMaster()
        {
            Draw draw = new Draw();
            if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "LuckyDrawMaster"))
            {
                draw.FDate = DateTime.Now;
                ViewBag.ProductList = objTransacManager.getDrawProductList();
                return View();
            }
            else
                return RedirectToAction("Dashboard", "Home");

        }

        [HttpGet]
        public ActionResult getDrawProductList()
        {
            return Json(objTransacManager.getDrawProductList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getAllDraws()
        {
            return Json(objTransacManager.getAllDraws(), JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult DrawProduct()
        {
            if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "DrawProduct"))
            {
                return View();
            }
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult SaveDrawProducts(DrawProds PermittedList)
        {
            ResponseDetail objResponse = new ResponseDetail();
            List<DrawProds> objPermissionList = new List<DrawProds>();

            if (!string.IsNullOrEmpty(PermittedList.prodString))
            {
                var objects = JArray.Parse(PermittedList.prodString); // parse as array  
                foreach (JObject root in objects)
                {
                    DrawProds objTemp = new DrawProds();
                    foreach (KeyValuePair<String, JToken> app in root)
                    {
                        if (app.Key == "ProdCode")
                        {
                            objTemp.ProdCode = (decimal)app.Value;
                        }
                        else if (app.Key == "ProductName")
                        {
                            objTemp.ProductName = (string)app.Value;
                        }
                        else if (app.Key == "ActiveStatus")
                        {
                            objTemp.IsActive = (bool)app.Value;
                        }
                    }

                    objPermissionList.Add(objTemp);
                }
            }
            objResponse = objTransacManager.SaveDrawPoducts(objPermissionList);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult FPVInvoice()
        {


            DistributorBillModel objDistributorModel = new DistributorBillModel();
            try
            {
                //if (objTransacManager.IsFPVInvoiceValid()) { //Cmnted on 04Mar19
                // if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "DistributorBill"))
                // {
                objDistributorModel.objCustomer = new CustomerDetail();
                objDistributorModel.objProduct = new ProductModel();
                List<SelectListItem> objBankList = new List<SelectListItem>();
                var result = objTransacManager.GetBankList();
                objDistributorModel.objProduct.PayDetails = new PayDetails();
                foreach (var obj in result)
                {
                    if (obj.BankCode == 0)
                    {
                        objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString(), Selected = true });
                        objDistributorModel.objProduct.PayDetails.BDBankName = obj.BankCode.ToString();
                    }
                    else
                    {
                        objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString() });
                    }
                }
                ViewBag.BankNames = objBankList;
                List<SelectListItem> CardTypes = new List<SelectListItem>();
                CardTypes.Add(new SelectListItem { Text = "Credit Card", Value = "CC" });
                CardTypes.Add(new SelectListItem { Text = "Debit Card", Value = "DB" });
                ViewBag.CardTypes = CardTypes;

                objDistributorModel.objProduct.PayDetails.CardType = "CC";

                //List<SelectListItem> objListCustomerTypes = new List<SelectListItem>();
                //objListCustomerTypes.Add(new SelectListItem { Text = "New", Value = "New" });
                //objListCustomerTypes.Add(new SelectListItem { Text = "Existing", Value = "Existing" });
                //ViewBag.CustomerType = objListCustomerTypes;

                //ViewBag.ConfigDetails = objTransacManager.GetConfigDetails();
                //objDistributorModel.objConfigDetails = objTransacManager.GetConfigDetails();

                objDistributorModel.objCustomer.CustomerType = "New";
                //var OfferList = objTransacManager.GetOfferList();
                List<SelectListItem> OfferSelectList = new List<SelectListItem>();
                OfferSelectList.Add(new SelectListItem { Text = "--Choose Offer--", Value = "0" });
                //foreach (var obj in OfferList)
                //{ OfferSelectList.Add(new SelectListItem { Value = obj.ProdCode.ToString(), Text = obj.ProductName }); }
                ViewBag.OfferList = OfferSelectList;

                var KitIdlist = objTransacManager.GetKitIdList();
                List<SelectListItem> KidIsListObj = new List<SelectListItem>();
                KidIsListObj.Add(new SelectListItem { Text = "--Select Kit--", Value = "0" });
                foreach (var obj in KitIdlist)
                {
                    KidIsListObj.Add(new SelectListItem { Text = obj.KitName, Value = obj.KitId.ToString() });
                }
                ViewBag.objKitList = KidIsListObj;
                objDistributorModel.objCustomer.KitId = 0;
                //objDistributorModel.objCustomer.IsRegisteredCustomer = true;
                //objDistributorModel.objCustomer.ReferenceIdNo = InventorySession.LoginUser.PartyCode;
                //objDistributorModel.objCustomer.ReferenceName = InventorySession.LoginUser.PartyName;
                InventorySession.StoredDistributorValues = null;
                return View(objDistributorModel);
                //}
                //else
                //    return RedirectToAction("Dashboard", "Home");

            }
            catch (Exception ex)
            {
                LogError(ex);
                Console.Write("Exception:", ex.Message);
            }
            return RedirectToAction("Dashboard", "Home"); ;
        }
        [SessionExpire]
        public ActionResult CPVInvoice()
        {


            DistributorBillModel objDistributorModel = new DistributorBillModel();
            try
            {
                //if (objTransacManager.IsFPVInvoiceValid()) { //Cmnted on 04Mar19
                // if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "DistributorBill"))
                // {
                objDistributorModel.objCustomer = new CustomerDetail();
                objDistributorModel.objProduct = new ProductModel();
                List<SelectListItem> objBankList = new List<SelectListItem>();
                var result = objTransacManager.GetBankList();
                objDistributorModel.objProduct.PayDetails = new PayDetails();
                foreach (var obj in result)
                {
                    if (obj.BankCode == 0)
                    {
                        objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString(), Selected = true });
                        objDistributorModel.objProduct.PayDetails.BDBankName = obj.BankCode.ToString();
                    }
                    else
                    {
                        objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString() });
                    }
                }
                ViewBag.BankNames = objBankList;
                List<SelectListItem> CardTypes = new List<SelectListItem>();
                CardTypes.Add(new SelectListItem { Text = "Credit Card", Value = "CC" });
                CardTypes.Add(new SelectListItem { Text = "Debit Card", Value = "DB" });
                ViewBag.CardTypes = CardTypes;

                objDistributorModel.objProduct.PayDetails.CardType = "CC";

                //List<SelectListItem> objListCustomerTypes = new List<SelectListItem>();
                //objListCustomerTypes.Add(new SelectListItem { Text = "New", Value = "New" });
                //objListCustomerTypes.Add(new SelectListItem { Text = "Existing", Value = "Existing" });
                //ViewBag.CustomerType = objListCustomerTypes;

                //ViewBag.ConfigDetails = objTransacManager.GetConfigDetails();
                //objDistributorModel.objConfigDetails = objTransacManager.GetConfigDetails();

                objDistributorModel.objCustomer.CustomerType = "New";
                //var OfferList = objTransacManager.GetOfferList();
                List<SelectListItem> OfferSelectList = new List<SelectListItem>();
                OfferSelectList.Add(new SelectListItem { Text = "--Choose Offer--", Value = "0" });
                //foreach (var obj in OfferList)
                //{ OfferSelectList.Add(new SelectListItem { Value = obj.ProdCode.ToString(), Text = obj.ProductName }); }
                ViewBag.OfferList = OfferSelectList;

                var KitIdlist = objTransacManager.GetKitIdList();
                List<SelectListItem> KidIsListObj = new List<SelectListItem>();
                KidIsListObj.Add(new SelectListItem { Text = "--Select Kit--", Value = "0" });
                foreach (var obj in KitIdlist)
                {
                    KidIsListObj.Add(new SelectListItem { Text = obj.KitName, Value = obj.KitId.ToString() });
                }
                ViewBag.objKitList = KidIsListObj;
                objDistributorModel.objCustomer.KitId = 0;
                //objDistributorModel.objCustomer.IsRegisteredCustomer = true;
                //objDistributorModel.objCustomer.ReferenceIdNo = InventorySession.LoginUser.PartyCode;
                //objDistributorModel.objCustomer.ReferenceName = InventorySession.LoginUser.PartyName;
                InventorySession.StoredDistributorValues = null;
                return View(objDistributorModel);
                //}
                //else
                //    return RedirectToAction("Dashboard", "Home");

            }
            catch (Exception ex)
            {
                LogError(ex);
                Console.Write("Exception:", ex.Message);
            }
            return RedirectToAction("Dashboard", "Home"); ;
        }


        [SessionExpire]
        public ActionResult GetFreeVoucherDetail(string VoucherNo, string IdNo)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objTransacManager.GetFreeVoucherDetail(VoucherNo, IdNo);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult GetFreeCPVVoucherDetail(string VoucherNo, string IdNo)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objTransacManager.GetFreeCPVVoucherDetail(VoucherNo, IdNo);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProductList()
        {
            decimal LoginStateCode = (Session["LoginUser"] as User).StateCode;
            List<ProductDetails> objProductList = objProductManager.GetProductList(LoginStateCode);
            var list = (from r in objProductList
                        where r.IsActive == true && r.IsAvailableforOffers == true
                        select new
                        {
                            ProductName = r.ProductName,
                            ProductCode = r.ProductCodeStr
                        }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllProductList()
        {
            decimal LoginStateCode = (Session["LoginUser"] as User).StateCode;
            List<ProductDetails> objProductList = objProductManager.GetProductList(LoginStateCode);
            var list = (from r in objProductList
                        where r.IsActive == true
                        select new
                        {
                            ProductName = r.ProductName,
                            ProductCode = r.ProductCodeStr
                        }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult IssueSampleProduct()
        {
            if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "IssueSampleProduct"))
            {
                return View();
            }
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public ActionResult SaveIssueSampleProducts(DistributorBillModel objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            if (objModel != null)
            {
                objModel.objListProduct = new List<ProductModel>();
                if (!string.IsNullOrEmpty(objModel.objProductListStr))
                {
                    var objects = JArray.Parse(objModel.objProductListStr); // parse as array  
                    foreach (JObject root in objects)
                    {
                        ProductModel objTemp = new ProductModel();
                        foreach (KeyValuePair<String, JToken> app in root)
                        {
                            if (app.Key == "Code")
                            {
                                objTemp.ProductCodeStr = (string)app.Value;
                            }
                            else if (app.Key == "ProductName")
                            {
                                objTemp.ProductName = (string)app.Value;
                            }

                            else if (app.Key == "Barcode")
                            {
                                objTemp.Barcode = app.Value.ToString();
                            }
                            else if (app.Key == "BatchNo")
                            {
                                objTemp.BatchNo = app.Value.ToString();
                            }

                            else if (app.Key == "Qty")
                            {
                                objTemp.Quantity = (decimal)app.Value;
                            }
                        }
                        objModel.objListProduct.Add(objTemp);
                    }
                    objModel.objCustomer.UserDetails = Session["LoginUser"] as User;
                    objResponse = objTransacManager.SaveIssueSampleProducts(objModel);
                }
            }
            else
            {
                objResponse.ResponseMessage = "Something went wrong!";
                objResponse.ResponseStatus = "FAILED";
            }

            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetFPVProductsOnly(int ProdBunchID, string InvoiceType)
        {
            List<string> model = objTransacManager.GetFPVProductsOnly(ProdBunchID, InvoiceType);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult OneRupeeOffer(string ActionName, string OfferCode)
        {
            Offer OfferDetail = new Offer();
            try
            {
                List<SelectListItem> Activeoption = new List<SelectListItem>();
                Activeoption.Add(new SelectListItem() { Text = "Active", Value = "Y" });
                Activeoption.Add(new SelectListItem() { Text = "Deactive", Value = "N" });
                ViewBag.ActiveDropDown = Activeoption;

                List<SelectListItem> option = new List<SelectListItem>();
                option.Add(new SelectListItem() { Text = "All", Value = "A" });
                option.Add(new SelectListItem() { Text = "Yes", Value = "Y" });
                //option.Add(new SelectListItem() { Text = "No", Value = "N" });
                ViewBag.DropDownOptions = option;

                List<SelectListItem> idStatus = new List<SelectListItem>();
                idStatus.Add(new SelectListItem() { Text = "All", Value = "A" });
                idStatus.Add(new SelectListItem() { Text = "Active", Value = "Y" });
                idStatus.Add(new SelectListItem() { Text = "Deactive", Value = "N" });
                ViewBag.idStatusDropDown = idStatus;

                List<SelectListItem> ForBillType = new List<SelectListItem>();
                ForBillType.Add(new SelectListItem() { Text = "All", Value = "All" });
                ForBillType.Add(new SelectListItem() { Text = "Repurchase", Value = "Repurchase" });
                ForBillType.Add(new SelectListItem() { Text = "FirstBill", Value = "FirstBill" });
                ViewBag.ForBillTypeDropDown = ForBillType;
                List<SelectListItem> isfixed = new List<SelectListItem>();
                isfixed.Add(new SelectListItem() { Text = "No", Value = "N" });
                isfixed.Add(new SelectListItem() { Text = "Yes", Value = "Y" });
                //option.Add(new SelectListItem() { Text = "No", Value = "N" });
                ViewBag.IsFixedQtyDropDown = isfixed;
                OfferDetail.OfferFromDt = DateTime.Now;
                OfferDetail.OfferToDt = DateTime.Now;
                if (ActionName.ToLower() == "edit")
                {
                    if (!string.IsNullOrEmpty(OfferCode))
                    {
                        decimal OfferId = decimal.Parse(OfferCode);
                        OfferDetail = objTransacManager.GetSelectedOfferDetails(OfferId);
                    }
                }
                OfferDetail.Action = ActionName;
                OfferDetail.offerType = 2;
                List<PartyModel> objparty = objRegistrationManager.GetAllPartyList(false);
                objparty = objparty.Where(r => r.GroupId != 5).ToList();
                OfferDetail.OfferParty = objparty;
            }
            catch (Exception ex)
            {

            }
            return View(OfferDetail);

        }

        [SessionExpire]
        public ActionResult OneRupeeOfferMaster()
        {
            if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "OneRupeeOfferMaster"))
            {
                ViewBag.OfferType = 2;
                return View("OfferMaster");
            }
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet]
        public ActionResult CheckBillCustomer(string mobile)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objTransacManager.CheckBillCustomer(mobile);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }
        [SessionExpire]
        public ActionResult UpgradeID()
        {
            if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "UpgradeID"))
            {
                return View("Upgrade_ID_With_Package");
            }
            else
                return RedirectToAction("Dashboard", "Home");

        }

        [HttpPost]
        public ActionResult ActivateIdWithPackage(UpgradeID objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            if (objModel != null)
            {
                objModel.objListProduct = new List<ProductModel>();
                if (!string.IsNullOrEmpty(objModel.productstring))
                {
                    var objects = JArray.Parse(objModel.productstring); // parse as array  
                    decimal totalBV = 0;
                    decimal totalRP = 0;
                    decimal totalPV = 0;
                    decimal totalCV = 0;
                    decimal TotalNetPayable = 0;
                    decimal Totaltax = 0;
                    decimal TotalDisc = 0;
                    foreach (JObject root in objects)
                    {
                        ProductModel objTemp = new ProductModel();
                        foreach (KeyValuePair<String, JToken> app in root)
                        {

                            if (app.Key == "ProductId")
                            {
                                objTemp.ProductCodeStr = (string)app.Value;
                            }
                            if (app.Key == "Amount")
                            {
                                objTemp.Amount = (decimal)app.Value;
                            }
                            else if (app.Key == "ProductName")
                            {
                                objTemp.ProductName = (string)app.Value;
                            }

                            else if (app.Key == "Rate")
                            {
                                objTemp.Rate = (decimal)app.Value;
                            }
                            else if (app.Key == "Barcode")
                            {
                                objTemp.Barcode = app.Value.ToString();
                            }
                            else if (app.Key == "ProductTye")
                            {
                                objTemp.ProductTye = app.Value.ToString();
                            }
                            else if (app.Key == "MRP")
                            {
                                objTemp.MRP = (decimal?)app.Value;
                            }
                            else if (app.Key == "Quantity")
                            {
                                objTemp.Quantity = (decimal)app.Value;
                            }
                            else if (app.Key == "PV")
                            {
                                objTemp.PV = (decimal)app.Value;
                            }
                            else if (app.Key == "CV")
                            {
                                objTemp.CV = (decimal)app.Value;
                            }
                            else if (app.Key == "BV")
                            {
                                objTemp.BV = (decimal)app.Value;
                            }
                            else if (app.Key == "RP")
                            {
                                objTemp.RP = (decimal)app.Value;
                            }
                            else if (app.Key == "DP")
                            {
                                objTemp.DP = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxAmt")
                            {
                                objTemp.TaxAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxPer")
                            {
                                objTemp.TaxPer = (decimal)app.Value;
                            }
                            else if (app.Key == "DiscAmt")
                            {
                                objTemp.DiscAmt = (decimal)app.Value;
                            }


                        }
                        objTemp.BVValue = objTemp.BV * objTemp.Quantity;
                        objTemp.RPValue = objTemp.RP * objTemp.Quantity;
                        objTemp.CVValue = objTemp.CV * objTemp.Quantity;
                        objTemp.PVValue = objTemp.PV * objTemp.Quantity;

                        totalBV += objTemp.BVValue ?? 0;
                        totalRP += objTemp.RPValue ?? 0;
                        totalCV += objTemp.CVValue ?? 0;
                        totalPV += objTemp.PVValue ?? 0;
                        Totaltax += objTemp.TaxAmt ?? 0;

                        TotalDisc += objTemp.DiscAmt ?? 0;
                        TotalNetPayable += objTemp.Amount;
                        objModel.objListProduct.Add(objTemp);
                    }
                    objModel.objProduct = new ProductModel();
                    objModel.objCustomer = new CustomerDetail();
                    objModel.objCustomer.UserDetails = Session["LoginUser"] as User;
                    string hostName = Dns.GetHostName();
                    string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                    string currentDate = DateTime.Now.ToString("yyyyMMddHHmmssfff"); ;
                    objModel.objProduct.UID = myIP + currentDate;
                    objModel.objProduct.TotalDiscount = TotalDisc;// TotalNetPayable + Totaltax - objModel.KitAmount;
                    objModel.objProduct.TotalBV = totalBV;
                    objModel.objProduct.TotalRP = totalRP;
                    objModel.objProduct.TotalCV = totalCV;
                    objModel.objProduct.TotalPV = totalPV;
                    objModel.objProduct.TotalAmount = TotalNetPayable;
                    objModel.objProduct.TotalTaxAmount = Totaltax;
                    objModel.objProduct.TotalNetPayable = objModel.KitAmount;
                    decimal WBalance = objTransacManager.GetPartyWalletBalance(objModel.objCustomer.UserDetails.BranchCode, "R");

                    if (WBalance < Math.Round(objModel.objProduct.TotalNetPayable) && objModel.objCustomer.UserDetails.BranchCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                    {
                        objResponse.ResponseStatus = "FAILED";
                        objResponse.ResponseMessage = "Insufficient Balance in your wallet";
                    }
                    else
                    {
                        objResponse = objTransacManager.ActivateIdWithPackage(objModel);
                        WBalance = objTransacManager.GetPartyWalletBalance(objModel.objCustomer.UserDetails.BranchCode, "R");
                        (Session["LoginUser"] as User).WBalance = WBalance.ToString();
                    }
                }

            }
            else
            {
                objResponse.ResponseMessage = "Something went wrong!";
                objResponse.ResponseStatus = "FAILED";
            }

            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomerKitDetail(string IdNo)
        {
            UpgradeID model = new UpgradeID();
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
            bool IsHO = false;
            if (CurrentPartyCode == System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                IsHO = true;

            model = objTransacManager.GetCustomerKitDetail(IdNo, IsHO);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetKitProductList(string kitId)
        {
            UpgradeID model = new UpgradeID();
            model = objTransacManager.GetKitProductList(kitId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult AddWallet()
        {
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
            {
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
            }

            var result = objTransacManager.GetPartyBalance();

            List<SelectListItem> PartyList = new List<SelectListItem>();
            foreach (var obj in result)
            {
                PartyList.Add(new SelectListItem
                {
                    Text = obj.PartyName + " (" + obj.PartyCode + ")",
                    Value = obj.PartyCode.ToString()
                });
            }
            ViewBag.PartyList = PartyList;

            var WalletTypes = objTransacManager.GetWalletTypes(); 
            List<SelectListItem> ACType = new List<SelectListItem>();
            foreach (var obj in WalletTypes)
            {
                ACType.Add(new SelectListItem
                {
                    Text = obj.Voucher_Discrption,
                    Value = obj.Vtype
                });
            }
            //ACType.Add(new SelectListItem
            //{
            //    Text = "Main Wallet",
            //    Value = "R"
            //});
            //ACType.Add(new SelectListItem
            //{
            //    Text = "PV Wallet Purchase Balanace",
            //    Value = "W"
            //});
            //ACType.Add(new SelectListItem
            //{
            //    Text = "BV Wallet Purchase Balanace",
            //    Value = "Z"
            //});
            //ACType.Add(new SelectListItem
            //{
            //    Text = "PV Wallet",
            //    Value = "P"
            //});
            //ACType.Add(new SelectListItem
            //{
            //    Text = "BV Wallet",
            //    Value = "B"
            //});
            ViewBag.ACType = ACType;
            if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "AddWallet"))
                return View();
            else
                return RedirectToAction("Dashboard", "Home");

        }
        public ActionResult DebitCreditWallet(Wallet objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                if (objModel != null)
                {

                    objResponse = objTransacManager.DebitCreditWallet(objModel);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult GVInvoice()
        {


            DistributorBillModel objDistributorModel = new DistributorBillModel();
            try
            {
                //if (objTransacManager.IsFPVInvoiceValid()) { //Cmnted on 04Mar19
                // if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "DistributorBill"))
                // {
                objDistributorModel.objCustomer = new CustomerDetail();
                objDistributorModel.objProduct = new ProductModel();
                List<SelectListItem> objBankList = new List<SelectListItem>();
                var result = objTransacManager.GetBankList();
                objDistributorModel.objProduct.PayDetails = new PayDetails();
                foreach (var obj in result)
                {
                    if (obj.BankCode == 0)
                    {
                        objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString(), Selected = true });
                        objDistributorModel.objProduct.PayDetails.BDBankName = obj.BankCode.ToString();
                    }
                    else
                    {
                        objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString() });
                    }
                }
                ViewBag.BankNames = objBankList;
                List<SelectListItem> CardTypes = new List<SelectListItem>();
                CardTypes.Add(new SelectListItem { Text = "Credit Card", Value = "CC" });
                CardTypes.Add(new SelectListItem { Text = "Debit Card", Value = "DB" });
                ViewBag.CardTypes = CardTypes;

                objDistributorModel.objProduct.PayDetails.CardType = "CC";

                //List<SelectListItem> objListCustomerTypes = new List<SelectListItem>();
                //objListCustomerTypes.Add(new SelectListItem { Text = "New", Value = "New" });
                //objListCustomerTypes.Add(new SelectListItem { Text = "Existing", Value = "Existing" });
                //ViewBag.CustomerType = objListCustomerTypes;

                //ViewBag.ConfigDetails = objTransacManager.GetConfigDetails();
                //objDistributorModel.objConfigDetails = objTransacManager.GetConfigDetails();

                objDistributorModel.objCustomer.CustomerType = "New";
                //var OfferList = objTransacManager.GetOfferList();
                List<SelectListItem> OfferSelectList = new List<SelectListItem>();
                OfferSelectList.Add(new SelectListItem { Text = "--Choose Offer--", Value = "0" });
                //foreach (var obj in OfferList)
                //{ OfferSelectList.Add(new SelectListItem { Value = obj.ProdCode.ToString(), Text = obj.ProductName }); }
                ViewBag.OfferList = OfferSelectList;

                var KitIdlist = objTransacManager.GetKitIdList();
                List<SelectListItem> KidIsListObj = new List<SelectListItem>();
                KidIsListObj.Add(new SelectListItem { Text = "--Select Kit--", Value = "0" });
                foreach (var obj in KitIdlist)
                {
                    KidIsListObj.Add(new SelectListItem { Text = obj.KitName, Value = obj.KitId.ToString() });
                }
                ViewBag.objKitList = KidIsListObj;
                objDistributorModel.objCustomer.KitId = 0;
                //objDistributorModel.objCustomer.IsRegisteredCustomer = true;
                //objDistributorModel.objCustomer.ReferenceIdNo = InventorySession.LoginUser.PartyCode;
                //objDistributorModel.objCustomer.ReferenceName = InventorySession.LoginUser.PartyName;
                InventorySession.StoredDistributorValues = null;
                return View(objDistributorModel);
                //}
                //else
                //    return RedirectToAction("Dashboard", "Home");

            }
            catch (Exception ex)
            {
                LogError(ex);
                Console.Write("Exception:", ex.Message);
            }
            return RedirectToAction("Dashboard", "Home"); ;
        }

        [SessionExpire]
        public ActionResult GetGVDetail(string VoucherNo, string FormNo)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objTransacManager.GetGVDetail(VoucherNo, FormNo);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult WalletRequest()
        {
            WalletRequest objReq = new WalletRequest();
            List<SelectListItem> objBankList = new List<SelectListItem>();

            objBankList.Add(new SelectListItem
            {
                Text = "SBI",
                Value = "SBI"
            });

            objBankList.Add(new SelectListItem
            {
                Text = "AXIS",
                Value = "AXIS"
            });

            objBankList.Add(new SelectListItem
            {
                Text = "INDUS",
                Value = "INDUS"
            });

            objBankList.Add(new SelectListItem
            {
                Text = "HDFC",
                Value = "HDFC"
            });

            ViewBag.BankNames = objBankList;

            if (Session["LoginUserType"] as string == "mobileshoppe" || new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "WalletRequest"))
            {
                return View();
            }
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [SessionExpire]
        public ActionResult ApproveWalletRequest()
        {
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "ApproveWalletRequest"))
            {
                return View();
            }
            else
                return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> SaveWalletRequest(WalletRequest objWallet, HttpPostedFileBase upload)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseStatus = "Failed";
            objResponse.ResponseMessage = "Something went wrong.";
            if (upload != null)
            {
                var path = Server.MapPath("~/WalletReqsImages");
                //var paths = path.Split(':');
                //if (paths != null || paths.Count() > 0)
                //{
                //    path = paths[0] + ":\\" + "WalletReqs";
                //}
                if (upload != null && upload.FileName != null)
                {
                    if (!Directory.Exists(path))
                    {
                        //DirectoryInfo dInfo = new DirectoryInfo(path);
                        //DirectorySecurity dSecurity = dInfo.GetAccessControl();
                        //dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                        //dInfo.SetAccessControl(dSecurity);
                        Directory.CreateDirectory(path);
                    }
                    //string myfile = Guid.NewGuid() + "-" + BannerImage.FileName;
                    string myfile = Guid.NewGuid() + "-" + Path.GetFileName(upload.FileName);
                    myfile = myfile.Replace(" ", "").Replace(" ", "");
                    var FlName = Path.Combine(path, myfile);
                    upload.SaveAs(FlName);

                    var req = HttpContext.Request;
                    string baseUrl = string.Format("{0}://{1}/{2}", req.Url.Scheme, req.Url.Authority, "WalletReqsImages/" + myfile);

                    objWallet.ScannedFileName = baseUrl;
                    if (Session["LoginUserType"] as string == "mobileshoppe")
                    {
                        objWallet.VType = "M";
                    }
                    else
                    {
                        objWallet.VType = "R";
                    }
                }
            }
            string resp = await objTransacManager.SaveWalletRequest(objWallet);
            if (resp == "OK")
            {
                objResponse.ResponseStatus = "OK";
                objResponse.ResponseMessage = "Saved Successfully!";
            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllWalletRequest(string PartyCode, string dateType, string FromDate, string ToDate, string IsApproved)
        {
            //**Added on 24Nov18
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;

            if (PartyCode == "" || CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
                PartyCode = CurrentPartyCode;

            if (Session["LoginUserType"] as string == "mobileshoppe")
            {
                PartyCode = (Session["LoginUser"] as User).UserName;
            }
            //**
            List<WalletRequest> objModel = new List<WalletRequest>();
            objModel = objTransacManager.GetAllWalletRequest(PartyCode, dateType, FromDate, ToDate, IsApproved);
            return Json(objModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveApproveWaletRequest(WalletRequest objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            List<WalletRequest> objwallet = new List<Entity.Common.WalletRequest>();
            string logString = string.Empty;
            if (!string.IsNullOrEmpty(objModel.GridString))
            {
                var objects = JArray.Parse(objModel.GridString); // parse as array  
                foreach (JObject root in objects)
                {
                    WalletRequest objTemp = new WalletRequest();
                    foreach (KeyValuePair<String, JToken> app in root)
                    {
                        if (app.Key == "ReqNo")
                        {
                            objTemp.ReqNo = (string)app.Value;
                            logString += objTemp.ReqNo;
                        }
                        if (app.Key == "IsApproved")
                        {
                            if ((string)app.Value.ToString().ToUpper() == "REJECT")
                            {
                                objTemp.IsApproved = "R";
                                logString += "- Rejected";
                            }
                            else if ((string)app.Value.ToString().ToUpper() == "PENDING")
                            {
                                objTemp.IsApproved = "N";
                                logString += "- PENDING";
                            }
                            else if ((string)app.Value.ToString().ToUpper() == "Approve".ToUpper())
                            {
                                objTemp.IsApproved = "Y";
                                logString += "- Approved";
                            }
                            else
                                objTemp.IsApproved = (string)app.Value;
                        }
                        if (app.Key == "ApproveRemark")
                        {
                            objTemp.ApproveRemark = (string)app.Value;
                        }
                        if (app.Key == "VType")
                        {
                            objTemp.VType = (string)app.Value;
                        }
                        objTemp.ApproveBy = (Session["LoginUser"] as User).UserId;
                    }
                    objwallet.Add(objTemp);
                }
                objResponse = objTransacManager.SaveApproveWaletRequest(objwallet);

            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult FreeProductOffer(string ActionName, string OfferCode)
        {
            Offer OfferDetail = new Offer();
            try
            {
                List<SelectListItem> Activeoption = new List<SelectListItem>();
                Activeoption.Add(new SelectListItem() { Text = "Active", Value = "Y" });
                Activeoption.Add(new SelectListItem() { Text = "Deactive", Value = "N" });
                ViewBag.ActiveDropDown = Activeoption;

                List<SelectListItem> option = new List<SelectListItem>();
                option.Add(new SelectListItem() { Text = "All", Value = "A" });
                option.Add(new SelectListItem() { Text = "Yes", Value = "Y" });
                //option.Add(new SelectListItem() { Text = "No", Value = "N" });
                ViewBag.DropDownOptions = option;

                List<SelectListItem> idStatus = new List<SelectListItem>();
                idStatus.Add(new SelectListItem() { Text = "All", Value = "A" });
                idStatus.Add(new SelectListItem() { Text = "Active", Value = "Y" });
                idStatus.Add(new SelectListItem() { Text = "Deactive", Value = "N" });
                ViewBag.idStatusDropDown = idStatus;

                List<SelectListItem> ForBillType = new List<SelectListItem>();
                ForBillType.Add(new SelectListItem() { Text = "All", Value = "All" });
                ForBillType.Add(new SelectListItem() { Text = "Repurchase", Value = "Repurchase" });
                ForBillType.Add(new SelectListItem() { Text = "FirstBill", Value = "FirstBill" });
                ViewBag.ForBillTypeDropDown = ForBillType;
                OfferDetail.OfferFromDt = DateTime.Now;
                OfferDetail.OfferToDt = DateTime.Now;
                if (ActionName.ToLower() == "edit")
                {
                    if (!string.IsNullOrEmpty(OfferCode))
                    {
                        decimal OfferId = decimal.Parse(OfferCode);
                        OfferDetail = objTransacManager.GetSelectedOfferDetails(OfferId);
                    }
                }
                OfferDetail.Action = ActionName;
                OfferDetail.offerType = 3;
                List<PartyModel> objparty = objRegistrationManager.GetAllPartyList(false);
                objparty = objparty.Where(r => r.GroupId != 5).ToList();
                OfferDetail.OfferParty = objparty;
            }
            catch (Exception ex)
            {

            }
            return View(OfferDetail);

        }

        [SessionExpire]
        public ActionResult FreeProductOfferMaster()
        {
            if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "FreeProductOfferMaster"))
            {
                ViewBag.OfferType = 3;
                return View("OfferMaster");
            }
            else
                return RedirectToAction("Dashboard", "Home");
        }


        [SessionExpire]
        public ActionResult CEDInvoice()
        {
            DistributorBillModel objDistributorModel = new DistributorBillModel();
            try
            {
                objDistributorModel.objCustomer = new CustomerDetail();
                objDistributorModel.objProduct = new ProductModel();
                List<SelectListItem> objBankList = new List<SelectListItem>();
                var result = objTransacManager.GetBankList();
                objDistributorModel.objProduct.PayDetails = new PayDetails();
                foreach (var obj in result)
                {
                    if (obj.BankCode == 0)
                    {
                        objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString(), Selected = true });
                        objDistributorModel.objProduct.PayDetails.BDBankName = obj.BankCode.ToString();
                    }
                    else
                    {
                        objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString() });
                    }
                }
                ViewBag.BankNames = objBankList;
                List<SelectListItem> CardTypes = new List<SelectListItem>();
                CardTypes.Add(new SelectListItem { Text = "Credit Card", Value = "CC" });
                CardTypes.Add(new SelectListItem { Text = "Debit Card", Value = "DB" });
                ViewBag.CardTypes = CardTypes;

                objDistributorModel.objProduct.PayDetails.CardType = "CC";
                objDistributorModel.objCustomer.CustomerType = "New";
                List<SelectListItem> OfferSelectList = new List<SelectListItem>();
                OfferSelectList.Add(new SelectListItem { Text = "--Choose Offer--", Value = "0" });
                ViewBag.OfferList = OfferSelectList;

                var KitIdlist = objTransacManager.GetKitIdList();
                List<SelectListItem> KidIsListObj = new List<SelectListItem>();
                KidIsListObj.Add(new SelectListItem { Text = "--Select Kit--", Value = "0" });
                foreach (var obj in KitIdlist)
                {
                    KidIsListObj.Add(new SelectListItem { Text = obj.KitName, Value = obj.KitId.ToString() });
                }
                ViewBag.objKitList = KidIsListObj;
                objDistributorModel.objCustomer.KitId = 0;
                InventorySession.StoredDistributorValues = null;
                return View(objDistributorModel);

            }
            catch (Exception ex)
            {
                LogError(ex);
                Console.Write("Exception:", ex.Message);
            }
            return RedirectToAction("Dashboard", "Home"); ;
        }

        [SessionExpire]
        public ActionResult GetCEDDetail(string VoucherNo, string FormNo)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objTransacManager.GetCEDDetail(VoucherNo, FormNo);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }


        [SessionExpire]
        public ActionResult ShoppingJackpotInvoice()
        {

            DistributorBillModel objDistributorModel = new DistributorBillModel();
            try
            {

                if (Session["LoginUserType"] as string == "mobileshoppe" || CanUserAccessMenu((Session["LoginUser"] as User).UserId, "ShoppingJackpotInvoice"))
                {
                    objDistributorModel.objCustomer = new CustomerDetail();
                    objDistributorModel.objProduct = new ProductModel();
                    List<SelectListItem> objBankList = new List<SelectListItem>();
                    var result = objTransacManager.GetBankList();
                    objDistributorModel.objProduct.PayDetails = new PayDetails();

                    ViewBag.BankNames = objBankList;
                    List<SelectListItem> CardTypes = new List<SelectListItem>();
                    CardTypes.Add(new SelectListItem { Text = "Credit Card", Value = "CC" });
                    CardTypes.Add(new SelectListItem { Text = "Debit Card", Value = "DB" });
                    ViewBag.CardTypes = CardTypes;

                    objDistributorModel.objProduct.PayDetails.CardType = "CC";
                    objDistributorModel.objCustomer.CustomerType = "New";

                    objDistributorModel.objCustomer.KitId = 0;
                    return View(objDistributorModel);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                Console.Write("Exception:", ex.Message);
            }
            return RedirectToAction("Dashboard", "Home");
        }


        [HttpPost]
        public ActionResult GetSJPCustInfo(string IdNo)

        {
            CustomerDetail model = new CustomerDetail();
            model = objTransacManager.GetSJPCustInfo(IdNo);
            return Json(model, JsonRequestBehavior.AllowGet);

        }


        [SessionExpire]
        public ActionResult SJP_Scratch_Card_Invoice()
        {
            DistributorBillModel objDistributorModel = new DistributorBillModel();
            try
            {
                objDistributorModel.objCustomer = new CustomerDetail();
                objDistributorModel.objProduct = new ProductModel();
                List<SelectListItem> objBankList = new List<SelectListItem>();
                var result = objTransacManager.GetBankList();
                objDistributorModel.objProduct.PayDetails = new PayDetails();
                foreach (var obj in result)
                {
                    if (obj.BankCode == 0)
                    {
                        objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString(), Selected = true });
                        objDistributorModel.objProduct.PayDetails.BDBankName = obj.BankCode.ToString();
                    }
                    else
                    {
                        objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString() });
                    }
                }
                ViewBag.BankNames = objBankList;
                List<SelectListItem> CardTypes = new List<SelectListItem>();
                CardTypes.Add(new SelectListItem { Text = "Credit Card", Value = "CC" });
                CardTypes.Add(new SelectListItem { Text = "Debit Card", Value = "DB" });
                ViewBag.CardTypes = CardTypes;

                objDistributorModel.objProduct.PayDetails.CardType = "CC";
                objDistributorModel.objCustomer.CustomerType = "New";
                List<SelectListItem> OfferSelectList = new List<SelectListItem>();
                OfferSelectList.Add(new SelectListItem { Text = "--Choose Offer--", Value = "0" });
                ViewBag.OfferList = OfferSelectList;

                var KitIdlist = objTransacManager.GetKitIdList();
                List<SelectListItem> KidIsListObj = new List<SelectListItem>();
                KidIsListObj.Add(new SelectListItem { Text = "--Select Kit--", Value = "0" });
                foreach (var obj in KitIdlist)
                {
                    KidIsListObj.Add(new SelectListItem { Text = obj.KitName, Value = obj.KitId.ToString() });
                }
                ViewBag.objKitList = KidIsListObj;
                objDistributorModel.objCustomer.KitId = 0;
                InventorySession.StoredDistributorValues = null;
                return View(objDistributorModel);


            }
            catch (Exception ex)
            {
                LogError(ex);
                Console.Write("Exception:", ex.Message);
            }
            return RedirectToAction("Dashboard", "Home"); ;
        }

        [SessionExpire]
        public ActionResult GetScratchCardDetail(string VoucherNo)

        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objTransacManager.GetFreeScratchCardDetail(VoucherNo);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }
        [SessionExpire]
        public ActionResult UpdateRpPoint(string IsActionName, int ID)
        {
            RewardPoint objModel = new RewardPoint();
            string CurrentPartyCode = "";
            if (Session["LoginUser"] != null)
            {
                CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
            }
            var result = objTransacManager.GetSlab();
            List<SelectListItem> Slab = new List<SelectListItem>();
            foreach (var obj in result)
            {
                Slab.Add(new SelectListItem
                {
                    Text = obj.Slab.ToString(),
                    Value = obj.ID.ToString()
                });
            }
            if (IsActionName == "Add")
            {

                objModel = new RewardPoint();

            }
            else
            {
                if (!string.IsNullOrEmpty(ID.ToString()))
                {

                    objModel = objTransacManager.GetSlabPoint(ID);

                }

            }
            objModel.FromDate = DateTime.Now.Date;
            objModel.ToDate = DateTime.Now.Date;
            ViewBag.Slab = Slab;
            return View(objModel);
            //if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "AddWallet"))
            //    return View();
            //else
            //    return RedirectToAction("Dashboard", "Home");

        }
        public ActionResult SaveSlab(RewardPoint objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                if (objModel != null)
                {

                    objResponse = objTransacManager.SaveSlab(objModel);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllSlabPoint()
        {
            List<RewardPoint> objListModel = new List<RewardPoint>();
            objListModel = objTransacManager.GetAllSlab();
            var jsonPartyList = Json(objListModel, JsonRequestBehavior.AllowGet);
            jsonPartyList.MaxJsonLength = int.MaxValue;
            return jsonPartyList;
        }
        [SessionExpire]
        public ActionResult SlabMaster()
        {
            return View();
        }
        [SessionExpire]
        public ActionResult EInvoiceCredentail()
        {
            M_EInvoiceCredential obj = new M_EInvoiceCredential();
            return View(obj);
        }
        [HttpPost]
        public ActionResult GetEInvoiceCredentail()
        {
            List<M_EInvoiceCredential> objResponse = new List<M_EInvoiceCredential>();
            objResponse = objTransacManager.GetEInvoicecre();
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveEInvoiceCredentail(M_EInvoiceCredential Model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objTransacManager.SaveEInvoiceCredentail(Model);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CheckFpWallet(string Idno)
        {
            string Msg = string.Empty;
            string stscode = string.Empty;
            decimal amount = 0;
            try
            {
                FPVVoucher obj = new FPVVoucher();
                FPVoucherEligibilityResult fPVoucherEligibilityResult = objTransacManager.CheckFPVoucherEligibility(Idno);
                if (fPVoucherEligibilityResult.EligibilityStatus == "Eligible")
                {
                    obj = objTransacManager.GetCheckFpWallet(Idno);
                    stscode = "200";
                    amount = Convert.ToDecimal(obj.TotalBalance);
                }
                else
                {
                    stscode = "100";
                    Msg = fPVoucherEligibilityResult.Reason;
                }

            }
            catch (Exception ex)
            {
                stscode = "100";
                Msg = "Something went wrong";
            }
            return Json(new { Msg, stscode, amount }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetWalletTypeBalance(string Shoppe, string Vtype)
        {
            decimal WalletBalance = 0;
            try
            {
                WalletBalance = objTransacManager.GetPartyWalletBalance(Shoppe, Vtype);
            }
            catch (Exception ex)
            {

            }
            return Json(WalletBalance, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateBill()
        {

            var AccessTo = new UserController().UserCanAccess((Session["LoginUser"] as User).UserId, "UpdateBill");
            if (!string.IsNullOrEmpty(AccessTo))
            {
                ViewBag.UserCanAccess = AccessTo;
                return View();
            }
            else
                return RedirectToAction("Dashboard", "Home");
        }
        [HttpGet]
        public ActionResult CourierList()
        {
            List<Courier> objModel = new List<Courier>();
            objModel = objTransacManager.GetCouierList(0);
            return Json(objModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CourierDetailByweight(int CourierId, int Weight)
        {
            Courier objResult = new Entity.Common.Courier();
            objResult = objTransacManager.CourierDetailByweight(CourierId, Weight);
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBillData(string BillNo)
        {

            DistributorBillModel model = new DistributorBillModel();
            if (Session["LoginUser"] != null)
            {

                string CurrentPartyCode = (Session["LoginUser"] as User).PartyCode;
                model = objTransacManager.getInvoice(BillNo, CurrentPartyCode, "F");

            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveBillDetail(DistributorBillModel model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objTransacManager.SaveBillDetail(model);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }
        private class ConnModel
        {
            public int Id { get; internal set; }
        }

        [SessionExpire]
        public ActionResult Courier()
        {
            var AccessTo = new UserController().UserCanAccess((Session["LoginUser"] as User).UserId, "Courier");
            if (!string.IsNullOrEmpty(AccessTo))
            {
                ViewBag.UserCanAccess = AccessTo;
                return View();
            }
            else
                return RedirectToAction("Dashboard", "Home");
        }
        [SessionExpire]
        public ActionResult CourierMaster(string ActionName, string id)
        {
            Courier objModel = new Courier();

            List<SelectListItem> objActiveStatus = new List<SelectListItem>();
            objActiveStatus.Add(new SelectListItem
            {
                Text = "Yes",
                Value = "Y"
            });

            objActiveStatus.Add(new SelectListItem
            {
                Text = "No",
                Value = "N"
            });
            ViewBag.ActiveStatus = objActiveStatus;
            if (id != null)
            {
                List<Courier> objcourier = new List<Courier>();
                objcourier = objTransacManager.GetCouierList(Convert.ToInt32(id));
                objModel = objcourier.FirstOrDefault();
            }
            return View(objModel);
        }


        [SessionExpire]
        public ActionResult AddCourierDetail(decimal ID)
        {
            Courier obj = new Entity.Common.Courier();
            obj.ID = ID;

            var AccessTo = new UserController().UserCanAccess((Session["LoginUser"] as User).UserId, "Courier");
            if (!string.IsNullOrEmpty(AccessTo))
            {
                ViewBag.UserCanAccess = AccessTo;
                return View(obj);
            }
            else
                return RedirectToAction("Dashboard", "Home");
        }
    }
}