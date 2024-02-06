using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using PagedList;
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Threading.Tasks;
using JobPortal.Web.App_Start;

namespace JobPortal.Web.Controllers
{
    [Authorize]
    public class InterviewController : BaseController
    {
#pragma warning disable CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
        IJobService jobService;
#pragma warning restore CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
        public InterviewController(IUserService service, IJobService jobService)
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
            : base(service)
        {
            this.jobService = jobService;
        }
        [Authorize]
        [UrlPrivilegeFilter]
#pragma warning disable CS0246 // The type or namespace name 'InterviewFilter' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<ActionResult> Index(InterviewFilter filter)
#pragma warning restore CS0246 // The type or namespace name 'InterviewFilter' could not be found (are you missing a using directive or an assembly reference?)
        {
            filter.UserId = User.Id;
            filter.Type = User.Info.Type;
            var list = await _service.Interviews(filter);
            //int rows = 0;
            //int pageSize = 10;
            //if (User != null)
            //{
            //    using (JobPortalEntities context = new JobPortalEntities())
            //    {
            //        DataHelper dataHelper = new DataHelper(context);
            //        var profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
            //        IQueryable<Interview> result = null;
            //        SecurityRoles type = (SecurityRoles)profile.Type;
            //        switch (type)
            //        {
            //            case SecurityRoles.Employers:
            //                result = dataHelper.Get<Interview>().Where(x => x.UserId == profile.UserId && x.IsDeleted == false);
            //                break;
            //            case SecurityRoles.Jobseeker:
            //                result = dataHelper.Get<Interview>().Where(x => x.Tracking.JobseekerId == profile.UserId && x.IsDeleted == false);
            //                break;
            //        }

            //        if (Status != null)
            //        {
            //            result = result.Where(x => x.Status == Status.Value);
            //        }

            //        if (!string.IsNullOrEmpty(JobTitle))
            //        {
            //            result = result.Where(x => x.Tracking.Job != null && x.Tracking.Job.Title.ToLower().Contains(JobTitle.ToLower()));
            //        }

            //        if (CountryId != null)
            //        {
            //            result = result.Where(x =>
            //                        (x.Tracking.Job != null && x.Tracking.Job.CountryId == CountryId.Value) ||
            //                        (x.Tracking.Jobseeker != null && x.Tracking.Jobseeker.CountryId == CountryId.Value));
            //        }

            //        list = result.ToList();

            //        if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
            //        {
            //            var sdt = Convert.ToDateTime(StartDate);
            //            var edt = Convert.ToDateTime(EndDate);
            //            list =
            //                list.Where(x => x.DateUpdated.Value.Date >= sdt.Date && x.DateUpdated.Value.Date <= edt.Date)
            //                    .ToList();
            //        }

            //        ViewBag.JobList = new SelectList(list.Where(x => x.Tracking.JobId != null).Select(a => a.Tracking.Job.Title).Distinct().ToList());
            //    }
            //}

            //ViewBag.CountryList = new SelectList(SharedService.Instance.GetCountryList(), "Id", "Text");
            //rows = list.Count;
            //ViewBag.Rows = rows;
            //list = list.OrderByDescending(x => x.DateUpdated).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize)).Take(pageSize).ToList();
            //ViewBag.Model = new StaticPagedList<Interview>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
            if (Request.IsAjaxRequest())
            {
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View();
            }
        }

        [Authorize]
#pragma warning disable CS0246 // The type or namespace name 'InterviewFilter' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<ActionResult> JobTitleList(InterviewFilter filter)
#pragma warning restore CS0246 // The type or namespace name 'InterviewFilter' could not be found (are you missing a using directive or an assembly reference?)
        {
            filter.UserId = User.Id;
            filter.Type = User.Info.Type;
            var list = await _service.InterviewJobList(filter);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Authorize(Roles = "Employers, Jobseeker")]
        public ActionResult FollowUps(long Id)
        {
            var model = new Interview();
            if (User != null)
            {
                model = InterviewService.Instance.Get(Id);
                ViewBag.FollowUpList = InterviewService.Instance.GetFolloupList(Id);
            }
            return View(model);
        }

