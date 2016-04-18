namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;

    public interface INotificationTransactionRepository
    {
        void Add(NotificationTransactionData notificationTransactionData);

        Task<IList<NotificationTransaction>> GetTransactions(Guid notificationId);
    }
}
