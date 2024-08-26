using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.Helper
{
    public class AdminAreaAuthorization : ActionFilterAttribute 
    {
     
        public AdminAreaAuthorization()
        {
    
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (!filterContext.HttpContext.User.IsInRole("Admin")|| !filterContext.HttpContext.User.IsInRole("Staff"))
            //{
            //    filterContext.Result = new RedirectToRouteResult(
            //        new RouteValueDictionary
            //        {
            //        { "controller", "Home" },
            //        { "action", "Index" },
            //        { "area", "" }
            //        });
            //}
            var user = filterContext.HttpContext.User;
            var isAdmin = user.IsInRole("Admin");
            var isStaff = user.IsInRole("Staff");

            if (!isAdmin && !isStaff)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                { "controller", "Home" },
                { "action", "Index" },
                { "area", "" }
                    });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
