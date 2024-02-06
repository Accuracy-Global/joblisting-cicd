using JobPortal.Web.App_Start;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new UrlPrivilegeFilter());
        }
    }
}