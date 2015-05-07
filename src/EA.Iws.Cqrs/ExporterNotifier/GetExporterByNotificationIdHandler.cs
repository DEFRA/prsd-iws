namespace EA.Iws.Cqrs.ExporterNotifier
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Cqrs;
    using DataAccess;
    using Domain;

    public class GetExporterByNotificationIdHandler : IQueryHandler<GetExporterByNotificationId, Exporter>
    {
        private readonly IwsContext context;

        public GetExporterByNotificationIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Exporter> ExecuteAsync(GetExporterByNotificationId query)
        {
            try
            {
                var notification = await context.NotificationApplications
                    .Include(n => n.Exporter.Address.Country)
                    .Include(n => n.Exporter.Contact)
                    .SingleAsync(n => n.Id == query.NotificationId);
                return notification.Exporter;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
