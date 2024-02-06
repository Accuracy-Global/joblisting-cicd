using Hangfire.Dashboard;
using Microsoft.Owin;
using System.Web;

namespace JobPortal.Web
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
    }
}