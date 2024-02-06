using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Web;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Text;
using System.Linq.Expressions;
using System.Data.SqlClient;
using System.Configuration;
#pragma warning disable CS0234 // The type or namespace name 'Web' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Web.Models;
#pragma warning restore CS0234 // The type or namespace name 'Web' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Data.Common;

namespace JobPortal.Domain
{
#pragma warning disable CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    public class SharedService : DataService
#pragma warning restore CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    {
        private static volatile SharedService instance;
        private static readonly object sync = new object();

        /// <summary>
        /// Default private constructor
        /// </summary>
        private SharedService()
        {
        }

        /// <summary>
        /// Gets the Single Instance of SharedService
        /// </summary>
        public static SharedService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new SharedService();
                        }
                    }
                }
                return instance;
            }
        }

        public string GetBlockLink(string Username, string email, string cssClass = "btn btn-info")
        {
            string link = string.Empty;

            UserProfile profile = MemberService.Instance.Get(Username);
            UserProfile emailProfile = MemberService.Instance.Get(email);
            SecurityRoles type = (SecurityRoles)profile.Type;
            bool flag = IsConnected(email, profile.UserId);

            switch (type)
            {
                case SecurityRoles.Jobseeker:
                    link = string.Format("<a href=\"#\" data-href=\"/Home/Block?email={0}\" data-name=\"{1} {2}\" data-role=\"{3}\" role=\"button\" data-toggle=\"modal\" data-target=\"#cDialog\" data-connected=\"{4}\" class=\"{5} aBlock\" style=\"height: 20px; width:60px; font-size:8pt; padding:3px;\">Block</a>", emailProfile.Username, emailProfile.FirstName, emailProfile.LastName, "Individual", flag, cssClass);
                    break;
                case SecurityRoles.Employers:
                    link = string.Format("<a href=\"#\" data-href=\"/Home/Block?email={0}\" data-name=\"{1}\" data-role=\"{2}\" role=\"button\" data-toggle=\"modal\" data-target=\"#cDialog\" data-connected=\"{3}\" class=\"{4} aBlock\" style=\"height: 20px; width:60px; font-size:8pt; padding:3px;\">Block</a>", emailProfile.Username, emailProfile.Company, "Company", flag, cssClass);
                    break;
            }
            return link;
        }

        /// <summary>
        /// Gets the meta data.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'MetaData' could not be found (are you missing a using directive or an assembly reference?)
        public MetaData GetMetaData(string page)
#pragma warning restore CS0246 // The type or namespace name 'MetaData' could not be found (are you missing a using directive or an assembly reference?)
        {
            MetaData entity = new MetaData();
            entity = ReadSingleData<MetaData>(string.Format("SELECT PageTitle Title, PageDescription [Description], PageKeywords Keywords FROM SitePages WHERE PageId='{0}'", page));

            //using (JobPortalEntities context = new JobPortalEntities())
            //{
            //    DataHelper dataHelper = new DataHelper(context);
            //    SitePage sp = dataHelper.Get<SitePage>().SingleOrDefault(x => x.PageId.ToLower().Equals(page.ToLower()));
            //    entity.Title = sp.PageTitle;
            //    entity.Description = sp.PageDescription;
            //    entity.Keywords = sp.PageKeywords;
            //}
            return entity;
        }

        public bool IsConnected(string email, long requestor)
        {
            bool flag = false;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                List<Parameter> parameters = new List<Parameter>();
                parameters.Add(new Parameter() { Name = "EmailAddress", Value = email, Comparison = ParameterComparisonTypes.Equals });
                parameters.Add(new Parameter() { Name = "UserId", Value = requestor, Comparison = ParameterComparisonTypes.Equals });
                parameters.Add(new Parameter() { Name = "IsConnected", Value = true, Comparison = ParameterComparisonTypes.Equals });
                var connected = dataHelper.GetSingle<Connection>(parameters);
                flag = (connected != null);
            }
            return flag;
        }

#pragma warning disable CS0246 // The type or namespace name 'Connection' could not be found (are you missing a using directive or an assembly reference?)
        public Connection Get(string contactEmail, string loggedInUserEmail)
