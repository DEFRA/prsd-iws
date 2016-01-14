namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Threading.Tasks;

    public interface INotificationUserService
    {
        Task ChangeNotificationUser(Guid notificationId, Guid newUserId);
    }
}
