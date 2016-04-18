namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotification;

    internal class InterimStatusRepository : IInterimStatusRepository
    {
        private readonly ImportNotificationContext context;

        public InterimStatusRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<InterimStatus> GetByNotificationId(Guid notificationId)
        {
            return await context.InterimStatuses.SingleAsync(i => i.ImportNotificationId == notificationId);
        }

        public async Task<InterimStatus> GetByNotificationIdOrDefault(Guid notificationId)
        {
            return await context.InterimStatuses.SingleOrDefaultAsync(i => i.ImportNotificationId == notificationId);
        }

        public void Add(InterimStatus interimStatus)
        {
            context.InterimStatuses.Add(interimStatus);
        }
    }
}
