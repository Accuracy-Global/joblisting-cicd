using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JobPortal.Library.Helpers
{
    public class ImapProvider : IDisposable
    {
        TcpClient client;
        StreamReader reader;
        StreamWriter writer;
        string folder = string.Empty;
        int prefix = 1;

        string host = ConfigurationManager.AppSettings["ImapHost"];
        int port = Convert.ToInt32(ConfigurationManager.AppSettings["ImapPort"]);
        bool ssl = Convert.ToBoolean(ConfigurationManager.AppSettings["ImapSSLEnabled"]);

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ImapProvider()
        {
            try
            {
                client = new TcpClient(host, port);

                if (ssl)
                {
                    var stream = new SslStream(client.GetStream());
                    stream.AuthenticateAsClient(host);

                    reader = new StreamReader(stream);
                    writer = new StreamWriter(stream);
                }
                else
                {
                    var stream = client.GetStream();
                    reader = new StreamReader(stream);
                    writer = new StreamWriter(stream);
                }

                //string greeting = _reader.ReadLine();
            }
            catch (Exception)
            {
            }
        }
        
        /// <summary>
        /// Authenticate User Credentials.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        public void Authenicate()
        {
            string user = ConfigurationManager.AppSettings["ImapUser"];
            string pass = ConfigurationManager.AppSettings["ImapPassword"];
            ExecuteCommand(string.Format("LOGIN {0} {1}", user, pass));
            string response = Response();
        }
        public void Logout()
        {
            ExecuteCommand(". LOGOUT");
            string response = Response();
        }
        /// <summary>
        /// Selects the specified folder.
        /// </summary>
        /// <param name="folderName"></param>
        public void SelectFolder(string folderName)
        {
            folder = folderName;
            ExecuteCommand("SELECT " + folderName);
            string response = Response();
        }

        private void ExecuteCommand(string command)
        {
            writer.WriteLine("A" + prefix.ToString() + " " + command);
            writer.Flush();
            prefix++;
        }

        protected string Response()
        {
            string response = string.Empty;

            while (true)
            {
                string line = reader.ReadLine();
                string[] tags = line.Split(new char[] { ' ' });
                response += line + Environment.NewLine;
                
                if (!tags[0].Equals(")"))
                {
                    if (tags[0].Substring(0, 1) == "A" && tags[1].Trim() == "OK" || tags[1].Trim() == "BAD" || tags[1].Trim() == "NO")
                    {
                        break;
                    }
                }

            }
            return response;
        }    

        private string GetMessage()
        {
            string line = reader.ReadLine();
            MatchCollection m = Regex.Matches(line, "\\{(.*?)\\}");

            if (m.Count > 0)
            {
                int length = Convert.ToInt32(m[0].ToString().Trim(new char[] { '{', '}' }));

                char[] buffer = new char[length];
                int read = (length < 128) ? length : 128;
                int remaining = length;
                int offset = 0;
                while (true)
                {
                    read = reader.Read(buffer, offset, read);
                    remaining -= read;
                    offset += read;
                    read = (remaining >= 128) ? 128 : remaining;

                    if (remaining == 0)
                    {
                        break;
                    }
                }
                return new String(buffer);
            }
            return "";
        }


        public int GetMessageCount()
        {
            ExecuteCommand("STATUS " + folder + " (MESSAGES)");
            string response = Response();
            Match m = Regex.Match(response, "[0-9]*[0-9]");
            return Convert.ToInt32(m.ToString());
        }

        public int GetUnseenMessageCount()
        {
            ExecuteCommand("STATUS " + folder + " (unseen)");
            string response = Response();
            Match m = Regex.Match(response, "[0-9]*[0-9]");
            return Convert.ToInt32(m.ToString());
        }

        public string GetMessage(string uid, string section)
        {
            ExecuteCommand("FETCH " + uid + " BODY[" + section + "]");
            return GetMessage();
        }

        public string GetMessage(int index, string section)
        {
            ExecuteCommand("FETCH " + index + " BODY[" + section + "]");
            return GetMessage();
        }

        /// <summary>
        /// Gets the message from the selected folder.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetMessage(int index)
        {
            ExecuteCommand("FETCH " + index + " BODY[TEXT]");
            return GetMessage();
        }

        /// <summary>
        /// Gets the list of Messages
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public List<string> GetMessageList()
        {
            List<string> msgs = new List<string>();

            int counts = GetMessageCount();
            try
            {
                //Parallel.For(1, counts,
                //   idx =>
                //   {
                //       string message = GetMessage(idx);
                //       if (!string.IsNullOrEmpty(message))
                //       {
                //           msgs.Add(message);
                //       }
                //   });

                for (int idx = 1; idx <= counts; idx++)
                {
                    string message = GetMessage(idx);
                    if (!string.IsNullOrEmpty(message))
                    {
                        msgs.Add(message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return msgs;
            
        }

        /// <summary>
        /// Verifies the the specified email address is valid or not.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsValidEmailAddress(string email)
        {
            bool flag = true;

            int counts = GetMessageCount();
            try
            {
                for (int idx = 1; idx <= counts; idx++)
                {
                    string message = GetMessage(idx);
                    if (message.Contains(string.Format("Failed Recipient: {0}", email)))
                    {
                        flag = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        public void Dispose()
        {
            if (reader != null)
            {
                reader.Dispose();
            }

            if (writer != null)
            {
                writer.Dispose();
            }
        }
    }
}