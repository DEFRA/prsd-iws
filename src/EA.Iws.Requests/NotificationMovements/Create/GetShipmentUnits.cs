namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanCreateExportMovements)]
    public class GetShipmentUnits : IRequest<ShipmentQuantityUnits>
    {
        //TODO: This request is being used in places other than "Create Movements", 
        // so should move to a different namespace and have a different permission?

        public Guid NotificationId { get; private set; }

        public GetShipmentUnits(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}