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
        Task<NotificationTransaction> LastestPayment(Guid notificationId);
        Task<bool> PaymentIsNowFullyReceived(NotificationTransactionData data);
    }
}