using System;
using System.Runtime.Serialization;

namespace Incident.Comm.Integration.Api.Exceptions
{
    [Serializable]
    public class AppException : Exception
    {
        public AppException()
            : base()
        {
        }

        public AppException(string message)
            : base(message)
        {
        }

        protected AppException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
