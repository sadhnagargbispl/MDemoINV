using InventoryManagement.API.Controllers;
using InventoryManagement.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.DataAccess
{
    public class MasterRepository
    {
        MasterAPIController objMasterAPI = new MasterAPIController();
        public FPVMaster GetFPVDetail()
        {
            return objMasterAPI.GetFPVDetail();
        }

        public ResponseDetail SaveFPVDetails(FPVMaster objDetails)
        {
            return objMasterAPI.SaveFPVDetails(objDetails);
        }

        public List<OfferProducts> getFPVProducts(int BunchID)
        {
            return objMasterAPI.getFPVProducts(BunchID);
        }

        public List<IdAssignment> GetAllPCID(string PartyCode)
        {
            return objMasterAPI.GetAllPCID(PartyCode);
        }

        public ResponseDetail AssignIDToShoppe(IdAssignment ObjAssignList)
        {
            return objMasterAPI.AssignIDToShoppe(ObjAssignList);
        }

        public List<CourierchargeByWeight> GetChargeList()
        {
            return objMasterAPI.GetChargeList();
        }

        public ResponseDetail SaveCourierCharge(CourierchargeByWeight obj)
        {
            return objMasterAPI.SaveCourierCharge(obj);
        }

        public ResponseDetail getIdUpgradeDetails()
        {
            return objMasterAPI.getIdUpgradeDetails();
        }

        public ResponseDetail UpdateIDUpgradeDetails(IDUdgradeViewModel viewModel)
        {
            return (objMasterAPI.UpdateIDUpgradeDetails(viewModel));
        }
    }
}