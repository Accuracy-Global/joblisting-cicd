using System.Collections.Generic;
using System.Linq;
#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using NPOI.OpenXmlFormats.Dml;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Collections;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace JobPortal.Domain
{
#pragma warning disable CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    public class JobService : DataService
#pragma warning restore CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    {
        private static volatile JobService instance;
        private static readonly object sync = new object();

        /// <summary>
        /// Default private constructor.
        /// </summary>
        private JobService()
        {
        }

        /// <summary>
        /// Single Instance of Job Service
        /// </summary>
        public static JobService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new JobService();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Gets the list of jobs
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        public List<Job> GetList(long Id)
#pragma warning restore CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Job> list = new List<Job>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                list = context.Jobs.Where(x => x.EmployerId == Id
                    && x.IsPublished == true && x.IsActive == true && x.IsDeleted == false
                    && x.IsExpired.Value == false).ToList();
            }
            return list;
        }
        public bool IsAutomatched(long jobId, long jobseekeId)
        {
            var automatched = (int)TrackingTypes.AUTO_MATCHED;
            bool flag = false;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var record = dataHelper.Get<Tracking>().Where(x => x.JobId == jobId && x.JobseekerId == jobseekeId && x.Type == automatched && x.IsDeleted == false)
                    .OrderByDescending(x => x.DateUpdated).FirstOrDefault();
                flag = (record != null);
            }
            return flag;
        }

        public bool IsApplied(long jobId, long jobseekerId)
        {
            bool flag = false;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                var applied = (int)TrackingTypes.APPLIED;
                var record = dataHelper.Get<Tracking>().Where(x => x.JobId == jobId && x.JobseekerId == jobseekerId && x.Type == applied).OrderByDescending(x => x.DateUpdated).FirstOrDefault();
                flag = (record != null);
            }
            return flag;
        }

#pragma warning disable CS0246 // The type or namespace name 'BookmarkedTypes' could not be found (are you missing a using directive or an assembly reference?)
        public bool IsBookmarked(BookmarkedTypes type, long Id, long? jobId)
#pragma warning restore CS0246 // The type or namespace name 'BookmarkedTypes' could not be found (are you missing a using directive or an assembly reference?)
        {
            bool flag = false;
            var bookmarked = (int)TrackingTypes.BOOKMAKRED;
            Tracking record = new Tracking();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                switch (type)
                {
                    case BookmarkedTypes.JOBSEEKER:
                        record = dataHelper.Get<Tracking>().Where(x => x.JobseekerId == Id && x.ResumeId == null && x.JobId == jobId && x.Type == bookmarked)
                       .OrderByDescending(x => x.DateUpdated).FirstOrDefault();
                        flag = (record != null);
                        break;
                    case BookmarkedTypes.RESUME:
                        record = dataHelper.Get<Tracking>().Where(x => x.JobseekerId == null && x.ResumeId == Id && x.JobId == jobId && x.Type == bookmarked)
                       .OrderByDescending(x => x.DateUpdated).FirstOrDefault();
                        flag = (record != null);
                        break;
                }
            }
            return flag;
        }

        public bool IsBookmarked(long jobId, long jobseekerId)
        {
            var bookmarked = (int)TrackingTypes.BOOKMAKRED;

            bool flag = false;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                var record = dataHelper.Get<Tracking>().Where(x => x.JobId == jobId && x.JobseekerId == jobseekerId && x.Type == bookmarked)
                .OrderByDescending(x => x.DateUpdated).FirstOrDefault();
                flag = (record != null);
            }
            return flag;
        }

#pragma warning disable CS0246 // The type or namespace name 'ShortlistTypes' could not be found (are you missing a using directive or an assembly reference?)
        public bool IsShortlisted(ShortlistTypes type, long Id, long? jobId)
