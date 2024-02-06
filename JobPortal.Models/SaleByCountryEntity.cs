using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class SaleByCountryEntity
    {
        public long CountryId { get;set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public decimal Free { get; set; }
        public decimal Advance { get; set; }
        public decimal Basic { get; set; }
    }
}
