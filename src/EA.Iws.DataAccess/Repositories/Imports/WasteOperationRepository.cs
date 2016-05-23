namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Domain.Security;

    internal class WasteOperationRepository : IWasteOperationRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public WasteOperationRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<WasteOperation> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.OperationCodes.SingleAsync(o => o.ImportNotificationId == notificationId);
        }

        public void Add(WasteOperation wasteOperation)
        {
            context.OperationCodes.Add(wasteOperation);
        }
    }
}