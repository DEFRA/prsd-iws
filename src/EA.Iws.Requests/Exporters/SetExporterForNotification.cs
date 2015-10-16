namespace EA.Iws.Requests.Exporters
{
    using System;
    using Core.Authorization;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization("Set Exporter For Export Notification")]
    public class SetExporterForNotification : IRequest<Guid>
    {
        public BusinessInfoData Business { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public Guid NotificationId { get; set; }
    }
}