using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Routing;

namespace ShallvaMVC.Filters
{
    public class AdminOnlyFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool isAdmin = true;
            try
            {
                base.OnActionExecuting(filterContext);
                HttpContext ctx = HttpContext.Current;

                ctx.Session.Timeout = 60;

                var userManager = filterContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                if (!ctx.User.Identity.IsAuthenticated || !userManager.IsInRole(ctx.User.Identity.GetUserId(), "Admin"))
                {
                    isAdmin = false;
                }
            }
            catch
            {
                isAdmin = false;
            }

            if (!isAdmin)
            {
                filterContext.Result = new RedirectToRouteResult(
                           new RouteValueDictionary {{ "Controller", "Home" },
                                      { "Action", "Index" } });
            }
        }
    }
}