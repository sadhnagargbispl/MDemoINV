using InventoryManagement.App_Start;
using InventoryManagement.Business;
using InventoryManagement.Common;
using InventoryManagement.Entity.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        // GET: CategoryMaster
        ProductManager objProductManager = new ProductManager();
        
        [SessionExpire]
        public ActionResult CategoryMaster()
        {
            CategoryDetails objCategoryModel = new CategoryDetails();
            objCategoryModel.IsAdd = "Add";
            objCategoryModel.IsActive = true;
        if(  new TransactionController(). CanUserAccessMenu((Session["LoginUser"] as User).UserId, "CategoryMaster"))
            return View(objCategoryModel);
            else
              return RedirectToAction("Dashboard", "Home");
        }

        // POST: SaveCategoryMaster
        [HttpPost]
        public ActionResult SaveCategoryMaster(CategoryDetails model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            if (model != null)
            {
                if (Session["LoginUser"] != null)
                {
                    model.UserDetails = Session["LoginUser"] as User;
                }
                objResponse = objProductManager.AddCategoryDetails(model);
            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        // GET: SubCategoryMaster
        [SessionExpire]
        public ActionResult SubCategoryMaster()
        {
            List<SelectListItem> objCategoryList = new List<SelectListItem>();
            List<SelectListItem> objChildCategoryList = new List<SelectListItem>();
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
            // ViewBag.ListChildCategory = objCategoryList;
            model.IsAdd = "Add";
            model.IsActive = true;
   
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "SubCategoryMaster"))
                return View(model);
            else
                 return RedirectToAction("Dashboard", "Home");
        }

        // POST: SaveSubCategoryMaster
        [HttpPost]
        public ActionResult SaveSubCategoryMaster(SubCategoryDetails model)
        {
            ResponseDetail objResponse = new ResponseDetail();

            if (model != null)
            {
                if (Session["LoginUser"] != null)
                {
                    model.UserDetails = Session["LoginUser"] as User;
                }
                objResponse = objProductManager.AddSubCategoryDetails(model);
            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetFullSubCategoryList()
        {
            List<SubCategoryDetails> objList = objProductManager.GetSubcategoryDetails(0, "");
            return Json(objList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetFullCategoryList()
        {
            List<CategoryDetails> objList = objProductManager.GetCategoryList("");
            return Json(objList, JsonRequestBehavior.AllowGet);
        }

        // GET: ProductMaster
        [SessionExpire]
        public ActionResult ProductMaster(string ActionName, string ProductCode)
        {
            List<CategoryDetails> objCategoryDetails = objProductManager.GetCategoryList("Y");

            List<SelectListItem> ListCategoryDetails = new List<SelectListItem>();
            List<SelectListItem> ListSubCategoryDetails = new List<SelectListItem>();
            List<SelectListItem> ListGST = new List<SelectListItem>(); 
            ProductDetails model = new ProductDetails();
            bool f = true;
            foreach (var obj in objCategoryDetails)
            {
                SelectListItem objTemp = new SelectListItem();
                objTemp.Text = obj.CategoryName;
                objTemp.Value = obj.CategoryId.ToString();
                if (f)
                {
                    objTemp.Selected = true;
                    model.CategoryId = obj.CategoryId;
                    f = false;
                }
                ListCategoryDetails.Add(objTemp);
            }
            f = true;
            List<SubCategoryDetails> objSubCategoryDetails = objProductManager.GetSubcategoryDetails(model.CategoryId, "Y");
            foreach (var obj in objSubCategoryDetails)
            {
                SelectListItem objTemp = new SelectListItem();
                objTemp.Text = obj.subCategoryName;
                objTemp.Value = obj.SubCatId.ToString();
                if (f)
                {
                    objTemp.Selected = true;
                    model.SubCatgeoryId = (int)obj.SubCatId;
                    f = false;
                }
                ListSubCategoryDetails.Add(objTemp);
            }
            List<GSTMasterLIST> objList = objProductManager.GetGSTList();
            bool G = true;
            foreach (var obj in objList)
            {
                SelectListItem objTemp = new SelectListItem();
                objTemp.Text = Convert.ToString(obj.GSTTax)+" %";
                objTemp.Value = Convert.ToString(obj.GSTTax);
                if (G)
                {
                    objTemp.Selected = true;
                    model.ProductTaxDetails = new TaxDetails();
                    model.ProductTaxDetails.GSTTax = (decimal)obj.GSTTax;
                    G = false;
                }
                ListGST.Add(objTemp);
            }
            ViewBag.ListCategory = ListCategoryDetails;
            ViewBag.ListSubCategory = ListSubCategoryDetails;
            ViewBag.ListGST = ListGST;
            List<SelectListItem> objBarcodeType = new List<SelectListItem>();
            objBarcodeType.Add(new SelectListItem { Text = "System Generated", Value = "System Generated" });
            objBarcodeType.Add(new SelectListItem { Text = "Other", Value = "Other" });
            ViewBag.ListBarcodetype = objBarcodeType;
            model.ProductBarcodeDetails = new BarcodeDetails();
            model.ProductBarcodeDetails.BarcodeType = "System Generated";
            model.ProductBarcodeDetails.Barcode = objProductManager.MaxBarCode().ToString();
            model.ProductBarcodeDetails.ExisitingBarcode = model.ProductBarcodeDetails.Barcode;
            model.ProductCode = objProductManager.MaxProductCode();
            model.ProductImagePath = "/images/DefaultProduct.jpg";
            model.ProductImagePath1 = "/images/DefaultProduct.jpg";
            model.ProductImagePath2 = "/images/DefaultProduct.jpg";
            model.ProductImagePath3 = "/images/DefaultProduct.jpg";
            model.ProductImagePath4 = "/images/DefaultProduct.jpg";
            model.ProductImagePath5 = "/images/DefaultProduct.jpg";
            if (!string.IsNullOrEmpty(ActionName))
            {
                if (ActionName == "Edit")
                {
                    
                    if (!string.IsNullOrEmpty(ProductCode))
                    {
                        //if (!string.IsNullOrEmpty(Passedmodel.ProductImagePath))
                        //{
                        //    var test = Url.Action("GetImage", "Common", new { imageName = "-1" });
                        //    var productPic = test.Replace("-1", Passedmodel.ProductImagePath);
                        //    Passedmodel.ProductImagePath = productPic;
                        //}
                        //else
                        //{
                        //    Passedmodel.ProductImagePath = "/images/DefaultProduct.jpg";
                        //}
                        //model = Passedmodel;
                        decimal productId = decimal.Parse(ProductCode);
                        decimal LoginStateCode = (Session["LoginUser"] as User).StateCode;
                        model = objProductManager.GetProductDetail(productId,LoginStateCode);


                        objSubCategoryDetails = objProductManager.GetSubcategoryDetails(model.CategoryId, "Y");
                        ListSubCategoryDetails = new List<SelectListItem>();
                        foreach (var obj in objSubCategoryDetails)
                        {
                            SelectListItem objTemp = new SelectListItem();
                            objTemp.Text = obj.subCategoryName;
                            objTemp.Value = obj.SubCatId.ToString();
                            if (f)
                            {
                                objTemp.Selected = true;
                                model.SubCatgeoryId = (int)obj.SubCatId;
                                f = false;
                            }
                            ListSubCategoryDetails.Add(objTemp);
                        }
                        ViewBag.ListSubCategory = ListSubCategoryDetails;


                        ListGST = new List<SelectListItem>();
                        foreach (var obj in objList)
                        {
                            SelectListItem objTemp = new SelectListItem();
                            objTemp.Text = Convert.ToString(obj.GSTTax) + " %";
                            objTemp.Value = Convert.ToString(obj.GSTTax);
                            if (obj.GSTTax== model.ProductTaxDetails.GSTTax)
                            {
                                objTemp.Selected = true;
                                model.ProductTaxDetails = new TaxDetails();
                                model.ProductTaxDetails.GSTTax = (decimal)obj.GSTTax;
                            }
                            ListGST.Add(objTemp);
                        }
                        ViewBag.ListGST = ListGST;
                        if (model != null)
                        {
                            model.IsAdd = "Edit";
                            model.MinQty = string.IsNullOrEmpty(model.MinQtyStr) ? 0 : decimal.Parse(model.MinQtyStr);
                        }
                        if (!string.IsNullOrEmpty(model.ProductImagePath))
                        {
                            model.ProductImagePath = "/ProductImages/" + model.ProductImagePath;
                           // model.ProductImagePath = model.ProductImagePath;
                        }
                        else
                        {
                            model.ProductImagePath = "/images/DefaultProduct.jpg";
                        }
                        if (!string.IsNullOrEmpty(model.ProductImagePath1))
                        {
                            model.ProductImagePath1 = "/ProductImages/" + model.ProductImagePath1;
                        }
                        else
                        {
                            model.ProductImagePath1 = "/images/DefaultProduct.jpg";
                        }
                        if (!string.IsNullOrEmpty(model.ProductImagePath2))
                        {
                            model.ProductImagePath2 = "/ProductImages/" + model.ProductImagePath2;
                        }
                        else
                        {
                            model.ProductImagePath2 = "/images/DefaultProduct.jpg";
                        }
                        if (!string.IsNullOrEmpty(model.ProductImagePath3))
                        {
                            model.ProductImagePath3 = "/ProductImages/" + model.ProductImagePath3;
                        }
                        else
                        {
                            model.ProductImagePath3 = "/images/DefaultProduct.jpg";
                        }
                        if (!string.IsNullOrEmpty(model.ProductImagePath4))
                        {
                            model.ProductImagePath4 = "/ProductImages/" + model.ProductImagePath4;
                        }
                        else
                        {
                            model.ProductImagePath4 = "/images/DefaultProduct.jpg";
                        }
                        if (!string.IsNullOrEmpty(model.ProductImagePath5))
                        {
                            model.ProductImagePath5 = "/ProductImages/" + model.ProductImagePath5;
                        }
                        else
                        {
                            model.ProductImagePath5 = "/images/DefaultProduct.jpg";
                        }

                    }
                }
                else if (ActionName == "Delete")
                {
                    model.IsAdd = "Delete";
                }
                else
                {
                    model.IsAdd = "Add";
                }
            }
         //TansactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "ProductMaster"))
                return View(model);
            //else
            //     return RedirectToAction("Dashboard", "Home");
        }
        // POST: SaveProductMaster
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveProductMaster(ProductDetails model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                if (model.upload != null)
                {
                    var path = System.Web.HttpContext.Current.Server.MapPath("~/ProductImages");
                    
                    for (int i =0;i< model.upload.Length;i++)
                    {
                       
                        if (model.upload[i] != null && model.upload[i].FileName != null && !model.upload[i].FileName.Contains("DefaultProduct.jpg"))
                        {
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            string myfile = Guid.NewGuid() + "-" + Path.GetFileName(model.upload[i].FileName);
                            var productImage = Path.Combine(path, myfile);
                            model.upload[i].SaveAs(productImage);

                            var req = HttpContext.Request;
                            //string baseUrl = string.Format("{0}://{1}/{2}", req.Url.Scheme, req.Url.Authority, "ProductImages/" + myfile);
                            string baseUrl =  myfile;

                            if (i==0)
                            model.ProductImagePath = baseUrl;
                            else if (i == 1)
                                model.ProductImagePath1 = baseUrl;
                            else if (i == 2)
                                model.ProductImagePath2 = baseUrl;
                            else if (i == 3)
                                model.ProductImagePath3 = baseUrl;
                            else if (i == 4)
                                model.ProductImagePath4 = baseUrl;
                            else if (i == 5)
                                model.ProductImagePath5 = baseUrl;
                        }
                    }
                }
                if (Session["LoginUser"] != null)
                {
                    model.UserDetails = Session["LoginUser"] as User;


                }
                model.IsAdd = "Add";
                objResponse = objProductManager.SaveProductMaster(model);
            }
            catch (Exception ex)
            {
                objResponse.ResponseStatus = "FAILED";
                objResponse.ResponseMessage = ex.ToString();
            }

            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult ProductList()
        {

            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "ProductList"))
                return View();
            else
                 return RedirectToAction("Dashboard", "Home");
        }

        //GET:ListProduct
        [HttpPost]
        public ActionResult GetProductList()
        {
            List<ProductDetails> objProductList = new List<ProductDetails>();
            decimal LoginStateCode = (Session["LoginUser"] as User).StateCode;
            objProductList = objProductManager.GetProductList(LoginStateCode);
            var jsonProduct = Json(objProductList, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditProductMaster(ProductDetails model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                if (model.upload != null)
                {
                    var path = System.Web.HttpContext.Current.Server.MapPath("~/ProductImages");
                    int count = model.Numberofimages;


                    for (int i = 0; i < model.upload.Length; i++)
                    {                       
                        if (model.upload[i] != null && model.upload[i].FileName != null && !model.upload[i].FileName.Contains("DefaultProduct.jpg"))
                        {
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            string myfile = Guid.NewGuid() + "-" + Path.GetFileName(model.upload[i].FileName);
                            var productImage = Path.Combine(path, myfile);
                            model.upload[i].SaveAs(productImage);

                            var req = HttpContext.Request;
                            //string baseUrl = string.Format("{0}://{1}/{2}", req.Url.Scheme, req.Url.Authority, "ProductImages/" + myfile);
                            string baseUrl = myfile;
                            if (i == 0)
                                model.ProductImagePath = baseUrl;
                            else if (i == 1)
                                model.ProductImagePath1 = baseUrl;
                            else if (i == 2)
                                model.ProductImagePath2 = baseUrl;
                            else if (i == 3)
                                model.ProductImagePath3 = baseUrl;
                            else if (i == 4)
                                model.ProductImagePath4 = baseUrl;
                            else if (i == 5)
                                model.ProductImagePath5 = baseUrl;                            
                        }
                    }
                }
                if (Session["LoginUser"] != null)
                {
                    model.UserDetails = Session["LoginUser"] as User;
                }
                model.IsAdd = "Edit";
                objResponse =  objProductManager.EditProductMaster(model);
            }
            catch (Exception ex)
            {

            }

            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DeleteProductMaster(ProductDetails model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            model.IsAdd = "Delete";
            objResponse = objProductManager.DeleteProductMaster(model);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        // GET: CheckDuplicateMaster
        [HttpPost]
        public ActionResult CheckDuplicateMaster(CheckDuplicateModel model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objProductManager.IsMasterExists(model);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CheckDuplicateBarCode(string batchcode,string prodid) 
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objProductManager.CheckDuplicateBarCode( batchcode ,prodid);
            return Json(objResponse, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult CheckDuplicateBatchCode(string batchcode, string prodid)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objProductManager.CheckDuplicateBatchCode(batchcode, prodid);
            return Json(objResponse, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult GetActiveListOfSubCat(CheckDuplicateModel model)
        {
            List<SubCategoryDetails> objSubCategoryDetails = objProductManager.GetSubcategoryDetails(0, "Y");
            return Json(objSubCategoryDetails, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult BatchcodeMaster(string ActionName, string BatchcodeId)
        {

            ProductDetails model = new ProductDetails();
            BatchCode batchcodemodel = new BatchCode ();
           // List<SelectListItem> objBarcodeType = new List<SelectListItem>();
            //objBarcodeType.Add(new SelectListItem { Text = "System Generated", Value = "System Generated" });
            //objBarcodeType.Add(new SelectListItem { Text = "Other", Value = "Other" });
            //ViewBag.ListBarcodetype = objBarcodeType;
            model.productbatchcodedetails  = new BatchCode ();
            //model.ProductBarcodeDetails.BarcodeType = "System Generated";
            //model.ProductBarcodeDetails.Barcode = objProductManager.MaxBarCode().ToString();
            model.productbatchcodedetails.ExisitingBatchcode  = model.productbatchcodedetails.Batchcode  ;
            if (!string.IsNullOrEmpty(ActionName))
            {
                if (ActionName == "Edit")
                {

                    if (!string.IsNullOrEmpty(BatchcodeId ))
                    {
                        decimal Batchcodeidd = decimal.Parse(BatchcodeId);
                        decimal LoginStateCode = (Session["LoginUser"] as User).StateCode;
                        batchcodemodel  = objProductManager.GetBatchCodeDetail (Batchcodeidd );
                        model.productbatchcodedetails.BatchcodeId  = batchcodemodel.BatchcodeId ;
                        model.productbatchcodedetails.ExpDate = batchcodemodel.ExpDate;
                        model.productbatchcodedetails.MfgDate = batchcodemodel.MfgDate;
                        model.productbatchcodedetails.ProductName = batchcodemodel.ProductName;
                        model.productbatchcodedetails.ProductCode = batchcodemodel.ProductCode;
                        model.productbatchcodedetails.PurchaseRate = batchcodemodel.PurchaseRate;
                        model.productbatchcodedetails.MRP = batchcodemodel.MRP;
                        model.productbatchcodedetails.DP = batchcodemodel.DP;
                        model.productbatchcodedetails.IsExpirable = batchcodemodel.IsExpirable;
                        model.productbatchcodedetails.Batchcode  = batchcodemodel.Batchcode ;
                        model.productbatchcodedetails.Remarks = batchcodemodel.Remarks;
                        model.productbatchcodedetails.ActiveStatus = batchcodemodel.ActiveStatus;
                        model.productbatchcodedetails.ProductId = batchcodemodel.ProductId;
                        model.productbatchcodedetails.ExpDateStr = batchcodemodel.ExpDateStr;
                        model.productbatchcodedetails.MfgDateStr = batchcodemodel.MfgDateStr;
                        model.productbatchcodedetails.Bv = batchcodemodel.Bv;
                        model.productbatchcodedetails.IsAdd = batchcodemodel.IsAdd;
                        model.productbatchcodedetails.IsActive = batchcodemodel.IsActive;
                        model.IsAdd = ActionName;
                        model.PV =Convert.ToDecimal(batchcodemodel.PV);
                        //barcodemodel.BarcodeType =
                    }
                }
            }
            //  if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "BarcodeList"))
            return View(model);
            // else
            //  return RedirectToAction("Dashboard", "Home");
        }
        public ActionResult SaveBatchcode(ProductDetails Model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            List<BatchCode > batchcodeList = new List<BatchCode >();

            if (!string.IsNullOrEmpty(Model.productbatchcodedetails .objProductListStr))
            {
                var objects = JArray.Parse(Model.productbatchcodedetails.objProductListStr); // parse as array  
                foreach (JObject root in objects)
                {
                    BatchCode  objTemp = new BatchCode();
                    foreach (KeyValuePair<String, JToken> app in root)
                    {
                        //ProductGrid.push({ "MDate": MDate, "EDate": EDate, "remark": remark, "Status": Status })             
                        if (app.Key == "productCode")
                        {
                            objTemp.ProductCode = (string)app.Value;
                        }
                        if (app.Key == "productName")
                        {
                            objTemp.ProductName = (string)app.Value;
                        }
                        if (app.Key == "pRate")
                        {
                            objTemp.PurchaseRate = (decimal)app.Value;
                        }
                        if (app.Key == "MRP")
                        {
                            objTemp.MRP = (decimal)app.Value;
                        }
                        if (app.Key == "dp")
                        {
                            objTemp.DP = (decimal)app.Value;
                        }
                    
                        if (app.Key == "batchcode")
                        {
                            objTemp.Batchcode  = (string)app.Value;
                            //objTemp.Batchno = (int)app.Value;
                        }
                        if (app.Key == "Isexpirable")
                        {
                            objTemp.IsExpirable = (string)app.Value;
                        }
                        if (app.Key == "Status")
                        {
                            objTemp.IsActive = (bool)app.Value;
                        }
                        if (app.Key == "remark")
                        {
                            objTemp.Remarks = (string)app.Value;
                        }
                        if (app.Key == "MDate")
                        {
                            objTemp.MfgDateStr = (string)app.Value;
                        }
                        if (app.Key == "EDate")
                        {
                            objTemp.ExpDateStr = (string)app.Value;
                        }
                        if (app.Key=="bv")
                        {
                            objTemp.Bv=(decimal)app.Value;
                        }
                        if (app.Key == "PV")
                        {
                            objTemp.PV = (decimal)app.Value;
                        }
                    }
                    objTemp.UserId = (Session["LoginUser"] as User).UserId;
                    objTemp.UserName = (Session["LoginUser"] as User).UserName;
                    objTemp.purchasefrom  = (Session["LoginUser"] as User).PartyCode ;

                    batchcodeList.Add(objTemp);
                }
            }

            objResponse = objProductManager.SaveBatchcode(batchcodeList );
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditBatchcode(ProductDetails Model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            Model.IsAdd = "Edit";
            if (Session["LoginUser"] != null)
            {
                Model.UserDetails = Session["LoginUser"] as User;
            }
            objResponse = objProductManager.UpdateBatchcode(Model);



            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BatchcodeList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetBatchcodeList()
        {
            List<BatchCode > objList = objProductManager.GetBatchCodeList ();
            return Json(objList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActivateBatchcode(string BCode)
        {
            ResponseDetail objResponse = objProductManager.ActivateBatchcode(BCode);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeActiveBatchcode(string BCode)
        {
            ResponseDetail objResponse = objProductManager.DeActivateBatchcode(BCode);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult BarcodeMaster(string ActionName, string BarcodeId)
        {
           
            ProductDetails model = new ProductDetails();
            BarcodeDetails barcodemodel = new BarcodeDetails();
            List<SelectListItem> objBarcodeType = new List<SelectListItem>();
            objBarcodeType.Add(new SelectListItem { Text = "System Generated", Value = "System Generated" });
            objBarcodeType.Add(new SelectListItem { Text = "Other", Value = "Other" });
            ViewBag.ListBarcodetype = objBarcodeType;
            model.ProductBarcodeDetails = new BarcodeDetails();
            model.ProductBarcodeDetails.BarcodeType = "System Generated";
            model.ProductBarcodeDetails.Barcode = objProductManager.MaxBarCode().ToString();
            model.ProductBarcodeDetails.ExisitingBarcode = model.ProductBarcodeDetails.Barcode;
            if (!string.IsNullOrEmpty(ActionName))
            {
                if (ActionName == "Edit")
                {

                    if (!string.IsNullOrEmpty(BarcodeId))
                    {
                        decimal Barcodeidd = decimal.Parse(BarcodeId);
                        decimal LoginStateCode = (Session["LoginUser"] as User).StateCode;
                        barcodemodel = objProductManager.GetBarCodeDetail (Barcodeidd);
                        model.ProductBarcodeDetails.BarcodeId  = barcodemodel.BarcodeId ;
                        model.ProductBarcodeDetails.ExpDate = barcodemodel.ExpDate;
                        model.ProductBarcodeDetails.MfgDate = barcodemodel.MfgDate;
                        model.ProductBarcodeDetails.ProductName = barcodemodel.ProductName;
                        model.ProductBarcodeDetails.ProductCode  = barcodemodel.ProductCode ;
                        model.ProductBarcodeDetails.PurchaseRate = barcodemodel.PurchaseRate;
                        model.ProductBarcodeDetails.MRP = barcodemodel.MRP;
                        model.ProductBarcodeDetails.DP = barcodemodel.DP;
                        model.ProductBarcodeDetails.IsExpirable = barcodemodel.IsExpirable;
                        model.ProductBarcodeDetails.BarcodeType = barcodemodel.BarcodeType;
                        model.ProductBarcodeDetails.Barcode = barcodemodel.Barcode;
                        model.ProductBarcodeDetails.Remarks = barcodemodel.Remarks;
                        model.ProductBarcodeDetails.ActiveStatus = barcodemodel.ActiveStatus;
                        model.ProductBarcodeDetails.ProductId = barcodemodel.ProductId;
                        model.ProductBarcodeDetails.ExpDateStr = barcodemodel.ExpDateStr;
                        model.ProductBarcodeDetails.MfgDateStr = barcodemodel.MfgDateStr;
                        model.ProductBarcodeDetails.IsAdd = barcodemodel.IsAdd;
                        model.IsAdd = ActionName ;
                        //barcodemodel.BarcodeType =
                    }
                }
            }
                      //  if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "BarcodeList"))
                return View(model);
           // else
               //  return RedirectToAction("Dashboard", "Home");
        }

        public ActionResult SaveBarcode(ProductDetails Model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            List<BarcodeDetails> barcodeList = new List<BarcodeDetails>();

            if (!string.IsNullOrEmpty(Model.ProductBarcodeDetails.objProductListStr))
            {
                var objects = JArray.Parse(Model.ProductBarcodeDetails.objProductListStr); // parse as array  
                foreach (JObject root in objects)
                {
                    BarcodeDetails objTemp = new BarcodeDetails();
                    foreach (KeyValuePair<String, JToken> app in root)
                    {
                        //ProductGrid.push({ "MDate": MDate, "EDate": EDate, "remark": remark, "Status": Status })             
                        if (app.Key == "productCode")
                        {
                            objTemp.ProductCode = (string)app.Value;
                        }
                        if (app.Key == "productName")
                        {
                            objTemp.ProductName = (string)app.Value;
                        }
                        if (app.Key == "pRate")
                        {
                            objTemp.PurchaseRate = (int)app.Value;
                        }
                        if (app.Key == "MRP")
                        {
                            objTemp.MRP = (int)app.Value;
                        }
                        if (app.Key == "dp")
                        {
                            objTemp.DP = (int)app.Value;
                        }
                        if (app.Key == "barcodetype")
                        {
                            objTemp.BarcodeType = (string)app.Value;
                        }
                        if (app.Key == "barcode")
                        {
                            objTemp.Barcode = (string)app.Value;
                        }
                        if (app.Key == "Isexpirable")
                        {
                            objTemp.IsExpirable = (string)app.Value;
                        }
                        if (app.Key == "Status")
                        {
                            objTemp.IsActive = (bool)app.Value;
                        }
                        if (app.Key == "remark")
                        {
                            objTemp.Remarks = (string)app.Value;
                        }
                        if (app.Key == "MDate")
                        {
                            objTemp.MfgDateStr = (string)app.Value;
                        }
                        if (app.Key == "EDate")
                        {
                            objTemp.ExpDateStr = (string)app.Value;
                        }
                    }
                    objTemp.UserId = (Session["LoginUser"] as User).UserId;
                    objTemp.UserName= (Session["LoginUser"] as User).UserName;
                    barcodeList.Add(objTemp);
                }                
            }
           
            objResponse = objProductManager.SaveBarcode(barcodeList);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditBarcode(ProductDetails  Model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            Model.IsAdd = "Edit";
            if (Session["LoginUser"] != null)
            {
                Model.UserDetails = Session["LoginUser"] as User;
            }
            objResponse = objProductManager.UpdateBarcode(Model);



            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BarcodeList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetBarcodeList()
        {
            List<BarcodeDetails> objList = objProductManager.GetBarcodeList();
            return Json(objList, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult ActivateBarcode(string BCode)
        {
            ResponseDetail objResponse = objProductManager.ActivateBarcode(BCode);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeActiveBarcode(string BCode)
        {
            ResponseDetail objResponse = objProductManager.DeActivateBarcode(BCode);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }
        [SessionExpire]
        public ActionResult GSTMasterList()
        {
            return View();
        }
        [SessionExpire]
        public ActionResult ADDEditGST()
        { 
            return View();
        }

        [HttpPost]
        public ActionResult GetGSTList()
        {
            List<GSTMasterLIST> objList = objProductManager.GetGSTList();
            return Json(objList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveGST(GSTMasterLIST Model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            List<GSTMasterLIST> GSTList = new List<GSTMasterLIST>();
            GSTMasterLIST objTemp = new GSTMasterLIST();
            if (!string.IsNullOrEmpty(Convert.ToString(Model.objGSTStr)))
            {
                var objects = JArray.Parse(Model.objGSTStr); // parse as array  
                foreach (JObject root in objects)
                {
                   
                    foreach (KeyValuePair<String, JToken> app in root)
                    {           
                        if (app.Key == "GSTTax")
                        {
                            objTemp.GSTTax = (decimal)app.Value;
                        }
                    }
                }
            }
            objResponse = objProductManager.SaveGST(objTemp);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }
    }
}