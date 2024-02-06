using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Helpers;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Web;
using System.Data.SqlClient;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Data;

namespace JobPortal.Domain
{
#pragma warning disable CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    public class MemberService : DataService
#pragma warning restore CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    {
        private static volatile MemberService instance;
        private static readonly object sync = new object();

        /// <summary>
        /// Default private constructor.
        /// </summary>
        private MemberService()
        {
        }

        /// <summary>d
        /// Single Instance of MemberService
        /// </summary>
        public static MemberService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new MemberService();
                        }
                    }
                }
                return instance;
            }
        }

        public decimal GetProfileWeightage(long id)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", id));
            return ReadDataField<decimal>("GetProfileWeightage1", parameters);
        }
        public byte[] GetProfilePhoto(long id)
        {
            byte[] buffer = new byte[1];
            object v = ReadDataField(string.Format("GetUserPhoto {0}", id));

            if (v != null || v != DBNull.Value)
            {
                try
                {
                    if (v is string)
                    {
                        buffer = Convert.FromBase64String(Convert.ToString(v));
                    }
                    else
                    {
                        buffer = (byte[])v;
                    }
                }
                catch (Exception)
                {

                }
            }
            return buffer;
        }
        public byte[] GetAlbumPhoto(long? id = null, long? userId = null, string area = null)
        {
            byte[] buffer = null;

            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@Id", id));
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@Area", area));

            object v = ReadDataField("GetPhoto", parameters);

            if (v != null)
            {
                buffer = (byte[])v;
            }
            return buffer;
        }

        public long? GetAlbumPhotoId(long userId, string area)
        {
            long? id = null;


            object v = ReadDataField(string.Format("SELECT Id FROM dbo.Photos WHERE UserId = {0} AND Area = '{1}'", userId, area));

            if (v != null)
            {
                id = Convert.ToInt64(v);
            }
            return id;
        }

#pragma warning disable CS0246 // The type or namespace name 'Photo' could not be found (are you missing a using directive or an assembly reference?)
        public Photo GetPhotoByArea(long userId, string area)
#pragma warning restore CS0246 // The type or namespace name 'Photo' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadSingleData<Photo>(string.Format("SELECT * FROM dbo.Photos WHERE UserId = {0} AND Area = '{1}' AND IsDeleted = 0 AND ((IsApproved = 1 AND IsRejected = 0 AND InEditMode = 0) OR (IsApproved = 0 AND IsRejected = 0 AND InEditMode = 1))", userId, area));
        }

#pragma warning disable CS0246 // The type or namespace name 'User_Skills' could not be found (are you missing a using directive or an assembly reference?)
        public List<User_Skills> GetUserskill(long userId)
#pragma warning restore CS0246 // The type or namespace name 'User_Skills' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadData<User_Skills>(string.Format("SELECT * FROM User_Skills WHERE UserId = {0}", userId));
        }
        public List<UnivList> UnivListName()
        {
            return ReadData<UnivList>(string.Format("SELECT distinct University as UnivName  FROM UserProfiles where Type=12  and University != ''"));
        }
        public List<CompanyList> CompanyListName()
        {
            return ReadData<CompanyList>(string.Format("SELECT distinct Company as CompanyName  FROM UserProfiles where Type=5  and Company != ''"));
        }
        public class UnivList
        {
            public string UnivName { get; set; }
        }
        public class CompanyList
        {
            public string CompanyName { get; set; }
        }
#pragma warning disable CS0246 // The type or namespace name 'User_Experience' could not be found (are you missing a using directive or an assembly reference?)
        public List<User_Experience> GetUserexp(long userId)
#pragma warning restore CS0246 // The type or namespace name 'User_Experience' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadData<User_Experience>(string.Format("SELECT Employer,Industry,CountryId,Category FROM User_Experience   WHERE UserId = {0} group by  Employer,Industry,CountryId,Category ", userId));
        }
#pragma warning disable CS0246 // The type or namespace name 'User_Experience' could not be found (are you missing a using directive or an assembly reference?)
        public List<User_Experience> GetUserexpr(long userId, string company, string industry, string category)
#pragma warning restore CS0246 // The type or namespace name 'User_Experience' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadData<User_Experience>(string.Format("SELECT * FROM User_Experience WHERE UserId = {0} and Employer='{1}' and Industry='{2}' and Category='{3}'", userId, company, industry, category));
        }
#pragma warning disable CS0246 // The type or namespace name 'User_Experience' could not be found (are you missing a using directive or an assembly reference?)
        public List<User_Experience> GetUserexprf(long userId)
#pragma warning restore CS0246 // The type or namespace name 'User_Experience' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadData<User_Experience>(string.Format("SELECT Top 1 FromYr FROM User_Experience WHERE UserId ={0} order by FromYr", userId));
        }
#pragma warning disable CS0246 // The type or namespace name 'User_Experience' could not be found (are you missing a using directive or an assembly reference?)
        public List<User_Experience> GetUserexprt(long userId)
#pragma warning restore CS0246 // The type or namespace name 'User_Experience' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadData<User_Experience>(string.Format("SELECT Top 1 ToYr FROM User_Experience WHERE UserId ={0} order by ToYr desc", userId));
        }
#pragma warning disable CS0246 // The type or namespace name 'User_Education' could not be found (are you missing a using directive or an assembly reference?)
        public List<User_Education> GetUseredu(long userId)
#pragma warning restore CS0246 // The type or namespace name 'User_Education' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadData<User_Education>(string.Format("SELECT * FROM User_Education WHERE UserId = {0} order by [FromYr] desc,[ToYr] desc", userId));

        }

        //public List<SkillList> GetSkillList(string a)
        //{
        //    return ReadData<SkillList>(string.Format("SELECT [Skills] FROM [dbo].[SkillList] where Skills like '{0}%'", a));
        //}

#pragma warning disable CS0246 // The type or namespace name 'User_Certificate' could not be found (are you missing a using directive or an assembly reference?)
        public List<User_Certificate> GetUsercer(long userId)
