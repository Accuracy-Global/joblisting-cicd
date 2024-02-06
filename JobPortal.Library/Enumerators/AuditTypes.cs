using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Library.Enumerators
{
    public enum AuditTypes
    {
        [Description("Logged In")]
        LOGGED_IN=1,
        [Description("Logged Out")]
        LOGGED_OUT,

        [Description("Online")]
        ON_LINE,
        [Description("Offline")]
        OFF_LINE,

        [Description("Profile Updated")]
        PROFILE_UPDATED,
        [Description("Profile Shared")]
        PROFILE_SHARED,
        [Description("Profile Searched")]
        PROFILE_SEARCHED,

        [Description("Image Uploaded")]
        PHOTO_UPLOADED,
        [Description("Image Approved")]
        PHOTO_APPROVED,
        [Description("Image Rejected")]
        PHOTO_REJECTED,
        [Description("Image Deleted")]
        PHOTO_DELETED,

        [Description("Resume Uploaded")]
        RESUME_UPLOADED,
        [Description("Resume Downloaded")]
        RESUME_DOWNLOADED,

        [Description("Job Posted")]
        JOB_POSTED,
        [Description("Job Approved")]
        JOB_APPROVED,
        [Description("Job Rejected")]
        JOB_REJECTED,
        [Description("Job Posted On Media")]
        JOB_POSTED_ON_MEDIA,
        [Description("Job Activated")]
        JOB_ACTIVATED,
        [Description("Job Deactivated")]
        JOB_DEACTIVATED,
        [Description("Job Deleted")]
        JOB_DELETED,
        [Description("Job Searched")]
        JOB_SEARCHED,
        [Description("Job Viewed")]
        JOB_VIEWED,
        [Description("Job Applied")]
        JOB_APPLIED,
        [Description("Job Matched")]
        JOB_MATCHED,
        [Description("Job Bookmarked")]
        JOB_BOOKMARKED,
        [Description("Job Shared")]
        JOB_SHARED,

        [Description("Jobseeker Matched")]
        JOBSEEKER_MATCHED,
        [Description("Jobseeker Searched")]
        JOBSEEKER_SEARCHED,
         [Description("Jobseeker Bookmarked")]
        JOBSEEKER_BOOKMARKED,

        [Description("Application Deleted")]
        APPLICATION_DELETED,
        [Description("Application Withdrawn")]
        APPLICATION_WITHDRAWN,
        [Description("Application Rejected")]
        APPLICATION_REJECTED,
        [Description("Application Selected")]
        APPLICATION_SELECTED,
        [Description("Application Shortlisted")]
        APPLICATION_SHORTLIST,

        [Description("Interview Initiated")]
        INTERVIEW_INITIATED,
        [Description("Interview Accepted")]
        INTERVIEW_ACCEPTED,
        [Description("Interview Rescheduled")]
        INTERVIEW_RESCHEDULED,
        [Description("Interview Selected")]
        INTERVIEW_SELECTED,
        [Description("Interview Rejected")]
        INTERVIEW_REJECTED,
        [Description("Interview Widthdrawn")]
        INTERVIEW_WITHDRAWN,

        [Description("Message Sent")]
        MESSAGE_SENT,
        [Description("Message Viewed")]
        MESSAGE_VIEWED,
        [Description("Message Deleted")]
        MESSAGE_DELETED,

        [Description("Invited Via Email")]
        INVITATION_EMAIL,
        [Description("Invited Via SMS")]
        INVITATION_SMS,
        [Description("Invitation Accepted")]
        INVITATION_ACCEPTED,
        [Description("Invitation Rejected")]
        INVITATION_REJECTED,

        [Description("Disconnected")]
        DISCONNECTED,
        [Description("Blocked")]
        BLOCKED,
        [Description("Unblocked")]
        UNBLOCKED,

        [Description("Announcement Created")]
        ANNOUNCEMENT_CREATED,
        [Description("Announcement Updated")]
        ANNOUNCEMENT_UPDATED,
        [Description("Announcement Deleted")]
        ANNOUNCEMENT_DELETED,
        [Description("Announcement Viewed")]
        ANNOUNCEMENT_VIEWED,

        [Description("Tip Created")]
        TIP_CREATED,
        [Description("Tip Updated")]
        TIP_UPDATED,
        [Description("Tip Deleted")]
        TIP_DELETED,
        [Description("Tip Viewed")]
        TIP_VIEWED,

        [Description("Password Changed")]
        PASSWORD_CHANGED,

        [Description("Password Reset")]
        PASSWORD_RESET,

        [Description("Email Sent")]
        EMAIL_SENT,
    }
}
