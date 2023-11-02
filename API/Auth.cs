using TidalWrapper.Exceptions;
using TidalWrapper.Responses;

namespace TidalWrapper.API
{
    internal class Auth : Engine
    {
        private readonly string clientId;
        public Auth(string clientId)
        {
            this.clientId = clientId;
        }
        public async Task<DeviceAuthorization> GetDeviceCode()
        {
            KeyValuePair<string, string>[] content =
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("scope", "r_usr+w_usr+w_sub")
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

    internal enum LoginMethod
    {
        RefreshToken = 1,
        DeviceCode = 2
    }
}
