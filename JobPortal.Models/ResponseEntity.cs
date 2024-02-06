using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class ResponseEntity
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
