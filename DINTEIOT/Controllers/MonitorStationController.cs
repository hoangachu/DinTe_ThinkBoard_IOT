using DINTEIOT.Helpers;
using DINTEIOT.Models;
using DINTEIOT.Models.MonitorStation;
using DINTEIOT.Models.StationData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace DINTEIOT.Controllers
{
    public interface IMonitorStationController
    {
        public List<MonitorStation> GetAllListMonitorStation();
        public List<MonitorStation> GetListMonitorStation(MonitorStationFilter MonitorStationFilter);
        public MonitorStation GetMonitorStationByID(int id = 0);
    }
    public class MonitorStationController : BaseController, IMonitorStationController  // Trạm quan trắc
    {
        private IOrganController _iorganController;
        private IStationDataController _iStationDataController;
        public MonitorStationController(IOrganController IOrganController, IStationDataController IStationDataController)
        {
            _iorganController = IOrganController;
            _iStationDataController = IStationDataController;
        }
        public IActionResult Index()
        {
            MonitorStationFilter MonitorStationFilter = new MonitorStationFilter();
            if (TempData["OptionFilter"] != null) { MonitorStationFilter = JsonConvert.DeserializeObject<MonitorStationFilter>((string)TempData["OptionFilter"]); TempData.Keep(); }
            ViewBag.CurrentPageName = ScreenName.MonitorStation;
            ViewBag.CoQuan = _iorganController.GetSelectTreeViewNode();
            ViewBag.LoaiDuLieu = _iStationDataController.GetAllListStationData();
            return View(GetListMonitorStation(MonitorStationFilter));
        }
        //lấy danh sách ngưỡng cảnh báo có lọc
        public List<MonitorStation> GetListMonitorStation(MonitorStationFilter MonitorStationFilter)
        {
            List<MonitorStation> listMonitorStation = new List<MonitorStation>();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MonitorStation_GetListMonitorStation_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@pagenumber", SqlDbType.Int).Value = MonitorStationFilter.pagenumber;
                            cmd.Parameters.Add("@pagesize", SqlDbType.Int).Value = MonitorStationFilter.pagesize;
                            cmd.Parameters.Add("@txtsearch", SqlDbType.NVarChar).Value = MonitorStationFilter.txtsearch == null ? DBNull.Value : MonitorStationFilter.txtsearch;
                            cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = MonitorStationFilter.startdate == null ? DBNull.Value : MonitorStationFilter.startdate;
                            cmd.Parameters.Add("@enddate", SqlDbType.DateTime).Value = MonitorStationFilter.enddate == null ? DBNull.Value : MonitorStationFilter.enddate;
                            cmd.Parameters.Add("@organID", SqlDbType.Int).Value = MonitorStationFilter.organID == 0 ? DBNull.Value : MonitorStationFilter.organID;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                MonitorStation MonitorStation = new MonitorStation();
                                MonitorStation.monitorStationID = dr.IsDBNull("MonitorStationId") == true ? 0 : (int)dr["MonitorStationId"];
                                MonitorStation.monitorStationName = dr.IsDBNull("MonitorStationName") == true ? null : (string)dr["MonitorStationName"];
                                MonitorStation.monitorStationCode = dr.IsDBNull("monitorStationCode") == true ? null : (string)dr["monitorStationCode"];
                                MonitorStation.latitude = dr.IsDBNull("latitude") == true ? 0 : (float)(double)dr["latitude"];
                                MonitorStation.longitude = dr.IsDBNull("longitude") == true ? 0 : (float)(double)dr["longitude"];
                                MonitorStation.siteAddress = dr.IsDBNull("siteAddress") == true ? null : (string)dr["siteAddress"];
                                MonitorStation.organName = dr.IsDBNull("organName") == true ? null : (string)dr["organName"];
                                MonitorStation.address = dr.IsDBNull("address") == true ? null : (string)dr["address"];
                                MonitorStation.description = dr.IsDBNull("description") == true ? null : (string)dr["description"];
                                MonitorStation.totalrecord = dr.IsDBNull("totalrecord") == true ? 0 : (int)dr["totalrecord"];
                                listMonitorStation.Add(MonitorStation);
                            }
                            cmd.Dispose();
                            dr.Close();
                            ViewBag.TotalRecord = listMonitorStation.Count() > 0 ? listMonitorStation.First().totalrecord : 0;
                            ViewBag.TotalPage = (ViewBag.TotalRecord / MonitorStationFilter.pagesize) + 1;
                            ViewBag.PageNumber = MonitorStationFilter.pagenumber > 0 ? MonitorStationFilter.pagenumber : 1;
                            ViewBag.PageFirst = MonitorStationFilter.pagefirst > 0 ? MonitorStationFilter.pagefirst : 1;
                        }
                        catch (Exception e)
                        {
                            //throw e;
                        }

                        con.Close();
                    }
                }
            }
            return listMonitorStation;
        }
        //lấy danh sách ngưỡng cảnh báo không lọc
        public List<MonitorStation> GetAllListMonitorStation()
        {
            List<MonitorStation> listMonitorStation = new List<MonitorStation>();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MonitorStation_GetAllListMonitorStation_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                MonitorStation MonitorStation = new MonitorStation();
                                MonitorStation.monitorStationID = dr.IsDBNull("MonitorStationId") == true ? 0 : (int)dr["MonitorStationId"];
                                MonitorStation.monitorStationName = dr.IsDBNull("MonitorStationName") == true ? null : (string)dr["MonitorStationName"];
                                MonitorStation.monitorStationCode = dr.IsDBNull("monitorStationCode") == true ? null : (string)dr["monitorStationCode"];
                                MonitorStation.latitude = dr.IsDBNull("latitude") == true ? 0 : (float)(double)dr["latitude"];
                                MonitorStation.longitude = dr.IsDBNull("longitude") == true ? 0 : (float)(double)dr["longitude"];
                                MonitorStation.siteAddress = dr.IsDBNull("siteAddress") == true ? null : (string)dr["siteAddress"];
                                //MonitorStation.organName = dr.IsDBNull("organName") == true ? null : (string)dr["organName"];
                                MonitorStation.address = dr.IsDBNull("address") == true ? null : (string)dr["address"];
                                MonitorStation.description = dr.IsDBNull("description") == true ? null : (string)dr["description"];
                                MonitorStation.GuiID = dr.IsDBNull("GuiID") == true ? null : (string)dr["GuiID"];
                                MonitorStation.type = dr.IsDBNull("type") == true ? 1 : (int)dr["type"];
                                listMonitorStation.Add(MonitorStation);
                            }
                            cmd.Dispose();
                            dr.Close();
                        }
                        catch (Exception e)
                        {
                            //throw e;
                        }

                        con.Close();
                    }
                }
            }
            return listMonitorStation;
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.THEM, WebApi.Helpers.Function.ORGAN)]
        public IActionResult insertpre() //check quyền trc khi thêm mới
        {
            return Redirect("/MonitorStation/GetMonitorStationByIDPre");
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.SUA, WebApi.Helpers.Function.ORGAN)]
        public IActionResult updatepre(int id = 0) //check quyền trc khi cập nhật
        {
            return Redirect("/MonitorStation/GetMonitorStationByIDPre?id=" + id);
        }
        public IActionResult GetMonitorStationByIDPre(int id = 0)
        {
            var MonitorStation = GetMonitorStationByID(id);
            if (MonitorStation.monitorStationID < 0)
            {
                return Ok(new { status = (int)ExitCodes.Error, data = "", message = "Lỗi!" });
            }

            return Ok(new { status = (int)ExitCodes.Success, data = MonitorStation, message = "" });
        }
        public MonitorStation GetMonitorStationByID(int id = 0)
        {
            MonitorStation MonitorStation = new MonitorStation();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MonitorStation_GetMonitorStationByID_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                MonitorStation.monitorStationID = dr.IsDBNull("MonitorStationId") == true ? 0 : (int)dr["MonitorStationId"];
                                MonitorStation.monitorStationName = dr.IsDBNull("MonitorStationName") == true ? null : (string)dr["MonitorStationName"];
                                MonitorStation.monitorStationCode = dr.IsDBNull("monitorStationCode") == true ? null : (string)dr["monitorStationCode"];
                                MonitorStation.latitude = dr.IsDBNull("latitude") == true ? 0 : (float)(double)dr["latitude"];
                                MonitorStation.longitude = dr.IsDBNull("longitude") == true ? 0 : (float)(double)dr["longitude"];
                                MonitorStation.siteAddress = dr.IsDBNull("siteAddress") == true ? null : (string)dr["siteAddress"];
                                MonitorStation.address = dr.IsDBNull("address") == true ? null : (string)dr["address"];
                                MonitorStation.description = dr.IsDBNull("description") == true ? null : (string)dr["description"];
                                MonitorStation.organName = dr.IsDBNull("organName") == true ? null : (string)dr["organName"];
                                MonitorStation.organID = dr.IsDBNull("organID") == true ? 0 : (int)dr["organID"];
                                MonitorStation.ListStationData = _iStationDataController.GetStationDataByMonitorStationID(MonitorStation.monitorStationID);
                                break;
                            }
                            cmd.Dispose();
                            dr.Close();
                        }
                        catch (Exception e)
                        {
                            //throw e;
                        }

                        con.Close();
                    }
                }
            }
            return MonitorStation;
        }


        [HttpPost]
        public IActionResult Insert(MonitorStation MonitorStation, string[] listStationDataId)  // Thêm mới
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("Insert_MonitorStation_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@MonitorStationName          ", SqlDbType.NVarChar).Value = MonitorStation.monitorStationName == null ? DBNull.Value : MonitorStation.monitorStationName;
                        cmd.Parameters.Add("@monitorStationCode       ", SqlDbType.VarChar).Value = MonitorStation.monitorStationCode == null ? DBNull.Value : MonitorStation.monitorStationCode;
                        cmd.Parameters.Add("@organID                     ", SqlDbType.Int).Value = MonitorStation.organID == 0 ? DBNull.Value : MonitorStation.organID;
                        cmd.Parameters.Add("@longitude              ", SqlDbType.Float).Value = MonitorStation.longitude == 0 ? DBNull.Value : MonitorStation.longitude;
                        cmd.Parameters.Add("@latitude                ", SqlDbType.Float).Value = MonitorStation.latitude == 0 ? DBNull.Value : MonitorStation.latitude;
                        cmd.Parameters.Add("@address                  ", SqlDbType.VarChar).Value = MonitorStation.address == null ? DBNull.Value : MonitorStation.address;
                        cmd.Parameters.Add("@siteAddress                ", SqlDbType.VarChar).Value = MonitorStation.siteAddress == null ? DBNull.Value : MonitorStation.siteAddress;
                        cmd.Parameters.Add("@type                ", SqlDbType.Int).Value = MonitorStation.type == 0 ? DBNull.Value : MonitorStation.type;
                        cmd.Parameters.Add("@GuiID                ", SqlDbType.VarChar).Value = MonitorStation.GuiID == null ? DBNull.Value : MonitorStation.GuiID;
                        cmd.Parameters.Add("@description            ", SqlDbType.VarChar).Value = MonitorStation.description == null ? DBNull.Value : MonitorStation.description;
                        cmd.Parameters.Add("@monitorStationID", SqlDbType.Int).Direction = ParameterDirection.Output;
                        con.Open();
                        i = cmd.ExecuteNonQuery();
                        int monitorStationID = Convert.ToInt32(cmd.Parameters["@monitorStationID"].Value.ToString());
                        if (i > 0)
                        {
                            if (listStationDataId.Length > 0 && monitorStationID > 0)
                            {
                                List<int> listid = new List<int>();
                                listid = listStationDataId[0].Split(',').Select(x => Convert.ToInt32(x)).ToList();
                                InsertToMonitorStation_StationData(listid, monitorStationID);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Ok(new { status = (int)ExitCodes.Error, data = i, message = e.Message });
            }

            return Ok(new { status = (int)ExitCodes.Success, data = i, message = "Thêm mới thành công" });

        }
        public IActionResult Update(MonitorStation MonitorStation, string[] listStationDataId) // Sửa
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Update_MonitorStation_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@MonitorStationName          ", SqlDbType.NVarChar).Value = MonitorStation.monitorStationName == null ? DBNull.Value : MonitorStation.monitorStationName;
                        cmd.Parameters.Add("@monitorStationCode     ", SqlDbType.VarChar).Value = MonitorStation.monitorStationCode == null ? DBNull.Value : MonitorStation.monitorStationCode;
                        cmd.Parameters.Add("@organID     ", SqlDbType.Int).Value = MonitorStation.organID == 0 ? DBNull.Value : MonitorStation.organID;
                        cmd.Parameters.Add("@longitude              ", SqlDbType.Float).Value = MonitorStation.longitude == 0 ? DBNull.Value : MonitorStation.longitude;
                        cmd.Parameters.Add("@latitude       ", SqlDbType.Float).Value = MonitorStation.latitude == 0 ? DBNull.Value : MonitorStation.latitude;
                        cmd.Parameters.Add("@address     ", SqlDbType.VarChar).Value = MonitorStation.address == null ? DBNull.Value : MonitorStation.address;
                        cmd.Parameters.Add("@siteAddress     ", SqlDbType.VarChar).Value = MonitorStation.siteAddress == null ? DBNull.Value : MonitorStation.siteAddress;
                        cmd.Parameters.Add("@description     ", SqlDbType.VarChar).Value = MonitorStation.description == null ? DBNull.Value : MonitorStation.description;
                        cmd.Parameters.Add("@monitorStationID            ", SqlDbType.Int).Value = MonitorStation.monitorStationID == 0 ? DBNull.Value : MonitorStation.monitorStationID;
                        con.Open();
                        i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            if (listStationDataId.Length > 0 && MonitorStation.monitorStationID > 0)
                            {
                                List<int> listid = new List<int>();
                                listid = listStationDataId[0].Split(',').Select(x => Convert.ToInt32(x)).ToList();
                                InsertToMonitorStation_StationData(listid, MonitorStation.monitorStationID);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Ok(new { status = (int)ExitCodes.Error, data = i, message = e.Message });
            }
            return Ok(new { status = (int)ExitCodes.Success, data = i, message = "Cập nhật thành công" });
        }

        [HttpPost]
        public void GetListAfterFilter(MonitorStationFilter MonitorStationFilter)  //hàm gọi khi nhấn trong phần trang,lọc,...
        {
            TempData["OptionFilter"] = JsonConvert.SerializeObject(MonitorStationFilter);
        }
        [HttpPost]
        public IActionResult CheckMonitorStationByCode(string monitorStationCode = null, int id = 0) //hàm ktra mã trạm đã tồn tại chưa
        {
            bool index = false;
            if (!string.IsNullOrEmpty(monitorStationCode))
            {
                var MonitorStation = GetMonitorStationByCodeAndOtherID(monitorStationCode, id);
                if (MonitorStation.monitorStationID > 0)
                {
                    index = true;
                }
            }
            return Ok(new { data = index });
        }
        public MonitorStation GetMonitorStationByName(string MonitorStationName, int id = 0) // lấy ra trạm quan trắc cùng tên nhưng khác id
        {
            MonitorStation MonitorStation = new MonitorStation();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MonitorStation_GetMonitorStationByNameAndOtherID_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@MonitorStationName", SqlDbType.NVarChar).Value = MonitorStationName == null ? DBNull.Value : MonitorStationName;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {

                                MonitorStation.monitorStationID = dr.IsDBNull("MonitorStationId") == true ? 0 : (int)dr["MonitorStationId"];
                                MonitorStation.monitorStationName = dr.IsDBNull("MonitorStationName") == true ? null : (string)dr["MonitorStationName"];
                                MonitorStation.monitorStationCode = dr.IsDBNull("monitorStationCode") == true ? null : (string)dr["monitorStationCode"];
                                MonitorStation.latitude = dr.IsDBNull("MonitorStationValueTo") == true ? 0 : (float)(double)dr["latitude"];
                                MonitorStation.longitude = dr.IsDBNull("longitude") == true ? 0 : (float)(double)dr["longitude"];
                                MonitorStation.siteAddress = dr.IsDBNull("siteAddress") == true ? null : (string)dr["siteAddress"];
                                MonitorStation.address = dr.IsDBNull("address") == true ? null : (string)dr["address"];
                                MonitorStation.description = dr.IsDBNull("description") == true ? null : (string)dr["description"];

                                break;
                            }
                            cmd.Dispose();
                            dr.Close();
                        }
                        catch (Exception e)
                        {
                            //throw e;
                        }

                        con.Close();
                    }
                }
            }
            return MonitorStation;
        }
        public MonitorStation GetMonitorStationByCodeAndOtherID(string code, int id = 0) // lấy ra trạm quan trắc cùng mã nhưng khác id
        {
            MonitorStation MonitorStation = new MonitorStation();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Organ_GetMonitorStationByCodeAndOtherID_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@code", SqlDbType.VarChar).Value = code == null ? DBNull.Value : code.Trim();
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {

                                MonitorStation.monitorStationID = dr.IsDBNull("MonitorStationId") == true ? 0 : (int)dr["MonitorStationId"];
                                MonitorStation.monitorStationName = dr.IsDBNull("MonitorStationName") == true ? null : (string)dr["MonitorStationName"];
                                MonitorStation.monitorStationCode = dr.IsDBNull("monitorStationCode") == true ? null : (string)dr["monitorStationCode"];
                                MonitorStation.latitude = dr.IsDBNull("MonitorStationValueTo") == true ? 0 : (float)(double)dr["latitude"];
                                MonitorStation.longitude = dr.IsDBNull("longitude") == true ? 0 : (float)(double)dr["longitude"];
                                MonitorStation.siteAddress = dr.IsDBNull("siteAddress") == true ? null : (string)dr["siteAddress"];
                                MonitorStation.address = dr.IsDBNull("address") == true ? null : (string)dr["address"];
                                MonitorStation.description = dr.IsDBNull("description") == true ? null : (string)dr["description"];
                                break;
                            }
                            cmd.Dispose();
                            dr.Close();
                        }
                        catch (Exception e)
                        {
                            //throw e;
                        }

                        con.Close();
                    }
                }
            }
            return MonitorStation;
        }

        public void InsertToMonitorStation_StationData(List<int> listid, int monitorstationid = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("Insert_MonitorStation_StationData_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        var table = new DataTable();
                        table.Columns.Add("ID", typeof(int));
                        listid.ForEach(x => table.Rows.Add(x));

                        var pList = new SqlParameter("@list", SqlDbType.Structured);
                        pList.TypeName = "dbo.ListID";
                        pList.Value = table;
                        cmd.Parameters.Add(pList);
                        cmd.Parameters.Add("@monitorstationid", SqlDbType.Int).Value = monitorstationid;

                        con.Open();
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception e)
            {
                //return Ok(new { status = (int)ExitCodes.Error, data = i, message = e.Message });
            }
        }
        public void InsertToMonitorStation_StationData_FromApi(string guiId, string key)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("Insert_MonitorStation_StationData_FromAPI_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@guiId", SqlDbType.VarChar).Value = guiId == null ? DBNull.Value : guiId;
                        cmd.Parameters.Add("@key", SqlDbType.VarChar).Value = key == null ? DBNull.Value : key;
                        cmd.Parameters.Add("@type", SqlDbType.VarChar).Value = (int)MethodType.TUDONG ;
                        con.Open();
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception e)
            {
                //return Ok(new { status = (int)ExitCodes.Error, data = i, message = e.Message });
            }
        }
        public IActionResult Delete(int id, int type = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("Delete_MonitorStation_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id       ", SqlDbType.VarChar).Value = id == 0 ? DBNull.Value : id;
                        cmd.Parameters.Add("@type", SqlDbType.Int).Value = type;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        
                    }
                }
            }
            catch (Exception e)
            {
                //return Ok(new { status = (int)ExitCodes.Error, data = i, message = e.Message });
            }

            return Ok(new { status = (int)ExitCodes.Success, data = "", message = "Xóa thành công" });
        }
        public List<MonitorStation> GetAllListMonitorStationByStationDataID(int id=0,int type =0,string key = null) // get ds trạm theo loại dl
        {
            List<MonitorStation> listMonitorStation = new List<MonitorStation>();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MonitorStation_GetAllListMonitorStationByStationDataID_v1", con))
                {
                    {
                        try
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id == 0 ? DBNull.Value :id;
                            cmd.Parameters.Add("@type", SqlDbType.Int).Value = type == 0 ? (int)MethodType.THUCONG : type;
                            cmd.Parameters.Add("@key", SqlDbType.VarChar).Value = key  == null ? DBNull.Value :key;
                            con.Open();
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                MonitorStation MonitorStation = new MonitorStation();
                                MonitorStation.monitorStationID = dr.IsDBNull("MonitorStationId") == true ? 0 : (int)dr["MonitorStationId"];
                                MonitorStation.monitorStationName = dr.IsDBNull("MonitorStationName") == true ? null : (string)dr["MonitorStationName"];
                                MonitorStation.monitorStationCode = dr.IsDBNull("monitorStationCode") == true ? null : (string)dr["monitorStationCode"];
                                MonitorStation.latitude = dr.IsDBNull("latitude") == true ? 0 : (float)(double)dr["latitude"];
                                MonitorStation.longitude = dr.IsDBNull("longitude") == true ? 0 : (float)(double)dr["longitude"];
                                MonitorStation.siteAddress = dr.IsDBNull("siteAddress") == true ? null : (string)dr["siteAddress"];
                                //MonitorStation.organName = dr.IsDBNull("organName") == true ? null : (string)dr["organName"];
                                MonitorStation.address = dr.IsDBNull("address") == true ? null : (string)dr["address"];
                                MonitorStation.description = dr.IsDBNull("description") == true ? null : (string)dr["description"];
                                MonitorStation.GuiID = dr.IsDBNull("GuiID") == true ? null : (string)dr["GuiID"];
                                MonitorStation.type = dr.IsDBNull("type") == true ? 1 : (int)dr["type"];
                                listMonitorStation.Add(MonitorStation);
                            }
                            cmd.Dispose();
                            dr.Close();
                        }
                        catch (Exception e)
                        {
                            //throw e;
                        }

                        con.Close();
                    }
                }
            }
            return listMonitorStation;
        }

    }
}
