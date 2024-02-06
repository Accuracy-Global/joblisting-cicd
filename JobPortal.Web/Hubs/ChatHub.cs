using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using JobPortal.Web.Models;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Threading.Tasks;
using System.Web.Security;


namespace JobPortal.Web.Hubs
{
    public class ChatHub : Hub
    {
        //static List<UserEntity> ConnectedUsers = new List<UserEntity>();
        //static List<MessageDetail> CurrentMessage = new List<MessageDetail>();

        //public void Connect(string Username)
        //{
        //    if (!string.IsNullOrEmpty(Username))
        //    {
        //        UserEntity model = ConnectedUsers.Where(x => x.ConnectionId == Context.ConnectionId || x.Username == Username).SingleOrDefault();
        //        if (model == null)
        //        {
        //            UserProfile profile = DataHelper.GetList<UserProfile>().Where(x => x.Username == Username).SingleOrDefault();
        //            model = new UserEntity()
        //            {
        //                ConnectionId = Context.ConnectionId,
        //                UserId = profile.UserId,
        //                Username = Username,
        //                FullName = string.Format("{0} {1}", profile.FirstName, profile.LastName),
        //                Photo = profile.Logo,
        //                Status = (OnlineStatus.Online).ToString().TitleCase()
        //            };
        //            ConnectedUsers.Add(model);
        //            Clients.Caller.onConnected(model.ConnectionId, model.FullName, ConnectedUsers, CurrentMessage);
        //            // send to all except caller client
        //            Clients.AllExcept(model.ConnectionId).onNewUserConnected(model);
        //        }
        //    }

        //    //    if (!string.IsNullOrWhiteSpace(Username))
        //    //    {
        //    //        //  HubHelper.RegisterClient(userName, Context.ConnectionId);
        //    //        // SendPendingMessages(userName);
        //    //    }


        //    //}

        //}

        public void SendPendingMessages(string userName)
        {
            // Get pending messages for user and send it
            //var messages = ChatService.Instance.GetPendingChatMessages(userName);
            //if (messages != null)
            //{
            //    Clients.User(userName).processPendingMessages(messages);
            //}
        }

        public void SendMessageToAll(string userName, string message)
        {
            // store last 100 messages in cache
            AddMessageinCache(userName, message);

            // Broad cast message
            Clients.All.messageReceived(userName, message);
            ChatService.Instance.InsertMessageDetail(userName, "All", message, "Message", "");
        }

        public void Send(string from, string to, string message)
        {
            //if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            //{
            //    long fromId = Convert.ToInt64(from);
            //    long toId = Convert.ToInt64(to);
            //   // UserEntity fromUser = ConnectedUsers.Where(x => x.UserId == fromId).SingleOrDefault();
            //    //UserEntity toUser = ConnectedUsers.Where(x => x.UserId == toId).SingleOrDefault();

            //    // Recipient
            //    Clients.Client(toUser.ConnectionId).send(fromUser, string.Format("<div class=\"chat-message\" style=\"padding:5px; border-radius:4px;margin-bottom:5px; text-align:left;\">{0}</div>", message));

            //    // Caller
            //    Clients.Caller.send(toUser, string.Format("<div class=\"chat-message\" style=\"padding:5px; border-radius:4px;margin-bottom:5px; text-align:right;\">{0}</div>", message));
            //    using (JobPortalEntities context = new JobPortalEntities())
            //    {
            //        DataHelper dataHelper = new DataHelper(context);
            //        OnlineUser onlineUser = dataHelper.GetSingle<OnlineUser>("Id",toUser.UserId);

            //        if (onlineUser != null)
            //        {
            //            Message entity = new Message()
            //            {
            //                Content = message,
            //                SenderId = fromUser.UserId,
            //                RecipientId = toUser.UserId,
            //                StartTimeStamp = DateTime.Now,
            //                TimeStamp = DateTime.Now,
            //                Sent = (onlineUser.Status == (int)OnlineStatus.Online)
            //            };

            //            long id = Convert.ToInt64(dataHelper.Add<Message>(entity));
            //        }
            //    }
            //}
        }


