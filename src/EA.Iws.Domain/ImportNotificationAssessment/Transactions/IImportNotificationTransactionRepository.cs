namespace EA.Iws.Domain.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IImportNotificationTransactionRepository
    {
        Task<IEnumerable<ImportNotificationTransaction>> GetTransactions(Guid importNotificationId);

        void Add(ImportNotificationTransaction transaction);

        Task DeleteById(Guid transactionId);
    }
}
