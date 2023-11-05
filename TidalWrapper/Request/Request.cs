using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using TidalWrapper.Responses;

namespace TidalWrapper.Requests
{
    internal class Request
    {
        private static APIClient CheckClient(APIClient apiClient)
        {
            if (apiClient.UseAuthorizationToken == true && apiClient.client != null && apiClient.client.AuthCache != null)
            {
                OAuthToken authCache = apiClient.client.AuthCache;
                apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authCache.TokenType, authCache.AccessToken);
            }
            return apiClient;
        }

        /// <summary>
        /// Requests a resource json (GET)
        /// </summary>
        /// <typeparam name="T">Expected response type</typeparam>
        /// <param name="client">API client</param>
        /// <param name="url">Resource to request</param>
        /// <returns>Response data</returns>
        public static async Task<Response<T>> GetJsonAsync<T>(APIClient client, string url)
        {
            try
            {
                CheckClient(client);
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    T? data = JsonConvert.DeserializeObject<T>(json);
                    return new Response<T>
                    {
                        StatusCode = response.StatusCode,
                        Data = data
                    };
                }
                else
                {
                    return new Response<T> { StatusCode = response.StatusCode, Error = new HttpRequestException(response.ReasonPhrase) };
                }
            }
            catch (Exception ex)
            {
                return new Response<T> { StatusCode = HttpStatusCode.InternalServerError, Error = new HttpRequestException("An error occurred during the request.", ex) };
            }
        }

        /// <summary>
        /// Requests a resource json (POST)
        /// </summary>
        /// <typeparam name="T">Expected response type</typeparam>
        /// <param name="client">API client</param>
        /// <param name="url">Resource to request</param>
        /// <param name="content">Content to post</param>
        /// <returns>Response data</returns>
        public static async Task<Response<T>> PostJsonAsync<T>(APIClient client, string url, HttpContent content)
        {
            try
            {
                CheckClient(client);
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    try
                    {
                        T? data = JsonConvert.DeserializeObject<T>(json);
                        return new Response<T> { StatusCode = response.StatusCode, Data = data };
                    }
                    catch (Exception)
                    {
                        return new Response<T> { StatusCode = HttpStatusCode.BadRequest, Error = new HttpRequestException("Deserialization failed") };
                    }
                }
                else
                {
                    return new Response<T> { StatusCode = response.StatusCode, Error = new HttpRequestException(response.ReasonPhrase) };
                }
            }
            catch (Exception ex)
            {
                return new Response<T> { StatusCode = HttpStatusCode.InternalServerError, Error = new HttpRequestException("An error occurred during the request.", ex) };
            }
        }
    }
}
