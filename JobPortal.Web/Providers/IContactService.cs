using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Providers
{
    public interface IContactService
    {
        string AuthUrl { get;}
        List<ImportContact> Contacts(string code);
    }

    public class ImportContact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }

    public class Response
    {
        public string access_token { get; set; }
        public string id_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public DateTime created { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }
        public string user_id { get; set; }
        public string xoauth_yahoo_guid { get; set; }
    }
}