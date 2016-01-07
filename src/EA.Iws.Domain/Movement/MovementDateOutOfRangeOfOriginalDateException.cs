namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class MovementDateOutOfRangeOfOriginalDateException : MovementDateException
    {
        public MovementDateOutOfRangeOfOriginalDateException()
        {
        }

        public MovementDateOutOfRangeOfOriginalDateException(string message) : base(message)
        {
        }

        public MovementDateOutOfRangeOfOriginalDateException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MovementDateOutOfRangeOfOriginalDateException(
          SerializationInfo info,
          StreamingContext context) : base(info, context)
        {
        }
    }
}