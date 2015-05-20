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
            if (Address == null)
            {
                Address = new AddressData();
            }

            if (Contact == null)
            {
                Contact = new ContactData();
            }

            if (Business == null)
            {
                Business = new BusinessData();
            }
        }
    }
}
