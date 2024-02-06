#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Helpers;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Data.Common;
using System.Data.SqlClient;
namespace JobPortal.Domain
{
#pragma warning disable CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    public class InterviewService : DataService
#pragma warning restore CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    {
        private static volatile InterviewService instance;
        private static readonly object sync = new object();

        /// <summary>
        /// Default private constructor.
        /// </summary>
        private InterviewService()
        {
        }

        /// <summary>
        /// Single Instance of InterviewService
        /// </summary>
        public static InterviewService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new InterviewService();
                        }
                    }
                }
                return instance;
            }
        }

#pragma warning disable CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        public Interview Get(long Id)
#pragma warning restore CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        {
            Interview entity = new Interview();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                entity = dataHelper.GetSingle<Interview>(Id);
            }
            return entity;
        }

#pragma warning disable CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        public Interview Get(Guid Id)
#pragma warning restore CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        {
            Interview entity = new Interview();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                entity = dataHelper.Get<Interview>().Where(x => x.TrackingId == Id).OrderByDescending(x=>x.DateUpdated).FirstOrDefault();
            }
            return entity;
        }

#pragma warning disable CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        public Interview Get(long Id, int round)
#pragma warning restore CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        {
            Interview entity = new Interview();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                entity = dataHelper.Get<Interview>().Where(x=>x.Id == Id && x.Round == round).SingleOrDefault();
            }
            return entity;
        }

#pragma warning disable CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        public Interview Get(Guid Id, int round)
#pragma warning restore CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        {
            Interview entity = new Interview();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                entity = dataHelper.Get<Interview>().Where(x => x.TrackingId == Id && x.Round == round).SingleOrDefault();
            }
            return entity;
        }

        ///// <summary>
        ///// Gets the Inprogress Interview
        ///// </summary>
        ///// <param name="jobseekerId"></param>
        ///// <param name="employerId"></param>
        ///// <returns></returns>
        //public Interview Get(long jobseekerId, long employerId)
        //{
        //    Interview interview = new Interview();
        //    int initiated = (int)InterviewStatus.INITIATED;
        //    int inprogress = (int)InterviewStatus.INTERVIEW_IN_PROGRESS;
           
        //    using (JobPortalEntities context = new JobPortalEntities())
        //    {
        //        DataHelper dataHelper = new DataHelper(context);
        //        interview = dataHelper.Get<Interview>().Where(x => (x.Status == initiated || x.Status == inprogress) && x.UserId == employerId && x.Tracking.JobseekerId == jobseekerId && x.Tracking.Job == null).OrderByDescending(x => x.Id).FirstOrDefault();
        //    }
        //    return interview;
        //}
        /// <summary>
        /// Gets interview followup list.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'FollowUp' could not be found (are you missing a using directive or an assembly reference?)
        public List<FollowUp> GetFolloupList(long Id)
#pragma warning restore CS0246 // The type or namespace name 'FollowUp' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<FollowUp> followupList = new List<FollowUp>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Interview entity = dataHelper.Get<Interview>().SingleOrDefault(x=>x.Id ==Id);
                followupList = entity.FollowUps.ToList();
            }
            return followupList;
        }

#pragma warning disable CS0246 // The type or namespace name 'FollowUp' could not be found (are you missing a using directive or an assembly reference?)
        public FollowUp GetFollowUpEntry(long Id)
#pragma warning restore CS0246 // The type or namespace name 'FollowUp' could not be found (are you missing a using directive or an assembly reference?)
        {
            FollowUp followup = new FollowUp();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                followup = dataHelper.Get<FollowUp>().SingleOrDefault(x=>x.Id == Id);
            }
            return followup;
        }

        /// <summary>
        /// Gets interview latest followup entry
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'FollowUp' could not be found (are you missing a using directive or an assembly reference?)
        public FollowUp GetFollowUp(long Id)
#pragma warning restore CS0246 // The type or namespace name 'FollowUp' could not be found (are you missing a using directive or an assembly reference?)
        {
            FollowUp followup = new FollowUp();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Interview entity = dataHelper.Get<Interview>().SingleOrDefault(x => x.Id == Id);
                followup = entity.FollowUps.OrderByDescending(x=>x.Id).FirstOrDefault();
            }
            return followup;
        }

        /// <summary>
        /// Gets interview initial followup entry
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'FollowUp' could not be found (are you missing a using directive or an assembly reference?)
        public FollowUp GetInitialFollowUp(long Id)
#pragma warning restore CS0246 // The type or namespace name 'FollowUp' could not be found (are you missing a using directive or an assembly reference?)
        {
            FollowUp followup = new FollowUp();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Interview entity = dataHelper.Get<Interview>().SingleOrDefault(x => x.Id == Id);
                followup = entity.FollowUps.OrderBy(x => x.DateUpdated).FirstOrDefault();
            }
            return followup;
        }