#pragma warning restore CS0246 // The type or namespace name 'Connection' could not be found (are you missing a using directive or an assembly reference?)
        {
            Connection connection = new Connection();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", contactEmail);
                UserProfile loggedInUser = dataHelper.GetSingle<UserProfile>("Username", loggedInUserEmail);

                Hashtable parameters = new Hashtable();
                parameters.Add("UserId", loggedInUser.UserId);
                parameters.Add("EmailAddress", contactEmail);
                connection = dataHelper.GetSingle<Connection>(parameters);
            }

            return connection;
        }

        public void Invite(string Username, string baseUrl)
        {
            UserProfile profile = MemberService.Instance.Get(Username);
            List<Parameter> parameters = new List<Parameter>();
            List<UserProfile> user_list = new List<UserProfile>();

            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            string image = string.Empty;
            string name = string.Empty;
            string profileName = string.Empty;
            string body = string.Empty;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                var blockedList = dataHelper.Get<BlockedPeople>().Where(x => x.BlockedId == profile.UserId).Select(x => x.BlockerId);

                int company = (int)SecurityRoles.Employers;
                var companies = dataHelper.Get<UserProfile>().Where(x => x.IsActive == true && x.IsDeleted == false && x.Type == company && x.UserId != profile.UserId);
                if (blockedList.Any())
                {
                    companies = companies.Where(x => !blockedList.Contains(x.UserId));
                }

                int individual = (int)SecurityRoles.Jobseeker;
                var individuals = dataHelper.Get<UserProfile>().Where(x => x.IsActive == true && x.IsDeleted == false && x.Type == individual && x.UserId != profile.UserId);
                if (blockedList.Any())
                {
                    individuals = individuals.Where(x => !blockedList.Contains(x.UserId));
                }

                image = string.Format("<img src=\"{0}/image/avtar?Id={1}\" style=\"width:64px; height:64px\"/>", baseUrl, profile.UserId);


                if (profile.Type == (int)SecurityRoles.Employers)
                {
                    profileName = !string.IsNullOrEmpty(profile.Company) ? profile.Company : string.Format("{0} {1}", profile.FirstName, profile.LastName);
                }
                else if (profile.Type == (int)SecurityRoles.Jobseeker)
                {
                    profileName = string.Format("{0} {1}", profile.FirstName, profile.LastName);
                }

                sb1.AppendFormat("<div><div style=\"float:left; width:64px; height:64px; border:1px solid #d7d7d7\"><a href=\"{0}/{1}\" target=\"_blank\">{2}</a></div>", baseUrl, profile.PermaLink, image);
                sb1.Append("<div style=\"float:left; padding-left:20px; height:64px;\">");
                sb1.AppendFormat("<div><a href=\"{0}/connect?EmailAddress={1}&via=email\" style=\"padding: 10px;  background-color: #01a7e1; text-decoration: none; color: #fff; -webkit-border-radius: 4px; border-radius: 4px; width: 100px; display: block; text-align: center;\" target=\"_blank\">Connect</a></div>", baseUrl, profile.Username);
                sb1.Append("</div><div style=\"clear:both; padding-bottom:65px;\"></div></div>");

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
                        (x.LastName == profile.LastName && x.CountryId == profile.CountryId)
                        || ((x.Address != null || x.Zip != null) && x.Address == profile.Address && x.City == profile.City && x.StateId == profile.StateId && x.Zip == profile.Zip && x.CountryId == profile.CountryId)
                        || (x.Phone != null && x.Phone == profile.Phone && x.CountryId == profile.CountryId)
                        || (x.School != null && x.School == profile.School && x.CountryId == profile.CountryId)
                        || (x.University != null && x.University == profile.University)
                        || ((profile.CategoryId != null && x.CategoryId == profile.CategoryId) && (profile.SpecializationId != null && x.SpecializationId == profile.SpecializationId)
                         && profile.CurrentEmployer != null && (x.CurrentEmployer.ToLower().Contains(cemployer) || x.CurrentEmployer.ToLower().Contains(pemployer))
                                || profile.PreviousEmployer != null && (x.PreviousEmployer.ToLower().Contains(cemployer) || x.PreviousEmployer.ToLower().Contains(pemployer)) && x.CountryId == profile.CountryId));

                    user_list.AddRange(individuals.ToList());

                    companies = companies.Where(x =>
                       (x.LastName == profile.LastName && x.CountryId == profile.CountryId)
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
                     (x.LastName == profile.LastName && x.CountryId == profile.CountryId)
                     || ((x.Address != null || x.Zip != null) && x.Address == profile.Address && x.City == profile.City && x.StateId == profile.StateId && x.Zip == profile.Zip && x.CountryId == profile.CountryId)
                     || (x.Phone != null && x.Phone == profile.Phone && x.CountryId == profile.CountryId)
                     || (profile.Company != null && x.Company.ToLower().Contains(cname) && x.CountryId == profile.CountryId));

                    user_list.AddRange(companies.ToList());

                    individuals = individuals.Where(x =>
                       (x.LastName == profile.LastName && x.CountryId == profile.CountryId)
                       || ((x.Address != null || x.Zip != null) && x.Address == profile.Address && x.City == profile.City && x.StateId == profile.StateId && x.Zip == profile.Zip && x.CountryId == profile.CountryId)
                       || (x.Phone != null && x.Phone == profile.Phone && x.CountryId == profile.CountryId)
                       || ((x.CurrentEmployer != null && x.CurrentEmployer.ToLower().Contains(cname) || x.PreviousEmployer != null && x.PreviousEmployer.ToLower().Contains(cname)) && x.CountryId == profile.CountryId));

                    user_list.AddRange(individuals.ToList());
                }

                foreach (var item in user_list)
                {
                    bool connected = IsConnected(item.Username, profile.UserId);
                    Connection connection = Get(item.Username, profile.Username);
                    BlockedPeople entity = GetBlockedEntity(item.UserId, profile.UserId);

                    if (!connected)
                    {
                        if (connection != null && connection.IsAccepted == false && connection.IsConnected == false && connection.Initiated == false)
                        {
                            if ((entity != null && !entity.CreatedBy.Equals(profile.Username)))
                            {
                                name = string.Empty;
                                if (item.Type == (int)SecurityRoles.Employers)
                                {
                                    name = !string.IsNullOrEmpty(item.Company) ? item.Company : string.Format("{0} {1}", item.FirstName, item.LastName);
                                }
                                else if (item.Type == (int)SecurityRoles.Jobseeker)
                                {
                                    name = string.Format("{0} {1}", item.FirstName, item.LastName);
                                }

                                using (var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/Mail/youmayknow.html")))
                                {
                                    body = reader.ReadToEnd();
                                    body = body.Replace("@@name", profileName);
                                    body = body.Replace("@@content", sb1.ToString());

                                    string[] receipent = { item.Username };
                                    var subject = string.Format("You May Know {0}", profileName);

                                    AlertService.Instance.SendMail(subject, receipent, body);
                                }

                                image = string.Format("<img src=\"{0}/image/avtar?Id={1}\" style=\"width:64px; height:64px\"/>", baseUrl, item.UserId);


                                sb.AppendFormat("<div><div style=\"float:left; width:64px; height:64px; border:1px solid #d7d7d7\"><a href=\"{0}/{1}\" target=\"_blank\">{2}</a></div>", baseUrl, item.PermaLink, image);
                                sb.Append("<div style=\"float:left; padding-left:20px; height:64px;\">");
                                sb.AppendFormat("<div style=\"padding-bottom:5px;\">{0}</div>", name);
                                sb.AppendFormat("<div><a href=\"{0}/connect?EmailAddress={1}&via=email\" style=\"padding: 10px;  background-color: #01a7e1; text-decoration: none; color: #fff; -webkit-border-radius: 4px; border-radius: 4px; width: 100px; display: block; text-align: center;\" target=\"_blank\">Connect</a></div>", baseUrl, item.Username);
                                sb.Append("</div><div style=\"clear:both; padding-bottom:65px;\"></div></div>");
                            }
                            else if (entity == null)
                            {
                                name = string.Empty;
                                if (item.Type == (int)SecurityRoles.Employers)
                                {
                                    name = !string.IsNullOrEmpty(item.Company) ? item.Company : string.Format("{0} {1}", item.FirstName, item.LastName);
                                }
                                else if (item.Type == (int)SecurityRoles.Jobseeker)
                                {
                                    name = string.Format("{0} {1}", item.FirstName, item.LastName);
                                }

                                using (var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/Mail/youmayknow.html")))
                                {
                                    body = reader.ReadToEnd();
                                    body = body.Replace("@@name", profileName);
                                    body = body.Replace("@@content", sb1.ToString());

                                    string[] receipent = { item.Username };
                                    var subject = string.Format("You May Know {0}", profileName);

                                    AlertService.Instance.SendMail(subject, receipent, body);
                                }

                                image = string.Format("<img src=\"{0}/image/avtar?Id={1}\" style=\"width:64px; height:64px\"/>", baseUrl, item.UserId);


                                sb.AppendFormat("<div><div style=\"float:left; width:64px; height:64px; border:1px solid #d7d7d7\"><a href=\"{0}/{1}\" target=\"_blank\">{2}</a></div>", baseUrl, item.PermaLink, image);
                                sb.Append("<div style=\"float:left; padding-left:20px; height:64px;\">");
                                sb.AppendFormat("<div style=\"padding-bottom:5px;\">{0}</div>", name);
                                sb.AppendFormat("<div><a href=\"{0}/connect?EmailAddress={1}&via=email\" style=\"padding: 10px;  background-color: #01a7e1; text-decoration: none; color: #fff; -webkit-border-radius: 4px; border-radius: 4px; width: 100px; display: block; text-align: center;\" target=\"_blank\">Connect</a></div>", baseUrl, item.Username);
                                sb.Append("</div><div style=\"clear:both; padding-bottom:65px;\"></div></div>");

                            }
                        }
                    }
                }

                if (sb.ToString().Length > 0)
                {
                    using (var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/Mail/peopleyoumayknow.html")))
                    {
                        body = reader.ReadToEnd();
                        body = body.Replace("@@name", name);
                        body = body.Replace("@@content", sb.ToString());
                        string[] receipent = { profile.Username };
                        var subject = "Alert About People May be Known To You";
                        if (user_list.Count > 1)
                        {
                            body = body.Replace("@@type", "People");
                        }
                        else
                        {
                            body = body.Replace("@@type", "Person");
                        }

                        AlertService.Instance.SendMail(subject, receipent, body);
                    }
                }
            }
        }

#pragma warning disable CS0246 // The type or namespace name 'BlockedPeople' could not be found (are you missing a using directive or an assembly reference?)
        public BlockedPeople GetBlockedEntity(long BlockedId, long BlockerId)
#pragma warning restore CS0246 // The type or namespace name 'BlockedPeople' could not be found (are you missing a using directive or an assembly reference?)
        {
            BlockedPeople entity = new BlockedPeople();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                entity = dataHelper.Get<BlockedPeople>().SingleOrDefault(x => x.BlockedId == BlockedId && x.BlockerId == BlockerId);
                if (entity == null)
                {
                    entity = dataHelper.Get<BlockedPeople>().SingleOrDefault(x => x.BlockedId == BlockerId && x.BlockerId == BlockedId);
                }
            }
            return entity;
        }

        public long GetAcceptId(string contactEmail, string loggedInUserEmail)
        {
            long id = 0;
            try
            {
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);

                    UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", contactEmail);
                    UserProfile loggedInUser = dataHelper.GetSingle<UserProfile>("Username", loggedInUserEmail);

                    if (profile != null)
                    {
                        Hashtable parameters = new Hashtable();
                        parameters.Add("UserId", loggedInUser.UserId);
                        parameters.Add("EmailAddress", profile.Username);

                        Connection contact = dataHelper.GetSingle<Connection>(parameters);
                        if (contact != null && !string.IsNullOrEmpty(contact.CreatedBy) && !contact.CreatedBy.Equals(loggedInUserEmail))
                        {
                            id = contact.Id;
                        }
                    }
                }
            }
            catch (Exception)
            {
                id = -1;
            }
            return id;
        }

        public void GenerateUplodJobTemplate(string filePath)
        {
            string[] columns =
            {
                "Job Title", "Summary", "Category", "Specialization", "Country", "State", "City", "Zip",
                "Minimum Experience", "Maximum Experience", "Minimum Age", "Maximum Age", "Minimum Salary",
                "Maximum Salary", "Education", "Qualifications"
            };
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                IWorkbook wb = new HSSFWorkbook();
                var Original = wb.CreateSheet("UploadJobs");
                var Hidden = wb.CreateSheet("Hidden");

                var headerRow = Original.CreateRow(0);

                for (var i = 0; i < columns.Length; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(columns[i]);
                    Original.AutoSizeColumn(i);
                }

                var categorylist =
                    (from c in dataHelper.Get<JobPortal.Data.Specialization>() orderby c.Id ascending select c.Id + " - " + c.Name)
                        .ToList<string>();

                for (var i = 0; i < categorylist.Count; i++)
                {
                    var row = Hidden.CreateRow(i);

                    var cell = row.CreateCell(0);
                    cell.SetCellValue(categorylist[i]);
                }

                var namedCategory = wb.CreateName();
                namedCategory.NameName = "Category";
                namedCategory.RefersToFormula = "Hidden!$A$1:$A$" + categorylist.Count;

                var addressList = new CellRangeAddressList(1, 65535, 2, 2);
                var categoryDvConstraint = DVConstraint.CreateFormulaListConstraint("Category");
                IDataValidation categoryDataValidation = new HSSFDataValidation(addressList, categoryDvConstraint);
                categoryDataValidation.SuppressDropDownArrow = false;
                Original.AddValidationData(categoryDataValidation);


                var specializationlist =
                    (from c in dataHelper.Get<SubSpecialization>() orderby c.Id ascending select c.Id + " - " + c.Name)
                        .ToList<string>();

                for (var i = 0; i < specializationlist.Count; i++)
                {
                    var row = Hidden.GetRow(i);
                    if (row == null)
                    {
                        row = Hidden.CreateRow(i);
                    }
                    var cell = row.CreateCell(1);
                    cell.SetCellValue(specializationlist[i]);
                }

                var namedSpecialization = wb.CreateName();
                namedSpecialization.NameName = "Specialization";
                namedSpecialization.RefersToFormula = "Hidden!$B$1:$B$" + specializationlist.Count;

                var specializationAddressList = new CellRangeAddressList(1, 65535, 3, 3);
                var specializationDvConstraint = DVConstraint.CreateFormulaListConstraint("Specialization");
                IDataValidation specializationDataValidation = new HSSFDataValidation(specializationAddressList,
                    specializationDvConstraint);
                specializationDataValidation.SuppressDropDownArrow = false;
                Original.AddValidationData(specializationDataValidation);

                /* Xls Country DropDown */


                var countrylist = (from c in dataHelper.Get<List>()
                                   where c.Name.Equals("Country")
                                   orderby c.Text ascending
                                   select c.Id + " - " + c.Text).ToList<string>();

                for (var i = 0; i < countrylist.Count; i++)
                {
                    var row = Hidden.GetRow(i);
                    if (row == null)
                    {
                        row = Hidden.CreateRow(i);
                    }
                    var cell = row.CreateCell(2);
                    cell.SetCellValue(countrylist[i]);
                }

                var namedCountry = wb.CreateName();
                namedCountry.NameName = "CountryList";
                namedCountry.RefersToFormula = "Hidden!$C$1:$C$" + countrylist.Count;

                var countryAddressList = new CellRangeAddressList(1, 65535, 4, 4);

                var countryDvConstraint = DVConstraint.CreateFormulaListConstraint("CountryList");
                IDataValidation countryDataValidation = new HSSFDataValidation(countryAddressList, countryDvConstraint);
                countryDataValidation.SuppressDropDownArrow = false;
                Original.AddValidationData(countryDataValidation);


                /* Xls Educationlist DropDown */


                /* Maximum Experience */
                var explist = GetExperienceList(0);
                var rowidx = 0;
                foreach (int key in explist.Keys)
                {
                    var row = Hidden.GetRow(rowidx);
                    if (row == null)
                    {
                        row = Hidden.CreateRow(rowidx);
                    }
                    var cell = row.CreateCell(3);
                    cell.SetCellValue(explist[key].ToString());

                    rowidx++;
                }

                var nameexp = wb.CreateName();
                nameexp.NameName = "Experience";
                nameexp.RefersToFormula = "Hidden!$D$1:$D$" + explist.Count;

                var minExpAddressList = new CellRangeAddressList(1, 65535, 8, 8);

                var minExpDvConstraint = DVConstraint.CreateFormulaListConstraint("Experience");
                IDataValidation minExpDataValidation = new HSSFDataValidation(minExpAddressList, minExpDvConstraint);
                minExpDataValidation.SuppressDropDownArrow = false;
                Original.AddValidationData(minExpDataValidation);

                var maxExpAddressList = new CellRangeAddressList(1, 65535, 9, 9);

                var maxExpDvConstraint = DVConstraint.CreateFormulaListConstraint("Experience");
                IDataValidation maxExpDataValidation = new HSSFDataValidation(maxExpAddressList, maxExpDvConstraint);
                maxExpDataValidation.SuppressDropDownArrow = false;
                Original.AddValidationData(maxExpDataValidation);

                /* Maximum Experience */
                var agelist = GetAgeList();
                rowidx = 0;
                foreach (int key in agelist.Keys)
                {
                    var row = Hidden.GetRow(rowidx);
                    if (row == null)
                    {
                        row = Hidden.CreateRow(rowidx);
                    }
                    var cell = row.CreateCell(4);
                    cell.SetCellValue(agelist[key].ToString());
                    rowidx++;
                }

                var nameAge = wb.CreateName();
                nameAge.NameName = "Age";
                nameAge.RefersToFormula = "Hidden!$E$1:$E$" + agelist.Count;

                var minAgeAddressList = new CellRangeAddressList(1, 65535, 10, 10);

                var minAgeDvConstraint = DVConstraint.CreateFormulaListConstraint("Age");
                IDataValidation minAgeDataValidation = new HSSFDataValidation(minAgeAddressList, minAgeDvConstraint);
                minAgeDataValidation.SuppressDropDownArrow = false;
                Original.AddValidationData(minAgeDataValidation);

                var maxAgeAddressList = new CellRangeAddressList(1, 65535, 11, 11);

                var maxAgeDvConstraint = DVConstraint.CreateFormulaListConstraint("Age");
                IDataValidation maxAgeDataValidation = new HSSFDataValidation(maxAgeAddressList, maxAgeDvConstraint);
                maxAgeDataValidation.SuppressDropDownArrow = false;
                Original.AddValidationData(maxAgeDataValidation);


                /* dropdown Education */

                var Educationlist = (from c in dataHelper.Get<List>()
                                     where c.Name.Equals("Job Type")
                                     orderby c.Text ascending
                                     select c.Id + " - " + c.Text).ToList<string>();

                for (var i = 0; i < Educationlist.Count; i++)
                {
                    var row = Hidden.GetRow(i);
                    if (row == null)
                    {
                        row = Hidden.CreateRow(i);
                    }
                    var cell = row.CreateCell(14);
                    cell.SetCellValue(Educationlist[i]);
                }

                var namedEducation = wb.CreateName();
                namedEducation.NameName = "Education";
                namedEducation.RefersToFormula = "Hidden!$O$1:$O$" + Educationlist.Count;

                var EducationlistAddressList = new CellRangeAddressList(1, 65535, 14, 14);

                var EducationlistDvConstraint = DVConstraint.CreateFormulaListConstraint("Education");
                IDataValidation EducationlistDataValidation = new HSSFDataValidation(EducationlistAddressList,
                    EducationlistDvConstraint);
                EducationlistDataValidation.SuppressDropDownArrow = false;
                Original.AddValidationData(EducationlistDataValidation);


                var Qualificationlist = (from c in dataHelper.Get<List>()
                                         where c.Name.Equals("Qualification")
                                         orderby c.Text ascending
                                         select c.Id + " - " + c.Text).ToList<string>();

                for (var i = 0; i < Qualificationlist.Count; i++)
                {
                    var row = Hidden.GetRow(i);
                    if (row == null)
                    {
                        row = Hidden.CreateRow(i);
                    }
                    var cell = row.CreateCell(15);
                    cell.SetCellValue(Qualificationlist[i]);
                }

                var namedQualificationlist = wb.CreateName();
                namedQualificationlist.NameName = "Qualificationss";
                namedQualificationlist.RefersToFormula = "Hidden!$P$1:$P$" + Qualificationlist.Count;

                var QualificationAddressList = new CellRangeAddressList(1, 65535, 15, 15);

                var QualificationDvConstraint = DVConstraint.CreateFormulaListConstraint("Qualificationss");
                IDataValidation QualificationDataValidation = new HSSFDataValidation(QualificationAddressList,
                    QualificationDvConstraint);
                QualificationDataValidation.SuppressDropDownArrow = false;
                Original.AddValidationData(QualificationDataValidation);


                wb.SetSheetHidden(1, SheetState.Hidden);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                var fs = File.Create(filePath);
                wb.Write(fs);
                fs.Close();
            }
        }

        /// <summary>
        ///     Gets the country by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
