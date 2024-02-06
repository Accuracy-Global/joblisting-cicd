#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace JobPortal.Web
{
    public class JobFeeder
    {        
        public static void Generate(string fileName)
        {
            JobService jobService = new JobService();

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
            lastBuild.InnerText = DateTime.Now.Add(new TimeSpan(08,00,00)).ToString("ddd, dd MMM yyyy hh:mm:ss") + " GMT";
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