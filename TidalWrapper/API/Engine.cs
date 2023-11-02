using System.Net.Http.Headers;
using TidalWrapper.Responses;

namespace TidalWrapper.API
{
    /// <summary>
    /// Abstract Engine
    /// </summary>
    public abstract class Engine
    {
        /// <summary>
        /// HTTP client for the engine
        /// </summary>
        internal readonly HttpClient httpClient = Request.CreateClient();

        /// <summary>
        /// Updates authorization data for the engine
        /// </summary>
        /// <param name="auth"></param>
        internal void SetAuth(OAuthToken auth)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(auth.TokenType, auth.AccessToken);
        }
    }
}
