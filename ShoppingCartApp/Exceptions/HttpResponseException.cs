using System;
using System.Net;

namespace ShoppingCart.Exceptions
{
    public class HttpResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;

        public object Value { get; set; }

        public HttpResponseException(string message) : base(message)
        {
        }

        public HttpResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public HttpResponseException()
        {
        }
    }
}
