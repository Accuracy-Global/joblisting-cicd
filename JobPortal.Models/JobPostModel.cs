using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models 
{
    public class JobPostModel
    {
        public long? Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int SpecializationId { get; set; }
        public long CountryId { get; set; }
        public long? StateId { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public byte? MinimumAge { get; set; }
        public byte? MaximumAge { get; set; }
        public byte? MinimumExperience { get; set; }
        public byte? MaximumExperience { get; set; }
        public string Currency { get; set; }
        public decimal? MinimumSalary { get; set; }
        public decimal? MaximumSalary { get;set; }
        public int? EmployementTypeId { get; set; }
        public int? Qualification { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Requirements { get; set; }
        public string Responsibilities { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime ClosingDate { get; set; }

        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
        public bool IsRejected { get; set; }
        public bool IsDeleted { get; set; }
        public int Distribute { get; set; }
        public bool IsPaid { get; set; }
        public string Noticeperiod { get; set; }

        public string Optionalskills { get; set; }


    }
}
