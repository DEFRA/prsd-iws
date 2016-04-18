namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class MovementDateOutOfRangeException : MovementDateException
    {
        public MovementDateOutOfRangeException()
        {
        }

        public MovementDateOutOfRangeException(string message) : base(message)
        {
        }

        public MovementDateOutOfRangeException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MovementDateOutOfRangeException(
          SerializationInfo info,
          StreamingContext context) : base(info, context)
        {
        }
    }
}