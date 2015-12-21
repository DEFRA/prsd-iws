namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;

    public interface INotificationTransactionCalculator
    {
        decimal TotalCredits(IList<NotificationTransaction> transactions);

        decimal TotalDebits(IList<NotificationTransaction> transactions);

        Task<decimal> Balance(Guid notificationId);

        Task<bool> IsPaymentComplete(Guid notificationId);

        Task<NotificationTransaction> LatestPayment(Guid notificationId);

        Task<bool> PaymentIsNowFullyReceived(NotificationTransactionData data);

        Task<decimal> RefundLimit(Guid notificationId);

        Task<decimal> TotalPaid(Guid notificationId);
    }
}