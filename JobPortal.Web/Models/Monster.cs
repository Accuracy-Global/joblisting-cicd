using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class Monster
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        //public string Website { get; set; }
        public string Email { get; set; }
        public string Contact_No { get; set; }
        public string Experiance { get; set; }
        public string Location { get; set; }
        public string Resume { get; set; }
        public string Total_Score { get; set; }
        public string skills_count { get; set; }
    }
}