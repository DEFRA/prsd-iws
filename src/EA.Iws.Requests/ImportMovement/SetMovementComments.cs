namespace EA.Iws.Requests.ImportMovement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanEditImportMovements)]
    public class SetMovementComments : IRequest<Unit>
    {
        public SetMovementComments(Guid movementId)
        {
            MovementId = movementId;
        }

        public Guid MovementId { get; private set; }

        public string Comments { get; set; }

        public string StatsMarking { get; set; }
    }
}