using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;
//using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JoblistingVerificationService
{
    public partial class VerificationService : ServiceBase
    {
        public VerificationService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Task jobber = new Task(new Action(Send), TaskCreationOptions.LongRunning);
            jobber.Start();

            Task gender = new Task(new Action(SendGender), TaskCreationOptions.LongRunning);
            gender.Start();
        }

        protected override void OnStop()
        {
        }

        private void Send()
        {
            Reminder reminder = new Reminder();
            try
            {
                while (true)
                {
                    reminder.Send();                    
                    Thread.Sleep(new TimeSpan(0, 5, 0));
                }
            }
            catch (Exception ex)
            {
                reminder.SendEx(ex);
            }
        }
        private void SendGender()
        {
            Reminder reminder = new Reminder();
            try
            {
                while (true)
                {
                    reminder.SendGender();
                    Thread.Sleep(new TimeSpan(0, 5, 0));
                }
            }
            catch (Exception ex)
            {
                reminder.SendEx(ex);
            }
        }
    }
}
