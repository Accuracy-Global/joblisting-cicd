#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc.Html;
using System.Web.WebPages.Html;

namespace JobPortal.Library.Helpers
{
    public static class ConnectionHelper
    {        
        /// <summary>
        /// Get connection counts
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>Number of Connections</returns>
        public static int Counts(long userid)
        {
            int rows = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                UserProfile profile = dataHelper.Get<UserProfile>().SingleOrDefault(x => x.UserId == userid);
                rows = dataHelper.Get<Connection>().Count(x => x.UserId == userid && x.IsDeleted == false && !x.EmailAddress.Equals(profile.Username) && x.IsAccepted == false && x.IsConnected == false && !x.CreatedBy.Equals(profile.Username)  && x.Initiated == true);
            }
            return rows;
        }

#pragma warning disable CS0246 // The type or namespace name 'Connection' could not be found (are you missing a using directive or an assembly reference?)
        public static List<Connection> GetList(long userId)
#pragma warning restore CS0246 // The type or namespace name 'Connection' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Connection> list = new List<Connection>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                list = dataHelper.Get<Connection>().Where(x => x.IsDeleted == false).ToList();
            }
            return list;
        }
#pragma warning disable CS0246 // The type or namespace name 'Connection' could not be found (are you missing a using directive or an assembly reference?)
        public static Connection Get(long Id)
#pragma warning restore CS0246 // The type or namespace name 'Connection' could not be found (are you missing a using directive or an assembly reference?)
        {
            Connection connection = new Connection();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                connection = dataHelper.GetSingle<Connection>(Id);
            }
            return connection;
        }
                                                                                                                                                                                                                                     

#pragma warning disable CS0246 // The type or namespace name 'Connection' could not be found (are you missing a using directive or an assembly reference?)
        public static Connection Get(string contactEmail, string loggedInUserEmail)
#pragma warning restore CS0246 // The type or namespace name 'Connection' could not be found (are you missing a using directive or an assembly reference?)
        {
            Connection connection = new Connection();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", contactEmail);
                UserProfile loggedInUser = dataHelper.GetSingle<UserProfile>("Username", loggedInUserEmail);

                Hashtable parameters = new Hashtable();
                parameters.Add("UserId", loggedInUser.UserId);
                parameters.Add("EmailAddress", contactEmail);
                parameters.Add("IsDeleted", false);
                connection = dataHelper.GetSingle<Connection>(parameters);
            }
            return connection;
        }

#pragma warning disable CS0246 // The type or namespace name 'Connection' could not be found (are you missing a using directive or an assembly reference?)
        public static Connection GetEntity(string contactEmail, string loggedInUserEmail)
