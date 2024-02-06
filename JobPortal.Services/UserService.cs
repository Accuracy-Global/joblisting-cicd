#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Services
{
    public class UserService : DataService, IUserService
    {
#pragma warning disable CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        public UserInfoEntity Get(long id)
#pragma warning restore CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("UserId", id));
            return Single<UserInfoEntity>("GetUserInfo", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        public UserInfoEntity Get(string username)
#pragma warning restore CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("Username", username));
            UserInfoEntity info = Single<UserInfoEntity>("GetUserInfo", parameters);

            return info;
        }

#pragma warning disable CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        public UserInfoEntity GetByAddress(string address)
#pragma warning restore CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("PermaLink", address));
            
            UserInfoEntity info = Single<UserInfoEntity>("GetUserInfo", parameters);
            return info;
        }

        public int TrackLoginHistory(string username, string ipAddress, string browser, string comments)
        {
            int stat = 0;
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("Username", username));
            parameters.Add(new Parameter("Browser", browser));
            parameters.Add(new Parameter("IPAddress", ipAddress));

            if (!string.IsNullOrEmpty(comments))
            {
                parameters.Add(new Parameter("Comments", comments));
            }
            stat = HandleData("ManageLoginHistory", parameters);
            return stat;
        }


        public int TrackUserStatus(long userId, int status, DateTime onlineSince)
        {
            int stat = 0;
            List<Parameter> parameters = new List<Parameter>();

            parameters.Add(new Parameter("Id", userId));
            parameters.Add(new Parameter("Status", status));
            parameters.Add(new Parameter("OnlineSince", onlineSince));

            stat = HandleData("ManageOnlineUser", parameters);
            return stat;
        }


        public int Confirm(long id)
        {
            int stat = 0;
            List<Parameter> parameters = new List<Parameter>();

            parameters.Add(new Parameter("UserId", id));
            stat = HandleData("ConfirmUserAccount", parameters);

            return stat;
        }
        public int Confirm(string username, string token)
        {
            int stat = 0;
            List<Parameter> parameters = new List<Parameter>();

            parameters.Add(new Parameter("Username", username));
            parameters.Add(new Parameter("Token", token));
            stat = HandleData("ConfirmUserAccount", parameters);

            return stat;
        }

        public int GenerateToken(long id, string token)
        {
            int stat = 0;
            List<Parameter> parameters = new List<Parameter>();

            parameters.Add(new Parameter("UserId", id));
            parameters.Add(new Parameter("Token", token));
            stat = HandleData("GenerateToken", parameters);

            return stat;
        }

        public string GenerateTokenMobile(string deviceId, string username, string token)
        {
            int stat = 0;
            List<Parameter> parameters = new List<Parameter>();

            parameters.Add(new Parameter("DeviceId", deviceId));
            parameters.Add(new Parameter("Username", username));
            parameters.Add(new Parameter("Token", token));
            stat = HandleData("GenerateTokenMobile", parameters);
            if (stat <= 0)
            {
                token = string.Empty;
            }
            return token;
        }
        public int TrackProfileView(long profileId, long? userId = null)
        {
            int stat = 0;
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("ProfileId", profileId));
            parameters.Add(new Parameter("UserId", userId));
            stat = HandleData("ManageProfileView", parameters);

            return stat;
        }

        public int ManageAccount(long id, int? pdebit = null, int? mdebit = null, int? idebit = null, int? rdebit = null, int? jdebit = null, long? referenceId = null, long? jobId = null)
        {
            List<Parameter> parameters = new List<Parameter>();

            parameters.Add(new Parameter("UserId", id));
            parameters.Add(new Parameter("JobDebit", jdebit));
            parameters.Add(new Parameter("ProfileDebit", pdebit));
            parameters.Add(new Parameter("MessageDebit", mdebit));
            parameters.Add(new Parameter("InterviewDebit", idebit));
            parameters.Add(new Parameter("ResumeDebit", rdebit));
            parameters.Add(new Parameter("ReferenceId", referenceId));
            parameters.Add(new Parameter("JobId", jobId));

            return HandleData("ManageAccount", parameters);
        }

        public int ManageAccount(long senderId, long receiverId, int? pdebit = null, int? mdebit = null, int? idebit = null, int? rdebit = null, int? jdebit = null, long? referenceId = null, long? jobId = null)
        {
            List<Parameter> parameters = new List<Parameter>();

            parameters.Add(new Parameter("UserId", senderId));
            parameters.Add(new Parameter("SenderId", senderId));
            parameters.Add(new Parameter("ReceiverId", receiverId));
            parameters.Add(new Parameter("JobDebit", jdebit));
            parameters.Add(new Parameter("ProfileDebit", pdebit));
            parameters.Add(new Parameter("MessageDebit", mdebit));
            parameters.Add(new Parameter("InterviewDebit", idebit));
            parameters.Add(new Parameter("ResumeDebit", rdebit));
            parameters.Add(new Parameter("ReferenceId", referenceId));
            parameters.Add(new Parameter("JobId", jobId));

            return HandleData("ManageAccount", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'BlockedEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<BlockedEntity> GetBlockerList(long userId)
#pragma warning restore CS0246 // The type or namespace name 'BlockedEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();

            parameters.Add(new Parameter("BlockedId", userId));
            return Read<BlockedEntity>("GetBlockedList", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'BlockedEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<BlockedEntity> GetBlockedList(long userId)
#pragma warning restore CS0246 // The type or namespace name 'BlockedEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();

            parameters.Add(new Parameter("BlockerId", userId));
            return Read<BlockedEntity>("GetBlockedList", parameters);
        }

        public List<Output> SearchResumes<Output, Input>(Input model)
        {
            List<Output> list = new List<Output>();
            list = Read<Output, Input>("SearchResumes", model);
            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'UserByCountryEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<UserByCountryEntity> CountryWiseUsers(long? countryId = null, DateTime? start = null, DateTime? end = null, bool? pending = null, int pageSize = 111, int pageNumber = 1)
#pragma warning restore CS0246 // The type or namespace name 'UserByCountryEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<UserByCountryEntity> list = new List<UserByCountryEntity>();
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("CountryId", countryId));
            parameters.Add(new Parameter("StartDate", start));
            parameters.Add(new Parameter("EndDate", end));
            parameters.Add(new Parameter("Pending", pending));
            parameters.Add(new Parameter("PageSize", pageSize));
            parameters.Add(new Parameter("PageNumber", pageNumber));

            list = Read<UserByCountryEntity>("CountUsersByCountry", parameters);

            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<UserInfoEntity> GetUsers(int? type = null, long? countryId = null, DateTime? start = null, DateTime? end = null, string name = null, bool? confirmed = null, bool? active = null, int pageSize = 10, int pageNumber = 1)
#pragma warning restore CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<UserInfoEntity> list = new List<UserInfoEntity>();
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("Type", type));
            parameters.Add(new Parameter("Name", name));
            parameters.Add(new Parameter("Confirmed", confirmed));
            parameters.Add(new Parameter("Active", active));
            parameters.Add(new Parameter("CountryId", countryId));
            parameters.Add(new Parameter("StartDate", start));
            parameters.Add(new Parameter("EndDate", end));
            parameters.Add(new Parameter("PageSize", pageSize));
            parameters.Add(new Parameter("PageNumber", pageNumber));

            list = Read<UserInfoEntity>("GetUserList", parameters);

            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<UserInfoEntity> GetJobseekers(string type = null, long? countryId = null, DateTime? start = null, DateTime? end = null, string name = null, int pageSize = 10, int pageNumber = 1)
#pragma warning restore CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<UserInfoEntity> list = new List<UserInfoEntity>();
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("CountryId", countryId));
            parameters.Add(new Parameter("StartDate", start));
            parameters.Add(new Parameter("EndDate", end));
            parameters.Add(new Parameter("Type", type));
            parameters.Add(new Parameter("Name", name));
            parameters.Add(new Parameter("PageSize", pageSize));
            parameters.Add(new Parameter("PageNumber", pageNumber));

            list = Read<UserInfoEntity>("GetJobseekerList", parameters);

            return list;
        }


        public int ChangeEmail(string existing_email, string new_email, string token)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("OldEmail", existing_email));
            parameters.Add(new Parameter("NewEmail", new_email));
            parameters.Add(new Parameter("Token", token));

            return HandleData("ChangeEmail", parameters);
        }
