using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Web.Models
{
    public class SafetyAdviceModel
    {
        [AllowHtml]
        [Required(ErrorMessage="Provide safety advice!")]
        public string Advice { get; set; }
    }
}