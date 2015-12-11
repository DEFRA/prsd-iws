namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using Prsd.Core.Domain;

    public class ImportMovement : Entity
    {
        public Guid NotificationId { get; private set; }

        public int Number { get; private set; }

        public DateTimeOffset ActualShipmentDate { get; private set; }

        public DateTimeOffset? PrenotificationDate { get; private set; }

        internal ImportMovement(Guid notificationId, int number, DateTimeOffset actualDate)
        {
            ActualShipmentDate = actualDate;
            NotificationId = notificationId;
            Number = number;
        }

        private ImportMovement()
        {
        }

        public void SetActualShipmentDate(DateTimeOffset actualShipmentDate)
        {
            ActualShipmentDate = actualShipmentDate;
        }

        public void SetPrenotificationDate(DateTimeOffset prenotificationDate)
        {
            PrenotificationDate = prenotificationDate;
        }

        public ImportMovementReceipt Receive(ShipmentQuantity quantity, DateTimeOffset date)
        {
            return new ImportMovementReceipt(Id, quantity, date);
        }

        public ImportMovementRejection Reject(DateTimeOffset date, string reason, string furtherDetails)
        {
            return new ImportMovementRejection(Id, date, reason, furtherDetails);
        }

        public ImportMovementCompletedReceipt Complete(DateTimeOffset date)
        {
            return new ImportMovementCompletedReceipt(Id, date);
        }
    }
}
