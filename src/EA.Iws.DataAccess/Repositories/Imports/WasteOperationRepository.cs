namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotification;

    internal class WasteOperationRepository : IWasteOperationRepository
    {
        private readonly ImportNotificationContext context;

        public WasteOperationRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<WasteOperation> GetByNotificationId(Guid notificationId)
        {
            return await context.OperationCodes.SingleAsync(o => o.ImportNotificationId == notificationId);
        }

        public void Add(WasteOperation wasteOperation)
        {
            context.OperationCodes.Add(wasteOperation);
        }
    }
}