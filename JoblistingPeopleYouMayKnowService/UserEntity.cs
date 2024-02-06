using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoblistingPeopleYouMayKnowService
{
    public class UserEntity
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Company { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PermaLink { get; set; }

        public int AlertType { get; set; }
        public string Template { get; set; }
        public string BaseUrl { get; set; }
    }
}
