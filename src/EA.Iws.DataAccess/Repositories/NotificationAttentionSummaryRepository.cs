namespace EA.Iws.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;

    internal class NotificationAttentionSummaryRepository : INotificationAttentionSummaryRepository
    {
        private readonly IwsContext context;

        public NotificationAttentionSummaryRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<NotificationAttentionSummary>> GetByCompetentAuthority(UKCompetentAuthority competentAuthority)
        {
            var result = await context.NotificationAssessments
                .Where(x => 
                    x.Status == NotificationStatus.DecisionRequiredBy
                    || x.Status == NotificationStatus.Unlocked
                    || x.Status == NotificationStatus.Reassessment)
                .Join(context.NotificationApplications,
                    assessment => assessment.NotificationApplicationId,
                    notification => notification.Id,
                    (a, n) => new { Assessment = a, Notification = n })
                .Where(x => x.Notification.CompetentAuthority == competentAuthority)
                .Select(x => new
                {
                    x.Notification.Id,
                    x.Notification.NotificationNumber,
                    x.Assessment.Dates.NameOfOfficer,
                    x.Assessment.Dates.AcknowledgedDate
                }).ToArrayAsync();

            return result.Select(x =>
                    NotificationAttentionSummary.Load(x.Id,
                        x.NotificationNumber,
                        x.NameOfOfficer,
                        x.AcknowledgedDate.Value));
        }
    }
}