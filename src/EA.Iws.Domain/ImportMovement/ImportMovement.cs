namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ImportMovement : Entity
    {
        public Guid NotificationId { get; private set; }

        public int Number { get; private set; }

        public DateTime ActualShipmentDate { get; private set; }

        public DateTime? PrenotificationDate { get; private set; }

        public bool IsCancelled { get; private set; }

        public string Comments { get; private set; }

        public string StatsMarking { get; private set; }

        internal ImportMovement(Guid notificationId, int number, DateTime actualDate)
        {
            ActualShipmentDate = actualDate;
            NotificationId = notificationId;
            Number = number;
            IsCancelled = false;
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

        public ImportMovementRejection Reject(DateTime date, string reason)
        {
            return new ImportMovementRejection(Id, date, reason);
        }

        public ImportMovementCompletedReceipt Complete(DateTime date)
        {
            return new ImportMovementCompletedReceipt(Id, date);
        }

        internal void Cancel()
        {
            if (IsCancelled)
            {
                throw new InvalidOperationException(string.Format("Movement {0} is already cancelled", Id));
            }

            IsCancelled = true;
        }

        public void SetComments(string comments)
        {
            Guard.ArgumentNotNullOrEmpty(() => comments, comments);
            Comments = comments;
        }

        public void SetStatsMarking(string statsMarking)
        {
            Guard.ArgumentNotNullOrEmpty(() => statsMarking, statsMarking);
            StatsMarking = statsMarking;
        }
    }
}
