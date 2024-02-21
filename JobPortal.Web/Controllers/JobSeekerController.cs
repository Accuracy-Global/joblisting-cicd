using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web.Mvc;
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
using JobPortal.Web.Models;
using Microsoft.Web.WebPages.OAuth;
using PagedList;
using System.Web;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Threading.Tasks;
using iTextSharp.tool.xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
using JobPortal.Web.App_Start;
using System.Net;
using System.Web.Script.Serialization;
using JobPortal.Web.Models.Pagination;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)

namespace JobPortal.Web.Controllers
{
    public class JobSeekerController : BaseController
    {
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
        IUserService service;
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
        IJobService jobService;
#pragma warning restore CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INetworkService' could not be found (are you missing a using directive or an assembly reference?)
        INetworkService iNetworkService;
#pragma warning restore CS0246 // The type or namespace name 'INetworkService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
        IHelperService helper;
#pragma warning restore CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INetworkService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
        public JobSeekerController(IUserService service, IJobService jobService, IHelperService helper, INetworkService iNetworkService)
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'INetworkService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
            : base(service)
        {
            this.service = service;
            this.jobService = jobService;
            this.iNetworkService = iNetworkService;
            this.helper = helper;
        }
        public enum AlertType
        {
            RecommendedJobs = 1,
            Tips = 2
        }

        private StringBuilder strEmailContent = new StringBuilder();
#pragma warning disable CS0246 // The type or namespace name 'AutomatchJob' could not be found (are you missing a using directive or an assembly reference?)
        List<AutomatchJob> jobList = new List<AutomatchJob>();
#pragma warning restore CS0246 // The type or namespace name 'AutomatchJob' could not be found (are you missing a using directive or an assembly reference?)


        [Authorize]
        public ActionResult CareerTips()
        {
            UserProfile profile = MemberService.Instance.Get(User.Username);
            List<Tip> list = new List<Tip>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<Tip>();/*.Where(x => x.Type == profile.Type);*/
                list = result.ToList();
            }
            return View(list);
        }