#pragma warning disable CS0246 // The type or namespace name 'LoginHistoryEntity' could not be found (are you missing a using directive or an assembly reference?)
        public LoginHistoryEntity GetLoginHistory(string username)
#pragma warning restore CS0246 // The type or namespace name 'LoginHistoryEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            return Single<LoginHistoryEntity>(string.Format("SELECT TOP 1 Id, Username, IPAddress, LoginDateTime, Browser, Comments, 1 MaxRows FROM LoginHistories WHERE Username = '{0}' ORDER BY Id DESC", username));
        }


        public int Confirm(long id, string type = null)
        {
            int stat = 0;
            List<Parameter> parameters = new List<Parameter>();

            parameters.Add(new Parameter("UserId", id));
            parameters.Add(new Parameter("Type", type));
            stat = HandleData("ConfirmUserAccount", parameters);

            return stat;
        }


        public long IsActiveSession(string deviceId, string token)
        {
            long stat = 0;
            List<Parameter> parameters = new List<Parameter>();

            parameters.Add(new Parameter("DeviceId", deviceId));
            parameters.Add(new Parameter("Token", token));
            stat = Scaler<long>("IsActiveSession", parameters);

            return stat;
        }


#pragma warning disable CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<ContactEntity> GetContactList(long userId, int? offset, int? pageSize)
#pragma warning restore CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("UserId", userId));
            parameters.Add(new Parameter("PageIndex", offset));
            parameters.Add(new Parameter("PageSize", pageSize));

            return Read<ContactEntity>("GetContacts", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<ContactEntity>> GetContactListAsync(long userId, int? offset, int? pageSize)
#pragma warning restore CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("UserId", userId));
            parameters.Add(new Parameter("PageNumber", offset));
            parameters.Add(new Parameter("PageSize", pageSize));

            return await ReadAsync<ContactEntity>("ContactList", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'Country' could not be found (are you missing a using directive or an assembly reference?)
        public Country GetCountryByCode(string code)
#pragma warning restore CS0246 // The type or namespace name 'Country' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("Code", code));
            return Single<Country>("CountrySingle", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'PeopleSearchModel' could not be found (are you missing a using directive or an assembly reference?)
        public List<ContactEntity> Search(PeopleSearchModel model)
#pragma warning restore CS0246 // The type or namespace name 'PeopleSearchModel' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("UserId", model.UserId));
            parameters.Add(new Parameter("Name", model.Name));
            parameters.Add(new Parameter("Gender", model.Gender));
            parameters.Add(new Parameter("CountryId", model.CountryId));
            parameters.Add(new Parameter("IsConnected", model.IsConnected));
            parameters.Add(new Parameter("PageNumber", model.PageNumber));
            parameters.Add(new Parameter("PageSize", model.PageSize));

            return Read<ContactEntity>("PeopleSearch", parameters);
        }
#pragma warning disable CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        public ContactEntity SingleConnection(long loginUserId, long userId)
#pragma warning restore CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("LoginUserId", loginUserId));
            parameters.Add(new Parameter("UserId", userId));

            return Single<ContactEntity>("ConnectionSingleMobile", parameters);
        }
      

        public bool IsConfirmed(string username)
        {
            return Scaler<bool>(string.Format("SELECT IsConfirmed FROM UserProfiles WHERE LOWER(Username) ='{0}'", username.ToLower()));
        }

#pragma warning disable CS0246 // The type or namespace name 'BuyMessage' could not be found (are you missing a using directive or an assembly reference?)
        public BuyMessage MessagePrice(long countryId, int type)
#pragma warning restore CS0246 // The type or namespace name 'BuyMessage' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>(){
                new Parameter("CountryId", countryId),
                new Parameter("Type", "M"),
                new Parameter("UserType", type)
            };
            return Single<BuyMessage>("GetPrice", parameters);
        }
