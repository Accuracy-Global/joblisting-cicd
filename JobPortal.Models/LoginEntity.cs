using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class LoginEntity
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
