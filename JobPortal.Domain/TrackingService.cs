using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Helpers;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Collections.Generic;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Data.Common;
using System.Data.SqlClient;

namespace JobPortal.Domain
{
#pragma warning disable CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    public class TrackingService : DataService
#pragma warning restore CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    {
        private static volatile TrackingService instance;
        private static readonly object sync = new object();
        private static Regex tokenRegx = new Regex("{.*?}", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        /// Default Private constructor.
        /// </summary>
        private TrackingService()
        {
        }

        /// <summary>
        /// Gets the instance of TrackingService.
        /// </summary>
        public static TrackingService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new TrackingService();
                        }
                    }
                }
                return instance;
            }
        }

        public bool IsInterviewStarted(long jobseekerId, long employerId)
        {
            int initiated = (int)InterviewStatus.INITIATED;
            int inprogress = (int)InterviewStatus.INTERVIEW_IN_PROGRESS;
            int count = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                count = dataHelper.Get<Interview>().Count(x => (x.Status == initiated || x.Status == inprogress) && x.UserId == employerId && x.Tracking.JobseekerId == jobseekerId);
            }
            return (count <= 0);
        }
#pragma warning disable CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
        public bool Delete(Tracking entity, string username)
#pragma warning restore CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
        {
            bool flag = false;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                flag = dataHelper.Delete<Tracking>(entity, username);
            }
            return flag;
        }

        public bool Delete(Guid Id)
        {
            bool flag = false;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Tracking entity = Get(Id);
                flag = dataHelper.Remove<Tracking>(entity);
            }
            return flag;
        }

        /// <summary>
        /// Tracks the viewed job.
        /// </summary>
        /// <param name="ResumeId"></param>
        /// <param name="Username"></param>
        /// <param name="message"></param>
#pragma warning disable CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
        public Tracking JobViewed(long jobId, string username, out string message)
#pragma warning restore CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
        {
            var record = new Tracking();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var profile = dataHelper.GetSingle<UserProfile>("Username", username);
                var viewed = (int)TrackingTypes.VIEWED;
                message = "";

                if (profile != null)
                {
                    record = new Tracking
                    {
                        Id = Guid.NewGuid(),
                        JobId = jobId,
                        JobseekerId = profile.UserId,
                        ResumeId = null,
                        Type = viewed,
                        UserId = profile.UserId,
                        DateUpdated = DateTime.Now,
                        IsDownloaded = false,
                    };
                    dataHelper.Add(record);

                    if (!string.IsNullOrEmpty(profile.Title) && profile.CategoryId != null && profile.SpecializationId != null && !string.IsNullOrEmpty(profile.Content))
                    {
                        var detail = new TrackingDetail
                        {
                            Id = record.Id,
                            Title = profile.Title,
                            CategoryId = profile.CategoryId.Value,
                            SpecializationId = profile.SpecializationId.Value,
                            CountryId = profile.CountryId.Value,
                            StateId = profile.StateId,
                            FileName = profile.FileName,
                            Content = profile.Content
                        };

                        dataHelper.Add(detail);
                    }
                }
                else
                {
                    record = null;
                }
            }
            message = "Job Viewed!";
            return record;
        }

        /// <summary>
        /// Tracks downloaded resumes
        /// </summary>
        /// <param name="ResumeId"></param>
        /// <param name="Username"></param>
        /// <param name="message"></param>
        public void Downloaded(long Id, string Username, out string message)
        {
            Tracking record = new Tracking();
            TrackingDetail detail = new TrackingDetail();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                var employer = dataHelper.GetSingle<UserProfile>("Username", Username);
                var downloaded = (int)TrackingTypes.DOWNLOADED;
                message = "";

                //if (type == DownloadTypes.JOBSEEKER)
                //{
                    UserProfile jobSeeker = dataHelper.GetSingle<UserProfile>("UserId", Id);

                    record = dataHelper.Get<Tracking>().Where(x => x.JobseekerId == Id && x.Type == downloaded && x.UserId == employer.UserId).FirstOrDefault();
                    if (record == null)
                    {
                        record = new Tracking
                        {
                            Id = Guid.NewGuid(),
                            JobseekerId = Id,
                            Type = downloaded,
                            UserId = employer.UserId,
                            DateUpdated = DateTime.Now,
                            IsDownloaded = false
                        };
                        dataHelper.Add(record, Username);

                        detail = new TrackingDetail()
                        {
                            Id = record.Id,
                            Title = jobSeeker.Title,
                            CategoryId = jobSeeker.CategoryId.Value,
                            SpecializationId = jobSeeker.SpecializationId.Value,
                            CountryId = jobSeeker.CountryId.Value,
                            StateId = jobSeeker.StateId,
                            FileName = jobSeeker.FileName,
                            Content = jobSeeker.Content
                        };
                        dataHelper.Add(detail);
                        message = "Resume downloaded!";
                    }
                //}
            }
        }

        /// <summary>
        /// Tracks bookmarks
        /// </summary>
        /// <param name="JobId"></param>
        /// <param name="Username"></param>
        /// <param name="message"></param>
