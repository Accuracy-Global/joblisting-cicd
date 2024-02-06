using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Helpers;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
namespace JobPortal.Web
{
    public class AuthorizeUser : AuthorizeAttribute
    {       
        protected virtual UserPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as UserPrincipal; }
        }
        
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (CurrentUser != null)
            {
                if (filterContext.HttpContext.Request.IsAuthenticated)
                {
                    if (Roles != null)
                    {
                        if (!Roles.Contains(CurrentUser.Info.Role.GetDescription()))
                        {
                            filterContext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));

                            // base.OnAuthorization(filterContext); //returns to login url
                        }
                        //if (CurrentUser.Id != UserId)
                        //{
                        //    filterContext.Result = new RedirectToRouteResult(new
                        //    RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));

                        //    // base.OnAuthorization(filterContext); //returns to login url
                        //}
                    }
                }
            }
        }
    }
}