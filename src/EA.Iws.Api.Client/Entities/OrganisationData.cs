namespace EA.Iws.Api.Client.Entities
{
    using System;

    public class OrganisationData
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public AddressData Address { get; set; }
    }
}
