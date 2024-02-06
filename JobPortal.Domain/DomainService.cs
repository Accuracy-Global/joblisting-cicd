#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
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
    public class DomainService : DataService
#pragma warning restore CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    {
        private static volatile DomainService instance;
        private static readonly object sync = new object();

        private DomainService()
        {
        }

        /// <summary>
        /// Single Instance of JobPortalService
        /// </summary>
        public static DomainService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new DomainService();
                        }
                    }
                }
                return instance;
            }
        }
        public long GetAdminUserId()
        {
            return ReadDataField<long>("SELECT TOP 1 UserId FROM userprofiles WHERE [Type] IN(1,2)");
        }

        public string GetAdminUsername()
        {
            return ReadDataField<string>("SELECT TOP 1 Username FROM userprofiles WHERE [Type] IN(1,2)");
        }

#pragma warning disable CS0246 // The type or namespace name 'AuditEntity' could not be found (are you missing a using directive or an assembly reference?)
        public int ManageAudit(AuditEntity model)
#pragma warning restore CS0246 // The type or namespace name 'AuditEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            int stat = 0;
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@Type", model.Type));
            parameters.Add(new SqlParameter("@Description", model.Description));
            parameters.Add(new SqlParameter("@UserId", model.UserId));
            parameters.Add(new SqlParameter("@Browser", model.Browser));
            parameters.Add(new SqlParameter("@IPAddress", model.IPAddress));
            parameters.Add(new SqlParameter("@Failed", model.Failed));

            // Optional parameters
            if (model.Reference != null)
            {
                parameters.Add(new SqlParameter("@Reference", model.Reference));
            }
            if (!string.IsNullOrEmpty(model.Comments))
            {
                parameters.Add(new SqlParameter("@Comments", model.Comments));
            }

            stat = HandleData("ManageSystemAudit", parameters);
            return stat;
        }
        public int ManageLoginHistory(string username, string ipAddress, string browser, string comments)
        {
            int stat = 0;
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@Username", username));
            parameters.Add(new SqlParameter("@Browser", browser));
            parameters.Add(new SqlParameter("@IPAddress", ipAddress));

            if (!string.IsNullOrEmpty(comments))
            {
                parameters.Add(new SqlParameter("@Comments", comments));
            }
            stat = HandleData("ManageLoginHistory", parameters);
            return stat;
        }

        public int ManageOnlineUser(long userId, int status, DateTime onlineSince)
        {
            int stat = 0;
            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Add(new SqlParameter("@Id", userId));
            parameters.Add(new SqlParameter("@Status", status));
            parameters.Add(new SqlParameter("@OnlineSince", onlineSince));

            stat = HandleData("ManageOnlineUser", parameters);
            return stat;
        }

#pragma warning disable CS0246 // The type or namespace name 'ImageEntity' could not be found (are you missing a using directive or an assembly reference?)
        public ImageEntity GetPhoto(long? id)
#pragma warning restore CS0246 // The type or namespace name 'ImageEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            ImageEntity image = null;
            if (id != null)
            {
                image = ReadSingleData<ImageEntity>(string.Format("SELECT [Id], [UserId], [Image], [Type], [Area], [DateUpdated], [IsApproved], [IsRejected], [Reason], [IsDeleted], [NewImage], [ImageSize], [NewImageSize], [InEditMode] FROM Photos WHERE Id = {0}", id));
            }
            return image;
        }

#pragma warning disable CS0246 // The type or namespace name 'LocationEntity' could not be found (are you missing a using directive or an assembly reference?)
        public LocationEntity GetLocation(string ipAddress)
#pragma warning restore CS0246 // The type or namespace name 'LocationEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            LocationEntity model = null;
            if (!string.IsNullOrEmpty(ipAddress))
            {
                List<DbParameter> parameters = new List<DbParameter>();
                parameters.Add(new SqlParameter("@IPAddress", ipAddress));
                model = ReadSingleData<LocationEntity>("GetCountryFromIP", parameters);
            }
            return model;
        }

#pragma warning disable CS0246 // The type or namespace name 'LoginHistoryEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<LoginHistoryEntity> GetLoginHistory(string username, int pageSize = 10, int pageNumber = 1)
#pragma warning restore CS0246 // The type or namespace name 'LoginHistoryEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<LoginHistoryEntity> list = new List<LoginHistoryEntity>();
            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Add(new SqlParameter("@Username", username));
            parameters.Add(new SqlParameter("@PageSize", pageSize));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));

            list = ReadData<LoginHistoryEntity>("GetLoginHistory", parameters);

            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'LoginHistory' could not be found (are you missing a using directive or an assembly reference?)
        public LoginHistory GetLastLoginHistory(string username)
#pragma warning restore CS0246 // The type or namespace name 'LoginHistory' could not be found (are you missing a using directive or an assembly reference?)
        {
            LoginHistory last = new LoginHistory();
            last = ReadSingleData<LoginHistory>(string.Format("SELECT TOP 1 Id, LoginDateTime, Username, IPAddress, Browser, Comments FROM LoginHistories WHERE LTRIM(RTRIM(LOWER(Username))) = LTRIM(RTRIM(LOWER('{0}'))) ORDER BY LoginDateTime DESC", username));
            return last;
        }