#pragma warning restore CS0246 // The type or namespace name 'User_Certificate' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadData<User_Certificate>(string.Format("SELECT * FROM User_Certificate WHERE UserId = {0}", userId));
        }
        public List<sentE> sentE1(long userId, long jobid)
        {
            return ReadData<sentE>(string.Format("SELECT * FROM shareJMails WHERE UserId = {0} and JobId={1} group by UserId,JobId,EName,EmailID,Contact,RStatus,Dt,Update_profile,Apply_job", userId, jobid));
        }
        public List<sentE> sentEI(long userId, long jobid, string emailid)
        {
            return ReadData<sentE>(string.Format("Insert into shareJMails(UserId,JobId,EmailID,Dt) values({0},{1},'{2}','{3}')", userId, jobid, emailid, DateTime.Now));
        }
        public List<sentE> sentSI(long userId, long jobid, string emailid)
        {
            return ReadData<sentE>(string.Format("Insert into shareJMails(UserId,JobId,Contact,Dt) values({0},{1},'{2}','{3}')", userId, jobid, emailid, DateTime.Now));
        }
        public List<Invit> InviteS(string mid, long userId, string mobile, Guid? token)
        {
            return ReadData<Invit>(string.Format("Insert into Invites(MessageSID,UserId,Invited,Mobile,Token,DateInvited,IsConnected) values('{0}',{1},'true','{2}','{3}','{4}','false')", mid, userId, mobile, token, DateTime.Now));
        }
        public List<Invit> InviteSC(long userId, string mobile)
        {
            return ReadData<Invit>(string.Format("Select * from Invites where UserId={0} and Mobile='{1}' and DateInvited='{2}'", userId, mobile, DateTime.Now));
        }
#pragma warning disable CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        public List<Job> InviteJob(long userId)
#pragma warning restore CS0246 // The type or namespace name 'Job' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadData<Job>(string.Format("Select * from Jobs where Id={0}", userId));
        }
        public class Invit
        {

            public long Id { get; set; }
            public string Mobile { get; set; }
            public bool Invited { get; set; }
            public long UserId { get; set; }
            public string MessageSID { get; set; }
            public Nullable<System.Guid> Token { get; set; }
            public bool IsConnected { get; set; }
            public System.DateTime DateInvited { get; set; }
            public Nullable<System.DateTime> LastReminderDate { get; set; }
        }
        public class sentE
        {
            public long UserId { get; set; }

            public long JobId { get; set; }
            public string EName { get; set; }
            public string EmailID { get; set; }
            public string Contact { get; set; }
            public string RStatus { get; set; }
            public System.DateTime Dt { get; set; }
            public string Update_profile { get; set; }
            public string Apply_job { get; set; }
        }

        public class UserPVal
        {
            public long UserId { get; set; }
            public string Username { get; set; }
            public int Type { get; set; }
            public string Company { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Summary { get; set; }
            public string Address { get; set; }
            public Nullable<long> CountryId { get; set; }
            public Nullable<long> StateId { get; set; }
            public string City { get; set; }
            public string Zip { get; set; }
            public string Phone { get; set; }
            public string DateOfBirth { get; set; }
            public string Website { get; set; }
            public System.DateTime DateCreated { get; set; }
            public string CreatedBy { get; set; }
            public Nullable<System.DateTime> DateUpdated { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> DateDeleted { get; set; }
            public string DeletedBy { get; set; }
            public bool IsDeleted { get; set; }
            public bool IsActive { get; set; }
            public bool IsFeatured { get; set; }
            public string PermaLink { get; set; }
            public string Provider { get; set; }
            public string ProviderId { get; set; }
            public string ProviderUsername { get; set; }
            public string ProviderAccessToken { get; set; }
            public string Title { get; set; }
            public Nullable<byte> Age { get; set; }
            public Nullable<long> QualificationId { get; set; }
            public Nullable<long> EmploymentTypeId { get; set; }

            public Nullable<long> CategoryId { get; set; }
            public Nullable<long> SpecializationId { get; set; }
            public string Gender { get; set; }
            public Nullable<int> RelationshipStatus { get; set; }
            public string ConfirmationToken { get; set; }
            public bool IsConfirmed { get; set; }
            public string NewUsername { get; set; }
            public int EmailCorrectionTries { get; set; }
            public bool IsValidUsername { get; set; }
            public string Mobile { get; set; }
            public string PhoneCountryCode { get; set; }
            public string MobileCountryCode { get; set; }
            public string Facebook { get; set; }
            public string Twitter { get; set; }
            public string LinkedIn { get; set; }
            public string GooglePlus { get; set; }
            public bool IsBuild { get; set; }
            public bool IsApproved { get; set; }
            public string Education { get; set; }
            public string School { get; set; }
            public string FromYrE { get; set; }
            public string FromYr { get; set; }
            public string ToYrE { get; set; }
            public string ToYr { get; set; }
            public string ToMonth { get; set; }
            public string FromMonth { get; set; }
            public string Grade { get; set; }
            public string Employer { get; set; }
            public string Roles { get; set; }
            public string Responsibilities { get; set; }
        }

        public DataTable GetUserval(long userId)
        {
            //reader["Id"].ToString();
            DataTable info = ReadSingleData1(string.Format("SELECT u.UserId,u.Username,u.[Type],u.[Company],u.[Title],u.[FirstName],u.[LastName], u.[Summary], u.[Address], u.[CountryId], u.[StateId], u.[City], u.[Zip], u.[PhoneCountryCode], u.[Phone], u.[MobileCountryCode], u.[Mobile], u.[Gender], u.[RelationshipStatus], u.[Age], u.[QualificationId], u.[EmploymentTypeId], u.[Website], u.[DateCreated], u.[CreatedBy], u.[DateUpdated], u.[UpdatedBy], u.[DateDeleted], u.[DeletedBy], u.[IsDeleted], u.[IsActive], u.[IsFeatured], u.[PermaLink], u.[IsApproved], u.[Provider], u.[ProviderId], u.[ProviderUsername], u.[ProviderAccessToken], u.[IsConfirmed], u.[NewUsername], u.[EmailCorrectionTries], u.[IsValidUsername], u.[Facebook], u.[Twitter], u.[LinkedIn], u.[GooglePlus], u.[DateOfBirth], u.[ConfirmationToken], u.[IsBuild], e1.Education, e1.School, e1.FromYr, e1.ToYr, e1.Grade, d.Employer, d.CategoryId, d.SpecializationId, d.Roles, d.FromMonth, d.ToMonth, d.FromYr, d.ToYr, d.Responsibilities FROM UserProfiles u  Left join(select Top 1 UserId, Employer, F.Id CategoryId, G.Id SpecializationId, Roles, FromMonth, ToMonth, FromYr, ToYr, Responsibilities from User_Experience E  Left outer JOIN Specializations F ON E.Industry = F.Name Left outer JOIN SubSpecializations G ON E.Category = G.Name where E.UserId = {0} Order by toYr desc, toMonth desc) d on u.UserId = d.UserId Left join(select Top 1 UserId, Education, School, FromYr, ToYr, Grade from User_Education where UserId = {0} Order by toYr desc) e1 on u.UserId = e1.UserId WHERE u.UserId = {0} ", userId));
            return (Convert.ToInt32(info.Rows[0]["UserId"].ToString()) > 0) ? info : null;
        }
#pragma warning disable CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        public UserInfoEntity GetUserInfo(string username)
#pragma warning restore CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@Username", username));
            UserInfoEntity info = ReadSingleData<UserInfoEntity>("GetUserInfo", parameters);

            return (info.Id > 0) ? info : null;
        }

#pragma warning disable CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        public UserInfoEntity GetUserInfoByAddress(string address)
#pragma warning restore CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@PermaLink", address));
            UserInfoEntity info = ReadSingleData<UserInfoEntity>("GetUserInfo", parameters);

            return (info.Id > 0) ? info : null;
        }
