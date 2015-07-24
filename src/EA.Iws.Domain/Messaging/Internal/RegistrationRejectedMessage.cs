namespace EA.Iws.Domain.Messaging.Internal
{
    public class RegistrationRejectedMessage
    {
        public string EmailAddress { get; private set; }

        public RegistrationRejectedMessage(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
    }
}
