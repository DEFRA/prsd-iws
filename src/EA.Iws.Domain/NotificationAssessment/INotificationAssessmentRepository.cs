namespace EA.Iws.Domain.NotificationAssessment
{
    using Core.NotificationAssessment;
    using System;
    using System.Threading.Tasks;

    public interface INotificationAssessmentRepository
    {
        Task<NotificationAssessment> GetByNotificationId(Guid notificationId);

        Task<string> GetNumberForAssessment(Guid notificationAssessmentId);

        Task<NotificationStatus> GetStatusByNotificationId(Guid notificationId);

        Task<NotificationStatusChange> GetPreviousStatusChangeByNotification(Guid notificationId);

        Task<NotificationStatus> GetPreviousStatusByNotification(Guid notificationId);
    }
}