namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class MovementNumberException : Exception
    {
        public MovementNumberException()
        {
        }

        public MovementNumberException(string message) : base(message)
        {
        }

        public MovementNumberException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MovementNumberException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
