namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotification;

    internal class ProducerRepository : IProducerRepository
    {
        private readonly ImportNotificationContext context;

        public ProducerRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<Producer> GetByNotificationId(Guid notificationId)
        {
            return await context.Producers.SingleAsync(p => p.ImportNotificationId == notificationId);
        }

        public void Add(Producer producer)
        {
            context.Producers.Add(producer);
        }
    }
}