#pragma warning disable CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'BookmarkedTypes' could not be found (are you missing a using directive or an assembly reference?)
        public Tracking Bookmark(long Id, BookmarkedTypes type, string Username, out string message)
#pragma warning restore CS0246 // The type or namespace name 'BookmarkedTypes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
        {
            Tracking record = null;
            message = "";
            Hashtable parameters;
            var profile = MemberService.Instance.Get(Username);

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var bookmakred = (int)TrackingTypes.BOOKMAKRED;

                switch (type)
                {
                    case BookmarkedTypes.JOB:
                        parameters = new Hashtable();
                        parameters.Add("JobId", Id);
                        parameters.Add("UserId", profile.UserId);
                        parameters.Add("Type", bookmakred);
                        parameters.Add("IsDeleted", false);

                        record = dataHelper.GetSingle<Tracking>(parameters);

                        if (record == null)
                        {
                            record = new Tracking
                            {
                                Id = Guid.NewGuid(),
                                JobId = Id,
                                ResumeId = null,
                                Type = bookmakred,
                                UserId = profile.UserId,
                                DateUpdated = DateTime.Now,
                                IsDownloaded = false
                            };

                            dataHelper.Add(record);

                            var detail = new TrackingDetail
                              {
                                  Id = record.Id,
                                  Title = profile.Title,
                                  CategoryId = profile.CategoryId.Value,
                                  SpecializationId = profile.SpecializationId.Value,
                                  CountryId = profile.CountryId.Value,
                                  StateId = profile.StateId,
                                  FileName = profile.FileName,
                                  Content = profile.Content
                              };
                            dataHelper.Add(detail);
                            message = "Bookmarked successfully!";
                        }
                        else
                        {
                            message = "Already bookmarked!";
                        }
                        break;
                    case BookmarkedTypes.JOBSEEKER:
                        parameters = new Hashtable();
                        parameters.Add("JobseekerId", Id);
                        parameters.Add("UserId", profile.UserId);
                        parameters.Add("Type", bookmakred);
                        parameters.Add("IsDeleted", false);
                        record = dataHelper.GetSingle<Tracking>(parameters);

                        if (record == null)
                        {
                            record = new Tracking
                            {
                                Id = Guid.NewGuid(),
                                JobId = null,
                                ResumeId = null,
                                JobseekerId = Id,
                                Type = bookmakred,
                                UserId = profile.UserId,
                                DateUpdated = DateTime.Now,
                                IsDownloaded = false
                            };
                            dataHelper.Add(record);

                            UserProfile jobSeeker = dataHelper.GetSingle<UserProfile>("UserId", Id);
                            if (jobSeeker != null)
                            {
                                var detail = new TrackingDetail
                                {
                                    Id = record.Id,
                                    Title = jobSeeker.Title,
                                    CategoryId = jobSeeker.CategoryId.Value,
                                    SpecializationId = jobSeeker.SpecializationId.Value,
                                    CountryId = jobSeeker.CountryId.Value,
                                    StateId = jobSeeker.StateId,
                                    FileName = jobSeeker.FileName,
                                    Content = jobSeeker.Content
                                };
                                dataHelper.Add(detail);
                            }
                            message = "Bookmarked successfully!";
                        }
                        else
                        {
                            message = "Already bookmarked!";
                        }
                        break;
                }
            }
            return record;
        }

        /// <summary>
        /// Gets the tracking record
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="shortListType"></param>
        /// <param name="Id"></param>
        /// <param name="Username"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'ShortlistTypes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'TrackingTypes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
        public Tracking Get(TrackingTypes Type, ShortlistTypes shortListType, long Id, string Username)
#pragma warning restore CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'TrackingTypes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'ShortlistTypes' could not be found (are you missing a using directive or an assembly reference?)
        {
            var type = (int)Type;
            Tracking record = new Tracking();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", Username);
                switch (shortListType)
                {
                    case ShortlistTypes.JOBSEEKER:
                        record = dataHelper.Get<Tracking>().Where(x => x.Type == type && x.JobseekerId == Id && x.Resume == null && x.UserProfile.Username == Username).OrderByDescending(x => x.DateUpdated).FirstOrDefault();
                        break;
                    case ShortlistTypes.RESUME:

                        record = dataHelper.Get<Tracking>().Where(x => x.Type == type && x.ResumeId == Id && x.Jobseeker == null && x.UserProfile.Username == Username).OrderByDescending(x => x.DateUpdated).FirstOrDefault();

                        break;
                }
            }
            return record;
        }

        /// <summary>
        /// Gets the tracking record
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="shortListType"></param>
        /// <param name="Id"></param>
        /// <param name="Username"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'ShortlistTypes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'TrackingTypes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
        public Tracking Get(TrackingTypes Type, ShortlistTypes shortListType, long Id, long? JobId, string Username)