#pragma warning disable CS0246 // The type or namespace name 'JobsByCountryEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<JobsByCountryEntity> GetJobsByCountry(long? countryId = null, bool? pending = null, DateTime? start = null, DateTime? end = null, int pageSize = 111, int pageNumber = 1)
#pragma warning restore CS0246 // The type or namespace name 'JobsByCountryEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<JobsByCountryEntity> list = new List<JobsByCountryEntity>();
            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Add(new SqlParameter("@CountryId", countryId));
            parameters.Add(new SqlParameter("@StartDate", start));
            parameters.Add(new SqlParameter("@EndDate", end));
            parameters.Add(new SqlParameter("@Pending", pending));
            parameters.Add(new SqlParameter("@PageSize", pageSize));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));

            list = ReadData<JobsByCountryEntity>("GetJobByCountry", parameters);
            return list;
        }
#pragma warning disable CS0246 // The type or namespace name 'JobsByCountryEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<JobsByCountryEntity> GetJobsByCountry(long? countryId = null, string status = null, bool? pending = null, DateTime? start = null, DateTime? end = null, int pageSize = 111, int pageNumber = 1)
#pragma warning restore CS0246 // The type or namespace name 'JobsByCountryEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<JobsByCountryEntity> list = new List<JobsByCountryEntity>();
            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Add(new SqlParameter("@CountryId", countryId));
            parameters.Add(new SqlParameter("@StartDate", start));
            parameters.Add(new SqlParameter("@EndDate", end));
            parameters.Add(new SqlParameter("@Status", status));
            parameters.Add(new SqlParameter("@Pending", pending));
            parameters.Add(new SqlParameter("@PageSize", pageSize));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));

            list = ReadData<JobsByCountryEntity>("GetJobByCountry", parameters);
            return list;
        }
#pragma warning disable CS0246 // The type or namespace name 'JobDetailByCountryEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<JobDetailByCountryEntity> GetJobListByCountry(long? countryId = null, string name = null, bool? pending = null, DateTime? start = null, DateTime? end = null, int pageSize = 10, int pageNumber = 1)
#pragma warning restore CS0246 // The type or namespace name 'JobDetailByCountryEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<JobDetailByCountryEntity> list = new List<JobDetailByCountryEntity>();
            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Add(new SqlParameter("@CountryId", countryId));
            parameters.Add(new SqlParameter("@Name", name));
            parameters.Add(new SqlParameter("@StartDate", start));
            parameters.Add(new SqlParameter("@EndDate", end));
            parameters.Add(new SqlParameter("@Pending", pending));
            parameters.Add(new SqlParameter("@PageSize", pageSize));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));

            list = ReadData<JobDetailByCountryEntity>("GetJobListByCountry", parameters);
            return list;
        }



#pragma warning disable CS0246 // The type or namespace name 'JobStatus' could not be found (are you missing a using directive or an assembly reference?)
        public int GetJobCount(JobStatus status)
#pragma warning restore CS0246 // The type or namespace name 'JobStatus' could not be found (are you missing a using directive or an assembly reference?)
        {
            int count = 0;
            StringBuilder query = new StringBuilder();

            switch (status)
            {
                case JobStatus.ACTIVE:
                    query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 0 AND IsRejected = 0 AND IsActive = 1 AND IsPublished = 1 AND CAST(ClosingDate AS DATE) >= CAST(GETDATE() AS DATE)");
                    break;
                case JobStatus.INACTIVE:
                    query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 0 AND IsActive = 0");
                    break;
                case JobStatus.EXPIRED:
                    query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 0 AND IsRejected = 0 AND IsActive = 1 AND IsPublished = 1 AND CAST(ClosingDate AS DATE) < CAST(GETDATE() AS DATE)");
                    break;
                case JobStatus.IN_APPROVAL:
                    query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 0 AND IsRejected = 0 AND IsActive = 1 AND IsPublished = 0 AND CAST(ClosingDate AS DATE) >= CAST(GETDATE() AS DATE)");
                    break;
                case JobStatus.REJECTED:
                    query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 0 AND IsRejected = 1 AND IsPublished = 0 AND CAST(ClosingDate AS DATE) >= CAST(GETDATE() AS DATE)");
                    break;
                case JobStatus.DELETED:
                    query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 1");
                    break;

            }

            count = Convert.ToInt32(ReadDataField(query.ToString()));
            return count;
        }

#pragma warning disable CS0246 // The type or namespace name 'SecurityRoles' could not be found (are you missing a using directive or an assembly reference?)
        public int GetPhotoCount(SecurityRoles role)
#pragma warning restore CS0246 // The type or namespace name 'SecurityRoles' could not be found (are you missing a using directive or an assembly reference?)
        {
            int count = 0;
            StringBuilder query = new StringBuilder();

            switch (role)
            {
                case SecurityRoles.Jobseeker:
                    query.Append("SELECT COUNT(1) FROM Photos WHERE IsDeleted = 0 AND IsRejected = 0 AND IsApproved = 0 AND UserId IN (SELECT UserId FROM UserProfiles WHERE [Type]= 4)");
                    break;
                case SecurityRoles.Employers:
                    query.Append("SELECT COUNT(1) FROM Photos WHERE IsDeleted = 0 AND IsRejected = 0 AND IsApproved = 0 AND UserId IN (SELECT UserId FROM UserProfiles WHERE [Type]= 5)");
                    break;
            }

            count = Convert.ToInt32(ReadDataField(query.ToString()));
            return count;
        }

#pragma warning disable CS0246 // The type or namespace name 'JobStatus' could not be found (are you missing a using directive or an assembly reference?)
        public int GetJobCount1(JobStatus status,int countryId)
