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
    /// Represents the interface for parsing the raw result from a LinkedIn API call and transforming it into a more friendly and object-oriented type.
    /// </summary>
    /// <typeparam name="TLinkedInResult">The generic type of the object returned after the parsing is complete.</typeparam>
    /// <typeparam name="TLinkedInRawResult">The generic type of the object that represents the raw result from LinkedIn.</typeparam>
    public interface ILinkedInResultParser<TLinkedInResult, TLinkedInRawResult>
    {
        /// <summary>
        /// Parses the raw result from LinkedIn and transforms it into a friendly .NET object.
        /// </summary>
        /// <param name="result">The argument that represents the raw data from LinkedIn.</param>
        /// <returns>Returns an instance of the generic type <see cref="TLinkedInResult"/>.</returns>
        TLinkedInResult Parse(TLinkedInRawResult result);
    }
}
