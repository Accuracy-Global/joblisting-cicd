using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class TransactionEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string PackageName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Price { get; set; }
        public int Blocked { get; set; }
        public int? Jobs { get; set; }
        public int UsedJobs { get; set; }
        public int BalanceJobs { get; set; }

        public int Profiles { get; set; }
        public int UsedProfiles { get; set; }
        public int BalanceProfiles { get; set; }

        public int Messages { get; set; }
        public int UsedMessages { get; set; }
        public int BalanceMessages { get; set; }

        public int Interviews { get; set; }
        public int UsedInterviews { get; set; }
        public int BalanceInterviews { get; set; }

        public int Resumes { get; set; }
        public int UsedResumes { get; set; }
        public int BalanceResumes { get; set; }
        public int? Days { get; set; }
        public string PaymentMethod { get; set; }
        public int MaxRows { get; set; }
    }
}