#pragma warning disable CS0246 // The type or namespace name 'BuyResume' could not be found (are you missing a using directive or an assembly reference?)
        public BuyResume ResumeDonwloadPrice(long countryId)
#pragma warning restore CS0246 // The type or namespace name 'BuyResume' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>(){
                new Parameter("CountryId", countryId),
                new Parameter("Type", "R")
            };
            return Single<BuyResume>("GetPrice", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'MessageModel' could not be found (are you missing a using directive or an assembly reference?)
        public List<MessageModel> GetMessageList(string from, string to)
#pragma warning restore CS0246 // The type or namespace name 'MessageModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("From", from));
            parameters.Add(new Parameter("To", to));

            return Read<MessageModel>("MessageList", parameters);
        }
#pragma warning disable CS0246 // The type or namespace name 'PhoneContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        public long ManagePhontContact(PhoneContactEntity model)
#pragma warning restore CS0246 // The type or namespace name 'PhoneContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            object retval = null;
            long stat = 0;
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("Username", model.Username));
            parameters.Add(new Parameter("Name", model.Name));
            parameters.Add(new Parameter("Phone", model.Phone));
            parameters.Add(new Parameter("CountryCode", model.CountryCode));
            parameters.Add(new Parameter("DeviceId", model.DeviceId));

            retval = Scaler("ManagePhoneContacts", parameters);
            if (retval != null)
            {
                stat = Convert.ToInt64(retval);
            }
            return stat;
        }

        public int AcceptRequest(string from, string to)
        {
            int stat = 0;
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("From", from));
            parameters.Add(new Parameter("To", to));

            stat = HandleData("AcceptRequest", parameters);
            return stat;
        }

        public int DeleteAll(string from, string to)
        {
            int stat = 0;
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("From", from));
            parameters.Add(new Parameter("To", to));

            stat = HandleData("DeleteAll", parameters);
            return stat;
        }

