namespace EA.Iws.Core.Registration
{
    using System;
    using Shared;

    public class OrganisationRegistrationData
    {
        public Guid OrganisationId { get; set; }
        public string Name { get; set; }
        public string TownOrCity { get; set; }
        public string CountyOrProvince { get; set; }
        public string Building { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Postcode { get; set; }
        public Guid CountryId { get; set; }
        public BusinessType BusinessType { get; set; }
        public string OtherDescription { get; set; }
        public string RegistrationNumber { get; set; }
    }
}