#pragma warning restore CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'TrackingTypes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'ShortlistTypes' could not be found (are you missing a using directive or an assembly reference?)
        {
            var type = (int)Type;
            Tracking record = new Tracking();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", Username);
                switch (shortListType)
                {
                    case ShortlistTypes.JOBSEEKER:
                        if (JobId != null)
                        {
                            record = dataHelper.Get<Tracking>().Where(x => x.Type == type && x.JobseekerId == Id && x.Resume == null && x.UserProfile.Username == Username && x.JobId == JobId.Value).OrderByDescending(x => x.DateUpdated).FirstOrDefault();
                        }
                        else
                        {
                            record = dataHelper.Get<Tracking>().Where(x => x.Type == type && x.JobseekerId == Id && x.Resume == null && x.UserProfile.Username == Username).OrderByDescending(x => x.DateUpdated).FirstOrDefault();
                        }
                        break;
                    case ShortlistTypes.RESUME:
                        if (JobId != null)
                        {
                            record = dataHelper.Get<Tracking>().Where(x => x.Type == type && x.ResumeId == Id && x.Jobseeker == null && x.UserProfile.Username == Username && x.JobId == JobId.Value).OrderByDescending(x => x.DateUpdated).FirstOrDefault();
                        }
                        else
                        {
                            record = dataHelper.Get<Tracking>().Where(x => x.Type == type && x.ResumeId == Id && x.Jobseeker == null && x.UserProfile.Username == Username).OrderByDescending(x => x.DateUpdated).FirstOrDefault();
                        }
                        break;
                }
            }
            return record;
        }

        /// <summary>
        /// Gets the tracking record
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="shortListType"></param>
        /// <param name="Id"></param>
        /// <param name="Username"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'ShortlistTypes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'TrackingTypes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
        public Tracking Get(TrackingTypes Type, ShortlistTypes shortListType, long Id, long jobId, string Username)
#pragma warning restore CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'TrackingTypes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'ShortlistTypes' could not be found (are you missing a using directive or an assembly reference?)
        {
            var type = (int)Type;
            Tracking record = new Tracking();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", Username);
                Hashtable parameters;
                switch (shortListType)
                {
                    case ShortlistTypes.JOBSEEKER:
                        parameters = new Hashtable();
                        parameters.Add("JobseekerId", Id);
                        parameters.Add("JobId", jobId);
                        parameters.Add("UserId", profile.UserId);
                        parameters.Add("Type", type);
                        parameters.Add("IsDeleted", false);
                        record = dataHelper.GetSingle<Tracking>(parameters);
                        break;
                    case ShortlistTypes.RESUME:
                        parameters = new Hashtable();
                        parameters.Add("ResumeId", Id);
                        parameters.Add("JobId", jobId);
                        parameters.Add("UserId", profile.UserId);
                        parameters.Add("Type", type);
                        parameters.Add("IsDeleted", false);
                        record = dataHelper.GetSingle<Tracking>(parameters);
                        break;
                }
            }
            return record;
        }

        /// <summary>
        /// Gets the tracking record by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
        public Tracking Get(Guid Id)
#pragma warning restore CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
        {
            Tracking record = new Tracking();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                record = dataHelper.Get<Tracking>().SingleOrDefault(x => x.Id == Id);
            }
            return record;
        }

        /// <summary>
        /// Gets the tracking record by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'TrackingDetail' could not be found (are you missing a using directive or an assembly reference?)
        public TrackingDetail GetDetail(Guid Id)
#pragma warning restore CS0246 // The type or namespace name 'TrackingDetail' could not be found (are you missing a using directive or an assembly reference?)
        {
            TrackingDetail record = new TrackingDetail();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                record = dataHelper.GetSingle<TrackingDetail>("Id", Id);
            }
            return record;
        }

        /// <summary>
        /// Gets the tracking record by Tracking Type, ResumeId and Username
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="ResumeId"></param>
        /// <param name="Username"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'TrackingTypes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
        public Tracking Get(TrackingTypes Type, long ResumeId, string Username)
