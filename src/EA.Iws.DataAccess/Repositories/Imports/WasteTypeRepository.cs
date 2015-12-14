namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotification;

    internal class WasteTypeRepository : IWasteTypeRepository
    {
        private readonly ImportNotificationContext context;

        public WasteTypeRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public void Add(WasteType wasteType)
        {
            context.WasteTypes.Add(wasteType);
        }

        public async Task<WasteType> GetByNotificationId(Guid notificationId)
        {
            return await context.WasteTypes.SingleAsync(wt => wt.ImportNotificationId == notificationId);
        }
    }
}