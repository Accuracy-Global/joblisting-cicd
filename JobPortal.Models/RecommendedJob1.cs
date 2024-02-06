using System;

namespace JobPortal.Models
{
    public sealed class RecommendedJob1
    {
        public long JobID { get; set; }
        public string Title { get; set; }
        public byte[] Image { get; set; }
        public string ImageType { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Status { get; set; }
        public Nullable<int> TotalViews { get; set; }
        public string JobType { get; set; }
        public string MinimumExperience { get; set; }
        public string JobLevel { get; set; }
        public string jurl { get; set; }
    }
}
