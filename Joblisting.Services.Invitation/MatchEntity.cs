using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoblistingReminderService
{
    public class MatchEntity
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string PermaLink { get; set; }
        public int Type { get; set; }
        public int? CategoryId { get; set; }
        public int? SpecializationId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public long? CountryId { get; set; }
        public long? StateId { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string University { get; set; }
        public string School { get; set; }
        public string CurrentEmployer { get; set; }
        public string PreviousEmployer { get; set; }
    }
}
