using System.ComponentModel;

namespace JobPortal.Library.Enumerators
{
    public enum TrackingTypes
    {
        [Description("Applied")]
        APPLIED,
        [Description("Shortlisted")]
        SHORTLISTED,
        [Description("Rejected")]
        REJECTED,
        [Description("Selected")]
        SELECTED,
        [Description("On Hold")]
        ONHOLD,
        [Description("Pending")]
        PENDING,
        [Description("Withdrawn")]
        WITHDRAWN,
        [Description("Matched")]
        AUTO_MATCHED,
        [Description("Viewed")]
        VIEWED,
        [Description("Initiated")]
        INTERVIEW_INITIATED,
        [Description("Downloaded")]
        DOWNLOADED,
        [Description("Bookmarked")]
        BOOKMAKRED,
        [Description("In-progress")]
        INTERVIEW_IN_PROGRESS,
        [Description("Deleted")]
        DELETED,
        [Description("Activated")]
        ACTIVATED,
        [Description("Deactivated")]
        DEACTIVATED,
        [Description("Approved")]
        APPROVED,
        [Description("Published")]
        PUBLISHED,
        [Description("Withdrawn")]
        INTERVIEW_WITHDRAW,
    }
}
