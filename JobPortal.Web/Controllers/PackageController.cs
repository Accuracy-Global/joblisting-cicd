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
using JobPortal.Web.Models;
using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace JobPortal.Web.Controllers
{
    [Authorize]
    public class PackageController : BaseController
    {

#pragma warning disable CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
        IHelperService helper;
#pragma warning restore CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
        IJobService jobService;
#pragma warning restore CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
        public PackageController(IUserService service, IHelperService helper, IJobService jobService)
#pragma warning restore CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
            : base(service)
        {
            this.helper = helper;
            this.jobService = jobService;
        }        

        // GET: Package
        [Authorize(Roles="Super, Administrator")]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Super, Administrator")]
        [HttpPost]
        public async Task<ActionResult> Index(long? countryId, string name, int page = 1)
        {
            UserProfile profile = MemberService.Instance.Get(User.Username);
            SecurityRoles type = (SecurityRoles)profile.Type;
            ResponseContext context = null;
            name = (!string.IsNullOrEmpty(name) ? name : null);
            try
            {
                List<Package> list = await helper.PackageList(countryId, name, page);

                if (list.Count > 0)
                {
                    context = new ResponseContext()
                    {
                        Id = 1,
                        Type = "Success",
                        Data = list
                    };
                }
                else
                {
                    context = new ResponseContext()
                    {
                        Id = 0,
                        Type = "Error",
                        Message = "No packages defined yet!"
                    };
                }
            }
            catch (Exception ex)
            {
                context = new ResponseContext()
                {
                    Id = -1,
                    Type = "Error",
                    Message = ex.Message
                };
            }

            return Json(context, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "JobSeeker, Employer")]
        [HttpGet]
        public ActionResult Promote(long id, string type, string returnUrl = null)
        {
            ViewBag.Type = type;
            ViewBag.ReturnUrl = returnUrl;
            if (type == "J")
            {
                ViewBag.Entity = JobService.Instance.Get(id);
            }
            else if (type == "P")
            {                                
                ViewBag.Entity = _service.Get(id);

                decimal weightage = MemberService.Instance.GetProfileWeightage(id);
                if (weightage < 90)
                {
                    return RedirectToAction("UpdateProfile1", "Jobseeker", new { type = type, returnUrl = string.Format("/package/promote?id={0}&type=P&returnurl={1}", id, returnUrl) });
                }
            }
            ViewBag.Promote = helper.PromotePrice(id, type);
            return View();
        }

        [Authorize(Roles = "Individual, Company")]
        [HttpGet]
        public ActionResult ResumePriceList(long id, string type, string returnUrl = null)
        {
            ViewBag.Type = type;
            if (type == "R")
            {
                var item = _service.Get(id);
                ViewBag.Entity = item;
                ViewBag.BuyResume = helper.ResumeDonwloadPrice(User.Info.CountryId.Value);
            }
            else if (type == "JR")
            {
                var item = _service.Get(id);
                ViewBag.Entity = item;
                ViewBag.BuyResume = helper.ResumeDonwloadPrice(User.Info.CountryId.Value);
            }
            return View();
        }

        [Authorize(Roles = "Individual, Company")]
        [HttpGet]
        public ActionResult MessagePriceList(Guid? id, string returnUrl = null)
        {
            ViewBag.SessionId = id;            
            ViewBag.BuyMessage = helper.MessagePrice(User.Info.CountryId.Value, User.Info.Type);            

            return View();
        }

#pragma warning disable CS0246 // The type or namespace name 'BuyPromote' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult ProcessMessage(BuyPromote model)
#pragma warning restore CS0246 // The type or namespace name 'BuyPromote' could not be found (are you missing a using directive or an assembly reference?)
        {
            Package entity = helper.GetPackage(model.PackageId);
            model.Description = string.Format("{0} Messages to Other Users" , model.Days);
            
            Payment paymentModel = new Payment()
            {
                PackageId = model.PackageId,
                Description = model.Description,
                HolderName = User.Info.FullName,
                PayeeEmail = User.Username,
                RUrl = model.ReturnUrl,
                SessionId = model.SessionId,
                Amount = model.TotalAmount,
                Type = "M",
                Id = model.Id,  
                Days = model.Days,
                PaymentMode = 1
            };
            return View(paymentModel);
        }

        [Authorize(Roles = "Individual, Company")]
        [HttpGet]
        public ActionResult InterviewPriceList(string returnUrl = null, string type = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Type = type;
            ViewBag.BuyInterview = helper.InterviewPrice(User.Info.CountryId.Value);

            return View();
        }

#pragma warning disable CS0246 // The type or namespace name 'BuyPromote' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult ProcessInterview(BuyPromote model)
#pragma warning restore CS0246 // The type or namespace name 'BuyPromote' could not be found (are you missing a using directive or an assembly reference?)
        {
            Package entity = helper.GetPackage(model.PackageId);
            model.Description = string.Format("{0} Online Interviews", model.Days);

            Payment paymentModel = new Payment()
            {
                PackageId = model.PackageId,
                Description = model.Description,
                HolderName = User.Info.FullName,
                PayeeEmail = User.Username,
                RUrl = model.ReturnUrl,
                SessionId = model.SessionId,
                Amount = model.TotalAmount,
                Type = "I",
                Id = model.Id,
                Days = model.Days
            };
            return View(paymentModel);
        }

        [Authorize(Roles = "Individual, Company")]
        [HttpGet]
        public ActionResult ProfilePriceList(string returnUrl = null, string type = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Type = type;
            ViewBag.BuyProfile = helper.ProfilePrice(User.Info.CountryId.Value);

            return View();
        }

#pragma warning disable CS0246 // The type or namespace name 'BuyPromote' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult ProcessProfile(BuyPromote model)
#pragma warning restore CS0246 // The type or namespace name 'BuyPromote' could not be found (are you missing a using directive or an assembly reference?)
        {
            Package entity = helper.GetPackage(model.PackageId);
            model.Description = string.Format("{0} Online Profiles Views", model.Days);

            Payment paymentModel = new Payment()
            {
                PackageId = model.PackageId,
                Description = model.Description,
                HolderName = User.Info.FullName,
                PayeeEmail = User.Username,
                RUrl = model.ReturnUrl,
                SessionId = model.SessionId,
                Amount = model.TotalAmount,
                Type = "P",
                Id = model.Id,
                Days = model.Days
            };
            return View(paymentModel);
        }

        [Authorize(Roles = "Individual, Company")]
#pragma warning disable CS0246 // The type or namespace name 'PackageSelect' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<ActionResult> Select(PackageSelect model)
#pragma warning restore CS0246 // The type or namespace name 'PackageSelect' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Package> list = new List<Package>();
            if (Request.IsAjaxRequest())
            {
                ResponseContext context = null;
                try
                {                    
                    list = await helper.PackageList(model.CountryId, null, User.Info.Type, model.Type, 1);                    
                    model.List = list;

                    context = new ResponseContext()
                    {
                        Id = 1,
                        Data = model
                    };
                }
                catch (Exception ex)
                {
                    context = new ResponseContext()
                    {
                        Id = -1,
                        Message = ex.Message
                    };
                }
                return Json(context, JsonRequestBehavior.AllowGet);
            }
            else
            {                               
                list = await helper.PackageList(model.CountryId, null, User.Info.Type, model.Type, 1);                
                model.List = list;
            }
            
            return View(model);
        }
       

        [Authorize(Roles = "Individual, Company")]
        [HttpGet]
        public ActionResult PriceList()
        {            
            PackageSelect model = new PackageSelect()
            {                
                CountryId = User.Info.CountryId.Value             
            };
            return View(model);
        }

        [Authorize(Roles = "Individual, Company")]
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'PackageSelect' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<ActionResult> PriceList(PackageSelect model)
#pragma warning restore CS0246 // The type or namespace name 'PackageSelect' could not be found (are you missing a using directive or an assembly reference?)
        {
            ResponseContext context = null;
            try
            {
                List<Package> list = await helper.PackageList(model.CountryId, null, 1);
                model.List = list;

                context = new ResponseContext()
                {
                    Id = 1,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                context = new ResponseContext()
                {
                    Id = -1,
                    Message = ex.Message
                };
            }
            return Json(context, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Individual, Company")]
        [HttpPost]
        public async Task<ActionResult> BoostList(int id)
        {
            ResponseContext context = null;
            try
            {
                List<Boost> list = await helper.BoostList(id);

                if (list.Count > 0)
                {
                    context = new ResponseContext()
                    {
                        Id = 1,
                        Data = list
                    };
                }
                else
                {
                    context = new ResponseContext()
                    {
                        Id = 0,
                        Data = list
                    };
                }
            }
            catch (Exception ex)
            {
                context = new ResponseContext()
                {
                    Id = -1,
                    Message = ex.Message
                };
            }
            return Json(context, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Individual, Company")]
        [HttpGet]
        public ActionResult Process(int packageId, string type, Guid? id = null, string returnUrl = null)
        {            
            Payment model = new Payment();
            string description = string.Empty;
            Package entity = helper.GetPackage(packageId);
            if (entity != null)
            {                
                if (entity.TypeName == "Company")
                {
                    if (entity.Days != null)
                    {
                        description = string.Format("{0} package with {1} Jobs, {2} Profiles, {3} Messages, {4} Interviews, {5} Resumes Download, Promote Job for {6} days", entity.Name, entity.Jobs, entity.Profiles, entity.Messages, entity.Interviews, entity.Downloads, entity.Days);
                    }
                    else
                    {
                        description = string.Format("{0} package {1} Profiles, {2} Messages, {3} Interviews, {4} Resumes Download", entity.Name, entity.Profiles, entity.Messages, entity.Interviews, entity.Downloads);
                    }
                }
                else
                {
                    if (entity.Days != null)
                    {
                        description = string.Format("{0} package with {1} Profiles, {2} Messages, Promote Profile for {3} days", entity.Name, entity.Profiles, entity.Messages, entity.Days);
                    }
                    else
                    {
                        description = string.Format("{0} package with {1} Profiles, {2} Messages", entity.Name, entity.Profiles, entity.Messages);
                    }
                }
            }

            model = new Payment()
            {
                PackageId = packageId,
                Description = description,
                HolderName = User.Info.FullName,
                PayeeEmail = User.Username,
                RUrl = returnUrl,
                SessionId = id,
                Amount = entity.Rate,
                Type = type,                
            };

            if (entity.Name.Equals("Free"))
            {
                string jobData = DomainService.Instance.ReadData(id.Value);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                JobPostModel job_model = serializer.Deserialize<JobPostModel>(jobData);
                
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    var employer = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                    var permalink = job_model.Title;

                    permalink =
                        permalink.Replace('.', ' ')
                            .Replace(',', ' ')
                            .Replace('-', ' ')
                            .Replace(" - ", " ")
                            .Replace(" , ", " ")
                            .Replace('/', ' ')
                            .Replace(" / ", " ")
                            .Replace(" & ", " ")
                            .Replace("&", " ");
                    var pattern = "\\s+";
                    var replacement = " ";
                    permalink = Regex.Replace(permalink, pattern, replacement).Trim().ToLower();
                    permalink = permalink.Replace(' ', '-');

                    var job = new Job();
                    job.Title = job_model.Title.TitleCase();

                    string desc = Sanitizer.GetSafeHtmlFragment(job_model.Description);
                    desc = desc.RemoveEmails();
                    desc = desc.RemoveNumbers();
                    job.Description = desc.RemoveWebsites();

                    string summary = job_model.Summary;
                    summary = summary.RemoveEmails();
                    summary = summary.RemoveNumbers();
                    summary = summary.RemoveWebsites();
                    job.Summary = summary;

                    string requirements = job_model.Requirements;
                    requirements = requirements.RemoveEmails();
                    requirements = requirements.RemoveNumbers();
                    requirements = requirements.RemoveWebsites();
                    job.Requirements = requirements;
                    string resp = job_model.Responsibilities;
                    resp = resp.RemoveEmails();
                    resp = resp.RemoveNumbers();
                    resp = resp.RemoveWebsites();
                    job.Responsilibies = resp;

                    job.IsFeaturedJob = false;
                    job.CategoryId = job_model.CategoryId;
                    job.SpecializationId = job_model.SpecializationId;
                    job.CountryId = job_model.CountryId;
                    job.StateId = job_model.StateId;
                    job.City = job_model.City;
                    job.Zip = job_model.Zip;
                    job.EmploymentTypeId = job_model.EmployementTypeId;
                    job.QualificationId = job_model.Qualification;
                    job.MinimumAge = (byte?)job_model.MinimumAge;
                    job.MaximumAge = (byte?)job_model.MaximumAge;
                    job.MinimumExperience = (byte)job_model.MinimumExperience;
                    job.MaximumExperience = (byte)job_model.MaximumExperience;
                    job.MinimumSalary = job_model.MinimumSalary;
                    job.MaximumSalary = job_model.MaximumSalary;
                    job.Currency = job_model.Currency;
                    job.PublishedDate = DateTime.Now;
                    job.ClosingDate = DateTime.Now.AddMonths(1);
                    job.PermaLink = permalink;
                    job.EmployerId = employer.UserId;
                    job.IsActive = true;
                    job.IsDeleted = false;
                    job.IsPostedOnTwitter = false;
                    job.InEditMode = false;
                    job.Distribute = (job_model.Distribute == 1);
                    job.IsPaid = job_model.IsPaid;
                    var job_id = Convert.ToInt64(dataHelper.Add<Job>(job, User.Username));

                    if (job_id > 0)
                    {                      
                        var tracking = new Tracking
                        {
                            Id = Guid.NewGuid(),
                            JobId = job.Id,
                            UserId = employer.UserId,
                            Type = (int)TrackingTypes.PUBLISHED,
                            DateUpdated = DateTime.Now,
                            IsDownloaded = false
                        };

                        dataHelper.Add<Tracking>(tracking);

                        var reader = new StreamReader(Server.MapPath("~/Templates/Mail/employer_postjob.html"));
                        var body = string.Empty;

                        if (reader != null)
                        {
                            body = reader.ReadToEnd();
                            body = body.Replace("@@employer", employer.Company);
                            body = body.Replace("@@jobtitle", job.Title);
                            if (job.IsFeaturedJob.Value)
                            {
                                body = body.Replace("@@featured",
                                    "This is featured job which will appear at main page as well as on top of search results!");
                            }
                            else
                            {
                                body = body.Replace("@@featured", "");
                            }
                            body = body.Replace("@@joburl",
                                string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job_id));

                            string[] receipent = { employer.Username };
                            var subject = string.Format("Thanks for Posting {0} Job", job.Title);

                            var recipients = new List<Recipient>();
                            recipients.Add(new Recipient
                            {
                                Email = employer.Username,
                                DisplayName = string.Format("{0} {1}", employer.FirstName, employer.LastName),
                                Type = RecipientTypes.TO
                            });

                            List<int> typeList = new List<int>() { (int)SecurityRoles.Administrator, (int)SecurityRoles.SuperUser };
                            var profileList = dataHelper.Get<UserProfile>().Where(x => typeList.Contains(x.Type)).ToList();
                            foreach (var profile in profileList)
                            {
                                recipients.Add(new Recipient
                                {
                                    Email = profile.Username,
                                    DisplayName = string.Format("{0} {1}", profile.FirstName, profile.LastName),
                                    Type = RecipientTypes.BCC
                                });
                            }
                            AlertService.Instance.SendMail(subject, recipients, body);
                        }
                        TempData["UpdateData"] = string.Format("{0} job has been submitted successfully. It is in approval process, we will inform you once it is approved!", job_model.Title);
                    }
                }                
                DomainService.Instance.RemoveData(id.Value);

                return Redirect(returnUrl);
            }
            else
            {

                return View(model);
            }
        }

        [Authorize(Roles = "Individual, Company")]
        [HttpPost]
        public ActionResult Process(Guid id)
        {            
            return View();
        }

#pragma warning disable CS0246 // The type or namespace name 'BuyPromote' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult ProcessDownload(BuyPromote model)
#pragma warning restore CS0246 // The type or namespace name 'BuyPromote' could not be found (are you missing a using directive or an assembly reference?)
        {
            Package entity = helper.GetPackage(model.PackageId);
            if (entity != null)
            {
                if (entity.TypeName == "Company")
                {
                    string description = string.Format("{0} Resumes Download", model.Days);
                    model.Description = description;
                }
            }

            Payment paymentModel = new Payment()
            {
                PackageId = model.PackageId,
                Description = model.Description,
                HolderName = User.Info.FullName,
                PayeeEmail = User.Username,
                RUrl = model.ReturnUrl,
                SessionId = null,
                Amount = model.TotalAmount,
                Type = model.Type,
                Id = model.Id,
                Days = model.Days,
                PaymentMode = 1
            };
            return View(paymentModel);
        }

        [Authorize(Roles = "Individual, Company")]     
#pragma warning disable CS0246 // The type or namespace name 'BuyPromote' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult ProcessPromotion(BuyPromote model)
#pragma warning restore CS0246 // The type or namespace name 'BuyPromote' could not be found (are you missing a using directive or an assembly reference?)
        {
            Package entity = helper.GetPackage(model.PackageId);            
            if (entity != null)
            {                
                if (entity.TypeName == "Company")
                {                    
                    model.Description = string.Format("{0} Job with Social Network, Promote Job in Searches and to Jobseekers for {1} days", entity.Jobs, model.Days);
                }
                else
                {
                    model.Description = string.Format("Promote Profile in Searches and to Employers for {0} days", model.Days);
                }
            }

            Payment paymentModel = new Payment()
            {
                PackageId = model.PackageId,
                Description = model.Description,
                HolderName = User.Info.FullName,
                PayeeEmail = User.Username,
                RUrl = model.ReturnUrl,
                SessionId = null,
                Amount = model.TotalAmount,
                Type = model.Type,
                Id = model.Id,
                Days = model.Days,
            };
            return View(paymentModel);
        }

        [Authorize(Roles = "Individual")]
        [HttpGet]
        public ActionResult PrePromote(long Id, string type, string returnUrl)
        {            
            UserProfile member = MemberService.Instance.Get(User.Username);           
            PreJobApplyModel model = new PreJobApplyModel()
            {
                Title = member.Title,
                CategoryId = member.CategoryId,
                SpecializationId = member.SpecializationId,
                MobileCountryCode = member.MobileCountryCode,
                Telephone = member.Mobile,
                ReturnUrl = returnUrl,
                Type = type
            };
            List Country = new List();
            if (member.CountryId != null)
            {
                Country = SharedService.Instance.GetCountry(member.CountryId.Value);
                model.MobileCountryCode = string.Format("+{0}", Country.Code);
            }
            return View(model);
        }

        [Authorize(Roles = "Individual")]
        [HttpPost]        
        public ActionResult PrePromote(PreJobApplyModel model)
        {
            UserProfile original = MemberService.Instance.Get(User.Id);
            if (ModelState.IsValid)
            {
                var fileSize = Convert.ToInt32(ConfigService.Instance.GetConfigValue("UploadFileSize"));
                var actualFileSize = fileSize * 1024;
                HttpPostedFileBase rFile = model.Resume;
                if (rFile != null && rFile.ContentLength > actualFileSize)
                {
                    TempData["Error"] = string.Format("Your resume size exceeds the upload limit({0} KB).", fileSize);
                }
                else
                {
                    if (rFile != null)
                    {
                        string fileName = rFile.FileName;
                        if (fileName.Contains("\\"))
                        {
                            fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                        }
                        var extension = fileName.Substring(fileName.LastIndexOf(".") + 1).ToUpper();
                        if (extension == "PDF" || extension == "DOC" || extension == "DOCX")
                        {
                            var file = rFile.InputStream;
                            var buffer = new byte[file.Length];
                            file.Read(buffer, 0, (int)file.Length);
                            file.Close();

                            original.Content = Convert.ToBase64String(buffer);
                            original.FileName = fileName;
                        }
                        else
                        {
                            TempData["Error"] = "Only .doc, .docx, .pdf files are allowed!";
                            return View(model);
                        }
                    }

                    original.Title = model.Title;
                    original.CategoryId = model.CategoryId;
                    original.SpecializationId = model.SpecializationId;
                    original.CurrentEmployer = model.CurrentEmployer;
                    original.CurrentEmployerFromMonth = model.FromMonth;
                    original.CurrentEmployerFromYear = model.FromYear;
                    original.CurrentEmployerToMonth = model.ToMonth;
                    original.CurrentEmployerToYear = model.ToYear;
                    if (!string.IsNullOrEmpty(model.MobileCountryCode))
                    {
                        original.MobileCountryCode = model.MobileCountryCode;
                        original.Mobile = model.Telephone;
                    }
                    original.Summary = model.Summary;
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        dataHelper.Update<UserProfile>(original, User.Username);
                    }

                    if (!string.IsNullOrEmpty(original.Title) && original.CategoryId != null && original.SpecializationId != null && !string.IsNullOrEmpty(original.Content))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                }
            }

            if (string.IsNullOrEmpty(original.Content))
            {
                TempData["Error"] = "Please upload or build your resume online!";
            }
            return View(model);
        }
    }
}