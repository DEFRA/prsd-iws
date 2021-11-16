namespace EA.Iws.Core.ImportNotification.Summary
{
    using EA.Iws.Core.Shared;

    public class Producer
    {
        public string Name { get; set; }

        public Contact Contact { get; set; }

        public Address Address { get; set; }

        public bool AreMultiple { get; set; }

        public BusinessType BusinessType { get; set; }

        public string RegistrationNumber { get; set; }

        public bool IsEmpty()
        {
            return Address.IsEmpty()
                && Contact.IsEmpty()
                && string.IsNullOrWhiteSpace(Name);
        }
    }
}
