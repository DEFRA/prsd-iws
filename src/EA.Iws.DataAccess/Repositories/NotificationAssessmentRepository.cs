namespace EA.Iws.DataAccess.Repositories
{
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;
    using Domain.Security;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

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

        public async Task<NotificationStatusChange> GetPreviousStatusChangeByNotification(Guid notificationId)
        {
            var notification = await GetByNotificationId(notificationId);
            var result = notification.StatusChanges.OrderByDescending(x => x.ChangeDate).FirstOrDefault();
            return result;
        }

        public async Task<NotificationStatus> GetPreviousStatusByNotification(Guid notificationId)
        {
            var notification = await GetByNotificationId(notificationId);
            var result = notification.StatusChanges.OrderByDescending(x => x.ChangeDate).Skip(1).FirstOrDefault();
            return result.Status;
        }

        public async Task<List<NotificationStatusChangeData>> GetUnderProhibitionHistory(Guid notificationId)
        {
            var notification = await GetByNotificationId(notificationId);

            var result = notification.StatusChanges
                .Where(x => x.Status == NotificationStatus.UnderProhibition ||
                    notification.StatusChanges.Where(p => p.ChangeDate < x.ChangeDate)
                    .OrderByDescending(p => p.ChangeDate)
                    .Select(p => p.Status)
                    .FirstOrDefault() == NotificationStatus.UnderProhibition)
                .OrderByDescending(x => x.ChangeDate)
                .Select(sc => new NotificationStatusChangeData
                {
                    Status = sc.Status,
                    ChangeDate = sc.ChangeDate.Date,
                    UserId = sc.UserId
                })
                .ToList();

            foreach (var statusChange in result)
            {
                statusChange.FullName = await context.Users.Where(u => u.Id == statusChange.UserId).Select(x => x.FirstName + " " + x.Surname).SingleOrDefaultAsync();
            }

            return result;
        }

        public async Task<DateTime?> GetSubmitedDate(Guid notificationId)
        {
            var assessment = await GetByNotificationId(notificationId);
            var submittedStatuses = new List<NotificationStatus> { NotificationStatus.Submitted, NotificationStatus.Resubmitted };
            var submittedStatusChange = assessment.StatusChanges.Where(x => submittedStatuses.Contains(x.Status))
                .OrderByDescending(x => x.ChangeDate)
                .FirstOrDefault();

            return submittedStatusChange?.ChangeDate.UtcDateTime;
        }
    }
}