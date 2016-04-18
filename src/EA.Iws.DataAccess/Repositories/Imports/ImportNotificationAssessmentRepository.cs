namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;

    internal class ImportNotificationAssessmentRepository : IImportNotificationAssessmentRepository
    {
        private readonly ImportNotificationContext context;

        public ImportNotificationAssessmentRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<ImportNotificationAssessment> GetByNotification(Guid notificationId)
        {
            return await context.ImportNotificationAssessments.SingleAsync(na => na.NotificationApplicationId == notificationId);
        }

        public async Task<ImportNotificationAssessment> Get(Guid id)
        {
            return await context.ImportNotificationAssessments.SingleAsync(na => na.Id == id);
        }

        public void Add(ImportNotificationAssessment assessment)
        {
            context.ImportNotificationAssessments.Add(assessment);
        }

        public async Task<ImportNotificationStatus> GetStatusByNotification(Guid notificationId)
        {
            return (await GetByNotification(notificationId)).Status;
        }
    }
}