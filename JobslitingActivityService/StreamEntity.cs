using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobslitingActivityService
{
    public class StreamEntity
    {
        public long ActivityId { get; set; }
        public int ActivityType { get; set; }
        public string SenderName { get; set; }
        public string SenderProfileUrl { get; set; }
        public string Recipient { get; set; }
        public string RecipientName { get; set; }
        public long? ImageId { get; set; }
        public long? JobId { get; set; }
        public string JobTitle { get; set; }
        public string JobUrl { get; set; }
        public string Template { get; set; }
        public string BaseUrl { get; set; }
    }
}