#pragma warning disable CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        public UserInfoEntity GetUserInfo(long id)
#pragma warning restore CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", id));
            return ReadSingleData<UserInfoEntity>("GetUserInfo", parameters);
        }
#pragma warning disable CS0246 // The type or namespace name 'Connection' could not be found (are you missing a using directive or an assembly reference?)
        public List<Connection> GetFriendList(string Username)
#pragma warning restore CS0246 // The type or namespace name 'Connection' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Connection> friends = new List<Connection>();
            UserProfile profile = Get(Username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                friends = dataHelper.Get<Connection>().Where(x => x.UserId == profile.UserId).ToList();
            }
            return friends;
        }

#pragma warning disable CS0246 // The type or namespace name 'Activity' could not be found (are you missing a using directive or an assembly reference?)
        public List<Activity> GetActivityList(string Username)
#pragma warning restore CS0246 // The type or namespace name 'Activity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Activity> latestActivities = new List<Activity>();
            UserProfile profile = Get(Username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var friends = dataHelper.Get<Connection>().Where(x => x.UserId == profile.UserId).Select(x => x.UserId).ToList();
                latestActivities = dataHelper.Get<Activity>().Where(x => x.UserId == profile.UserId && friends.Contains(x.UserId))
                    .OrderByDescending(z => z.ActivityDate).Take(10).ToList();
            }
            return latestActivities;
        }


#pragma warning disable CS0246 // The type or namespace name 'Photo' could not be found (are you missing a using directive or an assembly reference?)
        public List<Photo> GetPhotoList(string Username, bool IsAuthenticated, int PageNumber = 0)
#pragma warning restore CS0246 // The type or namespace name 'Photo' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Photo> latestPhotos = new List<Photo>();
            UserProfile profile = Get(Username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                var connections = dataHelper.Get<Connection>().Where(x => x.UserId == profile.UserId && x.IsConnected == true && x.IsDeleted == false).ToList();
                var blocked = dataHelper.Get<BlockedPeople>().Where(x => x.BlockerId == profile.UserId).Select(x => x.BlockedUser.Username).ToList();
                var friends = connections.Where(x => !blocked.Contains(x.EmailAddress)).Select(x => x.EmailAddress).ToList();
                friends.Add(profile.Username);

                latestPhotos = dataHelper.Get<Photo>().Where(x => friends.Contains(x.UserProfile.Username) && x.Area != "Profile" && x.IsApproved == true)
                    .OrderByDescending(z => z.DateUpdated).Take(5).ToList();
                if (connections.Count > 0)
                {
                    var clst = connections.ToList();
                    latestPhotos = latestPhotos.Where(x => clst.Any(y => y.DateUpdated != null && y.DateUpdated.Value <= x.DateUpdated)).ToList();
                }
            }
            return latestPhotos;
        }

        public long GetProfileViews(string Username)
        {
            long views = 0;
            UserInfoEntity profile = GetUserInfo(Username);
            views = ReadDataField<long>(string.Format("SELECT CAST(COUNT(1) AS BIGINT) FROM ProfileViews WHERE ProfileId = {0}", profile.Id));
            //using (JobPortalEntities context = new JobPortalEntities())
            //{
            //    views = context.ProfileViews.Count(x => x.ProfileId == profile.Id);
            //}
            return views;
        }
        public List<User_Skills> GetUserskillProfiles(string skillName)
