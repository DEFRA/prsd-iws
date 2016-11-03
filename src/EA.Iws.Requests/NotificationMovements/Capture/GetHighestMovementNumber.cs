namespace EA.Iws.Requests.NotificationMovements.Capture
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovementsInternal)]
    public class GetLatestMovementNumber : IRequest<int>
    {
        public Guid NotificationId { get; private set; }

        public GetLatestMovementNumber(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
