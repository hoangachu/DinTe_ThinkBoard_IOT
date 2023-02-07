using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
//Okw
namespace DINTEIOT.Controllers
{
    public class BaseController : Controller, IActionFilter
    {
        public int userid;
        public static int roleid;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                // not login
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index" }));
            }
            else
            {
                if (user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name) != null)
                {
                    TempData["UserName"] = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value.ToString();
                }
                if (user.Claims.FirstOrDefault(x => x.Type == "Email") != null)
                {
                    TempData["Email"] = user.Claims.FirstOrDefault(x => x.Type == "Email").Value.ToString();
                }
            }
        }
    }
}
