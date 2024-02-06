//using JobPortal.Data;
//#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
//using JobPortal.Domain;
//#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
//#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
//using JobPortal.Library.Enumerators;
//#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
//#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
//using JobPortal.Services.Contracts;
//#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
//using PagedList;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace JobPortal.Web.Controllers
//{
//    [Authorize]
//    public class AnnouncementController : BaseController
//    {
//#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
//        public AnnouncementController(IUserService service)
//#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
//            : base(service)
//        {

//        }
//        // GET: Announcement
//        //public ActionResult Index(DateTime? StartDate = null, DateTime? EndDate = null, int pageNumber = 0)
//        //{
//        //    int rows = 0;
//        //    int pageSize = 10;
//        //    List<Announcement> list = new List<Announcement>();
//        //    using (JobPortalEntities context = new JobPortalEntities())
//        //    {
//        //        DataHelper dataHelper = new DataHelper(context);
//        //        var user_announcements = dataHelper.Get<UserAnnouncement>().Where(x => x.UserId == User.Id).Select(x => x.AnnouncementId).ToList();
//        //        var result = dataHelper.Get<Announcement>(x => user_announcements.Contains(x.Id));

//        //        if (StartDate != null)
//        //        {
//        //            result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
//        //        }

//        //        if (EndDate != null)
//        //        {
//        //            if (StartDate == null)
//        //            {
//        //                result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
//        //            }
//        //            result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
//        //        }
//        //        else
//        //        {
//        //            result = result.Where(x => x.DateCreated.Day <= DateTime.Now.Day && x.DateCreated.Month <= DateTime.Now.Month && x.DateCreated.Year <= DateTime.Now.Year);
//        //        }

//        //        result = result.OrderByDescending(x => x.DateCreated);
//        //        rows = result.Count();
//        //        if (rows > 0)
//        //        {
//        //            list = result.Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
//        //        }
//        //    }
//        //    ViewBag.Rows = rows;
//        //    ViewBag.Model = new StaticPagedList<Announcement>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);

//        //    return View();
//        //}



//        public ActionResult Show(long Id)
//        {
//            UserProfile profile = MemberService.Instance.Get(User.Username);
//            Announcement entity = null;
//            UserAnnouncement uAnnouncement = new UserAnnouncement();
//            using (JobPortalEntities context = new JobPortalEntities())
//            {
//                DataHelper dataHelper = new DataHelper(context);
//                uAnnouncement = dataHelper.Get<UserAnnouncement>().SingleOrDefault(x => x.UserId == profile.UserId && x.AnnouncementId == Id);
//                if (uAnnouncement != null)
//                {
//                    if (uAnnouncement.Unread)
//                    {
//                        uAnnouncement.Unread = false;
//                        dataHelper.Update<UserAnnouncement>(uAnnouncement);

//                    }
//                    entity = uAnnouncement.Announcement;
//                }
//                else
//                {
//                    if (profile.Type == (int)SecurityRoles.Employers)
//                    {
//                        TempData["Error"] = "To view this message, kindly login with Individual User Account!";
//                    }
//                    if (profile.Type == (int)SecurityRoles.Jobseeker)
//                    {
//                        TempData["Error"] = "To view this message, kindly login with Company User Account!";
//                    }
//                    else
//                    {
//                        TempData["Error"] = "Announcement not found!";
//                    }
//                }                
//            }

//            return View(entity);
//        }
//    }
//}

using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
using JobPortal.Web.App_Start;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Web.Controllers
{
    [Authorize]
    public class AnnouncementController : BaseController
    {
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
        public AnnouncementController(IUserService service)
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
            : base(service)
        {

        }
        // GET: Announcement

        [UrlPrivilegeFilter]
        public ActionResult Index()
        {
            UserProfile profile = MemberService.Instance.Get(User.Username);
            List<Announcement> list = new List<Announcement>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<Announcement>().Where(x => x.Type == profile.Type);
                list = result.ToList();
            }
            return View(list);

        }

        public ActionResult Show(long Id)
        {
            UserProfile profile = MemberService.Instance.Get(User.Username);
            Announcement entity = new Announcement();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                entity = dataHelper.GetSingle<Announcement>(Id);
            }
            return View(entity);


        }
    }
}

