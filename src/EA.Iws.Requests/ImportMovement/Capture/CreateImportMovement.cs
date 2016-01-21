namespace EA.Iws.Requests.ImportMovement.Capture
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanCreateImportMovements)]
    public class CreateImportMovement : IRequest<Guid>
    {
        public Guid NotificationId { get; private set; }

        public int Number { get; private set; }

        public DateTime ActualShipmentDate { get; private set; }

        public DateTime? PrenotificationDate { get; private set; }

        public CreateImportMovement(Guid notificationId, int number, DateTime actualShipmentDate, DateTime? prenotificationDate)
        {
            NotificationId = notificationId;
            Number = number;
            ActualShipmentDate = actualShipmentDate;
            PrenotificationDate = prenotificationDate;
        }
    }
}