#pragma warning disable CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        public List GetCountry(string name)
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List single = new List();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Hashtable parameters = new Hashtable();
                parameters.Add("Name", "Country");
                parameters.Add("Text", name);

                single = dataHelper.GetSingle<List>(parameters);
            }
            return single;
        }

        /// <summary>
        ///     Gets the country by abbrivation.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
#pragma warning disable CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        public List GetCountryByShortForm(string name)
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List single = new List();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Hashtable parameters = new Hashtable();
                parameters.Add("Name", "Country");
                parameters.Add("Value", name);

                single = dataHelper.GetSingle<List>(parameters);
            }
            return single;
        }

        /// <summary>
        ///     Gets the country by Country Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
#pragma warning disable CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        public List GetCountry(long Id)
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List single = new List();

            single = ReadSingleData<List>(string.Format("SELECT * FROM Lists WHERE Id = {0}", Id));
            //using (JobPortalEntities context = new JobPortalEntities())
            //{
            //    DataHelper dataHelper = new DataHelper(context);
            //    single = dataHelper.GetSingle<List>(Id);
            //}
            return single;
        }

        /// <summary>
        ///     Gets the country list.
        /// </summary>
        /// <returns>List of Country</returns>
#pragma warning disable CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        public List<List> GetCountryList() 
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List<List> countryList = ReadData<List>("SELECT [Id], [Name], [ParentList], [Text], [Value], [IsDefault], [contientId], [Code]  FROM [dbo].[Lists]  WHERE Name='Country' ORDER BY IsDefault DESC, [Text] ASC");
            return countryList;
        }

