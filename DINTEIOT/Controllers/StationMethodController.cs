using DINTEIOT.Helpers;
using DINTEIOT.Models.StationMethod;
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
    public class StationMethodController : BaseController   //phương thức quan trắc
    {
        private IStationDataController _istationDataController;

        public StationMethodController(IStationDataController IStationDataController)
        {
            _istationDataController = IStationDataController;
        }
        public IActionResult Index()
        {
            StationMethodFilter StationMethodFilter = new StationMethodFilter();
            if (TempData["OptionFilter"] != null) { StationMethodFilter = JsonConvert.DeserializeObject<StationMethodFilter>((string)TempData["OptionFilter"]); TempData.Keep(); }
            ViewBag.CurrentPageName = ScreenName.StationMethod;
            ViewBag.LoaiDuLieu = _istationDataController.GetAllListStationData();
            return View(GetListStationMethod(StationMethodFilter));
        }
        //lấy danh sách ngưỡng cảnh báo
        public List<StationMethod> GetListStationMethod(StationMethodFilter StationMethodFilter)
        {
            List<StationMethod> listStationMethod = new List<StationMethod>();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("StationMethod_GetListStationMethod_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@pagenumber", SqlDbType.Int).Value = StationMethodFilter.pagenumber;
                            cmd.Parameters.Add("@pagesize", SqlDbType.Int).Value = StationMethodFilter.pagesize;
                            cmd.Parameters.Add("@txtsearch", SqlDbType.NVarChar).Value = StationMethodFilter.txtsearch == null ? DBNull.Value : StationMethodFilter.txtsearch;
                            cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = StationMethodFilter.startdate == null ? DBNull.Value : StationMethodFilter.startdate;
                            cmd.Parameters.Add("@enddate", SqlDbType.DateTime).Value = StationMethodFilter.enddate == null ? DBNull.Value : StationMethodFilter.enddate;
                            cmd.Parameters.Add("@stationDataID", SqlDbType.Int).Value = StationMethodFilter.stationDataID == 0 ? DBNull.Value : StationMethodFilter.stationDataID;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                StationMethod StationMethod = new StationMethod();
                                StationMethod.stationMethodID = dr.IsDBNull("StationMethodId") == true ? 0 : (int)dr["StationMethodId"];
                                StationMethod.stationMethodName = dr.IsDBNull("StationMethodName") == true ? null : (string)dr["StationMethodName"];
                                StationMethod.stationMethodCode = dr.IsDBNull("StationMethodCode") == true ? null : (string)dr["StationMethodCode"];
                                StationMethod.stationDataID = dr.IsDBNull("stationDataID") == true ? 0 : (int)dr["stationDataID"];
                                StationMethod.stationDataName = dr.IsDBNull("stationDataName") == true ? null : (string)dr["stationDataName"];
                                StationMethod.description = dr.IsDBNull("description") == true ? null : (string)dr["description"];
                                StationMethod.totalrecord = dr.IsDBNull("totalrecord") == true ? 0 : (int)dr["totalrecord"];
                                listStationMethod.Add(StationMethod);
                            }
                            cmd.Dispose();
                            dr.Close();
                            ViewBag.TotalRecord = listStationMethod.Count() > 0 ? listStationMethod.First().totalrecord : 0;
                            ViewBag.TotalPage = (ViewBag.TotalRecord / StationMethodFilter.pagesize) + 1;
                            ViewBag.PageNumber = StationMethodFilter.pagenumber > 0 ? StationMethodFilter.pagenumber : 1;
                            ViewBag.PageFirst = StationMethodFilter.pagefirst > 0 ? StationMethodFilter.pagefirst : 1;
                        }
                        catch (Exception e)
                        {
                            //throw e;
                        }

                        con.Close();
                    }
                }
            }
            return listStationMethod;
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.THEM, WebApi.Helpers.Function.ORGAN)]
        public IActionResult insertpre() //check quyền trc khi thêm mới
        {
            return Redirect("/StationMethod/GetStationMethodByIDPre");
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.SUA, WebApi.Helpers.Function.ORGAN)]
        public IActionResult updatepre(int id = 0) //check quyền trc khi cập nhật
        {
            return Redirect("/StationMethod/GetStationMethodByIDPre?id=" + id);
        }
        public IActionResult GetStationMethodByIDPre(int id = 0)
        {
            var StationMethod = GetStationMethodByID(id);
            if (StationMethod.stationMethodID < 0)
            {
                return Ok(new { status = (int)ExitCodes.Error, data = "", message = "Lỗi!" });
            }

            return Ok(new { status = (int)ExitCodes.Success, data = StationMethod, message = "" });
        }
        public StationMethod GetStationMethodByID(int id = 0)
        {
            StationMethod StationMethod = new StationMethod();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("StationMethod_GetStationMethodByID_v1", con))
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
                                StationMethod.stationMethodID = dr.IsDBNull("StationMethodId") == true ? 0 : (int)dr["StationMethodId"];
                                StationMethod.stationMethodName = dr.IsDBNull("StationMethodName") == true ? null : (string)dr["StationMethodName"];
                                StationMethod.stationMethodCode = dr.IsDBNull("StationMethodCode") == true ? null : (string)dr["StationMethodCode"];
                                StationMethod.stationDataID = dr.IsDBNull("stationDataID") == true ? 0 : (int)dr["stationDataID"];
                                StationMethod.stationDataName = dr.IsDBNull("stationDataName") == true ? null : (string)dr["stationDataName"];
                                StationMethod.description = dr.IsDBNull("description") == true ? null : (string)dr["description"];

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
            return StationMethod;
        }


        [HttpPost]
        public IActionResult Insert(StationMethod StationMethod)  // Thêm mới
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("Insert_StationMethod_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@StationMethodName          ", SqlDbType.NVarChar).Value = StationMethod.stationMethodName == null ? DBNull.Value : StationMethod.stationMethodName;
                        cmd.Parameters.Add("@StationMethodCode       ", SqlDbType.VarChar).Value = StationMethod.stationMethodCode == null ? DBNull.Value : StationMethod.stationMethodCode;
                        cmd.Parameters.Add("@stationDataID                     ", SqlDbType.Int).Value = StationMethod.stationDataID == 0 ? DBNull.Value : StationMethod.stationDataID;
                        cmd.Parameters.Add("@description                ", SqlDbType.NVarChar).Value = StationMethod.description == null ? DBNull.Value : StationMethod.description;
                        cmd.Parameters.Add("@StationMethodID            ", SqlDbType.Int).Direction = ParameterDirection.Output;
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
        public IActionResult Update(StationMethod StationMethod) // Sửa
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Update_StationMethod_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@StationMethodName          ", SqlDbType.NVarChar).Value = StationMethod.stationMethodName == null ? DBNull.Value : StationMethod.stationMethodName;
                        cmd.Parameters.Add("@StationMethodCode       ", SqlDbType.VarChar).Value = StationMethod.stationMethodCode == null ? DBNull.Value : StationMethod.stationMethodCode;
                        cmd.Parameters.Add("@stationDataID                     ", SqlDbType.Int).Value = StationMethod.stationDataID == 0 ? DBNull.Value : StationMethod.stationDataID;
                        cmd.Parameters.Add("@description                ", SqlDbType.NVarChar).Value = StationMethod.description == null ? DBNull.Value : StationMethod.description;
                        cmd.Parameters.Add("@StationMethodID            ", SqlDbType.Int).Value = StationMethod.stationMethodID == 0 ? DBNull.Value : StationMethod.stationMethodID;
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
        public void GetListAfterFilter(StationMethodFilter StationMethodFilter)  //hàm gọi khi nhấn trong phần trang,lọc,...
        {
            TempData["OptionFilter"] = JsonConvert.SerializeObject(StationMethodFilter);
        }
        [HttpPost]
        public IActionResult CheckStationMethodByCode(string StationMethodCode = null, int id = 0) //hàm ktra mã trạm đã tồn tại chưa
        {
            bool index = false;
            if (!string.IsNullOrEmpty(StationMethodCode))
            {
                var StationMethod = GetStationMethodByCodeAndOtherID(StationMethodCode, id);
                if (StationMethod.stationMethodID > 0)
                {
                    index = true;
                }
            }
            return Ok(new { data = index });
        }
        public StationMethod GetStationMethodByName(string StationMethodName, int id = 0) // lấy ra trạm quan trắc cùng tên nhưng khác id
        {
            StationMethod StationMethod = new StationMethod();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("StationMethod_GetStationMethodByNameAndOtherID_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@StationMethodName", SqlDbType.NVarChar).Value = StationMethodName == null ? DBNull.Value : StationMethodName;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {

                                StationMethod.stationMethodID = dr.IsDBNull("StationMethodId") == true ? 0 : (int)dr["StationMethodId"];
                                StationMethod.stationMethodName = dr.IsDBNull("StationMethodName") == true ? null : (string)dr["StationMethodName"];
                                StationMethod.stationMethodCode = dr.IsDBNull("StationMethodCode") == true ? null : (string)dr["StationMethodCode"];
                                StationMethod.stationDataID = dr.IsDBNull("monitorStationID") == true ? 0 : (int)dr["monitorStationID"];
                                StationMethod.description = dr.IsDBNull("description") == true ? null : (string)dr["description"];
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
            return StationMethod;
        }
        public StationMethod GetStationMethodByCodeAndOtherID(string code, int id = 0) // lấy ra trạm quan trắc cùng mã nhưng khác id
        {
            StationMethod StationMethod = new StationMethod();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Organ_GetStationMethodByCodeAndOtherID_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@code", SqlDbType.NVarChar).Value = code == null ? DBNull.Value : code.Trim();
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {

                                StationMethod.stationMethodID = dr.IsDBNull("StationMethodId") == true ? 0 : (int)dr["StationMethodId"];
                                StationMethod.stationMethodName = dr.IsDBNull("StationMethodName") == true ? null : (string)dr["StationMethodName"];
                                StationMethod.stationMethodCode = dr.IsDBNull("StationMethodCode") == true ? null : (string)dr["StationMethodCode"];
                                StationMethod.stationDataID = dr.IsDBNull("monitorStationID") == true ? 0 : (int)dr["monitorStationID"];
                                StationMethod.description = dr.IsDBNull("description") == true ? null : (string)dr["description"];
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
            return StationMethod;
        }
    }
}
