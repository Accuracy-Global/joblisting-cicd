#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using Newtonsoft.Json;
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
    public class YahooContactsData
    {
        public YContacts contacts;
    }
    public class YContacts
    {
        public List<YContact> contact { get; set; }
    }
    public class YContact
    {
        public List<YFields> fields { get; set; }
    }
    public class YFields
    {
        public int id { get; set; }
        public string type { get; set; }
        public object value { get; set; }
    }

    public class YahooContactService : IContactService
    {
        protected string yahoo_client_id = string.Empty;    // Replace this with your Client ID
        protected string yahoo_client_sceret = string.Empty;                                                // Replace this with your Client Secret
        protected string yahoo_redirect_url = string.Empty;
        protected string yahoo_auth_url = string.Empty;
        protected string yahoo_auth_token_url = string.Empty;
        protected string yahoo_contacts_url = string.Empty;

        public YahooContactService()
        {
            yahoo_client_id = ConfigService.Instance.GetConfigValue("yahoo_client_id");
            yahoo_client_sceret = ConfigService.Instance.GetConfigValue("yahoo_client_sceret");
            yahoo_auth_url = ConfigService.Instance.GetConfigValue("yahoo_auth_url");
            yahoo_redirect_url = ConfigService.Instance.GetConfigValue("yahoo_redirect_url");
            yahoo_auth_token_url = ConfigService.Instance.GetConfigValue("yahoo_auth_token_url");
            yahoo_contacts_url = ConfigService.Instance.GetConfigValue("yahoo_contacts_url");
        }

        public string AuthUrl
        {
            get
            {
                return string.Format(yahoo_auth_url, yahoo_client_id, yahoo_redirect_url);
            }
        }

        public List<ImportContact> Contacts(string code)
        {
            List<ImportContact> contacts = new List<ImportContact>();

            var request = (HttpWebRequest)WebRequest.Create(yahoo_auth_token_url);

            string postData = string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code", code, yahoo_client_id, yahoo_client_sceret, yahoo_redirect_url);
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

            yahoo_contacts_url = string.Format(yahoo_contacts_url, result.xoauth_yahoo_guid);
            request = (HttpWebRequest)WebRequest.Create(string.Format(yahoo_contacts_url, result.access_token));
            request.Headers["Authorization"] = "Bearer " + result.access_token;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                YahooContactsData yahooContactsData = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<YahooContactsData>(responseString);
                foreach (YContact yContact in yahooContactsData.contacts.contact)
                {
                    List<YFields> fields = yContact.fields;
                    Dictionary<string, object> names = new Dictionary<string, object>();
                    string email = string.Empty;
                    foreach(var item in fields){
                        if (item.type == "name")
                        {
                            names = item.value as Dictionary<string, object>;
                        }

                        if (item.type == "email")
                        {
                            email = Convert.ToString(item.value);
                        }

                        if (names.Count>0 && !string.IsNullOrEmpty(email))
                        {
                            break;
                        }
                    }
                    string firstName = string.Empty;
                    string lastName = string.Empty;
                    if (names.ContainsKey("givenName"))
                    {
                        firstName = Convert.ToString(names["givenName"]);
                    }

                    if (names.ContainsKey("familyName"))
                    {
                        lastName = Convert.ToString(names["familyName"]);
                    }

                    if (!string.IsNullOrEmpty(email))
                    {
                        ImportContact entity = new ImportContact()
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            Email = email
                        };
                        contacts.Add(entity);
                    }
                }
               
            }
            return contacts;
        }
    }
}