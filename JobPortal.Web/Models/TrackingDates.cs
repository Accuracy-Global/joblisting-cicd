using System;
using System.Linq;
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)

namespace JobPortal.Data
{
    public class TrackingDates
    {
        public TrackingDates(long? jobId, long Id)
        {
            var automatched = (int) TrackingTypes.AUTO_MATCHED;
            var applied = (int) TrackingTypes.APPLIED;
            var shortlisted = (int) TrackingTypes.SHORTLISTED;
            var downloaded = (int) TrackingTypes.DOWNLOADED;
            var viewed = (int) TrackingTypes.VIEWED;
            var bookmarked = (int)TrackingTypes.BOOKMAKRED;

            var Track_Automatched = new Tracking();
            var Track_Applied = new Tracking();
            var Track_Shortlisted = new Tracking();
            var Track_Viewed = new Tracking();
            var Track_Bookmarked = new Tracking();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var track_list = dataHelper.Get<Tracking>().Where(x => x.JobId == jobId && (x.ResumeId == Id || x.JobseekerId == Id) && x.Type != downloaded && x.Type != viewed).ToList();
                if (track_list.Count > 0)
                {
                    Track_Automatched =
                        track_list.Where(x => x.JobId == jobId && (x.ResumeId == Id || x.JobseekerId == Id) && x.Type == automatched)
                            .OrderByDescending(x => x.DateUpdated)
                            .FirstOrDefault();
                    Track_Applied =
                        track_list.Where(x => x.JobId == jobId && (x.ResumeId == Id || x.JobseekerId == Id) && x.Type == applied)
                            .OrderByDescending(x => x.DateUpdated)
                            .FirstOrDefault();
                    Track_Shortlisted =
                        track_list.Where(x => x.JobId == jobId && (x.ResumeId == Id || x.JobseekerId == Id) && x.Type == shortlisted)
                            .OrderByDescending(x => x.DateUpdated)
                            .FirstOrDefault();
                    Track_Bookmarked =
                     track_list.Where(x => x.JobId == jobId && (x.ResumeId == Id || x.JobseekerId == Id) && x.Type == bookmarked)
                         .SingleOrDefault();


                    if (Track_Automatched != null)
                    {
                        Automatched = Track_Automatched.DateUpdated;
                        IsAutomatched = true;
                    }
                    else
                    {
                        Automatched = null;
                        IsAutomatched = false;
                    }

                    if (Track_Applied != null)
                    {
                        Applied = Track_Applied.DateUpdated;
                        IsApplied = true;
                    }
                    else
                    {
                        Applied = null;
                        IsApplied = false;
                    }

                    if (Track_Bookmarked != null)
                    {
                        Bookmarked = Track_Bookmarked.DateUpdated;
                        IsBookmarked = true;
                    }
                    else
                    {
                        Bookmarked = null;
                        IsBookmarked = false;
                    }

                    if (Track_Shortlisted != null)
                    {
                        Shortlisted = Track_Shortlisted.DateUpdated;
                        IsShortlisted = true;
                    }
                    else
                    {
                        Shortlisted = null;
                        IsShortlisted = false;
                    }
                }
            }
        }

        public TrackingDates(long Id)
        {
            var automatched = (int) TrackingTypes.AUTO_MATCHED;
            var bookmarked = (int) TrackingTypes.BOOKMAKRED;
            var shortlisted = (int) TrackingTypes.SHORTLISTED;
            var downloaded = (int) TrackingTypes.DOWNLOADED;
            var viewed = (int) TrackingTypes.VIEWED;

            var Track_Automatched = new Tracking();
            var Track_Bookmarked = new Tracking();
            var Track_Shortlisted = new Tracking();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var track_list = dataHelper.Get<Tracking>().Where(x => x.JobId == null && (x.ResumeId == Id || x.JobseekerId == Id) && x.Type != downloaded && x.Type != viewed).ToList();
                if (track_list.Count > 0)
                {
                    Track_Automatched =
                        track_list.Where(x => x.JobId == null && (x.ResumeId == Id || x.JobseekerId == Id) && x.Type == automatched)
                            .SingleOrDefault();
                    Track_Bookmarked =
                        track_list.Where(x => x.JobId == null && (x.ResumeId == Id || x.JobseekerId == Id) && x.Type == bookmarked)
                            .SingleOrDefault();
                    Track_Shortlisted =
                        track_list.Where(x => x.JobId == null && (x.ResumeId == Id || x.JobseekerId == Id) && x.Type == shortlisted)
                            .SingleOrDefault();

                    if (Track_Automatched != null)
                    {
                        Automatched = Track_Automatched.DateUpdated;
                        IsAutomatched = true;
                    }
                    else
                    {
                        Automatched = null;
                        IsAutomatched = false;
                    }

                    if (Track_Bookmarked != null)
                    {
                        Bookmarked = Track_Bookmarked.DateUpdated;
                        IsBookmarked = true;
                    }
                    else
                    {
                        Bookmarked = null;
                        IsBookmarked = false;
                    }

                    if (Track_Shortlisted != null)
                    {
                        Shortlisted = Track_Shortlisted.DateUpdated;
                        IsShortlisted = true;
                    }
                    else
                    {
                        Shortlisted = null;
                        IsShortlisted = false;
                    }
                }
            }
        }

        public TrackingDates()
        {
        }

        public bool IsAutomatched { get; set; }
        public bool IsApplied { get; set; }
        public bool IsBookmarked { get; set; }
        public bool IsShortlisted { get; set; }
        public DateTime? Automatched { get; set; }
        public DateTime? Applied { get; set; }
        public DateTime? Bookmarked { get; set; }
        public DateTime? Shortlisted { get; set; }

    }
}