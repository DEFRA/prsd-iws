namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;

    public interface INotificationAssessmentDatesSummaryRepository
    {
        Task<NotificationDatesSummary> GetById(Guid notificationId);
    }
}
