using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoblistingVerificationService
{
    public class Entity
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public DateTime? SentDate { get; set; }
        public string Template { get; set; }
        public string BaseUrl { get; set; }
        public int AlertType { get; set; }
        public long CountryId { get; set; }
        public string PermaLink { get; set; }
        public string Relation { get; set; }
        public int Age { get; set; }
    }
}
