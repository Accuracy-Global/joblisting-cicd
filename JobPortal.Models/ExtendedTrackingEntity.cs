#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
#pragma warning disable CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
    public class ExtendedTrackingEntity : Tracking
#pragma warning restore CS0246 // The type or namespace name 'Tracking' could not be found (are you missing a using directive or an assembly reference?)
    {
        public int MaxRows { get; set; }
    }
}
