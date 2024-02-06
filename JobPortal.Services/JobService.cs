#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Services
{
    public class JobService : DataService, IJobService
    {
#pragma warning disable CS0246 // The type or namespace name 'ViewJobModel' could not be found (are you missing a using directive or an assembly reference?)
        public ViewJobModel Get(long jobId, long? userId = null)
#pragma warning restore CS0246 // The type or namespace name 'ViewJobModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>(){
                new Parameter("Id", jobId),
                new Parameter("UserId", userId)
            };

            return Single<ViewJobModel>("JobSingle", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'JobPostModel' could not be found (are you missing a using directive or an assembly reference?)
        public List<JobPostModel> Jobs(long employerId)
#pragma warning restore CS0246 // The type or namespace name 'JobPostModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            throw new NotImplementedException();
        }

#pragma warning disable CS0246 // The type or namespace name 'JobPostModel' could not be found (are you missing a using directive or an assembly reference?)
        public long Post(JobPostModel model)
#pragma warning restore CS0246 // The type or namespace name 'JobPostModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            throw new NotImplementedException();
        }

#pragma warning disable CS0246 // The type or namespace name 'JobPostModel' could not be found (are you missing a using directive or an assembly reference?)
        public long Repost(JobPostModel model)
#pragma warning restore CS0246 // The type or namespace name 'JobPostModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            throw new NotImplementedException();
        }

        public bool Approve(long jobId, long userId)
        {
            throw new NotImplementedException();
        }

        public bool Reject(long jobId, long userId)
        {
            throw new NotImplementedException();
        }

        public bool Delete(long jobId, long userId)
        {
            throw new NotImplementedException();
        }


        public bool Bookmark(long jobId, long jobseekerId)
        {
            throw new NotImplementedException();
        }