#pragma warning restore CS0246 // The type or namespace name 'ShortlistTypes' could not be found (are you missing a using directive or an assembly reference?)
        {
            var shortlisted = (int)TrackingTypes.SHORTLISTED;
            Tracking record = new Tracking();
            bool flag = false;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                switch (type)
                {
                    case ShortlistTypes.JOBSEEKER:
                        record = dataHelper.Get<Tracking>().Where(x => x.JobseekerId == Id && x.ResumeId == null && x.JobId == jobId && x.Type == shortlisted)
                        .OrderByDescending(x => x.DateUpdated).FirstOrDefault();
                        flag = (record != null);
                        break;
                    case ShortlistTypes.RESUME:
                        record = dataHelper.Get<Tracking>().Where(x => x.JobseekerId == Id && x.ResumeId == null && x.JobId == jobId && x.Type == shortlisted)
                        .OrderByDescending(x => x.DateUpdated).FirstOrDefault();
                        flag = (record != null);
                        break;
                }
            }
            return flag;
        }

        public bool IsShortlisted(long jobId, long jobseekerId)
        {
            var shortlisted = (int)TrackingTypes.SHORTLISTED;

            bool flag = false;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                var record = dataHelper.Get<Tracking>().Where(x => x.JobId == jobId && x.JobseekerId == jobseekerId && x.Type == shortlisted)
                .OrderByDescending(x => x.DateUpdated).FirstOrDefault();
                flag = (record != null);
            }
            return flag;
        }

        public int CountJobsByCountry(long Id)
        {
            int jobs = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                jobs = dataHelper.Get<Job>().Where(x => x.IsActive == true && x.IsDeleted == false && x.IsPublished == true && x.CountryId == Id)
                    .ToList().Count(x => x.IsExpired.Value == false);
            }
            return jobs;
        }

        /// <summary>
        /// Search jobs as per the specified criteria.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <param name="records">Number of Records</param>
        /// <param name="offset">Page Number</param>
        /// <param name="rows">Rows Per Page</param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SearchJob' could not be found (are you missing a using directive or an assembly reference?)
        public IEnumerable<Job> Search(SearchJob model, string username, out int records, int offset = 0, int rows = 20)
#pragma warning restore CS0246 // The type or namespace name 'SearchJob' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        {
            records = 0;
            IEnumerable<Job> result = null;
            //int applied = (int)TrackingTypes.APPLIED;
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter() { Name = "IsDeleted", Value = false, Comparison = ParameterComparisonTypes.Equals });
            parameters.Add(new Parameter() { Name = "IsActive", Value = true, Comparison = ParameterComparisonTypes.Equals });
            parameters.Add(new Parameter() { Name = "IsPublished", Value = true, Comparison = ParameterComparisonTypes.Equals });

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var joblist = dataHelper.Get<Job>(parameters);
                //var jobApplied = new List<Tracking>();

                if (!string.IsNullOrEmpty(username))
                {
                    UserProfile profile = MemberService.Instance.Get(username);
                    //jobApplied = dataHelper.Get<Tracking>().Where(x => x.Type == applied && x.JobseekerId == profile.UserId && x.IsDeleted==false && x.JobId != null).ToList();
                    var blockedList = dataHelper.Get<BlockedPeople>().Where(x => x.BlockedId == profile.UserId).Select(x => x.BlockerId).ToList();
                    joblist = joblist.Where(x => x.EmployerId != null && !blockedList.Contains(x.EmployerId.Value));
                }
                //if (jobApplied.Count > 0)
                //{
                //    joblist = joblist.Where(x => jobApplied.Any(z => z.JobId.Value != x.Id));
                //}
                if (!string.IsNullOrEmpty(model.Title))
                {
                    var keywords = model.Title.Trim().ToLower().Split(' ').ToList();
                    if (keywords.Count > 0)
                    {
                        joblist = joblist.Where(x => keywords.Any(k => (x.Title + " " + (x.Employer.Company != null ? x.Employer.Company : "")).ToLower().Contains(k)));
                    }
                }

                if (!string.IsNullOrEmpty(model.Where))
                {
                    joblist = joblist.Where(x => (((x.Country != null) ? x.Country.Text + x.Country.Value : "") + ((x.State != null) ? x.State.Text : "") + (x.City != null ? x.City : "")).ToLower().Contains(model.Where.ToLower()));
                }

                if (model.CategoryId != null)
                {
                    joblist = joblist.Where(x => x.CategoryId == model.CategoryId);
                }

                if (model.SpecializationId != null)
                {
                    joblist = joblist.Where(x => x.SpecializationId == model.SpecializationId);
                }

                if (model.CountryId != null)
                {
                    joblist = joblist.Where(x => x.CountryId == model.CountryId);
                }

                if (!string.IsNullOrEmpty(model.StateOrCity))
                {
                    if (model.EmploymentType != null || model.QualificationId != null)
                    {
                        joblist =
                            joblist.Where(
                                x =>
                                    ((x.StateId != null ? x.State.Text : "") + (x.City != null ? x.City : "")).ToLower()
                                        .Contains(model.StateOrCity.ToLower()) && (x.EmploymentTypeId == model.EmploymentType || x.QualificationId == model.QualificationId));
                    }
                    else
                    {
                        joblist =
                            joblist.Where(x => ((x.StateId != null ? x.State.Text : "") + (x.City != null ? x.City : "")).ToLower().Contains(model.StateOrCity.ToLower()));
                    }
                }

                if (!string.IsNullOrEmpty(model.Zip))
                {
                    if (model.EmploymentType != null || model.QualificationId != null)
                    {
                        joblist = joblist.Where(x => x.Zip.Equals(model.Zip) && (x.EmploymentTypeId == model.EmploymentType || x.QualificationId == model.QualificationId));
                    }
                    else
                    {
                        joblist = joblist.Where(x => x.Zip.Equals(model.Zip));
                    }
                }

                List<Job> jobListByType = new List<Job>();
                if (model.EmploymentType != null)
                {
                    jobListByType = joblist.Where(x => x.EmploymentTypeId == model.EmploymentType).ToList();
                }

                if (jobListByType.Count > 0)
                {
                    joblist = joblist.Where(x => x.EmploymentTypeId == model.EmploymentType);
                }

                List<Job> jobListByQualification = new List<Job>();
                if (model.EmploymentType != null)
                {
                    jobListByQualification = joblist.Where(x => x.QualificationId == model.QualificationId).ToList();
                }

                if (jobListByQualification.Count > 0)
                {
                    joblist = joblist.Where(x => x.QualificationId == model.QualificationId);
                }

                result = joblist.AsEnumerable();

                if (model.StartDate != null)
                {
                    result = result.Where(x => x.PublishedDate.Date >= model.StartDate.Value.Date);
                }

                if (model.EndDate != null)
                {
                    result = result.Where(x => x.PublishedDate.Date <= model.EndDate.Value.Date);
                }

                records = result.Count();
                result = result.OrderByDescending(x => x.PublishedDate);

                result = result.Skip((offset > 0 ? (offset - 1) * rows : offset * rows)).Take(rows).ToList();
            }
            return result;
        }

