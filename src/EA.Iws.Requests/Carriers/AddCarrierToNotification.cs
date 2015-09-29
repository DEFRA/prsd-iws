namespace EA.Iws.Requests.Carriers
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    public class AddCarrierToNotification : IRequest<Guid>
    {
        public Guid NotificationId { get; set; }

        public BusinessInfoData Business { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }
    }
}
