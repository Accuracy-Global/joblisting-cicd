using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;
//using System.Net.Mail;
namespace JoblistingReminderService
{
    public partial class JoblistingService : ServiceBase
    {
        private readonly TimeSpan _sleepTimeSpan = new TimeSpan(0, 5, 0);
        public JoblistingService()
        {
            InitializeComponent();
        }

        #region Protected Area
        protected override void OnStart(string[] args)
        {
            Task inviteReminderTask = new Task(new Action(InviteFriends), TaskCreationOptions.LongRunning);
            inviteReminderTask.Start();

            Task jobExpiryReminderTask = new Task(new Action(JobExpiresSoon), TaskCreationOptions.LongRunning);
            jobExpiryReminderTask.Start();

            Task jobExpiredTask = new Task(new Action(JobsExpired), TaskCreationOptions.LongRunning);
            jobExpiredTask.Start();

            Task applicationReminderTask = new Task(new Action(ApplicationReminder), TaskCreationOptions.LongRunning);
            applicationReminderTask.Start();

            Task automatchedReminderTask = new Task(new Action(AutomatchedReminder), TaskCreationOptions.LongRunning);
            automatchedReminderTask.Start();

            Task interviewFollowupTask = new Task(new Action(InterviewFollowUps), TaskCreationOptions.LongRunning);
            interviewFollowupTask.Start();

            Task interviewReminderTask = new Task(new Action(InterviewReminder), TaskCreationOptions.LongRunning);
            interviewReminderTask.Start();

            Task interviewReminderOneHourBefore = new Task(new Action(HourBeforeInterviewReminder), TaskCreationOptions.LongRunning);
            interviewReminderOneHourBefore.Start();

            Task messageReminderTask = new Task(new Action(MessageReminder), TaskCreationOptions.LongRunning);
            messageReminderTask.Start();

            Task individualList = new Task(new Action(IndividualsList), TaskCreationOptions.LongRunning);
            individualList.Start();

            Task companyList = new Task(new Action(CompanyList), TaskCreationOptions.LongRunning);
            companyList.Start();
        }

        protected override void OnStop()
        {
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            string[] toList = ConfigurationManager.AppSettings["ServiceNotifyEmail"].Split(',');
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            using (SmtpClient oSmtp = new SmtpClient())
            {
                MimeMessage mail = new MimeMessage();
                mail.From.Add(new MailboxAddress("Joblisting", from));
                foreach (string to in toList)
                {
                    mail.To.Add(new MailboxAddress("Excited User", to));
                }
                mail.Subject = "Joblisting Reminder Service Stopped Working";
                mail.Body = new TextPart("html")
                {
                    Text = "<h3>Joblisting Reminder Service has been stopped, please start once again!</h3>"
                };
                oSmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                oSmtp.Connect("smtp.mailgun.org", 587, false);
                oSmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                oSmtp.Authenticate(postmail, postpassword);

                oSmtp.Send(mail);
                oSmtp.Disconnect(true);

            }
        }

        #endregion


        #region Public Area

        /// <summary>
        /// Send Friend Invite reminder to the users 
        /// </summary>
        public void InviteFriends()
        {
            while (true)
            {
                try
                {
                    ReminderService service = new ReminderService();
                    service.InviteFriends();
                    service = null;
                }
                catch (Exception ex)
                {
                    SendEx(ex);
                }
                Thread.Sleep(new TimeSpan(0, 0, 5));
            }
        }

        /// <summary>
        /// Send Job Expires Soon Reminder
        /// </summary>
        public void JobExpiresSoon()
        {
            while (true)
            {
                try
                {
                    ReminderService service = new ReminderService();
                    service.JobsExpires();
                    service = null;
                }
                catch (Exception ex)
                {
                    SendEx(ex);
                }
                Thread.Sleep(new TimeSpan(0, 5, 0));
            }
        }

        /// <summary>
        /// Send Job Expired Message
        /// </summary>
        public void JobsExpired()
        {
            while (true)
            {
                try
                {
                    ReminderService service = new ReminderService();
                    service.JobExpired();
                    service = null;
                }
                catch (Exception ex)
                {
                    SendEx(ex);
                }
                Thread.Sleep(new TimeSpan(0, 5, 0));
            }
        }

        /// <summary>
        /// Application waiting for action reminder
        /// </summary>
        public void ApplicationReminder()
        {
            while (true)
            {
                try
                {
                    ReminderService service = new ReminderService();
                    service.ApplicationReminder();
                    service = null;
                }
                catch (Exception ex)
                {
                    SendEx(ex);
                }
                Thread.Sleep(new TimeSpan(0, 5, 0));
            }
        }

        /// <summary>
        /// Automatched Reminder
        /// </summary>
        public void AutomatchedReminder()
        {
            while (true)
            {
                try
                {
                    ReminderService service = new ReminderService();
                    service.AutomatchReminder();
                    service = null;
                }
                catch (Exception ex)
                {
                    SendEx(ex);
                }
                Thread.Sleep(new TimeSpan(0, 5, 0));
            }
        }

        /// <summary>
        /// Send Interview Reminder
        /// </summary>
        public void InterviewReminder()
        {
            while (true)
            {
                try
                {
                    ReminderService service = new ReminderService();
                    service.InterviewReminder();
                    service = null;
                }
                catch (Exception ex)
                {
                    SendEx(ex);
                }
                Thread.Sleep(new TimeSpan(0, 5, 00));
            }
        }

        /// <summary>
        /// Send Interview Reminder
        /// </summary>
        public void HourBeforeInterviewReminder()
        {
            while (true)
            {
                try
                {
                    ReminderService service = new ReminderService();
                    service.HourBeforeInterviewReminder();
                    service = null;
                }
                catch (Exception ex)
                {
                    SendEx(ex);
                }
                Thread.Sleep(new TimeSpan(0, 1, 0));
            }
        }

        /// <summary>
        /// Interview FllowUps Reminder
        /// </summary>
        public void InterviewFollowUps()
        {
            while (true)
            {
                try
                {
                    ReminderService service = new ReminderService();
                    service.InterviewFollowUps();
                    service = null;
                }
                catch (Exception ex)
                {
                    SendEx(ex);
                }
                Thread.Sleep(new TimeSpan(0, 1, 0));
            }
        }

        /// <summary>
        /// Send message waiting for action reminder.
        /// </summary>
        public void MessageReminder()
        {
            while (true)
            {
                try
                {
                    ReminderService service = new ReminderService();
                    service.MessageReminder();
                    service = null;
                }
                catch (Exception ex)
                {
                    SendEx(ex);
                }
                Thread.Sleep(new TimeSpan(0, 1, 0));
            }
        }

        /// <summary>
        /// Send Individuals list to employer
        /// </summary>
        public void IndividualsList()
        {
            while (true)
            {
                try
                {
                    ReminderService service = new ReminderService();
                    service.IndividualsList();
                    service = null;
                }
                catch (Exception ex)
                {
                    SendEx(ex);
                }
                Thread.Sleep(new TimeSpan(0, 5, 0));
            }
        }

        /// <summary>
        /// Send Company list to individuals
        /// </summary>
        public void CompanyList()
        {
            while (true)
            {
                try
                {
                    ReminderService service = new ReminderService();
                    service.CompanyList();
                    service = null;
                }
                catch (Exception ex)
                {
                    SendEx(ex);
                }
                Thread.Sleep(new TimeSpan(0, 5, 0));
            }
        }
        #endregion
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
      
    }
}