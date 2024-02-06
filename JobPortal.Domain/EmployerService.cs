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
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Helpers;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Linq.Expressions;
using System.Collections;
using PagedList;
namespace JobPortal.Domain
{
    /// <summary>
    ///     Class for all Employer related operations
    /// </summary>
    public class EmployerService
    {
        private static volatile EmployerService instance;
        private static readonly object sync = new object();

        private EmployerService()
        {
        }

        /// <summary>
        ///     Single Instance of JobPortalService
        /// </summary>
        public static EmployerService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new EmployerService();
                        }
                    }
                }
                return instance;
            }
        }

#pragma warning disable CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        public StaticPagedList<Job> GetJobs(string Username, string type = "", string status = "", long? CountryId = null, string fd = "", string fm = "", string fy = "", string td = "", string tm = "", string ty = "", int page = 0)
#pragma warning restore CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        {
            int PAGE_SIZE = Convert.ToInt32(ConfigService.Instance.GetConfigValue("PageSize"));
            int SKIP = (page > 0 ? (page - 1) * PAGE_SIZE : page * PAGE_SIZE);
            UserProfile employer = MemberService.Instance.Get(Username);
            int rows = 0;
            List<Job> list = new List<Job>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<Job>().Where(x => x.EmployerId == employer.UserId);

                if (!string.IsNullOrEmpty(type))
                {
                    bool flag = type.Equals("Featured");
                    result = result.Where(x => x.IsFeaturedJob == flag);
                }



                if (CountryId != null)
                {
                    result = result.Where(x => x.CountryId == CountryId.Value);
                }

                if ((!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy)) || (!string.IsNullOrEmpty(td) && !string.IsNullOrEmpty(tm) && !string.IsNullOrEmpty(ty)))
                {
                    DateTime? sdt = new DateTime?();
                    DateTime? edt = new DateTime?();
                    if ((!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy)))
                    {
                        string start = string.Format("{0}/{1}/{2}", fm, fd, fy);
                        sdt = Convert.ToDateTime(start);
                    }

                    if ((!string.IsNullOrEmpty(td) && !string.IsNullOrEmpty(tm) && !string.IsNullOrEmpty(ty)))
                    {
                        if ((string.IsNullOrEmpty(fd) && string.IsNullOrEmpty(fm) && string.IsNullOrEmpty(fy)))
                        {
                            sdt = DateTime.Now;
                        }
                        string end = string.Format("{0}/{1}/{2}", tm, td, ty);
                        edt = Convert.ToDateTime(end);
                    }

                    if (sdt == null && edt != null)
                    {
                        string start = string.Format("{0}/{1}/{2}", tm, td, ty);
                        sdt = Convert.ToDateTime(start);
                        edt = Convert.ToDateTime(start);
                    }

                    if (edt == null && sdt != null)
                    {
                        string start = string.Format("{0}/{1}/{2}", fm, fd, fy);
                        sdt = Convert.ToDateTime(start);
                        edt = Convert.ToDateTime(start);
                    }

                    result = result.Where(x => x.PublishedDate.Day >= sdt.Value.Day && x.PublishedDate.Month >= sdt.Value.Month && x.PublishedDate.Year >= sdt.Value.Year);
                    result = result.Where(x => x.PublishedDate.Day <= edt.Value.Day && x.PublishedDate.Month <= edt.Value.Month && x.PublishedDate.Year <= edt.Value.Year);
                }
                if (!string.IsNullOrEmpty(status))
                {
                    if (status.Equals("Deleted"))
                    {
                        result = result.Where(x => x.IsDeleted == true);
                    }
                    else if (status.Equals("Rejected"))
                    {
                        result = result.Where(x => x.IsRejected == true);
                    }
                    else if (status.Equals("Deactivated"))
                    {
                        result = result.Where(x => x.IsActive == false && x.IsPublished == true && x.IsRejected == false && x.IsDeleted == false);
                    }
                    else if (status.Equals("In Approval Processs"))
                    {
                        result = result.Where(x => x.IsPublished == false && x.IsRejected == false && x.IsActive == true && x.IsDeleted == false);
                    }
                    else if (status.Equals("Live"))
                    {
                        result = result.Where(x => x.IsDeleted == false && x.IsPublished == true && x.IsActive == true && x.IsExpired.Value == false);
                    }
                    else if (status.Equals("Expired"))
                    {
                        result = result.Where(x => x.IsDeleted == false && x.IsPublished == true && x.IsActive == true && x.IsExpired.Value == true);
                    }
                }

                rows = result.Count();
                list = result.OrderByDescending(x => x.PublishedDate).Skip(SKIP).Take(PAGE_SIZE).ToList();
            }
            return new StaticPagedList<Job>(list, (page == 0 ? 1 : page), PAGE_SIZE, rows);
        }
        public int GetPublishedJobCount(string username)
        {
            int counts = 0;
            UserProfile profile = MemberService.Instance.Get(username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                counts = dataHelper.Get<Job>().Where(x => x.EmployerId == profile.UserId).ToList()
                    .Where(x => x.IsDeleted == false && x.IsPublished == true && x.IsActive == true && x.IsExpired.Value == false).Count();
            }
            return counts;
        }

        public int GetListedJobCount(string username)
        {
            int counts = 0;
            UserProfile profile = MemberService.Instance.Get(username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                counts = dataHelper.Get<Job>().Count(x => x.EmployerId == profile.UserId);
            }
            return counts;
        }

        public int GetBookmarkCount(string username)
        {
            int counts = 0;
            UserProfile profile = MemberService.Instance.Get(username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                int bookmarked = (int)TrackingTypes.BOOKMAKRED;

                counts = dataHelper.Get<Tracking>().Count(x => x.UserId == profile.UserId && x.Type == bookmarked && x.IsDeleted == false);
            }
            return counts;
        }

        public int GetBookmarkCount(long userId)
        {
            int counts = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                int bookmarked = (int)TrackingTypes.BOOKMAKRED;

                counts = dataHelper.Get<Tracking>().Count(x => x.UserId == userId && x.Type == bookmarked && x.IsDeleted == false);
            }
            return counts;
        }

        public int GetMatchListCount(string username)
        {
            int counts = 0;
            UserProfile profile = MemberService.Instance.Get(username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                int matched = (int)TrackingTypes.AUTO_MATCHED;
                counts = dataHelper.Get<Tracking>().Count(x => x.Job.EmployerId == profile.UserId && x.Type == matched && x.IsDeleted == false);
            }
            return counts;
        }

        public int GetApplicationCount(string username)
        {
            int counts = 0;
            UserProfile profile = MemberService.Instance.Get(username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var TypeList = new List<int>();

                if (profile.Type == (int)SecurityRoles.Employers)
                {
                    TypeList.Add((int)TrackingTypes.AUTO_MATCHED);
                    TypeList.Add((int)TrackingTypes.APPLIED);
                    TypeList.Add((int)TrackingTypes.WITHDRAWN);
                    counts = dataHelper.Get<Tracking>().Count(x => x.Job.EmployerId == profile.UserId && TypeList.Contains(x.Type) && x.IsDeleted == false && x.JobseekerId != null);
                }
                else
                {
                    TypeList.Add((int)TrackingTypes.AUTO_MATCHED);
                    TypeList.Add((int)TrackingTypes.APPLIED);
                    TypeList.Add((int)TrackingTypes.WITHDRAWN);
                    counts = dataHelper.Get<Tracking>().Count(x => x.JobseekerId == profile.UserId && TypeList.Contains(x.Type) && x.IsDeleted == false && x.JobId != null);
                }
            }
            return counts;
        }

        public int GetApplicationCount(long userId)
        {
            int counts = 0;
            UserProfile profile = MemberService.Instance.Get(userId);

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var TypeList = new List<int>();

                if (profile.Type == (int)SecurityRoles.Employers)
                {
                    TypeList.Add((int)TrackingTypes.AUTO_MATCHED);
                    TypeList.Add((int)TrackingTypes.APPLIED);
                    TypeList.Add((int)TrackingTypes.WITHDRAWN);
                    counts = dataHelper.Get<Tracking>().Count(x => x.Job.EmployerId == userId && TypeList.Contains(x.Type) && x.IsDeleted == false && x.JobseekerId != null);
                }
                else
                {
                    TypeList.Add((int)TrackingTypes.AUTO_MATCHED);
                    TypeList.Add((int)TrackingTypes.APPLIED);
                    TypeList.Add((int)TrackingTypes.WITHDRAWN);
                    counts = dataHelper.Get<Tracking>().Count(x => x.JobseekerId == userId && TypeList.Contains(x.Type) && x.IsDeleted == false && x.JobId != null);
                }
            }
            return counts;
        }
        public int GetInterviewCount(string username)
        {
            int counts = 0;
            UserProfile profile = MemberService.Instance.Get(username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                //int inprogress = (int)InterviewStatus.INTERVIEW_IN_PROGRESS;
                //int initiated = (int)InterviewStatus.INITIATED;
                if (profile.Type == (int)SecurityRoles.Employers)
                {
                    counts = dataHelper.Get<Interview>().Count(x => x.Tracking.JobseekerId != null && x.UserId == profile.UserId && x.IsDeleted == false);
                }
                else
                {
                    counts = dataHelper.Get<Interview>().Count(x => x.Tracking.JobseekerId == profile.UserId && x.IsDeleted == false);
                }

            }
            return counts;
        }

        public int GetInterviewCount(long userId)
        {
            int counts = 0;
            UserProfile profile = MemberService.Instance.Get(userId);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                //int inprogress = (int)InterviewStatus.INTERVIEW_IN_PROGRESS;
                //int initiated = (int)InterviewStatus.INITIATED;
                if (profile.Type == (int)SecurityRoles.Employers)
                {
                    counts = dataHelper.Get<Interview>().Count(x => x.Tracking.JobseekerId != null && x.UserId == profile.UserId && x.IsDeleted == false);
                }
                else
                {
                    counts = dataHelper.Get<Interview>().Count(x => x.Tracking.JobseekerId == profile.UserId && x.IsDeleted == false);
                }

            }
            return counts;
        }

        public int GetDownloadCount(string username)
        {
            int counts = 0;
            UserProfile profile = MemberService.Instance.Get(username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                int download = (int)TrackingTypes.DOWNLOADED;
                if (profile.Type == (int)SecurityRoles.Employers)
                {
                    counts = dataHelper.Get<Tracking>().Count(x => x.UserId == profile.UserId && x.Type == download && x.IsDeleted == false);
                }
                else
                {
                    counts = dataHelper.Get<Tracking>().Count(x => x.JobseekerId == profile.UserId && x.Type == download && x.IsDeleted == false);
                }
            }
            return counts;
        }

        public int GetBlockCount(string username)
        {
            int counts = 0;
            UserProfile profile = MemberService.Instance.Get(username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                counts = dataHelper.Get<BlockedPeople>().Count(x => x.BlockerId == profile.UserId);
            }
            return counts;
        }

