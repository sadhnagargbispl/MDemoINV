using InventoryManagement.API.Models;
using InventoryManagement.Entity.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Globalization;

namespace InventoryManagement.API.Controllers
{
    public class ProductAPIController : ApiController
    {
        public ResponseDetail AddCategoryDetails(CategoryDetails model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseStatus = "FAIL";
            objResponse.ResponseMessage = "Something went wrong!";
            try
            {
                using (var entity = new InventoryEntities())
                {
                    M_CatMaster objDTCategory = new M_CatMaster();
                    objDTCategory = (from category in entity.M_CatMaster where category.CatId == model.CategoryId select category).FirstOrDefault();
                    if (objDTCategory == null)
                    {
                        objDTCategory = new M_CatMaster();
                    }
                    else
                        ShiftDataintoTemptable("M_CatMaster", "TempCatMaster", ",'" + model.UserDetails.UserId + "',Getdate()", " AND CatID='" + model.CategoryId + "'");

                    if (model.IsAdd != "Delete")
                    {
                        if (model.IsAdd == "Add")
                        {
                            decimal CategoryId = (from r in entity.M_CatMaster select r.CatId).DefaultIfEmpty(0).Max();
                            CategoryId = CategoryId + 1;
                            objDTCategory.CatId = (int)CategoryId;
                        }
                        objDTCategory.CatName = model.CategoryName;
                        objDTCategory.CatDescription = string.IsNullOrEmpty(model.Description) ? "" : model.Description;
                        objDTCategory.ActiveStatus = model.IsActive ? "Y" : "N";
                        objDTCategory.RecTimeStamp = DateTime.Now;
                        objDTCategory.UserId = model.UserDetails.UserId;
                        objDTCategory.IsForPC = "";
                        objDTCategory.DelCharge = "";
                        objDTCategory.Company = "";
                        objDTCategory.LastModified = DateTime.Now.ToString();
                        objDTCategory.WebSequence = 0;
                        objDTCategory.Remarks = "";
                        objDTCategory.Prefix = "";
                        objDTCategory.OnWebSite = "Y";
                        objDTCategory.ImgPath = model.ImgPath;
                        //objDTCategory.AlterID = model.UserDetails.UserId;

                        if (model.IsAdd == "Add")
                        {
                            objDTCategory.RecTimeStamp = DateTime.Now;
                            entity.M_CatMaster.Add(objDTCategory);
                        }

                    }
                    else
                    {
                        if (objDTCategory != null)
                        {
                            //  entity.M_CatMaster.Remove(objDTCategory);
                            objDTCategory.ActiveStatus = "N";
                        }
                    }
                    try
                    {
                        int isSaved = entity.SaveChanges();
                        if (isSaved > 0)
                        {
                            if (model.IsAdd == "Add")
                            {
                                objResponse.ResponseStatus = "OK";
                                objResponse.ResponseMessage = "Saved Successfully!";
                            }
                            else if (model.IsAdd == "Edit")
                            {
                                objResponse.ResponseStatus = "OK";
                                objResponse.ResponseMessage = "Updated Successfully!";

                            }
                            else
                            {
                                objResponse.ResponseStatus = "OK";
                                objResponse.ResponseMessage = "Deleted Successfully!";

                            }
                        }
                        else
                        {
                            objResponse.ResponseStatus = "Failed";
                            objResponse.ResponseMessage = "Something went wrong!";

                        }
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        var errors = ex.EntityValidationErrors
                            .SelectMany(e => e.ValidationErrors)
                            .Select(v => v.PropertyName + ": " + v.ErrorMessage);
                        objResponse.ResponseStatus = "FAILED";
                        objResponse.ResponseMessage = string.Join(" | ", errors);
                    }
                    //catch (Exception ex)
                    //{

                    //}
                }
            }
            catch (Exception ex)
            {

            }
            return objResponse;
        }

