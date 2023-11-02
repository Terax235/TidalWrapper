using System.Net;

namespace TidalWrapper.Responses
{
    internal class Response<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T? Data { get; set; }
        public HttpRequestException? Error { get; set; }
    }
}
