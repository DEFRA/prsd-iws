namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanCreateExportMovements)]
    public class GetRemainingShipments : IRequest<RemainingShipmentsData>
    {
        public GetRemainingShipments(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}