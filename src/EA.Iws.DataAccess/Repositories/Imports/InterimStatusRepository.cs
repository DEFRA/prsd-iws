namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Domain.Security;

    internal class InterimStatusRepository : IInterimStatusRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public InterimStatusRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<InterimStatus> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.InterimStatuses.SingleAsync(i => i.ImportNotificationId == notificationId);
        }

        public async Task<InterimStatus> GetByNotificationIdOrDefault(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.InterimStatuses.SingleOrDefaultAsync(i => i.ImportNotificationId == notificationId);
        }

        public void Add(InterimStatus interimStatus)
        {
            context.InterimStatuses.Add(interimStatus);
        }

        public async Task UpdateStatus(Guid notificationId, bool isInterim)
        {
            var currentInterimStatus = await GetByNotificationId(notificationId);

            currentInterimStatus.UpdateStatus(isInterim);
        }
    }
}
