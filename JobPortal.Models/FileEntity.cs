using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class InboxMessage
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public string FileName { get; set; }
        public string Data { get; set; }
    }

    public class InboxAttachment
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public int Size { get; set; }
    }
}
