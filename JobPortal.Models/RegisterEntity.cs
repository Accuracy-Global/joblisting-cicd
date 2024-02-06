using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class RegisterEntity
    {
        public string Mobile { get; set; }

        public string Username { get; set; }
        public string Company { get; set; }
        public string University { get; set; }
        public string Education { get; set; }
        public string CurrentEmployerToYear { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public long? CountryId { get; set; }
        public string PermaLink { get; set; }
        public string ConfirmationToken { get; set; }
        public string DateOfBirth { get; set; }
        public byte? Age { get; set; }
        public string Gender { get; set; }
        public string Language { get; set; }
        public int Type { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public string ProviderUsername { get; set; }
        public string ProviderAccessToken { get; set; }
        public int Category { get; set; }
        public bool Confirmed { get; set; }
      
    }
}
