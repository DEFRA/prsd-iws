﻿namespace EA.Iws.Domain.ImportMovement
{
    using Core.Shared;

    public class ImportMovementSummary
    {
        public ImportMovement Movement { get; private set; }

        public ImportMovementReceipt Receipt { get; private set; }

        public ImportMovementRejection Rejection { get; private set; }

        public ImportMovementPartialRejection PartialRejection { get; private set; }

        public ImportMovementCompletedReceipt CompletedReceipt { get; private set; }

        public NotificationType NotificationType { get; private set; }

        public string NotificationNumber { get; private set; }

        public ShipmentQuantityUnits Units { get; private set; }

        public ImportMovementSummary(ImportMovement movement, 
            ImportMovementReceipt receipt, 
            ImportMovementRejection rejection, 
            ImportMovementCompletedReceipt completedReceipt, 
            NotificationType notificationType, 
            string notificationNumber,
            ShipmentQuantityUnits units,
            ImportMovementPartialRejection partialRejection)
        {
            Movement = movement;
            Receipt = receipt;
            Rejection = rejection;
            CompletedReceipt = completedReceipt;
            NotificationType = notificationType;
            NotificationNumber = notificationNumber;
            Units = units;
            PartialRejection = partialRejection;
        }
    }
}
