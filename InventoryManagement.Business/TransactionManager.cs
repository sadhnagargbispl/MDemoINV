using InventoryManagement.API.Models;
using InventoryManagement.Business.Contract;
using InventoryManagement.DataAccess;
using InventoryManagement.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InventoryManagement.Business
{
    public class TransactionManager
    {
        TransactionRepository objTransacRepo = new TransactionRepository();
        public List<string> GetAutocompleteProductNames(string InvType)
        {
            return (objTransacRepo.GetAutocompleteProductNames(InvType));
        }
        public List<string> GetAvailStockProductNamesOnly(string StockforParty)
        {
            return (objTransacRepo.GetAvailStockProductNamesOnly(StockforParty));
        }
        public List<string> GetAllBarcode()
        {
            return (objTransacRepo.GetAllBarcode());
        }
        public List<string> GetAutocompProductsOnly(string StockforParty)
        {
            return (objTransacRepo.GetAutocompProductsOnly(StockforParty));
        }
        
       
        public List<OfferProducts> GetOfferProductNamesOnly(decimal OfferId, int type)
        {
            return (objTransacRepo.GetOfferProductNamesOnly(OfferId, type));
        }

        public List<OfferProducts> GetOfferBuyProducts(decimal OfferId)
        {
            return (objTransacRepo.GetOfferBuyProducts(OfferId));
        }


        public List<ProductModel> GetproductInfo(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool bvhalf, string Invoice, string IsSpclOffer)
        {
            return (objTransacRepo.GetproductInfo(SearchType, data, isCForm, BillType, CurrentStateCode, CurrentPartyCode, IsBillOnMrp, OfferID, bvhalf, Invoice, IsSpclOffer));
        }
        public List<ProductModel> GetproductInfoBatchWise(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool bvhalf, string Invoice, string IsSpclOffer)
        {
            return (objTransacRepo.GetproductInfoBatchWise(SearchType, data, isCForm, BillType, CurrentStateCode, CurrentPartyCode, IsBillOnMrp, OfferID, bvhalf, Invoice, IsSpclOffer));
        }
        public List<ProductModel> GetAllproductInfoBatchWise(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool bvhalf, string Invoice, string IsSpclOffer)
        {
            return (objTransacRepo.GetAllproductInfoBatchWise(SearchType, data, isCForm, BillType, CurrentStateCode, CurrentPartyCode, IsBillOnMrp, OfferID, bvhalf, Invoice, IsSpclOffer));
        }
        public List<ProductModel> PendingGetProductInfo(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool bvhalf, string Invoice, string IsSpclOffer)
        {
            return (objTransacRepo.PendingGetProductInfo(SearchType, data, isCForm, BillType, CurrentStateCode, CurrentPartyCode, IsBillOnMrp, OfferID, bvhalf, Invoice, IsSpclOffer));
        }
        public List<ProductModel> GetAddStockGVproductInfo(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool bvhalf, string Invoice, string IsSpclOffer)
        {
            return (objTransacRepo.GetAddStockGVproductInfo(SearchType, data, isCForm, BillType, CurrentStateCode, CurrentPartyCode, IsBillOnMrp, OfferID, bvhalf, Invoice, IsSpclOffer));
        }
        public List<ProductModel> GetproductBatchInfo(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool bvhalf, string Invoice, string IsSpclOffer)
        {
            return (objTransacRepo.GetproductBatchInfo(SearchType, data, isCForm, BillType, CurrentStateCode, CurrentPartyCode, IsBillOnMrp, OfferID, bvhalf, Invoice, IsSpclOffer));
        }
        public CustomerDetail GetCustInfo(string IdNo)
        {
            return (objTransacRepo.GetCustInfo(IdNo));
        }
        public CustomerDetail GetCustPasswordValidate(string IdNo, string Password)
        {
            return (objTransacRepo.GetCustPasswordValidate(IdNo, Password));
        }
        public MemberAPIRoot ValidateCustomerbyAPI(string IdNo, string Password)
        {
            return (objTransacRepo.ValidateCustomerbyAPI(IdNo, Password));
        }
        public Coupons CheckCoupon(string Code, string IdNo)
        {
            return (objTransacRepo.CheckCoupon(Code, IdNo));
        }
        public FPVVoucher CheckFpVoucher(string Code, string IdNo)
        {
            return (objTransacRepo.CheckFpVoucher(Code, IdNo));
        }
        public async Task<ResponseDetail> SaveDistributorBill(DistributorBillModel objModel)
        {
            return await objTransacRepo.SaveDistributorBill(objModel);
        }
        public List<BankModel> GetBankList()
        {
            List<BankModel> objListBank = objTransacRepo.GetBankList();
            return (objListBank);
        }
        public Task<ResponseDetail> SendOTP(string MobileNo, string TotalBillAmount, string idNo)
        {
            Task<ResponseDetail> objResponse = objTransacRepo.SendOTP(MobileNo, TotalBillAmount, idNo);
            return objResponse;
        }
        public DistributorBillModel getInvoice(string BillNo, string CurrentPartyCode, string id)
        {
            DistributorBillModel objResponse = objTransacRepo.getInvoice(BillNo, CurrentPartyCode, id);
            return objResponse;
        }
        public DistributorBillModel getInvoiceNew(string BillNo, string CurrentPartyCode, string id)
        {
            DistributorBillModel objResponse = objTransacRepo.getInvoiceNew(BillNo, CurrentPartyCode, id);
            return objResponse;
        }
        public List<PartyModel> GetAllParty(string LoginPartyCode, decimal LoginStateCode)
        {
            return (objTransacRepo.GetAllParty(LoginPartyCode, LoginStateCode));
        }
        public List<PartyModel> GetAllPartyNew(string LoginPartyCode, decimal LoginStateCode, bool NeedWallet)
        {
            return (objTransacRepo.GetAllPartyNew(LoginPartyCode, LoginStateCode, NeedWallet));
        }

        public List<GroupModel> GetGroupList()
        {
            List<GroupModel> objGroupList = new List<GroupModel>();
            objGroupList = objTransacRepo.GetGroupList();
            return objGroupList;
        }

        public List<PartyModel> GetPartyList()
        {
            List<PartyModel> objPartyList = new List<PartyModel>();
            objPartyList = objTransacRepo.GetPartyList();
            return objPartyList;
        }
        public ResponseDetail SaveStockJv(StockJv objModel)
        {
            return (objTransacRepo.SaveStockJv(objModel));
        }
        public ResponseDetail SavePurchaseInvoice(DistributorBillModel objModel)
        {
            return (objTransacRepo.SavePurchaseInvoice(objModel));
        }
        public List<ProductModel> GetOfferList(string Doj, string UpgradeDate, bool IsFirstBill, bool ActiveStatus, string CurrentParty)
        {
            return (objTransacRepo.GetOfferList(Doj, UpgradeDate, IsFirstBill, ActiveStatus, CurrentParty));
        }
        //public List<OfferProducts> GetOfferDetail(decimal OfferId)
        //{
        //    return (objTransacRepo.GetOfferDetail(OfferId));
        //}
        public List<PartyModel> GetAllSupplierList(string LoginPartyCode, decimal LoginStateCode)
        {
            return (objTransacRepo.GetAllSupplierList(LoginPartyCode, LoginStateCode));
        }
        public List<PurchaseReport> GetPurchaseInvoice(string InvoiceNo)
        {
            return (objTransacRepo.GetPurchaseInvoice(InvoiceNo));
        }
        public ResponseDetail SavePartyOrderDetails(PartyOrderModel objPartyOrderModel)
        {
            return (objTransacRepo.SavePartyOrderDetails(objPartyOrderModel));
        }
        public decimal GetPartyWalletBalance(string LoginPartyCode, string vtype)
        {
            return (objTransacRepo.GetPartyWalletBalance(LoginPartyCode, vtype));
        }
        public string GetOrderNo(string LoginPartyCode)
        {
            return (objTransacRepo.GetOrderNo(LoginPartyCode));
        }
        public List<ProductModel> GetOrderProductList(string OrderNo, string OrderBy)
        {
            return (objTransacRepo.GetOrderProductList(OrderNo, OrderBy));
        }
        public List<ProductModel> GetPendingOrderProductList(string OrderNo, string OrderBy, string CurrentPartyCode)
        {
            return (objTransacRepo.GetPendingOrderProductList(OrderNo, OrderBy, CurrentPartyCode));
        }
        public List<PartyOrderModel> GetOrderList(string OrderBy, string OrderTo, string Status)
        {
            return (objTransacRepo.GetOrderList(OrderBy, OrderTo, Status));
        }
        public PartyOrderModel GetOrderPrintdata(string OrderNo)
        {
            return (objTransacRepo.GetOrderPrintdata(OrderNo));
        }
        public ResponseDetail SaveDispatchOrder(PartyOrderModel objPartyDispatchOrder)
        {
            return (objTransacRepo.SaveDispatchOrder(objPartyDispatchOrder));
        }
        public List<DisptachOrderModel> GetDispatchOrderList(string FromDate, string ToDate, string PartyCode, string ViewType, string IdNo, string OrderNo, string DispMode)
        {
            return (objTransacRepo.GetDispatchOrderList(FromDate, ToDate, PartyCode, ViewType, IdNo, OrderNo, DispMode));
        }
        public List<OldBills> GetOldBills(string FromDate, string ToDate, string IdNo, string OrderNo, string PartyCode)
        {
            return (objTransacRepo.GetOldBills(FromDate, ToDate, IdNo, OrderNo, PartyCode));
        }
        public List<ProductModel> GetBillProducts(string billNo)
        {
            return (objTransacRepo.GetBillProducts(billNo));
        }
        public ResponseDetail RejectOrder(string OrderNo, string RejectReason, decimal RejectedByUserId)
        {
            return (objTransacRepo.RejectOrder(OrderNo, RejectReason, RejectedByUserId));
        }
        public List<ProductModel> GetOrderProduct(string OrderNo, string CurrentPartyCode)
        {
            return (objTransacRepo.GetOrderProduct(OrderNo, CurrentPartyCode));
        }
        public ResponseDetail SaveDispatchOrderdetails(List<DisptachOrderModel> objModel)
        {
            return (objTransacRepo.SaveDispatchOrderdetails(objModel));
        }
        public ResponseDetail RejectFranchiseOrder(string OrderNo, string RejectReason, decimal RejectedByUserId)
        {
            return (objTransacRepo.RejectFranchiseOrder(OrderNo, RejectReason, RejectedByUserId));
        }
        public ResponseDetail SaveOrderReturn(SalesReturnModel objPartyDispatchOrder)
        {
            return (objTransacRepo.SaveOrderReturn(objPartyDispatchOrder));
        }
        public ResponseDetail DeleteBills(string BillNo, string FsessId, decimal UserId, string Reason)
        {
            return (objTransacRepo.DeleteBills(BillNo, FsessId, UserId, Reason));
        }
        public string GetSalesReturnNumber(string Loggedinparty)
        {
            return (objTransacRepo.GetSalesReturnNumber(Loggedinparty));
        }

        public List<PartyBill> GetBillList(string partyType, string Fcode)
        {
            return (objTransacRepo.GetBillList(partyType, Fcode));
        }
        public List<PartyBill> GetListOfSupplierBills(string supplier)
        {
            return (objTransacRepo.GetListOfSupplierBills(supplier));
        }









        public List<KitDetail> GetKitIdList()
        {
            return (objTransacRepo.GetKitIdList());
        }

        public PartyBill GetBillDetail(string BillNo)
        {
            return (objTransacRepo.GetBillDetail(BillNo));
        }
        public PartyBill GetBillDetailNew(string BillNo, string partycode)
        {
            return (objTransacRepo.GetBillDetailNew(BillNo, partycode));
        }
        public KitDescriptionModel GetKitDescription(decimal KitId)
        {
            return (objTransacRepo.GetKitDescription(KitId));
        }
        //public ConfigDetails GetConfigDetails()
        //{
        //    ConfigDetails objConfigDetails = objTransacRepo.GetConfigDetails();
        //    return (objConfigDetails);
        //}
        //public decimal GetWalletBalance(string CustCode)
        //{
        //    return (objTransacRepo.GetWalletBalance(CustCode));
        //}
        //public ReferenceModel CheckReferenceId(string CustCode)
        //{
        //    return (objTransacRepo.CheckReferenceId(CustCode));
        //}
        //public Task<ResponseDetail> IssueCard(string CardNo, string IdNo, string ContactNo, string CustomerType)
        //{
        //    return (objTransacRepo.IssueCard(CardNo, IdNo,ContactNo, CustomerType));
        //}
        public ResponseDetail DeletePurchaseInvoice(string InwardNo, decimal FsessId, decimal UserId, string Reason)
        {
            return (objTransacRepo.DeletePurchaseInvoice(InwardNo, FsessId, UserId, Reason));
        }

        public ResponseDetail SavePartyTargetDetails(PartyTargetMaster objModel)
        {
            return (objTransacRepo.SavePartyTargetDetails(objModel));
        }

        public List<SalesReport> GetRecordToUpdateDelDetails(string FromDate, string ToDate, string PartyCode, string Fcode, string status)
        {
            return (objTransacRepo.GetRecordToUpdateDelDetails(FromDate, ToDate, PartyCode, Fcode, status));
        }

        public ResponseDetail UpdateDeliveryDetails(UpdateDeliveryDetails obj)
        {
            return (objTransacRepo.UpdateDeliveryDetails(obj));
        }

        public List<kit> GetKitList()
        {
            return (objTransacRepo.GetKitList());
        }

        public List<PackUnpackProduct> GetPackUnpackProducts(string PackUnpack, decimal KitId, string prodID, string LoginPartyCode)
        {
            return (objTransacRepo.GetPackUnpackProducts(PackUnpack, KitId, prodID, LoginPartyCode));
        }

        public ResponseDetail SavePackUnpack(PackUnpack obj)
        {
            return (objTransacRepo.SavePackUnpack(obj));
        }

        public UpgradeID GetCustomerKitDetail(string obj, bool IsHO)
        {
            return (objTransacRepo.GetCustomerKitDetail(obj, IsHO));
        }
        public UpgradeID GetKitProductList(string kitId)
        {
            return (objTransacRepo.GetKitProductList(kitId));
        }
        public ResponseDetail CheckForOffer(DistributorBillModel objModel)
        {
            ResponseDetail objResponse = objTransacRepo.CheckForOffer(objModel);
            return objResponse;
        }

        public bool CanUserAccessMenu(int UserID, string MenuFile)
        {
            return objTransacRepo.CanUserAccessMenu(UserID, MenuFile);
        }
        public ResponseDetail SaveOffer(Offer offerDetail)
        {
            ResponseDetail objResponse = objTransacRepo.SaveOffer(offerDetail);
            return objResponse;
        }
        public List<ProductModel> GetproductInfobyCode(string data)
        {
            return (objTransacRepo.GetproductInfobyCode(data));
        }
        public List<Offer> GetAllOfferList(decimal OfferType)
        {
            return (objTransacRepo.GetAllOfferList(OfferType));
        }
        public Offer GetSelectedOfferDetails(decimal OfferId)
        {
            return (objTransacRepo.GetSelectedOfferDetails(OfferId));
        }
        public List<OfferProducts> GetSelectedOfferProductList(decimal OfferId, int Type)
        {
            return (objTransacRepo.GetSelectedOfferProductList(OfferId, Type));
        }
        public List<Offer> GetAllOtherOfferList()
        {
            return (objTransacRepo.GetAllOtherOfferList());
        }
        public Offer GetSelectedOtherOfferDetails(decimal OfferId)
        {
            return (objTransacRepo.GetSelectedOtherOfferDetails(OfferId));
        }
        public ResponseDetail SaveOtherOffer(Offer offerDetail)
        {
            ResponseDetail objResponse = objTransacRepo.SaveOtherOffer(offerDetail);
            return objResponse;
        }
        public List<DrawProds> getDrawProductList()
        {
            return objTransacRepo.getDrawProductList();
        }

        public ResponseDetail SaveDraw(Draw offerDetail)
        {
            ResponseDetail objResponse = objTransacRepo.SaveDraw(offerDetail);
            return objResponse;
        }

        public ResponseDetail DeleteDraw(Draw offerDetail)
        {
            ResponseDetail objResponse = objTransacRepo.DeleteDraw(offerDetail);
            return objResponse;
        }

        public List<Draw> getAllDraws()
        {
            return objTransacRepo.getAllDraws();
        }

        public ResponseDetail SaveDrawPoducts(List<DrawProds> products)
        {
            ResponseDetail objResponse = objTransacRepo.SaveDrawPoducts(products);
            return objResponse;
        }

        public ResponseDetail GetFreeVoucherDetail(string VoucherNo, string IdNo)
        {
            ResponseDetail objResponse = objTransacRepo.GetFreeVoucherDetail(VoucherNo, IdNo);
            return objResponse;
        }
        public ResponseDetail GetFreeCPVVoucherDetail(string VoucherNo, string IdNo)
        {
            ResponseDetail objResponse = objTransacRepo.GetFreeCPVVoucherDetail(VoucherNo, IdNo);
            return objResponse;
        }
        public ResponseDetail SaveIssueSampleProducts(DistributorBillModel products)
        {
            ResponseDetail objResponse = objTransacRepo.SaveIssueSampleProducts(products);
            return objResponse;
        }

        public List<string> GetFPVProductsOnly(int ProdBunchID, string InvoiceType)
        {
            return (objTransacRepo.GetFPVProductsOnly(ProdBunchID, InvoiceType));
        }

        public ResponseDetail CheckBillCustomer(string mobile)
        {
            ResponseDetail objResponse = objTransacRepo.CheckBillCustomer(mobile);
            return objResponse;
        }
        public ResponseDetail ActivateIdWithPackage(UpgradeID objModel)
        {
            return (objTransacRepo.ActivateIdWithPackage(objModel));
        }
        public Boolean IsFPVInvoiceValid()
        {
            return (objTransacRepo.IsFPVInvoiceValid());
        }

        public ResponseDetail DebitCreditWallet(Wallet objWallet)
        {
            return (objTransacRepo.DebitCreditWallet(objWallet));
        }
        public List<PartyModel> GetPartyBalance()
        {
            return (objTransacRepo.GetPartyBalance());
        }

        public async Task<string> SaveWalletRequest(WalletRequest objWallet)
        {
            return await objTransacRepo.SaveWalletRequest(objWallet);
        }

        public ResponseDetail GetGVDetail(string VoucherNo, string FormNo)
        {
            return (objTransacRepo.GetGVDetail(VoucherNo, FormNo));
        }
        public ResponseDetail GetCEDDetail(string VoucherNo, string FormNo)
        {
            return (objTransacRepo.GetCEDDetail(VoucherNo, FormNo));
        }

        public ResponseDetail SaveApproveWaletRequest(List<WalletRequest> objModelList)
        {
            ResponseDetail objResponse = objTransacRepo.SaveApproveWaletRequest(objModelList);
            return objResponse;
        }

        public ResponseDetail RejectWalletRequest(string ReqNo, string RejectReason, int UserID)
        {
            ResponseDetail objResponse = objTransacRepo.RejectWalletRequest(ReqNo, RejectReason, UserID);
            return objResponse;
        }

        public List<WalletRequest> GetAllWalletRequest(string PartyCode, string dateType, string FromDate, string ToDate, string IsApproved)
        {
            return objTransacRepo.GetAllWalletRequest(PartyCode, dateType, FromDate, ToDate, IsApproved);
        }

        public CustomerDetail GetSJPCustInfo(string IdNo)
        {
            return (objTransacRepo.GetSJPCustInfo(IdNo));
        }

        public ResponseDetail GetFreeScratchCardDetail(string VoucherNo)
        {
            return (objTransacRepo.GetFreeScratchCardDetail(VoucherNo));
        }
        public List<SlabModel> GetSlab()
        {
            return (objTransacRepo.GetSlab());
        }
        public ResponseDetail SaveSlab(RewardPoint objModel)
        {
            return (objTransacRepo.SaveSlab(objModel));
        }
        public RewardPoint GetSlabPoint(int ID)
        {
            return (objTransacRepo.GetSlabPoint(ID));
        }
        public List<RewardPoint> GetAllSlab()
        {
            return (objTransacRepo.GetAllSlab());
        }
        public List<M_EInvoiceCredential> GetEInvoicecre()
        {
            List<M_EInvoiceCredential> obj = new List<M_EInvoiceCredential>();
            obj = objTransacRepo.GetEInvoicecre();
            return obj;
        }
        public ResponseDetail SaveEInvoiceCredentail(M_EInvoiceCredential Model)
        {
            ResponseDetail obj = new ResponseDetail();
            obj = objTransacRepo.SaveEInvoiceCredentail(Model);
            return obj;
        }

        public FPVVoucher GetCheckFpWallet(string Idno)
        {
            FPVVoucher obj = new FPVVoucher();
            obj = objTransacRepo.GetCheckFpWallet(Idno);
            return obj;
        }
        public FPVoucherEligibilityResult CheckFPVoucherEligibility(string idno)
        {
            FPVoucherEligibilityResult obj = new FPVoucherEligibilityResult();
            obj = objTransacRepo.CheckFPVoucherEligibility(idno);
            return obj;
        }

        public List<VoucherTypeModel> GetWalletTypes()
        {
            return objTransacRepo.GetWalletTypes();
        }
        public List<Courier> GetCouierList(int CourierID)
        {
            return objTransacRepo.GetCouierList(CourierID);
        }
        public Courier CourierDetailByweight(int CourierId, int Weight)
        {
            return objTransacRepo.CourierDetailByweight(CourierId, Weight);
        }
        public ResponseDetail SaveBillDetail(DistributorBillModel model)
        {
            ResponseDetail objResponse = objTransacRepo.SaveBillDetail(model);
            return objResponse;
        }
    }
}
