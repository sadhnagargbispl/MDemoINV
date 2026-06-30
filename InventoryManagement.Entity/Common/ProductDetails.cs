using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class ProductDetails
    {
        public TaxDetails ProductTaxDetails { get; set; }
        public BarcodeDetails ProductBarcodeDetails { get; set; }
        public BatchCode productbatchcodedetails { get; set; }
        public CategoryDetails ProductCategoryDetails { get; set; }
        public SubCategoryDetails ProductSubCategoryDetails { get; set; }
        public CurrentStockModel ProductCurrentStockDetails { get; set; }
        public  HttpPostedFileBase[] upload { get; set; }
        public int ProductId { get; set; }
        public int ProductCode { get; set; }
        public string MinQtyStr { get; set; }
        public string ProductCodeStr { get; set; }
        public string UserDefinedCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int CategoryId { get; set; }
        public int SubCatgeoryId { get; set; }
        public bool IsActive { get; set; }
        public decimal BV { get; set; }
        public decimal CV { get; set; }
        public decimal PV { get; set; }
        public decimal RP { get; set; }
        public decimal DiscountPer { get; set; }
        public decimal DiscountInRs { get; set; }
        public decimal ProductCommission { get; set; }
        public decimal Weight { get; set; }        
        public string Message { get; set; }
        public string MessageStatus { get; set; }
        public string OnWebsite { get; set; }
        public string SpecialOffer { get; set; }
        public string HotSell { get; set; }        
        public string ProductImagePath { get; set; }
        public string ProductImagePath1 { get; set; }
        public string ProductImagePath2 { get; set; }
        public string ProductImagePath3 { get; set; }
        public string ProductImagePath4 { get; set; }
        public string ProductImagePath5 { get; set; }
        public int Numberofimages { get; set; }
        public string IsAdd { get; set; }
        public decimal MinQty { get; set; }
        public string HSNCode { get; set; }
        public User UserDetails { get; set; }
        public string Size { get; set; }
        public decimal? Sequence { get; set; }
        public bool IsBillingAllowed { get; set; }
        public bool IsAvailableforOffers { get; set; }
        public bool AllowedForMRI { get; set; }
        public bool AllowedForFPV { get; set; }
        public bool AllowedForGV { get; set; }
        public decimal SJPDiscount { get; set; }

        public bool AllowedForGPV { get; set; }
        public string ProductLink { get; set; }
    }
}