        [Authorize(Roles = "Employers, Jobseeker")]
        public ActionResult Update(long Id)
        {
            var model = new Interview();
            if (User != null)
            {
                UserProfile profile = MemberService.Instance.Get(User.Username);
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    model = dataHelper.GetSingle<Interview>(Id);
                    if (model != null)
                    {
                        if (profile.Type == (int)SecurityRoles.Jobseeker && profile.IsConfirmed == true)
                        {
                            if (model.Status != (int)InterviewStatus.WITHDRAW)
                            {
                                InterviewService.Instance.Connect(model.UserProfile.Username, profile.Username);
                            }
                        }
                        Interview FirstRound = model;
                        if (model.Round != 1)
                        {
                            FirstRound = InterviewService.Instance.Get(model.TrackingId, 1);
                        }

                        Interview SecondRound = InterviewService.Instance.Get(model.TrackingId, 2);
                        if (FirstRound != null)
                        {
                            ViewBag.First_FollowUpList = InterviewService.Instance.GetFolloupList(FirstRound.Id);
                            ViewBag.First_DiscussionList = InterviewService.Instance.GetDiscussions(FirstRound.Id);
                            ViewBag.First_NoteList = InterviewService.Instance.GetNotes(FirstRound.Id);
                        }

                        if (SecondRound != null)
                        {
                            ViewBag.Second_FollowUpList = InterviewService.Instance.GetFolloupList(SecondRound.Id);
                            ViewBag.Second_DiscussionList = InterviewService.Instance.GetDiscussions(SecondRound.Id);
                            ViewBag.Second_NoteList = InterviewService.Instance.GetNotes(SecondRound.Id);
                        }

                        var followups = dataHelper.Get<FollowUp>().Where(x => x.UserId != profile.UserId && x.Unread == true && x.InterviewId == Id);
                        var discussions = dataHelper.Get<InterviewDiscussion>().Where(x => x.UserId != profile.UserId && x.Unread == true && x.InterviewId == Id);

                        foreach (var item in followups)
                        {
                            item.Unread = false;
                            dataHelper.UpdateEntity<FollowUp>(item);
                        }

                        foreach (var item in discussions)
                        {
                            item.Unread = false;
                            dataHelper.UpdateEntity<InterviewDiscussion>(item);
                        }

                        if (followups.Count() > 0 || discussions.Count() > 0)
                        {
                            dataHelper.Save();
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Interview does not exist!";
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Start(long Id)
        {
            ViewBag.Jobseeker = new UserInfo(Id);
           

            if (User != null && !UserInfo.IsConfirmed)
            {                
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
            }

            if (User != null)
            {
                //if (DomainService.Instance.HasInterviewQuota(User.Id) == false)
                //{
                //    if (i == 0)
                //    {
                //        TempData["TrackingId"] = Id;
                //        TempData["Round"] = round;
                //        return RedirectToAction("InterviewPriceList", "Package", new { returnurl = Request.Url.ToString(), type = "I" });
                //    }
                //}
            }

            var model = new InterviewModel
            {
                JobseekerId = Id,
                Round = 1
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Start(InterviewModel model)
        {
            var employer = MemberService.Instance.Get(User.Username);
            var message = string.Empty;
            string subject = string.Empty;
            Interview interview = null;

            UserProfile jobSeeker = MemberService.Instance.Get(model.JobseekerId.Value);
            if (ModelState.IsValid)
            {
                if (model.Round == 1)
                {
                    Tracking tracking = null;
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);

                        tracking = new Tracking
                        {
                            Id = Guid.NewGuid(),
                            JobseekerId = model.JobseekerId,
                            Type = (int)TrackingTypes.INTERVIEW_INITIATED,
                            DateUpdated = DateTime.Now,
                            UserId = employer.UserId,
                            IsDownloaded = false
                        };

                        dataHelper.AddEntity<Tracking>(tracking, User.Username);
                       
                        TrackingDetail trackingDetails = new TrackingDetail
                        {
                            Id = tracking.Id,
                            Title = jobSeeker.Title,
                            CategoryId = jobSeeker.CategoryId.Value,
                            SpecializationId = jobSeeker.SpecializationId.Value,
                            CountryId = jobSeeker.CountryId.Value,
                            StateId = jobSeeker.StateId,
                            FileName = jobSeeker.FileName,
                            Content = jobSeeker.Content
                        };
                        dataHelper.AddEntity(trackingDetails);
                        dataHelper.Save();
                    }

                    interview = new Interview()
                    {
                        TrackingId = tracking.Id,
                        Interviewer = model.Interviewer,
                        InterviewDate = model.InterviewDate.Value,
                        InterviewTime = model.InterviewDate.Value.ToString("hh:mm tt"),
                        Description = model.Description,
                        Round = (int)InterviewRounds.FIRST_ROUND,
                        Status = (int)InterviewStatus.INITIATED,
                        UserId = employer.UserId
                    };

                    interview = InterviewService.Instance.Initiate(interview, User.Username);

                    var followUp = new FollowUp
                    {
                        InterviewId = interview.Id,
                        NewDate = interview.InterviewDate,
                        NewTime = interview.InterviewTime,
                        Message = model.Description,
                        Status = (int)FeedbackStatus.INVITED,
                        DateUpdated = DateTime.Now,
                        UserId = employer.UserId,
                        Unread= true
                    };

                    followUp = InterviewService.Instance.FollowUpEntry(followUp);
                }
                _service.ManageAccount(User.Id, null, null, 1, null, null, interview.Id);
                
                if (jobSeeker != null)
                {
                    string jTemplate = string.Empty;
                    string eTemplate = string.Empty;

                    bool connected = DomainService.Instance.IsConnected(jobSeeker.UserId, employer.UserId);
                    if (!connected)
                    {
                        InterviewService.Instance.Connect(jobSeeker.Username, User.Username);
                        jTemplate = Server.MapPath("~/Templates/Mail/interview_call_invitation.html");

                        using (var reader = new StreamReader(jTemplate))
                        {
                            var body = string.Empty;
                            body = reader.ReadToEnd();
                            body = body.Replace("@@firstname", jobSeeker.FirstName);
                            body = body.Replace("@@lastname", jobSeeker.LastName);

                            body = body.Replace("@@round", ((InterviewRounds)model.Round).GetDescription());
                            body = body.Replace("@@agenda", model.Description);
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));
                            body = body.Replace("@@accepturl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, interview.Id));
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FileName, employer.LastName)));
                            body = body.Replace("@@button", "Connect & View");
                            string[] receipent = { jobSeeker.Username };
                            subject = "Interview Invitation for Your Application";
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }

                        eTemplate = Server.MapPath("~/Templates/Mail/interview_call_invitation_employer.html");
                        using (var reader = new StreamReader(eTemplate))
                        {
                            var body = string.Empty;
                            body = reader.ReadToEnd();
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FileName, employer.LastName)));
                            body = body.Replace("@@firstname", jobSeeker.FirstName);
                            body = body.Replace("@@lastname", jobSeeker.LastName);
                            body = body.Replace("@@round", ((InterviewRounds)model.Round).GetDescription());
                            body = body.Replace("@@agenda", model.Description);
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, jobSeeker.PermaLink));
                            body = body.Replace("@@accepturl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, interview.Id));
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FileName, employer.LastName)));
                            body = body.Replace("@@button", "View");
                            string[] receipent = { employer.Username };
                            subject = "Interview Schedule";
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                    else
                    {
                        jTemplate = Server.MapPath("~/Templates/Mail/interview_call.html");
                        using (var reader = new StreamReader(jTemplate))
                        {
                            var body = reader.ReadToEnd();
                            body = body.Replace("@@firstname", jobSeeker.FirstName);
                            body = body.Replace("@@lastname", jobSeeker.LastName);
                            string[] receipent = { jobSeeker.Username };


                            body = body.Replace("@@round", ((InterviewRounds)model.Round).GetDescription());
                            body = body.Replace("@@agenda", model.Description);
                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, interview.Id));
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FileName, employer.LastName)));

                            subject = string.Format("Interview Schedule for {0} {1}", jobSeeker.FirstName, jobSeeker.LastName);
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }

                        eTemplate = Server.MapPath("~/Templates/Mail/employer_interview_call.html");
                        using (var reader = new StreamReader(eTemplate))
                        {
                            string body = reader.ReadToEnd();
                            body = body.Replace("@@firstname", jobSeeker.FirstName);
                            body = body.Replace("@@lastname", jobSeeker.LastName);
                            string[] receipent = { employer.Username };
                            body = body.Replace("@@round", ((InterviewRounds)model.Round).GetDescription());
                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, interview.Id));
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, jobSeeker.PermaLink));
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FileName, employer.LastName)));

                            subject = string.Format("Interview Schedule for {0} {1}", jobSeeker.FirstName, jobSeeker.LastName);

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                }
                TempData["UpdateData"] = string.Format("{0} initiated successfully!", ((InterviewRounds)model.Round).GetDescription());
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Initiate(Guid Id, int round = 1)
        {
            UserInfoEntity user = null;
            if (User != null)
            {
                user = _service.Get(User.Id);
                int i = jobService.VerifiedTokenI(User.Id);

                if (!user.IsConfirmed)
                {
                    return RedirectToAction("Confirm", "Account", new { id = user.Id, returnUrl = Request.Url.ToString() });
                }
             
                if (round == 1)
                {
                    if (DomainService.Instance.PaymentProcessEnabled())
                    {
                        if (DomainService.Instance.HasInterviewQuota(user.Id) == false)
                        {
                            if (i == 0)
                            {
                                TempData["TrackingId"] = Id;
                                TempData["Round"] = round;
                
                            return RedirectToAction("InterviewPriceList", "Package", new { returnurl = Request.Url.ToString(), type = "I" });
                            }
                        }
                    }
                }
            }
            var model = new InterviewModel
            {
                TrackingId = Id,
                Round = round
            };
            ViewBag.Record = TrackingService.Instance.Get(Id) as Tracking;

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Initiate(InterviewModel model)
        {
            var employer = MemberService.Instance.Get(User.Username);
            var message = string.Empty;
            string subject = string.Empty;
            var TrackingId = new Guid();
            Interview interview = null;

            var record = new Tracking();
            if (ModelState.IsValid)
            {
                if (model.Round == 1)
                {
                    record = TrackingService.Instance.Update(TrackingTypes.INTERVIEW_INITIATED, model.TrackingId, employer.Username, out message);
                    TrackingId = record.Id;
                    interview = InterviewService.Instance.Get(TrackingId);

                    if (interview == null)
                    {
                        interview = new Interview()
                        {
                            TrackingId = TrackingId,
                            Interviewer = model.Interviewer,
                            InterviewDate = model.InterviewDate.Value,
                            InterviewTime = model.InterviewDate.Value.ToString("hh:mm tt"),
                            Description = model.Description,
                            Round = (int)InterviewRounds.FIRST_ROUND,
                            Status = (int)InterviewStatus.INITIATED,
                            UserId = employer.UserId
                        };

                        interview = InterviewService.Instance.Initiate(interview, User.Username);

                        var followUp = new FollowUp
                        {
                            InterviewId = interview.Id,
                            NewDate = interview.InterviewDate,
                            NewTime = interview.InterviewTime,
                            Message = model.Description,
                            Status = (int)FeedbackStatus.INVITED,
                            DateUpdated = DateTime.Now,
                            UserId = employer.UserId,
                            Unread = true
                        };

                        followUp = InterviewService.Instance.FollowUpEntry(followUp);
                    }
                }
                else if (model.Round == 2)
                {
                    Interview first = InterviewService.Instance.Get(model.TrackingId.Value, 1);

                    if (first.Status != (int)InterviewStatus.COMPLETED)
                    {
                        first.Status = (int)InterviewStatus.COMPLETED;
                        first.IsDeleted = true;
                        InterviewService.Instance.Update(first, User.Username);
                    }

                    interview = new Interview()
                    {
                        TrackingId = model.TrackingId.Value,
                        Interviewer = model.Interviewer,
                        InterviewDate = model.InterviewDate.Value,
                        InterviewTime = model.InterviewDate.Value.ToString("hh:mm tt"),
                        Description = model.Description,
                        Round = (int)InterviewRounds.SECOND_ROUND,
                        Status = (int)InterviewStatus.INITIATED,
                        UserId = employer.UserId
                    };

                    interview = InterviewService.Instance.Initiate(interview, User.Username);

                    var followUp = new FollowUp
                    {
                        InterviewId = interview.Id,
                        NewDate = interview.InterviewDate,
                        NewTime = interview.InterviewTime,
                        Message = model.Description,
                        Status = (int)FeedbackStatus.INVITED,
                        DateUpdated = DateTime.Now,
                        UserId = employer.UserId,
                        Unread = true
                    };

                    followUp = InterviewService.Instance.FollowUpEntry(followUp);
                }

                if (model.Round == 1)
                {
                    _service.ManageAccount(User.Id, null, null, 1, null, null, interview.Id);
                }
                record = TrackingService.Instance.Get(interview.TrackingId);
                if (record.JobseekerId != null)
                {
                    string jTemplate = string.Empty;
                    string eTemplate = string.Empty;
                    var jobSeeker = MemberService.Instance.Get(record.JobseekerId.Value);
                    bool connected = ConnectionHelper.IsConnected(jobSeeker.Username, employer.UserId);
                    if (!connected)
                    {
                        InterviewService.Instance.Connect(jobSeeker.Username, User.Username);
                        if (record.JobId != null)
                        {

                            jTemplate = Server.MapPath("~/Templates/Mail/interview_call_for_job_invitation.html");
                        }
                        else
                        {
                            jTemplate = Server.MapPath("~/Templates/Mail/interview_call_invitation.html");
                        }


                        using (var reader = new StreamReader(jTemplate))
                        {
                            var body = string.Empty;
                            body = reader.ReadToEnd();
                            body = body.Replace("@@firstname", jobSeeker.FirstName);
                            body = body.Replace("@@lastname", jobSeeker.LastName);

                            if (record.JobId != null)
                            {
                                var job = JobService.Instance.Get(record.JobId.Value);
                                body = body.Replace("@@jobtitle", job.Title);

                                body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                            }

                            body = body.Replace("@@round", ((InterviewRounds)model.Round).GetDescription());
                            body = body.Replace("@@agenda", model.Description);
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));
                            body = body.Replace("@@accepturl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, interview.Id));
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FileName, employer.LastName)));
                            body = body.Replace("@@button", "Connect & View");
                            string[] receipent = { jobSeeker.Username };
                            subject = "Interview Invitation for Your Application";
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }


                        if (record.JobId != null)
                        {

                            eTemplate = Server.MapPath("~/Templates/Mail/interview_call_for_job_invitation_employer.html");
                        }
                        else
                        {
                            eTemplate = Server.MapPath("~/Templates/Mail/interview_call_invitation_employer.html");
                        }

                        using (var reader = new StreamReader(eTemplate))
                        {
                            var body = string.Empty;
                            body = reader.ReadToEnd();
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FileName, employer.LastName)));
                            body = body.Replace("@@firstname", jobSeeker.FirstName);
                            body = body.Replace("@@lastname", jobSeeker.LastName);

                            if (record.JobId != null)
                            {
                                var job = JobService.Instance.Get(record.JobId.Value);
                                body = body.Replace("@@jobtitle", job.Title);

                                body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                            }

                            body = body.Replace("@@round", ((InterviewRounds)model.Round).GetDescription());
                            body = body.Replace("@@agenda", model.Description);
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, jobSeeker.PermaLink));
                            body = body.Replace("@@accepturl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, interview.Id));
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FileName, employer.LastName)));
                            body = body.Replace("@@button", "View");
                            string[] receipent = { employer.Username };
                            subject = "Interview Schedule";
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                    else
                    {
                        if (record.JobId != null)
                        {
                            jTemplate = Server.MapPath("~/Templates/Mail/interview_call_for_job.html");
                        }
                        else
                        {
                            jTemplate = Server.MapPath("~/Templates/Mail/interview_call.html");
                        }
                        var reader = new StreamReader(jTemplate);
                        var body = string.Empty;

                        if (reader != null)
                        {
                            body = reader.ReadToEnd();
                            body = body.Replace("@@firstname", jobSeeker.FirstName);
                            body = body.Replace("@@lastname", jobSeeker.LastName);
                            string[] receipent = { jobSeeker.Username };

                            if (record.JobId != null)
                            {
                                var job = JobService.Instance.Get(record.JobId.Value);
                                body = body.Replace("@@jobtitle", job.Title);
                                body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));

                                body = body.Replace("@@round", ((InterviewRounds)model.Round).GetDescription());
                                body = body.Replace("@@agenda", model.Description);
                                body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, interview.Id));
                                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));
                                body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FileName, employer.LastName)));

                                subject = string.Format("Interview Schedule for {0}", job.Title);
                            }
                            else
                            {
                                body = body.Replace("@@round", ((InterviewRounds)model.Round).GetDescription());
                                body = body.Replace("@@agenda", model.Description);
                                body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, interview.Id));
                                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));
                                body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FileName, employer.LastName)));

                                subject = string.Format("Interview Schedule for {0} {1}", jobSeeker.FirstName, jobSeeker.LastName);
                            }
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }


                        if (record.JobId != null)
                        {
                            eTemplate = Server.MapPath("~/Templates/Mail/employer_interview_call_for_job.html");
                        }
                        else
                        {
                            eTemplate = Server.MapPath("~/Templates/Mail/employer_interview_call.html");
                        }
                        reader = new StreamReader(eTemplate);
                        body = string.Empty;

                        if (reader != null)
                        {
                            body = reader.ReadToEnd();
                            body = body.Replace("@@firstname", jobSeeker.FirstName);
                            body = body.Replace("@@lastname", jobSeeker.LastName);
                            string[] receipent = { employer.Username };

                            if (record.JobId != null)
                            {
                                var job = JobService.Instance.Get(record.JobId.Value);
                                body = body.Replace("@@jobtitle", job.Title);
                                body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));

                                body = body.Replace("@@round", ((InterviewRounds)model.Round).GetDescription());

                                body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, interview.Id));
                                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, jobSeeker.PermaLink));
                                body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FileName, employer.LastName)));

                                subject = string.Format("Interview Schedule for {0}", job.Title);
                            }
                            else
                            {
                                body = body.Replace("@@round", ((InterviewRounds)model.Round).GetDescription());

                                body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, interview.Id));
                                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, jobSeeker.PermaLink));
                                body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FileName, employer.LastName)));

                                subject = string.Format("Interview Schedule for {0} {1}", jobSeeker.FirstName, jobSeeker.LastName);
                            }
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }

                    }
                }
                TempData["UpdateData"] = string.Format("{0} initiated successfully!", ((InterviewRounds)model.Round).GetDescription());
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Reschedule(long Id, DateTime? NewDate, string Comments)
        {
            var model = new FollowUp();
            if (User != null)
            {
                var original = InterviewService.Instance.GetFollowUp(Id);
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    var profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);

                    model = dataHelper.Get<FollowUp>().Where(x => x.InterviewId == Id).OrderByDescending(x => x.DateUpdated).FirstOrDefault();
                    dataHelper.DeleteUpdate(model);

                    model = new FollowUp
                    {
                        InterviewId = Id,
                        NewDate = NewDate,
                        NewTime = NewDate.Value.ToString("hh:mm tt"),
                        Status = (int)FeedbackStatus.RESCHEDULE,
                        Message = Comments,
                        UserId = profile.UserId,
                        DateUpdated = DateTime.Now,
                        Unread = true
                    };
                    dataHelper.AddEntity(model);

                    dataHelper.Save();

                    Interview interview = dataHelper.GetSingle<Interview>(Id);
                    var job = interview.Tracking.Job;
                    var employer = interview.UserProfile;
                    var jobSeeker = interview.Tracking.Jobseeker;

                    string eTemplate = string.Empty;
                    string jTemplate = string.Empty;
                    string[] receipent = { string.Empty };


                    StreamReader reader;
                    var body = string.Empty;
                    string subject = string.Empty;
                    if (job != null)
                    {
                        if (profile.Type == (int)SecurityRoles.Jobseeker)
                        {
                            jTemplate = Server.MapPath("~/Templates/Mail/jobseeker_reschedule.html");
                            eTemplate = Server.MapPath("~/Templates/Mail/employer_reschedule.html");

                            reader = new StreamReader(jTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@firstname", profile.FirstName);
                            body = body.Replace("@@lastname", profile.LastName);
                            body = body.Replace("@@round", interview.Round.ToString());
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));

                            body = body.Replace("@@jobtitle", job.Title);
                            body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));

                            body = body.Replace("@@date", model.NewDate.Value.ToString("MMM-dd-yyyy"));
                            body = body.Replace("@@time", model.NewTime);
                            body = body.Replace("@@details", model.Message);
                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));


                            subject = string.Format("Interview Reschedule for {0}", job.Title);
                            receipent[0] = profile.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);


                            reader = new StreamReader(eTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                            body = body.Replace("@@firstname", profile.FirstName);
                            body = body.Replace("@@lastname", profile.LastName);
                            body = body.Replace("@@round", interview.Round.ToString());
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, jobSeeker.PermaLink));
                            body = body.Replace("@@jobtitle", job.Title);
                            body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));

                            body = body.Replace("@@date", model.NewDate.Value.ToString("MMM-dd-yyyy"));
                            body = body.Replace("@@time", model.NewTime);
                            body = body.Replace("@@details", model.Message);
                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                            receipent[0] = employer.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                        else
                        {
                            eTemplate = Server.MapPath("~/Templates/Mail/to_employer_reschedule.html");
                            jTemplate = Server.MapPath("~/Templates/Mail/from_employer_reschedule.html");

                            reader = new StreamReader(jTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@firstname", jobSeeker.FirstName);
                            body = body.Replace("@@lastname", jobSeeker.LastName);
                            body = body.Replace("@@round", interview.Round.ToString());
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));

                            body = body.Replace("@@jobtitle", job.Title);
                            body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));

                            body = body.Replace("@@date", model.NewDate.Value.ToString("MMM-dd-yyyy"));
                            body = body.Replace("@@time", model.NewTime);
                            body = body.Replace("@@details", model.Message);
                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                            subject = string.Format("Interview Reschedule for {0}", job.Title);
                            receipent[0] = jobSeeker.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);


                            reader = new StreamReader(eTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                            body = body.Replace("@@firstname", jobSeeker.FirstName);
                            body = body.Replace("@@lastname", jobSeeker.LastName);
                            body = body.Replace("@@round", interview.Round.ToString());
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, jobSeeker.PermaLink));
                            body = body.Replace("@@jobtitle", job.Title);
                            body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                            body = body.Replace("@@date", model.NewDate.Value.ToString("MMM-dd-yyyy"));
                            body = body.Replace("@@time", model.NewTime);
                            body = body.Replace("@@details", model.Message);

                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));


                            receipent[0] = employer.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                    else
                    {
                        if (profile.Type == (int)SecurityRoles.Jobseeker)
                        {
                            jTemplate = Server.MapPath("~/Templates/Mail/jobseeker_reschedule_js.html");
                            eTemplate = Server.MapPath("~/Templates/Mail/employer_reschedule_js.html");

                            reader = new StreamReader(jTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@firstname", profile.FirstName);
                            body = body.Replace("@@lastname", profile.LastName);
                            body = body.Replace("@@round", interview.Round.ToString());
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));

                            body = body.Replace("@@date", model.NewDate.Value.ToString("MMM-dd-yyyy"));
                            body = body.Replace("@@time", model.NewTime);
                            body = body.Replace("@@details", model.Message);
                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));


                            subject = "Interview Reschedule";
                            receipent[0] = profile.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);


                            reader = new StreamReader(eTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                            body = body.Replace("@@firstname", profile.FirstName);
                            body = body.Replace("@@lastname", profile.LastName);
                            body = body.Replace("@@round", interview.Round.ToString());
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, jobSeeker.PermaLink));

                            body = body.Replace("@@date", model.NewDate.Value.ToString("MMM-dd-yyyy"));
                            body = body.Replace("@@time", model.NewTime);
                            body = body.Replace("@@details", model.Message);
                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                            receipent[0] = employer.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                        else
                        {
                            eTemplate = Server.MapPath("~/Templates/Mail/to_employer_reschedule_js.html");
                            jTemplate = Server.MapPath("~/Templates/Mail/from_employer_reschedule_js.html");

                            reader = new StreamReader(jTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@firstname", jobSeeker.FirstName);
                            body = body.Replace("@@lastname", jobSeeker.LastName);
                            body = body.Replace("@@round", interview.Round.ToString());
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));

                            body = body.Replace("@@date", model.NewDate.Value.ToString("MMM-dd-yyyy"));
                            body = body.Replace("@@time", model.NewTime);
                            body = body.Replace("@@details", model.Message);
                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                            subject = "Interview Reschedule";
                            receipent[0] = jobSeeker.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);


                            reader = new StreamReader(eTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                            body = body.Replace("@@firstname", jobSeeker.FirstName);
                            body = body.Replace("@@lastname", jobSeeker.LastName);
                            body = body.Replace("@@round", interview.Round.ToString());
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, jobSeeker.PermaLink));

                            body = body.Replace("@@date", model.NewDate.Value.ToString("MMM-dd-yyyy"));
                            body = body.Replace("@@time", model.NewTime);
                            body = body.Replace("@@details", model.Message);

                            body = body.Replace("@@viewurl", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));


                            receipent[0] = employer.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                }
                TempData["UpdateData"] = "Recheduled successfully!";
            }
            return RedirectToAction("Update", new { Id });
        }

        [Authorize]
        public ActionResult Accept(long Id)
        {
            var model = new FollowUp();
            if (User != null)
            {
                var entity = InterviewService.Instance.GetFollowUp(Id);
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    var profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                    model = new FollowUp
                    {
                        InterviewId = Id,
                        NewDate = entity.NewDate,
                        NewTime = entity.NewTime,
                        Status = (int)FeedbackStatus.ACCEPTED,
                        UserId = profile.UserId,
                        DateUpdated = DateTime.Now,
                        Unread = true
                    };
                    dataHelper.AddEntity(model);
                    dataHelper.DeleteUpdate(entity);

                    var interview = dataHelper.GetSingle<Interview>(Id);
                    if (interview != null)
                    {
                        interview.InterviewDate = model.NewDate.Value;
                        interview.InterviewTime = model.NewTime;
                        interview.Status = (int)InterviewStatus.INTERVIEW_IN_PROGRESS;
                        interview.UpdatedBy = User.Username;
                        interview.DateUpdated = DateTime.Now;
                        dataHelper.UpdateEntity(interview);
                    }

                    var record = dataHelper.Get<Tracking>().Where(x => x.Id == interview.TrackingId).SingleOrDefault();
                    if (record != null)
                    {
                        record.Type = (int)TrackingTypes.INTERVIEW_IN_PROGRESS;
                        record.DateUpdated = DateTime.Now;
                        dataHelper.UpdateEntity(record);
                    }

                    if (record.Jobseeker != null)
                    {
                        UserProfile employer = dataHelper.GetSingle<UserProfile>("UserId", interview.UserId); // Contact
                        UserProfile jobSeeker = dataHelper.GetSingle<UserProfile>("Username", User.Username); // Logged in user
                        Connection connection = dataHelper.Get<Connection>().Where(x => x.UserId == jobSeeker.UserId && x.EmailAddress == employer.Username).SingleOrDefault();

                        if (connection != null)
                        {
                            Connection invitor = dataHelper.Get<Connection>().Where(x => x.UserId == employer.UserId && x.EmailAddress == jobSeeker.Username).SingleOrDefault();
                            if (invitor != null)
                            {
                                invitor.IsAccepted = true;
                                invitor.IsConnected = true;
                                invitor.Initiated = false;
                                invitor.DateUpdated = DateTime.Now;
                                invitor.UpdatedBy = User.Username;

                                dataHelper.UpdateEntity(invitor);
                            }
                            connection.IsAccepted = true;
                            connection.IsConnected = true;
                            connection.Initiated = false;
                            connection.DateUpdated = DateTime.Now;
                            connection.UpdatedBy = User.Username;

                            dataHelper.UpdateEntity(connection);
                        }

                        employer = interview.UserProfile;
                        jobSeeker = interview.Tracking.Jobseeker;

                        string eTemplate = string.Empty;
                        string jTemplate = string.Empty;
                        string[] receipent = { string.Empty };

                        StreamReader reader;
                        var body = string.Empty;
                        string subject = string.Empty;
                        if (interview.Tracking.Job != null)
                        {
                            Job job = interview.Tracking.Job;
                            if (profile.Type == (int)SecurityRoles.Jobseeker)
                            {
                                jTemplate = Server.MapPath("~/Templates/Mail/from_jobseeker_accepted.html");
                                eTemplate = Server.MapPath("~/Templates/Mail/to_employer_accepted.html");

                                reader = new StreamReader(jTemplate);
                                body = reader.ReadToEnd();

                                body = body.Replace("@@firstname", profile.FirstName);
                                body = body.Replace("@@lastname", profile.LastName);
                                body = body.Replace("@@jobtitle", job.Title);
                                body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                                body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                                subject = string.Format("Interview Accepted for {0}", job.Title);
                                receipent[0] = profile.Username;
                                AlertService.Instance.SendMail(subject, receipent, body);

                                reader = new StreamReader(eTemplate);
                                body = reader.ReadToEnd();

                                body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                                body = body.Replace("@@firstname", profile.FirstName);
                                body = body.Replace("@@lastname", profile.LastName);
                                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, jobSeeker.PermaLink));
                                body = body.Replace("@@jobtitle", job.Title);
                                body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                                body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                                subject = string.Format("Interview Accepted for {0}", job.Title);
                                receipent[0] = employer.Username;
                                AlertService.Instance.SendMail(subject, receipent, body);
                            }
                            else
                            {
                                eTemplate = Server.MapPath("~/Templates/Mail/from_employer_accepted.html");
                                jTemplate = Server.MapPath("~/Templates/Mail/to_jobseeker_accepted.html");

                                reader = new StreamReader(jTemplate);
                                body = reader.ReadToEnd();

                                body = body.Replace("@@firstname", jobSeeker.FirstName);
                                body = body.Replace("@@lastname", jobSeeker.LastName);

                                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));
                                body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));

                                body = body.Replace("@@jobtitle", job.Title);
                                body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                                body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));


                                subject = string.Format("Interview Accepted for {0}", job.Title);
                                receipent[0] = jobSeeker.Username;
                                AlertService.Instance.SendMail(subject, receipent, body);


                                reader = new StreamReader(eTemplate);
                                body = reader.ReadToEnd();

                                body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                                body = body.Replace("@@jobtitle", job.Title);
                                body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                                body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                                subject = string.Format("Interview Accepted for {0}", job.Title);

                                receipent[0] = employer.Username;
                                AlertService.Instance.SendMail(subject, receipent, body);
                            }
                        }
                        else
                        {
                            if (profile.Type == (int)SecurityRoles.Jobseeker)
                            {
                                jTemplate = Server.MapPath("~/Templates/Mail/from_jobseeker_accepted_js.html");
                                eTemplate = Server.MapPath("~/Templates/Mail/to_employer_accepted_js.html");

                                reader = new StreamReader(jTemplate);
                                body = reader.ReadToEnd();

                                body = body.Replace("@@firstname", profile.FirstName);
                                body = body.Replace("@@lastname", profile.LastName);
                                body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                                body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                                subject = "Interview Accepted";
                                receipent[0] = profile.Username;
                                AlertService.Instance.SendMail(subject, receipent, body);

                                reader = new StreamReader(eTemplate);
                                body = reader.ReadToEnd();

                                body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                                body = body.Replace("@@firstname", profile.FirstName);
                                body = body.Replace("@@lastname", profile.LastName);
                                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, jobSeeker.PermaLink));
                                body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                                subject = "Interview Accepted";
                                receipent[0] = employer.Username;
                                AlertService.Instance.SendMail(subject, receipent, body);
                            }
                            else
                            {
                                eTemplate = Server.MapPath("~/Templates/Mail/from_employer_accepted_js.html");
                                jTemplate = Server.MapPath("~/Templates/Mail/to_jobseeker_accepted_js.html");

                                reader = new StreamReader(jTemplate);
                                body = reader.ReadToEnd();

                                body = body.Replace("@@firstname", jobSeeker.FirstName);
                                body = body.Replace("@@lastname", jobSeeker.LastName);
                                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));
                                body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                                body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));


                                subject = "Interview Accepted";
                                receipent[0] = jobSeeker.Username;
                                AlertService.Instance.SendMail(subject, receipent, body);

                                reader = new StreamReader(eTemplate);
                                body = reader.ReadToEnd();

                                body = body.Replace("@@firstname", jobSeeker.FirstName);
                                body = body.Replace("@@lastname", jobSeeker.LastName);
                                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, jobSeeker.PermaLink));
                                body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));

                                body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                                subject = "Interview Accepted";

                                receipent[0] = employer.Username;
                                AlertService.Instance.SendMail(subject, receipent, body);
                            }
                        }
                        dataHelper.Save();
                    }
                }

                TempData["UpdateData"] = "Accepted successfully!";
            }
            return RedirectToAction("Update", new { Id });
        }


        [Authorize]
        public ActionResult Reject(long Id, string Reason, string redirect = "")
        {
            var model = new FollowUp();
            if (User != null)
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    var interview = dataHelper.GetSingle<Interview>(Id);
                    var entity = dataHelper.Get<FollowUp>().Where(x => x.InterviewId == Id).OrderByDescending(x => x.DateUpdated).FirstOrDefault();
                    var profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                    model = new FollowUp
                    {
                        InterviewId = Id,
                        NewDate = entity.NewDate,
                        NewTime = entity.NewTime,
                        Message = Reason,
                        Status = (int)FeedbackStatus.REJECTED,
                        UserId = profile.UserId,
                        DateUpdated = DateTime.Now,
                        Unread = true
                    };
                    dataHelper.AddEntity(model);

                    interview.DateUpdated = DateTime.Now;
                    interview.Status = (int)InterviewStatus.REJECTED;
                    interview.UpdatedBy = User.Username;

                    dataHelper.UpdateEntity<Interview>(interview);

                    var record = dataHelper.Get<Tracking>().Where(x => x.Id == interview.TrackingId).SingleOrDefault();
                    if (record != null)
                    {
                        record.Type = (int)TrackingTypes.REJECTED;
                        record.DateUpdated = DateTime.Now;
                        dataHelper.UpdateEntity(record);
                    }

                    dataHelper.DeleteUpdate(entity);
                    dataHelper.Save();

                    var job = interview.Tracking.Job;
                    var employer = interview.UserProfile;
                    var jobSeeker = interview.Tracking.Jobseeker;

                    string eTemplate = string.Empty;
                    string jTemplate = string.Empty;
                    string[] receipent = { string.Empty };

                    StreamReader reader;
                    var body = string.Empty;
                    string subject = string.Empty;
                    if (job != null)
                    {
                        if (profile.Type == (int)SecurityRoles.Jobseeker)
                        {
                            jTemplate = Server.MapPath("~/Templates/Mail/from_jobseeker_rejected.html");
                            eTemplate = Server.MapPath("~/Templates/Mail/to_employer_rejected.html");

                            reader = new StreamReader(jTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@firstname", profile.FirstName);
                            body = body.Replace("@@lastname", profile.LastName);
                            body = body.Replace("@@jobtitle", job.Title);
                            body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                            body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                            subject = string.Format("Interview Withdrawn for {0}", job.Title);
                            receipent[0] = profile.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);

                            reader = new StreamReader(eTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                            body = body.Replace("@@firstname", profile.FirstName);
                            body = body.Replace("@@lastname", profile.LastName);
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, jobSeeker.PermaLink));
                            body = body.Replace("@@jobtitle", job.Title);
                            body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                            body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                            subject = string.Format("Interview Withdrawn for {0}", job.Title);
                            receipent[0] = employer.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                        else
                        {
                            eTemplate = Server.MapPath("~/Templates/Mail/from_employer_rejected.html");
                            jTemplate = Server.MapPath("~/Templates/Mail/to_jobseeker_rejected.html");

                            reader = new StreamReader(jTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@firstname", jobSeeker.FirstName);
                            body = body.Replace("@@lastname", jobSeeker.LastName);

                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));

                            body = body.Replace("@@jobtitle", job.Title);
                            body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                            body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));


                            subject = string.Format("Interview is Rejected for {0}", job.Title);
                            receipent[0] = jobSeeker.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);


                            reader = new StreamReader(eTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                            body = body.Replace("@@jobtitle", job.Title);
                            body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                            body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                            subject = string.Format("Interview is Rejected for {0}", job.Title);

                            receipent[0] = employer.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                    else
                    {
                        if (profile.Type == (int)SecurityRoles.Jobseeker)
                        {
                            jTemplate = Server.MapPath("~/Templates/Mail/from_jobseeker_rejected_js.html");
                            eTemplate = Server.MapPath("~/Templates/Mail/to_employer_rejected_js.html");

                            reader = new StreamReader(jTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@firstname", profile.FirstName);
                            body = body.Replace("@@lastname", profile.LastName);

                            body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                            subject = "Interview Withdrawn";
                            receipent[0] = profile.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);

                            reader = new StreamReader(eTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                            body = body.Replace("@@firstname", profile.FirstName);
                            body = body.Replace("@@lastname", profile.LastName);
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, jobSeeker.PermaLink));

                            body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                            subject = "Interview Withdrawn";
                            receipent[0] = employer.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                        else
                        {
                            eTemplate = Server.MapPath("~/Templates/Mail/from_employer_rejected_js.html");
                            jTemplate = Server.MapPath("~/Templates/Mail/to_jobseeker_rejected_js.html");

                            reader = new StreamReader(jTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@firstname", jobSeeker.FirstName);
                            body = body.Replace("@@lastname", jobSeeker.LastName);

                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                            body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));


                            subject = "Interview is Rejected";
                            receipent[0] = jobSeeker.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);


                            reader = new StreamReader(eTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                            body = body.Replace("@@firstname", jobSeeker.FirstName);
                            body = body.Replace("@@lastname", jobSeeker.LastName);
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));
                            body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                            subject = "Interview is Rejected";

                            receipent[0] = employer.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                }
                TempData["UpdateData"] = "Rejected successfully!";
            }
            if (string.IsNullOrEmpty(redirect))
            {
                return RedirectToAction("Update", new { Id });
            }
            else
            {
                return Redirect(redirect);
            }
        }


        [Authorize]
        public ActionResult Withdraw(long Id, string Reason)
        {
            var model = new FollowUp();
            if (User != null)
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    var interview = dataHelper.GetSingle<Interview>(Id);
                    var entity = dataHelper.Get<FollowUp>().Where(x => x.InterviewId == Id).OrderByDescending(x => x.DateUpdated).FirstOrDefault();
                    var profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                    model = new FollowUp
                    {
                        InterviewId = Id,
                        NewDate = entity.NewDate,
                        NewTime = entity.NewTime,
                        Message = Reason,
                        Status = (int)FeedbackStatus.WITHDRAW,
                        UserId = profile.UserId,
                        DateUpdated = DateTime.Now,
                        Unread = true
                    };
                    dataHelper.AddEntity(model);

                    interview.DateUpdated = DateTime.Now;
                    interview.Status = (int)InterviewStatus.WITHDRAW;
                    interview.UpdatedBy = User.Username;

                    dataHelper.UpdateEntity<Interview>(interview);

                    var record = dataHelper.Get<Tracking>().Where(x => x.Id == interview.TrackingId).SingleOrDefault();
                    if (record != null)
                    {
                        record.Type = (int)TrackingTypes.INTERVIEW_WITHDRAW;
                        record.DateUpdated = DateTime.Now;
                        dataHelper.UpdateEntity(record);
                    }

                    dataHelper.DeleteUpdate(entity);
                    dataHelper.Save();


                    var job = interview.Tracking.Job;
                    var employer = interview.UserProfile;
                    var jobSeeker = interview.Tracking.Jobseeker;

                    string eTemplate = string.Empty;
                    string jTemplate = string.Empty;
                    string[] receipent = { string.Empty };

                    StreamReader reader;
                    var body = string.Empty;
                    string subject = string.Empty;

                    if (profile.Type == (int)SecurityRoles.Jobseeker)
                    {
                        if (job != null)
                        {
                            jTemplate = Server.MapPath("~/Templates/Mail/from_jobseeker_withdraw.html");
                            eTemplate = Server.MapPath("~/Templates/Mail/to_employer_withdraw.html");

                            reader = new StreamReader(jTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@firstname", profile.FirstName);
                            body = body.Replace("@@lastname", profile.LastName);
                            body = body.Replace("@@jobtitle", job.Title);
                            body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                            body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                            subject = string.Format("Interview Withdrawn for {0}", job.Title);
                            receipent[0] = profile.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);

                            reader = new StreamReader(eTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                            body = body.Replace("@@firstname", profile.FirstName);
                            body = body.Replace("@@lastname", profile.LastName);
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, jobSeeker.PermaLink));
                            body = body.Replace("@@jobtitle", job.Title);
                            body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                            body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                            subject = string.Format("Interview Withdrawn for {0}", job.Title);
                            receipent[0] = employer.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                        else
                        {
                            jTemplate = Server.MapPath("~/Templates/Mail/from_jobseeker_withdraw_js.html");
                            eTemplate = Server.MapPath("~/Templates/Mail/to_employer_withdraw_js.html");

                            reader = new StreamReader(jTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@firstname", profile.FirstName);
                            body = body.Replace("@@lastname", profile.LastName);
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                            body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                            subject = "Interview Withdrawn";
                            receipent[0] = profile.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);

                            reader = new StreamReader(eTemplate);
                            body = reader.ReadToEnd();

                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName)));
                            body = body.Replace("@@firstname", profile.FirstName);
                            body = body.Replace("@@lastname", profile.LastName);
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, jobSeeker.PermaLink));
                            body = body.Replace("@@url", string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority, Id));

                            subject = "Interview Withdrawn";
                            receipent[0] = employer.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }

                    TempData["UpdateData"] = "Withdrawn successfully!";
                }
            }
            return RedirectToAction("Update", new { Id });
        }

        /// <summary>
        /// Add note for the interview.
        /// </summary>
        /// <param name="Id">Interview Id</param>
        /// <returns></returns>
        public ActionResult AddNote(long Id, string Comments, string Name)
        {
            if (User != null)
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    var profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);

                    var note = new InterviewNote
                    {
                        InterviewId = Id,
                        Comments = Comments,
                        NoteTaker = Name,
                        UserId = profile.UserId,
                        DateUpdated = DateTime.Now
                    };

                    dataHelper.Add(note);
                }
            }
            return Redirect("/Interview/Update/" + Id + "#Notes");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Send(long Id, string Message)
        {
            var model = new InterviewDiscussion();
            if (User != null)
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    var profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                    var interview = dataHelper.GetSingle<Interview>(Id);
                    model = new InterviewDiscussion
                    {
                        InterviewId = Id,
                        Message = Message,
                        UserId = profile.UserId,
                        DateUpdated = DateTime.Now,
                        Unread = true
                    };
                    dataHelper.Add(model);
                    if (profile.Type == (int)SecurityRoles.Employers)
                    {
                        var reader = new StreamReader(Server.MapPath("~/Templates/Mail/employer_message.html"));
                        var body = string.Empty;

                        if (reader != null)
                        {
                            body = reader.ReadToEnd();
                            body = body.Replace("@@firstname", interview.Tracking.Jobseeker.FirstName);
                            body = body.Replace("@@lastname", interview.Tracking.Jobseeker.LastName);
                            //body = body.Replace("@@message", Message);
                            body = body.Replace("@@employer", interview.Tracking.Job.Employer.Company);
                            body = body.Replace("@@profileurl",
                                string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority,
                                    interview.UserProfile.PermaLink));
                            body = body.Replace("@@viewurl",
                               string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority,
                                   interview.Id));

                            string[] receipent = { interview.Tracking.Jobseeker.Username };
                            var subject = string.Format("Interview Message from {0}",
                                (!string.IsNullOrEmpty(interview.UserProfile.Company) ? interview.UserProfile.Company : string.Format("{0} {1}", interview.UserProfile.FirstName, interview.UserProfile.LastName)));

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                    else if (profile.Type == (int)SecurityRoles.Jobseeker)
                    {
                        var reader = new StreamReader(Server.MapPath("~/Templates/Mail/jobseeker_message.html"));
                        var body = string.Empty;

                        if (reader != null)
                        {
                            body = reader.ReadToEnd();
                            body = body.Replace("@@employer", (!string.IsNullOrEmpty(interview.UserProfile.Company) ? interview.UserProfile.Company : string.Format("{0} {1}", interview.UserProfile.FirstName, interview.UserProfile.LastName)));
                            //body = body.Replace("@@message", Message);
                            body = body.Replace("@@firstname", interview.Tracking.Jobseeker.FirstName);
                            body = body.Replace("@@lastname", interview.Tracking.Jobseeker.LastName);
                            body = body.Replace("@@profileurl",
                                string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority,
                                    interview.Tracking.Jobseeker.PermaLink));
                            body = body.Replace("@@viewurl",
                                string.Format("{0}://{1}/Interview/Update?Id={2}", Request.Url.Scheme, Request.Url.Authority,
                                    interview.Id));


                            string[] receipent = { interview.UserProfile.Username };
                            var subject = string.Format("Interview Message from {0} {1}.",
                                interview.Tracking.Jobseeker.FirstName, interview.Tracking.Jobseeker.LastName);

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                }
                TempData["UpdateData"] = "Message sent successfully!";
            }
            return Redirect("/Interview/Update/" + Id + "#Messages");
        }

        /// <summary>
        ///     Reject application
        /// </summary>
        /// <param name="JobId"></param>
        /// <param name="ResumeId"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult RejectJobseeker(long Id)
        {
            var message = string.Empty;
            var interview = new Interview();
            UserProfile profile = MemberService.Instance.Get(User.Username);

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                interview = dataHelper.GetSingle<Interview>(Id);
                var entity = dataHelper.Get<FollowUp>().Where(x => x.InterviewId == interview.Id).OrderByDescending(x => x.DateUpdated).FirstOrDefault();
                FollowUp model = new FollowUp
                {
                    InterviewId = Id,
                    NewDate = entity.NewDate,
                    NewTime = entity.NewTime,
                    Message = "Reject",
                    Status = (int)FeedbackStatus.REJECTED,
                    UserId = profile.UserId,
                    DateUpdated = DateTime.Now,
                    Unread = true
                };
                dataHelper.AddEntity(model);

                interview.Status = (int)InterviewStatus.REJECTED;
                interview.DateUpdated = DateTime.Now;
                interview.UpdatedBy = User.Username;
                dataHelper.UpdateEntity(interview);

                dataHelper.Save();
            }

            EmployerService.Instance.Reject(interview.TrackingId, User.Username, out message);
            TempData["UpdateData"] = message;

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Select(long Id)
        {
            var message = string.Empty;
            Guid? trackingId = null;
            try
            {                
                var interview = new Interview();
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    interview = dataHelper.GetSingle<Interview>(Id);
                    trackingId = interview.TrackingId;
                    interview.Status = (int)InterviewStatus.SELECTED;
                    dataHelper.Update(interview);
                }

                var followUp = new FollowUp
                {
                    InterviewId = interview.Id,
                    NewDate = interview.InterviewDate,
                    NewTime = interview.InterviewTime,
                    Message = "Selected",
                    Status = (int)FeedbackStatus.SELECTED,
                    DateUpdated = DateTime.Now,
                    UserId = interview.UserId,
                    Unread = true
                };

                followUp = InterviewService.Instance.FollowUpEntry(followUp);
                if (trackingId != null)
                {
                    EmployerService.Instance.Select(trackingId.Value, User.Username, out message);
                }
                message = "Selected successfully!";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Delete(long Id)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var interview = dataHelper.GetSingle<Interview>(Id);
                dataHelper.Delete(interview);
                TempData["UpdateData"] = "Interview record has been deleted successfully!";
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult DeleteNote(long interviewId, int Id)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var note = dataHelper.GetSingle<InterviewNote>(Id);
                dataHelper.Delete<InterviewNote>(note);
            }
            return RedirectToAction("Update", new { Id = interviewId });
        }
    }
}