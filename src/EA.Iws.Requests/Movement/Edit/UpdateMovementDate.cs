namespace EA.Iws.Requests.Movement.Edit
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovements)]
    public class UpdateMovementDate : IRequest<bool>
    {
        public Guid MovementId { get; private set; }
        public DateTime NewDate { get; private set; }

        public UpdateMovementDate(Guid movementId, DateTime newDate)
        {
            NewDate = newDate;
            MovementId = movementId;
        }
    }
}