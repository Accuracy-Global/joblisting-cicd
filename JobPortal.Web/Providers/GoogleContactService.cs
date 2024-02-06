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
    public class GoogleContactService : IContactService
    {
        protected string google_client_id = string.Empty;    // Replace this with your Client ID
        protected string google_client_sceret = string.Empty;                                                // Replace this with your Client Secret
        protected string google_redirect_url = string.Empty;
        protected string google_auth_url = string.Empty;
        protected string google_auth_token_url = string.Empty;
        protected string google_contacts_url = string.Empty;

        //protected string google_client_id = "956678047963-i87vfl26le7sn9oskptrjph4hjq633b2.apps.googleusercontent.com";    // Replace this with your Client ID
        //protected string google_client_sceret = "YICMtHXGT7KMbO0T9RpOzcF9";                                                // Replace this with your Client Secret
        //protected string google_redirect_url = "https://localhost:44300/network/google";
        //protected string google_auth_url = "https://accounts.google.com/o/oauth2/auth?response_type=code&redirect_uri={0}&scope=profile%20https:%2F%2Fwww.googleapis.com%2Fauth%2Fcontacts&client_id={1}";

        public GoogleContactService()
        {
            google_client_id = ConfigService.Instance.GetConfigValue("google_client_id");
            google_client_sceret = ConfigService.Instance.GetConfigValue("google_client_sceret");
            google_auth_url = ConfigService.Instance.GetConfigValue("google_auth_url");
            google_redirect_url = ConfigService.Instance.GetConfigValue("google_redirect_url");
            google_auth_token_url = ConfigService.Instance.GetConfigValue("google_auth_token_url");
            google_contacts_url = ConfigService.Instance.GetConfigValue("google_contacts_url");
        }

        public string AuthUrl
        {
            get
            {
                return string.Format(google_auth_url, google_redirect_url, google_client_id);
            }
        }

        public List<ImportContact> Contacts(string code)
        {
            List<ImportContact> contacts = new List<ImportContact>();

            var request = (HttpWebRequest)WebRequest.Create(google_auth_token_url);

            string postData = string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code", code, google_client_id, google_client_sceret, google_redirect_url);
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

            request = (HttpWebRequest)WebRequest.Create(string.Format(google_contacts_url, result.access_token));
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var jsonData = JsonConvert.DeserializeObject<JObject>(responseString);
                if (jsonData["connections"] != null)
                {
                    foreach (var item in jsonData["connections"].ToList())
                    {
                        if (item["names"] != null)
                        {
                            var jData = item["names"].ToList().First().ToString();

                            string familyName = string.Empty;
                            string givenName = string.Empty;

                            var gnames = JsonConvert.DeserializeObject<Hashtable>(jData);

                            familyName = Convert.ToString(gnames["familyName"]);
                            givenName = Convert.ToString(gnames["givenName"]);
                            if (item["emailAddresses"] != null)
                            {
                                string email = Convert.ToString(item["emailAddresses"].First()["value"]);
                                if (!string.IsNullOrEmpty(email))
                                {
                                    ImportContact contact = new ImportContact()
                                    {
                                        FirstName = givenName,
                                        LastName = familyName,
                                        Email = email
                                    };
                                    contacts.Add(contact);
                                }

                            }
                        }
                        else
                        {
                            if (item["emailAddresses"] != null)
                            {
                                string email = Convert.ToString(item["emailAddresses"].First()["value"]);
                                if (!string.IsNullOrEmpty(email))
                                {
                                    ImportContact contact = new ImportContact()
                                    {
                                        FirstName = null,
                                        LastName = null,
                                        Email = email
                                    };
                                    contacts.Add(contact);
                                }

                            }
                        }
                    }
                }
            }

            return contacts;
        }
    }
}