using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using TweetSharp;

namespace JobPortal.Web
{
    public class TwitterPostingService : IPostingService
    {
        string consumerKey = string.Empty;
        string consumerSecret = string.Empty;
        string accessToken = string.Empty;
        string accessTokenSecret = string.Empty;

        public TwitterPostingService()
        {
            consumerKey = ConfigService.Instance.GetConfigValue("TwitterConsumerKey");
            consumerSecret = ConfigService.Instance.GetConfigValue("TwitterConsumerSecret");
            accessToken = ConfigService.Instance.GetConfigValue("TwitterAccessToken");
            accessTokenSecret = ConfigService.Instance.GetConfigValue("TwitterAccessTokenSecret");
        }
        public string Post(Job model)
        {
            string id = string.Empty;
            try
            {
                if (model != null)
                {
                    StringBuilder sbJob = new StringBuilder();
                    sbJob.AppendFormat("{0}", model.Title);
                    string jurl = string.Format("{0}://{1}/job/{2}-{3}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, model.PermaLink, model.Id);
                    sbJob.AppendFormat(" - {0}", jurl);

                    TwitterService twitterService = new TwitterService(consumerKey, consumerSecret);
                    twitterService.AuthenticateWith(accessToken, accessTokenSecret);

                    SendTweetOptions tweet = new SendTweetOptions();
                    tweet.Status = sbJob.ToString();
                    var tweetStatus = twitterService.SendTweet(tweet);
                    if (tweetStatus != null)
                    {
                        id = tweetStatus.IdStr;
                    }
                }
            }
            catch (Exception)
            {
                id = "";
            }
            return id;
        }


        public void Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                TwitterService twitterService = new TwitterService(consumerKey, consumerSecret);
                twitterService.AuthenticateWith(accessToken, accessTokenSecret);

                DeleteTweetOptions tweet = new DeleteTweetOptions();
                tweet.Id = Convert.ToInt64(id);

                var tweetStatus = twitterService.DeleteTweet(tweet);
            }
        }
    }
}