#pragma warning disable CS0246 // The type or namespace name 'Student' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<Student>> GetLatestStudent1(string student, string country)
#pragma warning restore CS0246 // The type or namespace name 'Student' could not be found (are you missing a using directive or an assembly reference?)
        {

            List<Parameter> parameters = new List<Parameter>()
            {
                 new Parameter("StudentName", student),
                new Parameter("country", country)
        };
            return await ReadAsync<Student>("GetStudentList", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'SearchedJobEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SearchJob' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<SearchedJobEntity>> Search(SearchJob model)
#pragma warning restore CS0246 // The type or namespace name 'SearchJob' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'SearchedJobEntity' could not be found (are you missing a using directive or an assembly reference?)
        {

            SqlParameter[] parameters = {
                    new SqlParameter("@Name", SqlDbType.Text,100) { Value = model.Title },
                    new SqlParameter("@Where", SqlDbType.Text,100) { Value = model.Where },
new SqlParameter("@CategoryId", SqlDbType.Int,100) { Value = model.CategoryId },
new SqlParameter("@SpecializationId", SqlDbType.Int,100) { Value = model.SpecializationId },
new SqlParameter("@CountryId", SqlDbType.Int,100) { Value = model.CountryId },
new SqlParameter("@StateOrCity", SqlDbType.Text,100) { Value = model.StateOrCity },
new SqlParameter("@Zip", SqlDbType.Text,100) { Value = model.Zip },
new SqlParameter("@JobType", SqlDbType.Int,100) { Value = model.EmploymentType },
new SqlParameter("@Qualification", SqlDbType.Int,100) { Value = model.QualificationId },
new SqlParameter("@Start", SqlDbType.DateTime,100) { Value = model.StartDate },
new SqlParameter("@End", SqlDbType.DateTime,100) { Value = model.EndDate },
new SqlParameter("@Username", SqlDbType.Text,100) { Value = model.Username },
new SqlParameter("@PageNumber", SqlDbType.Int,100) { Value = model.PageNumber },
new SqlParameter("@PageSize", SqlDbType.Int,100) { Value = model.PageSize },
                };


            //List<Parameter> parameters = new List<Parameter>()
            //{
            //    new Parameter("Name", model.Title),
            //    new Parameter("Where", model.Where),
            //    new Parameter("CategoryId", model.CategoryId),
            //    new Parameter("SpecializationId", model.SpecializationId),
            //    new Parameter("CountryId", model.CountryId),
            //    new Parameter("StateOrCity", model.StateOrCity),
            //    new Parameter("Zip", model.Zip),
            //    new Parameter("JobType", model.EmploymentType),
            //    new Parameter("Qualification", model.QualificationId),
            //    new Parameter("Start", model.StartDate),
            //    new Parameter("End", model.EndDate),
            //    new Parameter("Username", model.Username),
            //    new Parameter("PageNumber", model.PageNumber),
            //    new Parameter("PageSize", model.PageSize)
            //};
            return await Read1Async<SearchedJobEntity>("SearchJobsNew", parameters);
        }

        public async Task<List<SearchedJobEntity>> SearchNew(SearchJob model)
#pragma warning restore CS0246 // The type or namespace name 'SearchJob' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'SearchedJobEntity' could not be found (are you missing a using directive or an assembly reference?)
        {

            SqlParameter[] parameters = {
                    new SqlParameter("@Name", SqlDbType.Text,100) { Value = model.Title },
                    new SqlParameter("@Where", SqlDbType.Text,100) { Value = model.Where },
new SqlParameter("@CategoryId", SqlDbType.Int,100) { Value = model.CategoryId },
new SqlParameter("@SpecializationId", SqlDbType.Int,100) { Value = model.SpecializationId },
new SqlParameter("@CountryId", SqlDbType.Int,100) { Value = model.CountryId },
new SqlParameter("@StateOrCity", SqlDbType.Text,100) { Value = model.StateOrCity },
new SqlParameter("@Zip", SqlDbType.Text,100) { Value = model.Zip },
new SqlParameter("@JobType", SqlDbType.Int,100) { Value = model.EmploymentType },
new SqlParameter("@Qualification", SqlDbType.Int,100) { Value = model.QualificationId },
new SqlParameter("@Start", SqlDbType.DateTime,100) { Value = model.StartDate },
new SqlParameter("@End", SqlDbType.DateTime,100) { Value = model.EndDate },
new SqlParameter("@Username", SqlDbType.Text,100) { Value = model.Username },
new SqlParameter("@PageNumber", SqlDbType.Int,100) { Value = model.PageNumber },
new SqlParameter("@PageSize", SqlDbType.Int,100) { Value = model.PageSize },
                };


            //List<Parameter> parameters = new List<Parameter>()
            //{
            //    new Parameter("Name", model.Title),
            //    new Parameter("Where", model.Where),
            //    new Parameter("CategoryId", model.CategoryId),
            //    new Parameter("SpecializationId", model.SpecializationId),
            //    new Parameter("CountryId", model.CountryId),
            //    new Parameter("StateOrCity", model.StateOrCity),
            //    new Parameter("Zip", model.Zip),
            //    new Parameter("JobType", model.EmploymentType),
            //    new Parameter("Qualification", model.QualificationId),
            //    new Parameter("Start", model.StartDate),
            //    new Parameter("End", model.EndDate),
            //    new Parameter("Username", model.Username),
            //    new Parameter("PageNumber", model.PageNumber),
            //    new Parameter("PageSize", model.PageSize)
            //};
            return await Read1Async<SearchedJobEntity>("SearchJobsNew1", parameters);
        }


        public async Task<List<LatestJob>> SearchNew3(SearchJob model)
#pragma warning restore CS0246 // The type or namespace name 'SearchJob' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'SearchedJobEntity' could not be found (are you missing a using directive or an assembly reference?)
        {

            SqlParameter[] parameters = {
                    new SqlParameter("@Name", SqlDbType.Text,100) { Value = model.Title },
                    new SqlParameter("@Where", SqlDbType.Text,100) { Value = model.Where },
new SqlParameter("@CategoryId", SqlDbType.Int,100) { Value = model.CategoryId },
new SqlParameter("@SpecializationId", SqlDbType.Int,100) { Value = model.SpecializationId },
new SqlParameter("@CountryId", SqlDbType.Int,100) { Value = model.CountryId },
new SqlParameter("@StateOrCity", SqlDbType.Text,100) { Value = model.StateOrCity },
new SqlParameter("@Zip", SqlDbType.Text,100) { Value = model.Zip },
new SqlParameter("@JobType", SqlDbType.Int,100) { Value = model.EmploymentType },
new SqlParameter("@Qualification", SqlDbType.Int,100) { Value = model.QualificationId },
new SqlParameter("@Start", SqlDbType.DateTime,100) { Value = model.StartDate },
new SqlParameter("@End", SqlDbType.DateTime,100) { Value = model.EndDate },
new SqlParameter("@Username", SqlDbType.Text,100) { Value = model.Username },
new SqlParameter("@PageNumber", SqlDbType.Int,100) { Value = model.PageNumber },
new SqlParameter("@PageSize", SqlDbType.Int,100) { Value = model.PageSize },
                };


            //List<Parameter> parameters = new List<Parameter>()
            //{
            //    new Parameter("Name", model.Title),
            //    new Parameter("Where", model.Where),
            //    new Parameter("CategoryId", model.CategoryId),
            //    new Parameter("SpecializationId", model.SpecializationId),
            //    new Parameter("CountryId", model.CountryId),
            //    new Parameter("StateOrCity", model.StateOrCity),
            //    new Parameter("Zip", model.Zip),
            //    new Parameter("JobType", model.EmploymentType),
            //    new Parameter("Qualification", model.QualificationId),
            //    new Parameter("Start", model.StartDate),
            //    new Parameter("End", model.EndDate),
            //    new Parameter("Username", model.Username),
            //    new Parameter("PageNumber", model.PageNumber),
            //    new Parameter("PageSize", model.PageSize)
            //};


            return await Read1Async<LatestJob>("SearchJobsNew3", parameters); ;
        }
#pragma warning disable CS0246 // The type or namespace name 'Companies' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<Companies>> GetLatestCompanies1(string company, string country)
#pragma warning restore CS0246 // The type or namespace name 'Companies' could not be found (are you missing a using directive or an assembly reference?)
        {

            List<Parameter> parameters = new List<Parameter>()
            {
                 new Parameter("Company", company),
                new Parameter("country", country)
        };
            return await ReadAsync<Companies>("GetCompanies", parameters);
        }
#pragma warning disable CS0246 // The type or namespace name 'RecentJobEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<RecentJobEntity>> RecentJobs(long? countryId = null, long? employerId = null, int pageNumber = 1)
#pragma warning restore CS0246 // The type or namespace name 'RecentJobEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("CountryId", countryId),
                new Parameter("EmployerId", employerId),
                new Parameter("PageNumber", pageNumber)
            };
            return await ReadAsync<RecentJobEntity>("GetRecentJobs", parameters);
        }

        public int ApplicationCounts(long? employerId = null, long? jobseekerId = null)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("EmployerId", employerId),
                new Parameter("JobseekerId", jobseekerId),
            };

            return Scaler<int>("ApplicationCounts", parameters);
        }

        public int BookmarkCounts(long userId)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", userId)
            };

            return Scaler<int>("BookmarkCounts", parameters);
        }

        public int InterviewCounts(long? employerId = null, long? jobseekerId = null, long? id = null)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("EmployerId", employerId),
                new Parameter("JobseekerId", jobseekerId),
                new Parameter("Id", id),
            };

            return Scaler<int>("InterviewCounts", parameters);
        }
        public int VerifyTokenM(long userid, string name, string token)
        {
            int count = Scaler<int>(string.Format("SELECT COUNT(*) FROM Tokentable WHERE Userid= {0} AND Username = '{1}' and Token='{2}' and Type='Message'", userid, name, token));

            return count;
        }
        public int VerifiedTokenM(long userid)
        {
            int count = Scaler<int>(string.Format("select Count(*) from [dbo].[Tokentable] where Validity_date>='{0}' and status = 'Y' and Userid = {1} and Type = 'Message'", DateTime.Now, userid));

            return count;
        }
        public int UpdateTokenM(long userid)
        {
            return HandleData(string.Format("Update [dbo].[Tokentable] set status='Y' where Userid={0} and Type = 'Message'", userid));
        }
        public int VerifyTokenI(long userid, string name, string token)
        {
            int count = Scaler<int>(string.Format("SELECT COUNT(*) FROM Tokentable WHERE Userid= {0} AND Username = '{1}' and Token='{2}' and Type='Interview'", userid, name, token));

            return count;
        }
        public int VerifiedTokenI(long userid)
        {
            int count = Scaler<int>(string.Format("select Count(*) from[dbo].[Tokentable] where Validity_date>='{0}' and status = 'Y' and Userid = {1} and Type = 'Interview'", DateTime.Now, userid));

            return count;
        }
        public int UpdateTokenI(long userid)
        {
            return HandleData(string.Format("Update [dbo].[Tokentable] set status='Y' where Userid={0} and Type = 'Interview'", userid));
        }
        public int VerifyTokenR(long userid, string name, string token)
        {
            int count = Scaler<int>(string.Format("SELECT COUNT(*) FROM Tokentable WHERE Userid= {0} AND Username = '{1}' and Token='{2}' and Type='Resume'", userid, name, token));

            return count;
        }
        public int VerifiedTokenR(long userid)
        {
            int count = Scaler<int>(string.Format("select Count(*) from[dbo].[Tokentable] where Validity_date>='{0}' and status = 'Y' and Userid = {1} and Type = 'Resume'", DateTime.Now, userid));

            return count;
        }
        public int UpdateTokenR(long userid)
        {
            return HandleData(string.Format("Update [dbo].[Tokentable] set status='Y' where Userid={0} and Type = 'Resume'", userid));
        }
        public int VerifyTokenP(long userid, string name, string token)
        {
            int count = Scaler<int>(string.Format("SELECT COUNT(*) FROM Tokentable WHERE Userid= {0} AND Username = '{1}' and Token='{2}' and Type='Promote'", userid, name, token));

            return count;
        }
        public int VerifiedTokenP(long userid)
        {
            int count = Scaler<int>(string.Format("select Count(*) from[dbo].[Tokentable] where Validity_date>='{0}' and status = 'Y' and Userid = {1} and Type = 'Promote'", DateTime.Now, userid));

            return count;
        }
        public int UpdateTokenP(long userid)
        {
            return HandleData(string.Format("Update [dbo].[Tokentable] set status='Y' where Userid={0} and Type = 'Promote'", userid));
        }
        public int DashboardInboxItems(long userid)
        {
            int count = Scaler<int>(string.Format("SELECT COUNT(Id) FROM Inbox WHERE ReceiverId = {0} AND Unread = 1", userid));
            return count;
        }

        public int MessageCounts(long userId)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", userId)
            };

            return Scaler<int>("MessageCounts", parameters);
        }


