namespace EA.Iws.Web.Infrastructure
{
    using Core.Notification.Audit;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class AuditService : IAuditService
    {
        public async Task AddAuditEntry(IMediator mediator, Guid notificationId, string userId, bool existingEntry, string screenName)
        {
            var screens = await mediator.SendAsync(new GetNotificationAuditScreens());

            var audit = CreateAudit(notificationId,
                userId,
                screens.FirstOrDefault(p => p.ScreenName == screenName).Id,
                existingEntry ? NotificationAuditType.Update : NotificationAuditType.Create);

            await mediator.SendAsync(audit);
        }

        private CreateNotificationAudit CreateAudit(Guid notificationId, string userId, int screen, NotificationAuditType type)
        {
            return new CreateNotificationAudit()
            {
                DateAdded = DateTime.Now,
                NotificationId = notificationId,
                UserId = userId,
                Screen = screen,
                Type = type
            };
        }
    }
}