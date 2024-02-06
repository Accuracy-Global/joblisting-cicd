using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JobPortal.Models
{
    public class AddBlog
    {
        public int BlogID { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Provide Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Provide descriptions!")]
        [Display(Name = "descriptions")]
        [AllowHtml]
        public string descriptions { get; set; }
        public byte[] Images { get; set; }
        public string Name { get; set; }
        public string categories { get; set; }
        public Nullable<System.DateTime> Createddate { get; set; }
    }
}