#pragma warning disable CS0246 // The type or namespace name 'InterviewNote' could not be found (are you missing a using directive or an assembly reference?)
        public List<InterviewNote> GetInterviewNotes(long Id)
#pragma warning restore CS0246 // The type or namespace name 'InterviewNote' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<InterviewNote> notes = new List<InterviewNote>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                // notes = dataHelper.Get<InterviewNote>().Where(x => x.InterviewId == Id && x.IsDeleted == false).OrderByDescending(x => x.DateUpdated).ToList();
            }
            return notes;
        }

#pragma warning disable CS0246 // The type or namespace name 'InterviewDiscussion' could not be found (are you missing a using directive or an assembly reference?)
        public List<InterviewDiscussion> GetInterviewDiscussions(long Id)
#pragma warning restore CS0246 // The type or namespace name 'InterviewDiscussion' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<InterviewDiscussion> discussions = new List<InterviewDiscussion>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                //discussions = dataHelper.Get<InterviewDiscussion>().Where(x => x.InterviewId == Id && x.IsDeleted == false).OrderByDescending(x => x.DateUpdated).ToList();
            }
            return discussions;
        }

        public int GetViewCounts(long JobId)
        {
            var viewed = (int)TrackingTypes.VIEWED;
            int views = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                views = dataHelper.Get<Tracking>().Count(x => x.Type == viewed && x.Job != null && x.JobId.Value == JobId);
            }
            return views;
        }

        public DateTime? GetAppliedDate(long JobId, long ResumeId)
        {
            var applied = (int)TrackingTypes.APPLIED;

            var date = new DateTime?();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                date = dataHelper.Get<Tracking>().Where(x => x.JobId == JobId && x.ResumeId == ResumeId && x.Type == applied).OrderByDescending(x => x.Id)
                        .Select(x => x.DateUpdated).FirstOrDefault();
            }
            return date;
        }

        public DateTime? GetAutomatchedDate(long JobId, long ResumeId)
        {
            var automatched = (int)TrackingTypes.AUTO_MATCHED;

            var date = new DateTime?();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                date = dataHelper.Get<Tracking>().Where(x => x.JobId == JobId && x.ResumeId == ResumeId && x.Type == automatched).OrderByDescending(x => x.Id)
                        .Select(x => x.DateUpdated).FirstOrDefault();
            }
            return date;
        }

