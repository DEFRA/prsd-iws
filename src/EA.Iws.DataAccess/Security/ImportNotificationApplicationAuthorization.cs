namespace EA.Iws.DataAccess.Security
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Security;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Domain.Security;
    using Prsd.Core.Domain;

    internal class ImportNotificationApplicationAuthorization : IImportNotificationApplicationAuthorization
    {
        private readonly IwsContext context;
        private readonly ImportNotificationContext importNotificationContext;
        private readonly IUserContext userContext;

        public ImportNotificationApplicationAuthorization(IwsContext context,
            ImportNotificationContext importNotificationContext, IUserContext userContext)
        {
            this.context = context;
            this.importNotificationContext = importNotificationContext;
            this.userContext = userContext;
        }

        public async Task EnsureAccessAsync(Guid notificationId)
        {
            var notification = await importNotificationContext.ImportNotifications
                .Where(n => n.Id == notificationId).SingleAsync();

            if (await IsInternal())
            {
                await CheckCompetentAuthority(notification);
            }
            else
            {
                throw new SecurityException(string.Format("Access denied to this notification {0} for user {1}",
                    notificationId, userContext.UserId));
            }
        }

        private async Task CheckCompetentAuthority(ImportNotification notification)
        {
            var userCompetentAuthority = await context.GetUsersCompetentAuthority(userContext);

            if (notification.CompetentAuthority != userCompetentAuthority)
            {
                throw new SecurityException(string.Format("Access denied to this notification {0} for user {1} for competent authority {2}",
                    notification.Id, userContext.UserId, userCompetentAuthority));
            }
        }

        private async Task<bool> IsInternal()
        {
            return await context.InternalUsers.AnyAsync(u => u.UserId == userContext.UserId.ToString());
        }
    }
}