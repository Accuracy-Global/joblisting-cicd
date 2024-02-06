using System.Collections.Generic;
using System.Linq;
#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)

namespace JobPortal.Domain
{
    public class AdministratorService
    {
        private static volatile AdministratorService instance;
        private static readonly object sync = new object();

        /// <summary>
        ///     Default private constructor.
        /// </summary>
        private AdministratorService()
        {
        }

        /// <summary>
        ///     Single Instance of AdministratorService
        /// </summary>
        public static AdministratorService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new AdministratorService();
                        }
                    }
                }
                return instance;
            }
        }


        public int GetApplicationCount(long Id)
        {
            int counts = 0;
            var job = JobService.Instance.Get(Id);
            UserProfile profile = MemberService.Instance.Get(job.EmployerId.Value);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var TypeList = new List<int>();
                TypeList.Add((int)TrackingTypes.AUTO_MATCHED);
                TypeList.Add((int)TrackingTypes.APPLIED);
                counts = dataHelper.Get<Tracking>().Count(x => x.JobId == Id && TypeList.Contains(x.Type) && x.JobseekerId != null && x.IsDeleted == false);
            }
            return counts;
        }

        public int GetInterviewCount(long Id)
        {
            int counts = 0;
            var job = JobService.Instance.Get(Id);
            UserProfile profile = MemberService.Instance.Get(job.EmployerId.Value);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var TypeList = new List<int>();
                TypeList.Add((int)TrackingTypes.INTERVIEW_IN_PROGRESS);
                counts = dataHelper.Get<Tracking>().Count(x => x.JobId == Id && TypeList.Contains(x.Type) && x.JobseekerId != null && x.IsDeleted == false);
            }
            return counts;
        }

        public int GetJobViewedCount(long Id)
        {
            int counts = 0;
            var job = JobService.Instance.Get(Id);
            UserProfile profile = MemberService.Instance.Get(job.EmployerId.Value);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var TypeList = new List<int>();

                TypeList.Add((int)TrackingTypes.VIEWED);
                counts = dataHelper.Get<Tracking>().Count(x => x.JobseekerId != null && x.JobId == Id && TypeList.Contains(x.Type) && x.IsDeleted == false);
            }
            return counts;
        }

#pragma warning disable CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SearchJob' could not be found (are you missing a using directive or an assembly reference?)
        public IEnumerable<Job> Search(SearchJob model, out int records, int offset = 0, int rows = 20)
#pragma warning restore CS0246 // The type or namespace name 'SearchJob' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        {
            records = 0;
            List<Job> list = new List<Job>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                var joblist = dataHelper.Get<Job>();
                if (!string.IsNullOrEmpty(model.Title))
                {
                    joblist = joblist.Where(x => x.Title.Contains(model.Title));
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
                    joblist =
                        joblist.Where(
                            x =>
                                ((x.StateId != null) ? x.State.Text + "" + x.City : x.City).ToLower()
                                    .Contains(model.StateOrCity.ToLower()));
                }

                if (!string.IsNullOrEmpty(model.Zip))
                {
                    joblist = joblist.Where(x => x.Zip.Equals(model.Zip));
                }

                if (model.EmploymentType != null)
                {
                    joblist = joblist.Where(x => x.EmploymentTypeId == model.EmploymentType);
                }
                if (model.QualificationId != null)
                {
                    joblist = joblist.Where(x => x.QualificationId == model.QualificationId);
                }

                list = joblist.ToList();
                if (model.StartDate != null && model.EndDate != null)
                {
                    list = list.Where(x => x.PublishedDate.Date >= model.StartDate.Value.Date).ToList();
                }

                if (model.StartDate != null && model.EndDate != null)
                {
                    list = list.Where(x => x.PublishedDate.Date <= model.EndDate.Value.Date).ToList();
                }
                records = list.Count();
                list = list.OrderByDescending(x => x.PublishedDate).Skip(offset*rows).Take(rows).ToList();
            }
            return list;
        }

        public string GetUserStatus(long Id, out string toolTip)
        {
            string stat = "#fff";
            toolTip = string.Empty;
            UserProfile profile = MemberService.Instance.Get(Id);
            LoginHistory last = DomainService.Instance.GetLastLoginHistory(profile.Username);
            
            if (last != null && !string.IsNullOrEmpty(last.IPAddress) && !last.IPAddress.Equals("::1"))
            {
                LocationEntity location = DomainService.Instance.GetLocation(last.IPAddress);
                if (location != null)
                {
                    if (profile.CountryId == location.CountryId)
                    {
                        stat = "#87D42E";
                        toolTip = "Normal";
                    }
                    else
                    {
                        stat = "red";
                        toolTip = "Cautious";
                    }
                }
            }
            return stat;
        }
    }
}