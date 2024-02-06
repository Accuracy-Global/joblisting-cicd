using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Helpers;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;
//using System.Net.Mail;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using OpenPop.Pop3;
using System.IO;
using OpenPop.Mime;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
namespace JoblistingUserVerifyingService
{
    public class Email
    {
        public int Id { get; set; }
        public string Body { get; set; }
    }
    public partial class UserVerifyingService : ServiceBase
    {
        public UserVerifyingService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Task verifyingUser = new Task(new Action(ValidateEmails), TaskCreationOptions.LongRunning);
            verifyingUser.Start();
        }
        protected override void OnContinue()
        {
            Task verifyingUser = new Task(new Action(ValidateEmails), TaskCreationOptions.LongRunning);
            verifyingUser.Start();

            base.OnContinue();
        }
        protected override void OnStop()
        {
            SmtpClient oSmtp = new SmtpClient();
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
            mail.Subject = "Joblisting User Verifying Service Stopped Working";
            mail.Body = new TextPart("html")
            {
                Text = "<h3>Joblisting User Verifying Service has been stopped, please start once again!</h3>"
            };

            oSmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

            oSmtp.Connect("smtp.mailgun.org", 587, false);
            oSmtp.AuthenticationMechanisms.Remove("XOAUTH2");
            oSmtp.Authenticate(postmail, postpassword);

            oSmtp.Send(mail);
            oSmtp.Disconnect(true);

        }
        private void DeleteMessage(int number)
        {
            using (Pop3Client pop3 = new Pop3Client())
            {
                try
                {
                    pop3.Connect(ConfigurationManager.AppSettings["POPHost"], Convert.ToInt32(ConfigurationManager.AppSettings["POPPort"]), Convert.ToBoolean(ConfigurationManager.AppSettings["POPSSLEnabled"]), 50000, 50000, null);           // Connect to server and login
                    pop3.Authenticate(ConfigurationManager.AppSettings["POPUser"], ConfigurationManager.AppSettings["POPPassword"]);

                    pop3.DeleteMessage(number);
                }
                catch (Exception ex)
                {
                    SmtpClient oSmtp = new SmtpClient();
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
                    mail.Subject = "Error occured while running Joblisting User Verifying Service - Delete Message";
                    mail.Body = new TextPart("html")
                    {
                        Text = string.Format("<h2>{0}</h2>", ConfigService.Instance.GetConfigValue("BaseUrl")) + string.Format("{0}", ex.ToString())

                    };

                    oSmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    oSmtp.Connect("smtp.mailgun.org", 587, false);
                    oSmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                    oSmtp.Authenticate(postmail, postpassword);

                    oSmtp.Send(mail);
                    oSmtp.Disconnect(true);

                }



            }
        }
        private List<Email> GetMessageList()
        {
            List<Email> list = new List<Email>();
            using (Pop3Client pop3 = new Pop3Client())
            {
                try
                {
                    pop3.Connect(ConfigurationManager.AppSettings["POPHost"], Convert.ToInt32(ConfigurationManager.AppSettings["POPPort"]), Convert.ToBoolean(ConfigurationManager.AppSettings["POPSSLEnabled"]), 100000, 100000, null);           // Connect to server and login
                    pop3.Authenticate(ConfigurationManager.AppSettings["POPUser"], ConfigurationManager.AppSettings["POPPassword"]);

                    int messageCount = pop3.GetMessageCount();
                    for (int i = messageCount; i > 0; i--)
                    {
                        OpenPop.Mime.Message msg = pop3.GetMessage(i);
                        string subject = msg.Headers.Subject;

                        if (msg.MessagePart.Body != null)
                        {
                            string body = msg.MessagePart.GetBodyAsText();
                            if (subject.ToLower().Contains("undeliverable") || subject.ToLower().Contains("delivery failure") || subject.ToLower().Contains("failure notice") || subject.ToLower().Contains("returned mail"))
                            {
                                list.Add(new Email() { Id = i, Body = body });
                            }
                        }
                    }
                    pop3.Disconnect();
                }
                catch (Exception ex)
                {
                    SmtpClient oSmtp = new SmtpClient();
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
                    mail.Subject = "Error occured Joblisting User Verifying Service - Get Message List";
                    mail.Body = new TextPart("html")
                    {
                        Text = string.Format("<h2>{0}</h2>", ConfigService.Instance.GetConfigValue("BaseUrl")) + string.Format("{0}", ex.ToString())

                    };

                    oSmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    oSmtp.Connect("smtp.mailgun.org", 587, false);
                    oSmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                    oSmtp.Authenticate(postmail, postpassword);

                    oSmtp.Send(mail);
                    oSmtp.Disconnect(true);

                }
            }

            return list;
        }



        public void ValidateEmails()
        {
            while (true)
            {
                try
                {
                    using (Pop3Client pop3 = new Pop3Client())
                    {
                        try
                        {
                            pop3.Connect(ConfigurationManager.AppSettings["POPHost"], Convert.ToInt32(ConfigurationManager.AppSettings["POPPort"]), Convert.ToBoolean(ConfigurationManager.AppSettings["POPSSLEnabled"]), 50000, 50000, null);           // Connect to server and login
                            pop3.Authenticate(ConfigurationManager.AppSettings["POPUser"], ConfigurationManager.AppSettings["POPPassword"]);
                            int messageCount = pop3.GetMessageCount();
                            for (int i = messageCount; i > 0; i--)
                            {
                                OpenPop.Mime.Message msg = pop3.GetMessage(i);
                                string subject = msg.Headers.Subject;

                                if (msg.MessagePart.Body != null)
                                {
                                    string body = msg.MessagePart.GetBodyAsText();
                                    if (subject.ToLower().Contains("undeliverable") || subject.ToLower().Contains("delivery failure") || subject.ToLower().Contains("failure notice") || subject.ToLower().Contains("returned mail"))
                                    {
                                        if (!string.IsNullOrEmpty(body))
                                        {
                                            MemberService.Instance.ValidateEmail(body);
                                        }
                                    }
                                }
                            }
                            pop3.Disconnect();
                        }
                        catch (Exception ex)
                        {
                            SmtpClient oSmtp = new SmtpClient();
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
                            mail.Subject = "Error occured Joblisting User Verifying Service";
                            mail.Body = new TextPart("html")
                            {
                                Text = string.Format("<h2>{0}</h2>", ConfigService.Instance.GetConfigValue("BaseUrl")) + string.Format("{0}", ex.ToString())

                            };

                            oSmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                            oSmtp.Connect("smtp.mailgun.org", 587, false);
                            oSmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                            oSmtp.Authenticate(postmail, postpassword);

                            oSmtp.Send(mail);
                            oSmtp.Disconnect(true);

                           
                           
                        }
                    }
                    Thread.Sleep(new TimeSpan(0, 0, 15));
                }
                catch (Exception ex)
                {
                    SmtpClient oSmtp = new SmtpClient();
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
                    mail.Subject = "Error occured while running Joblisting User Verifying Service";
                    mail.Body = new TextPart("html")
                    {
                        Text = string.Format("<h2>{0}</h2>", ConfigService.Instance.GetConfigValue("BaseUrl")) + string.Format("{0}", ex.ToString())

                    };

                    oSmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    oSmtp.Connect("smtp.mailgun.org", 587, false);
                    oSmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                    oSmtp.Authenticate(postmail, postpassword);

                    oSmtp.Send(mail);
                    oSmtp.Disconnect(true);
                  
                }
            }

        }
    }
}
