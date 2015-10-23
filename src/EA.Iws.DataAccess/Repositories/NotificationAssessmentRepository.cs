namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;

    internal class NotificationAssessmentRepository : INotificationAssessmentRepository
    {
        private readonly IwsContext context;

        public NotificationAssessmentRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<NotificationAssessment> GetByNotificationId(Guid notificationId)
        {
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
    }
}