#pragma warning disable CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        public Interview Initiate(Interview interview, string Username)
#pragma warning restore CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                dataHelper.Add<Interview>(interview, Username);
            }
            return interview;
        }

        /// <summary>
        /// Updates the details
        /// </summary>
        /// <param name="round"></param>
        /// <param name="Username"></param>
        /// <returns>Updated Inverview Details</returns>
#pragma warning disable CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        public Interview Update(Interview entity, string Username)
#pragma warning restore CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                dataHelper.Update<Interview>(entity, Username);
            }
            return entity;
        }

        /// <summary>
        /// Gets list of discussion
        /// </summary>
        /// <param name="interviewId">Interview Id</param>
        /// <returns>List of discussion</returns>
#pragma warning disable CS0246 // The type or namespace name 'InterviewDiscussion' could not be found (are you missing a using directive or an assembly reference?)
        public List<InterviewDiscussion> GetDiscussions(long interviewId)
#pragma warning restore CS0246 // The type or namespace name 'InterviewDiscussion' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<InterviewDiscussion> list = new List<InterviewDiscussion>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                list = dataHelper.Get<InterviewDiscussion>().Where(x => x.InterviewId == interviewId).ToList();
            }
            return list;
        }

        /// <summary>
        /// Gets list of notes
        /// </summary>
        /// <param name="interviewId">Interview Id</param>
        /// <returns>List of notes</returns>
#pragma warning disable CS0246 // The type or namespace name 'InterviewNote' could not be found (are you missing a using directive or an assembly reference?)
        public List<InterviewNote> GetNotes(long interviewId)
#pragma warning restore CS0246 // The type or namespace name 'InterviewNote' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<InterviewNote> list = new List<InterviewNote>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                list = dataHelper.Get<InterviewNote>().Where(x => x.InterviewId == interviewId).ToList();
            }
            return list;
        }

        /// <summary>
        /// Gets Single discussion
        /// </summary>
        /// <param name="roundId">Round Id</param>
        /// <param name="Id">Discussion Id</param>
        /// <returns>Single discussion entity</returns>
#pragma warning disable CS0246 // The type or namespace name 'InterviewDiscussion' could not be found (are you missing a using directive or an assembly reference?)
        public InterviewDiscussion GetDiscussion(long interviewId, long Id)
#pragma warning restore CS0246 // The type or namespace name 'InterviewDiscussion' could not be found (are you missing a using directive or an assembly reference?)
        {
            InterviewDiscussion entity = new InterviewDiscussion();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                entity = dataHelper.Get<InterviewDiscussion>().SingleOrDefault(x => x.Id == Id && x.InterviewId == interviewId);
            }
            return entity;
        }

        /// <summary>
        /// Gets single discussion
        /// </summary>
        /// <param name="discussionId">Discussion Id</param>
        /// <returns>Single discussion entity</returns>
#pragma warning disable CS0246 // The type or namespace name 'InterviewDiscussion' could not be found (are you missing a using directive or an assembly reference?)
        public InterviewDiscussion GetDiscussion(long discussionId)
#pragma warning restore CS0246 // The type or namespace name 'InterviewDiscussion' could not be found (are you missing a using directive or an assembly reference?)
        {
            InterviewDiscussion entity = new InterviewDiscussion();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                entity = dataHelper.Get<InterviewDiscussion>().SingleOrDefault(x => x.Id == discussionId);
            }
            return entity;
        }

        /// <summary>
        /// Gets Single note based on RoundId, NoteId
        /// </summary>
        /// <param name="roundId">Round Id</param>
        /// <param name="Id">Note Id</param>
        /// <returns>Single Note entity</returns>
#pragma warning disable CS0246 // The type or namespace name 'InterviewNote' could not be found (are you missing a using directive or an assembly reference?)
        public InterviewNote GetNote(long interviewId, long Id)
#pragma warning restore CS0246 // The type or namespace name 'InterviewNote' could not be found (are you missing a using directive or an assembly reference?)
        {
            InterviewNote entity = new InterviewNote();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                entity = dataHelper.Get<InterviewNote>().SingleOrDefault(x => x.Id == Id && x.InterviewId == interviewId);
            }
            return entity;
        }

        /// <summary>
        /// Gets single note
        /// </summary>
        /// <param name="noteId">Note Id</param>
        /// <returns>Single Note entity</returns>
#pragma warning disable CS0246 // The type or namespace name 'InterviewNote' could not be found (are you missing a using directive or an assembly reference?)
        public InterviewNote GetNote(long noteId)
