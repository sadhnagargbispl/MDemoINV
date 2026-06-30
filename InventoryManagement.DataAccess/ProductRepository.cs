using InventoryManagement.API.Controllers;
using InventoryManagement.DataAccess.Contract;
using InventoryManagement.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.DataAccess
{
    public class ProductRepository:IProductRepository
    {
        ProductAPIController objProductAPI = new ProductAPIController();
     
        public ResponseDetail AddCategoryDetails(CategoryDetails model)
        {
            ResponseDetail objResponse = objProductAPI.AddCategoryDetails(model);
            return objResponse;
        }
        public ResponseDetail IsMasterExists(CheckDuplicateModel model)
        {
            ResponseDetail objResponse = objProductAPI.IsMasterExists(model);
            return objResponse;
        }
        public ResponseDetail AddSubCategoryDetails(SubCategoryDetails model)
        {
            ResponseDetail objResponse = objProductAPI.AddSubCategoryDetails(model);
            return objResponse;
        }
        public List<CategoryDetails> GetCategoryList(string ActiveFlag)
        {
            List<CategoryDetails> objCategoryList = new List<CategoryDetails>();
            objCategoryList = objProductAPI.GetCategoryList(ActiveFlag);
            return objCategoryList;
        }
        public List<SubCategoryDetails> GetSubcategoryDetails(int CategoryId, string ActiveStatus)
        {
            List<SubCategoryDetails> objSubCategoryList = new List<SubCategoryDetails>();
            objSubCategoryList = objProductAPI.GetSubcategoryDetails(CategoryId, ActiveStatus);
            return objSubCategoryList;
        }
        public ResponseDetail SaveProductMaster(ProductDetails model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objProductAPI.SaveProductMaster(model);
            return objResponse;
        }
        public int MaxBarCode()
        {
            int MaxCode = objProductAPI.MaxBarCode();
            return MaxCode;
        }
        public int MaxProductCode()
        {
            int MaxCode = objProductAPI.MaxProductCode();
            return MaxCode;
        }
        public List<ProductDetails> GetProductList(decimal LoginStateCode)
        {
            List<ProductDetails> objproductList = objProductAPI.GetProductList(LoginStateCode);
            return objproductList;
        }
        public ResponseDetail EditProductMaster(ProductDetails model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objProductAPI.EditProductMaster(model);
            return objResponse;
        }
        public ResponseDetail DeleteProductMaster(ProductDetails model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objProductAPI.DeleteProductMaster(model);
            return objResponse;
        }
        public ProductDetails GetProductDetail(decimal ProductId, decimal LoginStateCode)
        {
            ProductDetails objproduct = objProductAPI.GetProductDetail(ProductId,LoginStateCode);
            return objproduct;
        }
        public ResponseDetail SaveBarcode(List<BarcodeDetails> barcodeList)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objProductAPI.SaveBarcode(barcodeList);
            return objResponse;
        }
        public List<BarcodeDetails> GetBarcodeList()
        {
            return objProductAPI.GetBarcodeList();
        }
        public BarcodeDetails GetBarCodeDetail(decimal Bcode)
        {
            return objProductAPI.getBarCodeDetail(Bcode);
        }
        public ResponseDetail UpdateBarcode(ProductDetails  objbarcode)
        {
            return objProductAPI.UpdateBarcode(objbarcode);
        }
        public ResponseDetail ActivateBarcode(string objbarcode)
        {
            return objProductAPI.ActivateBarcode(objbarcode);
        }
        public ResponseDetail DeActivateBarcode(string objbarcode)
        {
            return objProductAPI.DeActivateBarcode(objbarcode);
        }
        public ResponseDetail SaveBatchcode(List<BatchCode> barcodeList)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objProductAPI.SaveBatchcode(barcodeList);
            return objResponse;
        }
        public List<BatchCode > GetBatchcodeList()
        {
            return objProductAPI.GetBatchcodeList();
        }
        public BatchCode GetBatchCodeDetail(decimal Bcode)
        {
            return objProductAPI.getBatchCodeDetail(Bcode);
        }
        public ResponseDetail UpdateBatchcode(ProductDetails objbatchcode)
        {
            return objProductAPI.UpdateBatchcode(objbatchcode);
        }
        public ResponseDetail ActivateBatchcode(string objbatchcode)
        {
            return objProductAPI.ActivateBatchcode(objbatchcode);
        }
        public ResponseDetail DeActivateBatchcode(string objbatchcode)
        {
            return objProductAPI.DeActivateBatchcode(objbatchcode);
        }
        public ResponseDetail CheckBarcodeCode(string batchcode, string prodid) 
        {
            ResponseDetail objResponse = objProductAPI.CheckBarcodeCode(batchcode ,prodid );
            return objResponse;
        }
        public ResponseDetail CheckDuplicateBatchCode(string batchcode, string prodid)
        {
            ResponseDetail objResponse = objProductAPI.CheckDuplicateBatchCode(batchcode, prodid); 
            return objResponse;
        }
        public List<GSTMasterLIST> GetGSTList()
        {
            return objProductAPI.GetGSTList();
        }
        public ResponseDetail SaveGST(GSTMasterLIST GStList) 
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objProductAPI.SaveGST(GStList);
            return objResponse;
        }
    }
}