#pragma warning disable CS0246 // The type or namespace name 'MessageModel' could not be found (are you missing a using directive or an assembly reference?)
        public MessageModel GetRecentMessage(string from, string to)
#pragma warning restore CS0246 // The type or namespace name 'MessageModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("From", from));
            parameters.Add(new Parameter("To", to));

            return Single<MessageModel>("RecentMessage", parameters);
        }

        public int MessageCount(string from)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("From", from));

            return Scaler<int>("MessageCount", parameters);
        }

        public int ReadMark(string from, string to)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("From", from));
            parameters.Add(new Parameter("To", to));

            return HandleData("MarkAsRead", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'BlockedEntity' could not be found (are you missing a using directive or an assembly reference?)
        public BlockedEntity GetBlockedEntry(long blocked, long blocker)
#pragma warning restore CS0246 // The type or namespace name 'BlockedEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("BlockedId", blocked));
            parameters.Add(new Parameter("BlockerId", blocker));

            return Single<BlockedEntity>("GetBlockedEntry", parameters);
        }

        public bool IsBlockedByMe(long userId, long requestor)
        {
            int blocked = 0;
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("UserId", userId));
            parameters.Add(new Parameter("Requestor", requestor));

            blocked = Scaler<int>("IsBlockedByMe", parameters);

            return (blocked > 0);
        }

        public bool IsBlocked(long userId, long requestor)
        {
            int blocked = 0;
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("UserId", userId));
            parameters.Add(new Parameter("Requestor", requestor));

            blocked = Scaler<int>("IsBlockedBySomeone", parameters);

            return (blocked > 0);
        }

#pragma warning disable CS0246 // The type or namespace name 'StatusEntity' could not be found (are you missing a using directive or an assembly reference?)
        public StatusEntity ManageConnection(long senderId, string receiver)
#pragma warning restore CS0246 // The type or namespace name 'StatusEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("SenderId", senderId));
            parameters.Add(new Parameter("ReceiverEmail", receiver));

            return Single<StatusEntity>("ManageConnection", parameters);
        }

        public int Agree(string deviceId, string name)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("Name", name));
            parameters.Add(new Parameter("DeviceId", deviceId));
            return HandleData("Agree", parameters);
        }

        public bool IsAccepted(string deviceId)
        {
            return Scaler<bool>(string.Format("SELECT CAST(ISNULL(Accepted,0) AS BIT) FROM Agreements WHERE DeviceId='{0}'", deviceId));
        }

#pragma warning disable CS0246 // The type or namespace name 'Agreement' could not be found (are you missing a using directive or an assembly reference?)
        public Agreement GetAgreement(string deviceId)
#pragma warning restore CS0246 // The type or namespace name 'Agreement' could not be found (are you missing a using directive or an assembly reference?)
        {
            return Single<Agreement>(string.Format("SELECT * FROM Agreements WHERE DeviceId='{0}'", deviceId));
        }


        public int RecordSMSStatus(long id, string reason = null)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("Id", id));
            parameters.Add(new Parameter("Reason", reason));
            return HandleData("RecordSMSStatus", parameters);
        }

        public long ManageTransaction(long id, int packageId, string transactionId = null, string method = null)
        {
            object rvalue = null;
            long value = 0;
            List<Parameter> parameters = new List<Parameter>();

            parameters.Add(new Parameter("UserId", id));
            parameters.Add(new Parameter("PackageId", packageId));
            parameters.Add(new Parameter("TransactionId", transactionId));
            parameters.Add(new Parameter("PaymentMethod", method));

            rvalue = Scaler("ManageTransaction", parameters);
            if (rvalue != null)
            {
                value = Convert.ToInt64(rvalue);
            }
            return value;
        }