#pragma warning restore CS0246 // The type or namespace name 'InterviewNote' could not be found (are you missing a using directive or an assembly reference?)
        {
            InterviewNote entity = new InterviewNote();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                entity = dataHelper.Get<InterviewNote>().SingleOrDefault(x => x.Id == noteId);
            }
            return entity;
        }

#pragma warning disable CS0246 // The type or namespace name 'FollowUp' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'FollowUp' could not be found (are you missing a using directive or an assembly reference?)
        public FollowUp FollowUpEntry(FollowUp followup)
#pragma warning restore CS0246 // The type or namespace name 'FollowUp' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'FollowUp' could not be found (are you missing a using directive or an assembly reference?)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                dataHelper.Add<FollowUp>(followup);
            }
            return followup;
        }

        public void Connect(string email, string Username)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                UserProfile friend = dataHelper.GetSingle<UserProfile>("Username", email);
                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", Username);

                Connection contact = dataHelper.Get<Connection>().Where(x => x.UserId == profile.UserId && x.EmailAddress == email).SingleOrDefault();
                if (contact == null)
                {
                    contact = new Connection()
                    {
                        UserId = profile.UserId,
                        FirstName = friend != null ? friend.FirstName.TitleCase() : "",
                        LastName = friend != null ? friend.LastName.TitleCase() : "",
                        EmailAddress = email,
                        IsAccepted = false,
                        IsConnected = false,
                        IsBlocked = false,
                        Initiated = true
                    };

                    dataHelper.AddEntity<Connection>(contact, Username);

                    if (friend != null)
                    {
                        Connection connection = dataHelper.Get<Connection>().Where(x => x.UserId == friend.UserId && x.EmailAddress == profile.Username).SingleOrDefault();
                        if (connection == null)
                        {
                            connection = new Connection()
                            {
                                UserId = friend.UserId,
                                FirstName = profile.FirstName.TitleCase(),
                                LastName = profile.LastName.TitleCase(),
                                EmailAddress = profile.Username,
                                IsAccepted = false,
                                IsConnected = false,
                                IsBlocked = false,
                                Initiated = true
                            };

                            dataHelper.AddEntity<Connection>(connection, Username);
                        }
                    }
                }
                else
                {
                    if (contact.IsDeleted == false && contact.Initiated == true && contact.IsAccepted==false && contact.IsConnected==false)
                    {
                        contact.IsAccepted = true;
                        contact.IsConnected = true;
                        contact.IsDeleted = false;
                        contact.IsBlocked = false;
                        contact.Initiated = false;
                        contact.DateUpdated = DateTime.Now;
                        contact.UpdatedBy = Username;
                        contact.CreatedBy = Username;

                        dataHelper.UpdateEntity<Connection>(contact);

                        Connection connection = dataHelper.Get<Connection>().Where(x => x.UserId == friend.UserId && x.EmailAddress == profile.Username).SingleOrDefault();
                        if (connection != null)
                        {
                            connection.IsAccepted = true;
                            connection.IsConnected = true;
                            connection.IsDeleted = false;
                            connection.IsBlocked = false;
                            connection.Initiated = false;
                            connection.DateUpdated = DateTime.Now;
                            connection.UpdatedBy = Username;
                            connection.CreatedBy = Username;
                            dataHelper.UpdateEntity<Connection>(connection);
                        }
                    }
                    else if (contact.IsDeleted == true && contact.Initiated == false && contact.IsAccepted == false && contact.IsConnected == false)
                    {
                        contact.IsAccepted = false;
                        contact.IsConnected = false;
                        contact.Initiated = true;
                        contact.IsDeleted = false;
                        contact.IsBlocked = false;
                        contact.DateUpdated = DateTime.Now;
                        contact.UpdatedBy = Username;
                        contact.CreatedBy = Username;

                        dataHelper.UpdateEntity<Connection>(contact);

                        Connection connection = dataHelper.Get<Connection>().Where(x => x.UserId == friend.UserId && x.EmailAddress == profile.Username).SingleOrDefault();
                        if (connection != null)
                        {
                            connection.IsAccepted = false;
                            connection.IsConnected = false;
                            connection.Initiated = true;
                            connection.IsDeleted = false;
                            connection.IsBlocked = false;
                            connection.DateUpdated = DateTime.Now;
                            connection.UpdatedBy = Username;
                            connection.CreatedBy = Username;
                            dataHelper.UpdateEntity<Connection>(connection);
                        }
                    }
                }
                dataHelper.Save();
            }
        }

        public int Counts(string Username)
        {
            int count = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                UserProfile profile = MemberService.Instance.Get(Username);

                if (profile.Type == (int)SecurityRoles.Jobseeker)
                {
                    count = dataHelper.Get<Interview>().Count(x => x.Tracking.JobseekerId == profile.UserId && x.IsDeleted == false);
                }
                else if (profile.Type == (int)SecurityRoles.Employers)
                {
                    count = dataHelper.Get<Interview>().Count(x => x.UserId == profile.UserId && x.IsDeleted == false);
                }
            }
            return count;
        }
        public int Counts(string Username, long InterviewId)
        {
            int count = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                UserProfile profile = MemberService.Instance.Get(Username);

                //int accepted = (int)FeedbackStatus.ACCEPTED;
                int followupCounts = dataHelper.Get<FollowUp>().Count(x => x.InterviewId == InterviewId && x.Unread == true && x.UserId != profile.UserId);
                int discussionCounts = dataHelper.Get<InterviewDiscussion>().Count(x => x.InterviewId == InterviewId && x.Unread == true && x.UserId != profile.UserId);
                count  = followupCounts + discussionCounts;
            }
            return count;
        }

