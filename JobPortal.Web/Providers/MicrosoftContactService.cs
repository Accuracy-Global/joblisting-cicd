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
    public class MicrosoftContactService : IContactService
    {
        //protected string live_client_id = "0000000040162C01";    // Replace this with your Client ID
        //protected string live_client_sceret = "SWvZcndp3kdQ-5PkcLFsk91raCAbZZjW";                                                // Replace this with your Client Secret
        //protected string live_redirect_url = "https://localhost:44300/network/live";

        //protected string live_auth_url = "https://login.live.com/oauth20_authorize.srf?client_id={0}&scope=wl.signin%20wl.basic%20wl.emails&response_type=code&redirect_uri={1}";
        //protected string live_auth_token_url = "https://login.live.com/oauth20_token.srf";

        protected string live_client_id = string.Empty;    // Replace this with your Client ID
        protected string live_client_sceret = string.Empty;                                                // Replace this with your Client Secret
        protected string live_redirect_url = string.Empty;
        protected string live_auth_url = string.Empty;
        protected string live_auth_token_url = string.Empty;
        protected string live_contacts_url = string.Empty;
        public MicrosoftContactService()
        {
            live_client_id = ConfigService.Instance.GetConfigValue("live_client_id");
            live_client_sceret = ConfigService.Instance.GetConfigValue("live_client_sceret");
            live_auth_url = ConfigService.Instance.GetConfigValue("live_auth_url");
            live_redirect_url = ConfigService.Instance.GetConfigValue("live_redirect_url");
            live_auth_token_url = ConfigService.Instance.GetConfigValue("live_auth_token_url");
            live_contacts_url = ConfigService.Instance.GetConfigValue("live_contacts_url");
        }

        public string AuthUrl
        {
            get
            {
                return string.Format(live_auth_url, live_client_id, live_redirect_url);
            }
        }

        public List<ImportContact> Contacts(string code)
        {
            List<ImportContact> contacts = new List<ImportContact>();

            var request = (HttpWebRequest)WebRequest.Create(live_auth_token_url);
            string postData = string.Format("client_id={0}&redirect_uri={1}&client_secret={2}&code={3}&grant_type=authorization_code", live_client_id, live_redirect_url, live_client_sceret, code);
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
            
            request = (HttpWebRequest)WebRequest.Create(string.Format(live_contacts_url, result.access_token));
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var jsonData = JsonConvert.DeserializeObject<JObject>(responseString);
                if (jsonData["data"] != null)
                {
                    foreach (var mitem in jsonData["data"].ToList())
                    {
                        string first_name = Convert.ToString(mitem["first_name"]);
                        string last_name = Convert.ToString(mitem["last_name"]);

                        Hashtable emails = JsonConvert.DeserializeObject<Hashtable>(Convert.ToString(mitem["emails"]));
                        if (emails != null && emails.Count > 0)
                        {
                            foreach (var key in emails.Keys)
                            {
                                string email = Convert.ToString(emails[key]);
                                if (!string.IsNullOrEmpty(email))
                                {
                                    ImportContact contact = new ImportContact()
                                    {
                                        FirstName = Convert.ToString(mitem["first_name"]),
                                        LastName = Convert.ToString(mitem["last_name"]),
                                        Email = email
                                    };
                                    if (contacts.Where(x => x.Email.Equals(email)).Count() == 0)
                                    {
                                        contacts.Add(contact);
                                    }
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