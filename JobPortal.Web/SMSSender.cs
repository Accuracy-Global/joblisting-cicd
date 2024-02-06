#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Twilio;

namespace JobPortal.Web
{
    public class SMSSender
    {
        public static void Send()
        {
            IUserService iUserService = new UserService();
            List<PhoneContactReminder> contacts = iUserService.GetPhoneContacts();
            foreach (PhoneContactReminder item in contacts)
            {                
                string AccountSid = ConfigService.Instance.GetConfigValue("TwilioSID");
                string AuthToken = ConfigService.Instance.GetConfigValue("TwilioToken");
                string from = ConfigService.Instance.GetConfigValue("TwilioNumber");
                string baseUrl = ConfigService.Instance.GetConfigValue("BaseUrl");
                string mobile_app_url = ConfigService.Instance.GetConfigValue("mobile_app_url");
                var twilio = new TwilioRestClient(AccountSid, AuthToken);

                StringBuilder sbSMS = new StringBuilder();

                sbSMS.AppendFormat("This is {0}.\n", item.Sender);
                sbSMS.Append("I am using joblisting.com messenger!\n");
                var url = string.Format("{0}", mobile_app_url);
                sbSMS.AppendFormat("Download here {0} and connect!", url);

                var sms = twilio.SendMessage(from, item.Phone, sbSMS.ToString());
                if (sms.RestException == null)
                {
                    var msg = twilio.GetMessage(sms.Sid);
                    if (msg.Status.Equals("delivered") || msg.Status.Equals("sent"))
                    {
                        int status = iUserService.RecordSMSStatus(item.Id, msg.Status);
                    }
                    else
                    {
                        iUserService.RecordSMSStatus(item.Id, msg.Status);
                    }
                }
                else
                {
                    if (sms.RestException.Code.Equals("14101") || sms.RestException.Code.Equals("21211"))
                    {
                        iUserService.RecordSMSStatus(item.Id, sms.RestException.Message);
                    }
                    else
                    {
                        iUserService.RecordSMSStatus(item.Id, "Failed");
                    }
                }
            }
        }         
    }
}