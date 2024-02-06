using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class InterviewModel
    {
        public InterviewModel() {
        }
        public long? JobseekerId { get; set; }
       public Guid? TrackingId { get; set; }

       [Required(ErrorMessage="Provide interview type")]
       [Display(Name="Interview Type")]
       public int InterviewType { get; set; }

        [Required(ErrorMessage="Provide interview date")]
        [Display(Name="Date")]
        public DateTime? InterviewDate { get; set; }
       
        [Required(ErrorMessage = "Provide interview details")]
        [Display(Name = "Interview Details")]
        public string Description { get; set; }
        
        [Required(ErrorMessage="Provide Interviewer name")]
        public string Interviewer { get; set; }
        public int Status { get; set; }

        [Required(ErrorMessage = "Select round of Interview")]
        [Display(Name="Interview Round")]
        public int Round { get; set; }
    }
}