namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGovUkPaySessionRepository
    {
        void Add(GovUkPaySession session);

        Task<GovUkPaySession> GetBySecureToken(string secureToken);

        Task<GovUkPaySession> GetInProgressByNotificationId(Guid notificationId);

        Task<IList<GovUkPaySession>> GetFailedByNotificationId(Guid notificationId);
    }
}