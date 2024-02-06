namespace JobPortal.Models
{
    public class JobsByCategory
    {
        public long? Id { get; set; }
        public string Category { get; set; }
        public string PremaLink { get; set; }
        public int Jobs { get; set; }
    }
}