#pragma warning restore CS0246 // The type or namespace name 'JobStatus' could not be found (are you missing a using directive or an assembly reference?)
        {
            
            int count = 0;
            StringBuilder query = new StringBuilder();
            if (countryId == 0)
            {
                switch (status)
                {
                    case JobStatus.ACTIVE:
                        query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 0 AND IsRejected = 0 AND IsActive = 1 AND IsPublished = 1 AND CAST(ClosingDate AS DATE) >= CAST(GETDATE() AS DATE)");
                        break;
                    case JobStatus.INACTIVE:
                        query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 0 AND IsActive = 0");
                        break;
                    case JobStatus.EXPIRED:
                        query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 0 AND IsRejected = 0 AND IsActive = 1 AND IsPublished = 1 AND CAST(ClosingDate AS DATE) < CAST(GETDATE() AS DATE)");
                        break;
                    case JobStatus.IN_APPROVAL:
                        query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 0 AND IsRejected = 0 AND IsActive = 1 AND IsPublished = 0 AND CAST(ClosingDate AS DATE) >= CAST(GETDATE() AS DATE)");
                        break;
                    case JobStatus.REJECTED:
                        query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 0 AND IsRejected = 1 AND IsPublished = 0 AND CAST(ClosingDate AS DATE) >= CAST(GETDATE() AS DATE)");
                        break;
                    case JobStatus.DELETED:
                        query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 1");
                        break;

                }
            }
            else
            {
                switch (status)
                {
                    case JobStatus.ACTIVE:
                        query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 0 AND IsRejected = 0 AND IsActive = 1 AND IsPublished = 1 and CountryId="+ countryId + " AND CAST(ClosingDate AS DATE) >= CAST(GETDATE() AS DATE)");
                        break;
                    case JobStatus.INACTIVE:
                        query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 0 AND IsActive = 0 and CountryId=" + countryId);
                        break;
                    case JobStatus.EXPIRED:
                        query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 0 AND IsRejected = 0 AND IsActive = 1 AND IsPublished = 1 and CountryId=" + countryId + " AND CAST(ClosingDate AS DATE) < CAST(GETDATE() AS DATE)");
                        break;
                    case JobStatus.IN_APPROVAL:
                        query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 0 AND IsRejected = 0 AND IsActive = 1 AND IsPublished = 0 and CountryId=" + countryId + " AND CAST(ClosingDate AS DATE) >= CAST(GETDATE() AS DATE)");
                        break;
                    case JobStatus.REJECTED:
                        query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 0 AND IsRejected = 1 AND IsPublished = 0 and CountryId=" + countryId + " AND CAST(ClosingDate AS DATE) >= CAST(GETDATE() AS DATE)");
                        break;
                    case JobStatus.DELETED:
                        query.Append("SELECT COUNT(1) FROM Jobs WHERE IsDeleted = 1 and CountryId="+ countryId);
                        break;

                }
            }
            count = Convert.ToInt32(ReadDataField(query.ToString()));
            return count;
        }

#pragma warning disable CS0246 // The type or namespace name 'SecurityRoles' could not be found (are you missing a using directive or an assembly reference?)
        public int GetPhotoCount1(SecurityRoles role,int countryId)
#pragma warning restore CS0246 // The type or namespace name 'SecurityRoles' could not be found (are you missing a using directive or an assembly reference?)
        {
            int count = 0;
            StringBuilder query = new StringBuilder();
            if (countryId == 0)
            {
                switch (role)
                {
                    case SecurityRoles.Jobseeker:
                        query.Append("SELECT COUNT(1) FROM Photos WHERE IsDeleted = 0 AND IsRejected = 0 AND IsApproved = 0 AND UserId IN (SELECT UserId FROM UserProfiles WHERE [Type]= 4)");
                        break;
                    case SecurityRoles.Employers:
                        query.Append("SELECT COUNT(1) FROM Photos WHERE IsDeleted = 0 AND IsRejected = 0 AND IsApproved = 0 AND UserId IN (SELECT UserId FROM UserProfiles WHERE [Type]= 5)");
                        break;
                }
            }
            else
            {
                switch (role)
                {
                    case SecurityRoles.Jobseeker:
                        query.Append("SELECT COUNT(1) FROM Photos WHERE IsDeleted = 0 AND IsRejected = 0 AND IsApproved = 0 AND UserId IN (SELECT UserId FROM UserProfiles WHERE [Type]= 4 and CountryId="+countryId+")");
                        break;
                    case SecurityRoles.Employers:
                        query.Append("SELECT COUNT(1) FROM Photos WHERE IsDeleted = 0 AND IsRejected = 0 AND IsApproved = 0 AND UserId IN (SELECT UserId FROM UserProfiles WHERE [Type]= 5 and CountryId="+countryId+")");
                        break;
                }
            }
            count = Convert.ToInt32(ReadDataField(query.ToString()));
            return count;
        }

