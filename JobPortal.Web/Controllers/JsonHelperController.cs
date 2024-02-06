using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using System.Collections;
using System.IO;
using System.Web.Script.Serialization;
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Helpers;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Web.Models;
using System.Text;
using System.Configuration;
using System.Web.Security;
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Utility;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Text.RegularExpressions;
using Twilio;
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Threading.Tasks;

namespace JobPortal.Web.Controllers
{
    public class JsonHelperController : BaseController
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
        public JsonHelperController(IUserService service, IHelperService helper, IJobService jobService)
#pragma warning restore CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
            : base(service)
        {
            this.helper = helper;
            this.jobService = jobService;
        }

        //
        // GET: /JsonHelper/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GlobalInbox(long? CountryId, string fd, string fm, string fy, string td, string tm, string ty, int pageNumber = 1)
        {
            int pageSize = 20;

            string sdate = string.Empty;
            string edate = string.Empty;
            var sdt = new DateTime?();
            var edt = new DateTime?();

            UserInfoEntity uinfo = _service.Get(User.Id);
            if (uinfo.Username == "tasnim@joblisting.com" || uinfo.Username == "lakshmii@joblisting.com")
            {
                CountryId = 106;
            }
             else if (uinfo.Username == "maitrantn@joblisting.com" || uinfo.Username == "Linhv@joblisting.com")
            {
                CountryId = 3343;
            }
            else if (uinfo.Username == "sandhyam@joblisting.com" || uinfo.Username == "Tanyam@joblisting.com")
            {
                CountryId = 139;
            }
            else if (uinfo.Username == "sandhyas@joblisting.com" || uinfo.Username == "balus@joblisting.com")
            {
                CountryId = 205;
            }
            else if (uinfo.Username == "adminIndo@joblisting.com" || uinfo.Username == "RecruiterIndo@joblisting.com")
            {
                CountryId = 107;
            }
            else if (uinfo.Username == "adminPhil@joblisting.com" || uinfo.Username == "RecruiterPhil@joblisting.com")
            {
                CountryId = 180;
            }

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg"


)
            {
                CountryId = 40;
            }
            if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy))
            {
                sdate = string.Format("{0}/{1}/{2}", fm, fd, fy);
                sdt = Convert.ToDateTime(sdate);
            }

            if (!string.IsNullOrEmpty(td) && !string.IsNullOrEmpty(tm) && !string.IsNullOrEmpty(ty))
            {
                if (string.IsNullOrEmpty(fd) && string.IsNullOrEmpty(fm) && string.IsNullOrEmpty(fy))
                {
                    sdt = DateTime.Now;
                }
                edate = string.Format("{0}/{1}/{2}", tm, td, ty);
                edt = Convert.ToDateTime(edate);
            }
            else
            {
                if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy))
                {
                    edt = DateTime.Now;
                }
            }

            List<GlobalInboxEntity> list = DomainService.Instance.GetGlobalInboxList(CountryId, sdt, edt, pageSize, pageNumber);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InboxList(long CountryId, string fd, string fm, string fy, string td, string tm, string ty, string name, int type = 0, int pageNumber = 1)
        {
            int pageSize = 20;

            string sdate = string.Empty;
            string edate = string.Empty;
            var sdt = new DateTime?();
            var edt = new DateTime?();

            if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy))
            {
                sdate = string.Format("{0}/{1}/{2}", fm, fd, fy);
                sdt = Convert.ToDateTime(sdate);
            }

            if (!string.IsNullOrEmpty(td) && !string.IsNullOrEmpty(tm) && !string.IsNullOrEmpty(ty))
            {
                if (string.IsNullOrEmpty(fd) && string.IsNullOrEmpty(fm) && string.IsNullOrEmpty(fy))
                {
                    sdt = DateTime.Now;
                }
                edate = string.Format("{0}/{1}/{2}", tm, td, ty);
                edt = Convert.ToDateTime(edate);
            }
            else
            {
                if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy))
                {
                    edt = DateTime.Now;
                }
            }

            List<InboxEntity> list = DomainService.Instance.GetInboxList(CountryId, type, sdt, edt, name, pageSize, pageNumber);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AccountStatistics(long? countryId)
        {
            UserInfoEntity uinfo = _service.Get(User.Id);
            if (uinfo.Username == "tasnim@joblisting.com" || uinfo.Username == "lakshmii@joblisting.com")
            {
                countryId = 106;
            }
             else if (uinfo.Username == "maitrantn@joblisting.com" || uinfo.Username == "Linhv@joblisting.com")
            {
                countryId = 3343;
            }
            else if (uinfo.Username == "sandhyam@joblisting.com" || uinfo.Username == "Tanyam@joblisting.com")
            {
                countryId = 139;
            }
            else if (uinfo.Username == "sandhyas@joblisting.com" || uinfo.Username == "balus@joblisting.com")
            {
                countryId = 205;
            }
            else if (uinfo.Username == "adminIndo@joblisting.com" || uinfo.Username == "RecruiterIndo@joblisting.com")
            {
                countryId = 107;
            }
            else if (uinfo.Username == "adminPhil@joblisting.com" || uinfo.Username == "RecruiterPhil@joblisting.com")
            {
                countryId = 180;
            }

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com"  || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
            {
                countryId = 40;
            }
            AccountsInfoEntity data = (new PortalDataService()).GetAccountStatistics(countryId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult IsFriend(long UserId, string Username)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(Username))
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", Username);
                    if (profile != null)
                    {
                        UserProfile friendProfile = dataHelper.GetSingle<UserProfile>("UserId", UserId);
                        if (friendProfile != null)
                        {
                            Hashtable parameters = new Hashtable();
                            parameters.Add("UserId", profile.UserId);
                            parameters.Add("EmailAddress", friendProfile.Username);
                            parameters.Add("IsConnected", true);

                            flag = (dataHelper.GetCounts<Connection>(parameters) > 0);
                        }
                    }
                }
            }
            return Json(flag);
        }

        [Authorize]
        [System.Web.Mvc.HttpPost]
        [ValidateInput(true)]
        public ActionResult UpdateWebAddress(string address)
        {
            string message = string.Empty;
            UserProfile loggedInUser = MemberService.Instance.Get(User.Username);
            if (!string.IsNullOrEmpty(address))
            {
                UserProfile profile = MemberService.Instance.GetByAddress(address);
                if (profile != null)
                {
                    if (!loggedInUser.PermaLink.Equals(address))
                    {
                        message = "NA";
                    }
                    else
                    {
                        message = address.ToLower();
                    }
                }
                else
                {
                    profile = MemberService.Instance.Get(User.Username);
                    profile.PermaLink = address.ToLower();
                    MemberService.Instance.Update(profile);
                    message = address.ToLower();
                }
            }
            return Json(message);
        }

        public ActionResult GetConnectionCounts(long? countryId)
        {
            int todays = 0;
            int sevenDays = 0;
            Hashtable data = new Hashtable();
            DateTime end_date = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            DateTime start_date = end_date.Subtract(new TimeSpan(6, 0, 0, 0));

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var connections = dataHelper.Get<UserProfile>().Where(x => x.DateCreated.Day == DateTime.Now.Day && x.DateCreated.Month == DateTime.Now.Month && x.DateCreated.Year == DateTime.Now.Year);
                if (countryId != null)
                {
                    todays = connections.Count(x => x.IsDeleted == false && x.IsActive == true && x.CountryId == countryId.Value && x.Connections.Count > 0);
                }
                else
                {
                    todays = connections.Count(x => x.IsDeleted == false && x.IsActive == true && x.Connections.Count > 0);
                }
            }

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var connections = dataHelper.Get<UserProfile>().
                    Where(x => x.DateCreated.Day >= start_date.Day && x.DateCreated.Month >= start_date.Month && x.DateCreated.Year >= start_date.Year && x.DateCreated.Day <= end_date.Day && x.DateCreated.Month <= end_date.Month && x.DateCreated.Year <= end_date.Year);
                if (countryId != null)
                {
                    sevenDays = connections.Count(x => x.IsDeleted == false && x.IsActive == true && x.CountryId == countryId.Value && x.Connections.Count > 0);
                }
                else
                {
                    sevenDays = connections.Count(x => x.IsDeleted == false && x.IsActive == true && x.Connections.Count > 0);
                }
            }

            data.Add("Today", todays);
            data.Add("7 Days", sevenDays);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetMessageCountsByCountry(long? CountryId)
        {
            List<object> list = new List<object>();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                var result = dataHelper.Get<List>().Where(x => x.Name.Equals("Country"))
                    .Select(x => new { CountryId = x.Id, Country = x.Text, Code = x.Value, Individuals = x.UserCountries.Count(z => z.Type == 4 && z.Communications.Count() > 0), Companies = x.UserCountries.Count(z => z.Type == 5 && z.Communications.Count() > 0) });
                result = result.Where(x => (x.Individuals > 0 || x.Companies > 0));
                if (CountryId != null)
                {
                    result = result.Where(x => x.CountryId == CountryId.Value);
                }
                list = result.ToList<object>();
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult GetMessageCounts(long? CountryId)
        {
            Hashtable data = new Hashtable();
            int todays = 0;
            int sevenDays = 0;
            DateTime end_date = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            DateTime start_date = end_date.Subtract(new TimeSpan(6, 0, 0, 0));
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                todays = dataHelper.Get<UserProfile>().Count(x => (x.DateCreated.Day == DateTime.Now.Day
                        && x.DateCreated.Month == DateTime.Now.Month
                        && x.DateCreated.Year == DateTime.Now.Year));

                sevenDays = dataHelper.Get<UserProfile>().ToList().
                    Count(x => x.DateCreated.Date >= start_date.Date
                        && x.DateCreated.Date <= end_date.Date);

                if (CountryId != null)
                {
                    var t = dataHelper.Get<UserProfile>().Select(x => x.Communications.Count(y => y.DateCreated.Day == DateTime.Now.Day && y.DateCreated.Month == DateTime.Now.Month && y.DateCreated.Year == DateTime.Now.Year));
                    var s = dataHelper.Get<UserProfile>().Select(x => x.Communications.Count(y => (y.DateCreated.Day >= start_date.Day && y.DateCreated.Month >= start_date.Month && y.DateCreated.Year >= start_date.Year) && (y.DateCreated.Day <= end_date.Day && y.DateCreated.Month <= end_date.Month && y.DateCreated.Year <= end_date.Year)));

                    if (t.Count() > 0)
                    {
                        todays = t.Sum();
                    }
                    if (s.Count() > 0)
                    {
                        sevenDays = s.Sum();
                    }
                }
                else
                {
                    var t = dataHelper.Get<UserProfile>().Where(x => x.CountryId == CountryId.Value).Select(x => x.Communications.Count(y => y.DateCreated.Day == DateTime.Now.Day && y.DateCreated.Month == DateTime.Now.Month && y.DateCreated.Year == DateTime.Now.Year));
                    if (t.Count() > 0)
                    {
                        todays = t.Sum();
                    }

                    var s = dataHelper.Get<UserProfile>().Where(x => x.CountryId == CountryId.Value).Select(x => x.Communications.Count(y => (y.DateCreated.Day >= start_date.Day && y.DateCreated.Month >= start_date.Month && y.DateCreated.Year >= start_date.Year) && (y.DateCreated.Day <= end_date.Day && y.DateCreated.Month <= end_date.Month && y.DateCreated.Year <= end_date.Year)));
                    if (s.Count() > 0)
                    {
                        sevenDays = t.Sum();
                    }
                }
            }
            data.Add("Today", todays);
            data.Add("7 Days", sevenDays);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetConnectionCountsByCountry(long? countryId)
        {
            List<object> list = new List<object>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var users = dataHelper.Get<UserProfile>().Where(x => x.IsDeleted == false && x.IsActive == true && x.Connections.Count > 0);
                if (countryId != null)
                {
                    users = users.Where(x => x.CountryId == countryId.Value);
                }
                list = users.GroupBy(x => new { CountryId = x.CountryId, Code = x.Country.Value, Country = x.Country.Text }).Select(x => new { CountryId = x.Key.CountryId, Country = x.Key.Country, Code = x.Key.Code, Users = x.Count(), Individuals = x.Count(z => z.Type == 4), Companies = x.Count(z => z.Type == 5) }).ToList<object>();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRegisteredUsersByCountry(long? countryId, string fd, string fm, string fy, string td, string tm, string ty)
        {
            UserInfoEntity uinfo = _service.Get(User.Id);
            if (uinfo.Username == "tasnim@joblisting.com" || uinfo.Username == "lakshmii@joblisting.com")
            {
                countryId = 106;
            }
             else if (uinfo.Username == "maitrantn@joblisting.com" || uinfo.Username == "Linhv@joblisting.com")
            {
                countryId = 3343;
            }
            else if (uinfo.Username == "sandhyam@joblisting.com" || uinfo.Username == "Tanyam@joblisting.com")
            {
                countryId = 139;
            }
            else if (uinfo.Username == "sandhyas@joblisting.com" || uinfo.Username == "balus@joblisting.com")
            {
                countryId = 205;
            }
            else if (uinfo.Username == "adminIndo@joblisting.com" || uinfo.Username == "RecruiterIndo@joblisting.com")
            {
                countryId = 107;
            }
            else if (uinfo.Username == "adminPhil@joblisting.com" || uinfo.Username == "RecruiterPhil@joblisting.com")
            {
                countryId = 180;
            }

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
            {
                countryId = 40;
            }
            string sdate = string.Empty;
            string edate = string.Empty;
            var sdt = new DateTime?();
            var edt = new DateTime?();

            if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy))
            {
                sdate = string.Format("{0}/{1}/{2}", fm, fd, fy);
                sdt = Convert.ToDateTime(sdate);
            }

            if (!string.IsNullOrEmpty(td) && !string.IsNullOrEmpty(tm) && !string.IsNullOrEmpty(ty))
            {
                if (string.IsNullOrEmpty(fd) && string.IsNullOrEmpty(fm) && string.IsNullOrEmpty(fy))
                {
                    sdt = DateTime.Now;
                }
                edate = string.Format("{0}/{1}/{2}", tm, td, ty);
                edt = Convert.ToDateTime(edate);
            }
            else
            {
                if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy))
                {
                    edt = DateTime.Now;
                }
            }
            List<UserByCountryEntity> list = _service.CountryWiseUsers(countryId, sdt, edt, null, 1000, 1);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJobs(long? countryId, string status, string fd, string fm, string fy, string td, string tm, string ty)
        {
            UserInfoEntity uinfo = _service.Get(User.Id);
            if (uinfo.Username == "tasnim@joblisting.com" || uinfo.Username == "lakshmii@joblisting.com")
            {
                countryId = 106;
            }
             else if (uinfo.Username == "maitrantn@joblisting.com" || uinfo.Username == "Linhv@joblisting.com")
            {
                countryId = 3343;
            }
            else if (uinfo.Username == "sandhyam@joblisting.com" || uinfo.Username == "Tanyam@joblisting.com")
            {
                countryId = 139;
            }
            else if (uinfo.Username == "sandhyas@joblisting.com" || uinfo.Username == "balus@joblisting.com")
            {
                countryId = 205;
            }
            else if (uinfo.Username == "adminIndo@joblisting.com" || uinfo.Username == "RecruiterIndo@joblisting.com")
            {
                countryId = 107;
            }
            else if (uinfo.Username == "adminPhil@joblisting.com" || uinfo.Username == "RecruiterPhil@joblisting.com")
            {
                countryId = 180;
            }

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg"

)
            {
                countryId = 40;
            }
            Hashtable data = new Hashtable();
            List<JobsByCountryEntity> list = new List<JobsByCountryEntity>();

            string sdate = string.Empty;
            string edate = string.Empty;
            var sdt = new DateTime?();
            var edt = new DateTime?();
            if ((!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy)) || (!string.IsNullOrEmpty(td) && !string.IsNullOrEmpty(tm) && !string.IsNullOrEmpty(ty)))
            {
                if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy))
                {
                    sdate = string.Format("{0}/{1}/{2}", fm, fd, fy);
                    sdt = Convert.ToDateTime(sdate);
                }

                if (!string.IsNullOrEmpty(td) && !string.IsNullOrEmpty(tm) && !string.IsNullOrEmpty(ty))
                {
                    if (string.IsNullOrEmpty(fd) && string.IsNullOrEmpty(fm) && string.IsNullOrEmpty(fy))
                    {
                        sdt = DateTime.Now;
                    }
                    edate = string.Format("{0}/{1}/{2}", tm, td, ty);
                    edt = Convert.ToDateTime(edate);
                }
                else
                {
                    if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy))
                    {
                        edt = DateTime.Now;
                    }
                }
            }

            list = DomainService.Instance.GetJobsByCountry(countryId, status, true, sdt, edt, 1000, 1);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCountryJobs(long countryId)
        {
            List<object> list = new List<object>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<List>().Where(x => x.Name == "Country" && x.Id == countryId)
                    .Select(x => new { ActiveJobs = x.JobCountries.Count(z => z.IsActive == true && z.IsPublished == true && z.IsDeleted == false && z.IsExpired.Value == false), DeactiveJobs = x.JobCountries.Count(z => z.IsActive == false && z.IsPublished == true && z.IsDeleted == false), ExpiredJobs = x.JobCountries.Count(z => z.IsExpired.Value == true && z.IsDeleted == false), DeletedJobs = x.JobCountries.Count(z => z.IsDeleted == true), RejectedJobs = x.JobCountries.Count(z => z.IsRejected == true && z.IsDeleted == false), JobsInApproval = x.JobCountries.Count(z => z.IsDeleted == false && z.IsActive == true && z.IsPublished == false && z.IsRejected == false) });

                list = result.ToList<object>();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJobsByCountry(long countryId, string name)
        {
            List<object> list = new List<object>();
            List<object> jobsByStateList = new List<object>();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<Job>().Where(x => x.CountryId == countryId).GroupBy(x => new { Id = x.EmployerId, Company = x.Employer.Company })
                .Select(x => new
                {
                    Id = x.Key.Id,
                    Company = x.Key.Company,
                    ActiveJobs = x.Count(z => z.IsActive == true && z.IsPublished == true && z.IsDeleted == false),
                    DeactiveJobs = x.Count(z => z.IsActive == false && z.IsPublished == true && z.IsDeleted == false),
                    ExpiredJobs = x.Count(z => z.ClosingDate.Day <= DateTime.Now.Day && z.ClosingDate.Month < DateTime.Now.Month && z.ClosingDate.Year <= DateTime.Now.Year && z.IsDeleted == false),
                    DeletedJobs = x.Count(z => z.IsDeleted == true),
                    RejectedJobs = x.Count(z => z.IsRejected == true && z.IsDeleted == false),
                    JobsInApproval = x.Count(z => z.IsDeleted == false && z.IsActive == true && z.IsPublished == false && z.IsRejected == false)
                });
                if (!string.IsNullOrEmpty(name))
                {
                    result = result.Where(x => x.Company.ToString().Contains(name.ToLower().Trim()));
                }
                list = result.ToList<object>();

                //var iResult = dataHelper.Get<List>().Where(x => x.Name.Equals("State") && x.ParentList.Value == countryId);

                //jobsByStateList = iResult.Select(x => new { CountryId = x.Id, Country = x.Text, Code = x.Value, Jobs = x.JobCountries.Count(), JobsInApproval = x.JobCountries.Count(z => z.IsDeleted == false && z.IsActive == true && z.IsPublished == false && z.IsRejected == false) }).Where(x => x.Jobs > 0).ToList<object>();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJobsByState(long countryId)
        {
            List<object> list = new List<object>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var iResult = dataHelper.Get<List>().Where(x => x.Name.Equals("State") && x.ParentList == countryId);

                list = iResult.Select(x => new { StateId = x.Id, Name = x.Text, Code = x.Value, Jobs = x.JobsStates.Count(), JobsInApproval = x.JobsStates.Count(z => z.IsDeleted == false && z.IsActive == true && z.IsPublished == false && z.IsRejected == false) }).Where(x => x.Jobs > 0).ToList<object>();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJobCounts(long? countryId)
        {
           UserInfoEntity uinfo = _service.Get(User.Id);
            if (uinfo.Username == "tasnim@joblisting.com" || uinfo.Username == "lakshmii@joblisting.com")
            {
                countryId = 106;
            }
             else if (uinfo.Username == "maitrantn@joblisting.com" || uinfo.Username == "Linhv@joblisting.com")
            {
                countryId = 3343;
            }
            else if (uinfo.Username == "sandhyam@joblisting.com" || uinfo.Username == "Tanyam@joblisting.com")
            {
                countryId = 139;
            }
            else if (uinfo.Username == "sandhyas@joblisting.com" || uinfo.Username == "balus@joblisting.com")
            {
                countryId = 205;
            }
            else if (uinfo.Username == "adminIndo@joblisting.com" || uinfo.Username == "RecruiterIndo@joblisting.com")
            {
                countryId = 107;
            }
            else if (uinfo.Username == "adminPhil@joblisting.com" || uinfo.Username == "RecruiterPhil@joblisting.com")
            {
                countryId = 180;
            }

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg"


)
            {
                countryId = 40;
            }
            CountEntity entity = (new PortalDataService()).CountJobs(countryId);

            Hashtable data = new Hashtable();
            data.Add("Today", entity.Todays);
            data.Add("7 Days", entity.SevenDays);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CountJobs(int? countryId)
        {
            Hashtable data = new Hashtable();
            int todays = 0;
            int sevenDays = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                if (countryId == null)
                {
                    todays = dataHelper.Get<Job>().Count(x => (x.DateCreated.Day == DateTime.Now.Day
                        && x.DateCreated.Month == DateTime.Now.Month
                        && x.DateCreated.Year == DateTime.Now.Year));

                    DateTime date = DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0));

                    sevenDays = dataHelper.Get<Job>().
                        Count(x => (x.DateCreated.Day >= date.Day
                            && x.DateCreated.Month >= date.Month
                            && x.DateCreated.Year >= date.Year)
                        && (x.DateCreated.Day <= DateTime.Now.Day
                        && x.DateCreated.Month <= DateTime.Now.Month
                        && x.DateCreated.Year <= DateTime.Now.Year));
                }
                else
                {
                    todays = dataHelper.Get<Job>().Count(x => x.CountryId == countryId.Value && (x.DateCreated.Day == DateTime.Now.Day
                       && x.DateCreated.Month == DateTime.Now.Month
                       && x.DateCreated.Year == DateTime.Now.Year));

                    DateTime date = DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0));

                    sevenDays = dataHelper.Get<Job>().
                        Count(x => x.CountryId == countryId.Value && (x.DateCreated.Day >= date.Day
                            && x.DateCreated.Month >= date.Month
                            && x.DateCreated.Year >= date.Year)
                        && (x.DateCreated.Day <= DateTime.Now.Day
                        && x.DateCreated.Month <= DateTime.Now.Month
                        && x.DateCreated.Year <= DateTime.Now.Year));
                }
                data.Add("Today", todays);
                data.Add("7 Days", sevenDays);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCountriesCountHtml()
        {
            long data = (new PortalDataService()).CountCountries();
            int countes = Convert.ToInt32(data);

            return Json(countes, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCompaniesCountHtml()
        {
          long data = (new PortalDataService()).CountCompanies();
            int countes = Convert.ToInt32(data);

            return Json(countes, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInstitutesCountHtml()
        {
            long data = (new PortalDataService()).CountInstitutes();
            int countes = Convert.ToInt32(data);

            return Json(countes, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetJobSeekersCountHtml()
        {
            long data = (new PortalDataService()).CountJobSeekers();
            int countes = Convert.ToInt32(data);

            return Json(countes, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRAgenciesCountHtml()
        {
            long data = (new PortalDataService()).CountRAgencies();
            int countes = Convert.ToInt32(data);

            return Json(countes, JsonRequestBehavior.AllowGet);
        }
        //CountWebListkrishna

             public ActionResult GetCountWebListkrishnaCountHtml()
        {
            long data = (new PortalDataService()).CountWebListkrishna();
            int countes = Convert.ToInt32(data);

            return Json(countes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCountWebListPoojithaCountHtml()
        {
            long data = (new PortalDataService()).CountWebListPoojitha();
            int countes = Convert.ToInt32(data);

            return Json(countes, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCountWebListPRASANNACountHtml()
        {
            long data = (new PortalDataService()).CountWebListPRASANNA();
            int countes = Convert.ToInt32(data);

            return Json(countes, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCountWebListprudhviCountHtml()
        {
            long data = (new PortalDataService()).CountWebListprudhvi();
            int countes = Convert.ToInt32(data);

            return Json(countes, JsonRequestBehavior.AllowGet); 
        }
        public ActionResult GetCountWebListHARSHASREECountHtml()
        {
            long data = (new PortalDataService()).CountWebListHARSHASREE();
            int countes = Convert.ToInt32(data);

            return Json(countes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStudentCountHtml()
        {
            long data = (new PortalDataService()).CountStudent();
            int countes = Convert.ToInt32(data);

            return Json(countes, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUserCounts(long? countryId, int? typeId)
        {
            UserInfoEntity uinfo = _service.Get(User.Id);
            if (uinfo.Username == "tasnim@joblisting.com" || uinfo.Username == "lakshmii@joblisting.com")
            {
                countryId = 106;
            }
             else if (uinfo.Username == "maitrantn@joblisting.com" || uinfo.Username == "Linhv@joblisting.com")
            {
                countryId = 3343;
            }
            else if (uinfo.Username == "sandhyam@joblisting.com" || uinfo.Username == "Tanyam@joblisting.com")
            {
                countryId = 139;
            }
            else if (uinfo.Username == "sandhyas@joblisting.com" || uinfo.Username == "balus@joblisting.com")
            {
                countryId = 205;
            }
            else if (uinfo.Username == "adminIndo@joblisting.com" || uinfo.Username == "RecruiterIndo@joblisting.com")
            {
                countryId = 107;
            }
            else if (uinfo.Username == "adminPhil@joblisting.com" || uinfo.Username == "RecruiterPhil@joblisting.com")
            {
                countryId = 180;
            }

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg"


)
            {
                countryId = 40;
            }

            Hashtable data = new Hashtable();
            int? utype = null;
            if (countryId == null)
            {
                CountEntity entity = (new PortalDataService()).CountUsers(countryId, utype);
                data.Add("Today", entity.Todays);
                data.Add("7 Days", entity.SevenDays);
            }
            else
            {
                if (typeId == null)
                {
                    CountEntity entity = (new PortalDataService()).CountUsers(countryId, utype);
                    data.Add("Today", entity.Todays);
                    data.Add("7 Days", entity.SevenDays);
                }
                else
                {
                    if (typeId.Value == 1)
                    {
                        utype = 4;
                        CountEntity entity = (new PortalDataService()).CountUsers(countryId, utype);
                        data.Add("Today", entity.Todays);
                        data.Add("7 Days", entity.SevenDays);
                    }
                    else if (typeId.Value == 3)
                    {
                        utype = 5;
                        CountEntity entity = (new PortalDataService()).CountUsers(countryId, utype);
                        data.Add("Today", entity.Todays);
                        data.Add("7 Days", entity.SevenDays);
                    }
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Validate()
        {
            UserProfile UserInfo = MemberService.Instance.Get(User.Username);
            bool flag = true;
            if (UserInfo != null)
            {
                flag = ValidatorHelper.IsValidEmail(UserInfo.Username);
                if (!flag)
                {
                    UserInfo.IsValidUsername = false;
                    MemberService.Instance.Update(UserInfo);
                }
            }
            return Json(flag);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ValidateEmail(string email)
        {
            bool flag = true;
            if (!string.IsNullOrEmpty(email))
            {
                flag = ValidatorHelper.IsValidEmail(email);
            }
            return Json(flag);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult VerifySendEmail(string email)
        {
            string[] receipent = { email };
            var reader = new StreamReader(Server.MapPath("~/Templates/Mail/verify_email.html"));
            var body = string.Empty;

            body = reader.ReadToEnd();

            var subject = "Thanks";
            AlertService.Instance.SendMail(subject, receipent, body);

            return Json("Sent");
        }

        [Authorize]
        [System.Web.Mvc.HttpPost]
        public ActionResult ImportContacts(List<Contact> data)
        {
            string message = string.Empty;
            UserProfile profile = MemberService.Instance.Get(User.Id);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                if (data != null && profile != null)
                {
                    foreach (var item in data)
                    {
                        if (profile != null && !profile.Username.ToLower().Equals(item.Email.ToLower()))
                        {
                            Connection contact = dataHelper.Get<Connection>().SingleOrDefault(x => x.UserId == profile.UserId && x.EmailAddress.ToLower().Equals(item.Email.ToLower()));
                            if (contact == null)
                            {
                                UserProfile registered = MemberService.Instance.Get(item.Email.ToLower());

                                string firstName = string.Empty;
                                string lastName = string.Empty;

                                if (registered != null)
                                {
                                    firstName = registered.FirstName;
                                    lastName = registered.LastName;
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(item.FirstName))
                                    {
                                        firstName = null;
                                    }
                                    else
                                    {
                                        firstName = item.FirstName.TitleCase();
                                    }
                                    if (string.IsNullOrEmpty(item.LastName))
                                    {
                                        lastName = null;
                                    }
                                    else
                                    {
                                        lastName = item.LastName.TitleCase();
                                    }
                                }

                                contact = new Connection()
                                {
                                    UserId = profile.UserId,
                                    FirstName = firstName,
                                    LastName = lastName,
                                    EmailAddress = item.Email.ToLower(),
                                    Initiated = false,
                                    IsDeleted = false,
                                    IsAccepted = false,
                                    IsConnected = false,
                                    IsBlocked = false,
                                    Phone = item.Phone,
                                    Sent = false,
                                    DateSent = null,
                                };

                                dataHelper.Add<Connection>(contact, User.Username);
                            }
                            else
                            {
                                if (contact.IsDeleted)
                                {
                                    UserProfile registered = MemberService.Instance.Get(item.Email.ToLower());

                                    string firstName = string.Empty;
                                    string lastName = string.Empty;

                                    if (registered != null)
                                    {
                                        firstName = registered.FirstName;
                                        lastName = registered.LastName;
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(item.FirstName))
                                        {
                                            firstName = null;
                                        }
                                        else
                                        {
                                            firstName = item.FirstName.TitleCase();
                                        }
                                        if (string.IsNullOrEmpty(item.LastName))
                                        {
                                            lastName = null;
                                        }
                                        else
                                        {
                                            lastName = item.LastName.TitleCase();
                                        }
                                    }

                                    contact.FirstName = firstName;
                                    contact.LastName = lastName;
                                    contact.EmailAddress = item.Email.ToLower();
                                    contact.Initiated = false;
                                    contact.IsDeleted = false;
                                    contact.IsAccepted = false;
                                    contact.IsConnected = false;
                                    contact.IsBlocked = false;
                                    contact.Phone = item.Phone;
                                    contact.Sent = false;
                                    contact.DateSent = null;
                                    dataHelper.Update<Connection>(contact, User.Username);
                                }
                            }
                        }
                    }
                    message = "Contact Imported Successfully!";
                }
                else
                {
                    message = "No Data Found";
                }
            }
            TempData["UpdateData"] = message;
            return Json(message);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> SearchPeople(string name = null, int? ageFrom = null, int? ageTo = null, string gender = null, int? relation = null, bool showconnect = true, int pageNumber = 1)
        {
            StringBuilder sbHtml = new StringBuilder();
            int pageSize = 6;
            JsonDataModel jModel = new JsonDataModel();
            if (User != null)
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);

                    var result = dataHelper.Get<UserProfile>().Where(x => x.IsActive == true && x.IsDeleted == false && x.UserId != profile.UserId);
                    List<Connection> connections = profile.Connections.ToList();

                    List<long> blockList = dataHelper.Get<BlockedPeople>().Where(x => x.BlockedId == profile.UserId).Select(x => x.BlockerId).ToList();
                    int individual = (int)SecurityRoles.Jobseeker;
                    int company = (int)SecurityRoles.Employers;
                    List<int> types = new List<int>() { individual, company };
                    SearchResume filter = new SearchResume()
                    {
                        Name = string.IsNullOrEmpty(name) ? null : name,
                        AgeMin = ageFrom,
                        AgeMax = ageTo,
                        Gender = string.IsNullOrEmpty(gender) ? null : gender,
                        Relationship = relation,
                        Username = User.Username,
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    };

                    List<PeopleEntity> list = await _service.SearchDirectory(filter);

                    if (list.Count > 0)
                    {
                        jModel.Rows = list.First().MaxRows.Value;
                    }
                    foreach (PeopleEntity item in list)
                    {
                        StringBuilder sbLinks = new StringBuilder();

                        sbHtml.Append("<div class=\"list-group-item\" style=\"margin-bottom:15px; margin-left:0px; margin-right:0px; margin-top:0px;\">");
                        sbHtml.Append("<div class=\"row\"><div class=\"col-lg-2 col-md-2 col-sm-2 col-xs-2\" style=\"padding-left: 5px;\">");
                        sbHtml.AppendFormat("<a href=\"/{0}\" target=\"_blank\"><div style=\"width:48px; height:48px; background:url('/Image/Avtar?Id={1}&height=48&width=48') no-repeat; background-size:contain; background-position:top center\"></div></a>", item.PermaLink, item.UserId);
                        sbHtml.Append("</div><div class=\"col-lg-10 col-md-10 col-sm-10 col-xs-10\">");
                        sbHtml.AppendFormat("<div class='form-group'><a href=\"/{0}\" target=\"_blank\">{1}</a><br/>{2}<br/>{3}</div></div></div>", item.PermaLink, item.FullName, item.Country, string.IsNullOrEmpty(item.Title) ? "&nbsp;" : item.Title);

                        Connection connection = ConnectionHelper.Get(item.Username, profile.Username);
                        if (connection != null && connection.IsDeleted == false && connection.IsAccepted == true && connection.IsConnected == true)
                        {
                            sbLinks.AppendFormat("<a href=\"#\" data-aurl=\"{0}\" role=\"button\" data-toggle=\"modal\" data-target=\"#discDialog\" class=\"btn btn-info discon\" style=\"height: 20px; width:70px; font-size:8pt; padding:3px;\">Disconnect</a></b>&nbsp;", Url.Action("Disconnect", "Network", new { Id = connection.Id, redirect = "/Network/Index" }));
                            sbLinks.AppendFormat("<a href=\"/Message/List?SenderId={0}&redirect=/Network/Index\" target=\"_blank\" class=\"btn btn-info\" style=\"height: 20px; width:70px; font-size:8pt; padding:3px;\" >Message</a>&nbsp;", item.UserId);

                            string bname = item.FullName;

                            bool flag = ConnectionHelper.IsConnected(item.Username, profile.Username);
                            sbLinks.AppendFormat("<a href=\"#\" data-href=\"/Home/Block?email={0}\" data-name=\"{1}\" data-role=\"{2}\"  data-connected=\"{3}\" role=\"button\" data-toggle=\"modal\" data-target=\"#cDialog\" class=\"btn btn-info aBlock\" style=\"height: 20px; width:60px; font-size:8pt; padding:3px;\">Block</a>", item.Username, bname, ((SecurityRoles)item.Type).GetDescription(), flag);
                            if (item.IsJobseeker)
                            {
                                sbLinks.AppendFormat("&nbsp;<a href=\"/Employer/BookmarkJobseeker?Id={0}&redirect=/Employer/Bookmarks\" target=\"_blank\" class=\"btn btn-info\" style=\"height: 20px; width:60px; font-size:8pt; padding:3px;\">Bookmark</a>", item.UserId);
                            }
                        }
                        else if (connection != null && connection.IsDeleted == false && connection.IsAccepted == false && connection.IsConnected == false)
                        {
                            if (connection.CreatedBy.Equals(User.Username))
                            {
                                sbLinks.AppendFormat("<a href=\"/Message/List?SenderId={0}&redirect=/Network/Index\" target=\"_blank\" class=\"btn btn-info\" style=\"height: 20px; width:70px; font-size:8pt; padding:3px;\" >Message</a>&nbsp;", item.UserId);
                                sbLinks.AppendFormat("<a href=\"#\" data-aurl=\"{0}\" role=\"button\" data-toggle=\"modal\" data-target=\"#discDialog\" class=\"btn btn-info discon\" style=\"height: 20px; width:70px; font-size:8pt; padding:3px;\">Disconnect</a></b>&nbsp;", Url.Action("Disconnect", "Network", new { Id = connection.Id, redirect = "/Network/Index" }));
                            }
                            else
                            {
                                sbLinks.AppendFormat("<a href=\"/Message/List?SenderId={0}&redirect=/Network/Index\" target=\"_blank\" class=\"btn btn-info\" style=\"height: 20px; width:70px; font-size:8pt; padding:3px;\" >Message</a>&nbsp;", item.UserId);
                                sbLinks.AppendFormat("<a href=\"/Network/Accept?Id={0}&redirect=/Network/Index\" class=\"btn btn-info\" style=\"height: 20px; width:70px; font-size:8pt; padding:3px;\" >Accept</a>&nbsp;", connection.Id);
                                sbLinks.AppendFormat("<a href=\"#\" data-aurl=\"{0}\" role=\"button\" data-toggle=\"modal\" data-target=\"#discDialog\" class=\"btn btn-info discon\" style=\"height: 20px; width:70px; font-size:8pt; padding:3px;\">Disconnect</a></b>&nbsp;", Url.Action("Disconnect", "Network", new { Id = connection.Id, redirect = "/Network/Index" }));
                            }
                        }
                        else
                        {
                            BlockedPeople blocked = ConnectionHelper.GetBlockedEntity(item.UserId, profile.UserId);
                            if (blocked != null && blocked.CreatedBy.Equals(profile.Username))
                            {
                                sbLinks.AppendFormat("<a href=\"/Home/Unblock?id={0}&redirect=/Network/Index\" class=\"btn btn-info\" style=\"height: 20px; width:60px; font-size:8pt; padding:3px;\">Unblock</a>", item.UserId);
                            }
                            else
                            {
                                string bname = item.FullName;
                                if (showconnect)
                                {
                                    sbLinks.AppendFormat("<a href=\"/home/connect?id={0}&redirect=/Network/Index\" class=\"btn btn-info\" style=\"height: 20px; width:60px; font-size:8pt; padding:3px;\" >Connect</a>&nbsp;", item.UserId);
                                }
                                sbLinks.AppendFormat("<a href=\"/Message/List?SenderId={0}&redirect=/Network/Index\" target=\"_blank\" class=\"btn btn-info\" style=\"height: 20px; width:70px; font-size:8pt; padding:3px;\" >Message</a>&nbsp;", item.UserId);
                                bool flag = ConnectionHelper.IsConnected(item.Username, profile.Username);
                                sbLinks.AppendFormat("<a href=\"#\" data-href=\"/Home/Block?email={0}\" data-name=\"{1}\" data-role=\"{2}\"  data-connected=\"{3}\" role=\"button\" data-toggle=\"modal\" data-target=\"#cDialog\" class=\"btn btn-info aBlock\" style=\"height: 20px; width:60px; font-size:8pt; padding:3px;\">Block</a>", item.Username, bname, ((SecurityRoles)item.Type).GetDescription(), flag);
                                if (item.IsJobseeker)
                                {
                                    sbLinks.AppendFormat("&nbsp;<a href=\"/Employer/BookmarkJobseeker?Id={0}&redirect=/Employer/Bookmarks\" target=\"_blank\" class=\"btn btn-info\" style=\"height: 20px; width:60px; font-size:8pt; padding:3px;\">Bookmark</a>", item.UserId);
                                }
                            }
                        }
                        sbHtml.AppendFormat("<div class=\"row\"><div class=\"col-lg-12 col-md-12 col-sm-12 text-right\" style=\"padding-top:10px;\">{0}</div></div></div>", sbLinks.ToString());
                    }
                }
            }
            jModel.Data = sbHtml.ToString();
            return Json(jModel);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult SearchIndividuals(string name = "", int? ageFrom = null, int? ageTo = null, string gender = "", int? relation = null, bool showconnect = true, int pageNumber = 1)
        {
            StringBuilder sbHtml = new StringBuilder();
            int pageSize = 6;
            JsonDataModel jModel = new JsonDataModel();

            if (User != null)
            {
                UserProfile UserInfo = MemberService.Instance.Get(User.Username);
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                    var result = dataHelper.Get<UserProfile>().Where(x => x.IsActive == true && x.IsDeleted == false && x.UserId != profile.UserId);
                    int individual = (int)SecurityRoles.Jobseeker;
                    int company = (int)SecurityRoles.Employers;
                    List<int> types = new List<int>() { individual, company };
                    List<Connection> connections = profile.Connections.ToList();
                    List<long> blockList = dataHelper.Get<BlockedPeople>().Where(x => x.BlockedId == profile.UserId).Select(x => x.BlockerId).ToList();
                    if (blockList.Count > 0)
                    {
                        result = result.Where(x => !blockList.Contains(x.UserId));
                    }

                    if (!string.IsNullOrEmpty(name))
                    {
                        if (name.Split(' ').Length > 0)
                        {
                            string[] names = name.ToLower().Split(' ');
                            result = result.Where(x => types.Contains(x.Type) && names.Any(z => (x.Company != null ? x.Company : x.FirstName + " " + x.LastName).ToLower().Contains(z)));
                        }
                        else
                        {
                            result = result.Where(x => types.Contains(x.Type) && (x.Company != null ? x.Company : x.FirstName + " " + x.LastName).ToLower().Contains(name.ToLower()));
                        }
                    }

                    if (ageFrom != null && ageTo != null)
                    {
                        result = result.Where(x => x.Age >= ageFrom && x.Age <= ageTo);
                    }

                    if (!string.IsNullOrEmpty(gender))
                    {
                        result = result.Where(x => x.Gender.Equals(gender));
                    }

                    if (relation != null)
                    {
                        result = result.Where(x => x.RelationshipStatus == relation);
                    }

                    jModel.Rows = result.Count();
                    if (result.Count() > 0)
                    {
                        result = result.OrderByDescending(x => x.DateCreated).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : (pageNumber * pageSize))).Take(pageSize);
                    }

                    foreach (UserProfile item in result)
                    {
                        StringBuilder sbLinks = new StringBuilder();

                        sbHtml.Append("<div class=\"list-group-item\" style=\"margin-bottom:15px; margin-left:0px; margin-right:0px; margin-top:0px;\">");
                        sbHtml.Append("<div class=\"row\"><div class=\"col-lg-2 col-md-2 col-sm-2 col-xs-2\" style=\"padding-left: 5px;\">");
                        sbHtml.AppendFormat("<a href=\"/{0}\" target=\"_blank\"><div style=\"width:48px; height:48px; background:url('/Image/Avtar?Id={1}&height=48&width=48') no-repeat; background-size:contain; background-position:top center\"></div></a>", item.PermaLink, item.UserId);
                        sbHtml.Append("</div><div class=\"col-lg-10 col-md-10 col-sm-10 col-xs-10\"><div class='form-group'>");
                        if (item.Type == (int)SecurityRoles.Jobseeker)
                        {
                            sbHtml.AppendFormat("<a href=\"/{0}\" target=\"_blank\">{1}&nbsp;{2}</a><br/>{3}<br/>{4}, Age: {5}, {6}</div></div></div>", item.PermaLink, item.FirstName, item.LastName, (item.CountryId != null ? item.Country.Text : ""), (!string.IsNullOrEmpty(item.Gender) ? item.Gender : "--"), (item.Age != null ? Convert.ToString(item.Age) : "--"), (item.RelationshipStatus != null ? ((Relationships)item.RelationshipStatus).GetDescription().TitleCase() : ""));
                        }
                        else
                        {
                            sbHtml.AppendFormat("<a href=\"/{0}\" target=\"_blank\">{1}&nbsp;{2}</a><br/>{3}<br/>Company</div></div></div>", item.PermaLink, item.FirstName, item.LastName, (item.CountryId != null ? item.Country.Text : ""));
                        }

                        //Connection connection = connections.Where(x => x.EmailAddress.Equals(item.Username)).SingleOrDefault();
                        Connection connection = ConnectionHelper.Get(item.Username, profile.Username);
                        if (connection != null && connection.IsDeleted == false && connection.IsAccepted == true && connection.IsConnected == true)
                        {
                            sbLinks.AppendFormat("<a href=\"#\" data-aurl=\"{0}\" role=\"button\" data-toggle=\"modal\" data-target=\"#discDialog\" class=\"btn btn-info discon\" style=\"height: 20px; width:70px; font-size:8pt; padding:3px;\">Disconnect</a></b>&nbsp;", Url.Action("Disconnect", "Network", new { Id = connection.Id, redirect = "/Network/Index" }));
                            sbLinks.AppendFormat("<a href=\"/Message/List?SenderId={0}&redirect=/Network/Index\" target=\"_blank\" class=\"btn btn-info\" style=\"height: 20px; width:70px; font-size:8pt; padding:3px;\" >Message</a>&nbsp;", item.UserId);

                            string bname = string.Empty;
                            if (item.Type == (int)SecurityRoles.Employers)
                            {
                                if (!string.IsNullOrEmpty(item.Company))
                                {
                                    bname = item.Company;
                                }
                                else
                                {
                                    bname = string.Format("{0} {1}", item.FirstName, item.LastName);
                                }
                            }
                            else
                            {
                                bname = string.Format("{0} {1}", item.FirstName, item.LastName);
                            }
                            bool flag = ConnectionHelper.IsConnected(item.Username, UserInfo.Username);
                            sbLinks.AppendFormat("<a href=\"#\" data-href=\"/Home/Block?email={0}\" data-name=\"{1}\" data-role=\"{2}\"  data-connected=\"{3}\" role=\"button\" data-toggle=\"modal\" data-target=\"#cDialog\" class=\"btn btn-info aBlock\" style=\"height: 20px; width:60px; font-size:8pt; padding:3px;\">Block</a>", item.Username, bname, ((SecurityRoles)item.Type).GetDescription(), flag);
                        }
                        else if (connection != null && connection.IsDeleted == false && connection.IsAccepted == false && connection.IsConnected == false)
                        {
                            if (connection.CreatedBy.Equals(User.Username))
                            {
                                sbLinks.AppendFormat("<a href=\"/Message/List?SenderId={0}&redirect=/Network/Index\" target=\"_blank\" class=\"btn btn-info\" style=\"height: 20px; width:70px; font-size:8pt; padding:3px;\" >Message</a>&nbsp;", item.UserId);
                                sbLinks.AppendFormat("<a href=\"#\" data-aurl=\"{0}\" role=\"button\" data-toggle=\"modal\" data-target=\"#discDialog\" class=\"btn btn-info discon\" style=\"height: 20px; width:70px; font-size:8pt; padding:3px;\">Disconnect</a></b>&nbsp;", Url.Action("Disconnect", "Network", new { Id = connection.Id, redirect = "/Network/Index" }));
                            }
                            else
                            {
                                sbLinks.AppendFormat("<a href=\"/Message/List?SenderId={0}&redirect=/Network/Index\" target=\"_blank\" class=\"btn btn-info\" style=\"height: 20px; width:70px; font-size:8pt; padding:3px;\" >Message</a>&nbsp;", item.UserId);
                                sbLinks.AppendFormat("<a href=\"/Network/Accept?Id={0}&redirect=/Network/Index\" class=\"btn btn-info\" style=\"height: 20px; width:70px; font-size:8pt; padding:3px;\" >Accept</a>&nbsp;", connection.Id);
                                sbLinks.AppendFormat("<a href=\"#\" data-aurl=\"{0}\" role=\"button\" data-toggle=\"modal\" data-target=\"#discDialog\" class=\"btn btn-info discon\" style=\"height: 20px; width:70px; font-size:8pt; padding:3px;\">Disconnect</a></b>&nbsp;", Url.Action("Disconnect", "Network", new { Id = connection.Id, redirect = "/Network/Index" }));
                            }
                        }
                        else
                        {
                            BlockedPeople blocked = ConnectionHelper.GetBlockedEntity(item.UserId, profile.UserId);
                            if (blocked != null && blocked.CreatedBy.Equals(profile.Username))
                            {
                                sbLinks.AppendFormat("<a href=\"/Home/Unblock?id={0}&redirect=/Network/Index\" class=\"btn btn-info\" style=\"height: 20px; width:60px; font-size:8pt; padding:3px;\">Unblock</a>", item.UserId);
                            }
                            else
                            {
                                string bname = string.Empty;
                                if (item.Type == (int)SecurityRoles.Employers)
                                {
                                    if (!string.IsNullOrEmpty(item.Company))
                                    {
                                        bname = item.Company;
                                    }
                                    else
                                    {
                                        bname = string.Format("{0} {1}", item.FirstName, item.LastName);
                                    }
                                }
                                else
                                {
                                    bname = string.Format("{0} {1}", item.FirstName, item.LastName);
                                }
                                if (showconnect)
                                {
                                    sbLinks.AppendFormat("<a href=\"/home/connect?id={0}&redirect=/Network/Index\" class=\"btn btn-info\" style=\"height: 20px; width:60px; font-size:8pt; padding:3px;\" >Connect</a>&nbsp;", item.UserId);
                                }
                                sbLinks.AppendFormat("<a href=\"/Message/List?SenderId={0}&redirect=/Network/Index\" target=\"_blank\" class=\"btn btn-info\" style=\"height: 20px; width:70px; font-size:8pt; padding:3px;\" >Message</a>&nbsp;", item.UserId);
                                sbLinks.AppendFormat("<a href=\"#\" data-href=\"/Home/Block?email={0}\" data-name=\"{1}\" data-role=\"{2}\" role=\"button\" data-toggle=\"modal\" data-target=\"#cDialog\" class=\"btn btn-info aBlock\" style=\"height: 20px; width:60px; font-size:8pt; padding:3px;\">Block</a>", item.Username, bname, ((SecurityRoles)item.Type).GetDescription());
                            }
                        }
                        sbHtml.AppendFormat("<div class=\"row\"><div class=\"col-lg-12 col-md-12 col-sm-12 text-right\" style=\"padding-top:10px;\">{0}</div></div></div>", sbLinks.ToString());
                    }
                }
            }

            jModel.Data = sbHtml.ToString();
            return Json(jModel);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetDialingCode(long? countryId)
        {
            if (Request.IsAjaxRequest())
            {
                if (countryId != null)
                {
                    List country = SharedService.Instance.GetCountry(countryId.Value);
                    return Json(string.Format("+{0}", country.Code), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            return null;
        }


        [System.Web.Mvc.HttpPost]
        public async Task<JsonResult> GetCountryList()
        {
            List<Country> countryList = await helper.CountryList();
            return Json(new SelectList(countryList, "Id", "Name"), JsonRequestBehavior.AllowGet);
        }

        //[System.Web.Mvc.HttpPost]
        //public JsonResult GetCountryList()
        //{
        //    //if (Request.IsAjaxRequest())
        //    //{
        //    List<List> countryList = SharedService.Instance.GetCountryList();
        //    return Json(new SelectList(countryList, "Id", "Text"), JsonRequestBehavior.AllowGet);
        //    //}
        //    //return null;
        //}

        [System.Web.Mvc.HttpPost]
        public ActionResult GetContactList()
        {
            List<NetworkContactModel> contactList = new List<NetworkContactModel>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                contactList = dataHelper.GetList<Connection>().Where(x => x.UserId == profile.UserId).Select(x => new NetworkContactModel() { Id = x.Id, Name = x.FirstName + " " + x.LastName, EmailAddress = x.EmailAddress, Connected = x.IsConnected, IsValid = x.IsAccepted }).ToList();
            }
            return Json(contactList);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetJob(long jobid)
        {
            var job = new Job();
            if (Request.IsAjaxRequest())
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    job = dataHelper.GetSingle<Job>(jobid);
                }
                return Json(job);

            }
            return null;
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetAgeList(int? age)
        {
            if (Request.IsAjaxRequest())
            {
                if (age != null)
                {
                    if (age == 18)
                    {
                        return Json(new SelectList(SharedService.Instance.GetAgeList(20), "value", "key"),
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new SelectList(SharedService.Instance.GetAgeList(age.Value + 5), "value", "key"),
                        JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetExperienceList(int? exp)
        {
            int e = 0;
            if (Request.IsAjaxRequest())
            {
                if (exp != null)
                {
                    e = exp.Value;
                }
                if (e == 0)
                {
                    return Json(new SelectList(SharedService.Instance.GetExperienceList(e), "value", "key"),
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new SelectList(SharedService.Instance.GetExperienceList(e + 1), "value", "key"),
                    JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            JobPortalEntities entities = new JobPortalEntities();
            var customers = (from customer in entities.User_Skills
                             where customer.SkillName.StartsWith(prefix)
                             select new
                             {
                                 label = customer.SkillName,
                                 val = customer.UserId
                             }).ToList();

            return Json(customers);
        }

        [HttpPost]
        public JsonResult GetNamesd(int? categoryid, int? type, string Prefix)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                //spamWord = dataHelper.Get<UserProfile>().SingleOrDefault<UserProfile>(x => x.Word.ToLower().Equals(word.ToLower().Trim()));

                if (categoryid != null && type != null)
                {
                    if (Request.IsAjaxRequest())
                    {
                        if (type == 4)
                        {
                            return Json(new SelectList(SharedService.Instance.GetNames1(categoryid.Value, type.Value, Prefix), "UserId", "FirstName"),
                                JsonRequestBehavior.AllowGet);
                        }
                        else if (type == 5)
                        {
                            return Json(new SelectList(SharedService.Instance.GetNames1(categoryid.Value, type.Value, Prefix), "UserId", "Company"),
                                JsonRequestBehavior.AllowGet);
                        }
                        else if (type == 12)
                        {
                            return Json(new SelectList(SharedService.Instance.GetNames1(categoryid.Value, type.Value, Prefix), "UserId", "University"),
                                JsonRequestBehavior.AllowGet);
                        }
                        else if (type == 13)
                        {
                            return Json(new SelectList(SharedService.Instance.GetNames1(categoryid.Value, type.Value, Prefix), "UserId", "FirstName"),
                                JsonRequestBehavior.AllowGet);
                        }

                    }
                }

            }
            return null;
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetName(int? categoryid,int? type)
        {
            if (categoryid != null && type != null)
            {
                if (Request.IsAjaxRequest())
                {
                    if (type == 4)
                    {
                        return Json(new SelectList(SharedService.Instance.GetNames(categoryid.Value, type.Value), "UserId", "FirstName"),
                            JsonRequestBehavior.AllowGet);
                    }
                    else if (type == 5)
                    {
                        return Json(new SelectList(SharedService.Instance.GetNames(categoryid.Value, type.Value), "UserId", "Company"),
                            JsonRequestBehavior.AllowGet);
                    }
                    else if (type == 12)
                    {
                        return Json(new SelectList(SharedService.Instance.GetNames(categoryid.Value, type.Value), "UserId", "University"),
                            JsonRequestBehavior.AllowGet);
                    }
                    else if (type == 13)
                    {
                        return Json(new SelectList(SharedService.Instance.GetNames(categoryid.Value, type.Value), "UserId", "FirstName"),
                            JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return null;
        }
 
        

        [System.Web.Mvc.HttpPost]
        public JsonResult GetSpecializations(int? categoryid)
        {
            if (categoryid != null)
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new SelectList(SharedService.Instance.GetSubSpecialisations(categoryid.Value), "Id", "Name"),
                        JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetSpecializationContents(int categoryid)
        {
            if (Request.IsAjaxRequest())
            {
                return Json((SharedService.Instance.GetContentSpecialization(categoryid)), JsonRequestBehavior.AllowGet);
            }
            return null;
        }


        public async Task<JsonResult> GetStates(int countryid = 0)
        {
            if (Request.IsAjaxRequest())
            {
                List<State> list = await helper.StateList(countryid);
                if (list != null)
                {
                    return Json(new SelectList(list, "Id", "Name"),
                        JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }


        //public JsonResult GetStates(int countryid = 0)
        //{
        //    if (Request.IsAjaxRequest())
        //    {
        //        return Json(new SelectList(SharedService.Instance.GetStatesById(countryid), "Id", "Text"),
        //            JsonRequestBehavior.AllowGet);
        //    }
        //    return null;
        //}

        public JsonResult chkoldpasswordStatus(string stroldpwd, string strnewpwd)
        {
            var hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Username));
            var changePasswordSucceeded = false;
            if (hasLocalAccount)
            {
                // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                try
                {
                    changePasswordSucceeded = WebSecurity.ChangePassword(User.Username, stroldpwd, strnewpwd);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }
            }
            return Json(changePasswordSucceeded);
        }



        public List<Addressbook> SerializeAddressBook(string data)
        {
            var list_addressbook = new List<Addressbook>();
            var serializer = new JavaScriptSerializer();
            var obj_ad = serializer.Deserialize<deserializer>("{\"data\":" + data + "}");
            var len = ((object[])(obj_ad.data)).Length;
            if (len > 0)
            {
                var dics = new Dictionary<string, object>();
                for (var i = 0; i < len; i++)
                {
                    string firstname = "", lastname = "", day = "", month = "", year = "", imageurl = "", notes = "";
                    var email = new ArrayList();
                    var website = new ArrayList();
                    var phone = new ArrayList();
                    var address = new List<Address>();
                    var birthday = new Birthday();
                    var name = new Name();
                    try
                    {
                        dics = ((Dictionary<string, object>)((object[])(obj_ad.data))[i]);
                    }
                    catch (Exception)
                    {
                        email.Add((((object[])(obj_ad.data))[i]));
                        dics = new Dictionary<string, object>();
                    }
                    foreach (var contacts in dics)
                    {
                        if (contacts.Value != null)
                        {
                            if (contacts.Key == "name")
                            {
                                var dic_contacts = ((Dictionary<string, object>)(contacts.Value));
                                foreach (var nameobj in dic_contacts)
                                {
                                    if (nameobj.Key == "first_name")
                                    {
                                        if (nameobj.Value != null)
                                            firstname = Server.UrlDecode(nameobj.Value.ToString());
                                    }
                                    else if (nameobj.Key == "last_name")
                                    {
                                        if (nameobj.Value != null)
                                            lastname = Server.UrlDecode(nameobj.Value.ToString());
                                    }
                                }
                                name.first_name = firstname;
                                name.last_name = lastname;
                            }
                            else if (contacts.Key == "email")
                            {
                                var emailLen = ((object[])(contacts.Value)).Length;
                                for (var em = 0; em < emailLen; em++)
                                {
                                    email.Add(((object[])(contacts.Value))[em].ToString());
                                }
                            }
                            else if (contacts.Key == "imageurl")
                            {
                                if (contacts.Value != null)
                                    imageurl = Server.UrlDecode(contacts.Value.ToString());
                            }
                            else if (contacts.Key == "notes")
                            {
                                if (contacts.Value != null)
                                    notes = Server.UrlDecode(contacts.Value.ToString());
                            }
                            else if (contacts.Key == "birthday")
                            {
                                var dic_dob = ((Dictionary<string, object>)(contacts.Value));
                                foreach (var dobobj in dic_dob)
                                {
                                    if (dobobj.Value != null && dobobj.Key == "day")
                                        day = dobobj.Value.ToString();
                                    else if (dobobj.Value != null && dobobj.Key == "month")
                                        month = dobobj.Value.ToString();
                                    else if (dobobj.Value != null && dobobj.Key == "year")
                                        year = dobobj.Value.ToString();
                                }
                                birthday.day = day;
                                birthday.month = month;
                                birthday.year = year;
                            }
                            else if (contacts.Key == "website")
                            {
                                var websiteLen = ((object[])(contacts.Value)).Length;
                                for (var we = 0; we < websiteLen; we++)
                                {
                                    website.Add(((object[])(contacts.Value))[we].ToString());
                                }
                            }
                            else if (contacts.Key == "phone")
                            {
                                var phoneLen = ((object[])(contacts.Value)).Length;
                                for (var ph = 0; ph < phoneLen; ph++)
                                {
                                    phone.Add(((object[])(contacts.Value))[ph].ToString());
                                }
                            }
                            else if (contacts.Key == "address")
                            {
                                var addLen = ((object[])(contacts.Value)).Length;
                                for (var ad = 0; ad < addLen; ad++)
                                {
                                    var dic_address = ((Dictionary<string, object>)((object[])(contacts.Value))[ad]);
                                    string street = "", city = "", state = "", zip = "", country = "", formattedaddress = "";
                                    foreach (var address_obj in dic_address)
                                    {
                                        if (address_obj.Key == "street")
                                        {
                                            if (address_obj.Value != null)
                                                street = Server.UrlDecode(address_obj.Value.ToString());
                                        }
                                        else if (address_obj.Key == "city")
                                        {
                                            if (address_obj.Value != null)
                                                city = Server.UrlDecode(address_obj.Value.ToString());
                                        }
                                        else if (address_obj.Key == "state")
                                        {
                                            if (address_obj.Value != null)
                                                state = Server.UrlDecode(address_obj.Value.ToString());
                                        }
                                        else if (address_obj.Key == "zip")
                                        {
                                            if (address_obj.Value != null)
                                                zip = Server.UrlDecode(address_obj.Value.ToString());
                                        }
                                        else if (address_obj.Key == "country")
                                        {
                                            if (address_obj.Value != null)
                                                country = Server.UrlDecode(address_obj.Value.ToString());
                                        }
                                        else if (address_obj.Key == "formattedaddress")
                                        {
                                            if (address_obj.Value != null)
                                                formattedaddress = Server.UrlDecode(address_obj.Value.ToString());
                                        }
                                    }
                                    var addr = new Address();
                                    addr.city = city;
                                    addr.street = street;
                                    addr.state = state;
                                    addr.zip = zip;
                                    addr.country = country;
                                    addr.formattedaddress = formattedaddress;
                                    address.Add(addr);
                                }
                            }
                        }
                    }
                    if (email.Count > 0)
                    {
                        var gc = new Addressbook();
                        gc.address = address;
                        gc.email = email;
                        gc.name = name;
                        gc.phone = phone;
                        gc.birthday = birthday;
                        gc.imageurl = imageurl.Replace("\"", "");
                        gc.website = website;
                        gc.notes = notes;
                        list_addressbook.Add(gc);
                    }
                }
            }
            return list_addressbook;
        }
        public class Addressbook
        {
            public List<Address> address;
            public Birthday birthday;
            public ArrayList email;
            public string imageurl;
            public Name name;
            public string notes;
            public ArrayList phone;
            public ArrayList website;
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult JobTitleList(string find)
        {
            var result = new List<KeyValuePair<string, string>>();
            List<string> jobs = new List<string>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                jobs = dataHelper.Get<Job>().Where(x => x.Title.ToLower().StartsWith(find.ToLower())).Select(x => x.Title).Distinct().Take(15).ToList();
            }

            var idx = 0;
            foreach (string item in jobs)
            {
                result.Add(new KeyValuePair<string, string>(Convert.ToString(idx), item));
                idx++;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserInfo(string email)
        {
            UserInfo userInfo = new UserInfo(email);
            return Json(userInfo, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetResizedPhotoByUsername(string Username, string area, int? height, int? width)
        {
            Photo photo = null;
            if (!string.IsNullOrEmpty(Username))
            {
                UserProfile profile = MemberService.Instance.Get(Username);

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    photo = dataHelper.Get<Photo>().SingleOrDefault(x => x.UserId == profile.UserId && x.IsDeleted == false && ((x.IsApproved == true && x.IsRejected == false && x.InEditMode == false) || (x.IsApproved == false && x.IsRejected == false && x.InEditMode == true)) && x.Area.Equals(area));
                }
            }

            if (photo != null)
            {
                return Json(Convert.ToBase64String(photo.Image), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetResizedPhotoById(long? Id, string area, int? height, int? width)
        {
            Photo photo = null;
            if (Id != null)
            {
                UserProfile profile = MemberService.Instance.Get(Id.Value);
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    photo = dataHelper.Get<Photo>().SingleOrDefault(x => x.UserId == profile.UserId && x.IsDeleted == false && ((x.IsApproved == true && x.IsRejected == false && x.InEditMode == false) || (x.IsApproved == false && x.IsRejected == false && x.InEditMode == true)) && x.Area.Equals(area));
                }
            }
            if (photo != null)
            {
                return Json(Convert.ToBase64String(photo.Image), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        [System.Web.Mvc.HttpPost]
        public JsonResult SkillUpload(string Username, string skill, string val, string per)
        {
            string flag = "Failed";
            decimal per1 = Convert.ToDecimal(per);
            int val1 = Convert.ToInt32(val);
            if (!string.IsNullOrEmpty(Username))
            {
                UserProfile original = MemberService.Instance.Get(Username);

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    var count = dataHelper.Get<User_Skills>().Where(m => m.SkillName == skill && m.UserId == original.UserId);

                    if (count != null)
                    {

                        int i = jobService.UserSkillUpdate(original.UserId, skill, val1, per1, 4);
                        if (i == 1)
                        {
                            flag = "Uploaded successfully!";
                        }
                    }
                    else
                    {
                        int i = jobService.UserSkillUpdateIn(original.UserId, skill, val1, per1, 4);
                        if (i == 1)
                        {
                            flag = "Uploaded successfully!";
                        }
                    }
                }
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        

       
        [System.Web.Mvc.HttpPost]
        public JsonResult ExperienceUpload(string Username, string employer, string frommonth, string tomonth, string fromyr, string toyr, string ind, string role, string resp,string cid,string cat)
        {
            string flag = "Failed";
            int toyr1 = Convert.ToInt32(toyr);
            int fromyr1 = Convert.ToInt32(fromyr);
            int tomonth1 = Convert.ToInt32(tomonth);
            int frommonth1 = Convert.ToInt32(frommonth);

            if (!string.IsNullOrEmpty(Username))
            {
                UserProfile original = MemberService.Instance.Get(Username);

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    User_Experience count = context.User_Experience.Where(m => m.Employer == employer && m.Industry == ind && m.Category == cat && m.Roles == role && m.UserId == original.UserId).FirstOrDefault();

                    //var count = dataHelper.GetAll<User_Experience>().Where(m => m.Employer == employer && m.Industry == ind  && m.Roles == role && m.UserId == original.UserId).FirstOrDefault();

                    if (count == null)
                    {

                        int i = jobService.UserExperienceUpdate(original.UserId, employer, frommonth1, tomonth1, fromyr1, toyr1, ind, role, resp, 4,cid,cat);
                        if (i == 1)
                        {
                            flag = "Uploaded successfully!";
                        }
                    }
                    else
                    {
                        int i = jobService.UserExperienceUpdateIn(original.UserId, employer, frommonth1, tomonth1, fromyr1, toyr1, ind, role, resp, 4,cid,cat);
                        if (i == 1)
                        {
                            flag = "Uploaded successfully!";
                        }
                    }
                }
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }




        [Authorize]
        [System.Web.Mvc.HttpPost]
        public JsonResult EducationUpload(string Username, string education, string fromyr, string toyr, string school, string grade)
        {
            string flag = "Failed";
            int toyr1 = Convert.ToInt32(toyr);
            int fromyr1 = Convert.ToInt32(fromyr);
            if (!string.IsNullOrEmpty(Username))
            {
                UserProfile original = MemberService.Instance.Get(Username);

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    var count = dataHelper.Get<User_Education>().Where(m => m.Education == education && m.UserId == original.UserId);

                    if (count != null)
                    {

                        int i = jobService.UserEducationUpdate(original.UserId, education, fromyr1, toyr1, school, grade, 4);
                        if (i == 1)
                        {
                            flag = "Uploaded successfully!";
                        }
                    }
                    else
                    {
                        int i = jobService.UserEducationUpdateIn(original.UserId, education, fromyr1, toyr1, school, grade, 4);
                        if (i == 1)
                        {
                            flag = "Uploaded successfully!";
                        }
                    }
                }
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }



        [Authorize]
        [System.Web.Mvc.HttpPost]
        public JsonResult LogoCertUpload(string Username, string data, string area, string certificate, string institute, string type)
        {
            string flag = "Failed";
            long LogoId = 0;
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

                    var count = dataHelper.Get<User_Certificate>().Where(m => m.Certificate == certificate && m.UserId == original.UserId);
                    imageHeight = 130;
                    imageWidth = 145;
                    buffer = Convert.FromBase64String(data);
                    byte[] imgBytes = UIHelper.ResizeImage(buffer, imageWidth, imageHeight);

                    if (area.Equals("Certificate"))
                    {
                        if (count != null)
                        {

                            int i = jobService.UserCertUpdate(original.UserId, certificate, institute, data, type, 4);
                            if (i == 1)
                            {
                                flag = "Uploaded successfully!";
                            }
                        }
                        else
                        {
                            int i = jobService.UserCertUpdateIn(original.UserId, certificate, institute, data, type, 4);
                            if (i == 1)
                            {
                                flag = "Uploaded successfully!";
                            }
                        }

                        comment = "Logo uploaded";
                    }
                }



            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        [System.Web.Mvc.HttpPost]
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

                    if (area.Equals("Background"))
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
                        comment = "Background picture uploaded";
                    }
                    else if (area.Equals("Profile"))
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
                    else if (area.Equals("TopRightFirst"))
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
                        comment = "First picture for top right slider uploaded";
                    }
                    else if (area.Equals("TopRightSecond"))
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
                        comment = "Second picture for top right slider uploaded";
                    }
                    else if (area.Equals("BottomRightFirst"))
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
                        comment = "First picture for bottom right slider uploaded";
                    }
                    else if (area.Equals("BottomRightSecond"))
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
                        comment = "Second picture for bottom right slider uploaded";
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

        public JsonResult DeleteImage(string Username, string Reason = "")
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
            message = "Photo deleted successfully!";
            return Json(message);
        }

        public JsonResult ImageDelete(long Id, string Reason = "")
        {
            string message = string.Empty;
            var original = MemberService.Instance.Get(Id);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                //original.Logo = null;
                //original.IsApproved = false;
                dataHelper.Update<UserProfile>(original, User.Username);
            }

            StringBuilder comments = new StringBuilder();
            if (original.Type == (int)SecurityRoles.Employers)
            {
                comments.AppendFormat("{0}<br/>", Reason);
                comments.AppendFormat("<br/><a href=\"{0}://{1}/company/profile\">CLICK HERE</a> to upload logo!", Request.Url.Scheme, Request.Url.Authority);
            }
            else if (original.Type == (int)SecurityRoles.Jobseeker)
            {
                comments.AppendFormat("{0}<br/>", Reason);
                comments.AppendFormat("<br/><a href=\"{0}://{1}/profile-resume\">CLICK HERE</a> to upload photo!", Request.Url.Scheme, Request.Url.Authority);
            }
            Activity activity = new Activity();
            if (!string.IsNullOrEmpty(Reason))
            {
                activity = new Activity()
                {
                    Comments = comments.ToString(),
                    ActivityDate = DateTime.Now,
                    UserId = original.UserId,
                    DateUpdated = DateTime.Now,
                    UpdatedBy = User.Username,
                    Type = (original.Type == 4) ? (int)ActivityTypes.PHOTO_DELETED : (int)ActivityTypes.LOGO_DELETED,
                    Unread = true
                };
                MemberService.Instance.Track(activity);
            }

            string subject = string.Empty;
            string body = string.Empty;
            if (original.Type == (int)SecurityRoles.Employers)
            {
                message = "Logo deleted successfully!";
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/logodeleted.html")))
                {
                    body = reader.ReadToEnd();
                    subject = "Logo Deleted";
                    body = body.Replace("@@employer", original.Company);
                    body = body.Replace("@@url", string.Format("{0}://{1}/Inbox/Show?Id={2}", Request.Url.Scheme, Request.Url.Authority, activity.Id));
                    body = body.Replace("@@uurl", string.Format("{0}://{1}/company/profile", Request.Url.Scheme, Request.Url.Authority));
                }
            }
            else if (original.Type == (int)SecurityRoles.Jobseeker)
            {
                message = "Photo deleted successfully!";
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/photodeleted.html")))
                {
                    body = reader.ReadToEnd();
                    subject = "Photo Deleted";
                    body = body.Replace("@@firstname", string.Format("{0}", original.FirstName));
                    body = body.Replace("@@lastname", string.Format("{0}", original.LastName));
                    body = body.Replace("@@url", string.Format("{0}://{1}/Inbox/Show?Id={2}", Request.Url.Scheme, Request.Url.Authority, activity.Id));
                    body = body.Replace("@@uurl", string.Format("{0}://{1}/profile-resume", Request.Url.Scheme, Request.Url.Authority));
                }
            }
            string[] receipent = { original.Username };
            AlertService.Instance.SendMail(subject, receipent, body);

            return Json(message);
        }

        public JsonResult ResetPassword(long Id)
        {
            var profile = MemberService.Instance.Get(Id);
            string message = "Failed";
            if (profile != null)
            {
                var membershipUser = Membership.GetUser(profile.Username);
                if (membershipUser != null && membershipUser.IsApproved)
                {
                    var token = WebSecurity.GeneratePasswordResetToken(profile.Username);

                    var reader = new StreamReader(Server.MapPath("~/Templates/Mail/forgot.html"));
                    var body = reader.ReadToEnd();
                    body = body.Replace("@@firstname", profile.FirstName);
                    body = body.Replace("@@lastname", profile.LastName);
                    body = body.Replace("@@url", UrlManager.GetPasswordResetUrl(token, profile.Username));

                    string[] receipent = { profile.Username };
                    var subject = "Reset Your Joblisting Account Password";

                    AlertService.Instance.SendMail(subject, receipent, body);

                    message = string.Format("<div class=\"info_message\"> <h3 style=\"color:#34ba08\">Reset password link sent successfully to {0}</h3></div>", profile.Username);
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Mvc.HttpPost]
        public JsonResult ResumeUpload(string Username, string fileName, string data)
        {
            string flag = "Failed";
            UserProfile original = MemberService.Instance.Get(Username);
            try
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    original.Content = data;
                    original.FileName = fileName;
                    original.IsBuild = false;
                    dataHelper.Update<UserProfile>(original, User.Username);
                    flag = "Resume uploaded successfully!";
                }
            }
            catch (Exception)
            {
                flag = "Failed";
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSeconds()
        {
            int second = 0;

            return Json(second, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Invite(string countryCode, string mobile)
        {
            Regex reg = null;
            reg = new System.Text.RegularExpressions.Regex("^[1-9][0-9]*$");
            string message = string.Empty;
            if (reg.IsMatch(mobile))
            {
                UserProfile profile = MemberService.Instance.Get(User.Username);

                string AccountSid = ConfigService.Instance.GetConfigValue("TwilioSID");
                string AuthToken = ConfigService.Instance.GetConfigValue("TwilioToken");
                string from = ConfigService.Instance.GetConfigValue("TwilioNumber");
                var twilio = new TwilioRestClient(AccountSid, AuthToken);
                StringBuilder sbSMS = new StringBuilder();

                if (profile.Type == 4)
                {
                    sbSMS.AppendFormat("{0} {1} invites you to connect at Joblisting.com", profile.FirstName, profile.LastName);
                }
                else
                {
                    sbSMS.AppendFormat("{0} invites you to connect at Joblisting.com", profile.Company);
                }
                string to = string.Format("+{0}{1}", countryCode, mobile);
                var sms = twilio.SendMessage(from, mobile, sbSMS.ToString());
                if (sms.RestException == null)
                {
                    var msg = twilio.GetMessage(sms.Sid);
                    if (msg.Status.Equals("sent"))
                    {
                        message = "Invitation sent successfully!";
                    }
                }
                else
                {
                    if (sms.RestException.Code.Equals("14101"))
                    {
                        message = "Please provide valid mobile number!";
                    }
                }
            }
            else
            {
                message = "Please provide valid mobile number!";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActivityList(string Username, int PageNumber = 0)
        {
            List<ActivityStreamModel> activities = MemberService.Instance.GetPhotoList(Username, Request.IsAuthenticated)
                .Select(x => new ActivityStreamModel(x.UserId) { Id = x.Id, DateUpdated = x.DateUpdated, Image = Convert.ToBase64String(x.Image), Type = x.Type, Area = x.Area }).ToList();

            var jsonResult = Json(activities, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
        public ActionResult ReadStreams(long id, int pageNumber = 1)
        {
            List<StreamEntity> list = new List<StreamEntity>();
            if (User != null)
            {
                list = (new PortalDataService()).ListStream(id, User.Id, pageNumber);
            }
            else
            {
                list = (new PortalDataService()).ListStream(id, null, pageNumber);
            }
            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
        public async Task<ActionResult> RecentJobs(long? Id, int pageNumber = 1)
        {
            JsonResult jsonResult = new JsonResult();
            List<RecentJobEntity> jobs = new List<RecentJobEntity>();

            if (User != null)
            {
                switch (User.Info.Role)
                {
                    case SecurityRoles.Jobseeker:
                        jobs = await jobService.RecentJobs(User.Info.CountryId, null, pageNumber);
                        break;
                    case SecurityRoles.Employers:
                        jobs = await jobService.RecentJobs(User.Info.CountryId, null, pageNumber);
                        break;
                }
            }
            else
            {
                jobs = await jobService.RecentJobs(null, null, pageNumber);
            }

            jsonResult = Json(jobs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [ValidateInput(false)]
        public ActionResult Connections(string address = null)
        {
            UserProfile profile = null;
            if (string.IsNullOrEmpty(address))
            {
                profile = MemberService.Instance.Get(User.Username);
            }
            else
            {
                profile = MemberService.Instance.GetByAddress(address);
            }

            List<UserInfo> connections = new List<UserInfo>();
            if (profile != null && User != null)
            {
                List<long?> connected = DomainService.Instance.ConnectionList(profile.UserId, User.Id);

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    if (profile != null)
                    {
                        connected = connected.Where(x => x != null).ToList();
                        if (connected.Count > 0)
                        {
                            connections = connected.Select(x => new UserInfo(x.Value)).ToList();
                            if (connections != null && connections.Count > 0)
                            {
                                connections = connections.OrderBy(x => Guid.NewGuid()).Take(3).ToList();
                            }
                        }
                    }
                }
            }
            var jsonResult = Json(connections, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        public ActionResult PeopleList()
        {
            List<UserInfo> user_list1 = new List<UserInfo>();
            if (User != null)
            {
                UserProfile profile = MemberService.Instance.Get(User.Username);
                List<Parameter> parameters = new List<Parameter>();
                List<UserProfile> user_list = new List<UserProfile>();

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    var blockedList = dataHelper.Get<BlockedPeople>().Where(x => x.BlockedId == profile.UserId).Select(x => x.BlockerId);

                    int company = (int)SecurityRoles.Employers;
                    var companies = dataHelper.Get<UserProfile>().Where(x => x.IsActive == true && x.IsDeleted == false && x.IsConfirmed == true && x.Type == company && x.UserId != profile.UserId);
                    if (blockedList.Any())
                    {
                        companies = companies.Where(x => !blockedList.Contains(x.UserId));
                    }

                    int individual = (int)SecurityRoles.Jobseeker;
                    var individuals = dataHelper.Get<UserProfile>().Where(x => x.IsActive == true && x.IsDeleted == false && x.IsConfirmed == true && x.Type == individual && x.UserId != profile.UserId);
                    if (blockedList.Any())
                    {
                        individuals = individuals.Where(x => !blockedList.Contains(x.UserId));
                    }

                    if (profile.Type == (int)SecurityRoles.Jobseeker)
                    {
                        string[] cemployer_names = new string[] { "" };
                        string cemployer = string.Empty;
                        if (!string.IsNullOrEmpty(profile.CurrentEmployer))
                        {
                            cemployer_names = profile.CurrentEmployer.Split(' ');
                            if (cemployer_names.Length > 1)
                            {
                                cemployer = string.Format("{0} {1}", cemployer_names[0], cemployer_names[1]).ToLower();
                            }
                            else
                            {
                                cemployer = profile.CurrentEmployer.ToLower();
                            }
                        }

                        string[] pemployer_names = new string[] { "" };
                        string pemployer = string.Empty;
                        if (!string.IsNullOrEmpty(profile.PreviousEmployer))
                        {
                            pemployer_names = profile.PreviousEmployer.Split(' ');
                            if (pemployer_names.Length > 1)
                            {
                                pemployer = string.Format("{0} {1}", pemployer_names[0], pemployer_names[1]).ToLower();
                            }
                            else
                            {
                                pemployer = profile.PreviousEmployer.ToLower();
                            }
                        }

                        individuals = individuals.Where(x =>
                            (x.LastName == profile.LastName && x.CountryId == profile.CountryId && (x.FirstName != profile.FirstName && x.LastName != profile.LastName))
                            || ((x.Address != null || x.Zip != null) && x.Address == profile.Address && x.City == profile.City && x.StateId == profile.StateId && x.Zip == profile.Zip && x.CountryId == profile.CountryId)
                            || (x.Phone != null && x.Phone == profile.Phone && x.CountryId == profile.CountryId)
                            || (x.School != null && x.School == profile.School && x.CountryId == profile.CountryId)
                            || (x.University != null && x.University == profile.University)
                            || ((profile.CategoryId != null && x.CategoryId == profile.CategoryId) && (profile.SpecializationId != null && x.SpecializationId == profile.SpecializationId)
                            && ((profile.CurrentEmployer != null && (x.CurrentEmployer.ToLower().Contains(cemployer) || x.CurrentEmployer.ToLower().Contains(pemployer)))
                            || (profile.PreviousEmployer != null && (x.PreviousEmployer.ToLower().Contains(cemployer) || x.PreviousEmployer.ToLower().Contains(pemployer)))) && x.CountryId == profile.CountryId));

                        user_list.AddRange(individuals.ToList());

                        companies = companies.Where(x =>
                           (x.LastName == profile.LastName && x.CountryId == profile.CountryId && (x.FirstName != profile.FirstName && x.LastName != profile.LastName))
                           || ((x.Address != null || x.Zip != null) && x.Address == profile.Address && x.City == profile.City && x.StateId == profile.StateId && x.Zip == profile.Zip && x.CountryId == profile.CountryId)
                           || (x.Phone != null && x.Phone == profile.Phone && x.CountryId == profile.CountryId)
                           || ((profile.CurrentEmployer != null && x.Company.ToLower().Contains(cemployer) || profile.PreviousEmployer != null && x.Company.ToLower().Contains(pemployer)) && x.CountryId == profile.CountryId));

                        user_list.AddRange(companies.ToList());
                    }
                    else if (profile.Type == (int)SecurityRoles.Employers)
                    {
                        string[] company_names = new string[] { "" };
                        string cname = string.Empty;
                        if (!string.IsNullOrEmpty(profile.Company))
                        {
                            company_names = profile.Company.Split(' ');
                            if (company_names.Length > 1)
                            {
                                cname = string.Format("{0} {1}", company_names[0], company_names[1]).ToLower();
                            }
                            else
                            {
                                cname = profile.Company.ToLower();
                            }
                        }
                        companies = companies.Where(x =>
                         (x.LastName == profile.LastName && x.CountryId == profile.CountryId && (x.FirstName != profile.FirstName && x.LastName != profile.LastName))
                         || ((x.Address != null || x.Zip != null) && x.Address == profile.Address && x.City == profile.City && x.StateId == profile.StateId && x.Zip == profile.Zip && x.CountryId == profile.CountryId)
                         || (x.Phone != null && x.Phone == profile.Phone && x.CountryId == profile.CountryId)
                         || (profile.Company != null && x.Company.ToLower().Contains(cname) && x.CountryId == profile.CountryId));

                        user_list.AddRange(companies.ToList());

                        individuals = individuals.Where(x =>
                           (x.LastName == profile.LastName && x.CountryId == profile.CountryId && (x.FirstName != profile.FirstName && x.LastName != profile.LastName))
                           || ((x.Address != null || x.Zip != null) && x.Address == profile.Address && x.City == profile.City && x.StateId == profile.StateId && x.Zip == profile.Zip && x.CountryId == profile.CountryId)
                           || (x.Phone != null && x.Phone == profile.Phone && x.CountryId == profile.CountryId)
                           || ((x.CurrentEmployer != null && x.CurrentEmployer.ToLower().Contains(cname) || x.PreviousEmployer != null && x.PreviousEmployer.ToLower().Contains(cname)) && x.CountryId == profile.CountryId));

                        user_list.AddRange(individuals.ToList());
                    }

                    List<long> matchlist = dataHelper.Get<PeopleMatch>().Where(x => x.UserId == profile.UserId).Select(x => x.MatchId).ToList();
                    user_list = user_list.Where(x => !matchlist.Contains(x.UserId)).ToList();

                    List<Connection> connection_list = MemberService.Instance.GetConnections(User.Username);
                    connection_list = connection_list.Where(x => x.IsDeleted == false).ToList();
                    user_list1 = user_list.Where(x => !connection_list.Any(z => z.EmailAddress.Equals(x.Username)))
                        .OrderBy(x => Guid.NewGuid()).Take(8).Select(x => new UserInfo(x.UserId)).ToList();
                }
            }
            var jsonResult = Json(user_list1, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        public ActionResult ProfileViewed(int pageNumber = 0)
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
            var jsonResult = Json(user_list, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Remove from people you may known list.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Remove(long Id)
        {
            string message = "Failed";
            UserProfile member = MemberService.Instance.Get(User.Username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                PeopleMatch entity = dataHelper.Get<PeopleMatch>().SingleOrDefault(x => x.MatchId == Id && x.UserId == member.UserId);
                if (entity == null)
                {
                    entity = new PeopleMatch()
                    {
                        MatchId = Id,
                        UserId = member.UserId
                    };
                    dataHelper.Add<PeopleMatch>(entity);
                    message = "Success";
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SalesByPackage(string fd, string fm, string fy, string td, string tm, string ty)
        {
            DateTime? start = null, end = null;
            if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy))
            {
                start = new DateTime(Convert.ToInt32(fy), Convert.ToInt32(fm), Convert.ToInt32(fd));
            }

            if (!string.IsNullOrEmpty(td) && !string.IsNullOrEmpty(tm) && !string.IsNullOrEmpty(ty))
            {
                end = new DateTime(Convert.ToInt32(ty), Convert.ToInt32(tm), Convert.ToInt32(td));
            }

            List<SoldPackageEntity> list = DomainService.Instance.SalesByPackage(start, end);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SalesByCountry(string fd, string fm, string fy, string td, string tm, string ty, long? countryId = null)
        {
            DateTime? start = null, end = null;
            if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy))
            {
                start = new DateTime(Convert.ToInt32(fy), Convert.ToInt32(fm), Convert.ToInt32(fd));
            }

            if (!string.IsNullOrEmpty(td) && !string.IsNullOrEmpty(tm) && !string.IsNullOrEmpty(ty))
            {
                end = new DateTime(Convert.ToInt32(ty), Convert.ToInt32(tm), Convert.ToInt32(td));
            }

            List<SaleByCountryEntity> list = DomainService.Instance.SalesByCountry(start, end, countryId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Resend()
        {
            string message = string.Empty;
            try
            {
                if (User != null)
                {
                    UserInfoEntity uinfo = _service.Get(User.Id);
                    string token = UIHelper.Get6DigitCode();
                    _service.GenerateToken(User.Id, token);

                    var reader = new StreamReader(Server.MapPath("~/Templates/Mail/resend_confirmation.html"));
                    var body = reader.ReadToEnd();
                    body = body.Replace("@@receiver", uinfo.FullName);

                    body = body.Replace("@@code", token);
                    var hosturl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                        string.Format("/Confirm?id={0}&token={1}", User.Id, token);

                    body = body.Replace("@@url", hosturl);

                    string[] receipent = { User.Username };
                    var subject = "Confirm Your Email Address";

                    AlertService.Instance.SendMail(subject, receipent, body);
                }
                message = "Email sent successfully!";
            }
            catch (Exception ex)
            {
                message = ex.ToString();
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Confirm(long id, string token)
        {
            ResponseContext context = new ResponseContext();
            UserInfoEntity uinfo = _service.Get(id);
            if (uinfo != null)
            {
                if (uinfo.IsConfirmed)
                {
                    context = new ResponseContext()
                    {
                        Id = -1,
                        Type = "Error",
                        Message = "You have already verified your account!"
                    };
                }
                else
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        if (uinfo.ConfirmationToken.Equals(token))
                        {
                            _service.Confirm(id);
                            context = new ResponseContext()
                            {
                                Id = 1,
                                Type = "Success",
                                Message = "Account confirmed successfully!",
                            };
                        }
                        else
                        {
                            context = new ResponseContext()
                            {
                                Id = -1,
                                Type = "Error",
                                Message = "The code you are using has already expired!"
                            };
                        }
                    }
                }
            }
            return Json(context, JsonRequestBehavior.AllowGet);
        }
    }
}