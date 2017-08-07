namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovementsExternal)]
    public class SetMultipleMovementFileId : IRequest<Guid>
    {
        public SetMultipleMovementFileId(Guid notificationId, Guid[] movementIds, byte[] movementBytes, string fileType)
        {
            NotificationId = notificationId;
            MovementIds = movementIds;
            MovementBytes = movementBytes;
            FileType = fileType;
        }

        public Guid NotificationId { get; private set; }

        public Guid[] MovementIds { get; private set; }

        public byte[] MovementBytes { get; private set; }

        public string FileType { get; private set; }
    }
}
