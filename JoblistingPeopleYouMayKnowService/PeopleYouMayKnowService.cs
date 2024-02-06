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

namespace JoblistingPeopleYouMayKnowService
{
    public partial class PeopleYouMayKnowService : ServiceBase
    {
        public PeopleYouMayKnowService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Task peopleMayKnow = new Task(new Action(PeopleYouMayKnow), TaskCreationOptions.LongRunning);
            peopleMayKnow.Start();
        }

        protected override void OnStop()
        {
        }

        public void PeopleYouMayKnow()
        {
            while (true)
            {
                try
                {
                    PeopleYouMayKnow service = new PeopleYouMayKnow();
                    service.SendReminder();
                    service = null;
                }
                catch (Exception ex)
                {
                    SendEx(ex);
                }
                Thread.Sleep(new TimeSpan(0, 0, 10));
            }
        }
        private void SendEx(Exception ex)
        {
            string baseUrl = ConfigurationManager.AppSettings["SiteUrl"];
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            MimeMessage mail = new MimeMessage();

            string[] toList = ConfigurationManager.AppSettings["ServiceNotifyEmail"].Split(',');

            string body = string.Format("<h2>{0}</h2>", baseUrl);
            body += string.Format("{0}", ex.ToString());
            mail.From.Add(new MailboxAddress("Joblisting", from));
            foreach (string email in toList)
            {
                mail.To.Add(new MailboxAddress("Excited User", email));
            }
            mail.Subject = "Error occured while running Joblisting People You May Know Service";
            mail.Body = new TextPart("html")
            {
                Text = body,
            };

            using (SmtpClient osmtp = new SmtpClient())
            {
                osmtp.Timeout = 100000;
                osmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                osmtp.Connect("smtp.mailgun.org", 587, false);
                osmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                osmtp.Authenticate(postmail, postpassword);

                osmtp.Send(mail);
                osmtp.Disconnect(true);
            }

        }

    }
}
