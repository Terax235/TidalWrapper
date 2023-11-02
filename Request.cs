using Newtonsoft.Json;
using System.Net;
using TidalWrapper.Responses;

namespace TidalWrapper
{
    internal class Request
    {
        public static string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) TIDAL/2.30.4 Chrome/91.0.4472.164 Electron/13.6.9 Safari/537.36"; // Todo

        /// <summary>
        /// Creates a new HTTP client
        /// </summary>
        /// <returns>New HTTP client</returns>
        public static HttpClient CreateClient()
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            return client;
        }

        /// <summary>
        /// Requests a resource json (GET)
        /// </summary>
        /// <typeparam name="T">Expected response type</typeparam>
        /// <param name="client">HTTP client</param>
        /// <param name="url">Resource to request</param>
        /// <returns>Response data</returns>
        public static async Task<Response<T>> GetJsonAsync<T>(HttpClient client, string url)
        {
            try
            {
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
        /// <param name="client">HTTP client</param>
        /// <param name="url">Resource to request</param>
        /// <param name="content">Content to post</param>
        /// <returns>Response data</returns>
        public static async Task<Response<T>> PostJsonAsync<T>(HttpClient client, string url, HttpContent content)
        {
            try
            {
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
