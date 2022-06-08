using System;

namespace ShoppingCart.Exceptions
{
    public class MiddlewareException : Exception
    {
        public ExceptionCode Code { get; set; } = ExceptionCode.Unknown;
        public MiddlewareException(ExceptionCode code) : this(code, string.Empty)
        {
        }

        public MiddlewareException(ExceptionCode code, string message) : this(code, message, null)
        {
        }

        public MiddlewareException(ExceptionCode code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }
    }
}