#pragma warning disable CS0246 // The type or namespace name 'ApplicationEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<ApplicationEntity> GetJobAppList(long? Id, long? UserId, long? countryId, string JobTitle = null, string Company = null, DateTime? StartDate = null, DateTime? EndDate = null, int pageSize = 10, int pageNumber = 1)
#pragma warning restore CS0246 // The type or namespace name 'ApplicationEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<ApplicationEntity> list = new List<ApplicationEntity>();
            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Add(new SqlParameter("@UserId", UserId));
            parameters.Add(new SqlParameter("@JobId", Id));
            parameters.Add(new SqlParameter("@JobTitle", JobTitle));
            parameters.Add(new SqlParameter("@CountryId", countryId));
            parameters.Add(new SqlParameter("@Company", Company));
            parameters.Add(new SqlParameter("@StartDate", StartDate));
            parameters.Add(new SqlParameter("@EndDate", EndDate));
            parameters.Add(new SqlParameter("@PageSize", pageSize));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));

            list = ReadData<ApplicationEntity>("GetJobAppList", parameters);
            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'JobEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<JobEntity> GetJobList(long employerId, string status, long? countryId, DateTime? StartDate = null, DateTime? EndDate = null, int pageSize = 10, int pageNumber = 1)
#pragma warning restore CS0246 // The type or namespace name 'JobEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<JobEntity> list = new List<JobEntity>();
            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Add(new SqlParameter("@UserId", employerId));
            parameters.Add(new SqlParameter("@CountryId", countryId));
            parameters.Add(new SqlParameter("@Status", status));
            parameters.Add(new SqlParameter("@StartDate", StartDate));
            parameters.Add(new SqlParameter("@EndDate", EndDate));
            parameters.Add(new SqlParameter("@PageSize", pageSize));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));

            list = ReadData<JobEntity>("GetJobList", parameters);
            return list;
        }

        public int GetApplicationStatus(long jobId, long jobseekerId)
        {
            int count = 0;
            object ob = ReadDataField(string.Format("SELECT COUNT(1) FROM Trackings WHERE [Type] IN(0, 7) AND JobId={0} AND JobseekerId = {1} AND IsDeleted = 0", jobId, jobseekerId));
            if (ob != null)
            {
                count = Convert.ToInt32(ob);
            }
            return count;
        }

#pragma warning disable CS0246 // The type or namespace name 'GlobalInboxEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<GlobalInboxEntity> GetGlobalInboxList(long? countryId = null, DateTime? start = null, DateTime? end = null, int pageSize = 111, int pageNumber = 1)
#pragma warning restore CS0246 // The type or namespace name 'GlobalInboxEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<GlobalInboxEntity> list = new List<GlobalInboxEntity>();
            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Add(new SqlParameter("@CountryId", countryId));
            parameters.Add(new SqlParameter("@Type", null));
            parameters.Add(new SqlParameter("@StartDate", start));
            parameters.Add(new SqlParameter("@EndDate", end));
            parameters.Add(new SqlParameter("@PageSize", pageSize));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));

            list = ReadData<GlobalInboxEntity>("ListGlobalInbox", parameters);

            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'InboxEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<InboxEntity> GetInboxList(long countryId, int type = 0, DateTime? start = null, DateTime? end = null, string name = null, int pageSize = 111, int pageNumber = 1)
#pragma warning restore CS0246 // The type or namespace name 'InboxEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<InboxEntity> list = new List<InboxEntity>();
            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Add(new SqlParameter("@CountryId", countryId));
            parameters.Add(new SqlParameter("@Type", type));
            parameters.Add(new SqlParameter("@Name", name));
            parameters.Add(new SqlParameter("@StartDate", start));
            parameters.Add(new SqlParameter("@EndDate", end));
            parameters.Add(new SqlParameter("@PageSize", pageSize));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));

            list = ReadData<InboxEntity>("ListGlobalInbox", parameters);

            return list;
        }

        public int InboxItemCounts(long? userId = null, long? referenceId = null, long? id = null)
        {
            int count = 0;
            object cnt = null;
            string innerQuery = "SELECT B.Id InboxId,  ReceiverId, ReferenceId, (SELECT CountryId FROM UserProfiles WHERE UserId = B.ReceiverId) CountryId  FROM Inbox B WHERE Unread = 1";
            if (id.HasValue)
            {
                if (id.Value > 0)
                {
                    innerQuery += string.Format(" AND ParentId = {0}", id.Value);
                }
                else if (id == 0)
                {
                    innerQuery += string.Format(" AND ParentId > 0", id.Value);
                }
            }
            else
            {
                innerQuery += string.Format(" AND ParentId IS NULL");
            }
            string query = string.Format("SELECT COUNT(InboxId) Total FROM ({0}) A INNER JOIN Lists B ON B.Id = A.CountryId", innerQuery);

            if (userId.HasValue == true && referenceId.HasValue == true)
            {
                query += string.Format(" WHERE A.ReceiverId = {0} AND A.ReferenceId = {1}", userId.Value, referenceId.Value);
            }
            else if (userId.HasValue == false && referenceId.HasValue == true)
            {
                query += string.Format(" WHERE A.ReferenceId = {0}", referenceId.Value);
            }
            else if (userId.HasValue == true && referenceId.HasValue == false)
            {
                query += string.Format(" WHERE A.ReceiverId = {0}", userId.Value);
            }

            cnt = ReadDataField(query);

            if (cnt != null)
            {
                count = Convert.ToInt32(cnt);
            }
            return count;
        }

        public int DashboardInboxItems(long receiverId)
        {
            int count = 0;
            object cnt = null;
            cnt = ReadDataField(string.Format("SELECT COUNT(*) FROM Inbox WHERE ReceiverId = {0} AND Unread = 1", receiverId));

            if (cnt != null)
            {
                count = Convert.ToInt32(cnt);
            }
            return count;
        }

        public int DashboardInboxItems(long receiverId, long senderId)
        {
            int count = 0;
            object cnt = null;
            cnt = ReadDataField(string.Format("SELECT COUNT(*) FROM Inbox WHERE ReceiverId = {0} AND SenderId = {1} AND Unread = 1", receiverId, senderId));

            if (cnt != null)
            {
                count = Convert.ToInt32(cnt);
            }
            return count;
        }

        public int DashboardInboxItems(long receiverId, long senderId, long ReferenceId, long? inboxId = null)
        {
            int count = 0;
            object cnt = null;
            if (inboxId == null)
            {
                cnt = ReadDataField(string.Format("SELECT COUNT(*) FROM Inbox WHERE ReceiverId = {0} AND SenderId = {1} AND ReferenceId = {2} AND Unread = 1", receiverId, senderId, ReferenceId));
            }
            else
            {
                cnt = ReadDataField(string.Format("SELECT COUNT(*) FROM Inbox WHERE ReceiverId = {0} AND SenderId = {1} AND ReferenceId = {2} AND Unread = 1 AND ParentId = {3}", receiverId, senderId, ReferenceId, inboxId.Value));
            }
            if (cnt != null)
            {
                count = Convert.ToInt32(cnt);
            }
            return count;
        }

        public int InboxItemFileCounts(long itemId)
        {
            int count = 0;
            object cnt = ReadDataField(string.Format("SELECT COUNT(Id) FROM InboxItems WHERE InboxId = {0}", itemId));
            if (cnt != null)
            {
                count = Convert.ToInt32(cnt);
            }
            return count;
        }

