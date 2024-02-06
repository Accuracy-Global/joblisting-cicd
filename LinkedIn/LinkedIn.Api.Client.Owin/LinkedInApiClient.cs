/* ============================================================================================================================
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
 * =========================================================================================================================== */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LinkedIn.Api.Client.Core;

namespace LinkedIn.Api.Client.Owin
{
    /// <summary>
    /// Represents the context for calling OAuth-based REST APIs of LinkedIn.
    /// </summary>
    public class LinkedInApiClient : ILinkedInApiClient<JObject>, IDisposable
    {
        private HttpClient _HttpClient;
        private IOwinRequest _Request;
        private string _AccessToken;

        /// <summary>
        /// Creates an instance of the <see cref="LinkedInApiClient"/> class.
        /// </summary>
        /// <param name="request">The <see cref="IOwinRequest"/> object which is used by the context to send REST-based requests to LinkedIn API server.</param>
        /// <param name="accessToken">The access token of the user on whose behalf the context can be used to make REST calls.</param>
        public LinkedInApiClient(IOwinRequest request, string accessToken)
        {
            if(request == null)
            {
                throw new ArgumentNullException("request");
            }

            if(string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException("accessToken");
            }

            _HttpClient = new HttpClient();
            _Request = request;
            _AccessToken = accessToken;
        }

        /// <summary>
        /// Executes a LinkedIn REST API call.
        /// </summary>
        /// <param name="endpoint">The LinkedIn API endpoint.</param>
        /// <returns>Returns the result of the API call as an instance of the <see cref="JObject"/> class.</returns>
        public async Task<JObject> ExecuteAsync(string endpoint)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, endpoint + "?oauth2_access_token=" + Uri.EscapeDataString(_AccessToken));
                request.Headers.Add("x-li-format", "json");
                var response = await _HttpClient.SendAsync(request, _Request.CallCancelled);
                response.EnsureSuccessStatusCode();
                var text = await response.Content.ReadAsStringAsync();
                return JObject.Parse(text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Disposes the resources of the context.
        /// </summary>
        public void Dispose()
        {
            if(_HttpClient != null)
            {
                _HttpClient.Dispose();
                _HttpClient = null;
            }
        }
    }
}