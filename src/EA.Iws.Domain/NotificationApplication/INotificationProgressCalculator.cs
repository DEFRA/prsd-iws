namespace EA.Iws.Domain.NotificationApplication
{
    using System;

    public interface INotificationProgressCalculator
    {
        bool IsComplete(Guid notificationId);
    }
}