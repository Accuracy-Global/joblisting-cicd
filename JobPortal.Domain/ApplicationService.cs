#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain
{
#pragma warning disable CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    public class ApplicationService : DataService
#pragma warning restore CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    {
        private static volatile ApplicationService instance;
        private static readonly object sync = new object();

        /// <summary>
        ///     Default private constructor.
        /// </summary>
        private ApplicationService()
        {

        }

        /// <summary>
        /// Single Instance of ApplicationService
        /// </summary>
        public static ApplicationService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new ApplicationService();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Gets the list of application
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="jobseekerId"></param>
        /// <param name="employerId"></param>
        /// <param name="jobTitle"></param>
        /// <param name="company"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'ExtendedTrackingEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<ExtendedTrackingEntity> GetList(long? jobId, long? jobseekerId, long? employerId, string jobTitle = null, string company = null, DateTime? start = null, DateTime? end = null, int pageNumber = 0, int pageSize = 10)
#pragma warning restore CS0246 // The type or namespace name 'ExtendedTrackingEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@JobId", jobId));
            parameters.Add(new SqlParameter("@JobseekerId", jobseekerId));
            parameters.Add(new SqlParameter("@EmployerId", employerId));
            parameters.Add(new SqlParameter("@JobTitle", jobTitle));
            parameters.Add(new SqlParameter("@Company", company));
            parameters.Add(new SqlParameter("@Start", start));
            parameters.Add(new SqlParameter("@End", end));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));
            parameters.Add(new SqlParameter("@PageSize", pageSize));

            return ReadData<ExtendedTrackingEntity>("JobApplicationList", parameters);
        }

        /// <summary>
        /// Send application for the job
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="jobseekerId"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'TrackingStatus' could not be found (are you missing a using directive or an assembly reference?)
        public TrackingStatus Apply(long jobId, long jobseekerId)
#pragma warning restore CS0246 // The type or namespace name 'TrackingStatus' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@JobId", jobId));
            parameters.Add(new SqlParameter("@JobseekerId", jobseekerId));

            return ReadSingleData<TrackingStatus>("ApplyOnJob", parameters);
        }

        /// <summary>
        /// Resend application for the job
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="jobId"></param>
        /// <param name="jobseekerId"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'TrackingStatus' could not be found (are you missing a using directive or an assembly reference?)
        public TrackingStatus Reapply(Guid Id, long jobId, long jobseekerId)
#pragma warning restore CS0246 // The type or namespace name 'TrackingStatus' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@Id", Id));
            parameters.Add(new SqlParameter("@JobId", jobId));
            parameters.Add(new SqlParameter("@JobseekerId", jobseekerId));

            return ReadSingleData<TrackingStatus>("ReapplyOnJob", parameters);
        }

        /// <summary>
        /// Reject's application received for the job
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="employerId"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'TrackingStatus' could not be found (are you missing a using directive or an assembly reference?)
        public TrackingStatus Reject(Guid Id, long employerId)
#pragma warning restore CS0246 // The type or namespace name 'TrackingStatus' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@Id", Id));
            parameters.Add(new SqlParameter("@UserId", employerId));

            return ReadSingleData<TrackingStatus>("RejectJobApplication", parameters);
        }

        /// <summary>
        /// Withdraw's application from the job
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="jobseekerId"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'TrackingStatus' could not be found (are you missing a using directive or an assembly reference?)
        public TrackingStatus Withdraw(Guid Id, long jobseekerId)
#pragma warning restore CS0246 // The type or namespace name 'TrackingStatus' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@Id", Id));
            parameters.Add(new SqlParameter("@UserId", jobseekerId));

            return ReadSingleData<TrackingStatus>("WithdrawJobApplication", parameters);
        }
    }
}
