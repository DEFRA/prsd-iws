namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Notification.Audit;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement;
    using Requests.Movement;
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

        public async Task AddMovementAudit(IMediator mediator, Guid notificationId, int shipmentNumber, string userId,
            MovementAuditType type)
        {
            var audit = new AuditMovement(notificationId, shipmentNumber, userId, type, SystemTime.Now);

            await mediator.SendAsync(audit);
        }

        public async Task AddImportMovementAudit(IMediator mediator, Guid notificationId, int shipmentNumber, string userId,
            MovementAuditType type)
        {
            var audit = new AuditImportMovement(notificationId, shipmentNumber, userId, type, SystemTime.Now);

            await mediator.SendAsync(audit);
        }

        private static CreateNotificationAudit CreateAudit(Guid notificationId, 
            string userId, 
            NotificationAuditScreenType screen, 
            NotificationAuditType type)
        {
            return new CreateNotificationAudit()
            {
                DateAdded = SystemTime.Now,
                NotificationId = notificationId,
                UserId = userId,
                Screen = screen,
                Type = type
            };
        }
    }
}