#pragma warning disable CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        public Interview Get1stRound(Guid TrackingId)
#pragma warning restore CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        {
            var round = (int)InterviewRounds.FIRST_ROUND;
            Hashtable parameters = new Hashtable();
            parameters.Add("TrackingId", TrackingId);
            parameters.Add("Round", round);
            Interview interview = new Interview();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                interview = dataHelper.GetSingle<Interview>(parameters);
            }
            return interview;
        }

#pragma warning disable CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        public Interview Get2ndRound(Guid TrackingId)
#pragma warning restore CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        {
            var round = (int)InterviewRounds.SECOND_ROUND;
            Hashtable parameters = new Hashtable();
            parameters.Add("TrackingId", TrackingId);
            parameters.Add("Round", round);

            Interview interview = new Interview();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                interview = dataHelper.GetSingle<Interview>(parameters);
            }
            return interview;
        }



        public void Select(Guid Id, string Username, out string message)
        {
            message = string.Empty;
            Interview interview = InterviewService.Instance.Get(Id);
            UserProfile employer = null;
            if (interview != null)
            {
                employer = MemberService.Instance.Get(interview.UserId);
            }

            var record = TrackingService.Instance.Update(TrackingTypes.SELECTED, Id, Username, out message);
            Job job = null;
            if (record.JobId != null)
            {
                job = JobService.Instance.Get(record.JobId.Value);
            }
            UserProfile jobseeker = null;
            if (record.JobseekerId != null)
            {
                jobseeker = MemberService.Instance.Get(record.JobseekerId.Value);
            }

            if (record != null)
            {
                string jTemplate = string.Empty;
                string eTemplate = string.Empty;
                if (jobseeker != null)
                {

                    if (job != null)
                    {
                        jTemplate = HttpContext.Current.Server.MapPath("~/Templates/Mail/to_jobseeker_selected_for_job.html");
                        eTemplate = HttpContext.Current.Server.MapPath("~/Templates/Mail/from_employer_selected_for_job.html");
                    }
                    else
                    {
                        jTemplate = HttpContext.Current.Server.MapPath("~/Templates/Mail/to_jobseeker_selected.html");
                        eTemplate = HttpContext.Current.Server.MapPath("~/Templates/Mail/from_employer_selected.html");
                    }

                    using (var reader = new StreamReader(jTemplate))
                    {
                        var body = string.Empty;
                        body = reader.ReadToEnd();
                        body = body.Replace("@@firstname", jobseeker.FirstName);
                        body = body.Replace("@@lastname", jobseeker.LastName);

                        if (interview != null)
                        {
                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Update?Id={2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, interview.Id));
                        }

                        if (job != null)
                        {
                            body = body.Replace("@@jobtitle", record.Job.Title);
                            body = body.Replace("@@joburl",
                                string.Format("{0}://{1}/job/{2}-{3}", HttpContext.Current.Request.Url.Scheme,
                                    HttpContext.Current.Request.Url.Authority, job.PermaLink, record.JobId));
                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Index", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority));
                        }

                        if (employer != null)
                        {
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, employer.PermaLink));
                            body = body.Replace("@@employer", employer.Company);
                        }

                        string subject = string.Empty;
                        if (job != null)
                        {
                            subject = string.Format("Interview Status for {0} is Selected", job.Title);
                        }
                        else
                        {
                            subject = string.Format("Interview Status is Selected");
                        }
                        string[] receipent = { jobseeker.Username };

                        AlertService.Instance.SendMail(subject, receipent, body);
                    }

                    using (var reader = new StreamReader(eTemplate))
                    {
                        var body = string.Empty;

                        body = reader.ReadToEnd();

                        if (interview != null)
                        {
                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Update?Id={2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, interview.Id));
                        }

                        if (job != null)
                        {
                            body = body.Replace("@@jobtitle", record.Job.Title);
                            body = body.Replace("@@joburl",
                                string.Format("{0}://{1}/job/{2}-{3}", HttpContext.Current.Request.Url.Scheme,
                                    HttpContext.Current.Request.Url.Authority, job.PermaLink, record.JobId));
                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Index", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority));
                        }

                        if (jobseeker != null)
                        {
                            body = body.Replace("@@firstname", jobseeker.FirstName);
                            body = body.Replace("@@lastname", jobseeker.LastName);
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, jobseeker.PermaLink));
                        }
                        if (employer != null)
                        {
                            body = body.Replace("@@employer", employer.Company);
                            string[] receipent = { employer.Username };

                            string subject = string.Empty;
                            if (job != null)
                            {
                                subject = string.Format("Interview Status for {0} is Selected", job.Title);
                            }
                            else
                            {
                                subject = string.Format("Interview Status is Selected");
                            }

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Shortlist resume
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Username"></param>
        /// <param name="message"></param>
        public void Shortlist(Guid Id, string Username, out string message)
        {
            message = string.Empty;
            var profile = MemberService.Instance.Get(Username);

            var record = TrackingService.Instance.Update(TrackingTypes.SHORTLISTED, Id, Username, out message);

            string subject = string.Empty;
            string[] receipent = new string[1];

            if (record.JobseekerId != null && record.ResumeId == null)
            {
                UserProfile jobSeeker = MemberService.Instance.Get(record.JobseekerId.Value);
                if (record.JobId != null)
                {
                    Job job = JobService.Instance.Get(record.JobId.Value);
                    var reader =
                      new StreamReader(
                          HttpContext.Current.Server.MapPath("~/Templates/Mail/jobseeker_shortlisted_for_job.html"));
                    var body = string.Empty;

                    if (reader != null)
                    {
                        body = reader.ReadToEnd();
                        body = body.Replace("@@firstname", jobSeeker.FirstName);
                        body = body.Replace("@@lastname", jobSeeker.LastName);
                        body = body.Replace("@@employer", profile.Company);
                        body = body.Replace("@@jobtitle", job.Title);
                        body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, job.PermaLink, job.Id));
                        body = body.Replace("@@profileurl",
                            string.Format("{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, profile.PermaLink));

                        receipent[0] = record.Jobseeker.Username;

                        subject = string.Format("Application for {0} is Shortlisted!", record.Job.Title);
                        AlertService.Instance.SendMail(subject, receipent, body);
                    }

                    reader =
                       new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/Mail/employer_shortlisted_for_job.html"));
                    body = string.Empty;

                    if (reader != null)
                    {
                        body = reader.ReadToEnd();
                        body = body.Replace("@@employer", profile.Company);
                        body = body.Replace("@@jobtitle", job.Title);
                        body = body.Replace("@@joburl",
                            string.Format("{0}://{1}/job/{2}-{3}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, job.PermaLink, record.Job.Id));

                        body = body.Replace("@@profileurl",
                            string.Format("{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, jobSeeker.PermaLink));

                        receipent[0] = profile.Username;
                        subject = string.Format("Application for {0} is Shortlisted!", job.Title);
                        AlertService.Instance.SendMail(subject, receipent, body);
                    }
                }
                else
                {
                    var reader =
                      new StreamReader(
                          HttpContext.Current.Server.MapPath("~/Templates/Mail/jobseeker_shortlisted.html"));
                    var body = string.Empty;

                    if (reader != null)
                    {
                        body = reader.ReadToEnd();
                        body = body.Replace("@@firstname", jobSeeker.FirstName);
                        body = body.Replace("@@lastname", jobSeeker.LastName);
                        body = body.Replace("@@employer", profile.Company);

                        body = body.Replace("@@profileurl",
                            string.Format("{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, profile.PermaLink));

                        receipent[0] = jobSeeker.Username;

                        subject = string.Format("Profile Shortlisted by {0}", (!string.IsNullOrEmpty(profile.Company) ? profile.Company : profile.FirstName + " " + profile.LastName));
                        AlertService.Instance.SendMail(subject, receipent, body);
                    }

                    reader =
                       new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/Mail/employer_shortlisted.html"));
                    body = string.Empty;

                    if (reader != null)
                    {
                        body = reader.ReadToEnd();
                        body = body.Replace("@@employer", (!string.IsNullOrEmpty(profile.Company) ? profile.Company : profile.FirstName + " " + profile.LastName));
                        body = body.Replace("@@profileurl",
                            string.Format("{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, jobSeeker.PermaLink));

                        receipent[0] = profile.Username;
                        subject = string.Format("Profile of {0} {1} is Shortlisted!", jobSeeker.FirstName, jobSeeker.LastName);
                        AlertService.Instance.SendMail(subject, receipent, body);
                    }
                }

                if (!message.Contains("already"))
                {
                    message = string.Format("{0} profile has been {1} successfully!", jobSeeker.Title, message);
                }
                else
                {
                    message = string.Format("{0} profile has been {1}!", jobSeeker.Title, message);
                }
            }
            else if (record.JobseekerId == null && record.ResumeId != null)
            {
                Resume resume = SharedService.Instance.GetResume(record.ResumeId.Value);
                if (record.JobId != null)
                {
                    Job job = JobService.Instance.Get(record.JobId.Value);
                    var reader =
                        new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/Mail/employer_resume_shortlisted_for_job.html"));
                    var body = string.Empty;

                    if (reader != null)
                    {
                        body = reader.ReadToEnd();
                        body = body.Replace("@@employer", profile.Company);
                        body = body.Replace("@@jobtitle", job.Title);
                        body = body.Replace("@@joburl",
                            string.Format("{0}://{1}/job/{2}-{3}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, job.PermaLink, job.Id));
                        body = body.Replace("@@downloadurl",
                            string.Format("{0}://{1}/jobseeker/download/{2}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, record.ResumeId));

                        receipent[0] = profile.Username;
                        subject = string.Format("Resume Shortlisted for {0}!", job.Title);
                        AlertService.Instance.SendMail(subject, receipent, body);
                    }
                }
                else
                {
                    var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/Mail/employer_resume_shortlisted.html"));
                    var body = string.Empty;

                    if (reader != null)
                    {
                        body = reader.ReadToEnd();
                        body = body.Replace("@@employer", profile.Company);

                        body = body.Replace("@@downloadurl",
                            string.Format("{0}://{1}/jobseeker/download/{2}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, record.ResumeId));

                        receipent[0] = profile.Username;
                        subject = "Resume is Shortlisted";
                        AlertService.Instance.SendMail(subject, receipent, body);
                    }
                }
                if (!message.Contains("already"))
                {
                    message = string.Format("{0} resume has been {1} successfully!", resume.Title, message);
                }
                else
                {
                    message = string.Format("{0} resume has been {1}!", resume.Title, message);
                }
            }
        }

        /// <summary>
        ///     Reject resume
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Username"></param>
        /// <param name="message"></param>
        public void Reject(Guid Id, string Username, out string message)
        {
            message = string.Empty;
            var profile = MemberService.Instance.Get(Username);
            string status = "";
            string type = "";
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var record = dataHelper.GetSingle<Tracking>(Id);
                switch ((TrackingTypes)record.Type)
                {
                    case TrackingTypes.APPLIED:
                        status = "Job Application Status";
                        type = "job application";
                        break;
                    case TrackingTypes.AUTO_MATCHED:
                        status = "Status of Matched Profile for Job";
                        type = "matched profile";
                        break;
                }
                record.Type = (int)TrackingTypes.REJECTED;
                dataHelper.Update(record);

                Job job = JobService.Instance.Get(record.JobId.Value);
                UserProfile employer = new UserProfile();
                if (job != null)
                {
                    employer = MemberService.Instance.Get(job.EmployerId.Value);
                }
                if (record.JobseekerId != null)
                {
                    UserProfile jobSeeker = MemberService.Instance.Get(record.JobseekerId.Value);
                    message = string.Format("Jobseeker {0} successfully!", message);
                    string eTemplate = string.Empty;
                    string jTemplate = string.Empty;

                    if (record.JobId != null)
                    {
                        jTemplate = HttpContext.Current.Server.MapPath("~/Templates/Mail/jobseeker_rejected_by_employer_for_job.html");
                    }
                    else
                    {
                        jTemplate = HttpContext.Current.Server.MapPath("~/Templates/Mail/jobseeker_rejected_by_employer.html");
                    }
                    using (var reader = new StreamReader(jTemplate))
                    {
                        string body = reader.ReadToEnd();
                        string subject = string.Empty;
                        body = body.Replace("@@firstname", jobSeeker.FirstName);
                        body = body.Replace("@@lastname", jobSeeker.LastName);
                        body = body.Replace("@@type", type);
                        if (employer != null)
                        {
                            body = body.Replace("@@employer", employer.Company);
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, employer.PermaLink));
                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Jobseeker/Index", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority));
                            //body = body.Replace("@@viewurl", string.Format("{0}://{1}/Employer/Update?Id={2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, interview.Id));
                        }

                        if (employer != null)
                        {
                            body = body.Replace("@@employer", employer.Company);
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.Url.Authority, employer.PermaLink));

                            body = body.Replace("@@jobtitle", job.Title);
                            body = body.Replace("@@joburl",
                                string.Format("{0}://{1}/job/{2}-{3}", HttpContext.Current.Request.Url.Scheme,
                                    HttpContext.Current.Request.Url.Authority, job.PermaLink, job.Id));

                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Jobseeker/Index", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority));
                            subject = string.Format("{0}", status);
                        }
                        else
                        {
                            subject = status;
                        }

                        string[] receipent = { jobSeeker.Username };
                        AlertService.Instance.SendMail(subject, receipent, body);
                    }

                    if (record.JobId != null)
                    {
                        eTemplate = HttpContext.Current.Server.MapPath("~/Templates/Mail/employer_has_rejected_jobseeker_for_job.html");
                    }
                    else
                    {
                        eTemplate = HttpContext.Current.Server.MapPath("~/Templates/Mail/employer_has_rejected_jobseekerd.html");
                    }

                    using (var reader = new StreamReader(eTemplate))
                    {
                        string body = reader.ReadToEnd();
                        string subject = string.Empty;

                        if (employer != null)
                        {
                            body = body.Replace("@@employer", employer.Company);
                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Employer/Applications", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority));
                            //employer = interview.UserProfile;
                        }

                        if (job != null)
                        {
                            body = body.Replace("@@employer", employer.Company);
                            body = body.Replace("@@jobtitle", job.Title);
                            body = body.Replace("@@joburl",
                                string.Format("{0}://{1}/job/{2}-{3}", HttpContext.Current.Request.Url.Scheme,
                                    HttpContext.Current.Request.Url.Authority, job.PermaLink, job.Id));

                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Employer/Applications", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority));
                            subject = string.Format("{0}", status);
                            employer = record.Job.Employer;
                        }
                        else
                        {
                            subject = string.Format("{0}", status);
                        }

                        body = body.Replace("@@firstname", jobSeeker.FirstName);
                        body = body.Replace("@@lastname", jobSeeker.LastName);
                        body = body.Replace("@@type", type.TitleCase());
                        body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, jobSeeker.PermaLink));

                        string[] receipent = { employer.Username };
                        AlertService.Instance.SendMail(subject, receipent, body);
                    }

                    if (!message.Contains("already"))
                    {
                        message = string.Format("Application for {0} Job has been rejected!", job.Title);
                    }
                    else
                    {
                        message = string.Format("Application has been {0} Job rejected!", job.Title);
                    }
                }
            }
        }

        /// <summary>
        ///     Automated Search Jobs
        /// </summary>
        /// <param name="jobSeeker"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'AutomatchJobseeker' could not be found (are you missing a using directive or an assembly reference?)
        public List<AutomatchJobseeker> AutomatchedJobseekers(Job job, string Username)
#pragma warning restore CS0246 // The type or namespace name 'AutomatchJobseeker' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        {
            var jobSeekerList = new List<UserProfile>();
            int jobSeeker = (int)SecurityRoles.Jobseeker;

            var automatchedJobSeekers = new List<AutomatchJobseeker>();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                IQueryable<UserProfile> result = dataHelper.Get<UserProfile>().Where(x => x.Type == jobSeeker && x.IsDeleted == false && x.IsActive == true && x.IsConfirmed == true);

                var title = job.Title.Trim().ToLower();
                var category = job.CategoryId;
                var specialization = job.SpecializationId;
                var country = job.CountryId;
                var city = job.City;

                var keywords = job.Title.Trim().ToLower().Split(' ').ToList();
                string first = string.Empty;
                string second = string.Empty;
                List<Parameter> tparameters = new List<Parameter>();
                result = result.Where(x => x.Title != null && x.Content != null && x.CountryId != null && x.CountryId == country && x.CategoryId != null && x.CategoryId == category && x.SpecializationId != null && x.SpecializationId == specialization);

                var keyword_list = new List<string>();
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
                        result = result.Where(dataHelper.BuildExpression<UserProfile>(tparameters)).Where(x => x.Title != null && x.Content != null && x.CountryId != null && x.CountryId == country && x.CategoryId != null && x.CategoryId == category && x.SpecializationId != null && x.SpecializationId == specialization);
                    }
                    else
                    {
                        Parameter pfirst = new Parameter()
                        {
                            Name = "Title",
                            Value = keyword_list[0]
                        };
                        tparameters.Add(pfirst);

                        result = result.Where(dataHelper.BuildExpression<UserProfile>(tparameters)).Where(x => x.Title != null && x.Content != null && x.CountryId != null && x.CountryId == country && x.CategoryId != null && x.CategoryId == category && x.SpecializationId != null && x.SpecializationId == specialization);
                    }
                }

                int matched = (int)TrackingTypes.AUTO_MATCHED;
                var matchedList = dataHelper.Get<Tracking>().Where(x => x.JobseekerId != null && x.JobId == job.Id && x.Type == matched && x.IsDeleted == false).Select(x => x.JobseekerId);

                result = result.Where(x => !matchedList.Contains(x.UserId));
                jobSeekerList = result.ToList();

                automatchedJobSeekers.AddRange(
                    jobSeekerList.Select(x => new AutomatchJobseeker { Job = job, Jobseeker = x, Percentage = 33.33M }).ToList());

                for (var idx = 0; idx < automatchedJobSeekers.Count; idx++)
                {
                    var entity = automatchedJobSeekers[idx].Jobseeker;
                    if (entity.StateId == job.StateId)
                    {
                        automatchedJobSeekers[idx].Percentage = 41.67M;
                    }
                }

                for (var idx = 0; idx < automatchedJobSeekers.Count; idx++)
                {
                    var entity = automatchedJobSeekers[idx].Jobseeker;
                    entity.City = (entity.City != null ? entity.City : "");
                    if (entity.City.Equals(job.City) && entity.StateId == job.StateId)
                    {
                        automatchedJobSeekers[idx].Percentage = 50.00M;
                    }
                }

                for (var idx = 0; idx < automatchedJobSeekers.Count; idx++)
                {
                    var entity = automatchedJobSeekers[idx].Jobseeker;
                    if (entity.StateId == job.StateId && entity.City == job.City && entity.Zip == job.Zip)
                    {
                        automatchedJobSeekers[idx].Percentage = 58.33M;
                    }
                }

                for (var idx = 0; idx < automatchedJobSeekers.Count; idx++)
                {
                    var entity = automatchedJobSeekers[idx].Jobseeker;
                    if (entity.StateId == job.StateId && entity.City == job.City && entity.Zip == job.Zip &&
                        entity.QualificationId == job.QualificationId)
                    {
                        automatchedJobSeekers[idx].Percentage = 66.67M;
                    }
                }

                for (var idx = 0; idx < automatchedJobSeekers.Count; idx++)
                {
                    var entity = automatchedJobSeekers[idx].Jobseeker;
                    if (entity.StateId == job.StateId && entity.City == job.City && entity.Zip == job.Zip &&
                        entity.QualificationId == job.QualificationId
                        && (entity.Experience >= job.MinimumExperience && entity.Experience <= job.MaximumExperience))
                    {
                        automatchedJobSeekers[idx].Percentage = 75.00M;
                    }
                }

                for (var idx = 0; idx < automatchedJobSeekers.Count; idx++)
                {
                    var entity = automatchedJobSeekers[idx].Jobseeker;
                    if (entity.StateId == job.StateId && entity.City == job.City && entity.Zip == job.Zip &&
                        entity.QualificationId == job.QualificationId
                        && (entity.Experience >= job.MinimumExperience && entity.Experience <= job.MaximumExperience)
                        && (entity.Age >= job.MinimumAge && entity.Age <= job.MaximumAge))
                    {
                        automatchedJobSeekers[idx].Percentage = 83.33M;
                    }
                }

                for (var idx = 0; idx < automatchedJobSeekers.Count; idx++)
                {
                    var entity = automatchedJobSeekers[idx].Jobseeker;
                    if (entity.StateId == job.StateId && entity.City == job.City && entity.Zip == job.Zip &&
                        entity.QualificationId == job.QualificationId
                        && (entity.Experience >= job.MinimumExperience && entity.Experience <= job.MaximumExperience)
                        && (entity.Age >= job.MinimumAge && entity.Age <= job.MaximumAge)
                        && (entity.ExpectedSalary >= job.MinimumSalary && entity.ExpectedSalary <= job.MaximumSalary))
                    {
                        automatchedJobSeekers[idx].Percentage = 91.67M;
                    }
                }

                for (var idx = 0; idx < automatchedJobSeekers.Count; idx++)
                {
                    var entity = automatchedJobSeekers[idx].Jobseeker;
                    if (entity.StateId == job.StateId && entity.City == job.City && entity.Zip == job.Zip &&
                        entity.QualificationId == job.QualificationId
                        && (entity.Experience >= job.MinimumExperience && entity.Experience <= job.MaximumExperience)
                        && (entity.Age >= job.MinimumAge && entity.Age <= job.MaximumAge)
                        && (entity.ExpectedSalary >= job.MinimumSalary && entity.ExpectedSalary <= job.MaximumSalary) &&
                        entity.EmploymentTypeId == job.EmploymentTypeId)
                    {
                        automatchedJobSeekers[idx].Percentage = 100M;
                    }
                }

                var message = "";
                for (var idx = 0; idx < automatchedJobSeekers.Count; idx++)
                {
                    if (automatchedJobSeekers[idx].Jobseeker != null)
                    {
                        var jobSeekerId = automatchedJobSeekers[idx].Jobseeker.UserId;
                        BlockedPeople blocked = null;
                        if (job.EmployerId != null)
                        {
                            Hashtable parameters = new Hashtable();
                            parameters.Add("BlockedId", job.EmployerId);
                            parameters.Add("BlockerId", jobSeekerId);

                            blocked = dataHelper.GetSingle<BlockedPeople>(parameters);
                        }

                        if (blocked == null)
                        {
                            var shortlisted = TrackingService.Instance.Get(TrackingTypes.SHORTLISTED, ShortlistTypes.JOBSEEKER, jobSeekerId, job.Id, Username);
                            if (shortlisted == null)
                            {
                                var tracking = TrackingService.Instance.AutoMatch(AutomatchTypes.JOBSEEKER, job.Id, jobSeekerId, automatchedJobSeekers[idx].Percentage, Username, out message);
                            }
                        }
                    }
                }
            }
            return automatchedJobSeekers;
        }

        /// <summary>
        ///     Automated Search Jobs
        /// </summary>
        /// <param name="jobSeeker"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'AutomatchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        public List<AutomatchResume> AutomatchedResumes(Job job, string Username)
#pragma warning restore CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'AutomatchResume' could not be found (are you missing a using directive or an assembly reference?)
        {
            var resumeList = new List<Resume>();
            var automatched_resumes = new List<AutomatchResume>();

            var title = job.Title;
            var category = job.CategoryId;
            var specialization = job.SpecializationId;
            var country = job.CountryId;
            var city = job.City;

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var resumes = dataHelper.Get<Resume>();

                var keywords = job.Title.Trim().ToLower().Split(' ').ToList();
                string first = string.Empty;
                string second = string.Empty;

                resumes = resumes.Where(x => x.Title != null && x.CountryId == country && x.CategoryId == category && x.SpecializationId == specialization);

                List<Parameter> tparameters = new List<Parameter>();
                //if (keywords.Count > 1)
                //{
                //    for (int i = 0; i < keywords.Count; i++)
                //    {
                //        first = keywords[i];
                //        for (int j = 0; j < keywords.Count; j++)
                //        {
                //            if (!first.Equals(keywords[j]))
                //            {
                //                second = string.Format("{0} {1}", first, keywords[j]);

                //                Parameter pSecond = new Parameter()
                //                {
                //                    Name = "Title",
                //                    Value = second
                //                };
                //                tparameters.Add(pSecond);
                //            }
                //        }
                //    }

                //    resumes = resumes.Where(dataHelper.BuildExpression<Resume>(tparameters)).Where(x => x.CountryId == country && x.CategoryId == category && x.SpecializationId == specialization);
                //}
                //else
                //{
                //    resumes = resumes.Where(x => x.Title != null && x.Title.Contains(keywords[0]) && x.CountryId == country && x.CategoryId == category && x.SpecializationId == specialization);
                //}

                int matched = (int)TrackingTypes.AUTO_MATCHED;

                var matchedList = dataHelper.Get<Tracking>().Where(x => x.ResumeId != null && x.JobId == job.Id && x.Type == matched && x.IsDeleted == false).Select(x => x.ResumeId);

                resumes = resumes.Where(x => !matchedList.Contains(x.Id));

                resumeList = resumes.ToList();

                automatched_resumes.AddRange(
                    resumeList.Select(x => new AutomatchResume { Job = job, Resume = x, Percentage = 33.33M }).ToList());

                for (var idx = 0; idx < automatched_resumes.Count; idx++)
                {
                    var entity = automatched_resumes[idx].Resume;
                    if (entity.StateId == job.StateId)
                    {
                        automatched_resumes[idx].Percentage = 41.67M;
                    }
                }

                for (var idx = 0; idx < automatched_resumes.Count; idx++)
                {
                    var entity = automatched_resumes[idx].Resume;
                    entity.City = (entity.City != null ? entity.City : "");
                    if (entity.City.Equals(job.City) && entity.StateId == job.StateId)
                    {
                        automatched_resumes[idx].Percentage = 50.00M;
                    }
                }

                for (var idx = 0; idx < automatched_resumes.Count; idx++)
                {
                    var entity = automatched_resumes[idx].Resume;
                    if (entity.StateId == job.StateId && entity.City == job.City && entity.Zip == job.Zip)
                    {
                        automatched_resumes[idx].Percentage = 58.33M;
                    }
                }

                for (var idx = 0; idx < automatched_resumes.Count; idx++)
                {
                    var entity = automatched_resumes[idx].Resume;
                    if (entity.StateId == job.StateId && entity.City == job.City && entity.Zip == job.Zip &&
                        entity.QualificationId == job.QualificationId)
                    {
                        automatched_resumes[idx].Percentage = 66.67M;
                    }
                }

                for (var idx = 0; idx < automatched_resumes.Count; idx++)
                {
                    var entity = automatched_resumes[idx].Resume;
                    if (entity.StateId == job.StateId && entity.City == job.City && entity.Zip == job.Zip &&
                        entity.QualificationId == job.QualificationId
                        && (entity.Experience >= job.MinimumExperience && entity.Experience <= job.MaximumExperience))
                    {
                        automatched_resumes[idx].Percentage = 75.00M;
                    }
                }

                for (var idx = 0; idx < automatched_resumes.Count; idx++)
                {
                    var entity = automatched_resumes[idx].Resume;
                    if (entity.StateId == job.StateId && entity.City == job.City && entity.Zip == job.Zip &&
                        entity.QualificationId == job.QualificationId
                        && (entity.Experience >= job.MinimumExperience && entity.Experience <= job.MaximumExperience)
                        && (entity.MinimumAge >= job.MinimumAge && entity.MaximumAge <= job.MaximumAge))
                    {
                        automatched_resumes[idx].Percentage = 83.33M;
                    }
                }

                for (var idx = 0; idx < automatched_resumes.Count; idx++)
                {
                    var entity = automatched_resumes[idx].Resume;
                    if (entity.StateId == job.StateId && entity.City == job.City && entity.Zip == job.Zip &&
                        entity.QualificationId == job.QualificationId
                        && (entity.Experience >= job.MinimumExperience && entity.Experience <= job.MaximumExperience)
                        && (entity.MinimumAge >= job.MinimumAge && entity.MaximumAge <= job.MaximumAge)
                        && (entity.ExpectedSalary >= job.MinimumSalary && entity.ExpectedSalary <= job.MaximumSalary))
                    {
                        automatched_resumes[idx].Percentage = 91.67M;
                    }
                }

                for (var idx = 0; idx < automatched_resumes.Count; idx++)
                {
                    var entity = automatched_resumes[idx].Resume;
                    if (entity.StateId == job.StateId && entity.City == job.City && entity.Zip == job.Zip &&
                        entity.QualificationId == job.QualificationId
                        && (entity.Experience >= job.MinimumExperience && entity.Experience <= job.MaximumExperience)
                        && (entity.MinimumAge >= job.MinimumAge && entity.MaximumAge <= job.MaximumAge)
                        && (entity.ExpectedSalary >= job.MinimumSalary && entity.ExpectedSalary <= job.MaximumSalary) &&
                        entity.EmploymentTypeId == job.EmploymentTypeId)
                    {
                        automatched_resumes[idx].Percentage = 100M;
                    }
                }
                var message = "";
                for (var idx = 0; idx < automatched_resumes.Count; idx++)
                {
                    if (automatched_resumes[idx].Resume != null)
                    {
                        var resumeid = automatched_resumes[idx].Resume.Id;
                        var shortlisted = TrackingService.Instance.Get(TrackingTypes.SHORTLISTED, ShortlistTypes.RESUME, resumeid, job.Id, Username);

                        if (shortlisted == null)
                        {
                            var tracking = TrackingService.Instance.AutoMatch(AutomatchTypes.RESUME, job.Id, resumeid, automatched_resumes[idx].Percentage, Username, out message);
                        }
                    }
                }
            }
            return automatched_resumes;
        }

        /// <summary>
        ///     Search resumes by Who and Where Parameters.
        /// </summary>
        /// <param name="who"></param>
        /// <param name="where"></param>
        /// <param name="records"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'Resume' could not be found (are you missing a using directive or an assembly reference?)
        public IEnumerable<Resume> SearchResumes(string who, string where, out int records)
#pragma warning restore CS0246 // The type or namespace name 'Resume' could not be found (are you missing a using directive or an assembly reference?)
        {
            IQueryable<Resume> resumes = null;
            List<Resume> list = new List<Resume>();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                resumes = dataHelper.Get<Resume>().Where(x => x.IsActive == true && x.IsDeleted == false);

                records = 0;
                if (!string.IsNullOrEmpty(who))
                {
                    var who_list = new List<string>();
                    if (who.Split(' ').Length > 0)
                    {
                        who_list = who.ToLower().Split(' ').ToList();
                    }

                    if (who_list.Count > 0)
                    {
                        resumes = resumes.Where(x => who_list.Any(k => x.Title.ToLower().Contains(k)));
                    }
                }

                if (!string.IsNullOrEmpty(where))
                {
                    resumes = resumes.Where(x => ((x.Country != null ? x.Country.Text + x.Country.Value : "") + (x.State != null ? x.State.Text : "") + (!string.IsNullOrEmpty(x.City) ? x.City : "")).ToLower().Contains(where.ToLower()));
                }

                records = resumes.Count();
                list = resumes.ToList();
            }
            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'Resume' could not be found (are you missing a using directive or an assembly reference?)
        public IEnumerable<Resume> SearchResumes(string title, int? categoryId, int? specializationId, long? countryId,
#pragma warning restore CS0246 // The type or namespace name 'Resume' could not be found (are you missing a using directive or an assembly reference?)
            long? stateId, string city, out int records)
        {
            IQueryable<Resume> resumes = null;
            List<Resume> list = new List<Resume>();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                resumes = dataHelper.Get<Resume>().Where(x => x.IsActive == true && x.IsDeleted == false);
                records = 0;

                if (!string.IsNullOrEmpty(title))
                {
                    var title_list = new List<string>();
                    if (title.Split(' ').Length > 0)
                    {
                        title_list = title.ToLower().Split(' ').ToList();
                    }

                    if (title_list.Count > 0)
                    {
                        resumes = resumes.Where(x => title_list.Any(k => x.Title.ToLower().Contains(k)));
                    }
                }

                if (categoryId != null)
                {
                    resumes = resumes.Where(x => x.CategoryId == categoryId);
                }

                if (specializationId != null)
                {
                    resumes = resumes.Where(x => x.SpecializationId == specializationId);
                }

                if (countryId != null)
                {
                    resumes = resumes.Where(x => x.CountryId == countryId);
                }

                if (stateId != null)
                {
                    resumes = resumes.Where(x => x.StateId == stateId);
                }

                if (!string.IsNullOrEmpty(city))
                {
                    resumes = resumes.Where(x => x.City.Contains(city));
                }

                records = resumes.Count();
                list = resumes.ToList();
            }
            return list;
        }

        ///// <summary>
        /////     Search Jobseekers by Who and Where parameters.
        ///// </summary>
        ///// <param name="who"></param>
        ///// <param name="where"></param>
        ///// <param name="records"></param>
        ///// <returns></returns>
        //public IEnumerable<UserProfile> SearchJobseekers(string who, string where, string username, out int records)
        //{
        //    var blockedList = new List<long>();
        //    int jobSeeker = (int)SecurityRoles.Jobseeker;

        //    if (!string.IsNullOrEmpty(username))
        //    {
        //        blockedList =
        //            DataHelper.Instance.GetList<BlockedPeople>()
        //                .Where(x => x.BlockedUser.Username.Equals(username) && x.Blocker.Type == jobSeeker)
        //                .Select(x => x.BlockerId)
        //                .ToList();
        //    }
        //    records = 0;
        //    IEnumerable<UserProfile> jobSeekers = DataHelper.Instance.GetList<UserProfile>().Where(x => x.Type == jobSeeker);
        //    if (blockedList.Count > 0)
        //    {
        //        jobSeekers = jobSeekers.Where(x => !blockedList.Contains(x.UserId));
        //    }

        //    if (!string.IsNullOrEmpty(who))
        //    {
        //        var who_list = new List<string>();
        //        if (who.Split(' ').Length > 0)
        //        {
        //            who_list = who.ToLower().Split(' ').ToList();
        //        }

        //        if (who_list.Count > 0)
        //        {
        //            jobSeekers = jobSeekers.Where(x => who_list.Any(k => x.Title.ToLower().Contains(k)));
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(where))
        //    {
        //        jobSeekers = jobSeekers.Where(x => ((x.Country != null ? x.Country.Text + x.Country.Value : "") + (x.State != null ? x.State.Text : "") + (!string.IsNullOrEmpty(x.City) ? x.City : "")).ToLower().Contains(where.ToLower()));
        //    }

        //    records = jobSeekers.Count();
        //    return jobSeekers;
        //}

#pragma warning disable CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        public IEnumerable<UserProfile> SearchJobseekers(SearchResume model, out int records)
#pragma warning restore CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
        {
            records = 0;
            var blockedList = new List<string>();
            int jobSeeker = (int)SecurityRoles.Jobseeker;

            List<UserProfile> jobSeekers = new List<UserProfile>();
            List<Parameter> parameters = new List<Parameter>();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                var result = dataHelper.Get<UserProfile>().Where(x => x.Title != null && x.CategoryId != null && x.SpecializationId != null);

                if (!string.IsNullOrEmpty(model.Username))
                {
                    UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", model.Username);
                    blockedList = dataHelper.Get<BlockedPeople>().Where(x => x.BlockedId == profile.UserId).Select(x => x.Blocker.Username).ToList();
                }

                if (blockedList.Count > 0)
                {
                    result = result.Where(x => !blockedList.Contains(x.Username));
                }

                if (!string.IsNullOrEmpty(model.Title))
                {
                    var who_list = new List<string>();
                    if (model.Title.Split(' ').Length > 0)
                    {
                        who_list = model.Title.ToLower().Split(' ').ToList();
                    }

                    if (who_list.Count > 0)
                    {
                        result = result.Where(x => who_list.Any(k => (x.Title != null && x.Title.ToLower().Contains(k)) || ((x.FirstName != null && x.LastName != null) && (x.FirstName + " " + x.LastName).ToLower().Contains(k))));
                    }
                }

                if (!string.IsNullOrEmpty(model.Where))
                {
                    result = result.Where(x => ((x.Country != null ? x.Country.Text + x.Country.Value : "") + (x.State != null ? x.State.Text : "") + (!string.IsNullOrEmpty(x.City) ? x.City : "")).ToLower().Contains(model.Where.ToLower()));
                }

                if (!string.IsNullOrEmpty(model.City))
                {
                    result = result.Where(x => x.City.ToLower().Contains(model.City.ToLower()));
                }
                parameters = new List<Parameter>();
                if (model.CategoryId != null)
                {
                    parameters.Add(new Parameter() { Name = "CategoryId", Value = model.CategoryId, Comparison = ParameterComparisonTypes.Equals });
                }

                if (model.SpecializationId != null)
                {
                    parameters.Add(new Parameter() { Name = "SpecializationId", Value = model.SpecializationId, Comparison = ParameterComparisonTypes.Equals });
                }

                if (model.CountryId != null)
                {
                    parameters.Add(new Parameter() { Name = "CountryId", Value = model.CountryId, Comparison = ParameterComparisonTypes.Equals });
                }

                if (model.StateId != null)
                {
                    parameters.Add(new Parameter() { Name = "StateId", Value = model.StateId, Comparison = ParameterComparisonTypes.Equals });
                }

                if (model.AgeMin != null)
                {
                    var ageMin = (byte)model.AgeMin;
                    parameters.Add(new Parameter() { Name = "Age", Value = ageMin, Comparison = ParameterComparisonTypes.GreaterThanEquals });
                }

                if (model.AgeMax != null)
                {
                    var ageMax = (byte)model.AgeMax;
                    parameters.Add(new Parameter() { Name = "Age", Value = ageMax, Comparison = ParameterComparisonTypes.LessThanEqual });
                }

                if (model.Experience != null)
                {
                    var exp = (byte)model.Experience.Value;
                    parameters.Add(new Parameter() { Name = "Experience", Value = exp, Comparison = ParameterComparisonTypes.Equals });
                }

                if (model.QualificationId != null)
                {
                    parameters.Add(new Parameter() { Name = "QualificationId", Value = Convert.ToInt64(model.QualificationId.Value), Comparison = ParameterComparisonTypes.Equals });
                }

                if (model.MinSalary != null)
                {
                    parameters.Add(new Parameter() { Name = "ExpectedSalary", Value = model.MinSalary, Comparison = ParameterComparisonTypes.GreaterThanEquals });
                }

                if (model.MaxSalary != null)
                {
                    parameters.Add(new Parameter() { Name = "ExpectedSalary", Value = model.MaxSalary, Comparison = ParameterComparisonTypes.LessThanEqual });
                }

                parameters.Add(new Parameter() { Name = "Type", Value = jobSeeker, Comparison = ParameterComparisonTypes.Equals });


                var criteria = dataHelper.BuildAnd<UserProfile>(parameters);
                result = result.Where(criteria);

                records = result.Count();
                jobSeekers = result.ToList();
            }
            return jobSeekers;
        }
    }
}