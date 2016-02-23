namespace EA.Iws.Requests.NotificationMovements
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.MovementOperation;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovementsExternal)]
    public class GetReceivedMovements : IRequest<MovementOperationData>
    {
        public Guid NotificationId { get; private set; }

        public GetReceivedMovements(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
