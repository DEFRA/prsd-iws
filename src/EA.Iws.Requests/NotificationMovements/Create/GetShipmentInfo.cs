namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanCreateExportMovements)]
    public class GetShipmentInfo : IRequest<ShipmentInfo>
    {
        public Guid NotificationId { get; private set; }

        public GetShipmentInfo(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}