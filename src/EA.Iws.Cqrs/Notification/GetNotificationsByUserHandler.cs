namespace EA.Iws.Cqrs.Notification
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Cqrs;
    using DataAccess;
    using Domain.Notification;

    internal class GetNotificationsByUserHandler : IQueryHandler<GetNotificationsByUser, IList<NotificationApplication>>
    {
        private readonly IwsContext context;

        public GetNotificationsByUserHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IList<NotificationApplication>> ExecuteAsync(GetNotificationsByUser query)
        {
            var notificationApplications = await context.NotificationApplications.Where(na => na.UserId == query.UserId).ToArrayAsync();

            return notificationApplications;
        }
    }
}
