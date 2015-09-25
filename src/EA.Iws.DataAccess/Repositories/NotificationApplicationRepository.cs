namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Security;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Prsd.Core.Domain;

    internal class NotificationApplicationRepository : INotificationApplicationRepository
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public NotificationApplicationRepository(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<NotificationApplication> GetById(Guid id)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == id);
            if (notification.UserId != userContext.UserId)
            {
                throw new SecurityException(string.Format("Access denied to this notification {0} for user {1}",
                    id, userContext.UserId));
            }
            return notification;
        }
    }
}