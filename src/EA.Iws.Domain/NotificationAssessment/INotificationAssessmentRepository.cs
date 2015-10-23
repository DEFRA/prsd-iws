namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;

    public interface INotificationAssessmentRepository
    {
        Task<NotificationAssessment> GetByNotificationId(Guid notificationId);

        Task<string> GetNumberForAssessment(Guid notificationAssessmentId);
    }
}