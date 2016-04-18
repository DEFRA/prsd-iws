namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;

    public interface INotificationAssessmentDecisionRepository
    {
        Task<IList<NotificationAssessmentDecision>> GetByNotificationId(Guid notificationId);
    }
}
