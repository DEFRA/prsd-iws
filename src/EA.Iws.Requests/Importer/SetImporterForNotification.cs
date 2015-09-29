namespace EA.Iws.Requests.Importer
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    public class SetImporterForNotification : IRequest<Guid>
    {
        public BusinessInfoData Business { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public Guid NotificationId { get; set; }
    }
}
