using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class PhoneContactReminder
    {
        public long Id {get;set;}
        public string DeviceId { get; set; }
        public string Sender{get;set;}
        public string Receiver{get;set;}
        public string Phone{get;set;}        
    }
}
