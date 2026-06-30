using InventoryManagement.DataAccess;
using InventoryManagement.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Business
{
    public class MasterManager
    {
        MasterRepository objMasterRepository = new MasterRepository();

        public FPVMaster GetFPVDetail()
        {
            return objMasterRepository.GetFPVDetail();
        }        

        public ResponseDetail SaveFPVDetails(FPVMaster objDetails)
        {
            return objMasterRepository.SaveFPVDetails(objDetails);
        }

        public List<OfferProducts> getFPVProducts(int BunchID)
        {
            return objMasterRepository.getFPVProducts( BunchID);
        }

        public List<IdAssignment> GetAllPCID(string PartyCode)
        {
            return objMasterRepository.GetAllPCID(PartyCode);
        }

        public ResponseDetail AssignIDToShoppe(IdAssignment ObjAssignList)
        {
            return objMasterRepository.AssignIDToShoppe(ObjAssignList);
        }

        public List<CourierchargeByWeight> GetChargeList()
        {
            return objMasterRepository.GetChargeList();
        }

        public ResponseDetail SaveCourierCharge(CourierchargeByWeight obj)
        {
            return objMasterRepository.SaveCourierCharge(obj);
        }

        public ResponseDetail getIdUpgradeDetails()
        {
            return objMasterRepository.getIdUpgradeDetails();
        }

        public ResponseDetail UpdateIDUpgradeDetails(IDUdgradeViewModel viewModel)
        {
            return (objMasterRepository.UpdateIDUpgradeDetails(viewModel));
        }

    }
}