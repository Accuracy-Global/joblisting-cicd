using Facebook;
using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Linq;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
//using System.Net.Mail;
using System.Text;
using System.Web;

namespace JobPortal.Web
{
    public class FacebookPostingService : IPostingService
    {
        string app_id = string.Empty;
        string app_secret = string.Empty;
        string access_token = string.Empty;
        string auth_url = "https://graph.facebook.com/oauth/access_token?client_id={0}&client_secret={1}&grant_type=client_credentials&scope={2}";
        string scopes = "manage_pages,publish_stream,publish_pages,publish_actions";
        string pageId = string.Empty;

        public FacebookPostingService()
        {
            app_id = ConfigService.Instance.GetConfigValue("FacebookAppId");
            app_secret = ConfigService.Instance.GetConfigValue("FacebookAppSecret");
            access_token = ConfigService.Instance.GetConfigValue("FacebookAccessToken");
            pageId = ConfigService.Instance.GetConfigValue("FacebookPageId");
        }

        public string Post(Job model)
        {
            string id = string.Empty;
            try
            {
                if (model != null)
                {
                    UserProfile employer = MemberService.Instance.Get(model.EmployerId.Value);

                    FacebookClient facebookClient = new FacebookClient(access_token);
                    dynamic messagePost = new ExpandoObject();
                    messagePost.access_token = access_token;
                    messagePost.picture = string.Format("{0}://{1}/images/facebook-post-400-300.png", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority);
                    messagePost.link = string.Format("{0}://{1}/job/{2}-{3}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, model.PermaLink, model.Id);
                    messagePost.name = model.Title;
                    string region = string.Empty;
                    if (!string.IsNullOrEmpty(model.City))
                    {
                        region = model.City + ", ";
                    }

                    if (model.StateId != null)
                    {
                        List state = SharedService.Instance.GetState(model.StateId.Value);
                        region += state.Text + ", ";
                    }

                    if (model.CountryId != null)
                    {
                        List country = SharedService.Instance.GetCountry(model.CountryId.Value);
                        region += country.Text;
                    }

                    messagePost.description = string.Format("Apply for {0} at joblisting | job listing, {0} job in {1}. Read {0} job description and find other {2} jobs at Joblisting. Find all jobs of the company, share job of the company, connect with company.", model.Title, region, employer.Company);
                    var result = facebookClient.Post(string.Format("/{0}/feed", pageId), messagePost);
                    if (result != null)
                    {
                        id = Convert.ToString(result["id"]);
                    }
                }
            }
            catch (Exception)
            {
                id = string.Empty;
            }
            return id;
        }

        public void Delete(string id)
        {
            FacebookClient facebookClient = new FacebookClient(access_token);
            var result = facebookClient.Delete(id);
        }

        private string GetAccessToken()
        {
            string accessToken = string.Empty;
            string url = string.Format(auth_url, app_id, app_secret, scopes);

            WebRequest request = WebRequest.Create(url);
            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    String responseString = reader.ReadToEnd();

                    NameValueCollection query = HttpUtility.ParseQueryString(responseString);

                    accessToken = query["access_token"];
                }
            }
        
            if (accessToken.Trim().Length == 0)
                throw new Exception("No Access Token");

            return accessToken;
        } 
    }
}