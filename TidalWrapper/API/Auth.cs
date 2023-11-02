using TidalWrapper.Exceptions;
using TidalWrapper.Responses;

namespace TidalWrapper.API
{
    /// <summary>
    /// Authorization Engine
    /// </summary>
    internal class Auth : Engine
    {
        private readonly string clientId;
        public Auth(string clientId)
        {
            this.clientId = clientId;
        }

        /// <summary>
        /// Gets a device code that can be used to authorize
        /// </summary>
        /// <returns>Device Authorization data</returns>
        /// <exception cref="APIException">API exception</exception>
        /// <exception cref="Exception">Exception if there was some internal error</exception>
        public async Task<DeviceAuthorization> GetDeviceCode()
        {
            KeyValuePair<string, string>[] content =
            {
                new("client_id", clientId),
                new("scope", "r_usr+w_usr+w_sub")
            };
            FormUrlEncodedContent requestContent = new(content);
            Response<DeviceAuthorization> deviceCode = await Request.PostJsonAsync<DeviceAuthorization>(httpClient, "https://auth.tidal.com/v1/oauth2/device_authorization", requestContent);
            if (deviceCode.Data != null)
            {
                return deviceCode.Data;
            }
            else if (deviceCode.Error != null)
            {
                throw new APIException { StatusCode = deviceCode.StatusCode, Error = deviceCode.Error };
            }
            else
            {
                throw new Exception("Could not retrieve device token.");
            }
        }

        /// <summary>
        /// Returns authorization data for a given login method
        /// </summary>
        /// <param name="loginMethod">Login method to use</param>
        /// <param name="codeOrToken">The according token or device code</param>
        /// <param name="clientId">The client id to authorize with</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">Invalid LoginMethod</exception>
        /// <exception cref="APIException">API request failure</exception>
        public async Task<OAuthToken> GetOAuthToken(LoginMethod loginMethod, string codeOrToken, string clientId)
        {
            // Choose keys according to login method
            KeyValuePair<string, string>[] content = loginMethod switch
            {
                LoginMethod.DeviceCode => new KeyValuePair<string, string>[] {
                        new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:device_code"),
                        new KeyValuePair<string, string>("device_code", codeOrToken),
                        new KeyValuePair<string, string>("client_id", clientId),
                        new KeyValuePair<string, string>("scope", "r_usr+w_usr+w_sub")
                    },
                LoginMethod.RefreshToken => new KeyValuePair<string, string>[] {
                        new KeyValuePair<string, string>("grant_type", "refresh_token"),
                        new KeyValuePair<string, string>("refresh_token", codeOrToken),
                        new KeyValuePair<string, string>("client_id", clientId),
                        new KeyValuePair<string, string>("scope", "r_usr+w_usr+w_sub")
                    },
                _ => throw new NotImplementedException(),
            };
            FormUrlEncodedContent requestContent = new(content);
            Response<OAuthToken> oAuthToken = await Request.PostJsonAsync<OAuthToken>(httpClient, "https://auth.tidal.com/v1/oauth2/token", requestContent);
            if (oAuthToken.Data != null)
            {
                return oAuthToken.Data;
            }
            else
            {
                throw new APIException { StatusCode = oAuthToken.StatusCode, Error = oAuthToken.Error };
            }
        }
    }

    /// <summary>
    /// Represents login methods
    /// </summary>
    internal enum LoginMethod
    {
        RefreshToken = 1,
        DeviceCode = 2
    }
}
