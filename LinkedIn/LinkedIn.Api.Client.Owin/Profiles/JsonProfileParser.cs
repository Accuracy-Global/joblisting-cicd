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
    public class JsonProfileParser : ILinkedInResultParser<LinkedInFullProfile, JObject>
    {
        public LinkedInFullProfile Parse(JObject json)
        {
            var jsonProfile = json.ToObject<JsonLinkedInFullProfile>();

            if (jsonProfile == null)
            {
                return null;
            }

            var fullProfile = new LinkedInFullProfile()
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
                NumberOfRecommenders = jsonProfile.NumberOfRecommenders,
                Interests = jsonProfile.Interests,
                Location = jsonProfile.Location != null ? jsonProfile.Location.Name : null
            };

            if (jsonProfile.Positions != null && jsonProfile.Positions.Positions != null)
            {
                fullProfile.Positions = new List<LinkedInPosition>();

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

                    fullProfile.Positions.Add(new LinkedInPosition()
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

            if (jsonProfile.Educations != null && jsonProfile.Educations.Educations != null)
            {
                fullProfile.Educations = new List<LinkedInEducation>();

                foreach (var education in jsonProfile.Educations.Educations)
                {
                    fullProfile.Educations.Add(new LinkedInEducation()
                    {
                        Id = education.Id,
                        SchoolName = education.SchoolName,
                        Degree = education.Degree,
                        FieldOfStudy = education.FieldOfStudy
                    });
                }
            }

            if (jsonProfile.Certifications != null && jsonProfile.Certifications.Certifications != null)
            {
                fullProfile.Certifications = new Dictionary<int, string>();

                foreach (var cert in jsonProfile.Certifications.Certifications)
                {
                    fullProfile.Certifications.Add(cert.Id, cert.Name);
                }
            }

            if (jsonProfile.DateOfBirth != null)
            {
                fullProfile.DOB = new LinkedInDate()
                {
                    Day = jsonProfile.DateOfBirth.Day,
                    Month = jsonProfile.DateOfBirth.Month,
                    Year = jsonProfile.DateOfBirth.Year
                };
            }

            if (jsonProfile.Skills != null && jsonProfile.Skills.Skills != null)
            {
                fullProfile.Skills = new Dictionary<int, string>();

                foreach (var skill in jsonProfile.Skills.Skills)
                {
                    if (skill.Skill != null)
                    {
                        fullProfile.Skills.Add(skill.Id, skill.Skill.Name);
                    }
                }
            }

            if (jsonProfile.Courses != null && jsonProfile.Courses.Courses != null)
            {
                fullProfile.Courses = new Dictionary<int, string>();

                foreach (var course in jsonProfile.Courses.Courses)
                {
                    fullProfile.Courses.Add(course.Id, course.Name);
                }
            }

            if (jsonProfile.HonorsAwards != null && jsonProfile.HonorsAwards.HonorAwards != null)
            {
                fullProfile.HonorAwards = new Dictionary<int, string>();

                foreach (var honorAward in jsonProfile.HonorsAwards.HonorAwards)
                {
                    fullProfile.HonorAwards.Add(honorAward.Id, honorAward.Name);
                }
            }

            if (jsonProfile.Languages != null && jsonProfile.Languages.Languages != null)
            {
                fullProfile.Languages = new Dictionary<int, string>();

                foreach (var lang in jsonProfile.Languages.Languages)
                {
                    if (lang.Language != null)
                    {
                        fullProfile.Languages.Add(lang.Id, lang.Language.Name);
                    }
                }
            }

            return fullProfile;
        }
    }
}
