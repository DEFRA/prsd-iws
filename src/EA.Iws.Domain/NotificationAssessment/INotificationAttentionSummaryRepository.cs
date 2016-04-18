namespace EA.Iws.Domain.NotificationAssessment
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Notification;

    public interface INotificationAttentionSummaryRepository
    {
        Task<IEnumerable<NotificationAttentionSummary>> GetByCompetentAuthority(UKCompetentAuthority competentAuthority);
    }
}