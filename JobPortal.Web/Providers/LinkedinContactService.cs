#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace JobPortal.Web.Providers
{
    public class LinkedinContactService : IContactService
    {
        protected string linkedin_client_id = string.Empty;    // Replace this with your Client ID
        protected string linkedin_client_sceret = string.Empty;                                                // Replace this with your Client Secret
        protected string linkedin_redirect_url = string.Empty;
        protected string linkedin_auth_url = string.Empty;
        protected string linkedin_auth_token_url = string.Empty;
        protected string linkedin_contacts_url = string.Empty;

        public LinkedinContactService()
        {
            linkedin_client_id = ConfigService.Instance.GetConfigValue("linkedin_client_id");
            linkedin_client_sceret = ConfigService.Instance.GetConfigValue("linkedin_client_sceret");
            linkedin_auth_url = ConfigService.Instance.GetConfigValue("linkedin_auth_url");
            linkedin_redirect_url = ConfigService.Instance.GetConfigValue("linkedin_redirect_url");
            linkedin_auth_token_url = ConfigService.Instance.GetConfigValue("linkedin_auth_token_url");
            linkedin_contacts_url = ConfigService.Instance.GetConfigValue("linkedin_contacts_url");
        }
        public string AuthUrl
        {
            get
            {
                return string.Format(linkedin_auth_url, linkedin_client_id, linkedin_redirect_url, (new Random()).Next(9999999));
            }
        }

        public List<ImportContact> Contacts(string code)
        {
            List<ImportContact> contacts = new List<ImportContact>();

        //https://www.linkedin.com/oauth/v2/accessToken?grant_type=authorization_code&code={0}&redirect_uri={1}&client_id={2}&client_secret={3}
            var request = (HttpWebRequest)WebRequest.Create(linkedin_auth_token_url);
        string postData = string.Format("grant_type=authorization_code&code={0}&redirect_uri={1}&client_id={2}&client_secret={3}", code, linkedin_redirect_url, linkedin_client_id, linkedin_client_sceret);
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

            request = (HttpWebRequest)WebRequest.Create(string.Format(linkedin_contacts_url, result.access_token));
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var jsonData = JsonConvert.DeserializeObject<JObject>(responseString);
                //foreach (var mitem in jsonData["data"].ToList())
                //{
                //    string first_name = Convert.ToString(mitem["first_name"]);
                //    string last_name = Convert.ToString(mitem["last_name"]);

                //    Hashtable emails = JsonConvert.DeserializeObject<Hashtable>(Convert.ToString(mitem["emails"]));
                //    if (emails != null && emails.Count > 0)
                //    {
                //        foreach (var key in emails.Keys)
                //        {
                //            string email = Convert.ToString(emails[key]);
                //            if (!string.IsNullOrEmpty(email))
                //            {
                //                ImportContact contact = new ImportContact()
                //                {
                //                    FirstName = Convert.ToString(mitem["first_name"]),
                //                    LastName = Convert.ToString(mitem["last_name"]),
                //                    Email = email
                //                };
                //                if (contacts.Where(x => x.Email.Equals(email)).Count() == 0)
                //                {
                //                    contacts.Add(contact);
                //                }
                //            }
                //        }
                //    }
                //}
            }
            return contacts;
        }
    }
}