#pragma warning disable CS0246 // The type or namespace name 'PhoneContactReminder' could not be found (are you missing a using directive or an assembly reference?)
        public List<PhoneContactReminder> GetPhoneContacts()
#pragma warning restore CS0246 // The type or namespace name 'PhoneContactReminder' could not be found (are you missing a using directive or an assembly reference?)
        {
            return Read<PhoneContactReminder>("PhoheContactList");
        }

#pragma warning disable CS0246 // The type or namespace name 'MatchEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<MatchEntity> JobMatchList(long userId, int pageNumber = 0, int pageSize = 10)
#pragma warning restore CS0246 // The type or namespace name 'MatchEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();

            parameters.Add(new Parameter("UserId", userId));
            parameters.Add(new Parameter("PageNumber", pageNumber));
            parameters.Add(new Parameter("PageSize", pageSize));

            return Read<MatchEntity>("JobMatchList", parameters);
        }


#pragma warning disable CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<PeopleEntity>> SearchDirectory(SearchResume model)
#pragma warning restore CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
        {           
            return await ReadAsync<PeopleEntity, SearchResume>("SearchPeople", model);
        }

        /// <summary>
        /// Search jobseekers as per the criteria
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<PeopleEntity>> Jobseekers(SearchResume model)
#pragma warning restore CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
        {
            return await ReadAsync<PeopleEntity, SearchResume>("SearchJobseekers", model);
        }

#pragma warning disable CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
        public List<PeopleEntity> PromotedProfiles(SearchResume model)
#pragma warning restore CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", model.UserId),
                new Parameter("Name", model.Name),
                new Parameter("Where", model.Where),
                new Parameter("AgeMin", model.AgeMin),
                new Parameter("AgeMax", model.AgeMax),
                new Parameter("Gender", model.Gender),
                new Parameter("Relationship", model.Relationship),
                new Parameter("PageNumber", model.PageNumber),
                new Parameter("PageSize", model.PageSize)
            };

            return Read<PeopleEntity>("PromotedSearchPeople", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<PeopleEntity> PromotedJobseekers(SearchResume model)
#pragma warning restore CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", model.UserId),
                new Parameter("Name", model.Name),
                new Parameter("Where", model.Where),
                new Parameter("AgeMin", model.AgeMin),
                new Parameter("AgeMax", model.AgeMax),
                new Parameter("Gender", model.Gender),
                new Parameter("Relationship", model.Relationship),
                new Parameter("PageNumber", model.PageNumber),
                new Parameter("PageSize", model.PageSize)
            };
            
            return Read<PeopleEntity>("GetRecentProfiles", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'UserFilter' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
        public Task<List<PeopleEntity>> Search(UserFilter model)
#pragma warning restore CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'UserFilter' could not be found (are you missing a using directive or an assembly reference?)
        {
            throw new NotImplementedException();
        }

#pragma warning disable CS0246 // The type or namespace name 'ApplicationEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<ApplicationEntity>> Applictions(long userId,string title = null, string company=null, DateTime? start = null, DateTime? end = null, int pageNumber = 1, int pageSize = 10)
#pragma warning restore CS0246 // The type or namespace name 'ApplicationEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("UserId", userId));
            parameters.Add(new Parameter("Title", title));
            parameters.Add(new Parameter("Company", company));
            parameters.Add(new Parameter("Start", start));
            parameters.Add(new Parameter("End", end));
            parameters.Add(new Parameter("PageNumber", pageNumber));
            parameters.Add(new Parameter("PageSize", pageSize));
            return await ReadAsync<ApplicationEntity>("ApplicationList", parameters);
        }


        public async Task<int> Unsubscribe(int type, string email)
        {            
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("AlertId", type));
            parameters.Add(new Parameter("Username", email));

            return await HandleDataAsync("Unsubscribe", parameters);            
        }


#pragma warning disable CS0246 // The type or namespace name 'ProfileViewEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<ProfileViewEntity>> AnonymousViews(long profileId, int page)
#pragma warning restore CS0246 // The type or namespace name 'ProfileViewEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();

            parameters.Add(new Parameter("Id", profileId));
            parameters.Add(new Parameter("PageNumber", page));
            return await ReadAsync<ProfileViewEntity>("AnonymousProfileViews", parameters);
        }


