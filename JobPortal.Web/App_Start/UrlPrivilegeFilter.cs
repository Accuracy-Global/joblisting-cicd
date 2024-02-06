using JobPortal.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Routing;

namespace JobPortal.Web.App_Start
{
    public class UrlPrivilegeFilter : ActionFilterAttribute
    {
        private bool flag;
        private string[] staticActionName;
        private bool isLogCreate;
        public UrlPrivilegeFilter()
        {
            flag = false;
            this.staticActionName = ConfigurationManager.AppSettings["ActionName"].ToLower().Split(',');
            this.isLogCreate = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLogCreate"]);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var url = filterContext.HttpContext.Request.Url.AbsolutePath;
            string countrySession = HttpContext.Current.Session != null ? Convert.ToString(HttpContext.Current.Session["Country"])?.ToLower() : null;

            HttpContextBase context = filterContext.HttpContext;

            if (!flag && !string.IsNullOrEmpty(countrySession) && !url.ToLower().Contains(countrySession) && url != "/")
            {
                var routeData = filterContext.RouteData;
                var controllerName = Convert.ToString(routeData.Values["controller"])?.ToLower();
                var actionName = Convert.ToString(routeData.Values["action"])?.ToLower();
                var id = Convert.ToString(routeData.Values["id"])?.ToLower();
                var queryString = filterContext.HttpContext.Request.QueryString.AllKeys;

                var countryRouteDetail = Array.Find(this.staticActionName, a => a == actionName) == actionName ? RouteTable.Routes[countrySession] : null;

                filterContext.Controller.TempData["ControllerName"] = controllerName;
                filterContext.Controller.TempData["ActionName"] = actionName;
                filterContext.Controller.TempData["CountryRouteDetail"] = countryRouteDetail;
                filterContext.Controller.TempData["Url"] = url;

                #region
                RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();

                if (!string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(actionName) &&
                !string.IsNullOrEmpty(Convert.ToString(countrySession)))
                {
                    if (countryRouteDetail == null)
                    {
                        switch (Convert.ToString(countrySession))
                        {
                            #region
                            case "india":
                                redirectTargetDictionary = RedirectToCountryUrl("india", controllerName, actionName, id, queryString, filterContext);
                                break;

                            case "singapore":
                                redirectTargetDictionary = RedirectToCountryUrl("singapore", controllerName, actionName, id, queryString, filterContext);
                                break;

                            case "malaysia":
                                redirectTargetDictionary = RedirectToCountryUrl("malaysia", controllerName, actionName, id, queryString, filterContext);
                                break;

                            case "china":
                                redirectTargetDictionary = RedirectToCountryUrl("china", controllerName, actionName, id, queryString, filterContext);
                                break;

                            case "vietnam":
                                redirectTargetDictionary = RedirectToCountryUrl("Vietnam", controllerName, actionName, id, queryString, filterContext);
                                break;

                            case "suriname":
                                redirectTargetDictionary = RedirectToCountryUrl("suriname", controllerName, actionName, id, queryString, filterContext);
                                break;

                            case "thailand":
                                redirectTargetDictionary = RedirectToCountryUrl("thailand", controllerName, actionName, id, queryString, filterContext);
                                break;

                            case "philippines":
                                redirectTargetDictionary = RedirectToCountryUrl("philippines", controllerName, actionName, id, queryString, filterContext);
                                break;

                            case "japan":
                                redirectTargetDictionary = RedirectToCountryUrl("japan", controllerName, actionName, id, queryString, filterContext);
                                break;

                            case "indonesia":
                                redirectTargetDictionary = RedirectToCountryUrl("indonesia", controllerName, actionName, id, queryString, filterContext);
                                break;

                            case "afghanistan":
                                redirectTargetDictionary = RedirectToCountryUrl("afghanistan", controllerName, actionName, id, queryString, filterContext);
                                break;

                            case "brunei":
                                redirectTargetDictionary = RedirectToCountryUrl("brunei", controllerName, actionName, id, queryString, filterContext);
                                break;

                            case "cyprus":
                                redirectTargetDictionary = RedirectToCountryUrl("cyprus", controllerName, actionName, id, queryString, filterContext);
                                break;

                            case "georgia":
                                redirectTargetDictionary = RedirectToCountryUrl("georgia", controllerName, actionName, id, queryString, filterContext);
                                break;

                            default:
                                redirectTargetDictionary = RedirectToCountryUrl(countrySession, controllerName, actionName, id, queryString, filterContext);
                                break;
                                #endregion
                        }

                        flag = true;
                        filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
                        filterContext.Result.ExecuteResult(filterContext);

                        if (isLogCreate)
                        {
                            GeolocationService locationService = new GeolocationService();
                            locationService.LogEntry($"UrlPrivilegeFilter => Contoller: {controllerName}, Action: {actionName}, Session: {countrySession}");
                        }
                    }
                    else
                    {
                        flag = true;
                        filterContext.Result = new RedirectToRouteResult(countrySession, null);
                        filterContext.Result.ExecuteResult(filterContext);

                        if (isLogCreate)
                        {
                            GeolocationService locationService = new GeolocationService();
                            locationService.LogEntry($"UrlPrivilegeFilter => Contoller: {controllerName}, Action: {actionName}, Session: {countrySession}");
                        }
                    }
                }
                #endregion
            }
            else
            {
                flag = false;
            }

            base.OnActionExecuting(filterContext);
        }

        public RouteValueDictionary RedirectToCountryUrl(string country, string controller, string action, string id, string[] queryString, ActionExecutingContext filterContext)
        {
            RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();

            redirectTargetDictionary.Add("controller", controller);
            redirectTargetDictionary.Add("action", action);
            redirectTargetDictionary.Add("country", country);

            if (!string.IsNullOrEmpty(id))
            {
                redirectTargetDictionary.Add("id", id);
            }

            if (queryString.Length > 0)
            {
                foreach (var item in queryString)
                {
                    redirectTargetDictionary.Add(item, filterContext.HttpContext.Request.QueryString[item]);
                }
            }

            return redirectTargetDictionary;
        }
    }
}