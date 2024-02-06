using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoblistingPeopleYouMayKnowService
{
    public class PeopleEntity
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
        public long MUserId { get; set; }
        public string MUsername { get; set; }
        public string MFirstName { get; set; }
        public string MLastName { get; set; }
        public string MCompany { get; set; }
        public string MPermaLink { get; set; }
        public int MType { get; set; }
        public int? MCategoryId { get; set; }
        public int? MSpecializationId { get; set; }
        public string MAddress { get; set; }
        public string MCity { get; set; }
        public long? MCountryId { get; set; }
        public long? MStateId { get; set; }
        public string MZip { get; set; }
        public string MPhone { get; set; }
        public string MMobile { get; set; }
        public string MUniversity { get; set; }
        public string MSchool { get; set; }
        public string MCurrentEmployer { get; set; }
        public string MPreviousEmployer { get; set; }

        public int AlertType { get; set; }
        public string Template { get; set; }
        public string BaseUrl { get; set; }
    }
}
