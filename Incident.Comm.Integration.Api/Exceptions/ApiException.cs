using System;

namespace Incident.Comm.Integration.Api.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }
    }
}
