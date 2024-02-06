using System;
using System.Linq;
#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)

namespace JobPortal.Domain
{
    public class DateService
    {
        private static volatile DateService instance;
        private static readonly object sync = new object();

        /// <summary>
        ///     Single Instance of DateService
        /// </summary>
        public static DateService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new DateService();
                        }
                    }
                }
                return instance;
            }
        }

        public DateTime AppliedOn(long JobId, long ResumeId)
        {
            var status = (int) TrackingTypes.APPLIED;

            var date = new DateTime();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                date = dataHelper.Get<Tracking>().Where(x => x.JobId == JobId && x.ResumeId == ResumeId && x.Type == status)
                        .Select(x => x.DateUpdated).SingleOrDefault();
            }
            return date;
        }

        public DateTime ShortlistedOn(long JobId, long ResumeId)
        {
            var status = (int) TrackingTypes.SHORTLISTED;

            var date = new DateTime();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                date = dataHelper.Get<Tracking>().Where(x => x.JobId == JobId && x.ResumeId == ResumeId && x.Type == status)
                        .Select(x => x.DateUpdated).SingleOrDefault();
            }
            return date;
        }

        public DateTime RejectedOn(long JobId, long ResumeId)
        {
            var status = (int) TrackingTypes.REJECTED;

            var date = new DateTime();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                date = dataHelper.Get<Tracking>().Where(x => x.JobId == JobId && x.ResumeId == ResumeId && x.Type == status)
                        .Select(x => x.DateUpdated).SingleOrDefault();
            }
            return date;
        }

        public DateTime DateWithdrawn(long JobId, long ResumeId)
        {
            var status = (int) TrackingTypes.WITHDRAWN;

            var date = new DateTime();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                date = dataHelper.Get<Tracking>().Where(x => x.JobId == JobId && x.ResumeId == ResumeId && x.Type == status)
                        .Select(x => x.DateUpdated).SingleOrDefault();
            }
            return date;
        }

        public DateTime SelectdOn(long JobId, long ResumeId)
        {
            var status = (int) TrackingTypes.SELECTED;

          var date = new DateTime();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                date = dataHelper.Get<Tracking>().Where(x => x.JobId == JobId && x.ResumeId == ResumeId && x.Type == status)
                        .Select(x => x.DateUpdated).SingleOrDefault();
            }
            return date;
        }
    }
}