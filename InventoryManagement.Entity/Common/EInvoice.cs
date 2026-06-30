using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace InventoryManagement.Entity.Common
{
    //public class EInvoice
    //{
    //    public RespPlGenIRN ResppigenIRN { get; set; }
    //    public RespGenIRNInvData RespgenIRNinvdata { get; set; }
    //    public RespGenIRNQrCodeData RespgenIRNQrCode { get; set; }
    //}

  
        public class Einvoice
        {
            public string Version { get; set; }
            public Trandtls TranDtls { get; set; }
            public DocDtls DocDtls { get; set; }
            public Sellerdtls SellerDtls { get; set; }
            public Buyerdtls BuyerDtls { get; set; }
            public Dispdtls DispDtls { get; set; }
            public Shipdtls ShipDtls { get; set; }
            public List<ItemList> ItemList { get; set; } = new List<ItemList>();
            public Valdtls ValDtls { get; set; }
        public Paydtls PayDtls { get; set; }
        public Refdtls RefDtls { get; set; }
    }

        public class Trandtls
        {
            public string TaxSch { get; set; }
            public string SupTyp { get; set; }
            public string RegRev { get; set; }
            public object EcmGstin { get; set; }
            public string IgstOnIntra { get; set; }
        }

        public class DocDtls
        {
            public string Typ { get; set; }
            public string No { get; set; }
            public string Dt { get; set; }
        }

        public class Sellerdtls
        {
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string TrdNm { get; set; }
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Loc { get; set; }
            public int Pin { get; set; }
            public string Stcd { get; set; }
            public string Ph { get; set; }
            public string Em { get; set; }
        }

        public class Buyerdtls
        {
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string TrdNm { get; set; }
            public string Pos { get; set; }
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Loc { get; set; }
            public int Pin { get; set; }
            public string Stcd { get; set; }
            public string Ph { get; set; }
            public string Em { get; set; }
        }

        public class Dispdtls
        {
            public string Nm { get; set; }
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Loc { get; set; }
            public int Pin { get; set; }
            public string Stcd { get; set; }
        }

        public class Shipdtls
        {
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string TrdNm { get; set; }
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Loc { get; set; }
            public int Pin { get; set; }
            public string Stcd { get; set; }
        }

        public class Valdtls
        {
            public double AssVal { get; set; }
            public double CgstVal { get; set; }
            public double SgstVal { get; set; }
            public double IgstVal { get; set; }
            public double CesVal { get; set; }
            public double StCesVal { get; set; }
            public double Discount { get; set; }
            public double OthChrg { get; set; }
            public double RndOffAmt { get; set; }
            public double TotInvVal { get; set; }
            public double TotInvValFc { get; set; }
        
    }

        public class Paydtls
        {
            public string Nm { get; set; }
            public string AccDet { get; set; }
            public string Mode { get; set; }
            public string FinInsBr { get; set; }
            public string PayTerm { get; set; }
            public string PayInstr { get; set; }
            public string CrTrn { get; set; }
            public string DirDr { get; set; }
            public int CrDay { get; set; }
            public int PaidAmt { get; set; }
            public int PaymtDue { get; set; }
        }

        public class Refdtls
        {
            public string InvRm { get; set; }
            public Docperddtls DocPerdDtls { get; set; }
            public Precdocdtl PrecDocDtls { get; set; }
            public Contrdtl ContrDtls { get; set; }
        }

        public class Docperddtls
        {
            public string InvStDt { get; set; }
            public string InvEndDt { get; set; }
        }

        public class Precdocdtl
        {
            public string InvNo { get; set; }
            public string InvDt { get; set; }
            public string OthRefNo { get; set; }
        }

        public class Contrdtl
        {
            public string RecAdvRefr { get; set; }
            public string RecAdvDt { get; set; }
            public string TendRefr { get; set; }
            public string ContrRefr { get; set; }
            public string ExtRefr { get; set; }
            public string ProjRefr { get; set; }
            public string PORefr { get; set; }
            public string PORefDt { get; set; }
        }

        public class Expdtls
        {
            public string ShipBNo { get; set; }
            public string ShipBDt { get; set; }
            public string Port { get; set; }
            public string RefClm { get; set; }
            public string ForCur { get; set; }
            public string CntCode { get; set; }
            public object ExpDuty { get; set; }
        }

        public class Ewbdtls
        {
            public string TransId { get; set; }
            public string TransName { get; set; }
            public int Distance { get; set; }
            public string TransDocNo { get; set; }
            public string TransDocDt { get; set; }
            public string VehNo { get; set; }
            public string VehType { get; set; }
            public string TransMode { get; set; }
        }

        public class ItemList
        {
            public string SlNo { get; set; }
            public string PrdDesc { get; set; }
            public string IsServc { get; set; }
            public string HsnCd { get; set; }
            public string Barcde { get; set; }
            public int Qty { get; set; }
            public int FreeQty { get; set; }
            public string Unit { get; set; }
            public double UnitPrice { get; set; }
            public double TotAmt { get; set; }
            public double Discount { get; set; }
            public double PreTaxVal { get; set; }
            public double AssAmt { get; set; }
            public double GstRt { get; set; }
            public double IgstAmt { get; set; }
            public double CgstAmt { get; set; }
            public double SgstAmt { get; set; }
            public double CesRt { get; set; }
            public double CesAmt { get; set; }
            public double CesNonAdvlAmt { get; set; }
            public double StateCesRt { get; set; }
            public double StateCesAmt { get; set; }
            public double StateCesNonAdvlAmt { get; set; }
            public double OthChrg { get; set; }
            public double TotItemVal { get; set; }
            public string OrdLineRef { get; set; }
            public string OrgCntry { get; set; }
            public string PrdSlNo { get; set; }
        public Bchdtls BchDtls { get; set; }
    }

        public class Bchdtls
        {
            public string Nm { get; set; }
            public string ExpDt { get; set; }
            public string WrDt { get; set; }
        }

        public class Attribdtl
        {
            public string Nm { get; set; }
            public string Val { get; set; }
        }

        public class Addldocdtl
        {
            public string Url { get; set; }
            public string Docs { get; set; }
            public string Info { get; set; }
        }

        public class JsonReadVal
        {
            public string Status { get; set; }
            public Data Data { get; set; }
            public string ErrorDetails { get; set; }
            public string InfoDtls { get; set; }
        }

        public class Data
        {
            public string ClientId { get; set; }
            public string UserName { get; set; }
            public string AuthToken { get; set; }
            public string Sek { get; set; }
            public DateTime TokenExpiry { get; set; }
        }

        public class EINVOICEJSON1
        {
            public string Status { get; set; }
            public DATA1 Data { get; set; }
            public string ErrorDetails { get; set; }
            public string InfoDtls { get; set; }
        }

        public class DATA1
        {
            public string AckNo { get; set; }
            public string AckDt { get; set; }
            public string Irn { get; set; }
            public string SignedInvoice { get; set; }
            public string SignedQRCode { get; set; }
            public string Status { get; set; }
            public string EwbNo { get; set; }
            public string EwbDt { get; set; }
            public string EwbValidTill { get; set; }
            public ExtractedSignedInvoiceData ExtractedSignedInvoiceData { get; set; }
            public ExtractedSignedQrCode ExtractedSignedQrCode { get; set; }
            public string QrCodeImage { get; set; }
            public string JwtIssuer { get; set; }
        }

        public class ExtractedSignedInvoiceData
        {
            public string AckNo { get; set; }
            public string AckDt { get; set; }
            public string Version { get; set; }
            public string Irn { get; set; }
            // Public Property SignedQRCode As String
            // Public Property Status As String
            // Public Property EwbNo As String
            // Public Property EwbDt As Byte
            // Public Property EwbValidTill As String
            public Trandtls TranDtls { get; set; }
            public DocDtls DocDtls { get; set; }
            public Sellerdtls SellerDtls { get; set; }
            public Buyerdtls BuyerDtls { get; set; }
            public Dispdtls DispDtls { get; set; }
            public Shipdtls ShipDtls { get; set; }
            public Valdtls ValDtls { get; set; }
            public Paydtls PayDtls { get; set; }
            public Expdtls ExpDtls { get; set; }
            public List<Refdtls> RefDtls { get; set; } = new List<Refdtls>();
            public List<ItemList> ItemList { get; set; } = new List<ItemList>();
        }

        public class ExtractedSignedQrCode
        {
            public string SellerGstin { get; set; }
            public string BuyerGstin { get; set; }
            public string DocNo { get; set; }
            public string DocTyp { get; set; }
            public string DocDt { get; set; }
            public string TotInvVal { get; set; }
            public string ItemCnt { get; set; }
            public byte MainHsnCode { get; set; }
            public string Irn { get; set; }
        }

        public class ErrorResponse
        {
            public string ErrorInfo { get; set; }
            public string CoulumnName { get; set; }
            public string ColumnValue { get; set; }
        }
    public class IRNClass
    {
        public string status_cd { get; set; }
        public ErrorResp error { get; set; }
        public object Data { get; set; }
        public string Status { get; set; }
        public Errordetail[] ErrorDetails { get; set; }
        public Infodtl[] InfoDtls { get; set; }

        public class Errordetail
        {
            public string ErrorCode { get; set; }
            public string ErrorMessage { get; set; }
        }

        public class Infodtl
        {
            public string InfCd { get; set; }
            public object Desc { get; set; }
        }

        public class Desc
        {
            public long AckNo { get; set; }
            public string AckDt { get; set; }
            public string Irn { get; set; }
        }
    }

    public class ErrorResp
    {
        public string error_cd { get; set; }
        public string message { get; set; }
    }

    public class IRNData
    {
        public string AckNo { get; set; }
        public string AckDt { get; set; }
        public string Irn { get; set; }
        public string SignedInvoice { get; set; }
        public string SignedQRCode { get; set; }
        public string QrCodeImage { get; set; }
        public string JwtIssuer { get; set; }
    }

    public class RespInfoPl
    {
        public long ewayBillNo { get; set; }
        public string ewayBillDate { get; set; }
        public string validUpto { get; set; }
        public string alert { get; set; }
    }



}
