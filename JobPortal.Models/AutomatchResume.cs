#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
namespace JobPortal.Models
{
    public class AutomatchResume
    {
        public AutomatchResume()
        {
        }

#pragma warning disable CS0246 // The type or namespace name 'Resume' could not be found (are you missing a using directive or an assembly reference?)
        public AutomatchResume(Resume resume, decimal percentage)
#pragma warning restore CS0246 // The type or namespace name 'Resume' could not be found (are you missing a using directive or an assembly reference?)
        {
            Resume = resume;
            Percentage = percentage;
        }

#pragma warning disable CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        public Job Job { get; set; }
#pragma warning restore CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Resume' could not be found (are you missing a using directive or an assembly reference?)
        public Resume Resume { get; set; }
#pragma warning restore CS0246 // The type or namespace name 'Resume' could not be found (are you missing a using directive or an assembly reference?)
        public decimal Percentage { get; set; }
    }

    public class AutomatchJobseeker
    {
        public AutomatchJobseeker()
        {
        }

#pragma warning disable CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        public AutomatchJobseeker(UserProfile jobSeeker, decimal percentage)
#pragma warning restore CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        {
            Jobseeker = jobSeeker;
            Percentage = percentage;
        }

#pragma warning disable CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        public Job Job { get; set; }
#pragma warning restore CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        public UserProfile Jobseeker { get; set; }
#pragma warning restore CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        public decimal Percentage { get; set; }
    }
}