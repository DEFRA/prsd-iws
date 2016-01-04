namespace EA.Iws.RequestHandlers.Copy
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication.Annexes;
    using Prsd.Core.Domain;

    public class AnnexCollectionToAnnexCollectionCopy
    {
        public async Task CopyAsync(IwsContext context, Guid sourceId, Guid destinationId)
        {
            var clonedAnnexCollection = await context.Set<AnnexCollection>()
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.NotificationId == sourceId);

            if (clonedAnnexCollection != null)
            {
                typeof(AnnexCollection).GetProperty("NotificationId")
                    .SetValue(clonedAnnexCollection, destinationId, null);
                typeof(Entity).GetProperty("Id")
                    .SetValue(clonedAnnexCollection, Guid.Empty, null);

                context.AnnexCollections.Add(clonedAnnexCollection);
            }
        }
    }
}
