/* ============================================================================================================================
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
 * =========================================================================================================================== */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedIn.Api.Client.Core.Profiles
{
    public abstract class LinkedInProfileApi<TLinkedInApiClient, TLinkedInResult> : LinkedInApiBase<TLinkedInApiClient, TLinkedInResult>
        where TLinkedInApiClient : ILinkedInApiClient<TLinkedInResult>
    {
        private string _BasicProfileSelectors = null;
        private string _FullProfileSelectors = null;

        public LinkedInProfileApi(TLinkedInApiClient client)
            : base(client)
        {
            string[] basicProfileSelectors =
            {
                    "id",
                    "first-name",
                    "last-name",
                    "maiden-name",
                    "formatted-name",
                    "email-address",
                    "public-profile-url",
                    "picture-url",
                    "headline",
                    "location:(name)",
                    "industry",
                    "num-connections",
                    "summary",
                    "specialties",
                    "positions"
            };

            string[] additionalProfileSelectors =
            {
                    "date-of-birth",
                    "interests",
                    "skills",
                    "languages",
                    "certifications",
                    "educations",
                    "courses",
                    "num-recommenders",
                    "recommendations-received",
                    "honors-awards"
            };

            var sbBasicProfileSelectors = new StringBuilder();

            foreach (string s in basicProfileSelectors)
            {
                sbBasicProfileSelectors.Append(s);
                sbBasicProfileSelectors.Append(",");
            }

            var sbFullProfileSelectors = new StringBuilder();
            sbFullProfileSelectors.Append(sbBasicProfileSelectors.ToString());

            foreach (string s in additionalProfileSelectors)
            {
                sbFullProfileSelectors.Append(s);
                sbFullProfileSelectors.Append(",");
            }

            _BasicProfileSelectors = sbBasicProfileSelectors.ToString();
            _FullProfileSelectors = sbFullProfileSelectors.ToString();

            if (_BasicProfileSelectors.EndsWith(","))
            {
                _BasicProfileSelectors.Remove(_BasicProfileSelectors.Length - 1, 1);
            }

            if (_FullProfileSelectors.EndsWith(","))
            {
                _FullProfileSelectors.Remove(_FullProfileSelectors.Length - 1, 1);
            }
        }

        public abstract Task<LinkedInBasicProfile> GetBasicProfileAsync();
        public abstract Task<LinkedInFullProfile> GetFullProfileAsync();

        protected string BasicProfileSelectors
        {
            get
            {
                return _BasicProfileSelectors;
            }
        }

        protected string FullProfileSelectors
        {
            get
            {
                return _FullProfileSelectors;
            }
        }
    }
}
