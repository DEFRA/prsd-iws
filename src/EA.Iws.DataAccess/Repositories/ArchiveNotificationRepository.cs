namespace EA.Iws.DataAccess.Repositories
{
    using EA.Iws.Core.Admin.ArchiveNotification;
    using EA.Iws.Domain;
    using EA.Iws.Domain.Security;
    using EA.Prsd.Core.Domain;
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    internal class ArchiveNotificationRepository : IArchiveNotificationRepository
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly INotificationApplicationAuthorization authorization;

        public ArchiveNotificationRepository(IwsContext context, IUserContext userContext, INotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.userContext = userContext;
            this.authorization = authorization;
        }

        public async Task<ArchiveNotificationResult> ArchiveNotificationAsync(Guid notificationId)
        {
            //TODO Do we need this the EnsureAccess?
            //await authorization.EnsureAccessAsync(notificationId);
            var result = await context.Database.SqlQuery<ArchiveNotificationResult>
                (@"[Notification].[uspArchiveNotification] @NotificationId, @CurrentUserId",
                new SqlParameter("@NotificationId", notificationId),
                new SqlParameter("@CurrentUserId", userContext.UserId)).SingleAsync();

            return result;
        }
    }
}