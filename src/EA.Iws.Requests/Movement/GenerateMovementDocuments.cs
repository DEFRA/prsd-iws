namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Documents;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GenerateMovementDocuments : IRequest<FileData>
    {
        public Guid[] MovementIds { get; private set; }

        public GenerateMovementDocuments(Guid[] movementIds)
        {
            MovementIds = movementIds;
        }
    }
}
