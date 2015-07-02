namespace EA.Iws.Requests.Producers
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class AddProducerToNotification : IRequest<Guid>
    {
        public BusinessInfoData Business { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public Guid NotificationId { get; set; }
    }
}