namespace EA.Iws.Core.ImportNotification.Summary
{
    using Shared;

    public class Importer
    {
        public Address Address { get; set; }

        public Contact Contact { get; set; }

        public BusinessType? BusinessType { get; set; }

        public string Name { get; set; }

        public string RegistrationNumber { get; set; }

        public bool IsEmpty()
        {
            return Address.IsEmpty() &&
                   Contact.IsEmpty() &&
                   !BusinessType.HasValue &&
                   string.IsNullOrWhiteSpace(Name) &&
                   string.IsNullOrWhiteSpace(RegistrationNumber);
        }
    }
}