        public ResponseDetail AddSubCategoryDetails(SubCategoryDetails model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseStatus = "FAIL";
            objResponse.ResponseMessage = "Something went wrong!";
            try
            {
                using (var entity = new InventoryEntities())
                {
                    M_SubCatMaster objDTSubCategory = new M_SubCatMaster();
                    objDTSubCategory = (from subcategory in entity.M_SubCatMaster where subcategory.SubCatId == model.SubCatId select subcategory).FirstOrDefault();
                    if (objDTSubCategory == null)
                    {
                        objDTSubCategory = new M_SubCatMaster();
                    }
                    if (model.IsAdd != "Delete")
                    {
                        if (model.IsAdd == "Add")
                        {
                            decimal subCategoryId = (from r in entity.M_SubCatMaster select r.SubCatId).DefaultIfEmpty(0).Max();
                            subCategoryId = subCategoryId + 1;
                            objDTSubCategory.SubCatId = (int)subCategoryId;
                        }
                        else
                            ShiftDataintoTemptable("M_SubCatMaster", "TempSubCatMaster", ",'" + model.UserDetails.UserId + "',Getdate()", " AND SubCatID='" + objDTSubCategory.SubCatId + "'");
                        objDTSubCategory.Remarks = "";
                        objDTSubCategory.OfferHtml = "";
                        objDTSubCategory.IsForPC = "Y";
                        objDTSubCategory.OnWebSite = "Y";
                        objDTSubCategory.Description = string.IsNullOrEmpty(model.Description) ? "" : model.Description;
                        //objDTSubCategory.AlterID = model.UserDetails.UserId;
                        objDTSubCategory.SubCatName = model.subCategoryName;
                        //objDTSubCategory.Description = model.Description;
                        objDTSubCategory.CatId = model.CategoryId;
                        objDTSubCategory.ActiveStatus = model.IsActive ? "Y" : "N";
                        objDTSubCategory.ImgPath = model.ImgPath;
                        objDTSubCategory.UserId = model.UserDetails.UserId;
                        objDTSubCategory.LastModified = DateTime.Now.ToString();
                        if (model.IsAdd == "Add")
                        {
                            objDTSubCategory.RecTimeStamp = DateTime.Now;
                            entity.M_SubCatMaster.Add(objDTSubCategory);
                        }
                    }
                    else
                    {
                        if (objDTSubCategory != null)
                        {
                            //entity.M_SubCatMaster.Remove(objDTSubCategory);
                            objDTSubCategory.ActiveStatus = "N";
                        }
                    }
                    try
                    {
                        int isSaved = entity.SaveChanges();
                        if (isSaved > 0)
                        {
                            if (model.IsAdd == "Add")
                            {
                                objResponse.ResponseStatus = "OK";
                                objResponse.ResponseMessage = "Saved Successfully!";
                            }
                            else if (model.IsAdd == "Edit")
                            {
                                objResponse.ResponseStatus = "OK";
                                objResponse.ResponseMessage = "Updated Successfully!";
                            }
                            else
                            {
                                objResponse.ResponseStatus = "OK";
                                objResponse.ResponseMessage = "Deleted Successfully!";
                            }
                        }
                        else
                        {
                            objResponse.ResponseStatus = "Failed";
                            objResponse.ResponseMessage = "Something went wrong!";

                        }
                    }
                    catch (DbEntityValidationException ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objResponse;
        }

        public ResponseDetail IsMasterExists(CheckDuplicateModel model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseStatus = "FAIL";
            objResponse.ResponseMessage = "Match found!";
            try
            {
                using (var entity = new InventoryEntities())
                {

                    switch (model.masterTable)
                    {
                        case "":
                            objResponse.ResponseStatus = "FAIL";
                            objResponse.ResponseMessage = "Something went wrong!";
                            break;
                        case "CategoryMaster":
                            if (!string.IsNullOrEmpty(model.masterName))
                            {
                                var result = (from cm in entity.M_CatMaster where cm.CatName.ToLower().Equals(model.masterName.ToLower()) select cm.CatName).FirstOrDefault();
                                if (result == null)
                                {
                                    objResponse.ResponseStatus = "OK";
                                    objResponse.ResponseMessage = "Match not found!";
                                }
                                else if (model.isAdd == "Edit" || model.isAdd == "Delete")
                                {
                                    objResponse.ResponseStatus = "OK";
                                    objResponse.ResponseMessage = "Match not found!";


                                }
                                else
                                {
                                    objResponse.ResponseStatus = "FAIL";
                                    objResponse.ResponseMessage = "This Category Name already exists!";
                                }
                            }
                            else
                            {
                                objResponse.ResponseStatus = "FAIL";
                                objResponse.ResponseMessage = "Something went wrong!";
                            }
                            break;
                        case "SubCategoryMaster":
                            var result1 = (from cm in entity.M_SubCatMaster where cm.CatId == model.CategoryId && cm.SubCatName == model.masterName select cm).FirstOrDefault();
                            if (result1 == null)
                            {
                                objResponse.ResponseStatus = "OK";
                                objResponse.ResponseMessage = "Match not found!";
                            }
                            else if (model.isAdd == "Edit" || model.isAdd == "Delete")
                            {
                                objResponse.ResponseStatus = "OK";
                                objResponse.ResponseMessage = "Match not found!";


                            }
                            else
                            {
                                objResponse.ResponseStatus = "FAIL";
                                objResponse.ResponseMessage = "This Sub Category already exists!";

                            }
                            break;
                        case "ProductMaster":
                            var result2 = (from cm in entity.M_ProductMaster where cm.CatId == model.CategoryId && cm.SubCatId == model.SubCategoryId && cm.ProductName == model.masterName select cm).FirstOrDefault();
                            if (result2 == null)
                            {
                                objResponse.ResponseStatus = "OK";
                                objResponse.ResponseMessage = "Match not found!";
                            }
                            else if (model.isAdd == "Edit" || model.isAdd == "Delete")
                            {
                                objResponse.ResponseStatus = "OK";
                                objResponse.ResponseMessage = "Match not found!";


                            }
                            else
                            {
                                objResponse.ResponseStatus = "FAIL";
                                objResponse.ResponseMessage = "This Product already exists!";

                            }
                            break;
                        default:
                            objResponse.ResponseStatus = "FAIL";
                            objResponse.ResponseMessage = "Something went wrong!";
                            break;

                    }


                }
            }
            catch (Exception ex)
            {

            }
            return objResponse;
        }

        public List<CategoryDetails> GetCategoryList(string ActiveFlag)
        {
            List<CategoryDetails> objCategoryList = new List<CategoryDetails>();

            try
            {
                using (var entity = new InventoryEntities())
                {
                    if (!string.IsNullOrEmpty(ActiveFlag))
                    {
                        objCategoryList = (from category in entity.M_CatMaster
                                           where category.ActiveStatus == ActiveFlag
                                           select new CategoryDetails
                                           {
                                               CategoryId = (int)category.CatId,
                                               CategoryName = category.CatName,
                                               Description = category.CatDescription,
                                               ImgPath = category.ImgPath,
                                               IsActive = category.ActiveStatus == "Y" ? true : false
                                           }
                                           ).ToList();
                    }
                    else
                    {
                        objCategoryList = (from category in entity.M_CatMaster
                                               // where category.ActiveStatus == "Y"
                                           select new CategoryDetails
                                           {
                                               CategoryId = (int)category.CatId,
                                               CategoryName = category.CatName,
                                               Description = category.CatDescription,
                                               ImgPath = category.ImgPath,
                                               IsActive = category.ActiveStatus == "Y" ? true : false
                                           }
                                          ).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return objCategoryList;
        }

        public List<SubCategoryDetails> GetSubcategoryDetails(int CategoryId, string ActiveStatus)
        {
            List<SubCategoryDetails> objSubCategoryDetails = new List<SubCategoryDetails>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    if (!string.IsNullOrEmpty(ActiveStatus))
                    {
                        if (CategoryId != 0)
                        {
                            objSubCategoryDetails = (from c in entity.M_CatMaster
                                                         //join sc in entity.SubCategoryMasters on c.CategoryId equals sc.ParentCategoryId
                                                     join sc in entity.M_SubCatMaster on c.CatId equals sc.CatId
                                                     where sc.CatId == CategoryId && sc.ActiveStatus == ActiveStatus && c.ActiveStatus == "Y"
                                                     select new SubCategoryDetails
                                                     {
                                                         SubCategoryId = (int)sc.AId,
                                                         SubCatId = (int)sc.SubCatId,
                                                         CategoryId = (int)sc.CatId,
                                                         CategoryName = c.CatName,
                                                         IsActive = sc.ActiveStatus == "Y" ? true : false,
                                                         Description = sc.Description,
                                                         ImgPath = sc.ImgPath,
                                                         subCategoryName = sc.SubCatName
                                                     }).ToList();
                        }
                        else
                        {
                            objSubCategoryDetails = (from c in entity.M_CatMaster
                                                         //join sc in entity.SubCategoryMasters on c.CategoryId equals sc.ParentCategoryId
                                                     join sc in entity.M_SubCatMaster on c.CatId equals sc.CatId
                                                     where sc.ActiveStatus == ActiveStatus && c.ActiveStatus == "Y"
                                                     select new SubCategoryDetails
                                                     {
                                                         SubCategoryId = (int)sc.AId,
                                                         SubCatId = (int)sc.SubCatId,
                                                         CategoryId = (int)sc.CatId,
                                                         CategoryName = c.CatName,
                                                         Description = sc.Description,
                                                         ImgPath = sc.ImgPath,
                                                         IsActive = sc.ActiveStatus == "Y" ? true : false,
                                                         subCategoryName = sc.SubCatName
                                                     }).ToList();
                        }
                    }
                    else
                    {
                        if (CategoryId != 0)
                        {
                            objSubCategoryDetails = (from c in entity.M_CatMaster
                                                         //join sc in entity.SubCategoryMasters on c.CategoryId equals sc.ParentCategoryId
                                                     join sc in entity.M_SubCatMaster on c.CatId equals sc.CatId
                                                     where sc.CatId == CategoryId && c.ActiveStatus == "Y"
                                                     select new SubCategoryDetails
                                                     {
                                                         SubCategoryId = (int)sc.AId,
                                                         SubCatId = (int)sc.SubCatId,
                                                         CategoryId = (int)sc.CatId,
                                                         CategoryName = c.CatName,
                                                         IsActive = sc.ActiveStatus == "Y" ? true : false,
                                                         subCategoryName = sc.SubCatName,
                                                         ImgPath = sc.ImgPath,
                                                         Description = sc.Description,
                                                     }).ToList();
                        }
                        else
                        {
                            objSubCategoryDetails = (from c in entity.M_CatMaster
                                                         //join sc in entity.SubCategoryMasters on c.CategoryId equals sc.ParentCategoryId
                                                     join sc in entity.M_SubCatMaster on c.CatId equals sc.CatId
                                                     // where sc.ActiveStatus == ActiveStatus
                                                     where c.ActiveStatus == "Y"
                                                     select new SubCategoryDetails
                                                     {
                                                         SubCategoryId = (int)sc.AId,
                                                         SubCatId = (int)sc.SubCatId,
                                                         CategoryId = (int)sc.CatId,
                                                         CategoryName = c.CatName,
                                                         IsActive = sc.ActiveStatus == "Y" ? true : false,
                                                         subCategoryName = sc.SubCatName,
                                                         ImgPath = sc.ImgPath,
                                                         Description = sc.Description
                                                     }).ToList();
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return objSubCategoryDetails;
        }

        public int MaxProductCode()
        {
            decimal maxCode = 1000;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    maxCode = (from result in entity.M_ProductMaster select result.ProductCode).DefaultIfEmpty(0).Max();

                }

            }
            catch (Exception ex)
            {

            }
            if (maxCode == 0)
            {
                maxCode = 1000;
            }
            return ((int)maxCode + 1);
        }

        public int MaxBarCode()
        {
            decimal maxCode = 1000000;
            string maxCodeStr = "0";
            try
            {
                using (var entity = new InventoryEntities())
                {
                    maxCode = (from result in entity.M_BarCodeMaster select result.BCode).DefaultIfEmpty(1000000).Max();

                }

            }
            catch (Exception ex)
            {

            }
            //if (!string.IsNullOrEmpty(maxCodeStr)&& maxCodeStr!="0")
            //{
            //maxCode = decimal.Parse(maxCodeStr);
            //}
            if (maxCode == 0)
            {
                maxCode = 1000000;
            }
            return ((int)maxCode + 1);
        }
        // UI se aaya date string robustly parse karta hai. Picker "DD-MMM-YYYY"
        // (jaise "30-Jun-2026") bhejta hai; kuch jagah dd-MM-yyyy / dd/MM/yyyy bhi aa
        // sakta hai. Sab try karke, fail hone par aaj ki date de deta hai.
        private DateTime ParseUiDate(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return DateTime.Now;
            s = s.Trim();
            string[] formats =
            {
        "dd-MMM-yyyy", "d-MMM-yyyy", "dd-MM-yyyy", "dd/MM/yyyy",
        "yyyy-MM-dd", "MM/dd/yyyy", "dd-MMM-yyyy HH:mm:ss"
    };
            DateTime dt;
            if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture,
                                       DateTimeStyles.None, out dt))
                return dt;
            if (DateTime.TryParse(s, CultureInfo.InvariantCulture,
                                  DateTimeStyles.None, out dt))
                return dt;
            return DateTime.Now;   // last resort, taaki save kabhi crash na ho
        }

        public ResponseDetail SaveProductMaster(ProductDetails model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            M_ProductMaster objDTProduct = new M_ProductMaster();
            M_BarCodeMaster objDTBarcode = new M_BarCodeMaster();
            M_TaxMaster objDTTax = new M_TaxMaster();
            TrnStockJv objDtStock = new TrnStockJv();
            Im_CurrentStock objDtCurrentStock = new Im_CurrentStock();
            string objversion = "";
            M_FiscalMaster objFiscalMaster = new M_FiscalMaster();
            int i = 0;
            // decimal ProductBarcodeId = 0;
            //decimal ProductTaxId = 0 ;
            string BatchCode = "0";
            decimal Bcode = MaxBarCode();
            decimal StockJNo = 1001;
            objResponse.ResponseMessage = "Something went wrong!";
            objResponse.ResponseStatus = "FAILED";
            try
            {
                using (var entity = new InventoryEntities())
                {
                    if (model != null)
                    {
                        objFiscalMaster = (from result in entity.M_FiscalMaster where result.ActiveStatus == "Y" select result).FirstOrDefault();
                        objDTProduct = (from result in entity.M_ProductMaster where result.ProductCode == model.ProductCode select result).FirstOrDefault();
                        objversion = (from result in entity.M_NewHOVersionInfo select result.VersionNo).FirstOrDefault();
                        if (objDTProduct == null)
                        {
                            objDTProduct = new M_ProductMaster();
                            model.ProductCode = MaxProductCode();

                        }
                        else
                            ShiftDataintoTemptable("M_ProductMaster", "TempProductMaster", ",'" + model.UserDetails.UserId + "',Getdate()", " AND ProdID='" + model.ProductCode.ToString() + "'");


                        i = 0;

                        objDTProduct.ProductCode = model.ProductCode;
                        objDTProduct.ProductName = model.ProductName;
                        objDTProduct.ProdId = model.ProductCode.ToString();
                        objDTProduct.PV = model.PV;
                        objDTProduct.RP = model.RP;
                        objDTProduct.SpclOffer = model.SpecialOffer;
                        objDTProduct.HotSell = model.HotSell;
                        objDTProduct.ImagePath = string.IsNullOrEmpty(model.ProductImagePath) ? "" : model.ProductImagePath;
                        objDTProduct.ImgPath1 = string.IsNullOrEmpty(model.ProductImagePath1) ? "" : model.ProductImagePath1;
                        objDTProduct.ImgPath2 = string.IsNullOrEmpty(model.ProductImagePath2) ? "" : model.ProductImagePath2;
                        objDTProduct.ImgPath3 = string.IsNullOrEmpty(model.ProductImagePath3) ? "" : model.ProductImagePath3;
                        objDTProduct.ImgPath4 = string.IsNullOrEmpty(model.ProductImagePath4) ? "" : model.ProductImagePath4;
                        objDTProduct.ImgPath5 = string.IsNullOrEmpty(model.ProductImagePath5) ? "" : model.ProductImagePath5;
                        objDTProduct.IsImage = string.IsNullOrEmpty(model.ProductImagePath) ? "N" : "Y";
                        objDTProduct.ActiveStatus = model.IsActive ? "Y" : "N";
                        objDTProduct.IsBillingAllowed = "Y";
                        objDTProduct.IsAvailableforOffers = model.IsAvailableforOffers ? "Y" : "N";
                        objDTProduct.AllowedForFPV = model.AllowedForFPV ? "Y" : "N";
                        objDTProduct.AllowedForGV = model.AllowedForGV ? "Y" : "N";
                        objDTProduct.AllowedForMRI = model.AllowedForMRI ? "Y" : "N";
                        objDTProduct.AllowedForGPV = model.AllowedForGPV ? "Y" : "N";
                        objDTProduct.OnWebSite = model.OnWebsite;
                        objDTProduct.Weight = model.Weight;
                        objDTProduct.MsgText = string.IsNullOrEmpty(model.Message) ? "" : model.Message;
                        objDTProduct.MsgStatus = model.MessageStatus;
                        // objDTProduct.MinQty = model.MinQty;
                        objDTProduct.IMEINo = model.MinQty.ToString();
                        objDTProduct.ProdCommssn = model.ProductCommission;
                        objDTProduct.Discount = model.DiscountPer;
                        objDTProduct.DiscInRs = model.DiscountInRs;
                        objDTProduct.UserProdId = string.IsNullOrEmpty(model.UserDefinedCode) ? "" : model.UserDefinedCode;
                        objDTProduct.SubCatId = model.SubCatgeoryId;
                        objDTProduct.CatId = model.CategoryId;
                        objDTProduct.ProductDesc = model.ProductDescription;
                        objDTProduct.CV = model.CV;
                        objDTProduct.BV = model.BV;
                        objDTProduct.Sequence = model.Sequence;
                        objDTProduct.PackSize = model.Size;
                        objDTProduct.SJDiscount = model.SJPDiscount;
                        if (model.UserDetails != null)
                        {
                            objDTProduct.UserId = model.UserDetails.UserId;
                        }
                        objDTProduct.RecTimeStamp = DateTime.Now;

                        objDTProduct.GenerateBy = model.UserDetails.PartyCode;
                        objDTProduct.BrandCode = 0;
                        objDTProduct.ProductType = "P";
                        objDTProduct.Prefix = "";
                        objDTProduct.ItemType = "";
                        objDTProduct.BuyingTax = 0;
                        //objDTProduct.Weight = 0;
                        objDTProduct.PurchaseRate = model.ProductBarcodeDetails.PurchaseRate;
                        objDTProduct.DP1 = 0;
                        objDTProduct.OtherStateDP = 0;
                        objDTProduct.Exp = 0;
                        objDTProduct.Costing = 0;
                        objDTProduct.FundPoint = 0;
                        if (model.DiscountPer > 0 || model.DiscountInRs > 0)
                        {
                            objDTProduct.IsDiscount = "Y";
                        }
                        else if (model.DiscountPer > 0 && model.DiscountInRs == 0)
                        {
                            objDTProduct.IsDiscount = "Y";
                        }
                        else if (model.DiscountPer == 0 && model.DiscountInRs > 0)
                        {
                            objDTProduct.IsDiscount = "Y";
                        }
                        else
                        {
                            objDTProduct.IsDiscount = "N";
                        }
                        objDTProduct.VDiscount = 0;
                        objDTProduct.GRate = 0;
                        objDTProduct.GMCharge = 0;
                        objDTProduct.GMType = "";
                        objDTProduct.IsCardIssue = "N";
                        objDTProduct.Remarks = "";
                        objDTProduct.TaxType = "I";
                        decimal BarcodeId = (from result in entity.M_BarCodeMaster select result.BId).DefaultIfEmpty(1000000).Max();
                        BarcodeId = BarcodeId + 1;
                        objDTProduct.BId = BarcodeId;
                        objDTProduct.Imported = "N";
                        objDTProduct.BNo = "0";
                        objDTProduct.PType = "G";
                        if (model.ProductBarcodeDetails.BarcodeType == "System Generated")
                        { objDTProduct.BarcodeType = "W"; }
                        else
                        {
                            objDTProduct.BarcodeType = "O";
                        }

                        objDTProduct.BCode = Bcode;
                        objDTProduct.Barcode = (!string.IsNullOrEmpty(model.ProductBarcodeDetails.Barcode)) ? model.ProductBarcodeDetails.Barcode : "0";
                        objDTProduct.OpStockQty = model.ProductCurrentStockDetails.OpeningStockQty;
                        objDTProduct.LastModified = "";
                        objDTProduct.Val1 = 0;
                        objDTProduct.Val2 = 0;
                        objDTProduct.Company = "U";
                        objDTProduct.PurchaseFrom = "WR";
                        objDTProduct.PurchaseStore = "WR";
                        objDTProduct.IsFlexible = "N";
                        objDTProduct.IsForPC = "N";
                        objDTProduct.SubQty = 0;
                        objDTProduct.CalcKitRate = "N";
                        objDTProduct.FlexiQty = 0;
                        //objDTProduct.ImgPath1 = "";
                        //objDTProduct.ImgPath2 = "";
                        //objDTProduct.ImgPath3 = "";
                        //objDTProduct.ImgPath4 = "";
                        //objDTProduct.ImgPath5 = "";
                        objDTProduct.AlterID = model.UserDetails.UserId;
                        // objDTProduct.UnitID = 3;
                        // objDTProduct.UnitName = "Pc.";
                        objDTProduct.MRP = model.ProductBarcodeDetails.MRP;
                        objDTProduct.DP = model.ProductBarcodeDetails.DP;
                        objDTProduct.IsExpired = model.ProductBarcodeDetails.IsExpirable;
                        DateTime MfgDate = DateTime.Now;
                        DateTime ExpDate = DateTime.Now;
                        if (!string.IsNullOrEmpty(model.ProductBarcodeDetails.MfgDateStr))
                        {
                            MfgDate = ParseUiDate(model.ProductBarcodeDetails.MfgDateStr).Date;
                        }
                        //if (!string.IsNullOrEmpty(model.ProductBarcodeDetails.MfgDateStr))
                        //{
                        //    var date = Convert.ToDateTime(model.ProductBarcodeDetails.MfgDateStr);
                        //    var SplitDate = date.ToString().Split('-');
                        //    string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                        //    var NewDate1 = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
                        //    MfgDate = Convert.ToDateTime(NewDate1);
                        //    MfgDate = MfgDate.Date;
                        //}
                        if (!string.IsNullOrEmpty(model.ProductBarcodeDetails.ExpDateStr))
                        {
                            ExpDate = ParseUiDate(model.ProductBarcodeDetails.ExpDateStr).Date;
                        }
                        //if (!string.IsNullOrEmpty(model.ProductBarcodeDetails.ExpDateStr))
                        //{
                        //    var date = Convert.ToDateTime(model.ProductBarcodeDetails.ExpDateStr);
                        //    var SplitDate = date.ToString().Split('-');
                        //    string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                        //    var NewDate1 = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
                        //    ExpDate = Convert.ToDateTime(NewDate1);
                        //    ExpDate = ExpDate.Date;
                        //}
                        objDTProduct.MfgDate = MfgDate.Date;
                        objDTProduct.ExpDate = ExpDate.Date;
                        // objDTProduct.ProductBarcodeId = ProductBarcodeId;
                        // objDTProduct.ProductTaxId = ProductTaxId;
                        bool isBarcodeMasterSave = true, isTaxMasterSave = true;
                        objDTProduct.OrderDesc = "";
                        objDTProduct.GenerateBy = model.UserDetails.UserName;
                        objDTProduct.HSNCode = model.HSNCode ?? "";
                        objDTProduct.ProductLink = model.ProductLink;
                        entity.M_ProductMaster.Add(objDTProduct);
                        try
                        {
                            i = entity.SaveChanges();

                            if (i > 0)
                            {
                                if (model.ProductBarcodeDetails != null)
                                {
                                    objDTBarcode.BId = BarcodeId;
                                    objDTBarcode.SupplierCode = "0";
                                    objDTBarcode.BCode = Bcode;
                                    objDTBarcode.Company = "";
                                    objDTBarcode.Imported = "N";
                                    objDTBarcode.LastModified = "";
                                    objDTBarcode.DP1 = 0;
                                    objDTBarcode.Val1 = 0;
                                    objDTBarcode.Val2 = 0;


                                    objDTBarcode.BarCode = (!string.IsNullOrEmpty(model.ProductBarcodeDetails.Barcode)) ? model.ProductBarcodeDetails.Barcode : "0";
                                    objDTBarcode.BatchNo = (!string.IsNullOrEmpty(model.ProductBarcodeDetails.Barcode)) ? model.ProductBarcodeDetails.Barcode : "0";

                                    objDTBarcode.BarCodeType = model.ProductBarcodeDetails.BarcodeType;
                                    objDTBarcode.DP = model.ProductBarcodeDetails.DP;
                                    if (model.ProductBarcodeDetails.IsExpirable == "Y")
                                        objDTBarcode.ExpDate = ExpDate.Date;
                                    else
                                    {
                                        objDTBarcode.ExpDate = DateTime.Now;
                                    }
                                    objDTBarcode.MfgDate = MfgDate.Date;
                                    objDTBarcode.MRP = model.ProductBarcodeDetails.MRP;
                                    objDTBarcode.IsExpired = model.ProductBarcodeDetails.IsExpirable;
                                    objDTBarcode.ProdId = model.ProductCode.ToString();
                                    objDTBarcode.PRate = model.ProductBarcodeDetails.PurchaseRate;
                                    objDTBarcode.Remarks = string.IsNullOrEmpty(model.ProductBarcodeDetails.Remarks) ? "" : model.ProductBarcodeDetails.Remarks;
                                    objDTBarcode.ActiveStatus = model.IsActive ? "Y" : "N";
                                    objDTBarcode.GenerateDate = DateTime.Now;
                                    if (model.UserDetails != null)
                                    {
                                        objDTBarcode.GenerateBy = model.UserDetails.UserName;
                                        objDTBarcode.UserId = model.UserDetails.UserId;
                                        //objDTBarcode.AlterID = model.UserDetails.UserId;
                                    }
                                    objDTBarcode.RecTimeStamp = DateTime.Now;

                                    if (model.ProductBarcodeDetails.BarcodeType == "System Generated")
                                    { objDTBarcode.BType = "W"; }
                                    else
                                    {
                                        objDTBarcode.BType = "O";
                                    }

                                    //BatchCode = (from result in entity.BarcodeMasters select result.BatchCode).Max()??1000000;

                                    //objDTBarcode.BatchCode = BatchCode + 1;

                                    BatchCode = objDTBarcode.BatchNo;
                                    entity.M_BarCodeMaster.Add(objDTBarcode);
                                    try
                                    {
                                        i = entity.SaveChanges();
                                        if (i > 0)
                                        {
                                            //ProductBarcodeId = (from result in entity.M_BarCodeMaster select result.AId).Max();
                                            isBarcodeMasterSave = true;
                                        }
                                        else
                                        {
                                            isBarcodeMasterSave = false;
                                        }
                                    }
                                    catch (DbEntityValidationException ex)
                                    {

                                    }
                                }

                                if (model.ProductTaxDetails != null)
                                {
                                    objDTTax.WithCForm = 0;
                                    objDTTax.CstTax = 0;
                                    objDTTax.ActiveStatus = model.IsActive ? "Y" : "N";
                                    objDTTax.VatTax = model.ProductTaxDetails.GSTTax;
                                    objDTTax.ProdName = model.ProductName;
                                    objDTTax.Imported = "N";
                                    objDTTax.LastModified = DateTime.Now.ToString();
                                    objDTTax.Remarks = "";
                                    objDTTax.Company = "";
                                    //objDTTax.GeneratedDate = DateTime.Now;
                                    objDTTax.RecTimeStamp = DateTime.Now;

                                    objDTTax.AValue = 0;
                                    objDTTax.STax = 0;
                                    if (model.UserDetails != null)
                                    {
                                        // objDTTax.StateCode = model.UserDetails.StateCode;
                                        objDTTax.StateCode = (int)(from r in entity.M_CompanyMaster select r.CompState).FirstOrDefault();
                                        objDTTax.UserId = model.UserDetails.UserId;
                                        objDTTax.GenerateBy = model.UserDetails.UserName;
                                    }
                                    objDTTax.ProdCode = model.ProductCode.ToString();
                                    i = 0;
                                    entity.M_TaxMaster.Add(objDTTax);
                                    try
                                    {
                                        i = entity.SaveChanges();
                                        if (i > 0)
                                        {
                                            //ProductTaxId = (from result in entity.M_TaxMaster select result.AId).Max();
                                            isTaxMasterSave = true;
                                        }
                                        else
                                        {
                                            isTaxMasterSave = false;
                                        }
                                    }
                                    catch (DbEntityValidationException ex)
                                    {

                                    }
                                }


                                // i = 0;
                                objDtStock.Barcode = model.ProductBarcodeDetails.Barcode;
                                objDtStock.BatchNo = BatchCode.ToString();
                                if (model.UserDetails != null)
                                {
                                    objDtStock.UserId = model.UserDetails.UserId;
                                    objDtStock.UserName = model.UserDetails.UserName;
                                }
                                objDtStock.RecTimeStamp = DateTime.Now;
                                if (objFiscalMaster != null)
                                    objDtStock.FSessId = objFiscalMaster.FSessId;
                                else
                                    objDtStock.FSessId = 0;
                                var res = (from result in entity.TrnStockJvs select result).ToList();
                                if (res.Count == 0)
                                    objDtStock.JNo = 1001;
                                else
                                {
                                    decimal MaxJNo = (from r in res select r.JNo).DefaultIfEmpty(0).Max();
                                    objDtStock.JNo = MaxJNo + 1;
                                }
                                objDtStock.JvNo = "OPN/" + objDtStock.JNo;
                                StockJNo = objDtStock.JNo == 0 ? 1001 : objDtStock.JNo;
                                objDtStock.ProdId = model.ProductCode.ToString();
                                objDtStock.ProductName = (from result in entity.M_ProductMaster where result.ProductCode == model.ProductCode select result.ProductName).FirstOrDefault() ?? "";
                                objDtStock.ProdType = "P";
                                objDtStock.Qty = model.ProductCurrentStockDetails.OpeningStockQty;
                                objDtStock.Remarks = "Opening Stock Of Product Registration";
                                objDtStock.RefNo = objDtStock.JvNo;
                                objDtStock.JType = "O";
                                objDtStock.ActiveStatus = model.IsActive ? "Y" : "N";
                                objDtStock.Version = objversion;
                                if (model.UserDetails != null)
                                {
                                    objDtStock.PartyName = model.UserDetails.PartyName;
                                    objDtStock.FCode = model.UserDetails.FCode;
                                    objDtStock.SoldBy = model.UserDetails.FCode;
                                }

                                objDtStock.StockDate = DateTime.Now;
                                try
                                {
                                    entity.TrnStockJvs.Add(objDtStock);
                                    i = 0;
                                    i = entity.SaveChanges();
                                    if (i > 0)
                                    {

                                        //if (model.UserDetails != null)
                                        //{
                                        //    objDtCurrentStock.FCode = model.UserDetails.FCode;
                                        //}
                                        //if (objFiscalMaster != null)
                                        //{
                                        //    objDtCurrentStock.FSessId = objFiscalMaster.FSessId;
                                        //}
                                        //objDtCurrentStock.ActiveStatus = model.IsActive ? "Y" : "N";
                                        //objDtCurrentStock.ProdId = model.ProductCode.ToString();
                                        //objDtCurrentStock.ProdType = "P";
                                        //objDtCurrentStock.Qty = model.ProductCurrentStockDetails.OpeningStockQty;
                                        //objDtCurrentStock.RefNo = "OPN/" + StockJNo;
                                        //objDtCurrentStock.SupplierCode = model.UserDetails.FCode;
                                        //objDtCurrentStock.Version = objversion;
                                        //objDtCurrentStock.Remarks = "Opening Stock For Product Registration";
                                        //objDtCurrentStock.EntryBy = objDtCurrentStock.FCode;
                                        //objDtCurrentStock.RecTimeStamp = DateTime.Now;
                                        //objDtCurrentStock.BillType = "OP";
                                        //objDtCurrentStock.BType = "OP";
                                        //objDtCurrentStock.SType = "I";
                                        //objDtCurrentStock.StockFor = "Gen.Thr.TRG.OpBl";
                                        //objDtCurrentStock.IsDisp = "N";
                                        //objDtCurrentStock.InvoiceNo = "";
                                        //objDtCurrentStock.Barcode = model.ProductBarcodeDetails.Barcode;
                                        //objDtCurrentStock.BatchCode = BatchCode.ToString();
                                        //if (model.UserDetails != null)
                                        //{
                                        //    objDtCurrentStock.UserId = model.UserDetails.UserId;
                                        //    objDtCurrentStock.GroupId = model.UserDetails.GroupId;
                                        //}
                                        //objDtCurrentStock.StockDate = DateTime.Now;


                                        //entity.Im_CurrentStock.Add(objDtCurrentStock);
                                        //i = 0;
                                        //i = entity.SaveChanges();

                                        objResponse.ResponseStatus = "OK";
                                        objResponse.ResponseMessage = "Saved Successfully!";

                                    }
                                    else
                                    {
                                        objResponse.ResponseStatus = "FAILED";
                                        objResponse.ResponseMessage = "Something went wrong!";
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
                                }
                            }

                        }
                        catch (DbEntityValidationException ex)
                        {
                            objResponse.ResponseStatus = "FAILED";
                            objResponse.ResponseMessage = ex.ToString();
                        }

                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                objResponse.ResponseStatus = "FAILED";
                objResponse.ResponseMessage = ex.ToString();
            }
            return objResponse;
        }

        public ResponseDetail EditProductMaster(ProductDetails model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            M_ProductMaster objDTProduct = new M_ProductMaster();
            M_BarCodeMaster objDTBarcode = new M_BarCodeMaster();
            M_TaxMaster objDTTax = new M_TaxMaster();
            TrnStockJv objDTStock = new TrnStockJv();
            Im_CurrentStock objCurrentStock = new Im_CurrentStock();
            objResponse.ResponseMessage = "Something Went Wrong!";
            objResponse.ResponseStatus = "FAILED";
            int i = 0;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objDTProduct = (from p in entity.M_ProductMaster where p.ProductCode == model.ProductCode select p).FirstOrDefault();
                    if (objDTProduct != null)
                    {
                        ShiftDataintoTemptable("M_ProductMaster", "TempProductMaster", ",'" + model.UserDetails.UserId + "',Getdate()", " AND ProdID='" + model.ProductCode.ToString() + "'");
                        if (model != null)
                        {
                            i = 0;
                            objDTProduct.ProductCode = model.ProductCode;
                            objDTProduct.ProductName = model.ProductName;
                            objDTProduct.ProdId = model.ProductCode.ToString();
                            objDTProduct.PV = model.PV;
                            objDTProduct.RP = model.RP;
                            objDTProduct.SpclOffer = model.SpecialOffer;
                            objDTProduct.HotSell = model.HotSell;
                            if (string.IsNullOrEmpty(model.ProductImagePath) == false)

                                objDTProduct.ImagePath = string.IsNullOrEmpty(model.ProductImagePath) ? "" : model.ProductImagePath.Replace("/ProductImages/", "");
                            objDTProduct.ImgPath1 = string.IsNullOrEmpty(model.ProductImagePath1) ? "" : model.ProductImagePath1.Replace("/ProductImages/", "");
                            objDTProduct.ImgPath2 = string.IsNullOrEmpty(model.ProductImagePath2) ? "" : model.ProductImagePath2.Replace("/ProductImages/", "");
                            objDTProduct.ImgPath3 = string.IsNullOrEmpty(model.ProductImagePath3) ? "" : model.ProductImagePath3.Replace("/ProductImages/", "");
                            objDTProduct.ImgPath4 = string.IsNullOrEmpty(model.ProductImagePath4) ? "" : model.ProductImagePath4.Replace("/ProductImages/", "");
                            objDTProduct.ImgPath5 = string.IsNullOrEmpty(model.ProductImagePath5) ? "" : model.ProductImagePath5.Replace("/ProductImages/", "");

                            objDTProduct.IsImage = string.IsNullOrEmpty(model.ProductImagePath) ? "N" : "Y";
                            objDTProduct.ActiveStatus = model.IsActive ? "Y" : "N";
                            objDTProduct.IsBillingAllowed = model.IsBillingAllowed ? "Y" : "N";
                            objDTProduct.IsAvailableforOffers = model.IsAvailableforOffers ? "Y" : "N";
                            objDTProduct.AllowedForFPV = model.AllowedForFPV ? "Y" : "N";
                            objDTProduct.AllowedForGPV = model.AllowedForGPV ? "Y" : "N";

                            objDTProduct.AllowedForGV = model.AllowedForGV ? "Y" : "N";
                            objDTProduct.AllowedForMRI = model.AllowedForMRI ? "Y" : "N";
                            objDTProduct.OnWebSite = model.OnWebsite;
                            objDTProduct.Weight = model.Weight;
                            objDTProduct.MsgText = string.IsNullOrEmpty(model.Message) ? "" : model.Message;
                            objDTProduct.MsgStatus = string.IsNullOrEmpty(model.MessageStatus) ? "N" : model.MessageStatus;

                            objDTProduct.IMEINo = model.MinQty.ToString();
                            objDTProduct.ProdCommssn = model.ProductCommission;
                            objDTProduct.Discount = model.DiscountPer;
                            objDTProduct.DiscInRs = model.DiscountInRs;
                            objDTProduct.UserProdId = string.IsNullOrEmpty(model.UserDefinedCode) ? "" : model.UserDefinedCode;
                            objDTProduct.SubCatId = model.SubCatgeoryId;
                            objDTProduct.CatId = model.CategoryId;
                            objDTProduct.ProductDesc = model.ProductDescription;
                            objDTProduct.CV = model.CV;
                            objDTProduct.BV = model.BV;
                            objDTProduct.HSNCode = string.IsNullOrEmpty(model.HSNCode) ? "" : model.HSNCode;
                            objDTProduct.GenerateBy = model.UserDetails.PartyCode;
                            objDTProduct.Sequence = model.Sequence;
                            objDTProduct.PackSize = model.Size;
                            objDTProduct.PurchaseRate = model.ProductBarcodeDetails.PurchaseRate;
                            objDTProduct.SJDiscount = model.SJPDiscount;

                            if (model.DiscountPer > 0 || model.DiscountInRs > 0)
                            {
                                objDTProduct.IsDiscount = "Y";
                            }
                            else if (model.DiscountPer > 0 && model.DiscountInRs == 0)
                            {
                                objDTProduct.IsDiscount = "Y";
                            }
                            else if (model.DiscountPer == 0 && model.DiscountInRs > 0)
                            {
                                objDTProduct.IsDiscount = "Y";
                            }
                            else
                            {
                                objDTProduct.IsDiscount = "N";
                            }


                            //if (model.ProductBarcodeDetails.BarcodeType == "System Generated")
                            //{ objDTProduct.BarcodeType = "W"; }
                            //else
                            //{
                            //    objDTProduct.BarcodeType = "O";
                            //}

                            //objDTProduct.BCode = Bcode;
                            //objDTProduct.Barcode = (!string.IsNullOrEmpty(model.ProductBarcodeDetails.Barcode)) ? model.ProductBarcodeDetails.Barcode : "0";

                            //if (model.ProductBarcodeDetails.BarcodeType == "System Generated")
                            //{ objDTProduct.BarcodeType = "W"; }
                            //else
                            //{
                            //    objDTProduct.BarcodeType = "O";
                            //}

                            //objDTProduct.BCode = (string.IsNullOrEmpty(model.ProductBarcodeDetails.Barcode)) ? decimal.Parse(model.ProductBarcodeDetails.Barcode) : 0;
                            // objDTProduct.Barcode = (string.IsNullOrEmpty(model.ProductBarcodeDetails.Barcode)) ? model.ProductBarcodeDetails.Barcode : "0";
                            // objDTProduct.OpStockQty = model.ProductCurrentStockDetails.OpeningStockQty;
                            if (model.UserDetails != null)
                            {
                                objDTProduct.LastModified = model.UserDetails.UserName;
                            }


                            objDTProduct.AlterID = model.UserDetails.UserId;

                            objDTProduct.MRP = model.ProductBarcodeDetails.MRP;
                            objDTProduct.DP = model.ProductBarcodeDetails.DP;
                            objDTProduct.IsExpired = model.ProductBarcodeDetails.IsExpirable;
                            DateTime MfgDate = DateTime.Now;
                            DateTime ExpDate = DateTime.Now;
                            if (!string.IsNullOrEmpty(model.ProductBarcodeDetails.MfgDateStr))
                            {
                                MfgDate = ParseUiDate(model.ProductBarcodeDetails.MfgDateStr).Date;
                            }
                            //if (!string.IsNullOrEmpty(model.ProductBarcodeDetails.MfgDateStr))
                            //{
                            //    var date = Convert.ToDateTime(model.ProductBarcodeDetails.MfgDateStr);
                            //    var SplitDate = date.ToString().Split('-');
                            //    string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                            //    var NewDate1 = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
                            //    MfgDate = Convert.ToDateTime(NewDate1);

                            //    //var SplitDate = model.ProductBarcodeDetails.MfgDateStr.Split('-');
                            //    //string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                            //    //var NewDate1 = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                            //    //MfgDate = Convert.ToDateTime(NewDate1);
                            //    MfgDate = MfgDate.Date;
                            //}
                            if (!string.IsNullOrEmpty(model.ProductBarcodeDetails.ExpDateStr))
                            {
                                ExpDate = ParseUiDate(model.ProductBarcodeDetails.ExpDateStr).Date;
                            }
                            //if (!string.IsNullOrEmpty(model.ProductBarcodeDetails.ExpDateStr))
                            //{
                            //    var date = Convert.ToDateTime(model.ProductBarcodeDetails.ExpDateStr);
                            //    var SplitDate = date.ToString().Split('-');
                            //    string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                            //    var NewDate1 = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
                            //    ExpDate = Convert.ToDateTime(NewDate1);
                            //    ExpDate = ExpDate.Date;
                            //    //var SplitDate = model.ProductBarcodeDetails.ExpDateStr.Split('-');
                            //    //string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];

                            //    //var NewDate1 = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                            //    //ExpDate = Convert.ToDateTime(NewDate1);
                            //    //ExpDate = ExpDate.Date;
                            //}
                            objDTProduct.MfgDate = MfgDate.Date;
                            objDTProduct.ExpDate = ExpDate.Date;
                            objDTProduct.ProductLink = model.ProductLink;
                            i = entity.SaveChanges();
                            //if (i > 0)
                            //{
                            //barcode details
                            string ProductCodeStr = model.ProductCode.ToString();
                            objDTBarcode = (from b in entity.M_BarCodeMaster where b.ProdId == ProductCodeStr && b.BarCode == model.ProductBarcodeDetails.ExisitingBarcode select b).FirstOrDefault();
                            if (objDTBarcode != null)
                            {
                                ShiftDataintoTemptable("M_BarcodeMaster", "TempBarcodeMaster", ",'" + model.UserDetails.UserId + "',Getdate()", " AND ProdID='" + ProductCodeStr + "'");
                                //objDTBarcode.BCode = (string.IsNullOrEmpty(model.ProductBarcodeDetails.Barcode)) ? decimal.Parse(model.ProductBarcodeDetails.Barcode) : 0;
                                if (model.UserDetails != null)
                                {
                                    objDTBarcode.LastModified = "Product Edited by " + model.UserDetails.UserName + " on " + DateTime.Now.ToString();
                                }


                                //objDTBarcode.BarCode = (string.IsNullOrEmpty(model.ProductBarcodeDetails.Barcode)) ? model.ProductBarcodeDetails.Barcode : "0";
                                // objDTBarcode.BatchNo = (string.IsNullOrEmpty(model.ProductBarcodeDetails.Barcode)) ? model.ProductBarcodeDetails.Barcode : "0";
                                //objDTBarcode.BarCodeType = model.ProductBarcodeDetails.BarcodeType;
                                objDTBarcode.DP = model.ProductBarcodeDetails.DP;
                                if (model.ProductBarcodeDetails.IsExpirable == "Y")
                                    objDTBarcode.ExpDate = ExpDate.Date;
                                else
                                {
                                    objDTBarcode.ExpDate = DateTime.Now;
                                }
                                objDTBarcode.MfgDate = MfgDate.Date;
                                objDTBarcode.MRP = model.ProductBarcodeDetails.MRP;
                                objDTBarcode.IsExpired = model.ProductBarcodeDetails.IsExpirable;
                                objDTBarcode.ProdId = model.ProductCode.ToString();
                                objDTBarcode.PRate = model.ProductBarcodeDetails.PurchaseRate;
                                objDTBarcode.Remarks = string.IsNullOrEmpty(model.ProductBarcodeDetails.Remarks) ? "" : model.ProductBarcodeDetails.Remarks;
                                objDTBarcode.ActiveStatus = model.IsActive ? "Y" : "N";
                                //if (model.ProductBarcodeDetails.BarcodeType == "System Generated")
                                //{ objDTBarcode.BType = "W"; }
                                //else
                                //{
                                //    objDTBarcode.BType = "O";
                                //}

                                //BatchCode = (from result in entity.BarcodeMasters select result.BatchCode).Max()??1000000;

                                //objDTBarcode.BatchCode = BatchCode + 1;
                                //  objDTBarcode.BatchNo = (string.IsNullOrEmpty(model.ProductBarcodeDetails.Barcode)) ? model.ProductBarcodeDetails.Barcode : "0";
                                i = 0;
                                i = entity.SaveChanges();
                                //if (i > 0)
                                //{
                                //tax details
                                //string ProductCodeStr = model.ProductCode.ToString();
                                if (model.ProductTaxDetails != null)
                                {
                                    objDTTax = (from t in entity.M_TaxMaster where t.ProdCode == ProductCodeStr select t).FirstOrDefault();
                                    if (objDTTax != null)
                                    {
                                        ShiftDataintoTemptable("M_TaxMaster", "TempTaxMaster", ",'" + model.UserDetails.UserId + "',Getdate()", " AND ProdCode='" + ProductCodeStr + "'");
                                        objDTTax.ActiveStatus = model.IsActive ? "Y" : "N";
                                        objDTTax.VatTax = model.ProductTaxDetails.GSTTax;
                                        objDTTax.ProdName = model.ProductName;
                                        objDTTax.LastModified = "Product Edited by " + model.UserDetails.UserName + " on " + DateTime.Now.ToString();

                                        if (model.UserDetails != null)
                                        {
                                            objDTTax.UserId = model.UserDetails.UserId;
                                            objDTTax.GenerateBy = model.UserDetails.UserName;
                                        }
                                        objDTTax.ProdCode = model.ProductCode.ToString();
                                        i = 0;
                                        i = entity.SaveChanges();

                                        //if (i > 0)
                                        //{
                                        objResponse.ResponseStatus = "OK";
                                        objResponse.ResponseMessage = "Updated Succesfully!";
                                        //objDTStock = (from s in entity.TrnStockJvs where s.ProdId == ProductCodeStr && s.JType == "O" select s).FirstOrDefault();
                                        //if (objDTStock != null)
                                        //{
                                        //    //objDTStock.Barcode = model.ProductBarcodeDetails.Barcode;
                                        //    // objDTStock.BatchNo = objDTStock.Barcode.ToString();
                                        //    objDTStock.ActiveStatus = model.IsActive ? "Y" : "N";
                                        //    objDTStock.ProductName = (from result in entity.M_ProductMaster where result.ProductCode == model.ProductCode select result.ProductName).FirstOrDefault() ?? "";
                                        //    if (model.UserDetails != null)
                                        //    {
                                        //        objDTStock.PartyName = model.UserDetails.PartyName;
                                        //        objDTStock.FCode = model.UserDetails.FCode;
                                        //        objDTStock.SoldBy = model.UserDetails.FCode;
                                        //    }

                                        //    // objDTStock.StockDate = DateTime.Now;
                                        //    i = 0;
                                        //    i = entity.SaveChanges();
                                        //if (i > 0)
                                        //{
                                        //objCurrentStock = (from c in entity.Im_CurrentStock where c.ProdId == ProductCodeStr && c.BType == "O" select c).FirstOrDefault();
                                        //if (objCurrentStock != null)
                                        //{
                                        //    if (model.UserDetails != null)
                                        //    {
                                        //        objCurrentStock.FCode = model.UserDetails.FCode;
                                        //    }

                                        //    objCurrentStock.ActiveStatus = model.IsActive ? "Y" : "N";
                                        //    objCurrentStock.ProdId = model.ProductCode.ToString();

                                        //    //objCurrentStock.Qty = model.ProductCurrentStockDetails.OpeningStockQty;

                                        //    objCurrentStock.SupplierCode = model.UserDetails.FCode;


                                        //    objCurrentStock.EntryBy = objCurrentStock.FCode;

                                        //    //objCurrentStock.Barcode = model.ProductBarcodeDetails.Barcode;
                                        //    // objCurrentStock.BatchCode = objCurrentStock.Barcode.ToString();
                                        //    if (model.UserDetails != null)
                                        //    {
                                        //        objCurrentStock.UserId = model.UserDetails.UserId;
                                        //        objCurrentStock.GroupId = model.UserDetails.GroupId;
                                        //    }

                                        //    i = 0;
                                        //    i = entity.SaveChanges();

                                        //objResponse.ResponseStatus = "OK";
                                        //objResponse.ResponseMessage = "Updated Successfully!";
                                        //}
                                        //objResponse.ResponseStatus = "OK";
                                        //objResponse.ResponseMessage = "Updated Successfully!";
                                        //else
                                        //{
                                        //    objResponse.ResponseStatus = "FAILED";
                                        //    objResponse.ResponseMessage = "Something went wrong!";
                                        //}
                                        // }
                                    }
                                }
                                //}
                                //else
                                //{
                                //    //objResponse.ResponseStatus = "FAILED";
                                //    //objResponse.ResponseMessage = "Something went wrong!";
                                //}
                                //}
                            }
                            //}
                        }
                    }

                }
                //}
                // }
            }

            catch (Exception ex)
            {

            }
            return objResponse;
        }

        public ResponseDetail DeleteProductMaster(ProductDetails model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            M_ProductMaster objDTProduct = new M_ProductMaster();
            M_BarCodeMaster objDTBarcode = new M_BarCodeMaster();
            M_TaxMaster objDTTax = new M_TaxMaster();
            TrnStockJv objDtStock = new TrnStockJv();
            Im_CurrentStock objDtCurrentStock = new Im_CurrentStock();
            int i = 0;
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objDTProduct = (from p in entity.M_ProductMaster where p.ProductCode == model.ProductCode select p).FirstOrDefault();
                    if (objDTProduct != null)
                    {
                        ShiftDataintoTemptable("M_ProductMaster", "TempProductMaster", ",'" + model.UserDetails.UserId + "',Getdate()", " AND ProdID='" + model.ProductCode.ToString() + "'");
                        objDTProduct.LastModified = "Product deleted by " + model.UserDetails.UserName + " on " + DateTime.Now.ToString();
                        objDTProduct.ActiveStatus = "N";
                        i = 0;
                        i = entity.SaveChanges();
                        if (i > 0)
                        {
                            string ProductCodeStr = model.ProductCode.ToString();
                            objDTBarcode = (from b in entity.M_BarCodeMaster where b.ProdId == ProductCodeStr && b.BarCode == model.ProductBarcodeDetails.ExisitingBarcode select b).FirstOrDefault();
                            if (objDTBarcode != null)
                            {
                                ShiftDataintoTemptable("M_BarcodeMaster", "TempBarcodeMaster", ",'" + model.UserDetails.UserId + "',Getdate()", " AND ProdID='" + ProductCodeStr + "'");
                                objDTBarcode.LastModified = "Product deleted by " + model.UserDetails.UserName + " on " + DateTime.Now.ToString();
                                objDTBarcode.ActiveStatus = "N";
                                i = 0;
                                i = entity.SaveChanges();
                                if (i > 0)
                                {
                                    objDTTax = (from t in entity.M_TaxMaster where t.ProdCode == ProductCodeStr && t.StateCode == model.UserDetails.StateCode select t).FirstOrDefault();
                                    if (objDTTax != null)
                                    {
                                        ShiftDataintoTemptable("M_TaxMaster", "TempTaxMaster", ",'" + model.UserDetails.UserId + "',Getdate()", " AND ProdCode='" + ProductCodeStr + "'");
                                        objDTTax.ActiveStatus = "N";
                                        objDTTax.LastModified = "Product deleted by " + model.UserDetails.UserName + " on " + DateTime.Now.ToString();
                                        i = 0;
                                        i = entity.SaveChanges();
                                        if (i > 0)
                                        //{
                                        //    objDtStock = (from s in entity.TrnStockJvs where s.ProdId == ProductCodeStr && s.JType == "O" select s).FirstOrDefault();
                                        //    if (objDtStock != null)
                                        //    {
                                        //        objDtStock.ActiveStatus = "N";
                                        //        i = 0;
                                        //        i = entity.SaveChanges();
                                        //        if (i > 0)
                                        //        {
                                        //            objDtCurrentStock = (from c in entity.Im_CurrentStock where c.ProdId == ProductCodeStr && c.BType == "O" select c).FirstOrDefault();
                                        //            if (objDtCurrentStock != null)
                                        //            {
                                        //                objDtCurrentStock.ActiveStatus = "N";
                                        //                i = 0;
                                        //                i = entity.SaveChanges();
                                        //                if (i > 0)
                                        {
                                            objResponse.ResponseStatus = "OK";
                                            objResponse.ResponseMessage = "Deleted Succesfully!";
                                        }
                                        else
                                        {
                                            objResponse.ResponseStatus = "OK";
                                            objResponse.ResponseMessage = "Something went wrong!";
                                        }
                                        //            }
                                        //        }
                                        //    }
                                        //}
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
            return objResponse;
        }

        public List<ProductDetails> GetProductList(decimal LoginStateCode)
        {
            List<ProductDetails> objProductList = new List<ProductDetails>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objProductList = (from product in entity.M_ProductMaster
                                      join barcode in entity.M_BarCodeMaster on product.ProdId equals barcode.ProdId
                                      into barcodes
                                      from r in barcodes.Take(1)
                                          //join stockJv in entity.TrnStockJvs on product.ProdId equals stockJv.ProdId
                                          //join currentStock in entity.Im_CurrentStock on product.ProdId equals currentStock.ProdId
                                      join category in entity.M_CatMaster on product.CatId equals category.CatId
                                      join subcategory in entity.M_SubCatMaster on product.SubCatId equals subcategory.SubCatId
                                      from tax in entity.M_TaxMaster
                                      where tax.ProdCode == product.ProdId
                                      // where tax.StateCode==LoginStateCode
                                      //where currentStock.BillType == "OP"
                                      select new ProductDetails
                                      {

                                          ProductId = (int)product.AId,
                                          BV = product.BV,
                                          CategoryId = (int)product.CatId,
                                          ProductCategoryDetails = new CategoryDetails
                                          {
                                              CategoryId = (int)category.CatId,
                                              CategoryName = category.CatName
                                          },
                                          CV = product.CV,
                                          DiscountInRs = product.DiscInRs,
                                          DiscountPer = product.Discount,
                                          HotSell = product.HotSell,
                                          IsActive = product.ActiveStatus == "Y" ? true : false,
                                          IsBillingAllowed = product.IsBillingAllowed == "Y" ? true : false,
                                          IsAvailableforOffers = product.IsAvailableforOffers == "Y" ? true : false,
                                          AllowedForFPV = product.AllowedForFPV == "Y" ? true : false,
                                          AllowedForGPV = product.AllowedForGPV == "Y" ? true : false,

                                          AllowedForGV = product.AllowedForGV == "Y" ? true : false,
                                          AllowedForMRI = product.AllowedForMRI == "Y" ? true : false,
                                          Message = product.MsgText,
                                          MessageStatus = product.MsgStatus,
                                          MinQtyStr = product.IMEINo,
                                          OnWebsite = product.OnWebSite,
                                          Weight = product.Weight,
                                          ProductCode = (int)product.ProductCode,
                                          ProductCodeStr = product.ProdId,
                                          ProductCommission = product.ProdCommssn,
                                          ProductDescription = product.ProductDesc,
                                         // ProductImagePath = "ProductImages/" + (string.IsNullOrEmpty(product.ProductLink) ? product.ImagePath : product.ProductLink),
                                          ProductImagePath = string.IsNullOrEmpty(product.ProductLink) == true ? product.ImagePath : product.ProductLink,
                                          ProductImagePath1 = product.ImgPath1,
                                          ProductImagePath2 = product.ImgPath2,
                                          ProductImagePath3 = product.ImgPath3,
                                          ProductImagePath4 = product.ImgPath4,
                                          ProductImagePath5 = product.ImgPath5,
                                          ProductName = product.ProductName,
                                          PV = product.PV,
                                          RP = product.RP,
                                          SpecialOffer = product.SpclOffer,
                                          SubCatgeoryId = (int)product.SubCatId,
                                          UserDefinedCode = product.UserProdId,
                                          HSNCode = product.HSNCode,
                                          Sequence = product.Sequence,
                                          SJPDiscount = product.SJDiscount ?? 0,
                                          Size = product.PackSize,
                                          ProductBarcodeDetails = new BarcodeDetails
                                          {
                                              ExisitingBarcode = r.BarCode,
                                              Barcode = r.BarCode,
                                              BarcodeType = r.BarCodeType,
                                              BType = r.BType,
                                              DP = r.DP,
                                              ExpDate = r.ExpDate,
                                              GenerateDate = r.GenerateDate,
                                              GeneratedBy = r.GenerateBy,
                                              IsActive = r.ActiveStatus == "Y" ? true : false,
                                              IsExpirable = r.IsExpired,
                                              MfgDate = r.MfgDate,
                                              MRP = r.MRP,
                                              //ProductId=barcode.ProdId,
                                              PurchaseRate = r.PRate,
                                              Remarks = r.Remarks,
                                              UserId = (int)r.UserId

                                          },
                                          ProductCurrentStockDetails = new CurrentStockModel
                                          {
                                              OpeningStockQty = product.OpStockQty
                                          },
                                          ProductSubCategoryDetails = new SubCategoryDetails
                                          {
                                              SubCategoryId = (int)subcategory.SubCatId,
                                              subCategoryName = subcategory.SubCatName
                                          },
                                          ProductTaxDetails = new TaxDetails
                                          {
                                              GSTTax = tax.VatTax,

                                          },

                                      }


                                    ).Distinct().ToList();

                }
            }
            catch (Exception ex)
            {

            }
            return objProductList;
        }

        public ProductDetails GetProductDetail(decimal ProductId, decimal LoginStateCode)
        {
            ProductDetails objProductList = new ProductDetails();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objProductList = (from product in entity.M_ProductMaster
                                      join barcode in entity.M_BarCodeMaster on product.ProdId equals barcode.ProdId
                                      join category in entity.M_CatMaster on product.CatId equals category.CatId
                                      join subcategory in entity.M_SubCatMaster on product.SubCatId equals subcategory.SubCatId
                                      where product.ProductCode == ProductId
                                      from tax in entity.M_TaxMaster
                                      where tax.ProdCode == product.ProdId
                                      select new ProductDetails
                                      {

                                          ProductId = (int)product.AId,
                                          BV = product.BV,
                                          CategoryId = (int)product.CatId,
                                          ProductCategoryDetails = new CategoryDetails
                                          {
                                              CategoryId = (int)category.CatId,
                                              CategoryName = category.CatName
                                          },
                                          CV = product.CV,
                                          DiscountInRs = product.DiscInRs,
                                          DiscountPer = product.Discount,
                                          HotSell = product.HotSell,
                                          IsActive = product.ActiveStatus == "Y" ? true : false,
                                          IsBillingAllowed = product.IsBillingAllowed == "Y" ? true : false,
                                          IsAvailableforOffers = product.IsAvailableforOffers == "Y" ? true : false,
                                          AllowedForFPV = product.AllowedForFPV == "Y" ? true : false,
                                          AllowedForGPV = product.AllowedForGPV == "Y" ? true : false,

                                          AllowedForGV = product.AllowedForGV == "Y" ? true : false,
                                          AllowedForMRI = product.AllowedForMRI == "Y" ? true : false,
                                          Message = product.MsgText,
                                          MessageStatus = product.MsgStatus,
                                          MinQtyStr = product.IMEINo,
                                          OnWebsite = product.OnWebSite,
                                          Weight = product.Weight,
                                          ProductCode = (int)product.ProductCode,
                                          ProductCodeStr = product.ProdId,
                                          ProductCommission = product.ProdCommssn,
                                          ProductDescription = product.ProductDesc,
                                          ProductImagePath = product.ImagePath,
                                          ProductImagePath1 = product.ImgPath1,
                                          ProductImagePath2 = product.ImgPath2,
                                          ProductImagePath3 = product.ImgPath3,
                                          ProductImagePath4 = product.ImgPath4,
                                          ProductImagePath5 = product.ImgPath5,
                                          ProductName = product.ProductName,
                                          PV = product.PV,
                                          RP = product.RP,
                                          SpecialOffer = product.SpclOffer,
                                          SubCatgeoryId = (int)product.SubCatId,
                                          UserDefinedCode = product.UserProdId,
                                          HSNCode = product.HSNCode,
                                          Sequence = product.Sequence,
                                          Size = product.PackSize,
                                          SJPDiscount = product.SJDiscount ?? 0,
                                          ProductLink = product.ProductLink,
                                          ProductBarcodeDetails = new BarcodeDetails
                                          {
                                              ExisitingBarcode = barcode.BarCode,
                                              Barcode = barcode.BarCode,
                                              BarcodeType = barcode.BarCodeType,
                                              BType = barcode.BType,
                                              DP = product.DP,
                                              ExpDate = barcode.ExpDate,
                                              GenerateDate = barcode.GenerateDate,
                                              GeneratedBy = barcode.GenerateBy,
                                              IsActive = barcode.ActiveStatus == "Y" ? true : false,
                                              IsExpirable = barcode.IsExpired,
                                              MfgDate = barcode.MfgDate,
                                              MRP = product.MRP,
                                              //ProductId=barcode.ProdId,
                                              PurchaseRate = barcode.PRate,
                                              Remarks = barcode.Remarks,
                                              UserId = (int)barcode.UserId

                                          },
                                          ProductCurrentStockDetails = new CurrentStockModel
                                          {
                                              OpeningStockQty = product.OpStockQty
                                          },
                                          ProductSubCategoryDetails = new SubCategoryDetails
                                          {
                                              SubCategoryId = (int)subcategory.SubCatId,
                                              subCategoryName = subcategory.SubCatName
                                          },
                                          ProductTaxDetails = new TaxDetails
                                          {
                                              GSTTax = tax.VatTax,

                                          },

                                      }


                                    ).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {

            }
            return objProductList;
        }

        public ResponseDetail SaveBarcode(List<BarcodeDetails> barcodeList)
        {
            ResponseDetail objResponse = new ResponseDetail();
            M_BarCodeMaster objDTBarcode = new M_BarCodeMaster();
            decimal BatchCode = 1000000;
            using (var entity = new InventoryEntities())
            {
                foreach (var model in barcodeList)
                {
                    decimal BarcodeId = (from result in entity.M_BarCodeMaster select result.BId).DefaultIfEmpty(1000000).Max();
                    BarcodeId = BarcodeId + 1;



                    DateTime MfgDate = DateTime.Now;
                    DateTime ExpDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(model.MfgDateStr))
                    {
                        var SplitDate = model.MfgDateStr.Split('-');
                        string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                        var NewDate1 = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                        MfgDate = Convert.ToDateTime(NewDate1);
                        MfgDate = MfgDate.Date;
                    }
                    if (!string.IsNullOrEmpty(model.ExpDateStr))
                    {
                        var SplitDate = model.ExpDateStr.Split('-');
                        string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                        var NewDate1 = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                        ExpDate = Convert.ToDateTime(NewDate1);
                        ExpDate = ExpDate.Date;
                    }
                    if (model != null)
                    {
                        objDTBarcode.BId = BarcodeId;
                        objDTBarcode.SupplierCode = "0";
                        objDTBarcode.BCode = MaxBarCode();
                        objDTBarcode.Company = "";
                        objDTBarcode.Imported = "N";
                        objDTBarcode.LastModified = "";
                        objDTBarcode.DP1 = 0;
                        objDTBarcode.Val1 = 0;
                        objDTBarcode.Val2 = 0;


                        objDTBarcode.BarCode = (!string.IsNullOrEmpty(model.Barcode)) ? model.Barcode : "0";
                        objDTBarcode.BatchNo = (!string.IsNullOrEmpty(model.Barcode)) ? model.Barcode : "0";
                        objDTBarcode.BarCodeType = model.BarcodeType;
                        objDTBarcode.DP = model.DP;
                        if (model.IsExpirable == "Y")
                            objDTBarcode.ExpDate = ExpDate.Date;
                        else
                        {
                            objDTBarcode.ExpDate = DateTime.Now;
                        }
                        objDTBarcode.MfgDate = MfgDate.Date;
                        objDTBarcode.MRP = model.MRP;
                        objDTBarcode.IsExpired = model.IsExpirable;
                        objDTBarcode.ProdId = model.ProductCode.ToString();
                        objDTBarcode.PRate = model.PurchaseRate;
                        objDTBarcode.Remarks = string.IsNullOrEmpty(model.Remarks) ? "" : model.Remarks;
                        objDTBarcode.ActiveStatus = model.IsActive ? "Y" : "N";
                        objDTBarcode.GenerateDate = DateTime.Now;
                        objDTBarcode.GenerateBy = model.UserName;
                        objDTBarcode.UserId = model.UserId;
                        objDTBarcode.RecTimeStamp = DateTime.Now;

                        if (model.BarcodeType == "System Generated")
                        {
                            objDTBarcode.BType = "W";
                        }
                        else
                        {
                            objDTBarcode.BType = "O";
                        }

                        objDTBarcode.BatchNo = (!string.IsNullOrEmpty(model.Barcode)) ? model.Barcode : "0";
                        //BatchCode = (!string.IsNullOrEmpty(objDTBarcode.BatchNo)) ? decimal.Parse(objDTBarcode.BatchNo) : 1000001;
                        entity.M_BarCodeMaster.Add(objDTBarcode);
                    }
                    try
                    {
                        int i = entity.SaveChanges();
                        if (i > 0)
                        {
                            objResponse.ResponseStatus = "OK";
                            objResponse.ResponseMessage = "Saved Succesfully!";
                        }
                        else
                        {
                            objResponse.ResponseStatus = "FAILED";
                            objResponse.ResponseMessage = "Something went wrong!";
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {

                    }
                }
            }
            return objResponse;
        }

        public List<BarcodeDetails> GetBarcodeList()
        {
            List<BarcodeDetails> objBarcodeList = new List<BarcodeDetails>();
            try
            {
                using (var entity = new InventoryEntities())
                {

                    objBarcodeList = (from c in entity.M_BarCodeMaster
                                      join
     p in entity.M_ProductMaster on c.ProdId equals p.ProdId
                                      select new BarcodeDetails
                                      {
                                          ProductCode = c.ProdId,
                                          ProductName = p.ProductName,
                                          Barcode = c.BarCode,
                                          BarcodeType = c.BarCodeType,
                                          ActiveStatus = c.ActiveStatus,
                                          BarcodeId = c.BCode,
                                          ExpDate = c.ExpDate,
                                          MfgDate = c.MfgDate,
                                          IsExpirable = c.IsExpired == "Y" ? "Not Expired" : "Expired",

                                          //                      if (c.IsExpired = 'Y')
                                          //{
                                          //    IsExpirable = 'Not Expire'},
                                          ExpDateStr = c.ExpDate.ToString(),
                                          MfgDateStr = c.MfgDate.ToString()
                                      }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return objBarcodeList;
        }

        public BarcodeDetails getBarCodeDetail(decimal bCode)
        {
            BarcodeDetails objBarcode = new BarcodeDetails();
            try
            {
                using (var entities = new InventoryEntities())
                {
                    var Barcode = (from r in entities.M_BarCodeMaster
                                   where r.BCode == bCode
                                   select r).FirstOrDefault();
                    objBarcode.IsActive = Barcode.ActiveStatus == "Y" ? true : false;
                    objBarcode.Barcode = Barcode.BarCode;
                    objBarcode.BarcodeType = Barcode.BarCodeType;
                    objBarcode.BType = Barcode.BType;
                    objBarcode.ProductCode = Barcode.ProdId;
                    objBarcode.DP = Barcode.DP;
                    objBarcode.MRP = Barcode.MRP;
                    objBarcode.MfgDate = Barcode.MfgDate;
                    objBarcode.ExpDate = Barcode.ExpDate;
                    objBarcode.IsExpirable = Barcode.IsExpired;
                    objBarcode.Remarks = Barcode.Remarks;
                    objBarcode.BarcodeId = Barcode.BCode;
                    objBarcode.ExpDateStr = Barcode.ExpDate.ToString();
                    objBarcode.MfgDateStr = Barcode.MfgDate.ToString();
                    objBarcode.ProductName = (from r in entities.M_ProductMaster where r.ProdId == objBarcode.ProductCode select r.ProductName).FirstOrDefault();
                }
            }
            catch (Exception)
            {
            }
            return objBarcode;
        }

        public ResponseDetail UpdateBarcode(ProductDetails model)
        {
            int i = 0;
            ResponseDetail objResponse = new ResponseDetail();
            M_BarCodeMaster objDTBarcode = new M_BarCodeMaster();
            //var entity = new InventoryEntities();

            DateTime MfgDate = DateTime.Now;
            DateTime ExpDate = DateTime.Now;


            using (var entity = new InventoryEntities())
            {
                objDTBarcode = (from b in entity.M_BarCodeMaster where b.BCode == model.ProductBarcodeDetails.BarcodeId select b).FirstOrDefault();
                if (objDTBarcode != null)
                {
                    ShiftDataintoTemptable("M_BarcodeMaster", "TempBarcodeMaster", ",'" + model.UserDetails.UserId + "',Getdate()", " AND Bcode='" + objDTBarcode.BCode + "'");
                    //objDTBarcode.BCode = (string.IsNullOrEmpty(model.ProductBarcodeDetails.Barcode)) ? decimal.Parse(model.ProductBarcodeDetails.Barcode) : 0;
                    if (model.UserDetails != null)
                    {
                        objDTBarcode.LastModified = "Barcode Edited by " + model.UserDetails.UserName + " on " + DateTime.Now.ToString();
                    }

                    if (!string.IsNullOrEmpty(model.ProductBarcodeDetails.MfgDateStr))
                    {
                        var SplitDate = model.ProductBarcodeDetails.MfgDateStr.Split('-');
                        string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                        var NewDate1 = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                        MfgDate = Convert.ToDateTime(NewDate1);
                        MfgDate = MfgDate.Date;
                    }
                    if (!string.IsNullOrEmpty(model.ProductBarcodeDetails.ExpDateStr))
                    {
                        var SplitDate = model.ProductBarcodeDetails.ExpDateStr.Split('-');
                        string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                        var NewDate1 = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                        ExpDate = Convert.ToDateTime(NewDate1);
                        ExpDate = ExpDate.Date;
                    }
                    //objDTBarcode.MfgDate = MfgDate.Date;
                    //objDTBarcode.ExpDate = ExpDate.Date;
                    objDTBarcode.BCode = model.ProductBarcodeDetails.BarcodeId;
                    objDTBarcode.IsExpired = model.ProductBarcodeDetails.IsExpirable;

                    if (model.ProductBarcodeDetails.IsExpirable == "Y")
                        objDTBarcode.ExpDate = ExpDate.Date;
                    else
                    {
                        objDTBarcode.ExpDate = DateTime.Now;
                    }
                    objDTBarcode.MfgDate = MfgDate.Date;

                    objDTBarcode.IsExpired = model.ProductBarcodeDetails.IsExpirable;

                    objDTBarcode.Remarks = string.IsNullOrEmpty(model.ProductBarcodeDetails.Remarks) ? "" : model.ProductBarcodeDetails.Remarks;
                    objDTBarcode.ActiveStatus = model.IsActive ? "Y" : "N";

                    //if (model.ProductBarcodeDetails.BarcodeType == "System Generated")
                    //{
                    //    objDTBarcode.BType = "W";
                    //}
                    //else
                    //{
                    //    objDTBarcode.BType = "O";
                    //}

                    i = 0;
                    i = entity.SaveChanges();

                    objResponse.ResponseStatus = "OK";
                    objResponse.ResponseMessage = "Updated Succesfully!";

                }

            }
            return objResponse;
        }

        public ResponseDetail ActivateBarcode(string BCode)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                decimal dCode = Convert.ToDecimal(BCode);
                using (var entities = new InventoryEntities())
                {
                    var objBarcode = (from r in entities.M_BarCodeMaster
                                      where r.BCode == dCode
                                      select r).FirstOrDefault();
                    objBarcode.ActiveStatus = "Y";

                    int i = entities.SaveChanges();
                    if (i > 0)
                    {
                        objResponse.ResponseStatus = "OK";
                        objResponse.ResponseMessage = "Activated Succesfully!";
                    }
                    else
                    {
                        objResponse.ResponseStatus = "FAILED";
                        objResponse.ResponseMessage = "Something went wrong!";
                    }
                }
            }
            catch (Exception)
            {
                objResponse.ResponseStatus = "FAILED";
                objResponse.ResponseMessage = "Something went wrong!";
            }
            return objResponse;
        }

        public ResponseDetail DeActivateBarcode(string BCode)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                decimal dCode = Convert.ToDecimal(BCode);
                using (var entities = new InventoryEntities())
                {
                    var objBarcode = (from r in entities.M_BarCodeMaster
                                      where r.BCode == dCode
                                      select r).FirstOrDefault();
                    objBarcode.ActiveStatus = "N";

                    int i = entities.SaveChanges();
                    if (i > 0)
                    {
                        objResponse.ResponseStatus = "OK";
                        objResponse.ResponseMessage = "DeActivated Succesfully!";
                    }
                    else
                    {
                        objResponse.ResponseStatus = "FAILED";
                        objResponse.ResponseMessage = "Something went wrong!";
                    }
                }
            }
            catch (Exception)
            {
                objResponse.ResponseStatus = "FAILED";
                objResponse.ResponseMessage = "Something went wrong!";
            }
            return objResponse;
        }



        public ResponseDetail SaveBatchcode(List<BatchCode> batchcodeList)
        {
            ResponseDetail objResponse = new ResponseDetail();
            M_BatchMaster objDTBarcode = new M_BatchMaster();
            // decimal BatchCode = 1000000;
            using (var entity = new InventoryEntities())
            {
                foreach (var model in batchcodeList)
                {
                    decimal BarcodeId = (from result in entity.M_BatchMaster select result.BatchCode).DefaultIfEmpty(1000000).Max();
                    BarcodeId = BarcodeId + 1;



                    DateTime MfgDate = DateTime.Now;
                    DateTime ExpDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(model.MfgDateStr))
                    {
                        var SplitDate = model.MfgDateStr.Split('-');
                        string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                        var NewDate1 = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                        MfgDate = Convert.ToDateTime(NewDate1);
                        MfgDate = MfgDate.Date;
                    }
                    if (!string.IsNullOrEmpty(model.ExpDateStr))
                    {
                        var SplitDate = model.ExpDateStr.Split('-');
                        string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                        var NewDate1 = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                        ExpDate = Convert.ToDateTime(NewDate1);
                        ExpDate = ExpDate.Date;
                    }
                    if (model != null)
                    {
                        objDTBarcode.BatchCode = BarcodeId;
                        objDTBarcode.SupplierCode = "0";
                        // objDTBarcode.BCode = MaxBarCode();
                        objDTBarcode.Company = "";
                        objDTBarcode.Imported = "N";
                        // objDTBarcode.LastModified = "";
                        objDTBarcode.DP1 = 0;
                        objDTBarcode.Val1 = 0;
                        objDTBarcode.Val2 = 0;

                        objDTBarcode.Bv = model.Bv;
                        objDTBarcode.PV = model.PV;
                        //  objDTBarcode.BatchCode  = (model.Batchno );
                        objDTBarcode.BatchNo = (!string.IsNullOrEmpty(model.Batchcode)) ? model.Batchcode : "0";
                        // objDTBarcode.BarCodeType = model.BarcodeType;
                        objDTBarcode.Dp = model.DP;
                        if (model.IsExpirable == "Y")
                            objDTBarcode.ExpDate = ExpDate.Date;
                        else
                        {
                            objDTBarcode.ExpDate = DateTime.Now;
                        }
                        objDTBarcode.MfgDate = MfgDate.Date;
                        objDTBarcode.Mrp = model.MRP;
                        objDTBarcode.IsExpired = model.IsExpirable;
                        objDTBarcode.ProdId = model.ProductCode.ToString();
                        objDTBarcode.Costing = model.PurchaseRate;
                        objDTBarcode.Pfx = "";
                        objDTBarcode.Remarks = string.IsNullOrEmpty(model.Remarks) ? "" : model.Remarks;
                        objDTBarcode.ActiveStatus = model.IsActive ? "Y" : "N";
                        objDTBarcode.RecTimeStamp = DateTime.Now;
                        objDTBarcode.BatchBy = string.IsNullOrEmpty(model.UserName) ? "" : model.UserName;
                        objDTBarcode.PurchaseFrom = string.IsNullOrEmpty(model.purchasefrom) ? "" : model.purchasefrom;
                        //objDTBarcode.BatchNo = (!string.IsNullOrEmpty(model.Barcode)) ? model.Barcode : "0";
                        //BatchCode = (!string.IsNullOrEmpty(objDTBarcode.BatchNo)) ? decimal.Parse(objDTBarcode.BatchNo) : 1000001;
                        entity.M_BatchMaster.Add(objDTBarcode);
                    }
                    try
                    {
                        int i = entity.SaveChanges();
                        if (i > 0)
                        {
                            objResponse.ResponseStatus = "OK";
                            objResponse.ResponseMessage = "Saved Succesfully!";
                        }
                        else
                        {
                            objResponse.ResponseStatus = "FAILED";
                            objResponse.ResponseMessage = "Something went wrong!";
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {

                    }
                }
            }
            return objResponse;
        }

        public List<BatchCode> GetBatchcodeList()
        {
            List<BatchCode> objBatchcodeList = new List<BatchCode>();
            try
            {
                using (var entity = new InventoryEntities())
                {

                    objBatchcodeList = (from c in entity.M_BatchMaster
                                        join
       p in entity.M_ProductMaster on c.ProdId equals p.ProdId
                                        select new BatchCode
                                        {
                                            ProductCode = c.ProdId,
                                            ProductName = p.ProductName,
                                            Batchno = c.BatchCode,
                                            Bv = c.Bv,
                                            MRP = c.Mrp,
                                            DP = c.Dp,
                                            BatchcodeId = c.BId,
                                            //BarcodeType = c.BarCodeType,
                                            ActiveStatus = c.ActiveStatus,
                                            Batchcode = c.BatchNo,
                                            ExpDate = c.ExpDate,
                                            MfgDate = c.MfgDate,
                                            IsExpirable = c.IsExpired == "Y" && c.ExpDate <= DateTime.Today ? " Expired" : "Not Expired",
                                            IsExpireStatus = c.IsExpired,

                                            //                      if (c.IsExpired = 'Y')
                                            //{
                                            //    IsExpirable = 'Not Expire'},

                                            ExpDateStr = c.IsExpired == "Y" ? c.ExpDate.ToString() : "Not Expired",


                                            MfgDateStr = c.MfgDate.ToString(),
                                            PV = c.PV ?? 0
                                        }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return objBatchcodeList;
        }

        public BatchCode getBatchCodeDetail(decimal bCode)
        {
            BatchCode objBatchcode = new BatchCode();
            try
            {
                using (var entities = new InventoryEntities())
                {
                    var BatchCode = (from r in entities.M_BatchMaster
                                     where r.BId == bCode
                                     select r).FirstOrDefault();
                    objBatchcode.IsActive = BatchCode.ActiveStatus == "Y" ? true : false;
                    objBatchcode.Batchcode = BatchCode.BatchNo;
                    objBatchcode.Batchno = BatchCode.BatchCode;
                    // objBarcode.BarcodeType = Barcode.BarCodeType;
                    //objBarcode.BType = Barcode.BType;
                    objBatchcode.Bv = BatchCode.Bv;
                    objBatchcode.ProductCode = BatchCode.ProdId;
                    objBatchcode.DP = BatchCode.Dp;
                    objBatchcode.MRP = BatchCode.Mrp;
                    objBatchcode.MfgDate = BatchCode.MfgDate;
                    objBatchcode.ExpDate = BatchCode.ExpDate;
                    objBatchcode.IsExpirable = BatchCode.IsExpired;
                    objBatchcode.Remarks = BatchCode.Remarks;
                    objBatchcode.BatchcodeId = BatchCode.BatchCode;
                    objBatchcode.ExpDateStr = BatchCode.ExpDate.ToString();
                    objBatchcode.MfgDateStr = BatchCode.MfgDate.ToString();
                    objBatchcode.PurchaseRate = BatchCode.Costing;
                    objBatchcode.BatchcodeId = BatchCode.BId;
                    objBatchcode.PV = BatchCode.PV ?? 0;
                    // objBatchcode .PurchaseRate =BatchCode.Purchas
                    objBatchcode.ProductName = (from r in entities.M_ProductMaster where r.ProdId == objBatchcode.ProductCode select r.ProductName).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return objBatchcode;
        }

        public ResponseDetail UpdateBatchcode(ProductDetails model)
        {
            int i = 0;
            ResponseDetail objResponse = new ResponseDetail();
            M_BatchMaster objDTBatchcode = new M_BatchMaster();
            //var entity = new InventoryEntities();

            DateTime MfgDate = DateTime.Now;
            DateTime ExpDate = DateTime.Now;


            using (var entity = new InventoryEntities())
            {
                objDTBatchcode = (from b in entity.M_BatchMaster where b.BId == model.productbatchcodedetails.BatchcodeId select b).FirstOrDefault();
                if (objDTBatchcode != null)
                {
                    // ShiftDataintoTemptable("M_BarcodeMaster", "TempBarcodeMaster", ",'" + model.UserDetails.UserId + "',Getdate()", " AND Bcode='" + objDTBarcode.BCode + "'");
                    //objDTBarcode.BCode = (string.IsNullOrEmpty(model.ProductBarcodeDetails.Barcode)) ? decimal.Parse(model.ProductBarcodeDetails.Barcode) : 0;
                    //if (model.UserDetails != null)
                    //{
                    //    objDTBatchcode.LastModified = "Batchcode Edited by " + model.UserDetails.UserName + " on " + DateTime.Now.ToString();
                    //}

                    if (!string.IsNullOrEmpty(model.productbatchcodedetails.MfgDateStr))
                    {
                        var SplitDate = model.productbatchcodedetails.MfgDateStr.Split('-');
                        string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                        var NewDate1 = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                        MfgDate = Convert.ToDateTime(NewDate1);
                        MfgDate = MfgDate.Date;
                    }
                    if (!string.IsNullOrEmpty(model.productbatchcodedetails.ExpDateStr))
                    {
                        var SplitDate = model.productbatchcodedetails.ExpDateStr.Split('-');
                        string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                        var NewDate1 = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                        ExpDate = Convert.ToDateTime(NewDate1);
                        ExpDate = ExpDate.Date;
                    }
                    //  objDTBatchcode.BatchCode  = model.productbatchcodedetails.BatchcodeId  ;
                    objDTBatchcode.IsExpired = model.productbatchcodedetails.IsExpirable;

                    if (model.productbatchcodedetails.IsExpirable == "Y")
                        objDTBatchcode.ExpDate = ExpDate.Date;
                    else
                    {
                        objDTBatchcode.ExpDate = DateTime.Now;
                    }
                    objDTBatchcode.MfgDate = MfgDate.Date;

                    objDTBatchcode.IsExpired = model.productbatchcodedetails.IsExpirable;

                    objDTBatchcode.Remarks = string.IsNullOrEmpty(model.productbatchcodedetails.Remarks) ? "" : model.productbatchcodedetails.Remarks;
                    objDTBatchcode.ActiveStatus = model.productbatchcodedetails.IsActive ? "Y" : "N";
                    objDTBatchcode.Bv = model.productbatchcodedetails.Bv;
                    objDTBatchcode.PV = model.productbatchcodedetails.PV;
                    objDTBatchcode.Mrp = model.productbatchcodedetails.MRP;
                    objDTBatchcode.Dp = model.productbatchcodedetails.DP;
                    objDTBatchcode.Costing = model.productbatchcodedetails.PurchaseRate;

                    i = 0;

                    //i = entity.SaveChanges();

                    //objResponse.ResponseStatus = "OK";
                    //objResponse.ResponseMessage = "Updated Succesfully!";
                    try
                    {
                        i = entity.SaveChanges();
                        if (i > 0)
                        {
                            objResponse.ResponseStatus = "OK";
                            objResponse.ResponseMessage = "Updated Successfully!";
                        }
                        else
                        {
                            objResponse.ResponseStatus = "ERROR";
                            objResponse.ResponseMessage = "No data was saved.";
                        }
                    }
                    catch (Exception ex)
                    {
                        objResponse.ResponseStatus = "ERROR";
                        objResponse.ResponseMessage = "Error occurred: " + ex.Message;
                    }



                }

            }
            return objResponse;
        }

        public ResponseDetail ActivateBatchcode(string BCode)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                decimal dCode = Convert.ToDecimal(BCode);
                using (var entities = new InventoryEntities())
                {
                    var objBatchcode = (from r in entities.M_BatchMaster
                                        where r.BatchCode == dCode
                                        select r).FirstOrDefault();
                    objBatchcode.ActiveStatus = "Y";

                    int i = entities.SaveChanges();
                    if (i > 0)
                    {
                        objResponse.ResponseStatus = "OK";
                        objResponse.ResponseMessage = "Activated Succesfully!";
                    }
                    else
                    {
                        objResponse.ResponseStatus = "FAILED";
                        objResponse.ResponseMessage = "Something went wrong!";
                    }
                }
            }
            catch (Exception)
            {
                objResponse.ResponseStatus = "FAILED";
                objResponse.ResponseMessage = "Something went wrong!";
            }
            return objResponse;
        }

        public ResponseDetail DeActivateBatchcode(string BCode)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                decimal dCode = Convert.ToDecimal(BCode);
                using (var entities = new InventoryEntities())
                {
                    var objBatchcode = (from r in entities.M_BatchMaster
                                        where r.BatchCode == dCode
                                        select r).FirstOrDefault();
                    objBatchcode.ActiveStatus = "N";

                    int i = entities.SaveChanges();
                    if (i > 0)
                    {
                        objResponse.ResponseStatus = "OK";
                        objResponse.ResponseMessage = "DeActivated Succesfully!";
                    }
                    else
                    {
                        objResponse.ResponseStatus = "FAILED";
                        objResponse.ResponseMessage = "Something went wrong!";
                    }
                }
            }
            catch (Exception)
            {
                objResponse.ResponseStatus = "FAILED";
                objResponse.ResponseMessage = "Something went wrong!";
            }
            return objResponse;
        }

        public ResponseDetail CheckBarcodeCode(string batchcode, string prodid)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                // decimal dCode = Convert.ToDecimal(BCode);
                using (var entities = new InventoryEntities())
                {
                    var objBatchcode = (from r in entities.M_BarCodeMaster
                                        where r.BarCode == batchcode && r.ProdId == prodid
                                        select r).FirstOrDefault();
                    if (objBatchcode != null)
                    {

                        objResponse.ResponseStatus = "FAILED";
                        objResponse.ResponseMessage = "BarCode already Exists!";

                    }
                    else
                    {
                        objResponse.ResponseStatus = "OK";
                        objResponse.ResponseMessage = "BarCode Not Exists!";

                    }
                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseStatus = "FAILED";
                objResponse.ResponseMessage = "Something went wrong!";
            }
            return objResponse;
        }

        public ResponseDetail CheckDuplicateBatchCode(string batchcode, string prodid)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                // decimal dCode = Convert.ToDecimal(BCode);
                using (var entities = new InventoryEntities())
                {
                    var objBatchcode = (from r in entities.M_BatchMaster
                                        where r.BatchNo == batchcode && r.ProdId == prodid
                                        select r).FirstOrDefault();
                    if (objBatchcode != null)
                    {

                        objResponse.ResponseStatus = "FAILED";
                        objResponse.ResponseMessage = "Batch No already Exists!";

                    }
                    else
                    {
                        objResponse.ResponseStatus = "OK";
                        objResponse.ResponseMessage = "Batch No Not Exists!";

                    }
                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseStatus = "FAILED";
                objResponse.ResponseMessage = "Something went wrong!";
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
            catch (Exception)
            {
            }
        }
        public List<GSTMasterLIST> GetGSTList()
        {
            List<GSTMasterLIST> objGSTList = new List<GSTMasterLIST>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objGSTList = (from c in entity.GSTMasters
                                  select new GSTMasterLIST
                                  {
                                      id = c.id,
                                      GSTTax = c.GSTTax
                                  }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return objGSTList;
        }

        public ResponseDetail SaveGST(GSTMasterLIST GStList)
        {
            ResponseDetail objResponse = new ResponseDetail();
            GSTMaster objGStlist = new GSTMaster();
            using (var entity = new InventoryEntities())
            {
                try
                {
                    var objgst = (from r in entity.GSTMasters
                                  where r.GSTTax == GStList.GSTTax
                                  select r).FirstOrDefault();
                    if (objgst == null)
                    {
                        objGStlist.GSTTax = GStList.GSTTax;
                        entity.GSTMasters.Add(objGStlist);
                        int i = entity.SaveChanges();
                        if (i > 0)
                        {
                            objResponse.ResponseStatus = "OK";
                            objResponse.ResponseMessage = "Saved Succesfully!";
                        }
                        else
                        {
                            objResponse.ResponseStatus = "FAILED";
                            objResponse.ResponseMessage = "Something went wrong!";
                        }
                    }
                    else
                    {
                        objResponse.ResponseStatus = "FAILED";
                        objResponse.ResponseMessage = GStList.GSTTax + " already exists";
                    }

                }
                catch (DbEntityValidationException ex)
                {

                }
            }
            return objResponse;
        }
    }
}
