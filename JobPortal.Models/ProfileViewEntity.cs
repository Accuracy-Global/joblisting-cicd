using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class ProfileViewEntity
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime DateViewed { get; set; }
        public string ViewedOn { get; set; }
        public int Views { get; set; }
        public int MaxRows { get; set; }
    }
}
