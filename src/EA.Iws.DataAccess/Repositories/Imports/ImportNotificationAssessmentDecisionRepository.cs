namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.ImportNotificationAssessment;
    using Domain.Security;

    public class ImportNotificationAssessmentDecisionRepository : IImportNotificationAssessmentDecisionRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ImportNotificationAssessmentDecisionRepository(ImportNotificationContext context,
            IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<IList<NotificationAssessmentDecision>> GetByImportNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            var assessment = await context.ImportNotificationAssessments.SingleOrDefaultAsync(n => n.NotificationApplicationId == notificationId);
            var withdrawals = await context.ImportWithdrawals.SingleOrDefaultAsync(n => n.NotificationId == notificationId);
            var objections = await context.ImportObjections.SingleOrDefaultAsync(n => n.NotificationId == notificationId);
            var consent = await context.ImportConsents.SingleOrDefaultAsync(n => n.NotificationId == notificationId);
            var result = new List<NotificationAssessmentDecision>();

            if (assessment.Dates.ConsentedDate != null)
            {
                result.Add(new NotificationAssessmentDecision(notificationId,
                    assessment.Dates.ConsentedDate.GetValueOrDefault(),
                    consent.ConsentRange.From,
                    consent.ConsentRange.To,
                    NotificationStatus.Consented));
            }

            if (withdrawals != null)
            {
                result.Add(new NotificationAssessmentDecision(notificationId,
                    withdrawals.Date,
                    null,
                    null,
                    NotificationStatus.Withdrawn));
            }

            if (objections != null)
            {
                result.Add(new NotificationAssessmentDecision(notificationId,
                    objections.Date,
                    null,
                    null,
                    NotificationStatus.Objected));
            }

            if (assessment.Dates.ConsentWithdrawnDate != null)
            {
                result.Add(new NotificationAssessmentDecision(notificationId,
                    assessment.Dates.ConsentWithdrawnDate.GetValueOrDefault(),
                    null,
                    null,
                    NotificationStatus.ConsentWithdrawn));
            }

            return result.OrderByDescending(r => r.Date).ToList();
        }
    }
}
