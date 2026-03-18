namespace EA.Iws.Domain.ImportNotification
{
    using Core.ImportNotificationAssessment;
    using ImportNotificationAssessment;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IImportNotificationAssessmentRepository
    {
        Task<ImportNotificationAssessment> GetByNotification(Guid notificationId);

        Task<ImportNotificationAssessment> Get(Guid id);

        Task<ImportNotificationStatus> GetStatusByNotification(Guid notificationId);

        Task<ImportNotificationStatusChange> GetPreviousStatusChangeByNotification(Guid notificationId);

        Task<List<ImportNotificationStatusChangeData>> GetUnderProhibitionHistory(Guid notificationId);

        void Add(ImportNotificationAssessment assessment);

        Task<DateTime?> GetConsentedDate(Guid notificationId);

        Task<DateTime?> GetSubmitedDate(Guid notificationId);
    }
}
