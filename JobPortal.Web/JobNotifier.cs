using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;
//using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace JobPortal.Web
{
    public class JobNotifier
    {
        public static void FreshJobsFirst()
        {
            IJobService jobService = new Services.JobService();

            string baseUrl = JobPortal.Domain.ConfigService.Instance.GetConfigValue("BaseUrl");
            string tmplPath = JobPortal.Domain.ConfigService.Instance.GetConfigValue("Templates");
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            List<JobNotifyEntity> list = jobService.JobNotifyList();
            using (SmtpClient smtp = new SmtpClient())
            {
                foreach (var item in list)
                {
                    System.Threading.Thread.Sleep(5);
                    using (var reader = new StreamReader(string.Format("{0}jobstatus.html", tmplPath)))
                    {
                        var body = reader.ReadToEnd();
                        body = body.Replace("@@name", item.Name);
                        body = body.Replace("@@url", string.Format("{0}jobs?country={1}", baseUrl, item.Country));
                        body = body.Replace("@@unsubscribe", string.Format("{0}/Home/Unsubscribe?alertId={1}&email={2}", baseUrl, 15, item.Username));
                        body = body.Replace("@@country", item.Country);
                        var subject = string.Format("Jobs Hiring Now in {0}", item.Country);
                        MimeMessage message = new MimeMessage();
                        message.From.Add(new MailboxAddress("Joblisting", "notify@joblisting.com"));
                        message.To.Add(new MailboxAddress("", item.Username));
                        message.Subject = subject;
                        message.Body = new TextPart("html") { Text = body };
                        try
                        {
                            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                            smtp.Connect("smtp.mailgun.org", 587, false);
                            smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                            smtp.Authenticate(postmail, postpassword);

                            smtp.Send(message);
                            smtp.Disconnect(true);
                            jobService.JobNotifyUpdate(item.Username);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                }
            }
        }

        public static void FreshJobsNext()
        {
            IJobService jobService = new Services.JobService();

            string baseUrl = JobPortal.Domain.ConfigService.Instance.GetConfigValue("BaseUrl");
            string tmplPath = JobPortal.Domain.ConfigService.Instance.GetConfigValue("Templates");
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            List<JobNotifyEntity> list = jobService.JobNotifyList();
            using (SmtpClient smtp = new SmtpClient())
            {
                foreach (var item in list)
                {
                    System.Threading.Thread.Sleep(5);
                    using (var reader = new StreamReader(string.Format("{0}jobstatus.html", tmplPath)))
                    {
                        var body = reader.ReadToEnd();
                        body = body.Replace("@@name", item.Name);
                        body = body.Replace("@@url", string.Format("{0}jobs?country={1}", baseUrl, item.Country));
                        body = body.Replace("@@unsubscribe", string.Format("{0}/Home/Unsubscribe?alertId={1}&email={2}", baseUrl, 15, item.Username));
                        body = body.Replace("@@country", item.Country);
                        var subject = string.Format("Jobs Hiring Now in {0}", item.Country);
                        MimeMessage message = new MimeMessage();
                        message.From.Add(new MailboxAddress("Joblisting", "notify@joblisting.com"));
                        message.To.Add(new MailboxAddress("", item.Username));

                        message.Subject = subject;
                        message.Body = new TextPart("html") { Text = body };
                        try
                        {
                            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                            smtp.Connect("smtp.mailgun.org", 587, false);
                            smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                            smtp.Authenticate(postmail, postpassword);

                            smtp.Send(message);
                            smtp.Disconnect(true);

                            jobService.JobNotifyUpdate(item.Username);

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                }
            }
        }
        private static void SendEx(Exception ex)
        {
            string baseUrl = ConfigurationManager.AppSettings["SiteUrl"];
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            MimeMessage msg = new MimeMessage();
            string[] toList = ConfigurationManager.AppSettings["ServiceNotifyEmail"].Split(',');

            string body = string.Format("<h2>{0}</h2>", baseUrl);
            body += string.Format("{0}", ex.ToString());
            msg.From.Add(new MailboxAddress("",from));
            foreach (string email in toList)
            {
                msg.To.Add(new MailboxAddress("", email));
            }

            msg.Subject = "Error occurred while running Joblisting Reminder Service";
            msg.Body = new TextPart("html") { Text = body };

            using (SmtpClient osmtp = new SmtpClient())
            {
                osmtp.Timeout = 100000;
                osmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                osmtp.Connect("smtp.mailgun.org", 587, false);
                osmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                osmtp.Authenticate(postmail, postpassword);

                osmtp.Send(msg);
                osmtp.Disconnect(true);
            }

        }
    }
}