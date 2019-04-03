namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using Core.Notification.Audit;
    using Prsd.Core.Mediator;

    public interface IAuditService
    {
        Task AddAuditEntry(IMediator mediator, Guid notificationId, string userId, NotificationAuditType auditType, NotificationAuditScreenType screenType);
    }
}