#pragma warning restore CS0246 // The type or namespace name 'Connection' could not be found (are you missing a using directive or an assembly reference?)
        {
            Connection connection = new Connection();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", contactEmail);
                UserProfile loggedInUser = dataHelper.GetSingle<UserProfile>("Username", loggedInUserEmail);

                Hashtable parameters = new Hashtable();
                parameters.Add("UserId", loggedInUser.UserId);
                parameters.Add("EmailAddress", contactEmail);
                connection = dataHelper.GetSingle<Connection>(parameters);
            }
            return connection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="requestor"></param>
        /// <returns></returns>
        public static bool Block(string email, long requestor)
        {
            bool flag = false;
            try
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    UserProfile profile = dataHelper.GetSingle<UserProfile>("UserId", requestor);
                    UserProfile blocked = dataHelper.GetSingle<UserProfile>("Username", email);

                    if (blocked != null)
                    {
                        Hashtable parameters = new Hashtable();
                        parameters.Add("UserId", profile.UserId);
                        parameters.Add("EmailAddress", email);
                        //parameters.Add("IsDeleted", false);
                        Connection connection = dataHelper.GetSingle<Connection>(parameters);

                        parameters = new Hashtable();
                        parameters.Add("UserId", blocked.UserId);
                        parameters.Add("EmailAddress", profile.Username);
                        //parameters.Add("IsDeleted", false);
                        Connection connected = dataHelper.GetSingle<Connection>(parameters);

                        if (connection != null && connected != null)
                        {
                            connection.IsAccepted = false;
                            connection.IsConnected = false;
                            connection.IsBlocked = true;
                            connection.IsDeleted = true;
                            connection.Initiated = false;
                            connection.DateDeleted = DateTime.Now;
                            connection.UpdatedBy = profile.Username;
                            connection.DateUpdated = DateTime.Now;

                            connected.IsAccepted = false;
                            connected.IsConnected = false;
                            connected.IsBlocked = true;
                            connected.IsDeleted = true;
                            connected.Initiated = false;
                            connected.DateDeleted = DateTime.Now;
                            connected.UpdatedBy = profile.Username;
                            connected.DateUpdated = DateTime.Now;

                            dataHelper.UpdateEntity<Connection>(connection);
                            dataHelper.UpdateEntity<Connection>(connected);

                            var result = dataHelper.Get<Communication>().Where(x => x.UserId == profile.UserId && (x.SenderId == blocked.UserId || x.ReceiverId == blocked.UserId));
                            foreach (var item in result)
                            {
                                item.IsDeleted = true;
                                item.DateUpdated = DateTime.Now;
                                item.UpdatedBy = profile.Username;
                                dataHelper.UpdateEntity<Communication>(item);
                            }
                            result = dataHelper.Get<Communication>().Where(x => x.UserId == blocked.UserId && (x.SenderId == profile.UserId || x.ReceiverId == profile.UserId));
                            foreach (var item in result)
                            {
                                item.IsDeleted = true;
                                item.DateUpdated = DateTime.Now;
                                item.UpdatedBy = profile.Username;
                                dataHelper.UpdateEntity<Communication>(item);
                            }
                            dataHelper.Save();
                            flag = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }


        public static bool Unblock(string email, long requestor)
        {
            bool flag = false;
            try
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    UserProfile profile = dataHelper.GetSingle<UserProfile>("UserId", requestor);
                    UserProfile blocked = dataHelper.GetSingle<UserProfile>("Username", email);

                    if (blocked != null)
                    {
                        Hashtable parameters = new Hashtable();
                        parameters.Add("UserId", profile.UserId);
                        parameters.Add("EmailAddress", email);
                        parameters.Add("IsDeleted", false);
                        Connection connection = dataHelper.GetSingle<Connection>(parameters);

                        parameters = new Hashtable();
                        parameters.Add("UserId", blocked.UserId);
                        parameters.Add("EmailAddress", profile.Username);
                        parameters.Add("IsDeleted", false);
                        Connection connected = dataHelper.GetSingle<Connection>(parameters);

                        if (connection != null && connected != null)
                        {
                            connection.IsBlocked = false;
                            dataHelper.Update<Connection>(connection, profile.Username);

                            connected.IsBlocked = false;
                            dataHelper.Update<Connection>(connected, profile.Username);

                            flag = true;
                        }
                    }

                    if (blocked != null && profile != null)
                    {
                        BlockedPeople model = dataHelper.Get<BlockedPeople>().SingleOrDefault(x => x.BlockedId == blocked.UserId && x.BlockerId == profile.UserId);
                        if (model != null)
                        {
                            flag = dataHelper.Remove<BlockedPeople>(model);
                        }
                    }
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="requestor"></param>
        /// <returns></returns>
        public static bool IsBlocked(string email, long requestor)
        {
            bool flag = false;

            Hashtable parameters = new Hashtable();
            parameters.Add("UserId", requestor);
            parameters.Add("EmailAddress", email);
            parameters.Add("IsBlocked", true);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Connection connection = dataHelper.GetSingle<Connection>(parameters);
                flag = (connection != null);
            }

            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="requestor"></param>
        /// <returns></returns>
        public static bool IsBlocked(string email, string requestor)
        {
            bool flag = false;

            Hashtable parameters = new Hashtable();
            UserProfile requestorProfile = new UserProfile();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                requestorProfile = dataHelper.GetSingle<UserProfile>("Username", requestor);

                parameters.Add("UserId", requestorProfile.UserId);
                parameters.Add("EmailAddress", email);
                parameters.Add("IsBlocked", true);

                Connection connection = dataHelper.GetSingle<Connection>(parameters);
                flag = (connection != null);
            }

            return flag;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="requestor"></param>
        /// <returns></returns>
        public static bool IsBlockedByMe(string email, long requestor)
        {
            bool flag = false;
            Hashtable parameters = new Hashtable();
            parameters.Add("UserId", requestor);

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                UserProfile requestorProfile = dataHelper.GetSingle<UserProfile>(parameters);
                if (requestorProfile != null)
                {
                    parameters = new Hashtable();
                    parameters.Add("UserId", requestor);
                    parameters.Add("EmailAddress", email);
                    parameters.Add("UpdatedBy", requestorProfile.Username);
                    parameters.Add("IsBlocked", true);

                    Connection connection = dataHelper.GetSingle<Connection>(parameters);
                    flag = (connection != null);
                }
            }
            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="requestor"></param>
        /// <returns></returns>
        public static bool IsAccepted(string email, long requestor)
        {
            bool flag = false;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                List<Parameter> parameters = new List<Parameter>();
                parameters.Add(new Parameter() { Name = "EmailAddress", Value = email, Comparison = ParameterComparisonTypes.Equals });
                parameters.Add(new Parameter() { Name = "UserId", Value = requestor, Comparison = ParameterComparisonTypes.Equals });
                parameters.Add(new Parameter() { Name = "IsAccepted", Value = true, Comparison = ParameterComparisonTypes.Equals });
                var accepted = dataHelper.GetSingle<Connection>(parameters);
                flag = (accepted != null);
            }
            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="requestor"></param>
        /// <returns></returns>
        public static bool IsConnected(string email, long requestor)
        {

            bool flag = false;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                List<Parameter> parameters = new List<Parameter>();
                parameters.Add(new Parameter() { Name = "EmailAddress", Value = email, Comparison = ParameterComparisonTypes.Equals });
                parameters.Add(new Parameter() { Name = "UserId", Value = requestor, Comparison = ParameterComparisonTypes.Equals });
                parameters.Add(new Parameter() { Name = "IsConnected", Value = true, Comparison = ParameterComparisonTypes.Equals });
                parameters.Add(new Parameter() { Name = "IsDeleted", Value = false, Comparison = ParameterComparisonTypes.Equals });
                var connected = dataHelper.GetSingle<Connection>(parameters);
                flag = (connected != null);
            }
            return flag;
        }

        /// <summary>
        /// Gets true or false depending on connection status
        /// </summary>
        /// <param name="email"></param>
        /// <param name="requestor"></param>
        /// <returns></returns>
        public static bool IsConnected(string email, string requestor)
        {

            bool flag = false;
            UserProfile requestorProfile = new UserProfile();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                requestorProfile = dataHelper.GetSingle<UserProfile>("Username", requestor);
                List<Parameter> parameters = new List<Parameter>();
                parameters.Add(new Parameter() { Name = "EmailAddress", Value = email, Comparison = ParameterComparisonTypes.Equals });
                parameters.Add(new Parameter() { Name = "UserId", Value = requestorProfile.UserId, Comparison = ParameterComparisonTypes.Equals });
                parameters.Add(new Parameter() { Name = "IsConnected", Value = true, Comparison = ParameterComparisonTypes.Equals });
                var connected = dataHelper.GetSingle<Connection>(parameters);
                flag = (connected != null);
            }
            return flag;
        }

        public static int MessageCounts(long userid)
        {
            int rows = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                rows = dataHelper.Get<Communication>().Count(x => x.ReceiverId == userid && x.Unread == true);
            }
            return rows;
        }

        public static int MessageCounts(string username)
        {
            int rows = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", username);
                if (profile != null)
                {
                    rows = dataHelper.Get<Communication>().Count(x => x.SenderId == profile.UserId && x.ReceiverId == profile.UserId);
                }
            }
            return rows;
        }

        public static int MessageCounts(long senderId, long receiverId)
        {
            int rows = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                rows = dataHelper.Get<Communication>().Count(x => x.SenderId == senderId && x.ReceiverId == receiverId && x.Unread == true);
            }
            return rows;
        }

        public static int GetMessageCounts(long senderId, long receiverId)
        {
            int rows = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                rows = dataHelper.Get<Communication>().Count(x => x.UserId == receiverId && (x.SenderId ==senderId || x.ReceiverId==senderId));
            }
            return rows;
        }

