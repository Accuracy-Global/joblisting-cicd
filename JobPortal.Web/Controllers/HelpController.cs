#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Web;
using JobPortal.Web.App_Start;
using JobPortal.Web.Controllers;
using JobPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Controllers
{
    public class HelpController : BaseController
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
        public HelpController(IUserService service, IHelperService helper, IJobService jobService)
#pragma warning restore CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
            : base(service)
        {
            this.helper = helper;
            this.jobService = jobService;
        }
        //
        // GET: /Help/

        public ActionResult Index()
        {
            return View();
        }

        [UrlPrivilegeFilter]
        public ActionResult Terms()
        {
            ViewBag.Content = (new PortalDataService()).GetPageContent("termsofuse");
            return View();
        }

        [UrlPrivilegeFilter]
        public ActionResult Privacy()
        {
            ViewBag.Content = (new PortalDataService()).GetPageContent("privacy");
            return View();
        }

        [UrlPrivilegeFilter]
        public ActionResult Copyright()
        {
            ViewBag.Content = (new PortalDataService()).GetPageContent("copyright");
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> CountryList()
        {
            List<Country> list = await helper.CountryList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> SkillsList()
        {
            List<SkillsList> list = await helper.SkillsList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> CategoryList1()
        {
            List<CategoryList1> list = await helper.CategoryList1();
            return Json(list, JsonRequestBehavior.AllowGet);
        }





        [HttpGet]
        public async Task<ActionResult> UnivList1()
        {
            List<MemberService.UnivList> list = MemberService.Instance.UnivListName();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> CompanyList()
        {
            List<MemberService.CompanyList> list = MemberService.Instance.CompanyListName();
            return Json(list, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public async Task<ActionResult> CategoryList()
        {
            List<Category> list = await helper.Categories();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> StateList(long countryId)
        {
            List<State> list = await helper.StateList(countryId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> SpecializationList(int categoryId)
        {
            List<Specialization> list = await helper.Specializations(categoryId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Qualifications()
        {
            List<Qualification> list = await helper.Qualifications();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