#pragma warning restore CS0246 // The type or namespace name 'User_Skills' could not be found (are you missing a using directive or an assembly reference?)
        {
            //var SQL = string.Format("SELECT * FROM User_Skills WHERE SkillName LIKE ?", skillName);
            //Cmd.Parameters.AddWithValue(skillName, "%" + skillName + "%");
            return ReadData<User_Skills>(string.Format("SELECT * FROM User_Skills WHERE SkillName LIKE '" + skillName + "'"));
        }
        public void ValidateEmail(string body)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                context.USP_ValidateEmails(body, false);
            }
        }
        public int PeopleYouMayKnowCounts(string Username)
        {
            int rows = 0;
            UserProfile profile = MemberService.Instance.Get(Username);
            if (profile.IsConfirmed == true)
            {
                List<UserProfile> user_list = new List<UserProfile>();
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    var blockedList = dataHelper.Get<BlockedPeople>().Where(x => x.BlockedId == profile.UserId).Select(x => x.BlockerId);

                    int company = (int)SecurityRoles.Employers;
                    var companies = dataHelper.Get<UserProfile>().Where(x => x.IsActive == true && x.IsDeleted == false && x.IsConfirmed == true && x.Type == company && x.UserId != profile.UserId);
                    if (blockedList.Any())
                    {
                        companies = companies.Where(x => !blockedList.Contains(x.UserId));
                    }

                    int individual = (int)SecurityRoles.Jobseeker;
                    var individuals = dataHelper.Get<UserProfile>().Where(x => x.IsActive == true && x.IsDeleted == false && x.IsConfirmed == true && x.Type == individual && x.UserId != profile.UserId);
                    if (blockedList.Any())
                    {
                        individuals = individuals.Where(x => !blockedList.Contains(x.UserId));
                    }

                    if (profile.Type == (int)SecurityRoles.Jobseeker)
                    {
                        string[] cemployer_names = new string[] { "" };
                        string cemployer = string.Empty;
                        if (!string.IsNullOrEmpty(profile.CurrentEmployer))
                        {
                            cemployer_names = profile.CurrentEmployer.Split(' ');
                            if (cemployer_names.Length > 1)
                            {
                                cemployer = string.Format("{0} {1}", cemployer_names[0], cemployer_names[1]).ToLower();
                            }
                            else
                            {
                                cemployer = profile.CurrentEmployer.ToLower();
                            }
                        }

                        string[] pemployer_names = new string[] { "" };
                        string pemployer = string.Empty;
                        if (!string.IsNullOrEmpty(profile.PreviousEmployer))
                        {
                            pemployer_names = profile.PreviousEmployer.Split(' ');
                            if (pemployer_names.Length > 1)
                            {
                                pemployer = string.Format("{0} {1}", pemployer_names[0], pemployer_names[1]).ToLower();
                            }
                            else
                            {
                                pemployer = profile.PreviousEmployer.ToLower();
                            }
                        }

                        individuals = individuals.Where(x =>
                        (x.LastName == profile.LastName && x.CountryId == profile.CountryId && (x.FirstName != profile.FirstName && x.LastName != profile.LastName))
                        || ((x.Address != null || x.Zip != null) && x.Address == profile.Address && x.City == profile.City && x.StateId == profile.StateId && x.Zip == profile.Zip && x.CountryId == profile.CountryId)
                        || (x.Phone != null && x.Phone == profile.Phone && x.CountryId == profile.CountryId)
                        || (x.School != null && x.School == profile.School && x.CountryId == profile.CountryId)
                        || (x.University != null && x.University == profile.University)
                        || ((profile.CategoryId != null && x.CategoryId == profile.CategoryId) && (profile.SpecializationId != null && x.SpecializationId == profile.SpecializationId)
                        && ((profile.CurrentEmployer != null && (x.CurrentEmployer.ToLower().Contains(cemployer) || x.CurrentEmployer.ToLower().Contains(pemployer)))
                        || (profile.PreviousEmployer != null && (x.PreviousEmployer.ToLower().Contains(cemployer) || x.PreviousEmployer.ToLower().Contains(pemployer)))) && x.CountryId == profile.CountryId));

                        user_list.AddRange(individuals.ToList());

                        companies = companies.Where(x =>
                      (x.LastName == profile.LastName && x.CountryId == profile.CountryId && (x.FirstName != profile.FirstName && x.LastName != profile.LastName))
                      || ((x.Address != null || x.Zip != null) && x.Address == profile.Address && x.City == profile.City && x.StateId == profile.StateId && x.Zip == profile.Zip && x.CountryId == profile.CountryId)
                      || (x.Phone != null && x.Phone == profile.Phone && x.CountryId == profile.CountryId)
                      || ((profile.CurrentEmployer != null && x.Company.ToLower().Contains(cemployer) || profile.PreviousEmployer != null && x.Company.ToLower().Contains(pemployer)) && x.CountryId == profile.CountryId));

                        user_list.AddRange(companies.ToList());

                    }
                    else if (profile.Type == (int)SecurityRoles.Employers)
                    {
                        string[] company_names = new string[] { "" };
                        string cname = string.Empty;
                        if (!string.IsNullOrEmpty(profile.Company))
                        {
                            company_names = profile.Company.Split(' ');
                            if (company_names.Length > 1)
                            {
                                cname = string.Format("{0} {1}", company_names[0], company_names[1]).ToLower();
                            }
                            else
                            {
                                cname = profile.Company.ToLower();
                            }
                        }

                        companies = companies.Where(x =>
                     (x.LastName == profile.LastName && x.CountryId == profile.CountryId && (x.FirstName != profile.FirstName && x.LastName != profile.LastName))
                     || ((x.Address != null || x.Zip != null) && x.Address == profile.Address && x.City == profile.City && x.StateId == profile.StateId && x.Zip == profile.Zip && x.CountryId == profile.CountryId)
                     || (x.Phone != null && x.Phone == profile.Phone && x.CountryId == profile.CountryId)
                     || (profile.Company != null && x.Company.ToLower().Contains(cname) && x.CountryId == profile.CountryId));

                        user_list.AddRange(companies.ToList());

                        individuals = individuals.Where(x =>
                       (x.LastName == profile.LastName && x.CountryId == profile.CountryId && (x.FirstName != profile.FirstName && x.LastName != profile.LastName))
                       || ((x.Address != null || x.Zip != null) && x.Address == profile.Address && x.City == profile.City && x.StateId == profile.StateId && x.Zip == profile.Zip && x.CountryId == profile.CountryId)
                       || (x.Phone != null && x.Phone == profile.Phone && x.CountryId == profile.CountryId)
                       || ((x.CurrentEmployer != null && x.CurrentEmployer.ToLower().Contains(cname) || x.PreviousEmployer != null && x.PreviousEmployer.ToLower().Contains(cname)) && x.CountryId == profile.CountryId));
                        user_list.AddRange(individuals.ToList());
                    }

                    List<long> matchlist = dataHelper.Get<PeopleMatch>().Where(x => x.UserId == profile.UserId).Select(x => x.MatchId).ToList();
                    user_list = user_list.Where(x => !matchlist.Contains(x.UserId)).ToList();

                    List<Connection> connection_list = MemberService.Instance.GetConnections(Username);
                    connection_list = connection_list.Where(x => x.IsDeleted == false).ToList();
                    rows = user_list.Count(x => !connection_list.Any(z => z.EmailAddress.Equals(x.Username)));
                }
            }
            return rows;
        }

        public int ProfileViews(string Username)
        {
            UserProfile profile = MemberService.Instance.Get(Username);
            UserProfile member = MemberService.Instance.Get(Convert.ToInt64(HttpContext.Current.User.Identity.Name));
            List<UserProfile> user_list = new List<UserProfile>();
            int counts = 0;
            List<int> typeList = new List<int>() { 4, 5 };
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var blockedList = dataHelper.Get<BlockedPeople>().Where(x => x.BlockedId == profile.UserId).Select(x => x.BlockerId);

                var views = dataHelper.Get<ProfileView>().Where(x => x.ProfileId == profile.UserId && typeList.Contains(x.UserProfile.Type) && x.UserId != null);
                if (blockedList.Any())
                {
                    views = views.Where(x => !blockedList.Contains(x.UserId.Value));
                }
                counts = views.Where(x => x.UserId != member.UserId).Select(x => x.UserProfile).Distinct().Count();
            }
            return counts;
        }

        public int ProfileViewCounts(string Username)
        {
            UserProfile member = MemberService.Instance.Get(Username);
            UserProfile profile = MemberService.Instance.Get(Convert.ToInt64(HttpContext.Current.User.Identity.Name));
            int views = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                views = dataHelper.Get<ProfileView>().Count(x => x.ProfileId == profile.UserId && x.UserId == member.UserId && x.Unread == true);
            }
            return views;
        }

