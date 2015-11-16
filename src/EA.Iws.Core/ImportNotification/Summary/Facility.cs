namespace EA.Iws.Core.ImportNotification.Summary
{
    using Shared;

    public class Facility
    {
        public Address Address { get; set; }

        public Contact Contact { get; set; }

        public BusinessType? BusinessType { get; set; }

        public string RegistrationNumber { get; set; }

        public string Name { get; set; }

        public bool IsActualSite { get; set; }
    }
}
