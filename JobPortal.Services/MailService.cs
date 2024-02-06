using JobPortal.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;
//using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace JobPortal.Services
{
    public class MailService : Contracts.IMailService
    {
        public bool Send(string subject, string from, string to, string body)
        {
            bool flag = false;
            try
            {
                

                using (var smtp = new SmtpClient())
                {
                    string postmail = ConfigurationManager.AppSettings["postmail"];
                    string postpassword = ConfigurationManager.AppSettings["postpassword"];
                    MimeMessage mail = new MimeMessage();
                    mail.From.Add(new MailboxAddress("Joblisting", from));
                    
                        //mail.To.Add(new MailboxAddress("Excited User", to));
                   
                    //var s =new  MimeKit.MimeMessage();
                    //smtp.Configuration.DeliveryNotificationOption = DeliveryNotificationOptions.OnFailure;
                    
                    var addr = new MailboxAddress("Excited User", to);
                    try
                    {
                        if (addr.Address.Equals(to))
                        {
                            mail.To.Add(addr);
                        }
                    }
                    catch (Exception ex)
                    {
                        flag = false;
                        throw ex;
                    }
                    mail.Subject = subject;
                    mail.Body = new TextPart("html")
                    {
                        Text = body
                    };
                    
                    if (mail.To.Count > 0)
                    {
                        
                        smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        smtp.Connect("smtp.mailgun.org", 587, false);
                        smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                        smtp.Authenticate(postmail, postpassword);

                        smtp.Send(mail);
                        smtp.Disconnect(true);
                        
                    }
                }
            }

            catch (Exception ex)
            {
                flag = false;
                throw ex;
            }
            return flag;
        }
    }
}
