using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
namespace JobPortal.Web.Models
{
    public class BaseModel
    {
        public string SELECT = "SELECT";
        public int PageSize = Convert.ToInt32(ConfigService.Instance.GetConfigValue("PageSize"));
    }
}
