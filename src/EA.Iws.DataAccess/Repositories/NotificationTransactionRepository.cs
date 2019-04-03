namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using Domain.Security;

    internal class NotificationTransactionRepository : INotificationTransactionRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization authorization;

        public NotificationTransactionRepository(IwsContext context, INotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public void Add(NotificationTransaction notificationTransaction)
        {
            context.NotificationTransactions.Add(notificationTransaction);
        }

        public async Task<IList<NotificationTransaction>> GetTransactions(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.NotificationTransactions.Where(n => n.NotificationId == notificationId).ToListAsync();
        }

        public async Task<NotificationTransaction> GetById(Guid transactionId)
        {
            return await context.NotificationTransactions.Where(t => t.Id == transactionId).SingleAsync();
        }

        public async Task DeleteById(Guid transactionId)
        {
            var transaction = await context.NotificationTransactions.Where(t => t.Id == transactionId).SingleAsync();

            context.DeleteOnCommit(transaction);
        }

        public async Task UpdateById(Guid id, string comment)
        {
            var existing = await context.NotificationTransactions.Where(t => t.Id == id).SingleAsync();

            existing.UpdateComments(comment);

            await context.SaveChangesAsync();
        }
    }
}
