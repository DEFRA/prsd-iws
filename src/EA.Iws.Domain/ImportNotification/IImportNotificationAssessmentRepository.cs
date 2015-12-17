namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;
    using ImportNotificationAssessment;

    public interface IImportNotificationAssessmentRepository
    {
        Task<ImportNotificationAssessment> GetByNotification(Guid notificationId);

        Task<ImportNotificationAssessment> Get(Guid id);

        void Add(ImportNotificationAssessment assessment);
    }
}
