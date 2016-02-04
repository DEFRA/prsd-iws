namespace EA.Iws.Domain.NotificationAssessment
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public interface INotificationAttentionSummaryRepository
    {
        Task<IEnumerable<NotificationAttentionSummary>> GetByCompetentAuthority(CompetentAuthorityEnum competentAuthority);
    }
}