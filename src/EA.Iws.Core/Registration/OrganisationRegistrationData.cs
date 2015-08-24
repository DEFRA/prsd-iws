namespace EA.Iws.Core.Registration
{
    using System;
    using Shared;

    public class OrganisationRegistrationData
    {
        public Guid OrganisationId { get; set; }
        public string Name { get; set; }
        public BusinessType BusinessType { get; set; }
        public string OtherDescription { get; set; }
        public string RegistrationNumber { get; set; }
    }
}