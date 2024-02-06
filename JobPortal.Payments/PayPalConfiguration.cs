using PayPal.Api;
using System.Collections.Generic;

namespace JobPortal.Payments
{
    /// <summary>
    /// Represents the application level configuration for the paypal API integration.
    /// </summary>
    public static class PayPalConfiguration
    {
        #region Fields
        private readonly static string _ClientId;
        private readonly static string _ClientSecret;
        #endregion


        #region Constructor
        /// <summary>
        /// For setting the readonly static members.
        /// </summary>
        static PayPalConfiguration()
        {
            Dictionary<string, string> config = GetConfig();
            _ClientId = config["clientId"];
            _ClientSecret = config["clientSecret"];
        }
        #endregion


        /// <summary>
        /// Create the configuration map that contains mode and other optional configuration details.
        /// </summary>
        public static Dictionary<string, string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();

            // Added for testing purpose.
            //return new Dictionary<string, string>() {
            //    { "clientId", "AQKdi1cdq0CxRxijWd6DnukAvkmRKTVTLvpSbNHsYXAxBMbvMMJHiOhVVyuBr4FafO1VhQ91d7IvZG8e" },
            //    { "clientSecret", "EK2drrYoJIXIUivHTZ98uxryePBTAHiXf5mHf2ItY0YjRAb833Wz8jYdnXvWkH95HYQxXBdjk1fwp6nK" }
            //};
        }

        /// <summary>
        /// Returns the paypal API context information based on access toekn.
        /// </summary>
        /// <param name="accessToken">Optional access token information.</param>
        public static APIContext GetContext(string accessToken = null)
        {
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 
            var apiContext = new APIContext(string.IsNullOrWhiteSpace(accessToken) ? GetAccessToken() : accessToken);
            apiContext.Config = GetConfig();
            return apiContext;
        }

        /// <summary>
        /// Returns the access token information.
        /// </summary>
        /// <returns></returns>
        private static string GetAccessToken()
        {
            // ###AccessToken
            // Retrieve the access token from OAuthTokenCredential by passing in
            // ClientID and ClientSecret
            // It is not mandatory to generate Access Token on a per call basis.
            // Typically the access token can be generated once and reused within the expiry window                
            string accessToken = new OAuthTokenCredential(_ClientId, _ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }
    }
}