#pragma warning disable CS0246 // The type or namespace name 'DownloadEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<DownloadEntity>> DownloadHistory(long userId, string title = null, long? countryId = null, DateTime? start = null, DateTime? end = null, int page = 1)
#pragma warning restore CS0246 // The type or namespace name 'DownloadEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("UserId", userId));
            parameters.Add(new Parameter("Title", title));
            parameters.Add(new Parameter("CountryId", countryId));
            parameters.Add(new Parameter("Start", start));
            parameters.Add(new Parameter("End", end));
            parameters.Add(new Parameter("PageNumber", page));
            //parameters.Add(new Parameter("PageSize", pageSize));
            return await ReadAsync<DownloadEntity>("DownloadHistory", parameters);
        }

        public int ProfileViewedMarkAsRead(long profileId, long userId)
        {
            int stat = 0;
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("ProfileId", userId));
            parameters.Add(new Parameter("UserId", profileId));
            stat = HandleData("ProfileViewedMarkAsRead", parameters);

            return stat;
        }

        public int ProfileViews(long profileId)
        {            
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("ProfileId", profileId));
            return  Scaler<int>("ProfileViewCount", parameters);            
        }


        public int JobCounts(long userId)
        {
            return Scaler<int>(string.Format("SELECT COUNT(1) FROM Jobs A INNER JOIN Inbox B ON B.ReferenceId = A.Id WHERE B.Unread = 1 AND A.EmployerId = {0}", userId));
        }


#pragma warning disable CS0246 // The type or namespace name 'UserMatchEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<UserMatchEntity>> PeopleMatchList(long userId, int page = 1, int size = 10)
#pragma warning restore CS0246 // The type or namespace name 'UserMatchEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", userId),
                new Parameter("PageNumber", page),
                new Parameter("PageSize", size)
            };
            return await ReadAsync<UserMatchEntity>("PeopleMatchList", parameters);
        }

        public int PeopleMatchCounts(long userId)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", userId)              
            };
            return Scaler<int>("PeopleMatchCount", parameters);
        }


#pragma warning disable CS0246 // The type or namespace name 'UserMatchEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<UserMatchEntity>> PeopleMatchList(long userId)
#pragma warning restore CS0246 // The type or namespace name 'UserMatchEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", userId)              
            };
            return await ReadAsync<UserMatchEntity>("PeopleMatchListAll", parameters);
        }


#pragma warning disable CS0246 // The type or namespace name 'CreditEntry' could not be found (are you missing a using directive or an assembly reference?)
        public long ManageTransaction(CreditEntry model)
#pragma warning restore CS0246 // The type or namespace name 'CreditEntry' could not be found (are you missing a using directive or an assembly reference?)
        {
            
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", model.UserId),
                new Parameter("JobId", model.JobId),
                new Parameter("PackageId", model.PackageId),
                new Parameter("Description", model.Description),
                new Parameter("BillingZip", model.BillingZip),
                new Parameter("Amount", model.Amount),                
                new Parameter("TransactionId", model.TransactionId),
                new Parameter("PaymentMethod", model.Method),
                new Parameter("Profiles", model.Profiles),
                new Parameter("Messages", model.Messages),
                new Parameter("Interviews", model.Interviews),
                new Parameter("Resumes", model.Resumes),                
            };

            return Scaler<long>("ManageTransaction", parameters);                        
        }


        public int ApplicationMarkAsViewed(long id)
        {
            int stat = 0;
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("Id", id));
            stat = HandleData("ApplicationMarkAsViewed", parameters);

            return stat;
        }


#pragma warning disable CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<ContactEntity> PaidSearch(int page, long userId, string gender)
#pragma warning restore CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("UserId", userId));
            parameters.Add(new Parameter("Gender", gender));
            parameters.Add(new Parameter("PageNumber", page));
            parameters.Add(new Parameter("PageSize", 1));

            return Read<ContactEntity>("PaidPeopleSearch", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'MessageFilter' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'CommunicationEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<CommunicationEntity>> Messages(MessageFilter filter)
#pragma warning restore CS0246 // The type or namespace name 'CommunicationEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'MessageFilter' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", filter.UserId),
                new Parameter("ReceiverId", filter.ReceiverId),
                new Parameter("Status", filter.Status),
                new Parameter("PageNumber", filter.PageNumber),
                new Parameter("PageSize", filter.PageSize),
            };

            return await ReadAsync<CommunicationEntity>("ListMessages", parameters);
        }


