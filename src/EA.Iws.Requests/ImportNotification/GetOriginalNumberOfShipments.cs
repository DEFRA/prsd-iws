namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shipment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanChangeNumberOfShipmentsOnImportNotification)]
    public class GetOriginalNumberOfShipments : IRequest<ShipmentNumberHistoryData>
    {
        public GetOriginalNumberOfShipments(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}
