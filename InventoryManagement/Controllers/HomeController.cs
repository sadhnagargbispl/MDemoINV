
using InventoryManagement.App_Start;
using InventoryManagement.Business;
using InventoryManagement.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        DashboardManager objDashboardManager = new DashboardManager();
        [SessionExpire]
        public ActionResult Dashboard()
        {
            string LoginPartyCode = "";
            int userId = 0;
            decimal FWalletBalance = 0;
            if (Session["LoginUser"] != null)
            {
                if (Session["LoginUserType"] as string == "shoppe")
                {
                    LoginPartyCode = (Session["LoginUser"] as User).PartyCode;
                    FWalletBalance = objDashboardManager.GetFWalletBalance(LoginPartyCode,"R");
                }
                else
                {
                    LoginPartyCode = (Session["LoginUser"] as User).UserName;
                    FWalletBalance = objDashboardManager.GetFWalletBalance(LoginPartyCode,"M");
                }
                userId = (Session["LoginUser"] as User).UserId;
            }
            
            
            ViewBag.WalletBalance = FWalletBalance;

            List <KitDetail> objList = objDashboardManager.GetDashboardSummary(LoginPartyCode, userId);

            return View(objList);
        }
    }
}