using InventoryManagement.App_Start;
using InventoryManagement.Business;
using InventoryManagement.Entity.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    [Authorize]
    public class MasterController : Controller
    {
        // GET: Master
        MasterManager objMasterManager = new MasterManager();
        TransactionManager objTransacManager = new TransactionManager();

        public bool CanUserAccessMenu(int UserID, string MenuFile)
        {
            return objTransacManager.CanUserAccessMenu(UserID, MenuFile);
        }

        [SessionExpire]
        public ActionResult FPVMaster()
        {
            FPVMaster objMaster = null;
            try
            {
                List<SelectListItem> Activeoption = new List<SelectListItem>();
                Activeoption.Add(new SelectListItem() { Text = "Active", Value = "Y" });
                Activeoption.Add(new SelectListItem() { Text = "Deactive", Value = "N" });
                ViewBag.ActiveOptions = Activeoption;

                List<SelectListItem> OnBillType = new List<SelectListItem>();
                OnBillType.Add(new SelectListItem() { Text = "On FV", Value = "B" });
                OnBillType.Add(new SelectListItem() { Text = "On RV", Value = "R" });
                ViewBag.OnBillTypeList = OnBillType;

                List<SelectListItem> IsMultiple = new List<SelectListItem>();
                IsMultiple.Add(new SelectListItem() { Text = "Yes", Value = "Y" });
                IsMultiple.Add(new SelectListItem() { Text = "No", Value = "N" });
                ViewBag.IsMultipleList = IsMultiple;

                objMaster = objMasterManager.GetFPVDetail();
            }
            catch (Exception ex)
            {

            }
            if (CanUserAccessMenu((Session["LoginUser"] as User).UserId, "FPVMaster"))
            {
                return View(objMaster);
            }
            else
                 return RedirectToAction("Dashboard", "Home");

        }

        public ActionResult SaveFPV(FPVMaster objDetails)
        {
            if (!string.IsNullOrEmpty(objDetails.productString))
            {
                int BunchID = 0;
                objDetails.products = new List<OfferProducts>();
                var objects = JArray.Parse(objDetails.productString); // parse as array  
                foreach (JObject root in objects)
                {
                    OfferProducts objTemp = new OfferProducts();
                    foreach (KeyValuePair<String, JToken> app in root)
                    {
                        if (app.Key == "ProdCode")
                        {
                            objTemp.ProdID = (string)app.Value;
                        }
                        else if (app.Key == "ProductName")
                        {
                            objTemp.ProdName = (string)app.Value;
                        }
                    }

                    objDetails.products.Add(objTemp);
                }
            }
            return Json(objMasterManager.SaveFPVDetails(objDetails), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getFPVProducts(int BunchID)
        {
            return Json(objMasterManager.getFPVProducts( BunchID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssignIDToShoppe()
        {
            return View();
        }

        public ActionResult IDUdgrade()
        {
            IDUdgradeViewModel viewModel = new IDUdgradeViewModel();
            try
            {
                var config = objMasterManager.getIdUpgradeDetails();
                viewModel.CanIdBeUpgraded = config.ResponseMessage;
            }
            catch (Exception Ex)
            {

            }
            return View(viewModel);
        }

        
        [HttpPost]
            public ActionResult UpdateIDUpgradeDetails(IDUdgradeViewModel viewModel)
        {
            return Json(objMasterManager.UpdateIDUpgradeDetails(viewModel), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllPCID(string PartyCode)
        {
            return Json(objMasterManager.GetAllPCID(PartyCode), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AssignIDToShoppe(IdAssignment objDetails)
        {            
            return Json(objMasterManager.AssignIDToShoppe(objDetails), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddCourierCharges()
        {
            return View();
        }

        public ActionResult GetChargeList()
        {
            return Json(objMasterManager.GetChargeList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveCourierCharge(CourierchargeByWeight obj)
        {
            return Json(objMasterManager.SaveCourierCharge(obj), JsonRequestBehavior.AllowGet);
        }
    }
}