 ﻿using System.Text;
 ﻿using System.Web.Mvc;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
 ﻿using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
using System.Web.Mvc.Filters;
using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.Script.Serialization;
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)

namespace JobPortal.Web.Controllers
{
    [HandleError]
    public class BaseController : Controller
    {
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
        protected IUserService _service = null;
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
        public BaseController(IUserService service)
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
        {
            _service = service;
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {            
            if (User != null)
            {
                UserInfo = MemberService.Instance.GetUserInfo(Convert.ToInt64(User.Identity.Name));
                ViewBag.UserInfo = UserInfo;
            }
            base.OnActionExecuting(filterContext);
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception;
            
            var sbError = new StringBuilder();
            sbError.AppendFormat("<b>IPAddress:</b> {0}<br/>", Request.UserHostAddress);
            sbError.AppendFormat("<b>Browser:</b> {0}<br/>", Request.Browser.Browser);
            sbError.AppendFormat("<b>Site:</b> {0}<br/>", Request.Url);
            
            if (User != null)
            {
               // UserInfoEntity user = _service.Get(Convert.ToInt64(User.Identity.Name));
               UserInfoEntity user = _service.Get((User.Identity.Name));
                if (user != null)
                {
                    sbError.AppendFormat("<b>Username:</b> {0}<br/>", user.Username);
                }
            }
            if (Request.UrlReferrer != null)
            {
                sbError.AppendFormat("<b>Referer:</b> {0}<br/>", Request.UrlReferrer.ToString());
            }
            
            sbError.AppendFormat("<b>Exception:</b> {0}<br/>", ex.ToString());
           // AlertService.Instance.SendMail("Error Occurred ", new[] { ConfigurationManager.AppSettings["FromEmailAddress"] }, sbError.ToString());
            filterContext.ExceptionHandled = true;
            var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");

            filterContext.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };

            base.OnException(filterContext);
        }

        protected virtual new UserPrincipal User
        {
            get {
                UserPrincipal user = null;
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

                if (authCookie != null)
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null)
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();                        
                        UserPrincipal newUser = new UserPrincipal(authTicket.Name);
                        user = newUser;                        
                    }
                }
                return user;
            }
        }

#pragma warning disable CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        public UserInfoEntity UserInfo { get; set; }
#pragma warning restore CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
    }
}
