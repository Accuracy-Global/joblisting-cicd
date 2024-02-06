using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;
//using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace JoblistingInvitationService
{
    public class Invitation : DataService
    {
        public void Send()
        {
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            List<InviteEntity> request_list = ReadData<InviteEntity>("GetFriendRequestList");
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            using (SmtpClient oSmtp = new SmtpClient())
            {
                foreach (var item in request_list)
                {
                    using (var reader = new StreamReader(string.Format("{0}invitation.html", item.Template)))
                    {
                        string body = reader.ReadToEnd();

                        if (!string.IsNullOrEmpty(item.RecipientName))
                        {
                            body = body.Replace("@@receiver", item.RecipientName);
                        }
                        else
                        {
                            body = body.Replace("@@receiver", item.Recipient);
                        }

                        body = body.Replace("@@sender", item.SenderName);
                        body = body.Replace("@@profileurl", string.Format("{0}{1}", item.BaseUrl, item.SenderProfileUrl));
                        body = body.Replace("@@accepturl", string.Format("{0}Network/Accept/{1}", item.BaseUrl, item.ConnectionId));
                        body = body.Replace("@@button", "Accept");

                        if (item.Registered > 0)
                        {
                            body = body.Replace("@@unsubscribe", "");
                        }
                        else
                        {
                            string ulink = string.Format("<a href=\"{0}Network/Unsubscribe?Id={1}\">unsubscribe</a> or ", item.BaseUrl, item.ConnectionId);
                            body = body.Replace("@@unsubscribe", ulink);
                        }

                        MimeMessage mail = new MimeMessage();
                        mail.From.Add(new MailboxAddress("Joblisting", from));
                        mail.To.Add(new MailboxAddress(item.RecipientName, item.Recipient));
                        mail.Subject = string.Format("{0} Invites you to connect at Joblisting", item.SenderName);
                        mail.Body = new TextPart("html")
                        {
                            Text = body,
                        };
                        try
                        {
                            oSmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                            oSmtp.Connect("smtp.mailgun.org", 587, false);
                            oSmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                            oSmtp.Authenticate(postmail, postpassword);

                            oSmtp.Send(mail);
                            oSmtp.Disconnect(true);

                            List<DbParameter> parameters = new List<DbParameter>();
                            parameters.Add(new SqlParameter("@ConnectionId", item.ConnectionId));

                            int stat = HandleData("UpdateFriendRequest", parameters);
                        }
                        catch (Exception ex)
                        {
                            SendEx(ex);
                        }

                    }
                }
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
