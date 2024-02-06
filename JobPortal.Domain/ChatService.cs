using System.Collections.Generic;
using System.Data;
#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)

namespace JobPortal.Domain
{
    public class ChatService
    {
        private static volatile ChatService instance;
        private static readonly object sync = new object();
        //private static readonly string DELETE_FIELD_NAME = "IsDeleted";
        //private static readonly string ACTIVE_FIELD_NAME = "IsActive";

        private ChatService()
        {
        }

        /// <summary>
        ///     Single Instance of JobPortalService
        /// </summary>
        public static ChatService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new ChatService();
                        }
                    }
                }
                return instance;
            }
        }

        public DataSet ChatUserDetail(string UserName)
        {
            var dsChatUserDetail = new DataSet();
            //SqlConnection sqlcon = new SqlConnection(context.Database.Connection.ConnectionString);
            //SqlDataAdapter adpviewjobtrack = new SqlDataAdapter("Sp_ChatUserDetail", sqlcon);
            //adpviewjobtrack.SelectCommand.CommandType = CommandType.StoredProcedure;
            //adpviewjobtrack.SelectCommand.Parameters.AddWithValue("@UserName", UserName); 

            //adpviewjobtrack.Fill(dsChatUserDetail);
            //sqlcon.Close();

            return dsChatUserDetail;
        }

        public DataTable SearchChatUserDetail(string UserName, string FriendName, int ContactTyleId)
        {
            var dtChatUserDetail = new DataTable();
            //SqlConnection sqlcon = new SqlConnection(context.Database.Connection.ConnectionString);
            //SqlDataAdapter adpviewjobtrack = new SqlDataAdapter("Sp_ChatUserDetail_Search", sqlcon);
            //adpviewjobtrack.SelectCommand.CommandType = CommandType.StoredProcedure;
            //adpviewjobtrack.SelectCommand.Parameters.AddWithValue("@UserName", UserName);
            //adpviewjobtrack.SelectCommand.Parameters.AddWithValue("@FriendName", FriendName);
            //adpviewjobtrack.SelectCommand.Parameters.AddWithValue("@ContactTypeId", ContactTyleId);

            //adpviewjobtrack.Fill(dtChatUserDetail);
            //sqlcon.Close();
            return dtChatUserDetail;
        }

        public void InsertMessageDetail(string strSenderName, string strReceiverName, string strMessage,
            string strMessageType, string strUserStatus)
        {
            //context.Usp_InsertChatMessages(strSenderName, strReceiverName, strMessage, strMessageType, strUserStatus);
        }

        public void ManageChatlistStatus(long Id, string strcheckVal)
        {
            //context.Usp_UpdateEnableStatus(Id, strcheckVal);
        }

        public DataTable GetUserChatMessage(string SenderId, string ReceiverId)
        {
            //  var = context.Usp_GetChatMessageDetails(strSenderId, strReceiverId).ToList();

            var dtChatUserMessage = new DataTable();
            //SqlConnection sqlcon = new SqlConnection(context.Database.Connection.ConnectionString);
            //SqlDataAdapter adpMessageList = new SqlDataAdapter("Usp_GetChatMessageDetails", sqlcon);
            //adpMessageList.SelectCommand.CommandType = CommandType.StoredProcedure;
            //adpMessageList.SelectCommand.Parameters.AddWithValue("@SenderId", SenderId);
            //adpMessageList.SelectCommand.Parameters.AddWithValue("@ReceiverId", ReceiverId);

            //adpMessageList.Fill(dtChatUserMessage);
            //sqlcon.Close();
            return dtChatUserMessage;
        }
    }
}