/* ============================================================================================================================
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
 * =========================================================================================================================== */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedIn.Api.Client.Core.Companies
{
    /// <summary>
    /// 
    /// </summary>
    public class LinkedInCompany
    {
        /// <summary>
        /// Id
        /// </summary>
        public int? Id
        {
            get;
            set;
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Industry
        /// </summary>
        public String Industry
        {
            get;
            set;
        }

        /// <summary>
        /// Size
        /// </summary>
        public string CompanySize
        {
            get;
            set;
        }

        /// <summary>
        /// Type
        /// </summary>
        public string CompanyType
        {
            get;
            set;
        }
    }
}
