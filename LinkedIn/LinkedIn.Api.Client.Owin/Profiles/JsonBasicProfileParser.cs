/* ============================================================================================================================
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
 * =========================================================================================================================== */

using LinkedIn.Api.Client.Core;
using LinkedIn.Api.Client.Core.Companies;
using LinkedIn.Api.Client.Core.Profiles;
using LinkedIn.Api.Client.Owin.Json.Profiles;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedIn.Api.Client.Owin.Profiles
{
    public class JsonBasicProfileParser : ILinkedInResultParser<LinkedInBasicProfile, JObject>
    {
        public LinkedInBasicProfile Parse(JObject json)
        {
            var jsonProfile = json.ToObject<JsonLinkedInBasicProfile>();

            if (jsonProfile == null)
            {
                return null;
            }

            var basicProfile = new LinkedInBasicProfile()
            {
                Id = jsonProfile.Id,
                EmailAddress = jsonProfile.EmailAddress,
                FirstName = jsonProfile.FirstName,
                FormattedName = jsonProfile.FormattedName,
                LastName = jsonProfile.LastName,
                Headline = jsonProfile.Headline,
                Industry = jsonProfile.Industry,
                MaidenName = jsonProfile.MaidenName,
                NumberOfConnections = jsonProfile.NumberOfConnections,
                PictureUrl = jsonProfile.PictureUrl,
                PublicProfileUrl = jsonProfile.PublicProfileUrl,
                Specialities = jsonProfile.Specialities,
                Summary = jsonProfile.Summary,
                Location = jsonProfile.Location != null ? jsonProfile.Location.Name : null
            };

            if (jsonProfile.Positions != null && jsonProfile.Positions.Positions != null)
            {
                basicProfile.Positions = new List<LinkedInPosition>();

                foreach (var pos in jsonProfile.Positions.Positions)
                {
                    LinkedInDate startDate = null, endDate = null;

                    if (pos.StartDate != null)
                    {
                        startDate = new LinkedInDate()
                        {
                            Day = pos.StartDate.Day,
                            Month = pos.StartDate.Month,
                            Year = pos.StartDate.Year
                        };
                    }

                    if (pos.EndDate != null)
                    {
                        endDate = new LinkedInDate()
                        {
                            Day = pos.EndDate.Day,
                            Month = pos.EndDate.Month,
                            Year = pos.EndDate.Year
                        };
                    }

                    basicProfile.Positions.Add(new LinkedInPosition()
                    {
                        Id = pos.Id,
                        IsCurrent = pos.IsCurrent,
                        Company = new LinkedInCompany()
                        {
                            Id = pos.Company.Id,
                            Name = pos.Company.Name,
                            CompanySize = pos.Company.Size,
                            CompanyType = pos.Company.Type,
                            Industry = pos.Company.Industry
                        },
                        StartDate = startDate,
                        EndDate = endDate
                    });
                }
            }

            return basicProfile;
        }
    }
}
