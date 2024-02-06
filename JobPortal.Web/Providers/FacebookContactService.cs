#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace JobPortal.Web.Providers
{
    public class FacebookContactService : IContactService
    {
        protected string facebook_client_id = string.Empty;    // Replace this with your Client ID
        protected string facebook_client_sceret = string.Empty;                                                // Replace this with your Client Secret
        protected string facebook_redirect_url = string.Empty;
        protected string facebook_auth_url = string.Empty;
        protected string facebook_auth_token_url = string.Empty;
        protected string facebook_contacts_url = string.Empty;     
        
        public FacebookContactService()
        {
            facebook_client_id = ConfigService.Instance.GetConfigValue("facebook_client_id");
            facebook_client_sceret = ConfigService.Instance.GetConfigValue("facebook_client_sceret");
            facebook_auth_url = ConfigService.Instance.GetConfigValue("facebook_auth_url");
            facebook_redirect_url = ConfigService.Instance.GetConfigValue("facebook_redirect_url");
            facebook_auth_token_url = ConfigService.Instance.GetConfigValue("facebook_auth_token_url");
            facebook_contacts_url = ConfigService.Instance.GetConfigValue("facebook_contacts_url");
        }

        public string AuthUrl
        {
            get
            {
                return string.Format(facebook_auth_url, facebook_client_id, facebook_redirect_url, (new Random()).Next(9999999));
            }
        }

        public List<ImportContact> Contacts(string code)
        {
            List<ImportContact> contacts = new List<ImportContact>();

            var request = (HttpWebRequest)WebRequest.Create(facebook_auth_token_url);

            string postData = string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code", code, facebook_client_id, facebook_client_sceret, facebook_redirect_url);
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            Response result = null;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                result = JsonConvert.DeserializeObject<Response>(responseString);
            }
             request = (HttpWebRequest)WebRequest.Create(string.Format(facebook_contacts_url, result.access_token));
             using (var response = (HttpWebResponse)request.GetResponse())
             {
                 var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
             }
            //Facebook.FacebookClient fc = new Facebook.FacebookClient(result.access_token);
            //dynamic f = fc.Get("me/friends");

            return contacts;
        }
    }
}