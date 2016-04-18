namespace EA.Iws.Requests.Carriers
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Carriers;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetCarrierForNotification : IRequest<CarrierData>
    {
        public GetCarrierForNotification(Guid notificationId, Guid carrierId)
        {
            CarrierId = carrierId;
            NotificationId = notificationId;
        }

        public Guid CarrierId { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}