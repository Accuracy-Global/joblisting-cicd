using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class ImageEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public byte[] Image { get; set; }
        public string Type { get; set; }
        public string Area { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public string Reason { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] NewImage { get; set; }
        public string ImageSize { get; set; }
        public string NewImageSize { get; set; }
        public bool InEditMode { get; set; }
    
    }
}
