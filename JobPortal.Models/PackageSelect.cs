using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class PackageSelect
    {
        public Guid? SessionId { get; set; }
        public int Id { get; set; }        
        public long CountryId { get; set; }
        public string Type { get; set; }
        public string ReturnUrl { get; set; }
        public List<Package> List { get; set; }
        public string RedirectUrl { get; set; }     
        public string Email { get; set; }

        public bool ShowCountry { get; set; }
    }
}
