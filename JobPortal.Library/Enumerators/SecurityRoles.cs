
using System.ComponentModel;
namespace JobPortal.Library.Enumerators
{
    public enum SecurityRoles
    {
        [Description("SuperUser")]
        SuperUser = 1,
        [Description("Administrator")]
        Administrator = 2,
        [Description("Partner")]
        Partner = 3,
       
        [Description("Employer")]
        Employers = 5,
        [Description("Recruitment Agency")]
        RecruitmentAgency = 14,

        [Description("Institute")]
        Institute = 12,
        [Description("Student")]
        Student = 13,
        [Description("UnverifiedUser")]
        UnverifiedUser = 6,
        [Description("Uploader")]
        Uploader = 7,
        [Description("SearchEngineOptimizer")]
        SearchEngineOptimizer = 8,
        [Description("Support")]
        Support = 9,
        [Description("WebScraper")]
        WebScraper =15,
        [Description("Partner")]
        Partners = 16,
        [Description("Freelancer")]
        Freelancers = 17,
        [Description("Intern")]
        Interns = 18,

        [Description("Jobseeker")]
        Jobseeker = 4,

    }
}
