namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using Domain.Security;

    internal class ImportNotificationAssessmentRepository : IImportNotificationAssessmentRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ImportNotificationAssessmentRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<ImportNotificationAssessment> GetByNotification(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.ImportNotificationAssessments.SingleAsync(na => na.NotificationApplicationId == notificationId);
        }

        public async Task<ImportNotificationAssessment> Get(Guid id)
        {
            var assessment = await context.ImportNotificationAssessments.SingleAsync(na => na.Id == id);
            await authorization.EnsureAccessAsync(assessment.NotificationApplicationId);
            return assessment;
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