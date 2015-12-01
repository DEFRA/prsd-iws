namespace EA.Iws.RequestHandlers.Copy
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication.Importer;
    using Prsd.Core.Domain;

    public class ImporterToImporterCopy
    {
        public async Task CopyAsync(IwsContext context, Guid sourceId, Guid destinationId)
        {
            var clonedImporter = await context.Set<Importer>()
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.NotificationId == sourceId);

            if (clonedImporter != null)
            {
                typeof(Importer).GetProperty("NotificationId")
                    .SetValue(clonedImporter, destinationId, null);
                typeof(Entity).GetProperty("Id")
                    .SetValue(clonedImporter, Guid.Empty, null);

                context.Importers.Add(clonedImporter);
            }
        }
    }
}
