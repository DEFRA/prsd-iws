namespace EA.Iws.Domain.Events
{
    using Prsd.Core.Domain;

    public class RegistrationRejectedEvent : IEvent
    {
        public string EmailAddress { get; private set; }

        public RegistrationRejectedEvent(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
    }
}