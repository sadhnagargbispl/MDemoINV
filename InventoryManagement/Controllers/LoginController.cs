using InventoryManagement.Entity.Common;
using System.Web.Mvc;
using System.Web.Security;
using InventoryManagement.Business;

namespace InventoryManagement.Controllers
{
    public class LoginController : Controller
    {
        LoginManager objLoginManager = new LoginManager();
        // GET: Login
        public ActionResult Login()
        {
            LoginModel model = new LoginModel();
            FormsAuthentication.SignOut();
            //InventorySession.LoginUser = null;
            Session["LoginUser"] = null;
            Session["LoginUserType"] = null;
            Session["MenuList"] = null;
            return View(model);
        }

        [HttpPost]
        public ActionResult ValidateUser(LoginModel model)
        {
            ResponseDetail objResponseModel = new ResponseDetail();
            objResponseModel.ResponseStatus = "FAILED";
            objResponseModel.ResponseMessage = "Something went wrong!";
            if (model != null)
            {
                if ((!string.IsNullOrEmpty(model.UserName)) && (!string.IsNullOrEmpty(model.password)))
                {
                    var IsValidate = false;
                    User Objresponse = objLoginManager.ValidateUser(model);
                    if(Objresponse.UserId != -1)
                    {
                        if (Objresponse.PartyCode != "WR")
                        {
                            Session["MenuList"] = null;
                            Session["LoginUser"] = null;
                            objResponseModel.ResponseStatus = "FAILED";
                            objResponseModel.ResponseMessage = "You are not authorized to login this portal.";
                            return Json(objResponseModel, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (Objresponse.UserId == -1)
                    {
                        Objresponse = objLoginManager.ValidateMobileUser(model);
                        if (Objresponse != null && Objresponse.UserId != -1)
                        {
                            Session["LoginUserType"] = "mobileshoppe";
                            IsValidate = true;
                        }
                    }
                    else
                    {
                        Session["LoginUserType"] = "shoppe";
                        IsValidate = true;
                    }

                    if (IsValidate)
                    {
                        objResponseModel.ResponseStatus = "OK";
                        objResponseModel.ResponseMessage = "Success!";
                        Session["LoginUser"] = Objresponse;
                        Session["MenuList"] = Objresponse.objMenuList;
                        FormsAuthentication.SetAuthCookie(model.UserName, false);
                    }
                    else
                    {
                        Session["MenuList"] = null;
                        Session["LoginUser"] = null;
                        objResponseModel.ResponseStatus = "FAILED";
                        objResponseModel.ResponseMessage = Objresponse.UserName;
                    }                    
                    return Json(objResponseModel, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(objResponseModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangePassword()
        {
            ChangePassword objModel = new ChangePassword();
            
            return PartialView(objModel);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = objLoginManager.ChangePassword(objModel);
            return Json(objResponse,JsonRequestBehavior.AllowGet);
        }

        public ActionResult SessionExpire()
        {
            FormsAuthentication.SignOut();
            return View();
        }
        
    }
}