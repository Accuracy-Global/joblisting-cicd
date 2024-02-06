#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace JobPortal.Web
{
    public class JobAggregator
    {
        public static void IndeedFirst()
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
                            Debug.Print(string.Format("Category: {0}, CountryCode: {1}, CountryId: {2}, CategoryId: {3}", item.Category, item.CountryCode, countryId, categoryId));
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
                                    Debug.Print(string.Format("Category: {0}, CountryCode: {1}, CountryId: {2}, CategoryId: {3}", item.Category, item.CountryCode, countryId, categoryId));
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

            //JobNotifier.FreshJobs();
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
                            Debug.Print(string.Format("Category: {0}, CountryCode: {1}, CountryId: {2}, CategoryId: {3}", item.Category, item.CountryCode, countryId, categoryId));
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
                                    Debug.Print(string.Format("Category: {0}, CountryCode: {1}, CountryId: {2}, CategoryId: {3}", item.Category, item.CountryCode, countryId, categoryId));
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

            //JobNotifier.FreshJobs();
        }
    }
}