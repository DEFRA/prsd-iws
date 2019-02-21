namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.Admin.KeyDates;

    public interface IKeyDatesOverrideRepository
    {
        Task<KeyDatesOverrideData> GetKeyDatesForNotification(Guid notificationId);

        Task SetKeyDatesForNotification(KeyDatesOverrideData data);

        Task SetDecisionRequiredByDateForNotification(Guid notificationAssessmentId, DateTime? decisionRequiredByDate);
    }
}