namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;
    using Domain.Security;

    internal class NotificationAssessmentRepository : INotificationAssessmentRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization authorization;

        public NotificationAssessmentRepository(IwsContext context, INotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<NotificationAssessment> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.NotificationAssessments.SingleAsync(p => p.NotificationApplicationId == notificationId);
        }

        public async Task<string> GetNumberForAssessment(Guid notificationAssessmentId)
        {
            return await context.NotificationAssessments.Join(context.NotificationApplications,
               assess => assess.NotificationApplicationId,
               n => n.Id,
               (a, n) => new { Number = n.NotificationNumber, AssessmentId = a.Id })
               .Where(r => r.AssessmentId == notificationAssessmentId)
               .Select(r => r.Number)
               .SingleAsync();
        }

        public async Task<NotificationStatus> GetStatusByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);

            return
                await
                    context.NotificationAssessments.Where(n => n.NotificationApplicationId == notificationId)
                        .Select(n => n.Status)
                        .SingleAsync();
        }
    }
}