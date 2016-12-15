namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.DeleteTransaction
{
    using System;
    using System.Collections.Generic;
    using Core.Shared;

    public class DeleteTransactionViewModel
    {
        public Guid NotificationId { get; set; }
        public IList<TransactionRecordData> Transactions { get; set; }

        public DeleteTransactionViewModel(Guid notificationId, IList<TransactionRecordData> transactions)
        {
            NotificationId = notificationId;
            Transactions = transactions;
        }
    }
}