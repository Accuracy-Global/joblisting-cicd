#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class UserInfoEntity
    {
        public long Id { get; set; } 
        public string Username { get; set; }
        public string Gender { get; set; }
        public int Type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }        
        public string Summary { get; set; }
        public string Address { get; set; }
        public Nullable<long> CountryId { get; set; }
        public string CountryName { get; set; }
        public Nullable<long> StateId { get; set; }
        public string StateName { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }        
        public string Website { get; set; }    
        public bool IsDeleted { get; set; }
        public string PermaLink { get; set; }    
        public string Title { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string Category { get; set; }
        public Nullable<int> SpecializationId { get; set; }
        public string Specialization { get; set; }
        public string Content { get; set; }    
        public bool IsConfirmed { get; set; }
        public bool IsActive { get; set; }
        public string PhoneCountryCode { get; set; }
        public string Phone { get; set; }
        public string MobileCountryCode { get; set; }
        public string Mobile { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public string GooglePlus { get; set; }
        public bool IsJobseeker { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public bool IsValidUsername { get; set; }
        public int? Age { get; set; }
        public int? RelationshipStatus { get; set; }
        public string ConfirmationToken { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? PromoteDate { get; set; }
#pragma warning disable CS0246 // The type or namespace name 'SecurityRoles' could not be found (are you missing a using directive or an assembly reference?)
        public SecurityRoles Role
#pragma warning restore CS0246 // The type or namespace name 'SecurityRoles' could not be found (are you missing a using directive or an assembly reference?)
        {
            get
            {
                return (SecurityRoles)Type;
            }
        }
        public decimal weightage { get; set; }
        public int countofjobs { get; set; }
        public int MaxRows { get; set; }
    }
}
