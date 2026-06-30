using InventoryManagement.API.Controllers;
using InventoryManagement.Entity.Common;
using InventoryManagement.DataAccess.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.DataAccess
{
    public class DashboardRepository: IDashboardRepository
    {
        DashboardAPIController objDashboardApi = new DashboardAPIController();
        public decimal GetFWalletBalance(string LoginPartyCode,string Vtype)
        {
            return (objDashboardApi.GetFWalletBalance(LoginPartyCode,Vtype));
        }
        public List<KitDetail> GetDashboardSummary(string LoginPartyCode,int? userId)
        {
            return (objDashboardApi.GetDashboardSummary(LoginPartyCode, userId));
        }
    }
}