#pragma warning disable CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        public IEnumerable<Job> Search(string keywords, string location, string username, out int records, int offset = 0, int rows = 20)
#pragma warning restore CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        {
            records = 0;
            IEnumerable<Job> result = null;
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter() { Name = "IsDeleted", Value = false, Comparison = ParameterComparisonTypes.Equals });
            parameters.Add(new Parameter() { Name = "IsActive", Value = true, Comparison = ParameterComparisonTypes.Equals });
            parameters.Add(new Parameter() { Name = "IsPublished", Value = true, Comparison = ParameterComparisonTypes.Equals });


            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var joblist = dataHelper.Get<Job>(parameters);

                if (!string.IsNullOrEmpty(username))
                {
                    UserProfile profile = MemberService.Instance.Get(username);
                    List<long> blockedList = dataHelper.Get<BlockedPeople>("BlockedId", profile.UserId).Select(x => x.BlockedId).ToList();
                    joblist = joblist.Where(x => x.EmployerId != null && !blockedList.Contains(x.EmployerId.Value));
                }

                if (!string.IsNullOrEmpty(keywords))
                {
                    var keywordList = new List<string>();
                    if (keywords.Split(' ').Length > 0)
                    {
                        keywordList = keywords.ToLower().Split(' ').ToList();
                    }
                    if (keywordList.Count > 0)
                    {
                        joblist = joblist.Where(x =>
                                    keywordList.Any(k => x.Title.ToLower().Contains(k)) ||
                                    keywordList.Any(k => x.Summary.ToLower().Contains(k)) ||
                                    keywordList.Any(k => x.Employer.Company.Contains(k)));
                    }
                }

                if (!string.IsNullOrEmpty(location))
                {
                    joblist =
                        joblist.Where(x => (x.Country.Text.Equals(location) || x.Country.Value.Equals(location) || ((x.StateId != null) ? x.State.Text : "").Equals(location) || x.City.Equals(location)));
                }

                records = joblist.Count();
                joblist = joblist.OrderByDescending(x => x.PublishedDate);
                result = joblist.Skip(offset).Take(rows).AsEnumerable();
            }
            return result;
        }

#pragma warning disable CS0246 // The type or namespace name 'JobsByCategory' could not be found (are you missing a using directive or an assembly reference?)
        public List<JobsByCategory> GetJobByCategory()
