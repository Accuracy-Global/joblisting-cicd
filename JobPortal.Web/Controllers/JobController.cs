using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Web.Models;
using PagedList;
using System.Collections;
using System.Collections.Generic;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Helpers;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Web;
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Threading.Tasks;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using static JobPortal.Domain.MemberService;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Data;
using JobPortal.Web.Models.Pagination;
using System.Threading;
using System.Globalization;
using System.Net;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Web.Helpers;
using System.Text.RegularExpressions;
using Twilio;
using MimeKit;
using JobPortal.Web.App_Start;

namespace JobPortal.Web.Controllers
{
    [RoutePrefix("Jobs-in-")]
    public class JobController : BaseController
    {
#pragma warning disable CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
        IJobService iJobService;
#pragma warning restore CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IManageService' could not be found (are you missing a using directive or an assembly reference?)
        IManageService iManageService;
#pragma warning restore CS0246 // The type or namespace name 'IManageService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'ITrackService' could not be found (are you missing a using directive or an assembly reference?)
        ITrackService iTrackingService;
#pragma warning restore CS0246 // The type or namespace name 'ITrackService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IManageService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'ITrackService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
        public JobController(IUserService service, IJobService jobService, IManageService iManageService, ITrackService iTrackingService)
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'ITrackService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IManageService' could not be found (are you missing a using directive or an assembly reference?)
            : base(service)
        {
            this.iJobService = jobService;
            this.iManageService = iManageService;
            this.iTrackingService = iTrackingService;
        }
        /// <summary>
        /// Bookmarks job.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Bookmark(long Id, string redirect = null)
        {
            if (UserInfo != null && UserInfo.IsConfirmed == false)
            {
                //return RedirectToAction("ConfirmRegistration", "Account");
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
            }

            var message = string.Empty;
            var profile = MemberService.Instance.Get(User.Username);
            if (profile != null && profile.Type == (int)SecurityRoles.Jobseeker)
            {
                //if (!string.IsNullOrEmpty(profile.Title) && profile.CategoryId != null && profile.SpecializationId != null)
                //{
                //var bookmark = TrackingService.Instance.Bookmark(Id, BookmarkedTypes.JOB, User.Username, out message);
                var bookmark = iTrackingService.Bookmark(Id, (int)BookmarkedTypes.JOB, profile.UserId, out message);
                TempData["UpdateData"] = message;
                //}
                //else
                //{
                //    message = "For bookmarking, kindly complete your <a href=\"/profile-resume\">profile</a> by updating following fields:<br/>";
                //    message += "<ul>";
                //    if (string.IsNullOrEmpty(profile.Title))
                //    {
                //        message += "<li>Title</li>";
                //    }

                //    if (profile.CategoryId == null)
                //    {
                //        message += "<li>Category</li>";
                //    }

                //    if (profile.SpecializationId == null)
                //    {
                //        message += "<li>Specialization</li>";
                //    }
                //    message += "</ul>";
                //    message += "<a href=\"/profile-resume\">CLICK HERE</a> to update now!";
                //    TempData["Error"] = message;
                //}
            }
            else
            {
                message = "Only Jobseeker(s) can bookmark the job!";
                TempData["UpdateData"] = message;
            }

            //if (redirect.Contains("jobs"))
            //{
            return RedirectToAction("BookMarkedJobs", "Jobseeker");
            //}
            //return Redirect(redirect);
        }

        public ActionResult ViewJob(long Id)
        {
            ViewJobModel job = new ViewJobModel();
            if (User != null)
            {
                job = iJobService.Get(Id, User.Id);
            }
            else
            {
                job = iJobService.Get(Id);
            }
            var message = string.Empty;



            if (User != null && User.Info.Role == SecurityRoles.Jobseeker)
            {
                if (job != null)
                {
                    Tracking record = TrackingService.Instance.JobViewed(Id, User.Username, out message);
                }
            }

            UserInfoEntity employer = _service.Get(job.EmployerId);
            if (job == null)
            {
                return RedirectToAction("JobStatus", new { Id });
            }

            if (job != null)
            {
                if (User != null && User.Info.Role == SecurityRoles.Employers)
                {
                    ViewBag.ShowApplyButton = false;
                }
                else
                {
                    ViewBag.ShowApplyButton = true;
                }
                if (!string.IsNullOrEmpty(job.Description))
                {
                    job.Description = job.Description.RemoveNumbers();
                    job.Description = job.Description.RemoveEmails();
                    job.Description = job.Description.RemoveWebsites();
                }

                if (!string.IsNullOrEmpty(job.NewDescription))
                {
                    job.NewDescription = job.NewDescription.RemoveNumbers();
                    job.NewDescription = job.NewDescription.RemoveEmails();
                    job.NewDescription = job.NewDescription.RemoveWebsites();
                }

                if (!string.IsNullOrEmpty(job.Requirements))
                {
                    job.Requirements = job.Requirements.RemoveNumbers();
                    job.Requirements = job.Requirements.RemoveEmails();
                    job.Requirements = job.Requirements.RemoveWebsites();
                }

                if (!string.IsNullOrEmpty(job.NewRequirements))
                {
                    job.NewRequirements = job.NewRequirements.RemoveNumbers();
                    job.NewRequirements = job.NewRequirements.RemoveEmails();
                    job.NewRequirements = job.NewRequirements.RemoveWebsites();
                }
                if (!string.IsNullOrEmpty(job.Responsibilities))
                {
                    job.Responsibilities = job.Responsibilities.RemoveNumbers();
                    job.Responsibilities = job.Responsibilities.RemoveEmails();
                    job.Responsibilities = job.Responsibilities.RemoveWebsites();
                }

                if (!string.IsNullOrEmpty(job.NewResponsibilities))
                {
                    job.NewResponsibilities = job.NewResponsibilities.RemoveNumbers();
                    job.NewResponsibilities = job.NewResponsibilities.RemoveEmails();
                    job.NewResponsibilities = job.NewResponsibilities.RemoveWebsites();
                }
                return View(job);
            }

            return RedirectToAction("JobView", new { Id });
        }

        [UrlPrivilegeFilter]
        public ActionResult JobView(long Id)
        {
            DateTime dts = DateTime.Now;
            ViewJobModel job = new ViewJobModel();
            if (User != null)
            {
                job = iJobService.Get(Id, User.Id);
            }
            else
            {
                job = iJobService.Get(Id);
            }

            if (User != null)
            {
                UserProfile profile = MemberService.Instance.Get(User.Username);
                if (job != null && job.IsRejected == true)
                {
                    switch ((SecurityRoles)profile.Type)
                    {
                        case SecurityRoles.Jobseeker:
                            return RedirectToAction("JobStatus", new { Id });
                        case SecurityRoles.Employers:
                            if (job.EmployerId != profile.UserId)
                            {
                                return RedirectToAction("JobStatus", new { Id });
                            }
                            break;
                        case SecurityRoles.Administrator:
                            break;
                    }
                }

                if (job != null && job.IsDeleted == true)
                {
                    if ((SecurityRoles)profile.Type != SecurityRoles.Administrator && (SecurityRoles)profile.Type != SecurityRoles.SuperUser)
                    {
                        switch ((SecurityRoles)profile.Type)
                        {
                            case SecurityRoles.Jobseeker:
                                return RedirectToAction("JobStatus", new { Id });
                            case SecurityRoles.Employers:
                                if (job.EmployerId != profile.UserId)
                                {
                                    return RedirectToAction("JobStatus", new { Id });
                                }
                                break;
                            case SecurityRoles.Administrator:
                                break;
                            case SecurityRoles.SuperUser:
                                break;
                        }
                    }
                }

                if (job != null && job.IsActive == false)
                {
                    switch ((SecurityRoles)profile.Type)
                    {
                        case SecurityRoles.Jobseeker:
                            return RedirectToAction("JobStatus", new { Id });
                        case SecurityRoles.Employers:
                            if (job.EmployerId != profile.UserId)
                            {
                                return RedirectToAction("JobStatus", new { Id });
                            }
                            break;
                        case SecurityRoles.Administrator:
                            break;
                    }
                }

                if (job != null && job.IsPublished == false)
                {
                    if (!job.InEditMode)
                    {
                        switch ((SecurityRoles)profile.Type)
                        {
                            case SecurityRoles.Jobseeker:
                                return RedirectToAction("JobStatus", new { Id });
                            case SecurityRoles.Employers:
                                if (job.EmployerId != profile.UserId)
                                {
                                    return RedirectToAction("JobStatus", new { Id });
                                }
                                break;
                            case SecurityRoles.Administrator:
                                break;
                        }
                    }
                }
            }
            else
            {
                if (job != null && job.IsRejected == true)
                {
                    return RedirectToAction("JobStatus", new { Id });
                }
                if (job != null && job.IsDeleted == true)
                {
                    return RedirectToAction("JobStatus", new { Id });
                }
                if (job != null && job.IsActive == false)
                {
                    return RedirectToAction("JobStatus", new { Id });
                }
                if (job != null && job.IsPublished == false)
                {
                    if (job.InEditMode == false)
                    {
                        return RedirectToAction("JobStatus", new { Id });
                    }
                }
            }

            if (job != null)
            {
                if (User != null && User.Info.Role == SecurityRoles.Employers)
                {
                    ViewBag.ShowApplyButton = false;
                }
                else
                {
                    ViewBag.ShowApplyButton = true;
                }
                if (!string.IsNullOrEmpty(job.Description))
                {
                    job.Description = job.Description.RemoveNumbers();
                    job.Description = job.Description.RemoveEmails();
                    job.Description = job.Description.RemoveWebsites();
                }

                if (!string.IsNullOrEmpty(job.NewDescription))
                {
                    job.NewDescription = job.NewDescription.RemoveNumbers();
                    job.NewDescription = job.NewDescription.RemoveEmails();
                    job.NewDescription = job.NewDescription.RemoveWebsites();
                }

                if (!string.IsNullOrEmpty(job.Requirements))
                {
                    job.Requirements = job.Requirements.RemoveNumbers();
                    job.Requirements = job.Requirements.RemoveEmails();
                    job.Requirements = job.Requirements.RemoveWebsites();
                }

                if (!string.IsNullOrEmpty(job.NewRequirements))
                {
                    job.NewRequirements = job.NewRequirements.RemoveNumbers();
                    job.NewRequirements = job.NewRequirements.RemoveEmails();
                    job.NewRequirements = job.NewRequirements.RemoveWebsites();
                }
                if (!string.IsNullOrEmpty(job.Responsibilities))
                {
                    job.Responsibilities = job.Responsibilities.RemoveNumbers();
                    job.Responsibilities = job.Responsibilities.RemoveEmails();
                    job.Responsibilities = job.Responsibilities.RemoveWebsites();
                }

                if (!string.IsNullOrEmpty(job.NewResponsibilities))
                {
                    job.NewResponsibilities = job.NewResponsibilities.RemoveNumbers();
                    job.NewResponsibilities = job.NewResponsibilities.RemoveEmails();
                    job.NewResponsibilities = job.NewResponsibilities.RemoveWebsites();
                }
                return View(job);
            }


            return RedirectToAction("JobView", new { Id });
        }
        public ActionResult GetJob(long Id)
        {
            ViewJobModel job = new ViewJobModel();
            if (User != null)
            {
                job = iJobService.Get(Id, User.Id);
            }
            else
            {
                job = iJobService.Get(Id);
            }

            if (!string.IsNullOrEmpty(job.Description))
            {
                job.Description = job.Description.RemoveNumbers();
                job.Description = job.Description.RemoveEmails();
                job.Description = job.Description.RemoveWebsites();
            }

            if (!string.IsNullOrEmpty(job.NewDescription))
            {
                job.NewDescription = job.NewDescription.RemoveNumbers();
                job.NewDescription = job.NewDescription.RemoveEmails();
                job.NewDescription = job.NewDescription.RemoveWebsites();
            }

            if (!string.IsNullOrEmpty(job.Requirements))
            {
                job.Requirements = job.Requirements.RemoveNumbers();
                job.Requirements = job.Requirements.RemoveEmails();
                job.Requirements = job.Requirements.RemoveWebsites();
            }

            if (!string.IsNullOrEmpty(job.NewRequirements))
            {
                job.NewRequirements = job.NewRequirements.RemoveNumbers();
                job.NewRequirements = job.NewRequirements.RemoveEmails();
                job.NewRequirements = job.NewRequirements.RemoveWebsites();
            }
            if (!string.IsNullOrEmpty(job.Responsibilities))
            {
                job.Responsibilities = job.Responsibilities.RemoveNumbers();
                job.Responsibilities = job.Responsibilities.RemoveEmails();
                job.Responsibilities = job.Responsibilities.RemoveWebsites();
            }

            if (!string.IsNullOrEmpty(job.NewResponsibilities))
            {
                job.NewResponsibilities = job.NewResponsibilities.RemoveNumbers();
                job.NewResponsibilities = job.NewResponsibilities.RemoveEmails();
                job.NewResponsibilities = job.NewResponsibilities.RemoveWebsites();
            }

            return Json(job, JsonRequestBehavior.AllowGet);
        }

