namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationInfoInternalHandler : IRequestHandler<GetNotificationInfoInternal, NotificationInfo>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, NotificationInfo> notificationInfoMap;
        private readonly IUserContext userContext;

        public GetNotificationInfoInternalHandler(IwsContext context,
            IMap<NotificationApplication, NotificationInfo> notificationInfoMap,
            IUserContext userContext)
        {
            this.context = context;
            this.notificationInfoMap = notificationInfoMap;
            this.userContext = userContext;
        }

        public async Task<NotificationInfo> HandleAsync(GetNotificationInfoInternal message)
        {
            if (!await context.IsInternalUserAsync(userContext))
            {
                throw new InvalidOperationException(
                    string.Format("Cannot access the notification {0} because the requesting user {1} is not an internal user.",
                    message.NotificationId,
                    userContext.UserId));
            }

            var notification = await context.NotificationApplications
                .SingleAsync(na => na.Id == message.NotificationId);

            return notificationInfoMap.Map(notification);
        }
    }
}