#pragma warning restore CS0246 // The type or namespace name 'JobsByCategory' could not be found (are you missing a using directive or an assembly reference?)
        {
            var list = new List<JobsByCategory>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var specializations = dataHelper.Get<JobPortal.Data.Specialization>().OrderBy(x => x.Name);
                if (specializations.Count() > 0)
                {
                    list = specializations.Select(x =>
                               new JobsByCategory
                               {
                                   Id = x.Id,
                                   Category = x.Name,
                                   PremaLink = x.FullName,
                                   Jobs = GetJobCount(x.Id)
                               })
                            .ToList();
                }
            }
            return list;
        }

        public int GetJobCount(int categoryId)
        {
            int counts = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                counts = dataHelper.Get<Job>().Count(x => x.CategoryId == categoryId && x.IsPublished == true && x.IsDeleted == false && x.IsActive == true);
            }
            return counts;
        }

#pragma warning disable CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        public Job Get(long Id)
#pragma warning restore CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        {
            Job job = new Job();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                job = dataHelper.GetSingle<Job>(Id);
            }
            return job;
        }

#pragma warning disable CS0246 // The type or namespace name 'TrackingStatus' could not be found (are you missing a using directive or an assembly reference?)
        public TrackingStatus ApplyOnJob(long jobId, long jobseekerId)
#pragma warning restore CS0246 // The type or namespace name 'TrackingStatus' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@JobId", jobId));
            parameters.Add(new SqlParameter("@JobseekerId", jobseekerId));

            return ReadSingleData<TrackingStatus>("ApplyOnJob", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'TrackingStatus' could not be found (are you missing a using directive or an assembly reference?)
        public TrackingStatus ReapplyOnJob(Guid Id, long jobId, long jobseekerId)
#pragma warning restore CS0246 // The type or namespace name 'TrackingStatus' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@Id", Id));
            parameters.Add(new SqlParameter("@JobId", jobId));
            parameters.Add(new SqlParameter("@JobseekerId", jobseekerId));

            return ReadSingleData<TrackingStatus>("ReapplyOnJob", parameters);
        }

        /// <summary>
        /// Get top 9 most recommended jobs information.
        /// </summary>
        /// <returns>Returns top 9 jobs.</returns>


#pragma warning disable CS0246 // The type or namespace name 'LatestJob' could not be found (are you missing a using directive or an assembly reference?)
        //        public List<LatestJob> GetLatestJobs()
        //#pragma warning restore CS0246 // The type or namespace name 'LatestJob' could not be found (are you missing a using directive or an assembly reference?)
        //        {
        //            var list = ReadData<LatestJob>("GetLatestJobs");
        //            return list;
        //        }
        public List<LatestJob> GetLatestJobs1()
#pragma warning restore CS0246 // The type or namespace name 'LatestJob' could not be found (are you missing a using directive or an assembly reference?)
        {
            var list1 = ReadData<LatestJob>("GetLatestJobsunion");
            return list1;
        }
        public List<LatestJob> GetLatestJobs21()
#pragma warning restore CS0246 // The type or namespace name 'LatestJob' could not be found (are you missing a using directive or an assembly reference?)
        {
            var list2 = ReadData<LatestJob>("GetLatestJobsunionNew 1,8");
            return list2;
        }

        public List<LatestJob> GetLatestJobs22(long Id)
#pragma warning restore CS0246 // The type or namespace name 'LatestJob' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@JobId", Id));
            return ReadData<LatestJob>("GetLatestJobsunionJOBID", parameters);

        }
        public List<LatestJob> GetLatestJobs24(string state)
#pragma warning restore CS0246 // The type or namespace name 'LatestJob' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@state", state));
            return ReadData<LatestJob>("jobsstate", parameters);

        }

        public List<LatestJob> GetLatestJobs2()
#pragma warning restore CS0246 // The type or namespace name 'LatestJob' could not be found (are you missing a using directive or an assembly reference?)
        {
            var list2 = ReadData<LatestJob>("GetLatestJobsunionNew");
            return list2;
        }

        public List<LatestJob> GetLatestJobs2(int pageNo = 1)
#pragma warning restore CS0246 // The type or namespace name 'LatestJob' could not be found (are you missing a using directive or an assembly reference?)
        {
            //countryName = countryName.Length > 0 ? "," + countryName + ",".ToString() : string.Empty;
            var list2 = ReadData<LatestJob>($"exec GetLatestJobsunionNew2  {pageNo}");


            return list2;
        }

        public List<LatestJob> GetLatestJobs22(string countryName, string location, string company, int pageNo = 1)
#pragma warning restore CS0246 // The type or namespace name 'LatestJob' could not be found (are you missing a using directive or an assembly reference?)
        {

            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@RowFrom", pageNo));
            parameters.Add(new SqlParameter("@company", ""));
            parameters.Add(new SqlParameter("@location", location));
            parameters.Add(new SqlParameter("@Country", countryName));
            return ReadData<LatestJob>("GetLatestJobsunionNew2old", parameters);
        }
        public List<LatestJob> GetLatestJobs221(string countryName, string location, string company, int pageNo = 25)
