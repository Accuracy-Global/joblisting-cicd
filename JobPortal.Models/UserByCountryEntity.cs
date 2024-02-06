using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class UserByCountryEntity
    {
        public long CountryId { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
        public int Individuals { get; set; }
        public int VerifiedIndividuals { get; set; }
        public int Jobseekers { get; set; }
        public int Resumes { get; set; }
        public int Companies { get; set; }
        public int VerifiedCompanies { get; set; }
        public int PhotoApprovals { get; set; }
        public int JPhotoApprovals { get; set; }
        public int LogoApprovals { get; set; }
        public int TotalUsers { get; set; }
        public int TotalIndividuals { get; set; }
        public int TotalCompanies { get; set; }
        public int MaxRows { get; set; }
    }
}