#pragma warning disable CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        public List<List> GetCategoryList()
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List<List> countryList = ReadData<List>("SELECT [Id], [Name], [ParentList], [Text], [Value], [IsDefault], [contientId], [Code]  FROM [dbo].[Lists]  WHERE Name='Country' ORDER BY IsDefault DESC, [Text] ASC");
            return countryList;
        }
#pragma warning disable CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        public List<List> GetNameList()
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List<List> countryList = ReadData<List>("SELECT [Id], [Name], [ParentList], [Text], [Value], [IsDefault], [contientId], [Code]  FROM [dbo].[Lists]  WHERE Name='Country' ORDER BY IsDefault DESC, [Text] ASC");
            return countryList;
        }
        public List<List> GetStateList()
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List<List> countryList = ReadData<List>("SELECT [Id], [Name], [ParentList], [Text], [Value], [IsDefault], [contientId], [Code]  FROM [dbo].[Lists]  WHERE Name='Country' ORDER BY IsDefault DESC, [Text] ASC");
            return countryList;
        }
        public List<List> GetCityList()
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List<List> countryList = ReadData<List>("SELECT [Id], [Name], [ParentList], [Text], [Value], [IsDefault], [contientId], [Code]  FROM [dbo].[Lists]  WHERE Name='Country' ORDER BY IsDefault DESC, [Text] ASC");
            return countryList;
        }



