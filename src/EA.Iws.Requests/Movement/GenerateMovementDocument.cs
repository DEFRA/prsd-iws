namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Documents;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GenerateMovementDocument : IRequest<FileData>
    {
        public Guid Id { get; private set; }

        public GenerateMovementDocument(Guid id)
        {
            Id = id;
        }
    }
}
