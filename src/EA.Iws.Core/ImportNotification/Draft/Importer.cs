namespace EA.Iws.Core.ImportNotification.Draft
{
    using Shared;

    public class Importer
    {
        public Address Address { get; set; }

        public string BusinessName { get; set; }

        public BusinessType? Type { get; set; }

        public string RegistrationNumber { get; set; }

        public Contact Contact { get; set; }
    }
}