        [Authorize]
        [System.Web.Mvc.HttpPost]
        public ActionResult deleteskill(long userid, string skillname)
        {
            string flag = "Failed";
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                int i = jobService.DelSkillData(userid, skillname);
                if (i == 1)
                {
                    flag = "Deleted successfully!";
                }
            }
            return View("UpdateProfile1", "JobSeeker");
        }
        [Authorize]
        [System.Web.Mvc.HttpPost]
        public ActionResult deletecert(long userid, string cert)
        {
            string flag = "Failed";
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                int i = jobService.DelCertData(userid, cert);
                if (i == 1)
                {
                    flag = "Deleted successfully!";
                }
            }
            return View("UpdateProfile1", "JobSeeker");
        }
        [Authorize]
        [System.Web.Mvc.HttpPost]
        public ActionResult deleteedu(long userid, string edu)
        {
            string flag = "Failed";
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                int i = jobService.DelEduData(userid, edu);
                if (i == 1)
                {
                    flag = "Deleted successfully!";
                }
            }

            return View("UpdateProfile1", "JobSeeker");
        }


        [Authorize]
        public ActionResult InviteViaEmailVerEd(long UserId, string emailv, string edv, string scv, string ftv, string ttv)
        {
            string flag = "Failed";
            UserInfoEntity profile = _service.Get(UserId);
            if (!string.IsNullOrEmpty(emailv))
            {
                if (profile != null)
                {
                    var reader = new StreamReader(Server.MapPath("~/Templates/Mail/empverifyse.html"));
                    var body = reader.ReadToEnd();


                    body = body.Replace("@@person", profile.FullName);
                    body = body.Replace("@person", profile.FullName);
                    body = body.Replace("@@scv", scv);
                    body = body.Replace("@scv", scv);
                    body = body.Replace("@@edv", edv);
                    body = body.Replace("@edv", edv);
                    body = body.Replace("@@ftv", ftv);
                    body = body.Replace("@ftv", ftv);
                    body = body.Replace("@@ttv", ttv);
                    body = body.Replace("@ttv", ttv);
                    body = body.Replace("@@x", emailv);
                    body = body.Replace("@x", emailv);






                    string[] receipent = { profile.Username };

                    var subject = string.Format("{0} Verified", string.Format("{0} {1}", profile.FirstName, profile.LastName));
                    AlertService.Instance.SendMail(subject, receipent, body);
                    int i = jobService.InviteViaEmailEdU(UserId, emailv, "", edv, scv, ftv, ttv, "");
                    if (i == 1)
                    {
                        flag = "Verified Mail Send successfully!";
                        return RedirectToAction("UpdateProfile1", "JobSeeker");
                    }



                }
                else
                {

                }
            }
            else
            {

            }
            return Json(flag, JsonRequestBehavior.AllowGet);


            // return View("UpdateProfile1", "JobSeeker");

        }

        [Authorize]
        public ActionResult InviteViaEmailVer(long UserId, string emailv, string comv, string indv, string conv)
        {
            string flag = "Failed";
            UserInfoEntity profile = _service.Get(UserId);
            if (!string.IsNullOrEmpty(emailv))
            {
                if (profile != null)
                {
                    var reader = new StreamReader(Server.MapPath("~/Templates/Mail/empverifys.html"));
                    var body = reader.ReadToEnd();


                    body = body.Replace("@@person", profile.FullName);
                    body = body.Replace("@person", profile.FullName);
                    body = body.Replace("@@com", comv);
                    body = body.Replace("@com", comv);
                    body = body.Replace("@@con", conv);
                    body = body.Replace("@con", conv);
                    body = body.Replace("@@x", emailv);
                    body = body.Replace("@x", emailv);






                    string[] receipent = { profile.Username };

                    var subject = string.Format("{0} Verified", string.Format("{0} {1}", profile.FirstName, profile.LastName));
                    AlertService.Instance.SendMail(subject, receipent, body);
                    int i = jobService.InviteViaEmailU(UserId, emailv, "", comv, indv.Replace("and", "&"), conv, "");
                    if (i == 1)
                    {
                        flag = "Verified Mail Send successfully!";
                        return RedirectToAction("UpdateProfile1", "JobSeeker");
                    }



                }
                else
                {

                }
            }
            else
            {

            }
            return Json(flag, JsonRequestBehavior.AllowGet);


            // return View("UpdateProfile1", "JobSeeker");

        }



        [Authorize]
        [HttpPost]
        public ActionResult VerifyToken1(string Username, string Tokenv, string url)
        {
            string u = url;
            string[] v = u.Split('=');
            var sender = MemberService.Instance.Get(Username);
            var receiver = MemberService.Instance.Get(Convert.ToInt32(v[1]));
            UserInfoEntity profile = _service.Get(Username);
            if (!string.IsNullOrEmpty(Tokenv))
            {
                if (profile != null)
                {
                    int i = jobService.VerifyTokenM(profile.Id, Username, Tokenv);
                    if (i == 1)
                    {
                        int j = jobService.UpdateTokenM(profile.Id);

                        bool registered = false;
                        bool connected = false;
                        bool blocked = false;
                        string[] receipent = new string[1];
                        string subject;
                        RedirectToRouteResult result = null;

                        if (sender != null && receiver != null)
                        {
                            string receiverName = (!string.IsNullOrEmpty(receiver.Company) ? receiver.Company : string.Format("{0} {1}", receiver.FirstName, receiver.LastName));
                            string senderName = (!string.IsNullOrEmpty(sender.Company) ? sender.Company : string.Format("{0} {1}", sender.FirstName, sender.LastName));

                            var mrelation = ConnectionHelper.GetEntity(receiver.Username, sender.Username);
                            var orelation = ConnectionHelper.GetEntity(sender.Username, receiver.Username);

                            var blockedEntity = ConnectionHelper.GetBlockedEntity(receiver.UserId, sender.UserId);

                            registered = (receiver != null);
                            connected = ConnectionHelper.IsConnected(receiver.Username, sender.Username);
                            blocked = ConnectionHelper.IsBlocked(sender.Username, receiver.UserId);

                            Communication entity = new Communication();
                            string msg = "";
                            if (!string.IsNullOrEmpty(TempData["M"].ToString()))
                            {
                                msg = TempData["M"].ToString().RemoveEmails();
                                msg = msg.RemoveNumbers();
                                msg = msg.RemoveWebsites();
                            }

                            if (registered == true && connected == false && blocked == true)
                            {
                                if (!blockedEntity.CreatedBy.Equals(User.Username))
                                {
                                    TempData["Instruction"] = string.Format("{0} {1} do not want to receive messages from you!!", receiver.FileName, receiver.LastName);
                                    result = RedirectToAction("List", "Message", new { SenderId = receiver.UserId });
                                }
                                else
                                {
                                    DomainService.Instance.Unblock(receiver.UserId, sender.UserId);

                                    using (JobPortalEntities context = new JobPortalEntities())
                                    {
                                        DataHelper dataHelper = new DataHelper(context);
                                        if (mrelation == null && orelation == null)
                                        {
                                            mrelation = new Connection()
                                            {
                                                UserId = sender.UserId,
                                                FirstName = !string.IsNullOrEmpty(receiver.FirstName) ? receiver.FirstName.TitleCase() : "",
                                                LastName = !string.IsNullOrEmpty(receiver.LastName) ? receiver.LastName.TitleCase() : "",
                                                EmailAddress = receiver.Username,
                                                IsAccepted = false,
                                                IsConnected = false,
                                                IsBlocked = false,
                                                IsDeleted = false,
                                                Initiated = true
                                            };
                                            dataHelper.Add<Connection>(mrelation, User.Username);

                                            orelation = new Connection()
                                            {
                                                UserId = receiver.UserId,
                                                FirstName = !string.IsNullOrEmpty(sender.FirstName) ? sender.FirstName.TitleCase() : "",
                                                LastName = !string.IsNullOrEmpty(sender.LastName) ? sender.LastName.TitleCase() : "",
                                                EmailAddress = sender.Username,
                                                IsBlocked = false,
                                                IsAccepted = false,
                                                IsConnected = false,
                                                IsDeleted = false,
                                                Initiated = true
                                            };
                                            dataHelper.Add<Connection>(orelation, User.Username);

                                            MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username, true);
                                            _service.ManageAccount(User.Id, receiver.UserId, null, 1);

                                            using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitationmessage.html")))
                                            {
                                                string body = reader.ReadToEnd();
                                                body = body.Replace("@@sender", string.Format("{0}", senderName));
                                                string url1 = string.Format("{0}://{1}/Message/Index", Request.Url.Scheme, Request.Url.Authority);
                                                string viewurl = string.Format("{0}://{1}/Message/Accept?ConnectionId={2}", Request.Url.Scheme, Request.Url.Authority, mrelation.Id);
                                                body = body.Replace("@@viewurl", viewurl);
                                                body = body.Replace("@@message", msg);
                                                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));

                                                body = body.Replace("@@receiver", receiverName);
                                                subject = string.Format("Message from {0}", senderName);

                                                receipent[0] = receiver.Username;
                                                AlertService.Instance.SendMail(subject, receipent, body);
                                            }
                                        }
                                        else if (mrelation != null && orelation != null)
                                        {
                                            if (mrelation.IsDeleted == true && orelation.IsDeleted == true)
                                            {
                                                mrelation.IsAccepted = false;
                                                mrelation.IsConnected = false;
                                                mrelation.IsDeleted = false;
                                                mrelation.IsBlocked = false;
                                                mrelation.Initiated = true;
                                                mrelation.DateUpdated = DateTime.Now;
                                                mrelation.UpdatedBy = User.Username;
                                                mrelation.CreatedBy = User.Username;
                                                dataHelper.UpdateEntity<Connection>(mrelation);

                                                orelation.IsAccepted = false;
                                                orelation.IsConnected = false;
                                                orelation.IsDeleted = false;
                                                orelation.IsBlocked = false;
                                                orelation.Initiated = true;
                                                orelation.DateUpdated = DateTime.Now;
                                                orelation.UpdatedBy = User.Username;
                                                orelation.CreatedBy = User.Username;
                                                dataHelper.UpdateEntity<Connection>(orelation);

                                                dataHelper.Save();

                                                MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username, true);
                                                _service.ManageAccount(User.Id, receiver.UserId, null, 1);


                                                using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitationmessage.html")))
                                                {
                                                    string body = reader.ReadToEnd();

                                                    body = body.Replace("@@sender", string.Format("{0}", senderName));
                                                    string url1 = string.Format("{0}://{1}/Message/Index", Request.Url.Scheme, Request.Url.Authority);
                                                    string viewurl = string.Format("{0}://{1}/Message/Accept?ConnectionId={2}", Request.Url.Scheme, Request.Url.Authority, mrelation.Id);
                                                    body = body.Replace("@@viewurl", viewurl);
                                                    body = body.Replace("@@message", msg);
                                                    body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));

                                                    body = body.Replace("@@receiver", receiverName);
                                                    subject = string.Format("Message from {0}", senderName);

                                                    receipent[0] = receiver.Username;
                                                    AlertService.Instance.SendMail(subject, receipent, body);
                                                }
                                                TempData["UpdateData"] = "Message sent successfully!";
                                            }
                                            else
                                            {
                                                if (!mrelation.CreatedBy.Equals(User.Username) && !orelation.CreatedBy.Equals(User.Username))
                                                {
                                                    mrelation.IsAccepted = true;
                                                    mrelation.IsConnected = true;
                                                    mrelation.IsDeleted = false;
                                                    mrelation.IsBlocked = false;
                                                    mrelation.DateUpdated = DateTime.Now;
                                                    mrelation.UpdatedBy = User.Username;
                                                    dataHelper.UpdateEntity<Connection>(mrelation);

                                                    orelation.IsAccepted = true;
                                                    orelation.IsConnected = true;
                                                    orelation.IsDeleted = false;
                                                    orelation.IsBlocked = false;
                                                    orelation.DateUpdated = DateTime.Now;
                                                    orelation.UpdatedBy = User.Username;
                                                    dataHelper.UpdateEntity<Connection>(orelation);

                                                    dataHelper.Save();

                                                    MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username, true);
                                                    //_service.ManageAccount(User.Id, receiver.UserId, null, 1);

                                                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/message.html")))
                                                    {
                                                        string body = reader.ReadToEnd();
                                                        body = body.Replace("@@sender", senderName);
                                                        body = body.Replace("@@message", msg);
                                                        string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, sender.UserId);
                                                        body = body.Replace("@@viewurl", viewurl);
                                                        body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));
                                                        body = body.Replace("@@navigateurl", string.Format("{0}://{1}/Message", Request.Url.Scheme, Request.Url.Authority));

                                                        body = body.Replace("@@receiver", receiverName);
                                                        subject = string.Format("Message from {0}", senderName);

                                                        receipent[0] = receiver.Username;
                                                        AlertService.Instance.SendMail(subject, receipent, body);
                                                    }

                                                    using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation_accepted.html")))
                                                    {
                                                        string body = reader.ReadToEnd();
                                                        body = body.Replace("@@receiver", receiverName);
                                                        body = body.Replace("@@sender", senderName);
                                                        body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));
                                                        string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, sender.UserId);
                                                        body = body.Replace("@@viewurl", viewurl);

                                                        receipent[0] = receiver.Username;
                                                        subject = string.Format("{0} has Accepted Connection", senderName);
                                                        AlertService.Instance.SendMail(subject, receipent, body);
                                                    }
                                                    TempData["UpdateData"] = "Message sent successfully!";
                                                }
                                                else
                                                {
                                                    MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username, true);
                                                    //if (MessageService.Instance.Count(receiver.UserId, sender.UserId) == 0)
                                                    //{
                                                    //    _service.ManageAccount(User.Id, receiver.UserId, null, 1);
                                                    //}
                                                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitationmessage.html")))
                                                    {
                                                        string body = reader.ReadToEnd();

                                                        body = body.Replace("@@sender", string.Format("{0}", senderName));
                                                        string url1 = string.Format("{0}://{1}/Message/Index", Request.Url.Scheme, Request.Url.Authority);
                                                        string viewurl = string.Format("{0}://{1}/Message/Accept?ConnectionId={2}", Request.Url.Scheme, Request.Url.Authority, mrelation.Id);
                                                        body = body.Replace("@@viewurl", viewurl);
                                                        body = body.Replace("@@message", msg);
                                                        body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));

                                                        body = body.Replace("@@receiver", receiverName);
                                                        subject = string.Format("Message from {0}", senderName);

                                                        receipent[0] = receiver.Username;
                                                        AlertService.Instance.SendMail(subject, receipent, body);
                                                    }
                                                    TempData["UpdateData"] = "Message sent successfully!";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (registered == true && connected == true && blocked == true)
                            {
                                if (!blockedEntity.CreatedBy.Equals(User.Username))
                                {
                                    TempData["Instruction"] = string.Format("{0} {1} do not want to receive messages from you!!", receiver.FileName, receiver.LastName);
                                    result = RedirectToAction("List", "Message", new { SenderId = receiver.UserId });
                                }
                            }
                            else if (registered == true && connected == true && blocked == false)
                            {
                                MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username);
                                using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/message.html")))
                                {
                                    string body = reader.ReadToEnd();
                                    body = body.Replace("@@sender", senderName);
                                    body = body.Replace("@@message", msg);
                                    string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, sender.UserId);
                                    body = body.Replace("@@viewurl", viewurl);
                                    body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));
                                    body = body.Replace("@@navigateurl", string.Format("{0}://{1}/Message", Request.Url.Scheme, Request.Url.Authority));

                                    body = body.Replace("@@receiver", receiverName);
                                    subject = string.Format("Message from {0}", senderName);

                                    receipent[0] = receiver.Username;
                                    AlertService.Instance.SendMail(subject, receipent, body);
                                }
                                TempData["UpdateData"] = "Message sent successfully!";
                                return View("List", "Message", new { SenderId = receiver.UserId });
                            }
                            else if (registered == true && connected == false && blocked == false)
                            {
                                using (JobPortalEntities context = new JobPortalEntities())
                                {
                                    DataHelper dataHelper = new DataHelper(context);
                                    if (mrelation == null && orelation == null)
                                    {
                                        mrelation = new Connection()
                                        {
                                            UserId = sender.UserId,
                                            FirstName = !string.IsNullOrEmpty(receiver.FirstName) ? receiver.FirstName.TitleCase() : "",
                                            LastName = !string.IsNullOrEmpty(receiver.LastName) ? receiver.LastName.TitleCase() : "",
                                            EmailAddress = receiver.Username,
                                            IsAccepted = false,
                                            IsConnected = false,
                                            IsBlocked = false,
                                            IsDeleted = false,
                                            Initiated = true
                                        };
                                        dataHelper.Add<Connection>(mrelation, User.Username);

                                        orelation = new Connection()
                                        {
                                            UserId = receiver.UserId,
                                            FirstName = !string.IsNullOrEmpty(sender.FirstName) ? sender.FirstName.TitleCase() : "",
                                            LastName = !string.IsNullOrEmpty(sender.LastName) ? sender.LastName.TitleCase() : "",
                                            EmailAddress = sender.Username,
                                            IsBlocked = false,
                                            IsAccepted = false,
                                            IsConnected = false,
                                            IsDeleted = false,
                                            Initiated = true
                                        };
                                        dataHelper.Add<Connection>(orelation, User.Username);

                                        MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username, true);
                                        _service.ManageAccount(User.Id, receiver.UserId, null, 1);
                                        using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitationmessage.html")))
                                        {
                                            string body = reader.ReadToEnd();
                                            body = body.Replace("@@sender", string.Format("{0}", senderName));
                                            string url1 = string.Format("{0}://{1}/Message/Index", Request.Url.Scheme, Request.Url.Authority);
                                            string viewurl = string.Format("{0}://{1}/Message/Accept?ConnectionId={2}", Request.Url.Scheme, Request.Url.Authority, mrelation.Id);
                                            body = body.Replace("@@viewurl", viewurl);
                                            body = body.Replace("@@message", msg);
                                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));

                                            body = body.Replace("@@receiver", receiverName);
                                            subject = string.Format("Message from {0}", senderName);

                                            receipent[0] = receiver.Username;
                                            AlertService.Instance.SendMail(subject, receipent, body);
                                        }
                                        return View("List", "Message", new { SenderId = receiver.UserId });
                                    }
                                    else if (mrelation != null && orelation != null)
                                    {
                                        if (mrelation.IsDeleted == true && orelation.IsDeleted == true)
                                        {
                                            mrelation.IsAccepted = false;
                                            mrelation.IsConnected = false;
                                            mrelation.IsDeleted = false;
                                            mrelation.IsBlocked = false;
                                            mrelation.Initiated = true;
                                            mrelation.DateUpdated = DateTime.Now;
                                            mrelation.UpdatedBy = User.Username;
                                            mrelation.CreatedBy = User.Username;
                                            dataHelper.UpdateEntity<Connection>(mrelation);

                                            orelation.IsAccepted = false;
                                            orelation.IsConnected = false;
                                            orelation.IsDeleted = false;
                                            orelation.IsBlocked = false;
                                            orelation.Initiated = true;
                                            orelation.DateUpdated = DateTime.Now;
                                            orelation.UpdatedBy = User.Username;
                                            orelation.CreatedBy = User.Username;
                                            dataHelper.UpdateEntity<Connection>(orelation);

                                            dataHelper.Save();

                                            MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username, true);
                                            _service.ManageAccount(User.Id, receiver.UserId, null, 1);
                                            using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitationmessage.html")))
                                            {
                                                string body = reader.ReadToEnd();

                                                body = body.Replace("@@sender", string.Format("{0}", senderName));
                                                string url1 = string.Format("{0}://{1}/Message/Index", Request.Url.Scheme, Request.Url.Authority);
                                                string viewurl = string.Format("{0}://{1}/Message/Accept?ConnectionId={2}", Request.Url.Scheme, Request.Url.Authority, mrelation.Id);
                                                body = body.Replace("@@viewurl", viewurl);
                                                body = body.Replace("@@message", msg);
                                                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));

                                                body = body.Replace("@@receiver", receiverName);
                                                subject = string.Format("Message from {0}", senderName);

                                                receipent[0] = receiver.Username;
                                                AlertService.Instance.SendMail(subject, receipent, body);
                                            }
                                            TempData["UpdateData"] = "Message sent successfully!";
                                        }
                                        else
                                        {
                                            if (!mrelation.CreatedBy.Equals(User.Username) && !orelation.CreatedBy.Equals(User.Username))
                                            {
                                                mrelation.IsAccepted = true;
                                                mrelation.IsConnected = true;
                                                mrelation.IsDeleted = false;
                                                mrelation.IsBlocked = false;
                                                mrelation.DateUpdated = DateTime.Now;
                                                mrelation.UpdatedBy = User.Username;
                                                dataHelper.UpdateEntity<Connection>(mrelation);

                                                orelation.IsAccepted = true;
                                                orelation.IsConnected = true;
                                                orelation.IsDeleted = false;
                                                orelation.IsBlocked = false;
                                                orelation.DateUpdated = DateTime.Now;
                                                orelation.UpdatedBy = User.Username;
                                                dataHelper.UpdateEntity<Connection>(orelation);

                                                dataHelper.Save();

                                                MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username, true);
                                                //_service.ManageAccount(User.Id, receiver.UserId, null, 1);
                                                using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/message.html")))
                                                {
                                                    string body = reader.ReadToEnd();
                                                    body = body.Replace("@@sender", senderName);
                                                    body = body.Replace("@@message", msg);
                                                    string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, sender.UserId);
                                                    body = body.Replace("@@viewurl", viewurl);
                                                    body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));
                                                    body = body.Replace("@@navigateurl", string.Format("{0}://{1}/Message", Request.Url.Scheme, Request.Url.Authority));

                                                    body = body.Replace("@@receiver", receiverName);
                                                    subject = string.Format("Message from {0}", senderName);

                                                    receipent[0] = receiver.Username;
                                                    AlertService.Instance.SendMail(subject, receipent, body);
                                                }

                                                using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation_accepted.html")))
                                                {
                                                    string body = reader.ReadToEnd();
                                                    body = body.Replace("@@receiver", receiverName);
                                                    body = body.Replace("@@sender", senderName);
                                                    body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));
                                                    string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, sender.UserId);
                                                    body = body.Replace("@@viewurl", viewurl);

                                                    receipent[0] = receiver.Username;
                                                    subject = string.Format("{0} has Accepted Connection", senderName);
                                                    AlertService.Instance.SendMail(subject, receipent, body);
                                                }
                                                TempData["UpdateData"] = "Message sent successfully!";
                                            }
                                            else
                                            {
                                                MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username, true);
                                                //_service.ManageAccount(User.Id, receiver.UserId, null, 1);

                                                using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitationmessage.html")))
                                                {
                                                    string body = reader.ReadToEnd();

                                                    body = body.Replace("@@sender", string.Format("{0}", senderName));
                                                    string url1 = string.Format("{0}://{1}/Message/Index", Request.Url.Scheme, Request.Url.Authority);
                                                    string viewurl = string.Format("{0}://{1}/Message/Accept?ConnectionId={2}", Request.Url.Scheme, Request.Url.Authority, mrelation.Id);
                                                    body = body.Replace("@@viewurl", viewurl);
                                                    body = body.Replace("@@message", msg);
                                                    body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));

                                                    body = body.Replace("@@receiver", receiverName);
                                                    subject = string.Format("Message from {0}", senderName);

                                                    receipent[0] = receiver.Username;
                                                    AlertService.Instance.SendMail(subject, receipent, body);
                                                }
                                                TempData["UpdateData"] = "Message sent successfully!";
                                            }
                                        }
                                        return View("List", "Message", new { SenderId = receiver.UserId });
                                    }
                                }
                                return View("List", "Message", new { SenderId = receiver.UserId });
                            }
                        }
                        else
                        {
                            TempData["UpdateData"] = "Message sending failed, unable to retrieve recipient details!";
                        }
                        return View("List", "Message", new { SenderId = receiver.UserId });
                    }
                }

            }
            else
            {

            }
            //return Json(context, JsonRequestBehavior.AllowGet);
            return View("List", "Message", new { SenderId = receiver.UserId });

        }



        [Authorize]
        [HttpPost]
        public ActionResult VerifyToken8(string Username, string Tokenv, string url, string id, string type)
        {
            string u = url;
            var sender = MemberService.Instance.Get(Username);
            //  var receiver = MemberService.Instance.Get(Convert.ToInt32(v[1]));
            UserInfoEntity profile = _service.Get(Username);
            if (!string.IsNullOrEmpty(Tokenv))
            {
                if (profile != null)
                {
                    int i = jobService.VerifyTokenP(profile.Id, Username, Tokenv);
                    if (i == 1)
                    {
                        int j = jobService.UpdateTokenP(profile.Id);

                        if (type == "J")
                        {
                            ViewBag.Entity = JobService.Instance.Get(Convert.ToInt32(id));
                        }
                        else if (type == "P")
                        {
                            ViewBag.Entity = _service.Get(id);

                            decimal weightage = MemberService.Instance.GetProfileWeightage(Convert.ToInt32(id));
                            if (weightage < 90)
                            {
                                return RedirectToAction("UpdateProfile1", "Jobseeker", new { type = type, returnUrl = string.Format("/package/promote?id={0}&type=P&returnurl={1}", id, url) });
                            }
                        }
                        ViewBag.Promote = helper.PromotePrice(Convert.ToInt32(id), type);

                        return Redirect(url);

                    }
                }

            }
            else
            {

            }

            return Redirect(url);
        }




        [Authorize]
        [HttpPost]
        public ActionResult VerifyToken4(string Username, string Tokenv, string url)
        {
            string u = url;

            var sender = MemberService.Instance.Get(Username);

            UserInfoEntity profile = _service.Get(Username);
            if (!string.IsNullOrEmpty(Tokenv))
            {
                if (profile != null)
                {
                    int i = jobService.VerifyTokenI(profile.Id, Username, Tokenv);
                    if (i == 1)
                    {
                        int j = jobService.UpdateTokenI(profile.Id);

                        var model = new InterviewModel
                        {
                            TrackingId = Guid.Parse(TempData["TrackingId"].ToString()),
                            Round = Convert.ToInt32(TempData["Round"])
                        };
                        ViewBag.Record = TrackingService.Instance.Get(Guid.Parse(TempData["TrackingId"].ToString())) as Tracking;

                        return RedirectToAction("Initiate", "Interview", new { model = model });

                    }
                }

            }
            else
            {

            }
            var model1 = new InterviewModel
            {
                TrackingId = Guid.Parse(TempData["TrackingId"].ToString()),
                Round = Convert.ToInt32(TempData["Round"])
            };
            ViewBag.Record = TrackingService.Instance.Get(Guid.Parse(TempData["TrackingId"].ToString())) as Tracking;

            //return Json(context, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Initiate", "Interview", new { model = model1 });

        }




        [Authorize]
        [HttpPost]
        public ActionResult InviteViaEmail(string Username, string emailv, string emailname, string comv, string indv, string conv, string body1)
        {
            string flag = "Failed";
            UserInfoEntity profile = _service.Get(Username);
            if (!string.IsNullOrEmpty(emailv))
            {
                if (profile != null)
                {
                    var reader = new StreamReader(Server.MapPath("~/Templates/Mail/empverify.html"));
                    var body = reader.ReadToEnd();

                    body = body.Replace("@@body", body1);
                    body = body.Replace("[", "<").Replace("]", ">").Replace("'", "\"").Replace("andnbsp", "&nbsp;");


                    body = body.Replace("@@person", emailname);
                    body = body.Replace("@person", emailname);


                    //body = body.Replace("@@accepturl", string.Format("{0}://{1}/Network/Accept/{2}", Request.Url.Scheme, Request.Url.Authority, id));
                    body = body.Replace("@@button", "Confirm");

                    //body = body.Replace("@@verifyurl", string.Format("{0}://{1}/Network/EmailVerify?UserId={2}&&comv={3}&&indv={4}&&conv={5}&&emailv={6}", 
                    //    Request.Url.Scheme, Request.Url.Authority, profile.Id, comv, indv.Replace("&", "and"), conv.Trim(), emailv));
                    string verifyurl = string.Format("{0}://{1}/Inbox/Show1?uname={2}", Request.Url.Scheme, Request.Url.Authority, emailv);
                    body = body.Replace("@@verifyurl", verifyurl);


                    string[] receipent = { emailv };

                    var subject = string.Format("{0} Employment Verification at Joblisting", string.Format("{0} {1}", profile.FirstName, profile.LastName));
                    AlertService.Instance.SendMail(subject, receipent, body);

                    int i = jobService.InviteViaEmailI(profile.Id, emailv, emailname, comv, indv, conv.Trim(), body1);
                    if (i == 1)
                    {
                        flag = "Verification Mail Send successfully!";
                    }



                }
                else
                {

                }
            }
            else
            {

            }
            //return Json(context, JsonRequestBehavior.AllowGet);
            return View("UpdateProfile1", "JobSeeker");

        }


        [Authorize]
        [HttpPost]
        public ActionResult InviteViaEmailEd(string Username, string emailv1, string emailname1, string edv, string scv, string ftv, string ttv, string body2)
        {
            string flag = "Failed";
            UserInfoEntity profile = _service.Get(Username);
            if (!string.IsNullOrEmpty(emailv1))
            {
                if (profile != null)
                {
                    var reader = new StreamReader(Server.MapPath("~/Templates/Mail/empverify.html"));
                    var body = reader.ReadToEnd();

                    body = body.Replace("@@body", body2);
                    body = body.Replace("[", "<").Replace("]", ">").Replace("'", "\"").Replace("andnbsp", "&nbsp;");


                    body = body.Replace("@@person", emailname1);
                    body = body.Replace("@person", emailname1);


                    //body = body.Replace("@@accepturl", string.Format("{0}://{1}/Network/Accept/{2}", Request.Url.Scheme, Request.Url.Authority, id));
                    body = body.Replace("@@button", "Confirm");

                    string verifyurl = string.Format("{0}://{1}/Inbox/Show2?uname={2}", Request.Url.Scheme, Request.Url.Authority, emailv1);
                    body = body.Replace("@@verifyurl", verifyurl);


                    //body = body.Replace("@@verifyurl", string.Format("{0}://{1}/Network/EmailVerifyEd?UserId={2}&&edv={3}&&scv={4}&&ftv={5}&&ttv={6}&&emailv1={7}",
                    //Request.Url.Scheme, Request.Url.Authority, profile.Id, edv, scv,ftv,ttv, emailv1));



                    string[] receipent = { emailv1 };

                    var subject = string.Format("{0} Education Verification at Joblisting", string.Format("{0} {1}", profile.FirstName, profile.LastName));
                    AlertService.Instance.SendMail(subject, receipent, body);
                    int i = jobService.InviteViaEmailEdI(profile.Id, emailv1, emailname1, edv, scv, ftv, ttv, body2);


                    //if (i == 1)
                    //{
                    //flag = "Verification Mail Send successfully!";
                    //}



                }
                else
                {

                }
            }
            else
            {

            }
            //return Json(context, JsonRequestBehavior.AllowGet);
            return View("UpdateProfile1", "JobSeeker");

        }


        [Authorize]
        [System.Web.Mvc.HttpPost]
        public ActionResult deleteexp(long userid, string emp, string ind)
        {
            string flag = "Failed";
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                int i = jobService.DelExpData(userid, emp, ind);
                if (i == 1)
                {
                    flag = "Deleted successfully!";
                }
            }
            return View("UpdateProfile1", "JobSeeker");
        }
        public ActionResult ShowCareerTip(long Id)
        {
            UserProfile profile = MemberService.Instance.Get(User.Username);
            Tip entity = new Tip();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                entity = dataHelper.GetSingle<Tip>(Id);
            }
            return View(entity);
        }
        public ActionResult Block(string email, string redirect)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                UserProfile blocked = dataHelper.GetSingle<UserProfile>("Username", email);

                if (blocked != null && profile != null)
                {
                    BlockedPeople model = dataHelper.Get<BlockedPeople>().SingleOrDefault(x => x.BlockedId == blocked.UserId && x.BlockerId == profile.UserId);
                    if (model == null)
                    {
                        model = new BlockedPeople()
                        {
                            BlockedId = blocked.UserId,
                            BlockerId = profile.UserId
                        };
                        dataHelper.Add<BlockedPeople>(model, User.Username);
                        TempData["UpdateData"] = string.Format("You have successfully blocked {0}!", (!string.IsNullOrEmpty(blocked.Company) ? blocked.Company : string.Format("{0} {1}", blocked.FirstName, blocked.LastName)));
                    }
                }
            }
            return Redirect(redirect);
        }

        [Authorize]
        [UrlPrivilegeFilter]
        public ActionResult Index()
        {
            UserInfoEntity uinfo = _service.Get(User.Id);
            if (!uinfo.IsConfirmed)
            {
                return RedirectToAction("Confirm", "Account", new { id = uinfo.Id, returnUrl = Request.Url.ToString() });
            }
            return View();
        }

        [UrlPrivilegeFilter]
        public ActionResult Resume12(long uid)
        {

            // Validate that the resume download subscription is already active for the logged in user.
            // IF subscription is active, then open the download page
            // Else Go through the gateway to active the subscription.
            // Need to check for existing promo codes available, if it is available then apply the promocode.

            //var paymentProcessEnabled = DomainService.Instance.PaymentProcessEnabled();
            //var hasDownloadQuota = DomainService.Instance.HasDownloadQuota(uid);
            //int verifiedTokenCount = jobService.VerifiedTokenR(Convert.ToInt32(uid));
            //bool isPaidResume = DomainService.Instance.IsPaidResume(UserInfo.Id, uid);

            //if (paymentProcessEnabled && !hasDownloadQuota && verifiedTokenCount == 0 && !isPaidResume)
            //    return RedirectToAction("ResumePriceList", "Package", new { id = uid, returnurl = Request.Url.ToString(), type = "R", countryId = User.Info.CountryId });

            //ViewBag.uid = uid;
            //return View();
            ViewBag.uid = uid;

            return View();
        }

        [HttpPost]
        public PartialViewResult GetRResp(long userid, string emp, string ind, string cat)
        {
            List<User_Experience> rrList = MemberService.Instance.GetUserexpr(userid, emp, ind, cat);
            ViewBag.rrList = rrList;
            return PartialView("_RRPartial");
        }

        //[HttpPost]
        //public ActionResult Getskill(string a)
        //{
        //    List<SkillList> slist = MemberService.Instance.GetSkillList(a);
        //    ViewBag.slist = slist;
        //    return View(slist);
        //}



        [Authorize]
        [HttpGet]
        public async Task<ActionResult> ApplicationJobTitles()
        {
            List<ApplicationEntity> list = await _service.ApplictionJobTitleList(User.Id);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> ApplicationCompanies()
        {
            List<ApplicationEntity> list = await _service.ApplictionCompanyList(User.Id);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'ApplicationFilter' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<ActionResult> Index(ApplicationFilter model)
#pragma warning restore CS0246 // The type or namespace name 'ApplicationFilter' could not be found (are you missing a using directive or an assembly reference?)
        {
            model.JobTitle = (model.JobTitle != null && model.JobTitle == "") ? null : model.JobTitle;
            model.JobTitle = (model.JobTitle != null && model.JobTitle == "") ? null : model.JobTitle;
            //List<ApplicationEntity> list = await _service.Applictions(User.Id, model.JobTitle, model.Company, model.Start, model.End, model.PageNumber);
            List<ApplicationEntity> list = await _service.Applictions(User.Id, null, null, null, null, model.PageNumber);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Build Resume
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult BuildResume(string redirectUrl)
        {
            var model = new BuildResumeModel();
            if (User != null)
            {
                var jobSeeker = MemberService.Instance.Get(User.Id);
                if (jobSeeker != null)
                {
                    model = new BuildResumeModel()
                    {
                        Id = jobSeeker.UserId,
                        Title = jobSeeker.Title,
                        CategoryId = jobSeeker.CategoryId,
                        SpecializationId = jobSeeker.SpecializationId,
                        Summary = jobSeeker.Summary,
                        //AreaOfExpertise = jobSeeker.AreaOfExpertise,
                        Skills = jobSeeker.TechnicalSkills,
                        ManagementSkills = jobSeeker.ManagementSkills,
                        Education = jobSeeker.Education,
                        Experience = jobSeeker.ProfessionalExperience,
                        Certifications = jobSeeker.Certifications,
                        Affiliations = jobSeeker.Affiliations,
                        ReturnUrl = redirectUrl
                    };
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult BuildResume(BuildResumeModel model)
        {
            bool flag = false;
            if (ModelState.IsValid)
            {
                UserProfile original = MemberService.Instance.Get(model.Id);
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    original.Summary = model.Summary;
                    //original.AreaOfExpertise = model.AreaOfExpertise;
                    original.TechnicalSkills = model.Skills;
                    original.ManagementSkills = model.ManagementSkills;
                    original.ProfessionalExperience = model.Experience;
                    original.Education = model.Education;
                    original.Certifications = model.Certifications;
                    original.Affiliations = model.Affiliations;


                    dataHelper.Update(original, "Self");
                    if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        TempData["UpdateData"] = "Resume built successfully.";
                    }
                    flag = true;
                }

                if (flag)
                {
                    original = MemberService.Instance.Get(model.Id);
                    byte[] buffer = Pdf(original.UserId);
                    original.Content = System.Text.Encoding.UTF8.GetString(buffer); // Convert.ToBase64String(buffer);
                    original.FileName = string.Format("{0}_{1}.pdf", original.FirstName, original.LastName);
                    original.IsBuild = true;

                    if (!string.IsNullOrEmpty(model.Title))
                    {
                        if ((string.IsNullOrEmpty(original.Title) || (!string.IsNullOrEmpty(original.Title) && !original.Title.Equals(model.Title))) || (original.CategoryId != null && original.CategoryId != model.CategoryId) || (original.SpecializationId != null && original.SpecializationId != model.SpecializationId))
                        {
                            original.Title = model.Title;
                            original.CategoryId = model.CategoryId;
                            original.SpecializationId = model.SpecializationId;
                            MemberService.Instance.Update(original);
                            if (original.IsConfirmed == true)
                            {
                                var job_list = JobSeekerService.Instance.AutomatchedJobs(original);

                                if (job_list.Any())
                                {
                                    var sbbody = new StringBuilder();
                                    var resumeLink = string.Empty;

                                    sbbody.Append("<table>");
                                    sbbody.Append("<tr>");
                                    sbbody.Append("<th>Title</th><th>Last Modified Date</th><th>Number of Matches</th>");
                                    sbbody.Append("</tr>");

                                    var automatchLink =
                                        string.Format(
                                            "<a href=\"{0}://{1}/Jobseeker/Index\" target=\"_Blank\">{2}</a> | <a href=\"{0}://{1}/Jobseeker/Index\" target=\"_Blank\">View</a>",
                                            Request.Url.Scheme, Request.Url.Authority, job_list.Count);
                                    resumeLink = string.Format("<a href=\"{0}://{1}/{2}\" target=\"_Blank\">{3}</a>",
                                        Request.Url.Scheme, Request.Url.Authority, original.PermaLink, original.Title);

                                    sbbody.Append("<tr>");
                                    sbbody.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td>", resumeLink,
                                        original.DateUpdated.Value.ToString("MM/dd/yyyy"), automatchLink);
                                    sbbody.Append("</tr>");
                                    sbbody.Append("</table>");

                                    var reader = new StreamReader(Server.MapPath("~/Templates/Mail/jobseeker_automatch.html"));
                                    if (reader != null)
                                    {
                                        string body = reader.ReadToEnd();
                                        body = body.Replace("@@firstname", original.FirstName);
                                        body = body.Replace("@@lastname", original.LastName);
                                        body = body.Replace("@@list", sbbody.ToString());
                                        string[] receipent = { original.Username };
                                        var subject = "Job Match List";

                                        AlertService.Instance.SendMail(subject, receipent, body);
                                    }

                                    jobList = job_list.ToList();
                                    Action sendAction = new Action(Send);
                                    Task sendTask = new Task(sendAction, TaskCreationOptions.None);
                                    sendTask.Start();
                                }
                            }
                            if (!string.IsNullOrEmpty(model.ReturnUrl))
                            {
                                return Redirect(model.ReturnUrl);
                            }
                        }
                        else
                        {
                            original.Title = model.Title;
                            original.CategoryId = model.CategoryId;
                            original.SpecializationId = model.SpecializationId;
                            MemberService.Instance.Update(original);
                            if (!string.IsNullOrEmpty(model.ReturnUrl))
                            {
                                return Redirect(model.ReturnUrl);
                            }
                        }
                    }
                    else
                    {
                        original.Title = model.Title;
                        original.CategoryId = model.CategoryId;
                        original.SpecializationId = model.SpecializationId;
                        MemberService.Instance.Update(original);
                        if (!string.IsNullOrEmpty(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                    }
                }
            }
            return View(model);
        }
        /// <summary>
        /// Updates the Jobseeker's Profile.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [UrlPrivilegeFilter]
        public ActionResult UpdateProfile1(string type = null, string returnUrl = null)
        {
            long id;
            LocationModel location1 = new LocationModel();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string url = "https://ipinfo.io/json?token=18061b6ccf594f";
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                location1 = new JavaScriptSerializer().Deserialize<LocationModel>(json);
            }
            string cn = ""; //location1.Country;
            string city = ""; //location1.City;
            string ip = ""; //location1.IP;
            string re = location1.region;
            int i = 1;
            var jobss = JobService.Instance.GetLatestJobs22(cn, re, city, i).Take(5);

            ViewBag.Id = 180954;
            //ViewBag.LatestJob1 = result;
            var result = jobss.Count();
            ViewBag.result = result;
            if (result >= 1)
            {
                ViewBag.RecommendedJobs1 = jobss;
            }
            else if (result < 1)
            {
                ViewBag.Text = "For Recommended Jobs Create Resume";
            }
            var model = new JobseekerProfileModel();
            var jobSeeker = MemberService.Instance.Get(User.Username);
            model.Type = type;
            model.ReturnUrl = returnUrl;

            model.Id = jobSeeker.UserId;
            model.FirstName = jobSeeker.FirstName;
            model.LastName = jobSeeker.LastName;
            model.Address = jobSeeker.Address;
            model.Zip = jobSeeker.Zip;
            model.PhoneCountryCode = jobSeeker.PhoneCountryCode;
            model.Phone = jobSeeker.Phone;
            model.MobileCountryCode = jobSeeker.MobileCountryCode;
            model.Mobile = jobSeeker.Mobile;
            model.CountryId = jobSeeker.CountryId;
            model.StateId = jobSeeker.StateId;
            model.City = jobSeeker.City;
            if (jobSeeker.DateOfBirth == null)
            {

            }
            else if (jobSeeker.DateOfBirth.Length == 4)
            {
                model.BYear = jobSeeker.DateOfBirth;
            }
            else
            {
                string[] dob = jobSeeker.DateOfBirth.Split('-');
                model.BYear = dob[0];
                model.BMonth = dob[1];
                model.BDay = dob[2];
            }
            model.PremaLink = jobSeeker.PermaLink;
            model.School = jobSeeker.School;
            model.University = jobSeeker.University;
            model.Title = jobSeeker.Title;
            model.CategoryId = jobSeeker.CategoryId;
            model.SpecializationId = jobSeeker.SpecializationId;
            model.CurrentEmployer = jobSeeker.CurrentEmployer;
            model.PreviousEmployer = jobSeeker.PreviousEmployer;
            model.Experience = jobSeeker.Experience;
            model.CurrentCurrency = jobSeeker.CurrentCurrency;
            model.ExpectedCurrency = jobSeeker.ExpectedCurrency;
            model.CurrentSalary = jobSeeker.DrawingSalary;
            model.ExpectedSalary = jobSeeker.ExpectedSalary;
            model.QualificationId = jobSeeker.QualificationId;
            model.AreaOfExpertise = jobSeeker.AreaOfExpertise;
            model.TechnicalSkills = jobSeeker.TechnicalSkills;
            model.ManagementSkills = jobSeeker.ManagementSkills;
            model.Summary = jobSeeker.Summary;
            model.Education = jobSeeker.Education;
            model.ProfessionalExperience = jobSeeker.ProfessionalExperience;
            model.ProfessionalCertification = jobSeeker.Certifications;
            model.ProfessionalAffiliation = jobSeeker.Affiliations;
            model.Gender = jobSeeker.Gender;
            model.RelationshipStatus = jobSeeker.RelationshipStatus;
            model.Website = jobSeeker.Website;
            model.Facebook = jobSeeker.Facebook;
            model.Twitter = jobSeeker.Twitter;
            model.LinkedIn = jobSeeker.LinkedIn;
            model.GooglePlus = jobSeeker.GooglePlus;
            model.PreviousEmployerFromMonth = jobSeeker.PreviousEmployerFromMonth;
            model.PreviousEmployerFromYear = jobSeeker.PreviousEmployerFromYear;
            model.PreviousEmployerToMonth = jobSeeker.PreviousEmployerToMonth;
            model.PreviousEmployerToYear = jobSeeker.PreviousEmployerToYear;

            model.CurrentEmployerFromMonth = jobSeeker.CurrentEmployerFromMonth;
            model.CurrentEmployerFromYear = jobSeeker.CurrentEmployerFromYear;
            model.CurrentEmployerToMonth = jobSeeker.CurrentEmployerToMonth;
            model.CurrentEmployerToYear = jobSeeker.CurrentEmployerToYear;
            return View(model);
        }
        /// <returns></returns>


        /// <summary>
        /// Updates the Jobseeker's Profile.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UpdateProfile(string type = null, string returnUrl = null)
        {
            var model = new JobseekerProfileModel();
            var jobSeeker = MemberService.Instance.Get(User.Username);
            model.Type = type;
            model.ReturnUrl = returnUrl;

            model.Id = jobSeeker.UserId;
            model.FirstName = jobSeeker.FirstName;
            model.LastName = jobSeeker.LastName;
            model.Address = jobSeeker.Address;
            model.Zip = jobSeeker.Zip;
            model.PhoneCountryCode = jobSeeker.PhoneCountryCode;
            model.Phone = jobSeeker.Phone;
            model.MobileCountryCode = jobSeeker.MobileCountryCode;
            model.Mobile = jobSeeker.Mobile;
            model.CountryId = jobSeeker.CountryId;
            model.StateId = jobSeeker.StateId;
            model.City = jobSeeker.City;
            if (jobSeeker.DateOfBirth == null)
            {

            }
            else if (jobSeeker.DateOfBirth.Length == 4)
            {
                model.BYear = jobSeeker.DateOfBirth;
            }
            else
            {
                string[] dob = jobSeeker.DateOfBirth.Split('-');
                model.BYear = dob[0];
                model.BMonth = dob[1];
                model.BDay = dob[2];
            }
            model.PremaLink = jobSeeker.PermaLink;
            model.School = jobSeeker.School;
            model.University = jobSeeker.University;
            model.Title = jobSeeker.Title;
            model.CategoryId = jobSeeker.CategoryId;
            model.SpecializationId = jobSeeker.SpecializationId;
            model.CurrentEmployer = jobSeeker.CurrentEmployer;
            model.PreviousEmployer = jobSeeker.PreviousEmployer;
            model.Experience = jobSeeker.Experience;
            model.CurrentCurrency = jobSeeker.CurrentCurrency;
            model.ExpectedCurrency = jobSeeker.ExpectedCurrency;
            model.CurrentSalary = jobSeeker.DrawingSalary;
            model.ExpectedSalary = jobSeeker.ExpectedSalary;
            model.QualificationId = jobSeeker.QualificationId;
            model.AreaOfExpertise = jobSeeker.AreaOfExpertise;
            model.TechnicalSkills = jobSeeker.TechnicalSkills;
            model.ManagementSkills = jobSeeker.ManagementSkills;
            model.Summary = jobSeeker.Summary;
            model.Education = jobSeeker.Education;
            model.ProfessionalExperience = jobSeeker.ProfessionalExperience;
            model.ProfessionalCertification = jobSeeker.Certifications;
            model.ProfessionalAffiliation = jobSeeker.Affiliations;
            model.Gender = jobSeeker.Gender;
            model.RelationshipStatus = jobSeeker.RelationshipStatus;
            model.Website = jobSeeker.Website;
            model.Facebook = jobSeeker.Facebook;
            model.Twitter = jobSeeker.Twitter;
            model.LinkedIn = jobSeeker.LinkedIn;
            model.GooglePlus = jobSeeker.GooglePlus;
            model.PreviousEmployerFromMonth = jobSeeker.PreviousEmployerFromMonth;
            model.PreviousEmployerFromYear = jobSeeker.PreviousEmployerFromYear;
            model.PreviousEmployerToMonth = jobSeeker.PreviousEmployerToMonth;
            model.PreviousEmployerToYear = jobSeeker.PreviousEmployerToYear;

            model.CurrentEmployerFromMonth = jobSeeker.CurrentEmployerFromMonth;
            model.CurrentEmployerFromYear = jobSeeker.CurrentEmployerFromYear;
            model.CurrentEmployerToMonth = jobSeeker.CurrentEmployerToMonth;
            model.CurrentEmployerToYear = jobSeeker.CurrentEmployerToYear;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadResume(JobseekerProfileModel model, HttpPostedFileBase Resume)
        {
            var original = MemberService.Instance.Get(model.Id);
            var fileSize = Convert.ToInt32(ConfigService.Instance.GetConfigValue("LogoSize"));
            var actualFileSize = fileSize * 1024;

            if (Resume != null && Resume.ContentLength > actualFileSize)
            {
                TempData["Error"] = string.Format("Your resume size exceeds the upload limit({0} KB).", fileSize);
                return RedirectToAction("UpdateProfile");
            }

            if (Resume != null)
            {
                var extension =
                    Resume.FileName.Substring(Resume.FileName.LastIndexOf(".") + 1).ToUpper();

                if (extension == "PDF" || extension == "DOC" || extension == "DOCX")
                {
                    var file = Resume.InputStream;
                    var buffer = new byte[file.Length];
                    file.Read(buffer, 0, (int)file.Length);
                    file.Close();

                    original.Content = Convert.ToBase64String(buffer);
                    original.FileName = Resume.FileName;
                    original.IsBuild = false;
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        dataHelper.Update<UserProfile>(original, User.Username);
                    }
                    TempData["SaveData"] = "Resume uploaded successfully!";
                }
                else
                {
                    TempData["Error"] = "Only .doc, .docx, .pdf files are allowed!";
                }
            }
            return Redirect("/Jobseeker/UpdateProfile1");
        }

        public ActionResult DeleteImage(string Username, string Reason = "")
        {
            string message = string.Empty;
            var original = _service.Get(Username);
            long? id = MemberService.Instance.GetPhotoId("Profile", Username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                if (id != null)
                {
                    Photo photo = dataHelper.GetSingle<Photo>(id.Value);
                    dataHelper.Remove<Photo>(photo);
                }
            }

            if (!string.IsNullOrEmpty(Reason))
            {
                Activity activity = new Activity()
                {
                    Comments = Reason,
                    ActivityDate = DateTime.Now,
                    UserId = original.Id,
                    DateUpdated = DateTime.Now,
                    UpdatedBy = User.Username,
                    Type = (int)ActivityTypes.PHOTO_DELETED,
                    Unread = true
                };
                MemberService.Instance.Track(activity);
            }
            TempData["SaveData"] = "Photo deleted successfully!";
            return Redirect("/Jobseeker/UpdateProfile1");
        }

        public ActionResult DeletePhoto(JobseekerProfileModel model)
        {
            var original = MemberService.Instance.Get(model.Id);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                dataHelper.Update<UserProfile>(original, User.Username);
            }
            TempData["SaveData"] = "Photo deleted successfully!";
            return Redirect("/Jobseeker/UpdateProfile1");
        }
        [Authorize]
        [HttpPost]
        public ActionResult UpdateProfile1(JobseekerProfileModel model)
        {
            long id;
            LocationModel location1 = new LocationModel();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string url = "https://ipinfo.io/json?token=18061b6ccf594f";
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                location1 = new JavaScriptSerializer().Deserialize<LocationModel>(json);
            }
            string cn = ""; //location1.Country;
            string city = ""; //location1.City;
            string ip = ""; //location1.IP;
            string re = location1.region;
            int i = 1;
           
            var body = string.Empty;
            UserProfile original = new UserProfile();
            original = MemberService.Instance.Get(model.Id);

            body = string.Empty;
            if (!string.IsNullOrEmpty(model.BYear))
            {
                if (!string.IsNullOrEmpty(model.BDay) && !string.IsNullOrEmpty(model.BMonth))
                {
                    model.BirthDate = string.Format("{0}-{1}-{2}", model.BYear, model.BMonth, model.BDay);

                    int year = Convert.ToInt32(model.BYear);
                    model.Age = DateTime.Now.Year - year;
                    int limit = 13;
                    if (model.Age <= limit)
                    {
                        TempData["Error"] = "You must be 13 years of age!";
                        return View(model);
                    }
                }
                else
                {
                    model.BirthDate = model.BYear;
                    int year = Convert.ToInt32(model.BYear);

                    model.Age = DateTime.Now.Year - year;
                    int limit = 13;
                    if (model.Age <= limit)
                    {
                        TempData["Error"] = "You must be 13 years of age!";
                        return View(model);
                    }
                }
            }
            else
            {
                TempData["Error"] = "Please provide birth year!";
                return View(model);
            }
            if (!string.IsNullOrEmpty(model.FirstName))
            {
                if (original != null && original.IsConfirmed == false)
                {
                    //return RedirectToAction("ConfirmRegistration", "Account");
                    return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
                }


                if (!string.IsNullOrEmpty(model.Title))
                {
                    if ((string.IsNullOrEmpty(original.Title)) || (original.CountryId != null && original.CountryId != model.CountryId))
                    {
                        Update(original, model);
                        if (original.IsConfirmed == true)
                        {
                            var job_list = JobSeekerService.Instance.AutomatchedJobs(original);

                            if (job_list.Any())
                            {
                                var sbbody = new StringBuilder();
                                var resumeLink = string.Empty;

                                sbbody.Append("<table>");
                                sbbody.Append("<tr>");
                                sbbody.Append("<th>Title</th><th>Last Modified Date</th><th>Number of Matches</th>");
                                sbbody.Append("</tr>");

                                var automatchLink =
                                    string.Format(
                                        "<a href=\"{0}://{1}/Jobseeker/Index\" target=\"_Blank\">{2}</a> | <a href=\"{0}://{1}/Jobseeker/Index\" target=\"_Blank\">View</a>",
                                        Request.Url.Scheme, Request.Url.Authority, job_list.Count);
                                resumeLink = string.Format("<a href=\"{0}://{1}/{2}\" target=\"_Blank\">{3}</a>",
                                    Request.Url.Scheme, Request.Url.Authority, original.PermaLink, original.Title);

                                sbbody.Append("<tr>");
                                sbbody.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td>", resumeLink,
                                    original.DateUpdated.Value.ToString("MM/dd/yyyy"), automatchLink);
                                sbbody.Append("</tr>");
                                sbbody.Append("</table>");

                                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/jobseeker_automatch.html"));
                                if (reader != null)
                                {
                                    body = reader.ReadToEnd();
                                    body = body.Replace("@@firstname", original.FirstName);
                                    body = body.Replace("@@lastname", original.LastName);
                                    body = body.Replace("@@list", sbbody.ToString());
                                    string[] receipent = { original.Username };
                                    var subject = "Job Match List";

                                    AlertService.Instance.SendMail(subject, receipent, body);
                                }

                                jobList = job_list.ToList();
                                Action sendAction = new Action(Send);
                                Task sendTask = new Task(sendAction, TaskCreationOptions.None);
                                sendTask.Start();
                            }
                        }
                    }
                    else
                    {
                        Update1(original, model);
                    }
                }
                else
                {
                    Update1(original, model);
                }
            }
            else
            {
                Update1(original, model);
            }

            if (!string.IsNullOrEmpty(model.ReturnUrl))
            {
                //TempData["SaveData"] = "Profile promoted successfully.";
                return Redirect(model.ReturnUrl);
            }
            else
            {
                return RedirectToAction("UpdateProfile1", "Jobseeker", new { type = model.Type, returnurl = model.ReturnUrl });
            }
           
            
            var jobss = JobService.Instance.GetLatestJobs22(cn, re, city, i).Take(5);

            ViewBag.Id = id;
            //ViewBag.LatestJob1 = result;
            var result = jobss.Count();
            ViewBag.result = result;
            if (result >= 1)
            {
                ViewBag.RecommendedJobs1 = jobss;
            }
            else if (result < 1)
            {
                ViewBag.Text = "For Recommended Jobs Create Resume";
            }
        }
        private void Update1(UserProfile original, JobseekerProfileModel model)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                //if (ModelState.IsValid)
                //{
                original.CountryId = model.CountryId;
                original.StateId = model.StateId;
                original.City = model.City;
                if (!string.IsNullOrEmpty(model.FirstName))
                {
                    original.FirstName = model.FirstName.TitleCase();
                }
                if (!string.IsNullOrEmpty(original.LastName))
                {
                    original.LastName = model.LastName.TitleCase();
                }
                original.DateOfBirth = model.BirthDate;
                original.Address = model.Address;
                original.Zip = model.Zip;
                original.PhoneCountryCode = model.PhoneCountryCode;
                original.Phone = model.Phone;
                original.MobileCountryCode = model.MobileCountryCode;
                original.Mobile = model.Mobile;
                original.DateOfBirth = model.BirthDate;
                if (!string.IsNullOrEmpty(model.BirthDate))
                {
                    original.Age = Convert.ToByte(model.Age);
                }
                original.Gender = model.Gender;
                original.RelationshipStatus = model.RelationshipStatus;
                original.Facebook = model.Facebook;
                original.Twitter = model.Twitter;
                original.LinkedIn = model.LinkedIn;
                original.GooglePlus = model.GooglePlus;
                original.Summary = model.Summary;
                original.CategoryId = model.CategoryId;
                original.SpecializationId = model.SpecializationId;

                original.CurrentEmployer = model.CurrentEmployer;
                original.CurrentEmployerFromMonth = model.CurrentEmployerFromMonth;
                original.CurrentEmployerFromYear = model.CurrentEmployerFromYear;
                original.CurrentEmployerToMonth = model.CurrentEmployerToMonth;
                original.CurrentEmployerToYear = model.CurrentEmployerToYear;

                original.PreviousEmployer = model.PreviousEmployer;
                original.PreviousEmployerFromMonth = model.PreviousEmployerFromMonth;
                original.PreviousEmployerFromYear = model.PreviousEmployerFromYear;
                original.PreviousEmployerToMonth = model.PreviousEmployerToMonth;
                original.PreviousEmployerToYear = model.PreviousEmployerToYear;

                original.Experience = (byte?)model.Experience;

                original.DrawingSalary = model.CurrentSalary;
                original.ExpectedSalary = model.ExpectedSalary;
                original.CurrentCurrency = model.CurrentCurrency;
                original.ExpectedCurrency = model.ExpectedCurrency;
                original.School = model.School;
                original.University = model.University;
                original.QualificationId = model.QualificationId;
                original.AreaOfExpertise = model.AreaOfExpertise;
                original.TechnicalSkills = model.TechnicalSkills;
                original.ManagementSkills = model.ManagementSkills;
                original.ProfessionalExperience = model.ProfessionalExperience;
                original.Education = model.Education;
                original.Certifications = model.ProfessionalCertification;
                original.Affiliations = model.ProfessionalAffiliation;
                original.Website = model.Website;

                dataHelper.Update(original, "Self");
                TempData["SaveData"] = "Profile updated successfully.";
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateProfile(JobseekerProfileModel model)
        {
            var body = string.Empty;
            UserProfile original = new UserProfile();
            original = MemberService.Instance.Get(model.Id);

            body = string.Empty;
            if (!string.IsNullOrEmpty(model.BYear))
            {
                if (!string.IsNullOrEmpty(model.BDay) && !string.IsNullOrEmpty(model.BMonth))
                {
                    model.BirthDate = string.Format("{0}-{1}-{2}", model.BYear, model.BMonth, model.BDay);

                    int year = Convert.ToInt32(model.BYear);
                    model.Age = DateTime.Now.Year - year;
                    int limit = 13;
                    if (model.Age <= limit)
                    {
                        TempData["Error"] = "You must be 13 years of age!";
                        return View(model);
                    }
                }
                else
                {
                    model.BirthDate = model.BYear;
                    int year = Convert.ToInt32(model.BYear);

                    model.Age = DateTime.Now.Year - year;
                    int limit = 13;
                    if (model.Age <= limit)
                    {
                        TempData["Error"] = "You must be 13 years of age!";
                        return View(model);
                    }
                }
            }
            else
            {
                TempData["Error"] = "Please provide birth year!";
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.Title) && model.CategoryId != null && model.SpecializationId != null)
            {
                if (original != null && original.IsConfirmed == false)
                {
                    //return RedirectToAction("ConfirmRegistration", "Account");
                    return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
                }

                if (!string.IsNullOrEmpty(model.Title) && string.IsNullOrEmpty(original.Content))
                {
                    TempData["Error"] = "Please upload or build your resume online!";
                    return View(model);
                }
                if (!string.IsNullOrEmpty(model.Title))
                {
                    if ((string.IsNullOrEmpty(original.Title) || (!string.IsNullOrEmpty(original.Title) && !original.Title.Equals(model.Title))) || (original.CategoryId != null && original.CategoryId != model.CategoryId) || (original.SpecializationId != null && original.SpecializationId != model.SpecializationId) || (original.CountryId != null && original.CountryId != model.CountryId))
                    {
                        Update(original, model);
                        if (original.IsConfirmed == true)
                        {
                            var job_list = JobSeekerService.Instance.AutomatchedJobs(original);

                            if (job_list.Any())
                            {
                                var sbbody = new StringBuilder();
                                var resumeLink = string.Empty;

                                sbbody.Append("<table>");
                                sbbody.Append("<tr>");
                                sbbody.Append("<th>Title</th><th>Last Modified Date</th><th>Number of Matches</th>");
                                sbbody.Append("</tr>");

                                var automatchLink =
                                    string.Format(
                                        "<a href=\"{0}://{1}/Jobseeker/Index\" target=\"_Blank\">{2}</a> | <a href=\"{0}://{1}/Jobseeker/Index\" target=\"_Blank\">View</a>",
                                        Request.Url.Scheme, Request.Url.Authority, job_list.Count);
                                resumeLink = string.Format("<a href=\"{0}://{1}/{2}\" target=\"_Blank\">{3}</a>",
                                    Request.Url.Scheme, Request.Url.Authority, original.PermaLink, original.Title);

                                sbbody.Append("<tr>");
                                sbbody.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td>", resumeLink,
                                    original.DateUpdated.Value.ToString("MM/dd/yyyy"), automatchLink);
                                sbbody.Append("</tr>");
                                sbbody.Append("</table>");

                                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/jobseeker_automatch.html"));
                                if (reader != null)
                                {
                                    body = reader.ReadToEnd();
                                    body = body.Replace("@@firstname", original.FirstName);
                                    body = body.Replace("@@lastname", original.LastName);
                                    body = body.Replace("@@list", sbbody.ToString());
                                    string[] receipent = { original.Username };
                                    var subject = "Job Match List";

                                    AlertService.Instance.SendMail(subject, receipent, body);
                                }

                                jobList = job_list.ToList();
                                Action sendAction = new Action(Send);
                                Task sendTask = new Task(sendAction, TaskCreationOptions.None);
                                sendTask.Start();
                            }
                        }
                    }
                    else
                    {
                        Update(original, model);
                    }
                }
                else
                {
                    Update(original, model);
                }
            }
            else
            {
                Update(original, model);
            }

            if (!string.IsNullOrEmpty(model.ReturnUrl))
            {
                //TempData["SaveData"] = "Profile promoted successfully.";
                return Redirect(model.ReturnUrl);
            }
            else
            {
                return RedirectToAction("UpdateProfile", "Jobseeker", new { type = model.Type, returnurl = model.ReturnUrl });
            }
        }

        private void Update(UserProfile original, JobseekerProfileModel model)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                //if (ModelState.IsValid)
                //{
                original.CountryId = model.CountryId;
                original.StateId = model.StateId;
                original.City = model.City;
                if (!string.IsNullOrEmpty(model.FirstName))
                {
                    original.FirstName = model.FirstName.TitleCase();
                }
                if (!string.IsNullOrEmpty(original.LastName))
                {
                    original.LastName = model.LastName.TitleCase();
                }
                original.DateOfBirth = model.BirthDate;
                original.Address = model.Address;
                original.Zip = model.Zip;
                original.PhoneCountryCode = model.PhoneCountryCode;
                original.Phone = model.Phone;
                original.MobileCountryCode = model.MobileCountryCode;
                original.Mobile = model.Mobile;
                original.DateOfBirth = model.BirthDate;
                if (!string.IsNullOrEmpty(model.BirthDate))
                {
                    original.Age = Convert.ToByte(model.Age);
                }
                original.Gender = model.Gender;
                original.RelationshipStatus = model.RelationshipStatus;
                original.Facebook = model.Facebook;
                original.Twitter = model.Twitter;
                original.LinkedIn = model.LinkedIn;
                original.GooglePlus = model.GooglePlus;
                if (!string.IsNullOrEmpty(model.Title))
                {
                    original.Title = model.Title.TitleCase();
                }
                else
                {
                    original.Title = null;
                }
                original.Summary = model.Summary;
                original.CategoryId = model.CategoryId;
                original.SpecializationId = model.SpecializationId;

                original.CurrentEmployer = model.CurrentEmployer;
                original.CurrentEmployerFromMonth = model.CurrentEmployerFromMonth;
                original.CurrentEmployerFromYear = model.CurrentEmployerFromYear;
                original.CurrentEmployerToMonth = model.CurrentEmployerToMonth;
                original.CurrentEmployerToYear = model.CurrentEmployerToYear;

                original.PreviousEmployer = model.PreviousEmployer;
                original.PreviousEmployerFromMonth = model.PreviousEmployerFromMonth;
                original.PreviousEmployerFromYear = model.PreviousEmployerFromYear;
                original.PreviousEmployerToMonth = model.PreviousEmployerToMonth;
                original.PreviousEmployerToYear = model.PreviousEmployerToYear;

                original.Experience = (byte?)model.Experience;

                original.DrawingSalary = model.CurrentSalary;
                original.ExpectedSalary = model.ExpectedSalary;
                original.CurrentCurrency = model.CurrentCurrency;
                original.ExpectedCurrency = model.ExpectedCurrency;
                original.School = model.School;
                original.University = model.University;
                original.QualificationId = model.QualificationId;
                original.AreaOfExpertise = model.AreaOfExpertise;
                original.TechnicalSkills = model.TechnicalSkills;
                original.ManagementSkills = model.ManagementSkills;
                original.ProfessionalExperience = model.ProfessionalExperience;
                original.Education = model.Education;
                original.Certifications = model.ProfessionalCertification;
                original.Affiliations = model.ProfessionalAffiliation;
                original.Website = model.Website;

                dataHelper.Update(original, "Self");
                TempData["SaveData"] = "Profile updated successfully.";
            }
        }

        public void Send()
        {
            UserProfile profile = MemberService.Instance.Get(User.Username);
            string body = string.Empty;
            foreach (AutomatchJob item in jobList)
            {
                Job job = JobService.Instance.Get(item.Job.Id);
                UserProfile employer = MemberService.Instance.Get(job.EmployerId.Value);

                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/employer_jobseeker_automatch.html"));
                if (reader != null)
                {
                    body = reader.ReadToEnd();
                    body = body.Replace("@@employer", !string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName));
                    body = body.Replace("@@firstname", profile.FirstName);
                    body = body.Replace("@@lastname", profile.LastName);
                    body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, profile.PermaLink));

                    body = body.Replace("@@jobtitle", job.Title);
                    body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));

                    string[] receipent = { employer.Username };
                    var subject = string.Format("Profile Matched for {0}", job.Title);

                    AlertService.Instance.SendMail(subject, receipent, body);
                }
            }
        }

        [Authorize]
        [HttpGet]
        [UrlPrivilegeFilter]
        public ActionResult BookMarkedJobs()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'JobBookmarkFilter' could not be found (are you missing a using directive or an assembly reference?)
        public  ActionResult BookMarkedJobs(JobBookmarkFilter filter)
