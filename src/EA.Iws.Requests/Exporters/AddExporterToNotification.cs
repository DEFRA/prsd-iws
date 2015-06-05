namespace EA.Iws.Requests.Exporters
{
    using System;
    using Prsd.Core.Mediator;
    using Shared;

    public class AddExporterToNotification : IRequest<Guid>
    {
        public BusinessInfoData Business { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public Guid NotificationId { get; set; }
    }
}