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
    public class LinkedInBasicProfile
    {
        public string Id
        {
            get;
            set;
        }

        public string EmailAddress
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string FormattedName
        {
            get;
            set;
        }

        public string Headline
        {
            get;
            set;
        }

        public string Location
        {
            get;
            set;
        }

        public string MaidenName
        {
            get;
            set;
        }

        public string PictureUrl
        {
            get;
            set;
        }

        public string PublicProfileUrl
        {
            get;
            set;
        }

        public string Industry
        {
            get;
            set;
        }

        public int NumberOfConnections
        {
            get;
            set;
        }

        public string Summary
        {
            get;
            set;
        }

        public string Specialities
        {
            get;
            set;
        }

        public List<LinkedInPosition> Positions
        {
            get;
            set;
        }
    }
}
