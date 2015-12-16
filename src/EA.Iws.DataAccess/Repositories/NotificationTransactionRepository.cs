namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;

    internal class NotificationTransactionRepository : INotificationTransactionRepository
    {
        private readonly IwsContext context;

        public NotificationTransactionRepository(IwsContext context)
        {
            this.context = context;
        }

        public void Add(NotificationTransactionData notificationTransactionData)
        {
            context.NotificationTransactions.Add(new NotificationTransaction(notificationTransactionData));
        }

        public async Task<IList<NotificationTransaction>> GetTransactions(Guid notificationId)
        {
            return await context.NotificationTransactions.Where(n => n.NotificationId == notificationId).ToListAsync();
        }
    }
}
