using System;

namespace JobPortal.Models
{
    public class SearchJob
    {
        public long JobID { get; set; }
        public string Title { get; set; }
        public int? CategoryId { get; set; }
        public int? SpecializationId { get; set; }
        public long? EmploymentType { get; set; }
        public long? QualificationId { get; set; }
        public long? CountryId { get; set; }
        public string StateOrCity { get; set; }
        public string Zip { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? JobseekerId { get; set; }
        public string Where { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Username { get; set; }
    }
}