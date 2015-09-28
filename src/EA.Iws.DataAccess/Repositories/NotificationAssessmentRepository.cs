namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
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
    }
}