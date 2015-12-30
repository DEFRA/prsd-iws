namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment.Transactions;

    internal class ImportNotificationTransactionRepository : IImportNotificationTransactionRepository
    {
        private readonly ImportNotificationContext context;

        public ImportNotificationTransactionRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ImportNotificationTransaction>> GetTransactions(Guid importNotificationId)
        {
            return
                await
                    context.ImportNotificationTransactions.Where(t => t.NotificationId == importNotificationId)
                        .ToListAsync();
        }

        public void Add(ImportNotificationTransaction transaction)
        {
            context.ImportNotificationTransactions.Add(transaction);
        }
    }
}
