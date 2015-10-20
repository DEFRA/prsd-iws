namespace EA.Iws.RequestHandlers.Copy
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication.WasteRecovery;
    using Prsd.Core.Domain;

    public class WasteRecoveryToWasteRecoveryCopy
    {
        public async Task CopyAsync(IwsContext context, Guid sourceId, Guid destinationId)
        {
            var clonedWasteRecovery = await context.Set<WasteRecovery>()
                .AsNoTracking()
                .SingleOrDefaultAsync(n => n.NotificationId == sourceId);

            if (clonedWasteRecovery != null)
            {
                typeof(WasteRecovery).GetProperty("NotificationId")
                .SetValue(clonedWasteRecovery, destinationId, null);
                typeof(Entity).GetProperty("Id").SetValue(clonedWasteRecovery, Guid.Empty, null);

                context.WasteRecoveries.Add(clonedWasteRecovery);
            }

            var clonedWasteDisposal = await context.Set<WasteDisposal>()
                .AsNoTracking()
                .SingleOrDefaultAsync(n => n.NotificationId == sourceId);

            if (clonedWasteDisposal != null)
            {
                typeof(WasteDisposal).GetProperty("NotificationId")
                .SetValue(clonedWasteDisposal, destinationId, null);
                typeof(Entity).GetProperty("Id").SetValue(clonedWasteDisposal, Guid.Empty, null);

                context.WasteDisposals.Add(clonedWasteDisposal);
            }
        }
    }
}
