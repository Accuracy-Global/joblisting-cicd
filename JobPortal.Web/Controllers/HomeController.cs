using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Utility;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Web.Models;
using PagedList;
using WebMatrix.WebData;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Collections;
using System.Configuration;
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Helpers;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Net;
using iTextSharp.text;
using System.Web.Script.Serialization;
using System.Web;
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Threading.Tasks;
using JobPortal.Web.App_Start;

namespace JobPortal.Web.Controllers
{
    public class HomeController : BaseController
    {
#pragma warning disable CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
        IHelperService helper;
#pragma warning restore CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INetworkService' could not be found (are you missing a using directive or an assembly reference?)
        INetworkService iNetworkService;
#pragma warning restore CS0246 // The type or namespace name 'INetworkService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INetworkService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
        public HomeController(IUserService service, IHelperService helper, INetworkService iNetworkService)
#pragma warning restore CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'INetworkService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
            : base(service)
        {
            this.helper = helper;
            this.iNetworkService = iNetworkService;
            //string pwd = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String("Tm5hYnVpa2U2NjAw"));
        }

        List<Tracking> app_list = new List<Tracking>();
        string email = string.Empty;

        [Authorize]
        [UrlPrivilegeFilter]
        public ActionResult Contact()
        {
            UserInfoEntity uinfo = _service.Get(User.Id);
            if (!uinfo.IsConfirmed)
            {
                return RedirectToAction("Confirm", "Account", new { id = uinfo.Id, returnUrl = Request.Url.ToString() });
            }
            ViewBag.Message = "Your contact page.";
            return View(new ContactModel());
        }

        [HttpPost]
        public ActionResult Contact(ContactModel model)
        {
            StringBuilder sbHtml = new StringBuilder();
            Recipient from = new Recipient()
            {
                Email = UserInfo.Username,
                DisplayName = "Joblisting"
            };

            Recipient to = new Recipient()
            {
                Email = ConfigurationManager.AppSettings["WebmasterEmail"],
                DisplayName = "Joblisting Support"
            };

            try
            {
                sbHtml.Append("<div style=\"padding:25px;\"><h3>Support Requested</h3>");
                sbHtml.AppendFormat("<p>From: {0}<br/>", UserInfo.FullName);
                sbHtml.AppendFormat("Email: {0}<br/></p>", UserInfo.Username);
                sbHtml.AppendFormat("<p>Message:<br/>{0}</p><div>", model.Message);

                AlertService.Instance.SendMail(model.Subject, sbHtml.ToString(), from, to);
                TempData["UpdateData"] = "Message sent successfully!";
            }
            catch (Exception)
            {
                TempData["Error"] = "Failed to send message!";
            }
            return Redirect("/contact");
        }
        
        [UrlPrivilegeFilter]
        public ActionResult AboutUs()
        {
            return View();
        }
        [UrlPrivilegeFilter]
        public ActionResult Support()
        {
            return View();
        }

        [UrlPrivilegeFilter]
        public ActionResult Support1()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ValidateEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ValidateEmail(string email)
        {
            string[] receipent = { email };
            var reader = new StreamReader(Server.MapPath("~/Templates/Mail/verify_email.html"));
            var body = reader.ReadToEnd();
            AlertService.Instance.SendMail("Thanks", receipent, body);

            return View();
        }

        [HttpGet]
        [UrlPrivilegeFilter]
        public ActionResult Share(string url)
        {
            ShareProfileModel model = new ShareProfileModel();
            string name = string.Empty;
            if (User != null)
            {
                string webAddress = url.Substring(url.LastIndexOf('/') + 1);
                UserInfoEntity urlProfile = _service.GetByAddress(webAddress);                
                if (urlProfile != null)
                {                   
                    name = urlProfile.FullName;
                }
                model = new ShareProfileModel()
                {
                    ProfileName = name,
                    ProfileUrl = url,
                    SenderName = string.Format("{0}", UserInfo.FullName),
                    SenderEmailAddress = UserInfo.Username
                };
            }
            else
            {
                string webAddress = url.Substring(url.LastIndexOf('/') + 1);
                UserInfoEntity urlProfile = _service.GetByAddress(webAddress);
                if (urlProfile != null)
                {
                    if (urlProfile != null)
                    {
                        name = urlProfile.FullName;
                    }
                }

                model = new ShareProfileModel()
                {
                    ProfileName = name,
                    ProfileUrl = url
                };
            }
            ViewBag.Title = string.Format("Share {0} Profile", name);
            return View(model);
        }

