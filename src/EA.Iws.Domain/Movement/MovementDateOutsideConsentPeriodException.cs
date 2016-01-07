namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class MovementDateOutsideConsentPeriodException : MovementDateException
    {
        public MovementDateOutsideConsentPeriodException()
        {
        }

        public MovementDateOutsideConsentPeriodException(string message) : base(message)
        {
        }

        public MovementDateOutsideConsentPeriodException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MovementDateOutsideConsentPeriodException(
          SerializationInfo info,
          StreamingContext context) : base(info, context)
        {
        }
    }
}