using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;
//using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobslitingActivityService
{
    public partial class ActivityService : ServiceBase
    {
        public ActivityService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Task informer = new Task(new Action(ActivityInformer), TaskCreationOptions.LongRunning);
            informer.Start();
        }

        protected override void OnStop()
        {
        }

        public void ActivityInformer()
        {
            while (true)
            {
                try
                {
                    ActivityStreamService streamService = new ActivityStreamService();
                    streamService.Send();
                }
                catch (Exception ex)
                {
                    SendEx(ex);
                }
                Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }

        void SendEx(Exception ex)
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

           
            msg.Subject = "Error occured while running Joblisting Activity Service";
            msg.Body = new TextPart("html") { Text = body };

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                smtp.Connect("smtp.mailgun.org", 587, false);
                smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                smtp.Authenticate(postmail, postpassword);

                smtp.Send(msg);
                smtp.Disconnect(true);
            }

        }
    }
}
