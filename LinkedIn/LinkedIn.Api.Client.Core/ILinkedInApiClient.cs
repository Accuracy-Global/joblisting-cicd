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
    /// Represents the interface for calling LinkedIn APIs.
    /// </summary>
    public interface ILinkedInApiClient<TLinkedInResult>
    {
        /// <summary>
        /// Executes a REST-based LinkedIn API call with a given LinkedIn endpoint.
        /// </summary>
        /// <param name="endpoint">The LinkedIn endpoint specific for a particular REST-based LinkedIn API call.</param>
        /// <returns>Returns the result of an API call as an instance of the <see cref="TLinkedInResult"/> generic type.</returns>
        Task<TLinkedInResult> ExecuteAsync(string endpoint);
    }
}