#pragma warning disable CS0246 // The type or namespace name 'DialingEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<DialingEntity> CountryWithCodes
#pragma warning restore CS0246 // The type or namespace name 'DialingEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            get
            {
                return ReadData<DialingEntity>("SELECT ([Text] + ' (+' + [Code]  +')') AS [Key], ('+' + [Code])  AS [Value] FROM [dbo].[Lists] WHERE Name='Country' AND [Code] IS NOT NULL ORDER BY IsDefault DESC, [Text] ASC");
            }
        }

        /// <summary>
        /// Gets the list of country code (dialing codes)
        /// </summary>
        /// <returns></returns>
        public List<object> GetCountryCodes()
        {
            List<object> countryList = new List<object>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var result = dataHelper.Get<List>().Where(x => x.Name.Equals("Country") && x.Code != null && x.Code != "0")
                    .OrderByDescending(x => x.IsDefault).OrderBy(x => x.Text);
                countryList = result.Select(x => new { Code = "+" + x.Code }).ToList<object>();
            }
            return countryList;
        }

        /// <summary>
        /// Gets the day list.
        /// </summary>
        /// <returns>List of days</returns>
        public List<int> GetDayList()
        {
            List<int> dayList = new List<int>();

            for (int i = 1; i <= 31; i++)
            {
                dayList.Add(i);
            }
            return dayList;
        }

        /// <summary>
        /// Gets the day list.
        /// </summary>
        /// <returns>List of days</returns>
#pragma warning disable CS0246 // The type or namespace name 'Month' could not be found (are you missing a using directive or an assembly reference?)
        public List<Month> GetMonthList()
#pragma warning restore CS0246 // The type or namespace name 'Month' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Month> months = new List<Month>();

            months.Add(new Month() { Name = "January", Id = 1 });
            months.Add(new Month() { Name = "February", Id = 2 });
            months.Add(new Month() { Name = "March", Id = 3 });
            months.Add(new Month() { Name = "April", Id = 4 });
            months.Add(new Month() { Name = "May", Id = 5 });
            months.Add(new Month() { Name = "June", Id = 6 });
            months.Add(new Month() { Name = "July", Id = 7 });
            months.Add(new Month() { Name = "August", Id = 8 });
            months.Add(new Month() { Name = "September", Id = 9 });
            months.Add(new Month() { Name = "October", Id = 10 });
            months.Add(new Month() { Name = "November", Id = 11 });
            months.Add(new Month() { Name = "December", Id = 12 });

            return months;
        }

        /// <summary>
        /// Gets the year list.
        /// </summary>
        /// <returns>List of Year</returns>
        public List<int> GetYearList(bool decreasing, int start)
        {
            List<int> yearList = new List<int>();
            if (decreasing)
            {
                for (int i = start; i > (start - 100); i--)
                {
                    yearList.Add(i);
                }
            }
            else
            {
                for (int i = start; i < (start + 100); i++)
                {
                    yearList.Add(i);
                }
            }
            return yearList;
        }

        public List GetUserSkills(string prefix)
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List defaultCountry = new List();
            JobPortalEntities entities = new JobPortalEntities();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                Hashtable parameters = new Hashtable();
                parameters.Add("Name", "Country");
                parameters.Add("IsDefault", true);

                defaultCountry = dataHelper.GetSingle<List>(parameters);
            }
            return defaultCountry;
        }
        //public List<string> GetUserSkills(string prefix)
        //{
        //    List<string> yearList = new List<string>();
        //    JobPortalEntities entities = new JobPortalEntities();
        //    yearList = (from customer in entities.User_Skills
        //                     where customer.SkillName.StartsWith(prefix)
        //                     select new
        //                     {
        //                         label = customer.SkillName,
        //                         val = customer.UserId
        //                     }).ToList();

        //   // return Json(customers);
        //    return yearList;
        //}

        /// <summary>
        /// Gets the year list.
        /// </summary>
        /// <returns>List of Year</returns>
        public List<string> GetCardYearList(bool decreasing, int start, int digit = 4)
        {
            List<string> yearList = new List<string>();
            if (decreasing)
            {
                for (int i = start; i > (start - 100); i--)
                {
                    if (digit == 2)
                    {
                        yearList.Add(i.ToString().Substring(2));
                    }
                    else
                    {
                        yearList.Add(i.ToString());
                    }
                }
            }
            else
            {
                for (int i = start; i < (start + 100); i++)
                {
                    if (digit == 2)
                    {
                        yearList.Add(i.ToString().Substring(2));
                    }
                    else
                    {
                        yearList.Add(i.ToString());
                    }
                }
            }
            return yearList;
        }

        /// <summary>
        /// Gets the default country.
        /// </summary>
        /// <returns></returns>
