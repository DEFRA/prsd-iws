namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using System.Collections.Generic;
    using Core.Carriers;
    using Prsd.Core.Mediator;

    public class GetCarriers : IRequest<IList<CarrierData>>
    {
        public Guid NotificationId { get; private set; }

        public GetCarriers(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}