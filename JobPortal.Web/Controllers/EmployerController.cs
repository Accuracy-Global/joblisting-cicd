using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
using Microsoft.Web.WebPages.OAuth;
using PagedList;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using Microsoft.Security.Application;
using System.Web.Script.Serialization;
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using MimeKit;
using MailKit.Net.Smtp;
using System.Configuration;
using JobPortal.Web.App_Start;
using NPOI.HSSF.UserModel;

namespace JobPortal.Web.Controllers
{
    public class EmployerController : BaseController
    {
#pragma warning disable CS0246 // The type or namespace name 'ITrackService' could not be found (are you missing a using directive or an assembly reference?)
        ITrackService iTrackingService;
#pragma warning restore CS0246 // The type or namespace name 'ITrackService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
        IHelperService helper;
#pragma warning restore CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'ITrackService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
        public EmployerController(IUserService service, ITrackService iTrackingService, IHelperService helper)
#pragma warning restore CS0246 // The type or namespace name 'IHelperService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'ITrackService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
            : base(service)
        {
            this.iTrackingService = iTrackingService;
            this.helper = helper;
        }
        private StringBuilder strEmailContent = new StringBuilder();

        public string Logo { get; private set; }

        [Authorize(Roles = "Employers, Institute")]
        [UrlPrivilegeFilter]
        public ActionResult Index(long? Id = null, string Type = "", string Status = null, long? CountryId = null, string fd = "", string fm = "", string fy = "", string td = "", string tm = "", string ty = "", int pageNumber = 1)
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

        [Authorize(Roles = "Employers")]
        [UrlPrivilegeFilter]
        public ActionResult Index_Copy(long? Id = null, string Type = "", string Status = null, long? CountryId = null, string fd = "", string fm = "", string fy = "", string td = "", string tm = "", string ty = "", int pageNumber = 1)
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

        public ActionResult DeleteImage(string Username, string Reason = "")
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
            TempData["SaveData"] = "Logo deleted successfully!";
            return RedirectToAction("UpdateProfile", "Employer");
        }
        [HttpGet]
        public ActionResult AddWebsites1()
        {
            ViewData["error"] = TempData["error"];
            return View(new WebScrapModel());
        }

