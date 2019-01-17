namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using Core.Notification.Audit;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    public class AuditService : IAuditService
    {
        public async Task AddAuditEntry(IMediator mediator, 
            Guid notificationId, 
            string userId, 
            NotificationAuditType auditType, 
            NotificationAuditScreenType screenType)
        {
            var audit = CreateAudit(notificationId,
                userId,
                screenType,
                auditType);

            await mediator.SendAsync(audit);
        }

        private static CreateNotificationAudit CreateAudit(Guid notificationId, 
            string userId, 
            NotificationAuditScreenType screen, 
            NotificationAuditType type)
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