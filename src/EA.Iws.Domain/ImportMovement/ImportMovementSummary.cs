namespace EA.Iws.Domain.ImportMovement
{
    using Core.Shared;

    public class ImportMovementSummary
    {
        public ImportMovement Movement { get; private set; }

        public ImportMovementReceipt Receipt { get; private set; }

        public ImportMovementRejection Rejection { get; private set; }

        public ImportMovementCompletedReceipt CompletedReceipt { get; private set; }

        public NotificationType NotificationType { get; private set; }

        public string NotificationNumber { get; private set; }

        public ImportMovementSummary(ImportMovement movement, 
            ImportMovementReceipt receipt, 
            ImportMovementRejection rejection, 
            ImportMovementCompletedReceipt completedReceipt, 
            NotificationType notificationType, 
            string notificationNumber)
        {
            Movement = movement;
            Receipt = receipt;
            Rejection = rejection;
            CompletedReceipt = completedReceipt;
            NotificationType = notificationType;
            NotificationNumber = notificationNumber;
        }
    }
}
