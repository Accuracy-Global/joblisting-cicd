using JobPortal.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Web.Controllers
{
    public class WebinarController : Controller
    {
      
        [UrlPrivilegeFilter]
        public ActionResult Index()
        {
            return View();
        }


     
        public ActionResult GetHtmlPage(string path)
        {
            return new FilePathResult(path, "text/html");
        }
    }
}