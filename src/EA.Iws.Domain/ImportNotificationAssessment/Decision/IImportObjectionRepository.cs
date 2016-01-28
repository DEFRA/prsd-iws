namespace EA.Iws.Domain.ImportNotificationAssessment.Decision
{
    using System;
    using System.Threading.Tasks;

    public interface IImportObjectionRepository
    {
        Task<ImportObjection> GetByNotificationId(Guid notificationId);
 
        Task<ImportObjection> GetByNotificationIdOrDefault(Guid notificationId); 

        void Add(ImportObjection importObjection);
    }
}
