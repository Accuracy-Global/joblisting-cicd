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

namespace JoblistingBirthdayService
{
    public class Birthday : DataService
    {
        public void Wish()
        {
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            List<UserEntity> users = ReadData<UserEntity>("GetBirthdayList");
            using (SmtpClient oSmtp = new SmtpClient())
            {
                foreach (var item in users)
                {
                    using (var reader = new StreamReader(string.Format("{0}birthday.html", item.Template)))
                    {
                        string body = reader.ReadToEnd();
                        body = body.Replace("@@firstname", item.FirstName);
                        body = body.Replace("@@lastname", item.LastName);

                        string[] receipent = { item.Username };

                        List<DbParameter> parameters = new List<DbParameter>();
                        parameters.Add(new SqlParameter("@AlertId", item.AlertType));
                        parameters.Add(new SqlParameter("@Username", item.Username));

                        object dateSent = ReadDataField("GetAlertSentOn", parameters);
                        bool remind = false;
                        if (dateSent == DBNull.Value)
                        {
                            remind = true;
                        }
                        else
                        {
                            DateTime dated = Convert.ToDateTime(dateSent);
                            if (!dated.Date.Equals(DateTime.Now.Date))
                            {
                                remind = true;
                            }
                        }

                        if (remind)
                        {

                            MimeMessage mail = new MimeMessage();
                            mail.From.Add(new MailboxAddress("Joblisting", from));
                            mail.To.Add(new MailboxAddress(string.Format("{0} {1}", item.FirstName, item.LastName), item.Username));
                            mail.Subject = "Happy Birthday";
                            mail.Body = new TextPart("html")
                            {
                                Text = body,
                            };
                            try
                            {
                                // XXX - Should this be a little different?
                                oSmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                                oSmtp.Connect("smtp.mailgun.org", 587, false);
                                oSmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                                oSmtp.Authenticate(postmail, postpassword);

                                oSmtp.Send(mail);
                                oSmtp.Disconnect(true);
                                parameters = new List<DbParameter>();
                                parameters.Add(new SqlParameter("@AlertId", item.AlertType));
                                parameters.Add(new SqlParameter("@Receiver", item.Username));
                                parameters.Add(new SqlParameter("@Sender", from));

                                int stat = HandleData("TrackAlertHistory", parameters);

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

        void SendEx(Exception ex)
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
            mail.Subject = "Error occured while running Birthday Wishing Service";
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
