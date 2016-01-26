namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;

    public interface INotificationTransactionCalculator
    {
        Task<decimal> Balance(Guid notificationId);

        Task<bool> IsPaymentComplete(Guid notificationId);

        Task<NotificationTransaction> LatestPayment(Guid notificationId);

        Task<bool> PaymentIsNowFullyReceived(NotificationTransactionData data);

        Task<decimal> RefundLimit(Guid notificationId);

        Task<decimal> TotalPaid(Guid notificationId);
    }
}