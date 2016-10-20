namespace EA.Iws.Requests.ImportMovement.Cancel
{
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportMovement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanEditImportMovements)]
    public class CancelImportMovements : IRequest<bool>
    {
        public CancelImportMovements(IEnumerable<ImportCancelMovementData> cancelledMovements)
        {
            CancelledMovements = cancelledMovements;
        }

        public IEnumerable<ImportCancelMovementData> CancelledMovements { get; private set; }
    }
}