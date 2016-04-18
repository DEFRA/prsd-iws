namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using Core.Notification;

    public interface INotificationProgressService
    {
        bool IsComplete(Guid notificationId);

        NotificationApplicationCompletionProgress GetNotificationProgressInfo(Guid notificationId);
    }
}