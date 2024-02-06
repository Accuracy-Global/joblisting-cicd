using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public int? Jobs { get; set; }
        public int Profiles { get; set; }
        public int Messages { get; set; }
        public int? Interviews { get; set; }
        public int? Downloads { get; set; }
        public int? Days { get; set; }
        public long CountryId { get; set; }
        public string CountryName { get; set; }

        public string Description 
        {
            get
            {
                string description = string.Empty;
                if (this.TypeName == "Company")
                {
                    if (this.Days != null)
                    {
                        description = string.Format("{0} package with {1} Jobs, {2} Profiles, {3} Messages, {4} Interviews, {5} Resumes Download, Promote Job for {6} days", this.Name, this.Jobs, this.Profiles, this.Messages, this.Interviews, this.Downloads, this.Days);
                    }
                    else
                    {
                        description = string.Format("{0} package {1} Profiles, {2} Messages, {3} Interviews, {4} Resumes Download", this.Name, this.Profiles, this.Messages, this.Interviews, this.Downloads);
                    }
                }
                else
                {
                    if (this.Days != null)
                    {
                        description = string.Format("{0} package with {1} Profiles, {2} Messages, Promote Profile for {3} days", this.Name, this.Profiles, this.Messages, this.Days);
                    }
                    else
                    {
                        description = string.Format("{0} package with {1} Profiles, {2} Messages", this.Name, this.Profiles, this.Messages);
                    }
                }
                return description;
            }
        }
    }
}
