using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class UserEntity
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public int Type { get; set; }
        public string Company { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Summary { get; set; }
        public string Address { get; set; }
        public Nullable<long> CountryId { get; set; }
        public string CountryName { get; set; }
        public Nullable<long> StateId { get; set; }
        public string StateName { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string DateOfBirth { get; set; }
        public string Website { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public string PermaLink { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public string ProviderUsername { get; set; }
        public string ProviderAccessToken { get; set; }
        public string Title { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> SpecializationId { get; set; }
        public Nullable<byte> Age { get; set; }
        public string CurrentEmployer { get; set; }
        public string CurrentEmployerFromMonth { get; set; }
        public string CurrentEmployerFromYear { get; set; }
        public Nullable<byte> Experience { get; set; }
        public string CurrentCurrency { get; set; }
        public Nullable<decimal> DrawingSalary { get; set; }
        public string ExpectedCurrency { get; set; }
        public Nullable<decimal> ExpectedSalary { get; set; }
        public string AreaOfExpertise { get; set; }
        public string ProfessionalExperience { get; set; }
        public string TechnicalSkills { get; set; }
        public string ManagementSkills { get; set; }
        public string Education { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public string School { get; set; }
        public string University { get; set; }
        public Nullable<long> QualificationId { get; set; }
        public Nullable<long> EmploymentTypeId { get; set; }
        public string Certifications { get; set; }
        public string Affiliations { get; set; }
        public string PreviousEmployer { get; set; }
        public string PreviousEmployerFromMonth { get; set; }
        public string PreviousEmployerFromYear { get; set; }
        public string Gender { get; set; }
        public Nullable<int> RelationshipStatus { get; set; }
        public string ConfirmationToken { get; set; }
        public bool IsConfirmed { get; set; }
        public string NewUsername { get; set; }
        public int EmailCorrectionTries { get; set; }
        public bool IsValidUsername { get; set; }
        public string Mobile { get; set; }
        public string PhoneCountryCode { get; set; }
        public string MobileCountryCode { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public string GooglePlus { get; set; }
        public bool IsBuild { get; set; }
    }
}
