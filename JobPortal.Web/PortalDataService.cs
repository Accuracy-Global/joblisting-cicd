using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace JobPortal.Web
{
    public class PortalDataService : DataService
    {
#pragma warning disable CS0246 // The type or namespace name 'CountEntity' could not be found (are you missing a using directive or an assembly reference?)
        public CountEntity CountUsers(long? countryId, int? typeId)
#pragma warning restore CS0246 // The type or namespace name 'CountEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            CountEntity entity = new CountEntity();

            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@CountryId", countryId));
            parameters.Add(new SqlParameter("@Type", typeId));

            entity = ReadSingleData<CountEntity>("GetUserCounts", parameters);

            return entity;
        }
       

#pragma warning disable CS0246 // The type or namespace name 'CountEntity' could not be found (are you missing a using directive or an assembly reference?)
        public CountEntity CountJobs(long? countryId)
#pragma warning restore CS0246 // The type or namespace name 'CountEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            CountEntity entity = new CountEntity();

            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@CountryId", countryId));
            entity = ReadSingleData<CountEntity>("GetJobCounts", parameters);

            return entity;
        }

#pragma warning disable CS0246 // The type or namespace name 'UserByCountryEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<UserByCountryEntity> GetUsersByCountry(long? countryId=null, int? type = null, bool? confirmed = null, bool? active = null, DateTime? start=null, DateTime? end=null, bool? pending = null,  int pageSize = 111, int pageNumber = 1, string ut = null)
#pragma warning restore CS0246 // The type or namespace name 'UserByCountryEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<UserByCountryEntity> list = new List<UserByCountryEntity>();
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@CountryId", countryId));
            parameters.Add(new SqlParameter("@Type", type));
            parameters.Add(new SqlParameter("@Confirmed", confirmed));
            parameters.Add(new SqlParameter("@Active", active));
            parameters.Add(new SqlParameter("@StartDate", start));
            parameters.Add(new SqlParameter("@EndDate", end));
            parameters.Add(new SqlParameter("@Pending", pending));
            parameters.Add(new SqlParameter("@PageSize", pageSize));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));
            parameters.Add(new SqlParameter("@UT", ut));

            list = ReadData<UserByCountryEntity>("GetUsersByCountry", parameters);

            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'RecentJobEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<RecentJobEntity> GetRecentJobs(long? countryId = null, long? employerId = null, int pageNumber = 1)
#pragma warning restore CS0246 // The type or namespace name 'RecentJobEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<RecentJobEntity> list = new List<RecentJobEntity>();
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@CountryId", countryId));
            parameters.Add(new SqlParameter("@EmployerId", employerId));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));

            list = ReadData<RecentJobEntity>("GetRecentJobs", parameters);

            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'AccountsInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        public AccountsInfoEntity GetAccountStatistics(long? countryId = null)
#pragma warning restore CS0246 // The type or namespace name 'AccountsInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@CountryId", countryId));
            return ReadSingleData<AccountsInfoEntity>("GetAccountStatistics", parameters);
        }

        public string GetPageContent(string pageId)
        {
            object value = null;
            value = ReadDataField(string.Format("SELECT PageContent FROM SitePages WHERE PageId='{0}'", pageId));
            return Convert.ToString(value);
        }
        public long CountCountries()
        {
            object value = null;
            value = ReadDataField(string.Format("select Count(distinct Text) as countcountry FROM [dbo].[Lists] where Name='Country'"));
            return Convert.ToInt64(value);
           
        }
        public long CountCompanies()
        {
            object value = null;
            value = ReadDataField(string.Format("select count(*) as countcompanies from UserProfiles where Type=5;"));
            return Convert.ToInt64(value);

        }
        public long CountJobSeekers()
        {
            object value = null;
            value = ReadDataField(string.Format("select count(*) as countjobseekers from UserProfiles where Type=4;"));
            return Convert.ToInt64(value);

        }
        public long CountWebListkrishna()
        {
            object value = null;
            value = ReadDataField(string.Format("select count(*) as countjobseekers from [dbo].[WebsiteList] where Name='krishna veni';"));
            return Convert.ToInt64(value);

        }
        public long CountWebListPoojitha()
        {
            object value = null;
            value = ReadDataField(string.Format("select count(*) as countjobseekers from [dbo].[WebsiteList] where Name='Poojitha';"));
            return Convert.ToInt64(value);

        }
        public long CountWebListPRASANNA()
        {
            object value = null;
            value = ReadDataField(string.Format("select count(*) as countjobseekers from [dbo].[WebsiteList] where Name='PRASANNA';"));
            return Convert.ToInt64(value);

        }
        public long CountWebListprudhvi()
        {
            object value = null;
            value = ReadDataField(string.Format("select count(*) as countjobseekers from [dbo].[WebsiteList] where Name='prudhvi';"));
            return Convert.ToInt64(value);

        }
        public long CountWebListHARSHASREE()
        {
            object value = null;
            value = ReadDataField(string.Format("select count(*) as countjobseekers from [dbo].[WebsiteList] where Name='HARSHASREE';"));
            return Convert.ToInt64(value);

        }


        public long CountInstitutes()
        {
            object value = null;
            value = ReadDataField(string.Format("select count(*) as countInstitutes from UserProfiles where Type=12;"));
            return Convert.ToInt64(value);

        }
        public long CountRAgencies()
        {
            object value = null;
            value = ReadDataField(string.Format("select count(*) as countRAgencies from UserProfiles where Type=14;"));
            return Convert.ToInt64(value);

        }
        public long CountStudent()
        {
            object value = null;
            value = ReadDataField(string.Format("select count(*) as countRAgencies from UserProfiles where Type=13;"));
            return Convert.ToInt64(value);

        }
        public int TrackSocialMediaPost(long jobId, string postId, string media)
        {
            int status = 0;
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@JobId", jobId));
            parameters.Add(new SqlParameter("@PostId", postId));
            parameters.Add(new SqlParameter("@SocialMedia", media));

            status = HandleData("TrackSocialMediaPost", parameters);
            return status;
        }

#pragma warning disable CS0246 // The type or namespace name 'StreamEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<StreamEntity> ListStream(long userid, long? loggedInUserId, int pageNumber)
#pragma warning restore CS0246 // The type or namespace name 'StreamEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<StreamEntity> list = new List<StreamEntity>();
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", userid));
            parameters.Add(new SqlParameter("@LoggedInUserId", loggedInUserId));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));
            list = ReadData<StreamEntity>("List_ActivityStream", parameters);
            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'PostTypes' could not be found (are you missing a using directive or an assembly reference?)
        public int ManageStream(long userId, long reference, PostTypes type)
#pragma warning restore CS0246 // The type or namespace name 'PostTypes' could not be found (are you missing a using directive or an assembly reference?)
        {
            int stat = 0;
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@Reference", reference));
            parameters.Add(new SqlParameter("@Type", type));

            stat = HandleData("Manage_ActivityStream", parameters);
            return stat;
        }
    }
}