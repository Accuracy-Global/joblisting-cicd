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
    public class JsonLinkedInRecommendation
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("recommendationText")]
        public string RecommendationText { get; set; }

        [JsonProperty("recommendationType")]
        public JsonLinkedInRecommendationType RecommendationType { get; set; }

        [JsonProperty("recommender")]
        public JsonLinkedInRecommender Recommender { get; set; }
    }
}
