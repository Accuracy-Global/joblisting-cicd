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
    public class JsonLinkedInBasicProfile
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("maidenName")]
        public string MaidenName { get; set; }

        [JsonProperty("location")]
        public JsonLinkedInLocation Location { get; set; }

        [JsonProperty("formattedName")]
        public string FormattedName { get; set; }

        [JsonProperty("headline")]
        public string Headline { get; set; }

        [JsonProperty("industry")]
        public string Industry { get; set; }

        [JsonProperty("numConnections")]
        public int NumberOfConnections { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("positions")]
        public JsonLinkedInPositionList Positions { get; set; }

        [JsonProperty("pictureUrl")]
        public string PictureUrl { get; set; }

        [JsonProperty("publicProfileUrl")]
        public string PublicProfileUrl { get; set; }

        [JsonProperty("specialities")]
        public string Specialities { get; set; }
    }
}