#pragma warning restore CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'TrackingTypes' could not be found (are you missing a using directive or an assembly reference?)
        {
            var type = (int)Type;
            Tracking record = new Tracking();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", Username);
                Hashtable parameters = new Hashtable();

                parameters.Add("ResumeId", ResumeId);
                parameters.Add("UserId", profile.UserId);
                parameters.Add("Type", Type);
                parameters.Add("IsDeleted", false);
                record = dataHelper.GetSingle<Tracking>(parameters);
            }
            return record;
        }

        /// <summary>
        /// Tracks Automatch
        /// </summary>
        /// <param name="matchType"></param>
        /// <param name="reference"></param>
        /// <param name="Id"></param>
        /// <param name="Username"></param>
        /// <param name="message"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'AutomatchTypes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
        public Tracking AutoMatch(AutomatchTypes matchType, long reference, long Id, string Username, out string message, string title = "")
#pragma warning restore CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'AutomatchTypes' could not be found (are you missing a using directive or an assembly reference?)
        {
            var type = (int)TrackingTypes.AUTO_MATCHED;
            Tracking tracking = null;
            var max_id = new Guid();
            Tracking latest = null;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                var list = dataHelper.Get<Tracking>();

                var parameters = new Hashtable();

                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", Username);

                message = string.Empty;
                switch (matchType)
                {
                    case AutomatchTypes.JOB:
                        parameters.Add("JobId", Id);
                        parameters.Add("JobseekerId", reference);
                        parameters.Add("Type", type);
                        parameters.Add("UserId", profile.UserId);
                        parameters.Add("IsDeleted", false);

                        list = list.Where(x => x.JobId == Id && x.UserId == profile.UserId);
                        if (list.Count() > 0)
                        {
                            max_id = list.ToList().Max(x => x.Id);
                            latest = list.Where(x => x.Id == max_id).SingleOrDefault();
                        }
                        if (!string.IsNullOrEmpty(title))
                        {
                            tracking = dataHelper.Get<Tracking>().Where(x => x.JobId == Id && x.JobseekerId == reference && x.Type == type && x.UserId == profile.UserId && x.IsDeleted == false && x.Job.Title.Equals(title)).SingleOrDefault();
                        }
                        else
                        {
                            tracking = dataHelper.GetSingle<Tracking>(parameters);
                        }
                        break;
                    case AutomatchTypes.JOBSEEKER:
                        parameters.Add("JobId", reference);
                        parameters.Add("JobseekerId", Id);
                        parameters.Add("Type", type);
                        parameters.Add("UserId", profile.UserId);
                        parameters.Add("IsDeleted", false);

                        list = list.Where(x => x.JobseekerId == Id && x.UserId == profile.UserId);
                        if (list.Count() > 0)
                        {
                            max_id = list.ToList().Max(x => x.Id);
                            latest = list.Where(x => x.Id == max_id).SingleOrDefault();
                        }
                        if (!string.IsNullOrEmpty(title))
                        {
                            tracking = dataHelper.Get<Tracking>().Where(x => x.JobId == reference && x.JobseekerId == Id && x.Type == type && x.UserId == profile.UserId && x.IsDeleted == false && x.Job.Title.Equals(title)).SingleOrDefault();
                        }
                        else
                        {
                            tracking = dataHelper.GetSingle<Tracking>(parameters);
                        }
                        break;
                    case AutomatchTypes.RESUME:
                        parameters.Add("JobId", reference);
                        parameters.Add("ResumeId", Id);
                        parameters.Add("Type", type);
                        parameters.Add("UserId", profile.UserId);
                        parameters.Add("IsDeleted", false);

                        list = list.Where(x => x.ResumeId == Id && x.UserId == profile.UserId);
                        if (list.Count() > 0)
                        {
                            max_id = list.ToList().Max(x => x.Id);
                            latest = list.Where(x => x.Id == max_id).SingleOrDefault();
                        }

                        if (!string.IsNullOrEmpty(title))
                        {
                            tracking = dataHelper.Get<Tracking>().Where(x => x.JobId == reference && x.ResumeId == Id && x.Type == type && x.UserId == profile.UserId && x.IsDeleted == false && x.Job.Title.Equals(title)).SingleOrDefault();
                        }
                        else
                        {
                            tracking = dataHelper.GetSingle<Tracking>(parameters);
                        }
                        break;
                }


                if (tracking == null)
                {
                    switch (matchType)
                    {
                        case AutomatchTypes.JOB:
                            tracking = new Tracking()
                            {
                                Id = Guid.NewGuid(),
                                JobId = Id,
                                JobseekerId = reference,
                                ResumeId = null,
                                Type = type,
                                DateUpdated = DateTime.Now,
                                UserId = profile.UserId,
                                IsDownloaded = false
                            };

                            dataHelper.Add(tracking, Username);

                            var jobSeeker = dataHelper.GetSingle<UserProfile>("UserId", reference);
                            var detail = new TrackingDetail
                            {
                                Id = tracking.Id,
                                Title = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.Title : latest.TrackingDetail.Title,
                                CategoryId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.CategoryId.Value : latest.TrackingDetail.CategoryId,
                                SpecializationId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.SpecializationId.Value : latest.TrackingDetail.SpecializationId,
                                CountryId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.CountryId.Value : latest.TrackingDetail.CountryId,
                                StateId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.StateId.Value : latest.TrackingDetail.StateId,
                                FileName = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.FileName : latest.TrackingDetail.FileName,
                                Content = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.Content : latest.TrackingDetail.Content
                            };
                            dataHelper.Add(detail);
                            if (latest != null)
                            {
                                dataHelper.Delete(latest);
                            }
                            message = string.Format("{0}", TrackingTypes.AUTO_MATCHED.GetDescription().ToLower());
                            break;
                        case AutomatchTypes.JOBSEEKER:
                            tracking = new Tracking
                            {
                                Id = Guid.NewGuid(),
                                JobId = reference,
                                JobseekerId = Id,
                                ResumeId = null,
                                Type = type,
                                DateUpdated = DateTime.Now,
                                UserId = profile.UserId,
                                IsDownloaded = false
                            };

                            dataHelper.Add(tracking, Username);

                            jobSeeker = dataHelper.GetSingle<UserProfile>("UserId", Id);
                            detail = new TrackingDetail
                            {
                                Id = tracking.Id,
                                Title = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.Title : latest.TrackingDetail.Title,
                                CategoryId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.CategoryId.Value : latest.TrackingDetail.CategoryId,
                                SpecializationId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.SpecializationId.Value : latest.TrackingDetail.SpecializationId,
                                CountryId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.CountryId.Value : latest.TrackingDetail.CountryId,
                                StateId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.StateId.Value : latest.TrackingDetail.StateId,
                                FileName = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.FileName : latest.TrackingDetail.FileName,
                                Content = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.Content : latest.TrackingDetail.Content
                            };
                            dataHelper.Add(detail);
                            if (latest != null)
                            {
                                dataHelper.Delete(latest);
                            }
                            message = string.Format("{0}", TrackingTypes.AUTO_MATCHED.GetDescription().ToLower());
                            break;
                        case AutomatchTypes.RESUME:
                            tracking = new Tracking
                            {
                                Id = Guid.NewGuid(),
                                JobId = reference,
                                ResumeId = Id,
                                JobseekerId = null,
                                Type = type,
                                DateUpdated = DateTime.Now,
                                UserId = profile.UserId,
                                IsDownloaded = false
                            };

                            dataHelper.Add(tracking, Username);

                            var resume = dataHelper.GetSingle<Resume>("Id", Id);
                            detail = new TrackingDetail
                            {
                                Id = tracking.Id,
                                Title = (latest == null || (latest != null && latest.TrackingDetail == null)) ? resume.Title : latest.TrackingDetail.Title,
                                CategoryId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? resume.CategoryId.Value : latest.TrackingDetail.CategoryId,
                                SpecializationId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? resume.SpecializationId.Value : latest.TrackingDetail.SpecializationId,
                                CountryId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? resume.CountryId.Value : latest.TrackingDetail.CountryId,
                                StateId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? resume.StateId.Value : latest.TrackingDetail.StateId,
                                FileName = (latest == null || (latest != null && latest.TrackingDetail == null)) ? resume.FileName : latest.TrackingDetail.FileName,
                                Content = (latest == null || (latest != null && latest.TrackingDetail == null)) ? resume.Content : latest.TrackingDetail.Content
                            };
                            dataHelper.Add(detail);
                            if (latest != null)
                            {
                                dataHelper.Delete(latest);
                            }
                            message = string.Format("{0}", TrackingTypes.AUTO_MATCHED.GetDescription().ToLower());
                            break;
                    }
                }
                else
                {
                    message = string.Format("already {0}", TrackingTypes.AUTO_MATCHED.GetDescription().ToLower());
                }
            }
            return tracking;
        }

        /// <summary>
        /// Tracks Automatch
        /// </summary>
        /// <param name="matchType"></param>
        /// <param name="reference"></param>
        /// <param name="Id"></param>
        /// <param name="Username"></param>
        /// <param name="message"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'AutomatchTypes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
        public Tracking AutoMatch(AutomatchTypes matchType, long reference, long Id, decimal Weightage, string Username, out string message, string title = "")
