namespace EA.Iws.DataAccess.Repositories.Imports
{
    using EA.Iws.Domain.ImportNotification;
    using EA.Iws.Domain.Security;
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;

    internal class WasteComponentRepository : IWasteComponentRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public WasteComponentRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<WasteComponent> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.WasteComponents.SingleAsync(o => o.ImportNotificationId == notificationId);
        }

        public void Add(WasteComponent wasteComponent)
        {
            context.WasteComponents.Add(wasteComponent);
        }
    }
}
