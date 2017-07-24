namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovementsInternal)]
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