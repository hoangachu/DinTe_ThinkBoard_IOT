using DINTEIOT.Helpers;
using DINTEIOT.Models.MonitorDatabase;
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
    public class MonitorDatabaseController : Controller
    {
        private IOrganController _iorganController;
        private IMonitorStationController _iMonitorStationController;
        private IStationDataController _istationDataController;

        public MonitorDatabaseController(IOrganController IOrganController, IMonitorStationController IMonitorStationController, IStationDataController IStationDataController)
        {
            _iorganController = IOrganController;
            _iMonitorStationController = IMonitorStationController;
            _istationDataController = IStationDataController;
        }
        public IActionResult Index()
        {
        
            Models.MonitorStation.MonitorStationFilter MonitorStationFilter = new Models.MonitorStation.MonitorStationFilter();
            if (TempData["OptionFilter"] != null) { MonitorStationFilter = JsonConvert.DeserializeObject<Models.MonitorStation.MonitorStationFilter>((string)TempData["OptionFilter"]); TempData.Keep(); }
            ViewBag.CurrentPageName = ScreenName.MonitorDatabase;
            ViewBag.CoQuan = _iorganController.GetSelectTreeViewNode();
            return View(_iMonitorStationController.GetListMonitorStation(MonitorStationFilter));
        }
       
        //lấy danh sách số liệu có lọc
        public List<MonitorDatabase> GetListMonitorDatabaseByMonitorID(MonitorDatabaseFilter MonitorDatabaseFilter)
        {
            List<MonitorDatabase> listMonitorDatabase = new List<MonitorDatabase>();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MonitorDatabase_GetListMonitorDatabaseByMonitorID_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@pagenumber", SqlDbType.Int).Value = MonitorDatabaseFilter.pagenumber;
                            cmd.Parameters.Add("@pagesize", SqlDbType.Int).Value = MonitorDatabaseFilter.pagesize;
                            cmd.Parameters.Add("@txtsearch", SqlDbType.NVarChar).Value = MonitorDatabaseFilter.txtsearch == null ? DBNull.Value : MonitorDatabaseFilter.txtsearch;
                            cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = MonitorDatabaseFilter.startdate == null ? DBNull.Value : MonitorDatabaseFilter.startdate;
                            cmd.Parameters.Add("@enddate", SqlDbType.DateTime).Value = MonitorDatabaseFilter.enddate == null ? DBNull.Value : MonitorDatabaseFilter.enddate;
                            cmd.Parameters.Add("@monitorStationID", SqlDbType.Int).Value = MonitorDatabaseFilter.monitorID == 0 ? DBNull.Value : MonitorDatabaseFilter.monitorID;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                MonitorDatabase MonitorDatabase = new MonitorDatabase();
                                MonitorDatabase.monitorDatabaseID = dr.IsDBNull("MonitorDatabaseId") == true ? 0 : (int)dr["MonitorDatabaseId"];
                                MonitorDatabase.monitorDatabaseTime = dr.IsDBNull("monitorDatabaseTime") == true ? null : Convert.ToDateTime(dr["monitorDatabaseTime"]).ToString("dd/MM/yyyy");
                                MonitorDatabase.monitorDatabaseUnit = dr.IsDBNull("monitorDatabaseUnit") == true ? null : (string)dr["monitorDatabaseUnit"];
                                MonitorDatabase.monitorDatabaseValue = dr.IsDBNull("monitorDatabaseValue") == true ? 0 : (int)dr["monitorDatabaseValue"];
                                MonitorDatabase.monitorStationID = dr.IsDBNull("monitorStationID") == true ? 0 : (int)dr["monitorStationID"];
                                MonitorDatabase.stationDataID = dr.IsDBNull("stationDataID") == true ? 0 : (int)dr["stationDataID"];
                                MonitorDatabase.stationDataName = dr.IsDBNull("stationDataName") == true ? null : (string)dr["stationDataName"];
                                MonitorDatabase.totalrecord = dr.IsDBNull("totalrecord") == true ? 0 : (int)dr["totalrecord"];
                                listMonitorDatabase.Add(MonitorDatabase);
                            }
                            cmd.Dispose();
                            dr.Close();
                            ViewBag.TotalRecord = listMonitorDatabase.Count() > 0 ? listMonitorDatabase.First().totalrecord : 0;
                            ViewBag.TotalPage = (ViewBag.TotalRecord / MonitorDatabaseFilter.pagesize) + 1;
                            ViewBag.PageNumber = MonitorDatabaseFilter.pagenumber > 0 ? MonitorDatabaseFilter.pagenumber : 1;
                            ViewBag.PageFirst = MonitorDatabaseFilter.pagefirst > 0 ? MonitorDatabaseFilter.pagefirst : 1;
                        }
                        catch (Exception e)
                        {
                            //throw e;
                        }

                        con.Close();
                    }
                }
            }
            return listMonitorDatabase;
        }
        //lấy danh sách ngưỡng cảnh báo không lọc
        public List<MonitorDatabase> GetAllListMonitorDatabase()
        {
            List<MonitorDatabase> listMonitorDatabase = new List<MonitorDatabase>();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MonitorDatabase_GetAllListMonitorDatabase_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                MonitorDatabase MonitorDatabase = new MonitorDatabase();
                                MonitorDatabase.monitorDatabaseID = dr.IsDBNull("MonitorDatabaseId") == true ? 0 : (int)dr["MonitorDatabaseId"];
                                //MonitorDatabase.monitorDatabaseTime = dr.IsDBNull("monitorDatabaseTime") == true ? DateTime.Now : (DateTime)dr["monitorDatabaseTime"];
                                MonitorDatabase.monitorDatabaseUnit = dr.IsDBNull("monitorDatabaseUnit") == true ? null : (string)dr["monitorDatabaseUnit"];
                                MonitorDatabase.monitorDatabaseValue = dr.IsDBNull("monitorDatabaseValue") == true ? 0 : (int)dr["monitorDatabaseValue"];
                                MonitorDatabase.monitorStationID = dr.IsDBNull("monitorStationID") == true ? 0 : (int)dr["monitorStationID"];
                                MonitorDatabase.stationDataID = dr.IsDBNull("stationDataID") == true ? 0 : (int)dr["stationDataID"];
                                listMonitorDatabase.Add(MonitorDatabase);
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
            return listMonitorDatabase;
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.THEM, WebApi.Helpers.Function.ORGAN)]
        public IActionResult insertpre() //check quyền trc khi thêm mới
        {
            return Redirect("/MonitorDatabase/GetMonitorDatabaseByIDPre");
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.SUA, WebApi.Helpers.Function.ORGAN)]
        public IActionResult updatepre(int id = 0) //check quyền trc khi cập nhật
        {
            return Redirect("/MonitorDatabase/GetMonitorDatabaseByIDPre?id=" + id);
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.THEM, WebApi.Helpers.Function.ORGAN)]
        public IActionResult showdatapre() //check quyền trc khi thêm mới
        {
            return Ok();
        }
        public IActionResult ListMonitorDatabase(int monitorID = 0)
        {
            MonitorDatabaseFilter MonitorDatabaseFilter = new MonitorDatabaseFilter();
            MonitorDatabaseFilter.monitorID = monitorID;
            if (TempData["OptionFilter"] != null) { MonitorDatabaseFilter = JsonConvert.DeserializeObject<MonitorDatabaseFilter>((string)TempData["OptionFilter"]); TempData.Keep(); }
            ViewBag.LoaiDuLieu = _istationDataController.GetAllListStationData();
            ViewBag.MonitorID = monitorID;
            ViewBag.CurrentPageName = "/" + ScreenName.MonitorDatabase;
            return View(GetListMonitorDatabaseByMonitorID(MonitorDatabaseFilter)); ;
        }
        public IActionResult GetMonitorDatabaseByIDPre(int id = 0)
        {
            var MonitorDatabase = GetMonitorDatabaseByID(id);
            if (MonitorDatabase.monitorDatabaseID < 0)
            {
                return Ok(new { status = (int)ExitCodes.Error, data = "", message = "Lỗi!" });
            }

            return Ok(new { status = (int)ExitCodes.Success, data = MonitorDatabase, message = "" });
        }
        public MonitorDatabase GetMonitorDatabaseByID(int id = 0)
        {
            MonitorDatabase MonitorDatabase = new MonitorDatabase();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MonitorDatabase_GetMonitorDatabaseByID_v1", con))
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
                                MonitorDatabase.monitorDatabaseID = dr.IsDBNull("MonitorDatabaseId") == true ? 0 : (int)dr["MonitorDatabaseId"];
                                MonitorDatabase.monitorDatabaseTime = dr.IsDBNull("monitorDatabaseTime") == true ? null :Convert.ToDateTime(dr["monitorDatabaseTime"]).ToString("dd/MM/yyyy");
                                MonitorDatabase.monitorDatabaseUnit = dr.IsDBNull("monitorDatabaseUnit") == true ? null : (string)dr["monitorDatabaseUnit"];
                                MonitorDatabase.monitorDatabaseValue = dr.IsDBNull("monitorDatabaseValue") == true ? 0 : (int)dr["monitorDatabaseValue"];
                                MonitorDatabase.monitorStationID = dr.IsDBNull("monitorStationID") == true ? 0 : (int)dr["monitorStationID"];
                                MonitorDatabase.stationDataName = dr.IsDBNull("stationDataName") == true ? null : (string)dr["stationDataName"];
                                MonitorDatabase.stationDataID = dr.IsDBNull("stationDataID") == true ? 0 : (int)dr["stationDataID"];
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
            return MonitorDatabase;
        }


        [HttpPost]
        public IActionResult Insert(MonitorDatabase MonitorDatabase)  // Thêm mới
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("Insert_MonitorDatabase_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@monitorDatabaseTime          ", SqlDbType.DateTime).Value = MonitorDatabase.monitorDatabaseTime == null ? DBNull.Value : MonitorDatabase.monitorDatabaseTime;
                        cmd.Parameters.Add("@monitorDatabaseValue       ", SqlDbType.Int).Value = MonitorDatabase.monitorDatabaseValue == 0 ? DBNull.Value : MonitorDatabase.monitorDatabaseValue;
                        cmd.Parameters.Add("@monitorDatabaseUnit                     ", SqlDbType.VarChar).Value = MonitorDatabase.monitorDatabaseUnit == null ? DBNull.Value : MonitorDatabase.monitorDatabaseUnit;
                        cmd.Parameters.Add("@stationDataID              ", SqlDbType.Int).Value = MonitorDatabase.stationDataID == 0 ? DBNull.Value : MonitorDatabase.stationDataID;
                        cmd.Parameters.Add("@monitorStationID                ", SqlDbType.Int).Value = MonitorDatabase.monitorStationID == 0 ? DBNull.Value : MonitorDatabase.monitorStationID;
                        cmd.Parameters.Add("@MonitorDatabaseID            ", SqlDbType.Int).Direction = ParameterDirection.Output;
                        con.Open();
                        i = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                return Ok(new { status = (int)ExitCodes.Error, data = i, message = e.Message });
            }

            return Ok(new { status = (int)ExitCodes.Success, data = i, message = "Thêm mới thành công" });

        }
        public IActionResult Update(MonitorDatabase MonitorDatabase) // Sửa
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Update_MonitorDatabase_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@monitorDatabaseTime          ", SqlDbType.DateTime).Value = MonitorDatabase.monitorDatabaseTime == null ? DBNull.Value : MonitorDatabase.monitorDatabaseTime;
                        cmd.Parameters.Add("@monitorDatabaseValue       ", SqlDbType.Int).Value = MonitorDatabase.monitorDatabaseValue == 0 ? DBNull.Value : MonitorDatabase.monitorDatabaseValue;
                        cmd.Parameters.Add("@monitorDatabaseUnit                     ", SqlDbType.VarChar).Value = MonitorDatabase.monitorDatabaseUnit == null ? DBNull.Value : MonitorDatabase.monitorDatabaseUnit;
                        cmd.Parameters.Add("@stationDataID              ", SqlDbType.Int).Value = MonitorDatabase.stationDataID == 0 ? DBNull.Value : MonitorDatabase.stationDataID;
                        cmd.Parameters.Add("@monitorStationID                ", SqlDbType.Int).Value = MonitorDatabase.monitorStationID == 0 ? DBNull.Value : MonitorDatabase.monitorStationID;
                        cmd.Parameters.Add("@MonitorDatabaseID                ", SqlDbType.Int).Value = MonitorDatabase.monitorDatabaseID == 0 ? DBNull.Value : MonitorDatabase.monitorDatabaseID;
                        con.Open();
                        i = cmd.ExecuteNonQuery();
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
        public void GetListAfterFilter(MonitorDatabaseFilter MonitorDatabaseFilter)  //hàm gọi khi nhấn trong phần trang,lọc,...
        {
            TempData["OptionFilter"] = JsonConvert.SerializeObject(MonitorDatabaseFilter);
        }
        [HttpPost]
        public IActionResult CheckMonitorDatabaseByCode(string MonitorDatabaseCode = null, int id = 0) //hàm ktra mã trạm đã tồn tại chưa
        {
            bool index = false;
            if (!string.IsNullOrEmpty(MonitorDatabaseCode))
            {
                var MonitorDatabase = GetMonitorDatabaseByCodeAndOtherID(MonitorDatabaseCode, id);
                if (MonitorDatabase.monitorDatabaseID > 0)
                {
                    index = true;
                }
            }
            return Ok(new { data = index });
        }
        public MonitorDatabase GetMonitorDatabaseByName(string MonitorDatabaseName, int id = 0) // lấy ra trạm quan trắc cùng tên nhưng khác id
        {
            MonitorDatabase MonitorDatabase = new MonitorDatabase();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MonitorDatabase_GetMonitorDatabaseByNameAndOtherID_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@MonitorDatabaseName", SqlDbType.NVarChar).Value = MonitorDatabaseName == null ? DBNull.Value : MonitorDatabaseName;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {

                                MonitorDatabase.monitorDatabaseID = dr.IsDBNull("MonitorDatabaseId") == true ? 0 : (int)dr["MonitorDatabaseId"];
                                MonitorDatabase.monitorDatabaseTime = dr.IsDBNull("monitorDatabaseTime") == true ? null : Convert.ToDateTime(dr["monitorDatabaseTime"]).ToString("dd/MM/yyyy");
                                MonitorDatabase.monitorDatabaseUnit = dr.IsDBNull("monitorDatabaseUnit") == true ? null : (string)dr["monitorDatabaseUnit"];
                                MonitorDatabase.monitorDatabaseValue = dr.IsDBNull("monitorDatabaseValue") == true ? 0 : (int)dr["monitorDatabaseValue"];
                                MonitorDatabase.monitorStationID = dr.IsDBNull("monitorStationID") == true ? 0 : (int)dr["monitorStationID"];
                                MonitorDatabase.stationDataID = dr.IsDBNull("stationDataID") == true ? 0 : (int)dr["stationDataID"];

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
            return MonitorDatabase;
        }
        public MonitorDatabase GetMonitorDatabaseByCodeAndOtherID(string code, int id = 0) // lấy ra trạm quan trắc cùng mã nhưng khác id
        {
            MonitorDatabase MonitorDatabase = new MonitorDatabase();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Organ_GetMonitorDatabaseByCodeAndOtherID_v1", con))
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

                                MonitorDatabase.monitorDatabaseID = dr.IsDBNull("MonitorDatabaseId") == true ? 0 : (int)dr["MonitorDatabaseId"];
                                MonitorDatabase.monitorDatabaseTime = dr.IsDBNull("monitorDatabaseTime") == true ? null : Convert.ToDateTime(dr["monitorDatabaseTime"]).ToString("dd/MM/yyyy");
                                MonitorDatabase.monitorDatabaseUnit = dr.IsDBNull("monitorDatabaseUnit") == true ? null : (string)dr["monitorDatabaseUnit"];
                                MonitorDatabase.monitorDatabaseValue = dr.IsDBNull("monitorDatabaseValue") == true ? 0 : (int)dr["monitorDatabaseValue"];
                                MonitorDatabase.monitorStationID = dr.IsDBNull("monitorStationID") == true ? 0 : (int)dr["monitorStationID"];
                                MonitorDatabase.stationDataID = dr.IsDBNull("stationDataID") == true ? 0 : (int)dr["stationDataID"];
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
            return MonitorDatabase;
        }
    }
}
