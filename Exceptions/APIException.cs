using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TidalWrapper.Exceptions
{
    public class APIException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public HttpRequestException? Error { get; set; }
    }
}
