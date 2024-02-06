using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoblistingReminderService
{
    public class IndividualListEntity
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string PermaLink { get; set; }
        public long CountryId { get; set; }
        public long IUserId { get; set; }
        public string IUsername { get; set; }
        public string IFirstName { get; set; }
        public string ILastName { get; set; }
        public string ICompany { get; set; }
        public string IPermaLink { get; set; }

        public int AlertType { get; set; }
        public string Template { get; set; }
        public string BaseUrl { get; set; }
    }
}
