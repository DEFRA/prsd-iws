namespace EA.Iws.Requests.NotificationMovements.Create
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;
    using System;
    using System.Collections.Generic;

    [RequestAuthorization(ExportMovementPermissions.CanCreateExportMovementsExternal)]
    public class CreateMovementCarriers : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public IEnumerable<Guid> MovementId { get; private set; }

        public Dictionary<int, Guid> SelectedCarriers { get; private set; }

        public CreateMovementCarriers(Guid notificationId, IEnumerable<Guid> movementId, Dictionary<int, Guid> selectedCarriers)
        {
            NotificationId = notificationId;
            MovementId = movementId;
            SelectedCarriers = selectedCarriers;
        }
    }
}
