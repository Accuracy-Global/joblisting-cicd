/* ============================================================================================================================
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
 * =========================================================================================================================== */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedIn.Api.Client.Owin.Json.Profiles
{
    public class JsonLinkedInFullProfile : JsonLinkedInBasicProfile
    {
        [JsonProperty("certifications")]
        public JsonLinkedInCertificationList Certifications { get; set; }
        
        [JsonProperty("courses")]
        public JsonLinkedInCourseList Courses { get; set; }

        [JsonProperty("dateOfBirth")]
        public JsonLinkedInDate DateOfBirth { get; set; }

        [JsonProperty("educations")]
        public JsonLinkedInEducationList Educations { get; set; }

        [JsonProperty("honorsAwards")]
        public JsonLinkedInHonorAwardList HonorsAwards { get; set; }

        [JsonProperty("interests")]
        public string Interests { get; set; }

        [JsonProperty("languages")]
        public JsonLinkedInLanguageList Languages { get; set; }

        [JsonProperty("numRecommenders")]
        public int NumberOfRecommenders { get; set; }

        [JsonProperty("recommendationsReceived")]
        public JsonLinkedInRecommendationList RecommendationsReceived { get; set; }

        [JsonProperty("skills")]
        public JsonLinkedInSkillList Skills { get; set; }
    }
}
