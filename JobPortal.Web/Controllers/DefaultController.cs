using JobPortal.Domain;
using JobPortal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JobPortal.Web.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            var url = HttpContext.Request.Url.AbsolutePath;
            var countrySession = Convert.ToString(HttpContext.Session["Country"])?.ToLower();

            var countryRouteDetail = RouteTable.Routes[countrySession];

            if (!string.IsNullOrEmpty(countrySession) && countryRouteDetail == null) //!string.IsNullOrEmpty(controller) && !string.IsNullOrEmpty(action) &&
            {
                return RedirectToAction("Home", "Job", new
                {
                    country = countrySession
                });
            }
            else
            {
                return Redirect(countrySession);
            }
        }

        [ChildActionOnly]
        public ActionResult GetHtmlPage(string path)
        {
            return new FilePathResult(path, "text/html");
        }
    }
}