#pragma warning disable CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        public List GetDefaultCountry()
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List defaultCountry = new List();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                Hashtable parameters = new Hashtable();
                parameters.Add("Name", "Country");
                parameters.Add("IsDefault", true);

                defaultCountry = dataHelper.GetSingle<List>(parameters);
            }
            return defaultCountry;
        }

        /// <summary>
        /// Gets the state by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
#pragma warning disable CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        public List GetState(long Id)
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List single = new List();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Hashtable parameters = new Hashtable();
                parameters.Add("Name", "State");
                parameters.Add("Id", Id);
                single = dataHelper.GetSingle<List>(parameters);
            }
            return single;
        }

        /// <summary>
        ///     Gets the state by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
#pragma warning disable CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        public List GetState(string name)
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List single = new List();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Hashtable parameters = new Hashtable();
                parameters.Add("Name", "State");
                parameters.Add("Text", name);
                single = dataHelper.GetSingle<List>(parameters);
            }
            return single;
        }

        /// <summary>
        ///     Gets the country by abbrivation.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
#pragma warning disable CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        public List GetStateByShortForm(string name)
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List single = new List();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Hashtable parameters = new Hashtable();
                parameters.Add("Name", "State");
                parameters.Add("Value", name);
                single = dataHelper.GetSingle<List>(parameters);
            }
            return single;
        }

        /// <summary>
        ///     Gets the state list of default country.
        /// </summary>
        /// <returns>State list of default country</returns>
#pragma warning disable CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        public List<List> GetStatesOfDefaultCountry()
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            var defaultCountry = GetDefaultCountry();
            List<List> list = new List<List>();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Hashtable parameters = new Hashtable();
                parameters.Add("Name", "State");
                parameters.Add("ParentList", defaultCountry.Id);
                list = dataHelper.GetList<List>(parameters).ToList();
            }
            return list;
        }

        /// <summary>
        /// Gets the state list by country Id.
        /// </summary>
        /// <param name="CountryId"></param>
        /// <returns>State List</returns>
#pragma warning disable CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        public List<List> GetStatesByCountry(long CountryId)
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List<List> list = new List<List>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                List<Parameter> parameters = new List<Parameter>();
                parameters.Add(new Parameter() { Name = "Name", Value = "State", Comparison = ParameterComparisonTypes.Equals });

                List<SortBy> orderFields = new List<SortBy>();
                orderFields.Add(new SortBy() { Field = "Text", Ascending = true });

                var criteria = dataHelper.BuildAnd<List>(parameters);

                list = dataHelper.GetList<List>(criteria, orderFields);
            }
            return list;
        }

#pragma warning disable CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        public List<List> GetQualificationList()
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List<List> list = new List<List>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                List<Parameter> parameters = new List<Parameter>();
                parameters.Add(new Parameter() { Name = "Name", Value = "Qualification", Comparison = ParameterComparisonTypes.Equals });

                var criteria = dataHelper.BuildAnd<List>(parameters);

                list = dataHelper.GetList<List>(criteria);
            }
            return list;
        }

#pragma warning disable CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        public List<List> GetHighestQualificationList()
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List<List> list = new List<List>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                List<Parameter> parameters = new List<Parameter>();
                parameters.Add(new Parameter() { Name = "Name", Value = "Qualification", Comparison = ParameterComparisonTypes.Equals });

                List<SortBy> orderFields = new List<SortBy>();
                orderFields.Add(new SortBy() { Field = "Text", Ascending = true });

                var criteria = dataHelper.BuildAnd<List>(parameters);

                list = dataHelper.GetList<List>(criteria, orderFields);
            }
            return list;
        }


        /// <summary>
        /// Gets Specialization by Full Name(Friendly Name)
        /// </summary>
        /// <param name="specialiseFriendlyName"></param>
        /// <returns></returns>
#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
        public JobPortal.Data.Specialization GetSpecialisationByName(string specialiseFriendlyName)
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
        {
            JobPortal.Data.Specialization single = new JobPortal.Data.Specialization();
            List<JobPortal.Data.Specialization> list = new List<JobPortal.Data.Specialization>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                single = dataHelper.GetSingle<JobPortal.Data.Specialization>("Name", specialiseFriendlyName);
            }
            return single;
        }

        /// <summary>
        /// Get Parent Category of Specialization
        /// </summary>
        /// <returns></returns>
#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
        public IList<JobPortal.Data.Specialization> GetSpecialisations()
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
        {
            List<JobPortal.Data.Specialization> list = new List<JobPortal.Data.Specialization>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                List<SortBy> orderFields = new List<SortBy>();
                orderFields.Add(new SortBy() { Field = "Name", Ascending = true });
                list = dataHelper.GetList<JobPortal.Data.Specialization>(orderFields).ToList();
            }
            return list;
        }

        /// <summary>
        /// Gets the default specialization.
        /// </summary>
        /// <returns></returns>
#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
        public JobPortal.Data.Specialization GetDefaultSpecialization()
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
        {
            JobPortal.Data.Specialization defaultSpecialization = new JobPortal.Data.Specialization();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                defaultSpecialization = dataHelper.GetList<JobPortal.Data.Specialization>().OrderBy(x => x.Name).FirstOrDefault();
            }
            return defaultSpecialization;
        }

