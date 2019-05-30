namespace EA.Iws.Requests.Movement
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovements)]
    public class CancelMovements : IRequest<bool>
    {
        public IEnumerable<MovementData> CancelledMovements { get; private set; }

        public IEnumerable<AddedCancellableMovement> AddedMovements { get; private set; }

        public Guid NotificationId { get; set; }

        public CancelMovements(Guid notificationId, IEnumerable<MovementData> cancelledMovements,
            IEnumerable<AddedCancellableMovement> addedMovements = null)
        {
            NotificationId = notificationId;
            CancelledMovements = cancelledMovements;
            AddedMovements = addedMovements ?? new List<AddedCancellableMovement>();
        }
    }
}