#pragma warning disable CS0246 // The type or namespace name 'ViewedEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<ViewedEntity>> ProfileViewedList(long userId, int page)
#pragma warning restore CS0246 // The type or namespace name 'ViewedEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("ProfileId", userId),
                new Parameter("PageNumber", page)
            };
            return await ReadAsync<ViewedEntity>("WhoViewedMyProfile", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'ProfileViewEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<ProfileViewEntity>> ProfileViewedDetail(long userId, long profileId, int page)
#pragma warning restore CS0246 // The type or namespace name 'ProfileViewEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("ProfileId", profileId),
                new Parameter("UserId", userId),
                new Parameter("PageNumber", page)
            };
            return await ReadAsync<ProfileViewEntity>("WhoViewedMyProfileDetails", parameters);
        }


#pragma warning disable CS0246 // The type or namespace name 'Blocked' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<Blocked>> BlockedProfileList(long userId, int page)
#pragma warning restore CS0246 // The type or namespace name 'Blocked' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", userId),
                new Parameter("PageNumber", page)
            };
            return await ReadAsync<Blocked>("BlockedProfileList", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'JobBookmarked' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'JobBookmarkFilter' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<JobBookmarked>> JobBookmarkedList(JobBookmarkFilter model)
#pragma warning restore CS0246 // The type or namespace name 'JobBookmarkFilter' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'JobBookmarked' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", model.UserId),
                new Parameter("EmployerId", model.EmployerId),
                new Parameter("CountryId", model.CountryId),
                new Parameter("Start", model.Start),
                new Parameter("End", model.End),
                new Parameter("PageNumber",1)
            };
            return await ReadAsync<JobBookmarked>("JobsBookmarkedList", parameters);
        }


#pragma warning disable CS0246 // The type or namespace name 'JobBookmarked' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<JobBookmarked>> JobsBookmarkedCompanyList(long userId)
#pragma warning restore CS0246 // The type or namespace name 'JobBookmarked' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", userId),  
            };
            return await ReadAsync<JobBookmarked>("JobsBookmarkedCompanyList", parameters);
        }


#pragma warning disable CS0246 // The type or namespace name 'ApplicationEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<ApplicationEntity>> ApplictionCompanyList(long userId)
#pragma warning restore CS0246 // The type or namespace name 'ApplicationEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", userId),  
            };
            return await ReadAsync<ApplicationEntity>("ApplicationCompanyList", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'ApplicationEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<ApplicationEntity>> ApplictionJobTitleList(long userId)
#pragma warning restore CS0246 // The type or namespace name 'ApplicationEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", userId),  
            };
            return await ReadAsync<ApplicationEntity>("ApplicationJobTitles", parameters);
        }


#pragma warning disable CS0246 // The type or namespace name 'InterviewEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'InterviewFilter' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<InterviewEntity>> Interviews(InterviewFilter filter)
#pragma warning restore CS0246 // The type or namespace name 'InterviewFilter' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'InterviewEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", filter.UserId),  
                new Parameter("Type", filter.Type),  
                new Parameter("Title", filter.Title),  
                new Parameter("CountryId", filter.CountryId),  
                new Parameter("Status", filter.Status),
                new Parameter("Start", filter.Start),
                new Parameter("End", filter.End),
            };
            return await ReadAsync<InterviewEntity>("InterviewList", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'InterviewEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'InterviewFilter' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<InterviewEntity>> InterviewJobList(InterviewFilter filter)
#pragma warning restore CS0246 // The type or namespace name 'InterviewFilter' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'InterviewEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", filter.UserId),  
                new Parameter("Type", filter.Type)               
            };
            return await ReadAsync<InterviewEntity>("InterviewJobTitleList", parameters);
        }


#pragma warning disable CS0246 // The type or namespace name 'MessageFilter' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'CommunicationEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<CommunicationEntity>> ReceiverList(MessageFilter filter)
#pragma warning restore CS0246 // The type or namespace name 'CommunicationEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'MessageFilter' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", filter.UserId)                
            };

            return await ReadAsync<CommunicationEntity>("ReceiverList", parameters);
        }
    }
}