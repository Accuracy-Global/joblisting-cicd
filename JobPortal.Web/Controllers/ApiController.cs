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
using JobPortal.Services;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Twilio;
using JobPortal.Library.Utility;
using WebMatrix.WebData;
using JobPortal.Web.Models;
using System.Data.SqlClient;
using System.Data;

namespace JobPortal.Web.Controllers
{
    public class ApiController : Controller
    {
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
        IUserService iUserService = null;
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'ITrackService' could not be found (are you missing a using directive or an assembly reference?)
        ITrackService iTrackerService = null;
#pragma warning restore CS0246 // The type or namespace name 'ITrackService' could not be found (are you missing a using directive or an assembly reference?)
        List<Tracking> app_list = new List<Tracking>();
        string email = string.Empty;

        public ApiController()
        {
            iUserService = new UserService();
            iTrackerService = new TrackService(iUserService);
        }
        public void SendSMS()
        {
            List<PhoneContactReminder> contacts = iUserService.GetPhoneContacts();
            foreach (PhoneContactReminder item in contacts)
            {
                SendMessage(item.Sender, item.Id, item.Phone);
            }
        }
        public void SendMessage(string sender, long id, string mobile)
        {
            //if (!string.IsNullOrEmpty(mobile) && reg.IsMatch(mobile))
            //{
            string AccountSid = ConfigService.Instance.GetConfigValue("TwilioSID");
            string AuthToken = ConfigService.Instance.GetConfigValue("TwilioToken");
            string from = ConfigService.Instance.GetConfigValue("TwilioNumber");
            string mobile_app_url = ConfigService.Instance.GetConfigValue("mobile_app_url");
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            StringBuilder sbSMS = new StringBuilder();

            sbSMS.AppendFormat("This is {0}.\n", sender);
            sbSMS.Append("I am using joblisting.com messenger!\n");
            var url = string.Format("{0}", mobile_app_url);
            sbSMS.AppendFormat("Download here {0} and connect!", url);

            var sms = twilio.SendMessage(from, mobile, sbSMS.ToString());
            if (sms.RestException == null)
            {
                var msg = twilio.GetMessage(sms.Sid);
                if (msg.Status.Equals("delivered") || msg.Status.Equals("sent"))
                {
                    int status = iUserService.RecordSMSStatus(id, msg.Status);
                }
                else
                {
                    iUserService.RecordSMSStatus(id, msg.Status);
                }
            }
            else
            {
                if (sms.RestException.Code.Equals("14101") || sms.RestException.Code.Equals("21211"))
                {
                    iUserService.RecordSMSStatus(id, sms.RestException.Message);
                }
                else
                {
                    iUserService.RecordSMSStatus(id, "Failed");
                }
            }
            //}
        }

        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'DeviceEntity' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Authenticate(DeviceEntity model)
#pragma warning restore CS0246 // The type or namespace name 'DeviceEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            ResponseEntity result = new ResponseEntity();

