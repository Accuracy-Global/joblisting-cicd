/* ============================================================================================================================
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
 * =========================================================================================================================== */

using LinkedIn.Api.Client.Core.Profiles;
using LinkedIn.Api.Client.Owin.Json.Profiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedIn.Api.Client.Owin.Profiles
{
    public class LinkedInProfileApi : LinkedInProfileApi<LinkedInApiClient, JObject>
    {
        public LinkedInProfileApi(LinkedInApiClient client)
            : base(client)
        { }

        public override async Task<LinkedInBasicProfile> GetBasicProfileAsync()
        {
            var result = await Client.ExecuteAsync(string.Format("https://api.linkedin.com/v1/people/~:({0})", BasicProfileSelectors.ToString()));
            
            if(result == null)
            {
                return null;
            }

            return new JsonBasicProfileParser().Parse(result);
        }

        public override async Task<LinkedInFullProfile> GetFullProfileAsync()
        {
            var result = await Client.ExecuteAsync(string.Format("https://api.linkedin.com/v1/people/~:({0})", FullProfileSelectors));

            if (result == null)
            {
                return null;
            }

            return new JsonProfileParser().Parse(result);
        }
    }
}