#pragma warning disable CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        public long ManageInbox(Inbox model)
#pragma warning restore CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Add(new SqlParameter("@Id", model.Id));
            parameters.Add(new SqlParameter("@SenderId", model.SenderId));
            parameters.Add(new SqlParameter("@ReceiverId", model.ReceiverId));
            parameters.Add(new SqlParameter("@ParentId", model.ParentId));
            parameters.Add(new SqlParameter("@ReferenceId", model.ReferenceId));
            parameters.Add(new SqlParameter("@ReferenceType", model.ReferenceType));
            parameters.Add(new SqlParameter("@Subject", model.Subject));
            parameters.Add(new SqlParameter("@Body", model.Body));
            parameters.Add(new SqlParameter("@Unread", model.Unread));
            object return_val = ReadDataField("ManageInbox", parameters);
            long id = 0;
            if (return_val != DBNull.Value)
            {
                id = Convert.ToInt64(return_val);
            }
            return id;
        }

#pragma warning disable CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        public Inbox GetInboxItem(long Id)
#pragma warning restore CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@Id", Id));

            return ReadSingleData<Inbox>("GetInboxItem", parameters);
        }
#pragma warning disable CS0246 // The type or namespace name 'Inboxv1' could not be found (are you missing a using directive or an assembly reference?)
        public List<Inboxv1> VerUnVer1(long UserId, string ed, string sc, string ft, string tt)
#pragma warning restore CS0246 // The type or namespace name 'Inboxv1' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadData<Inboxv1>(string.Format("SELECT UserId Id,UserId ParentId,UserId as  ReferenceId,2 as ReferenceType,'Education Verfication at Joblisting' as [Subject],[Subject] as  Body, UserId as SenderId,Education as Unread,SCName as SenderName,UserId ReceiverId,Email as  ReceiverName,[To] as  ReceiverName1, DateUpdated, '' as JobTitle, '' as JobUrl, [From] as [Messages],  100 MaxRows FROM [UserEducation_Verify] where UserId={0} and Education='{1}' and SCName='{2}' and [From]='{3}' and [To]='{4}' and Status='Verified' ORDER BY DateUpdated DESC", UserId,ed,sc,ft,tt));
        }
#pragma warning disable CS0246 // The type or namespace name 'Inboxv' could not be found (are you missing a using directive or an assembly reference?)
        public List<Inboxv> VerUnVer(long UserId,string com,string ind,string con)
#pragma warning restore CS0246 // The type or namespace name 'Inboxv' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadData<Inboxv>(string.Format("SELECT UserId Id,UserId ParentId,UserId as  ReferenceId,2 as ReferenceType,'Employement Verfication at Joblisting' as [Subject],[Subject] as  Body, UserId as SenderId,Company as Unread,Country as SenderName,UserId ReceiverId,Email as  ReceiverName, DateUpdated, '' as JobTitle, '' as JobUrl, Industry as [Messages],  100 MaxRows FROM [User_Verify] where UserId={0} and Company='{1}' and Industry='{2}' and Country='{3}' and Status='Verified' ORDER BY DateUpdated DESC", UserId,com,ind,con));
        }
#pragma warning disable CS0246 // The type or namespace name 'Inboxv' could not be found (are you missing a using directive or an assembly reference?)
        public Inboxv ListVerify(string uname)
#pragma warning restore CS0246 // The type or namespace name 'Inboxv' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadSingleData<Inboxv>(string.Format("SELECT UserId Id,UserId ParentId,UserId as  ReferenceId,2 as ReferenceType,'Employment Verfication at Joblisting' as [Subject],[Subject] as  Body, UserId as SenderId,Company as Unread,Country as SenderName,UserId ReceiverId,Email as  ReceiverName, DateUpdated, '' as JobTitle, '' as JobUrl, Industry as [Messages],  100 MaxRows FROM [User_Verify] where Lower(Email)='{0}' and Status='Verify' ORDER BY DateUpdated DESC", uname));
        }
