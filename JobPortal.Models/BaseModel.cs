using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class BaseModel
    {        
        public string DeviceId { get; set; }
        public string Token { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
