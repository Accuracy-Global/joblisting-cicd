using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoblistingReminderService
{
    class CompanyListEntity
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string PermaLink { get; set; }

        public long CUserId { get; set; }
        public string CUsername { get; set; }
        public string CFirstName { get; set; }
        public string CLastName { get; set; }
        public string CCompany { get; set; }
        public string CPermaLink { get; set; }
    }
}
