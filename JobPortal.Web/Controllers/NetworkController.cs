using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CsvHelper;
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
using Exception = System.Exception;
using PagedList;
using Twilio;
using System.Text.RegularExpressions;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Web.Providers;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;
//using System.Net.Mail;
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Threading.Tasks;
using JobPortal.Web.App_Start;

namespace JobPortal.Web.Controllers
{
    public class NetworkController : BaseController
    {
#pragma warning disable CS0246 // The type or namespace name 'INetworkService' could not be found (are you missing a using directive or an assembly reference?)
        INetworkService iNetworkService;
#pragma warning restore CS0246 // The type or namespace name 'INetworkService' could not be found (are you missing a using directive or an assembly reference?)

#pragma warning disable CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
        IJobService jobService;
#pragma warning restore CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INetworkService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
        public NetworkController(IUserService service, INetworkService iNetworkService, IJobService jobService)
#pragma warning restore CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'INetworkService' could not be found (are you missing a using directive or an assembly reference?)
            : base(service)
        {
            this.iNetworkService = iNetworkService;
            this.jobService = jobService;
        }

        [Authorize(Roles = "Jobseeker, Company, Institute, Student,RecruitmentAgency,Interns")]
        [HttpGet]
        [UrlPrivilegeFilter]
        public ActionResult Index()
        {
            UserInfoEntity uinfo = _service.Get(User.Id);
            if (!uinfo.IsConfirmed)
            {
                return RedirectToAction("Confirm", "Account", new { id = uinfo.Id, returnUrl = Request.Url.ToString() });
            }
            return View(new ConnectionModel());
        }

        [Authorize(Roles = "Jobseeker, Company, Institute, Student,RecruitmentAgency,Interns")]
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'ConnectionFilter' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<ActionResult> Index(ConnectionFilter filter)
#pragma warning restore CS0246 // The type or namespace name 'ConnectionFilter' could not be found (are you missing a using directive or an assembly reference?)
        {
            UserInfoEntity uinfo = _service.Get(User.Id);
            if (!uinfo.IsConfirmed)
            {
                return RedirectToAction("Confirm", "Account", new { id = uinfo.Id, returnUrl = Request.Url.ToString() });
            }

            filter.UserId = User.Id;
            filter.PageSize = 10;
            if (filter.PageNumber == 0)
            {
                filter.PageNumber = 1;
            }
            List<ConnectionEntity> list = await iNetworkService.Connections(filter);
            //if (list.Count > 0)
            //{
            //    rows = list.FirstOrDefault().MaxRows;
            //}

            //ViewBag.Contacts = new StaticPagedList<ConnectionEntity>(list, filter.PageNumber, filter.PageSize, rows);
            //ViewBag.Total = rows;

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [UrlPrivilegeFilter]
        public ActionResult Connect(string EmailAddress, string redirect = "")
        {
            UserProfile UserInfo = MemberService.Instance.Get(User.Username);
            if (UserInfo != null && UserInfo.IsConfirmed == false)
            {
                //return RedirectToAction("ConfirmRegistration", "Account");
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.UserId, returnUrl = Request.Url.ToString() });
            }

            UserProfile registeredProfile = MemberService.Instance.Get(EmailAddress.ToLower());
            BlockedPeople entity = JobSeekerService.Instance.GetBlockedEntity(registeredProfile.UserId, UserInfo.UserId);
            if (entity != null && !entity.CreatedBy.Equals(UserInfo.Username))
            {
                return View("Blocked");
            }

