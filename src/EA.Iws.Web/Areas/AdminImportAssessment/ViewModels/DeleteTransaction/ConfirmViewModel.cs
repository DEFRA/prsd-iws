namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.DeleteTransaction
{
    using System;

    public class ConfirmViewModel
    {
        public Guid NotificationId { get; set; }

        public Guid TransactionId { get; set; }

        public ConfirmViewModel(Guid notificationId, Guid transactionId)
        {
            NotificationId = notificationId;
            TransactionId = transactionId;
        }
    }
}