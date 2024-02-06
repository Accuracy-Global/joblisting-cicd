using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class AuditEntity
    {
        public long UserId { get; set; }
        public int Type{get;set;}
        public string Description { get; set; }
        public long? Reference { get; set; }
        public string Browser { get; set; }
        public string IPAddress { get; set; }
        public bool Failed { get; set; }
        public string Comments { get; set; }
        public AuditEntity()
        {
            Failed = false;
        }

    }
}
