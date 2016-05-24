namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Domain.Security;

    internal class WasteTypeRepository : IWasteTypeRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public WasteTypeRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public void Add(WasteType wasteType)
        {
            context.WasteTypes.Add(wasteType);
        }

        public async Task<WasteType> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.WasteTypes.SingleAsync(wt => wt.ImportNotificationId == notificationId);
        }
    }
}