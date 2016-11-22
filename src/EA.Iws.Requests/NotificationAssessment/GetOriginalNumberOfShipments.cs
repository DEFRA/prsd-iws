namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shipment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanChangeNumberOfShipmentsOnExportNotification)]
    public class GetOriginalNumberOfShipments : IRequest<ShipmentNumberHistoryData>
    {
        public GetOriginalNumberOfShipments(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}
