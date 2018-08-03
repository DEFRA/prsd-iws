namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;

    public interface INotificationTransactionCalculator
    {
        Task<decimal> Balance(Guid notificationId);

        Task<bool> IsPaymentComplete(Guid notificationId);

        Task<NotificationTransaction> LatestPayment(Guid notificationId);

        Task<decimal> RefundLimit(Guid notificationId);

        Task<decimal> TotalPaid(Guid notificationId);
    }
}