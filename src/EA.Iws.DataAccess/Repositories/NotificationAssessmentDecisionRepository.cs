namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;

    public class NotificationAssessmentDecisionRepository : INotificationAssessmentDecisionRepository
    {
        private readonly IwsContext context;

        public NotificationAssessmentDecisionRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IList<NotificationAssessmentDecision>> GetByNotificationId(Guid notificationId)
        {
            var assessment = await context.NotificationAssessments.SingleOrDefaultAsync(n => n.NotificationApplicationId == notificationId);
            var consent = await context.Consents.SingleOrDefaultAsync(n => n.NotificationApplicationId == notificationId);
            var result = new List<NotificationAssessmentDecision>();

            if (assessment.Dates.ConsentedDate != null)
            {
                result.Add(new NotificationAssessmentDecision(notificationId,
                    assessment.Dates.ConsentedDate.GetValueOrDefault(),
                    consent.ConsentRange.From, 
                    consent.ConsentRange.To,
                    NotificationStatus.Consented));
            }

            if (assessment.Dates.WithdrawnDate != null)
            {
                result.Add(new NotificationAssessmentDecision(notificationId,
                    assessment.Dates.WithdrawnDate.GetValueOrDefault(),
                    null,
                    null,
                    NotificationStatus.Withdrawn));
            }

            if (assessment.Dates.ObjectedDate != null)
            {
                result.Add(new NotificationAssessmentDecision(notificationId,
                    assessment.Dates.ObjectedDate.GetValueOrDefault(),
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
