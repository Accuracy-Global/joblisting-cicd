using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public sealed class Latestcompanies
    {
        
        //public string Image { get; set; }

        //public string ImageType { get; set; }
        public DateTime DateCreated { get; set; }
        public string Company { get; set; }

        public string Country { get; set; }
        public string jurl { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string status { get; set; }
        public long UserId { get; set; }
        public double TotalRow { get; set; }
    }
}