#pragma warning disable CS0246 // The type or namespace name 'Alert' could not be found (are you missing a using directive or an assembly reference?)
        public List<Alert> GetAlerts(string Username)
#pragma warning restore CS0246 // The type or namespace name 'Alert' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Alert> alerts = new List<Alert>();
            if (!string.IsNullOrEmpty(Username))
            {
                UserProfile profile = Get(Username);
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    alerts = dataHelper.Get<Alert>().Where(x => x.Configurable == true).ToList();
                }
            }
            return alerts;
        }
        public string GetAlertStatus(int Id, string Username)
        {
            string status = string.Empty;
            UserProfile profile = Get(Username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                AlertSetting setting = dataHelper.Get<AlertSetting>().SingleOrDefault(x => x.AlertId == Id && x.UserId == profile.UserId);
                if (setting != null)
                {
                    if (setting.Enabled)
                    {
                        status = "Disable";
                    }
                    else
                    {
                        status = "Enable";
                    }
                }
                else
                {
                    status = "Disable";
                }
            }
            return status;
        }

        public int GetInboxItems(string Username)
        {
            int counts = 0;
            if (!string.IsNullOrEmpty(Username))
            {
                UserProfile profile = Get(Username);
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    counts = dataHelper.Get<Activity>().Count(x => x.UserId == profile.UserId && x.Unread == true);
                }
            }
            return counts;
        }

        public int GetInboxItems(long userId)
        {
            int counts = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                counts = dataHelper.Get<Activity>().Count(x => x.UserId == userId && x.Unread == true);
            }
            return counts;
        }

        public int GetApplicationCount(string Username)
        {
            int counts = 0;
            UserProfile profile = MemberService.Instance.Get(Username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var TypeList = new List<int>();

                if (profile.Type == (int)SecurityRoles.Employers)
                {
                    TypeList.Add((int)TrackingTypes.AUTO_MATCHED);
                    TypeList.Add((int)TrackingTypes.APPLIED);
                    TypeList.Add((int)TrackingTypes.WITHDRAWN);
                    counts = dataHelper.Get<Tracking>().Count(x => x.JobseekerId != null && ((x.JobId != null && x.Job.EmployerId == profile.UserId) || x.UserId == profile.UserId) && TypeList.Contains(x.Type) && x.IsDeleted == false);
                }
                else
                {
                    TypeList.Add((int)TrackingTypes.AUTO_MATCHED);
                    TypeList.Add((int)TrackingTypes.APPLIED);
                    TypeList.Add((int)TrackingTypes.WITHDRAWN);
                    counts = dataHelper.Get<Tracking>().Count(x => x.JobId != null && x.UserId == profile.UserId && TypeList.Contains(x.Type) && x.IsDeleted == false);
                }
            }
            return counts;
        }

        public int GetInterviewCount(string username)
        {
            int counts = 0;
            UserProfile profile = MemberService.Instance.Get(username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                if (profile.Type == (int)SecurityRoles.Employers)
                {
                    counts = dataHelper.Get<Interview>().Count(x => x.Tracking.JobseekerId != null && x.UserId == profile.UserId && x.IsDeleted == false);
                }
                else
                {
                    counts = dataHelper.Get<Interview>().Count(x => x.Tracking.JobseekerId == profile.UserId && x.IsDeleted == false);
                }

            }
            return counts;
        }

        public int GetUserCountByCategory(int categoryId)
        {
            int counts = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                counts = dataHelper.Get<UserProfile>().Count(x => x.Type == 5 && x.CategoryId == categoryId);
            }
            return counts;
        }

        public int GetUserCountByCategory(int categoryId, long countryId)
        {
            int counts = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                counts = dataHelper.Get<UserProfile>().Count(x => x.Type == 5 && x.CategoryId == categoryId && x.CountryId == countryId);
            }
            return counts;
        }

        /// <summary>
        /// Gets profile by Name
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public int GetAnnouncements(string username)
        {
            int counts = 0;
            if (!string.IsNullOrEmpty(username))
            {
                UserProfile profile = Get(username);
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    counts = dataHelper.Get<UserAnnouncement>().Count(x => x.UserId == profile.UserId && x.Unread == true
                        && (x.DateCreated.Day <= DateTime.Now.Day && x.DateCreated.Month <= DateTime.Now.Month && x.DateCreated.Year <= DateTime.Now.Year));
                }
            }
            return counts;
        }

        /// <summary>
        /// Gets profile by Name
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        public UserProfile GetByAddress(string address)
#pragma warning restore CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        {
            UserProfile profile = new UserProfile();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                profile = dataHelper.GetSingle<UserProfile>("PermaLink", address);
            }
            return profile;
        }

        public long? GetPhotoId(string area, string Username)
        {
            long? id = null;
            if (!string.IsNullOrEmpty(Username))
            {
                UserInfoEntity profile = GetUserInfo(Username);
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    Photo photo = GetPhotoByArea(profile.Id, area);
                    //dataHelper.Get<Photo>().SingleOrDefault(x => x.UserId == profile.Id && x.IsDeleted == false && ((x.IsApproved == true && x.IsRejected == false && x.InEditMode == false) || (x.IsApproved == false && x.IsRejected == false && x.InEditMode == true)) && x.Area.Equals(area));
                    if (photo != null && photo.Id > 0)
                    {
                        id = photo.Id;
                    }
                }
            }
            return id;
        }

#pragma warning disable CS0246 // The type or namespace name 'Photo' could not be found (are you missing a using directive or an assembly reference?)
        public Photo GetPhoto(long Id)
#pragma warning restore CS0246 // The type or namespace name 'Photo' could not be found (are you missing a using directive or an assembly reference?)
        {
            Photo photo = null;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                photo = dataHelper.Get<Photo>().SingleOrDefault(x => x.Id == Id);
            }
            return photo;
        }

#pragma warning disable CS0246 // The type or namespace name 'Photo' could not be found (are you missing a using directive or an assembly reference?)
        public Photo GetPhoto(string area, string username)