            if (string.IsNullOrEmpty(model.DeviceId))
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Device Id is required!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(model.Username))
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "You can not leave username empty!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "You can not leave password empty!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            bool flag = WebSecurity.Login(model.Username, model.Password, false);
            if (flag)
            {
                UserInfoEntity uinfo = iUserService.Get(model.Username);

                result = new ResponseEntity()
                {
                    Type = "Success",
                    Message = "",
                    Data = new LoginEntity()
                    {
                        Id = uinfo.Id,
                        Type = uinfo.Type,
                        Username = uinfo.Username,
                        Token = iUserService.GenerateTokenMobile(model.DeviceId, model.Username, UIHelper.GetToken()),
                        IsConfirmed = uinfo.IsConfirmed
                    }
                };
            }
            else
            {
                UserInfoEntity uinfo = iUserService.Get(model.Username);
                if (uinfo == null)
                {
                    result = new ResponseEntity()
                    {
                        Type = "Error",
                        Message = "User account does not exist!"
                    };
                }
                else
                {
                    result = new ResponseEntity()
                    {
                        Type = "Error",
                        Message = "Invalid username or password!"
                    };
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'DeviceRegister' could not be found (are you missing a using directive or an assembly reference?)
        public JsonResult Avtar1(DeviceRegister model)
#pragma warning restore CS0246 // The type or namespace name 'DeviceRegister' could not be found (are you missing a using directive or an assembly reference?)
        {
            ResponseEntity result = new ResponseEntity();
            if (string.IsNullOrEmpty(model.Username))
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Username is required!"
                };
            }
            else
            {
                UserInfoEntity uinfo = iUserService.Get(model.Username);
                byte[] buffer = MemberService.Instance.GetProfilePhoto(uinfo.Id);
                string content = Convert.ToBase64String(buffer);

                result = new ResponseEntity()
                {
                    Type = "Success",
                    Message = "",
                    Data = content
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'DeviceRegister' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Register(DeviceRegister model)
#pragma warning restore CS0246 // The type or namespace name 'DeviceRegister' could not be found (are you missing a using directive or an assembly reference?)
        {
            ResponseEntity result = new ResponseEntity();

            if (string.IsNullOrEmpty(model.DeviceId))
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Device Id is required!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(model.Username))
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "You can not leave username empty!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "You can not leave password empty!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(model.FirstName))
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "You can not leave first name empty!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(model.LastName))
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "You can not leave last name empty!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (Convert.ToInt32(model.Type) == (int)SecurityRoles.Jobseeker)
            {
                
                if (string.IsNullOrEmpty(model.Title))
                {
                    result = new ResponseEntity()
                    {
                        Type = "Error",
                        Message = "You can not leave Current Role as empty!"
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            else if (Convert.ToInt32(model.Type) == (int)SecurityRoles.Employers)
            {
                if (string.IsNullOrEmpty(model.Company))
                {
                    result = new ResponseEntity()
                    {
                        Type = "Error",
                        Message = "You can not leave company name empty!"
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }

            WebSecurity.CreateUserAndAccount(model.Username.Trim().ToLower(), model.Password.Trim());
            RegisterEntity entity = new RegisterEntity();
            int age = DateTime.Now.Year - Convert.ToInt32(model.BirthYear);
            if (Convert.ToInt32(model.Type) == (int)SecurityRoles.Jobseeker)
            {
                entity = new RegisterEntity()
                {
                    Username = model.Username.Trim().ToLower(),
                    Type = Convert.ToInt32(model.Type),
                    FirstName = model.FirstName.ToString().Trim().TitleCase(),
                    Title = model.Title.ToString().Trim().TitleCase(),
                    LastName = model.LastName.ToString().Trim().TitleCase(),
                    
                    CountryId = model.CountryId,
                    ConfirmationToken = UIHelper.Get6DigitCode()
                };
            }
            else if (Convert.ToInt32(model.Type) == (int)SecurityRoles.Employers)
            {
                entity = new RegisterEntity()
                {
                    Username = model.Username.Trim().ToLower(),
                    Type = Convert.ToInt32(model.Type),
                    FirstName = model.FirstName.ToString().Trim().TitleCase(),
                    LastName = model.LastName.ToString().Trim().TitleCase(),
                    Company = (!string.IsNullOrEmpty(model.Company) ? model.Company.ToString().Trim().TitleCase() : null),
                    CountryId = model.CountryId,
                    ConfirmationToken = UIHelper.Get6DigitCode()
                };
            }
            UserInfoEntity uinfo = MemberService.Instance.Register(entity);

            Roles.AddUserToRoles(model.Username, new[] { ((SecurityRoles)Convert.ToInt32(model.Type)).ToString() });

            bool flag = WebSecurity.Login(model.Username, model.Password, false);
            if (flag)
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(model.Password);
                DomainService.Instance.ManageLoginHistory(entity.Username, Request.UserHostAddress, Request.Browser.Browser, Convert.ToBase64String(bytes));

                if (!string.IsNullOrEmpty(entity.ConfirmationToken))
                {
                    var reader = new StreamReader(Server.MapPath("~/Templates/Mail/registration.html"));
                    var body = reader.ReadToEnd();
                    body = body.Replace("@@code", entity.ConfirmationToken);

                    var hosturl = Request.Url.GetLeftPart(UriPartial.Authority) +
                        string.Format("/Confirm?id={0}&token={1}", uinfo.Id, entity.ConfirmationToken);

                    body = body.Replace("@@url", hosturl);
                    if (Convert.ToInt32(model.Type) == (int)SecurityRoles.Jobseeker)
                    {
                        string tiplink = string.Format("You may also check <a href=\"{0}\">Career Tips</a>!<br />", string.Format("{0}/jobseeker/careertips", Request.Url.GetLeftPart(UriPartial.Authority)));
                        body = body.Replace("@@tiplink", tiplink);
                    }
                    else
                    {
                        body = body.Replace("@@tiplink", "");
                    }
                    string[] receipent = { model.Username.Trim().ToLower() };
                    var subject = "Confirm Your Email Address";

                    AlertService.Instance.SendMail(subject, receipent, body);
                }
                iUserService.ManageTransaction(uinfo.Id, 7, Guid.NewGuid().ToString(), "NONE");

                result = new ResponseEntity()
                {
                    Type = "Success",
                    Message = "",
                    Data = new LoginEntity()
                    {
                        Id = uinfo.Id,
                        Username = uinfo.Username,
                        Token = iUserService.GenerateTokenMobile(model.DeviceId, model.Username, UIHelper.GetToken()),
                        IsConfirmed = uinfo.IsConfirmed
                    }
                };

                //result = new ResponseEntity()
                //{
                //    Type = "Success",
                //    Message =  iUserService.GenerateTokenMobile(model.DeviceId, model.Username, UIHelper.GetToken())
                //};
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Download(long id, long jobId, string username)
        {
            ResponseEntity result = new ResponseEntity();
            List<int> typeList = new List<int>() { (int)SecurityRoles.SuperUser, (int)SecurityRoles.Administrator };
            UserInfoEntity UserInfo = iUserService.Get(username);

            if ((UserInfo != null && !UserInfo.IsConfirmed) && !typeList.Contains(UserInfo.Type))
            {
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
                //return RedirectToAction("ConfirmRegistration", "Account");
            }

            if (UserInfo.Type == 5)
            {
                if (DomainService.Instance.HasDownloadQuota(UserInfo.Id) == false)
                {
                    if (!DomainService.Instance.IsPaidResume(UserInfo.Id, id))
                    {
                        result = new ResponseEntity()
                        {
                            Type = "Error",
                            Message = "MakePayment"
                        };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            Resume resume = new Resume();
            UserProfile profile = MemberService.Instance.Get(username);
            if (profile != null && profile.Type == (int)SecurityRoles.Employers)
            {
                UserProfile jobSeeker = new UserProfile();
                string message = string.Empty;

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
                        TrackingService.Instance.Downloaded(id, username, out message);
                        if (UserInfo.Type == 5)
                        {
                            iUserService.ManageAccount(UserInfo.Id, null, null, null, 1, null, jobSeeker.UserId, jobId);
                        }
                        //return File(Convert.FromBase64String(content), MediaTypeNames.Application.Octet, jobSeeker.FileName);
                        result = new ResponseEntity()
                        {
                            Type = "Success",
                            Message = jobSeeker.FileName,
                            Data = content
                        };
                        return Json(result, JsonRequestBehavior.AllowGet);
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
                        //return File(Convert.FromBase64String(content), MediaTypeNames.Application.Octet, jobSeeker.FileName);
                        result = new ResponseEntity()
                        {
                            Type = "Success",
                            Message = jobSeeker.FileName,
                            Data = content
                        };
                        return Json(result, JsonRequestBehavior.AllowGet);
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
                        //return File(Convert.FromBase64String(content), MediaTypeNames.Application.Octet, jobSeeker.FileName);
                        result = new ResponseEntity()
                        {
                            Type = "Success",
                            Message = jobSeeker.FileName,
                            Data = content
                        };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = new ResponseEntity()
                    {
                        Type = "Error",
                        Message = string.Format("Only Company account can download resume!")
                    };
                }
            }
            else
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = string.Format("Only Company account can download resume!")
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Bookmark(long Id, string username)
        {
            var message = string.Empty;
            ResponseEntity result = new ResponseEntity();

            UserProfile profile = MemberService.Instance.Get(username);
            var record = TrackingService.Instance.Bookmark(Id, BookmarkedTypes.JOBSEEKER, username, out message);
            TempData["UpdateData"] = message;

            if (!string.IsNullOrEmpty(message))
            {
                result = new ResponseEntity()
                {
                    Type = "Success",
                    Message = message
                };
            }
            else
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Unable to bookmark!"
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Withdraw(Guid id, string username)
        {
            var message = string.Empty;
            ResponseEntity result = new ResponseEntity();
            JobSeekerService.Instance.Withdraw(id, username, out message);
            TempData["UpdateData"] = message;
            if (!string.IsNullOrEmpty(message))
            {
                result = new ResponseEntity()
                {
                    Type = "Success",
                    Message = message
                };
            }
            else
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Unable to withdraw application!"
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Reject(Guid id, string username)
        {
            var message = string.Empty;
            ResponseEntity result = new ResponseEntity();
            EmployerService.Instance.Reject(id, username, out message);
            TempData["UpdateData"] = message;

            if (!string.IsNullOrEmpty(message))
            {
                result = new ResponseEntity()
                {
                    Type = "Success",
                    Message = message
                };
            }
            else
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Unable to reject application!"
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
        public async Task<ActionResult> Contacts(JobPortal.Models.BaseModel model)
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
        {
            ResponseEntity result = null;
            List<ContactEntity> list = new List<ContactEntity>();
            long userId = iUserService.IsActiveSession(model.DeviceId, model.Token);
            if (userId > 0)
            {
                list = await iUserService.GetContactListAsync(userId, model.PageNumber, model.PageSize);
                result = new ResponseEntity()
                {
                    Type = "Success",
                    Message = "",
                    Data = list
                };
            }
            else
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Invalid credentials or Session expired!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'PeopleSearchModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Search(PeopleSearchModel model)
#pragma warning restore CS0246 // The type or namespace name 'PeopleSearchModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<ContactEntity> list = new List<ContactEntity>();
            ResponseEntity result = null;
            if (string.IsNullOrEmpty(model.DeviceId))
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Device Id is required!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            long userId = iUserService.IsActiveSession(model.DeviceId, model.Token);
            if (userId > 0)
            {
                model.UserId = userId;
                if (model.CountryId == 0)
                {
                    model.CountryId = null;
                }
                list = iUserService.Search(model);
                result = new ResponseEntity()
                {
                    Type = "Success",
                    Message = "",
                    Data = list
                };
            }
            else
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Invalid credentials or Session expired!"
                };
            }
            JsonResult jr = new JsonResult();
            jr.Data = result;
            jr.MaxJsonLength = int.MaxValue;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jr;
        }
        [HttpPost]
        public ActionResult SingleConnection(long loginUserId, long userId)
        {
            ContactEntity single = new ContactEntity();
            ResponseEntity result = null;
            if (userId > 0)
            {

                single = iUserService.SingleConnection(loginUserId, userId);
                result = new ResponseEntity()
                {
                    Type = "Success",
                    Message = "",
                    Data = single
                };
            }
            else
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Invalid credentials or Session expired!"
                };
            }
            JsonResult jr = new JsonResult();
            jr.Data = result;
            jr.MaxJsonLength = int.MaxValue;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jr;
        }

        [HttpPost]
        public ActionResult GetAppCounts(string deviceId, string token)
        {
            long userId = iUserService.IsActiveSession(deviceId, token);
            if (userId > 0)
            {
                int counts = EmployerService.Instance.GetApplicationCount(userId);
                return Json(userId, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult IsVerified(string email)
        {
            bool flag = iUserService.IsConfirmed(email);

            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult IsActiveSession(string deviceId, string token)
        {
            long userId = iUserService.IsActiveSession(deviceId, token);
            UserInfoEntity uinfo = iUserService.Get(userId);
            if (uinfo != null)
            {
                return Json(userId, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult JobMatchList(string deviceId, string token, int pageNumber, int pageSize)
        {
            ResponseEntity result = null;
            long userId = iUserService.IsActiveSession(deviceId, token);
            List<MatchEntity> list = new List<MatchEntity>();
            if (userId > 0)
            {
                list = iUserService.JobMatchList(userId, pageNumber, pageSize);
                if (list.Count > 0)
                {
                    result = new ResponseEntity()
                    {
                        Type = "Success",
                        Message = "Able to fetch data!",
                        Data = list
                    };
                }
                else
                {
                    result = new ResponseEntity()
                    {
                        Type = "Error",
                        Message = "No data found!"
                    };
                }
            }
            else
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Unable to retrieve data!"
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult verify(string email, string token)
        {
            int flag = iUserService.Confirm(email, token);

            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'MessageEntity' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Send(MessageEntity model)
#pragma warning restore CS0246 // The type or namespace name 'MessageEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            ResponseEntity result = null;

            if (string.IsNullOrEmpty(model.DeviceId))
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Device Id is required!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            long userId = iUserService.IsActiveSession(model.DeviceId, model.Token);
            if (userId <= 0)
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Invalid credentials or Session expired!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var sender = iUserService.Get(model.From);
                var receiver = iUserService.Get(model.To);

                if (DomainService.Instance.HasMessageQuota(sender.Id) == false)
                {
                    if (MessageService.Instance.Count(receiver.Id, sender.Id) == 0)
                    {
                        result = new ResponseEntity()
                        {
                            Type = "Error",
                            Message = "MakePayment"
                        };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }

                bool registered = false;
                bool connected = false;
                bool blocked = false;
                string[] receipent = new string[1];
                string subject;

                if (sender == null && receiver == null)
                {
                    result = new ResponseEntity()
                    {
                        Type = "Error",
                        Message = "Message sending failed, unable to retrieve recipient details!"
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                string receiverName = receiver.FullName;
                string senderName = sender.FullName;

                registered = (receiver != null);
                connected = ConnectionHelper.IsConnected(receiver.Username, sender.Username);
                blocked = iUserService.IsBlocked(sender.Id, receiver.Id);

                Communication entity = new Communication();
                string msg = "";
                if (!string.IsNullOrEmpty(model.Message))
                {
                    msg = model.Message.RemoveEmails();
                    msg = msg.RemoveNumbers();
                    msg = msg.RemoveWebsites();
                }

                if (registered == true && connected == true && blocked == true)
                {
                    bool blockedByMe = iUserService.IsBlockedByMe(receiver.Id, sender.Id);
                    if (blockedByMe == false)
                    {
                        result = new ResponseEntity()
                        {
                            Type = "Error",
                            Message = string.Format("{0} do not want to receive messages from you!", receiver.FullName)
                        };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }

                if (registered == true && connected == false && blocked == true)
                {
                    bool blockedByMe = iUserService.IsBlockedByMe(receiver.Id, sender.Id);
                    if (blockedByMe == false)
                    {
                        result = new ResponseEntity()
                        {
                            Type = "Error",
                            Message = string.Format("{0} do not want to receive messages from you!", receiver.FullName)
                        };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    DomainService.Instance.Unblock(receiver.Id, sender.Id);

                    StatusEntity status = iUserService.ManageConnection(sender.Id, model.To);
                    if (status.Initiated)
                    {
                        MessageService.Instance.Send(msg, sender.Id, receiver.Id, sender.Username, true);
                        iUserService.ManageAccount(sender.Id, null, 1);

                        using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitationmessage.html")))
                        {
                            string body = reader.ReadToEnd();
                            body = body.Replace("@@sender", string.Format("{0}", senderName));
                            string url = string.Format("{0}://{1}/Message/Index", Request.Url.Scheme, Request.Url.Authority);
                            string viewurl = string.Format("{0}://{1}/Message/Accept?ConnectionId={2}", Request.Url.Scheme, Request.Url.Authority, status.Id);
                            body = body.Replace("@@viewurl", viewurl);
                            body = body.Replace("@@message", msg);
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));

                            body = body.Replace("@@receiver", receiverName);
                            subject = string.Format("Message from {0}", senderName);

                            receipent[0] = receiver.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                        result = new ResponseEntity()
                        {
                            Type = "Success",
                            Message = "Message sent successfully!"
                        };
                    }
                    else if (status.Accepted)
                    {
                        MessageService.Instance.Send(msg, sender.Id, receiver.Id, sender.Username, true);
                        iUserService.ManageAccount(sender.Id, null, 1);

                        using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/message.html")))
                        {
                            string body = reader.ReadToEnd();
                            body = body.Replace("@@sender", senderName);
                            body = body.Replace("@@message", msg);
                            string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, sender.Id);
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
                            string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, sender.Id);
                            body = body.Replace("@@viewurl", viewurl);

                            receipent[0] = receiver.Username;
                            subject = string.Format("{0} has Accepted Connection", senderName);
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }

                        result = new ResponseEntity()
                        {
                            Type = "Success",
                            Message = "Message sent successfully!"
                        };
                    }
                }
                else if (registered == true && connected == true && blocked == false)
                {
                    if (MessageService.Instance.Count(sender.Id, receiver.Id) == 0)
                    {
                        iUserService.ManageAccount(sender.Id, null, 1);
                    }

                    MessageService.Instance.Send(msg, sender.Id, receiver.Id, sender.Username);
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/message.html")))
                    {
                        string body = reader.ReadToEnd();
                        body = body.Replace("@@sender", senderName);
                        body = body.Replace("@@message", msg);
                        string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, sender.Id);
                        body = body.Replace("@@viewurl", viewurl);
                        body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));
                        body = body.Replace("@@navigateurl", string.Format("{0}://{1}/Message", Request.Url.Scheme, Request.Url.Authority));

                        body = body.Replace("@@receiver", receiverName);
                        subject = string.Format("Message from {0}", senderName);

                        receipent[0] = receiver.Username;
                        AlertService.Instance.SendMail(subject, receipent, body);
                    }

                    result = new ResponseEntity()
                    {
                        Type = "Success",
                        Message = "Message sent successfully!"
                    };
                }
                else if (registered == true && connected == false && blocked == false)
                {
                    StatusEntity status = iUserService.ManageConnection(sender.Id, model.To);

                    if (status.Initiated)
                    {
                        MessageService.Instance.Send(msg, sender.Id, receiver.Id, sender.Username, true);
                        iUserService.ManageAccount(sender.Id, null, 1);

                        using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitationmessage.html")))
                        {
                            string body = reader.ReadToEnd();
                            body = body.Replace("@@sender", string.Format("{0}", senderName));
                            string url = string.Format("{0}://{1}/Message/Index", Request.Url.Scheme, Request.Url.Authority);
                            string viewurl = string.Format("{0}://{1}/Message/Accept?ConnectionId={2}", Request.Url.Scheme, Request.Url.Authority, status.Id);
                            body = body.Replace("@@viewurl", viewurl);
                            body = body.Replace("@@message", msg);

                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));

                            body = body.Replace("@@receiver", receiverName);
                            subject = string.Format("Message from {0}", senderName);

                            receipent[0] = receiver.Username;
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                        result = new ResponseEntity()
                        {
                            Type = "Success",
                            Message = "Message sent and connection is initiated!\nWaiting for acceptance."
                        };
                    }
                    else if (status.Accepted)
                    {
                        MessageService.Instance.Send(msg, sender.Id, receiver.Id, sender.Username, true);
                        iUserService.ManageAccount(sender.Id, null, 1);

                        using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/message.html")))
                        {
                            string body = reader.ReadToEnd();
                            body = body.Replace("@@sender", senderName);
                            body = body.Replace("@@message", msg);
                            string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, sender.Id);
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
                            string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, sender.Id);
                            body = body.Replace("@@viewurl", viewurl);

                            receipent[0] = receiver.Username;
                            subject = string.Format("{0} has Accepted Connection", senderName);
                            AlertService.Instance.SendMail(subject, receipent, body);
                        }

                        result = new ResponseEntity()
                        {
                            Type = "Success",
                            Message = "Message sent successfully!"
                        };
                    }
                    else
                    {

                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Pacakage(string deviceId, string token, string type)
        {
            ResponseEntity result = null;
            long userId = iUserService.IsActiveSession(deviceId, token);
            if (userId > 0)
            {
                UserInfoEntity uinfo = iUserService.Get(userId);
                if (type.Equals("M"))
                {
                    BuyMessage pkg = iUserService.MessagePrice(uinfo.CountryId.Value, uinfo.Type);
                    if (pkg != null)
                    {
                        result = new ResponseEntity()
                        {
                            Type = "Success",
                            Message = "Packages found",
                            Data = pkg
                        };
                    }
                    else
                    {
                        result = new ResponseEntity()
                        {
                            Type = "Error",
                            Message = "No packages found!"
                        };
                    }
                }
                else
                {
                    BuyResume pkg = iUserService.ResumeDonwloadPrice(uinfo.CountryId.Value);
                    if (pkg != null)
                    {
                        result = new ResponseEntity()
                        {
                            Type = "Success",
                            Message = "Packages found",
                            Data = pkg
                        };
                    }
                    else
                    {
                        result = new ResponseEntity()
                        {
                            Type = "Error",
                            Message = "No packages found!"
                        };
                    }
                }
            }
            else
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Invalid credentials!",
                    Data = 0
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'PaymentInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult MakePayment(PaymentInfoEntity model)
#pragma warning restore CS0246 // The type or namespace name 'PaymentInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            ResponseEntity outcome = null;
            PaymentResponse response = null;
            Package package = DomainService.Instance.GetPackage(model.PackageId);
            UserInfoEntity uinfo = iUserService.Get(model.Username);

            string payment_gateway = ConfigService.Instance.GetConfigValue("PaymentGateway");
            try
            {
                if (payment_gateway.Equals("PayPal"))
                {
                   // string[] names = model.HolderName.Split(' ');
                    CreditCard card = new CreditCard()
                    {
                       // FirstName = names[0],
                        //LastName = names[names.Length - 1],
                        Number = model.CardNumber,
                        ExpiryMonth = model.ExpiryMonth,
                        ExpiryYear = model.ExpiryYear,
                        SecurityCode = model.SecurityCode,
                        Amount = model.Amount.ToString(),
                        //Email = User.Username
                    };

                    //response = service.CardProcess(card, PaymentId);
                    // var gateway = config.GetGateway();
                }


                if (response != null && !string.IsNullOrEmpty(response.Status) && response.Status.Equals("approved"))
                {
                    string fullName = model.Name;
                    CreditEntry entry = new CreditEntry()
                    {
                        UserId = uinfo.Id,
                        PackageId = model.PackageId,
                        Description = model.Description,
                        BillingZip = model.BillingZip,
                        Amount = model.Amount,
                        TransactionId = response.Id,
                        Method = response.Method
                    };

                    if (model.Type == "M")
                    {
                        entry.Profiles = null;
                        entry.Messages = model.Qty;
                        entry.Interviews = null;
                        entry.Resumes = null;
                    }
                    else
                    {
                        entry.Profiles = null;
                        entry.Messages = null;
                        entry.Interviews = null;
                        entry.Resumes = model.Qty;
                    }

                    long invoice_id = iUserService.ManageTransaction(entry);
                    using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/payment_success.html")))
                    {
                        string body = reader.ReadToEnd();
                        body = body.Replace("@@receiver", fullName);
                        string durl = string.Format("{0}://{1}/billing?userId={2}", Request.Url.Scheme, Request.Url.Authority, uinfo.Id);
                        body = body.Replace("@@durl", durl);
                        string[] receipent = { model.Username };

                        AlertService.Instance.SendMail("Payment Received", receipent, body);
                    }
                    outcome = new ResponseEntity()
                    {
                        Type = "Success",
                        Message = "Payment successful!"
                    };
                }
                else
                {
                    outcome = new ResponseEntity()
                    {
                        Type = "Error",
                        Message = "Payment unsuccessful.\n" + response.Message
                    };
                }
            }
            catch (Exception ex)
            {
                outcome = new ResponseEntity()
                {
                    Type = "Error",
                    Message = ex.Message
                };
            }
            return Json(outcome, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'PaymentModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Payment(PaymentModel model)
#pragma warning restore CS0246 // The type or namespace name 'PaymentModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            ResponseEntity outcome = null;
            Package package = DomainService.Instance.GetPackage(model.PackageId);
            UserInfoEntity uinfo = iUserService.Get(model.Username);

            try
            {
                string fullName = uinfo.FullName;
                long invoice_id = DomainService.Instance.ManageTransaction(uinfo.Id, model.PackageId, model.Amount, model.Description, model.PayId, "Pay Pal");

                using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/payment_success.html")))
                {
                    string body = reader.ReadToEnd();
                    body = body.Replace("@@receiver", fullName);
                    string durl = string.Format("{0}://{1}/billing?userId={2}", Request.Url.Scheme, Request.Url.Authority, uinfo.Id);
                    body = body.Replace("@@durl", durl);
                    string[] receipent = { model.Username };

                    AlertService.Instance.SendMail("Payment Received", receipent, body);
                }
                outcome = new ResponseEntity()
                {
                    Type = "Success",
                    Message = "Payment successful!"
                };
            }
            catch (Exception ex)
            {
                outcome = new ResponseEntity()
                {
                    Type = "Error",
                    Message = ex.Message
                };
            }
            return Json(outcome, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MessageList(string from, string to)
        {
            ResponseEntity result = null;
            int status = iUserService.ReadMark(from, to);
            if (string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Sender and Receiver can not be empty!"
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }

            List<MessageModel> list = iUserService.GetMessageList(from, to);

            result = new ResponseEntity()
            {
                Type = "Success",
                Message = "Message found!",
                Data = list
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Recent(string from, string to)
        {
            ResponseEntity result = null;
            if (string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Sender and Receiver can not be empty!"
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }

            MessageModel model = iUserService.GetRecentMessage(from, to);

            result = new ResponseEntity()
            {
                Type = "Success",
                Message = "Message found!",
                Data = model
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'PhoneContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult SyncContact(PhoneContactEntity model)
#pragma warning restore CS0246 // The type or namespace name 'PhoneContactEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            ResponseEntity result = null;
            string phone = model.Phone;

            if (!phone.StartsWith("+"))
            {
                if (phone.StartsWith("0"))
                {
                    phone = Convert.ToString(Convert.ToInt64(phone));
                    if (phone.Length <= 10)
                    {
                        JobPortal.Data.List country = SharedService.Instance.GetCountryByShortForm(model.CountryCode.ToUpper());
                        if (country != null)
                        {
                            if (!phone.Contains(country.Code))
                            {
                                phone = string.Format("+{0}{1}", country.Code, phone);
                            }
                            else
                            {
                                phone = string.Format("+{0}", phone);
                            }
                        }
                    }
                    else if (phone.Length > 10)
                    {
                        phone = string.Format("+{0}", phone);
                    }
                }
                else
                {
                    if (phone.Length <= 10)
                    {
                        JobPortal.Data.List country = SharedService.Instance.GetCountryByShortForm(model.CountryCode.ToUpper());
                        if (country != null)
                        {
                            if (!phone.Contains(country.Code))
                            {
                                phone = string.Format("+{0}{1}", country.Code, phone);
                            }
                            else
                            {
                                phone = string.Format("+{0}", phone);
                            }
                        }
                    }
                    else if (phone.Length > 10)
                    {
                        phone = string.Format("+{0}", phone);
                    }
                }
            }
            model.Phone = phone;
            long flag = iUserService.ManagePhontContact(model);
            if (flag > 0)
            {
                SendMessage(model.DeviceId, phone, flag);
                result = new ResponseEntity()
                {
                    Type = "Success",
                    Message = "Contact synced successfully"
                };
            }
            else
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Unable to sync contact!"
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Connect(string from, string to)
        {
            ResponseEntity result = null;
            UserInfoEntity fromUser = iUserService.Get(from);
            UserInfoEntity toUser = iUserService.Get(to);

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                List<Connection> contact_list = new List<Connection>();
                bool byLoggedInUser = false;
                bool byEmailUser = false;

                if (fromUser != null && toUser != null)
                {
                    byEmailUser = ConnectionHelper.IsBlocked(fromUser.Username, toUser.Id);
                    byLoggedInUser = ConnectionHelper.IsBlockedByMe(to.Trim().ToLower(), fromUser.Id);
                }
                if (byEmailUser == false && byLoggedInUser == false)
                {
                    if (!fromUser.Username.ToLower().Equals(to.ToLower()))
                    {
                        Hashtable parameters = new Hashtable();
                        parameters.Add("UserId", fromUser.Id);
                        parameters.Add("EmailAddress", to.Trim().ToLower());
                        Connection contact = dataHelper.GetSingle<Connection>(parameters);
                        if (contact == null)
                        {
                            string[] names = { null, null };
                            if (toUser != null)
                            {
                                names = toUser.FullName.Split(' ');
                            }

                            contact = new Connection()
                            {
                                UserId = fromUser.Id,
                                FirstName = names[0],
                                LastName = names[1],
                                EmailAddress = to.Trim().ToLower(),
                                IsAccepted = false,
                                IsConnected = false,
                                IsBlocked = false,
                                Initiated = true,
                                IsDeleted = false,
                                Sent = true,
                                DateSent = DateTime.Now,
                                IsValid = true
                            };

                            dataHelper.Add<Connection>(contact, fromUser.Username);

                            if (toUser != null)
                            {
                                Connection networkContact = dataHelper.Get<Connection>().SingleOrDefault(x => x.UserId == toUser.Id && x.EmailAddress == fromUser.Username);
                                if (networkContact == null)
                                {
                                    names = fromUser.FullName.Split(' ');
                                    networkContact = new Connection()
                                    {
                                        UserId = toUser.Id,
                                        FirstName = names != null ? names[0] : null,
                                        LastName = names != null ? names[1] : null,
                                        EmailAddress = fromUser.Username,
                                        IsAccepted = false,
                                        IsConnected = false,
                                        IsBlocked = false,
                                        Initiated = true,
                                        IsDeleted = false,
                                        Sent = true,
                                        DateSent = DateTime.Now,
                                        IsValid = true
                                    };

                                    dataHelper.Add<Connection>(networkContact, fromUser.Username);
                                }

                            }
                            var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation.html"));
                            var body = reader.ReadToEnd();
                            string name = contact.FirstName + " " + contact.LastName;
                            var subject = string.Empty;
                            if (toUser != null)
                            {
                                name = toUser.FullName;
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

                            subject = string.Format("{0} Invites you to connect at Joblisting", string.Format("{0}", fromUser.FullName));

                            body = body.Replace("@@receiver", name);
                            body = body.Replace("@@sender", string.Format("{0}", fromUser.FullName));
                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, fromUser.PermaLink));
                            body = body.Replace("@@accepturl",
                                string.Format("{0}://{1}/Network/Accept/{2}", Request.Url.Scheme, Request.Url.Authority,
                                    contact.Id));
                            body = body.Replace("@@button", "Accept");

                            if (fromUser != null)
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

                            result = new ResponseEntity()
                            {
                                Type = "Success",
                                Message = "Connection invitation sent successfully!",
                            };
                        }
                        else
                        {
                            if (contact.IsDeleted)
                            {
                                contact.IsAccepted = false;
                                contact.IsConnected = false;
                                contact.IsDeleted = false;
                                contact.Initiated = true;
                                contact.CreatedBy = fromUser.Username;
                                contact.DateUpdated = DateTime.Now;
                                contact.UpdatedBy = fromUser.Username;

                                if (toUser != null)
                                {
                                    Connection networkContact = dataHelper.Get<Connection>().SingleOrDefault(x => x.UserId == toUser.Id && x.EmailAddress == fromUser.Username);
                                    if (networkContact != null)
                                    {
                                        networkContact.IsAccepted = false;
                                        networkContact.IsConnected = false;
                                        networkContact.IsDeleted = false;
                                        networkContact.Initiated = true;
                                        networkContact.CreatedBy = fromUser.Username;
                                        networkContact.DateUpdated = DateTime.Now;
                                        networkContact.UpdatedBy = fromUser.Username;

                                        dataHelper.UpdateEntity<Connection>(contact);
                                        dataHelper.UpdateEntity<Connection>(networkContact);
                                        dataHelper.Save();
                                    }
                                }

                                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation.html"));
                                var body = reader.ReadToEnd();
                                string name = contact.FirstName + " " + contact.LastName;
                                var subject = string.Empty;
                                if (toUser != null)
                                {
                                    name = toUser.FullName;
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

                                subject = string.Format("{0} Invites you to connect at Joblisting", fromUser.FullName);
                                body = body.Replace("@@receiver", name);
                                body = body.Replace("@@sender", fromUser.FullName);
                                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, fromUser.PermaLink));
                                body = body.Replace("@@accepturl",
                                    string.Format("{0}://{1}/Network/Accept/{2}", Request.Url.Scheme, Request.Url.Authority,
                                        contact.Id));
                                body = body.Replace("@@button", "Accept");


                                string[] receipent = { contact.EmailAddress };

                                AlertService.Instance.SendMail(subject, receipent, body);

                                result = new ResponseEntity()
                                {
                                    Type = "Success",
                                    Message = "Connection invitation sent successfully!",
                                };
                            }
                            else
                            {
                                if (contact.CreatedBy.Equals(fromUser.Username))
                                {
                                    if (contact.IsAccepted == false && contact.IsConnected == false)
                                    {
                                        result = new ResponseEntity()
                                        {
                                            Type = "Error",
                                            Message = "Connection invitation already sent, waiting for acceptance!"
                                        };
                                    }
                                    else if (contact.IsAccepted == true & contact.IsConnected == true)
                                    {
                                        result = new ResponseEntity()
                                        {
                                            Type = "Error",
                                            Message = string.Format("You are already connected with {0}!", toUser.FullName)
                                        };
                                    }
                                }
                                else
                                {
                                    if (contact.IsAccepted == false && contact.IsConnected == false)
                                    {
                                        TempData["UpdateData"] = string.Format("Connection is already initiated by {0} and waiting for  your acceptance!<br/>You may &nbsp;<a href=\"/Network/Accept?Id=" + contact.Id + "&redirect=/Network/Index\">Accept</a>&nbsp;or&nbsp;<a href=\"/Network/Disconnect?Id=" + contact.Id + "&redirect=/Network/Index\">Reject</a>", toUser.FullName);
                                        result = new ResponseEntity()
                                        {
                                            Type = "Error",
                                            Message = string.Format("Connection is already initiated by {0} and waiting for  your acceptance!", toUser.FullName),
                                            Data = contact
                                        };
                                    }
                                    else if (contact.IsAccepted == true & contact.IsConnected == true)
                                    {
                                        result = new ResponseEntity()
                                        {
                                            Type = "Error",
                                            Message = string.Format("You are already connected with {0}!", toUser.FullName)
                                        };
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        result = new ResponseEntity()
                        {
                            Type = "Error",
                            Message = "You cannot connect with yourself!"
                        };
                    }
                }
                else
                {
                    if (byLoggedInUser)
                    {
                        if (toUser != null)
                        {
                            result = new ResponseEntity()
                            {
                                Type = "Error",
                                Message = "You have blocked this person!"
                            };
                        }
                    }
                    else if (byEmailUser)
                    {
                        if (fromUser != null)
                        {
                            result = new ResponseEntity()
                            {
                                Type = "Error",
                                Message = "This profile is in private mode!"
                            };
                        }
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Disconnect(string from, string to)
        {
            ResponseEntity result = null;
            UserInfoEntity fromUser = iUserService.Get(from);

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Connection connection = ConnectionHelper.Get(to.Trim().ToLower(), from.Trim().ToLower());

                if (connection != null)
                {
                    if (connection.IsDeleted == false && connection.Initiated == false && connection.IsAccepted == false && connection.IsBlocked == false && connection.IsConnected == false)
                    {
                        connection.IsAccepted = false;
                        connection.IsConnected = false;
                        connection.IsDeleted = true;
                        connection.Initiated = false;
                        connection.DateDeleted = DateTime.Now;
                        connection.UpdatedBy = fromUser.Username;
                        connection.DateUpdated = DateTime.Now;
                        dataHelper.Update<Connection>(connection);

                        result = new ResponseEntity()
                        {
                            Type = "Success",
                            Message = "Disconnected successfully!"
                        };
                    }
                    else
                    {
                        connection.IsAccepted = false;
                        connection.IsConnected = false;
                        connection.IsDeleted = true;
                        connection.Initiated = false;
                        connection.DateDeleted = DateTime.Now;
                        connection.UpdatedBy = fromUser.Username;
                        connection.DateUpdated = DateTime.Now;

                        dataHelper.UpdateEntity<Connection>(connection);

                        UserProfile connected = MemberService.Instance.Get(connection.EmailAddress);
                        if (connected != null)
                        {
                            Hashtable parameters = new Hashtable();
                            parameters.Add("UserId", connected.UserId);
                            parameters.Add("EmailAddress", fromUser.Username);
                            Connection connectedUser = dataHelper.GetSingle<Connection>(parameters);
                            if (connectedUser != null)
                            {
                                connectedUser.IsAccepted = false;
                                connectedUser.IsConnected = false;
                                connectedUser.IsDeleted = true;
                                connectedUser.Initiated = false;
                                connectedUser.UpdatedBy = fromUser.Username;
                                connectedUser.DateDeleted = DateTime.Now;
                                connectedUser.DateUpdated = DateTime.Now;
                                dataHelper.UpdateEntity<Connection>(connectedUser);
                            }
                        }
                        long user_id = 0;
                        if (connected != null)
                        {
                            if (connected.UserId != fromUser.Id)
                            {
                                user_id = connected.UserId;
                            }

                            var outcome = dataHelper.Get<Communication>().Where(x => (x.SenderId == fromUser.Id || x.ReceiverId == fromUser.Id) || (x.SenderId == connected.UserId || x.ReceiverId == connected.UserId));
                            foreach (var item in outcome)
                            {
                                item.IsDeleted = true;
                                item.DateUpdated = DateTime.Now;
                                item.UpdatedBy = fromUser.Username;
                                dataHelper.UpdateEntity<Communication>(item);
                            }
                        }

                        dataHelper.Save();
                        result = new ResponseEntity()
                        {
                            Type = "Success",
                            Message = "Disconnected successfully!"
                        };
                    }
                }
                else
                {
                    result = new ResponseEntity()
                    {
                        Type = "Error",
                        Message = "Connection does not exist!"
                    };
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Block(string from, string to)
        {
            ResponseEntity outcome = null;
            UserInfoEntity fromUser = iUserService.Get(from);
            email = to.Trim().ToLower();

            SecurityRoles type = (SecurityRoles)fromUser.Type;
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
                    connected = ConnectionHelper.IsConnected(email, fromUser.Username); // Connected
                    DomainService.Instance.Block(friend.UserId, fromUser.Id);
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

                            var result = dataHelper.Get<Tracking>().Where(x => typeList.Contains(x.Type) && (x.JobseekerId == fromUser.Id || x.UserId == fromUser.Id) && x.Job.EmployerId == friend.UserId && x.IsDeleted == false);

                            if (result.Count() > 0)
                            {
                                app_list = result.ToList();
                            }

                            var msg_list = dataHelper.Get<Communication>().Where(x => (x.UserId == fromUser.Id && (x.SenderId == friend.UserId || x.ReceiverId == friend.UserId)) || (x.UserId == friend.UserId && (x.SenderId == fromUser.Id || x.ReceiverId == fromUser.Id)) && x.IsDeleted == false).ToList();
                            foreach (Communication item in msg_list)
                            {
                                dataHelper.DeleteUpdate<Communication>(item, fromUser.Username);
                            }
                            if (msg_list.Count > 0)
                            {
                                dataHelper.Save();
                            }

                            var bResult = dataHelper.Get<Tracking>().Where(x => x.Type == bookmark && x.UserId == friend.UserId && x.JobseekerId == fromUser.Id && x.IsDeleted == false);
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
                                                UserId = fromUser.Id,
                                                DateUpdated = DateTime.Now,
                                                Unread = true
                                            };
                                            dataHelper.AddEntity(followUp);

                                            interview.Status = (int)InterviewStatus.WITHDRAW;
                                            interview.DateUpdated = DateTime.Now;
                                            interview.UpdatedBy = fromUser.Username;
                                            dataHelper.UpdateEntity(interview);
                                        }

                                        if (interviews.Count > 0)
                                        {
                                            dataHelper.Save();
                                        }
                                    }
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, fromUser.Username, out message);
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
                                            interview.UpdatedBy = fromUser.Username;
                                            dataHelper.UpdateEntity(interview);
                                        }

                                        if (interviews.Count > 0)
                                        {
                                            dataHelper.Save();
                                        }
                                    }
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, fromUser.Username, out message);
                                    int_count++;
                                    break;
                                case TrackingTypes.APPLIED:
                                    app_count++;
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, fromUser.Username, out message);
                                    break;
                                case TrackingTypes.AUTO_MATCHED:
                                    matchings++;
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, fromUser.Username, out message);
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

                        outcome = new ResponseEntity()
                        {
                            Type = "Success",
                            Message = string.Format("Successfully blocked {0}!", friend.Company)
                        };
                    }
                    else
                    {
                        outcome = new ResponseEntity()
                        {
                            Type = "Success",
                            Message = string.Format("Successfully blocked {0}!", string.Format("{0} {1}", friend.FirstName, friend.LastName))
                        };
                    }

                    // Sending mail
                    using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/from_jobseeker_block.html")))
                    {
                        var body = string.Empty;

                        body = reader.ReadToEnd();
                        body = body.Replace("@@firstname", fromUser.FullName);
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
                            string[] receipent = { fromUser.Username };
                            var subject = string.Format("You have blocked {0}", (!string.IsNullOrEmpty(friend.Company) ? friend.Company : string.Format("{0} {1}", friend.FirstName, friend.LastName)));

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }

                    using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/to_employer_block.html")))
                    {
                        var body = string.Empty;

                        body = reader.ReadToEnd();
                        body = body.Replace("@@employer", (!string.IsNullOrEmpty(friend.Company) ? friend.Company : string.Format("{0} {1}", friend.FirstName, friend.LastName)));

                        body = body.Replace("@@firstname", fromUser.FullName);
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
                            var subject = string.Format("{0} has Withdrawn Application and or Interview", fromUser.FullName);

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                    break;
                case SecurityRoles.Employers:

                    DomainService.Instance.Block(friend.UserId, fromUser.Id);

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

                            var result = dataHelper.Get<Tracking>().Where(x => typeList.Contains(x.Type) && x.JobseekerId == friend.UserId && (x.Job.EmployerId == fromUser.Id || x.UserId == fromUser.Id) && x.IsDeleted == false);
                            if (result.Count() > 0)
                            {
                                app_list = result.ToList();
                            }
                            var msg_list = dataHelper.Get<Communication>().Where(x => (x.UserId == fromUser.Id && (x.SenderId == friend.UserId || x.ReceiverId == friend.UserId)) || (x.UserId == friend.UserId && (x.SenderId == fromUser.Id || x.ReceiverId == fromUser.Id)) && x.IsDeleted == false).ToList();
                            foreach (Communication item in msg_list)
                            {
                                dataHelper.DeleteUpdate<Communication>(item, fromUser.Username);
                            }
                            if (msg_list.Count > 0)
                            {
                                dataHelper.Save();
                            }

                            var bResult = dataHelper.Get<Tracking>().Where(x => x.Type == bookmark && x.UserId == friend.UserId && x.JobId != null && x.Job.EmployerId == fromUser.Id && x.IsDeleted == false);
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
                                                UserId = fromUser.Id,
                                                DateUpdated = DateTime.Now,
                                                Unread = true
                                            };
                                            dataHelper.AddEntity(followUp);

                                            interview.Status = (int)InterviewStatus.WITHDRAW;
                                            interview.DateUpdated = DateTime.Now;
                                            interview.UpdatedBy = fromUser.Username;
                                            dataHelper.UpdateEntity(interview);
                                        }

                                        if (interviews.Count > 0)
                                        {
                                            dataHelper.Save();
                                        }
                                    }
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, fromUser.Username, out message);
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
                                            interview.UpdatedBy = fromUser.Username;
                                            dataHelper.UpdateEntity(interview);
                                        }

                                        if (interviews.Count > 0)
                                        {
                                            dataHelper.Save();
                                        }
                                    }
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, fromUser.Username, out message);
                                    int_count++;
                                    break;
                                case TrackingTypes.APPLIED:
                                    app_count++;
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, fromUser.Username, out message);
                                    break;
                                case TrackingTypes.AUTO_MATCHED:
                                    matchings++;
                                    record = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, item.Id, fromUser.Username, out message);
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

                        outcome = new ResponseEntity()
                        {
                            Type = "Success",
                            Message = string.Format("Successfully blocked {0}!", string.Format("{0} {1}", friend.FirstName, friend.LastName))
                        };
                    }
                    else
                    {
                        outcome = new ResponseEntity()
                        {
                            Type = "Success",
                            Message = string.Format("Successfully blocked {0}!", friend.Company)
                        };
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

                        if (app_count > 0 || matchings > 0 || int_count > 0)
                        {
                            body = body.Replace("@@content", "<ul>" + content.Trim() + "</ul>");
                            body = body.Replace("@@employer", fromUser.FullName);
                            string[] receipent = { friend.Username };
                            var subject = "Application(s) and or Interview(s) Rejected";

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }

                    using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/from_employer_block.html")))
                    {
                        var body = string.Empty;

                        body = reader.ReadToEnd();
                        body = body.Replace("@@employer", fromUser.FullName);

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

                        if (app_count > 0 || matchings > 0 || int_count > 0)
                        {
                            body = body.Replace("@@content", "<ul>" + content.Trim() + "</ul>");
                            string[] receipent = { fromUser.Username };
                            var subject = string.Format("You have blocked {0}", (!string.IsNullOrEmpty(friend.Company) ? friend.Company : string.Format("{0} {1}", friend.FirstName, friend.LastName)));

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                    break;
            }

            return Json(outcome, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Unblock(string from, string to)
        {
            ResponseEntity result = null;
            UserInfoEntity fromUser = iUserService.Get(from);
            UserInfoEntity toUser = iUserService.Get(to);

            if (fromUser != null)
            {
                int stat = DomainService.Instance.Unblock(toUser.Id, fromUser.Id);
                if (stat > 0)
                {
                    result = new ResponseEntity()
                    {
                        Type = "Success",
                        Message = "Unblocked successfully!"
                    };
                }
                else
                {
                    result = new ResponseEntity()
                    {
                        Type = "Error",
                        Message = "Unable to unblock user!"
                    };
                }
            }
            else
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "User does not exist!"
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Accept(string from, string to)
        {
            ResponseEntity result = null;

            UserInfoEntity acceptor = iUserService.Get(from);
            UserInfoEntity invitor = iUserService.Get(to);

            int status = iUserService.AcceptRequest(from, to);
            if (status > 0)
            {
                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation_accepted.html"));
                var body = reader.ReadToEnd();

                var subject = string.Format("{0} has Accepted Connection", acceptor.FullName);
                body = body.Replace("@@receiver", invitor.FullName);
                body = body.Replace("@@sender", acceptor.FullName);

                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, acceptor.PermaLink));
                string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, acceptor.Id);
                body = body.Replace("@@viewurl", viewurl);

                string[] receipent = { invitor.Username };

                AlertService.Instance.SendMail(subject, receipent, body);

                result = new ResponseEntity()
                {
                    Type = "Success",
                    Message = "Connection invitation accepted!"
                };
            }
            else
            {
                result = new ResponseEntity()
                {
                    Type = "Success",
                    Message = "Connection already rejected!"
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(long id, string username)
        {
            MessageService.Instance.Delete(id, username);
            ResponseEntity result = new ResponseEntity()
            {
                Type = "Success",
                Message = "Message deleted successfully!"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteAll(string from, string to)
        {
            int status = iUserService.DeleteAll(from, to);

            ResponseEntity result = new ResponseEntity()
            {
                Type = "Success",
                Message = "Message deleted successfully!"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Count(string from)
        {
            int status = iUserService.MessageCount(from);

            ResponseEntity result = new ResponseEntity()
            {
                Type = "Success",
                Message = "Message deleted successfully!",
                Data = status
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadMark(string from, string to)
        {
            int status = iUserService.ReadMark(from, to);
            ResponseEntity result = null;
            if (status > 0)
            {
                result = new ResponseEntity()
                {
                    Type = "Success",
                    Message = "Message deleted successfully!",
                    Data = status
                };
            }
            else
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Unable to mark as read!"
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CountryList()
        {
            List<JobPortal.Data.List> countryList = SharedService.Instance.GetCountryList();
            return Json(countryList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Agree(string deviceId, string name)
        {
            return Json(iUserService.Agree(deviceId, name), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetAgreement(string deviceId)
        {
            return Json(iUserService.GetAgreement(deviceId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult IsAccepted(string deviceId)
        {
            return Json(iUserService.IsAccepted(deviceId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Resend(string deviceId, string token)
        {
            ResponseEntity result = null;
            long userId = iUserService.IsActiveSession(deviceId, token);
            if (userId > 0)
            {
                UserInfoEntity uinfo = iUserService.Get(userId);
                string stoken = UIHelper.Get6DigitCode();
                iUserService.GenerateToken(userId, stoken);

                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/resend_confirmation.html"));
                var body = reader.ReadToEnd();
                if (uinfo.IsConfirmed)
                {
                    body = body.Replace("@@receiver", uinfo.FullName);
                }
                else
                {
                    body = body.Replace("@@receiver", "Dear User");
                }
                body = body.Replace("@@code", stoken);
                var hosturl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                    string.Format("/Confirm?id={0}&token={1}", userId, stoken);

                body = body.Replace("@@url", hosturl);

                string[] receipent = { uinfo.Username };
                var subject = "Confirm Your Email Address";

                AlertService.Instance.SendMail(subject, receipent, body);

                result = new ResponseEntity()
                {
                    Type = "Success",
                    Message = "Verification code sent!",
                    Data = 1
                };
            }
            else
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "Unable to re-send verification code!",
                    Data = 0
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Jobs(string title, int? countryId, string deviceId, string token, int offset = 1, int size = 5)
        {
            List<SearchedJobEntity> jobs = new List<SearchedJobEntity>();
            ResponseEntity result = null;
            long userId = iUserService.IsActiveSession(deviceId, token);
            if (userId > 0)
            {
                UserInfoEntity uinfo = iUserService.Get(userId);
                var jobSearch = new SearchJob
                {
                    Title = (!string.IsNullOrEmpty(title) ? title.Trim() : null),
                    CountryId = countryId,
                    PageNumber = offset,
                    PageSize = size,
                };

                jobSearch.Username = uinfo.Username;
                jobs = SearchService.Instance.Jobs(jobSearch);
                if (jobs.Count > 0)
                {
                    result = new ResponseEntity()
                    {
                        Type = "Success",
                        Message = "",
                        Data = jobs
                    };
                }
                else
                {
                    result = new ResponseEntity()
                    {
                        Type = "Error",
                        Message = "Unable to mark as read!"
                    };
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult JobBookmark(long Id, string deviceId, string token)
        {
            ResponseEntity result = null;
            long userId = iUserService.IsActiveSession(deviceId, token);
            UserInfoEntity userInfo = null;
            if (userId > 0)
            {
                userInfo = iUserService.Get(userId);
            }

            var message = string.Empty;
            if (userInfo != null && userInfo.Type == (int)SecurityRoles.Jobseeker)
            {
                if (!string.IsNullOrEmpty(userInfo.Title) && userInfo.CategoryId != null && userInfo.SpecializationId != null)
                {
                    var bookmark = iTrackerService.Bookmark(Id, (int)BookmarkedTypes.JOB, userId, out message);
                    result = new ResponseEntity()
                    {
                        Type = "Success",
                        Message = message
                    };
                }
                else
                {
                    result = new ResponseEntity()
                    {
                        Type = "Error",
                        Message = "Please complete your profile!"
                    };
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Apply(long JobId, string deviceId, string token)
        {
            ResponseEntity result = null;
            long userId = iUserService.IsActiveSession(deviceId, token);
            UserInfoEntity userInfo = null;
            if (userId > 0)
            {
                userInfo = iUserService.Get(userId);
            }

            if (userInfo != null)
            {
                if (string.IsNullOrEmpty(userInfo.Content) || string.IsNullOrEmpty(userInfo.Title) || userInfo.CategoryId == null || userInfo.SpecializationId == null)
                {
                    result = new ResponseEntity()
                    {
                        Type = "Error",
                        Message = "Upload or build your resume online at joblisting.com!"
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var job = JobPortal.Domain.JobService.Instance.Get(JobId);
                if (job != null)
                {
                    UserProfile employer = MemberService.Instance.Get(job.EmployerId.Value);

                    TrackingStatus widthdrawn = TrackingService.Instance.GetStatus(userId, null, TrackingTypes.WITHDRAWN, job.Id, userId);
                    if (widthdrawn != null && (TrackingTypes)widthdrawn.Type != TrackingTypes.WITHDRAWN)
                    {
                        TrackingStatus trackingStatus = ApplicationService.Instance.Apply(job.Id, userId);
                        if (trackingStatus != null && trackingStatus.StatusCount > 0)
                        {
                            switch ((TrackingTypes)trackingStatus.Type)
                            {
                                case TrackingTypes.APPLIED:
                                    result = new ResponseEntity()
                                    {
                                        Type = "Error",
                                        Message = string.Format("You have already applied for {0}!", job.Title)
                                    };
                                    break;
                                case TrackingTypes.AUTO_MATCHED:
                                    result = new ResponseEntity()
                                    {
                                        Type = "Error",
                                        Message = string.Format("You have already applied for {0}!", job.Title)
                                    };
                                    break;
                            }
                        }
                        else
                        {
                            if (trackingStatus.Id != null)
                            {
                                Send(job, employer, userId);
                                result = new ResponseEntity()
                                {
                                    Type = "Success",
                                    Message = string.Format("You have successfully applied for {0}!", job.Title)
                                };
                            }
                        }
                    }
                    else
                    {
                        TrackingStatus trackingStatus = ApplicationService.Instance.Reapply(widthdrawn.Id, job.Id, userId);
                        if (trackingStatus != null && trackingStatus.StatusCount > 0)
                        {
                            switch ((TrackingTypes)trackingStatus.Type)
                            {
                                case TrackingTypes.APPLIED:
                                    result = new ResponseEntity()
                                    {
                                        Type = "Error",
                                        Message = string.Format("You have already applied for {0}!", job.Title)
                                    };
                                    break;
                                case TrackingTypes.AUTO_MATCHED:
                                    result = new ResponseEntity()
                                    {
                                        Type = "Error",
                                        Message = string.Format("You have already applied for {0}!", job.Title)
                                    };
                                    break;
                            }
                        }
                        else
                        {
                            if (trackingStatus.Id != null)
                            {
                                Send(job, employer, userId);
                                result = new ResponseEntity()
                                {
                                    Type = "Error",
                                    Message = string.Format("You have successfully applied for {0}!", job.Title)
                                };
                            }
                        }
                    }
                }
            }
            else
            {
                result = new ResponseEntity()
                {
                    Type = "Error",
                    Message = "User does not exist!"
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Email(long JobId, string deviceId, string token, string emails)
        {
            ResponseEntity result = null;
            long userId = iUserService.IsActiveSession(deviceId, token);
            UserInfoEntity userInfo = null;
            if (userId > 0)
            {
                userInfo = iUserService.Get(userId);
            }

            if (userInfo != null)
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
                            var job = dataHelper.GetSingle<Job>(JobId);

                            SenderName = userInfo.FullName;
                            SenderEmailAddress = userInfo.Username;

                            var receipents = emails.Split(',').Distinct().ToArray<string>();
                            var body = string.Empty;
                          
                            if (userInfo.Type == 5)
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
                            result = new ResponseEntity()
                            {
                                Type = "Success",
                                Message = "Job sharing link sent successfully!"
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    result = new ResponseEntity()
                    {
                        Type = "Error",
                        Message = "Unable to share link!"
                    };
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private void Send(Job job, UserProfile employer, long userId)
        {
            UserInfoEntity uinfo = iUserService.Get(userId);

            using (var reader = new StreamReader(HttpContext.Server.MapPath("~/Templates/Mail/apply.html")))
            {
                string body = reader.ReadToEnd();
                body = body.Replace("@@firstname", uinfo.FullName);
                body = body.Replace("@@lastname", " ");
                body = body.Replace("@@jobtitle", job.Title);
                body = body.Replace("@@joburl",
                    string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                body = body.Replace("@@companyprofile",
                    string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, employer.PermaLink));
                body = body.Replace("@@employer", employer.Company);

                string[] receipent = { uinfo.Username };
                var subject = string.Format("Application for {0}", job.Title);

                AlertService.Instance.SendMail(subject, receipent, body);
            }
            using (var reader = new StreamReader(HttpContext.Server.MapPath("~/Templates/Mail/employer_apply.html")))
            {
                if (!employer.Username.ToLower().Contains("admin"))
                {
                    string body = reader.ReadToEnd();
                    body = body.Replace("@@firstname", uinfo.FullName);
                    body = body.Replace("@@lastname", " ");
                    body = body.Replace("@@jobtitle", job.Title);
                    body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));

                    string rurl = string.Format("{0}://{1}/applications", Request.Url.Scheme, Request.Url.Authority);
                    body = body.Replace("@@downloadurl", string.Format("{0}://{1}/jobseeker/Download?id={2}&redirect={3}", Request.Url.Scheme, Request.Url.Authority, userId, rurl));
                    body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, uinfo.PermaLink));
                    body = body.Replace("@@employer", employer.Company);

                    string[] receipent = { employer.Username };
                    var subject = string.Format("Application for {0}", job.Title);

                    AlertService.Instance.SendMail(subject, receipent, body);
                }
            }
        }

        private void SendMessage(string deviceId, string mobile, long id)
        {
            string message = string.Empty;
            Agreement agreement = iUserService.GetAgreement(deviceId);
            string name = agreement.Name;

            string AccountSid = ConfigService.Instance.GetConfigValue("TwilioSID");
            string AuthToken = ConfigService.Instance.GetConfigValue("TwilioToken");
            string from = ConfigService.Instance.GetConfigValue("TwilioNumber");
            string mobile_app_url = ConfigService.Instance.GetConfigValue("mobile_app_url");

            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            StringBuilder sbSMS = new StringBuilder();

            sbSMS.AppendFormat("This is {0}.\n", name);
            sbSMS.Append("I am using joblisting.com messenger!\n");
            var url = string.Format("{0}", mobile_app_url);
            sbSMS.AppendFormat("Download here {0} and connect!", url);

            var sms = twilio.SendMessage(from, mobile, sbSMS.ToString());
            if (sms.RestException == null)
            {
                var msg = twilio.GetMessage(sms.Sid);
                if (msg.Status.Equals("delivered") || msg.Status.Equals("sent"))
                {
                    int status = iUserService.RecordSMSStatus(id, msg.Status);
                }
                else
                {
                    iUserService.RecordSMSStatus(id, msg.Status);
                }
            }
            else
            {
                if (sms.RestException.Code.Equals("14101") || sms.RestException.Code.Equals("21211"))
                {
                    iUserService.RecordSMSStatus(id, sms.RestException.Message);
                }
                else
                {
                    iUserService.RecordSMSStatus(id, "Failed");
                }
            }
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
        [HttpPost]
        public ActionResult SendOTP(string country, string mobile1)
        {

            Regex reg = null;
            String mobile = Convert.ToString(Request["number"]);
            reg = new Regex("^[1-9][0-9]*$");

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {

                SqlCommand sql = new SqlCommand("SELECT * FROM UserProfiles WHERE Mobile = @mobile", conn);
                SqlDataAdapter sda = new SqlDataAdapter(sql);
                sql.Parameters.AddWithValue("@mobile", mobile.Trim());
                DataSet ds = new DataSet();
                sda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string message = string.Empty;
                    if (!string.IsNullOrEmpty(mobile) && reg.IsMatch(mobile))
                    {
                        Random random = new Random();
                        int value = random.Next(1001, 9999);
                        string AccountSid = ConfigService.Instance.GetConfigValue("TwilioSID");
                        string AuthToken = ConfigService.Instance.GetConfigValue("TwilioToken");
                        string from = ConfigService.Instance.GetConfigValue("TwilioNumber");
                        var twilio = new TwilioRestClient(AccountSid, AuthToken);

                        StringBuilder sbSMS = new StringBuilder();
                        sbSMS.AppendFormat("Yur Joblisting account verification OTP is " + value + " ", value);
                        ViewBag.SaveData = "OTP sent successfully!";
                        ViewBag.SaveData1 = mobile;
                        Guid token = Guid.NewGuid();
                        Session["otp"] = value;

                        string to = string.Format("{0}{1}", country, mobile);
                        using (JobPortalEntities context = new JobPortalEntities())
                        {

                            var sms = twilio.SendMessage(from, to, sbSMS.ToString());

                            if (sms.RestException == null)
                            {
                                Session["SaveData"] = "OTP sent successfully!!";
                                //Session["number"] = mobile;
                                var msg = twilio.GetMessage(sms.Sid);
                                if (msg.Status.Equals("delivered") || msg.Status.Equals("sent"))
                                {

                                    //Session["SaveData"] = "OTP sent successfully!";
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
                        ViewBag.Error1 = "Please provide valid mobile number!";
                    }
                }
                else
                {

                    TempData["error"] = mobile + " is not register with Joblisting";
                    return Content("Please provide valid mobile number!");
                }
            }

            return Json(mobile, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Verify()
        {
            String otp = Convert.ToString(Request["otp"]);
            string enterotp = Convert.ToString(Session["otp"]);


            if (otp.Equals(enterotp))
            {
                TempData["valid"] = "OTP verified..!";
                //Session["msg"] = ViewBag.ss;

                return Content("OTP verified..!");

            }
            else
            {
                TempData["invalid"] = "You Entered Wrong OTP try again!";
                return Content("You Entered Wrong OTP try again!");
            }
        }


        [HttpPost]

        public ActionResult ForgotPassword(string Email)
        {
            ResponseEntity result = new ResponseEntity();
            var profile = MemberService.Instance.Get(Email);
            var subject = "Reset Your Joblisting Account Password";
            if (profile != null)
            {
                var message = new StringBuilder();
                var membershipUser = Membership.GetUser(Email);

                if (membershipUser != null && membershipUser.IsApproved)
                {
                    var token = WebSecurity.GeneratePasswordResetToken(Email);

                    var reader = new StreamReader(Server.MapPath("~/Templates/Mail/forgot.html"));
                    var body = reader.ReadToEnd();
                    body = body.Replace("@@firstname", profile.FirstName);
                    body = body.Replace("@@lastname", profile.LastName);
                    body = body.Replace("@@url", UrlManager.GetPasswordResetUrl(token, profile.Username));

                    string[] receipent = { profile.Username };
                     subject = "Reset Your Joblisting Account Password";

                    AlertService.Instance.SendMail(subject, receipent, body);
                }
                //return RedirectToAction("thanks", "Home", new { email = model.Email });
               
            }
            return Json(subject, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'DeviceChangePass' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult ChangePass(DeviceChangePass model)
#pragma warning restore CS0246 // The type or namespace name 'DeviceChangePass' could not be found (are you missing a using directive or an assembly reference?)
        {
            ResponseEntity result = new ResponseEntity();

            Regex regex = new Regex(@"^(?=.*\d)(?=.*[a-zA-Z]).{9,}$");

            if (model.NewPassword.Equals(model.ConfirmPassword))
            {
                if (regex.IsMatch(model.NewPassword))
                {
                    try
                    {
                        bool changed = WebSecurity.ChangePassword(model.Username, model.OldPassword,
                                   model.NewPassword);
                        if (changed)
                        {
                            result = new ResponseEntity()
                            {

                                Type = "Success",
                                Message = "Password changed successfully!"
                            };
                        }
                        else
                        {
                            result = new ResponseEntity()
                            {

                                Type = "Failed",
                                Message = "Unable to change password!"
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        result = new ResponseEntity()
                        {

                            Type = "Failed",
                            Message = "Old password and New Password does not match!"
                        };
                    }
                }
                else
                {
                    result = new ResponseEntity()
                    {

                        Type = "Failed",
                        Message = "Password Min. 9 numbers & letters mixed!"
                    };
                }
            }
            else
            {
                result = new ResponseEntity()
                {

                    Type = "Failed",
                    Message = "New Password and Confirm Password does not match!"
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PictureUpload(string Username, string data, string area, string type, string size)
        {
            string flag = "Failed";
            long photoId = 0;
            byte[] buffer = new byte[1];
            try
            {
                if (!string.IsNullOrEmpty(data) && !string.IsNullOrEmpty(area))
                {
                    buffer = !string.IsNullOrEmpty(data) ? Convert.FromBase64String(data) : new byte[1];
                }
                else
                {
                    return Json(flag, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(flag, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrEmpty(Username))
            {
                UserProfile original = MemberService.Instance.Get(Username);
                flag = "Uploaded successfully! It is in approval process.";
                string comment = string.Empty;
                int imageHeight = 0;
                int imageWidth = 0;
                if (string.IsNullOrEmpty(data) && data.Trim().Length <= 0)
                {
                    flag = "Unable to upload photo!";
                    return Json(flag, JsonRequestBehavior.AllowGet);
                }

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    int count = dataHelper.Get<Photo>().Count(x => x.Area.Equals(area) && x.UserId == original.UserId);
                    imageHeight = Convert.ToInt32(ConfigService.Instance.GetConfigValue(string.Format("{0}ImageHeight", area)));
                    imageWidth = Convert.ToInt32(ConfigService.Instance.GetConfigValue(string.Format("{0}ImageWidth", area)));
                    buffer = Convert.FromBase64String(data);
                    byte[] imgBytes = UIHelper.ResizeImage(buffer, imageWidth, imageHeight);


                    if (area.Equals("Profile"))
                    {
                        if (count == 0)
                        {
                            Photo photo = new Photo()
                            {
                                UserId = original.UserId,
                                Image = imgBytes,
                                NewImage = null,
                                Area = area,
                                DateUpdated = DateTime.Now,
                                IsApproved = false,
                                IsRejected = false,
                                IsDeleted = false,
                                Type = type,
                                ImageSize = size
                            };
                            photoId = Convert.ToInt64(dataHelper.Add<Photo>(photo));
                        }
                        else
                        {
                            Photo photo = dataHelper.Get<Photo>().SingleOrDefault(x => x.Area.Equals(area) && x.UserId == original.UserId);
                            photo.NewImage = imgBytes;
                            photo.Area = area;
                            photo.DateUpdated = DateTime.Now;
                            if (photo.IsApproved == true)
                            {
                                photo.IsApproved = false;
                                photo.IsRejected = false;
                                photo.InEditMode = true;
                            }
                            photo.Type = type;
                            photo.NewImageSize = size;
                            dataHelper.Update<Photo>(photo);
                            photoId = photo.Id;
                        }
                        comment = "Profile picture uploaded";
                    }

                }

                string subject = "Photo Uploaded Successfully";

                using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/photoupload.html")))
                {
                    string body = reader.ReadToEnd();
                    body = body.Replace("@@firstname", original.FirstName);
                    body = body.Replace("@@lastname", original.LastName);
                    string[] receipent = { original.Username };

                    AlertService.Instance.Send(subject, original.Username, body);
                }

                using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/photoupload_to_admin.html")))
                {
                    string body = reader.ReadToEnd();

                    subject = "Photo Uploaded for Review";
                    body = body.Replace("@@type", "photo");
                    body = body.Replace("@@uploader", string.Format("{0} {1}", original.FirstName, original.LastName));
                    body = body.Replace("@@url", string.Format("{0}://{1}/admin/photolist?type={2}", Request.Url.Scheme, Request.Url.Authority, original.Type));

                    body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, original.PermaLink));
                    string admin = ConfigurationManager.AppSettings["admin_email"];
                    AlertService.Instance.Send(subject, admin, body);
                }
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public Photo GetPhoto(string area, long UserId)
        {
            Photo photo = null;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                photo = dataHelper.Get<Photo>().SingleOrDefault(x => x.UserId == UserId && x.IsDeleted == false && ((x.IsApproved == true && x.IsRejected == false && x.InEditMode == false) || (x.IsApproved == false && x.IsRejected == false && x.InEditMode == true)) && x.Area.Equals(area));
            }

            return photo;
        }
        
        //[AllowAnonymous]
        //public ActionResult PromotedJobseekers(string deviceId, string token, string gender=null, int pageNumber = 1)
        //{
        //    List<ContactEntity> list = new List<ContactEntity>();
        //    ResponseEntity result = null;

        //    if (string.IsNullOrEmpty(deviceId))
        //    {
        //        result = new ResponseEntity()
        //        {
        //            Type = "Error",
        //            Message = "Device Id is required!"
        //        };
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }

        //    try
        //    {
        //        var searchResume = new SearchResume
        //        {
        //            PageNumber = pageNumber,
        //            PageSize = 2
        //        };
        //        long userId = iUserService.IsActiveSession(deviceId, token);
        //        if (userId > 0)
        //        {
        //            searchResume.UserId = userId;
        //            list = iUserService.PaidSearch(pageNumber, userId, gender);

        //            result = new ResponseEntity()
        //            {
        //                Type = "Success",
        //                Message = "",
        //                Data = list
        //            };
        //        }
        //        else
        //        {
        //            result = new ResponseEntity()
        //            {
        //                Type = "Error",
        //                Message = "Invalid credentials or Session expired!"
        //            };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        result = new ResponseEntity()
        //        {
        //            Type = "Error",
        //            Data = list,
        //            Message = ex.Message
        //        };
        //    }

        //    JsonResult jr = new JsonResult();
        //    jr.Data = result;
        //    jr.MaxJsonLength = int.MaxValue;
        //    jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

        //    return jr;
        //}
    }
}