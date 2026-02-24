namespace EA.Iws.DataAccess.Repositories.Imports
{
    using Core.ImportNotificationAssessment;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using Domain.Security;
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.Domain.NotificationAssessment;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    internal class ImportNotificationAssessmentRepository : IImportNotificationAssessmentRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IwsContext iws_context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ImportNotificationAssessmentRepository(ImportNotificationContext context,
            IImportNotificationApplicationAuthorization authorization, 
            IwsContext iws_context)
        {
            this.context = context;
            this.authorization = authorization;
            this.iws_context = iws_context;
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

        public async Task<DateTime?> GetConsentedDate(Guid notificationId)
        {
            var assessment = await GetByNotification(notificationId);

            return assessment.Dates.ConsentedDate;
        }

        public async Task<ImportNotificationStatus> GetStatusByNotification(Guid notificationId)
        {
            return (await GetByNotification(notificationId)).Status;
        }

        public async Task<ImportNotificationStatusChange> GetPreviousStatusChangeByNotification(Guid notificationId)
        {
            var notification = await GetByNotification(notificationId);
            var result = notification.StatusChanges.OrderByDescending(x => x.ChangeDate).First();
            return result;
        }

        public async Task<List<ImportNotificationStatusChangeData>> GetUnderProhibitionHistory(Guid notificationId)
        {
            var notification = await GetByNotification(notificationId);

            var result = notification.StatusChanges
                .Where(x => x.NewStatus == ImportNotificationStatus.UnderProhibition || x.PreviousStatus == ImportNotificationStatus.UnderProhibition)
                .OrderByDescending(x => x.ChangeDate)
                .Select(sc => new ImportNotificationStatusChangeData
                {
                    Status = sc.NewStatus,
                    ChangeDate = sc.ChangeDate.Date,
                    UserId = sc.UserId
                })
                .ToList();

            foreach (var statusChange in result)
            {
                statusChange.FullName = await iws_context.Users.Where(u => u.Id == statusChange.UserId).Select(x => x.FirstName + " " + x.Surname).SingleOrDefaultAsync();
            }

            return result;
        }
    }
}