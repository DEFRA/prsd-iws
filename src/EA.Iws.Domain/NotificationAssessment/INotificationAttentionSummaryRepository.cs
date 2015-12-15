namespace EA.Iws.Domain.NotificationAssessment
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INotificationAttentionSummaryRepository
    {
        Task<IEnumerable<NotificationAttentionSummary>> GetByCompetentAuthority(UKCompetentAuthority competentAuthority);
    }
}