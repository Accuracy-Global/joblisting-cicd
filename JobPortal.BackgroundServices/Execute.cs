
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JobPortal.BackgroundServices
{
    public class Execute
    {
        public static void IndeedFirst()
        {
            string publisherId = ConfigService.Instance.GetConfigValue("indeed_publisher_id");
            string indeedUrl = ConfigService.Instance.GetConfigValue("indeed_jobsearch_url");
            IHelperService helper = new HelperService();
            IJobService jobService = new Services.JobService();
            List<IndeedEntity> list = helper.IndeedList();
        
            Parallel.ForEach(list, (item) =>
            {
                int start = 0;
                int pages = 0;
                int rows = 0;
                long countryId = item.CountryId;
                int categoryId = item.CategoryId;

                using (WebClient wcIndeed = new WebClient())
                {
                    string url = string.Format(indeedUrl, publisherId, item.Category.ToLower(), "", start, item.CountryCode.ToLower());
                    string result = wcIndeed.DownloadString(url);
                    Hashtable json = JsonConvert.DeserializeObject<Hashtable>(result);

                    rows = Convert.ToInt32(json["totalResults"]);
                    double d = rows / 25;
                    double p = Math.Ceiling(d);
                    pages = Convert.ToInt32(p);
                    List<IndeedJob> jobs = JsonConvert.DeserializeObject<List<IndeedJob>>(Convert.ToString(json["results"]));

                    foreach (var job in jobs)
                    {
                        if (job.Country.ToLower().Equals(item.CountryCode.ToLower()))
                        {                            
                            job.Source = "Indeed";
                            job.CountryId = countryId;
                            job.CategoryId = categoryId;
                            jobService.ManageAggregatorJob(job);
                        }
                    }

                    for (int pg = 1; pg < pages; pg++)
                    {
                        start = start + 25;
                        url = string.Format(indeedUrl, publisherId, item.Category.ToLower(), "", start, item.CountryCode.ToLower());
                        try
                        {
                            result = wcIndeed.DownloadString(url);
                            json = JsonConvert.DeserializeObject<Hashtable>(result);

                            jobs = JsonConvert.DeserializeObject<List<IndeedJob>>(Convert.ToString(json["results"]));
                            foreach (var job in jobs)
                            {
                                if (job.Country.ToLower().Equals(item.CountryCode.ToLower()))
                                {                                    
                                    job.Source = "Indeed";
                                    job.CountryId = countryId;
                                    job.CategoryId = categoryId;
                                    jobService.ManageAggregatorJob(job);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            });            
        }

        public static void IndeedNext()
        {
            string publisherId = ConfigService.Instance.GetConfigValue("indeed_publisher_id");
            string indeedUrl = ConfigService.Instance.GetConfigValue("indeed_jobsearch_url");
            IHelperService helper = new HelperService();
            IJobService jobService = new Services.JobService();
            List<IndeedEntity> list = helper.IndeedList();

            int wt = 0;

            Parallel.ForEach(list, (item) =>
            {
                int start = 0;
                int pages = 0;
                int rows = 0;
                long countryId = item.CountryId;
                int categoryId = item.CategoryId;

                using (WebClient wcIndeed = new WebClient())
                {
                    string url = string.Format(indeedUrl, publisherId, item.Category.ToLower(), "", start, item.CountryCode.ToLower());
                    string result = wcIndeed.DownloadString(url);
                    Hashtable json = JsonConvert.DeserializeObject<Hashtable>(result);

                    rows = Convert.ToInt32(json["totalResults"]);
                    double d = rows / 25;
                    double p = Math.Ceiling(d);
                    pages = Convert.ToInt32(p);
                    List<IndeedJob> jobs = JsonConvert.DeserializeObject<List<IndeedJob>>(Convert.ToString(json["results"]));

                    foreach (var job in jobs)
                    {
                        if (job.Country.ToLower().Equals(item.CountryCode.ToLower()))
                        {                            
                            job.Source = "Indeed";
                            job.CountryId = countryId;
                            job.CategoryId = categoryId;
                            jobService.ManageAggregatorJob(job);
                        }
                    }

                    for (int pg = 1; pg < pages; pg++)
                    {
                        start = start + 25;
                        url = string.Format(indeedUrl, publisherId, item.Category.ToLower(), "", start, item.CountryCode.ToLower());
                        try
                        {
                            result = wcIndeed.DownloadString(url);
                            json = JsonConvert.DeserializeObject<Hashtable>(result);

                            jobs = JsonConvert.DeserializeObject<List<IndeedJob>>(Convert.ToString(json["results"]));
                            foreach (var job in jobs)
                            {
                                if (job.Country.ToLower().Equals(item.CountryCode.ToLower()))
                                {                                    
                                    job.Source = "Indeed";
                                    job.CountryId = countryId;
                                    job.CategoryId = categoryId;
                                    jobService.ManageAggregatorJob(job);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            });            
        }

        public static void IndeedJobFeeder(string fileName)
        {
            JobPortal.Services.JobService jobService = new JobPortal.Services.JobService();

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            XmlDocument xmlDoc = new XmlDocument();

            XmlDeclaration xmldecl;
            xmldecl = xmlDoc.CreateXmlDeclaration("1.0", null, null);
            xmldecl.Encoding = "UTF-8";
            //xmldecl.Standalone = "yes";

            // Add the new node to the document.
            XmlElement root = xmlDoc.DocumentElement;
            xmlDoc.InsertBefore(xmldecl, root);

            XmlNode rootNode = xmlDoc.CreateElement("source");
            xmlDoc.AppendChild(rootNode);

            XmlNode pub = xmlDoc.CreateElement("publisher");
            pub.InnerText = "Joblisting.com";
            rootNode.AppendChild(pub);

            XmlNode puburl = xmlDoc.CreateElement("publisherurl");
            puburl.InnerText = "https://www.joblisting.com";
            rootNode.AppendChild(puburl);

            XmlNode lastBuild = xmlDoc.CreateElement("lastBuildDate");
            lastBuild.InnerText = DateTime.Now.Add(new TimeSpan(08, 00, 00)).ToString("ddd, dd MMM yyyy hh:mm:ss") + " GMT";
            rootNode.AppendChild(lastBuild);

            List<JobFeedEntity> list = jobService.JobFeedList();
            foreach (var item in list)
            {
                XmlNode job = xmlDoc.CreateElement("job");
                rootNode.AppendChild(job);

                XmlNode title = xmlDoc.CreateElement("title");
                XmlCDataSection title_cdata = xmlDoc.CreateCDataSection(item.Title);
                title.AppendChild(title_cdata);
                job.AppendChild(title);

                XmlNode date = xmlDoc.CreateElement("date");
                XmlCDataSection date_cdata = xmlDoc.CreateCDataSection(item.PublishedDate);
                date.AppendChild(date_cdata);
                job.AppendChild(date);

                XmlNode referencenumber = xmlDoc.CreateElement("referencenumber");
                XmlCDataSection refno_cdata = xmlDoc.CreateCDataSection(item.Id.ToString());
                referencenumber.AppendChild(refno_cdata);
                job.AppendChild(referencenumber);

                XmlNode url = xmlDoc.CreateElement("url");
                XmlCDataSection url_cdata = xmlDoc.CreateCDataSection(string.Format("https://www.joblisting.com/job/{0}", item.JobUrl));
                url.AppendChild(url_cdata);
                job.AppendChild(url);

                XmlNode company = xmlDoc.CreateElement("company");
                XmlCDataSection company_cdata = xmlDoc.CreateCDataSection(item.Company);
                company.AppendChild(company_cdata);
                job.AppendChild(company);

                XmlNode city = xmlDoc.CreateElement("city");
                XmlCDataSection city_cdata = xmlDoc.CreateCDataSection(item.City);
                city.AppendChild(city_cdata);
                job.AppendChild(city);

                XmlNode state = xmlDoc.CreateElement("state");
                XmlCDataSection state_cdata = xmlDoc.CreateCDataSection(item.State);
                state.AppendChild(state_cdata);
                job.AppendChild(state);

                XmlNode country = xmlDoc.CreateElement("country");
                XmlCDataSection country_cdata = xmlDoc.CreateCDataSection(item.CountryCode);
                country.AppendChild(country_cdata);
                job.AppendChild(country);

                XmlNode postalcode = xmlDoc.CreateElement("postalcode");
                XmlCDataSection postalcode_cdata = xmlDoc.CreateCDataSection(item.Zip);
                postalcode.AppendChild(postalcode_cdata);
                job.AppendChild(postalcode);

                XmlNode description = xmlDoc.CreateElement("description");

                string desc = string.Format("{0}\nJob Requirements:\n{1}", item.Description, item.Requirements);
                XmlCDataSection description_cdata = xmlDoc.CreateCDataSection(desc);
                description.AppendChild(description_cdata);
                job.AppendChild(description);

                XmlNode salary = xmlDoc.CreateElement("salary");
                XmlCDataSection salary_cdata = xmlDoc.CreateCDataSection("");
                salary.AppendChild(salary_cdata);
                job.AppendChild(salary);

                XmlNode education = xmlDoc.CreateElement("education");
                XmlCDataSection education_cdata = xmlDoc.CreateCDataSection(item.Education);
                education.AppendChild(education_cdata);
                job.AppendChild(education);

                XmlNode jobtype = xmlDoc.CreateElement("jobtype");
                XmlCDataSection jobtype_cdata = xmlDoc.CreateCDataSection(item.JobType);
                jobtype.AppendChild(jobtype_cdata);
                job.AppendChild(jobtype);

                XmlNode category = xmlDoc.CreateElement("category");
                XmlCDataSection category_cdata = xmlDoc.CreateCDataSection(item.Category);
                category.AppendChild(category_cdata);
                job.AppendChild(category);

                XmlNode experience = xmlDoc.CreateElement("experience");
                XmlCDataSection experience_cdata = xmlDoc.CreateCDataSection(item.Experience);
                experience.AppendChild(experience_cdata);
                job.AppendChild(experience);
            }
            xmlDoc.Save(fileName);
        }
    }
}
