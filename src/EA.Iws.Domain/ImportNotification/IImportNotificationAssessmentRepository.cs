namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using ImportNotificationAssessment;

    public interface IImportNotificationAssessmentRepository
    {
        Task<ImportNotificationAssessment> GetByNotification(Guid notificationId);

        Task<ImportNotificationAssessment> Get(Guid id);

        Task<ImportNotificationStatus> GetStatusByNotification(Guid notificationId);

        void Add(ImportNotificationAssessment assessment);
    }
}