#pragma warning restore CS0246 // The type or namespace name 'Photo' could not be found (are you missing a using directive or an assembly reference?)
        {
            Photo photo = null;
            if (!string.IsNullOrEmpty(username))
            {
                UserProfile profile = Get(username);

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    photo = dataHelper.Get<Photo>().SingleOrDefault(x => x.UserId == profile.UserId && x.IsDeleted == false && ((x.IsApproved == true && x.IsRejected == false && x.InEditMode == false) || (x.IsApproved == false && x.IsRejected == false && x.InEditMode == true)) && x.Area.Equals(area));
                }
            }
            return photo;
        }

#pragma warning disable CS0246 // The type or namespace name 'Photo' could not be found (are you missing a using directive or an assembly reference?)
        public Photo GetPhotoByUserID(string area, long userID)
#pragma warning restore CS0246 // The type or namespace name 'Photo' could not be found (are you missing a using directive or an assembly reference?)
        {
            Photo photo = null;
            long profileUserId = Get(userID).UserId;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                photo = dataHelper.Get<Photo>().SingleOrDefault(x => x.UserId == profileUserId && x.IsDeleted == false && ((x.IsApproved == true && x.IsRejected == false && x.InEditMode == false) || (x.IsApproved == false && x.IsRejected == false && x.InEditMode == true)) && x.Area.Equals(area));
            }

            return photo;
        }

#pragma warning disable CS0246 // The type or namespace name 'Photo' could not be found (are you missing a using directive or an assembly reference?)
        public Photo GetBackgroundPhoto(string Username)
#pragma warning restore CS0246 // The type or namespace name 'Photo' could not be found (are you missing a using directive or an assembly reference?)
        {
            UserProfile profile = Get(Username);
            Photo photo = new Photo();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                photo = dataHelper.Get<Photo>().SingleOrDefault(x => x.UserId == profile.UserId && x.IsDeleted == false && ((x.IsApproved == true && x.IsRejected == false && x.InEditMode == false) || (x.IsApproved == false && x.IsRejected == false && x.InEditMode == true)) && x.Area.Equals("Background"));
            }
            return photo;
        }

#pragma warning disable CS0246 // The type or namespace name 'Photo' could not be found (are you missing a using directive or an assembly reference?)
        public Photo GetAvtar(string Username)
#pragma warning restore CS0246 // The type or namespace name 'Photo' could not be found (are you missing a using directive or an assembly reference?)
        {
            UserProfile profile = Get(Username);
            Photo photo = new Photo();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                photo = dataHelper.Get<Photo>().SingleOrDefault(x => x.UserId == profile.UserId && x.IsDeleted == false && ((x.IsApproved == true && x.IsRejected == false && x.InEditMode == false) || (x.IsApproved == false && x.IsRejected == false && x.InEditMode == true)) && x.Area.Equals("Profile"));
            }
            return photo;
        }

        /// <summary>
        /// Gets profile by Name
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        public UserProfile Get(string Username)
#pragma warning restore CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        {
            UserProfile profile = new UserProfile();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                profile = dataHelper.GetSingle<UserProfile>("Username", Username);
            }
            return profile;
        }

        public string ProfileLinkAndLogo(string Username)
        {
            string link = string.Empty;
            string image = string.Empty;
            UserProfile profile = Get(Username);
            SecurityRoles type = (SecurityRoles)profile.Type;
            string imgUrl = string.Format("/Image/Avtar?Id={0}", profile.UserId);

            switch (type)
            {
                case SecurityRoles.Jobseeker:
                    image = string.Format("<img src=\"{0}\" />", imgUrl);
                    link = string.Format("<a id=\"photoUploadDialog\" href=\"/{0}\" target=\"_blank\" title=\"{1} {2}\">{3}</a>", profile.PermaLink, profile.FirstName, profile.LastName, image);
                    break;
                case SecurityRoles.Employers:
                    image = string.Format("<img src=\"{0}\" />", imgUrl);
                    link = string.Format("<a id=\"photoUploadDialog\" href=\"/{0}\" target=\"_blank\" title=\"{1}\">{2}</a>", profile.PermaLink, profile.Company, image);
                    break;
            }

            return link;
        }
        public string ProfileLinkAndLogo(string Username, string cssClass)
        {
            string link = string.Empty;
            string image = string.Empty;
            UserProfile profile = Get(Username);
            SecurityRoles type = (SecurityRoles)profile.Type;
            string imgUrl = string.Format("/Image/Avtar?Id={0}", profile.UserId);

            switch (type)
            {
                case SecurityRoles.Jobseeker:
                    image = string.Format("<img src=\"{0}\" class=\"{1}\" />", imgUrl, cssClass);
                    link = string.Format("<a id=\"photoUploadDialog\" href=\"/{0}\" target=\"_blank\" title=\"{1} {2}\">{3}</a>", profile.PermaLink, profile.FirstName, profile.LastName, image);
                    break;
                case SecurityRoles.Employers:
                    image = string.Format("<img src=\"{0}\" class=\"{1}\" />", imgUrl, cssClass);
                    link = string.Format("<a id=\"photoUploadDialog\" href=\"/{0}\" target=\"_blank\" title=\"{1}\">{2}</a>", profile.PermaLink, profile.Company, image);
                    break;
            }


            return link;

        }

        public string ProfileLinkAndLogo(string Username, int height, int width)
        {
            string link = string.Empty;
            string image = string.Empty;
            string imageData = string.Empty;
            UserProfile profile = Get(Username);
            SecurityRoles type = (SecurityRoles)profile.Type;
            string imgUrl = string.Format("/Image/Avtar?Id={0}&height={1}&width={2}", profile.UserId, height, width);

            switch (type)
            {
                case SecurityRoles.Jobseeker:
                    image = string.Format("<img src=\"{0}\" />", imgUrl);
                    link = string.Format("<a id=\"photoUploadDialog\" href=\"/{0}\" target=\"_blank\" data-toggle=\"tooltip\" title=\"{1} {2}\">{3}</a>", profile.PermaLink, profile.FirstName, profile.LastName, image);
                    break;
                case SecurityRoles.Employers:
                    image = string.Format("<img src=\"{0}\" />", imgUrl);
                    link = string.Format("<a id=\"photoUploadDialog\" href=\"/{0}\" target=\"_blank\" data-toggle=\"tooltip\" title=\"{1}\">{2}</a>", profile.PermaLink, profile.Company, image);
                    break;
            }


            return link;

        }

        public string ProfileLinkAndUploadLogo(string Username, int height, int width)
        {
            string link = string.Empty;
            string image = string.Empty;
            UserProfile profile = Get(Username);
            SecurityRoles type = (SecurityRoles)profile.Type;
            string imgUrl = string.Format("/Image/Avtar?Id={0}&height={1}&width={2}", profile.UserId, height, width);

            switch (type)
            {
                case SecurityRoles.Jobseeker:
                    image = string.Format("<img src=\"{0}\" />", imgUrl);
                    link = string.Format("<a id=\"photoUploadDialog\" href=\"/#\" role=\"button\" data-toggle='modal' data-target='#uploadDialog' title=\"Upload Photo\">{0}</a>", image);
                    break;
                case SecurityRoles.Employers:
                    image = string.Format("<img src=\"{0}\" />", imgUrl);
                    link = string.Format("<a id=\"photoUploadDialog\" href=\"/#\" role=\"button\" data-toggle='modal' data-target='#uploadDialog' title=\"Upload Photo\">{0}</a>", image);
                    break;
            }


            return link;

        }

        public string ProfileLinkAndUploadLogo(string Username)
        {
            string link = string.Empty;
            string image = string.Empty;
            UserProfile profile = Get(Username);
            SecurityRoles type = (SecurityRoles)profile.Type;
            string imgUrl = string.Format("/Image/Avtar?Id={0}", profile.UserId);

            switch (type)
            {
                case SecurityRoles.Jobseeker:
                    image = string.Format("<img src=\"{0}\" />", imgUrl);
                    link = string.Format("<a id=\"photoUploadDialog\" href=\"/#\" role=\"button\" data-toggle='modal' data-target='#uploadDialog' title=\"Upload Photo\">{0}</a>", image);
                    break;
                case SecurityRoles.Employers:
                    image = string.Format("<img src=\"{0}\" />", imgUrl);
                    link = string.Format("<a id=\"photoUploadDialog\" href=\"/#\" role=\"button\" data-toggle='modal' data-target='#uploadDialog' title=\"Upload Photo\">{0}</a>", image);
                    break;
            }

            return link;

        }

        public string ProfileLink(string Username)
        {
            string link = string.Empty;
            int type;
            UserProfile profile = Get(Username);
            type = profile.Type;

            if (type == 4)
            {
                link = string.Format("<a href=\"/{0}\" target=\"_blank\" title=\"{1} {2}\">{1} {2}</a>", profile.PermaLink, profile.FirstName, profile.LastName);
            }
            else if (type == 5)
            {
                link = string.Format("<a href=\"/{0}\" target=\"_blank\" title=\"{1}\">{1}</a>", profile.PermaLink, profile.Company);
            }
            return link;
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'Activity' could not be found (are you missing a using directive or an assembly reference?)
        public void Track(Activity entity)
#pragma warning restore CS0246 // The type or namespace name 'Activity' could not be found (are you missing a using directive or an assembly reference?)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                dataHelper.Add<Activity>(entity);
            }
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'Activity' could not be found (are you missing a using directive or an assembly reference?)
        public void Track(Activity entity, out long id)
