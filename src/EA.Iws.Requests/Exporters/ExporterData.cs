namespace EA.Iws.Requests.Exporters
{
    using System;
    using Shared;

    public class ExporterData
    {
        public Guid Id { get; set; }

        public BusinessData Business { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public Guid NotificationId { get; set; }

        public ExporterData()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessData();
        }
    }
}
