namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class ConsentNotificationApplication : IRequest<bool>
    {
        public Guid NotificationId { get; set; }

        public DateTime ConsentFrom { get; set; }

        public DateTime ConsentTo { get; set; }

        public string ConsentConditions { get; set; }

        public DateTime ConsentedDate { get; set; }

        public ConsentNotificationApplication(Guid notificationId,
            DateTime consentFrom,
            DateTime consentTo,
            DateTime consentedDate,
            string consentConditions)
        {
            ConsentedDate = consentedDate;
            NotificationId = notificationId;
            ConsentFrom = consentFrom;
            ConsentTo = consentTo;
            ConsentConditions = consentConditions;
        }
    }
}