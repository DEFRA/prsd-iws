namespace EA.Iws.Web.Areas.AdminImportMovement.ViewModels.Home
{
    using System;
    using Core.ImportMovement;

    public class HomeViewModel
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        public Guid NotificationId { get; set; }

        public DateTime ActualShipmentDate { get; set; }

        public DateTime? PrenotificationDate { get; set; }

        public ReceiptViewModel Receipt { get; set; }

        public RecoveryViewModel Recovery { get; set; }

        public bool IsReceived { get; set; }

        public bool IsOperationCompleted { get; set; }

        public HomeViewModel()
        {
        }

        public HomeViewModel(ImportMovementSummaryData movementSummaryData)
        {
            Id = movementSummaryData.Data.NotificationId;
            Number = movementSummaryData.Data.Number;
            NotificationId = movementSummaryData.Data.NotificationId;
            ActualShipmentDate = movementSummaryData.Data.ActualDate.DateTime;
            Receipt = new ReceiptViewModel(movementSummaryData.ReceiptData);
            Recovery = new RecoveryViewModel(movementSummaryData.RecoveryData.OperationCompleteDate, movementSummaryData.Data.NotificationType);
            IsReceived = movementSummaryData.ReceiptData.IsReceived;
            IsOperationCompleted = movementSummaryData.RecoveryData.IsOperationCompleted;

            if (movementSummaryData.Data.PreNotificationDate.HasValue)
            {
                PrenotificationDate = movementSummaryData.Data.PreNotificationDate.Value.DateTime;
            }
        }
    }
}