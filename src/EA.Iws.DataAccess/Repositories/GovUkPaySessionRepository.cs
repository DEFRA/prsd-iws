namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;

    internal class GovUkPaySessionRepository : IGovUkPaySessionRepository
    {
        private readonly IwsContext context;

        public GovUkPaySessionRepository(IwsContext context)
        {
            this.context = context;
        }

        public void Add(GovUkPaySession session)
        {
            context.GovUkPaySessions.Add(session);
        }

        public async Task<GovUkPaySession> GetBySecureToken(string secureToken)
        {
            return await context.GovUkPaySessions
                .Where(s => s.SecureToken == secureToken)
                .SingleOrDefaultAsync();
        }

        public async Task<GovUkPaySession> GetInProgressByNotificationId(Guid notificationId)
        {
            return await context.GovUkPaySessions
                .Where(s => s.NotificationId == notificationId && !s.InFinalState)
                .OrderByDescending(s => s.CreatedDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<GovUkPaySession>> GetFailedByNotificationId(Guid notificationId)
        {
            return await context.GovUkPaySessions
                .Where(s => s.NotificationId == notificationId
                    && s.InFinalState
                    && s.Status != "success")
                .OrderBy(s => s.CreatedDate)
                .ToListAsync();
        }
    }
}