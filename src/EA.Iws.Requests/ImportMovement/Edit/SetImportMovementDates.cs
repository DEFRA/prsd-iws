namespace EA.Iws.Requests.ImportMovement.Edit
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanEditImportMovements)]
    public class SetImportMovementDates : IRequest<bool>
    {
        public Guid MovementId { get; private set; }

        public DateTime ActualShipmentDate { get; private set; }

        public DateTime? PrenotificationDate { get; private set; }

        public SetImportMovementDates(Guid movementId, DateTime actualShipmentDate, DateTime? prenotificationDate)
        {
            MovementId = movementId;
            ActualShipmentDate = actualShipmentDate;
            PrenotificationDate = prenotificationDate;
        }
    }
}
