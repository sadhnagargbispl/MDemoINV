using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    public class Wallet
    {
        public string FCode { get; set; }
        public Decimal Amount { get; set; }
        public string DrCr { get; set; }
        public string Narration { get; set; }
        public string ACType { get; set; }
    }
    public class Courier
    {
        public decimal ID { get; set; }
        public string Name { get; set; }
        public decimal Weight { get; set; }
        public decimal Amount { get; set; }
        public string Website { get; set; }
        public string Remark { get; set; }
        public string ActiveStatus { get; set; }
        public int UserId { get; set; }
  
    }

    public class CourierDetail
    {
        public string Name { get; set; }
        public string Remark { get; set; }
        public int USerId { get; set; }
    }
    public class ConnModelNew

    {
        public string CompID { get; set; }
        public string InvConnStr { get; set; }
        public string InvDb { get; set; }
        public string EnttConnStr { get; set; }
        public string AppConnStr { get; set; }
        public string Db { get; set; }
        public string WRPartyCode { get; set; }
        public string CompName { get; set; }
        public string BVCaption { get; set; }
        public string CVCaption { get; set; }
        public string PVCaption { get; set; }
        public string RPCaption { get; set; }
        public string FirstActivationBill { get; set; }
        public string FirstBillOnMRP { get; set; }
        public string FirstBillonBV { get; set; }
        public string FirstBillonAmt { get; set; }
        public string FirstBillonPV { get; set; }
        public decimal FirstBillMinAmt { get; set; }
        public decimal FirstBillMinBV { get; set; }
        public decimal FirstBillMinPV { get; set; }
        public string FirstIDUpgrade { get; set; }
        //public string CanUpgrade { get; set; }

        public string ShowInvType { get; set; }
        public string ShowOffers { get; set; }
        public string WalletType { get; set; }
        public string LogoPath { get; set; }

        public string ShowRepurchBV { get; set; }
        public string ShowActiveBV { get; set; }
        public string ShowRepurchPV { get; set; }
        public string ShowActivePV { get; set; }

        public string Token { get; set; }

        /*public string FirstActivationBill { get; set; }*/

    }
    public partial class VisionOffer
    {
        public int AID { get; set; }
        public System.DateTime OfferFromDt { get; set; }
        public System.DateTime OfferToDt { get; set; }
        public string OfferDatePart { get; set; }
        public decimal OfferOnValue { get; set; }
        public string OfferExceptSubCat { get; set; }
        public string FreeProdIDs { get; set; }
        public decimal FreeProdQty { get; set; }
        public decimal OfferOnBV { get; set; }
        public string ConfFreeProdIDs { get; set; }
        public string ConfFreeProdQtys { get; set; }
        public string OfferBillType { get; set; }
        public Nullable<System.DateTime> IdDate { get; set; }
        public string IdStaus { get; set; }
        public string SortFirstBy { get; set; }
        public string ActiveStatus { get; set; }
        public Nullable<decimal> ContinueForMonth { get; set; }
        public string OfferType { get; set; }
        public string ForNewIds { get; set; }
        public string FreeProdQtys { get; set; }
        public Nullable<int> IdDays { get; set; }
        public int OfferId { get; set; }
        public Nullable<int> OfferStartDay { get; set; }
        public Nullable<int> OfferEndDay { get; set; }
        public string OfferName { get; set; }
        public string IsPVApplicable { get; set; }
        public Nullable<decimal> ExtraPV { get; set; }
        public string Remarks { get; set; }
        public string CombineWithOffer { get; set; }
        public string CheckFirstBillWith { get; set; }
        public Nullable<bool> CombineOffer { get; set; }
        public Nullable<decimal> CBAmount { get; set; }
        public Nullable<int> OfferFrequency { get; set; }
    }
}
