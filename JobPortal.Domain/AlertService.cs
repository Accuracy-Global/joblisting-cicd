using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
//using System.Net.Mail;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;
#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Collections;
using System;

namespace JobPortal.Domain
{
    public class AlertService
    {
        private static volatile AlertService instance;
        private static readonly object sync = new object();

        /// <summary>
        ///     Default private constructor.
        /// </summary>
        private AlertService()
        {
        }

        /// <summary>
        ///     Single Instance of AlertService
        /// </summary>
        public static AlertService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new AlertService();
                        }
                    }
                }
                return instance;
            }
        }


        /// <summary>
        ///     Sending mail notification.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="receipents"></param>
        /// <param name="message"></param>
        public void SendMail(string subject, string[] receipents, string message)
        {
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];


            var cnt = 0;

            //MimeMessage mail = new MimeMessage();
            //var strfrom = new MailboxAddress("Joblisting", ConfigurationManager.AppSettings["FromEmailAddress"]);
            //mail.From.Add(strfrom);


            //if (receipents.Length > 0)
            //{
            //    foreach (var email in receipents)
            //    {
            //        if (email != null)
            //        {
            //            if (cnt == 0)
            //            {
            //                if (email.ToLower().Contains("admin_"))
            //                {
            //                    mail.To.Add(new MailboxAddress("", "admin@joblisting.com"));
            //                }
            //                else
            //                {
            //                    try
            //                    {
            //                        var addr = new MailboxAddress("", email);
            //                        if (addr.Address == email)
            //                        {
            //                            mail.To.Add(new MailboxAddress("", email));
            //                        }
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        using (var oSmtpClient = new SmtpClient())
            //                        {
            //                            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            //                            string[] toList = ConfigurationManager.AppSettings["ServiceNotifyEmail"].Split(',');

            //                            MimeMessage exmsg = new MimeMessage();

            //                            exmsg.From.Add(new MailboxAddress(from, "Joblisting"));
            //                            foreach (string to in toList)
            //                            {
            //                                exmsg.To.Add(new MailboxAddress("", to));
            //                            }
            //                            exmsg.Subject = "Invalid Email Address";
            //                            exmsg.Body = new TextPart("html")
            //                            {

            //                                Text = string.Format("<h3>{0}</h3>", ex.Message) + string.Format("{0}", ex.StackTrace)
            //                            };
            //                            if (mail.To.Count > 0)
            //                            {
            //                                oSmtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

            //                                oSmtpClient.Connect("smtp.mailgun.org", 587, false);
            //                                oSmtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
            //                                oSmtpClient.Authenticate(postmail, postpassword);

            //                                oSmtpClient.Send(mail);
            //                                oSmtpClient.Disconnect(true);

            //                            }
            //                        }
            //                    }
            //                }

            //            }
            //            else
            //            {
            //                if (email.ToLower().Contains("admin_"))
            //                {
            //                    mail.Bcc.Add(new MailboxAddress("", "admin@joblisting.com"));
            //                }
            //                else
            //                {
            //                    try
            //                    {
            //                        var addr = new MailboxAddress("", email);
            //                        if (addr.Address == email)
            //                        {
            //                            mail.Bcc.Add(new MailboxAddress("", email));
            //                        }
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        using (var oSmtpClient = new SmtpClient())
            //                        {
            //                            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            //                            string[] toList = ConfigurationManager.AppSettings["ServiceNotifyEmail"].Split(',');

            //                            MimeMessage mail1 = new MimeMessage();
            //                            mail1.From.Add(new MailboxAddress("Joblisting", from));


            //                            foreach (string to in toList)
            //                            {
            //                                mail1.To.Add(new MailboxAddress("", to));
            //                            }
            //                            mail1.Subject = "Invalid Email Address";

            //                            mail1.Body = new TextPart("html")
            //                            { Text = string.Format("<h3>{0}</h3>", ex.Message) + string.Format("{0}", ex.StackTrace) };

            //                            if (mail1.To.Count > 0)
            //                            {
            //                                oSmtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

            //                                oSmtpClient.Connect("smtp.mailgun.org", 587, false);
            //                                oSmtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
            //                                oSmtpClient.Authenticate(postmail, postpassword);

            //                                oSmtpClient.Send(mail1);
            //                                oSmtpClient.Disconnect(true);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }

            //    if (mail.To.Count > 0)
            //    {
            //        try
            //        {
            //            using (var oSmtpClient = new SmtpClient())
            //            {
            //                //mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            //                mail.From.Add(strfrom);
            //                //mail.From.Add(new MailboxAddress("",postmail));
            //                mail.Subject = subject;
            //                mail.Body = new TextPart("html")
            //                {
            //                    Text = message
            //                };
            //                if (mail.To.Count > 0)
            //                {
            //                    oSmtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

            //                    oSmtpClient.Connect("smtp.mailgun.org", 587, false);
            //                    oSmtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
            //                    oSmtpClient.Authenticate(postmail, postpassword);

            //                   // oSmtpClient.Send(mail);
            //                    oSmtpClient.Disconnect(true);
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Exception iEx = ex.InnerException;
            //            if (!ex.Message.ToLower().Contains("Failure sending mail."))
            //            {
            //                throw ex;
            //            }
            //        }

            //    }
            //}
        }

        public void SendVerifyEmail(string emailAddress, string message)
        {
            MimeMessage msg = new MimeMessage();

            var strfrom = new MailboxAddress("Joblisting", ConfigurationManager.AppSettings["FromEmailAddress"]);
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            msg.To.Add(new MailboxAddress("", emailAddress));

            using (var oSmtpClient = new SmtpClient())
            {
                if (msg.To.Count > 0)
                {
                    msg.From.Add(strfrom);
                    msg.Subject = "Verifying your Email Address";
                    msg.Body = new TextPart("html")
                    {
                        Text = message
                    };
                    oSmtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    oSmtpClient.Connect("smtp.mailgun.org", 587, false);
                    oSmtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                    oSmtpClient.Authenticate(postmail, postpassword);

                    oSmtpClient.Send(msg);
                    oSmtpClient.Disconnect(true);

                }
            }
        }

        public void Send(List<MimeMessage> list)
        {
            MimeMessage msg = new MimeMessage();
            var strfrom = new MailboxAddress("Joblisting", ConfigurationManager.AppSettings["FromEmailAddress"]);
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            try
            {
                using (var oSmtpClient = new SmtpClient())
                {
                    oSmtpClient.Timeout = 1000000;
                    if (list.Count > 0)
                    {
                        foreach (MimeMessage item in list)
                        {
                            oSmtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                            oSmtpClient.Connect("smtp.mailgun.org", 587, false);
                            oSmtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                            oSmtpClient.Authenticate(postmail, postpassword);

                            oSmtpClient.Send(item);
                            oSmtpClient.Disconnect(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (var oSmtpClient = new SmtpClient())
                {
                    string from = ConfigurationManager.AppSettings["FromEmailAddress"];
                    string[] toList = ConfigurationManager.AppSettings["ServiceNotifyEmail"].Split(',');
                    MimeMessage exmsg = new MimeMessage();


                    exmsg.From.Add(new MailboxAddress("Joblisting", from));
                    foreach (string to in toList)
                    {
                        exmsg.To.Add(new MailboxAddress("", to));
                    }
                    exmsg.Subject = "Invalid Email Address";
                    exmsg.Body = new TextPart("html") { Text = string.Format("<h3>{0}</h3>", ex.Message) + string.Format("{0}", ex.StackTrace) };
                    oSmtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    oSmtpClient.Connect("smtp.mailgun.org", 587, false);
                    oSmtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                    oSmtpClient.Authenticate(postmail, postpassword);

                    oSmtpClient.Send(exmsg);
                    oSmtpClient.Disconnect(true);
                }
            }
        }

        public void Send(string subject, string email, string message)
        {
            MimeMessage msg = new MimeMessage();
            var strfrom = new MailboxAddress("Joblisting", ConfigurationManager.AppSettings["FromEmailAddress"]);
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            //msg.From.Add(strfrom);
            try
            {
                var addr = new MailboxAddress("", email);
                if (addr.Address == email)
                {
                    if (email.ToLower().Contains("admin_"))
                    {
                        msg.To.Add(new MailboxAddress("", "admin@joblisting.com"));
                    }
                    else
                    {
                        msg.To.Add(new MailboxAddress("", email));
                    }
                }

                using (var oSmtpClient = new SmtpClient())
                {
                    if (msg.To.Count > 0)
                    {
                        msg.From.Add(strfrom);
                        msg.Subject = subject;
                        msg.Body = new TextPart("html")
                        {
                            Text = message
                        };
                        //msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                        //msg.Headers.Add("Disposition-Notification-To", "mouni@accuracy.com.sg");
                        oSmtpClient.Timeout = 1000000;
                        oSmtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        oSmtpClient.Connect("smtp.mailgun.org", 587, false);
                        oSmtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                        oSmtpClient.Authenticate(postmail, postpassword);

                        oSmtpClient.Send(msg);
                        oSmtpClient.Disconnect(true);
                    }
                }
            }
            catch (Exception ex)
            {
                using (var oSmtpClient = new SmtpClient())
                {
                    string from = ConfigurationManager.AppSettings["FromEmailAddress"];
                    string[] toList = ConfigurationManager.AppSettings["ServiceNotifyEmail"].Split(',');
                    MimeMessage exmsg = new MimeMessage();

                    exmsg.From.Add(new MailboxAddress("Joblisting", from));
                    foreach (string to in toList)
                    {
                        exmsg.To.Add(new MailboxAddress("", to));
                    }
                    exmsg.Subject = "Invalid Email Address";
                    exmsg.Body = new TextPart("html") { Text = string.Format("<h3>{0}</h3>", ex.Message) + string.Format("{0}", ex.StackTrace) };
                    oSmtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    oSmtpClient.Connect("smtp.mailgun.org", 587, false);
                    oSmtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                    oSmtpClient.Authenticate(postmail, postpassword);

                    oSmtpClient.Send(exmsg);
                    oSmtpClient.Disconnect(true);
                }
            }
        }


#pragma warning disable CS0246 // The type or namespace name 'Recipient' could not be found (are you missing a using directive or an assembly reference?)
        public void SendMail(string subject, List<Recipient> recipents, string message)
#pragma warning restore CS0246 // The type or namespace name 'Recipient' could not be found (are you missing a using directive or an assembly reference?)
        {
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            var msg = new MimeMessage();
            var strfrom = new MailboxAddress("Joblisting", ConfigurationManager.AppSettings["FromEmailAddress"]);
            if (recipents.Count > 0)
            {
                foreach (var recipient in recipents)
                {
                    try
                    {
                        var addr = new MailboxAddress("", recipient.Email);
                        if (addr.Address == recipient.Email)
                        {
                            switch (recipient.Type)
                            {
                                case RecipientTypes.TO:
                                    if (recipient.Email.ToLower().Contains("admin_"))
                                    {
                                        msg.To.Add(new MailboxAddress("Joblisting", "admin@joblisting.com"));
                                    }
                                    else
                                    {
                                        msg.To.Add(new MailboxAddress(recipient.DisplayName, recipient.Email));
                                    }
                                    break;
                                case RecipientTypes.CC:
                                    if (recipient.Email.ToLower().Contains("admin_"))
                                    {
                                        msg.Cc.Add(new MailboxAddress("Joblisting", "admin@joblisting.com"));
                                    }
                                    else
                                    {
                                        msg.Cc.Add(new MailboxAddress(recipient.DisplayName, recipient.Email));
                                    }
                                    break;
                                case RecipientTypes.BCC:
                                    if (recipient.Email.ToLower().Contains("admin_"))
                                    {
                                        msg.Bcc.Add(new MailboxAddress("Joblisting", "admin@joblisting.com"));
                                    }
                                    else
                                    {
                                        msg.Bcc.Add(new MailboxAddress(recipient.DisplayName, recipient.Email));
                                    }
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        using (var oSmtpClient = new SmtpClient())
                        {
                            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
                            string[] toList = ConfigurationManager.AppSettings["ServiceNotifyEmail"].Split(',');
                            MimeMessage exmsg = new MimeMessage();
                            exmsg.From.Add(new MailboxAddress("Joblisting", from));
                            foreach (string to in toList)
                            {
                                exmsg.To.Add(new MailboxAddress("", to));
                            }
                            exmsg.Subject = "Invalid Email Address";
                            exmsg.Body = new TextPart("html") { Text = string.Format("<h3>{0}</h3>", ex.Message) + string.Format("{0}", ex.StackTrace) };
                            oSmtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                            oSmtpClient.Connect("smtp.mailgun.org", 587, false);
                            oSmtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                            oSmtpClient.Authenticate(postmail, postpassword);

                            oSmtpClient.Send(exmsg);
                            oSmtpClient.Disconnect(true);
                        }
                    }
                }
                using (var oSmtpClient = new SmtpClient())
                {
                    if (msg.To.Count > 0)
                    {
                        msg.From.Add(strfrom);
                        msg.Subject = subject;
                        msg.Body = new TextPart("html") { Text = message };
                        oSmtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        oSmtpClient.Connect("smtp.mailgun.org", 587, false);
                        oSmtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                        oSmtpClient.Authenticate(postmail, postpassword);

                        oSmtpClient.Send(msg);
                        oSmtpClient.Disconnect(true);
                    }
                }
            }
        }

#pragma warning disable CS0246 // The type or namespace name 'Recipient' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Recipient' could not be found (are you missing a using directive or an assembly reference?)
        public void SendMail(string subject, string message, Recipient from, Recipient to)
#pragma warning restore CS0246 // The type or namespace name 'Recipient' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Recipient' could not be found (are you missing a using directive or an assembly reference?)
        {

            if (from != null && to != null)
            {
                var msg = new MimeMessage();
                try
                {
                    string postmail = ConfigurationManager.AppSettings["postmail"];
                    string postpassword = ConfigurationManager.AppSettings["postpassword"];
                    var addr = new MailboxAddress("", to.Email);
                    if (addr.Address == to.Email)
                    {
                        var msgFrom = new MailboxAddress(from.DisplayName, from.Email);
                        if (to.Email.ToLower().Contains("admin_"))
                        {
                            msg.To.Add(new MailboxAddress("Joblisting", "renukag@accuracy.com.sg"));
                            msg.To.Add(new MailboxAddress("Joblisting", "Nagalakshmi@accuracy.com.sg"));
                        }
                        else
                        {
                            msg.To.Add(new MailboxAddress(to.DisplayName, to.Email));
                        }
                        msg.From.Add(msgFrom);
                        msg.Subject = subject;
                        msg.Body = new TextPart("html") { Text = message };
                        using (var oSmtpClient = new SmtpClient())
                        {
                            oSmtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                            oSmtpClient.Connect("smtp.mailgun.org", 587, false);
                            oSmtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                            oSmtpClient.Authenticate(postmail, postpassword);

                            oSmtpClient.Send(msg);
                            oSmtpClient.Disconnect(true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string frmaddr = ConfigurationManager.AppSettings["FromEmailAddress"];
                    string[] toList = ConfigurationManager.AppSettings["ServiceNotifyEmail"].Split(',');
                    string postmail = ConfigurationManager.AppSettings["postmail"];
                    string postpassword = ConfigurationManager.AppSettings["postpassword"];
                    MimeMessage exmsg = new MimeMessage();
                    exmsg.From.Add(new MailboxAddress("Joblisting", frmaddr));
                    foreach (string toaddr in toList)
                    {
                        exmsg.To.Add(new MailboxAddress("", toaddr));
                    }
                    exmsg.Subject = "Invalid Email Address";
                    exmsg.Body = new TextPart("html") { Text = string.Format("<h3>{0}</h3>", ex.Message) + string.Format("{0}", ex.StackTrace) };

                    using (var oSmtpClient = new SmtpClient())
                    {
                        oSmtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        oSmtpClient.Connect("smtp.mailgun.org", 587, false);
                        oSmtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                        oSmtpClient.Authenticate(postmail, postpassword);

                        oSmtpClient.Send(exmsg);
                        oSmtpClient.Disconnect(true);
                    }
                }
            }
        }
    }
}