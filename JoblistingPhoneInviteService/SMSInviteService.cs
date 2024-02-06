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
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JoblistingInviteSMSService
{
    public partial class SMSInviteService : ServiceBase
    {
        public SMSInviteService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Task smsInvite = new Task(new Action(SMSInviteFriend), TaskCreationOptions.LongRunning);
            smsInvite.Start();
        }

        protected override void OnStop()
        {
        }

        /// <summary>
        /// Send invite via Phone
        /// </summary>
        public void SMSInviteFriend()
        {
            while (true)
            {
                try
                {
                    Invitation service = new Invitation();
                    service.Send();
                    service = null;
                }
                catch (Exception ex)
                {
                    SendEx(ex);
                }

                Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }
        void SendEx(Exception ex, string failedEmail = "")
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
            mail.Subject = "Error occured while running Joblisting SMS Invitation Service";
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
