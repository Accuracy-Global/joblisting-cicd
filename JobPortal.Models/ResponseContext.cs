using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class ResponseContext
    {
        public long? Id { get; set; }
        public string Type { get; set; } // Success/Failed
        public string Message { get; set; }
        public object Data { get; set; }
        public string Redirect { get; set; }
    }
}
