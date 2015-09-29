namespace EA.Iws.Requests.Exporters
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    public class SetExporterForNotification : IRequest<Guid>
    {
        public BusinessInfoData Business { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public Guid NotificationId { get; set; }
    }
}