#pragma warning restore CS0246 // The type or namespace name 'Activity' could not be found (are you missing a using directive or an assembly reference?)
        {
            id = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                id = Convert.ToInt64(dataHelper.Add<Activity>(entity));
            }
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'AlertHistory' could not be found (are you missing a using directive or an assembly reference?)
        public void TrackAlertHistory(AlertHistory entity)
#pragma warning restore CS0246 // The type or namespace name 'AlertHistory' could not be found (are you missing a using directive or an assembly reference?)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                dataHelper.Add<AlertHistory>(entity);
            }
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        public void Update(UserProfile entity)
#pragma warning restore CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        {
            UserProfile profile = new UserProfile();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                dataHelper.Update<UserProfile>(entity, entity.Username);
            }
        }

        /// <summary>
        /// Gets Profile by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        public UserProfile Get(long Id)
#pragma warning restore CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        {
            UserProfile profile = new UserProfile();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                profile = dataHelper.GetSingle<UserProfile>("UserId", Id);
            }
            return profile;
        }

        /// <summary>
        /// Gets the list of connection by Username
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public int GetConnectionCounts(string Username)
        {
            UserProfile profile = new UserProfile();
            int count = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                profile = dataHelper.GetSingle<UserProfile>("Username", Username);
                count = profile.Connections.Count();
            }
            return count;
        }

        /// <summary>
        /// Gets the message count by Username
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public int GetMessageCounts(string Username)
        {
            UserProfile profile = new UserProfile();
            int count = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                profile = dataHelper.GetSingle<UserProfile>("Username", Username);
                count = profile.Communications.Count();
            }
            return count;
        }

        /// <summary>
        /// Gets the list of connection by Username
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'Connection' could not be found (are you missing a using directive or an assembly reference?)
        public List<Connection> GetConnections(string Username)
#pragma warning restore CS0246 // The type or namespace name 'Connection' could not be found (are you missing a using directive or an assembly reference?)
        {
            UserProfile profile = new UserProfile();
            List<Connection> connections = new List<Connection>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                profile = dataHelper.GetSingle<UserProfile>("Username", Username);
                connections = profile.Connections.ToList();
            }
            return connections;
        }

        /// <summary>
        /// Update user's online status.
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Status"></param>
#pragma warning disable CS0103 // The name 'OnlineStatus' does not exist in the current context
#pragma warning disable CS0246 // The type or namespace name 'OnlineStatus' could not be found (are you missing a using directive or an assembly reference?)
        public void UpdateStatus(string username, OnlineStatus status = OnlineStatus.Online)
