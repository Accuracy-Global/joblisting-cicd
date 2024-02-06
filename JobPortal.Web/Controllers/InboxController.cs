using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Helpers;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Web.Controllers
{
    public class InboxController : BaseController
    {
#pragma warning disable CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
        IJobService jobService;
#pragma warning restore CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
        public InboxController(IUserService service, IJobService jobService)
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
            : base(service)
        {
            this.jobService = jobService;
        }
        [Authorize]
        // GET: Inbox
        public ActionResult Index(int pageNumber = 1)
        {
            UserInfoEntity uinfo = _service.Get(User.Id);
            if (!uinfo.IsConfirmed)
            {
                return RedirectToAction("Confirm", "Account", new { id = uinfo.Id, returnUrl = Request.Url.ToString() });
            }

            List<Inbox> list = new List<Inbox>();
            List<Inboxv> list1 = new List<Inboxv>();
            int rows = 0;
            int pageSize = 15;

            list = DomainService.Instance.ListInbox(User.Id, null, null, pageNumber);
            if (list.Count > 0)
            {
                rows = list.FirstOrDefault().MaxRows.Value;
            }


            ViewBag.Model = new StaticPagedList<Inbox>(list, pageNumber, pageSize, rows);
            ViewBag.Rows = rows;

            return View();
        }

        [Authorize]
        public ActionResult Show(long Id)
        {
            Inbox inbox = DomainService.Instance.GetInboxItem(Id);
            Inbox updateInbox = new Inbox()
            {
                Id = Id,
                ReceiverId = User.Id,
                Unread = false
            };
            DomainService.Instance.ManageInbox(updateInbox);

            inbox.ChildInbox = DomainService.Instance.ListChildInbox(Id);

            if (User.Info.Role == SecurityRoles.Jobseeker || User.Info.Role == SecurityRoles.Employers)
            {
                Inbox rInbox = DomainService.Instance.RestrictedInboxCount(inbox.ReferenceId.Value);
                ViewBag.Enabled = (rInbox.ReceiverId == User.Id);
            }

            return View(inbox);
        }

        [Authorize]
        public ActionResult Show1(string uname)
        {
            ViewBag.uname = uname;
            return View();
        }
        [Authorize]
        public ActionResult Show2(string uname)
        {
            ViewBag.uname = uname;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Send(long id, string message)
        {
            Inbox inbox = DomainService.Instance.GetInboxItem(id);

            Inbox ibox = new Inbox()
            {
                Subject = inbox.Subject,
                Body = message,
                ReceiverId = inbox.SenderId,
                SenderId = User.Id,
                ReferenceId = inbox.ReferenceId,
                ReferenceType = inbox.ReferenceType,
                ParentId = inbox.Id,
                Unread = true
            };

            long inboxId = DomainService.Instance.ManageInbox(ibox);
            TempData["UpdateData"] = "Message sent successfully!";
            return Json(inboxId, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'InboxFile' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AttachFile(InboxFile model)
#pragma warning restore CS0246 // The type or namespace name 'InboxFile' could not be found (are you missing a using directive or an assembly reference?)
        {
            int stat = 0;
            if (!string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.Data))
            {
                InboxFile attach = new InboxFile()
                {
                    InboxId = model.InboxId,
                    Name = model.Name,
                    Data = model.Data,
                    Size = model.Size
                };
                stat = DomainService.Instance.ManageInboxItem(attach);
            }
            return Json(stat, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Download(long Id)
        {
            InboxFile inboxFile = DomainService.Instance.GetInboxFile(Id);
            return File(Convert.FromBase64String(inboxFile.Data), MediaTypeNames.Application.Octet, inboxFile.Name);
        }

        [Authorize(Roles = "SuperUser, Administrator, Sales1")]
        public ActionResult DeleteFile(long Id)
        {
            InboxFile iFile = DomainService.Instance.GetInboxFile(Id);
            Inbox inbox = DomainService.Instance.GetInboxItem(iFile.InboxId);
            TempData["UpdateData"] = string.Format("{0} deleted successfully!", iFile.Name);

            InboxFile inboxFile = new InboxFile()
            {
                Id = iFile.Id
            };

            DomainService.Instance.ManageInboxItem(inboxFile);
            ActionResult returnto = new RedirectResult(Request.UrlReferrer.ToString());

            return returnto;
        }

        [Authorize(Roles = "SuperUser, Administrator, Sales1")]
        public ActionResult Mark(long Id)
        {
            Inbox inbox = DomainService.Instance.GetInboxItem(Id);
            if (inbox.Unread)
            {
                TempData["UpdateData"] = "Successfully marked as Read!";
                Inbox ibox = new Inbox()
                {
                    Id = Id,
                    Unread = false,
                    ReceiverId = User.Id
                };
                DomainService.Instance.MarkInboxItem(ibox);
            }
            else
            {
                TempData["UpdateData"] = "Successfully marked as Unread!";
                Inbox ibox = new Inbox()
                {
                    Id = Id,
                    Unread = true,
                    ReceiverId = User.Id
                };
                DomainService.Instance.MarkInboxItem(ibox);
            }

            ActionResult return_action = new RedirectResult(Request.UrlReferrer.ToString());
            return return_action;
        }
    }
}