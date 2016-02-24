namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetMovementSummary : IRequest<MovementSummary>
    {
        public Guid Id { get; private set; }

        public GetMovementSummary(Guid id)
        {
            Id = id;
        }
    }
}
