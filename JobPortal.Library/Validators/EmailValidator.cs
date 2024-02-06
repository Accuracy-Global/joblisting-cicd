namespace JobPortal.Library.Validators
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    public class EmailValidator
    {
        //private string _MailFrom = "admin@monoprog.com";
        private string _MailFrom = "admin@joblisting.com";
        private int _Smtpport = 0x19;

        public bool Check_DomainName(string Email_or_Domain)
        {
            string domainName = this.GetDomainName(Email_or_Domain);
            try
            {
                Dns.GetHostEntry(domainName);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Check_MailBox(string Email_Adress)
        {
            if (!Email_Adress.Contains("@") || this.Check_Syntax(Email_Adress))
            {
                foreach (string str in this.FindMXRecords(Email_Adress))
                {
                    try
                    {
                        IPEndPoint remoteEP = new IPEndPoint(Dns.GetHostEntry(str).AddressList[0], this._Smtpport);
                        Socket s = new Socket(remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        s.Connect(remoteEP);
                        if (this.Get_Response_Code(s) == 220)
                        {
                            this.Senddata(s, string.Format("HELO {0}",Dns.GetHostName()));
                            //this.Senddata(s, string.Format("EHLO {0}", Dns.GetHostName()));
                            if (this.Get_Response_Code(s) == 250)
                            {
                                this.Senddata(s, string.Format("MAIL FROM:<{0}>",this._MailFrom));
                                if (this.Get_Response_Code(s) == 250)
                                {
                                    this.Senddata(s, string.Format("RCPT TO:<{0}>",Email_Adress));
                                    if (this.Get_Response_Code(s) == 250)
                                    {
                                        this.Senddata(s, "QUIT\r\n");
                                        s.Close();
                                        return true;
                                    }
                                    return false;
                                }
                            }
                            return false;
                        }
                        s.Close();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return false;
        }

        public string Check_MailBox_Error(string Email_Adress)
        {
            if (Email_Adress.Contains("@") && !this.Check_Syntax(Email_Adress))
            {
                return "001 : Invalid email syntax";
            }
            string str = "003 : No MX records found";
            foreach (string str2 in this.FindMXRecords(Email_Adress))
            {
                try
                {
                    SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                    IPEndPoint remoteEP = new IPEndPoint(Dns.GetHostEntry(str2).AddressList[0], this._Smtpport);
                    Socket s = new Socket(remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    e.RemoteEndPoint = remoteEP;
                    e.UserToken = s;
                    e.Completed += new EventHandler<SocketAsyncEventArgs>(e_Completed);
                    s.ConnectAsync(e);
                    //s.Connect(remoteEP);
                    str = this.Get_Response(s);
                    if (Convert.ToInt32(str.Substring(0, 3)) == 220)
                    {
                       this.Senddata(s, string.Format("HELO {0}","joblisting.com"));
                        str = this.Get_Response(s);
                        if (Convert.ToInt32(str.Substring(0, 3)) == 250)
                        {
                            this.Senddata(s, string.Format("MAIL FROM:<{0}>",this._MailFrom));
                            str = this.Get_Response(s);
                            if (Convert.ToInt32(str.Substring(0, 3)) != 250)
                            {
                                return str;
                            }
                            this.Senddata(s, string.Format("RCPT TO:<{0}>",Email_Adress));
                            str = this.Get_Response(s);
                            if (Convert.ToInt32(str.Substring(0, 3)) == 250)
                            {
                                this.Senddata(s, "QUIT\r\n");
                                s.Close();
                                return "";
                            }
                        }
                        return str;
                    }
                }
                catch (Exception)
                {
                    if (str == "")
                    {
                        str = "004 : Mx entry problem";
                    }
                }
            }
            return str;
        }

        public bool Check_SMTP(string Email_Adress)
        {
            if (!Email_Adress.Contains("@") || this.Check_Syntax(Email_Adress))
            {
                foreach (string str in this.FindMXRecords(Email_Adress))
                {
                    try
                    {
                        

                        IPEndPoint remoteEP = new IPEndPoint(Dns.GetHostEntry(str).AddressList[0], this._Smtpport);
                        Socket s = new Socket(remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        s.Connect(remoteEP);
                        if (this.Get_Response_Code(s) == 220)
                        {
                            this.Senddata(s, string.Format("HELO {0}",Dns.GetHostName()));
                            if (this.Get_Response_Code(s) == 250)
                            {
                                this.Senddata(s, "QUIT\r\n");
                                s.Close();
                                return true;
                            }
                        }
                        s.Close();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return false;
        }

        private void e_Completed(object sender, SocketAsyncEventArgs e)
        {
            //string stat = "Connection Established";
           //WaitForServerData();
        }
     
        public bool Check_Syntax(string Mail_Adress)
        {
            Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return regex.IsMatch(Mail_Adress);
        }

        public void Dispose()
        {
        }

        [DllImport("dnsapi", EntryPoint="DnsQuery_W", CharSet=CharSet.Unicode, SetLastError=true, ExactSpelling=true)]
        private static extern int DnsQuery([MarshalAs(UnmanagedType.VBByRefStr)] ref string pszName, QueryTypes wType, QueryOptions options, int aipServers, ref IntPtr ppQueryResults, int pReserved);
        [DllImport("dnsapi", CharSet=CharSet.Auto, SetLastError=true)]
        private static extern void DnsRecordListFree(IntPtr pRecordList, int FreeType);
        public string[] FindMXRecords(string Email_or_Domain)
        {
            string domainName = this.GetDomainName(Email_or_Domain);
            ArrayList list = new ArrayList();
            try
            {
                MXRecord record;
                IntPtr zero = IntPtr.Zero;
                IntPtr ptr = IntPtr.Zero;
                if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                {
                    throw new NotSupportedException();
                }
                int error = DnsQuery(ref domainName, QueryTypes.DNS_TYPE_MX, QueryOptions.DNS_QUERY_BYPASS_CACHE, 0, ref zero, 0);
                if (error != 0)
                {
                    throw new Win32Exception(error);
                }
                for (ptr = zero; !ptr.Equals(IntPtr.Zero); ptr = record.pNext)
                {
                    record = (MXRecord) Marshal.PtrToStructure(ptr, typeof(MXRecord));
                    if (record.wType == 15)
                    {
                        string str2 = Marshal.PtrToStringAuto(record.pNameExchange);
                        list.Add(str2);
                    }
                }
                DnsRecordListFree(zero, 0);
            }
            catch (Exception)
            {
            }
            if (list.Count == 0)
            {
                list.Add(domainName);
            }
            return (string[]) list.ToArray(typeof(string));
        }

        private string Get_Response(Socket s)
        {
            byte[] buffer = new byte[0x400];
            int num = 0;
            while (s.Available == 0)
            {
                Thread.Sleep(100);
                num++;
                if (num > 30)
                {
                    s.Close();
                    return "000 : Timeout";
                }
            }
            s.Receive(buffer, 0, s.Available, SocketFlags.None);
            string str = Encoding.ASCII.GetString(buffer);
            if (str.IndexOf(Environment.NewLine) > 0)
            {
                return str.Substring(0, str.IndexOf(Environment.NewLine));
            }
            return str;
        }

        private int Get_Response_Code(Socket s)
        {
            byte[] buffer = new byte[0x400];
            int num = 0;
            while (s.Available == 0)
            {
                Thread.Sleep(100);
                num++;
                if (num > 30)
                {
                    s.Close();
                    return 0;
                }
            }
            s.Receive(buffer, 0, s.Available, SocketFlags.None);
            return Convert.ToInt32(Encoding.ASCII.GetString(buffer).Substring(0, 3));
        }

        private string GetDomainName(string Mail_Adress)
        {
            if (Mail_Adress.Contains("@"))
            {
                return Mail_Adress.Split(new char[] { '@' })[1];
            }
            return Mail_Adress;
        }

        public bool HasMXRecords(string Mail_Adress){
            return (this.FindMXRecords(Mail_Adress).Length > 0);
        }

        private void Senddata(Socket s, string msg)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(msg);
            s.Send(bytes, 0, bytes.Length, SocketFlags.None);
        }

        public string Mail_From
        {
            get 
            {
                return this._MailFrom;
            }
            set
            {
                this._MailFrom = value;
            }
        }

        public int Smtp_Port
        {
            get
            {
                return this._Smtpport;
            }
            set
            {
                this._Smtpport = value;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MXRecord
        {
            public IntPtr pNext;
            public string pName;
            public short wType;
            public short wDataLength;
            public int flags;
            public int dwTtl;
            public int dwReserved;
            public IntPtr pNameExchange;
            public short wPreference;
            public short Pad;
        }

        private enum QueryOptions
        {
            DNS_QUERY_ACCEPT_TRUNCATED_RESPONSE = 1,
            DNS_QUERY_BYPASS_CACHE = 8,
            DNS_QUERY_DONT_RESET_TTL_VALUES = 0x100000,
            DNS_QUERY_NO_HOSTS_FILE = 0x40,
            DNS_QUERY_NO_LOCAL_NAME = 0x20,
            DNS_QUERY_NO_NETBT = 0x80,
            DNS_QUERY_NO_RECURSION = 4,
            DNS_QUERY_NO_WIRE_QUERY = 0x10,
            DNS_QUERY_RESERVED = -16777216,
            DNS_QUERY_RETURN_MESSAGE = 0x200,
            DNS_QUERY_STANDARD = 0,
            DNS_QUERY_TREAT_AS_FQDN = 0x1000,
            DNS_QUERY_USE_TCP_ONLY = 2,
            DNS_QUERY_WIRE_ONLY = 0x100
        }

        private enum QueryTypes
        {
            DNS_TYPE_MX = 15
        }
    }
}

