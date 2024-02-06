using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class BuyResume
    {
        public long Id { get; set; }
        public int Resumes { get; set; }
        public decimal Rate { get; set; }
        public int PackageId { get; set; }         
    }

    public class BuyMessage
    {
        public Guid? Id { get; set; }
        public int Messages { get; set; }
        public decimal Rate { get; set; }
        public int PackageId { get; set; }
    }

    public class BuyProfile
    {
        public long Id { get; set; }
        public int Profiles { get; set; }
        public decimal Rate { get; set; }
        public int PackageId { get; set; }
    }

    public class BuyInterview
    {
        public long Id { get; set; }
        public int Interviews { get; set; }
        public decimal Rate { get; set; }
        public int PackageId { get; set; }
    }
}
