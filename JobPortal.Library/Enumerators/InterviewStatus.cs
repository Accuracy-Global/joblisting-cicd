using System.ComponentModel;

namespace JobPortal.Library.Enumerators
{
    public enum InterviewStatus
    {
        [Description("Initiated")]
        INITIATED,
        [Description("In-progress")]
        INTERVIEW_IN_PROGRESS,
        [Description("Completed")]
        COMPLETED,
        [Description("Selected")]
        SELECTED,
        [Description("Rejected")]
        REJECTED,
        [Description("Rescheduled")]
        RESCHEDULED,
        [Description("Withdrawn")]
        WITHDRAW,
    }
}
