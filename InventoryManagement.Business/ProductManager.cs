using InventoryManagement.Business.Contract;
using InventoryManagement.DataAccess;
using InventoryManagement.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Business
{
    public class ProductManager:IProductManager
    {
        ProductRepository objProductRepository = new ProductRepository();
        
        public ResponseDetail AddCategoryDetails(CategoryDetails model)
        {
            ResponseDetail objResponse = objProductRepository.AddCategoryDetails(model);
            return objResponse;
        }
        public ResponseDetail IsMasterExists(CheckDuplicateModel model)
        {
            ResponseDetail objResponse = objProductRepository.IsMasterExists(model);
            return objResponse;
        }
        public ResponseDetail AddSubCategoryDetails(SubCategoryDetails model)
        {
            ResponseDetail objResponse = objProductRepository.AddSubCategoryDetails(model);
            return objResponse;
        }
        public List<CategoryDetails> GetCategoryList(string ActiveFlag)
        {
            List<CategoryDetails> objCategoryList = new List<CategoryDetails>();
            objCategoryList = objProductRepository.GetCategoryList(ActiveFlag);
            return objCategoryList;
        }
        public List<SubCategoryDetails> GetSubcategoryDetails(int CategoryId, string ActiveStatus)
        {
            List<SubCategoryDetails> objSubCategoryList = new List<SubCategoryDetails>();
            objSubCategoryList = objProductRepository.GetSubcategoryDetails(CategoryId, ActiveStatus);
            return objSubCategoryList;
        }
        public ResponseDetail SaveProductMaster(ProductDetails model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objProductRepository.SaveProductMaster(model);
            return objResponse;
        }
        public int MaxBarCode()
        {
            int MaxCode = objProductRepository.MaxBarCode();
            return MaxCode;
        }
        public int MaxProductCode()
        {
            int MaxCode = objProductRepository.MaxProductCode();
            return MaxCode;
        }
        public List<ProductDetails> GetProductList(decimal LoginStateCode)
        {
            List<ProductDetails> objproductList = objProductRepository.GetProductList(LoginStateCode);
            return objproductList;
        }
        public ResponseDetail EditProductMaster(ProductDetails model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objProductRepository.EditProductMaster(model);
            return objResponse;
        }
        public ResponseDetail DeleteProductMaster(ProductDetails model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objProductRepository.DeleteProductMaster(model);
            return objResponse;
        }
        public ProductDetails GetProductDetail(decimal ProductId, decimal LoginStateCode)
        {
            ProductDetails objproduct = objProductRepository.GetProductDetail(ProductId,LoginStateCode);
            return objproduct;
        }
        public ResponseDetail SaveBarcode(List<BarcodeDetails> barcodeList)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objProductRepository.SaveBarcode(barcodeList);
            return objResponse;
        }

        public List<BarcodeDetails> GetBarcodeList()
        {
            return objProductRepository.GetBarcodeList();
        }
        public BarcodeDetails GetBarCodeDetail(decimal Bcode)
        {
            return objProductRepository.GetBarCodeDetail(Bcode);
        }
        public ResponseDetail UpdateBarcode(ProductDetails objbarcode)
        {
            return objProductRepository.UpdateBarcode(objbarcode);
        }
        public ResponseDetail ActivateBarcode(string objbarcode)
        {
            return objProductRepository.ActivateBarcode(objbarcode);
        }
        public ResponseDetail DeActivateBarcode(string objbarcode)
        {
            return objProductRepository.DeActivateBarcode(objbarcode);
        }

        public ResponseDetail SaveBatchcode(List<BatchCode > barcodeList)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objProductRepository.SaveBatchcode (barcodeList);
            return objResponse;
        }


        public List<BatchCode> GetBatchCodeList()
        {
            return objProductRepository.GetBatchcodeList();
        }
        public BatchCode GetBatchCodeDetail(decimal Bcode)
        {
            return objProductRepository.GetBatchCodeDetail(Bcode);
        }
        public ResponseDetail UpdateBatchcode(ProductDetails objbatchcode)
        {
            return objProductRepository.UpdateBatchcode(objbatchcode);
        }
        public ResponseDetail ActivateBatchcode(string objbatchcode)
        {
            return objProductRepository.ActivateBatchcode(objbatchcode);
        }
        public ResponseDetail DeActivateBatchcode(string objbatchcode)
        {
            return objProductRepository.DeActivateBatchcode(objbatchcode);
        }
        public ResponseDetail  CheckDuplicateBarCode(string batchcode, string prodid) 
        {
            return objProductRepository.CheckBarcodeCode(batchcode, prodid);
        } 
        public ResponseDetail CheckDuplicateBatchCode(string batchcode, string prodid)
        {
            return objProductRepository.CheckDuplicateBatchCode(batchcode, prodid); 
        }
        public List<GSTMasterLIST> GetGSTList()
        {
            return objProductRepository.GetGSTList();
        }
        public ResponseDetail SaveGST(GSTMasterLIST GStList) 
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objProductRepository.SaveGST(GStList);
            return objResponse;
        }
    }
}