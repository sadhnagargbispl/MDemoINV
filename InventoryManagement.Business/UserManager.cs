using InventoryManagement.Business.Contract;
using InventoryManagement.DataAccess;
using InventoryManagement.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Business
{
    public class UserManager: IUserManager
    {
        UserRepository objUserAPI = new UserRepository();
        public List<User> GetUserList()
        {
            return (objUserAPI.GetUserList());
        }
        public ResponseDetail SetUserRights(List<UserPermissionMasterModel> objPermissionList)
        {
            return (objUserAPI.SetUserRights(objPermissionList));
        }
        public ResponseDetail StopInvoicingSave(List<UserPermissionMasterModel> objPermissionList)
        {
            return (objUserAPI.StopInvoicingSave(objPermissionList));
        }
        
        public List<UserPermissionMasterModel> ListUserPermittedMenus(decimal UserId)
        {
            return (objUserAPI.ListUserPermittedMenus(UserId));
        }
        public List<User> GetAllUserList()
        {
            return (objUserAPI.GetAllUserList());
        }
        public User GetUser(int UserId)
        {
            return (objUserAPI.GetUser(UserId));
        }
        public decimal GetPartyGroupId(string PartyCode)
        {
            return (objUserAPI.GetPartyGroupId(PartyCode));
        }
        public ResponseDetail AddEditUserDetails(User objModel, User LoggedUser)
        {
            return (objUserAPI.AddEditUserDetails(objModel, LoggedUser));
        }
        public List<PartyModel> GetPartyListForUsers()
        {
            return (objUserAPI.GetPartyListForUsers());
        }
        public ResponseDetail IsDuplicateUserName(string IsActionType, string UserCode, string UserName)
        {
            return (objUserAPI.IsDuplicateUserName(IsActionType, UserCode, UserName));
        }
        
         public List<MenuMasterModel> GetAllMenuList()
        {
            return (objUserAPI.GetAllMenuList());
        }
        public List<UserPermissionMasterModel> GetPermittedUserList(decimal SMenuId)
        {           
            return (objUserAPI.GetPermittedUserList(SMenuId));
        }
        public string UserCanAccess(int UserID, string MenuFile)
        {
            return objUserAPI.UserCanAccess(UserID, MenuFile);
        }
    }
}