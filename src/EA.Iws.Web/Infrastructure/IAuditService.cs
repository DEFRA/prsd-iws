namespace EA.Iws.Web.Infrastructure
{
    using Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;

    public interface IAuditService
    {
        Task AddAuditEntry(IMediator mediator, Guid notificationId, string userId, bool existingEntry, string screenName);
    }
}
