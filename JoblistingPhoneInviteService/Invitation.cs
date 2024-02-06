using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;
//using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Twilio;

namespace JoblistingInviteSMSService
{
    public class Invitation : DataService
    {
        /// <summary>
        /// Send Invitation via Phone
        /// </summary>
        public void Send()
        {
            List<ContactEntity> contacts = ReadData<ContactEntity>("GetInviteList");

            using (SmtpClient oSmtp = new SmtpClient())
            {
                foreach (var item in contacts)
                {
                    string AccountSid = item.TwilioSID;
                    string AuthToken = item.TwilioToken;
                    string from = item.TwilioNumber;
                    var twilio = new TwilioRestClient(AccountSid, AuthToken);

                    StringBuilder sbSMS = new StringBuilder();
                    sbSMS.AppendFormat("{0} invites you to connect at Joblisting\n", item.FullName);

                    if (item.Token != null)
                    {
                        var url = string.Format("{0}/connectbyphone?token={1}", item.BaseUrl, item.Token.Value);
                        sbSMS.AppendFormat("CLICK HERE {0}", url);
                    }

                    string to = string.Format("{0}", item.Mobile);

                    var sms = twilio.SendMessage(from, to, sbSMS.ToString());
                    if (sms != null && sms.RestException == null)
                    {
                        var msg = twilio.GetMessage(sms.Sid);
                        if (msg.Status.Equals("delivered") || msg.Status.Equals("sent"))
                        {
                            List<DbParameter> parameters = new List<DbParameter>();
                            parameters.Add(new SqlParameter("@Id", item.ContactId));

                            int stat = HandleData("UpdatePhoneInviter", parameters);
                        }
                    }
                }
            }
        }
    }
}
