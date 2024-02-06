//using JobPortal.Data;
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
using JobPortal.Library.Helpers;
using JobPortal.Library.Enumerators;
namespace JoblistingReminderService
{
    public class ReminderService : DataService
    {
        /// <summary>
        /// Send reminder to the friends to connect at joblisting.com
        /// </summary>
        public void InviteFriends()
        {
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            List<Friend> friends = ReadData<Friend>("GetFriendsList");

            using (SmtpClient oSmtp = new SmtpClient())
            {
                foreach (var item in friends)
                {
                    try
                    {
                        using (var reader = new StreamReader(string.Format("{0}invitation.html", item.Template)))
                        {
                            var body = reader.ReadToEnd();
                            string name = (string.IsNullOrEmpty(item.CFirstName) ? "" : item.CFirstName) + " " + (string.IsNullOrEmpty(item.CLastName) ? "" : item.CLastName);
                            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                            {
                                name = item.EmailAddress;
                            }

                            body = body.Replace("@@receiver", name);
                            body = body.Replace("@@sender", string.IsNullOrEmpty(item.Company) ? string.Format("{0} {1}", item.FirstName, item.LastName) : item.Company);
                            body = body.Replace("@@profileurl", string.Format("{0}/{1}", item.BaseUrl, item.PermaLink));
                            body = body.Replace("@@accepturl", string.Format("{0}/Network/Accept/{1}", item.BaseUrl, item.ConnectionId));
                            body = body.Replace("@@button", "Accept");

                            if (item.Registered > 0)
                            {
                                body = body.Replace("@@unsubscribe", "");
                            }
                            else
                            {
                                string ulink = string.Format("<a href=\"{0}/Network/Unsubscribe?Id={1}\">unsubscribe</a> or ", item.BaseUrl, item.ConnectionId);
                                body = body.Replace("@@unsubscribe", ulink);
                            }

                            string[] receipent = { item.EmailAddress };
                            var subject = string.Format("{0} Invites you to connect at Joblisting", (string.IsNullOrEmpty(item.Company) ? string.Format("{0} {1}", item.FirstName, item.LastName) : item.Company));
                            MimeMessage mail = new MimeMessage();
                            mail.From.Add(new MailboxAddress("Joblisting", from));
                            mail.To.Add(new MailboxAddress(name, item.EmailAddress));
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

                                List<DbParameter> parameters = new List<DbParameter>();
                                parameters.Add(new SqlParameter("@ConnectionId", item.ConnectionId));
                                HandleData("FriendReminderSent", parameters);
                            }
                            catch (Exception ex)
                            {
                                SendEx(ex);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        SendEx(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Send jobs expired reminder to employers
        /// </summary>
        public void JobExpired()
        {
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            List<ReminderEntity> job_expired_list = ReadData<ReminderEntity>("GetExpiredJobList");
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            using (SmtpClient oSmtp = new SmtpClient())
            {
                foreach (var employer in job_expired_list)
                {
                    bool remind = Remind(employer.Username, employer.AlertType);
                    if (remind)
                    {
                        using (var reader = new StreamReader(string.Format("{0}job_expired.html", employer.Template)))
                        {
                            var body = reader.ReadToEnd();
                            body = body.Replace("@@employer", employer.Company);
                            body = body.Replace("@@jobs", Convert.ToString(employer.Counts));
                            body = body.Replace("@@url", string.Format("{0}Employer/Index?Status=Expired", employer.BaseUrl));

                            var subject = "Your Job(s) Posted at Joblisting have Expired";
                            MimeMessage mail = new MimeMessage();
                            mail.From.Add(new MailboxAddress("Joblisting", from));

                            mail.To.Add(new MailboxAddress("Excited User", employer.Username));

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

                                List<DbParameter> parameters = new List<DbParameter>();
                                parameters.Add(new SqlParameter("@AlertId", employer.AlertType));
                                parameters.Add(new SqlParameter("@Receiver", employer.Username));
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

        /// <summary>
        /// Send jobs going to expire, reminder to employers
        /// </summary>
        public void JobsExpires()
        {
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            List<ReminderEntity> job_expired_list = ReadData<ReminderEntity>("GetJobsToExpireList");
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            using (SmtpClient oSmtp = new SmtpClient())
            {
                foreach (var employer in job_expired_list)
                {
                    bool remind = Remind(employer.Username, employer.AlertType);
                    if (remind)
                    {
                        using (var reader = new StreamReader(string.Format("{0}job_expiry_reminder.html", employer.Template)))
                        {
                            var body = reader.ReadToEnd();
                            body = body.Replace("@@employer", employer.Company);
                            body = body.Replace("@@jobs", Convert.ToString(employer.Counts));
                            body = body.Replace("@@url", string.Format("{0}Employer/Index", employer.BaseUrl));

                            var subject = "Your Job(s) Posted at Joblisting are Expiring Soon";
                            MimeMessage mail = new MimeMessage();
                            mail.From.Add(new MailboxAddress("Joblisting", from));

                            mail.To.Add(new MailboxAddress("Excited User", employer.Username));

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


                                List<DbParameter> parameters = new List<DbParameter>();

                                parameters.Add(new SqlParameter("@AlertId", employer.AlertType));
                                parameters.Add(new SqlParameter("@Receiver", employer.Username));
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


        /// <summary>
        /// Send applications waiting for actions, reminder to employers
        /// </summary>
        public void ApplicationReminder()
        {
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            List<ReminderEntity> job_expired_list = ReadData<ReminderEntity>("GetApplicationList");
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            using (SmtpClient oSmtp = new SmtpClient())
            {
                foreach (var employer in job_expired_list)
                {
                    bool remind = Remind(employer.Username, employer.AlertType, null, "Application");
                    if (remind)
                    {
                        using (var reader = new StreamReader(string.Format("{0}application_reminder.html", employer.Template)))
                        {
                            var body = reader.ReadToEnd();
                            body = body.Replace("@@employer", employer.Company);
                            body = body.Replace("@@apps", Convert.ToString(employer.Counts));
                            body = body.Replace("@@url", string.Format("{0}Employer/Applications", employer.BaseUrl));

                            var subject = "Job Application(s) Waiting for Your Action";
                            MimeMessage mail = new MimeMessage();
                            mail.From.Add(new MailboxAddress("Joblisting", from));

                            mail.To.Add(new MailboxAddress("Excited User", employer.Username));

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



                                List<DbParameter> parameters = new List<DbParameter>();

                                parameters.Add(new SqlParameter("@AlertId", employer.AlertType));
                                parameters.Add(new SqlParameter("@Receiver", employer.Username));
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

        /// <summary>
        /// Send auto-matched applications waiting for actions, reminder to employers
        /// </summary>
        public void AutomatchReminder()
        {
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            List<ReminderEntity> job_expired_list = ReadData<ReminderEntity>("GetAutomatchList");
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            using (SmtpClient oSmtp = new SmtpClient())
            {
                foreach (var employer in job_expired_list)
                {
                    bool remind = Remind(employer.Username, employer.AlertType, null, "Automatch");
                    if (remind)
                    {
                        using (var reader = new StreamReader(string.Format("{0}autoapps_reminder.html", employer.Template)))
                        {
                            var body = reader.ReadToEnd();
                            body = body.Replace("@@employer", employer.Company);
                            body = body.Replace("@@apps", Convert.ToString(employer.Counts));
                            body = body.Replace("@@url", string.Format("{0}Employer/Applications", employer.BaseUrl));

                            var subject = "Matched Profile(s) Waiting for Your Action";

                            MimeMessage mail = new MimeMessage();
                            mail.From.Add(new MailboxAddress("Joblisting", from));

                            mail.To.Add(new MailboxAddress("Excited User", employer.Username));

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

                                List<DbParameter> parameters = new List<DbParameter>();

                                parameters.Add(new SqlParameter("@AlertId", employer.AlertType));
                                parameters.Add(new SqlParameter("@Receiver", employer.Username));
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

        public void InterviewReminder()
        {
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            List<InterviewEntity> interview_list = ReadData<InterviewEntity>("GetInterviewList");
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            using (SmtpClient oSmtp = new SmtpClient())
            {
                foreach (var item in interview_list)
                {
                    bool remind = Remind(item.EmployerEmail, item.AlertType, null, "InterviewReminder", item.InterviewDate);
                    if (remind)
                    {
                        using (var reader = new StreamReader(string.Format("{0}interview_reminder.html", item.Template)))
                        {
                            var body = reader.ReadToEnd();
                            body = body.Replace("@@receiver", string.Format("{0} {1}", item.FirstName, item.LastName));
                            if (item.JobId != null)
                            {
                                string job_link = string.Format(" for <a href=\"{0}job/{1}-{2}\">{3}</a>", item.BaseUrl, item.JobUrl, item.JobId, item.JobTitle);
                                body = body.Replace("@@joblink", job_link);
                            }

                            body = body.Replace("@@round", ((InterviewRounds)item.Round).GetDescription());

                            string curl = string.Format("{0}{1}", item.BaseUrl, item.EmployerUrl);
                            body = body.Replace("@@companyurl", curl);
                            body = body.Replace("@@employer", item.EmployerName);

                            body = body.Replace("@@datetime", item.InterviewDate.ToString("MMM-dd-yyyy hh:mm tt"));
                            body = body.Replace("@@interviewer", item.Interviewer);

                            string url = string.Format("{0}{1}", item.BaseUrl, item.JobseekerUrl);
                            body = body.Replace("@@profileurl", url);
                            body = body.Replace("@@jobseeker", string.Format("{0} {1}", item.FirstName, item.LastName));
                            string vurl = string.Format("{0}Interview/Update?Id={1}", item.BaseUrl, item.Id);
                            body = body.Replace("@@viewurl", vurl);

                            var subject = "Interview Schedule Reminder";
                            MimeMessage mail = new MimeMessage();
                            mail.From.Add(new MailboxAddress("Joblisting", from));

                            mail.To.Add(new MailboxAddress("Excited User", item.JobseekerEmail));

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
                                List<DbParameter> parameters = new List<DbParameter>();

                                parameters.Add(new SqlParameter("@AlertId", item.AlertType));
                                parameters.Add(new SqlParameter("@Receiver", item.JobseekerEmail));
                                parameters.Add(new SqlParameter("@Sender", from));
                                parameters.Add(new SqlParameter("@ReferenceId", item.Id));

                                int stat = HandleData("TrackAlertHistory", parameters);
                            }
                            catch (Exception ex)
                            {
                                SendEx(ex);
                            }

                        }

                        using (var reader = new StreamReader(string.Format("{0}interview_reminder.html", item.Template)))
                        {
                            var body = reader.ReadToEnd();
                            body = body.Replace("@@receiver", item.EmployerName);
                            if (item.JobId != null)
                            {
                                string job_link = string.Format(" for <a href=\"{0}job/{1}-{2}\">{3}</a>", item.BaseUrl, item.JobUrl, item.JobId, item.JobTitle);
                                body = body.Replace("@@joblink", job_link);
                            }
                            body = body.Replace("@@round", ((InterviewRounds)item.Round).GetDescription());

                            string curl = string.Format("{0}{1}", item.BaseUrl, item.EmployerUrl);
                            body = body.Replace("@@companyurl", curl);
                            body = body.Replace("@@employer", item.EmployerName);

                            body = body.Replace("@@datetime", item.InterviewDate.ToString("MMM-dd-yyyy hh:mm tt"));
                            body = body.Replace("@@interviewer", item.Interviewer);

                            string url = string.Format("{0}{1}", item.BaseUrl, item.JobseekerUrl);
                            body = body.Replace("@@profileurl", url);
                            body = body.Replace("@@jobseeker", string.Format("{0} {1}", item.FirstName, item.LastName));

                            string vurl = string.Format("{0}Interview/Update?Id={1}", item.BaseUrl, item.Id);
                            body = body.Replace("@@viewurl", vurl);

                            var subject = "Interview Schedule Reminder";
                            MimeMessage mail = new MimeMessage();
                            mail.From.Add(new MailboxAddress("Joblisting", from));

                            mail.To.Add(new MailboxAddress("Excited User", item.EmployerEmail));

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

                                List<DbParameter> parameters = new List<DbParameter>();

                                parameters.Add(new SqlParameter("@AlertId", item.AlertType));
                                parameters.Add(new SqlParameter("@Receiver", item.EmployerEmail));
                                parameters.Add(new SqlParameter("@Sender", from));
                                parameters.Add(new SqlParameter("@ReferenceId", item.Id));

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

        public void HourBeforeInterviewReminder()
        {
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            List<InterviewEntity> interview_list = ReadData<InterviewEntity>("GetBeforeInterviewList");
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            using (SmtpClient oSmtp = new SmtpClient())
            {
                foreach (var item in interview_list)
                {

                    using (var reader = new StreamReader(string.Format("{0}interview_reminder.html", item.Template)))
                    {
                        var body = reader.ReadToEnd();
                        body = body.Replace("@@receiver", string.Format("{0} {1}", item.FirstName, item.LastName));
                        if (item.JobId != null)
                        {
                            string job_link = string.Format(" for <a href=\"{0}job/{1}-{2}\">{3}</a>", item.BaseUrl, item.JobUrl, item.JobId, item.JobTitle);
                            body = body.Replace("@@joblink", job_link);
                        }

                        body = body.Replace("@@round", ((InterviewRounds)item.Round).GetDescription());

                        string curl = string.Format("{0}{1}", item.BaseUrl, item.EmployerUrl);
                        body = body.Replace("@@companyurl", curl);
                        body = body.Replace("@@employer", item.EmployerName);

                        body = body.Replace("@@datetime", item.InterviewDate.ToString("MMM-dd-yyyy hh:mm tt"));
                        body = body.Replace("@@interviewer", item.Interviewer);

                        string url = string.Format("{0}{1}", item.BaseUrl, item.JobseekerUrl);
                        body = body.Replace("@@profileurl", url);
                        body = body.Replace("@@jobseeker", string.Format("{0} {1}", item.FirstName, item.LastName));
                        string vurl = string.Format("{0}Interview/Update?Id={1}", item.BaseUrl, item.Id);
                        body = body.Replace("@@viewurl", vurl);

                        var subject = "Interview Schedule Reminder";
                        MimeMessage mail = new MimeMessage();
                        mail.From.Add(new MailboxAddress("Joblisting", from));

                        mail.To.Add(new MailboxAddress("Excited User", item.JobseekerEmail));

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



                            List<DbParameter> parameters = new List<DbParameter>();

                            parameters.Add(new SqlParameter("@AlertId", item.AlertType));
                            parameters.Add(new SqlParameter("@Receiver", item.JobseekerEmail));
                            parameters.Add(new SqlParameter("@Sender", from));
                            parameters.Add(new SqlParameter("@ReferenceId", item.Id));

                            int stat = HandleData("TrackAlertHistory", parameters);
                        }
                        catch (Exception ex)
                        {
                            SendEx(ex);
                        }

                    }

                    using (var reader = new StreamReader(string.Format("{0}interview_reminder.html", item.Template)))
                    {
                        var body = reader.ReadToEnd();
                        body = body.Replace("@@receiver", item.EmployerName);
                        if (item.JobId != null)
                        {
                            string job_link = string.Format(" for <a href=\"{0}job/{1}-{2}\">{3}</a>", item.BaseUrl, item.JobUrl, item.JobId, item.JobTitle);
                            body = body.Replace("@@joblink", job_link);
                        }
                        body = body.Replace("@@round", ((InterviewRounds)item.Round).GetDescription());

                        string curl = string.Format("{0}{1}", item.BaseUrl, item.EmployerUrl);
                        body = body.Replace("@@companyurl", curl);
                        body = body.Replace("@@employer", item.EmployerName);

                        body = body.Replace("@@datetime", item.InterviewDate.ToString("MMM-dd-yyyy hh:mm tt"));
                        body = body.Replace("@@interviewer", item.Interviewer);

                        string url = string.Format("{0}{1}", item.BaseUrl, item.JobseekerUrl);
                        body = body.Replace("@@profileurl", url);
                        body = body.Replace("@@jobseeker", string.Format("{0} {1}", item.FirstName, item.LastName));

                        string vurl = string.Format("{0}Interview/Update?Id={1}", item.BaseUrl, item.Id);
                        body = body.Replace("@@viewurl", vurl);

                        var subject = "Interview Schedule Reminder";
                        MimeMessage mail = new MimeMessage();
                        mail.From.Add(new MailboxAddress("Joblisting", from));

                        mail.To.Add(new MailboxAddress("Excited User", item.EmployerEmail));

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


                            List<DbParameter> parameters = new List<DbParameter>();

                            parameters.Add(new SqlParameter("@AlertId", item.AlertType));
                            parameters.Add(new SqlParameter("@Receiver", item.EmployerEmail));
                            parameters.Add(new SqlParameter("@Sender", from));
                            parameters.Add(new SqlParameter("@ReferenceId", item.Id));

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

        public void InterviewFollowUps()
        {
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            List<InterviewEntity> interview_list = ReadData<InterviewEntity>("GetInterviewFollowUpList");
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            using (SmtpClient oSmtp = new SmtpClient())
            {
                foreach (var item in interview_list)
                {
                    string eTemplate = string.Empty;
                    string jTemplate = string.Empty;
                    FeedbackStatus feedback = (FeedbackStatus)item.FollowUpStatus;
                    bool remind = false;
                    switch (feedback)
                    {
                        case FeedbackStatus.INVITED:
                            jTemplate = string.Format("{0}interview_invitation_followup_jobseeker.html", item.Template);
                            eTemplate = string.Format("{0}interview_invitation_followup_employer.html", item.Template);
                            remind = Remind(item.EmployerEmail, item.AlertType, item.Id);
                            if (remind)
                            {
                                /* Send mail to Employer */
                                using (var reader = new StreamReader(eTemplate))
                                {
                                    var body = reader.ReadToEnd();
                                    body = body.Replace("@@firstname", string.Format("{0}", item.FirstName));
                                    body = body.Replace("@@lastname", string.Format("{0}", item.LastName));
                                    body = body.Replace("@@employer", string.Format("{0}", item.EmployerName));
                                    if (item.JobId != null)
                                    {
                                        string job_link = string.Format(" for <a href=\"{0}job/{1}-{2}\">{3}</a>", item.BaseUrl, item.JobUrl, item.JobId, item.JobTitle);
                                        body = body.Replace("@@type", job_link);
                                    }
                                    else
                                    {
                                        body = body.Replace("@@type", " ");
                                    }

                                    body = body.Replace("@@round", ((InterviewRounds)item.Round).GetDescription());

                                    string curl = string.Format("{0}{1}", item.BaseUrl, item.EmployerUrl);
                                    body = body.Replace("@@companyurl", curl);
                                    body = body.Replace("@@datetime", item.InterviewDate.ToString("MMM-dd-yyyy hh:mm tt"));
                                    body = body.Replace("@@interviewer", item.Interviewer);

                                    string url = string.Format("{0}{1}", item.BaseUrl, item.JobseekerUrl);
                                    body = body.Replace("@@profileurl", url);

                                    string vurl = string.Format("{0}Interview/Update?Id={1}", item.BaseUrl, item.Id);
                                    body = body.Replace("@@viewurl", vurl);

                                    var subject = string.Empty;
                                    if (item.UserType == (int)SecurityRoles.Employers)
                                    {
                                        subject = "Interview Schedule";
                                        body = body.Replace("@@action", "View");
                                    }
                                    else
                                    {
                                        subject = "Interview Invitation Waiting for Action";
                                        body = body.Replace("@@action", "Take Action");
                                    }
                                    MimeMessage mail = new MimeMessage();
                                    mail.From.Add(new MailboxAddress("Joblisting", from));

                                    mail.To.Add(new MailboxAddress(item.EmployerName, item.EmployerEmail));

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


                                        List<DbParameter> parameters = new List<DbParameter>();

                                        parameters.Add(new SqlParameter("@AlertId", item.AlertType));
                                        parameters.Add(new SqlParameter("@Receiver", item.EmployerEmail));
                                        parameters.Add(new SqlParameter("@Sender", from));
                                        parameters.Add(new SqlParameter("@ReferenceId", item.Id));

                                        int stat = HandleData("TrackAlertHistory", parameters);
                                    }
                                    catch (Exception ex)
                                    {
                                        SendEx(ex);
                                    }
                                }


                                /* Send mail to Jobseeker */
                                using (var reader = new StreamReader(jTemplate))
                                {
                                    var body = reader.ReadToEnd();
                                    body = body.Replace("@@firstname", string.Format("{0}", item.FirstName));
                                    body = body.Replace("@@lastname", string.Format("{0}", item.LastName));
                                    body = body.Replace("@@employer", string.Format("{0}", item.EmployerName));
                                    if (item.JobId != null)
                                    {
                                        string job_link = string.Format(" for <a href=\"{0}job/{1}-{2}\">{3}</a>", item.BaseUrl, item.JobUrl, item.JobId, item.JobTitle);
                                        body = body.Replace("@@type", job_link);
                                    }
                                    else
                                    {
                                        body = body.Replace("@@type", " ");
                                    }

                                    body = body.Replace("@@round", ((InterviewRounds)item.Round).GetDescription());

                                    string curl = string.Format("{0}{1}", item.BaseUrl, item.EmployerUrl);
                                    body = body.Replace("@@companyurl", curl);
                                    body = body.Replace("@@datetime", item.InterviewDate.ToString("MMM-dd-yyyy hh:mm tt"));
                                    body = body.Replace("@@interviewer", item.Interviewer);

                                    string url = string.Format("{0}{1}", item.BaseUrl, item.JobUrl);
                                    body = body.Replace("@@profileurl", url);

                                    string vurl = string.Format("{0}Interview/Update?Id={1}", item.BaseUrl, item.Id);
                                    body = body.Replace("@@viewurl", vurl);

                                    var subject = string.Empty;
                                    if (item.UserType == (int)SecurityRoles.Jobseeker)
                                    {
                                        subject = "Interview Schedule";
                                        body = body.Replace("@@action", "View");
                                    }
                                    else
                                    {
                                        subject = "Interview Invitation Waiting for Action";
                                        body = body.Replace("@@action", "Take Action");
                                    }
                                    MimeMessage mail = new MimeMessage();
                                    mail.From.Add(new MailboxAddress("Joblisting", from));
                                    
                                    mail.To.Add(new MailboxAddress(string.Format("{0} {1}", item.FirstName, item.LastName), item.JobseekerEmail));

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

                                        
                                        List<DbParameter> parameters = new List<DbParameter>();

                                        parameters.Add(new SqlParameter("@AlertId", item.AlertType));
                                        parameters.Add(new SqlParameter("@Receiver", item.JobseekerEmail));
                                        parameters.Add(new SqlParameter("@Sender", from));
                                        parameters.Add(new SqlParameter("@ReferenceId", item.Id));

                                        int stat = HandleData("TrackAlertHistory", parameters);
                                    }
                                    catch (Exception ex)
                                    {
                                        SendEx(ex);
                                    }
                                }
                            }

                            break;
                        case FeedbackStatus.RESCHEDULE:

                            switch ((SecurityRoles)item.UserType)
                            {
                                case SecurityRoles.Employers:
                                    jTemplate = string.Format("{0}interview_followup_jobseeker_action.html", item.Template);
                                    eTemplate = string.Format("{0}interview_followup_employer_view.html", item.Template);
                                    remind = Remind(item.EmployerEmail, item.AlertType, item.Id);
                                    if (remind)
                                    {
                                        using (var reader = new StreamReader(eTemplate))
                                        {
                                            var body = reader.ReadToEnd();
                                            body = body.Replace("@@firstname", string.Format("{0}", item.FirstName));
                                            body = body.Replace("@@lastname", string.Format("{0}", item.LastName));
                                            body = body.Replace("@@employer", string.Format("{0}", item.EmployerName));
                                            if (item.JobId != null)
                                            {
                                                string job_link = string.Format(" for <a href=\"{0}job/{1}-{2}\">{3}</a>", item.BaseUrl, item.JobUrl, item.JobId, item.JobTitle);
                                                body = body.Replace("@@type", job_link);
                                            }
                                            else
                                            {
                                                body = body.Replace("@@type", " ");
                                            }

                                            body = body.Replace("@@round", ((InterviewRounds)item.Round).GetDescription());

                                            string curl = string.Format("{0}{1}", item.BaseUrl, item.EmployerUrl);
                                            body = body.Replace("@@companyurl", curl);
                                            body = body.Replace("@@datetime", item.InterviewDate.ToString("MMM-dd-yyyy hh:mm tt"));
                                            body = body.Replace("@@interviewer", item.Interviewer);

                                            string url = string.Format("{0}{1}", item.BaseUrl, item.JobseekerUrl);
                                            body = body.Replace("@@profileurl", url);

                                            string vurl = string.Format("{0}Interview/Update?Id={1}", item.BaseUrl, item.Id);
                                            body = body.Replace("@@viewurl", vurl);

                                            var subject = "Interview Reschedule";
                                            body = body.Replace("@@action", "View");

                                            MimeMessage mail = new MimeMessage();
                                            mail.From.Add(new MailboxAddress("Joblisting", from));

                                            mail.To.Add(new MailboxAddress(item.EmployerName, item.EmployerEmail));

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

                                                List<DbParameter> parameters = new List<DbParameter>();

                                                parameters.Add(new SqlParameter("@AlertId", item.AlertType));
                                                parameters.Add(new SqlParameter("@Receiver", item.EmployerEmail));
                                                parameters.Add(new SqlParameter("@Sender", from));
                                                parameters.Add(new SqlParameter("@ReferenceId", item.Id));
                                                int stat = HandleData("TrackAlertHistory", parameters);

                                            }
                                            catch (Exception ex)
                                            {
                                                SendEx(ex);
                                            }
                                        }



                                        using (var reader = new StreamReader(jTemplate))
                                        {
                                            var body = reader.ReadToEnd();
                                            body = body.Replace("@@firstname", string.Format("{0}", item.FirstName));
                                            body = body.Replace("@@lastname", string.Format("{0}", item.LastName));
                                            body = body.Replace("@@employer", string.Format("{0}", item.EmployerName));
                                            if (item.JobId != null)
                                            {
                                                string job_link = string.Format(" for <a href=\"{0}job/{1}-{2}\">{3}</a>", item.BaseUrl, item.JobUrl, item.JobId, item.JobTitle);
                                                body = body.Replace("@@type", job_link);
                                            }
                                            else
                                            {
                                                body = body.Replace("@@type", " ");
                                            }

                                            body = body.Replace("@@round", ((InterviewRounds)item.Round).GetDescription());

                                            string curl = string.Format("{0}{1}", item.BaseUrl, item.EmployerUrl);
                                            body = body.Replace("@@companyurl", curl);
                                            body = body.Replace("@@datetime", item.InterviewDate.ToString("MMM-dd-yyyy hh:mm tt"));
                                            body = body.Replace("@@interviewer", item.Interviewer);

                                            string url = string.Format("{0}{1}", item.BaseUrl, item.JobseekerUrl);
                                            body = body.Replace("@@profileurl", url);

                                            string vurl = string.Format("{0}Interview/Update?Id={1}", item.BaseUrl, item.Id);
                                            body = body.Replace("@@viewurl", vurl);

                                            var subject = "Interview Reschedule";
                                            body = body.Replace("@@action", "Take Action");


                                            MimeMessage mail = new MimeMessage();
                                            mail.From.Add(new MailboxAddress("Joblisting", from));

                                            mail.To.Add(new MailboxAddress(string.Format("{0} {1}", item.FirstName, item.LastName), item.JobseekerEmail));

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

                                                List<DbParameter> parameters = new List<DbParameter>();

                                                parameters.Add(new SqlParameter("@AlertId", item.AlertType));
                                                parameters.Add(new SqlParameter("@Receiver", item.JobseekerEmail));
                                                parameters.Add(new SqlParameter("@Sender", from));
                                                parameters.Add(new SqlParameter("@ReferenceId", item.Id));

                                                int stat = HandleData("TrackAlertHistory", parameters);
                                            }
                                            catch (Exception ex)
                                            {
                                                SendEx(ex);
                                            }
                                        }
                                    }

                                    break;
                                case SecurityRoles.Jobseeker:
                                    jTemplate = string.Format("{0}interview_followup_jobseeker_view.html", item.Template);
                                    eTemplate = string.Format("{0}interview_followup_employer_action.html", item.Template);
                                    remind = Remind(item.EmployerEmail, item.AlertType, item.Id);
                                    if (remind)
                                    {
                                        using (var reader = new StreamReader(eTemplate))
                                        {
                                            var body = reader.ReadToEnd();
                                            body = body.Replace("@@firstname", string.Format("{0}", item.FirstName));
                                            body = body.Replace("@@lastname", string.Format("{0}", item.LastName));
                                            body = body.Replace("@@employer", string.Format("{0}", item.EmployerName));
                                            if (item.JobId != null)
                                            {
                                                string job_link = string.Format(" for <a href=\"{0}job/{1}-{2}\">{3}</a>", item.BaseUrl, item.JobUrl, item.JobId, item.JobTitle);
                                                body = body.Replace("@@type", job_link);
                                            }
                                            else
                                            {
                                                body = body.Replace("@@type", " ");
                                            }

                                            body = body.Replace("@@round", ((InterviewRounds)item.Round).GetDescription());

                                            string curl = string.Format("{0}{1}", item.BaseUrl, item.EmployerUrl);
                                            body = body.Replace("@@companyurl", curl);
                                            body = body.Replace("@@datetime", item.InterviewDate.ToString("MMM-dd-yyyy hh:mm tt"));
                                            body = body.Replace("@@interviewer", item.Interviewer);

                                            string url = string.Format("{0}{1}", item.BaseUrl, item.JobseekerUrl);
                                            body = body.Replace("@@profileurl", url);

                                            string vurl = string.Format("{0}Interview/Update?Id={1}", item.BaseUrl, item.Id);
                                            body = body.Replace("@@viewurl", vurl);

                                            var subject = "Interview Reschedule";
                                            body = body.Replace("@@action", "Take Action");



                                            MimeMessage mail = new MimeMessage();
                                            mail.From.Add(new MailboxAddress("Joblisting", from));

                                            mail.To.Add(new MailboxAddress(item.EmployerName, item.EmployerEmail));

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

                                                List<DbParameter> parameters = new List<DbParameter>();

                                                parameters.Add(new SqlParameter("@AlertId", item.AlertType));
                                                parameters.Add(new SqlParameter("@Receiver", item.EmployerEmail));
                                                parameters.Add(new SqlParameter("@Sender", from));
                                                parameters.Add(new SqlParameter("@ReferenceId", item.Id));
                                                int stat = HandleData("TrackAlertHistory", parameters);

                                            }
                                            catch (Exception ex)
                                            {
                                                SendEx(ex);
                                            }
                                        }


                                        using (var reader = new StreamReader(jTemplate))
                                        {
                                            var body = reader.ReadToEnd();
                                            body = body.Replace("@@firstname", string.Format("{0}", item.FirstName));
                                            body = body.Replace("@@lastname", string.Format("{0}", item.LastName));
                                            body = body.Replace("@@employer", string.Format("{0}", item.EmployerName));
                                            if (item.JobId != null)
                                            {
                                                string job_link = string.Format(" for <a href=\"{0}job/{1}-{2}\">{3}</a>", item.BaseUrl, item.JobUrl, item.JobId, item.JobTitle);
                                                body = body.Replace("@@type", job_link);
                                            }
                                            else
                                            {
                                                body = body.Replace("@@type", " ");
                                            }

                                            body = body.Replace("@@round", ((InterviewRounds)item.Round).GetDescription());

                                            string curl = string.Format("{0}{1}", item.BaseUrl, item.EmployerUrl);
                                            body = body.Replace("@@companyurl", curl);
                                            body = body.Replace("@@datetime", item.InterviewDate.ToString("MMM-dd-yyyy hh:mm tt"));
                                            body = body.Replace("@@interviewer", item.Interviewer);

                                            string url = string.Format("{0}{1}", item.BaseUrl, item.JobseekerUrl);
                                            body = body.Replace("@@profileurl", url);

                                            string vurl = string.Format("{0}Interview/Update?Id={1}", item.BaseUrl, item.Id);
                                            body = body.Replace("@@viewurl", vurl);

                                            var subject = "Interview Reschedule";
                                            body = body.Replace("@@action", "View");


                                            MimeMessage mail = new MimeMessage();
                                            mail.From.Add(new MailboxAddress("Joblisting", from));

                                            mail.To.Add(new MailboxAddress(string.Format("{0} {1}", item.FirstName, item.LastName), item.JobseekerEmail));

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

                                                List<DbParameter> parameters = new List<DbParameter>();

                                                parameters.Add(new SqlParameter("@AlertId", item.AlertType));
                                                parameters.Add(new SqlParameter("@Receiver", item.JobseekerEmail));
                                                parameters.Add(new SqlParameter("@Sender", from));
                                                parameters.Add(new SqlParameter("@ReferenceId", item.Id));
                                                int stat = HandleData("TrackAlertHistory", parameters);
                                            }
                                            catch (Exception ex)
                                            {
                                                SendEx(ex);
                                            }

                                        }
                                    }
                                    break;
                            }
                            break;
                    }

                }
            }
        }

        /// <summary>
        /// Send message waiting for action, reminder to user.
        /// </summary>
        public void MessageReminder()
        {
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            List<ReminderEntity> message_list = ReadData<ReminderEntity>("GetMessageList");
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            using (SmtpClient oSmtp = new SmtpClient())
            {
                foreach (var item in message_list)
                {
                    bool remind = Remind(item.Username, item.AlertType, null, "Message");
                    if (remind)
                    {
                        using (var reader = new StreamReader(string.Format("{0}message_reminder.html", item.Template)))
                        {
                            var body = reader.ReadToEnd();
                            if (!string.IsNullOrEmpty(item.Company))
                            {
                                body = body.Replace("@@user", item.Company);
                            }
                            else
                            {
                                body = body.Replace("@@user", string.Format("{0} {1}", item.FirstName, item.LastName));
                            }
                            body = body.Replace("@@msgs", Convert.ToString(item.Counts));
                            body = body.Replace("@@url", string.Format("{0}Message/Index", item.BaseUrl));

                            var subject = "Message(s) Waiting for Your Action";



                            MimeMessage mail = new MimeMessage();
                            mail.From.Add(new MailboxAddress("Joblisting", from));

                            mail.To.Add(new MailboxAddress(item.Username, item.Username));

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
                                List<DbParameter> parameters = new List<DbParameter>();

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

        public void IndividualsList()
        {
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            List<IndividualListEntity> individual_list = ReadData<IndividualListEntity>("GetIndividualsList");
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            string baseUrl = string.Empty;
            string template = string.Empty;
            int alertType = 0;
            if (individual_list.Count > 0)
            {
                baseUrl = individual_list.Select(x => x.BaseUrl).FirstOrDefault();
                template = individual_list.Select(x => x.Template).FirstOrDefault();
                alertType = individual_list.Select(x => x.AlertType).FirstOrDefault();
            }
            List<UserEntity> employers = individual_list.Select(x => new UserEntity() { UserId = x.UserId, Username = x.Username, FirstName = x.FirstName, LastName = x.LastName, Company = x.Company, PermaLink = x.PermaLink, CountryId = x.CountryId }).Distinct().ToList();
            using (SmtpClient oSmtp = new SmtpClient())
            {
                foreach (var employer in employers)
                {
                    List<DbParameter> parameters = new List<DbParameter>();
                    parameters.Add(new SqlParameter("@AlertId", alertType));
                    parameters.Add(new SqlParameter("@Username", employer.Username));

                    object dateSent = ReadDataField("GetAlertSentOn", parameters);
                    DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;

                    bool remind = false;
                    if (dateSent == null || dateSent == DBNull.Value)
                    {
                        remind = (dayOfWeek == DayOfWeek.Monday);
                    }
                    else
                    {
                        DateTime dated = Convert.ToDateTime(dateSent);
                        if (!dated.Date.Equals(DateTime.Now.Date) && dayOfWeek == DayOfWeek.Monday)
                        {
                            remind = true;
                        }
                    }

                    if (remind)
                    {
                        List<UserEntity> individuals = individual_list.Where(x => x.UserId == employer.UserId && x.IUsername != null).Select(x => new UserEntity() { UserId = x.IUserId, Username = x.IUsername, FirstName = x.IFirstName, LastName = x.ILastName, Company = x.ICompany, PermaLink = x.IPermaLink }).Distinct().ToList();
                        StringBuilder sbHtml = new StringBuilder();
                        foreach (var individual in individuals)
                        {
                            StringBuilder sb = new StringBuilder();
                            string name = string.Format("{0} {1}", individual.FirstName, individual.LastName);

                            string image = string.Empty;

                            image = string.Format("<img src=\"{0}image/avtar?Id={1}\" style=\"width:64px; max-height:64px;\"/>", baseUrl, individual.UserId);

                            sb.AppendFormat("<div><div style=\"float:left; width:64px; height:64px; border:1px solid #d7d7d7;display: table-cell;vertical-align: middle;\"><a href=\"{0}{1}\" target=\"_blank\">{2}</a></div>", baseUrl, individual.PermaLink, image);
                            sb.Append("<div style=\"float:left; padding-left:20px; height:64px;\">");
                            sb.AppendFormat("<div style=\"padding-bottom:5px;\">{0}</div>", name);
                            sb.AppendFormat("<div><a href=\"{0}connect?EmailAddress={1}&via=email\" style=\"padding: 10px;  background-color: #01a7e1; text-decoration: none; color: #fff; -webkit-border-radius: 4px; border-radius: 4px; width: 100px; display: block; text-align: center;\" target=\"_blank\">Connect</a></div>", baseUrl, individual.Username);
                            sb.Append("</div><div style=\"clear:both; padding-bottom:65px;\"></div></div>");

                            sbHtml.Append(sb.ToString());
                        }

                        if (sbHtml.Length > 0)
                        {
                            using (var reader = new StreamReader(string.Format("{0}individuals.html", template)))
                            {
                                var body = reader.ReadToEnd();
                                body = body.Replace("@@company", employer.Company);
                                body = body.Replace("@@content", sbHtml.ToString());
                                body = body.Replace("@@viewurl", string.Format("{0}individuals?countryId={1}", baseUrl, employer.CountryId));
                                body = body.Replace("@@unsubscribe", "");



                                MimeMessage mail = new MimeMessage();
                                mail.From.Add(new MailboxAddress("Joblisting", from));

                                mail.To.Add(new MailboxAddress(employer.Username, employer.Username));

                                mail.Subject = "Individuals You May be Interested In";
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
                                    parameters.Add(new SqlParameter("@Receiver", employer.Username));
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

        public void CompanyList()
        {
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            List<IndividualListEntity> company_list = ReadData<IndividualListEntity>("GetCompanyList");
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            string baseUrl = string.Empty;
            string template = string.Empty;
            int alertType = 0;
            if (company_list.Count > 0)
            {
                baseUrl = company_list.Select(x => x.BaseUrl).FirstOrDefault();
                template = company_list.Select(x => x.Template).FirstOrDefault();
                alertType = company_list.Select(x => x.AlertType).FirstOrDefault();
            }
            List<UserEntity> individuals = company_list.Select(x => new UserEntity() { UserId = x.UserId, Username = x.Username, FirstName = x.FirstName, LastName = x.LastName, Company = x.Company, PermaLink = x.PermaLink, CountryId = x.CountryId }).Distinct().ToList();
            using (SmtpClient oSmtp = new SmtpClient())
            {
                foreach (var individual in individuals)
                {
                    List<DbParameter> parameters = new List<DbParameter>();
                    parameters.Add(new SqlParameter("@AlertId", alertType));
                    parameters.Add(new SqlParameter("@Username", individual.Username));

                    object dateSent = ReadDataField("GetAlertSentOn", parameters);
                    DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;

                    bool remind = false;
                    if (dateSent == null || dateSent == DBNull.Value)
                    {
                        remind = (dayOfWeek == DayOfWeek.Monday);
                    }
                    else
                    {
                        DateTime dated = Convert.ToDateTime(dateSent);
                        if (!dated.Date.Equals(DateTime.Now.Date) && dayOfWeek == DayOfWeek.Monday)
                        {
                            remind = true;
                        }
                    }

                    if (remind)
                    {
                        List<UserEntity> employers = company_list.Where(x => x.UserId == individual.UserId && x.IUsername != null).Select(x => new UserEntity() { UserId = x.IUserId, Username = x.IUsername, FirstName = x.IFirstName, LastName = x.ILastName, Company = x.ICompany, PermaLink = x.IPermaLink }).Distinct().ToList();
                        StringBuilder sbHtml = new StringBuilder();
                        foreach (var employer in employers)
                        {
                            StringBuilder sb = new StringBuilder();
                            string name = string.Format("{0}", employer.Company);

                            string image = string.Empty;

                            image = string.Format("<img src=\"{0}image/avtar?Id={1}\" style=\"width:64px; max-height:64px;\"/>", baseUrl, employer.UserId);

                            sb.AppendFormat("<div><div style=\"float:left; width:64px; height:64px; border:1px solid #d7d7d7;display: table-cell;vertical-align: middle;\"><a href=\"{0}{1}\" target=\"_blank\">{2}</a></div>", baseUrl, employer.PermaLink, image);
                            sb.Append("<div style=\"float:left; padding-left:20px; height:64px;\">");
                            sb.AppendFormat("<div style=\"padding-bottom:5px;\">{0}</div>", name);
                            sb.AppendFormat("<div><a href=\"{0}connect?EmailAddress={1}&via=email\" style=\"padding: 10px;  background-color: #01a7e1; text-decoration: none; color: #fff; -webkit-border-radius: 4px; border-radius: 4px; width: 100px; display: block; text-align: center;\" target=\"_blank\">Connect</a></div>", baseUrl, employer.Username);
                            sb.Append("</div><div style=\"clear:both; padding-bottom:65px;\"></div></div>");

                            sbHtml.Append(sb.ToString());
                        }

                        if (sbHtml.Length > 0)
                        {
                            using (var reader = new StreamReader(string.Format("{0}companies.html", template)))
                            {
                                var body = reader.ReadToEnd();
                                body = body.Replace("@@firstname", individual.FirstName);
                                body = body.Replace("@@lastname", individual.LastName);
                                body = body.Replace("@@content", sbHtml.ToString());
                                body = body.Replace("@@viewurl", string.Format("{0}companies?countryId={1}", baseUrl, individual.CountryId));
                                body = body.Replace("@@unsubscribe", "");



                                MimeMessage mail = new MimeMessage();
                                mail.From.Add(new MailboxAddress("Joblisting", from));

                                mail.To.Add(new MailboxAddress("", individual.Username));

                                mail.Subject = "Companies You May be Interested In";
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
                                    parameters.Add(new SqlParameter("@Receiver", individual.Username));
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
            mail.Subject = "Error occured while running Joblisting Reminder Service";
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
      
        public bool Remind(string username, int alertType, long? referenceId = null, string reminder = "", DateTime? refDate = null)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@AlertId", alertType));
            parameters.Add(new SqlParameter("@Username", username));

            if (referenceId != null)
            {
                parameters.Add(new SqlParameter("@ReferenceId", referenceId.Value));
            }

            object dateSent = ReadDataField("GetAlertSentOn", parameters);
            bool remind = false;
            if (dateSent == DBNull.Value || dateSent == null)
            {
                remind = true;
            }
            else
            {
                DateTime dated = Convert.ToDateTime(dateSent);
                if (DateTime.Now.Date > dated.Date)
                {
                    if (reminder.Equals("Message"))
                    {
                        int diff = DateTime.Now.Date.Subtract(dated.Date).Days;
                        if (diff >= 2)
                        {
                            remind = true;
                        }
                    }
                    else if (reminder.Equals("Application") || reminder.Equals("Automatch"))
                    {
                        int diff = DateTime.Now.Date.Subtract(dated.Date).Days;
                        if (diff >= 3)
                        {
                            remind = true;
                        }
                    }
                    else
                    {
                        remind = true;
                    }
                }
                else
                {
                    if (reminder.Equals("InterviewReminder"))
                    {
                        if (refDate != null)
                        {
                            if (DateTime.Now.Date == dated.Date && refDate.Value.Date == DateTime.Now.Date)
                            {
                                if (DateTime.Now.TimeOfDay < refDate.Value.Subtract(new TimeSpan(1, 0, 0)).TimeOfDay)
                                {
                                    remind = false;
                                }
                                else if (DateTime.Now.TimeOfDay == refDate.Value.Subtract(new TimeSpan(1, 0, 0)).TimeOfDay)
                                {
                                    remind = true;
                                }
                            }
                        }
                    }
                }
            }
            return remind;
        }
    }
}