#pragma warning disable CS0246 // The type or namespace name 'Communication' could not be found (are you missing a using directive or an assembly reference?)
        public static Communication GetLatestMessage(long senderId, long receiverId)
#pragma warning restore CS0246 // The type or namespace name 'Communication' could not be found (are you missing a using directive or an assembly reference?)
        {
            Communication message = new Communication();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                long rid = 0;
                long sid = 0;

                if (dataHelper.Get<Communication>().Where(x => x.SenderId == senderId && x.ReceiverId == receiverId).Count() > 0)
                {
                    rid = dataHelper.Get<Communication>().Where(x => x.SenderId == senderId && x.ReceiverId == receiverId).Max(x => x.Id);
                }
                if (dataHelper.Get<Communication>().Where(x => x.SenderId == receiverId & x.ReceiverId == senderId).Count() > 0)
                {
                    sid = dataHelper.Get<Communication>().Where(x => x.SenderId == receiverId & x.ReceiverId == senderId).Max(x => x.Id);
                }

                if (rid > sid)
                {
                    message = dataHelper.GetSingle<Communication>(rid);
                }
                else
                {
                    message = dataHelper.GetSingle<Communication>(sid);
                }
            }
            return message;
        }

#pragma warning disable CS0246 // The type or namespace name 'BlockedPeople' could not be found (are you missing a using directive or an assembly reference?)
        public static BlockedPeople GetBlockedEntity(long BlockedId, long BlockerId)
