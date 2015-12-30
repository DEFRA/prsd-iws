namespace EA.Iws.Core.ImportNotificationAssessment
{
    using System;

    public class KeyDatesData
    {
        public DateTime? NotificationReceived { get; set; }

        public DateTime? PaymentReceived { get; set; }

        public bool IsPaymentComplete { get; set; }
    }
}
