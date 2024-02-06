using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class Country
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string TwoDigitCode { get; set; }
        public bool IsDefault { get; set; }
        public string DialingCode { get; set; }
        public bool IsDeveloped { get; set; }
   
    }

    public class SkillsList
    {
        public string SkillName { get; set; }
    }



    public class CategoryList1
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

}
