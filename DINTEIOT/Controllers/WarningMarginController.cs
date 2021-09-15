using DINTEIOT.Helpers;
using DINTEIOT.Models.WarningMargin;
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
    public interface IWarningMarginController
    {
        public List<WarningMargin> GetWarningMarginByStationDataID(int id = 0);
    }
    public class WarningMarginController : BaseController, IWarningMarginController
    {
        private IStationDataController _istationDataController;
        public WarningMarginController(IStationDataController IStationDataController)
        {
            _istationDataController = IStationDataController;
        }
        public IActionResult Index()
        {
            WarningMarginFilter warningMarginFilter = new WarningMarginFilter();
            if (TempData["OptionFilter"] != null) { warningMarginFilter = JsonConvert.DeserializeObject<WarningMarginFilter>((string)TempData["OptionFilter"]); TempData.Keep(); }
            ViewBag.CurrentPageName = ScreenName.WarningMargin;
            ViewBag.LoaiDuLieu = _istationDataController.GetAllListStationData();
            return View(GetListWarningMargin(warningMarginFilter));
        }
        //lấy danh sách ngưỡng cảnh báo
        public List<WarningMargin> GetListWarningMargin(WarningMarginFilter WarningMarginFilter)
        {
            List<WarningMargin> listWarningMargin = new List<WarningMargin>();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("WarningMargin_GetListWarningMargin_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@pagenumber", SqlDbType.Int).Value = WarningMarginFilter.pagenumber;
                            cmd.Parameters.Add("@pagesize", SqlDbType.Int).Value = WarningMarginFilter.pagesize;
                            cmd.Parameters.Add("@txtsearch", SqlDbType.NVarChar).Value = WarningMarginFilter.txtsearch == null ? DBNull.Value : WarningMarginFilter.txtsearch;
                            cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = WarningMarginFilter.startdate == null ? DBNull.Value : WarningMarginFilter.startdate;
                            cmd.Parameters.Add("@enddate", SqlDbType.DateTime).Value = WarningMarginFilter.enddate == null ? DBNull.Value : WarningMarginFilter.enddate;
                            cmd.Parameters.Add("@stationDataID", SqlDbType.Int).Value = WarningMarginFilter.stationDataID == 0 ? DBNull.Value : WarningMarginFilter.stationDataID;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                WarningMargin WarningMargin = new WarningMargin();
                                WarningMargin.warningMarginID = dr.IsDBNull("WarningMarginId") == true ? 0 : (int)dr["WarningMarginId"];
                                WarningMargin.warningMarginName = dr.IsDBNull("WarningMarginName") == true ? null : (string)dr["WarningMarginName"];
                                WarningMargin.warningMarginValueFrom = dr.IsDBNull("warningMarginValueFrom") == true ? 0 : (int)dr["warningMarginValueFrom"];
                                WarningMargin.warningMarginValueTo = dr.IsDBNull("warningMarginValueTo") == true ? 0 : (int)dr["warningMarginValueTo"];
                                WarningMargin.totalrecord = dr.IsDBNull("totalrecord") == true ? 0 : (int)dr["totalrecord"];
                                WarningMargin.warningMarginValueUnit = dr.IsDBNull("warningMarginValueUnit") == true ? null : (string)dr["warningMarginValueUnit"];
                                WarningMargin.warningMarginValueColor = dr.IsDBNull("warningMarginValueColor") == true ? null : (string)dr["warningMarginValueColor"];
                                listWarningMargin.Add(WarningMargin);
                            }
                            cmd.Dispose();
                            dr.Close();
                            ViewBag.TotalRecord = listWarningMargin.Count() > 0 ? listWarningMargin.First().totalrecord : 0;
                            ViewBag.TotalPage = (ViewBag.TotalRecord / WarningMarginFilter.pagesize) + 1;
                            ViewBag.PageNumber = WarningMarginFilter.pagenumber > 0 ? WarningMarginFilter.pagenumber : 1;
                            ViewBag.PageFirst = WarningMarginFilter.pagefirst > 0 ? WarningMarginFilter.pagefirst : 1;
                        }
                        catch (Exception e)
                        {
                            //throw e;
                        }

                        con.Close();
                    }
                }
            }
            return listWarningMargin;
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.THEM, WebApi.Helpers.Function.ORGAN)]
        public IActionResult insertpre() //check quyền trc khi thêm mới
        {
            return Redirect("/WarningMargin/GetWarningMarginByIDPre");
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.SUA, WebApi.Helpers.Function.ORGAN)]
        public IActionResult updatepre(int id = 0) //check quyền trc khi cập nhật
        {
            return Redirect("/WarningMargin/GetWarningMarginByIDPre?id=" + id);
        }
        public IActionResult GetWarningMarginByIDPre(int id = 0)
        {
            var WarningMargin = GetWarningMarginByID(id);
            if (WarningMargin.warningMarginID < 0)
            {
                return Ok(new { status = (int)ExitCodes.Error, data = "", message = "Lỗi!" });
            }
            
            return Ok(new { status = (int)ExitCodes.Success, data = WarningMargin, message = "" });
        }
        public WarningMargin GetWarningMarginByID(int id = 0)
        {
            WarningMargin warningMargin = new WarningMargin();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("WarningMargin_GetWarningMarginByID_v1", con))
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
                                warningMargin.warningMarginID = dr.IsDBNull("WarningMarginId") == true ? 0 : (int)dr["WarningMarginId"];
                                warningMargin.warningMarginName = dr.IsDBNull("WarningMarginName") == true ? null : (string)dr["WarningMarginName"];
                                warningMargin.warningMarginValueFrom = dr.IsDBNull("warningMarginValueFrom") == true ? 0 : (int)dr["warningMarginValueFrom"];
                                warningMargin.warningMarginValueTo = dr.IsDBNull("warningMarginValueTo") == true ? 0 : (int)dr["warningMarginValueTo"];
                                warningMargin.warningMarginValueUnit = dr.IsDBNull("warningMarginValueUnit") == true ? null : (string)dr["warningMarginValueUnit"];
                                warningMargin.warningMarginValueColor = dr.IsDBNull("warningMarginValueColor") == true ? null : (string)dr["warningMarginValueColor"];
                                warningMargin.stationDataID = dr.IsDBNull("stationDataID") == true ? 0 : (int)dr["stationDataID"];
                                warningMargin.stationDataName = dr.IsDBNull("stationDataName") == true ? null : (string)dr["stationDataName"];
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
            return warningMargin;
        }
        public WarningMargin GetWarningMarginByCode(string code, int id = 0)
        {
            WarningMargin WarningMargin = new WarningMargin();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Organ_GetWarningMarginByCode_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@code", SqlDbType.VarChar).Value = code == null ? DBNull.Value : code;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {

                                WarningMargin.warningMarginID = dr.IsDBNull("WarningMarginId") == true ? 0 : (int)dr["WarningMarginId"];
                                WarningMargin.warningMarginName = dr.IsDBNull("WarningMarginName") == true ? null : (string)dr["WarningMarginName"];
                                WarningMargin.warningMarginValueFrom = dr.IsDBNull("warningMarginValueFrom") == true ? 0 : (int)dr["warningMarginValueFrom"];
                                WarningMargin.warningMarginValueTo = dr.IsDBNull("warningMarginValueTo") == true ? 0 : (int)dr["warningMarginValueTo"];
                                WarningMargin.totalrecord = dr.IsDBNull("totalrecord") == true ? 0 : (int)dr["totalrecord"];
                                WarningMargin.warningMarginValueUnit = dr.IsDBNull("warningMarginValueUnit") == true ? null : (string)dr["warningMarginValueUnit"];
                                WarningMargin.warningMarginValueColor = dr.IsDBNull("warningMarginValueColor") == true ? null : (string)dr["warningMarginValueColor"];
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
            return WarningMargin;
        }

        [HttpPost]
        public IActionResult Insert(WarningMargin warningMargin)  // Thêm mới
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("Insert_WarningMargin_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@WarningMarginName          ", SqlDbType.NVarChar).Value = warningMargin.warningMarginName == null ? DBNull.Value : warningMargin.warningMarginName;
                        cmd.Parameters.Add("@warningMarginValueFrom     ", SqlDbType.Int).Value = warningMargin.warningMarginValueFrom == 0 ? DBNull.Value : warningMargin.warningMarginValueFrom;
                        cmd.Parameters.Add("@stationDataID              ", SqlDbType.Int).Value = warningMargin.stationDataID == 0 ? DBNull.Value : warningMargin.stationDataID;
                        cmd.Parameters.Add("@warningMarginValueTo       ", SqlDbType.Int).Value = warningMargin.warningMarginValueTo == 0 ? DBNull.Value : warningMargin.warningMarginValueTo;
                        cmd.Parameters.Add("@warningMarginValueUnit     ", SqlDbType.VarChar).Value = warningMargin.warningMarginValueUnit == null ? DBNull.Value : warningMargin.warningMarginValueUnit;
                        cmd.Parameters.Add("@warningMarginValueColor     ", SqlDbType.VarChar).Value = warningMargin.warningMarginValueColor == null ? DBNull.Value : warningMargin.warningMarginValueColor;
                        cmd.Parameters.Add("@WarningMarginID            ", SqlDbType.Int).Direction = ParameterDirection.Output;
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
        public IActionResult Update(WarningMargin warningMargin) // Sửa
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Update_WarningMargin_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@WarningMarginName", SqlDbType.NVarChar).Value = warningMargin.warningMarginName == null ? DBNull.Value : warningMargin.warningMarginName;
                        cmd.Parameters.Add("@warningMarginValueFrom", SqlDbType.Int).Value = warningMargin.warningMarginValueFrom == 0 ? DBNull.Value : warningMargin.warningMarginValueFrom;
                        cmd.Parameters.Add("@stationDataID", SqlDbType.Int).Value = warningMargin.stationDataID == 0 ? DBNull.Value : warningMargin.stationDataID;
                        cmd.Parameters.Add("@warningMarginValueTo", SqlDbType.Int).Value = warningMargin.warningMarginValueTo == 0 ? DBNull.Value : warningMargin.warningMarginValueTo;
                        cmd.Parameters.Add("@warningMarginValueUnit", SqlDbType.VarChar).Value = warningMargin.warningMarginValueUnit == null ? DBNull.Value : warningMargin.warningMarginValueUnit;
                        cmd.Parameters.Add("@warningMarginValueColor", SqlDbType.VarChar).Value = warningMargin.warningMarginValueColor == null ? DBNull.Value : warningMargin.warningMarginValueColor;
                        cmd.Parameters.Add("@WarningMarginID", SqlDbType.VarChar).Value = warningMargin.warningMarginID == 0 ? DBNull.Value : warningMargin.warningMarginID;
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
        public void GetListAfterFilter(WarningMarginFilter warningMarginFilter)  //hàm gọi khi nhấn trong phần trang,lọc,...
        {
            TempData["OptionFilter"] = JsonConvert.SerializeObject(warningMarginFilter);
        }
        [HttpPost]
        public IActionResult CheckWarningMarginByName(string warningMarginName = null, int id = 0) //hàm ktra mã loại dl đã tồn tại chưa
        {
            bool index = false;
            if (!string.IsNullOrEmpty(warningMarginName))
            {
                var warningMargin = GetWarningMarginByName(warningMarginName, id);
                if (warningMargin.warningMarginID > 0)
                {
                    index = true;
                }
            }
            return Ok(new { data = index });
        }
        public WarningMargin GetWarningMarginByName(string warningMarginName, int id = 0)
        {
            WarningMargin warningMargin = new WarningMargin();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("WarningMargin_GetWarningMarginByName_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@warningMarginName", SqlDbType.NVarChar).Value = warningMarginName == null ? DBNull.Value : warningMarginName;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {

                                warningMargin.warningMarginID = dr.IsDBNull("WarningMarginId") == true ? 0 : (int)dr["WarningMarginId"];
                                warningMargin.warningMarginName = dr.IsDBNull("WarningMarginName") == true ? null : (string)dr["WarningMarginName"];
                                warningMargin.warningMarginValueFrom = dr.IsDBNull("warningMarginValueFrom") == true ? 0 : (int)dr["warningMarginValueFrom"];
                                warningMargin.warningMarginValueTo = dr.IsDBNull("warningMarginValueTo") == true ? 0 : (int)dr["warningMarginValueTo"];
                                warningMargin.totalrecord = dr.IsDBNull("totalrecord") == true ? 0 : (int)dr["totalrecord"];
                                warningMargin.warningMarginValueUnit = dr.IsDBNull("warningMarginValueUnit") == true ? null : (string)dr["warningMarginValueUnit"];
                                warningMargin.warningMarginValueColor = dr.IsDBNull("warningMarginValueColor") == true ? null : (string)dr["warningMarginValueColor"];

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
            return warningMargin;
        }

        public List<WarningMargin> GetWarningMarginByStationDataID(int id = 0)
        {
            List<WarningMargin> listwarningMargin = new List<WarningMargin>();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("WarningMargin_GetWarningMarginByStationData_v1", con))
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
                                WarningMargin warningMargin = new WarningMargin();
                                warningMargin.warningMarginID = dr.IsDBNull("WarningMarginId") == true ? 0 : (int)dr["WarningMarginId"];
                                warningMargin.warningMarginName = dr.IsDBNull("WarningMarginName") == true ? null : (string)dr["WarningMarginName"];
                                warningMargin.warningMarginValueFrom = dr.IsDBNull("warningMarginValueFrom") == true ? 0 : (int)dr["warningMarginValueFrom"];
                                warningMargin.warningMarginValueTo = dr.IsDBNull("warningMarginValueTo") == true ? 0 : (int)dr["warningMarginValueTo"];
                                warningMargin.warningMarginValueUnit = dr.IsDBNull("warningMarginValueUnit") == true ? null : (string)dr["warningMarginValueUnit"];
                                warningMargin.warningMarginValueColor = dr.IsDBNull("warningMarginValueColor") == true ? null : (string)dr["warningMarginValueColor"];
                                listwarningMargin.Add(warningMargin);
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
            return listwarningMargin;
        }
    }
}
