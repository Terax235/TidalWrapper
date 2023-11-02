using TidalWrapper.API;
using TidalWrapper.Responses;
using TidalWrapper.Util;
using System.Net;
using TidalWrapper.Exceptions;
using System.Net.Http.Headers;
using System.Net.Http;

namespace TidalWrapper
{
    public class Client
    {
        readonly string ClientId;
        private string? DeviceCode;
        private CustomTimer? DeviceCodeTimer;
        private OAuthToken? AuthCache;

        internal Auth AuthEngine;
        public Search SearchEngine;

        public Client(string clientId)
        {
            this.ClientId = clientId;
            AuthEngine = new(clientId);
            SearchEngine = new();
        }

        /// <summary>Updates the authorization for engines that need it.</summary>
        private void UpdateAuth()
        {
            if (AuthCache != null)
            {
                SearchEngine.SetAuth(AuthCache);
                StaticEngine.SetAuth(AuthCache);
            }
        }

        public FormattedAuth? Auth
        {
            get
            {
                if (AuthCache == null || AuthCache.RefreshToken == null) return null;
                else return new FormattedAuth { AccessToken = AuthCache.AccessToken, RefreshToken = AuthCache.RefreshToken, ClientId = ClientId };
            }
        }

        public string? CountryCode
        {
            get { return AuthCache?.User.CountryCode; }
        }

        public async Task<bool> LoginWithDeviceCode()
        {
            DeviceAuthorization deviceCode = await AuthEngine.GetDeviceCode();
            Console.WriteLine("Open up https://" + deviceCode.VerificationUriComplete + " and login with your account. You have " + deviceCode.ExpiresIn + " seconds to complete this step.");
            this.DeviceCode = deviceCode.DeviceCode;
            DeviceCodeTimer = new(10, deviceCode.ExpiresIn, CheckDeviceCode);
            await DeviceCodeTimer.WaitForCompletion();
            if (AuthCache != null)
            {
                UpdateAuth();
                return true;
            }
            else
            {
                throw new Exception("Login with device code failed.");
            }
        }

        public async Task<bool> LoginWithRefreshToken(string refreshToken)
        {
            OAuthToken oAuthToken = await AuthEngine.GetOAuthToken(LoginMethod.RefreshToken, refreshToken, ClientId);
            if (oAuthToken != null)
            {
                // Store same refresh token since refresh token login does not update the current one.
                oAuthToken.RefreshToken ??= refreshToken;
                AuthCache = oAuthToken;
                UpdateAuth();
                return true;
            }
            else
            {
                throw new Exception("Login with refresh token failed.");
            }
        }

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
                OAuthToken oAuthToken = await AuthEngine.GetOAuthToken(LoginMethod.DeviceCode, DeviceCode, ClientId);
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
                    throw ex;
                }
            }
        }
    }
}