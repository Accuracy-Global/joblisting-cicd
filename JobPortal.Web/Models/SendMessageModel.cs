using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class SendMessageModel
    {
        public long MessageId { get; set; }
        public string ReceiverEmail { get; set; }
        public Guid? TrackingId { get; set; }

        [RegularExpression(@"[ A-Za-z0-9,'!?:_.-]*$", ErrorMessage = "Special characters not allowed!")]
        public string Message { get; set; }
    }
}