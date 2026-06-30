using InventoryManagement.API.Controllers;
using InventoryManagement.API.Models;
using InventoryManagement.DataAccess.Contract;
using InventoryManagement.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InventoryManagement.DataAccess
{
    public class TransactionRepository 
    {
        TransactionAPIController objTransacAPI = new TransactionAPIController();
        public List<string> GetAutocompleteProductNames(string InvType)
        {
            return (objTransacAPI.GetAutocompleteProductNames( InvType));
        }
        public List<string> GetAvailStockProductNamesOnly(string StockforParty)
        {
            return (objTransacAPI.GetAvailStockProductNamesOnly(StockforParty));
        }
        public List<string> GetAllBarcode()
        {
            return (objTransacAPI.GetAllBarcode());
        }
        public List<string> GetAutocompProductsOnly(string  StockforParty)
        {
            return (objTransacAPI.GetAutocompProductsOnly(StockforParty));
        }
      

        public List<OfferProducts> GetOfferProductNamesOnly(decimal OfferId, int type)
        {
            return (objTransacAPI.GetOfferProductList(OfferId,type));
        }

        public List<OfferProducts> GetOfferBuyProducts(decimal OfferId)
        {
            return (objTransacAPI.GetOfferBuyProducts(OfferId));
        }
        public List<ProductModel> GetproductInfo(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID,bool allhalf,string Invoice,string IsSpclOffer)
        {
            return (objTransacAPI.GetproductInfo(SearchType, data, isCForm, BillType, CurrentStateCode, CurrentPartyCode, IsBillOnMrp, OfferID, allhalf, Invoice, IsSpclOffer));
        }
        public List<ProductModel> GetproductInfoBatchWise(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool allhalf, string Invoice, string IsSpclOffer)
        {
            return (objTransacAPI.GetproductInfoBatchWise(SearchType, data, isCForm, BillType, CurrentStateCode, CurrentPartyCode, IsBillOnMrp, OfferID, allhalf, Invoice, IsSpclOffer));
        }
        public List<ProductModel> GetAllproductInfoBatchWise(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool allhalf, string Invoice, string IsSpclOffer)
        {
            return (objTransacAPI.GetAllproductInfoBatchWise(SearchType, data, isCForm, BillType, CurrentStateCode, CurrentPartyCode, IsBillOnMrp, OfferID, allhalf, Invoice, IsSpclOffer));
        }
        public List<ProductModel> PendingGetProductInfo(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool allhalf, string Invoice, string IsSpclOffer)
        {
            return (objTransacAPI.PendingGetProductInfo(SearchType, data, isCForm, BillType, CurrentStateCode, CurrentPartyCode, IsBillOnMrp, OfferID, allhalf, Invoice, IsSpclOffer));
        }
        public List<ProductModel> GetAddStockGVproductInfo(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool allhalf, string Invoice, string IsSpclOffer)
        {
            return (objTransacAPI.GetAddStockGVproductInfo(SearchType, data, isCForm, BillType, CurrentStateCode, CurrentPartyCode, IsBillOnMrp, OfferID, allhalf, Invoice, IsSpclOffer));
        }
        public List<ProductModel> GetproductBatchInfo(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool allhalf, string Invoice, string IsSpclOffer)
        {
            return (objTransacAPI.GetproductBatchInfo(SearchType, data, isCForm, BillType, CurrentStateCode, CurrentPartyCode, IsBillOnMrp, OfferID, allhalf, Invoice, IsSpclOffer));
        }
        public CustomerDetail GetCustInfo(string IdNo)
        {
            return (objTransacAPI.GetCustInfo(IdNo));
        }
        public CustomerDetail GetCustPasswordValidate(string IdNo, string Password)
        {
            return (objTransacAPI.GetCustPasswordValidate(IdNo, Password));
        }
        public MemberAPIRoot ValidateCustomerbyAPI(string IdNo, string Password)
        {
            return (objTransacAPI.ValidateCustomerbyAPI(IdNo, Password));
        }
        public Coupons CheckCoupon(string Code, string Idno)
        {
            return (objTransacAPI.CheckCoupon(Code, Idno));
        }
        public FPVVoucher CheckFpVoucher(string Code, string Idno)
        {
            return (objTransacAPI.CheckFpVoucher(Code, Idno));
        }
        public async Task<ResponseDetail> SaveDistributorBill(DistributorBillModel objModel)
        {
            return await objTransacAPI.SaveDistributorBill(objModel); ;
        }
        public List<BankModel> GetBankList()
        {
            List<BankModel> objListBank = objTransacAPI.GetBankList();
            return (objListBank);
        }
        public Task<ResponseDetail> SendOTP(string MobileNo, string TotalBillAmount, string idNo)
        {
            Task<ResponseDetail> objResponse = objTransacAPI.SendOTP(MobileNo, TotalBillAmount,idNo);
            return objResponse;
        }
        public DistributorBillModel getInvoice(string BillNo, string CurrentPartyCode, string id)
        {
            DistributorBillModel objResponse = objTransacAPI.getInvoice(BillNo, CurrentPartyCode,  id);
            return objResponse;
        }
        public DistributorBillModel getInvoiceNew(string BillNo, string CurrentPartyCode, string id)
        {
            DistributorBillModel objResponse = objTransacAPI.getInvoiceNew(BillNo, CurrentPartyCode, id);
            return objResponse;
        }
        public List<PartyModel> GetAllParty(string LoginPartyCode, decimal LoginStateCode)
        {
            return (objTransacAPI.GetAllParty(LoginPartyCode, LoginStateCode));
        }

        public List<PartyModel> GetAllPartyNew(string LoginPartyCode, decimal LoginStateCode, bool NeedWallet)
        {
            return (objTransacAPI.GetAllPartyNew(LoginPartyCode, LoginStateCode, NeedWallet));
        }
        public List<GroupModel> GetGroupList()
        {
            List<GroupModel> objGroupList = new List<GroupModel>();
            objGroupList = objTransacAPI.GetGroupList();
            return objGroupList;
        }

        public List<PartyModel> GetPartyList()
        {
            List<PartyModel> objPartyList = new List<PartyModel>();
            objPartyList = objTransacAPI.GetPartyList();
            return objPartyList;
        }
        public ResponseDetail SaveStockJv(StockJv objModel)
        {
            return (objTransacAPI.SaveStockJv(objModel));
        }
        public ResponseDetail SavePurchaseInvoice(DistributorBillModel objModel)
        {
            return (objTransacAPI.SavePurchaseInvoice(objModel));
        }
        public List<PartyModel> GetAllSupplierList(string LoginPartyCode, decimal LoginStateCode)
        {
            return (objTransacAPI.GetAllSupplierList(LoginPartyCode, LoginStateCode));
        }
        public List<PurchaseReport> GetPurchaseInvoice(string InvoiceNo)
        {
            return (objTransacAPI.GetPurchaseInvoice(InvoiceNo));
        }
        public ResponseDetail SavePartyOrderDetails(PartyOrderModel objPartyOrderModel)
        {
            return (objTransacAPI.SavePartyOrderDetails(objPartyOrderModel));
        }
        public decimal GetPartyWalletBalance(string LoginPartyCode,string vtype)
        {
            return (objTransacAPI.GetPartyWalletBalance(LoginPartyCode,vtype));
        }
        public string GetOrderNo(string LoginPartyCode)
        {
            return (objTransacAPI.GetOrderNo(LoginPartyCode));
        }
        public List<ProductModel> GetOrderProductList(string OrderNo, string OrderBy)
        {
            return (objTransacAPI.GetOrderProductList(OrderNo, OrderBy));
        }
        public List<ProductModel> GetPendingOrderProductList(string OrderNo, string OrderBy,string CurrentPartyCode)
        {
            return (objTransacAPI.GetPendingOrderProductList(OrderNo, OrderBy, CurrentPartyCode));
        }
        public List<PartyOrderModel> GetOrderList(string OrderBy, string OrderTo, string status)
        {
            return (objTransacAPI.GetOrderList(OrderBy, OrderTo, status));
        }
        public PartyOrderModel GetOrderPrintdata(string OrderNo)
        {
            return (objTransacAPI.GetOrderPrintdata(OrderNo));
        }
        public ResponseDetail SaveDispatchOrder(PartyOrderModel objPartyDispatchOrder)
        {
            return (objTransacAPI.SaveDispatchOrder(objPartyDispatchOrder));
        }
        public List<DisptachOrderModel> GetDispatchOrderList(string FromDate, string ToDate, string PartyCode, string ViewType, string IdNo, string OrderNo, string DispMode)
        {
            return (objTransacAPI.GetDispatchOrderList(FromDate, ToDate, PartyCode, ViewType, IdNo, OrderNo, DispMode));
        }
        public ResponseDetail RejectOrder(string OrderNo, string RejectReason, decimal RejectedByUserId)
        {
            return (objTransacAPI.RejectOrder(OrderNo, RejectReason, RejectedByUserId));
        }
        public List<ProductModel> GetOrderProduct(string OrderNo, string CurrentPartyCode)
        {
            return (objTransacAPI.GetOrderProduct(OrderNo, CurrentPartyCode));
        }
        public ResponseDetail SaveDispatchOrderdetails(List<DisptachOrderModel> objModel)
        {
            return (objTransacAPI.SaveDispatchOrderdetails(objModel));
        }

        public ResponseDetail RejectFranchiseOrder(string OrderNo, string RejectReason, decimal RejectedByUserId)
        {
            return (objTransacAPI.RejectFranchiseOrder(OrderNo, RejectReason, RejectedByUserId));
        }
        public ResponseDetail SaveOrderReturn(SalesReturnModel objPartyDispatchOrder)
        {
            return (objTransacAPI.SaveOrderReturn(objPartyDispatchOrder));
        }
        public List<OldBills> GetOldBills(string FromDate, string ToDate, string IdNo, string OrderNo, string PartyCode)
        {
            return (objTransacAPI.GetOldBills(FromDate, ToDate, IdNo, OrderNo, PartyCode));
        }
        public List<ProductModel> GetBillProducts(string billNo)
        {
            return (objTransacAPI.GetOldBillProducts(billNo));
        }
        public ResponseDetail DeleteBills(string BillNo, string FsessId, decimal UserId, string Reason)
        {
            return (objTransacAPI.DeleteBills(BillNo, FsessId, UserId, Reason));
        }
        public List<KitDetail> GetKitIdList()
        {
            return (objTransacAPI.GetKitIdList());
        }

        public KitDescriptionModel GetKitDescription(decimal KitId)
        {
            return (objTransacAPI.GetKitDescription(KitId));
        }

        public List<ProductModel> GetOfferList(string Doj, string UpgradeDate, bool IsFirstBill, bool ActiveStatus, string CurrentParty)
        {
            return (objTransacAPI.GetOfferList(Doj, UpgradeDate, IsFirstBill, ActiveStatus, CurrentParty));
        }
        //public List<OfferProducts> GetOfferDetail(decimal OfferId)
        //{
        //    return (objTransacAPI.GetOfferDetail(OfferId));
        //}
        //public ConfigDetails GetConfigDetails()
        //{
        //    ConfigDetails objConfigDetails = objTransacAPI.GetConfigDetails();
        //    return (objConfigDetails);
        //}
        //public decimal GetWalletBalance(string CustCode)
        //{
        //    return (objTransacAPI.GetWalletBalance(CustCode));
        //}
        //public ReferenceModel CheckReferenceId(string CustCode)
        //{
        //    return (objTransacAPI.CheckReferenceId(CustCode));
        //}
        //public Task<ResponseDetail> IssueCard(string CardNo, string IdNo, string ContactNo, string CustomerType)
        //{
        //    return (objTransacAPI.IssueCard(CardNo, IdNo,ContactNo, CustomerType));
        //}

        public ResponseDetail DeletePurchaseInvoice(string InwardNo, decimal FsessId, decimal UserId, string Reason)
        {
            return (objTransacAPI.DeletePurchaseInvoice(InwardNo, FsessId, UserId, Reason));
        }
        public string GetSalesReturnNumber(string Loggedinparty)
        {
            return (objTransacAPI.GetSalesReturnNumber(Loggedinparty));
        }
        public List<PartyBill> GetBillList(string partyType, string Fcode)
        {
            return (objTransacAPI.GetBillList(partyType, Fcode));
        }

        public List<PartyBill> GetListOfSupplierBills(string supplier)
        {
            return (objTransacAPI.GetListOfSupplierBills(supplier));
        }

        public PartyBill GetBillDetail(string BillNo)
        {
            return (objTransacAPI.GetBillDetail(BillNo));
        }
        public PartyBill GetBillDetailNew(string BillNo, string partycode)
        {
            return (objTransacAPI.GetBillDetailNew(BillNo,partycode));
        }
        public ResponseDetail SavePartyTargetDetails(PartyTargetMaster objModel)
        {
            return (objTransacAPI.SavePartyTargetDetails(objModel));
        }

        public List<SalesReport> GetRecordToUpdateDelDetails(string FromDate, string ToDate, string PartyCode, string Fcode, string status)
        {
            return (objTransacAPI.GetRecordToUpdateDelDetails(FromDate, ToDate, PartyCode, Fcode, status));
        }

        public ResponseDetail UpdateDeliveryDetails(UpdateDeliveryDetails obj)
        {
            return (objTransacAPI.UpdateDeliveryDetails(obj));
        }

        public List<kit> GetKitList()
        {
            return (objTransacAPI.GetKitList());
        }

        public List<PackUnpackProduct> GetPackUnpackProducts(string PackUnpack, decimal KitId, string prodID, string LoginPartyCode)
        {
            return (objTransacAPI.GetPackUnpackProducts(PackUnpack, KitId, prodID, LoginPartyCode));
        }

        public ResponseDetail SavePackUnpack(PackUnpack obj)
        {
            return (objTransacAPI.SavePackUnpack(obj));
        }

        public UpgradeID GetCustomerKitDetail(string obj,bool IsHO)
        {
            return (objTransacAPI.GetCustomerKitDetail(obj, IsHO));
        }
        public UpgradeID GetKitProductList(string kitId)
        {
            return (objTransacAPI.GetKitProductList(kitId));
        }
        public ResponseDetail CheckForOffer(DistributorBillModel objModel)
        {
            ResponseDetail objResponse = objTransacAPI.CheckForOffer(objModel);
            return objResponse;
        }
        public bool CanUserAccessMenu(int UserID, string MenuFile)
        {
            return objTransacAPI.CanUserAccessMenu(UserID, MenuFile);
        }
        public ResponseDetail SaveOffer(Offer offerDetail)
        {
            ResponseDetail objResponse = objTransacAPI.SaveOffer(offerDetail);
            return objResponse;
        }
        public List<ProductModel> GetproductInfobyCode(string data)
        {
            return (objTransacAPI.GetproductInfobyCode(data));
        }
        public List<Offer> GetAllOfferList(decimal OfferType)
        {
            return (objTransacAPI.GetAllOfferList(OfferType));
        }
		
		public async Task<string> SaveWalletRequest(WalletRequest objWallet)
        {
            return await objTransacAPI.SaveWalletRequest(objWallet); 
        }
        public Offer GetSelectedOfferDetails(decimal OfferId)
        {
            return (objTransacAPI.GetSelectedOfferDetails(OfferId));
        }
        public List<OfferProducts> GetSelectedOfferProductList(decimal OfferId,int Type)
        {
            return (objTransacAPI.GetSelectedOfferProductList(OfferId,Type));
        }
        public List<Offer> GetAllOtherOfferList()
        {
            return (objTransacAPI.GetAllOtherOfferList());
        }
        public Offer GetSelectedOtherOfferDetails(decimal OfferId)
        {
            return (objTransacAPI.GetSelectedOtherOfferDetails(OfferId));
        }
        public ResponseDetail SaveOtherOffer(Offer offerDetail)
        {
            ResponseDetail objResponse = objTransacAPI.SaveOtherOffer(offerDetail);
            return objResponse;
        }
        public List<DrawProds> getDrawProductList()
        {
            return objTransacAPI.getDrawProductList();
        }
        public ResponseDetail SaveDraw(Draw offerDetail)
        {
            ResponseDetail objResponse = objTransacAPI.SaveDraw(offerDetail);
            return objResponse;
        }
        public ResponseDetail DeleteDraw(Draw offerDetail)
        {
            ResponseDetail objResponse = objTransacAPI.DeleteDraw(offerDetail);
            return objResponse;
        }
        public List<Draw> getAllDraws()
        {
            return objTransacAPI.getAllDraws();
        }

        public ResponseDetail SaveDrawPoducts(List<DrawProds> products)
        {
            ResponseDetail objResponse = objTransacAPI.SaveDrawPoducts(products);
            return objResponse;
        }
        public ResponseDetail GetFreeVoucherDetail(string VoucherNo, string IdNo)
        {
            ResponseDetail objResponse = objTransacAPI.GetFreeVoucherDetail(VoucherNo, IdNo);
            return objResponse;
        }
        public ResponseDetail GetFreeCPVVoucherDetail(string VoucherNo, string IdNo)
        {
            ResponseDetail objResponse = objTransacAPI.GetFreeCPVVoucherDetail(VoucherNo, IdNo);
            return objResponse;
        }
        public List<string> GetFPVProductsOnly(int ProdBunchID,string InvoiceType)
        {
            return (objTransacAPI.GetAutocompFPVProducts(ProdBunchID, InvoiceType));
        }
        public ResponseDetail SaveIssueSampleProducts(DistributorBillModel products)
        {
            ResponseDetail objResponse = objTransacAPI.SaveIssueSampleProducts(products);
            return objResponse;
        }

        public ResponseDetail CheckBillCustomer(string mobile)
        {
            ResponseDetail objResponse = objTransacAPI.CheckCustomer(mobile);
            return objResponse;
        }

        public ResponseDetail ActivateIdWithPackage(UpgradeID objModel)
        {
            return (objTransacAPI.ActivateIdWithPackage(objModel));
        }

        public Boolean IsFPVInvoiceValid()
        {
            return (objTransacAPI.IsFPVInvoiceValid());
        }
        public ResponseDetail DebitCreditWallet(Wallet objWallet)
        {
            return (objTransacAPI.DebitCreditWallet(objWallet));
        }
        public List<PartyModel> GetPartyBalance()
        {
            return (objTransacAPI.GetPartyBalance());
        }
        public ResponseDetail GetGVDetail(string VoucherNo, string FormNo)
        {
            return (objTransacAPI.GetGVDetail(VoucherNo, FormNo));
        }
        public ResponseDetail GetCEDDetail(string VoucherNo, string FormNo)
        {
            return (objTransacAPI.GetCEDDetail(VoucherNo, FormNo));
        }

        public List<WalletRequest> GetAllWalletRequest(string PartyCode, string dateType, string FromDate, string ToDate, string IsApproved)
        {
            return objTransacAPI.GetAllWalletRequest( PartyCode,  dateType,  FromDate,  ToDate,  IsApproved);
        }
        public ResponseDetail SaveApproveWaletRequest(List<WalletRequest> objModelList)
        {
            ResponseDetail objResponse = objTransacAPI.SaveApproveWaletRequest(objModelList);
            return objResponse;
        }
        public ResponseDetail RejectWalletRequest(string ReqNo, string RejectReason, int UserID)
        {
            ResponseDetail objResponse = objTransacAPI.RejectWalletRequest(ReqNo, RejectReason, UserID);
            return objResponse;
        }
        public CustomerDetail GetSJPCustInfo(string IdNo)
        {
            return (objTransacAPI.GetSJPCustInfo(IdNo));
        }

        public ResponseDetail GetFreeScratchCardDetail(string VoucherNo)
        {
            return (objTransacAPI.GetFreeScratchCardDetail(VoucherNo));
        }
        public List<SlabModel> GetSlab()
        {
            return (objTransacAPI.GetSlab());
        }
        public ResponseDetail SaveSlab(RewardPoint objModel)
        {
            return (objTransacAPI.SaveSlab(objModel));
        }
        public RewardPoint GetSlabPoint(int ID)
        {
            return (objTransacAPI.GetSlabPoint(ID));
        }
        public List<RewardPoint> GetAllSlab()
        {
            return (objTransacAPI.GetAllSlab());
        }
        public List<M_EInvoiceCredential> GetEInvoicecre()
        {
            List<M_EInvoiceCredential> obj = new List<M_EInvoiceCredential>();
            obj = objTransacAPI.GetEInvoicecre();
            return obj;
        }
        public ResponseDetail SaveEInvoiceCredentail(M_EInvoiceCredential Model)
        {
            ResponseDetail obj = new ResponseDetail();
            obj = objTransacAPI.SaveEInvoiceCredentail(Model);
            return obj;
        }
        public FPVVoucher GetCheckFpWallet(string Idno)
        {
            FPVVoucher obj = new FPVVoucher();
            obj = objTransacAPI.GetCheckFpWallet(Idno);
            return obj;
        }
        public FPVoucherEligibilityResult CheckFPVoucherEligibility(string idno)
        {
            FPVoucherEligibilityResult obj = new FPVoucherEligibilityResult();
            obj = objTransacAPI.CheckFPVoucherEligibility(idno);
            return obj;
        }
        public List<VoucherTypeModel> GetWalletTypes()
        {
            return objTransacAPI.GetWalletTypes();
        }
        public List<Courier> GetCouierList(int CourierID)
        {
            return objTransacAPI.GetCouierList(CourierID);
        }
        public Courier CourierDetailByweight(int CourierId, int Weight)
        {
            return objTransacAPI.CourierDetailByweight(CourierId, Weight);
        }
        public ResponseDetail SaveBillDetail(DistributorBillModel model)
        {
            ResponseDetail objResponse = objTransacAPI.SaveBillDetail(model);
            return objResponse;
        }
    }


}
