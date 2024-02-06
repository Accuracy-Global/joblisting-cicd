using System.Collections.Generic;
using System.Linq;
using Pop3;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace JobPortal.Library.Helpers
{
    public static class ValidatorHelper
    {
        private static readonly HashSet<char> _base64Characters = new HashSet<char>() { 
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 
        'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 
        'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 
        'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/', 
        '='
        };

        public static bool IsBase64String(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            else if (value.Any(c => !_base64Characters.Contains(c)))
            {
                return false;
            }

            try
            {
                Convert.FromBase64String(value);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsValidEmail(string email)
        {
            var flag = true;
            try
            {
                using (var pop3Client = new Pop3Client())
                {
                    pop3Client.Connect(ConfigurationManager.AppSettings["POPHost"], ConfigurationManager.AppSettings["POPUser"], ConfigurationManager.AppSettings["POPPassword"], Convert.ToInt32(ConfigurationManager.AppSettings["POPPort"]), Convert.ToBoolean(ConfigurationManager.AppSettings["POPSSLEnabled"]));

                    IEnumerable<Pop3Message> messages = pop3Client.List();



                    foreach (Pop3Message message in messages)
                    //Parallel.ForEach(messages, (message) =>
                    {
                        pop3Client.Retrieve(message);
                        if (message.Subject.Contains("Undeliverable") || message.Subject.Contains("Delivery Failure"))
                        {
                            if (message.Body.Contains(string.Format("Recipient Address:      {0}", email)) || message.Body.Contains(string.Format("Failed Recipient: {0}", email)) || message.Body.Contains(string.Format("To: {0}", email)))
                            {
                                flag = false;
                            }
                        }
                        else if (message.Subject.Contains("failure notice"))
                        {
                            if (message.Body.Contains(string.Format("To: {0}", email)))
                            {
                                flag = false;
                            }
                        }
                    }
                    pop3Client.Disconnect();
                }
            }
            catch (Exception)
            {
                flag = true;
            }
            return flag;
        }
    }
}