#pragma warning restore CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'AutomatchTypes' could not be found (are you missing a using directive or an assembly reference?)
        {
            var type = (int)TrackingTypes.AUTO_MATCHED;
            Tracking tracking = null;
            var max_id = new Guid();
            Tracking latest = null;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                var list = dataHelper.Get<Tracking>();

                var parameters = new Hashtable();
                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", Username);

                message = string.Empty;
                switch (matchType)
                {
                    case AutomatchTypes.JOB:
                        parameters.Add("JobId", Id);
                        parameters.Add("JobseekerId", reference);
                        parameters.Add("Type", type);
                        parameters.Add("UserId", profile.UserId);
                        parameters.Add("IsDeleted", false);

                        list = list.Where(x => x.JobId == Id && x.UserId == profile.UserId);
                        if (list.Count() > 0)
                        {
                            max_id = list.ToList().Max(x => x.Id);
                            latest = list.Where(x => x.Id == max_id).SingleOrDefault();
                        }

                        if (!string.IsNullOrEmpty(title))
                        {
                            tracking = dataHelper.Get<Tracking>().Where(x => x.JobId == Id && x.JobseekerId == reference && x.Type == type && x.UserId == profile.UserId && x.IsDeleted == false && x.Job.Title.Equals(title)).SingleOrDefault();
                        }
                        else
                        {
                            tracking = dataHelper.GetSingle<Tracking>(parameters);
                        }

                        break;
                    case AutomatchTypes.JOBSEEKER:
                        parameters.Add("JobId", reference);
                        parameters.Add("JobseekerId", Id);
                        parameters.Add("Type", type);
                        parameters.Add("UserId", profile.UserId);
                        parameters.Add("IsDeleted", false);

                        list = list.Where(x => x.JobseekerId == Id && x.UserId == profile.UserId);
                        if (list.Count() > 0)
                        {
                            max_id = list.ToList().Max(x => x.Id);
                            latest = list.Where(x => x.Id == max_id).SingleOrDefault();
                        }

                        if (!string.IsNullOrEmpty(title))
                        {
                            tracking = dataHelper.Get<Tracking>().Where(x => x.JobId == reference && x.JobseekerId == Id && x.Type == type && x.UserId == profile.UserId && x.IsDeleted == false && x.Job.Title.Equals(title)).SingleOrDefault();
                        }
                        else
                        {
                            tracking = dataHelper.GetSingle<Tracking>(parameters);
                        }
                        break;
                }

                tracking = dataHelper.GetSingle<Tracking>(parameters);
                if (tracking == null)
                {
                    switch (matchType)
                    {
                        case AutomatchTypes.JOB:
                            tracking = new Tracking()
                            {
                                Id = Guid.NewGuid(),
                                JobId = Id,
                                JobseekerId = reference,
                                ResumeId = null,
                                Type = type,
                                Weightage = Weightage,
                                DateUpdated = DateTime.Now,
                                UserId = profile.UserId,
                                IsDownloaded = false
                            };

                            dataHelper.AddEntity(tracking, Username);

                            var jobSeeker = dataHelper.GetSingle<UserProfile>("UserId", reference);
                            var detail = new TrackingDetail
                            {
                                Id = tracking.Id,
                                Title = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.Title : latest.TrackingDetail.Title,
                                CategoryId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.CategoryId.Value : latest.TrackingDetail.CategoryId,
                                SpecializationId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.SpecializationId.Value : latest.TrackingDetail.SpecializationId,
                                CountryId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.CountryId.Value : latest.TrackingDetail.CountryId,
                                StateId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.StateId : latest.TrackingDetail.StateId,
                                FileName = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.FileName : latest.TrackingDetail.FileName,
                                Content = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.Content : latest.TrackingDetail.Content
                            };
                            dataHelper.AddEntity(detail);
                            if (latest != null)
                            {
                                dataHelper.DeleteUpdate(latest);
                            }
                            message = string.Format("{0}", TrackingTypes.AUTO_MATCHED.GetDescription().ToLower());
                            break;
                        case AutomatchTypes.JOBSEEKER:
                            tracking = new Tracking
                            {
                                Id = Guid.NewGuid(),
                                JobId = reference,
                                JobseekerId = Id,
                                ResumeId = null,
                                Type = type,
                                Weightage = Weightage,
                                DateUpdated = DateTime.Now,
                                UserId = profile.UserId,
                                IsDownloaded = false
                            };

                            dataHelper.AddEntity(tracking, Username);

                            jobSeeker = dataHelper.GetSingle<UserProfile>("UserId", Id);
                            detail = new TrackingDetail
                            {
                                Id = tracking.Id,
                                Title = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.Title : latest.TrackingDetail.Title,
                                CategoryId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.CategoryId.Value : latest.TrackingDetail.CategoryId,
                                SpecializationId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.SpecializationId.Value : latest.TrackingDetail.SpecializationId,
                                CountryId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.CountryId.Value : latest.TrackingDetail.CountryId,
                                StateId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.StateId : latest.TrackingDetail.StateId,
                                FileName = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.FileName : latest.TrackingDetail.FileName,
                                Content = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.Content : latest.TrackingDetail.Content
                            };
                            dataHelper.AddEntity(detail);
                            if (latest != null)
                            {
                                dataHelper.DeleteUpdate(latest);
                            }
                            message = string.Format("{0}", TrackingTypes.AUTO_MATCHED.GetDescription().ToLower());
                            break;
                        case AutomatchTypes.RESUME:
                            tracking = new Tracking
                            {
                                Id = Guid.NewGuid(),
                                JobId = reference,
                                ResumeId = Id,
                                JobseekerId = null,
                                Type = type,
                                Weightage = Weightage,
                                DateUpdated = DateTime.Now,
                                UserId = profile.UserId,
                                IsDownloaded = false
                            };

                            dataHelper.AddEntity(tracking, Username);

                            var resume = dataHelper.GetSingle<Resume>("Id", Id);
                            detail = new TrackingDetail
                            {
                                Id = tracking.Id,
                                Title = (latest == null || (latest != null && latest.TrackingDetail == null)) ? resume.Title : latest.TrackingDetail.Title,
                                CategoryId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? resume.CategoryId.Value : latest.TrackingDetail.CategoryId,
                                SpecializationId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? resume.SpecializationId.Value : latest.TrackingDetail.SpecializationId,
                                CountryId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? resume.CountryId.Value : latest.TrackingDetail.CountryId,
                                StateId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? resume.StateId : latest.TrackingDetail.StateId,
                                FileName = (latest == null || (latest != null && latest.TrackingDetail == null)) ? resume.FileName : latest.TrackingDetail.FileName,
                                Content = (latest == null || (latest != null && latest.TrackingDetail == null)) ? resume.Content : latest.TrackingDetail.Content
                            };
                            dataHelper.AddEntity(detail);
                            if (latest != null)
                            {
                                dataHelper.DeleteUpdate(latest);
                            }
                            message = string.Format("{0}", TrackingTypes.AUTO_MATCHED.GetDescription().ToLower());
                            break;
                    }
                }
                else
                {
                    message = string.Format("already {0}", TrackingTypes.AUTO_MATCHED.GetDescription().ToLower());
                }
                dataHelper.Save();
            }
            return tracking;
        }
        /// <summary>
        /// Tracks job applications
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="JobId"></param>
        /// <param name="jobseekerId"></param>
        /// <param name="Username"></param>
        /// <param name="message"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'TrackingTypes' could not be found (are you missing a using directive or an assembly reference?)
        public Tracking Application(TrackingTypes Type, long JobId, long jobseekerId, string Username, out string message)
