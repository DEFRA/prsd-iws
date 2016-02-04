namespace EA.Iws.RequestHandlers.Copy
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Domain;

    [AutoRegister]
    public class MeansOfTransportToMeansOfTransportCopy
    {
        public async Task CopyAsync(IwsContext context, Guid sourceId, Guid destinationId)
        {
            var clonedMeans = await context.Set<MeansOfTransport>()
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.NotificationId == sourceId);

            if (clonedMeans != null)
            {
                typeof(MeansOfTransport).GetProperty("NotificationId")
                    .SetValue(clonedMeans, destinationId, null);
                typeof(Entity).GetProperty("Id")
                    .SetValue(clonedMeans, Guid.Empty, null);

                context.MeansOfTransports.Add(clonedMeans);
            }
        }
    }
}