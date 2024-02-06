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

namespace JoblistingVerificationService
{
    public class Reminder : DataService
    {
        public int Update(string username, int alert)
        {
            return HandleData(string.Format("INSERT INTO AlertHistories (AlertId, DateUpdated, Receiver) VALUES({0}, GETDATE(), '{1}')", alert, username));
        }
        public void Send()
        {
            List<DbParameter> param_list = new List<DbParameter>();
            param_list.Add(new SqlParameter("@Type", "First Reminder to Non Verified"));
            List<Entity> user_list = ReadData<Entity>("GetNonVerifiedUsers", param_list);
            foreach (var item in user_list)
            {
                List<DbParameter> parameters = new List<DbParameter>();
                parameters.Add(new SqlParameter("@CountryId", item.CountryId));
                List<JobEntity> jobs = ReadData<JobEntity>("GetTopFiveJobs ", parameters);
                if (jobs.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table style='width:100%' cellpadding='5px;'>");
                    sb.Append("<thead>");
                    sb.Append("<tr>");
                    sb.Append("<th style='border-bottom:solid 1px #dddddd; text-align:left'>Job Title</th>");
                    sb.Append("<th style='border-bottom:solid 1px #dddddd; text-align:left'>Action</th>");
                    sb.Append("</tr>");
                    sb.Append("</thead>");
                    sb.Append("<tbody>");
                    foreach (var jitem in jobs)
                    {
                        sb.Append("<tr>");
                        string job_link = string.Format("<a href=\"{0}job/{1}-{2}\">{3}</a>", item.BaseUrl, jitem.PermaLink, jitem.Id, jitem.Title);
                        string apply_link = string.Format("<a href=\"{0}job/apply?JobId={1}\">Apply</a>", item.BaseUrl, jitem.Id);
                        sb.AppendFormat("<td>{0}</td>", job_link);
                        sb.AppendFormat("<td>{0}</td>", apply_link);
                        sb.Append("</tr>");
                    }
                    sb.Append("</tbody>");
                    sb.Append("</table>");
                    using (var reader = new StreamReader(string.Format("{0}not_verified.html", item.Template)))
                    {
                        var body = reader.ReadToEnd();
                        body = body.Replace("@@receiver", item.FullName);
                        if (item.Type == 4)
                        {
                            body = body.Replace("@@content", sb.ToString());
                            body = body.Replace("@@typedesc", "Jobs posted in your area");
                        }
                        string postmail = ConfigurationManager.AppSettings["postmail"];
                        string postpassword = ConfigurationManager.AppSettings["postpassword"];
                        MimeMessage mail = new MimeMessage();
                        mail.From.Add(new MailboxAddress("Joblisting", "donotreply@joblisting.com"));
                        mail.To.Add(new MailboxAddress(item.FullName, item.Username));

                        if (item.Type == 4)
                        {
                            mail.Subject = "Jobs Posted at Joblisting";
                        }
                        mail.Body = new TextPart("html")
                        {
                            Text = body
                        };




                        try
                        {
                            using (var smtp = new SmtpClient())
                            {
                                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                                smtp.Connect("smtp.mailgun.org", 587, false);
                                smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                                smtp.Authenticate(postmail, postpassword);

                                smtp.Send(mail);
                                smtp.Disconnect(true);

                                int stat = Update(item.Username, item.AlertType);
                            }
                        }
                        catch (Exception ex)
                        {
                            SendEx(ex);
                        }

                    }
                }
            }
        }
        public void SendGender()
        {
            List<DbParameter> param_list = new List<DbParameter>();
            param_list.Add(new SqlParameter("@Type", "Second Reminder to Non Verified"));
            List<Entity> user_list = ReadData<Entity>("GetNonVerifiedUsers", param_list);
            foreach (var item in user_list)
            {
                List<DbParameter> parameters = new List<DbParameter>();
                parameters.Add(new SqlParameter("@UserId", item.Id));
                List<Entity> ulist = ReadData<Entity>("GetTopFiveIndividuals ", parameters);
                if (ulist.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var jitem in ulist)
                    {
                        string image = string.Format("<img src=\"{0}image/avtar?Id={1}\" style=\"width:64px; max-height:64px;\"/>", item.BaseUrl, jitem.Id);

                        sb.AppendFormat("<div><div style=\"float:left; width:64px; height:64px; border:1px solid #d7d7d7;display: table-cell;vertical-align: middle;\"><a href=\"{0}{1}\" target=\"_blank\">{2}</a></div>", item.BaseUrl, jitem.PermaLink, image);
                        sb.Append("<div style=\"float:left; padding-left:20px; height:64px;\">");
                        sb.AppendFormat("<div style=\"padding-bottom:5px;\">{0}</div>", jitem.FullName);
                        sb.AppendFormat("<div><a href=\"{0}connect?EmailAddress={1}&via=email\" style=\"padding: 10px;  background-color: #01a7e1; text-decoration: none; color: #fff; -webkit-border-radius: 4px; border-radius: 4px; width: 100px; display: block; text-align: center;\" target=\"_blank\">Connect</a></div>", item.BaseUrl, jitem.Username);
                        sb.Append("</div><div style=\"clear:both; padding-bottom:65px;\"></div></div>");
                    }

                    using (var reader = new StreamReader(string.Format("{0}not_verified.html", item.Template)))
                    {
                        var body = reader.ReadToEnd();
                        body = body.Replace("@@receiver", item.FullName);
                        if (item.Type == 4)
                        {
                            body = body.Replace("@@content", sb.ToString());
                            body = body.Replace("@@typedesc", "You may be interested to connect following person(s)");
                        }
                        string postmail = ConfigurationManager.AppSettings["postmail"];
                        string postpassword = ConfigurationManager.AppSettings["postpassword"];
                        MimeMessage mail = new MimeMessage();
                        mail.From.Add(new MailboxAddress("Joblisting", "donotreply@joblisting.com"));
                        mail.To.Add(new MailboxAddress(item.FullName, item.Username));

                        if (item.Type == 4)
                        {
                            mail.Subject = "You may be Interested to Connect";
                        }
                        mail.Body = new TextPart("html")
                        {
                            Text = body
                        };



                        try
                        {
                            using (var smtp = new SmtpClient())
                            {
                                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                                smtp.Connect("smtp.mailgun.org", 587, false);
                                smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                                smtp.Authenticate(postmail, postpassword);

                                smtp.Send(mail);
                                smtp.Disconnect(true);
                                int stat = Update(item.Username, item.AlertType);
                            }
                        }
                        catch (Exception ex)
                        {
                            SendEx(ex);
                        }
                    }
                }

            }
        }
        public void SendEx(Exception ex)
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
            mail.Subject = "Error occurred while running Joblisting Activity Service";
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
