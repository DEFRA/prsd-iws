namespace EA.Iws.Core.NotificationAssessment
{
    using System;

    public class NotificationDatesData
    {
        public DateTime? NotificationReceivedDate { get; set; }

        public Guid NotificationId { get; set; }

        public DateTime? PaymentReceivedDate { get; set; }
    }
}