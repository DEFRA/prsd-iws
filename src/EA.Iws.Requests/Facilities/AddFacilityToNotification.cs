namespace EA.Iws.Requests.Facilities
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Shared;

    public class AddFacilityToNotification : IRequest<Guid>
    {
        public BusinessData Business { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public Guid NotificationId { get; set; }
    }
}
