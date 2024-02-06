using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)

namespace JobPortal.Domain
{
#pragma warning disable CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    public class MessageService : DataService
#pragma warning restore CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    {
        private static volatile MessageService instance;
        private static readonly object sync = new object();

        /// <summary>
        /// Default private constructor.
        /// </summary>
        private MessageService()
        {
        }

        /// <summary>
        /// Single Instance of MessageService
        /// </summary>
        public static MessageService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new MessageService();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Gets the list of message list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'Communication' could not be found (are you missing a using directive or an assembly reference?)
        public List<Communication> GetMessageList(long userId)
#pragma warning restore CS0246 // The type or namespace name 'Communication' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Communication> list = new List<Communication>();
            UserProfile profile = MemberService.Instance.Get(userId);

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                list = dataHelper.Get<Communication>()
                    .Where(x => (x.SenderId == profile.UserId || x.ReceiverId== profile.UserId) && x.IsDeleted == false)
                    .OrderByDescending(x => x.DateUpdated).ToList();
            }
            return list;
        }

        /// <summary>
        /// Gets the Message by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'Communication' could not be found (are you missing a using directive or an assembly reference?)
        public Communication Get(long id)
#pragma warning restore CS0246 // The type or namespace name 'Communication' could not be found (are you missing a using directive or an assembly reference?)
        {
            Communication msg = new Communication();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                msg = dataHelper.Get<Communication>().Where(x => x.Id == id).SingleOrDefault();
            }
            return msg;
        }

        public void Send(string message, long fromUserId, long toUserId, string username, bool initial=false, bool reply=false)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Communication oMessage = new Communication()
                {
                    Message = message,
                    SenderId = fromUserId,
                    ReceiverId = toUserId,
                    IsSent = true,
                    Unread = false,
                    IsInitial = initial,
                    IsReply = reply,
                    UserId = fromUserId
                };

                Communication iMessage = new Communication()
                {
                    Message = message,
                    SenderId = fromUserId,
                    ReceiverId = toUserId,
                    IsSent = false,
                    Unread = true,
                    IsInitial = initial,
                    IsReply = reply,
                    UserId = toUserId
                };

                dataHelper.AddEntity<Communication>(oMessage, username);
                dataHelper.AddEntity<Communication>(iMessage, username);
                dataHelper.Save();
            }
        }

        /// <summary>
        /// Deletes the message and keeps in the database untill other person deletes the same.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        public void Delete(long id, string username)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Communication msg = dataHelper.GetSingle<Communication>(id);
                dataHelper.Delete<Communication>(msg, username);
            }
        }

        /// <summary>
        /// Removes message
        /// </summary>
        /// <param name="id">Message Id</param>
        /// <param name="username"> Username</param>
        public void Remove(long id)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Communication msg = dataHelper.GetSingle<Communication>(id);
                dataHelper.Remove<Communication>(msg);
            }
        }

        public int Count(long sender, long receiver)
        {
            return ReadDataField<int>(string.Format("SELECT COUNT(1) FROM Communications WHERE SenderId={0} AND ReceiverId={1} AND IsDeleted = 0", sender, receiver));
        }
    }
}
