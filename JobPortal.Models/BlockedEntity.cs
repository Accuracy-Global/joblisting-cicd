using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class BlockedEntity
    {
        public long Id { get; set; }
        public long BlockedId { get; set; }
        public long BlockerId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }
}
