namespace EA.Iws.Requests.NotificationMovements.Capture
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanCreateExportMovementsInternal)]
    public class CreateMovementInternal : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public int Number { get; private set; }

        public DateTime? PrenotificationDate { get; private set; }

        public DateTime ActualShipmentDate { get; private set; }

        public CreateMovementInternal(Guid notificationId, int number, DateTime? prenotificationDate, DateTime actualShipmentDate)
        {
            NotificationId = notificationId;
            Number = number;
            PrenotificationDate = prenotificationDate;
            ActualShipmentDate = actualShipmentDate;
        }
    }
}