#pragma warning disable CS0246 // The type or namespace name 'Inboxv' could not be found (are you missing a using directive or an assembly reference?)
        public List<Inboxv> vList(string uname)
#pragma warning restore CS0246 // The type or namespace name 'Inboxv' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadData<Inboxv>(string.Format("SELECT UserId Id,UserId ParentId,UserId as  ReferenceId,2 as ReferenceType,'Employment Verfication at Joblisting' as [Subject],[Subject] as  Body, UserId as SenderId,Company as Unread,Country as SenderName,UserId ReceiverId,Email as  ReceiverName, DateUpdated, '' as JobTitle, '' as JobUrl, Industry as [Messages],  100 MaxRows FROM [User_Verify] where Lower(Email)='{0}' and Status='Verify' ORDER BY DateUpdated DESC", uname));
        }
#pragma warning disable CS0246 // The type or namespace name 'Inboxv1' could not be found (are you missing a using directive or an assembly reference?)
        public List<Inboxv1> vList1(string uname)
#pragma warning restore CS0246 // The type or namespace name 'Inboxv1' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadData<Inboxv1>(string.Format("SELECT UserId Id,UserId ParentId,UserId as  ReferenceId,2 as ReferenceType,'Education Verfication at Joblisting' as [Subject],[Subject] as  Body, UserId as SenderId,Education as Unread,SCName as SenderName,UserId ReceiverId,Email as  ReceiverName,[To] as  ReceiverName1, DateUpdated, '' as JobTitle, '' as JobUrl, [From] as [Messages],  100 MaxRows FROM [UserEducation_Verify] where Lower(Email)='{0}' and Status='Verify' ORDER BY DateUpdated DESC", uname));
        }
#pragma warning disable CS0246 // The type or namespace name 'Inboxv1' could not be found (are you missing a using directive or an assembly reference?)
        public Inboxv1 ListVerify1(string uname)
#pragma warning restore CS0246 // The type or namespace name 'Inboxv1' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadSingleData<Inboxv1>(string.Format("SELECT UserId Id,UserId ParentId,UserId as  ReferenceId,2 as ReferenceType,'Education Verfication at Joblisting' as [Subject],[Subject] as  Body, UserId as SenderId,Education as Unread,SCName as SenderName,UserId ReceiverId,Email as  ReceiverName,[To] as  ReceiverName1, DateUpdated, '' as JobTitle, '' as JobUrl, [From] as [Messages],  100 MaxRows FROM [UserEducation_Verify] where Lower(Email)='{0}' and Status='Verify' ORDER BY DateUpdated DESC", uname));
        }
#pragma warning disable CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        public List<Inbox> ListInbox(long? senderId = null, long? receiverId = null, bool? unread = null, int pageNumber = 1)
#pragma warning restore CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Add(new SqlParameter("@SenderId", senderId));
            parameters.Add(new SqlParameter("@Unread", unread));
            parameters.Add(new SqlParameter("@ReceiverId", receiverId));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));

            return ReadData<Inbox>("ListInbox", parameters);
        }
       
#pragma warning disable CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        public List<Inbox> ListChildInbox(long? parentId = null)
#pragma warning restore CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@ParentId", parentId));

            return ReadData<Inbox>("ListChildInbox", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'InboxFile' could not be found (are you missing a using directive or an assembly reference?)
        public int ManageInboxItem(InboxFile model)
#pragma warning restore CS0246 // The type or namespace name 'InboxFile' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Add(new SqlParameter("@Id", model.Id));
            parameters.Add(new SqlParameter("@InboxId", model.InboxId));
            parameters.Add(new SqlParameter("@Name", model.Name));
            parameters.Add(new SqlParameter("@Data", model.Data));
            parameters.Add(new SqlParameter("@Size", model.Size));

            return HandleData("ManageInboxItems", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'InboxFile' could not be found (are you missing a using directive or an assembly reference?)
        public List<InboxFile> ListInboxItem(long inboxId)
#pragma warning restore CS0246 // The type or namespace name 'InboxFile' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@InboxId", inboxId));

            return ReadData<InboxFile>("ListInboxItems", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'InboxFile' could not be found (are you missing a using directive or an assembly reference?)
        public InboxFile GetInboxFile(long Id)
#pragma warning restore CS0246 // The type or namespace name 'InboxFile' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadSingleData<InboxFile>(string.Format("SELECT Id, InboxId, Name, Data, Size FROM InboxItems WHERE Id={0}", Id));
        }

#pragma warning disable CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        public Inbox RestrictedInboxCount(long referenceId)
#pragma warning restore CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        {
            string query = string.Format("SELECT * FROM Inbox WHERE Id = (SELECT MAX(Id) FROM Inbox WHERE ReferenceId={0})", referenceId);
            return ReadSingleData<Inbox>(query);
        }

#pragma warning disable CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        public Inbox GetParentInbox(long userId, long referenceId)
#pragma warning restore CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        {
            string query = string.Format("SELECT TOP 1 * FROM Inbox WHERE ReferenceId = {0} AND (SenderId = {1} OR ReceiverId = {1}) AND ParentId is null ORDER BY Id DESC", referenceId, userId);
            return ReadSingleData<Inbox>(query);
        }

        public long GetInboxId(long userId, long referenceId, int type)
        {
            long id = 0;
            string query = string.Format("SELECT TOP 1 Id FROM Inbox WHERE ReferenceType = {0} AND ReferenceId = {1} AND ReceiverId = {2} ORDER BY Id DESC", type, referenceId, userId);
            id = ReadDataField<long>(query);
            return id;
        }

