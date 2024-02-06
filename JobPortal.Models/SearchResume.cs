namespace JobPortal.Models
{
    public class SearchResume
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public int? CategoryId { get; set; }
        public int? SpecializationId { get; set; }
        public int? EmploymentType { get; set; }
        public int? QualificationId { get; set; }
        public long? CountryId { get; set; }
        public long? StateId { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public int? AgeMin { get; set; }
        public int? AgeMax { get; set; }
        public int? Experience { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public string Username { get; set; }
        public string Who { get; set; }
        public string Where { get; set; }
        public int? Type { get; set; }
        public string Gender { get; set; }
        public int? Relationship { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long? UserId { get; set; }
    }
}