namespace EA.Iws.DataAccess.Repositories
{
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

        public async Task<string> ArchiveNotificationAsync(Guid notificationId)
        {
            //TODO Do we need this the EnsureAccess?
            //await authorization.EnsureAccessAsync(notificationId);
            try
            {
                return await context.Database.SqlQuery<string>
                    (@"[Notification].[uspArchiveNotification] @NotificationId, @CurrentUserId",
                    new SqlParameter("@NotificationId", notificationId),
                    new SqlParameter("@CurrentUserId", userContext.UserId)).SingleAsync();
            }
            catch (Exception ex)
            {
                //TODO Add some error handling
                Console.WriteLine(ex);
            }
            return null;
        }
    }
}