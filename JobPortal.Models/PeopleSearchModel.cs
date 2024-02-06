using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class PeopleSearchModel
    {
        public long? UserId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public long? CountryId { get; set; }
        public bool? IsConnected { get; set; }
        public string DeviceId { get; set; }
        public string Token { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