#pragma warning disable CS0246 // The type or namespace name 'IndeedJob' could not be found (are you missing a using directive or an assembly reference?)
        public long ManageAggregatorJob(IndeedJob job)
#pragma warning restore CS0246 // The type or namespace name 'IndeedJob' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("Source", job.Source),
                new Parameter("JobKey", job.JobKey),
                new Parameter("JobUrl", job.Url),
                new Parameter("JobTitle", job.JobTitle),
                new Parameter("Description", job.Snippet),
                new Parameter("Company", job.Company),
                new Parameter("Country", job.Country),
                new Parameter("State", job.State),
                new Parameter("City", job.City),
                new Parameter("Sponsored", job.Sponsored),
                new Parameter("Expired", job.Expired),
                new Parameter("CountryId", job.CountryId),
                new Parameter("CategoryId", job.CategoryId),
            };

            return Scaler<long>("ManageAggregatedJob", parameters);
        }


#pragma warning disable CS0246 // The type or namespace name 'JobNotifyEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<JobNotifyEntity> JobNotifyList()
#pragma warning restore CS0246 // The type or namespace name 'JobNotifyEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            //HandleData("DELETE FROM JobHireMails");
            return Read<JobNotifyEntity>("SELECT UserId, Username, Name, Country, Jobs FROM VIEW_JOBS_BY_USER_COUNTRY WHERE Jobs IS NOT NULL ORDER BY UserId");
        }
        public int DelSkillData(long userid, string skillname)
        {
            return HandleData(string.Format("Delete from User_Skills where UserId={0} and SkillName='{1}'", userid, skillname));
        }
        public int InviteViaEmailEdU(long userid, string emailv1, string emailname1, string edv, string scv, string ftv, string ttv, string body2)
        {
            return HandleData(string.Format("Update UserEducation_Verify set Status='Verified', DateUpdated='{6}' where UserId={0} and Education='{1}' and SCName='{2}' and [From] ='{3}' and [To]='{4}' and Email='{5}'", userid, edv, scv, ftv, ttv, emailv1, DateTime.Now));
        }
        public int InviteViaEmailEdI(long userid, string emailv1, string emailname1, string edv, string scv, string ftv, string ttv, string body2)
        {
            return HandleData(string.Format("INSERT INTO UserEducation_Verify(UserId,Education,SCName,[From],[To],EmailName,Email,[Subject],[Status],DateUpdated) VALUES({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','Verify','{8}')", userid, edv, scv, ftv, ttv, emailname1, emailv1, body2, DateTime.Now));
        }
        public int InviteViaEmailI(long userid, string emailv, string emailname, string comv, string indv, string conv, string body1)
        {
            return HandleData(string.Format("INSERT INTO User_Verify(UserId,Company,Industry,Country,EmailName,Email,Subject,Status,DateUpdated) VALUES({0},'{1}','{2}','{3}','{4}','{5}','{6}','Verify','{7}')", userid, comv, indv, conv, emailname, emailv, body1, DateTime.Now));
        }
        public int InviteViaEmailU(long userid, string emailv, string emailname, string comv, string indv, string conv, string body1)
        {
            return HandleData(string.Format("Update User_Verify set Status='Verified', DateUpdated='{5}' where UserId={0} and Company='{1}' and Industry='{2}' and Country ='{3}' and Email='{4}'", userid, comv, indv, conv, emailv, DateTime.Now));
        }
        public int UserExperienceUpdate(long userid, string employer, int frommonth, int tomonth, long fromyr, long toyr, string ind, string role, string resp, int utype, string cid, string cat)
        {
            return HandleData(string.Format("INSERT INTO User_Experience(UserId,Employer,Industry,Roles,FromMonth,ToMonth,FromYr,ToYr,Responsibilities,UType,CountryId,Category) VALUES({0},'{1}','{2}','{3}',{4},{5},{6},{7},'{8}',{9},'{10}','{11}')", userid, employer, ind, role, frommonth, tomonth, fromyr, toyr, resp, utype, cid, cat));
        }
        public int UserExperienceUpdateIn(long userid, string employer, int frommonth, int tomonth, long fromyr, long toyr, string ind, string role, string resp, int utype, string cid, string cat)
        {
            return HandleData(string.Format("UPDATE User_Experience set FromMonth={4},ToMonth={5},FromYr={6},ToYr={7},Responsibilities='{8}',CountryId='{10}' where UserId={0} and Employer='{1}' and Industry='{2}' and Category='{11}' and Roles='{3}'", userid, employer, ind, role, frommonth, tomonth, fromyr, toyr, resp, utype, cid, cat));
        }
        public int UserEducationUpdate(long userid, string education, long fromyr, long toyr, string school, string grade, int utype)
        {
            return HandleData(string.Format("INSERT INTO User_Education(UserId,Education,FromYr,ToYr,School,Grade,UType) VALUES({0},'{1}',{2},{3},'{4}','{5}',{6})", userid, education, fromyr, toyr, school, grade, utype));
        }
        public int UserEducationUpdateIn(long userid, string education, long fromyr, long toyr, string school, string grade, int utype)
        {
            return HandleData(string.Format("Update User_Education set FromYr={2},ToYr={3},School='{4}',Grade='{5}' where UserId= {0} and Education='{1}'", userid, education, fromyr, toyr, school, grade, utype));
        }


        public int UserSkillUpdate(long userid, string skill, int val, decimal per, int utype)
        {
            return HandleData(string.Format("INSERT INTO User_Skills(UserId,SkillName,Rating,Percentage_Cal,UType) VALUES({0},'{1}',{2},{3},{4})", userid, skill, val, per, utype));
        }
        public int UserSkillUpdateIn(long userid, string skill, int val, decimal per, int utype)
        {
            return HandleData(string.Format("Update User_Skills set Rating={2},Percentage_Cal={3} where UserId= {0} and SkillName='{1}'", userid, skill, val, per, utype));
        }
        public int DelEduData(long userid, string edu)
        {
            return HandleData(string.Format("Delete from User_Education where UserId={0} and Education='{1}'", userid, edu));
        }
        public int DelExpData(long userid, string emp, string ind)
        {
            return HandleData(string.Format("Delete from User_Experience where UserId={0} and Employer='{1}' and Industry='{2}'", userid, emp, ind));
        }
        public int DelCertData(long userid, string cert)
        {
            return HandleData(string.Format("Delete from User_Certificate where UserId={0} and Certificate='{1}'", userid, cert));
        }

        public int UserCertUpdate(long userid, string cert, string inst, string logo, string logotype, int utype)
        {
            return HandleData(string.Format("INSERT INTO User_Certificate(UserId,Certificate,Institute,Logo,LogoType,Utype) VALUES({0},'{1}','{2}','{3}','{4}',{5})", userid, cert, inst, logo, logotype, utype));
        }

        public int UserCertUpdateIn(long userid, string cert, string inst, string logo, string logotype, int utype)
        {
            return HandleData(string.Format("Update User_Certificate set Institute='{2}',Logo='{3}',LogoType='{4}',Utype={5} where UserId={0} and Certificate='{1}'", userid, cert, inst, logo, logotype, utype));
        }
        public int JobNotifyUpdate(string email)
        {
            return HandleData(string.Format("INSERT INTO JobHireMails(Email) VALUES('{0}')", email));
        }


        public int UpdateAlert(int alertType, string receiver)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("AlertId", alertType));
            parameters.Add(new Parameter("Receiver", receiver));
            parameters.Add(new Parameter("Sender", "donotreply@joblisting.com"));

            return HandleData("TrackAlertHistory", parameters);
        }


