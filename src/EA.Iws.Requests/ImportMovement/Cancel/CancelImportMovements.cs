namespace EA.Iws.Requests.ImportMovement.Cancel
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportMovement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanEditImportMovements)]
    public class CancelImportMovements : IRequest<bool>
    {
        public CancelImportMovements(Guid notificationId, IEnumerable<ImportCancelMovementData> cancelledMovements)
        {
            CancelledMovements = cancelledMovements;
            NotificationId = notificationId;
        }

        public IEnumerable<ImportCancelMovementData> CancelledMovements { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}