        [HttpPost]
        public ActionResult AddWebsites1(HttpPostedFileBase file, long? countryId, WebScrapModel model)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            if (ModelState.IsValid == false)
            {
                UserInfoEntity uinfo = _service.Get(User.Id);

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    try
                    {
                        SqlCommand sql = new SqlCommand("SELECT * FROM WebsiteList WHERE CompanyName = @CompanyName", conn);
                        SqlDataAdapter sda = new SqlDataAdapter(sql);
                        sql.Parameters.AddWithValue("@CompanyName", model.CompanyName.Trim());
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            TempData["error"] = @model.CompanyName + " is Already Exist Enter Any Other";
                            return RedirectToAction("AddWebsites1", "Employer");
                        }

                        SqlCommand sql1 = new SqlCommand("SELECT * FROM WebsiteList WHERE Website = @Website", conn);
                        SqlDataAdapter sda1 = new SqlDataAdapter(sql1);
                        sql1.Parameters.AddWithValue("@website", model.Website);
                        DataSet ds1 = new DataSet();
                        sda1.Fill(ds1);
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            TempData["error"] = "This link is Already Exist Enter Any Other";
                            return RedirectToAction("AddWebsites1", "Employer");
                        }
                        else
                        {
                            conn.Open();
                            using (SqlCommand command = conn.CreateCommand())
                            {

                                HttpPostedFileBase postedFile = Request.Files["FileUpload"];

                                if (postedFile != null && postedFile.ContentLength > 0)
                                {
                                    //string filePath = Server.MapPath("~/CompLogo/") + Path.GetFileName(postedFile.FileName);
                                    //postedFile.SaveAs(filePath);
                                    string theFileName = Path.GetFileName(postedFile.FileName);
                                    byte[] thePictureAsBytes = new byte[postedFile.ContentLength];
                                    using (BinaryReader theReader = new BinaryReader(postedFile.InputStream))
                                    {
                                        thePictureAsBytes = theReader.ReadBytes(postedFile.ContentLength);
                                    }
                                    string thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);

                                    command.CommandType = CommandType.Text;
                                    try
                                    {
                                        command.CommandText = string.Format("insert into  WebsiteList" +
                                            "(CountryName,CompanyName,Website,CreatedBy,DateCreated,Name,companylogoslink,Logo,Email) " +
                                            "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                                            model.CountryName, model.CompanyName, model.Website, uinfo.FirstName, DateTime.Now, uinfo.FirstName, model.companylogoslink, thePictureDataAsString, "ksdileep8@gmail.com");
                                        command.ExecuteNonQuery();
                                    }
                                    catch (Exception es)
                                    {
                                        ViewBag.es = es;
                                    }
                                    TempData["UpdateData"] = "Website added successfully!";
                                }
                                else
                                {

                                    command.CommandType = CommandType.Text;
                                    command.CommandText = string.Format("insert into  WebsiteList" +
                                        "(CountryName,CompanyName,Website,CreatedBy,DateCreated,Category,Name,companylogoslink,Logo,Email) " +
                                        "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                                        model.CountryName, model.CompanyName, model.Website, uinfo.FirstName, DateTime.Now, uinfo.FirstName, model.companylogoslink, null, "ksdileep8@gmail.com");
                                    command.ExecuteNonQuery();
                                    TempData["UpdateData"] = "Website added successfully!";
                                }
                            }
                        }

                        conn.Close();

                        string from = "notify@joblisting.com";

                        //string templates = ConfigurationManager.AppSettings["Template"];
                        string baseUrl = "https://www.joblisting.com";
                        string postmail = "master@joblisting.com";
                        string postpassword = "052e14c947dbb70b9d04776344b6d88e-1f1bd6a9-d1f0adb4";
                        string body = string.Empty;

                        MimeMessage mail1 = new MimeMessage();

                        using (SmtpClient client1 = new SmtpClient())
                        {

                            try
                            {

                                var reader1 = new StreamReader(Server.MapPath("~/Templates/Mail/EmailWebsite.html"));
                                if (reader1 != null)
                                {
                                    string ebody1 = reader1.ReadToEnd();
                                    ebody1 = ebody1.Replace("@@receiver", string.Format("{0}", uinfo.FirstName));
                                    ebody1 = ebody1.Replace("@@subject", "Website Details from WebScraping Team");
                                    ebody1 = ebody1.Replace("@@viewurl", "These are details of Website");
                                    ebody1 = ebody1.Replace("@@v1", string.Format("Company Name :: {0}", model.CompanyName));
                                    ebody1 = ebody1.Replace("@@d2", string.Format("Country Name ::::: {0}", model.CountryName));
                                    ebody1 = ebody1.Replace("@@s3", string.Format("Company Url :::: {0}", model.Website));

                                    mail1.From.Clear();
                                    mail1.To.Clear();
                                    mail1.From.Add(new MailboxAddress("Joblisting", from));
                                    mail1.To.Add(new MailboxAddress("Excited User", model.Email));
                                    mail1.Subject = "Website Details from WebScraping Team";
                                    mail1.Body = new TextPart("html")
                                    {
                                        Text = ebody1,
                                    };
                                    try
                                    {

                                        // XXX - Should this be a little different?
                                        client1.ServerCertificateValidationCallback = (s, c, h, e) => true;

                                        client1.Connect("smtp.mailgun.org", 587, false);
                                        client1.AuthenticationMechanisms.Remove("XOAUTH2");
                                        client1.Authenticate(postmail, postpassword);

                                        client1.Send(mail1);
                                        client1.Disconnect(true);

                                    }
                                    catch (Exception ex)
                                    {
                                        //SendEx(ex);
                                    }

                                }



                            }
                            catch (Exception ex)
                            {
                                SendEx(ex);
                            }



                        }


                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        //SendEx(ex);
                    }
                }



            }
            return Redirect("/Employer/Websites1");
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

            using (SmtpClient osmtp = new SmtpClient())
            {
                osmtp.Timeout = 100000;
                osmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                osmtp.Connect("smtp.mailgun.org", 587, false);
                osmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                osmtp.Authenticate(postmail, postpassword);

                osmtp.Send(mail);
                osmtp.Disconnect(true);
            }

        }
        public ActionResult DeleteWebsites(long Id, WebScrapModel model)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                SqlCommand sql = new SqlCommand("delete  FROM WebsiteList WHERE Id = @Id", conn);
                sql.Parameters.AddWithValue("@Id", model.Id);
                conn.Open();
                sql.ExecuteNonQuery();
                conn.Close();
                TempData["UpdateData"] = "Website  Deleted successfully!";
                return RedirectToAction("Websites1", "Employer");
            }
        }
        //[HttpGet]
        //public ActionResult Editwebsites11(int id)
        //{

        //    websiteDB ItemHandler = new websiteDB();

        //    return View(ItemHandler.GetItemList().Find(itemmodel => itemmodel.Id == id));
        //}
        //[HttpPost]
        //public ActionResult Editwebsites11(int id, WebScrapModel iList)
        //{
        //    try
        //    {
        //        websiteDB ItemHandler = new websiteDB();
        //        ItemHandler.UpdateItem(iList);
        //        return RedirectToAction("Index");
        //    }
        //    catch { return View(); }
        //}
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
                    // Name = tip.Name,
                    Logo = tip.Logo,
                    companylogoslink = tip.companylogoslink
                };
            }
            return View(entity);
        }
        [HttpPost]
        public ActionResult EditWebsites(HttpPostedFileBase file, WebScrapModel model)
        {

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            UserInfoEntity uinfo = _service.Get(User.Id);

            using (SqlConnection conn = new SqlConnection(constr))
            {

                conn.Open();
                using (SqlCommand command = conn.CreateCommand())
                {
                    HttpPostedFileBase postedFile = Request.Files["FileUpload"];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        //string filePath = Server.MapPath("~/CompLogo/") + Path.GetFileName(postedFile.FileName);
                        //postedFile.SaveAs(filePath);
                        string theFileName = Path.GetFileName(postedFile.FileName);
                        byte[] thePictureAsBytes = new byte[postedFile.ContentLength];
                        using (BinaryReader theReader = new BinaryReader(postedFile.InputStream))
                        {
                            thePictureAsBytes = theReader.ReadBytes(postedFile.ContentLength);
                        }
                        string thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);

                        command.CommandType = CommandType.Text;
                        command.CommandText = string.Format("update  WebsiteList " +
                            "set CountryName='{0}',CompanyName='{1}',Website='{2}',Email='{3}',UpdatedBy='{4}',DateUpdated='{5}',Name='{7}',companylogoslink='{8}',Logo='{9}' " +
                            "where Id={6}",
                            model.CountryName, model.CompanyName, model.Website, model.Email, uinfo.FirstName, DateTime.Now, model.Id, uinfo.FirstName, model.companylogoslink, thePictureDataAsString);
                        command.ExecuteNonQuery();
                        TempData["UpdateData"] = "Website  updated successfully!";
                    }
                    else
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = string.Format("update  WebsiteList " +
                            "set CountryName='{0}',CompanyName='{1}',Website='{2}',Email='{3}',UpdatedBy='{4}',DateUpdated='{5}',Name='{7}',companylogoslink='{8}',Logo='{9}' " +
                            "where Id={6}",
                            model.CountryName, model.CompanyName, model.Website, model.Email, uinfo.FirstName, DateTime.Now, model.Id, uinfo.FirstName, model.companylogoslink, null);
                        command.ExecuteNonQuery();
                        TempData["UpdateData"] = "Website  updated successfully!";
                    }
                }
                conn.Close();
            }

            //if (ModelState.IsValid)
            //{
            //    using (SqlConnection conn = new SqlConnection(constr))
            //    {
            //        try
            //        {
            //            conn.Open();
            //            using (SqlCommand command = conn.CreateCommand())
            //            {
            //                command.CommandType = CommandType.Text;
            //                command.CommandText = string.Format("update  WebsiteList " +
            //                    "set CountryName='{0}',CompanyName='{1}',Website='{2}',Email='{3}',UpdatedBy='{4}',DateUpdated='{5}',Name='{7}' " +
            //                    "where Id={Id}",
            //                    model.CountryName, model.CompanyName, model.Website, model.Email, uinfo.FirstName, DateTime.Now, model.Id, model.Name);
            //                command.ExecuteNonQuery();
            //            }
            //            conn.Close();
            //            conn.Close();

            //            string from = "notify@joblisting.com";

            //            //string templates = ConfigurationManager.AppSettings["Template"];
            //            string baseUrl = "https://www.joblisting.com";
            //            string postmail = "master@joblisting.com";
            //            string postpassword = "052e14c947dbb70b9d04776344b6d88e-1f1bd6a9-d1f0adb4";
            //            string body = string.Empty;

            //            MimeMessage mail1 = new MimeMessage();

            //            using (SmtpClient client1 = new SmtpClient())
            //            {

            //                try
            //                {

            //                    var reader1 = new StreamReader(Server.MapPath("~/Templates/Mail/EmailWebsite.html"));
            //                    if (reader1 != null)
            //                    {
            //                        string ebody1 = reader1.ReadToEnd();
            //                        ebody1 = ebody1.Replace("@@receiver", string.Format("{0}", uinfo.FirstName));
            //                        ebody1 = ebody1.Replace("@@subject", "Website Details from WebScraping Team");
            //                        ebody1 = ebody1.Replace("@@viewurl", "These are details of Website");
            //                        ebody1 = ebody1.Replace("@@v1", string.Format("Company Name :: {0}", model.CompanyName));
            //                        ebody1 = ebody1.Replace("@@d2", string.Format("Country Name ::::: {0}", model.CountryName));
            //                        ebody1 = ebody1.Replace("@@s3", string.Format("Company Url :::: {0}", model.Website));

            //                        mail1.From.Clear();
            //                        mail1.To.Clear();
            //                        mail1.From.Add(new MailboxAddress("Joblisting", from));
            //                        mail1.To.Add(new MailboxAddress("Excited User", model.Email));
            //                        mail1.Subject = "Website Details from WebScraping Team";
            //                        mail1.Body = new TextPart("html")
            //                        {
            //                            Text = ebody1,
            //                        };
            //                        try
            //                        {

            //                            // XXX - Should this be a little different?
            //                            client1.ServerCertificateValidationCallback = (s, c, h, e) => true;

            //                            client1.Connect("smtp.mailgun.org", 587, false);
            //                            client1.AuthenticationMechanisms.Remove("XOAUTH2");
            //                            client1.Authenticate(postmail, postpassword);

            //                            client1.Send(mail1);
            //                            client1.Disconnect(true);

            //                        }
            //                        catch (Exception ex)
            //                        {
            //                            SendEx(ex);
            //                        }

            //                    }



            //                }
            //                catch (Exception ex)
            //                {
            //                    SendEx(ex);
            //                }



            //            }
            //        }
            //        catch (Exception ex)c
            //        {
            //            conn.Close();
            //            SendEx(ex);
            //        }
            //    }
            //        TempData["UpdateData"] = "Website updated successfully!";
            //}

            return Redirect("/Employer/Websites1");
        }

        public ActionResult Summary(int? Type = null, DateTime? StartDate = null, DateTime? EndDate = null, int pageNumber = 0, string Name = null)
        {


            List<WebsiteList> customers = new List<WebsiteList>();
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                if (UserInfo.Username == "ksdileep77@gmail.com")
                {

                    if (Name != null)
                    {
                        SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where  CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                        SqlDataAdapter sda = new SqlDataAdapter(sql);
                        sql.Parameters.AddWithValue("@CompanyName", Name.Trim());
                        DataSet ds = new DataSet();
                        //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                        //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                        sql.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = sql.ExecuteReader())
                        {

                            while (sdr.Read())
                            {
                                customers.Add(new WebsiteList
                                {

                                    // Id = sdr["Id"].ToString(),
                                    CreatedBy = sdr["CreatedBy"].ToString(),
                                    DateCreated = Convert.ToDateTime(sdr["Date"]),
                                    Email = sdr["Total"].ToString(),

                                });
                            }
                        }
                        con.Close();
                    }
                    else
                    {

                        SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy   from websitelist  where CreatedBy='Prasanna' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                        //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                        //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                        sql.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = sql.ExecuteReader())
                        {

                            while (sdr.Read())
                            {
                                customers.Add(new WebsiteList
                                {

                                    // Id = sdr["Id"].ToString(),
                                    CreatedBy = sdr["CreatedBy"].ToString(),
                                    DateCreated = Convert.ToDateTime(sdr["Date"]),
                                    Email = sdr["Total"].ToString(),

                                });
                            }
                        }
                        con.Close();
                    }

                }
                
                else if (UserInfo.Username == "sarikas@accuracy.com.sg")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='Sarikas' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                    //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                    //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }

                else if (UserInfo.Username == "pranaykumar@accuracy.com.sg")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='Pranaykumar' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                    //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                    //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }

                else if (UserInfo.Username == "lakshmi.prapurna@joblisting.com")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='Lakshmi' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                    //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                    //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }

                else if (UserInfo.Username == "pavanireddy@accuracy.com.sg")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='Pavanireddy' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                    //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                    //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }
                else if (UserInfo.Username == "manisha@joblisting.com")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='Manisha' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                    //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                    //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }

                else if (UserInfo.Username == "prapurna@accuracy.com.sg")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='Prapurna' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                    //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                    //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }

                else if (UserInfo.Username == "venkateswari@accuracy.com.sg")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='Venkateswari' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                    //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                    //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }
                else if (UserInfo.Username == "anusha@accuracy.com.sg")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='Anusha' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                    //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                    //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }

                else if (UserInfo.Username == "nirmal.verma@joblisting.com" || UserInfo.Username == "nirmal.verma@joblisting.com")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='Nirmal' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                    //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                    //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }


                else if (UserInfo.Username == "kavyav@accuracy.com.sg"|| UserInfo.Username == "Kavyav@accuracy.com.sg")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='Kavyav' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                    //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                    //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }
                else if (UserInfo.Username == "anjaneyulu@accuracy.com.sg")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='Anjaneyulu' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                    //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                    //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }

               
                else if (UserInfo.Username == "vinay@joblisting.com")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='vinay' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                    //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                    //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }
              
                else if (UserInfo.Username == "sindhuja@joblisting.com")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='Sindhuja' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);
                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }
                else if (UserInfo.Username == "Vinay@joblisting.com")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='Vinay' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);
                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }
               
                else if (UserInfo.Username == "mubasher@accuracy.com.sg")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='mubasher' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                    //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                    //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }
                else if (UserInfo.Username == "hema@accuracy.com.sg")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='hema' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                    //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                    //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }

                else if (UserInfo.Username == "surekha@joblisting.com")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='Surekha' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                    //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                    //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }
                else if (UserInfo.Username == "yugandhar@accuracy.com.sg")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='Yugandhar' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                    //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                    //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }
                else if (UserInfo.Username == "chetanmore@accuracy.com.sg")
                {
                    SqlCommand sql = new SqlCommand("select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy='Chetanmore' group by CreatedBy,CAST(DateCreated AS DATE) order by Date desc", con);

                    //string query = "select CAST(DateCreated AS DATE) as Date,count(*) as Total ,CreatedBy from websitelist where CreatedBy=@CompanyName group by CreatedBy,CAST(DateCreated AS DATE)";
                    //query.Parameters.AddWithValue("@CompanyName",Name.Trim());

                    sql.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = sql.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            customers.Add(new WebsiteList
                            {

                                // Id = sdr["Id"].ToString(),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["Date"]),
                                Email = sdr["Total"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }

            }




            ViewBag.Customers = customers;
            return View();
        }
        public ActionResult Websites11(int? Type = null, DateTime? StartDate = null, DateTime? EndDate = null, int pageNumber = 0)
        {
            int rows = 0;
            int pageSize = 10;
            websiteDB dbhandle = new websiteDB();
            ModelState.Clear();
            var result = dbhandle.GetItemListAsif();
            rows = result.Count();
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<WebScrapModel>(result, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
            return View();
        }
        public ActionResult Websites1(int? Type = null, DateTime? StartDate = null, DateTime? EndDate = null, int pageNumber = 0)
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            int rows = 0;
            int pageSize = 10;
            List<WebsiteList> list = new List<WebsiteList>();
            //if (UserInfo.Username == "ksdileep77@gmail.com")
            if (User != null)
            {
                if (User.Username == "ksdileep77@gmail.com")
                {
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
                }
                else if (User.Info.FirstName == "Sarikas")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);

                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Sarikas");
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
                }
                else if (User.Info.FirstName == "Pranaykumar")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);

                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Pranaykumar");
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
                }

                else if (User.Info.FirstName == "Pavanireddy")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Pavanireddy");
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
                }
              
                else if (User.Info.FirstName == "Manisha")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Manisha");
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
                }
                else if (User.Info.FirstName == "Chetanmore")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Chetanmore");
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
                }


                else if (User.Info.FirstName == "Lakshmi")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Lakshmi");
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
                }

                else if (User.Info.FirstName == "Prapurna")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Prapurna");
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
                }

                else if (User.Info.FirstName == "Venkateswari")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Venkateswari");
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
                }
                else if (User.Info.FirstName == "Anusha")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Anusha");
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
                }
                

                     else if (User.Info.FirstName == "Nirmal")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Nirmal");
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
                }
                else if (User.Info.FirstName == "Kavyav")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Kavyav");
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
                }

                else if (User.Info.FirstName == "Vinay")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Vinay");
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
                }

                else if (User.Info.FirstName == "anjaneyulu")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Anjaneyulu");
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
                }

                else if (User.Info.FirstName == "Sindhuja")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Sindhuja");
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
                }

                else if (User.Info.FirstName == "Yugandhar")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Yugandhar");
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
                }


                else if (User.Info.FirstName == "Mubasher")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Mubasher");
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
                }
                else if (User.Info.FirstName == "Hema")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Hema");
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
                }
                else if (User.Info.FirstName == "Surekha")
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Surekha");
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
                }
                //else if (User.Info.FirstName == "Surendra")
                //{
                //    using (JobPortalEntities context = new JobPortalEntities())
                //    {
                //        DataHelper dataHelper = new DataHelper(context);
                //        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Surendra");
                //        if (StartDate != null)
                //        {
                //            result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                //        }

                //        if (EndDate != null)
                //        {
                //            if (StartDate == null)
                //            {
                //                result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                //            }
                //            result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                //        }

                //        rows = result.Count();
                //        if (rows > 0)
                //        {
                //            list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                //        }
                //    }
                //    ViewBag.Rows = rows;
                //    ViewBag.Model = new StaticPagedList<WebsiteList>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
                //}
                //else if (User.Info.FirstName == "Amulya")
                //{
                //    using (JobPortalEntities context = new JobPortalEntities())
                //    {
                //        DataHelper dataHelper = new DataHelper(context);
                //        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Amulya");
                //        if (StartDate != null)
                //        {
                //            result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                //        }

                //        if (EndDate != null)
                //        {
                //            if (StartDate == null)
                //            {
                //                result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                //            }
                //            result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                //        }

                //        rows = result.Count();
                //        if (rows > 0)
                //        {
                //            list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                //        }
                //    }
                //    ViewBag.Rows = rows;
                //    ViewBag.Model = new StaticPagedList<WebsiteList>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
                //}
                //else if (User.Info.FirstName == "Prasanna")
                //{
                //    using (JobPortalEntities context = new JobPortalEntities())
                //    {
                //        DataHelper dataHelper = new DataHelper(context);
                //        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Prasanna");
                //        if (StartDate != null)
                //        {
                //            result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                //        }

                //        if (EndDate != null)
                //        {
                //            if (StartDate == null)
                //            {
                //                result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                //            }
                //            result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                //        }

                //        rows = result.Count();
                //        if (rows > 0)
                //        {
                //            list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                //        }
                //    }
                //    ViewBag.Rows = rows;
                //    ViewBag.Model = new StaticPagedList<WebsiteList>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
                //}
                //else if (User.Info.FirstName == "Prudhvi")
                //{
                //    using (JobPortalEntities context = new JobPortalEntities())
                //    {
                //        DataHelper dataHelper = new DataHelper(context);
                //        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Prudhvi");
                //        if (StartDate != null)
                //        {
                //            result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                //        }

                //        if (EndDate != null)
                //        {
                //            if (StartDate == null)
                //            {
                //                result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                //            }
                //            result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                //        }

                //        rows = result.Count();
                //        if (rows > 0)
                //        {
                //            list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                //        }
                //    }
                //    ViewBag.Rows = rows;
                //    ViewBag.Model = new StaticPagedList<WebsiteList>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
                //}
                //else if (User.Info.FirstName == "Ramya")
                //{
                //    using (JobPortalEntities context = new JobPortalEntities())
                //    {
                //        DataHelper dataHelper = new DataHelper(context);
                //        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Ramya");
                //        if (StartDate != null)
                //        {
                //            result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                //        }

                //        if (EndDate != null)
                //        {
                //            if (StartDate == null)
                //            {
                //                result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                //            }
                //            result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                //        }

                //        rows = result.Count();
                //        if (rows > 0)
                //        {
                //            list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                //        }
                //    }
                //    ViewBag.Rows = rows;
                //    ViewBag.Model = new StaticPagedList<WebsiteList>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
                //}
                //else if (User.Info.FirstName == "Sandhya")
                //{
                //    using (JobPortalEntities context = new JobPortalEntities())
                //    {
                //        DataHelper dataHelper = new DataHelper(context);
                //        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Sandhya");
                //        if (StartDate != null)
                //        {
                //            result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                //        }

                //        if (EndDate != null)
                //        {
                //            if (StartDate == null)
                //            {
                //                result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                //            }
                //            result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                //        }

                //        rows = result.Count();
                //        if (rows > 0)
                //        {
                //            list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                //        }
                //    }
                //    ViewBag.Rows = rows;
                //    ViewBag.Model = new StaticPagedList<WebsiteList>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
                //}
                //else if (User.Info.FirstName == "Surekha")
                //{
                //    using (JobPortalEntities context = new JobPortalEntities())
                //    {
                //        DataHelper dataHelper = new DataHelper(context);
                //        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Surekha");
                //        if (StartDate != null)
                //        {
                //            result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                //        }

                //        if (EndDate != null)
                //        {
                //            if (StartDate == null)
                //            {
                //                result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                //            }
                //            result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                //        }

                //        rows = result.Count();
                //        if (rows > 0)
                //        {
                //            list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                //        }
                //    }
                //    ViewBag.Rows = rows;
                //    ViewBag.Model = new StaticPagedList<WebsiteList>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
                //}
                //else if (User.Info.FirstName == "More")
                //{
                //    using (JobPortalEntities context = new JobPortalEntities())
                //    {
                //        DataHelper dataHelper = new DataHelper(context);
                //        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "More");
                //        if (StartDate != null)
                //        {
                //            result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                //        }

                //        if (EndDate != null)
                //        {
                //            if (StartDate == null)
                //            {
                //                result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                //            }
                //            result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                //        }

                //        rows = result.Count();
                //        if (rows > 0)
                //        {
                //            list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                //        }
                //    }
                //    ViewBag.Rows = rows;
                //    ViewBag.Model = new StaticPagedList<WebsiteList>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
                //}
                //else if (User.Info.FirstName == "Sushma")
                //{
                //    using (JobPortalEntities context = new JobPortalEntities())
                //    {
                //        DataHelper dataHelper = new DataHelper(context);
                //        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Sushma");
                //        if (StartDate != null)
                //        {
                //            result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                //        }

                //        if (EndDate != null)
                //        {
                //            if (StartDate == null)
                //            {
                //                result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                //            }
                //            result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                //        }

                //        rows = result.Count();
                //        if (rows > 0)
                //        {
                //            list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                //        }
                //    }
                //    ViewBag.Rows = rows;
                //    ViewBag.Model = new StaticPagedList<WebsiteList>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
                //}
                //else if (User.Info.FirstName == "Harshavardhan")
                //{
                //    using (JobPortalEntities context = new JobPortalEntities())
                //    {
                //        DataHelper dataHelper = new DataHelper(context);
                //        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Harshavardhan");
                //        if (StartDate != null)
                //        {
                //            result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                //        }

                //        if (EndDate != null)
                //        {
                //            if (StartDate == null)
                //            {
                //                result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                //            }
                //            result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                //        }

                //        rows = result.Count();
                //        if (rows > 0)
                //        {
                //            list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                //        }
                //    }
                //    ViewBag.Rows = rows;
                //    ViewBag.Model = new StaticPagedList<WebsiteList>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
                //}


                //else if (User.Info.FirstName == "Sharda")
                //{
                //    using (JobPortalEntities context = new JobPortalEntities())
                //    {
                //        DataHelper dataHelper = new DataHelper(context);
                //        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Sharda");
                //        if (StartDate != null)
                //        {
                //            result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                //        }

                //        if (EndDate != null)
                //        {
                //            if (StartDate == null)
                //            {
                //                result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                //            }
                //            result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                //        }

                //        rows = result.Count();
                //        if (rows > 0)
                //        {
                //            list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                //        }
                //    }
                //    ViewBag.Rows = rows;
                //    ViewBag.Model = new StaticPagedList<WebsiteList>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
                //}
                //else if (User.Info.FirstName == "Bharathi")
                //{
                //    using (JobPortalEntities context = new JobPortalEntities())
                //    {
                //        DataHelper dataHelper = new DataHelper(context);
                //        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Bharathi");
                //        if (StartDate != null)
                //        {
                //            result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                //        }

                //        if (EndDate != null)
                //        {
                //            if (StartDate == null)
                //            {
                //                result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                //            }
                //            result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                //        }

                //        rows = result.Count();
                //        if (rows > 0)
                //        {
                //            list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                //        }
                //    }
                //    ViewBag.Rows = rows;
                //    ViewBag.Model = new StaticPagedList<WebsiteList>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
                //}
                //else if (User.Info.FirstName == "Sunesh")
                //{
                //    using (JobPortalEntities context = new JobPortalEntities())
                //    {
                //        DataHelper dataHelper = new DataHelper(context);
                //        var result = dataHelper.Get<WebsiteList>().Where(x => x.CreatedBy == "Sunesh");
                //        if (StartDate != null)
                //        {
                //            result = result.Where(x => x.DateCreated.Day >= StartDate.Value.Day && x.DateCreated.Month >= StartDate.Value.Month && x.DateCreated.Year >= StartDate.Value.Year);
                //        }

                //        if (EndDate != null)
                //        {
                //            if (StartDate == null)
                //            {
                //                result = result.Where(x => x.DateCreated.Day >= DateTime.Now.Day && x.DateCreated.Month >= DateTime.Now.Month && x.DateCreated.Year >= DateTime.Now.Year);
                //            }
                //            result = result.Where(x => x.DateCreated.Day <= EndDate.Value.Day && x.DateCreated.Month <= EndDate.Value.Month && x.DateCreated.Year <= EndDate.Value.Year);
                //        }

                //        rows = result.Count();
                //        if (rows > 0)
                //        {
                //            list = result.OrderByDescending(x => (x.DateUpdated != null ? x.DateUpdated : x.DateCreated)).Skip(pageNumber > 1 ? (pageNumber - 1) * pageSize : pageNumber * pageSize).Take(pageSize).ToList();
                //        }
                //    }
                //    ViewBag.Rows = rows;
                //    ViewBag.Model = new StaticPagedList<WebsiteList>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
                //}

            }
            //else
            //{

            //    if (UserInfo.Username == "asif.s@joblisting.com")
            //    {
            //        websiteDB dbhandle = new websiteDB();
            //        ModelState.Clear();
            //        var result = dbhandle.GetItemListAsif();
            //        rows = result.Count();

            //        ViewBag.Rows = rows;
            //        ViewBag.Model = new StaticPagedList<WebScrapModel>(result, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);


            //    }
            //    else if (UserInfo.Username == "manisha@joblisting.com")
            //    {
            //        websiteDB dbhandle = new websiteDB();
            //        ModelState.Clear();
            //        var result = dbhandle.GetItemListManisha();
            //        rows = result.Count();

            //        ViewBag.Rows = rows;
            //        ViewBag.Model = new StaticPagedList<WebScrapModel>(result, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);


            //    }
            else if (UserInfo.Username == "nirmal.verma@joblisting.com")
            {
                websiteDB dbhandle = new websiteDB();
                ModelState.Clear();
                var result = dbhandle.GetItemListNirmal();
                rows = result.Count();

                ViewBag.Rows = rows;
                ViewBag.Model = new StaticPagedList<WebScrapModel>(result, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);


            }

            //    else if (UserInfo.Username == "Reshma@accuracy.com.sg")
            //    {
            //        websiteDB dbhandle = new websiteDB();
            //        ModelState.Clear();
            //        var result = dbhandle.GetItemListReshma();
            //        rows = result.Count();

            //        ViewBag.Rows = rows;
            //        ViewBag.Model = new StaticPagedList<WebScrapModel>(result, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);


            //    }
            //    else if (UserInfo.Username == "vinay@joblisting.com")
            //    {
            //        websiteDB dbhandle = new websiteDB();
            //        ModelState.Clear();
            //        var result = dbhandle.GetItemListvinay();
            //        rows = result.Count();

            //        ViewBag.Rows = rows;
            //        ViewBag.Model = new StaticPagedList<WebScrapModel>(result, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);


            //    }
            //    else if (UserInfo.Username == "madhava@accuracy.com.sg")
            //    {
            //        websiteDB dbhandle = new websiteDB();
            //        ModelState.Clear();
            //        var result = dbhandle.GetItemListMadhava();
            //        rows = result.Count();

            //        ViewBag.Rows = rows;
            //        ViewBag.Model = new StaticPagedList<WebScrapModel>(result, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);


            //    }
            //    else if (UserInfo.Username == "yugandhar@joblisting.com")
            //    {
            //        websiteDB dbhandle = new websiteDB();
            //        ModelState.Clear();
            //        var result = dbhandle.GetItemListYagandhar();
            //        rows = result.Count();

            //        ViewBag.Rows = rows;
            //        ViewBag.Model = new StaticPagedList<WebScrapModel>(result, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);


            //    }
            //    else if (UserInfo.Username == "Poojitha@joblisting.com")
            //    {
            //        websiteDB dbhandle = new websiteDB();
            //        ModelState.Clear();
            //        var result = dbhandle.GetItemListPoojitha();
            //        rows = result.Count();

            //        ViewBag.Rows = rows;
            //        ViewBag.Model = new StaticPagedList<WebScrapModel>(result, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);




            //    }
            //    else if (UserInfo.Username == "sandhya.lakshmi@joblisting.com")
            //    {
            //        websiteDB dbhandle = new websiteDB();
            //        ModelState.Clear();
            //        var result = dbhandle.GetItemListSandhya();
            //        rows = result.Count();

            //        ViewBag.Rows = rows;
            //        ViewBag.Model = new StaticPagedList<WebScrapModel>(result, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);


            //    }
            //    else if (UserInfo.Username == "ramya.nikkudala@joblisting.com")
            //    {
            //        websiteDB dbhandle = new websiteDB();
            //        ModelState.Clear();
            //        var result = dbhandle.GetItemListRamya();
            //        rows = result.Count();

            //        ViewBag.Rows = rows;
            //        ViewBag.Model = new StaticPagedList<WebScrapModel>(result, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);


            //    }
            //    else if (UserInfo.Username == "surekha@joblisting.com")
            //    {
            //        websiteDB dbhandle = new websiteDB();
            //        ModelState.Clear();
            //        var result = dbhandle.GetItemListSurekha();
            //        rows = result.Count();

            //        ViewBag.Rows = rows;
            //        ViewBag.Model = new StaticPagedList<WebScrapModel>(result, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);


            //    }
            //    else if (UserInfo.Username == "lakshmi.prapurna@joblisting.com")
            //    {
            //        websiteDB dbhandle = new websiteDB();
            //        ModelState.Clear();
            //        var result = dbhandle.GetItemListlakshmi();
            //        rows = result.Count();

            //        ViewBag.Rows = rows;
            //        ViewBag.Model = new StaticPagedList<WebScrapModel>(result, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);


            //    }
            //    else if (UserInfo.Username == "ahamed@joblisting.com")
            //    {
            //        websiteDB dbhandle = new websiteDB();
            //        ModelState.Clear();
            //        var result = dbhandle.GetItemListahamed();
            //        rows = result.Count();

            //        ViewBag.Rows = rows;
            //        ViewBag.Model = new StaticPagedList<WebScrapModel>(result, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);


            //    }
            //    else if (UserInfo.Username == "raghu@joblisting.com")
            //    {
            //        websiteDB dbhandle = new websiteDB();
            //        ModelState.Clear();
            //        var result = dbhandle.GetItemListRaghu();
            //        rows = result.Count();

            //        ViewBag.Rows = rows;
            //        ViewBag.Model = new StaticPagedList<WebScrapModel>(result, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);


            //    }

            //    else if (UserInfo.Username == "rajurajmanthena123@gmail.com")
            //    {
            //        websiteDB dbhandle = new websiteDB();
            //        ModelState.Clear();
            //        var result = dbhandle.GetItemListrajurajmanthena123();
            //        rows = result.Count();

            //        ViewBag.Rows = rows;
            //        ViewBag.Model = new StaticPagedList<WebScrapModel>(result, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);


            //    }
            //    else if (UserInfo.Username == "prasanna.a@joblisting.com")
            //    {
            //        websiteDB dbhandle = new websiteDB();
            //        ModelState.Clear();
            //        var result = dbhandle.GetItemListPrasanna();
            //        rows = result.Count();

            //        ViewBag.Rows = rows;
            //        ViewBag.Model = new StaticPagedList<WebScrapModel>(result, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);


            //    }
            //}



            return View();
        }

        [Authorize(Roles = "Company, Institute")]
        public ActionResult ViewHistory(long Id, int pageNumber = 0)
        {
            var list = new List<JobViewModel>();
            int rows = 0;
            int pageSize = 10;

            if (User != null)
            {
                var viewed = (int)TrackingTypes.VIEWED;
                var employer = MemberService.Instance.Get(User.Id);

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    var result = dataHelper.Get<Tracking>().Where(x => x.Type == viewed && x.Job != null && x.JobId == Id &&
                                x.Job.Employer.UserId == employer.UserId && x.JobseekerId != null && x.Job.IsDeleted == false && x.Job.IsActive &&
                                x.Job.IsPublished == true);
                    var listResult = result.ToList();

                    list = listResult.GroupBy(x => new { JobId = x.JobId, JobseekerId = x.JobseekerId }).Select(x => new JobViewModel() { JobId = x.Key.JobId.Value, JobseekerId = x.Key.JobseekerId.Value, Times = x.Count() }).ToList();
                    list = list.OrderBy(x => x.JobId).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize)).Take(pageSize).ToList();
                }
            }

            ViewBag.Model = new StaticPagedList<JobViewModel>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
            ViewBag.Rows = rows;
            ViewBag.Id = Id;
            return View();
        }


        [Authorize(Roles = "Employers, Institute")]
        public ActionResult ViewHistoryDetails(long Id, long JobseekerId, int pageNumber = 0)
        {
            var list = new List<JobViewModel>();
            int rows = 0;
            int pageSize = 10;

            if (User != null)
            {
                var viewed = (int)TrackingTypes.VIEWED;
                var employer = MemberService.Instance.Get(User.Username);

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    var result = dataHelper.Get<Tracking>().Where(x => x.Type == viewed && x.Job != null && x.JobId == Id &&
                                x.Job.Employer.UserId == employer.UserId && x.JobseekerId != null && x.Job.IsDeleted == false && x.Job.IsActive &&
                                x.Job.IsPublished == true && x.JobseekerId == JobseekerId);
                    var listResult = result.ToList();

                    list = listResult.GroupBy(x => new { JobId = x.JobId, JobseekerId = x.JobseekerId, ViewedOn = x.DateUpdated.Date }).Select(x => new JobViewModel() { JobId = x.Key.JobId.Value, JobseekerId = x.Key.JobseekerId.Value, ViewedOn = x.Key.ViewedOn, Times = x.Count() }).ToList();
                    rows = list.Count;
                    list = list.OrderBy(x => x.JobId).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize)).Take(pageSize).ToList();
                }
            }

            ViewBag.Model = new StaticPagedList<JobViewModel>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
            ViewBag.Rows = rows;
            ViewBag.Id = Id;
            ViewBag.JobseekerId = JobseekerId;

            return View();
        }

        [Authorize(Roles = "Employers, Institute")]
        [UrlPrivilegeFilter]
        public async Task<ActionResult> DownloadHistory(string title, long? CountryId, string StartDate, string EndDate, int pageNumber = 1)
        {
            int rows = 0;
            int pageSize = 10;
            List<DownloadEntity> history = new List<DownloadEntity>();
            if (User != null)
            {
                DateTime? sdt = null, edt = null;
                if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
                {
                    sdt = Convert.ToDateTime(StartDate);
                    edt = Convert.ToDateTime(EndDate);
                }
                string rtitle = string.IsNullOrEmpty(title) ? null : title;
                history = await _service.DownloadHistory(User.Id, rtitle, CountryId, sdt, edt, pageNumber);
                if (history.Count > 0)
                {
                    rows = history.First().MaxRows;
                    ViewBag.ResumeList = new SelectList(history.Where(x => x.Title != null).Select(x => x.Title).Distinct().ToList());
                }
                ViewBag.CountryList = new SelectList(SharedService.Instance.GetCountryList(), "Id", "Text");
            }
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<DownloadEntity>(history, pageNumber, pageSize, rows);

            return View();
        }

        /// <summary>
        /// Promote job.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Employers")]
        [HttpGet]
        public ActionResult Promote(long id)
        {
            return RedirectToAction("Promote", "Package", new { id = id, type = "J", returnUrl = "/Employer/Index" });
        }

        /// <summary>
        /// Updates listed job by employer.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Employers")]
        [HttpGet]
        public ActionResult UpdateJob(long id)
        {
            var job = JobService.Instance.Get(id);
            if (job.IsDeleted)
            {
                TempData["Error"] = "Job has been already Deleted";
                return Redirect(Request.UrlReferrer.ToString());
            }
            var model = new JobListingModel();
            model.Id = id;
            model.Title = job.Title;

            if (job.IsPublished.Value == true && job.InEditMode == false)
            {
                model.Summary = job.Summary;
                model.Description = job.Description;
                model.Requirements = job.Requirements;
                model.Responsibilities = job.Responsilibies;

                model.MinimumExperience = job.MinimumExperience;
                model.MaximumExperience = job.MaximumExperience;
                model.SalaryCurrency = job.Currency;
                model.MinimumSalary = job.MinimumSalary;
                model.MaximumSalary = job.MaximumSalary;
                model.MinimumAge = job.MinimumAge;
                model.MaximumAge = job.MaximumAge;
                model.EmploymentType = job.EmploymentTypeId.Value;
                model.QualificationId = job.QualificationId;
            }
            else if (job.IsPublished.Value == true && job.InEditMode == true)
            {
                model.Summary = job.NewSummary;
                model.Description = job.NewDescription;
                model.Requirements = job.NewRequirements;
                model.Responsibilities = job.NewResponsilibies;
                model.MinimumExperience = job.NewMinimumExperience;
                model.MaximumExperience = job.NewMaximumExperience;
                model.SalaryCurrency = job.NewCurrency;
                model.MinimumSalary = job.NewMinimumSalary;
                model.MaximumSalary = job.NewMaximumSalary;
                model.MinimumAge = job.NewMinimumAge;
                model.MaximumAge = job.NewMaximumAge;
                if (job.NewEmploymentTypeId != null)
                {
                    model.EmploymentType = job.NewEmploymentTypeId.Value;
                }
                model.QualificationId = job.NewQualificationId;
            }
            else if (job.IsPublished.Value == false && job.InEditMode == false)
            {
                model.Summary = job.Summary;
                model.Description = job.Description;
                model.Requirements = job.Requirements;
                model.Responsibilities = job.Responsilibies;
                model.MinimumExperience = job.MinimumExperience;
                model.MaximumExperience = job.MaximumExperience;
                model.SalaryCurrency = job.Currency;
                model.MinimumSalary = job.MinimumSalary;
                model.MaximumSalary = job.MaximumSalary;
                model.MinimumAge = job.MinimumAge;
                model.MaximumAge = job.MaximumAge;
                model.EmploymentType = job.EmploymentTypeId.Value;
                model.QualificationId = job.QualificationId;
            }
            else if (job.IsPublished.Value == false && job.InEditMode == true)
            {
                model.Summary = job.NewSummary;
                model.Description = job.NewDescription;
                model.Requirements = job.NewRequirements;
                model.Responsibilities = job.NewResponsilibies;
                model.MinimumExperience = job.NewMinimumExperience;
                model.MaximumExperience = job.NewMaximumExperience;
                model.SalaryCurrency = job.NewCurrency;
                model.MinimumSalary = job.NewMinimumSalary;
                model.MaximumSalary = job.NewMaximumSalary;
                model.MinimumAge = job.NewMinimumAge;
                model.MaximumAge = job.NewMaximumAge;
                if (job.NewEmploymentTypeId != null)
                {
                    model.EmploymentType = job.NewEmploymentTypeId.Value;
                }
                model.QualificationId = job.NewQualificationId;
            }
            model.IsFeaturedJob = Convert.ToBoolean(job.IsFeaturedJob);
            model.CategoryId = (int)job.CategoryId;
            //model.SpecializationId = (int)job.SpecializationId;
            //model.CountryId = job.CountryId.Value;
          //  model.StateId = job.StateId;
            //model.City = job.City;
            model.Zip = job.Zip == null ? "" : Convert.ToInt32(job.Zip).ToString();
            model.IsPublished = job.IsPublished.Value;
            model.IsRejected = job.IsRejected;
            model.InEditMode = job.InEditMode;

            ViewBag.IsPublished = job.IsPublished;
            ViewBag.IsRejected = job.IsRejected;
            return View(model);
        }

        /// <summary>
        ///     Updates listed job by employer.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Employers")]
        [ValidateInput(false)]
        public ActionResult UpdateJob(JobListingModel model)
        {
            try
            {
                var employer = MemberService.Instance.Get(User.Username);
                var job = JobService.Instance.Get(model.Id);
                var profileList = new List<UserProfile>();

                string description = Sanitizer.GetSafeHtmlFragment(model.Description);
                description = description.RemoveNumbers();
                description = description.RemoveEmails();
                description = description.RemoveWebsites();

                string summary = model.Summary;
                summary = summary.RemoveNumbers();
                summary = summary.RemoveEmails();
                summary = summary.RemoveWebsites();

                string requirements = model.Requirements;
                requirements = requirements.RemoveEmails();
                requirements = requirements.RemoveNumbers();
                requirements = requirements.RemoveWebsites();
                string responsibilities = model.Responsibilities;
                responsibilities = responsibilities.RemoveEmails();
                responsibilities = responsibilities.RemoveNumbers();
                responsibilities = responsibilities.RemoveWebsites();

                if (job.IsPublished.Value == true && job.InEditMode == false)
                {
                    job.NewDescription = description;
                    job.NewSummary = summary;
                    job.NewRequirements = requirements;
                    job.NewResponsilibies = responsibilities;
                    job.NewMinimumExperience = (byte?)model.MinimumExperience;
                    job.NewMaximumExperience = (byte?)model.MaximumExperience;
                    job.NewCurrency = model.SalaryCurrency;
                    job.NewMinimumSalary = model.MinimumSalary;
                    job.NewMaximumSalary = model.MaximumSalary;
                    job.NewMinimumAge = (byte?)model.MinimumAge;
                    job.NewMaximumAge = (byte?)model.MaximumAge;
                    job.NewEmploymentTypeId = model.EmploymentType;
                    job.NewQualificationId = model.QualificationId;

                    job.InEditMode = true;
                }
                if (job.IsPublished.Value == false && job.InEditMode == true)
                {
                    job.NewDescription = description;
                    job.NewSummary = summary;
                    job.NewRequirements = requirements;
                    job.NewResponsilibies = responsibilities;
                    job.NewMinimumExperience = (byte?)model.MinimumExperience;
                    job.NewMaximumExperience = (byte?)model.MaximumExperience;
                    job.NewCurrency = model.SalaryCurrency;
                    job.NewMinimumSalary = model.MinimumSalary;
                    job.NewMaximumSalary = model.MaximumSalary;
                    job.NewMinimumAge = (byte?)model.MinimumAge;
                    job.NewMaximumAge = (byte?)model.MaximumAge;
                    job.NewEmploymentTypeId = model.EmploymentType;
                    job.NewQualificationId = model.QualificationId;
                    job.InEditMode = true;
                }
                if (job.IsPublished.Value == false && job.InEditMode == false)
                {
                    job.Description = description;
                    job.Summary = summary;
                    job.Requirements = requirements;
                    job.Responsilibies = responsibilities;
                    job.Title = model.Title;
                    job.CategoryId = model.CategoryId;
                    job.SpecializationId = model.SpecializationId;
                    job.CountryId = model.CountryId;
                    job.StateId = model.StateId;
                    job.City = model.City;
                    job.Zip = model.Zip;
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

                    job.InEditMode = false;
                }

                job.IsFeaturedJob = model.IsFeaturedJob;

                job.EmploymentTypeId = model.EmploymentType;
                job.QualificationId = model.QualificationId;
                job.MinimumAge = (byte?)model.MinimumAge;
                job.MaximumAge = (byte?)model.MaximumAge;
                job.MinimumExperience = (byte?)model.MinimumExperience;
                job.MaximumExperience = (byte?)model.MaximumExperience;
                job.MinimumSalary = model.MinimumSalary;
                job.MaximumSalary = model.MaximumSalary;
                job.Currency = model.SalaryCurrency;

                job.IsActive = true;
                job.IsDeleted = false;
                job.IsPublished = false;
                job.IsRejected = false;
                //job.IsExpired = false;
                job.IsSiteJob = true;

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    dataHelper.Update(job, User.Username);
                    var admin = (int)SecurityRoles.Administrator;
                    profileList = dataHelper.Get<UserProfile>().Where(x => x.Type == admin).ToList();
                }

                //Activity activity = new Activity()
                //{
                //    Comments = "Job Content Updated",
                //    ActivityDate = DateTime.Now,
                //    UserId = employer.UserId,
                //    DateUpdated = DateTime.Now,
                //    UpdatedBy = User.Username,
                //    Type = (int)ActivityTypes.JOB_LISTED,
                //    Unread = false
                //};
                //MemberService.Instance.Track(activity);

                using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/employer_postjob.html")))
                {
                    var body = string.Empty;

                    body = reader.ReadToEnd();
                    body = body.Replace("@@employer", employer.Company);
                    body = body.Replace("@@jobtitle", job.Title);
                    if (job.IsFeaturedJob.Value)
                    {
                        body = body.Replace("@@featured", "This is featuered job which will appear at main page as well as on top of search results!");
                    }
                    else
                    {
                        body = body.Replace("@@featured", "");
                    }
                    body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job.Id));

                    string[] receipent = { employer.Username };
                    var subject = string.Format("Thanks for Posting {0} Job", job.Title);

                    var recipients = new List<Recipient>();
                    recipients.Add(new Recipient
                    {
                        Email = employer.Username,
                        DisplayName = string.Format("{0} {1}", employer.FirstName, employer.LastName),
                        Type = RecipientTypes.TO
                    });

                    foreach (var profile in profileList)
                    {
                        recipients.Add(new Recipient
                        {
                            Email = profile.Username,
                            DisplayName = string.Format("{0} {1}", profile.FirstName, profile.LastName),
                            Type = RecipientTypes.BCC
                        });
                    }
                    AlertService.Instance.SendMail(subject, receipent, body);
                }

                TempData["UpdateData"] = string.Format("Job {0} has been updated successfully.", job.Title);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("UpdateJob", new { id = model.Id });
            }
        }

        /// <summary>
        /// Updates listed job by employer.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Employers")]
        [HttpGet]
        public ActionResult Repost(long id)
        {
            var job = JobService.Instance.Get(id);
            var model = new JobListingModel();
            model.Id = id;
            model.Title = job.Title;
            model.Summary = job.Summary;
            model.Description = job.Description;
            model.IsFeaturedJob = (bool)job.IsFeaturedJob;
            model.CategoryId = (int)job.CategoryId;
            model.SpecializationId = (int)job.SpecializationId;
            model.CountryId = job.CountryId.Value;
            model.StateId = job.StateId;
            model.City = job.City;
            model.Zip = job.Zip == null ? "" : Convert.ToInt32(job.Zip).ToString();
            if (job.EmploymentTypeId != null)
            {
                model.EmploymentType = (long)job.EmploymentTypeId;
            }
            if (job.QualificationId != null)
            {
                model.QualificationId = (long)job.QualificationId;
            }
            model.MinimumAge = (byte?)job.MinimumAge;
            model.MaximumAge = (byte?)job.MaximumAge;
            model.MinimumExperience = (byte?)job.MinimumExperience;
            model.MaximumExperience = (byte?)job.MaximumExperience;
            if (job.MinimumSalary != null)
            {
                model.MinimumSalary = (decimal)job.MinimumSalary;
            }
            if (job.MaximumSalary != null)
            {
                model.MaximumSalary = (decimal)job.MaximumSalary;
            }
            model.SalaryCurrency = job.Currency;

            return View(model);
        }

        /// <summary>
        /// Updates listed job by employer.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Employers")]
        public ActionResult Repost(JobListingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var employer = MemberService.Instance.Get(User.Username);
                    var job = JobService.Instance.Get(model.Id);

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

                    string description = Sanitizer.GetSafeHtmlFragment(model.Description);
                    description = description.RemoveNumbers();
                    description = description.RemoveEmails();
                    description = description.RemoveWebsites();
                    job.Description = description;

                    string summary = model.Summary;
                    summary = summary.RemoveNumbers();
                    summary = summary.RemoveEmails();
                    summary = summary.RemoveWebsites();

                    string requirements = model.Requirements;
                    if (!string.IsNullOrEmpty(requirements))
                    {
                        requirements = requirements.RemoveNumbers();
                        requirements = requirements.RemoveEmails();
                        requirements = requirements.RemoveWebsites();
                    }
                    string responsibilities = model.Responsibilities;
                    if (!string.IsNullOrEmpty(responsibilities))
                    {
                        responsibilities = responsibilities.RemoveNumbers();
                        responsibilities = responsibilities.RemoveEmails();
                        responsibilities = responsibilities.RemoveWebsites();
                    }
                    var repost_job = new Job
                    {
                        Title = model.Title,
                        Summary = summary,
                        Description = description,
                        Requirements = requirements,
                        Responsilibies = responsibilities,
                        IsFeaturedJob = model.IsFeaturedJob,
                        CountryId = model.CountryId,
                        StateId = model.StateId,
                        City = model.City,
                        Zip = model.Zip,
                        EmployerId = employer.UserId,
                        EmploymentTypeId = model.EmploymentType,
                        QualificationId = model.QualificationId,
                        MinimumAge = (byte?)model.MinimumAge,
                        MaximumAge = (byte?)model.MaximumAge,
                        MinimumExperience = (byte?)model.MinimumExperience,
                        MaximumExperience = (byte?)model.MaximumExperience,
                        MinimumSalary = model.MinimumSalary,
                        MaximumSalary = model.MaximumSalary,
                        Currency = model.SalaryCurrency,
                        CategoryId = model.CategoryId,
                        SpecializationId = model.SpecializationId,
                        PublishedDate = DateTime.Now,
                        ClosingDate = DateTime.Now.AddMonths(1),
                        PermaLink = permalink,
                        IsPostedOnTwitter = false,
                        InEditMode = false
                    };

                    long job_id = 0;
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        dataHelper.DeleteUpdate<Job>(job, User.Username);
                        job_id = Convert.ToInt64(dataHelper.Add(repost_job, User.Username));
                    }

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

                        using (JobPortalEntities context = new JobPortalEntities())
                        {
                            DataHelper dataHelper = new DataHelper(context);
                            dataHelper.Add(tracking);

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
                                body = body.Replace("@@joburl", string.Format("{0}://{1}/job/{2}-{3}", Request.Url.Scheme, Request.Url.Authority, job.PermaLink, job_id));

                                string[] receipent = { employer.Username };
                                var subject = string.Format("Thanks for Posting {0} Job", job.Title);

                                var recipients = new List<Recipient>();
                                recipients.Add(new Recipient
                                {
                                    Email = employer.Username,
                                    DisplayName = string.Format("{0} {1}", employer.FirstName, employer.LastName),
                                    Type = RecipientTypes.TO
                                });
                                var super = (int)SecurityRoles.SuperUser;
                                var profile = dataHelper.GetSingle<UserProfile>("Type", super);
                                recipients.Add(new Recipient
                                {
                                    Email = profile.Username,
                                    DisplayName = string.Format("{0} {1}", profile.FirstName, profile.LastName),
                                    Type = RecipientTypes.BCC
                                });
                                AlertService.Instance.SendMail(subject, receipent, body);
                            }
                        }
                    }
                    TempData["SaveData"] = string.Format("{0} job has been re-posted successfully.", model.Title);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Repost", new { id = model.Id });
            }
            return View(model);
        }

        [Authorize]
        public ActionResult DownloadTemplate()
        {
            var filePath = Server.MapPath("~/Templates/");
            var template = ConfigService.Instance.GetConfigValue("UploadJobsTemplate");
            filePath = string.Format("{0}{1}", filePath, template);

            SharedService.Instance.GenerateUplodJobTemplate(filePath);

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = template;

            return File(fileBytes, MediaTypeNames.Application.Octet, fileName);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Employers")]
        [UrlPrivilegeFilter]
        public ActionResult Bookmarks(string ResumeTitle, long? CountryId, string Type, string StartDate, string EndDate, int pageNumber = 0)
        {
            var bookmarks = new List<Tracking>();
            int rows = 0;
            int pageSize = 10;
            SelectList ResumeList;
            if (User != null)
            {
                var employer = MemberService.Instance.Get(User.Username);
                var bookmarked = (int)TrackingTypes.BOOKMAKRED;

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    var result = dataHelper.Get<Tracking>().Where(x => x.UserId == employer.UserId && x.Type == bookmarked && x.IsDeleted == false);
                    if (!string.IsNullOrEmpty(ResumeTitle))
                    {
                        result = result.Where(x => (x.Resume != null ? x.Resume.Title : x.Jobseeker.Title).ToLower().Contains(ResumeTitle.ToLower()));
                    }

                    if (CountryId != null)
                    {
                        result = result.Where(x => x.Jobseeker.CountryId == CountryId);
                    }

                    if (!string.IsNullOrEmpty(Type))
                    {
                        if (Type.Equals("Jobseeker"))
                        {
                            result = result.Where(x => x.Jobseeker != null);
                        }
                        else
                        {
                            result = result.Where(x => x.Resume != null);
                        }
                    }
                    rows = result.Count();
                    bookmarks = result.OrderByDescending(x => x.DateUpdated).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize)).Take(pageSize).ToList();
                    ResumeList = new SelectList((from l in bookmarks group l by new { Title = (l.Resume != null ? l.Resume.Title : l.Jobseeker.Title) } into g select g.Key.Title).ToList());
                }

                ViewBag.CountryList = new SelectList(SharedService.Instance.GetCountryList(), "Id", "Text");
                ViewBag.ResumeList = ResumeList;
                ViewBag.TypeList = new SelectList(new List<string> { "Jobseeker", "Resume" });

                ViewBag.Rows = rows;
                ViewBag.Model = new StaticPagedList<Tracking>(bookmarks, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
            }
            return View(bookmarks);
        }

        public ActionResult RemoveBookMarks(long id)
        {
            TempData["Error"] = "Bookmarked removed Succesfully!";
            return RedirectToAction("BookmarkedResumes");
        }




        [Authorize(Roles = "Employers")]
        [UrlPrivilegeFilter]
        public ActionResult Applications(ApplicationSearchModel model, int pageNumber = 0)
        {

            UserInfoEntity uinfo = _service.Get(User.Id);
            if (!uinfo.IsConfirmed)
            {
                return RedirectToAction("Confirm", "Account", new { id = uinfo.Id, returnUrl = Request.Url.ToString() });
            }

            int pageSize = 10;
            int rows = 0;
            List<ApplicationListModel> list = new List<ApplicationListModel>();
            var TypeList = new List<int>();
            TypeList.Add((int)TrackingTypes.AUTO_MATCHED);
            TypeList.Add((int)TrackingTypes.APPLIED);
            TypeList.Add((int)TrackingTypes.WITHDRAWN);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                var result = dataHelper.Get<Tracking>().Where(x => TypeList.Contains(x.Type) && x.IsDeleted == false && x.Job.EmployerId == User.Id && x.JobseekerId != null);

                if (!string.IsNullOrEmpty(model.Title))
                {
                    result = result.Where(x => x.Job.Title == model.Title);
                }

                if (model.CountryId != null)
                {
                    result = result.Where(x => x.Job.CountryId == model.CountryId);
                }

                if (!string.IsNullOrEmpty(model.Type))
                {
                    if (model.Type.Equals("Jobseeker"))
                    {
                        result = result.Where(x => x.Jobseeker != null);
                    }
                }

                if (model.Status != null)
                {
                    result = result.Where(x => x.Type == model.Status);
                }

                if ((!string.IsNullOrEmpty(model.FromDay) && !string.IsNullOrEmpty(model.FromMonth) && !string.IsNullOrEmpty(model.FromYear)) || (!string.IsNullOrEmpty(model.ToDay) && !string.IsNullOrEmpty(model.ToMonth) && !string.IsNullOrEmpty(model.ToYear)))
                {
                    var sdt = new DateTime();
                    var edt = new DateTime();

                    if ((!string.IsNullOrEmpty(model.FromDay) && !string.IsNullOrEmpty(model.FromMonth) && !string.IsNullOrEmpty(model.FromYear)))
                    {
                        string startDate = string.Format("{0}/{1}/{2}", model.FromMonth, model.FromDay, model.FromYear);
                        sdt = Convert.ToDateTime(startDate);
                    }

                    if ((!string.IsNullOrEmpty(model.ToDay) && !string.IsNullOrEmpty(model.ToMonth) && !string.IsNullOrEmpty(model.ToYear)))
                    {
                        if ((string.IsNullOrEmpty(model.FromDay) && string.IsNullOrEmpty(model.FromMonth) && string.IsNullOrEmpty(model.FromYear)))
                        {
                            sdt = DateTime.Now;
                        }
                        string endDate = string.Format("{0}/{1}/{2}", model.ToMonth, model.ToDay, model.ToYear);
                        edt = Convert.ToDateTime(endDate);
                    }
                    else
                    {
                        if ((!string.IsNullOrEmpty(model.FromDay) && !string.IsNullOrEmpty(model.FromMonth) && !string.IsNullOrEmpty(model.FromYear)))
                        {
                            edt = DateTime.Now;
                        }
                    }
                    result = result.Where(x => (x.DateUpdated.Day >= sdt.Day && x.DateUpdated.Month >= sdt.Month && x.DateUpdated.Year >= sdt.Year) && (x.DateUpdated.Day <= edt.Day && x.DateUpdated.Month <= edt.Month && x.DateUpdated.Year <= edt.Year));
                }

                list = result.GroupBy(x => x.Job).Select(x => new ApplicationListModel() { JobId = x.Key.Id, Title = x.Key.Title, PermaLink = x.Key.PermaLink, Counts = x.Count() }).ToList();

                rows = result.Count();
                if (rows > 0)
                {
                    list = list.OrderByDescending(x => x.JobId).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize)).Take(pageSize).ToList();
                }
                ViewBag.JobList = new SelectList((from l in list select l.Title).AsEnumerable());
            }

            ViewBag.CountryList = new SelectList(SharedService.Instance.GetCountryList(), "Id", "Text");
            ViewBag.Model = new StaticPagedList<ApplicationListModel>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
            ViewBag.Rows = rows;

            return View(list);
        }

        [Authorize(Roles = "Employers")]
        public ActionResult ApplicationDetails(long Id, int pageNumber = 1)
        {
            List<ExtendedTrackingEntity> list = new List<ExtendedTrackingEntity>();
            int pageSize = 10;
            int rows = 0;
            _service.ApplicationMarkAsViewed(Id);
            list = JobSeekerService.Instance.JobApplicationList(Id, null, null, null, null, null, null, pageNumber, pageSize);
            if (list.Count > 0)
            {
                rows = list.First().MaxRows;
            }
            ViewBag.Rows = rows;
            ViewBag.Model = new StaticPagedList<ExtendedTrackingEntity>(list, pageNumber, pageSize, rows);
            Job job = JobService.Instance.Get(Id);
            ViewBag.Job = job;

            return View(list);
        }

        public ActionResult JobList(long Id, int pageNumber = 0)
        {
            if (UserInfo != null && !UserInfo.IsConfirmed)
            {
                //return RedirectToAction("ConfirmRegistration", "Account");
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
            }

            int pageSize = 10;
            int rows = 0;
            List<Job> list = new List<Job>();
            UserProfile employer = new UserProfile();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                employer = dataHelper.GetSingle<UserProfile>("UserId", Id);
                if (employer != null)
                {
                    var result = employer.Jobs.Where(x => x.IsActive == true && x.IsDeleted == false && x.IsPublished == true);
                    list = result.ToList();
                    list = list.Where(x => x.IsExpired.Value == false).ToList();
                    rows = list.Count();
                    if (rows > 0)
                    {
                        list = list.OrderByDescending(x => x.Id).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize)).Take(pageSize).ToList();
                    }
                }
            }
            ViewBag.Employer = employer;
            ViewBag.Model = new StaticPagedList<Job>(list, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
            ViewBag.Rows = rows;

            return View(list);
        }

        [Authorize(Roles = "Employers")]
        [UrlPrivilegeFilter]
        public ActionResult ListJob()
        {
            if (UserInfo != null)
            {
                if (UserInfo.IsConfirmed == false)
                {
                    return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
                }

                if (UserInfo.Role != SecurityRoles.Employers)
                {
                    return Redirect(UserInfo.PermaLink);
                }
            }

            if (string.IsNullOrEmpty(UserInfo.Address) && string.IsNullOrEmpty(UserInfo.Mobile))
            {
                return RedirectToAction("ListJobError", "Employer", new { returnUrl = "/listjob" });
            }

            JobListingModel model = new JobListingModel();
            if (!string.IsNullOrEmpty(Request.QueryString["sid"]))
            {
                Guid id = new Guid(Request.QueryString["sid"]);
                string jobData = DomainService.Instance.ReadData(id);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                JobPostModel jpe = serializer.Deserialize<JobPostModel>(jobData);

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

        [Authorize(Roles = "Employers, Jobseeker")]
        public ActionResult PostListJob(int packageId, long countryId, string returnUrl)
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


        [Authorize(Roles = "Employers")]
        [HttpGet]
        public ActionResult ListJobError(string returnUrl = null)
        {
            if (UserInfo.Role != SecurityRoles.Employers)
            {
                return Redirect(UserInfo.PermaLink);
            }

            EmployerRequiredModel model = new EmployerRequiredModel(UserInfo.Id);
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        [Authorize(Roles = "Employers")]
        [HttpPost]
        public ActionResult ListJobError(EmployerRequiredModel model)
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
                if (model.ReturnUrl.ToLower().Contains("listjob"))
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

        [Authorize(Roles = "Employers")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ListJob(JobListingModel model)
        {
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
                return RedirectToAction("Index");
            }
            return View(new JobListingModel());
        }

        [Authorize(Roles = "Employers")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PostJob(JobListingModel model)
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

        //[Authorize(Roles = "Company, Institute")]
        [ValidateInput(false)]
        public ActionResult InsetProfile(int id)
        {
            //if (UserInfo.Role != SecurityRoles.Employers && UserInfo.Role != SecurityRoles.Institute)
            //{
            //    return Redirect(UserInfo.PermaLink);
            //}


            var employer = MemberService.Instance.Get(id);
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
        [HttpPost]
        [Authorize(Roles = "Employers, Institute")]
        public ActionResult InsetProfile(EmployerModel model)
        {
            if (string.IsNullOrEmpty(model.Company))
            {
                TempData["Error"] = "Company Name should not be empty!";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var original = MemberService.Instance.Get(model.Id);
                model.Company = model.Company.TitleCase();

                original.Company = model.Company;
                original.CategoryId = model.CategoryId;
                string summary = model.Overview;
                summary = summary.RemoveEmails();
                summary = summary.RemoveNumbers();
                summary = summary.RemoveWebsites();
                original.Summary = summary;
                if (!string.IsNullOrEmpty(model.FirstName))
                {
                    original.FirstName = model.FirstName.TitleCase();
                }
                if (!string.IsNullOrEmpty(original.LastName))
                {
                    original.LastName = model.LastName.TitleCase();
                }
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
                original.IsFeatured = model.IsFeatured;
                model.PremaLink = original.PermaLink;
                original.Facebook = model.Facebook;
                original.Twitter = model.Twitter;
                original.LinkedIn = model.LinkedIn;
                original.GooglePlus = model.GooglePlus;

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    dataHelper.Update(original, User.Username);
                }
                TempData["UpdateData"] = "Your profile has been updated successfully!";
                return View(model);
            }
            return View(model);
        }

        [Authorize(Roles = "Employers, Institute,RecruitmentAgency")]
        [UrlPrivilegeFilter]
        public ActionResult UpdateProfile()
        {
            if (UserInfo.Role != SecurityRoles.Employers && UserInfo.Role != SecurityRoles.Institute && UserInfo.Role != SecurityRoles.RecruitmentAgency)
            {
                return Redirect(UserInfo.PermaLink);
            }


            var employer = MemberService.Instance.Get(User.Id);
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


        [HttpPost]
        [Authorize(Roles = "Employers, Institute")]
        public ActionResult UpdateProfile(EmployerModel model)
        {
            if (string.IsNullOrEmpty(model.Company))
            {
                TempData["Error"] = "Company Name should not be empty!";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var original = MemberService.Instance.Get(model.Id);
                model.Company = model.Company.TitleCase();

                original.Company = model.Company;
                original.CategoryId = model.CategoryId;
                string summary = model.Overview;
                summary = summary.RemoveEmails();
                summary = summary.RemoveNumbers();
                summary = summary.RemoveWebsites();
                original.Summary = summary;
                if (!string.IsNullOrEmpty(model.FirstName))
                {
                    original.FirstName = model.FirstName.TitleCase();
                }
                if (!string.IsNullOrEmpty(original.LastName))
                {
                    original.LastName = model.LastName.TitleCase();
                }
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
                original.IsFeatured = model.IsFeatured;
                model.PremaLink = original.PermaLink;
                original.Facebook = model.Facebook;
                original.Twitter = model.Twitter;
                original.LinkedIn = model.LinkedIn;
                original.GooglePlus = model.GooglePlus;

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    dataHelper.Update(original, User.Username);
                }
                TempData["UpdateData"] = "Your profile has been updated successfully!";
                return View(model);
            }
            return View(model);
        }

        [Authorize(Roles = "Employers")]
        [UrlPrivilegeFilter]
        public ActionResult UploadJobs()
        {
            if (UserInfo != null && !UserInfo.IsConfirmed)
            {
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Employers")]
        public ActionResult UploadJobs(HttpPostedFileBase file)
        {
            var job = new Job();
            if (file != null)
            {
                var ZipCode = "0";
                var title = string.Empty;
                var summary = string.Empty;
                string[] category = null;
                string[] education = null;
                string[] qualiication = null;

                string[] specialization = null;
                string[] country = null;
                var state = string.Empty;
                var city = string.Empty;


                HSSFWorkbook hssfwb;
                using (var fileStream = file.InputStream)
                {
                    hssfwb = new HSSFWorkbook(fileStream);
                }

                var sheet = hssfwb.GetSheet("UploadJobs");
                if (sheet.LastRowNum > 0 && sheet.LastRowNum < 101)
                {
                    for (var row = 1; row <= sheet.LastRowNum; row++)
                    {
                        if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                        {
                            job.Title = sheet.GetRow(row).GetCell(0).StringCellValue;
                            job.Description = sheet.GetRow(row).GetCell(1).StringCellValue;
                            job.Summary = sheet.GetRow(row).GetCell(1).StringCellValue;
                            if (!string.IsNullOrEmpty(sheet.GetRow(row).GetCell(2).StringCellValue))
                            {
                                category = sheet.GetRow(row).GetCell(2).StringCellValue.Split('-');
                                job.CategoryId = Convert.ToInt32(category[0]);
                            }

                            if (!string.IsNullOrEmpty(sheet.GetRow(row).GetCell(3).StringCellValue))
                            {
                                specialization = sheet.GetRow(row).GetCell(3).StringCellValue.Split('-');
                                job.SpecializationId = Convert.ToInt32(specialization[0]);
                            }

                            if (!string.IsNullOrEmpty(sheet.GetRow(row).GetCell(4).StringCellValue))
                            {
                                country = sheet.GetRow(row).GetCell(4).StringCellValue.Split('-');
                                job.CountryId = Convert.ToInt32(country[0]);
                            }

                            if (!string.IsNullOrEmpty(sheet.GetRow(row).GetCell(5).StringCellValue))
                            {
                                state = sheet.GetRow(row).GetCell(5).StringCellValue;
                                List entity = SharedService.Instance.GetState(state);
                                if (entity != null)
                                {
                                    long? StateId = entity.Id;
                                    job.StateId = StateId;
                                }
                            }
                            job.City = sheet.GetRow(row).GetCell(6).StringCellValue;

                            try
                            {
                                ZipCode = sheet.GetRow(row).GetCell(7).NumericCellValue.ToString();
                                if (ZipCode.Length >= 5)
                                    job.Zip = ZipCode;
                                else
                                    job.Zip = "";
                                job.MinimumSalary = Convert.ToDecimal(sheet.GetRow(row).GetCell(12).NumericCellValue);
                                job.MaximumSalary = Convert.ToDecimal(sheet.GetRow(row).GetCell(13).NumericCellValue);
                            }
                            catch (Exception)
                            {
                                job.Zip = "";
                                job.MinimumSalary = 0;
                                job.MaximumSalary = 0;
                            }

                            job.MinimumExperience = Convert.ToByte(sheet.GetRow(row).GetCell(8).StringCellValue);
                            job.MaximumExperience = Convert.ToByte(sheet.GetRow(row).GetCell(9).StringCellValue);
                            job.MinimumAge = Convert.ToByte(sheet.GetRow(row).GetCell(10).StringCellValue);
                            job.MaximumAge = Convert.ToByte(sheet.GetRow(row).GetCell(11).StringCellValue);

                            if (!string.IsNullOrEmpty(sheet.GetRow(row).GetCell(14).StringCellValue))
                            {
                                education = sheet.GetRow(row).GetCell(14).StringCellValue.Split('-');
                                job.EmploymentTypeId = Convert.ToInt32(education[0]);
                            }
                            if (!string.IsNullOrEmpty(sheet.GetRow(row).GetCell(15).StringCellValue))
                            {
                                qualiication = sheet.GetRow(row).GetCell(15).StringCellValue.Split('-');
                                job.QualificationId = Convert.ToInt32(qualiication[0]);
                            }
                            job.PublishedDate = DateTime.Now;
                            job.ClosingDate = DateTime.Now.AddDays(30);
                            job.EmployerId = MemberService.Instance.Get(User.Username).UserId;

                            job.Currency = null;
                            job.IsActive = true;
                            job.IsPublished = false;
                            job.IsDeleted = false;
                            job.IsSiteJob = true;
                            job.IsRejected = false;
                            job.CreatedBy = User.Username;
                            job.UpdatedBy = User.Username;

                            if (job.MinimumSalary == 0 || job.MaximumSalary == 0 || job.Zip == "")
                            {
                                TempData["Error"] =
                                    "If there is some invalid data in uploaded file, so that data may not be upload!";
                            }
                            else
                            {
                                using (JobPortalEntities context = new JobPortalEntities())
                                {
                                    DataHelper dataHelper = new DataHelper(context);
                                    var job_id = dataHelper.Add(job, User.Username, "Id");
                                    if (job_id != null)
                                    {
                                        var jobid = Convert.ToInt64(job_id);
                                        job = dataHelper.GetSingle<Job>(jobid);

                                        var premalink = string.Format("{0}", job.Title);
                                        premalink = premalink.Replace(" & ", "-");
                                        premalink = premalink.Replace("/", "-");
                                        premalink = premalink.Replace(", ", "-");
                                        premalink = premalink.Replace(" ", "-");

                                        job.PermaLink = premalink.ToLower();

                                        dataHelper.Update(job, User.Username);
                                    }
                                    TempData["SaveData"] = "Jobs has been uploaded successfully.";
                                }
                            }
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "Your Excel file is empty or File is not selected, Please fill it carefully.";
                    return View();
                }
            }
            return View();
        }

        /// <summary>
        ///     Shortlist application
        /// </summary>
        /// <param name="JobId"></param>
        /// <param name="ResumeId"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        [Authorize(Roles = "Employers")]
        public ActionResult Delete(Guid Id, string redirect)
        {
            if (User != null)
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    var record = dataHelper.GetSingle<Tracking>(Id);
                    if (record != null)
                    {
                        dataHelper.Delete(record);
                        TempData["UpdateData"] = "Removed successfully!";
                    }
                    else
                    {
                        TempData["Error"] = "Record not found for deletion!";
                    }
                }
            }
            return Redirect(redirect);
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists.";

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

        /// <summary>
        ///     Shortlist Application/Resume
        /// </summary>
        /// <param name="JobId"></param>
        /// <param name="ResumeId"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        [Authorize(Roles = "Employers")]
        public ActionResult ShortList(Guid Id, string redirect)
        {
            if (User != null)
            {
                var employer = MemberService.Instance.Get(User.Username);
                var message = string.Empty;
                EmployerService.Instance.Shortlist(Id, User.Username, out message);

                TempData["UpdateData"] = message;
            }
            return RedirectToAction(redirect);
        }

        /// <summary>
        ///     Reject application
        /// </summary>
        /// <param name="JobId"></param>
        /// <param name="ResumeId"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        [Authorize(Roles = "Employers")]
        public ActionResult Reject(Guid Id, string redirect, long InterviewId = 0)
        {
            var message = string.Empty;
            EmployerService.Instance.Reject(Id, User.Username, out message);
            TempData["UpdateData"] = message;

            return RedirectToAction(redirect);
        }

        /// <summary>
        ///     Use this method to select jobseeker.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        [Authorize(Roles = "Employers")]
        public ActionResult Select(Guid Id, string redirect, long InterviewId = 0)
        {
            var message = string.Empty;
            EmployerService.Instance.Select(Id, User.Username, out message);
            if (!string.IsNullOrEmpty(message))
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    var interview = dataHelper.GetSingle<Interview>(InterviewId);
                    if (interview != null)
                    {
                        interview.Status = (int)InterviewStatus.SELECTED;
                        interview.DateUpdated = DateTime.Now;
                        dataHelper.Update(interview);
                    }
                }
            }
            TempData["UpdateData"] = "Selected successfully!";

            return Redirect(redirect);
        }

        [Authorize(Roles = "Employers")]
        public ActionResult BookmarkJobseeker(long Id, string redirect)
        {
            var message = string.Empty;
            UserInfoEntity profile = _service.Get(User.Id);
            if (User != null)
            {
                if (profile.IsConfirmed == false)
                {
                    return RedirectToAction("Confirm", "Account", new { id = profile.Id, returnUrl = Request.Url.ToString() });
                }

                if (profile.Type == (int)SecurityRoles.Employers)
                {
                    var bookmark = iTrackingService.Bookmark(Id, (int)BookmarkedTypes.JOBSEEKER, profile.Id, out message);

                    //var record = TrackingService.Instance.Bookmark(Id, BookmarkedTypes.JOBSEEKER, profile.Username, out message);
                    TempData["UpdateData"] = message;
                }
                else
                {
                    TempData["Error"] = "Only Employers can bookmark jobseeker!";
                }
            }
            return Redirect(redirect);
        }

        [Authorize(Roles = "Employers")]
        public ActionResult BookmarkResume(long Id, string redirect)
        {
            var message = string.Empty;
            if (UserInfo != null && UserInfo.IsConfirmed == false)
            {
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
            }

            if (User != null)
            {
                UserProfile profile = MemberService.Instance.Get(User.Username);
                if (profile.Type == (int)SecurityRoles.Employers)
                {
                    var record = TrackingService.Instance.Bookmark(Id, BookmarkedTypes.RESUME, User.Username, out message);
                    TempData["UpdateData"] = message;
                }
                else
                {
                    TempData["Error"] = "Only Employers can bookmark resume!";
                }
            }
            return Redirect(redirect);
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        public ActionResult ChangeWebAddress(string address)
        {
            string status = "Failed";
            try
            {
                UserProfile profile = MemberService.Instance.Get(User.Username);
                profile.PermaLink = address;
                MemberService.Instance.Update(profile);
                status = "Success";
            }
            catch (Exception)
            {
                status = "Failed";
            }
            return Json(status);
        }

    }
}