        /// <summary>
        /// The OnDisconnected event.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override Task OnDisconnected(bool flag)
        {
            //if (Context != null)
            //{
            //    UserEntity profile = ConnectedUsers.Where(x => x.Username == Context.User.Identity.Name).SingleOrDefault();

            //    if (profile != null && ConnectedUsers.Contains(profile))
            //    {
            //        ConnectedUsers.Remove(profile);
            //        Clients.AllExcept(profile.ConnectionId).disconnected(profile.UserId, profile.Username);
                    
            //        //UserProfile loggedInUser = MemberService.Instance.Get(Context.User.Identity.Name);
            //        //List<Connection> contacts = loggedInUser.Connections.ToList();
            //        //List<UserEntity> onlineUsers = new List<UserEntity>();
            //        //foreach (Connection contact in contacts)
            //        //{
            //        //    OnlineUser onlineUser = DataHelper.GetList<OnlineUser>().Where(x => x.UserProfile.Username.Equals(contact.EmailAddress)).SingleOrDefault();
            //        //    if (onlineUser != null)
            //        //    {
            //        //        if (!onlineUser.UserProfile.Username.Equals(Context.User.Identity.Name))
            //        //        {
            //        //            UserEntity connected = ConnectedUsers.SingleOrDefault(x => x.UserId == onlineUser.UserProfile.UserId);

            //        //            UserEntity entity = new UserEntity()
            //        //            {
            //        //                ConnectionId = (connected != null) ? connected.ConnectionId : null,
            //        //                UserId = onlineUser.UserProfile.UserId,
            //        //                Username = onlineUser.UserProfile.Username,
            //        //                FullName = string.Format("{0} {1}", onlineUser.UserProfile.FirstName, onlineUser.UserProfile.LastName),
            //        //                GroupId = contact.GroupId,
            //        //                Photo = onlineUser.UserProfile.Logo,
            //        //                Status = ((OnlineStatus)onlineUser.Status).ToString().TitleCase(),
            //        //            };
            //        //            onlineUsers.Add(entity);
            //        //        }
            //        //    }
            //        //}

            //        ////Clients.Caller.listOnlineUsers(onlineUsers);

            //        //Clients.Caller.onConnected(profile.UserId, profile.FullName, onlineUsers, CurrentMessage);

            //        //Clients.Caller.onConnected(model.UserId, model.FullName, ConnectedUsers, CurrentMessage);
            //        // send to all except caller client
            //        //Clients.AllExcept(model.ConnectionId).onNewUserConnected(model);
            //    }
            //}

            // Send the current count of users
            //Send(Users, profile);
            return base.OnDisconnected(flag);
        }

