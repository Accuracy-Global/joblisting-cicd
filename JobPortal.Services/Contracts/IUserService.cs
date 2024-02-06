#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Services.Contracts
{
    public interface IUserService
    {
        int Confirm(long id);
        int Confirm(string username, string token);
        int Confirm(long id, string type = null);
        int ChangeEmail(string existing_email, string new_email, string token);
#pragma warning disable CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        UserInfoEntity Get(long id);
#pragma warning restore CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        UserInfoEntity Get(string username);
#pragma warning restore CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        UserInfoEntity GetByAddress(string address);
#pragma warning restore CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        bool IsConfirmed(string username);
        long IsActiveSession(string deviceId, string token);
#pragma warning disable CS0246 // The type or namespace name 'LoginHistoryEntity' could not be found (are you missing a using directive or an assembly reference?)
        LoginHistoryEntity GetLoginHistory(string username);
#pragma warning restore CS0246 // The type or namespace name 'LoginHistoryEntity' could not be found (are you missing a using directive or an assembly reference?)
        int ManageAccount(long id, int? pdebit = null, int? mdebit = null, int? idebit = null, int? rdebit = null, int? jdebit = null, long? referenceId = null, long? jobId = null);
        int ManageAccount(long senderId, long receiverId, int? pdebit = null, int? mdebit = null, int? idebit = null, int? rdebit = null, int? jdebit = null, long? referenceId = null, long? jobId = null);
        int TrackLoginHistory(string username, string ipAddress, string browser, string comments);
        int TrackUserStatus(long userId, int status, DateTime onlineSince);
        int TrackProfileView(long profileId, long? userId = null);
        int ProfileViewedMarkAsRead(long profileId, long userId);
        int ProfileViews(long profileId);
#pragma warning disable CS0246 // The type or namespace name 'BlockedEntity' could not be found (are you missing a using directive or an assembly reference?)
        List<BlockedEntity> GetBlockerList(long userId);
#pragma warning restore CS0246 // The type or namespace name 'BlockedEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'BlockedEntity' could not be found (are you missing a using directive or an assembly reference?)
        List<BlockedEntity> GetBlockedList(long userId);
#pragma warning restore CS0246 // The type or namespace name 'BlockedEntity' could not be found (are you missing a using directive or an assembly reference?)
        int GenerateToken(long id, string token);
        string GenerateTokenMobile(string deviceId, string username, string token);
        List<Output> SearchResumes<Output, Input>(Input model);
        
#pragma warning disable CS0246 // The type or namespace name 'UserByCountryEntity' could not be found (are you missing a using directive or an assembly reference?)
        List<UserByCountryEntity> CountryWiseUsers(long? countryId = null, DateTime? start = null, DateTime? end = null, bool? pending = null, int pageSize = 111, int pageNumber = 1);
#pragma warning restore CS0246 // The type or namespace name 'UserByCountryEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        List<UserInfoEntity> GetUsers(int? type = null, long? countryId = null, DateTime? start = null, DateTime? end = null, string name = null, bool? confirmed = null, bool? active = null, int pageSize = 10, int pageNumber = 1);
#pragma warning restore CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        List<UserInfoEntity> GetJobseekers(string type = null, long? countryId = null, DateTime? start = null, DateTime? end = null, string name = null, int pageSize = 10, int pageNumber = 1);
#pragma warning restore CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        List<ContactEntity> GetContactList(long userId, int? offset, int? pageSize);
#pragma warning restore CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<ContactEntity>> GetContactListAsync(long userId, int? offset, int? pageSize);
#pragma warning restore CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        //List<ContactEntity> Search(long userId, string name, string gender, int? offset, int? pageSize);
#pragma warning disable CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'PeopleSearchModel' could not be found (are you missing a using directive or an assembly reference?)
        List<ContactEntity> Search(PeopleSearchModel model);
#pragma warning restore CS0246 // The type or namespace name 'PeopleSearchModel' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        ContactEntity SingleConnection(long loginUserId, long userId);
#pragma warning restore CS0246 // The type or namespace name 'ContactEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Country' could not be found (are you missing a using directive or an assembly reference?)
        Country GetCountryByCode(string code);
#pragma warning restore CS0246 // The type or namespace name 'Country' could not be found (are you missing a using directive or an assembly reference?)

#pragma warning disable CS0246 // The type or namespace name 'BuyMessage' could not be found (are you missing a using directive or an assembly reference?)
        BuyMessage MessagePrice(long countryId, int type);
#pragma warning restore CS0246 // The type or namespace name 'BuyMessage' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'BuyResume' could not be found (are you missing a using directive or an assembly reference?)
        BuyResume ResumeDonwloadPrice(long countryId);
#pragma warning restore CS0246 // The type or namespace name 'BuyResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'MessageModel' could not be found (are you missing a using directive or an assembly reference?)
        List<MessageModel> GetMessageList(string from, string to);
#pragma warning restore CS0246 // The type or namespace name 'MessageModel' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'MessageModel' could not be found (are you missing a using directive or an assembly reference?)
        MessageModel GetRecentMessage(string from, string to);
#pragma warning restore CS0246 // The type or namespace name 'MessageModel' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'PhoneContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        long ManagePhontContact(PhoneContactEntity model);
#pragma warning restore CS0246 // The type or namespace name 'PhoneContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        int AcceptRequest(string from, string to);
        int DeleteAll(string from, string to);
        int MessageCount(string from);
        int ReadMark(string from, string to);
        //ConnectionEntity GetConnection(string from, string to);
#pragma warning disable CS0246 // The type or namespace name 'BlockedEntity' could not be found (are you missing a using directive or an assembly reference?)
        BlockedEntity GetBlockedEntry(long blocked, long blocker);
#pragma warning restore CS0246 // The type or namespace name 'BlockedEntity' could not be found (are you missing a using directive or an assembly reference?)
        bool IsBlockedByMe(long userId, long requestor);
        bool IsBlocked(long userId, long requestor);

#pragma warning disable CS0246 // The type or namespace name 'StatusEntity' could not be found (are you missing a using directive or an assembly reference?)
        StatusEntity ManageConnection(long senderId, string receiver);
#pragma warning restore CS0246 // The type or namespace name 'StatusEntity' could not be found (are you missing a using directive or an assembly reference?)
        bool IsAccepted(string deviceId);
        int Agree(string deviceId, string name);
#pragma warning disable CS0246 // The type or namespace name 'Agreement' could not be found (are you missing a using directive or an assembly reference?)
        Agreement GetAgreement(string deviceId);
#pragma warning restore CS0246 // The type or namespace name 'Agreement' could not be found (are you missing a using directive or an assembly reference?)
        int RecordSMSStatus(long id, string reason = null);
        long ManageTransaction(long id, int packageId, string transactionId = null, string method = null);
#pragma warning disable CS0246 // The type or namespace name 'PhoneContactReminder' could not be found (are you missing a using directive or an assembly reference?)
        List<PhoneContactReminder> GetPhoneContacts();
#pragma warning restore CS0246 // The type or namespace name 'PhoneContactReminder' could not be found (are you missing a using directive or an assembly reference?)

#pragma warning disable CS0246 // The type or namespace name 'MatchEntity' could not be found (are you missing a using directive or an assembly reference?)
        List<MatchEntity> JobMatchList(long userId, int pageNumber = 0, int pageSize = 10);
#pragma warning restore CS0246 // The type or namespace name 'MatchEntity' could not be found (are you missing a using directive or an assembly reference?)

#pragma warning disable CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<PeopleEntity>> SearchDirectory(SearchResume model);
#pragma warning restore CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<PeopleEntity>> Jobseekers(SearchResume model);
#pragma warning restore CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
        List<PeopleEntity> PromotedProfiles(SearchResume model);
#pragma warning restore CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
        List<PeopleEntity> PromotedJobseekers(SearchResume model);
#pragma warning restore CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)

#pragma warning disable CS0246 // The type or namespace name 'UserFilter' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<PeopleEntity>> Search(UserFilter model);
#pragma warning restore CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'UserFilter' could not be found (are you missing a using directive or an assembly reference?)

#pragma warning disable CS0246 // The type or namespace name 'ApplicationEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<ApplicationEntity>> Applictions(long userId, string title = null, string company = null, DateTime? start = null, DateTime? end = null, int pageNumber = 1, int pageSize = 10);
#pragma warning restore CS0246 // The type or namespace name 'ApplicationEntity' could not be found (are you missing a using directive or an assembly reference?)

        Task<int> Unsubscribe(int type, string email);

#pragma warning disable CS0246 // The type or namespace name 'ProfileViewEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<ProfileViewEntity>> AnonymousViews(long profileId, int page);
#pragma warning restore CS0246 // The type or namespace name 'ProfileViewEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'DownloadEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<DownloadEntity>> DownloadHistory(long userId, string title = null, long? countryId = null, DateTime? start = null, DateTime? end = null, int page = 1);
#pragma warning restore CS0246 // The type or namespace name 'DownloadEntity' could not be found (are you missing a using directive or an assembly reference?)

        int JobCounts(long userId);
#pragma warning disable CS0246 // The type or namespace name 'UserMatchEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<UserMatchEntity>> PeopleMatchList(long userId, int page = 1, int size = 10);
#pragma warning restore CS0246 // The type or namespace name 'UserMatchEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'UserMatchEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<UserMatchEntity>> PeopleMatchList(long userId);
#pragma warning restore CS0246 // The type or namespace name 'UserMatchEntity' could not be found (are you missing a using directive or an assembly reference?)
        int PeopleMatchCounts(long userId);

#pragma warning disable CS0246 // The type or namespace name 'CreditEntry' could not be found (are you missing a using directive or an assembly reference?)
        long ManageTransaction(CreditEntry model);
#pragma warning restore CS0246 // The type or namespace name 'CreditEntry' could not be found (are you missing a using directive or an assembly reference?)

        int ApplicationMarkAsViewed(long id);

        //List<ContactEntity> PaidSearch(int page, long userId, string gender);

#pragma warning disable CS0246 // The type or namespace name 'MessageFilter' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'CommunicationEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<CommunicationEntity>> ReceiverList(MessageFilter filter);
#pragma warning restore CS0246 // The type or namespace name 'CommunicationEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'MessageFilter' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'MessageFilter' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'CommunicationEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<CommunicationEntity>> Messages(MessageFilter filter);
#pragma warning restore CS0246 // The type or namespace name 'CommunicationEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'MessageFilter' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'ViewedEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<ViewedEntity>> ProfileViewedList(long userId, int page);
#pragma warning restore CS0246 // The type or namespace name 'ViewedEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'ProfileViewEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<ProfileViewEntity>> ProfileViewedDetail(long userId, long profileId, int page);
#pragma warning restore CS0246 // The type or namespace name 'ProfileViewEntity' could not be found (are you missing a using directive or an assembly reference?)
        
#pragma warning disable CS0246 // The type or namespace name 'Blocked' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<Blocked>> BlockedProfileList(long userId, int page);
#pragma warning restore CS0246 // The type or namespace name 'Blocked' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'JobBookmarked' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<JobBookmarked>> JobsBookmarkedCompanyList(long userId);
#pragma warning restore CS0246 // The type or namespace name 'JobBookmarked' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'JobBookmarked' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'JobBookmarkFilter' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<JobBookmarked>> JobBookmarkedList(JobBookmarkFilter model);
#pragma warning restore CS0246 // The type or namespace name 'JobBookmarkFilter' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'JobBookmarked' could not be found (are you missing a using directive or an assembly reference?)

#pragma warning disable CS0246 // The type or namespace name 'ApplicationEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<ApplicationEntity>> ApplictionCompanyList(long userId);
#pragma warning restore CS0246 // The type or namespace name 'ApplicationEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'ApplicationEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<ApplicationEntity>> ApplictionJobTitleList(long userId);
#pragma warning restore CS0246 // The type or namespace name 'ApplicationEntity' could not be found (are you missing a using directive or an assembly reference?)

#pragma warning disable CS0246 // The type or namespace name 'InterviewFilter' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'InterviewEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<InterviewEntity>> Interviews(InterviewFilter filter);
#pragma warning restore CS0246 // The type or namespace name 'InterviewEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'InterviewFilter' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'InterviewEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'InterviewFilter' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<InterviewEntity>> InterviewJobList(InterviewFilter filter);
#pragma warning restore CS0246 // The type or namespace name 'InterviewFilter' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'InterviewEntity' could not be found (are you missing a using directive or an assembly reference?)
    }
}
