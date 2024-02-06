using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;
using System.Linq;
//using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JoblistingAnnouncementService
{

    public partial class AnnouncementService : ServiceBase
    {
        public AnnouncementService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Task announce = new Task(new Action(Send), TaskCreationOptions.LongRunning);
            announce.Start();
        }

        protected override void OnStop()
        {
        }

        protected override void OnContinue()
        {
            Task announce = new Task(new Action(Send), TaskCreationOptions.LongRunning);
            announce.Start();
            base.OnContinue();
        }

        public void Send()
        {
            // Compose a message

            string from = ConfigurationManager.AppSettings["FromEmailAddress"];

            string templates = ConfigurationManager.AppSettings["Template"];
            string baseUrl = ConfigurationManager.AppSettings["SiteUrl"];
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            string body = string.Empty;
            while (true)
            {
                long aid = 0;
                string subject = "";
                int type = 0;
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = "SELECT Id, Subject, Type FROM Announcements WHERE Id = (SELECT MAX(Id) FROM Announcements WHERE Sent=0)";

                            using (SqlDataReader rdr = command.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    if (rdr["Id"] != DBNull.Value)
                                    {
                                        aid = Convert.ToInt64(rdr["Id"]);
                                        subject = Convert.ToString(rdr["Subject"]);
                                        type = Convert.ToInt32(rdr["Type"]);
                                    }
                                }
                            }
                        }

                        if (aid > 0)
                        {
                            string query = string.Empty;
                            if (type == 0)
                            {
                                query = string.Format("SELECT TOP 100000 UserId, Username, Company, FirstName, LastName FROM UserProfiles WHERE UserId NOT IN(SELECT UserId FROM UserAnnouncements WHERE Sent=0 AND AnnouncementId={0}) AND Type IN(4, 5) AND IsConfirmed = 1 AND IsActive = 1 AND IsDeleted = 0", aid);
                            }
                            else
                            {
                                query = string.Format("SELECT TOP 100000 UserId, Username, Company, FirstName, LastName FROM UserProfiles WHERE UserId NOT IN (SELECT UserId FROM UserAnnouncements WHERE Sent=0 AND AnnouncementId={0}) AND Type = {1} AND IsConfirmed = 1 AND IsActive = 1 AND IsDeleted = 0", aid, type);
                            }

                            try
                            {
                                List<UserEntity> userList = new List<UserEntity>();
                                using (SqlCommand command = conn.CreateCommand())
                                {
                                    command.CommandType = CommandType.Text;
                                    command.CommandText = query;

                                    using (SqlDataReader rdr = command.ExecuteReader())
                                    {
                                        while (rdr.Read())
                                        {
                                            UserEntity entity = new UserEntity()
                                            {
                                                Id = Convert.ToInt64(rdr["UserId"]),
                                                Email = Convert.ToString(rdr["Username"]),
                                                Company = Convert.ToString(rdr["Company"]),
                                                FirstName = Convert.ToString(rdr["FirstName"]),
                                                LastName = Convert.ToString(rdr["LastName"])
                                            };
                                            userList.Add(entity);
                                        }
                                        rdr.Close();
                                    }
                                }
                                MimeMessage mail = new MimeMessage();
                                
                                using (SmtpClient client = new SmtpClient())
                                {
                                    foreach (var item in userList)
                                    {
                                        try
                                        {
                                            if (!item.Email.ToLower().Contains("admin_"))
                                            {
                                                var reader = new StreamReader(string.Format("{0}announcement.html", templates));
                                                if (reader != null)
                                                {
                                                    string ebody = reader.ReadToEnd();
                                                    ebody = ebody.Replace("@@receiver", !string.IsNullOrEmpty(item.Company) ? item.Company : string.Format("{0} {1}", item.FirstName, item.LastName));
                                                    ebody = ebody.Replace("@@subject", subject);
                                                    ebody = ebody.Replace("@@viewurl", string.Format("{0}/Announcement/Show?Id={1}", baseUrl, aid));



                                                    mail.From.Add(new MailboxAddress("Joblisting", from));
                                                    mail.To.Add(new MailboxAddress("Excited User", item.Email));
                                                    mail.Subject = subject;
                                                    mail.Body = new TextPart("html")
                                                    {
                                                        Text = ebody,
                                                    };
                                                    try
                                                    {

                                                        // XXX - Should this be a little different?
                                                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                                                        client.Connect("smtp.mailgun.org", 587, false);
                                                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                                                        client.Authenticate(postmail, postpassword);

                                                        client.Send(mail);
                                                        client.Disconnect(true);

                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        SendEx(ex);
                                                    }

                                                }

                                                using (SqlCommand command = conn.CreateCommand())
                                                {
                                                    command.CommandType = CommandType.StoredProcedure;
                                                    command.CommandText = "USP_Announce";
                                                    command.Parameters.Add(new SqlParameter("@AnnouncementId", aid));
                                                    command.Parameters.Add(new SqlParameter("@UserId", item.Id));
                                                    command.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            SendEx(ex);
                                        }
                                    }

                                    using (SqlCommand command = conn.CreateCommand())
                                    {
                                        command.CommandType = CommandType.Text;
                                        command.CommandText = string.Format("UPDATE Announcements SET Sent=1 WHERE Id={0} --AND (SELECT COUNT(ID) FROM UserAnnouncements WHERE AnnouncementId={0}) > 0", aid);
                                        command.ExecuteNonQuery();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                conn.Close();
                                SendEx(ex);
                            }
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        SendEx(ex);
                    }
                }
                Thread.Sleep(10000);
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
            mail.Subject = "Error occured while running Joblisting Announcement Service";
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
    public class UserEntity
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
