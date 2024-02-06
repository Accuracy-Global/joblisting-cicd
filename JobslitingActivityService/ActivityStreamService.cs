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

namespace JobslitingActivityService
{
    public class ActivityStreamService : DataService
    {
        public void Send()
        {
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            List<StreamEntity> activity_list = ReadData<StreamEntity>("GetActivityStreams");
            using (SmtpClient oSmtp = new SmtpClient())
            {
                foreach (var item in activity_list)
                {
                    string tmpl_photo = string.Format("{0}photoupload_share.html", item.Template);
                    string tmpl_job = string.Format("{0}job_share.html", item.Template);
                    if (item.ActivityType == 1)
                    {
                        using (var reader = new StreamReader(tmpl_photo))
                        {
                            string body = reader.ReadToEnd();
                            body = body.Replace("@@receiver", item.RecipientName);
                            body = body.Replace("@@sender", item.SenderName);
                            body = body.Replace("@@url", string.Format("{0}{1}", item.BaseUrl, item.SenderProfileUrl));
                            MimeMessage message = new MimeMessage();
                            message.From.Add(new MailboxAddress("Joblisting", from));
                            message.To.Add(new MailboxAddress(item.RecipientName, item.Recipient));

                            message.Subject = string.Format("{0} has Uploaded New Photo", item.SenderName);
                            message.Body = new TextPart("html") { Text = body };
                            try
                            {
                                oSmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                                oSmtp.Connect("smtp.mailgun.org", 587, false);
                                oSmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                                oSmtp.Authenticate(postmail, postpassword);

                                oSmtp.Send(message);
                                oSmtp.Disconnect(true);
                                List<DbParameter> parameters = new List<DbParameter>();
                                parameters.Add(new SqlParameter("@Id", item.ActivityId));

                                int stat = HandleData("UpdateStream", parameters);
                            }
                            catch (Exception ex)
                            {
                                SendEx(ex);
                            }
                        }

                    }
                    else if (item.ActivityType == 2)
                    {
                        using (var reader = new StreamReader(tmpl_job))
                        {
                            string body = reader.ReadToEnd();
                            body = body.Replace("@@receiver", item.RecipientName);
                            body = body.Replace("@@sender", item.SenderName);
                            body = body.Replace("@@url", string.Format("{0}{1}", item.BaseUrl, item.SenderProfileUrl));
                            body = body.Replace("@@jobtitle", item.JobTitle);
                            body = body.Replace("@@joburl", string.Format("{0}/job/{1}", item.BaseUrl, item.JobUrl));
                            MimeMessage message = new MimeMessage();
                            message.From.Add(new MailboxAddress("Joblisting", from));
                            message.To.Add(new MailboxAddress(item.RecipientName, item.Recipient));
                            message.Subject = string.Format("{0} has Posted Job", item.SenderName);
                            message.Body = new TextPart("html") { Text = body };
                            try
                            {
                                oSmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                                oSmtp.Connect("smtp.mailgun.org", 587, false);
                                oSmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                                oSmtp.Authenticate(postmail, postpassword);

                                oSmtp.Send(message);
                                oSmtp.Disconnect(true);
                                List<DbParameter> parameters = new List<DbParameter>();
                                parameters.Add(new SqlParameter("@Id", item.ActivityId));

                                int stat = HandleData("UpdateStream", parameters);
                            }
                            catch (Exception ex)
                            {
                                SendEx(ex);
                            }
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
            MimeMessage msg = new MimeMessage();


                string[] toList = ConfigurationManager.AppSettings["ServiceNotifyEmail"].Split(',');

            string body = string.Format("<h2>{0}</h2>", baseUrl);
            body += string.Format("{0}", ex.ToString());
            msg.From.Add(new MailboxAddress("",from));
            foreach (string email in toList)
            {
                msg.To.Add(new MailboxAddress("",email));
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
