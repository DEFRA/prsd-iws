namespace EA.Iws.DataAccess.Security
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Security;
    using System.Threading.Tasks;
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
            var notificationUserId = await context.NotificationApplications.Where(n => n.Id == notificationId).Select(n => n.UserId).SingleAsync();

            if (!await IsInternal())
            {
                CheckUserId(notificationId, notificationUserId);
            }
        }

        public void EnsureAccess(Guid notificationId)
        {
            var notificationUserId = context.NotificationApplications.Where(n => n.Id == notificationId).Select(n => n.UserId).Single();
            CheckUserId(notificationId, notificationUserId);
        }

        private async Task<bool> IsInternal()
        {
            return await context.InternalUsers.AnyAsync(u => u.UserId == userContext.UserId.ToString());
        }

        private void CheckUserId(Guid notificationId, Guid notificationUserId)
        {
            if (notificationUserId != userContext.UserId)
            {
                throw new SecurityException(string.Format("Access denied to this notification {0} for user {1}",
                    notificationId, userContext.UserId));
            }
        }
    }
}