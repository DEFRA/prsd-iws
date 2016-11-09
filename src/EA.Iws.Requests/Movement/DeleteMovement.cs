namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanDeleteMovements)]
    public class DeleteMovement : IRequest<bool>
    { 
        public Guid MovementId { get; private set; }

        public DeleteMovement(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
