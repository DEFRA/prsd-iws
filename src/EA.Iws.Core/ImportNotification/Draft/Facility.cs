namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;
    using Shared;

    public class Facility
    {
        public Guid Id { get; set; }

        public Address Address { get; set; }

        public string BusinessName { get; set; }

        public BusinessType? Type { get; set; }

        public string RegistrationNumber { get; set; }

        public Contact Contact { get; set; }

        public bool IsSiteOfExport { get; set; }
    }
}