#pragma warning disable CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        public Inbox GetInboxItem(long userId, long referenceId, int type)
#pragma warning restore CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        {            
            string query = string.Format("SELECT TOP 1 * FROM Inbox WHERE ReferenceType = {0} AND ReferenceId = {1} AND ReceiverId = {2} ORDER BY Id DESC", type, referenceId, userId);
            return  ReadSingleData<Inbox>(query);            
        }

#pragma warning disable CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        public int MarkInboxItem(Inbox model)
#pragma warning restore CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Add(new SqlParameter("@Id", model.Id));
            parameters.Add(new SqlParameter("@ReceiverId", model.ReceiverId));
            parameters.Add(new SqlParameter("@Unread", model.Unread));
            return HandleData("MarkInboxItem", parameters);

        }

        public int UpdateSafetyAdvice(string advice)
        {
            return HandleData(string.Format("UPDATE ConfigParameters SET Value='{0}' WHERE Name='SafetyAdvice'", advice));
        }

        public int DeleteUserAccount(long id)
        {
            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Add(new SqlParameter("@UserId", id));
            return HandleData("DeleteUserAccount", parameters);
        }

        public bool PaymentProcessEnabled()
        {
            int PaymentProcessEnabled = Convert.ToInt32(ConfigService.Instance.GetConfigValue("EnablePaymentProcess"));
            return (PaymentProcessEnabled == 1);
        }
        //public bool HasJobQuota(long userId)
        //{
        //    bool flag = false;

        //    if (PaymentProcessEnabled())
        //    {
        //        int? bal = ReadDataField<int?>(string.Format("SELECT TOP 1 BalanceJob FROM Accounts WHERE UserId={0} ORDER BY Id DESC", userId));
        //        flag = (bal != null && bal > 0);
        //    }
        //    else
        //    {
        //        flag = true;
        //    }
        //    return flag;
        //}

        public bool HasProfileQuota(long userId)
        {
            bool flag = false;

            if (PaymentProcessEnabled())
            {
                int? bal = ReadDataField<int?>(string.Format("SELECT TOP 1 BalanceProfiles FROM Accounts WHERE UserId={0} ORDER BY Id DESC", userId));
                flag = (bal != null && bal > 0);
            }
            else
            {
                flag = true;
            }
            return flag;
        }
        public bool IsPaidProfile(long userId, long referenceId)
        {
            bool flag = false;
            if (PaymentProcessEnabled())
            {
                int count = ReadDataField<int>(string.Format("SELECT COUNT(1) FROM Accounts WHERE UserId = {0} AND ReferenceId = {1} AND ReferenceType='P'", userId, referenceId));
                flag = (count > 0);
            }
            else
            {
                flag = true;
            }
            return flag;
        }
        public bool HasMessageQuota(long userId)
        {            
            int bal = ReadDataField<int>(string.Format("SELECT TOP 1 ISNULL(BalanceMessages,0) FROM Accounts WHERE UserId={0} ORDER BY Id DESC", userId));
            return (bal > 0);
        }

        public bool HasInterviewQuota(long userId)
        {
            bool flag = false;
            if (PaymentProcessEnabled())
            {
                int? bal = ReadDataField<int?>(string.Format("SELECT TOP 1 BalanceInterviews FROM Accounts WHERE UserId={0} ORDER BY Id DESC", userId));
                flag = (bal != null && bal > 0);
            }
            return flag;
        }

        public bool HasDownloadQuota(long userId)
        {
            bool flag = false;
            int? bal = ReadDataField<int?>(string.Format("SELECT TOP 1 BalanceResumes FROM Accounts WHERE UserId={0} ORDER BY Id DESC", userId));
            flag = (bal != null && bal > 0);
            return flag;
        }

        public bool IsPaidResume(long userId, long referenceId)
        {
            bool flag = false;
            if (PaymentProcessEnabled())
            {
                int count = ReadDataField<int>(string.Format("SELECT COUNT(1) FROM Accounts WHERE UserId = {0} AND ReferenceId = {1} AND ReferenceType='R'", userId, referenceId));
                flag = (count > 0);
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        public bool IsPaidJob(long userId, long referenceId)
        {
            bool flag = false;
            if (PaymentProcessEnabled())
            {
                int count = ReadDataField<int>(string.Format("SELECT COUNT(1) FROM Accounts WHERE UserId = {0} AND ReferenceId = {1} AND ReferenceType='J'", userId, referenceId));
                flag = (count > 0);
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        public long ManageTransaction(long id, int packageId, decimal? amount = null, string description = null, string transactionId = null, string method = null)
        {
            object oRetrunValue = null;
            long value=0;
            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Add(new SqlParameter("@UserId", id));
            parameters.Add(new SqlParameter("@PackageId", packageId));
            parameters.Add(new SqlParameter("@Amount", amount));
            parameters.Add(new SqlParameter("@Description", description));
            parameters.Add(new SqlParameter("@TransactionId", transactionId));
            parameters.Add(new SqlParameter("@PaymentMethod", method));

            oRetrunValue = ReadDataField("ManageTransaction", parameters);
            if (oRetrunValue != null)
            {
                value = Convert.ToInt64(oRetrunValue);
            }
            return value;
        }
       
       

#pragma warning disable CS0246 // The type or namespace name 'Package' could not be found (are you missing a using directive or an assembly reference?)
        public Package GetPackage(int Id)
#pragma warning restore CS0246 // The type or namespace name 'Package' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadSingleData<Package>(string.Format("SELECT * FROM Packages WHERE Id = {0}", Id));
        }

#pragma warning disable CS0246 // The type or namespace name 'TransactionEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<TransactionEntity> GetInvoiceList(long userId, DateTime? start = null, DateTime? end = null, int pageNumber = 1, int pageSize = 20)
#pragma warning restore CS0246 // The type or namespace name 'TransactionEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@StartDate", start));
            parameters.Add(new SqlParameter("@EndDate", end));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));
            parameters.Add(new SqlParameter("@PageSize", pageSize));

            return ReadData<TransactionEntity>("InvoiceList", parameters);
        }
