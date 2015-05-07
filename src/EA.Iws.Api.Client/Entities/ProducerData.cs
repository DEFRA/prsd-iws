namespace EA.Iws.Api.Client.Entities
{
    using System;

    public class ProducerData
    {
        public string Name { get; set; }
        public bool IsSiteOfExport { get; set; }
        public string Type { get; set; }
        public string CompaniesHouseNumber { get; set; }
        public string RegistrationNumber1 { get; set; }
        public string RegistrationNumber2 { get; set; }
        public AddressData Address { get; set; }
        public ContactData Contact { get; set; }
        public Guid NotificationId { get; set; }
    }
}
