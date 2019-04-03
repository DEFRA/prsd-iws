namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INotificationTransactionRepository
    {
        void Add(NotificationTransaction notificationTransaction);

        Task<IList<NotificationTransaction>> GetTransactions(Guid notificationId);

        Task<NotificationTransaction> GetById(Guid transactionId);

        Task DeleteById(Guid transactionId);

        Task UpdateById(Guid id, string comment);
    }
}