#pragma warning restore CS0246 // The type or namespace name 'BlockedPeople' could not be found (are you missing a using directive or an assembly reference?)
        {
            BlockedPeople entity = new BlockedPeople();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                entity = dataHelper.Get<BlockedPeople>().SingleOrDefault(x => x.BlockedId == BlockedId && x.BlockerId == BlockerId);
                if (entity == null)
                {
                    entity = dataHelper.Get<BlockedPeople>().SingleOrDefault(x => x.BlockedId == BlockerId && x.BlockerId == BlockedId);
                }
            }
            return entity;
        }

        public static string Status(long senderId, long userId)
        {
            string status = string.Empty;
            UserProfile sender = null;
            UserProfile profile = null;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                sender = dataHelper.GetSingle<UserProfile>("UserId", senderId);
                profile = dataHelper.GetSingle<UserProfile>("UserId", userId);
            }

            bool connected = ConnectionHelper.IsConnected(sender.Username, profile.UserId);
            bool blockedByMe = ConnectionHelper.IsBlockedByMe(sender.Username, profile.UserId);
            bool blocked = ConnectionHelper.IsBlocked(sender.Username, profile.UserId);
            bool accepted = ConnectionHelper.IsAccepted(sender.Username, profile.UserId);

            if (connected)
            {
                if (blocked)
                {
                    status = "Blocked";
                }
                else
                {
                    status = "Connected";
                }
            }
            else
            {
                if (accepted == false)
                {
                    Connection connection = ConnectionHelper.Get(sender.Username, profile.Username);
                    if (connection != null && connection.IsDeleted == false)
                    {
                        if (!string.IsNullOrEmpty(connection.CreatedBy) && connection.CreatedBy.Equals(profile.Username))
                        {
                            status = "Waiting for Acceptance";
                        }
                        else
                        {
                            status = "Waiting for Acceptance";
                        }
                    }
                    else if (connection != null && connection.IsDeleted == false && connection.Initiated==true)
                    {
                        status = "Waiting for Acceptance";
                    }
                    else
                    {
                        status = "Not Connected";
                    }
                }
                else
                {
                    status = "Not Connected";
                }
            }

            return status;
        }

        public static string Status(string senderName, string username)
        {
            string status = string.Empty;
            UserProfile sender = null;
            UserProfile profile = null;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                sender = dataHelper.GetSingle<UserProfile>("Username", senderName);
                profile = dataHelper.GetSingle<UserProfile>("Username", username);
            }

            if (sender != null)
            {
                bool connected = ConnectionHelper.IsConnected(sender.Username, profile.UserId);
                bool blockedByMe = ConnectionHelper.IsBlockedByMe(sender.Username, profile.UserId);
                bool blocked = ConnectionHelper.IsBlocked(sender.Username, profile.UserId);
                bool accepted = ConnectionHelper.IsAccepted(sender.Username, profile.UserId);

                if (connected)
                {
                    if (blocked)
                    {
                        status = "Blocked";
                    }
                    else
                    {
                        status = "Connected";
                    }
                }
                else
                {
                    if (accepted == false)
                    {
                        Connection connection = ConnectionHelper.Get(sender.Username, profile.Username);
                        if (connection != null)
                        {
                            if (!string.IsNullOrEmpty(connection.CreatedBy) && connection.CreatedBy.Equals(profile.Username))
                            {
                                if (connection.IsDeleted == false && connection.Initiated == false && connection.IsAccepted == false && connection.IsBlocked == false && connection.IsConnected == false)
                                {
                                    status = "Contact Imported";
                                }
                                else
                                {
                                    status = "Waiting for Acceptance";
                                }
                            }
                            else
                            {
                                status = "Waiting for Acceptance";
                            }
                        }
                        else if (connection != null && connection.IsDeleted == false && connection.Initiated == true)
                        {
                            status = "Waiting for Acceptance";
                        }
                        else
                        {
                            status = "Not Connected";
                        }
                    }
                    else
                    {
                        status = "Not Connected";
                    }
                }
            }
            else
            {
                Connection connection = ConnectionHelper.Get(senderName, profile.Username);
                if (connection!=null && (connection.IsDeleted == false && connection.Initiated == false && connection.IsAccepted == false && connection.IsBlocked == false && connection.IsConnected == false))
                {
                    status = "Contact Imported";
                }
                else if (connection != null && connection.Initiated == true)
                {
                    status = "Waiting for Acceptance";
                }
                else
                {
                    status = "Not Connected";
                }

            }

            return status;
        }
    }
}