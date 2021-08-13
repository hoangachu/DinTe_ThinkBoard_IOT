using DINTEIOT.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DINTEIOT.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            var userchecklogin = HttpContext.User;
            if (userchecklogin.Identity.IsAuthenticated)
            {
                return Redirect("/home/index");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Account account)
        {
            if (string.IsNullOrEmpty(account.username) || string.IsNullOrEmpty(account.password))
            {
                TempData["NotFound"] = true;
                return Redirect("/login");
            }
            account.password = LoginHelper.LoginHelper.Encrypt(account.password);
            var uservalidate = ValidateUser(account);
            if (uservalidate.accountID <= 0)
            {
                TempData["IsAlertLoginFail"] = true;
                return Redirect("/login");
            }
            //var listauthorizebyrole = _iauthorizeController.GetListAuthorizeByRole(uservalidate.roleid);
            var claims = new List<Claim>
            {
                            new Claim(ClaimTypes.Name, account.username),
                            new Claim(ClaimTypes.Role, uservalidate.roleid.ToString())
                            //new Claim("Email", uservalidate.email)
                            //new Claim("UserID", user.userId.ToString()),
                            //new Claim("RoleID", uservalidate.roleid.ToString()),
                            //new Claim(AuthorizeCode.THEM, string.Join(",", listauthorizebyrole.Where(x => x.Create == true).Select(x => x.FuncCode).ToList())),
                            //new Claim(AuthorizeCode.SUA, string.Join(",", listauthorizebyrole.Where(x => x.Edit == true).Select(x => x.FuncCode).ToList())),
                            //new Claim(AuthorizeCode.XOA, string.Join(",", listauthorizebyrole.Where(x => x.Delete == true).Select(x => x.FuncCode).ToList())),
                            //new Claim(AuthorizeCode.XEM, string.Join(",", listauthorizebyrole.Where(x => x.View == true).Select(x => x.FuncCode).ToList())),
                            //new Claim(AuthorizeCode.DUYET, string.Join(",", listauthorizebyrole.Where(x => x.Accept == true).Select(x => x.FuncCode).ToList())),
                            //new Claim(AuthorizeCode.CONGBO, string.Join(",", listauthorizebyrole.Where(x => x.Publish == true).Select(x => x.FuncCode).ToList()))
             };

            var claimsIdentity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            new AuthenticationProperties
                            {
                                IsPersistent = account.rememberme,
                                ExpiresUtc = DateTime.UtcNow.AddMinutes(4320)
                            });
            return Redirect("/home/index");
            //return Ok(new { data = 1, url = "/admin/home/index" });
        }
        public Account ValidateUser(Account accounts)
        {
            Account account = new Account();
            using (SqlConnection con = new SqlConnection(Startup.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Account_ValidateAccount_v1", con))
                {
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@userName", SqlDbType.VarChar).Value = accounts.username;
                            cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = accounts.password;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                account.accountID = (int)dr["AccountID"];
                                account.username = dr.IsDBNull("Username") == true ? "" : (string)dr["Username"];
                                account.roleid = dr.IsDBNull("roleid") == true ? 0 : (int)dr["roleid"];
                                account.email = dr.IsDBNull("email") == true ? null : (string)dr["email"];
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
    }
}
