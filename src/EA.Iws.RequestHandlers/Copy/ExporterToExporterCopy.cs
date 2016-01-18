namespace EA.Iws.RequestHandlers.Copy
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using DataAccess;
    using Domain.NotificationApplication.Exporter;
    using Prsd.Core.Domain;

    [AutoRegister]
    public class ExporterToExporterCopy
    {
        public async Task CopyAsync(IwsContext context, Guid sourceId, Guid destinationId)
        {
            var clonedExporter = await context.Set<Exporter>()
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.NotificationId == sourceId);

            if (clonedExporter != null)
            {
                typeof(Exporter).GetProperty("NotificationId")
                    .SetValue(clonedExporter, destinationId, null);
                typeof(Entity).GetProperty("Id")
                    .SetValue(clonedExporter, Guid.Empty, null);

                context.Exporters.Add(clonedExporter);
            }
        }
    }
}
