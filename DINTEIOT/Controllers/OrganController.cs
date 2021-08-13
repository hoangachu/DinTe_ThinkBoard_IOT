using DINTEIOT.Helpers;
using DINTEIOT.Helpers.Common;
using DINTEIOT.Models;
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
    public interface IOrganController
    {
        public List<Organ> GetAllListOrgan();
        public List<TreeViewNode> GetSelectTreeViewNode();
    }
    public class OrganController : BaseController, IOrganController
    {
        public OrganController()
        {

        }
        //[ClaimRequirement(AuthorizeCode.XEM, WebApi.Helpers.Function.ORGAN)]
        public IActionResult Index()
        {
            ViewBag.Organ = GetSelectTreeViewNode();
            //ViewBag.ScreenName = ScreenName.ORGAN;
            //ViewBag.ControlHierachy = GetListControlHierachy();
            return View(GetListOrgan(new OptionFilter()));
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.THEM, WebApi.Helpers.Function.ORGAN)]
        public IActionResult insertpre() //check quyền trc khi thêm mới
        {
            return Redirect("/organ/GetOrganJsonByID");
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.SUA, WebApi.Helpers.Function.ORGAN)]
        public IActionResult updatepre(int id = 0) //check quyền trc khi cập nhật
        {
            return Redirect("/organ/GetOrganJsonByID?id=" + id);
        }
        [HttpPost]
        public IActionResult GetListAfterFilter(OptionFilter optionFilter)
        {
            return PartialView("_PartialIndex", GetListOrgan(optionFilter));
        }

        //lấy danh sách dạng ngang hàng
        public List<TreeViewNode> GetSelectTreeViewNode()
        {
            var listorgan = GetAllListOrgan().Select(x => new TreeViewNode
            {
                id = x.organid,
                parentid = x.organparentid,
                title = x.organname
            }).ToList();
            var listorganparent = listorgan.Where(x => x.parentid == 0 || x.parentid == null).ToList();
            listorganparent.ForEach(x => x.displaynumber = "l1");
            return Common.SelectSortNodesPeer(listorgan, listorganparent);
        }
        //lấy danh sách category
        public List<Organ> GetListOrgan(OptionFilter optionFilter)
        {
            List<Organ> listorgan = new List<Organ>();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Organ_GetListOrgan_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@pagenumber", SqlDbType.Int).Value = optionFilter.pagenumber;
                            cmd.Parameters.Add("@pagesize", SqlDbType.Int).Value = optionFilter.pagesize;
                            cmd.Parameters.Add("@txtsearch", SqlDbType.NVarChar).Value = optionFilter.txtsearch == null ? DBNull.Value : optionFilter.txtsearch;
                            cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = optionFilter.startdate == null ? DBNull.Value : optionFilter.startdate;
                            cmd.Parameters.Add("@enddate", SqlDbType.DateTime).Value = optionFilter.enddate == null ? DBNull.Value : optionFilter.enddate;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                Organ organ = new Organ();
                                organ.organid = dr.IsDBNull("organid") == true ? 0 : (int)dr["organid"];
                                organ.organname = dr.IsDBNull("organname") == true ? "" : (string)dr["organname"];
                                organ.organcode = dr.IsDBNull("organcode") == true ? "" : (string)dr["organcode"];
                                organ.organparentid = dr.IsDBNull("organparentid") == true ? 0 : (int)dr["organparentid"];
                                organ.spokesmanid = dr.IsDBNull("spokesmanid") == true ? 0 : (int)dr["spokesmanid"];
                                organ.url = dr.IsDBNull("url") == true ? "" : (string)dr["url"];
                                organ.fax = dr.IsDBNull("fax") == true ? "" : (string)dr["fax"];
                                organ.email = dr.IsDBNull("email") == true ? "" : (string)dr["email"];
                                organ.address = dr.IsDBNull("address") == true ? "" : (string)dr["address"];
                                organ.phonenumber = dr.IsDBNull("phonenumber") == true ? 0 : (int)dr["phonenumber"];
                                organ.createdate = dr.IsDBNull("createdate") == true ? DateTime.Now : (DateTime)dr["createdate"];
                                //organ.rownumber = dr.IsDBNull("rownumber") == true ? 0 : (long)dr["rownumber"];
                                listorgan.Add(organ);

                            }
                            cmd.Dispose();
                            dr.Close();
                            //ViewBag.TotalRecordCurrent = listArticle.Count();
                            //ViewBag.TotalRecord = GetTotalArticle();
                            ViewBag.TotalPage = (listorgan.Count() / optionFilter.pagesize) + 1;
                            ViewBag.Currentpage = optionFilter.pagenumber;
                            ViewBag.ScreenName = ScreenName.Organ;
                            ViewBag.PageSize = optionFilter.pagesize;

                        }
                        catch (Exception e)
                        {
                            //throw e;
                        }

                        con.Close();
                    }
                }
            }
            return listorgan;
        }
        public List<Organ> GetAllListOrgan()
        {
            List<Organ> listorgan = new List<Organ>();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Organ_GetAllListOrgan_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                Organ organ = new Organ();
                                organ.organid = dr.IsDBNull("organid") == true ? 0 : (int)dr["organid"];
                                organ.organname = dr.IsDBNull("organname") == true ? "" : (string)dr["organname"];
                                organ.organparentid = dr.IsDBNull("organparentid") == true ? 0 : (int)dr["organparentid"];
                                organ.email = dr.IsDBNull("email") == true ? "" : (string)dr["email"];
                                organ.address = dr.IsDBNull("address") == true ? "" : (string)dr["address"];
                                organ.phonenumber = dr.IsDBNull("phonenumber") == true ? 0 : (int)dr["phonenumber"];
                                listorgan.Add(organ);

                            }
                            cmd.Dispose();
                            dr.Close();
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }

                        con.Close();
                    }
                }
            }
            return listorgan;
        }
        //lấy danh sách category json đổ vào jxgrid
        #region
        public IActionResult GetJsonDataTreeGrid()
        {
            var data = GetAllListOrgan().Select(x => new GridTree { ID = x.organid, Name = x.organname, ParentID = x.organparentid }).ToList();
            return Ok(new { data = JsonConvert.SerializeObject(data) });
        }
        //public List<CategoryTree> GetListTreeViewNode()
        //{
        //    var listarticlecategory = new ArticleCategoryController().GetListCategory().Select(x => new CategoryTree
        //    {
        //        CategoryID = x.category_id,
        //        ReportsTo = x.categoryparent_id,
        //        CategoryName = x.category_name
        //    }).ToList();
        //    //var listarticlecategoryparent = listarticlecategory.Where(x => x.ReportsTo == 0 || x.ReportsTo == null).ToList();
        //    return listarticlecategory;
        //}
        #endregion

        [HttpPost]
        public IActionResult Insert(Organ organ)
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("Insert_Organ_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@organname", SqlDbType.NVarChar).Value = organ.organname == null ? DBNull.Value : organ.organname;
                        cmd.Parameters.Add("@organparentid", SqlDbType.Int).Value = organ.organparentid == 0 ? DBNull.Value : organ.organparentid;
                        cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = organ.address == null ? DBNull.Value : organ.address;
                        cmd.Parameters.Add("@phonenumber", SqlDbType.Int).Value = organ.phonenumber == 0 ? DBNull.Value : organ.address;
                        cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = organ.email == null ? DBNull.Value : organ.email;
                        cmd.Parameters.Add("@organid", SqlDbType.Int).Direction = ParameterDirection.Output;
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
        public IActionResult Update(Organ organ)
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Update_Organ_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@organname", SqlDbType.NVarChar).Value = organ.organname == null ? DBNull.Value : organ.organname;
                        cmd.Parameters.Add("@organparentid", SqlDbType.Int).Value = organ.organparentid == 0 ? DBNull.Value : organ.organparentid;
                        cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = organ.address == null ? DBNull.Value : organ.address;
                        cmd.Parameters.Add("@phonenumber", SqlDbType.Int).Value = organ.phonenumber == 0 ? DBNull.Value : organ.address;
                        cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = organ.email == null ? DBNull.Value : organ.email;
                        cmd.Parameters.Add("@organid", SqlDbType.Int).Value = organ.organid;
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
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.XOA, WebApi.Helpers.Function.ORGAN)]
        public IActionResult Delete(int organid)
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Delete_Organ_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@organid", SqlDbType.Int).Value = organid;
                        con.Open();
                        i = cmd.ExecuteNonQuery();

                    }
                }

            }
            catch (Exception e)
            {
                return Ok(new { status = (int)ExitCodes.Error, data = i, message = e.Message });
            }
            return Ok(new { status = (int)ExitCodes.Success, data = i, message = "Xóa thành công" });

        }
        public Organ GetOrganByID(int id)
        {
            Organ organ = new Organ();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Organ_GetOrganByID_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@organid", SqlDbType.Int).Value = id;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {

                                organ.organid = dr.IsDBNull("organid") == true ? 0 : (int)dr["organid"];
                                organ.organname = dr.IsDBNull("organname") == true ? "" : (string)dr["organname"];
                                organ.email = dr.IsDBNull("email") == true ? "" : (string)dr["email"];
                                organ.address = dr.IsDBNull("address") == true ? "" : (string)dr["address"];
                                organ.phonenumber = dr.IsDBNull("phonenumber") == true ? 0 : (int)dr["phonenumber"];
                                organ.organparentid = dr.IsDBNull("organparentid") == true ? 0 : (int)dr["organparentid"];
                                organ.organparentname = dr.IsDBNull("organparentname") == true ? "Không thuộc đơn vị nào" : (string)dr["organparentname"];
                                break;
                            }
                            cmd.Dispose();
                            dr.Close();
                        }
                        catch (Exception e)
                        {
                        }
                        con.Close();
                    }
                }
            }
            return organ;
        }
        public IActionResult GetOrganJsonByID(int id)
        {
            return Ok(new { status = (int)ExitCodes.Success, data = JsonConvert.SerializeObject(GetOrganByID(id)) });
        }
        //Lấy danh sách cấp quản lý
        public List<ControlHierachy> GetListControlHierachy()
        {
            List<ControlHierachy> listcontrolHierachies = new List<ControlHierachy>();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                //using (SqlCommand cmd = new SqlCommand("ControlHierachy_GetListControlHierachy_v1", con))
                //{
                //    {
                //        try
                //        {
                //            con.Open();
                //            cmd.CommandType = CommandType.StoredProcedure;
                //            SqlDataReader dr = cmd.ExecuteReader();

                //            while (dr.Read())
                //            {
                //                ControlHierachy controlhierachy = new ControlHierachy();
                //                controlhierachy.controlhierarchyid = dr.IsDBNull("ControlhierarchyID") == true ? 0 : (int)dr["ControlhierarchyID"];
                //                controlhierachy.controlhierarchyname = dr.IsDBNull("ControlhierarchyName") == true ? "" : (string)dr["ControlhierarchyName"];
                //                listcontrolHierachies.Add(controlhierachy);

                //            }
                //            cmd.Dispose();
                //            dr.Close();
                //            //ViewBag.TotalRecordCurrent = listArticle.Count();
                //            //ViewBag.TotalRecord = GetTotalArticle();


                //        }
                //        catch (Exception e)
                //        {
                //            throw e;
                //        }

                //        con.Close();
                //    }
                //}
            }
            return listcontrolHierachies;
        }
        [HttpPost]
        public IActionResult CheckOrganByName(string organName, int id = 0) //hàm ktra tên khối tin đã tồn tại chưa
        {
            bool index = false;
            if (!string.IsNullOrEmpty(organName))
            {
                var organ = GetOrganByName(organName, id);
                if (organ.organid > 0)
                {
                    index = true;
                }
            }
            return Ok(new { data = index });
        }
        public Organ GetOrganByName(string organName, int id) // lấy cơ quan theo tên
        {
            Organ organ = new Organ();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Organ_GetOrganByName_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@organName", SqlDbType.NVarChar).Value = organName == null ? DBNull.Value : organName;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                organ.organid = dr.IsDBNull("organid") == true ? 0 : (int)dr["organid"];
                                organ.organname = dr.IsDBNull("organname") == true ? "" : (string)dr["organname"];
                                organ.organcode = dr.IsDBNull("organcode") == true ? "" : (string)dr["organcode"];
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
            return organ;
        }
        [HttpPost]
        public IActionResult CheckOrganByCode(string organcode, int id = 0) //hàm ktra tên khối tin đã tồn tại chưa
        {
            bool index = false;
            if (!string.IsNullOrEmpty(organcode))
            {
                var organ = GetOrganByCode(organcode, id);
                if (organ.organid > 0)
                {
                    index = true;
                }
            }
            return Ok(new { data = index });
        }
        public Organ GetOrganByCode(string organCode, int id) // lấy khối tin theo tên
        {
            Organ organ = new Organ();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Organ_GetOrganByCode_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@organCode", SqlDbType.NVarChar).Value = organCode == null ? DBNull.Value : organCode;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                organ.organid = dr.IsDBNull("organid") == true ? 0 : (int)dr["organid"];
                                organ.organname = dr.IsDBNull("organname") == true ? "" : (string)dr["organname"];
                                organ.organcode = dr.IsDBNull("organcode") == true ? "" : (string)dr["organcode"];
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
            return organ;
        }
    }
}
