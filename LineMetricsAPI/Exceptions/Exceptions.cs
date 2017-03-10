using LineMetrics.API.ReturnTypes;
using System;

namespace LineMetrics.API.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException(string message) : base(message) { }
        public ServiceException(string message, params object[] args) : base(String.Format(message, args)) { }
    }

    public class AuthorizationException : RequestException
    {
        public AuthorizationException(ErrorResponse error) : base(error) { }
    }

    public class RequestException : ServiceException
    {
        public ErrorResponse Error { get; set; }

        public RequestException(ErrorResponse error)
            : base("{0}: {1}", (int)error.StatusCode, error.Message)
        {
            Error = error;
        }
    }
}
