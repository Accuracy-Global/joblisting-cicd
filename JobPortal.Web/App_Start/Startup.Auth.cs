using System.Configuration;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Web.WebPages.OAuth;
using System.Security.Claims;
using Microsoft.Owin.Security.Facebook;
using Owin.Security.Providers.Yahoo;
using Owin;
using Microsoft.Owin;
using Owin.Security.Providers.GooglePlus;
using Owin.Security.Providers.GooglePlus.Provider;
using Owin.Security.Providers.LinkedIn;
using Owin.Security.Providers.Instagram;
using Microsoft.Owin.Security.Google;
using System.Threading.Tasks;
using Microsoft.Owin.Security;

namespace JobPortal.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");






        }
    }
}