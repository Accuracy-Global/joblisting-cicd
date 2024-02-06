using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Collections;
using System.Linq.Expressions;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using System.Data.Common;
using System.Data.SqlClient;

namespace JobPortal.Domain
{
#pragma warning disable CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    public class JobSeekerService : DataService
#pragma warning restore CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    {
        private static volatile JobSeekerService instance;
        private static readonly object sync = new object();

        /// <summary>
        /// Default private constructor.
        /// </summary>
        private JobSeekerService()
        {
        }

        public static JobSeekerService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new JobSeekerService();
                        }
                    }
                }
                return instance;
            }
        }

        public bool Block(string email, long requestor)
        {
            bool flag = false;
            try
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    UserProfile profile = dataHelper.GetSingle<UserProfile>("UserId", requestor);
                    UserProfile blocked = dataHelper.GetSingle<UserProfile>("Username", email);

                    if (blocked != null && profile != null)
                    {
                        BlockedPeople model = dataHelper.Get<BlockedPeople>().SingleOrDefault(x => x.BlockedId == blocked.UserId && x.BlockerId == profile.UserId);
                        if (model == null)
                        {
                            model = new BlockedPeople()
                            {
                                BlockedId = blocked.UserId,
                                BlockerId = profile.UserId
                            };
                            dataHelper.Add<BlockedPeople>(model, profile.Username);
                            flag = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        public bool Unblock(string email, long requestor)
        {
            bool flag = false;
            try
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    UserProfile profile = dataHelper.GetSingle<UserProfile>("UserId", requestor);
                    UserProfile blocked = dataHelper.GetSingle<UserProfile>("Username", email);

                    if (blocked != null && profile != null)
                    {
                        BlockedPeople model = dataHelper.Get<BlockedPeople>().SingleOrDefault(x => x.BlockedId == blocked.UserId && x.BlockerId == profile.UserId);
                        if (model != null)
                        {
                            flag = dataHelper.Remove<BlockedPeople>(model);
                        }
                    }
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        public bool IsBlocked(long BlockedId, long BlockerId)
        {
            bool flag = false;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var blocked = dataHelper.Get<BlockedPeople>().SingleOrDefault(x => x.BlockedId == BlockedId && x.BlockerId == BlockerId);
                flag = (blocked != null);
            }
            return flag;
        }

#pragma warning disable CS0246 // The type or namespace name 'BlockedPeople' could not be found (are you missing a using directive or an assembly reference?)
        public BlockedPeople GetBlockedEntity(long BlockedId, long userId)
#pragma warning restore CS0246 // The type or namespace name 'BlockedPeople' could not be found (are you missing a using directive or an assembly reference?)
        {
            BlockedPeople entity = new BlockedPeople();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                entity = dataHelper.Get<BlockedPeople>().SingleOrDefault(x => x.BlockedId == BlockedId && x.BlockerId == userId);
                if (entity == null)
                {
                    entity = dataHelper.Get<BlockedPeople>().SingleOrDefault(x => x.BlockedId == userId && x.BlockerId == BlockedId);
                }
            }
            return entity;
        }

        public void Apply(long JobId, long jobseekerId, string Username, out string message)
        {
            message = string.Empty;
            var withdrawn = TrackingService.Instance.Get(TrackingTypes.WITHDRAWN, ShortlistTypes.JOBSEEKER, jobseekerId, JobId, Username);

            var Shortlisted = TrackingService.Instance.Get(TrackingTypes.SHORTLISTED, ShortlistTypes.JOBSEEKER, jobseekerId, JobId, Username);
            if (Shortlisted == null && withdrawn == null)
            {
                var tracking = TrackingService.Instance.Application(TrackingTypes.APPLIED,JobId,jobseekerId,Username, out message);

                var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/Mail/apply.html"));
                var body = string.Empty;
                UserProfile jobSeeker = MemberService.Instance.Get(tracking.JobseekerId.Value);
                Job job = JobService.Instance.Get(tracking.JobId.Value);
                UserProfile employer = MemberService.Instance.Get(job.EmployerId.Value);

                if (!message.Contains("already"))
                {
                    if (reader != null)
                    {
                        body = reader.ReadToEnd();
                        body = body.Replace("@@firstname", jobSeeker.FirstName);
                        body = body.Replace("@@lastname", jobSeeker.LastName);
                        body = body.Replace("@@jobtitle", job.Title);
                        body = body.Replace("@@joburl",
                            string.Format("{0}://{1}/job/{2}-{3}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, job.PermaLink, tracking.JobId));
                        body = body.Replace("@@companyprofile",
                            string.Format("{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, employer.PermaLink));
                        body = body.Replace("@@employer", employer.Company);

                        string[] receipent = { jobSeeker.Username };
                        var subject = string.Format("Application for {0}", job.Title);

                        AlertService.Instance.SendMail(subject, receipent, body);
                    }

                    reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/Mail/employer_apply.html"));
                    body = string.Empty;

                    if (reader != null)
                    {
                        if (!employer.Username.ToLower().Contains("admin"))
                        {
                            body = reader.ReadToEnd();
                            body = body.Replace("@@firstname", jobSeeker.FirstName);
                            body = body.Replace("@@lastname", jobSeeker.LastName);
                            body = body.Replace("@@jobtitle", job.Title);
                            body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, job.PermaLink, job.Id));
                            
                            string redirect = string.Format("{0}://{1}/applications", HttpContext.Current.Request.Url.Scheme,
                                    HttpContext.Current.Request.Url.Authority);

                            body = body.Replace("@@downloadurl", string.Format("{0}://{1}/jobseeker/Download?id={2}&redirect={3}",
                                    HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, jobSeeker.UserId, redirect));
                            
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme,
                                    HttpContext.Current.Request.Url.Authority, jobSeeker.PermaLink));
                            
                            body = body.Replace("@@employer", employer.Company);

                            string[] receipent = { employer.Username };
                            var subject = string.Format("Application for {0}", job.Title);

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                    reader.Close();
                    reader = null;
                    message = string.Format("You have successfully {0} for {1}!", message, job.Title);
                }
                else
                {
                    message = string.Format("You have {0} for {1}!", message, job.Title);
                }
            }
            else if (Shortlisted == null && withdrawn != null)
            {
                var tracking = TrackingService.Instance.Update(TrackingTypes.APPLIED, withdrawn.Id, Username, out message);

                //var tracking = TrackingService.Instance.Application(TrackingTypes.APPLIED, JobId, jobseekerId, Username,
                //    out message);
                var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/Mail/apply.html"));
                var body = string.Empty;

                UserProfile jobSeeker = MemberService.Instance.Get(tracking.JobseekerId.Value);
                Job job = JobService.Instance.Get(tracking.JobId.Value);
                UserProfile employer = MemberService.Instance.Get(job.EmployerId.Value);
                if (!message.Contains("already"))
                {
                    if (reader != null)
                    {
                        body = reader.ReadToEnd();
                        body = body.Replace("@@firstname", jobSeeker.FirstName);
                        body = body.Replace("@@lastname", jobSeeker.LastName);
                        body = body.Replace("@@jobtitle", job.Title);
                        body = body.Replace("@@joburl",
                            string.Format("{0}://{1}/job/{2}-{3}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, job.PermaLink, tracking.JobId));
                        body = body.Replace("@@companyprofile",
                            string.Format("{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, employer.PermaLink));
                        body = body.Replace("@@employer", employer.Company);

                        string[] receipent = { jobSeeker.Username };
                        var subject = string.Format("Application for {0}", job.Title);

                        AlertService.Instance.SendMail(subject, receipent, body);
                    }

                    reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/Mail/employer_apply.html"));
                    body = string.Empty;

                    if (reader != null)
                    {
                        if (!employer.Username.ToLower().Contains("admin"))
                        {
                            body = reader.ReadToEnd();
                            body = body.Replace("@@firstname", jobSeeker.FirstName);
                            body = body.Replace("@@lastname", jobSeeker.LastName);
                            body = body.Replace("@@jobtitle", job.Title);
                            body = body.Replace("@@joburl",
                                string.Format("{0}://{1}/job/{2}-{3}", HttpContext.Current.Request.Url.Scheme,
                                    HttpContext.Current.Request.Url.Authority, job.PermaLink, job.Id));
                            string redirect = string.Format("{0}://{1}/applications", HttpContext.Current.Request.Url.Scheme,
                                    HttpContext.Current.Request.Url.Authority);

                            body = body.Replace("@@downloadurl", string.Format("{0}://{1}/jobseeker/Download?id={2}&redirect={3}",
                                    HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, jobSeeker.UserId, redirect));
                            body = body.Replace("@@profileurl",
                                string.Format("{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme,
                                    HttpContext.Current.Request.Url.Authority, jobSeeker.PermaLink));
                            body = body.Replace("@@employer", employer.Company);

                            string[] receipent = { employer.Username };
                            var subject = string.Format("Application for {0}", job.Title);

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                    reader.Close();
                    reader = null;
                    message = string.Format("You have successfully {0} for {1}!", message, job.Title);
                }
                else
                {
                    message = string.Format("You have {0} for {1}!", message, job.Title);
                }
            }
            else if (Shortlisted != null && withdrawn == null)
            {
                message = "Your application is already shortlisted!";
            }
        }

        public void Withdraw(Guid Id, string Username, out string message)
        {
            var record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, Id, Username, out message);
            if (record.JobseekerId != null)
            {
                var reader =
                    new StreamReader(
                        HttpContext.Current.Server.MapPath("~/Templates/Mail/jobseeker_application_withdrawn.html"));
                var body = string.Empty;
                UserProfile jobSeeker = MemberService.Instance.Get(record.JobseekerId.Value);
                Job job = JobService.Instance.Get(record.JobId.Value);
                UserProfile employer = MemberService.Instance.Get(job.EmployerId.Value);
                if (!message.Contains("already"))
                {
                    if (reader != null)
                    {
                        body = reader.ReadToEnd();
                        body = body.Replace("@@firstname", jobSeeker.FirstName);
                        body = body.Replace("@@lastname", jobSeeker.LastName);
                        body = body.Replace("@@jobtitle", job.Title);
                        body = body.Replace("@@joburl",
                            string.Format("{0}://{1}/job/{2}-{3}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, job.PermaLink, record.JobId));

                        body = body.Replace("@@companyprofile", string.Format("{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, employer.PermaLink));

                        body = body.Replace("@@employer", employer.Company);

                        string[] receipent = { jobSeeker.Username };
                        var subject = string.Format("Application for {0} Job has been Withdrawn!", job.Title);

                        AlertService.Instance.SendMail(subject, receipent, body);
                    }

                    reader =
                        new StreamReader(
                            HttpContext.Current.Server.MapPath("~/Templates/Mail/employer_application_withdrawn.html"));
                    body = string.Empty;

                    if (reader != null)
                    {
                        body = reader.ReadToEnd();
                        body = body.Replace("@@employer", employer.Company);
                        body = body.Replace("@@firstname", jobSeeker.FirstName);
                        body = body.Replace("@@lastname", jobSeeker.LastName);
                        body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, jobSeeker.PermaLink));

                        body = body.Replace("@@jobtitle", job.Title);
                        body = body.Replace("@@joburl",
                            string.Format("{0}://{1}/job/{2}-{3}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, job.PermaLink, record.JobId));

                        string[] receipent = { employer.Username };
                        var subject = string.Format("Application for {0} Job has been Withdrawn!", job.Title);

                        AlertService.Instance.SendMail(subject, receipent, body);
                    }
                    message = string.Format("Application for {0} job has been {1}!", job.Title, message);
                }
                else
                {
                    message = string.Format("Application for {0} job has been {1}!", job.Title, message);
                }
            }
        }

        public static bool IsEven(int value)
        {
            return value % 2 == 0;
        }

        /// <summary>
        /// Automated Search Jobs
        /// </summary>
        /// <param name="jobSeeker"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'AutomatchJob' could not be found (are you missing a using directive or an assembly reference?)
        public List<AutomatchJob> AutomatchedJobs(UserProfile jobSeeker)
#pragma warning restore CS0246 // The type or namespace name 'AutomatchJob' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        {
            var jobs = new List<AutomatchJob>();
            var list_job = new List<Job>();
            /* Compulsory Parameters */
            var title = jobSeeker.Title.Trim().ToLower();
            var category = jobSeeker.CategoryId;
            var specialization = jobSeeker.SpecializationId;
            var country = jobSeeker.CountryId;

            /* Optional Parameters */
            var city = jobSeeker.City;
            var state = jobSeeker.StateId;
            var zip = jobSeeker.Zip;
            var qualificationId = jobSeeker.QualificationId;
            var exp = jobSeeker.Experience;
            var age = jobSeeker.Age;
            var salary = jobSeeker.ExpectedSalary;

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                IQueryable<Job> jobList = (from element in dataHelper.Get<Job>().Where(x => x.IsPublished == true && x.IsDeleted == false)
                        group element by new { element.EmployerId, element.Title } into groups
                        select groups.OrderByDescending(x => x.Id).FirstOrDefault());

                var blockedList = dataHelper.Get<BlockedPeople>().Where(x => x.BlockedId == jobSeeker.UserId).Select(x => x.BlockerId);

                if (blockedList.Any())
                {
                    jobList = jobList.Where(x => !blockedList.Contains(x.Employer.UserId));
                }

                var keyword_list = new List<string>();
                List<Parameter> tparameters = new List<Parameter>();

                string first = string.Empty;
                string second = string.Empty;
                if (title.Trim().Length > 0)
                {
                    keyword_list = title.Trim().ToLower().Split(' ').ToList();

                    if (keyword_list.Count > 1)
                    {
                        for (int i = 0; i < keyword_list.Count; i++)
                        {
                            first = keyword_list[i];
                            for (int j = 0; j < keyword_list.Count; j++)
                            {
                                if (!first.Equals(keyword_list[j]))
                                {
                                    second = string.Format("{0} {1}", first, keyword_list[j]);

                                    Parameter pSecond = new Parameter()
                                    {
                                        Name = "Title",
                                        Value = second
                                    };
                                    tparameters.Add(pSecond);
                                }
                            }
                        }
                        jobList = jobList.Where(dataHelper.BuildExpression<Job>(tparameters)).Where(x => x.CategoryId == category
                        && x.SpecializationId == specialization && x.CountryId == country);
                    }
                    else
                    {
                        Parameter pfirst = new Parameter()
                        {
                            Name = "Title",
                            Value = keyword_list[0]
                        };
                        tparameters.Add(pfirst);
                        jobList = jobList.Where(dataHelper.BuildExpression<Job>(tparameters)).Where(x => x.CategoryId == category
                        && x.SpecializationId == specialization && x.CountryId == country);
                    }

                    //jobList = jobList.Where(x => x.CategoryId == category
                    //    && x.SpecializationId == specialization && x.CountryId == country);
                }
                int matched = (int)TrackingTypes.AUTO_MATCHED;

                var matchedList = dataHelper.Get<Tracking>().Where(x => x.JobseekerId == jobSeeker.UserId && x.JobId != null && x.Type == matched && x.IsDeleted == false).Select(x => x.JobId).ToList();
                if (matchedList.Count > 0)
                {
                    list_job = jobList.Where(x => !matchedList.Contains(x.Id)).ToList();
                }
                else
                {
                    list_job = jobList.ToList();
                }

                if (list_job.Count > 0)
                {
                    list_job = list_job.Where(x => x.IsExpired.Value == false).ToList();
                }            
                jobs = list_job.Select(x => new AutomatchJob { Job = x, Percentage = 33.33M })
                        .OrderByDescending(x => x.Job.PublishedDate).ToList();

                for (var idx = 0; idx < jobs.Count; idx++)
                {
                    var entity = jobs[idx].Job;
                    if (entity.StateId == jobSeeker.StateId)
                    {
                        jobs[idx].Percentage = 41.67M;
                    }
                }

                for (var idx = 0; idx < jobs.Count; idx++)
                {
                    var entity = jobs[idx].Job;
                    entity.City = (entity.City != null ? entity.City : "");
                    if (entity.City.Equals(jobSeeker.City) && entity.StateId == jobSeeker.StateId)
                    {
                        jobs[idx].Percentage = 50.00M;
                    }
                }

                for (var idx = 0; idx < jobs.Count; idx++)
                {
                    var entity = jobs[idx].Job;
                    entity.City = (entity.City != null ? entity.City : "");
                    if (entity.City.Equals(jobSeeker.City) && entity.StateId == jobSeeker.StateId &&
                        entity.Zip == jobSeeker.Zip)
                    {
                        jobs[idx].Percentage = 58.33M;
                    }
                }

                for (var idx = 0; idx < jobs.Count; idx++)
                {
                    var entity = jobs[idx].Job;
                    entity.City = (entity.City != null ? entity.City : "");
                    if (entity.City.Equals(jobSeeker.City) && entity.StateId == jobSeeker.StateId &&
                        entity.Zip == jobSeeker.Zip && entity.QualificationId == jobSeeker.QualificationId)
                    {
                        jobs[idx].Percentage = 66.67M;
                    }
                }

                for (var idx = 0; idx < jobs.Count; idx++)
                {
                    var entity = jobs[idx].Job;
                    if (entity.StateId == jobSeeker.StateId && entity.City == jobSeeker.City &&
                        entity.Zip == jobSeeker.Zip && entity.QualificationId == jobSeeker.QualificationId
                        &&
                        (jobSeeker.Experience >= entity.MinimumExperience &&
                         jobSeeker.Experience <= entity.MaximumExperience))
                    {
                        jobs[idx].Percentage = 75.00M;
                    }
                }

                for (var idx = 0; idx < jobs.Count; idx++)
                {
                    var entity = jobs[idx].Job;
                    if (entity.StateId == jobSeeker.StateId && entity.City == jobSeeker.City &&
                        entity.Zip == jobSeeker.Zip && entity.QualificationId == jobSeeker.QualificationId
                        &&
                        (jobSeeker.Experience >= entity.MinimumExperience &&
                         jobSeeker.Experience <= entity.MaximumExperience)
                        && (jobSeeker.Age >= entity.MinimumAge && jobSeeker.Age <= entity.MaximumAge))
                    {
                        jobs[idx].Percentage = 83.33M;
                    }
                }

                for (var idx = 0; idx < jobs.Count; idx++)
                {
                    var entity = jobs[idx].Job;
                    if (entity.StateId == jobSeeker.StateId && entity.City == jobSeeker.City &&
                        entity.Zip == jobSeeker.Zip && entity.QualificationId == jobSeeker.QualificationId
                        &&
                        (jobSeeker.Experience >= entity.MinimumExperience &&
                         jobSeeker.Experience <= entity.MaximumExperience)
                        && (jobSeeker.Age >= entity.MinimumAge && jobSeeker.Age <= entity.MaximumAge)
                        && (jobSeeker.ExpectedSalary >= entity.MinimumSalary &
                         jobSeeker.ExpectedSalary <= entity.MaximumSalary))
                    {
                        jobs[idx].Percentage = 91.67M;
                    }
                }

                for (var idx = 0; idx < jobs.Count; idx++)
                {
                    var entity = jobs[idx].Job;
                    if (entity.StateId == jobSeeker.StateId && entity.City == jobSeeker.City &&
                        entity.Zip == jobSeeker.Zip && entity.QualificationId == jobSeeker.QualificationId
                        &&
                        (jobSeeker.Experience >= entity.MinimumExperience &&
                         jobSeeker.Experience <= entity.MaximumExperience)
                        && (jobSeeker.Age >= entity.MinimumAge && jobSeeker.Age <= entity.MaximumAge)
                        &&
                        (jobSeeker.ExpectedSalary >= entity.MinimumSalary &
                         jobSeeker.ExpectedSalary <= entity.MaximumSalary) &&
                        entity.EmploymentTypeId == jobSeeker.EmploymentTypeId)
                    {
                        jobs[idx].Percentage = 100M;
                    }
                }

                var message = "";
                for (var idx = 0; idx < jobs.Count; idx++)
                {
                    var jobid = jobs[idx].Job.Id;
                    Job job = dataHelper.GetSingle<Job>("Id", jobid);
                    BlockedPeople blocked = null;
                    if (jobs[idx].Job.EmployerId != null)
                    {
                        Hashtable parameters = new Hashtable();
                        parameters.Add("BlockedId", jobs[idx].Job.EmployerId);
                        parameters.Add("BlockerId", jobSeeker.UserId);

                        blocked = dataHelper.GetSingle<BlockedPeople>(parameters);
                    }

                    if (blocked == null)
                    {
                        var tracking = TrackingService.Instance.AutoMatch(AutomatchTypes.JOB, jobSeeker.UserId, jobid, jobs[idx].Percentage, jobSeeker.Username, out message, jobSeeker.Title);
                    }
                }
            }
            return jobs.OrderByDescending(x => x.Job.PublishedDate).ToList();
        }

#pragma warning disable CS0246 // The type or namespace name 'ExtendedTrackingEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<ExtendedTrackingEntity> JobApplicationList(long? jobId, long? jobseekerId, long? employerId, string jobTitle = null, string company = null, DateTime? start = null, DateTime? end = null, int pageNumber = 0, int pageSize = 10)
#pragma warning restore CS0246 // The type or namespace name 'ExtendedTrackingEntity' could not be found (are you missing a using directive or an assembly reference?)
        {            
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@JobId", jobId));
            parameters.Add(new SqlParameter("@JobseekerId", jobseekerId));
            parameters.Add(new SqlParameter("@EmployerId", employerId));         
            parameters.Add(new SqlParameter("@JobTitle", jobTitle));
            parameters.Add(new SqlParameter("@Company", company));
            parameters.Add(new SqlParameter("@Start", start));
            parameters.Add(new SqlParameter("@End", end));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));
            parameters.Add(new SqlParameter("@PageSize", pageSize));

            return ReadData<ExtendedTrackingEntity>("JobApplicationList", parameters);
        }
    }
}