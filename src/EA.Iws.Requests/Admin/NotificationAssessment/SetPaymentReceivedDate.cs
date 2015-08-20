namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class SetPaymentReceivedDate : IRequest<bool>
    {
        public SetPaymentReceivedDate(Guid notificationId, DateTime paymentReceivedDate)
        {
            NotificationId = notificationId;
            PaymentReceivedDate = paymentReceivedDate;
        }

        public Guid NotificationId { get; private set; }

        public DateTime PaymentReceivedDate { get; private set; }
    }
}