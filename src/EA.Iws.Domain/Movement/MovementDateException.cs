namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class MovementDateException : Exception
    {
        public MovementDateException()
        {
        }

        public MovementDateException(string message) : base(message)
        {
        }

        public MovementDateException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MovementDateException(
          SerializationInfo info,
          StreamingContext context) : base(info, context)
        {
        }
    }
}