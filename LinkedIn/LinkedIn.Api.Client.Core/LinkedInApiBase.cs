/* ============================================================================================================================
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
 * =========================================================================================================================== */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedIn.Api.Client.Core
{
    /// <summary>
    /// Represents the base functionality for a LinkedIn API class.
    /// </summary>
    public abstract class LinkedInApiBase<TLinkedInApiClient, TLinkedInResult>
        where TLinkedInApiClient : ILinkedInApiClient<TLinkedInResult>
    {
        /// <summary>
        /// Creates an instance of the <see cref="LinkedInApiBase"/> class.
        /// </summary>
        /// <param name="client">The object used by <see cref="LinkedInApiBase"/> to execute LinkedIn REST APIs.</param>
        protected LinkedInApiBase(TLinkedInApiClient client)
        {
            if(client == null)
            {
                throw new ArgumentNullException("client");
            }

            Client = client;
        }

        /// <summary>
        /// Gets the current LinkedIn API client.
        /// </summary>
        protected TLinkedInApiClient Client
        {
            get;
            private set;
        }
    }
}