        /// <summary>
        /// The OnConnected event.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override Task OnConnected()
        {
            //if (Context != null)
            //{
            //    if (Context.User != null)
            //    {
            //        UserProfile profile = MemberService.Instance.Get(Context.User.Identity.Name);
            //        UserEntity model = ConnectedUsers.Where(x => x.ConnectionId == Context.ConnectionId || x.Username == Context.User.Identity.Name).SingleOrDefault();
            //        if (model == null && profile != null)
            //        {
            //            model = new UserEntity()
            //            {
            //                ConnectionId = Context.ConnectionId,
            //                UserId = profile.UserId,
            //                Username = profile.Username,
            //                FullName = string.Format("{0} {1}", profile.FirstName, profile.LastName),
            //                Photo = profile.Logo,
            //                Status = (OnlineStatus.Online).ToString().TitleCase()
            //            };

            //            ConnectedUsers.Add(model);

            //            //List<Connection> contacts = profile.Connections.ToList();
            //            //List<UserEntity> onlineUsers = new List<UserEntity>();
            //            //foreach (Connection contact in contacts)
            //            //{
            //            //    OnlineUser onlineUser = DataHelper.GetList<OnlineUser>().Where(x => x.UserProfile.Username.Equals(contact.EmailAddress)).SingleOrDefault();
            //            //    if (onlineUser != null)
            //            //    {
            //            //        if (!onlineUser.UserProfile.Username.Equals(Context.User.Identity.Name))
            //            //        {
            //            //            UserEntity connected = ConnectedUsers.SingleOrDefault(x => x.UserId == onlineUser.UserProfile.UserId);

            //            //            UserEntity entity = new UserEntity()
            //            //            {
            //            //                ConnectionId = (connected != null) ? connected.ConnectionId : null,
            //            //                UserId = onlineUser.UserProfile.UserId,
            //            //                Username = onlineUser.UserProfile.Username,
            //            //                FullName = string.Format("{0} {1}", onlineUser.UserProfile.FirstName, onlineUser.UserProfile.LastName),
            //            //                GroupId = contact.GroupId,
            //            //                Photo = onlineUser.UserProfile.Logo,
            //            //                Status = ((OnlineStatus)onlineUser.Status).ToString().TitleCase(),
            //            //            };
            //            //            onlineUsers.Add(entity);
            //            //        }
            //            //    }
            //            //}

            //            // Currently loggedin user
            //            Clients.Caller.connected(model.UserId, ConnectedUsers, CurrentMessage);

            //            // All users except currently logged in user
            //            Clients.AllExcept(model.ConnectionId).connected(model.UserId, ConnectedUsers, CurrentMessage);
            //        }
            //    }
            //}

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            //if (Context != null)
            //{
            //    UserProfile profile = MemberService.Instance.Get(Context.User.Identity.Name);
            //    if (profile != null)
            //    {
            //        UserEntity model = ConnectedUsers.Where(x => x.ConnectionId == Context.ConnectionId || x.Username == Context.User.Identity.Name).SingleOrDefault();
            //        if (model == null)
            //        {
            //            model = new UserEntity()
            //            {
            //                ConnectionId = Context.ConnectionId,
            //                UserId = profile.UserId,
            //                Username = profile.Username,
            //                FullName = string.Format("{0} {1}", profile.FirstName, profile.LastName),
            //                Photo = profile.Logo,
            //                Status = (OnlineStatus.Online).ToString().TitleCase()
            //            };
            //            ConnectedUsers.Add(model);
            //            Clients.Caller.onConnected(model.ConnectionId, model.FullName, ConnectedUsers, CurrentMessage);
            //            // send to all except caller client
            //            Clients.AllExcept(model.ConnectionId).onNewUserConnected(model);

            //            List<Connection> contacts = profile.Connections.ToList();
            //            List<UserEntity> onlineUsers = new List<UserEntity>();
            //            foreach (Connection contact in contacts)
            //            {
            //                OnlineUser onlineUser = DataHelper.GetList<OnlineUser>().Where(x => x.UserProfile.Username.Equals(contact.EmailAddress)).SingleOrDefault();
            //                if (onlineUser != null)
            //                {
            //                    if (ConnectedUsers.Count(x => x.Username.Equals(onlineUser.UserProfile.Username)) > 0)
            //                    {
            //                        UserEntity entity = new UserEntity()
            //                        {
            //                            UserId = onlineUser.UserProfile.UserId,
            //                            Username = onlineUser.UserProfile.Username,
            //                            FullName = string.Format("{0} {1}", onlineUser.UserProfile.FirstName, onlineUser.UserProfile.LastName),
            //                            GroupId = contact.GroupId,
            //                            Photo = onlineUser.UserProfile.Logo,
            //                            Status = ((OnlineStatus)onlineUser.Status).ToString().TitleCase(),

            //                        };
            //                        onlineUsers.Add(entity);
            //                    }
            //                }
            //            }

            //            Clients.Caller.listOnlineUsers(onlineUsers);
            //        }
            //    }
            //}
            return base.OnReconnected();
        }

        private void AddMessageinCache(string userName, string message)
        {
            //CurrentMessage.Add(new MessageDetail { UserName = userName, Message = message });

            //if (CurrentMessage.Count > 100)
            //    CurrentMessage.RemoveAt(0);
        }
    }
}