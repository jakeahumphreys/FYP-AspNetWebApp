using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FYP_WebApp.Common_Logic
{
    public class CustomAuth : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // user is logged-in, so redirecting to login page won't help, must be premium
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "Error", Error = Errors.AccountUnauthorized}));
            }
            else
            {
                // let the base implementation redirect the user
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "Error", Error = Errors.Unauthorized}));
            }
        }
    }
}