using DINTEIOT.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace GovermentPortal.Helpers
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string claimType, string claimValue) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly Claim _claim;
        public ClaimRequirementFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //if (GovermentPortal.Areas.Admin.Controllers.BaseController.roleid != (int)Role.Admin) // ko check quyền với admin
            //{
            //    var hasClaim = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == _claim.Type).Value.ToString().Split(",").ToList().Contains(_claim.Value);
            //    if (!hasClaim)
            //    {
            //        if (_claim.Type == AuthorizeCode.XEM)
            //        {
            //            context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "NotAuthorize", area = "admin" }));
            //        }
            //        else
            //        {
            //            context.Result = new ObjectResult(new { status = (int)ExitCodes.NotAuthorize,data = (int)ExitCodes.NotAuthorize, message = "Bạn chưa được cấp quyền để thực hiện tác vụ này" }); ;
            //        }
            //    }
            //}

        }
    }
}