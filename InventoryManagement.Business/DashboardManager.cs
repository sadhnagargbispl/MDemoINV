using InventoryManagement.DataAccess;
using InventoryManagement.Entity.Common;
using InventoryManagement.Business.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Business
{
    public class DashboardManager: IDashboardManager
    {
        DashboardRepository objDashboardRepo = new DashboardRepository();
        public decimal GetFWalletBalance(string LoginPartyCode,string Vtype)
        {
            return (objDashboardRepo.GetFWalletBalance(LoginPartyCode,Vtype));
        }
        public List<KitDetail> GetDashboardSummary(string LoginPartyCode, int? userId)
        {
            return (objDashboardRepo.GetDashboardSummary(LoginPartyCode, userId));
        }
    }
}