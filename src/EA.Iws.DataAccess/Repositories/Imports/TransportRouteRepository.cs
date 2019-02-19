namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Domain.Security;

    internal class TransportRouteRepository : ITransportRouteRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public TransportRouteRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<TransportRoute> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.TransportRoutes.SingleAsync(t => t.ImportNotificationId == notificationId);
        }

        public void Add(TransportRoute transportRoute)
        {
            context.TransportRoutes.Add(transportRoute);
        }

        public async Task DeleteEntryCustomsOfficeByNotificationId(Guid notificationId)
        {
            await context.Database.ExecuteSqlCommandAsync(@"
                DELETE FROM [Notification].[EntryCustomsOffice] WHERE TransportRouteId IN (SELECT [Id] FROM [Notification].[TransportRoute] WHERE NotificationId = @Id)",
                new SqlParameter("@Id", notificationId));
        }
    }
}