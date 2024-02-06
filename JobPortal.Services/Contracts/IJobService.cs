#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Services.Contracts
{
    public interface IJobService
    {
#pragma warning disable CS0246 // The type or namespace name 'ViewJobModel' could not be found (are you missing a using directive or an assembly reference?)
        ViewJobModel Get(long jobId, long? userId = null);
#pragma warning restore CS0246 // The type or namespace name 'ViewJobModel' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'JobPostModel' could not be found (are you missing a using directive or an assembly reference?)
        List<JobPostModel> Jobs(long employerId);
#pragma warning restore CS0246 // The type or namespace name 'JobPostModel' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'JobPostModel' could not be found (are you missing a using directive or an assembly reference?)
        long Post(JobPostModel model);
#pragma warning restore CS0246 // The type or namespace name 'JobPostModel' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'JobPostModel' could not be found (are you missing a using directive or an assembly reference?)
        long Repost(JobPostModel model);
#pragma warning restore CS0246 // The type or namespace name 'JobPostModel' could not be found (are you missing a using directive or an assembly reference?)
        bool Approve(long jobId, long userId);
        bool Reject(long jobId, long userId);
        bool Delete(long jobId, long userId);
        bool Bookmark(long jobId, long jobseekerId);
#pragma warning disable CS0246 // The type or namespace name 'Student' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<Student>> GetLatestStudent1(string student, string country);
#pragma warning restore CS0246 // The type or namespace name 'Student' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Companies' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<Companies>> GetLatestCompanies1(string company, string country);
#pragma warning restore CS0246 // The type or namespace name 'Companies' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SearchJob' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SearchedJobEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<SearchedJobEntity>> Search(SearchJob model);

        Task<List<LatestJob>> SearchNew3(SearchJob model);
#pragma warning restore CS0246 // The type or namespace name 'SearchedJobEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'SearchJob' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SearchJob' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SearchedJobEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<SearchedJobEntity>> PaidJobs(SearchJob model);
#pragma warning restore CS0246 // The type or namespace name 'SearchedJobEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'SearchJob' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'RecentJobEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<RecentJobEntity>> RecentJobs(long? countryId = null, long? employerId = null, int pageNumber = 1);
#pragma warning restore CS0246 // The type or namespace name 'RecentJobEntity' could not be found (are you missing a using directive or an assembly reference?)
        int ApplicationCounts(long? employerId = null, long? jobseekerId = null);
        int BookmarkCounts(long userId);
        int InterviewCounts(long? employerId = null, long? jobseekerId = null, long? id = null);
        int DashboardInboxItems(long userid);
        int VerifiedTokenM(long userid);
        int VerifyTokenM(long userid, string name, string token);
         int UpdateTokenM(long userid);
        int VerifiedTokenR(long userid);
        int VerifyTokenR(long userid, string name, string token);
         int UpdateTokenR(long userid);
        int VerifiedTokenI(long userid);
        int VerifyTokenI(long userid, string name, string token);
        int UpdateTokenI(long userid);
        int VerifiedTokenP(long userid);
        int VerifyTokenP(long userid, string name, string token);
         int UpdateTokenP(long userid);
        int MessageCounts(long userId);
#pragma warning disable CS0246 // The type or namespace name 'IndeedJob' could not be found (are you missing a using directive or an assembly reference?)
        long ManageAggregatorJob(IndeedJob job);
#pragma warning restore CS0246 // The type or namespace name 'IndeedJob' could not be found (are you missing a using directive or an assembly reference?)
        int InviteViaEmailU(long userid, string emailv, string emailname, string comv, string indv, string conv, string body1);

        int InviteViaEmailI(long userid, string emailv, string emailname, string comv, string indv, string conv, string body1);
        int InviteViaEmailEdU(long userid, string emailv1, string emailname1, string edv, string scv, string ftv, string ttv, string body2);

        int InviteViaEmailEdI(long userid, string emailv1, string emailname1, string edv, string scv, string ftv,string ttv, string body2);

        int DelSkillData(long userid, string skillname);
        int DelCertData(long userid, string cert);
        int DelExpData(long userid, string emp,string ind);
        int DelEduData(long userid, string edu);
#pragma warning disable CS0246 // The type or namespace name 'JobNotifyEntity' could not be found (are you missing a using directive or an assembly reference?)
        List<JobNotifyEntity> JobNotifyList();
#pragma warning restore CS0246 // The type or namespace name 'JobNotifyEntity' could not be found (are you missing a using directive or an assembly reference?)
        int UserExperienceUpdate(long userid, string employer, int frommonth, int tomonth, long fromyr, long toyr, string ind, string role, string resp, int utype,string cid,string cat);

        int UserExperienceUpdateIn(long userid, string employer, int frommonth, int tomonth, long fromyr, long toyr, string ind, string role, string resp, int utype, string cid,string cat);
        int UserEducationUpdate(long userid, string education, long fromyr, long toyr, string school, string grade, int utype);
        int UserEducationUpdateIn(long userid, string education, long fromyr, long toyr, string school, string grade, int utype);
        int UserSkillUpdate(long userid, string skill, int val, decimal per, int utype);
        int UserSkillUpdateIn(long userid, string skill, int val, decimal per, int utype);

        int UserCertUpdate(long userid, string cert, string inst, string logo, string logotype, int utype);
        int UserCertUpdateIn(long userid, string cert, string inst, string logo, string logotype, int utype);


        int JobNotifyUpdate(string email);
        int UpdateAlert(int alertType, string receiver);

#pragma warning disable CS0246 // The type or namespace name 'JobFeedEntity' could not be found (are you missing a using directive or an assembly reference?)
        List<JobFeedEntity> JobFeedList();
#pragma warning restore CS0246 // The type or namespace name 'JobFeedEntity' could not be found (are you missing a using directive or an assembly reference?)
       
    }
}
