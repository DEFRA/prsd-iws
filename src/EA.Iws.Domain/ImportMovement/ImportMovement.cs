namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using Prsd.Core.Domain;

    public class ImportMovement : Entity
    {
        public Guid NotificationId { get; private set; }

        public int Number { get; private set; }

        public DateTime ActualShipmentDate { get; private set; }

        public DateTime? PrenotificationDate { get; private set; }

        internal ImportMovement(Guid notificationId, int number, DateTime actualDate)
        {
            ActualShipmentDate = actualDate;
            NotificationId = notificationId;
            Number = number;
        }

        private ImportMovement()
        {
        }

        public void SetActualShipmentDate(DateTime actualShipmentDate)
        {
            ActualShipmentDate = actualShipmentDate;
        }

        public void SetPrenotificationDate(DateTime prenotificationDate)
        {
            PrenotificationDate = prenotificationDate;
        }

        public ImportMovementReceipt Receive(ShipmentQuantity quantity, DateTime date)
        {
            return new ImportMovementReceipt(Id, quantity, date);
        }

        public ImportMovementRejection Reject(DateTime date, string reason, string furtherDetails)
        {
            return new ImportMovementRejection(Id, date, reason, furtherDetails);
        }

        public ImportMovementCompletedReceipt Complete(DateTime date)
        {
            return new ImportMovementCompletedReceipt(Id, date);
        }
    }
}
