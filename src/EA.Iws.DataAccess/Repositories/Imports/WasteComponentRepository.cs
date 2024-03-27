namespace EA.Iws.DataAccess.Repositories.Imports
{
    using EA.Iws.Domain.ImportNotification;
    using EA.Iws.Domain.Security;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
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

        public async Task<IEnumerable<WasteComponent>> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.WasteComponents.Where(o => o.ImportNotificationId == notificationId).ToArrayAsync();
        }

        public void Add(List<WasteComponent> wasteComponent)
        {
            context.WasteComponents.AddRange(wasteComponent);
        }
    }
}