#pragma warning restore CS0246 // The type or namespace name 'TrackingTypes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
        {
            var type = (int)Type;
            Tracking tracking = null;
            var max_id = new Guid();
            message = string.Empty;
            Tracking latest = null;
            Hashtable parameters = new Hashtable();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var profile = dataHelper.GetSingle<UserProfile>("Username", Username);

                var list = dataHelper.Get<Tracking>().Where(x => x.UserId == profile.UserId && x.JobseekerId == jobseekerId).ToList();
                if (list.Count > 0)
                {
                    max_id = list.Max(x => x.Id);
                    latest = list.Where(x => x.Id == max_id).SingleOrDefault();
                }

                parameters = new Hashtable();
                parameters.Add("JobId", JobId);
                parameters.Add("JobseekerId", jobseekerId);
                parameters.Add("Type", type);
                parameters.Add("UserId", profile.UserId);
                parameters.Add("IsDeleted", false);

                tracking = dataHelper.GetSingle<Tracking>(parameters);
                if (tracking == null)
                {
                    tracking = new Tracking
                    {
                        Id = Guid.NewGuid(),
                        JobId = JobId,
                        JobseekerId = jobseekerId,
                        Type = type,
                        DateUpdated = DateTime.Now,
                        UserId = profile.UserId,
                        IsDownloaded = false
                    };

                    dataHelper.Add(tracking, Username);
                    if (Type != TrackingTypes.VIEWED && Type != TrackingTypes.DOWNLOADED)
                    {
                        var jobSeeker = dataHelper.GetSingle<UserProfile>("UserId", jobseekerId);
                        var detail = new TrackingDetail
                        {
                            Id = tracking.Id,
                            Title = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.Title : latest.TrackingDetail.Title,
                            CategoryId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.CategoryId.Value : latest.TrackingDetail.CategoryId,
                            SpecializationId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.SpecializationId.Value : latest.TrackingDetail.SpecializationId,
                            CountryId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.CountryId.Value : latest.TrackingDetail.CountryId,
                            StateId = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.StateId : latest.TrackingDetail.StateId,
                            FileName = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.FileName : latest.TrackingDetail.FileName,
                            Content = (latest == null || (latest != null && latest.TrackingDetail == null)) ? jobSeeker.Content : latest.TrackingDetail.Content
                        };
                        dataHelper.Add(detail);
                        if (latest != null)
                        {
                            dataHelper.Delete(latest);
                        }
                    }
                    message = string.Format("{0}", Type.GetDescription().ToLower());
                }
                else
                {
                    message = string.Format("already {0}", Type.GetDescription().ToLower());
                }
            }
            return tracking;
        }

        /// <summary>
        /// Updates tracking record
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Id"></param>
        /// <param name="Username"></param>
        /// <param name="message"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'TrackingTypes' could not be found (are you missing a using directive or an assembly reference?)
        public Tracking Update(TrackingTypes Type, Guid? Id, string Username, out string message)
