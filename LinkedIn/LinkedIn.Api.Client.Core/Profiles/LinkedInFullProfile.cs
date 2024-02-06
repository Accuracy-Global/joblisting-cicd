/* ============================================================================================================================
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
 * =========================================================================================================================== */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedIn.Api.Client.Core.Profiles
{
    public class LinkedInFullProfile : LinkedInBasicProfile
    {
        public LinkedInDate DOB
        {
            get;
            set;
        }

        public string Interests
        {
            get;
            set;
        }

        public Dictionary<int, string> Certifications
        {
            get;
            set;
        }

        public Dictionary<int, string> Courses
        {
            get;
            set;
        }

        public List<LinkedInEducation> Educations
        {
            get;
            set;
        }

        public Dictionary<int, string> HonorAwards
        {
            get;
            set;
        }

        public Dictionary<int, string> Languages
        {
            get;
            set;
        }

        public Dictionary<int, string> Skills
        {
            get;
            set;
        }

        public int NumberOfRecommenders
        {
            get;
            set;
        }
    }
}
