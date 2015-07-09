namespace EA.Iws.Domain.Notification
{
    using System;
    using System.Threading.Tasks;

    public interface INotificationChargeCalculator
    {
        Task<decimal> GetValue(Guid notificationId);
    }
}