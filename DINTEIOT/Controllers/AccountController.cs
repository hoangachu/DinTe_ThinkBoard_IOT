using DINTEIOT;
using DINTEIOT.Controllers;
using DINTEIOT.Helpers;
using DINTEIOT.Models;
using DINTEIOT.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace GovermentPortal.Areas.Admin.Controllers
{
   
    public interface IAccountController
    {
        //User GetUserByUserID(int UserID);
    }
    public class AccountController : BaseController, IAccountController
    {
        private IOrganController _iorganController;
        public AccountController(IConfiguration IConfiguration, IOrganController IOrganController)
        {
            _iorganController = IOrganController;
            //_configuration = IConfiguration;
            //_iauthorizeController = IAuthorizeController;
        }
        [HttpGet]
        public IActionResult Index()
        {
            AccountFilter accountFilter = new AccountFilter();
            if (TempData["OptionFilter"] != null) { accountFilter = JsonConvert.DeserializeObject<AccountFilter>((string)TempData["OptionFilter"]); TempData.Keep(); }
            ViewBag.CurrentPageName = ScreenName.Account;
            ViewBag.CoQuan = _iorganController.GetSelectTreeViewNode();
            return View(GetListAccount(accountFilter));
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }
        //public IActionResult ListAccount()  // lấy danh sách người dùng
        //{
        //    return View(GetListAccount(new AccountFilter()));
        //}
        [HttpGet]
        public List<Account> GetListAccount(AccountFilter accountFilter)
        {
            List<Account> listUser = new List<Account>();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("User_GetListAccount_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@pagenumber", SqlDbType.Int).Value = accountFilter.pagenumber;
                            cmd.Parameters.Add("@pagesize", SqlDbType.Int).Value = accountFilter.pagesize;
                            cmd.Parameters.Add("@txtsearch", SqlDbType.NVarChar).Value = accountFilter.txtsearch == null ? DBNull.Value : accountFilter.txtsearch;
                            cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = accountFilter.startdate == null ? DBNull.Value : accountFilter.startdate;
                            cmd.Parameters.Add("@enddate", SqlDbType.DateTime).Value = accountFilter.enddate == null ? DBNull.Value : accountFilter.enddate;
                            cmd.Parameters.Add("@organID", SqlDbType.Int).Value = accountFilter.organID == 0 ? DBNull.Value : accountFilter.organID;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                Account account = new Account();
                                account.accountID = dr.IsDBNull("accountid") == true ? 0 : (int)dr["accountid"];
                                account.username = dr.IsDBNull("Username") == true ? "" : (string)dr["Username"];
                                account.fullName = dr.IsDBNull("fullName") == true ? "" : (string)dr["fullName"];
                                account.totalrecord = dr.IsDBNull("totalrecord") == true ? 0 : (int)dr["totalrecord"];
                                account.status = dr.IsDBNull("status") == true ? 0 : (int)dr["status"];
                                account.organ = new Organ();
                                account.organ.organname = dr.IsDBNull("organname") == true ? "" : (string)dr["organname"];
                                account.phoneNumber = dr.IsDBNull("phonenumber") == true ? 0 : (int)dr["phonenumber"];
                                account.email = dr.IsDBNull("email") == true ? "" : (string)dr["email"];
                                listUser.Add(account);

                            }
                            cmd.Dispose();
                            dr.Close();
                            ViewBag.TotalRecord = listUser.Count() > 0 ? listUser.First().totalrecord : 0;
                            ViewBag.TotalPage = (ViewBag.TotalRecord / accountFilter.pagesize) + 1;
                            ViewBag.PageNumber = accountFilter.pagenumber > 0 ? accountFilter.pagenumber : 1;
                            ViewBag.PageFirst = accountFilter.pagefirst > 0 ? accountFilter.pagefirst : 1;

                        }
                        catch (Exception e)
                        {
                            //throw e;
                        }

                        con.Close();
                    }


                }

            }
            return listUser;
        }
 

        //public User GetUserByUserID(int UserID)
        //{
        //    User User = new User();
        //    using (SqlConnection con = new SqlConnection(Startup.connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("User_GetUserByID_v1", con))
        //        {
        //            {
        //                try
        //                {
        //                    con.Open();
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
        //                    SqlDataReader dr = cmd.ExecuteReader();

        //                    while (dr.Read())
        //                    {
        //                        User.userId = (int)dr["UserId"];
        //                        User.username = dr.IsDBNull("Username") == true ? "" : (string)dr["Username"];
        //                        User.fullName = dr.IsDBNull("fullName") == true ? "" : (string)dr["fullName"];
        //                        User.status = dr.IsDBNull("status") == true ? 0 : (int)dr["status"];
        //                        User.roleid = dr.IsDBNull("roleid") == true ? 0 : (int)dr["roleid"];
        //                        User.organ = new Models.Organ();
        //                        User.organ.organname = dr.IsDBNull("organname") == true ? "" : (string)dr["organname"];
        //                        User.phonenumber = dr.IsDBNull("phonenumber") == true ? 0 : (int)dr["phonenumber"];
        //                        User.email = dr.IsDBNull("email") == true ? "" : (string)dr["email"];
        //                        break;
        //                    }
        //                    cmd.Dispose();
        //                    dr.Close();
        //                }
        //                catch (Exception e)
        //                {
        //                    //throw e;
        //                }

        //                con.Close();
        //            }
        //        }
        //    }
        //    return User;
        //}
   
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
        public List<Account> GetAllListAccount()
        {
            List<Account> listAccount = new List<Account>();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Account_GetAllListAccount_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                Account account = new Account();
                                account.accountID = dr.IsDBNull("UserId") == true ? 0 : (int)dr["UserId"];
                                account.username = dr.IsDBNull("Username") == true ? "" : (string)dr["Username"];
                                account.fullName = dr.IsDBNull("fullName") == true ? "" : (string)dr["fullName"];
                                account.status = dr.IsDBNull("status") == true ? 0 : (int)dr["status"];
                                account.roleid = dr.IsDBNull("roleid") == true ? 0 : (int)dr["roleid"];
                                account.organ = new Organ();
                                account.organ.organname = dr.IsDBNull("organname") == true ? "" : (string)dr["organname"];
                                account.phoneNumber = dr.IsDBNull("phonenumber") == true ? 0 : (int)dr["phonenumber"];
                                account.email = dr.IsDBNull("email") == true ? "" : (string)dr["email"];
                                listAccount.Add(account);
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
            return listAccount;
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.THEM, WebApi.Helpers.Function.ORGAN)]
        public IActionResult insertpre() //check quyền trc khi thêm mới
        {
            return Redirect("/account/GetAccountByIDPre");
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.SUA, WebApi.Helpers.Function.ORGAN)]
        public IActionResult updatepre(int id = 0) //check quyền trc khi cập nhật
        {
            return Redirect("/account/GetAccountByIDPre?id=" + id);
        }
        public IActionResult GetAccountByIDPre(int id = 0)
        {
            var account = GetAccountByID(id);
            if (account.accountID < 0)
            {
                return Ok(new { status = (int)ExitCodes.Error, data = "", message = "Lỗi!" });
            }

            return Ok(new { status = (int)ExitCodes.Success, data = account, message = "" });
        }
        public Account GetAccountByID(int id = 0)
        {
            Account account = new Account();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Account_GetAccountByID_v1", con))
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
                                account.accountID = dr.IsDBNull("accountID") == true ? 0 : (int)dr["accountID"];
                                account.username = dr.IsDBNull("Username") == true ? null : (string)dr["Username"];
                                account.fullName = dr.IsDBNull("fullName") == true ? null : (string)dr["fullName"];
                                account.status = dr.IsDBNull("status") == true ? 0 : (int)dr["status"];
                                account.organ = new Organ();
                                account.organ.organid = dr.IsDBNull("organid") == true ? 0 : (int)dr["organid"];
                                account.organ.organname = dr.IsDBNull("organname") == true ? null : (string)dr["organname"];
                                account.phoneNumber = dr.IsDBNull("phonenumber") == true ? 0 : (int)dr["phonenumber"];
                                account.email = dr.IsDBNull("email") == true ? "" : (string)dr["email"];
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
            return account;
        }


        [HttpPost]
        public IActionResult Insert(Account account,int organid = 0)  // Thêm mới
        {
            var i = 0;
            var password = "123456";
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("Insert_Account_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@fullName          ", SqlDbType.NVarChar).Value = account.fullName == null ? DBNull.Value : account.username;
                        cmd.Parameters.Add("@username       ", SqlDbType.VarChar).Value = account.username == null ? DBNull.Value : account.username;
                        cmd.Parameters.Add("@organID                     ", SqlDbType.Int).Value = organid == 0 ? DBNull.Value : organid;
                        cmd.Parameters.Add("@phonenumber              ", SqlDbType.Int).Value = account.phoneNumber == 0 ? DBNull.Value : account.phoneNumber;
                        cmd.Parameters.Add("@email                ", SqlDbType.VarChar).Value = account.email == null ? DBNull.Value : account.email;
                        cmd.Parameters.Add("@password                ", SqlDbType.VarChar).Value = LoginHelper.LoginHelper.Encrypt(password);
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
        public IActionResult Update(Account account,int organid = 0) // Sửa
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Update_Account_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@fullName          ", SqlDbType.NVarChar).Value = account.fullName == null ? DBNull.Value : account.username;
                        cmd.Parameters.Add("@username       ", SqlDbType.VarChar).Value = account.username == null ? DBNull.Value : account.username;
                        cmd.Parameters.Add("@organID                     ", SqlDbType.Int).Value = organid == 0 ? DBNull.Value : organid;
                        cmd.Parameters.Add("@phonenumber              ", SqlDbType.Int).Value = account.phoneNumber == 0 ? DBNull.Value : account.phoneNumber;
                        cmd.Parameters.Add("@email                ", SqlDbType.VarChar).Value = account.email == null ? DBNull.Value : account.email;
                        cmd.Parameters.Add("@accountid                ", SqlDbType.Int).Value = account.accountID == 0 ? DBNull.Value : account.accountID;
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
        public void GetListAfterFilter(AccountFilter accountFilter)  //hàm gọi khi nhấn trong phần trang,lọc,...
        {
            TempData["OptionFilter"] = JsonConvert.SerializeObject(accountFilter);
        }
        [HttpPost]
        public IActionResult CheckAccountByUserName(string userName = null, int id = 0) //hàm ktra nd cùng tên đăng nhập
        {
            bool index = false;
            if (!string.IsNullOrEmpty(userName))
            {
                var account = GetAccountByUserNameAndOtherID(userName, id);
                if (account.accountID > 0)
                {
                    index = true;
                }
            }
            return Ok(new { data = index });
        }
        public Account GetAccountByUserNameAndOtherID(string userName, int id = 0) // lấy ra nd cùng tên tk
        {
            Account account = new Account();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Account_GetAccountByUserNameAndOtherID_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@userName", SqlDbType.VarChar).Value = userName == null ? DBNull.Value : userName.Trim();
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                account.accountID = dr.IsDBNull("accountID") == true ? 0 : (int)dr["accountID"];
                                account.username = dr.IsDBNull("Username") == true ? "" : (string)dr["Username"];
                                account.fullName = dr.IsDBNull("fullName") == true ? "" : (string)dr["fullName"];
                                account.status = dr.IsDBNull("status") == true ? 0 : (int)dr["status"];
                                account.phoneNumber = dr.IsDBNull("phonenumber") == true ? 0 : (int)dr["phonenumber"];
                                account.email = dr.IsDBNull("email") == true ? "" : (string)dr["email"];
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
            return account;
        }
        [HttpGet]
        public IActionResult UpdateAccountStatus(int id, int status = 0) // cập nhật trạng thái tài khoản (Khóa or ko khóa)
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Update_StatusAccount_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id          ", SqlDbType.Int).Value = id == 0 ? DBNull.Value : id;
                        cmd.Parameters.Add("@status                     ", SqlDbType.Int).Value = status == 0 ? DBNull.Value : status;
                        con.Open();
                        i = cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception e)
            {
                return Ok(new { status = (int)ExitCodes.Error, data = i, message = e.Message });
            }
            return Ok(new { status = (int)ExitCodes.Success, data = i, message = "Cập nhật trạng thái thành công" });
        }
        [HttpGet]
        //[ClaimRequirement(AuthorizeCode.XOA, WebApi.Helpers.Function.ORGAN)]
        public IActionResult Delete(int id)        // Xóa người dùng
        {
            var i = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Delete_Account_v1", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
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
        //[HttpPost]
        //public IActionResult CheckMonitorStationByCode(string monitorStationCode = null, int id = 0) //hàm ktra mã trạm đã tồn tại chưa
        //{
        //    bool index = false;
        //    if (!string.IsNullOrEmpty(monitorStationCode))
        //    {
        //        var MonitorStation = GetMonitorStationByCodeAndOtherID(monitorStationCode, id);
        //        if (MonitorStation.monitorStationID > 0)
        //        {
        //            index = true;
        //        }
        //    }
        //    return Ok(new { data = index });
        //}
        //public MonitorStation GetMonitorStationByName(string MonitorStationName, int id = 0) // lấy ra trạm quan trắc cùng tên nhưng khác id
        //{
        //    MonitorStation MonitorStation = new MonitorStation();
        //    using (SqlConnection con = new SqlConnection(Startup.connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("MonitorStation_GetMonitorStationByNameAndOtherID_v1", con))
        //        {
        //            {
        //                try
        //                {
        //                    con.Open();
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Add("@MonitorStationName", SqlDbType.NVarChar).Value = MonitorStationName == null ? DBNull.Value : MonitorStationName;
        //                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        //                    SqlDataReader dr = cmd.ExecuteReader();

        //                    while (dr.Read())
        //                    {

        //                        MonitorStation.monitorStationID = dr.IsDBNull("MonitorStationId") == true ? 0 : (int)dr["MonitorStationId"];
        //                        MonitorStation.monitorStationName = dr.IsDBNull("MonitorStationName") == true ? null : (string)dr["MonitorStationName"];
        //                        MonitorStation.monitorStationCode = dr.IsDBNull("monitorStationCode") == true ? null : (string)dr["monitorStationCode"];
        //                        MonitorStation.latitude = dr.IsDBNull("MonitorStationValueTo") == true ? 0 : (int)dr["latitude"];
        //                        MonitorStation.longitude = dr.IsDBNull("longitude") == true ? 0 : (int)dr["longitude"];
        //                        MonitorStation.siteAddress = dr.IsDBNull("siteAddress") == true ? null : (string)dr["siteAddress"];
        //                        MonitorStation.address = dr.IsDBNull("address") == true ? null : (string)dr["address"];
        //                        MonitorStation.description = dr.IsDBNull("description") == true ? null : (string)dr["description"];

        //                        break;
        //                    }
        //                    cmd.Dispose();
        //                    dr.Close();
        //                }
        //                catch (Exception e)
        //                {
        //                    //throw e;
        //                }

        //                con.Close();
        //            }
        //        }
        //    }
        //    return MonitorStation;
        //}
        //public MonitorStation GetMonitorStationByCodeAndOtherID(string code, int id = 0) // lấy ra trạm quan trắc cùng mã nhưng khác id
        //{
        //    MonitorStation MonitorStation = new MonitorStation();
        //    using (SqlConnection con = new SqlConnection(Startup.connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("Organ_GetMonitorStationByCodeAndOtherID_v1", con))
        //        {
        //            {
        //                try
        //                {
        //                    con.Open();
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Add("@code", SqlDbType.VarChar).Value = code == null ? DBNull.Value : code.Trim();
        //                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        //                    SqlDataReader dr = cmd.ExecuteReader();

        //                    while (dr.Read())
        //                    {

        //                        MonitorStation.monitorStationID = dr.IsDBNull("MonitorStationId") == true ? 0 : (int)dr["MonitorStationId"];
        //                        MonitorStation.monitorStationName = dr.IsDBNull("MonitorStationName") == true ? null : (string)dr["MonitorStationName"];
        //                        MonitorStation.monitorStationCode = dr.IsDBNull("monitorStationCode") == true ? null : (string)dr["monitorStationCode"];
        //                        MonitorStation.latitude = dr.IsDBNull("MonitorStationValueTo") == true ? 0 : (int)dr["latitude"];
        //                        MonitorStation.longitude = dr.IsDBNull("longitude") == true ? 0 : (int)dr["longitude"];
        //                        MonitorStation.siteAddress = dr.IsDBNull("siteAddress") == true ? null : (string)dr["siteAddress"];
        //                        MonitorStation.address = dr.IsDBNull("address") == true ? null : (string)dr["address"];
        //                        MonitorStation.description = dr.IsDBNull("description") == true ? null : (string)dr["description"];
        //                        break;
        //                    }
        //                    cmd.Dispose();
        //                    dr.Close();
        //                }
        //                catch (Exception e)
        //                {
        //                    //throw e;
        //                }

        //                con.Close();
        //            }
        //        }
        //    }
        //    return MonitorStation;
        //}
        //public IActionResult ListUser()
        //{
        //    return View(GetListUser(new OptionFilter()));
        //}
        //[HttpGet]
        //public IActionResult CreateOrEdit(int id)
        //{
        //    var model = new User();
        //    model.organ = new Models.Organ();
        //    if (id > 0)
        //    {
        //        model = GetUserByUserID(id);
        //        //ViewBag.ListFileMedia = _ifileController.GetListFileUpload(model.mediaradio, model.ArticleID);
        //    }
        //    return View(model);
        //}
        //[HttpGet]
        //public List<User> GetListUser(OptionFilter optionFilter)
        //{
        //    List<User> listUser = new List<User>();
        //    using (SqlConnection con = new SqlConnection(Startup.connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("User_GetListUser_v1", con))
        //        {
        //            {
        //                try
        //                {
        //                    con.Open();
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Add("@pagenumber", SqlDbType.Int).Value = optionFilter.pagenumber;
        //                    cmd.Parameters.Add("@pagesize", SqlDbType.Int).Value = optionFilter.pagesize;
        //                    cmd.Parameters.Add("@txtsearch", SqlDbType.NVarChar).Value = optionFilter.txtsearch == null ? DBNull.Value : optionFilter.txtsearch;
        //                    cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = optionFilter.startdate == null ? DBNull.Value : optionFilter.startdate;
        //                    cmd.Parameters.Add("@enddate", SqlDbType.DateTime).Value = optionFilter.enddate == null ? DBNull.Value : optionFilter.enddate;
        //                    SqlDataReader dr = cmd.ExecuteReader();

        //                    while (dr.Read())
        //                    {
        //                        User user = new User();
        //                        user.userId = dr.IsDBNull("UserId") == true ? 0 : (int)dr["UserId"];
        //                        user.username = dr.IsDBNull("Username") == true ? "" : (string)dr["Username"];
        //                        user.fullName = dr.IsDBNull("fullName") == true ? "" : (string)dr["fullName"];
        //                        user.totalrecord = dr.IsDBNull("totalrecord") == true ? 0 : (int)dr["totalrecord"];
        //                        user.rownumber = dr.IsDBNull("rownumber") == true ? 0 : (long)dr["rownumber"];
        //                        user.status = dr.IsDBNull("status") == true ? 0 : (int)dr["status"];
        //                        user.roleid = dr.IsDBNull("roleid") == true ? 0 : (int)dr["roleid"];
        //                        user.organ = new Models.Organ();
        //                        user.organ.organname = dr.IsDBNull("organname") == true ? "" : (string)dr["organname"];
        //                        user.phonenumber = dr.IsDBNull("phonenumber") == true ? 0 : (int)dr["phonenumber"];
        //                        user.email = dr.IsDBNull("email") == true ? "" : (string)dr["email"];
        //                        listUser.Add(user);

        //                    }
        //                    cmd.Dispose();
        //                    dr.Close();
        //                    ViewBag.TotalRecordCurrent = listUser.Count();
        //                    ViewBag.TotalRecord = listUser.Count() > 0 ? listUser.First().totalrecord : 0;
        //                    ViewBag.TotalPage = (ViewBag.TotalRecord / optionFilter.pagesize) + 1;
        //                    ViewBag.Currentpage = optionFilter.pagenumber;
        //                    ViewBag.PageSize = optionFilter.pagesize;
        //                    ViewBag.ScreenName = ScreenName.USER;


        //                }
        //                catch (Exception e)
        //                {
        //                    //throw e;
        //                }

        //                con.Close();
        //            }


        //        }

        //    }
        //    return listUser;
        //}
        //[HttpPost]
        //public IActionResult GetListAfterFilter(OptionFilter optionFilter)
        //{
        //    return PartialView("_PartialIndex", GetListUser(optionFilter));
        //}

        //[HttpPost]
        //public IActionResult Insert(User user)
        //{
        //    var i = 0;
        //    if (string.IsNullOrEmpty(user.password))
        //    {
        //        user.password = "12345";
        //    }
        //    user.password = LoginHelper.LoginHelper.Encrypt(user.password);
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Startup.connectionString))
        //        {

        //            using (SqlCommand cmd = new SqlCommand("Insert_User_v1", con))
        //            {
        //                cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //                cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = user.username == null ? DBNull.Value : user.username;
        //                cmd.Parameters.Add("@fullName", SqlDbType.NVarChar).Value = user.fullName == null ? DBNull.Value : user.fullName;
        //                cmd.Parameters.Add("@roleid", SqlDbType.NVarChar).Value = user.roleid == 0 ? DBNull.Value : user.roleid;
        //                cmd.Parameters.Add("@phonenumber", SqlDbType.Int).Value = user.phonenumber == 0 ? DBNull.Value : user.phonenumber;
        //                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = user.email == null ? DBNull.Value : user.email;
        //                cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = user.password == null ? DBNull.Value : user.password;

        //                //cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = user.username == null ? DBNull.Value : user.username;

        //                con.Open();
        //                i = cmd.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(new { data = i, message = e.Message });
        //    }

        //    return Ok(new { data = i, message = "Thêm mới thành công" });

        //}
        //public IActionResult Update(User user)
        //{
        //    var i = 0;
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Startup.connectionString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("Update_User_v1", con))
        //            {
        //                cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = user.username == null ? DBNull.Value : user.username;
        //                cmd.Parameters.Add("@fullName", SqlDbType.NVarChar).Value = user.fullName == null ? DBNull.Value : user.fullName;
        //                cmd.Parameters.Add("@roleid", SqlDbType.NVarChar).Value = user.roleid == 0 ? DBNull.Value : user.roleid;
        //                cmd.Parameters.Add("@phonenumber", SqlDbType.Int).Value = user.phonenumber == 0 ? DBNull.Value : user.phonenumber;
        //                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = user.email == null ? DBNull.Value : user.email;
        //                cmd.Parameters.Add("@userid", SqlDbType.NVarChar).Value = user.userId == 0 ? DBNull.Value : user.userId;
        //                con.Open();
        //                i = cmd.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(new { data = i, message = e.Message });
        //    }
        //    return Ok(new { data = i, message = "Cập nhật thành công" });
        //}
        //[HttpPost]
        //public IActionResult CheckUserName(string username)
        //{
        //    var user = GetUserByUserName(username);
        //    if (user.userId > 0)
        //    {
        //        return Ok(new { data = 0, message = "Tên đăng nhập đã tồn tại" });
        //    }
        //    return Ok(new { data = 1, message = "Tên đăng nhập đã tồn tại" });
        //}
        //public User GetUserByUserName(string username)
        //{
        //    User User = new User();
        //    using (SqlConnection con = new SqlConnection(Startup.connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("User_GetUserByUserName_v1", con))
        //        {
        //            {
        //                try
        //                {
        //                    con.Open();
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username == null ? DBNull.Value : username;
        //                    SqlDataReader dr = cmd.ExecuteReader();

        //                    while (dr.Read())
        //                    {
        //                        User.userId = (int)dr["UserId"];
        //                        User.username = dr.IsDBNull("Username") == true ? "" : (string)dr["Username"];
        //                        User.phonenumber = dr.IsDBNull("phonenumber") == true ? 0 : (int)dr["phonenumber"];
        //                        User.email = dr.IsDBNull("email") == true ? "" : (string)dr["email"];
        //                        break;
        //                    }
        //                    cmd.Dispose();
        //                    dr.Close();
        //                }
        //                catch (Exception e)
        //                {
        //                    //throw e;
        //                }

        //                con.Close();
        //            }
        //        }
        //    }
        //    return User;
        //}
        //[HttpPost]
        //public IActionResult UpdateAccountStatus(int accountid, int status)
        //{
        //    var i = 0;
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Startup.connectionString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("Update_UpdateAccountStatus_v1", con))
        //            {
        //                cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@accountid", SqlDbType.Int).Value = accountid == 0 ? DBNull.Value : accountid;
        //                cmd.Parameters.Add("@status", SqlDbType.Int).Value = status == 0 ? DBNull.Value : status;
        //                con.Open();
        //                i = cmd.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(new { data = i, message = e.Message });
        //    }
        //    return Ok(new { data = i, message = "Cập nhật trạng thái tài khoản thành công" });
        //}
        //[HttpPost]
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return Redirect("/admin/account");
        //}
        //[HttpPost]
        //public IActionResult DeleteMulti(string listid) //hàm cho phép xóa nhiều 
        //{
        //    var index = 0;
        //    List<int> listuserid = new List<int>();
        //    if (!string.IsNullOrEmpty(listid)) { listuserid = listid.Split(",").Select(x => Convert.ToInt32(x)).ToList(); }
        //    else { return Ok(new { data = index, message = "Bạn chưa chọn đối tượng nào" }); }
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Startup.connectionString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("Delete_DeleteMultiUser_v1", con))
        //            {
        //                cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //                var table = new DataTable();
        //                table.Columns.Add("ID", typeof(int));
        //                foreach (var item in listuserid)
        //                    table.Rows.Add(item);
        //                var pList = new SqlParameter("@list", SqlDbType.Structured);
        //                pList.TypeName = "dbo.ListID";
        //                pList.Value = table;
        //                cmd.Parameters.Add(pList);
        //                con.Open();
        //                index = cmd.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { data = index, message = ex.Message });
        //    }

        //    return Ok(new { data = index });
        //}
        //[HttpPost]
        //public IActionResult UpdateAccountStatusMulti(string listid, int status)
        //{
        //    var i = 0;
        //    List<int> listuserid = new List<int>();
        //    if (!string.IsNullOrEmpty(listid)) { listuserid = listid.Split(",").Select(x => Convert.ToInt32(x)).ToList(); }
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Startup.connectionString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("Update_UpdateAccountStatusMulti_v1", con))
        //            {
        //                cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //                var table = new DataTable();
        //                table.Columns.Add("ID", typeof(int));
        //                foreach (var item in listuserid)
        //                    table.Rows.Add(item);
        //                var pList = new SqlParameter("@list", SqlDbType.Structured);
        //                pList.TypeName = "dbo.ListID";
        //                pList.Value = table;
        //                cmd.Parameters.Add(pList);
        //                cmd.Parameters.Add("@status", SqlDbType.Int).Value = status == 0 ? DBNull.Value : status;
        //                con.Open();
        //                i = cmd.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(new { data = i, message = e.Message });
        //    }
        //    return Ok(new { data = i, message = "Thành công" });
        //}

        //public IActionResult ForgotPassword()
        //{
        //    return View();
        //}
    }
}
