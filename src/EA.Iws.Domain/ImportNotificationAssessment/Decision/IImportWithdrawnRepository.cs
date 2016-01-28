namespace EA.Iws.Domain.ImportNotificationAssessment.Decision
{
    using System;
    using System.Threading.Tasks;

    public interface IImportWithdrawnRepository
    {
        Task<ImportWithdrawn> GetByNotificationIdOrDefault(Guid notificationId);

        Task<ImportWithdrawn> GetByNotificationId(Guid notificationId);

        void Add(ImportWithdrawn importWithdrawn);
    }
}
