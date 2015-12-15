namespace EA.Iws.Domain.NotificationAssessment
{
    using System;

    public class NotificationAttentionSummary
    {
        public Guid NotificationId { get; private set; }

        public string NotificationNumber { get; private set; }

        public string Officer { get; private set; }

        public DateTime AcknowledgedDate { get; private set; }

        public static NotificationAttentionSummary Load(Guid notificationId,
            string notificationNumber,
            string officer,
            DateTime acknowledgedDate)
        {
            return new NotificationAttentionSummary
            {
                NotificationId = notificationId,
                NotificationNumber = notificationNumber,
                Officer = officer,
                AcknowledgedDate = acknowledgedDate,
            };
        }
    }
}