using InventoryManagement.API.Models;
using InventoryManagement.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Business.Contract
{
    public interface ITransactionManager
    {
        List<string> GetAutocompleteProductNames(string InvType);
        List<string> GetAutocompProductsOnly();
        List<ProductModel> GetproductInfo(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode,bool IsBillOnMrp, string OfferID,bool allhalf, string Invoice, string IsSpclOffer);
        CustomerDetail GetCustInfo(string IdNo);
        ResponseDetail SaveDistributorBill(DistributorBillModel objModel);
        Task<ResponseDetail> SendOTP(string MobileNo, string TotalBillAmount, string idNo);
        DistributorBillModel getInvoice(string BillNo, string CurrentPartyCode, string id);
        List<PartyModel> GetAllParty(string LoginPartyCode, decimal LoginStateCode);
        List<GroupModel> GetGroupList();
        List<PartyModel> GetPartyList();
        List<PartyModel> GetAllSupplierList(string LoginPartyCode, decimal LoginStateCode);
        ResponseDetail SaveStockJv(StockJv objModel);
        ResponseDetail SavePurchaseInvoice(DistributorBillModel objModel);
        List<PurchaseReport> GetPurchaseInvoice(string InvoiceNo);
        ResponseDetail SavePartyOrderDetails(PartyOrderModel objPartyOrderModel);
        decimal GetPartyWalletBalance(string LoginPartyCode,string vtype);
        string GetOrderNo(string LoginPartyCode);
        List<ProductModel> GetOrderProductList(string OrderNo, string OrderBy);
        List<PartyOrderModel> GetOrderList(string OrderBy, string OrderTo, string Status);
        ResponseDetail SaveDispatchOrder(PartyOrderModel objPartyDispatchOrder);
        List<DisptachOrderModel> GetDispatchOrderList(string FromDate, string ToDate, string PartyCode, string ViewType, string IdNo, string OrderNo, string DispMode);
        ResponseDetail RejectOrder(string OrderNo, string RejectReason, decimal RejectedByUserId);
        List<ProductModel> GetOrderProduct(string OrderNo, string CurrentPartyCode);
        ResponseDetail SaveDispatchOrderdetails(List<DisptachOrderModel> objModel);
        ResponseDetail DeleteBills(string BillNo, string FsessId, decimal UserId, string Reason);
        List<SlabModel> GetSlab();
        ResponseDetail SaveSlab(RewardPoint objModel);
        RewardPoint GetSlabPoint(int ID);
        List<RewardPoint> GetAllSlab();
    }
}
