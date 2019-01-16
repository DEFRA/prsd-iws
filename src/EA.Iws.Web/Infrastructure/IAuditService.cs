namespace EA.Iws.Web.Infrastructure
{
    using Core.Notification.Audit;
    using Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;

    public interface IAuditService
    {
        Task AddAuditEntry(IMediator mediator, Guid notificationId, string userId, NotificationAuditType auditType, string screenName);
    }
}
