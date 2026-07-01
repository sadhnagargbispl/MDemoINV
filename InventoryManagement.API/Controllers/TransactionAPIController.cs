using InventoryManagement.API.Common;
using InventoryManagement.API.Models;
using InventoryManagement.Entity.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using static InventoryManagement.Entity.Common.EnumCalculation;

namespace InventoryManagement.API.Controllers
{
    public class TransactionAPIController : ApiController
    {
        private static string _numbers = "0123456789";
        Random random = new Random();
        //According to new schema Inventory DB
        //public ProductModel GetproductInfo(string SearchType,string data,bool isCForm,decimal CurrentStateCode,string CurrentPartyCode)
        //{
        //    ProductModel objProductModel = new ProductModel();
        //    bool searchByProductFlag = true;
        //    if(SearchType=="B")
        //    {
        //        searchByProductFlag = false;
        //    }

        //    try
        //    {
        //        if (!string.IsNullOrEmpty(data))
        //        {
        //            using (var entity = new InventoryEntities())
        //            {

        //                if (searchByProductFlag)
        //                {

        //                        objProductModel = (from product in entity.ProductMasters
        //                                      where product.ProductName.ToLower().Equals(data.ToLower()) && product.IsActive == true
        //                                           join barcode in entity.BarcodeMasters on product.ProductCode equals barcode.ProductCode
        //                                      join tax in entity.TaxMasters on product.ProductCode equals tax.ProductId
        //                                      select new ProductModel
        //                                      {
        //                                          ProductName = product.ProductName,
        //                                          Barcode = barcode.Barcode,
        //                                          BatchNo=barcode.BatchCode,
        //                                          DP = barcode.DP,
        //                                          RP = product.RP,
        //                                          DiscPer = product.DiscountPer,
        //                                          DiscAmt = product.DiscountInRs,
        //                                          ProdCode = (int)product.ProductCode,
        //                                          TaxPer = tax.VatTax,
        //                                          MRP=barcode.MRP,
        //                                          BV = product.BV,
        //                                          PV = product.PV,
        //                                          CV = product.CV,

        //                                           TaxType ="VAT",
        //                                           CommissionPer=product.ProductCommission
        //                                      }).FirstOrDefault();


        //                }
        //                else
        //                {
        //                    decimal? BarCodeData = decimal.Parse(data);


        //                        objProductModel = (from product in entity.ProductMasters
        //                                           where product.IsActive == true
        //                                           join barcode in entity.BarcodeMasters on product.ProductCode equals barcode.ProductCode
        //                                   where barcode.Barcode==BarCodeData
        //                                   join tax in entity.TaxMasters on product.ProductCode equals tax.ProductId
        //                                   select new ProductModel
        //                                   {
        //                                       ProductName = product.ProductName,
        //                                       Barcode = barcode.Barcode,
        //                                       BatchNo = barcode.BatchCode,
        //                                       DP = barcode.DP,
        //                                       RP = product.RP,
        //                                       DiscPer = product.DiscountPer,
        //                                       DiscAmt = product.DiscountInRs,
        //                                       ProdCode = (int)product.ProductCode,
        //                                       TaxPer = tax.VatTax,
        //                                       MRP = barcode.MRP,
        //                                       BV = product.BV,
        //                                       PV = product.PV,
        //                                       CV = product.CV,
        //                                       TaxType = "VAT",
        //                                       CommissionPer = product.ProductCommission
        //                                   }).FirstOrDefault();


        //                }
        //                objProductModel.StockAvailable = (from stockAvail in entity.Im_CurrentStock
        //                                                  where stockAvail.BatchCode == objProductModel.Barcode.ToString() && stockAvail.ProdId == objProductModel.ProdCode.ToString() && stockAvail.FCode.Equals(CurrentPartyCode)
        //                                                  select stockAvail.Qty
        //                                         ).Sum();
        //            }

        //        }
        //        //calculations
        //        //objProductModel = DoCalculation(objProductModel, Qty);
        //        object valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAdd.ToString());
        //        object valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAdd.ToString());
        //        int IsCommission = Convert.ToInt32(valueIsCommissonAdd);
        //        int IsDiscount = Convert.ToInt32(valueIsDiscountAdd);
        //        objProductModel.IsCommissionAdd = IsCommission;
        //        objProductModel.IsDiscountAdd = IsDiscount;

        //    }
        //    catch(Exception e)
        //    {

        //    }

        //    return objProductModel;
        //}

        //public CustomerDetail GetCustInfo(string IdNo)
        //{
        //    CustomerDetail objCustomerDetail = new CustomerDetail();
        //    if (!(string.IsNullOrEmpty(IdNo)))
        //    {
        //        try {
        //            SqlConnection SC = new SqlConnection("Data Source=144.217.216.195;Initial Catalog=SjLabs;Integrated Security=False;User Id=usrsjl;Password=S90$#usr02J;");



        //            string query = "select a.FormNo,a.MemFirstName+' '+ a.MemLastName as Name,a.IDno as IDno,a.Address1+','+a.Address2+','+a.City as Address,a.StateCode as StateCode,b.idno as RefId,b.MemFirstName+' '+ b.MemLastName as RefName FROM M_MemberMaster a,M_MemberMaster b WHERE a.RefFormNo=b.FormNo AND a.IDno=@IdNo";
        //            SqlCommand cmd = new SqlCommand();
        //            cmd.CommandText = query;
        //            cmd.Parameters.AddWithValue("@IdNo", IdNo);
        //            cmd.Connection = SC;
        //            SC.Close();
        //            SC.Open();
        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {

        //                    objCustomerDetail.IdNo = reader["IDno"] != null ? reader["IDno"].ToString() : "";
        //                    objCustomerDetail.ReferenceIdNo = reader["RefId"] != null ? reader["RefId"].ToString() : "";
        //                    objCustomerDetail.ReferenceName = reader["RefName"] != null ? reader["RefName"].ToString() : "";
        //                    objCustomerDetail.Name = reader["Name"] != null ? reader["Name"].ToString() : "";
        //                    objCustomerDetail.Address = reader["Address"] != null ? reader["Address"].ToString() : "";
        //                    objCustomerDetail.FormNo= reader["FormNo"] != null ? decimal.Parse(reader["FormNo"].ToString()) : 0;
        //                }
        //                else
        //                {
        //                    objCustomerDetail = null;
        //                }
        //            }
        //            SC.Close();
        //        }
        //        catch(Exception e)
        //        {

        //        }
        //      }

        //    return objCustomerDetail;
        //}

        //public List<string> GetAutocompleteProductNames()
        //{
        //    List<string> objProductNames = new List<string>();
        //    try
        //    {
        //        using(var entity=new InventoryEntities())
        //        {
        //            objProductNames = (from result in entity.ProductMasters
        //                               where result.IsActive == true
        //                               select result.ProductName).ToList();
        //        }
        //    }
        //    catch(Exception e){

        //    }

        //    return objProductNames;
        //}

        //public ResponseDetail SaveDistributorBill(DistributorBillModel objModel)
        //{
        //    ResponseDetail objResponse = new ResponseDetail();
        //    try
        //    {
        //        using (var entity = new InventoryEntities())
        //        {
        //            decimal maxSbillNo = (from result in entity.TrnBillMains select result.SBillNo).Max();
        //            maxSbillNo = maxSbillNo + 1;
        //            decimal? FsessId = (from result in entity.FiscalMasters where result.IsActive==true select result.FSessionId).FirstOrDefault();
        //            decimal? SessId = (from result in entity.M_SessnMaster select result.SessID).Max();
        //            SessId = SessId + 1;
        //            string billPrefix = (from result in entity.M_ConfigMaster select result.BillPrefix).FirstOrDefault();
        //            string version = (from result in entity.M_NewHOVersionInfo select result.VersionNo).FirstOrDefault();
        //            TrnPayModeDetail objDtPayModeDetail = new TrnPayModeDetail();
        //            List<string> Paymode=new List<string>();
        //            List<string> PayPrefix = new List<string>();
        //            List<TrnPayModeDetail> objDTListPayMode = new List<TrnPayModeDetail>();
        //            var resultPayMode = (from r in entity.M_PayModeMaster select r).ToList();
        //            //saving data in table
        //            // decimal? SessId=(from result in entity)


        //            if (objModel != null)
        //            {
        //                if (objModel.objProduct.PayDetails != null)
        //                {
        //                    if (objModel.objProduct.PayDetails.IsBD)
        //                    {
        //                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.BankDeposit;
        //                        string value = EnumPayModes.GetEnumDescription(enumVar);
        //                        PayPrefix.Add(value);
        //                        objDTListPayMode.Add(new TrnPayModeDetail {BillAmt= objModel.objProduct.TotalNetPayable,SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate=DateTime.Now,BillType="R",BillNo= billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value,Amount= objModel.objProduct.PayDetails.AmountByBD,BankCode=0,ChqDDDate=null,ChqDDNo="",CardNo="",Narration="",DUserId=0,DRecTimeStamp=null,BankName= objModel.objProduct.PayDetails.BDBankName, AcNo= objModel.objProduct.PayDetails.AccNo,IFSCode= objModel.objProduct.PayDetails.IFSCCode,ActiveStatus="Y", RecTimeStamp=DateTime.Now,UserId=objModel.objCustomer.UserDetails.UserId,Version= version, UserName=objModel.objCustomer.UserDetails.UserName, FSessId=FsessId??0,SBillNo=maxSbillNo});


        //                    }
        //                    if (objModel.objProduct.PayDetails.IsCC)
        //                    {
        //                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Card;
        //                        string value = EnumPayModes.GetEnumDescription(enumVar);
        //                        PayPrefix.Add(value);
        //                        objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now, BillType = "R", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value,AcNo="",IFSCode="",BankCode=0,Narration="",BankName="", DUserId=0,DRecTimeStamp=null,ChqDDNo="",ChqDDDate=null, Amount = objModel.objProduct.PayDetails.AmountByCard, CardNo = objModel.objProduct.PayDetails.CardNo, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
        //                    }
        //                    if (objModel.objProduct.PayDetails.IsQ)
        //                    {
        //                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cheque;
        //                        string value = EnumPayModes.GetEnumDescription(enumVar);
        //                        PayPrefix.Add(value);
        //                        objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now, BillType = "R", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByCheque,CardNo="", AcNo = "", IFSCode = "", BankCode = 0, Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.CHBankName,ChqDDNo = objModel.objProduct.PayDetails.ChequeNo,ChqDDDate=objModel.objProduct.PayDetails.ChequeDate, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
        //                    }
        //                    if (objModel.objProduct.PayDetails.IsD)
        //                    {
        //                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.DD;
        //                        string value = EnumPayModes.GetEnumDescription(enumVar);
        //                        PayPrefix.Add(value);
        //                        objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now, BillType = "R", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByDD, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.DDBankName,ChqDDNo = objModel.objProduct.PayDetails.DDNo, ChqDDDate = objModel.objProduct.PayDetails.DDDate, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
        //                    }
        //                    if (objModel.objProduct.PayDetails.IsT)
        //                    {
        //                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Credit;
        //                        string value = EnumPayModes.GetEnumDescription(enumVar);
        //                        PayPrefix.Add(value);
        //                        objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now, BillType = "R", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value,BankName="", Amount = objModel.objProduct.PayDetails.AmountByCredit, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0,DUserId = 0, DRecTimeStamp = null,ChqDDDate=null,ChqDDNo="", Narration =objModel.objProduct.PayDetails.Narration, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
        //                    }
        //                    if (objModel.objProduct.PayDetails.IsV)
        //                    {
        //                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Voucher;
        //                        string value = EnumPayModes.GetEnumDescription(enumVar);
        //                        PayPrefix.Add(value);
        //                        objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now, BillType = "R", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByVoucher, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, DUserId = 0, DRecTimeStamp = null, ChqDDDate = null, ChqDDNo = "", Narration ="",BankName="", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
        //                    }
        //                    if (objModel.objProduct.PayDetails.IsW)
        //                    {
        //                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Wallet;
        //                        string value = EnumPayModes.GetEnumDescription(enumVar);
        //                        PayPrefix.Add(value);
        //                        objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now, BillType = "R", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByWallet,BankCode=0, BankName = "", AcNo = "", IFSCode = "",Narration="",DUserId=0,DRecTimeStamp=null,ChqDDNo="",ChqDDDate=null,CardNo="", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
        //                    }
        //                    if (objModel.objProduct.CashAmount>0)
        //                    {
        //                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cash;
        //                        string value = EnumPayModes.GetEnumDescription(enumVar);
        //                        PayPrefix.Add(value);
        //                        objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.CashAmount, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now, BillType = "R", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.TotalNetPayable,BankCode=0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, CardNo = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
        //                    }
        //                    if (PayPrefix.Count > 0)
        //                    {

        //                        Paymode = (from r in entity.M_PayModeMaster where PayPrefix.Contains(r.Prefix) select r.PayMode).ToList();
        //                    }

        //                }


        //                foreach (var obj in objModel.objListProduct)
        //                {
        //                    TrnBillData objDTBillData = new TrnBillData();
        //                    objDTBillData.SBillNo = maxSbillNo;
        //                    objDTBillData.FSessId = FsessId ?? 0;
        //                    objDTBillData.SessId =SessId??0;
        //                    objDTBillData.ActiveStatus = "Y";
        //                    objDTBillData.BillDate = DateTime.Now;

        //                    objDTBillData.RefNo = "";
        //                    objDTBillData.RefId = "";
        //                    objDTBillData.RefName = "";
        //                    objDTBillData.Remarks = objModel.objCustomer.Remarks;
        //                    objDTBillData.CType = "M";
        //                    objDTBillData.SoldBy = objModel.objCustomer.UserDetails.PartyCode;
        //                    objDTBillData.BillBy = objDTBillData.SoldBy;
        //                    objDTBillData.BillNo = billPrefix+"/" + objDTBillData.BillBy + "/" + maxSbillNo;
        //                    objDTBillData.FType = "M";
        //                    objDTBillData.FCode = objModel.objCustomer.IdNo;
        //                    objDTBillData.PartyName = objModel.objCustomer.Name;
        //                    objDTBillData.SupplierId = 0;
        //                    objDTBillData.ChDDNo = 0;
        //                    objDTBillData.ChDate = DateTime.Now;
        //                    objDTBillData.ChAmt = 0;
        //                    objDTBillData.BankCode = 0;
        //                    objDTBillData.BankName = "";
        //                    objDTBillData.FormNo = objModel.objCustomer.FormNo;
        //                    objDTBillData.TotalTaxAmount = objModel.objProduct.TotalTaxAmount;
        //                    objDTBillData.TotalSTaxAmount = 0;
        //                    objDTBillData.TotalDiscount = objModel.objProduct.TotalDiscount;
        //                    objDTBillData.TotalKitBvValue = 0;
        //                    objDTBillData.TotalBvValue = objModel.objProduct.TotalBV;
        //                    objDTBillData.TotalCVValue = objModel.objProduct.TotalCV;
        //                    objDTBillData.TotalPVValue = objModel.objProduct.TotalPV;
        //                    objDTBillData.TotalRPValue = objModel.objProduct.TotalRP;

        //                    objDTBillData.DP = obj.DP??0;
        //                    objDTBillData.RP = obj.RP ?? 0;
        //                    objDTBillData.MRP = obj.MRP??0;
        //                    objDTBillData.CVValue = obj.CVValue ?? 0;
        //                    objDTBillData.CV = obj.CV ?? 0;
        //                    objDTBillData.PV = obj.PV ?? 0;
        //                    objDTBillData.BV = obj.BV ?? 0;
        //                    objDTBillData.BVValue = obj.BVValue ?? 0;
        //                    objDTBillData.PVValue = obj.PVValue ?? 0;
        //                    objDTBillData.RPValue = obj.RPValue ?? 0;
        //                    objDTBillData.Barcode = obj.Barcode.ToString();
        //                    objDTBillData.BatchNo = obj.BatchNo.ToString();
        //                    objDTBillData.TaxAmount = obj.TaxAmt??0;
        //                    objDTBillData.Tax = obj.TaxPer ?? 0;
        //                    objDTBillData.DiscountPer = obj.DiscPer ?? 0;
        //                    objDTBillData.Discount = obj.DiscAmt ?? 0;
        //                    objDTBillData.ProdCommssn = obj.CommissionPer ?? 0;
        //                    objDTBillData.ProdCommssnAmt = obj.CommissionAmt ?? 0;
        //                    objDTBillData.ProductId = obj.ProdCode.ToString();
        //                    objDTBillData.ProductName = obj.ProductName;
        //                    objDTBillData.Qty = obj.Quantity;
        //                    objDTBillData.Rate = obj.Rate??0;
        //                    objDTBillData.IsKitBV = "N";
        //                    objDTBillData.DSeries = "";
        //                    objDTBillData.DImported = "N";
        //                    objDTBillData.IMEINo = "";
        //                    objDTBillData.BNo = "";
        //                    objDTBillData.ItemType = "";



        //                    objDTBillData.JType = "Cash:" + objModel.objProduct.TotalNetPayable;
        //                    objDTBillData.BillTo = "R";
        //                    objDTBillData.BillFor = "RB";
        //                    objDTBillData.IsReceive = "R";
        //                    objDTBillData.IsCredit = "F";
        //                    objDTBillData.BillType = "R";
        //                    objDTBillData.ProdType = "P";
        //                    objDTBillData.PaymentDtl="Cash:" + objModel.objProduct.TotalNetPayable;
        //                    objDTBillData.TotalAmount = obj.TotalAmount;
        //                    objDTBillData.NetAmount = objModel.objProduct.TotalPayAmount;
        //                    objDTBillData.TaxType = obj.TaxType;
        //                    objDTBillData.CashDiscPer = obj.CashDiscPer;
        //                    objDTBillData.CashDiscAmount = obj.CashDiscAmount;
        //                    objDTBillData.NetPayable = objModel.objProduct.TotalNetPayable;
        //                    objDTBillData.RndOff = objModel.objProduct.Roundoff;
        //                    objDTBillData.CardAmount = 0;
        //                    objDTBillData.PayMode = Paymode.Count>1?string.Join(",",Paymode):Paymode[0];
        //                    objDTBillData.PayPrefix = PayPrefix.Count > 1 ? string.Join(",", PayPrefix) : PayPrefix[0];
        //                    objDTBillData.BvTransfer = "N";

        //                    objDTBillData.UserSBillNo = maxSbillNo;
        //                    objDTBillData.UserBillNo= billPrefix + "/" + objDTBillData.BillBy + "/" + maxSbillNo;

        //                    objDTBillData.DispatchStatus = "N";
        //                    objDTBillData.LR = "0";
        //                    objDTBillData.LRDate = DateTime.Now;
        //                    objDTBillData.TransporterName = "";
        //                    objDTBillData.DispatchTo = objModel.objCustomer.IdNo;
        //                    objDTBillData.FreightType = "";
        //                    objDTBillData.Series = "";
        //                    objDTBillData.Scratch = "";
        //                    objDTBillData.Unit = 0;
        //                    objDTBillData.PSessId = 0;
        //                    objDTBillData.DcNo = "";
        //                    objDTBillData.Imported = "N";
        //                    objDTBillData.FPoint=0;
        //                    objDTBillData.FPointValue = 0;
        //                    objDTBillData.OrdStatus = "";
        //                    objDTBillData.OrdQty=0;
        //                    objDTBillData.OrderType = "";
        //                    objDTBillData.OrderDate = DateTime.Now;
        //                    objDTBillData.OrderNo = "";
        //                    objDTBillData.RemQty = 0;
        //                    objDTBillData.DP1 = 0;
        //                    objDTBillData.DReason = "";
        //                    objDTBillData.DUserId = 0;
        //                    objDTBillData.DRecTimeStamp = DateTime.Now;
        //                    objDTBillData.DocWeight = 0;
        //                    objDTBillData.DocketNo = "";
        //                    objDTBillData.DocketDate = DateTime.Now;
        //                    objDTBillData.UserBillNo = "";
        //                    objDTBillData.UserSBillNo = 0;
        //                    objDTBillData.STNFormNo = "";
        //                    objDTBillData.StkRecv = "N";
        //                    objDTBillData.StkRecvDate = DateTime.Now;
        //                    objDTBillData.StkRecvUserId = 0;
        //                    objDTBillData.InTransit = "N";
        //                    objDTBillData.UID = "";
        //                    objDTBillData.OfferUID = 0;
        //                    objDTBillData.IsKit = "N";
        //                    objDTBillData.TotalCorton = "";
        //                    objDTBillData.TotalMonoCorton = "";
        //                    objDTBillData.SpclOfferId = 0;
        //                    objDTBillData.VAT = 0;
        //                    objDTBillData.BuyerAddress = "";
        //                    objDTBillData.BuyerTIN = "";

        //                    objDTBillData.TotalDiscount = objModel.objProduct.TotalDiscPer;
        //                    objDTBillData.TotalDiscountAmt = objModel.objProduct.TotalDiscount;
        //                    objDTBillData.VDiscountAmt = 0;
        //                    objDTBillData.VDiscount = 0;
        //                    objDTBillData.ReceiverID = "";
        //                    objDTBillData.ReceiverName = "";
        //                    objDTBillData.ReceiverMNo = "";
        //                    objDTBillData.ReceiverIDProof = "";
        //                    objDTBillData.TotalFPoint = 0;
        //                    objDTBillData.TotalQty = objModel.objProduct.TotalQty;
        //                    objDTBillData.CashReward = 0;
        //                    objDTBillData.CommssnAmt = objModel.objProduct.TotalCommsonAmt;
        //                    objDTBillData.RecvAmount = 0;
        //                    objDTBillData.ReturnToCustAmt = 0;
        //                    objDTBillData.ActiveStatus = "Y";
        //                    objDTBillData.RecTimeStamp = DateTime.Now;
        //                    objDTBillData.UserId = objModel.objCustomer.UserDetails.UserId;
        //                    objDTBillData.UserName= objModel.objCustomer.UserDetails.UserName;
        //                    objDTBillData.DelvPlace =string.IsNullOrEmpty(objModel.objProduct.DeliveryPlace)?"": objModel.objProduct.DeliveryPlace;
        //                    objDTBillData.DelvStatus = "";
        //                    objDTBillData.DelvUserId = 0;
        //                    objDTBillData.DelvRecTimeStamp = DateTime.Now;
        //                    objDTBillData.Version = version;
        //                    objDTBillData.IDType = "";
        //                    objDTBillData.BranchName = "";
        //                    objDTBillData.CourierId = 0;
        //                    objDTBillData.CourierName = "";
        //                    objDTBillData.LocId = 0;
        //                    objDTBillData.LocName = "";
        //                    objDTBillData.DelvAddress = "";
        //                    objDTBillData.Pincode = "";
        //                    objDTBillData.UnitName = "";
        //                    entity.TrnBillDatas.Add(objDTBillData);
        //                }
        //                try
        //                {
        //                    int i = entity.SaveChanges();
        //                    if (i == objModel.objListProduct.Count)
        //                    {

        //                        foreach(var obj in objDTListPayMode)
        //                        {
        //                            TrnPayModeDetail objTemp = new TrnPayModeDetail();
        //                            objTemp = obj;
        //                            objTemp.PayMode = (from r in resultPayMode where r.Prefix.Trim()==obj.PayPrefix.Trim() select r.PayMode).FirstOrDefault();
        //                            entity.TrnPayModeDetails.Add(objTemp);
        //                        }
        //                        i = 0;
        //                        i = entity.SaveChanges();
        //                        if (i == objDTListPayMode.Count)
        //                        {
        //                            objResponse.ResponseMessage = "Saved Successfully!";
        //                            objResponse.ResponseStatus = "OK";
        //                        }
        //                    }
        //                }
        //                catch (DbEntityValidationException e)
        //                {

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    return objResponse;
        //}

        //public List<BankModel> GetBankList()
        //{
        //    List<BankModel> objListBanks = new List<BankModel>();
        //    try
        //    {
        //        using(var entity=new InventoryEntities())
        //        {
        //            objListBanks = (from result in entity.M_BankMaster
        //                            where result.ActiveStatus == "Y"
        //                            select new BankModel
        //                            {
        //                                BankCode = (int)result.BankCode,
        //                                BankName = result.BankName,
        //                                ActiveStatus=result.ActiveStatus,
        //                                AccNo=result.AcNo,
        //                                Remarks=result.Remarks,
        //                                 IFSCCode=result.IFSCode,

        //                            }).ToList();
        //        }
        //    }
        //    catch(Exception e)
        //    {

        //    }
        //    return objListBanks;
        //}

        public List<ProductModel> GetproductInfo(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool allhalf, string Invoice, string IsSpclOffer)
        {
            List<ProductModel> objProductModel = new List<ProductModel>();
            List<ProductModel> TempResult = new List<ProductModel>();
            bool searchByProductFlag = true;
            if (SearchType == "B")
            {
                searchByProductFlag = false;
            }

            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    using (var entity = new InventoryEntities())
                    {

                        if (searchByProductFlag)
                        {

                            TempResult = (from product in entity.M_ProductMaster
                                          where product.ProductName.ToLower().Equals(data.ToLower()) && product.ActiveStatus == "Y" //&& product.IsCardIssue == "N"
                                          join barcode in entity.M_BarCodeMaster on product.ProdId equals barcode.ProdId
                                          where barcode.ActiveStatus == "Y"
                                          join BatchCode in entity.M_BatchMaster on product.ProdId equals BatchCode.ProdId
                                          where BatchCode.ActiveStatus == "Y"
                                          join tax in entity.M_TaxMaster on product.ProdId equals tax.ProdCode
                                          //where tax.StateCode == CurrentStateCode
                                          from c in entity.M_CatMaster
                                          where c.CatId == product.CatId
                                          select new ProductModel
                                          {
                                              ProductName = product.ProductName,
                                              CatId = product.CatId,
                                              CatName = c.CatName,
                                              Barcode = barcode.BarCode,
                                              BatchNo = BatchCode.BatchNo,
                                              DP = BatchCode.Dp,
                                              RP = product.RP,
                                              DiscPer = product.Discount,
                                              DiscAmt = product.DiscInRs,
                                              ProdCode = (int)product.ProductCode,
                                              ProductCodeStr = product.ProdId,
                                              TaxPer = tax.VatTax,
                                              ProdStateCode = tax.StateCode,
                                              MRP = BatchCode.Mrp,
                                              BV = product.BV,
                                              PV = product.PV,
                                              CV = product.CV,
                                              IsExpirable = BatchCode.IsExpired == "Y" ? true : false,
                                              ExpDate = BatchCode.ExpDate,
                                              TaxType = "GST",
                                              Rate = product.PurchaseRate,
                                              CommissionPer = product.ProdCommssn,
                                              SubCatId = product.SubCatId,
                                              IsAvailableForOffer = product.IsAvailableforOffers,
                                              IsAvailableForBilling = product.IsBillingAllowed,
                                              Weight = product.Weight,
                                              TotalDiscPer = product.SJDiscount ?? 0
                                          }).ToList();


                        }
                        else
                        {
                            //decimal? BarCodeData = decimal.Parse(data);
                            TempResult = (from product in entity.M_ProductMaster
                                          where product.ActiveStatus == "Y" && product.IsCardIssue == "N"
                                          join barcode in entity.M_BarCodeMaster on product.ProdId equals barcode.ProdId
                                          join BatchCode in entity.M_BatchMaster on product.ProdId equals BatchCode.ProdId
                                          where BatchCode.ActiveStatus == "Y" && BatchCode.BatchNo == data
                                          join tax in entity.M_TaxMaster on product.ProdId equals tax.ProdCode
                                          from c in entity.M_CatMaster
                                          where c.CatId == product.CatId
                                          select new ProductModel
                                          {
                                              ProductName = product.ProductName,
                                              CatId = product.CatId,
                                              CatName = c.CatName,
                                              Barcode = barcode.BarCode,
                                              BatchNo = BatchCode.BatchNo,
                                              DP = BatchCode.Dp,
                                              RP = product.RP,
                                              ProdStateCode = tax.StateCode,
                                              DiscPer = product.Discount,
                                              DiscAmt = product.DiscInRs,
                                              ProdCode = (int)product.ProductCode,
                                              ProductCodeStr = product.ProdId,
                                              TaxPer = tax.VatTax,
                                              MRP = BatchCode.Mrp,
                                              BV = product.BV,
                                              PV = product.PV,
                                              CV = product.CV,
                                              IsExpirable = BatchCode.IsExpired == "Y" ? true : false,
                                              ExpDate = BatchCode.ExpDate,
                                              TaxType = "GST",
                                              Rate = product.PurchaseRate,
                                              CommissionPer = product.ProdCommssn,
                                              IsAvailableForOffer = product.IsAvailableforOffers,
                                              TotalDiscPer = product.SJDiscount ?? 0
                                          }).ToList();
                        }
                        bool IsDistributorBill = false;
                        bool IsPartyBill = false;
                        bool IsCustomerBill = false;
                        bool IsPurchaseInvoice = false;
                        bool IsOrderCreation = false;
                        bool IsPendingOrder = false;
                        if (BillType == "distributor")
                        {
                            IsDistributorBill = true;
                        }
                        else
                        {
                            IsDistributorBill = false;
                        }
                        if (BillType == "party")
                        {
                            IsPartyBill = true;
                        }
                        else
                        {
                            IsPartyBill = false;
                        }
                        if (BillType == "customer")
                        {
                            IsCustomerBill = true;
                        }
                        else
                        {
                            IsCustomerBill = false;
                        }
                        if (BillType == "purchase")
                        {
                            IsPurchaseInvoice = true;
                        }
                        else
                        {
                            IsPurchaseInvoice = false;
                        }
                        if (BillType == "order")
                        {
                            IsOrderCreation = true;
                        }
                        else
                        {
                            IsOrderCreation = false;
                        }
                        if (BillType == "pendingorder")
                        {
                            IsPendingOrder = true;
                        }
                        else
                        {
                            IsPendingOrder = false;
                        }

                        foreach (var obj in TempResult)
                        {
                            ProductModel TempObj = new ProductModel();
                            if ((obj.IsExpirable && obj.ExpDate > DateTime.Now) || (obj.IsExpirable == false))
                            {
                                TempObj = obj;
                                object valueIsDiscountAdd = 0;
                                object valueIsCommissonAdd = 0;
                                if (IsDistributorBill || IsCustomerBill || IsPurchaseInvoice || IsOrderCreation || IsPendingOrder)
                                {
                                    valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAdd.ToString());
                                    valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAdd.ToString());

                                }
                                else
                                {
                                    valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAddOnPartyBill.ToString());
                                    valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAddOnPartyBill.ToString());

                                }
                                int IsCommission = Convert.ToInt32(valueIsCommissonAdd);
                                int IsDiscount = Convert.ToInt32(valueIsDiscountAdd);
                                TempObj.IsCommissionAdd = IsCommission;
                                TempObj.IsDiscountAdd = IsDiscount;
                                TempObj.StockAvailable = (from stockAvail in entity.Im_CurrentStock
                                                          where stockAvail.Barcode == TempObj.Barcode.ToString() && stockAvail.ProdId == TempObj.ProductCodeStr.ToString() && stockAvail.FCode.Equals(CurrentPartyCode)
                                                          && stockAvail.BatchCode == TempObj.BatchNo
                                                          select stockAvail.Qty
                                                     ).DefaultIfEmpty(0).Sum();
                                TempObj.DP1 = TempObj.DP;
                                if (IsCustomerBill)
                                {
                                    TempObj.DP = obj.MRP;

                                }
                                else
                                {
                                    if (!IsPurchaseInvoice && IsBillOnMrp)
                                    {
                                        TempObj.DP = obj.MRP;
                                    }
                                }
                                CurrentStateCode = (int)(from r in entity.M_CompanyMaster select r.CompState).FirstOrDefault();
                                if (allhalf)
                                {
                                    TempObj.DP = TempObj.DP / 2;
                                    TempObj.BV = TempObj.BV / 2;
                                    TempObj.PV = TempObj.PV / 2;
                                    TempObj.RP = TempObj.RP / 2;
                                    TempObj.DiscAmt = TempObj.DP;
                                    TempObj.IsDiscountAdd = 1;
                                }
                                if (!string.IsNullOrEmpty(Invoice) && Invoice == "CED")
                                {
                                    var oridp = TempObj.DP;
                                    TempObj.DP = (TempObj.DP * 1) / 4;
                                    TempObj.BV = 0;
                                    TempObj.PV = (TempObj.PV * 1) / 4;
                                    TempObj.RP = (TempObj.RP * 1) / 4;
                                    TempObj.DiscAmt = oridp - TempObj.DP;
                                    TempObj.IsDiscountAdd = 1;
                                }
                                if (!string.IsNullOrEmpty(OfferID))
                                {
                                    decimal iOfferID = Convert.ToDecimal(OfferID);
                                    if (iOfferID != 0)
                                    {
                                        TempObj.offerDetail = GetOfferDetail(iOfferID, obj.ProductCodeStr, IsSpclOffer);
                                        if (!string.IsNullOrEmpty(TempObj.offerDetail.offerType))
                                        {
                                            decimal offerType = Convert.ToDecimal(TempObj.offerDetail.offerType);
                                            if (offerType == 2 || offerType == 3)
                                            {
                                                TempObj.DP = TempObj.offerDetail.OfferMrp / 1;// TempObj.offerDetail.FreeQty;
                                                TempObj.OfferProdQty = TempObj.offerDetail.FreeQty;
                                                TempObj.OfferProdQty = 1;
                                                TempObj.BV = 0;
                                            }
                                        }
                                    }
                                }
                                objProductModel.Add(TempObj);
                            }
                        }
                        if (objProductModel.Count > 1 && !IsPurchaseInvoice)
                        {
                            objProductModel = objProductModel.Where(m => m.StockAvailable > 0).OrderBy(m => m.ExpDate).ThenBy(m => m.StockAvailable).ToList();
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return objProductModel;
        }


        public List<ProductModel> GetproductInfoBatchWise(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool allhalf, string Invoice, string IsSpclOffer)
        {
            List<ProductModel> objProductModel = new List<ProductModel>();
            List<ProductModel> TempResult = new List<ProductModel>();
            bool searchByProductFlag = true;
            if (SearchType == "B")
            {
                searchByProductFlag = false;
            }

            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    using (var entity = new InventoryEntities())
                    {

                        if (searchByProductFlag)
                        {

                            TempResult = (from product in entity.M_ProductMaster
                                          where product.ProductName.ToLower().Equals(data.ToLower()) && product.ActiveStatus == "Y" //&& product.IsCardIssue == "N"
                                          join barcode in entity.M_BarCodeMaster on product.ProdId equals barcode.ProdId
                                          where barcode.ActiveStatus == "Y"
                                          //join BatchCode in entity.M_BatchMaster on product.ProdId equals BatchCode.ProdId
                                          //where BatchCode.ActiveStatus == "Y"
                                          join tax in entity.M_TaxMaster on product.ProdId equals tax.ProdCode
                                          //where tax.StateCode == CurrentStateCode
                                          from c in entity.M_CatMaster
                                          where c.CatId == product.CatId
                                          select new ProductModel
                                          {
                                              ProductName = product.ProductName,
                                              CatId = product.CatId,
                                              CatName = c.CatName,
                                              Barcode = barcode.BarCode,
                                              // BatchNo = barcode.BatchNo,
                                              //DP = product.DP,
                                              RP = product.RP,
                                              DiscPer = product.Discount,
                                              DiscAmt = product.DiscInRs,
                                              ProdCode = (int)product.ProductCode,
                                              ProductCodeStr = product.ProdId,
                                              TaxPer = tax.VatTax,
                                              ProdStateCode = tax.StateCode,
                                              //MRP = product .MRP,
                                              BV = product.BV,
                                              PV = product.PV,
                                              CV = product.CV,
                                              IsExpirable = product.IsExpired == "Y" ? true : false,
                                              ExpDate = product.ExpDate,
                                              TaxType = "GST",
                                              Rate = product.PurchaseRate,
                                              CommissionPer = product.ProdCommssn,
                                              SubCatId = product.SubCatId,
                                              IsAvailableForOffer = product.IsAvailableforOffers,
                                              IsAvailableForBilling = product.IsBillingAllowed,
                                              Weight = product.Weight,
                                              TotalDiscPer = product.SJDiscount ?? 0
                                          }).ToList();


                        }
                        else
                        {
                            //decimal? BarCodeData = decimal.Parse(data);
                            TempResult = (from product in entity.M_ProductMaster
                                          where product.ActiveStatus == "Y" && product.IsCardIssue == "N"
                                          join barcode in entity.M_BarCodeMaster on product.ProdId equals barcode.ProdId
                                          //join BatchCode in entity.M_BatchMaster on product.ProdId equals BatchCode.ProdId
                                          //where BatchCode.ActiveStatus == "Y"
                                          where barcode.BarCode == data
                                          join tax in entity.M_TaxMaster on product.ProdId equals tax.ProdCode
                                          from c in entity.M_CatMaster
                                          where c.CatId == product.CatId
                                          select new ProductModel
                                          {
                                              ProductName = product.ProductName,
                                              CatId = product.CatId,
                                              CatName = c.CatName,
                                              Barcode = barcode.BarCode,
                                              //BatchNo = barcode.BatchNo,
                                              //DP = product.DP,
                                              RP = product.RP,
                                              ProdStateCode = tax.StateCode,
                                              DiscPer = product.Discount,
                                              DiscAmt = product.DiscInRs,
                                              ProdCode = (int)product.ProductCode,
                                              ProductCodeStr = product.ProdId,
                                              TaxPer = tax.VatTax,
                                              //MRP = product.MRP,
                                              BV = product.BV,
                                              PV = product.PV,
                                              CV = product.CV,
                                              IsExpirable = product.IsExpired == "Y" ? true : false,
                                              ExpDate = product.ExpDate,
                                              TaxType = "GST",
                                              Rate = product.PurchaseRate,
                                              CommissionPer = product.ProdCommssn,
                                              IsAvailableForOffer = product.IsAvailableforOffers,
                                              TotalDiscPer = product.SJDiscount ?? 0
                                          }).ToList();
                        }

                        foreach (var obj in TempResult)
                        {
                            if (allhalf)
                            {

                                obj.BV = obj.BV / 2;
                                obj.PV = obj.PV / 2;
                                obj.RP = obj.RP / 2;
                                obj.DiscAmt = obj.DP;
                                obj.IsDiscountAdd = 1;
                            }
                            if (!string.IsNullOrEmpty(Invoice) && Invoice == "CED")
                            {
                                obj.BV = 0;
                                obj.PV = (obj.PV * 1) / 4;
                                obj.DiscAmt = obj.DP;
                                obj.IsDiscountAdd = 1;
                            }

                            objProductModel.Add(obj);
                        }

                        PopulateBatchDetails(objProductModel, entity, BillType, CurrentStateCode, CurrentPartyCode, IsBillOnMrp, OfferID, allhalf, Invoice, IsSpclOffer);


                    }
                }
            }
            catch (Exception e)
            {

            }
            return objProductModel;
        }
        private void PopulateBatchDetails(List<ProductModel> products, InventoryEntities entity, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool allhalf, string Invoice, string IsSpclOffer)
        {
            bool IsDistributorBill = false;
            bool IsPartyBill = false;
            bool IsCustomerBill = false;
            bool IsPurchaseInvoice = false;
            bool IsOrderCreation = false;
            bool IsPendingOrder = false;
            if (BillType == "distributor")
            {
                IsDistributorBill = true;
            }
            else
            {
                IsDistributorBill = false;
            }
            if (BillType == "party")
            {
                IsPartyBill = true;
            }
            else
            {
                IsPartyBill = false;
            }
            if (BillType == "customer")
            {
                IsCustomerBill = true;
            }
            else
            {
                IsCustomerBill = false;
            }
            if (BillType == "purchase")
            {
                IsPurchaseInvoice = true;
            }
            else
            {
                IsPurchaseInvoice = false;
            }
            if (BillType == "order")
            {
                IsOrderCreation = true;
            }
            else
            {
                IsOrderCreation = false;
            }
            if (BillType == "pendingorder")
            {
                IsPendingOrder = true;
            }
            else
            {
                IsPendingOrder = false;
            }


            foreach (var product in products)
            {
                // Fetch batch details for each product
                var batchDetails = (from batch in entity.M_BatchMaster
                                    where batch.ProdId == product.ProductCodeStr // or however you identify the product
                                                                                 && batch.ActiveStatus == "Y" // ensuring the batch is active
                                    select new ProductBatchModel
                                    {
                                        ProductName = product.ProductName,
                                        ProdCode = product.ProdCode,
                                        ProductCodeStr = product.ProductCodeStr,
                                        ProdId = batch.ProdId,
                                        BatchNo = batch.BatchNo,
                                        Batchcode = batch.BatchNo, // or any other unique code you have
                                        Barcode = product.Barcode,
                                        IsExpirable = batch.IsExpired == "Y",
                                        ExpDate = batch.ExpDate,
                                        MRP = batch.Mrp, // ensure you have MRP field in your batch model
                                        DP = batch.Dp, // ensuring you have DP field in your batch model
                                        BV = batch.Bv,
                                        PV = batch.PV,
                                        CV = product.CV,
                                        RP = product.RP,
                                        DiscPer = product.DiscPer,
                                        DiscAmt = product.DiscAmt,
                                        TaxType = "GST",
                                        Rate = product.Rate,
                                        Costing = batch.Costing,
                                        Weight = product.Weight,
                                        CommissionPer = product.CommissionPer,
                                        IsAvailableForOffer = product.IsAvailableForOffer,
                                        TotalDiscPer = product.TotalDiscPer,
                                        TaxPer = product.TaxPer,
                                        StockAvailable = (from stockAvail in entity.Im_CurrentStock
                                                          where stockAvail.ProdId == product.ProductCodeStr.ToString() &&
                                                                stockAvail.FCode.Equals(CurrentPartyCode)
                                                                && stockAvail.BatchCode == batch.BatchNo
                                                          select stockAvail.Qty
                                                         ).DefaultIfEmpty(0).Sum()
                                    }).ToList();

                // Initialize a new list to store valid batches
                List<ProductBatchModel> validBatchDetails = new List<ProductBatchModel>();

                foreach (var obj in batchDetails)
                {
                    if (obj.StockAvailable > 0)
                    {
                        if ((obj.IsExpirable && obj.ExpDate > DateTime.Now) || !obj.IsExpirable)
                        {
                            // Logic to process TempObj
                            ProductBatchModel TempObj = obj;  // Process obj directly without re-adding it
                            object valueIsDiscountAdd = 0;
                            object valueIsCommissonAdd = 0;

                            if (IsDistributorBill || IsCustomerBill || IsPurchaseInvoice || IsOrderCreation || IsPendingOrder)
                            {
                                valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAdd.ToString());
                                valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAdd.ToString());
                            }
                            else
                            {
                                valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAddOnPartyBill.ToString());
                                valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAddOnPartyBill.ToString());
                            }

                            int IsCommission = Convert.ToInt32(valueIsCommissonAdd);
                            int IsDiscount = Convert.ToInt32(valueIsDiscountAdd);
                            product.IsCommissionAdd = IsCommission;
                            product.IsDiscountAdd = IsDiscount;

                            product.DP1 = TempObj.DP;

                            if (IsCustomerBill)
                            {
                                //TempObj.DP = obj.MRP;
                                TempObj.DP = obj.DP; // change 27-06-2025 because client requirement selection DP or MRP 
                            }
                            else
                            {
                                if (!IsPurchaseInvoice && IsBillOnMrp)
                                {
                                    TempObj.DP = obj.MRP;
                                }
                            }
                            if (IsPurchaseInvoice == true)
                            {

                                TempObj.Rate = obj.Costing;
                            }

                            if (allhalf)
                            {
                                TempObj.DP = TempObj.DP / 2;
                                TempObj.RP = TempObj.RP / 2;
                            }

                            if (!string.IsNullOrEmpty(Invoice) && Invoice == "CED")
                            {
                                TempObj.DP = (TempObj.DP * 1) / 4;
                                TempObj.RP = (TempObj.RP * 1) / 4;
                            }

                            if (!string.IsNullOrEmpty(OfferID))
                            {
                                decimal iOfferID = Convert.ToDecimal(OfferID);
                                if (iOfferID != 0)
                                {
                                    product.offerDetail = GetOfferDetail(iOfferID, product.ProductCodeStr, IsSpclOffer);
                                    if (!string.IsNullOrEmpty(product.offerDetail.offerType))
                                    {
                                        decimal offerType = Convert.ToDecimal(product.offerDetail.offerType);
                                        if (offerType == 2 || offerType == 3)
                                        {
                                            TempObj.DP = product.offerDetail.OfferMrp / 1;
                                            product.OfferProdQty = product.offerDetail.FreeQty;
                                            product.OfferProdQty = 1;
                                            product.BV = 0;
                                        }
                                    }
                                }
                            }

                            // Add processed TempObj to the valid batch list
                            validBatchDetails.Add(TempObj);
                        }
                    }
                }

                // Assign the filtered batch list to the product
                product.batchdetail = validBatchDetails;
            }

            // Optionally, filter and sort products by stock and expiration date
            if (products.Count > 1 && !IsPurchaseInvoice)
            {
                products = products.Where(m => m.StockAvailable > 0)
                                   .OrderBy(m => m.ExpDate)
                                   .ThenBy(m => m.StockAvailable)
                                   .ToList();
            }


        }


        public List<ProductModel> GetAllproductInfoBatchWise(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool allhalf, string Invoice, string IsSpclOffer)
        {
            List<ProductModel> objProductModel = new List<ProductModel>();
            List<ProductModel> TempResult = new List<ProductModel>();
            bool searchByProductFlag = true;
            if (SearchType == "B")
            {
                searchByProductFlag = false;
            }

            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    using (var entity = new InventoryEntities())
                    {

                        if (searchByProductFlag)
                        {

                            TempResult = (from product in entity.M_ProductMaster
                                          where product.ProductName.ToLower().Equals(data.ToLower()) && product.ActiveStatus == "Y" //&& product.IsCardIssue == "N"
                                          join barcode in entity.M_BarCodeMaster on product.ProdId equals barcode.ProdId
                                          where barcode.ActiveStatus == "Y"
                                          //join BatchCode in entity.M_BatchMaster on product.ProdId equals BatchCode.ProdId
                                          //where BatchCode.ActiveStatus == "Y"
                                          join tax in entity.M_TaxMaster on product.ProdId equals tax.ProdCode
                                          //where tax.StateCode == CurrentStateCode
                                          from c in entity.M_CatMaster
                                          where c.CatId == product.CatId
                                          select new ProductModel
                                          {
                                              ProductName = product.ProductName,
                                              CatId = product.CatId,
                                              CatName = c.CatName,
                                              Barcode = barcode.BarCode,
                                              // BatchNo = barcode.BatchNo,
                                              //DP = product.DP,
                                              RP = product.RP,
                                              DiscPer = product.Discount,
                                              DiscAmt = product.DiscInRs,
                                              ProdCode = (int)product.ProductCode,
                                              ProductCodeStr = product.ProdId,
                                              TaxPer = tax.VatTax,
                                              ProdStateCode = tax.StateCode,
                                              //MRP = product .MRP,
                                              BV = product.BV,
                                              PV = product.PV,
                                              CV = product.CV,
                                              IsExpirable = product.IsExpired == "Y" ? true : false,
                                              ExpDate = product.ExpDate,
                                              TaxType = "GST",
                                              Rate = product.PurchaseRate,
                                              CommissionPer = product.ProdCommssn,
                                              SubCatId = product.SubCatId,
                                              IsAvailableForOffer = product.IsAvailableforOffers,
                                              IsAvailableForBilling = product.IsBillingAllowed,
                                              Weight = product.Weight,
                                              TotalDiscPer = product.SJDiscount ?? 0
                                          }).ToList();


                        }
                        else
                        {
                            //decimal? BarCodeData = decimal.Parse(data);
                            TempResult = (from product in entity.M_ProductMaster
                                          where product.ActiveStatus == "Y" && product.IsCardIssue == "N"
                                          join barcode in entity.M_BarCodeMaster on product.ProdId equals barcode.ProdId
                                          //join BatchCode in entity.M_BatchMaster on product.ProdId equals BatchCode.ProdId
                                          //where BatchCode.ActiveStatus == "Y"
                                          where barcode.BarCode == data
                                          join tax in entity.M_TaxMaster on product.ProdId equals tax.ProdCode
                                          from c in entity.M_CatMaster
                                          where c.CatId == product.CatId
                                          select new ProductModel
                                          {
                                              ProductName = product.ProductName,
                                              CatId = product.CatId,
                                              CatName = c.CatName,
                                              Barcode = barcode.BarCode,
                                              //BatchNo = barcode.BatchNo,
                                              //DP = product.DP,
                                              RP = product.RP,
                                              ProdStateCode = tax.StateCode,
                                              DiscPer = product.Discount,
                                              DiscAmt = product.DiscInRs,
                                              ProdCode = (int)product.ProductCode,
                                              ProductCodeStr = product.ProdId,
                                              TaxPer = tax.VatTax,
                                              //MRP = product.MRP,
                                              BV = product.BV,
                                              PV = product.PV,
                                              CV = product.CV,
                                              IsExpirable = product.IsExpired == "Y" ? true : false,
                                              ExpDate = product.ExpDate,
                                              TaxType = "GST",
                                              Rate = product.PurchaseRate,
                                              CommissionPer = product.ProdCommssn,
                                              IsAvailableForOffer = product.IsAvailableforOffers,
                                              TotalDiscPer = product.SJDiscount ?? 0
                                          }).ToList();
                        }

                        foreach (var obj in TempResult)
                        {
                            if (allhalf)
                            {

                                obj.BV = obj.BV / 2;
                                obj.PV = obj.PV / 2;
                                obj.RP = obj.RP / 2;
                                obj.DiscAmt = obj.DP;
                                obj.IsDiscountAdd = 1;
                            }
                            if (!string.IsNullOrEmpty(Invoice) && Invoice == "CED")
                            {
                                obj.BV = 0;
                                obj.PV = (obj.PV * 1) / 4;
                                obj.DiscAmt = obj.DP;
                                obj.IsDiscountAdd = 1;
                            }

                            objProductModel.Add(obj);
                        }

                        PopulateAllBatchDetails(objProductModel, entity, BillType, CurrentStateCode, CurrentPartyCode, IsBillOnMrp, OfferID, allhalf, Invoice, IsSpclOffer);


                    }
                }
            }
            catch (Exception e)
            {

            }
            return objProductModel;
        }
        private void PopulateAllBatchDetails(List<ProductModel> products, InventoryEntities entity, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool allhalf, string Invoice, string IsSpclOffer)
        {
            bool IsDistributorBill = false;
            bool IsPartyBill = false;
            bool IsCustomerBill = false;
            bool IsPurchaseInvoice = false;
            bool IsOrderCreation = false;
            bool IsPendingOrder = false;
            if (BillType == "distributor")
            {
                IsDistributorBill = true;
            }
            else
            {
                IsDistributorBill = false;
            }
            if (BillType == "party")
            {
                IsPartyBill = true;
            }
            else
            {
                IsPartyBill = false;
            }
            if (BillType == "customer")
            {
                IsCustomerBill = true;
            }
            else
            {
                IsCustomerBill = false;
            }
            if (BillType == "purchase")
            {
                IsPurchaseInvoice = true;
            }
            else
            {
                IsPurchaseInvoice = false;
            }
            if (BillType == "order")
            {
                IsOrderCreation = true;
            }
            else
            {
                IsOrderCreation = false;
            }
            if (BillType == "pendingorder")
            {
                IsPendingOrder = true;
            }
            else
            {
                IsPendingOrder = false;
            }


            foreach (var product in products)
            {
                // Fetch batch details for each product
                var batchDetails = (from batch in entity.M_BatchMaster
                                    where batch.ProdId == product.ProductCodeStr // or however you identify the product
                                                                                 && batch.ActiveStatus == "Y" // ensuring the batch is active
                                    select new ProductBatchModel
                                    {
                                        ProductName = product.ProductName,
                                        ProdCode = product.ProdCode,
                                        ProductCodeStr = product.ProductCodeStr,
                                        ProdId = batch.ProdId,
                                        BatchNo = batch.BatchNo,
                                        Batchcode = batch.BatchNo, // or any other unique code you have
                                        Barcode = product.Barcode,
                                        IsExpirable = batch.IsExpired == "Y",
                                        ExpDate = batch.ExpDate,
                                        MRP = batch.Mrp, // ensure you have MRP field in your batch model
                                        DP = batch.Dp, // ensuring you have DP field in your batch model
                                        BV = batch.Bv,
                                        PV = batch.PV,
                                        CV = product.CV,
                                        RP = product.RP,
                                        DiscPer = product.DiscPer,
                                        DiscAmt = product.DiscAmt,
                                        TaxType = "GST",
                                        Rate = product.Rate,
                                        Costing = batch.Costing,
                                        Weight = product.Weight,
                                        CommissionPer = product.CommissionPer,
                                        IsAvailableForOffer = product.IsAvailableForOffer,
                                        TotalDiscPer = product.TotalDiscPer,
                                        TaxPer = product.TaxPer,
                                        StockAvailable = (from stockAvail in entity.Im_CurrentStock
                                                          where stockAvail.ProdId == product.ProductCodeStr.ToString() &&
                                                                stockAvail.FCode.Equals(CurrentPartyCode)
                                                                && stockAvail.BatchCode == batch.BatchNo
                                                          select stockAvail.Qty
                                                         ).DefaultIfEmpty(0).Sum()
                                    }).ToList();

                // Initialize a new list to store valid batches
                List<ProductBatchModel> validBatchDetails = new List<ProductBatchModel>();

                foreach (var obj in batchDetails)
                {

                    if ((obj.IsExpirable && obj.ExpDate > DateTime.Now) || !obj.IsExpirable)
                    {
                        // Logic to process TempObj
                        ProductBatchModel TempObj = obj;  // Process obj directly without re-adding it
                        object valueIsDiscountAdd = 0;
                        object valueIsCommissonAdd = 0;

                        if (IsDistributorBill || IsCustomerBill || IsPurchaseInvoice || IsOrderCreation || IsPendingOrder)
                        {
                            valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAdd.ToString());
                            valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAdd.ToString());
                        }
                        else
                        {
                            valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAddOnPartyBill.ToString());
                            valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAddOnPartyBill.ToString());
                        }

                        int IsCommission = Convert.ToInt32(valueIsCommissonAdd);
                        int IsDiscount = Convert.ToInt32(valueIsDiscountAdd);
                        product.IsCommissionAdd = IsCommission;
                        product.IsDiscountAdd = IsDiscount;

                        product.DP1 = TempObj.DP;

                        if (IsCustomerBill)
                        {
                            //TempObj.DP = obj.MRP;
                            TempObj.DP = obj.DP; // change 27-06-2025 because client requirement selection DP or MRP 
                        }
                        else
                        {
                            if (!IsPurchaseInvoice && IsBillOnMrp)
                            {
                                TempObj.DP = obj.MRP;
                            }
                        }
                        if (IsPurchaseInvoice == true)
                        {

                            TempObj.Rate = obj.Costing;
                        }

                        if (allhalf)
                        {
                            TempObj.DP = TempObj.DP / 2;
                            TempObj.RP = TempObj.RP / 2;
                        }

                        if (!string.IsNullOrEmpty(Invoice) && Invoice == "CED")
                        {
                            TempObj.DP = (TempObj.DP * 1) / 4;
                            TempObj.RP = (TempObj.RP * 1) / 4;
                        }

                        if (!string.IsNullOrEmpty(OfferID))
                        {
                            decimal iOfferID = Convert.ToDecimal(OfferID);
                            if (iOfferID != 0)
                            {
                                product.offerDetail = GetOfferDetail(iOfferID, product.ProductCodeStr, IsSpclOffer);
                                if (!string.IsNullOrEmpty(product.offerDetail.offerType))
                                {
                                    decimal offerType = Convert.ToDecimal(product.offerDetail.offerType);
                                    if (offerType == 2 || offerType == 3)
                                    {
                                        TempObj.DP = product.offerDetail.OfferMrp / 1;
                                        product.OfferProdQty = product.offerDetail.FreeQty;
                                        product.OfferProdQty = 1;
                                        product.BV = 0;
                                    }
                                }
                            }
                        }

                        // Add processed TempObj to the valid batch list
                        validBatchDetails.Add(TempObj);
                    }

                }

                // Assign the filtered batch list to the product
                product.batchdetail = validBatchDetails;
            }

            // Optionally, filter and sort products by stock and expiration date
            if (products.Count > 1 && !IsPurchaseInvoice)
            {
                products = products.Where(m => m.StockAvailable > 0)
                                   .OrderBy(m => m.ExpDate)
                                   .ThenBy(m => m.StockAvailable)
                                   .ToList();
            }


        }

        public FPVVoucher CheckFpVoucher(string Code, string Idno)
        {
            FPVVoucher fPVoucher = new FPVVoucher();
            try
            {
                using (var entity = new InventoryEntities())
                {

                    fPVoucher = (from product in entity.FPVouchers
                                 where product.Code == Code && product.IdNo == Idno
                                 select new FPVVoucher
                                 {
                                     Id = product.Id,
                                     Code = product.Code,
                                     IdNo = product.IdNo,
                                     Isuse = product.Isuse,
                                     Amount = product.Amount,
                                 }).ToList().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return fPVoucher;
        }
        //public Coupons CheckCoupon(string Code, string Idno)
        //{
        //    Coupons obj = new Coupons();
        //    try
        //    {
        //        using (var entity = new InventoryEntities())
        //        {

        //            obj = (from product in entity.Coupons
        //                   where product.Code == Code && product.IdNo == Idno
        //                   select new Coupons
        //                   {
        //                       Id = product.Id,
        //                       Code = product.Code,
        //                       IdNo = product.IdNo,
        //                       Isuse = product.Isuse,
        //                       Amount = product.Amount,
        //                   }).ToList().FirstOrDefault();
        //        }
        //            if (obj ==null)
        //            {
        //                obj.Amount = 0;

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return obj;
        //}
        public Coupons CheckCoupon(string code, string idNo)
        {
            try
            {
                using (var entity = new InventoryEntities())
                {
                    var result = (from product in entity.Coupons
                                  where product.Code == code && product.IdNo == idNo
                                  select new Coupons
                                  {
                                      Id = product.Id,
                                      Code = product.Code,
                                      IdNo = product.IdNo,
                                      Isuse = product.Isuse,
                                      Amount = product.Amount,
                                  }).FirstOrDefault();

                    if (result == null)
                    {
                        Console.WriteLine("No matching coupon found. Returning null.");
                        return null; // Explicitly return null if no match
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking coupon: {ex.Message}");
                return null;
            }
        }
        public List<ProductModel> GetAddStockGVproductInfo(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool allhalf, string Invoice, string IsSpclOffer)
        {
            List<ProductModel> objProductModel = new List<ProductModel>();
            List<ProductModel> TempResult = new List<ProductModel>();
            bool searchByProductFlag = true;
            if (SearchType == "B")
            {
                searchByProductFlag = false;
            }

            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    using (var entity = new InventoryEntities())
                    {

                        if (searchByProductFlag)
                        {

                            TempResult = (from product in entity.M_ProductMaster
                                          where product.ProductName.ToLower().Equals(data.ToLower()) && product.ActiveStatus == "Y" //&& product.IsCardIssue == "N"
                                          join barcode in entity.M_BarCodeMaster on product.ProdId equals barcode.ProdId
                                          where barcode.ActiveStatus == "Y"
                                          join BatchCode in entity.M_BatchMaster on product.ProdId equals BatchCode.ProdId
                                          where BatchCode.ActiveStatus == "Y"
                                          join tax in entity.M_TaxMaster on product.ProdId equals tax.ProdCode
                                          //where tax.StateCode == CurrentStateCode
                                          from c in entity.M_CatMaster
                                          where c.CatId == product.CatId
                                          select new ProductModel
                                          {
                                              ProductName = product.ProductName,
                                              CatId = product.CatId,
                                              CatName = c.CatName,
                                              Barcode = barcode.BarCode,
                                              BatchNo = BatchCode.BatchNo,
                                              DP = BatchCode.Dp,
                                              RP = product.RP,
                                              DiscPer = product.Discount,
                                              DiscAmt = product.DiscInRs,
                                              ProdCode = (int)product.ProductCode,
                                              ProductCodeStr = product.ProdId,
                                              TaxPer = tax.VatTax,
                                              ProdStateCode = tax.StateCode,
                                              MRP = BatchCode.Mrp,
                                              BV = product.BV,
                                              PV = product.PV,
                                              CV = product.CV,
                                              IsExpirable = BatchCode.IsExpired == "Y" ? true : false,
                                              ExpDate = BatchCode.ExpDate,
                                              TaxType = "GST",
                                              Rate = product.PurchaseRate,
                                              CommissionPer = product.ProdCommssn,
                                              SubCatId = product.SubCatId,
                                              IsAvailableForOffer = product.IsAvailableforOffers,
                                              IsAvailableForBilling = product.IsBillingAllowed,
                                              Weight = product.Weight,
                                              TotalDiscPer = product.SJDiscount ?? 0
                                          }).ToList();


                        }
                        else
                        {
                            decimal? BarCodeData = decimal.Parse(data);
                            TempResult = (from product in entity.M_ProductMaster
                                          where product.ActiveStatus == "Y" && product.IsCardIssue == "N"
                                          join barcode in entity.M_BarCodeMaster on product.ProdId equals barcode.ProdId
                                          where barcode.BarCode == data
                                          join BatchCode in entity.M_BatchMaster on product.ProdId equals BatchCode.ProdId
                                          where BatchCode.ActiveStatus == "Y"
                                          join tax in entity.M_TaxMaster on product.ProdId equals tax.ProdCode
                                          from c in entity.M_CatMaster
                                          where c.CatId == product.CatId
                                          select new ProductModel
                                          {
                                              ProductName = product.ProductName,
                                              CatId = product.CatId,
                                              CatName = c.CatName,
                                              Barcode = barcode.BarCode,
                                              BatchNo = BatchCode.BatchNo,
                                              DP = barcode.DP,
                                              RP = product.RP,
                                              ProdStateCode = tax.StateCode,
                                              DiscPer = product.Discount,
                                              DiscAmt = product.DiscInRs,
                                              ProdCode = (int)product.ProductCode,
                                              ProductCodeStr = product.ProdId,
                                              TaxPer = tax.VatTax,
                                              MRP = barcode.MRP,
                                              BV = product.BV,
                                              PV = product.PV,
                                              CV = product.CV,
                                              IsExpirable = BatchCode.IsExpired == "Y" ? true : false,
                                              ExpDate = BatchCode.ExpDate,
                                              TaxType = "GST",
                                              Rate = product.PurchaseRate,
                                              CommissionPer = product.ProdCommssn,
                                              IsAvailableForOffer = product.IsAvailableforOffers,
                                              TotalDiscPer = product.SJDiscount ?? 0
                                          }).ToList();


                        }
                        bool IsDistributorBill = false;
                        bool IsPartyBill = false;
                        bool IsCustomerBill = false;
                        bool IsPurchaseInvoice = false;
                        bool IsOrderCreation = false;
                        bool IsPendingOrder = false;
                        if (BillType == "distributor")
                        {
                            IsDistributorBill = true;
                        }
                        else
                        {
                            IsDistributorBill = false;
                        }
                        if (BillType == "party")
                        {
                            IsPartyBill = true;
                        }
                        else
                        {
                            IsPartyBill = false;
                        }
                        if (BillType == "customer")
                        {
                            IsCustomerBill = true;
                        }
                        else
                        {
                            IsCustomerBill = false;
                        }
                        if (BillType == "purchase")
                        {
                            IsPurchaseInvoice = true;
                        }
                        else
                        {
                            IsPurchaseInvoice = false;
                        }
                        if (BillType == "order")
                        {
                            IsOrderCreation = true;
                        }
                        else
                        {
                            IsOrderCreation = false;
                        }
                        if (BillType == "pendingorder")
                        {
                            IsPendingOrder = true;
                        }
                        else
                        {
                            IsPendingOrder = false;
                        }

                        foreach (var obj in TempResult)
                        {
                            ProductModel TempObj = new ProductModel();
                            if ((obj.IsExpirable && obj.ExpDate > DateTime.Now) || (obj.IsExpirable == false))
                            {
                                TempObj = obj;
                                object valueIsDiscountAdd = 0;
                                object valueIsCommissonAdd = 0;
                                if (IsDistributorBill || IsCustomerBill || IsPurchaseInvoice || IsOrderCreation || IsPendingOrder)
                                {
                                    valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAdd.ToString());
                                    valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAdd.ToString());

                                }
                                else
                                {
                                    valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAddOnPartyBill.ToString());
                                    valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAddOnPartyBill.ToString());

                                }
                                int IsCommission = Convert.ToInt32(valueIsCommissonAdd);
                                int IsDiscount = Convert.ToInt32(valueIsDiscountAdd);
                                TempObj.IsCommissionAdd = IsCommission;
                                TempObj.IsDiscountAdd = IsDiscount;
                                TempObj.StockAvailable = (from stockAvail in entity.Im_CurrentStock
                                                          where stockAvail.Barcode == TempObj.Barcode.ToString() && stockAvail.ProdId == TempObj.ProductCodeStr.ToString() && stockAvail.FCode.Equals(CurrentPartyCode) && stockAvail.BatchCode.Equals(TempObj.BatchNo.ToString())
                                                          select stockAvail.Qty
                                                     ).DefaultIfEmpty(0).Sum();
                                TempObj.DP1 = TempObj.DP;
                                if (IsCustomerBill)
                                {
                                    TempObj.DP = obj.MRP;

                                }
                                else
                                {
                                    if (!IsPurchaseInvoice && IsBillOnMrp)
                                    {
                                        TempObj.DP = obj.MRP;
                                    }
                                }
                                CurrentStateCode = (int)(from r in entity.M_CompanyMaster select r.CompState).FirstOrDefault();
                                if (allhalf)
                                {
                                    TempObj.DP = TempObj.DP / 2;
                                    TempObj.BV = TempObj.BV / 2;
                                    TempObj.PV = TempObj.PV / 2;
                                    TempObj.RP = TempObj.RP / 2;
                                    TempObj.DiscAmt = TempObj.DP;
                                    TempObj.IsDiscountAdd = 1;
                                }
                                if (!string.IsNullOrEmpty(Invoice) && Invoice == "CED")
                                {
                                    var oridp = TempObj.DP;
                                    TempObj.DP = (TempObj.DP * 1) / 4;
                                    TempObj.BV = 0;
                                    TempObj.PV = (TempObj.PV * 1) / 4;
                                    TempObj.RP = (TempObj.RP * 1) / 4;
                                    TempObj.DiscAmt = oridp - TempObj.DP;
                                    TempObj.IsDiscountAdd = 1;
                                }
                                if (!string.IsNullOrEmpty(OfferID))
                                {
                                    decimal iOfferID = Convert.ToDecimal(OfferID);
                                    if (iOfferID != 0)
                                    {
                                        TempObj.offerDetail = GetOfferDetail(iOfferID, obj.ProductCodeStr, IsSpclOffer);
                                        if (!string.IsNullOrEmpty(TempObj.offerDetail.offerType))
                                        {
                                            decimal offerType = Convert.ToDecimal(TempObj.offerDetail.offerType);
                                            if (offerType == 2 || offerType == 3)
                                            {
                                                TempObj.DP = TempObj.offerDetail.OfferMrp / 1;// TempObj.offerDetail.FreeQty;
                                                TempObj.OfferProdQty = TempObj.offerDetail.FreeQty;
                                                TempObj.OfferProdQty = 1;
                                                TempObj.BV = 0;
                                            }
                                        }
                                    }
                                }
                                objProductModel.Add(TempObj);
                            }
                        }

                        //if (objProductModel.Count > 1 && !IsPurchaseInvoice)
                        //{
                        //    objProductModel = objProductModel.Where(m => m.StockAvailable > 0).OrderBy(m => m.ExpDate).ThenBy(m => m.StockAvailable).ToList();
                        //}
                    }
                }
            }
            catch (Exception e)
            {

            }
            return objProductModel;
        }
        public List<ProductModel> GetproductBatchInfo(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool allhalf, string Invoice, string IsSpclOffer)
        {
            List<ProductModel> objProductModel = new List<ProductModel>();
            List<ProductModel> TempResult = new List<ProductModel>();
            bool searchByProductFlag = true;
            if (SearchType == "B")
            {
                searchByProductFlag = false;
            }

            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    using (var entity = new InventoryEntities())
                    {

                        if (searchByProductFlag)
                        {

                            TempResult = (from product in entity.M_ProductMaster
                                          where product.ProductName.ToLower().Equals(data.ToLower()) && product.ActiveStatus == "Y" //&& product.IsCardIssue == "N"
                                          join barcode in entity.M_BarCodeMaster on product.ProdId equals barcode.ProdId
                                          where barcode.ActiveStatus == "Y"
                                          join tax in entity.M_TaxMaster on product.ProdId equals tax.ProdCode
                                          //where tax.StateCode == CurrentStateCode
                                          from c in entity.M_CatMaster
                                          where c.CatId == product.CatId
                                          select new ProductModel
                                          {
                                              ProductName = product.ProductName,
                                              CatId = product.CatId,
                                              CatName = c.CatName,
                                              Barcode = barcode.BarCode,
                                              BatchNo = barcode.BatchNo,
                                              DP = barcode.DP,
                                              RP = product.RP,
                                              DiscPer = product.Discount,
                                              DiscAmt = product.DiscInRs,
                                              ProdCode = (int)product.ProductCode,
                                              ProductCodeStr = product.ProdId,
                                              TaxPer = tax.VatTax,
                                              ProdStateCode = tax.StateCode,
                                              MRP = barcode.MRP,
                                              BV = product.BV,
                                              PV = product.PV,
                                              CV = product.CV,
                                              IsExpirable = barcode.IsExpired == "Y" ? true : false,
                                              ExpDate = barcode.ExpDate,
                                              TaxType = "GST",
                                              Rate = product.PurchaseRate,
                                              CommissionPer = product.ProdCommssn,
                                              SubCatId = product.SubCatId,
                                              IsAvailableForOffer = product.IsAvailableforOffers,
                                              IsAvailableForBilling = product.IsBillingAllowed,
                                              Weight = product.Weight,
                                              TotalDiscPer = product.SJDiscount ?? 0
                                          }).ToList();


                        }
                        else
                        {
                            decimal? BarCodeData = decimal.Parse(data);
                            TempResult = (from product in entity.M_ProductMaster
                                          where product.ActiveStatus == "Y" && product.IsCardIssue == "N"
                                          join barcode in entity.M_BarCodeMaster on product.ProdId equals barcode.ProdId
                                          where barcode.BarCode == data
                                          join tax in entity.M_TaxMaster on product.ProdId equals tax.ProdCode
                                          from c in entity.M_CatMaster
                                          where c.CatId == product.CatId
                                          select new ProductModel
                                          {
                                              ProductName = product.ProductName,
                                              CatId = product.CatId,
                                              CatName = c.CatName,
                                              Barcode = barcode.BarCode,
                                              BatchNo = barcode.BatchNo,
                                              DP = barcode.DP,
                                              RP = product.RP,
                                              ProdStateCode = tax.StateCode,
                                              DiscPer = product.Discount,
                                              DiscAmt = product.DiscInRs,
                                              ProdCode = (int)product.ProductCode,
                                              ProductCodeStr = product.ProdId,
                                              TaxPer = tax.VatTax,
                                              MRP = barcode.MRP,
                                              BV = product.BV,
                                              PV = product.PV,
                                              CV = product.CV,
                                              IsExpirable = barcode.IsExpired == "Y" ? true : false,
                                              ExpDate = barcode.ExpDate,
                                              TaxType = "GST",
                                              Rate = product.PurchaseRate,
                                              CommissionPer = product.ProdCommssn,
                                              IsAvailableForOffer = product.IsAvailableforOffers,
                                              TotalDiscPer = product.SJDiscount ?? 0
                                          }).ToList();


                        }
                        bool IsDistributorBill = false;
                        bool IsPartyBill = false;
                        bool IsCustomerBill = false;
                        bool IsPurchaseInvoice = false;
                        bool IsOrderCreation = false;
                        bool IsPendingOrder = false;
                        if (BillType == "distributor")
                        {
                            IsDistributorBill = true;
                        }
                        else
                        {
                            IsDistributorBill = false;
                        }
                        if (BillType == "party")
                        {
                            IsPartyBill = true;
                        }
                        else
                        {
                            IsPartyBill = false;
                        }
                        if (BillType == "customer")
                        {
                            IsCustomerBill = true;
                        }
                        else
                        {
                            IsCustomerBill = false;
                        }
                        if (BillType == "purchase")
                        {
                            IsPurchaseInvoice = true;
                        }
                        else
                        {
                            IsPurchaseInvoice = false;
                        }
                        if (BillType == "order")
                        {
                            IsOrderCreation = true;
                        }
                        else
                        {
                            IsOrderCreation = false;
                        }
                        if (BillType == "pendingorder")
                        {
                            IsPendingOrder = true;
                        }
                        else
                        {
                            IsPendingOrder = false;
                        }

                        foreach (var obj in TempResult)
                        {
                            ProductModel TempObj = new ProductModel();
                            //&& obj.ExpDate > DateTime.Now
                            if ((obj.IsExpirable) || (obj.IsExpirable == false))
                            {
                                TempObj = obj;
                                object valueIsDiscountAdd = 0;
                                object valueIsCommissonAdd = 0;
                                if (IsDistributorBill || IsCustomerBill || IsPurchaseInvoice || IsOrderCreation || IsPendingOrder)
                                {
                                    valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAdd.ToString());
                                    valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAdd.ToString());

                                }
                                else
                                {
                                    valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAddOnPartyBill.ToString());
                                    valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAddOnPartyBill.ToString());

                                }
                                int IsCommission = Convert.ToInt32(valueIsCommissonAdd);
                                int IsDiscount = Convert.ToInt32(valueIsDiscountAdd);
                                TempObj.IsCommissionAdd = IsCommission;
                                TempObj.IsDiscountAdd = IsDiscount;
                                TempObj.StockAvailable = (from stockAvail in entity.Im_CurrentStock
                                                          where stockAvail.BatchCode == TempObj.Barcode.ToString() && stockAvail.ProdId == TempObj.ProductCodeStr.ToString() && stockAvail.FCode.Equals(CurrentPartyCode)
                                                          select stockAvail.Qty
                                                     ).DefaultIfEmpty(0).Sum();
                                TempObj.DP1 = TempObj.DP;
                                if (IsCustomerBill)
                                {
                                    TempObj.DP = obj.MRP;

                                }
                                else
                                {
                                    if (!IsPurchaseInvoice && IsBillOnMrp)
                                    {
                                        TempObj.DP = obj.MRP;
                                    }
                                }
                                CurrentStateCode = (int)(from r in entity.M_CompanyMaster select r.CompState).FirstOrDefault();
                                if (allhalf)
                                {
                                    TempObj.DP = TempObj.DP / 2;
                                    TempObj.BV = TempObj.BV / 2;
                                    TempObj.PV = TempObj.PV / 2;
                                    TempObj.RP = TempObj.RP / 2;
                                    TempObj.DiscAmt = TempObj.DP;
                                    TempObj.IsDiscountAdd = 1;
                                }
                                if (!string.IsNullOrEmpty(Invoice) && Invoice == "CED")
                                {
                                    var oridp = TempObj.DP;
                                    TempObj.DP = (TempObj.DP * 1) / 4;
                                    TempObj.BV = 0;
                                    TempObj.PV = (TempObj.PV * 1) / 4;
                                    TempObj.RP = (TempObj.RP * 1) / 4;
                                    TempObj.DiscAmt = oridp - TempObj.DP;
                                    TempObj.IsDiscountAdd = 1;
                                }
                                if (!string.IsNullOrEmpty(OfferID))
                                {
                                    decimal iOfferID = Convert.ToDecimal(OfferID);
                                    if (iOfferID != 0)
                                    {
                                        TempObj.offerDetail = GetOfferDetail(iOfferID, obj.ProductCodeStr, IsSpclOffer);
                                        if (!string.IsNullOrEmpty(TempObj.offerDetail.offerType))
                                        {
                                            decimal offerType = Convert.ToDecimal(TempObj.offerDetail.offerType);
                                            if (offerType == 2 || offerType == 3)
                                            {
                                                TempObj.DP = TempObj.offerDetail.OfferMrp / 1;// TempObj.offerDetail.FreeQty;
                                                TempObj.OfferProdQty = TempObj.offerDetail.FreeQty;
                                                TempObj.OfferProdQty = 1;
                                                TempObj.BV = 0;
                                            }
                                        }
                                    }
                                }
                                objProductModel.Add(TempObj);
                            }
                        }
                        if (objProductModel.Count > 1 && !IsPurchaseInvoice)
                        {
                            objProductModel = objProductModel.OrderBy(m => m.ExpDate).ThenBy(m => m.StockAvailable).ToList();
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return objProductModel;
        }



        public string GetStateGstName(decimal StateCode)
        {
            string statename = "";
            try
            {
                using (var entity = new InventoryEntities())
                {
                    string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                    SqlConnection SC = new SqlConnection(AppConnectionString);
                    string query = "Select StateName +' ('+ CASE WHEN LEN(Cast(DivisionCode as varchar(5)))=1 THEN '0'+Cast(DivisionCode as varchar(5)) ELSE Cast(DivisionCode as varchar(5)) END +')' as S, * FROM M_StateDivMaster WHERE RowStatus='Y' AND StateCode>0 AND StateCode=@StateCode";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@StateCode", StateCode);
                    cmd.Connection = SC;
                    SC.Close();
                    SC.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            statename = reader["S"].ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return statename;
        }

        public CustomerDetail GetCustInfo(string IdNo)
        {
            CustomerDetail objCustomerDetail = new CustomerDetail();
            if (!(string.IsNullOrEmpty(IdNo)))
            {
                try
                {
                    string ElligibleFor = "";
                    int isoldID = 0;
                    using (var entity = new InventoryEntities())
                    {
                        //string MemberResponse = GetMemeberDetail(IdNo);
                        //if (!string.IsNullOrEmpty(MemberResponse))
                        //{
                        //MemberAPIRoot memapi = JsonConvert.DeserializeObject<MemberAPIRoot>(MemberResponse);
                        //if (memapi.Success == "true")
                        //{

                        string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                        SqlConnection SC = new SqlConnection(AppConnectionString);

                        //Insert new user 
                        //string userinertsql = "exec Sp_AddMemberDetail @Username,@Password,@MemName,@MobileNo,@EMail,@Address1,@State,@City";
                        //if (string.IsNullOrEmpty(memapi.Result.MobileNo))
                        //{
                        //    memapi.Result.MobileNo = "0";
                        //}
                        //SqlCommand cmd1 = new SqlCommand();
                        //cmd1.CommandText = userinertsql;
                        //cmd1.Parameters.AddWithValue("@Username", memapi.Result.loginid);
                        //cmd1.Parameters.AddWithValue("@Password", "123");
                        //cmd1.Parameters.AddWithValue("@MemName", memapi.Result.name);
                        //cmd1.Parameters.AddWithValue("@MobileNo", memapi.Result.MobileNo);
                        //cmd1.Parameters.AddWithValue("@EMail", memapi.Result.email);
                        //cmd1.Parameters.AddWithValue("@Address1", memapi.Result.Address);
                        //cmd1.Parameters.AddWithValue("@State", memapi.Result.State);
                        //cmd1.Parameters.AddWithValue("@City", memapi.Result.city);
                        //cmd1.Connection = SC;
                        //SC.Close();
                        //SC.Open();
                        //using (SqlDataReader reader = cmd1.ExecuteReader())
                        //{

                        //}


                        string query = "select a.Passw,a.Doj,a.UpgradeDate,a.Mobl,a.FormNo,a.MemFirstName+' '+ a.MemLastName as Name,a.KitId,a.IDno as IDno,a.Address1+','+a.Address2+','+a.City as Address,a.StateCode as StateCode,a.City,ISNULL(s.StateName,'') StateName,a.ActiveStatus as ActiveStatus,a.IsBlock as IsBlock,IsOldID=IIF(a.RefID>0,1,0),a.Imported as ElligibleFor,b.idno as RefId,b.MemFirstName+' '+ b.MemLastName as RefName,a.PanNo,a.District FROM M_MemberMaster b,M_MemberMaster a LEFT JOIN M_StateDivMaster s ON a.StateCode=s.StateCode WHERE a.RefFormNo=b.FormNo AND a.IDno=@IdNo";
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@IdNo", IdNo);
                        cmd.Connection = SC;
                        SC.Close();
                        SC.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isoldID = Convert.ToInt16(reader["IsOldID"].ToString());
                                ElligibleFor = reader["ElligibleFor"].ToString();
                                objCustomerDetail.Password = reader["Passw"] != null ? reader["Passw"].ToString() : "";
                                objCustomerDetail.IdNo = reader["IDno"] != null ? reader["IDno"].ToString() : "";
                                objCustomerDetail.ReferenceIdNo = reader["RefId"] != null ? reader["RefId"].ToString() : "";
                                objCustomerDetail.ReferenceName = reader["RefName"] != null ? reader["RefName"].ToString() : "";
                                objCustomerDetail.Name = reader["Name"] != null ? reader["Name"].ToString() : "";
                                objCustomerDetail.Address = reader["Address"] != null ? reader["Address"].ToString() : "";
                                objCustomerDetail.CityName = reader["city"] != null ? reader["city"].ToString() : "";
                                objCustomerDetail.StateName = reader["StateName"] != null ? reader["StateName"].ToString() : "";
                                objCustomerDetail.FormNo = reader["FormNo"] != null ? decimal.Parse(reader["FormNo"].ToString()) : 0;
                                objCustomerDetail.StateCode = reader["StateCode"] != null ? decimal.Parse(reader["StateCode"].ToString()) : 0;
                                objCustomerDetail.MobileNo = reader["Mobl"] != null ? reader["Mobl"].ToString() : "";
                                objCustomerDetail.KitId = reader["KitId"] != null ? int.Parse(reader["KitId"].ToString()) : 0;
                                objCustomerDetail.PANNo = reader["PanNo"] != null ? reader["PanNo"].ToString() : "";
                                objCustomerDetail.Doj = reader["Doj"] != null ? reader["Doj"].ToString() : "";
                                objCustomerDetail.UpgradeDate = reader["UpgradeDate"] != null ? reader["UpgradeDate"].ToString() : "";
                                objCustomerDetail.IsActive = reader["ActiveStatus"].ToString() == "Y" ? true : false;
                                objCustomerDetail.DistrictName = reader["District"] != null ? reader["District"].ToString() : "";
                                if (reader["IsBlock"] != null)
                                {
                                    var BlockValue = reader["IsBlock"].ToString();
                                    if (BlockValue == "Y")
                                    {
                                        objCustomerDetail.IsBlock = true;
                                    }
                                    else
                                    {
                                        objCustomerDetail.IsBlock = false;
                                    }
                                }
                                else
                                {
                                    objCustomerDetail.IsBlock = false;
                                }
                                //Cmnted on 18Jan19
                                //var result = (from r in entity.TrnBillMains where r.FCode == IdNo select r).FirstOrDefault();
                                //if (result == null)
                                if (ElligibleFor == "B")
                                {
                                    objCustomerDetail.IsFirstBill = true;
                                }
                                else
                                {
                                    objCustomerDetail.IsFirstBill = false;
                                }
                                objCustomerDetail.IsBillOnMrp = false;
                                //if (objCustomerDetail.IsFirstBill)
                                //{
                                //    objCustomerDetail.IsBillOnMrp = true;
                                //}
                                //else
                                //{
                                //    objCustomerDetail.IsBillOnMrp = false;
                                //}
                                //if (reader["ActiveStatus"] != null)
                                //{
                                //    var ActiveS = reader["ActiveStatus"].ToString();
                                //    if (ActiveS == "Y")
                                //    {
                                //        objCustomerDetail.IsFirstBill = false;
                                //        objCustomerDetail.IsBillOnMrp = false;
                                //        objCustomerDetail.IsActive = true;
                                //    }
                                //    else
                                //    {
                                //        objCustomerDetail.IsFirstBill = true;
                                //        objCustomerDetail.IsActive = false;
                                //        objCustomerDetail.IsBillOnMrp = true;
                                //    }
                                //}


                                //objCustomerDetail.WalletBalance = GetWalletBalance(IdNo);
                            }
                            else
                            {
                                objCustomerDetail = new CustomerDetail();
                                objCustomerDetail.IdNo = "Record does not exists!";
                                objCustomerDetail.Name = "";
                            }
                        }
                        SC.Close();
                        objCustomerDetail.MinRepurch = 0;
                        try
                        {
                            var delvPlace = entity.TrnBillMains
                           .Where(x => x.FCode == IdNo)
                           .OrderByDescending(x => x.BillId)
                           .Select(x => x.DelvPlace)
                           .FirstOrDefault();
                            if (delvPlace != null)
                            {
                                objCustomerDetail.DeliveryAddress = delvPlace;
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        //cmnted on 18Jan19
                        //if (objCustomerDetail.IsFirstBill == true && isoldID==0)//Condition Added on 18Jan19
                        //{
                        //    query = "select Min(BV) BV FROM M_KitMaster WHERE ActiveStatus='Y' AND BV>0";
                        //    cmd = new SqlCommand();
                        //    cmd.CommandText = query;
                        //    cmd.Connection = SC;
                        //    if (SC.State == ConnectionState.Closed)
                        //        SC.Open();
                        //    using (SqlDataReader reader = cmd.ExecuteReader())
                        //    {
                        //        if (reader.Read())
                        //            objCustomerDetail.MinRepurch = Convert.ToDecimal(reader["BV"].ToString());
                        //    }
                        //    SC.Close();
                        //}

                        if (objCustomerDetail != null)
                        {

                            query = "Select * FROM dbo.ufnGetBalance('" + objCustomerDetail.FormNo + "','S')";
                            cmd = new SqlCommand();
                            cmd.CommandText = query;
                            //cmd.Parameters.AddWithValue("@IdNo", IdNo);
                            cmd.Connection = SC;
                            SC.Close();
                            SC.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    objCustomerDetail.WalletBalance = decimal.Parse(reader["Balance"].ToString());
                                }
                            }
                            decimal Ktamt = 0;

                            string db = System.Configuration.ConfigurationManager.AppSettings["Database"];
                            string dbInv = System.Configuration.ConfigurationManager.AppSettings["INVDatabase"];


                            //   query = "Select Min(KitAmount) KitAmount FROM " + db + "..M_KitMaster WHERE KitAmount>0 AND ActiveStatus='Y'";
                            query = "Select BV FROM " + db + "..M_KitMaster WHERE ActiveStatus='Y' AND TopUpSeq>(Select TopUpSeq From " + db + "..M_KitMaster WHERE KitID=" + objCustomerDetail.KitId.ToString() + ") AND BV>0 AND KitAmount=0 AND IsBill='N' ORDER BY BV";
                            cmd = new SqlCommand();
                            cmd.CommandText = query;
                            //cmd.Parameters.AddWithValue("@IdNo", IdNo);
                            cmd.Connection = SC;
                            SC.Close();
                            SC.Open();
                            string MacAdres = "";
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Ktamt = !String.IsNullOrEmpty(reader["BV"].ToString()) ? decimal.Parse(reader["BV"].ToString()) : 0;
                                    //MacAdres = reader["MacAdrs"] != null ? reader["MacAdrs"].ToString() : "";

                                    break;
                                }
                            }
                            objCustomerDetail.MinBillAmt = Ktamt;

                            objCustomerDetail.MaxBV = 1000000;
                            query = "Select BVValue FROM " + dbInv + "..FPVConfig WHERE ActiveStatus='Y' AND RowStatus='Y'";
                            cmd = new SqlCommand();
                            cmd.CommandText = query;
                            //cmd.Parameters.AddWithValue("@IdNo", IdNo);
                            cmd.Connection = SC;
                            SC.Close();
                            SC.Open();

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Ktamt = !String.IsNullOrEmpty(reader["BVValue"].ToString()) ? decimal.Parse(reader["BVValue"].ToString()) : 0;
                                    objCustomerDetail.MaxBV = Ktamt;
                                    break;
                                }
                            }
                            SC.Close();
                            //**03Jun19[Start]
                            objCustomerDetail.InvoiceType = new List<string>();

                            var config = entity.M_ConfigMaster.FirstOrDefault();

                            if (objCustomerDetail.IsActive == true || ElligibleFor == "R")
                            {
                                if (Ktamt > 0)
                                {
                                    if (config.CanIDBeUpgraded == "Y")
                                    {
                                        objCustomerDetail.InvoiceType.Add("Activation Upgrade,B");
                                    }
                                    objCustomerDetail.InvoiceType.Add("Repurchase Bill,R");
                                }
                                else
                                {
                                    objCustomerDetail.MinBillAmt = 0;
                                    objCustomerDetail.InvoiceType.Add("Repurchase Bill,R");
                                }
                            }
                            else
                            {
                                objCustomerDetail.InvoiceType.Add("Activation Purchase,B");
                                //if (isoldID == 1)//Added on 18Jun19
                                objCustomerDetail.InvoiceType.Add("General Billing,A");//18Jun19

                            }

                            //**03Jun19[End]
                        }
                        //}

                        //}

                    }

                }
                catch (Exception e)
                {
                    objCustomerDetail = new CustomerDetail();
                    objCustomerDetail.IdNo = "Something went wrong!";
                    objCustomerDetail.Name = "";
                }
            }
            return objCustomerDetail;
        }
        public MemberAPIRoot ValidateCustomerbyAPI(string IdNo, string Password)
        {
            MemberAPIRoot memapi = new MemberAPIRoot();
            try
            {
                string MemberResponse = GetMemeberPasswordDetail(IdNo, Password);
                if (!string.IsNullOrEmpty(MemberResponse))
                {
                    memapi = JsonConvert.DeserializeObject<MemberAPIRoot>(MemberResponse);
                    if (memapi.Success == "true")
                    {
                        string FPBALbalance = GetMemeberBalance("FPBAL", IdNo);
                        var fpvpalres = JsonConvert.DeserializeObject<BalanceAPIRoot>(FPBALbalance);
                        if (fpvpalres.Success == "true")
                        {
                            memapi.Result.Fpv_Balance = fpvpalres.Balance;
                        }
                        var VOUCHERBAL = GetMemeberBalance("VOUCHERBAL", IdNo);
                        var VOUCHERBALres = JsonConvert.DeserializeObject<BalanceAPIRoot>(VOUCHERBAL);
                        if (VOUCHERBALres.Success == "true")
                        {
                            memapi.Result.VOUCHERBAL_Balance = VOUCHERBALres.Balance;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return memapi;
        }
        private string GetMemeberDetail(string Username)
        {
            string Response = string.Empty;
            try
            {
                string jsonrequest = "{\"act\":\"LOGIN\",\"uid\":\"" + Username + "\",\"pwd\":\"234\",\"logkey\":\"ZoewellnessgugddkhJJHJsddd\"}";
                HttpWebRequest request = WebRequest.Create("https://zoewellness.co.in/Api/api.ashx") as HttpWebRequest;
                request.ContentType = @"application/json";
                request.Method = @"POST";
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());
                requestWriter.Write(jsonrequest);
                requestWriter.Close();
                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                Response = responseReader.ReadToEnd();
            }
            catch (Exception ex)
            {

            }
            return Response;
        }

        private string GetMemeberBalance(string act, string Username)
        {
            string Response = string.Empty;
            try
            {
                string jsonrequest = "{\"act\":\"" + act + "\",\"uid\":\"" + Username + "\",\"logkey\":\"ZoewellnessgugddkhJJHJsddd\"}";
                HttpWebRequest request = WebRequest.Create("https://zoewellness.co.in/Api/api.ashx") as HttpWebRequest;
                request.ContentType = @"application/json";
                request.Method = @"POST";
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());
                requestWriter.Write(jsonrequest);
                requestWriter.Close();
                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                Response = responseReader.ReadToEnd();
            }
            catch (Exception ex)
            {

            }
            return Response;
        }


        public CustomerDetail GetCustPasswordValidate(string IdNo, string Password)
        {
            CustomerDetail objCustomerDetail = new CustomerDetail();
            if (!(string.IsNullOrEmpty(IdNo)) && !string.IsNullOrEmpty(Password))
            {
                try
                {
                    string MemberResponse = GetMemeberPasswordDetail(IdNo, Password);
                    if (!string.IsNullOrEmpty(MemberResponse))
                    {
                        MemberPasswordAPIRoot memapi = JsonConvert.DeserializeObject<MemberPasswordAPIRoot>(MemberResponse);
                        if (memapi.Success == "true")
                        {
                            objCustomerDetail = new CustomerDetail();
                            objCustomerDetail.IdNo = memapi.ApiMessage;
                            objCustomerDetail.Name = memapi.Success;
                            return objCustomerDetail;
                        }
                        else
                        {
                            objCustomerDetail = new CustomerDetail();
                            objCustomerDetail.IdNo = memapi.ApiMessage;
                            objCustomerDetail.Name = memapi.Success;
                            return objCustomerDetail;
                        }
                    }
                    else
                    {
                        objCustomerDetail = new CustomerDetail();
                        objCustomerDetail.IdNo = "Record does not exists!";
                        objCustomerDetail.Name = "false";
                        return objCustomerDetail;
                    }
                }
                catch (Exception e)
                {
                    objCustomerDetail = new CustomerDetail();
                    objCustomerDetail.IdNo = "Something went wrong!";
                    objCustomerDetail.Name = "";
                }
            }
            return objCustomerDetail;
        }

        private string GetMemeberPasswordDetail(string Username, string Password)
        {
            string Response = string.Empty;
            try
            {
                string jsonrequest = "{\"act\":\"CHECKPASS\",\"uid\":\"" + Username + "\",\"pwd\":\"" + Password + "\",\"logkey\":\"ZoewellnessgugddkhJJHJsddd\"}";
                HttpWebRequest request = WebRequest.Create("https://zoewellness.co.in/Api/api.ashx") as HttpWebRequest;
                request.ContentType = @"application/json";
                request.Method = @"POST";
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());
                requestWriter.Write(jsonrequest);
                requestWriter.Close();
                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                Response = responseReader.ReadToEnd();
            }
            catch (Exception ex)
            {

            }
            return Response;
        }
        private string GetMemeberDetail(string Username, string Password)
        {
            string Response = string.Empty;
            //try
            //{
            //    HttpWebRequest request = WebRequest.Create("https://zoewellness.in/Api/api.ashx") as HttpWebRequest;
            //    request.ContentType = @"application/json";
            //    request.Method = @"POST";
            //    StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());
            //    requestWriter.Write("{\"act\":\"LOGIN\",\"uid\":\"" + Username + "\",\"pwd\":\"" + Password + "\",\"logkey\":\"ZoewellnessgugddkhJJHJsddd\"}");
            //    requestWriter.Close();
            //    HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
            //    StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            //    Response = responseReader.ReadToEnd();
            //}
            try
            {
                string jsonrequest = "{\"act\":\"LOGIN\",\"uid\":\"" + Username + "\",\"pwd\":\"" + Password + "\",\"logkey\":\"ZoewellnessgugddkhJJHJsddd\"}";
                HttpWebRequest request = WebRequest.Create("https://zoewellness.co.in/Api/api.ashx") as HttpWebRequest;
                request.ContentType = @"application/json";
                request.Method = @"POST";
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());
                requestWriter.Write(jsonrequest);
                requestWriter.Close();
                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                Response = responseReader.ReadToEnd();
            }
            catch (Exception ex)
            {

            }
            return Response;
        }

        public List<string> GetAutocompleteProductNames(string InvType)
        {
            List<string> objProductNames = new List<string>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    var ObjProdList = (from result in entity.M_ProductMaster
                                       where result.ActiveStatus == "Y" && result.IsBillingAllowed == "Y" //&& result.PType != "K"
                                       select result).ToList();
                    if (InvType != "Purchase")
                    {
                        objProductNames = ObjProdList.Where(m => m.IsCardIssue == "N").Select(m => m.ProductName).ToList();
                    }
                    else if (InvType != "Partybill")
                    {
                        objProductNames = ObjProdList.Where(m => m.IsCardIssue == "N").Select(m => m.ProductName).ToList();
                        //int i = 0;
                        //foreach (var product in objProductNames)
                        //{
                        //    var prodstockdetail = (from prod in entity.M_ProductMaster
                        //                           where prod.ProductName == product
                        //                           select new ProductBatchModel
                        //                           {
                        //                               StockAvailable = (from stockAvail in entity.Im_CurrentStock
                        //                                                 where stockAvail.ProdId == prod.ProdId.ToString() //&&
                        //                                                 //stockAvail.FCode.Equals(CurrentPartyCode)
                        //                                                 select stockAvail.Qty
                        //                                                ).DefaultIfEmpty(0).Sum()
                        //                           }).FirstOrDefault();
                        //    if (prodstockdetail.StockAvailable <= 0)
                        //    {
                        //        product.Remove(i);
                        //    }
                        //    i++;
                        //}

                    }
                    else
                    {
                        objProductNames = ObjProdList.Select(m => m.ProductName).ToList();
                    }
                }

            }
            catch (Exception e)
            {

            }

            return objProductNames;
        }
        public List<string> GetAvailStockProductNamesOnly(string StockforParty)
        {
            List<string> objProductNames = new List<string>();
            try
            {
                using (var entity = new InventoryEntities())
                {

                    objProductNames = (from p in entity.M_ProductMaster
                                       join b in entity.M_BatchMaster on p.ProdId equals b.ProdId
                                       join s in entity.Im_CurrentStock on b.BatchNo equals s.BatchCode
                                       where p.ActiveStatus == "Y"
                                             && (p.IsBillingAllowed == "Y" || p.IsAvailableforOffers == "Y")
                                             && p.IsCardIssue == "N"
                                             && p.PType != "K"
                                             && b.ActiveStatus == "Y"
                                             && s.FCode == StockforParty
                                             && s.ProdId == p.ProdId
                                       group s by new
                                       {
                                           p.ProductName,
                                       } into g
                                       where g.Sum(x => x.Qty) > 0
                                       select g.Key.ProductName
                       ).Distinct().ToList();

                    //var ObjProdList = (from result in entity.M_ProductMaster
                    //                   where result.ActiveStatus == "Y" && result.IsBillingAllowed == "Y" //&& result.PType != "K"
                    //                   select result).ToList();

                    //objProductNames = ObjProdList.Where(m => m.IsCardIssue == "N").Select(m => m.ProductName).ToList();

                    //foreach (var product in objProductNames)
                    //{
                    //    var prodstockdetail = (from prod in entity.M_ProductMaster
                    //                           where prod.ProductName == product
                    //                           select new ProductBatchModel
                    //                           {
                    //                               StockAvailable = (from stockAvail in entity.Im_CurrentStock
                    //                                                 where stockAvail.ProdId == prod.ProdId.ToString() &&
                    //                                                 stockAvail.FCode.Equals(StockforParty)
                    //                                                 select stockAvail.Qty
                    //                                                ).DefaultIfEmpty(0).Sum()
                    //                           }).FirstOrDefault();
                    //    if (prodstockdetail.StockAvailable <= 0)
                    //    {
                    //        objProductNames.Remove(product);
                    //    }
                    //}

                }

            }
            catch (Exception e)
            {

            }

            return objProductNames;
        }
        public List<string> GetAllBarcode()
        {
            List<string> objProductNames = new List<string>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    var ObjProdList = (from result in entity.M_ProductMaster
                                       where result.ActiveStatus == "Y" && result.IsBillingAllowed == "Y" //&& result.PType != "K"
                                       select result).ToList();

                    objProductNames = ObjProdList.Select(m => m.Barcode).ToList();
                }

            }
            catch (Exception e)
            {

            }

            return objProductNames;
        }

        public List<string> GetAutocompProductsOnly(string CurrentPartyCode)
        {
            List<string> objProductNames = new List<string>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objProductNames = (from result in entity.M_ProductMaster
                                       where result.ActiveStatus == "Y" && (result.IsBillingAllowed == "Y" || result.IsAvailableforOffers == "Y") && result.IsCardIssue == "N" && result.PType != "K"
                                       select result.ProductName).ToList();


                    //objProductNames = (from p in entity.M_ProductMaster
                    //                   join b in entity.M_BatchMaster on p.ProdId equals b.ProdId
                    //                   join s in entity.Im_CurrentStock on b.BatchNo equals s.BatchCode
                    //                   where p.ActiveStatus == "Y"
                    //                         && (p.IsBillingAllowed == "Y" || p.IsAvailableforOffers == "Y")
                    //                         && p.IsCardIssue == "N"
                    //                         && p.PType != "K"
                    //                         && b.ActiveStatus == "Y"
                    //                         && s.FCode == CurrentPartyCode
                    //                         && s.ProdId == p.ProdId
                    //                   group s by new
                    //                   {
                    //                       p.ProductName,
                    //                   } into g
                    //                   where g.Sum(x => x.Qty) > 0
                    //                   select g.Key.ProductName
                    //   ).Distinct().ToList();

                }
            }
            catch (Exception e)
            {

            }
            return objProductNames;
        }

        public List<string> GetAutocompFPVProducts(int ProdBunchID, string InvoiceType)
        {
            List<string> objProductNames = new List<string>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    if (InvoiceType == "SJP")
                    {
                        objProductNames = (from result in entity.M_ProductMaster
                                           where result.ActiveStatus == "Y" && result.IsBillingAllowed == "Y" && result.IsCardIssue == "N" && result.PType != "K"
                                           select result.ProductName).ToList();
                    }
                    else if (InvoiceType == "FPV")
                    {
                        objProductNames = (from result in entity.M_ProductMaster
                                           where result.ActiveStatus == "Y" && result.IsBillingAllowed == "Y" && result.IsCardIssue == "N" && result.PType != "K" && result.AllowedForFPV == "Y"
                                           select result.ProductName).ToList();
                    }
                    else if (InvoiceType == "CPV")
                    {
                        objProductNames = (from result in entity.M_ProductMaster
                                           where result.ActiveStatus == "Y" && result.IsBillingAllowed == "Y" && result.IsCardIssue == "N" && result.PType != "K" && result.AllowedForGPV == "Y"
                                           select result.ProductName).ToList();
                    }

                    else if (InvoiceType == "GPV")
                    {
                        objProductNames = (from result in entity.M_ProductMaster
                                               //                  join
                                               //f in entity.FPVProductConfigs on result.ProdId equals f.ProductId.ToString()
                                               //                  where f.ActiveStatus == "Y" && f.RowStatus == "Y" && f.BunchId == ProdBunchID
                                           where result.ActiveStatus == "Y" && result.IsBillingAllowed == "Y" && result.IsCardIssue == "N" && result.PType != "K" && result.AllowedForGV == "Y"
                                           select result.ProductName).ToList();
                    }
                    else if (InvoiceType == "MRI")
                    {
                        objProductNames = (from result in entity.M_ProductMaster
                                               //                  join
                                               //f in entity.FPVProductConfigs on result.ProdId equals f.ProductId.ToString()
                                               //                  where f.ActiveStatus == "Y" && f.RowStatus == "Y" && f.BunchId == ProdBunchID
                                           where result.ActiveStatus == "Y" && result.IsBillingAllowed == "Y" && result.IsCardIssue == "N" && result.PType != "K" && result.AllowedForMRI == "Y"
                                           select result.ProductName).ToList();
                    }
                }
            }
            catch (Exception e)
            {

            }
            return objProductNames;
        }
        public string QPostJSON(string URL)
        {
            using (var entity = new InventoryEntities())
            {
                var Credentials = entity.EInvoiceCredentials
                                           .OrderByDescending(t => t.id)
                                            .FirstOrDefault();

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12;
                //string Data = "aspid=1674282722&Password=aCHYUTA@17&gstin=34AACCC1596Q002&user_name=TaxProEnvPON&eInvPwd=abc34*";
                string Data = "aspid=" + Credentials.Aspid + "&Password=" + Credentials.ApiPassword + "&gstin=" + Credentials.Gstin + "&user_name=" + Credentials.Username + "&eInvPwd=" + Credentials.EInvPwd + "";
                string sResponseFromServer = string.Empty;
                try
                {
                    WebRequest tRequest;
                    Stream dataStream;
                    tRequest = WebRequest.Create(URL + Data);
                    WebResponse tResponse = tRequest.GetResponse();
                    dataStream = tResponse.GetResponseStream();
                    StreamReader tReader = new StreamReader(dataStream);
                    sResponseFromServer = tReader.ReadToEnd();
                    return sResponseFromServer;
                }
                finally
                {
                }
            }
        }
        private void Make_Inv_json(string UserBillNo)
        {

            string strres = "";
            SqlTransaction objTrans = null;
            string Atk = "";
            string s = "";
            //Einvoice eInvoice = new Einvoice();
            JsonReadVal AthToken;
            strres = QPostJSON("http://gstsandbox.charteredinfo.com/eivital/dec/v1.04/auth?");
            AthToken = JsonConvert.DeserializeObject<JsonReadVal>(strres);
            Atk = AthToken.Data.AuthToken;
            strres = "";



            string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
            SqlConnection SC1 = new SqlConnection(InvConnectionString);
            string query = "exec sp_getinvoiceNew @BillNo";
            DataSet dataSet = new DataSet();

            using (SqlConnection connection = new SqlConnection(InvConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Use a parameterized query
                        command.Parameters.AddWithValue("@BillNo", UserBillNo);

                        // Set the command timeout (e.g., 120 seconds)
                        command.CommandTimeout = 120;

                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                        {
                            dataAdapter.Fill(dataSet);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL Exception: " + ex.Message);
                }
            }
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt3 = new DataTable();




            if (dataSet.Tables.Count > 0)
            {
                Einvoice eInvoice = new Einvoice();
                eInvoice.TranDtls = new Trandtls();
                eInvoice.DocDtls = new DocDtls();
                eInvoice.SellerDtls = new Sellerdtls();
                eInvoice.BuyerDtls = new Buyerdtls();
                eInvoice.DispDtls = new Dispdtls();
                eInvoice.ShipDtls = new Shipdtls();
                eInvoice.ValDtls = new Valdtls();
                eInvoice.RefDtls = new Refdtls();
                eInvoice.PayDtls = new Paydtls();
                dt = dataSet.Tables[0];
                dt1 = dataSet.Tables[1];
                if (dataSet.Tables[3].Rows.Count > 0)
                    dt3 = dataSet.Tables[3];
                else
                    dt3 = dataSet.Tables[4];
                eInvoice.Version = "1.1";
                eInvoice.TranDtls.TaxSch = "GST";
                eInvoice.TranDtls.SupTyp = "B2B";
                eInvoice.TranDtls.RegRev = "N";
                eInvoice.TranDtls.EcmGstin = null;
                eInvoice.TranDtls.IgstOnIntra = "N";
                eInvoice.DocDtls.Typ = "INV";
                eInvoice.DocDtls.No = UserBillNo;

                eInvoice.DocDtls.Dt = dt.Rows[0]["BillDatestr"].ToString().Trim();
                eInvoice.SellerDtls.Gstin = "34AACCC1596Q002";//dt1.Rows[0]["TinNo"].ToString();
                eInvoice.SellerDtls.LglNm = dt1.Rows[0]["PartyName"].ToString();

                eInvoice.SellerDtls.TrdNm = dt1.Rows[0]["PartyName"].ToString();
                eInvoice.SellerDtls.Addr1 = dt1.Rows[0]["Address1"].ToString();

                eInvoice.SellerDtls.Addr2 = dt1.Rows[0]["Address2"].ToString() != "" ? dt1.Rows[0]["Address2"].ToString() : "   ";
                eInvoice.SellerDtls.Loc = dt1.Rows[0]["CityName"].ToString();
                eInvoice.SellerDtls.Pin = Convert.ToInt32("605001"); //dt1.Rows[0]["PinCode"].ToString () != ""
                                                                     // ? Convert.ToInt32(dt1.Rows[0]["PinCode"])
                                                                     //: 100000;
                eInvoice.SellerDtls.Stcd = "34";//dt1.Rows[0]["StateCode"].ToString();
                                                // eInvoice.SellerDtls.Ph = dt.Rows[0]["PhoneNo"].ToString() != "" ? dt.Rows[0]["PhoneNo"].ToString() : "999999999999";
                                                //eInvoice.SellerDtls.Em = dt.Rows[0]["E_mailAdd"].ToString() != "" ? dt.Rows[0]["E_mailAdd"].ToString() :"abc@gmail.com";
                eInvoice.DispDtls.Nm = dt1.Rows[0]["PartyName"].ToString();
                eInvoice.DispDtls.Addr1 = dt1.Rows[0]["Address1"].ToString();
                eInvoice.DispDtls.Addr2 = dt1.Rows[0]["Address2"].ToString() != "" ? dt1.Rows[0]["Address2"].ToString() : "   ";
                eInvoice.DispDtls.Loc = dt1.Rows[0]["CityName"].ToString();
                eInvoice.DispDtls.Pin = dt1.Rows[0]["PinCode"].ToString() != ""
                        ? Convert.ToInt32(dt1.Rows[0]["PinCode"])
                        : 100000;
                eInvoice.DispDtls.Stcd = dt1.Rows[0]["StateCode"].ToString();
                List<ItemList> products = new List<ItemList>();
                double cgstamt = 0;
                double sgstamt = 0;
                double AssVal = 0;
                double discount = 0;
                double netpayable = 0;
                double igstamt = 0;

                double roundoff = 0;
                foreach (DataRow reader in dt.Rows)
                {
                    cgstamt += reader["CGSTAmt"] != DBNull.Value ? Convert.ToDouble(reader["CGSTAmt"]) : 0.0;
                    sgstamt += reader["SGSTAmt"] != DBNull.Value ? Convert.ToDouble(reader["SGSTAmt"]) : 0.0;
                    igstamt += reader["TaxAmount"] != DBNull.Value ? Convert.ToDouble(reader["TaxAmount"]) : 0.0;
                    discount += reader["Discount"] != DBNull.Value ? Convert.ToDouble(reader["Discount"]) : 0.0;
                    AssVal += reader["NetAmount"] != DBNull.Value ? Convert.ToDouble(reader["NetAmount"]) : 0.0;
                    netpayable += reader["NetPayable"] != DBNull.Value ? Convert.ToDouble(reader["NetPayable"]) : 0.0;
                    roundoff += reader["rndoff"] != DBNull.Value ? Convert.ToDouble(reader["rndoff"]) : 0.0;

                    products.Add(new ItemList
                    {
                        SlNo = reader["SNo"]?.ToString() ?? string.Empty,
                        PrdDesc = reader["ProductName"]?.ToString() ?? string.Empty,
                        IsServc = "N",
                        HsnCd = reader["HsnCode"]?.ToString() ?? string.Empty,
                        Barcde = reader["Barcode"]?.ToString() ?? string.Empty,
                        Qty = reader["Qty"] != DBNull.Value ? Convert.ToInt32(reader["Qty"]) : 0,
                        FreeQty = reader["FreeQty"] != DBNull.Value ? Convert.ToInt32(reader["FreeQty"]) : 0,
                        Unit = "pcs",
                        UnitPrice = reader["Rate"] != DBNull.Value ? Convert.ToDouble(reader["Rate"]) : 0.0,
                        TotAmt = reader["NetAmount"] != DBNull.Value ? Convert.ToDouble(reader["NetAmount"]) : 0.0,
                        Discount = reader["Discount"] != DBNull.Value ? Convert.ToDouble(reader["Discount"]) : 0.0,
                        PreTaxVal = 0,
                        AssAmt = (reader["NetAmount"] != DBNull.Value && reader["Discount"] != DBNull.Value)
             ? Convert.ToDouble(reader["NetAmount"]) - Convert.ToDouble(reader["Discount"])
             : 0.0,
                        GstRt = reader["Tax"] != DBNull.Value ? Convert.ToDouble(reader["Tax"]) : 0.0,
                        IgstAmt = reader["TaxAmount"] != DBNull.Value ? Convert.ToDouble(reader["TaxAmount"]) : 0.0,
                        CgstAmt = reader["CGSTAmt"] != DBNull.Value ? Convert.ToDouble(reader["CGSTAmt"]) : 0.0,
                        SgstAmt = reader["SGSTAmt"] != DBNull.Value ? Convert.ToDouble(reader["SGSTAmt"]) : 0.0,
                        CesRt = 0,
                        CesAmt = 0,
                        CesNonAdvlAmt = 0,
                        StateCesRt = 0,
                        StateCesAmt = 0,
                        StateCesNonAdvlAmt = 0,
                        OthChrg = 0,
                        TotItemVal = (reader["NetAmount"] != DBNull.Value ? Convert.ToDouble(reader["NetAmount"]) : 0.0) + (reader["TaxAmount"] != DBNull.Value ? Convert.ToDouble(reader["TaxAmount"]) : 0.0)
                        + (reader["CGSTAmt"] != DBNull.Value ? Convert.ToDouble(reader["CGSTAmt"]) : 0.0) + (reader["SGSTAmt"] != DBNull.Value ? Convert.ToDouble(reader["SGSTAmt"]) : 0.0),
                        OrdLineRef = string.IsNullOrEmpty(reader["RefNo"]?.ToString()) ? "R" : reader["RefNo"].ToString(),
                        OrgCntry = "IN",
                        //PrdSlNo = "",
                        BchDtls = new Bchdtls
                        {
                            Nm = reader["BatchNo"]?.ToString() ?? string.Empty,
                            ExpDt = reader["ExpDate"]?.ToString() ?? string.Empty,
                            WrDt = reader["Mfgdate"]?.ToString() ?? string.Empty
                        }

                    });
                }

                eInvoice.ItemList.AddRange(products);
                //  dt3 = dataSet.Tables[3];
                eInvoice.BuyerDtls.Gstin = "29AWGPV7107B1Z1"; //dt3.Rows[0]["TinNo"]?.ToString() ?? string.Empty;
                eInvoice.BuyerDtls.LglNm = dt3.Rows[0]["PartyName"]?.ToString() ?? string.Empty;
                eInvoice.BuyerDtls.TrdNm = dt3.Rows[0]["PartyName"]?.ToString() ?? string.Empty;
                eInvoice.BuyerDtls.Addr1 = dt3.Rows[0]["Address1"]?.ToString() ?? string.Empty;
                eInvoice.BuyerDtls.Addr2 = dt3.Rows[0]["Address2"]?.ToString() != "" ? dt3.Rows[0]["Address2"].ToString() : "   ";
                eInvoice.BuyerDtls.Loc = dt3.Rows[0]["CityName"]?.ToString() ?? string.Empty;
                eInvoice.BuyerDtls.Pin = Convert.ToInt32(562160);// dt3.Rows[0]["PinCode"].ToString() != ""
                                                                 //? Convert.ToInt32(dt3.Rows[0]["PinCode"])
                                                                 //: 100000;
                eInvoice.BuyerDtls.Stcd = "29";//dt3.Rows[0]["StateCode"]?.ToString() ?? string.Empty;
                eInvoice.BuyerDtls.Pos = "12";//dt3.Rows[0]["StateCode"].ToString();
                //  eInvoice.BuyerDtls.Ph = dt.Rows[0]["PhoneNo"]?.ToString() != "" ? dt.Rows[0]["PhoneNo"].ToString() : "999999999999";
                //eInvoice.BuyerDtls.Em = dt.Rows[0]["E_mailAdd"]?.ToString() != "" ? dt.Rows[0]["E_mailAdd"].ToString() : "abc@gmail.com";

                // Populate Ship Details
                eInvoice.ShipDtls.Gstin = "29AWGPV7107B1Z1";//dt3.Rows[0]["TinNo"]?.ToString() ?? string.Empty;
                eInvoice.ShipDtls.LglNm = dt3.Rows[0]["PartyName"]?.ToString() ?? string.Empty;
                eInvoice.ShipDtls.TrdNm = dt3.Rows[0]["PartyName"]?.ToString() ?? string.Empty;
                eInvoice.ShipDtls.Addr1 = dt3.Rows[0]["Address1"]?.ToString() ?? string.Empty;
                eInvoice.ShipDtls.Addr2 = dt3.Rows[0]["Address2"]?.ToString() != "" ? dt3.Rows[0]["Address2"].ToString() : "   ";
                eInvoice.ShipDtls.Loc = dt3.Rows[0]["CityName"]?.ToString() ?? string.Empty;
                eInvoice.ShipDtls.Pin = dt3.Rows[0]["PinCode"].ToString() != ""
                    ? Convert.ToInt32(dt3.Rows[0]["PinCode"])
                    : 100000;
                eInvoice.ShipDtls.Stcd = dt3.Rows[0]["StateCode"]?.ToString() ?? string.Empty;

                //eInvoice.ValDtls.AssVal = Convert.ToDouble(dt.Rows[0]["NetPayable"]);
                eInvoice.ValDtls.CgstVal = cgstamt;
                eInvoice.ValDtls.SgstVal = sgstamt;
                eInvoice.ValDtls.IgstVal = igstamt;
                eInvoice.ValDtls.AssVal = AssVal - discount;
                eInvoice.ValDtls.TotInvVal = netpayable;
                eInvoice.ValDtls.RndOffAmt = roundoff;
                eInvoice.ValDtls.Discount = discount;
                eInvoice.ValDtls.StCesVal = 0;
                eInvoice.ValDtls.TotInvValFc = netpayable;
                eInvoice.ValDtls.CesVal = 0;
                eInvoice.PayDtls.AccDet = "0";
                eInvoice.PayDtls.CrDay = 0;
                eInvoice.PayDtls.CrTrn = eInvoice.ShipDtls.LglNm;
                eInvoice.PayDtls.DirDr = eInvoice.BuyerDtls.LglNm;
                eInvoice.PayDtls.Nm = eInvoice.BuyerDtls.LglNm;

                eInvoice.PayDtls.Mode = dataSet.Tables[2].Rows[0]["Paymode"].ToString();
                eInvoice.PayDtls.PaymtDue = 0;
                eInvoice.PayDtls.PaidAmt = Convert.ToInt32(dataSet.Tables[2].Rows[0]["BillAmt"]);
                //  eInvoice.RefDtls.InvRm = "";
                // eInvoice.RefDtls.I =  dt.Rows[0]["Billdate"].ToString  ();

                strres = JsonConvert.SerializeObject(eInvoice);
                if (SC1.State == ConnectionState.Closed)
                    SC1.Open();

                objTrans = SC1.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                s = "insert into EnVoiceRequest(UserBilNo,ERequest)Values('" + UserBillNo + "','" + strres + "')";

                cmd = new SqlCommand();
                cmd.CommandText = s;
                cmd.Connection = SC1;
                cmd.Transaction = objTrans;



                int i = cmd.ExecuteNonQuery();

                objTrans.Commit();
                SC1.Close();
                s = GetQrCode(strres, AthToken.Data.AuthToken, UserBillNo);
                IRNClass respon = new IRNClass();
                IRNData irndataa = new IRNData();
                respon = JsonConvert.DeserializeObject<IRNClass>(s);
                if (respon.Status == "1")
                {
                    if (SC1.State == ConnectionState.Closed)
                        SC1.Open();
                    irndataa = JsonConvert.DeserializeObject<IRNData>(respon.Data.ToString());




                    // Decode the Base64 string

                    byte[] bytes = Convert.FromBase64String(irndataa.QrCodeImage);
                    // string decodedPayload = Encoding.UTF8.GetString(bytes);
                    // Image image;
                    string fileName = "";
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        //Image image = Image.FromStream(ms);
                        //string serverPath = HttpContext.Current.Server.MapPath("~/images/QrCode");
                        //if (!Directory.Exists(serverPath))
                        //{
                        //    Directory.CreateDirectory(serverPath);
                        //}
                        //string FlNm = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        //fileName = Path.Combine("'https://dv9wellnes.cryptpayapi.com/images/QrCode/", FlNm+".png" );
                        //image.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
                        ////     Console.WriteLine("Image saved successfully at: " + fileName);


                        Image image = Image.FromStream(ms);

                        // Define the server path to save the image locally
                        string serverPath = HttpContext.Current.Server.MapPath("~/images/QrCode");

                        // Ensure the directory exists
                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }

                        // Generate a unique file name with a timestamp
                        string FlNm = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        string localFilePath = Path.Combine(serverPath, FlNm + ".png");

                        // Save the image locally
                        image.Save(localFilePath, System.Drawing.Imaging.ImageFormat.Png);

                        // Construct the URL to access the saved image
                        fileName = $"https://dv9wellnes.cryptpayapi.com/images/QrCode/{FlNm}.png";

                    }


                    objTrans = SC1.BeginTransaction();
                    cmd = new SqlCommand();
                    s = "Update TrnBillmain set AckNo='" + irndataa.AckNo + "',Ackdate='" + irndataa.AckDt + "',IrnNo='" + irndataa.Irn + "',SignedInvoice='" + irndataa.SignedInvoice + "'" +
                        ", Qrcode='" + irndataa.SignedQRCode + "',QrCodeImage='" + fileName + "' where UserBillNo='" + UserBillNo + "'";

                    cmd = new SqlCommand();
                    cmd.CommandText = s;
                    cmd.Connection = SC1;
                    cmd.Transaction = objTrans;

                    i = cmd.ExecuteNonQuery();


                    objTrans.Commit();
                    SC1.Close();


                }
            }
        }

        public string GetQrCode(string detail, string authNo, string userbillno)
        {
            try
            {
                using (var entity = new InventoryEntities())
                {
                    var Credentials = entity.EInvoiceCredentials
                                           .OrderByDescending(t => t.id)
                                            .FirstOrDefault();
                    // Create a request
                    string url = "	 https://gstsandbox.charteredinfo.com/eicore/dec/v1.03/Invoice?QrCodeSize=250";
                    string Data = "aspid=1674282722&password=aCHYUTA@17&Gstin=34AACCC1596Q002&user_name=TaxProEnvPON&AuthToken=" + authNo + "&QrCodeSize=250";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "POST";
                    request.ContentType = "application/json; charset=utf-8"; // Set content type to JSON
                    //request.Headers.Add("aspid", "1674282722");
                    //request.Headers.Add("Gstin", "34AACCC1596Q002");
                    //request.Headers.Add("user_name", "TaxProEnvPON");
                    //request.Headers.Add("AuthToken", authNo);
                    //request.Headers.Add("password", "aCHYUTA@17");
                    request.Headers.Add("aspid", Credentials.Aspid);
                    request.Headers.Add("Gstin", Credentials.Gstin);
                    request.Headers.Add("user_name", Credentials.Username);
                    request.Headers.Add("AuthToken", authNo);
                    request.Headers.Add("password", Credentials.ApiPassword);
                    // request.Headers.Add("QrCodeSize","250");
                    string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                    SqlConnection SC1 = new SqlConnection(InvConnectionString);
                    SqlTransaction objTrans = null;
                    // request.Headers .Add("Content-Type", "application/json; charset=utf-8");
                    //    // If the API requires headers (e.g., Authorization), add them here
                    //    // request.Headers.Add("Authorization", "Bearer YOUR_TOKEN");
                    //    // Write JSON data to request stream
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(detail);
                        streamWriter.Flush();
                    }
                    //    // Get the response
                    string s = "";
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            string result = streamReader.ReadToEnd();
                            if (SC1.State == ConnectionState.Closed)
                                SC1.Open();

                            objTrans = SC1.BeginTransaction();
                            SqlCommand cmd = new SqlCommand();
                            s = "Update  EnVoiceRequest set EResponse='" + result + "' where UserBilNo='" + userbillno + "'";

                            cmd = new SqlCommand();
                            cmd.CommandText = s;
                            cmd.Connection = SC1;
                            cmd.Transaction = objTrans;



                            int i = cmd.ExecuteNonQuery();

                            objTrans.Commit();
                            SC1.Close();
                            return result; // Optionally deserialize this JSON string to an object
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
            return null;
        }


        //public void GetQrCode(string reqStr, string authNo)
        //{
        //    try
        //    {
        //        Use modern security protocols
        //        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

        //        Define the base URL and parameters
        //        string baseUrl = "http://gstsandbox.charteredinfo.com/eicore/dec/v1.03/Invoice";
        //        var queryParams = new Dictionary<string, string>
        //{
        //    { "aspid", "1674282722" },
        //    { "Password", "aCHYUTA@17" },
        //    { "gstin", "34AACCC1596Q002" },
        //    { "user_name", "TaxProEnvPON" },
        //    { "AuthToken", authNo },
        //    { "QrCodeSize", "250" }
        //};

        //        // Build URL with parameters
        //        string urlWithParams = $"{baseUrl}?{string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"))}";

        //        // Configure RestClient and Request
        //        var client = new RestClient(new Uri(urlWithParams));
        //        var request = new RestRequest(Method.POST)
        //        {
        //            RequestFormat = DataFormat.Json
        //        };
        //        request.AddHeader("Content-Type", "application/json; charset=utf-8");
        //        request.AddParameter("application/json", reqStr, ParameterType.RequestBody);

        //        // Execute request
        //        var response = client.Execute(request);

        //        if (response == null || string.IsNullOrEmpty(response.Content))
        //        {
        //            throw new Exception($"No response or empty content. Error: {response?.ErrorMessage}");
        //        }

        //        // Deserialize response
        //        var irnResp = JsonConvert.DeserializeObject<IRNClass>(response.Content);
        //        if (irnResp?.Status != "1")
        //        {
        //            throw new Exception($"Invalid response received. Status: {irnResp?.Status}");
        //        }

        //        var irnData = JsonConvert.DeserializeObject<IRNData>(irnResp.Data?.ToString());
        //        if (irnData == null)
        //        {
        //            throw new Exception("Failed to deserialize IRN data.");
        //        }

        //        // Process QR Code
        //        if (!string.IsNullOrEmpty(irnData.QrCodeImage))
        //        {
        //            byte[] imageBytes = Convert.FromBase64String(irnData.QrCodeImage);
        //            using (var ms = new MemoryStream(imageBytes))
        //            using (var img = System.Drawing.Image.FromStream(ms))
        //            {
        //                img.Save("QR.png", System.Drawing.Imaging.ImageFormat.Png);
        //                Console.WriteLine("QR Code saved as QR.png");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in GetQrCode: {ex.Message}\n{ex.StackTrace}");
        //    }
        //}


        public async Task<ResponseDetail> SaveDistributorBill(DistributorBillModel objModel)
        {
            //Make_Inv_json("VF/WR/00099");
            ResponseDetail objResponse = new ResponseDetail();
            DistributorBillModel TempDistributor = new DistributorBillModel();
            bool IsGSTcalc = false;
            decimal maxUserSBillNo = 0;
            decimal? SessId = 0;
            string billPrefix = "";
            decimal maxSbillNo = 0;
            decimal? FsessId = 0;
            string BillSeries = "";
            string UserBillNo = "";
            string version = "";
            SqlTransaction objTrans = null;
            decimal WalletBalance = 0;
            decimal LastBillAmt = 0;
            int NewKitId = 0;
            string NewKitName = "";
            TrnPayModeDetail objDtPayModeDetail = new TrnPayModeDetail();
            List<string> Paymode = new List<string>();
            List<string> PayPrefix = new List<string>();
            List<TrnPayModeDetail> objDTListPayMode = new List<TrnPayModeDetail>();
            objResponse.ResponseMessage = "Something went wrong!";
            objResponse.ResponseStatus = "FAILED";
            string billno_ = "", narration_ = "", soldby_ = "", fcode_ = "";
            decimal netpayable_ = 0;
            string BillGSTType = "";
            try
            {
                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(AppConnectionString);
                SqlConnection SC1 = new SqlConnection(InvConnectionString);

                string query = "Select Max(SessID) as MaxSessId from M_SessnMaster";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        SessId = decimal.Parse(reader["MaxSessId"].ToString());
                    }
                }
                //SessId = SessId + 1;
                query = "Select * FROM dbo.ufnGetBalance('" + objModel.objCustomer.FormNo + "','S')";
                cmd = new SqlCommand();
                cmd.CommandText = query;
                //cmd.Parameters.AddWithValue("@IdNo", IdNo);
                cmd.Connection = SC;
                SC.Close();
                SC.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        WalletBalance = decimal.Parse(reader["Balance"].ToString());
                    }
                }

                using (var entity = new InventoryEntities())
                {
                    if (objModel.GstType == "G" && !string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                    {
                        BillGSTType = "BB.";
                    }
                    else if (objModel.GstType == "N" && !string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                    {
                        BillGSTType = "BB";
                    }
                    else if (objModel.GstType == "G" && string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                    {
                        BillGSTType = "BC.";
                    }
                    else if (objModel.GstType == "N" && string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                    {
                        BillGSTType = "BC";
                    }

                    maxSbillNo = (from result in entity.TrnBillMains select result.SBillNo).DefaultIfEmpty(0).Max();

                    maxSbillNo = maxSbillNo + 1;
                    FsessId = (from result in entity.M_FiscalMaster where result.ActiveStatus == "Y" select result.FSessId).Max();
                    BillSeries = Convert.ToString((from result in entity.M_FiscalMaster where result.ActiveStatus == "Y" select result.BillSeries).Max()).TrimEnd();
                    ////decimal? SessId = (from result in entity.M_SessnMaster select result.SessID).Max();

                    billPrefix = (from result in entity.M_ConfigMaster select result.BillPrefix).FirstOrDefault();
                    if (!string.IsNullOrEmpty(objModel.TaxORStock) && objModel.TaxORStock.ToLower() == "stock")
                    {
                        maxUserSBillNo = (from result in entity.TrnBillMains where result.FSessId == FsessId && result.SoldBy == objModel.objCustomer.UserDetails.PartyCode && result.BillType == "S" && result.BillGSTType == BillGSTType select result.UserSBillNo).DefaultIfEmpty(0).Max();

                    }
                    else
                    {
                        maxUserSBillNo = (from result in entity.TrnBillMains where result.FSessId == FsessId && result.SoldBy == objModel.objCustomer.UserDetails.PartyCode && result.BillType != "S" && result.BillGSTType == BillGSTType select result.UserSBillNo).DefaultIfEmpty(0).Max();

                    }

                    maxUserSBillNo = maxUserSBillNo + 1;
                    string strMaxUserSBillNo = maxUserSBillNo.ToString();
                    if (strMaxUserSBillNo.Count() < 5)
                    {
                        var countNum = strMaxUserSBillNo.Count();
                        var ToBeAddedDigits = 5 - countNum;
                        for (var j = 0; j < ToBeAddedDigits; j++)
                        {
                            strMaxUserSBillNo = "0" + strMaxUserSBillNo;
                        }
                    }

                    var UserPCode = (from result in entity.M_LedgerMaster where result.ActiveStatus == "Y" && result.PartyCode == objModel.objCustomer.UserDetails.PartyCode select result.UserPartyCode).FirstOrDefault();
                    //if (objModel.BillType == "party")
                    //{
                    //    if (objModel.GstType == "G")
                    //    {
                    //        if (!string.IsNullOrEmpty(objModel.TaxORStock) && objModel.TaxORStock.ToLower() == "stock")
                    //        {
                    //            UserBillNo = billPrefix + "/ST/" + UserPCode + "/" + strMaxUserSBillNo;
                    //        }
                    //        else
                    //        {
                    //            UserBillNo = billPrefix + "/" + UserPCode + "/BB." + "/" + strMaxUserSBillNo;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (!string.IsNullOrEmpty(objModel.TaxORStock) && objModel.TaxORStock.ToLower() == "stock")
                    //        {
                    //            UserBillNo = billPrefix + "/ST/" + UserPCode + "/" + strMaxUserSBillNo;
                    //        }
                    //        else
                    //        {
                    //            UserBillNo = billPrefix + "/" + UserPCode + "/BB" + "/" + strMaxUserSBillNo;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    if (objModel.GstType == "G" && !string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                    //    {
                    //        UserBillNo = billPrefix + "/" + UserPCode + "/BB." + "/" + strMaxUserSBillNo;
                    //    }
                    //    else if (objModel.GstType == "N" && !string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                    //    {
                    //        UserBillNo = billPrefix + "/" + UserPCode + "/BB" + "/" + strMaxUserSBillNo;
                    //    }
                    //    else if (objModel.GstType == "G" && string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                    //    {
                    //        UserBillNo = billPrefix + "/" + UserPCode + "/BC." + "/" + strMaxUserSBillNo;
                    //    }
                    //    else if (objModel.GstType == "N" && string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                    //    {
                    //        UserBillNo = billPrefix + "/" + UserPCode + "/BC" + "/" + strMaxUserSBillNo;
                    //    }
                    //}

                    //if (objModel.GstType == "G" && !string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                    //{
                    //    UserBillNo = billPrefix + "/" + UserPCode + "/BB." + "/" + BillSeries + "/" + strMaxUserSBillNo;
                    //}
                    //else if (objModel.GstType == "N" && !string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                    //{
                    //    UserBillNo = billPrefix + "/" + UserPCode + "/BB" + "/" + BillSeries + "/" + strMaxUserSBillNo;
                    //}
                    //else if (objModel.GstType == "G" && string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                    //{
                    //    UserBillNo = billPrefix + "/" + UserPCode + "/BC." + "/" + BillSeries + "/" + strMaxUserSBillNo;
                    //}
                    //else if (objModel.GstType == "N" && string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                    //{
                    //    UserBillNo = billPrefix + "/" + UserPCode + "/BC" + "/" + BillSeries + "/" + strMaxUserSBillNo;
                    //}
                    UserBillNo = billPrefix + "/" + UserPCode + "/" + BillSeries + "/" + strMaxUserSBillNo;
                    version = (from result in entity.M_NewHOVersionInfo select result.VersionNo).FirstOrDefault();

                    if (string.IsNullOrEmpty(objModel.BillType) || objModel.BillType == "X" || objModel.BillType == "J" || objModel.BillType == "F" || objModel.BillType == "G" || objModel.BillType == "C" || objModel.BillType == "P")
                    {
                        //saving data in table
                        // decimal? SessId=(from result in entity)
                        bool IsWalletEntry = false;
                        string tempBillNo = "";
                        tempBillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo;
                        //if (objModel.GstType == "G" && !string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                        //{
                        //    tempBillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/BB." + "/" + maxSbillNo;
                        //    IsGSTcalc = true;
                        //}
                        //else if (objModel.GstType == "N" && !string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                        //{
                        //    tempBillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/BB" + "/" + maxSbillNo;
                        //}
                        //else if (objModel.GstType == "G" && string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                        //{
                        //    tempBillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/BC." + "/" + maxSbillNo;
                        //    IsGSTcalc = true;
                        //}
                        //else if (objModel.GstType == "N" && string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                        //{
                        //    tempBillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/BC" + "/" + maxSbillNo;
                        //}
                        if (objModel != null)
                        {
                            if (objModel.objProduct.PayDetails != null)
                            {
                                if (objModel.objProduct.PayDetails.IsBD)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.BankDeposit;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail
                                    {
                                        BillAmt = objModel.objProduct.TotalNetPayable,
                                        SoldBy = objModel.objCustomer.UserDetails.PartyCode,
                                        BillDate = DateTime.Now.Date,
                                        BillType = objModel.objCustomer.SelectedInvoiceType,
                                        BillNo = tempBillNo,
                                        PayPrefix = value,
                                        Amount = objModel.objProduct.PayDetails.AmountByBD,
                                        BankCode = 0,
                                        ChqDDDate = null,
                                        ChqDDNo = "",
                                        CardNo = "",
                                        Narration = "",
                                        DUserId = 0,
                                        DRecTimeStamp = null,
                                        BankName = objModel.objProduct.PayDetails.BDBankName,
                                        AcNo = objModel.objProduct.PayDetails.AccNo,
                                        IFSCode = objModel.objProduct.PayDetails.IFSCCode,
                                        ActiveStatus = "Y",
                                        RecTimeStamp = DateTime.Now,
                                        UserId = objModel.objCustomer.UserDetails.UserId,
                                        Version = version,
                                        UserName = objModel.objCustomer.UserDetails.UserName,
                                        FSessId = FsessId ?? 0,
                                        SBillNo = maxSbillNo
                                    });


                                }
                                if (objModel.objProduct.PayDetails.IsCC)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Card;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail
                                    {
                                        BillAmt = objModel.objProduct.TotalNetPayable,
                                        SoldBy = objModel.objCustomer.UserDetails.PartyCode,
                                        BillDate = DateTime.Now.Date,
                                        BillType = objModel.objCustomer.SelectedInvoiceType,
                                        BillNo = tempBillNo,
                                        PayPrefix = value,
                                        AcNo = "",
                                        IFSCode = "",
                                        BankCode = 0,
                                        Narration = "",
                                        BankName = "",
                                        DUserId = 0,
                                        DRecTimeStamp = null,
                                        ChqDDNo = "",
                                        ChqDDDate = null,
                                        Amount = objModel.objProduct.PayDetails.AmountByCard,
                                        CardNo = objModel.objProduct.PayDetails.CardNo,
                                        ActiveStatus = "Y",
                                        RecTimeStamp = DateTime.Now,
                                        UserId = objModel.objCustomer.UserDetails.UserId,
                                        Version = version,
                                        UserName = objModel.objCustomer.UserDetails.UserName,
                                        FSessId = FsessId ?? 0,
                                        SBillNo = maxSbillNo
                                    });
                                }
                                if (objModel.objProduct.PayDetails.IsQ)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cheque;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByCheque, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.CHBankName, ChqDDNo = objModel.objProduct.PayDetails.ChequeNo, ChqDDDate = objModel.objProduct.PayDetails.ChequeDate, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsD)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.DD;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByDD, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.DDBankName, ChqDDNo = objModel.objProduct.PayDetails.DDNo, ChqDDDate = objModel.objProduct.PayDetails.DDDate, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsT)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Credit;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = tempBillNo, PayPrefix = value, BankName = "", Amount = objModel.objProduct.PayDetails.AmountByCredit, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, DUserId = 0, DRecTimeStamp = null, ChqDDDate = null, ChqDDNo = "", Narration = objModel.objProduct.PayDetails.Narration, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsV)
                                {
                                    if (objModel.objProduct.PayDetails.AmountByVoucher > 0)
                                    {
                                        billno_ = tempBillNo;
                                        var fpsql = "update FPVoucher set Isuse=1,BillNo='" + billno_ + "' where Code='" + objModel.objProduct.PayDetails.FpVoucher + "' and IdNo='" + objModel.objCustomer.IdNo + "'";

                                        //narration_ = billno_ + " against F.P. Voucher adjust " + (objModel.objProduct.PayDetails.AmountByVoucher);


                                        if (SC1.State == ConnectionState.Closed)
                                            SC1.Open();
                                        objTrans = SC1.BeginTransaction();
                                        cmd = new SqlCommand();
                                        cmd.CommandText = fpsql;
                                        cmd.Connection = SC1;
                                        cmd.Transaction = objTrans;



                                        int i = cmd.ExecuteNonQuery();

                                        objTrans.Commit();
                                        SC1.Close();
                                    }
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Voucher;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByVoucher, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, DUserId = 0, DRecTimeStamp = null, ChqDDDate = null, ChqDDNo = "", Narration = "", BankName = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsCU)
                                {
                                    if (objModel.objProduct.PayDetails.AmountbyCoupon > 0)
                                    {
                                        billno_ = tempBillNo;
                                        var fpsql = "update Coupon set Isuse=1,BillNo='" + billno_ + "' where Code='" + objModel.objProduct.PayDetails.Coupon + "' and IdNo='" + objModel.objCustomer.IdNo + "'";

                                        if (SC1.State == ConnectionState.Closed)
                                            SC1.Open();
                                        objTrans = SC1.BeginTransaction();
                                        cmd = new SqlCommand();
                                        cmd.CommandText = fpsql;
                                        cmd.Connection = SC1;
                                        cmd.Transaction = objTrans;



                                        int i = cmd.ExecuteNonQuery();

                                        objTrans.Commit();
                                        SC1.Close();
                                    }
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Coupon;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = "V", BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountbyCoupon, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, DUserId = 0, DRecTimeStamp = null, ChqDDDate = null, ChqDDNo = "", Narration = "", BankName = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsW)
                                {
                                    if (objModel.objProduct.PayDetails.AmountByWallet > 0)
                                    {
                                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Wallet;
                                        string value = EnumPayModes.GetEnumDescription(enumVar);
                                        PayPrefix.Add(value);
                                        objDTListPayMode.Add(new TrnPayModeDetail
                                        {
                                            BillAmt = objModel.objProduct.TotalNetPayable,
                                            SoldBy = objModel.objCustomer.UserDetails.PartyCode,
                                            BillDate = DateTime.Now.Date,
                                            BillType = objModel.objCustomer.SelectedInvoiceType,
                                            BillNo = tempBillNo,
                                            PayPrefix = value,
                                            Amount = objModel.objProduct.PayDetails.AmountByWallet,
                                            BankCode = 0,
                                            BankName = "",
                                            AcNo = "",
                                            IFSCode = "",
                                            Narration = "",
                                            DUserId = 0,
                                            DRecTimeStamp = null,
                                            ChqDDNo = "",
                                            ChqDDDate = null,
                                            CardNo = objModel.objCustomer.CardNo,
                                            ActiveStatus = "Y",
                                            RecTimeStamp = DateTime.Now,
                                            UserId = objModel.objCustomer.UserDetails.UserId,
                                            Version = version,
                                            UserName = objModel.objCustomer.UserDetails.UserName,
                                            FSessId = FsessId ?? 0,
                                            SBillNo = maxSbillNo
                                        });
                                        ////insert entry into couponsalesdetails for wallet
                                        IsWalletEntry = true;
                                        var walletquery = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                                        "Select ISNULL(Max(VoucherNo),0)+1, Cast(Convert(varchar,Getdate(),106) as Datetime),'','" + objModel.objCustomer.UserDetails.PartyCode + "','" + objModel.objProduct.PayDetails.AmountByWallet + "','Wallet credit against bill " + UserBillNo + ".','" + tempBillNo + "','Z','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher";
                                        //" UNION ALL Select ISNULL(Max(VoucherNo),0)+2, Cast(Convert(varchar,Getdate(),106) as Datetime),'','" + objModel.objCustomer.PartyCode + "','" + cashAmt + "','Wallet credited against cash in bill " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher " +
                                        //" UNION ALL Select ISNULL(Max(VoucherNo),0)+3, Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.PartyCode + "','','" + cashAmt + "','Wallet debited against bill " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher; ";


                                        if (SC1.State == ConnectionState.Closed)
                                            SC1.Open();
                                        objTrans = SC1.BeginTransaction();
                                        cmd = new SqlCommand();
                                        cmd.CommandText = walletquery;
                                        cmd.Connection = SC1;
                                        cmd.Transaction = objTrans;

                                        int i = cmd.ExecuteNonQuery();

                                        objTrans.Commit();
                                        SC1.Close();

                                    }

                                    //if (WalletBalance >= objModel.objProduct.PayDetails.AmountByWallet)
                                    //{
                                    //    if (SC.State == ConnectionState.Closed)
                                    //        SC.Open();
                                    //    SC.Open();
                                    //    objTrans = SC.BeginTransaction();
                                    //    query = "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,AcType,VType,SessID,WSEssID) " +
                                    //            "Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.FormNo + "','0','" + objModel.objProduct.PayDetails.AmountByWallet + "','Product purchased Against " + tempBillNo + "','R','D','" + SessId + "','" + SessId + "' FROM TrnVoucher";
                                    //    cmd = new SqlCommand();
                                    //    cmd.CommandText = query;
                                    //    cmd.Connection = SC;
                                    //    cmd.Transaction = objTrans;



                                    //    int i = cmd.ExecuteNonQuery();

                                    //    objTrans.Commit();
                                    //    SC.Close();
                                    //    if (i > 0)
                                    //    {
                                    //        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Wallet;
                                    //        string value = EnumPayModes.GetEnumDescription(enumVar);
                                    //        PayPrefix.Add(value);
                                    //        objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByWallet, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, CardNo = objModel.objCustomer.CardNo, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                    //        ////insert entry into couponsalesdetails for wallet
                                    //        IsWalletEntry = true;
                                    //    }
                                    //    else
                                    //    {
                                    //        objResponse.ResponseStatus = "FAILED";
                                    //        objResponse.ResponseMessage = "Something went wrong";
                                    //        return objResponse;
                                    //    }
                                    //    i = 0;
                                    //}
                                    //else
                                    //{
                                    //    objResponse.ResponseStatus = "FAILED";
                                    //    objResponse.ResponseMessage = "Sorry!Insufficient Wallet Balance.";
                                    //    return objResponse;
                                    //}

                                }
                                if (objModel.objProduct.PayDetails.IsP)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Paytm;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByPaytm, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = objModel.objProduct.PayDetails.PaytmTransactionId, ChqDDDate = DateTime.Now, CardNo = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }


                            }

                            if (objModel.objProduct.CashAmount > 0)
                            {
                                EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cash;
                                string value = EnumPayModes.GetEnumDescription(enumVar);
                                PayPrefix.Add(value);
                                objDTListPayMode.Add(new TrnPayModeDetail
                                {
                                    BillAmt = objModel.objProduct.TotalNetPayable,
                                    SoldBy = objModel.objCustomer.UserDetails.PartyCode,
                                    BillDate = DateTime.Now.Date,
                                    BillType = objModel.objCustomer.SelectedInvoiceType,
                                    BillNo = tempBillNo,
                                    PayPrefix = value,
                                    Amount = objModel.objProduct.CashAmount,
                                    BankCode = 0,
                                    BankName = "",
                                    AcNo = "",
                                    IFSCode = "",
                                    Narration = "",
                                    DUserId = 0,
                                    DRecTimeStamp = null,
                                    ChqDDNo = "",
                                    ChqDDDate = null,
                                    CardNo = "",
                                    ActiveStatus = "Y",
                                    RecTimeStamp = DateTime.Now,
                                    UserId = objModel.objCustomer.UserDetails.UserId,
                                    Version = version,
                                    UserName = objModel.objCustomer.UserDetails.UserName,
                                    FSessId = FsessId ?? 0,
                                    SBillNo = maxSbillNo
                                });
                            }
                            if (PayPrefix.Count > 0)
                            {

                                Paymode = (from r in entity.M_PayModeMaster where PayPrefix.Contains(r.Prefix) select r.PayMode).ToList();
                            }
                        }

                        string SoldByCode = "";
                        List<TrnBillData> tempTableList = new List<TrnBillData>();
                        try
                        {
                            List<ProductModel> objListProductModel = new List<ProductModel>();
                            //TempDistributor.objListProduct.AddRange(objModel.objListProduct);
                            foreach (var obj in objModel.objListProduct)
                            {
                                objListProductModel.Add(obj);
                                TrnBillData objDTBillData = new TrnBillData();
                                objDTBillData.SBillNo = maxSbillNo;
                                objDTBillData.FSessId = FsessId ?? 0;
                                objDTBillData.SessId = SessId ?? 0;
                                objDTBillData.ActiveStatus = "Y";
                                objDTBillData.BillDate = DateTime.Now.Date;

                                objDTBillData.RefNo = "";
                                objDTBillData.RefId = 0;
                                objDTBillData.RefName = "";
                                objDTBillData.Remarks = string.IsNullOrEmpty(objModel.objCustomer.Remarks) ? "" : objModel.objCustomer.Remarks;
                                objDTBillData.CType = "M";
                                objDTBillData.SoldBy = objModel.objCustomer.UserDetails.PartyCode;
                                SoldByCode = objDTBillData.SoldBy;
                                objDTBillData.BillBy = objDTBillData.SoldBy;
                                objDTBillData.BillNo = tempBillNo;
                                objDTBillData.FType = "M";
                                objDTBillData.FCode = objModel.objCustomer.IdNo;
                                objDTBillData.PartyName = objModel.objCustomer.Name;
                                objDTBillData.SupplierId = 0;
                                objDTBillData.ChDDNo = 0;
                                objDTBillData.ChDate = DateTime.Now;
                                objDTBillData.ChAmt = objModel.objProduct.CourierCharges;
                                objDTBillData.BankCode = 0;
                                objDTBillData.BankName = "";
                                objDTBillData.FormNo = objModel.objCustomer.FormNo;
                                objDTBillData.TotalTaxAmount = objModel.objProduct.TotalTaxAmount;
                                objDTBillData.TotalSTaxAmount = 0;
                                objDTBillData.TotalDiscount = objModel.objProduct.TotalDiscount;
                                objDTBillData.TotalKitBvValue = 0;
                                objDTBillData.TotalBvValue = objModel.objProduct.TotalBV;
                                objDTBillData.TotalCVValue = objModel.objProduct.TotalCV;
                                objDTBillData.TotalPVValue = objModel.objProduct.TotalPV;
                                objDTBillData.TotalRPValue = objModel.objProduct.TotalRP;
                                objDTBillData.DP = obj.DP ?? 0;
                                objDTBillData.RP = obj.RP ?? 0;
                                objDTBillData.MRP = obj.MRP ?? 0;
                                objDTBillData.CVValue = obj.CVValue ?? 0;
                                objDTBillData.CV = obj.CV ?? 0;
                                objDTBillData.PV = obj.PV ?? 0;
                                objDTBillData.BV = obj.BV ?? 0;
                                objDTBillData.BVValue = obj.BVValue ?? 0;
                                objDTBillData.PVValue = obj.PVValue ?? 0;
                                objDTBillData.RPValue = obj.RPValue ?? 0;
                                objDTBillData.Barcode = obj.Barcode.ToString();
                                objDTBillData.BatchNo = obj.BatchNo.ToString();

                                //if (objModel.objCustomer.StateCode == objModel.objCustomer.UserDetails.StateCode)
                                //{
                                if (IsGSTcalc == true)
                                {
                                    if (objModel.objCustomer.StateCode != objModel.objCustomer.UserDetails.StateCode)
                                    {
                                        objDTBillData.TaxAmount = obj.TaxAmt ?? 0;
                                        if (obj.OldTaxAmount != 0 && obj.OldTaxAmount != obj.TaxAmt)
                                        {
                                            objDTBillData.TaxAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.TaxAmount) + 0.01).ToString());
                                            objDTBillData.NetAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.NetAmount) - 0.01).ToString());
                                        }
                                        objDTBillData.Tax = obj.TaxPer ?? 0;
                                        objDTBillData.CGST = 0;
                                        objDTBillData.CGSTAmt = 0;
                                        objDTBillData.SGST = 0;
                                        objDTBillData.SGSTAmt = 0;
                                        objDTBillData.TaxType = "I";
                                    }
                                    else
                                    {
                                        objDTBillData.TaxAmount = 0;
                                        objDTBillData.Tax = 0;
                                        objDTBillData.CGST = obj.TaxPer / 2 ?? 0;
                                        objDTBillData.CGSTAmt = obj.TaxAmt / 2 ?? 0;
                                        objDTBillData.SGST = obj.TaxPer / 2 ?? 0;
                                        objDTBillData.SGSTAmt = obj.TaxAmt / 2 ?? 0;
                                        objDTBillData.TaxType = "S";
                                    }
                                }
                                else
                                {
                                    objDTBillData.TaxAmount = 0;
                                    objDTBillData.Tax = 0;
                                    objDTBillData.CGST = 0;
                                    objDTBillData.CGSTAmt = 0;
                                    objDTBillData.SGST = 0;
                                    objDTBillData.SGSTAmt = 0;
                                    objDTBillData.TaxType = "N";
                                }

                                //}
                                //else
                                //{
                                //    objDTBillData.TaxAmount = obj.TaxAmt ?? 0;
                                //    if (obj.OldTaxAmount != 0 && obj.OldTaxAmount != obj.TaxAmt)
                                //    {
                                //        objDTBillData.TaxAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.TaxAmount) + 0.01).ToString());
                                //        objDTBillData.NetAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.NetAmount) - 0.01).ToString());
                                //    }
                                //    objDTBillData.Tax = obj.TaxPer ?? 0;
                                //    objDTBillData.CGST = 0;
                                //    objDTBillData.CGSTAmt = 0;
                                //    objDTBillData.SGST = 0;
                                //    objDTBillData.SGSTAmt = 0;
                                //    objDTBillData.TaxType = "I";
                                //}

                                objDTBillData.DiscountPer = obj.DiscPer ?? 0;
                                objDTBillData.Discount = obj.DiscAmt ?? 0;
                                objDTBillData.ProdCommssn = obj.CommissionPer ?? 0;
                                objDTBillData.ProdCommssnAmt = obj.CommissionAmt ?? 0;
                                objDTBillData.ProductId = obj.ProdCode.ToString();
                                objDTBillData.ProductName = obj.ProductName;
                                objDTBillData.Qty = obj.Quantity;
                                objDTBillData.FreeQty = obj.FreeQty;
                                if (!string.IsNullOrEmpty(obj.ProductTye) && obj.ProductTye.ToUpper() == "F")
                                {
                                    objDTBillData.TFreeQty = 0;
                                }
                                else
                                {
                                    objDTBillData.TFreeQty = obj.TFreeQty;
                                }
                                objDTBillData.Rate = obj.Rate ?? 0;
                                objDTBillData.IsKitBV = "N";
                                objDTBillData.DSeries = "";
                                objDTBillData.DImported = "N";
                                objDTBillData.IMEINo = "";
                                objDTBillData.BNo = "";
                                objDTBillData.ItemType = "";



                                objDTBillData.JType = "Cash:" + objModel.objProduct.TotalNetPayable;
                                objDTBillData.BillTo = "R";
                                objDTBillData.BillFor = "RB";
                                objDTBillData.IsReceive = "R";
                                objDTBillData.IsCredit = "F";
                                //objDTBillData.BillType = "R";
                                //if (objModel.objCustomer.IsFirstBill)
                                //{
                                //    objDTBillData.BillType = (objModel.objProduct.VoucherNo ?? "") != "" ? objModel.BillType : "B";
                                //}
                                //else
                                //{
                                //    if (string.IsNullOrEmpty(objModel.BillType))
                                //        objDTBillData.BillType = (objModel.objProduct.VoucherNo ?? "") != "" ? objModel.BillType : "R";
                                //    else
                                //        objDTBillData.BillType = objModel.BillType;
                                //}

                                if (objModel.objCustomer.SelectedInvoiceType == "BV")
                                {
                                    objDTBillData.BillType = "B";
                                }
                                else if (objModel.objCustomer.SelectedInvoiceType == "PV")
                                {
                                    objDTBillData.BillType = "P";
                                }

                                if (!string.IsNullOrEmpty(obj.ProductTye))
                                {
                                    objDTBillData.ProdType = obj.ProductTye;
                                }
                                else
                                {
                                    objDTBillData.ProdType = "P";
                                }
                                objDTBillData.PaymentDtl = "Cash:" + objModel.objProduct.TotalNetPayable;

                                objDTBillData.TotalAmount = objModel.objProduct.TotalTotalAmount;
                                //tax excluding
                                objDTBillData.NetAmount = obj.Amount;
                                objDTBillData.CashDiscPer = obj.CashDiscPer;
                                objDTBillData.CashDiscAmount = obj.CashDiscAmount;

                                objDTBillData.NetPayable = Math.Round(objModel.objProduct.TotalNetPayable);
                                if (objModel.objProduct.Roundoff == 0)
                                {
                                    objDTBillData.RndOff = objDTBillData.NetPayable - objModel.objProduct.TotalNetPayable;
                                }
                                else
                                {
                                    objDTBillData.RndOff = objModel.objProduct.Roundoff;
                                }
                                objDTBillData.CardAmount = 0;
                                objDTBillData.PayMode = Paymode.Count > 1 ? string.Join(",", Paymode) : Paymode[0];
                                objDTBillData.PayPrefix = PayPrefix.Count > 1 ? string.Join(",", PayPrefix) : PayPrefix[0];
                                objDTBillData.BvTransfer = "N";

                                //objDTBillData.UserSBillNo = maxSbillNo;
                                //objDTBillData.UserBillNo = billPrefix + "/" + objDTBillData.BillBy + "/" + maxSbillNo;
                                objDTBillData.UserSBillNo = maxUserSBillNo;
                                objDTBillData.UserBillNo = UserBillNo;
                                objDTBillData.DispatchStatus = "N";
                                objDTBillData.LR = "0";
                                objDTBillData.LRDate = DateTime.Now;
                                objDTBillData.TransporterName = "";
                                objDTBillData.DispatchTo = objModel.objCustomer.IdNo;
                                objDTBillData.FreightType = "";
                                objDTBillData.Series = "";
                                objDTBillData.Scratch = "";
                                //if (objModel.objCustomer.IsBillOnMrp)
                                //{
                                //    objDTBillData.Unit = 1;
                                //}
                                //else
                                //{
                                objDTBillData.Unit = 0;
                                // objDTBillData.LocId = objModel.objCustomer.KitId;
                                //}
                                objDTBillData.PSessId = 0;
                                objDTBillData.DcNo = objModel.objProduct.VoucherNo ?? "";
                                objDTBillData.Imported = "N";
                                objDTBillData.FPoint = 0;
                                objDTBillData.FPointValue = 0;
                                objDTBillData.OrdStatus = "";
                                objDTBillData.OrdQty = 0;
                                // objDTBillData.OrderType = "";
                                objDTBillData.OrderDate = DateTime.Now;
                                objDTBillData.OrderNo = "";
                                objDTBillData.RemQty = 0;
                                objDTBillData.DP1 = 0;
                                objDTBillData.DReason = "";
                                objDTBillData.DUserId = 0;
                                objDTBillData.DRecTimeStamp = DateTime.Now;
                                objDTBillData.DocWeight = objModel.objProduct.TotalWeight;
                                objDTBillData.DocketNo = "";
                                objDTBillData.DocketDate = DateTime.Now;
                                objDTBillData.STNFormNo = "";
                                objDTBillData.StkRecv = "N";
                                objDTBillData.StkRecvDate = DateTime.Now;
                                objDTBillData.StkRecvUserId = 0;
                                objDTBillData.InTransit = "N";
                                objDTBillData.UID = string.IsNullOrEmpty(objModel.objProduct.UID) ? "" : objModel.objProduct.UID;
                                objDTBillData.OfferUID = objModel.offerId;
                                objDTBillData.IsKit = "N";
                                objDTBillData.TotalCorton = "";
                                objDTBillData.TotalMonoCorton = "";
                                objDTBillData.SpclOfferId = 0;
                                objDTBillData.VAT = 0;
                                objDTBillData.BuyerAddress = "";
                                objDTBillData.BuyerTIN = "";

                                objDTBillData.TotalDiscount = objModel.objProduct.TotalDiscPer;
                                objDTBillData.TotalDiscountAmt = objModel.objProduct.TotalDiscount;
                                objDTBillData.VDiscountAmt = objModel.objProduct.VDiscountAmt ?? 0;
                                objDTBillData.VDiscount = 0;
                                objDTBillData.ReceiverID = "";
                                objDTBillData.ReceiverName = "";
                                objDTBillData.ReceiverMNo = "";
                                objDTBillData.ReceiverIDProof = "";
                                objDTBillData.TotalFPoint = 0;
                                objDTBillData.TotalQty = objModel.objProduct.TotalQty;
                                objDTBillData.CashReward = objModel.objProduct.VoucherAmt ?? 0;
                                objDTBillData.CommssnAmt = objModel.objProduct.TotalCommsonAmt;
                                objDTBillData.RecvAmount = 0;
                                objDTBillData.ReturnToCustAmt = 0;
                                objDTBillData.ActiveStatus = "Y";
                                objDTBillData.RecTimeStamp = DateTime.Now;
                                objDTBillData.UserId = objModel.objCustomer.UserDetails.UserId;
                                objDTBillData.UserName = objModel.objCustomer.UserDetails.UserName;
                                objDTBillData.DelvPlace = string.IsNullOrEmpty(objModel.objProduct.DeliveryPlace) ? "" : objModel.objProduct.DeliveryPlace;
                                objDTBillData.DelvStatus = "";
                                objDTBillData.DelvUserId = 0;
                                objDTBillData.DelvRecTimeStamp = DateTime.Now;
                                objDTBillData.Version = version;
                                objDTBillData.IDType = "";
                                objDTBillData.BranchName = "";
                                objDTBillData.CourierId = 0;
                                objDTBillData.CourierName = "";
                                objDTBillData.LocId = 0;
                                objDTBillData.LocName = "";
                                objDTBillData.DelvAddress = "";
                                objDTBillData.Pincode = "";
                                objDTBillData.OrderType = "";
                                objDTBillData.Coupon = "";
                                objDTBillData.CouponAmount = 0;
                                objDTBillData.PaidBV = 0;
                                objDTBillData.IRNNo = "";
                                objDTBillData.AckNo = "";
                                objDTBillData.AckDate = DateTime.Now;
                                objDTBillData.QrCodeimage = "";
                                objDTBillData.QrCode = "";
                                objDTBillData.SignedInvoice = "";
                                objDTBillData.BillGSTType = BillGSTType;
                                objDTBillData.InvoiceType = "GST";
                                // objDTBillData.ReceiverMNo = "";
                                //objDTBillData.DSeries = "";
                                //objDTBillData.DImported = "";
                                // objDTBillData.FCode = string.IsNullOrEmpty(objModel.objCustomer.IdNo) ? "" : objModel.objCustomer.IdNo;
                                //objDTBillData.FormNo = objModel.objCustomer.FormNo;
                                //objDTBillData.RefId = string.IsNullOrEmpty(objModel.objCustomer.ReferenceIdNo)?0:decimal.Parse(objModel.objCustomer.ReferenceIdNo);
                                //objDTBillData.RefName= string.IsNullOrEmpty(objModel.objCustomer.ReferenceName) ? "" : objModel.objCustomer.ReferenceName;
                                //objDTBillData.Remarks = "";
                                // tempTableList.Add(objDTBillData);
                                billno_ = objDTBillData.BillNo;
                                if (objModel.UserType == "shoppe")
                                    soldby_ = objDTBillData.SoldBy;
                                else
                                    soldby_ = objDTBillData.UserName;
                                fcode_ = objDTBillData.FCode;
                                netpayable_ = objDTBillData.NetPayable;
                                narration_ = "Wallet deducted against " + objDTBillData.UserBillNo + ".";
                                entity.TrnBillDatas.Add(objDTBillData);
                            }
                            int i = 0;

                            using (var objDTTrans = entity.Database.BeginTransaction())
                            {
                                //entity.TrnBillDatas.AddRange(tempTableList);
                                try
                                {
                                    i = entity.SaveChanges();
                                    objDTTrans.Commit();
                                }
                                catch (DbUpdateConcurrencyException ex)
                                {
                                    objDTTrans.Rollback();
                                }
                                catch (DbUpdateException ex)
                                {

                                }

                                catch (Exception ex)
                                {
                                    objDTTrans.Rollback();
                                }
                            }
                            if (i == objModel.objListProduct.Count)
                            {
                                DeductPartyWallet(billno_, narration_, soldby_, fcode_, netpayable_, objModel.UserType);
                                if (!string.IsNullOrEmpty(objModel.DeliveryBy) && objModel.DeliveryBy.ToLower() == "pickup")
                                {
                                    var amount = objModel.objProduct.CourierCharges / 2;
                                    AddShoppeWallet(billno_, "Against Couriercharges for bill no: " + UserBillNo, fcode_, soldby_, amount);
                                }

                                var resultPayMode = (from r in entity.M_PayModeMaster select r).ToList();
                                foreach (var obj in objDTListPayMode)
                                {
                                    TrnPayModeDetail objTemp = new TrnPayModeDetail();
                                    objTemp = obj;
                                    if (objModel.objCustomer.IsFirstBill)
                                    {
                                        objTemp.BillType = "B";
                                    }
                                    else
                                    {
                                        objTemp.BillType = "R";
                                    }
                                    objTemp.PayMode = (from r in resultPayMode where r.Prefix.Trim() == obj.PayPrefix.Trim() select r.PayMode).FirstOrDefault();
                                    if (string.IsNullOrEmpty(objTemp.CardNo))
                                    {
                                        objTemp.CardNo = "";
                                    }
                                    entity.TrnPayModeDetails.Add(objTemp);
                                }
                                i = 0;
                                i = entity.SaveChanges();
                                if (i == objDTListPayMode.Count)
                                {
                                    if (objModel.objProduct.PayDetails.AmountByVoucher > 0)
                                    {
                                        string voucherno = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                        var fpvwallet = new FPVucherWallet
                                        {
                                            Idno = objModel.objCustomer.IdNo,
                                            Remark = "FPV debit against bill " + billno_,
                                            credit = 0,
                                            debit = objModel.objProduct.PayDetails.AmountByVoucher,
                                            Vtype = "F",
                                            VoucherNo = voucherno,
                                            BillNo = billno_,
                                            RectimeStamp = DateTime.Now
                                        };
                                        entity.FPVucherWallets.Add(fpvwallet);
                                        entity.SaveChanges();

                                        var fpVoucherUsed = new FPVoucherUsed
                                        {
                                            IDno = objModel.objCustomer.IdNo,
                                            BillNo = billno_,
                                            AdjustAmount = objModel.objProduct.PayDetails.AmountByVoucher,
                                            FPVucherWallet_id = fpvwallet.id,
                                            RectimeStamp = DateTime.Now
                                        };
                                        entity.FPVoucherUseds.Add(fpVoucherUsed);
                                        entity.SaveChanges();
                                    }

                                    //hit api 
                                    //string result = "";
                                    //string detail = "", apierror = "";
                                    //try
                                    //{
                                    //    string Bvvalue = "0";
                                    //    string Pvvalue = "0";
                                    //    string fpamt = "0";
                                    //    string voucheramt = "0";
                                    //    if (objModel.objProduct.PayDetails.AmountByVoucher > 0)
                                    //    {
                                    //        fpamt = Convert.ToString(objModel.objProduct.PayDetails.AmountByVoucher);
                                    //    }
                                    //    if (objModel.objProduct.PayDetails.AmountByWallet > 0)
                                    //    {
                                    //        voucheramt = Convert.ToString(objModel.objProduct.PayDetails.AmountByWallet);
                                    //    }

                                    //    if (objModel.objCustomer.SelectedInvoiceType == "BV")
                                    //    {
                                    //        decimal totalBV = Convert.ToDecimal(objModel.objProduct.TotalBV);
                                    //        decimal voucherAmount = Convert.ToDecimal(objModel.objProduct.PayDetails.AmountByVoucher);

                                    //        if (voucherAmount > 0)
                                    //        {
                                    //            decimal calculatedBV = totalBV - (voucherAmount / 2);

                                    //            // Prevent negative value
                                    //            Bvvalue = Convert.ToString(calculatedBV < 0 ? 0 : calculatedBV);
                                    //        }
                                    //        else
                                    //        {
                                    //            Bvvalue = Convert.ToString(totalBV);
                                    //        }
                                    //    }
                                    //    else if (objModel.objCustomer.SelectedInvoiceType == "PV")
                                    //    {
                                    //        Pvvalue = Convert.ToString(objModel.objProduct.TotalPV);
                                    //    }

                                    //    var reqobj = new
                                    //    {
                                    //        act = "INSERTPVBV",
                                    //        uid = Convert.ToString(objModel.objCustomer.IdNo),
                                    //        logkey = "ZoewellnessgugddkhJJHJsddd",
                                    //        billno = UserBillNo,
                                    //        billdate = DateTime.Now.Date.ToString("dd-MMM-yyyy").ToUpper(),
                                    //        bv = Bvvalue,
                                    //        partycode = "WR",
                                    //        PV = Pvvalue,
                                    //        billamount = Convert.ToString(objModel.objProduct.TotalNetPayable),
                                    //        fpamt = fpamt,
                                    //        voucheramt = voucheramt
                                    //    };

                                    //    detail = JsonConvert.SerializeObject(reqobj);
                                    //    // Create a request
                                    //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://zoewellness.co.in/api/api.ashx");
                                    //    request.Method = "POST";
                                    //    request.ContentType = "application/json"; // Set content type to JSON
                                    //                                              // If the API requires headers (e.g., Authorization), add them here
                                    //                                              // request.Headers.Add("Authorization", "Bearer YOUR_TOKEN");
                                    //                                              // Write JSON data to request stream
                                    //    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                                    //    {
                                    //        streamWriter.Write(detail);
                                    //        streamWriter.Flush();
                                    //    }
                                    //    // Get the response
                                    //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                                    //    {
                                    //        using (var streamReader = new StreamReader(response.GetResponseStream()))
                                    //        {
                                    //            result = streamReader.ReadToEnd();
                                    //            //return result; // Optionally deserialize this JSON string to an object
                                    //        }
                                    //    }

                                    //    InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                                    //    SqlConnection SCapi = new SqlConnection(InvConnectionString);

                                    //    string queryapi = "exec Sp_InsertApiRequestResponse @UserBillNo='" + UserBillNo + "',@ApiURL='https://zoewellness.co.in/api/api.ashx',@Request='" + detail + "',@Response='" + result + "',@Error='" + apierror + "'";
                                    //    if (SCapi.State == ConnectionState.Closed)
                                    //        SCapi.Open();
                                    //    SqlCommand cmdapi = new SqlCommand();
                                    //    cmdapi.CommandText = queryapi;
                                    //    cmdapi.Connection = SCapi;
                                    //    i = cmdapi.ExecuteNonQuery();
                                    //    if (SCapi.State == ConnectionState.Open)
                                    //        SCapi.Close();

                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //    apierror = ex.Message;
                                    //    InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                                    //    SqlConnection SCapi = new SqlConnection(InvConnectionString);
                                    //    string queryapi = "exec Sp_InsertApiRequestResponse @UserBillNo='" + UserBillNo + "',@ApiURL='https://zoewellness.co.in/api/api.ashx',@Request='" + detail + "',@Response='" + result + "',@Error='" + apierror + "'";
                                    //    if (SCapi.State == ConnectionState.Closed)
                                    //        SCapi.Open();
                                    //    SqlCommand cmdapi = new SqlCommand();
                                    //    cmdapi.CommandText = queryapi;
                                    //    cmdapi.Connection = SCapi;
                                    //    i = cmdapi.ExecuteNonQuery();
                                    //    if (SCapi.State == ConnectionState.Open)
                                    //        SCapi.Close();
                                    //    //var message = ex.Message;
                                    //}


                                    if (objModel.EInvoice == "Y")
                                    { Make_Inv_json(UserBillNo); }
                                    objResponse.ResponseMessage = "Saved Successfully!";
                                    objResponse.ResponseStatus = "OK";
                                    await SendInvoiceSMS(objModel.objCustomer.MobileNo, objModel.objProduct.TotalNetPayable.ToString(), objModel.objProduct.TotalRP.ToString(), objModel.objProduct.TotalBV.ToString());
                                    objResponse.ResponseDetailsToPrint = new DistributorBillModel();
                                    objResponse.ResponseDetailsToPrint.BillNo = UserBillNo;
                                    objResponse.ResponseDetailsToPrint.SoldBy = SoldByCode;
                                }
                            }
                        }
                        catch (DbEntityValidationException e)
                        {
                            objResponse.ResponseMessage = "Something went wrong!";
                            objResponse.ResponseStatus = "FAILED";
                        }

                    }
                    else if (objModel.BillType == "party")
                    {
                        //saving party bill
                        DateTime BillDate = DateTime.Now;
                        if (!string.IsNullOrEmpty(objModel.BillDateStr))
                        {
                            var SplitDate = objModel.BillDateStr.Split('-');
                            string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                            var NewDate1 = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                            BillDate = Convert.ToDateTime(NewDate1);
                            BillDate = BillDate.Date;
                        }




                        //saving data in table
                        // decimal? SessId=(from result in entity)
                        bool IsWalletEntry = false;
                        string tempBillNo = "";
                        tempBillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo;
                        //if (objModel.GstType == "G" && !string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                        //{
                        //    tempBillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/BB." + "/" + maxSbillNo;
                        //    IsGSTcalc = true;
                        //}
                        //else if (objModel.GstType == "N" && !string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                        //{
                        //    tempBillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/BB" + "/" + maxSbillNo;
                        //}
                        //else if (objModel.GstType == "G" && string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                        //{
                        //    tempBillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/BC." + "/" + maxSbillNo;
                        //    IsGSTcalc = true;
                        //}
                        //else if (objModel.GstType == "N" && string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                        //{
                        //    tempBillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/BC" + "/" + maxSbillNo;
                        //}

                        if (objModel != null)
                        {
                            if (objModel.objProduct.PayDetails != null)
                            {
                                if (objModel.objProduct.PayDetails.IsBD)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.BankDeposit;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "V", BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByBD, BankCode = 0, ChqDDDate = null, ChqDDNo = "", CardNo = "", Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.BDBankName, AcNo = objModel.objProduct.PayDetails.AccNo, IFSCode = objModel.objProduct.PayDetails.IFSCCode, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });


                                }
                                if (objModel.objProduct.PayDetails.IsCC)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Card;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "V", BillNo = tempBillNo, PayPrefix = value, AcNo = "", IFSCode = "", BankCode = 0, Narration = "", BankName = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, Amount = objModel.objProduct.PayDetails.AmountByCard, CardNo = objModel.objProduct.PayDetails.CardNo, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsQ)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cheque;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "V", BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByCheque, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.CHBankName, ChqDDNo = objModel.objProduct.PayDetails.ChequeNo, ChqDDDate = objModel.objProduct.PayDetails.ChequeDate, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsD)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.DD;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "V", BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByDD, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.DDBankName, ChqDDNo = objModel.objProduct.PayDetails.DDNo, ChqDDDate = objModel.objProduct.PayDetails.DDDate, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsT)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Credit;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "V", BillNo = tempBillNo, PayPrefix = value, BankName = "", Amount = objModel.objProduct.PayDetails.AmountByCredit, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, DUserId = 0, DRecTimeStamp = null, ChqDDDate = null, ChqDDNo = "", Narration = objModel.objProduct.PayDetails.Narration, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsV)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Voucher;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "V", BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByVoucher, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, DUserId = 0, DRecTimeStamp = null, ChqDDDate = null, ChqDDNo = "", Narration = "", BankName = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });

                                    if (SC1.State == ConnectionState.Closed)
                                        SC1.Open();
                                    objTrans = SC1.BeginTransaction();
                                    decimal cashAmt = objModel.objProduct.TotalNetPayable - objModel.objProduct.PayDetails.AmountByWallet;
                                    //query = "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,AcType,VType,SessID,WSEssID) " +
                                    //"Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.FormNo + "','0','" + objModel.objProduct.PayDetails.AmountByWallet + "','Product purchased Against " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','D','" + SessId + "','" + SessId + "' FROM TrnVoucher";
                                    query = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                                    "Select ISNULL(Max(VoucherNo),0)+1, Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.PartyCode + "','" + objModel.objCustomer.UserDetails.PartyCode + "','" + objModel.objProduct.PayDetails.AmountByVoucher + "','Wallet deducted against bill " + UserBillNo + ".','" + tempBillNo + "','X','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher";
                                    //" UNION ALL Select ISNULL(Max(VoucherNo),0)+2, Cast(Convert(varchar,Getdate(),106) as Datetime),'','" + objModel.objCustomer.PartyCode + "','" + cashAmt + "','Wallet credited against cash in bill " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher " +
                                    //" UNION ALL Select ISNULL(Max(VoucherNo),0)+3, Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.PartyCode + "','','" + cashAmt + "','Wallet debited against bill " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher; ";

                                    cmd = new SqlCommand();
                                    cmd.CommandText = query;
                                    cmd.Connection = SC1;
                                    cmd.Transaction = objTrans;

                                    int i = cmd.ExecuteNonQuery();

                                    objTrans.Commit();
                                    SC1.Close();

                                }
                                if (objModel.objProduct.PayDetails.IsW)
                                {
                                    WalletBalance = 0;
                                    using (InventoryEntities db = new InventoryEntities())
                                    {
                                        var result = (from s in db.V_PartyBalance where s.PartyCode == objModel.objCustomer.PartyCode select s.Balance).FirstOrDefault();
                                        WalletBalance = result ?? 0;
                                    }





                                    if (WalletBalance >= objModel.objProduct.PayDetails.AmountByWallet && objModel.objProduct.PayDetails.AmountByWallet > 0)
                                    {

                                        if (SC1.State == ConnectionState.Closed)
                                            SC1.Open();
                                        objTrans = SC1.BeginTransaction();
                                        decimal cashAmt = objModel.objProduct.TotalNetPayable - objModel.objProduct.PayDetails.AmountByWallet;
                                        //query = "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,AcType,VType,SessID,WSEssID) " +
                                        //"Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.FormNo + "','0','" + objModel.objProduct.PayDetails.AmountByWallet + "','Product purchased Against " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','D','" + SessId + "','" + SessId + "' FROM TrnVoucher";
                                        query = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                                        " Select ISNULL(Max(VoucherNo),0)+1, Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.PartyCode + "','" + objModel.objCustomer.UserDetails.PartyCode + "','" + objModel.objProduct.PayDetails.AmountByWallet + "','Wallet deducted against bill " + UserBillNo + ".','" + tempBillNo + "','R','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher";
                                        //" UNION ALL Select ISNULL(Max(VoucherNo),0)+2, Cast(Convert(varchar,Getdate(),106) as Datetime),'','" + objModel.objCustomer.PartyCode + "','" + cashAmt + "','Wallet credited against cash in bill " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher " +
                                        //" UNION ALL Select ISNULL(Max(VoucherNo),0)+3, Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.PartyCode + "','','" + cashAmt + "','Wallet debited against bill " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher; ";

                                        cmd = new SqlCommand();
                                        cmd.CommandText = query;
                                        cmd.Connection = SC1;
                                        cmd.Transaction = objTrans;

                                        int i = cmd.ExecuteNonQuery();

                                        objTrans.Commit();
                                        SC1.Close();
                                        if (i > 0)
                                        {
                                            EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Wallet;
                                            string value = EnumPayModes.GetEnumDescription(enumVar);
                                            PayPrefix.Add(value);
                                            objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "V", BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByWallet, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, CardNo = objModel.objCustomer.CardNo, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                            ////insert entry into couponsalesdetails for wallet
                                            IsWalletEntry = true;
                                        }
                                        else
                                        {
                                            objResponse.ResponseStatus = "FAILED";
                                            objResponse.ResponseMessage = "Something went wrong";
                                            return objResponse;
                                        }
                                        i = 0;
                                    }
                                    else if (objModel.objProduct.PayDetails.AmountByWallet > 0)
                                    {
                                        objResponse.ResponseStatus = "FAILED";
                                        objResponse.ResponseMessage = "Sorry!Insufficient Wallet Balance.";
                                        return objResponse;
                                    }

                                }

                                if (objModel.objProduct.PayDetails.IsPPW)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.PVPurchaseWallet;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail
                                    {
                                        BillAmt = objModel.objProduct.TotalNetPayable,
                                        SoldBy = objModel.objCustomer.UserDetails.PartyCode,
                                        BillDate = BillDate.Date,
                                        BillType = "V",
                                        BillNo = tempBillNo,
                                        PayPrefix = value,
                                        Amount = objModel.objProduct.PayDetails.AmountByPPW,
                                        CardNo = "",
                                        AcNo = "",
                                        IFSCode = "",
                                        BankCode = 0,
                                        DUserId = 0,
                                        DRecTimeStamp = null,
                                        ChqDDDate = null,
                                        ChqDDNo = "",
                                        Narration = "",
                                        BankName = "",
                                        ActiveStatus = "Y",
                                        RecTimeStamp = DateTime.Now,
                                        UserId = objModel.objCustomer.UserDetails.UserId,
                                        Version = version,
                                        UserName = objModel.objCustomer.UserDetails.UserName,
                                        FSessId = FsessId ?? 0,
                                        SBillNo = maxSbillNo
                                    });

                                    if (SC1.State == ConnectionState.Closed)
                                        SC1.Open();
                                    objTrans = SC1.BeginTransaction();
                                    decimal cashAmt = objModel.objProduct.TotalNetPayable - objModel.objProduct.PayDetails.AmountByWallet;
                                    //query = "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,AcType,VType,SessID,WSEssID) " +
                                    //"Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.FormNo + "','0','" + objModel.objProduct.PayDetails.AmountByWallet + "','Product purchased Against " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','D','" + SessId + "','" + SessId + "' FROM TrnVoucher";
                                    query = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                                    " Select ISNULL(Max(VoucherNo),0)+1, Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.PartyCode + "','" + objModel.objCustomer.UserDetails.PartyCode + "','" + objModel.objProduct.PayDetails.AmountByPPW + "','Wallet deducted against bill " + UserBillNo + ".','" + tempBillNo + "','W','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher";
                                    //" UNION ALL Select ISNULL(Max(VoucherNo),0)+2, Cast(Convert(varchar,Getdate(),106) as Datetime),'','" + objModel.objCustomer.PartyCode + "','" + cashAmt + "','Wallet credited against cash in bill " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher " +
                                    //" UNION ALL Select ISNULL(Max(VoucherNo),0)+3, Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.PartyCode + "','','" + cashAmt + "','Wallet debited against bill " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher; ";

                                    cmd = new SqlCommand();
                                    cmd.CommandText = query;
                                    cmd.Connection = SC1;
                                    cmd.Transaction = objTrans;

                                    int i = cmd.ExecuteNonQuery();

                                    objTrans.Commit();
                                    SC1.Close();

                                }

                                if (objModel.objProduct.PayDetails.IsBPW)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.BVPurchaseWallet;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "V", BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByBPW, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, DUserId = 0, DRecTimeStamp = null, ChqDDDate = null, ChqDDNo = "", Narration = "", BankName = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });

                                    if (SC1.State == ConnectionState.Closed)
                                        SC1.Open();
                                    objTrans = SC1.BeginTransaction();
                                    decimal cashAmt = objModel.objProduct.TotalNetPayable - objModel.objProduct.PayDetails.AmountByWallet;
                                    //query = "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,AcType,VType,SessID,WSEssID) " +
                                    //"Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.FormNo + "','0','" + objModel.objProduct.PayDetails.AmountByWallet + "','Product purchased Against " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','D','" + SessId + "','" + SessId + "' FROM TrnVoucher";
                                    query = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                                    " Select ISNULL(Max(VoucherNo),0)+1, Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.PartyCode + "','" + objModel.objCustomer.UserDetails.PartyCode + "','" + objModel.objProduct.PayDetails.AmountByBPW + "','Wallet deducted against bill " + UserBillNo + ".','" + tempBillNo + "','Z','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher";
                                    //" UNION ALL Select ISNULL(Max(VoucherNo),0)+2, Cast(Convert(varchar,Getdate(),106) as Datetime),'','" + objModel.objCustomer.PartyCode + "','" + cashAmt + "','Wallet credited against cash in bill " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher " +
                                    //" UNION ALL Select ISNULL(Max(VoucherNo),0)+3, Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.PartyCode + "','','" + cashAmt + "','Wallet debited against bill " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher; ";

                                    cmd = new SqlCommand();
                                    cmd.CommandText = query;
                                    cmd.Connection = SC1;
                                    cmd.Transaction = objTrans;

                                    int i = cmd.ExecuteNonQuery();

                                    objTrans.Commit();
                                    SC1.Close();

                                }

                                if (objModel.objProduct.PayDetails.IsP)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Paytm;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "V", BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByPaytm, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = objModel.objProduct.PayDetails.PaytmTransactionId, ChqDDDate = DateTime.Now, CardNo = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.CashAmount > 0)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cash;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail
                                    {
                                        BillAmt = objModel.objProduct.TotalNetPayable,
                                        SoldBy = objModel.objCustomer.UserDetails.PartyCode,
                                        BillDate = BillDate.Date,
                                        BillType = "V",
                                        BillNo = tempBillNo,
                                        PayPrefix = value,
                                        Amount = objModel.objProduct.CashAmount,
                                        BankCode = 0,
                                        BankName = "",
                                        AcNo = "",
                                        IFSCode = "",
                                        Narration = "",
                                        DUserId = 0,
                                        DRecTimeStamp = null,
                                        ChqDDNo = "",
                                        ChqDDDate = null,
                                        CardNo = "",
                                        ActiveStatus = "Y",
                                        RecTimeStamp = DateTime.Now,
                                        UserId = objModel.objCustomer.UserDetails.UserId,
                                        Version = version,
                                        UserName = objModel.objCustomer.UserDetails.UserName,
                                        FSessId = FsessId ?? 0,
                                        SBillNo = maxSbillNo
                                    });
                                }
                                if (PayPrefix.Count > 0)
                                {

                                    Paymode = (from r in entity.M_PayModeMaster where PayPrefix.Contains(r.Prefix) select r.PayMode).ToList();
                                }

                            }
                        }
                        if (!string.IsNullOrEmpty(objModel.TaxORStock) && objModel.TaxORStock.ToLower() == "stock")
                        {
                            maxUserSBillNo = (from result in entity.TrnBillMains where result.FSessId == FsessId && result.SoldBy == objModel.objCustomer.UserDetails.PartyCode && result.BillType == "S" select result.UserSBillNo).DefaultIfEmpty(0).Max();
                            maxUserSBillNo = maxUserSBillNo + 1;
                            strMaxUserSBillNo = maxUserSBillNo.ToString();
                            if (strMaxUserSBillNo.Count() < 3)
                            {
                                var countNum = strMaxUserSBillNo.Count();
                                var ToBeAddedDigits = 3 - countNum;
                                for (var j = 0; j < ToBeAddedDigits; j++)
                                {
                                    strMaxUserSBillNo = "0" + strMaxUserSBillNo;
                                }
                            }
                            UserBillNo = billPrefix + "/ST/" + strMaxUserSBillNo;
                        }



                        string SoldByCode = "";
                        List<TrnBillData> tempTableList = new List<TrnBillData>();
                        string GroupPrefix = "";
                        string BillingPartyCode = objModel.objCustomer.PartyCode;
                        GroupPrefix = (from p in entity.M_GroupMaster
                                       where p.GroupId == (from r in entity.M_LedgerMaster where r.PartyCode == BillingPartyCode select r.GroupId).FirstOrDefault()
                                       select p.Prefix
                                       ).FirstOrDefault();

                        try
                        {
                            List<ProductModel> objListProductModel = new List<ProductModel>();
                            //TempDistributor.objListProduct.AddRange(objModel.objListProduct);
                            foreach (var obj in objModel.objListProduct)
                            {
                                objListProductModel.Add(obj);
                                TrnBillData objDTBillData = new TrnBillData();
                                objDTBillData.SBillNo = maxSbillNo;
                                objDTBillData.FSessId = FsessId ?? 0;
                                objDTBillData.SessId = SessId ?? 0;
                                objDTBillData.ActiveStatus = "Y";
                                objDTBillData.BillDate = BillDate.Date;

                                objDTBillData.RefNo = string.IsNullOrEmpty(objModel.objCustomer.ReferenceIdNo) ? "" : objModel.objCustomer.ReferenceIdNo;
                                objDTBillData.RefId = 0;
                                objDTBillData.RefName = "";
                                objDTBillData.Remarks = string.IsNullOrEmpty(objModel.objCustomer.Remarks) ? "" : objModel.objCustomer.Remarks;
                                objDTBillData.CType = GroupPrefix;
                                objDTBillData.SoldBy = objModel.objCustomer.UserDetails.PartyCode;
                                SoldByCode = objDTBillData.SoldBy;
                                objDTBillData.BillBy = objDTBillData.SoldBy;
                                objDTBillData.BillNo = tempBillNo;
                                objDTBillData.FType = GroupPrefix;
                                objDTBillData.FCode = objModel.objCustomer.PartyCode;
                                objDTBillData.PartyName = objModel.objCustomer.PartyName;
                                objDTBillData.SupplierId = 0;
                                objDTBillData.ChDDNo = 0;
                                objDTBillData.ChDate = DateTime.Now;
                                objDTBillData.ChAmt = 0;
                                objDTBillData.BankCode = 0;
                                objDTBillData.BankName = "";
                                objDTBillData.FormNo = 0;
                                objDTBillData.TotalTaxAmount = objModel.objProduct.TotalTaxAmount;
                                objDTBillData.TotalSTaxAmount = 0;
                                objDTBillData.TotalDiscount = objModel.objProduct.TotalDiscount;
                                objDTBillData.TotalKitBvValue = 0;
                                objDTBillData.TotalBvValue = objModel.objProduct.TotalBV;
                                objDTBillData.TotalCVValue = objModel.objProduct.TotalCV;
                                objDTBillData.TotalPVValue = objModel.objProduct.TotalPV;
                                objDTBillData.TotalRPValue = objModel.objProduct.TotalRP;

                                objDTBillData.DP = obj.DP ?? 0;
                                objDTBillData.RP = obj.RP ?? 0;
                                objDTBillData.MRP = obj.MRP ?? 0;
                                objDTBillData.CVValue = obj.CVValue ?? 0;
                                objDTBillData.CV = obj.CV ?? 0;
                                objDTBillData.PV = obj.PV ?? 0;
                                objDTBillData.BV = obj.BV ?? 0;
                                objDTBillData.BVValue = obj.BVValue ?? 0;
                                objDTBillData.PVValue = obj.PVValue ?? 0;
                                objDTBillData.RPValue = obj.RPValue ?? 0;
                                objDTBillData.Barcode = obj.Barcode.ToString();
                                objDTBillData.BatchNo = obj.BatchNo.ToString();

                                objDTBillData.DiscountPer = obj.DiscPer ?? 0;
                                objDTBillData.Discount = obj.DiscAmt ?? 0;
                                objDTBillData.ProdCommssn = obj.CommissionPer ?? 0;
                                objDTBillData.ProdCommssnAmt = obj.CommissionAmt ?? 0;
                                objDTBillData.ProductId = obj.ProdCode.ToString();
                                objDTBillData.ProductName = obj.ProductName;
                                objDTBillData.Qty = obj.Quantity;
                                objDTBillData.Rate = obj.Rate ?? 0;
                                objDTBillData.IsKitBV = "N";
                                objDTBillData.DSeries = "";
                                objDTBillData.DImported = "N";
                                objDTBillData.IMEINo = "D";
                                objDTBillData.BNo = "";
                                objDTBillData.ItemType = "N";



                                objDTBillData.JType = "Cash:" + objModel.objProduct.TotalNetPayable;
                                objDTBillData.BillTo = objModel.objCustomer.PartyCode;
                                objDTBillData.BillFor = objModel.objCustomer.PartyCode;
                                objDTBillData.IsReceive = "N";
                                objDTBillData.IsCredit = "F";
                                //objDTBillData.BillType = "R";
                                if (objModel.TaxORStock.ToLower() == "tax")
                                {
                                    objDTBillData.BillType = "V";
                                }
                                else
                                {
                                    objDTBillData.BillType = "S";
                                }

                                objDTBillData.ProdType = "P";
                                objDTBillData.PaymentDtl = "Cash:" + objModel.objProduct.TotalNetPayable;

                                objDTBillData.TotalAmount = objModel.objProduct.TotalTotalAmount;
                                //tax excluding
                                objDTBillData.NetAmount = obj.Amount;
                                if (IsGSTcalc == true)
                                {
                                    if (objModel.objCustomer.StateCode == objModel.objCustomer.UserDetails.StateCode)
                                    {
                                        objDTBillData.TaxAmount = 0;
                                        objDTBillData.Tax = 0;
                                        objDTBillData.CGST = obj.TaxPer / 2 ?? 0;
                                        objDTBillData.CGSTAmt = obj.TaxAmt / 2 ?? 0;
                                        objDTBillData.SGST = obj.TaxPer / 2 ?? 0;
                                        objDTBillData.SGSTAmt = obj.TaxAmt / 2 ?? 0;
                                        objDTBillData.TaxType = "S";
                                    }
                                    else
                                    {

                                        objDTBillData.TaxAmount = obj.TaxAmt ?? 0;
                                        if (obj.OldTaxAmount != 0 && obj.OldTaxAmount != obj.TaxAmt)
                                        {
                                            objDTBillData.TaxAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.TaxAmount) + 0.01).ToString());
                                            objDTBillData.NetAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.NetAmount) - 0.01).ToString());
                                        }
                                        objDTBillData.Tax = obj.TaxPer ?? 0;
                                        objDTBillData.CGST = 0;
                                        objDTBillData.CGSTAmt = 0;
                                        objDTBillData.SGST = 0;
                                        objDTBillData.SGSTAmt = 0;
                                        objDTBillData.TaxType = "I";
                                    }
                                }
                                else
                                {
                                    objDTBillData.TaxAmount = 0;
                                    objDTBillData.Tax = 0;
                                    objDTBillData.CGST = 0;
                                    objDTBillData.CGSTAmt = 0;
                                    objDTBillData.SGST = 0;
                                    objDTBillData.SGSTAmt = 0;
                                    objDTBillData.TaxType = "N";
                                }

                                objDTBillData.CashDiscPer = obj.CashDiscPer;
                                objDTBillData.CashDiscAmount = obj.CashDiscAmount;

                                objDTBillData.NetPayable = Math.Round(objModel.objProduct.TotalNetPayable);
                                if (objModel.objProduct.Roundoff == 0)
                                {
                                    objDTBillData.RndOff = objDTBillData.NetPayable - objModel.objProduct.TotalNetPayable;
                                }
                                else
                                {
                                    objDTBillData.RndOff = objModel.objProduct.Roundoff;
                                }
                                objDTBillData.CardAmount = 0;
                                objDTBillData.PayMode = Paymode.Count > 1 ? string.Join(",", Paymode) : Paymode[0];
                                objDTBillData.PayPrefix = PayPrefix.Count > 1 ? string.Join(",", PayPrefix) : PayPrefix[0];
                                objDTBillData.BvTransfer = "N";

                                //objDTBillData.UserSBillNo = maxSbillNo;
                                //objDTBillData.UserBillNo = billPrefix + "/" + objDTBillData.BillBy + "/" + maxSbillNo;
                                objDTBillData.UserSBillNo = maxUserSBillNo;
                                objDTBillData.UserBillNo = UserBillNo;
                                objDTBillData.DispatchStatus = "N";
                                objDTBillData.LR = "0";
                                objDTBillData.LRDate = DateTime.Now;
                                objDTBillData.TransporterName = "";
                                objDTBillData.DispatchTo = "";
                                objDTBillData.FreightType = "";
                                objDTBillData.Series = "";
                                objDTBillData.Scratch = "";

                                objDTBillData.Unit = 0;

                                objDTBillData.PSessId = 0;
                                objDTBillData.DcNo = "";
                                objDTBillData.Imported = "N";
                                objDTBillData.FPoint = 0;
                                objDTBillData.FPointValue = 0;
                                objDTBillData.OrdStatus = "";
                                objDTBillData.OrdQty = 0;
                                // objDTBillData.OrderType = "";
                                objDTBillData.OrderDate = DateTime.Now;
                                objDTBillData.OrderNo = "";
                                objDTBillData.RemQty = 0;
                                objDTBillData.DP1 = 0;
                                objDTBillData.DReason = "";
                                objDTBillData.DUserId = 0;
                                objDTBillData.DRecTimeStamp = DateTime.Now;
                                objDTBillData.DocWeight = objModel.objProduct.TotalWeight;
                                objDTBillData.DocketNo = "";
                                objDTBillData.DocketDate = DateTime.Now;
                                //objDTBillData.UserBillNo = "";
                                //objDTBillData.UserSBillNo = 0;
                                objDTBillData.STNFormNo = "";
                                objDTBillData.StkRecv = "N";
                                objDTBillData.StkRecvDate = DateTime.Now;
                                objDTBillData.StkRecvUserId = 0;
                                objDTBillData.InTransit = "N";
                                objDTBillData.UID = string.IsNullOrEmpty(objModel.objProduct.UID) ? "" : objModel.objProduct.UID;
                                objDTBillData.OfferUID = 0;
                                objDTBillData.IsKit = "N";
                                objDTBillData.TotalCorton = "";
                                objDTBillData.TotalMonoCorton = "";
                                objDTBillData.SpclOfferId = 0;
                                objDTBillData.VAT = 0;
                                objDTBillData.BuyerAddress = "";
                                objDTBillData.BuyerTIN = "";

                                objDTBillData.TotalDiscount = objModel.objProduct.TotalDiscPer;
                                objDTBillData.TotalDiscountAmt = objModel.objProduct.TotalDiscount;
                                objDTBillData.VDiscountAmt = 0;
                                objDTBillData.VDiscount = 0;
                                objDTBillData.ReceiverID = "";
                                objDTBillData.ReceiverName = "";
                                objDTBillData.ReceiverMNo = "";
                                objDTBillData.ReceiverIDProof = "";
                                objDTBillData.TotalFPoint = 0;
                                objDTBillData.TotalQty = objModel.objProduct.TotalQty;
                                objDTBillData.CashReward = 0;
                                objDTBillData.CommssnAmt = objModel.objProduct.TotalCommsonAmt;
                                objDTBillData.RecvAmount = 0;
                                objDTBillData.ReturnToCustAmt = 0;
                                objDTBillData.ActiveStatus = "Y";
                                objDTBillData.RecTimeStamp = DateTime.Now;
                                objDTBillData.UserId = objModel.objCustomer.UserDetails.UserId;
                                objDTBillData.UserName = objModel.objCustomer.UserDetails.UserName;
                                objDTBillData.DelvPlace = string.IsNullOrEmpty(objModel.objProduct.DeliveryPlace) ? "" : objModel.objProduct.DeliveryPlace;
                                objDTBillData.DelvStatus = "";
                                objDTBillData.DelvUserId = 0;
                                objDTBillData.DelvRecTimeStamp = DateTime.Now;
                                objDTBillData.Version = version;
                                objDTBillData.IDType = "";
                                objDTBillData.BranchName = "";
                                objDTBillData.CourierId = 0;
                                objDTBillData.CourierName = "";
                                objDTBillData.LocId = 0;
                                objDTBillData.LocName = "";
                                objDTBillData.DelvAddress = "";
                                objDTBillData.Pincode = "";
                                objDTBillData.OrderType = objModel.PartyInvoice;

                                objDTBillData.Coupon = "";
                                objDTBillData.CouponAmount = 0;
                                objDTBillData.PaidBV = 0;
                                objDTBillData.IRNNo = "";
                                objDTBillData.AckNo = "";
                                objDTBillData.AckDate = DateTime.Now;
                                objDTBillData.QrCodeimage = "";
                                objDTBillData.QrCode = "";
                                objDTBillData.SignedInvoice = "";
                                objDTBillData.BillGSTType = BillGSTType;
                                objDTBillData.InvoiceType = "GST";
                                billno_ = objDTBillData.BillNo;
                                soldby_ = objDTBillData.SoldBy;
                                fcode_ = objDTBillData.FCode;
                                netpayable_ = objDTBillData.NetPayable;
                                narration_ = "Wallet deducted against " + objDTBillData.UserBillNo + ".";
                                entity.TrnBillDatas.Add(objDTBillData);
                                // entity.TrnBillDatas.Add(objDTBillData);
                            }
                            int i = 0;

                            using (var objDTTrans = entity.Database.BeginTransaction())
                            {

                                try
                                {
                                    i = entity.SaveChanges();
                                    objDTTrans.Commit();
                                }
                                catch (DbEntityValidationException e)
                                {
                                    foreach (var eve in e.EntityValidationErrors)
                                    {
                                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                        foreach (var ve in eve.ValidationErrors)
                                        {
                                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                                ve.PropertyName, ve.ErrorMessage);
                                        }
                                    }
                                    throw;
                                }
                                catch (Exception ex)
                                {

                                    objDTTrans.Rollback();
                                }
                            }
                            if (i == objModel.objListProduct.Count)
                            {
                                if (objModel.PartyInvoice == "B" && objModel.objProduct.PayDetails.AmountByVoucher > 0)
                                {
                                    //Jab bhi franchise / party bill banta he to usme Promo/Voucher use hota he to uska BV sale limit nahi badhana he
                                    netpayable_ = netpayable_ - objModel.objProduct.PayDetails.AmountByVoucher;
                                }
                                CreditPartyWallet(billno_, "Wallet Credited against " + UserBillNo + ".", SoldByCode, fcode_, netpayable_, objModel.PartyInvoice);
                                //DeductPartyWallet(billno_, narration_, soldby_, fcode_,netpayable_);
                                var resultPayMode = (from r in entity.M_PayModeMaster select r).ToList();
                                foreach (var obj in objDTListPayMode)
                                {
                                    TrnPayModeDetail objTemp = new TrnPayModeDetail();
                                    objTemp = obj;
                                    objTemp.BillType = "V";
                                    objTemp.PayMode = (from r in resultPayMode where r.Prefix.Trim() == obj.PayPrefix.Trim() select r.PayMode).FirstOrDefault();
                                    if (string.IsNullOrEmpty(objTemp.CardNo))
                                    {
                                        objTemp.CardNo = "";
                                    }
                                    entity.TrnPayModeDetails.Add(objTemp);
                                }
                                i = 0;
                                i = entity.SaveChanges();
                                if (i == objDTListPayMode.Count)
                                {
                                    if (objModel.EInvoice == "Y")
                                    {
                                        Make_Inv_json(UserBillNo);

                                    }
                                    objResponse.ResponseMessage = "Saved Successfully!";
                                    objResponse.ResponseStatus = "OK";
                                    objResponse.ResponseDetailsToPrint = new DistributorBillModel();
                                    objResponse.ResponseDetailsToPrint.BillNo = UserBillNo;
                                    objResponse.ResponseDetailsToPrint.SoldBy = SoldByCode;


                                }
                            }
                        }
                        catch (DbEntityValidationException e)
                        {
                            objResponse.ResponseMessage = "Something went wrong!";
                            objResponse.ResponseStatus = "FAILED";
                        }

                    }
                    else
                    {
                        // saving process of customer bill
                        //customer bill
                        //customer bill
                        //customer bill

                        DateTime BillDate = DateTime.Now.Date;




                        //saving data in table
                        // decimal? SessId=(from result in entity)
                        bool IsWalletEntry = false;
                        string tempBillNo = "";
                        tempBillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo;
                        //if (objModel.GstType == "G" && !string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                        //{
                        //    tempBillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/BB." + "/" + maxSbillNo;
                        //    IsGSTcalc = true;
                        //}
                        //else if (objModel.GstType == "N" && !string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                        //{
                        //    tempBillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/BB" + "/" + maxSbillNo;
                        //}
                        //else if (objModel.GstType == "G" && string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                        //{
                        //    tempBillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/BC." + "/" + maxSbillNo;
                        //    IsGSTcalc = true;
                        //}
                        //else if (objModel.GstType == "N" && string.IsNullOrEmpty(objModel.objCustomer.GSTNo))
                        //{
                        //    tempBillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/BC" + "/" + maxSbillNo;
                        //}
                        if (objModel != null)
                        {

                            if (objModel.objProduct.PayDetails != null)
                            {


                                if (objModel.objProduct.PayDetails.IsBD)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.BankDeposit;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "G", BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByBD, BankCode = objModel.objProduct.PayDetails.BankCode, ChqDDDate = null, ChqDDNo = "", CardNo = "", Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.BDBankName, AcNo = "", IFSCode = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });


                                }
                                if (objModel.objProduct.PayDetails.IsCC)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Card;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "G", BillNo = tempBillNo, PayPrefix = value, AcNo = "", IFSCode = "", BankCode = 0, Narration = "", BankName = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, Amount = objModel.objProduct.PayDetails.AmountByCard, CardNo = objModel.objProduct.PayDetails.CardNo, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsQ)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cheque;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "G", BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByCheque, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.CHBankName, ChqDDNo = objModel.objProduct.PayDetails.ChequeNo, ChqDDDate = objModel.objProduct.PayDetails.ChequeDate, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsD)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.DD;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "G", BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByDD, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.DDBankName, ChqDDNo = objModel.objProduct.PayDetails.DDNo, ChqDDDate = objModel.objProduct.PayDetails.DDDate, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsT)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Credit;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "G", BillNo = tempBillNo, PayPrefix = value, BankName = "", Amount = objModel.objProduct.PayDetails.AmountByCredit, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, DUserId = 0, DRecTimeStamp = null, ChqDDDate = null, ChqDDNo = "", Narration = objModel.objProduct.PayDetails.Narration, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsV)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Voucher;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "G", BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByVoucher, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, DUserId = 0, DRecTimeStamp = null, ChqDDDate = null, ChqDDNo = "", Narration = "", BankName = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsW)
                                {


                                    if (WalletBalance >= objModel.objProduct.PayDetails.AmountByWallet)
                                    {

                                        SC.Close();
                                        SC.Open();
                                        objTrans = SC.BeginTransaction();
                                        query = "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,AcType,VType,SessID,WSEssID) " +
                                                       "Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.FormNo + "','0','" + objModel.objProduct.PayDetails.AmountByWallet + "','Product purchased Against " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','D','" + SessId + "','" + SessId + "' FROM TrnVoucher";
                                        cmd = new SqlCommand();
                                        cmd.CommandText = query;
                                        cmd.Connection = SC;
                                        cmd.Transaction = objTrans;



                                        int i = cmd.ExecuteNonQuery();

                                        objTrans.Commit();
                                        SC.Close();
                                        if (i > 0)
                                        {
                                            EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Wallet;
                                            string value = EnumPayModes.GetEnumDescription(enumVar);
                                            PayPrefix.Add(value);
                                            objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "G", BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByWallet, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, CardNo = objModel.objCustomer.CardNo, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                            ////insert entry into couponsalesdetails for wallet
                                            IsWalletEntry = true;
                                        }
                                        else
                                        {
                                            objResponse.ResponseStatus = "FAILED";
                                            objResponse.ResponseMessage = "Something went wrong";
                                            return objResponse;
                                        }
                                        i = 0;
                                    }
                                    else
                                    {
                                        objResponse.ResponseStatus = "FAILED";
                                        objResponse.ResponseMessage = "Sorry!Insufficient Wallet Balance.";
                                        return objResponse;
                                    }

                                }
                                if (objModel.objProduct.PayDetails.IsP)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Paytm;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "G", BillNo = tempBillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByPaytm, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = objModel.objProduct.PayDetails.PaytmTransactionId, ChqDDDate = DateTime.Now, CardNo = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.CashAmount > 0)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cash;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail
                                    {
                                        BillAmt = objModel.objProduct.TotalNetPayable,
                                        SoldBy = objModel.objCustomer.UserDetails.PartyCode,
                                        BillDate = BillDate.Date,
                                        BillType = "G",
                                        BillNo = tempBillNo,
                                        PayPrefix = value,
                                        Amount = objModel.objProduct.CashAmount,
                                        BankCode = 0,
                                        BankName = "",
                                        AcNo = "",
                                        IFSCode = "",
                                        Narration = "",
                                        DUserId = 0,
                                        DRecTimeStamp = null,
                                        ChqDDNo = "",
                                        ChqDDDate = null,
                                        CardNo = "",
                                        ActiveStatus = "Y",
                                        RecTimeStamp = DateTime.Now,
                                        UserId = objModel.objCustomer.UserDetails.UserId,
                                        Version = version,
                                        UserName = objModel.objCustomer.UserDetails.UserName,
                                        FSessId = FsessId ?? 0,
                                        SBillNo = maxSbillNo
                                    });
                                }
                                if (PayPrefix.Count > 0)
                                {

                                    Paymode = (from r in entity.M_PayModeMaster where PayPrefix.Contains(r.Prefix) select r.PayMode).ToList();
                                }

                            }
                        }


                        string SoldByCode = "";
                        List<TrnBillData> tempTableList = new List<TrnBillData>();
                        string GroupPrefix = "";
                        string BillingPartyCode = objModel.objCustomer.PartyCode;
                        GroupPrefix = (from p in entity.M_GroupMaster
                                       where p.GroupId == (from r in entity.M_LedgerMaster where r.PartyCode == BillingPartyCode select r.GroupId).FirstOrDefault()
                                       select p.Prefix
                                       ).FirstOrDefault();

                        try
                        {
                            List<ProductModel> objListProductModel = new List<ProductModel>();
                            //TempDistributor.objListProduct.AddRange(objModel.objListProduct);
                            foreach (var obj in objModel.objListProduct)
                            {
                                objListProductModel.Add(obj);
                                TrnBillData objDTBillData = new TrnBillData();
                                objDTBillData.SBillNo = maxSbillNo;
                                objDTBillData.FSessId = FsessId ?? 0;
                                objDTBillData.SessId = SessId ?? 0;
                                objDTBillData.ActiveStatus = "Y";
                                objDTBillData.BillDate = BillDate.Date;

                                objDTBillData.RefNo = string.IsNullOrEmpty(objModel.objCustomer.ReferenceIdNo) ? "" : objModel.objCustomer.ReferenceIdNo;
                                objDTBillData.RefId = 0;
                                objDTBillData.RefName = "";
                                objDTBillData.Remarks = string.IsNullOrEmpty(objModel.objCustomer.Remarks) ? "" : objModel.objCustomer.Remarks;
                                objDTBillData.CType = GroupPrefix;
                                objDTBillData.SoldBy = objModel.objCustomer.UserDetails.PartyCode;
                                SoldByCode = objDTBillData.SoldBy;
                                objDTBillData.BillBy = objDTBillData.SoldBy;
                                objDTBillData.BillNo = tempBillNo;

                                objDTBillData.FType = GroupPrefix;
                                objDTBillData.FCode = string.IsNullOrEmpty(objModel.objCustomer.Name) ? "" : objModel.objCustomer.Name;
                                objDTBillData.PartyName = string.IsNullOrEmpty(objModel.objCustomer.Name) ? "" : objModel.objCustomer.Name;
                                objDTBillData.SupplierId = 0;
                                objDTBillData.ChDDNo = 0;
                                objDTBillData.ChDate = DateTime.Now;
                                objDTBillData.ChAmt = 0;
                                objDTBillData.BankCode = objModel.objProduct.PayDetails.BankCode == null ? 0 : objModel.objProduct.PayDetails.BankCode;
                                objDTBillData.BankName = string.IsNullOrEmpty(objModel.objProduct.PayDetails.BDBankName) ? "" : objModel.objProduct.PayDetails.BDBankName;
                                objDTBillData.FormNo = 0;
                                objDTBillData.TotalTaxAmount = objModel.objProduct.TotalTaxAmount;
                                objDTBillData.TotalSTaxAmount = 0;
                                objDTBillData.TotalDiscount = objModel.objProduct.TotalDiscount;
                                objDTBillData.TotalKitBvValue = 0;
                                objDTBillData.TotalBvValue = objModel.objProduct.TotalBV;
                                objDTBillData.TotalCVValue = objModel.objProduct.TotalCV;
                                objDTBillData.TotalPVValue = objModel.objProduct.TotalPV;
                                objDTBillData.TotalRPValue = objModel.objProduct.TotalRP;

                                objDTBillData.DP = obj.DP ?? 0;
                                objDTBillData.RP = obj.RP ?? 0;
                                objDTBillData.MRP = obj.MRP ?? 0;
                                objDTBillData.CVValue = obj.CVValue ?? 0;
                                objDTBillData.CV = obj.CV ?? 0;
                                objDTBillData.PV = obj.PV ?? 0;
                                objDTBillData.BV = obj.BV ?? 0;
                                objDTBillData.BVValue = obj.BVValue ?? 0;
                                objDTBillData.PVValue = obj.PVValue ?? 0;
                                objDTBillData.RPValue = obj.RPValue ?? 0;
                                objDTBillData.Barcode = obj.Barcode.ToString();
                                objDTBillData.BatchNo = obj.BatchNo.ToString();

                                objDTBillData.DiscountPer = obj.DiscPer ?? 0;
                                objDTBillData.Discount = obj.DiscAmt ?? 0;
                                objDTBillData.ProdCommssn = obj.CommissionPer ?? 0;
                                objDTBillData.ProdCommssnAmt = obj.CommissionAmt ?? 0;
                                objDTBillData.ProductId = obj.ProdCode.ToString();
                                objDTBillData.ProductName = obj.ProductName;
                                objDTBillData.Qty = obj.Quantity;
                                objDTBillData.Rate = obj.Rate ?? 0;
                                objDTBillData.IsKitBV = "N";
                                objDTBillData.DSeries = "";
                                objDTBillData.DImported = "N";
                                objDTBillData.IMEINo = "D";
                                objDTBillData.BNo = "";
                                objDTBillData.ItemType = "N";



                                objDTBillData.JType = "Cash:" + objModel.objProduct.TotalNetPayable;
                                objDTBillData.BillTo = string.IsNullOrEmpty(objModel.objCustomer.Name) ? "" : objModel.objCustomer.Name;
                                objDTBillData.BillFor = string.IsNullOrEmpty(objModel.objCustomer.Name) ? "" : objModel.objCustomer.Name;
                                objDTBillData.IsReceive = "G";
                                objDTBillData.IsCredit = "F";
                                //objDTBillData.BillType = "R";
                                objDTBillData.BillType = "GC";
                                objDTBillData.ProdType = "P";
                                objDTBillData.PaymentDtl = "Cash:" + objModel.objProduct.TotalNetPayable;

                                objDTBillData.TotalAmount = objModel.objProduct.TotalTotalAmount;
                                //customer bill tax calaulating
                                objDTBillData.NetAmount = obj.Amount;
                                if (objModel.InvoiceType == "GST")
                                {
                                    if (IsGSTcalc)
                                    {
                                        if (objModel.CustTaxType == "I")
                                        {
                                            objDTBillData.TaxAmount = obj.TaxAmt ?? 0;
                                            if (obj.OldTaxAmount != 0 && obj.OldTaxAmount != obj.TaxAmt)
                                            {
                                                objDTBillData.TaxAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.TaxAmount) + 0.01).ToString());
                                                objDTBillData.NetAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.NetAmount) - 0.01).ToString());
                                            }
                                            objDTBillData.Tax = obj.TaxPer ?? 0;
                                            objDTBillData.CGST = 0;
                                            objDTBillData.CGSTAmt = 0;
                                            objDTBillData.SGST = 0;
                                            objDTBillData.SGSTAmt = 0;
                                            objDTBillData.TaxType = "I";
                                        }
                                        else
                                        {
                                            objDTBillData.TaxAmount = 0;
                                            objDTBillData.Tax = 0;
                                            objDTBillData.CGST = obj.TaxPer / 2 ?? 0;
                                            objDTBillData.CGSTAmt = obj.TaxAmt / 2 ?? 0;
                                            objDTBillData.SGST = obj.TaxPer / 2 ?? 0;
                                            objDTBillData.SGSTAmt = obj.TaxAmt / 2 ?? 0;
                                            objDTBillData.TaxType = "S";
                                        }

                                    }
                                    else
                                    {
                                        objDTBillData.TaxAmount = 0;
                                        objDTBillData.Tax = 0;
                                        objDTBillData.CGST = 0;
                                        objDTBillData.CGSTAmt = 0;
                                        objDTBillData.SGST = 0;
                                        objDTBillData.SGSTAmt = 0;
                                        objDTBillData.TaxType = "N";
                                    }
                                }
                                else
                                {
                                    objDTBillData.TaxAmount = 0;
                                    objDTBillData.Tax = 0;
                                    objDTBillData.CGST = 0;
                                    objDTBillData.CGSTAmt = 0;
                                    objDTBillData.SGST = 0;
                                    objDTBillData.SGSTAmt = 0;
                                    objDTBillData.TaxType = "N";
                                }

                                objDTBillData.CashDiscPer = obj.CashDiscPer;
                                objDTBillData.CashDiscAmount = obj.CashDiscAmount;

                                objDTBillData.NetPayable = Math.Round(objModel.objProduct.TotalNetPayable);
                                if (objModel.objProduct.Roundoff == 0)
                                {
                                    objDTBillData.RndOff = objDTBillData.NetPayable - objModel.objProduct.TotalNetPayable;
                                }
                                else
                                {
                                    objDTBillData.RndOff = objModel.objProduct.Roundoff;
                                }
                                objDTBillData.CardAmount = 0;
                                objDTBillData.PayMode = Paymode.Count > 1 ? string.Join(",", Paymode) : Paymode[0];
                                objDTBillData.PayPrefix = PayPrefix.Count > 1 ? string.Join(",", PayPrefix) : PayPrefix[0];
                                objDTBillData.BvTransfer = "N";

                                //objDTBillData.UserSBillNo = maxSbillNo;
                                //objDTBillData.UserBillNo = billPrefix + "/" + objDTBillData.BillBy + "/" + maxSbillNo;
                                objDTBillData.UserSBillNo = maxUserSBillNo;
                                objDTBillData.UserBillNo = UserBillNo;
                                objDTBillData.DispatchStatus = "N";
                                objDTBillData.LR = "0";
                                objDTBillData.LRDate = DateTime.Now;
                                objDTBillData.TransporterName = "";
                                objDTBillData.DispatchTo = "";
                                objDTBillData.FreightType = "";
                                objDTBillData.Series = "";
                                objDTBillData.Scratch = "";

                                objDTBillData.Unit = 0;

                                objDTBillData.PSessId = 0;
                                objDTBillData.DcNo = "";
                                objDTBillData.Imported = "N";
                                objDTBillData.FPoint = 0;
                                objDTBillData.FPointValue = 0;
                                objDTBillData.OrdStatus = "";
                                objDTBillData.OrdQty = 0;
                                // objDTBillData.OrderType = "";
                                objDTBillData.OrderDate = DateTime.Now;
                                objDTBillData.OrderNo = "";
                                objDTBillData.RemQty = 0;
                                objDTBillData.DP1 = 0;
                                objDTBillData.DReason = "";
                                objDTBillData.DUserId = 0;
                                objDTBillData.DRecTimeStamp = DateTime.Now;
                                objDTBillData.DocWeight = objModel.objProduct.TotalWeight;
                                objDTBillData.DocketNo = "";
                                objDTBillData.DocketDate = DateTime.Now;
                                //objDTBillData.UserBillNo = "";
                                //objDTBillData.UserSBillNo = 0;
                                objDTBillData.STNFormNo = "";
                                objDTBillData.StkRecv = "N";
                                objDTBillData.StkRecvDate = DateTime.Now;
                                objDTBillData.StkRecvUserId = 0;
                                objDTBillData.InTransit = "N";
                                objDTBillData.UID = string.IsNullOrEmpty(objModel.objProduct.UID) ? "" : objModel.objProduct.UID;
                                objDTBillData.OfferUID = 0;
                                objDTBillData.IsKit = "N";
                                objDTBillData.TotalCorton = "";
                                objDTBillData.TotalMonoCorton = "";
                                objDTBillData.SpclOfferId = 0;
                                objDTBillData.VAT = 0;
                                objDTBillData.BuyerAddress = "";
                                objDTBillData.BuyerTIN = "";

                                objDTBillData.TotalDiscount = objModel.objProduct.TotalDiscPer;
                                objDTBillData.TotalDiscountAmt = objModel.objProduct.TotalDiscount;
                                objDTBillData.VDiscountAmt = 0;
                                objDTBillData.VDiscount = 0;
                                objDTBillData.ReceiverID = "";
                                objDTBillData.ReceiverName = "";
                                objDTBillData.ReceiverMNo = string.IsNullOrEmpty(objModel.objCustomer.MobileNo) ? "" : objModel.objCustomer.MobileNo;
                                objDTBillData.ReceiverIDProof = "";
                                objDTBillData.TotalFPoint = 0;
                                objDTBillData.TotalQty = objModel.objProduct.TotalQty;
                                objDTBillData.CashReward = 0;
                                objDTBillData.CommssnAmt = objModel.objProduct.TotalCommsonAmt;
                                objDTBillData.RecvAmount = 0;
                                objDTBillData.ReturnToCustAmt = 0;
                                objDTBillData.ActiveStatus = "Y";
                                objDTBillData.RecTimeStamp = DateTime.Now;
                                objDTBillData.UserId = objModel.objCustomer.UserDetails.UserId;
                                objDTBillData.UserName = objModel.objCustomer.UserDetails.UserName;
                                objDTBillData.DelvPlace = string.IsNullOrEmpty(objModel.objProduct.DeliveryPlace) ? "" : objModel.objProduct.DeliveryPlace;
                                objDTBillData.DelvStatus = "";
                                objDTBillData.DelvUserId = 0;
                                objDTBillData.DelvRecTimeStamp = DateTime.Now;
                                objDTBillData.Version = version;
                                objDTBillData.IDType = "";
                                objDTBillData.BranchName = "";
                                objDTBillData.CourierId = 0;
                                objDTBillData.CourierName = "";
                                objDTBillData.LocId = 0;
                                objDTBillData.LocName = "";
                                objDTBillData.DelvAddress = "";
                                objDTBillData.Pincode = "";
                                objDTBillData.OrderType = "";
                                objDTBillData.Coupon = "";
                                objDTBillData.CouponAmount = 0;
                                objDTBillData.IRNNo = "";
                                objDTBillData.AckNo = "";
                                objDTBillData.AckDate = DateTime.Now;
                                objDTBillData.QrCodeimage = "";
                                objDTBillData.QrCode = "";
                                objDTBillData.SignedInvoice = "";
                                objDTBillData.BillGSTType = BillGSTType;
                                objDTBillData.InvoiceType = objModel.InvoiceType;
                                billno_ = objDTBillData.BillNo;
                                soldby_ = objDTBillData.SoldBy;
                                fcode_ = objDTBillData.FCode;
                                netpayable_ = objDTBillData.NetPayable;
                                narration_ = "Wallet deducted against " + objDTBillData.UserBillNo + ".";

                                entity.TrnBillDatas.Add(objDTBillData);
                            }

                            TrnCustomerDetail objcustmerDetail = new TrnCustomerDetail();
                            objcustmerDetail.CustomerName = objModel.objCustomer.Name == null ? "" : objModel.objCustomer.Name;
                            objcustmerDetail.MobileNo = objModel.objCustomer.MobileNo == null ? "0" : objModel.objCustomer.MobileNo;
                            objcustmerDetail.ActiveStatus = "Y";
                            objcustmerDetail.BillNo = billno_;
                            objcustmerDetail.Address1 = objModel.objCustomer.Address == null ? "" : objModel.objCustomer.Address;
                            //objcustmerDetail.DeliveryAddress = objModel.objCustomer.Address;
                            entity.TrnCustomerDetails.Add(objcustmerDetail);

                            int i = 0;
                            using (var objDTTrans = entity.Database.BeginTransaction())
                            {
                                try
                                {

                                    i = entity.SaveChanges();
                                    objDTTrans.Commit();
                                    DeductPartyWallet(billno_, narration_, soldby_, "", netpayable_, "shoppe");
                                }
                                catch (DbEntityValidationException e)
                                {
                                    foreach (var eve in e.EntityValidationErrors)
                                    {
                                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                        foreach (var ve in eve.ValidationErrors)
                                        {
                                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                                ve.PropertyName, ve.ErrorMessage);
                                        }
                                    }
                                    throw;
                                }
                                catch (Exception ex)
                                {
                                    objDTTrans.Rollback();
                                }
                            }
                            if (i == objModel.objListProduct.Count + 1)//+1 for TrnCustomerDetails
                            {

                                var resultPayMode = (from r in entity.M_PayModeMaster select r).ToList();
                                foreach (var obj in objDTListPayMode)
                                {
                                    TrnPayModeDetail objTemp = new TrnPayModeDetail();
                                    objTemp = obj;
                                    objTemp.BillType = "G";
                                    objTemp.PayMode = (from r in resultPayMode where r.Prefix.Trim() == obj.PayPrefix.Trim() select r.PayMode).FirstOrDefault();
                                    if (string.IsNullOrEmpty(objTemp.CardNo))
                                    {
                                        objTemp.CardNo = "";
                                    }
                                    entity.TrnPayModeDetails.Add(objTemp);
                                }
                                i = 0;
                                i = entity.SaveChanges();
                                if (i == objDTListPayMode.Count)
                                {
                                    objResponse.ResponseMessage = "Saved Successfully!";
                                    objResponse.ResponseStatus = "OK";
                                    objResponse.ResponseDetailsToPrint = new DistributorBillModel();
                                    objResponse.ResponseDetailsToPrint.BillNo = UserBillNo;
                                    objResponse.ResponseDetailsToPrint.SoldBy = SoldByCode;


                                }
                            }
                        }
                        catch (DbEntityValidationException e)
                        {
                            objResponse.ResponseMessage = "Something went wrong!";
                            objResponse.ResponseStatus = "FAILED";
                        }
                    }
                }

            }
            catch (Exception e)
            {

            }
            return objResponse;
        }

        public int AddShoppeWallet(string refno, string narration, string drto, string crto, decimal amount)
        {
            int i = 0;
            try
            {
                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConnectionString);
                string db = System.Configuration.ConfigurationManager.AppSettings["Database"];

                string query = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                "Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + drto + "','" + crto + "','" + amount + "','" + narration + "','" + refno + "','R','O','Wallet Deducted.',(Select Max(SessID) FROM " + db + "..M_SessnMaster),(Select Max(FSessID) FROM M_FiscalMaster) FROM TrnVoucher ;";

                query += "INSERT INTO TrnVoucher(VoucherNo, VoucherDate, DrTo, Crto, Amount, Narration, Refno, VType, BType, AccDocType, SessID, FSessID) " +
                 "Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + drto + "','" + crto + "','" + amount + "','" + narration + "','" + refno + "','M','O','Wallet Added.',(Select Max(SessID) FROM " + db + "..M_SessnMaster),(Select Max(FSessID) FROM M_FiscalMaster) FROM TrnVoucher ;";

                if (SC.State == ConnectionState.Closed)
                    SC.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = SC;
                i = cmd.ExecuteNonQuery();
                if (SC.State == ConnectionState.Open)
                    SC.Close();
            }
            catch (Exception ex)
            {
            }
            return i;
        }
        public int CreditPartyWallet(string refno, string narration, string drto, string crto, decimal amount, string VType)
        {
            int i = 0;
            try
            {
                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConnectionString);
                string db = System.Configuration.ConfigurationManager.AppSettings["Database"];
                string query = string.Empty;

                if (drto == crto)
                {
                    crto = "";
                }

                query = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
  " Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + drto + "','" + crto + "','" + amount + "','" + narration + "','" + refno + "','" + VType + "','O','Wallet credit.',(Select Max(SessID) FROM " + db + "..M_SessnMaster),(Select Max(FSessID) FROM M_FiscalMaster) FROM TrnVoucher ;";
                if (SC.State == ConnectionState.Closed)
                    SC.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = SC;
                i = cmd.ExecuteNonQuery();
                if (SC.State == ConnectionState.Open)
                    SC.Close();
            }
            catch (Exception ex)
            {
            }
            return i;
        }

        public int DeductPartyWallet(string refno, string narration, string drto, string crto, decimal amount, string UserType)
        {
            int i = 0;
            try
            {
                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConnectionString);
                string db = System.Configuration.ConfigurationManager.AppSettings["Database"];
                string query = string.Empty;

                if (drto == crto)
                {
                    crto = "";
                }

                if (UserType == "shoppe")
                {
                    query = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                   " Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + drto + "','" + crto + "','" + amount + "','" + narration + "','" + refno + "','R','O','Wallet Deducted.',(Select Max(SessID) FROM " + db + "..M_SessnMaster),(Select Max(FSessID) FROM M_FiscalMaster) FROM TrnVoucher ;";
                }
                else
                {
                    query = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                   " Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + drto + "','" + crto + "','" + amount + "','" + narration + "','" + refno + "','M','O','Wallet Deducted.',(Select Max(SessID) FROM " + db + "..M_SessnMaster),(Select Max(FSessID) FROM M_FiscalMaster) FROM TrnVoucher ;";
                }
                if (SC.State == ConnectionState.Closed)
                    SC.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = SC;
                i = cmd.ExecuteNonQuery();
                if (SC.State == ConnectionState.Open)
                    SC.Close();
            }
            catch (Exception ex)
            {
            }
            return i;
        }
        public DistributorBillModel getInvoiceNew(string BillNo, string CurrentPartyCode, string id)
        {
            DistributorBillModel objDistributorModel = new DistributorBillModel();
            try

            {
                using (var entity = new InventoryEntities())
                {
                    decimal? TotCGSTTaxPer = 0;
                    decimal? TotCGSTTaxAmt = 0;
                    decimal? TotSGSTTaxPer = 0;
                    decimal? TotSGSTTaxAmt = 0;
                    decimal? TotalTaxAmt = 0;
                    decimal? TotalTaxPer = 0;
                    objDistributorModel.objListProduct = new List<ProductModel>();
                    objDistributorModel.objListProduct = (from r in entity.TrnBillMains
                                                          join t in entity.TrnBillDetails on r.BillNo equals t.BillNo
                                                          join M in entity.M_ProductMaster on t.ProductId equals M.ProdId
                                                          where r.UserBillNo == BillNo && (id == "F" || r.FCode == id)
                                                          select new ProductModel
                                                          {
                                                              Size = M.PackSize,
                                                              IdNo = r.FCode,
                                                              Mobileno = r.ReceiverMNo,
                                                              PartyName = r.PartyName,
                                                              ProductCodeStr = t.ProductId,
                                                              ProductName = t.ProductName,
                                                              Barcode = t.Barcode,
                                                              BatchNo = t.BatchNo,
                                                              PVValue = t.PVValue,
                                                              Rate = t.Rate,
                                                              MRP = t.MRP,
                                                              DP = (t.ProdType == "F") ? t.DP + (t.Discount / t.Qty) : t.DP,
                                                              Quantity = t.Qty,
                                                              FreeQty = t.FreeQty,
                                                              Amount = t.NetAmount,
                                                              BVValue = t.BvValue,
                                                              CGST = t.CGST,
                                                              Roundoff = r.RndOff,
                                                              SGST = t.SGST,
                                                              CGSTAmount = t.CGSTAmt,
                                                              SGSTAmount = t.SGSTAmt,
                                                              TotalNetPayable = r.NetPayable,
                                                              BillDate = r.BillDate,
                                                              TaxAmt = t.TaxAmount,
                                                              TaxPer = t.Tax,
                                                              //TotalAmount=t.TotalAmount,
                                                              BillSoldBy = r.SoldBy,
                                                              OrderType = r.OrderType,
                                                              TotalBV = r.BvValue,
                                                              TotalQty = r.TotalQty,
                                                              TaxType = t.TaxType,
                                                              ProductType = t.ProdType,
                                                              HSNCode = M.HSNCode,
                                                              billType = r.BillType,
                                                              VoucherNo = r.DcNo,
                                                              VoucherAmt = r.CashReward,
                                                              VDiscountAmt = r.VDiscountAmt + r.DiscountAmt,
                                                              Remarks = r.Remarks,
                                                              OfferUID = t.OfferUID,
                                                              Ftype = r.FType,
                                                              UserName = r.UserName,
                                                              TotalWeight = r.DocWeight,
                                                              OrderNo = r.OrderNo,
                                                              DiscAmt = t.Discount,
                                                              UOM = M.UOM,
                                                              CourierId = r.CourierId,
                                                              CourierName = r.CourierName,
                                                              VehicleNo = r.LR,
                                                              DocketNo = r.DocketNo,
                                                              DocketDate = r.DocketDate,
                                                              Station = r.ReceiverName,
                                                              EWayBillNo = r.Scratch,
                                                              FreightType = r.FreightType,
                                                              TransporterName = r.TransporterName,


                                                          }
                                                         ).ToList();
                    if (objDistributorModel.objListProduct.Count > 0)
                    {

                        objDistributorModel.objPaymentMode = (from r in entity.TrnPayModeDetails
                                                              join b in entity.TrnBillMains on r.BillNo equals b.BillNo
                                                              where b.UserBillNo == BillNo & r.ActiveStatus == "Y"
                                                              select new PaymentModeDetail
                                                              {
                                                                  PayMode = r.PayMode,
                                                                  CardNo = r.CardNo,
                                                                  BillAmt = r.BillAmt,
                                                                  ChqDDNo = r.ChqDDNo,
                                                                  ChqDDDate = r.ChqDDDate,

                                                                  Amount = r.Amount
                                                              }
                                                           ).ToList();

                        string PartyCode = objDistributorModel.objListProduct[0].IdNo;
                        if (objDistributorModel.objListProduct[0].billType == "S")
                            objDistributorModel.Username = (from r in entity.Inv_M_UserMaster where r.BranchCode == PartyCode select r.UserName).FirstOrDefault();

                        //foreach (var obj in objDistributorModel.objListProduct)
                        //{
                        //    obj.DP = Math.Round(obj.DP ?? 0, 2);
                        //    //get expriry batch expirydate
                        //    var expdate = entity.M_BatchMaster.FirstOrDefault(p => p.BatchNo == obj.BatchNo);
                        //    obj.ExpDate = expdate.ExpDate.Date;
                        //}

                        // Step 1: Get all unique batch numbers
                        var batchNos = objDistributorModel.objListProduct
                                        .Where(x => x.BatchNo != null)
                                        .Select(x => x.BatchNo)
                                        .Distinct()
                                        .ToList();
                        // Step 2: Fetch all required batch data in ONE query
                        var batchData = entity.M_BatchMaster
                                        .Where(x => batchNos.Contains(x.BatchNo))
                                        .ToDictionary(x => x.BatchNo, x => x.ExpDate);

                        // Step 3: Loop and assign values
                        foreach (var obj in objDistributorModel.objListProduct)
                        {
                            obj.DP = Math.Round(obj.DP ?? 0, 2);

                            if (obj.BatchNo != null && batchData.ContainsKey(obj.BatchNo))
                            {
                                obj.ExpDate = batchData[obj.BatchNo].Date;
                            }
                        }


                        // }

                        objDistributorModel.IsSequneceproduct = 0;
                        var offerID = objDistributorModel.objListProduct[0].OfferUID;
                        var Offer = (from r in entity.M_OtherOffers where r.OfferID == offerID select r).FirstOrDefault();
                        if (Offer != null)
                        {
                            objDistributorModel.IsSequneceproduct = Offer.StartProduct;
                            objDistributorModel.OfferName = Offer.OfferName;
                        }
                        else
                        {
                            var Offer1 = (from r in entity.M_Offers where r.AID == offerID select r).FirstOrDefault();
                            if (Offer1 != null)
                            {
                                objDistributorModel.OfferName = Offer1.OfferName;
                            }
                            else
                            {
                                objDistributorModel.OfferName = "PC Discounts";
                            }
                        }
                        string db = System.Configuration.ConfigurationManager.AppSettings["Database"];
                        if (objDistributorModel.objListProduct[0].OrderNo != "")
                        {
                            string Sql = " Select Row_number() over(Partition By Orderno Order by BillDate)as SNo,* from TrnBillMain where Orderno='" + objDistributorModel.objListProduct[0].OrderNo + "' Order By BillDate";
                            string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                            SqlConnection SC = new SqlConnection(InvConnectionString);
                            SqlCommand cmd = new SqlCommand();

                            cmd.CommandText = Sql;
                            cmd.Connection = SC;
                            SC.Close();
                            SC.Open();
                            DataTable dt = new DataTable();
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            // this will query your database and return the result to your datatable
                            da.Fill(dt);
                            DataRow[] Dr = dt.Select("UserBillNo='" + BillNo + "'");
                            if (Dr.Length > 0)
                            {

                                objDistributorModel.OrderNo = objDistributorModel.objListProduct[0].OrderNo + "-" + Dr[0]["SNo"].ToString();
                            }
                        }
                        string qrcodeimage = "";
                        //objDistributorModel.BillType = (from r in entity.TrnBillMains where r.UserBillNo == BillNo select r.BillType).FirstOrDefault();
                        //objDistributorModel.InvoiceType = (from r in entity.TrnBillMains where r.UserBillNo == BillNo select r.InvoiceType).FirstOrDefault();
                        //objDistributorModel.PartyInvoice = (from r in entity.TrnBillMains where r.UserBillNo == BillNo select r.OrderType).FirstOrDefault();
                        //qrcodeimage = (from r in entity.TrnBillMains where r.UserBillNo == BillNo select r.QrCodeimage).FirstOrDefault();
                        //objDistributorModel.AckNo = (from r in entity.TrnBillMains where r.UserBillNo == BillNo select r.AckNo).FirstOrDefault();

                        var billMain = entity.TrnBillMains
                                       .Where(r => r.UserBillNo == BillNo)
                                       .Select(r => new
                                       {
                                           r.BillType,
                                           r.InvoiceType,
                                           r.OrderType,
                                           r.QrCodeimage,
                                           r.AckNo,
                                           r.BillNo
                                       })
                                      .FirstOrDefault();
                        objDistributorModel.BillType = billMain.BillType;
                        objDistributorModel.InvoiceType = billMain.InvoiceType;
                        objDistributorModel.PartyInvoice = billMain.OrderType;
                        objDistributorModel.AckNo = billMain.AckNo;
                        if (!string.IsNullOrEmpty(billMain.QrCodeimage))
                        {
                            objDistributorModel.QRCodeImage = qrcodeimage;// "<img src='" + qrcodeimage  +"'/>";
                        }
                        objDistributorModel.objListProduct = objDistributorModel.objListProduct.OrderByDescending(m => m.ProductType).ThenByDescending(m => m.Rate).ToList();
                        decimal? TotalNetAmount = 0;
                        string OrderType = objDistributorModel.objListProduct[0].OrderType;
                        objDistributorModel.objTaxSummary = new List<TaxSummary>();

                        if (OrderType == "T")
                            objDistributorModel.objListProduct = objDistributorModel.objListProduct.Where(m => m.Amount > 0).ToList();
                        //if (OrderType != "T")
                        //{
                        objDistributorModel.objTaxSummary = objDistributorModel.objListProduct

                        .GroupBy(m => new
                        {
                            m.HSNCode,
                            m.TaxPer,
                            m.CGST,
                            m.SGST,

                        }).Select(m => new TaxSummary
                        {
                            HSNCode = m.Key.HSNCode,
                            SumTaxPer = m.Key.TaxPer ?? 0,
                            SumCGSTPer = m.Key.CGST,
                            SumSGSTPer = m.Key.SGST,
                            SumTaxAmt = m.Sum(r => r.TaxAmt) ?? 0,
                            SumCGSTAmt = m.Sum(r => r.CGSTAmount),
                            SumSGSTAmt = m.Sum(r => r.SGSTAmount),
                            SumAmount = m.Sum(r => r.Amount),
                            TotalTaxAmount = m.Sum(p => p.CGSTAmount + p.SGSTAmount + p.TaxAmt) ?? 0,
                            SumNetPayableAmount = m.Sum(p => p.CGSTAmount + p.SGSTAmount + p.TaxAmt + p.Amount) ?? 0
                        }).ToList();
                        //}
                        //else
                        //{
                        //    var Taxresult = (from r in objDistributorModel.objListProduct
                        //                     where r.Rate > 0
                        //                     select new TaxSummary
                        //                     {
                        //                         SumTaxPer = r.TaxPer ?? 0,
                        //                         SumTaxAmt = r.TaxAmt ?? 0,
                        //                         SumCGSTPer = r.CGST,
                        //                         SumCGSTAmt = r.CGSTAmount,
                        //                         SumSGSTPer = r.SGST,
                        //                         SumSGSTAmt = r.SGSTAmount,
                        //                         SumAmount = r.Amount,
                        //                         TotalTaxAmount= (r.TaxAmt + r.CGSTAmount + r.SGSTAmount ) ?? 0,
                        //                         SumNetPayableAmount = (r.TaxAmt + r.CGSTAmount + r.SGSTAmount + r.Amount) ?? 0


                        //                     }).FirstOrDefault();
                        //    objDistributorModel.objTaxSummary.Add(Taxresult);
                        //}
                        objDistributorModel.objTaxSummary = objDistributorModel.objTaxSummary.Where(m => m.SumNetPayableAmount > 0).ToList();


                        decimal TotalNetPayableTobill = 0;
                        TotalNetPayableTobill = objDistributorModel.objTaxSummary.Sum(m => m.SumNetPayableAmount);
                        TotalTaxAmt = objDistributorModel.objTaxSummary.Sum(m => m.TotalTaxAmount);
                        //foreach (var obj in objListproduct)
                        //{
                        //    objDistributorModel.objListProduct.Add(obj);
                        //    TotalNetAmount += obj.Amount;
                        //    TotalTaxPer += obj.TaxPer;
                        //    TotalTaxAmt += obj.TaxAmt;
                        //    TotCGSTTaxPer += obj.CGST;
                        //    TotCGSTTaxAmt += obj.CGSTAmount;
                        //    TotSGSTTaxPer += obj.SGST;
                        //    TotSGSTTaxAmt += obj.SGSTAmount;
                        //}
                        var result = (from r in entity.M_CompanyMaster where r.ActiveStatus == "Y" select r).FirstOrDefault();
                        if (result != null)
                        {
                            objDistributorModel.CompCity = result.CompCity;
                            string SoldBy = objDistributorModel.objListProduct[0].BillSoldBy;
                            var resultDetails = (from r in entity.M_LedgerMaster where r.PartyCode == SoldBy select r).FirstOrDefault();
                            if (resultDetails != null)
                            {
                                objDistributorModel.GSTNo = resultDetails.TinNo;
                                objDistributorModel.SoldByName = resultDetails.PartyName;
                                objDistributorModel.SoldByAddress = resultDetails.Address1;
                                objDistributorModel.SoldByCity = resultDetails.CityName;
                                objDistributorModel.IsGSTRegistered = resultDetails.NewFld3;
                                objDistributorModel.PANNo = resultDetails.PanNo;
                                objDistributorModel.CINNo = resultDetails.NewFld1;
                            }
                            objDistributorModel.BillNo = BillNo;
                            objDistributorModel.BillDate = objDistributorModel.objListProduct[0].BillDate.Date;
                            objDistributorModel.CompanyName = result.CompName;
                            objDistributorModel.CompanyAdd = result.CompAdd;


                            objDistributorModel.objCustomer = new CustomerDetail();
                            //if Idno is in M_Ledgermaster then get details from there else from sjlabs.M_Memvbermaster
                            var Fcode = objDistributorModel.objListProduct[0].IdNo;
                            var CustomerResult = (from r in entity.M_LedgerMaster where r.PartyCode == Fcode select r).FirstOrDefault();
                            if (CustomerResult != null)
                            {
                                var StateName = (from r in entity.M_StateDivMaster where r.StateCode == CustomerResult.StateCode select r.StateName).FirstOrDefault();
                                objDistributorModel.objCustomer.StateName = StateName.ToString();
                                objDistributorModel.objCustomer.IdNo = CustomerResult.PartyCode;
                                objDistributorModel.objCustomer.Name = CustomerResult.PartyName;
                                objDistributorModel.objCustomer.Address = CustomerResult.Address1 + ' ' + CustomerResult.CityName + ' ' + StateName + '-' + CustomerResult.PinCode;
                                objDistributorModel.objCustomer.MobileNo = CustomerResult.MobileNo.ToString();
                                objDistributorModel.objCustomer.GSTNo = CustomerResult.TinNo;
                                objDistributorModel.objCustomer.PANNo = CustomerResult.PanNo;
                                objDistributorModel.objCustomer.CityName = CustomerResult.CityName;
                                objDistributorModel.objCustomer.StateCode = CustomerResult.StateCode;
                                objDistributorModel.objCustomer.CardNo = "Shoppe Code";
                                objDistributorModel.objCustomer.CustomerType = "Shoppe Name";
                                objDistributorModel.objCustomer.IsRegisteredCustomer = false;

                                try
                                {
                                    var delvPlace = entity.TrnBillMains
                                   .Where(x => x.FCode == objDistributorModel.objCustomer.IdNo)
                                   .OrderByDescending(x => x.BillId)
                                   .Select(x => x.DelvPlace)
                                   .FirstOrDefault();
                                    if (delvPlace != null)
                                    {
                                        objDistributorModel.objCustomer.DeliveryAddress = delvPlace;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            else
                            {
                                if (objDistributorModel.BillType != "J" && objDistributorModel.BillType != "X")
                                {
                                    objDistributorModel.objCustomer = GetCustInfo(objDistributorModel.objListProduct[0].IdNo);
                                    objDistributorModel.objCustomer.CityName = objDistributorModel.objCustomer.CityName + "[" + objDistributorModel.objCustomer.StateName + "]";
                                }
                                else
                                {
                                    objDistributorModel.objCustomer = GetSJPCustInfo(objDistributorModel.objListProduct[0].IdNo);
                                    int NumberOfbill = entity.sp_GetNoOfJackpotBill(objDistributorModel.objListProduct[0].IdNo).DefaultIfEmpty(0).FirstOrDefault() ?? 0;
                                    objDistributorModel.NumberOfBill = NumberOfbill;
                                }
                                if (objDistributorModel.objCustomer.IdNo == "Record does not exists!")
                                {
                                    objDistributorModel.objCustomer = new CustomerDetail();
                                    objDistributorModel.objCustomer.IdNo = objDistributorModel.objListProduct[0].IdNo;
                                    objDistributorModel.objCustomer.Name = objDistributorModel.objListProduct[0].PartyName;
                                    //var addresses = entity.TrnCustomerDetails
                                    //                .OrderByDescending(c => c.AId)
                                    //                .Select(c => c.Address1)
                                    //                .FirstOrDefault();

                                    var address = (from r in entity.TrnCustomerDetails
                                                   join b in entity.TrnBillMains on r.BillNo equals b.BillNo
                                                   where b.UserBillNo == BillNo && r.ActiveStatus == "Y"
                                                   select new CustomerDetail
                                                   {
                                                       Address = r.Address1
                                                   }).FirstOrDefault();
                                    objDistributorModel.objCustomer.Address = address.Address;
                                    objDistributorModel.objCustomer.CityName = objDistributorModel.objCustomer.CityName + "[" + objDistributorModel.objCustomer.StateName + "]";
                                    objDistributorModel.objCustomer.MobileNo = objDistributorModel.objListProduct[0].Mobileno;
                                    objDistributorModel.objCustomer.PANNo = "";
                                    objDistributorModel.objCustomer.CardNo = "Customer Name";
                                    objDistributorModel.objCustomer.CustomerType = "Customer Name";
                                    objDistributorModel.objCustomer.IsRegisteredCustomer = true;
                                    try
                                    {
                                        var delvPlace = entity.TrnBillMains
                                       .Where(x => x.FCode == objDistributorModel.objCustomer.IdNo)
                                       .OrderByDescending(x => x.BillId)
                                       .Select(x => x.DelvPlace)
                                       .FirstOrDefault();
                                        if (delvPlace != null)
                                        {
                                            objDistributorModel.objCustomer.DeliveryAddress = delvPlace;
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                else
                                {
                                    objDistributorModel.objCustomer.CardNo = "PC ID";
                                    objDistributorModel.objCustomer.CustomerType = "PC Name";
                                    objDistributorModel.objCustomer.IsRegisteredCustomer = false;
                                }
                            }
                            objDistributorModel.StateGSTName = GetStateGstName(objDistributorModel.objCustomer.StateCode);
                            objDistributorModel.objProduct = new ProductModel();
                            objDistributorModel.objProduct.TotalTaxPer = TotalTaxPer ?? 0;
                            objDistributorModel.objProduct.TotalTaxAmount = TotalTaxAmt ?? 0;
                            objDistributorModel.objProduct.TotalCGSTPer = TotCGSTTaxPer ?? 0;
                            objDistributorModel.objProduct.TotalCGSTAmt = TotCGSTTaxAmt ?? 0;
                            objDistributorModel.objProduct.TotalSGSTPer = TotSGSTTaxPer ?? 0;
                            objDistributorModel.objProduct.TotalSGSTAmt = TotSGSTTaxAmt ?? 0;
                            //objDistributorModel.objProduct.TotalNetPayable = objDistributorModel.objListProduct[0].TotalNetPayable;
                            objDistributorModel.objProduct.TotalNetPayable = TotalNetPayableTobill;
                            objDistributorModel.objProduct.TotalAmount = TotalNetAmount ?? 0;
                            objDistributorModel.Username = objDistributorModel.objListProduct[0].UserName;
                            objDistributorModel.objProduct.Roundoff = objDistributorModel.objListProduct[0].Roundoff;
                            objDistributorModel.objProduct.TotalAmount = objDistributorModel.objListProduct[0].TotalAmount;
                            objDistributorModel.objProduct.TotalBV = objDistributorModel.objListProduct[0].TotalBV;
                            objDistributorModel.objProduct.TotalQty = objDistributorModel.objListProduct[0].TotalQty;
                            objDistributorModel.objProduct.TotalWeight = objDistributorModel.objListProduct[0].TotalWeight;

                            if (objDistributorModel.BillType == "B")
                            {
                                decimal adjustmonut = 0;
                                var billno = (from p in entity.TrnBillMains where p.UserBillNo == objDistributorModel.BillNo select p.BillNo).FirstOrDefault();
                                if (billno != null)
                                {
                                    Models.FPVoucher Fpvoucher = new Models.FPVoucher();
                                    Fpvoucher.Amount = 0;
                                    var fpVoucherData = (from r in entity.FPVoucherUseds
                                                         where r.BillNo == billno
                                                         select new
                                                         {
                                                             Amount = r.AdjustAmount,
                                                         }).FirstOrDefault();
                                    if (fpVoucherData != null)
                                    {
                                        Fpvoucher.Amount = fpVoucherData.Amount;
                                        //objDistributorModel.FpVoucher = fpVoucherData.code;
                                    }
                                    Coupon Coupon = new Coupon();
                                    Coupon.Amount = 0;
                                    var Couponamt = (from r in entity.Coupons
                                                     where r.BillNo == billno && r.IdNo == objDistributorModel.objCustomer.IdNo
                                                     select new
                                                     {
                                                         Amount = r.Amount,
                                                         code = r.Code
                                                     }).FirstOrDefault();
                                    if (Couponamt != null)
                                    {
                                        Coupon.Amount = Couponamt.Amount;
                                        objDistributorModel.CouponCode = Couponamt.code;
                                    }
                                    objDistributorModel.CouponAmt = Convert.ToDecimal(Coupon.Amount);
                                    objDistributorModel.FpVoucherAmt = Convert.ToDecimal(Fpvoucher.Amount);

                                    //if (Coupon.Amount != 0 && Fpvoucher.Amount == 0)
                                    //{
                                    //    if (TotalNetPayableTobill <= Coupon.Amount)
                                    //    {
                                    //        objDistributorModel.CouponAmt = TotalNetPayableTobill;
                                    //    }
                                    //    else
                                    //    {
                                    //        adjustmonut = TotalNetPayableTobill - Convert.ToDecimal(Coupon.Amount);
                                    //        objDistributorModel.CouponAmt = TotalNetPayableTobill - Convert.ToDecimal(Coupon.Amount);
                                    //    }
                                    //}
                                    //else if (Coupon.Amount == 0 && Fpvoucher.Amount != 0)
                                    //{
                                    //    if (TotalNetPayableTobill <= Fpvoucher.Amount)
                                    //    {
                                    //        objDistributorModel.FpVoucherAmt = TotalNetPayableTobill;
                                    //    }
                                    //    else
                                    //    {
                                    //        adjustmonut = TotalNetPayableTobill - Convert.ToDecimal(Coupon.Amount);
                                    //        objDistributorModel.FpVoucherAmt = TotalNetPayableTobill - Convert.ToDecimal(Fpvoucher.Amount);
                                    //    }
                                    //}
                                    //else if (Coupon.Amount != 0 && Fpvoucher.Amount != 0)
                                    //{
                                    //    decimal temptot = Convert.ToDecimal(Fpvoucher.Amount) + Convert.ToDecimal(Coupon.Amount);
                                    //    if (TotalNetPayableTobill <= temptot)
                                    //    {
                                    //        objDistributorModel.FpVoucherAmt = TotalNetPayableTobill - Convert.ToDecimal(Coupon.Amount);
                                    //        objDistributorModel.CouponAmt = Convert.ToDecimal(Coupon.Amount);
                                    //    }
                                    //    else
                                    //    {
                                    //        adjustmonut = TotalNetPayableTobill - Convert.ToDecimal(temptot);
                                    //        objDistributorModel.FpVoucherAmt = Convert.ToDecimal(Fpvoucher.Amount);
                                    //        objDistributorModel.CouponAmt = Convert.ToDecimal(Coupon.Amount);
                                    //    }
                                    //}
                                }
                            }



                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return objDistributorModel;
        }
        public DistributorBillModel getInvoice(string BillNo, string CurrentPartyCode, string id)
        {
            DistributorBillModel objDistributorModel = new DistributorBillModel();
            try

            {
                using (var entity = new InventoryEntities())
                {
                    decimal? TotCGSTTaxPer = 0;
                    decimal? TotCGSTTaxAmt = 0;
                    decimal? TotSGSTTaxPer = 0;
                    decimal? TotSGSTTaxAmt = 0;
                    decimal? TotalTaxAmt = 0;
                    decimal? TotalTaxPer = 0;
                    objDistributorModel.objListProduct = new List<ProductModel>();
                    objDistributorModel.objListProduct = (from r in entity.TrnBillMains
                                                          join t in entity.TrnBillDetails on r.BillNo equals t.BillNo
                                                          join M in entity.M_ProductMaster on t.ProductId equals M.ProdId
                                                          where r.UserBillNo == BillNo && (id == "F" || r.FCode == id)
                                                          select new ProductModel
                                                          {
                                                              Size = M.PackSize,
                                                              IdNo = r.FCode,
                                                              Mobileno = r.ReceiverMNo,
                                                              PartyName = r.PartyName,
                                                              ProductCodeStr = t.ProductId,
                                                              ProductName = t.ProductName,
                                                              Barcode = t.Barcode,
                                                              BatchNo = t.BatchNo,
                                                              PVValue = t.PVValue,
                                                              Rate = t.Rate,
                                                              MRP = t.MRP,
                                                              DP = (t.ProdType == "F") ? t.DP + (t.Discount / t.Qty) : t.DP,
                                                              Quantity = t.Qty,
                                                              FreeQty = t.FreeQty,
                                                              Amount = t.NetAmount,
                                                              BVValue = t.BvValue,
                                                              CGST = t.CGST,
                                                              Roundoff = r.RndOff,
                                                              SGST = t.SGST,
                                                              CGSTAmount = t.CGSTAmt,
                                                              SGSTAmount = t.SGSTAmt,
                                                              TotalNetPayable = r.NetPayable,
                                                              BillDate = r.BillDate,
                                                              TaxAmt = t.TaxAmount,
                                                              TaxPer = t.Tax,
                                                              //TotalAmount=t.TotalAmount,
                                                              BillSoldBy = r.SoldBy,
                                                              OrderType = r.OrderType,
                                                              TotalBV = r.BvValue,
                                                              TotalQty = r.TotalQty,
                                                              TaxType = t.TaxType,
                                                              ProductType = t.ProdType,
                                                              HSNCode = M.HSNCode,
                                                              billType = r.BillType,
                                                              VoucherNo = r.DcNo,
                                                              VoucherAmt = r.CashReward,
                                                              VDiscountAmt = r.VDiscountAmt + r.DiscountAmt,
                                                              Remarks = r.Remarks,
                                                              OfferUID = t.OfferUID,
                                                              Ftype = r.FType,
                                                              UserName = r.UserName,
                                                              TotalWeight = r.DocWeight,
                                                              OrderNo = r.OrderNo,
                                                              DiscAmt = t.Discount,
                                                              UOM = M.UOM,
                                                              CourierId = r.CourierId,
                                                              CourierName = r.CourierName,
                                                              VehicleNo = r.LR,
                                                              DocketNo = r.DocketNo,
                                                              DocketDate = r.DocketDate,
                                                              Station = r.ReceiverName,
                                                              EWayBillNo = r.Scratch,
                                                              FreightType = r.FreightType,
                                                              TransporterName = r.TransporterName,
                                                              

                                                          }
                                                         ).ToList();
                    if (objDistributorModel.objListProduct.Count > 0)
                    {

                        objDistributorModel.objPaymentMode = (from r in entity.TrnPayModeDetails
                                                              join b in entity.TrnBillMains on r.BillNo equals b.BillNo
                                                              where b.UserBillNo == BillNo & r.ActiveStatus == "Y"
                                                              select new PaymentModeDetail
                                                              {
                                                                  PayMode = r.PayMode,
                                                                  CardNo = r.CardNo,
                                                                  BillAmt = r.BillAmt,
                                                                  ChqDDNo = r.ChqDDNo,
                                                                  ChqDDDate = r.ChqDDDate,

                                                                  Amount = r.Amount
                                                              }
                                                           ).ToList();

                        string PartyCode = objDistributorModel.objListProduct[0].IdNo;
                        if (objDistributorModel.objListProduct[0].billType == "S")
                            objDistributorModel.Username = (from r in entity.Inv_M_UserMaster where r.BranchCode == PartyCode select r.UserName).FirstOrDefault();

                        //foreach (var obj in objDistributorModel.objListProduct)
                        //{
                        //    obj.DP = Math.Round(obj.DP ?? 0, 2);
                        //    //get expriry batch expirydate
                        //    var expdate = entity.M_BatchMaster.FirstOrDefault(p => p.BatchNo == obj.BatchNo);
                        //    obj.ExpDate = expdate.ExpDate.Date;
                        //}

                        // Step 1: Get all unique batch numbers
                        var batchNos = objDistributorModel.objListProduct
                                        .Where(x => x.BatchNo != null)
                                        .Select(x => x.BatchNo)
                                        .Distinct()
                                        .ToList();
                        // Step 2: Fetch all required batch data in ONE query
                        var batchData = entity.M_BatchMaster
                                        .Where(x => batchNos.Contains(x.BatchNo))
                                        .ToDictionary(x => x.BatchNo, x => x.ExpDate);

                        // Step 3: Loop and assign values
                        foreach (var obj in objDistributorModel.objListProduct)
                        {
                            obj.DP = Math.Round(obj.DP ?? 0, 2);

                            if (obj.BatchNo != null && batchData.ContainsKey(obj.BatchNo))
                            {
                                obj.ExpDate = batchData[obj.BatchNo].Date;
                            }
                        }


                        // }

                        objDistributorModel.IsSequneceproduct = 0;
                        var offerID = objDistributorModel.objListProduct[0].OfferUID;
                        var Offer = (from r in entity.M_OtherOffers where r.OfferID == offerID select r).FirstOrDefault();
                        if (Offer != null)
                        {
                            objDistributorModel.IsSequneceproduct = Offer.StartProduct;
                            objDistributorModel.OfferName = Offer.OfferName;
                        }
                        else
                        {
                            var Offer1 = (from r in entity.M_Offers where r.AID == offerID select r).FirstOrDefault();
                            if (Offer1 != null)
                            {
                                objDistributorModel.OfferName = Offer1.OfferName;
                            }
                            else
                            {
                                objDistributorModel.OfferName = "PC Discounts";
                            }
                        }
                        string db = System.Configuration.ConfigurationManager.AppSettings["Database"];
                        if (objDistributorModel.objListProduct[0].OrderNo != "")
                        {
                            string Sql = " Select Row_number() over(Partition By Orderno Order by BillDate)as SNo,* from TrnBillMain where Orderno='" + objDistributorModel.objListProduct[0].OrderNo + "' Order By BillDate";
                            string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                            SqlConnection SC = new SqlConnection(InvConnectionString);
                            SqlCommand cmd = new SqlCommand();

                            cmd.CommandText = Sql;
                            cmd.Connection = SC;
                            SC.Close();
                            SC.Open();
                            DataTable dt = new DataTable();
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            // this will query your database and return the result to your datatable
                            da.Fill(dt);
                            DataRow[] Dr = dt.Select("UserBillNo='" + BillNo + "'");
                            if (Dr.Length > 0)
                            {

                                objDistributorModel.OrderNo = objDistributorModel.objListProduct[0].OrderNo + "-" + Dr[0]["SNo"].ToString();
                            }
                        }
                        string qrcodeimage = "";
                        //objDistributorModel.BillType = (from r in entity.TrnBillMains where r.UserBillNo == BillNo select r.BillType).FirstOrDefault();
                        //objDistributorModel.InvoiceType = (from r in entity.TrnBillMains where r.UserBillNo == BillNo select r.InvoiceType).FirstOrDefault();
                        //objDistributorModel.PartyInvoice = (from r in entity.TrnBillMains where r.UserBillNo == BillNo select r.OrderType).FirstOrDefault();
                        //qrcodeimage = (from r in entity.TrnBillMains where r.UserBillNo == BillNo select r.QrCodeimage).FirstOrDefault();
                        //objDistributorModel.AckNo = (from r in entity.TrnBillMains where r.UserBillNo == BillNo select r.AckNo).FirstOrDefault();

                        var billMain = entity.TrnBillMains
                                       .Where(r => r.UserBillNo == BillNo)
                                       .Select(r => new
                                       {
                                           r.BillType,
                                           r.InvoiceType,
                                           r.OrderType,
                                           r.QrCodeimage,
                                           r.AckNo,
                                           r.BillNo
                                       })
                                      .FirstOrDefault();
                        objDistributorModel.BillType = billMain.BillType;
                        objDistributorModel.InvoiceType = billMain.InvoiceType;
                        objDistributorModel.PartyInvoice = billMain.OrderType;
                        objDistributorModel.AckNo = billMain.AckNo;
                        if (!string.IsNullOrEmpty(billMain.QrCodeimage))
                        {
                            objDistributorModel.QRCodeImage = qrcodeimage;// "<img src='" + qrcodeimage  +"'/>";
                        }
                        objDistributorModel.objListProduct = objDistributorModel.objListProduct.OrderByDescending(m => m.ProductType).ThenByDescending(m => m.Rate).ToList();
                        decimal? TotalNetAmount = 0;
                        string OrderType = objDistributorModel.objListProduct[0].OrderType;
                        objDistributorModel.objTaxSummary = new List<TaxSummary>();

                        if (OrderType == "T")
                            objDistributorModel.objListProduct = objDistributorModel.objListProduct.Where(m => m.Amount > 0).ToList();
                        //if (OrderType != "T")
                        //{
                        objDistributorModel.objTaxSummary = objDistributorModel.objListProduct

                        .GroupBy(m => new
                        {
                            m.TaxPer,
                            m.CGST,
                            m.SGST,

                        }).Select(m => new TaxSummary
                        {
                            SumTaxPer = m.Key.TaxPer ?? 0,
                            SumCGSTPer = m.Key.CGST,
                            SumSGSTPer = m.Key.SGST,
                            SumTaxAmt = m.Sum(r => r.TaxAmt) ?? 0,
                            SumCGSTAmt = m.Sum(r => r.CGSTAmount),
                            SumSGSTAmt = m.Sum(r => r.SGSTAmount),
                            SumAmount = m.Sum(r => r.Amount),
                            TotalTaxAmount = m.Sum(p => p.CGSTAmount + p.SGSTAmount + p.TaxAmt) ?? 0,
                            SumNetPayableAmount = m.Sum(p => p.CGSTAmount + p.SGSTAmount + p.TaxAmt + p.Amount) ?? 0
                        }).ToList();
                        //}
                        //else
                        //{
                        //    var Taxresult = (from r in objDistributorModel.objListProduct
                        //                     where r.Rate > 0
                        //                     select new TaxSummary
                        //                     {
                        //                         SumTaxPer = r.TaxPer ?? 0,
                        //                         SumTaxAmt = r.TaxAmt ?? 0,
                        //                         SumCGSTPer = r.CGST,
                        //                         SumCGSTAmt = r.CGSTAmount,
                        //                         SumSGSTPer = r.SGST,
                        //                         SumSGSTAmt = r.SGSTAmount,
                        //                         SumAmount = r.Amount,
                        //                         TotalTaxAmount= (r.TaxAmt + r.CGSTAmount + r.SGSTAmount ) ?? 0,
                        //                         SumNetPayableAmount = (r.TaxAmt + r.CGSTAmount + r.SGSTAmount + r.Amount) ?? 0


                        //                     }).FirstOrDefault();
                        //    objDistributorModel.objTaxSummary.Add(Taxresult);
                        //}
                        objDistributorModel.objTaxSummary = objDistributorModel.objTaxSummary.Where(m => m.SumNetPayableAmount > 0).ToList();


                        decimal TotalNetPayableTobill = 0;
                        TotalNetPayableTobill = objDistributorModel.objTaxSummary.Sum(m => m.SumNetPayableAmount);
                        TotalTaxAmt = objDistributorModel.objTaxSummary.Sum(m => m.TotalTaxAmount);
                        //foreach (var obj in objListproduct)
                        //{
                        //    objDistributorModel.objListProduct.Add(obj);
                        //    TotalNetAmount += obj.Amount;
                        //    TotalTaxPer += obj.TaxPer;
                        //    TotalTaxAmt += obj.TaxAmt;
                        //    TotCGSTTaxPer += obj.CGST;
                        //    TotCGSTTaxAmt += obj.CGSTAmount;
                        //    TotSGSTTaxPer += obj.SGST;
                        //    TotSGSTTaxAmt += obj.SGSTAmount;
                        //}
                        var result = (from r in entity.M_CompanyMaster where r.ActiveStatus == "Y" select r).FirstOrDefault();
                        if (result != null)
                        {
                            objDistributorModel.CompCity = result.CompCity;
                            string SoldBy = objDistributorModel.objListProduct[0].BillSoldBy;
                            var resultDetails = (from r in entity.M_LedgerMaster where r.PartyCode == SoldBy select r).FirstOrDefault();
                            if (resultDetails != null)
                            {
                                objDistributorModel.GSTNo = resultDetails.TinNo;
                                objDistributorModel.SoldByName = resultDetails.PartyName;
                                objDistributorModel.SoldByAddress = resultDetails.Address1;
                                objDistributorModel.SoldByCity = resultDetails.CityName;
                                objDistributorModel.IsGSTRegistered = resultDetails.NewFld3;
                                objDistributorModel.PANNo = resultDetails.PanNo;
                                objDistributorModel.CINNo = resultDetails.NewFld1;
                            }
                            objDistributorModel.BillNo = BillNo;
                            objDistributorModel.BillDate = objDistributorModel.objListProduct[0].BillDate.Date;
                            objDistributorModel.CompanyName = result.CompName;
                            objDistributorModel.CompanyAdd = result.CompAdd;


                            objDistributorModel.objCustomer = new CustomerDetail();
                            //if Idno is in M_Ledgermaster then get details from there else from sjlabs.M_Memvbermaster
                            var Fcode = objDistributorModel.objListProduct[0].IdNo;
                            var CustomerResult = (from r in entity.M_LedgerMaster where r.PartyCode == Fcode select r).FirstOrDefault();
                            if (CustomerResult != null)
                            {
                                var StateName = (from r in entity.M_StateDivMaster where r.StateCode == CustomerResult.StateCode select r.StateName).FirstOrDefault();
                                objDistributorModel.objCustomer.StateName = StateName.ToString();
                                objDistributorModel.objCustomer.IdNo = CustomerResult.PartyCode;
                                objDistributorModel.objCustomer.Name = CustomerResult.PartyName;
                                objDistributorModel.objCustomer.Address = CustomerResult.Address1 + ' ' + CustomerResult.CityName + ' ' + StateName + '-' + CustomerResult.PinCode;
                                objDistributorModel.objCustomer.MobileNo = CustomerResult.MobileNo.ToString();
                                objDistributorModel.objCustomer.GSTNo = CustomerResult.TinNo;
                                objDistributorModel.objCustomer.PANNo = CustomerResult.PanNo;
                                objDistributorModel.objCustomer.CityName = CustomerResult.CityName;
                                objDistributorModel.objCustomer.StateCode = CustomerResult.StateCode;
                                objDistributorModel.objCustomer.CardNo = "Shoppe Code";
                                objDistributorModel.objCustomer.CustomerType = "Shoppe Name";
                                objDistributorModel.objCustomer.IsRegisteredCustomer = false;

                                try
                                {
                                    var delvPlace = entity.TrnBillMains
                                   .Where(x => x.FCode == objDistributorModel.objCustomer.IdNo)
                                   .OrderByDescending(x => x.BillId)
                                   .Select(x => x.DelvPlace)
                                   .FirstOrDefault();
                                    if (delvPlace != null)
                                    {
                                        objDistributorModel.objCustomer.DeliveryAddress = delvPlace;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            else
                            {
                                if (objDistributorModel.BillType != "J" && objDistributorModel.BillType != "X")
                                {
                                    objDistributorModel.objCustomer = GetCustInfo(objDistributorModel.objListProduct[0].IdNo);
                                    objDistributorModel.objCustomer.CityName = objDistributorModel.objCustomer.CityName + "[" + objDistributorModel.objCustomer.StateName + "]";
                                }
                                else
                                {
                                    objDistributorModel.objCustomer = GetSJPCustInfo(objDistributorModel.objListProduct[0].IdNo);
                                    int NumberOfbill = entity.sp_GetNoOfJackpotBill(objDistributorModel.objListProduct[0].IdNo).DefaultIfEmpty(0).FirstOrDefault() ?? 0;
                                    objDistributorModel.NumberOfBill = NumberOfbill;
                                }
                                if (objDistributorModel.objCustomer.IdNo == "Record does not exists!")
                                {
                                    objDistributorModel.objCustomer = new CustomerDetail();
                                    objDistributorModel.objCustomer.IdNo = objDistributorModel.objListProduct[0].IdNo;
                                    objDistributorModel.objCustomer.Name = objDistributorModel.objListProduct[0].PartyName;
                                    //var addresses = entity.TrnCustomerDetails
                                    //                .OrderByDescending(c => c.AId)
                                    //                .Select(c => c.Address1)
                                    //                .FirstOrDefault();

                                    var address = (from r in entity.TrnCustomerDetails
                                                   join b in entity.TrnBillMains on r.BillNo equals b.BillNo
                                                   where b.UserBillNo == BillNo && r.ActiveStatus == "Y"
                                                   select new CustomerDetail
                                                   {
                                                       Address = r.Address1
                                                   }).FirstOrDefault();
                                    objDistributorModel.objCustomer.Address = address.Address;
                                    objDistributorModel.objCustomer.CityName = objDistributorModel.objCustomer.CityName + "[" + objDistributorModel.objCustomer.StateName + "]";
                                    objDistributorModel.objCustomer.MobileNo = objDistributorModel.objListProduct[0].Mobileno;
                                    objDistributorModel.objCustomer.PANNo = "";
                                    objDistributorModel.objCustomer.CardNo = "Customer Name";
                                    objDistributorModel.objCustomer.CustomerType = "Customer Name";
                                    objDistributorModel.objCustomer.IsRegisteredCustomer = true;
                                    try
                                    {
                                        var delvPlace = entity.TrnBillMains
                                       .Where(x => x.FCode == objDistributorModel.objCustomer.IdNo)
                                       .OrderByDescending(x => x.BillId)
                                       .Select(x => x.DelvPlace)
                                       .FirstOrDefault();
                                        if (delvPlace != null)
                                        {
                                            objDistributorModel.objCustomer.DeliveryAddress = delvPlace;
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                else
                                {
                                    objDistributorModel.objCustomer.CardNo = "PC ID";
                                    objDistributorModel.objCustomer.CustomerType = "PC Name";
                                    objDistributorModel.objCustomer.IsRegisteredCustomer = false;
                                }
                            }
                            objDistributorModel.StateGSTName = GetStateGstName(objDistributorModel.objCustomer.StateCode);
                            objDistributorModel.objProduct = new ProductModel();
                            objDistributorModel.objProduct.TotalTaxPer = TotalTaxPer ?? 0;
                            objDistributorModel.objProduct.TotalTaxAmount = TotalTaxAmt ?? 0;
                            objDistributorModel.objProduct.TotalCGSTPer = TotCGSTTaxPer ?? 0;
                            objDistributorModel.objProduct.TotalCGSTAmt = TotCGSTTaxAmt ?? 0;
                            objDistributorModel.objProduct.TotalSGSTPer = TotSGSTTaxPer ?? 0;
                            objDistributorModel.objProduct.TotalSGSTAmt = TotSGSTTaxAmt ?? 0;
                            //objDistributorModel.objProduct.TotalNetPayable = objDistributorModel.objListProduct[0].TotalNetPayable;
                            objDistributorModel.objProduct.TotalNetPayable = TotalNetPayableTobill;
                            objDistributorModel.objProduct.TotalAmount = TotalNetAmount ?? 0;
                            objDistributorModel.Username = objDistributorModel.objListProduct[0].UserName;
                            objDistributorModel.objProduct.Roundoff = objDistributorModel.objListProduct[0].Roundoff;
                            objDistributorModel.objProduct.TotalAmount = objDistributorModel.objListProduct[0].TotalAmount;
                            objDistributorModel.objProduct.TotalBV = objDistributorModel.objListProduct[0].TotalBV;
                            objDistributorModel.objProduct.TotalQty = objDistributorModel.objListProduct[0].TotalQty;
                            objDistributorModel.objProduct.TotalWeight = objDistributorModel.objListProduct[0].TotalWeight;

                            if (objDistributorModel.BillType == "B")
                            {
                                decimal adjustmonut = 0;
                                var billno = (from p in entity.TrnBillMains where p.UserBillNo == objDistributorModel.BillNo select p.BillNo).FirstOrDefault();
                                if (billno != null)
                                {
                                    Models.FPVoucher Fpvoucher = new Models.FPVoucher();
                                    Fpvoucher.Amount = 0;
                                    var fpVoucherData = (from r in entity.FPVoucherUseds
                                                         where r.BillNo == billno
                                                         select new
                                                         {
                                                             Amount = r.AdjustAmount,
                                                         }).FirstOrDefault();
                                    if (fpVoucherData != null)
                                    {
                                        Fpvoucher.Amount = fpVoucherData.Amount;
                                        //objDistributorModel.FpVoucher = fpVoucherData.code;
                                    }
                                    Coupon Coupon = new Coupon();
                                    Coupon.Amount = 0;
                                    var Couponamt = (from r in entity.Coupons
                                                     where r.BillNo == billno && r.IdNo == objDistributorModel.objCustomer.IdNo
                                                     select new
                                                     {
                                                         Amount = r.Amount,
                                                         code = r.Code
                                                     }).FirstOrDefault();
                                    if (Couponamt != null)
                                    {
                                        Coupon.Amount = Couponamt.Amount;
                                        objDistributorModel.CouponCode = Couponamt.code;
                                    }
                                    objDistributorModel.CouponAmt = Convert.ToDecimal(Coupon.Amount);
                                    objDistributorModel.FpVoucherAmt = Convert.ToDecimal(Fpvoucher.Amount);

                                    //if (Coupon.Amount != 0 && Fpvoucher.Amount == 0)
                                    //{
                                    //    if (TotalNetPayableTobill <= Coupon.Amount)
                                    //    {
                                    //        objDistributorModel.CouponAmt = TotalNetPayableTobill;
                                    //    }
                                    //    else
                                    //    {
                                    //        adjustmonut = TotalNetPayableTobill - Convert.ToDecimal(Coupon.Amount);
                                    //        objDistributorModel.CouponAmt = TotalNetPayableTobill - Convert.ToDecimal(Coupon.Amount);
                                    //    }
                                    //}
                                    //else if (Coupon.Amount == 0 && Fpvoucher.Amount != 0)
                                    //{
                                    //    if (TotalNetPayableTobill <= Fpvoucher.Amount)
                                    //    {
                                    //        objDistributorModel.FpVoucherAmt = TotalNetPayableTobill;
                                    //    }
                                    //    else
                                    //    {
                                    //        adjustmonut = TotalNetPayableTobill - Convert.ToDecimal(Coupon.Amount);
                                    //        objDistributorModel.FpVoucherAmt = TotalNetPayableTobill - Convert.ToDecimal(Fpvoucher.Amount);
                                    //    }
                                    //}
                                    //else if (Coupon.Amount != 0 && Fpvoucher.Amount != 0)
                                    //{
                                    //    decimal temptot = Convert.ToDecimal(Fpvoucher.Amount) + Convert.ToDecimal(Coupon.Amount);
                                    //    if (TotalNetPayableTobill <= temptot)
                                    //    {
                                    //        objDistributorModel.FpVoucherAmt = TotalNetPayableTobill - Convert.ToDecimal(Coupon.Amount);
                                    //        objDistributorModel.CouponAmt = Convert.ToDecimal(Coupon.Amount);
                                    //    }
                                    //    else
                                    //    {
                                    //        adjustmonut = TotalNetPayableTobill - Convert.ToDecimal(temptot);
                                    //        objDistributorModel.FpVoucherAmt = Convert.ToDecimal(Fpvoucher.Amount);
                                    //        objDistributorModel.CouponAmt = Convert.ToDecimal(Coupon.Amount);
                                    //    }
                                    //}
                                }
                            }



                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return objDistributorModel;
        }
        public List<BankModel> GetBankList()
        {
            List<BankModel> objListBanks = new List<BankModel>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objListBanks = (from result in entity.M_BankMaster
                                    where result.ActiveStatus == "Y"
                                    select new BankModel
                                    {
                                        BankCode = (int)result.BankCode,
                                        BankName = result.BankName,
                                        ActiveStatus = result.ActiveStatus,
                                        AccNo = result.AcNo,
                                        Remarks = result.Remarks,
                                        IFSCCode = result.IFSCode,

                                    }).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return objListBanks;
        }

        public List<PartyModel> GetAllParty(string LoginPartyCode, decimal LoginStateCode)
        {
            List<PartyModel> objpartyList = new List<PartyModel>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    bool IsAdmin = false;
                    bool IsSoldByHo = false;
                    decimal LoginGroupId = 0;
                    if (!string.IsNullOrEmpty(LoginPartyCode))
                    {
                        var result = (from r in entity.Inv_M_UserMaster where r.BranchCode == LoginPartyCode select r).FirstOrDefault();
                        if (result != null)
                        {
                            LoginGroupId = result.GroupId;
                            if (LoginGroupId == 0)
                            {
                                IsSoldByHo = true;
                            }
                            else
                            {
                                IsSoldByHo = false;
                            }
                            IsAdmin = result.IsAdmin == "Y" ? true : false;
                        }
                    }
                    if (IsSoldByHo)
                    {

                        objpartyList = (from party in entity.M_LedgerMaster
                                        where party.ActiveStatus == "Y" && party.PartyCode != LoginPartyCode && party.ISApprove == "Y"
                                        select new PartyModel
                                        {
                                            PartyCode = party.PartyCode,
                                            PartyName = party.PartyName,
                                            StateCode = party.StateCode,
                                            GroupId = party.GroupId,
                                        }
                                     ).ToList();
                    }
                    else
                    {

                        objpartyList = (from party in entity.M_LedgerMaster
                                        where party.ActiveStatus == "Y" && party.PartyCode != LoginPartyCode && ((party.StateCode == LoginStateCode && party.GroupId > LoginGroupId) || party.ParentPartyCode == LoginPartyCode)
                                        && party.ISApprove == "Y"
                                        select new PartyModel
                                        {
                                            PartyCode = party.PartyCode,
                                            PartyName = party.PartyName,
                                            StateCode = party.StateCode,
                                            GroupId = party.GroupId,
                                        }
                                     ).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return objpartyList;
        }
        public List<PartyModel> GetAllPartyNew(string LoginPartyCode, decimal LoginStateCode, bool NeedWallet)
        {
            List<PartyModel> objpartyList = new List<PartyModel>();
            try
            {

                using (var entity = new InventoryEntities())
                {
                    bool IsAdmin = false;
                    bool IsSoldByHo = false;
                    decimal LoginGroupId = 0;
                    var SameLevelBilling = "";
                    if (!string.IsNullOrEmpty(LoginPartyCode))
                    {
                        var result = (from r in entity.Inv_M_UserMaster
                                      join s in entity.M_LedgerMaster on r.BranchCode equals s.PartyCode
                                      where s.PartyCode == LoginPartyCode
                                      select new
                                      {
                                          IsAdmin = r.IsAdmin,
                                          GroupID = s.GroupId,
                                          SameLevelBilling = s.RecvdCForm
                                      }).FirstOrDefault();
                        if (result != null)
                        {
                            LoginGroupId = result.GroupID;
                            if (LoginGroupId == 0)
                            {
                                IsSoldByHo = true;
                            }
                            else
                            {
                                IsSoldByHo = false;
                            }
                            IsAdmin = result.IsAdmin == "Y" ? true : false;

                            SameLevelBilling = result.SameLevelBilling;
                        }
                    }

                    if (NeedWallet)
                        //objpartyList = (from party in entity.M_LedgerMaster
                        //                join w in entity.V_PartyBalance on party.PartyCode equals w.PartyCode
                        //                where party.ActiveStatus == "Y" && party.PartyCode != LoginPartyCode
                        //                orderby party.PartyName
                        //                select new PartyModel
                        //                {
                        //                    PartyCode = party.PartyCode,
                        //                    ParentPartyCode = party.ParentPartyCode,
                        //                    PartyName = party.PartyName,
                        //                    StateCode = party.StateCode,
                        //                    GroupId = party.GroupId,
                        //                    UserPartyCode = party.UserPartyCode,
                        //                    Address1 = party.Address1,
                        //                    CreditLimit = w.Balance ?? 0,
                        //                    GSTIN = party.TinNo,
                        //                    //  GoldenBoanza =GetPartyGoldenWalletBalance ( Convert.ToString (party.PartyCode))

                        //                }
                        //                ).ToList();

                        objpartyList = (
       from party in entity.M_LedgerMaster
       join w in entity.V_PartyBalance on party.PartyCode equals w.PartyCode
       where party.ActiveStatus == "Y" && party.PartyCode != LoginPartyCode
       orderby party.PartyName
       select new PartyModel
       {
           PartyCode = party.PartyCode,
           ParentPartyCode = party.ParentPartyCode,
           PartyName = party.PartyName,
           StateCode = party.StateCode,
           GroupId = party.GroupId,
           UserPartyCode = party.UserPartyCode,
           Address1 = party.Address1,
           CreditLimit = w.Balance ?? 0,
           GSTIN = party.TinNo,

           // Calculate PromoBalance (Cr - Dr)
           PromoBalance = (
               (
                   (from t in entity.TrnVouchers
                    where t.VType == "X" && t.CrTo == party.PartyCode
                    select (decimal?)t.Amount
                   ).Sum() ?? 0)
               -
               (
                   (from t in entity.TrnVouchers
                    where t.VType == "X" && t.DrTo == party.PartyCode
                    select (decimal?)t.Amount
                   ).Sum() ?? 0)
           )
       }
   ).ToList();
                    else
                        objpartyList = (from party in entity.M_LedgerMaster
                                        where party.ActiveStatus == "Y" && party.PartyCode != LoginPartyCode
                                        orderby party.PartyName
                                        select new PartyModel
                                        {
                                            PartyCode = party.PartyCode,
                                            ParentPartyCode = party.ParentPartyCode,
                                            PartyName = party.PartyName,
                                            StateCode = party.StateCode,
                                            GroupId = party.GroupId,
                                            UserPartyCode = party.UserPartyCode,
                                            Address1 = party.Address1,
                                            CreditLimit = 0,
                                            GSTIN = party.TinNo,
                                            //GoldenBoanza = GetPartyGoldenWalletBalance(Convert.ToString(party.PartyCode))

                                        }).ToList();


                    if (IsSoldByHo == false)
                    {
                        objpartyList = objpartyList.Where(m => m.PartyCode != LoginPartyCode && ((NeedWallet && LoginGroupId == 1 && ((m.StateCode == LoginStateCode && m.GroupId > LoginGroupId) || (SameLevelBilling == "Y" && m.GroupId >= LoginGroupId) || m.ParentPartyCode == LoginPartyCode || m.GroupId == 0)) || (!(NeedWallet && LoginGroupId == 1) && ((m.StateCode == LoginStateCode && m.GroupId > LoginGroupId) || (SameLevelBilling == "Y" && m.GroupId >= LoginGroupId) || m.ParentPartyCode == LoginPartyCode)))).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return objpartyList;
        }

        //public ConfigDetails GetConfigDetails()
        //{
        //    ConfigDetails objConfigDetails = new ConfigDetails();
        //    try
        //    {
        //        using(var entity=new InventoryEntities())
        //        {
        //            objConfigDetails = (from result in entity.Inv_M_ConfigMaster
        //                                select new ConfigDetails
        //                                {
        //                                    C_PrintBill = result.C_PrintBill,
        //                                    C_IsBillOnMRP = result.C_IsBillOnMRP,
        //                                    C_AddDuplicateProd = result.C_AddDuplicateProd,
        //                                    C_AllowDiscount = result.C_AllowDiscount,
        //                                    C_DiscForAllCust = result.C_DiscForAllCust

        //                                }).FirstOrDefault();
        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //    return objConfigDetails;
        //}

        //public decimal GetWalletBalance(string CustCode)
        //{
        //    decimal WalletBalance = 0;

        //    try
        //    {
        //        using (var entity = new InventoryEntities())
        //        {
        //            WalletBalance = (from result in entity.CouponSalesDetails
        //                             where result.ActiveStatus == "Y" && result.CustCode == CustCode

        //                             select result.Amount
        //                           ).DefaultIfEmpty(0).Sum();
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    return WalletBalance;
        //}

        public List<GroupModel> GetGroupList()
        {
            List<GroupModel> objGroupList = new List<GroupModel>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objGroupList = (from r in entity.M_GroupMaster
                                    where r.ActiveStatus == "Y" && r.InvLogin == "Y"
                                    join p in entity.M_LedgerMaster on r.GroupId equals p.GroupId
                                    where p.ISApprove == "Y"
                                    select new GroupModel
                                    {
                                        GroupName = r.GroupName,
                                        GroupId = r.GroupId
                                    }

                                  ).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return objGroupList;
        }

        public List<PartyModel> GetPartyList()
        {
            List<PartyModel> objPartyList = new List<PartyModel>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objPartyList = (from r in entity.M_LedgerMaster
                                    where r.ActiveStatus == "Y"
                                    select new PartyModel
                                    {
                                        PartyName = r.PartyName,
                                        PartyCode = r.PartyCode,
                                        GroupId = r.GroupId
                                    }
                                  ).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return objPartyList;
        }

        public ResponseDetail SaveStockJv(StockJv objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            StockJv TempStockJv = new StockJv();

            decimal maxJNo = 0;
            decimal? FsessId = 0;
            string JvNo = "";
            string version = "";
            objResponse.ResponseMessage = "Something went wrong!";
            objResponse.ResponseStatus = "FAILED";

            try
            {
                using (var entity = new InventoryEntities())
                {
                    FsessId = (from result in entity.M_FiscalMaster where result.ActiveStatus == "Y" select result.FSessId).DefaultIfEmpty(0).Max();
                    maxJNo = (from r in entity.TrnStockJvs select r.JNo).DefaultIfEmpty(0).Max();
                    if (maxJNo == 0)
                    {
                        maxJNo = 1000;
                    }
                    maxJNo = maxJNo + 1;
                    if (objModel.isAdd)
                    {
                        JvNo = "Add/" + maxJNo;
                    }
                    else
                    {
                        JvNo = "Less/" + maxJNo;
                    }
                    DateTime CurrentJvDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(objModel.JvDate))
                    {
                        //var SplitDate = objModel.JvDate.Split('-');
                        //string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                        var CurrentJvDate1 = Convert.ToDateTime(objModel.JvDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        CurrentJvDate = DateTime.Parse(CurrentJvDate1, new CultureInfo("en-US", true));
                    }
                    version = (from result in entity.M_NewHOVersionInfo select result.VersionNo).FirstOrDefault();

                    foreach (var obj in objModel.objListProduct)
                    {
                        TrnStockJv objDTStockData = new TrnStockJv();
                        objDTStockData.JNo = maxJNo;
                        objDTStockData.JvNo = JvNo;
                        objDTStockData.Version = version;
                        objDTStockData.UserId = objModel.LoginUser.UserId;
                        objDTStockData.UserName = objModel.LoginUser.UserName;
                        objDTStockData.StockDate = CurrentJvDate;
                        objDTStockData.SoldBy = objModel.LoginUser.PartyCode;
                        objDTStockData.Remarks = string.IsNullOrEmpty(objModel.Remarks) ? "" : objModel.Remarks;
                        objDTStockData.RefNo = string.IsNullOrEmpty(objModel.RefNo) ? "" : objModel.RefNo;
                        objDTStockData.RecTimeStamp = DateTime.Now;
                        objDTStockData.Qty = obj.Quantity;
                        objDTStockData.ProductName = obj.ProductName;
                        objDTStockData.ProdType = "P";
                        objDTStockData.ProdId = obj.ProdCode.ToString();
                        objDTStockData.PartyName = objModel.PartyName;
                        objDTStockData.FCode = objModel.FCode;
                        if (objModel.isAdd)
                            objDTStockData.JType = "A";
                        else
                            objDTStockData.JType = "L";
                        objDTStockData.BatchNo = obj.BatchNo;
                        objDTStockData.Barcode = obj.Barcode;
                        objDTStockData.ActiveStatus = "Y";
                        objDTStockData.FSessId = FsessId ?? 0;
                        entity.TrnStockJvs.Add(objDTStockData);
                    }
                    int i = 0;


                    try
                    {
                        i = entity.SaveChanges();
                        if (i == objModel.objListProduct.Count)
                        {

                            foreach (var obj in objModel.objListProduct)
                            {
                                Im_CurrentStock objCurrentStock = new Im_CurrentStock();
                                objCurrentStock.FSessId = FsessId ?? 0;
                                objCurrentStock.SupplierCode = "0";
                                objCurrentStock.StockDate = CurrentJvDate;
                                objCurrentStock.RefNo = JvNo;
                                objCurrentStock.FCode = objModel.FCode;
                                objCurrentStock.GroupId = objModel.GroupId;
                                objCurrentStock.ProdId = obj.ProdCode.ToString();
                                objCurrentStock.BatchCode = obj.BatchNo;
                                objCurrentStock.Barcode = obj.Barcode;

                                if (objModel.isAdd)
                                {
                                    objCurrentStock.SType = "I";
                                    objCurrentStock.Qty = obj.Quantity;
                                    objCurrentStock.BType = "A";
                                    objCurrentStock.Remarks = "Stock Added";
                                    objCurrentStock.BillType = "A";
                                }
                                else
                                {
                                    objCurrentStock.SType = "O";
                                    objCurrentStock.Qty = -(obj.Quantity);
                                    objCurrentStock.BType = "L";
                                    objCurrentStock.Remarks = "Stock Lessed";
                                    objCurrentStock.BillType = "L";
                                }
                                objCurrentStock.ActiveStatus = "Y";
                                objCurrentStock.EntryBy = objModel.LoginUser.PartyCode;
                                objCurrentStock.StockFor = objModel.FCode;
                                objCurrentStock.RecTimeStamp = DateTime.Now;
                                objCurrentStock.UserId = objModel.LoginUser.UserId;
                                objCurrentStock.Version = version;
                                objCurrentStock.IsDisp = "N";
                                objCurrentStock.InvoiceNo = "";
                                objCurrentStock.ProdType = "P";
                                objCurrentStock.DispQty = 0;

                                entity.Im_CurrentStock.Add(objCurrentStock);
                            }
                            i = 0;
                            try
                            {
                                i = entity.SaveChanges();
                                if (i == objModel.objListProduct.Count)
                                {
                                    objResponse.ResponseMessage = "Saved Successfully!";
                                    objResponse.ResponseStatus = "OK";
                                }
                                else
                                {
                                    objResponse.ResponseMessage = "Something went wrong!";
                                    objResponse.ResponseStatus = "FAILED";
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
            catch (Exception ex)
            {

            }

            return objResponse;
        }

        public ResponseDetail SavePurchaseInvoice(DistributorBillModel objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            M_InwardData objInwardData = new M_InwardData();

            DistributorBillModel TempDistributor = new DistributorBillModel();

            decimal maxUserSBillNo = 0;
            decimal? SessId = 0;
            string billPrefix = "";
            decimal maxGrNo = 0;
            decimal? FsessId = 0;
            string InwardNo = "";
            string version = "";


            objResponse.ResponseMessage = "Something went wrong!";
            objResponse.ResponseStatus = "FAILED";

            try
            {
                string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(AppConnectionString);

                string query = "Select Max(SessID) as MaxSessId from M_SessnMaster";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        SessId = decimal.Parse(reader["MaxSessId"].ToString());
                    }
                }
                using (var entity = new InventoryEntities())
                {
                    maxGrNo = (from result in entity.M_InwardMain select result.GrNo).DefaultIfEmpty(0).Max();
                    maxGrNo = maxGrNo + 1;
                    FsessId = (from result in entity.M_FiscalMaster where result.ActiveStatus == "Y" select result.FSessId).DefaultIfEmpty(0).Max();
                    ////decimal? SessId = (from result in entity.M_SessnMaster select result.SessID).Max();

                    billPrefix = (from result in entity.M_ConfigMaster select result.BillPrefix).FirstOrDefault();
                    //maxUserSBillNo = (from result in entity.TrnBillMains where result.FSessId == FsessId && result.SoldBy == objModel.objCustomer.UserDetails.PartyCode && result.BillType != "S" select result.UserSBillNo).Max();
                    //maxUserSBillNo = maxUserSBillNo + 1;
                    InwardNo = "PI/" + "WR" + "/" + maxGrNo;
                    version = (from result in entity.M_NewHOVersionInfo select result.VersionNo).FirstOrDefault();


                    if (objModel != null)
                    {
                        DateTime BillDate = DateTime.Now;
                        if (!string.IsNullOrEmpty(objModel.BillDateStr))
                        {
                            var SplitDate = objModel.BillDateStr.Split('-');
                            string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                            BillDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                            BillDate = BillDate.Date;
                            string SoldByCode = "";
                            string GroupPrefix = "";
                            string BillingPartyCode = objModel.objCustomer.PartyCode;
                            //GroupPrefix = (from p in entity.M_GroupMaster
                            //               where p.GroupId == (from r in entity.M_LedgerMaster where r.PartyCode == BillingPartyCode select r.GroupId).FirstOrDefault()
                            //               select p.Prefix
                            //               ).FirstOrDefault();
                            GroupPrefix = "PI";
                            List<ProductModel> objListProductModel = new List<ProductModel>();
                            foreach (var obj in objModel.objListProduct)
                            {
                                objListProductModel.Add(obj);
                                M_InwardData objDTBillData = new M_InwardData();

                                objDTBillData.FSessId = FsessId ?? 0;
                                objDTBillData.InwardBy = objModel.objCustomer.UserDetails.PartyCode;
                                objDTBillData.InwardByName = objModel.objCustomer.UserDetails.PartyName;
                                objDTBillData.GrNo = maxGrNo;
                                objDTBillData.InwardNo = InwardNo;
                                objDTBillData.SupplierCode = objModel.objCustomer.PartyCode;
                                objDTBillData.SupplierName = objModel.objCustomer.PartyName;
                                objDTBillData.OrderDate = DateTime.Now.Date;
                                objDTBillData.DeliveryDate = DateTime.Now.Date;
                                objDTBillData.RecvDate = BillDate.Date;
                                objDTBillData.RefNo = string.IsNullOrEmpty(objModel.objCustomer.ReferenceIdNo) ? "" : objModel.objCustomer.ReferenceIdNo;
                                objDTBillData.ProdCode = obj.ProdCode.ToString();
                                objDTBillData.ProdName = obj.ProductName;
                                objDTBillData.BatchNo = obj.BatchNo.ToString();
                                objDTBillData.Barcode = obj.Barcode.ToString();
                                objDTBillData.Qty = obj.Quantity;
                                objDTBillData.FreeQty = 0;
                                objDTBillData.MRP = obj.MRP ?? 0;
                                objDTBillData.PRate = obj.Rate ?? 0;
                                objDTBillData.DP = obj.DP ?? 0;
                                //to be asked
                                objDTBillData.TradeDiscount = obj.DiscPer ?? 0;
                                objDTBillData.TradeAmount = obj.DiscAmt ?? 0;

                                objDTBillData.LotDiscount = 0;
                                objDTBillData.TotalLotDiscount = 0;
                                //tax excluding
                                objDTBillData.Amount = obj.Amount;
                                objDTBillData.AValue = 0;
                                objDTBillData.AValueAmt = 0;
                                objDTBillData.DiscountAmt = obj.DiscAmt ?? 0;
                                objDTBillData.TotalAmt = obj.TotalAmount;
                                objDTBillData.PStatus = "Y";
                                objDTBillData.TotalAmount = objModel.objProduct.TotalTotalAmount;
                                objDTBillData.TotalTradeDiscount = objModel.objProduct.TotalDiscount;

                                objDTBillData.CashDiscPer = obj.CashDiscPer;
                                objDTBillData.TotalCashDiscount = obj.CashDiscAmount;
                                objDTBillData.TotalQty = objModel.objProduct.TotalQty;
                                objDTBillData.TotalFreeQty = 0;
                                objDTBillData.TotalDiscount = objModel.objProduct.TotalDiscount + objModel.objProduct.CashDiscAmount;
                                objDTBillData.TotalTaxAmt = objModel.objProduct.TotalTaxAmount;
                                objDTBillData.TotalEAmt = 0;
                                objDTBillData.RndOff = objModel.objProduct.Roundoff;
                                objDTBillData.NetPayable = objModel.objProduct.TotalNetPayable;
                                objDTBillData.Remarks = string.IsNullOrEmpty(objModel.objCustomer.Remarks) ? "" : objModel.objCustomer.Remarks;
                                objDTBillData.ActiveStatus = "Y";
                                objDTBillData.InwardFor = objModel.objCustomer.UserDetails.PartyCode;
                                objDTBillData.InwardForName = objModel.objCustomer.UserDetails.PartyName;
                                objDTBillData.Status = "P";
                                objDTBillData.RecTimeStamp = DateTime.Now;
                                objDTBillData.UserId = objModel.objCustomer.UserDetails.UserId;
                                objDTBillData.UserName = objModel.objCustomer.UserDetails.UserName;
                                objDTBillData.Version = version;
                                objDTBillData.DUserId = 0;
                                objDTBillData.DReason = "";
                                objDTBillData.DRecTimeStamp = DateTime.Now;
                                objDTBillData.OrdQty = 0;
                                objDTBillData.ShortQty = 0;
                                objDTBillData.DemQty = 0;
                                objDTBillData.ExpQty = 0;
                                objDTBillData.RemQty = 0;
                                objDTBillData.TotalOrdQty = 0;
                                objDTBillData.TotalShortQty = 0;
                                objDTBillData.TotalDemQty = 0;
                                objDTBillData.TotalExpQty = 0;
                                objDTBillData.TotalRemQty = 0;
                                objDTBillData.OrderNo = "";
                                objDTBillData.OrderBy = "";
                                objDTBillData.CourierId = 0;
                                objDTBillData.CourierName = "";
                                objDTBillData.TransId = "";
                                objDTBillData.TransName = "";
                                objDTBillData.LRNo = "";
                                objDTBillData.LRDate = DateTime.Now;
                                objDTBillData.UID = string.IsNullOrEmpty(objModel.objProduct.UID) ? "" : objModel.objProduct.UID;
                                objDTBillData.FreightAmt = 0;
                                objDTBillData.OtherCharges = 0;




                                objDTBillData.ShortAmt = 0;
                                objDTBillData.DemAmt = 0;
                                objDTBillData.ExpAmt = 0;
                                objDTBillData.TtlDedcAmt = 0;
                                objDTBillData.TotalShortAmt = 0;
                                objDTBillData.TotalDemAmt = 0;
                                objDTBillData.TotalExpAmt = 0;
                                objDTBillData.TotalDedcAmt = 0;
                                objDTBillData.GenDN = "N";
                                objDTBillData.GenDNBy = "";
                                objDTBillData.DNNo = "";
                                objDTBillData.DNDate = null;
                                //to be asked
                                objDTBillData.BType = "U";
                                objDTBillData.MfgDate = null;
                                objDTBillData.ExpDate = null;

                                if (objModel.objCustomer.StateCode == objModel.objCustomer.UserDetails.StateCode)
                                {
                                    objDTBillData.TaxAmt = 0;
                                    objDTBillData.Tax = 0;
                                    objDTBillData.CGST = obj.TaxPer / 2 ?? 0;
                                    objDTBillData.CGSTAmt = obj.TaxAmt / 2 ?? 0;
                                    objDTBillData.SGST = obj.TaxPer / 2 ?? 0;
                                    objDTBillData.SGSTAmt = obj.TaxAmt / 2 ?? 0;
                                    objDTBillData.TaxType = "S";
                                    objDTBillData.TaxBase = "S";
                                }
                                else
                                {

                                    objDTBillData.TaxAmt = obj.TaxAmt ?? 0;
                                    if (obj.OldTaxAmount != 0 && obj.OldTaxAmount != obj.TaxAmt)
                                    {
                                        objDTBillData.TaxAmt = Decimal.Parse((Convert.ToDouble(objDTBillData.TaxAmt) + 0.01).ToString());
                                        objDTBillData.Amount = Decimal.Parse((Convert.ToDouble(objDTBillData.Amount) - 0.01).ToString());
                                    }
                                    objDTBillData.Tax = obj.TaxPer ?? 0;
                                    objDTBillData.CGST = 0;
                                    objDTBillData.CGSTAmt = 0;
                                    objDTBillData.SGST = 0;
                                    objDTBillData.SGSTAmt = 0;
                                    objDTBillData.TaxType = "I";
                                    objDTBillData.TaxBase = "I";
                                }





                                entity.M_InwardData.Add(objDTBillData);
                            }
                            int i = 0;
                            try
                            {
                                i = entity.SaveChanges();
                                if (i > 0)
                                {
                                    objResponse.ResponseMessage = "Saved Succesfully!";
                                    objResponse.ResponseStatus = "OK";
                                }
                                else
                                {
                                    objResponse.ResponseMessage = "Something went wrong!";
                                    objResponse.ResponseStatus = "FAILED";
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return objResponse;
        }

        public List<PartyModel> GetAllSupplierList(string LoginPartyCode, decimal LoginStateCode)
        {
            List<PartyModel> objPartyList = new List<PartyModel>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    bool IsAdmin = false;
                    decimal LoginGroupId = 0;
                    if (!string.IsNullOrEmpty(LoginPartyCode))
                    {
                        var result = (from r in entity.Inv_M_UserMaster where r.BranchCode == LoginPartyCode select r).FirstOrDefault();
                        if (result != null)
                        {
                            LoginGroupId = result.GroupId;
                            IsAdmin = result.IsAdmin == "Y" ? true : false;
                        }
                    }
                    if (IsAdmin)
                    {
                        objPartyList = (from r in entity.M_LedgerMaster
                                        where r.ActiveStatus == "Y" && r.GroupId == 5 && r.PartyCode != LoginPartyCode

                                        select new PartyModel
                                        {
                                            GroupId = r.GroupId,
                                            StateCode = r.StateCode,
                                            PartyCode = r.PartyCode,
                                            PartyName = r.PartyName,

                                        }
                                              ).ToList();

                    }
                    else
                    {

                        objPartyList = (from party in entity.M_LedgerMaster
                                        where party.ActiveStatus == "Y" && party.PartyCode != LoginPartyCode && ((party.StateCode == LoginStateCode && party.GroupId == 5) || party.ParentPartyCode == LoginPartyCode)
                                        select new PartyModel
                                        {
                                            PartyCode = party.PartyCode,
                                            PartyName = party.PartyName,
                                            StateCode = party.StateCode,
                                            GroupId = party.GroupId,
                                        }
                                     ).ToList();
                    }

                }


            }
            catch (Exception ex)
            {

            }
            return objPartyList;
        }


        //public ReferenceModel CheckReferenceId(string CustCode)
        //{

        //    ReferenceModel objReference = new ReferenceModel();
        //    try
        //    {
        //        using(var entity=new InventoryEntities())
        //        {
        //            if (!string.IsNullOrEmpty(CustCode)) {
        //                M_CustomerMaster objDTCustomer = (from r in entity.M_CustomerMaster where r.CustCode == CustCode select r).FirstOrDefault();
        //                if (objDTCustomer != null)
        //                {
        //                    objReference.objresponse = new ResponseDetail();
        //                    objReference.objresponse.ResponseMessage = "Record Found!";
        //                    objReference.objresponse.ResponseStatus = "OK";
        //                    objReference.RefId = objDTCustomer.CustCode;
        //                    objReference.RefName = objDTCustomer.CustName;
        //                }
        //                else
        //                {
        //                    objReference.objresponse.ResponseMessage = "Record Not Found!";
        //                    objReference.objresponse.ResponseStatus = "FAILED";
        //                    objReference.RefId = "";
        //                    objReference.RefName = "";
        //                }
        //            }
        //            else
        //            {
        //                objReference.objresponse = new ResponseDetail();
        //                objReference.objresponse.ResponseMessage = "Something went wrong!";
        //                objReference.objresponse.ResponseStatus = "FAILED";
        //                objReference.RefId = "";
        //                objReference.RefName = "";
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }

        //    return objReference;
        //}

        //public async Task<ResponseDetail> IssueCard(string CardNo,string IdNo, string ContactNo,string CustomerType)
        //{
        //    ResponseDetail objResponse = new ResponseDetail();
        //    string TempCustCode ="";

        //    try
        //    {
        //        using (var entity = new InventoryEntities())
        //        {
        //            if ((!string.IsNullOrEmpty(CardNo)) && (!string.IsNullOrEmpty(IdNo)))
        //            {
        //                if (CheckCard(CardNo))
        //                {
        //                    if (CustomerType=="New" || (CustomerType == "Existing" && CheckCustomer(IdNo)))
        //                    {
        //                        CustomerDetail objcustomer = GetCustInfo(IdNo, false);
        //                        if (CustomerType == "New" && (!string.IsNullOrEmpty(ContactNo)))
        //                        {
        //                            TempCustCode = ContactNo;
        //                        }
        //                        else
        //                        {
        //                            TempCustCode = IdNo;
        //                        }
        //                        //issue card
        //                        SqlConnection SC = new SqlConnection("Data Source=64.62.143.84;Initial Catalog=ResetAgro;Integrated Security=False;User Id=agritech;Password=tech$reset#32;");
        //                        string tableName = "TempCardGeneration";
        //                        //     string cmdText = @"IF EXISTS(SELECT * FROM ResetAgro.INFORMATION_SCHEMA.TABLES 
        //                        //WHERE TABLE_SCHEMA ='dbo' AND TABLE_NAME='" + tableName + "') SELECT 1 ELSE SELECT 0";
        //                        //     SC.Close();
        //                        //     SC.Open();
        //                        //     SqlCommand TableCheck = new SqlCommand(cmdText, SC);
        //                        //     int x = Convert.ToInt32(TableCheck.ExecuteScalar());
        //                        //     if (x != 1)
        //                        //     {

        //                        //     }
        //                        string cmdText = "Insert into TempCardGeneration Select *,Getdate() FROM M_CardGeneration WHERE UsedBy='" + IdNo + "' AND ActiveStatus='Y' ;";
        //                        SqlCommand cmd = new SqlCommand(cmdText, SC);
        //                        SC.Close();
        //                        SC.Open();
        //                        cmd.ExecuteNonQuery();
        //                        SC.Close();

        //                        M_CardGeneration ObjDTCardGeneration = (from r in entity.M_CardGeneration
        //                                                                where r.ActiveStatus == "Y" && r.UsedBy == IdNo
        //                                                                select r
        //                                            ).FirstOrDefault();
        //                        ObjDTCardGeneration.ActiveStatus = "N";
        //                        int i = entity.SaveChanges();
        //                        if (i > 0)
        //                        {
        //                            M_CardGeneration objDT1CardGeneration = (from r in entity.M_CardGeneration
        //                                                                     where r.ActiveStatus == "Y" && r.CardNO == CardNo && (r.UsedBy == null || r.UsedBy == "")
        //                                                                     select r
        //                                                                   ).FirstOrDefault();
        //                            objDT1CardGeneration.UsedBy = IdNo;
        //                            objDT1CardGeneration.UsedDate = DateTime.Now;
        //                            i = 0;
        //                            i = entity.SaveChanges();
        //                            if (i > 0)
        //                            {
        //                                var result = (from r in entity.M_CompanyMaster where r.ActiveStatus == "Y" select r).FirstOrDefault();
        //                                if (result != null)
        //                                {
        //                                    string IssueSms = "Dear Customer,Card has been successfully issued to " + IdNo + " .Regards " + result.CompName;
        //                                    var IsSendIssue = (from r in entity.M_ConfigMaster select r.SendSMSOnIssue).FirstOrDefault();
        //                                    if (IsSendIssue == "Y")
        //                                    {
        //                                        bool IsSuccess = await Task.Run(() => Program.SendSMS(result.smsUserNm, result.smPass, result.smsSenderId, TempCustCode, IssueSms));
        //                                        if (IsSuccess)
        //                                        {
        //                                            objResponse.ResponseStatus = "OK";
        //                                            objResponse.ResponseMessage = "Congratulations, Card Issued Successfully!";
        //                                        }
        //                                        else
        //                                        {
        //                                            objResponse.ResponseStatus = "FAILED";
        //                                            objResponse.ResponseMessage = "Sorry, Something went wrong!";
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        objResponse.ResponseStatus = "FAILED";
        //                        objResponse.ResponseMessage = "Invalid Customer ID!";
        //                    }
        //                }
        //                else
        //                {
        //                    objResponse.ResponseStatus = "FAILED";
        //                    objResponse.ResponseMessage = "Invalid Card No.!";
        //                }
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }

        //    return objResponse;
        //}



        //public bool CheckCard(string CardNo)
        //{
        //    try
        //    {
        //        using(var entity=new InventoryEntities())
        //        {
        //            if (!string.IsNullOrEmpty(CardNo))
        //            {
        //                var result = (from r in entity.M_CardGeneration
        //                              where r.ActiveStatus == "Y" && r.CardNO == CardNo
        //                              select r
        //                            ).FirstOrDefault();
        //                if (result != null)
        //                {
        //                    return true;
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //    return false;
        //}

        //public bool CheckCustomer(string IdNo)
        //{
        //    try
        //    {
        //        using (var entity = new InventoryEntities())
        //        {
        //            if (!string.IsNullOrEmpty(IdNo))
        //            {
        //                var result = (from r in entity.M_CustomerMaster
        //                              where r.ActiveStatus == "Y" && r.CustCode == IdNo
        //                              select r
        //                            ).FirstOrDefault();
        //                if (result != null)
        //                {
        //                    return true;
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return false;
        //}

        public async Task<ResponseDetail> SendInvoiceSMS(string MobileNo, string TotalBillAmount, string RCP, string BV)
        {
            ResponseDetail objResponse = new ResponseDetail();

            string message = "Congrats! Your order is billed with " + TotalBillAmount + " RCP and " + BV + " BV, Thanx for shopping with PROGLEN. info: www.proglen.co.in PROGLEN";
            bool IsSuccess = await Task.Run(() => Program.SendInvoiceSMS(message, MobileNo, "1507162012817633678"));

            if (IsSuccess)
            {
                objResponse.ResponseStatus = "OK";
                objResponse.ResponseMessage = "SMS has send Successfully!";
            }
            else
            {
                objResponse.ResponseStatus = "FAILED";
                objResponse.ResponseMessage = "Sorry, Something went wrong!";
                objResponse.GeneratedOTP = "";
            }
            return objResponse;
        }

        public async Task<ResponseDetail> SendOTP(string MobileNo, string TotalBillAmount, string idNo)
        {
            ResponseDetail objResponse = new ResponseDetail();

            string OTPGenerated = "";
            System.Text.StringBuilder builder = new System.Text.StringBuilder(5);
            string numberAsString = "";
            //int numberAsNumber = 0;
            for (var i = 0; i < 5; i++)
            {
                builder.Append(_numbers[random.Next(0, _numbers.Length)]);
            }
            OTPGenerated = builder.ToString();

            string message = "Your OTP is " + OTPGenerated + " for Invoice with Amount " + TotalBillAmount + " PROGLEN";
            bool IsSuccess = await Task.Run(() => Program.SendSMSOTP(message, MobileNo));

            if (IsSuccess)
            {
                objResponse.ResponseStatus = "OK";
                objResponse.ResponseMessage = "OTP has send Successfully!";
                objResponse.GeneratedOTP = OTPGenerated;
            }
            else
            {
                objResponse.ResponseStatus = "FAILED";
                objResponse.ResponseMessage = "Sorry, Something went wrong!";
                objResponse.GeneratedOTP = "";
            }
            return objResponse;
        }

        public List<PurchaseReport> GetPurchaseInvoice(string InvoiceNo)
        {
            List<PurchaseReport> objListPurchaseInvoice = new List<PurchaseReport>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objListPurchaseInvoice = (from r in entity.M_InwardMain
                                              where r.InwardNo == InvoiceNo
                                              join p in entity.M_InwardData
                                              on r.InwardNo equals p.InwardNo
                                              select new PurchaseReport
                                              {
                                                  RefNo = r.RefNo,
                                                  SupplierCode = r.SupplierCode,
                                                  SupplierName = r.SupplierName,
                                                  Remarks = r.Remarks,
                                                  InvoiceNo = r.InwardNo,
                                                  InvoiceDateStr = r.RecvDate,
                                                  InvoiceFor = r.InwardForName + "-" + r.InwardFor,
                                                  objproduct = new ProductModel
                                                  {
                                                      ProductCodeStr = p.ProdCode,
                                                      ProductName = p.ProdName,
                                                      Barcode = p.Barcode,
                                                      BatchNo = p.BatchNo,
                                                      Quantity = p.Qty,
                                                      Rate = p.PRate,
                                                      Amount = p.Amount,
                                                      DiscAmt = p.TradeAmount,
                                                      DiscPer = p.TradeDiscount,

                                                      CGST = p.CGST,
                                                      CGSTAmount = p.CGSTAmt,
                                                      SGST = p.SGST,
                                                      SGSTAmount = p.SGSTAmt,
                                                      TaxAmt = p.TaxAmt,
                                                      TaxPer = p.Tax,
                                                      TotalAmount = p.TotalAmt,
                                                      Roundoff = p.RndOff,
                                                      TaxType = p.TaxType
                                                  }
                                                  ,
                                                  TotalTradeDisc = r.TotalTradeDiscount.ToString(),
                                                  TotalTaxAmt = r.TotalTaxAmt.ToString(),
                                                  CashDiscount = r.TotalCashDiscount.ToString(),
                                                  NetPayable = r.NetPayable.ToString(),
                                                  RndOff = r.RndOff.ToString(),
                                                  TotalAmount = r.TotalAmt.ToString()
                                              }
                                            ).ToList();

                    DateTime CurrentDate = new DateTime(2017, 07, 01);
                    bool isNewBill = false;
                    if (objListPurchaseInvoice.Count > 0)
                    {
                        if (objListPurchaseInvoice[0].InvoiceDateStr.Date >= CurrentDate.Date)
                        {
                            isNewBill = true;

                        }
                        else
                        {
                            isNewBill = false;
                        }
                        objListPurchaseInvoice[0].IsNewBill = isNewBill;
                        objListPurchaseInvoice[0].NetAmount = (decimal.Parse(objListPurchaseInvoice[0].TotalAmount) + decimal.Parse(objListPurchaseInvoice[0].TotalTaxAmt)).ToString();
                        objListPurchaseInvoice[0].NetPayable = (decimal.Parse(objListPurchaseInvoice[0].NetAmount) + decimal.Parse(objListPurchaseInvoice[0].RndOff)).ToString();
                        //var result = (from r in entity.M_CompanyMaster where r.ActiveStatus == "Y" select r).FirstOrDefault();
                        //if (result != null)
                        //{
                        string SoldBy = objListPurchaseInvoice[0].InvoiceFor;
                        var SoldByCode = SoldBy.Split('-');
                        var SoldByCodeStr = SoldByCode[1];
                        var resultDetails = (from r in entity.M_LedgerMaster where r.PartyCode == SoldByCodeStr select r).FirstOrDefault();
                        if (resultDetails != null)
                        {
                            objListPurchaseInvoice[0].GSTIN = resultDetails.TinNo;
                            objListPurchaseInvoice[0].SoldByName = resultDetails.PartyName;
                            objListPurchaseInvoice[0].SoldByAddress = resultDetails.Address1;
                            objListPurchaseInvoice[0].SoldByCity = resultDetails.CityName;
                        }

                        //objListPurchaseInvoice[0].CompanyName = result.CompName;
                        //objListPurchaseInvoice[0].CompanyAdd = result.CompAdd;
                        //}
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return objListPurchaseInvoice;
        }

        public decimal GetPartyWalletBalance(string LoginPartyCode, string vtype)
        {
            decimal WalletBalance = 0;
            try
            {
                if (!string.IsNullOrEmpty(LoginPartyCode))
                {
                    using (var entity = new InventoryEntities())
                    {
                        string Sql = "";
                        string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;

                        SqlConnection Sc = new SqlConnection(InvConnectionString);

                        SqlCommand cmd = new SqlCommand();
                        Sql = "Select CrAmt-DrAmt as AcBalance FROM (Select ISNULL(SUM(Amount),0) as CrAmt FROM TrnVoucher WHERE Vtype='" + vtype + "' AND Crto='" + LoginPartyCode + "') a," +
"(Select ISNULL(SUM(Amount),0) as DrAmt FROM TrnVoucher WHERE Vtype='" + vtype + "' AND Drto='" + LoginPartyCode + "') b";
                        cmd.CommandText = Sql;
                        cmd.Connection = Sc;
                        Sc.Close();
                        Sc.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                WalletBalance = reader["AcBalance"] != null ? decimal.Parse(reader["AcBalance"].ToString()) : 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return WalletBalance;
        }

        public decimal GetCourierCharge(decimal PackageWeight)
        {
            decimal CourierCharge = 0;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    var CourierChargedetail = (from r in entity.CourierCharges
                                               where r.Weight >= PackageWeight
                                               select r
                                    ).OrderByDescending(o => o.Weight).FirstOrDefault();
                    CourierCharge = CourierChargedetail != null ? CourierChargedetail.Amount : 0;
                }
            }
            catch (Exception ex)
            {

            }
            return CourierCharge;
        }

        public string GetOrderNo(string LoginPartyCode)
        {
            string OrderNo = "ORD/";
            try
            {
                using (var entity = new InventoryEntities())
                {
                    var maxOrderNo = (from r in entity.TrnPartyOrderMains
                                      where r.ActiveStatus == "Y" && r.OrderBy == LoginPartyCode
                                      select r.SOrderNo
                                    ).DefaultIfEmpty(10000).DefaultIfEmpty(0).Max();
                    maxOrderNo = maxOrderNo + 1;
                    OrderNo = OrderNo + LoginPartyCode + "/" + maxOrderNo;
                }
            }
            catch (Exception ex)
            {

            }
            return OrderNo;
        }
        public ResponseDetail SavePartyOrderDetails(PartyOrderModel objPartyOrderModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseMessage = "Something went wrong!";
            objResponse.ResponseStatus = "FAILED";
            try
            {
                decimal WalletBalance = 0;
                decimal? SessId = 0;
                decimal? FsessId = 0;
                string version = "";
                string billPrefix = "";
                DateTime BillDate = DateTime.Now.Date;
                //Cmnted on 01Sep18
                //if (!string.IsNullOrEmpty(objPartyOrderModel.OrderDateStr))
                //{
                //    var SplitDate = objPartyOrderModel.OrderDateStr.Split('-');
                //    string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                //    BillDate = Convert.ToDateTime(NewDate);
                //    BillDate = BillDate.Date;
                //}
                TrnPayModeDetail objDtPayModeDetail = new TrnPayModeDetail();
                List<string> Paymode = new List<string>();
                List<string> PayPrefix = new List<string>();
                List<TrnPayModeDetail> objDTListPayMode = new List<TrnPayModeDetail>();
                WalletBalance = GetPartyWalletBalance(objPartyOrderModel.OrderBy, "R");

                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(AppConnectionString);
                SqlConnection SC1 = new SqlConnection(InvConnectionString);

                string query = "Select Max(SessID) as MaxSessId from M_SessnMaster";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        SessId = decimal.Parse(reader["MaxSessId"].ToString());
                    }
                }
                using (var entity = new InventoryEntities())
                {
                    bool IsWalletEntry = false;
                    billPrefix = (from result in entity.M_ConfigMaster select result.BillPrefix).FirstOrDefault();
                    version = (from result in entity.M_NewHOVersionInfo select result.VersionNo).FirstOrDefault();
                    if (objPartyOrderModel != null)
                    {
                        if (objPartyOrderModel.objProduct.PayDetails != null)
                        {
                            if (objPartyOrderModel.objProduct.PayDetails.PayMode == "BD")
                            {
                                EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.BankDeposit;
                                string value = EnumPayModes.GetEnumDescription(enumVar);
                                PayPrefix.Add(value);
                                objPartyOrderModel.objProduct.PayDetails.PayMode = "Bank Deposit";
                                objPartyOrderModel.objProduct.PayDetails.CHBankName = objPartyOrderModel.objProduct.PayDetails.BDBankName;
                                objPartyOrderModel.objProduct.PayDetails.ChequeNo = objPartyOrderModel.objProduct.PayDetails.AccNo;
                                if (!string.IsNullOrEmpty(objPartyOrderModel.objProduct.PayDetails.ChequeDateStr))
                                {
                                    var SplitDate = objPartyOrderModel.objProduct.PayDetails.ChequeDateStr.Split('-');
                                    string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                                    objPartyOrderModel.objProduct.PayDetails.ChequeDate = Convert.ToDateTime(NewDate);
                                    objPartyOrderModel.objProduct.PayDetails.ChequeDate = objPartyOrderModel.objProduct.PayDetails.ChequeDate.Date;
                                }
                                else
                                {
                                    objPartyOrderModel.objProduct.PayDetails.ChequeDate = DateTime.Now.Date;
                                }
                                objPartyOrderModel.objProduct.PayDetails.AmountByCheque = objPartyOrderModel.objProduct.PayDetails.AmountByBD;
                                //objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByBD, BankCode = 0, ChqDDDate = null, ChqDDNo = "", CardNo = "", Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.BDBankName, AcNo = objModel.objProduct.PayDetails.AccNo, IFSCode = objModel.objProduct.PayDetails.IFSCCode, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });


                            }
                            if (objPartyOrderModel.objProduct.PayDetails.PayMode == "CC")
                            {
                                EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Card;
                                string value = EnumPayModes.GetEnumDescription(enumVar);
                                PayPrefix.Add(value);
                                objPartyOrderModel.objProduct.PayDetails.PayMode = objPartyOrderModel.objProduct.PayDetails.CardType;
                                objPartyOrderModel.objProduct.PayDetails.CHBankName = "";
                                objPartyOrderModel.objProduct.PayDetails.ChequeNo = objPartyOrderModel.objProduct.PayDetails.CardNo;
                                if (!string.IsNullOrEmpty(objPartyOrderModel.objProduct.PayDetails.ChequeDateStr))
                                {
                                    var SplitDate = objPartyOrderModel.objProduct.PayDetails.ChequeDateStr.Split('-');
                                    string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                                    objPartyOrderModel.objProduct.PayDetails.ChequeDate = Convert.ToDateTime(NewDate);
                                    objPartyOrderModel.objProduct.PayDetails.ChequeDate = objPartyOrderModel.objProduct.PayDetails.ChequeDate.Date;
                                }
                                else
                                {
                                    objPartyOrderModel.objProduct.PayDetails.ChequeDate = DateTime.Now.Date;
                                }
                                objPartyOrderModel.objProduct.PayDetails.AmountByCheque = objPartyOrderModel.objProduct.PayDetails.AmountByCard;
                                //objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, AcNo = "", IFSCode = "", BankCode = 0, Narration = "", BankName = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, Amount = objModel.objProduct.PayDetails.AmountByCard, CardNo = objModel.objProduct.PayDetails.CardNo, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                            }
                            if (objPartyOrderModel.objProduct.PayDetails.PayMode == "Q")
                            {
                                EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cheque;
                                string value = EnumPayModes.GetEnumDescription(enumVar);
                                PayPrefix.Add(value);
                                objPartyOrderModel.objProduct.PayDetails.PayMode = "Cheque";
                                objPartyOrderModel.objProduct.PayDetails.CHBankName = objPartyOrderModel.objProduct.PayDetails.BDBankName;
                                objPartyOrderModel.objProduct.PayDetails.ChequeNo = objPartyOrderModel.objProduct.PayDetails.ChequeNo;
                                if (!string.IsNullOrEmpty(objPartyOrderModel.objProduct.PayDetails.ChequeDateStr))
                                {
                                    var SplitDate = objPartyOrderModel.objProduct.PayDetails.ChequeDateStr.Split('-');
                                    string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                                    objPartyOrderModel.objProduct.PayDetails.ChequeDate = Convert.ToDateTime(NewDate);
                                    objPartyOrderModel.objProduct.PayDetails.ChequeDate = objPartyOrderModel.objProduct.PayDetails.ChequeDate.Date;
                                }
                                else
                                {
                                    objPartyOrderModel.objProduct.PayDetails.ChequeDate = DateTime.Now.Date;
                                }
                                objPartyOrderModel.objProduct.PayDetails.AmountByCheque = objPartyOrderModel.objProduct.PayDetails.AmountByCheque;
                                //objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByCheque, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.CHBankName, ChqDDNo = objModel.objProduct.PayDetails.ChequeNo, ChqDDDate = objModel.objProduct.PayDetails.ChequeDate, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                            }
                            if (objPartyOrderModel.objProduct.PayDetails.PayMode == "NE")
                            {
                                EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cheque;
                                string value = EnumPayModes.GetEnumDescription(enumVar);
                                PayPrefix.Add(value);
                                objPartyOrderModel.objProduct.PayDetails.PayMode = "NEFT";
                                objPartyOrderModel.objProduct.PayDetails.CHBankName = objPartyOrderModel.objProduct.PayDetails.BDBankName;
                                objPartyOrderModel.objProduct.PayDetails.ChequeNo = objPartyOrderModel.objProduct.PayDetails.ChequeNo;
                                objPartyOrderModel.objProduct.PayDetails.ChequeDate = DateTime.Now.Date;
                                objPartyOrderModel.objProduct.PayDetails.AmountByCheque = objPartyOrderModel.objProduct.PayDetails.AmountByCheque;
                                //objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByCheque, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.CHBankName, ChqDDNo = objModel.objProduct.PayDetails.ChequeNo, ChqDDDate = objModel.objProduct.PayDetails.ChequeDate, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                            }
                            if (objPartyOrderModel.objProduct.PayDetails.IsD)
                            {
                                EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.DD;
                                string value = EnumPayModes.GetEnumDescription(enumVar);
                                PayPrefix.Add(value);
                                objPartyOrderModel.objProduct.PayDetails.PayMode = "Bank Deposit";
                                objPartyOrderModel.objProduct.PayDetails.CHBankName = objPartyOrderModel.objProduct.PayDetails.DDBankName;
                                objPartyOrderModel.objProduct.PayDetails.ChequeNo = objPartyOrderModel.objProduct.PayDetails.DDNo;
                                objPartyOrderModel.objProduct.PayDetails.ChequeDate = DateTime.Now.Date;
                                objPartyOrderModel.objProduct.PayDetails.AmountByCheque = objPartyOrderModel.objProduct.PayDetails.AmountByDD;
                                //objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByDD, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.DDBankName, ChqDDNo = objModel.objProduct.PayDetails.DDNo, ChqDDDate = objModel.objProduct.PayDetails.DDDate, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                            }
                            if (objPartyOrderModel.objProduct.PayDetails.IsT)
                            {
                                EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Credit;
                                string value = EnumPayModes.GetEnumDescription(enumVar);
                                PayPrefix.Add(value);
                                objPartyOrderModel.objProduct.PayDetails.PayMode = "Credit";
                                objPartyOrderModel.objProduct.PayDetails.CHBankName = "";
                                objPartyOrderModel.objProduct.PayDetails.ChequeNo = "0";
                                objPartyOrderModel.objProduct.PayDetails.ChequeDate = DateTime.Now.Date;
                                objPartyOrderModel.objProduct.PayDetails.AmountByCheque = objPartyOrderModel.objProduct.PayDetails.AmountByCredit;
                                //objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, BankName = "", Amount = objModel.objProduct.PayDetails.AmountByCredit, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, DUserId = 0, DRecTimeStamp = null, ChqDDDate = null, ChqDDNo = "", Narration = objModel.objProduct.PayDetails.Narration, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                            }
                            if (objPartyOrderModel.objProduct.PayDetails.IsV)
                            {
                                EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Voucher;
                                string value = EnumPayModes.GetEnumDescription(enumVar);
                                PayPrefix.Add(value);
                                objPartyOrderModel.objProduct.PayDetails.PayMode = "Voucher";
                                objPartyOrderModel.objProduct.PayDetails.CHBankName = "";
                                objPartyOrderModel.objProduct.PayDetails.ChequeNo = "0";
                                objPartyOrderModel.objProduct.PayDetails.ChequeDate = DateTime.Now.Date;
                                objPartyOrderModel.objProduct.PayDetails.AmountByCheque = objPartyOrderModel.objProduct.PayDetails.AmountByVoucher;
                                //objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByVoucher, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, DUserId = 0, DRecTimeStamp = null, ChqDDDate = null, ChqDDNo = "", Narration = "", BankName = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                            }
                            if (objPartyOrderModel.objProduct.PayDetails.IsW)
                            {

                                objPartyOrderModel.objProduct.PayDetails.ChequeDate = DateTime.Now.Date;


                                if (WalletBalance >= objPartyOrderModel.objProduct.PayDetails.AmountByWallet)
                                {


                                    if (SC1.State == ConnectionState.Closed)
                                        SC1.Open();

                                    query = "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
           " Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objPartyOrderModel.OrderBy + "','" + (objPartyOrderModel.OrderTo).Trim() + "','" + decimal.Parse(objPartyOrderModel.objProduct.PayDetails.AmountByWallet.ToString()) + "','Order " + (objPartyOrderModel.OrderNo).Trim() + " generated for product.','" + (objPartyOrderModel.OrderNo).Trim() + "','R','O','Order Generated','" + SessId + "','" + FsessId + "' FROM TrnVoucher;";
                                    cmd = new SqlCommand();
                                    cmd.CommandText = query;
                                    cmd.Connection = SC1;
                                    int i = cmd.ExecuteNonQuery();

                                    SC1.Close();
                                    if (i > 0)
                                    {
                                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Wallet;
                                        string value = EnumPayModes.GetEnumDescription(enumVar);
                                        PayPrefix.Add(value);
                                        //objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByWallet, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, CardNo = objModel.objCustomer.CardNo, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                        ////insert entry into couponsalesdetails for wallet
                                        IsWalletEntry = true;
                                    }
                                    else
                                    {
                                        objResponse.ResponseStatus = "FAILED";
                                        objResponse.ResponseMessage = "Something went wrong";
                                        return objResponse;
                                    }
                                    i = 0;
                                }
                                else
                                {
                                    objResponse.ResponseStatus = "FAILED";
                                    objResponse.ResponseMessage = "Sorry!Insufficient Wallet Balance.";
                                    return objResponse;
                                }

                            }
                            if (objPartyOrderModel.objProduct.PayDetails.IsP)
                            {
                                EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Paytm;
                                string value = EnumPayModes.GetEnumDescription(enumVar);
                                PayPrefix.Add(value);
                                objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objPartyOrderModel.objProduct.TotalNetPayable, BillDate = DateTime.Now.Date, PayPrefix = value, Amount = objPartyOrderModel.objProduct.PayDetails.AmountByPaytm, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = objPartyOrderModel.objProduct.PayDetails.PaytmTransactionId, ChqDDDate = DateTime.Now, CardNo = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objPartyOrderModel.LoginUser.UserId, Version = version, UserName = objPartyOrderModel.LoginUser.UserName, FSessId = FsessId ?? 0 });
                            }
                            //if (objPartyOrderModel.objProduct.CashAmount > 0)
                            //{
                            //    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cash;
                            //    string value = EnumPayModes.GetEnumDescription(enumVar);
                            //    PayPrefix.Add(value);
                            //    //objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.CashAmount, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.TotalNetPayable, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, CardNo = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                            //}
                            if (PayPrefix.Count > 0)
                            {

                                Paymode = (from r in entity.M_PayModeMaster where PayPrefix.Contains(r.Prefix) select r.PayMode).ToList();
                            }

                        }
                    }

                    string SoldByCode = "";
                    List<TrnBillData> tempTableList = new List<TrnBillData>();
                    try
                    {
                        List<ProductModel> objListProductModel = new List<ProductModel>();

                        foreach (var obj in objPartyOrderModel.objListProduct)
                        {
                            objListProductModel.Add(obj);
                            TrnPartyOrderDetail objDTBillData = new TrnPartyOrderDetail();
                            objDTBillData.OrderNo = objPartyOrderModel.OrderNo;
                            objDTBillData.OrderTo = objPartyOrderModel.OrderTo;
                            objDTBillData.OrderBy = objPartyOrderModel.OrderBy;
                            var splitValues = objPartyOrderModel.OrderNo.Split('/');
                            objDTBillData.SOrderNo = decimal.Parse(splitValues[2]);
                            objDTBillData.PLNo = 0;
                            objDTBillData.PLDate = DateTime.Now;
                            objDTBillData.ProductCode = obj.ProdCode.ToString();
                            objDTBillData.ProductName = obj.ProductName;
                            objDTBillData.Qty = obj.Quantity;
                            objDTBillData.DispatchQty = 0;
                            objDTBillData.RemQty = obj.Quantity;
                            objDTBillData.Weight = 0;
                            objDTBillData.Carton = "";
                            objDTBillData.MonoCarton = "";
                            objDTBillData.MRP = obj.MRP ?? 0;
                            objDTBillData.DP = obj.DP ?? 0;
                            objDTBillData.Rate = obj.Rate ?? 0;
                            objDTBillData.Amount = obj.Amount;
                            objDTBillData.NetWeight = 0;
                            objDTBillData.DispatchAmt = 0;
                            objDTBillData.DispWeight = 0;
                            objDTBillData.ProdStatus = "O";
                            objDTBillData.PLGen = "N";
                            objDTBillData.OrdType = "O";
                            objDTBillData.Status = "P";
                            objDTBillData.CardStatus = "";
                            objDTBillData.ActiveStatus = "Y";
                            objDTBillData.Version = version;
                            objDTBillData.UserId = objPartyOrderModel.LoginUser.UserId;
                            objDTBillData.RecTimeStamp = DateTime.Now;
                            objDTBillData.PLUser = "0";
                            objDTBillData.PLUser = "";
                            objDTBillData.PLRecTimeStamp = DateTime.Now;
                            objDTBillData.Transporter = "";
                            objDTBillData.LRNo = "";
                            objDTBillData.LRDate = DateTime.Now;
                            objDTBillData.Fld1 = "";
                            objDTBillData.Fld2 = "";
                            objDTBillData.Fld3 = "";
                            //objDTBillData.BatchNo = obj.BatchNo;
                            //objDTBillData.Barcode = obj.Barcode;
                            objDTBillData.BatchNo = "";
                            objDTBillData.Barcode = "";
                            objDTBillData.PLQty = 0;
                            objDTBillData.PLDispQty = 0;
                            objDTBillData.PLRemQty = 0;
                            objDTBillData.PLStatus = "P";
                            objDTBillData.MID = 0;
                            objDTBillData.DiscPer = obj.DiscPer ?? 0;
                            objDTBillData.Discount = obj.DiscAmt ?? 0;
                            objDTBillData.FSessId = FsessId ?? 0;
                            objDTBillData.IsKit = "N";
                            objDTBillData.ProdType = "P";
                            objDTBillData.BV = obj.BV ?? 0;
                            objDTBillData.BVValue = obj.BVValue ?? 0;
                            objDTBillData.RP = obj.RP ?? 0;
                            objDTBillData.RPValue = obj.RPValue ?? 0;
                            objDTBillData.OfferUId = 0;
                            objDTBillData.VAT = 0;
                            objDTBillData.TaxAmount = obj.TaxAmt ?? 0;
                            objDTBillData.Tax = obj.TaxPer ?? 0;
                            //objDTBillData.CGST = obj.TaxPer / 2 ?? 0;
                            //objDTBillData.CGSTAmt = obj.TaxAmt / 2 ?? 0;
                            //objDTBillData.SGST = obj.TaxPer / 2 ?? 0;
                            //objDTBillData.SGSTAmt = obj.TaxAmt / 2 ?? 0;
                            objDTBillData.TaxType = "I";
                            entity.TrnPartyOrderDetails.Add(objDTBillData);
                        }
                        int i = 0;
                        try
                        {
                            i = entity.SaveChanges();
                            if (i == objPartyOrderModel.objListProduct.Count())
                            {
                                TrnPartyOrderMain objDTBillMain = new TrnPartyOrderMain();
                                objDTBillMain.OrderBy = objPartyOrderModel.OrderBy;
                                objDTBillMain.OrderTo = objPartyOrderModel.OrderTo;
                                objDTBillMain.OrderDate = BillDate.Date;
                                objDTBillMain.GroupId = objPartyOrderModel.LoginUser.GroupId;
                                objDTBillMain.PGroupId = 0;
                                var splitValues = objPartyOrderModel.OrderNo.Split('/');
                                objDTBillMain.SOrderNo = decimal.Parse(splitValues[2]);
                                objDTBillMain.OrderNo = objPartyOrderModel.OrderNo;
                                objDTBillMain.PLNo = 0;
                                objDTBillMain.PLDate = DateTime.Now;
                                objDTBillMain.BillNo = "";
                                objDTBillMain.BillDate = BillDate.Date;
                                objDTBillMain.PartyName = objPartyOrderModel.LoginUser.PartyName;
                                objDTBillMain.RefNo = "";
                                objDTBillMain.Paymode = Paymode[0];
                                objDTBillMain.chNo = string.IsNullOrEmpty(objPartyOrderModel.objProduct.PayDetails.ChequeNo) ? 0 : decimal.Parse(objPartyOrderModel.objProduct.PayDetails.ChequeNo);
                                //objPartyOrderModel.objProduct.PayDetails.ChequeDate = DateTime.Now;

                                objDTBillMain.ChDate = objPartyOrderModel.objProduct.PayDetails.ChequeDate;
                                objDTBillMain.ChAmt = objPartyOrderModel.objProduct.PayDetails.AmountByCheque;
                                objDTBillMain.BankName = objPartyOrderModel.objProduct.PayDetails.CHBankName;
                                objDTBillMain.TotalWeight = objPartyOrderModel.objProduct.PayDetails.AmountByWallet;
                                objDTBillMain.TotalOrdQty = objPartyOrderModel.objProduct.TotalQty;
                                objDTBillMain.TotalDispQty = 0;
                                objDTBillMain.TotalRemQty = objPartyOrderModel.objProduct.TotalQty;
                                objDTBillMain.TotalAmount = objPartyOrderModel.objProduct.TotalTotalAmount;
                                objDTBillMain.TotalTaxAmt = objPartyOrderModel.objProduct.TotalTaxAmount;
                                objDTBillMain.RndOff = objPartyOrderModel.objProduct.Roundoff;
                                objDTBillMain.NetPayable = objPartyOrderModel.objProduct.TotalNetPayable;
                                objDTBillMain.LastPLDate = DateTime.Now;
                                objDTBillMain.Remarks = string.IsNullOrEmpty(objPartyOrderModel.Remarks) ? "" : objPartyOrderModel.Remarks;
                                objDTBillMain.OType = "O";
                                objDTBillMain.PLUserId = 0;
                                objDTBillMain.PLUser = "";
                                objDTBillMain.PLRecTimeStamp = DateTime.Now;
                                objDTBillMain.IsModify = "N";
                                objDTBillMain.PLStatus = "P";
                                objDTBillMain.MID = 0;
                                objDTBillMain.Status = "P";
                                objDTBillMain.ActiveStatus = "Y";
                                objDTBillMain.Version = version;
                                objDTBillMain.UserId = objPartyOrderModel.LoginUser.UserId;
                                objDTBillMain.RecTimeStamp = DateTime.Now;
                                objDTBillMain.FSessId = FsessId ?? 0;
                                objDTBillMain.UserName = objPartyOrderModel.LoginUser.UserName;
                                objDTBillMain.IsConfirm = "N";
                                objDTBillMain.ConfDate = DateTime.Now;
                                objDTBillMain.ConfUserID = 0;
                                objDTBillMain.TotalDiscount = objPartyOrderModel.objProduct.TotalDiscount;
                                objDTBillMain.BankCode = objPartyOrderModel.objProduct.PayDetails.BankCode;
                                objDTBillMain.TotalBV = objPartyOrderModel.objProduct.TotalBV;
                                objDTBillMain.TotalRP = objPartyOrderModel.objProduct.TotalRP;
                                objDTBillMain.UID = objPartyOrderModel.objProduct.UID;
                                objDTBillMain.OrderAmount = objPartyOrderModel.objProduct.TotalNetPayable;

                                entity.TrnPartyOrderMains.Add(objDTBillMain);
                                i = 0;
                                i = entity.SaveChanges();
                                if (i > 0)
                                {

                                    if (SC1.State == ConnectionState.Closed)
                                        SC1.Open();
                                    query = "Exec OrderKitProducts '" + (objPartyOrderModel.OrderNo).Trim() + "';";
                                    cmd = new SqlCommand();
                                    cmd.CommandText = query;
                                    cmd.Connection = SC1;
                                    i = cmd.ExecuteNonQuery();
                                    SC1.Close();
                                    objResponse.ResponseMessage = "Saved Successfully!";
                                    objResponse.ResponseStatus = "OK";
                                }
                                else
                                {
                                    objResponse.ResponseMessage = "Something went wrong!";
                                    objResponse.ResponseStatus = "FAILED";
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }

            }
            catch (Exception ex)
            {

            }
            return objResponse;
        }

        public List<PartyOrderModel> GetOrderList(string Orderby, string OrderTo, string Status)
        {
            List<PartyOrderModel> objPartyOrderModel = new List<PartyOrderModel>();
            List<PartyOrderModel> objPartyOrderDetailModel = new List<PartyOrderModel>();

            try
            {
                using (var entity = new InventoryEntities())
                {


                    objPartyOrderModel = (from r in entity.TrnPartyOrderMains
                                          where r.ActiveStatus == "Y"
                                          from l in entity.M_LedgerMaster
                                          where r.OrderBy == l.PartyCode

                                          select new PartyOrderModel
                                          {
                                              OrderNo = r.OrderNo,
                                              PartyCode = r.OrderBy,
                                              PartyName = l.PartyName,
                                              OrderDate = r.OrderDate,
                                              OrderAmt = r.TotalWeight,
                                              ChNo = r.chNo.ToString() ?? "0",
                                              ChDate = r.ChDate ?? DateTime.Now,
                                              ChAmt = r.ChAmt ?? 0,
                                              BankName = r.BankName,
                                              WalletAmt = r.TotalWeight,
                                              OrderBy = r.OrderBy,
                                              OrderTo = r.OrderTo,
                                              DispStatus = r.Status,
                                              OrderMethod = r.OrderMethod,
                                              SOrderNo = r.SOrderNo,
                                              //  DispAmt = (from s in entity.TrnPartyOrderDetails where r.SOrderNo == s.SOrderNo select s.DispatchAmt).Sum(),
                                              DispAmt = r.NetPayable,
                                              RemAmt = r.TotalWeight - r.NetPayable,
                                              Ispromobaluse = r.Ispromobaluse
                                          }

                                        ).ToList();

                    if (Orderby.ToUpper() != "ALL")
                        objPartyOrderModel = objPartyOrderModel.Where(m => m.OrderBy == Orderby).ToList();
                    if (OrderTo.ToUpper() != "ALL")
                        objPartyOrderModel = objPartyOrderModel.Where(m => m.OrderTo == OrderTo).ToList();
                    if (Status.ToUpper() != "A")
                        objPartyOrderModel = objPartyOrderModel.Where(m => m.DispStatus == Status).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return objPartyOrderModel;
        }
        public PartyOrderModel GetOrderPrintdata(string OrderNo)
        {
            PartyOrderModel obj = new PartyOrderModel();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    obj = (from r in entity.TrnPartyOrderMains
                           where r.ActiveStatus == "Y"
                           from l in entity.M_LedgerMaster
                           where r.OrderBy == l.PartyCode
                           from S in entity.M_LedgerMaster
                           where r.OrderTo == S.PartyCode
                           && r.OrderNo == OrderNo
                           select new PartyOrderModel
                           {
                               OrderNo = r.OrderNo,
                               PartyCode = r.OrderBy,
                               PartyName = l.PartyName,
                               Address = l.Address1,
                               OrderDate = r.OrderDate,
                               OrderAmt = r.OrderAmount,
                               ChNo = r.chNo.ToString() ?? "0",
                               ChDate = r.ChDate ?? DateTime.Now,
                               ChAmt = r.ChAmt ?? 0,
                               BankName = r.BankName,
                               WalletAmt = r.TotalWeight,
                               OrderBy = r.OrderBy,
                               OrderTo = r.OrderTo,
                               DispStatus = r.Status,
                               Mobileno = l.MobileNo,
                               NetPayable = r.NetPayable,
                               TotalOrdQty = r.TotalOrdQty,
                               TopartyName = S.PartyName,


                           }
                                        ).FirstOrDefault();
                    if (obj != null)
                    {

                        var objOrderProductModel = (from r in entity.TrnPartyOrderDetails
                                                    where r.ActiveStatus == "Y" && r.OrderNo == OrderNo /*&& r.Status == "P"*/// && r.ProdType=="P"
                                                    from p in entity.TrnPartyOrderMains
                                                    where p.OrderNo == OrderNo
                                                    from S in entity.M_ProductMaster
                                                    where S.ProdId == r.ProductCode
                                                    select new ProductModel
                                                    {
                                                        ProductName = r.ProductName,
                                                        Barcode = S.Barcode,
                                                        DP = r.DP,
                                                        RP = r.RP,
                                                        DiscPer = r.DiscPer,
                                                        DiscAmt = r.Discount,
                                                        ProductCodeStr = r.ProductCode,
                                                        TaxPer = r.Tax,
                                                        TaxAmt = r.TaxAmount,
                                                        MRP = r.MRP,
                                                        BV = r.BV,
                                                        PV = 0,
                                                        CV = 0,
                                                        Rate = r.Rate,
                                                        Amount = r.Amount,
                                                        TotalNetPayable = p.NetPayable,
                                                        OrderedOty = r.RemQty == 0 ? r.Qty : r.RemQty,
                                                        DispQty = r.DispatchQty,
                                                        OfferUID = r.OfferUId,
                                                        ProductType = r.ProdType
                                                    }
                                         ).ToList();

                        List<ProductModel> objprodmodel = new List<ProductModel>();
                        objOrderProductModel = objOrderProductModel.OrderBy(m => m.ExpDate).ToList();

                        foreach (var TempObj in objOrderProductModel)
                        {
                            var totalqty = TempObj.OrderedOty;


                            var GetprodBatch = (from p in entity.M_BatchMaster
                                                where p.ProdId == TempObj.ProductCodeStr.ToString() && p.ActiveStatus == "Y"
                                                join stockAvail in entity.Im_CurrentStock
                                                on new { ProdId = p.ProdId.ToString(), BatchCode = p.BatchNo } equals new { stockAvail.ProdId, stockAvail.BatchCode } into stockGroup
                                                from sg in stockGroup.DefaultIfEmpty()
                                                where sg == null || sg.FCode == "WR"
                                                group sg by new
                                                {
                                                    p.BatchCode,
                                                    p.BatchNo,
                                                    p.ExpDate,
                                                    p.IsExpired,
                                                    p.ProdId
                                                } into g
                                                select new
                                                {
                                                    BatchCode = g.Key.BatchCode,
                                                    BatchNo = g.Key.BatchNo.ToString(),
                                                    ExpDate = g.Key.ExpDate,
                                                    IsExpirable = g.Key.IsExpired == "Y",
                                                    ProdId = g.Key.ProdId,
                                                    StockAvailable = g.Sum(x => x != null ? x.Qty : 0)
                                                }).OrderBy(p => p.ExpDate).ToList();

                            foreach (var item in GetprodBatch)
                            {
                                //if ((item.IsExpirable && item.ExpDate > DateTime.Now) || (item.IsExpirable == false))
                                //{
                                ProductModel pp = CreateProductModel(TempObj, item);
                                if (GetprodBatch.Count() == 1 && item.StockAvailable == 0)
                                {
                                    objprodmodel.Add(pp);
                                }
                                else if (item.StockAvailable != 0)
                                {
                                    if (totalqty == item.StockAvailable)
                                    {
                                        pp.OrderedOty = totalqty;
                                        objprodmodel.Add(pp);
                                        break;
                                    }
                                    else if (item.StockAvailable >= totalqty)
                                    {
                                        pp.OrderedOty = totalqty;
                                        objprodmodel.Add(pp);
                                        break;
                                    }
                                    else if (totalqty >= item.StockAvailable)
                                    {
                                        totalqty = totalqty - item.StockAvailable;
                                        pp.OrderedOty = totalqty;
                                        objprodmodel.Add(pp);
                                    }
                                }
                                //}

                            }
                        }
                        obj.objListProduct = objprodmodel;
                    }

                    var result = (from r in entity.M_CompanyMaster where r.ActiveStatus == "Y" select r).FirstOrDefault();
                    if (result != null)
                    {
                        obj.CompCity = result.CompCity;
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        private static ProductModel CreateProductModel(ProductModel TempObj, dynamic item)
        {
            return new ProductModel
            {
                ProductName = TempObj.ProductName,
                Barcode = TempObj.Barcode,
                BatchNo = item.BatchNo,
                DP = TempObj.DP,
                RP = TempObj.RP,
                DiscPer = TempObj.DiscPer,
                DiscAmt = TempObj.DiscAmt,
                ProductCodeStr = TempObj.ProductCodeStr,
                TaxPer = TempObj.TaxPer,
                TaxAmt = TempObj.TaxAmt,
                MRP = TempObj.MRP,
                BV = TempObj.BV,
                PV = TempObj.PV,
                CV = TempObj.CV,
                Rate = TempObj.Rate,
                Amount = TempObj.Amount,
                TotalNetPayable = TempObj.TotalNetPayable,
                OrderedOty = TempObj.OrderedOty,
                DispQty = TempObj.DispQty,
                OfferUID = TempObj.OfferUID,
                ProductType = TempObj.ProductType,
                StockAvailable = item.StockAvailable
            };
        }
        public List<ProductModel> PendingGetProductInfo(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool allhalf, string Invoice, string IsSpclOffer)
        {
            List<ProductModel> objProductModel = new List<ProductModel>();
            List<ProductModel> TempResult = new List<ProductModel>();
            bool searchByProductFlag = true;
            if (SearchType == "B")
            {
                searchByProductFlag = false;
            }

            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    using (var entity = new InventoryEntities())
                    {

                        if (searchByProductFlag)
                        {

                            TempResult = (from product in entity.M_ProductMaster
                                          where product.ProductName.ToLower().Equals(data.ToLower()) && product.ActiveStatus == "Y" //&& product.IsCardIssue == "N"
                                          join barcode in entity.M_BarCodeMaster on product.ProdId equals barcode.ProdId
                                          where barcode.ActiveStatus == "Y"
                                          join BatchCode in entity.M_BatchMaster on product.ProdId equals BatchCode.ProdId
                                          where BatchCode.ActiveStatus == "Y"
                                          join tax in entity.M_TaxMaster on product.ProdId equals tax.ProdCode
                                          //where tax.StateCode == CurrentStateCode
                                          from c in entity.M_CatMaster
                                          where c.CatId == product.CatId
                                          select new ProductModel
                                          {
                                              ProductName = product.ProductName,
                                              CatId = product.CatId,
                                              CatName = c.CatName,
                                              Barcode = barcode.BarCode,
                                              BatchNo = BatchCode.BatchNo,
                                              DP = BatchCode.Dp,
                                              RP = product.RP,
                                              DiscPer = product.Discount,
                                              DiscAmt = product.DiscInRs,
                                              ProdCode = (int)product.ProductCode,
                                              ProductCodeStr = product.ProdId,
                                              TaxPer = tax.VatTax,
                                              ProdStateCode = tax.StateCode,
                                              MRP = BatchCode.Mrp,
                                              BV = product.BV,
                                              PV = product.PV,
                                              CV = product.CV,
                                              IsExpirable = BatchCode.IsExpired == "Y" ? true : false,
                                              ExpDate = BatchCode.ExpDate,
                                              TaxType = "GST",
                                              Rate = product.PurchaseRate,
                                              CommissionPer = product.ProdCommssn,
                                              SubCatId = product.SubCatId,
                                              IsAvailableForOffer = product.IsAvailableforOffers,
                                              IsAvailableForBilling = product.IsBillingAllowed,
                                              Weight = product.Weight,
                                              TotalDiscPer = product.SJDiscount ?? 0
                                          }).ToList();


                        }
                        else
                        {
                            decimal? BarCodeData = decimal.Parse(data);
                            TempResult = (from product in entity.M_ProductMaster
                                          where product.ActiveStatus == "Y" && product.IsCardIssue == "N"
                                          join barcode in entity.M_BarCodeMaster on product.ProdId equals barcode.ProdId
                                          where barcode.BarCode == data
                                          join BatchCode in entity.M_BatchMaster on product.ProdId equals BatchCode.ProdId
                                          where BatchCode.ActiveStatus == "Y"
                                          join tax in entity.M_TaxMaster on product.ProdId equals tax.ProdCode
                                          from c in entity.M_CatMaster
                                          where c.CatId == product.CatId
                                          select new ProductModel
                                          {
                                              ProductName = product.ProductName,
                                              CatId = product.CatId,
                                              CatName = c.CatName,
                                              Barcode = barcode.BarCode,
                                              BatchNo = BatchCode.BatchNo,
                                              DP = BatchCode.Dp,
                                              RP = product.RP,
                                              ProdStateCode = tax.StateCode,
                                              DiscPer = product.Discount,
                                              DiscAmt = product.DiscInRs,
                                              ProdCode = (int)product.ProductCode,
                                              ProductCodeStr = product.ProdId,
                                              TaxPer = tax.VatTax,
                                              MRP = BatchCode.Mrp,
                                              BV = product.BV,
                                              PV = product.PV,
                                              CV = product.CV,
                                              IsExpirable = BatchCode.IsExpired == "Y" ? true : false,
                                              ExpDate = BatchCode.ExpDate,
                                              TaxType = "GST",
                                              Rate = product.PurchaseRate,
                                              CommissionPer = product.ProdCommssn,
                                              IsAvailableForOffer = product.IsAvailableforOffers,
                                              TotalDiscPer = product.SJDiscount ?? 0
                                          }).ToList();


                        }
                        bool IsDistributorBill = false;
                        bool IsPartyBill = false;
                        bool IsCustomerBill = false;
                        bool IsPurchaseInvoice = false;
                        bool IsOrderCreation = false;
                        bool IsPendingOrder = false;
                        if (BillType == "distributor")
                        {
                            IsDistributorBill = true;
                        }
                        else
                        {
                            IsDistributorBill = false;
                        }
                        if (BillType == "party")
                        {
                            IsPartyBill = true;
                        }
                        else
                        {
                            IsPartyBill = false;
                        }
                        if (BillType == "customer")
                        {
                            IsCustomerBill = true;
                        }
                        else
                        {
                            IsCustomerBill = false;
                        }
                        if (BillType == "purchase")
                        {
                            IsPurchaseInvoice = true;
                        }
                        else
                        {
                            IsPurchaseInvoice = false;
                        }
                        if (BillType == "order")
                        {
                            IsOrderCreation = true;
                        }
                        else
                        {
                            IsOrderCreation = false;
                        }
                        if (BillType == "pendingorder")
                        {
                            IsPendingOrder = true;
                        }
                        else
                        {
                            IsPendingOrder = false;
                        }

                        foreach (var obj in TempResult)
                        {
                            ProductModel TempObj = new ProductModel();
                            if ((obj.IsExpirable && obj.ExpDate > DateTime.Now) || (obj.IsExpirable == false))
                            {
                                TempObj = obj;
                                object valueIsDiscountAdd = 0;
                                object valueIsCommissonAdd = 0;
                                if (IsDistributorBill || IsCustomerBill || IsPurchaseInvoice || IsOrderCreation || IsPendingOrder)
                                {
                                    valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAdd.ToString());
                                    valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAdd.ToString());

                                }
                                else
                                {
                                    valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAddOnPartyBill.ToString());
                                    valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAddOnPartyBill.ToString());

                                }
                                int IsCommission = Convert.ToInt32(valueIsCommissonAdd);
                                int IsDiscount = Convert.ToInt32(valueIsDiscountAdd);
                                TempObj.IsCommissionAdd = IsCommission;
                                TempObj.IsDiscountAdd = IsDiscount;
                                TempObj.StockAvailable = (from stockAvail in entity.Im_CurrentStock
                                                          where stockAvail.Barcode == TempObj.Barcode.ToString() && stockAvail.ProdId == TempObj.ProductCodeStr.ToString() && stockAvail.FCode.Equals(CurrentPartyCode)
                                                          && stockAvail.BatchCode == TempObj.BatchNo
                                                          select stockAvail.Qty
                                                     ).DefaultIfEmpty(0).Sum();
                                TempObj.DP1 = TempObj.DP;
                                if (IsCustomerBill)
                                {
                                    TempObj.DP = obj.MRP;

                                }
                                else
                                {
                                    if (!IsPurchaseInvoice && IsBillOnMrp)
                                    {
                                        TempObj.DP = obj.MRP;
                                    }
                                }
                                CurrentStateCode = (int)(from r in entity.M_CompanyMaster select r.CompState).FirstOrDefault();
                                if (allhalf)
                                {
                                    TempObj.DP = TempObj.DP / 2;
                                    TempObj.BV = TempObj.BV / 2;
                                    TempObj.PV = TempObj.PV / 2;
                                    TempObj.RP = TempObj.RP / 2;
                                    TempObj.DiscAmt = TempObj.DP;
                                    TempObj.IsDiscountAdd = 1;
                                }
                                if (!string.IsNullOrEmpty(Invoice) && Invoice == "CED")
                                {
                                    var oridp = TempObj.DP;
                                    TempObj.DP = (TempObj.DP * 1) / 4;
                                    TempObj.BV = 0;
                                    TempObj.PV = (TempObj.PV * 1) / 4;
                                    TempObj.RP = (TempObj.RP * 1) / 4;
                                    TempObj.DiscAmt = oridp - TempObj.DP;
                                    TempObj.IsDiscountAdd = 1;
                                }
                                if (!string.IsNullOrEmpty(OfferID))
                                {
                                    decimal iOfferID = Convert.ToDecimal(OfferID);
                                    if (iOfferID != 0)
                                    {
                                        TempObj.offerDetail = GetOfferDetail(iOfferID, obj.ProductCodeStr, IsSpclOffer);
                                        if (!string.IsNullOrEmpty(TempObj.offerDetail.offerType))
                                        {
                                            decimal offerType = Convert.ToDecimal(TempObj.offerDetail.offerType);
                                            if (offerType == 2 || offerType == 3)
                                            {
                                                TempObj.DP = TempObj.offerDetail.OfferMrp / 1;// TempObj.offerDetail.FreeQty;
                                                TempObj.OfferProdQty = TempObj.offerDetail.FreeQty;
                                                TempObj.OfferProdQty = 1;
                                                TempObj.BV = 0;
                                            }
                                        }
                                    }
                                }
                                objProductModel.Add(TempObj);
                            }
                        }
                        if (objProductModel.Count > 1 && !IsPurchaseInvoice)
                        {
                            objProductModel = objProductModel.Where(m => m.StockAvailable > 0).OrderBy(m => m.ExpDate).ThenBy(m => m.StockAvailable).ToList();
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return objProductModel;
        }
        public List<ProductModel> GetPendingOrderProductList(string OrderNo, string OrderBy, string CurrentPartyCode)
        {
            List<ProductModel> objOrderProductModel = new List<ProductModel>();
            List<ProductModel> TempResult = new List<ProductModel>();

            List<ProductModel> Returnlist = new List<ProductModel>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objOrderProductModel = (from r in entity.TrnPartyOrderDetails
                                            where r.ActiveStatus == "Y" && r.OrderNo == OrderNo && r.OrderBy == OrderBy && r.Status == "P"// && r.ProdType=="P"
                                            from p in entity.TrnPartyOrderMains
                                            from l in entity.M_LedgerMaster
                                            where p.OrderNo == OrderNo && l.PartyCode == p.OrderBy
                                            select new ProductModel
                                            {
                                                ProductName = r.ProductName,
                                                Barcode = r.Barcode,
                                                BatchNo = r.BatchNo,
                                                DP = r.DP,
                                                RP = r.RP,
                                                DiscPer = r.DiscPer,
                                                DiscAmt = r.Discount,
                                                ProductCodeStr = r.ProductCode,
                                                TaxPer = r.Tax,
                                                TaxAmt = r.TaxAmount,
                                                MRP = r.MRP,
                                                BV = r.BV,
                                                PV = 0,
                                                CV = 0,
                                                Rate = r.Rate,
                                                Amount = r.Amount,
                                                TotalNetPayable = p.NetPayable,
                                                OrderedOty = r.RemQty == 0 ? r.Qty : r.RemQty,
                                                DispQty = r.DispatchQty,
                                                OfferUID = r.OfferUId,
                                                ProductType = r.ProdType,
                                                DispatchQty = r.DispatchQty,
                                                RemainingQty = r.RemQty,
                                                OrderQty = r.Qty,
                                                Ispromobaluse = p.Ispromobaluse,
                                                OrderBy = r.OrderBy,
                                                PartyGSTNo = l.TinNo
                                            }
                                          ).ToList();

                    bool IsDistributorBill = false;
                    bool IsPartyBill = true;
                    bool IsCustomerBill = false;
                    bool IsPurchaseInvoice = false;
                    bool IsOrderCreation = false;
                    bool IsPendingOrder = false;
                    int CurrentStateCode = 0;
                    bool IsBillOnMrp = false;
                    string IsSpclOffer = "";
                    string Invoice = "";
                    bool allhalf = false;
                    if (objOrderProductModel != null)
                    {
                        foreach (var item in objOrderProductModel)
                        {
                            TempResult = (from product in entity.M_ProductMaster
                                          where product.ProductName.ToLower().Equals(item.ProductName.ToLower()) && product.ActiveStatus == "Y" //&& product.IsCardIssue == "N"
                                          join barcode in entity.M_BarCodeMaster on product.ProdId equals barcode.ProdId
                                          where barcode.ActiveStatus == "Y"
                                          join BatchCode in entity.M_BatchMaster on product.ProdId equals BatchCode.ProdId
                                          where BatchCode.ActiveStatus == "Y"
                                          join tax in entity.M_TaxMaster on product.ProdId equals tax.ProdCode
                                          //where tax.StateCode == CurrentStateCode
                                          from c in entity.M_CatMaster
                                          where c.CatId == product.CatId
                                          select new ProductModel
                                          {
                                              ProductName = product.ProductName,
                                              CatId = product.CatId,
                                              CatName = c.CatName,
                                              Barcode = barcode.BarCode,
                                              BatchNo = BatchCode.BatchNo,
                                              DP = BatchCode.Dp,
                                              RP = product.RP,
                                              DiscPer = product.Discount,
                                              DiscAmt = product.DiscInRs,
                                              ProdCode = (int)product.ProductCode,
                                              ProductCodeStr = product.ProdId,
                                              TaxPer = tax.VatTax,
                                              ProdStateCode = tax.StateCode,
                                              MRP = BatchCode.Mrp,
                                              BV = product.BV,
                                              PV = product.PV,
                                              CV = product.CV,
                                              IsExpirable = BatchCode.IsExpired == "Y" ? true : false,
                                              ExpDate = BatchCode.ExpDate,
                                              TaxType = "GST",
                                              Rate = product.PurchaseRate,
                                              CommissionPer = product.ProdCommssn,
                                              SubCatId = product.SubCatId,
                                              IsAvailableForOffer = product.IsAvailableforOffers,
                                              IsAvailableForBilling = product.IsBillingAllowed,
                                              Weight = product.Weight,
                                              TotalDiscPer = product.SJDiscount ?? 0
                                          }).ToList();
                            List<ProductModel> objProductModel = new List<ProductModel>();
                            foreach (var obj in TempResult)
                            {
                                ProductModel TempObj = new ProductModel();
                                if ((obj.IsExpirable && obj.ExpDate > DateTime.Now) || (obj.IsExpirable == false))
                                {
                                    TempObj = obj;
                                    object valueIsDiscountAdd = 0;
                                    object valueIsCommissonAdd = 0;
                                    if (IsDistributorBill || IsCustomerBill || IsPurchaseInvoice || IsOrderCreation || IsPendingOrder)
                                    {
                                        valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAdd.ToString());
                                        valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAdd.ToString());
                                    }
                                    else
                                    {
                                        valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAddOnPartyBill.ToString());
                                        valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAddOnPartyBill.ToString());
                                    }
                                    int IsCommission = Convert.ToInt32(valueIsCommissonAdd);
                                    int IsDiscount = Convert.ToInt32(valueIsDiscountAdd);
                                    TempObj.IsCommissionAdd = IsCommission;
                                    TempObj.IsDiscountAdd = IsDiscount;
                                    TempObj.StockAvailable = (from stockAvail in entity.Im_CurrentStock
                                                              where stockAvail.ProdId == TempObj.ProductCodeStr.ToString() && stockAvail.FCode.Equals(CurrentPartyCode)
                                                              && stockAvail.BatchCode == TempObj.BatchNo
                                                              select stockAvail.Qty
                                                         ).DefaultIfEmpty(0).Sum();
                                    TempObj.DP1 = TempObj.DP;
                                    if (IsCustomerBill)
                                    {
                                        TempObj.DP = obj.MRP;
                                    }
                                    else
                                    {
                                        if (!IsPurchaseInvoice && IsBillOnMrp)
                                        {
                                            TempObj.DP = obj.MRP;
                                        }
                                    }
                                    CurrentStateCode = (int)(from r in entity.M_CompanyMaster select r.CompState).FirstOrDefault();
                                    if (allhalf)
                                    {
                                        TempObj.DP = TempObj.DP / 2;
                                        TempObj.BV = TempObj.BV / 2;
                                        TempObj.PV = TempObj.PV / 2;
                                        TempObj.RP = TempObj.RP / 2;
                                        TempObj.DiscAmt = TempObj.DP;
                                        TempObj.IsDiscountAdd = 1;
                                    }
                                    if (!string.IsNullOrEmpty(Invoice) && Invoice == "CED")
                                    {
                                        var oridp = TempObj.DP;
                                        TempObj.DP = (TempObj.DP * 1) / 4;
                                        TempObj.BV = 0;
                                        TempObj.PV = (TempObj.PV * 1) / 4;
                                        TempObj.RP = (TempObj.RP * 1) / 4;
                                        TempObj.DiscAmt = oridp - TempObj.DP;
                                        TempObj.IsDiscountAdd = 1;
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item.OfferUID)))
                                    {
                                        decimal iOfferID = Convert.ToDecimal(item.OfferUID);
                                        if (iOfferID != 0)
                                        {
                                            TempObj.offerDetail = GetOfferDetail(iOfferID, obj.ProductCodeStr, IsSpclOffer);
                                            if (!string.IsNullOrEmpty(TempObj.offerDetail.offerType))
                                            {
                                                decimal offerType = Convert.ToDecimal(TempObj.offerDetail.offerType);
                                                if (offerType == 2 || offerType == 3)
                                                {
                                                    TempObj.DP = TempObj.offerDetail.OfferMrp / 1;// TempObj.offerDetail.FreeQty;
                                                    TempObj.OfferProdQty = TempObj.offerDetail.FreeQty;
                                                    TempObj.OfferProdQty = 1;
                                                    TempObj.BV = 0;
                                                }
                                            }
                                        }
                                    }
                                    objProductModel.Add(TempObj);
                                }
                            }
                            objProductModel = objProductModel.Where(m => m.StockAvailable > 0).OrderBy(m => m.ExpDate).ThenBy(m => m.StockAvailable).ToList();
                            if (objProductModel.Count == 0)
                            {
                                ProductModel pp = new ProductModel();

                                pp.DispatchQty = item.DispQty;
                                pp.RemainingQty = item.RemainingQty;
                                pp.OrderQty = item.OrderQty;
                                pp.ProductName = item.ProductName;
                                pp.Barcode = item.Barcode;
                                pp.BatchNo = item.BatchNo;
                                pp.DP = item.DP;
                                pp.RP = item.RP;
                                pp.DiscPer = item.DiscPer;
                                pp.DiscAmt = item.DiscAmt;
                                pp.ProductCodeStr = item.ProductCodeStr;
                                pp.TaxPer = item.TaxPer;
                                pp.TaxAmt = item.TaxAmt;
                                pp.MRP = item.MRP;
                                pp.BV = item.BV;
                                pp.PV = item.PV;
                                pp.CV = item.CV;
                                pp.Rate = item.Rate;
                                pp.Amount = item.Amount;
                                pp.TotalNetPayable = Convert.ToDecimal(item.Amount) + Convert.ToDecimal(item.TaxAmt);
                                pp.OrderedOty = item.OrderedOty;
                                pp.DispQty = item.DispQty;
                                pp.OfferUID = item.OfferUID;
                                pp.ProductType = item.ProductType;
                                pp.IsCommissionAdd = item.IsCommissionAdd;
                                pp.IsDiscountAdd = item.IsDiscountAdd;
                                pp.OfferProdQty = item.OfferProdQty;
                                pp.IsAvailableForBilling = item.IsAvailableForBilling;
                                pp.IsAvailableForOffer = item.IsAvailableForOffer;
                                pp.StockAvailable = 0;
                                pp.DispatchQty = item.DispatchQty;
                                pp.RemainingQty = item.RemainingQty;
                                pp.OrderQty = item.OrderQty;
                                pp.Ispromobaluse = item.Ispromobaluse;
                                pp.PartyGSTNo = item.PartyGSTNo;
                                Returnlist.Add(pp);
                            }
                            else
                            {
                                foreach (var prodcalc in objProductModel)
                                {
                                    ProductModel pp = new ProductModel();
                                    var totalqty = item.OrderedOty;
                                    //var totalqty = item.RemainingQty;

                                    if (totalqty == prodcalc.StockAvailable)
                                    {

                                        pp.DispatchQty = item.DispQty;
                                        pp.RemainingQty = item.RemainingQty;
                                        pp.OrderQty = item.OrderQty;
                                        pp.OrderedOty = item.OrderedOty;
                                        pp.DispQty = totalqty;
                                        pp.ProductName = item.ProductName;
                                        pp.Barcode = prodcalc.Barcode;
                                        pp.BatchNo = prodcalc.BatchNo;
                                        pp.DP = item.DP;
                                        pp.RP = item.RP;
                                        pp.DiscPer = item.DiscPer;
                                        pp.DiscAmt = item.DiscAmt;
                                        pp.ProductCodeStr = item.ProductCodeStr;
                                        pp.TaxPer = item.TaxPer;
                                        pp.TaxAmt = item.TaxAmt;
                                        pp.MRP = item.MRP;
                                        pp.BV = item.BV;
                                        pp.PV = item.PV;
                                        pp.CV = item.CV;
                                        pp.Rate = item.Rate;
                                        pp.Amount = item.Amount;
                                        pp.TotalNetPayable = Convert.ToDecimal(item.Amount) + Convert.ToDecimal(item.TaxAmt);
                                        pp.OfferUID = item.OfferUID;
                                        pp.ProductType = item.ProductType;
                                        pp.IsCommissionAdd = prodcalc.IsCommissionAdd;
                                        pp.IsDiscountAdd = prodcalc.IsDiscountAdd;
                                        pp.OfferProdQty = prodcalc.OfferProdQty;
                                        pp.IsAvailableForBilling = prodcalc.IsAvailableForBilling;
                                        pp.IsAvailableForOffer = prodcalc.IsAvailableForOffer;
                                        pp.StockAvailable = prodcalc.StockAvailable;
                                        pp.DispatchQty = item.DispatchQty;
                                        pp.RemainingQty = item.RemainingQty;
                                        pp.OrderQty = item.OrderQty;
                                        pp.Ispromobaluse = item.Ispromobaluse;
                                        pp.PartyGSTNo = item.PartyGSTNo;
                                        Returnlist.Add(pp);
                                        break;
                                    }
                                    else if (prodcalc.StockAvailable >= totalqty)
                                    {

                                        pp.DispatchQty = item.DispQty;
                                        pp.RemainingQty = item.RemainingQty;
                                        pp.OrderQty = item.OrderQty;
                                        pp.OrderedOty = item.OrderedOty;
                                        pp.DispQty = totalqty;
                                        pp.ProductName = item.ProductName;
                                        pp.Barcode = prodcalc.Barcode;
                                        pp.BatchNo = prodcalc.BatchNo;
                                        pp.DP = item.DP;
                                        pp.RP = prodcalc.RP;
                                        pp.DiscPer = item.DiscPer;
                                        pp.DiscAmt = item.DiscAmt;
                                        pp.ProductCodeStr = item.ProductCodeStr;
                                        pp.TaxPer = item.TaxPer;
                                        pp.TaxAmt = item.TaxAmt;
                                        pp.MRP = item.MRP;
                                        pp.BV = item.BV;
                                        pp.PV = item.PV;
                                        pp.CV = item.CV;
                                        pp.Rate = item.Rate;
                                        pp.Amount = item.Amount;
                                        pp.TotalNetPayable = Convert.ToDecimal(item.Amount) + Convert.ToDecimal(item.TaxAmt);
                                        pp.OfferUID = item.OfferUID;
                                        pp.ProductType = item.ProductType;
                                        pp.IsCommissionAdd = prodcalc.IsCommissionAdd;
                                        pp.IsDiscountAdd = prodcalc.IsDiscountAdd;
                                        pp.OfferProdQty = prodcalc.OfferProdQty;
                                        pp.IsAvailableForBilling = prodcalc.IsAvailableForBilling;
                                        pp.IsAvailableForOffer = prodcalc.IsAvailableForOffer;
                                        pp.StockAvailable = prodcalc.StockAvailable;
                                        pp.DispatchQty = item.DispatchQty;
                                        pp.RemainingQty = item.RemainingQty;
                                        pp.OrderQty = item.OrderQty;
                                        pp.Ispromobaluse = item.Ispromobaluse;
                                        pp.PartyGSTNo = item.PartyGSTNo;
                                        Returnlist.Add(pp);
                                        break;
                                    }
                                    else if (totalqty >= prodcalc.StockAvailable)
                                    {
                                        pp.DispatchQty = item.DispQty;
                                        pp.RemainingQty = prodcalc.StockAvailable;
                                        pp.OrderQty = item.OrderQty;
                                        totalqty = totalqty - item.StockAvailable;
                                        pp.OrderedOty = item.OrderedOty;
                                        pp.DispQty = totalqty;
                                        pp.ProductName = item.ProductName;
                                        pp.Barcode = prodcalc.Barcode;
                                        pp.BatchNo = prodcalc.BatchNo;
                                        pp.DP = item.DP;
                                        pp.RP = item.RP;
                                        pp.DiscPer = item.DiscPer;
                                        pp.DiscAmt = item.DiscAmt;
                                        pp.ProductCodeStr = item.ProductCodeStr;
                                        pp.TaxPer = item.TaxPer;
                                        pp.TaxAmt = item.TaxAmt;
                                        pp.MRP = item.MRP;
                                        pp.BV = item.BV;
                                        pp.PV = item.PV;
                                        pp.CV = item.CV;
                                        pp.Rate = item.Rate;
                                        pp.Amount = item.Amount;
                                        pp.TotalNetPayable = Convert.ToDecimal(item.Amount) + Convert.ToDecimal(item.TaxAmt);
                                        pp.OfferUID = item.OfferUID;
                                        pp.ProductType = item.ProductType;
                                        pp.IsCommissionAdd = prodcalc.IsCommissionAdd;
                                        pp.IsDiscountAdd = prodcalc.IsDiscountAdd;
                                        pp.OfferProdQty = prodcalc.OfferProdQty;
                                        pp.IsAvailableForBilling = prodcalc.IsAvailableForBilling;
                                        pp.IsAvailableForOffer = prodcalc.IsAvailableForOffer;
                                        pp.StockAvailable = prodcalc.StockAvailable;
                                        pp.DispatchQty = item.DispatchQty;
                                        pp.RemainingQty = prodcalc.StockAvailable;
                                        pp.OrderQty = item.OrderQty;
                                        pp.Ispromobaluse = item.Ispromobaluse;
                                        pp.PartyGSTNo = item.PartyGSTNo;
                                        Returnlist.Add(pp);
                                    }

                                }
                            }
                        }



                    }

                }
            }
            catch (Exception ex)
            {

            }
            return Returnlist;
        }
        public List<ProductModel> GetOrderProductList(string OrderNo, string OrderBy)
        {
            List<ProductModel> objOrderProductModel = new List<ProductModel>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objOrderProductModel = (from r in entity.TrnPartyOrderDetails
                                            where r.ActiveStatus == "Y" && r.OrderNo == OrderNo && r.OrderBy == OrderBy && r.Status == "P"// && r.ProdType=="P"
                                            from p in entity.TrnPartyOrderMains
                                            where p.OrderNo == OrderNo
                                            select new ProductModel
                                            {
                                                ProductName = r.ProductName,
                                                Barcode = r.Barcode,
                                                BatchNo = r.BatchNo,
                                                DP = r.DP,
                                                RP = r.RP,
                                                DiscPer = r.DiscPer,
                                                DiscAmt = r.Discount,
                                                ProductCodeStr = r.ProductCode,
                                                TaxPer = r.Tax,
                                                TaxAmt = r.TaxAmount,
                                                MRP = r.MRP,
                                                BV = r.BV,
                                                PV = 0,
                                                CV = 0,
                                                Rate = r.Rate,
                                                Amount = r.Amount,
                                                TotalNetPayable = p.NetPayable,
                                                OrderedOty = r.RemQty == 0 ? r.Qty : r.RemQty,
                                                DispQty = r.DispatchQty,
                                                OfferUID = r.OfferUId,
                                                ProductType = r.ProdType
                                            }
                                          ).ToList();
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {

            }
            return objOrderProductModel;
        }

        public ResponseDetail SaveDispatchOrder(PartyOrderModel objPartyDispatchOrder)
        {
            List<TrnPayModeDetail> objDTListPayMode = new List<TrnPayModeDetail>();
            ResponseDetail objResponse = new ResponseDetail();
            TrnPartyOrderDetail objDTPartyOrderDetail = new TrnPartyOrderDetail();
            TrnPartyOrderMain objDtPartyOrderMain = new TrnPartyOrderMain();
            decimal maxUserSBillNo = 0;
            decimal? SessId = 0;
            string billPrefix = "";
            decimal maxSbillNo = 0;
            decimal? FsessId = 0;
            string UserBillNo = "";
            string version = "";
            string BillGSTType = "";
            string Billseries = "";
            objResponse.ResponseMessage = "Something went wrong!";
            objResponse.ResponseStatus = "FAILED";
            try
            {

                string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(AppConnectionString);
                SqlConnection SC1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString);
                string query = "Select Max(SessID) as MaxSessId from M_SessnMaster";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        SessId = decimal.Parse(reader["MaxSessId"].ToString());
                    }
                }
                using (var entity = new InventoryEntities())
                {
                    if (objPartyDispatchOrder.GstType == "G" && !string.IsNullOrEmpty(objPartyDispatchOrder.GSTNo))
                    {
                        BillGSTType = "BB.";
                    }
                    else if (objPartyDispatchOrder.GstType == "N" && !string.IsNullOrEmpty(objPartyDispatchOrder.GSTNo))
                    {
                        BillGSTType = "BB";
                    }
                    else if (objPartyDispatchOrder.GstType == "G" && string.IsNullOrEmpty(objPartyDispatchOrder.GSTNo))
                    {
                        BillGSTType = "BC.";
                    }
                    else if (objPartyDispatchOrder.GstType == "N" && string.IsNullOrEmpty(objPartyDispatchOrder.GSTNo))
                    {
                        BillGSTType = "BC";
                    }

                    maxSbillNo = (from result in entity.TrnBillMains select result.SBillNo).DefaultIfEmpty(0).Max();
                    maxSbillNo = maxSbillNo + 1;
                    FsessId = (from result in entity.M_FiscalMaster where result.ActiveStatus == "Y" select result.FSessId).DefaultIfEmpty(0).Max();
                    Billseries = Convert.ToString((from result in entity.M_FiscalMaster where result.ActiveStatus == "Y" select result.BillSeries).Max()).TrimEnd();
                    ////decimal? SessId = (from result in entity.M_SessnMaster select result.SessID).Max();

                    billPrefix = (from result in entity.M_ConfigMaster select result.BillPrefix).FirstOrDefault();

                    maxUserSBillNo = (from result in entity.TrnBillMains where result.FSessId == FsessId && result.SoldBy == objPartyDispatchOrder.LoginUser.PartyCode && result.BillType != "S" && result.BillGSTType == BillGSTType select result.UserSBillNo).DefaultIfEmpty(0).Max();
                    FsessId = (from result in entity.M_FiscalMaster where result.ActiveStatus == "Y" select result.FSessId).DefaultIfEmpty(0).Max();

                    var UserPCode = (from result in entity.M_LedgerMaster where result.ActiveStatus == "Y" && result.PartyCode == objPartyDispatchOrder.LoginUser.PartyCode select result.UserPartyCode).FirstOrDefault();

                    maxUserSBillNo = maxUserSBillNo + 1;
                    //maxUserSBillNo = maxUserSBillNo + 1;
                    string strMaxUserSBillNo = maxUserSBillNo.ToString();
                    if (strMaxUserSBillNo.Count() < 5)
                    {
                        var countNum = strMaxUserSBillNo.Count();
                        var ToBeAddedDigits = 5 - countNum;
                        for (var j = 0; j < ToBeAddedDigits; j++)
                        {
                            strMaxUserSBillNo = "0" + strMaxUserSBillNo;
                        }
                    }

                    //UserBillNo = billPrefix + "/" + UserPCode + "/" + strMaxUserSBillNo;
                    if (objPartyDispatchOrder.GstType == "G" && !string.IsNullOrEmpty(objPartyDispatchOrder.GSTNo))
                    {
                        UserBillNo = billPrefix + "/" + UserPCode + "/BB." + "/" + Billseries + "/" + strMaxUserSBillNo;
                    }
                    else if (objPartyDispatchOrder.GstType == "N" && !string.IsNullOrEmpty(objPartyDispatchOrder.GSTNo))
                    {
                        UserBillNo = billPrefix + "/" + UserPCode + "/BB" + "/" + Billseries + "/" + strMaxUserSBillNo;
                    }
                    else if (objPartyDispatchOrder.GstType == "G" && string.IsNullOrEmpty(objPartyDispatchOrder.GSTNo))
                    {
                        UserBillNo = billPrefix + "/" + UserPCode + "/BC." + "/" + Billseries + "/" + strMaxUserSBillNo;
                    }
                    else if (objPartyDispatchOrder.GstType == "N" && string.IsNullOrEmpty(objPartyDispatchOrder.GSTNo))
                    {
                        UserBillNo = billPrefix + "/" + UserPCode + "/BC" + "/" + Billseries + "/" + strMaxUserSBillNo;
                    }

                    version = (from result in entity.M_NewHOVersionInfo select result.VersionNo).FirstOrDefault();

                    DateTime BillDate = DateTime.Now.Date;

                    string SoldByCode = "";
                    List<TrnBillData> tempTableList = new List<TrnBillData>();
                    string GroupPrefix = "";
                    string ordertype = "";
                    var ordermethod = (from r in entity.TrnPartyOrderMains where r.OrderNo == objPartyDispatchOrder.OrderNo select r.OrderMethod).FirstOrDefault();
                    ordermethod = string.IsNullOrEmpty(ordermethod) ? "" : ordermethod;
                    string BillingPartyCode = objPartyDispatchOrder.PartyCode;
                    if (ordermethod == "")
                    { ordertype = "S"; }
                    else if (ordermethod.ToString().ToUpper() == "BV")
                    {
                        ordertype = "B";
                    }
                    else if (ordermethod.ToString().ToUpper() == "PV")
                    {
                        ordertype = "P";
                    }
                    GroupPrefix = (from p in entity.M_GroupMaster
                                   where p.GroupId == (from r in entity.M_LedgerMaster where r.PartyCode == BillingPartyCode select r.GroupId).FirstOrDefault()
                                   select p.Prefix
                                   ).FirstOrDefault();

                    try
                    {

                        string tempBillNo = "";
                        bool IsGSTcalc = false;
                        List<ProductModel> objListProductModel = new List<ProductModel>();
                        //TempDistributor.objListProduct.AddRange(objModel.objListProduct);
                        foreach (var obj in objPartyDispatchOrder.objListProduct)
                        {


                            TrnBillData objDTBillData = new TrnBillData();
                            if (obj.Quantity > 0)
                            {
                                objListProductModel.Add(obj);
                                objDTBillData.SBillNo = maxSbillNo;
                                objDTBillData.FSessId = FsessId ?? 0;
                                objDTBillData.SessId = SessId ?? 0;
                                objDTBillData.ActiveStatus = "Y";
                                objDTBillData.BillDate = BillDate.Date;

                                objDTBillData.RefNo = "";
                                objDTBillData.RefId = 0;
                                objDTBillData.RefName = "";
                                objDTBillData.Remarks = "";
                                objDTBillData.CType = GroupPrefix;
                                objDTBillData.SoldBy = objPartyDispatchOrder.LoginUser.PartyCode;
                                SoldByCode = objDTBillData.SoldBy;
                                objDTBillData.BillBy = objDTBillData.SoldBy;
                                //objDTBillData.BillNo = billPrefix + "/" + objDTBillData.BillBy + "/" + maxSbillNo;
                                if (objPartyDispatchOrder.GstType == "G" && !string.IsNullOrEmpty(objPartyDispatchOrder.GSTNo))
                                {
                                    tempBillNo = billPrefix + "/" + objDTBillData.BillBy + "/BB." + "/" + maxSbillNo;
                                    IsGSTcalc = true;
                                }
                                else if (objPartyDispatchOrder.GstType == "N" && !string.IsNullOrEmpty(objPartyDispatchOrder.GSTNo))
                                {
                                    tempBillNo = billPrefix + "/" + objDTBillData.BillBy + "/BB" + "/" + maxSbillNo;
                                }
                                else if (objPartyDispatchOrder.GstType == "G" && string.IsNullOrEmpty(objPartyDispatchOrder.GSTNo))
                                {
                                    tempBillNo = billPrefix + "/" + objDTBillData.BillBy + "/BC." + "/" + maxSbillNo;
                                    IsGSTcalc = true;
                                }
                                else if (objPartyDispatchOrder.GstType == "N" && string.IsNullOrEmpty(objPartyDispatchOrder.GSTNo))
                                {
                                    tempBillNo = billPrefix + "/" + objDTBillData.BillBy + "/BC" + "/" + maxSbillNo;
                                }
                                objDTBillData.BillNo = tempBillNo;
                                objDTBillData.FType = GroupPrefix;
                                objDTBillData.FCode = objPartyDispatchOrder.PartyCode;
                                objDTBillData.PartyName = objPartyDispatchOrder.PartyName;
                                objDTBillData.SupplierId = 0;
                                objDTBillData.ChDDNo = 0;
                                objDTBillData.ChDate = DateTime.Now;
                                objDTBillData.ChAmt = 0;
                                objDTBillData.BankCode = 0;
                                objDTBillData.BankName = "";
                                objDTBillData.FormNo = 0;
                                objDTBillData.TotalTaxAmount = objPartyDispatchOrder.objProduct.TotalTaxAmount;
                                objDTBillData.TotalSTaxAmount = 0;
                                objDTBillData.TotalDiscount = objPartyDispatchOrder.objProduct.TotalDiscount;
                                objDTBillData.TotalKitBvValue = 0;
                                if (ordermethod.ToString().ToUpper() == "BV")
                                {
                                    objDTBillData.TotalBvValue = objPartyDispatchOrder.objProduct.TotalBV;
                                    objDTBillData.TotalPVValue = 0;
                                    objDTBillData.PV = 0;
                                    objDTBillData.BV = obj.BV ?? 0;
                                    objDTBillData.BVValue = obj.BVValue ?? 0;
                                    objDTBillData.PVValue = 0;
                                }
                                else if (ordermethod.ToString().ToUpper() == "PV")
                                {
                                    objDTBillData.TotalBvValue = 0;
                                    objDTBillData.TotalPVValue = objPartyDispatchOrder.objProduct.TotalPV;
                                    objDTBillData.PV = obj.PV ?? 0;
                                    objDTBillData.BV = 0;
                                    objDTBillData.BVValue = 0;
                                    objDTBillData.PVValue = obj.PVValue ?? 0;
                                }
                                else
                                {
                                    objDTBillData.TotalBvValue = objPartyDispatchOrder.objProduct.TotalBV;
                                    objDTBillData.TotalPVValue = objPartyDispatchOrder.objProduct.TotalPV;
                                    objDTBillData.PV = obj.PV ?? 0;
                                    objDTBillData.BV = obj.BV ?? 0;
                                    objDTBillData.BVValue = obj.BVValue ?? 0;
                                    objDTBillData.PVValue = obj.PVValue ?? 0;
                                }

                                objDTBillData.TotalCVValue = objPartyDispatchOrder.objProduct.TotalCV;
                                objDTBillData.TotalRPValue = objPartyDispatchOrder.objProduct.TotalRP;
                                objDTBillData.DP = obj.DP ?? 0;
                                objDTBillData.RP = obj.RP ?? 0;
                                objDTBillData.MRP = obj.MRP ?? 0;
                                objDTBillData.CVValue = obj.CVValue ?? 0;
                                objDTBillData.CV = obj.CV ?? 0;
                                objDTBillData.RPValue = obj.RPValue ?? 0;
                                objDTBillData.Barcode = obj.Barcode.ToString();
                                objDTBillData.BatchNo = obj.BatchNo.ToString();
                                objDTBillData.DiscountPer = obj.DiscPer ?? 0;
                                objDTBillData.Discount = obj.DiscAmt ?? 0;
                                objDTBillData.ProdCommssn = obj.CommissionPer ?? 0;
                                objDTBillData.ProdCommssnAmt = obj.CommissionAmt ?? 0;
                                objDTBillData.ProductId = obj.ProdCode.ToString();
                                objDTBillData.ProductName = obj.ProductName;
                                objDTBillData.Qty = obj.Quantity;
                                objDTBillData.Rate = obj.Rate ?? 0;
                                objDTBillData.IsKitBV = "N";
                                objDTBillData.DSeries = "";
                                objDTBillData.DImported = "N";
                                objDTBillData.IMEINo = "D";
                                objDTBillData.BNo = "";
                                objDTBillData.ItemType = "N";
                                objDTBillData.JType = "Cash:" + objPartyDispatchOrder.objProduct.TotalNetPayable;
                                objDTBillData.BillTo = objPartyDispatchOrder.PartyCode;
                                objDTBillData.BillFor = objPartyDispatchOrder.PartyCode;
                                objDTBillData.IsReceive = "N";
                                objDTBillData.IsCredit = "F";
                                //objDTBillData.BillType = "R";
                                objDTBillData.BillType = "V";
                                objDTBillData.ProdType = obj.ProductType;
                                objDTBillData.PaymentDtl = "Cash:" + objPartyDispatchOrder.objProduct.TotalNetPayable;

                                objDTBillData.TotalAmount = objPartyDispatchOrder.objProduct.TotalTotalAmount;
                                //tax excluding
                                objDTBillData.NetAmount = obj.Amount;
                                var PartyStateCode = (from r in entity.M_LedgerMaster where r.PartyCode == objPartyDispatchOrder.PartyCode select r.StateCode).FirstOrDefault();
                                if (IsGSTcalc)
                                {
                                    if (PartyStateCode == objPartyDispatchOrder.LoginUser.StateCode)
                                    {
                                        objDTBillData.TaxAmount = 0;
                                        objDTBillData.Tax = 0;
                                        objDTBillData.CGST = obj.TaxPer / 2 ?? 0;
                                        objDTBillData.CGSTAmt = obj.TaxAmt / 2 ?? 0;
                                        objDTBillData.SGST = obj.TaxPer / 2 ?? 0;
                                        objDTBillData.SGSTAmt = obj.TaxAmt / 2 ?? 0;
                                        objDTBillData.TaxType = "S";
                                    }
                                    else
                                    {

                                        objDTBillData.TaxAmount = obj.TaxAmt ?? 0;
                                        if (obj.OldTaxAmount != 0 && obj.OldTaxAmount != obj.TaxAmt)
                                        {
                                            objDTBillData.TaxAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.TaxAmount) + 0.01).ToString());
                                            objDTBillData.NetAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.NetAmount) - 0.01).ToString());
                                        }
                                        objDTBillData.Tax = obj.TaxPer ?? 0;
                                        objDTBillData.CGST = 0;
                                        objDTBillData.CGSTAmt = 0;
                                        objDTBillData.SGST = 0;
                                        objDTBillData.SGSTAmt = 0;
                                        objDTBillData.TaxType = "I";
                                    }
                                }
                                else
                                {
                                    objDTBillData.TaxAmount = 0;
                                    objDTBillData.Tax = 0;
                                    objDTBillData.CGST = 0;
                                    objDTBillData.CGSTAmt = 0;
                                    objDTBillData.SGST = 0;
                                    objDTBillData.SGSTAmt = 0;
                                    objDTBillData.TaxType = "N";
                                }

                                objDTBillData.CashDiscPer = obj.CashDiscPer;
                                objDTBillData.CashDiscAmount = obj.CashDiscAmount;

                                objDTBillData.NetPayable = objPartyDispatchOrder.objProduct.TotalNetPayable;
                                objDTBillData.RndOff = objPartyDispatchOrder.objProduct.Roundoff;
                                objDTBillData.CardAmount = 0;
                                objDTBillData.PayMode = "Cash";
                                objDTBillData.PayPrefix = "";
                                objDTBillData.BvTransfer = "N";

                                //objDTBillData.UserSBillNo = maxSbillNo;
                                //objDTBillData.UserBillNo = billPrefix + "/" + objDTBillData.BillBy + "/" + maxSbillNo;
                                objDTBillData.UserSBillNo = maxUserSBillNo;
                                objDTBillData.UserBillNo = UserBillNo;
                                objDTBillData.DispatchStatus = "N";
                                objDTBillData.LR = "0";
                                objDTBillData.LRDate = DateTime.Now;
                                objDTBillData.TransporterName = "";
                                objDTBillData.DispatchTo = "";
                                objDTBillData.FreightType = "";
                                objDTBillData.Series = "";
                                objDTBillData.Scratch = "";

                                objDTBillData.Unit = 0;

                                objDTBillData.PSessId = 0;
                                objDTBillData.DcNo = "";
                                objDTBillData.Imported = "N";
                                objDTBillData.FPoint = 0;
                                objDTBillData.FPointValue = 0;
                                objDTBillData.OrdStatus = "";
                                objDTBillData.OrdQty = 0;
                                // objDTBillData.OrderType = "";
                                objDTBillData.OrderDate = DateTime.Now;
                                objDTBillData.OrderNo = objPartyDispatchOrder.OrderNo;
                                objDTBillData.RemQty = 0;
                                objDTBillData.DP1 = 0;
                                objDTBillData.DReason = "";
                                objDTBillData.DUserId = 0;
                                objDTBillData.DRecTimeStamp = DateTime.Now;
                                objDTBillData.DocWeight = 0;
                                objDTBillData.DocketNo = "";
                                objDTBillData.DocketDate = DateTime.Now;
                                //objDTBillData.UserBillNo = "";
                                //objDTBillData.UserSBillNo = 0;
                                objDTBillData.STNFormNo = "";
                                objDTBillData.StkRecv = "N";
                                objDTBillData.StkRecvDate = DateTime.Now;
                                objDTBillData.StkRecvUserId = 0;
                                objDTBillData.InTransit = "N";
                                objDTBillData.UID = string.IsNullOrEmpty(objPartyDispatchOrder.objProduct.UID) ? "" : objPartyDispatchOrder.objProduct.UID;
                                objDTBillData.OfferUID = obj.OfferUID;
                                objDTBillData.IsKit = "N";
                                objDTBillData.TotalCorton = "";
                                objDTBillData.TotalMonoCorton = "";
                                objDTBillData.SpclOfferId = Convert.ToInt32(obj.OfferUID);
                                objDTBillData.VAT = 0;
                                objDTBillData.BuyerAddress = "";
                                objDTBillData.BuyerTIN = "";

                                objDTBillData.TotalDiscount = objPartyDispatchOrder.objProduct.TotalDiscPer;
                                objDTBillData.TotalDiscountAmt = objPartyDispatchOrder.objProduct.TotalDiscount;
                                objDTBillData.VDiscountAmt = 0;
                                objDTBillData.VDiscount = 0;
                                objDTBillData.ReceiverID = "";
                                objDTBillData.ReceiverName = "";
                                objDTBillData.ReceiverMNo = "";
                                objDTBillData.ReceiverIDProof = "";
                                objDTBillData.TotalFPoint = 0;
                                objDTBillData.TotalQty = objPartyDispatchOrder.objProduct.TotalQty;
                                objDTBillData.CashReward = 0;
                                objDTBillData.CommssnAmt = objPartyDispatchOrder.objProduct.TotalCommsonAmt;
                                objDTBillData.RecvAmount = 0;
                                objDTBillData.ReturnToCustAmt = 0;
                                objDTBillData.ActiveStatus = "Y";
                                objDTBillData.RecTimeStamp = DateTime.Now;
                                objDTBillData.UserId = objPartyDispatchOrder.LoginUser.UserId;
                                objDTBillData.UserName = objPartyDispatchOrder.LoginUser.UserName;
                                objDTBillData.DelvPlace = string.IsNullOrEmpty(objPartyDispatchOrder.objProduct.DeliveryPlace) ? "" : objPartyDispatchOrder.objProduct.DeliveryPlace;
                                objDTBillData.DelvStatus = "";
                                objDTBillData.DelvUserId = 0;
                                objDTBillData.DelvRecTimeStamp = DateTime.Now;
                                objDTBillData.Version = version;
                                objDTBillData.IDType = "";
                                objDTBillData.BranchName = "";
                                objDTBillData.CourierId = 0;
                                objDTBillData.CourierName = "";
                                objDTBillData.LocId = 0;
                                objDTBillData.LocName = "";
                                objDTBillData.DelvAddress = "";
                                objDTBillData.Pincode = "";
                                objDTBillData.OrderType = ordertype;
                                objDTBillData.Coupon = "";
                                objDTBillData.IRNNo = "";
                                objDTBillData.AckNo = "";
                                objDTBillData.AckDate = DateTime.Now;
                                objDTBillData.QrCodeimage = "";
                                objDTBillData.QrCode = "";
                                objDTBillData.SignedInvoice = "";
                                objDTBillData.BillGSTType = BillGSTType;
                                entity.TrnBillDatas.Add(objDTBillData);

                                //updating entries in trnpartyorderdetails
                                //objDTPartyOrderDetail = (from r in entity.TrnPartyOrderDetails
                                //                         where r.ProductCode == objDTBillData.ProductId && r.OrderNo == objPartyDispatchOrder.OrderNo
                                //                         select r
                                //                       ).FirstOrDefault();
                                //if (objDTPartyOrderDetail != null)
                                //{

                                //    objDTPartyOrderDetail.Status = "D";

                                //    objDTPartyOrderDetail.RemQty = objDTPartyOrderDetail.Qty - obj.Quantity;
                                //    objDTPartyOrderDetail.DispatchQty = obj.Quantity;

                                //}
                                //objDtPartyOrderMain = (from r in entity.TrnPartyOrderMains
                                //                       where r.OrderNo == objPartyDispatchOrder.OrderNo
                                //                       select r
                                //                       ).FirstOrDefault();
                                //if (objDtPartyOrderMain != null)
                                //{
                                //    objDtPartyOrderMain.Status = "D";
                                //    objDtPartyOrderMain.BillNo = UserBillNo;
                                //    objDtPartyOrderMain.BillDate = DateTime.Now.Date;

                                //}
                            }
                        }
                        int i = 0;



                        try
                        {
                            i = entity.SaveChanges();

                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                        ve.PropertyName, ve.ErrorMessage);
                                }
                            }
                            throw;
                        }
                        catch (Exception ex)
                        {

                        }

                        if (i > 0)
                        {
                            if (SC1.State == ConnectionState.Closed)
                                SC1.Open();

                            query = "Exec SaveFranchiseLimit '" + (objPartyDispatchOrder.PartyCode) + "','" + (FsessId) + "','" + (SessId) + "','" + (objPartyDispatchOrder.OrderNo) + "';";
                            cmd = new SqlCommand();
                            cmd.CommandText = query;
                            cmd.Connection = SC1;
                            int j = cmd.ExecuteNonQuery();
                            SC1.Close();
                            string Sql = "Exec Sp_UpdatePartyDispatchOrder '" + objPartyDispatchOrder.OrderNo + "','" + objPartyDispatchOrder.OrderMethod + "' ";
                            //string Sql = "Update TrnPartyOrderDetail Set DispatchQty=a.DispQty,DispatchAmt=a.DispAmt,Discount=a.DiscountAmt";
                            //Sql = Sql + " FROM (";
                            //Sql = Sql + " Select a.FSessId,a.OrderNo,b.ProductId,b.ProdType,b.OfferUId,IsNull(SUM(b.Discount),0) as DiscountAmt,IsNull(SUM(b.Qty),0) as DispQty,IsNull(SUM(b.TaxAmount),0)+IsNull(SUM(b.NetAmount),0) as DispAmt";
                            //Sql = Sql + " FROM TrnBillMain as a,TrnBillDetails as b Where a.FSessId=b.FSessId And a.BillNo=b.BillNo And a.OrderNo='" + objPartyDispatchOrder.OrderNo + "'";
                            //Sql = Sql + " Group BY a.FSessId,a.OrderNo,b.ProductId,b.ProdType,b.OfferUId) as a,TrnPartyOrderDetail as b";
                            //Sql = Sql + " Where a.OrderNo=b.OrderNo And a.ProductId=b.ProductCode And a.ProdType=b.ProdType AND a.OfferUId=b.OfferUId";
                            //Sql = Sql + " ;Update TrnPartyOrderDetail Set RemQty=Qty-DispatchQty Where OrderNo='" + objPartyDispatchOrder.OrderNo + "' AND ActiveStatus='Y'";
                            //Sql = Sql + " ;Update TrnPartyOrderDetail Set Status=Case When RemQty<=0 Then 'C' Else 'P' End Where OrderNo='" + objPartyDispatchOrder.OrderNo + "' AND ActiveStatus='Y'";
                            //Sql = Sql + " ;Update TrnPartyOrderMain Set TotalDiscount=a.TotalDiscount,TotalDispQty=a.TotalDispQty,TotalAmount=a.TotalAmount,";
                            //Sql = Sql + " TotalTaxAmt=a.TotalTaxAmt,RndOff=Round(a.TotalAmount+a.TotalTaxAmt,0)-Round(a.TotalAmount+a.TotalTaxAmt,2),NetPayable=Round(a.NetPayable,0)";
                            //Sql = Sql + " FROM (";
                            //Sql = Sql + " Select FSessId,OrderNo,IsNull(SUM(Discount),0) as TotalDiscount,IsNull(SUM(TotalQty),0) as TotalDispQty,";
                            //Sql = Sql + " IsNull(SUM(Amount),0) as TotalAmount,";
                            //Sql = Sql + " IsNull(SUM(TaxAmount),0)+IsNull(SUM(STaxAmount),0) as TotalTaxAmt,";
                            //Sql = Sql + " IsNull(SUM(NetPayable),0) as NetPayable";
                            //Sql = Sql + " FROM TrnBillMain Where OrderType='S'";
                            //Sql = Sql + " Group By FSessId,OrderNo) as a,TrnPartyOrderMain as b ";
                            //Sql = Sql + " Where a.OrderNo=b.OrderNo And b.OrderNo='" + objPartyDispatchOrder.OrderNo + "'";
                            //Sql = Sql + " ;Update TrnPartyOrderMain Set TotalRemQty=TotalOrdQty-TotalDispQty Where OrderNo='" + objPartyDispatchOrder.OrderNo + "' AND ActiveStatus='Y'";
                            //Sql = Sql + " ;Update TrnPartyOrderMain Set Status=Case When TotalRemQty<=0 Then 'C' Else 'P' End Where OrderNo='" + objPartyDispatchOrder.OrderNo + "' AND ActiveStatus='Y'";
                            //Sql = Sql + ";update TrnBillMain set OrderMethod='" + objPartyDispatchOrder.OrderMethod + "' where OrderNo='" + objPartyDispatchOrder.OrderNo + "'";
                            if (SC1.State == ConnectionState.Closed)
                                SC1.Open();
                            cmd = new SqlCommand();
                            cmd.CommandText = Sql;
                            cmd.Connection = SC1;
                            i = cmd.ExecuteNonQuery();
                            SC1.Close();

                            try
                            {
                                if (ordermethod.ToString().ToUpper() == "BV")
                                {
                                    objDTListPayMode.Add(new TrnPayModeDetail
                                    {
                                        BillAmt = objPartyDispatchOrder.objProduct.TotalNetPayable,
                                        SoldBy = objPartyDispatchOrder.LoginUser.PartyCode,
                                        BillDate = BillDate.Date,
                                        BillType = "V",
                                        BillNo = tempBillNo,
                                        PayPrefix = "BW",
                                        Amount = objPartyDispatchOrder.objProduct.TotalNetPayable,
                                        CardNo = "",
                                        AcNo = "",
                                        IFSCode = "",
                                        BankCode = 0,
                                        DUserId = 0,
                                        DRecTimeStamp = null,
                                        ChqDDDate = null,
                                        ChqDDNo = "",
                                        Narration = "",
                                        BankName = "",
                                        ActiveStatus = "Y",
                                        RecTimeStamp = DateTime.Now,
                                        UserId = objPartyDispatchOrder.LoginUser.UserId,
                                        Version = version,
                                        UserName = objPartyDispatchOrder.LoginUser.UserName,
                                        FSessId = FsessId ?? 0,
                                        SBillNo = maxSbillNo,
                                        PayMode = "BV Purchase Wallet"
                                    });
                                }

                                if (ordermethod.ToString().ToUpper() == "PV")
                                {
                                    objDTListPayMode.Add(new TrnPayModeDetail
                                    {
                                        BillAmt = objPartyDispatchOrder.objProduct.TotalNetPayable,
                                        SoldBy = objPartyDispatchOrder.LoginUser.PartyCode,
                                        BillDate = BillDate.Date,
                                        BillType = "V",
                                        BillNo = tempBillNo,
                                        PayPrefix = "PW",
                                        Amount = objPartyDispatchOrder.objProduct.TotalNetPayable,
                                        CardNo = "",
                                        AcNo = "",
                                        IFSCode = "",
                                        BankCode = 0,
                                        DUserId = 0,
                                        DRecTimeStamp = null,
                                        ChqDDDate = null,
                                        ChqDDNo = "",
                                        Narration = "",
                                        BankName = "",
                                        ActiveStatus = "Y",
                                        RecTimeStamp = DateTime.Now,
                                        UserId = objPartyDispatchOrder.LoginUser.UserId,
                                        Version = version,
                                        UserName = objPartyDispatchOrder.LoginUser.UserName,
                                        FSessId = FsessId ?? 0,
                                        SBillNo = maxSbillNo,
                                        PayMode = "PV Purchase Wallet"
                                    });
                                }


                                var DiscAmt = Convert.ToDecimal(objPartyDispatchOrder.objListProduct.Sum(p => p.DiscAmt));
                                if (DiscAmt > 0)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Voucher;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    objDTListPayMode.Add(new TrnPayModeDetail
                                    {
                                        BillAmt = objPartyDispatchOrder.objProduct.TotalNetPayable,
                                        SoldBy = objPartyDispatchOrder.LoginUser.PartyCode,
                                        BillDate = DateTime.Now.Date,
                                        BillType = "V",
                                        BillNo = tempBillNo,
                                        PayPrefix = value,
                                        Amount = Convert.ToDecimal(DiscAmt),
                                        CardNo = "",
                                        AcNo = "",
                                        IFSCode = "",
                                        BankCode = 0,
                                        DUserId = 0,
                                        DRecTimeStamp = null,
                                        ChqDDDate = null,
                                        ChqDDNo = "",
                                        Narration = "",
                                        BankName = "",
                                        ActiveStatus = "Y",
                                        RecTimeStamp = DateTime.Now,
                                        UserId = objPartyDispatchOrder.LoginUser.UserId,
                                        Version = version,
                                        UserName = objPartyDispatchOrder.LoginUser.UserName,
                                        FSessId = FsessId ?? 0,
                                        SBillNo = maxSbillNo,
                                        PayMode = "Voucher"
                                    });
                                }

                                foreach (var obj in objDTListPayMode)
                                {
                                    TrnPayModeDetail objTemp = new TrnPayModeDetail();
                                    objTemp = obj;
                                    if (string.IsNullOrEmpty(objTemp.CardNo))
                                    {
                                        objTemp.CardNo = "";
                                    }
                                    entity.TrnPayModeDetails.Add(objTemp);
                                }
                                i = 0;
                                i = entity.SaveChanges();
                            }
                            catch
                            {

                            }

                            objResponse.ResponseMessage = "Saved Successfully!";
                            objResponse.ResponseStatus = "OK";
                        }
                        else
                        {
                            objResponse.ResponseMessage = "Something went wrong!";
                            objResponse.ResponseStatus = "FAILED";
                        }
                    }

                    catch (DbEntityValidationException e)
                    {
                        objResponse.ResponseMessage = "Something went wrong!";
                        objResponse.ResponseStatus = "FAILED";
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return objResponse;
        }
        public List<ProductModel> GetOrderProduct(string OrderNo, string CurrentPartyCode)
        {
            List<ProductModel> objOrderProduct = new List<ProductModel>();
            List<ProductModel> objTempOrderProduct = new List<ProductModel>();
            try
            {
                string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(AppConnectionString);

                decimal Orderno = 0;
                if (!string.IsNullOrEmpty(OrderNo))
                {
                    Orderno = decimal.Parse(OrderNo);
                }
                string query = "Select * from TrnOrderDetail where ProdType='P' AND OrderNo=" + OrderNo;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                //cmd.Parameters.AddWithValue("@OrdeNo", Orderno);
                cmd.Connection = SC;
                SC.Close();
                SC.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        objTempOrderProduct.Add(new ProductModel
                        {
                            ProdCode = int.Parse(reader["ProductID"].ToString()),
                            ProductCodeStr = reader["ProductID"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            Quantity = decimal.Parse(reader["Qty"].ToString()),
                            StockAvailable = 0,


                        });
                    }
                }
                using (var entity = new InventoryEntities())
                {
                    foreach (var obj in objTempOrderProduct)
                    {
                        obj.StockAvailable = (from stockAvail in entity.Im_CurrentStock
                                              where stockAvail.ProdId == obj.ProductCodeStr && stockAvail.FCode.Equals(CurrentPartyCode)
                                              select stockAvail.Qty
                                                     ).DefaultIfEmpty(0).Sum();
                        objOrderProduct.Add(obj);
                    }
                }


            }
            catch (Exception ex)
            {

            }
            return objOrderProduct;
        }
        public ResponseDetail RejectOrder(string OrderNo, string RejectReason, decimal RejectedByUserId)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseMessage = "Something went wrong!";
            objResponse.ResponseStatus = "FAILED";
            try
            {

                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConnectionString);

                SqlCommand cmd = new SqlCommand();
                string Query = "Exec REJECTORDER '" + decimal.Parse(OrderNo.Trim()) + "','" + (RejectReason.Trim()) + " :Web Inv','" + RejectedByUserId + "'";
                cmd.CommandText = Query;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();

                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    objResponse.ResponseMessage = "Rejected Successfully!";
                    objResponse.ResponseStatus = "OK";
                }
                else
                {
                    objResponse.ResponseMessage = "Something went wrong!";
                    objResponse.ResponseStatus = "FAILED";
                }


            }
            catch (Exception ex)
            {

            }
            return objResponse;
        }



        public List<DisptachOrderModel> GetDispatchOrderList(string FromDate, string ToDate, string PartyCode, string ViewType, string IdNo, string OrderNo, string DispMode)
        {
            List<DisptachOrderModel> objOrderList = new List<DisptachOrderModel>();
            try
            {
                DateTime StartDate = DateTime.Now;
                DateTime EndDate = DateTime.Now;

                string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(AppConnectionString);

                using (var entity = new InventoryEntities())
                {
                    string WhereCondition = "";
                    string DispStatus = "N";
                    if (!string.IsNullOrEmpty(ViewType))
                    {
                        if (ViewType == "Pending")
                        {
                            DispStatus = "N";
                        }
                        else
                        {
                            DispStatus = "C";
                        }
                    }
                    if (!string.IsNullOrEmpty(PartyCode) && PartyCode != "0" && PartyCode != "All")
                    {
                        WhereCondition = WhereCondition + "AND a.OrderFor='" + PartyCode + "'";
                    }
                    else
                    {
                        WhereCondition = WhereCondition + "";
                    }
                    if (!string.IsNullOrEmpty(IdNo) && IdNo != "0" && IdNo != "All")
                    {
                        WhereCondition = WhereCondition + "AND C.IDNo='" + IdNo + "'";
                    }
                    else
                    {
                        WhereCondition = WhereCondition + "";
                    }
                    if (!string.IsNullOrEmpty(OrderNo) && OrderNo != "0" && OrderNo != "All")
                    {
                        WhereCondition = WhereCondition + "AND a.OrderNo='" + OrderNo + "'";
                    }
                    else
                    {
                        WhereCondition = WhereCondition + "";
                    }
                    if (!string.IsNullOrEmpty(DispMode) && DispMode != "0" && DispMode != "All" && DispMode != "A")
                    {
                        WhereCondition = WhereCondition + "AND a.HostIP='" + DispMode + "'";
                    }


                    if (!string.IsNullOrEmpty(FromDate) && (!string.IsNullOrEmpty(ToDate)))
                    {
                        if (FromDate != "All")
                        {
                            var sqlFromdate = FromDate.Split('-');
                            StartDate = new DateTime(Convert.ToInt16(sqlFromdate[2]), Convert.ToInt16(sqlFromdate[1]), Convert.ToInt16(sqlFromdate[0]));
                            //var SplitFromDate = FromDate.Split('-');
                            //FromDate = SplitFromDate[1] + "-" + SplitFromDate[0] + "-" + SplitFromDate[2];
                            //StartDate = Convert.ToDateTime(FromDate);
                        }
                        if (ToDate != "All")
                        {
                            var sqlFromdate = ToDate.Split('-');
                            EndDate = new DateTime(Convert.ToInt16(sqlFromdate[2]), Convert.ToInt16(sqlFromdate[1]), Convert.ToInt16(sqlFromdate[0]));
                            //var SplitToDate = ToDate.Split('-');
                            //ToDate = SplitToDate[1] + "-" + SplitToDate[0] + "-" + SplitToDate[2];
                            //EndDate = Convert.ToDateTime(ToDate);
                        }


                    }
                    string NewFromDate = StartDate.ToString("dd-MMM-yyyy");
                    string NewToDate = EndDate.ToString("dd-MMM-yyyy");

                    if (FromDate != "All" && ToDate != "All")
                    {
                        WhereCondition += " AND CONVERT(date,a.OrderDate) >= CONVERT(date,'" + NewFromDate + "')";
                        WhereCondition += " AND CONVERT(date,a.OrderDate) <= CONVERT(date,'" + NewToDate + "')";
                    }
                    else if (FromDate != "All" && ToDate == "All")
                    {
                        WhereCondition += " AND CONVERT(date,a.OrderDate) >= CONVERT(date,'" + NewFromDate + "')";
                    }
                    else if (FromDate == "All" && ToDate != "All")
                    {
                        WhereCondition += " AND CONVERT(date,a.OrderDate) <= CONVERT(date,'" + NewToDate + "')";
                    }
                    else
                    {

                    }
                    //string NewFromDate = StartDate.ToString("dd-MMM-yyyy");
                    //string NewToDate = EndDate.AddDays(1).ToString("dd-MMM-yyyy");
                    //if (FromDate != "All" && ToDate != "All")
                    //{
                    //    WhereCondition = WhereCondition +" and a.OrderDate>='" + NewFromDate + "' and a.OrderDate<'" + NewToDate + "'";
                    //    //WhereCondition = WhereCondition + " and a.OrderDate>='" + NewFromDate + "' and a.OrderDate<='" + NewToDate + "'";
                    //}
                    //else if (FromDate != "All" && ToDate == "All")
                    //{
                    //    WhereCondition = WhereCondition + " and a.OrderDate>='" + NewFromDate + "'";
                    //    //WhereCondition = WhereCondition + " and a.OrderDate>='" + NewFromDate + "'";
                    //}
                    //else if (FromDate == "All" && ToDate != "All")
                    //{
                    //    WhereCondition = WhereCondition + " and a.OrderDate<'" + NewToDate + "'";
                    //    //WhereCondition = WhereCondition + "and a.OrderDate<='" + NewToDate + "'";
                    //}
                    //else
                    //{

                    //}

                    string db = System.Configuration.ConfigurationManager.AppSettings["Database"];
                    string dbInv = System.Configuration.ConfigurationManager.AppSettings["INVDatabase"];


                    string query = "Select  0 as Blank,a.OrderNo,Replace(Convert(varchar,a.OrderDate,106),' ','-') as ODte,a.IDNo,a.MemFirstName + ' ' + a.MemLastName as MemName,a.OrderAmt,a.Remark,a.Address1,a.Pincode,a.Mobl ,a.OrderType ,CASE WHEN a.OrderTYpe='T' THEN 'Activation' ELSE 'Product Request' END AS OType,CASE WHEN a.HostIP='C' THEN 'By Courier' WHEN a.HostIP='S' THEN 'By Speed Post' WHEN a.HostIP='H' THEN 'By Hand - '+ b.PartyName ELSE b.PartyName END As OrdFor,Case WHEN a.DispatchStatus='N' AND a.DispatchQty=0 THEN 'Reject' ELSE '' END as Reject  From " + db + "..TrnOrder a," + dbInv + "..M_LedgerMaster b," + db + "..m_membermaster C Where a.OrderFor=b.PartyCode AND a.ActiveStatus='Y'and a.formno=c.formno AND a.DispatchStatus='" + DispStatus + "' " + WhereCondition +
              " Order By OrderDate";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;

                    cmd.Connection = SC;
                    SC.Close();
                    SC.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            objOrderList.Add(new DisptachOrderModel
                            {
                                OrderNo = !string.IsNullOrEmpty(reader["OrderNo"].ToString()) ? decimal.Parse(reader["OrderNo"].ToString()) : 0,
                                OrderDateStr = reader["ODte"].ToString(),
                                OrderDate = DateTime.Parse(reader["ODte"].ToString()),
                                IdNo = reader["IDNo"].ToString(),
                                Name = reader["MemName"].ToString(),
                                OrderAmount = decimal.Parse(reader["OrderAmt"].ToString()),
                                Remarks = reader["Remark"].ToString(),
                                Address = reader["Address1"].ToString(),
                                Pincode = decimal.Parse(reader["Pincode"].ToString()),
                                Mobile = decimal.Parse(reader["Mobl"].ToString()),
                                OrderType = reader["OrderType"].ToString(),
                                DispBy = reader["OrdFor"].ToString(),
                                SoldBy = "",
                                IsDispatched = false,
                                Reject = reader["Reject"].ToString(),
                            });
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ErrorLog.txt");

                System.IO.File.AppendAllText(
                    path,
                    DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") +
                    " => " + ex.ToString() + Environment.NewLine
                );
            }
            return objOrderList;
        }

        public ResponseDetail SaveDispatchOrderdetails(List<DisptachOrderModel> objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseMessage = "Something went wrong!";

            objResponse.ResponseStatus = "FAILED";
            try
            {
                List<string> ProdIds = new List<string>();
                List<string> OrderIds = new List<string>();
                bool FlagIsSave = false;
                bool FlagToDispatch = true;
                foreach (var obj in objModel)
                {
                    ProdIds = new List<string>();
                    FlagToDispatch = true;
                    //using (var entity=new InventoryEntities())
                    //{
                    if (obj.IsDispatched)
                    {
                        List<ProductModel> objProductList = GetOrderProduct(obj.OrderNo.ToString(), obj.SoldBy);
                        if (objProductList.Count() == 0)
                        {
                            FlagToDispatch = false;
                        }
                        else
                        {
                            foreach (var objProduct in objProductList)
                            {
                                if (objProduct.StockAvailable < objProduct.Quantity)
                                {
                                    ProdIds.Add(objProduct.ProductCodeStr);

                                    FlagToDispatch = false;
                                }
                                else
                                {

                                }
                            }
                        }

                        //dispatch code
                        if (FlagToDispatch)
                        {

                            string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                            SqlConnection SC = new SqlConnection(InvConnectionString);
                            string sqlQry = "Exec DispatchOrder '" + obj.OrderNo + "','" + obj.SoldBy + "';";


                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandText = sqlQry;

                            cmd.Connection = SC;


                            SC.Close();
                            SC.Open();
                            int i = cmd.ExecuteNonQuery();
                            if (i > 0)
                            {
                                FlagIsSave = true;

                            }
                            else
                            {
                                FlagIsSave = false;
                                objResponse.ResponseMessage = "Something went wrong!";
                                objResponse.ResponseStatus = "FAILED";
                            }


                        }
                        else
                        {
                            OrderIds.Add(obj.OrderNo.ToString() + " can't be dispatched.Stock of products " + string.Join(",", ProdIds) + " is not available.");
                        }
                    }
                    else
                    {

                    }
                    //if (FlagIsSave)
                    //{
                    //    objResponse.ResponseMessage = "Dispatched Successfully!";

                    //   objResponse.ResponseStatus = "OK";
                    //}
                    //}


                }

                if (OrderIds.Count() > 0)
                {
                    objResponse.ResponseMessage = string.Join(";", OrderIds);

                    objResponse.ResponseStatus = "FAILED";
                }
                else
                {
                    objResponse.ResponseMessage = "Orders Dispatched Successfully!";

                    objResponse.ResponseStatus = "OK";
                }
            }
            catch (Exception ex)
            {

            }

            return objResponse;
        }
        public ResponseDetail RejectFranchiseOrder(string OrderNo, string RejectReason, decimal RejectedByUserId)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseMessage = "Something went wrong!";
            objResponse.ResponseStatus = "FAILED";
            TrnPartyOrderMain objPartyOrderMain = new TrnPartyOrderMain();
            try
            {
                string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(AppConnectionString);
                string Sql = "Select CASE WHEN a.OrderAmount-b.NetPayable<TotalWeight THEN a.OrderAmount-b.NetPayable ELSE TotalWeight END AS WalletAmt,a.OrderBy,a.OrderTo ";
                Sql = Sql + " FROM TrnPartyOrderMain a LEFT JOIN (Select OrderNo, SUM(NetPayable) NetPayable FROM TrnBillMain WHERE OrderNo='" + OrderNo + "' GROUP BY OrderNo) b  ";
                Sql = Sql + " On a.OrderNo=b.OrderNo AND a.ActiveStatus='Y' WHERE a.OrderNo='" + OrderNo + "'";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = Sql;
                cmd.Connection = SC;
                if (SC.State == ConnectionState.Closed)
                    SC.Open();
                Sql = "";
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        decimal WalletAmt = Convert.ToDecimal(reader["WalletAmt"].ToString());
                        string OrderTo = reader["OrderTo"].ToString();
                        string OrderBy = reader["OrderBy"].ToString();
                        string db = System.Configuration.ConfigurationManager.AppSettings["Database"];
                        if (WalletAmt > 0)
                        {
                            Sql = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) ";
                            Sql = Sql + " Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + OrderTo + "','" + OrderBy + "','" + WalletAmt + "','Order " + OrderNo + " generated for product.','" + (OrderNo) + "','R','O','Order Generated',(Select Max(SessID) FROM " + db + "..M_SessnMaster),(Select Max(FSessID) FROM M_FiscalMaster) FROM TrnVoucher ;";
                        }
                    }
                }
                Sql = Sql + "UPDATE TrnPartyOrderMain SET Status='C',ActiveStatus='D',OrderNo= 'Del:'+ OrderNo ,Remarks=Remarks +'; Del:  " + RejectReason + " by " + RejectedByUserId + " on " + DateTime.Now + "' WHERE OrderNo='" + OrderNo + "';";
                Sql = Sql + "UPDATE TrnPartyOrderDetail SET Status='C',ActiveStatus='D',OrderNo= 'Del:'+ OrderNo WHERE OrderNo='" + OrderNo + "';";
                cmd = new SqlCommand();
                cmd.CommandText = Sql;
                cmd.Connection = SC;
                if (SC.State == ConnectionState.Closed)
                    SC.Open();
                cmd.ExecuteNonQuery();
                SC.Close();

                if (SC.State == ConnectionState.Closed)
                    SC.Open();
                var query = "Exec Sp_RevertOrderFromlimit '" + (OrderNo) + "';";
                cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = SC;
                int j = cmd.ExecuteNonQuery();
                SC.Close();
            }
            catch (Exception ex)
            {
                objResponse.ResponseMessage = "Something went wrong!";
                objResponse.ResponseStatus = "FAILED";
            }
            return objResponse;
        }
        public KitDescriptionModel GetKitDescription(decimal KitId)
        {
            KitDescriptionModel objKitDesc = new KitDescriptionModel();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    //objKitDesc = (from r in entity.KitDescriptions
                    //              where r.KitID == KitId
                    //              select new KitDescriptionModel
                    //              {
                    //                  KitId=r.KitID,
                    //                  Cat1=r.Cat1,
                    //                  Cat2=r.Cat2,
                    //                  Cat3=r.Cat3,
                    //                  Cat4=r.Cat4,
                    //                  Cat5=r.Cat5,
                    //                  Cat6=r.Cat6,
                    //                  Others=r.Others
                    //              }
                    //            ).FirstOrDefault();
                    //var result =(from r in entity.M_CatMaster
                    //              where r.ActiveStatus == "Y"
                    //              select new
                    //              {
                    //                 CatId= r.CatId,
                    //                 CatName= r.CatName
                    //              }

                    //            ).ToList();
                    List<string> objListCategory = new List<string>();
                    //foreach(var obj in result)
                    //{
                    //    objListCategory.Add(obj.CatId+"&"+obj.CatName);
                    //}
                    //var resultCategory = string.Join(",", objListCategory);
                    //objKitDesc.CategoryNames = resultCategory;
                }
            }
            catch (Exception ex)
            {

            }
            return objKitDesc;
        }
        public List<KitDetail> GetKitIdList()
        {
            List<KitDetail> KidIDs = new List<KitDetail>();
            try
            {
                string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(AppConnectionString);
                string query = "select KitId,KitName from M_KitMaster where KitAmount>0";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;

                cmd.Connection = SC;
                SC.Close();
                SC.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        KidIDs.Add(new KitDetail { KitId = decimal.Parse(reader["KitId"].ToString()), KitName = reader["KitName"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KidIDs;
        }

        public List<ProductModel> GetOfferList(string Doj, string UpgradeDate, bool IsFirstBill, bool ActiveStatus, string currentPartyCode)
        {
            List<ProductModel> currentOffers = new List<ProductModel>();
            try
            {
                DateTime CrntDate = DateTime.Now.Date;
                using (var entity = new InventoryEntities())
                {
                    List<M_Offers> Offers = (from s in entity.M_Offers
                                             where s.ActiveStatus == "Y" && s.OfferToDt >= CrntDate && s.OfferFromDt <= CrntDate
                                             select s
                                     ).ToList();



                    foreach (var obj in Offers)
                    {
                        bool flag = false;
                        bool flagParty = false;
                        ProductModel objKit = new ProductModel();

                        if (!string.IsNullOrEmpty(obj.ForFranchise))
                        {
                            string[] franchise = obj.ForFranchise.Split(',');
                            for (int i = 0; i < franchise.Length; i++)
                            {
                                if (franchise[i].ToLower() == "all" || franchise[i] == currentPartyCode)
                                {
                                    flagParty = true;
                                    break;
                                }
                            }
                        }

                        if (flagParty == true && !string.IsNullOrEmpty(obj.ForBillType) && (obj.ForBillType.ToLower() == "all" || (obj.ForBillType.ToLower() == "firstbill" && IsFirstBill) || (obj.ForBillType.ToLower() == "repurchase" && IsFirstBill == false)))
                        {
                            if (obj.ForNewIds == "A")
                            {
                                flag = true;
                            }
                            else if (obj.ForNewIds == "Y")
                            {

                                if (obj.IdStatus == "A" || obj.IdStatus == "N")
                                {
                                    if (!string.IsNullOrEmpty(Doj))
                                    {
                                        DateTime dateofjoining = Convert.ToDateTime(Doj);
                                        if (dateofjoining >= obj.IdDate)
                                        {
                                            flag = true;
                                        }
                                    }
                                }
                                if (obj.IdStatus == "Y")
                                {
                                    if (ActiveStatus)
                                    {
                                        if (!string.IsNullOrEmpty(UpgradeDate))
                                        {
                                            DateTime UDate = Convert.ToDateTime(UpgradeDate);
                                            if (UDate >= obj.IdDate)
                                            {
                                                flag = true;
                                            }
                                        }
                                    }
                                }
                            }

                            else if (obj.ForNewIds == "N")
                            {
                                if (ActiveStatus)
                                {
                                    if (!string.IsNullOrEmpty(Doj))
                                    {
                                        DateTime UDate = Convert.ToDateTime(Doj);
                                        if (UDate <= obj.IdDate)
                                        {
                                            flag = true;
                                        }
                                    }
                                }
                            }
                        }

                        if (flag)
                        {
                            objKit.ProdCode = obj.AID;
                            if (obj.OfferOnValue > 0)
                                objKit.ProductName = !string.IsNullOrEmpty(obj.OfferName) ? obj.OfferName : "Amount " + obj.OfferOnValue.ToString();
                            else
                                objKit.ProductName = !string.IsNullOrEmpty(obj.OfferName) ? obj.OfferName : (IsFirstBill ? "FV " : "RV ") + obj.OfferOnBV.ToString();
                            objKit.BV = obj.OfferOnBV;
                            objKit.MinToBV = obj.OfferOnToBV;
                            objKit.OfferType = obj.OfferType != null ? obj.OfferType : 0;

                            if (objKit.OfferType == 2 && objKit.BV == 0)
                            {
                                objKit.ProductName = !string.IsNullOrEmpty(obj.OfferName) ? obj.OfferName : "Rs. 1";
                            }
                            else if (objKit.OfferType == 2)
                            {
                                objKit.ProductName = !string.IsNullOrEmpty(obj.OfferName) ? obj.OfferName : "Rs. 1 on" + objKit.ProductName;
                            }
                            objKit.isFixedQty = obj.IsFixedQty;
                            objKit.FixedQty = obj.FixedQty;

                            objKit.Quantity = 0;// 0 for Normal Offer; greater than Zero for Other Offer
                            currentOffers.Add(objKit);
                        }
                    }
                }

                //if (currentOffers.Count <= 0)//Added on 08Jan19
                var otherOffers = CheckOtherOfferList(currentPartyCode);
                if (otherOffers.Count > 0)
                {
                    foreach (var offer in otherOffers)
                    {
                        offer.OfferType = 9999;
                        if (offer.HSNCode.ToLower().Contains("fv") && IsFirstBill)
                        {
                            currentOffers.Add(offer);
                        }
                        else if (offer.HSNCode.ToLower().Contains("rv") && IsFirstBill == false)
                        {
                            currentOffers.Add(offer);
                        }
                        else if (offer.HSNCode.ToLower().Contains("all"))
                        {
                            currentOffers.Add(offer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return currentOffers;
        }

        public OfferProducts GetOfferDetail(decimal offerId, string prodCode, string IsSpclOffer)
        {
            OfferProducts objProds = new OfferProducts();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    if (IsSpclOffer.ToLower() == "false")
                    {
                        M_Offers Offers = (from s in entity.M_Offers
                                           where s.ActiveStatus == "Y" && s.AID == offerId
                                           select s).FirstOrDefault();
                        var MinBV = Offers.OfferOnBV;
                        var MinToBV = Offers.OfferOnToBV;
                        var TotalQty = Offers.TotalQty;
                        var offerType = Offers.OfferType;
                        objProds = (from s in entity.M_OfferProducts
                                    where s.OfferID == offerId && s.ProdID == prodCode && s.IsBuyProduct != "Y"
                                    select new OfferProducts
                                    {

                                        ProdID = s.ProdID,
                                        offerType = offerType.ToString(),
                                        MinBV = MinBV,
                                        MinToBV = MinToBV,
                                        TotalQty = TotalQty,
                                        Qty = s.Qty,
                                        FreeQty = s.FreeQty,
                                        IsFlexible = s.IsFlexible,
                                        OfferMrp = s.OfferMrp
                                    }).FirstOrDefault();
                    }
                    else
                    {
                        M_OtherOffers Offers = (from s in entity.M_OtherOffers
                                                where s.OfferID == offerId //&& s.ActiveStatus == "Y"
                                                select s).FirstOrDefault();
                        var MinBV = Offers.MinBV;
                        var MinToBV = Offers.OfferOnToBV;
                        var TotalQty = 0;
                        var offerType = 9999;
                        objProds = (from s in entity.M_OfferOtherProducts
                                    where s.OfferID == offerId && s.ProdID == prodCode && s.IsBuyProduct != "Y"
                                    select new OfferProducts
                                    {

                                        ProdID = s.ProdID,
                                        offerType = offerType.ToString(),
                                        MinBV = MinBV,
                                        MinToBV = MinToBV,
                                        TotalQty = TotalQty,
                                        Qty = s.Qty,
                                        FreeQty = s.FreeQty,
                                        IsFlexible = s.IsFlexible,
                                        OfferMrp = s.OfferMrp
                                    }).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return objProds;
        }

        public List<ProductModel> CheckOtherOfferList(string currentPartyCode)//08Jan19
        {
            List<ProductModel> currentOffers = new List<ProductModel>();
            try
            {

                DateTime Dt = DateTime.Now.Date;
                using (var entity = new InventoryEntities())
                {
                    var offers = (from s in entity.M_OtherOffers
                                  where s.FromDate <= Dt && s.ToDate >= Dt && s.ActiveStatus == "Y"
                                  select new ProductModel
                                  {
                                      ProdCode = s.OfferID,
                                      ProductName = s.OfferName,
                                      BV = s.MinBV,
                                      Quantity = s.StartProduct,
                                      PartyName = s.ForFranchise,
                                      HSNCode = s.Forbill
                                  }).ToList();
                    foreach (var obj in offers)
                    {
                        if (!string.IsNullOrEmpty(obj.PartyName))
                        {
                            string[] franchise = obj.PartyName.Split(',');
                            for (int i = 0; i < franchise.Length; i++)
                            {
                                if (franchise[i].ToLower() == "all" || franchise[i] == currentPartyCode)
                                {
                                    currentOffers.Add(obj);
                                }
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
            }
            return currentOffers;
        }

        public ResponseDetail DeleteBills(string BillNo, string FsessId, decimal UserId, string Reason)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseMessage = "Something went wrong!";

            objResponse.ResponseStatus = "FAILED";
            try
            {

                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConnectionString);
                //get billids from TrnBillMain 
                var billNos = BillNo.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                List<decimal> billids = new List<decimal>();
                using (var entity = new InventoryEntities())
                {
                    billids = (from bill in entity.TrnBillMains
                               where billNos.Contains(bill.BillNo)
                               select bill.BillId).ToList();
                }
                //------------------------------------
                string sqlQry = "";
                var billNoArray = BillNo.Split(',');
                var FsessIdArray = FsessId.Split(',');
                for (var j = 0; j < billNoArray.Length; j++)
                {
                    if (!string.IsNullOrEmpty(billNoArray[j]))
                    {
                        sqlQry += "Exec ROLLBACKBILL '" + billNoArray[j] + "','" + Reason + "'," + UserId + "," + FsessIdArray[j] + ";";
                    }
                }

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlQry;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    //try
                    //{
                    //    string result = "";
                    //    using (var entity = new InventoryEntities())
                    //    {
                    //        var billnoresponse = (from res in entity.tblApiRequestResponses
                    //                              join bill in entity.DeletedBillMains
                    //                              on res.UserBillNo equals bill.UserBillNo
                    //                              where billids.Contains(bill.BillId)
                    //                                    && res.Billrefid == bill.BillId
                    //                                    && res.Entrytype == "A"
                    //                              orderby bill.RecTimeStamp descending
                    //                              select new
                    //                              {
                    //                                  res.UserBillNo,
                    //                                  res.ApiURL,
                    //                                  res.Request,
                    //                                  res.Response,
                    //                                  res.RestimeStamp,
                    //                                  res.Error,
                    //                                  res.Entrytype,
                    //                                  res.Billrefid
                    //                              }).ToList();


                    //        foreach (var row in billnoresponse)
                    //        {
                    //            if (!string.IsNullOrEmpty(row.Response))
                    //            {
                    //                var responseObj = JObject.Parse(row.Response);

                    //                if (responseObj["Success"]?.ToString().ToLower() == "true")
                    //                {
                    //                    var requestObj = JObject.Parse(row.Request);

                    //                    string[] fields = { "bv", "PV", "billamount", "fpamt", "voucheramt" };

                    //                    foreach (var field in fields)
                    //                    {
                    //                        if (requestObj[field] != null)
                    //                        {
                    //                            decimal val = requestObj[field].Value<decimal>();

                    //                            if (val > 0)
                    //                                requestObj[field] = -val;
                    //                        }
                    //                    }

                    //                    string newRequestJson = requestObj.ToString();

                    //                    // Call API again
                    //                    tblApiRequestResponse log = new tblApiRequestResponse();
                    //                    log.ApiURL = row.ApiURL;
                    //                    log.Billrefid = row.Billrefid;
                    //                    try
                    //                    {
                    //                        // Create a request
                    //                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://zoewellness.co.in/api/api.ashx");
                    //                        request.Method = "POST";
                    //                        request.ContentType = "application/json"; // Set content type to JSON
                    //                                                                  // If the API requires headers (e.g., Authorization), add them here
                    //                                                                  // request.Headers.Add("Authorization", "Bearer YOUR_TOKEN");
                    //                                                                  // Write JSON data to request stream
                    //                        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    //                        {
                    //                            streamWriter.Write(newRequestJson);
                    //                            streamWriter.Flush();
                    //                        }
                    //                        // Get the response
                    //                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    //                        {
                    //                            using (var streamReader = new StreamReader(response.GetResponseStream()))
                    //                            {
                    //                                result = streamReader.ReadToEnd();
                    //                                //return result; // Optionally deserialize this JSON string to an object
                    //                            }
                    //                        }
                    //                        // Save new request/response
                    //                        log.UserBillNo = row.UserBillNo;
                    //                        log.Request = newRequestJson;
                    //                        log.Response = result;
                    //                        log.RestimeStamp = DateTime.Now;
                    //                        log.Entrytype = "D";
                    //                        log.Error = "";
                    //                    }
                    //                    catch (Exception ex)
                    //                    {
                    //                        log.UserBillNo = row.UserBillNo;
                    //                        log.Request = newRequestJson;
                    //                        log.Response = "";
                    //                        log.RestimeStamp = DateTime.Now;
                    //                        log.Entrytype = "D";
                    //                        log.Error = ex.Message;
                    //                    }
                    //                    entity.tblApiRequestResponses.Add(log);
                    //                }
                    //            }
                    //        }
                    //        entity.SaveChanges();
                    //    }

                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    objResponse.ResponseMessage = "Successfully Deleted!";
                    objResponse.ResponseStatus = "OK";
                }
                else
                {

                    objResponse.ResponseMessage = "Something went wrong!";
                    objResponse.ResponseStatus = "FAILED";
                    return objResponse;
                }

            }
            catch (Exception ex)
            {

            }
            return objResponse;
        }
        public ResponseDetail DeletePurchaseInvoice(string InwardNo, decimal FsessId, decimal UserId, string Reason)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseMessage = "Something went wrong!";

            objResponse.ResponseStatus = "FAILED";
            try
            {

                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConnectionString);

                string sqlQry = "Exec ROLLBACK_PI '" + InwardNo + "','" + Reason + "'," + UserId + "," + FsessId + ";";


                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlQry;

                cmd.Connection = SC;


                SC.Close();
                SC.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    objResponse.ResponseMessage = "Successfully Deleted!";
                    objResponse.ResponseStatus = "OK";


                }
                else
                {

                    objResponse.ResponseMessage = "Something went wrong!";
                    objResponse.ResponseStatus = "FAILED";
                }

            }
            catch (Exception ex)
            {

            }
            return objResponse;
        }

        public ResponseDetail SaveOrderReturn(SalesReturnModel objSalesReturnOrder)
        {

            ResponseDetail objResponse = new ResponseDetail();


            decimal? SessId = 0;
            string UserReturnNo = "";
            string Loggedinparty = "";
            objResponse.ResponseMessage = "Something went wrong!";
            objResponse.ResponseStatus = "FAILED";
            try
            {
                Loggedinparty = objSalesReturnOrder.EntryBy;

                string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(AppConnectionString);

                string query = "Select Max(SessID) as MaxSessId from M_SessnMaster";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        SessId = decimal.Parse(reader["MaxSessId"].ToString());
                    }
                }
                using (var entity = new InventoryEntities())
                {

                    try
                    {

                        string part1 = (from r in entity.M_ConfigMaster select r.BillPrefix).FirstOrDefault();
                        if (!String.IsNullOrEmpty(part1))
                        {
                            UserReturnNo += part1 + "GR/" + Loggedinparty + "/";
                        }

                        decimal part2 = (from r in entity.TrnSalesReturnMains select r.SRNo).DefaultIfEmpty(0).Max();
                        part2 += 1;
                        UserReturnNo += part2;

                        int fessid = Convert.ToInt32((from result in entity.M_FiscalMaster where result.ActiveStatus == "Y" select result.FSessId).DefaultIfEmpty(0).Max());
                        string version = (from result in entity.M_NewHOVersionInfo select result.VersionNo).FirstOrDefault();

                        //var billDetail = (from r in entity.TrnBillMains where r.BillNo == objSalesReturnOrder.BillNo select r).FirstOrDefault();

                        TrnSalesReturnMain objreturnMain = new TrnSalesReturnMain();

                        objreturnMain.SReturnNo = UserReturnNo;
                        objreturnMain.SRNo = part2;
                        objreturnMain.ReturnBy = objSalesReturnOrder.returnby;
                        objreturnMain.ReturnByName = (from result in entity.M_LedgerMaster where result.PartyCode == objSalesReturnOrder.returnby select result.PartyName).FirstOrDefault();
                        objreturnMain.ReturnTo = (from result in entity.M_LedgerMaster where result.PartyCode == objSalesReturnOrder.returnby select result.ParentPartyCode).FirstOrDefault();
                        objreturnMain.ReturnToName = (from result in entity.M_LedgerMaster where result.PartyCode == objreturnMain.ReturnTo select result.PartyName).FirstOrDefault();
                        objreturnMain.Ftype = objSalesReturnOrder.Ftype;
                        objreturnMain.FormNo = 0;
                        objreturnMain.ReturnDate = DateTime.Now.Date;
                        objreturnMain.RefNo = objSalesReturnOrder.refno ?? "";
                        objreturnMain.TotalAmount = objSalesReturnOrder.tAmt;
                        objreturnMain.Remarks = objSalesReturnOrder.remark;
                        objreturnMain.RndOff = objSalesReturnOrder.rndOff;
                        objreturnMain.NetPayable = objSalesReturnOrder.netPay;
                        objreturnMain.RType = "S";
                        objreturnMain.Reason = objSalesReturnOrder.reason;
                        objreturnMain.RecTimeStamp = DateTime.Now;
                        objreturnMain.ActiveStatus = "Y";
                        objreturnMain.Status = "P";
                        objreturnMain.Version = version;
                        objreturnMain.UserID = objSalesReturnOrder.LoggedInUserId;
                        objreturnMain.TotalBV = objSalesReturnOrder.TotalBV;
                        objreturnMain.TotalRP = objSalesReturnOrder.TotalRP;
                        objreturnMain.UID = objSalesReturnOrder.LoggedInUserIP;
                        objreturnMain.OrderNo = objSalesReturnOrder.BillNo;
                        objreturnMain.OrderDate = objSalesReturnOrder.BillDate;
                        objreturnMain.FSessId = fessid;
                        objreturnMain.EntryBy = objSalesReturnOrder.EntryBy;

                        decimal Totaltax = 0;
                        decimal TotalSGSTAmt = 0;
                        decimal TotalCGSTAmt = 0;
                        foreach (var product in objSalesReturnOrder.ProductList)
                        {
                            TrnSalesReturnDetail objDTBillData = new TrnSalesReturnDetail();
                            if (product.ReturnQty > 0)
                            {
                                objDTBillData.SReturnNo = UserReturnNo;
                                objDTBillData.SRNo = part2;
                                objDTBillData.ReturnBy = objSalesReturnOrder.returnby;
                                objDTBillData.ReturnTo = objreturnMain.ReturnTo;
                                objDTBillData.BatchNo = product.BatchNo;
                                objDTBillData.Ftype = objSalesReturnOrder.Ftype;
                                objDTBillData.ReturnDate = DateTime.Now.Date;
                                objDTBillData.ProdId = product.ProductCodeStr;
                                objDTBillData.ProductName = product.ProductName;
                                objDTBillData.ReturnQty = product.ReturnQty;
                                objDTBillData.FreeQty = 0;
                                objDTBillData.AcceptQty = 0;
                                objDTBillData.RemainingQty = 0;
                                objDTBillData.Rate = product.Rate ?? 0;
                                objDTBillData.Amount = product.Amount;
                                objDTBillData.RecTimeStamp = DateTime.Now;
                                objDTBillData.UserID = objSalesReturnOrder.LoggedInUserId;
                                objDTBillData.Version = version;
                                objDTBillData.BV = product.BV ?? 0;
                                objDTBillData.RP = product.RP ?? 0;
                                objDTBillData.RPValue = product.RPValue ?? 0;
                                objDTBillData.BVValue = product.BVValue ?? 0;
                                objDTBillData.TaxType = "";
                                objDTBillData.Status = "P";
                                objDTBillData.ActiveStatus = "Y";
                                objDTBillData.IsKit = "N";
                                objDTBillData.ProdType = "P";
                                objDTBillData.ROn = "D";

                                objDTBillData.OfferUId = 0;
                                objDTBillData.MRP = product.MRP ?? 0;
                                objDTBillData.DP = product.DP ?? 0;
                                objDTBillData.FSessId = fessid;
                                objDTBillData.EntryBy = objSalesReturnOrder.EntryBy;

                                if (product.TaxAmt != 0)
                                {
                                    objDTBillData.Tax = 0;
                                    objDTBillData.TaxAmount = 0;
                                    objDTBillData.CGST = product.GSTPer / 2;
                                    objDTBillData.CGSTAmt = ((product.TaxAmt ?? 0) / 2);
                                    objDTBillData.SGST = product.GSTPer / 2;
                                    objDTBillData.SGSTAmt = ((product.TaxAmt ?? 0) / 2);
                                    Totaltax += objDTBillData.TaxAmount;
                                    TotalSGSTAmt += objDTBillData.SGSTAmt;
                                    TotalCGSTAmt += objDTBillData.CGSTAmt;
                                }
                                else
                                {
                                    objDTBillData.Tax = product.GSTPer;
                                    objDTBillData.TaxAmount = product.TaxAmt ?? 0;
                                    objDTBillData.CGST = 0;
                                    objDTBillData.CGSTAmt = 0;
                                    objDTBillData.SGST = 0;
                                    objDTBillData.SGSTAmt = 0;
                                    Totaltax += objDTBillData.TaxAmount;
                                    TotalSGSTAmt += objDTBillData.SGSTAmt;
                                    TotalCGSTAmt += objDTBillData.CGSTAmt;
                                }

                                entity.TrnSalesReturnDetails.Add(objDTBillData);

                            }

                            objreturnMain.TaxAmount = Totaltax;

                            objreturnMain.CGSTAmt = TotalSGSTAmt;
                            objreturnMain.SGSTAmt = TotalCGSTAmt;

                            entity.TrnSalesReturnMains.Add(objreturnMain);
                        }
                        int i = 0;
                        try
                        {
                            i = entity.SaveChanges();
                        }

                        catch (DbUpdateException ex)
                        {

                        }

                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                        ve.PropertyName, ve.ErrorMessage);
                                }
                            }
                            throw;
                        }

                        if (i > 0)
                        {
                            objResponse.ResponseMessage = "Saved Successfully!";
                            objResponse.ResponseStatus = "OK";
                        }

                    }
                    catch (DbEntityValidationException e)
                    {
                        objResponse.ResponseMessage = "Something went wrong!";
                        objResponse.ResponseStatus = "FAILED";
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return objResponse;
        }

        public List<PartyBill> GetBillList(string partyType, string Fcode)
        {
            List<PartyBill> billList = null;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    DateTime date1 = DateTime.Now.Date.AddDays(-30).Date;
                    DateTime date2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01).Date;
                    int lastdate = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

                    if (partyType == "customer")
                    {
                        billList = (from r in entity.TrnBillMains
                                    let date = r.OrderType == "T" ? date1 : date2
                                    where (r.FType == "GC") && (r.BillDate >= date2) && (r.BillDate <= DateTime.Now)
                                    select new PartyBill
                                    {
                                        BillNo = r.BillNo,
                                        UserBillNo = r.UserBillNo,
                                        SoldBy = r.SoldBy,
                                        SoldByName = r.PartyName,
                                        BillDate = r.BillDate

                                    }).OrderByDescending(o => o.BillDate).ToList();
                    }
                    else if (partyType == "party")
                    {
                        billList = (from r in entity.TrnBillMains
                                    let date = r.OrderType == "T" ? date1 : date2
                                    where r.FCode == Fcode && (r.FType != "GC" && r.FType != "M") && (r.BillDate >= date2) && (r.BillDate <= DateTime.Now)
                                    select new PartyBill
                                    {
                                        BillNo = r.BillNo,
                                        UserBillNo = r.UserBillNo,
                                        SoldBy = r.SoldBy,
                                        SoldByName = r.PartyName,
                                        BillDate = r.BillDate

                                    }).ToList().OrderByDescending(o => o.BillDate).ToList();
                    }
                    else
                    {
                        billList = (from r in entity.TrnBillMains
                                    let date = r.OrderType == "T" ? date1 : date2
                                    where r.FCode == Fcode && r.FType == "M" && (r.BillDate >= date2) && (r.BillDate <= DateTime.Now)
                                    select new PartyBill
                                    {
                                        BillNo = r.BillNo,
                                        UserBillNo = r.UserBillNo,
                                        SoldBy = r.SoldBy,
                                        SoldByName = r.PartyName,
                                        BillDate = r.BillDate

                                    }).ToList().OrderByDescending(o => o.BillDate).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return billList;
        }

        public PartyBill GetBillDetail(string BillNo)
        {
            PartyBill billdetail = null;
            try
            {
                using (var entity = new InventoryEntities())
                {

                    billdetail = (from r in entity.TrnBillMains
                                  where r.BillNo == BillNo
                                  select new PartyBill
                                  {
                                      BillNo = r.BillNo,
                                      SoldBy = r.SoldBy,
                                      partyCode = r.FCode,
                                      partyName = r.PartyName,
                                      BillDate = r.BillDate,
                                      NetPayable = r.NetPayable,
                                      RoundOff = r.RndOff,
                                      TotalTax = r.TaxAmount,
                                      TotalAmount = r.Amount
                                  }).FirstOrDefault();
                    billdetail.BillDateStr = billdetail.BillDate.ToString("dd-MMM-yyyy");
                    billdetail.ProductList = new List<ProductModel>();
                    billdetail.ProductList = (from r in entity.TrnBillDetails
                                              join t in entity.M_TaxMaster on r.ProductId equals t.ProdCode
                                              where r.BillNo == BillNo
                                              select new ProductModel
                                              {
                                                  Barcode = r.Barcode,
                                                  BatchNo = r.BatchNo,
                                                  ProductName = r.ProductName,

                                                  ProductCodeStr = r.ProductId,
                                                  Rate = r.Rate,
                                                  MRP = r.MRP,
                                                  BV = r.BV,
                                                  BVValue = r.BvValue,
                                                  RP = r.RP,
                                                  RPValue = r.RPValue,
                                                  TaxAmt = r.TaxAmount,
                                                  Amount = r.NetAmount,
                                                  TotalAmount = r.NetAmount,
                                                  Quantity = r.Qty,
                                                  GSTPer = t.VatTax,
                                                  ReturnQty = 0,
                                                  DP = r.DP,
                                                  DiscPer = r.DiscountPer,
                                                  DiscAmt = r.Discount,
                                                  ProdStateCode = t.StateCode,
                                                  PV = r.PV,
                                                  CV = r.CV,
                                                  CommissionPer = r.ProdCommssn,
                                                  CommissionAmt = r.ProdCommssnAmt,
                                                  CVValue = r.CVValue,
                                                  PVValue = r.PVValue
                                              }).ToList();
                }

            }
            catch (Exception ex)
            {

            }

            return billdetail;
        }
        public PartyBill GetBillDetailNew(string BillNo, string CurrentPartyCode)
        {
            PartyBill billdetail = null;
            try
            {

                List<ProductModel> ProductReturnDetail = new List<ProductModel>();
                string InvConstr = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                //string dbInv = invDbName;
                string sql = "";

                sql += "Select a.ProductID as ProductID,a.ProductName,a.MRP,a.DP,a.Rate,a.Qty,ISNULL(c.ReturnQty,0) as PReturn,a.Qty-ISNULL(c.ReturnQty,0) as PRemaing FROm TrnBillDetails a ";
                sql += " LEFT JOIN (Select b.OrderNo,c.ProdID,SUM(ISNULL(c.ReturnQty,0)) ReturnQty  FROM TrnSalesReturnMain b";
                sql += " INNER JOIN TrnSalesReturnDetail c ON b.SReturnNo = c.SReturnNo WHERE OrderNo = '" + BillNo + "' GROUP BY c.ProdID,b.OrderNo) c ON a.BillNo = c.OrderNo AND a.ProductID=c.ProdID";
                sql += " WHERE a.BillNo = '" + BillNo + "' AND a.Qty-ISNULL(c.ReturnQty,0)>0";

                string InvConnectionString = InvConstr;
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
                        ProductModel tempobj = new ProductModel();
                        tempobj.ProductCodeStr = reader["ProductID"] != null ? reader["ProductID"].ToString() : "";
                        tempobj.PReturnQuantity = reader["PReturn"] != null ? Convert.ToDecimal(reader["PReturn"].ToString()) : 0;
                        tempobj.Remaining = reader["PRemaing"] != null ? Convert.ToDecimal(reader["PRemaing"].ToString()) : 0;
                        ProductReturnDetail.Add(tempobj);
                    }
                }


                using (var entity = new InventoryEntities())
                {

                    billdetail = (from r in entity.TrnBillMains
                                  where r.BillNo == BillNo
                                  select new PartyBill
                                  {
                                      BillNo = r.BillNo,
                                      SoldBy = r.SoldBy,
                                      partyCode = r.FCode,
                                      partyName = r.PartyName,
                                      BillDate = r.BillDate,
                                      NetPayable = r.NetPayable,
                                      RoundOff = r.RndOff,
                                      TotalTax = r.TaxAmount,
                                      TotalAmount = r.Amount
                                  }).FirstOrDefault();
                    billdetail.BillDateStr = billdetail.BillDate.ToString("dd-MMM-yyyy");


                    billdetail.ProductList = new List<ProductModel>();
                    billdetail.ProductList = (from r in entity.TrnBillDetails
                                              join t in entity.M_TaxMaster on r.ProductId equals t.ProdCode
                                              where r.BillNo == BillNo
                                              select new ProductModel
                                              {
                                                  Barcode = r.Barcode,
                                                  BatchNo = r.BatchNo,
                                                  ProductName = r.ProductName,

                                                  ProductCodeStr = r.ProductId,
                                                  Rate = r.Rate,
                                                  MRP = r.MRP,
                                                  BV = r.BV,
                                                  BVValue = r.BvValue,
                                                  RP = r.RP,
                                                  RPValue = r.RPValue,
                                                  TaxAmt = r.TaxAmount,
                                                  Amount = r.NetAmount,
                                                  TotalAmount = r.NetAmount,
                                                  Quantity = r.Qty,
                                                  GSTPer = t.VatTax,
                                                  ReturnQty = 0,
                                                  DP = r.DP,
                                                  DiscPer = r.DiscountPer,
                                                  DiscAmt = r.Discount,
                                                  ProdStateCode = t.StateCode,
                                                  PV = r.PV,
                                                  CV = r.CV,
                                                  CommissionPer = r.ProdCommssn,
                                                  CommissionAmt = r.ProdCommssnAmt,
                                                  CVValue = r.CVValue,
                                                  PVValue = r.PVValue
                                              }).ToList();

                    foreach (var record in billdetail.ProductList)
                    {

                        record.StockAvailable = (from stockAvail in entity.Im_CurrentStock
                                                 where stockAvail.BatchCode == record.Barcode.ToString() && stockAvail.ProdId == record.ProductCodeStr.ToString() && stockAvail.FCode.Equals(billdetail.partyCode)
                                                 select stockAvail.Qty
                                                 ).DefaultIfEmpty(0).Sum();
                        record.PReturnQuantity = (from r in ProductReturnDetail where r.ProductCodeStr == record.ProductCodeStr select r.PReturnQuantity).FirstOrDefault();
                        record.Remaining = (from r in ProductReturnDetail where r.ProductCodeStr == record.ProductCodeStr select r.Remaining).FirstOrDefault();
                    }
                }
            }
            //catch (Exception ex)
            //{

            //}
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return billdetail;
        }
        public List<PartyBill> GetListOfSupplierBills(string supplier)
        {
            List<PartyBill> billList = null;
            try
            {
                using (var entity = new InventoryEntities())
                {

                    billList = (from r in entity.M_InwardMain
                                where r.SupplierCode == supplier
                                select new PartyBill
                                {
                                    BillNo = r.InwardNo,
                                    BillDate = r.OrderDate
                                }).ToList().OrderByDescending(o => o.BillDate).ToList();

                }
            }
            catch (Exception ex)
            {

            }
            return billList;
        }

        public string GetSalesReturnNumber(string Loggedinparty)
        {
            string returnNo = string.Empty;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    string part1 = (from r in entity.M_ConfigMaster select r.BillPrefix).FirstOrDefault();
                    if (!String.IsNullOrEmpty(part1))
                    {
                        returnNo += part1 + "GR/" + Loggedinparty + "/";
                    }

                    decimal part2 = (from r in entity.TrnSalesReturnMains select r.SRNo).DefaultIfEmpty(0).Max();
                    part2 += 1;
                    returnNo += part2;
                }
            }
            catch (Exception ex)
            {

            }
            return returnNo;
        }

        public ResponseDetail SavePartyTargetDetails(PartyTargetMaster objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseMessage = "Something went wrong!";
            objResponse.ResponseStatus = "FAILED";
            TargetMaster targetMaster = new TargetMaster();
            TargetDetail targetDetail = new TargetDetail();
            string Version = "";
            try
            {
                using (var entity = new InventoryEntities())
                {

                    decimal maxTargetId = 0;
                    maxTargetId = (from r in entity.TargetMasters
                                   select r.TID
                               ).DefaultIfEmpty(0).Max();
                    maxTargetId = maxTargetId + 1;

                    targetMaster.TID = maxTargetId;
                    targetMaster.UserID = objModel.UserID;
                    targetMaster.ToDate = objModel.ToDate;
                    targetMaster.FrmDate = objModel.FrmDate;
                    targetMaster.RecTimeStamp = DateTime.Now;
                    targetMaster.MaxValue = objModel.MaxValue;
                    targetMaster.CommPer = objModel.CommPer;
                    targetMaster.OnValue = "P";

                    entity.TargetMasters.Add(targetMaster);


                    targetDetail.TID = maxTargetId;
                    targetDetail.CatID = objModel.CatId;
                    targetDetail.RecTimeStamp = DateTime.Now;

                    entity.TargetDetails.Add(targetDetail);

                    int i = entity.SaveChanges();
                    if (i > 0)
                    {
                        objResponse.ResponseMessage = "Saved Successfully!";
                        objResponse.ResponseStatus = "OK";
                    }
                    else
                    {
                        objResponse.ResponseMessage = "Something went wrong!";
                        objResponse.ResponseStatus = "FAILED";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objResponse;
        }

        public List<SalesReport> GetRecordToUpdateDelDetails(string FromDate, string ToDate, string PartyCode, string Fcode, string status)
        {
            List<SalesReport> objReport = new List<SalesReport>();
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



                if (!string.IsNullOrEmpty(PartyCode) && PartyCode.ToUpper() != "ALL" && PartyCode.ToUpper() != "0")
                {
                    wherea = wherea + " and SoldBy='" + PartyCode.Trim() + "' ";
                }

                if (!string.IsNullOrEmpty(Fcode) && Fcode.ToUpper() != "ALL" && Fcode.ToUpper() != "0")
                {
                    wherea = wherea + " and a.FCode='" + Fcode.Trim() + "' ";
                }

                if (!string.IsNullOrEmpty(status) && status.ToUpper() != "ALL")
                {
                    if (status.ToUpper() == "PENDING")
                        wherea = wherea + " And o.DispatchStatus<>'C'";
                    else
                        wherea = wherea + " And o.DispatchStatus='C'";
                }

                string db = System.Configuration.ConfigurationManager.AppSettings["Database"];
                string dbInv = System.Configuration.ConfigurationManager.AppSettings["INVDatabase"];


                string sql = "Select  a.UserBillNo,BillDate,SoldBy as BillBy,b.PartyName,IsNull(c.IDNo,a.FCode) as Code,Case When (IsNull(c.MemFirstName,'') + ' ' + IsNull(c.MemLastName,'')) = '' Then a.PartyName Else IsNull(c.MemFirstName,'') + ' ' + IsNull(c.MemLastName,'') End as Name,a.CourierName,a.DocWeight,a.DocketNo,a.DocketDate as DocketDate,a.DOD as DOD,a.DelvAddress,a.CourierId as CID,a.BillNo,LRDate as DispDate,NetPayable,c.Mobl as MobileNO,a.OrderNo ";
                sql += "From M_LedgerMaster as b , " + db + "..M_MemberMaster as c,TrnBillMain as a LEFT JOIN " + db + "..TrnOrder o On a.OrderNo=Cast(o.OrderNo as varchar(20)) Where a.BillFor=Cast(c.FormNo as Varchar) AND a.SoldBy=b.PartyCode AND BillDate between '" + NewFromDate + "' AND '" + NewToDate + "' " + wherea;

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
                        SalesReport tempobj = new SalesReport();

                        tempobj.BillNo = reader["UserBillNo"] != null ? reader["UserBillNo"].ToString() : "";
                        tempobj.BillDate = reader["BillDate"] != System.DBNull.Value ? Convert.ToDateTime(reader["BillDate"]) : (DateTime?)null;
                        tempobj.SoldBy = reader["BillBy"] != null ? reader["BillBy"].ToString() : "";
                        tempobj.PartyName = reader["PartyName"] != null ? reader["PartyName"].ToString() : "";
                        tempobj.PartyCode = reader["Code"] != null ? reader["Code"].ToString() : "";
                        tempobj.Name = reader["Name"] != null ? reader["Name"].ToString() : "";
                        tempobj.CourierName = reader["CourierName"] != null ? reader["CourierName"].ToString() : "";
                        tempobj.DocWeight = reader["DocWeight"] != null ? reader["DocWeight"].ToString() : "";

                        tempobj.DocketNo = reader["DocketNo"] != null ? reader["DocketNo"].ToString() : "";
                        tempobj.DocketDate = reader["DocketDate"] != System.DBNull.Value ? Convert.ToDateTime(reader["DocketDate"]) : (DateTime?)null;
                        tempobj.DOD = reader["DOD"] != System.DBNull.Value ? Convert.ToDateTime(reader["DOD"]) : (DateTime?)null;
                        tempobj.DelvAddress = reader["DelvAddress"] != null ? reader["DelvAddress"].ToString() : "";

                        tempobj.CID = reader["CID"] != null ? reader["CID"].ToString() : "";
                        tempobj.BillNo = reader["BillNo"] != null ? reader["BillNo"].ToString() : "";
                        tempobj.DispDate = reader["DispDate"] != System.DBNull.Value ? Convert.ToDateTime(reader["DispDate"]) : (DateTime?)null;
                        tempobj.DelvAddress = reader["DelvAddress"] != null ? reader["DelvAddress"].ToString() : "";

                        tempobj.NetPayable = reader["NetPayable"] != null ? reader["NetPayable"].ToString() : "";
                        tempobj.MobileNO = reader["MobileNO"] != null ? reader["MobileNO"].ToString() : "";
                        tempobj.OrderNo = reader["OrderNo"] != null ? reader["OrderNo"].ToString() : "";
                        tempobj.DelvAddress = reader["DelvAddress"] != null ? reader["DelvAddress"].ToString() : "";

                        objReport.Add(tempobj);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objReport;
        }

        public ResponseDetail UpdateDeliveryDetails(UpdateDeliveryDetails obj)
        {
            ResponseDetail objresponse = new ResponseDetail();
            int fessid = 0;
            int MaxCode = 0;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    fessid = Convert.ToInt32((from r in entity.M_FiscalMaster where r.ActiveStatus == "Y" select r.FSessId).DefaultIfEmpty(0).Max());
                    MaxCode = Convert.ToInt32((from r in entity.TrnOrderDeliveryDetails select r.SNo).DefaultIfEmpty(0).Max());
                    MaxCode += 1;

                }

                string Sql = string.Empty;
                foreach (var record in obj.DeliverDetailList)
                {
                    Sql += "UPDATE TrnBillMain SET CourierName = '" + record.CourierName + "',";
                    Sql += "DocketNo = '" + record.DocketNo + "',";
                    Sql += "DocketDate = " +
                           (record.DocketDate != null
                                ? "'" + Convert.ToDateTime(record.DocketDate).ToString("yyyy-MM-dd HH:mm:ss") + "'"
                                : "null") + ",";
                    Sql += "DocWeight = '" + record.DocWeight + "',";
                    Sql += "DelvAddress = '" + record.DelvAddress + "',";
                    Sql += "DelvStatus = 'C', DelvRecTimeStamp = GetDate(), DelvUserId = '" + obj.LoggedInUser + "'";
                    Sql += " WHERE FSessId = '" + fessid + "' AND BillNo = '" + record.BillNo + "' ;";

                    Sql += "UPDATE TrnProductDispatchDetail SET ";
                    Sql += "CourierName = '" + record.CourierName + "',";
                    Sql += "DocketNo = '" + record.DocketNo + "',";
                    Sql += "DocketDate = " +
                           (record.DocketDate != null
                                ? "'" + Convert.ToDateTime(record.DocketDate).ToString("yyyy-MM-dd HH:mm:ss") + "'"
                                : "null") + ",";
                    Sql += "DocWeight = '" + record.DocWeight + "',";
                    Sql += "DelvAddress = '" + record.DelvAddress + "',";
                    Sql += "DelvStatus = 'C',";
                    Sql += "DelvRecTimeStamp = GetDate(),";
                    Sql += "DelvUserId = '" + obj.LoggedInUser + "'";
                    Sql += " WHERE FSessId = '" + fessid + "' AND BillNo = '" + record.BillNo + "'; ";
                    ////Sql += "UPDATE TrnBillMain SET CourierId = '" + record.CID +"',";
                    //Sql += "UPDATE TrnBillMain SET CourierName = '" + record.CourierName + "',";
                    //Sql += "DocketNo = '" + record.DocketNo + "',";
                    //Sql += "DocketDate = " + (record.DocketDate != null ? ("'" + record.DocketDate + "'") : "null") + ",";
                    //Sql += "DocWeight = '" + record.DocWeight + "',";
                    ////Sql += "DOD = "  + (record.DOD != null ? ("'" + record.DOD + "'") : "null") + ",";
                    //Sql += "DelvAddress = '" + record.DelvAddress + "',";
                    //Sql += "DelvStatus = 'C', DelvRecTimeStamp = GetDate(), DelvUserId = '" + obj.LoggedInUser + "'";
                    //Sql += "WHERE FSessId = '" + fessid + "' AND BillNo = '" + record.BillNo + "' ;";

                    ////Sql += "UPDATE TrnProductDispatchDetail SET  CourierId='" + record.CID + "',CourierName='" + record.CourierName + "', DocketNo='" + record.DocketNo + "',DocketDate=" + (record.DocketDate != null ? ("'" + record.DocketDate + "'") : "null") + ",DocWeight='" + record.DocWeight + "',DOD=" + (record.DOD != null ? ("'" + record.DOD + "'") : "null") + ",DelvAddress='" + record.DelvAddress + "',DelvStatus='C',DelvRecTimeStamp=GetDate(),DelvUserId='" + obj.LoggedInUser + "' WHERE FSessId='" + fessid + "' AND BillNo='" + record.BillNo + "'; ";
                    //Sql += "UPDATE TrnProductDispatchDetail SET  CourierName='" + record.CourierName + "', DocketNo='" + record.DocketNo + "',DocketDate=" + (record.DocketDate != null ? ("'" + record.DocketDate + "'") : "null") + ",DocWeight='" + record.DocWeight + "',DelvAddress='" + record.DelvAddress + "',DelvStatus='C',DelvRecTimeStamp=GetDate(),DelvUserId='" + obj.LoggedInUser + "' WHERE FSessId='" + fessid + "' AND BillNo='" + record.BillNo + "'; ";

                }

                var billnos = (from r in obj.DeliverDetailList select "'" + r.BillNo + "'").ToArray();

                var result = String.Join(", ", billnos.ToArray());

                Sql += " INSERT INTO TrnOrderDeliveryDetail(SNo, SoldBy, OrderNo, OrderDate, BillNo, BillDate, CourierId, CourierName, DocWeight, DocketNo, DocketDate, DOD, DelvAddress, UserId) ";
                Sql += " SELECT " + MaxCode + " as SNo,SoldBy,OrderNo,OrderDate,BillNo,BillDate,CourierId,CourierName,DocWeight,DocketNo,DocketDate,DOD,DelvAddress,UserId From TrnBillMain Where FSessId='" + fessid + "' And BillNo In(" + result + ")";

                SqlTransaction objTrans = null;
                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConnectionString);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = Sql;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();

                //throw new Exception(Sql);
                int i = cmd.ExecuteNonQuery();


                if (i > 0)
                {
                    objresponse.ResponseStatus = "OK";
                    objresponse.ResponseMessage = "Records Updated Successfully";
                }
                else
                {
                    objresponse.ResponseStatus = "FAILED";
                    objresponse.ResponseMessage = "Something went wrong";
                }
                SC.Close();

            }
            catch (Exception ex)
            {
                objresponse.ResponseStatus = "FAILED";
                objresponse.ResponseMessage = "Something went wrong";
            }

            return objresponse;
        }

        public ResponseDetail CheckForOffer(DistributorBillModel objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            //try
            //{
            //    VisionOffer monthOfferList = new VisionOffer();
            //    VisionOffer EarlyRiserOffer = new VisionOffer();
            //    var CurrentDate = DateTime.Now;
            //    using (var entity = new InventoryEntities())
            //    {
            //        string OfferDatePart=""; int OfferID = 0; string FreeProdIDs = ""; decimal FreeProdQtys = 0; string confProdIDs = ""; string ConfProdQtys = ""; string offerbillvalue = "0";
            //        EarlyRiserOffer = (from r in entity.VisionOffers where r.OfferDatePart == "D" && r.OfferOnValue <= objModel.objProduct.TotalNetPayable && r.OfferOnBV <= objModel.objProduct.TotalBV select r).OrderByDescending(o => o.OfferOnValue).FirstOrDefault();
            //        if (EarlyRiserOffer != null)
            //        {
            //            if (CurrentDate.Date.Day >= EarlyRiserOffer.OfferFromDt.Date.Day && CurrentDate.Date.Day <= EarlyRiserOffer.OfferToDt.Date.Day)
            //                objResponse.ResponseStatus = "Success";
            //            OfferDatePart = EarlyRiserOffer.OfferDatePart;
            //            OfferID = EarlyRiserOffer.AID;
            //            FreeProdIDs = EarlyRiserOffer.FreeProdIDs;FreeProdQtys = EarlyRiserOffer.FreeProdQty;
            //            confProdIDs = EarlyRiserOffer.ConfFreeProdIDs; ConfProdQtys = EarlyRiserOffer.ConfFreeProdQtys;
            //            offerbillvalue = EarlyRiserOffer.OfferOnValue.ToString();
            //        }
            //        else
            //        {
            //            monthOfferList = (from r in entity.VisionOffers where r.OfferDatePart == "R" && r.OfferOnValue <= objModel.objProduct.TotalNetPayable && r.OfferOnBV <= objModel.objProduct.TotalBV select r).OrderByDescending(o => o.OfferOnValue).FirstOrDefault();
            //            if (monthOfferList != null)
            //            {
            //                if (CurrentDate.Date >= monthOfferList.OfferFromDt.Date && CurrentDate.Date <= monthOfferList.OfferToDt.Date)
            //                    objResponse.ResponseStatus = "Success";
            //                OfferDatePart = monthOfferList.OfferDatePart;
            //                OfferID = monthOfferList.AID;
            //                FreeProdIDs = monthOfferList.FreeProdIDs; FreeProdQtys = monthOfferList.FreeProdQty;
            //                confProdIDs = monthOfferList.ConfFreeProdIDs; ConfProdQtys = monthOfferList.ConfFreeProdQtys;
            //                offerbillvalue = monthOfferList.OfferOnValue.ToString();
            //            }
            //        }
            //        if (objResponse.ResponseStatus == "Success")
            //        {
            //            var freeproduct = FreeProdIDs.Split(',');
            //            string productList = string.Empty;
            //            //string Quant = string.Empty;
            //            foreach (var prod in freeproduct)
            //            {
            //                var product = (from r in entity.M_ProductMaster where r.ProdId == prod select r).FirstOrDefault();
            //                productList += product.ProductName + "~" + prod + ",";
            //            }
            //            productList = productList.Substring(0, productList.Length - 1);
            //            string confproductList = ""; string confproductQtyList = "";
            //            if (confProdIDs != "")
            //            {
            //                string[] confproduct = confProdIDs.Split(',');
            //                string[] confproductQty = ConfProdQtys.Split(','); 
            //                for (int i = 0; i < confproduct.Length; i++)
            //                {
            //                    string ProdID_ = confproduct[i];
            //                    var product = (from r in entity.M_ProductMaster where r.ProdId == ProdID_ select r).FirstOrDefault();
            //                    confproductList += product.ProductName + "~" + confproduct[i] + ",";
            //                    confproductQtyList += confproductQty[i].ToString() + ",";
            //                }
            //                confproductList = confproductList.Substring(0, confproductList.Length - 1);
            //            }
            //            objResponse.ResponseMessage = OfferDatePart + "δ" + OfferID.ToString() + "δ" + productList + "δ" + FreeProdQtys.ToString() + "δ" + confproductList + "δ" + confproductQtyList + "δ" + offerbillvalue;
            //        }
            //        else
            //            objResponse.ResponseMessage = "NoOffer";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    objResponse.ResponseStatus = "FAILED";
            //    objResponse.ResponseMessage = "Something went wrong";
            //}
            objResponse.ResponseMessage = "NoOffer";
            return objResponse;
        }

        public List<OldBills> GetOldBills(string FromDate, string ToDate, string IdNo, string BillNo, string PartyCode)
        {
            List<OldBills> objOldBillList = new List<OldBills>();
            try
            {
                DateTime StartDate = DateTime.Now;
                DateTime EndDate = DateTime.Now;

                string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(AppConnectionString);

                using (var entity = new InventoryEntities())
                {
                    string WhereCondition = "";

                    if (!string.IsNullOrEmpty(IdNo) && IdNo != "0" && IdNo != "All")
                    {
                        WhereCondition = WhereCondition + "AND a.IDNo='" + IdNo + "'";
                    }

                    if (!string.IsNullOrEmpty(BillNo) && BillNo != "0" && BillNo != "All")
                    {
                        WhereCondition = WhereCondition + "AND a.BillNo='" + BillNo + "'";
                    }
                    if (PartyCode.ToUpper() != "ALL")
                    {
                        WhereCondition = WhereCondition + "AND b.PartyCode='" + PartyCode + "'";
                    }
                    if (!string.IsNullOrEmpty(FromDate) && (!string.IsNullOrEmpty(ToDate)))
                    {
                        if (FromDate != "All")
                        {
                            var SplitFromDate = FromDate.Split('-');
                            FromDate = SplitFromDate[1] + "-" + SplitFromDate[0] + "-" + SplitFromDate[2];
                            StartDate = Convert.ToDateTime(FromDate);
                        }
                        if (ToDate != "All")
                        {
                            var SplitToDate = ToDate.Split('-');
                            ToDate = SplitToDate[1] + "-" + SplitToDate[0] + "-" + SplitToDate[2];
                            EndDate = Convert.ToDateTime(ToDate);
                        }
                    }
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

                    string query = "Select  0 as Blank,a.BillNo,Replace(Convert(varchar,a.BillDate,106),' ','-') as ODte,a.FCode as IDNO,a.BVValue,a.PartyName as MemName,a.NetPayable as OrderAmt,a.Username,b.PartyName,CASE WHEN a.DRecTimeStamp is null THEN 'No' ELSE 'Deleted' END as IsDeleted From OldBillMain a,M_LedgerMaster b Where b.UserPartyCode=a.Username " + WhereCondition +
              " Order By BillDate DESC";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;

                    cmd.Connection = SC;
                    SC.Close();
                    SC.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            objOldBillList.Add(new OldBills
                            {
                                BillNo = reader["BillNo"].ToString(),
                                BillDateStr = reader["ODte"].ToString(),
                                BillDate = DateTime.Parse(reader["ODte"].ToString()),
                                FCode = reader["IDNo"].ToString(),
                                PartyName = reader["MemName"].ToString(),
                                NetPayable = decimal.Parse(reader["OrderAmt"].ToString()),
                                BVValue = decimal.Parse(reader["BVValue"].ToString()),
                                IsDeleted = reader["IsDeleted"].ToString(),
                                Username = reader["Username"].ToString(),
                            });
                        }
                    }


                }
            }
            catch (Exception ex)
            {

            }
            return objOldBillList;
        }

        public List<ProductModel> GetOldBillProducts(string BillNo)
        {
            List<ProductModel> objOrderProductModel = new List<ProductModel>();
            //try
            //{
            //    using (var entity = new InventoryEntities())
            //    {
            //        objOrderProductModel = (from r in entity.OLDBillDetails
            //                                where r.BillNo == BillNo.ToString()
            //                                select new ProductModel
            //                                {
            //                                    ProductName = r.ProductName,
            //                                    DP = r.DP,
            //                                    ProductCodeStr = r.ProductID,
            //                                    MRP = r.MRP,
            //                                    BV = r.PValue,
            //                                    Amount = r.NetAmount,
            //                                    DispQty = r.Qty
            //                                }
            //                              ).ToList();
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
            return objOrderProductModel;
        }

        public List<kit> GetKitList()
        {
            ResponseDetail objresponse = new ResponseDetail();
            List<kit> kitllist = new List<kit>();

            try
            {
                kitllist.Add(new kit
                {
                    kitName = "Select Kit",
                    kitId = 0,
                    productId = "0"
                });
                using (var entity = new InventoryEntities())
                {
                    var kitllistitem = (from r in entity.M_ProductMaster
                                        where r.ActiveStatus == "Y" && r.PType == "K"
                                        select new kit
                                        {
                                            kitId = r.BrandCode,
                                            kitName = r.ProductName,
                                            productId = r.ProdId
                                        }).ToList();
                    kitllist.AddRange(kitllistitem);
                }
            }
            catch (Exception ex)
            {
                objresponse.ResponseStatus = "FAILED";
                objresponse.ResponseMessage = "Something went wrong";
            }
            return (kitllist);
        }

        public List<PackUnpackProduct> GetPackUnpackProducts(string PackUnpack, decimal KitId, string prodID, string LoginPartyCode)
        {
            ResponseDetail objresponse = new ResponseDetail();
            List<PackUnpackProduct> kitproductlist = new List<PackUnpackProduct>();
            string db = System.Configuration.ConfigurationManager.AppSettings["Database"];
            string dbInv = System.Configuration.ConfigurationManager.AppSettings["INVDatabase"];
            try
            {
                string Sql = string.Empty;
                if (PackUnpack.ToLower() == "pack")
                {
                    Sql = "Select a.ProdID ,a.ProductName ,SUM(a.Qty ) as Qty ,SUM(b.AvailStock) as AvailStock FROM (";
                    Sql += " Select b.ProdID,b.ProductName,a.Qty,0 as AvailStock FROM " + db + "..M_KitProductDetail a,M_ProductMaster b ";
                    Sql += " WHERE a.ProdID=b.ProdID AND a.KItID=" + KitId + " AND a.RowStatus='Y' AND a.Qty>0 ) a ";
                    Sql += "Left Join";
                    Sql += "(Select b.ProdID,b.ProductName,0 Qty,SUM(a.Qty) as AvailStock FROM IM_CurrentStock a,M_ProductMaster b ";
                    Sql += " WHERE a.ProdID=b.ProdID AND a.FCode='" + LoginPartyCode + "' GROUP BY b.ProdID,b.ProductName) b";
                    Sql += " ON a.ProdID=b.ProdID";
                    Sql += " GROUP BY a.ProdID,a.ProductName";
                }
                else
                {
                    //Sql = "Select b.ProdID,b.ProductName,1 Qty,ISNULL(SUM(a.Qty),0) as AvailStock FROM IM_CurrentStock a RIGHT JOIN M_ProductMaster b ";
                    //Sql += " ON a.ProdID=b.ProdID AND a.FCode='" + LoginPartyCode + "' AND a.ProdID='" + prodID + "' GROUP BY b.ProdID,b.ProductName";
                    Sql = "Select b.ProdID,a.ProductName,1 Qty,ISNULL(AvailStock,0) as AvailStock  FROM M_ProductMaster a LEFT JOIN ( ";
                    Sql += "Select ProdID, ISNULL(SUM(a.Qty), 0) as AvailStock FROM IM_CurrentStock a WHERE a.FCode='" + LoginPartyCode + "' AND a.ProdID = '" + prodID + "'  GROUP BY ProdID) b ";
                    Sql += "ON a.ProdID = b.ProdID WHERE a.ProdID ='" + prodID + "' ";
                }

                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConnectionString);

                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = Sql;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();
                decimal NoOfKit = 0;
                decimal MaxKit = 10000;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NoOfKit = Convert.ToDecimal(reader["AvailStock"].ToString()) / Convert.ToDecimal(reader["Qty"].ToString());
                        MaxKit = Math.Min(NoOfKit, MaxKit);
                        PackUnpackProduct tempobj = new PackUnpackProduct();
                        tempobj.ProductId = reader["ProdID"] != null ? reader["ProdID"].ToString() : "";
                        tempobj.ProductName = reader["ProductName"] != null ? reader["ProductName"].ToString() : "";
                        tempobj.Qunatity = reader["Qty"] != null ? reader["Qty"].ToString() : "";
                        tempobj.AvailStock = reader["AvailStock"] != null ? reader["AvailStock"].ToString() : "";
                        kitproductlist.Add(tempobj);
                    }
                }

                foreach (PackUnpackProduct tempobj in kitproductlist)
                {
                    tempobj.MaxPack = Convert.ToInt32(MaxKit);
                }
            }
            catch (Exception ex)
            {
                objresponse.ResponseStatus = "FAILED";
                objresponse.ResponseMessage = "Something went wrong";
            }
            return (kitproductlist);
        }

        public ResponseDetail SavePackUnpack(PackUnpack obj)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseMessage = "Something went wrong!";
            objResponse.ResponseStatus = "FAILED";
            MakeKit objmakekit = new MakeKit();
            try
            {
                objmakekit.KitID = obj.kitId;
                objmakekit.KitName = obj.kitName;
                objmakekit.ActiveStatus = "Y";
                if (obj.PackOrUnpack.ToLower() == "pack")
                    objmakekit.ConvType = "B";
                else
                    objmakekit.ConvType = "U";
                objmakekit.UserID = Convert.ToDecimal(obj.UserId);
                objmakekit.RecTimeStamp = DateTime.Now;
                objmakekit.Qty = obj.qunatity;

                int i = 0;
                using (var entity = new InventoryEntities())
                {
                    decimal maxSbillNo = (from result in entity.MakeKits select result.ID).DefaultIfEmpty(0).Max();
                    maxSbillNo = maxSbillNo + 1;
                    objmakekit.ID = maxSbillNo;
                    entity.MakeKits.Add(objmakekit);


                    int fessid = Convert.ToInt32((from result in entity.M_FiscalMaster where result.ActiveStatus == "Y" select result.FSessId).DefaultIfEmpty(0).Max());
                    string version = (from result in entity.M_NewHOVersionInfo select result.VersionNo).FirstOrDefault();

                    decimal maxStockNo = (from result in entity.Im_CurrentStock select result.StockId).DefaultIfEmpty(0).Max();
                    maxStockNo = maxStockNo + 1;





                    Im_CurrentStock objkit = new Im_CurrentStock();

                    objkit.StockId = maxStockNo;
                    objkit.FSessId = fessid;
                    objkit.SupplierCode = "0";
                    objkit.StockDate = DateTime.Now;
                    objkit.RecTimeStamp = DateTime.Now;
                    if (obj.PackOrUnpack.ToLower() == "pack")
                    {
                        objkit.RefNo = "Pack Kit/" + maxSbillNo;
                        objkit.Qty = obj.qunatity;
                        objkit.BillType = "A";
                        objkit.Remarks = obj.qunatity + " Kits Packed.";
                    }
                    else
                    {
                        objkit.RefNo = "UnPack Kit/" + maxSbillNo;
                        objkit.Qty = (-1 * obj.qunatity);
                        objkit.BillType = "L";
                        objkit.Remarks = obj.qunatity + " Kits UnPacked.";

                        string db = System.Configuration.ConfigurationManager.AppSettings["Database"];
                        string Sql = " Select b.ProdID,b.ProductName,a.Qty " +
        "FROM " + db + "..M_KitProductDetail a, M_ProductMaster b WHERE a.ProdID = b.ProdID AND a.KItID = " + obj.kitId +
        " AND a.ActiveStatus = 'Y' AND a.RowStatus='Y' AND a.Qty>0";
                        string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                        SqlConnection SC = new SqlConnection(InvConnectionString);
                        SqlCommand cmd = new SqlCommand();

                        cmd.CommandText = Sql;
                        cmd.Connection = SC;
                        SC.Close();
                        SC.Open();
                        obj.productList.RemoveAt(0);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PackUnpackProduct objpackProd = new PackUnpackProduct();
                                objpackProd.ProductId = reader["ProdID"].ToString();
                                objpackProd.ProductName = reader["ProductName"].ToString();
                                objpackProd.Qunatity = reader["Qty"].ToString();
                                obj.productList.Add(objpackProd);
                            }
                        }

                    }
                    objkit.InvoiceNo = "";
                    objkit.FCode = obj.UserCode;
                    objkit.StockFor = obj.UserCode;
                    objkit.EntryBy = obj.UserCode;
                    objkit.GroupId = 0;
                    objkit.ActiveStatus = "Y";
                    objkit.BType = "P";
                    objkit.SType = "I";
                    objkit.ProdType = "P";

                    var barcodedetail = (from result in entity.M_ProductMaster where result.BrandCode == obj.kitId select result).FirstOrDefault();
                    objkit.ProdId = barcodedetail.ProdId;
                    objkit.Barcode = barcodedetail.Barcode;
                    // objkit.Rate = barcodedetail.PurchaseRate;
                    objkit.BatchCode = Convert.ToString(barcodedetail.BCode);
                    objkit.Version = version;
                    objkit.IsDisp = "N";
                    objkit.UserId = obj.UserId;

                    entity.Im_CurrentStock.Add(objkit);

                    foreach (var product in obj.productList)
                    {
                        maxStockNo = (from result in entity.Im_CurrentStock select result.StockId).DefaultIfEmpty(0).Max();
                        maxStockNo = maxStockNo + 1;

                        Im_CurrentStock objprod = new Im_CurrentStock();

                        objprod.StockId = maxStockNo;
                        objprod.FSessId = fessid;
                        objprod.SupplierCode = "0";
                        objprod.StockDate = DateTime.Now;
                        objprod.RecTimeStamp = DateTime.Now;
                        int prodqty = Convert.ToInt32(product.Qunatity);
                        if (obj.PackOrUnpack.ToLower() == "pack")
                        {
                            objprod.RefNo = "Pack Kit/" + maxSbillNo;
                            objprod.Qty = -1 * obj.qunatity * prodqty;
                            objprod.BillType = "L";
                            objprod.Remarks = obj.qunatity + " Kits Packed.";
                        }
                        else
                        {
                            objprod.RefNo = "UnPack Kit/" + maxSbillNo;
                            objprod.Qty = (obj.qunatity * prodqty);
                            objprod.BillType = "A";
                            objprod.Remarks = obj.qunatity + " Kits UnPacked.";
                        }
                        objprod.InvoiceNo = "";
                        objprod.FCode = obj.UserCode;
                        objprod.StockFor = obj.UserCode;
                        objprod.EntryBy = obj.UserCode;
                        objprod.UserId = obj.UserId;
                        objprod.GroupId = 0;
                        objprod.ActiveStatus = "Y";
                        objprod.BType = "P";
                        objprod.SType = "I";
                        objprod.ProdType = "P";
                        objprod.ProdId = product.ProductId;
                        var prodbarcodedetail = (from result in entity.M_BarCodeMaster where result.ProdId == product.ProductId select result).FirstOrDefault();
                        objprod.Barcode = prodbarcodedetail.BarCode;
                        var prodDetail = (from result in entity.M_ProductMaster where result.ProdId == product.ProductId select result).FirstOrDefault();
                        // objprod.Rate = prodDetail.PurchaseRate;
                        objprod.BatchCode = Convert.ToString(prodbarcodedetail.BCode);
                        objprod.Version = version;
                        objprod.IsDisp = "N";
                        entity.Im_CurrentStock.Add(objprod);
                    }

                    i = entity.SaveChanges();
                }

                if (i > 0)
                {
                    objResponse.ResponseMessage = "Saved Successfully!";
                    objResponse.ResponseStatus = "OK";
                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseMessage = "Something went wrong!";
                objResponse.ResponseStatus = "FAILED";
            }
            return objResponse;
        }

        public List<OfferProducts> GetOfferProductList(decimal OfferId, int offerType)
        {
            List<OfferProducts> objProductNames = new List<OfferProducts>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    if (offerType == 9999)
                    {
                        objProductNames = (from r in entity.M_OfferOtherProducts
                                           where r.OfferID == OfferId && r.IsBuyProduct.ToUpper() != "Y"
                                           join
                                            result in entity.M_ProductMaster on r.ProdID equals result.ProdId
                                           where result.ActiveStatus == "Y" && result.IsCardIssue == "N" //&& result.PType != "K"
                                           select new OfferProducts
                                           {
                                               ProdID = r.ProdID,
                                               ProdName = result.ProductName,
                                               Qty = r.Qty
                                           }).ToList();
                    }
                    else
                    {
                        objProductNames = (from r in entity.M_OfferProducts
                                           where r.OfferID == OfferId && r.IsBuyProduct.ToUpper() != "Y"
                                           join
                                            result in entity.M_ProductMaster on r.ProdID equals result.ProdId
                                           where result.ActiveStatus == "Y" && result.IsCardIssue == "N" //&& result.PType != "K"
                                           select new OfferProducts
                                           {
                                               ProdID = r.ProdID,
                                               ProdName = result.ProductName,
                                               Qty = r.Qty
                                           }).ToList();
                    }
                }
            }
            catch (Exception e)
            {

            }
            return objProductNames;
        }

        public List<OfferProducts> GetOfferBuyProducts(decimal OfferId)
        {
            List<OfferProducts> objProductNames = new List<OfferProducts>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objProductNames = (from r in entity.M_OfferProducts
                                       where r.OfferID == OfferId && r.IsBuyProduct.ToUpper() == "Y"
                                       join result in entity.M_ProductMaster on r.ProdID equals result.ProdId
                                       select new OfferProducts
                                       {
                                           ProdID = r.ProdID,
                                           ProdName = result.ProductName,
                                           Qty = r.Qty
                                       }).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return objProductNames;
        }

        public bool CanUserAccessMenu(int UserID, string MenuFile)
        {
            bool UserCanAcess = false;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    var result = (from r in entity.Web_M_MenuMaster
                                  join s in entity.Web_M_UserPermissionMaster on r.MenuId equals s.MenuId
                                  where s.GroupId == UserID
                                  select r).ToList();

                    var config = entity.M_ConfigMaster.FirstOrDefault();


                    foreach (var obj in result)
                    {
                        string[] onselect = obj.OnSelect.Split('/');
                        if (onselect.Length > 1)
                            if (onselect[1] == MenuFile)
                            {
                                UserCanAcess = true;
                                break;
                            }
                    }

                    if (MenuFile.ToLower() == "upgradeid" && config.CanIDBeUpgraded == "N")
                    {
                        UserCanAcess = false;
                    }

                }

            }
            catch (Exception)
            {
            }
            return UserCanAcess;
        }

        public ResponseDetail SaveOffer(Offer offerDetail)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                M_Offers offer = new M_Offers();
                int MaxId = 0;

                using (var entity = new InventoryEntities())
                {
                    if (offerDetail.Action.ToLower() == "edit")
                    {
                        offer = (from r in entity.M_Offers where r.AID == offerDetail.OfferID select r).FirstOrDefault();
                        MaxId = offer.AID;
                        var prodList = (from r in entity.M_OfferProducts where r.OfferID == offerDetail.OfferID select r).ToList();
                        foreach (var obj in prodList)
                        {
                            entity.M_OfferProducts.Remove(obj);
                        }
                    }
                    else
                    {
                        MaxId = (from r in entity.M_Offers select r.AID).DefaultIfEmpty(0).Max();
                        MaxId += 1;
                        offer.AID = MaxId;
                    }
                    offer.ActiveStatus = offerDetail.ActiveStatus;
                    offer.ForNewIds = offerDetail.ForNewIds;
                    offer.IdDate = offerDetail.IdDate;
                    offer.OfferDatePart = "R";
                    offer.OfferOnBV = offerDetail.OfferOnBV;
                    offer.OfferOnToBV = offerDetail.OfferOnToBV;
                    offer.OfferOnValue = offerDetail.OfferOnValue;
                    offer.OfferToDt = offerDetail.OfferToDt;
                    offer.IdStatus = offerDetail.IdStatus;
                    offer.OfferFromDt = offerDetail.OfferFromDt;
                    offer.TotalQty = offerDetail.TotalQty;
                    offer.ForBillType = offerDetail.ForBillType;
                    offer.ForFranchise = string.IsNullOrEmpty(offerDetail.Party) ? "all" : offerDetail.Party;
                    offer.OfferType = offerDetail.offerType;
                    offer.OfferName = offerDetail.OfferName;
                    offer.IsFixedQty = "N";
                    offer.FixedQty = 0;

                    if (offerDetail.Action.ToLower() != "edit")
                    {
                        offer.UserID = offerDetail.CreatedBy;
                        offer.RecTimeStamp = DateTime.Now;
                        entity.M_Offers.Add(offer);
                    }
                    foreach (var product in offerDetail.OfferProds)
                    {
                        M_OfferProducts offerProduct = new M_OfferProducts();
                        offerProduct.OfferID = MaxId;
                        offerProduct.ActiveStatus = "Y";
                        offerProduct.ProdID = product.ProdID;
                        offerProduct.RectimeStamp = DateTime.Now;
                        offerProduct.Qty = product.Qty;
                        offerProduct.FreeQty = product.FreeQty;
                        offerProduct.IsFlexible = String.IsNullOrEmpty(product.IsFlexible) ? "N" : product.IsFlexible;
                        offerProduct.OfferMrp = product.OfferMrp;
                        offerProduct.IsBuyProduct = product.BuyProduct;

                        entity.M_OfferProducts.Add(offerProduct);
                    }
                    int i = entity.SaveChanges();
                    if (i > 0)
                    {
                        objResponse.ResponseStatus = "OK";
                    }
                    else
                    {
                        objResponse.ResponseMessage = "Something went wrong.";
                    }

                }
            }
            //catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            //{
            //    Exception raise = dbEx;
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            string message = string.Format("{0}:{1}",
            //                validationErrors.Entry.Entity.ToString(),
            //                validationError.ErrorMessage);
            //            // raise a new exception nesting  
            //            // the current instance as InnerException  
            //            raise = new InvalidOperationException(message, raise);
            //        }
            //    }
            //    throw raise;
            //}
            catch (Exception e)
            {
                objResponse.ResponseMessage = "Something went wrong.";
            }
            return objResponse;
        }

        public List<ProductModel> GetproductInfobyCode(string data)
        {
            List<ProductModel> objProductModel = new List<ProductModel>();
            List<ProductModel> TempResult = new List<ProductModel>();
            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    using (var entity = new InventoryEntities())
                    {
                        objProductModel = (from product in entity.M_ProductMaster
                                           where product.ProdId.Equals(data) && product.ActiveStatus == "Y"
                                           join barcode in entity.M_BarCodeMaster on product.ProdId equals barcode.ProdId
                                           select new ProductModel
                                           {
                                               ProductName = product.ProductName
                                           }).ToList();

                    }

                }
            }
            catch (Exception e)
            {

            }

            return objProductModel;
        }

        public List<Offer> GetAllOfferList(decimal OfferType)
        {
            List<Offer> offerList = new List<Offer>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    offerList = (from r in entity.M_Offers
                                 where r.OfferType == OfferType
                                 select new Offer
                                 {
                                     OfferID = r.AID,
                                     OfferFromDt = r.OfferFromDt,
                                     OfferToDt = r.OfferToDt,
                                     OfferOnBV = r.OfferOnBV,
                                     OfferOnToBV = r.OfferOnToBV,
                                     OfferOnValue = r.OfferOnValue,
                                     OfferDatePart = r.OfferDatePart,
                                     ActiveStatus = r.ActiveStatus,
                                     ForNewIds = r.ForNewIds,
                                     IdDate = r.IdDate,
                                     IdStatus = r.IdStatus,
                                     ForBillType = r.ForBillType,
                                     RecTimeStamp = r.RecTimeStamp,
                                     OfferName = r.OfferName,
                                     IsFixedQty = r.IsFixedQty,
                                     FixedQty = r.FixedQty

                                 }).OrderByDescending(r => r.RecTimeStamp).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return (offerList);
        }

        public Offer GetSelectedOfferDetails(decimal offerID)
        {
            Offer offer = new Offer();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    offer = (from r in entity.M_Offers
                             where r.AID == offerID
                             select new Offer
                             {
                                 OfferID = r.AID,
                                 OfferName = r.OfferName,
                                 OfferFromDt = r.OfferFromDt,
                                 OfferToDt = r.OfferToDt,
                                 OfferOnBV = r.OfferOnBV,
                                 OfferOnToBV = r.OfferOnToBV,
                                 OfferOnValue = r.OfferOnValue,
                                 OfferDatePart = r.OfferDatePart,
                                 ActiveStatus = r.ActiveStatus,
                                 ForNewIds = r.ForNewIds,
                                 IdDate = r.IdDate,
                                 IdStatus = r.IdStatus,
                                 ForBillType = r.ForBillType,
                                 Party = r.ForFranchise,
                                 IsFixedQty = r.IsFixedQty,
                                 FixedQty = r.FixedQty,
                             }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {

            }
            return (offer);
        }

        public List<OfferProducts> GetSelectedOfferProductList(decimal OfferId, int type)
        {

            List<OfferProducts> objProductNames = new List<OfferProducts>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    if (type == 9999)
                    {
                        objProductNames = (from r in entity.M_OfferOtherProducts
                                           where r.OfferID == OfferId && r.IsBuyProduct != "Y"
                                           join result in entity.M_ProductMaster on r.ProdID equals result.ProdId
                                           select new OfferProducts
                                           {
                                               ProdID = r.ProdID,
                                               FreeQty = r.FreeQty,
                                               Qty = r.Qty,
                                               offerID = r.OfferID,
                                               IsFlexible = r.IsFlexible,
                                               ProdName = result.ProductName,
                                               OfferMrp = r.OfferMrp,
                                               BuyProduct = r.IsBuyProduct
                                           }).ToList();
                    }
                    else
                    {
                        objProductNames = (from r in entity.M_OfferProducts
                                           where r.OfferID == OfferId && r.IsBuyProduct != "Y"
                                           join result in entity.M_ProductMaster on r.ProdID equals result.ProdId
                                           select new OfferProducts
                                           {
                                               ProdID = r.ProdID,
                                               FreeQty = r.FreeQty,
                                               Qty = r.Qty,
                                               offerID = r.OfferID,
                                               IsFlexible = r.IsFlexible,
                                               ProdName = result.ProductName,
                                               OfferMrp = r.OfferMrp,
                                               BuyProduct = r.IsBuyProduct
                                           }).ToList();
                    }
                }
            }
            catch (Exception e)
            {

            }
            return objProductNames;
        }

        public List<Offer> GetAllOtherOfferList()
        {
            List<Offer> offerList = new List<Offer>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    offerList = (from r in entity.M_OtherOffers
                                 select new Offer
                                 {
                                     OfferID = r.OfferID,
                                     OfferFromDt = r.FromDate,
                                     OfferToDt = r.ToDate,
                                     OfferOnBV = r.MinBV,
                                     OfferOnToBV = r.OfferOnToBV,
                                     OfferName = r.OfferName,
                                     StartProduct = r.StartProduct,
                                     ActiveStatus = r.ActiveStatus
                                 }).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return (offerList);
        }

        public Offer GetSelectedOtherOfferDetails(decimal offerID)
        {
            Offer offer = new Offer();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    offer = (from r in entity.M_OtherOffers
                             where r.OfferID == offerID
                             select new Offer
                             {
                                 OfferID = r.OfferID,
                                 OfferFromDt = r.FromDate,
                                 OfferToDt = r.ToDate,
                                 OfferOnBV = r.MinBV,
                                 OfferOnToBV = r.OfferOnToBV,
                                 OfferName = r.OfferName,
                                 StartProduct = r.StartProduct,
                                 ForBillType = r.Forbill,
                                 ActiveStatus = r.ActiveStatus
                             }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {

            }
            return (offer);
        }

        public ResponseDetail SaveOtherOffer(Offer offerDetail)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                M_OtherOffers offer = new M_OtherOffers();
                int MaxId = 0;
                int maxOfferId = 0;
                using (var entity = new InventoryEntities())
                {
                    if (offerDetail.Action.ToLower() == "edit")
                    {
                        offer = (from r in entity.M_OtherOffers where r.OfferID == offerDetail.OfferID select r).FirstOrDefault();
                        MaxId = offer.AID;
                        maxOfferId = offer.OfferID;

                        var prodList = (from r in entity.M_OfferOtherProducts where r.OfferID == offerDetail.OfferID select r).ToList();
                        foreach (var obj in prodList)
                        {
                            entity.M_OfferOtherProducts.Remove(obj);
                        }
                    }
                    else
                    {
                        MaxId = (from r in entity.M_OtherOffers select r.AID).Max();
                        MaxId = MaxId + 1;

                        maxOfferId = (from r in entity.M_OtherOffers select r.OfferID).Max();
                        maxOfferId = maxOfferId + 1;
                    }
                    offer.OfferID = maxOfferId;
                    offer.AID = MaxId;
                    offer.ActiveStatus = offerDetail.ActiveStatus;
                    offer.MinBV = offerDetail.OfferOnBV;
                    offer.OfferOnToBV = offerDetail.OfferOnToBV;
                    offer.StartProduct = offerDetail.StartProduct;
                    offer.ToDate = offerDetail.OfferToDt;
                    offer.FromDate = offerDetail.OfferFromDt;
                    offer.OfferName = offerDetail.OfferName;
                    offer.ForFranchise = string.IsNullOrEmpty(offerDetail.Party) ? "all" : offerDetail.Party;
                    offer.Forbill = offerDetail.ForBillType;
                    offer.RecTimeStamp = DateTime.Now;
                    if (offerDetail.Action.ToLower() != "edit")
                    {
                        entity.M_OtherOffers.Add(offer);
                    }

                    foreach (var product in offerDetail.OfferProds)
                    {
                        M_OfferOtherProducts offerProduct = new M_OfferOtherProducts();
                        offerProduct.OfferID = maxOfferId;
                        offerProduct.ActiveStatus = "Y";
                        offerProduct.ProdID = product.ProdID;
                        offerProduct.RectimeStamp = DateTime.Now;
                        offerProduct.Qty = product.Qty;
                        offerProduct.FreeQty = product.FreeQty;
                        offerProduct.IsFlexible = String.IsNullOrEmpty(product.IsFlexible) ? "N" : product.IsFlexible;
                        offerProduct.OfferMrp = product.OfferMrp;
                        offerProduct.IsBuyProduct = product.BuyProduct;
                        entity.M_OfferOtherProducts.Add(offerProduct);
                    }

                    int i = entity.SaveChanges();
                    if (i > 0)
                    {
                        objResponse.ResponseStatus = "OK";
                        objResponse.ResponseMessage = "Saved Successfully.";
                    }
                    else
                    {
                        objResponse.ResponseStatus = "FAILED";
                        objResponse.ResponseMessage = "Something went wrong.";
                    }

                }
            }
            catch (Exception e)
            {
                objResponse.ResponseMessage = "Something went wrong.";
            }
            return objResponse;
        }

        public List<DrawProds> getDrawProductList()
        {
            List<DrawProds> offerList = new List<DrawProds>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    offerList = (from r in entity.DrawProducts
                                 where r.RowStatus == "Y"
                                 select
        new DrawProds
        {
            ProdCode = r.ProdCode,
            ProductName = r.ProductName,
            ActiveStatus = r.ActiveStatus,
            RowStatus = r.RowStatus
            //IsActive = r.ActiveStatus == "Y"?true:false
        }).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return (offerList);
        }

        public List<Draw> getAllDraws()
        {
            List<Draw> offerList = new List<Draw>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    offerList = (from r in entity.FixedDraws
                                 select new Draw
                                 {
                                     AID = r.AID,
                                     ProdCode = r.ProdCode,
                                     ProductName = r.ProductName,
                                     FDate = r.FDate,
                                     BillSeq = r.BillSeq
                                 }).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return (offerList);
        }

        public ResponseDetail SaveDraw(Draw offerDetail)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                FixedDraw draw = new FixedDraw();


                var SplitDate = offerDetail.FdateStr.Split('-');
                string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                var BillDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                draw.FDate = BillDate;
                draw.BillSeq = offerDetail.BillSeq;
                draw.ProdCode = offerDetail.ProdCode;
                draw.ProductName = offerDetail.ProductName;
                draw.RecTimeStamp = DateTime.Now;

                using (var entity = new InventoryEntities())
                {
                    entity.FixedDraws.Add(draw);
                    int i = entity.SaveChanges();
                    if (i > 0)
                    {
                        objResponse.ResponseStatus = "OK";
                        objResponse.ResponseMessage = "Saved Successfully.";
                    }
                    else
                    {
                        objResponse.ResponseStatus = "FAILED";
                        objResponse.ResponseMessage = "Something went wrong.";
                    }
                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseMessage = ex.InnerException.InnerException.Message;
                objResponse.ResponseStatus = "FAILED";

            }
            return objResponse;
        }

        public ResponseDetail DeleteDraw(Draw model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                ShiftDataintoTemptable("FixedDraw", "TempFixedDraw", ",'" + model.UserID + "',Getdate()", " AND AID='" + model.AID + "'");
                using (var entity = new InventoryEntities())
                {
                    var obj = (from r in entity.FixedDraws where r.AID == model.AID select r).FirstOrDefault();
                    if (obj != null)
                    {
                        entity.FixedDraws.Remove(obj);
                        entity.SaveChanges();
                        objResponse.ResponseMessage = "Saved Successfully!";
                        objResponse.ResponseStatus = "OK";
                    }
                    else
                    {
                        objResponse.ResponseMessage = "Not Available";
                        objResponse.ResponseStatus = "FAILED";
                    }
                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseMessage = "Saved Successfully!";
                objResponse.ResponseStatus = "FAILED";
            }
            return objResponse;
        }

        private void ShiftDataintoTemptable(string TblName, string TempTblName, string columns, string wherecond)
        {
            try
            {
                string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                using (SqlConnection SC = new SqlConnection(AppConnectionString))
                {
                    string query = "INSERT " + TempTblName + " Select *" + columns + " FROM " + TblName + " WHERE 1=1 " + wherecond;
                    SC.Open();
                    SqlCommand cmd = new SqlCommand(query, SC);
                    cmd.ExecuteNonQuery();
                    SC.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public ResponseDetail SaveDrawPoducts(List<DrawProds> products)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    var list = (from r in entity.DrawProducts where r.RowStatus == "Y" select r).ToList();
                    foreach (var record in list)
                    {
                        record.RowStatus = "N";
                    }

                    foreach (var record in products)
                    {
                        DrawProduct prod = new DrawProduct();
                        prod.RowStatus = "Y";
                        prod.ProductName = record.ProductName;
                        prod.ProdCode = record.ProdCode;
                        prod.ActiveStatus = record.IsActive ? "Y" : "N";
                        prod.RecTimeStamp = DateTime.Now;
                        entity.DrawProducts.Add(prod);
                    }
                    int i = entity.SaveChanges();
                    if (i > 0)
                    {
                        objResponse.ResponseMessage = "Saved Successfully";
                        objResponse.ResponseStatus = "OK";
                    }
                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseMessage = ex.InnerException.InnerException.Message;
                objResponse.ResponseStatus = "FAILED";

            }
            return objResponse;
        }

        public ResponseDetail GetFreeVoucherDetail(string VoucherNo, string IdNo)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.StatusCode = 0;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                    SqlConnection SC = new SqlConnection(InvConnectionString);
                    string query = "Select  * FROM TrnFPVCoupon WHERE CouponNo=@CouponNo AND IdNo=@IdNo AND Cast(Getdate() as Date) <=Cast(ExpiryDate as Date) ";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@CouponNo", VoucherNo);
                    cmd.Parameters.AddWithValue("@IdNo", IdNo);
                    cmd.Connection = SC;
                    SC.Close();
                    SC.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["IsUsed"].ToString().ToUpper() == "Y")
                            {
                                objResponse.ResponseStatus = "FAILED";
                                objResponse.ResponseMessage = "Voucher is already used on " + reader["UsedDate"].ToString() + ".";
                            }
                            else
                            {
                                objResponse.ResponseStatus = "OK";
                                objResponse.StatusCode = Convert.ToInt32(reader["ProdBunchID"].ToString());
                                objResponse.ResponseMessage = reader["CouponAmt"].ToString();
                                // objResponse.ResponseMessage = "success.";
                            }
                        }
                        else
                        {
                            objResponse.ResponseStatus = "FAILED";
                            objResponse.ResponseMessage = "Invalid Voucher No.";
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return objResponse;
        }

        public ResponseDetail GetFreeCPVVoucherDetail(string VoucherNo, string IdNo)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.StatusCode = 0;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                    SqlConnection SC = new SqlConnection(InvConnectionString);
                    string query = "Select  * FROM TrnCPVCoupon WHERE CouponNo=@CouponNo AND IdNo=@IdNo AND Cast(Getdate() as Date) <=Cast(ExpiryDate as Date) ";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@CouponNo", VoucherNo);
                    cmd.Parameters.AddWithValue("@IdNo", IdNo);
                    cmd.Connection = SC;
                    SC.Close();
                    SC.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["IsUsed"].ToString().ToUpper() == "Y")
                            {
                                objResponse.ResponseStatus = "FAILED";
                                objResponse.ResponseMessage = "Voucher is already used on " + reader["UsedDate"].ToString() + ".";
                            }
                            else
                            {
                                objResponse.ResponseStatus = "OK";
                                objResponse.StatusCode = Convert.ToInt32(reader["ProdBunchID"].ToString());
                                objResponse.ResponseMessage = reader["CouponAmt"].ToString();
                                // objResponse.ResponseMessage = "success.";
                            }
                        }
                        else
                        {
                            objResponse.ResponseStatus = "FAILED";
                            objResponse.ResponseMessage = "Invalid Voucher No.";
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return objResponse;
        }

        public ResponseDetail SaveIssueSampleProducts(DistributorBillModel products)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                decimal maxJNo = 0;
                decimal? FsessId = 0;
                string JvNo = "";
                string version = "";
                using (var entity = new InventoryEntities())
                {

                    FsessId = (from result in entity.M_FiscalMaster where result.ActiveStatus == "Y" select result.FSessId).DefaultIfEmpty(0).Max();
                    DateTime CurrentJvDate = DateTime.Now;

                    var maxrecord = (from result in entity.TrnSampleProducts select result).OrderByDescending(o => o.RecTimeStamp).Take(1).FirstOrDefault();
                    var maxSbillNo = (decimal)0;
                    if (maxrecord != null)
                    {
                        maxSbillNo = Convert.ToDecimal(maxrecord.TransNo);
                    }

                    maxSbillNo += 1;
                    string strMaxUserSBillNo = maxSbillNo.ToString();
                    if (strMaxUserSBillNo.Count() < 4)
                    {
                        var countNum = strMaxUserSBillNo.Count();
                        var ToBeAddedDigits = 4 - countNum;
                        for (var j = 0; j < ToBeAddedDigits; j++)
                        {
                            strMaxUserSBillNo = "0" + strMaxUserSBillNo;
                        }
                    }
                    JvNo = "Less/" + strMaxUserSBillNo;
                    version = (from result in entity.M_NewHOVersionInfo select result.VersionNo).FirstOrDefault();
                    foreach (var sample in products.objListProduct)
                    {
                        TrnSampleProduct sampleprod = new TrnSampleProduct();
                        sampleprod.ActiveStatus = "Y";
                        sampleprod.RecTimeStamp = DateTime.Now;
                        sampleprod.UserID = products.objCustomer.UserDetails.UserId;
                        sampleprod.TransNo = strMaxUserSBillNo.ToString();

                        var SplitDate = products.BillDateStr.Split('-');
                        string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                        var BillDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                        sampleprod.TransDate = BillDate;
                        sampleprod.Remarks = products.objCustomer.Remarks ?? "";
                        sampleprod.SoldBy = "";
                        sampleprod.PartyName = products.objCustomer.PartyName ?? "";
                        sampleprod.ProdID = sample.ProductCodeStr;
                        sampleprod.ProductName = sample.ProductName;
                        sampleprod.Barcode = sample.Barcode ?? "";
                        sampleprod.BatchNo = sample.BatchNo ?? "";
                        sampleprod.Qty = sample.Quantity;
                        sampleprod.RefNo = products.objCustomer.ReferenceIdNo ?? "";


                        entity.TrnSampleProducts.Add(sampleprod);



                        Im_CurrentStock objCurrentStock = new Im_CurrentStock();
                        objCurrentStock.FSessId = FsessId ?? 0;
                        objCurrentStock.SupplierCode = "0";
                        objCurrentStock.StockDate = CurrentJvDate;
                        objCurrentStock.RefNo = JvNo;
                        objCurrentStock.FCode = products.objCustomer.UserDetails.FCode;
                        objCurrentStock.GroupId = products.objCustomer.UserDetails.GroupId;
                        objCurrentStock.ProdId = sample.ProductCodeStr;
                        objCurrentStock.BatchCode = sample.BatchNo;
                        objCurrentStock.Barcode = sample.Barcode;

                        objCurrentStock.SType = "O";
                        objCurrentStock.Qty = -(sample.Quantity);
                        objCurrentStock.BType = "L";
                        objCurrentStock.Remarks = "Stock Lessed";
                        objCurrentStock.BillType = "L";

                        objCurrentStock.ActiveStatus = "Y";
                        objCurrentStock.EntryBy = products.objCustomer.UserDetails.PartyCode;
                        objCurrentStock.StockFor = products.objCustomer.UserDetails.FCode;
                        objCurrentStock.RecTimeStamp = DateTime.Now;
                        objCurrentStock.UserId = products.objCustomer.UserDetails.UserId;
                        objCurrentStock.Version = version;
                        objCurrentStock.IsDisp = "N";
                        objCurrentStock.InvoiceNo = "";
                        objCurrentStock.ProdType = "P";
                        objCurrentStock.DispQty = 0;

                        entity.Im_CurrentStock.Add(objCurrentStock);

                    }

                    int i = entity.SaveChanges();
                    if (i > 0)
                    {
                        objResponse.ResponseMessage = "Saved Successfully";
                        objResponse.ResponseStatus = "OK";
                    }
                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseMessage = ex.InnerException.InnerException.Message;
                objResponse.ResponseStatus = "FAILED";

            }
            return objResponse;
        }

        public ResponseDetail CheckCustomer(string mobile)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseMessage = "Not Exist";
            objResponse.ResponseStatus = "OK";
            try
            {
                //using (var entity = new InventoryEntities())
                //{
                //    var data = (from r in entity.TrnCustomerDetails where r.MobileNo == objmodel.objCustomer.MobileNo select r).FirstOrDefault();
                //    if (data != null)
                //    {
                //        objResponse.ResponseMessage = data.CustomerName;
                //        objResponse.ResponseStatus = "OK";
                //    }
                //    else
                //    {
                //        objResponse.ResponseMessage = "Not Exist";
                //        objResponse.ResponseStatus = "OK";
                //    }
                //}


                string db = System.Configuration.ConfigurationManager.AppSettings["Database"];
                string dbInv = System.Configuration.ConfigurationManager.AppSettings["INVDatabase"];


                string sql = "Select  * From " + db + "..M_MemberMaster where Mobl = '" + mobile + "'";

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
                        objResponse.ResponseMessage = reader["IdNo"] != null ? reader["IdNo"].ToString() : "";
                        objResponse.ResponseStatus = "OK";
                    }
                }

            }
            catch (Exception ex)
            {
                objResponse.ResponseMessage = ex.InnerException.InnerException.Message;
                objResponse.ResponseStatus = "FAILED";

            }
            return objResponse;
        }

        public UpgradeID GetCustomerKitDetail(string IdNo, bool IsHO)
        {
            UpgradeID objCustomerDetail = new UpgradeID();
            if (!(string.IsNullOrEmpty(IdNo)))
            {
                try
                {

                    string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                    SqlConnection SC = new SqlConnection(AppConnectionString);

                    string query = "select a.Mobl,a.FormNo,a.MemFirstName+' '+ a.MemLastName as Name,A.Imported,a.KitId,a.IDno as IDno,a.Address1+','+a.Address2+','+a.City as Address,a.StateCode as StateCode,a.ActiveStatus as ActiveStatus,a.IsBlock as IsBlock,b.idno as RefId,b.MemFirstName+' '+ b.MemLastName as RefName FROM M_MemberMaster a,M_MemberMaster b WHERE a.RefFormNo=b.FormNo AND a.IDno=@IdNo";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@IdNo", IdNo);
                    cmd.Connection = SC;
                    SC.Close();
                    SC.Open();
                    string ActiveStatus = "Y";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            objCustomerDetail.MemberName = reader["Name"] != null ? reader["Name"].ToString() : "";
                            objCustomerDetail.KitId = reader["KitId"] != null ? int.Parse(reader["KitId"].ToString()) : 0;
                            ActiveStatus = reader["ActiveStatus"].ToString();
                            objCustomerDetail.BillType = "B"; //05Jun19 ActiveStatus == "Y" ? "R" : "B";
                        }
                        else
                        {
                            objCustomerDetail = new UpgradeID();
                            objCustomerDetail.IDno = "Record does not exists!";
                            objCustomerDetail.MemberName = "";
                        }
                    }
                    SC.Close();
                    if (objCustomerDetail != null)
                    {
                        decimal Ktamt = 0;
                        string KitName = "";
                        string MacAdres = "";
                        decimal BV = 0;
                        decimal TopupSeq = 0;
                        query = "select * FROM M_KitMaster WHERE KitId=" + objCustomerDetail.KitId;
                        // + " WHERE ActiveStatus='Y' AND IsBill='Y'" ;
                        cmd = new SqlCommand();
                        cmd.CommandText = query;
                        cmd.Connection = SC;
                        SC.Close();
                        SC.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                KitName = reader["KitName"] != null ? reader["KitName"].ToString() : "";
                                Ktamt = reader["KitAmount"] != null ? decimal.Parse(reader["KitAmount"].ToString()) : 0;
                                MacAdres = reader["MacAdrs"] != null ? reader["MacAdrs"].ToString() : "";
                                BV = reader["BV"] != null ? decimal.Parse(reader["BV"].ToString()) : 0;
                                TopupSeq = reader["TopupSeq"] != null ? decimal.Parse(reader["TopupSeq"].ToString()) : 0;
                                break;
                            }
                        }

                        objCustomerDetail.KitAmount = Ktamt;
                        objCustomerDetail.KitName = KitName;
                        objCustomerDetail.MacAdres = MacAdres;
                        objCustomerDetail.KitBV = BV;


                        //if (ActiveStatus == "N" || IsHO==true) //Cmnted on 04Jun19
                        //{
                        query = "Select * FROM M_KitMaster WHERE TopupSeq>'" + TopupSeq + "' AND ActiveStatus='Y' AND IsBill='Y' AND RowStatus='Y'";
                        // query = "Select * FROM M_KitMaster WHERE BV>" + BV + " AND ActiveStatus='Y' AND IsBill='Y' AND RowStatus='Y'";

                        cmd = new SqlCommand();
                        cmd.CommandText = query;
                        cmd.Connection = SC;
                        SC.Close();
                        SC.Open();
                        objCustomerDetail.NewKitList = new List<kits>();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                kits newkit = new kits();
                                newkit.kitName = reader["KitName"] != null ? reader["KitName"].ToString() : "";
                                newkit.kitId = reader["KitId"] != null ? int.Parse(reader["KitId"].ToString()) : 0;
                                objCustomerDetail.NewKitList.Add(newkit);
                            }
                        }
                        //}
                    }
                }
                catch (Exception e)
                {
                    objCustomerDetail = new UpgradeID();
                    objCustomerDetail.IDno = "Something went wrong!";
                    objCustomerDetail.MemberName = "";
                }
            }

            return objCustomerDetail;
        }

        public UpgradeID GetKitProductList(string kitId)
        {
            UpgradeID objCustomerDetail = new UpgradeID();
            objCustomerDetail.objListProduct = new List<ProductModel>();
            string query = "";
            if (!(string.IsNullOrEmpty(kitId)))
            {
                try
                {

                    string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                    string db = System.Configuration.ConfigurationManager.AppSettings["Database"];
                    string dbInv = System.Configuration.ConfigurationManager.AppSettings["INVDatabase"];
                    SqlConnection SC = new SqlConnection(AppConnectionString);
                    SqlCommand cmd = new SqlCommand();
                    string promoID = "";
                    query = "select * FROM M_KitMaster WHERE KitId=" + kitId;
                    cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = SC;
                    SC.Close();
                    SC.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            promoID = "0";// reader["PromoId"] != null ? reader["PromoId"].ToString() : "";
                            objCustomerDetail.promoId = Convert.ToInt16(promoID);
                            objCustomerDetail.KitAmount = reader["KitAmount"] != null ? Convert.ToDecimal(reader["KitAmount"].ToString()) : 0;
                            break;
                        }
                    }

                    decimal CompStateCode = 0;
                    using (var entity = new InventoryEntities())
                    {
                        CompStateCode = (from r in entity.M_CompanyMaster select r.CompState).FirstOrDefault();
                    }
                    promoID = "1";
                    query = "Select a.ProdId,a.ProductName,'1' as Qty,a.DP as DP1,CASE WHEN " + promoID + " = 0 THEN CAST((b.KitAmount * 100 / (100 + t.VatTax)) as numeric(18, 2)) ELSE 0 END as Rate,t.VatTax,CASE WHEN " + promoID + " = 0 THEN CAST((b.KitAmount * 100 / (100 + t.VatTax)) as numeric(18, 2)) ELSE '0' END as NetAmount,CASE WHEN " + promoID + " = 0 THEN b.KitAmount - CAST((b.KitAmount * 100 / (100 + t.VatTax)) as numeric(18, 2)) ELSE '0' END as TaxAmount,'N' as DispStatus,";
                    query += " a.MRP,a.DP,b.RP as RP,CASE WHEN " + promoID + " = 0 THEN b.bv else 0 END as bv,0 as Discount,'P' as ProdType,c.Barcode,a.Cv,a.PV from " + dbInv + "..M_ProductMaster as a, " + db + "..M_KitMaster as b," + dbInv + "..M_TaxMaster t, " + dbInv + "..V#AvailProdStockBarcodes c ";
                    query += " where a.brandCode=b.KitId and a.activeStatus='Y' and b.ActiveStatus='Y' and b.RowStatus='Y' AND a.ProdID=t.ProdCode AND a.ProdId =c.ProdId AND t.StateCode='" + CompStateCode + "' and  b.kitId='" + kitId + "'";
                    query += " UNION ALL ";
                    query += " Select a.ProdID,a.ProductName,b.Qty as Qty,b.DP as DP1,CASE WHEN " + promoID + "=0 THEN 0 ELSE CAST((b.Rate * 100/(100+t.VatTax)) as numeric(18,2)) END as Rate,t.VatTax,CASE WHEN " + promoID + "=0 THEN 0 ELSE CAST((b.Rate *b.Qty * 100/(100+t.VatTax)) as numeric(18,2)) END as NetAmount,CASE WHEN " + promoID + "=0 THEN 0 ELSE (b.Rate * b.Qty)-CAST((b.Rate * b.Qty * 100/(100+t.VatTax)) as numeric(18,2)) END as TaxAmount,'N' as DispStatus,";
                    query += " a.MRP,b.Rate,0 as RP,CASE WHEN " + promoID + " = 0 THEN 0 else b.bv END as bv,TotDisc as Discount,'F' as ProdType,c.Barcode,a.Cv,a.PV from " + dbInv + "..M_ProductMaster as a, " + db + "..M_KitProductDetail as b, " + dbInv + "..V#AvailProdStockBarcodes c," + dbInv + "..M_TaxMaster t  ";
                    query += " where Cast(a.ProdId as varchar(10))=Cast(b.ProdId as varchar(10)) and b.ActiveStatus='Y' and b.RowStatus='Y' AND a.ProdId =c.ProdId AND a.ProdID=t.ProdCode and a.activeStatus='Y' and b.activeStatus='Y' and b.kitId='" + kitId + "'";

                    cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = SC;
                    SC.Close();
                    SC.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductModel product = new ProductModel();
                            product.IdNo = reader["ProdId"] != null ? Convert.ToString(reader["ProdId"].ToString()) : "";
                            product.ProductName = reader["ProductName"] != null ? reader["ProductName"].ToString() : "";
                            product.Rate = reader["Rate"] != null ? decimal.Parse(reader["Rate"].ToString()) : 0;
                            product.Quantity = reader["Qty"] != null ? decimal.Parse(reader["Qty"].ToString()) : 0;
                            product.Amount = reader["NetAmount"] != null ? decimal.Parse(reader["NetAmount"].ToString()) : 0;
                            product.TaxAmt = reader["TaxAmount"] != null ? decimal.Parse(reader["TaxAmount"].ToString()) : 0;
                            product.DiscAmt = reader["Discount"] != null ? decimal.Parse(reader["Discount"].ToString()) : 0;
                            product.TaxPer = reader["VatTax"] != null ? decimal.Parse(reader["VatTax"].ToString()) : 0;
                            // product.DispStatus = reader["DispStatus"] != null ? reader["DispStatus"].ToString() : "";
                            product.RP = reader["RP"] != null ? decimal.Parse(reader["RP"].ToString()) : 0;
                            product.DP = reader["DP"] != null ? decimal.Parse(reader["DP"].ToString()) : 0;
                            product.DP1 = reader["DP1"] != null ? decimal.Parse(reader["DP1"].ToString()) : 0;
                            product.MRP = reader["MRP"] != null ? decimal.Parse(reader["MRP"].ToString()) : 0;
                            product.BV = reader["bv"] != null ? decimal.Parse(reader["bv"].ToString()) : 0;
                            product.CV = reader["Cv"] != null ? decimal.Parse(reader["Cv"].ToString()) : 0;
                            product.PV = reader["PV"] != null ? decimal.Parse(reader["PV"].ToString()) : 0;
                            product.ProductTye = reader["ProdType"] != null ? reader["ProdType"].ToString() : "";
                            product.Barcode = reader["Barcode"] != null ? reader["Barcode"].ToString() : "";
                            objCustomerDetail.objListProduct.Add(product);
                        }
                    }


                }
                catch (Exception e)
                {
                    objCustomerDetail.objListProduct = new List<ProductModel>();
                }
            }

            return objCustomerDetail;
        }

        public ResponseDetail ActivateIdWithPackage(UpgradeID objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            DistributorBillModel TempDistributor = new DistributorBillModel();

            decimal maxUserSBillNo = 0;
            decimal? SessId = 0;
            string billPrefix = "";
            decimal maxSbillNo = 0;
            decimal? FsessId = 0;
            string billseries = "";
            string UserBillNo = "";
            string version = "";
            string BillNo = "";
            string narration_ = "", soldby_ = "", fcode_ = "";
            decimal netpayable_ = 0;
            objResponse.ResponseMessage = "Something went wrong!";
            objResponse.ResponseStatus = "FAILED";
            List<TrnPayModeDetail> objDTListPayMode = new List<TrnPayModeDetail>();
            try
            {
                //** Added om 21Jan19 (Code Start)
                List<string> ProdIds = new List<string>();
                List<string> OrderIds = new List<string>();

                string OutStockProdIDs = "";
                int Count_ = 0;
                foreach (var obj in objModel.objListProduct)
                {
                    if (obj.ProductTye == "P")
                    {
                        ProdIds = new List<string>();
                        decimal stock = CheckStock(obj.ProductCodeStr, objModel.objCustomer.UserDetails.PartyCode);
                        if (stock < obj.Quantity)
                        {
                            OutStockProdIDs = OutStockProdIDs + obj.ProductCodeStr + ",";
                            Count_ += 1;
                        }
                    }
                }

                if (Count_ > 0 && OutStockProdIDs.Length > 1)
                {
                    OutStockProdIDs = OutStockProdIDs.Substring(0, OutStockProdIDs.Length - 1);
                    objResponse.ResponseMessage = "Stock not Found of Package!";
                    objResponse.ResponseStatus = "FAILED";
                }
                //** Added om 21Jan19 (Code End)
                else
                {
                    string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                    string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                    SqlConnection SC = new SqlConnection(AppConnectionString);
                    SqlConnection SC1 = new SqlConnection(InvConnectionString);

                    string query = "Select Max(SessID) as MaxSessId from M_SessnMaster";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = SC;
                    SC.Close();
                    SC.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            SessId = decimal.Parse(reader["MaxSessId"].ToString());
                        }
                    }

                    using (var entity = new InventoryEntities())
                    {
                        CustomerDetail objCustomerDetail = new CustomerDetail();
                        objCustomerDetail = GetCustInfo(objModel.IDno);
                        maxSbillNo = (from result in entity.TrnBillMains select result.SBillNo).DefaultIfEmpty(0).Max();
                        maxSbillNo = maxSbillNo + 1;
                        FsessId = (from result in entity.M_FiscalMaster where result.ActiveStatus == "Y" select result.FSessId).DefaultIfEmpty(0).Max();

                        billseries = (from result in entity.M_FiscalMaster where result.FSessId == FsessId select result.BillSeries).FirstOrDefault();
                        billPrefix = (from result in entity.M_ConfigMaster select result.BillPrefix).FirstOrDefault();
                        maxUserSBillNo = (from result in entity.TrnBillMains where result.FSessId == FsessId && result.SoldBy == objModel.objCustomer.UserDetails.PartyCode && result.BillType != "S" select result.UserSBillNo).DefaultIfEmpty(0).Max();
                        maxUserSBillNo = maxUserSBillNo + 1;
                        //UserBillNo = billPrefix + "/" + billseries.Trim() + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxUserSBillNo;
                        BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo;
                        version = (from result in entity.M_NewHOVersionInfo select result.VersionNo).FirstOrDefault();

                        string strMaxUserSBillNo = maxUserSBillNo.ToString();
                        if (strMaxUserSBillNo.Count() < 5)
                        {
                            var countNum = strMaxUserSBillNo.Count();
                            var ToBeAddedDigits = 5 - countNum;
                            for (var j = 0; j < ToBeAddedDigits; j++)
                            {
                                strMaxUserSBillNo = "0" + strMaxUserSBillNo;
                            }
                        }

                        var UserPCode = (from result in entity.M_LedgerMaster where result.ActiveStatus == "Y" && result.PartyCode == objModel.objCustomer.UserDetails.PartyCode select result.UserPartyCode).FirstOrDefault();
                        UserBillNo = billPrefix + "/" + UserPCode + "/" + strMaxUserSBillNo;

                        List<TrnBillData> tempTableList = new List<TrnBillData>();
                        try
                        {
                            List<ProductModel> objListProductModel = new List<ProductModel>();
                            foreach (var obj in objModel.objListProduct)
                            {
                                TrnBillData objDTBillData = new TrnBillData();

                                objDTBillData.FSessId = FsessId ?? 0;
                                objDTBillData.SessId = SessId ?? 0;
                                objDTBillData.SBillNo = maxSbillNo;
                                objDTBillData.BillNo = BillNo;
                                objDTBillData.RefNo = "";
                                objDTBillData.BillDate = DateTime.Now.Date;
                                objDTBillData.CType = "M";
                                objDTBillData.SoldBy = objModel.objCustomer.UserDetails.PartyCode;
                                objDTBillData.BillBy = objModel.objCustomer.UserDetails.PartyCode;
                                objDTBillData.FType = "M";
                                objDTBillData.FCode = objCustomerDetail.IdNo;
                                objDTBillData.PartyName = objCustomerDetail.Name;
                                objDTBillData.SupplierId = 0;
                                objDTBillData.ChDDNo = 0;
                                objDTBillData.ChDate = DateTime.Now;
                                objDTBillData.ChAmt = 0;
                                objDTBillData.BankCode = 0;
                                objDTBillData.BankName = "";
                                objDTBillData.FormNo = objCustomerDetail.FormNo;
                                objDTBillData.TotalTaxAmount = objModel.objProduct.TotalTaxAmount;
                                objDTBillData.TotalSTaxAmount = 0;//check
                                objDTBillData.TotalDiscountAmt = objModel.objProduct.TotalDiscount;//==null?0: objModel.objProduct.TotalDiscount;
                                objDTBillData.Discount = obj.DiscAmt ?? 0;
                                objDTBillData.TotalKitBvValue = objModel.objProduct.TotalBV;
                                objDTBillData.TotalBvValue = objModel.objProduct.TotalBV;
                                objDTBillData.TotalCVValue = objModel.objProduct.TotalCV;
                                objDTBillData.TotalPVValue = objModel.objProduct.TotalPV;
                                objDTBillData.TotalRPValue = objModel.objProduct.TotalRP;
                                objDTBillData.CashDiscPer = 0;
                                objDTBillData.CashDiscAmount = 0;
                                objDTBillData.NetPayable = objModel.objProduct.TotalNetPayable; //objModel.objProduct.TotalAmount + objModel.objProduct.TotalTaxAmount;
                                objDTBillData.TotalAmount = objModel.objProduct.TotalAmount;
                                objDTBillData.RndOff = Math.Round(objDTBillData.NetPayable, 0) - Math.Round(objModel.objProduct.TotalAmount + objModel.objProduct.TotalTaxAmount, 2);//check
                                objDTBillData.CardAmount = 0;
                                objDTBillData.PayMode = "";
                                objDTBillData.PayPrefix = "";
                                objDTBillData.BvTransfer = "N";
                                objDTBillData.Remarks = "Upgraded by " + objModel.NewKitName + " (" + System.Configuration.ConfigurationManager.AppSettings["BVCaption"] + ": " + objModel.objProduct.TotalBV.ToString() + ")";
                                objDTBillData.DispatchStatus = "Y";
                                objDTBillData.LR = "0";
                                objDTBillData.LRDate = DateTime.Now;
                                objDTBillData.TransporterName = "";
                                objDTBillData.DispatchTo = "";
                                objDTBillData.FreightType = "";
                                objDTBillData.FreightAmt = 0;
                                objDTBillData.Series = "";//check
                                objDTBillData.Scratch = "";
                                objDTBillData.RefId = 0;
                                objDTBillData.RefName = "";
                                objDTBillData.JType = "";
                                objDTBillData.Unit = 0;
                                objDTBillData.BillTo = "R";
                                objDTBillData.PSessId = 0;
                                objDTBillData.BillFor = "RB";
                                objDTBillData.DcNo = "";
                                objDTBillData.Imported = "N";
                                objDTBillData.IsReceive = "R";
                                objDTBillData.IsCredit = "F";
                                objDTBillData.BillType = objModel.BillType;
                                // objDTBillData.TotalDiscountAmt = 0;
                                objDTBillData.VDiscountAmt = 0;
                                objDTBillData.ReceiverID = "";
                                objDTBillData.ReceiverName = "";
                                objDTBillData.ReceiverMNo = "";

                                if (objModel.promoId == 1)
                                    objDTBillData.ReceiverIDProof = "E";
                                else
                                    objDTBillData.ReceiverIDProof = "";

                                objDTBillData.TotalFPoint = 0;
                                objDTBillData.CashReward = 0;
                                objDTBillData.RecvAmount = 0;
                                objDTBillData.ReturnToCustAmt = 0;
                                objDTBillData.ActiveStatus = "Y";
                                objDTBillData.RecTimeStamp = DateTime.Now;
                                objDTBillData.UserId = objModel.objCustomer.UserDetails.UserId;
                                objDTBillData.UserName = objModel.objCustomer.UserDetails.UserName;
                                objDTBillData.Version = version;
                                objDTBillData.DelvPlace = "";
                                objDTBillData.PaymentDtl = "Cash: " + objDTBillData.NetPayable;
                                objDTBillData.IDType = "";
                                objDTBillData.BranchName = "";
                                objDTBillData.LocId = objModel.NewKitId;
                                objDTBillData.LocName = "";
                                objDTBillData.Pincode = "";
                                objDTBillData.CourierId = 0;
                                objDTBillData.CourierName = "";
                                objDTBillData.ProductId = obj.ProductCodeStr;
                                objDTBillData.ProductName = obj.ProductName;
                                objDTBillData.Barcode = obj.Barcode ?? "";
                                objDTBillData.BatchNo = obj.BatchNo ?? (obj.Barcode ?? "");
                                objDTBillData.Qty = obj.Quantity;
                                objDTBillData.MRP = obj.MRP ?? 0;
                                objDTBillData.DP = obj.DP ?? 0;
                                objDTBillData.Rate = obj.Rate ?? 0;
                                objDTBillData.BV = obj.BV ?? 0;
                                objDTBillData.BVValue = objDTBillData.BV * objDTBillData.Qty;
                                objDTBillData.CV = obj.CV ?? 0;
                                objDTBillData.CVValue = objDTBillData.CV * objDTBillData.Qty;
                                objDTBillData.PV = obj.PV ?? 0;
                                objDTBillData.PVValue = objDTBillData.PV * objDTBillData.Qty;
                                objDTBillData.RP = obj.RP ?? 0;
                                objDTBillData.RPValue = objDTBillData.RP * objDTBillData.Qty;
                                objDTBillData.IsKitBV = "N";
                                objDTBillData.TaxType = objModel.TaxType;
                                if (objDTBillData.TaxType == "S")
                                {
                                    string taxAmt_ = String.Format("{0:F2}", obj.TaxAmt);
                                    taxAmt_ = taxAmt_.Substring(taxAmt_.Length - 1, 1);
                                    if (Convert.ToInt16(taxAmt_) % 2 != 0)
                                    {
                                        obj.TaxAmt = Decimal.Parse((Convert.ToDouble(obj.TaxAmt) - 0.01).ToString());
                                        obj.Amount = Decimal.Parse((Convert.ToDouble(obj.Amount) + 0.01).ToString());
                                    }
                                    objDTBillData.TaxAmount = 0;
                                    objDTBillData.Tax = 0;
                                    objDTBillData.CGST = obj.TaxPer / 2 ?? 0;
                                    objDTBillData.CGSTAmt = obj.TaxAmt / 2 ?? 0;
                                    objDTBillData.SGST = obj.TaxPer / 2 ?? 0;
                                    objDTBillData.SGSTAmt = obj.TaxAmt / 2 ?? 0;

                                }
                                else
                                {
                                    objDTBillData.TaxAmount = obj.TaxAmt ?? 0;
                                    objDTBillData.Tax = obj.TaxPer ?? 0;
                                    objDTBillData.CGST = 0;
                                    objDTBillData.CGSTAmt = 0;
                                    objDTBillData.SGST = 0;
                                    objDTBillData.SGSTAmt = 0;
                                    objDTBillData.TaxType = "I";
                                }
                                objDTBillData.DiscountPer = 0;
                                //objDTBillData.Discount = 0;
                                objDTBillData.NetAmount = obj.Amount;
                                objDTBillData.DSeries = "";
                                objDTBillData.DImported = "N";
                                objDTBillData.IMEINo = "";
                                objDTBillData.BNo = "";
                                objDTBillData.ItemType = "";
                                objDTBillData.VDiscount = 0;
                                objDTBillData.VDiscountValue = 0;
                                objDTBillData.FPoint = 0;
                                objDTBillData.FPointValue = 0;
                                objDTBillData.ProdCommssn = 0;
                                objDTBillData.ProdCommssnAmt = 0;
                                objDTBillData.OrdStatus = "";
                                objDTBillData.OrdQty = 0;
                                objDTBillData.RemQty = 0;
                                objDTBillData.DP1 = 0;
                                objDTBillData.DReason = "";
                                objDTBillData.DUserId = 0;
                                objDTBillData.DRecTimeStamp = null;
                                objDTBillData.DocWeight = 0;
                                objDTBillData.DocketNo = "";
                                objDTBillData.DOD = null;
                                objDTBillData.DelvAddress = "";
                                objDTBillData.OrderNo = "";
                                objDTBillData.OrderDate = null;
                                objDTBillData.DocketDate = null;
                                objDTBillData.DelvStatus = "P";
                                objDTBillData.DelvUserId = 0;
                                objDTBillData.DelvRecTimeStamp = null;
                                objDTBillData.OrderType = "T";
                                objDTBillData.UserBillNo = UserBillNo;
                                objDTBillData.UserSBillNo = maxUserSBillNo;
                                objDTBillData.STNFormNo = "";
                                objDTBillData.StkRecv = "N";
                                objDTBillData.StkRecvDate = null;
                                objDTBillData.StkRecvUserId = 0;
                                objDTBillData.InTransit = "N";
                                objDTBillData.UID = string.IsNullOrEmpty(objModel.objProduct.UID) ? "" : objModel.objProduct.UID;
                                objDTBillData.OfferUID = 0;
                                objDTBillData.IsKit = "N";
                                objDTBillData.ProdType = obj.ProductTye;
                                objDTBillData.TotalCorton = "";
                                objDTBillData.TotalMonoCorton = "";
                                objDTBillData.SpclOfferId = 0;
                                objDTBillData.VAT = 0;
                                objDTBillData.BuyerAddress = "";
                                objDTBillData.BuyerTIN = "";
                                soldby_ = objDTBillData.SoldBy;
                                fcode_ = objDTBillData.FCode;
                                netpayable_ = objDTBillData.NetPayable;
                                narration_ = "Wallet deducted against " + objDTBillData.UserBillNo + ".";

                                entity.TrnBillDatas.Add(objDTBillData);
                            }
                            int i = 0;

                            using (var objDTTrans = entity.Database.BeginTransaction())
                            {
                                try
                                {
                                    i = entity.SaveChanges();
                                    objDTTrans.Commit();

                                    DeductPartyWallet(BillNo, narration_, soldby_, fcode_, netpayable_, "shoppe");

                                    if (i > 0)
                                    {
                                        query = "Exec Sp_ActivateMembers '" + (objCustomerDetail.IdNo).Trim() + "','" + objModel.NewKitId + "','" + BillNo + "','" + objModel.objProduct.TotalBV + "';";
                                        cmd.CommandText = query;
                                        cmd.Connection = SC;
                                        SC.Close();
                                        SC.Open();
                                        using (SqlDataReader reader = cmd.ExecuteReader())
                                        {
                                            if (reader.Read())
                                            {
                                                string msg = reader["Result"].ToString();

                                            }
                                        }

                                        try
                                        {
                                            EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cash;
                                            string value = EnumPayModes.GetEnumDescription(enumVar);
                                            entity.TrnPayModeDetails.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = "B", BillNo = BillNo, PayPrefix = "C", PayMode = "Cash", Amount = objModel.objProduct.TotalNetPayable, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, CardNo = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                            //entity.TrnPayModeDetails.Add(objDTListPayMode);
                                            i = entity.SaveChanges();
                                            //objDTTrans.Commit();
                                        }
                                        catch (Exception ex)
                                        {

                                        }

                                        objResponse.ResponseMessage = "Saved Successfully.";
                                        objResponse.ResponseStatus = "OK";

                                    }
                                }
                                catch (Exception ex)
                                {
                                    objDTTrans.Rollback();
                                }
                            }


                        }
                        catch (Exception e)
                        {
                            objResponse.ResponseMessage = "Something went wrong!";
                            objResponse.ResponseStatus = "FAILED";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objResponse;
        }

        public decimal CheckStock(string ProdID, string PartyCode)
        {
            decimal stock = 0;
            using (var entity = new InventoryEntities())
            {
                stock = (from stockAvail in entity.Im_CurrentStock
                         where stockAvail.ProdId == ProdID && stockAvail.FCode.Equals(PartyCode)
                         select stockAvail.Qty
                                                      ).DefaultIfEmpty(0).Sum();
            }
            return stock;

        }

        public Boolean IsFPVInvoiceValid()
        {
            Boolean b = false;
            int Dt = DateTime.Now.Day;
            using (var entity = new InventoryEntities())
            {
                var result = (from s in entity.M_ConfigMaster where s.FPVOfferFrom <= Dt && s.FPVOfferTo >= Dt && s.FPVOfferTill >= DateTime.Now select s.BillPrefix).FirstOrDefault();
                if (result != null)
                    b = true;
            }
            return b;
        }

        public ResponseDetail DebitCreditWallet(Wallet objWallet)
        {
            ResponseDetail objResponse = new ResponseDetail();

            try
            {
                decimal fsessId;
                using (var entity = new InventoryEntities())
                {
                    fsessId = entity.M_FiscalMaster
                                    .Where(x => x.ActiveStatus == "Y")
                                    .Max(x => x.FSessId);
                }

                string connStr = ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;

                string query = @"
INSERT INTO TrnVoucher
(
    VoucherNo,
    VoucherDate,
    DrTo,
    CrTo,
    Amount,
    Narration,
    VType,
    SessID,
    FSessId
)
SELECT
    ISNULL(MAX(VoucherNo),0) + 1,
    GETDATE(),
    @DrTo,
    @CrTo,
    @Amount,
    @Narration,
    @VType,
    @SessID,
    @FSessId
FROM TrnVoucher WITH (UPDLOCK, HOLDLOCK);";

                using (SqlConnection con = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@DrTo",
                        objWallet.DrCr?.ToLower() == "debit" ? objWallet.FCode : "");

                    cmd.Parameters.AddWithValue("@CrTo",
                        objWallet.DrCr?.ToLower() == "credit" ? objWallet.FCode : "");

                    cmd.Parameters.AddWithValue("@Amount", objWallet.Amount);
                    cmd.Parameters.AddWithValue("@Narration", objWallet.Narration ?? "");
                    cmd.Parameters.AddWithValue("@VType", objWallet.ACType);
                    cmd.Parameters.AddWithValue("@SessID", fsessId);
                    cmd.Parameters.AddWithValue("@FSessId", fsessId);

                    con.Open();
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        objResponse.ResponseMessage = "Saved Successfully";
                        objResponse.ResponseStatus = "OK";
                    }
                    else
                    {
                        objResponse.ResponseMessage = "Insert failed";
                        objResponse.ResponseStatus = "FAILED";
                    }
                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseMessage = ex.Message;
                objResponse.ResponseStatus = "ERROR";
            }

            return objResponse;
        }


        public List<PartyModel> GetPartyBalance()
        {
            List<PartyModel> objpartyList = new List<PartyModel>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objpartyList = (from party in entity.V_PartyBalance
                                    orderby party.PartyName
                                    select new PartyModel
                                    {
                                        PartyCode = party.PartyCode,
                                        PartyName = party.PartyName,
                                        StateCode = party.StateCode,
                                        GroupId = party.GroupID,
                                        UserPartyCode = party.UserPartyCode,
                                        CreditLimit = party.Balance ?? 0
                                    }
                                 ).ToList();


                }
            }
            catch (Exception ex)
            {

            }
            return objpartyList;
        }
        public ResponseDetail GetGVDetail(string VoucherNo, string FormNo)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.StatusCode = 0;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                    SqlConnection SC = new SqlConnection(InvConnectionString);
                    string query = "Select  *,Replace(Convert(varchar,Getdate(),106),' ','-') as TodayDate,Replace(Convert(varchar,StartDate,106),' ','-') as SDate,Replace(Convert(varchar,ExpiryDate,106),' ','-') as EDate FROM TrnGPVCoupon WHERE CouponNo=@CouponNo AND FormNo=@FormNo ";// AND Cast(Getdate() as Date) >=Cast(StartDate as Date)  AND Cast(Getdate() as Date) <=Cast(ExpiryDate as Date) ";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@CouponNo", VoucherNo);
                    cmd.Parameters.AddWithValue("@FormNo", FormNo);
                    cmd.Connection = SC;
                    SC.Close();
                    SC.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            if (reader["IsUsed"].ToString().ToUpper() == "Y")
                            {
                                objResponse.ResponseStatus = "FAILED";
                                objResponse.ResponseMessage = "Voucher is already used on " + reader["UsedDate"].ToString() + ".";
                            }
                            else
                            {
                                if (Convert.ToDateTime(reader["SDate"].ToString()) <= DateTime.Now.Date && Convert.ToDateTime(reader["EDate"].ToString()) >= DateTime.Now.Date)
                                {
                                    objResponse.ResponseStatus = "OK";
                                    objResponse.StatusCode = Convert.ToInt32(reader["ProdBunchID"].ToString());
                                    objResponse.ResponseMessage = reader["CouponAmt"].ToString();

                                }
                                else
                                {
                                    objResponse.ResponseStatus = "FAILED";
                                    objResponse.ResponseMessage = "Voucher is valid for " + reader["SDate"].ToString().Substring(3, 3) + ".";
                                }
                                // objResponse.ResponseMessage = "success.";
                            }
                        }
                        else
                        {
                            objResponse.ResponseStatus = "FAILED";
                            objResponse.ResponseMessage = "Invalid Voucher No.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return objResponse;
        }

        public ResponseDetail GetCEDDetail(string VoucherNo, string FormNo)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.StatusCode = 0;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                    SqlConnection SC = new SqlConnection(InvConnectionString);
                    string query = "Select  *,Replace(Convert(varchar,Getdate(),106),' ','-') as TodayDate,Replace(Convert(varchar,StartDate,106),' ','-') as SDate,Replace(Convert(varchar,ExpiryDate,106),' ','-') as EDate FROM TrnCEDCoupon WHERE CouponNo=@CouponNo AND FormNo=@FormNo ";// AND Cast(Getdate() as Date) >=Cast(StartDate as Date)  AND Cast(Getdate() as Date) <=Cast(ExpiryDate as Date) ";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@CouponNo", VoucherNo);
                    cmd.Parameters.AddWithValue("@FormNo", FormNo);
                    cmd.Connection = SC;
                    SC.Close();
                    SC.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            if (reader["IsUsed"].ToString().ToUpper() == "Y")
                            {
                                objResponse.ResponseStatus = "FAILED";
                                objResponse.ResponseMessage = "Voucher is already used on " + reader["UsedDate"].ToString() + ".";
                            }
                            else
                            {
                                if (Convert.ToDateTime(reader["SDate"].ToString()) <= DateTime.Now.Date && Convert.ToDateTime(reader["EDate"].ToString()) >= DateTime.Now.Date)
                                {
                                    objResponse.ResponseStatus = "OK";
                                    objResponse.StatusCode = Convert.ToInt32(reader["ProdBunchID"].ToString());
                                    objResponse.ResponseMessage = reader["CouponAmt"].ToString();

                                }
                                else
                                {
                                    objResponse.ResponseStatus = "FAILED";
                                    objResponse.ResponseMessage = "Voucher is valid for " + reader["SDate"].ToString().Substring(3, 3) + ".";
                                }
                                // objResponse.ResponseMessage = "success.";
                            }
                        }
                        else
                        {
                            objResponse.ResponseStatus = "FAILED";
                            objResponse.ResponseMessage = "Invalid Voucher No.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return objResponse;
        }

        public ResponseDetail SaveApproveWaletRequest(List<WalletRequest> objModelList)
        {
            ResponseDetail objResponse = new ResponseDetail();
            string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
            SqlConnection SC1 = new SqlConnection(InvConnectionString);
            try
            {
                var PartyCode = "";
                decimal Amount = 0;
                string db = System.Configuration.ConfigurationManager.AppSettings["Database"];
                string compName = System.Configuration.ConfigurationManager.AppSettings["CompName"];

                using (var entity = new InventoryEntities())
                {
                    foreach (var record in objModelList)
                    {
                        if (record.IsApproved != "N")
                        {
                            var obj = (from r in entity.WalletReqs where r.ReqNo.ToString() == record.ReqNo select r).FirstOrDefault();
                            if (obj.IsApprove == "N")
                            {
                                string sms, Sql;
                                if (obj.IsApprove != "Y" && record.IsApproved == "Y")
                                {
                                    if (SC1.State == ConnectionState.Closed)
                                        SC1.Open();
                                    Sql = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                                           " Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'','" + obj.ReqBy + "','" + obj.Amount + "','Wallet credited against Req No. " + record.ReqNo + ".','WReq/" + record.ReqNo + "','" + record.VType + "','O','Wallet Request Approved.',(Select Max(SessID) FROM " + db + "..M_SessnMaster),(Select Max(FSessID) FROM M_FiscalMaster) FROM TrnVoucher ;";
                                    SqlCommand cmd = new SqlCommand();
                                    cmd.CommandText = Sql;
                                    cmd.Connection = SC1;
                                    cmd.ExecuteNonQuery();

                                    // sms = "Your wallet request [Req. No. " + obj.ReqNo.ToString() + "] for amount " + obj.Amount.ToString() + " has been Approved. info: " + compName;
                                }
                                //else
                                //sms = "Your wallet request [Req. No. " + obj.ReqNo.ToString() + "] for amount " + obj.Amount.ToString() + " has been Rejected. info: " + compName;

                                obj.IsApprove = record.IsApproved;
                                obj.ApproveRemark = record.ApproveRemark != null ? record.ApproveRemark : "";
                                obj.ApproveDate = DateTime.Now;
                                obj.ApproveBy = record.ApproveBy;
                                PartyCode = obj.ReqBy;
                                Amount = obj.Amount;
                                //Sql = ";INSERT INTO "+ db +"..SendSMS(Formno,MobileNo,sms,IsSent) " +
                                //            " Select 0,MobileNo,'"+ sms +"','N' FROM M_LedgerMaster WHERE PartyCode='" + obj.ReqBy + "' AND ISNUMERIC(MobileNo)=1 AND LEN(Cast(MobileNo as varchar(20)))=10;";
                                //using (SqlCommand cmd = new SqlCommand())
                                //{
                                //    if (SC1.State == ConnectionState.Closed)
                                //        SC1.Open();
                                //    cmd.CommandText = Sql;
                                //    cmd.Connection = SC1;
                                //    cmd.ExecuteNonQuery();
                                //}
                                //if (SC1.State == ConnectionState.Open)
                                //    SC1.Close();
                            }
                        }
                    }
                    SC1.Close();
                    int i = entity.SaveChanges();
                    if (i > 0)
                    {
                        objResponse.ResponseStatus = "OK";
                        objResponse.ResponseMessage = "Saved Successfully!";
                    }
                    else
                    {
                        objResponse.ResponseStatus = "Failed";
                        objResponse.ResponseMessage = "Something went wrong.";
                    }
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errors = ex.EntityValidationErrors
                    .SelectMany(e => e.ValidationErrors)
                    .Select(v => v.PropertyName + ": " + v.ErrorMessage);
                objResponse.ResponseStatus = "FAILED";
                objResponse.ResponseMessage = string.Join(" | ", errors);

                try
                {
                    using (var entity = new InventoryEntities())
                    {
                        foreach (var record in objModelList)
                        {
                            if (record.IsApproved != "N")
                            {
                                var obj = (from r in entity.WalletReqs where r.ReqNo.ToString() == record.ReqNo select r).FirstOrDefault();
                                if (obj.IsApprove == "N")
                                {
                                    if (SC1.State == ConnectionState.Closed)
                                        SC1.Open();
                                    string Sql = ";Delete FROM TrnVoucher WHERE RefNo='WReq/" + record.ReqNo + "'";
                                    SqlCommand cmd = new SqlCommand();
                                    cmd.CommandText = Sql;
                                    cmd.Connection = SC1;
                                    cmd.ExecuteNonQuery();

                                }
                            }
                        }
                        SC1.Close();
                    }
                }
                catch (Exception)
                {

                }
                // (aapka TrnVoucher rollback code yahin andar rakh sakti hain)
            }
            //catch (DbEntityValidationException ex)
            //{
            //    try
            //    {
            //        using (var entity = new InventoryEntities())
            //        {
            //            foreach (var record in objModelList)
            //            {
            //                if (record.IsApproved != "N")
            //                {
            //                    var obj = (from r in entity.WalletReqs where r.ReqNo.ToString() == record.ReqNo select r).FirstOrDefault();
            //                    if (obj.IsApprove == "N")
            //                    {
            //                        if (SC1.State == ConnectionState.Closed)
            //                            SC1.Open();
            //                        string Sql = ";Delete FROM TrnVoucher WHERE RefNo='WReq/" + record.ReqNo + "'";
            //                        SqlCommand cmd = new SqlCommand();
            //                        cmd.CommandText = Sql;
            //                        cmd.Connection = SC1;
            //                        cmd.ExecuteNonQuery();

                //                    }
                //                }
                //            }
                //            SC1.Close();
                //        }
                //    }
                //    catch (Exception)
                //    {

                //    }

                //}
                return objResponse;
        }

        public ResponseDetail RejectWalletRequest(string ReqNo, string RejectReason, int UserID)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                var PartyCode = "";
                decimal Amount = 0;
                using (var entity = new InventoryEntities())
                {
                    var obj = (from r in entity.WalletReqs where r.ReqNo.ToString() == ReqNo select r).FirstOrDefault();
                    if (obj.IsApprove == "N")
                    {
                        obj.IsApprove = "R";
                        obj.ApproveRemark = RejectReason != null ? RejectReason : "";
                        obj.ApproveDate = DateTime.Now;
                        obj.ApproveBy = UserID;
                        PartyCode = obj.ReqBy;
                        Amount = obj.Amount;
                    }

                    int i = entity.SaveChanges();
                    if (i > 0)
                    {
                        objResponse.ResponseStatus = "OK";
                        objResponse.ResponseMessage = "Rejected Successfully!";
                    }
                    else
                    {
                        objResponse.ResponseStatus = "Failed";
                        objResponse.ResponseMessage = "Something went wrong.";
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {

            }
            return objResponse;
        }

        public async Task<string> SaveWalletRequest(WalletRequest objWallet)
        {
            string objResp = "";
            try
            {
                decimal reqNo = 0;
                using (var entity = new InventoryEntities())
                {
                    reqNo = (from result in entity.WalletReqs select result.ReqNo).DefaultIfEmpty(1000).Max();
                    reqNo = reqNo + 1;
                    WalletReq objWReq = new WalletReq();
                    objWReq.ReqNo = reqNo;
                    objWReq.ReqDate = DateTime.Now.Date;
                    objWReq.Amount = objWallet.Amount;
                    objWReq.BankId = objWallet.BankID;// != null ? objWallet.BankID :0; 
                    objWReq.BankName = objWallet.BankName != null ? objWallet.BankName : "";
                    objWReq.BranchName = objWallet.BranchName != null ? objWallet.BranchName : "";
                    objWReq.ChqDate = objWallet.ChqDate;
                    objWReq.ChqNo = objWallet.ChqNo ?? "0";
                    objWReq.Paymode = objWallet.Paymode;
                    objWReq.PID = objWallet.PID;
                    objWReq.Remarks = objWallet.Remarks != null ? objWallet.Remarks : "";
                    objWReq.ReqBy = objWallet.ReqBy;
                    objWReq.ScannedFile = objWallet.ScannedFileName != null ? objWallet.ScannedFileName : "";
                    objWReq.RecTimeStamp = DateTime.Now;
                    objWReq.TransNo = "0";
                    objWReq.IsApprove = "N";
                    objWReq.ApproveBy = 0;
                    objWReq.ApproveRemark = "";
                    objWReq.VType = objWallet.VType;
                    entity.WalletReqs.Add(objWReq);
                    entity.SaveChanges();
                    objResp = "OK";
                    string PartyName = entity.M_LedgerMaster.Where(r => r.PartyCode == objWallet.ReqBy).FirstOrDefault().PartyName;
                    string message = "Hi PGV, Wallet of Rs." + objWallet.Amount + "/- is requested by " + PartyName + " through Pay mode " + objWallet.Paymode + " waiting for Approval.PROGLEN";
                    bool IsSuccess = await Task.Run(() => Program.SendInvoiceSMS(message, "9017927000,8107504222,7419854863", "1507162877454787795"));
                }
            }
            catch (DbEntityValidationException e)
            {

            }
            catch (Exception ex)
            {

            }
            return objResp;
        }

        public List<WalletRequest> GetAllWalletRequest(string PartyCode, string dateType, string FromDate, string ToDate, string IsApproved)
        {
            List<WalletRequest> list = new List<WalletRequest>();
            try
            {
                DateTime startDate = DateTime.Now.AddYears(-5);
                DateTime endDate = DateTime.Now.AddDays(1);

                if (FromDate != null && FromDate.ToUpper() != "ALL" && FromDate.ToUpper() != "")
                    try
                    {
                        startDate = Convert.ToDateTime(FromDate);
                    }
                    catch (Exception)
                    { }
                if (ToDate != null && ToDate.ToUpper() != "ALL" && ToDate.ToUpper() != "")
                    try
                    {
                        endDate = Convert.ToDateTime(ToDate);
                    }
                    catch (Exception)
                    { }

                using (var entity = new InventoryEntities())
                {
                    list = (from r in entity.WalletReqs
                            join l in entity.M_LedgerMaster on r.ReqBy equals l.PartyCode
                            into RequestParty
                            from ed in RequestParty.DefaultIfEmpty()
                            select new WalletRequest
                            {
                                ReqNo = r.ReqNo.ToString(),
                                ReqDate = r.ReqDate,
                                ReqDateStr = r.ReqDate.ToString(),
                                Remarks = r.Remarks,
                                ScannedFileName = r.ScannedFile == "" ? "noImg.jpeg" : r.ScannedFile,
                                Amount = r.Amount,
                                BankName = r.BankName,
                                BranchName = r.BranchName,
                                ChqDate = r.ChqDate,
                                ChqNo = r.ChqNo,
                                Paymode = r.Paymode,
                                ReqBy = r.ReqBy,
                                ReqByName = ed.PartyName == null ? "" : ed.PartyName,
                                IsApproved = r.IsApprove,
                                ApprovedDate = r.ApproveDate,
                                ApproveDateStr = r.ApproveDate.ToString(),
                                ApproveRemark = r.ApproveRemark,
                                ApproveBy = r.ApproveBy,
                                VType = r.VType

                            }).ToList();
                    if (PartyCode != null)//Page: WalletRequestReport
                    {
                        if (PartyCode != null && PartyCode != "" && PartyCode != "All" && PartyCode != "0")
                            list = list.Where(m => m.ReqBy == PartyCode).ToList();
                        if (dateType == "R")
                        {
                            list = list.Where(m => m.ReqDate >= startDate).ToList();
                            list = list.Where(m => m.ReqDate <= endDate).ToList();
                        }
                        else
                        {
                            list = list.Where(m => m.ApprovedDate != null).ToList();
                            list = list.Where(m => m.ApprovedDate >= startDate).ToList();
                            list = list.Where(m => m.ApprovedDate <= endDate).ToList();
                        }

                        if (IsApproved != "A" && IsApproved != null)
                            list = list.Where(m => m.IsApproved == IsApproved).ToList();
                    }
                    else//Page: ApproveWalletRequest 
                        list = list.Where(m => m.IsApproved == "N").ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public CustomerDetail GetSJPCustInfo(string IdNo)
        {
            CustomerDetail objCustomerDetail = new CustomerDetail();
            if (!(string.IsNullOrEmpty(IdNo)))
            {
                try
                {
                    using (var entity = new InventoryEntities())
                    {
                        string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                        SqlConnection SC = new SqlConnection(AppConnectionString);

                        string query = "select * from M_JackpotMaster where IDno=@IdNo";
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@IdNo", IdNo);
                        cmd.Connection = SC;
                        SC.Close();
                        SC.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                objCustomerDetail.IdNo = reader["IDno"] != null ? reader["IDno"].ToString() : "";
                                objCustomerDetail.Name = reader["MemberName"] != null ? reader["MemberName"].ToString() : "";
                                objCustomerDetail.FormNo = reader["Formno"] != null ? decimal.Parse(reader["Formno"].ToString()) : 0;
                                objCustomerDetail.IsActive = reader["ActiveStatus"].ToString() == "Y" ? true : false;
                                objCustomerDetail.MobileNo = reader["Mobl"].ToString();
                                objCustomerDetail.NumberOfBill = entity.sp_GetNoOfJackpotBill(objCustomerDetail.IdNo).DefaultIfEmpty(0).FirstOrDefault() ?? 0;
                            }
                            else
                            {
                                objCustomerDetail = new CustomerDetail();
                                objCustomerDetail.IdNo = "Record does not exists!";
                                objCustomerDetail.Name = "";
                            }
                        }
                        SC.Close();
                        objCustomerDetail.MinRepurch = 0;

                        objCustomerDetail.InvoiceType = new List<string>();
                        objCustomerDetail.InvoiceType.Add("SJP Customer Invoice,J");
                        objCustomerDetail.IsBillOnMrp = true;
                    }
                }
                catch (Exception e)
                {
                    objCustomerDetail = new CustomerDetail();
                    objCustomerDetail.IdNo = "Something went wrong!";
                    objCustomerDetail.Name = "";
                }
            }
            return objCustomerDetail;
        }


        public ResponseDetail GetFreeScratchCardDetail(string VoucherNo)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.StatusCode = 0;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                    SqlConnection SC = new SqlConnection(InvConnectionString);
                    string query = "Select * FROM TrnSJPCoupon WHERE CouponNo=@CouponNo";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@CouponNo", VoucherNo);
                    cmd.Connection = SC;
                    SC.Close();
                    SC.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["IsUsed"].ToString().ToUpper() == "Y")
                            {
                                objResponse.ResponseStatus = "FAILED";
                                objResponse.ResponseMessage = "Coupon is already used on " + reader["UsedDate"].ToString() + ".";
                            }
                            else
                            {
                                objResponse.ResponseStatus = "OK";
                                objResponse.ResponseMessage = reader["CouponAmt"].ToString();
                            }
                        }
                        else
                        {
                            objResponse.ResponseStatus = "FAILED";
                            objResponse.ResponseMessage = "Invalid Coupon No.";
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return objResponse;
        }
        public List<SlabModel> GetSlab()
        {
            List<SlabModel> objSlabList = new List<SlabModel>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objSlabList = (from slab in entity.TblSlabMasters
                                   orderby slab.ID
                                   select new SlabModel
                                   {
                                       ID = slab.ID,
                                       Slab = slab.FromSlab + "-" + slab.ToSlab
                                   }
                                 ).ToList();


                }
            }
            catch (Exception ex)
            {

            }
            return objSlabList;
        }
        public ResponseDetail SaveSlab(RewardPoint objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            Int32 reqNo = 0;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    if (objModel.ID == 0)
                    {
                        reqNo = (from result in entity.TrnRewardPoints select result.ID).DefaultIfEmpty(1000).Max();
                        reqNo = reqNo + 1;
                        TrnRewardPoint objReward = new TrnRewardPoint();
                        objReward.ID = reqNo;
                        objReward.FromDate = Convert.ToDateTime(objModel.FromDateStr);
                        objReward.ToDate = Convert.ToDateTime(objModel.ToDateStr);
                        objReward.RPoint = objModel.RPoint;
                        objReward.SlabID = objModel.SlabID;
                        entity.TrnRewardPoints.Add(objReward);
                        objResponse.ResponseStatus = "OK";
                    }
                    else
                    {
                        TrnRewardPoint objRewardPoint = new TrnRewardPoint();
                        objRewardPoint = (from r in entity.TrnRewardPoints
                                          where r.ID == objModel.ID
                                          select r
                                         ).FirstOrDefault();
                        objRewardPoint.ID = objModel.ID;
                        objRewardPoint.FromDate = Convert.ToDateTime(objModel.FromDateStr);
                        objRewardPoint.ToDate = Convert.ToDateTime(objModel.ToDateStr);
                        objRewardPoint.RPoint = objModel.RPoint;
                        objRewardPoint.SlabID = objModel.SlabID;
                        objResponse.ResponseStatus = "OK";
                    }
                    int i = 0;
                    try
                    {
                        i = entity.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        objResponse.ResponseStatus = "FAILED";
                    }
                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseStatus = "FAILED";
            }
            return objResponse;
        }
        public RewardPoint GetSlabPoint(int ID)
        {
            RewardPoint objRewardPoint = new RewardPoint();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objRewardPoint = (from r in entity.TrnRewardPoints
                                      where r.ID == ID
                                      select new RewardPoint
                                      {
                                          ID = r.ID,
                                          SlabID = r.SlabID.Value,
                                          FromDate = r.FromDate.Value,
                                          ToDate = r.ToDate.Value,
                                          //FromDateStr = r.FromDate.Value.ToShortDateString(),
                                          //ToDateStr = r.ToDate.Value.ToShortDateString(),
                                          RPoint = r.RPoint.Value
                                      }
                                    ).ToList().FirstOrDefault();

                    objRewardPoint.FromDateStr = objRewardPoint.FromDate.ToShortDateString();
                    objRewardPoint.ToDateStr = objRewardPoint.ToDate.ToShortDateString();
                }
            }
            catch (Exception ex)
            {

            }
            return objRewardPoint;
        }
        public List<RewardPoint> GetAllSlab()
        {
            List<RewardPoint> objRewardPoint = new List<RewardPoint>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objRewardPoint = (from r in entity.TrnRewardPoints
                                      join b in entity.TblSlabMasters on r.SlabID equals b.ID
                                      select new RewardPoint
                                      {
                                          ID = r.ID,
                                          SlabID = r.SlabID.Value,
                                          SlabRange = b.FromSlab + "-" + b.ToSlab,
                                          FromDate = r.FromDate.Value,
                                          ToDate = r.ToDate.Value,
                                          RPoint = r.RPoint.Value
                                      }
                                    ).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return objRewardPoint;
        }
        public List<M_EInvoiceCredential> GetEInvoicecre()
        {
            List<M_EInvoiceCredential> obj = new List<M_EInvoiceCredential>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    obj = (from p in entity.EInvoiceCredentials
                           select new M_EInvoiceCredential
                           {
                               ApiPassword = p.ApiPassword,
                               Aspid = p.Aspid,
                               EInvPwd = p.EInvPwd,
                               Gstin = p.Gstin,
                               id = p.id,
                               Modifydate = p.Modifydate,
                               RectimeStamp = p.RectimeStamp,
                               Username = p.Username,
                               ActiveStatus = p.ActiveStatus == "Y" ? "Active" : "Deactive"
                           }).OrderByDescending(p => p.id).ToList();
                }
            }
            catch
            {

            }
            return obj;
        }

        public ResponseDetail SaveEInvoiceCredentail(M_EInvoiceCredential Model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseMessage = "Something went wrong!";
            objResponse.ResponseStatus = "FAILED";
            EInvoiceCredential objsave = new EInvoiceCredential();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    var lastRow = entity.EInvoiceCredentials
                                           .OrderByDescending(t => t.id)
                                            .FirstOrDefault();
                    if (lastRow != null)
                    {
                        lastRow.ActiveStatus = "N";
                        entity.SaveChanges();
                    }
                    objsave.Username = Model.Username;
                    objsave.Gstin = Model.Gstin;
                    objsave.ApiPassword = Model.ApiPassword;
                    objsave.Aspid = Model.Aspid;
                    objsave.EInvPwd = Model.EInvPwd;
                    objsave.RectimeStamp = DateTime.Now;
                    objsave.Modifydate = DateTime.Now;
                    objsave.ActiveStatus = "Y";
                    entity.EInvoiceCredentials.Add(objsave);
                    var i = entity.SaveChanges();
                    if (i > 0)
                    {

                        objResponse.ResponseStatus = "OK";
                        objResponse.ResponseMessage = "Saved Successfully!";
                    }
                    else
                    {
                        objResponse.ResponseStatus = "FAILED";
                        objResponse.ResponseMessage = "Something went wrong!";
                    }
                }
            }
            catch
            {

            }
            return objResponse;
        }
        public FPVVoucher GetCheckFpWallet(string Idno)
        {
            FPVVoucher obj = new FPVVoucher();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    sp_GetFPBalance_Result result = entity.sp_GetFPBalance(Idno, "F").FirstOrDefault();
                    if (result != null)
                    {
                        obj.balance = result.balance;
                        obj.credit = result.credit;
                        obj.debit = result.debit;
                        obj.TotalBalance = result.TotalBalance;
                    }
                }

            }
            catch
            {

            }
            return obj;
        }

        public FPVoucherEligibilityResult CheckFPVoucherEligibility(string idno)
        {
            FPVoucherEligibilityResult obj = new FPVoucherEligibilityResult();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    CheckFPVoucherEligibility_Result result = entity.CheckFPVoucherEligibility(idno).FirstOrDefault();
                    if (result != null)
                    {
                        obj.EligibilityStatus = result.EligibilityStatus;
                        obj.Reason = result.Reason;
                    }
                }
            }
            catch
            {

            }
            return obj;
        }

        public List<VoucherTypeModel> GetWalletTypes()
        {
            List<VoucherTypeModel> result = new List<VoucherTypeModel>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    result = (from v in entity.VoucherTypes
                              where v.IsWr == true
                              select new VoucherTypeModel
                              {
                                  id = v.id,
                                  Vtype = v.Vtype,
                                  Voucher_Discrption = v.Voucher_Discrption,
                                  IsWr = v.IsWr
                              }).ToList();
                }

            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public List<Courier> GetCouierList(int CourierID)
        {
            List<Courier> list = new List<Courier>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    list = (from r in entity.M_CourierMaster
                            where r.CourierId != 0
                            select new Courier
                            {
                                ID = r.CourierId,
                                Name = r.CourierName,
                                Website = r.Website,
                                ActiveStatus = r.ActiveStatus,
                                Remark = r.Remarks
                            }).ToList();
                    if (CourierID > 0)
                        list = list.Where(m => m.ID == CourierID).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }
        public Courier CourierDetailByweight(int CourierId, int Weight)
        {
            Courier list = new Courier();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    //list = (from r in entity.M_CourierMaster
                    //        where r.CourierId == CourierId && (r.weight >= Weight)
                    //        join t in entity.M_CourierMaster on r.CourierId equals t.CourierId
                    //        select new Courier
                    //        {
                    //            ID = r.Id,
                    //            Name = t.CourierName,
                    //            Amount = r.Price,
                    //            Weight = r.Weight,
                    //            ActiveStatus = "Y",
                    //            Remark = r.Remarks
                    //        }).OrderBy(o => o.Weight).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public ResponseDetail SaveBillDetail(DistributorBillModel objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                // TrnBillMain obj = new TrnBillMain();
                ShiftDataintoTemptable("TrnBillMain", "TempBillMain", ",Getdate()", "  AND UserBillNo='" + objModel.BillNo + "'");
                using (var db = new InventoryEntities())
                {
                    var obj = (from s in db.TrnBillMains where s.UserBillNo == objModel.BillNo select s).FirstOrDefault();
                    if (obj != null)
                    {
                        obj.ReceiverName = objModel.Station ?? "";
                        obj.DocketNo = objModel.DocketNo ?? "";
                        obj.DispatchStatus = objModel.FreightType != "H" ? "N" : "Y";
                        obj.LR = objModel.VehicleNo ?? "";
                        //obj.TransporterName = objModel.TransporterName ?? "";
                        obj.TransporterName = (string.IsNullOrEmpty(objModel.objProduct.CourierName) ? "" : objModel.objProduct.CourierName);
                        obj.FreightType = objModel.FreightType;
                        //obj.NetPayable=obj.NetPayable-obj.FreightAmt+ objModel.FreightAmt;
                        // obj.FreightAmt = objModel.FreightAmt;
                        DateTime BillDate = DateTime.Now;
                        if (!string.IsNullOrEmpty(objModel.DispatchDateStr))
                        {
                            BillDate = Convert.ToDateTime(objModel.DispatchDateStr);
                            BillDate = BillDate.Date;
                            obj.DocketDate = BillDate;
                        }
                        obj.Scratch = objModel.EWayBillNo ?? "";
                        obj.CourierId = objModel.objProduct.CourierId;
                        obj.CourierName = (string.IsNullOrEmpty(objModel.objProduct.CourierName) ? "" : objModel.objProduct.CourierName);
                        obj.DelvAddress = "";
                        obj.DelvPlace = string.IsNullOrEmpty(objModel.objProduct.DeliveryPlace) ? "" : objModel.objProduct.DeliveryPlace;
                        obj.Remarks = string.IsNullOrEmpty(objModel.objCustomer.Remarks) ? "" : objModel.objCustomer.Remarks;
                    }
                    int i = db.SaveChanges();
                    if (i != 0)
                    {
                        objResponse.ResponseStatus = "OK";
                        objResponse.ResponseMessage = "Bill updated Successfully!";
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return objResponse;
        }
    }
}
