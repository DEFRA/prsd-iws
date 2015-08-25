namespace EA.Iws.Domain
{
    using System;

    public interface INotificationMovementService
    {
        int GetNextMovementNumber(Guid notificationId);
        bool DateIsValid(Guid notificationId, DateTime date);
    }
}
