using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class StreamEntity
    {
        public long Id { get; set; }
        public long Reference { get; set; }
        public int Type { get; set; }
        public long UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Username { get; set; }
        public int? CategoryId { get; set; }
        public int? SpecializationId { get; set; }
        public string Content { get; set; }
        public string FullName { get; set; }
        public string ProfileUrl { get; set; }
        public long? ImageId { get; set; }
        public string ImageArea { get; set; }
        public long? JobId { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string JobLocation { get; set; }
        public string PublishedDate { get; set; }
        public string ClosingDate { get; set; }
        public string JobUrl { get; set; }
        public string Area { get; set; }
        public int Connected { get; set; }
        public int BlockedByMe { get; set; }
        public int Blocked { get; set; }
        public string UserRole { get; set; }
        public int MaxRows { get; set; }

    }
}
