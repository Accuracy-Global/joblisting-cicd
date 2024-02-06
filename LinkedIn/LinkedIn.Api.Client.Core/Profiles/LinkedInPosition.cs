/* ============================================================================================================================
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
 * =========================================================================================================================== */

using LinkedIn.Api.Client.Core.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedIn.Api.Client.Core.Profiles
{
    public class LinkedInPosition
    {
        public int Id
        {
            get;
            set;
        }

        public LinkedInCompany Company
        {
            get;
            set;
        }

        public LinkedInDate StartDate
        {
            get;
            set;
        }

        public LinkedInDate EndDate
        {
            get;
            set;
        }

        public bool IsCurrent
        {
            get;
            set;
        }
    }
}