#pragma warning disable CS0246 // The type or namespace name 'SubSpecialization' could not be found (are you missing a using directive or an assembly reference?)
        public IList<SubSpecialization> GetSubSpecialisations(int? specialisationid)
#pragma warning restore CS0246 // The type or namespace name 'SubSpecialization' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<SubSpecialization> list = new List<SubSpecialization>();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                if (specialisationid != null)
                {
                    list = dataHelper.Get<SubSpecialization>("CategoryId", specialisationid.Value).OrderBy(x => x.Name).ToList();
                }
            }
            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        public IList<UserProfile> GetNames(int? categoryid,int? type)
#pragma warning restore CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<UserProfile> list = new List<UserProfile>();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                if ( type != null && type == 4)
                {
                    list = dataHelper.Get<UserProfile>().Where(x => x.Type == type.Value).OrderByDescending(x => x.DateCreated).Take(50).ToList();
                }
                else if (categoryid != null && type != null && type == 5)
                {
                    list = dataHelper.Get<UserProfile>().Where(x => x.CategoryId == categoryid.Value && x.Type == type.Value).OrderByDescending(x => x.DateCreated).Take(50).ToList();
                }
                else if ( type != null && type == 12)
                {
                    list = dataHelper.Get<UserProfile>().Where(x => x.Type == type.Value).OrderByDescending(x => x.DateCreated).Take(50).ToList();
                }
                else if ( type != null && type == 13)
                {
                    list = dataHelper.Get<UserProfile>().Where(x =>  x.Type == type.Value).OrderByDescending(x => x.DateCreated).Take(50).ToList();
                }
            }
            return list;
        }       
#pragma warning disable CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        public IList<UserProfile> GetNames1(int? categoryid, int? type,string prefix)
#pragma warning restore CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<UserProfile> list = new List<UserProfile>();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                if (type != null && type == 4)
                {
                    list = dataHelper.Get<UserProfile>().Where(x => x.Type == type.Value && (x.FirstName.StartsWith(prefix) || x.LastName.StartsWith(prefix))).OrderByDescending(x => x.DateCreated).Take(50).ToList();
                }
                else if (categoryid != null && type != null && type == 5)
                {
                    list = dataHelper.Get<UserProfile>().Where(x => x.CategoryId == categoryid.Value && x.Type == type.Value && x.Company.StartsWith(prefix)).OrderByDescending(x => x.DateCreated).Take(50).ToList();
                }
                else if (type != null && type == 12)
                {
                    list = dataHelper.Get<UserProfile>().Where(x => x.Type == type.Value && x.University.StartsWith(prefix)).OrderByDescending(x => x.DateCreated).Take(50).ToList();
                }
                else if (type != null && type == 13)
                {
                    list = dataHelper.Get<UserProfile>().Where(x => x.Type == type.Value && (x.FirstName.StartsWith(prefix) || x.LastName.StartsWith(prefix))).OrderByDescending(x => x.DateCreated).Take(50).ToList();
                }
            }
            return list;
        }

        /// <summary>
        /// Gets the category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
        public JobPortal.Data.Specialization GetContentSpecialization(int id)
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
        {
            JobPortal.Data.Specialization specialization = new JobPortal.Data.Specialization();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                specialization = dataHelper.GetSingle<JobPortal.Data.Specialization>("Id", id);
            }
            return specialization;
        }

        /// <summary>
        /// Gets currency list
        /// </summary>
        /// <returns></returns>
        public List<string> GetCurrenciesList()
        {
            var CurrencySymbols = new List<string>();

            foreach (var item in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                if (item.IsNeutralCulture != true)
                {
                    var region = new RegionInfo(item.LCID);
                    var CurrencyName = region.CurrencyEnglishName;
                    var CurrenctSymbol = region.ISOCurrencySymbol;

                    CurrencySymbols.Add(CurrenctSymbol);
                }
            }
            return CurrencySymbols;
        }

        /// <summary>
        /// Gets experience list
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public SortedList GetExperienceList(int exp)
        {
            var explist = new SortedList();
            for (var i = exp; i <= 60; i++)
            {
                explist.Add(i, i);
            }
            return explist;
        }

        /// <summary>
        /// Get the list Age.
        /// </summary>
        /// <param name="age"></param>
        /// <returns></returns>
        public SortedList GetAgeList(int age = 18)
        {
            var agelist = new SortedList();
            if (age == 18)
            {
                agelist.Add(age, age);
                age += 2;
            }

            for (var i = age; i <= 75; i += 5)
            {
                agelist.Add(i, i);
            }
            return agelist;
        }

        /// <summary>
        /// Gets the list of Job Type.
        /// </summary>
        /// <returns></returns>
#pragma warning disable CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        public List<List> GetJobTypeList()
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List<List> list = new List<List>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                Hashtable parameters = new Hashtable();
                parameters.Add("Name", "Job Type");
                parameters.Add("ParentList", null);

                list = dataHelper.GetList<List>(parameters).ToList();
            }
            return list;
        }

        /// <summary>
        /// Gest the list of Frequency
        /// </summary>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'FrequencyType' could not be found (are you missing a using directive or an assembly reference?)
        public List<FrequencyType> GetFrequencyList()
#pragma warning restore CS0246 // The type or namespace name 'FrequencyType' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<FrequencyType> list = new List<FrequencyType>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                list = dataHelper.GetList<FrequencyType>().OrderBy(x => x.FrequencyName).ToList();
            }
            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'SubSpecialization' could not be found (are you missing a using directive or an assembly reference?)
        public SubSpecialization GetDefaultSubSpecialization()
#pragma warning restore CS0246 // The type or namespace name 'SubSpecialization' could not be found (are you missing a using directive or an assembly reference?)
        {
            var defaultSepcialization = GetDefaultSpecialization();
            SubSpecialization specialization = new SubSpecialization();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                specialization = dataHelper.GetSingle<SubSpecialization>("Id", defaultSepcialization.Id);
            }
            return specialization;
        }

#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
        public JobPortal.Data.Specialization GetCategory(string name)
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
        {
            JobPortal.Data.Specialization category = new JobPortal.Data.Specialization();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                category = dataHelper.GetSingle<JobPortal.Data.Specialization>("Name", name);
            }
            return category;
        }

#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
        public JobPortal.Data.Specialization GetCategory(int id)
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
        {
            JobPortal.Data.Specialization category = new JobPortal.Data.Specialization();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                category = dataHelper.GetSingle<JobPortal.Data.Specialization>("Id", id);
            }

            return category;
        }