#pragma warning restore CS0246 // The type or namespace name 'OnlineStatus' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0103 // The name 'OnlineStatus' does not exist in the current context
        {
            var profile = Instance.Get(username);
            if (profile != null)
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("Id", profile.UserId);
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    var onlineUser = dataHelper.GetSingle<OnlineUser>(parameters);
                    if (onlineUser == null)
                    {
                        onlineUser = new OnlineUser
                        {
                            Id = profile.UserId,
                            Status = (int)status,
                            OnlineSince = DateTime.Now
                        };
                        dataHelper.Add(onlineUser, username, "UniqueId");
                    }
                    else
                    {
                        onlineUser.Status = (int)status;
                        dataHelper.Update<OnlineUser>(onlineUser);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the list of latest 10 records for login history
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'LoginHistory' could not be found (are you missing a using directive or an assembly reference?)
        public List<LoginHistory> GetLoginHistory(string username)
#pragma warning restore CS0246 // The type or namespace name 'LoginHistory' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<LoginHistory> history = new List<LoginHistory>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                history = dataHelper.Get<LoginHistory>().Where(x => x.Username.Equals(username)).OrderByDescending(x => x.LoginDateTime).Take(10).ToList();
            }
            return history;
        }

        public int CountWaitingForAcceptance(long userid)
        {
            UserInfoEntity profile = GetUserInfo(userid);
            int rows = ReadDataField<int>(string.Format("SELECT COUNT(1) FROM Connections WHERE IsDeleted = 0 AND IsAccepted = 0 AND IsConnected = 0 AND Initiated = 1 AND UserId = {0} AND EmailAddress = '{1}'", userid, profile.Username));
            return rows;
        }

#pragma warning disable CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<ConnectionEntity> GetUserConnectionList(long userId, string username, int pageNumber = 1)
#pragma warning restore CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@Username", username));
            parameters.Add(new SqlParameter("@PageNumber", pageNumber));

            return ReadData<ConnectionEntity>("GetUserConnectionList", parameters);
        }

        public bool IsExist(string username)
        {
            int exist = ReadDataField<int>(string.Format("SELECT COUNT(1) FROM UserProfiles WHERE LTRIM(RTRIM(LOWER(Username))) = '{0}'", username));
            return (exist > 0);
        }
#pragma warning disable CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'RegisterEntity' could not be found (are you missing a using directive or an assembly reference?)
        public UserInfoEntity Register(RegisterEntity model)
#pragma warning restore CS0246 // The type or namespace name 'RegisterEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            string premalink = string.Empty;
            var pattern = "\\s+";
            var replacement = " ";
            switch ((SecurityRoles)model.Type)
            {
                case SecurityRoles.Jobseeker:
                    premalink = string.Format("{0}-{1}", model.FirstName.ToLower().Replace(" ", "-"), model.LastName.ToLower().Replace(" ", "-"));
                    premalink = premalink.Replace('.', ' ')
                            .Replace(',', ' ')
                            .Replace('-', ' ')
                            .Replace(" - ", " ")
                            .Replace(" , ", " ")
                            .Replace('/', ' ')
                            .Replace(" / ", " ")
                            .Replace("&", " ")
                            .Replace("#", " ")
                            .Replace(" & ", " ")
                            .Replace(" . ", " ")
                            .Replace("  .", " ")
                            .Replace(". ", " ");

                    premalink = Regex.Replace(premalink, pattern, replacement).Trim().ToLower();
                    premalink = premalink.Replace(' ', '-');
                    model.PermaLink = premalink;

                    break;
                case SecurityRoles.Student:
                    premalink = string.Format("{0}-{1}", model.FirstName.ToLower().Replace(" ", "-"), model.LastName.ToLower().Replace(" ", "-"));
                    premalink = premalink.Replace('.', ' ')
                            .Replace(',', ' ')
                            .Replace('-', ' ')
                            .Replace(" - ", " ")
                            .Replace(" , ", " ")
                            .Replace('/', ' ')
                            .Replace(" / ", " ")
                            .Replace("&", " ")
                            .Replace("#", " ")
                            .Replace(" & ", " ")
                            .Replace(" . ", " ")
                            .Replace("  .", " ")
                            .Replace(". ", " ");

                    premalink = Regex.Replace(premalink, pattern, replacement).Trim().ToLower();
                    premalink = premalink.Replace(' ', '-');
                    model.PermaLink = premalink;

                    break;
                case SecurityRoles.Interns:
                    premalink = string.Format("{0}-{1}", model.FirstName.ToLower().Replace(" ", "-"), model.LastName.ToLower().Replace(" ", "-"));
                    premalink = premalink.Replace('.', ' ')
                            .Replace(',', ' ')
                            .Replace('-', ' ')
                            .Replace(" - ", " ")
                            .Replace(" , ", " ")
                            .Replace('/', ' ')
                            .Replace(" / ", " ")
                            .Replace("&", " ")
                            .Replace("#", " ")
                            .Replace(" & ", " ")
                            .Replace(" . ", " ")
                            .Replace("  .", " ")
                            .Replace(". ", " ");

                    premalink = Regex.Replace(premalink, pattern, replacement).Trim().ToLower();
                    premalink = premalink.Replace(' ', '-');
                    model.PermaLink = premalink;

                    break;
                case SecurityRoles.Employers:
                    premalink = model.Company.ToLower().Replace(" ", "-");
                    premalink =
                        premalink.Replace('.', ' ')
                            .Replace(',', ' ')
                            .Replace('-', ' ')
                            .Replace(" - ", " ")
                            .Replace(" , ", " ")
                            .Replace('/', ' ')
                            .Replace(" / ", " ")
                            .Replace("&", " ")
                            .Replace("#", " ")
                            .Replace(" & ", " ")
                            .Replace(" . ", " ")
                            .Replace("  .", " ")
                            .Replace(". ", " ");

                    premalink = Regex.Replace(premalink, pattern, replacement).Trim().ToLower();
                    premalink = premalink.Replace(' ', '-');
                    model.PermaLink = premalink;
                    break;
                case SecurityRoles.Institute:
                    premalink = model.University.ToLower().Replace(" ", "-");
                    premalink =
                        premalink.Replace('.', ' ')
                            .Replace(',', ' ')
                            .Replace('-', ' ')
                            .Replace(" - ", " ")
                            .Replace(" , ", " ")
                            .Replace('/', ' ')
                            .Replace(" / ", " ")
                            .Replace("&", " ")
                            .Replace("#", " ")
                            .Replace(" & ", " ")
                            .Replace(" . ", " ")
                            .Replace("  .", " ")
                            .Replace(". ", " ");

                    premalink = Regex.Replace(premalink, pattern, replacement).Trim().ToLower();
                    premalink = premalink.Replace(' ', '-');
                    model.PermaLink = premalink;
                    break;

            }

            return ReadSingleData<UserInfoEntity, RegisterEntity>("RegisterUser1", model);
        }

        public int UpdateFriendRequest(long id)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@ConnectionId", id));
            int stat = HandleData("UpdateFriendRequest", parameters);

            return stat;
        }

        public int UpdateConnections(string username)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@Username", username));
            int stat = HandleData("UpdateConnections", parameters);
            return stat;
        }
    }
}