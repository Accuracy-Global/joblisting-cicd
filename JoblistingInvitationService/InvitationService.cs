using System;
using System.Configuration;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;
//using System.Net.Mail;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace JoblistingInvitationService
{
    public partial class InvitationService : ServiceBase
    {
        public InvitationService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Task inviteFriends = new Task(new Action(InviteFriends), TaskCreationOptions.LongRunning);
            inviteFriends.Start();
        }

        protected override void OnStop()
        {
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            string[] toList = ConfigurationManager.AppSettings["ServiceNotifyEmail"].Split(',');
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            MimeMessage mail = new MimeMessage();
            mail.From.Add(new MailboxAddress("Joblisting", from));
            foreach (string to in toList)
            {
                mail.To.Add(new MailboxAddress("Excited User", to));
            }
            mail.Subject = "Joblisting Invitation Service Stopped Working";
            mail.Body = new TextPart("html")
            {
                Text = "<h3>Joblisting Invitation Service has been stopped, please start once again!</h3>"
            };
            using (SmtpClient oSmtp = new SmtpClient())
            {
                oSmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                oSmtp.Connect("smtp.mailgun.org", 587, false);
                oSmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                oSmtp.Authenticate(postmail, postpassword);

                oSmtp.Send(mail);
                oSmtp.Disconnect(true);
            }
        }

        public void InviteFriends()
        {
            while (true)
            {
                try
                {
                    Invitation invitation = new Invitation();
                    invitation.Send();
                }
                catch (Exception ex)
                {
                    SendEx(ex);
                }
                Thread.Sleep(new TimeSpan(0, 0, 1));
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
            mail.Subject = "Error occured while running Joblisting Activity Service";
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
