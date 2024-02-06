using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class Inbox
    {
        public long? Id { get; set; }
        public long? ParentId { get; set; }
        public long? ReferenceId { get; set; }
        public int? ReferenceType { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool Unread { get; set; }
        public long? SenderId { get; set; }
        public string SenderName { get; set; }
        public long? ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? MaxRows { get; set; }
        public int Messages { get; set; }
        public List<Inbox> ChildInbox { get; set; }
        public Inbox()
        {
            PageNumber = 1;
            PageSize = 10;
            ChildInbox = new List<Inbox>();
        }
    }
    public class Inboxv2
    {
        public long? Id { get; set; }
        public long? ParentId { get; set; }
        public long? ReferenceId { get; set; }
        public int? ReferenceType { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Unread { get; set; }
        public long? SenderId { get; set; }
        public string SenderName { get; set; }
        public long? ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverName1 { get; set; }
        public string Status { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? MaxRows { get; set; }
        public string Messages { get; set; }
        public List<Inboxv2> ChildInbox { get; set; }
        public Inboxv2()
        {
            PageNumber = 1;
            PageSize = 10;
            ChildInbox = new List<Inboxv2>();
        }
    }

    public class Inboxv
    {
        public long? Id { get; set; }
        public long? ParentId { get; set; }
        public long? ReferenceId { get; set; }
        public int? ReferenceType { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Unread { get; set; }
        public long? SenderId { get; set; }
        public string SenderName { get; set; }
        public long? ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? MaxRows { get; set; }
        public string Messages { get; set; }
        public List<Inboxv> ChildInbox { get; set; }
        public Inboxv()
        {
            PageNumber = 1;
            PageSize = 10;
            ChildInbox = new List<Inboxv>();
        }
    }
    public class Inboxv1
    {
        public long? Id { get; set; }
        public long? ParentId { get; set; }
        public long? ReferenceId { get; set; }
        public int? ReferenceType { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Unread { get; set; }
        public long? SenderId { get; set; }
        public string SenderName { get; set; }
        public long? ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverName1 { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? MaxRows { get; set; }
        public string Messages { get; set; }
        public List<Inboxv1> ChildInbox { get; set; }
        public Inboxv1()
        {
            PageNumber = 1;
            PageSize = 10;
            ChildInbox = new List<Inboxv1>();
        }
    }

    public class InboxFile
    {
        public long? Id { get; set; }
        public long InboxId { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public int Size { get; set; }
    }
}
