namespace EA.Iws.Web.Infrastructure.VirusScanning
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class VirusFoundException : Exception
    {
        public VirusFoundException()
        {
        }

        public VirusFoundException(string message) : base(message)
        {
        }

        public VirusFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected VirusFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}