namespace EA.Iws.Domain.ImportNotification
{
    using Prsd.Core;

    public class Contact
    {
        protected Contact()
        {
        }

        public Contact(string name, PhoneNumber phoneNumber, EmailAddress emailAddress)
        {
            Guard.ArgumentNotNullOrEmpty(() => name, name);
            Guard.ArgumentNotNull(() => phoneNumber, phoneNumber);
            Guard.ArgumentNotNull(() => emailAddress, emailAddress);

            Name = name;
            Telephone = phoneNumber;
            Email = emailAddress;
        }

        public string Name { get; private set; }

        public PhoneNumber Telephone { get; private set; }

        public EmailAddress Email { get; private set; }
    }
}