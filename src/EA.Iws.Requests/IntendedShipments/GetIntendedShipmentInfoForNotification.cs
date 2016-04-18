namespace EA.Iws.Requests.IntendedShipments
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.IntendedShipments;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetIntendedShipmentInfoForNotification : IRequest<IntendedShipmentData>
    {
        public GetIntendedShipmentInfoForNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}