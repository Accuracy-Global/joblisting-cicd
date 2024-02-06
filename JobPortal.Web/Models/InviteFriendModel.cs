using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class InviteFriendModel
    {
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*?\.[a-zA-Z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$", ErrorMessage = "Invalid Email Address!")]
        public string EmailAddress { get; set; }

        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Invalid Mobile Number!")]
        public string Mobile { get; set; }
    }
}