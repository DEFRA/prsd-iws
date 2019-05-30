namespace EA.Iws.Requests.ImportMovement.Cancel
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportMovement;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanEditImportMovements)]
    public class CancelImportMovements : IRequest<bool>
    {
        public CancelImportMovements(Guid notificationId, IEnumerable<ImportCancelMovementData> cancelledMovements, IEnumerable<AddedCancellableMovement> addedMovements = null)
        {
            CancelledMovements = cancelledMovements;
            NotificationId = notificationId;
            AddedMovements = addedMovements ?? new List<AddedCancellableMovement>();
        }

        public IEnumerable<ImportCancelMovementData> CancelledMovements { get; private set; }

        public Guid NotificationId { get; private set; }

        public IEnumerable<AddedCancellableMovement> AddedMovements { get; private set; }
    }
}