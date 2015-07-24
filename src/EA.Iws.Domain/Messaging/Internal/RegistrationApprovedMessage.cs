namespace EA.Iws.Domain.Messaging.Internal
{
    public class RegistrationApprovedMessage
    {
        public string EmailAddress { get; private set; }

        public RegistrationApprovedMessage(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
    }
}
