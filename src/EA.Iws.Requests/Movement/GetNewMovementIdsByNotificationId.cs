namespace EA.Iws.Requests.Movement
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetNewMovementIdsByNotificationId : IRequest<IList<MovementData>>
    {
        public Guid Id { get; private set; }

        public GetNewMovementIdsByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}
