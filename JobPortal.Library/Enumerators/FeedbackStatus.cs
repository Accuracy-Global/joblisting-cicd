using System.ComponentModel;

namespace JobPortal.Library.Enumerators
{
    public enum FeedbackStatus
    {
        [Description("Initiated")] INVITED,
        [Description("Rejected")] REJECTED,
        [Description("Schedule Accepted")] ACCEPTED,
        [Description("Rescheduled")] RESCHEDULE,
        [Description("Withdrawn")]
        WITHDRAW,
        [Description("Selected")]
        SELECTED,
    }
}