        public ActionResult JobStatus(long Id)
        {
            if (Request.IsAuthenticated)
            {
                TempData["Error"] = "At present this job is not available!";
            }
            else
            {
                return Redirect("/account/login");
            }
            return View();
        }
        public ActionResult Thanks()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult PreApply(long Id, string returnUrl)
        {

            Job job = JobService.Instance.Get(Id);

            UserProfile member = MemberService.Instance.Get(User.Username);
            DataTable GetUser = MemberService.Instance.GetUserval(User.Id);
            ViewBag.JobTitle = job.Title;
            int CategoryId = 0;
            int SpecializationId = 0;
            if (GetUser.Rows[0]["CategoryId"].ToString() != "")
            {
                CategoryId = Convert.ToInt32(GetUser.Rows[0]["CategoryId"]);
            }
            if (GetUser.Rows[0]["SpecializationId"].ToString() != "")
            {
                SpecializationId = Convert.ToInt32(GetUser.Rows[0]["SpecializationId"]);
            }

            PreJobApplyModel model = new PreJobApplyModel()
            {
                Title = GetUser.Rows[0]["Title"].ToString(),
                CategoryId = CategoryId,
                SpecializationId = SpecializationId,
                MobileCountryCode = GetUser.Rows[0]["MobileCountryCode"].ToString(),
                Telephone = GetUser.Rows[0]["Mobile"].ToString(),
                ReturnUrl = returnUrl
            };
            List Country = new List();
            if (member.CountryId != null)
            {
                Country = SharedService.Instance.GetCountry(member.CountryId.Value);
                model.MobileCountryCode = string.Format("+{0}", Country.Code);
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult PreApply(PreJobApplyModel model)
        {
            UserProfile original = MemberService.Instance.Get(User.Id);
            DataTable GetUser = MemberService.Instance.GetUserval(User.Id);

            if (ModelState.IsValid)
            {
                //var fileSize = Convert.ToInt32(ConfigService.Instance.GetConfigValue("UploadFileSize"));
                //var actualFileSize = fileSize * 1024;                
                //HttpPostedFileBase rFile = model.Resume;
                //if (rFile != null && rFile.ContentLength > actualFileSize)
                //{
                //    TempData["Error"] = string.Format("Your resume size exceeds the upload limit({0} KB).", fileSize);
                //}
                //else
                //{                    
                //if (rFile != null)
                //{
                //    string fileName = rFile.FileName;
                //    if (fileName.Contains("\\"))
                //    {
                //        fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                //    }
                //    var extension = fileName.Substring(fileName.LastIndexOf(".") + 1).ToUpper();
                //    if (extension == "PDF" || extension == "DOC" || extension == "DOCX")
                //    {
                //        var file = rFile.InputStream;
                //        var buffer = new byte[file.Length];
                //        file.Read(buffer, 0, (int)file.Length);
                //        file.Close();

                //        original.Content = Convert.ToBase64String(buffer);
                //        original.FileName = fileName;
                //    }
                //    else
                //    {
                //        TempData["Error"] = "Only .doc, .docx, .pdf files are allowed!";
                //        return View(model);
                //    }
                //}


                //GetUser.Title = model.Title;
                //GetUser.CategoryId = model.CategoryId;
                //GetUser.SpecializationId = model.SpecializationId;
                //GetUser.Employer = model.CurrentEmployer;
                //GetUser.FromMonth = model.FromMonth;
                //GetUser.FromYr = model.FromYear;
                //GetUser.ToMonth = model.ToMonth;
                //GetUser.ToYr = model.ToYear;
                //    if (!string.IsNullOrEmpty(model.MobileCountryCode))
                //    {
                //    GetUser.MobileCountryCode = model.MobileCountryCode;
                //    GetUser.Mobile = model.Telephone;
                //    }
                //GetUser.Responsibilities = model.Summary;
                //    using (JobPortalEntities context = new JobPortalEntities())
                //    {
                //        DataHelper dataHelper = new DataHelper(context);
                //        dataHelper.Update<UserProfile>(original, User.Username);
                //    }

                //if (!string.IsNullOrEmpty(GetUser.Rows[0]["Title"].ToString()))
                //{
                return Redirect(model.ReturnUrl);
                //}


                //}
            }

            //if (string.IsNullOrEmpty(original.Content))
            //{
            //TempData["Error"] = "Please upload or build your resume online!";
            //}
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Preview(string returnUrl)
        {
            UserProfile member = MemberService.Instance.Get(User.Id);
            DataTable GetUser = MemberService.Instance.GetUserval(User.Id);
            int CategoryId = 0;
            int SpecializationId = 0;
            if (GetUser.Rows[0]["CategoryId"].ToString() != "")
            {
                CategoryId = Convert.ToInt32(GetUser.Rows[0]["CategoryId"]);
            }
            if (GetUser.Rows[0]["SpecializationId"].ToString() != "")
            {
                SpecializationId = Convert.ToInt32(GetUser.Rows[0]["SpecializationId"]);
            }

            PreJobApplyModel model = new PreJobApplyModel()
            {
                Title = GetUser.Rows[0]["Title"].ToString(),
                CategoryId = CategoryId,
                SpecializationId = SpecializationId,
                MobileCountryCode = GetUser.Rows[0]["MobileCountryCode"].ToString(),
                Telephone = GetUser.Rows[0]["Mobile"].ToString(),
                ReturnUrl = returnUrl
            };

            List Country = new List();
            if (member.CountryId != null)
            {
                Country = SharedService.Instance.GetCountry(member.CountryId.Value);
                model.MobileCountryCode = string.Format("+{0}", Country.Code);
            }
            if (!User.Info.IsJobseeker)
            {
                return View(model);
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        [Authorize]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Preview(PreJobApplyModel model)
        {
            UserProfile original = MemberService.Instance.Get(User.Id);

            DataTable GetUser = MemberService.Instance.GetUserval(User.Id);
            if (ModelState.IsValid)
            {
                //var fileSize = Convert.ToInt32(ConfigService.Instance.GetConfigValue("UploadFileSize"));
                //var actualFileSize = fileSize * 1024;
                //HttpPostedFileBase rFile = model.Resume;
                //if (rFile != null && rFile.ContentLength > actualFileSize)
                //{
                //    TempData["Error"] = string.Format("Your resume size exceeds the upload limit({0} KB).", fileSize);
                //}
                //else
                //{
                //    if (rFile != null)
                //    {
                //        string fileName = rFile.FileName;
                //        if (fileName.Contains("\\"))
                //        {
                //            fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                //        }
                //        var extension = fileName.Substring(fileName.LastIndexOf(".") + 1).ToUpper();
                //        if (extension == "PDF" || extension == "DOC" || extension == "DOCX")
                //        {
                //            var file = rFile.InputStream;
                //            var buffer = new byte[file.Length];
                //            file.Read(buffer, 0, (int)file.Length);
                //            file.Close();

                //            original.Content = Convert.ToBase64String(buffer);
                //            original.FileName = fileName;
                //        }
                //        else
                //        {
                //            TempData["Error"] = "Only .doc, .docx, .pdf files are allowed!";
                //            return View(model);
                //        }
                //    }
                //GetUser.Title = model.Title;
                //GetUser.CategoryId = model.CategoryId;
                //GetUser.SpecializationId = model.SpecializationId;
                //GetUser.Employer = model.CurrentEmployer;
                //GetUser.FromMonth = model.FromMonth;
                //GetUser.FromYr = model.FromYear;
                //GetUser.ToMonth = model.ToMonth;
                //GetUser.ToYr = model.ToYear;

                //if (!string.IsNullOrEmpty(model.MobileCountryCode))
                //{
                //    GetUser.MobileCountryCode = model.MobileCountryCode;
                //    GetUser.Mobile = model.Telephone;
                //}
                //GetUser.Responsibilities = model.Summary;
                //using (JobPortalEntities context = new JobPortalEntities())
                //{
                //    DataHelper dataHelper = new DataHelper(context);
                //    dataHelper.Update<UserProfile>(original, User.Username);
                //}

                //if (!string.IsNullOrEmpty(GetUser.Rows[0]["Title"].ToString()))
                //{
                return Redirect(model.ReturnUrl);
                //}
                //}
            }

            //if (string.IsNullOrEmpty(original.Content))
            //{
            //    TempData["Error"] = "Please upload or build your resume online!";
            //}
            return View(model);
        }

        private void Send(Job job, UserProfile employer)
        {
            UserInfoEntity user = User.Info;
            using (var reader = new StreamReader(HttpContext.Server.MapPath("~/Templates/Mail/apply.html")))
            {
                string body = reader.ReadToEnd();
                body = body.Replace("@@firstname", user.FullName);
                body = body.Replace("@@lastname", " ");
                body = body.Replace("@@jobtitle", job.Title);
                body = body.Replace("@@joburl",
                    string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                body = body.Replace("@@companyprofile",
                    string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));
                body = body.Replace("@@employer", employer.Company);

                string[] receipent = { user.Username };
                var subject = string.Format("Application for {0}", job.Title);

                AlertService.Instance.SendMail(subject, receipent, body);
            }
            using (var reader = new StreamReader(HttpContext.Server.MapPath("~/Templates/Mail/employer_apply.html")))
            {
                if (!employer.Username.ToLower().Contains("admin"))
                {
                    string body = reader.ReadToEnd();
                    body = body.Replace("@@firstname", user.FullName);
                    body = body.Replace("@@lastname", " ");
                    body = body.Replace("@@jobtitle", job.Title);
                    body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));

                    string rurl = string.Format("{0}://{1}/applications", Request.Url.Scheme, Request.Url.Authority);
                    body = body.Replace("@@downloadurl", string.Format("{0}://{1}/jobseeker/Download?id={2}&redirect={3}", Request.Url.Scheme, Request.Url.Authority, User.Id, rurl));
                    body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, user.PermaLink));
                    body = body.Replace("@@employer", employer.Company);

                    string[] receipent = { employer.Username };
                    var subject = string.Format("Application for {0}", job.Title);

                    AlertService.Instance.SendMail(subject, receipent, body);
                }
            }
        }
        [Authorize(Roles = "Jobseeker")]
        [UrlPrivilegeFilter]
        public ActionResult Apply(long JobId, string redirect)
        {


            //if (UserInfo != null && UserInfo.IsConfirmed == false)
            //{
            //    return RedirectToAction("ConfirmRegistration", "Account");
            //}           
            if (UserInfo != null && UserInfo.IsConfirmed == false)
            {
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });

            }
            DataTable GetUser = MemberService.Instance.GetUserval(UserInfo.Id);
            //if (string.IsNullOrEmpty(GetUser.Rows[0]["Title"].ToString()))
            //{
            //    string redirectUrl = string.Format("/apply?JobId={0}", JobId);
            //    return RedirectToAction("PreApply", "Job", new { Id = JobId, returnUrl = string.Format("/job/apply?jobId={0}&redirect={1}", JobId, redirect) });
            //}

