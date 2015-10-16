namespace EA.Iws.Core.Authorization
{
    using System;
    using System.Runtime.Serialization;
    using Prsd.Core.Mediator;

    [Serializable]
    public class RequestAuthorizationException : Exception
    {
        public RequestAuthorizationException()
        {
        }

        public RequestAuthorizationException(string message) : base(message)
        {
        }

        public RequestAuthorizationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RequestAuthorizationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public static RequestAuthorizationException CreateForRequest<T>(IRequest<T> request)
        {
            return new RequestAuthorizationException("Access denied for request: " + request.GetType().Name);
        }
    }
}
