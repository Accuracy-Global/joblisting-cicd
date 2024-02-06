using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Library.Enumerators
{
    public enum ActivityTypes
    {
        [Description("Photo Uploaded")]
        PHOTO_UPLODED,
        [Description("Photo Approved")]
        PHOTO_APPROVED,
        [Description("Photo Rejected")]
        PHOTO_REJECTED,
        [Description("Photo Deleted")]
        PHOTO_DELETED,

        [Description("Logo Uploaded")]
        LOGO_UPLODED,
        [Description("Logo Approved")]
        LOGO_APPROVED,
        [Description("Logo Rejected")]
        LOGO_REJECTED,
        [Description("Logo Deleted")]
        LOGO_DELETED,

        [Description("Job Listed")]
        JOB_LISTED,
        [Description("Job Approved")]
        JOB_APPROVED,
        [Description("Job Rejected")]
        JOB_REJECTED,
        [Description("Job Deleted")]
        JOB_DELETED,
        [Description("Job Deactivated")]
        JOB_DEACTIVATED,
        [Description("Job Activated")]
        JOB_ACTIVATED,

        [Description("Application Deleted")]
        APPLICATION_DELETED,
        [Description("Application Withdrawn")]
        APPLICATION_WITHDRAWN,

        [Description("Interview Withdrawn")]
        INTERVIEW_WITHDRAWN
    }
}