#pragma warning restore CS0246 // The type or namespace name 'JobBookmarkFilter' could not be found (are you missing a using directive or an assembly reference?)
        {
            filter.UserId = User.Id;
            //List<JobBookmarked> list = await _service.JobBookmarkedList(filter);
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> BookmarkedCompanies()
        {
            List<JobBookmarked> list = await _service.JobsBookmarkedCompanyList(User.Id);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remove(Guid id)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var bookmakred = dataHelper.GetSingle<Tracking>(id);
                if (bookmakred != null)
                {
                    dataHelper.Delete<Tracking>(bookmakred, User.Username);
                    TempData["DeleteData"] = "Bookmark removed!";
                }
            }
            return RedirectToAction("BookMarkedJobs", "JobSeeker");
        }

        [Authorize]
        public ActionResult DownloadResume(long id, string redirect)
        {
            List<int> typeList = new List<int>() { (int)SecurityRoles.SuperUser, (int)SecurityRoles.Administrator };
            if ((UserInfo != null && !UserInfo.IsConfirmed) && !typeList.Contains(UserInfo.Type))
            {
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
            }

            if (UserInfo.Type == 5)
            {
                if (DomainService.Instance.HasDownloadQuota(UserInfo.Id) == false)
                {
                    return RedirectToAction("Download", "Jobseeker", new { id = id, redirect = redirect });
                }
            }
            else
            {
                //TempData["Error"] = "Only Company account can download resume!<br/><a href='/account/logout'>CLICK HERE</a> to login or register company account.";
                TempData["Error"] = string.Format("Only Company account can download resume!<br/><a href='/account/logout?ReturnUrl={0}'>CLICK HERE</a> to login or register company account.", Request.Url);
                return Redirect(redirect);
            }

            ViewBag.Id = id;
            ViewBag.ReturnUrl = redirect;

            return View();
        }
        [Authorize]
        public ActionResult Download(long id, string redirect, Guid? trackingId = null)
        {
            List<int> typeList = new List<int>() { (int)SecurityRoles.SuperUser, (int)SecurityRoles.Administrator };
            if ((UserInfo != null && !UserInfo.IsConfirmed) && !typeList.Contains(UserInfo.Type))
            {
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
                //return RedirectToAction("ConfirmRegistration", "Account");
            }

            if (UserInfo.Type == 5)
            {
                //if (DomainService.Instance.HasDownloadQuota(UserInfo.Id) == false)
                //{
                //    if (!DomainService.Instance.IsPaidResume(UserInfo.Id, id))
                //    {
                //        UserInfoEntity uinfo = _service.Get(id);
                //        return RedirectToAction("ResumePriceList", "Package", new { id = uinfo.Id, returnurl = Request.Url.ToString(), type = "R", countryId = User.Info.CountryId });
                //    }
                //}
            }

            Resume resume = new Resume();
            Tracking tracking = null;
            if (User != null)
            {
                UserProfile profile = MemberService.Instance.Get(User.Username);
                if (profile != null && profile.Type == (int)SecurityRoles.Employers)
                {
                    UserProfile jobSeeker = new UserProfile();
                    string message = string.Empty;
                    if (trackingId != null)
                    {
                        tracking = TrackingService.Instance.Get(trackingId.Value);
                        if (tracking != null)
                        {
                            tracking.IsDownloaded = true;
                            using (JobPortalEntities context = new JobPortalEntities())
                            {
                                DataHelper dataHelper = new DataHelper(context);
                                dataHelper.Update<Tracking>(tracking);
                            }
                        }
                    }

                    jobSeeker = MemberService.Instance.Get(id);
                    if (jobSeeker != null)
                    {
                        string content = string.Empty;
                        if (jobSeeker.IsBuild)
                        {
                            UserProfile original = MemberService.Instance.Get(jobSeeker.UserId);
                            byte[] buffer = Pdf(original.UserId);
                            original.Content = Convert.ToBase64String(buffer);
                            original.FileName = string.Format("{0}_{1}.pdf", original.FirstName, original.LastName);
                            original.IsBuild = true;
                            MemberService.Instance.Update(original);
                            original = MemberService.Instance.Get(jobSeeker.UserId);
                            content = original.Content;
                        }
                        else
                        {
                            content = jobSeeker.Content;
                        }

                        if (!string.IsNullOrEmpty(content))
                        {
                            TrackingService.Instance.Downloaded(id, User.Username, out message);
                            if (UserInfo.Type == 5)
                            {
                                long? jobid = (tracking != null ? tracking.JobId : null);
                                _service.ManageAccount(User.Id, null, null, null, 1, null, jobSeeker.UserId, jobid);
                            }
                            return File(Convert.FromBase64String(content), MediaTypeNames.Application.Octet, jobSeeker.FileName);
                        }
                    }
                }
                else if (profile != null && (profile.Type == (int)SecurityRoles.Administrator || profile.Type == (int)SecurityRoles.SuperUser))
                {
                    UserProfile jobSeeker = MemberService.Instance.Get(id);
                    if (jobSeeker != null)
                    {
                        string content = string.Empty;
                        if (jobSeeker.IsBuild)
                        {
                            UserProfile original = MemberService.Instance.Get(jobSeeker.UserId);
                            byte[] buffer = Pdf(original.UserId);
                            original.Content = Convert.ToBase64String(buffer);
                            original.FileName = string.Format("{0}_{1}.pdf", original.FirstName, original.LastName);
                            original.IsBuild = true;
                            content = original.Content;
                            MemberService.Instance.Update(original);
                        }
                        else
                        {
                            content = jobSeeker.Content;
                        }

                        if (!string.IsNullOrEmpty(content))
                        {
                            return File(Convert.FromBase64String(content), MediaTypeNames.Application.Octet, jobSeeker.FileName);
                        }
                    }
                }
                else if (profile != null && profile.Type == (int)SecurityRoles.Jobseeker)
                {
                    UserProfile jobSeeker = MemberService.Instance.Get(id);
                    if (jobSeeker != null && jobSeeker.Username.Equals(profile.Username))
                    {

                        string content = string.Empty;
                        if (jobSeeker.IsBuild)
                        {
                            UserProfile original = MemberService.Instance.Get(jobSeeker.UserId);
                            byte[] buffer = Pdf(original.UserId);
                            original.Content = Convert.ToBase64String(buffer);
                            original.FileName = string.Format("{0}_{1}.pdf", original.FirstName, original.LastName);
                            original.IsBuild = true;
                            content = original.Content;
                            MemberService.Instance.Update(original);
                        }
                        else
                        {
                            content = jobSeeker.Content;
                        }
                        if (!string.IsNullOrEmpty(content))
                        {
                            return File(Convert.FromBase64String(content), MediaTypeNames.Application.Octet, jobSeeker.FileName);
                        }
                    }
                    else
                    {
                        //TempData["Error"] = "Only Company account can download resume!<br/><a href='/account/logout'>CLICK HERE</a> to login or register company account.";
                        TempData["Error"] = string.Format("Only Company account can download resume!<br/><a href='/account/logout?ReturnUrl={0}'>CLICK HERE</a> to login or register company account.", Request.Url);
                    }
                }
                else
                {
                    TempData["Error"] = string.Format("Only Company account can download resume!<br/><a href='/account/logout?ReturnUrl={0}'>CLICK HERE</a> to login or register company account.", Request.Url);
                    //TempData["Error"] = "Only Company account can download resume!<br/><a href='/account/logout>CLICK HERE</a> to login or register company account.";
                }
            }
            return Redirect(redirect);
        }

        /// <summary>
        /// Withdraw application
        /// </summary>
        /// <param name="JobId"></param>
        /// <param name="ResumeId"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Withdraw(Guid Id)
        {
            if (User != null)
            {
                var employer = MemberService.Instance.Get(User.Username);
                var message = string.Empty;
                JobSeekerService.Instance.Withdraw(Id, User.Username, out message);

                TempData["UpdateData"] = message;
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Delete bookmark
        /// </summary>
        /// <param name="JobId"></param>
        /// <param name="ResumeId"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(Guid Id)
        {
            if (User != null)
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    var record = dataHelper.GetSingle<Tracking>(Id);
                    if (record != null)
                    {
                        dataHelper.Delete(record);
                    }
                    TempData["UpdateData"] = "Deleted successfully!";
                }
            }
            return RedirectToAction("Index");
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        [ValidateInput(false)]
        [AllowAnonymous]
        [UrlPrivilegeFilter]
        public async Task<ActionResult> Search(SearchJobModel model,int pageNumber = 1)
        {
            //SearchJobModel model = new SearchJobModel();
            var data = JobService.Instance.GetJobSeekers22(pageNumber);
            //string base64String = Convert.ToBase64String(data.Select(m=>m.Image), 0, (data.Select(m => m.Image)).le);
            //data.Select
            //Convert.FromBase64String(data.Select(m => m.Image)); 
            if (model.JobTitle == null)
            {
                //ViewBag.cn = countryNam;
                ViewBag.LatestJobs = data;    
            }
            else
            {
                string strModified = model.JobTitle.Substring(0, 2);
                //var ff = data.Where(m => m.Title == model.JobTitle).ToList();
                var ff1 = data.Where(m => m.Title.StartsWith(strModified)).ToList();
              //  ViewBag.cn = countryNam;
                ViewBag.LatestJobs = ff1;
            }

           // ViewBag.LatestJobs = data;
            var pageModel = new Pager(data.FirstOrDefault().TotalRow, pageNumber, 20);
            model.DataSize = pageModel.PageSize;
           // model.Where = country; //changes
            model.CurrentPage = pageModel.CurrentPage;
            model.EndPage = pageModel.EndPage;
            model.StartPage = pageModel.StartPage;
            model.CurrentPage = pageModel.CurrentPage;
            model.TotalPages = pageModel.TotalPages;
            model.PageSize = pageModel.PageSize;
            return View(model);
            //if (Request.QueryString["returnurl"] != null)
            //{
            //    ViewBag.ReturnUrl = Request["returnurl"];
            //}
            //var records = 0;
            //int pageSize = 10;
            //model.ModelList = new List<PeopleEntity>();
            //if (model == null)
            //{
            //    model = new SearchJobSeekerModel();
            //}
            //else
            //{
            //    if ((!string.IsNullOrEmpty(model.Where) && model.Where.Trim().Length > 0) || (!string.IsNullOrEmpty(model.Title) && model.Title.Trim().Length > 0) || model.CategoryId != null || model.SpecializationId != null || model.CountryId != null || model.StateId != null
            //        || (!string.IsNullOrEmpty(model.City) && model.City.Trim().Length > 0) || model.AgeMin != null || model.AgeMax != null || model.Experience != null || model.QualificationId != null
            //        || model.SalaryMin != null || model.SalaryMax != null)
            //    {
            //        var searchResume = new SearchResume
            //        {
            //            Title = (!string.IsNullOrEmpty(model.Title) && model.Title.Trim().Length > 0) ? model.Title.Trim() : null,
            //            Where = (!string.IsNullOrEmpty(model.Where) && model.Where.Trim().Length > 0) ? model.Where.Trim() : null,
            //            CategoryId = model.CategoryId,
            //            SpecializationId = model.SpecializationId,
            //            CountryId = model.CountryId,
            //            StateId = model.StateId,
            //            City = (!string.IsNullOrEmpty(model.City) && model.City.Trim().Length > 0) ? model.City.Trim() : null,
            //            AgeMin = model.AgeMin,
            //            AgeMax = model.AgeMax,
            //            Experience = model.Experience,
            //            QualificationId = model.QualificationId,
            //            MinSalary = model.SalaryMin,
            //            MaxSalary = model.SalaryMax,
            //            Username = User != null ? User.Username : null,
            //            PageNumber = pageNumber,
            //            PageSize = pageSize
            //        };
            //        model.ModelList = await _service.Jobseekers(searchResume); //SearchService.Instance.Jobseekers(searchResume);
            //        if (model.ModelList.Count() > 0)
            //        {
            //            records = model.ModelList.Max(x => x.MaxRows.Value);
            //        }
            //    }
            //    ViewBag.Model = new StaticPagedList<PeopleEntity>(model.ModelList, pageNumber, pageSize, records);
            //    ViewBag.Rows = records;
            //}

            //if (Request.IsAjaxRequest())
            //{
            //    ResponseContext context = null;
            //    try
            //    {
            //        context = new ResponseContext()
            //        {
            //            Id = 1,
            //            Data = model,
            //        };
            //    }
            //    catch (Exception ex)
            //    {
            //        context = new ResponseContext()
            //        {
            //            Id = 0,
            //            Data = model,
            //        };
            //    }
            //    return Json(context, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return View(model);
            //}

        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult PromotedJobseekers(int pageNumber = 1)
        {
            List<PeopleEntity> list = new List<PeopleEntity>();
            ResponseContext context = null;
            try
            {
                var searchResume = new SearchResume
                {
                    PageNumber = pageNumber,
                    PageSize = 2
                };

                if (User != null)
                {
                    searchResume.UserId = User.Id;
                }

                list = _service.PromotedProfiles(searchResume);

                context = new ResponseContext()
                {
                    Id = 1,
                    Data = list
                };
            }
            catch (Exception ex)
            {
                context = new ResponseContext()
                {
                    Id = 0,
                    Data = list,
                    Message = ex.Message
                };
            }

            return Json(context, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PromotedProciles(int pageNumber = 1)
        {
            List<PeopleEntity> list = new List<PeopleEntity>();
            ResponseContext context = null;
            try
            {
                var searchResume = new SearchResume
                {
                    PageNumber = pageNumber,
                    PageSize = 5
                };

                if (User != null)
                {
                    searchResume.UserId = User.Id;
                }

                list = _service.PromotedJobseekers(searchResume);

                context = new ResponseContext()
                {
                    Id = 1,
                    Data = list
                };
            }
            catch (Exception ex)
            {
                context = new ResponseContext()
                {
                    Id = 0,
                    Data = list,
                    Message = ex.Message
                };
            }
            return Json(context, JsonRequestBehavior.AllowGet);
        }
        public byte[] Pdf(long Id)
        {
            UserProfile member = MemberService.Instance.Get(Id);
            Byte[] bytes;

            //Boilerplate iTextSharp setup here
            //Create a stream that we can write to, in this case a MemoryStream
            using (var ms = new MemoryStream())
            {
                //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
                using (var doc = new Document())
                {
                    //Create a writer that's bound to our PDF abstraction and our stream
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {
                        //Open the document for writing
                        doc.Open();
                        writer.CloseStream = false;
                        StreamReader sreader = new StreamReader(Server.MapPath("~/Templates/Resume.html"));

                        //Our sample HTML and CSS
                        var resume_html = sreader.ReadToEnd(); //@"<p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p>";
                        resume_html = resume_html.Replace("@@url", string.Format("{0}://{1}/Image/Avtar?Id={2}", Request.Url.Scheme, Request.Url.Authority, member.UserId));
                        resume_html = resume_html.Replace("@@firstname", member.FirstName);
                        resume_html = resume_html.Replace("@@lastname", member.LastName);
                        resume_html = resume_html.Replace("@@title", member.Title);
                        resume_html = resume_html.Replace("@@email", member.Username);
                        resume_html = resume_html.Replace("@@address", member.Address);
                        JobPortal.Data.List country = SharedService.Instance.GetCountry(member.CountryId.Value);
                        if (!string.IsNullOrEmpty(member.Mobile))
                        {
                            if (!string.IsNullOrEmpty(member.MobileCountryCode))
                            {
                                resume_html = resume_html.Replace("@@phone", string.Format("{0} {1}", member.MobileCountryCode, member.Mobile));
                            }
                            else
                            {
                                resume_html = resume_html.Replace("@@phone", string.Format("{0}", member.Mobile));
                            }
                        }
                        else
                        {
                            resume_html = resume_html.Replace("@@phone", "");
                        }
                        resume_html = resume_html.Replace("@@pageaddr", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, member.PermaLink));
                        resume_html = resume_html.Replace("@@summary", member.Summary);

                        if (!string.IsNullOrEmpty(member.TechnicalSkills))
                        {
                            resume_html = resume_html.Replace("@@skills", member.TechnicalSkills);
                        }
                        else
                        {
                            resume_html = resume_html.Replace("@@skills", "Nil");
                        }

                        if (!string.IsNullOrEmpty(member.ManagementSkills))
                        {
                            resume_html = resume_html.Replace("@@mskills", member.ManagementSkills);
                        }
                        else
                        {
                            resume_html = resume_html.Replace("@@mskills", "Nil");
                        }

                        if (!string.IsNullOrEmpty(member.AreaOfExpertise))
                        {
                            resume_html = resume_html.Replace("@@expertise", member.AreaOfExpertise);
                        }
                        else
                        {
                            resume_html = resume_html.Replace("@@expertise", "Nil");
                        }

                        if (!string.IsNullOrEmpty(member.ProfessionalExperience))
                        {
                            resume_html = resume_html.Replace("@@experience", member.ProfessionalExperience);
                        }
                        else
                        {
                            resume_html = resume_html.Replace("@@experience", "Nil");
                        }

                        if (!string.IsNullOrEmpty(member.Education))
                        {
                            resume_html = resume_html.Replace("@@education", member.Education);
                        }
                        else
                        {
                            resume_html = resume_html.Replace("@@education", "Nil");
                        }

                        if (!string.IsNullOrEmpty(member.Certifications))
                        {
                            resume_html = resume_html.Replace("@@certification", member.Certifications);
                        }
                        else
                        {
                            resume_html = resume_html.Replace("@@certification", "Nil");
                        }

                        if (!string.IsNullOrEmpty(member.Affiliations))
                        {
                            resume_html = resume_html.Replace("@@affiliations", member.Affiliations);
                        }
                        else
                        {
                            resume_html = resume_html.Replace("@@affiliations", "Nil");
                        }
                        sreader.Close();
                        resume_html = resume_html.Replace("<br>", "<br/>");
                        //XMLWorker also reads from a TextReader and not directly from a string
                        using (var srHtml = new StringReader(resume_html))
                        {
                            //Parse the HTML
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
                        }
                        doc.Close();
                    }
                }

                //After all of the PDF "stuff" above is done and closed but **before** we
                //close the MemoryStream, grab all of the active bytes from the stream
                bytes = ms.ToArray();
            }
            return bytes;
        }
    }
}