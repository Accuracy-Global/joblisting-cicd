using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class PackageEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public int Jobs { get; set; }
        public int Profiles { get; set; }
        public int Messages { get; set; }
        public int Interviews { get; set; }
        public int ResumeDownloads { get; set; }
    }
}
