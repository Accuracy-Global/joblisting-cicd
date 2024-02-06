using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Web.Controllers
{
    public class TipController : BaseController
    {
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
        public TipController(IUserService service)
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
            : base(service)
        {

        }
        // GET: Tip
        public ActionResult Index(string username, DateTime? StartDate = null, DateTime? EndDate = null, int pageNumber = 0)
        {
            int rows = 0;
            int pageSize = 10;
            UserProfile profile = MemberService.Instance.Get(User.Username);
            List<Tip> list = new List<Tip>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
       
                var result = dataHelper.Get<Tip>().Where(x=>x.Type == profile.Type);

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
                    result = result.Where(x => x.DateCreated.Day <= DateTime.Now.Day && x.DateCreated.Month <= DateTime.Now.Month && x.DateCreated.Year <= DateTime.Now.Year);
                }

                result = result.OrderByDescending(x => x.DateCreated);
                rows = result.Count();
                if (rows > 0)
                {
                    list = result.Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                }
            }
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<Tip>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);

            return View();
        }

        public ActionResult Show(long Id)
        {
            UserProfile profile = MemberService.Instance.Get(User.Username);
            Tip entity = new Tip();
           
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                entity = dataHelper.GetSingle<Tip>(Id);
            }
            return View(entity);
        }
    }
}