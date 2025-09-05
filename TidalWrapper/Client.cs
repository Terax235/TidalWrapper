using TidalWrapper.Engines;
using TidalWrapper.Responses;
using TidalWrapper.Util;
using System.Net;
using TidalWrapper.Exceptions;

namespace TidalWrapper
{
    public class Client
    {
        public readonly string ClientId;
        private string? DeviceCode;
        private CustomTimer? DeviceCodeTimer;
        public OAuthToken? AuthCache;
        internal AuthEngine Auth;
        [Obsolete("Using the search engine is deprecated. Future methods will go in according engines, including Track Engine, Album engine etc.")]
        public SearchEngine SearchEngine;
        public TrackEngine Tracks;

        public Client(string clientId)
        {
            ClientId = clientId;
            Auth = new(this, false);
            SearchEngine = new(this, true);
            Tracks = new(this, true);
        }

        /// <summary>
        /// Returns the users country code
        /// </summary>
        public string? CountryCode
        {
            get { return AuthCache?.User.CountryCode; }
        }

        /// <summary>
        /// Uses an interactive prompt to authorize using a device code. You will have to open the url that will be printed and login with your account to generate an access token.
        /// </summary>
        /// <returns>Whether the authorization was successful or not</returns>
        /// <exception cref="Exception">Exception if the authorization errored at some point</exception>
        public async Task<bool> LoginWithDeviceCode()
        {
            DeviceAuthorization deviceCode = await Auth.GetDeviceCode();
            Console.WriteLine("Open up https://" + deviceCode.VerificationUriComplete + " and login with your account. You have " + deviceCode.ExpiresIn + " seconds to complete this step.");
            DeviceCode = deviceCode.DeviceCode;
            DeviceCodeTimer = new(10, deviceCode.ExpiresIn, CheckDeviceCode);
            await DeviceCodeTimer.WaitForCompletion();
            if (AuthCache != null)
            {
                StaticEngine.SetAuth(AuthCache);
                return true;
            }
            else
            {
                throw new Exception("Login with device code failed.");
            }
        }

        /// <summary>
        /// Authorizes using a given refresh token
        /// </summary>
        /// <param name="refreshToken">The refresh token to use</param>
        /// <returns>Whether the authorization was successful or not</returns>
        /// <exception cref="Exception">Exception if the authorization errored at some point</exception>
        public async Task<bool> LoginWithRefreshToken(string refreshToken)
        {
            OAuthToken oAuthToken = await Auth.GetOAuthToken(LoginMethod.RefreshToken, refreshToken, ClientId);
            if (oAuthToken != null)
            {
                // Store same refresh token since refresh token login does not update the current one.
                oAuthToken.RefreshToken ??= refreshToken;
                AuthCache = oAuthToken;
                StaticEngine.SetAuth(AuthCache);
                return true;
            }
            else
            {
                throw new Exception("Login with refresh token failed.");
            }
        }

        /// <summary>
        /// Interactive device code prompt check
        /// </summary>
        /// <returns>Task</returns>
        private async Task CheckDeviceCode()
        {
            if (DeviceCodeTimer == null) { return; }
            if (DeviceCode == null)
            {
                DeviceCodeTimer.Stop();
                return;
            }
            Console.WriteLine("Checking for token...");
            try
            {
                OAuthToken oAuthToken = await Auth.GetOAuthToken(LoginMethod.DeviceCode, DeviceCode, ClientId);
                DeviceCodeTimer.Stop();
                Console.WriteLine("Successfully logged in using device code.");
                AuthCache = oAuthToken;
            }
            catch (APIException ex)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    Console.WriteLine("Login not completed, waiting...");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}