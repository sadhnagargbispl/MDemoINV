using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class Offer
    {
        public decimal OfferID { get; set; }
        public DateTime OfferFromDt { get; set; }
        public DateTime OfferToDt { get; set; }
        public string OfferDatePart { get; set; }
        public decimal OfferOnValue { get; set; }
        public decimal OfferOnBV { get; set; }
        public decimal OfferOnToBV { get; set; }
        public decimal TotalQty { get; set; }
        public decimal CreatedBy { get; set; }
        public string ActiveStatus { get; set; }
        public DateTime RecTimeStamp { get; set; }
        public string ForNewIds { get; set; }
        public DateTime? IdDate { get; set; }
        public string IdStatus { get; set; }
        public bool ForAllProducts { get; set; }        
        public string PrductString { get; set; }
        public string BuyPrductString { get; set; }        
        public string ForBillType { get; set; }
        public string Action { get; set; }
        public string OfferName { get; set; }
        public int StartProduct { get; set; }
        public decimal offerType { get; set; }
        public List<OfferProducts>OfferProds{ get; set; }
        public List<PartyModel> OfferParty { get; set; }
        public string Party { get; set; }
        public string IsFixedQty { get; set; }
        public decimal  FixedQty { get; set; }
    }

    public class OfferReport
    {
        public string OfferName { get; set; }
        public string OfferType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public decimal OfferUID { get; set; }
        public decimal? TotalSale { get; set; }
        public decimal? Qty { get; set; }
        public decimal? FreeQty { get; set; }
        public decimal? TotalQty { get; set; }
        public decimal? FreeProdValue { get; set; }
        public decimal? AmtCollected { get; set; }
        public decimal? OfferValue { get; set; }
        public string SoldBy { get; set; }
        public string BillNo { get; set; }
        public string UserBillNo { get; set; }
        public string BillDate { get; set; }
        public string BillDateStr { get; set; }
        public string FCode { get; set; }
        public string PartyName { get; set; }
        public string ProdID { get; set; }
        public string ProductName { get; set; }
        public string UserProdID { get; set; }
        public string OfferRange { get; set; }
    }
}
