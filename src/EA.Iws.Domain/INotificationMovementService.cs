namespace EA.Iws.Domain
{
    using System;

    public interface INotificationMovementService
    {
        bool CanCreateNewMovementForNotification(Guid notificationId);
        int GetNextMovementNumber(Guid notificationApplicationId);
        bool DateIsValid(Guid notificationId, DateTime date);
    }
}