            if (!string.IsNullOrEmpty(Request.QueryString["via"]))
            {
                Connection connection = ConnectionHelper.Get(EmailAddress, UserInfo.Username);
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

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                List<Connection> contact_list = new List<Connection>();
                bool byLoggedInUser = false;
                bool byEmailUser = false;

                if (UserInfo != null && registeredProfile != null)
                {
                    byEmailUser = ConnectionHelper.IsBlocked(User.Username, registeredProfile.UserId);
                    byLoggedInUser = ConnectionHelper.IsBlockedByMe(EmailAddress, UserInfo.UserId);
                }
                if (byEmailUser == false && byLoggedInUser == false)
                {
                    if (!UserInfo.Username.ToLower().Equals(EmailAddress.ToLower()))
                    {
                        Hashtable parameters = new Hashtable();
                        parameters.Add("UserId", UserInfo.UserId);
                        parameters.Add("EmailAddress", EmailAddress);
                        Connection contact = dataHelper.GetSingle<Connection>(parameters);
                        if (contact == null)
                        {
                            contact = new Connection()
                            {
                                UserId = UserInfo.UserId,
                                FirstName = registeredProfile != null && !string.IsNullOrEmpty(registeredProfile.FirstName) ? registeredProfile.FirstName.TitleCase() : "",
                                LastName = registeredProfile != null && !string.IsNullOrEmpty(registeredProfile.LastName) ? registeredProfile.LastName.TitleCase() : "",
                                EmailAddress = EmailAddress,
                                IsAccepted = false,
                                IsConnected = false,
                                IsBlocked = false,
                                Initiated = true,
                                IsDeleted = false,
                                Sent = true,
                                DateSent = DateTime.Now,
                                IsValid = true
                            };

                            dataHelper.Add<Connection>(contact, User.Username);

                            if (registeredProfile != null)
                            {
                                Connection networkContact = dataHelper.Get<Connection>().SingleOrDefault(x => x.UserId == registeredProfile.UserId && x.EmailAddress == UserInfo.Username);
                                if (networkContact == null)
                                {
                                    networkContact = new Connection()
                                    {
                                        UserId = registeredProfile.UserId,
                                        FirstName = !string.IsNullOrEmpty(UserInfo.FirstName) ? UserInfo.FirstName.TitleCase() : null,
                                        LastName = !string.IsNullOrEmpty(UserInfo.LastName) ? UserInfo.LastName.TitleCase() : null,
                                        EmailAddress = UserInfo.Username,
                                        IsAccepted = false,
                                        IsConnected = false,
                                        IsBlocked = false,
                                        Initiated = true,
                                        IsDeleted = false,
                                        Sent = true,
                                        DateSent = DateTime.Now,
                                        IsValid = true
                                    };

                                    dataHelper.Add<Connection>(networkContact, User.Username);
                                }

                            }
                            var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation.html"));
                            var body = reader.ReadToEnd();
                            string name = contact.FirstName + " " + contact.LastName;
                            var subject = string.Empty;
                            if (registeredProfile != null)
                            {
                                name = registeredProfile.FirstName + " " + registeredProfile.LastName;
                            }
                            else
                            {
                                name = contact.FirstName + " " + contact.LastName;
                                if (string.IsNullOrEmpty(name))
                                {
                                    name = contact.EmailAddress;
                                }
                                else if (name.Trim() != " ")
                                {
                                    name = contact.EmailAddress;
                                }
                            }

                            body = body.Replace("@@receiver", name);
                            if (UserInfo.Type == (int)SecurityRoles.Jobseeker)
                            {
                                body = body.Replace("@@sender", string.Format("{0} {1}", UserInfo.FirstName, UserInfo.LastName));

                                subject = string.Format("{0} Invites you to connect at Joblisting", string.Format("{0} {1}", UserInfo.FirstName, UserInfo.LastName));
                            }
                            else
                            {
                                body = body.Replace("@@sender", string.Format("{0}", !string.IsNullOrEmpty(UserInfo.Company) ? UserInfo.Company : string.Format("{0} {1}", UserInfo.FirstName, UserInfo.LastName)));
                                subject = string.Format("{0} Invites you to connect at Joblisting", !string.IsNullOrEmpty(UserInfo.Company) ? UserInfo.Company : string.Format("{0} {1}", UserInfo.FirstName, UserInfo.LastName));
                            }
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, UserInfo.PermaLink));

                            body = body.Replace("@@accepturl",
                                string.Format("{0}://{1}/Network/Accept/{2}", Request.Url.Scheme, Request.Url.Authority,
                                    contact.Id));
                            body = body.Replace("@@button", "Accept");

                            if (registeredProfile != null)
                            {
                                body = body.Replace("@@unsubscribe", "");
                            }
                            else
                            {
                                string ulink = string.Format("<a href=\"{0}://{1}/Network/Unsubscribe?Id={2}\">unsubscribe</a> or ", Request.Url.Scheme,
                                        Request.Url.Authority, contact.Id);

                                body = body.Replace("@@unsubscribe", ulink);
                            }


                            string[] receipent = { contact.EmailAddress };

                            AlertService.Instance.SendMail(subject, receipent, body);

                            TempData["SaveData"] = "Connection invitation initiated successfully!";
                        }
                        else
                        {
                            if (contact.IsDeleted)
                            {
                                contact.IsAccepted = false;
                                contact.IsConnected = false;
                                contact.IsDeleted = false;
                                contact.Initiated = true;
                                contact.CreatedBy = User.Username;
                                contact.DateUpdated = DateTime.Now;
                                contact.UpdatedBy = User.Username;

                                if (registeredProfile != null)
                                {
                                    Connection networkContact = dataHelper.Get<Connection>().SingleOrDefault(x => x.UserId == registeredProfile.UserId && x.EmailAddress == UserInfo.Username);
                                    if (networkContact != null)
                                    {
                                        networkContact.IsAccepted = false;
                                        networkContact.IsConnected = false;
                                        networkContact.IsDeleted = false;
                                        networkContact.Initiated = true;
                                        networkContact.CreatedBy = User.Username;
                                        networkContact.DateUpdated = DateTime.Now;
                                        networkContact.UpdatedBy = User.Username;

                                        dataHelper.UpdateEntity<Connection>(contact);
                                        dataHelper.UpdateEntity<Connection>(networkContact);
                                        dataHelper.Save();
                                    }
                                }

                                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation.html"));
                                var body = reader.ReadToEnd();
                                string name = contact.FirstName + " " + contact.LastName;
                                var subject = string.Empty;
                                if (registeredProfile != null)
                                {
                                    name = registeredProfile.FirstName + " " + registeredProfile.LastName;
                                }
                                else
                                {
                                    name = contact.FirstName + " " + contact.LastName;
                                    if (string.IsNullOrEmpty(name))
                                    {
                                        name = contact.EmailAddress;
                                    }
                                    else if (name.Trim() != " ")
                                    {
                                        name = contact.EmailAddress;
                                    }
                                }

                                body = body.Replace("@@receiver", name);
                                if (UserInfo.Type == (int)SecurityRoles.Jobseeker)
                                {
                                    body = body.Replace("@@sender", string.Format("{0} {1}", UserInfo.FirstName, UserInfo.LastName));

                                    subject = string.Format("{0} Invites you to connect at Joblisting", string.Format("{0} {1}", UserInfo.FirstName, UserInfo.LastName));
                                }
                                else
                                {
                                    body = body.Replace("@@sender", string.Format("{0}", !string.IsNullOrEmpty(UserInfo.Company) ? UserInfo.Company : string.Format("{0} {1}", UserInfo.FirstName, UserInfo.LastName)));
                                    subject = string.Format("{0} Invites you to connect at Joblisting", !string.IsNullOrEmpty(UserInfo.Company) ? UserInfo.Company : string.Format("{0} {1}", UserInfo.FirstName, UserInfo.LastName));
                                }
                                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, UserInfo.PermaLink));

                                body = body.Replace("@@accepturl",
                                    string.Format("{0}://{1}/Network/Accept/{2}", Request.Url.Scheme, Request.Url.Authority,
                                        contact.Id));
                                body = body.Replace("@@button", "Accept");


                                string[] receipent = { contact.EmailAddress };

                                AlertService.Instance.SendMail(subject, receipent, body);

                                TempData["SaveData"] = "Connection invitation sent successfully!";
                            }
                            else
                            {
                                if (contact.CreatedBy.Equals(User.Username))
                                {
                                    if (contact.IsAccepted == false && contact.IsConnected == false)
                                    {
                                        TempData["Error"] = "Connection invitation already sent, waiting for acceptance!";
                                    }
                                    else if (contact.IsAccepted == true & contact.IsConnected == true)
                                    {
                                        TempData["Error"] = string.Format("You are already connected with {0} {1}!", registeredProfile.FirstName, registeredProfile.LastName);
                                    }
                                }
                                else
                                {
                                    if (contact.IsAccepted == false && contact.IsConnected == false)
                                    {
                                        TempData["UpdateData"] = string.Format("Connection is already initiated by {0} {1} and waiting for  your acceptance!<br/>You may &nbsp;<a href=\"/Network/Accept?Id=" + contact.Id + "&redirect=/Network/Index\">Accept</a>&nbsp;or&nbsp;<a href=\"/Network/Disconnect?Id=" + contact.Id + "&redirect=/Network/Index\">Reject</a>", registeredProfile.FirstName, registeredProfile.LastName);
                                    }
                                    else if (contact.IsAccepted == true & contact.IsConnected == true)
                                    {
                                        TempData["Error"] = string.Format("You are already connected with {0} {1}!", registeredProfile.FirstName, registeredProfile.LastName);
                                    }
                                }
                            }
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
                        if (registeredProfile != null)
                        {
                            TempData["Error"] = "You have blocked this person!<br/>Do you want to Unblock or Skip?<br/><a href=\"/Home/Unblock?id=" + registeredProfile.UserId + "&redirect=/Network/Index\">Unblock</a>&nbsp;&nbsp;&nbsp;<a href=\"#\" onclick=\"window.location.reload();\">Skip</a>";
                        }
                    }
                    else if (byEmailUser)
                    {
                        if (UserInfo != null)
                        {
                            TempData["Error"] = "This profile is in private mode!";
                        }
                    }
                }
                contact_list = dataHelper.GetList<Connection>().Where(x => x.UserId == UserInfo.UserId).ToList();
                ViewBag.Contacts = contact_list;
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

        [Authorize]
        [UrlPrivilegeFilter]
        public ActionResult ConnectByPhone(string token)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var invite = context.Invites.SingleOrDefault(x => x.Token.Value.Equals(new Guid(token)));
                if (invite != null)
                {
                    UserProfile invitor = MemberService.Instance.Get(invite.UserId);
                    UserProfile invitee = MemberService.Instance.Get(User.Id);
                    if (!invitor.Username.Equals(invitee.Username))
                    {
                        bool connected = ConnectionHelper.IsConnected(invitor.Username, User.Id);
                        if (!connected)
                        {
                            Connection contact = dataHelper.Get<Connection>().SingleOrDefault(x => x.UserId == invitee.UserId && x.EmailAddress.Equals(invitor.Username));
                            if (contact == null)
                            {
                                contact = new Connection()
                                {
                                    UserId = invitee.UserId,
                                    FirstName = !string.IsNullOrEmpty(invitor.FirstName) ? invitor.FirstName.TitleCase() : "",
                                    LastName = !string.IsNullOrEmpty(invitor.LastName) ? invitor.LastName.TitleCase() : "",
                                    EmailAddress = invitor.Username,
                                    IsAccepted = true,
                                    IsConnected = true,
                                    IsBlocked = false,
                                    IsDeleted = false,
                                    Sent = true,
                                    DateSent = DateTime.Now,
                                    IsValid = true,
                                    CreatedBy = User.Username
                                };
                                dataHelper.Add<Connection>(contact, invitor.Username);
                            }

                            Connection friend = dataHelper.Get<Connection>().SingleOrDefault(x => x.UserId == invitor.UserId && x.EmailAddress.Equals(invitee.Username));
                            if (friend == null)
                            {
                                friend = new Connection()
                                {
                                    UserId = invitor.UserId,
                                    FirstName = invitee.FirstName.TitleCase(),
                                    LastName = invitee.LastName.TitleCase(),
                                    EmailAddress = invitee.Username,
                                    IsAccepted = true,
                                    IsConnected = true,
                                    IsBlocked = false,
                                    IsDeleted = false,
                                    Sent = true,
                                    DateSent = DateTime.Now,
                                    IsValid = true,
                                    CreatedBy = User.Username
                                };
                                dataHelper.Add<Connection>(friend, invitor.Username);
                            }

                            invite = context.Invites.SingleOrDefault(x => x.Token.Value.Equals(new Guid(token)));
                            invite.IsConnected = true;
                            dataHelper.Update<Invite>(invite);

                            var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation_accepted.html"));
                            var body = reader.ReadToEnd();

                            var subject = string.Format("{0} has Accepted Connection", !string.IsNullOrEmpty(invitee.Company) ? invitee.Company : string.Format("{0} {1}", invitee.FirstName, invitee.LastName));
                            body = body.Replace("@@receiver", string.Format("{0}", !string.IsNullOrEmpty(invitor.Company) ? invitor.Company : string.Format("{0} {1}", invitor.FirstName, invitor.LastName)));
                            body = body.Replace("@@sender", string.Format("{0} {1}", invitee.FirstName, invitee.LastName));
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, invitee.PermaLink));
                            string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, invitee.UserId);
                            body = body.Replace("@@viewurl", viewurl);

                            string[] receipent = { invitor.Username };

                            AlertService.Instance.SendMail(subject, receipent, body);

                            TempData["UpdateData"] = "Connection invitation accepted!";
                        }
                        else
                        {
                            string fullName = string.Empty;
                            if (invitor.Type == (int)SecurityRoles.Jobseeker)
                            {
                                fullName = string.Format("{0} {1}", invitor.FirstName, invitor.LastName);
                            }
                            else if (invitor.Type == (int)SecurityRoles.Employers)
                            {
                                fullName = invitor.Company;
                            }
                            TempData["Error"] = string.Format("You are already connected with {0}!", fullName);
                        }
                    }
                    else
                    {
                        TempData["Error"] = "You cannot connect with yourself!";
                    }
                }
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult InviteFriendJ(string country, string mobile, string jurl)
        {
            ResponseContext context4 = new ResponseContext();
            string[] m = jurl.ToLower().Split('-');
            long x1= Convert.ToInt32(m.Last());
            Regex reg = null;
            reg = new Regex("^[1-9][0-9]*$");
            string message = string.Empty;
            if (!string.IsNullOrEmpty(mobile) && reg.IsMatch(mobile))
            {
                string AccountSid = ConfigService.Instance.GetConfigValue("TwilioSID");
                string AuthToken = ConfigService.Instance.GetConfigValue("TwilioToken");
                string from = ConfigService.Instance.GetConfigValue("TwilioNumber");
                var twilio = new TwilioRestClient(AccountSid, AuthToken);

                StringBuilder sbSMS = new StringBuilder();
                sbSMS.AppendFormat("{0} invites you to connect at Joblisting and Apply for the Job\n", User.Info.FullName);
                Guid token = Guid.NewGuid();
                var url = string.Format("{0}/connectbyphone?token={1}", Request.Url.GetLeftPart(UriPartial.Authority), token);
                

                string to = string.Format("{0}{1}", country, mobile);
                string url5 = "";
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    string[] jo = jurl.ToLower().Split('-');
                    List<Job> listjob = MemberService.Instance.InviteJob(Convert.ToInt32(jo.Last()));
                    //var job = dataHelper.GetSingle<Job>(Convert.ToInt32(jurl));
                    if (listjob.Count >= 0)
                    {
                        url5 = string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, listjob[0].PermaLink,
                                                  listjob[0].Id);

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
                                    List<MemberService.sentE> list2 = MemberService.Instance.sentSI(User.Id, listjob[0].Id, to);
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
            //return Json(context4, JsonRequestBehavior.AllowGet);
            return RedirectToAction("SMS","job",new { id= x1 });
        }
        [Authorize]
        public ActionResult InviteFriend(string country, string mobile)
        {
            Regex reg = null;
            reg = new Regex("^[1-9][0-9]*$");
            string message = string.Empty;
            if (!string.IsNullOrEmpty(mobile) && reg.IsMatch(mobile))
            {
                string AccountSid = ConfigService.Instance.GetConfigValue("TwilioSID");
                string AuthToken = ConfigService.Instance.GetConfigValue("TwilioToken");
                string from = ConfigService.Instance.GetConfigValue("TwilioNumber");
                var twilio = new TwilioRestClient(AccountSid, AuthToken);

                StringBuilder sbSMS = new StringBuilder();
                sbSMS.AppendFormat("{0} invites you to connect at Joblisting\n", User.Info.FullName);
                Guid token = Guid.NewGuid();
                var url = string.Format("{0}/connectbyphone?token={1}", Request.Url.GetLeftPart(UriPartial.Authority), token);
                sbSMS.AppendFormat("CLICK HERE {0}", url);

                string to = string.Format("{0}{1}", country, mobile);
                using (JobPortalEntities context = new JobPortalEntities())
                {

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
                            }
                            TempData["SaveData"] = "Invitation sent successfully!";
                        }
                    }
                    else
                    {
                        if (sms.RestException.Code.Equals("14101") || sms.RestException.Code.Equals("21211"))
                        {
                            TempData["Error"] = "Please provide valid mobile number!";
                        }
                    }

                }
            }
            else
            {
                TempData["Error"] = "Please provide valid mobile number!";
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> InviteViaEmail(ConnectionModel model)
        {
            ResponseContext context = new ResponseContext();

            UserInfoEntity profile = _service.Get(User.Id);
            if (!string.IsNullOrEmpty(model.EmailAddress))
            {
                UserInfoEntity registered = _service.Get(model.EmailAddress.ToLower());
                if (registered != null)
                {
                    BlockedEntity entity = _service.GetBlockedEntry(registered.Id, User.Id);
                    if (entity != null && !entity.CreatedBy.Equals(User.Username))
                    {
                        context = new ResponseContext()
                        {
                            Id = 0,
                            Type = "Failed",
                            Message = "This profile is in private mode!"
                        };

                        return Json(context, JsonRequestBehavior.AllowGet);
                        //return View("Blocked");
                    }
                }

                if (!profile.Username.Equals(model.EmailAddress.ToLower()))
                {
                    long id = await iNetworkService.Invite(model.EmailAddress, profile.Username);
                    if (id >= 0)
                    {
                        var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation.html"));
                        var body = reader.ReadToEnd();
                        string name = string.Empty;
                        if (registered != null)
                        {
                            name = registered.FirstName + " " + registered.LastName;
                        }
                        else
                        {
                            name = model.EmailAddress;
                        }

                        body = body.Replace("@@receiver", name);
                        if (profile.Type == (int)SecurityRoles.Jobseeker)
                        {
                            body = body.Replace("@@sender", string.Format("{0} {1}", profile.FirstName, profile.LastName));
                        }
                        else
                        {
                            body = body.Replace("@@sender", string.Format("{0}", profile.FullName));
                        }

                        body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, profile.PermaLink));
                        body = body.Replace("@@accepturl", string.Format("{0}://{1}/Network/Accept/{2}", Request.Url.Scheme, Request.Url.Authority, id));
                        body = body.Replace("@@button", "Accept");

                        if (registered != null)
                        {
                            body = body.Replace("@@unsubscribe", "");
                        }
                        else
                        {
                            string ulink = string.Format("<a href=\"{0}://{1}/Network/Unsubscribe?Id={2}\">unsubscribe</a> or ", Request.Url.Scheme, Request.Url.Authority, id);
                            body = body.Replace("@@unsubscribe", ulink);
                        }

                        string[] receipent = { model.EmailAddress };

                        var subject = string.Format("{0} Invites you to connect at Joblisting", string.Format("{0} {1}", profile.FirstName, profile.LastName));
                        AlertService.Instance.SendMail(subject, receipent, body);

                        context = new ResponseContext()
                        {
                            Id = id,
                            Type = "Success",
                            Message = "Invitation sent successfully!"
                        };
                        TempData["SaveData"] = "Invitation sent successfully!";
                    }
                    else if (id < 0)
                    {
                        context = new ResponseContext()
                        {
                            Id = 0,
                            Type = "Failed",
                            Message = string.Format("{0} already exist in the contact list!", model.EmailAddress)
                        };
                        TempData["Error"] = string.Format("{0} already exist in the contact list!", model.EmailAddress);
                    }
                }
                else
                {
                    context = new ResponseContext()
                    {
                        Id = 0,
                        Type = "Failed",
                        Message = "You cannot connect with yourself!"
                    };
                    TempData["Error"] = "You cannot connect with yourself!";
                }
            }
            else
            {
                context = new ResponseContext()
                {
                    Id = 0,
                    Type = "Failed",
                    Message = "Provide Email Address!"
                };
                TempData["Error"] = "Provide Email address!";
            }
            return RedirectToAction("Index");
            //return Json(context, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public async Task<ActionResult> Add(ConnectionModel model)
        {
            UserInfoEntity profile = _service.Get(User.Id);
            if (!string.IsNullOrEmpty(model.EmailAddress))
            {
                UserInfoEntity registered = _service.Get(model.EmailAddress.ToLower());
                if (registered != null)
                {
                    BlockedEntity entity = _service.GetBlockedEntry(registered.Id, User.Id);
                    if (entity != null && !entity.CreatedBy.Equals(User.Username))
                    {
                        return View("Blocked");
                    }
                }

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    if (!profile.Username.Equals(model.EmailAddress.ToLower()))
                    {
                        long id = await iNetworkService.Invite(model.EmailAddress, profile.Username);
                        if (id >= 0)
                        {
                            var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation.html"));
                            var body = reader.ReadToEnd();
                            string name = string.Empty;
                            if (registered != null)
                            {
                                name = registered.FirstName + " " + registered.LastName;
                            }
                            else
                            {
                                name = model.EmailAddress;
                            }

                            body = body.Replace("@@receiver", name);
                            body = body.Replace("@@sender", string.Format("{0}", profile.FullName));
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, profile.PermaLink));
                            body = body.Replace("@@accepturl", string.Format("{0}://{1}/Network/Accept/{2}", Request.Url.Scheme, Request.Url.Authority, id));
                            body = body.Replace("@@button", "Accept");

                            if (registered != null)
                            {
                                body = body.Replace("@@unsubscribe", "");
                            }
                            else
                            {
                                string ulink = string.Format("<a href=\"{0}://{1}/Network/Unsubscribe?Id={2}\">unsubscribe</a> or ", Request.Url.Scheme, Request.Url.Authority, id);
                                body = body.Replace("@@unsubscribe", ulink);
                            }

                            string[] receipent = { model.EmailAddress };

                            var subject = string.Format("{0} Invites you to connect at Joblisting", string.Format("{0}", profile.FullName));
                            AlertService.Instance.SendMail(subject, receipent, body);

                            TempData["SaveData"] = "Invitation sent successfully!";
                        }
                        else
                        {
                            TempData["Error"] = string.Format("{0} already exist in the contact list!", model.EmailAddress);
                        }
                    }
                    else
                    {
                        TempData["Error"] = "You cannot connect with yourself!";
                    }
                    List<Connection> contact_list = dataHelper.GetList<Connection>().Where(x => x.UserId == profile.Id).ToList();
                    ViewBag.Contacts = contact_list;
                }
            }
            else
            {
                TempData["Error"] = "Provide Email address!";
            }
            return RedirectToAction("Index");
        }



        //[Authorize]
        //public ActionResult EmailVerify(long Id, string comv, string indv, string conv, string emailv, string redirect = "")
        //{
        //    string flag = "";
        //    UserInfoEntity uinfo = _service.Get(emailv.ToLower());
        //    if(uinfo == null)
        //    {
        //        return RedirectToAction("Register", "Account");
        //    }
        //    if (uinfo != null && uinfo.IsConfirmed == false)
        //    {
        //        //return RedirectToAction("Login", "Account");
        //        return RedirectToAction("Confirm", "Account", new { id = uinfo.Id });
        //    }
        //    else
        //    {
        //        int i = jobService.InviteViaEmailI(Id, emailv, "", comv, indv.Replace("&", "and"), conv.Trim(), "");
        //        if (i == 1)
        //        {
        //            flag = "Verified  successfully!";
        //        }
        //        return RedirectToAction("Login", "Account", new { id = Id });
        //    }
        //   // return RedirectToAction("Index","Network");



        //}
        //[Authorize]
        //public ActionResult EmailVerifyEd(long Id, string edv, string scv, string ftv,string ttv, string emailv1)
        //{
        //    string flag = "";
        //    UserInfoEntity uinfo = _service.Get(emailv1.ToLower());
        //    if (uinfo == null)
        //    {
        //        return RedirectToAction("Register", "Account");
        //    }
        //    if (uinfo != null && uinfo.IsConfirmed == false)
        //    {
        //        //return RedirectToAction("ConfirmRegistration", "Account");
        //        return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
        //    }
        //    else
        //    {
        //        int i = jobService.InviteViaEmailEdI(Id, edv,scv, ftv, ttv, "", emailv1, "");
        //        if (i == 1)
        //        {
        //            flag = "Verified  successfully!";
        //        }
        //    }
        //    return RedirectToAction("Index", "Inbox");


        //}

        [HttpPost]
        public ActionResult UploadPst(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                List<ImportContact> contacts = new List<ImportContact>();
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                    TextReader textReader = new StreamReader(postedFile.InputStream);
                    CsvReader reader = new CsvReader(textReader);
                    bool errorFlag = false;

                    while (reader.Read())
                    {
                        string[] row = reader.CurrentRecord;
                        if (row.Length > 57)
                        {
                            string firstName = row[1];
                            string lastName = row[3];
                            string email = row[57];
                            ImportContact contact = new ImportContact()
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                Email = email
                            };

                            if (!string.IsNullOrEmpty(email))
                            {
                                contacts.Add(contact);
                            }
                        }
                        else
                        {
                            errorFlag = true;
                            break;
                        }
                    }
                    if (errorFlag)
                    {
                        TempData["Error"] = "Invalid file format. Provide outlook exported CSV File format!";
                    }
                    else
                    {
                        if (contacts.Count > 0)
                        {
                            return ImportAndRedirect(contacts);
                        }
                        else
                        {
                            TempData["Error"] = "Provide outlook exported CSV File format!";
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteContact(long Id)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                Connection contact = dataHelper.GetSingle<Connection>(Id);
                NetworkContactModel model = new NetworkContactModel();
                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                if (contact != null)
                {
                    string email = contact.EmailAddress;
                    dataHelper.Remove<Connection>(contact);

                    UserProfile connected = MemberService.Instance.Get(email);
                    if (connected != null)
                    {
                        Hashtable parameters = new Hashtable();
                        parameters.Add("UserId", connected.UserId);
                        parameters.Add("EmailAddress", profile.Username);
                        Connection connectedUserContact = dataHelper.GetSingle<Connection>(parameters);
                        if (connectedUserContact != null)
                        {
                            dataHelper.Remove<Connection>(connectedUserContact);
                        }
                    }
                }
            }
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [UrlPrivilegeFilter]
        public async Task<ActionResult> Disconnect(long Id, string redirect)
        {
            int status = await iNetworkService.Disconnect(Id, User.Username);
            string msg = string.Empty;
            if (Request.IsAjaxRequest())
            {
                if (status > 0)
                {
                    msg = "Disconnected successfully!";
                }
                else
                {
                    msg = "Unable to disconnect!";
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (status > 0)
                {
                    msg = "Disconnected successfully!";
                }
                else
                {
                    msg = "Unable to disconnect!";
                }
                return Redirect(redirect);
            }
        }
        [HttpGet]
        public ActionResult SearchPeople()
        {
            return PartialView();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Invite(long Id)
        {
            UserInfoEntity uinfo = _service.Get(User.Id);
            if (uinfo != null && uinfo.IsConfirmed == false)
            {
                //return RedirectToAction("ConfirmRegistration", "Account");
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
            }

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                Connection contact = dataHelper.GetSingle<Connection>(Id);
                NetworkContactModel model = new NetworkContactModel();
                if (contact != null)
                {
                    var reader = new StreamReader(Server.MapPath("~/Templates/Mail/reinvite.html"));
                    var body = reader.ReadToEnd();
                    string name = contact.FirstName + " " + contact.LastName;
                    if (string.IsNullOrEmpty(name))
                    {
                        name = contact.EmailAddress;
                    }
                    else if (name.Trim() != " ")
                    {
                        name = contact.EmailAddress;
                    }

                    body = body.Replace("@@receiver", name);
                    body = body.Replace("@@sender", uinfo.FullName);

                    body = body.Replace("@@accepturl", string.Format("{0}://{1}/Network/Accept/{2}", Request.Url.Scheme, Request.Url.Authority, contact.Id));
                    body = body.Replace("@@button", "Accept");
                    UserInfoEntity registered = _service.Get(contact.EmailAddress);
                    if (registered != null)
                    {
                        body = body.Replace("@@unsubscribe", "");
                    }
                    else
                    {
                        string ulink = string.Format("<a href=\"{0}://{1}/Network/Unsubscribe?Id={2}\">unsubscribe</a> or ", Request.Url.Scheme,
                                Request.Url.Authority, contact.Id);

                        body = body.Replace("@@unsubscribe", ulink);
                    }

                    string[] receipent = { contact.EmailAddress };
                    var subject = "Your Friend Re-invites you to Join at Joblisting";
                    AlertService.Instance.SendMail(subject, receipent, body);
                }
            }
            return Json("Re-invitation sent successfully!", JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public ActionResult Accept(long Id, string redirect = "")
        {
            if (User.Identity != null && !string.IsNullOrEmpty(User.Username))
            {
                UserProfile UserInfo = MemberService.Instance.Get(User.Username);
                if (UserInfo != null && UserInfo.IsConfirmed == false)
                {
                    if (TempData["Status"] == null)
                    {
                        //return RedirectToAction("ConfirmRegistration", "Account");
                        return RedirectToAction("Confirm", "Account", new { id = UserInfo.UserId, returnUrl = Request.Url.ToString() });
                    }
                    else
                    {
                        string refurl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : null;
                        if (refurl != null && !refurl.Contains(Request.Url.Authority))
                        {
                            Connection connection = ConnectionHelper.Get(Id);
                            if (connection != null)
                            {
                                UserProfile profile = MemberService.Instance.Get(User.Username);
                                profile.IsConfirmed = true;
                                profile.ConfirmationToken = null;
                                profile.IsValidUsername = true;
                                MemberService.Instance.Update(profile);
                            }
                        }
                        else if (string.IsNullOrEmpty(refurl))
                        {
                            Connection connection = ConnectionHelper.Get(Id);
                            if (connection != null)
                            {
                                UserProfile profile = MemberService.Instance.Get(User.Username);
                                profile.IsConfirmed = true;
                                profile.ConfirmationToken = null;
                                profile.IsValidUsername = true;
                                MemberService.Instance.Update(profile);
                            }
                        }
                    }
                }

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    Connection connection = dataHelper.GetSingle<Connection>(Id);
                    Hashtable parameters = new Hashtable();

                    if (connection != null && connection.IsDeleted == false && !string.IsNullOrEmpty(User.Username))
                    {
                        if (!string.IsNullOrEmpty(connection.CreatedBy) && connection.CreatedBy.Equals(User.Username))
                        {
                            UserProfile profile = MemberService.Instance.Get(connection.EmailAddress);
                            if (profile != null)
                            {
                                TempData["Error"] = string.Format("You cannot accept this connection. {0} {1} has to accept it!", profile.FirstName, profile.LastName);
                            }
                            else
                            {
                                TempData["Error"] = string.Format("You cannot accept this connection. {0} has to accept it!", connection.EmailAddress);
                            }
                        }
                        else
                        {
                            UserProfile loggedUser = MemberService.Instance.Get(User.Username);
                            UserProfile connected = dataHelper.GetSingle<UserProfile>("Username", connection.EmailAddress); // Contact
                            UserProfile requestor = dataHelper.GetSingle<UserProfile>("UserId", connection.UserId);
                            if (requestor != null && connected != null)
                            {
                                parameters = new Hashtable();
                                parameters.Add("UserId", connected.UserId);
                                parameters.Add("EmailAddress", requestor.Username);
                                Connection invitor = dataHelper.GetSingle<Connection>(parameters);
                                if (invitor == null)
                                {
                                    invitor = new Connection()
                                    {
                                        UserId = connected.UserId,
                                        EmailAddress = requestor.Username,
                                        FirstName = requestor.FirstName,
                                        LastName = requestor.LastName,
                                        IsAccepted = true,
                                        IsConnected = true,
                                        IsBlocked = false,
                                        IsDeleted = false,
                                        IsValid = true
                                    };
                                    dataHelper.Add<Connection>(invitor, User.Username);
                                }
                                else
                                {
                                    invitor.IsAccepted = true;
                                    invitor.IsConnected = true;
                                    dataHelper.Update<Connection>(invitor, User.Username);
                                }

                                connection.FirstName = connected.FirstName;
                                connection.LastName = connected.LastName;
                                connection.IsAccepted = true;
                                connection.IsConnected = true;
                                connected.CreatedBy = requestor.Username;
                                dataHelper.Update<Connection>(connection, User.Username);

                                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation_accepted.html"));
                                var body = reader.ReadToEnd();

                                var subject = string.Empty;
                                if (connected.Username.Equals(loggedUser.Username))
                                {
                                    body = body.Replace("@@receiver", string.Format("{0}", !string.IsNullOrEmpty(requestor.Company) ? requestor.Company : string.Format("{0} {1}", requestor.FirstName, requestor.LastName)));
                                }
                                else
                                {
                                    body = body.Replace("@@receiver", string.Format("{0}", !string.IsNullOrEmpty(connected.Company) ? connected.Company : string.Format("{0} {1}", connected.FirstName, connected.LastName)));
                                }
                                subject = string.Format("{0} has Accepted Connection", !string.IsNullOrEmpty(loggedUser.Company) ? loggedUser.Company : string.Format("{0} {1}", loggedUser.FirstName, loggedUser.LastName));


                                if (loggedUser.Type == (int)SecurityRoles.Jobseeker)
                                {
                                    body = body.Replace("@@sender", string.Format("{0} {1}", loggedUser.FirstName, loggedUser.LastName));
                                }
                                else
                                {
                                    body = body.Replace("@@sender", string.Format("{0}", !string.IsNullOrEmpty(loggedUser.Company) ? loggedUser.Company : string.Format("{0} {1}", loggedUser.FirstName, loggedUser.LastName)));
                                }
                                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, loggedUser.PermaLink));
                                string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, loggedUser.UserId);
                                body = body.Replace("@@viewurl", viewurl);

                                string[] receipent = { "" };
                                if (connected.Username.Equals(loggedUser.Username))
                                {
                                    receipent[0] = requestor.Username;
                                }
                                else
                                {
                                    receipent[0] = connected.Username;
                                }

                                AlertService.Instance.SendMail(subject, receipent, body);

                                TempData["UpdateData"] = "Connection invitation accepted!";
                            }
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Connection already rejected!";
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
        public ActionResult Unsubscribe(long Id)
        {
            string email = string.Empty;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Connection entity = dataHelper.Get<Connection>().SingleOrDefault(x => x.Id == Id);
                ViewBag.UserId = Id;
                if (entity != null)
                {
                    email = entity.EmailAddress;
                }
            }
            if (!string.IsNullOrEmpty(email))
            {
                ViewBag.Email = email;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Unsubscribe(long Id, string check = "")
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Connection entity = dataHelper.Get<Connection>().SingleOrDefault(x => x.Id == Id);
                ViewBag.UserId = Id;
                ViewBag.Email = entity.EmailAddress;

                if (entity != null)
                {
                    entity.Unsubscribed = true;
                    dataHelper.Update<Connection>(entity);
                    TempData["UpdateData"] = "Unsubscribed successfully!";
                    TempData["Status"] = "Unsubscribed";
                }
            }
            return RedirectToAction("Unsubscribe", new { Id = Id });
        }

        [Authorize]
        public ActionResult Search(string SearchBy, string Name, int? Status, string Email, int page = 0)
        {
            List<Connection> contact_list = new List<Connection>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);

                var users = dataHelper.Get<UserProfile>();
                var result = dataHelper.Get<Connection>().Where(x => x.UserId == profile.UserId && x.EmailAddress != profile.Username);

                if (Status != null && (Status.Value == 1 || Status.Value == 0))
                {
                    Boolean flag = Convert.ToBoolean(Status);
                    if (flag)
                    {
                        result = result.Where(x => x.IsConnected == true && x.IsAccepted == true && x.IsDeleted == false && x.IsBlocked == false);
                    }
                    else
                    {
                        result = result.Where(x => x.IsConnected == false && x.IsAccepted == false && x.IsDeleted == false && x.IsBlocked == false && x.Initiated == true);
                    }
                }

                if (Status != null && Status.Value == 2)
                {
                    result = result.Where(x => x.IsBlocked == true);
                }

                if (Status != null && (Status.Value == 4 || Status.Value == 5))
                {
                    users = dataHelper.Get<UserProfile>().Where(x => x.IsActive == true && x.IsDeleted == false && x.Type == Status.Value);
                    result = result.Where(x => users.Any(z => x.EmailAddress.Equals(z.Username)));
                }

                if (!string.IsNullOrEmpty(Name))
                {
                    if (Name.Split(' ').Length > 0)
                    {
                        string[] names = Name.ToLower().Split(' ');
                        users = dataHelper.Get<UserProfile>().Where(x => names.Any(z => (x.FirstName + " " + x.LastName).ToLower().Contains(z)));
                        if (users.Count() > 0)
                        {
                            result = result.Where(x => users.Any(z => z.Username.Contains(x.EmailAddress)));
                        }
                        else
                        {
                            result = result.Where(x => names.Any(z => (x.FirstName + " " + x.LastName).ToLower().Contains(z)));
                        }
                    }
                    else
                    {
                        users = dataHelper.Get<UserProfile>().Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(Name.ToLower()));
                        if (users.Count() > 0)
                        {
                            result = result.Where(x => users.Any(z => z.Username.Contains(x.EmailAddress)));
                        }
                        else
                        {
                            result = result.Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(Name.ToLower()));
                        }
                    }
                }

                int rows = result.Count();
                if (rows > 0)
                {
                    contact_list = result.OrderBy(x => x.Id).Skip((page > 0 ? (page - 1) * 10 : page * 10)).Take(10).ToList();
                }

                ViewBag.Contacts = new StaticPagedList<Connection>(contact_list, (page == 0 ? 1 : page), 10, rows);
                ViewBag.Total = rows;
            }

            return View("Index");
        }

        [HttpGet]
        public ActionResult BlockPeoples(string Status, string Type, string Search, int pageNumber = 0)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                int pageSize = 10;
                int rows = 0;
                //int admin = (int)SecurityRoles.Administrator;
                //int super = (int)SecurityRoles.SuperUser;

                Hashtable parameters = new Hashtable();
                //parameters.Add("Type", (int)SecurityRoles.Jobseeker);
                parameters.Add("Type", (int)SecurityRoles.Employers);
                IEnumerable<UserProfile> peoples = dataHelper.GetList<UserProfile>(parameters);

                //IEnumerable<UserProfile> peoples = DataHelper.Instance.GetList<UserProfile>().Where(x=>x.Type != admin && x.Type != super);


                if (!string.IsNullOrEmpty(Status) && Status.Equals("Blocked"))
                {
                    parameters = new Hashtable();
                    parameters.Add("BlockedId", profile.UserId);
                    List<long> blockedList = dataHelper.GetList<BlockedPeople>(parameters).Select(x => x.BlockedId).ToList();
                    peoples = peoples.Where(x => blockedList.Contains(x.UserId));
                }
                else if (!string.IsNullOrEmpty(Status) && Status.Equals("Unblocked"))
                {
                    parameters = new Hashtable();
                    parameters.Add("BlockedId", profile.UserId);
                    List<long> blockedList = dataHelper.GetList<BlockedPeople>(parameters).Select(x => x.BlockedId).ToList();
                    peoples = peoples.Where(x => !blockedList.Contains(x.UserId));
                }


                if (!string.IsNullOrEmpty(Type) && Type.Equals("Company"))
                {
                    int company = (int)SecurityRoles.Employers;
                    peoples = peoples.Where(x => x.Type == company);
                }
                else if (!string.IsNullOrEmpty(Type) && Type.Equals("Individual"))
                {
                    int individual = (int)SecurityRoles.Jobseeker;
                    peoples = peoples.Where(x => x.Type == individual);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    peoples = peoples.Where(x =>
                                    (((x.Company != null) ? x.Company : "") + ((x.FirstName != null) ? x.FirstName : "") +
                                     ((x.LastName != null) ? x.LastName : "")).ToLower().Contains(Search.ToLower()));
                }

                rows = peoples.Count();
                peoples =
                    peoples.OrderBy(x => x.Company != null ? x.Company : x.FirstName + " " + x.LastName).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : (pageNumber * pageSize)))
                        .Take(pageSize);
                ViewBag.Rows = rows;
                ViewBag.Model = new StaticPagedList<UserProfile>(peoples.ToList(), (pageNumber == 0 ? 1 : pageNumber), pageSize,
                    rows);
            }
            return View();
        }

        [HttpPost]
        public ActionResult BlockPeoples(string Status, string Type, string Search)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);

                int pageSize = 10;
                int pageNumber = 0;
                int rows = 0;

                int admin = (int)SecurityRoles.Administrator;
                int super = (int)SecurityRoles.SuperUser;

                IEnumerable<UserProfile> peoples = dataHelper.GetList<UserProfile>().Where(x => x.Type != admin && x.Type != super);

                if (!string.IsNullOrEmpty(Status) && Status.Equals("Blocked"))
                {
                    List<long> blockedList = dataHelper.GetList<BlockedPeople>().Where(x => x.BlockerId == profile.UserId).Select(x => x.BlockedId).ToList();
                    peoples = peoples.Where(x => blockedList.Contains(x.UserId));
                }
                else if (!string.IsNullOrEmpty(Status) && Status.Equals("Unblocked"))
                {
                    List<long> blockedList = dataHelper.GetList<BlockedPeople>().Where(x => x.BlockerId == profile.UserId).Select(x => x.BlockedId).ToList();
                    peoples = peoples.Where(x => !blockedList.Contains(x.UserId));
                }


                if (!string.IsNullOrEmpty(Type) && Type.Equals("Company"))
                {
                    int company = (int)SecurityRoles.Employers;
                    peoples = peoples.Where(x => x.Type == company);
                }
                else if (!string.IsNullOrEmpty(Type) && Type.Equals("Individual"))
                {
                    int individual = (int)SecurityRoles.Jobseeker;
                    peoples = peoples.Where(x => x.Type == individual);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    peoples = peoples.Where(x =>
                                    (((x.Company != null) ? x.Company : "") + ((x.FirstName != null) ? x.FirstName : "") +
                                     ((x.LastName != null) ? x.LastName : "")).ToLower().Contains(Search.ToLower()));
                }

                rows = peoples.Count();
                peoples =
                    peoples.OrderBy(x => (x.Company != null ? x.Company : x.FirstName + " " + x.LastName))
                        .Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : (pageNumber * pageSize)))
                        .Take(pageSize);
                ViewBag.Rows = rows;
                ViewBag.Model = new StaticPagedList<UserProfile>(peoples.ToList(), (pageNumber == 0 ? 1 : pageNumber), pageSize,
                    rows);
            }
            return View();
        }

        [Authorize]
        public ActionResult Block(string email, string redirect)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                UserProfile blocked = dataHelper.GetSingle<UserProfile>("Username", email);

                if (blocked != null)
                {
                    Hashtable parameters = new Hashtable();
                    parameters.Add("UserId", profile.UserId);
                    parameters.Add("EmailAddress", email);
                    Connection connection = dataHelper.GetSingle<Connection>(parameters);

                    parameters = new Hashtable();
                    parameters.Add("UserId", blocked.UserId);
                    parameters.Add("EmailAddress", profile.Username);
                    Connection connected = dataHelper.GetSingle<Connection>(parameters);

                    if (connection != null && connected != null)
                    {
                        connection.IsBlocked = true;
                        dataHelper.Update<Connection>(connection, User.Username);

                        connected.IsBlocked = true;
                        dataHelper.Update<Connection>(connected, User.Username);
                        TempData["UpdateData"] = string.Format("You have successfully blocked {0}!", (!string.IsNullOrEmpty(blocked.Company) ? blocked.Company : string.Format("{0} {1}", blocked.FirstName, blocked.LastName)));
                    }
                }
            }
            return Redirect(redirect);
        }

        [Authorize]
        [HttpGet]
        [UrlPrivilegeFilter]
        public ActionResult BlockedList()
        {
            UserInfoEntity uinfo = _service.Get(User.Id);
            if (!uinfo.IsConfirmed)
            {
                return RedirectToAction("Confirm", "Account", new { id = uinfo.Id, returnUrl = Request.Url.ToString() });
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> BlockedList(int pageNumber = 1)
        {
            List<Blocked> list = await _service.BlockedProfileList(User.Id, pageNumber);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> PeopleYouMayKnow()
        {
            UserProfile UserInfo = MemberService.Instance.Get(User.Username);
            if (UserInfo != null && UserInfo.IsConfirmed == false)
            {
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.UserId, returnUrl = Request.Url.ToString() });
            }

            UserProfile profile = MemberService.Instance.Get(User.Username);
            List<System.Web.UI.WebControls.Parameter> parameters = new List<System.Web.UI.WebControls.Parameter>();
            List<UserMatchEntity> user_list = new List<UserMatchEntity>();
            int pageSize = 9;
            int rows = 0;
            user_list = await _service.PeopleMatchList(User.Id, 1, pageSize);
            if (user_list.Count > 0)
            {
                rows = user_list.First().MaxRows;
            }
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<UserMatchEntity>(user_list, 1, pageSize, rows);
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> PeopleYouMayKnow(int pageNumber = 1)
        {
            List<System.Web.UI.WebControls.Parameter> parameters = new List<System.Web.UI.WebControls.Parameter>();
            List<UserMatchEntity> user_list = new List<UserMatchEntity>();
            int pageSize = 9;
            user_list = await _service.PeopleMatchList(User.Id, pageNumber, pageSize);

            return Json(user_list, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public ActionResult ProfileViewed()
        {
            if (UserInfo != null && UserInfo.IsConfirmed == false)
            {
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> ProfileViewed(int pageNumber = 1)
        {
            List<ViewedEntity> user_list = await _service.ProfileViewedList(User.Id, pageNumber);
            return Json(user_list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ProfileViewedDetails(long Id)
        {
            ViewBag.Id = Id;
            _service.ProfileViewedMarkAsRead(Id, User.Id);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ProfileViewedDetails(long Id, int pageNumber = 1)
        {
            List<ProfileViewEntity> list = await _service.ProfileViewedDetail(Id, User.Id, pageNumber);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AnonymousViews(int pageNumber = 1)
        {
            UserInfoEntity member = _service.Get(User.Username);
            int pageSize = 10;
            int rows = 0;
            List<ProfileViewEntity> user_list = await _service.AnonymousViews(member.Id, pageNumber);
            if (user_list.Count > 0)
            {
                rows = user_list.First().MaxRows;
            }
            ViewBag.Model = new StaticPagedList<ProfileViewEntity>(user_list, pageNumber, pageSize, rows);
            ViewBag.Rows = rows;

            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider">Google, Microsoft, GooglePlus, Facebook, LinkedIn</param>
        /// <param name="code"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Google(string code)
        {
            IContactService service = new GoogleContactService();
            List<ImportContact> contacts = new List<ImportContact>();
            if (string.IsNullOrEmpty(code))
            {
                return Redirect(service.AuthUrl);
            }
            else
            {
                contacts = service.Contacts(code);
                if (contacts.Count > 0)
                {
                    return ImportAndRedirect(contacts);
                }
                else
                {
                    TempData["Error"] = "No Contact(s) found!";
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [Authorize]
        public ActionResult Facebook(string code)
        {
            IContactService service = new FacebookContactService();
            List<ImportContact> contacts = new List<ImportContact>();
            if (string.IsNullOrEmpty(code))
            {
                return Redirect(service.AuthUrl);
            }
            else
            {
                contacts = service.Contacts(code);
                if (contacts.Count > 0)
                {
                    return ImportAndRedirect(contacts);
                }
                else
                {
                    TempData["Error"] = "No Contact(s) found!";
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        private ActionResult ImportAndRedirect(List<ImportContact> contacts)
        {

            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            if (contacts.Count > 0)
            {

                using (SmtpClient oSmtp = new SmtpClient())
                {
                    long id = 0;
                    int stat = 0;
                    foreach (var item in contacts)
                    {
                        id = SharedService.Instance.ImportContact(User.Id, item.FirstName, item.LastName, item.Email);
                        using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation.html")))
                        {
                            string name = string.Format("{0} {1}", item.FirstName, item.LastName);
                            string body = reader.ReadToEnd();

                            if (!string.IsNullOrEmpty(name))
                            {
                                body = body.Replace("@@receiver", name);
                            }
                            else
                            {
                                body = body.Replace("@@receiver", name);
                            }

                            body = body.Replace("@@sender", User.Info.FullName);
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, User.Info.PermaLink));
                            body = body.Replace("@@accepturl", string.Format("{0}://{1}/Network/Accept/{2}", Request.Url.Scheme, Request.Url.Authority, id));
                            body = body.Replace("@@button", "Accept");
                            UserInfoEntity uinfo = MemberService.Instance.GetUserInfo(item.Email);
                            if (uinfo == null)
                            {
                                body = body.Replace("@@unsubscribe", "");
                            }
                            else
                            {
                                string ulink = string.Format("<a href=\"{0}://{1}/Network/Unsubscribe?Id={2}\">unsubscribe</a> or ", Request.Url.Scheme, Request.Url.Authority, id);
                                body = body.Replace("@@unsubscribe", ulink);
                            }
                            string postmail = ConfigurationManager.AppSettings["postmail"];
                            string postpassword = ConfigurationManager.AppSettings["postpassword"];
                            MimeMessage message = new MimeMessage();
                            message.From.Add(new MailboxAddress("Joblisting", from));
                            message.To.Add(new MailboxAddress(name, item.Email));

                            message.Subject = string.Format("{0} Invites you to connect at Joblisting", User.Info.FullName);
                            message.Body = new TextPart("html") { Text = body };
                            try
                            {
                                oSmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                                oSmtp.Connect("smtp.mailgun.org", 587, false);
                                oSmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                                oSmtp.Authenticate(postmail, postpassword);

                                oSmtp.Send(message);
                                oSmtp.Disconnect(true);
                                stat += MemberService.Instance.UpdateFriendRequest(id);
                            }
                            catch (Exception ex)
                            {
                                SendEx(ex);
                            }
                        }

                    }

                    if (stat > 0)
                    {
                        TempData["UpdateData"] = "Contact(s) imported successfully!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = "There are no contact(s) found!";
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                TempData["Error"] = "There are no contact(s) found!";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Faliled");
        }

        public ActionResult Failed()
        {
            TempData["message"] = "Something went wrong!";

            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider">Google, Microsoft, GooglePlus, Facebook, LinkedIn</param>
        /// <param name="code"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Live(string code)
        {
            IContactService service = new MicrosoftContactService();
            List<ImportContact> contacts = new List<ImportContact>();

            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return Redirect(service.AuthUrl);
                }
                else
                {
                    contacts = service.Contacts(code);
                    return ImportAndRedirect(contacts);
                }
            }
            catch (Exception ex)
            {
                SendEx(ex);
            }
            return View();
        }

        [Authorize]
        public ActionResult Twitter(string code)
        {
            IContactService service = new TwitterContactService();
            List<ImportContact> contacts = new List<ImportContact>();
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];

            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return Redirect(service.AuthUrl);
                }
                else
                {
                    contacts = service.Contacts(code);
                    return ImportAndRedirect(contacts);
                }
            }
            catch (Exception ex)
            {
                SendEx(ex);
            }
            return View();
        }

        [Authorize]
        public ActionResult LinkedIn(string code)
        {
            IContactService service = new LinkedinContactService();
            List<ImportContact> contacts = new List<ImportContact>();
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];

            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return Redirect(service.AuthUrl);
                }
                else
                {
                    contacts = service.Contacts(code);
                    return ImportAndRedirect(contacts);
                }
            }
            catch (Exception ex)
            {
                SendEx(ex);
            }
            return View();
        }

        [Authorize]
        public ActionResult Yahoo(string code)
        {
            IContactService service = new YahooContactService();
            List<ImportContact> contacts = new List<ImportContact>();

            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return Redirect(service.AuthUrl);
                }
                else
                {
                    contacts = service.Contacts(code);
                    return ImportAndRedirect(contacts);
                }
            }
            catch (Exception ex)
            {
                SendEx(ex);
            }
            return View();
        }


        private void SendEx(Exception ex)
        {
            string baseUrl = string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority);
            string from = ConfigurationManager.AppSettings["FromEmailAddress"];
            string postmail = ConfigurationManager.AppSettings["postmail"];
            string postpassword = ConfigurationManager.AppSettings["postpassword"];
            MimeMessage msg = new MimeMessage();

            string[] toList = ConfigurationManager.AppSettings["ServiceNotifyEmail"].Split(',');

            string body = string.Format("<h2>{0}</h2>", baseUrl);
            body += string.Format("{0}", ex.ToString());
            msg.From.Add(new MailboxAddress("", from));
            foreach (string email in toList)
            {
                msg.To.Add(new MailboxAddress("", email));
            }

            msg.Subject = "Error occurred while running Joblisting Reminder Service";
            msg.Body = new TextPart("html") { Text = body };

            using (SmtpClient osmtp = new SmtpClient())
            {
                osmtp.Timeout = 100000;
                osmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                osmtp.Connect("smtp.mailgun.org", 587, false);
                osmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                osmtp.Authenticate(postmail, postpassword);

                osmtp.Send(msg);
                osmtp.Disconnect(true);
            }
        }

    }
}