namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanCreateExportMovements)]
    public class GetShipmentDates : IRequest<ShipmentDates>
    {
        public Guid NotificationId { get; private set; }

        public GetShipmentDates(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}