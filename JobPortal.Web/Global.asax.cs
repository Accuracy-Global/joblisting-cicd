#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Helpers;
using JobPortal.Models;
using Newtonsoft.Json;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
namespace JobPortal.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        private bool urlFlag;
        private string prvUrl = string.Empty;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            // WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            var r = new Random();
            Application["RndNum"] = r.Next(10000, 99999).ToString();
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;

            ServicePointManager.ServerCertificateValidationCallback = delegate
            {
                return true;
            };
        }
        //protected void Application_AcquireRequestState(object sender, EventArgs e)
        //{
        //    HttpContext context = HttpContext.Current;
        //    var languaguesession = "en";
        //    if (context != null && context.Session != null)
        //        languaguesession = context.Session["lang"] != null ? context.Session["lang"].ToString() : "en";
        //    Thread.CurrentThread.CurrentUICulture = new CultureInfo(languaguesession);
        //    Thread.CurrentThread.CurrentCulture = new CultureInfo(languaguesession);

        //}


        protected void Application_BeginRequest()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        }

        protected void Application_Error()
        {
            Exception ex = Server.GetLastError();
            if (ex is HttpAntiForgeryException)
            {
                Response.Clear();
                Server.ClearError(); //make sure you log the exception first
                Response.Redirect("~/account/login", true);
            }
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    UserPrincipal newUser = new UserPrincipal(authTicket.Name);
                    HttpContext.Current.User = newUser;

                }
            }
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    UserPrincipal newUser = new UserPrincipal(authTicket.Name);
                    HttpContext.Current.User = newUser;
                }
            }
        }
        protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    UserPrincipal newUser = new UserPrincipal(authTicket.Name);
                    HttpContext.Current.User = newUser;
                }
            }
        }

        #region Get public Ip based Geolocation
        protected void Session_Start()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Country"])))
            {
                bool isLogCreate = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLogCreate"]);
                GeolocationService locationService = new GeolocationService();

                if (isLogCreate)
                {
                    locationService.LogEntry("================================================================================================================");
                    locationService.LogEntry(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt"));
                    locationService.LogEntry($"Initial Url => {HttpContext.Current.Request.Url.AbsoluteUri}");
                }

                var publicIp = locationService.GetUser_IP();
                var locationDetail = locationService.GetUserCountryByIp(publicIp);
                var countryName = locationDetail != null && !string.IsNullOrEmpty(locationDetail.country)  && locationDetail.country.ToLower() == "united states" ? locationDetail.country_code :
                    locationDetail != null && string.IsNullOrEmpty(locationDetail.country) ? "us" :
                    locationDetail != null && !string.IsNullOrEmpty(locationDetail.country) && locationDetail.country.ToLower() == "not found" ? "us" : locationDetail.country;

                Session["Country"] = countryName;
                //Session["Country"] = "malaysia";

                if (string.IsNullOrEmpty(Convert.ToString(Session["Country"])))
                {
                    countryName = locationDetail != null && !string.IsNullOrEmpty(locationDetail.country) && locationDetail.country.ToLower() == "united states" ? locationDetail.country_code :
                        locationDetail != null && string.IsNullOrEmpty(locationDetail.country) ? "us" :
                        locationDetail != null && !string.IsNullOrEmpty(locationDetail.country) && locationDetail.country.ToLower() == "not found" ? "us" : locationDetail.country;
                    Session["Country"] = countryName;
                }

                if (isLogCreate)
                {
                    locationService.LogEntry($"Session_Start() => Session: {Convert.ToString(Session["Country"])}");
                }
            }

        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            GeolocationService locationService = new GeolocationService();
            locationService.LogEntry("--------------------------------------");
            locationService.LogEntry($"Application_PreRequestHandlerExecute() Initial Url => {HttpContext.Current.Request.Url.AbsoluteUri}");
            var context = ((HttpApplication)sender).Context;
            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(context));

            if (HttpContext.Current.Session != null && string.IsNullOrEmpty(Convert.ToString(Session["Country"])))
            {
                bool isLogCreate = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLogCreate"]);

                var publicIp = locationService.GetUser_IP();
                var locationDetail = locationService.GetUserCountryByIp(publicIp);
                var countryName = locationDetail != null && !string.IsNullOrEmpty(locationDetail.country) && locationDetail.country.ToLower() == "united states" ? locationDetail.country_code :
                    locationDetail != null && string.IsNullOrEmpty(locationDetail.country) ? "us" :
                    locationDetail != null && !string.IsNullOrEmpty(locationDetail.country) && locationDetail.country.ToLower() == "not found" ? "us" : locationDetail.country;

                Session["Country"] = countryName;

                if (isLogCreate)
                {
                    locationService.LogEntry($"Application_PreRequestHandlerExecute() => Session: {Convert.ToString(Session["Country"])}");
                }
            }
        }
        #endregion
    }
}