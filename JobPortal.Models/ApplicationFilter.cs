using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class ApplicationFilter
    {
        public string JobTitle {get;set;}
        public string Company {get;set;}
        public DateTime? Start {get;set;}
        public DateTime? End { get; set; }
        public int PageNumber {get;set;}
    }
}
