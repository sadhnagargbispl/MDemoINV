using InventoryManagement.App_Start;
using InventoryManagement.Business;
using InventoryManagement.Common;
using InventoryManagement.Entity.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        UserManager objUserManager = new UserManager();
        // GET: User
        [SessionExpire]
        public ActionResult UserMasterList()
        {

            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "UserMasterList"))
                return View();
            else
                 return RedirectToAction("Dashboard", "Home");
        }
        [SessionExpire]
        public ActionResult AddUser(string IsActionName, string UserCode)
        {
            User objModel = new User();
            var result = objUserManager.GetPartyListForUsers();
            
            List<SelectListItem> PartyList = new List<SelectListItem>();
            foreach (var obj in result)
            {
                PartyList.Add(new SelectListItem
                {
                    Text = obj.PartyName,
                    Value = obj.PartyCode.ToString()
                });
            }
            ViewBag.PartyList = PartyList;

            
            List<SelectListItem> objActiveStatus = new List<SelectListItem>();
            objActiveStatus.Add(new SelectListItem
            {
                Text = "Yes",
                Value = "Y"
            });

            objActiveStatus.Add(new SelectListItem
            {
                Text = "No",
                Value = "N"
            });
            ViewBag.ActiveStatus = objActiveStatus;
            if (result.Count > 0)
            {
                objModel.FCode = result[0].PartyCode;
                objModel.GroupId = (int)objUserManager.GetPartyGroupId(result[0].PartyCode);
            }
            if (IsActionName == "Add")
            {
               
                
                objModel.ActiveStatus = "Y";
            }
            else
            {
                if (!string.IsNullOrEmpty(UserCode))
                {
                    int UId = int.Parse(UserCode);
                    objModel = objUserManager.GetUser(UId);

                }
            }
            objModel.IsActionName = IsActionName;
            return View(objModel);

        }
        public ActionResult GetAllUserList()
        {
            List<User> objUserList = new List<User>();
            objUserList = objUserManager.GetAllUserList();
            var jsonUserList = Json(objUserList, JsonRequestBehavior.AllowGet);
            jsonUserList.MaxJsonLength = int.MaxValue;
            return jsonUserList;
        }
        [HttpPost]
        public ActionResult SaveUserDetails(User objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
           
            objResponse = objUserManager.AddEditUserDetails(objModel, Session["LoginUser"] as User);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult IsDuplicateUserName(string IsActionType, string UserId, string UserName)
        {
            return Json(objUserManager.IsDuplicateUserName(IsActionType, UserId, UserName), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPartyGroupId(string SelectedPartyCode)
        {
            return Json(objUserManager.GetPartyGroupId(SelectedPartyCode), JsonRequestBehavior.AllowGet);
        }
        [SessionExpire]
        public ActionResult SetUserRights()
        {
            UserPermissionMasterModel objUserList = new UserPermissionMasterModel();
            objUserList.UserList = objUserManager.GetUserList();
            if (objUserList.UserList != null && objUserList.UserList.Count() > 0)
            {
                ViewBag.Selecteduser = objUserList.UserList[0].UserId;
            }
            else
            {
                ViewBag.Selecteduser = 0;
            }
  
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "UserMasterList"))
                return View(objUserList);
            else
                 return RedirectToAction("Dashboard", "Home");
        }

        [SessionExpire]
        public ActionResult StopInvoicing()
        {
            UserPermissionMasterModel objUserList = new UserPermissionMasterModel();
            objUserList.MenuList = new List<MenuMasterModel>();
            objUserList.MenuList = objUserManager.GetAllMenuList();
            if (objUserList.MenuList != null && objUserList.MenuList.Count() > 0)
            {
                ViewBag.Selectedmenu = objUserList.MenuList[0].MenuId;
            }
            else
            {
                ViewBag.Selectedmenu = 0;
            }
     
            if (new TransactionController().CanUserAccessMenu((Session["LoginUser"] as User).UserId, "UserMasterList"))
                return View(objUserList);
            else
                 return RedirectToAction("Dashboard", "Home");
        }

        public ActionResult GetPermissionList(string UserId)
        {
            decimal userID = 0;
            if (!(string.IsNullOrEmpty(UserId)))
            {
                userID = decimal.Parse(UserId);
            }
            return Json(objUserManager.ListUserPermittedMenus(userID),JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPermittedUserList(decimal MenuId)
        {            
            return Json(objUserManager.GetPermittedUserList(MenuId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveUserRights(UserPermissionMasterModel PermittedList)
        {
            ResponseDetail objResponse = new ResponseDetail();
            List<UserPermissionMasterModel> objPermissionList = new List<UserPermissionMasterModel>();

            if (!string.IsNullOrEmpty(PermittedList.ListPermittedMenuList))
            {
                var objects = JArray.Parse(PermittedList.ListPermittedMenuList); // parse as array
                  
                List<string> userids = PermittedList.SelectedUserId.Split(',').ToList();
                int index = userids.IndexOf(PermittedList.UserId.ToString());
                if (index == -1)
                {
                    userids.Add(PermittedList.UserId.ToString());
                }

                foreach (string id in userids)
                {
                    if (!string.IsNullOrEmpty(id) && id != "0")
                    {
                        foreach (JObject root in objects)
                        {
                            UserPermissionMasterModel objTemp = new UserPermissionMasterModel();
                            foreach (KeyValuePair<String, JToken> app in root)
                            {
                                if (app.Key == "MenuId")
                                {
                                    objTemp.MenuId = (decimal)app.Value;
                                }
                                //else if (app.Key == "UserId")
                                //{
                                //    objTemp.UserId = (decimal)app.Value;
                                //}
                                else if (app.Key == "IsPermitted")
                                {
                                    objTemp.IsPermitted = (bool)app.Value;
                                }
                            }
                            objTemp.CurrentLoginUser = Session["LoginUser"] as User;
                            objTemp.UserId = Convert.ToDecimal(id);
                            objPermissionList.Add(objTemp);
                        }
                    }
                }
            }
                objResponse = objUserManager.SetUserRights(objPermissionList);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult StopInvoicingSave(UserPermissionMasterModel PermittedList)
        {
            ResponseDetail objResponse = new ResponseDetail();
            List<UserPermissionMasterModel> objPermissionList = new List<UserPermissionMasterModel>();

            if (!string.IsNullOrEmpty(PermittedList.ListPermittedMenuList))
            {
                var objects = JArray.Parse(PermittedList.ListPermittedMenuList); // parse as array  
                foreach (JObject root in objects)
                {
                    UserPermissionMasterModel objTemp = new UserPermissionMasterModel();
                    foreach (KeyValuePair<String, JToken> app in root)
                    {                       
                        if (app.Key == "UserId")
                        {
                            objTemp.UserId = (decimal)app.Value;
                        }
                        else if (app.Key == "IsPermitted")
                        {
                            objTemp.IsPermitted = (bool)app.Value;
                        }
                    }
                    objTemp.CurrentLoginUser = Session["LoginUser"] as User;
                    objTemp.MenuId = PermittedList.MenuId;
                    objPermissionList.Add(objTemp);
                }
            }
            objResponse = objUserManager.StopInvoicingSave(objPermissionList);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }
        public string UserCanAccess(int UserID, string MenuFile)
        {
            return objUserManager.UserCanAccess(UserID, MenuFile);
        }

    }
}