#pragma warning restore CS0246 // The type or namespace name 'TrackingTypes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
        {
            var type = (int)Type;
            Tracking tracking = null;

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var profile = dataHelper.GetSingle<UserProfile>("Username", Username);
                var original = dataHelper.GetSingle<Tracking>(Id.Value);
                if (original != null)
                {
                    message = string.Empty;
                    var parameters = new Hashtable();

                    if (original.Jobseeker != null)
                    {
                        if (original.Job != null)
                        {
                            parameters.Add("JobId", original.JobId);
                        }
                        parameters.Add("JobseekerId", original.JobseekerId);
                        parameters.Add("Type", type);
                        parameters.Add("UserId", profile.UserId);
                        parameters.Add("IsDeleted", false);
                    }

                    tracking = dataHelper.GetSingle<Tracking>(parameters);
                    if (tracking == null)
                    {
                        tracking = new Tracking
                        {
                            Id = Guid.NewGuid(),
                            JobId = original.JobId,
                            ResumeId = original.ResumeId,
                            JobseekerId = original.JobseekerId,
                            Type = type,
                            DateUpdated = DateTime.Now,
                            UserId = profile.UserId,
                            IsDownloaded = false
                        };

                        dataHelper.AddEntity<Tracking>(tracking, Username);

                        if (Type != TrackingTypes.VIEWED && Type != TrackingTypes.DOWNLOADED)
                        {
                            TrackingDetail trackingDetails = new TrackingDetail();
                            if (original.Jobseeker != null)
                            {
                                trackingDetails = new TrackingDetail
                                {
                                    Id = tracking.Id,
                                    Title = original.TrackingDetail != null ? original.TrackingDetail.Title : original.Jobseeker.Title,
                                    CategoryId = original.TrackingDetail != null ? original.TrackingDetail.CategoryId : original.Jobseeker.CategoryId.Value,
                                    SpecializationId = original.TrackingDetail != null ? original.TrackingDetail.SpecializationId : original.Jobseeker.SpecializationId.Value,
                                    CountryId = original.TrackingDetail != null ? original.TrackingDetail.CountryId : original.Jobseeker.CountryId.Value,
                                    StateId = original.TrackingDetail != null ? original.TrackingDetail.StateId : original.Jobseeker.StateId,
                                    FileName = original.TrackingDetail != null ? original.TrackingDetail.FileName : original.Jobseeker.FileName,
                                    Content = original.TrackingDetail != null ? original.TrackingDetail.Content : original.Jobseeker.Content
                                };
                                dataHelper.AddEntity(trackingDetails);
                            }
                        }

                        dataHelper.DeleteUpdate(original);
                        message = string.Format("{0}", Type.GetDescription().ToLower());
                    }
                    else
                    {
                        message = string.Format("already {0}", Type.GetDescription().ToLower());
                    }
                }
                else
                {
                    message = string.Format("already {0}", Type.GetDescription().ToLower());
                }
                dataHelper.Save();
            }
            return tracking;
        }

#pragma warning disable CS0246 // The type or namespace name 'TrackingStatus' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'TrackingTypes' could not be found (are you missing a using directive or an assembly reference?)
        public TrackingStatus GetStatus(long userId, Guid? Id = null, TrackingTypes? type = null, long? jobId = null, long? jobseekerId = null)
#pragma warning restore CS0246 // The type or namespace name 'TrackingTypes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'TrackingStatus' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@Id", Id));
            parameters.Add(new SqlParameter("@Type", (int?)type));
            parameters.Add(new SqlParameter("@JobId", jobId));
            parameters.Add(new SqlParameter("@JobseekerId", jobseekerId));

            return ReadSingleData<TrackingStatus>("GetTrackingStatus", parameters);
        }
    }
}