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

namespace JoblistingPeopleYouMayKnowService
{
    public class PeopleYouMayKnow : DataService
    {
        public void SendReminder()
        {
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            List<PeopleEntity> user_list = ReadData<PeopleEntity>("GetPeopleMatchList");
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            string baseUrl = string.Empty;
            string template = string.Empty;
            int alertType = 0;
            if (user_list.Count > 0)
            {
                baseUrl = user_list.Select(x => x.BaseUrl).FirstOrDefault();
                template = user_list.Select(x => x.Template).FirstOrDefault();
                alertType = user_list.Select(x => x.AlertType).FirstOrDefault();
            }

            List<long> uids = user_list.Select(x => x.UserId).Distinct().ToList();
            List<UserEntity> users = user_list.Select(x => new UserEntity() { UserId = x.UserId, Username = x.Username, FirstName = x.FirstName, LastName = x.LastName, Company = x.Company, PermaLink = x.PermaLink, AlertType = x.AlertType, BaseUrl = x.BaseUrl, Template = template }).ToList();


            using (SmtpClient oSmtp = new SmtpClient())
            {
                foreach (var id in uids)
                {
                    var item = users.FirstOrDefault(x => x.UserId == id);

                    List<DbParameter> parameters = new List<DbParameter>();
                    parameters.Add(new SqlParameter("@AlertId", alertType));
                    parameters.Add(new SqlParameter("@Username", item.Username));

                    object dateSent = ReadDataField("GetAlertSentOn", parameters);
                    bool remind = false;

                    if (dateSent == DBNull.Value || dateSent == null)
                    {
                        remind = true;
                    }
                    else
                    {
                        DateTime dated = Convert.ToDateTime(dateSent);
                        if (!dated.Date.Equals(DateTime.Now.Date))
                        {
                            int diff = DateTime.Now.Date.Subtract(dated.Date).Days;
                            if (diff == 3)
                            {
                                remind = true;
                            }
                        }
                    }

                    if (remind)
                    {
                        List<UserEntity> user_match_list = user_list.Where(x => x.UserId == item.UserId).Select(x => new UserEntity() { UserId = x.MUserId, Username = x.MUsername, FirstName = x.MFirstName, LastName = x.MLastName, Company = x.MCompany, PermaLink = x.MPermaLink }).Distinct().ToList();
                        StringBuilder sb = new StringBuilder();
                        string image = string.Empty;
                        string name = string.Empty;
                        string profileName = string.Empty;
                        string body = string.Empty;

                        foreach (var matched_user in user_match_list)
                        {
                            name = string.Empty;
                            if (!string.IsNullOrEmpty(matched_user.Company))
                            {
                                name = matched_user.Company;
                            }
                            else
                            {
                                name = string.Format("{0} {1}", matched_user.FirstName, matched_user.LastName);
                            }

                            image = string.Format("<img src=\"{0}image/avtar?Id={1}\" style=\"width:64px; max-height:64px;\"/>", baseUrl, matched_user.UserId);

                            sb.AppendFormat("<div><div style=\"float:left; width:64px; height:64px; border:1px solid #d7d7d7;display: table-cell;vertical-align: middle;\"><a href=\"{0}{1}\" target=\"_blank\">{2}</a></div>", baseUrl, matched_user.PermaLink, image);
                            sb.Append("<div style=\"float:left; padding-left:20px; height:64px;\">");
                            sb.AppendFormat("<div style=\"padding-bottom:5px;\">{0}</div>", name);
                            sb.AppendFormat("<div><a href=\"{0}connect?EmailAddress={1}&via=email\" style=\"padding: 10px;  background-color: #01a7e1; text-decoration: none; color: #fff; -webkit-border-radius: 4px; border-radius: 4px; width: 100px; display: block; text-align: center;\" target=\"_blank\">Connect</a></div>", baseUrl, matched_user.Username);
                            sb.Append("</div><div style=\"clear:both; padding-bottom:65px;\"></div></div>");
                        }
                        if (sb.Length > 0)
                        {
                            using (var reader = new StreamReader(string.Format("{0}peopleyoumayknow.html", template)))
                            {
                                body = reader.ReadToEnd();
                                body = body.Replace("@@name", name);
                                body = body.Replace("@@content", sb.ToString());
                                string[] receipent = { item.Username };
                                var subject = "People May be Known To You";
                                if (user_match_list.Count > 1)
                                {
                                    body = body.Replace("@@type", "People");
                                }
                                else
                                {
                                    body = body.Replace("@@type", "Person");
                                }
                                MimeMessage mail = new MimeMessage();
                                mail.From.Add(new MailboxAddress("Joblisting", from));
                                mail.To.Add(new MailboxAddress((!string.IsNullOrEmpty(item.Company) ? item.Company : string.Format("{0} {1}", item.FirstName, item.LastName)), item.Username));
                                mail.Subject = subject;
                                mail.Body = new TextPart("html")
                                {
                                    Text = body
                                };
                                try
                                {
                                    oSmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                                    oSmtp.Connect("smtp.mailgun.org", 587, false);
                                    oSmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                                    oSmtp.Authenticate(postmail, postpassword);

                                    oSmtp.Send(mail);
                                    oSmtp.Disconnect(true);

                                    parameters = new List<DbParameter>();

                                    parameters.Add(new SqlParameter("@AlertId", alertType));
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
