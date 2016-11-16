namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;

    public interface IImportNotificationAssessmentDecisionRepository
    {
        Task<IList<NotificationAssessmentDecision>> GetByImportNotificationId(Guid notificationId);
    }
}
