namespace EA.Iws.Requests.NotificationAssessment.Payment
{
    using System;

    public class ProcessGovUkPaymentReturnResult
    {
        public Guid NotificationId { get; set; }

        public bool Success { get; set; }

        public string PaymentReference { get; set; }
    }
}