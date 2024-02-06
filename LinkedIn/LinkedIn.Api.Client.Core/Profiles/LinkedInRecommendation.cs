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
    /// <summary>
    /// 
    /// </summary>
    public class LinkedInRecommendation
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or Sets Recommender
        /// </summary>
        public LinkedInRecommender Recommender
        {
            get;
            set;
        }
        /// <summary>
        /// Recommendation Text
        /// </summary>
        public string RecommendationText
        {
            get;
            set;
        }

        /// <summary>
        /// Recommendation Type
        /// </summary>
        public string RecommendationType
        {
            get;
            set;
        }
    }
}
