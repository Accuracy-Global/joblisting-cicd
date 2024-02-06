using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class AccountsInfoEntity
    {
        public int Todays { get; set; }
        public int SevenDays { get; set; }
        public int IndividualAccounts { get; set; }
        public int JobseekerAccounts { get; set; }
        public int Resumes { get; set; }
        public int CompanyAccounts { get; set; }
        public int VerifiedIndividualAccounts { get; set; }
        public int VerifiedCompanyAccounts { get; set; }
        public int TotalVerifiedAccounts { get; set; }
        public int DeletedAccounts { get; set; }
        public int InactiveAccounts { get; set; }
        public int TotalAccounts { get; set; }

        public int Jobs { get; set; }
        public int ActiveJobs { get; set; }
        public int ExpiredJobs { get; set; }
        public int RejectedJobs { get; set; }
        public int DeactivatedJobs { get; set; }
        public int DeletedJobs { get; set; }
        public int TodaysJobs { get; set; }
        public int SevenDaysJobs { get; set; }
        public int JobsInApproval { get; set; }
    }
}
