namespace EA.Iws.Requests.IntendedShipments
{
    using System;
    using Core.IntendedShipments;
    using Prsd.Core.Mediator;

    public class GetIntendedShipmentInfoForNotification : IRequest<IntendedShipmentData>
    {
        public GetIntendedShipmentInfoForNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}