using InventoryManagement.API.Models;
using InventoryManagement.Entity.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InventoryManagement.API.Controllers
{
    public class LoginAPIController : ApiController
    {


        public User ValidateUser(LoginModel model)
        {
            User objResponse = new User();
            TransactionAPIController objTransacAPI = new TransactionAPIController();
            var config = new M_ConfigMaster();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    config = entity.M_ConfigMaster.FirstOrDefault();
                    objResponse = (from result in entity.Inv_M_UserMaster
                                   where result.ActiveStatus == "Y" && result.UserName == model.UserName && result.Passw == model.password
                                   join ledger in entity.M_LedgerMaster on result.BranchCode equals ledger.PartyCode
                                   join groupid in entity.M_GroupMaster on ledger.GroupId equals groupid.GroupId
                                   select new User
                                   {
                                       UserId = (int)result.UserId,
                                       UserName = result.UserName,
                                       Password = result.Passw,
                                       BranchCode = result.BranchCode,
                                       PartyCode = ledger.PartyCode,
                                       PartyName = ledger.PartyName,
                                       GroupId = (int)ledger.GroupId,
                                       FCode = ledger.PartyCode,
                                       StateCode = (int)ledger.StateCode,
                                       IsAdmin = result.IsAdmin,
                                       ParentPartyCode = ledger.ParentPartyCode,
                                       IsApprove=ledger.ISApprove

                                   }

                                 ).FirstOrDefault();


                    if (objResponse == null)
                    {
                        objResponse = new User();
                        objResponse.UserId = -1;
                        objResponse.UserName = "Incorrect UserName or Password.";
                        
                    }

                    //if((!objResponse.UserName.Equals(model.UserName) || (!objResponse.Password.Equals(model.password)))){
                    //    objResponse = null;
                    //}


                    //dynamic menus
                    objResponse.objMenuList = new List<MenuMasterModel>();
                    if (objResponse != null && objResponse.UserId != -1 && objResponse.IsAdmin == "N")
                    {
                        string wBalance = (new TransactionAPIController().GetPartyWalletBalance(objResponse.BranchCode, "R")).ToString();
                        objResponse.WBalance = wBalance;
                        List<decimal> PermittedMenuId = (from r in entity.Web_M_UserPermissionMaster where r.GroupId == objResponse.UserId select r.MenuId).ToList();
                        objResponse.objMenuList = (from r in entity.Web_M_MenuMaster
                                                   where r.ActiveStatus == "Y" && PermittedMenuId.Contains(r.MenuId)
                                                   select new MenuMasterModel
                                                   {
                                                       MenuId = r.MenuId,
                                                       MenuName = r.MenuName,
                                                       ParentId = r.ParentId,
                                                       ActiveStatus = r.ActiveStatus,
                                                       Hierarchy = r.Hierar,
                                                       OnSelect = r.OnSelect,
                                                       Sequence = r.Sequence,
                                                       ChildSequence = r.ChildSequence
                                                   }).OrderBy(m => m.Sequence).ToList();
                    }
                    else
                    {

                        objResponse.objMenuList = (from r in entity.Web_M_MenuMaster
                                                   where r.ActiveStatus == "Y"

                                                   select new MenuMasterModel
                                                   {
                                                       MenuId = r.MenuId,
                                                       MenuName = r.MenuName,
                                                       ParentId = r.ParentId,
                                                       ActiveStatus = r.ActiveStatus,
                                                       Hierarchy = r.Hierar,
                                                       OnSelect = r.OnSelect,
                                                       Sequence = r.Sequence,
                                                       ChildSequence = r.ChildSequence
                                                   }).OrderBy(m => m.Sequence).ToList();
                    }

                }
            }
            catch (Exception ex)
            {
                objResponse.UserId = -1;
                objResponse.UserName = ex.Message;
            }
            //if (string.IsNullOrEmpty(objResponse.UserName))
            //{
            //    objResponse = null;
            //}
            if (objResponse.UserId != -1)
            {
                //Cmnted on 04Mar19
                //if (objTransacAPI.IsFPVInvoiceValid()==false && objResponse.objMenuList.Count>0)
                //{
                //    int indexOfFPVInv = objResponse.objMenuList.FindIndex(m => m.OnSelect.ToLower() == "Transaction/FPVInvoice".ToLower());
                //    if (indexOfFPVInv > -1)
                //        objResponse.objMenuList.RemoveAt(indexOfFPVInv);
                //}

                objResponse.IsSoldByHo = false;
                if (objResponse.GroupId == 0)
                {
                    objResponse.IsSoldByHo = true;
                }
                if (objResponse.IsSoldByHo)
                {
                    //int indexOfOrderCreationMenu = objResponse.objMenuList.FindIndex(m => m.MenuName == "Order Creation");
                    //if (indexOfOrderCreationMenu > -1)
                    //{
                    //    objResponse.objMenuList.RemoveAt(indexOfOrderCreationMenu);
                    //}
                    //indexOfOrderCreationMenu = objResponse.objMenuList.FindIndex(m => m.OnSelect == "Transaction/DistributorBill");
                    //if (indexOfOrderCreationMenu > -1)
                    //{
                    //    objResponse.objMenuList.RemoveAt(indexOfOrderCreationMenu);
                    //}
                }

                else
                {
                    int indexOfOrderCreationMenu = objResponse.objMenuList.FindIndex(m => m.MenuName == "Purchase Reports");
                    if (indexOfOrderCreationMenu > -1)
                    {
                        decimal ParentId = objResponse.objMenuList.Where(m => m.MenuName == "Purchase Reports").Select(m => m.MenuId).FirstOrDefault();
                        objResponse.objMenuList.RemoveAt(indexOfOrderCreationMenu);
                        List<MenuMasterModel> PurchaseReportsMenus = objResponse.objMenuList.Where(m => m.ParentId == ParentId).ToList();
                        foreach (MenuMasterModel obj in PurchaseReportsMenus)
                        {
                            indexOfOrderCreationMenu = objResponse.objMenuList.FindIndex(m => m.MenuId == obj.MenuId);
                            objResponse.objMenuList.RemoveAt(indexOfOrderCreationMenu);
                        }
                    }
                }

                if (config != null)
                {
                    if (config.CanIDBeUpgraded == "N")
                    {
                        int indexOfMenu = objResponse.objMenuList.FindIndex(m => m.MenuName.ToLower() == "combination bill");
                        if (indexOfMenu > -1)
                        {
                            objResponse.objMenuList.RemoveAt(indexOfMenu);
                        }
                    }
                }

            }

            return objResponse;
        }


        public User ValidateMobileShoppe(LoginModel model)
        {
            User objResponse = new User();
            TransactionAPIController objTransacAPI = new TransactionAPIController();
            try
            {
                using (var entity = new InventoryEntities())
                {

                    string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                    SqlConnection SC = new SqlConnection(AppConnectionString);
                    string db = System.Configuration.ConfigurationManager.AppSettings["Database"];
                    string dbInv = System.Configuration.ConfigurationManager.AppSettings["INVDatabase"];
                    string query = "select a.*,b.IdNo,b.Mobl,b.ActiveStatus,b.IsBlock,b.FormNo,b.MemFirstName+' '+ b.MemLastName as Name,b.StateCode as StateCode,b.Passw from " + dbInv + "..ShoppeAssignment a left join " + db + "..M_MemberMaster b on a.PCId = b.IDno where a.PCId = '" + model.UserName + "'";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = SC;
                    SC.Close();
                    SC.Open();
                    var ParentParty = string.Empty;
                    var isValid = true;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            objResponse.PartyCode = reader["FCode"] != null ? Convert.ToString(reader["FCode"]) : "";
                            objResponse.UserId = reader["AId"] != null ? Convert.ToInt16(reader["AId"]) : 0;
                            objResponse.UserName = reader["IdNo"] != null ? Convert.ToString(reader["IdNo"]) : "";
                            objResponse.Name = reader["Name"] != null ? Convert.ToString(reader["Name"]) : "";
                            objResponse.Password = reader["Passw"] != null ? Convert.ToString(reader["Passw"]) : "";
                            objResponse.ActiveStatus = reader["ActiveStatus"] != null ? Convert.ToString(reader["ActiveStatus"]) : "";
                            objResponse.IsAdmin = reader["IsBlock"] != null ? Convert.ToString(reader["IsBlock"]) : "";

                        }
                    }
                    SC.Close();

                    if (!string.IsNullOrEmpty(objResponse.PartyCode))
                    {
                        if (objResponse.ActiveStatus == "Y" && objResponse.IsAdmin == "N" && objResponse.UserName == model.UserName && objResponse.Password == model.password)
                        {
                            var party = (from r in entity.M_LedgerMaster where r.PartyCode == objResponse.PartyCode select r).FirstOrDefault();
                            objResponse.FCode = objResponse.PartyCode;
                            objResponse.PartyName = party.PartyName;
                            objResponse.BranchCode = objResponse.PartyCode;
                            objResponse.GroupId = (int)party.GroupId;
                            objResponse.StateCode = (int)party.StateCode;
                            objResponse.IsAdmin = "N";
                            objResponse.ParentPartyCode = party.ParentPartyCode;

                        }
                        else
                        {
                            objResponse.UserId = -1;
                            isValid = false;
                            objResponse.UserName = "Incorrect UserName or Password.";
                        }
                    }
                    else
                    {
                        objResponse.UserId = -1;
                        isValid = false;
                        objResponse.UserName = "Incorrect UserName or Password.";
                    }


                    if (isValid == true)
                    {
                        objResponse.objMenuList = new List<MenuMasterModel>();
                        string wBalance = (new TransactionAPIController().GetPartyWalletBalance(objResponse.UserName, "M")).ToString();
                        objResponse.WBalance = wBalance;
                        objResponse.objMenuList = (from r in entity.Mobile_Shoppe_MenuMaster
                                                   where r.ActiveStatus == "Y"
                                                   select new MenuMasterModel
                                                   {
                                                       MenuId = r.MenuId,
                                                       MenuName = r.MenuName,
                                                       ParentId = r.ParentId,
                                                       ActiveStatus = r.ActiveStatus,
                                                       Hierarchy = r.Hierar,
                                                       OnSelect = r.OnSelect,
                                                       Sequence = r.Sequence,
                                                       ChildSequence = r.ChildSequence
                                                   }).OrderBy(m => m.Sequence).ToList();
                    }


                }
            }
            catch (Exception ex)
            {
                objResponse.UserId = -1;
                objResponse.UserName = ex.Message;
            }
            if (objResponse != null)
            {
                objResponse.IsSoldByHo = false;
                if (objResponse.GroupId == 0)
                {
                    objResponse.IsSoldByHo = true;
                }

            }

            return objResponse;
        }



        public ResponseDetail ChangePassword(ChangePassword model)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    if (model != null)
                    {
                        Inv_M_UserMaster objDtUserMaser = new Inv_M_UserMaster();
                        objDtUserMaser = (from r in entity.Inv_M_UserMaster
                                          where r.Passw == model.CurrentPassword && r.UserName == model.UserName
                                          select r
                                   ).FirstOrDefault();
                        if (objDtUserMaser != null)
                        {
                            objDtUserMaser.Passw = model.NewPassword;
                            int i = entity.SaveChanges();
                            if (i > 0)
                            {
                                objResponse.ResponseStatus = "OK";
                                objResponse.ResponseMessage = "Password successfully changed!";
                            }
                            else
                            {
                                objResponse.ResponseStatus = "Failed";
                                objResponse.ResponseMessage = "Something went wrong!";
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return objResponse;
        }
    }
}
