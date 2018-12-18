namespace EA.Iws.DataAccess.Security
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Security;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Domain.Security;
    using Prsd.Core.Domain;

    internal class NotificationApplicationAuthorization : INotificationApplicationAuthorization
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public NotificationApplicationAuthorization(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task EnsureAccessAsync(Guid notificationId)
        {
            var notification = await context.NotificationApplications.Where(n => n.Id == notificationId).SingleAsync();

            if (await IsInternal())
            {
                await CheckCompetentAuthority(notification);
            }
            else
            {
                await CheckUserId(notificationId, notification.UserId);
            }
        }

        private async Task CheckCompetentAuthority(NotificationApplication notification)
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

        private async Task CheckUserId(Guid notificationId, Guid notificationUserId)
        {
            if (notificationUserId != userContext.UserId &&
                !await IsSharedUser(notificationId))
            {
                throw new SecurityException(string.Format("Access denied to this notification {0} for user {1}",
                    notificationId, userContext.UserId));
            }
        }

        private async Task<bool> IsSharedUser(Guid notificationId)
        {
            return
                await
                    context.SharedUser.AnyAsync(
                        u => u.NotificationId == notificationId && u.UserId == userContext.UserId.ToString());
        }
    }
}