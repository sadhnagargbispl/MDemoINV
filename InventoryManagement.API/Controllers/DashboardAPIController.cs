using System;

using InventoryManagement.Entity.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InventoryManagement.API.Models;

namespace InventoryManagement.API.Controllers
{
    public class DashboardAPIController : ApiController
    {
        public decimal GetFWalletBalance(string LoginPartyCode,string VType)
        {
            decimal FWalletBalance = 0;
            try
            {
                string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(InvConnectionString);
                SqlCommand cmd = new SqlCommand();
                string query = "Select PartyCode,SUM(CrAmt),SUM(DrAmt),SUM(CrAmt)-SUM(DrAmt) as Balance FROM  (Select Crto as PartyCode,ISNULL(SUM(Amount), 0) as CrAmt,0 as DrAmt FROM TrnVoucher where Crto = '" + LoginPartyCode + "' and Vtype = '"+ VType + "' GROUP BY CRto UNION ALL Select Drto as PartyCode,0,ISNULL(SUM(Amount), 0) as CrAmt FROM TrnVoucher where Drto = '" + LoginPartyCode + "' and Vtype = '"+ VType + "' GROUP BY DRto) a WHERE  GROUP BY PartyCode ";
                cmd.CommandText = query;
                //cmd.Parameters.AddWithValue("@IdNo", IdNo);
                cmd.Connection = SC;
                SC.Close();
                SC.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        FWalletBalance = decimal.Parse(reader["Balance"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return FWalletBalance;
        }

        public List<KitDetail> GetDashboardSummary(string LoginPartyCode, int? userId)
        {
            List<KitDetail> objList = new List<KitDetail>();
            var dashboardPermission = new Web_M_UserPermissionMaster();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    if (userId != null)
                    {
                        dashboardPermission = (from r in entity.Web_M_UserPermissionMaster where r.MenuId == 73 && r.GroupId == userId select r).FirstOrDefault();
                    }
                }
                if (dashboardPermission != null)
                {
                    string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
                    SqlConnection SC = new SqlConnection(AppConnectionString);
                    SqlCommand cmd = new SqlCommand();
                    string WRCode = System.Configuration.ConfigurationManager.AppSettings["WRPartyCode"];
                    string WRColms = "";
                    string TblName = "DashboardSummary('" + LoginPartyCode + "')";
                    if (LoginPartyCode == WRCode)
                    {
                        WRColms = ", WRVal, WRQty, FRVal, FRQty";
                        TblName = "V#WRDashboardSummary";
                    }
                    string query = "Select FldValue, ColmValue FROM( " +
     " SELECT   ColumnName, ColmValue FROM " + TblName + " " +
     " UNPIVOT(ColmValue FOR ColumnName IN( " +
     " TotalSale, TotalFV, TotalRV, TotalFVAmt, TotalRVAmt, LastMnthSale, " +
     " LastMnthFV, LastMnthRV, LastMnthFVAmt, LastMnthRVAmt, " +
     " LastMnthTDAmt, LastMnthTDFV, LastMnthTDRV, LastMnthTDFVAmt, LastMnthTDRVAmt," +
     " MnthSale, MnthFV, MnthRV, MnthFVAmt, MnthRVAmt, " +
     " TodaySale, TodayFV, TodayRV, TodayFVAmt, TodayRVAmt, " +
     " TodayBillCnt, TodayFVBillCnt, TodayRVBillCnt,LastMnthFVBillCnt,LastMnthRVBillCnt, " +
     " StockVal, StockQty " + WRColms + " )" +
     ") unpvt) a,DashboardSummaryColumn b WHERE a.ColumnName = b.FldName ORDER BY b.AID";
                    cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = SC;
                    SC.Close();
                    SC.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            KitDetail obj = new KitDetail();
                            obj.KitName = reader["FldValue"] != null ? reader["FldValue"].ToString() : "";
                            obj.KitId = Convert.ToDecimal(reader["ColmValue"]);
                            objList.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }        
            return objList;
        

        }
    }
}
