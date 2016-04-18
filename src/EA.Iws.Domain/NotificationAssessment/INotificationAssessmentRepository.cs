namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;

    public interface INotificationAssessmentRepository
    {
        Task<NotificationAssessment> GetByNotificationId(Guid notificationId);

        Task<string> GetNumberForAssessment(Guid notificationAssessmentId);

        Task<NotificationStatus> GetStatusByNotificationId(Guid notificationId);
    }
}