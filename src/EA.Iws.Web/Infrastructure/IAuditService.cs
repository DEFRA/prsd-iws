namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Notification.Audit;
    using Prsd.Core.Mediator;

    public interface IAuditService
    {
        Task AddAuditEntry(IMediator mediator, Guid notificationId, string userId, NotificationAuditType auditType,
            NotificationAuditScreenType screenType);

        Task AddMovementAudit(IMediator mediator, Guid notificationId, int shipmentNumber, string userId,
            MovementAuditType type);

        Task AddImportMovementAudit(IMediator mediator, Guid notificationId, int shipmentNumber, string userId,
            MovementAuditType type);
    }
}
