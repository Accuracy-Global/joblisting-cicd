using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Web.Models;
using PagedList;
using WebMatrix.WebData;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Collections;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Net.Mime;
using System.Text.RegularExpressions;
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Utility;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Helpers;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using TweetSharp;
using Microsoft.Security.Application;
using System.Configuration;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;
//using System.Net.Mail;
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0105 // The using directive for 'System.Data' appeared previously in this namespace
using System.Data;
#pragma warning restore CS0105 // The using directive for 'System.Data' appeared previously in this namespace
using System.Data.SqlClient;
#pragma warning disable CS0105 // The using directive for 'System.Collections.Generic' appeared previously in this namespace
using System.Collections.Generic;
#pragma warning restore CS0105 // The using directive for 'System.Collections.Generic' appeared previously in this namespace
#pragma warning disable CS0105 // The using directive for 'System.Text' appeared previously in this namespace
using System.Text;
#pragma warning restore CS0105 // The using directive for 'System.Text' appeared previously in this namespace
using System.Web.Script.Serialization;
using System.Drawing;
using JobPortal.Web.Models.Pagination;
using System.Dynamic;
using Twilio;
using System.Net.Http;
using System.Net.Mail;
using JobPortal.Web.App_Start;

namespace JobPortal.Web.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        JobPortalEntities db = new JobPortalEntities();
#pragma warning disable CS0246 // The type or namespace name 'INetworkService' could not be found (are you missing a using directive or an assembly reference?)
        INetworkService iNetworkService;

        IHelperService helper;
#pragma warning restore CS0246 // The type or namespace name 'INetworkService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INetworkService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
        public AdminController(IUserService service, INetworkService iNetworkService)
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'INetworkService' could not be found (are you missing a using directive or an assembly reference?)
            : base(service)
        {
            this.iNetworkService = iNetworkService;
        }
#pragma warning disable CS0246 // The type or namespace name 'AutomatchJobseeker' could not be found (are you missing a using directive or an assembly reference?)
        List<AutomatchJobseeker> jseekerList = new List<AutomatchJobseeker>();
#pragma warning restore CS0246 // The type or namespace name 'AutomatchJobseeker' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'AutomatchJob' could not be found (are you missing a using directive or an assembly reference?)
        List<AutomatchJob> jobList = new List<AutomatchJob>();
#pragma warning restore CS0246 // The type or namespace name 'AutomatchJob' could not be found (are you missing a using directive or an assembly reference?)

        public ActionResult Dashboard()
        {
            return View();
        }


        public ActionResult Users(int? countryId, string fd, string fm, string fy, string td, string tm, string ty, int? typeId = null, string name = null, bool? confirmed = null, bool? active = null, int pageNumber = 1)
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

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
            {
                countryId = 40;
            }


            countryId = countryId == 0 ? null : countryId;
            List country = null;
            if (countryId != null)
            {
                country = SharedService.Instance.GetCountry(countryId.Value);
            }
            int pageSize = 10;
            int rows = 0;
            List<UserInfoEntity> list = new List<UserInfoEntity>();

            switch (typeId)
            {
                case 4:
                    ViewBag.UserType = "Individuals User Accounts";
                    break;
                case 5:
                    ViewBag.UserType = "Company User Accounts";
                    break;
            }

            var sdt = new DateTime?();
            var edt = new DateTime?();
            string sdate = string.Empty;
            string edate = string.Empty;

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

            list = _service.GetUsers(typeId, countryId, sdt, edt, name, confirmed, active, pageSize, pageNumber);
            if (list.Count > 0)
            {
                rows = list.FirstOrDefault().MaxRows;
            }
            ViewBag.Males = list.Count(x => x.Gender != null && x.Gender.Equals("Male"));
            ViewBag.Females = list.Count(x => x.Gender != null && x.Gender.Equals("Female"));

            ViewBag.Individuals = list.Count(x => x.Type == 4);
            ViewBag.Companies = list.Count(x => x.Type == 5);

            ViewBag.TypeId = typeId;
            ViewBag.Country = country;

            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<UserInfoEntity>(list, pageNumber, pageSize, rows);
            return View();
        }

        public ActionResult Jobseekers(int? countryId, string fd, string fm, string fy, string td, string tm, string ty, string type = null, string name = null, int pageNumber = 1)
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

            countryId = countryId == 0 ? null : countryId;
            List country = null;
            if (countryId != null)
            {
                country = SharedService.Instance.GetCountry(countryId.Value);
            }
            int pageSize = 10;
            int rows = 0;
            ViewBag.UserType = "Jobseeker Accounts";

            var sdt = new DateTime?();
            var edt = new DateTime?();
            string sdate = string.Empty;
            string edate = string.Empty;

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

            List<UserInfoEntity> list = _service.GetJobseekers(type, countryId, sdt, edt, name, pageSize, pageNumber);
            if (list.Count > 0)
            {
                rows = list.FirstOrDefault().MaxRows;
            }
            ViewBag.Males = list.Count(x => x.Gender != null && x.Gender.Equals("Male"));
            ViewBag.Females = list.Count(x => x.Gender != null && x.Gender.Equals("Female"));

            ViewBag.Country = country;

            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<UserInfoEntity>(list, pageNumber, pageSize, rows);
            return View();
        }


        public ActionResult UserList(int countryId, int typeId, string name, string fd, string fm, string fy, string td, string tm, string ty, int pageNumber = 0, bool? verified = null, string ut = null)
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

            List country = SharedService.Instance.GetCountry(countryId);
            int pageSize = 10;
            int rows = 0;
            List<UserProfile> list = new List<UserProfile>();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<UserProfile>().Where(x => x.CountryId == countryId);

                switch (typeId)
                {
                    case 1:
                        result = result.Where(x => x.Type == 4);
                        ViewBag.UserType = "Individuals User Accounts";
                        break;
                    case 3:
                        result = result.Where(x => x.Type == 5);
                        ViewBag.UserType = "Company User Accounts";
                        break;
                }

                if (!string.IsNullOrEmpty(name))
                {
                    result = result.Where(x => ((x.FirstName == null ? " " : x.FirstName) + (x.LastName == null ? " " : x.LastName) + (x.Company == null ? "" : x.Company)).ToLower().Contains(name.ToLower()));
                }

                if (!string.IsNullOrEmpty(ut))
                {
                    result = result.Where(x => x.Title != null && x.CategoryId != null && x.SpecializationId != null);
                }
                else
                {
                    result = result.Where(x => x.Title == null && x.CategoryId == null && x.SpecializationId == null);
                }
                var sdt = new DateTime?();
                var edt = new DateTime?();
                string sdate = string.Empty;
                string edate = string.Empty;

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

                if (sdt != null && edt != null)
                {
                    result = result.Where(x => x.DateCreated.Day >= sdt.Value.Day && x.DateCreated.Month >= sdt.Value.Month && x.DateCreated.Year >= sdt.Value.Year && x.DateCreated.Day <= edt.Value.Day && x.DateCreated.Month <= edt.Value.Month && x.DateCreated.Year <= edt.Value.Year);
                }

                if (verified != null && verified.Value == true)
                {
                    result = result.Where(x => x.IsConfirmed == true);
                }
                rows = result.Count();
                ViewBag.Males = result.Count(x => x.Gender.Equals("Male"));
                ViewBag.Females = result.Count(x => x.Gender.Equals("Female"));
                list = result.OrderByDescending(x => x.DateCreated).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize)).Take(pageSize).ToList<UserProfile>();
            }

            ViewBag.TypeId = typeId;
            ViewBag.Country = country;

            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<UserProfile>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
            return View();
        }
        public ActionResult VerifiedUserList(int countryId, int typeId, string name, string fd, string fm, string fy, string td, string tm, string ty, int pageNumber = 0, bool? verified = null)
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

            List country = SharedService.Instance.GetCountry(countryId);
            int pageSize = 10;
            int rows = 0;
            List<UserProfile> list = new List<UserProfile>();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<UserProfile>().Where(x => x.CountryId == countryId);

                switch (typeId)
                {
                    case 1:
                        result = result.Where(x => x.Type == 4);
                        ViewBag.UserType = "Individuals User Accounts";
                        break;
                    case 3:
                        result = result.Where(x => x.Type == 5);
                        ViewBag.UserType = "Company User Accounts";
                        break;
                }

                if (!string.IsNullOrEmpty(name))
                {
                    result = result.Where(x => ((x.FirstName == null ? " " : x.FirstName) + (x.LastName == null ? " " : x.LastName) + (x.Company == null ? "" : x.Company)).ToLower().Contains(name.ToLower()));
                }


                var sdt = new DateTime?();
                var edt = new DateTime?();
                string sdate = string.Empty;
                string edate = string.Empty;

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

                if (sdt != null && edt != null)
                {
                    result = result.Where(x => x.DateCreated.Day >= sdt.Value.Day && x.DateCreated.Month >= sdt.Value.Month && x.DateCreated.Year >= sdt.Value.Year && x.DateCreated.Day <= edt.Value.Day && x.DateCreated.Month <= edt.Value.Month && x.DateCreated.Year <= edt.Value.Year);
                }

                if (verified != null && verified.Value == true)
                {
                    result = result.Where(x => x.IsConfirmed == true);
                }
                rows = result.Count();
                ViewBag.Males = result.Count(x => x.Gender.Equals("Male"));
                ViewBag.Females = result.Count(x => x.Gender.Equals("Female"));
                list = result.OrderByDescending(x => x.DateCreated).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize)).Take(pageSize).ToList<UserProfile>();
            }

            ViewBag.TypeId = typeId;
            ViewBag.Country = country;

            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<UserProfile>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
            return View();
        }

        public ActionResult SearchedUserList(string name, string fd, string fm, string fy, string td, string tm, string ty, int pageNumber = 0)
        {

            int pageSize = 10;
            int rows = 0;
            List<UserProfile> list = new List<UserProfile>();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<UserProfile>();

                if (!string.IsNullOrEmpty(name))
                {
                    result = result.Where(x => ((x.FirstName == null ? " " : x.FirstName) + " " + (x.LastName == null ? " " : x.LastName) + (x.Company == null ? "" : " " + x.Company) + " " + x.Username).ToLower().Contains(name.ToLower()));
                }

                var sdt = new DateTime?();
                var edt = new DateTime?();
                string sdate = string.Empty;
                string edate = string.Empty;

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

                if (sdt != null && edt != null)
                {
                    result = result.Where(x => x.DateCreated.Day >= sdt.Value.Day && x.DateCreated.Month >= sdt.Value.Month && x.DateCreated.Year >= sdt.Value.Year && x.DateCreated.Day <= edt.Value.Day && x.DateCreated.Month <= edt.Value.Month && x.DateCreated.Year <= edt.Value.Year);
                }

                rows = result.Count();
                list = result.OrderByDescending(x => x.DateCreated).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize)).Take(pageSize).ToList<UserProfile>();
            }
            if (!string.IsNullOrEmpty(name))
            {
                ViewBag.Name = name;
            }
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<UserProfile>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
            return View();
        }

        public ActionResult UserConnections(long Id, string name, string fd, string fm, string fy, string td, string tm, string ty, int pageNumber = 0)
        {

            int pageSize = 10;
            int rows = 0;
            List<Connection> list = new List<Connection>();
            UserProfile profile = MemberService.Instance.Get(Id);
            string sdate = string.Empty;
            string edate = string.Empty;
            var sdt = new DateTime?();
            var edt = new DateTime?();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var connections = dataHelper.Get<Connection>().Where(x => x.UserId == Id);


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
                if (sdt != null)
                {
                    connections = connections.Where(x => x.DateCreated.Day >= sdt.Value.Day && x.DateCreated.Month >= sdt.Value.Month && x.DateCreated.Year >= sdt.Value.Year);
                }

                if (edt != null)
                {
                    connections = connections.Where(x => x.DateCreated.Day <= edt.Value.Day && x.DateCreated.Month <= edt.Value.Month && x.DateCreated.Year <= edt.Value.Year);
                }

                rows = connections.Count();
                list = connections.ToList();
                if (!string.IsNullOrEmpty(name))
                {
                    list = list.Where(x => x.EmailAddress.ToLower().Contains(name.ToLower()) || (MemberService.Instance.Get(x.EmailAddress) != null && (MemberService.Instance.Get(x.EmailAddress).Company == null ? string.Format("{0} {1}", MemberService.Instance.Get(x.EmailAddress).FirstName, MemberService.Instance.Get(x.EmailAddress).LastName) : MemberService.Instance.Get(x.EmailAddress).Company).ToLower().Contains(name.ToLower()))).ToList();
                    list = list.OrderByDescending(x => x.DateCreated).Skip(pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList<Connection>();
                }
                else
                {
                    list = connections.OrderByDescending(x => x.DateCreated).Skip(pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList<Connection>();
                }
            }
            ViewBag.Country = SharedService.Instance.GetCountry(profile.CountryId.Value);
            ViewBag.UserInfo = profile;
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<Connection>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);

            return View();
        }
        public ActionResult UsersInConnection(long? countryId, int? userType, string name, string fd, string fm, string fy, string td, string tm, string ty, int pageNumber = 0)
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

            List country = new List();
            if (countryId != null)
            {
                country = SharedService.Instance.GetCountry(countryId.Value);
            }
            int pageSize = 10;
            int rows = 0;
            int concounts = 0;
            List<UserProfile> list = new List<UserProfile>();
            List<int> typeList = new List<int>() { 4, 5 };
            string sdate = string.Empty;
            string edate = string.Empty;
            var sdt = new DateTime?();
            var edt = new DateTime?();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var connections = dataHelper.Get<UserProfile>().Where(x => x.Connections.Count > 0);


                if (countryId != null)
                {
                    connections = connections.Where(x => x.CountryId == countryId.Value);
                }

                if (userType != null)
                {
                    connections = connections.Where(x => x.Type == userType);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    connections = connections.Where(x => ((x.FirstName == null ? " " : x.FirstName) + (x.LastName == null ? " " : x.LastName) + (x.Company == null ? "" : x.Company)).ToLower().Contains(name.ToLower()));
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
                if (sdt != null && edt != null)
                {
                    connections = connections.Where(x => x.DateCreated.Day >= sdt.Value.Day && x.DateCreated.Month >= sdt.Value.Month && x.DateCreated.Year >= sdt.Value.Year && x.DateCreated.Day <= edt.Value.Day && x.DateCreated.Month <= edt.Value.Month && x.DateCreated.Year <= edt.Value.Year);
                }


                concounts = connections.Where(x => x.Connections.Count > 0).ToList().Select(x => x.Connections.Count).Sum();
                rows = connections.Count();
                list = connections.OrderByDescending(x => x.DateCreated).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList<UserProfile>();
            }

            ViewBag.Connections = concounts;
            ViewBag.UserType = userType;
            ViewBag.Country = country;
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<UserProfile>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
            return View();
        }

        public ActionResult CountryUserList(int countryId, int? userType, string name, string fd, string fm, string fy, string td, string tm, string ty, int pageNumber = 0)
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

            List country = SharedService.Instance.GetCountry(countryId);
            int pageSize = 10;
            int rows = 0;
            List<UserProfile> list = new List<UserProfile>();
            List<int> typeList = new List<int>() { 4, 5 };
            string sdate = string.Empty;
            string edate = string.Empty;
            var sdt = new DateTime?();
            var edt = new DateTime?();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<UserProfile>().Where(x => x.CountryId == countryId && typeList.Contains(x.Type));

                if (userType != null)
                {
                    result = result.Where(x => x.Type == userType);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    result = result.Where(x => ((x.FirstName == null ? " " : x.FirstName) + (x.LastName == null ? " " : x.LastName) + (x.Company == null ? "" : x.Company)).ToLower().Contains(name.ToLower()));
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
                if (sdt != null)
                {
                    result = result.Where(x => x.DateCreated.Day >= sdt.Value.Day && x.DateCreated.Month >= sdt.Value.Month && x.DateCreated.Year >= sdt.Value.Year);
                }

                if (edt != null)
                {
                    result = result.Where(x => x.DateCreated.Day <= edt.Value.Day && x.DateCreated.Month <= edt.Value.Month && x.DateCreated.Year <= edt.Value.Year);
                }
                rows = result.Count();
                list = result.OrderByDescending(x => x.DateCreated).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList<UserProfile>();
            }
            ViewBag.UserType = userType;
            ViewBag.Country = country;
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<UserProfile>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
            return View();
        }
        public ActionResult Tips(int? Type = null, DateTime? StartDate = null, DateTime? EndDate = null, int pageNumber = 0)
        {
            int rows = 0;
            int pageSize = 10;
            List<Tip> list = new List<Tip>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<Tip>();
                if (Type != null)
                {
                    result = result.Where(x => x.Type == Type.Value);
                }

                if (StartDate != null)
                {
                    result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                }

                if (EndDate != null)
                {
                    if (StartDate == null)
                    {
                        result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                    }
                    result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                }

                //result = result.OrderByDescending(x => x.DateCreated);
                rows = result.Count();
                if (rows > 0)
                {
                    list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateCreated : x.DateUpdated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                }
            }
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<Tip>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);

            return View();
        }

        [UrlPrivilegeFilter]
        public ActionResult Websites(int? Type = null, DateTime? StartDate = null, DateTime? EndDate = null, int pageNumber = 0)
        {
            int rows = 0;
            int pageSize = 10;
            List<WebsiteList> list = new List<WebsiteList>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<WebsiteList>();
                if (StartDate != null)
                {
                    result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                }

                if (EndDate != null)
                {
                    if (StartDate == null)
                    {
                        result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                    }
                    result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                }

                rows = result.Count();
                if (rows > 0)
                {
                    list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                }
            }
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<WebsiteList>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);

            return View();
        }
       
        [UrlPrivilegeFilter]
        public ActionResult SMJobs(int? Type = null, DateTime? StartDate = null, DateTime? EndDate = null, int pageNumber = 0)
        {
            int rows = 0;
            int pageSize = 10;
            List<SMJobList> list = new List<SMJobList>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<SMJobList>();
                if (StartDate != null)
                {
                    result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                }

                if (EndDate != null)
                {
                    if (StartDate == null)
                    {
                        result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                    }
                    result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                }

                rows = result.Count();
                if (rows > 0)
                {
                    list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                }
            }
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<SMJobList>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);

            return View();
        }

        public ActionResult Campaign(int? Type = null, DateTime? StartDate = null, DateTime? EndDate = null, int pageNumber = 0)
        {
            ViewBag.exception1 = TempData["exception1"];
            ViewBag.exception2 = TempData["exception2"];
            ViewBag.exception3 = TempData["exception3"];
            ViewBag.exception4 = TempData["exception4"];
            int rows = 0;
            int pageSize = 10;
            List<Campaign> list = new List<Campaign>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<Campaign>();
                if (Type != null)
                {
                    result = result.Where(x => x.Type == Type.Value);
                }

                if (StartDate != null)
                {
                    result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                }

                if (EndDate != null)
                {
                    if (StartDate == null)
                    {
                        result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                    }
                    result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                }

                rows = result.Count();
                if (rows > 0)
                {
                    list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                }
            }
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<Campaign>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);

            return View();
        }

        public ActionResult Announcements(int? Type = null, DateTime? StartDate = null, DateTime? EndDate = null, int pageNumber = 0)
        {
            int rows = 0;
            int pageSize = 10;
            List<Announcement> list = new List<Announcement>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<Announcement>();
                
                if (Type != null)
                {
                    if (Type.Value == 0)
                    {
                        result = dataHelper.Get<Announcement>();
                    }
                    else {
                    result = result.Where(x => x.Type == Type.Value);
                    }
                }

                if (StartDate != null)
                {
                    result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                }

                if (EndDate != null)
                {
                    if (StartDate == null)
                    {
                        result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                    }
                    result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                }

                rows = result.Count();
                if (rows > 0)
                {
                    list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                }
            }
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<Announcement>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);

            return View();
        }
        public ActionResult Updates(int? Type = null, DateTime? StartDate = null, DateTime? EndDate = null, int pageNumber = 0)
        {
            int rows = 0;
            int pageSize = 10;
            List<Announcement> list = new List<Announcement>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<Announcement>();

                if (Type != null)
                {
                    if (Type.Value == 0)
                    {
                        result = dataHelper.Get<Announcement>();
                    }
                    else
                    {
                        result = result.Where(x => x.Type == Type.Value);
                    }
                }

                if (StartDate != null)
                {
                    result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                }

                if (EndDate != null)
                {
                    if (StartDate == null)
                    {
                        result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                    }
                    result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                }

                rows = result.Count();
                if (rows > 0)
                {
                    list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                }
            }
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<Announcement>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);

            return View();
        }
        [HttpGet]
        public ActionResult Recampaign(long Id)
        {
            CampaignModel model = new CampaignModel();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Campaign entity = dataHelper.GetSingle<Campaign>(Id);

                model = new CampaignModel()
                {
                    Id = entity.Id,
                    CountryId = entity.CountryId,
                    CategoryId = entity.CategoryId,
                    Name = entity.Username,
                    Type = entity.Type,
                    Subject = entity.Subject,
                    Body = entity.Body
                };
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Reannounce(long Id)
        {
            AnnouncementModel model = new AnnouncementModel();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Announcement entity = dataHelper.GetSingle<Announcement>(Id);

                model = new AnnouncementModel()
                {
                    Type = entity.Type,
                    CountryId = entity.CountryId,
                    Subject = entity.Subject,
                    Body = entity.Body
                };
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult Recampaign(CampaignModel model)
        {
            if (ModelState.IsValid)
            {
                Campaign campaign = new Campaign()
                {
                    Id = model.Id,
                    CountryId = model.CountryId,
                    CategoryId = model.CategoryId,
                    Username = model.Name,
                    Type = model.Type,
                    Subject = model.Subject,
                    Body = model.Body
                };
                List<long> userlist = new List<long>();
                long campaignId = 0;
                string subject = string.Empty;
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    campaignId = Convert.ToInt32(dataHelper.Add<Campaign>(campaign, User.Username));
                    subject = model.Subject;

                    if (campaignId > 0)
                    {
                        if (model.Type != 0)
                        {
                            userlist = dataHelper.Get<UserProfile>().Where(x => x.Type == model.Type).Select(x => x.UserId).ToList();
                        }
                        else
                        {
                            userlist = dataHelper.Get<UserProfile>().Select(x => x.UserId).ToList();
                        }

                        foreach (var userid in userlist)
                        {
                            UserCampaign entity = new UserCampaign()
                            {
                                CampaignId = campaignId,
                                UserId = userid,
                                DateCreated = DateTime.Now,
                                Unread = true
                            };
                            dataHelper.AddEntity<UserCampaign>(entity);
                        }
                        dataHelper.Save();
                    }
                }
                TempData["UpdateData"] = "ReCampaign successfully!";


            }
            return Redirect("/Admin/Campaign");
        }

        [HttpPost]
        public ActionResult Reannounce(AnnouncementModel model)
        {
            if (ModelState.IsValid)
            {
                Announcement announcement = new Announcement()
                {
                    Type = model.Type,
                    CountryId = model.CountryId,
                    Subject = model.Subject,
                    Body = model.Body
                };
                List<long> userlist = new List<long>();
                long announcmentId = 0;
                string subject = string.Empty;
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    announcmentId = Convert.ToInt32(dataHelper.Add<Announcement>(announcement, User.Username));
                    subject = model.Subject;

                    if (announcmentId > 0)
                    {
                        if (model.Type != 0)
                        {
                            userlist = dataHelper.Get<UserProfile>().Where(x => x.Type == model.Type).Select(x => x.UserId).ToList();
                        }
                        else
                        {
                            userlist = dataHelper.Get<UserProfile>().Select(x => x.UserId).ToList();
                        }

                        foreach (var userid in userlist)
                        {
                            UserAnnouncement entity = new UserAnnouncement()
                            {
                                AnnouncementId = announcmentId,
                                UserId = userid,
                                DateCreated = DateTime.Now,
                                Unread = true
                            };
                            dataHelper.AddEntity<UserAnnouncement>(entity);
                        }
                        dataHelper.Save();
                    }
                }
                TempData["UpdateData"] = "Reannounced successfully!";


            }
            return Redirect("/Admin/Announcements");
        }

        [HttpGet]
        public ActionResult AddCampaign()
        {
            return View(new CampaignModel());
        }

        [HttpGet]
        public ActionResult AddWebsites()
        {
            return View(new WebScrapModel());
        }

        
        [HttpGet]
        public ActionResult AddSMJobs()
        {
            return View(new SMJobModel());
        }

        [HttpGet]
        public ActionResult AddAnnouncement()
        {
            return View(new AnnouncementModel());
        }

        [HttpPost]
        public ActionResult AddAnnouncement(AnnouncementModel model)
        {
            if (ModelState.IsValid)
            {
                Announcement announcement = new Announcement()
                {

                    Type = model.Type,
                    CountryId = model.CountryId,
                    Subject = model.Subject,
                    Body = model.Body
                };
                List<long> userlist = new List<long>();
                long announcementId = 0;
                string subject = string.Empty;
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    announcementId = Convert.ToInt64(dataHelper.Add<Announcement>(announcement, User.Username));
                    subject = model.Subject;

                }
                Send1();
                TempData["UpdateData"] = "Announcement added successfully!";
            }
            return Redirect("/Admin/Announcements");
        }
        [HttpGet]
        public ActionResult AddUpdates()
        {
            return View(new AnnouncementModel());
        }

        [HttpPost]
        public ActionResult AddUpdates(AnnouncementModel model)
        {
            if (ModelState.IsValid)
            {
                Announcement announcement = new Announcement()
                {

                    Type = model.Type,
                    CountryId = model.CountryId,
                    Subject = model.Subject,
                    Body = model.Body
                };
                List<long> userlist = new List<long>();
                long announcementId = 0;
                string subject = string.Empty;
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    announcementId = Convert.ToInt64(dataHelper.Add<Announcement>(announcement, User.Username));
                    subject = model.Subject;

                }
                Send1();
                TempData["UpdateData"] = "Updates added successfully!";
            }
            return Redirect("/Admin/Updates");
        }
        [HttpPost]
        public ContentResult GetUserCount(string city, string country, string state, int type)
        {

            if(type==19)
            {
                int count = 4;
                return Content(Convert.ToString(count));

            }
            else if(type==12&city== "Birur" & country=="india")
                {
                int count = 16;
                return Content(Convert.ToString(count));
            }
            else
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    var result = dataHelper.Get<UserProfile>().Where(x => x.City == city & x.Type == type);
                    int count = result.Count();
                    return Content(Convert.ToString(count));
                }
            }
           
        }
        [HttpPost]
        public ContentResult GetStudentCount()
        {
            int type = 13;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<UserProfile>().Where(x =>x.Type == type);
                int count = result.Count();
                return Content(Convert.ToString(count));
            }
        }
        [HttpPost]
        public ContentResult GetCompanyCount()
        {
            int type = 5;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<UserProfile>().Where(x => x.Type == type);
                int count = result.Count();
                return Content(Convert.ToString(count));
            }
        }
        [HttpPost]
        public ContentResult GetJobseekerCount()
        {
            int type = 4;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<UserProfile>().Where(x => x.Type == type);
                int count = result.Count();
                return Content(Convert.ToString(count));
            }
        }
        [HttpPost]
        public ContentResult GetInstituteCount()
        {
            int type = 12;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<UserProfile>().Where(x => x.Type == type);
                int count = result.Count();
                return Content(Convert.ToString(count));
            }
        }
        [HttpPost]
        public ContentResult GetRecruiterCount()
        {
            int type = 14;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<UserProfile>().Where(x => x.Type == type);
                int count = result.Count();
                return Content(Convert.ToString(count));
            }
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


        [HttpPost]
        public ActionResult AddWebsites(long? countryId, WebScrapModel model)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            if (ModelState.IsValid)
            {
                UserInfoEntity uinfo = _service.Get(User.Id);

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = string.Format("insert into  WebsiteList" +
                                "(CountryName,CompanyName,Website,Email,CreatedBy,DateCreated,Name) " +
                                "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                                model.CountryName, model.CompanyName, model.Website, model.Email, uinfo.FirstName, DateTime.Now, model.Name);
                            command.ExecuteNonQuery();
                        }

                        conn.Close();

                        string from = "notify@joblisting.com";

                        //string templates = ConfigurationManager.AppSettings["Template"];
                        string baseUrl = "https://www.joblisting.com";
                        string postmail = "master@joblisting.com";
                        string postpassword = "052e14c947dbb70b9d04776344b6d88e-1f1bd6a9-d1f0adb4";
                        string body = string.Empty;

                        MimeMessage mail1 = new MimeMessage();

                        //using (SmtpClient client1 = new SmtpClient())
                        //{

                        //    try
                        //    {

                        //        var reader1 = new StreamReader(Server.MapPath("~/Templates/Mail/EmailWebsite.html"));
                        //        if (reader1 != null)
                        //        {
                        //            string ebody1 = reader1.ReadToEnd();
                        //            ebody1 = ebody1.Replace("@@receiver", string.Format("{0}", uinfo.FirstName));
                        //            ebody1 = ebody1.Replace("@@subject", "Website Details from WebScraping Team");
                        //            ebody1 = ebody1.Replace("@@viewurl", "These are details of Website");
                        //            ebody1 = ebody1.Replace("@@v1", string.Format("Company Name :: {0}", model.CompanyName));
                        //            ebody1 = ebody1.Replace("@@d2", string.Format("Country Name ::::: {0}", model.CountryName));
                        //            ebody1 = ebody1.Replace("@@s3", string.Format("Company Url :::: {0}", model.Website));

                        //            mail1.From.Clear();
                        //            mail1.To.Clear();
                        //            mail1.From.Add(new MailboxAddress("Joblisting", from));
                        //            mail1.To.Add(new MailboxAddress("Excited User", model.Email));
                        //            mail1.Subject = "Website Details from WebScraping Team";
                        //            mail1.Body = new TextPart("html")
                        //            {
                        //                Text = ebody1,
                        //            };
                        //            try
                        //            {

                        //                // XXX - Should this be a little different?
                        //                client1.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        //                client1.Connect("smtp.mailgun.org", 587, false);
                        //                client1.AuthenticationMechanisms.Remove("XOAUTH2");
                        //                client1.Authenticate(postmail, postpassword);

                        //                client1.Send(mail1);
                        //                client1.Disconnect(true);

                        //            }
                        //            catch (Exception ex)
                        //            {
                        //                SendEx(ex);
                        //            }

                        //        }



                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        SendEx(ex);
                        //    }



                        //}


                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        SendEx(ex);
                    }
                }


                TempData["UpdateData"] = "Website added successfully!";
            }
            return Redirect("/Admin/Websites");
        }

        [HttpGet]
        public ActionResult salescalllog()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult salescalllog(salescall model)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = string.Format("insert into  calllog (CompanyName,associatename,Phone,Email,Name,TimeOfCall,DofCall,CallBackNo,ActiontobeTaken,Notes) values('" + model.CompanyName + "','" + model.associatename + "','" + model.Phone + "','" + model.Email + "','" + model.Name + "','" + model.TimeOfCall + "','" + model.DofCall + "','" + model.CallBackNo + "','" + model.ActiontobeTaken + "','" + model.Notes + "')", conn);
                        command.ExecuteNonQuery();
                        TempData["UpdateData"] = "Call Log  added successfully!";
                    }
                }
                return RedirectToAction("salescall", "Admin");
            }
            else
            {
                if(model.associatename==null)
                {
                    TempData["error"] = "Associate name is required";
                }
                else if (model.CompanyName == null)
                {
                    TempData["error"] = "CompanyName name is required";
                }
                else if (model.Name == null)
                {
                    TempData["error"] = "Name  is required";
                }
                else if (model.TimeOfCall == null)
                {
                    TempData["error"] = "Time Of call  is required";
                }
                else if (model.DofCall == null)
                {
                    TempData["error"] = "Duration Of call  is required";
                }
                else if (model.Phone == null)
                {
                    TempData["error"] = "Phone Number  is required";
                }
                else if (model.Email == null)
                {
                    TempData["error"] = "Email  is required";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult salescall(salescall model,int?i)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            
                List<salescall> calllist = new List<salescall>();
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("Select * from calllog", conn);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    conn.Open();
                    sd.Fill(dt);
                    conn.Close();

                    foreach (DataRow dr in dt.Rows)
                    {
                        calllist.Add(
                        new salescall
                        {    //CompanyName,associatename,Phone,Email,Name,TimeOfCall,DofCall,CallBackNo,ActiontobeTaken,Notes
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                            associatename = Convert.ToString(dr["associatename"]),
                            Phone = Convert.ToString(dr["Phone"]),
                            Email = Convert.ToString(dr["Email"]),
                            Name = Convert.ToString(dr["Name"]),
                            TimeOfCall = Convert.ToString(dr["TimeOfCall"]),
                            DofCall = Convert.ToString(dr["DofCall"]),
                            CallBackNo = Convert.ToString(dr["CallBackNo"]),
                            ActiontobeTaken = Convert.ToString(dr["ActiontobeTaken"]),
                            Notes = Convert.ToString(dr["Notes"])
                        });
                    }
                }
                ViewBag.Latestcompanies = calllist.ToPagedList(i ?? 1, 10);
            
            //var data = calllist;


            return View();
        }
        [HttpPost]
        public ActionResult salescall(int i = 1, string searchstr1="", string searchstr2 = "")
        {
            int rows = 0;
            int pageSize = 10;

            SearchCompanyModel model = new SearchCompanyModel();

            var data = JobService.Instance.GetCompanies().Where(x => x.Company == searchstr1 || x.Country == searchstr2);

            //var data = JobService.Instance.GetCompanies();
            ViewBag.Latestcompanies = data;
            rows = data.Count();
            var pageModel = new Pager(data.FirstOrDefault().TotalRow, i, 15);
            model.DataSize = pageModel.PageSize;

            model.CurrentPage = pageModel.CurrentPage;
            model.EndPage = pageModel.EndPage;
            model.StartPage = pageModel.StartPage;
            model.CurrentPage = pageModel.CurrentPage;
            model.TotalPages = pageModel.TotalPages;
            model.PageSize = pageModel.PageSize;
            return View(model);

        }

        [HttpGet]
        [Authorize(Roles = "Administrator, SuperUser")]
        public ActionResult AddJob(string redirectUrl)
        {
            //if (string.IsNullOrEmpty(UserInfo.Address) && string.IsNullOrEmpty(UserInfo.Mobile))
            //{
            //    return RedirectToAction("ListJob1Error", "Admin", new { returnUrl = "/listjob1" });
            //}
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("http://209.105.231.183:5002/post/");
            //    //HTTP GET
            //    var responseTask = client.GetAsync("student");
            //    responseTask.Wait();

            //}
            JobListingModel model = new JobListingModel();
            if (!string.IsNullOrEmpty(Request.QueryString["sid"]))
            {
                Guid id = new Guid(Request.QueryString["sid"]);
                string jobData = DomainService.Instance.ReadData(id);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                JobPostModel jpe = serializer.Deserialize<JobPostModel>(jobData);
                ListJob1C jpe1 = serializer.Deserialize<ListJob1C>(jobData);
                model.CompanyName = jpe1.CompanyName;
                model.AboutCompany = jpe1.AboutCompany;
                model.job_id = Convert.ToInt16(jpe1.job_id);
                model.Title = jpe.Title;
                model.CategoryId = jpe.CategoryId;
                //model.SpecializationId = jpe.SpecializationId;
                model.CountryId = jpe.CountryId;
                model.StateId = jpe.StateId;
                model.City = jpe.City;
                model.Zip = jpe.Zip;
                model.Description = jpe.Description;
                model.Summary = jpe.Summary;
                //model.Requirements = jpe.Requirements;
                //model.Responsibilities = jpe.Responsibilities;
                model.MinimumExperience = (byte?)jpe.MinimumExperience;
                model.MaximumExperience = (byte?)jpe.MaximumExperience;
                model.SalaryCurrency = jpe.Currency;
                model.MinimumSalary = jpe.MinimumSalary;
                model.MaximumSalary = jpe.MaximumSalary;
                model.MinimumAge = (int?)jpe.MinimumAge;
                model.MaximumAge = (int?)jpe.MaximumAge;
                model.EmploymentType = (long)jpe.EmployementTypeId;
                model.QualificationId = (long?)jpe.Qualification;
                model.Noticeperiod = jpe.Noticeperiod;
                model.Optionalskills = jpe.Optionalskills;
                model.Distribute = 1;
                DomainService.Instance.RemoveData(id);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult AddJob(JobListingModel model)
        {

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            if (User != null)
            {
               
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    var employer = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                    var permalink = model.Title;

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
                    var job1 = new ListJob1C();
                    job1.CompanyName = model.CompanyName;
                    job1.AboutCompany = model.AboutCompany;
                    job.Title = model.Title;
                    job.Distribute = Convert.ToBoolean(model.Distribute);
                    string description = Sanitizer.GetSafeHtmlFragment(model.Description);
                    description = description.RemoveEmails();
                    description = description.RemoveNumbers();
                    job.Description = description.RemoveWebsites();

                    string summary = model.Summary;
                    summary = summary.RemoveEmails();
                    summary = summary.RemoveNumbers();
                    summary = summary.RemoveWebsites();
                    job.Summary = summary;

                    
                    job.IsFeaturedJob = model.IsFeaturedJob;
                    job.CategoryId = model.CategoryId;
                   
                        job.CountryId = model.CountryId;
                    //}
                    //job.CountryId = model.CountryId;
                    //job.DateOfBirth = model.DateOfBirth1;
                    //job.AdharNumber = model.Adharnumber;
                    job.CompanyName1 = model.CompanyName;
                    job.OptionalSkills = model.Optionalskills;
                    job.NoticePeriod = model.Noticeperiod;
                    job.StateId = model.StateId;
                    job.City = model.City;
                    job.Zip = model.Zip;
                    job.EmploymentTypeId = model.EmploymentType;
                    job.QualificationId = model.QualificationId;
                    job.MinimumAge = (byte?)model.MinimumAge;
                    job.MaximumAge = (byte?)model.MaximumAge;
                    job.MinimumExperience = (byte)model.MinimumExperience;
                    job.MaximumExperience = (byte)model.MaximumExperience;
                    job.MinimumSalary = model.MinimumSalary;
                    job.MaximumSalary = model.MaximumSalary;
                    job.Currency = model.SalaryCurrency;
                    job.PublishedDate = DateTime.Now;
                    job.ClosingDate = DateTime.Now.AddMonths(1);
                    job.PermaLink = permalink;
                    job.EmployerId = employer.UserId;
                    job.IsActive = true;
                    job.IsDeleted = false;
                    job.IsPostedOnTwitter = false;
                    job.InEditMode = false;
                    job.Distribute = (model.Distribute == 1);
                    job.IsPaid = job.IsPaid;
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
                            body = body.Replace("@@employer", model.CompanyName);
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
                           // UserInfoEntity uinfo = _service.Get(User.Id);
                            var recipients = new List<Recipient>();
                            MailAddress senderEmail=null;

                            if (User.Username == "gowthami@accuracy.com.sg" || User.Username == "naveena@accuracy.com.sg" || User.Username == "sarika123@accuracy.com.sg" || User.Username == "vanshika@accuracy.com.sg")
                            {
                                if(User.Username == "gowthami@accuracy.com.sg")
                                {
                                    
                                         senderEmail = new MailAddress("gowthami@joblisting.com", "gowthami");
                                        var receiverEmail = new MailAddress("Doli.chauhan@accuracy.com.sg", "doli");
                                        var password = "Vguestinn@123"; //I used here APP password 

                                        var subject1 = subject;
                                        var body1 = body;
                                    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                                    {
                                        Host = "relay-hosting.secureserver.net",
                                        Port = 25,
                                        UseDefaultCredentials = false,
                                        Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
                                    })
                                    using (var message = new MailMessage(senderEmail, receiverEmail)
                                    {
                                        Subject = subject,
                                        Body = body,
                                        IsBodyHtml = true
                                    }) ;
                                            //try
                                            //{
                                            //    //client.Send(message);
                                            //}
                                            //catch
                                            //{
                                            //    Exception ex;
                                            //}

                                }
                                else if(User.Username == "naveena@accuracy.com.sg")
                                {
                                     senderEmail = new MailAddress("gowthami@joblisting.com", "gowthami");
                                    var receiverEmail = new MailAddress("doli.chauhan@accuracy.com.sg", "doli");
                                    var password = "Vguestinn@123"; //I used here APP password 

                                    var subject1 = subject;
                                    var body1 = body;
                                    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                                    {
                                        Host = "relay-hosting.secureserver.net",
                                        Port = 25,
                                        UseDefaultCredentials = false,
                                        Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
                                    })
                                    using (var message = new MailMessage(senderEmail, receiverEmail)
                                    {
                                        Subject = subject,
                                        Body = body,
                                        IsBodyHtml = true
                                    }) ;
                                        //try
                                        //{
                                        //    client.Send(message);
                                        //}
                                        //catch
                                        //{
                                        //    Exception ex;
                                        //}
                                    
                                }
                                else if (User.Username == "sarika123@accuracy.com.sg")
                                {
                                     senderEmail = new MailAddress("nagasarika@joblisting.com", "nagasarika");
                                    var receiverEmail = new MailAddress("doli.chauhan@accuracy.com.sg", "doli");
                                    var password = "Vguestinn@123"; //I used here APP password 

                                    var subject1 = subject;
                                    var body1 = body;
                                    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                                    {
                                        Host = "relay-hosting.secureserver.net",
                                        Port = 25,
                                        UseDefaultCredentials = false,
                                        Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
                                    })
                                    using (var message = new MailMessage(senderEmail, receiverEmail)
                                    {
                                        Subject = subject,
                                        Body = body,
                                        IsBodyHtml = true
                                    }) ;
                                        //try
                                        //{
                                        //    client.Send(message);
                                        //}
                                        //catch
                                        //{
                                        //    Exception ex;
                                        //}
                                   
                                }
                                else if (User.Username == "vanshika@accuracy.com.sg")
                                {
                                   
                                     senderEmail = new MailAddress("vanshika@joblisting.com", "vanshika");
                                    var receiverEmail = new MailAddress("doli.chauhan@accuracy.com.sg", "doli");
                                    var password = "Vguestinn@123"; //I used here APP password 

                                    var subject1 = subject;
                                    var body1 = body;
                                    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                                    {
                                        Host = "relay-hosting.secureserver.net",
                                        Port = 25,
                                        UseDefaultCredentials = false,
                                        Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
                                    })
                                    using (var message = new MailMessage(senderEmail, receiverEmail)
                                    {
                                        Subject = subject,
                                        Body = body,
                                        IsBodyHtml = true
                                    }) ;
                                        //try
                                        //{
                                        //    client.Send(message);
                                        //}
                                        //catch
                                        //{
                                        //    Exception ex;
                                        //}
                                }

                              
                            }
                            else if(User.Username == "anshikagupta@accuracy.com.sg"|| User.Username == "vani123@accuracy.com.sg" || User.Username == "baba@accuracy.com.sg" || User.Username == "pallavi@accuracy.com.sg" || User.Username == "Shreyag@accuracy.com.sg"||User.Username== "shreyag1234@accuracy.com.sg")
                            {
                                if (User.Username == "anshikagupta@accuracy.com.sg")
                                {

                                     senderEmail = new MailAddress("anshika@joblisting.com", "gowthami");
                                    var receiverEmail = new MailAddress("tasnim@accuracy.com.sg", "tasnim");
                                    var password = "Vguestinn@123"; //I used here APP password 

                                    var subject1 = subject;
                                    var body1 = body;
                                    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                                    {
                                        Host = "relay-hosting.secureserver.net",
                                        Port = 25,
                                        UseDefaultCredentials = false,
                                        Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
                                    })
                                    using (var message = new MailMessage(senderEmail, receiverEmail)
                                    {
                                        Subject = subject,
                                        Body = body,
                                        IsBodyHtml = true
                                    }) ;
                                        //try
                                        //{
                                        //    client.Send(message);
                                        //}
                                        //catch
                                        //{
                                        //    Exception ex;
                                        //}

                                }
                                else if (User.Username == "baba@accuracy.com.sg")
                                {
                                     senderEmail = new MailAddress("babafakruddin@joblisting.com", "babafakruddin");
                                    var receiverEmail = new MailAddress("tasnim@accuracy.com.sg", "tasnim");
                                    var password = "Vguestinn@123"; //I used here APP password 

                                    var subject1 = subject;
                                    var body1 = body;
                                    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                                    {
                                        Host = "relay-hosting.secureserver.net",
                                        Port = 25,
                                        UseDefaultCredentials = false,
                                        Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
                                    })
                                    using (var message = new MailMessage(senderEmail, receiverEmail)
                                    {
                                        Subject = subject,
                                        Body = body,
                                        IsBodyHtml = true
                                    }) ;
                                        //try
                                        //{
                                        //    client.Send(message);
                                        //}
                                        //catch
                                        //{
                                        //    Exception ex;
                                        //}
                                   
                                }
                                else if (User.Username == "vani123@accuracy.com.sg")
                                {

                                    senderEmail = new MailAddress("vani@joblisting.com", "vani");
                                    var receiverEmail = new MailAddress("tasnim@accuracy.com.sg", "tasnim");
                                    var password = "Vguestinn@123"; //I used here APP password 

                                    var subject1 = subject;
                                    var body1 = body;
                                    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                                    {
                                        Host = "relay-hosting.secureserver.net",
                                        Port = 25,
                                        UseDefaultCredentials = false,
                                        Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
                                    })
                                    using (var message = new MailMessage(senderEmail, receiverEmail)
                                    {
                                        Subject = subject,
                                        Body = body,
                                        IsBodyHtml = true
                                    }) ;
                                        

                                }
                                else if (User.Username == "anilkumar@joblisting.com")
                                {
                                     senderEmail = new MailAddress("anilkumar@joblisting.com", "anilkumar");
                                    var receiverEmail = new MailAddress("tasnim@accuracy.com.sg", "tasnim");
                                    var password = "Vguestinn@123"; //I used here APP password 

                                    var subject1 = subject;
                                    var body1 = body;
                                    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                                    {
                                        Host = "relay-hosting.secureserver.net",
                                        Port = 25,
                                        UseDefaultCredentials = false,
                                        Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
                                    })
                                    using (var message = new MailMessage(senderEmail, receiverEmail)
                                    {
                                        Subject = subject,
                                        Body = body,
                                        IsBodyHtml = true
                                    }) ;

                                }

                                else if (User.Username == "Shreyag@accuracy.com.sg" || User.Username == "haris@joblisting.com"  || User.Username== "shreyag1234@accuracy.com.sg")
                                {
                                     senderEmail = new MailAddress("haris@joblisting.com", "haris");
                                    var receiverEmail = new MailAddress("tasnim@accuracy.com.sg", "tasnim");
                                    var password = "Vguestinn@123"; //I used here APP password 

                                    var subject1 = subject;
                                    var body1 = body;
                                    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                                    {
                                        Host = "relay-hosting.secureserver.net",
                                        Port = 25,
                                        UseDefaultCredentials = false,
                                        Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
                                    })
                                    using (var message = new MailMessage(senderEmail, receiverEmail)
                                    {
                                        Subject = subject,
                                        Body = body,
                                        IsBodyHtml = true
                                    }) ;

                                }

                                
                               
                            }
                            else if (User.Username == "lakshmip@accuracy.com.sg" || User.Username == "denise@accuracy.com.sg" || User.Username == "dianna@accuracy.com.sg" )
                            {
                                if (User.Username == "lakshmip@accuracy.com.sg")
                                {

                                     senderEmail = new MailAddress("lakshmi@joblisting.com", "lakshmi");
                                    var receiverEmail = new MailAddress("sandhya@accuracy.com.sg", "sandhya");
                                    var password = "Vguestinn@123"; //I used here APP password 

                                    var subject1 = subject;
                                    var body1 = body;
                                    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                                    {
                                        Host = "relay-hosting.secureserver.net",
                                        Port = 25,
                                        UseDefaultCredentials = false,
                                        Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
                                    })
                                    using (var message = new MailMessage(senderEmail, receiverEmail)
                                    {
                                        Subject = subject,
                                        Body = body,
                                        IsBodyHtml = true
                                    })
                                        ;

                                }
                                else if (User.Username == "denise@accuracy.com.sg")
                                {
                                     senderEmail = new MailAddress("denise@joblisting.com", "denise");
                                    var receiverEmail = new MailAddress("sandhya@accuracy.com.sg", "sandhya");
                                    var password = "Vguestinn@123"; //I used here APP password 

                                    var subject1 = subject;
                                    var body1 = body;
                                    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                                    {
                                        Host = "relay-hosting.secureserver.net",
                                        Port = 25,
                                        UseDefaultCredentials = false,
                                        Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
                                    })
                                    using (var message = new MailMessage(senderEmail, receiverEmail)
                                    {
                                        Subject = subject,
                                        Body = body,
                                        IsBodyHtml = true
                                    }) ;

                                }
                                else if (User.Username == "dianna@accuracy.com.sg")
                                {
                                     senderEmail = new MailAddress("pallavi@joblisting.com", "pallavi");
                                    var receiverEmail = new MailAddress("sandhya@accuracy.com.sg", "sandhya");
                                    var password = "Vguestinn@123"; //I used here APP password 

                                    var subject1 = subject;
                                    var body1 = body;
                                    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                                    {
                                        Host = "relay-hosting.secureserver.net",
                                        Port = 25,
                                        UseDefaultCredentials = false,
                                        Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
                                    })
                                    using (var message = new MailMessage(senderEmail, receiverEmail)
                                    {
                                        Subject = subject,
                                        Body = body,
                                        IsBodyHtml = true
                                    }) ;

                                }

                              
                              
                               
                            }
                            else if (User.Username == "ganeshr@joblisting.com")
                            {

                                senderEmail = new MailAddress("ganeshr@joblisting.com", "ganeshr");
                                var receiverEmail = new MailAddress("pratapchandran@accuracy.com.sg", "pratap");
                                var password = "Vguestinn@123"; //I used here APP password 

                                var subject1 = subject;
                                var body1 = body;
                                using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                                {
                                    Host = "relay-hosting.secureserver.net",
                                    Port = 25,
                                    UseDefaultCredentials = false,
                                    Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
                                })
                                using (var message = new MailMessage(senderEmail, receiverEmail)
                                {
                                    Subject = subject,
                                    Body = body,
                                    IsBodyHtml = true
                                }) ;

                            }

                            else if (User.Username == "vani123@accuracy.com.sg") {

                                 senderEmail = new MailAddress("vani@joblisting.com", "vani");
                                var receiverEmail = new MailAddress("deepti@accuracy.com.sg", "deepti");
                                var password = "Vguestinn@123"; //I used here APP password 

                                var subject1 = subject;
                                var body1 = body;
                                using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                                {
                                    Host = "relay-hosting.secureserver.net",
                                    Port = 25,
                                    UseDefaultCredentials = false,
                                    Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
                                })
                                using (var message = new MailMessage(senderEmail, receiverEmail)
                                {
                                    Subject = subject,
                                    Body = body,
                                    IsBodyHtml = true
                                }) ;
                              
                            }
                           

                            List<int> typeList = new List<int>() { (int)SecurityRoles.Administrator, (int)SecurityRoles.SuperUser };
                         //   SendMail1(subject, senderEmail, body);

                        }
                        
                        TempData["UpdateData"] = string.Format("{0} job has been submitted successfully. It is in approval process, we will inform you once it is approved!", job.Title);
                    }
                    //if (DomainService.Instance.PaymentProcessEnabled())
                    //    {
                    //        if (Request.QueryString["le"] != null && Request.QueryString["le"] == "1")
                    //        {
                    //            return RedirectToAction("Select", "Package", new { returnUrl = "/Employer/ListJobError?returnUrl=/listjob", RedirectUrl = "/employer/index", type = "J", sessionId = id, countryId = model.CountryId });
                    //        }
                    //        else
                    //        {
                    //            return RedirectToAction("Select", "Package", new { returnUrl = "/Employer/Index", RedirectUrl = "/employer/index", type = "J", sessionId = id, countryId = model.CountryId });
                    //        }
                    //    }
                    
                }
                return RedirectToAction("Jobsbycompanyall");
            }
            return View(new JobListingModel());
           // return RedirectToAction("Jobsbycompanyall");
        }
        //public void SendMail1(string su,MailAddress mail,string ms)
        //{
        //    var senderEmail = mail;
        //    var receiverEmail = new MailAddress("chetanb@accuracy.com.sg", "chetanb");
        //    var password = "Vguestinn@123"; //I used here APP password 

        //    var subject1 = su;
        //    var body1 = ms;
        //    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
        //    {
        //        Host = "relay-hosting.secureserver.net",
        //        Port = 25,
        //        UseDefaultCredentials = false,
        //        Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
        //    })
        //    using (var message = new MailMessage(senderEmail, receiverEmail)
        //    {
        //        Subject = su,
        //        Body = ms,
        //        IsBodyHtml = true
        //    })
        //        try
        //        {
        //            client.Send(message);
        //        }
        //        catch
        //        {
        //            Exception ex;
        //        }

        //}
        //        public void SendMail1(string subject, MailAddress senderEmail, string message)
        //#pragma warning restore CS0246 // The type or namespace name 'Recipient' could not be found (are you missing a using directive or an assembly reference?)
        //        {
        //            string postmail = ConfigurationManager.AppSettings["postmail"];
        //            string postpassword = ConfigurationManager.AppSettings["postpassword"];
        //            var msg = new MimeMessage();
        //            var strfrom = new MailboxAddress("Joblisting", ConfigurationManager.AppSettings["FromEmailAddress"]);
        //            if (recipents.Count > 0)
        //            {
        //                foreach (var recipient in recipents)
        //                {
        //                    try
        //                    {
        //                        var addr = new MailboxAddress("", recipient.Email);
        //                        if (addr.Address == recipient.Email)
        //                        {
        //                            switch (recipient.Type)
        //                            {
        //                                case RecipientTypes.TO:
        //                                    if (recipient.Email.ToLower().Contains("admin_"))
        //                                    {
        //                                        msg.To.Add(new MailboxAddress("Joblisting", "admin@joblisting.com"));
        //                                    }
        //                                    else
        //                                    {
        //                                        msg.To.Add(new MailboxAddress(recipient.DisplayName, recipient.Email));
        //                                    }
        //                                    break;
        //                                case RecipientTypes.CC:
        //                                    if (recipient.Email.ToLower().Contains("admin_"))
        //                                    {
        //                                        msg.Cc.Add(new MailboxAddress("Joblisting", "admin@joblisting.com"));
        //                                    }
        //                                    else
        //                                    {
        //                                        msg.Cc.Add(new MailboxAddress(recipient.DisplayName, recipient.Email));
        //                                    }
        //                                    break;
        //                                case RecipientTypes.BCC:
        //                                    if (recipient.Email.ToLower().Contains("admin_"))
        //                                    {
        //                                        msg.Bcc.Add(new MailboxAddress("Joblisting", "admin@joblisting.com"));
        //                                    }
        //                                    else
        //                                    {
        //                                        msg.Bcc.Add(new MailboxAddress(recipient.DisplayName, recipient.Email));
        //                                        msg.Cc.Add(new MailboxAddress("Joblisting", "admin@joblisting.com"));
        //                                    }
        //                                    break;
        //                            }
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        //using (var oSmtpClient = new SmtpClient())
        //                        //{
        //                        //    string from = ConfigurationManager.AppSettings["FromEmailAddress"];
        //                        //    string[] toList = ConfigurationManager.AppSettings["ServiceNotifyEmail"].Split(',');
        //                        //    MimeMessage exmsg = new MimeMessage();
        //                        //    exmsg.From.Add(new MailboxAddress("Joblisting", from));
        //                        //    foreach (string to in toList)
        //                        //    {
        //                        //        exmsg.To.Add(new MailboxAddress("", to));
        //                        //    }
        //                        //    exmsg.Subject = "Invalid Email Address";
        //                        //    exmsg.Body = new TextPart("html") { Text = string.Format("<h3>{0}</h3>", ex.Message) + string.Format("{0}", ex.StackTrace) };
        //                        //    oSmtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

        //                        //    oSmtpClient.Connect("smtp.mailgun.org", 587, false);
        //                        //    oSmtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
        //                        //    oSmtpClient.Authenticate(postmail, postpassword);

        //                        //    oSmtpClient.Send(exmsg);
        //                        //    oSmtpClient.Disconnect(true);
        //                        //}
        //                    }
        //                }
        //                //using (var oSmtpClient = new SmtpClient())
        //                //{
        //                //    if (msg.To.Count > 0)
        //                //    {
        //                //        msg.From.Add(strfrom);
        //                //        msg.Subject = subject;
        //                //        msg.Body = new TextPart("html") { Text = message };
        //                //        oSmtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

        //                //        oSmtpClient.Connect("smtp.mailgun.org", 587, false);
        //                //        oSmtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
        //                //        oSmtpClient.Authenticate(postmail, postpassword);

        //                //        oSmtpClient.Send(msg);
        //                //        oSmtpClient.Disconnect(true);
        //                //    }
        //                //}
        //            }
        //        }

        public ActionResult ListJob2(string country, int i = 1)
        {
            SearchJobModel model = new SearchJobModel();
            string location = "";

            //string name = RegionInfo.CurrentRegion.DisplayName;
            string countryName = (string)Session["cb"];
            string city = "";
            ViewBag.Title = "Search Jobs | Apply Jobs | Joblisting.com";
            ViewBag.Description = "Search jobs and apply for perfectly matching job according your requirements";
            ViewBag.Keywords = "Joblisting, Job Listing, List Jobs, Search Jobs, Latest Jobs, Accounting Jobs, I.T. Jobs, Healthcare Jobs, Sales Jobs, Government Jobs, Banking Jobs, Engineering Jobs";
            // var data = JobService.Instance.GetLatestJobs2(country == "" ? "" : country, "", i);
            var data = JobService.Instance.GetLatestJobs22(countryName == "" ? "" : countryName, location, city, i);
            //var data = JobService.Instance.GetLatestJobs2(cn, re, city, i);

            ViewBag.LatestJobs = data;
            var pageModel = new Pager(data.FirstOrDefault().TotalRow, i, 15);
            model.DataSize = pageModel.PageSize;
            model.Where = country;
            model.CurrentPage = pageModel.CurrentPage;
            model.EndPage = pageModel.EndPage;
            model.StartPage = pageModel.StartPage;
            model.CurrentPage = pageModel.CurrentPage;
            model.TotalPages = pageModel.TotalPages;
            model.PageSize = pageModel.PageSize;
            ViewBag.mess = TempData["Mess"];
            return View(model);
        }

     
        public ActionResult listthisjob(long Id)
        {
            int CountryId = (int)Session["CountryId"];
            long empid = (long)Session["EmpId"];
            string createdby = "Recruiter";
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SearchJobModel model = new SearchJobModel();          
            ViewBag.LatestJob2 = JobService.Instance.GetLatestJobs22(Id);
            foreach (LatestJob latestJob in ViewBag.LatestJob2)
            {
                DateTime dateTime = latestJob.DateCreated;
                DateTime ClosingDate1 = dateTime.AddDays(+30);

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    

                    using (SqlCommand cmd = new SqlCommand("insert into jobs (CountryId,EmployerId,Title,Description,PubLishedDate,ClosingDate,Summary,DateCreated,CreatedBy) values('" + CountryId+"' ,'" + empid + "','" + latestJob.Title + "','" + latestJob.Description + "','" + latestJob.DateCreated + "','" + ClosingDate1 + "','" +latestJob.Title+ "','" + latestJob.DateCreated + "','" + createdby + "')", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        TempData["Mess"]="Job Listed Successfully";
                    }

                }
            }
            
            return RedirectToAction("listjob2", "Admin");
        }

        [UrlPrivilegeFilter]
            public ActionResult ListJob1()
        {
            //if (UserInfo != null)
            //{
            //    if (UserInfo.IsConfirmed == false)
            //    {
            //        return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
            //    }

            //    if (UserInfo.Role != SecurityRoles.RecruitmentAgency)
            //    {
            //        //return Redirect(UserInfo.PermaLink);
            //        return View();
            //    }             
            //}

            if (string.IsNullOrEmpty(UserInfo.Address) && string.IsNullOrEmpty(UserInfo.Mobile))
            {
                return RedirectToAction("ListJob1Error", "Admin", new { returnUrl = "/listjob1" });
            }

            JobListingModel model = new JobListingModel();
            if (!string.IsNullOrEmpty(Request.QueryString["sid"]))
            {
                Guid id = new Guid(Request.QueryString["sid"]);
                string jobData = DomainService.Instance.ReadData(id);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                JobPostModel jpe = serializer.Deserialize<JobPostModel>(jobData);
                ListJob1C jpe1 = serializer.Deserialize<ListJob1C>(jobData);
                model.CompanyName = jpe1.CompanyName;
                model.AboutCompany = jpe1.AboutCompany;
                model.job_id = Convert.ToInt16(jpe1.job_id);
                model.Title = jpe.Title;
                model.CategoryId = jpe.CategoryId;
                model.SpecializationId = jpe.SpecializationId;
                model.CountryId = jpe.CountryId;
                model.StateId = jpe.StateId;
                model.City = jpe.City;
                model.Zip = jpe.Zip;
                model.Description = jpe.Description;
                model.Summary = jpe.Summary;
                model.Requirements = jpe.Requirements;
                model.Responsibilities = jpe.Responsibilities;
                model.MinimumExperience = (byte?)jpe.MinimumExperience;
                model.MaximumExperience = (byte?)jpe.MaximumExperience;
                model.SalaryCurrency = jpe.Currency;
                model.MinimumSalary = jpe.MinimumSalary;
                model.MaximumSalary = jpe.MaximumSalary;
                model.MinimumAge = (int?)jpe.MinimumAge;
                model.MaximumAge = (int?)jpe.MaximumAge;
                model.EmploymentType = (long)jpe.EmployementTypeId;
                model.QualificationId = (long?)jpe.Qualification;
                model.Noticeperiod = jpe.Noticeperiod;
                model.Optionalskills = jpe.Optionalskills;
                model.Distribute = 1;
                DomainService.Instance.RemoveData(id);
            }
            return View(model);
        }

       
        public ActionResult PostListJob1(int packageId, long countryId, string returnUrl)
        {
            JobListingModel model = new JobListingModel();
            model.PackageId = packageId;
            model.ReturnUrl = returnUrl;
            Package entity = helper.GetPackage(packageId);
            model.CountryId = countryId;
            if (!string.IsNullOrEmpty(Request.QueryString["sid"]))
            {
                Guid id = new Guid(Request.QueryString["sid"]);
                string jobData = DomainService.Instance.ReadData(id);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                JobPostModel jpe = serializer.Deserialize<JobPostModel>(jobData);
                ListJob1C jpe1 = serializer.Deserialize<ListJob1C>(jobData);
                model.CompanyName = jpe1.CompanyName;
                model.AboutCompany = jpe1.AboutCompany;
                model.job_id = Convert.ToInt16(jpe1.job_id);

                model.Title = jpe.Title;
                model.CategoryId = jpe.CategoryId;
                model.SpecializationId = jpe.SpecializationId;
                model.CountryId = jpe.CountryId;
                model.StateId = jpe.StateId;
                model.City = jpe.City;
                model.Zip = jpe.Zip;
                model.Description = jpe.Description;
                model.Summary = jpe.Summary;
                model.Requirements = jpe.Requirements;
                model.Responsibilities = jpe.Responsibilities;
                model.MinimumExperience = (byte?)jpe.MinimumExperience;
                model.MaximumExperience = (byte?)jpe.MaximumExperience;
                model.SalaryCurrency = jpe.Currency;
                model.MinimumSalary = jpe.MinimumSalary;
                model.MaximumSalary = jpe.MaximumSalary;
                model.MinimumAge = (int?)jpe.MinimumAge;
                model.MaximumAge = (int?)jpe.MaximumAge;
                model.EmploymentType = (long)jpe.EmployementTypeId;
                model.QualificationId = (long?)jpe.Qualification;
                model.Distribute = 1;
                model.Noticeperiod = jpe.Noticeperiod;
                model.Optionalskills = jpe.Optionalskills;
                DomainService.Instance.RemoveData(id);
            }
            return View(model);
        }


      
        [HttpGet]
        public ActionResult ListJob1Error(string returnUrl = null)
        {
            if (UserInfo.Role != SecurityRoles.RecruitmentAgency)
            {
                return Redirect(UserInfo.PermaLink);
            }

            EmployerRequiredModel model = new EmployerRequiredModel(UserInfo.Id);
            model.ReturnUrl = returnUrl;
            return View(model);
        }

      
        [HttpPost]
        public ActionResult ListJob1Error(EmployerRequiredModel model)
        {
            if (ModelState.IsValid)
            {
                var original = MemberService.Instance.Get(model.Id);
                string summary = model.Overview;
                summary = summary.RemoveEmails();
                summary = summary.RemoveNumbers();
                summary = summary.RemoveWebsites();
                original.Summary = summary;
                original.Address = model.Address;
                original.CountryId = model.CountryId;
                original.StateId = model.StateId;
                original.City = model.City;
                original.Zip = model.Zip;
                original.PhoneCountryCode = model.PhoneCountryCode;
                original.Phone = model.Phone;
                original.MobileCountryCode = model.MobileCountryCode;
                original.Mobile = model.Mobile;
                original.Website = model.Website;
                original.Facebook = model.Facebook;
                original.Twitter = model.Twitter;
                original.LinkedIn = model.LinkedIn;
                original.GooglePlus = model.GooglePlus;

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    dataHelper.Update(original, User.Username);
                }
            }

            if (!string.IsNullOrEmpty(model.ReturnUrl))
            {
                if (model.ReturnUrl.ToLower().Contains("listjob1"))
                {
                    if (model.ReturnUrl.Contains("?"))
                    {
                        model.ReturnUrl = model.ReturnUrl + "&le=1";
                    }
                    else
                    {
                        model.ReturnUrl = model.ReturnUrl + "?le=1";
                    }
                }
                return Redirect(model.ReturnUrl);
            }
            else
            {
                return View(model);
            }
        }

       
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ListJob1(JobListingModel model)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            if (ModelState.IsValid)
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    var employer = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                    var permalink = model.Title;

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
                    var job1 = new ListJob1C();
                    job1.CompanyName = model.CompanyName;
                    job1.AboutCompany = model.AboutCompany;
                    job.Title = model.Title;
                    job.Distribute = Convert.ToBoolean(model.Distribute);
                    string description = Sanitizer.GetSafeHtmlFragment(model.Description);
                    description = description.RemoveEmails();
                    description = description.RemoveNumbers();
                    job.Description = description.RemoveWebsites();

                    string summary = model.Summary;
                    summary = summary.RemoveEmails();
                    summary = summary.RemoveNumbers();
                    summary = summary.RemoveWebsites();
                    job.Summary = summary;

                    string requirements = model.Requirements;
                    requirements = requirements.RemoveEmails();
                    requirements = requirements.RemoveNumbers();
                    requirements = requirements.RemoveWebsites();
                    job.Requirements = requirements;
                    string roles = model.Responsibilities;
                    roles = roles.RemoveEmails();
                    roles = roles.RemoveNumbers();
                    roles = roles.RemoveWebsites();
                    job.Responsilibies = roles;

                    job.IsFeaturedJob = model.IsFeaturedJob;
                    job.CategoryId = model.CategoryId;
                    job.SpecializationId = model.SpecializationId;
                    job.CountryId = model.CountryId;
                    job.StateId = model.StateId;
                    job.City = model.City;
                    job.Zip = model.Zip;
                    job.EmploymentTypeId = model.EmploymentType;
                    job.QualificationId = model.QualificationId;
                    job.MinimumAge = (byte?)model.MinimumAge;
                    job.MaximumAge = (byte?)model.MaximumAge;
                    job.MinimumExperience = (byte)model.MinimumExperience;
                    job.MaximumExperience = (byte)model.MaximumExperience;
                    job.MinimumSalary = model.MinimumSalary;
                    job.MaximumSalary = model.MaximumSalary;
                    job.Currency = model.SalaryCurrency;
                    job.PublishedDate = DateTime.Now;
                    job.ClosingDate = DateTime.Now.AddMonths(1);
                    job.PermaLink = permalink;
                    job.EmployerId = employer.UserId;
                    job.IsActive = true;
                    job.IsDeleted = false;
                    job.IsPostedOnTwitter = false;
                    job.InEditMode = false;
                    job.Distribute = (model.Distribute == 1);
                    job.IsPaid = job.IsPaid;
                    var job_id = Convert.ToInt64(dataHelper.Add<Job>(job, User.Username));

                    //IList<ListJob1C> productList = new List<ListJob1C>();
                    //var query = from s in db.ListJob1C
                    //            select s;
                    //var listdata = query.ToList();

                    //foreach (var pdata in listdata)
                    //{
                    //    db.ListJob1C.Add(new ListJob1C()
                    //    {
                    //        id = pdata.id,
                    //        CompanyName = pdata.CompanyName,
                    //        AboutCompany = pdata.AboutCompany,
                    //        job_id =Convert.ToInt16( job_id)
                    //    });
                    //    //db.ListJob1C.Add(listdata.ToList());
                    //}

                    //  String fileLogo = model.CompanyLogo;
                    string fileName = Path.GetFileName(model.CompanyLogos.FileName);
                    string imgBase64String = Convert.ToString(model.CompanyLogos); 

                    //byte[] imageBytes = System.IO.File.ReadAllBytes(imgBase64String);
                    //string base64String = Convert.ToBase64String(imageBytes);
                   // return base64String;


                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        try
                        {
                            conn.Open();
                            using (SqlCommand command = conn.CreateCommand())
                            {
                                command.CommandType = CommandType.Text;
                                command.CommandText = string.Format("insert into  ListJob1C" +
                                    "(id,CompanyName,AboutCompany,CompanyLogos,job_id) " +
                                    "values('{0}','{1}','{2}','{3}','{4}')",
                                  model.Id, model.CompanyName, model.AboutCompany, "dhjvdvfdgvfydgfjdgkfdjvdgkyufdjfyudgkfg", job_id);
                                command.ExecuteNonQuery();
                            }

                            conn.Close();


                        }
                        catch (Exception ex)
                        {
                            conn.Close();
                            SendEx(ex);
                        }
                    }


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
                        TempData["UpdateData"] = string.Format("{0} job has been submitted successfully. It is in approval process, we will inform you once it is approved!", job.Title);
                    }
                    //if (DomainService.Instance.PaymentProcessEnabled())
                    //    {
                    //        if (Request.QueryString["le"] != null && Request.QueryString["le"] == "1")
                    //        {
                    //            return RedirectToAction("Select", "Package", new { returnUrl = "/Employer/ListJobError?returnUrl=/listjob", RedirectUrl = "/employer/index", type = "J", sessionId = id, countryId = model.CountryId });
                    //        }
                    //        else
                    //        {
                    //            return RedirectToAction("Select", "Package", new { returnUrl = "/Employer/Index", RedirectUrl = "/employer/index", type = "J", sessionId = id, countryId = model.CountryId });
                    //        }
                    //    }

                }
                return RedirectToAction("Index1");
            }
            return View(new JobListingModel());
        }
       

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PostJob1(JobListingModel model)
        {
            int status = 0;
            if (ModelState.IsValid)
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    var employer = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                    var permalink = model.Title;

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
                    job.Title = model.Title;
                    job.Distribute = Convert.ToBoolean(model.Distribute);

                    string description = Sanitizer.GetSafeHtmlFragment(model.Description);
                    description = description.RemoveEmails();
                    description = description.RemoveNumbers();
                    job.Description = description.RemoveWebsites();

                    string summary = model.Summary;
                    summary = summary.RemoveEmails();
                    summary = summary.RemoveNumbers();
                    summary = summary.RemoveWebsites();
                    job.Summary = summary;

                    string requirements = model.Requirements;
                    requirements = requirements.RemoveEmails();
                    requirements = requirements.RemoveNumbers();
                    requirements = requirements.RemoveWebsites();
                    job.Requirements = requirements;
                    string responsibilities = model.Responsibilities;
                    responsibilities = responsibilities.RemoveEmails();
                    responsibilities = responsibilities.RemoveNumbers();
                    responsibilities = responsibilities.RemoveWebsites();
                    job.Responsilibies = responsibilities;

                    job.IsFeaturedJob = model.IsFeaturedJob;
                    job.CategoryId = model.CategoryId;
                    job.SpecializationId = model.SpecializationId;
                    job.CountryId = model.CountryId;
                    job.StateId = model.StateId;
                    job.City = model.City;
                    job.Zip = model.Zip;
                    job.EmploymentTypeId = model.EmploymentType;
                    job.QualificationId = model.QualificationId;
                    job.MinimumAge = (byte?)model.MinimumAge;
                    job.MaximumAge = (byte?)model.MaximumAge;
                    job.MinimumExperience = (byte?)model.MinimumExperience;
                    job.MaximumExperience = (byte?)model.MaximumExperience;
                    job.MinimumSalary = model.MinimumSalary;
                    job.MaximumSalary = model.MaximumSalary;
                    job.Currency = model.SalaryCurrency;
                    job.PublishedDate = DateTime.Now;
                    job.ClosingDate = DateTime.Now.AddMonths(1);
                    job.PermaLink = permalink;
                    job.EmployerId = employer.UserId;
                    job.IsActive = true;
                    job.IsDeleted = false;
                    job.IsPostedOnTwitter = false;
                    job.InEditMode = false;
                    var job_id = Convert.ToInt64(dataHelper.Add<Job>(job, User.Username));

                    if (job_id > 0)
                    {
                        var tracking = new Tracking
                        {
                            Id = Guid.NewGuid(),
                            JobId = job_id,
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
                                    "This is featuered job which will appear at main page as well as on top of search results!");
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
                    }
                    TempData["SaveData"] = string.Format("{0} job has been submitted successfully. It is in approval process, we will inform you once it is approved!", model.Title);
                }
                status = 1;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
       
        [UrlPrivilegeFilter]
        public ActionResult Index1(long? Id = null, string Type = "", string Status = null, long? CountryId = null, string fd = "", string fm = "", string fy = "", string td = "", string tm = "", string ty = "", int pageNumber = 1)
        {
            var employer = new UserInfoEntity();
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


            if (Id == null)
            {
                employer = _service.Get(User.Id);
            }
            else
            {
                employer = _service.Get(Id.Value);
            }
            var jobs = DomainService.Instance.GetJobList(employer.Id, Status, CountryId, sdt, edt, 10, pageNumber);
            int rows = 0;
            if (jobs.Count > 0)
            {
                rows = jobs.Max(x => x.MaxRows);
            }
            ViewBag.CountryList = new SelectList(SharedService.Instance.GetCountryList(), "Id", "Text");
            ViewBag.TypeList = new SelectList(new List<string> { "Standard", "Featured" });
            ViewBag.StatusList = new SelectList(new List<string> { "In Approval Processs", "Live", "Expired", "Deleted", "Rejected", "Deactivated" });
            ViewBag.Model = new StaticPagedList<JobEntity>(jobs, pageNumber, 10, rows);
            ViewBag.User = employer;
            ViewBag.Rows = rows;
            return View();
        }

        [UrlPrivilegeFilter]
        public ActionResult Index_Copy1(long? Id = null, string Type = "", string Status = null, long? CountryId = null, string fd = "", string fm = "", string fy = "", string td = "", string tm = "", string ty = "", int pageNumber = 1)
        {
            var employer = new UserInfoEntity();
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


            if (Id == null)
            {
                employer = _service.Get(User.Id);
            }
            else
            {
                employer = _service.Get(Id.Value);
            }
            var jobs = DomainService.Instance.GetJobList(employer.Id, Status, CountryId, sdt, edt, 10, pageNumber);
            int rows = 0;
            if (jobs.Count > 0)
            {
                rows = jobs.Max(x => x.MaxRows);
            }
            ViewBag.CountryList = new SelectList(SharedService.Instance.GetCountryList(), "Id", "Text");
            ViewBag.TypeList = new SelectList(new List<string> { "Standard", "Featured" });
            ViewBag.StatusList = new SelectList(new List<string> { "In Approval Processs", "Live", "Expired", "Deleted", "Rejected", "Deactivated" });
            ViewBag.Model = new StaticPagedList<JobEntity>(jobs, pageNumber, 10, rows);
            ViewBag.User = employer;
            ViewBag.Rows = rows;
            return View();
        }

        [HttpPost]
        public ActionResult AddSMJobs(long? countryId, SMJobModel model)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            if (ModelState.IsValid)
            {
                UserInfoEntity uinfo = _service.Get(User.Id);
                string requirements = model.JobDescription;
                requirements = requirements.RemoveEmails();
                requirements = requirements.RemoveNumbers();
                requirements = requirements.RemoveWebsites();
                string requirements1 = model.JobResponsibility;
                requirements1 = requirements1.RemoveEmails();
                requirements1 = requirements1.RemoveNumbers();
                requirements1 = requirements1.RemoveWebsites();
                string requirements2 = model.JobRequirement;
                requirements2 = requirements2.RemoveEmails();
                requirements2 = requirements2.RemoveNumbers();
                requirements2 = requirements2.RemoveWebsites();

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = string.Format("insert into  SMJobList" +
                                "(CountryName,CompanyName,JobTitle,MSkills,JobDescription,JobResponsibility,JobRequirement,JobLink,ContactNo,Email,CreatedBy,DateCreated,JobSource,Name) " +
                                "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                                model.CountryName, model.CompanyName, model.JobTitle, model.MSkills, requirements, requirements1, requirements2, model.JobLink, model.ContactNo, model.Email, uinfo.FirstName, DateTime.Now, model.JobSource, model.Name);
                            command.ExecuteNonQuery();
                        }

                        conn.Close();


                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        SendEx(ex);
                    }
                }


                TempData["UpdateData"] = "Social Media Job added successfully!";
            }
            return Redirect("/Admin/SMJobs");
        }
        
        public ActionResult DeleteCampaign(long Id)
        {
           
            using (JobPortalEntities context = new JobPortalEntities())
            {                              
                var obj = context.Campaigns.Where(s => s.Id == Id).FirstOrDefault();
                context.Campaigns.Remove(obj);
                TempData["UpdateData"] = "Campaign deleted successfully!";
                context.SaveChanges();
            }
            return Redirect("/Admin/Campaign");
        }
        [HttpPost]
        public ActionResult AddCampaign(long? countryId, CampaignModel model)
        {
            //send33();
            ViewBag.rows = TempData["rows"];
            if (!ModelState.IsValid)
            {
                var employer = MemberService.Instance.Get(Convert.ToString(model.Name));
                Campaign campaign = new Campaign()
                {
                    CountryId = model.CountryId,
                    CategoryId = model.CategoryId,
                    Username = "anilkumar@joblisting.com",
                    Type = model.Type,
                    Subject = model.Subject,
                    Body = model.Body
                };
                List<long> userlist = new List<long>();
                long campaignId = 0;
                string subject = string.Empty;
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    campaignId = Convert.ToInt64(dataHelper.Add<Campaign>(campaign, "RecruiterCom@joblisting.com"));
                    subject = model.Subject;

                }
                
                MailMessage mail = new MailMessage();
                var senderEmail = new MailAddress("anilkumar@joblisting.com", "Anil Kumar");
                var receiverEmail = new MailAddress("mohanr1t@gmail.com", "Mohan");
                var password = "Accuracy@123"; 
                var body = campaign.Body;
                using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                {
                    Host = "webmail.joblisting.com",
                    Port = 25,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
                })
                using (var message = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                    try
                    {
                        client.Send(message);
                    }
                    catch(Exception ex)
                    {
                       
                    }
                   


                //mohan
                var senderEmail1 = new MailAddress("anilkumar@joblisting.com", "Anil Kumar");
                  var receiverEmail1 = new MailAddress("ksdileep8@gmail.com", "dileep");
                var password1 = "Accuracy@123";
                var body1 = campaign.Body;
                using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                {
                    Host = "webmail.accuracy.com.sg",
                    Port = 25,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(senderEmail1.Address, password1)
                })
                using (var message1 = new MailMessage(senderEmail1, receiverEmail1)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                }) 
                    client.Send(message1);
                //prathap

                var senderEmail12 = new MailAddress("chetan@joblisting.com", "Chetan Bandgar");
                var receiverEmail12 = new MailAddress("pratapchandrannair@gmail.com", "Pratap");
                var password12 = "Vguestinn@123";
                var body12 = campaign.Body;
                using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                {
                    Host = "mail.joblisting.com",
                    Port = 25,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(senderEmail12.Address, password12)
                })
                using (var message12 = new MailMessage(senderEmail12, receiverEmail12)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                    try
                    {
                        client.Send(message12);
                    }
                    catch(Exception ex)
                    {

                    }
                   

                //chetan

               
                //prathap

                var senderEmail123 = new MailAddress("chetan@joblisting.com", "chetan");
                var receiverEmail123 = new MailAddress("chetanb@accuracy.com.sg", "chetan");
                var password123 = "Vguestinn@123";
                var body123 = campaign.Body;
                using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                {
                    Host = "mail.joblisting.com",
                    Port = 25,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(senderEmail123.Address, password123)
                })
                using (var message123 = new MailMessage(senderEmail123, receiverEmail123)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                    client.Send(message123);
                //dileep

                //var senderEmail124 = new MailAddress("chetan@joblisting.com", "chetan");
                //var receiverEmail124 = new MailAddress("ksdileep8@gmail.com", "dileep");
                //var password124 = "Vguestinn@123";
                //var body124 = campaign.Body;
                //using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                //{
                //    Host = "mail.joblisting.com",
                //    Port = 25,
                //    UseDefaultCredentials = false,
                //    Credentials = new System.Net.NetworkCredential(senderEmail124.Address, password124)
                //})
                //using (var message1234 = new MailMessage(senderEmail124, receiverEmail124)
                //{
                //    Subject = subject,
                //    Body = body,
                //    IsBodyHtml = true
                //})
                //    client.Send(message1234);



                TempData["UpdateData"] = "Campaign Mail Send successfully By chetan@joblisting.com!";
            }
            return Redirect("/india/Admin/Campaign");
        }
        public void send33()
        {
            var senderEmail = new MailAddress("chetan@joblisting.com", "chetan");
            var receiverEmail = new MailAddress("dileepsk51197@gmail.com", "dileep");
            var password = "Vguestinn@123"; //I used here APP password 

            var subject = "Subject";
            var body = @"<html><body><p>Dear XYZ,</p></body></html>";
            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
            {
                Host = "relay-hosting.secureserver.net",
                Port = 25,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
            })
            using (var message = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                try
                {
                    client.Send(message);
                }
                catch
                {
                    Exception ex;
                }
                
            //MailMessage Msg = new MailMessage();
            //// Sender e-mail address.
            //Msg.From = new MailAddress("chetan@joblisting.com");
            //// Recipient e-mail address.
            //Msg.To.Add("ksdileep8@gmail.com");
            ////Msg.Subject = txtSubject.Text;
            //Msg.Body = "some body message";
            //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            //smtp.Host = "relay-hosting.secureserver.net";
            //smtp.Send(Msg);
            //Create the msg object to be sent

        }
        public void Send2()
        {
            // Compose a message

            string from = "chetan@joblisting.com";
           // string from = "recruiter@joblisting.com";

            //string templates = ConfigurationManager.AppSettings["Template"];
            string baseUrl = "https://www.joblisting.com";
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string postmail = "chetan@joblisting.com";
            //string postpassword = "052e14c947dbb70b9d04776344b6d88e-1f1bd6a9-d1f0adb4";
            string postpassword = "3mM44UcgzrhVA8_FHyBtTys3sFT9MmMkmdb54";
            string body = string.Empty;

            long aid = 0;
            string subject = "";
            string uname = "";
            string bodyText = "";
            int type = 0;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = "SELECT Id,Username, Subject, Type,Body FROM Campaign WHERE Id = (SELECT MAX(Id) FROM Campaign WHERE Sent=0)";

                        using (SqlDataReader rdr1 = command.ExecuteReader())
                        {
                            while (rdr1.Read())
                            {
                                if (rdr1["Id"] != DBNull.Value) 
                                {
                                    aid = Convert.ToInt64(rdr1["Id"]);
                                    uname = Convert.ToString(rdr1["Username"]);
                                    subject = Convert.ToString(rdr1["Subject"]);
                                    type = Convert.ToInt32(rdr1["Type"]);
                                    bodyText = Convert.ToString(rdr1["Body"]);
                                }
                            }
                            rdr1.Close();
                        }
                    }

                    if (aid > 0)
                    {
                        string query = string.Empty;
                        query = string.Format("SELECT UserId, Username, Company, FirstName, LastName FROM UserProfiles WHERE Username = '{0}'", uname);


                        try
                        {
                            List<UserEntity1> userList1 = new List<UserEntity1>();
                            using (SqlCommand command1 = conn.CreateCommand())
                            {
                                command1.CommandType = CommandType.Text;
                                command1.CommandText = query;

                                using (SqlDataReader rdr = command1.ExecuteReader())
                                {
                                    while (rdr.Read())
                                    {
                                        UserEntity1 entity1 = new UserEntity1()
                                        {
                                            Id = Convert.ToInt64(rdr["UserId"]),
                                            //type = Convert.ToInt32(rdr["Type"]),
                                            Email = Convert.ToString(rdr["Username"]),
                                            Company = Convert.ToString(rdr["Company"]),
                                            FirstName = Convert.ToString(rdr["FirstName"]),
                                            LastName = Convert.ToString(rdr["LastName"])
                                        };
                                        userList1.Add(entity1);
                                    }
                                    rdr.Close();
                                }
                            }
                            MimeMessage mail1 = new MimeMessage();

                            //using (SmtpClient client1 = new SmtpClient())
                            //{
                            //    foreach (var item1 in userList1)
                            //    {
                            //        try
                            //        {
                                        
                            //            if (!item1.Email.ToLower().Contains("admin_"))
                            //            {                                          
                            //                var reader1 = new StreamReader(Server.MapPath("~/Templates/Mail/campaign.html"));    
                            //                if (reader1 != null)
                            //                {
                                                
                            //                    string ebody1 = reader1.ReadToEnd();
                            //                    ebody1 = ebody1.Replace("@@receiver", string.Format("{0} {1}", item1.FirstName, item1.LastName));
                            //                    ebody1 = ebody1.Replace("@@subject", subject);
                            //                    // ebody = ebody.Replace("@@viewurl", string.Format("{0}/Announcement/Show?Id={1}", baseUrl, aid));
                            //                    ebody1 = ebody1.Replace("@@viewurl", bodyText);

                            //                    mail1.From.Clear();
                            //                    mail1.To.Clear();
                            //                    mail1.From.Add(new MailboxAddress("Joblisting", from));
                            //                    mail1.To.Add(new MailboxAddress("Excited User", item1.Email));
                            //                    mail1.Subject = subject;
                            //                    mail1.Body = new TextPart("html")
                            //                    {
                            //                        Text = ebody1,
                            //                    };
                            //                    try
                            //                    {
                            //                        string postmail1 =  "hmdddhsn@gmail.com";
                            //                        string postpassword1 = "ebnlxhdvypbibbzh";

                            //                        // XXX - Should this be a little different?
                            //                        client1.ServerCertificateValidationCallback = (s, c, h, e) => true;

                            //                        //client1.Connect("smtp.mailgun.org", 587, false);
                            //                        client1.Connect("smtp.gmail.com", 465, true); 
                            //                        client1.AuthenticationMechanisms.Remove("XOAUTH2");
                            //                        client1.Authenticate(postmail1, postpassword1);

                            //                        client1.Send(mail1);
                            //                        client1.Disconnect(true);

                            //                }
                            //                    catch (Exception ex)
                            //        {
                            //                        TempData["exception1"] = ex;
                            //            //SendEx(ex);
                            //        }

                            //    }

                            //                using (SqlCommand command = conn.CreateCommand())
                            //                {
                            //                    command.CommandType = CommandType.StoredProcedure;
                            //                    command.CommandText = "USP_Campaign";
                            //                    command.Parameters.Add(new SqlParameter("@CampaignId", aid));
                            //                    command.Parameters.Add(new SqlParameter("@UserId", item1.Id));
                            //                    command.ExecuteNonQuery();
                            //                }
                            //            }
                            //        }
                            //        catch (Exception ex)
                            //        {
                            //            TempData["exception2"] = ex;
                            //            // SendEx(ex);
                            //        }
                            //    }

                            //    using (SqlCommand command = conn.CreateCommand())
                            //    {
                            //        command.CommandType = CommandType.Text;
                            //        command.CommandText = string.Format("UPDATE Campaign SET Sent=1 WHERE Id={0} --AND (SELECT COUNT(ID) FROM UserAnnouncements WHERE AnnouncementId={0}) > 0", aid);
                            //        command.ExecuteNonQuery();
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {
                            conn.Close();
                            TempData["exception3"] = ex;
                            //SendEx(ex);
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    TempData["exception4"] = ex;
                    // SendEx(ex);
                }
            }

        }
        public void Send1()
        {
            // Compose a message

            string from = "notify@joblisting.com";

            //string templates = ConfigurationManager.AppSettings["Template"];
            string baseUrl = "https://www.joblisting.com";
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string postmail = "master@joblisting.com";
            string postpassword = "052e14c947dbb70b9d04776344b6d88e-1f1bd6a9-d1f0adb4";
            string body = string.Empty;

            long aid = 0;
            string subject = "";
            string bodyText = "";
            int type = 0;
            int cid = 0;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = "SELECT Id, Subject, Type,CountryId,Body FROM Announcements WHERE Id = (SELECT MAX(Id) FROM Announcements WHERE Sent=0)";

                        using (SqlDataReader rdr1 = command.ExecuteReader())
                        {
                            while (rdr1.Read())
                            {
                                if (rdr1["Id"] != DBNull.Value)
                                {
                                    aid = Convert.ToInt64(rdr1["Id"]);
                                    subject = Convert.ToString(rdr1["Subject"]);
                                    type = Convert.ToInt32(rdr1["Type"]);
                                    cid = Convert.ToInt32(rdr1["CountryId"]);
                                    bodyText = Convert.ToString(rdr1["Body"]);
                                }
                            }
                            rdr1.Close();
                        }
                    }

                    if (aid > 0)
                    {
                        string query = string.Empty;



                        query = string.Format("SELECT UserId, Username, Company, FirstName, LastName FROM UserProfiles WHERE UserId NOT IN (SELECT UserId FROM UserAnnouncements WHERE Sent=0 AND AnnouncementId={0}) AND Type = {1} AND IsConfirmed = 1 AND IsActive = 1 AND IsDeleted = 0 and CountryId={2} order by UserId desc", aid, type, cid);



                        try
                        {
                            List<UserEntity1> userList1 = new List<UserEntity1>();
                            using (SqlCommand command1 = conn.CreateCommand())
                            {
                                command1.CommandType = CommandType.Text;
                                command1.CommandText = query;

                                using (SqlDataReader rdr = command1.ExecuteReader())
                                {
                                    while (rdr.Read())
                                    {
                                        UserEntity1 entity1 = new UserEntity1()
                                        {
                                            Id = Convert.ToInt64(rdr["UserId"]),
                                            Email = Convert.ToString(rdr["Username"]),
                                            Company = Convert.ToString(rdr["Company"]),
                                            FirstName = Convert.ToString(rdr["FirstName"]),
                                            LastName = Convert.ToString(rdr["LastName"])
                                        };
                                        userList1.Add(entity1);
                                    }
                                    rdr.Close();
                                }
                            }
                            MimeMessage mail1 = new MimeMessage();

                            //using (SmtpClient client1 = new SmtpClient())
                            //{
                               
                            //    foreach (var item1 in userList1)
                            //    {
                            //        try
                            //        {
                            //            if (!item1.Email.ToLower().Contains("admin_"))
                            //            {
                            //                var reader1 = new StreamReader(Server.MapPath("~/Templates/Mail/announcement.html"));
                            //                if (reader1 != null)
                            //                {
                            //                    string ebody1 = reader1.ReadToEnd();
                            //                    ebody1 = ebody1.Replace("@@receiver", string.Format("{0} {1}", item1.FirstName, item1.LastName));
                            //                    ebody1 = ebody1.Replace("@@subject", subject);
                            //                    // ebody = ebody.Replace("@@viewurl", string.Format("{0}/Announcement/Show?Id={1}", baseUrl, aid));
                            //                    ebody1 = ebody1.Replace("@@viewurl", bodyText);

                            //                    mail1.From.Clear();
                            //                    mail1.To.Clear();
                            //                    mail1.From.Add(new MailboxAddress("Joblisting", from));
                            //                    mail1.To.Add(new MailboxAddress("Excited User", item1.Email));
                            //                    mail1.Subject = subject;
                            //                    mail1.Body = new TextPart("html")
                            //                    {
                            //                        Text = ebody1,
                            //                    };
                            //                    try
                            //                    {

                            //                        string postmail1 = "hmdddhsn@gmail.com";
                            //                        string postpassword1 = "ebnlxhdvypbibbzh";

                            //                        // XXX - Should this be a little different?
                            //                        client1.ServerCertificateValidationCallback = (s, c, h, e) => true;

                            //                        //client1.Connect("smtp.mailgun.org", 587, false);
                            //                        client1.Connect("smtp.gmail.com", 465, true);
                            //                        client1.AuthenticationMechanisms.Remove("XOAUTH2");
                            //                        client1.Authenticate(postmail1, postpassword1);

                            //                        client1.Send(mail1);
                            //                        client1.Disconnect(true);

                            //                    }
                            //                    catch (Exception ex)
                            //                    {
                            //                        SendEx(ex);
                            //                    }

                            //                }

                            //                using (SqlCommand command = conn.CreateCommand())
                            //                {
                            //                    command.CommandType = CommandType.StoredProcedure;
                            //                    command.CommandText = "USP_Announce";
                            //                    command.Parameters.Add(new SqlParameter("@AnnouncementId", aid));
                            //                    command.Parameters.Add(new SqlParameter("@UserId", item1.Id));
                            //                    command.ExecuteNonQuery();
                            //                }
                            //            }
                            //        }
                            //        catch (Exception ex)
                            //        {
                            //            SendEx(ex);
                            //        }

                            //    }

                            //    using (SqlCommand command = conn.CreateCommand())
                            //    {
                            //        command.CommandType = CommandType.Text;
                            //        command.CommandText = string.Format("UPDATE Announcements SET Sent=1 WHERE Id={0} --AND (SELECT COUNT(ID) FROM UserAnnouncements WHERE AnnouncementId={0}) > 0", aid);
                            //        command.ExecuteNonQuery();
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {
                            conn.Close();
                            SendEx(ex);
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    SendEx(ex);
                }
            }

        }
        void SendEx(Exception ex)
        {

            string baseUrl = "https://www.joblisting.com";
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string postmail = "master@joblisting.com";
            string postpassword = "052e14c947dbb70b9d04776344b6d88e-1f1bd6a9-d1f0adb4";
            string from = "notify@joblisting.com";

            MimeMessage mail = new MimeMessage();

            string[] toList = ConfigurationManager.AppSettings["ServiceNotifyEmail"].Split(',');

            string body = string.Format("<h2>{0}</h2>", baseUrl);
            body += string.Format("{0}", ex.ToString());
            mail.From.Add(new MailboxAddress("Joblisting", from));
            foreach (string email in toList)
            {
                mail.To.Add(new MailboxAddress("Excited User", email));
            }
            mail.Subject = "Error occured while running Joblisting Announcement Service";
            mail.Body = new TextPart("html")
            {
                Text = body,
            };

            //using (SmtpClient osmtp = new SmtpClient())
            //{
                
            //    osmtp.Timeout = 100000;
            //    osmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
            //    osmtp.Connect("smtp.mailgun.org", 587, false);
            //    osmtp.AuthenticationMechanisms.Remove("XOAUTH2");
            //    osmtp.Authenticate(postmail, postpassword);
            //    osmtp.Send(mail);
            //    osmtp.Disconnect(true);
                
            //}

        }
        public class UserEntity1
        {
            public long Id { get; set; }
            public string Email { get; set; }
            public int type { get; set; }
            public string Company { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

        }
        [HttpGet]
        public ActionResult AddTip()
        {
            return View(new TipModel());
        }

        [HttpPost]
        public ActionResult AddTip(TipModel model)
        {
            long tipId = 0;
            if (ModelState.IsValid)
            {
                Tip tip = new Tip()
                {
                    Type = model.Type,
                    Title = model.Title,
                    Body = model.Body
                };
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    tipId = Convert.ToInt64(dataHelper.Add<Tip>(tip, User.Username));
                }
                TempData["UpdateData"] = "Tip added successfully!";
            }
            return Redirect("/Admin/Tips");
        }

        [HttpGet]
        public ActionResult ShowTip(long Id)
        {
            Tip entity = new Tip();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                entity = dataHelper.GetSingle<Tip>(Id);
            }
            return View(entity);
        }

        [HttpGet]
        public ActionResult EditCampaign(long Id)
        {
            CampaignModel entity = new CampaignModel();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Campaign tip = dataHelper.GetSingle<Campaign>(Id);

                entity = new CampaignModel()
                {
                    Id = tip.Id,
                    CountryId = tip.CountryId,
                    CategoryId = tip.CategoryId,
                    Name = tip.Username,
                    Subject = tip.Subject,
                    Type = tip.Type,
                    Body = tip.Body
                };
            }
            return View(entity);
        }

        [HttpGet]
        public ActionResult EditWebsites(long Id)
        {


            WebScrapModel entity = new WebScrapModel();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                WebsiteList tip = dataHelper.GetSingle<WebsiteList>(Id);

                entity = new WebScrapModel()
                {
                    Id = tip.Id,
                    CountryName = tip.CountryName,
                    CompanyName = tip.CompanyName,
                    Website = tip.Website,
                    Email = tip.Email,
                    Name = tip.Name

                };
            }
            return View(entity);
        }
        [HttpPost]
        public ActionResult EditWebsites(WebScrapModel model)
        {

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            UserInfoEntity uinfo = _service.Get(User.Id);
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = string.Format("update  WebsiteList " +
                                "set CountryName='{0}',CompanyName='{1}',Website='{2}',Email='{3}',UpdatedBy='{4}',DateUpdated='{5}',Name='{7}' " +
                                "where Id={6}",
                                model.CountryName, model.CompanyName, model.Website, model.Email, uinfo.FirstName, DateTime.Now, model.Id, model.Name);
                            command.ExecuteNonQuery();
                        }
                        conn.Close();
                        conn.Close();

                        string from = "notify@joblisting.com";

                        //string templates = ConfigurationManager.AppSettings["Template"];
                        string baseUrl = "https://www.joblisting.com";
                        string postmail = "master@joblisting.com";
                        string postpassword = "052e14c947dbb70b9d04776344b6d88e-1f1bd6a9-d1f0adb4";
                        string body = string.Empty;

                        MimeMessage mail1 = new MimeMessage();

                        //using (SmtpClient client1 = new SmtpClient())
                        //{

                        //    try
                        //    {

                        //        var reader1 = new StreamReader(Server.MapPath("~/Templates/Mail/EmailWebsite.html"));
                        //        if (reader1 != null)
                        //        {
                        //            string ebody1 = reader1.ReadToEnd();
                        //            ebody1 = ebody1.Replace("@@receiver", string.Format("{0}", uinfo.FirstName));
                        //            ebody1 = ebody1.Replace("@@subject", "Website Details from WebScraping Team");
                        //            ebody1 = ebody1.Replace("@@viewurl", "These are details of Website");
                        //            ebody1 = ebody1.Replace("@@v1", string.Format("Company Name :: {0}", model.CompanyName));
                        //            ebody1 = ebody1.Replace("@@d2", string.Format("Country Name ::::: {0}", model.CountryName));
                        //            ebody1 = ebody1.Replace("@@s3", string.Format("Company Url :::: {0}", model.Website));

                        //            mail1.From.Clear();
                        //            mail1.To.Clear();
                        //            mail1.From.Add(new MailboxAddress("Joblisting", from));
                        //            mail1.To.Add(new MailboxAddress("Excited User", model.Email));
                        //            mail1.Subject = "Website Details from WebScraping Team";
                        //            mail1.Body = new TextPart("html")
                        //            {
                        //                Text = ebody1,
                        //            };
                        //            try
                        //            {

                        //                // XXX - Should this be a little different?
                        //                client1.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        //                client1.Connect("smtp.mailgun.org", 587, false);
                        //                client1.AuthenticationMechanisms.Remove("XOAUTH2");
                        //                client1.Authenticate(postmail, postpassword);

                        //                client1.Send(mail1);
                        //                client1.Disconnect(true);

                        //            }
                        //            catch (Exception ex)
                        //            {
                        //                SendEx(ex);
                        //            }

                        //        }



                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        SendEx(ex);
                        //    }



                        //}
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        SendEx(ex);
                    }
                }
                TempData["UpdateData"] = "Website updated successfully!";
            }
            return Redirect("/Admin/Websites");
        }


        [HttpGet]
        public ActionResult EditSMJobs(long Id)
        {


            SMJobModel entity = new SMJobModel();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                SMJobList tip = dataHelper.GetSingle<SMJobList>(Id);

                entity = new SMJobModel()
                {
                    Id = tip.Id,
                    CountryName = tip.CountryName,
                    CompanyName = tip.CompanyName,
                    JobTitle = tip.JobTitle,
                    MSkills = tip.MSkills,
                    JobDescription = tip.JobDescription,
                    JobResponsibility = tip.JobResponsibility,
                    JobRequirement = tip.JobRequirement,
                    JobLink = tip.JobLink,
                    ContactNo = tip.ContactNo,
                    Email = tip.Email,
                    JobSource = tip.JobSource,
                    Name = tip.Name

                };
            }
            return View(entity);
        }
        [HttpPost]
        public ActionResult EditSMJobs(SMJobModel model)
        {

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            UserInfoEntity uinfo = _service.Get(User.Id);
            string requirements = model.JobDescription;
            requirements = requirements.RemoveEmails();
            requirements = requirements.RemoveNumbers();
            requirements = requirements.RemoveWebsites();
            string requirements1 = model.JobResponsibility;
            requirements1 = requirements1.RemoveEmails();
            requirements1 = requirements1.RemoveNumbers();
            requirements1 = requirements1.RemoveWebsites();
            string requirements2 = model.JobRequirement;
            requirements2 = requirements2.RemoveEmails();
            requirements2 = requirements2.RemoveNumbers();
            requirements2 = requirements2.RemoveWebsites();
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = string.Format("update  SMJobList " +
                                "set CountryName='{0}',CompanyName='{1}',JobTitle='{2}',JobDescription='{3}',JobLink='{4}',ContactNo='{5}',Email='{6}',UpdatedBy='{7}',DateUpdated='{8}',MSkills='{10}',JobResponsibility='{11}',JobRequirement='{12}',JobSource='{13}',Name='{14}'  " +
                                "where Id={9}",
                                model.CountryName, model.CompanyName, model.JobTitle, requirements, model.JobLink, model.ContactNo, model.Email, uinfo.FirstName, DateTime.Now, model.Id, model.MSkills, requirements1, requirements2, model.JobSource, model.Name);
                            command.ExecuteNonQuery();
                        }

                        conn.Close();


                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        SendEx(ex);
                    }
                }
                TempData["UpdateData"] = "Social Media Job updated successfully!";
            }
            return Redirect("/Admin/SMJobs");
        }





        [HttpPost]
        public ActionResult EditCampaign(CampaignModel model)
        {
            List<long> userlist = new List<long>();
            long aid = 0;
            if (ModelState.IsValid)
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    Campaign campaign = dataHelper.GetSingle<Campaign>(model.Id);
                    campaign.Type = model.Type;
                    campaign.CountryId = model.CountryId;
                    campaign.CategoryId = model.CategoryId;
                    campaign.Username = model.Name;
                    campaign.Subject = model.Subject;
                    campaign.Body = model.Body;

                    aid = campaign.Id;
                    dataHelper.Update<Campaign>(campaign, User.Username);
                }
                //Send();
                TempData["UpdateData"] = "Announcement updated successfully!";
            }
            return Redirect("/Admin/Campaign");
        }
        public ActionResult DeleteWebsites(long Id)
        {
            List<long> userlist = new List<long>();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                context.Database.ExecuteSqlCommand(string.Format("DeleteWebsites {0}", Id));
            }
            TempData["UpdateData"] = "Websites deleted successfully!";

            return Redirect("/Admin/Websites");
        }
        //public ActionResult DeleteCampaign(long Id)
        //{
        //    List<long> userlist = new List<long>();

        //    using (JobPortalEntities context = new JobPortalEntities())
        //    {
        //        context.Database.ExecuteSqlCommand(string.Format("DeleteCampaign {0}", Id));
        //    }
        //    TempData["UpdateData"] = "Campaign deleted successfully!";

        //    return Redirect("/Admin/Campaign");
        //}

        [HttpGet]
        public ActionResult EditAnnouncement(long Id)
        {
            AnnouncementModel entity = new AnnouncementModel();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Announcement tip = dataHelper.GetSingle<Announcement>(Id);

                entity = new AnnouncementModel()
                {
                    Id = tip.Id,
                    Subject = tip.Subject,
                    Type = tip.Type,
                    CountryId=tip.CountryId,
                    Body = tip.Body
                };
            }
            return View(entity);
        }

        [HttpPost]
        public ActionResult EditAnnouncement(AnnouncementModel model)
        {
            List<long> userlist = new List<long>();
            long aid = 0;
            if (ModelState.IsValid)
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    Announcement announcement = dataHelper.GetSingle<Announcement>(model.Id);
                    announcement.Type = model.Type;
                    announcement.CountryId = model.CountryId;
                    announcement.Subject = model.Subject;
                    announcement.Body = model.Body;
                    aid = announcement.Id;
                    dataHelper.Update<Announcement>(announcement, User.Username);
                }
                Send1();
                TempData["UpdateData"] = "Announcement updated successfully!";
            }
            return Redirect("/Admin/Announcements");
        }

        public ActionResult DeleteAnnouncement(long Id)
        {
            List<long> userlist = new List<long>();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                context.Database.ExecuteSqlCommand(string.Format("DeleteAnnouncement {0}", Id));
            }
            TempData["UpdateData"] = "Announcement deleted successfully!";

            return Redirect("/Admin/Announcements");
        }

        [HttpGet]
        public ActionResult EditTip(long Id)
        {
            TipModel entity = new TipModel();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Tip tip = dataHelper.GetSingle<Tip>(Id);

                entity = new TipModel()
                {
                    Id = tip.Id,
                    Title = tip.Title,
                    Type = tip.Type,
                    Body = tip.Body
                };
            }
            return View(entity);
        }

        [HttpPost]
        public ActionResult EditTip(TipModel model)
        {
            List<long> userlist = new List<long>();
            long tipId = 0;
            if (ModelState.IsValid)
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    Tip tip = dataHelper.GetSingle<Tip>(model.Id);
                    tip.Type = model.Type;
                    tip.Title = model.Title;
                    tip.Body = model.Body;
                    tipId = tip.Id;
                    dataHelper.Update<Tip>(tip, User.Username);
                    userlist = dataHelper.Get<UserProfile>().Where(x => x.Type == 4).Select(x => x.UserId).ToList();
                }
                TempData["UpdateData"] = "Tip updated successfully!";

                string subject = model.Title;
            }
            return Redirect("/Admin/Tips");
        }

        public ActionResult DeleteTip(long Id)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Tip tip = dataHelper.GetSingle<Tip>(Id);
                dataHelper.Remove<Tip>(tip);
            }
            TempData["UpdateData"] = "Tip deleted successfully!";
            return Redirect("/Admin/Tips");
        }

        public ActionResult SpamWords(string word, short? hlevel, short? llevel, int pageNumber = 0)
        {
            List<Dictionary> highWords = new List<Dictionary>();
            List<Dictionary> lowWords = new List<Dictionary>();

            Dictionary spamWord = null;

            int high = 0;
            int low = 0;

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                spamWord = dataHelper.Get<Dictionary>().SingleOrDefault<Dictionary>(x => x.Word.ToLower().Equals(word.ToLower().Trim()));
                if (!string.IsNullOrEmpty(word) && (hlevel != null || llevel != null))
                {
                    if (spamWord == null)
                    {
                        spamWord = new Dictionary()
                        {
                            Word = word.ToLower().Trim(),
                            Level = hlevel == null ? llevel.Value : hlevel.Value
                        };
                        dataHelper.Add<Dictionary>(spamWord);
                    }
                }
                high = dataHelper.Get<Dictionary>().Count(x => x.Level == 1);
                low = dataHelper.Get<Dictionary>().Count(x => x.Level == 2);

                highWords = dataHelper.Get<Dictionary>().Where(x => x.Level == 1).OrderBy(x => x.Word).ToList();
                lowWords = dataHelper.Get<Dictionary>().Where(x => x.Level == 2).OrderBy(x => x.Word).ToList();
            }

            ViewBag.HighWords = highWords;
            ViewBag.LowWords = lowWords;
            ViewBag.High = high;
            ViewBag.Low = low;

            return View();
        }
        public ActionResult DeleteWord(long Id)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Dictionary word = dataHelper.GetSingle<Dictionary>(Id);

                dataHelper.Remove<Dictionary>(word);
            }
            TempData["UpdateData"] = "Spam word deleted successfully!";
            return Redirect("/Admin/SpamWords");
        }

        //
        // GET: /Admin/
        [Authorize(Roles = "SuperUser, Administrator, Sales1")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperUser, Administrator, Sales1")]
        public ActionResult Register(AdminUserSignUpModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the Admin
                try
                {
                    WebSecurity.CreateUserAndAccount(model.Username, model.Password);
                    WebSecurity.Login(model.Username, model.Password);

                    Roles.AddUserToRoles(model.Username, new[] { SecurityRoles.Administrator.ToString() });

                    var profile = MemberService.Instance.Get(model.Username);
                    if (profile != null)
                    {
                        profile.Type = (int)SecurityRoles.Administrator;
                        MemberService.Instance.Update(profile);

                        var entity = new UserProfile();
                        entity.Company = model.Company;
                        entity.Username = model.Username;
                        long adminId = 0;
                        using (JobPortalEntities context = new JobPortalEntities())
                        {
                            DataHelper dataHelper = new DataHelper(context);
                            adminId = (long)dataHelper.Add(entity, User.Username);
                        }

                        if (adminId > 0)
                        {
                            TempData["SaveData"] =
                                "Your account has been created successfully. Confirmation email has been sent to your registered email address.";
                            return RedirectToAction("Employer", "Admin");
                        }
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            return View(model);
        }

        [Authorize(Roles = "SuperUser, Administrator ,Sales1")]
        [UrlPrivilegeFilter]
        public ActionResult Index(long? countryId, string fd, string fm, string fy, string td, string tm, string ty, string ShowWithValue = "", int pageNumber = 1)
        {
            UserInfoEntity uinfo = _service.Get(User.Id);
            
            string sdate = string.Empty;
            string edate = string.Empty;
            var sdt = new DateTime?();
            var edt = new DateTime?();

            int pageSize = 111;

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
            int rows = 0;
            List<UserByCountryEntity> list = new List<UserByCountryEntity>();
            if (ShowWithValue.Equals("1"))
            {
                if (uinfo.Username == "tasnim@joblisting.com")
                {
                    list = _service.CountryWiseUsers(106, sdt, edt, true, pageSize, pageNumber);
                }
                else if (uinfo.Username == "maitrantn@joblisting.com" || uinfo.Username == "Linhv@joblisting.com")
                {
                    list = _service.CountryWiseUsers(3343, sdt, edt, true, pageSize, pageNumber);
                }
                else if (uinfo.Username == "sandhyam@joblisting.com" || uinfo.Username == "Tanyam@joblisting.com")
                {
                    list = _service.CountryWiseUsers(139, sdt, edt, true, pageSize, pageNumber);
                    // CountryId = 139;
                }
                else if (uinfo.Username == "sandhyas@joblisting.com" || uinfo.Username == "balus@joblisting.com")
                {
                    list = _service.CountryWiseUsers(205, sdt, edt, true, pageSize, pageNumber);
                    // CountryId = 205;
                }
                else if (uinfo.Username == "adminIndo@joblisting.com" || uinfo.Username == "RecruiterIndo@joblisting.com")
                {
                    list = _service.CountryWiseUsers(107, sdt, edt, null, pageSize, pageNumber);
                    //countryId = 107;
                }
                else if (uinfo.Username == "adminPhil@joblisting.com" || uinfo.Username == "RecruiterPhil@joblisting.com")
                {
                    list = _service.CountryWiseUsers(180, sdt, edt, null, pageSize, pageNumber);
                    //countryId = 180;
                }

                else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
                {
                    list = _service.CountryWiseUsers(40, sdt, edt, null, pageSize, pageNumber);
                    //countryId = 40;
                    Session["CountryId"] = 40;
                }

                else
                {
                    list = _service.CountryWiseUsers(countryId, sdt, edt, true, pageSize, pageNumber);
                }
            }
            else
            {
                if (uinfo.Username == "tasnim@joblisting.com" || uinfo.Username == "lakshmii@joblisting.com")
                {
                    list = _service.CountryWiseUsers(106, sdt, edt, null, pageSize, pageNumber);
                }
                else if (uinfo.Username == "maitrantn@joblisting.com" || uinfo.Username == "Linhv@joblisting.com")
                {
                    list = _service.CountryWiseUsers(3343, sdt, edt, null, pageSize, pageNumber);
                }
                else if (uinfo.Username == "sandhyam@joblisting.com" || uinfo.Username == "Tanyam@joblisting.com")
                {
                    list = _service.CountryWiseUsers(139, sdt, edt, null, pageSize, pageNumber);
                }
                else if (uinfo.Username == "sandhyas@joblisting.com" || uinfo.Username == "balus@joblisting.com")
                {
                    list = _service.CountryWiseUsers(205, sdt, edt, null, pageSize, pageNumber);
                }
                else if (uinfo.Username == "adminIndo@joblisting.com" || uinfo.Username == "RecruiterIndo@joblisting.com")
                {
                    list = _service.CountryWiseUsers(107, sdt, edt, null, pageSize, pageNumber);
                    // countryId = 107;
                }
                else if (uinfo.Username == "adminPhil@joblisting.com" || uinfo.Username == "RecruiterPhil@joblisting.com")
                {
                    list = _service.CountryWiseUsers(180, sdt, edt, null, pageSize, pageNumber);
                    //countryId = 180;
                }

                else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
                {
                    list = _service.CountryWiseUsers(40, sdt, edt, null, pageSize, pageNumber);
                    //countryId = 40;
                    Session["CountryId"] = 40;
                }

                else
                {
                    list = _service.CountryWiseUsers(countryId, sdt, edt, null, pageSize, pageNumber);
                }
            }

            if (list.Count > 0)
            {
                rows = list.FirstOrDefault().MaxRows;
            }
           
            Session["EmpId"] = uinfo.Id;
            ViewBag.Individuals = list.Sum(x => x.Individuals);
            ViewBag.Jobseekers = list.Sum(x => x.Jobseekers);
            ViewBag.Companies = list.Sum(x => x.Companies);
            ViewBag.Resumes = list.Sum(x => x.Resumes);
            ViewBag.VerifiedIndividuals = list.Sum(x => x.VerifiedIndividuals);
            ViewBag.VerifiedCompanies = list.Sum(x => x.VerifiedCompanies);
            ViewBag.PhotosInApproval = list.Sum(x => x.PhotoApprovals);
            ViewBag.JPhotosInApproval = list.Sum(x => x.JPhotoApprovals);
            ViewBag.LogosInApproval = list.Sum(x => x.LogoApprovals);

            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<UserByCountryEntity>(list, pageNumber, pageSize, rows);

            return View();
        }

        [Authorize(Roles = "SuperUser, Administrator ,Sales1")]
        public ActionResult UserStatus(long? countryId, int? type, bool? confirmed, bool? active, string fd, string fm, string fy, string td, string tm, string ty, string ShowWithValue = "", int pageNumber = 1, string ut = null)
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

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
            {
                countryId = 40;
            }

            string sdate = string.Empty;
            string edate = string.Empty;
            var sdt = new DateTime?();
            var edt = new DateTime?();

            int pageSize = 111;

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
            int rows = 0;
            List<UserByCountryEntity> list = new List<UserByCountryEntity>();
            if (ShowWithValue.Equals("1"))
            {
                list = (new PortalDataService()).GetUsersByCountry(countryId, type, confirmed, active, sdt, edt, true, pageSize, pageNumber);
            }
            else
            {
                list = (new PortalDataService()).GetUsersByCountry(countryId, type, confirmed, active, sdt, edt, null, pageSize, pageNumber);
            }

            if (list.Count > 0)
            {
                rows = list.FirstOrDefault().MaxRows;
            }
            ViewBag.Individuals = list.Sum(x => x.Individuals);
            ViewBag.Jobseekers = list.Sum(x => x.Jobseekers);
            ViewBag.Companies = list.Sum(x => x.Companies);
            ViewBag.VerifiedIndividuals = list.Sum(x => x.VerifiedIndividuals);
            ViewBag.VerifiedCompanies = list.Sum(x => x.VerifiedCompanies);
            ViewBag.PhotosInApproval = list.Sum(x => x.PhotoApprovals);
            ViewBag.LogosInApproval = list.Sum(x => x.LogoApprovals);

            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<UserByCountryEntity>(list, pageNumber, pageSize, rows);

            return View();
        }


        [Authorize(Roles = "SuperUser, Administrator, Sales1")]
        public ActionResult UpdateUser(long id)
        {
            var model = new UserModel(id);
            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperUser, Administrator, Sales1")]
        public ActionResult UpdateUser(UserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //AdminUser original = DataHelper.Instance.GetList<AdminUser>().FirstOrDefault(e => e.Id == model.Id);
                    //original.FirstName = model.FirstName;
                    ////original.LastName = model.LastName;
                    //original.Address = model.Address;
                    //original.CountryId = model.CountryId;
                    //if (model.StateId > 0)
                    //{
                    //    original.StateId = model.StateId;
                    //}
                    //original.City = model.City;
                    //original.Zip = model.Zip;
                    ////original.Telephone = model.AgreeCheck;
                    //original.AlternateEmail = model.AlternateEmail;

                    //DataHelper.Instance.Update<AdminUser>(original, User.Username);

                    TempData["UpdateData"] = "Profile has been updated successfully.";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View(model);
            }
            return View(model);
        }

        [Authorize]
        [UrlPrivilegeFilter]
        public ActionResult MyProfile()
        {
            var model = new UserModel(User.Username);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult MyProfile(UserModel model)
        {

            if (ModelState.IsValid)
            {
                var profile = MemberService.Instance.Get(model.Id);
                var logoSize = Convert.ToInt32(ConfigService.Instance.GetConfigValue("LogoSize"));

                var fileSize = logoSize * 1024;
                if (model.Photo != null && model.Photo.ContentLength > fileSize)
                {
                    ModelState.AddModelError("", string.Format("Your photo exceeds the upload limit({0} KB).", fileSize));
                    return View(model);
                }

                profile.FirstName = model.FirstName;
                profile.LastName = model.LastName;
                profile.Address = model.Address;
                profile.CountryId = model.CountryId;
                profile.StateId = model.StateId;
                profile.City = model.City;
                profile.Zip = model.Zip;
                profile.Phone = model.Phone;
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    dataHelper.Update(profile, "Self");
                }
                TempData["UpdateData"] = "Successfully updated your profile.";
                View(model);
            }
            return View(model);
        }

        [Authorize(Roles = "SuperUser, Administrator, Sales1")]
        public ActionResult ContentViews(SitePagesModel model)
        {
            var names = new[] { "home", "aboutus", "termsofuse", "privacy", "copyright" };
            SitePage page = new SitePage();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                ViewBag.Model = dataHelper.Get<SitePage>().Where(x => names.Contains(x.PageId)).ToList();
            }
            return View("ContentViews");
        }

        [Authorize(Roles = "SuperUser, Administrator, Sales1")]
        public ActionResult UpdateContentViews(int id)
        {
            var model = new SitePagesModel(id);
            return View(model);
        }

        [Authorize(Roles = "SuperUser, Administrator, Sales1")]
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateContentViews(SitePagesModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var original = new SitePage();
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);

                        original = dataHelper.GetSingle<SitePage>(model.Id);
                        original.PageTitle = model.PageTitle;
                        original.PageKeywords = model.PageKeywords;
                        original.PageDescription = model.PageDescription;
                        original.PageContent = model.PageContent;

                        dataHelper.Update(original, User.Username);

                        TempData["UpdateData"] = string.Format("{0} has been updated successfully.", original.PageText);
                    }
                    return RedirectToAction("ContentViews");
                }
            }
            catch
            {
                return View(model);
            }
            return View(model);
        }

        public ActionResult ShowOtherContents(int id)
        {
            SitePage page = new SitePage();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                page = dataHelper.GetSingle<SitePage>("Id", 1);
            }
            ViewBag.PageContent = page.PageContent;

            return View("OtherContents");
        }

        public ActionResult NewsLetter(string email)
        {
            List<Newsletter> list = new List<Newsletter>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                list = dataHelper.Get<Newsletter>().OrderByDescending(x => x.DateCreated).ToList();
            }

            return View(list);
        }

        public ActionResult UpdateNews(long id)
        {
            var model = new NewsLetterModel(id);
            return View("UpdateNews");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateNews(NewsLetterModel model, FormCollection formdata)
        {
            try
            {
                var strgroup = Request["dropdownlist"];
                if (ModelState.IsValid)
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var original = dataHelper.Get<Newsletter>().FirstOrDefault(e => e.Id == model.Id);

                        original.Frequency = Convert.ToInt32(model.Frequency);
                        original.Type = Convert.ToInt32(strgroup);
                        original.CountryId = model.CountryId;
                        original.StartDate = model.StartDate;
                        original.Content = model.Content;

                        dataHelper.Update(original, User.Username);
                        TempData["UpdateData"] = string.Format("NewsLetters {0} has been updated successfully.", original.Id);
                    }
                    return RedirectToAction("NewsLetter");
                }
            }
            catch
            {
                return View(model);
            }
            return View(model);
        }

        #region Employer Active/In-Active Process

        public ActionResult IsEmpActivation(int id)
        {
            //EmployerService.Instance.ActivationEmployers(User.Username, id);
            return RedirectToAction("Employer");
        }

        #endregion Employer Active/In-Active Process

        public ActionResult ReportsAndLogs()
        {
            return View();
        }

        #region Private Methods

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return
                        "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        #endregion

        public string OpenPopup()
        {
            return "<h1> This Is Modeless Popup Window</h1>";
        }




        public ActionResult OtherContents()
        {
            var obj = new JobPortal.Data.Specialization();
            var subobj = new SubSpecialization();
            var model = new OtherContentsModel();
            model.CategoryId = Convert.ToInt32(obj.Id);
            model.SpecializationId = subobj.CategoryId;
            return View(model);
        }

        public ActionResult updateContent(int id, string strDescs, string strval, int seplz)
        {
            if (strval == "Cate")
            {
                strval = "";
                if ((id > 0) && (seplz > 0))
                {
                    strval = "Sub";
                    id = seplz;
                }
                else if ((id > 0) && (seplz == 0))
                {
                    strval = "Cat";
                }
            }
            //SharedService.Instance.updateContent(id, strDescs, strval, 0);
            TempData["UpdateData"] = "Data Updated Successfully!";

            return RedirectToAction("OtherContents", "Admin");
        }

        public ActionResult updateSEOcontent(long id, long seplz, string strTitle, string strKey, string strDescs,
            string strval)
        {
            if (strval == "Cate")
            {
                strval = "";
                if ((id > 0) && (seplz > 0))
                {
                    strval = "Sub";
                    id = seplz;
                }
                else
                {
                    strval = "Cat";
                }
            }
            //SharedService.Instance.updateseoContent(id, seplz, strTitle, strKey, strDescs, strval);
            TempData["UpdateData"] = "Data Updated Successfully!";

            return RedirectToAction("SeoContent", "Admin");
        }

        public ActionResult SeoContent()
        {
            var obj = new JobPortal.Data.Specialization();
            var subobj = new SubSpecialization();
            var model = new SeoMainModel();
            model.CategoryId = Convert.ToInt32(obj.Id);
            model.SpecializationId = subobj.CategoryId;
            return View(model);
        }


        [Authorize(Roles = "SuperUser, Administrator, Sales1")]
        public ActionResult Contacts(long? countryId, string fd, string fm, string fy, string td, string tm, string ty, int pageNumber = 0)
        {
            UserInfoEntity uinfo = _service.Get(User.Id);
            if (uinfo.Username == "tasnim@joblisting.com" || uinfo.Username == "lakshmii@joblisting.com"||uinfo.Username== "Sailajaadmin@joblisting.com")
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

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
            {
                countryId = 40;
            }

            List<ConnectionsByCountryModel> list = new List<ConnectionsByCountryModel>();
            int rows = 0;
            int pageSize = 100;
            string sdate = string.Empty;
            string edate = string.Empty;
            var sdt = new DateTime?();
            var edt = new DateTime?();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var connections = dataHelper.Get<UserProfile>();

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
                if (sdt != null && edt != null)
                {
                    connections = connections.Where(x => x.DateCreated.Day >= sdt.Value.Day && x.DateCreated.Month >= sdt.Value.Month && x.DateCreated.Year >= sdt.Value.Year && x.DateCreated.Day <= edt.Value.Day && x.DateCreated.Month <= edt.Value.Month && x.DateCreated.Year <= edt.Value.Year);
                }

                var users = connections.Where(x => x.IsDeleted == false && x.IsActive == true && x.Connections.Count > 0);

                list = users.Where(x => x.CountryId != null).GroupBy(x => new { CountryId = x.CountryId, Code = x.Country.Value, Country = x.Country.Text })
                    .Select(x => new ConnectionsByCountryModel()
                    {
                        CountryId = x.Key.CountryId.Value,
                        Country = x.Key.Country,
                        Code = x.Key.Code,
                        Users = x.Count(),
                        Individuals = x.Count(z => z.Type == 4 && z.IsActive == true && z.IsDeleted == false && z.Connections.Count > 0),
                        Companies = x.Count(z => z.Type == 5 && z.IsActive == true && z.IsDeleted == false && z.Connections.Count > 0)
                    }).ToList<ConnectionsByCountryModel>();
                if (countryId != null)
                {
                    list = list.Where(x => x.CountryId == countryId.Value).ToList();
                }

                rows = list.Count;
                list = list.OrderByDescending(x => x.Country).Skip(pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
            }
            ViewBag.Individuals = list.Sum(x => x.Individuals);
            ViewBag.Companies = list.Sum(x => x.Companies);
            if (countryId != null)
            {
                ViewBag.Country = SharedService.Instance.GetCountry(countryId.Value);
            }
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<ConnectionsByCountryModel>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);

            return View();
        }

        [Authorize(Roles = "Administrator, SuperUser, Sales1")]
        public ActionResult ResumeList(int? CategoryId = null, int? SpecializationId = null, int? CountryId = null, DateTime? StartDate = null, DateTime? EndDate = null, int pageNumber = 0)
        {
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

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || User.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
            {
                CountryId = 40;
            }

            int rows = 0;
            int pageSize = 10;
            var resumes = new List<Resume>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<Resume>();

                if (CategoryId != null)
                {
                    result = result.Where(x => x.CategoryId == CategoryId.Value);
                }
                if (SpecializationId != null)
                {
                    result = result.Where(x => x.SpecializationId == SpecializationId.Value);
                }

                if (CountryId != null)
                {
                    result = result.Where(x => x.CountryId == CountryId.Value);
                }

                if (StartDate != null)
                {
                    result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                }

                if (EndDate != null)
                {
                    if (StartDate == null)
                    {
                        result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                    }
                    result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                }
                else
                {
                    if (StartDate != null)
                    {
                        result = result.Where(x => x.DateCreated.Day <= DateTime.Now.Day && x.DateCreated.Month <= DateTime.Now.Month && x.DateCreated.Year <= DateTime.Now.Year);
                    }
                }
                rows = result.Count();
                resumes = result.OrderByDescending(x => x.DateCreated).Skip(pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
            }
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<Resume>(resumes, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
            return View();
        }

        [Authorize(Roles = "Administrator, SuperUser, Sales1")]
        public ActionResult Download(long id, string redirect)
        {
            if (User != null)
            {
                UserProfile jobSeeker = new UserProfile();
                string message = string.Empty;

                jobSeeker = MemberService.Instance.Get(id);
                if (jobSeeker != null)
                {
                    if (!string.IsNullOrEmpty(jobSeeker.Content))
                    {
                        return File(Convert.FromBase64String(jobSeeker.Content), MediaTypeNames.Application.Octet, jobSeeker.FileName);
                    }
                }
            }
            return Redirect(redirect);
        }

        [Authorize(Roles = "Administrator, SuperUser, Sales1")]
        public ActionResult DeleteResume(long Id)
        {
            var resumes = new List<Resume>();
            var resume = SharedService.Instance.GetResume(Id);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                if (resume != null)
                {
                    dataHelper.Remove(resume);
                }
                resumes = dataHelper.Get<Resume>().Where(x => x.CreatedBy == User.Username).ToList();
            }
            return Redirect("/Admin/ResumeList");
        }

        [Authorize(Roles = "Administrator, SuperUser, Sales1")]
        public ActionResult DeleteAccount(long Id)
        {
            int stat = DomainService.Instance.DeleteUserAccount(Id);
            if (stat > 0)
            {
                TempData["UpdateData"] = "User account has been deleted successfully!";
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        [Authorize(Roles = "Administrator, SuperUser, Sales1")]
        public ActionResult UploadResume()
        {
            var model = new UploadResumeModal();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, SuperUser, Sales1")]
        public ActionResult UploadResume(UploadResumeModal model)
        {
            Resume entity = new Resume();
            string extension = string.Empty;
            string fileName = string.Empty;
            string message = string.Empty;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                if (model.UploadResume1 != null || model.UploadResume2 != null || model.UploadResume3 != null || model.UploadResume4 != null)
                {
                    if (model.UploadResume1 != null)
                    {
                        if (!string.IsNullOrEmpty(model.TitleOne) && model.CategoryIdOne > 0 && model.SpecializationIdOne > 0 && model.CountryIdOne > 0)
                        {
                            extension = model.UploadResume1.FileName.Substring(model.UploadResume1.FileName.LastIndexOf(".") + 1).ToUpper();
                            fileName = model.UploadResume1.FileName;

                            if (extension == "PDF" || extension == "DOC" || extension == "DOCX")
                            {
                                var file = model.UploadResume1.InputStream;
                                var buffer = new byte[file.Length];
                                file.Read(buffer, 0, (int)file.Length);
                                file.Close();

                                if (buffer.Length < 18)
                                {
                                    ModelState.AddModelError("", "Uploading file should have minimum size of 18 KB.");
                                    return View();
                                }

                                entity = new Resume();
                                entity.Title = model.TitleOne;
                                entity.CategoryId = model.CategoryIdOne;
                                entity.SpecializationId = model.SpecializationIdOne;
                                entity.CountryId = model.CountryIdOne;
                                entity.StateId = model.StateIdOne;
                                entity.City = model.City1;
                                entity.Experience = (byte?)model.Experience1;
                                entity.MinimumAge = (byte?)model.MinimumAge1;
                                entity.MaximumAge = (byte?)model.MaximumAge1;
                                entity.QualificationId = model.QualificationId1;
                                entity.Type = extension;
                                entity.FileName = fileName;
                                entity.Content = Convert.ToBase64String(buffer);
                                entity.CreatedBy = User.Username;
                                entity.DateCreated = DateTime.Now;
                                entity.IsActive = true;
                                entity.IsDeleted = false;

                                dataHelper.AddEntity(entity, User.Username);

                            }
                            else
                            {
                                message += string.Format("{0} is not type of .doc, docx, pdf file!", model.UploadResume1.FileName);
                            }
                        }
                        else
                        {
                            message += "Please provide Title, Category, Specialization and Country not provided while uploading 1st resume!";
                        }
                    }

                    if (model.UploadResume2 != null)
                    {
                        if (!string.IsNullOrEmpty(model.TitleTwo) && model.CategoryIdTwo > 0 && model.SpecializationIdTwo > 0 && model.CountryIdTwo > 0)
                        {
                            extension = model.UploadResume2.FileName.Substring(model.UploadResume2.FileName.LastIndexOf(".") + 1).ToUpper();
                            fileName = model.UploadResume2.FileName;

                            if (extension == "PDF" || extension == "DOC" || extension == "DOCX")
                            {
                                var file = model.UploadResume2.InputStream;
                                var buffer = new byte[file.Length];
                                file.Read(buffer, 0, (int)file.Length);
                                file.Close();

                                if (buffer.Length < 18)
                                {
                                    ModelState.AddModelError("", "Uploading file should have minimum size of 18 KB.");
                                    return View();
                                }

                                entity = new Resume();
                                entity.Title = model.TitleTwo;
                                entity.CategoryId = model.CategoryIdTwo;
                                entity.SpecializationId = model.SpecializationIdTwo;
                                entity.CountryId = model.CountryIdTwo;
                                entity.StateId = model.StateIdTwo;
                                entity.City = model.City2;
                                entity.Experience = (byte?)model.Experience2;
                                entity.MinimumAge = (byte?)model.MinimumAge2;
                                entity.MaximumAge = (byte?)model.MaximumAge2;
                                entity.QualificationId = model.QualificationId2;
                                entity.Type = extension;
                                entity.FileName = fileName;
                                entity.Content = Convert.ToBase64String(buffer);
                                entity.CreatedBy = User.Username;
                                entity.DateCreated = DateTime.Now;
                                entity.IsActive = true;
                                entity.IsDeleted = false;

                                dataHelper.Add(entity, User.Username);

                            }
                            else
                            {
                                message += string.Format("{0} is not type of .doc, docx, pdf file!", model.UploadResume2.FileName);
                            }
                        }
                        else
                        {
                            message += "Title, Category, Specialization and Country not provided while uploading 2nd Resume!";
                        }
                    }

                    if (model.UploadResume3 != null)
                    {
                        if (!string.IsNullOrEmpty(model.TitleThree) && model.CategoryIdThree > 0 && model.SpecializationIdThree > 0 && model.CountryIdThree > 0)
                        {
                            extension = model.UploadResume3.FileName.Substring(model.UploadResume3.FileName.LastIndexOf(".") + 1).ToUpper();
                            fileName = model.UploadResume3.FileName;

                            if (extension == "PDF" || extension == "DOC" || extension == "DOCX")
                            {
                                var file = model.UploadResume3.InputStream;
                                var buffer = new byte[file.Length];
                                file.Read(buffer, 0, (int)file.Length);
                                file.Close();

                                if (buffer.Length < 18)
                                {
                                    ModelState.AddModelError("", "Uploading file should have minimum size of 18 KB.");
                                    return View();
                                }

                                entity = new Resume();
                                entity.Title = model.TitleThree;
                                entity.CategoryId = model.CategoryIdThree;
                                entity.SpecializationId = model.SpecializationIdThree;
                                entity.CountryId = model.CountryIdThree;
                                entity.StateId = model.StateIdThree;
                                entity.City = model.City3;
                                entity.Experience = (byte?)model.Experience3;
                                entity.MinimumAge = (byte?)model.MinimumAge3;
                                entity.MaximumAge = (byte?)model.MaximumAge3;
                                entity.QualificationId = model.QualificationId3;
                                entity.Type = extension;
                                entity.FileName = fileName;
                                entity.Content = Convert.ToBase64String(buffer);
                                entity.CreatedBy = User.Username;
                                entity.DateCreated = DateTime.Now;
                                entity.IsActive = true;
                                entity.IsDeleted = false;

                                dataHelper.Add(entity, User.Username);

                            }
                            else
                            {
                                message += string.Format("{0} is not type of .doc, docx, pdf file!", model.UploadResume3.FileName);
                            }
                        }
                        else
                        {
                            message += "Title, Category, Specialization and Country not provided while uploading 3rd Resume!";
                        }
                    }

                    if (model.UploadResume4 != null)
                    {
                        if (!string.IsNullOrEmpty(model.TitleFour) && model.CategoryIdFour > 0 && model.SpecializationIdFour > 0 && model.CountryIdFour > 0)
                        {
                            extension = model.UploadResume4.FileName.Substring(model.UploadResume4.FileName.LastIndexOf(".") + 1).ToUpper();
                            fileName = model.UploadResume4.FileName;

                            if (extension == "PDF" || extension == "DOC" || extension == "DOCX")
                            {
                                var file = model.UploadResume4.InputStream;
                                var buffer = new byte[file.Length];
                                file.Read(buffer, 0, (int)file.Length);
                                file.Close();

                                if (buffer.Length < 18)
                                {
                                    ModelState.AddModelError("", "Uploading file should have minimum size of 18 KB.");
                                    return View();
                                }

                                entity = new Resume();
                                entity.Title = model.TitleFour;
                                entity.CategoryId = model.CategoryIdFour;
                                entity.SpecializationId = model.SpecializationIdFour;
                                entity.CountryId = model.CountryIdFour;
                                entity.StateId = model.StateIdFour;
                                entity.City = model.City4;
                                entity.Experience = (byte?)model.Experience4;
                                entity.MinimumAge = (byte?)model.MinimumAge4;
                                entity.MaximumAge = (byte?)model.MaximumAge4;
                                entity.QualificationId = model.QualificationId4;
                                entity.Type = extension;
                                entity.FileName = fileName;
                                entity.Content = Convert.ToBase64String(buffer);
                                entity.CreatedBy = User.Username;
                                entity.DateCreated = DateTime.Now;
                                entity.IsActive = true;
                                entity.IsDeleted = false;

                                dataHelper.Add(entity, User.Username);

                            }
                            else
                            {
                                message += string.Format("{0} is not type of .doc, docx, pdf file!", model.UploadResume4.FileName);
                            }
                        }
                        else
                        {
                            message += "Title, Category, Specialization and Country not provided while uploading 4th resume!";
                        }
                        TempData["SaveData"] = "Resumes uploaded successfully.";
                        dataHelper.Save();
                    }
                }
            }

            return RedirectToAction("ResumeList");
        }

        public ActionResult CategoryList(int pageNumber = 0)
        {
            List<JobPortal.Data.Specialization> categories = new List<JobPortal.Data.Specialization>();
            int rows = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                rows = dataHelper.GetCounts<JobPortal.Data.Specialization>();

                categories = dataHelper.Get<JobPortal.Data.Specialization>().Where(x => x.IsDeleted == false).OrderBy(x => x.Name).Skip(pageNumber > 0 ? (pageNumber - 1) * 15 : pageNumber * 15).Take(15).ToList();
            }
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<JobPortal.Data.Specialization>(categories, (pageNumber == 0 ? 1 : pageNumber), 15, rows);
            return View();
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            return View(new CategoryModel());
        }

        [HttpPost]
        public ActionResult AddCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new JobPortal.Data.Specialization
                {
                    Name = model.Name,
                    Title = model.Title,
                    Description = model.Description,
                    Keywords = model.Keywords,
                    FullName =
                        model.Name.Replace(' ', '-')
                            .Replace('&', '-')
                            .Replace(" & ", "-")
                            .Replace("  ", "-")
                            .Replace(',', '-')
                            .Replace(", ", "-")
                            .Replace(" / ", "-")
                            .Replace('/', '-')
                            .Replace(" - ", "-"),
                    UpdatedBy = User.Username,
                    DateUpdated = DateTime.Now
                };

                var id = 0;
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    id = Convert.ToInt32(dataHelper.Add(category, User.Username));
                }
                if (id > 0)
                {
                    TempData["SaveData"] = string.Format("Category <b>{0}</b> added successfully!", category.Name);

                    return RedirectToAction("CategoryList");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditCategory(int Id)
        {
            var category = SharedService.Instance.GetCategory(Id);
            var model = new CategoryModel
            {
                Id = category.Id,
                Name = category.Name,
                Title = category.Title,
                Description = category.Description,
                Keywords = category.Keywords
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var category = SharedService.Instance.GetCategory(model.Id);
                if (category != null)
                {
                    category.Name = model.Name;
                    category.Title = model.Title;
                    category.Description = model.Description;
                    category.Keywords = model.Keywords;
                    category.FullName =
                        model.Name.Replace(' ', '-')
                            .Replace('&', '-')
                            .Replace(" & ", "-")
                            .Replace("  ", "-")
                            .Replace(',', '-')
                            .Replace(", ", "-")
                            .Replace(" / ", "-")
                            .Replace('/', '-')
                            .Replace(" - ", "-");
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        dataHelper.Update(category, User.Username);
                    }
                }
                TempData["SaveData"] = string.Format("Category \"{0}\" updated successfully!", category.Name);
                return RedirectToAction("CategoryList");
            }
            return View(model);
        }

        public ActionResult DeleteCategory(int Id)
        {
            var category = SharedService.Instance.GetCategory(Id);
            if (category != null)
            {
                TempData["DeleteData"] = string.Format("Category {0} has been deleted!", category.Name);
                try
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        dataHelper.Remove<JobPortal.Data.Specialization>(category);
                    }
                }
                catch (Exception)
                {
                    TempData["DeleteData"] = "";
                }
            }
            return RedirectToAction("CategoryList");
        }

        public ActionResult SpecializationList(int Id)
        {
            var specializations = new List<SubSpecialization>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                specializations = dataHelper.Get<SubSpecialization>().Where(x => x.CategoryId == Id && x.IsDeleted == false).OrderBy(x => x.Name).ToList();
            }
            ViewBag.CategoryId = Id;
            return View(specializations);
        }

        [HttpGet]
        public ActionResult AddSpecialization(int CategoryId)
        {
            return View(new SpecializationModel() { CategoryId = CategoryId });
        }

        [HttpPost]
        public ActionResult AddSpecialization(SpecializationModel model)
        {
            if (ModelState.IsValid)
            {
                var specialization = new SubSpecialization
                {
                    CategoryId = model.CategoryId,
                    Name = model.Name,
                    Title = model.Title,
                    Description = model.Description,
                    Keywords = model.Keywords,
                    UpdatedBy = User.Username,
                    DateUpdated = DateTime.Now
                };
                var id = 0;
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    id = Convert.ToInt32(dataHelper.Add(specialization, User.Username));
                }
                if (id > 0)
                {
                    TempData["SaveData"] = string.Format("Specialization <b>{0}</b> added successfully!", specialization.Name);

                    return RedirectToAction("SpecializationList", new { Id = model.CategoryId });
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditSpecialization(int Id)
        {
            var Specialization = SharedService.Instance.GetSpecialization(Id);
            var model = new SpecializationModel
            {
                Id = Specialization.Id,
                CategoryId = Specialization.CategoryId,
                Name = Specialization.Name,
                Title = Specialization.Title,
                Description = Specialization.Description,
                Keywords = Specialization.Keywords
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditSpecialization(SpecializationModel model)
        {
            if (ModelState.IsValid)
            {
                var specialization = SharedService.Instance.GetSpecialization(model.Id);
                if (specialization != null)
                {
                    specialization.CategoryId = model.CategoryId;
                    specialization.Name = model.Name;
                    specialization.Title = model.Title;
                    specialization.Description = model.Description;
                    specialization.Keywords = model.Keywords;
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        dataHelper.Update(specialization, User.Username);
                    }
                }
                TempData["SaveData"] = string.Format("Specialization \"{0}\" updated successfully!", specialization.Name);

                return RedirectToAction("SpecializationList", new { Id = specialization.CategoryId });
            }
            return View(model);
        }

        public ActionResult DeleteSpecialization(int Id)
        {
            var specialization = SharedService.Instance.GetSpecialization(Id);
            if (specialization != null)
            {
                TempData["DeleteData"] = string.Format("Specialization <b>{0}</b> has been deleted!", specialization.Name);
                try
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        dataHelper.Remove<SubSpecialization>(specialization);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }
            return RedirectToAction("SpecializationList", new { Id = specialization.CategoryId });
        }

        #region Start Jobg8 Section

        public ActionResult JobsByCategory(long countryId, int categoryId, int? typeId, DateTime? startDate, DateTime? endDate, int pageNumber = 0)
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

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
            {
                countryId = 40;
            }

            List country = SharedService.Instance.GetCountry(countryId);
            JobPortal.Data.Specialization category = SharedService.Instance.GetCategory(categoryId);
            List<Job> jobs = new List<Job>();
            int rows = 0;
            int pageSize = 10;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<Job>().Where(x => x.CountryId == countryId && x.CategoryId == categoryId);
                var sdt = new DateTime?();
                var edt = new DateTime?();

                if (typeId != null)
                {
                    switch (typeId.Value)
                    {
                        case 1:
                            result = result.Where(x => x.IsActive == true && x.IsPublished == true);
                            break;
                        case 2:
                            result = result.Where(x => x.ClosingDate.Day <= DateTime.Now.Day && x.ClosingDate.Month <= DateTime.Now.Month && x.ClosingDate.Year < DateTime.Now.Year);
                            break;
                        case 3:
                            result = result.Where(x => x.IsDeleted == true);
                            break;
                    }
                }

                if (startDate != null || endDate != null)
                {
                    if (startDate != null)
                    {
                        sdt = Convert.ToDateTime(startDate);
                    }

                    if (endDate != null)
                    {
                        if (startDate == null)
                        {
                            sdt = DateTime.Now;
                        }
                        edt = Convert.ToDateTime(endDate);
                    }
                    else
                    {
                        if (startDate != null)
                        {
                            edt = DateTime.Now;
                        }
                    }
                }
                if (sdt != null)
                {
                    result = result.Where(x => x.PublishedDate.Day >= sdt.Value.Day && x.PublishedDate.Month >= sdt.Value.Month && x.PublishedDate.Year >= sdt.Value.Year);
                }

                if (edt != null)
                {
                    result = result.Where(x => x.PublishedDate.Day <= edt.Value.Day && x.PublishedDate.Month <= edt.Value.Month && x.PublishedDate.Year <= edt.Value.Year);
                }

                rows = result.Count();
                jobs = result.OrderByDescending(x => x.DateCreated).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
            }

            ViewBag.TypeId = typeId;
            ViewBag.Country = country;
            ViewBag.Category = category;
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<Job>(jobs, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);

            return View();
        }

        public ActionResult JobsByCountry(long countryId, string name, string fd, string fm, string fy, string td, string tm, string ty, string ShowWithValue = "", int pageNumber = 1)
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

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
            {
                countryId = 40;
            }

            List country = SharedService.Instance.GetCountry(countryId);
            int rows = 0;
            int pageSize = 10;
            string sdate = string.Empty;
            string edate = string.Empty;
            var sdt = new DateTime?();
            var edt = new DateTime?();
            List<JobDetailByCountryEntity> list = new List<JobDetailByCountryEntity>();

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

            if (ShowWithValue == "1")
            {
                list = DomainService.Instance.GetJobListByCountry(countryId, name, true, sdt, edt, pageSize, pageNumber);
            }
            else
            {
                list = DomainService.Instance.GetJobListByCountry(countryId, name, null, sdt, edt, pageSize, pageNumber);
            }

            if (list.Count > 0)
            {
                rows = list.FirstOrDefault().MaxRows;
            }

            ViewBag.Live = list.Sum(x => x.Live);
            ViewBag.Deactivated = list.Sum(x => x.Deactivated);
            ViewBag.Expired = list.Sum(x => x.Expired);
            ViewBag.Deleted = list.Sum(x => x.Deleted);
            ViewBag.Rejected = list.Sum(x => x.Rejected);
            ViewBag.Applications = list.Sum(x => x.Applications);
            ViewBag.Waiting = list.Sum(x => x.Waiting);

            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<JobDetailByCountryEntity>(list, pageNumber, pageSize, rows);

            ViewBag.Country = country;
            return View();
        }

        public ActionResult JobListByCountry(long countryId)
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

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
            {
                countryId = 40;
            }

            List country = SharedService.Instance.GetCountry(countryId);

            //using (JobPortalEntities context = new JobPortalEntities())
            //{
            //    DataHelper dataHelper = new DataHelper(context);
            //    var result = dataHelper.Get<UserProfile>().Where(x => x.Type == 5 && x.CountryId == countryId).Select(x => new { Id = x.UserId, Company = x.Company, ActiveJobs = x.Jobs.Count(z => z.CountryId == countryId && z.IsActive == true && z.IsPublished == true), ExpiredJobs = x.Jobs.Count(z => z.CountryId == countryId && z.ClosingDate.Day <= DateTime.Now.Day && z.ClosingDate.Month < DateTime.Now.Month && z.ClosingDate.Year <= DateTime.Now.Year), DeletedJobs = x.Jobs.Count(z => z.CountryId == countryId && z.IsDeleted == true) });
            //}
            ViewBag.Country = country;
            return View();
        }

        [UrlPrivilegeFilter]
        public ActionResult ProfilesLists(int? i, string skil = null,string ig=null)
     {

            string skil1 = skil;
            string re = skil1.Remove(3);
            string skills = "" + re + "%";
            ViewBag.profiles = null;
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            if (ig != null)
            {
                int id = Convert.ToInt32(ig);

                string countryName = string.Empty;
                List country = SharedService.Instance.GetCountry(id);
                if (country != null)
                {
                    countryName = country.Text;
                }

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    //string skills = "%" + itemss.summary + "%";

                    List<User_Skills> profilee = new List<User_Skills>();
                    List<UserInfoEntity> profile1 = new List<UserInfoEntity>();
                    profilee = MemberService.Instance.GetUserskillProfiles(skills);
                    int count = 0;
                    foreach (var item1 in profilee)
                    {
                        if (User.Username == "sandhya@accuracy.com.sg" || User.Username == "lakshmip@accuracy.com.sg" || User.Username == "haris@joblisting.com" || User.Username == "anilkumar@joblisting.com" || User.Username == "denise@accuracy.com.sg" || User.Username == "dianna@accuracy.com.sg")
                        {

                            var profile11 = MemberService.Instance.GetUserInfo(item1.UserId);
                            if (profile11.CountryName == countryName)
                            {
                                profile1.Add(profile11);
                                ViewBag.profiles = profile1.ToPagedList(i ?? 1, 10);
                                count++;
                            }

                        }
                        else if (User.Username == "gowthami@accuracy.com.sg" || User.Username == "naveena@accuracy.com.sg" || User.Username == "sarika123@accuracy.com.sg" || User.Username == "vanshika@accuracy.com.sg" || User.Username == "Doli.chauhan123@accuracy.com.sg"||User.Username== "doli.chauhan123@accuracy.com.sg")

                        {

                            var profile11 = MemberService.Instance.GetUserInfo(item1.UserId);
                            if (profile11.CountryName == "india" || profile11.CountryName == "India")
                            {
                                profile1.Add(profile11);
                                ViewBag.profiles = profile1.ToPagedList(i ?? 1, 10);
                                count++;
                            }
                        }
                        else if (User.Username == "anshikagupta@accuracy.com.sg" || User.Username == "baba@accuracy.com.sg" || User.Username == "pallavi@accuracy.com.sg" || User.Username == "Shreyag@accuracy.com.sg" || User.Username == "tasnim@accuracy.com.sg" || User.Username == "shreyag1234@accuracy.com.sg" || User.Username == "pallavi123@accuracy.com.sg")

                        {

                            var profile11 = MemberService.Instance.GetUserInfo(item1.UserId);
                            if (profile11.CountryName == "india" || profile11.CountryName == "India")
                            {
                                profile1.Add(profile11);
                                ViewBag.profiles = profile1.ToPagedList(i ?? 1, 10);
                                count++;
                            }
                        }
                        else if (User.Username == "vani123@accuracy.com.sg" || User.Username == "ganeshr@joblisting.com" || User.Username == "deepti@accuracy.com.sg")

                        {

                            var profile11 = MemberService.Instance.GetUserInfo(item1.UserId);
                            if (profile11.CountryName == "india" || profile11.CountryName == "India")
                            {
                                profile1.Add(profile11);
                                ViewBag.profiles = profile1.ToPagedList(i ?? 1, 10);
                                count++;
                            }
                        }
                    }
                }
            }
            // string skill = skil;
            else
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    //string skills = "%" + itemss.summary + "%";

                    List<User_Skills> profilee = new List<User_Skills>();
                    List<UserInfoEntity> profile1 = new List<UserInfoEntity>();
                    profilee = MemberService.Instance.GetUserskillProfiles(skills);
                    int count = 0;
                    foreach (var item1 in profilee)
                    {
                        if (User.Username == "sandhya@accuracy.com.sg" || User.Username == "lakshmip@accuracy.com.sg" || User.Username == "denise@accuracy.com.sg" || User.Username == "dianna@accuracy.com.sg")
                        {

                            var profile11 = MemberService.Instance.GetUserInfo(item1.UserId);
                            if (profile11.CountryName == "Singapore" || profile11.CountryName == "Malaysia")
                            {
                                profile1.Add(profile11);
                                ViewBag.profiles = profile1.ToPagedList(i ?? 1, 10);
                                count++;
                            }

                        }
                        else if (User.Username == "gowthami@accuracy.com.sg" || User.Username == "naveena@accuracy.com.sg" || User.Username == "sarika123@accuracy.com.sg" || User.Username == "vanshika@accuracy.com.sg" || User.Username == "Doli.chauhan123@accuracy.com.sg" || User.Username == "doli.chauhan123@accuracy.com.sg")

                        {

                            var profile11 = MemberService.Instance.GetUserInfo(item1.UserId);
                            if (profile11.CountryName == "india" || profile11.CountryName == "India")
                            {
                                profile1.Add(profile11);
                                ViewBag.profiles = profile1.ToPagedList(i ?? 1, 10);
                                count++;
                            }
                        }
                        else if (User.Username == "anshikagupta@accuracy.com.sg" || User.Username == "baba@accuracy.com.sg" || User.Username == "pallavi@accuracy.com.sg" || User.Username == "Shreyag@accuracy.com.sg" || User.Username == "tasnim@accuracy.com.sg" || User.Username == "shreyag1234@accuracy.com.sg" || User.Username == "pallavi123@accuracy.com.sg")

                        {

                            var profile11 = MemberService.Instance.GetUserInfo(item1.UserId);
                            if (profile11.CountryName == "india" || profile11.CountryName == "India")
                            {
                                profile1.Add(profile11);
                                ViewBag.profiles = profile1.ToPagedList(i ?? 1, 10);
                                count++;
                            }
                        }
                        else if (User.Username == "vani123@accuracy.com.sg" || User.Username == "ganeshr@joblisting.com" || User.Username == "deepti@accuracy.com.sg")

                        {

                            var profile11 = MemberService.Instance.GetUserInfo(item1.UserId);
                            if (profile11.CountryName == "india" || profile11.CountryName == "India")
                            {
                                profile1.Add(profile11);
                                ViewBag.profiles = profile1.ToPagedList(i ?? 1, 10);
                                count++;
                            }
                        }



                    }
                }
            }
            

            return View();
        }

        [UrlPrivilegeFilter]
        public ActionResult Monsterlist(int? i, string skil = null, string ig = null)
        {
            string skil1 = skil;
            string re = skil1.Remove(3);
            string skills = "" + re + "%";
            // string skill = @skil;
            ViewBag.profiles1 = null;
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            List<Monster> customers = new List<Monster>();

            if(ig!=null)
            {
                int id = Convert.ToInt32(ig);

                string countryName = string.Empty;
                List country = SharedService.Instance.GetCountry(id);
                if (country != null)
                {
                    countryName = country.Text;
                }
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    if (User.Username == "sandhya@accuracy.com.sg" || User.Username == "lakshmip@accuracy.com.sg" || User.Username == "denise@accuracy.com.sg" || User.Username == "dianna@accuracy.com.sg")
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT id,name,email,Contact_No,Prf_Location,Location,Resume, Designation,skills_count,Total_Score,Folder,id FROM Monstar2 where Designation LIKE '" + skills + "' AND Prf_Location LIKE '"+ countryName + "%' ", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                conn.Open();
                                using (SqlDataReader sdr = cmd.ExecuteReader())
                                {

                                    while (sdr.Read())
                                    {
                                        customers.Add(new Monster
                                        {

                                            id = (int)sdr["id"],
                                            Name = sdr["Name"].ToString(),
                                            Designation = sdr["Designation"].ToString(),
                                            Email = sdr["Email"].ToString(),
                                            Contact_No = sdr["Contact_No"].ToString(),
                                            Location = sdr["Prf_Location"].ToString(),
                                            Resume = sdr["Resume"].ToString(),
                                            Total_Score = sdr["Total_Score"].ToString(),
                                            skills_count = sdr["skills_count"].ToString(),
                                            Experiance = sdr["Folder"].ToString(),

                                        });
                                    }
                                    ViewBag.profiles1 = customers.ToPagedList(i ?? 1, 10);
                                }
                            }
                        }
                    }
                    else if (User.Username == "gowthami@accuracy.com.sg" || User.Username == "haris@joblisting.com" || User.Username == "anilkumar@joblisting.com" || User.Username == "naveena@accuracy.com.sg" || User.Username == "sarika123@accuracy.com.sg" || User.Username == "vanshika@accuracy.com.sg" || User.Username == "Doli.chauhan123@accuracy.com.sg" || User.Username == "doli.chauhan123@accuracy.com.sg")

                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT id,name,email,Contact_No,Location,Resume,skills_count,Total_Score, Designation,Folder,id FROM Monstar2 where Designation LIKE '" + skills + "'", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                conn.Open();
                                using (SqlDataReader sdr = cmd.ExecuteReader())
                                {

                                    while (sdr.Read())
                                    {
                                        customers.Add(new Monster
                                        {

                                            id = (int)sdr["id"],
                                            Name = sdr["Name"].ToString(),
                                            Designation = sdr["Designation"].ToString(),
                                            Email = sdr["Email"].ToString(),
                                            Contact_No = sdr["Contact_No"].ToString(),
                                            Location = sdr["Location"].ToString(),
                                            Resume = sdr["Resume"].ToString(),
                                            skills_count = sdr["skills_count"].ToString(),
                                            Total_Score = sdr["Total_Score"].ToString(),
                                            Experiance = sdr["Folder"].ToString(),

                                        });
                                    }
                                    ViewBag.profiles1 = customers.ToPagedList(i ?? 1, 10);
                                }
                            }
                        }
                    }
                    else if (User.Username == "anshikagupta@accuracy.com.sg" || User.Username == "baba@accuracy.com.sg" || User.Username == "pallavi@accuracy.com.sg" || User.Username == "Shreyag@accuracy.com.sg" || User.Username == "tasnim@accuracy.com.sg" || User.Username == "shreyag1234@accuracy.com.sg"||User.Username== "pallavi123@accuracy.com.sg")

                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT id,name,email,Contact_No,Location,Resume,skills_count,Total_Score,skills_count, Designation,Folder,id FROM Monstar2 where Designation LIKE '" + skills + "' AND Location like '%india%' ", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                conn.Open();
                                using (SqlDataReader sdr = cmd.ExecuteReader())
                                {

                                    while (sdr.Read())
                                    {
                                        customers.Add(new Monster
                                        {

                                            id = (int)sdr["id"],
                                            Name = sdr["Name"].ToString(),
                                            Designation = sdr["Designation"].ToString(),
                                            Email = sdr["Email"].ToString(),
                                            Contact_No = sdr["Contact_No"].ToString(),
                                            Location = sdr["Location"].ToString(),
                                            Resume = sdr["Resume"].ToString(),
                                            skills_count = sdr["skills_count"].ToString(),
                                            Total_Score = sdr["Total_Score"].ToString(),
                                            Experiance = sdr["Folder"].ToString(),

                                        });
                                    }
                                    ViewBag.profiles1 = customers.ToPagedList(i ?? 1, 10);
                                }
                            }
                        }
                    }
                    else if (User.Username == "vani123@accuracy.com.sg" || User.Username=="ganeshr@joblisting.com" || User.Username == "deepti@accuracy.com.sg")

                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT id,name,email,Contact_No,Location,Resume,skills_count,Total_Score, Designation,Folder,id FROM Monstar2 where Designation LIKE '" + skills + "' ", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                conn.Open();
                                using (SqlDataReader sdr = cmd.ExecuteReader())
                                {

                                    while (sdr.Read())
                                    {
                                        customers.Add(new Monster
                                        {

                                            id = (int)sdr["id"],
                                            Name = sdr["Name"].ToString(),
                                            Designation = sdr["Designation"].ToString(),
                                            Email = sdr["Email"].ToString(),
                                            Contact_No = sdr["Contact_No"].ToString(),
                                            Location = sdr["Location"].ToString(),
                                            Resume = sdr["Resume"].ToString(),
                                            skills_count = sdr["skills_count"].ToString(),
                                            Total_Score = sdr["Total_Score"].ToString(),
                                            Experiance = sdr["Folder"].ToString(),

                                        });
                                    }
                                    ViewBag.profiles1 = customers.ToPagedList(i ?? 1, 10);
                                }
                            }
                        }
                    }

                }
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    if (User.Username == "sandhya@accuracy.com.sg" || User.Username == "lakshmip@accuracy.com.sg" || User.Username == "denise@accuracy.com.sg" || User.Username == "dianna@accuracy.com.sg")
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT id,name,email,Contact_No,Prf_Location,Location,Resume,skills_count,Total_Score, Designation,Folder,id FROM Monstar2 where Designation LIKE '" + skills + "' AND Prf_Location LIKE '%Singapore%' ", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                conn.Open();
                                using (SqlDataReader sdr = cmd.ExecuteReader())
                                {

                                    while (sdr.Read())
                                    {
                                        customers.Add(new Monster
                                        {

                                            id = (int)sdr["id"],
                                            Name = sdr["Name"].ToString(),
                                            Designation = sdr["Designation"].ToString(),
                                            Email = sdr["Email"].ToString(),
                                            Contact_No = sdr["Contact_No"].ToString(),
                                            Location = sdr["Prf_Location"].ToString(),
                                            Resume = sdr["Resume"].ToString(),
                                            Total_Score = sdr["Total_Score"].ToString(),
                                            skills_count = sdr["skills_count"].ToString(),
                                            Experiance = sdr["Folder"].ToString(),

                                        });
                                    }
                                    ViewBag.profiles1 = customers.ToPagedList(i ?? 1, 10);
                                }
                            }
                        }
                    }
                    else if (User.Username == "gowthami@accuracy.com.sg" || User.Username == "haris@joblisting.com" || User.Username == "anilkumar@joblisting.com" || User.Username == "naveena@accuracy.com.sg" || User.Username == "sarika123@accuracy.com.sg" || User.Username == "vanshika@accuracy.com.sg" || User.Username == "Doli.chauhan123@accuracy.com.sg")

                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT id,name,email,Contact_No,Location,Resume,skills_count,Total_Score,Folder, Designation,id FROM Monstar2 where Designation LIKE '" + skills + "' AND Location like '%india%' ", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                conn.Open();
                                using (SqlDataReader sdr = cmd.ExecuteReader())
                                {

                                    while (sdr.Read())
                                    {
                                        customers.Add(new Monster
                                        {

                                            id = (int)sdr["id"],
                                            Name = sdr["Name"].ToString(),
                                            Designation = sdr["Designation"].ToString(),
                                            Email = sdr["Email"].ToString(),
                                            Contact_No = sdr["Contact_No"].ToString(),
                                            Location = sdr["Location"].ToString(),
                                            Resume = sdr["Resume"].ToString(),
                                            skills_count = sdr["skills_count"].ToString(),
                                            Total_Score = sdr["Total_Score"].ToString(),
                                            Experiance = sdr["Folder"].ToString(),

                                        });
                                    }
                                    ViewBag.profiles1 = customers.ToPagedList(i ?? 1, 10);
                                }
                            }
                        }
                    }
                    else if (User.Username == "anshikagupta@accuracy.com.sg" || User.Username == "baba@accuracy.com.sg" || User.Username == "pallavi@accuracy.com.sg" || User.Username == "Shreyag@accuracy.com.sg" || User.Username == "tasnim@accuracy.com.sg"||User.Username== "pallavi123@accuracy.com.sg")

                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT id,name,email,Contact_No,Location,Resume,skills_count,Total_Score, Designation,Folder,id FROM Monstar2 where Designation LIKE '" + skills + "' AND Location like '%india%' ", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                conn.Open();
                                using (SqlDataReader sdr = cmd.ExecuteReader())
                                {

                                    while (sdr.Read())
                                    {
                                        customers.Add(new Monster
                                        {

                                            id = (int)sdr["id"],
                                            Name = sdr["Name"].ToString(),
                                            Designation = sdr["Designation"].ToString(),
                                            Email = sdr["Email"].ToString(),
                                            Contact_No = sdr["Contact_No"].ToString(),
                                            Location = sdr["Location"].ToString(),
                                            Resume = sdr["Resume"].ToString(),
                                            skills_count = sdr["skills_count"].ToString(),
                                            Total_Score = sdr["Total_Score"].ToString(),
                                            Experiance = sdr["Folder"].ToString(),

                                        });
                                    }
                                    ViewBag.profiles1 = customers.ToPagedList(i ?? 1, 10);
                                }
                            }
                        }
                    }
                    else if (User.Username == "vani123@accuracy.com.sg" || User.Username == "ganeshr@joblisting.com" || User.Username == "deepti@accuracy.com.sg")

                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT id,name,email,Contact_No,Location,Resume,skills_count,Total_Score, Designation,Folder,id FROM Monstar2 where Designation LIKE '" + skills + "'", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                conn.Open();
                                using (SqlDataReader sdr = cmd.ExecuteReader())
                                {

                                    while (sdr.Read())
                                    {
                                        customers.Add(new Monster
                                        {

                                            id = (int)sdr["id"],
                                            Name = sdr["Name"].ToString(),
                                            Designation = sdr["Designation"].ToString(),
                                            Email = sdr["Email"].ToString(),
                                            Contact_No = sdr["Contact_No"].ToString(),
                                            Location = sdr["Location"].ToString(),
                                            Resume = sdr["Resume"].ToString(),
                                            skills_count = sdr["skills_count"].ToString(),
                                            Total_Score = sdr["Total_Score"].ToString(),
                                            Experiance = sdr["Folder"].ToString(),

                                        });
                                    }
                                    ViewBag.profiles1 = customers.ToPagedList(i ?? 1, 10);
                                }
                            }
                        }
                    }

                }
            }

            

            return View();
        }

        [UrlPrivilegeFilter]
        public ActionResult Shinelist(int? i, string skil = null, string ig = null)
        {
            string skil1 = skil;
            string re = skil1.Remove(3);
            string skills = "" + re + "%";
            // string skill = @skil;
            ViewBag.profiles1 = null;
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            List<Monster> customers = new List<Monster>();

            if (ig != null)
            {
                int id = Convert.ToInt32(ig);

                string countryName = string.Empty;
                List country = SharedService.Instance.GetCountry(id);
                if (country != null)
                {
                    countryName = country.Text;
                }
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    if (User.Username == "sandhya@accuracy.com.sg" || User.Username == "lakshmip@accuracy.com.sg" || User.Username == "denise@accuracy.com.sg" || User.Username == "dianna@accuracy.com.sg")
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT id,name,email,Contact_No,Prf_Location,Location,Resume, Designation,skills_count,Total_Score,Folder,id FROM Shine where Designation LIKE '" + skills + "' AND Prf_Location LIKE '" + countryName + "%' ", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                conn.Open();
                                using (SqlDataReader sdr = cmd.ExecuteReader())
                                {

                                    while (sdr.Read())
                                    {
                                        customers.Add(new Monster
                                        {

                                            id = (int)sdr["id"],
                                            Name = sdr["Name"].ToString(),
                                            Designation = sdr["Designation"].ToString(),
                                            Email = sdr["Email"].ToString(),
                                            Contact_No = sdr["Contact_No"].ToString(),
                                            Location = sdr["Prf_Location"].ToString(),
                                            Resume = sdr["Resume"].ToString(),
                                            Total_Score = sdr["Total_Score"].ToString(),
                                            skills_count = sdr["skills_count"].ToString(),
                                            Experiance = sdr["Folder"].ToString(),

                                        });
                                    }
                                    ViewBag.profiles1 = customers.ToPagedList(i ?? 1, 10);
                                }
                            }
                        }
                    }
                    else if (User.Username == "gowthami@accuracy.com.sg" || User.Username == "haris@joblisting.com" || User.Username == "anilkumar@joblisting.com" || User.Username == "naveena@accuracy.com.sg" || User.Username == "sarika123@accuracy.com.sg" || User.Username == "vanshika@accuracy.com.sg" || User.Username == "Doli.chauhan123@accuracy.com.sg" || User.Username == "doli.chauhan123@accuracy.com.sg")

                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT id,name,email,Contact_No,Location,Resume,skills_count,Total_Score, Designation,Folder,id FROM Shine where Designation LIKE '" + skills + "'", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                conn.Open();
                                using (SqlDataReader sdr = cmd.ExecuteReader())
                                {

                                    while (sdr.Read())
                                    {
                                        customers.Add(new Monster
                                        {

                                            id = (int)sdr["id"],
                                            Name = sdr["Name"].ToString(),
                                            Designation = sdr["Designation"].ToString(),
                                            Email = sdr["Email"].ToString(),
                                            Contact_No = sdr["Contact_No"].ToString(),
                                            Location = sdr["Location"].ToString(),
                                            Resume = sdr["Resume"].ToString(),
                                            skills_count = sdr["skills_count"].ToString(),
                                            Total_Score = sdr["Total_Score"].ToString(),
                                            Experiance = sdr["Folder"].ToString(),

                                        });
                                    }
                                    ViewBag.profiles1 = customers.ToPagedList(i ?? 1, 10);
                                }
                            }
                        }
                    }
                    else if (User.Username == "anshikagupta@accuracy.com.sg" || User.Username == "baba@accuracy.com.sg" || User.Username == "pallavi@accuracy.com.sg" || User.Username == "Shreyag@accuracy.com.sg" || User.Username == "tasnim@accuracy.com.sg" || User.Username == "shreyag1234@accuracy.com.sg" || User.Username == "pallavi123@accuracy.com.sg")

                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT id,name,email,Contact_No,Location,Resume,skills_count,Total_Score,skills_count, Designation,Folder,id FROM Shine where Designation LIKE '" + skills + "' AND Location like '%india%' ", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                conn.Open();
                                using (SqlDataReader sdr = cmd.ExecuteReader())
                                {

                                    while (sdr.Read())
                                    {
                                        customers.Add(new Monster
                                        {

                                            id = (int)sdr["id"],
                                            Name = sdr["Name"].ToString(),
                                            Designation = sdr["Designation"].ToString(),
                                            Email = sdr["Email"].ToString(),
                                            Contact_No = sdr["Contact_No"].ToString(),
                                            Location = sdr["Location"].ToString(),
                                            Resume = sdr["Resume"].ToString(),
                                            skills_count = sdr["skills_count"].ToString(),
                                            Total_Score = sdr["Total_Score"].ToString(),
                                            Experiance = sdr["Folder"].ToString(),

                                        });
                                    }
                                    ViewBag.profiles1 = customers.ToPagedList(i ?? 1, 10);
                                }
                            }
                        }
                    }
                    else if (User.Username == "vani123@accuracy.com.sg" || User.Username == "ganeshr@joblisting.com" || User.Username == "deepti@accuracy.com.sg")

                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT id,name,email,Contact_No,Location,Resume,skills_count,Total_Score, Designation,Folder,id FROM Shine where Designation LIKE '" + skills + "' ", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                conn.Open();
                                using (SqlDataReader sdr = cmd.ExecuteReader())
                                {

                                    while (sdr.Read())
                                    {
                                        customers.Add(new Monster
                                        {

                                            id = (int)sdr["id"],
                                            Name = sdr["Name"].ToString(),
                                            Designation = sdr["Designation"].ToString(),
                                            Email = sdr["Email"].ToString(),
                                            Contact_No = sdr["Contact_No"].ToString(),
                                            Location = sdr["Location"].ToString(),
                                            Resume = sdr["Resume"].ToString(),
                                            skills_count = sdr["skills_count"].ToString(),
                                            Total_Score = sdr["Total_Score"].ToString(),
                                            Experiance = sdr["Folder"].ToString(),

                                        });
                                    }
                                    ViewBag.profiles1 = customers.ToPagedList(i ?? 1, 10);
                                }
                            }
                        }
                    }

                }
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    if (User.Username == "sandhya@accuracy.com.sg" || User.Username == "lakshmip@accuracy.com.sg" || User.Username == "denise@accuracy.com.sg" || User.Username == "dianna@accuracy.com.sg")
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT id,name,email,Contact_No,Prf_Location,Location,Resume,skills_count,Total_Score, Designation,Folder,id FROM Shine where Designation LIKE '" + skills + "' AND Prf_Location LIKE '%Singapore%' ", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                conn.Open();
                                using (SqlDataReader sdr = cmd.ExecuteReader())
                                {

                                    while (sdr.Read())
                                    {
                                        customers.Add(new Monster
                                        {

                                            id = (int)sdr["id"],
                                            Name = sdr["Name"].ToString(),
                                            Designation = sdr["Designation"].ToString(),
                                            Email = sdr["Email"].ToString(),
                                            Contact_No = sdr["Contact_No"].ToString(),
                                            Location = sdr["Prf_Location"].ToString(),
                                            Resume = sdr["Resume"].ToString(),
                                            Total_Score = sdr["Total_Score"].ToString(),
                                            skills_count = sdr["skills_count"].ToString(),
                                            Experiance = sdr["Folder"].ToString(),

                                        });
                                    }
                                    ViewBag.profiles1 = customers.ToPagedList(i ?? 1, 10);
                                }
                            }
                        }
                    }
                    else if (User.Username == "gowthami@accuracy.com.sg" || User.Username == "haris@joblisting.com" || User.Username == "anilkumar@joblisting.com" || User.Username == "naveena@accuracy.com.sg" || User.Username == "sarika123@accuracy.com.sg" || User.Username == "vanshika@accuracy.com.sg" || User.Username == "Doli.chauhan123@accuracy.com.sg")

                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT id,name,email,Contact_No,Location,Resume,skills_count,Total_Score,Folder, Designation,id FROM Shine where Designation LIKE '" + skills + "' AND Location like '%india%' ", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                conn.Open();
                                using (SqlDataReader sdr = cmd.ExecuteReader())
                                {

                                    while (sdr.Read())
                                    {
                                        customers.Add(new Monster
                                        {

                                            id = (int)sdr["id"],
                                            Name = sdr["Name"].ToString(),
                                            Designation = sdr["Designation"].ToString(),
                                            Email = sdr["Email"].ToString(),
                                            Contact_No = sdr["Contact_No"].ToString(),
                                            Location = sdr["Location"].ToString(),
                                            Resume = sdr["Resume"].ToString(),
                                            skills_count = sdr["skills_count"].ToString(),
                                            Total_Score = sdr["Total_Score"].ToString(),
                                            Experiance = sdr["Folder"].ToString(),

                                        });
                                    }
                                    ViewBag.profiles1 = customers.ToPagedList(i ?? 1, 10);
                                }
                            }
                        }
                    }
                    else if (User.Username == "anshikagupta@accuracy.com.sg" || User.Username == "baba@accuracy.com.sg" || User.Username == "pallavi@accuracy.com.sg" || User.Username == "Shreyag@accuracy.com.sg" || User.Username == "tasnim@accuracy.com.sg" || User.Username == "pallavi123@accuracy.com.sg")

                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT id,name,email,Contact_No,Location,Resume,skills_count,Total_Score, Designation,Folder,id FROM Shine where Designation LIKE '" + skills + "' AND Location like '%india%' ", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                conn.Open();
                                using (SqlDataReader sdr = cmd.ExecuteReader())
                                {

                                    while (sdr.Read())
                                    {
                                        customers.Add(new Monster
                                        {

                                            id = (int)sdr["id"],
                                            Name = sdr["Name"].ToString(),
                                            Designation = sdr["Designation"].ToString(),
                                            Email = sdr["Email"].ToString(),
                                            Contact_No = sdr["Contact_No"].ToString(),
                                            Location = sdr["Location"].ToString(),
                                            Resume = sdr["Resume"].ToString(),
                                            skills_count = sdr["skills_count"].ToString(),
                                            Total_Score = sdr["Total_Score"].ToString(),
                                            Experiance = sdr["Folder"].ToString(),

                                        });
                                    }
                                    ViewBag.profiles1 = customers.ToPagedList(i ?? 1, 10);
                                }
                            }
                        }
                    }
                    else if (User.Username == "vani123@accuracy.com.sg" || User.Username == "ganeshr@joblisting.com" || User.Username == "deepti@accuracy.com.sg")

                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT id,name,email,Contact_No,Location,Resume,skills_count,Total_Score, Designation,Folder,id FROM Shine where Designation LIKE '" + skills + "'", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                conn.Open();
                                using (SqlDataReader sdr = cmd.ExecuteReader())
                                {

                                    while (sdr.Read())
                                    {
                                        customers.Add(new Monster
                                        {

                                            id = (int)sdr["id"],
                                            Name = sdr["Name"].ToString(),
                                            Designation = sdr["Designation"].ToString(),
                                            Email = sdr["Email"].ToString(),
                                            Contact_No = sdr["Contact_No"].ToString(),
                                            Location = sdr["Location"].ToString(),
                                            Resume = sdr["Resume"].ToString(),
                                            skills_count = sdr["skills_count"].ToString(),
                                            Total_Score = sdr["Total_Score"].ToString(),
                                            Experiance = sdr["Folder"].ToString(),

                                        });
                                    }
                                    ViewBag.profiles1 = customers.ToPagedList(i ?? 1, 10);
                                }
                            }
                        }
                    }

                }
            }



            return View();
        }

        public ActionResult ProfilesList(long id)
        {
           // long ids = 10801;
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from  jobs where [id] LIKE '" + id + "%'", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            TempData["cb"] = dt.Rows[0][4];
                            string text = (string)TempData["cb"];
                            Session["cb"] = text;
                            ViewBag.ss = text;
                        }
                    }
                }
                using (SqlCommand cmd = new SqlCommand("select userid from  user_skills where [skillname] LIKE '%" + Session["cb"] + "%'", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            TempData["ids"] = dt;
                        }
                    }
                }
            }

            return View();
        }

        //public ActionResult SmsList(string mob,string title)
        //{
        //    ResponseContext context4 = new ResponseContext();
        //    string[] m = title.ToLower().Split('-');
        //    //string m = jurl;
        //    long x1 = Convert.ToInt32(m.Last());
        //    Regex reg = null;
        //    reg = new Regex("^[1-9][0-9]*$");
        //    string message = string.Empty;
        //    if (!string.IsNullOrEmpty(mob) && reg.IsMatch(mob))
        //    {
        //        string AccountSid = ConfigService.Instance.GetConfigValue("TwilioSID");
        //        string AuthToken = ConfigService.Instance.GetConfigValue("TwilioToken");
        //        string from = ConfigService.Instance.GetConfigValue("TwilioNumber");
        //        var twilio = new TwilioRestClient(AccountSid, AuthToken);

        //        StringBuilder sbSMS = new StringBuilder();
        //        sbSMS.AppendFormat("{0} invites you to connect at Joblisting and Apply for the Job\n", User.Info.FullName);
        //        sbSMS.AppendFormat("Download Android App Now  https://play.google.com/store/apps/details?id=com.accuracy.joblisting");
        //        sbSMS.AppendFormat(" Download IOS App Now  https://apps.apple.com/in/app/job-listing/id1575724994/");
        //        Guid token = Guid.NewGuid();
        //        var url = string.Format("{0}/connectbyphone?token={1}", Request.Url.GetLeftPart(UriPartial.Authority), token);


        //        string to = string.Format("{0}{1}", "+91", mob);
        //        string url5 = "";
        //        using (JobPortalEntities context = new JobPortalEntities())
        //        {
        //            DataHelper dataHelper = new DataHelper(context);
        //            string[] jo = title.ToLower().Split('-');
        //            List<Job> listjob = MemberService.Instance.InviteJob(Convert.ToInt32(jo.Last()));
        //            //var job = dataHelper.GetSingle<Job>(Convert.ToInt32(jurl));
        //            if (listjob.Count >= 0)
        //            {
        //                url5 = string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, listjob[0].PermaLink,
        //                                          listjob[0].Id);

        //                sbSMS.AppendFormat("CLICK HERE TO APPLY {0}", url5);
        //                var sms = twilio.SendMessage(from, to, sbSMS.ToString());
        //                if (sms.RestException == null)
        //                {
        //                    var msg = twilio.GetMessage(sms.Sid);
        //                    if (msg.Status.Equals("delivered") || msg.Status.Equals("sent"))
        //                    {
        //                        List<MemberService.Invit> listc = MemberService.Instance.InviteSC(User.Id, to);
        //                        if (listc.Count <= 0)
        //                        {
        //                            List<MemberService.Invit> list = MemberService.Instance.InviteS(sms.Sid, User.Id, to, token);
        //                            List<MemberService.sentE> list2 = MemberService.Instance.sentSI(User.Id, listjob[0].Id, to);
        //                        }
        //                        context4 = new ResponseContext()
        //                        {
        //                            Id = 0,
        //                            Type = "Failed",
        //                            Message = "Invitation sent successfully!"
        //                        };

        //                        TempData["SaveData"] = "Invitation sent successfully!";
        //                    }
        //                }
        //                else
        //                {
        //                    if (sms.RestException.Code.Equals("14101") || sms.RestException.Code.Equals("21211"))
        //                    {
        //                        context4 = new ResponseContext()
        //                        {
        //                            Id = 0,
        //                            Type = "Failed",
        //                            Message = "Please provide valid mobile number!"
        //                        };
        //                        TempData["Error"] = "Please provide valid mobile number!";
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        TempData["Error"] = "Please provide valid mobile number!";
        //        context4 = new ResponseContext()
        //        {
        //            Id = 0,
        //            Type = "Failed",
        //            Message = "Please provide valid mobile number!"
        //        };

        //        //return Json(context4, JsonRequestBehavior.AllowGet);


        //    }

        //    return RedirectToAction("jobsbycompanyall", "admin");
        //}
        public void sendSmsBUllk(string country, string mobile, string jurl,string name)

        {
            ResponseContext context4 = new ResponseContext();
            string[] m = jurl.ToLower().Split('-');
            //string m = jurl;
            long x1 = Convert.ToInt32(m.Last());
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
                //sbSMS.AppendFormat("{0} invites you to connect at Joblisting and Apply for the Job\n", User.Info.FullName);
                //sbSMS.AppendFormat("Download Android App Now  https://play.google.com/store/apps/details?id=com.accuracy.joblisting");
                //sbSMS.AppendFormat( " Download IOS App Now  https://apps.apple.com/in/app/job-listing/id1575724994/");
                sbSMS.AppendFormat("Your Profile matches to {0} job. Register today at JobListing.com for applying for a job.  \n", jurl);
                sbSMS.AppendFormat("<a href='https://play.google.com/store/apps/details?id=com.accuracy.joblisting'>Download Android</a>");
                sbSMS.AppendFormat("<a href='https://apps.apple.com/in/app/job-listing/id1575724994/'>Download IOS</a> ");
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

                        //sbSMS.AppendFormat("CLICK HERE TO APPLY {0}", url5);
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
        }

      //  [AllowAnonymous]
      //public ActionResult SMSProfile()
      //  {
      //      string nu = "8651214771";
      //      var join = job.PermaLink + "-" + job.Id;
      //      //sendSmsBUllk("+91", "9380928867", join);
      //      sendSmsBUllk(profile11.MobileCountryCode, profile11.Mobile, join);
      //      TempData["UpdateData"] = "Job sharing link sent successfully via SMS!";
      //      TempData["UpdateData"] = "Job sharing link sent successfully via SMS!";
      //      return RedirectToAction("jobsbycompanyall", "admin");
      //  }
        [AllowAnonymous]
        public ActionResult SMSFORALL(long Id)
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
                        UserInfoEntity uinfo = _service.Get(User.Id);
                        UserProfile employer = new UserProfile();
                        var d = job.Title; //"java"; //job.Title;

                        string skil1 = d;
                        string re = skil1.Remove(3);
                        string dil = "" + re + "%";
                        List<UserInfoEntity> profile1 = new List<UserInfoEntity>();
                        List<User_Skills> profile_skills = MemberService.Instance.GetUserskillProfiles(dil);


                        foreach (var item in profile_skills)
                        {
                            var profile11 = MemberService.Instance.GetUserInfo(item.UserId);
                            //if (profile11.Mobile != null)
                            
                                string nu = "8651214771";
                                var join = job.PermaLink + "-" + job.Id;
                                //sendSmsBUllk("+91", "9380928867", join);
                                sendSmsBUllk(profile11.MobileCountryCode, profile11.Mobile, join,profile11.FirstName);
                                TempData["UpdateData"] = "Job sharing link sent successfully via SMS!";

                           

                            // list = item.UserId;
                            profile1.Add(profile11);


                        }
                        TempData["UpdateData"] = "Job sharing link sent successfully via SMS!";
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
        [HttpGet]
        [Authorize(Roles = "Administrator, SuperUser")]
        public ActionResult MonsterPage()
        {
            if (string.IsNullOrEmpty(UserInfo.Address) && string.IsNullOrEmpty(UserInfo.Mobile))
            {
                return RedirectToAction("ListJob1Error", "Admin", new { returnUrl = "/listjob1" });
            }

            JobListingModel model = new JobListingModel();
            if (!string.IsNullOrEmpty(Request.QueryString["sid"]))
            {
                Guid id = new Guid(Request.QueryString["sid"]);
                string jobData = DomainService.Instance.ReadData(id);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                JobPostModel jpe = serializer.Deserialize<JobPostModel>(jobData);
                ListJob1C jpe1 = serializer.Deserialize<ListJob1C>(jobData);
                model.CompanyName = jpe1.CompanyName;
                model.AboutCompany = jpe1.AboutCompany;
                model.job_id = Convert.ToInt16(jpe1.job_id);
                model.Title = jpe.Title;
                model.CategoryId = jpe.CategoryId;
                //model.SpecializationId = jpe.SpecializationId;
                model.CountryId = jpe.CountryId;
                model.StateId = jpe.StateId;
                model.City = jpe.City;
                model.Zip = jpe.Zip;
                model.Description = jpe.Description;
                model.Summary = jpe.Summary;
                //model.Requirements = jpe.Requirements;
                //model.Responsibilities = jpe.Responsibilities;
                model.MinimumExperience = (byte?)jpe.MinimumExperience;
                model.MaximumExperience = (byte?)jpe.MaximumExperience;
                model.SalaryCurrency = jpe.Currency;
                model.MinimumSalary = jpe.MinimumSalary;
                model.MaximumSalary = jpe.MaximumSalary;
                model.MinimumAge = (int?)jpe.MinimumAge;
                model.MaximumAge = (int?)jpe.MaximumAge;
                model.EmploymentType = (long)jpe.EmployementTypeId;
                model.QualificationId = (long?)jpe.Qualification;
                model.Distribute = 1;
                DomainService.Instance.RemoveData(id);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult MonsterPage(JobListingModel model)
        {

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            if (User != null)
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    var employer = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                    using (SqlConnection conn = new SqlConnection(constr))
                    {

                        if (employer.Username == "adminCom@joblisting.com" || employer.Username == "RecruiterCom@joblisting.com" || employer.Username == "recruiter@joblisting.com" || employer.Username == "vani123@accuracy.com.sg" || employer.Username == "tasnim@accuracy.com.sg" || employer.Username == "ganeshr@joblisting.com"  || employer.Username == "deepti@accuracy.com.sg" || employer.Username == "doli.chauhan123@accuracy.com.sg" || employer.Username == "sandhya@accuracy.com.sg" || employer.Username == "shreyag1234@accuracy.com.sg" || employer.Username == "pallavi123@accuracy.com.sg" || employer.Username == "druthi@accuracy.com.sg" || employer.Username == "baba@accuracy.com.sg" || employer.Username == "anshikagupta@accuracy.com.sg" || employer.Username == "dianna@accuracy.com.sg" || employer.Username == "haris@joblisting.com" || employer.Username == "anilkumar@joblisting.com" || employer.Username == "denise@accuracy.com.sg" || employer.Username == "lakshmip@accuracy.com.sg" || employer.Username == "vanshika@accuracy.com.sg" || employer.Username == "sarika123@accuracy.com.sg" || employer.Username == "naveena@accuracy.com.sg" || employer.Username == "gowthami@accuracy.com.sg" || User.Username == "ganeshr@joblisting.com")
                        {
                            TempData["countryid"] = 40;
                        }
                        DateTime dateTime = DateTime.Now;
                        DateTime ClosingDate = dateTime.AddDays(+30);
                        int CountryId = (int)TempData["countryid"];
                        conn.Open();
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = string.Format("insert into  jobs (EmployerId,Title,Description,Summary,PublishedDate,ClosingDate,DateCreated,CreatedBy,CountryId,StateId,City,Zip,MinimumExperience,MaximumExperience,Currency,MinimumSalary,MaximumSalary,CategoryId,EmploymentTypeId,QualificationId,NoticePeriod,OptionalSkills) Values('" + employer.UserId + "', '" + model.Title + "', '" + model.Description + "', '" + model.Summary + "','" + dateTime + "','" + ClosingDate + "','" + dateTime + "','" + employer.Username + "','" + CountryId + "','" + model.StateId + "','" + model.City + "','" + model.Zip + "','" + model.MinimumExperience + "','" + model.MaximumExperience + "','" + model.SalaryCurrency + "','" + model.MinimumSalary + "','" + model.MaximumSalary + "','" + model.CategoryId + "','" + model.EmploymentType + "','" + model.QualificationId + "','" + model.Noticeperiod + "','" + model.Optionalskills + "')", conn);

                            command.ExecuteNonQuery();
                            TempData["UpdateData"] = model.Title + " job has been submitted successfully.";
                        }
                        conn.Close();

                    }
                }
                //using (JobPortalEntities context = new JobPortalEntities())
                //{
                //    DataHelper dataHelper = new DataHelper(context);

                //    var employer = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                //    var permalink = model.Title;

                //    permalink =
                //        permalink.Replace('.', ' ')
                //            .Replace(',', ' ')
                //            .Replace('-', ' ')
                //            .Replace(" - ", " ")
                //            .Replace(" , ", " ")
                //            .Replace('/', ' ')
                //            .Replace(" / ", " ")
                //            .Replace(" & ", " ")
                //            .Replace("&", " ");
                //    var pattern = "\\s+";
                //    var replacement = " ";
                //    permalink = Regex.Replace(permalink, pattern, replacement).Trim().ToLower();
                //    permalink = permalink.Replace(' ', '-');

                //    var job = new Job();
                //    var job1 = new ListJob1C();
                //    job1.CompanyName = model.CompanyName;
                //    job1.AboutCompany = model.AboutCompany;
                //    job.Title = model.Title;
                //    job.Distribute = Convert.ToBoolean(model.Distribute);
                //    string description = Sanitizer.GetSafeHtmlFragment(model.Description);
                //    description = description.RemoveEmails();
                //    description = description.RemoveNumbers();
                //    job.Description = description.RemoveWebsites();

                //    string summary = model.Summary;
                //    summary = summary.RemoveEmails();
                //    summary = summary.RemoveNumbers();
                //    summary = summary.RemoveWebsites();
                //    job.Summary = summary;

                //    //string requirements = model.Requirements;
                //    //requirements = requirements.RemoveEmails();
                //    //requirements = requirements.RemoveNumbers();
                //    //requirements = requirements.RemoveWebsites();
                //    //job.Requirements = requirements;
                //    //string roles = model.Responsibilities;
                //    //roles = roles.RemoveEmails();
                //    //roles = roles.RemoveNumbers();
                //    //roles = roles.RemoveWebsites();
                //    //job.Responsilibies = roles;

                //    job.IsFeaturedJob = model.IsFeaturedJob;
                //    job.CategoryId = model.CategoryId;
                //    //job.SpecializationId = model.SpecializationId;
                //    if (employer.Username == "adminCom@joblisting.com" || employer.Username == "RecruiterCom@joblisting.com")
                //    {
                //        job.CountryId = 40;

                //    }
                //    else
                //    {
                //        job.CountryId = model.CountryId;
                //    }
                //    //job.CountryId = model.CountryId;
                //    job.DateOfBirth = model.DateOfBirth1;
                //    job.AdharNumber = model.Adharnumber;
                //    job.OptionalSkills = model.Optionalskills;
                //    job.NoticePeriod = model.Noticeperiod;
                //    job.StateId = model.StateId;
                //    job.City = model.City;
                //    job.Zip = model.Zip;
                //    job.EmploymentTypeId = model.EmploymentType;
                //    job.QualificationId = model.QualificationId;
                //    job.MinimumAge = (byte?)model.MinimumAge;
                //    job.MaximumAge = (byte?)model.MaximumAge;
                //    job.MinimumExperience = (byte)model.MinimumExperience;
                //    job.MaximumExperience = (byte)model.MaximumExperience;
                //    job.MinimumSalary = model.MinimumSalary;
                //    job.MaximumSalary = model.MaximumSalary;
                //    job.Currency = model.SalaryCurrency;
                //    job.PublishedDate = DateTime.Now;
                //    job.ClosingDate = DateTime.Now.AddMonths(1);
                //    job.PermaLink = permalink;
                //    job.EmployerId = employer.UserId;
                //    job.IsActive = true;
                //    job.IsDeleted = false;
                //    job.IsPostedOnTwitter = false;
                //    job.InEditMode = false;
                //    job.Distribute = (model.Distribute == 1);
                //    job.IsPaid = job.IsPaid;
                //    var job_id = Convert.ToInt64(dataHelper.Add<Job>(job, User.Username));


                //    //string fileName = Path.GetFileName(model.CompanyLogos.FileName);
                //    //string imgBase64String = Convert.ToString(model.CompanyLogos);




                //    //using (SqlConnection conn = new SqlConnection(constr))
                //    //{
                //    //    //try
                //    //    //{
                //    //        conn.Open();
                //    //        using (SqlCommand command = conn.CreateCommand())
                //    //        {
                //    //            command.CommandType = CommandType.Text;
                //    //            command.CommandText = string.Format("insert into  ListJob1C" +
                //    //                "(id,CompanyName,AboutCompany,CompanyLogos,job_id) " +
                //    //                "values('{0}','{1}','{2}','{3}','{4}')",
                //    //              model.Id, model.CompanyName, model.AboutCompany, "dhjvdvfdgvfydgfjdgkfdjvdgkyufdjfyudgkfg", job_id);
                //    //            command.ExecuteNonQuery();
                //    //        }

                //    //        conn.Close();


                //    //    //}
                //    //    //catch (Exception ex)
                //    //    //{
                //    //    //    conn.Close();
                //    //    //    SendEx(ex);
                //    //    //}
                //    //}


                //    if (job_id > 0)
                //    {
                //        var tracking = new Tracking
                //        {
                //            Id = Guid.NewGuid(),
                //            JobId = job.Id,
                //            UserId = employer.UserId,
                //            Type = (int)TrackingTypes.PUBLISHED,
                //            DateUpdated = DateTime.Now,
                //            IsDownloaded = false
                //        };

                //        dataHelper.Add<Tracking>(tracking);

                //        var reader = new StreamReader(Server.MapPath("~/Templates/Mail/employer_postjob.html"));
                //        var body = string.Empty;

                //        if (reader != null)
                //        {
                //            body = reader.ReadToEnd();
                //            body = body.Replace("@@employer", employer.Company);
                //            body = body.Replace("@@jobtitle", job.Title);
                //            if (job.IsFeaturedJob.Value)
                //            {
                //                body = body.Replace("@@featured",
                //                    "This is featured job which will appear at main page as well as on top of search results!");
                //            }
                //            else
                //            {
                //                body = body.Replace("@@featured", "");
                //            }
                //            body = body.Replace("@@joburl",
                //                string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job_id));

                //            string[] receipent = { employer.Username };
                //            var subject = string.Format("Thanks for Posting {0} Job", job.Title);

                //            var recipients = new List<Recipient>();
                //            recipients.Add(new Recipient
                //            {
                //                Email = employer.Username,
                //                DisplayName = string.Format("{0} {1}", employer.FirstName, employer.LastName),
                //                Type = RecipientTypes.TO
                //            });

                //            List<int> typeList = new List<int>() { (int)SecurityRoles.Administrator, (int)SecurityRoles.SuperUser };
                //            var profileList = dataHelper.Get<UserProfile>().Where(x => typeList.Contains(x.Type)).ToList();
                //            foreach (var profile in profileList)
                //            {
                //                recipients.Add(new Recipient
                //                {
                //                    Email = profile.Username,
                //                    DisplayName = string.Format("{0} {1}", profile.FirstName, profile.LastName),
                //                    Type = RecipientTypes.BCC
                //                });
                //            }
                //            AlertService.Instance.SendMail(subject, recipients, body);
                //        }
                //        TempData["UpdateData"] = string.Format("{0} job has been submitted successfully. It is in approval process, we will inform you once it is approved!", job.Title);
                //    }
                //    //if (DomainService.Instance.PaymentProcessEnabled())
                //    //    {
                //    //        if (Request.QueryString["le"] != null && Request.QueryString["le"] == "1")
                //    //        {
                //    //            return RedirectToAction("Select", "Package", new { returnUrl = "/Employer/ListJobError?returnUrl=/listjob", RedirectUrl = "/employer/index", type = "J", sessionId = id, countryId = model.CountryId });
                //    //        }
                //    //        else
                //    //        {
                //    //            return RedirectToAction("Select", "Package", new { returnUrl = "/Employer/Index", RedirectUrl = "/employer/index", type = "J", sessionId = id, countryId = model.CountryId });
                //    //        }
                //    //    }

                //}
                // return RedirectToAction("Jobsbycompanyall");
            }
            // return View(new JobListingModel());
            return RedirectToAction("Jobsbycompanyall");
        }
        public ActionResult JobsByCompanyAll(AdminJobSearchModel model, int pageNumber = 0)
        {
            List country = null;
            UserInfoEntity uinfo = _service.Get(User.Id);
            if (uinfo.Username == "tasnim@joblisting.com" || uinfo.Username == "lakshmii@joblisting.com")
            {
                model.CountryId = 106;
            }
            else if (uinfo.Username == "maitrantn@joblisting.com" || uinfo.Username == "Linhv@joblisting.com")
            {
                model.CountryId = 3343;
            }
            else if (uinfo.Username == "sandhyam@joblisting.com" || uinfo.Username == "Tanyam@joblisting.com")
            {
                model.CountryId = 139;
            }
            else if (uinfo.Username == "sandhyas@joblisting.com" || uinfo.Username == "balus@joblisting.com")
            {
                model.CountryId = 205;
            }
            else if (uinfo.Username == "adminIndo@joblisting.com" || uinfo.Username == "RecruiterIndo@joblisting.com")
            {
                model.CountryId = 107;
            }
            else if (uinfo.Username == "adminPhil@joblisting.com" || uinfo.Username == "RecruiterPhil@joblisting.com")
            {
                model.CountryId = 180;
            }

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
            {
                model.CountryId = 40;

            }

            if (model.CountryId != null && model.CountryId != 0)
            {
                country = SharedService.Instance.GetCountry(model.CountryId.Value);
            }
            Session["CountryId"] = model.CountryId;
            model.Id = uinfo.Id;
            Session["EmpId"] = model.Id;

            UserProfile employer = new UserProfile();

            if (model.Id != null)
            {
                employer = MemberService.Instance.Get(model.Id.Value);
            }

            List<Job> jobs = new List<Job>();
            int rows = 0;
            int pageSize = 10;
            using (JobPortalEntities context = new JobPortalEntities())
            {

                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<Job>();

                if (model.CountryId != null)
                {
                    if (uinfo.Username == "tasnim@accuracy.com.sg")
                    {
                        result = result.Where(x => x.EmployerId == 180991 || x.EmployerId == 182835 || x.EmployerId == 182830 || x.EmployerId == 182831 || x.EmployerId == 182833|| x.EmployerId == 1411408|| x.EmployerId == 1411409);

                    }

                    else if (uinfo.Username == "deepti@accuracy.com.sg")
                    {
                        result = result.Where(x => x.EmployerId == 182835|| x.EmployerId == 1411413);

                    }
                    else if (uinfo.Username == "ganeshr@joblisting.com")
                    {
                        result = result.Where(x => x.EmployerId == 1411413);

                    }
                  
                    else if (uinfo.Username == "doli.chauhan123@accuracy.com.sg")
                    {
                        result = result.Where(x => x.EmployerId == 182823 || x.EmployerId == 182824 || x.EmployerId == 182842 || x.EmployerId == 182826 || x.EmployerId == 1411408|| x.EmployerId == 1411409);

                    }
                    else if (uinfo.Username == "sandhya@accuracy.com.sg")
                    {
                        result = result.Where(x => x.EmployerId == 182827 || x.EmployerId == 182828 || x.EmployerId == 182829);

                    }
                    else if (uinfo.Username == "vani123@accuracy.com.sg")
                    {
                        result = result.Where(x => x.EmployerId == 182835);

                    }
                    else if (uinfo.Username == "lakshmip@accuracy.com.sg")
                    {
                        result = result.Where(x => x.EmployerId == 182827);

                    }
                    else if (uinfo.Username == "denise@accuracy.com.sg")
                    {
                        result = result.Where(x => x.EmployerId == 182828);

                    }
                    else if (uinfo.Username == "anilkumar@joblisting.com")
                    {
                        result = result.Where(x => x.EmployerId == 1411409);

                    }
                    else if (uinfo.Username == "haris@joblisting.com")
                    {
                        result = result.Where(x => x.EmployerId == 1411408);

                    }
                    else if (uinfo.Username == "dianna@accuracy.com.sg")
                    {
                        result = result.Where(x => x.EmployerId == 182829);

                    }

                    else if (uinfo.Username == "gowthami@accuracy.com.sg")
                    {
                        result = result.Where(x => x.EmployerId == 182823);

                    }
                    else if (uinfo.Username == "naveena@accuracy.com.sg")
                    {
                        result = result.Where(x => x.EmployerId == 182824);

                    }
                    else if (uinfo.Username == "sarika123@accuracy.com.sg")
                    {
                        result = result.Where(x => x.EmployerId == 182842);

                    }
                    else if (uinfo.Username == "vanshika@accuracy.com.sg")
                    {
                        result = result.Where(x => x.EmployerId == 182826);

                    }


                    else if (uinfo.Username == "anshikagupta@accuracy.com.sg")
                    {
                        result = result.Where(x => x.EmployerId == 182830);

                    }
                    else if (uinfo.Username == "baba@accuracy.com.sg")
                    {
                        result = result.Where(x => x.EmployerId == 182831);

                    }
                    else if (uinfo.Username == "pallavi123@accuracy.com.sg")
                    {
                        result = result.Where(x => x.EmployerId == 182839);

                    }
                    else if (uinfo.Username == "Shreyag@accuracy.com.sg")
                    {
                        result = result.Where(x => x.EmployerId == 182833);

                    }
                    else if (uinfo.Username == "shreyag1234@accuracy.com.sg")
                    {
                        result = result.Where(x => x.EmployerId == 182841);

                    }
                    else
                    {
                        result = result.Where(x => x.CountryId == model.Id);
                    }
                }

                //if (model.Id != null)
                //{
                //    result = result.Where(x => x.EmployerId == model.Id.Value);
                //}

                if (model.TypeId != null)
                {
                    switch (model.TypeId.Value)
                    {
                        case 1:
                            result = result.Where(x => x.IsActive == true && x.IsPublished == true && x.IsDeleted == false && x.IsExpired.Value == false && x.IsRejected == false);
                            break;
                        case 2:
                            result = result.Where(x => x.IsExpired.Value == true && x.IsDeleted == false);
                            break;
                        case 3:
                            result = result.Where(x => x.IsDeleted == true && x.IsExpired.Value == false);
                            break;
                        case 4:
                            result = result.Where(x => x.IsPublished == false && x.IsRejected == false && x.IsDeleted == false);
                            break;
                        case 5:
                            result = result.Where(x => x.IsRejected == true && x.IsDeleted == false);
                            break;
                        case 6:
                            result = result.Where(x => x.IsActive == false && x.IsDeleted == false);
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(model.JobTitle))
                {
                    result = result.Where(x => x.CompanyName1.ToLower().Contains(model.JobTitle.ToLower().Trim()));
                }

                string sdate = string.Empty;
                string edate = string.Empty;
                var sdt = new DateTime?();
                var edt = new DateTime?();
                if ((!string.IsNullOrEmpty(model.fd) && !string.IsNullOrEmpty(model.fm) && !string.IsNullOrEmpty(model.fy)) || (!string.IsNullOrEmpty(model.td) && !string.IsNullOrEmpty(model.tm) && !string.IsNullOrEmpty(model.ty)))
                {
                    if (!string.IsNullOrEmpty(model.fd) && !string.IsNullOrEmpty(model.fm) && !string.IsNullOrEmpty(model.fy))
                    {
                        sdate = string.Format("{0}/{1}/{2}", model.fm, model.fd, model.fy);
                        sdt = Convert.ToDateTime(sdate);
                    }

                    if (!string.IsNullOrEmpty(model.td) && !string.IsNullOrEmpty(model.tm) && !string.IsNullOrEmpty(model.ty))
                    {
                        if (string.IsNullOrEmpty(model.fd) && string.IsNullOrEmpty(model.fm) && string.IsNullOrEmpty(model.fy))
                        {
                            sdt = DateTime.Now;
                        }
                        edate = string.Format("{0}/{1}/{2}", model.tm, model.td, model.ty);
                        edt = Convert.ToDateTime(edate);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.fd) && !string.IsNullOrEmpty(model.fm) && !string.IsNullOrEmpty(model.fy))
                        {
                            edt = DateTime.Now;
                        }
                    }
                }
                if (sdt != null)
                {
                    result = result.Where(x => x.PublishedDate.Day >= sdt.Value.Day && x.PublishedDate.Month >= sdt.Value.Month && x.PublishedDate.Year >= sdt.Value.Year);
                }

                if (edt != null)
                {
                    result = result.Where(x => x.PublishedDate.Day <= edt.Value.Day && x.PublishedDate.Month <= edt.Value.Month && x.PublishedDate.Year <= edt.Value.Year);
                }

                rows = result.Count();
                jobs = result.OrderByDescending(x => x.DateCreated).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize)).Take(pageSize).ToList();
            }

            var d = jobs.Select(f => f.Title).ToList();

            ViewBag.Message11 = d;


            //List list;
            //string dil = "%" + d + "%";
            //List<UserInfoEntity> profile1 = new List<UserInfoEntity>();
            //List<User_Skills> profile = MemberService.Instance.GetUserskillProfiles(dil);


            //foreach (var item in profile)
            //{
            //    var profile11 = MemberService.Instance.GetUserInfo(item.UserId);
            //    // list = item.UserId;
            //    profile1.Add(profile11);


            //}

            ViewBag.TypeId = model.TypeId;
            ViewBag.Country = country;
            ViewBag.Employer = employer;

            // ViewBag.UserType = uinfo.Type;
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<Job>(jobs, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);


            return View();
        }
        public ActionResult ListSkillProfiles()
        {
            TempData["new"] = "No Such Skill Profiles!..";
            //var profile = MemberService.Instance.GetUserskillProfiles(Skilles);
            UserInfoEntity uinfo = _service.Get(User.Id);
            UserProfile employer = new UserProfile();
            ViewBag.Employer = employer;
            return View();
        }


        [HttpPost]
            

        public JsonResult ListSkillProfiles(string ViewerData)
        {
            UserInfoEntity uinfo = _service.Get(User.Id);
            UserProfile employer = new UserProfile();
            ViewBag.Employer = employer;
            string d = (string)Session["item"];
            //List list;
            string dil = "%" + d + "%";
            List<UserInfoEntity> profile1 = new List<UserInfoEntity>();
            List<User_Skills> profile = MemberService.Instance.GetUserskillProfiles(dil);


            foreach (var item in profile)
            {
                var profile11 = MemberService.Instance.GetUserInfo(item.UserId);
                // list = item.UserId;
                profile1.Add(profile11);


            }
            return Json(profile1);


        }

        public ActionResult JobsByCompany(AdminJobSearchModel model, int pageNumber = 0)
        {
            List country = null;
            UserInfoEntity uinfo = _service.Get(User.Id);
            if (uinfo.Username == "tasnim@joblisting.com" || uinfo.Username == "lakshmii@joblisting.com")
            {
                model.CountryId = 106;
            }
            else if (uinfo.Username == "maitrantn@joblisting.com" || uinfo.Username == "Linhv@joblisting.com")
            {
                model.CountryId = 3343;
            }
            else if (uinfo.Username == "sandhyam@joblisting.com" || uinfo.Username == "Tanyam@joblisting.com")
            {
                model.CountryId = 139;
            }
            else if (uinfo.Username == "sandhyas@joblisting.com" || uinfo.Username == "balus@joblisting.com")
            {
                model.CountryId = 205;
            }
            else if (uinfo.Username == "adminIndo@joblisting.com" || uinfo.Username == "RecruiterIndo@joblisting.com")
            {
                model.CountryId = 107;
            }
            else if (uinfo.Username == "adminPhil@joblisting.com" || uinfo.Username == "RecruiterPhil@joblisting.com")
            {
                model.CountryId = 180;
            }

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || User.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
            {
                model.CountryId = 40;
            }

            if (model.CountryId != null && model.CountryId != 0)
            {
                country = SharedService.Instance.GetCountry(model.CountryId.Value);
            }

            UserProfile employer = new UserProfile();
            if (model.Id != null)
            {
                employer = MemberService.Instance.Get(model.Id.Value);
            }

            List<Job> jobs = new List<Job>();
            int rows = 0;
            int pageSize = 10;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<Job>();

                if (model.CountryId != null)
                {
                    result = result.Where(x => x.CountryId == model.CountryId);
                }

                if (model.Id != null)
                {
                    result = result.Where(x => x.EmployerId == model.Id.Value);
                }

                if (model.TypeId != null)
                {
                    switch (model.TypeId.Value)
                    {
                        case 1:
                            result = result.Where(x => x.IsActive == true && x.IsPublished == true && x.IsDeleted == false && x.IsExpired.Value == false && x.IsRejected == false);
                            break;
                        case 2:
                            result = result.Where(x => x.IsExpired.Value == true && x.IsDeleted == false);
                            break;
                        case 3:
                            result = result.Where(x => x.IsDeleted == true && x.IsExpired.Value == false);
                            break;
                        case 4:
                            result = result.Where(x => x.IsPublished == false && x.IsRejected == false && x.IsDeleted == false);
                            break;
                        case 5:
                            result = result.Where(x => x.IsRejected == true && x.IsDeleted == false);
                            break;
                        case 6:
                            result = result.Where(x => x.IsActive == false && x.IsDeleted == false);
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(model.JobTitle))
                {
                    result = result.Where(x => x.Title.ToLower().Contains(model.JobTitle.ToLower().Trim()));
                }

                string sdate = string.Empty;
                string edate = string.Empty;
                var sdt = new DateTime?();
                var edt = new DateTime?();
                if ((!string.IsNullOrEmpty(model.fd) && !string.IsNullOrEmpty(model.fm) && !string.IsNullOrEmpty(model.fy)) || (!string.IsNullOrEmpty(model.td) && !string.IsNullOrEmpty(model.tm) && !string.IsNullOrEmpty(model.ty)))
                {
                    if (!string.IsNullOrEmpty(model.fd) && !string.IsNullOrEmpty(model.fm) && !string.IsNullOrEmpty(model.fy))
                    {
                        sdate = string.Format("{0}/{1}/{2}", model.fm, model.fd, model.fy);
                        sdt = Convert.ToDateTime(sdate);
                    }

                    if (!string.IsNullOrEmpty(model.td) && !string.IsNullOrEmpty(model.tm) && !string.IsNullOrEmpty(model.ty))
                    {
                        if (string.IsNullOrEmpty(model.fd) && string.IsNullOrEmpty(model.fm) && string.IsNullOrEmpty(model.fy))
                        {
                            sdt = DateTime.Now;
                        }
                        edate = string.Format("{0}/{1}/{2}", model.tm, model.td, model.ty);
                        edt = Convert.ToDateTime(edate);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.fd) && !string.IsNullOrEmpty(model.fm) && !string.IsNullOrEmpty(model.fy))
                        {
                            edt = DateTime.Now;
                        }
                    }
                }
                if (sdt != null)
                {
                    result = result.Where(x => x.PublishedDate.Day >= sdt.Value.Day && x.PublishedDate.Month >= sdt.Value.Month && x.PublishedDate.Year >= sdt.Value.Year);
                }

                if (edt != null)
                {
                    result = result.Where(x => x.PublishedDate.Day <= edt.Value.Day && x.PublishedDate.Month <= edt.Value.Month && x.PublishedDate.Year <= edt.Value.Year);
                }

                rows = result.Count();
                jobs = result.OrderByDescending(x => x.DateCreated).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize)).Take(pageSize).ToList();
            }

            ViewBag.TypeId = model.TypeId;
            ViewBag.Country = country;
            ViewBag.Employer = employer;
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<Job>(jobs, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);

            return View();
        }



        public ActionResult JobsForAction(AdminJobSearchModel model, int pageNumber = 0)
        {
            List country = null;

            if (model.CountryId != null)
            {
                country = SharedService.Instance.GetCountry(model.CountryId.Value);
            }

            UserProfile employer = new UserProfile();
            if (model.Id != null)
            {
                employer = MemberService.Instance.Get(model.Id.Value);
            }

            List<Job> jobs = new List<Job>();
            int rows = 0;
            int pageSize = 10;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<Job>();

                if (model.CountryId != null)
                {
                    result = result.Where(x => x.CountryId == model.CountryId);
                }

                if (model.Id != null)
                {
                    result = result.Where(x => x.EmployerId == model.Id.Value);
                }

                if (model.TypeId != null)
                {
                    switch (model.TypeId.Value)
                    {
                        case 1:
                            result = result.Where(x => x.IsActive == true && x.IsPublished == true && x.IsDeleted == false);
                            break;
                        case 2:
                            result = result.Where(x => x.ClosingDate.Day <= DateTime.Now.Day && x.ClosingDate.Month <= DateTime.Now.Month && x.ClosingDate.Year < DateTime.Now.Year && x.IsDeleted == false);
                            break;
                        case 3:
                            result = result.Where(x => x.IsDeleted == true);
                            break;
                        case 4:
                            result = result.Where(x => x.IsPublished == false && x.IsRejected == false && x.IsDeleted == false);
                            break;
                        case 5:
                            result = result.Where(x => x.IsRejected == true && x.IsDeleted == false);
                            break;
                        case 6:
                            result = result.Where(x => x.IsActive == false && x.IsDeleted == false);
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(model.JobTitle))
                {
                    result = result.Where(x => x.Title.ToLower().Contains(model.JobTitle.ToLower().Trim()));
                }

                string sdate = string.Empty;
                string edate = string.Empty;
                var sdt = new DateTime?();
                var edt = new DateTime?();
                if ((!string.IsNullOrEmpty(model.fd) && !string.IsNullOrEmpty(model.fm) && !string.IsNullOrEmpty(model.fy)) || (!string.IsNullOrEmpty(model.td) && !string.IsNullOrEmpty(model.tm) && !string.IsNullOrEmpty(model.ty)))
                {
                    if (!string.IsNullOrEmpty(model.fd) && !string.IsNullOrEmpty(model.fy) && !string.IsNullOrEmpty(model.fy))
                    {
                        sdate = string.Format("{0}/{1}/{2}", model.fm, model.fd, model.fy);
                        sdt = Convert.ToDateTime(sdate);
                    }

                    if (!string.IsNullOrEmpty(model.td) && !string.IsNullOrEmpty(model.tm) && !string.IsNullOrEmpty(model.ty))
                    {
                        if (string.IsNullOrEmpty(model.fd) && string.IsNullOrEmpty(model.fm) && string.IsNullOrEmpty(model.fy))
                        {
                            sdt = DateTime.Now;
                        }
                        edate = string.Format("{0}/{1}/{2}", model.tm, model.td, model.ty);
                        edt = Convert.ToDateTime(edate);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.fd) && !string.IsNullOrEmpty(model.fm) && !string.IsNullOrEmpty(model.fy))
                        {
                            edt = DateTime.Now;
                        }
                    }
                }
                if (sdt != null)
                {
                    result = result.Where(x => x.PublishedDate.Day >= sdt.Value.Day && x.PublishedDate.Month >= sdt.Value.Month && x.PublishedDate.Year >= sdt.Value.Year);
                }

                if (edt != null)
                {
                    result = result.Where(x => x.PublishedDate.Day <= edt.Value.Day && x.PublishedDate.Month <= edt.Value.Month && x.PublishedDate.Year <= edt.Value.Year);
                }

                rows = result.Count();
                jobs = result.OrderByDescending(x => x.DateCreated).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize)).Take(pageSize).Take(pageSize).ToList();
            }

            ViewBag.TypeId = model.TypeId;
            ViewBag.Country = country;
            ViewBag.Employer = employer;
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<Job>(jobs, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);

            return View();
        }

        [Authorize(Roles = "SuperUser, Administrator, Sales1")]
        public ActionResult Jobs(long? countryId, string fd, string fm, string fy, string td, string tm, string ty, string ShowWithValue = "", int pageNumber = 1)
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

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" ||  uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
            {
                countryId = 40;
            }

            int pageSize = 111;
            int rows = 0;
            string sdate = string.Empty;
            string edate = string.Empty;
            var sdt = new DateTime?();
            var edt = new DateTime?();

            List<JobsByCountryEntity> jobList = new List<JobsByCountryEntity>();

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

            if (ShowWithValue == "1")
            {
                jobList = DomainService.Instance.GetJobsByCountry(countryId, true, sdt, edt, pageSize, pageNumber);
            }
            else
            {
                jobList = DomainService.Instance.GetJobsByCountry(countryId, null, sdt, edt, pageSize, pageNumber);
            }

            if (jobList.Count > 0)
            {
                rows = jobList.FirstOrDefault().MaxRows;
            }

            ViewBag.Jobs = jobList.Sum(x => x.Jobs);
            ViewBag.JobsInApproval = jobList.Sum(x => x.JobsInApproval);
            ViewBag.Applications = jobList.Sum(x => x.Applications);

            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<JobsByCountryEntity>(jobList, pageNumber, pageSize, rows);

            return View();
        }

        [Authorize(Roles = "SuperUser, Administrator, Sales1")]
        public ActionResult JobsStatus(long? countryId, string status, string fd, string fm, string fy, string td, string tm, string ty, string ShowWithValue = "", int pageNumber = 1)
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

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com"|| uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com"  || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
            {
                countryId = 40;
            }

            int pageSize = 111;
            int rows = 0;
            string sdate = string.Empty;
            string edate = string.Empty;
            var sdt = new DateTime?();
            var edt = new DateTime?();

            List<JobsByCountryEntity> jobList = new List<JobsByCountryEntity>();

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

            if (ShowWithValue == "1")
            {
                jobList = DomainService.Instance.GetJobsByCountry(countryId, status, true, sdt, edt, pageSize, pageNumber);
            }
            else
            {
                jobList = DomainService.Instance.GetJobsByCountry(countryId, status, null, sdt, edt, pageSize, pageNumber);
            }

            if (jobList.Count > 0)
            {
                rows = jobList.FirstOrDefault().MaxRows;
            }

            ViewBag.Jobs = jobList.Sum(x => x.Jobs);
            ViewBag.JobsInApproval = jobList.Sum(x => x.JobsInApproval);
            ViewBag.Applications = jobList.Sum(x => x.Applications);

            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<JobsByCountryEntity>(jobList, pageNumber, pageSize, rows);

            return View();
        }

        public ActionResult RemoveJob(long Id)
        {
            DataProvider.Execute(string.Format("DeleteJob {0}", Id));
            TempData["UpdateData"] = "Job deleted Permanently!";
            return RedirectToAction("Jobs");
            //return View();
        }

        [HttpGet]
        public ActionResult DeleteJob(long Id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteJob(long Id, string Reason)
        {
            var job = JobService.Instance.Get(Id);
            var jsList = new List<UserProfile>();
            var intList = new Hashtable();
            if (job != null)
            {
                var profile = MemberService.Instance.Get(User.Username);
                var tracking = new Tracking
                {
                    Id = Guid.NewGuid(),
                    JobId = job.Id,
                    UserId = profile.UserId,
                    Type = (int)TrackingTypes.DELETED,
                    Comments = Reason,
                    DateUpdated = DateTime.Now,
                    IsDownloaded = false
                };
                Inbox ibox = new Inbox();
                long inboxId = 0;
                List<int> typeList = new List<int>() { (int)TrackingTypes.BOOKMAKRED, (int)TrackingTypes.AUTO_MATCHED, (int)TrackingTypes.APPLIED, (int)TrackingTypes.INTERVIEW_IN_PROGRESS, (int)TrackingTypes.INTERVIEW_INITIATED };
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    List<TrackingDetail> tdList = new List<TrackingDetail>();
                    List<Tracking> tList = new List<Tracking>();

                    dataHelper.DeleteUpdate(job, User.Username);
                    dataHelper.AddEntity(tracking);

                    long adminId = DomainService.Instance.GetAdminUserId();
                    ibox = new Inbox()
                    {
                        Subject = string.Format("{0} job Deleted", job.Title),
                        Body = Reason,
                        ReceiverId = job.EmployerId,
                        SenderId = adminId,
                        ReferenceId = job.Id,
                        ReferenceType = 1,
                        ParentId = null,
                        Unread = true
                    };
                    inboxId = DomainService.Instance.ManageInbox(ibox);

                    var result = dataHelper.Get<Tracking>().Where(x => typeList.Contains(x.Type) && x.JobId == Id);
                    jsList = result.Select(x => x.Jobseeker).Distinct().ToList<UserProfile>();

                    foreach (var item in result)
                    {
                        var t = TrackingService.Instance.Get(item.Id);
                        var td = TrackingService.Instance.GetDetail(item.Id);

                        string msg = string.Empty;
                        Tracking entity = new Tracking();
                        UserProfile jobSeeker = null;
                        if (item.JobseekerId != null)
                        {
                            jobSeeker = MemberService.Instance.Get(item.JobseekerId.Value);
                        }

                        switch ((TrackingTypes)item.Type)
                        {
                            case TrackingTypes.BOOKMAKRED:
                                tList.Add(t);
                                tdList.Add(td);
                                break;
                            case TrackingTypes.AUTO_MATCHED:
                                entity = UpdateTracking(dataHelper, TrackingTypes.WITHDRAWN, item.Id, jobSeeker.Username);
                                break;
                            case TrackingTypes.APPLIED:
                                entity = UpdateTracking(dataHelper, TrackingTypes.WITHDRAWN, item.Id, jobSeeker.Username);
                                break;
                            case TrackingTypes.INTERVIEW_INITIATED:
                                var interviews = dataHelper.Get<Interview>().Where(x => x.TrackingId == item.Id && x.IsDeleted == false).ToList();
                                foreach (Interview interview in interviews)
                                {
                                    interview.Status = (int)InterviewStatus.WITHDRAW;
                                    interview.DateUpdated = DateTime.Now;
                                    interview.UpdatedBy = profile.Username;

                                    dataHelper.UpdateEntity(interview);

                                    intList.Add(jobSeeker.UserId, interview.Id);

                                    FollowUp followUp = new FollowUp()
                                    {
                                        InterviewId = interview.Id,
                                        Status = (int)FeedbackStatus.WITHDRAW,
                                        NewDate = interview.InterviewDate,
                                        NewTime = interview.InterviewDate.ToShortTimeString(),
                                        Unread = true,
                                        UserId = profile.UserId,
                                        Message = "Withdrawan from Interview",
                                        DateUpdated = DateTime.Now
                                    };

                                    dataHelper.AddEntity<FollowUp>(followUp, User.Username);
                                }

                                entity = UpdateTracking(dataHelper, TrackingTypes.WITHDRAWN, item.Id, jobSeeker.Username);
                                break;
                            case TrackingTypes.INTERVIEW_IN_PROGRESS:
                                var igInterviews = dataHelper.Get<Interview>().Where(x => x.TrackingId == item.Id && x.IsDeleted == false).ToList();
                                foreach (Interview interview in igInterviews)
                                {
                                    interview.Status = (int)InterviewStatus.WITHDRAW;
                                    interview.DateUpdated = DateTime.Now;
                                    interview.UpdatedBy = profile.Username;
                                    dataHelper.UpdateEntity(interview);
                                    intList.Add(jobSeeker.UserId, interview.Id);
                                    FollowUp followUp = new FollowUp()
                                    {
                                        InterviewId = interview.Id,
                                        Status = (int)FeedbackStatus.WITHDRAW,
                                        NewDate = interview.InterviewDate,
                                        NewTime = interview.InterviewDate.ToShortTimeString(),
                                        Unread = true,
                                        UserId = profile.UserId,
                                        Message = "Withdrawan from Interview",
                                        DateUpdated = DateTime.Now
                                    };

                                    dataHelper.AddEntity<FollowUp>(followUp, User.Username);
                                }
                                entity = UpdateTracking(dataHelper, TrackingTypes.WITHDRAWN, item.Id, jobSeeker.Username);
                                break;
                        }
                    }

                    foreach (var item in tdList)
                    {
                        if (item != null)
                        {
                            TrackingDetail tdEntity = TrackingService.Instance.GetDetail(item.Id);
                            Tracking tEntity = TrackingService.Instance.Get(item.Id);

                            dataHelper.RemoveUnsaved<TrackingDetail>(tdEntity);
                            dataHelper.RemoveUnsaved<Tracking>(tEntity);
                        }
                    }
                    dataHelper.Save();
                }

                foreach (var item in jsList)
                {
                    if (item != null)
                    {
                        StringBuilder sbInboxMsg = new StringBuilder();
                        sbInboxMsg.AppendFormat("The job <b>{0}</b> is not available.<br/><br/>", job.Title);
                        sbInboxMsg.Append("Followings have been withdrawn:<br/>");
                        if (intList[item.UserId] != null)
                        {
                            string intlink = string.Format("<a href=\"/Interview/Update?Id={0}\">Interview(s)</a>", intList[item.UserId]);
                            sbInboxMsg.AppendFormat("<ul><li>Bookmark(s)</li><li><a href=\"/Jobseeker/Index\">Application(s)</a></li><li>{0}</li></ul><br/>", intlink);
                        }
                        else
                        {
                            sbInboxMsg.Append("<ul><li>Bookmark(s)</li><li><a href=\"/Jobseeker/Index\">Application(s)</a></li><li>Interview(s)</li></ul><br/>");
                        }

                        long adminId = DomainService.Instance.GetAdminUserId();
                        Inbox iboxd = new Inbox()
                        {
                            Subject = string.Format("{0} job Deleted", job.Title),
                            Body = Reason,
                            ReceiverId = item.UserId,
                            SenderId = adminId,
                            ReferenceId = job.Id,
                            ReferenceType = 1,
                            ParentId = null,
                            Unread = true
                        };
                        long iboxId = DomainService.Instance.ManageInbox(iboxd);

                        using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/job_on_deleted.html")))
                        {
                            var body = string.Empty;

                            body = reader.ReadToEnd();
                            body = body.Replace("@@firstname", item.FirstName);
                            body = body.Replace("@@lastname", item.LastName);
                            body = body.Replace("@@jobtitle", job.Title);
                            body = body.Replace("@@url", string.Format("{0}://{1}/Inbox/Show?Id={2}", Request.Url.Scheme, Request.Url.Authority, iboxId));
                            string[] receipent = { item.Username };

                            var subject = string.Format("Job {0} not Available!", job.Title);

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                }

                using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/employer_jobdeleted.html")))
                {
                    var body = string.Empty;
                    UserProfile employer = MemberService.Instance.Get(job.EmployerId.Value);
                    body = reader.ReadToEnd();
                    body = body.Replace("@@employer", employer.Company);
                    body = body.Replace("@@jobtitle", job.Title);
                    body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                    body = body.Replace("@@url", string.Format("{0}://{1}/Inbox/Show?Id={2}", Request.Url.Scheme, Request.Url.Authority, inboxId));

                    string[] receipent = { employer.Username };
                    var subject = string.Format("{0} Job is Deleted!", job.Title);

                    AlertService.Instance.SendMail(subject, receipent, body);
                }

                TempData["UpdateData"] = string.Format("Job {0} has been deleted successfully!", job.Title);
            }
            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("Jobs");
        }

        [HttpPost]
        public ActionResult ActivateJob(long Id, string Reason)
        {
            var job = JobService.Instance.Get(Id);
            if (job != null)
            {
                var profile = MemberService.Instance.Get(User.Username);
                var tracking = new Tracking
                {
                    Id = Guid.NewGuid(),
                    JobId = job.Id,
                    UserId = profile.UserId,
                    Type = (int)TrackingTypes.ACTIVATED,
                    DateUpdated = DateTime.Now,
                    IsDownloaded = false
                };

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    job.IsActive = true;
                    job.UpdatedBy = User.Username;
                    job.DateUpdated = DateTime.Now;
                    dataHelper.UpdateEntity(job);
                    dataHelper.AddEntity(tracking);
                    dataHelper.Save();
                }

                Activity activity = new Activity()
                {
                    Comments = string.Format("{0}<br/><a href=\"/Employer/Index\">CLICK HERE</a> or <a href=\"/job/{1}-{2}\">HERE</a> to see the activated job.", Reason, job.PermaLink, job.Id),
                    ActivityDate = DateTime.Now,
                    UserId = job.EmployerId.Value,
                    DateUpdated = DateTime.Now,
                    UpdatedBy = User.Username,
                    Type = (int)ActivityTypes.JOB_ACTIVATED,
                    Unread = true
                };
                MemberService.Instance.Track(activity);

                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/employer_jobactivate.html"));
                var body = string.Empty;

                if (reader != null)
                {
                    UserProfile employer = MemberService.Instance.Get(job.EmployerId.Value);
                    body = reader.ReadToEnd();
                    body = body.Replace("@@employer", employer.Company);
                    body = body.Replace("@@jobtitle", job.Title);
                    body = body.Replace("@@joburl",
                        string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                    body = body.Replace("@@url", string.Format("{0}://{1}/Inbox/Show?Id={2}", Request.Url.Scheme, Request.Url.Authority, activity.Id));
                    string[] receipent = { employer.Username };
                    var subject = string.Format("{0} Job is Activated!", job.Title);

                    AlertService.Instance.SendMail(subject, receipent, body);
                }
                TempData["UpdateData"] = string.Format("Job {0} has beed activated successfully!", job.Title);
            }
            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            return Redirect("/Admin/Jobs");
        }

        public ActionResult PhotoList(int type, long? Id, long? CountryId, string fd, string fm, string fy, string td, string tm, string ty)
        {
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

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
            {
                CountryId = 40;
            }

            List<UserProfile> list = new List<UserProfile>();
            string sdate = string.Empty;
            string edate = string.Empty;
            var sdt = new DateTime?();
            var edt = new DateTime?();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                var result = context.Photos.Where(x => x.UserProfile.Type == type && x.IsApproved == false && x.IsRejected == false && x.IsDeleted == false);
                if (CountryId != null)
                {
                    result = result.Where(x => x.UserProfile.CountryId == CountryId.Value);
                }

                if (Id != null)
                {
                    result = result.Where(x => x.UserId == Id.Value);
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

                if (sdt != null && edt != null)
                {
                    result = result.Where(x => x.DateUpdated.Day >= sdt.Value.Day && x.DateUpdated.Month >= sdt.Value.Month && x.DateUpdated.Year >= sdt.Value.Year && x.DateUpdated.Day <= edt.Value.Day && x.DateUpdated.Month <= edt.Value.Month && x.DateUpdated.Year <= edt.Value.Year);
                }

                list = result.Select(x => x.UserProfile).Distinct().ToList();
            }
            return View(list);
        }

        public ActionResult PhotoApproval(long Id)
        {
            List<Photo> list = new List<Photo>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                list = context.Photos.Where(x => x.UserId == Id && x.IsApproved == false && x.IsRejected == false && x.IsDeleted == false).ToList();
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult ApproveJob(long JobId, string redirect, bool PostOnMedia)
        {
            var profile = MemberService.Instance.Get(User.Username);
            var job = JobService.Instance.Get(JobId);
            bool edited = false;

            if (job != null)
            {
                var employer = MemberService.Instance.Get(job.EmployerId.Value);
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    if (job.InEditMode == true)
                    {
                        edited = true;
                        job.Description = job.NewDescription;
                        job.Summary = job.NewSummary;
                        job.Requirements = job.NewRequirements;

                        job.MinimumExperience = job.NewMinimumExperience;
                        job.MaximumExperience = job.NewMaximumExperience;
                        job.Currency = job.NewCurrency;
                        job.MinimumSalary = job.NewMinimumSalary;
                        job.MaximumSalary = job.NewMaximumSalary;
                        job.MinimumAge = job.NewMinimumAge;
                        job.MaximumAge = job.NewMaximumAge;
                        job.EmploymentTypeId = job.NewEmploymentTypeId;
                        job.QualificationId = job.NewQualificationId;
                    }
                    else
                    {
                        job.PublishedDate = DateTime.Now;
                        job.ClosingDate = DateTime.Now.AddDays(30);
                    }

                    job.IsPublished = true;
                    job.IsRejected = false;
                    job.InEditMode = false;



                    if (PostOnMedia)
                    {
                        if (job.IsPublished.Value)
                        {
                            if (!job.IsPostedOnTwitter)
                            {
                                if (!job.InEditMode)
                                {
                                    job.IsPostedOnTwitter = true;
                                }
                            }
                        }
                    }
                    dataHelper.Update(job, User.Username);

                    var tracking = new Tracking
                    {
                        Id = Guid.NewGuid(),
                        JobId = job.Id,
                        UserId = profile.UserId,
                        Type = (int)TrackingTypes.APPROVED,
                        DateUpdated = DateTime.Now,
                        IsDownloaded = false
                    };

                    dataHelper.Add(tracking);

                    (new PortalDataService()).ManageStream(employer.UserId, job.Id, PostTypes.JOB_POSTED);

                    //Inbox parent = DomainService.Instance.GetParentInbox(job.EmployerId.Value, job.Id);

                    long adminId = DomainService.Instance.GetAdminUserId();
                    Inbox ibox = new Inbox()
                    {
                        Subject = string.Format("{0} job approved", job.Title),
                        Body = string.Format("<b>{0}</b> job has been approved.", job.Title),
                        ReceiverId = job.EmployerId,
                        SenderId = adminId,
                        ReferenceId = job.Id,
                        ReferenceType = 1,
                        ParentId = null,
                        Unread = true
                    };
                    long inboxId = DomainService.Instance.ManageInbox(ibox);

                    try
                    {
                        if (PostOnMedia)
                        {
                            if (job.IsPublished.Value)
                            {
                                if (job.IsPostedOnTwitter)
                                {
                                    if (!job.InEditMode)
                                    {
                                        PostingService twitterService = new PostingService(new TwitterPostingService());
                                        string tweet_info = string.Empty;

                                        while (string.IsNullOrEmpty(tweet_info.Trim()))
                                        {
                                            tweet_info = twitterService.Post(job);
                                            if (!string.IsNullOrEmpty(tweet_info))
                                            {
                                                break;
                                            }
                                        }

                                        PostingService facebookService = new PostingService(new FacebookPostingService());
                                        string post_info = string.Empty;

                                        while (string.IsNullOrEmpty(post_info.Trim()))
                                        {
                                            post_info = facebookService.Post(job);
                                            if (!string.IsNullOrEmpty(post_info))
                                            {
                                                break;
                                            }
                                        }

                                        JobFeeder.Generate(Server.MapPath("/jobsfeed.xml"));

                                        if (!string.IsNullOrEmpty(tweet_info))
                                        {
                                            (new PortalDataService()).TrackSocialMediaPost(job.Id, tweet_info, "Twitter");
                                        }

                                        if (!string.IsNullOrEmpty(post_info))
                                        {
                                            (new PortalDataService()).TrackSocialMediaPost(job.Id, post_info, "Facebook");
                                        }

                                        //if (DomainService.Instance.HasJobQuota(job.EmployerId.Value))
                                        //{
                                        //    _service.ManageAccount(job.EmployerId.Value, null, null, null, null, 1, job.Id);
                                        //}
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string baseUrl = ConfigurationManager.AppSettings["SiteUrl"];
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

                        msg.Subject = "Error occured while posting job on social media";
                        msg.Body = new TextPart("html") { Text = body };

                        //using (SmtpClient oSmtp = new SmtpClient())
                        //{
                        //    oSmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        //    oSmtp.Connect("smtp.mailgun.org", 587, false);
                        //    oSmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                        //    oSmtp.Authenticate(postmail, postpassword);

                        //    oSmtp.Send(msg);
                        //    oSmtp.Disconnect(true);
                        //}

                    }
                }

                if (edited)
                {
                    //using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/edited_job_approved.html")))
                    //{
                    //    var body = reader.ReadToEnd();
                    //    body = body.Replace("@@employer", employer.Company);
                    //    body = body.Replace("@@jobtitle", job.Title);
                    //    if (job.IsFeaturedJob != null && job.IsFeaturedJob.Value == true)
                    //    {
                    //        body = body.Replace("@@featured",
                    //            "This is featuered job which will appear at main page as well as on top of search results!");
                    //    }
                    //    else
                    //    {
                    //        body = body.Replace("@@featured", "");
                    //    }
                    //    body = body.Replace("@@joburl",
                    //        string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink,
                    //            job.Id));

                    //    string[] receipent = { employer.Username };
                    //    var subject = string.Format("Your changes for {0} Job have been Approved", job.Title);

                    //    AlertService.Instance.SendMail(subject, receipent, body);
                    //}
                }
                else
                {
                    //using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/employer_jobapproved.html")))
                    //{
                    //    var body = reader.ReadToEnd();
                    //    body = body.Replace("@@employer", employer.Company);
                    //    body = body.Replace("@@jobtitle", job.Title);
                    //    if (job.IsFeaturedJob != null && job.IsFeaturedJob.Value == true)
                    //    {
                    //        body = body.Replace("@@featured",
                    //            "This is featuered job which will appear at main page as well as on top of search results!");
                    //    }
                    //    else
                    //    {
                    //        body = body.Replace("@@featured", "");
                    //    }
                    //    body = body.Replace("@@joburl",
                    //        string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink,
                    //            job.Id));

                    //    string[] receipent = { employer.Username };
                    //    var subject = string.Format("{0} Job has been Approved!", job.Title);

                    //    AlertService.Instance.SendMail(subject, receipent, body);
                    //}
                }

                var jobseeker_list = EmployerService.Instance.AutomatchedJobseekers(job, User.Username);
                if (jobseeker_list.Count() > 0)
                {
                    var sbbody = new StringBuilder();

                    sbbody.Append("<table>");
                    sbbody.Append("<tr>");
                    sbbody.Append("<th>Job Title</th><th>Posted Date</th><th>Expiry Date</th><th>Number of Matches</th>");
                    sbbody.Append("</tr>");
                    var jobLink = string.Empty;

                    var automatchLink =
                        string.Format(
                            "<a href=\"{0}://{1}/applications\" target=\"_Blank\">{2}</a> | <a href=\"{0}://{1}/applications\" target=\"_Blank\">View</a>",
                            Request.Url.Scheme, Request.Url.Authority, jobseeker_list.Count);
                    jobLink = string.Format("<a href=\"{0}://{1}/job/{2}-{3}\" target=\"_Blank\">{4}</a>",
                        Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id, job.Title);

                    sbbody.Append("<tr>");
                    sbbody.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>", jobLink,
                        job.PublishedDate.ToString("MM/dd/yyyy"), job.ClosingDate.ToString("MM/dd/yyyy"),
                        automatchLink);
                    sbbody.Append("</tr>");
                    sbbody.Append("</table>");

                    using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/employer_automatch.html")))
                    {
                        var body = reader.ReadToEnd();
                        body = body.Replace("@@employer", employer.Company);
                        body = body.Replace("@@jobtitle", job.Title);
                        body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                        body = body.Replace("@@list", sbbody.ToString());
                        string[] receipent = { employer.Username };
                        var subject = string.Format("Match List for {0} Job", job.Title);

                        AlertService.Instance.SendMail(subject, receipent, body);
                    }

                    if (jobseeker_list.Count() > 0)
                    {
                        jseekerList = jobseeker_list.ToList();
                        Action sendAction = new Action(SendJobSeeker);
                        Task sendTask = new Task(sendAction, TaskCreationOptions.None);
                        sendTask.Start();
                    }
                }
                TempData["UpdateData"] = string.Format("Job {0} has beed approved successfully!", job.Title);
            }
            return Redirect(redirect);
        }

        public void SendJobSeeker()
        {
            string body = string.Empty;
            foreach (AutomatchJobseeker item in jseekerList)
            {
                Job job = JobService.Instance.Get(item.Job.Id);
                UserProfile employer = MemberService.Instance.Get(job.EmployerId.Value);
                UserProfile jobSeeker = MemberService.Instance.Get(item.Jobseeker.UserId);

                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/jobseeker_employer_automatch.html"));
                if (reader != null)
                {
                    body = reader.ReadToEnd();
                    body = body.Replace("@@firstname", jobSeeker.FirstName);
                    body = body.Replace("@@lastname", jobSeeker.LastName);
                    body = body.Replace("@@employer", !string.IsNullOrEmpty(employer.Company) ? employer.Company : string.Format("{0} {1}", employer.FirstName, employer.LastName));
                    body = body.Replace("@@jobtitle", job.Title);
                    body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                    body = body.Replace("@@viewurl", string.Format("{0}://{1}/jobseeker/index", Request.Url.Scheme, Request.Url.Authority));

                    string[] receipent = { jobSeeker.Username };
                    var subject = string.Format("Profile Matched for {0}", job.Title);

                    AlertService.Instance.SendMail(subject, receipent, body);
                }
            }
        }

        public ActionResult RejectJob(long Id)
        {
            ViewBag.JobId = Id;
            return View();
        }

        [HttpPost]
        public ActionResult RejectJob(long Id, string Reason)
        {
            var job = JobService.Instance.Get(Id);
            bool edited = false;
            if (job != null)
            {
                var employer = MemberService.Instance.Get(job.EmployerId.Value);

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    if (job.IsPublished.Value == false && job.InEditMode == true)
                    {
                        edited = true;
                        job.NewMinimumExperience = null;
                        job.NewMaximumExperience = null;
                        job.NewCurrency = null;
                        job.NewMinimumSalary = null;
                        job.NewMaximumSalary = null;
                        job.NewMinimumAge = null;
                        job.NewMaximumAge = null;
                        job.NewEmploymentTypeId = null;
                        job.NewQualificationId = null;

                        job.IsPublished = false;
                        job.IsRejected = true;
                        job.InEditMode = false;
                    }
                    else if (job.IsPublished.Value == false && job.InEditMode == false)
                    {
                        job.IsPublished = false;
                        job.IsRejected = true;
                    }

                    dataHelper.Update(job, User.Username);

                    var tracking = new Tracking
                    {
                        Id = Guid.NewGuid(),
                        JobId = job.Id,
                        UserId = employer.UserId,
                        Type = (int)TrackingTypes.REJECTED,
                        Comments = Reason,
                        DateUpdated = DateTime.Now,
                        IsDownloaded = false
                    };

                    dataHelper.Add(tracking);
                }

                //Inbox parent = DomainService.Instance.GetParentInbox(job.EmployerId.Value, job.Id);
                long adminId = DomainService.Instance.GetAdminUserId();
                Inbox ibox = new Inbox()
                {
                    Subject = string.Format("{0} job Rejected", job.Title),
                    Body = Reason,
                    ReceiverId = job.EmployerId,
                    SenderId = adminId,
                    ReferenceId = job.Id,
                    ReferenceType = 1,
                    ParentId = null,
                    Unread = true
                };
                long inboxId = DomainService.Instance.ManageInbox(ibox);

                if (edited)
                {
                    //using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/edited_job_rejected.html")))
                    //{
                    //    var body = string.Empty;
                    //    body = reader.ReadToEnd();
                    //    body = body.Replace("@@employer", employer.Company);
                    //    body = body.Replace("@@jobtitle", job.Title);
                    //    body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                    //    body = body.Replace("@@url", string.Format("{0}://{1}/Inbox/Show?Id={2}", Request.Url.Scheme, Request.Url.Authority, inboxId));
                    //    body = body.Replace("@@eurl", string.Format("{0}://{1}/Employer/UpdateJob?id={2}", Request.Url.Scheme, Request.Url.Authority, job.Id));
                    //    string[] receipent = { employer.Username };
                    //    var subject = string.Format("Your changes for {0} Job have been Rejected", job.Title);

                    //    AlertService.Instance.SendMail(subject, receipent, body);
                    //}
                }
                else
                {
                    //using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/employer_jobrejected.html")))
                    //{
                    //    var body = string.Empty;
                    //    body = reader.ReadToEnd();
                    //    body = body.Replace("@@employer", employer.Company);
                    //    body = body.Replace("@@jobtitle", job.Title);
                    //    body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                    //    body = body.Replace("@@url", string.Format("{0}://{1}/Inbox/Show?Id={2}", Request.Url.Scheme, Request.Url.Authority, inboxId));
                    //    body = body.Replace("@@eurl", string.Format("{0}://{1}/Employer/UpdateJob?id={2}", Request.Url.Scheme, Request.Url.Authority, job.Id));
                    //    string[] receipent = { employer.Username };
                    //    var subject = string.Format("{0} Job has been Rejected!", job.Title);

                    //    AlertService.Instance.SendMail(subject, receipent, body);
                    //}
                }
                TempData["UpdateData"] = string.Format("Job {0} has beed rejected successfully!", job.Title);
            }
            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("Jobs");
        }

        [HttpPost]
        public ActionResult DeactivateJob(long Id, string Reason)
        {
            var profile = MemberService.Instance.Get(User.Username);
            var job = JobService.Instance.Get(Id);
            var jsList = new List<UserProfile>();
            var intList = new Hashtable();

            if (job != null)
            {
                Tracking tracking = new Tracking();
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    job.IsActive = false;
                    job.DateUpdated = DateTime.Now;
                    job.UpdatedBy = User.Username;

                    tracking = new Tracking
                    {
                        Id = Guid.NewGuid(),
                        JobId = job.Id,
                        UserId = job.EmployerId.Value,
                        Type = (int)TrackingTypes.DEACTIVATED,
                        Comments = Reason,
                        DateUpdated = DateTime.Now,
                        IsDownloaded = false
                    };
                }
                Activity activity = new Activity();
                long inboxId = 0;
                List<int> typeList = new List<int>() { (int)TrackingTypes.BOOKMAKRED, (int)TrackingTypes.AUTO_MATCHED, (int)TrackingTypes.APPLIED, (int)TrackingTypes.INTERVIEW_IN_PROGRESS, (int)TrackingTypes.INTERVIEW_INITIATED };
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    List<TrackingDetail> tdList = new List<TrackingDetail>();
                    List<Tracking> tList = new List<Tracking>();
                    dataHelper.UpdateEntity(job);
                    dataHelper.AddEntity(tracking);


                    long adminId = DomainService.Instance.GetAdminUserId();
                    Inbox ibox = new Inbox()
                    {
                        Subject = string.Format("{0} job De-activated", job.Title),
                        Body = Reason,
                        ReceiverId = job.EmployerId,
                        SenderId = adminId,
                        ReferenceId = job.Id,
                        ReferenceType = 1,
                        ParentId = null,
                        Unread = true
                    };
                    inboxId = DomainService.Instance.ManageInbox(ibox);

                    var result = dataHelper.Get<Tracking>().Where(x => typeList.Contains(x.Type) && x.JobId == Id);
                    jsList = result.Select(x => x.Jobseeker).Distinct().ToList<UserProfile>();

                    foreach (var item in result)
                    {
                        var t = TrackingService.Instance.Get(item.Id);
                        var td = TrackingService.Instance.GetDetail(item.Id);

                        string msg = string.Empty;
                        Tracking entity = new Tracking();
                        UserProfile jobSeeker = null;
                        if (item.JobseekerId != null)
                        {
                            jobSeeker = MemberService.Instance.Get(item.JobseekerId.Value);
                        }

                        switch ((TrackingTypes)item.Type)
                        {
                            case TrackingTypes.BOOKMAKRED:
                                tList.Add(t);
                                tdList.Add(td);
                                break;
                            case TrackingTypes.AUTO_MATCHED:
                                entity = UpdateTracking(dataHelper, TrackingTypes.WITHDRAWN, item.Id, jobSeeker.Username);
                                break;
                            case TrackingTypes.APPLIED:
                                entity = UpdateTracking(dataHelper, TrackingTypes.WITHDRAWN, item.Id, jobSeeker.Username);
                                break;
                            case TrackingTypes.INTERVIEW_INITIATED:
                                var interviews = dataHelper.Get<Interview>().Where(x => x.TrackingId == item.Id && x.IsDeleted == false).ToList();
                                foreach (Interview interview in interviews)
                                {
                                    interview.Status = (int)InterviewStatus.WITHDRAW;
                                    interview.DateUpdated = DateTime.Now;
                                    interview.UpdatedBy = profile.Username;
                                    dataHelper.UpdateEntity(interview);

                                    intList.Add(jobSeeker.UserId, interview.Id);

                                    FollowUp followUp = new FollowUp()
                                    {
                                        InterviewId = interview.Id,
                                        Status = (int)FeedbackStatus.WITHDRAW,
                                        NewDate = interview.InterviewDate,
                                        NewTime = interview.InterviewDate.ToShortTimeString(),
                                        Unread = true,
                                        UserId = profile.UserId,
                                        Message = "Withdrawan from Interview",
                                        DateUpdated = DateTime.Now
                                    };

                                    dataHelper.AddEntity<FollowUp>(followUp, User.Username);
                                }

                                entity = UpdateTracking(dataHelper, TrackingTypes.WITHDRAWN, item.Id, jobSeeker.Username);
                                break;
                            case TrackingTypes.INTERVIEW_IN_PROGRESS:
                                var igInterviews = dataHelper.Get<Interview>().Where(x => x.TrackingId == item.Id && x.IsDeleted == false).ToList();
                                foreach (Interview interview in igInterviews)
                                {
                                    interview.Status = (int)InterviewStatus.WITHDRAW;
                                    interview.DateUpdated = DateTime.Now;
                                    interview.UpdatedBy = profile.Username;
                                    dataHelper.UpdateEntity(interview);
                                    intList.Add(jobSeeker.UserId, interview.Id);
                                    FollowUp followUp = new FollowUp()
                                    {
                                        InterviewId = interview.Id,
                                        Status = (int)FeedbackStatus.WITHDRAW,
                                        NewDate = interview.InterviewDate,
                                        NewTime = interview.InterviewDate.ToShortTimeString(),
                                        Unread = true,
                                        UserId = profile.UserId,
                                        Message = "Withdrawan from Interview",
                                        DateUpdated = DateTime.Now
                                    };

                                    dataHelper.AddEntity<FollowUp>(followUp, User.Username);
                                }
                                entity = UpdateTracking(dataHelper, TrackingTypes.WITHDRAWN, item.Id, jobSeeker.Username);
                                break;
                        }
                    }

                    foreach (var item in tdList)
                    {
                        if (item != null)
                        {
                            TrackingDetail tdEntity = TrackingService.Instance.GetDetail(item.Id);
                            Tracking tEntity = TrackingService.Instance.Get(item.Id);

                            dataHelper.RemoveUnsaved<TrackingDetail>(tdEntity);
                            dataHelper.RemoveUnsaved<Tracking>(tEntity);
                        }
                    }

                    dataHelper.Save();
                }

                foreach (var item in jsList)
                {
                    if (item != null)
                    {
                        StringBuilder sbInboxMsg = new StringBuilder();
                        sbInboxMsg.AppendFormat("The job <b>{0}</b> is not available temporarily.<br/><br/>", job.Title);
                        sbInboxMsg.Append("Followings have been withdrawn:<br/>");
                        if (intList[item.UserId] != null)
                        {
                            string intlink = string.Format("<a href=\"/Interview/Update?Id={0}\">Interview(s)</a>", intList[item.UserId]);
                            sbInboxMsg.AppendFormat("<ul><li>Bookmark(s)</li><li><a href=\"/Jobseeker/Index\">Application(s)</a></li><li>{0}</li></ul><br/>", intlink);
                        }
                        else
                        {
                            sbInboxMsg.Append("<ul><li>Bookmark(s)</li><li><a href=\"/Jobseeker/Index\">Application(s)</a></li><li>Interview(s)</li></ul><br/>");
                        }



                        long adminId = DomainService.Instance.GetAdminUserId();
                        Inbox iboxd = new Inbox()
                        {
                            Subject = string.Format("{0} job De-Activated", job.Title),
                            Body = Reason,
                            ReceiverId = item.UserId,
                            SenderId = adminId,
                            ReferenceId = job.Id,
                            ReferenceType = 1,
                            ParentId = null,
                            Unread = true
                        };
                        long iboxId = DomainService.Instance.ManageInbox(iboxd);

                        using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/job_on_deactivated.html")))
                        {
                            var body = string.Empty;

                            body = reader.ReadToEnd();
                            body = body.Replace("@@firstname", item.FirstName);
                            body = body.Replace("@@lastname", item.LastName);
                            body = body.Replace("@@jobtitle", job.Title);
                            body = body.Replace("@@url", string.Format("{0}://{1}/Inbox/Show?Id={2}", Request.Url.Scheme, Request.Url.Authority, iboxId));
                            string[] receipent = { item.Username };

                            var subject = string.Format("Job {0} not Available Temporarily!", job.Title);

                            AlertService.Instance.SendMail(subject, receipent, body);
                        }
                    }
                }
                using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/employer_jobdeactivate.html")))
                {
                    var body = string.Empty;

                    UserProfile employer = MemberService.Instance.Get(job.EmployerId.Value);
                    body = reader.ReadToEnd();
                    body = body.Replace("@@employer", employer.Company);
                    body = body.Replace("@@jobtitle", job.Title);
                    body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                    body = body.Replace("@@url", string.Format("{0}://{1}/Inbox/Show?Id={2}", Request.Url.Scheme, Request.Url.Authority, inboxId));
                    string[] receipent = { employer.Username };
                    var subject = string.Format("{0} Job has been Deactivated!", job.Title);

                    AlertService.Instance.SendMail(subject, receipent, body);
                }

                TempData["UpdateData"] = string.Format("Job {0} has beed deactivated successfully!", job.Title);
            }
            return RedirectToAction("Jobs");
        }

        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult SendMessage(Inbox model)
#pragma warning restore CS0246 // The type or namespace name 'Inbox' could not be found (are you missing a using directive or an assembly reference?)
        {
            //Inbox parent = DomainService.Instance.GetInboxItem(model.Id.Value);
            //if (parent != null)
            //{
            Inbox ibox = new Inbox()
            {
                Subject = model.Subject,
                Body = model.Body,
                ReceiverId = (User.Id == model.ReceiverId) ? model.SenderId : model.ReceiverId,
                SenderId = User.Id,
                ReferenceId = model.ReferenceId,
                ReferenceType = model.ReferenceType,
                ParentId = model.ParentId,
                Unread = true
            };
            long inboxId = DomainService.Instance.ManageInbox(ibox);

            UserProfile receiver = MemberService.Instance.Get(ibox.ReceiverId.Value);
            var subject = string.Format("Message from Joblisting Support Team");
            string[] receipent = { string.Empty };
            using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/inbox_message.html")))
            {
                string body = reader.ReadToEnd();

                body = body.Replace("@@sender", "Joblisting Support Team");
                body = body.Replace("@@receiver", (!string.IsNullOrEmpty(receiver.Company) ? receiver.Company : receiver.FirstName + " " + receiver.LastName));
                body = body.Replace("@@url", string.Format("{0}://{1}/inbox/show?Id={2}", Request.Url.Scheme, Request.Url.Authority, inboxId));

                receipent[0] = receiver.Username;
                AlertService.Instance.SendMail(subject, receipent, body);
            }

            TempData["UpdateData"] = "Message sent successfully!";
            //}

            //if (Request.UrlReferrer != null)
            //{
            //    return Redirect(Request.UrlReferrer.ToString());
            //}
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult ApprovePhoto(long Id)
        {
            Photo photo = new Photo();
            List<string> connections = new List<string>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                photo = dataHelper.Get<Photo>().SingleOrDefault(x => x.Id == Id);

                if (!string.IsNullOrEmpty(photo.NewImageSize))
                {
                    photo.ImageSize = photo.NewImageSize;
                    photo.NewImageSize = null;
                }

                if (photo.IsApproved == false && photo.InEditMode == false)
                {
                    photo.IsApproved = true;
                }
                else if (photo.IsApproved == false && photo.InEditMode == true)
                {
                    photo.Image = photo.NewImage;

                    photo.IsApproved = true;
                    photo.IsRejected = false;
                    photo.InEditMode = false;
                    photo.NewImage = null;

                }
                (new PortalDataService()).ManageStream(photo.UserId, photo.Id, PostTypes.IMAGE_UPLADED);
                dataHelper.Update(photo);
            }

            var profile = MemberService.Instance.Get(photo.UserId);

            long adminId = DomainService.Instance.GetAdminUserId();
            Inbox ibox = new Inbox()
            {
                Subject = string.Format("Photo Approved"),
                Body = string.Format("Your photo has been approved!"),
                ReceiverId = photo.UserId,
                SenderId = adminId,
                ReferenceId = photo.Id,
                ReferenceType = 0,
                ParentId = null,
                Unread = true
            };
            long inboxId = DomainService.Instance.ManageInbox(ibox);

            TempData["UpdateData"] = "Photo approved successfully!";

            using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/photoapproved.html")))
            {
                var body = string.Empty;

                body = reader.ReadToEnd();
                body = body.Replace("@@firstname", profile.FirstName);
                body = body.Replace("@@lastname", profile.LastName);
                body = body.Replace("@@url", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, profile.PermaLink));

                string[] receipent = { profile.Username };
                var subject = "Your Photo is Approved";

                AlertService.Instance.SendMail(subject, receipent, body);
            }
            return RedirectToAction("PhotoApproval", new { Id = photo.UserId });
        }

        public ActionResult RejectPhoto(long Id, string Reason)
        {
            Photo photo = new Photo();
            long photoId = 0;
            long photoUserId = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                photo = dataHelper.Get<Photo>().SingleOrDefault(x => x.Id == Id);
                photo.Reason = Reason;

                photoId = photo.Id;
                photoUserId = photo.UserId;

                if (photo.IsRejected == false && photo.InEditMode == false)
                {
                    dataHelper.Remove<Photo>(photo);
                }
                else if (photo.IsRejected == false && photo.InEditMode == true)
                {
                    photo.IsRejected = false;
                    photo.IsApproved = true;
                    photo.InEditMode = false;
                    photo.NewImage = null;
                    dataHelper.Update(photo);
                }
            }
            var profile = MemberService.Instance.Get(photoUserId);

            long adminId = DomainService.Instance.GetAdminUserId();
            Inbox ibox = new Inbox()
            {
                Subject = string.Format("Photo Rejected"),
                Body = string.Format("{0}", Reason),
                ReceiverId = photoUserId,
                SenderId = adminId,
                ReferenceId = photoId,
                ReferenceType = 0,
                ParentId = null,
                Unread = true
            };
            long inboxId = DomainService.Instance.ManageInbox(ibox);

            TempData["UpdateData"] = "Photo rejected successfully!";

            var reader = new StreamReader(Server.MapPath("~/Templates/Mail/photorejected.html"));
            var body = string.Empty;

            if (reader != null)
            {
                body = reader.ReadToEnd();
                body = body.Replace("@@firstname", profile.FirstName);
                body = body.Replace("@@lastname", profile.LastName);
                body = body.Replace("@@url", string.Format("{0}://{1}/Inbox/Show?Id={2}", Request.Url.Scheme, Request.Url.Authority, inboxId));
                body = body.Replace("@@uurl", string.Format("{0}://{1}/profile-resume", Request.Url.Scheme, Request.Url.Authority));

                string[] receipent = { profile.Username };
                var subject = "Your Photo has been Rejected";

                AlertService.Instance.SendMail(subject, receipent, body);
            }

            return RedirectToAction("PhotoApproval", new { Id = photo.UserId });
        }

        public ActionResult DeletePhoto(long Id)
        {
            Photo photo = new Photo();
            long photoId = 0;
            long photoUserId = 0;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                photo = dataHelper.Get<Photo>().SingleOrDefault(x => x.Id == Id);

                photoId = photo.Id;
                photoUserId = photo.UserId;

                if (photo.IsRejected == false && photo.IsApproved == false && photo.InEditMode == false)
                {
                    dataHelper.Remove<Photo>(photo);
                }
                else if (photo.IsRejected == false && photo.IsApproved == false && photo.InEditMode == true)
                {
                    photo.IsRejected = false;
                    photo.IsApproved = true;
                    photo.InEditMode = false;
                    photo.NewImage = null;
                    dataHelper.Update(photo);
                }
            }
            //var profile = MemberService.Instance.Get(photoUserId);

            //long adminId = DomainService.Instance.GetAdminUserId();
            //Inbox ibox = new Inbox()
            //{
            //    Subject = "Photo Deleted",
            //    Body = "Photo has been deleted",
            //    ReceiverId = photoUserId,
            //    SenderId = adminId,
            //    ReferenceId = photoId,
            //    ReferenceType = 0,
            //    ParentId = null,
            //    Unread = true
            //};
            //long inboxId = DomainService.Instance.ManageInbox(ibox);

            TempData["UpdateData"] = "Photo deleted successfully!";

            //var reader = new StreamReader(Server.MapPath("~/Templates/Mail/photorejected.html"));
            //var body = string.Empty;

            //if (reader != null)
            //{
            //    body = reader.ReadToEnd();
            //    body = body.Replace("@@firstname", profile.FirstName);
            //    body = body.Replace("@@lastname", profile.LastName);
            //    body = body.Replace("@@url", string.Format("{0}://{1}/Inbox/Show?Id={2}", Request.Url.Scheme, Request.Url.Authority, inboxId));
            //    body = body.Replace("@@uurl", string.Format("{0}://{1}/profile-resume", Request.Url.Scheme, Request.Url.Authority));

            //    string[] receipent = { profile.Username };
            //    var subject = "Your Photo has been Rejected";

            //    AlertService.Instance.SendMail(subject, receipent, body);
            //}

            return RedirectToAction("PhotoApproval", new { Id = photo.UserId });
        }
        #endregion End Jobg8 Section


        /// <summary>
        /// Manage Individual Profile
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Administrator, SuperUser")]
        public ActionResult ManageIndividual(long Id)
        {
            var model = new JobseekerProfileModel();
            var jobSeeker = MemberService.Instance.Get(Id);
            //var jobSeeker = MemberService.Instance.Get(User.Username);
            if (jobSeeker != null)
            {

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
            }
            return View(model);
        }





        /// <summary>
        /// Manage Individual Profile
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        //[HttpGet]
        //[Authorize(Roles = "Administrator, SuperUser")]
        //public ActionResult ManageIndividual1(long Id)
        //{
        //    var model = new JobseekerProfileModel();

        //    if (User != null)
        //    {
        //        var jobSeeker = MemberService.Instance.Get(Id);
        //        if (jobSeeker != null)
        //        {
        //            model.Id = jobSeeker.UserId;
        //            model.FirstName = jobSeeker.FirstName;
        //            model.LastName = jobSeeker.LastName;
        //            model.Address = jobSeeker.Address;
        //            model.Zip = jobSeeker.Zip;
        //            model.PhoneCountryCode = jobSeeker.PhoneCountryCode;
        //            model.Phone = jobSeeker.Phone;
        //            model.MobileCountryCode = jobSeeker.MobileCountryCode;
        //            model.Mobile = jobSeeker.Mobile;
        //            model.CountryId = jobSeeker.CountryId;
        //            model.StateId = jobSeeker.StateId;
        //            model.City = jobSeeker.City;
        //            if (jobSeeker.DateOfBirth.Length == 4)
        //            {
        //                model.BYear = jobSeeker.DateOfBirth;
        //            }
        //            else
        //            {
        //                string[] dob = jobSeeker.DateOfBirth.Split('-');
        //                model.BYear = dob[0];
        //                model.BMonth = dob[1];
        //                model.BDay = dob[2];
        //            }
        //            model.PremaLink = jobSeeker.PermaLink;
        //            model.School = jobSeeker.School;
        //            model.University = jobSeeker.University;
        //            model.Title = jobSeeker.Title;
        //            model.CategoryId = jobSeeker.CategoryId;
        //            model.SpecializationId = jobSeeker.SpecializationId;
        //            model.CurrentEmployer = jobSeeker.CurrentEmployer;
        //            model.PreviousEmployer = jobSeeker.PreviousEmployer;
        //            model.Experience = jobSeeker.Experience;
        //            model.CurrentCurrency = jobSeeker.CurrentCurrency;
        //            model.ExpectedCurrency = jobSeeker.ExpectedCurrency;
        //            model.CurrentSalary = jobSeeker.DrawingSalary;
        //            model.ExpectedSalary = jobSeeker.ExpectedSalary;
        //            model.QualificationId = jobSeeker.QualificationId;
        //            model.AreaOfExpertise = jobSeeker.AreaOfExpertise;
        //            model.TechnicalSkills = jobSeeker.TechnicalSkills;
        //            model.ManagementSkills = jobSeeker.ManagementSkills;
        //            model.Summary = jobSeeker.Summary;
        //            model.Education = jobSeeker.Education;
        //            model.ProfessionalExperience = jobSeeker.ProfessionalExperience;
        //            model.ProfessionalCertification = jobSeeker.Certifications;
        //            model.ProfessionalAffiliation = jobSeeker.Affiliations;
        //            model.Gender = jobSeeker.Gender;
        //            model.RelationshipStatus = jobSeeker.RelationshipStatus;
        //            model.Website = jobSeeker.Website;
        //            model.Facebook = jobSeeker.Facebook;
        //            model.Twitter = jobSeeker.Twitter;
        //            model.LinkedIn = jobSeeker.LinkedIn;
        //            model.GooglePlus = jobSeeker.GooglePlus;

        //        }
        //    }
        //    return View(model);
        //}

        /// <summary>
        /// Manage Individual Profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Administrator, SuperUser")]
        public ActionResult ManageIndividual(JobseekerProfileModel model)
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
                                Action sendAction = new Action(SendJob);
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
                return RedirectToAction("UpdateProfileL", "Jobseeker", new { type = model.Type, returnurl = model.ReturnUrl });
            }
        }
        //public ActionResult ManageIndividual1(JobseekerProfileModel model)
        //{
        //    var body = string.Empty;
        //    UserProfile original = new UserProfile();
        //    original = MemberService.Instance.Get(model.Id);

        //    body = string.Empty;
        //    if (!string.IsNullOrEmpty(model.BYear))
        //    {
        //        if (!string.IsNullOrEmpty(model.BDay) && !string.IsNullOrEmpty(model.BMonth))
        //        {
        //            model.BirthDate = string.Format("{0}-{1}-{2}", model.BYear, model.BMonth, model.BDay);

        //            int year = Convert.ToInt32(model.BYear);
        //            model.Age = DateTime.Now.Year - year;
        //            int limit = 13;
        //            if (model.Age <= limit)
        //            {
        //                TempData["Error"] = "You must be 13 years of age!";
        //                return View(model);
        //            }
        //        }
        //        else
        //        {
        //            model.BirthDate = model.BYear;
        //            int year = Convert.ToInt32(model.BYear);

        //            model.Age = DateTime.Now.Year - year;
        //            int limit = 13;
        //            if (model.Age <= limit)
        //            {
        //                TempData["Error"] = "You must be 13 years of age!";
        //                return View(model);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        TempData["Error"] = "Please provide birth year!";
        //        return View(model);
        //    }

        //    if (model.CountryId != null && (!string.IsNullOrEmpty(model.Title) || model.CategoryId != null || model.SpecializationId != null || model.SpecializationId != null))
        //    {
        //        if (original.CountryId != null && ((!string.IsNullOrEmpty(original.Title) && !original.Equals(model.Title)) || (original.CategoryId != null && original.CategoryId != model.CategoryId) || (original.SpecializationId != null && original.SpecializationId != model.SpecializationId)))
        //        {
        //            Update(original, model);
        //            var job_list = JobSeekerService.Instance.AutomatchedJobs(original);

        //            if (job_list.Any())
        //            {
        //                var sbbody = new StringBuilder();
        //                var resumeLink = string.Empty;

        //                sbbody.Append("<table>");
        //                sbbody.Append("<tr>");
        //                sbbody.Append("<th>Title</th><th>Last Modified Date</th><th>Number of Matches</th>");
        //                sbbody.Append("</tr>");

        //                var automatchLink =
        //                    string.Format(
        //                        "<a href=\"{0}://{1}/Jobseeker/Index\" target=\"_Blank\">{2}</a> | <a href=\"{0}://{1}/Jobseeker/Index\" target=\"_Blank\">View</a>",
        //                        Request.Url.Scheme, Request.Url.Authority, job_list.Count);
        //                resumeLink = string.Format("<a href=\"{0}://{1}/{2}\" target=\"_Blank\">{3}</a>",
        //                    Request.Url.Scheme, Request.Url.Authority, original.PermaLink, original.Title);

        //                sbbody.Append("<tr>");
        //                sbbody.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td>", resumeLink,
        //                    original.DateUpdated.Value.ToString("MM/dd/yyyy"), automatchLink);
        //                sbbody.Append("</tr>");
        //                sbbody.Append("</table>");

        //                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/jobseeker_automatch.html"));
        //                if (reader != null)
        //                {
        //                    body = reader.ReadToEnd();
        //                    body = body.Replace("@@firstname", original.FirstName);
        //                    body = body.Replace("@@lastname", original.LastName);
        //                    body = body.Replace("@@list", sbbody.ToString());
        //                    string[] receipent = { original.Username };
        //                    var subject = "Job Match List";

        //                    AlertService.Instance.SendMail(subject, receipent, body);
        //                }

        //                jobList = job_list.ToList();
        //                Action sendAction = new Action(SendJob);
        //                Task sendTask = new Task(sendAction, TaskCreationOptions.None);
        //                sendTask.Start();
        //            }
        //        }
        //    }

        //    Update(original, model);
        //    return View(model);
        //}

        /// <summary>
        /// Manage Company Profile
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Administrator, SuperUser")]
        public ActionResult ManageCompany(long Id)
        {
            var employer = MemberService.Instance.Get(Id);
            var model = new EmployerModel();
            if (employer != null)
            {
                model = new EmployerModel()
                {
                    Id = employer.UserId,
                    Company = employer.Company,
                    CategoryId = (employer.CategoryId != null) ? employer.CategoryId.Value : 0,
                    Overview = employer.Summary,
                    FirstName = employer.FirstName,
                    LastName = employer.LastName,
                    Address = employer.Address,
                    CountryId = Convert.ToInt64(employer.CountryId),
                    StateId = Convert.ToInt64(employer.StateId),
                    City = employer.City,
                    Zip = employer.Zip,
                    PhoneCountryCode = employer.PhoneCountryCode,
                    Phone = employer.Phone,
                    MobileCountryCode = employer.MobileCountryCode,
                    Mobile = employer.Mobile,
                    Website = employer.Website,
                    IsFeatured = employer.IsFeatured,
                    PremaLink = employer.PermaLink,
                    Facebook = employer.Facebook,
                    Twitter = employer.Twitter,
                    LinkedIn = employer.LinkedIn,
                    GooglePlus = employer.GooglePlus
                };
            }
            return View(model);
        }

        /// <summary>
        /// Manage Company Profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Administrator, SuperUser")]
        public ActionResult ManageCompany(EmployerModel model)
        {
            if (ModelState.IsValid)
            {
                var original = MemberService.Instance.Get(model.Id);

                original.Company = model.Company.TitleCase();
                original.CategoryId = model.CategoryId;
                string summary = model.Overview;
                if (!string.IsNullOrEmpty(summary))
                {
                    summary = summary.RemoveEmails();
                    summary = summary.RemoveNumbers();
                    summary = summary.RemoveWebsites();
                }
                original.Summary = summary;
                original.FirstName = model.FirstName;
                original.LastName = model.LastName;
                original.Address = model.Address;
                original.CountryId = model.CountryId;

                var states = SharedService.Instance.GetStatesById(Convert.ToInt64(model.CountryId));

                if (states.Count > 0)
                {
                    if (model.StateId == null)
                    {
                        ModelState.AddModelError("", "Please select state.");
                        return View(model);
                    }
                }

                original.StateId = model.StateId;
                original.City = model.City;
                original.Zip = model.Zip;
                original.PhoneCountryCode = model.PhoneCountryCode;
                original.Phone = model.Phone;
                original.MobileCountryCode = model.MobileCountryCode;
                original.Mobile = model.Mobile;
                original.Website = model.Website;
                original.IsFeatured = model.IsFeatured;
                original.Facebook = model.Facebook;
                original.Twitter = model.Twitter;
                original.LinkedIn = model.LinkedIn;
                original.GooglePlus = model.GooglePlus;
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    dataHelper.Update(original, User.Username);
                }
                TempData["UpdateData"] = "Profile has been updated successfully!";

            }

            return RedirectToAction("ManageCompany", new { Id = model.Id });
        }
        [HttpGet]
        [Authorize(Roles = "Administrator, SuperUser")]
        public ActionResult EditJob(long UserId, long Id, string redirectUrl)
        {
            JobListingModel model = new JobListingModel();
            UserProfile profile = MemberService.Instance.Get(UserId);
            Job job = JobService.Instance.Get(Id);
            if (job.IsDeleted)
            {
                TempData["UpdateData"] = "Job has been already Deleted";
                return Redirect(Request.UrlReferrer.ToString());
            }
            ViewBag.UserInfo = profile;
            ViewBag.IsPublished = job.IsPublished;
            ViewBag.IsRejected = job.IsRejected;
            model = new JobListingModel(Id);
            ViewBag.RedirectUrl = redirectUrl;
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator, SuperUser")]
        [ValidateInput(false)]
        public ActionResult EditJob(JobListingModel model, string RedirectUrl)
        {
            Job job = JobService.Instance.Get(model.Id);
            UserProfile profile = MemberService.Instance.Get(job.EmployerId.Value);

            string description = Sanitizer.GetSafeHtmlFragment(model.Description);
            description = description.RemoveNumbers();
            description = description.RemoveEmails();
            description = description.RemoveWebsites();
            string companyname = model.CompanyName;
            string summary = model.Summary;
            summary = summary.RemoveNumbers();
            summary = summary.RemoveEmails();
            summary = summary.RemoveWebsites();

            string requirements = model.Requirements;
            if (!string.IsNullOrEmpty(model.Requirements))
            {
                requirements = requirements.RemoveNumbers();
                requirements = requirements.RemoveEmails();
                requirements = requirements.RemoveWebsites();
            }

            if (job.IsPublished.Value == true && job.InEditMode == false)
            {
                job.CompanyName1 = companyname;
                job.Description = description;
                job.Summary = summary;
                job.Requirements = requirements;

                job.MinimumExperience = (byte?)model.MinimumExperience;
                job.MaximumExperience = (byte?)model.MaximumExperience;
                job.Currency = model.SalaryCurrency;
                job.MinimumSalary = model.MinimumSalary;
                job.MaximumSalary = model.MaximumSalary;
                job.MinimumAge = (byte?)model.MinimumAge;
                job.MaximumAge = (byte?)model.MaximumAge;
                job.EmploymentTypeId = model.EmploymentType;
                job.QualificationId = model.QualificationId;
                //job.InEditMode = true;
                //job.IsPublished = false;
                //job.IsRejected = false;
            }
            else if (job.IsPublished.Value == false && job.InEditMode == true)
            {
                job.CompanyName1 = companyname;
                job.NewDescription = description;
                job.NewSummary = summary;
                job.NewRequirements = summary;
                job.NewMinimumExperience = (byte?)model.MinimumExperience;
                job.NewMaximumExperience = (byte?)model.MaximumExperience;
                job.NewCurrency = model.SalaryCurrency;
                job.NewMinimumSalary = model.MinimumSalary;
                job.NewMaximumSalary = model.MaximumSalary;
                job.NewMinimumAge = (byte?)model.MinimumAge;
                job.NewMaximumAge = (byte?)model.MaximumAge;
                job.NewEmploymentTypeId = model.EmploymentType;
                job.NewQualificationId = model.QualificationId;
                //job.InEditMode = true;
            }
            else if (job.IsPublished.Value == false && job.InEditMode == false)
            {
                job.CompanyName1 = companyname;
                job.Description = description;
                job.Summary = summary;
                job.Requirements = requirements;

                job.MinimumExperience = (byte?)model.MinimumExperience;
                job.MaximumExperience = (byte?)model.MaximumExperience;
                job.Currency = model.SalaryCurrency;
                job.MinimumSalary = model.MinimumSalary;
                job.MaximumSalary = model.MaximumSalary;
                job.MinimumAge = (byte?)model.MinimumAge;
                job.MaximumAge = (byte?)model.MaximumAge;
                job.EmploymentTypeId = model.EmploymentType;
                job.QualificationId = model.QualificationId;
                job.CountryId = model.CountryId;
                job.StateId = model.StateId;
                job.CategoryId = model.CategoryId;
               // job.SpecializationId = model.SpecializationId;
                job.City = model.City;
                job.Zip = model.Zip;
                //job.InEditMode = false;
            }
            job.OptionalSkills = model.Optionalskills;
            job.NoticePeriod = model.Noticeperiod;
            if (job.IsPublished == false && job.IsRejected == false)
            {
                job.Title = model.Title;
                var permalink = model.Title;

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

                job.PermaLink = permalink;
            }
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                dataHelper.Update(job, profile.Username);
            }

            TempData["UpdateData"] = string.Format("Job {0} has been updated successfully.", job.Title);

            ViewBag.UserInfo = profile;
            return Redirect(RedirectUrl);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, SuperUser")]
        public ActionResult ManageJob(long UserId, long Id, string redirectUrl)
        {
            JobListingModel model = new JobListingModel();
            UserProfile profile = MemberService.Instance.Get(UserId);
            Job job = JobService.Instance.Get(Id);
            if (job.IsDeleted)
            {
                TempData["UpdateData"] = "Job has been already Deleted";
                return Redirect(Request.UrlReferrer.ToString());
            }
            ViewBag.UserInfo = profile;
            ViewBag.IsPublished = job.IsPublished;
            ViewBag.IsRejected = job.IsRejected;
            model = new JobListingModel(Id);
            ViewBag.RedirectUrl = redirectUrl;
            return View(model);
        }

        private string ReplaceNumbers(string text)
        {
            string output = Regex.Replace(text, @"\d{5,}", string.Empty);
            return output;
        }

        private string ReplaceEmails(string text)
        {
            string output = Regex.Replace(text, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}", string.Empty);
            return output;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, SuperUser")]
        [ValidateInput(false)]
        public ActionResult ManageJob(JobListingModel model, string RedirectUrl)
        {
            Job job = JobService.Instance.Get(model.Id);
            UserProfile profile = MemberService.Instance.Get(job.EmployerId.Value);

            string description = Sanitizer.GetSafeHtmlFragment(model.Description);
            description = description.RemoveNumbers();
            description = description.RemoveEmails();
            description = description.RemoveWebsites();

            string summary = model.Summary;
            summary = summary.RemoveNumbers();
            summary = summary.RemoveEmails();
            summary = summary.RemoveWebsites();

            string requirements = model.Requirements;
            if (!string.IsNullOrEmpty(model.Requirements))
            {
                requirements = requirements.RemoveNumbers();
                requirements = requirements.RemoveEmails();
                requirements = requirements.RemoveWebsites();
            }

            if (job.IsPublished.Value == true && job.InEditMode == false)
            {
                job.Description = description;
                job.Summary = summary;
                job.Requirements = requirements;

                job.MinimumExperience = (byte?)model.MinimumExperience;
                job.MaximumExperience = (byte?)model.MaximumExperience;
                job.Currency = model.SalaryCurrency;
                job.MinimumSalary = model.MinimumSalary;
                job.MaximumSalary = model.MaximumSalary;
                job.MinimumAge = (byte?)model.MinimumAge;
                job.MaximumAge = (byte?)model.MaximumAge;
                job.EmploymentTypeId = model.EmploymentType;
                job.QualificationId = model.QualificationId;
                //job.InEditMode = true;
                //job.IsPublished = false;
                //job.IsRejected = false;
            }
            else if (job.IsPublished.Value == false && job.InEditMode == true)
            {
                job.NewDescription = description;
                job.NewSummary = summary;
                job.NewRequirements = summary;
                job.NewMinimumExperience = (byte?)model.MinimumExperience;
                job.NewMaximumExperience = (byte?)model.MaximumExperience;
                job.NewCurrency = model.SalaryCurrency;
                job.NewMinimumSalary = model.MinimumSalary;
                job.NewMaximumSalary = model.MaximumSalary;
                job.NewMinimumAge = (byte?)model.MinimumAge;
                job.NewMaximumAge = (byte?)model.MaximumAge;
                job.NewEmploymentTypeId = model.EmploymentType;
                job.NewQualificationId = model.QualificationId;
                //job.InEditMode = true;
            }
            else if (job.IsPublished.Value == false && job.InEditMode == false)
            {
                job.Description = description;
                job.Summary = summary;
                job.Requirements = requirements;

                job.MinimumExperience = (byte?)model.MinimumExperience;
                job.MaximumExperience = (byte?)model.MaximumExperience;
                job.Currency = model.SalaryCurrency;
                job.MinimumSalary = model.MinimumSalary;
                job.MaximumSalary = model.MaximumSalary;
                job.MinimumAge = (byte?)model.MinimumAge;
                job.MaximumAge = (byte?)model.MaximumAge;
                job.EmploymentTypeId = model.EmploymentType;
                job.QualificationId = model.QualificationId;
                job.CountryId = model.CountryId;
                job.StateId = model.StateId;
                job.CategoryId = model.CategoryId;
                job.SpecializationId = model.SpecializationId;
                job.City = model.City;
                job.Zip = model.Zip;
                //job.InEditMode = false;
            }

            if (job.IsPublished == false && job.IsRejected == false)
            {
                job.Title = model.Title;
                var permalink = model.Title;

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

                job.PermaLink = permalink;
            }
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                dataHelper.Update(job, profile.Username);
            }

            TempData["UpdateData"] = string.Format("Job {0} has been updated successfully.", job.Title);

            ViewBag.UserInfo = profile;
            return Redirect(RedirectUrl);
        }
        private void Update(UserProfile original, JobseekerProfileModel model)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                if (ModelState.IsValid)
                {
                    original.CountryId = model.CountryId;
                    original.StateId = model.StateId;
                    original.City = model.City;
                    original.FirstName = model.FirstName.TitleCase();
                    original.LastName = model.LastName.TitleCase();
                    original.DateOfBirth = model.BirthDate;
                    original.Address = model.Address;
                    original.Zip = model.Zip;
                    original.PhoneCountryCode = model.PhoneCountryCode;
                    original.Phone = model.Phone;
                    original.MobileCountryCode = model.MobileCountryCode;
                    original.Mobile = model.Mobile;
                    original.Gender = model.Gender;
                    original.RelationshipStatus = model.RelationshipStatus;
                    if (!string.IsNullOrEmpty(model.Title))
                    {
                        original.Title = model.Title.TitleCase();
                    }
                    string summary = model.Summary;
                    if (!string.IsNullOrEmpty(summary))
                    {
                        summary = summary.RemoveEmails();
                        summary = summary.RemoveNumbers();
                        summary = summary.RemoveWebsites();
                    }
                    original.Summary = summary;
                    original.CategoryId = model.CategoryId;
                    original.SpecializationId = model.SpecializationId;
                    original.DateOfBirth = model.BirthDate;
                    if (!string.IsNullOrEmpty(model.BirthDate))
                    {
                        original.Age = Convert.ToByte(model.Age);
                    }
                    original.CurrentEmployer = model.CurrentEmployer;
                    original.PreviousEmployer = model.PreviousEmployer;

                    if (model.Experience != null)
                    {
                        original.Experience = (byte)model.Experience;
                    }

                    original.ProfessionalExperience = model.ProfessionalExperience;
                    original.DrawingSalary = Convert.ToInt32(model.CurrentSalary);
                    original.ExpectedSalary = Convert.ToInt32(model.ExpectedSalary);
                    original.CurrentCurrency = model.CurrentCurrency;
                    original.ExpectedCurrency = model.ExpectedCurrency;
                    original.QualificationId = model.QualificationId;
                    original.AreaOfExpertise = model.AreaOfExpertise;
                    original.TechnicalSkills = model.TechnicalSkills;
                    original.ManagementSkills = model.ManagementSkills;
                    original.School = model.School;
                    original.University = model.University;
                    original.Education = model.Education;
                    original.Certifications = model.ProfessionalCertification;
                    original.Affiliations = model.ProfessionalAffiliation;
                    original.Website = model.Website;
                    original.Facebook = model.Facebook;
                    original.Twitter = model.Twitter;
                    original.LinkedIn = model.LinkedIn;
                    original.GooglePlus = model.GooglePlus;

                    dataHelper.Update(original, "Self");
                    TempData["SaveData"] = "Profile has been updated successfully.";
                }
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

        public void SendJob()
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
        public ActionResult ResetPassword(long Id)
        {
            var profile = MemberService.Instance.Get(Id);
            ViewBag.UserInfo = profile;
            return View();
        }

        [Authorize]
        public ActionResult LoginHistory(long Id, int pageNumber = 1)
        {
            int pageSize = 15;
            int rows = 0;
            UserProfile profile = MemberService.Instance.Get(Id);
            List<LoginHistoryEntity> loginHistoryList = DomainService.Instance.GetLoginHistory(profile.Username, pageSize, pageNumber);
            if (loginHistoryList.Count > 0)
            {
                rows = loginHistoryList.FirstOrDefault().MaxRows;
            }
            if (profile.Type == (int)SecurityRoles.Jobseeker)
            {
                ViewBag.UserType = "Individual";
            }
            else if (profile.Type == (int)SecurityRoles.Employers)
            {
                ViewBag.UserType = "Company";
            }
            ViewBag.UserInfo = profile;
            ViewBag.Country = SharedService.Instance.GetCountry(profile.CountryId.Value).Text;
            ViewBag.State = string.Empty;
            if (profile.StateId != null)
            {
                List state = SharedService.Instance.GetCountry(profile.StateId.Value);
                ViewBag.State = state.Text;
            }
            ViewBag.City = profile.City;
            ViewBag.Zip = profile.Zip;
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<LoginHistoryEntity>(loginHistoryList, pageNumber, pageSize, rows);
            return View();
        }

        [Authorize]
        // GET: Inbox
        public ActionResult Connections(long UserId, int pageNumber = 0)
        {
            List<Connection> list = new List<Connection>();
            int rows = 0;
            int pageSize = 15;

            UserProfile profile = MemberService.Instance.Get(UserId);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                var result = dataHelper.Get<Connection>().Where(x => x.UserId == UserId);
                rows = result.Count();
                list = result.OrderByDescending(x => x.DateCreated).Skip(pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
            }
            ViewBag.Model = new StaticPagedList<Connection>(list, pageNumber == 0 ? 1 : pageNumber, pageSize, rows);
            ViewBag.Rows = rows;          
            ViewBag.UserInfo = profile;
            if (ViewBag.UserInfo == null)
            {
                TempData["mes"] = "No Connections";
                return RedirectToAction("Blank", "Admin");
            }
            return View();
        }
        public ActionResult Blank()
        {
            ViewBag.mes = TempData["mes"];
            return View();
        }

        [Authorize]
        public ActionResult MessageList(string name, string fd, string fm, string fy, string td, string tm, string ty, int pageNumber = 0)
        {
            UserInfoEntity uinfo = _service.Get(User.Id);

            List<MessagesByCountryModel> list = new List<MessagesByCountryModel>();
            int rows = 0;
            int pageSize = 15;
            string sdate = string.Empty;
            string edate = string.Empty;
            var sdt = new DateTime?();
            var edt = new DateTime?();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<List>().Where(x => x.Name.Equals("Country"));
                if (uinfo.Username == "tasnim@joblisting.com" || uinfo.Username == "lakshmii@joblisting.com")
                {
                    result = dataHelper.Get<List>().Where(x => x.Name.Equals("Country") && x.Id == 106);

                }
                else if (uinfo.Username == "maitrantn@joblisting.com" || uinfo.Username == "Linhv@joblisting.com")
                {
                    result = dataHelper.Get<List>().Where(x => x.Name.Equals("Country") && x.Id == 3343);

                }
                else if (uinfo.Username == "sandhyam@joblisting.com" || uinfo.Username == "Tanyam@joblisting.com")
                {
                    result = dataHelper.Get<List>().Where(x => x.Name.Equals("Country") && x.Id == 139);
                }
                else if (uinfo.Username == "sandhyas@joblisting.com" || uinfo.Username == "balus@joblisting.com")
                {
                    result = dataHelper.Get<List>().Where(x => x.Name.Equals("Country") && x.Id == 205);
                }
                else if (uinfo.Username == "adminIndo@joblisting.com" || uinfo.Username == "RecruiterIndo@joblisting.com")
                {
                    result = dataHelper.Get<List>().Where(x => x.Name.Equals("Country") && x.Id == 107);
                    // countryId = 107;
                }
                else if (uinfo.Username == "adminPhil@joblisting.com" || uinfo.Username == "RecruiterPhil@joblisting.com")
                {
                    result = dataHelper.Get<List>().Where(x => x.Name.Equals("Country") && x.Id == 180);
                    //countryId = 180;
                }

                else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
                {
                    result = dataHelper.Get<List>().Where(x => x.Name.Equals("Country") && x.Id == 40);
                    //countryId = 40;
                }


                else
                { result = dataHelper.Get<List>().Where(x => x.Name.Equals("Country")); }

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

                if (sdt != null && edt != null)
                {
                    list = result.Select(x => new MessagesByCountryModel() { CountryId = x.Id, Country = x.Text, Code = x.Value, Individuals = x.UserCountries.Count(z => z.Type == 4 && z.Communications.Count(y => y.DateCreated.Day >= sdt.Value.Day && y.DateCreated.Month >= sdt.Value.Month && y.DateCreated.Year >= sdt.Value.Year && y.DateCreated.Day <= edt.Value.Day && y.DateCreated.Month <= edt.Value.Month && y.DateCreated.Year <= edt.Value.Year) > 0), Companies = x.UserCountries.Count(z => z.Type == 5 && z.Communications.Count(y => y.DateCreated.Day >= sdt.Value.Day && y.DateCreated.Month >= sdt.Value.Month && y.DateCreated.Year >= sdt.Value.Year && y.DateCreated.Day <= edt.Value.Day && y.DateCreated.Month <= edt.Value.Month && y.DateCreated.Year <= edt.Value.Year) > 0) }).ToList();
                }
                else
                {
                    list = result.Select(x => new MessagesByCountryModel() { CountryId = x.Id, Country = x.Text, Code = x.Value, Individuals = x.UserCountries.Count(z => z.Type == 4 && z.Communications.Count() > 0), Companies = x.UserCountries.Count(z => z.Type == 5 && z.Communications.Count() > 0) }).ToList();
                }

                list = list.Where(x => (x.Individuals > 0 || x.Companies > 0)).ToList();
                if (!string.IsNullOrEmpty(name))
                {
                    list = list.Where(x => x.Country.ToLower().Contains(name.ToLower())).ToList();
                }

                rows = list.Count();
                ViewBag.Individuals = list.Sum(x => x.Individuals);
                ViewBag.Companies = list.Sum(x => x.Companies);

                list = list.OrderBy(x => x.Country).Skip(pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList<MessagesByCountryModel>();
            }
            ViewBag.Model = new StaticPagedList<MessagesByCountryModel>(list, pageNumber == 0 ? 1 : pageNumber, pageSize, rows);
            ViewBag.Rows = rows;
            return View();
        }

        [Authorize]
        public ActionResult CountryMessageList(long countryId, int? type, string name, string fd, string fm, string fy, string td, string tm, string ty, int pageNumber = 0)
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

            List country = SharedService.Instance.GetCountry(countryId);

            List<UserProfile> list = new List<UserProfile>();
            int rows = 0;
            int pageSize = 10;
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

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<UserProfile>().Where(x => x.CountryId == countryId);

                if (sdt != null && edt != null)
                {
                    result = result.Where(x => x.Communications.Count(y => y.DateCreated.Day >= sdt.Value.Day && y.DateCreated.Month >= sdt.Value.Month && y.DateCreated.Year >= sdt.Value.Year && y.DateCreated.Day <= edt.Value.Day && y.DateCreated.Month <= edt.Value.Month && y.DateCreated.Year <= edt.Value.Year) > 0);
                }
                else
                {
                    result = result.Where(x => x.Communications.Count() > 0);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    result = result.Where(x => (x.Company != null ? x.Company : x.FirstName + " " + x.LastName).ToLower().Contains(name.ToLower()));
                }

                if (type != null)
                {
                    result = result.Where(x => x.Type == type);
                }

                list = result.OrderBy(x => x.DateCreated).Skip(pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
            }
            ViewBag.Model = new StaticPagedList<UserProfile>(list, pageNumber == 0 ? 1 : pageNumber, pageSize, rows);
            ViewBag.Rows = rows;
            ViewBag.Country = country;
            return View();
        }

        [Authorize]
        public ActionResult Messages(long Id, string name, string fd, string fm, string fy, string td, string tm, string ty, int pageNumber = 0)
        {
            List<Connection> list = new List<Connection>();
            int rows = 0;
            int pageSize = 15;
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
            UserProfile profile = new UserProfile();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                profile = dataHelper.GetSingle<UserProfile>("UserId", Id);
                list = profile.Connections.ToList();
                if (sdt != null && edt != null)
                {
                    list = list.Where(y => y.DateCreated.Day >= sdt.Value.Day && y.DateCreated.Month >= sdt.Value.Month && y.DateCreated.Year >= sdt.Value.Year && y.DateCreated.Day <= edt.Value.Day && y.DateCreated.Month <= edt.Value.Month && y.DateCreated.Year <= edt.Value.Year).ToList();
                }
                if (!string.IsNullOrEmpty(name))
                {
                    list = list.Where(x => !string.IsNullOrEmpty(MemberService.Instance.Get(x.EmailAddress) != null && MemberService.Instance.Get(x.EmailAddress).Company != null ? MemberService.Instance.Get(x.EmailAddress).Company : MemberService.Instance.Get(x.EmailAddress).FirstName + " " + MemberService.Instance.Get(x.EmailAddress).LastName) && (MemberService.Instance.Get(x.EmailAddress) != null && MemberService.Instance.Get(x.EmailAddress).Company != null ? MemberService.Instance.Get(x.EmailAddress).Company : MemberService.Instance.Get(x.EmailAddress).FirstName + " " + MemberService.Instance.Get(x.EmailAddress).LastName).ToLower().Contains(name.ToLower())).ToList();
                }
                rows = list.Count();
                list = list.OrderByDescending(x => x.DateCreated).Skip(pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
            }
            ViewBag.Model = new StaticPagedList<Connection>(list, pageNumber == 0 ? 1 : pageNumber, pageSize, rows);
            ViewBag.Rows = rows;
            ViewBag.UserInfo = profile;
            return View();
        }

        [Authorize]
        public ActionResult ListMessages(long SenderId, long UserId)
        {
            UserProfile UserInfo = MemberService.Instance.Get(UserId);
            Communication last = new Communication();
            List<Communication> msg_list_new = new List<Communication>();
            List<Communication> msg_list = new List<Communication>();
            UserProfile sender = MemberService.Instance.Get(SenderId);

            BlockedPeople entity = ConnectionHelper.GetBlockedEntity(sender.UserId, UserInfo.UserId);
            if (entity != null && !entity.CreatedBy.Equals(UserInfo.Username))
            {
                TempData["Error"] = string.Format("{0} does not accept messages!", (!string.IsNullOrEmpty(sender.Company) ? sender.Company : string.Format("{0} {1}", sender.FirstName, sender.LastName)));
            }
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                int counts = dataHelper.Get<Communication>().Count(x => x.UserId == UserInfo.UserId && (x.SenderId == SenderId || x.ReceiverId == SenderId) && x.IsDeleted == false);
                if (counts == 0)
                {
                    msg_list = dataHelper.Get<Communication>().Where(x => x.UserId == UserInfo.UserId && (x.SenderId == SenderId || x.ReceiverId == SenderId) && x.IsDeleted == true).OrderBy(x => x.Id).ToList();
                }
                else
                {
                    msg_list = dataHelper.Get<Communication>().Where(x => x.UserId == UserInfo.UserId && (x.SenderId == SenderId || x.ReceiverId == SenderId) && x.IsDeleted == true).OrderBy(x => x.Id).ToList();
                    msg_list_new = dataHelper.Get<Communication>().Where(x => x.UserId == UserInfo.UserId && (x.SenderId == SenderId || x.ReceiverId == SenderId) && x.IsDeleted == false).OrderBy(x => x.Id).ToList();
                }
            }
            ViewBag.ReceiverId = SenderId;
            ViewBag.MessageList = msg_list;
            ViewBag.MessageList_New = msg_list_new;
            ViewBag.UserInfo = UserInfo;
            return View(last);
        }


        //[Authorize]
        //public ActionResult Messages(long Id, string message, string fd, string fm, string fy, string td, string tm, string ty, int pageNumber = 0)
        //{
        //    List<Communication> list = new List<Communication>();
        //    int rows = 0;
        //    int pageSize = 15;
        //    string sdate = string.Empty;
        //    string edate = string.Empty;
        //    var sdt = new DateTime?();
        //    var edt = new DateTime?();

        //    if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy))
        //    {
        //        sdate = string.Format("{0}/{1}/{2}", fm, fd, fy);
        //        sdt = Convert.ToDateTime(sdate);
        //    }

        //    if (!string.IsNullOrEmpty(td) && !string.IsNullOrEmpty(tm) && !string.IsNullOrEmpty(ty))
        //    {
        //        if (string.IsNullOrEmpty(fd) && string.IsNullOrEmpty(fm) && string.IsNullOrEmpty(fy))
        //        {
        //            sdt = DateTime.Now;
        //        }
        //        edate = string.Format("{0}/{1}/{2}", tm, td, ty);
        //        edt = Convert.ToDateTime(edate);
        //    }
        //    else
        //    {
        //        if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy))
        //        {
        //            edt = DateTime.Now;
        //        }
        //    }
        //    UserProfile profile = new UserProfile();
        //    using (JobPortalEntities context = new JobPortalEntities())
        //    {
        //        DataHelper dataHelper = new DataHelper(context);
        //        profile = dataHelper.GetSingle<UserProfile>("UserId",Id);
        //        list = profile.Communications.ToList();
        //        if (sdt != null && edt != null)
        //        {
        //            list = list.Where(y => y.DateCreated.Day >= sdt.Value.Day && y.DateCreated.Month >= sdt.Value.Month && y.DateCreated.Year >= sdt.Value.Year && y.DateCreated.Day <= edt.Value.Day && y.DateCreated.Month <= edt.Value.Month && y.DateCreated.Year <= edt.Value.Year).ToList();
        //        }
        //        if (!string.IsNullOrEmpty(message))
        //        {
        //            list = list.Where(x => x.Message.ToLower().Contains(message.ToLower())).ToList();
        //        }
        //        rows = list.Count();
        //        list = list.OrderByDescending(x => x.DateCreated).Skip(pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
        //    }
        //    ViewBag.Model = new StaticPagedList<Communication>(list, pageNumber == 0 ? 1 : pageNumber, pageSize, rows);
        //    ViewBag.Rows = rows;
        //    ViewBag.UserInfo = profile;
        //    return View();
        //}

        public ActionResult Applications(long? Id, long? UserId, long? countryId, string JobTitle = null, string Company = null, DateTime? StartDate = null, DateTime? EndDate = null, int pageNumber = 1)
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

            int pageSize = 10;
            int rows = 0;
            List<ApplicationEntity> list = DomainService.Instance.GetJobAppList(Id, UserId, countryId, JobTitle, Company, StartDate, EndDate, pageSize, pageNumber);
            if (list.Count > 0)
            {
                rows = list.FirstOrDefault().MaxRows;

                ViewBag.JobList = new SelectList(list.Select(a => a.Title).Distinct().ToList());
                ViewBag.CompanyList = new SelectList(list.Select(a => a.Company).Distinct().ToList());
            }
            //else
            //{
            //    TempData["mes"] = "No Applications";
            //    return RedirectToAction("Blank", "Admin");
            //}
            ViewBag.Model = new StaticPagedList<ApplicationEntity>(list, pageNumber, pageSize, rows);
            ViewBag.Rows = rows;
            if (UserId != null)
            {
                ViewBag.UserId = UserId;
            }
           
            

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="JobTitle"></param>
        /// <param name="Status"></param>
        /// <param name="CountryId"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Interviews(long? Id, long? UserId, string JobTitle, int? Status, long? CountryId, string StartDate, string EndDate, int pageNumber = 0)
        {
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

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
            {
                CountryId = 40;
            }

            var job = new Job();
            if (Id != null)
            {
                job = JobService.Instance.Get(Id.Value);
            }
            var profile = new UserProfile();
            if (UserId == null)
            {
                profile = MemberService.Instance.Get(job.EmployerId.Value);
            }
            else
            {
                profile = MemberService.Instance.Get(UserId.Value);
            }

            var list = new List<Interview>();
            int rows = 0;
            int pageSize = 10;
            if (profile != null)
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    IQueryable<Interview> result = null;
                    SecurityRoles type = (SecurityRoles)profile.Type;
                    switch (type)
                    {
                        case SecurityRoles.Employers:
                            result = dataHelper.Get<Interview>().Where(x => x.UserId == profile.UserId && x.IsDeleted == false);
                            break;
                        case SecurityRoles.Jobseeker:
                            result = dataHelper.Get<Interview>().Where(x => x.Tracking.JobseekerId == profile.UserId && x.IsDeleted == false);
                            break;
                    }

                    if (Status != null)
                    {
                        result = result.Where(x => x.Status == Status.Value);
                    }

                    if (!string.IsNullOrEmpty(JobTitle))
                    {
                        result = result.Where(x => x.Tracking.Job != null && x.Tracking.Job.Title.ToLower().Contains(JobTitle.ToLower()));
                    }

                    if (CountryId != null)
                    {
                        result = result.Where(x =>
                                    (x.Tracking.Job != null && x.Tracking.Job.CountryId == CountryId.Value) ||
                                    (x.Tracking.Jobseeker != null && x.Tracking.Jobseeker.CountryId == CountryId.Value));
                    }

                    list = result.ToList();

                    if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
                    {
                        var sdt = Convert.ToDateTime(StartDate);
                        var edt = Convert.ToDateTime(EndDate);
                        list = list.Where(x => x.DateUpdated.Value.Date >= sdt.Date && x.DateUpdated.Value.Date <= edt.Date).ToList();
                    }

                    ViewBag.JobList = new SelectList(list.Where(x => x.Tracking.JobId != null).Select(a => a.Tracking.Job.Title).Distinct().ToList());
                }
            }

            ViewBag.CountryList = new SelectList(SharedService.Instance.GetCountryList(), "Id", "Text");
            rows = list.Count;
            ViewBag.UserInfo = profile;
            ViewBag.Rows = rows;
            list = list.OrderByDescending(x => x.DateUpdated).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize)).Take(pageSize).ToList();
            ViewBag.Model = new StaticPagedList<Interview>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
            return View();
        }

        public ActionResult Interview(long Id)
        {
            var model = new Interview();

            UserProfile profile = null;

            if (User != null)
            {
                Interview iView = InterviewService.Instance.Get(Id);
                Tracking track = TrackingService.Instance.Get(iView.TrackingId);
                if (track.JobId != null)
                {
                    Job job = JobService.Instance.Get(track.JobId.Value);
                    profile = MemberService.Instance.Get(job.EmployerId.Value);
                }
                else
                {
                    profile = MemberService.Instance.Get(track.UserId);
                }
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    model = dataHelper.GetSingle<Interview>(Id);

                    if (profile.Type == (int)SecurityRoles.Jobseeker)
                    {
                        InterviewService.Instance.Connect(model.UserProfile.Username, User.Username);
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
            }
            ViewBag.UserInfo = profile;
            return View(model);
        }

        /// <summary>
        /// Withdraw interview
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Reason"></param>
        /// <returns></returns>
        public ActionResult WithdrawnInterview(long Id, string Reason, string redirect)
        {
            Interview interview = InterviewService.Instance.Get(Id);
            Tracking track = TrackingService.Instance.Get(interview.TrackingId);
            UserProfile jobSeeker = MemberService.Instance.Get(track.JobseekerId.Value);
            UserProfile employer = MemberService.Instance.Get(interview.UserId);

            interview.Status = (int)InterviewStatus.WITHDRAW;
            interview.DateUpdated = DateTime.Now;
            interview.UpdatedBy = User.Username;

            Activity employerActivity = new Activity()
            {
                Type = (int)ActivityTypes.INTERVIEW_WITHDRAWN,
                Comments = string.Format("{0}<br><br><a href=\"/Interview/Update?Id={1}\">CLICK HERE</a> to see the interview status!", Reason, interview.Id),
                ActivityDate = DateTime.Now,
                UserId = employer.UserId,
                DateUpdated = DateTime.Now,
                UpdatedBy = User.Username,
                Unread = true
            };
            MemberService.Instance.Track(employerActivity);

            Activity jobseekerActivity = new Activity()
            {
                Type = (int)ActivityTypes.INTERVIEW_WITHDRAWN,
                Comments = string.Format("{0}<br><br><a href=\"/Interview/Update?Id={1}\">CLICK HERE</a> to see the interview status!", Reason, interview.Id),
                ActivityDate = DateTime.Now,
                UserId = jobSeeker.UserId,
                DateUpdated = DateTime.Now,
                UpdatedBy = User.Username,
                Unread = true
            };
            MemberService.Instance.Track(jobseekerActivity);

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                dataHelper.UpdateEntity(interview);

                FollowUp followUp = new FollowUp()
                {
                    InterviewId = interview.Id,
                    Status = (int)FeedbackStatus.WITHDRAW,
                    NewDate = interview.InterviewDate,
                    NewTime = interview.InterviewDate.ToShortTimeString(),
                    Unread = true,
                    UserId = employer.UserId,
                    Message = "Withdrawan from Interview",
                    DateUpdated = DateTime.Now
                };
                dataHelper.AddEntity<FollowUp>(followUp, User.Username);

                dataHelper.Save();
            }

            var subject = "Interview Withdrawn!";
            string[] receipent = { string.Empty };
            using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/interview_on_withdrawal_employer.html")))
            {
                string body = reader.ReadToEnd();

                body = body.Replace("@@employer", employer.Company);
                body = body.Replace("@@url", string.Format("{0}://{1}/Inbox/Show?Id={2}", Request.Url.Scheme, Request.Url.Authority, employerActivity.Id));

                receipent[0] = employer.Username;
                AlertService.Instance.SendMail(subject, receipent, body);
            }

            using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/interview_on_withdrawal_jobseeker.html")))
            {
                string body = reader.ReadToEnd();
                body = body.Replace("@@firstname", jobSeeker.FirstName);
                body = body.Replace("@@lastname", jobSeeker.LastName);
                body = body.Replace("@@url", string.Format("{0}://{1}/Inbox/Show?Id={2}", Request.Url.Scheme, Request.Url.Authority, jobseekerActivity.Id));

                receipent[0] = jobSeeker.Username;
                AlertService.Instance.SendMail(subject, receipent, body);
            }

            return Redirect(redirect);
        }

        /// <summary>
        /// Withdrawn from application
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Reason"></param>
        /// <returns></returns>
        public ActionResult WithdrawnApplication(Guid Id, string Reason, string redirect)
        {
            Tracking track = TrackingService.Instance.Get(Id);
            Job job = JobService.Instance.Get(track.JobId.Value);
            UserProfile jobSeeker = MemberService.Instance.Get(track.JobseekerId.Value);
            UserProfile employer = MemberService.Instance.Get(job.EmployerId.Value);

            string msg = string.Empty;
            Tracking withdrawal = TrackingService.Instance.Update(TrackingTypes.WITHDRAWN, Id, User.Username, out msg);
            if (withdrawal != null)
            {
                Activity employerActivity = new Activity()
                {
                    Type = (int)ActivityTypes.APPLICATION_WITHDRAWN,
                    Comments = string.Format("{0}<br><br><a href=\"/applications\">CLICK HERE</a> to see the application status!", Reason),
                    ActivityDate = DateTime.Now,
                    UserId = employer.UserId,
                    DateUpdated = DateTime.Now,
                    UpdatedBy = User.Username,
                    Unread = true
                };
                MemberService.Instance.Track(employerActivity);

                Activity jobseekerActivity = new Activity()
                {
                    Type = (int)ActivityTypes.APPLICATION_WITHDRAWN,
                    Comments = string.Format("{0}<br><br><a href=\"/jobseeker/index\">CLICK HERE</a> to see the application status!", Reason),
                    ActivityDate = DateTime.Now,
                    UserId = jobSeeker.UserId,
                    DateUpdated = DateTime.Now,
                    UpdatedBy = User.Username,
                    Unread = true
                };
                MemberService.Instance.Track(jobseekerActivity);

                var subject = string.Format("Application for {0} has beed Withdrawn!", job.Title);
                string[] receipent = { string.Empty };
                using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/application_on_withdrawal_employer.html")))
                {
                    string body = reader.ReadToEnd();

                    body = body.Replace("@@employer", employer.Company);
                    body = body.Replace("@@jobtitle", job.Title);
                    body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                    body = body.Replace("@@url", string.Format("{0}://{1}/Inbox/Show?Id={2}", Request.Url.Scheme, Request.Url.Authority, employerActivity.Id));

                    receipent[0] = employer.Username;
                    AlertService.Instance.SendMail(subject, receipent, body);
                }

                using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/application_on_withdrawal_jobseeker.html")))
                {
                    string body = reader.ReadToEnd();
                    body = body.Replace("@@firstname", jobSeeker.FirstName);
                    body = body.Replace("@@lastname", jobSeeker.LastName);
                    body = body.Replace("@@jobtitle", job.Title);
                    body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));
                    body = body.Replace("@@url", string.Format("{0}://{1}/Inbox/Show?Id={2}", Request.Url.Scheme, Request.Url.Authority, jobseekerActivity.Id));

                    receipent[0] = jobSeeker.Username;
                    AlertService.Instance.SendMail(subject, receipent, body);
                }
            }
            return Redirect(redirect);
        }

        /// <summary>
        /// Delete application
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Reason"></param>
        /// <returns></returns>
        public ActionResult DeleteApplication(Guid Id, string Reason, string redirect)
        {
            Tracking track = TrackingService.Instance.Get(Id);
            Job job = JobService.Instance.Get(track.JobId.Value);
            UserProfile jobSeeker = MemberService.Instance.Get(track.JobseekerId.Value);
            UserProfile employer = MemberService.Instance.Get(track.UserId);

            string msg = string.Empty;

            Tracking deletedTrack = TrackingService.Instance.Update(TrackingTypes.DELETED, Id, User.Username, out msg);
            if (deletedTrack != null)
            {
                Activity activity = new Activity()
                {
                    Type = (int)ActivityTypes.APPLICATION_DELETED,
                    Comments = string.Format("{0}", Reason),
                    ActivityDate = DateTime.Now,
                    UserId = employer.UserId,
                    DateUpdated = DateTime.Now,
                    UpdatedBy = User.Username,
                    Unread = true
                };

                MemberService.Instance.Track(activity);

                activity = new Activity()
                {
                    Type = (int)ActivityTypes.APPLICATION_DELETED,
                    Comments = string.Format("{0}", Reason),
                    ActivityDate = DateTime.Now,
                    UserId = jobSeeker.UserId,
                    DateUpdated = DateTime.Now,
                    UpdatedBy = User.Username,
                    Unread = true
                };

                MemberService.Instance.Track(activity);

                var subject = string.Format("Application for {0} has beed Deleted!", job.Title);
                string[] receipent = { string.Empty };
                using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/application_on_delete_employer.html")))
                {
                    string body = reader.ReadToEnd();

                    body = body.Replace("@@employer", employer.Company);
                    body = body.Replace("@@jobtitle", job.Title);
                    body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));

                    receipent[0] = employer.Username;
                    AlertService.Instance.SendMail(subject, receipent, body);
                }

                using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/application_on_delete_jobseeker.html")))
                {
                    string body = reader.ReadToEnd();
                    body = body.Replace("@@firstname", jobSeeker.FirstName);
                    body = body.Replace("@@lastname", jobSeeker.LastName);
                    body = body.Replace("@@jobtitle", job.Title);
                    body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));

                    receipent[0] = jobSeeker.Username;
                    AlertService.Instance.SendMail(subject, receipent, body);
                }
            }

            return Redirect(redirect);
        }

        public ActionResult JobViewed(long Id, int pageNumber = 0)
        {
            var job = JobService.Instance.Get(Id);
            var profile = MemberService.Instance.Get(job.EmployerId.Value);
            List<Tracking> list = new List<Tracking>();
            var TypeList = new List<int>();
            ViewBag.JobList = new SelectList(new List<string>());
            ViewBag.CompanyList = new SelectList(new List<string>());

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                TypeList.Add((int)TrackingTypes.VIEWED);

                IQueryable<Tracking> result = dataHelper.Get<Tracking>().Where(x => x.JobId == Id && TypeList.Contains(x.Type));

                result = result.Where(x => x.Jobseeker != null && x.Job.EmployerId == profile.UserId && x.IsDeleted == false);

                //if (!string.IsNullOrEmpty(JobTitle))
                //{
                //    result = result.Where(x => x.Job != null && x.Job.Title.ToLower() == JobTitle.ToLower());
                //}

                //if (!string.IsNullOrEmpty(Company))
                //{
                //    result = result.Where(x => x.Job != null && x.Job.Employer.Company.ToLower() == Company.ToLower());
                //}

                //if (StartDate != null)
                //{
                //    result = result.Where(x => x.DateUpdated.Day >= StartDate.Value.Day && x.DateUpdated.Month >= StartDate.Value.Month && x.DateUpdated.Year >= StartDate.Value.Year);
                //}

                //if (EndDate != null)
                //{
                //    if (StartDate == null)
                //    {
                //        result = result.Where(x => x.DateUpdated.Day >= DateTime.Now.Day && x.DateUpdated.Month >= DateTime.Now.Month && x.DateUpdated.Year >= DateTime.Now.Year);
                //    }
                //    result = result.Where(x => x.DateUpdated.Day <= EndDate.Value.Day && x.DateUpdated.Month <= EndDate.Value.Month && x.DateUpdated.Year <= EndDate.Value.Year);
                //}
                //else
                //{
                //    if (StartDate != null)
                //    {
                //        result = result.Where(x => x.DateUpdated.Day <= DateTime.Now.Day && x.DateUpdated.Month <= DateTime.Now.Month && x.DateUpdated.Year <= DateTime.Now.Year);
                //    }
                //}

                //if (result.Count() > 0)
                //{
                //    ViewBag.JobList = new SelectList(result.Where(x => x.Job != null).Select(a => a.Job.Title).Distinct().ToList());
                //}
                //var companyList = result.Where(x => x.Job != null && x.Job.Employer != null).Select(x => x.Job.Employer.Company != null ? x.Job.Employer.Company : "").Distinct().ToList();
                //if (companyList.Count > 0)
                //{
                //    ViewBag.CompanyList = new SelectList(companyList);
                //}

                int rows = result.Count();
                ViewBag.Rows = rows;
                if (result.Count() > 0)
                {
                    result = result.OrderByDescending(x => x.DateUpdated).Skip((pageNumber > 0 ? (pageNumber - 1) * 10 : pageNumber * 10)).Take(10);
                }
                ViewBag.Model = new StaticPagedList<Tracking>(result.ToList(), (pageNumber == 0 ? 1 : pageNumber), 10, rows);
            }
            ViewBag.UserInfo = profile;
            ViewBag.Job = job;
            return View();
        }

        [Authorize]
        public ActionResult GlobalInbox()
        {
            return View();
        }

        [Authorize]
        public ActionResult InboxList(long CountryId, string fd, string fm, string fy, string td, string tm, string ty, int type = 0, int pageNumber = 1)
        {
            return View();
        }

        [Authorize]
        // GET: Inbox
        public ActionResult AdminInbox(long Id, bool? unread = null, int pageNumber = 1)
        {
            List<Inbox> list = new List<Inbox>();
            int rows = 0;
            int pageSize = 15;

            list = DomainService.Instance.ListInbox(Id, null, unread, pageNumber);
            if (list.Count > 0)
            {
                rows = list.FirstOrDefault().MaxRows.Value;
            }
            ViewBag.Model = new StaticPagedList<Inbox>(list, pageNumber, pageSize, rows);
            ViewBag.Rows = rows;
            ViewBag.UserInfo = MemberService.Instance.Get(Id);
            return View();
        }


        [Authorize]
        // GET: Inbox
        public ActionResult Inbox(long Id, bool? unread = null, int pageNumber = 1)
        {
            List<Inbox> list = new List<Inbox>();
            int rows = 0;
            int pageSize = 15;

            list = DomainService.Instance.ListInbox(Id, null, unread, pageNumber);
            if (list.Count > 0)
            {
                rows = list.FirstOrDefault().MaxRows.Value;
            }
            ViewBag.Model = new StaticPagedList<Inbox>(list, pageNumber, pageSize, rows);
            ViewBag.Rows = rows;
            ViewBag.UserInfo = MemberService.Instance.Get(Id);
            if(ViewBag.UserInfo==null)
            {
                TempData["mes"] = "No Messages";
                return RedirectToAction("Blank", "Admin");
            }
            return View();
        }

        [Authorize]
        public ActionResult Show(long Id, long ItemId)
        {
            Inbox inbox = DomainService.Instance.GetInboxItem(ItemId);
            inbox.ChildInbox = DomainService.Instance.ListChildInbox(ItemId);
            ViewBag.UserInfo = MemberService.Instance.Get(Id);
            return View(inbox);
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
                ReceiverId = (User.Id == inbox.ReceiverId) ? inbox.SenderId : inbox.ReceiverId,
                SenderId = User.Id,
                ReferenceId = inbox.ReferenceId,
                ReferenceType = inbox.ReferenceType,
                ParentId = inbox.Id,
                Unread = true
            };
            long inboxId = DomainService.Instance.ManageInbox(ibox);

            UserProfile sender = MemberService.Instance.Get(ibox.ReceiverId.Value);
            var subject = "Message from Joblisting Support Team";
            string[] receipent = { string.Empty };
            using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/inbox_message.html")))
            {
                string body = reader.ReadToEnd();

                body = body.Replace("@@sender", "Joblisting Support Team");
                body = body.Replace("@@receiver", inbox.SenderName);
                body = body.Replace("@@url", string.Format("{0}://{1}/inbox/show?id={2}", Request.Url.Scheme, Request.Url.Authority, (inbox.Id != null ? inbox.Id : inboxId)));

                receipent[0] = sender.Username;
                AlertService.Instance.SendMail(subject, receipent, body);
            }
            TempData["UpdateData"] = "Message sent successfully!";
            return Json(inboxId, JsonRequestBehavior.AllowGet);
        }

        //[Authorize]
        //[HttpPost]
        //public ActionResult SendMessage(Inbox model)
        //{
        //    Activity activity = new Activity();
        //    UserProfile profile = MemberService.Instance.Get(User.Username);
        //    using (JobPortalEntities context = new JobPortalEntities())
        //    {
        //        DataHelper dataHelper = new DataHelper(context);
        //        activity = dataHelper.GetSingle<Activity>(model.Id);

        //        Job job = JobService.Instance.Get(activity.ReferenceId.Value);
        //        if(job!=null)
        //        {
        //            Activity newActivity = new Activity()
        //            {
        //                Subject = string.Format("{0} job listed", job.Title),
        //                Comments = model.Message,
        //                ActivityDate = DateTime.Now,
        //                UserId = activity.UserId,
        //                DateUpdated = DateTime.Now,
        //                UpdatedBy = User.Username,
        //                Type = activity.Type,
        //                ReferenceId = model.Id,
        //                ParentId = activity.Id,
        //                Unread = true
        //            };

        //            MemberService.Instance.Track(newActivity);
        //        }
        //        else
        //        {
        //            Activity newActivity = new Activity()
        //            {
        //                Subject = ((ActivityTypes)activity.Type).GetDescription().TitleCase(),
        //                Comments = model.Message,
        //                ActivityDate = DateTime.Now,
        //                UserId = activity.UserId,
        //                DateUpdated = DateTime.Now,
        //                UpdatedBy = User.Username,
        //                Type = activity.Type,
        //                ReferenceId = model.Id,
        //                ParentId = activity.Id,
        //                Unread = true
        //            };
        //            MemberService.Instance.Track(newActivity);
        //        }
        //    }
        //    return RedirectToAction("Show", new { Id = profile.UserId, ItemId = model.Id});
        //}

        /// <summary>
        /// Updates tracking record
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Id"></param>
        /// <param name="Username"></param>
        /// <param name="message"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'TrackingTypes' could not be found (are you missing a using directive or an assembly reference?)
        public Tracking UpdateTracking(DataHelper dataHelper, TrackingTypes Type, Guid Id, string Username)
#pragma warning restore CS0246 // The type or namespace name 'TrackingTypes' could not be found (are you missing a using directive or an assembly reference?)
        {
            var type = (int)Type;
            Tracking tracking = null;

            var profile = dataHelper.GetSingle<UserProfile>("Username", Username);
            var original = dataHelper.GetSingle<Tracking>(Id);
            if (original != null)
            {
                var parameters = new Hashtable();
                if (original.Jobseeker != null && original.Resume == null)
                {
                    if (original.Job != null)
                    {
                        parameters.Add("JobId", original.JobId);
                    }
                    parameters.Add("JobseekerId", original.JobseekerId);
                    parameters.Add("Type", type);
                    parameters.Add("UserId", profile.UserId);
                    parameters.Add("IsDeleted", false);
                }
                else if (original.Jobseeker == null && original.Resume != null)
                {
                    if (original.Job != null)
                    {
                        parameters.Add("JobId", original.JobId);
                    }
                    parameters.Add("ResumeId", original.ResumeId);
                    parameters.Add("Type", type);
                    parameters.Add("UserId", profile.UserId);
                    parameters.Add("IsDeleted", false);
                }
                tracking = dataHelper.GetSingle<Tracking>(parameters);
                if (tracking == null)
                {
                    tracking = new Tracking
                    {
                        Id = Guid.NewGuid(),
                        JobId = original.JobId,
                        ResumeId = original.ResumeId,
                        JobseekerId = original.JobseekerId,
                        Type = type,
                        DateUpdated = DateTime.Now,
                        UserId = profile.UserId,
                        IsDownloaded = false
                    };

                    dataHelper.AddEntity<Tracking>(tracking, Username);
                    if (Type != TrackingTypes.VIEWED && Type != TrackingTypes.DOWNLOADED)
                    {
                        TrackingDetail trackingDetails = new TrackingDetail();
                        if (original.Jobseeker != null && original.Resume == null)
                        {
                            trackingDetails = new TrackingDetail
                            {
                                Id = tracking.Id,
                                Title = original.TrackingDetail != null ? original.TrackingDetail.Title : original.Jobseeker.Title,
                                CategoryId = original.TrackingDetail != null ? original.TrackingDetail.CategoryId : original.Jobseeker.CategoryId.Value,
                                SpecializationId = original.TrackingDetail != null ? original.TrackingDetail.SpecializationId : original.Jobseeker.SpecializationId.Value,
                                CountryId = original.TrackingDetail != null ? original.TrackingDetail.CountryId : original.Jobseeker.CountryId.Value,
                                StateId = original.TrackingDetail != null ? original.TrackingDetail.StateId : original.Jobseeker.StateId,
                                FileName = original.TrackingDetail != null ? original.TrackingDetail.FileName : original.Jobseeker.FileName,
                                Content = original.TrackingDetail != null ? original.TrackingDetail.Content : original.Jobseeker.Content
                            };
                        }
                        else if (original.Jobseeker == null && original.Resume != null)
                        {
                            trackingDetails = new TrackingDetail
                            {
                                Id = tracking.Id,
                                Title = original.TrackingDetail != null ? original.TrackingDetail.Title : original.Resume.Title,
                                CategoryId = original.TrackingDetail != null ? original.TrackingDetail.CategoryId : original.Resume.CategoryId.Value,
                                SpecializationId = original.TrackingDetail != null ? original.TrackingDetail.SpecializationId : original.Resume.SpecializationId.Value,
                                CountryId = original.TrackingDetail != null ? original.TrackingDetail.CountryId : original.Resume.CountryId.Value,
                                StateId = original.TrackingDetail != null ? original.TrackingDetail.StateId : original.Resume.StateId,
                                FileName = original.TrackingDetail != null ? original.TrackingDetail.FileName : original.Resume.FileName,
                                Content = original.TrackingDetail != null ? original.TrackingDetail.Content : original.Resume.Content
                            };
                        }
                        dataHelper.AddEntity(trackingDetails);
                    }
                    dataHelper.DeleteUpdate(original);
                }
            }
            return tracking;
        }

        [Authorize]
        public ActionResult DownloadHistory(long Id, string ResumeTitle, long? CountryId, string StartDate, string EndDate, int pageNumber = 0)
        {
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

            else if (uinfo.Username == "adminCom@joblisting.com" || uinfo.Username == "RecruiterCom@joblisting.com" || uinfo.Username == "recruiter@joblisting.com"||uinfo.Username == "chetan@joblisting.com" || uinfo.Username == "vani123@accuracy.com.sg" || uinfo.Username == "tasnim@accuracy.com.sg" || uinfo.Username == "ganeshr@joblisting.com" || uinfo.Username == "deepti@accuracy.com.sg" || uinfo.Username == "doli.chauhan123@accuracy.com.sg" || uinfo.Username == "sandhya@accuracy.com.sg" || uinfo.Username == "shreyag1234@accuracy.com.sg" || uinfo.Username == "pallavi123@accuracy.com.sg" || uinfo.Username == "druthi@accuracy.com.sg" || uinfo.Username == "baba@accuracy.com.sg" || uinfo.Username == "anshikagupta@accuracy.com.sg" || uinfo.Username == "dianna@accuracy.com.sg" || uinfo.Username == "haris@joblisting.com" || uinfo.Username == "anilkumar@joblisting.com" || uinfo.Username == "denise@accuracy.com.sg" || uinfo.Username == "lakshmip@accuracy.com.sg" || uinfo.Username == "vanshika@accuracy.com.sg" || uinfo.Username == "sarika123@accuracy.com.sg" || uinfo.Username == "naveena@accuracy.com.sg" || uinfo.Username == "gowthami@accuracy.com.sg")
            {
                CountryId = 40;
            }

            var list = new List<Tracking>();
            int rows = 0;
            int pageSize = 10;
            //if (Id != null)
            //{
            var downloaded = (int)TrackingTypes.DOWNLOADED;
            var employer = _service.Get(Id);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<Tracking>()
                    .Where(x => x.Type == downloaded && x.UserId == employer.Id);
                result = result.OrderByDescending(x => x.DateUpdated);

                if (!string.IsNullOrEmpty(ResumeTitle))
                {
                    result = result.Where(x => (x.Resume != null && x.Resume.Title.Equals(ResumeTitle)) || (x.JobseekerId != null && x.Jobseeker.Title.Equals(ResumeTitle)));
                }

                if (CountryId != null)
                {
                    result = result.Where(x => (x.JobseekerId != null && x.Jobseeker.CountryId == CountryId) || (x.ResumeId != null && x.Resume.CountryId == CountryId));
                }
                list = result.ToList();

                if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
                {
                    var sdt = Convert.ToDateTime(StartDate);
                    var edt = Convert.ToDateTime(EndDate);
                    list = list.Where(x => x.DateUpdated.Date >= sdt.Date && x.DateUpdated.Date <= edt.Date).ToList();
                }
                ViewBag.ResumeList = new SelectList(list.Select(x => (x.ResumeId != null ? x.Resume.Title : x.Jobseeker.Title)).Distinct().ToList());
            }
            ViewBag.UserInfo = employer;
            ViewBag.CountryList = new SelectList(SharedService.Instance.GetCountryList(), "Id", "Text");
            //}
            rows = list.Count;
            ViewBag.Rows = rows;
            list = list.OrderByDescending(x => x.DateUpdated).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize)).Take(pageSize).ToList();
            ViewBag.Model = new StaticPagedList<Tracking>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);

            return View();
        }

        public ActionResult Alerts(long Id)
        {
            var profile = MemberService.Instance.Get(Id);
            IEnumerable<Alert> alerts = MemberService.Instance.GetAlerts(profile.Username);
            ViewBag.UserInfo = profile;
            return View(alerts);
        }

        public ActionResult UpdateUserAlert(long UserId, int AlertId)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Alert alert = dataHelper.GetSingle<Alert>(AlertId);
                AlertSetting setting = dataHelper.Get<AlertSetting>().SingleOrDefault(x => x.AlertId == AlertId && x.UserId == UserId);
                if (setting == null)
                {
                    setting = new AlertSetting()
                    {
                        AlertId = AlertId,
                        Enabled = false,
                        UserId = UserId,
                        DateUpdated = DateTime.Now
                    };
                    dataHelper.Add<AlertSetting>(setting);
                    TempData["UpdateData"] = string.Format("<b>{0}</b> alert disabled successfully!", alert.Description);
                }
                else
                {
                    if (setting.Enabled)
                    {
                        setting.Enabled = false;
                        TempData["UpdateData"] = string.Format("<b>{0}</b> alert disabled successfully!", alert.Description);
                    }
                    else
                    {
                        setting.Enabled = true;
                        TempData["UpdateData"] = string.Format("<b>{0}</b> alert enabled successfully!", alert.Description);
                    }
                    dataHelper.Update<AlertSetting>(setting);
                }
            }
            return RedirectToAction("Alerts", new { Id = UserId });
        }

        [HttpGet]
        public ActionResult SafetyAdvice()
        {
            string advice = ConfigService.Instance.GetConfigValue("SafetyAdvice");
            SafetyAdviceModel model = new SafetyAdviceModel()
            {
                Advice = advice
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult SafetyAdvice(SafetyAdviceModel model)
        {
            if (ModelState.IsValid)
            {
                DomainService.Instance.UpdateSafetyAdvice(model.Advice);
                TempData["UpdateData"] = "Safety advice updated successfully!";
            }
            return View(model);
        }

        public ActionResult Sales()
        {
            return View();
        }

        public ActionResult SalesDetails(long countryId, string fd, string fm, string fy, string td, string tm, string ty, string name, int pageNumber = 1)
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

            int pageSize = 20;
            int rows = 0;
            DateTime? start = null, end = null;
            if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy))
            {
                start = new DateTime(Convert.ToInt32(fy), Convert.ToInt32(fm), Convert.ToInt32(fd));
            }

            if (!string.IsNullOrEmpty(td) && !string.IsNullOrEmpty(tm) && !string.IsNullOrEmpty(ty))
            {
                end = new DateTime(Convert.ToInt32(ty), Convert.ToInt32(tm), Convert.ToInt32(td));
            }

            List<SalesDetails> list = DomainService.Instance.SalesDetails(countryId, start, end, name, pageNumber, pageSize);
            if (list.Count > 0)
            {
                rows = list.First().MaxRows;
            }

            ViewBag.Model = new StaticPagedList<SalesDetails>(list, pageNumber, pageSize, rows);
            ViewBag.Rows = rows;

            return View();
        }

        [HttpGet]
        public ActionResult ContactList()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ContactList(long? countryId, string fd, string fm, string fy, string td, string tm, string ty)
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

            DateTime? start = null, end = null;
            if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy))
            {
                start = new DateTime(Convert.ToInt32(fy), Convert.ToInt32(fm), Convert.ToInt32(fd));
            }

            if (!string.IsNullOrEmpty(td) && !string.IsNullOrEmpty(tm) && !string.IsNullOrEmpty(ty))
            {
                end = new DateTime(Convert.ToInt32(ty), Convert.ToInt32(tm), Convert.ToInt32(td));
            }

            List<ContactByCountry> list = await iNetworkService.ContactList(countryId, start, end);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult UsersContactList(long countryId)
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

            List country = SharedService.Instance.GetCountry(countryId);
            ViewBag.Country = country.Text;
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> UsersContactList(long countryId, string name, string fd, string fm, string fy, string td, string tm, string ty, int? pageNumber = 1)
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

            DateTime? start = null, end = null;
            if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(fm) && !string.IsNullOrEmpty(fy))
            {
                start = new DateTime(Convert.ToInt32(fy), Convert.ToInt32(fm), Convert.ToInt32(fd));
            }

            if (!string.IsNullOrEmpty(td) && !string.IsNullOrEmpty(tm) && !string.IsNullOrEmpty(ty))
            {
                end = new DateTime(Convert.ToInt32(ty), Convert.ToInt32(tm), Convert.ToInt32(td));
            }
            List<UserContactsByCountry> list = new List<UserContactsByCountry>();
            try
            {
                list = await iNetworkService.UserContactList(countryId, name, start, end, pageNumber, 10);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ContactListByUser(long userId)
        {
            UserInfoEntity user = _service.Get(userId);
            ViewBag.UserName = user.FullName;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ContactListByUser(long userId, string name, string fd, string fm, string fy, string td, string tm, string ty, int? pageNumber = 1)
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

            List<UserContact> list = new List<UserContact>();
            try
            {
                list = await iNetworkService.ContactListByUser(userId, name, start, end, pageNumber, 10);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ViewStatus(long id, string name)
        {
            ViewBag.Name = name;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ViewStatus(long id)
        {
            List<SMSStatusEntity> list = new List<SMSStatusEntity>();
            try
            {
                list = await iNetworkService.ViewStatus(id);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeletePhoneContact(long id)
        {
            int status = 0;
            try
            {
                status = await iNetworkService.DeleteContact(id);
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddBlog()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddBlog(AddBlog addBlog)
        {
            
                //var employer = MemberService.Instance.Get(Convert.ToString(addBlog.Name));
                Blog campaign = new Blog()
                {
                    Title = addBlog.Title,
                    descriptions = addBlog.descriptions,
                    Images =addBlog.Images,
                    Name = addBlog.Name,
                    categories = addBlog.categories,
                    Createddate = DateTime.Now
                };
                List<long> userlist = new List<long>();
                long campaignId = 0;
                string subject = string.Empty;
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    campaignId = Convert.ToInt64(dataHelper.Add<Blog>(campaign));
                    //subject = model.Subject;

                }

            TempData["UpdateData"] = "Add blog successfully!";

            return Redirect("/Admin/Index");
        }
            

    
    }
}