#pragma warning disable CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        public Interview Get(long jobseekerId, long employerId)
#pragma warning restore CS0246 // The type or namespace name 'Interview' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", jobseekerId));
            parameters.Add(new SqlParameter("@Requestor", employerId));

            return ReadSingleData<Interview>("GetInterview", parameters);
        }

        public bool Initiated(long jobseekerId, long employerId)
        {
            bool flag = false;
            Interview entity = Get(jobseekerId, employerId);
            if (entity.Id != 0)
            {
                flag = (entity.Status == (int)InterviewStatus.INITIATED);
            }
            return flag;
        }

        public bool InProgress(long jobseekerId, long employerId)
        {
            bool flag = false;
            Interview entity = Get(jobseekerId, employerId);
            if (entity.Id != 0)
            {
                flag = (entity.Status == (int)InterviewStatus.INTERVIEW_IN_PROGRESS);
            }
            return flag;
        }

        public bool IsWidthdrawn(long jobseekerId, long employerId)
        {
            bool flag = false;
            Interview entity = Get(jobseekerId, employerId);
            if (entity.Id != 0)
            {
                flag = (entity.Status == (int)InterviewStatus.WITHDRAW);
            }
            return flag;
        }

        public bool IsRejected(long jobseekerId, long employerId)
        {
            bool flag = false;
            Interview entity = Get(jobseekerId, employerId);
            if (entity.Id != 0)
            {
                flag = (entity.Status == (int)InterviewStatus.REJECTED);
            }
            return flag;
        }

        public bool IsSelected(long jobseekerId, long employerId)
        {
            bool flag = false;
            Interview entity = Get(jobseekerId, employerId);
            if (entity.Id != 0)
            {
                flag = (entity.Status == (int)InterviewStatus.SELECTED);
            }
            return flag;
        }

        public bool IsCompleted(long jobseekerId, long employerId)
        {
            bool flag = false;
            Interview entity = Get(jobseekerId, employerId);
            if (entity.Id != 0)
            {
                flag = (entity.Status == (int)InterviewStatus.COMPLETED);
            }
            return flag;
        }


        public bool Initiated(long Id)
        {
            bool flag = false;
            Interview entity = Get(Id);
            if (entity.Id != 0)
            {
                flag = (entity.Status == (int)InterviewStatus.INITIATED);
            }
            return flag;
        }

        public bool InProgress(long Id)
        {
            bool flag = false;
            Interview entity = Get(Id);
            if (entity.Id != 0)
            {
                flag = (entity.Status == (int)InterviewStatus.INTERVIEW_IN_PROGRESS);
            }
            return flag;
        }

        public bool IsWidthdrawn(long Id)
        {
            bool flag = false;
            Interview entity = Get(Id);
            if (entity.Id != 0)
            {
                flag = (entity.Status == (int)InterviewStatus.WITHDRAW);
            }
            return flag;
        }

        public bool IsRejected(long Id)
        {
            bool flag = false;
            Interview entity = Get(Id);
            if (entity.Id != 0)
            {
                flag = (entity.Status == (int)InterviewStatus.REJECTED);
            }
            return flag;
        }

        public bool IsSelected(long Id)
        {
            bool flag = false;
            Interview entity = Get(Id);
            if (entity.Id != 0)
            {
                flag = (entity.Status == (int)InterviewStatus.SELECTED);
            }
            return flag;
        }

        public bool IsCompleted(long Id)
        {
            bool flag = false;
            Interview entity = Get(Id);
            if (entity.Id != 0)
            {
                flag = (entity.Status == (int)InterviewStatus.COMPLETED);
            }
            return flag;
        }
    }
}
