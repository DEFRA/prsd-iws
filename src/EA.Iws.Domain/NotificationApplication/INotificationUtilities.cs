namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Threading.Tasks;

    public interface INotificationUtilities
    {
        Task<bool> ShouldDisplayShipmentSelfEnterDataQuestion(Guid notificationId);
    }
}