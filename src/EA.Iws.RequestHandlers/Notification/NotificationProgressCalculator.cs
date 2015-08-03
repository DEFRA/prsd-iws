namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Linq;
    using DataAccess;
    using Domain.NotificationApplication;

    internal class NotificationProgressCalculator : INotificationProgressCalculator
    {
        private readonly IwsContext context;

        public NotificationProgressCalculator(IwsContext context)
        {
            this.context = context;
        }

        public bool IsComplete(Guid notificationId)
        {
            var notification = context.NotificationApplications.Single(p => p.Id == notificationId);

            var progress = new NotificationProgress(notification);

            return progress.IsAllCompleted();
        }
    }
}