using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
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
using JobPortal.Payments;
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
using JobPortal.Web.App_Start;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Web.Models;
using Microsoft.Security.Application;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace JobPortal.Web.Controllers
{
    [Authorize]
    public class PaymentController : BaseController
    {
#pragma warning disable CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
        IHelperService helper;
#pragma warning restore CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
        public PaymentController(IUserService service, IHelperService helper)
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
            : base(service)
        {
            this.helper = helper;
        }

        [HttpGet]
        public ActionResult Process(string rurl, int PackageId, string type = null, Guid? id = null)
        {
            JobPortal.Web.Models.Payment model = new JobPortal.Web.Models.Payment();

            model.RUrl = rurl;
            model.PackageId = PackageId;
            Package package = DomainService.Instance.GetPackage(PackageId);
            model.Amount = package.Rate;

            string payment_gateway = ConfigService.Instance.GetConfigValue("PaymentGateway");

            return View(model);
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Cancel(string returnurl, Guid? id = null)
        {
            if (id != null)
            {
                if (returnurl.Contains("?"))
                {
                    string sid = string.Format("{0}&sid={1}", returnurl, id);
                    return (ActionResult)Redirect(sid);
                }
                else
                {
                    string sid = string.Format("{0}?sid={1}", returnurl, id);
                    return (ActionResult)Redirect(sid);
                }
            }
            else
            {
                return (ActionResult)Redirect(returnurl);
            }
        }

        [UrlPrivilegeFilter]
        public ActionResult Checkout(JobPortal.Web.Models.Payment model, Guid? payment_id = null)
        {
            Package package = DomainService.Instance.GetPackage(model.PackageId);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Guid request_id = Guid.NewGuid();
            if (payment_id == null)
            {
                string pData = serializer.Serialize(model);
                DomainService.Instance.StoreData(request_id, pData);
            }
            else
            {
                string pData = DomainService.Instance.ReadData(payment_id.Value);
                serializer = new JavaScriptSerializer();
                model = serializer.Deserialize<JobPortal.Web.Models.Payment>(pData);
                package = DomainService.Instance.GetPackage(model.PackageId);
                DomainService.Instance.RemoveData(payment_id.Value);
            }

            PaymentResponse response = null;
            //PaymentService service = new PaymentService(new PayPalService());
            //model.CardNumber = "4032035800825515";
            //model.ExpiryMonth ="8";
            //model.ExpiryYear ="2026";
            //model.SecurityCode = "333";
            if (string.IsNullOrEmpty(model.CardNumber) && string.IsNullOrEmpty(model.ExpiryMonth) && string.IsNullOrEmpty(model.ExpiryYear) && string.IsNullOrEmpty(model.SecurityCode))
            {
                string desc = model.Description;
                if (!string.IsNullOrEmpty(Request.Params["cancel"]) && Request.Params["cancel"].Equals("true"))
                {
                    TempData["Error"] = "Payment process canceled successfully!";
                    return RedirectToAction("Select", "Package", new { returnurl = Request.Params["RUrl"] });
                }

                SubscriptionInformation subscriptionInf = new SubscriptionInformation()
                {
                    PaymentId = Request.Params["paymentId"],
                    PayerId = Request.Params["PayerID"],
                    ItemId = model.PackageId,
                    Name = package.Name,
                    Amount = model.Amount.ToString("#0.00"),
                    Description = model.Description,
                    Url = string.Format("{0}://{1}/Payment/Checkout?{2}&RUrl={3}&payment_id={4}", Request.Url.Scheme, Request.Url.Authority, Guid.NewGuid().ToString(), model.RUrl, request_id)
                };

                if (string.IsNullOrEmpty(subscriptionInf.PayerId))
                {
                    response = PaymentService.ExecutePayment(subscriptionInf);
                    return new RedirectResult(response.Url);
                }
                else
                {
                    response = PaymentService.ExecutePayment(subscriptionInf);
                    response.Method = "PayPal";
                }
            }
            else
            {

                string desc = model.Description;

                string payment_gateway = ConfigService.Instance.GetConfigValue("PaymentGateway");
                if (payment_gateway.Equals("PayPal"))
                {
                    model.HolderName = "M Ch";
                    model.Amount = 5.00m;
                    string[] names = model.HolderName.Split(' ');
                    CreditCard card = new CreditCard()
                    {
                        FirstName = names[0],
                        LastName = names[names.Length - 1],
                        Number = model.CardNumber,
                        ExpiryMonth = model.ExpiryMonth,
                        ExpiryYear = model.ExpiryYear,
                        SecurityCode = model.SecurityCode,
                        Amount = model.Amount.ToString(),
                        Email = User.Username
                    };

                    response = PaymentService.ExecuteCreditCardPayment(card, request_id.ToString());
                    //var gateway = config.GetGateway();
                }

            }

            if (response != null && !string.IsNullOrEmpty(response.Status) && response.Status.Equals("approved"))
            {
                serializer = null;
                JobPostModel job_model = null;
                string fullName = User.Info.FullName;
                if (model.SessionId != null)
                {
                    string jobData = DomainService.Instance.ReadData(model.SessionId.Value);
                    serializer = new JavaScriptSerializer();
                    job_model = serializer.Deserialize<JobPostModel>(jobData);
                }
                CreditEntry entry = new CreditEntry()
                {
                    UserId = User.Id,
                    PackageId = model.PackageId,
                    Description = model.Description,
                    BillingZip = model.BillingZip,
                    Amount = model.Amount,
                    TransactionId = response.Id,
                    Method = response.Method,
                };
                if (model.Type.Equals("I"))
                {
                    entry.Profiles = null;
                    entry.Messages = null;
                    entry.Interviews = model.Days;
                    entry.Resumes = null;
                }
                else if (model.Type.Equals("M"))
                {
                    entry.Profiles = null;
                    entry.Messages = model.Days;
                    entry.Interviews = null;
                    entry.Resumes = null;
                }
                else if (model.Type.Equals("J"))
                {
                    entry.Profiles = package.Profiles;
                    entry.Messages = package.Messages;
                    entry.Interviews = package.Interviews;
                    entry.Resumes = package.Downloads;
                }
                else if (model.Type.Equals("P"))
                {
                    entry.Profiles = model.Days;
                    entry.Messages = null;
                    entry.Interviews = null;
                    entry.Resumes = null;
                }
                else if (model.Type.Equals("R"))
                {
                    entry.Profiles = null;
                    entry.Messages = null;
                    entry.Interviews = null;
                    entry.Resumes = model.Days;
                }
                else if (model.Type.Equals("JR"))
                {
                    entry.Profiles = null;
                    entry.Messages = null;
                    entry.Interviews = null;
                    entry.Resumes = model.Days;
                }
                long invoice_id = _service.ManageTransaction(entry);

                using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/payment_success.html")))
                {
                    string body = reader.ReadToEnd();
                    body = body.Replace("@@receiver", fullName);
                    string durl = string.Format("{0}://{1}/billing?userId={2}", Request.Url.Scheme, Request.Url.Authority, User.Id);
                    body = body.Replace("@@durl", durl);
                    string[] receipent = { User.Info.Username };

                    AlertService.Instance.SendMail("Payment Received", receipent, body);
                }
                if (model.SessionId != null)
                {
                    if (model.Type == "M")
                    {
                        Send(model.SessionId.Value);
                    }
                    if (model.Type == "J")
                    {
                        PostJob(model.SessionId.Value, model.PackageId);
                    }
                }
                return RedirectToAction("Success", new { returnurl = model.RUrl });
            }
            else
            {
                TempData["Error"] = "<b>Payment unsuccessful</b><br/>" + response.Message.Replace("\n", "<br/>");
                return RedirectToAction("Failed");
            }
        }

        public ActionResult PromotionCheckout(Payment model, Guid? payment_id = null)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Guid request_id = Guid.NewGuid();
            if (payment_id == null)
            {
                string pData = serializer.Serialize(model);
                DomainService.Instance.StoreData(request_id, pData);
            }
            else
            {
                string pData = DomainService.Instance.ReadData(payment_id.Value);
                serializer = new JavaScriptSerializer();
                model = serializer.Deserialize<Payment>(pData);
                DomainService.Instance.RemoveData(payment_id.Value);
            }

            PaymentResponse response = null;
            if (string.IsNullOrEmpty(model.CardNumber) && string.IsNullOrEmpty(model.ExpiryMonth) && string.IsNullOrEmpty(model.ExpiryYear) && string.IsNullOrEmpty(model.SecurityCode))
            {
                if (!string.IsNullOrEmpty(Request.Params["cancel"]) && Request.Params["cancel"].Equals("true"))
                {
                    TempData["Error"] = "Payment process canceled successfully!";
                    return RedirectToAction("Select", "Package", new { returnurl = Request.Params["RUrl"] });
                }

                SubscriptionInformation redirectEntity = new SubscriptionInformation()
                {
                    PaymentId = Request.Params["paymentId"],
                    PayerId = Request.Params["PayerID"],
                    ItemId = model.PackageId,
                    //ItemName = model.Description,
                    Amount = model.Amount.ToString("#0.00"),
                    Description = model.Description,
                    Url = string.Format("{0}://{1}/Payment/PromotionCheckout?{2}&RUrl={3}&payment_id={4}", Request.Url.Scheme, Request.Url.Authority, Guid.NewGuid().ToString(), model.RUrl, request_id)
                };

                if (string.IsNullOrEmpty(redirectEntity.PayerId))
                {
                    response = PaymentService.ExecutePayment(redirectEntity);
                    return new RedirectResult(response.Url);
                }
                else
                {
                    response = PaymentService.ExecutePayment(redirectEntity);
                    response.Method = "PayPal";
                }
            }
            else
            {
                string desc = model.Description;

                string payment_gateway = ConfigService.Instance.GetConfigValue("PaymentGateway");
                if (payment_gateway.Equals("PayPal"))
                {
                    string[] names = model.HolderName.Split(' ');
                    CreditCard card = new CreditCard()
                    {
                        FirstName = names[0],
                        LastName = names[names.Length - 1],
                        Number = model.CardNumber,
                        ExpiryMonth = model.ExpiryMonth,
                        ExpiryYear = model.ExpiryYear,
                        SecurityCode = model.SecurityCode,
                        Amount = model.Amount.ToString(),
                        Email = User.Username
                    };

                    response = PaymentService.ExecuteCreditCardPayment(card, request_id.ToString());
                    // var gateway = config.GetGateway();
                }

            }

            if (response != null && !string.IsNullOrEmpty(response.Status) && response.Status.Equals("approved"))
            {
                string fullName = User.Info.FullName;

                CreditEntry entry = new CreditEntry()
                {
                    UserId = User.Id,
                    PackageId = model.PackageId,
                    Description = model.Description,
                    BillingZip = model.BillingZip,
                    Amount = model.Amount,
                    TransactionId = response.Id,
                    Method = response.Method
                };

                long invoice_id = _service.ManageTransaction(entry);

                using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/payment_success.html")))
                {
                    string body = reader.ReadToEnd();
                    body = body.Replace("@@receiver", fullName);
                    string durl = string.Format("{0}://{1}/billing?userId={2}", Request.Url.Scheme, Request.Url.Authority, User.Id);
                    body = body.Replace("@@durl", durl);
                    string[] receipent = { User.Info.Username };

                    AlertService.Instance.SendMail("Payment Received", receipent, body);
                }

                PromoteItem item = new PromoteItem()
                {
                    Type = model.Type,
                    PackageId = model.PackageId,
                    Id = model.Id,
                    Days = model.Days,
                    TransactionId = response.Id,
                    Method = response.Method
                };
                helper.ManagePromotion(item);

                return RedirectToAction("Success", new { returnurl = model.RUrl });
            }
            else
            {
                TempData["Error"] = "<b>Payment unsuccessful</b><br/>" + response.Message.Replace("\n", "<br/>");
                return RedirectToAction("Failed");
            }
        }

        public ActionResult DownloadCheckout(JobPortal.Web.Models.Payment model)
        {
            PaymentResponse response = null;
            string paymentID = Request.Params["paymentId"];
            string payerId = Request.Params["PayerID"];

            if (string.IsNullOrEmpty(model.CardNumber) && string.IsNullOrEmpty(model.ExpiryMonth) && string.IsNullOrEmpty(model.ExpiryYear) && string.IsNullOrEmpty(model.SecurityCode))
            {
                string desc = model.Description;
                if (!string.IsNullOrEmpty(Request.Params["cancel"]) && Request.Params["cancel"].Equals("true"))
                {
                    TempData["Error"] = "Payment process canceled successfully!";
                    return RedirectToAction("Select", "Package", new { returnurl = Request.Params["RUrl"] });
                }


                SubscriptionInformation redirectEntity = new SubscriptionInformation()
                {
                    PaymentId = paymentID,
                    PayerId = payerId,
                    ItemId = model.PackageId,
                    //ItemName = model.Description,
                    Amount = model.Amount.ToString(),
                    Description = model.Description,
                    Url = string.Format("{0}://{1}/Payment/Checkout?{2}&RUrl={3}&PackageId={4}&type={5}", Request.Url.Scheme, Request.Url.Authority, Guid.NewGuid().ToString(), model.RUrl, model.PackageId, model.Type)
                };

                if (string.IsNullOrEmpty(redirectEntity.PayerId))
                {
                    response = PaymentService.ExecutePayment(redirectEntity);
                    return new RedirectResult(response.Url);
                }
                else
                {
                    response = PaymentService.ExecutePayment(redirectEntity);
                    response.Method = "PayPal";
                }
            }
            else
            {
                string desc = model.Description;
                //string PaymentId = request_id.ToString();
                string payment_gateway = ConfigService.Instance.GetConfigValue("PaymentGateway");
                if (payment_gateway.Equals("PayPal"))
                {
                    string[] names = model.HolderName.Split(' ');
                    CreditCard card = new CreditCard()
                    {
                        FirstName = names[0],
                        LastName = names[names.Length - 1],
                        Number = model.CardNumber,
                        ExpiryMonth = model.ExpiryMonth,
                        ExpiryYear = model.ExpiryYear,
                        SecurityCode = model.SecurityCode,
                        Amount = model.Amount.ToString(),
                        Email = User.Username
                    };

                    response = PaymentService.ExecuteCreditCardPayment(card, paymentID);
                    // var gateway = config.GetGateway();
                }
            }

            if (response != null && !string.IsNullOrEmpty(response.Status) && response.Status.Equals("approved"))
            {
                string fullName = User.Info.FullName;

                CreditEntry entry = new CreditEntry()
                {
                    UserId = User.Id,
                    PackageId = model.PackageId,
                    Description = model.Description,
                    BillingZip = model.BillingZip,
                    Amount = model.Amount,
                    TransactionId = response.Id,
                    Method = response.Method,
                    Resumes = model.Days
                };

                long invoice_id = _service.ManageTransaction(entry);

                using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/payment_success.html")))
                {
                    string body = reader.ReadToEnd();
                    body = body.Replace("@@receiver", fullName);
                    string durl = string.Format("{0}://{1}/billing?userId={2}", Request.Url.Scheme, Request.Url.Authority, User.Id);
                    body = body.Replace("@@durl", durl);
                    string[] receipent = { User.Info.Username };

                    AlertService.Instance.SendMail("Payment Received", receipent, body);
                }

                return RedirectToAction("Success", new { returnurl = model.RUrl });
            }
            else
            {
                TempData["Error"] = "<b>Payment unsuccessful</b><br/>" + response.Message.Replace("\n", "<br/>");
                return RedirectToAction("Failed");
            }
        }

        public ActionResult Success(string returnurl)
        {
            TempData["UpdateData"] = TempData["UpdateData"];
            ViewBag.ReturnUrl = returnurl;
            return View();
        }
        public ActionResult Failed()
        {
            return View();
        }
        private void PostJob(Guid id, int packageId)
        {
            string jobData = DomainService.Instance.ReadData(id);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JobPostModel job_model = serializer.Deserialize<JobPostModel>(jobData);

            //JobListingModel job_model = Session["JobModel"] as JobListingModel;
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

                string description = Sanitizer.GetSafeHtmlFragment(job_model.Description);
                description = description.RemoveEmails();
                description = description.RemoveNumbers();
                job.Description = description.RemoveWebsites();

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
                job.IsPaid = true;
                job.PackageId = packageId;
                var job_id = Convert.ToInt64(dataHelper.Add<Job>(job, User.Username));

                if (job_id > 0)
                {
                    Package pkg = helper.GetPackage(packageId);
                    PromoteItem bj = new PromoteItem()
                    {
                        Type = "J",
                        PackageId = packageId,
                        Id = job.Id,
                        Days = pkg.Days.Value
                    };

                    helper.ManagePromotion(bj);
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
            DomainService.Instance.RemoveData(id);
        }
        private void Send(Guid id)
        {
            string msgData = DomainService.Instance.ReadData(id);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            SendMessageModel model = serializer.Deserialize<SendMessageModel>(msgData);

            var sender = MemberService.Instance.Get(User.Username);
            var receiver = MemberService.Instance.Get(model.ReceiverEmail);

            bool registered = false;
            bool connected = false;
            bool blocked = false;
            string[] receipent = new string[1];
            string subject;

            if (sender != null && receiver != null)
            {
                string receiverName = (!string.IsNullOrEmpty(receiver.Company) ? receiver.Company : string.Format("{0} {1}", receiver.FirstName, receiver.LastName));
                string senderName = (!string.IsNullOrEmpty(sender.Company) ? sender.Company : string.Format("{0} {1}", sender.FirstName, sender.LastName));

                var mrelation = ConnectionHelper.GetEntity(model.ReceiverEmail, sender.Username);
                var orelation = ConnectionHelper.GetEntity(sender.Username, model.ReceiverEmail);

                var blockedEntity = ConnectionHelper.GetBlockedEntity(receiver.UserId, sender.UserId);

                registered = (receiver != null);
                connected = ConnectionHelper.IsConnected(receiver.Username, sender.Username);
                blocked = ConnectionHelper.IsBlocked(sender.Username, receiver.UserId);

                Communication entity = new Communication();
                string msg = "";
                if (!string.IsNullOrEmpty(model.Message))
                {
                    msg = model.Message.RemoveEmails();
                    msg = msg.RemoveNumbers();
                    msg = msg.RemoveWebsites();
                }

                if (registered == true && connected == true && blocked == false)
                {
                    if (MessageService.Instance.Count(sender.UserId, receiver.UserId) == 0)
                    {
                        _service.ManageAccount(User.Id, null, 1);
                    }
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
                            _service.ManageAccount(User.Id, null, 1);

                            using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitationmessage.html")))
                            {
                                string body = reader.ReadToEnd();
                                body = body.Replace("@@sender", string.Format("{0}", senderName));
                                string url = string.Format("{0}://{1}/Message/Index", Request.Url.Scheme, Request.Url.Authority);
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
                                _service.ManageAccount(User.Id, null, 1);

                                using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitationmessage.html")))
                                {
                                    string body = reader.ReadToEnd();

                                    body = body.Replace("@@sender", string.Format("{0}", senderName));
                                    string url = string.Format("{0}://{1}/Message/Index", Request.Url.Scheme, Request.Url.Authority);
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
                                    _service.ManageAccount(User.Id, null, 1);

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
                                    _service.ManageAccount(User.Id, null, 1);

                                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitationmessage.html")))
                                    {
                                        string body = reader.ReadToEnd();

                                        body = body.Replace("@@sender", string.Format("{0}", senderName));
                                        string url = string.Format("{0}://{1}/Message/Index", Request.Url.Scheme, Request.Url.Authority);
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
            else
            {
                TempData["UpdateData"] = "Message sending failed, unable to retrive receipent details!";
            }
            DomainService.Instance.RemoveData(id);
        }

        [Authorize]
        public ActionResult Invoice(long userId, long Id)
        {
            //UserProfile profile = MemberService.Instance.Get(User.Id);
            TransactionEntity entity = DomainService.Instance.GetInvoice(userId, Id);
            UserProfile invoiceUser = MemberService.Instance.Get(userId);
            Byte[] buffer;

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

                        StreamReader sreader = new StreamReader(Server.MapPath("~/Templates/Invoice.html"));

                        //Our sample HTML and CSS
                        var resume_html = sreader.ReadToEnd();
                        if (invoiceUser.Type == (int)SecurityRoles.Employers)
                        {
                            resume_html = resume_html.Replace("@@receiver", invoiceUser.Company);
                        }
                        else if (invoiceUser.Type == (int)SecurityRoles.Jobseeker)
                        {
                            resume_html = resume_html.Replace("@@receiver", string.Format("{0} {1}", invoiceUser.FirstName, invoiceUser.LastName));
                        }

                        StringBuilder address = new StringBuilder();
                        address.Append(invoiceUser.Address);
                        if (!string.IsNullOrEmpty(invoiceUser.City))
                        {
                            address.AppendFormat("<br/>{0}", invoiceUser.City);
                        }
                        if (invoiceUser.StateId != null)
                        {
                            JobPortal.Data.List state = SharedService.Instance.GetCountry(invoiceUser.StateId.Value);
                            address.AppendFormat("<br/>{0}", state.Text);
                        }
                        if (!string.IsNullOrEmpty(invoiceUser.Zip))
                        {
                            address.AppendFormat(" - {0}", invoiceUser.Zip);
                        }
                        if (invoiceUser.CountryId != null)
                        {
                            JobPortal.Data.List country = SharedService.Instance.GetCountry(invoiceUser.CountryId.Value);
                            address.AppendFormat("<br/>{0}", country.Text);
                        }


                        resume_html = resume_html.Replace("@@address", address.ToString());
                        //resume_html = resume_html.Replace("@@phone", txtbPhone.Text.Trim());
                        resume_html = resume_html.Replace("@@email", invoiceUser.Username);
                        resume_html = resume_html.Replace("@@date", entity.TransactionDate.ToString("MMMM, dd, yyyy"));
                        resume_html = resume_html.Replace("@@method", entity.PaymentMethod);

                        resume_html = resume_html.Replace("@@description", entity.Description);
                        resume_html = resume_html.Replace("@@amount", string.Format("{0}", entity.Amount));

                        sreader.Close();

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
                buffer = ms.ToArray();
            }
            return File(buffer, MediaTypeNames.Application.Octet, string.Format("INVOICE{0}.pdf", entity.Id));
        }
        [Authorize]
        public ActionResult InvoiceDetails(long Id)
        {
            TransactionEntity entity = DomainService.Instance.GetInvoice(User.Id, Id);
            ViewBag.Usage = DomainService.Instance.GetUsageStatus(User.Id);
            return View(entity);
        }

        [Authorize]
        [UrlPrivilegeFilter]
        public ActionResult Billing(DateTime? StartDate, DateTime? EndDate, long userId, int pageNumber = 1)
        {
            ViewBag.DistributeNumbers = ConfigService.Instance.GetConfigValue("DistributeNumbers");
            int pageSize = 20;
            int rows = 0;
            List<TransactionEntity> list = DomainService.Instance.GetInvoiceList(userId, StartDate, EndDate, pageNumber, pageSize);
            if (list.Count > 0)
            {
                rows = list[0].MaxRows;
            }
            ViewBag.Model = new StaticPagedList<TransactionEntity>(list, pageNumber, pageSize, rows);
            ViewBag.Rows = rows;
            ViewBag.User = _service.Get(userId);
            ViewBag.Usage = DomainService.Instance.GetUsageStatus(userId);

            return View();
        }

        [Authorize]
        [UrlPrivilegeFilter]
        public ActionResult PaidProfiles(long userId, int pageNumber = 1)
        {
            int pageSize = 20;
            int rows = 0;
            List<PaidProfileEntity> list = DomainService.Instance.GetPaidProfiles(userId, pageNumber, pageSize);
            if (list.Count > 0)
            {
                rows = list.First().MaxRows;
            }
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<PaidProfileEntity>(list, pageNumber, pageSize, rows);
            return View();
        }
    }
}