#pragma warning disable CS0246 // The type or namespace name 'JobFeedEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<JobFeedEntity> JobFeedList()
#pragma warning restore CS0246 // The type or namespace name 'JobFeedEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            return Read<JobFeedEntity>("JobFeedList");
        }


#pragma warning disable CS0246 // The type or namespace name 'SearchedJobEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SearchJob' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<SearchedJobEntity>> PaidJobs(SearchJob model)
#pragma warning restore CS0246 // The type or namespace name 'SearchJob' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'SearchedJobEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            //List<Parameter> parameters = new List<Parameter>()
            //{
            //    new Parameter("Name", model.Title),
            //    new Parameter("Where", model.Where),
            //    new Parameter("CategoryId", model.CategoryId),
            //    new Parameter("SpecializationId", model.SpecializationId),
            //    new Parameter("CountryId", model.CountryId),
            //    new Parameter("StateOrCity", model.StateOrCity),
            //    new Parameter("JobType", model.EmploymentType),
            //    new Parameter("Qualification", model.QualificationId),
            //    new Parameter("Start", model.StartDate),
            //    new Parameter("End", model.EndDate),
            //    new Parameter("Username", model.Username),
            //    new Parameter("PageNumber", model.PageNumber),
            //    new Parameter("PageSize", model.PageSize)
            //};

            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("CountryId", model.CountryId),
                new Parameter("Name", model.Title),
                new Parameter("Username", model.Username),
                new Parameter("PageNumber", model.PageNumber),
                new Parameter("PageSize", model.PageSize)
            };
            return await ReadAsync<SearchedJobEntity>("PaidJobs", parameters);
        }
    }
}
