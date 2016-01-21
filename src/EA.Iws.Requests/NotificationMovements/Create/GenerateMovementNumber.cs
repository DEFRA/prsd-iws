namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanCreateExportMovements)]
    public class GenerateMovementNumber : IRequest<int>
    {
        public Guid NotificationId { get; private set; }

        public GenerateMovementNumber(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}