        [HttpPost]
        public ActionResult Share(ShareProfileModel model)
        {          
            string webAddress = model.ProfileUrl.Substring(model.ProfileUrl.LastIndexOf('/') + 1);
         
            UserInfoEntity profile = _service.GetByAddress(webAddress);
            if (ModelState.IsValid)
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    var receipents = model.FriendEmails.Split(',').Distinct().ToArray<string>();

                    int same = 0;
                    int rcount = receipents.Count();
                    foreach (var email in receipents)
                    {
                        if (User != null && email.ToLower().Equals(User.Username.ToLower()))
                        {
                            same++;
                        }
                    }
                    if (same == rcount)
                    {
                        TempData["Error"] = "You cannot share profile with yourself!";
                    }
                    else
                    {
                        foreach (var email in receipents)
                        {
                            var reader = new StreamReader(Server.MapPath("~/Templates/Mail/profileshared.html"));
                            var body = string.Empty;

                            body = reader.ReadToEnd();
                            body = body.Replace("@@sender", model.SenderName);
                            body = body.Replace("@@profilename", profile.FullName);
                            body = body.Replace("@@profileurl", model.ProfileUrl);

                            string subject = string.Format("{0} has Shared Profile", model.SenderName);
                            AlertService.Instance.Send(subject, email, body);

                            var eProfile = dataHelper.GetSingle<UserProfile>("Username", email);
                            if (eProfile == null)
                            {
                                Hashtable parameters = new Hashtable();
                                parameters.Add("Email", email);

                                var entity = dataHelper.GetSingle<EmailAddress>(parameters);
                                if (entity == null)
                                {
                                    entity = new EmailAddress
                                    {
                                        Email = email
                                    };
                                    dataHelper.Add(entity);
                                }
                            }

                        }
                        TempData["UpdateData"] = "Profile shared successfully!";
                    }
                }
            }
            return Redirect(model.ProfileUrl);
        }


        [Authorize]
        public ActionResult Block1(long id, string redirect)
        {
            if (UserInfo != null && !UserInfo.IsConfirmed)
            {
                //return RedirectToAction("ConfirmRegistration", "Account");
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
            }

           // this.email = email;

            SecurityRoles type = (SecurityRoles)UserInfo.Type;
            UserProfile friend = MemberService.Instance.Get(id);
            SecurityRoles friendType = (SecurityRoles)friend.Type;

            bool connected = false;
            int app_count = 0;
            int matchings = 0;
            int int_count = 0;
            int bookmarks = 0;
            string message = string.Empty;
            Tracking record;
            switch (type)
            {
                case SecurityRoles.Jobseeker:
                    connected = ConnectionHelper.IsConnected(friend.Username, UserInfo.Username); // Connected
                    DomainService.Instance.Block(friend.UserId, UserInfo.Id);
                    if (friendType == SecurityRoles.Employers)
                    {
                        List<int> typeList = new List<int>();
                        typeList.Add((int)TrackingTypes.AUTO_MATCHED);
                        typeList.Add((int)TrackingTypes.APPLIED);
                        typeList.Add((int)TrackingTypes.INTERVIEW_INITIATED);
                        typeList.Add((int)TrackingTypes.INTERVIEW_IN_PROGRESS);
                        typeList.Add((int)TrackingTypes.BOOKMAKRED);
                        typeList.Add((int)TrackingTypes.DOWNLOADED);

                        app_count = 0;
                        matchings = 0;
                        int_count = 0;
                        bookmarks = 0;
                        int bookmark = (int)TrackingTypes.BOOKMAKRED;
                        using (JobPortalEntities context = new JobPortalEntities())
                        {
                            DataHelper dataHelper = new DataHelper(context);

                            var result = dataHelper.Get<Tracking>().Where(x => typeList.Contains(x.Type) && (x.JobseekerId == UserInfo.Id || x.UserId == UserInfo.Id) && x.Job.EmployerId == friend.UserId && x.IsDeleted == false);

                            if (result.Count() > 0)
                            {
                                app_list = result.ToList();
                            }

                            var msg_list = dataHelper.Get<Communication>().Where(x => (x.UserId == UserInfo.Id && (x.SenderId == friend.UserId || x.ReceiverId == friend.UserId)) || (x.UserId == friend.UserId && (x.SenderId == UserInfo.Id || x.ReceiverId == UserInfo.Id)) && x.IsDeleted == false).ToList();
                            foreach (Communication item in msg_list)
                            {
                                dataHelper.DeleteUpdate<Communication>(item, UserInfo.Username);
                            }
                            if (msg_list.Count > 0)
                            {
                                dataHelper.Save();
                            }

                            var bResult = dataHelper.Get<Tracking>().Where(x => x.Type == bookmark && x.UserId == friend.UserId && x.JobseekerId == UserInfo.Id && x.IsDeleted == false);
                            List<TrackingDetail> deleteDetails = new List<TrackingDetail>();
                            List<Tracking> deleteTrackings = new List<Tracking>();
                            foreach (var t in bResult)
                            {
                                TrackingDetail detail = dataHelper.Get<TrackingDetail>().SingleOrDefault(x => x.Id == t.Id);
                                if (detail != null)
                                {
                                    deleteDetails.Add(detail);
                                }
                                deleteTrackings.Add(t);
                                bookmarks++;
                            }
                            if (deleteDetails.Count > 0)
                            {
                                dataHelper.Remove<TrackingDetail>(deleteDetails);
                            }
                            if (deleteTrackings.Count > 0)
                            {
                                dataHelper.Remove<Tracking>(deleteTrackings);
                            }
                        }

                        foreach (Tracking item in app_list)
                        {
                            switch ((TrackingTypes)item.Type)
                            {
                                case TrackingTypes.INTERVIEW_INITIATED:
                                    using (JobPortalEntities context = new JobPortalEntities())
                                    {
                                        DataHelper dataHelper = new DataHelper(context);

                                        var interviews = dataHelper.Get<Interview>().Where(x => x.TrackingId == item.Id && x.IsDeleted == false).ToList();
                                        foreach (Interview interview in interviews)
                                        {
                                            FollowUp followUp = new FollowUp
                                            {
                                                InterviewId = interview.Id,
                                                NewDate = interview.InterviewDate,
                                                NewTime = interview.InterviewDate.ToString("hh:mm TT"),
                                                Message = "Interview canceled",
                                                Status = (int)FeedbackStatus.WITHDRAW,
                                                UserId = UserInfo.Id,
                                                DateUpdated = DateTime.Now,
                                                Unread = true
                                            };
                                            dataHelper.AddEntity(followUp);

                                            interview.Status = (int)InterviewStatus.WITHDRAW;
                                            interview.DateUpdated = DateTime.Now;
                                            interview.UpdatedBy = UserInfo.Username;
                                            dataHelper.UpdateEntity(interview);
                                        }

                                        if (interviews.Count > 0)
                                        {
                                            dataHelper.Save();
                                        }
                                    }
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, UserInfo.Username, out message);
                                    int_count++;
                                    break;
                                case TrackingTypes.INTERVIEW_IN_PROGRESS:
                                    using (JobPortalEntities context = new JobPortalEntities())
                                    {
                                        DataHelper dataHelper = new DataHelper(context);

                                        var interviews = dataHelper.Get<Interview>().Where(x => x.TrackingId == item.Id && x.IsDeleted == false).ToList();
                                        foreach (Interview interview in interviews)
                                        {
                                            interview.Status = (int)InterviewStatus.WITHDRAW;
                                            interview.DateUpdated = DateTime.Now;
                                            interview.UpdatedBy = UserInfo.Username;
                                            dataHelper.UpdateEntity(interview);
                                        }

                                        if (interviews.Count > 0)
                                        {
                                            dataHelper.Save();
                                        }
                                    }
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, UserInfo.Username, out message);
                                    int_count++;
                                    break;
                                case TrackingTypes.APPLIED:
                                    app_count++;
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, UserInfo.Username, out message);
                                    break;
                                case TrackingTypes.AUTO_MATCHED:
                                    matchings++;
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, UserInfo.Username, out message);
                                    break;
                                case TrackingTypes.BOOKMAKRED:
                                    using (JobPortalEntities context = new JobPortalEntities())
                                    {
                                        DataHelper dataHelper = new DataHelper(context);
                                        TrackingDetail detail = dataHelper.GetSingle<TrackingDetail>(item.Id);
                                        dataHelper.Remove<TrackingDetail>(detail);
                                        dataHelper.Remove<Tracking>(item);

                                        bookmarks++;
                                    }

                                    break;
                                case TrackingTypes.DOWNLOADED:
                                    using (JobPortalEntities context = new JobPortalEntities())
                                    {
                                        DataHelper dataHelper = new DataHelper(context);
                                        TrackingDetail detail = dataHelper.GetSingle<TrackingDetail>(item.Id);
                                        dataHelper.Remove<TrackingDetail>(detail);
                                        dataHelper.Remove<Tracking>(item);

                                        bookmarks++;
                                    }
                                    break;
                            }
                        }
                        TempData["UpdateData"] = string.Format("Successfully blocked {0}!", string.Format("{0}", friend.Company));
                    }
                    else
                    {
                        TempData["UpdateData"] = string.Format("Successfully blocked {0}!", string.Format("{0} {1}", friend.FirstName, friend.LastName));
                    }

                    // Sending mail
                    using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/from_jobseeker_block.html")))
                    {
                        var body = string.Empty;

                        body = reader.ReadToEnd();
                        body = body.Replace("@@firstname", UserInfo.FullName);
                        body = body.Replace("@@lastname", " ");

                        string content = string.Empty;
                        if (app_count > 0)
                        {
                            content += "<li>Application(s)</li>";
                        }

                        if (matchings > 0)
                        {
                            content += "<li>Matching</li>";
                        }
                        if (bookmarks > 0)
                        {
                            content += "<li>Bookmark(s)</li>";
                        }

                        if (int_count > 0)
                        {
                            content += "<li>Interview(s)</li>";
                        }

                        //content += "<li>Connection</li>";
                        if (app_count > 0 || matchings > 0 || int_count > 0)
                        {
                            body = body.Replace("@@content", "<ul>" + content.Trim() + "</ul>");
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(friend.Company) ? friend.Company : string.Format("{0} {1}", friend.FirstName, friend.LastName)));
                            string[] receipent = { UserInfo.Username };
                            var subject = string.Format("You have blocked {0}", (!string.IsNullOrEmpty(friend.Company) ? friend.Company : string.Format("{0} {1}", friend.FirstName, friend.LastName)));

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }

                    using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/to_employer_block.html")))
                    {
                        var body = string.Empty;

                        body = reader.ReadToEnd();
                        body = body.Replace("@@employer", (!string.IsNullOrEmpty(friend.Company) ? friend.Company : string.Format("{0} {1}", friend.FirstName, friend.LastName)));

                        body = body.Replace("@@firstname", UserInfo.FullName);
                        body = body.Replace("@@lastname", " ");

                        string content = string.Empty;
                        if (app_count > 0)
                        {
                            content += "<li>Application(s)</li>";
                        }

                        if (matchings > 0)
                        {
                            content += "<li>Matching</li>";
                        }
                        if (bookmarks > 0)
                        {
                            content += "<li>Bookmark(s)</li>";
                        }

                        if (int_count > 0)
                        {
                            content += "<li>Interview(s)</li>";
                        }

                        //content += "<li>Connection</li>";

                        if (app_count > 0 || matchings > 0 || int_count > 0)
                        {
                            body = body.Replace("@@content", "<ul>" + content.Trim() + "</ul>");
                            string[] receipent = { friend.Username };
                            var subject = string.Format("{0} has Withdrawn Application and or Interview", UserInfo.FullName);

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                    break;
                case SecurityRoles.Employers:

                    DomainService.Instance.Block(friend.UserId, UserInfo.Id);

                    if (friendType == SecurityRoles.Jobseeker)
                    {
                        app_list = new List<Tracking>();
                        List<int> typeList = new List<int>();
                        typeList.Add((int)TrackingTypes.AUTO_MATCHED);
                        typeList.Add((int)TrackingTypes.APPLIED);
                        typeList.Add((int)TrackingTypes.INTERVIEW_INITIATED);
                        typeList.Add((int)TrackingTypes.INTERVIEW_IN_PROGRESS);
                        typeList.Add((int)TrackingTypes.BOOKMAKRED);
                        typeList.Add((int)TrackingTypes.DOWNLOADED);

                        app_count = 0;
                        matchings = 0;
                        int_count = 0;
                        bookmarks = 0;
                        int bookmark = (int)TrackingTypes.BOOKMAKRED;
                        using (JobPortalEntities context = new JobPortalEntities())
                        {
                            DataHelper dataHelper = new DataHelper(context);

                            var result = dataHelper.Get<Tracking>().Where(x => typeList.Contains(x.Type) && x.JobseekerId == friend.UserId && (x.Job.EmployerId == UserInfo.Id || x.UserId == UserInfo.Id) && x.IsDeleted == false);
                            if (result.Count() > 0)
                            {
                                app_list = result.ToList();
                            }
                            var msg_list = dataHelper.Get<Communication>().Where(x => (x.UserId == UserInfo.Id && (x.SenderId == friend.UserId || x.ReceiverId == friend.UserId)) || (x.UserId == friend.UserId && (x.SenderId == UserInfo.Id || x.ReceiverId == UserInfo.Id)) && x.IsDeleted == false).ToList();
                            foreach (Communication item in msg_list)
                            {
                                dataHelper.DeleteUpdate<Communication>(item, UserInfo.Username);
                            }
                            if (msg_list.Count > 0)
                            {
                                dataHelper.Save();
                            }

                            var bResult = dataHelper.Get<Tracking>().Where(x => x.Type == bookmark && x.UserId == friend.UserId && x.JobId != null && x.Job.EmployerId == UserInfo.Id && x.IsDeleted == false);
                            List<TrackingDetail> deleteDetails = new List<TrackingDetail>();
                            List<Tracking> deleteTrackings = new List<Tracking>();
                            foreach (var t in bResult)
                            {
                                TrackingDetail detail = dataHelper.Get<TrackingDetail>().SingleOrDefault(x => x.Id == t.Id);
                                if (detail != null)
                                {
                                    deleteDetails.Add(detail);
                                }
                                deleteTrackings.Add(t);
                                bookmarks++;
                            }
                            if (deleteDetails.Count > 0)
                            {
                                dataHelper.Remove<TrackingDetail>(deleteDetails);
                            }
                            if (deleteTrackings.Count > 0)
                            {
                                dataHelper.Remove<Tracking>(deleteTrackings);
                            }
                        }

                        foreach (Tracking item in app_list)
                        {
                            switch ((TrackingTypes)item.Type)
                            {
                                case TrackingTypes.INTERVIEW_INITIATED:
                                    using (JobPortalEntities context = new JobPortalEntities())
                                    {
                                        DataHelper dataHelper = new DataHelper(context);

                                        var interviews = dataHelper.Get<Interview>().Where(x => x.TrackingId == item.Id && x.IsDeleted == false).ToList();
                                        foreach (Interview interview in interviews)
                                        {
                                            FollowUp followUp = new FollowUp
                                            {
                                                InterviewId = interview.Id,
                                                NewDate = interview.InterviewDate,
                                                NewTime = interview.InterviewDate.ToString("hh:mm TT"),
                                                Message = "Interview canceled",
                                                Status = (int)FeedbackStatus.WITHDRAW,
                                                UserId = UserInfo.Id,
                                                DateUpdated = DateTime.Now,
                                                Unread = true
                                            };
                                            dataHelper.AddEntity(followUp);

                                            interview.Status = (int)InterviewStatus.WITHDRAW;
                                            interview.DateUpdated = DateTime.Now;
                                            interview.UpdatedBy = UserInfo.Username;
                                            dataHelper.UpdateEntity(interview);
                                        }

                                        if (interviews.Count > 0)
                                        {
                                            dataHelper.Save();
                                        }
                                    }
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, UserInfo.Username, out message);
                                    int_count++;
                                    break;
                                case TrackingTypes.INTERVIEW_IN_PROGRESS:
                                    using (JobPortalEntities context = new JobPortalEntities())
                                    {
                                        DataHelper dataHelper = new DataHelper(context);

                                        var interviews = dataHelper.Get<Interview>().Where(x => x.TrackingId == item.Id && x.IsDeleted == false).ToList();
                                        foreach (Interview interview in interviews)
                                        {
                                            interview.Status = (int)InterviewStatus.WITHDRAW;
                                            interview.DateUpdated = DateTime.Now;
                                            interview.UpdatedBy = UserInfo.Username;
                                            dataHelper.UpdateEntity(interview);
                                        }

                                        if (interviews.Count > 0)
                                        {
                                            dataHelper.Save();
                                        }
                                    }
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, UserInfo.Username, out message);
                                    int_count++;
                                    break;
                                case TrackingTypes.APPLIED:
                                    app_count++;
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, UserInfo.Username, out message);
                                    break;
                                case TrackingTypes.AUTO_MATCHED:
                                    matchings++;
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, UserInfo.Username, out message);
                                    break;
                                case TrackingTypes.BOOKMAKRED:
                                    using (JobPortalEntities context = new JobPortalEntities())
                                    {
                                        DataHelper dataHelper = new DataHelper(context);
                                        TrackingDetail detail = dataHelper.GetSingle<TrackingDetail>(item.Id);
                                        dataHelper.Remove<TrackingDetail>(detail);
                                        dataHelper.Remove<Tracking>(item);

                                        bookmarks++;
                                    }
                                    break;
                                case TrackingTypes.DOWNLOADED:
                                    using (JobPortalEntities context = new JobPortalEntities())
                                    {
                                        DataHelper dataHelper = new DataHelper(context);
                                        TrackingDetail detail = dataHelper.GetSingle<TrackingDetail>(item.Id);
                                        dataHelper.Remove<TrackingDetail>(detail);
                                        dataHelper.Remove<Tracking>(item);

                                        bookmarks++;
                                    }
                                    break;
                            }
                        }

                        TempData["UpdateData"] = string.Format("Successfully blocked {0}!<br/><br/>", string.Format("{0} {1}", friend.FirstName, friend.LastName));
                    }
                    else
                    {
                        TempData["UpdateData"] = string.Format("Successfully blocked {0}!<br/><br/>", friend.Company);
                    }

                    // Sending mail
                    using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/to_jobseeker_block.html")))
                    {
                        var body = string.Empty;

                        body = reader.ReadToEnd();
                        body = body.Replace("@@firstname", friend.FirstName);
                        body = body.Replace("@@lastname", friend.LastName);

                        string content = string.Empty;
                        if (app_count > 0)
                        {
                            content += "<li>Application(s)</li>";
                        }

                        if (matchings > 0)
                        {
                            content += "<li>Matching</li>";
                        }
                        if (bookmarks > 0)
                        {
                            content += "<li>Bookmark(s)</li>";
                        }

                        if (int_count > 0)
                        {
                            content += "<li>Interview(s)</li>";
                        }

                        //content += "<li>Connection</li>";

                        if (app_count > 0 || matchings > 0 || int_count > 0)
                        {
                            body = body.Replace("@@content", "<ul>" + content.Trim() + "</ul>");
                            body = body.Replace("@@employer", UserInfo.FullName);
                            string[] receipent = { friend.Username };
                            var subject = "Application(s) and or Interview(s) Rejected";

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }

                    using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/from_employer_block.html")))
                    {
                        var body = string.Empty;

                        body = reader.ReadToEnd();
                        body = body.Replace("@@employer", UserInfo.FullName);

                        body = body.Replace("@@firstname", friend.FirstName);
                        body = body.Replace("@@lastname", friend.LastName);

                        string content = string.Empty;
                        if (app_count > 0)
                        {
                            content += "<li>Application(s)</li>";
                        }

                        if (matchings > 0)
                        {
                            content += "<li>Matching</li>";
                        }
                        if (bookmarks > 0)
                        {
                            content += "<li>Bookmark(s)</li>";
                        }

                        if (int_count > 0)
                        {
                            content += "<li>Interview(s)</li>";
                        }

                        //content += "<li>Connection</li>";

                        if (app_count > 0 || matchings > 0 || int_count > 0)
                        {
                            body = body.Replace("@@content", "<ul>" + content.Trim() + "</ul>");
                            string[] receipent = { UserInfo.Username };
                            var subject = string.Format("You have blocked {0}", (!string.IsNullOrEmpty(friend.Company) ? friend.Company : string.Format("{0} {1}", friend.FirstName, friend.LastName)));

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                    break;
            }

            if (string.IsNullOrEmpty(redirect))
            {
                redirect = "/Network/BlockedList";
            }
            return Redirect(redirect);
        }

        /// <summary>
        /// Blocks Person/Connection
        /// </summary>
        /// <param name="email"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        [Authorize]
        [UrlPrivilegeFilter]
        public ActionResult Block(string email, string redirect)
        {
            if (UserInfo != null && !UserInfo.IsConfirmed)
            {
                //return RedirectToAction("ConfirmRegistration", "Account");
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
            }

            this.email = email;

            SecurityRoles type = (SecurityRoles)UserInfo.Type;
            UserProfile friend = MemberService.Instance.Get(email);
            SecurityRoles friendType = (SecurityRoles)friend.Type;

            bool connected = false;
            int app_count = 0;
            int matchings = 0;
            int int_count = 0;
            int bookmarks = 0;
            string message = string.Empty;
            Tracking record;
            switch (type)
            {
                case SecurityRoles.Jobseeker:
                    connected = ConnectionHelper.IsConnected(email, UserInfo.Username); // Connected
                    DomainService.Instance.Block(friend.UserId, UserInfo.Id);
                    if (friendType == SecurityRoles.Employers)
                    {
                        List<int> typeList = new List<int>();
                        typeList.Add((int)TrackingTypes.AUTO_MATCHED);
                        typeList.Add((int)TrackingTypes.APPLIED);
                        typeList.Add((int)TrackingTypes.INTERVIEW_INITIATED);
                        typeList.Add((int)TrackingTypes.INTERVIEW_IN_PROGRESS);
                        typeList.Add((int)TrackingTypes.BOOKMAKRED);
                        typeList.Add((int)TrackingTypes.DOWNLOADED);

                        app_count = 0;
                        matchings = 0;
                        int_count = 0;
                        bookmarks = 0;
                        int bookmark = (int)TrackingTypes.BOOKMAKRED;
                        using (JobPortalEntities context = new JobPortalEntities())
                        {
                            DataHelper dataHelper = new DataHelper(context);

                            var result = dataHelper.Get<Tracking>().Where(x => typeList.Contains(x.Type) && (x.JobseekerId == UserInfo.Id || x.UserId == UserInfo.Id) && x.Job.EmployerId == friend.UserId && x.IsDeleted == false);

                            if (result.Count() > 0)
                            {
                                app_list = result.ToList();
                            }

                            var msg_list = dataHelper.Get<Communication>().Where(x => (x.UserId == UserInfo.Id && (x.SenderId == friend.UserId || x.ReceiverId == friend.UserId)) || (x.UserId == friend.UserId && (x.SenderId == UserInfo.Id || x.ReceiverId == UserInfo.Id)) && x.IsDeleted == false).ToList();
                            foreach (Communication item in msg_list)
                            {
                                dataHelper.DeleteUpdate<Communication>(item, UserInfo.Username);
                            }
                            if (msg_list.Count > 0)
                            {
                                dataHelper.Save();
                            }

                            var bResult = dataHelper.Get<Tracking>().Where(x => x.Type == bookmark && x.UserId == friend.UserId && x.JobseekerId == UserInfo.Id && x.IsDeleted == false);
                            List<TrackingDetail> deleteDetails = new List<TrackingDetail>();
                            List<Tracking> deleteTrackings = new List<Tracking>();
                            foreach (var t in bResult)
                            {
                                TrackingDetail detail = dataHelper.Get<TrackingDetail>().SingleOrDefault(x => x.Id == t.Id);
                                if (detail != null)
                                {
                                    deleteDetails.Add(detail);
                                }
                                deleteTrackings.Add(t);
                                bookmarks++;
                            }
                            if (deleteDetails.Count > 0)
                            {
                                dataHelper.Remove<TrackingDetail>(deleteDetails);
                            }
                            if (deleteTrackings.Count > 0)
                            {
                                dataHelper.Remove<Tracking>(deleteTrackings);
                            }
                        }

                        foreach (Tracking item in app_list)
                        {
                            switch ((TrackingTypes)item.Type)
                            {
                                case TrackingTypes.INTERVIEW_INITIATED:
                                    using (JobPortalEntities context = new JobPortalEntities())
                                    {
                                        DataHelper dataHelper = new DataHelper(context);

                                        var interviews = dataHelper.Get<Interview>().Where(x => x.TrackingId == item.Id && x.IsDeleted == false).ToList();
                                        foreach (Interview interview in interviews)
                                        {
                                            FollowUp followUp = new FollowUp
                                            {
                                                InterviewId = interview.Id,
                                                NewDate = interview.InterviewDate,
                                                NewTime = interview.InterviewDate.ToString("hh:mm TT"),
                                                Message = "Interview canceled",
                                                Status = (int)FeedbackStatus.WITHDRAW,
                                                UserId = UserInfo.Id,
                                                DateUpdated = DateTime.Now,
                                                Unread = true
                                            };
                                            dataHelper.AddEntity(followUp);

                                            interview.Status = (int)InterviewStatus.WITHDRAW;
                                            interview.DateUpdated = DateTime.Now;
                                            interview.UpdatedBy = UserInfo.Username;
                                            dataHelper.UpdateEntity(interview);
                                        }

                                        if (interviews.Count > 0)
                                        {
                                            dataHelper.Save();
                                        }
                                    }
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, UserInfo.Username, out message);
                                    int_count++;
                                    break;
                                case TrackingTypes.INTERVIEW_IN_PROGRESS:
                                    using (JobPortalEntities context = new JobPortalEntities())
                                    {
                                        DataHelper dataHelper = new DataHelper(context);

                                        var interviews = dataHelper.Get<Interview>().Where(x => x.TrackingId == item.Id && x.IsDeleted == false).ToList();
                                        foreach (Interview interview in interviews)
                                        {
                                            interview.Status = (int)InterviewStatus.WITHDRAW;
                                            interview.DateUpdated = DateTime.Now;
                                            interview.UpdatedBy = UserInfo.Username;
                                            dataHelper.UpdateEntity(interview);
                                        }

                                        if (interviews.Count > 0)
                                        {
                                            dataHelper.Save();
                                        }
                                    }
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, UserInfo.Username, out message);
                                    int_count++;
                                    break;
                                case TrackingTypes.APPLIED:
                                    app_count++;
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, UserInfo.Username, out message);
                                    break;
                                case TrackingTypes.AUTO_MATCHED:
                                    matchings++;
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, UserInfo.Username, out message);
                                    break;
                                case TrackingTypes.BOOKMAKRED:
                                    using (JobPortalEntities context = new JobPortalEntities())
                                    {
                                        DataHelper dataHelper = new DataHelper(context);
                                        TrackingDetail detail = dataHelper.GetSingle<TrackingDetail>(item.Id);
                                        dataHelper.Remove<TrackingDetail>(detail);
                                        dataHelper.Remove<Tracking>(item);

                                        bookmarks++;
                                    }

                                    break;
                                case TrackingTypes.DOWNLOADED:
                                    using (JobPortalEntities context = new JobPortalEntities())
                                    {
                                        DataHelper dataHelper = new DataHelper(context);
                                        TrackingDetail detail = dataHelper.GetSingle<TrackingDetail>(item.Id);
                                        dataHelper.Remove<TrackingDetail>(detail);
                                        dataHelper.Remove<Tracking>(item);

                                        bookmarks++;
                                    }
                                    break;
                            }
                        }
                        TempData["UpdateData"] = string.Format("Successfully blocked {0}!", string.Format("{0}", friend.Company));
                    }
                    else
                    {
                        TempData["UpdateData"] = string.Format("Successfully blocked {0}!", string.Format("{0} {1}", friend.FirstName, friend.LastName));
                    }

                    // Sending mail
                    using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/from_jobseeker_block.html")))
                    {
                        var body = string.Empty;

                        body = reader.ReadToEnd();
                        body = body.Replace("@@firstname", UserInfo.FullName);
                        body = body.Replace("@@lastname", " ");

                        string content = string.Empty;
                        if (app_count > 0)
                        {
                            content += "<li>Application(s)</li>";
                        }

                        if (matchings > 0)
                        {
                            content += "<li>Matching</li>";
                        }
                        if (bookmarks > 0)
                        {
                            content += "<li>Bookmark(s)</li>";
                        }

                        if (int_count > 0)
                        {
                            content += "<li>Interview(s)</li>";
                        }

                        //content += "<li>Connection</li>";
                        if (app_count > 0 || matchings > 0 || int_count > 0)
                        {
                            body = body.Replace("@@content", "<ul>" + content.Trim() + "</ul>");
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(friend.Company) ? friend.Company : string.Format("{0} {1}", friend.FirstName, friend.LastName)));
                            string[] receipent = { UserInfo.Username };
                            var subject = string.Format("You have blocked {0}", (!string.IsNullOrEmpty(friend.Company) ? friend.Company : string.Format("{0} {1}", friend.FirstName, friend.LastName)));

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }

                    using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/to_employer_block.html")))
                    {
                        var body = string.Empty;

                        body = reader.ReadToEnd();
                        body = body.Replace("@@employer", (!string.IsNullOrEmpty(friend.Company) ? friend.Company : string.Format("{0} {1}", friend.FirstName, friend.LastName)));

                        body = body.Replace("@@firstname", UserInfo.FullName);
                        body = body.Replace("@@lastname", " ");

                        string content = string.Empty;
                        if (app_count > 0)
                        {
                            content += "<li>Application(s)</li>";
                        }

                        if (matchings > 0)
                        {
                            content += "<li>Matching</li>";
                        }
                        if (bookmarks > 0)
                        {
                            content += "<li>Bookmark(s)</li>";
                        }

                        if (int_count > 0)
                        {
                            content += "<li>Interview(s)</li>";
                        }

                        //content += "<li>Connection</li>";

                        if (app_count > 0 || matchings > 0 || int_count > 0)
                        {
                            body = body.Replace("@@content", "<ul>" + content.Trim() + "</ul>");
                            string[] receipent = { friend.Username };
                            var subject = string.Format("{0} has Withdrawn Application and or Interview", UserInfo.FullName);

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                    break;
                case SecurityRoles.Employers:

                    DomainService.Instance.Block(friend.UserId, UserInfo.Id);

                    if (friendType == SecurityRoles.Jobseeker)
                    {
                        app_list = new List<Tracking>();
                        List<int> typeList = new List<int>();
                        typeList.Add((int)TrackingTypes.AUTO_MATCHED);
                        typeList.Add((int)TrackingTypes.APPLIED);
                        typeList.Add((int)TrackingTypes.INTERVIEW_INITIATED);
                        typeList.Add((int)TrackingTypes.INTERVIEW_IN_PROGRESS);
                        typeList.Add((int)TrackingTypes.BOOKMAKRED);
                        typeList.Add((int)TrackingTypes.DOWNLOADED);

                        app_count = 0;
                        matchings = 0;
                        int_count = 0;
                        bookmarks = 0;
                        int bookmark = (int)TrackingTypes.BOOKMAKRED;
                        using (JobPortalEntities context = new JobPortalEntities())
                        {
                            DataHelper dataHelper = new DataHelper(context);

                            var result = dataHelper.Get<Tracking>().Where(x => typeList.Contains(x.Type) && x.JobseekerId == friend.UserId && (x.Job.EmployerId == UserInfo.Id || x.UserId == UserInfo.Id) && x.IsDeleted == false);
                            if (result.Count() > 0)
                            {
                                app_list = result.ToList();
                            }
                            var msg_list = dataHelper.Get<Communication>().Where(x => (x.UserId == UserInfo.Id && (x.SenderId == friend.UserId || x.ReceiverId == friend.UserId)) || (x.UserId == friend.UserId && (x.SenderId == UserInfo.Id || x.ReceiverId == UserInfo.Id)) && x.IsDeleted == false).ToList();
                            foreach (Communication item in msg_list)
                            {
                                dataHelper.DeleteUpdate<Communication>(item, UserInfo.Username);
                            }
                            if (msg_list.Count > 0)
                            {
                                dataHelper.Save();
                            }

                            var bResult = dataHelper.Get<Tracking>().Where(x => x.Type == bookmark && x.UserId == friend.UserId && x.JobId != null && x.Job.EmployerId == UserInfo.Id && x.IsDeleted == false);
                            List<TrackingDetail> deleteDetails = new List<TrackingDetail>();
                            List<Tracking> deleteTrackings = new List<Tracking>();
                            foreach (var t in bResult)
                            {
                                TrackingDetail detail = dataHelper.Get<TrackingDetail>().SingleOrDefault(x => x.Id == t.Id);
                                if (detail != null)
                                {
                                    deleteDetails.Add(detail);
                                }
                                deleteTrackings.Add(t);
                                bookmarks++;
                            }
                            if (deleteDetails.Count > 0)
                            {
                                dataHelper.Remove<TrackingDetail>(deleteDetails);
                            }
                            if (deleteTrackings.Count > 0)
                            {
                                dataHelper.Remove<Tracking>(deleteTrackings);
                            }
                        }

                        foreach (Tracking item in app_list)
                        {
                            switch ((TrackingTypes)item.Type)
                            {
                                case TrackingTypes.INTERVIEW_INITIATED:
                                    using (JobPortalEntities context = new JobPortalEntities())
                                    {
                                        DataHelper dataHelper = new DataHelper(context);

                                        var interviews = dataHelper.Get<Interview>().Where(x => x.TrackingId == item.Id && x.IsDeleted == false).ToList();
                                        foreach (Interview interview in interviews)
                                        {
                                            FollowUp followUp = new FollowUp
                                            {
                                                InterviewId = interview.Id,
                                                NewDate = interview.InterviewDate,
                                                NewTime = interview.InterviewDate.ToString("hh:mm TT"),
                                                Message = "Interview canceled",
                                                Status = (int)FeedbackStatus.WITHDRAW,
                                                UserId = UserInfo.Id,
                                                DateUpdated = DateTime.Now,
                                                Unread = true
                                            };
                                            dataHelper.AddEntity(followUp);

                                            interview.Status = (int)InterviewStatus.WITHDRAW;
                                            interview.DateUpdated = DateTime.Now;
                                            interview.UpdatedBy = UserInfo.Username;
                                            dataHelper.UpdateEntity(interview);
                                        }

                                        if (interviews.Count > 0)
                                        {
                                            dataHelper.Save();
                                        }
                                    }
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, UserInfo.Username, out message);
                                    int_count++;
                                    break;
                                case TrackingTypes.INTERVIEW_IN_PROGRESS:
                                    using (JobPortalEntities context = new JobPortalEntities())
                                    {
                                        DataHelper dataHelper = new DataHelper(context);

                                        var interviews = dataHelper.Get<Interview>().Where(x => x.TrackingId == item.Id && x.IsDeleted == false).ToList();
                                        foreach (Interview interview in interviews)
                                        {
                                            interview.Status = (int)InterviewStatus.WITHDRAW;
                                            interview.DateUpdated = DateTime.Now;
                                            interview.UpdatedBy = UserInfo.Username;
                                            dataHelper.UpdateEntity(interview);
                                        }

                                        if (interviews.Count > 0)
                                        {
                                            dataHelper.Save();
                                        }
                                    }
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, UserInfo.Username, out message);
                                    int_count++;
                                    break;
                                case TrackingTypes.APPLIED:
                                    app_count++;
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, UserInfo.Username, out message);
                                    break;
                                case TrackingTypes.AUTO_MATCHED:
                                    matchings++;
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, UserInfo.Username, out message);
                                    break;
                                case TrackingTypes.BOOKMAKRED:
                                    using (JobPortalEntities context = new JobPortalEntities())
                                    {
                                        DataHelper dataHelper = new DataHelper(context);
                                        TrackingDetail detail = dataHelper.GetSingle<TrackingDetail>(item.Id);
                                        dataHelper.Remove<TrackingDetail>(detail);
                                        dataHelper.Remove<Tracking>(item);

                                        bookmarks++;
                                    }
                                    break;
                                case TrackingTypes.DOWNLOADED:
                                    using (JobPortalEntities context = new JobPortalEntities())
                                    {
                                        DataHelper dataHelper = new DataHelper(context);
                                        TrackingDetail detail = dataHelper.GetSingle<TrackingDetail>(item.Id);
                                        dataHelper.Remove<TrackingDetail>(detail);
                                        dataHelper.Remove<Tracking>(item);

                                        bookmarks++;
                                    }
                                    break;
                            }
                        }

                        TempData["UpdateData"] = string.Format("Successfully blocked {0}!<br/><br/>", string.Format("{0} {1}", friend.FirstName, friend.LastName));
                    }
                    else
                    {
                        TempData["UpdateData"] = string.Format("Successfully blocked {0}!<br/><br/>", friend.Company);
                    }

                    // Sending mail
                    using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/to_jobseeker_block.html")))
                    {
                        var body = string.Empty;

                        body = reader.ReadToEnd();
                        body = body.Replace("@@firstname", friend.FirstName);
                        body = body.Replace("@@lastname", friend.LastName);

                        string content = string.Empty;
                        if (app_count > 0)
                        {
                            content += "<li>Application(s)</li>";
                        }

                        if (matchings > 0)
                        {
                            content += "<li>Matching</li>";
                        }
                        if (bookmarks > 0)
                        {
                            content += "<li>Bookmark(s)</li>";
                        }

                        if (int_count > 0)
                        {
                            content += "<li>Interview(s)</li>";
                        }

                        //content += "<li>Connection</li>";

                        if (app_count > 0 || matchings > 0 || int_count > 0)
                        {
                            body = body.Replace("@@content", "<ul>" + content.Trim() + "</ul>");
                            body = body.Replace("@@employer", UserInfo.FullName);
                            string[] receipent = { friend.Username };
                            var subject = "Application(s) and or Interview(s) Rejected";

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }

                    using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/from_employer_block.html")))
                    {
                        var body = string.Empty;

                        body = reader.ReadToEnd();
                        body = body.Replace("@@employer", UserInfo.FullName);

                        body = body.Replace("@@firstname", friend.FirstName);
                        body = body.Replace("@@lastname", friend.LastName);

                        string content = string.Empty;
                        if (app_count > 0)
                        {
                            content += "<li>Application(s)</li>";
                        }

                        if (matchings > 0)
                        {
                            content += "<li>Matching</li>";
                        }
                        if (bookmarks > 0)
                        {
                            content += "<li>Bookmark(s)</li>";
                        }

                        if (int_count > 0)
                        {
                            content += "<li>Interview(s)</li>";
                        }

                        //content += "<li>Connection</li>";

                        if (app_count > 0 || matchings > 0 || int_count > 0)
                        {
                            body = body.Replace("@@content", "<ul>" + content.Trim() + "</ul>");
                            string[] receipent = { UserInfo.Username };
                            var subject = string.Format("You have blocked {0}", (!string.IsNullOrEmpty(friend.Company) ? friend.Company : string.Format("{0} {1}", friend.FirstName, friend.LastName)));

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                    break;
            }

            if (string.IsNullOrEmpty(redirect))
            {
                redirect = "/Network/BlockedList";
            }
            return Redirect(redirect);
        }

        [Authorize]
        [UrlPrivilegeFilter]
        public ActionResult Unblock(long id, string redirect)
        {
            if (User != null)
            {
                int stat = DomainService.Instance.Unblock(id, User.Id);
                if (stat > 0)
                {
                    TempData["UpdateData"] = "Unblocked successfully!";
                }
            }
            return Redirect(redirect);
        }

        [HttpGet]
        [ValidateInput(false)]
        [UrlPrivilegeFilter]
        public async Task<ActionResult> SearchPeople(SearchJobSeekerModel model)
        {
            var records = 0;
            int pageSize = 10;
            model.SearchJobSeekers = new List<UserProfile>();
            model.ModelList = new List<PeopleEntity>();
            if (model == null)
            {
                model = new SearchJobSeekerModel();
            }
            else
            {
                if ((!string.IsNullOrEmpty(model.Name) && model.Name.Trim().Length > 0) || (!string.IsNullOrEmpty(model.Where) && model.Where.Trim().Length > 0) || model.Type != null || model.CountryId != null || model.StateId != null || (!string.IsNullOrEmpty(model.City) && model.City.Trim().Length > 0) || (model.AgeFrom != null && model.AgeTo != null) || !string.IsNullOrEmpty(model.Gender) || model.Relationship != null)
                {
                    var searchResume = new SearchResume
                    {
                        Name = (!string.IsNullOrEmpty(model.Name) && model.Name.Trim().Length > 0) ? model.Name.Trim() : null,
                        Where = (!string.IsNullOrEmpty(model.Where) && model.Where.Trim().Length > 0) ? model.Where.Trim() : null,
                        Gender = model.Gender,
                        AgeMin = model.AgeFrom,
                        AgeMax = model.AgeTo,
                        Relationship = model.Relationship,
                        CountryId = model.CountryId,
                        StateId = model.StateId,
                        City = (!string.IsNullOrEmpty(model.City) && model.City.Trim().Length > 0) ? model.City.Trim() : null,
                        Username = (User != null ? User.Username : null),
                        PageSize = pageSize,
                        PageNumber = 1
                    };
                    model.ModelList = await _service.SearchDirectory(searchResume);
                    if (model.ModelList.Count() > 0)
                    {
                        records = model.ModelList.Max(x => x.MaxRows.Value);
                    }
                }
                ViewBag.Model = new StaticPagedList<PeopleEntity>(model.ModelList, 1, pageSize, records);
                ViewBag.Rows = records;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> SearchedPeople(SearchJobSeekerModel model, int pageNumber = 1)
        {
            JsonResult jr = new JsonResult();
            var records = 0;
            int pageSize = 10;
            model.SearchJobSeekers = new List<UserProfile>();
            model.ModelList = new List<PeopleEntity>();
            if (model == null)
            {
                model = new SearchJobSeekerModel();
            }
            else
            {
                if ((!string.IsNullOrEmpty(model.Name) && model.Name.Trim().Length > 0) || (!string.IsNullOrEmpty(model.Where) && model.Where.Trim().Length > 0) || model.Type != null || model.CountryId != null || model.StateId != null || (!string.IsNullOrEmpty(model.City) && model.City.Trim().Length > 0) || (model.AgeFrom != null && model.AgeTo != null) || !string.IsNullOrEmpty(model.Gender) || model.Relationship != null)
                {
                    var searchResume = new SearchResume
                    {
                        Name = (!string.IsNullOrEmpty(model.Name) && model.Name.Trim().Length > 0) ? model.Name.Trim() : null,
                        Where = (!string.IsNullOrEmpty(model.Where) && model.Where.Trim().Length > 0) ? model.Where.Trim() : null,
                        Gender = model.Gender,
                        AgeMin = model.AgeFrom,
                        AgeMax = model.AgeTo,
                        Relationship = model.Relationship,
                        CountryId = model.CountryId,
                        StateId = model.StateId,
                        City = (!string.IsNullOrEmpty(model.City) && model.City.Trim().Length > 0) ? model.City.Trim() : null,
                        Username = (User != null ? User.Username : null),
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    };
                    model.ModelList = await _service.SearchDirectory(searchResume);
                    if (model.ModelList.Count() > 0)
                    {
                        records = model.ModelList.Max(x => x.MaxRows.Value);
                    }
                }

                //ViewBag.Model = new StaticPagedList<PeopleEntity>(model.ModelList, pageNumber, pageSize, records);
                //ViewBag.Rows = records;
            }
            jr.Data = model.ModelList;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jr.MaxJsonLength = int.MaxValue;

            return jr;
            //return View(model);
        }



        [ValidateInput(false)]
        [AllowAnonymous]      
        [UrlPrivilegeFilter]
        public ActionResult SearchResumes(SearchResumeModel model)
        {            
            int max_rows = 0;
            List<ResumeEntity> list = new List<ResumeEntity>();
            
            if (model == null)
            {
                model = new SearchResumeModel();
                model.PageNumber = model.PageNumber == 0 ? 1 : model.PageNumber;
                model.PageSize = 10;
            }
            else
            {
                model.PageNumber = model.PageNumber == 0 ? 1 : model.PageNumber;
                model.PageSize = 10;
                if ((!string.IsNullOrEmpty(model.Name) && model.Name.Trim().Length > 0) || model.CategoryId != null || model.SpecializationId != null || model.CountryId != null || model.StateId != null || (!string.IsNullOrEmpty(model.City) && model.City.Trim().Length > 0))
                {
                    string name = (!string.IsNullOrEmpty(model.Name) && model.Name.Trim().Length > 0) ? model.Name.Trim() : null;
                    string city = (!string.IsNullOrEmpty(model.City) && model.City.Trim().Length > 0) ? model.City.Trim() : null;
                    model.Name = name;
                    model.City = city;                  
                    list = _service.SearchResumes<ResumeEntity, SearchResumeModel>(model);
                    if (list.Count > 0)
                    {
                        max_rows = list.FirstOrDefault().MaxRows;
                    }
                }

                ViewBag.Model = new StaticPagedList<ResumeEntity>(list, model.PageNumber, model.PageSize, max_rows);
                ViewBag.Rows = max_rows;               
                return View(model);               
            }
            return View(model);
        }

        [ValidateInput(false)]
        [AllowAnonymous]        
        public ActionResult Resumes(SearchResumeModel model)
        {
            List<ResumeEntity> list = new List<ResumeEntity>();
            model.PageSize = 10;
            if ((!string.IsNullOrEmpty(model.Name) && model.Name.Trim().Length > 0) || model.CategoryId != null || model.SpecializationId != null || model.CountryId != null || model.StateId != null || (!string.IsNullOrEmpty(model.City) && model.City.Trim().Length > 0))
            {
                string name = (!string.IsNullOrEmpty(model.Name) && model.Name.Trim().Length > 0) ? model.Name.Trim() : null;
                string city = (!string.IsNullOrEmpty(model.City) && model.City.Trim().Length > 0) ? model.City.Trim() : null;
                model.Name = name;
                model.City = city;
                list = _service.SearchResumes<ResumeEntity, SearchResumeModel>(model);                
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }   

        [AllowAnonymous]
        [UrlPrivilegeFilter]
        public ActionResult MemberProfile(string address, string returnUrl = "")
        {           
            ViewBag.DownloadUrl = TempData["DownloadUrl"];
            ViewBag.ReturnUrl = returnUrl;
            UserInfoEntity member = null;
            int status = 0;
            if (!string.IsNullOrEmpty(address))
            {
                member = _service.GetByAddress(address);
              
                if (member != null)
                {
                    ViewBag.Profile = member;

                    if (User != null)
                    {
                        bool isBlockedBySomeone = DomainService.Instance.IsBlockedByMe(User.Id, member.Id);

                        if (isBlockedBySomeone)
                        {
                            member = null;
                            TempData["Error"] = "This profile is in private mode!";
                        }

                        if (!User.Username.Equals(member.Username))
                        {
                            if (User.Info.Role == SecurityRoles.Jobseeker || User.Info.Role == SecurityRoles.Employers)
                            {
                                status = _service.TrackProfileView(member.Id, User.Id);
                                if (status > 0)
                                {
                                    var reader = new StreamReader(Server.MapPath("~/Templates/Mail/profile_viewed.html"));
                                    var body = string.Empty;

                                    body = reader.ReadToEnd();
                                    body = body.Replace("@@receiver", member.FullName);
                                    body = body.Replace("@@name", User.Info.FullName);
                                    body = body.Replace("@@profileurl",
                                        string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority,
                                            User.Info.PermaLink));                                   

                                    string email = member.Username;                                    
                                    string[] receipent = { email };
                                    var subject = "Your Profile was Viewed";

                                    AlertService.Instance.SendMail(subject, receipent, body);
                                }
                            }

                            if (member.Type == 4)
                            {
                                status = _service.ManageAccount(User.Info.Id, 1, null, null, null, null, member.Id);                                
                            }
                        }                       
                    }
                    else
                    {
                        status = _service.TrackProfileView(member.Id, null);
                    }

                    if (member.Type == 4)
                    {
                        return RedirectToAction("UpdateProfileL", "JobSeeker");
                    }
                    else if(member.Type==5)
                    {
                        return RedirectToAction("profile", "company");
                    }

                    //return RedirectToAction("SearchJobs", "Job");

                    // return RedirectToAction("updateprofile", "JobSeeker");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            ViewBag.BlockMessage = "";
            ViewBag.Profile = member;

            return View();
        }

        [AllowAnonymous]
        public ActionResult Thanks(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Forgot(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var profile = MemberService.Instance.Get(email);
                if (profile != null)
                {
                    var message = new StringBuilder();
                    var membershipUser = Membership.GetUser(email);

                    if (membershipUser != null && membershipUser.IsApproved)
                    {
                        var token = WebSecurity.GeneratePasswordResetToken(email);

                        var reader = new StreamReader(Server.MapPath("~/Templates/Mail/forgot.html"));
                        var body = reader.ReadToEnd();
                        body = body.Replace("@@firstname", profile.FirstName);
                        body = body.Replace("@@lastname", profile.LastName);
                        body = body.Replace("@@url", UrlManager.GetPasswordResetUrl(token, profile.Username));

                        string[] receipent = { profile.Username };
                        var subject = "Reset Your Joblisting Account Password";

                        AlertService.Instance.SendMail(subject, receipent, body);
                    }
                    return RedirectToAction("thanks", "Home", new { email = email });
                }
                else
                {
                    TempData["Error"] = string.Format("{0} email address is not in our record!");
                }
            }
            return View();
        }

        [AllowAnonymous]
        [UrlPrivilegeFilter]
        public ActionResult ForgotPassword()
        {            
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var profile = MemberService.Instance.Get(model.Email);
                if (profile != null)
                {
                    var message = new StringBuilder();
                    var membershipUser = Membership.GetUser(model.Email);

                    if (membershipUser != null && membershipUser.IsApproved)
                    {
                        var token = WebSecurity.GeneratePasswordResetToken(model.Email);

                        var reader = new StreamReader(Server.MapPath("~/Templates/Mail/forgot.html"));
                        var body = reader.ReadToEnd();
                        body = body.Replace("@@firstname", profile.FirstName);
                        body = body.Replace("@@lastname", profile.LastName);
                        body = body.Replace("@@url", UrlManager.GetPasswordResetUrl(token, profile.Username));

                        string[] receipent = { profile.Username };
                        var subject = "Reset Your Joblisting Account Password";

                        AlertService.Instance.SendMail(subject, receipent, body);
                    }
                    return RedirectToAction("thanks", "Home", new { email = model.Email });
                }
                ModelState.AddModelError("", "This email address is not in our record!");
                return View(model);
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string token, string username)
        {
            var model = new ResetPasswordModel();
            model.Token = token;
            model.Username = username;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var flag = WebSecurity.ResetPassword(model.Token, model.Password);
                //TempData["UpdateData"] = "Password changed successfully!<br/>Now you can login with new password.";
                //WebSecurity.Login(model.Username, model.Password);

                //UserInfoEntity uinfo = MemberService.Instance.GetUserInfo(model.Username);
                //UserPrincipalSerializeModel serializeModel = new UserPrincipalSerializeModel()
                //{
                //    Id = uinfo.Id,
                //    Username = uinfo.Username,
                //    FullName = uinfo.FullName,
                //    CountryName = uinfo.CountryName,
                //    StateName = uinfo.StateName,
                //    City = uinfo.City,
                //    PermaLink = uinfo.PermaLink,
                //    Type = uinfo.Type,
                //    Role = (SecurityRoles)uinfo.Type,
                //    IsConfirmed = uinfo.IsConfirmed,
                //    IsJobseeker = uinfo.IsJobseeker
                //};
                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                //string userData = serializer.Serialize(serializeModel);
                //FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                //         1,
                //         uinfo.Id.ToString(),
                //         DateTime.UtcNow,
                //         DateTime.UtcNow.AddHours(10),
                //         false,
                //         userData);

                //string encTicket = FormsAuthentication.Encrypt(authTicket);
                //HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                //Response.Cookies.Add(faCookie);

                //return Redirect(string.Format("/{0}", serializeModel.PermaLink));
                ViewBag.Changed = flag;
            }
            return View(model);
        }

        [UrlPrivilegeFilter]
        public ActionResult Companies(long countryId, string name, long? stateId = null, string city = null, int pageNumber = 0)
        {
            int pageSize = 10;
            int rows = 0;

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                var result = dataHelper.Get<UserProfile>().Where(x => x.Type == 5 && x.CountryId == countryId);
                if (!string.IsNullOrEmpty(name))
                {
                    result = result.Where(x => x.Company.ToLower().Contains(name.ToLower()));
                }

                if (stateId != null)
                {
                    result = result.Where(x => x.StateId == stateId.Value);
                }

                if (!string.IsNullOrEmpty(city))
                {
                    result = result.Where(x => x.City.ToLower().Contains(city.ToLower()));
                }
                rows = result.Count();
                result = result.OrderByDescending(x => x.DateCreated).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize)).Take(pageSize);

                ViewBag.CompanyList = new StaticPagedList<UserProfile>(result.ToList(), (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
                ViewBag.Rows = rows;
                ViewBag.StateList = SharedService.Instance.GetStatesById(countryId);
                ViewBag.Country = SharedService.Instance.GetCountry(countryId);
            }
            return View();
        }

        [UrlPrivilegeFilter]
        public ActionResult Individuals(long countryId, int? ageFrom = null, int? ageTo = null, string gender = null, int? Relationship = null, int pageNumber = 0)
        {
            int pageSize = 10;
            int rows = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<UserProfile>().Where(x => x.Type == 4 && x.CountryId == countryId);

                if (ageFrom != null)
                {
                    result = result.Where(x => x.Age >= ageFrom);
                }

                if (ageTo != null)
                {
                    result = result.Where(x => x.Age <= ageTo);
                }

                if (!string.IsNullOrEmpty(gender))
                {
                    result = result.Where(x => x.Gender.Equals(gender));
                }

                if (Relationship != null)
                {
                    result = result.Where(x => x.RelationshipStatus == Relationship);
                }

                rows = result.Count();
                result = result.OrderByDescending(x => x.DateCreated).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize)).Take(pageSize);

                ViewBag.UserList = new StaticPagedList<UserProfile>(result.ToList(), (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
                ViewBag.Rows = rows;
                ViewBag.StateList = SharedService.Instance.GetStatesById(countryId);
                ViewBag.Country = SharedService.Instance.GetCountry(countryId);
            }
            return View();
        }

        [Authorize]
        public PartialViewResult Connections(string address)
        {
          UserInfoEntity profile = null;
            if (string.IsNullOrEmpty(address))
            {
                profile = MemberService.Instance.GetUserInfo(User.Username);
            }
            else
            {
                profile = MemberService.Instance.GetUserInfoByAddress(address);
            }
            List<UserInfo> connections = new List<UserInfo>();

            if (profile != null && User != null)
            {
                List<long?> connected = DomainService.Instance.ConnectionList(profile.Id, User.Id);

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    if (profile != null)
                    {
                        connected = connected.Where(x => x.HasValue).ToList();
                        if (connected.Count > 0)
                        {
                            connections = connected.Select(x => new UserInfo(x.Value)).ToList();
                            if (connections.Count > 0)
                            {
                                connections = connections.OrderBy(x => Guid.NewGuid()).Take(3).ToList();
                            }
                        }
                    }
                }
            }
            return PartialView(connections);
        }
        [ChildActionOnly]
        public PartialViewResult ActivityStream(string Username)
        {
            UserProfile member = MemberService.Instance.Get(Username);
            ViewBag.Member = member;
            ViewBag.Default = new UserInfo(member.UserId);
            List<ActivityStreamModel> activities = MemberService.Instance.GetPhotoList(member.Username, Request.IsAuthenticated)
                .Select(x => new ActivityStreamModel(x.UserId) { Id = x.Id, DateUpdated = x.DateUpdated, Image = Convert.ToBase64String(x.Image), Type = x.Type, Area = x.Area }).OrderByDescending(x => x.DateUpdated).ToList();

            return PartialView(activities);
        }
        //[ChildActionOnly]
        //public PartialViewResult Wall1(long id,string country)
        //{

        //        ViewBag.Id = id;
        //        var jobss = JobService.Instance.GetRecommendedJobs1(id);
        //        var result = jobss.Count();
        //        ViewBag.result = result;
        //        if (result >= 1)
        //        {
        //            ViewBag.RecommendedJobs1 = jobss;
        //        }
        //    else if (result < 1)

        //    {
        //        if (User.Info.Role == SecurityRoles.Jobseeker || User.Info.Role == SecurityRoles.Interns || User.Info.Role == SecurityRoles.Student)
        //        {

        //            SearchJobModel model = new SearchJobModel();
        //            ViewBag.LatestJob2 = JobService.Instance.GetLatestJobs2(country == null ? "" : country, "").Take(5);//.OrderBy(x=>x.DateCreated).Take(5);
        //            return PartialView(model);
        //        }
        //    }
        //    return PartialView();            
        //}

       
        [ChildActionOnly]
        public PartialViewResult Wall1(string country, long id)
        {
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
            var defaultUser = new UserInfo(id);
            SearchJobModel model = new SearchJobModel();
            //var jobss = JobService.Instance.GetRecommendedJobs1(id);
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
           // return PartialView(model);


            //ViewBag.Id = id;
            //var jobss = JobService.Instance.GetRecommendedJobs1(id);
            //var result = jobss.Count();
            //ViewBag.result = result;
            //if (result >= 1)
            //{
            //    ViewBag.RecommendedJobs1 = jobss;
            //}
            //else if(result < 1)
            //{
            //    ViewBag.Text = "For Recommended Jobs Create Resume";
            //}
            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult Wall(long id)
        {
            ViewBag.Id = id;
            return PartialView();
        }

        public async Task<PartialViewResult> PeopleList()
        {                                               
            List<UserMatchEntity> user_list = new List<UserMatchEntity>();            
            if (User != null)
            {
                user_list = await _service.PeopleMatchList(User.Id);               
                user_list = user_list.OrderBy(x => Guid.NewGuid()).Take(8).ToList();
            }
            return PartialView(user_list);
        }

        public ActionResult ProfileViews()
        {
            List<Parameter> parameters = new List<Parameter>();
            List<UserInfo> user_list = new List<UserInfo>();

            if (User != null)
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    var blockedList = dataHelper.Get<BlockedPeople>().Where(x => x.BlockedId == User.Id).Select(x => x.BlockerId);

                    var views = dataHelper.Get<ProfileView>().Where(x => x.ProfileId == User.Id && x.UserId != null);
                    if (blockedList.Any())
                    {
                        views = views.Where(x => !blockedList.Contains(x.UserId.Value));
                    }
                    user_list = views.Where(x => x.UserId != User.Id).Select(x => x.UserProfile).Distinct().ToList()
                        .OrderBy(x => Guid.NewGuid()).Take(8).Select(x => new UserInfo(x.UserId)).ToList();
                }
            }
            return PartialView(user_list);
        }
        [ChildActionOnly]
        public PartialViewResult RecentJobs(long Id)
        {
            ViewBag.UserId = Id;
            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult RecentJobsWithoutLogo(long Id)
        {
            ViewBag.UserId = Id;
            return PartialView();
        }

        [Authorize]
        public async Task<ActionResult> Connect(long id, string redirect = "")
        {
            UserInfoEntity inviter = _service.Get(User.Id);
            UserInfoEntity acceptor = _service.Get(id);
            bool byLoggedInUser = false;
            bool byEmailUser = false;
            List<ConnectionEntity> contact_list = new List<ConnectionEntity>();

            if (inviter != null && inviter.IsConfirmed == false)
            {
                return RedirectToAction("Confirm", "Account", new { id = inviter.Id, returnUrl = Request.Url.ToString() });
            }

            BlockedEntity entity = _service.GetBlockedEntry(acceptor.Id, inviter.Id);
            if (inviter != null && acceptor != null)
            {
                if (entity != null)
                {
                    if (!entity.CreatedBy.Equals(inviter.Username))
                    {
                        byLoggedInUser = false;
                        byEmailUser = true;
                    }
                    else
                    {
                        byLoggedInUser = true;
                        byEmailUser = false;
                    }
                }
            }
            if (entity != null && !entity.CreatedBy.Equals(inviter.Username))
            {
                return View("Blocked");
            }

            ConnectionEntity connection = await iNetworkService.Get(inviter.Id, acceptor.Username);
            if (connection != null)
            {
                if (connection.CreatedBy.Equals(User.Username))
                {
                    if (connection.IsAccepted == false && connection.IsConnected == false)
                    {
                        TempData["Error"] = "Connection invitation already sent, waiting for acceptance!";
                    }
                    else if (connection.IsAccepted == true & connection.IsConnected == true)
                    {
                        TempData["Error"] = string.Format("You are already connected with {0} {1}!", acceptor.FirstName, acceptor.LastName);
                    }
                }
                else
                {
                    if (connection.IsAccepted == false && connection.IsConnected == false)
                    {
                        TempData["UpdateData"] = string.Format("Connection is already initiated by {0} and waiting for  your acceptance!<br/>You may &nbsp;<a href=\"/Network/Accept?Id=" + connection.Id + "&redirect=/Network/Index\">Accept</a>&nbsp;or&nbsp;<a href=\"/Network/Disconnect?Id=" + connection.Id + "&redirect=/Network/Index\">Reject</a>", acceptor.FullName);
                    }
                    else if (connection.IsAccepted == true & connection.IsConnected == true)
                    {
                        TempData["Error"] = string.Format("You are already connected with {0}!", acceptor.FullName);
                    }
                }
            }

            if (!string.IsNullOrEmpty(Request.QueryString["via"]))
            {               
                if (connection != null)
                {            
                    if (connection.IsDeleted == false && connection.IsAccepted == false && connection.IsConnected == false && connection.Initiated == true)
                    {
                        if (!connection.CreatedBy.Equals(UserInfo.Username))
                        {
                            return RedirectToAction("Accept", new { Id = connection.Id });
                        }
                    }
                }
            }
                

            if (byEmailUser == false && byLoggedInUser == false)
            {
                if (!inviter.Username.ToLower().Equals(acceptor.Username))
                {
                    ConnectionEntity contact = await iNetworkService.Connect(acceptor.Username, inviter.Username);
                    if (contact != null)
                    {
                        var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation.html"));
                        var body = reader.ReadToEnd();
                        var subject = string.Empty;
                        string name = "";
                        if (acceptor != null)
                        {
                            name = acceptor.FirstName + " " + acceptor.LastName;
                        }
                        else
                        {
                            name = acceptor.Username;
                        }

                        body = body.Replace("@@receiver", name);
                        body = body.Replace("@@sender", string.Format("{0}", inviter.FullName));
                        subject = string.Format("{0} Invites you to connect at Joblisting", inviter.FullName);
                        body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, UserInfo.PermaLink));
                        body = body.Replace("@@accepturl", string.Format("{0}://{1}/Network/Accept/{2}", Request.Url.Scheme, Request.Url.Authority, contact.Id));
                        body = body.Replace("@@button", "Accept");

                        if (acceptor != null)
                        {
                            body = body.Replace("@@unsubscribe", "");
                        }
                        else
                        {
                            string ulink = string.Format("<a href=\"{0}://{1}/Network/Unsubscribe?Id={2}\">unsubscribe</a> or ", Request.Url.Scheme, Request.Url.Authority, contact.Id);
                            body = body.Replace("@@unsubscribe", ulink);
                        }

                        string[] receipent = { acceptor.Username };
                        AlertService.Instance.SendMail(subject, receipent, body);
                        TempData["SaveData"] = "Connection invitation initiated successfully!";
                    }                  
                }
                else
                {
                    TempData["Error"] = "You cannot connect with yourself!";
                }
            }
            else
            {
                if (byLoggedInUser)
                {
                    if (acceptor != null)
                    {
                        TempData["Error"] = "You have blocked this person!<br/>Do you want to Unblock or Skip?<br/><a href=\"/Home/Unblock?id=" + acceptor.Id + "&redirect=/Network/Index\">Unblock</a>&nbsp;&nbsp;&nbsp;<a href=\"#\" onclick=\"window.location.reload();\">Skip</a>";
                    }
                }
                else if (byEmailUser)
                {
                    if (inviter != null)
                    {
                        TempData["Error"] = "This profile is in private mode!";
                    }
                }
            }        
            if (!string.IsNullOrEmpty(redirect))
            {
                return Redirect(redirect);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Unsubscribe(int alertId, string email = "")
        {
            int status = await _service.Unsubscribe(alertId, email);
            if (status > 0)
            {
                TempData["UpdateData"] = "Unsubscribed successfully!";
                TempData["Status"] = "Unsubscribed";
            }
            return View();
        }
    }
}