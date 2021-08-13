using DINTEIOT.Helpers;
using DINTEIOT.Models.StationDriver;
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
    public class StationDriverController : BaseController  // Phương tiện quan trắc
    {
        private IMonitorStationController _imonitorStationController;

        public StationDriverController(IMonitorStationController IMonitorStationController)
        {
            _imonitorStationController = IMonitorStationController;
        }
        public IActionResult Index()
        {
            StationDriverFilter StationDriverFilter = new StationDriverFilter();
            if (TempData["OptionFilter"] != null) { StationDriverFilter = JsonConvert.DeserializeObject<StationDriverFilter>((string)TempData["OptionFilter"]); TempData.Keep(); }
            ViewBag.CurrentPageName = ScreenName.StationDriver;
            ViewBag.TramQuanTrac = _imonitorStationController.GetAllListMonitorStation();
            return View(GetListStationDriver(StationDriverFilter));
        }
        //lấy danh sách ngưỡng cảnh báo
        public List<StationDriver> GetListStationDriver(StationDriverFilter StationDriverFilter)
        {
            List<StationDriver> listStationDriver = new List<StationDriver>();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("StationDriver_GetListStationDriver_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@pagenumber", SqlDbType.Int).Value = StationDriverFilter.pagenumber;
                            cmd.Parameters.Add("@pagesize", SqlDbType.Int).Value = StationDriverFilter.pagesize;
                            cmd.Parameters.Add("@txtsearch", SqlDbType.NVarChar).Value = StationDriverFilter.txtsearch == null ? DBNull.Value : StationDriverFilter.txtsearch;
                            cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = StationDriverFilter.startdate == null ? DBNull.Value : StationDriverFilter.startdate;
                            cmd.Parameters.Add("@enddate", SqlDbType.DateTime).Value = StationDriverFilter.enddate == null ? DBNull.Value : StationDriverFilter.enddate;
                            cmd.Parameters.Add("@monitorStationID", SqlDbType.Int).Value = StationDriverFilter.monitorStationID == 0 ? DBNull.Value : StationDriverFilter.monitorStationID;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                StationDriver StationDriver = new StationDriver();
                                StationDriver.stationDriverID = dr.IsDBNull("StationDriverId") == true ? 0 : (int)dr["StationDriverId"];
                                StationDriver.stationDriverName = dr.IsDBNull("stationDriverName") == true ? null : (string)dr["stationDriverName"];
                                StationDriver.stationDriverCode = dr.IsDBNull("StationDriverCode") == true ? null : (string)dr["StationDriverCode"];
                                StationDriver.monitorStationID = dr.IsDBNull("monitorStationID") == true ? 0 : (int)dr["monitorStationID"];
                                StationDriver.monitorStationName = dr.IsDBNull("monitorStationName") == true ? null : (string)dr["monitorStationName"];
                                StationDriver.stationDriverType = dr.IsDBNull("stationDriverType") == true ? null : (string)dr["stationDriverType"];
                                StationDriver.description = dr.IsDBNull("description") == true ? null : (string)dr["description"];
                                StationDriver.totalrecord = dr.IsDBNull("totalrecord") == true ? 0 : (int)dr["totalrecord"];
                                listStationDriver.Add(StationDriver);
                            }
                            cmd.Dispose();
                            dr.Close();
                            ViewBag.TotalRecord = listStationDriver.Count() > 0 ? listStationDriver.First().totalrecord : 0;
                            ViewBag.TotalPage = (ViewBag.TotalRecord / StationDriverFilter.pagesize) + 1;
                            ViewBag.PageNumber = StationDriverFilter.pagenumber > 0 ? StationDriverFilter.pagenumber : 1;
                            ViewBag.PageFirst = StationDriverFilter.pagefirst > 0 ? StationDriverFilter.pagefirst : 1;
                        }
                        catch (Exception e)
                        {
                            //throw e;
                        }

                        con.Close();
                    }
                }
            }
            return listStationDriver;
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.THEM, WebApi.Helpers.Function.ORGAN)]
        public IActionResult insertpre() //check quyền trc khi thêm mới
        {
            return Redirect("/StationDriver/GetStationDriverByIDPre");
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.SUA, WebApi.Helpers.Function.ORGAN)]
        public IActionResult updatepre(int id = 0) //check quyền trc khi cập nhật
        {
            return Redirect("/StationDriver/GetStationDriverByIDPre?id=" + id);
        }
        public IActionResult GetStationDriverByIDPre(int id = 0)
        {
            var StationDriver = GetStationDriverByID(id);
            if (StationDriver.stationDriverID < 0)
            {
                return Ok(new { status = (int)ExitCodes.Error, data = "", message = "Lỗi!" });
            }

            return Ok(new { status = (int)ExitCodes.Success, data = StationDriver, message = "" });
        }
        public StationDriver GetStationDriverByID(int id = 0)
        {
            StationDriver StationDriver = new StationDriver();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("StationDriver_GetStationDriverByID_v1", con))
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
                                StationDriver.stationDriverID = dr.IsDBNull("StationDriverId") == true ? 0 : (int)dr["StationDriverId"];
                                StationDriver.stationDriverName = dr.IsDBNull("stationDriverName") == true ? null : (string)dr["stationDriverName"];
                                StationDriver.stationDriverCode = dr.IsDBNull("StationDriverCode") == true ? null : (string)dr["StationDriverCode"];
                                StationDriver.monitorStationID = dr.IsDBNull("monitorStationID") == true ? 0 : (int)dr["monitorStationID"];
                                StationDriver.monitorStationName = dr.IsDBNull("monitorStationName") == true ? null : (string)dr["monitorStationName"];
                                StationDriver.stationDriverType = dr.IsDBNull("stationDriverType") == true ? null : (string)dr["stationDriverType"];
                                StationDriver.description = dr.IsDBNull("description") == true ? null : (string)dr["description"];
                              
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
            return StationDriver;
        }


        [HttpPost]
        public IActionResult Insert(StationDriver StationDriver)  // Thêm mới
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("Insert_StationDriver_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@StationDriverName          ", SqlDbType.NVarChar).Value = StationDriver.stationDriverName == null ? DBNull.Value : StationDriver.stationDriverName;
                        cmd.Parameters.Add("@StationDriverCode       ", SqlDbType.VarChar).Value = StationDriver.stationDriverCode == null ? DBNull.Value : StationDriver.stationDriverCode;
                        cmd.Parameters.Add("@monitorStationID                     ", SqlDbType.Int).Value = StationDriver.monitorStationID == 0 ? DBNull.Value : StationDriver.monitorStationID;
                        cmd.Parameters.Add("@stationDriverType              ", SqlDbType.NVarChar).Value = StationDriver.stationDriverType == null ? DBNull.Value : StationDriver.stationDriverType;
                        cmd.Parameters.Add("@description                ", SqlDbType.NVarChar).Value = StationDriver.description == null ? DBNull.Value : StationDriver.description;
                        cmd.Parameters.Add("@StationDriverID            ", SqlDbType.Int).Direction = ParameterDirection.Output;
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
        public IActionResult Update(StationDriver StationDriver) // Sửa
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Update_StationDriver_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@StationDriverCode       ", SqlDbType.VarChar).Value = StationDriver.stationDriverCode == null ? DBNull.Value : StationDriver.stationDriverCode;
                        cmd.Parameters.Add("@StationDriverName       ", SqlDbType.NVarChar).Value = StationDriver.stationDriverName == null ? DBNull.Value : StationDriver.stationDriverName;
                        cmd.Parameters.Add("@monitorStationID                     ", SqlDbType.Int).Value = StationDriver.monitorStationID == 0 ? DBNull.Value : StationDriver.monitorStationID;
                        cmd.Parameters.Add("@stationDriverType              ", SqlDbType.NVarChar).Value = StationDriver.stationDriverType == null ? DBNull.Value : StationDriver.stationDriverType;
                        cmd.Parameters.Add("@description                ", SqlDbType.NVarChar).Value = StationDriver.description == null ? DBNull.Value : StationDriver.description;
                        cmd.Parameters.Add("@StationDriverID            ", SqlDbType.Int).Value = StationDriver.stationDriverID == 0 ? DBNull.Value : StationDriver.stationDriverID;
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
        public void GetListAfterFilter(StationDriverFilter StationDriverFilter)  //hàm gọi khi nhấn trong phần trang,lọc,...
        {
            TempData["OptionFilter"] = JsonConvert.SerializeObject(StationDriverFilter);
        }
        [HttpPost]
        public IActionResult CheckStationDriverByCode(string stationDriverCode = null, int id = 0) //hàm ktra mã trạm đã tồn tại chưa
        {
            bool index = false;
            if (!string.IsNullOrEmpty(stationDriverCode))
            {
                var StationDriver = GetStationDriverByCodeAndOtherID(stationDriverCode, id);
                if (StationDriver.stationDriverID > 0)
                {
                    index = true;
                }
            }
            return Ok(new { data = index });
        }
        public StationDriver GetStationDriverByName(string StationDriverName, int id = 0) // lấy ra trạm quan trắc cùng tên nhưng khác id
        {
            StationDriver StationDriver = new StationDriver();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("StationDriver_GetStationDriverByNameAndOtherID_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@StationDriverName", SqlDbType.NVarChar).Value = StationDriverName == null ? DBNull.Value : StationDriverName;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {

                                StationDriver.stationDriverID = dr.IsDBNull("StationDriverId") == true ? 0 : (int)dr["StationDriverId"];
                                StationDriver.stationDriverName = dr.IsDBNull("stationDriverName") == true ? null : (string)dr["stationDriverName"];
                                StationDriver.stationDriverCode = dr.IsDBNull("StationDriverCode") == true ? null : (string)dr["StationDriverCode"];
                                StationDriver.monitorStationID = dr.IsDBNull("monitorStationID") == true ? 0 : (int)dr["monitorStationID"];
                                StationDriver.stationDriverType = dr.IsDBNull("stationDriveType") == true ? null : (string)dr["stationDriveType"];
                                StationDriver.description = dr.IsDBNull("description") == true ? null : (string)dr["description"];
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
            return StationDriver;
        }
        public StationDriver GetStationDriverByCodeAndOtherID(string code, int id = 0) // lấy ra trạm quan trắc cùng mã nhưng khác id
        {
            StationDriver StationDriver = new StationDriver();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Organ_GetStationDriverByCodeAndOtherID_v1", con))
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

                                StationDriver.stationDriverID = dr.IsDBNull("StationDriverId") == true ? 0 : (int)dr["StationDriverId"];
                                StationDriver.stationDriverName = dr.IsDBNull("stationDriverName") == true ? null : (string)dr["stationDriverName"];
                                StationDriver.stationDriverCode = dr.IsDBNull("StationDriverCode") == true ? null : (string)dr["StationDriverCode"];
                                StationDriver.monitorStationID = dr.IsDBNull("monitorStationID") == true ? 0 : (int)dr["monitorStationID"];
                                StationDriver.stationDriverType = dr.IsDBNull("stationDriveType") == true ? null : (string)dr["stationDriveType"];
                                StationDriver.description = dr.IsDBNull("description") == true ? null : (string)dr["description"];
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
            return StationDriver;
        }
    }
}
