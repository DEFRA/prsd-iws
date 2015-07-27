namespace EA.Iws.Domain.Events
{
    using Prsd.Core.Domain;

    public class RegistrationApprovedEvent : IEvent
    {
        public string EmailAddress { get; private set; }

        public RegistrationApprovedEvent(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
    }
}