            if (User != null)
            {
                var job = JobService.Instance.Get(JobId);
                if (job != null)
                {
                    UserProfile employer = MemberService.Instance.Get(job.EmployerId.Value);

                    TrackingStatus widthdrawn = TrackingService.Instance.GetStatus(User.Id, null, TrackingTypes.WITHDRAWN, job.Id, User.Id);
                    if (widthdrawn != null && (TrackingTypes)widthdrawn.Type != TrackingTypes.WITHDRAWN)
                    {
                        TrackingStatus trackingStatus = ApplicationService.Instance.Apply(job.Id, User.Id);
                        if (trackingStatus.StatusCount > 0)
                        {
                            switch ((TrackingTypes)trackingStatus.Type)
                            {
                                case TrackingTypes.APPLIED:
                                    TempData["Error"] = string.Format("You have already applied for {0}!", job.Title);
                                    break;
                                case TrackingTypes.AUTO_MATCHED:
                                    TempData["Error"] = string.Format("You have already applied for {0}!", job.Title);
                                    break;
                            }
                        }
                        else
                        {
                            if (trackingStatus.Id != null)
                            {
                                Send(job, employer);
                                TempData["UpdateData"] = string.Format("You have successfully applied for {0}!", job.Title);
                            }
                        }
                    }
                    else
                    {
                        TrackingStatus trackingStatus = ApplicationService.Instance.Reapply(widthdrawn.Id, job.Id, User.Id);
                        if (trackingStatus.StatusCount > 0)
                        {
                            switch ((TrackingTypes)trackingStatus.Type)
                            {
                                case TrackingTypes.APPLIED:
                                    TempData["Error"] = string.Format("You have already applied for {0}!", job.Title);
                                    break;
                                case TrackingTypes.AUTO_MATCHED:
                                    TempData["Error"] = string.Format("You have already applied for {0}!", job.Title);
                                    break;
                            }
                        }
                        else
                        {
                            if (trackingStatus.Id != null)
                            {
                                Send(job, employer);
                                TempData["UpdateData"] = string.Format("You have successfully applied for {0}!", job.Title);
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Jobseeker");
        }

        //[Authorize(Roles = "Individual")]
        public ActionResult ApplyJobs(string jobids)
        {

            string message = string.Empty;
            if (!string.IsNullOrEmpty(jobids))
            {
                var jids = jobids.Split(',');

                if (User != null && User.IsInRole("Jobseeker"))
                {
                    DataTable GetUser = MemberService.Instance.GetUserval(User.Id);
                    if (!string.IsNullOrEmpty(GetUser.Rows[0]["Title"].ToString()) || string.IsNullOrEmpty(GetUser.Rows[0]["Title"].ToString()))
                    {
                        var sbhtml = new StringBuilder();
                        //sbhtml.Append("<div class=\"message-info\"> <ul  class=\"paging\" style=\"list-style-type:none; padding-left:0;\">");
                        sbhtml.Append("<div> <ul  class=\"paging\" style=\"list-style-type:none; padding-left:0;\">");
                        for (var idx = 0; idx < jids.Length; idx++)
                        {
                            if (!string.IsNullOrEmpty(jids[idx]))
                            {
                                var job = JobService.Instance.Get(Convert.ToInt64(jids[idx]));
                                if (job != null)
                                {
                                    UserProfile employer = MemberService.Instance.Get(job.EmployerId.Value);

                                    TrackingStatus widthdrawn = TrackingService.Instance.GetStatus(User.Id, null, TrackingTypes.WITHDRAWN, job.Id, User.Id);
                                    if (widthdrawn != null && (TrackingTypes)widthdrawn.Type != TrackingTypes.WITHDRAWN)
                                    {
                                        TrackingStatus trackingStatus = ApplicationService.Instance.Apply(job.Id, User.Id);
                                        if (trackingStatus.StatusCount > 0)
                                        {
                                            switch ((TrackingTypes)trackingStatus.Type)
                                            {
                                                case TrackingTypes.APPLIED:
                                                    message = string.Format("Already applied for {0}!", job.Title);
                                                    break;
                                                case TrackingTypes.AUTO_MATCHED:
                                                    message = string.Format("Already applied for {0}!", job.Title);
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            if (trackingStatus.Id != null)
                                            {
                                                Send(job, employer);
                                                message = string.Format("Successfully applied for {0}!", job.Title);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TrackingStatus trackingStatus = ApplicationService.Instance.Reapply(widthdrawn.Id, job.Id, User.Id);
                                        if (trackingStatus.StatusCount > 0)
                                        {
                                            switch ((TrackingTypes)trackingStatus.Type)
                                            {
                                                case TrackingTypes.APPLIED:
                                                    message = string.Format("Already applied for {0}!", job.Title);
                                                    break;
                                                case TrackingTypes.AUTO_MATCHED:
                                                    message = string.Format("Already applied for {0}!", job.Title);
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            if (trackingStatus.Id != null)
                                            {
                                                Send(job, employer);
                                                message = string.Format("Successfully applied for {0}!", job.Title);
                                            }
                                        }
                                    }
                                    sbhtml.AppendFormat("<li><div class=\"row\"><div class=\"col-lg-12 col-md-12 col-sm-12\">{0}</div></div></li>", message);
                                }
                            }
                        }
                        sbhtml.Append("</ul></div>");
                        message = sbhtml.ToString();
                    }
                    else
                    {
                        message = "<div>Upload profile before applying to any job(s), <a href=\"/Jobseeker/UpdateProfile1\">CLICK HERE</a> to upload!</div>";
                    }
                }
                else
                {
                    message = "<div class=\"message-instruction\">Login or Register as Jobseeker account before applying to selected job(s)!!</div>";
                }
            }
            return Json(message);
        }
        [AllowAnonymous]
        public ActionResult SMS(long Id)
        {
            var model = new EmailJobModel();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                //var result = dataHelper.Get<shareJMails>().Where(x => typeList.Contains(x.Type) && x.JobseekerId == profile.UserId && x.Job.EmployerId == employer.UserId && x.IsDeleted == false);
                if (User != null)
                {
                    var profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                    var job = dataHelper.GetSingle<Job>(Id);
                    if (job != null)
                    {
                        model = new EmailJobModel
                        {
                            Id = Id,
                            Title = job.Title,
                            SenderEmailAddress = User.Username,
                            SenderName = string.Format("{0} {1}", profile.FirstName, profile.LastName)
                        };
                        ViewBag.x = model;
                    }
                }
                else
                {
                    var job = dataHelper.GetSingle<Job>(Id);
                    if (job != null)
                    {
                        model = new EmailJobModel
                        {
                            Id = Id,
                            Title = job.Title
                        };
                        ViewBag.x = model;
                    }
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SMS(EmailJobModel model)
        {
            string SenderName = string.Empty;
            string SenderEmailAddress = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var job = dataHelper.GetSingle<Job>(model.Id);
                        ViewBag.c = model.Id;
                        ViewBag.t = model.Title;
                        if (User != null)
                        {
                            var profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                            SenderName = string.Format("{0} {1}", profile.FirstName, profile.LastName);
                            SenderEmailAddress = profile.Username;
                        }
                        else
                        {
                            SenderName = model.SenderName;
                            SenderEmailAddress = model.SenderEmailAddress;
                        }
                        var receipents1 = model.FriendEmails.Split(' ').Distinct().ToArray<string>();
                        var receipents = model.FriendEmails.Split(',').Distinct().ToArray<string>();
                        int rcount1 = receipents1.Count();
                        if (rcount1 > 1)
                        {
                            TempData["Error"] = "Provide email address(es) in a comma seperated format!";
                        }
                        else
                        {
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
                                TempData["Error"] = "You cannot share job with yourself!";
                            }
                            else
                            {
                                var body = string.Empty;
                                var profile1 = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                                if (profile1.Type == 5)
                                {
                                    var reader = new StreamReader(Server.MapPath("~/Templates/Mail/emailjobC.html"));
                                    body = reader.ReadToEnd();
                                }
                                else
                                {
                                    var reader = new StreamReader(Server.MapPath("~/Templates/Mail/emailjob.html"));
                                    body = reader.ReadToEnd();
                                }

                                body = body.Replace("@@sender", SenderName);
                                body = body.Replace("@@jobtitle", job.Title);

                                if (Request.Url != null)
                                {
                                    body = body.Replace("@@joburl",
                                        string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink,
                                            job.Id));
                                }

                                string subject = string.Format("{0}", job.Title);
                                AlertService.Instance.SendMail(subject, receipents, body);

                                foreach (var email in receipents)
                                {
                                    List<MemberService.sentE> list = MemberService.Instance.sentEI(profile1.UserId, job.Id, email);
                                    var profile = dataHelper.GetSingle<UserProfile>("Username", email);
                                    if (profile == null)
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

                                TempData["UpdateData"] = "Job sharing link sent successfully!";
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(model);
        }
        public ActionResult SmsListforMonster(long id, string mob, string title)
        {
            ResponseContext context4 = new ResponseContext();
            string[] m = mob.ToLower().Split(' ');
            //string m = jurl;
            //long x1 = Convert.ToInt32(m.Last());
            Regex reg = null;
            reg = new Regex("^[1-9][0-9]*$");
            string message = string.Empty;
            if (!string.IsNullOrEmpty(m[1]) && reg.IsMatch(m[1]))
            {
                string AccountSid = ConfigService.Instance.GetConfigValue("TwilioSID");
                string AuthToken = ConfigService.Instance.GetConfigValue("TwilioToken");
                string from = ConfigService.Instance.GetConfigValue("TwilioNumber");
                //var client = require('twilio')(accountSid, authToken);
                var twilio = new TwilioRestClient(AccountSid, AuthToken);


                StringBuilder sbSMS = new StringBuilder();
                sbSMS.AppendFormat("{0} invites you to connect at Joblisting and Apply for the Job\n", User.Info.FullName);
                sbSMS.AppendFormat("Download Android App Now  https://play.google.com/store/apps/details?id=com.accuracy.joblisting");
                sbSMS.AppendFormat(" Download IOS App Now  https://apps.apple.com/in/app/job-listing/id1575724994/");
                Guid token = Guid.NewGuid();
                var url = string.Format("{0}/connectbyphone?token={1}", Request.Url.GetLeftPart(UriPartial.Authority), token);


                string to = string.Format("{0}{1}", m[0], m[1]);
                string url5 = "";
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    //string[] jo = title.ToLower().Split('-');
                    List<Job> listjob = MemberService.Instance.InviteJob(Convert.ToInt32(id));
                    //var job = dataHelper.GetSingle<Job>(Convert.ToInt32(jurl));
                    if (listjob.Count >= 0)
                    {
                        //url5 = string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, listjob[0].PermaLink,
                        //                                       listjob[0].Id);

                        url5 = string.Format("https://www.joblisting.com/" + title + "");

                        sbSMS.AppendFormat("CLICK HERE TO APPLY {0}", url5);
                        var sms = twilio.SendMessage(from, to, sbSMS.ToString());
                        if (sms.RestException == null)
                        {
                            var msg = twilio.GetMessage(sms.Sid);
                            if (msg.Status.Equals("delivered") || msg.Status.Equals("sent"))
                            {
                                List<MemberService.Invit> listc = MemberService.Instance.InviteSC(User.Id, to);
                                if (listc.Count <= 0)
                                {
                                    List<MemberService.Invit> list = MemberService.Instance.InviteS(sms.Sid, User.Id, to, token);
                                    // List<MemberService.sentE> list2 = MemberService.Instance.sentSI(User.Id, listjob[0].Id, to);
                                }
                                context4 = new ResponseContext()
                                {
                                    Id = 0,
                                    Type = "Failed",
                                    Message = "Invitation sent successfully!"
                                };

                                TempData["SaveData"] = "Invitation sent successfully!";
                            }
                        }
                        else
                        {
                            if (sms.RestException.Code.Equals("14101") || sms.RestException.Code.Equals("21211"))
                            {
                                context4 = new ResponseContext()
                                {
                                    Id = 0,
                                    Type = "Failed",
                                    Message = "Please provide valid mobile number!"
                                };
                                TempData["Error"] = "Please provide valid mobile number!";
                            }
                        }
                    }
                }
            }
            else
            {
                TempData["Error"] = "Please provide valid mobile number!";
                context4 = new ResponseContext()
                {
                    Id = 0,
                    Type = "Failed",
                    Message = "Please provide valid mobile number!"
                };

                //return Json(context4, JsonRequestBehavior.AllowGet);


            }

            //return RedirectToAction("Monsterlist" + "/" + title + "", "admin");
            return RedirectToAction("Monsterlist" + "/" + title + "/" + 205 + "", "admin");
        }
        public ActionResult SmsList(long id, string mob, string title)
        {
            ResponseContext context4 = new ResponseContext();
            //string[] m = title.ToLower().Split('-');
            ////string m = jurl;
            //long x1 = Convert.ToInt32(m.Last());
            Regex reg = null;
            reg = new Regex("^[1-9][0-9]*$");
            string message = string.Empty;
            if (!string.IsNullOrEmpty(mob) && reg.IsMatch(mob))
            {
                string AccountSid = ConfigService.Instance.GetConfigValue("TwilioSID");
                string AuthToken = ConfigService.Instance.GetConfigValue("TwilioToken");
                string from = ConfigService.Instance.GetConfigValue("TwilioNumber");
                var twilio = new TwilioRestClient(AccountSid, AuthToken);

                StringBuilder sbSMS = new StringBuilder();
                sbSMS.AppendFormat("Your Profile matches to {0} job. Register today at Job Listing.com for applying for a job.  \n", title);
                sbSMS.AppendFormat("<a href='https://play.google.com/store/apps/details?id=com.accuracy.joblisting'>Download Android</a> ");
                sbSMS.AppendFormat("<a href='https://apps.apple.com/in/app/job-listing/id1575724994/'>Download IOS</a> ");
                Guid token = Guid.NewGuid();
                var url = string.Format("{0}/connectbyphone?token={1}", Request.Url.GetLeftPart(UriPartial.Authority), token);


                string to = string.Format("{0}{1}", "+91", mob);
                string url5 = "";
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    //string[] jo = title.ToLower().Split('-');
                    List<Job> listjob = MemberService.Instance.InviteJob(Convert.ToInt32(id));
                    //var job = dataHelper.GetSingle<Job>(Convert.ToInt32(jurl));
                    if (listjob.Count >= 0)
                    {
                        //url5 = string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, listjob[0].PermaLink,
                        //                                       listjob[0].Id);

                        url5 = string.Format("https://www.joblisting.com/" + title + "");

                        sbSMS.AppendFormat("CLICK HERE TO APPLY {0}", url5);
                        var sms = twilio.SendMessage(from, to, sbSMS.ToString());
                        if (sms.RestException == null)
                        {
                            var msg = twilio.GetMessage(sms.Sid);
                            if (msg.Status.Equals("delivered") || msg.Status.Equals("sent"))
                            {
                                List<MemberService.Invit> listc = MemberService.Instance.InviteSC(User.Id, to);
                                if (listc.Count <= 0)
                                {
                                    List<MemberService.Invit> list = MemberService.Instance.InviteS(sms.Sid, User.Id, to, token);
                                    // List<MemberService.sentE> list2 = MemberService.Instance.sentSI(User.Id, listjob[0].Id, to);
                                }
                                context4 = new ResponseContext()
                                {
                                    Id = 0,
                                    Type = "Failed",
                                    Message = "Invitation sent successfully!"
                                };

                                TempData["SaveData"] = "Invitation sent successfully!";
                            }
                        }
                        else
                        {
                            if (sms.RestException.Code.Equals("14101") || sms.RestException.Code.Equals("21211"))
                            {
                                context4 = new ResponseContext()
                                {
                                    Id = 0,
                                    Type = "Failed",
                                    Message = "Please provide valid mobile number!"
                                };
                                TempData["Error"] = "Please provide valid mobile number!";
                            }
                        }
                    }
                }
            }
            else
            {
                TempData["Error"] = "Please provide valid mobile number!";
                context4 = new ResponseContext()
                {
                    Id = 0,
                    Type = "Failed",
                    Message = "Please provide valid mobile number!"
                };

                //return Json(context4, JsonRequestBehavior.AllowGet);


            }
            return RedirectToAction("ProfilesLists" + "/" + title + "/" + 205 + "", "admin");
            //return RedirectToAction("jobsbycompanyall", "admin");
        }
        public ActionResult EmailforList(string mobcode, string name, string title, string link, int id)
        {

            sendEmailBUllk(mobcode, name, title, link, id);
            TempData["UpdateData11"] = "Job sharing link sent successfully!";
            return RedirectToAction("ProfilesLists" + "/" + title + "/" + 205 + "", "admin");
        }

        public ActionResult EmailforListmon(string mobcode, string name, string title, string link, int id)
        {

            sendEmailBUllk(mobcode, name, title, link, id);
            TempData["UpdateData11"] = "Job sharing link sent successfully!";
            return RedirectToAction("Monsterlist" + "/" + title + "/" + 205 + "", "admin");
        }

        public ActionResult Email1(EmailJobModel model, long Id)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                if (User != null)
                {
                    var profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                    var job = dataHelper.GetSingle<Job>(Id);
                    if (job != null)
                    {
                        model = new EmailJobModel
                        {
                            Id = Id,
                            Title = job.Title,
                            SenderEmailAddress = User.Username,
                            SenderName = string.Format("{0} {1}", profile.FirstName, profile.LastName)
                        };
                        ViewBag.x = model;
                        UserInfoEntity uinfo = _service.Get(User.Id);
                        UserProfile employer = new UserProfile();
                        var d = job.Title; //"java"; //job.Title;

                        string skil1 = d;
                        string re = skil1.Remove(2);
                        string dil = "" + re + "%";

                        //string dil = "%" + d + "%";
                        List<UserInfoEntity> profile1 = new List<UserInfoEntity>();
                        List<User_Skills> profile_skills = MemberService.Instance.GetUserskillProfiles(dil);


                        foreach (var item in profile_skills)
                        {
                            var profile11 = MemberService.Instance.GetUserInfo(item.UserId);
                            if (profile11.Username != null)
                            {
                                //sendSmsBUllk(profile11.MobileCountryCode, profile11.Mobile, job.Title);
                                sendEmailBUllk(profile11.Username, profile11.FirstName, job.Title, job.PermaLink, job.Id);
                                TempData["UpdateData"] = "Job sharing link sent successfully!";
                            }
                            else
                            {
                                TempData["UpdateData"] = "There is none user Matching this skill";
                                RedirectToAction("jobsbycompanyall", "admin");
                            }
                            profile1.Add(profile11);
                        }
                        TempData["UpdateData"] = "Job sharing link sent successfully!";
                        RedirectToAction("jobsbycompanyall", "admin");
                    }
                }
                else
                {
                    var job = dataHelper.GetSingle<Job>(Id);
                    if (job != null)
                    {
                        model = new EmailJobModel
                        {
                            Id = Id,
                            Title = job.Title
                        };
                        ViewBag.x = model;
                    }
                }
            }
            return RedirectToAction("jobsbycompanyall", "admin");
        }
        public void sendEmailBUllk(string Email, string name, string jurl, string link, long ids)
        {
            ResponseContext context4 = new ResponseContext();
            string m = jurl;
            long x1 = Convert.ToInt32(m.Last());
            string message = string.Empty;
            if (!string.IsNullOrEmpty(Email))
            {
                sendEmail(Email, jurl, name, link, ids);
            }
            else
            {
                TempData["Error"] = "Please provide valid Email !";
                context4 = new ResponseContext()
                {
                    Id = 0,
                    Type = "Failed",
                    Message = "Please provide valid Email!"
                };
            }
        }



        public void sendEmail(string receiver, string title, string pname, string links, long idss)
        {
            try
            {
                var body = string.Empty;
                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/SimpleMail.html"));
                body = reader.ReadToEnd();
                body = body.Replace("@@sender", User.Info.FirstName);
                body = body.Replace("@@jobtitle", title);
                body = body.Replace("@@reciver", pname);
                if (Request.Url != null)
                {
                    body = body.Replace("@@joburl",
                        string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, links,
                            idss));
                }
                //string subject = string.Format("{0}", title);
                string subject = string.Format("Reminder to update Profile");
                //string subject = string.Format("Matching Job");

                WebMail.SmtpServer = "smtp.mailgun.org";
                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = true;
                WebMail.EnableSsl = true;
                if (User.Info.Username == "Doli.chauhan123@accuracy.com.sg" || User.Info.Username == "doli.chauhan123@accuracy.com.sg")
                {

                    string postmail = ConfigurationManager.AppSettings["postmail"];
                    string postpassword = ConfigurationManager.AppSettings["postpassword"];
                    var msg = new MimeMessage();
                    var strfrom = new MailboxAddress("Joblisting", ConfigurationManager.AppSettings["FromEmailAddress"]);
                    msg.To.Add(new MailboxAddress(pname, receiver));
                    msg.Cc.Add(new MailboxAddress("chauhandoli", "chauhandoli@joblisting.com"));

                    WebMail.UserName = "recruiter@joblisting.com";
                    WebMail.Password = "Shivam@_12345";
                    //WebMail.UserName = postmail;
                    //WebMail.Password = postpassword;
                    WebMail.Send(to: receiver, subject: subject, body: body, isBodyHtml: true);

                }
                else if (User.Info.Username == "sandhya@accuracy.com.sg" || User.Info.Username == "Sandhya@accuracy.com.sg")
                {
                    //WebMail.UserName = "mahesh@accuracy.com.sg";
                    //WebMail.Password = "Accuracy123";


                    //WebMail.UserName = "Sandhya@joblisting.com";
                    //WebMail.Password = "Accuracy123";

                    string postmail = ConfigurationManager.AppSettings["postmail"];
                    string postpassword = ConfigurationManager.AppSettings["postpassword"];
                    var msg = new MimeMessage();
                    var strfrom = new MailboxAddress("Joblisting", ConfigurationManager.AppSettings["FromEmailAddress"]);
                    msg.To.Add(new MailboxAddress(pname, receiver));
                    msg.Cc.Add(new MailboxAddress("Sandhya", "Sandhya@joblisting.com"));

                    //WebMail.UserName = "chauhandoli@joblisting.com";
                    //WebMail.Password = "Test@123";
                    WebMail.UserName = postmail;
                    WebMail.Password = postpassword;
                    WebMail.Send(to: receiver, subject: subject, body: body, isBodyHtml: true);

                    //WebMail.UserName = "hmdddhsn@gmail.com";
                    //WebMail.Password = "ebnlxhdvypbibbzh";


                    //WebMail.UserName = "pratapchandrannair@gmail.com";
                    //WebMail.Password = "Pcnair@8754";

                }
                else if (User.Info.Username == "tasnim@accuracy.com.sg" || User.Info.Username == "Tasnim@accuracy.com.sg")
                {
                    //WebMail.UserName = "Tasnim@joblisting.com";
                    //WebMail.Password = "Letmein@01";
                    string postmail = ConfigurationManager.AppSettings["postmail"];
                    string postpassword = ConfigurationManager.AppSettings["postpassword"];
                    var msg = new MimeMessage();
                    var strfrom = new MailboxAddress("Joblisting", ConfigurationManager.AppSettings["FromEmailAddress"]);
                    msg.To.Add(new MailboxAddress(pname, receiver));
                    msg.Cc.Add(new MailboxAddress("Tasnim", "Tasnim@joblisting.com"));

                    //WebMail.UserName = "chauhandoli@joblisting.com";
                    //WebMail.Password = "Test@123";
                    WebMail.UserName = postmail;
                    WebMail.Password = postpassword;
                    WebMail.Send(to: receiver, subject: subject, body: body, isBodyHtml: true);


                }
                else if (User.Info.Username == "deepti@accuracy.com.sg" || User.Info.Username == "Deepti@accuracy.com.sg")
                {
                    //MailMessage mail = new MailMessage();
                    //mail.To.Add("Lakshmiajay1302@gmail.com");
                    //mail.From = new MailAddress("dileeprolex511@gmail.com");
                    //mail.Subject = "dileep";
                    //string Body ="nothing";
                    //mail.Body = Body;
                    //mail.IsBodyHtml = true;
                    //SmtpClient smtp = new SmtpClient();
                    //smtp.Host = "smtp.gmail.com";
                    //smtp.Port = 587;
                    //smtp.UseDefaultCredentials = true;
                    ////client.UseDefaultCredentials = true;
                    //smtp.Credentials = new System.Net.NetworkCredential("dileeprolex511@gmail.com", "Dqdqdq@123"); // Enter seders User name and password  
                    //smtp.EnableSsl = true;
                    //smtp.Send(mail);

                    WebMail.UserName = "Lakshmiajay@joblisting.com";//"Lakshmiajay1302@gmail.com";
                    WebMail.Password = "Varma@2904";
                    WebMail.Send(to: receiver, subject: subject, body: body, isBodyHtml: true);
                    ViewBag.ss = "message Sent";
                }









            }
            catch (Exception ex)
            {
                ViewBag.error = ex;
            }
        }


        [AllowAnonymous]
        public ActionResult Email(long Id)
        {
            if (UserInfo == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var model = new EmailJobModel();
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    //var result = dataHelper.Get<shareJMails>().Where(x => typeList.Contains(x.Type) && x.JobseekerId == profile.UserId && x.Job.EmployerId == employer.UserId && x.IsDeleted == false);
                    if (User != null)
                    {
                        var profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                        //var job = dataHelper.GetSingle<Job>(Id);
                        //SearchJobModel model = new SearchJobModel();
                        ViewBag.job = JobService.Instance.GetLatestJobs22(Id);
                        foreach (LatestJob job in ViewBag.job)
                            if (job != null)
                            {
                                model = new EmailJobModel
                                {
                                    Id = Id,
                                    Title = job.Title,

                                    SenderEmailAddress = User.Username,
                                    SenderName = string.Format("{0} {1}", profile.FirstName, profile.LastName)
                                };
                                ViewBag.x = model;
                            }
                    }
                    else
                    {
                        var job = dataHelper.GetSingle<Job>(Id);
                        if (job != null)
                        {
                            model = new EmailJobModel
                            {
                                Id = Id,
                                Title = job.Title
                            };
                            ViewBag.x = model;
                        }
                    }
                }

                return View(model);
            }
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult Email(EmailJobModel model, long Id)
        {
            string SenderName = string.Empty;
            string SenderEmailAddress = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.job = JobService.Instance.GetLatestJobs22(Id);
                    foreach (LatestJob job in ViewBag.job)
                    {
                        TempData["id"] = job.JobID;
                        TempData["title"] = job.Title;
                        string str = job.JobURL;
                        if (str == null)
                        {
                            str = "#";
                        }
                        char x = '-';
                        int index = -1;
                        for (int i = 0; i < str.Length; i++)
                        {
                            if (str[i] == x)
                            {
                                index = i;
                            }
                        }
                        if (index == -1)
                        {
                            ViewBag.Message = "Not Found";
                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder(str);
                            sb[index] = '?';
                            str = sb.ToString();
                        }
                        TempData["permalink"] = str;
                    }
                    using (JobPortalEntities context = new JobPortalEntities())
                    {

                        DataHelper dataHelper = new DataHelper(context);
                        //var job = dataHelper.GetSingle<Job>(model.Id);
                        //ViewBag.c = model.Id;
                        //ViewBag.t = model.Title;

                        ViewBag.c = TempData["id"];
                        ViewBag.t = TempData["title"];
                        if (User != null)
                        {
                            var profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                            SenderName = string.Format("{0} {1}", profile.FirstName, profile.LastName);
                            SenderEmailAddress = profile.Username;
                        }
                        else
                        {
                            SenderName = model.SenderName;
                            SenderEmailAddress = model.SenderEmailAddress;
                        }
                        var receipents1 = model.FriendEmails.Split(' ').Distinct().ToArray<string>();
                        var receipents = model.FriendEmails.Split(',').Distinct().ToArray<string>();
                        int rcount1 = receipents1.Count();
                        if (rcount1 > 1)
                        {
                            TempData["Error"] = "Provide email address(es) in a comma seperated format!";
                        }
                        else
                        {
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
                                TempData["Error"] = "You cannot share job with yourself!";
                            }
                            else
                            {
                                var body = string.Empty;
                                var profile1 = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                                if (profile1.Type == 5)
                                {
                                    var reader = new StreamReader(Server.MapPath("~/Templates/Mail/emailjobC.html"));
                                    body = reader.ReadToEnd();
                                }
                                else
                                {
                                    var reader = new StreamReader(Server.MapPath("~/Templates/Mail/emailjob.html"));
                                    body = reader.ReadToEnd();
                                }
                                long id = (long)TempData["id"];
                                string title = (string)TempData["title"];
                                string permalink = (string)TempData["permalink"];
                                body = body.Replace("@@sender", SenderName);
                                body = body.Replace("@@jobtitle", title);

                                if (Request.Url != null)
                                {
                                    //body = body.Replace("@@joburl",
                                    //    string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, permalink,
                                    //        id));
                                    body = body.Replace("@@joburl", string.Format("{2}-{3}", Request.Url.Scheme, Request.Url.Authority, permalink, id));
                                }

                                string subject = string.Format("{0}", title);
                                AlertService.Instance.SendMail(subject, receipents, body);

                                foreach (var email in receipents)
                                {
                                    List<MemberService.sentE> list = MemberService.Instance.sentEI(profile1.UserId, id, email);
                                    var profile = dataHelper.GetSingle<UserProfile>("Username", email);
                                    if (profile == null)
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

                                TempData["UpdateData"] = "Job sharing link sent successfully!";
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(model);
        }


        [ValidateInput(false)]
        [HttpGet]
        [UrlPrivilegeFilter]
        public ActionResult SearchStudent(string country)
        {
            SearchStudentModel model = new SearchStudentModel();
            model.Where = country;
            ViewBag.Title = "Search Students | Joblisting.com";
            ViewBag.Description = "Search students ";
            ViewBag.Keywords = "Joblisting, Job Listing, , Search students";
            ViewBag.LatestJobs = JobService.Instance.GetLatestStudent();


            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> SearchStudent(SearchStudentModel model, int PageNumber = 1)
        {
            if (model.PageNumber <= 0)
            {
                model.PageNumber = PageNumber;
            }
            int pageSize = 1;


            List<Student> jobs = new List<Student>();
            if ((!string.IsNullOrEmpty(model.student) && model.student.Trim().Length > 0) || (!string.IsNullOrEmpty(model.Where) && model.Where.Trim().Length > 0))
            {
                //ViewBag.LatestJobs1 = JobService.Instance.GetLatestCompanies1(model.Company, model.Where);

                jobs = await iJobService.GetLatestStudent1(model.student, model.Where);
                //jobs = await JobService.Instance.GetLatestCompanies1(model.Company, model.Where);
            }
            //return Json(jobs, JsonRequestBehavior.AllowGet);
            return View(model);
        }




        [ValidateInput(false)]
        [HttpGet]
        [UrlPrivilegeFilter]
        public ActionResult SearchCompany(string country, int? i)
        {
            SearchCompanyModel model = new SearchCompanyModel();
            model.Where = country;
            ViewBag.Title = "Search Companies | Joblisting.com";
            ViewBag.Description = "Search companies ";
            ViewBag.Keywords = "Joblisting, Job Listing, , Search Companies, Latest Companies, Accounting Companies, I.T. Companies, Healthcare Companies,";
            ViewBag.LatestJobs = JobService.Instance.GetLatestCompanies().ToPagedList(i ?? 1, 12); ;


            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> SearchCompany(SearchCompanyModel model, int PageNumber = 1)
        {
            if (model.PageNumber <= 0)
            {
                model.PageNumber = PageNumber;
            }
            int pageSize = 1;


            List<Companies> jobs = new List<Companies>();
            if ((!string.IsNullOrEmpty(model.Company) && model.Company.Trim().Length > 0) || (!string.IsNullOrEmpty(model.Where) && model.Where.Trim().Length > 0))
            {
                //ViewBag.LatestJobs1 = JobService.Instance.GetLatestCompanies1(model.Company, model.Where);

                jobs = await iJobService.GetLatestCompanies1(model.Company, model.Where);
                //jobs = await JobService.Instance.GetLatestCompanies1(model.Company, model.Where);
            }
            return Json(jobs, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Homepk(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Pakistan";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(4);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homemau(string country, int i = 1, string address = null)

        {
            //, Philippines,Thailand, Japan, Pakistan, China, Afghanistan,
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "mauritius";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homehk(string country, int i = 1, string address = null)

        {
            //, Philippines,Thailand, Japan, Pakistan, China, Afghanistan,
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "hongkong";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homeml(string country, int i = 1, string address = null)
        {
            bool isLogCreate = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLogCreate"]);
            if (isLogCreate)
            {
                GeolocationService locationService = new GeolocationService();
                string countrySession = HttpContext.Session != null && !string.IsNullOrEmpty(Convert.ToString(HttpContext.Session["Country"])) ? Convert.ToString(HttpContext.Session["Country"]) : null;
                //locationService.LogEntry($"Controller: Job, Action: Homeml => Session: {countrySession}");
            }

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Malaysia";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homesn(string country, int i = 1, string address = null)
        {

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Suriname";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }

        public ActionResult Homein(string country, int i = 1, string address = null)
        {
            bool isLogCreate = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLogCreate"]);
            if (isLogCreate)
            {
                GeolocationService locationService = new GeolocationService();
                string countrySession = HttpContext.Session != null && !string.IsNullOrEmpty(Convert.ToString(HttpContext.Session["Country"])) ? Convert.ToString(HttpContext.Session["Country"]) : null;
               // locationService.LogEntry($"Controller: Job, Action: Homein => Session: {countrySession}");
            }

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "India";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }

        public ActionResult Homesg(string country, int i = 1, string address = null)
        {
            bool isLogCreate = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLogCreate"]);
            if (isLogCreate)
            {
                GeolocationService locationService = new GeolocationService();
                string countrySession = HttpContext.Session != null && !string.IsNullOrEmpty(Convert.ToString(HttpContext.Session["Country"])) ? Convert.ToString(HttpContext.Session["Country"]) : null;
                //locationService.LogEntry($"Controller: Job, Action: Homesg => Session: {countrySession}");
            }

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Singapore";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homeid(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Indonesia";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homech(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "China";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }

        public ActionResult HomeBRUNEI(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "BRUNEI";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }

        public ActionResult HomeCYPRUS(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "CYPRUS";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult HomeGEORGIA(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "GEORGIA";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }

        public ActionResult HomeIRAN(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "IRAN";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }

        public ActionResult HomeIRAQ(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "IRAQ";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult HomeISRAEL(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "ISRAEL";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }

        public ActionResult HomeJORDEN(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "JORDEN";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }




        public ActionResult HomeKAZAKHSTAN(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "KAZAKHSTAN";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }


        public ActionResult HomeLAOS(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "LAOS";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }

        public ActionResult HomeLEBANON(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "LEBANON";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }

        public ActionResult HomeMONGOLIA(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "MONGOLIA";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }

        public ActionResult HomePALESTINE(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "PALESTINE";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }

        public ActionResult Homeaf(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Afghanistan";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }

        public ActionResult Homeare(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Afghanistan";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homear(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Armenia";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homeaz(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Azerbaijan";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homebr(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Bahrain";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homebd(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Bangladesh";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homebh(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Bhutan";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homemd(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Maldives";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homenp(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Nepal";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }

        public ActionResult Homevie(string country, int i = 1, string address = null)

        {
            //, Philippines,Thailand, Japan, Pakistan, China, Afghanistan,
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "vietnam";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult HomePhil(string country, int i = 1, string address = null)

        {
            //, Thailand, Japan, Pakistan, China, Afghanistan,
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Philippines";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homejapan(string country, int i = 1, string address = null)

        {
            //, Thailand, Japan, Pakistan, China, Afghanistan,
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "japan";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult HomeThai(string country, int i = 1, string address = null)

        {
            //, Thailand, Japan, Pakistan, China, Afghanistan,
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Thailand";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homesl(string country, int i = 1, string address = null)

        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Srilanka";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homemn(string country, int i = 1, string address = null)
        {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            var result = localZone.StandardName;
            var s = result.Split(' ');
            Console.WriteLine(s[0]);
            Console.WriteLine(RegionInfo.CurrentRegion.DisplayName);





            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Myanmar";
            //string countryName = RegionInfo.CurrentRegion.DisplayName;
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homecmd(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Cambodia";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homefij(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Fiji";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homeaus(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Australia";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }
        public ActionResult Homeedr(string country, int i = 1, string address = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();
            string countryName = "Ecuador";
            Session["cn"] = countryName;//(string)Session["cn1"];
            string location = "";
            string city = "";
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return View(model);
        }

        [UrlPrivilegeFilter]
        public ActionResult Home(string country, int i = 1, string address = null)
        {
            bool isLogCreate = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLogCreate"]);
            if (isLogCreate)
            {
                GeolocationService locationService = new GeolocationService();
                string countrySession = HttpContext.Session != null && !string.IsNullOrEmpty(Convert.ToString(HttpContext.Session["Country"])) ? Convert.ToString(HttpContext.Session["Country"]) : null;
                //locationService.LogEntry($"Controller: Job, Action: Home => Session: { countrySession }");
            }

            TimeZone localZone = TimeZone.CurrentTimeZone;
            var result = localZone.StandardName;
            var s = result.Split(' ');
            Console.WriteLine(s[0]);
            Console.WriteLine(RegionInfo.CurrentRegion.DisplayName);
          string brlocation = RegionInfo.CurrentRegion.DisplayName;
           
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //LocationModel location1 = new LocationModel();
            //Dictionary<string, string> dict = new Dictionary<string, string>();
            ////string name = RegionInfo.CurrentRegion.DisplayName;
            //string url = "https://ipinfo.io/json?token=18061b6ccf594f";
            //using (WebClient client = new WebClient())
            //{
            //    string json = client.DownloadString(url);
            //    location1 = new JavaScriptSerializer().Deserialize<LocationModel>(json);
            //    TempData["cn"] = location1.Country;
            //}
            ////int c = 14;
            //string cn = (string)TempData["cn"];
            //string city = ""; //location1.City;
            //string ip = ""; //location1.IP;
            //string re = location1.region;
            //using (SqlConnection conn = new SqlConnection(constr))
            //{

            //    using (SqlCommand cmd = new SqlCommand("select * from  Lists where [Value] LIKE '" + cn + "%'", conn))
            //    {
            //        cmd.CommandType = CommandType.Text;
            //        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            //        {
            //            using (DataTable dt = new DataTable())
            //            {
            //                sda.Fill(dt);
            //                TempData["cb"] = dt.Rows[0][3];
            //                string text = (string)TempData["cb"];
            //                Session["cb"] = text;
            //                ViewBag.ss = text;

            //            }
            //        }
            //    }

            //}
            string countryName = address;

            //Session["cn"] = address;
            string location = "";
            string city = "";
            SearchJobModel model = new SearchJobModel();
            //var data = JobService.Instance.GetLatestJobs2(country == null ? "" : country, "", i).Take(16);
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, 1).Take(16);
            //var data = JobService.Instance.GetLatestJobs22(cn, re, city, i).Take(16);
            ViewBag.LatestJob1 = data;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("select top 100 Logo from WebsiteList", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ViewBag.Model = dt.AsEnumerable();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            if (brlocation == "Singapore" || brlocation == "singapore")
            {
                Session["cn1"] = address;
                RedirectToAction("Homesg", "job");
            }
            else if (brlocation == "india" || brlocation == "India")
            {
                Session["cn1"] = address;
                RedirectToAction("homein", "job");
            }
            else if (brlocation == "Suriname")
            {
                Session["cn1"] = address;
                RedirectToAction("homesn", "job");
            }
            return View(model);

            
        }
        [UrlPrivilegeFilter]
        public ActionResult View(long Id)
        {
            SearchJobModel model = new SearchJobModel();
            ViewBag.LatestJob2 = JobService.Instance.GetLatestJobs22(Id);
            return View(model);
        }
        [ValidateInput(false)]
        [HttpGet]
        [Route]
        [UrlPrivilegeFilter]
        public ActionResult SearchJobs(string country, int i = 1, string name1 = null, string dileep = null)
        {


            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string cn = (string)Session["cnm"];
            if (dileep != null || cn != null)
            {
                ViewBag.name = "jobs";
            }

            string countryName;

            if (dileep != null)
            {
                if (dileep.Contains("India"))
                {
                    cn = "india";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Suriname"))
                {
                    cn = "suriname";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Singapore"))
                {
                    cn = "singapore";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Pakistan"))
                {
                    cn = "pakistan";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Malaysia"))
                {
                    cn = "malaysia";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Indonesia"))
                {
                    cn = "indonesia";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("China"))
                {
                    cn = "china";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Afghanistan"))
                {
                    cn = "Afghanistan";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Armenia"))
                {
                    cn = "Armenia";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Azerbaijan"))
                {
                    cn = "Azerbaijan";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Bahrain"))
                {
                    cn = "Bahrain";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Bangladesh"))
                {
                    cn = "Bangladesh";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Bhutan"))
                {
                    cn = "Bhutan";
                    Session["cnm"] = cn;
                }
                else if (dileep.Contains("Maldives"))
                {
                    cn = "Maldives";
                    Session["cnm"] = cn;
                }
                else if (dileep.Contains("Nepal"))
                {
                    cn = "Nepal";
                    Session["cnm"] = cn;
                }
                else if (dileep.Contains("Myanmar"))
                {
                    cn = "Myanmar";
                    Session["cnm"] = cn;
                }
                else if (dileep.Contains("SriLanka"))
                {
                    cn = "SriLanka";
                    Session["cnm"] = cn;
                }
                else if (dileep.Contains("Cambodia"))
                {
                    cn = "Cambodia";
                    Session["cnm"] = cn;
                }
                else if (dileep.Contains("Fiji"))
                {
                    cn = "Fiji";
                    Session["cnm"] = cn;
                }
                else if (dileep.Contains("Australia"))
                {
                    cn = "Australia";
                    Session["cnm"] = cn;
                }
                else if (dileep.Contains("Ecuador"))
                {
                    cn = "Ecuador";
                    Session["cnm"] = cn;
                }

            }
            UserInfoEntity user = null;
            if (User != null)
            {
                user = User.Info;

                //string cn = (string)Session["cn"]; ;
                if (cn != null)
                {
                    cn = dileep;
                }
                else
                {
                    cn = user.CountryName;
                }
                //string cn = dileep; //user.CountryName;
                string city = "";
                //string ip = location1.IP;
                string re = ""; //user.StateName;


                SearchJobModel model = new SearchJobModel();
                string location = "";
                //country = cn;
                ViewBag.Title = "Search Jobs | Apply Jobs | Joblisting.com";
                ViewBag.Description = "Search jobs and apply for perfectly matching job according your requirements";
                ViewBag.Keywords = "Joblisting, Job Listing, List Jobs, Search Jobs, Latest Jobs, Accounting Jobs, I.T. Jobs, Healthcare Jobs, Sales Jobs, Government Jobs, Banking Jobs, Engineering Jobs";
                //var data = JobService.Instance.GetLatestJobs2(country == null ? "" : country, "", i);
                var data = JobService.Instance.GetLatestJobs22(cn, re, city, i);
                ViewBag.cn = cn;
                ViewBag.LatestJobs = data;
                //using (SqlConnection conn = new SqlConnection(constr))
                //{
                //    using (SqlCommand cmd = new SqlCommand("select count(*) from  WebJobList where CountryName='" + cn + "'", conn))
                //    {
                //        cmd.CommandType = CommandType.Text;
                //        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                //        {
                //            using (DataTable dt = new DataTable())
                //            {
                //                sda.Fill(dt);
                //                TempData["count11"] = dt.Rows[0][0];
                //                int count1 = (int)TempData["count11"];
                //                Session["count11"] = count1;
                //            }
                //        }
                //    }
                //}
                //int rows = (int)Session["count11"];//data.Count();
                var pageModel = new Pager(data.FirstOrDefault().TotalRow, i, 20);
                model.DataSize = pageModel.PageSize;
                model.Where = country; //changes
                model.CurrentPage = pageModel.CurrentPage;
                model.EndPage = pageModel.EndPage;
                model.StartPage = pageModel.StartPage;
                model.CurrentPage = pageModel.CurrentPage;
                model.TotalPages = pageModel.TotalPages;
                model.PageSize = pageModel.PageSize;
                return View(model);
            }



            else if (dileep == null)
            {
                SearchJobModel model = new SearchJobModel();
                string location = "";
                string countryNam;
                if (Session["cnm"] != null)
                {
                    countryNam = (string)Session["cnm"];
                }
                else
                {
                    countryNam = (string)Session["cn"];
                }
                //country = countryNam;
                //string countryName = "";
                string city = "";
                ViewBag.Title = "Search Jobs | Apply Jobs | Joblisting.com";
                ViewBag.Description = "Search jobs and apply for perfectly matching job according your requirements";
                ViewBag.Keywords = "Joblisting, Job Listing, List Jobs, Search Jobs, Latest Jobs, Accounting Jobs, I.T. Jobs, Healthcare Jobs, Sales Jobs, Government Jobs, Banking Jobs, Engineering Jobs";
                var data = JobService.Instance.GetLatestJobs22(countryNam == "" ? "" : countryNam, location, city, i);

                //var ff = data.Where(m => m.Title == ".Net Developer (C#)").ToList();
                //var ff1 = data.Where(m => m.Title.StartsWith("E")).ToList();
                ViewBag.cn = countryNam;
                ViewBag.LatestJobs = data;
                //using (SqlConnection conn = new SqlConnection(constr))
                //{
                //    using (SqlCommand cmd = new SqlCommand("select count(*) from  WebJobList where CountryName='" + countryNam + "'", conn))
                //    {
                //        cmd.CommandType = CommandType.Text;
                //        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                //        {
                //            using (DataTable dt = new DataTable())
                //            {
                //                sda.Fill(dt);
                //                TempData["count12"] = dt.Rows[0][0];
                //                int count1 = (int)TempData["count12"];
                //                Session["count12"] = count1;
                //            }
                //        }
                //    }
                //}
                //int rows = (int)Session["count12"];//data.Count();
                var pageModel = new Pager(data.FirstOrDefault().TotalRow, i, 20);
                model.DataSize = pageModel.PageSize;
                model.Where = country; //changes
                model.CurrentPage = pageModel.CurrentPage;
                model.EndPage = pageModel.EndPage;
                model.StartPage = pageModel.StartPage;
                model.CurrentPage = pageModel.CurrentPage;
                model.TotalPages = pageModel.TotalPages;
                model.PageSize = pageModel.PageSize;
                return View(model);
            }
            else
            {
                SearchJobModel model = new SearchJobModel();


                ViewBag.Title = "Search Jobs | Apply Jobs | Joblisting.com";
                ViewBag.Description = "Search jobs and apply for perfectly matching job according your requirements";
                ViewBag.Keywords = "Joblisting, Job Listing, List Jobs, Search Jobs, Latest Jobs, Accounting Jobs, I.T. Jobs, Healthcare Jobs, Sales Jobs, Government Jobs, Banking Jobs, Engineering Jobs";

                var data = JobService.Instance.GetLatestJobs22(dileep, "", "", i);
                ViewBag.cn = dileep;
                ViewBag.LatestJobs = data;
                //using (SqlConnection conn = new SqlConnection(constr))
                //{
                //    using (SqlCommand cmd = new SqlCommand("select count(*) from  WebJobList where CountryName='" + dileep + "'", conn))
                //    {
                //        cmd.CommandType = CommandType.Text;
                //        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                //        {
                //            using (DataTable dt = new DataTable())
                //            {
                //                sda.Fill(dt);
                //                TempData["count1"] = dt.Rows[0][0];
                //                int count1 = (int)TempData["count1"];
                //                Session["count1"] = count1;                              
                //            }
                //        }
                //    }
                //}
                // long rowss =model.TotalRow;
                //int rows = (int)Session["count1"];//data.Count();
                var pageModel = new Pager(data.FirstOrDefault().TotalRow, i, 20);
                model.DataSize = pageModel.PageSize;
                model.Where = country; //changes
                model.CurrentPage = pageModel.CurrentPage;
                model.EndPage = pageModel.EndPage;
                model.StartPage = pageModel.StartPage;
                model.CurrentPage = pageModel.CurrentPage;
                model.TotalPages = pageModel.TotalPages;
                model.PageSize = pageModel.PageSize;
                return View(model);
            }


        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SearchJobs(SearchJobModel model, string country, int i = 1, int PageNumber = 1, string dileep = null)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string cn = (string)Session["cnm"];
            if (dileep != null || cn != null)
            {
                ViewBag.name = "jobs";
            }

            string countryName;

            if (dileep != null)
            {
                if (dileep.Contains("India"))
                {
                    cn = "india";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Suriname"))
                {
                    cn = "suriname";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Singapore"))
                {
                    cn = "singapore";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Pakistan"))
                {
                    cn = "pakistan";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Malaysia"))
                {
                    cn = "malaysia";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Indonesia"))
                {
                    cn = "indonesia";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("China"))
                {
                    cn = "china";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Afghanistan"))
                {
                    cn = "Afghanistan";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Armenia"))
                {
                    cn = "Armenia";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Azerbaijan"))
                {
                    cn = "Azerbaijan";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Bahrain"))
                {
                    cn = "Bahrain";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Bangladesh"))
                {
                    cn = "Bangladesh";
                    Session["cnm"] = cn;

                }
                else if (dileep.Contains("Bhutan"))
                {
                    cn = "Bhutan";
                    Session["cnm"] = cn;
                }
                else if (dileep.Contains("Maldives"))
                {
                    cn = "Maldives";
                    Session["cnm"] = cn;
                }
                else if (dileep.Contains("Nepal"))
                {
                    cn = "Nepal";
                    Session["cnm"] = cn;
                }
                else if (dileep.Contains("Myanmar"))
                {
                    cn = "Myanmar";
                    Session["cnm"] = cn;
                }
                else if (dileep.Contains("SriLanka"))
                {
                    cn = "SriLanka";
                    Session["cnm"] = cn;
                }
                else if (dileep.Contains("Cambodia"))
                {
                    cn = "Cambodia";
                    Session["cnm"] = cn;
                }
                else if (dileep.Contains("Fiji"))
                {
                    cn = "Fiji";
                    Session["cnm"] = cn;
                }
                else if (dileep.Contains("Australia"))
                {
                    cn = "Australia";
                    Session["cnm"] = cn;
                }
                else if (dileep.Contains("Ecuador"))
                {
                    cn = "Ecuador";
                    Session["cnm"] = cn;
                }

            }
            UserInfoEntity user = null;
            if (User != null)
            {
                user = User.Info;

                //string cn = (string)Session["cn"]; ;
                if (cn != null)
                {
                    cn = dileep;
                }
                else
                {
                    cn = user.CountryName;
                }
                //string cn = dileep; //user.CountryName;
                string city = "";
                //string ip = location1.IP;
                string re = ""; //user.StateName;


                //SearchJobModel model = new SearchJobModel();
                string location = "";
                //country = cn;
                ViewBag.Title = "Search Jobs | Apply Jobs | Joblisting.com";
                ViewBag.Description = "Search jobs and apply for perfectly matching job according your requirements";
                ViewBag.Keywords = "Joblisting, Job Listing, List Jobs, Search Jobs, Latest Jobs, Accounting Jobs, I.T. Jobs, Healthcare Jobs, Sales Jobs, Government Jobs, Banking Jobs, Engineering Jobs";
                //var data = JobService.Instance.GetLatestJobs2(country == null ? "" : country, "", i);
                var data = JobService.Instance.GetLatestJobs22(cn, re, city, i);
                ViewBag.cn = cn;
                ViewBag.LatestJobs = data;
                //using (SqlConnection conn = new SqlConnection(constr))
                //{
                //    using (SqlCommand cmd = new SqlCommand("select count(*) from  WebJobList where CountryName='" + cn + "'", conn))
                //    {
                //        cmd.CommandType = CommandType.Text;
                //        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                //        {
                //            using (DataTable dt = new DataTable())
                //            {
                //                sda.Fill(dt);
                //                TempData["count11"] = dt.Rows[0][0];
                //                int count1 = (int)TempData["count11"];
                //                Session["count11"] = count1;
                //            }
                //        }
                //    }
                //}
                //int rows = (int)Session["count11"];//data.Count();
                var pageModel = new Pager(data.FirstOrDefault().TotalRow, i, 20);
                model.DataSize = pageModel.PageSize;
                model.Where = country; //changes
                model.CurrentPage = pageModel.CurrentPage;
                model.EndPage = pageModel.EndPage;
                model.StartPage = pageModel.StartPage;
                model.CurrentPage = pageModel.CurrentPage;
                model.TotalPages = pageModel.TotalPages;
                model.PageSize = pageModel.PageSize;
                return View(model);
            }



            else if (dileep == null)
            {
                //SearchJobModel model = new SearchJobModel();
                string location = "";
                string countryNam;
                if (Session["cnm"] != null)
                {
                    countryNam = (string)Session["cnm"];
                }
                else
                {
                    countryNam = (string)Session["cn"];
                }
                //country = countryNam;
                //string countryName = "";
                string city = "";
                ViewBag.Title = "Search Jobs | Apply Jobs | Joblisting.com";
                ViewBag.Description = "Search jobs and apply for perfectly matching job according your requirements";
                ViewBag.Keywords = "Joblisting, Job Listing, List Jobs, Search Jobs, Latest Jobs, Accounting Jobs, I.T. Jobs, Healthcare Jobs, Sales Jobs, Government Jobs, Banking Jobs, Engineering Jobs";
                var data = JobService.Instance.GetLatestJobs22(countryNam == "" ? "" : countryNam, location, city, 25);
                if(model.JobTitle==null)
                {
                    ViewBag.cn = countryNam;
                    ViewBag.LatestJobs = data;
                }   
                else
                {
                    string strModified = model.JobTitle.Substring(0, 2);
                    //var ff = data.Where(m => m.Title == model.JobTitle).ToList();
                    var ff1 = data.Where(m => m.Title.StartsWith(strModified)).ToList();
                    ViewBag.cn = countryNam;
                    ViewBag.LatestJobs = ff1;
                }
               
                //using (SqlConnection conn = new SqlConnection(constr))
                //{
                //    using (SqlCommand cmd = new SqlCommand("select count(*) from  WebJobList where CountryName='" + countryNam + "'", conn))
                //    {
                //        cmd.CommandType = CommandType.Text;
                //        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                //        {
                //            using (DataTable dt = new DataTable())
                //            {
                //                sda.Fill(dt);
                //                TempData["count12"] = dt.Rows[0][0];
                //                int count1 = (int)TempData["count12"];
                //                Session["count12"] = count1;
                //            }
                //        }
                //    }
                //}
                //int rows = (int)Session["count12"];//data.Count();
                var pageModel = new Pager(data.FirstOrDefault().TotalRow, i, 20);
                model.DataSize = pageModel.PageSize;
                model.Where = country; //changes
                model.CurrentPage = pageModel.CurrentPage;
                model.EndPage = pageModel.EndPage;
                model.StartPage = pageModel.StartPage;
                model.CurrentPage = pageModel.CurrentPage;
                model.TotalPages = pageModel.TotalPages;
                model.PageSize = pageModel.PageSize;
                return View(model);
            }
            else
            {
                //SearchJobModel model = new SearchJobModel();


                ViewBag.Title = "Search Jobs | Apply Jobs | Joblisting.com";
                ViewBag.Description = "Search jobs and apply for perfectly matching job according your requirements";
                ViewBag.Keywords = "Joblisting, Job Listing, List Jobs, Search Jobs, Latest Jobs, Accounting Jobs, I.T. Jobs, Healthcare Jobs, Sales Jobs, Government Jobs, Banking Jobs, Engineering Jobs";

                var data = JobService.Instance.GetLatestJobs22(dileep, "", "", i);
                ViewBag.cn = dileep;
                ViewBag.LatestJobs = data;
                //using (SqlConnection conn = new SqlConnection(constr))
                //{
                //    using (SqlCommand cmd = new SqlCommand("select count(*) from  WebJobList where CountryName='" + dileep + "'", conn))
                //    {
                //        cmd.CommandType = CommandType.Text;
                //        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                //        {
                //            using (DataTable dt = new DataTable())
                //            {
                //                sda.Fill(dt);
                //                TempData["count1"] = dt.Rows[0][0];
                //                int count1 = (int)TempData["count1"];
                //                Session["count1"] = count1;                              
                //            }
                //        }
                //    }
                //}
                // long rowss =model.TotalRow;
                //int rows = (int)Session["count1"];//data.Count();
                var pageModel = new Pager(data.FirstOrDefault().TotalRow, i, 20);
                model.DataSize = pageModel.PageSize;
                model.Where = country; //changes
                model.CurrentPage = pageModel.CurrentPage;
                model.EndPage = pageModel.EndPage;
                model.StartPage = pageModel.StartPage;
                model.CurrentPage = pageModel.CurrentPage;
                model.TotalPages = pageModel.TotalPages;
                model.PageSize = pageModel.PageSize;
                return View(model);
            }


        }


        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> PaidJobs(SearchJobModel model, int PageNumber = 1)
        {
            if (model.PageNumber <= 0)
            {
                model.PageNumber = PageNumber;
            }
            var jobSearch = new SearchJob
            {
                CountryId = model.CountryId,
                Title = model.JobTitle != null ? model.JobTitle : null,
                PageNumber = PageNumber,
                PageSize = 2
            };

            List<SearchedJobEntity> jobs = new List<SearchedJobEntity>();
            string username = User != null ? User.Username : null;
            jobSearch.Username = username;
            jobs = await iJobService.PaidJobs(jobSearch);

            return Json(jobs, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public ActionResult Block(Guid Id, string redirect)
        {
            UserProfile profile = MemberService.Instance.Get(User.Username);
            SecurityRoles type = (SecurityRoles)profile.Type;
            Tracking record = TrackingService.Instance.Get(Id);
            bool connected = false;
            bool connectionBlocked = false;
            bool employerBlocked = false;
            string message = string.Empty;

            if (record != null)
            {
                Job job = JobService.Instance.Get(record.JobId.Value);

                switch (type)
                {
                    case SecurityRoles.Jobseeker:
                        UserProfile employer = MemberService.Instance.Get(job.EmployerId.Value); // Getting Employer
                        connected = ConnectionHelper.IsConnected(employer.Username, profile.Username); // Connected or Not
                        connectionBlocked = ConnectionHelper.IsBlockedByMe(employer.Username, profile.UserId); // this connection has blocked by Individual (logged in user)
                        employerBlocked = JobSeekerService.Instance.IsBlocked(employer.UserId, profile.UserId); // Is employer blocked or not 

                        if (connected == true && connectionBlocked == false)
                        {
                            connectionBlocked = ConnectionHelper.Block(employer.Username, profile.UserId);
                            employerBlocked = JobSeekerService.Instance.Block(employer.Username, profile.UserId);
                            if (connectionBlocked == true && employerBlocked == true)
                            {
                                List<Tracking> app_list = new List<Tracking>();
                                List<int> typeList = new List<int>();
                                typeList.Add((int)TrackingTypes.AUTO_MATCHED);
                                typeList.Add((int)TrackingTypes.APPLIED);

                                using (JobPortalEntities context = new JobPortalEntities())
                                {
                                    DataHelper dataHelper = new DataHelper(context);

                                    var result = dataHelper.Get<Tracking>().Where(x => typeList.Contains(x.Type) && x.JobseekerId == profile.UserId && x.Job.EmployerId == employer.UserId && x.IsDeleted == false);
                                    if (result.Count() > 0)
                                    {
                                        app_list = result.ToList();
                                    }
                                }

                                StringBuilder sbMsg = new StringBuilder();
                                StringBuilder joblist = new StringBuilder();

                                sbMsg.Append(string.Format("You have successfully blocked {0}!<br/>", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName))));
                                sbMsg.Append("<b>You application(s) status is withdrawn for the following jobs:</b><br/><ul>");

                                foreach (Tracking item in app_list)
                                {
                                    Tracking status = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, profile.Username, out message);
                                    if (status.Type == (int)TrackingTypes.WITHDRAWN)
                                    {
                                        Job entity = JobService.Instance.Get(status.JobId.Value);
                                        sbMsg.AppendFormat("<li><a href=\"{0}://{1}/{2}-{3}\" target=\"_blank\">{4}</a></li>", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id, job.Title);
                                        joblist.AppendFormat("<li><a href=\"{0}://{1}/{2}-{3}\" target=\"_blank\">{4}</a></li>", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id, job.Title);
                                    }
                                }
                                sbMsg.Append("</ul>");

                                // Sending mail to Individual
                                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/jobseeker_all_application_withdrawn.html"));
                                var body = string.Empty;

                                if (reader != null)
                                {
                                    body = reader.ReadToEnd();
                                    body = body.Replace("@@firstname", profile.FirstName);
                                    body = body.Replace("@@lastname", profile.LastName);
                                    body = body.Replace("@@list", joblist.ToString());

                                    string[] receipent = { profile.Username };
                                    var subject = "Application(s) Withdrawn!";

                                    AlertService.Instance.SendMail(subject, receipent, body);
                                }

                                // Sending mail to Company
                                reader =
                                    new StreamReader(Server.MapPath("~/Templates/Mail/employer_all_application_withdrawn.html"));
                                body = string.Empty;

                                if (reader != null)
                                {
                                    body = reader.ReadToEnd();
                                    body = body.Replace("@@employer", employer.Company);
                                    body = body.Replace("@@firstname", profile.FirstName);
                                    body = body.Replace("@@lastname", profile.LastName);
                                    body = body.Replace("@@list", joblist.ToString());

                                    string[] receipent = { employer.Username };
                                    var subject = "Application(s) Withdrawn!";

                                    AlertService.Instance.SendMail(subject, receipent, body);
                                }

                                TempData["UpdateData"] = sbMsg.ToString();
                            }
                            else if (connectionBlocked == false && employerBlocked == true)
                            {
                                List<Tracking> app_list = new List<Tracking>();
                                List<int> typeList = new List<int>();
                                typeList.Add((int)TrackingTypes.AUTO_MATCHED);
                                typeList.Add((int)TrackingTypes.APPLIED);

                                using (JobPortalEntities context = new JobPortalEntities())
                                {
                                    DataHelper dataHelper = new DataHelper(context);

                                    var result = dataHelper.Get<Tracking>().Where(x => typeList.Contains(x.Type) && x.JobseekerId == profile.UserId && x.Job.EmployerId == employer.UserId && x.IsDeleted == false);
                                    if (result.Count() > 0)
                                    {
                                        app_list = result.ToList();
                                    }
                                }

                                StringBuilder sbMsg = new StringBuilder();
                                StringBuilder joblist = new StringBuilder();

                                sbMsg.Append(string.Format("You have successfully blocked {0}!<br/>", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName))));
                                sbMsg.Append("<b>You application(s) status is withdrawn for the following jobs:</b><br/><ul>");

                                foreach (Tracking item in app_list)
                                {
                                    Tracking status = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, profile.Username, out message);
                                    if (status.Type == (int)TrackingTypes.WITHDRAWN)
                                    {
                                        Job entity = JobService.Instance.Get(status.JobId.Value);
                                        sbMsg.AppendFormat("<li><a href=\"{0}://{1}/{2}-{3}\" target=\"_blank\">{4}</a></li>", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id, job.Title);
                                        joblist.AppendFormat("<li><a href=\"{0}://{1}/{2}-{3}\" target=\"_blank\">{4}</a></li>", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id, job.Title);
                                    }
                                }
                                sbMsg.Append("</ul>");

                                // Sending mail to Individual
                                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/jobseeker_all_application_withdrawn.html"));
                                var body = string.Empty;

                                if (reader != null)
                                {
                                    body = reader.ReadToEnd();
                                    body = body.Replace("@@firstname", profile.FirstName);
                                    body = body.Replace("@@lastname", profile.LastName);
                                    body = body.Replace("@@list", joblist.ToString());

                                    string[] receipent = { profile.Username };
                                    var subject = "Application(s) Withdrawn!";

                                    AlertService.Instance.SendMail(subject, receipent, body);
                                }

                                // Sending mail to Company
                                reader =
                                    new StreamReader(Server.MapPath("~/Templates/Mail/employer_all_application_withdrawn.html"));
                                body = string.Empty;

                                if (reader != null)
                                {
                                    body = reader.ReadToEnd();
                                    body = body.Replace("@@employer", employer.Company);
                                    body = body.Replace("@@firstname", profile.FirstName);
                                    body = body.Replace("@@lastname", profile.LastName);
                                    body = body.Replace("@@list", joblist.ToString());

                                    string[] receipent = { employer.Username };
                                    var subject = "Application(s) Withdrawn!";

                                    AlertService.Instance.SendMail(subject, receipent, body);
                                }

                                TempData["UpdateData"] = sbMsg.ToString();
                            }
                        }
                        else if (connected == false && connectionBlocked == false)
                        {
                            employerBlocked = JobSeekerService.Instance.Block(employer.Username, profile.UserId);
                            if (employerBlocked == true)
                            {
                                List<Tracking> app_list = new List<Tracking>();
                                List<int> typeList = new List<int>();
                                typeList.Add((int)TrackingTypes.AUTO_MATCHED);
                                typeList.Add((int)TrackingTypes.APPLIED);

                                using (JobPortalEntities context = new JobPortalEntities())
                                {
                                    DataHelper dataHelper = new DataHelper(context);

                                    var result = dataHelper.Get<Tracking>().Where(x => typeList.Contains(x.Type) && x.JobseekerId == profile.UserId && x.Job.EmployerId == employer.UserId && x.IsDeleted == false);
                                    if (result.Count() > 0)
                                    {
                                        app_list = result.ToList();
                                    }
                                }

                                StringBuilder sbMsg = new StringBuilder();
                                StringBuilder joblist = new StringBuilder();

                                sbMsg.Append(string.Format("You have successfully blocked {0}!<br/>", (!string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName))));
                                sbMsg.Append("<b>You application(s) status is withdrawn for the following jobs:</b><br/><ul>");

                                foreach (Tracking item in app_list)
                                {
                                    Tracking status = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, profile.Username, out message);
                                    if (status.Type == (int)TrackingTypes.WITHDRAWN)
                                    {
                                        Job entity = JobService.Instance.Get(status.JobId.Value);
                                        sbMsg.AppendFormat("<li><a href=\"{0}://{1}/{2}-{3}\" target=\"_blank\">{4}</a></li>", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id, job.Title);
                                        joblist.AppendFormat("<li><a href=\"{0}://{1}/{2}-{3}\" target=\"_blank\">{4}</a></li>", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id, job.Title);
                                    }
                                }
                                sbMsg.Append("</ul>");

                                // Sending mail to Individual
                                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/jobseeker_all_application_withdrawn.html"));
                                var body = string.Empty;

                                if (reader != null)
                                {
                                    body = reader.ReadToEnd();
                                    body = body.Replace("@@firstname", profile.FirstName);
                                    body = body.Replace("@@lastname", profile.LastName);
                                    body = body.Replace("@@list", joblist.ToString());

                                    string[] receipent = { profile.Username };
                                    var subject = "Application(s) Withdrawn!";

                                    AlertService.Instance.SendMail(subject, receipent, body);
                                }

                                // Sending mail to Company
                                reader =
                                    new StreamReader(Server.MapPath("~/Templates/Mail/employer_all_application_withdrawn.html"));
                                body = string.Empty;

                                if (reader != null)
                                {
                                    body = reader.ReadToEnd();
                                    body = body.Replace("@@employer", employer.Company);
                                    body = body.Replace("@@firstname", profile.FirstName);
                                    body = body.Replace("@@lastname", profile.LastName);
                                    body = body.Replace("@@list", joblist.ToString());

                                    string[] receipent = { employer.Username };
                                    var subject = "Application(s) Withdrawn!";

                                    AlertService.Instance.SendMail(subject, receipent, body);
                                }

                                TempData["UpdateData"] = sbMsg.ToString();
                            }
                        }
                        break;
                    case SecurityRoles.Employers:
                        UserProfile jobSeeker = MemberService.Instance.Get(record.JobseekerId.Value); // Getting Individual
                        connected = ConnectionHelper.IsConnected(jobSeeker.Username, profile.Username); // Checking Individual and Company are connected or not
                        connectionBlocked = ConnectionHelper.IsBlockedByMe(jobSeeker.Username, profile.UserId); // Connection is blocked by Company or not

                        if (connected == true && connectionBlocked == false)
                        {
                            connectionBlocked = ConnectionHelper.Block(jobSeeker.Username, profile.UserId);
                            if (connectionBlocked == true)
                            {
                                List<Tracking> app_list = new List<Tracking>();
                                List<int> typeList = new List<int>();
                                typeList.Add((int)TrackingTypes.AUTO_MATCHED);
                                typeList.Add((int)TrackingTypes.APPLIED);

                                using (JobPortalEntities context = new JobPortalEntities())
                                {
                                    DataHelper dataHelper = new DataHelper(context);

                                    var result = dataHelper.Get<Tracking>().Where(x => typeList.Contains(x.Type) && x.JobseekerId == jobSeeker.UserId && x.Job.EmployerId == profile.UserId && x.IsDeleted == false);
                                    if (result.Count() > 0)
                                    {
                                        app_list = result.ToList();
                                    }
                                }

                                StringBuilder sbMsg = new StringBuilder();
                                StringBuilder joblist = new StringBuilder();
                                sbMsg.Append(string.Format("You have successfully blocked {0}!<br/><br/>", string.Format("{0} {1}", jobSeeker.FirstName, jobSeeker.LastName)));
                                sbMsg.AppendFormat("Application(s) status submited by {0} {1} is withdrawn for the following jobs:<br/><ul>", jobSeeker.FirstName, jobSeeker.LastName);

                                foreach (Tracking item in app_list)
                                {
                                    Tracking status = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, profile.Username, out message);
                                    if (status.Type == (int)TrackingTypes.WITHDRAWN)
                                    {
                                        Job entity = JobService.Instance.Get(status.JobId.Value);
                                        sbMsg.AppendFormat("<li><a href=\"{0}://{1}/{2}-{3}\" target=\"_blank\">{4}</a></li>", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id, job.Title);
                                        joblist.AppendFormat("<li><a href=\"{0}://{1}/{2}-{3}\" target=\"_blank\">{4}</a></li>", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id, job.Title);
                                    }
                                }
                                sbMsg.Append("</ul>");

                                // Sending mail to Individual
                                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/jobseeker_all_application_withdrawn.html"));
                                var body = string.Empty;

                                if (reader != null)
                                {
                                    body = reader.ReadToEnd();
                                    body = body.Replace("@@firstname", jobSeeker.FirstName);
                                    body = body.Replace("@@lastname", jobSeeker.LastName);
                                    body = body.Replace("@@list", joblist.ToString());

                                    string[] receipent = { jobSeeker.Username };
                                    var subject = "Application(s) Withdrawn!";

                                    AlertService.Instance.SendMail(subject, receipent, body);
                                }

                                // Sending mail to Company
                                reader = new StreamReader(Server.MapPath("~/Templates/Mail/employer_all_application_withdrawn.html"));
                                body = string.Empty;

                                if (reader != null)
                                {
                                    body = reader.ReadToEnd();
                                    body = body.Replace("@@employer", profile.Company);
                                    body = body.Replace("@@firstname", jobSeeker.FirstName);
                                    body = body.Replace("@@lastname", jobSeeker.LastName);
                                    body = body.Replace("@@list", joblist.ToString());

                                    string[] receipent = { profile.Username };
                                    var subject = "Application(s) Withdrawn!";

                                    AlertService.Instance.SendMail(subject, receipent, body);
                                }

                                TempData["UpdateData"] = sbMsg.ToString();
                            }
                        }
                        break;
                }
            }
            return Redirect(redirect);
        }
    }

    internal class Location
    {
    }
}