#pragma warning disable CS0246 // The type or namespace name 'TransactionEntity' could not be found (are you missing a using directive or an assembly reference?)
        public TransactionEntity GetInvoice(long userId, long Id)
#pragma warning restore CS0246 // The type or namespace name 'TransactionEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@PageNumber", 1));
            parameters.Add(new SqlParameter("@PageSize", 1));
            parameters.Add(new SqlParameter("@Id", Id));

            return ReadSingleData<TransactionEntity>("InvoiceList", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'TransactionEntity' could not be found (are you missing a using directive or an assembly reference?)
        public TransactionEntity GetUsageStatus(long userId)
#pragma warning restore CS0246 // The type or namespace name 'TransactionEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", userId));
           
            return ReadSingleData<TransactionEntity>("GetUsageStatus", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'SoldPackageEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<SoldPackageEntity> SalesByPackage(DateTime? start = null, DateTime? end = null)
#pragma warning restore CS0246 // The type or namespace name 'SoldPackageEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@StartDate", start));
            parameters.Add(new SqlParameter("@EndDate", end));
  
            return ReadData<SoldPackageEntity>("SalesByPackage", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'SaleByCountryEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<SaleByCountryEntity> SalesByCountry(DateTime? start = null, DateTime? end = null, long? countryId = null)
#pragma warning restore CS0246 // The type or namespace name 'SaleByCountryEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@StartDate", start));
            parameters.Add(new SqlParameter("@EndDate", end));
            parameters.Add(new SqlParameter("@CountryId", countryId));

            return ReadData<SaleByCountryEntity>("SalesByCountry", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'SalesDetails' could not be found (are you missing a using directive or an assembly reference?)
        public List<SalesDetails> SalesDetails(long countryId, DateTime? start = null, DateTime? end = null, string name = null, int pageNumber = 1, int pageSize = 20)
#pragma warning restore CS0246 // The type or namespace name 'SalesDetails' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@CountryId", countryId));
            parameters.Add(new SqlParameter("@StartDate", start));
            parameters.Add(new SqlParameter("@EndDate", end));
            parameters.Add(new SqlParameter("@Name", name));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));
            parameters.Add(new SqlParameter("@PageSize", pageSize));

            return ReadData<SalesDetails>("SalesDetails", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'PaidProfileEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<PaidProfileEntity> GetPaidProfiles(long userId, int pageNumber = 1, int pageSize = 20)
#pragma warning restore CS0246 // The type or namespace name 'PaidProfileEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));
            parameters.Add(new SqlParameter("@PageSize", pageSize));

            return ReadData<PaidProfileEntity>("ListPaidProfiles", parameters);
        }

        public int Unblock(long userId, long requestor)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@Requestor", requestor));

            return HandleData("UnblockUser", parameters);
        }

        public int Block(long userId, long requestor)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@Requestor", requestor));

            return HandleData("BlockUser", parameters);
        }

        public bool IsBlockedByMe(long userId, long requestor)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@Requestor", requestor));

            int blocked = ReadDataField<int>("IsBlockedByMe", parameters);
            return (blocked < 0);
        }

        public bool IsBlockedBySomeone(long userId, long requestor)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@Requestor", requestor));

            int blocked = ReadDataField<int>("IsBlockedBySomeone", parameters);
            return (blocked > 0);
        }

        public bool IsInvitedByMe(long userId, long requestor)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@Requestor", requestor));

            int invited = ReadDataField<int>("IsInvitedByMe", parameters);
            return (invited > 0);
        }

        public bool IsInvitedBySomeone(long userId, long requestor)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@Requestor", requestor));

            int invited = ReadDataField<int>("IsInvitedBySomeone", parameters);
            return (invited > 0);
        }

        public bool IsConnected(long userId, long requestor)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@Requestor", requestor));

            int connected = ReadDataField<int>("IsConnected", parameters);
            return (connected > 0);
        }

        public List<long?> ConnectionList(long userId, long loggedInUserId)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@LoginUserId", loggedInUserId));
            return ReadDataList<long?>("ListConnections", parameters);
        }
        public string ReadData(Guid Id)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@Id", Id));
            parameters.Add(new SqlParameter("@Type", "0"));
            return Convert.ToString(ReadDataField("ManageSessionData", parameters));
        }

        public int StoreData(Guid Id, string data)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@Id", Id));
            parameters.Add(new SqlParameter("@Type", "1"));
            parameters.Add(new SqlParameter("@Data", data));

            return HandleData("ManageSessionData", parameters);
        }

        public int RemoveData(Guid Id)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@Id", Id));
            parameters.Add(new SqlParameter("@Type", "2"));
            return HandleData("ManageSessionData", parameters);
        }
    }
}
