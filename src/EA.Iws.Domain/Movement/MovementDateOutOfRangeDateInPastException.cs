namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class MovementDateOutOfRangeDateInPastException : MovementDateException
    {
        public MovementDateOutOfRangeDateInPastException()
        {
        }

        public MovementDateOutOfRangeDateInPastException(string message) : base(message)
        {
        }

        public MovementDateOutOfRangeDateInPastException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MovementDateOutOfRangeDateInPastException(
          SerializationInfo info,
          StreamingContext context) : base(info, context)
        {
        }
    }
}
