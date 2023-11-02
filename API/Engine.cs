using System.Net.Http.Headers;
using TidalWrapper.Responses;

namespace TidalWrapper.API
{
    public abstract class Engine
    {
        internal readonly HttpClient httpClient = Request.CreateClient();

        internal void SetAuth(OAuthToken auth)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(auth.TokenType, auth.AccessToken);
        }
    }
}
