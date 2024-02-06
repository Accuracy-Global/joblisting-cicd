using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class SearchedJobEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Overview { get; set; }
        public string Requirements { get; set; }
        public string Category { get; set; }
        public string Specialization { get; set; }
        public string Qualification { get; set; }
        public string MinimumExperience { get; set; }
        public string MaximumExperience { get; set; }
        public string Currency { get; set; }
        public string MinimumSalary { get; set; }
        public string MaximumSalary { get; set; }
        public string JobType { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public long EmployerId { get; set; }
        public string Company { get; set; }
        public string EmployerEmail { get; set; }
        public string EmployerUrl { get; set; }
        public string JobUrl { get; set; }
        public int Applied { get; set; }
        public int Expired { get; set; }
        public int NearExpiry { get; set; }
        public int JobseekerBlocked { get; set; }
        public int EmployerBlocked { get; set; }
        public int Confirmed { get; set; }
        public bool Connected { get; set; }
        public int MaxRows { get; set; }
        public string Source { get; set; }
        public string PostedOn
        {
            get
            {
                return PublishedDate.ToString("MMM-dd-yyyy");
            }
        }

        public string ClosedOn
        {
            get
            {
                return ClosingDate.ToString("MMM-dd-yyyy");
            }
        }
        public byte[] Image { get; set; }
        public string ImageType { get; set; }
        public string ExpiryStatus
        {
            get
            {
                string status = string.Empty;
                if (Expired > 0)
                {
                    status = string.Format("{0} (Expired)", ClosingDate.ToString("MMM-dd-yyyy"));
                }
                else if (Applied > 0)
                {
                    status = string.Format("{0} (Already Applied)", ClosingDate.ToString("MMM-dd-yyyy"));
                }
                else if(NearExpiry > 0)
                {
                    status = string.Format("{0} (Will expire soon)", ClosingDate.ToString("MMM-dd-yyyy"));
                }                
                return status;
            }
        }
    }
}
