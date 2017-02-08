namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetMovementFiles : IRequest<MovementFiles>
    {
        public GetMovementFiles(Guid notificationId, int pageNumber)
        {
            NotificationId = notificationId;
            PageNumber = pageNumber;
        }

        public Guid NotificationId { get; private set; }

        public int PageNumber { get; private set; }
    }
}