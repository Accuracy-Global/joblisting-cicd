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
using TweetSharp;

namespace JobPortal.Web.Providers
{
    public class TwitterContactService : IContactService
    {
        protected string twitter_client_id = "kh5yyV3nCQLHmr6mrqfmP2C2F";    // Replace this with your Client ID
        protected string twitter_client_sceret = "Y1NyyZouaCmIIFvBnKY3udzLFtNxUf2vDVvGVdt6GBcGILAA1K";                                                // Replace this with your Client Secret
        protected string twitter_redirect_url = "https://localhost:44300/network/twitter";
        protected string twitter_auth_url = "https://api.twitter.com/oauth2/token";
        protected string twitter_auth_token_url = "https://api.twitter.com/oauth2/token";
        protected string twitter_contacts_url = string.Empty;
        
        public TwitterContactService()
        {
            //twitter_client_id = ConfigService.Instance.GetConfigValue("twitter_client_id");
            //twitter_client_sceret = ConfigService.Instance.GetConfigValue("twitter_client_sceret");
            //twitter_auth_url = ConfigService.Instance.GetConfigValue("twitter_auth_url");
            //twitter_redirect_url = ConfigService.Instance.GetConfigValue("twitter_redirect_url");
            //twitter_auth_token_url = ConfigService.Instance.GetConfigValue("twitter_auth_token_url");
            //twitter_contacts_url = ConfigService.Instance.GetConfigValue("twitter_contacts_url");
            TweetSharp.TwitterService ts = new TweetSharp.TwitterService(twitter_client_id, twitter_client_sceret, "453698235-lmtnySoqVtVwEDYgDTVEm5uDVWcxeI154CsVIU85", "PCzQMuf02tdrUGferl8HuD4KkZwUH2C33fATl894Rnazk");
            //TweetSharp.TwitterUser tuser = ts.GetUserProfile(new TweetSharp.GetUserProfileOptions() { IncludeEntities = false });
            //TweetSharp.TwitterUser tuser = ts.VerifyCredentials(new TweetSharp.VerifyCredentialsOptions() { IncludeEntities = true });            

           
            var request = (HttpWebRequest)WebRequest.Create(twitter_auth_url);

            string postData = string.Format("client_id={0}&client_secret={1}&redirect_uri={2}&grant_type=client_credentials", twitter_client_id, twitter_client_sceret, twitter_redirect_url);
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


            request = (HttpWebRequest)WebRequest.Create("https://api.twitter.com/1.1/account/verify_credentials.json?include_email=true");
            request.Headers["Authorization"] = "Bearer " + result.access_token;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }

        }

        public string AuthUrl
        {
            get
            {
                TwitterService service = new TwitterService(twitter_client_id, twitter_client_sceret);

                // Step 1 - Retrieve an OAuth Request Token
                OAuthRequestToken requestToken = service.GetRequestToken();

                // Step 2 - Redirect to the OAuth Authorization URL
                Uri uri = service.GetAuthorizationUri(requestToken);
                return uri.ToString();
            }
        }

        public List<ImportContact> Contacts(string code)
        {
            throw new NotImplementedException();
        }
    }
}