#pragma warning disable CS0246 // The type or namespace name 'SubSpecialization' could not be found (are you missing a using directive or an assembly reference?)
        public SubSpecialization GetSpecialization(string name)
#pragma warning restore CS0246 // The type or namespace name 'SubSpecialization' could not be found (are you missing a using directive or an assembly reference?)
        {
            SubSpecialization specialization = new SubSpecialization();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                specialization = dataHelper.GetSingle<SubSpecialization>("Name", name);
            }

            return specialization;
        }

#pragma warning disable CS0246 // The type or namespace name 'SubSpecialization' could not be found (are you missing a using directive or an assembly reference?)
        public SubSpecialization GetSpecialization(int id)
#pragma warning restore CS0246 // The type or namespace name 'SubSpecialization' could not be found (are you missing a using directive or an assembly reference?)
        {
            SubSpecialization specialization = new SubSpecialization();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                specialization = dataHelper.GetSingle<SubSpecialization>("Id", id);
            }

            return specialization;
        }

#pragma warning disable CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        public List<List> GetStatesById(long? countryId)
#pragma warning restore CS0305 // Using the generic type 'List<T>' requires 1 type arguments
        {
            List<List> list = new List<List>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                Hashtable parameters = new Hashtable();
                parameters.Add("Name", "State");
                parameters.Add("ParentList", countryId);

                list = dataHelper.GetList<List>(parameters).OrderBy(x => x.Text).ToList();
            }
            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'Resume' could not be found (are you missing a using directive or an assembly reference?)
        public Resume GetResume(long id)
#pragma warning restore CS0246 // The type or namespace name 'Resume' could not be found (are you missing a using directive or an assembly reference?)
        {
            Resume resume = new Resume();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                resume = dataHelper.GetSingle<Resume>("Id", id);
            }
            return resume;
        }

#pragma warning disable CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
        public IEnumerable<UserProfile> SearchPeople(SearchResume model, out int records)
#pragma warning restore CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'UserProfile' could not be found (are you missing a using directive or an assembly reference?)
        {
            records = 10;
            var blockedList = new List<string>();

            List<int> typeList = new List<int> { (int)SecurityRoles.Jobseeker, (int)SecurityRoles.Employers };
            List<Parameter> parameters = new List<Parameter>();
            IQueryable<UserProfile> people = null;
            List<UserProfile> list = new List<UserProfile>();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                people = dataHelper.Get<UserProfile>().Where(x => typeList.Contains(x.Type) && x.IsActive == true && x.IsDeleted == false);

                if (!string.IsNullOrEmpty(model.Username))
                {
                    UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", model.Username);
                    people = people.Where(x => x.UserId != profile.UserId);
                    parameters = new List<Parameter>();

                    parameters.Add(new Parameter() { Name = "UpdatedBy", Value = profile.Username, Comparison = ParameterComparisonTypes.NotEquals });
                    parameters.Add(new Parameter() { Name = "IsBlocked", Value = true, Comparison = ParameterComparisonTypes.Equals });

                    blockedList = dataHelper.Get<BlockedPeople>().Where(x => x.BlockedId == profile.UserId).Select(x => x.Blocker.Username).ToList();
                }

                if (blockedList.Count > 0)
                {
                    people = people.Where(x => !blockedList.Contains(x.Username));
                }

                if (!string.IsNullOrEmpty(model.Name))
                {
                    string[] names = model.Name.ToLower().Split(' ');
                    people = people.Where(x => names.Any(z => (x.FirstName + " " + x.LastName).ToLower().Contains(z) || (x.Company != null && (x.Company).ToLower().Contains(z))));
                }

                if (!string.IsNullOrEmpty(model.Where))
                {
                    people = people.Where(x => (((x.Country != null) ? x.Country.Text + x.Country.Value : "") + ((x.State != null) ? x.State.Text : "") + (x.City != null ? x.City : "")).ToLower().Contains(model.Where.ToLower()));
                }

                if (model.AgeMin != null && model.AgeMax != null)
                {
                    people = people.Where(x => x.Age >= model.AgeMin && x.Age <= model.AgeMax);
                }

                if (!string.IsNullOrEmpty(model.Gender))
                {
                    people = people.Where(x => x.Gender.Equals(model.Gender));
                }

                if (model.Relationship != null)
                {
                    people = people.Where(x => x.RelationshipStatus == model.Relationship);
                }

                if (model.CountryId != null)
                {
                    people = people.Where(x => x.CountryId == model.CountryId);
                }

                if (model.StateId != null)
                {
                    people = people.Where(x => x.StateId == model.StateId);
                }

                if (!string.IsNullOrEmpty(model.City))
                {
                    people = people.Where(x => x.City.ToLower().Contains(model.City.ToLower()));
                }
                records = people.Count();
                list = people.Take(records).OrderByDescending(x => x.DateCreated).Skip(model.PageNumber > 0 ? (model.PageNumber - 1) * model.PageSize : model.PageNumber * model.PageSize).Take(model.PageSize).ToList();
            }
            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'SubSpecialization' could not be found (are you missing a using directive or an assembly reference?)
        public IList<SubSpecialization> GetSubSpecializationBySPID(int catId)
#pragma warning restore CS0246 // The type or namespace name 'SubSpecialization' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<SubSpecialization> list = new List<SubSpecialization>();

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                list = dataHelper.Get<SubSpecialization>().Where(obj => obj.IsDeleted == false && obj.CategoryId == catId).ToList();
            }
            return list;
        }

       

        /// <summary>
        /// Gets the Specialization By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'SubSpecialization' could not be found (are you missing a using directive or an assembly reference?)
        public SubSpecialization GetContentSubSpecialization(int id)
#pragma warning restore CS0246 // The type or namespace name 'SubSpecialization' could not be found (are you missing a using directive or an assembly reference?)
        {
            SubSpecialization specialization = new SubSpecialization();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                specialization = dataHelper.GetSingle<SubSpecialization>("Id", id);
            }
            return specialization;
        }

        public long ImportContact(long userId, string firstName, string lastName, string email)
        {
            long id = 0;
            object retval = null;
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@FirstName", firstName));
            parameters.Add(new SqlParameter("@LastName", lastName));
            parameters.Add(new SqlParameter("@Email", email));
            retval = ReadDataField("ImportContact", parameters);
            if (retval != null || retval != DBNull.Value)
            {
                id = Convert.ToInt64(retval);
            }
            return id;
        }
    }
}