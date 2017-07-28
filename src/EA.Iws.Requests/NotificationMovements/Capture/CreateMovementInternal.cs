namespace EA.Iws.Requests.NotificationMovements.Capture
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanCreateExportMovementsInternal)]
    public class CreateMovementInternal : IRequest<Guid>
    {
        public Guid NotificationId { get; private set; }

        public int Number { get; private set; }

        public DateTime? PrenotificationDate { get; private set; }

        public DateTime ActualShipmentDate { get; private set; }

        public bool HasNoPrenotification { get; set; }

        public CreateMovementInternal(Guid notificationId, int number, DateTime? prenotificationDate, DateTime actualShipmentDate,
            bool hasNoPrenotification)
        {
            NotificationId = notificationId;
            Number = number;
            PrenotificationDate = prenotificationDate;
            ActualShipmentDate = actualShipmentDate;
            HasNoPrenotification = hasNoPrenotification;
        }
    }
}