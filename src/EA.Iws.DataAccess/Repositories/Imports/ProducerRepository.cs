namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Domain.Security;

    internal class ProducerRepository : IProducerRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ProducerRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<Producer> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.Producers.SingleAsync(p => p.ImportNotificationId == notificationId);
        }

        public void Add(Producer producer)
        {
            context.Producers.Add(producer);
        }
    }
}