#pragma warning restore CS0246 // The type or namespace name 'LatestJob' could not be found (are you missing a using directive or an assembly reference?)
        {

            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@RowFrom", pageNo));
            parameters.Add(new SqlParameter("@company", ""));
            parameters.Add(new SqlParameter("@location", location));
            parameters.Add(new SqlParameter("@Country", countryName));
            return ReadData<LatestJob>("GetLatestJobsunionNew2old1", parameters);
        }


        public List<Jobseekers> GetJobSeekers22(int pageNo = 1)
#pragma warning restore CS0246 // The type or namespace name 'LatestJob' could not be found (are you missing a using directive or an assembly reference?)
        {

            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@PageNumber", 0));
            parameters.Add(new SqlParameter("@PageSize", 16));
            return ReadData<Jobseekers>("jobseekerList", parameters);   
        }     
        public List<Latestcompanies> GetCompanies()
#pragma warning restore CS0246 // The type or namespace name 'LatestJob' could not be found (are you missing a using directive or an assembly reference?)
        {
            var list2 = ReadData<Latestcompanies>("GetCompanies");
            return list2;
        }
        public List<LatestJob> GetLatestSearchJob1(string company)
#pragma warning restore CS0246 // The type or namespace name 'Companies' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@Company", company));

            return ReadData<LatestJob>("SearchJobsNew", parameters);

            //var list = ReadData<Companies>("GetCompanies", parameters);
            //return list;
        }
#pragma warning disable CS0246 // The type or namespace name 'Companies' could not be found (are you missing a using directive or an assembly reference?)
        public List<Companies> GetLatestCompanies()
#pragma warning restore CS0246 // The type or namespace name 'Companies' could not be found (are you missing a using directive or an assembly reference?)
        {
            var list = ReadData<Companies>("GetCompanies");
            return list;
        }
#pragma warning disable CS0246 // The type or namespace name 'Student' could not be found (are you missing a using directive or an assembly reference?)
        public List<Student> GetLatestStudent()
#pragma warning restore CS0246 // The type or namespace name 'Student' could not be found (are you missing a using directive or an assembly reference?)
        {
            var list = ReadData<Student>("GetStudentList");
            return list;
        }


#pragma warning disable CS0246 // The type or namespace name 'Companies' could not be found (are you missing a using directive or an assembly reference?)
        public List<Companies> GetLatestCompanies1(string company, string country)
#pragma warning restore CS0246 // The type or namespace name 'Companies' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@Company", company));
            parameters.Add(new SqlParameter("@country", country));

            return ReadData<Companies>("GetCompanies", parameters);

            //var list = ReadData<Companies>("GetCompanies", parameters);
            //return list;
        }
#pragma warning disable CS0246 // The type or namespace name 'RecommendedJob1' could not be found (are you missing a using directive or an assembly reference?)
        //        public List<RecommendedJob1> GetRecommendedJobs1(long Id)
        //#pragma warning restore CS0246 // The type or namespace name 'RecommendedJob1' could not be found (are you missing a using directive or an assembly reference?)
        //        {
        //            List<DbParameter> parameters = new List<DbParameter>();
        //            parameters.Add(new SqlParameter("@UserId", Id));
        //            var list = ReadData<RecommendedJob1>("GetRelatedJobs", parameters);
        //            return list;
        //        }
        public List<LatestJob> GetRecommendedJobs1(long Id)
#pragma warning restore CS0246 // The type or namespace name 'RecommendedJob1' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", Id));
            var list2 = ReadData<LatestJob>("GetLatestJobsunionNew 1,4");

            return list2;
        }
        //        public List<LatestJob> GetLatestJobs2(string countryName = "", string location = "", int pageNo = 1)
        //#pragma warning restore CS0246 // The type or namespace name 'LatestJob' could not be found (are you missing a using directive or an assembly reference?)
        //        {
        //            countryName = countryName.Length > 0 ? "," + countryName + ",".ToString() : string.Empty;
        //            var list2 = ReadData<LatestJob>($"exec GetLatestJobsunionNew2  {pageNo} {countryName} {location}");


        //            return list2;
        //        }

        //        public List<Job> GetReLatestJobs()
        //#pragma warning restore CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        //        {
        //            List<Job> list = new List<Job>();
        //            using (JobPortalEntities context = new JobPortalEntities())
        //            {
        //                list = context.Jobs.ToList();                    
        //            }
        //            return list;
        //        }
    }
}