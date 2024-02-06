using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class DeviceRegister
    {
        public string DeviceId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public long CountryId { get; set; }
        public string Type { get; set; }
        public string Company { get; set; }
        public string Gender { get; set; }
        public string BirthYear { get; set; }
        public string Password { get; set; }
    }
}
