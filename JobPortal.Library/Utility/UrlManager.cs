using System.Web;
using System.Web.Mvc;

namespace JobPortal.Library.Utility
{
    public class UrlManager
    {
        private static string GetCurrentUrl(string action, string controller, System.Web.Routing.RouteValueDictionary dictionary)
        {
            var urlHelper = new UrlHelper(((MvcHandler)HttpContext.Current.CurrentHandler).RequestContext);
            var url = urlHelper.Action(action, controller, dictionary);

            var baseURI = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter +
                         HttpContext.Current.Request.Url.Host +
                         (HttpContext.Current.Request.Url.Port != 80 ? ":" + HttpContext.Current.Request.Url.Port : "");

            return baseURI + url;
        }

        #region Common Urls

        //Get account activation Url
        public static string GetAccountActivatoinUrl(string userName, string accountCreateDate)
        {
            var urlParams =
                    new System.Web.Routing.RouteValueDictionary(
                        new
                        {
                            code = accountCreateDate,
                            name = userName
                        });

            return GetCurrentUrl("VerifyAccount", "account", urlParams);
        }

        //Get terms of use url
        public static string GetTermsUrl()
        {
            return GetCurrentUrl("terms", "help", null);
        }

        //Get privacy url
        public static string GetPrivacyUrl()
        {
            return GetCurrentUrl("privacy", "help", null);
        }

        //Get copyright url
        public static string GetCopyrightUrl()
        {
            return GetCurrentUrl("copyright", "help", null);
        }

        //Get copyright url
        public static string GetContactusUrl()
        {
            return GetCurrentUrl("contactus", "home", null);
        }

        //Get forgot password Url
        public static string GetPasswordResetUrl(string token, string username)
        {
            var urlParams =
                    new System.Web.Routing.RouteValueDictionary(
                        new
                        {
                            token = token,
                            username = username
                        });

            return GetCurrentUrl("ResetPassword", "Home", urlParams);
        }

        #endregion

        #region Jobseeker Urls

        //Get Job view url for Job alert emails
        public static string GetJobViewUrl(long jobId)
        {
            var urlParams =
                    new System.Web.Routing.RouteValueDictionary(
                        new
                        {
                            Id = jobId
                        });

            return GetCurrentUrl("jobview", "job", urlParams);
        }

        #endregion

        #region Employer Urls

        //Get Resume view url for Resume alert emails
        public static string GetResumeViewUrl(long resumeId, string mode)
        {
            var urlParams =
                    new System.Web.Routing.RouteValueDictionary(
                        new
                        {
                            resumeId = resumeId,
                            mode = mode
                        });

            return GetCurrentUrl("index", "Resume", urlParams);
        }

        //Get Employer profile view url
        public static string GetEmployerProfileUrl(long employerId)
        {
            var urlParams =
                    new System.Web.Routing.RouteValueDictionary(
                        new
                        {
                            Id = employerId
                        });

            return GetCurrentUrl("profile", "Employer", urlParams);
        }


        #endregion

        public static string GetHomePageUrl()
        {
            return GetCurrentUrl("index", "home",null);
        }
    }
}
