using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class DownloadEntity
    {
        public long JobseekerId { get; set; }
        public string Title { get; set; }
        public long CountryId { get; set; }
        public string CountryName { get; set; }
        public DateTime DateUpdated { get; set; }
        public int MaxRows { get; set; }
    }
}
