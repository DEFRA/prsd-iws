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

        public HomeViewModel(ImportMovement movement)
        {
            Id = movement.Data.NotificationId;
            Number = movement.Data.Number;
            NotificationId = movement.Data.NotificationId;
            ActualShipmentDate = movement.Data.ActualDate.DateTime;
            Receipt = new ReceiptViewModel(movement.Receipt);
            Recovery = new RecoveryViewModel(movement.Recovery.OperationCompleteDate, movement.Data.NotificationType);

            if (movement.Data.PreNotificationDate.HasValue)
            {
                PrenotificationDate = movement.Data.PreNotificationDate.Value.DateTime;
            }
        }
    }
}