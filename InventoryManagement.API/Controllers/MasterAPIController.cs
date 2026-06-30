using InventoryManagement.API.Models;
using InventoryManagement.Entity.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.API.Controllers
{
    public class MasterAPIController : Controller
    {
        // GET: MasterAPI
        public FPVMaster GetFPVDetail()
        {
            FPVMaster objMaster = new FPVMaster();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objMaster = (from t in entity.FPVConfigs
                                 where t.RowStatus == "Y"
                                 select new FPVMaster
                                 {
                                     AID = t.AID,
                                     ActiveStatus = t.ActiveStatus,
                                     ExpiredinDays = t.ExpiredinDays,
                                     RowStatus = t.RowStatus,
                                     FPV = t.FPV,
                                     BVValue = t.BVValue,
                                     IsMultiple = t.IsMultiple,
                                     OnBillType = t.OnBillType,
                                 }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return objMaster;
        }

        public ResponseDetail SaveFPVDetails(FPVMaster objDetails)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                if (objDetails != null)
                {
                    int BunchID = objDetails.BunchID;
                    using (var entity = new InventoryEntities())
                    {

                        List<FPVConfig> list = (from r in entity.FPVConfigs where r.RowStatus == "Y" select r).ToList();
                        foreach (var record in list)
                        {
                            record.RowStatus = "N";
                        }

                        List<FPVProductConfig> listProd = (from r in entity.FPVProductConfigs where r.RowStatus == "Y" && r.BunchId == BunchID select r).ToList();
                        foreach (var record in listProd)
                        {
                            record.RowStatus = "N";
                        }

                        FPVConfig newDetail = new FPVConfig();
                        newDetail.RecTimeStamp = DateTime.Now;
                        newDetail.ActiveStatus = objDetails.ActiveStatus;
                        newDetail.BVValue = objDetails.BVValue;
                        newDetail.FPV = objDetails.FPV;
                        newDetail.IsMultiple = objDetails.IsMultiple;
                        newDetail.RowStatus = "Y";
                        newDetail.ExpiredinDays = objDetails.ExpiredinDays;
                        newDetail.OnBillType = objDetails.OnBillType;
                        entity.FPVConfigs.Add(newDetail);

                        foreach (var product in objDetails.products)
                        {
                            FPVProductConfig newprod = new FPVProductConfig();
                            newprod.ActiveStatus = "Y";
                            newprod.RowStatus = "Y";
                            newprod.RecTimeStamp = DateTime.Now;
                            newprod.ProductName = product.ProdName;
                            newprod.ProductId = Convert.ToDecimal(product.ProdID);
                            newprod.BunchId = BunchID;
                            entity.FPVProductConfigs.Add(newprod);
                        }

                        int i = entity.SaveChanges();
                        if (i > 0)
                        {
                            objResponse.ResponseMessage = "Saved Successfully!";
                            objResponse.ResponseStatus = "OK";
                        }
                        else
                        {
                            objResponse.ResponseMessage = "Something Went Wrong..";
                            objResponse.ResponseStatus = "FAILED";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseMessage = "Something Went Wrong..";
                objResponse.ResponseStatus = "FAILED";
            }
            return objResponse;
        }

        public List<OfferProducts> getFPVProducts(int BunchID)
        {

            List<OfferProducts> objMaster = new List<OfferProducts>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objMaster = (from t in entity.FPVProductConfigs
                                 where t.RowStatus == "Y" && t.BunchId == BunchID
                                 select new OfferProducts
                                 {
                                     ActiveStatus = t.ActiveStatus,
                                     ProdID = t.ProductId.ToString(),
                                     ProdName = t.ProductName
                                 }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objMaster;
        }

        public List<IdAssignment> GetAllPCID(string PartyCode)
        {
            List<IdAssignment> objList = new List<IdAssignment>();
            try
            {
                string AppConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                SqlConnection SC = new SqlConnection(AppConnectionString);
                string dbInv = System.Configuration.ConfigurationManager.AppSettings["INVDatabase"];
                string db = System.Configuration.ConfigurationManager.AppSettings["Database"];
                string query = "select a.PCId,a.Fcode,c.PartyName,b.MemFirstName+' '+ b.MemLastName as Name,b.Passw";
                query += " from " + dbInv + "..ShoppeAssignment a";
                query += " left join " + db + "..M_MemberMaster b on a.PCId = b.IdNo";
                query += " left join "+dbInv+"..M_LedgerMaster c on a.Fcode = c.PartyCode";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = SC;
                SC.Close();
                SC.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var objId = new IdAssignment();

                        objId.IdNo = reader["PCId"] != null ? reader["PCId"].ToString() : "";
                        objId.Name = reader["Name"] != null ? reader["Name"].ToString() : "";
                        objId.FormNo = "";
                        objId.PartyCode = reader["Fcode"] != null ? reader["Fcode"].ToString() : "";
                        objId.PartyName = reader["PartyName"] != null ? reader["PartyName"].ToString() : "";
                        objId.Password = reader["Passw"] != null ? reader["Passw"].ToString() : "";
                        objList.Add(objId);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return objList;
        }

        public ResponseDetail AssignIDToShoppe(IdAssignment ObjAssignList)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    var IsExist = (from r in entity.ShoppeAssignments where r.PCId == ObjAssignList.IdNo select r).FirstOrDefault();
                    if (IsExist != null)
                    {
                        objResponse.ResponseMessage = "Already Exist.";
                        objResponse.ResponseStatus = "Fail";
                    }
                    else
                    {
                        ShoppeAssignment obj = new ShoppeAssignment();
                        obj.FCode = ObjAssignList.PartyCode;
                        obj.PCId = ObjAssignList.IdNo;
                        obj.PCName = ObjAssignList.Name;
                        obj.FormNo = ObjAssignList.FormNo;
                        obj.RecordedTime = DateTime.Now;
                        entity.ShoppeAssignments.Add(obj);
                        entity.SaveChanges();
                        objResponse.ResponseMessage = "Assigned Successfully.";
                        objResponse.ResponseStatus = "Ok";
                    }

                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseMessage = "Something Went Wrong..";
                objResponse.ResponseStatus = "FAILED";
            }
            return objResponse;
        }


        public List<CourierchargeByWeight> GetChargeList()
        {
            List<CourierchargeByWeight> objChargeList = new List<CourierchargeByWeight>();
            try
            {
                using (var entity = new InventoryEntities())
                {
                    objChargeList = (from r in entity.CourierCharges
                                select new CourierchargeByWeight
                                {
                                    Id = r.AId,
                                    Weight = r.Weight,
                                    Amount = r.Amount
                                }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return objChargeList;
        }

        public ResponseDetail SaveCourierCharge(CourierchargeByWeight model)
        {
            {
                ResponseDetail objResponse = new ResponseDetail();
                objResponse.ResponseStatus = "FAIL";
                objResponse.ResponseMessage = "Something went wrong!";
                try
                {
                    using (var entity = new InventoryEntities())
                    {
                        CourierCharge objDTCategory = new CourierCharge();
                        objDTCategory = (from r in entity.CourierCharges where r.AId == model.Id select r).FirstOrDefault();
                        if (objDTCategory == null)
                        {
                            objDTCategory = new CourierCharge();
                        }

                        if (model.isAdd != "Delete")
                        {
                            objDTCategory.Weight = model.Weight;
                            objDTCategory.Amount = model.Amount;
                            if (model.isAdd == "Add")
                            {
                                objDTCategory.RecordedTime = DateTime.Now;
                                entity.CourierCharges.Add(objDTCategory);
                            }
                        }
                        else
                        {
                            if (objDTCategory != null)
                            {
                                entity.CourierCharges.Remove(objDTCategory);
                            }
                        }
                        try
                        {
                            int isSaved = entity.SaveChanges();
                            if (isSaved > 0)
                            {
                                if (model.isAdd == "Add")
                                {
                                    objResponse.ResponseStatus = "OK";
                                    objResponse.ResponseMessage = "Saved Successfully!";
                                }
                                else if (model.isAdd == "Edit")
                                {
                                    objResponse.ResponseStatus = "OK";
                                    objResponse.ResponseMessage = "Updated Successfully!";

                                }
                                else
                                {
                                    objResponse.ResponseStatus = "OK";
                                    objResponse.ResponseMessage = "Deleted Successfully!";

                                }
                            }
                            else
                            {
                                objResponse.ResponseStatus = "Failed";
                                objResponse.ResponseMessage = "Something went wrong!";

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return objResponse;
            }
        }

        public ResponseDetail getIdUpgradeDetails()
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseStatus = "FAIL";
            objResponse.ResponseMessage = "Something went wrong!";
            try
            {
                using (var entity = new InventoryEntities())
                {
                    var CanIDBeUpgraded = entity.M_ConfigMaster.Select(r => r.CanIDBeUpgraded).FirstOrDefault();
                    objResponse.ResponseStatus = "OK";
                    objResponse.ResponseMessage = CanIDBeUpgraded;
                }
            }
            catch (Exception ex)
            {

            }
            return objResponse;            
        }

        public ResponseDetail UpdateIDUpgradeDetails(IDUdgradeViewModel viewModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseStatus = "FAIL";
            objResponse.ResponseMessage = "Something went wrong!";
            try
            {
                using (var entity = new InventoryEntities())
                {
                    var Config = entity.M_ConfigMaster.FirstOrDefault();
                    Config.CanIDBeUpgraded = viewModel.CanIdBeUpgraded;
                    entity.SaveChanges();
                    objResponse.ResponseStatus = "OK";
                    objResponse.ResponseMessage = "Updated Successfully";

                }
            }
            catch (Exception ex)
            {

            }
            return objResponse;

        }
    }
}