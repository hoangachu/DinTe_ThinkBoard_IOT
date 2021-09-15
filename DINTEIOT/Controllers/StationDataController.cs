using DINTEIOT.Helpers;
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
    public interface IStationDataController
    {
        public List<StationData> GetAllListStationData();
        public List<StationData> GetStationDataByMonitorStationID(int id = 0);
        public StationData GetStationDataByID(int id = 0);
        
    }
    public class StationDataController : BaseController, IStationDataController  // Loại dữ liệu quan trắc
    {
        public IActionResult Index()
        {
            StationDataFilter stationDataFilter = new StationDataFilter();
            if (TempData["OptionFilter"] != null) { stationDataFilter = JsonConvert.DeserializeObject<StationDataFilter>((string)TempData["OptionFilter"]); TempData.Keep(); }
            ViewBag.CurrentPageName = ScreenName.StationData;
            return View(GetListStationData(stationDataFilter));
        }
        //lấy danh sách loại dl có lọc
        public List<StationData> GetListStationData(StationDataFilter stationDataFilter)
        {
            List<StationData> listDataStation = new List<StationData>();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("StationData_GetListStationData_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@pagenumber", SqlDbType.Int).Value = stationDataFilter.pagenumber;
                            cmd.Parameters.Add("@pagesize", SqlDbType.Int).Value = stationDataFilter.pagesize;
                            cmd.Parameters.Add("@txtsearch", SqlDbType.NVarChar).Value = stationDataFilter.txtsearch == null ? DBNull.Value : stationDataFilter.txtsearch;
                            cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = stationDataFilter.startdate == null ? DBNull.Value : stationDataFilter.startdate;
                            cmd.Parameters.Add("@enddate", SqlDbType.DateTime).Value = stationDataFilter.enddate == null ? DBNull.Value : stationDataFilter.enddate;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                StationData StationData = new StationData();
                                StationData.stationDataId = dr.IsDBNull("stationDataId") == true ? 0 : (int)dr["stationDataId"];
                                StationData.stationDataName = dr.IsDBNull("stationDataName") == true ? null : (string)dr["stationDataName"];
                                StationData.stationDataCode = dr.IsDBNull("stationDataCode") == true ? null : (string)dr["stationDataCode"]; 
                                StationData.totalrecord = dr.IsDBNull("totalrecord") == true ? 0 : (int)dr["totalrecord"];
                                listDataStation.Add(StationData);

                            }
                            cmd.Dispose();
                            dr.Close();
                            ViewBag.TotalRecord = listDataStation.Count() > 0 ? listDataStation.First().totalrecord : 0;
                            ViewBag.TotalPage = (ViewBag.TotalRecord / stationDataFilter.pagesize) + 1;
                            ViewBag.PageNumber = stationDataFilter.pagenumber > 0 ? stationDataFilter.pagenumber : 1;
                            ViewBag.PageFirst = stationDataFilter.pagefirst > 0 ? stationDataFilter.pagefirst : 1;
                        }
                        catch (Exception e)
                        {
                            //throw e;
                        }

                        con.Close();
                    }
                }
            }
            return listDataStation;
        }
        //lấy hết danh sách loại dl
        public List<StationData> GetAllListStationData()
        {
            List<StationData> listDataStation = new List<StationData>();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("StationData_GetAllListStationData_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                StationData StationData = new StationData();
                                StationData.stationDataId = dr.IsDBNull("stationDataId") == true ? 0 : (int)dr["stationDataId"];
                                StationData.stationDataName = dr.IsDBNull("stationDataName") == true ? null : (string)dr["stationDataName"];
                                StationData.stationDataCode = dr.IsDBNull("stationDataCode") == true ? null : (string)dr["stationDataCode"];
                                StationData.type = dr.IsDBNull("type") == true ? 0 : (int)dr["type"];
                                listDataStation.Add(StationData);

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
            return listDataStation;
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.THEM, WebApi.Helpers.Function.ORGAN)]
        public IActionResult insertpre() //check quyền trc khi thêm mới
        {
            return Redirect("/stationdata/GetStationDataByIDPre");
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.SUA, WebApi.Helpers.Function.ORGAN)]
        public IActionResult updatepre(int id = 0) //check quyền trc khi cập nhật
        {
            return Redirect("/stationdata/GetStationDataByIDPre?id=" + id);
        }
        public IActionResult GetStationDataByIDPre(int id = 0)
        {
            var stationData = GetStationDataByID(id);
            if (stationData.stationDataId < 0)
            {
                return Ok(new { status = (int)ExitCodes.Error, data = "", message = "Lỗi!" });
            }
            return Ok(new { status = (int)ExitCodes.Success, data = stationData, message = "" });
        }
        public StationData GetStationDataByID(int id = 0)
        {
            StationData stationData = new StationData();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Organ_GetStationDataByID_v1", con))
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

                                stationData.stationDataId = dr.IsDBNull("stationDataId") == true ? 0 : (int)dr["stationDataId"];
                                stationData.stationDataName = dr.IsDBNull("stationDataName") == true ? "" : (string)dr["stationDataName"];
                                stationData.stationDataCode = dr.IsDBNull("stationDataCode") == true ? null : (string)dr["stationDataCode"];

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
            return stationData;
        }
        public StationData GetStationDataByCodeAndOtherID(string code,int id = 0) // lấy loại dl có cùng mã và khác id
        {
            StationData stationData = new StationData();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Organ_GetStationDataByCode_v1", con))
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

                                stationData.stationDataId = dr.IsDBNull("stationDataId") == true ? 0 : (int)dr["stationDataId"];
                                stationData.stationDataName = dr.IsDBNull("stationDataName") == true ? null : (string)dr["stationDataName"];
                                stationData.stationDataCode = dr.IsDBNull("stationDataCode") == true ? null : (string)dr["stationDataCode"];

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
            return stationData;
        }

        [HttpPost]
        public IActionResult Insert(StationData stationData)
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("Insert_StationData_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@stationDataName", SqlDbType.NVarChar).Value = stationData.stationDataName == null ? DBNull.Value : stationData.stationDataName;
                        cmd.Parameters.Add("@stationDataCode", SqlDbType.VarChar).Value = stationData.stationDataCode == null ? DBNull.Value : stationData.stationDataCode;
                        cmd.Parameters.Add("@type", SqlDbType.VarChar).Value = stationData.type == 0 ? DBNull.Value : stationData.type;
                        cmd.Parameters.Add("@stationDataID", SqlDbType.Int).Direction = ParameterDirection.Output;
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

        } // Thêm mới
        public IActionResult Update(StationData stationData)
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Update_StationData_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@stationDataName", SqlDbType.NVarChar).Value = stationData.stationDataName == null ? DBNull.Value : stationData.stationDataName;
                        cmd.Parameters.Add("@stationDataCode", SqlDbType.VarChar).Value = stationData.stationDataCode == null ? DBNull.Value : stationData.stationDataCode;
                        cmd.Parameters.Add("@stationDataID", SqlDbType.VarChar).Value = stationData.stationDataId == 0 ? DBNull.Value : stationData.stationDataId;
                        cmd.Parameters.Add("@type", SqlDbType.VarChar).Value = stationData.type == 0 ? DBNull.Value : stationData.type;
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
        }  // Sửa
        [HttpPost]
        public IActionResult CheckStationDataByCode(string stationDataCode, int id = 0) //hàm ktra mã loại dl đã tồn tại chưa
        {
            bool index = false;
            if (!string.IsNullOrEmpty(stationDataCode))
            {
                var stationData = GetStationDataByCodeAndOtherID(stationDataCode, id);
                if (stationData.stationDataId > 0)
                {
                    index = true;
                }
            }
            return Ok(new { data = index });
        }
        [HttpPost]
        public void GetListAfterFilter(StationDataFilter stationDataFilter)  //hàm gọi khi nhấn trong phần trang,lọc,...
        {
            TempData["OptionFilter"] = JsonConvert.SerializeObject(stationDataFilter);
        }

        public List<StationData> GetStationDataByMonitorStationID(int id = 0) // lấy ds loại dl quan trắc by trạm
        {
            List<StationData> listStationData = new List<StationData>();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetStationDataByMonitorStationID", con))
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
                                StationData StationData = new StationData();
                                StationData.stationDataId = dr.IsDBNull("stationDataId") == true ? 0 : (int)dr["stationDataId"];
                                StationData.stationDataName = dr.IsDBNull("stationDataName") == true ? "" : (string)dr["stationDataName"];
                                listStationData.Add(StationData);
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
            return listStationData;
        }

        public IActionResult Delete(int id, int type = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("Delete_StationData_v1", con))
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
        public IActionResult DeleteStationdata_MonitorStation()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("DeleteStationdata_MonitorStation_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //cmd.Parameters.Add("@id       ", SqlDbType.VarChar).Value = id == 0 ? DBNull.Value : id;
                        //cmd.Parameters.Add("@type", SqlDbType.Int).Value = type;
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
    }
}
