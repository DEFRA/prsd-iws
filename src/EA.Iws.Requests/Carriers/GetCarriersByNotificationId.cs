namespace EA.Iws.Requests.Carriers
{
    using System;
    using System.Collections.Generic;
    using Core.Carriers;
    using Prsd.Core.Mediator;

    public class GetCarriersByNotificationId : IRequest<IEnumerable<CarrierData>>
    {
        public GetCarriersByNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}