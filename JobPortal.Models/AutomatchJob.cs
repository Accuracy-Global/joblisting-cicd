#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
namespace JobPortal.Models
{
    public class AutomatchJob
    {
        public AutomatchJob()
        {
        }

#pragma warning disable CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        public AutomatchJob(Job job, decimal percentage)
#pragma warning restore CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        {
            Job = job;
            Percentage = percentage;
        }

#pragma warning disable CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        public Job Job { get; set; }
#pragma warning restore CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        public decimal Percentage { get; set; }
    }
}